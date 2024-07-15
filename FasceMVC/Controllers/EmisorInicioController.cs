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
    public class EmisorInicioController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmisorInicioController).Name);
        private readonly ClsTipo _clsTipo = new ClsTipo();
        private readonly ClsDocumento _clsDocumento = new ClsDocumento();

        #region EmisorInicio
        /// <summary>
        /// metodo principal de busqueda
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                Session["soFechaInicial"] = null;
                Session["soFechaFinal"] = null;
                Session["soIdentificacion"] = null;
                Session["soNombre"] = null;
                Session["soIdComprobante"] = null;
                Session["soIdEstado"] = null;
                Session["soIdEstadoFacse"] = null;
                Session["soNumero"] = null;

                var model = new adm_documento
                {
                    UrlImportar = ConfigurationManager.AppSettings["importExcel"].ToString() + $"?sucursal={(Guid)Session["i_id_sucursal"]}&emisor={(Guid)Session["i_id_emisor"]}",
                    FechaInicial = DateTime.Now,
                    FechaFinal = DateTime.Now
                };

                if ((Guid)Session["i_id_perfil"] == Guid.Parse("74D2CF52-4355-49FE-8B1F-E92E50DC8E33") || (Guid)Session["i_id_perfil"] == Guid.Parse("7721348C-A740-4E1F-AFFA-334235D26DCA") || (Guid)Session["i_id_perfil"] == Guid.Parse("99B8DD93-8CC1-402C-8082-B6361CCAF92F") || (Guid)Session["i_id_perfil"] == Guid.Parse("213216B2-3774-42A3-9C50-654D7D847D6F"))
                {
                    var general = _clsDocumento.GeneralDocumento((Guid)Session["i_id_sucursal"]);

                    var FacturasRestante = _clsDocumento.FacturasRestantes((Guid)Session["i_id_emisor"], Guid.Parse("E92E2E55-C00D-430A-89FC-8864900AE1B8"), (Guid)Session["i_id_sucursal"]);

                    ViewBag.FacturasRestantes = (FacturasRestante == 0) ? "" : FacturasRestante.ToString();
                    Session["sViewBagFacturasRestantes"] = ViewBag.FacturasRestantes;

                    ViewBag.Factura = (general.Where(d => d.CodigoTipoDocumento == "02" || d.CodigoTipoDocumento == "04" || d.CodigoTipoDocumento == "03" || d.CodigoTipoDocumento == "01").FirstOrDefault() == null ? 0 : general.Where(d => d.CodigoTipoDocumento == "02" || d.CodigoTipoDocumento == "04" || d.CodigoTipoDocumento == "03" || d.CodigoTipoDocumento == "01").Sum(s => s.doc_numero));
                    Session["dViewBagFactura"] = ViewBag.Factura;

                    ViewBag.FFecha = (general.Where(d => d.CodigoTipoDocumento == "02" || d.CodigoTipoDocumento == "04" || d.CodigoTipoDocumento == "03" || d.CodigoTipoDocumento == "01").FirstOrDefault() == null ? "No Aplica" : string.Format("{0:dddd MMMM d, yyyy}", general.Where(d => d.CodigoTipoDocumento == "02" || d.CodigoTipoDocumento == "04" || d.CodigoTipoDocumento == "03" || d.CodigoTipoDocumento == "01").Max(m => m.doc_fecha_recepcion)));
                    Session["sViewBagFFecha"] = ViewBag.FFecha;

                    ViewBag.FvencimientoResolucionFactura = _clsDocumento.ResolucionVencer((Guid)Session["i_id_emisor"], (Guid)Session["i_id_sucursal"], Guid.Parse("E92E2E55-C00D-430A-89FC-8864900AE1B8")).FirstOrDefault()?.ere_fecha_final.ToString("dd/MM/yyyy");
                    Session["sViewBagFvencimientoResolucionFactura"] = ViewBag.FvencimientoResolucionFactura;

                    ViewBag.Prefijo = _clsDocumento.ResolucionVencer((Guid)Session["i_id_emisor"], (Guid)Session["i_id_sucursal"], Guid.Parse("E92E2E55-C00D-430A-89FC-8864900AE1B8")).FirstOrDefault()?.NombreDocumento;
                    Session["sViewBagPrefijo"] = ViewBag.Prefijo;

                    ViewBag.NCredito = (general.Where(d => d.CodigoTipoDocumento == "91").FirstOrDefault() == null ? 0 : general.Where(d => d.CodigoTipoDocumento == "91").Sum(s => s.doc_numero));
                    Session["dViewBagNCredito"] = ViewBag.NCredito;

                    ViewBag.FNCredito = (general.Where(d => d.CodigoTipoDocumento == "91").FirstOrDefault() == null ? "No Aplica" : string.Format("{0:dddd MMMM d, yyyy}", general.Where(d => d.CodigoTipoDocumento == "91").Max(m => m.doc_fecha_recepcion)));
                    Session["sViewBagFNCredito"] = ViewBag.FNCredito;

                    ViewBag.FvencimientoResolucionFactura1 = _clsDocumento.ResolucionVencer((Guid)Session["i_id_emisor"], (Guid)Session["i_id_sucursal"], Guid.Parse("7E1E050B-B6F4-4B71-A3D9-1236FA5B2D27")).FirstOrDefault()?.ere_fecha_final.ToString("dd/MM/yyyy");
                    Session["sViewBagFvencimientoResolucionFactura1"] = ViewBag.FvencimientoResolucionFactura1;

                    ViewBag.NDebito = (general.Where(d => d.CodigoTipoDocumento == "92").FirstOrDefault() == null ? 0 : general.Where(d => d.CodigoTipoDocumento == "92").Sum(s => s.doc_numero));
                    Session["dViewBagNDebito"] = ViewBag.NDebito;

                    ViewBag.FNDebito = (general.Where(d => d.CodigoTipoDocumento == "92").FirstOrDefault() == null ? "No Aplica" : string.Format("{0:dddd MMMM d, yyyy}", general.Where(d => d.CodigoTipoDocumento == "92").Max(m => m.doc_fecha_recepcion)));
                    Session["sViewBagFNDebito"] = ViewBag.FNDebito;

                    ViewBag.FvencimientoResolucionFactura2 = _clsDocumento.ResolucionVencer((Guid)Session["i_id_emisor"], (Guid)Session["i_id_sucursal"], Guid.Parse("D06A400C-AA35-4095-BD5C-8D297532464B")).FirstOrDefault()?.ere_fecha_final.ToString("dd/MM/yyyy");
                    Session["sViewBagFvencimientoResolucionFactura2"] = ViewBag.FvencimientoResolucionFactura2;
                }

                var tipoComprobante = _clsTipo.ListadoTipocomprobante();
                Session["ListadoComprobanteController"] = tipoComprobante;
                ViewBag.IdComprobante = new SelectList((List<sys_tipo_documento>)Session["ListadoComprobanteController"], "tdo_id", "tdo_descripcion");

                var tipoEstado = _clsTipo.ListadoTipoEstadoDocumento();
                Session["ListadoEstadoController"] = tipoEstado;
                ViewBag.IdEstado = new SelectList((List<sys_estado_documento>)Session["ListadoEstadoController"], "ted_id", "ted_descripcion");

                var tipoEstadoFacse = _clsTipo.ListadoTipoEstadoFacse();
                Session["ListadoEstadoFacseController"] = tipoEstadoFacse;
                ViewBag.IdEstadoFacse = new SelectList((List<sys_estado_documento_facse>)Session["ListadoEstadoFacseController"], "edf_id", "edf_descripcion");

                return View(model);

            }
            catch (Exception ex)
            {
                var model = new adm_documento();

                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        //public JsonResult ListadoGrilla(string sidx, string sord, int page, int rows)
        //{
        //    int pageIndex = Convert.ToInt32(page) - 1;
        //    int pageSize = rows;
        //    int startRows = rows * (pageIndex);

        //    try
        //    {
        //        DateTime dFechaInicio, dFechaFinal;
        //        string sIdentificacion = "", sNombre = "";
        //        Guid? gIdComprobante = null;
        //        int? iNumero = null;

        //        if (Session["soFechaInicial"] != null)
        //            dFechaInicio = (DateTime)Session["soFechaInicial"];
        //        else
        //        {
        //            dFechaInicio = DateTime.Now;
        //            Session["soFechaInicial"] = dFechaInicio;
        //        }
        //        if (Session["soFechaFinal"] != null)
        //            dFechaFinal = (DateTime)Session["soFechaFinal"];
        //        else
        //        {
        //            dFechaFinal = DateTime.Now;
        //            Session["soFechaFinal"] = dFechaFinal;
        //        }

        //        if (Session["soIdentificacion"] != null)
        //            sIdentificacion = (string)Session["soIdentificacion"];
        //        if (Session["soNombre"] != null)
        //            sNombre = (string)Session["soNombre"];

        //        if (Session["soIdComprobante"] != null)
        //            gIdComprobante = (Guid)Session["soIdComprobante"];
        //        if (Session["soNumero"] != null)
        //            iNumero = (int)Session["soNumero"];

        //        int count = 0;

        //        var listadoDocumento = _clsDocumento.ListadoDocumento(dFechaInicio, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, sidx, sord, rows, startRows, ref count, (Guid)Session["i_id_sucursal"]);

        //        int totalRecords = count;
        //        var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

        //        var jsonData = new
        //        {
        //            total = totalPages,
        //            page,
        //            records = totalRecords,
        //            rows = (from d in listadoDocumento
        //                    select new
        //                    {
        //                        RutaJson = d.adm_documento_file.FirstOrDefault().dfi_json_facse,
        //                        CodigoEstado = d.sys_estado_documento.ted_codigo,
        //                        d.doc_id,
        //                        Estado = string.Format("{0}",
        //                                RenderHelpers.Image(this, "Action",
        //                        (d.sys_estado_documento.ted_codigo == "1" ? "Aprobado por la Dian" : d.sys_estado_documento.ted_codigo == "2" ? "En proceso de Validación" : d.sys_estado_documento.ted_codigo == "10" ? "Aprobado con Novedad" : "Sin Aprobacion de la Dian"),
        //                        (d.sys_estado_documento.ted_codigo == "1" ? "~/Content/Images/verde.png" : d.sys_estado_documento.ted_codigo == "2" ? "~/Content/Images/rojo.png" : d.sys_estado_documento.ted_codigo == "10" ? "~/Content/Images/aprobacion_novedad.png" : "~/Content/Images/amarillo.png"))),
        //                        Tipo = d.sys_tipo_documento == null ? "" : d.sys_tipo_documento.tdo_abreviatura.ToLower(),
        //                        d.doc_prefijo,
        //                        d.doc_numero,
        //                        Nombre = d.adm_receptor == null ? "" : d.adm_receptor.rec_nombre,
        //                        Identificacion = d.adm_receptor == null ? "" : d.adm_receptor.rec_identificacion,
        //                        doc_fecha_recepcion = d.doc_fecha_recepcion.ToString("dd/MM/yyyy hh:mm:ss"),
        //                        doc_fecha_envio = d.doc_fecha_envio.ToString("dd/MM/yyyy hh:mm:ss"),
        //                        d.doc_valor_total,
        //                        d.doc_usuario,
        //                        Anexo = d.adm_documentos_anexo.Count(),
        //                        Json = string.Format("{0}",
        //                                    RenderHelpers.ImageButton(
        //                                              this,
        //                                             "JSON",
        //                                            "~/Content/Images/descargar.png",
        //                                            "/ExportarJson",
        //                                            new { gDocumento = d.doc_id, sTipoDocumento = d.sys_tipo_documento.tdo_abreviatura.ToUpper(), sNitEmisor = d.adm_emisor.emi_identificacion, sPrefijo = d.doc_prefijo, sNumeroFactura = d.doc_numero },
        //                                            new { style = (d.adm_documento_file != null && !string.IsNullOrEmpty(d.adm_documento_file.FirstOrDefault().dfi_xml) && Session["clave_super"] != null ? "border:0px;" : "display:none") },
        //                                            new { target = "_blank" })),
        //                        Xml = string.Format("{0}",
        //                                    RenderHelpers.ImageButton(
        //                                              this,
        //                                             "Xml",
        //                                            "~/Content/Images/xml.png",
        //                                            d.adm_documento_file.FirstOrDefault().dfi_xml,
        //                                            new { gDocumento = d.doc_id, sTipoDocumento = d.sys_tipo_documento.tdo_abreviatura.ToUpper(), sNitEmisor = d.adm_emisor.emi_identificacion, sPrefijo = d.doc_prefijo, sNumeroFactura = d.doc_numero },
        //                                            new { style = (d.adm_documento_file != null && !string.IsNullOrEmpty(d.adm_documento_file.FirstOrDefault().dfi_xml) ? "border:0px;" : "display:none") },
        //                                            new { target = "_blank", href = d.adm_documento_file.FirstOrDefault().dfi_xml })),
        //                        Pdf = string.Format("{0}",
        //                                    RenderHelpers.ImageButton(
        //                                              this,
        //                                             "PDF",
        //                                            "~/Content/Images/pdf.png",
        //                                            "/ExportarPdf",
        //                                            new { gDocumento = d.doc_id },
        //                                            new { style = (d.adm_documento_file != null && !string.IsNullOrEmpty(d.adm_documento_file.FirstOrDefault().dfi_json_facse) ? "border:0px;" : "display:none") },
        //                                            new { target = "_blank" }))
        //                    }).ToArray()
        //        };

        //        return Json(jsonData, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        var model = new adm_documento
        //        {
        //            ListadoDocumento = null
        //        };

        //        log.Error($"Error al buscar informacion: {ex}");
        //        ModelState.AddModelError("", "Error al buscar informacion. Intente de nuevo, si el problema persiste consulte al administrador.");
        //    }
        //    return Json(new { isValid = false });
        //}

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
                Guid? gIdComprobante = null, gIdEstado = null, gIdEstadoFacse = null;
                int? iNumero = null;

                if (Session["soFechaInicial"] != null)
                    dFechaInicio = (DateTime)Session["soFechaInicial"];
                else
                {
                    dFechaInicio = DateTime.Now;
                    Session["soFechaInicial"] = dFechaInicio;
                }
                if (Session["soFechaFinal"] != null)
                    dFechaFinal = (DateTime)Session["soFechaFinal"];
                else
                {
                    dFechaFinal = DateTime.Now;
                    Session["soFechaFinal"] = dFechaFinal;
                }

                if (Session["soIdentificacion"] != null)
                    sIdentificacion = (string)Session["soIdentificacion"];
                if (Session["soNombre"] != null)
                    sNombre = (string)Session["soNombre"];

                if (Session["soIdComprobante"] != null)
                    gIdComprobante = (Guid)Session["soIdComprobante"];
                if (Session["soNumero"] != null)
                    iNumero = (int)Session["soNumero"];

                if (Session["soIdEstado"] != null)
                    gIdEstado = (Guid)Session["soIdEstado"];
                if (Session["soIdEstadoFacse"] != null)
                    gIdEstadoFacse = (Guid)Session["soIdEstadoFacse"];

                int count = 0;

                var listadoDocumento = _clsDocumento.ListadoDocumento(dFechaInicio, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, sortColumn, sortColumnDir, pageSize, skip, ref count, (Guid)Session["i_id_sucursal"], gIdEstado, gIdEstadoFacse);

                int totalRecords = count;
                //var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                var jsonData = (from d in listadoDocumento
                                select new
                                {
                                    CodigoEstado = d.ted_codigo,
                                    d.doc_id,
                                    Tipo = d.tdo_abreviatura.ToUpper(),
                                    d.doc_prefijo,
                                    d.doc_numero,
                                    Nombre = d.rec_nombre,
                                    Identificacion = d.rec_identificacion,
                                    doc_fecha_recepcion = d.doc_fecha_recepcion.ToString("dd/MM/yyyy hh:mm:ss"),
                                    doc_fecha_envio = d.doc_fecha_envio.ToString("dd/MM/yyyy hh:mm:ss"),
                                    d.doc_valor_total,
                                    d.doc_usuario,
                                    Xml = d.dfi_xml,
                                    d.correo,
                                    d.anexo,
                                    d.notificacion,
                                    d.edf_ruta_imagen,
                                    d.edf_descripcion,
                                    Session = (Session["clave_super"] == null ? true : false)
                                    //Json = string.Format("{0}",
                                    //            RenderHelpers.ImageButton(
                                    //                      this,
                                    //                     "JSON",
                                    //                    "~/Content/Images/descargar.png",
                                    //                    "/ExportarJson",
                                    //                    new { gDocumento = d.doc_id, sTipoDocumento = d.sys_tipo_documento.tdo_abreviatura.ToUpper(), sNitEmisor = d.adm_emisor.emi_identificacion, sPrefijo = d.doc_prefijo, sNumeroFactura = d.doc_numero },
                                    //                    new { style = (d.adm_documento_file != null && !string.IsNullOrEmpty(d.adm_documento_file.FirstOrDefault().dfi_xml) && Session["clave_super"] != null ? "border:0px;" : "display:none") },
                                    //                    new { target = "_blank" })),
                                    //Xml = string.Format("{0}",
                                    //            RenderHelpers.ImageButton(
                                    //                      this,
                                    //                     "Xml",
                                    //                    "~/Content/Images/xml.png",
                                    //                    d.adm_documento_file.FirstOrDefault().dfi_xml,
                                    //                    new { gDocumento = d.doc_id, sTipoDocumento = d.sys_tipo_documento.tdo_abreviatura.ToUpper(), sNitEmisor = d.adm_emisor.emi_identificacion, sPrefijo = d.doc_prefijo, sNumeroFactura = d.doc_numero },
                                    //                    new { style = (d.adm_documento_file != null && !string.IsNullOrEmpty(d.adm_documento_file.FirstOrDefault().dfi_xml) ? "border:0px;" : "display:none") },
                                    //                    new { target = "_blank", href = d.adm_documento_file.FirstOrDefault().dfi_xml })),
                                    //Pdf = string.Format("{0}",
                                    //            RenderHelpers.ImageButton(
                                    //                      this,
                                    //                     "PDF",
                                    //                    "~/Content/Images/pdf.png",
                                    //                    "/ExportarPdf",
                                    //                    new { gDocumento = d.doc_id },
                                    //                    new { style = (d.adm_documento_file != null && !string.IsNullOrEmpty(d.adm_documento_file.FirstOrDefault().dfi_json_facse) ? "border:0px;" : "display:none") },
                                    //                    new { target = "_blank" }))
                                }).ToList();

                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = jsonData });

                //return Json(jsonData, JsonRequestBehavior.AllowGet);
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

        public JsonResult SubListadoGrilla(Guid id)
        {
            try
            {

                var listadoEstado = _clsDocumento.ListadoEstadoFacse(id);

                var jsonData = (from d in listadoEstado
                                select new
                                {
                                    d.sys_estado_documento_facse.edf_descripcion
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

        public JsonResult SubListadoGrillaNotificacion(Guid id)
        {
            try
            {

                var listadoNotificacion = _clsDocumento.ListadoNotificacion(id);

                var jsonData = (from d in listadoNotificacion
                                select new
                                {
                                    d.sys_reglas_dian.rdi_regla,
                                    d.sys_reglas_dian.rdi_descripcion_facse
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

        public JsonResult SubListadoGrillaCorreo(Guid id)
        {
            try
            {

                var listadoCorreo = _clsDocumento.ListadoCorreo(id);

                var jsonData = (from d in listadoCorreo
                                select new
                                {
                                    Fecha = d.dco_fecha.ToString("dd/MM/yyyy"),
                                    d.dco_correo,
                                    d.sys_estado_documento_correo.edc_descripcion
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

                var listadoAnexo = _clsDocumento.ListadoAnexo(id);

                var jsonData = (from d in listadoAnexo
                                select new
                                {
                                    d.dan_nombre,
                                    d.dan_directorio
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



        //[HttpPost]
        //public ActionResult CargarListado(int pagina = 1, string p = null, string sortOrder = null)
        //{
        //    ViewBag.sortOrder = sortOrder;
        //    ViewBag.FechaRecepcionSortParm = String.IsNullOrEmpty(sortOrder) ? "FechaRecepcion" : "";
        //    ViewBag.NombreSortParm = sortOrder == "Nombre" ? "Nombre_desc" : "Nombre";

        //    ViewBag.IdentificacionSortParm = sortOrder == "Identificacion" ? "Identificacion_desc" : "Identificacion";
        //    ViewBag.FechaEnvioSortParm = sortOrder == "FechaEnvio" ? "FechaEnvio_desc" : "FechaEnvio";
        //    ViewBag.TotalSortParm = sortOrder == "Total" ? "Total_desc" : "Total";
        //    ViewBag.NumeroSortParm = sortOrder == "Numero" ? "Numero_desc" : "Numero";

        //    try
        //    {
        //        int count = 0;
        //        var model = new adm_documento
        //        {
        //            FechaInicial = DateTime.Now,
        //            FechaFinal = DateTime.Now,
        //            PaginaActual = pagina
        //        };

        //        if (p != null)
        //        {
        //            if (Session["soFechaInicial"] != null)
        //                model.FechaInicial = (DateTime)Session["soFechaInicial"];
        //            else
        //                model.FechaInicial = null;
        //            if (Session["soFechaFinal"] != null)
        //                model.FechaFinal = (DateTime)Session["soFechaFinal"];
        //            else
        //                model.FechaFinal = null;

        //            if (Session["soIdentificacion"] != null)
        //                model.Identificacion = (string)Session["soIdentificacion"];
        //            if (Session["soNombre"] != null)
        //                model.Nombre = (string)Session["soNombre"];

        //            if (Session["soIdComprobante"] != null)
        //                model.IdComprobante = (Guid)Session["soIdComprobante"];
        //            if (Session["soNumero"] != null)
        //                model.Numero = (int)Session["soNumero"];
        //        }
        //        else
        //        {
        //            Session["soFechaInicial"] = model.FechaInicial;
        //            Session["soFechaFinal"] = model.FechaFinal;
        //        }

        //        model.ListadoDocumento = _clsDocumento.ListadoDocumento(model, pagina, _RegistrosPorPagina, ref count, (Guid)Session["i_id_sucursal"], sortOrder);

        //        ViewBag.Total = model.ListadoDocumento.Sum(s => s.doc_valor_total);

        //        var _TotalPaginas = (int)Math.Ceiling((double)count / _RegistrosPorPagina);
        //        model.TotalPaginas = _TotalPaginas;
        //        model.TotalRegistros = count;

        //        var tipoComprobante = _clsTipo.ListadoTipocomprobante();
        //        ListadoComprobanteController = tipoComprobante;
        //        ViewBag.IdComprobante = new SelectList(ListadoComprobanteController, "tdo_id", "tdo_descripcion");

        //        return Json(new { isValid = true, li = RenderPartialViewToString(this, "Listado", model) });
        //    }
        //    catch (Exception ex)
        //    {
        //        var model = new adm_documento
        //        {
        //            ListadoDocumento = null
        //        };

        //        var tipoComprobante = _clsTipo.ListadoTipocomprobante();
        //        ListadoComprobanteController = tipoComprobante;
        //        ViewBag.IdComprobante = new SelectList(ListadoComprobanteController, "tdo_id", "tdo_descripcion");

        //        log.Error($"Error al buscar informacion: {ex}");
        //        ModelState.AddModelError("", "Error al buscar informacion. Intente de nuevo, si el problema persiste consulte al administrador.");
        //    }
        //    return Json(new { isValid = false });
        //}

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
        public ActionResult ListadoBuscar(adm_documento documento, string _search, string _exportar)
        {
            try
            {
                if (_exportar == null)
                {
                    if (documento.FechaInicial == null)
                    {
                        documento.FechaInicial = DateTime.Now;
                    }
                    if (documento.FechaFinal == null)
                    {
                        documento.FechaFinal = DateTime.Now;
                    }
                    Session["soFechaInicial"] = documento.FechaInicial;
                    Session["soFechaFinal"] = documento.FechaFinal;
                    Session["soIdentificacion"] = documento.Identificacion;
                    Session["soNombre"] = documento.Nombre;
                    Session["soIdComprobante"] = documento.IdComprobante;
                    Session["soIdEstado"] = documento.IdEstado;
                    Session["soIdEstadoFacse"] = documento.IdEstadoFacse;
                    Session["soNumero"] = documento.Numero;

                    ModelState.Clear();
                    documento.UrlImportar = ConfigurationManager.AppSettings["importExcel"].ToString() + $"?sucursal={(Guid)Session["i_id_sucursal"]}&emisor={(Guid)Session["i_id_emisor"]}";

                    if ((Guid)Session["i_id_perfil"] == Guid.Parse("74D2CF52-4355-49FE-8B1F-E92E50DC8E33") || (Guid)Session["i_id_perfil"] == Guid.Parse("7721348C-A740-4E1F-AFFA-334235D26DCA") || (Guid)Session["i_id_perfil"] == Guid.Parse("99B8DD93-8CC1-402C-8082-B6361CCAF92F") || (Guid)Session["i_id_perfil"] == Guid.Parse("213216B2-3774-42A3-9C50-654D7D847D6F"))
                    {
                        ViewBag.FacturasRestantes = Session["sViewBagFacturasRestantes"].ToString();

                        ViewBag.Factura = (decimal)Session["dViewBagFactura"];

                        ViewBag.FFecha = Session["sViewBagFFecha"].ToString();

                        ViewBag.FvencimientoResolucionFactura = Session["sViewBagFvencimientoResolucionFactura"].ToString();

                        ViewBag.Prefijo = Session["sViewBagPrefijo"].ToString();

                        ViewBag.NCredito = (decimal)Session["dViewBagNCredito"];

                        ViewBag.FNCredito = Session["sViewBagFNCredito"].ToString();

                        ViewBag.FvencimientoResolucionFactura1 = Session["sViewBagFvencimientoResolucionFactura1"] == null ? "" : Session["sViewBagFvencimientoResolucionFactura1"].ToString();

                        ViewBag.NDebito = (decimal)Session["dViewBagNDebito"];

                        ViewBag.FNDebito = Session["sViewBagFNDebito"].ToString();

                        Session["sViewBagFvencimientoResolucionFactura2"] = ViewBag.FvencimientoResolucionFactura2;
                    }

                    ViewBag.IdComprobante = new SelectList((List<sys_tipo_documento>)Session["ListadoComprobanteController"], "tdo_id", "tdo_descripcion", documento.IdComprobante);
                    ViewBag.IdEstado = new SelectList((List<sys_estado_documento>)Session["ListadoEstadoController"], "ted_id", "ted_descripcion", documento.IdEstado);
                    ViewBag.IdEstadoFacse = new SelectList((List<sys_estado_documento_facse>)Session["ListadoEstadoFacseController"], "edf_id", "edf_descripcion", documento.IdEstadoFacse);
                }
                else
                {
                    Generador rp = new Generador();

                    DateTime dFechaInicio, dFechaFinal;
                    string sIdentificacion = "", sNombre = "";
                    Guid? gIdComprobante = null, gIdEstado = null, gIdEstadoFacse = null;
                    int? iNumero = null;

                    if (Session["soFechaInicial"] != null)
                        dFechaInicio = (DateTime)Session["soFechaInicial"];
                    else
                    {
                        dFechaInicio = DateTime.Now;
                        Session["soFechaInicial"] = dFechaInicio;
                    }
                    if (Session["soFechaFinal"] != null)
                        dFechaFinal = (DateTime)Session["soFechaFinal"];
                    else
                    {
                        dFechaFinal = DateTime.Now;
                        Session["soFechaFinal"] = dFechaFinal;
                    }

                    if (Session["soIdentificacion"] != null)
                        sIdentificacion = (string)Session["soIdentificacion"];
                    if (Session["soNombre"] != null)
                        sNombre = (string)Session["soNombre"];

                    if (Session["soIdComprobante"] != null)
                        gIdComprobante = (Guid)Session["soIdComprobante"];
                    if (Session["soNumero"] != null)
                        iNumero = (int)Session["soNumero"];

                    if (Session["soIdEstado"] != null)
                        gIdEstado = (Guid)Session["soIdEstado"];
                    if (Session["soIdEstadoFacse"] != null)
                        gIdEstadoFacse = (Guid)Session["soIdEstadoFacse"];

                    Byte[] Report = rp.GetInicioReport(dFechaInicio, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, (Guid)Session["i_id_sucursal"], gIdEstado, gIdEstadoFacse);

                    string sNombreArchivo = $"Inicio.xlsx";

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
                documento.ListadoDocumento = null;

                ViewBag.IdComprobante = new SelectList((List<sys_tipo_documento>)Session["ListadoComprobanteController"], "tdo_id", "tdo_descripcion", documento.IdComprobante);
                ViewBag.IdEstado = new SelectList((List<sys_estado_documento>)Session["ListadoEstadoController"], "ted_id", "ted_descripcion", documento.IdEstado);
                ViewBag.IdEstadoFacse = new SelectList((List<sys_estado_documento_facse>)Session["ListadoEstadoFacseController"], "edf_id", "edf_descripcion", documento.IdEstadoFacse);

                log.Error($"Error al consultar la consulta documento: {ex}");
                ModelState.AddModelError("", "Error al consultar la consulta documento. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(new { archivo = false, pan = RenderPartialViewToString(this, "Index", documento) });
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
        /// Reenvio de la dian
        /// </summary>
        /// <param name="listadoReenviado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReenvioDian(List<adm_documento> listadoDocumento)
        {
            try
            {
                foreach (var item in listadoDocumento)
                {
                    if (item.CodigoEstado != "1" && item.CodigoEstado != "101")
                    {
                        var Rutajson = _clsDocumento.ArchivoDocumento(item.doc_id);

                        ConsumirApi("Comprobante", Rutajson.dfi_json_facse);
                    }
                    //_facturaRepositorio.AprobarFacturaRutinaria(item, iUsuario);
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
        public ActionResult ReenvioCorreo(List<adm_documento> listadoDocumento, string email)
        {
            try
            {
                var idSucursal = (Guid)Session["i_id_sucursal"];
                bool bCorreoJson = false;

                if (string.IsNullOrEmpty(email))
                {
                    bCorreoJson = true;
                }

                foreach (var item in listadoDocumento)
                {
                    if (item.CodigoEstado == "1" || item.CodigoEstado == "10")
                    {
                        if (bCorreoJson)
                        {
                            var Rutajson = _clsDocumento.ArchivoDocumento(item.doc_id);

                            string sEmail = JsonConvert.DeserializeObject<DocumentoModel>(Rutajson.dfi_json_facse)?.Receptor.email;
                            email = sEmail;
                        }

                        var listadoAdjuntos = new List<dynamic>();
                        byte[] bytes;

                        var listadoRutasAnexos = _clsDocumento.ListadoAnexo(item.doc_id);

                        Stream stream;
                        MemoryStream memoryStream = new MemoryStream();

                        listadoRutasAnexos.ForEach(anexo =>
                        {
                            stream = WebRequest.Create(anexo.dan_directorio).GetResponse().GetResponseStream();
                            stream.CopyTo(memoryStream);
                            bytes = memoryStream.ToArray();

                            var newFile = new
                            {
                                NombreCompleto = Path.GetFileName(anexo.dan_directorio),
                                Base64 = Convert.ToBase64String(bytes)
                            };

                            listadoAdjuntos.Add(newFile);
                        });


                        var json = new
                        {
                            IdDocumento = item.doc_id.ToString(),
                            IdEmisorSucursal = idSucursal.ToString(),
                            Correo = email,
                            ListadoAdjunto = listadoAdjuntos
                        };

                        ConsumirApi(JsonConvert.SerializeObject(json));
                    }
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
        /// Se usa para consumir el api con el json que se genera a partir de los datos ingresados en el formulario
        /// </summary>
        /// <param name="facturacionManual"></param>
        /// <param name="metodoApi"></param>
        /// <returns></returns>
        public string ConsumirApi(string metodoApi, string json)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api"].ToString() + metodoApi;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaApiCompleta);
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

                            //if (metodoApi == "ValidarJson")
                            //{
                            //    dynamic resultObjeto = JsonConvert.DeserializeObject(result);
                            //    return resultObjeto.respuesta;
                            //}
                            //else
                            //{
                            //    return JsonConvert.DeserializeObject<string>(result);
                            //}
                            dynamic resultObjeto = JsonConvert.DeserializeObject(result);
                            return resultObjeto.respuesta;
                        };
                    }
                    else
                    {
                        return "Error al validar el json";
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }


        #endregion

        #region ViewPdf

        public ActionResult ExportarPdf(Guid gDocumento)
        {
            try
            {

                Session["sListadoAnexos" + gDocumento] = null;
                Session["sListadoAnexosAdjuntos" + gDocumento] = null;
                Generador rp = new Generador();

                dynamic objectoRespuesta = ConsumirApiPDF(gDocumento);

                string sNombreArchivo = $"Documento.pdf";

                string reportBase64 = objectoRespuesta.PdfBase64;

                var model = new ViewPdf
                {
                    PdfBase64 = reportBase64,
                    Estado = _clsDocumento.ValidarEstadoDocumento(gDocumento),
                    IdDocumento = gDocumento,
                    EstadoJson = (bool)objectoRespuesta.Respuesta
                };

                Session["sListadoAnexos" + gDocumento] = _clsDocumento.ListadoAnexo(gDocumento);

                if (Session["sListadoAnexos" + gDocumento] != null)
                {
                    ViewBag.ListadoAnexos = Session["sListadoAnexos" + gDocumento] as List<adm_documentos_anexo>;
                }

                if (!model.Estado)
                {
                    model.Alerta = "Sin aprobacion.";
                }

                if (!(bool)objectoRespuesta.Respuesta)
                {
                    ViewBag.JsFuncion = $"mensaje('{objectoRespuesta.MensajeRespuesta}', 'error')";
                }
                else
                {
                    if (TempData["sMensajePdf"] is string mensaje)
                    {
                        ViewBag.JsFuncion = mensaje;
                    }
                }

                return View("ViewPdf", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        public ActionResult EliminarAnexo(Guid id, Guid idDocumento)
        {
            try
            {
                var listadoAnexos = Session["sListadoAnexos" + idDocumento] as List<adm_documentos_anexo>;

                var eliminarAnexo = listadoAnexos.Where(x => x.dan_id == id).FirstOrDefault();

                listadoAnexos.Remove(eliminarAnexo);

                Session["sListadoAnexos" + idDocumento] = listadoAnexos;

                string html = "";
                foreach (var item in listadoAnexos)
                {
                    html += $"<tr>";
                    html += $"<td>{item.dan_nombre}{item.dan_extension}</td>";
                    html += $"<td width='40'>";
                    html += $"<a onclick=\"eliminarAnexo('{item.dan_id}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
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
        public ActionResult EliminarAnexoAdjunto(int id, Guid idDocumento)
        {
            try
            {
                var listadoAnexosAdjuntos = Session["sListadoAnexosAdjuntos" + idDocumento] as List<dynamic>;

                var eliminarAdjunto = listadoAnexosAdjuntos.Where(x => x.ID == id).FirstOrDefault();

                listadoAnexosAdjuntos.Remove(eliminarAdjunto);

                Session["sListadoAnexosAdjuntos" + idDocumento] = listadoAnexosAdjuntos;


                string html = "";
                foreach (var item in listadoAnexosAdjuntos)
                {
                    html += $"<tr>";
                    html += $"<td>{item.NombreCompleto}</td>";
                    html += $"<td width='40'>";
                    html += $"<a onclick=\"eliminarAnexo('{item.ID}')\" style=\"cursor: pointer\"><img src = \"{Url.Content("~/Content/Images/basura.png")}\" /></a>";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarArchivosAdjuntos(ViewPdf viewPdf)
        {
            try
            {
                var listadoAnexosAdjuntos = Session["sListadoAnexosAdjuntos" + viewPdf.IdDocumento] as List<dynamic>;

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

                Session["sListadoAnexosAdjuntos" + viewPdf.IdDocumento] = listadoAdjuntos;

                return Json(html, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult EnviarCorreoPDF(ViewPdf viewPdf)
        {
            try
            {
                var idSucursal = (Guid)Session["i_id_sucursal"];
                var listadoAnexos = Session["sListadoAnexos" + viewPdf.IdDocumento] as List<adm_documentos_anexo>;
                var listadoAnexosAdjuntos = Session["sListadoAnexosAdjuntos" + viewPdf.IdDocumento] as List<dynamic>;
                var listadoAdjuntosEnviar = new List<dynamic>();

                byte[] bytes;

                if (listadoAnexos != null && listadoAnexos.Any())
                {
                    foreach (var item1 in listadoAnexos)
                    {
                        using (var memory = new MemoryStream())
                        {
                            ObtenerArchivoZip(item1.dan_directorio).CopyTo(memory);
                            bytes = memory.ToArray();
                        }

                        var newFile = new
                        {
                            NombreCompleto = $"{item1.dan_nombre}{item1.dan_extension}",
                            Base64 = Convert.ToBase64String(bytes)
                        };

                        listadoAdjuntosEnviar.Add(newFile);
                    }
                }

                if (listadoAnexosAdjuntos != null && listadoAnexosAdjuntos.Any())
                {
                    foreach (var item in listadoAnexosAdjuntos)
                    {
                        var newAdjunto = new
                        {
                            item.NombreCompleto,
                            item.Base64
                        };

                        listadoAdjuntosEnviar.Add(newAdjunto);
                    }
                }


                var json = new
                {
                    IdDocumento = viewPdf.IdDocumento.ToString(),
                    IdEmisorSucursal = idSucursal.ToString(),
                    Correo = viewPdf.Email,
                    ListadoAdjunto = listadoAdjuntosEnviar
                };



                var (res, mensaje) = ConsumirApi(JsonConvert.SerializeObject(json));
                string icon = (Convert.ToBoolean(res)) ? "success" : "error";
                TempData["sMensajePdf"] = $"mensaje(\"{mensaje}\",'{icon}')";

                return RedirectToAction("ExportarPdf", new { gDocumento = viewPdf.IdDocumento });


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
        public (string res, string mensaje) ConsumirApi(string json)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api_correo"].ToString();


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaApiCompleta);
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

        #endregion

        public ActionResult ExportarJson(Guid gDocumento, string sTipoDocumento, string sNitEmisor, string sPrefijo, string sNumeroFactura)
        {
            try
            {
                var savePath = _clsDocumento.WriteFileFinal(gDocumento, Server.MapPath("~/Content/plantilla/Json.txt"));

                return File(savePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{sNitEmisor}_{sTipoDocumento}_{sPrefijo}_{sNumeroFactura}.txt");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
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