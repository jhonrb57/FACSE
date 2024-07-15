using DataBase;
using FasceMVC.Code;
using FasceMVC.Models;
using log4net;
using Metodos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FasceMVC.Areas.Receptor.Controllers
{
    [VerifyReceptor]
    public class ReceptorInicioController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReceptorInicioController).Name);
        private readonly ClsDocumento _clsDocumento = new ClsDocumento();
        private readonly ClsTipo _clsTipo = new ClsTipo();
        private readonly int _RegistrosPorPagina = 50;

        public static List<sys_tipo_documento> ListadoComprobanteController;

        /// <summary>
        /// metodo principal de busqueda
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pagina = 1, string p = null, string sortOrder = null)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.FechaRecepcionSortParm = String.IsNullOrEmpty(sortOrder) ? "FechaRecepcion" : "";
            ViewBag.NombreSortParm = sortOrder == "Nombre" ? "Nombre_desc" : "Nombre";

            ViewBag.IdentificacionSortParm = sortOrder == "Identificacion" ? "Identificacion_desc" : "Identificacion";
            ViewBag.FechaEnvioSortParm = sortOrder == "FechaEnvio" ? "FechaEnvio_desc" : "FechaEnvio";
            ViewBag.TotalSortParm = sortOrder == "Total" ? "Total_desc" : "Total";
            ViewBag.NumeroSortParm = sortOrder == "Numero" ? "Numero_desc" : "Numero";

            try
            {
                int count = 0;
                var model = new adm_documento
                {
                    FechaInicial = DateTime.Now,
                    FechaFinal = DateTime.Now,
                    PaginaActual = pagina
                };

                if (p != null)
                {
                    if (Session["soFechaInicial"] != null)
                        model.FechaInicial = (DateTime)Session["soFechaInicial"];
                    else
                        model.FechaInicial = null;
                    if (Session["soFechaFinal"] != null)
                        model.FechaFinal = (DateTime)Session["soFechaFinal"];
                    else
                        model.FechaFinal = null;

                    if (Session["soIdentificacion"] != null)
                        model.Identificacion = (string)Session["soIdentificacion"];
                    if (Session["soNombre"] != null)
                        model.Nombre = (string)Session["soNombre"];

                    if (Session["soIdComprobante"] != null)
                        model.IdComprobante = (Guid)Session["soIdComprobante"];
                    if (Session["soNumero"] != null)
                        model.Numero = (int)Session["soNumero"];
                }
                else
                {
                    Session["soFechaInicial"] = model.FechaInicial;
                    Session["soFechaFinal"] = model.FechaFinal;
                }

                model.ListadoDocumento = _clsDocumento.ListadoDocumentoReceptor(model, pagina, _RegistrosPorPagina, ref count, (Guid)Session["i_id_receptor"], sortOrder);

                ViewBag.Total = model.ListadoDocumento.Sum(s => s.doc_valor_total);

                var _TotalPaginas = (int)Math.Ceiling((double)count / _RegistrosPorPagina);
                model.TotalPaginas = _TotalPaginas;
                model.TotalRegistros = count;

                var tipoComprobante = _clsTipo.ListadoTipocomprobante();
                ListadoComprobanteController = tipoComprobante;
                ViewBag.IdComprobante = new SelectList(ListadoComprobanteController, "tdo_id", "tdo_descripcion");

                return View(model);
            }
            catch (Exception ex)
            {
                var model = new adm_documento
                {
                    ListadoDocumento = null
                };

                var tipoComprobante = _clsTipo.ListadoTipocomprobante();
                ListadoComprobanteController = tipoComprobante;
                ViewBag.IdComprobante = new SelectList(ListadoComprobanteController, "tdo_id", "tdo_descripcion");

                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        /// <summary>
        /// buscar documentos filtrados
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(adm_documento documento, string _search, string _exportar)
        {
            try
            {
                ViewBag.sortOrder = null;
                ViewBag.FechaRecepcionSortParm = "FechaRecepcion";
                ViewBag.NombreSortParm = "Nombre";
                ViewBag.IdentificacionSortParm = "Identificacion";
                ViewBag.FechaEnvioSortParm = "FechaEnvio";
                ViewBag.TotalSortParm = "Total";
                ViewBag.NumeroSortParm = "Numero";

                if (_exportar == null)
                {
                    ModelState.Clear();
                    Session["soFechaInicial"] = documento.FechaInicial;
                    Session["soFechaFinal"] = documento.FechaFinal;
                    Session["soIdentificacion"] = documento.Identificacion;
                    Session["soNombre"] = documento.Nombre;
                    Session["soIdComprobante"] = documento.IdComprobante;
                    Session["soNumero"] = documento.Numero;

                    int count = 0;
                    documento.ListadoDocumento = _clsDocumento.ListadoDocumentoReceptor(documento, 1, _RegistrosPorPagina, ref count, (Guid)Session["i_id_receptor"], "");
                    ViewBag.Total = documento.ListadoDocumento.Sum(s => s.doc_valor_total);

                    documento.PaginaActual = 1;
                    var _TotalPaginas = (int)Math.Ceiling((double)count / _RegistrosPorPagina);
                    documento.TotalPaginas = _TotalPaginas;
                    documento.TotalRegistros = count;

                    ViewBag.IdComprobante = new SelectList(ListadoComprobanteController, "tdo_id", "tdo_descripcion", documento.IdComprobante);

                    return View(documento);
                }
                else
                {
                    Generador rp = new Generador();

                    Byte[] Report = rp.GetInicioReportReceptor(documento, (Guid)Session["i_id_receptor"]);

                    string sNombreArchivo = $"Inicio.xlsx";

                    return File(Report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sNombreArchivo);
                }
            }
            catch (Exception ex)
            {
                documento.ListadoDocumento = null;

                ViewBag.IdComprobante = new SelectList(ListadoComprobanteController, "tdo_id", "tdo_descripcion", documento.IdComprobante);

                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(documento);
            }
        }

        public ActionResult ExportarXml(string ruta)
        {
            try
            {
                //var savePath = $"C:\\Users\\WILLIAM-PC\\Documents\\0102Z3S000000000015122017.txt";
                var savePath = ruta;
                //_finacDO.GeneradorArchivo(finac.FechaInicial, finac.Fecha, finac.Mensual);

                return File(savePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Documento.xml");

            }
            catch (Exception ex)
            {
                log.Error($"Error al exportar xml: {ex.ToString()}");
                throw ex;
            }
        }



        public ActionResult ExportarPdf(Guid gDocumento)
        {
            try
            {
                Generador rp = new Generador();

                dynamic objectoRespuesta = ConsumirApiPDF(gDocumento);

                if ((bool)objectoRespuesta.Respuesta)
                {
                    string sNombreArchivo = $"Documento.pdf";


                    string reportBase64 = objectoRespuesta.PdfBase64;

                    var model = new ViewPdf
                    {
                        PdfBase64 = reportBase64,
                        Estado = _clsDocumento.ValidarEstadoDocumento(gDocumento),
                        IdDocumento = gDocumento
                    };

                    if (!model.Estado)
                    {
                        model.Alerta = "Sin aprobacion.";
                    }

                    if (TempData["sMensajePdf"] is string mensaje)
                    {
                        ViewBag.JsFuncion = mensaje;
                    }

                    return View("ViewPdf", model);
                }
                else
                {
                    return RedirectToAction("Error", "Error");
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public dynamic ConsumirApiPDF(Guid IdDocuemento)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api_pdf"].ToString() + IdDocuemento;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaApiCompleta);
                request.Method = "GET";
                request.ContentType = "application/json";

                using (HttpWebResponse respuesta = (HttpWebResponse)request.GetResponse())
                {
                    if (respuesta.StatusCode == HttpStatusCode.Accepted || respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.Created)
                    {
                        using (StreamReader read = new StreamReader(respuesta.GetResponseStream()))
                        {
                            var result = read.ReadToEnd();

                            dynamic resultObjeto = JsonConvert.DeserializeObject(result);
                            return resultObjeto;

                        };
                    }
                    else
                    {
                        return "Error al Generar el PDF";
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }

        public ActionResult ExportarAnexo(string sDirectorio, string sNombre, string sExtension)
        {
            try
            {
                //var savePath = $"C:\\Users\\WILLIAM-PC\\Documents\\0102Z3S000000000015122017.txt";
                var savePath = $"{sDirectorio}";
                //_finacDO.GeneradorArchivo(finac.FechaInicial, finac.Fecha, finac.Mensual);

                return File(savePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{sNombre}{sExtension}");

            }
            catch (Exception ex)
            {
                log.Error($"Error al exportar xml: {ex.ToString()}");
                throw ex;
            }
        }
    }
}