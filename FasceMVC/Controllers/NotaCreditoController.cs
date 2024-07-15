using DataBase;
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
using System.Web.Mvc;
using static FasceMVC.Models.FacturacionManual;

namespace FasceMVC.Controllers
{
    public class NotaCreditoController : BaseController
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(NotaCreditoController).Name);
        private readonly ClsNotaCredito _clsNotaCredito = new ClsNotaCredito();
        private readonly ClsFacturacionManual _clsFacturacion = new ClsFacturacionManual();
        private readonly ConvertNumeroToLetra _convertNumeroToLetra = new ConvertNumeroToLetra();
        private readonly ClsConfiguracion _clsConfiguracion = new ClsConfiguracion();
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


                if (Session["sNotaUmm"] != null)
                {
                    var dNota = Session["sNotaUmm"] as dynamic;
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
                    Session["sNotaUmm"] = newNota;
                    ViewBag.Titulo = titulo;
                }

                if (Session["sDatosFactura"] == null)
                {
                    var datosFactura = new NotaCredito.DatosFactura
                    {
                        Concepto = model.Concepto,
                        NumeroFactura = model.NumeroFactura,
                        Prefijo = model.Prefijo
                    };

                    Session["sDatosFactura"] = datosFactura;
                }
                else
                {
                    var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];
                    model.Prefijo = datosFactura.Prefijo;
                    model.NumeroFactura = datosFactura.NumeroFactura;
                    model.Concepto = datosFactura.Concepto;
                    model.FechaTrm = datosFactura.FechaTrm;
                    model.Trm = datosFactura.Trm;
                }

                if (_clsNotaCredito.ValidarNotaCredito(model.Prefijo, emisor, model.NumeroFactura))
                {
                    var resultadoConsultaJson = _clsNotaCredito.ConsultarJsonFactura(emisor, model.Prefijo, model.NumeroFactura);
                    string resultadoJson = resultadoConsultaJson.json;


                    if (Session["sDatosFactura"] != null)
                    {
                        var datosFac = Session["sDatosFactura"] as NotaCredito.DatosFactura;

                        ViewBag.Origen = Guid.Parse(resultadoConsultaJson.origen.ToString());

                    }

                    if (resultadoJson != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(resultadoJson);

                        var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];
                        datosFactura.OrdenCompra = json.Comprobante.NumeroOrden;
                        datosFactura.Despacho = json.Comprobante.NumeroDespacho;
                        datosFactura.Recepcion = json.Comprobante.NumeroRecepcion;
                        model.OrdenCompra = json.Comprobante.NumeroOrden;
                        model.Despacho = json.Comprobante.NumeroDespacho;
                        model.Recepcion = json.Comprobante.NumeroRecepcion;
                        datosFactura.FechaTrm = json.Comprobante.Fecha;
                        datosFactura.Trm = (model.Moneda == "USD") ? json.PaymentExchangeRate.CalculationRate : 0;

                        if (model.Moneda == "USD")
                        {
                            model.Trm = json.PaymentExchangeRate.CalculationRate;
                        }

                        datosFactura.Moneda = _clsFacturacion.CargarGuidMoneda(json.Comprobante.Moneda.Value);
                        Session["sDatosFactura"] = datosFactura;

                        var receptor = _clsNotaCredito.ReceptorDocumento(model.Prefijo, emisor, model.NumeroFactura);

                        if (receptor.rec_pais != null && receptor.rec_departamento != null && receptor.rec_municipio != null)
                        {
                            model.RazonSocial = receptor.rec_razon_social;
                            model.Identificacion = receptor.rec_identificacion;
                            model.ResFiscal = receptor.rec_tipo_receptor;

                            model.Digito = string.IsNullOrEmpty(receptor.rec_digito) ? 0 : int.Parse(receptor.rec_digito);

                            //model.Digito = json.Receptor.DigitoVerificador;
                            model.Correo = receptor.rec_correo;
                            model.Pais = receptor.sys_pais?.pai_nombre_comun;
                            model.Departamento = receptor.sys_departamento?.dep_nombre;
                            model.Ciudad = receptor.sys_municipio?.mun_nombre;
                            model.Direccion = receptor.rec_direccion;
                            model.Telefono = receptor.rec_telefono;
                            Session["sNotaCredito_Receptor"] = receptor.rec_id;


                            model.FechaTrm = json.Comprobante.Fecha;
                            model.Moneda = json.Comprobante.Moneda;
                            model.FormaPago = _clsNotaCredito.ConsultarFormaPago(json.Comprobante.MetodoPago[0].FormaPago.Value);
                            model.MedioPago = _clsNotaCredito.ConsultarMedioPago(json.Comprobante.MetodoPago[0].MedioPago.Value);

                            model.ListadoConceptos = _clsNotaCredito.CargarComboConcepto();
                            if (model.Concepto == Guid.Empty)
                            {
                                model.Concepto = Guid.Parse(model.ListadoConceptos[0].Value);
                            }

                            var listaDetalles = new List<NotaCredito.Detalle>();

                            var dNota = Session["sNotaUmm"] as dynamic;

                            if (dNota.TipoDocumento != "92")
                            {
                                foreach (var item in json.Detalles)
                                {
                                    var des = new List<Models.Descripcion>();
                                    if (item.Descripcion != null)
                                    {
                                        foreach (var d in item.Descripcion)
                                        {
                                            var newd = new Models.Descripcion
                                            {
                                                Nombre = d.Nombre,
                                                Valor = d.Valor
                                            };
                                            des.Add(newd);
                                        }
                                    }

                                    var detalle = new NotaCredito.Detalle
                                    {
                                        IdDetalle = item.idDetalle,
                                        Cantidad = item.Cantidad,
                                        Cargo = item.Cargos,
                                        Codigo = item.codigo,
                                        Descuento = item.Descuento,
                                        Descripcion = item.Nombre,
                                        Total = item.Total,
                                        ValorImpuesto = ((bool)item.AplicaImpuesto) ? decimal.Parse((item.Impuestos[0].Impuesto.Value == "") ? "0" : item.Impuestos[0].Impuesto.Value) : 0,
                                        ValorUnidad = item.ValorUnitario,
                                        SubTotal = item.SubTotal,
                                        CodigoImpuesto = ((bool)item.AplicaImpuesto) ? item.Impuestos[0].CodigoImpuesto.Value : null,
                                        Porcentaje = ((bool)item.AplicaImpuesto) ? item.Impuestos[0].Porcentaje : 0,
                                        Unidad = _clsFacturacion.CargarGuidUnidad(Convert.ToString(item.UnidadCodigo)),
                                        Impuesto = ((bool)item.AplicaImpuesto) ? _clsFacturacion.CargarGuidImpuesto(Convert.ToString(item.Impuestos[0].CodigoImpuesto.Value), Convert.ToString(item.Impuestos[0].Nombre.Value)) : Guid.Empty,
                                        ListadoDescripcion = des,
                                        AplicaImpuesto = (bool)item.AplicaImpuesto
                                    };


                                    listaDetalles.Add(detalle);
                                }
                            }
                            if (Session["sListadoDetalles"] != null)
                            {
                                var listaDetallesSession = (List<NotaCredito.Detalle>)Session["sListadoDetalles"];
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
                                Session["sListadoDetalles"] = listaDetalles;

                                //if (Guid.Parse(resultadoConsultaJson.origen.ToString()) == Guid.Parse("AE436474-1101-46B8-BC4D-C83C23542B21"))
                                //{
                                //    model.Impuesto = listaDetalles.Sum(x => x.ValorImpuesto);
                                //    model.Cargo = json.Totales.Cargo;
                                //    model.SubTotal = listaDetalles.Sum(x => x.SubTotal);
                                //    model.Descuento = json.Totales.Descuento;
                                //    model.Total = listaDetalles.Sum(x => x.ValorImpuesto) + model.SubTotal;
                                //}
                                //else
                                //{
                                model.Impuesto = listaDetalles.Sum(x => x.ValorImpuesto);
                                model.SubTotal = json.Totales.SubTotal;
                                model.Cargo = json.Totales.Cargos;
                                model.Descuento = json.Totales.Descuentos;
                                model.Total = json.Totales.Total;
                                //}
                                //model.SubTotal = json.Totales.SubTotal;
                                //model.Cargo = json.Totales.Cargos;
                                //model.Descuento = json.Totales.Descuentos;
                                //model.Total = json.Totales.Total;

                                //model.Cargo = listaDetalles.Sum(x => x.Cargo);
                                //model.SubTotal = json.Totales.SubTotal;
                                ////model.SubTotal = listaDetalles.Sum(x => x.SubTotal);
                                //model.Descuento = listaDetalles.Sum(x => x.Descuento);
                                //model.Total = listaDetalles.Sum(x => x.ValorImpuesto) + model.SubTotal;
                            }

                            //retenciones
                            var impuestoRetencion = json.TotalImpuestos;

                            if (Session["sRetencionesNC"] == null)
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

                                Session["sRetencionesNC"] = listadoImpuestoRetencion;
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

                            if (Session["sCatalogoGeneral"] == null)
                            {
                                var cag = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6"));
                                model.ListadoCatalogoGeneral = cag;
                                Session["sCatalogoGeneral"] = cag;
                            }
                            else
                            {
                                model.ListadoCatalogoGeneral = Session["sCatalogoGeneral"] as List<Catalogo>;
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
                            ViewBag.JsFuncion = $"mensaje('El receptor {receptor.rec_identificacion} - {receptor.rec_nombre} le fata pais o departamento o municipio','warning')";

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
                        ViewBag.JsFuncion = $"mensaje('No se puede crear nota crédito para la factura {model.Prefijo.ToUpper()}{model.NumeroFactura}','warning')";
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
                var detalles = (List<NotaCredito.Detalle>)Session["sListadoDetalles"];
                NotaCredito.Detalle item = new NotaCredito.Detalle();
                if (id == Guid.Empty)
                    item = detalles.Where(x => x.Descripcion == nombre).FirstOrDefault();
                else
                    item = detalles.Where(x => x.IdDetalle == id).FirstOrDefault();

                var catalogo = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));

                foreach (var d in catalogo)
                {
                    foreach (var f in item.ListadoDescripcion)
                    {
                        if (d.Descripcion == f.Nombre)
                        {
                            d.Valor = f.Valor;
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
                    //ListadoImpuestos = _clsFacturacion.ListadoImpuestos(),
                    ListadoImpuestos = _clsConfiguracion.ListadoImpuestoAsignado(emisor),
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
                var lDetalle = (List<NotaCredito.Detalle>)Session["sListadoDetalles"];

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

                Session["sListadoDetalles"] = lDetalle;

                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];
                var model = new NotaCredito
                {
                    Prefijo = datosFactura.Prefijo,
                    NumeroFactura = datosFactura.NumeroFactura,
                    Concepto = datosFactura.Concepto
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesNC"] = retencion;

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult GuardarDatosNew(NotaCredito.Detalle detalle)
        {
            try
            {
                var listadoDetalles = (Session["sListadoDetalles"] != null) ? Session["sListadoDetalles"] as List<NotaCredito.Detalle> : new List<NotaCredito.Detalle>();

                var item = new List<NotaCredito.Detalle>
                {
                    new NotaCredito.Detalle
                    {
                        Codigo=detalle.Codigo,
                        Cantidad = detalle.Cantidad,
                        Cargo = detalle.Cargo,
                        CodigoImpuesto = _clsFacturacion.CargarCodigoImpuesto(detalle.Impuesto).codigo,
                        Descripcion = detalle.Descripcion,
                        Descuento = detalle.Descuento,
                        SubTotal = detalle.SubTotal,
                        Total = detalle.Total,
                        Unidad = detalle.Unidad,
                        Impuesto = detalle.Impuesto,
                        ValorImpuesto = detalle.ValorImpuesto,
                        ValorUnidad = detalle.ValorUnidad,
                        Porcentaje = detalle.Porcentaje,
                        AplicaImpuesto=true
                    }
                };

                if (listadoDetalles.Any())
                {
                    listadoDetalles.AddRange(item);
                    Session["sListadoDetalles"] = listadoDetalles;
                }
                else
                {
                    Session["sListadoDetalles"] = item;
                }


                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];
                var model = new NotaCredito
                {
                    Prefijo = datosFactura.Prefijo,
                    NumeroFactura = datosFactura.NumeroFactura,
                    Concepto = datosFactura.Concepto
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesNC"] = retencion;

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult Eliminar(Guid id, string nombre, string prefijo, decimal numeroFactura)
        {
            try
            {
                var detalles = (List<NotaCredito.Detalle>)Session["sListadoDetalles"];

                NotaCredito.Detalle item = new NotaCredito.Detalle();


                if (id == Guid.Empty)
                    item = detalles.Where(x => x.Descripcion == nombre).FirstOrDefault();
                else
                    item = detalles.Where(x => x.IdDetalle == id).FirstOrDefault();

                //var item = detalles.Where(x => x.Codigo == id).FirstOrDefault();

                detalles.Remove(item);

                Session["sListadoDetalles"] = detalles;

                var model = new NotaCredito
                {
                    Prefijo = prefijo,
                    NumeroFactura = numeroFactura
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesNC"] = retencion;

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
                Session["sListadoDetalles"] = null;
                Session["sDatosFactura"] = null;
                Session["sCatalogoGeneral"] = null;
                Session["sRetencionesNC"] = null;

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
                Session["sListadoDetalles"] = null;
                Session["sDatosFactura"] = null;
                Session["sCatalogoGeneral"] = null;
                Session["sRetencionesNC"] = null;

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


                    var dNota = Session["sNotaUmm"] as dynamic;

                    if (totalJson >= totaldec || dNota.TipoDocumento == "92")
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

        [HttpPost]
        public ActionResult EnviarDatosFactura(NotaCredito notaCredito)
        {
            try
            {
                var idSucursal = (Guid)Session["i_id_sucursal"];
                var idemisor = (Guid)Session["i_id_emisor"];
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];

                var datosNota = Session["sNotaUmm"] as dynamic;

                if (datosFactura.NumeroNotaCredito == 0 && datosFactura.PrefijoNotaCredito == null)
                {
                    string tipoDoc = (datosNota.TipoDocumento == "91") ? "7E1E050B-B6F4-4B71-A3D9-1236FA5B2D27" : "D06A400C-AA35-4095-BD5C-8D297532464B";

                    string PrefijoNotaCredito = _clsFacturacion.CargarPrefijo(idemisor, DateTime.Now, tipoDoc, idSucursal);
                    decimal numeroNotaCredito = _clsFacturacion.CargarNumerofactura(idemisor, DateTime.Now, tipoDoc, idSucursal);

                    datosFactura.NumeroNotaCredito = numeroNotaCredito;
                    datosFactura.PrefijoNotaCredito = PrefijoNotaCredito;

                    Session["sDatosFactura"] = datosFactura;
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

                    Session["sListadoDetalles"] = null;
                    Session["sDatosFactura"] = null;

                    return Json(new { isValid = false, data = "OK" });

                }
                else
                {
                    string facturas = $"{datosFactura.PrefijoNotaCredito }{datosFactura.NumeroNotaCredito.ToString()} | ";

                    respuesta = facturas + respuesta;
                }

                var model = new Buscar
                {
                    CampoBuscar = respuesta
                };
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoNotaCredito", model) });

            }
            catch (CustomException ex)
            {
                var model = new Buscar
                {
                    CampoBuscar = ex.Message
                };
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoNotaCredito", model) });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                var model = new Buscar
                {
                    CampoBuscar = ex.ToString()
                };
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoNotaCredito", model) });
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
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];

                string resultadoJson = _clsNotaCredito.ConsultarJsonFactura(idemisor, notaCredito.Prefijo, notaCredito.NumeroFactura).json;

                var datosNota = Session["sNotaUmm"] as dynamic;

                if (resultadoJson != null)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultadoJson);
                    var cargardatosreceptor = _clsBuscar.CargarDatosReceptor((Guid)Session["sNotaCredito_Receptor"]);

                    //COMPROBANTE
                    var comprobante = new Comprobante
                    {
                        OrigenDocumento = "AE436474-1101-46B8-BC4D-C83C23542B21",
                        TipoComprobante = datosNota.TipoDocumento,
                        Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                        Prefijo = prefijoNotaCredito,
                        Numero = numeroNotaCredito.ToString(),
                        Moneda = json.Comprobante.Moneda,
                        Referencia = datosFactura.Prefijo.ToUpper() + datosFactura.NumeroFactura.ToString(),
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

                    var listadoDetalles = Session["sListadoDetalles"] as List<NotaCredito.Detalle>;

                    if (listadoDetalles == null || !listadoDetalles.Any())
                    {
                        throw new Exception("DEBE INGRESAR DETALLES");
                    }
                    if (listadoDetalles != null && listadoDetalles.Any())
                    {
                        foreach (var item in listadoDetalles)
                        {
                            var impuestos = new List<Impuestos>();
                            var imp = new Impuestos();

                            var (codigo, porcentaje) = _clsFacturacion.CargarCodigoImpuesto(item.Impuesto);


                            if (item.AplicaImpuesto)
                            {
                                imp = new Impuestos
                                {
                                    Base = decimal.Round(item.SubTotal, 2).ToString(),
                                    CodigoImpuesto = _clsFacturacion.CargarCodigoImpuestoC(item.Impuesto), //   item.Codigo, //  codigo,
                                    Impuesto = decimal.Round(item.ValorImpuesto, 2).ToString(),
                                    Nombre = _clsFacturacion.CargarNombreImpuesto(item.Impuesto),
                                    Porcentaje = decimal.Round(porcentaje, 2).ToString() //porcentaje
                                };

                                impuestos.Add(imp);

                            }

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
                                AplicaImpuesto = item.AplicaImpuesto,
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

                    if (Session["sRetencionesNC"] != null)
                    {
                        retencion = Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>;
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
                    //totales.TotalEnLetras = $"{_convertNumeroToLetra.Enletras(totales.Total)} {_clsFacturacion.TipoMoneda(datosFactura.Moneda)}";
                    totales.TotalConRetencion = decimal.Round(decimal.Parse(totales.Total) - retencion.Sum(r => r.Valor), 2);
                    totales.TotalEnLetras = $"{_convertNumeroToLetra.Enletras((decimal.Parse(totales.Total) - retencion.Sum(r => r.Valor)).ToString())} {_clsFacturacion.TipoMoneda(datosFactura.Moneda)}";

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
                        .Where(x => x.Impuesto != Guid.Empty && x.Porcentaje != 0)
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
                            Base = d.Sum(x => x.Base).ToString(),
                            CodigoImpuesto = d.Key.CodigoImpuesto,
                            Impuesto = d.Sum(x => x.Impuesto).ToString(),
                            Nombre = d.Key.Nombre,
                            Porcentaje = d.Key.Porcentaje.ToString()
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

                    List<Catalogo> listadoCatalogoGeneral = Session["sCatalogoGeneral"] as List<Catalogo>;

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
                        Duracion = Convert.ToString((json.TerminosPago != null) ? json.TerminosPago.Duracion : 0)
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
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFactura"];

                datosFactura.Concepto = idConcepto;

                Session["sDatosFactura"] = datosFactura;

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
                var cag = Session["sCatalogoGeneral"] as List<Catalogo>;

                var catalogo = cag.Where(x => x.Id == idCatalogo).FirstOrDefault();

                catalogo.Valor = valor;

                Session["sCatalogoGeneral"] = cag;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult AgregarDetalleFactura()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                //var emisor = "507F036A-7C1F-4436-AE2C-917590881064";

                var model = new NotaCredito.Detalle
                {
                    //ListadoImpuestos = _clsFacturacion.ListadoImpuestos(),
                    ListadoImpuestos = _clsConfiguracion.ListadoImpuestoAsignado(emisor),
                    ListadoUnidad = _clsFacturacion.ListadoUnidades(),
                    ListadoCatalogo = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"))
                };

                return View("Agregar", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Login");
            }
        }

        #region "Retenciones"
        public ActionResult ValidarRetenciones()
        {
            try
            {
                var retencion = Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>;

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
                    decimal valor = 0;
                    if (notaCredito.IdRetencion == Guid.Parse("ED7B87C5-8AB4-41DC-BE0C-AF7EA28CE79E"))
                    {
                        List<Detalle> listadoDetalle = Session["sListadoDetalles"] as List<Detalle>;
                        if (listadoDetalle != null && listadoDetalle.Any())
                        {
                            valor = (listadoDetalle.Where(x => _clsFacturacion.ListadoImpuestoIva().Contains(x.IdImpuesto)).Sum(x => x.ValorImpuesto)) * (notaCredito.PorcentajeRetencion / 100);
                        }
                    }
                    else
                    {
                        valor = (notaCredito.SubTotal * (notaCredito.PorcentajeRetencion / 100));

                    }
                    var detalle = new ImpuestoRetencion
                    {
                        Id = Guid.NewGuid(),
                        IdImRe = notaCredito.IdRetencion,
                        Descripcion = retencion.Text,
                        Base = notaCredito.SubTotal,
                        Valor = valor,
                        PorcentajeRetencion = notaCredito.PorcentajeRetencion,
                        DescripcionRetencion = sDescripcionRetencion

                    };
                    List<ImpuestoRetencion> listadoRetencion = new List<ImpuestoRetencion>();
                    if (!(Session["sRetencionesNC"] == null))
                        listadoRetencion = (List<ImpuestoRetencion>)Session["sRetencionesNC"];
                    listadoRetencion.Add(detalle);
                    Session["sRetencionesNC"] = listadoRetencion;
                }
                return Json(new { data = Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

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
                var retencions = Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>;

                var rm = retencions.Where(x => x.Id == id).FirstOrDefault();

                retencions.Remove(rm);

                Session["sRetencionesNC"] = retencions;

                return Json(new { data = Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetencionesNC"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

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