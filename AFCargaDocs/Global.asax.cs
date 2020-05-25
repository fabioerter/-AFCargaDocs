using AFCargaDocs.Controllers;
using AFCargaDocs.Models.Entidades;
using Microsoft.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace AFCargaDocs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            
            Response.Clear();

            HttpException httpException = exception as HttpException;

            int error = httpException != null ? httpException.GetHttpCode() : 0;
            if (!GlobalVariables.IsAjaxRequest(Request))
            {
                Server.ClearError();
                Response.Redirect(String.Format("~/Error/?message={0}&error={1}", exception.Message.Replace("\n", ""), error));
            }
            else
            {
                Server.ClearError();
                Response.StatusCode = error;

                Response.Write(
                    new JObject(
                        new JProperty("errNum", error),
                        new JProperty("message", exception.Message.Replace("\n", ""))
                        ).ToString() 
                    );
            }

        }
    }
}
