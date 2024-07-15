using System.Web.Mvc;

namespace FasceMVC.Area.Emisor
{
    public class EmisorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Emisor";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Emisor_default",
                "Emisor/{controller}/{action}/{id}",
                /*new { action = "Index", id = UrlParameter.Optional }*/
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}