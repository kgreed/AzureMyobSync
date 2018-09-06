using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using AMS.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;

namespace AMS.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            if(Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder) {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            AMSWindowsFormsApplication winApplication = new AMSWindowsFormsApplication();
            // Refer to the https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112680.aspx help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
            SecurityAdapterHelper.Enable();



            winApplication.CreateCustomTemplate += delegate (object sender, CreateCustomTemplateEventArgs e)
            {
                if (e.Context == TemplateContext.View)
                {
                    e.Template = new CustomDetailRibbonForm();
                }
            };


            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try {
                winApplication.Setup();
                winApplication.Start();
            }
            catch(Exception e) {
                winApplication.HandleException(e);
            }
        }

        private static bool CheckMigrationVersionAndUpgradeIfNeeded()
        {
            try
            {
                //using (var db = new AMSDbContext())
                //{
                //    var config = db.ConfigurationSettings.FirstOrDefault();
                //    if (config == null)
                //    { Debug.Print("There is not config record");}
                //    else
                //    { Debug.Print(config.Name); }
                //}

                using (var db = new AMSDbContext())
                {
                    if (!db.Database.Exists())
                    {
                        db.Database.Initialize(true);
                        //db.Database.Create();
                        // new DatabaseInitializer().Seed(this);
                    }


                    var hasMetaData = true;
                    try
                    {
                        var compatible = db.Database.CompatibleWithModel(true);
                    }
                    catch (Exception)
                    {
                        hasMetaData = false;
                    }
                    if (!hasMetaData)
                    {
                        return RunMigrations(db);
                    }
                    if (!db.Database.CompatibleWithModel(false))
                    {
                        return RunMigrations(db);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                var s = "Problem in MigrateIfNeeded /n/p" + ex;
                MessageBox.Show(s);
                return false;
            }
        }

        private static bool RunMigrations(AMSDbContext db)
        {
            var result = AskWhetherToUpgrade();
            if (!result)
            {
                return false;
            }
            try
            {
                //http://stackoverflow.com/questions/34699283/ef6-1-3-expects-createdon-field-in-migrationhistory-table-when-database-setiniti
                //MessageBox.Show(
                //	"You might need to the following SQL First /n/p " +
                //	"ALTER TABLE dbo.__MigrationHistory ADD CreatedOn DateTime Default GETDATE() /n/p" + "GO /n/p" +
                //	"UPDATE dbo.__MigrationHistory SET CreatedOn = GETDATE()");
                var num = db.RunMigrations();
                MessageBox.Show($"{num} migrations have been run");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private static bool AskWhetherToUpgrade()
        {
            var msg = new StringBuilder();
            msg.AppendLine("The database structure has changed.");
            msg.AppendLine("Do you want to upgrade the database structure.");
            msg.AppendLine("Call support if you are unsure) ");
            var result = (MessageBox.Show(msg.ToString(), "Upgrade database", MessageBoxButtons.YesNo) != DialogResult.Yes);
            return result;

        }

    }
}
