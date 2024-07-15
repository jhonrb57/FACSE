using FasceMVC.Code;
using log4net;
using Metodos;
using System;
using System.Web.Mvc;

namespace FasceMVC.App_Start
{
    [VerifyUser]
    public class BaseController : Controller
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(BaseController).Name);
        private readonly ClsSucursal _clsSucursal = new ClsSucursal();

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Session["i_id_usuario"] != null)
            {
                var sucursal = _clsSucursal.ListadoEmisorSucursal((Guid)Session["i_id_usuario"]);

                ViewBag.SreSucursal = new SelectList(sucursal, "esu_id", "esu_nombre", (Guid)Session["i_id_sucursal"]);
            }
        }

        /// <summary>
        /// cambio sucursal
        /// </summary>
        /// <param name="SreSucursal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CambiarSucursal(Guid SreSucursal)
        {
            _clsSucursal.CambioSucursalUsuario(SreSucursal, (Guid)Session["i_id_usuario"]);
            Session["i_id_sucursal"] = SreSucursal;
            return RedirectToAction("Index", "EmisorInicio");
        }


    }
}