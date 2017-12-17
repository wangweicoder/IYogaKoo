using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Manage",
            //    "Manage/{controller}/{action}/{id}",
            //    new { controller = "BackLogin", action = "Index" }
            //);
            context.MapRoute(
              name: "Manage",
              url: "Manage/{controller}/{action}",
              defaults: new { controller = "BackLogin", action = "Index" }
              );
        }
    }
}