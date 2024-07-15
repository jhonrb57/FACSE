using System;
using System.Web.Mvc;


namespace FasceMVC.Code
{
    public class VerifyReceptorAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var user = filterContext.HttpContext.Session.Contents["i_id_receptor"];
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (user == null)
                    {

                        filterContext.HttpContext.Response.StatusCode = 403;
                    }
                }
                else
                {
                    if (user == null)
                    {
                        filterContext.Result = new RedirectResult("~/LoginReceptor/Index/Se caduco la session");

                    }//RedirectToAction("Login/Se presento un inconveniente inesperado volver a intentar, si el incoveniente persiste comuniquese con el administrador", "Login", ModelState);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}