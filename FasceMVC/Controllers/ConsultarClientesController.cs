using FasceMVC.App_Start;
using FasceMVC.Models;
using log4net;
using Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    public class ConsultarClientesController : BaseController
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(AdquirienteController).Name);
        private readonly ClsBuscar _clsBuscar = new ClsBuscar();
        // GET: ConsultarClientes
        public ActionResult Index(ConsultarClientes model)
        {

            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                model.ListadoReceptor = _clsBuscar.CargarListadoReceptoresBusqueda(emisor, model.Buscar ?? "");

                if (TempData["tempCrearReceptor"] is string mensajeOk)
                {
                    model.JsFuncion = mensajeOk;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult EditarReceptor(Guid idReceptor)
        {
            try
            {
                return RedirectToAction("CrearAdquiriente", "Adquiriente", new { idReceptor = idReceptor });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
    }
}