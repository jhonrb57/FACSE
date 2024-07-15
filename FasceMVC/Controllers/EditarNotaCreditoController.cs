using FasceMVC.App_Start;
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
using static FasceMVC.Models.FacturacionManual;

namespace FasceMVC.Controllers
{
    public class EditarNotaCreditoController : BaseController
    {
        // GET: EditarNotaCredito
        private static readonly ILog log = LogManager.GetLogger(typeof(NotaCreditoController).Name);
        private readonly ClsNotaCredito _clsNotaCredito = new ClsNotaCredito();
        private readonly ClsFacturacionManual _clsFacturacion = new ClsFacturacionManual();
        private readonly ConvertNumeroToLetra _convertNumeroToLetra = new ConvertNumeroToLetra();
        private readonly ClsBuscar _clsBuscar = new ClsBuscar();

        // GET: NotaCredito
        //public ActionResult Index()
        //{
        //    var model = new NotaCredito();
        //    model.ListadoConceptos = _clsNotaCredito.CargarComboConcepto();

        //    return View(model);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(NotaCredito model, string tipoDoc)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                if (Session["sEditarNotaUmm"] != null)
                {
                    var dNota = Session["sEditarNotaUmm"] as dynamic;
                    ViewBag.Titulo = dNota.Titulo;
                }
                else
                {
                    var titulo = (tipoDoc == "91") ? "Nota Credito" : "Nota Debito";
                    var newNota = new
                    {
                        Titulo = titulo,
                        TipoDocumento = tipoDoc
                    };
                    Session["sEditarNotaUmm"] = newNota;
                    ViewBag.Titulo = titulo;
                }

                if (Session["sDatosFacturaEc"] == null)
                {
                    var datosFactura = new NotaCredito.DatosFactura
                    {
                        Concepto = model.Concepto,
                        NumeroFactura = model.NumeroFactura,
                        Prefijo = model.Prefijo
                    };

                    Session["sDatosFacturaEc"] = datosFactura;
                }
                else
                {
                    var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEc"];
                    model.Prefijo = datosFactura.Prefijo;
                    model.NumeroFactura = datosFactura.NumeroFactura;
                    model.Concepto = datosFactura.Concepto;
                    model.FechaTrm = datosFactura.FechaTrm;
                    model.Trm = datosFactura.Trm;
                }

                if (_clsFacturacion.ValidarFacturaEditar(model.Prefijo, emisor, model.NumeroFactura))
                {
                    string resultadoJson = _clsNotaCredito.ConsultarJsonFactura(emisor, model.Prefijo, model.NumeroFactura).json;

                    if (resultadoJson != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(resultadoJson);


                        var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEc"];
                        datosFactura.OrdenCompra = json.Comprobante.NumeroOrden;
                        datosFactura.Despacho = json.Comprobante.NumeroDespacho;
                        datosFactura.Recepcion = json.Comprobante.NumeroRecepcion;
                        model.OrdenCompra = json.Comprobante.NumeroOrden;
                        model.Despacho = json.Comprobante.NumeroDespacho;
                        model.Recepcion = json.Comprobante.NumeroRecepcion;


                        datosFactura.Trm = (model.Moneda == "USD") ? json.PaymentExchangeRate.CalculationRate : 0;

                        if (model.Moneda == "USD")
                        {
                            model.Trm = json.PaymentExchangeRate.CalculationRate;
                            model.FechaTrm = json.PaymentExchangeRate.FechaTrm;
                            datosFactura.FechaTrm = json.PaymentExchangeRate.FechaTrm;
                        }
                        else
                        {
                            model.FechaTrm = json.Comprobante.Fecha;
                            datosFactura.FechaTrm = json.Comprobante.Fecha;
                        }
                        datosFactura.Moneda = _clsFacturacion.CargarGuidMoneda(json.Comprobante.Moneda.Value);
                        Session["sDatosFacturaEc"] = datosFactura;

                        var receptor = _clsNotaCredito.ReceptorDocumento(model.Prefijo, emisor, model.NumeroFactura);

                        model.RazonSocial = receptor.rec_razon_social;
                        model.Identificacion = receptor.rec_identificacion;
                        model.ResFiscal = receptor.rec_tipo_receptor;
                        model.Digito = (receptor.rec_digito != null) ? 0 : int.Parse(receptor.rec_digito);
                        model.Correo = receptor.rec_correo;
                        model.Pais = receptor.sys_pais?.pai_nombre_comun;
                        model.Departamento = receptor.sys_departamento?.dep_nombre;
                        model.Ciudad = receptor.sys_municipio.mun_nombre;
                        model.Direccion = receptor.rec_direccion;
                        model.Telefono = receptor.rec_telefono;
                        model.Referencia = json.Comprobante.Referencia;
                        model.Moneda = json.Comprobante.Moneda;
                        model.FormaPago = _clsNotaCredito.ConsultarFormaPago(json.Comprobante.MetodoPago[0].FormaPago.Value);
                        model.MedioPago = _clsNotaCredito.ConsultarMedioPago(json.Comprobante.MetodoPago[0].MedioPago.Value);

                        model.ListadoConceptos = _clsNotaCredito.CargarComboConcepto();
                        if (model.Concepto == Guid.Empty)
                        {
                            model.Concepto = Guid.Parse(model.ListadoConceptos[0].Value);
                        }

                        var listaDetalles = new List<NotaCredito.Detalle>();

                        foreach (var item in json.Detalles)
                        {
                            var detalle = new NotaCredito.Detalle
                            {
                                IdDetalle = item.idDetalle,
                                Cantidad = item.Cantidad,
                                Cargo = item.Cargos,
                                Codigo = item.codigo,
                                Descuento = item.Descuento,
                                Descripcion = item.Nombre,
                                Total = item.Total,
                                ValorImpuesto = decimal.Parse(item.Impuestos[0].Impuesto.Value),
                                ValorUnidad = item.ValorUnitario,
                                SubTotal = item.SubTotal,
                                CodigoImpuesto = item.Impuestos[0].CodigoImpuesto.Value,
                                Porcentaje = item.Impuestos[0].Porcentaje,
                                Unidad = _clsFacturacion.CargarGuidUnidad(Convert.ToString(item.UnidadCodigo)),
                                Impuesto = _clsFacturacion.CargarGuidImpuesto(Convert.ToString(item.Impuestos[0].CodigoImpuesto.Value), Convert.ToString(item.Impuestos[0].Nombre.Value))
                            };

                            listaDetalles.Add(detalle);
                        }

                        if (Session["sListadoDetallesEc"] != null)
                        {

                            var listaDetallesSession = (List<NotaCredito.Detalle>)Session["sListadoDetallesEc"];
                            model.ListadoDetalles = listaDetallesSession;

                            model.Impuesto = listaDetallesSession.Sum(x => x.ValorImpuesto);
                            model.Cargo = json.Totales.Cargos;
                            model.SubTotal = listaDetallesSession.Sum(x => x.SubTotal);
                            model.Descuento = json.Totales.Descuentos;
                            model.Total = model.Impuesto + model.SubTotal + model.Cargo - model.Descuento;
                        }
                        else
                        {
                            model.ListadoDetalles = listaDetalles;
                            Session["sListadoDetallesEc"] = listaDetalles;

                            model.Cargo = json.Totales.Cargos;
                            model.SubTotal = json.Totales.SubTotal;
                            model.Descuento = json.Totales.Descuentos;
                            model.Total = json.Totales.Total;
                            model.Impuesto = json.Totales.IVA;
                        }

                        //retenciones
                        var impuestoRetencion = json.TotalImpuestos;

                        if (Session["sRetencionesENC"] == null)
                        {
                            List<FacturacionManual.ImpuestoRetencion> listadoImpuestoRetencion = new List<FacturacionManual.ImpuestoRetencion>();

                            foreach (var item in impuestoRetencion)
                            {
                                if (item.CodigoImpuesto == "05" || item.CodigoImpuesto == "06" || item.CodigoImpuesto == "07")
                                {
                                    var impuesto = new FacturacionManual.ImpuestoRetencion
                                    {
                                        DescripcionRetencion = item.Descripcion,
                                        Base = item.Base,
                                        Descripcion = item.Nombre,
                                        Id = Guid.NewGuid(),
                                        Valor = item.Impuesto,
                                        PorcentajeRetencion = item.PorcentajeRetencion,
                                        CodigoImpuesto = item.CodigoImpuesto
                                    };

                                    listadoImpuestoRetencion.Add(impuesto);
                                }
                            }

                            Session["sRetencionesENC"] = listadoImpuestoRetencion;
                        }

                        var listadoRetencion = _clsFacturacion.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5"));
                        Session["facturaManualRetencionSession"] = listadoRetencion;
                        model.ListaRetenciones = listadoRetencion;
                        if (listadoRetencion.Any())
                        {
                            model.IdRetencion = Guid.Parse(listadoRetencion.FirstOrDefault().Value);
                            var tipoRetencion = _clsFacturacion.CargarListadoTipoRetencion(model.IdRetencion);
                            Session["facturaManualTipoRetencionSession"] = tipoRetencion;

                            if (tipoRetencion.Any())
                            {
                                if (model.IdTipoRetencion == Guid.Empty)
                                {
                                    model.IdTipoRetencion = tipoRetencion.FirstOrDefault().trf_id;
                                    model.PorcentajeRetencion = tipoRetencion.FirstOrDefault().trf_porcentajes;
                                }
                                ViewBag.IdTipoRetencion = (new SelectList(tipoRetencion, "trf_id", "trf_concepto_retencion", model.IdTipoRetencion)).ToList();
                            }
                            else
                            {
                                ViewBag.IdTipoRetencion = null;
                                model.PorcentajeRetencion = 0;
                            }
                        }
                        else
                        {
                            model.ListadoTipoRetenciones = null;
                            model.PorcentajeRetencion = 0;
                        }

                        if (Session["sCatalogoGeneralEc"] == null)
                        {
                            var cag = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6"));
                            model.ListadoCatalogoGeneral = cag;
                            Session["sCatalogoGeneralEc"] = cag;
                        }
                        else
                        {
                            model.ListadoCatalogoGeneral = Session["sCatalogoGeneralEc"] as List<Catalogo>;
                        }

                    }
                    else
                    {
                        model = new NotaCredito
                        {
                            Prefijo = model.Prefijo,
                            NumeroFactura = model.NumeroFactura,
                            ListadoConceptos = _clsNotaCredito.CargarComboConcepto(),
                            Concepto = model.Concepto,
                            ListadoCatalogoGeneral = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6")),
                            ListaRetenciones = _clsFacturacion.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5"))
                        };
                    }
                }
                else
                {
                    model = new NotaCredito
                    {
                        Prefijo = model.Prefijo,
                        NumeroFactura = model.NumeroFactura,
                        ListadoConceptos = _clsNotaCredito.CargarComboConcepto(),
                        Concepto = model.Concepto,
                        ListadoCatalogoGeneral = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6")),
                        ListaRetenciones = _clsFacturacion.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5"))

                    };
                    if (model.NumeroFactura != 0)
                    {
                        ViewBag.JsFuncion = $"mensaje('No se puede editar nota crédito {model.Prefijo.ToUpper()}{model.NumeroFactura}','warning')";
                    }
                }


                return View(model);

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult Editar(Guid id, string nombre)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var detalles = (List<NotaCredito.Detalle>)Session["sListadoDetallesEc"];

                NotaCredito.Detalle item = new NotaCredito.Detalle();
                if (id == Guid.Empty)
                    item = detalles.Where(x => x.Descripcion == nombre).FirstOrDefault();
                else
                    item = detalles.Where(x => x.IdDetalle == id).FirstOrDefault();

                var catalogo = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));

                foreach (var d in catalogo)
                {
                    if (item.ListadoDescripcion != null && item.ListadoDescripcion.Any())
                    {
                        foreach (var f in item.ListadoDescripcion)
                        {
                            if (d.Descripcion == f.Nombre)
                            {
                                d.Valor = f.Valor;
                            }
                        }
                    }
                }

                item.ListadoCatalogo = catalogo;

                var model = new NotaCredito.Detalle
                {
                    Cantidad = item.Cantidad,
                    Cargo = item.Cargo,
                    Codigo = item.Codigo,
                    Descuento = item.Descuento,
                    Descripcion = item.Descripcion,
                    Total = item.Total,
                    ValorImpuesto = item.ValorImpuesto,
                    ValorUnidad = item.ValorUnidad,
                    SubTotal = item.SubTotal,
                    Unidad = item.Unidad,
                    Impuesto = item.Impuesto,
                    ListadoImpuestos = _clsFacturacion.ListadoImpuestos(),
                    ListadoUnidad = _clsFacturacion.ListadoUnidades(),
                    Porcentaje = item.Porcentaje,
                    ListadoCatalogo = item.ListadoCatalogo
                };

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult GuardarDatosEditados(NotaCredito.Detalle detalle)
        {
            try
            {
                var lDetalle = (List<NotaCredito.Detalle>)Session["sListadoDetallesEc"];

                var item = lDetalle.Where(x => x.Codigo == detalle.Codigo).FirstOrDefault();

                item.Cantidad = detalle.Cantidad;
                item.Cargo = detalle.Cargo;
                item.CodigoImpuesto = _clsFacturacion.CargarCodigoImpuesto(detalle.Impuesto).codigo;
                item.Descripcion = detalle.Descripcion;
                item.Descuento = detalle.Descuento;
                item.SubTotal = detalle.SubTotal;
                item.Total = detalle.Total;
                item.Unidad = detalle.Unidad;
                item.Impuesto = detalle.Impuesto;
                item.ValorImpuesto = detalle.ValorImpuesto;
                item.ValorUnidad = detalle.ValorUnidad;
                item.Porcentaje = detalle.Porcentaje;

                Session["sListadoDetallesEc"] = lDetalle;

                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEc"];
                var model = new NotaCredito
                {
                    Prefijo = datosFactura.Prefijo,
                    NumeroFactura = datosFactura.NumeroFactura,
                    Concepto = datosFactura.Concepto
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesENC"] = retencion;

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult Eliminar(string id, string prefijo, decimal numeroFactura)
        {
            try
            {
                var detalles = (List<NotaCredito.Detalle>)Session["sListadoDetallesEc"];

                var item = detalles.Where(x => x.Codigo == id).FirstOrDefault();

                detalles.Remove(item);

                Session["sListadoDetallesEc"] = detalles;

                var model = new NotaCredito
                {
                    Prefijo = prefijo,
                    NumeroFactura = numeroFactura
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesENC"] = retencion;

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {

                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult Cancelar()
        {
            try
            {
                Session["sListadoDetallesEc"] = null;
                Session["sDatosFacturaEc"] = null;
                Session["sCatalogoGeneralEc"] = null;
                Session["sRetencionesENC"] = null;

                return RedirectToAction("Buscar", "FacturacionManual", null);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult BorrarSession()
        {
            try
            {
                Session["sListadoDetallesEc"] = null;
                Session["sDatosFacturaEc"] = null;
                Session["sCatalogoGeneralEc"] = null;
                Session["sRetencionesENC"] = null;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult ValidarValor(string prefijo, string numeroFactura, string total)
        {
            try
            {
                decimal numero = decimal.Parse(numeroFactura);
                decimal totaldec = decimal.Parse(total);

                var emisor = (Guid)Session["i_id_emisor"];
                string resultadoJson = _clsNotaCredito.ConsultarJsonFactura(emisor, prefijo, numero).json;

                if (resultadoJson != null)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultadoJson);

                    var totalJson = json.Totales.Total;

                    if (totalJson >= totaldec)
                    {
                        return Json(new { result = true, texto = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, texto = "Valor total no puede superar el valor anterior" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, texto = "Valor total no puede superar el valor anterior" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult EnviarDatosFactura(NotaCredito notaCredito)
        {
            try
            {
                var idemisor = (Guid)Session["i_id_emisor"];
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEc"];

                if (datosFactura.NumeroNotaCredito == 0 && datosFactura.PrefijoNotaCredito == null)
                {
                    string PrefijoNotaCredito = datosFactura.Prefijo;
                    decimal numeroNotaCredito = datosFactura.NumeroFactura;

                    datosFactura.NumeroNotaCredito = numeroNotaCredito;
                    datosFactura.PrefijoNotaCredito = PrefijoNotaCredito;

                    Session["sDatosFacturaEc"] = datosFactura;
                }

                var respuesta = ConsumirApi(notaCredito, "Comprobante", datosFactura.NumeroNotaCredito, datosFactura.PrefijoNotaCredito);

                if (respuesta.Contains("Procesado Correctamente"))
                {

                    var p = respuesta.IndexOf("Documento:");
                    var f = respuesta.IndexOf("Descripción:");
                    var len = f - p;
                    string numeroFactura = respuesta.Substring(p, len);

                    respuesta = "Nota credito creada satisfactoriamente. N°: " + numeroFactura;

                    string mensaje = respuesta;
                    string icon = "success";

                    TempData["mensajeFacturaOk"] = $"mensaje('{mensaje}','{icon}')";

                    Session["sListadoDetallesEc"] = null;
                    Session["sDatosFacturaEc"] = null;

                    return Json(new { isValid = false, data = "OK" });

                }
                else
                {
                    string facturas = $"{datosFactura.PrefijoNotaCredito.ToUpper() }{datosFactura.NumeroNotaCredito.ToString()} | ";

                    respuesta = facturas + respuesta;
                }

                var model = new Buscar
                {
                    CampoBuscar = respuesta
                };

                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoEditarNotaCredito", model) });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

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
        /// Se usa para consumir el api con el json que se genera a partir de los datos ingresados en el formulario
        /// </summary>
        /// <param name="facturacionManual"></param>
        /// <param name="metodoApi"></param>
        /// <returns></returns>
        public string ConsumirApi(NotaCredito notaCredito, string metodoApi, decimal numeroNotaCredito, string prefijoNotaCredito)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api"].ToString() + metodoApi;

                var json = JsonConvert.SerializeObject(ConstruirJson(notaCredito, numeroNotaCredito, prefijoNotaCredito));

                var jsonObjeto = JsonConvert.DeserializeObject<DocumentoModel>(json);
                _clsFacturacion.GuardarJson(json, jsonObjeto.Emisor.Identificacion?.Trim(),
                                                jsonObjeto.Emisor.Sucursal?.Trim(), jsonObjeto.Comprobante.TipoComprobante?.Trim(),
                                                jsonObjeto.Comprobante.Prefijo?.Trim(), jsonObjeto.Comprobante.Numero?.Trim());

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

        public DocumentoModel ConstruirJson(NotaCredito notaCredito, decimal numeroNotaCredito, string prefijoNotaCredito)
        {
            try
            {
                var documento = new DocumentoModel();
                var idSucursal = (Guid)Session["i_id_sucursal"];
                var idemisor = (Guid)Session["i_id_emisor"];
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEc"];



                string resultadoJson = _clsNotaCredito.ConsultarJsonFactura(idemisor, notaCredito.Prefijo, notaCredito.NumeroFactura).json;

                var datosNota = Session["sEditarNotaUmm"] as dynamic;

                if (resultadoJson != null)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultadoJson);
                    var cargardatosreceptor = _clsBuscar.CargarDatosReceptor(_clsBuscar.CargarGuidReceptor(json.Receptor.Identificacion.Value));


                    //COMPROBANTE
                    var comprobante = new Comprobante
                    {
                        OrigenDocumento = "AE436474-1101-46B8-BC4D-C83C23542B21",
                        TipoComprobante = datosNota.TipoDocumento,
                        Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                        Prefijo = prefijoNotaCredito,
                        Numero = numeroNotaCredito.ToString(),
                        Moneda = json.Comprobante.Moneda,
                        Referencia = json.Comprobante.Referencia,
                        ConceptoRef = _clsFacturacion.CargarCodigoConcepto(datosFactura.Concepto),
                        Observaciones = notaCredito.Observacion,
                        Usuario = Session["i_susuario"].ToString(),
                        NumeroOrden = datosFactura.OrdenCompra ?? "",
                        NumeroDespacho = datosFactura.Despacho ?? "",
                        NumeroRecepcion = datosFactura.Recepcion ?? "",
                        DocumentoAdicionalNotaCredito = "",
                        DocumentoReferenciaCodigo = "",
                        MetodoPago = new List<MetodoPago> { new MetodoPago { FormaPago = json.Comprobante.MetodoPago[0].FormaPago.Value, MedioPago = json.Comprobante.MetodoPago[0].MedioPago.Value, Fecha = DateTime.Now.ToString("yyyy-MM-dd") } },
                    };

                    //EMISOR
                    var datosEmisor = _clsFacturacion.CargarDatosEmisorSucursal(idSucursal);
                    var emisor = new Emisor
                    {
                        Ciudad = datosEmisor.Ciudad,
                        CiudadCodigo = datosEmisor.CiudadCodigo,
                        CodigoPostal = datosEmisor.CodigoPostal,
                        Departamento = datosEmisor.Departamento,
                        DepartamentoCodigo = datosEmisor.DepartamentoCodigo,
                        Descripcion = null,
                        DigitoVerificador = datosEmisor.DigitoVerificador,
                        Direccion = datosEmisor.Direccion,
                        email = datosEmisor.email,
                        Identificacion = datosEmisor.Identificacion,
                        NombreComercial = datosEmisor.NombreComercial,
                        NumeroMatriculaMercantil = datosEmisor.NumeroMatriculaMercantil,
                        Pais = datosEmisor.Pais,
                        PaisCodigo = datosEmisor.PaisCodigo,
                        RazonSocial = datosEmisor.RazonSocial,
                        Sucursal = datosEmisor.Sucursal,
                        Telefono = datosEmisor.Telefono,
                        TipoEmisor = datosEmisor.TipoEmisor,
                        TipoIdentificacion = datosEmisor.TipoIdentificacion,
                        TipoPersona = datosEmisor.TipoPersona
                    };


                    //CREDENCIALES
                    var credenciales = new Credenciales
                    {
                        AccessToken = datosEmisor.AccessToken,
                        ClientToken = datosEmisor.ClientToken
                    };

                    var datosReceptor = json.Receptor;

                    //RECEPTOR
                    var receptor = new Receptor
                    {
                        RazonSocial = cargardatosreceptor.RazonSocial,
                        Identificacion = cargardatosreceptor.NumeroIdentificacion,
                        DigitoVerificador = cargardatosreceptor.Digito,
                        email = cargardatosreceptor.Email,
                        Telefono = cargardatosreceptor.Telefono,
                        Direccion = cargardatosreceptor.Direccion,
                        Ciudad = cargardatosreceptor.TextMunicipio,
                        Departamento = cargardatosreceptor.TextDepartamento,
                        Pais = cargardatosreceptor.TextPais,
                        CiudadCodigo = cargardatosreceptor.CodigoMunicipio,
                        CodigoPostal = cargardatosreceptor.CodigoPostal,
                        DepartamentoCodigo = cargardatosreceptor.CodigoDepartamento,
                        Descripcion = null,
                        NombreComercial = cargardatosreceptor.Nombre,
                        NumeroMatriculaMercantil = null,
                        PaisCodigo = cargardatosreceptor.CodigoPais,
                        TipoIdentificacion = cargardatosreceptor.CodigoTipoIdentificacion,
                        TipoPersona = cargardatosreceptor.CodigoTipoPersona,
                        TipoReceptor = cargardatosreceptor.TipoAdquiriente,
                    };

                    //DETALLES
                    var detalles = new List<Detalles>();

                    var listadoDetalles = Session["sListadoDetallesEc"] as List<NotaCredito.Detalle>;

                    if (listadoDetalles != null && listadoDetalles.Any())
                    {
                        foreach (var item in listadoDetalles)
                        {
                            var impuestos = new List<Impuestos>();
                            var imp = new Impuestos();
                            //var inc = Guid.Parse("208B7394-9916-4346-9881-86C4A92A6B12");
                            //var iva5 = Guid.Parse("2A7E3DD4-4AB8-447C-9224-9F4AA46B9C84");
                            //var noCausa = Guid.Parse("35191913-56E3-4A18-8810-9FF64A2C6C35");
                            //var iva19 = Guid.Parse("E2D47731-FDB3-4F2A-A899-F220942BD60E");


                            var (codigo, porcentaje) = _clsFacturacion.CargarCodigoImpuesto(item.Impuesto);
                            //var listaIva = new List<Guid> { inc, iva5, noCausa, iva19 };
                            //var impuestos = new List<Impuestos>();
                            //for (int i = 0; i < listaIva.Count; i++)
                            //{
                            //    var (codigoIM, porcentajeIM, nombreIM) = _clsFacturacionManual.ListadoImpuestisIva(listaIva[i]);

                            //    var imp = new Impuestos();
                            //    if (codigoIM == codigo && porcentajeIM == porcentaje)
                            //    {
                            imp = new Impuestos
                            {
                                Base = string.Format("{0:N2}", item.SubTotal),
                                CodigoImpuesto = _clsFacturacion.CargarCodigoImpuestoC(item.Impuesto), //   item.Codigo, //  codigo,
                                Impuesto = string.Format("{0:N2}", item.ValorImpuesto),
                                Nombre = _clsFacturacion.CargarNombreImpuesto(item.Impuesto),
                                Porcentaje = string.Format("{0:N2}", porcentaje) //porcentaje
                            };
                            //}
                            //else
                            //{
                            //    imp = new Impuestos
                            //    {
                            //        Base = "0.00",
                            //        CodigoImpuesto = codigoIM,
                            //        Impuesto = "0.00",
                            //        Nombre = nombreIM,
                            //        Porcentaje = porcentajeIM.ToString()
                            //    };
                            //}

                            impuestos.Add(imp);
                            //}


                            var unidad = _clsFacturacion.CargarCodigoUnidad(item.Unidad);

                            var listaDetalleCatalogo = new List<Models.Descripcion>();


                            if (item.ListadoCatalogo != null && item.ListadoCatalogo.Any())
                            {
                                foreach (var itemCat in item.ListadoCatalogo)
                                {
                                    var catdetalle = new Models.Descripcion();

                                    catdetalle.Nombre = itemCat.Descripcion;
                                    catdetalle.Valor = itemCat.Valor;
                                    listaDetalleCatalogo.Add(catdetalle);

                                }
                            }

                            var det = new Detalles
                            {
                                Cantidad = item.Cantidad.ToString(),
                                codigo = item.Id.ToString(),
                                Descripcion = listaDetalleCatalogo,
                                Nombre = item.Descripcion,
                                idDetalle = Guid.NewGuid().ToString(),
                                Impuestos = impuestos,
                                Descuento = string.Format("{0:N2}", item.Descuento),
                                SubTotal = string.Format("{0:N2}", item.SubTotal),
                                Total = string.Format("{0:N2}", item.Total),
                                AplicaImpuesto = true,
                                UnidadCodigo = unidad,
                                ValorUnitario = string.Format("{0:N2}", item.ValorUnidad),
                                Cargos = string.Format("{0:N2}", item.Cargo),
                                AllowanceCharge = null,
                                PricingReference = null
                            };

                            detalles.Add(det);
                        }
                    }

                    var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                    if (Session["sRetencionesENC"] != null)
                    {
                        retencion = Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>;
                    }

                    var totales = listadoDetalles
                       .Select(x => new
                       {
                           x.SubTotal,
                           Descuentos = x.Descuento,
                           //Descuentos = 0,
                           x.Total,
                           Cargos = x.Cargo,
                           //Cargos = 0,
                           SubTotalSinCargosDescuentos = x.SubTotal + x.Descuento - x.Cargo,
                           //SubTotalSinCargosDescuentos = x.SubTotal,
                           IVA = x.ValorImpuesto
                       })
                       .GroupBy(x => new { })
                       .Select(d => new Totales
                       {
                           IVA = decimal.Round(d.Sum(x => x.IVA), 2).ToString(),
                           SubTotal = decimal.Round(d.Sum(x => x.SubTotal), 2).ToString(),
                           SubTotalSinCargosDescuentos = decimal.Round(d.Sum(x => x.SubTotal), 2).ToString()
                       }).FirstOrDefault();

                    totales.Cargos = decimal.Round(notaCredito.Cargo, 2).ToString();
                    totales.Descuentos = decimal.Round(notaCredito.Descuento, 2).ToString();
                    totales.Total = (decimal.Parse(totales.SubTotal) + decimal.Parse(totales.IVA) + decimal.Parse(totales.Cargos) - decimal.Parse(totales.Descuentos)).ToString();
                    totales.TotalEnLetras = $"{_convertNumeroToLetra.Enletras(totales.Total)} {_clsFacturacion.TipoMoneda(datosFactura.Moneda)}";
                    totales.TotalConRetencion = decimal.Round(decimal.Parse(totales.Total) - retencion.Sum(r => r.Valor), 2);

                    var listaAllowanceCharge = new List<Models.AllowanceCharge>();

                    if (notaCredito.Cargo > 0)
                    {
                        var allowanceCharge = new AllowanceCharge
                        {
                            ID = "1",
                            ChargeIndicator = true,
                            AllowanceChargeReasonCode = "",
                            AllowanceChargeReason = "Cargo Factura",
                            Amount = notaCredito.Cargo,
                            BaseAmount = decimal.Parse(totales.SubTotal),
                            MultiplierFactorNumeric = (decimal.Parse(totales.SubTotal) == 0 ? 0 : notaCredito.Cargo / decimal.Parse(totales.SubTotal)) * 100 > 100 ? 100 : (decimal.Parse(totales.SubTotal) == 0 ? 0 : notaCredito.Cargo / decimal.Parse(totales.SubTotal)) * 100
                        };

                        listaAllowanceCharge.Add(allowanceCharge);
                    }

                    if (notaCredito.Descuento > 0)
                    {
                        var allowanceCharge = new AllowanceCharge
                        {
                            ID = "2",
                            ChargeIndicator = false,
                            AllowanceChargeReasonCode = "11",
                            AllowanceChargeReason = "Descuento Factura",
                            Amount = notaCredito.Descuento,
                            BaseAmount = decimal.Parse(totales.SubTotal),
                            MultiplierFactorNumeric = (decimal.Parse(totales.SubTotal) == 0 ? 0 : notaCredito.Descuento / decimal.Parse(totales.SubTotal)) * 100 > 100 ? 100 : (decimal.Parse(totales.SubTotal) == 0 ? 0 : notaCredito.Descuento / decimal.Parse(totales.SubTotal)) * 100
                        };

                        listaAllowanceCharge.Add(allowanceCharge);
                    }

                    //TOTALIMPUESTO
                    var inc = Guid.Parse("208B7394-9916-4346-9881-86C4A92A6B12");
                    var iva5 = Guid.Parse("2A7E3DD4-4AB8-447C-9224-9F4AA46B9C84");
                    var noCausa = Guid.Parse("35191913-56E3-4A18-8810-9FF64A2C6C35");
                    var iva19 = Guid.Parse("E2D47731-FDB3-4F2A-A899-F220942BD60E");

                    var listaIva = new List<Guid> { inc, iva5, noCausa, iva19 };
                    var impuestosT = new List<TotalImpuestos>();
                    for (int i = 0; i < listaIva.Count; i++)
                    {
                        var (codigoIM, porcentajeIM, nombreIM) = _clsFacturacion.ListadoImpuestisIva(listaIva[i]);

                        var impuestoNew = new TotalImpuestos
                        {
                            Base = "0.0",
                            CodigoImpuesto = codigoIM,
                            Impuesto = "0.0",
                            Nombre = nombreIM,
                            Porcentaje = porcentajeIM.ToString()
                        };

                        impuestosT.Add(impuestoNew);
                    }



                    var totalImpuesto = new List<TotalImpuestos>();

                    //var listadoImpuestosGeneral = Session["sImpuestos"] as List<FacturacionManual.ImpuestoRetencion>;
                    //var listadoRetencionesGeneral = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>;
                    var listadoDetallesImpuestos = listadoDetalles
                        .Select(d => new
                        {
                            Base = d.SubTotal,
                            CodigoImpuesto = _clsFacturacion.CargarCodigoImpuesto(d.Impuesto).codigo,
                            Impuesto = d.ValorImpuesto,
                            Nombre = _clsFacturacion.CargarNombreImpuesto(d.Impuesto),
                            Porcentaje = _clsFacturacion.CargarCodigoImpuesto(d.Impuesto).porcentaje
                        })
                        .GroupBy(x => new { x.CodigoImpuesto, x.Porcentaje, x.Nombre })
                        .Select(d => new TotalImpuestos
                        {
                            Base = string.Format("{0:N2}", d.Sum(x => x.Base)),
                            CodigoImpuesto = d.Key.CodigoImpuesto,
                            Impuesto = string.Format("{0:N2}", d.Sum(x => x.Impuesto)),
                            Nombre = d.Key.Nombre,
                            Porcentaje = string.Format("{0:N2}", d.Key.Porcentaje)
                        }).ToList();


                    var listadoConsolidadoImpuesto = impuestosT.Where(x => !listadoDetallesImpuestos.Select(l => new { l.CodigoImpuesto, l.Porcentaje }).Contains(new { x.CodigoImpuesto, x.Porcentaje })).ToList();

                    var listadoCompuestoImp = listadoConsolidadoImpuesto.Union(listadoDetallesImpuestos).ToList();

                    //Retenciones            

                    var listadoRetenciones = new List<TotalImpuestos>();
                    if (retencion != null && retencion.Any())
                    {
                        foreach (var itemImp in retencion)
                        {
                            if (itemImp.PorcentajeRetencion > 0)
                            {
                                var newRetencion = new TotalImpuestos
                                {
                                    Base = decimal.Round((itemImp.Base), 2).ToString(),
                                    CodigoImpuesto = (itemImp.IdImRe == Guid.Empty ? itemImp.CodigoImpuesto : _clsFacturacion.CargarCodigoImpuesto(itemImp.IdImRe).codigo.ToString()),
                                    Impuesto = decimal.Round((itemImp.Valor), 2).ToString(),
                                    Nombre = itemImp.Descripcion,
                                    Porcentaje = decimal.Round((itemImp.PorcentajeRetencion), 2).ToString(),
                                    Descripcion = itemImp.DescripcionRetencion,
                                    PorcentajeRetencion = itemImp.PorcentajeRetencion
                                };
                                listadoRetenciones.Add(newRetencion);
                            }
                        }
                        totalImpuesto = listadoCompuestoImp.Union(listadoRetenciones).ToList();
                    }
                    else
                    {
                        totalImpuesto = listadoCompuestoImp;
                    }

                    //CATALOGO GENERAL
                    var catalogo = new List<DetallesComprobante>();

                    List<Catalogo> listadoCatalogoGeneral = Session["sCatalogoGeneralEd"] as List<Catalogo>;

                    if (listadoCatalogoGeneral != null && listadoCatalogoGeneral.Any())
                    {
                        foreach (var item in listadoCatalogoGeneral)
                        {
                            var catG = new DetallesComprobante
                            {
                                Nombre = item.Descripcion,
                                Valor = item.Valor
                            };

                            catalogo.Add(catG);
                        }
                    }


                    //TERMINO DE PAGO
                    var terminodepago = new TerminosPago
                    {
                        Codigo = "2",
                        UnidadCodigo = "DAY",
                        Duracion = Convert.ToString(json.TerminosPago.Duracion)
                    };

                    //PaymentExchangeRate
                    PaymentExchangeRate paymentExchangeRate = null;
                    if (datosFactura.Moneda == Guid.Parse("21EBA47A-3970-4519-8226-C607413412AB"))
                    {
                        var newPaymentExchangeRate = new PaymentExchangeRate
                        {
                            SourceCurrencyCode = _clsFacturacion.CargarCodigoMoneda(datosFactura.Moneda),
                            TargetCurrencyCode = "COP",
                            TargetCurrencyBaseRate = 1,
                            SourceCurrencyBaseRate = 1,
                            CalculationRate = datosFactura.Trm,
                            Date = datosFactura.FechaTrm.ToString("yyyy-MM-dd")
                        };

                        paymentExchangeRate = newPaymentExchangeRate;
                    }

                    //DOCUMENTOMODEL
                    documento = new DocumentoModel
                    {
                        Comprobante = comprobante,
                        Emisor = emisor,
                        Receptor = receptor,
                        Credenciales = credenciales,
                        PaymentExchangeRate = paymentExchangeRate,
                        Detalles = detalles,
                        Totales = totales,
                        TotalImpuestos = totalImpuesto,
                        DetallesComprobante = catalogo,
                        TerminosPago = terminodepago,
                        AllowanceCharge = listaAllowanceCharge
                    };
                }
                return documento;
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }

        public ActionResult CambioConcepto(Guid idConcepto)
        {
            try
            {
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEc"];

                datosFactura.Concepto = idConcepto;

                Session["sDatosFacturaEc"] = datosFactura;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ActualizarCatalogoGeneral(Guid idCatalogo, string valor)
        {
            try
            {
                var cag = Session["sCatalogoGeneralEc"] as List<Catalogo>;

                var catalogo = cag.Where(x => x.Id == idCatalogo).FirstOrDefault();

                catalogo.Valor = valor;

                Session["sCatalogoGeneralEc"] = cag;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        #region "Retenciones"
        public ActionResult ValidarRetenciones()
        {
            try
            {
                var retencion = Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>;

                if (retencion != null)
                {
                    return Json(new { data = retencion, total = retencion.Sum(s => s.Valor) });

                }
                else
                {
                    return Json(new { data = "", total = 0 });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult CargarPorcentajeImpuesto(Guid impuesto)
        {
            try
            {
                decimal porcentaje = _clsFacturacion.CargarPorcentajeImpuesto(impuesto);
                return Json(porcentaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult GuardarRetencionGeneral([Bind(Include = "IdRetencion, SubTotal, PorcentajeRetencion, IdTipoRetencion")] NotaCredito notaCredito)
        {
            try
            {
                if (Session["facturaManualRetencionSession"] != null)
                {
                    string sDescripcionRetencion = "";
                    if (Session["facturaManualTipoRetencionSession"] != null && ((List<DataBase.adm_tipo_retencion_fuente>)Session["facturaManualTipoRetencionSession"]).Any())
                    {
                        sDescripcionRetencion = ((List<DataBase.adm_tipo_retencion_fuente>)Session["facturaManualTipoRetencionSession"]).Where(tr => tr.trf_id == notaCredito.IdTipoRetencion).FirstOrDefault().trf_concepto_retencion;
                    }
                    var retencion = ((List<SelectListItem>)Session["facturaManualRetencionSession"]).Where(r => r.Value == notaCredito.IdRetencion.ToString()).FirstOrDefault();
                    var detalle = new ImpuestoRetencion
                    {
                        Id = Guid.NewGuid(),
                        IdImRe = notaCredito.IdRetencion,
                        Descripcion = retencion.Text,
                        Base = notaCredito.SubTotal,
                        Valor = (notaCredito.SubTotal * (notaCredito.PorcentajeRetencion / 100)),
                        PorcentajeRetencion = notaCredito.PorcentajeRetencion,
                        DescripcionRetencion = sDescripcionRetencion

                    };
                    List<ImpuestoRetencion> listadoRetencion = new List<ImpuestoRetencion>();
                    if (!(Session["sRetencionesENC"] == null))
                        listadoRetencion = (List<ImpuestoRetencion>)Session["sRetencionesENC"];
                    listadoRetencion.Add(detalle);
                    Session["sRetencionesENC"] = listadoRetencion;
                }
                return Json(new { data = Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult EliminarRetencionGeneral(Guid id)
        {
            try
            {
                var retencions = Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>;

                var rm = retencions.Where(x => x.Id == id).FirstOrDefault();

                retencions.Remove(rm);

                Session["sRetencionesENC"] = retencions;

                return Json(new { data = Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetencionesENC"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult CargarPorcentaje(Guid gTipoRetencion)
        {
            try
            {
                return Json(_clsFacturacion.CargarPorcentaje(gTipoRetencion));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}