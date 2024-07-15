using FasceMVC.App_Start;
using FasceMVC.Area.Emisor.Models;
using log4net;
using Metodos;
using System;
using System.Web.Mvc;

namespace FasceMVC.Area.Emisor.Controllers
{
    public class NuevoDocumentoController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NuevoDocumentoController).Name);
        private readonly ClsEmisor _clsEmisor = new ClsEmisor();
        private readonly ClsAdquiriente _clsAdquiriente = new ClsAdquiriente();

        /// <summary>
        /// cargar pantalla inicial del nuevo documento
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new NuevoDocumento
            {
                ListadoReceptor = null
            };
            return View(model);
        }

        /// <summary>
        /// buscar adquirientes
        /// </summary>
        /// <param name="nuevoDocumento"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(NuevoDocumento nuevoDocumento)
        {
            try
            {
                if (nuevoDocumento.Nombre == null && nuevoDocumento.Identificacion == null)
                {
                    ModelState.AddModelError("Nombre", "Debe ingresar un filtro de busqueda");
                }
                else
                {
                    var adquiriente = _clsAdquiriente.ListadoAdquiriente(nuevoDocumento.Nombre, nuevoDocumento.Identificacion);

                    nuevoDocumento.ListadoReceptor = adquiriente;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return View(nuevoDocumento);
        }
    }
}