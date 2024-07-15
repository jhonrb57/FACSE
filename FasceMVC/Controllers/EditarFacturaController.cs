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
    public class EditarFacturaController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NotaCreditoController).Name);
        private readonly ClsNotaCredito _clsNotaCredito = new ClsNotaCredito();
        private readonly ClsFacturacionManual _clsFacturacion = new ClsFacturacionManual();
        private readonly ConvertNumeroToLetra _convertNumeroToLetra = new ConvertNumeroToLetra();
        private readonly ClsBuscar _clsBuscar = new ClsBuscar();

        public ActionResult Index(NotaCredito model)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                if (Session["sDatosFacturaEd"] == null)
                {
                    var datosFactura = new NotaCredito.DatosFactura
                    {
                        NumeroFactura = model.NumeroFactura,
                        Prefijo = model.Prefijo
                    };

                    Session["sDatosFacturaEd"] = datosFactura;
                }
                else
                {
                    var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];
                    model.Prefijo = datosFactura.Prefijo;
                    model.NumeroFactura = datosFactura.NumeroFactura;
                    model.OrdenCompra = datosFactura.OrdenCompra;
                    model.Despacho = datosFactura.Despacho;
                    model.Recepcion = datosFactura.Recepcion;
                    model.Trm = datosFactura.Trm;
                    model.FechaTrm = datosFactura.FechaTrm;
                }

                if (_clsFacturacion.ValidarFacturaEditar(model.Prefijo, emisor, model.NumeroFactura))
                {

                    string resultadoJson = _clsNotaCredito.ConsultarJsonFactura(emisor, model.Prefijo, model.NumeroFactura).json;

                    if (resultadoJson != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(resultadoJson);

                        var receptor = _clsNotaCredito.ReceptorDocumento(model.Prefijo, emisor, model.NumeroFactura);

                        model.RazonSocial = receptor.rec_razon_social;
                        model.Identificacion = receptor.rec_identificacion;
                        model.ResFiscal = receptor.rec_tipo_receptor;
                        model.Digito = string.IsNullOrEmpty(receptor.rec_digito) ? 0 : int.Parse(receptor.rec_digito);
                        model.Correo = receptor.rec_correo;
                        model.Pais = receptor.sys_pais?.pai_nombre_comun;
                        model.Departamento = receptor.sys_departamento?.dep_nombre;
                        model.Ciudad = receptor.sys_municipio.mun_nombre;
                        model.Direccion = receptor.rec_direccion;
                        model.Telefono = receptor.rec_telefono;
                        Session["sEditarFactira_Receptor"] = receptor.rec_id;

                        model.Moneda = json.Comprobante.Moneda;
                        model.FormaPago = _clsNotaCredito.ConsultarFormaPago(json.Comprobante.MetodoPago[0].FormaPago.Value);
                        model.MedioPago = _clsNotaCredito.ConsultarMedioPago(json.Comprobante.MetodoPago[0].MedioPago.Value);
                        model.Observacion = json.Comprobante.Observaciones;

                        model.FechaTrm = json.Comprobante.Fecha;
                        model.gMoneda = _clsFacturacion.CargarGuidMoneda(json.Comprobante.Moneda.Value);
                        model.gFormaPago = _clsFacturacion.CargarGuidFormaPago(json.Comprobante.MetodoPago[0].FormaPago.Value);
                        model.gMedioPago = _clsFacturacion.CargarGuidMedioPago(json.Comprobante.MetodoPago[0].MedioPago.Value);

                        var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                        datosFactura.Moneda = _clsFacturacion.CargarGuidMoneda(json.Comprobante.Moneda.Value);
                        Session["sDatosFacturaEd"] = datosFactura;

                        if (model.Moneda == "USD")
                        {
                            model.Trm = json.PaymentExchangeRate.CalculationRate;
                            datosFactura.FechaTrm = (json.PaymentExchangeRate != null) ? json.PaymentExchangeRate?.Date ?? DateTime.Now : DateTime.Now;
                            datosFactura.Trm = (model.Moneda == "USD") ? (json.PaymentExchangeRate != null) ? json.PaymentExchangeRate.CalculationRate : 0 : 0;
                        }

                        var listaDetalles = new List<NotaCredito.Detalle>();
                        int i = 1;
                        foreach (var item in json.Detalles)
                        {
                            var des = new List<Descripcion>();

                            if (item.Descripcion != null)
                            {
                                foreach (var d in item.Descripcion)
                                {
                                    var newd = new Descripcion
                                    {
                                        Nombre = d.Nombre,
                                        Valor = d.Valor
                                    };
                                    des.Add(newd);
                                }
                            }


                            var detalle = new NotaCredito.Detalle
                            {
                                Id = i,
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
                            i++;
                        }
                        if (Session["sListadoDetallesEd"] != null)
                        {
                            var listaDetallesSession = (List<NotaCredito.Detalle>)Session["sListadoDetallesEd"];
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
                            Session["sListadoDetallesEd"] = listaDetalles;

                            model.Cargo = json.Totales.Cargos;
                            model.SubTotal = json.Totales.SubTotal;
                            model.Descuento = json.Totales.Descuentos;
                            model.Total = json.Totales.Total;
                            model.Impuesto = json.Totales.IVA;
                        }

                        if (Session["sCatalogoGeneralEd"] == null)
                        {
                            var cag = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6"));
                            model.ListadoCatalogoGeneral = cag;

                            if (json.DetallesComprobante != null)
                            {
                                foreach (var item in json.DetallesComprobante)
                                {
                                    foreach (var d in model.ListadoCatalogoGeneral)
                                    {
                                        if (item.Nombre == d.Descripcion)
                                        {
                                            d.Valor = item.Valor;
                                        }
                                    }
                                }
                            }


                            Session["sCatalogoGeneralEd"] = cag;
                        }
                        else
                        {
                            model.ListadoCatalogoGeneral = Session["sCatalogoGeneralEd"] as List<Catalogo>;
                        }

                        //retenciones

                        var impuestoRetencion = json.TotalImpuestos;

                        if (Session["sRetencionesEd"] == null)
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

                            Session["sRetencionesEd"] = listadoImpuestoRetencion;
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



                        //Moneda
                        model.ListadoMoneda = _clsFacturacion.CargarListadoMoneda();

                        if (model.gMoneda == Guid.Empty)
                        {
                            model.gMoneda = Guid.Parse(model.ListadoMoneda[0].Value);
                        }

                        //Forma pago
                        model.ListadoFormaPago = _clsFacturacion.CargarListadoFormaPago();

                        if (model.gFormaPago == Guid.Empty)
                        {
                            model.gFormaPago = Guid.Parse(model.ListadoFormaPago[0].Value);
                        }

                        //Medio pago
                        model.ListadoMedioPago = _clsFacturacion.CargarListadoMedioPago();

                        if (model.gMedioPago == Guid.Empty)
                        {
                            model.gMedioPago = Guid.Parse(model.ListadoMedioPago[0].Value);
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
                            ListaRetenciones = _clsFacturacion.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5")),
                            ListadoMoneda = _clsFacturacion.CargarListadoMoneda(),
                            ListadoFormaPago = _clsFacturacion.CargarListadoFormaPago(),
                            ListadoMedioPago = _clsFacturacion.CargarListadoMedioPago()
                        };
                    }
                }
                else
                {
                    string mensajeJs = "";
                    if (model.NumeroFactura != 0)
                    {
                        string mensaje = $"La factura {model.Prefijo.ToUpper()}{model.NumeroFactura} no se puede editar";
                        string icon = "warning";
                        mensajeJs = $"mensaje('{mensaje}','{icon}')";
                    }

                    model = new NotaCredito
                    {
                        Prefijo = model.Prefijo,
                        NumeroFactura = model.NumeroFactura,
                        ListadoConceptos = _clsNotaCredito.CargarComboConcepto(),
                        Concepto = model.Concepto,
                        ListadoCatalogoGeneral = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6")),
                        ListaRetenciones = _clsFacturacion.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5")),
                        ListadoMoneda = _clsFacturacion.CargarListadoMoneda(),
                        ListadoFormaPago = _clsFacturacion.CargarListadoFormaPago(),
                        ListadoMedioPago = _clsFacturacion.CargarListadoMedioPago(),
                        JsFuncion = mensajeJs
                    };
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EditarDetalleFactura(Guid id, string nombre)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var detallesFactura = Session["sListadoDetallesEd"] as List<NotaCredito.Detalle>;
                NotaCredito.Detalle detalle = new NotaCredito.Detalle();
                if (id == Guid.Empty)
                    detalle = detallesFactura.Where(x => x.Descripcion == nombre).FirstOrDefault();
                else
                    detalle = detallesFactura.Where(x => x.IdDetalle == id).FirstOrDefault();

                if (detalle != null)
                {
                    if (detalle.ListadoCatalogo != null && detalle.ListadoCatalogo.Any())
                    {
                        Session["sCatalogoDetalleEd"] = detalle.ListadoCatalogo;
                    }
                    else
                    {
                        var catalogo = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));

                        if (detalle.ListadoDescripcion != null && detalle.ListadoCatalogo.Any())
                        {
                            foreach (var d in catalogo)
                            {

                                foreach (var f in detalle.ListadoDescripcion)
                                {
                                    if (d.Descripcion == f.Nombre)
                                    {
                                        d.Valor = f.Valor;
                                    }
                                }
                            }
                        }

                        detalle.ListadoCatalogo = catalogo;
                        Session["sCatalogoDetalleEd"] = catalogo;
                    }
                }

                var model = new NotaCredito.Detalle
                {
                    Id = detalle.Id,
                    Cantidad = detalle.Cantidad,
                    Codigo = detalle.Codigo,
                    Descripcion = detalle.Descripcion,
                    Descuento = detalle.Descuento,
                    Cargo = detalle.Cargo,
                    Impuesto = detalle.Impuesto,
                    ListadoUnidad = _clsFacturacion.ListadoUnidades(),
                    ListadoImpuestos = _clsFacturacion.ListadoImpuestos(),
                    ListadoCatalogo = detalle.ListadoCatalogo,
                    Total = detalle.Total,
                    Unidad = detalle.Unidad,
                    ValorImpuesto = detalle.ValorImpuesto,
                    ValorUnidad = detalle.ValorUnidad,
                    Porcentaje = detalle.Porcentaje,
                };

                return View("EditarDetalle", model);

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult AgregarDetalleFactura(NotaCredito.Detalle detalle)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var model = new NotaCredito.Detalle { };

                var catalogo = _clsFacturacion.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));
                Session["sCatalogoDetalleEd"] = catalogo;

                model.ListadoUnidad = _clsFacturacion.ListadoUnidades();
                model.ListadoImpuestos = _clsFacturacion.ListadoImpuestos();
                model.ListadoCatalogo = Session["sCatalogoDetalleEd"] as List<Catalogo>;
                model.Id = 0;

                return View("EditarDetalle", model);

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Login");
            }
        }



        public ActionResult GuardarDatosEditados(NotaCredito.Detalle detalle)
        {
            try
            {
                var lDetalle = (List<NotaCredito.Detalle>)Session["sListadoDetallesEd"];

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

                Session["sListadoDetallesEd"] = lDetalle;

                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];
                var model = new NotaCredito
                {
                    Prefijo = datosFactura.Prefijo,
                    NumeroFactura = datosFactura.NumeroFactura,
                    Concepto = datosFactura.Concepto,
                    FechaTrm = datosFactura.FechaTrm,
                    Trm = datosFactura.Trm
                };

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }



        public ActionResult GuardarDetalleFactura(NotaCredito.Detalle detallesFacturacion)
        {
            try
            {
                var listadoDetalles = (Session["sListadoDetallesEd"] != null) ? Session["sListadoDetallesEd"] as List<NotaCredito.Detalle> : new List<NotaCredito.Detalle> { };

                if (detallesFacturacion.Id != 0)
                {
                    var detalle = listadoDetalles.Where(x => x.Id == detallesFacturacion.Id).FirstOrDefault();

                    detalle.Codigo = detallesFacturacion.Codigo;
                    detalle.Descripcion = detallesFacturacion.Descripcion;
                    detalle.Cantidad = detallesFacturacion.Cantidad;
                    detalle.ValorUnidad = detallesFacturacion.ValorUnidad;
                    detalle.Descuento = detallesFacturacion.Descuento;
                    detalle.Cargo = detallesFacturacion.Cargo;
                    detalle.ValorImpuesto = detallesFacturacion.ValorImpuesto;
                    detalle.SubTotal = detallesFacturacion.SubTotal;
                    detalle.Total = detallesFacturacion.Total;
                    detalle.ListadoCatalogo = Session["sCatalogoDetalleEd"] as List<Catalogo>;
                    detalle.Impuesto = detallesFacturacion.Impuesto;
                }
                else
                {
                    var maxid = ((listadoDetalles.Any()) ? listadoDetalles.Max(x => x.Id) : 0) + 1;

                    listadoDetalles.Add(new NotaCredito.Detalle
                    {
                        Id = maxid,
                        Cantidad = detallesFacturacion.Cantidad,
                        Codigo = detallesFacturacion.Codigo,
                        Descripcion = detallesFacturacion.Descripcion,
                        Descuento = detallesFacturacion.Descuento,
                        Cargo = detallesFacturacion.Cargo,
                        Impuesto = detallesFacturacion.Impuesto,
                        Porcentaje = detallesFacturacion.Porcentaje,
                        SubTotal = detallesFacturacion.SubTotal,
                        Total = detallesFacturacion.Total,
                        Unidad = detallesFacturacion.Unidad,
                        ValorImpuesto = detallesFacturacion.ValorImpuesto,
                        ValorUnidad = detallesFacturacion.ValorUnidad,
                        ListadoCatalogo = Session["sCatalogoDetalleEd"] as List<Catalogo>
                    });

                    Session["sCatalogoDetalleEd"] = null;
                }

                Session["sListadoDetallesEd"] = listadoDetalles;

                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];
                var model = new NotaCredito
                {
                    Prefijo = datosFactura.Prefijo,
                    NumeroFactura = datosFactura.NumeroFactura,
                    Concepto = datosFactura.Concepto,
                    FechaTrm = datosFactura.FechaTrm,
                    Trm = datosFactura.Trm
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesEd"] = retencion;

                return RedirectToAction("Index", "EditarFactura", model);
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
                var detalles = (List<NotaCredito.Detalle>)Session["sListadoDetallesEd"];

                var item = detalles.Where(x => x.Codigo == id).FirstOrDefault();

                detalles.Remove(item);

                Session["sListadoDetallesEd"] = detalles;

                var model = new NotaCredito
                {
                    Prefijo = prefijo,
                    NumeroFactura = numeroFactura
                };

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                Session["sRetencionesEd"] = retencion;
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
                Session["sListadoDetallesEd"] = null;
                Session["sDatosFacturaEd"] = null;
                Session["sCatalogoGeneralEd"] = null;
                Session["sCatalogoDetalleEd"] = null;
                Session["sRetencionesEd"] = null;
                Session["sImpuestosEd"] = null;

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
                Session["sListadoDetallesEd"] = null;
                Session["sDatosFacturaEd"] = null;
                Session["sCatalogoGeneralEd"] = null;
                Session["sCatalogoDetalleEd"] = null;
                Session["sRetencionesEd"] = null;
                Session["sImpuestosEd"] = null;

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

        public ActionResult AgregarODR(string valor, int tipo)
        {
            try
            {
                var basic = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                if (tipo == 1)
                {
                    basic.OrdenCompra = valor;
                }
                else if (tipo == 2)
                {
                    basic.Despacho = valor;
                }
                else if (tipo == 3)
                {
                    basic.Recepcion = valor;
                }

                Session["sDatosFacturaEd"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarModeda(Guid id)
        {
            try
            {
                var basic = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                basic.Moneda = id;

                Session["sDatosFacturaEd"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarTrm(decimal valor)
        {
            try
            {
                var basic = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                basic.Trm = valor;

                Session["sDatosFacturaEd"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarFecha(DateTime fecha)
        {
            try
            {
                var basic = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                basic.FechaTrm = fecha;

                Session["sDatosFacturaEd"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
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
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                if (datosFactura.NumeroNotaCredito == 0 && datosFactura.PrefijoNotaCredito == null)
                {
                    datosFactura.NumeroNotaCredito = datosFactura.NumeroFactura;
                    datosFactura.PrefijoNotaCredito = datosFactura.Prefijo;

                    Session["sDatosFacturaEd"] = datosFactura;
                }

                var respuesta = ConsumirApi(notaCredito, "Comprobante", datosFactura.NumeroNotaCredito, datosFactura.PrefijoNotaCredito);

                var estado = _clsFacturacion.ConsultarFacturaCreada(datosFactura.Prefijo, idemisor, datosFactura.NumeroFactura, DateTime.Now);

                if (estado)
                {
                    var p = respuesta.IndexOf("Documento:");
                    var f = respuesta.IndexOf("Descripción:");
                    var len = f - p;
                    string numeroFactura = respuesta.Substring(p, len);

                    respuesta = "Factura se edito satisfactoriamente. N°: " + numeroFactura;

                    string mensaje = respuesta;
                    string icon = "success";

                    TempData["mensajeFacturaOk"] = $"mensaje('{mensaje}','{icon}')";

                    Session["sListadoDetallesEd"] = null;
                    Session["sDatosFacturaEd"] = null;

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
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoFacturaEditar", model) });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                var model = new Buscar
                {
                    CampoBuscar = ex.ToString()
                };
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoFacturaEditar", model) });
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
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                string resultadoJson = _clsNotaCredito.ConsultarJsonFactura(idemisor, notaCredito.Prefijo, notaCredito.NumeroFactura).json;
                if (resultadoJson != null)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultadoJson);

                    var cargardatosreceptor = _clsBuscar.CargarDatosReceptor((Guid)Session["sEditarFactira_Receptor"]);

                    //COMPROBANTE
                    var comprobante = new Comprobante
                    {
                        OrigenDocumento = "AE436474-1101-46B8-BC4D-C83C23542B21",
                        TipoComprobante = "01",
                        Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                        Prefijo = prefijoNotaCredito,
                        Numero = numeroNotaCredito.ToString(),
                        Moneda = _clsFacturacion.CargarCodigoMoneda(datosFactura.Moneda),
                        Referencia = "",
                        ConceptoRef = "",
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

                    var listadoDetalles = Session["sListadoDetallesEd"] as List<NotaCredito.Detalle>;

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

                    if (Session["sRetencionesEd"] != null)
                    {
                        retencion = Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>;
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
                var datosFactura = (NotaCredito.DatosFactura)Session["sDatosFacturaEd"];

                datosFactura.Concepto = idConcepto;

                Session["sDatosFacturaEd"] = datosFactura;

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
                var cag = Session["sCatalogoGeneralEd"] as List<Catalogo>;

                var catalogo = cag.Where(x => x.Id == idCatalogo).FirstOrDefault();

                catalogo.Valor = valor;

                Session["sCatalogoGeneralEd"] = cag;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public bool ValidarImpuestoRetencion(Guid id, int tabla)
        {
            try
            {
                bool result = false;

                if (tabla == 1)//impuesto
                {
                    var impuesto = Session["sImpuestosEd"] as List<NotaCredito.ImpuestoRetencionD>;

                    if (impuesto != null)
                    {
                        var cantidad = impuesto.Where(x => x.IdImRe == id).Count();

                        if (cantidad > 0)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        result = true;
                    }
                }
                else if (tabla == 2)//retencion
                {
                    var retencion = Session["sRetencionesEd"] as List<NotaCredito.ImpuestoRetencionD>;

                    if (retencion != null)
                    {
                        var cantidad = retencion.Where(x => x.IdImRe == id).Count();

                        if (cantidad > 0)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        result = true;
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region GuardarTemporalFactura

        public ActionResult CargaTemporalEditar(int Consecutivo)
        {
            try
            {

                Session["sTemporalJson"] = null;
                var model1 = new BasicInformation();
                var model = new FacturacionManual();
                var emisor = (Guid)Session["i_id_emisor"];
                var sucursal = (Guid)Session["i_id_sucursal"];
                var FechaVencimiento = DateTime.Today.AddDays(30);
                string resultadoJson = _clsNotaCredito.CargarJsonTemporal(sucursal, Consecutivo);
                var listaDetalles = new List<FacturacionManual.Detalle>();
                int i = 1;

                if (resultadoJson != null)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultadoJson);

                    var receptor = _clsFacturacion.CargarDatosReceptor(json.Receptor.RazonSocial.Value, json.Receptor.Identificacion.Value, sucursal);
                    Session["sDatosReceptor"] = receptor;

                    model = new FacturacionManual
                    {
                        TipoDocumento = json.Comprobante.TipoComprobante,
                        Observacion = json.Comprobante.Observaciones,
                        NumeroOrden = json.Comprobante.NumeroOrden,
                        NumeroDespacho = json.Comprobante.NumeroDespacho,
                        NumeroRecepcion = json.Comprobante.NumeroRecepcion,
                        Moneda = _clsFacturacion.CargarGuidMoneda(json.Comprobante.Moneda.Value),
                        FormaPago = _clsFacturacion.CargarGuidFormaPago(json.Comprobante.MetodoPago[0].FormaPago.Value),
                        IdAdquiriente = receptor.IdReceptor,
                        Fecha = json.Comprobante.Fecha,
                        FechaVencimiento = FechaVencimiento,
                        OrdenCompra = json.Comprobante.NumeroOrden,
                        Despacho = json.Comprobante.NumeroDespacho,
                        Recepcion = json.Comprobante.NumeroRecepcion,
                        Observaciones = json.Comprobante.Observaciones,
                        IdEmisor = emisor,
                        IdReceptor = receptor.IdReceptor,
                        MedioPago = _clsFacturacion.CargarGuidMedioPago((json.Comprobante.MetodoPago[0].MedioPago.Value != null) ? json.Comprobante.MetodoPago[0].MedioPago.Value : "10"),

                    };
                    model1 = new BasicInformation
                    {
                        TipoDocumento = json.Comprobante.TipoComprobante,
                        Observacion = json.Comprobante.Observaciones,
                        Moneda = _clsFacturacion.CargarGuidMoneda(json.Comprobante.Moneda.Value),
                        FormaPago = _clsFacturacion.CargarGuidFormaPago((json.Comprobante.MetodoPago[0].FormaPago.Value != null) ? json.Comprobante.MetodoPago[0].FormaPago.Value : "1"),
                        IdAdquiriente = receptor.IdReceptor,
                        Fecha = json.Comprobante.Fecha,
                        OrdenCompra = json.Comprobante.NumeroOrden,
                        Despacho = json.Comprobante.NumeroDespacho,
                        Recepcion = json.Comprobante.NumeroRecepcion,
                        Observaciones = json.Comprobante.Observaciones,
                        IdEmisor = emisor,
                        IdReceptor = receptor.IdReceptor,
                        MedioPago = _clsFacturacion.CargarGuidMedioPago((json.Comprobante.MetodoPago[0].MedioPago.Value != null) ? json.Comprobante.MetodoPago[0].MedioPago.Value : "10"),
                        Trm = (json.PaymentExchangeRate != null && json.PaymentExchangeRate.SourceCurrencyCode == "USD") ? json.PaymentExchangeRate.CalculationRate : 0
                    };

                    int count = 0;
                    if (json.DetallesComprobante != null)
                    {

                        foreach (var item in json.DetallesComprobante)
                        {
                            if (item.Nombre == "Fecha Vencimiento")
                            {
                                count++;
                                model1.FechaVencimiento = item.Valor;
                            }
                        }
                    }
                    if (count == 0)
                        model1.FechaVencimiento = DateTime.Now;

                    Session["sDatosBasicos"] = model1;
                    foreach (var item in json.Detalles)
                    {
                        var des = new List<Catalogo>();
                        foreach (var d in item.Descripcion)
                        {
                            var newd = new Catalogo
                            {
                                Descripcion = d.Nombre,
                                Valor = d.Valor,
                            };
                            des.Add(newd);
                        }

                        var detalle = new FacturacionManual.Detalle
                        {
                            Id = i,
                            Cantidad = item.Cantidad,
                            Cargo = item.Cargos,
                            Codigo = item.codigo,
                            Descuento = item.Descuento,
                            Descripcion = item.Nombre,
                            DescripcionDetalles = item.Nombre,
                            Total = item.Total,
                            ValorImpuesto = decimal.Parse(item.Impuestos[0].Impuesto.Value),
                            ValorUnidad = item.ValorUnitario,
                            SubTotal = item.SubTotal,
                            CodigoImpuesto = item.Impuestos[0].CodigoImpuesto.Value,
                            Porcentaje = item.Impuestos[0].Porcentaje,
                            Unidad = _clsFacturacion.CargarGuidUnidad(Convert.ToString(item.UnidadCodigo)),
                            Impuesto = _clsFacturacion.CargarGuidImpuesto(Convert.ToString(item.Impuestos[0].CodigoImpuesto.Value), Convert.ToString(item.Impuestos[0].Nombre.Value)),
                            ListaCatalogo = des,
                            IdImpuesto = _clsFacturacion.CargarGuidImpuesto(Convert.ToString(item.Impuestos[0].CodigoImpuesto.Value), Convert.ToString(item.Impuestos[0].Nombre.Value)),
                        };
                        listaDetalles.Add(detalle);
                        i++;
                    }
                    Session["sDetallesFactura"] = listaDetalles;
                    model.ListadoDetalles = listaDetalles;

                    model.Cargo = json.Totales.Cargos;
                    model.Descuento = json.Totales.Descuentos;
                    model.Total = json.Totales.Total;
                    model.SubTotal = json.Totales.SubTotal;
                    model.Impuesto = json.Totales.IVA;
                    model.TotalGeneral = json.Totales.Total;
                    model.SubTotalGeneral = json.Totales.SubTotal;

                    //DETALLES IMPUESTO

                    var impuestoRetencion = json.TotalImpuestos;
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

                    Session["sRetenciones"] = listadoImpuestoRetencion;

                    var Impuesto = new List<Impuestos>();

                    foreach (var item in json.Detalles)
                    {
                        var Impuestos = new List<Impuestos>();
                        foreach (var d in item.Impuestos)
                        {
                            var guidimpuesto = _clsFacturacion.CargarGuidImpuesto(d.CodigoImpuesto.Value, d.Nombre.Value);
                            var impuesto = new Impuestos
                            {
                                Base = string.Format("{0:N2}", decimal.Parse(d.Base.Value)),
                                CodigoImpuesto = d.CodigoImpuesto.Value,
                                Impuesto = string.Format("{0:N2}", decimal.Parse(d.Impuesto.Value)),
                                Nombre = d.Nombre.Value,
                                Porcentaje = string.Format("{0:N2}", decimal.Parse(d.Porcentaje.Value))
                            };
                            Impuesto.Add(impuesto);
                        }
                    };
                    Session["sCatalogoDetalle"] = Impuesto;

                    //CATALOGO
                    var ListadoCatalogo = new List<Catalogo>();


                    foreach (var item in json.DetallesComprobante)
                    {
                        var catG = new Catalogo
                        {
                            Descripcion = item.Nombre,
                            Valor = item.Valor
                        };
                        ListadoCatalogo.Add(catG);
                    }
                    Session["sCatalogoGeneral"] = ListadoCatalogo;

                    //RETENCIONES
                    var Retenciones = new List<FacturacionManual.ImpuestoRetencion>();

                    foreach (var item in json.TotalImpuestos)
                    {
                        if (item.CodigoImpuesto.Value == "05" || item.CodigoImpuesto.Value == "06" || item.CodigoImpuesto.Value == "07")
                        {
                            //var idMax = ((Retenciones.Any()) ? Retenciones.Max(x => x.Id) : 0) + 1;
                            Guid idImpuesto = _clsFacturacion.CargarGuidImpuesto(item.CodigoImpuesto.Value, item.Nombre.Value);
                            var retenciones = new FacturacionManual.ImpuestoRetencion
                            {
                                Base = item.Base,
                                Descripcion = _clsFacturacion.CargarNombreImpuesto(idImpuesto),
                                //Id = idMax,
                                IdImRe = idImpuesto,
                                //Porcentaje = item.Porcentaje,
                                Valor = item.Impuesto,
                            };
                            Retenciones.Add(retenciones);
                        }
                    }
                    Session["sRetencionesEd"] = Retenciones;
                }

                Session["sTemporalJson"] = Consecutivo;
                return RedirectToAction("Index", "FacturacionManual", model);
            }

            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        #endregion

        #region "Retenciones"
        public ActionResult ValidarRetenciones()
        {
            try
            {
                var retencion = Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>;

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
                        List<Detalle> listadoDetalle = Session["sListadoDetallesEd"] as List<Detalle>;
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
                    if (!(Session["sRetencionesEd"] == null))
                        listadoRetencion = (List<ImpuestoRetencion>)Session["sRetencionesEd"];
                    listadoRetencion.Add(detalle);
                    Session["sRetencionesEd"] = listadoRetencion;
                }
                return Json(new { data = Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

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
                var retencions = Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>;

                var rm = retencions.Where(x => x.Id == id).FirstOrDefault();

                retencions.Remove(rm);

                Session["sRetencionesEd"] = retencions;

                return Json(new { data = Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetencionesEd"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

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