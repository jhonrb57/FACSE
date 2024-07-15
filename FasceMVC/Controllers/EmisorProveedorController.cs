using DataBase;
using FasceMVC.App_Start;
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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    public class EmisorProveedorController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmisorProveedorController).Name);
        private readonly ClsTipo _clsTipo = new ClsTipo();
        private readonly ClsDocumentoProveedor _clsDocumentoProveedor = new ClsDocumentoProveedor();

        /// <summary>
        /// metodo principal de busqueda
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                Session["spFechaInicial"] = null;
                Session["spFechaFinal"] = null;
                Session["spIdentificacion"] = null;
                Session["spNombre"] = null;
                Session["spIdComprobante"] = null;
                Session["spIdEstado"] = null;
                Session["spIdEstadoFacse"] = null;
                Session["spNumero"] = null;

                var model = new adm_documento_proveedor
                {
                    FechaInicial = DateTime.Now,
                    FechaFinal = DateTime.Now
                };

                var tipoComprobante = _clsTipo.ListadoTipocomprobante();
                Session["ListadoComprobanteProController"] = tipoComprobante;
                ViewBag.IdComprobante = new SelectList((List<sys_tipo_documento>)Session["ListadoComprobanteProController"], "tdo_id", "tdo_descripcion");

                var tipoEstadoFacse = _clsTipo.ListadoTipoEstadoFacse();
                Session["ListadoEstadoFacseProController"] = tipoEstadoFacse;
                ViewBag.IdEstadoFacse = new SelectList((List<sys_estado_documento_facse>)Session["ListadoEstadoFacseProController"], "edf_id", "edf_descripcion");

                return View(model);

            }
            catch (Exception ex)
            {
                var model = new adm_documento_proveedor();

                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        public JsonResult ListadoGrillaDatatable()
        {
            //int pageIndex = Convert.ToInt32(page) - 1;
            //int pageSize = rows;
            //int startRows = rows * (pageIndex);

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            try
            {
                DateTime dFechaInicio, dFechaFinal;
                string sIdentificacion = "", sNombre = "";
                Guid? gIdComprobante = null, gIdEstadoFacse = null;
                int? iNumero = null;

                if (Session["spFechaInicial"] != null)
                    dFechaInicio = (DateTime)Session["spFechaInicial"];
                else
                {
                    dFechaInicio = DateTime.Now;
                    Session["spFechaInicial"] = dFechaInicio;
                }
                if (Session["spFechaFinal"] != null)
                    dFechaFinal = (DateTime)Session["spFechaFinal"];
                else
                {
                    dFechaFinal = DateTime.Now;
                    Session["spFechaFinal"] = dFechaFinal;
                }

                if (Session["spIdentificacion"] != null)
                    sIdentificacion = (string)Session["spIdentificacion"];
                if (Session["spNombre"] != null)
                    sNombre = (string)Session["spNombre"];

                if (Session["spIdComprobante"] != null)
                    gIdComprobante = (Guid)Session["spIdComprobante"];
                if (Session["spNumero"] != null)
                    iNumero = (int)Session["spNumero"];

                if (Session["spIdEstadoFacse"] != null)
                    gIdEstadoFacse = (Guid)Session["spIdEstadoFacse"];

                int count = 0;

                var listadoDocumento = _clsDocumentoProveedor.ListadoDocumento(dFechaInicio, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, sortColumn, sortColumnDir, pageSize, skip, ref count, (Guid)Session["i_id_sucursal"], gIdEstadoFacse);

                int totalRecords = count;
                //var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                var jsonData = (from d in listadoDocumento
                                select new
                                {
                                    d.dpr_id,
                                    Tipo = d.tdo_abreviatura.ToUpper(),
                                    d.dpr_prefijo,
                                    d.dpr_numero,
                                    Nombre = d.pro_nombre,
                                    Identificacion = d.pro_identificacion,
                                    dpr_fecha_documento = d.dpr_fecha_documento.ToString("dd/MM/yyyy"),
                                    dpr_fecha_acuse = d.dpr_fecha_acuse?.ToString("dd/MM/yyyy hh:mm:ss"),
                                    dpr_fecha_envio = d.dpr_fecha_envio.ToString("dd/MM/yyyy hh:mm:ss"),
                                    dpr_fecha_recibido = d.dpr_fecha_recibido.ToString("dd/MM/yyyy hh:mm:ss"),
                                    d.dpr_valor_total,
                                    d.dpr_usuario,
                                    Xml = d.dpf_xml,
                                    d.edf_ruta_imagen,
                                    d.dpr_acuse,
                                    d.edf_codigo,
                                    d.anexo,
                                    d.edf_descripcion,
                                    d.dpf_pdf
                                }).ToList();

                return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = jsonData });
            }
            catch (Exception ex)
            {
                var model = new adm_documento_proveedor();

                log.Error($"Error al buscar informacion: {ex}");
                ModelState.AddModelError("", "Error al buscar informacion. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(new { isValid = false });
        }

        /// <summary>
        /// convierte vista en string
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            }
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// buscar documentos filtrados
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ListadoBuscar")]
        [ValidateAntiForgeryToken]
        public ActionResult ListadoBuscar(adm_documento_proveedor documentoProveedor, string _search, string _exportar)
        {
            try
            {
                if (_exportar == null)
                {
                    if (documentoProveedor.FechaInicial == null)
                    {
                        documentoProveedor.FechaInicial = DateTime.Now;
                    }
                    if (documentoProveedor.FechaFinal == null)
                    {
                        documentoProveedor.FechaFinal = DateTime.Now;
                    }
                    Session["spFechaInicial"] = documentoProveedor.FechaInicial;
                    Session["spFechaFinal"] = documentoProveedor.FechaFinal;
                    Session["spIdentificacion"] = documentoProveedor.Identificacion;
                    Session["spNombre"] = documentoProveedor.Nombre;
                    Session["spIdComprobante"] = documentoProveedor.IdComprobante;
                    Session["spIdEstadoFacse"] = documentoProveedor.IdEstadoFacse;
                    Session["spNumero"] = documentoProveedor.Numero;

                    ModelState.Clear();

                    ViewBag.IdComprobante = new SelectList((List<sys_tipo_documento>)Session["ListadoComprobanteProController"], "tdo_id", "tdo_descripcion", documentoProveedor.IdComprobante);
                    ViewBag.IdEstadoFacse = new SelectList((List<sys_estado_documento_facse>)Session["ListadoEstadoFacseProController"], "edf_id", "edf_descripcion", documentoProveedor.IdEstadoFacse);
                }
                else
                {
                    Generador rp = new Generador();

                    DateTime dFechaInicio, dFechaFinal;
                    string sIdentificacion = "", sNombre = "";
                    Guid? gIdComprobante = null, gIdEstadoFacse = null;
                    int? iNumero = null;

                    if (Session["spFechaInicial"] != null)
                        dFechaInicio = (DateTime)Session["spFechaInicial"];
                    else
                    {
                        dFechaInicio = DateTime.Now;
                        Session["spFechaInicial"] = dFechaInicio;
                    }
                    if (Session["spFechaFinal"] != null)
                        dFechaFinal = (DateTime)Session["spFechaFinal"];
                    else
                    {
                        dFechaFinal = DateTime.Now;
                        Session["spFechaFinal"] = dFechaFinal;
                    }

                    if (Session["spIdentificacion"] != null)
                        sIdentificacion = (string)Session["spIdentificacion"];
                    if (Session["spNombre"] != null)
                        sNombre = (string)Session["spNombre"];

                    if (Session["spIdComprobante"] != null)
                        gIdComprobante = (Guid)Session["spIdComprobante"];
                    if (Session["spNumero"] != null)
                        iNumero = (int)Session["spNumero"];

                    if (Session["spIdEstadoFacse"] != null)
                        gIdEstadoFacse = (Guid)Session["spIdEstadoFacse"];

                    Byte[] Report = rp.GetInicioReportProveedor(dFechaInicio, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, (Guid)Session["i_id_sucursal"], gIdEstadoFacse);

                    string sNombreArchivo = $"Proveedor.xlsx";

                    if (!(Directory.Exists(Path.Combine(Server.MapPath("~/temp")))))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/temp")));
                    }
                    string fullPath = Path.Combine(Server.MapPath("~/temp"), sNombreArchivo);


                    MemoryStream theMemStream = new MemoryStream();

                    theMemStream.Write(Report, 0, Report.Length);

                    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    theMemStream.WriteTo(file);
                    file.Close();

                    return Json(new { archivo = true, pan = sNombreArchivo });
                    //return File(Report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sNombreArchivo);
                }
            }
            catch (Exception ex)
            {

                ViewBag.IdComprobante = new SelectList((List<sys_tipo_documento>)Session["ListadoComprobanteProController"], "tdo_id", "tdo_descripcion", documentoProveedor.IdComprobante);
                ViewBag.IdEstadoFacse = new SelectList((List<sys_estado_documento_facse>)Session["ListadoEstadoFacseProController"], "edf_id", "edf_descripcion", documentoProveedor.IdEstadoFacse);

                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(new { archivo = false, pan = RenderPartialViewToString(this, "Index", documentoProveedor) });
        }

        /// <summary>
        /// nuevo documento
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("AdicionarNuevoDocumento")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarNuevoDocumento()
        {
            Session["eListadoProveedorAnexoAdjunto"] = null;
            return PartialView("AdicionarNuevo");
        }


        [HttpGet]
        [DeleteFileAttribute] //Action Filter, it will auto delete the file after download, 
                              //I will explain it later
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/temp"), file);

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
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

        /// <summary>
        /// metodos para aceptar o rechazar el documento
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="validacion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AceptadoRechazo(Guid idDocumento, bool validacion)
        {
            try
            {
                var (res, mensaje) = ConsumirApiGet($"{ConfigurationManager.AppSettings["CambioEstadoDianProveedor"].ToString()}{idDocumento}/{validacion}");

                if (Boolean.Parse(res))
                {
                    return Json(new { isValid = true });
                }
                else
                {
                    log.Error(mensaje);
                    return Json(new { isValid = false, mensaje });
                }
                //_clsDocumentoProveedor.AceptadoRechazo(idDocumento, validacion);
            }
            catch (Exception ex)
            {
                log.Error($"Error al aceptar o rechazar: {ex.ToString()}");
                return Json(new { isValid = false, mensaje = $"Error al aceptar o rechazar: { ex.ToString()}" });
            }
        }
        public JsonResult SubListadoGrilla(Guid id)
        {
            try
            {

                var listadoEstado = _clsDocumentoProveedor.ListadoEstadoFacse(id);

                var jsonData = (from d in listadoEstado
                                select new
                                {
                                    d.sys_estado_documento_facse.edf_descripcion,
                                    dpe_fecha = d.dpe_fecha?.ToString("d/MM/yyyy")
                                }).ToList();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var model = new adm_documento
                {
                    ListadoDocumento = null
                };

                log.Error($"Error al buscar informacion: {ex}");
                ModelState.AddModelError("", "Error al buscar informacion. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(new { isValid = false });
        }

        public JsonResult SubListadoGrillaAnexo(Guid id)
        {
            try
            {

                var listadoAnexo = _clsDocumentoProveedor.ListadoAnexo(id);

                var jsonData = (from d in listadoAnexo
                                select new
                                {
                                    d.dpa_nombre,
                                    d.dpa_directorio
                                }).ToList();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var model = new adm_documento
                {
                    ListadoDocumento = null
                };

                log.Error($"Error al buscar informacion: {ex}");
                ModelState.AddModelError("", "Error al buscar informacion. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(new { isValid = false });
        }

        /// <summary>
        /// Reenvio de la dian
        /// </summary>
        /// <param name="listadoReenviado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReenvioAcuse(List<adm_documento_proveedor> listadoDocumentoProveedor)
        {
            try
            {
                foreach (var item in listadoDocumentoProveedor)
                {
                    //poner a juan a validar eso
                    var (res, mensaje) = ConsumirApiGet($"{ConfigurationManager.AppSettings["AcuseReciboProveedor"].ToString()}{item.dpr_id}");
                }

                return Json(true);

            }
            catch (Exception ex)
            {
                log.Error($"Error al exportar Reenvio Documento: {ex.ToString()}");
                throw ex; ;
            }
        }

        /// <summary>
        /// Reenvio de la dian
        /// </summary>
        /// <param name="listadoReenviado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReenvioRecibido(List<adm_documento_proveedor> listadoDocumentoProveedor)
        {
            try
            {
                foreach (var item in listadoDocumentoProveedor)
                {
                    var (res, mensaje) = ConsumirApiGet($"{ConfigurationManager.AppSettings["CambioEstadoDianProveedor"].ToString()}{item.dpr_id}/{true}");

                    //_clsDocumentoProveedor.AceptadoRechazo(item.dpr_id, true);

                    //var Rutajson = _clsDocumento.ArchivoDocumento(item.doc_id);

                    //ConsumirApi("Comprobante", Rutajson.dfi_json_facse);
                }

                return Json(true);
            }
            catch (Exception ex)
            {
                log.Error($"Error al exportar Reenvio Documento: {ex.ToString()}");
                throw ex; ;
            }
        }

        /// <summary>
        /// Reenvio de la dian
        /// </summary>
        /// <param name="listadoReenviado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReenvioRechazado(List<adm_documento_proveedor> listadoDocumentoProveedor)
        {
            try
            {
                foreach (var item in listadoDocumentoProveedor)
                {
                    var (res, mensaje) = ConsumirApiGet($"{ConfigurationManager.AppSettings["CambioEstadoDianProveedor"].ToString()}{item.dpr_id}/{false}");

                    //_clsDocumentoProveedor.AceptadoRechazo(item.dpr_id, true);

                    //var Rutajson = _clsDocumento.ArchivoDocumento(item.doc_id);

                    //ConsumirApi("Comprobante", Rutajson.dfi_json_facse);
                }

                return Json(true);

            }
            catch (Exception ex)
            {
                log.Error($"Error al exportar Reenvio Documento: {ex.ToString()}");
                throw ex; ;
            }
        }

        public ActionResult ExportarPdf(Guid gDocumento, string sRuta)
        {
            try
            {

                Session["sProListadoAnexos" + gDocumento] = null;
                Session["sProListadoAnexosAdjuntos" + gDocumento] = null;
                Generador rp = new Generador();

                var model = new ViewPdf
                {
                    RutaPdf = sRuta,
                    IdDocumento = gDocumento
                };

                Session["sProListadoAnexos" + gDocumento] = _clsDocumentoProveedor.ListadoAnexo(gDocumento);

                if (Session["sProListadoAnexos" + gDocumento] != null)
                {
                    ViewBag.ListadoAnexos = Session["sProListadoAnexos" + gDocumento] as List<adm_documento_proveedor_anexo>;
                }

                return View("ViewPdf", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EliminarAnexoProveedor(Guid id, Guid idDocumento)
        {
            try
            {
                var listadoAnexos = Session["sProListadoAnexos" + idDocumento] as List<adm_documento_proveedor_anexo>;

                var eliminarAnexo = listadoAnexos.Where(x => x.dpa_id == id).FirstOrDefault();

                listadoAnexos.Remove(eliminarAnexo);

                Session["sProListadoAnexos" + idDocumento] = listadoAnexos;

                var (res, mensaje) = ConsumirApiGet($"{ConfigurationManager.AppSettings["ruta_api_proveedor_elianexo"].ToString()}{id}");

                string html = "";
                if (Convert.ToBoolean(res))
                {

                    foreach (var item in listadoAnexos)
                    {
                        html += $"<tr>";
                        html += $"<td>{item.dpa_nombre}{item.dpa_extension}</td>";
                        html += $"<td width='40'>";
                        html += $"<a onclick=\"eliminarAnexo('{item.dpa_id}','{idDocumento}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
                        html += $"</td>";
                        html += $"</tr>";
                    }
                }


                return Json(html, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult GuardarArchivosAdjuntos(ViewPdf viewPdf)
        {
            try
            {
                var listadoAnexosAdjuntos = Session["sProListadoAnexosAdjuntos" + viewPdf.IdDocumento] as List<dynamic>;

                var listadoAdjuntos = new List<dynamic>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase fileup = Request.Files[i];

                    byte[] bytes;

                    using (var memory = new MemoryStream())
                    {
                        fileup.InputStream.CopyTo(memory);
                        bytes = memory.ToArray();
                    }

                    //if (bytes.Length > 0)
                    //{
                    var newFile = new
                    {
                        ID = ((listadoAnexosAdjuntos != null) ? listadoAnexosAdjuntos.Max(x => x.ID) : 0) + 1 + i,
                        NombreCompleto = fileup.FileName,
                        Base64 = Convert.ToBase64String(bytes)
                    };
                    listadoAdjuntos.Add(newFile);
                    //}
                }

                if (listadoAnexosAdjuntos != null && listadoAnexosAdjuntos.Any())
                {
                    listadoAdjuntos = listadoAdjuntos.Union(listadoAnexosAdjuntos).ToList();
                }

                string html = "";
                foreach (var item in listadoAdjuntos)
                {
                    html += $"<tr>";
                    html += $"<td>{item.NombreCompleto}</td>";
                    html += $"<td width='40'>";
                    html += $"<a onclick=\"eliminarAnexoAdjunto('{item.ID}','{viewPdf.IdDocumento}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
                    html += $"</td>";
                    html += $"</tr>";
                }

                Session["sProListadoAnexosAdjuntos" + viewPdf.IdDocumento] = listadoAdjuntos;

                return Json(html, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult eliminarAnexoAdjunto(int id, Guid idDocumento)
        {
            try
            {
                var listadoAnexosAdjuntos = Session["sProListadoAnexosAdjuntos" + idDocumento] as List<dynamic>;

                var eliminarAdjunto = listadoAnexosAdjuntos.Where(x => x.ID == id).FirstOrDefault();

                listadoAnexosAdjuntos.Remove(eliminarAdjunto);

                Session["sProListadoAnexosAdjuntos" + idDocumento] = listadoAnexosAdjuntos;


                string html = "";
                foreach (var item in listadoAnexosAdjuntos)
                {
                    html += $"<tr>";
                    html += $"<td>{item.NombreCompleto}</td>";
                    html += $"<td width='40'>";
                    html += $"<a onclick=\"eliminarAnexoAdjunto('{item.ID}','{idDocumento}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
                    html += $"</td>";
                    html += $"</tr>";
                }

                return Json(html, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult EnviarAdjuntoPDF(ViewPdf viewPdf)
        {
            try
            {
                var idSucursal = (Guid)Session["i_id_sucursal"];
                var listadoAnexosAdjuntos = Session["sProListadoAnexosAdjuntos" + viewPdf.IdDocumento] as List<dynamic>;
                var listadoAdjuntosEnviar = new List<dynamic>();

                if (listadoAnexosAdjuntos != null && listadoAnexosAdjuntos.Any())
                {
                    foreach (var item in listadoAnexosAdjuntos)
                    {
                        var newAdjunto = new
                        {
                            item.NombreCompleto,
                            item.Base64
                        };

                        var json = new
                        {
                            IdDocumento = viewPdf.IdDocumento.ToString(),
                            item.NombreCompleto,
                            AnexoBase64 = item.Base64
                        };

                        try
                        {
                            ConsumirApi(JsonConvert.SerializeObject(json), ConfigurationManager.AppSettings["ruta_api_proveedor_anexo"].ToString());

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.ToString());
                        }
                    }
                }

                string icon = "success";
                string mensaje = "Se envio con exito";
                TempData["sMensajePdf"] = $"mensaje(\"{mensaje}\",'{icon}')";

                return RedirectToAction("ExportarPdf", new { gDocumento = viewPdf.IdDocumento, sRuta = viewPdf.RutaPdf });


            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public Stream ObtenerArchivoZip(string url)
        {
            try
            {
                WebRequest zip = WebRequest.Create(url);
                WebResponse response = zip.GetResponse();

                Stream stream = response.GetResponseStream();
                //FileStreamResult fsr = new FileStreamResult(response.GetResponseStream(), response.ContentType);
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public ActionResult GuardarArchivosAnexo()
        {
            try
            {
                var listadoAnexosAdjuntos = Session["eListadoProveedorAnexoAdjunto"] as List<dynamic>;

                var listadoAdjuntos = new List<dynamic>();

                if (Request.Files.Count == 0)
                {
                    return Json(new { isValid = false });
                }

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase fileup = Request.Files[i];

                    byte[] bytes;

                    using (var memory = new MemoryStream())
                    {
                        fileup.InputStream.CopyTo(memory);
                        bytes = memory.ToArray();
                    }

                    //if (bytes.Length > 0)
                    //{
                    var newFile = new
                    {
                        ID = Guid.NewGuid(),
                        NombreCompleto = fileup.FileName,
                        Base64 = Convert.ToBase64String(bytes)
                    };
                    listadoAdjuntos.Add(newFile);
                    //}
                }

                if (listadoAnexosAdjuntos != null && listadoAnexosAdjuntos.Any())
                {
                    listadoAdjuntos = listadoAdjuntos.Union(listadoAnexosAdjuntos).ToList();
                }

                string html = "";
                foreach (var item in listadoAdjuntos)
                {
                    html += $"<tr>";
                    html += $"<td>{item.NombreCompleto}</td>";
                    html += $"<td width='40'>";
                    html += $"<a onclick=\"EliminarAnexoProveedorAdjunto('{item.ID}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
                    html += $"</td>";
                    html += $"</tr>";
                }

                Session["eListadoProveedorAnexoAdjunto"] = listadoAdjuntos;

                return Json(new { isValid = true, html });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult EliminarAnexoProveedorAdjunto(Guid id)
        {
            try
            {
                var listadoAnexosAdjuntos = Session["eListadoProveedorAnexoAdjunto"] as List<dynamic>;

                var eliminarAdjunto = listadoAnexosAdjuntos.Where(x => x.ID == id).FirstOrDefault();

                listadoAnexosAdjuntos.Remove(eliminarAdjunto);

                Session["eListadoProveedorAnexoAdjunto"] = listadoAnexosAdjuntos;


                string html = "";
                foreach (var item in listadoAnexosAdjuntos)
                {
                    html += $"<tr>";
                    html += $"<td>{item.NombreCompleto}</td>";
                    html += $"<td width='40'>";
                    html += $"<a onclick=\"EliminarAnexoProveedorAdjunto('{item.ID}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
                    html += $"</td>";
                    html += $"</tr>";
                }

                return Json(html, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult GuardarDocumentoProveedor()
        {
            try
            {
                var idSucursal = (Guid)Session["i_id_sucursal"];
                List<dynamic> listadoAnexosAdjuntos = Session["eListadoProveedorAnexoAdjunto"] as List<dynamic>;

                if (Request.Files.Count < 2)
                {
                    return Json(new { isValid = false, mensaje = "Se deben ingresar el documento pdf y el xml" });
                }

                HttpPostedFileBase fileup = Request.Files[0];

                byte[] bytes;

                using (var memory = new MemoryStream())
                {
                    fileup.InputStream.CopyTo(memory);
                    bytes = memory.ToArray();
                }

                var newFilePdf = new
                {
                    NombreCompleto = fileup.FileName,
                    Base64 = Convert.ToBase64String(bytes)
                };

                fileup = Request.Files[1];

                using (var memory = new MemoryStream())
                {
                    fileup.InputStream.CopyTo(memory);
                    bytes = memory.ToArray();
                }

                var newFileXml = new
                {
                    NombreCompleto = fileup.FileName,
                    Base64 = Convert.ToBase64String(bytes)
                };

                var json = new
                {
                    IdSucursal = idSucursal.ToString(),
                    Usuario = Session["i_susuario"].ToString(),
                    NombreAttachedXml = newFileXml.NombreCompleto,
                    XmlAttechedBase64 = newFileXml.Base64,
                    NombrePdf = newFilePdf.NombreCompleto,
                    PdfBase64 = newFilePdf.Base64,
                    ListadoAnexos = listadoAnexosAdjuntos?.Select(la => new { la.NombreCompleto, ArchivoBase64 = la.Base64 })
                };

                var (res, mensaje) = ConsumirApi(JsonConvert.SerializeObject(json), ConfigurationManager.AppSettings["ruta_api_proveedor"].ToString());

                if (Boolean.Parse(res))
                {
                    return Json(new { isValid = true });
                }
                else
                {
                    log.Error(mensaje);
                    return Json(new { isValid = false, mensaje });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public (string res, string mensaje) ConsumirApi(string json, string sRuta)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sRuta);
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse respuesta = (HttpWebResponse)request.GetResponse())
                {
                    if (respuesta.StatusCode == HttpStatusCode.Accepted || respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.Created)
                    {
                        using (StreamReader read = new StreamReader(respuesta.GetResponseStream()))
                        {
                            var result = read.ReadToEnd();

                            dynamic resultObjeto = JsonConvert.DeserializeObject(result);
                            return (resultObjeto.Respuesta, resultObjeto.Mensaje);

                        };
                    }
                    else
                    {
                        return ("False", "Se presento un error al enviar correo");
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }

        public (string res, string mensaje) ConsumirApiGet(string sRuta)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sRuta);
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
                            return (resultObjeto.Respuesta, resultObjeto.Mensaje);

                        };
                    }
                    else
                    {
                        return ("false", "Error al enviar la informacion");
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }
    }
}