using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using SBD.AMS.MYOB;

namespace SBD.AMS.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public class ProgressBarXeroLiistViewController : ViewController
    {
        private const string DisableNavigateToValidation = "Disable for Validation View";
        private SimpleAction actMYOBSync;
        private IContainer components;

        public ProgressBarXeroLiistViewController()
        {
            InitializeComponent();

            // Target required Views (via the TargetXXX properties) and create their Actions.
            //TargetObjectType = typeof(IHasMyobMenu);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            //Disable navigate to validation rule
            if (View.Id == "RuleSetValidationResultItem_ByTarget_ListView")
            {
                var c = Frame.GetController<ListViewProcessCurrentObjectController>();
                c.ProcessCurrentObjectAction.Enabled.SetItemValue(DisableNavigateToValidation, false);
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            if (View.Id == "RuleSetValidationResultItem_ByTarget_ListView")
            {
                var c = Frame.GetController<ListViewProcessCurrentObjectController>();
                c.ProcessCurrentObjectAction.Enabled.RemoveItem(DisableNavigateToValidation);
            }
        }

        private async void MyobSync(string login, List<MYOBTypeEnum> myobTypes)
        {
            var isPosting = myobTypes.Contains(MYOBTypeEnum.SalesInvoice) || myobTypes.Contains(MYOBTypeEnum.SalesOrder);
	        var caption = isPosting ? "Post" : "Import";
	         
	        InitProgressBar(caption);
            var cts = new CancellationTokenSource();
            var progress = new Progress<int>(UpdateProgressBar);
            var o = new MyobHandler();
            o.PrepareToCommunicateWithMyob(login);


            var num = await o.RunSyncs(cts.Token, progress, myobTypes);
            CleanUpProgressBar();
            View.ObjectSpace.Refresh();
        }

        private void CleanUpProgressBar()
        {
            var bar = ProgressBar();
            bar.Visibility = BarItemVisibility.OnlyInCustomizing;
        }

        private void UpdateProgressBar(int num)
        {
            try
            {
                var bar = ProgressBar();
                bar.EditValue = num;
                bar.Refresh();
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.ToString());

                //throw;
            }
        }

        private void InitProgressBar(string caption)
        {
            var bar = ProgressBar();
            bar.Caption = caption;
            bar.Visibility = BarItemVisibility.Always;
            bar.Refresh();
        }

        private BarEditItem ProgressBar()
        {
            var window = (WinWindow)Frame;
            var ct = window.Form.Controls.Find("ribbonStatusBar", true)[0] as RibbonStatusBar;
            var links = ct.ItemLinks;
            return (BarEditItem)links[2].Item;
        }

        private void InitializeComponent()
        {
            components = new Container();
            actMYOBSync = new SimpleAction(components);
            // 
            // actMYOBSync
            // 
            actMYOBSync.Caption = "Myob Sync";
            actMYOBSync.ConfirmationMessage = null;
            actMYOBSync.Id = "MyobSync";
            actMYOBSync.ImageName = "myob";
            actMYOBSync.ToolTip = null;
            actMYOBSync.Execute += new SimpleActionExecuteEventHandler(actMYOBSync_Execute_1);
            // 
            // ProgressBarMYOBListViewController
            // 
            Actions.Add(actMYOBSync);

        }

        

        private void actMYOBSync_Execute_1(object sender, SimpleActionExecuteEventArgs e)
        {
            var login = "Admin";
            var types = new List<MYOBTypeEnum>();
            if (View.Id.ToLower().Contains("salesinvoice"))
            {
                types.Add(MYOBTypeEnum.SalesInvoice);

            }
            else
            {

                if (View.Id.ToLower().Contains("salesorder"))
                {
                    types.Add(MYOBTypeEnum.SalesOrder);

                }
                else
                {
                    types.Add(MYOBTypeEnum.Customer);
                    types.Add(MYOBTypeEnum.Supplier);
                    types.Add(MYOBTypeEnum.TaxRate);
                    types.Add(MYOBTypeEnum.Inventory);
                    types.Add(MYOBTypeEnum.GeneralLedgerAccount);
                }
              
            }
            MyobSync(login, types);
        }
    }
}