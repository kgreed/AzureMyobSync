using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

namespace AMS.Module.BusinessObjects
{

    public abstract class BasicBo : IXafEntityObject  //, IObjectSpaceLink  we should just declare it when we really need it... mainly we want out business objects to be like POCOs 
    {
        [Browsable(false)]
        [Key]
        public virtual int Id { get; set; }

        public virtual void OnCreated()
        {
         
        }

        public virtual void OnSaving()
        {
             
        }

        public virtual void OnLoaded()
        {
        }


    }
}
