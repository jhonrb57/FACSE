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
    public class FacturacionManualController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FacturacionManualController).Name);
        private readonly ClsFacturacionManual _clsFacturacionManual = new ClsFacturacionManual();
        private readonly ClsBuscar _clsBuscar = new ClsBuscar();
        private readonly ConvertNumeroToLetra _convertNumeroToLetra = new ConvertNumeroToLetra();
        private readonly ClsNotaCredito _clsNotaCredito = new ClsNotaCredito();
        private readonly ClsConfiguracion _clsConfiguracion = new ClsConfiguracion();

        public ActionResult Index(FacturacionManual facturacion)
        {
            try
            {
                if (Session["sTemporalJson"] == null)
                {
                    Session["sTemporalJson"] = 0;
                }

                var emisor = (Guid)Session["i_id_emisor"];
                facturacion.IdEmisor = emisor;

                if (Session["sDatosReceptor"] == null)
                {
                    Session["sDetallesFactura"] = null;
                    Session["sCatalogoDetalle"] = null;
                    Session["sCatalogoGeneral"] = null;
                    Session["sImpuestos"] = null;
                    Session["sRetenciones"] = null;
                    Session["sTotalGeneral"] = null;
                    Session["sDatosBasicos"] = null;
                    var receptorDatos = _clsBuscar.CargarDatosReceptor(facturacion.IdAdquiriente);
                    Session["sDatosReceptor"] = receptorDatos;
                }
                else
                {
                    facturacion.ListadoDetalles = Session["sDetallesFactura"] as List<Detalle>;
                    if (facturacion.ListadoDetalles != null && facturacion.ListadoDetalles.Any())
                    {
                        facturacion.Impuesto = decimal.Round(facturacion.ListadoDetalles.Sum(x => x.ValorImpuesto), 2);
                        facturacion.SubTotalGeneral = Decimal.Round(facturacion.ListadoDetalles.Sum(x => x.SubTotal), 2);
                        facturacion.TotalGeneral = facturacion.SubTotalGeneral + facturacion.Impuesto + facturacion.Cargo - facturacion.Descuento;
                    }

                    facturacion.IdAdquiriente = facturacion.IdAdquiriente;
                }

                var receptor = (AdquirienteReceptor)Session["sDatosReceptor"];

                facturacion.RazonSocial = receptor.RazonSocial;
                facturacion.Identificacion = receptor.NumeroIdentificacion;
                facturacion.Digito = string.IsNullOrEmpty(receptor.Digito) ? 0 : int.Parse(receptor.Digito);
                facturacion.Pais = receptor.TextPais;
                facturacion.Departamento = receptor.TextDepartamento;
                facturacion.Ciudad = receptor.TextMunicipio;
                facturacion.Correo = receptor.Email;
                facturacion.Telefono = receptor.Telefono;
                facturacion.Direccion = receptor.Direccion;
                facturacion.ResFiscal = receptor.TipoAdquiriente;

                facturacion.ListaImpuestos = _clsFacturacionManual.ListadoImpuestoRetencion(Guid.Parse("318E6846-C1C4-4152-BB9A-3C51F6285891"));

                var listadoRetencion = _clsFacturacionManual.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5"));
                Session["facturaManualRetencionSession"] = listadoRetencion;
                facturacion.ListaRetenciones = listadoRetencion;
                if (listadoRetencion.Any())
                {
                    facturacion.IdRetencion = Guid.Parse(listadoRetencion.FirstOrDefault().Value);
                    var tipoRetencion = _clsFacturacionManual.CargarListadoTipoRetencion(facturacion.IdRetencion);
                    Session["facturaManualTipoRetencionSession"] = tipoRetencion;

                    if (tipoRetencion.Any())
                    {
                        if (facturacion.IdTipoRetencion == Guid.Empty)
                        {
                            facturacion.IdTipoRetencion = tipoRetencion.FirstOrDefault().trf_id;
                            facturacion.PorcentajeRetencion = tipoRetencion.FirstOrDefault().trf_porcentajes;
                        }
                        ViewBag.IdTipoRetencion = (new SelectList(tipoRetencion, "trf_id", "trf_concepto_retencion", facturacion.IdTipoRetencion)).ToList();
                    }
                    else
                    {
                        ViewBag.IdTipoRetencion = null;
                        facturacion.PorcentajeRetencion = 0;
                    }
                }
                else
                {
                    facturacion.ListadoTipoRetenciones = null;
                    facturacion.PorcentajeRetencion = 0;
                }



                //facturacion.ListaRetenciones = _clsFacturacionManual.ListadoImpuestoRetencion(Guid.Parse("44C4C365-61DD-41AB-A75C-7A4D7F4101F5"));
                facturacion.ListaMoneda = _clsFacturacionManual.CargarListadoMoneda();
                facturacion.ListaFormaPago = _clsFacturacionManual.CargarListadoFormaPago();
                facturacion.ListaMedioPago = _clsFacturacionManual.CargarListadoMedioPago();
                facturacion.ListaContingencia = _clsFacturacionManual.ListaContingencia();

                if (Session["sDatosBasicos"] == null)
                {
                    if (facturacion.Fecha.ToShortDateString() != DateTime.Now.ToShortDateString())
                    {
                        facturacion.Fecha = DateTime.Now;
                        facturacion.FechaVencimiento = DateTime.Now.AddDays(30);
                    }

                    var datosbasicos = new BasicInformation
                    {
                        Fecha = facturacion.Fecha,
                        FechaVencimiento = facturacion.FechaVencimiento
                    };
                    facturacion.FormaPago = facturacion.ListaFormaPago.Any() ? Guid.Parse(facturacion.ListaFormaPago[0].Value) : Guid.Empty;
                    datosbasicos.FormaPago = facturacion.FormaPago;
                    facturacion.MedioPago = facturacion.ListaMedioPago.Any() ? Guid.Parse(facturacion.ListaMedioPago[0].Value) : Guid.Empty;
                    datosbasicos.MedioPago = facturacion.MedioPago;
                    facturacion.CodigoContingencia = facturacion.ListaContingencia[0].Value.ToString();
                    datosbasicos.Moneda = facturacion.ListaMoneda.Any() ? Guid.Parse(facturacion.ListaMoneda[0].Value) : Guid.Empty;
                    datosbasicos.CodigoContingencia = facturacion.CodigoContingencia;
                    datosbasicos.IdEmisor = emisor;
                    datosbasicos.IdReceptor = facturacion.IdAdquiriente;
                    datosbasicos.Prefijo = facturacion.Prefijo;
                    datosbasicos.NumeroFactura = 0;
                    datosbasicos.Observacion = "";
                    datosbasicos.TipoDocumento = facturacion.TipoDocumento;
                    datosbasicos.FechaTrm = DateTime.Now;
                    datosbasicos.Trm = 0;
                    Session["sDatosBasicos"] = datosbasicos;
                }
                else
                {
                    var datosbasicos = (BasicInformation)Session["sDatosBasicos"];
                    facturacion.FormaPago = datosbasicos.FormaPago;
                    facturacion.MedioPago = datosbasicos.MedioPago;
                    facturacion.Moneda = datosbasicos.Moneda;
                    facturacion.Observacion = datosbasicos.Observacion;
                    facturacion.OrdenCompra = datosbasicos.OrdenCompra;
                    facturacion.Despacho = datosbasicos.Despacho;
                    facturacion.Recepcion = datosbasicos.Recepcion;
                    facturacion.CodigoContingencia = datosbasicos.CodigoContingencia;
                    facturacion.TipoDocumento = datosbasicos.TipoDocumento;
                    facturacion.FechaTrm = datosbasicos.FechaTrm;
                    facturacion.Trm = datosbasicos.Trm;
                    facturacion.Fecha = datosbasicos.Fecha;
                    facturacion.FechaVencimiento = datosbasicos.FechaVencimiento;
                }

                var prefijo = CargarPrefijo(emisor.ToString(), facturacion.Fecha);
                facturacion.Prefijo = prefijo;

                ViewBag.TipoSoporte = (facturacion.TipoDocumento == "03") ? true : false;
                if (Session["sCatalogoGeneral"] == null)
                {
                    var cag = _clsFacturacionManual.CargarDatosCatalogo(emisor, Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6"));
                    facturacion.ListadoCatalogo = cag;
                    Session["sCatalogoGeneral"] = cag;
                }
                else
                {
                    var catalogo = Session["sCatalogoGeneral"] as List<Catalogo>;
                    catalogo = catalogo.Where(c => c.Descripcion != "Fecha Vencimiento")
                        .Select(c => new Catalogo { Valor = c.Valor, Descripcion = c.Descripcion, Id = Guid.NewGuid() }).ToList();
                    Session["sCatalogoGeneral"] = catalogo;
                    facturacion.ListadoCatalogo = catalogo;
                }

                if (Session["sImpuestos"] == null)
                {
                    facturacion.ImpuestosTabla = new List<FacturacionManual.ImpuestoRetencion> { };
                }
                else
                {
                    facturacion.ImpuestosTabla = Session["sImpuestos"] as List<FacturacionManual.ImpuestoRetencion>;
                }

                if (Session["sRetenciones"] == null)
                {
                    facturacion.RetencionesTabla = new List<FacturacionManual.ImpuestoRetencion> { };
                    facturacion.TotalRetencion = 0;
                }
                else
                {
                    facturacion.RetencionesTabla = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>;
                    facturacion.TotalRetencion = facturacion.RetencionesTabla.Sum(s => s.Valor);
                }

                return View(facturacion);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }

        }
        public ActionResult Buscar()
        {
            try
            {

                var sucursal = (Guid)Session["i_id_sucursal"];
                Session["sDetallesFactura"] = null;
                Session["sCatalogoDetalle"] = null;
                Session["sCatalogoGeneral"] = null;
                Session["sImpuestos"] = null;
                Session["sRetenciones"] = null;
                Session["sTotalGeneral"] = null;
                Session["sDatosBasicos"] = null;
                Session["sDatosReceptor"] = null;
                Session["sDatosFactura"] = null;
                Session["sDatosProductos"] = null;
                Session["sNotaUmm"] = null;
                Session["sEditarNotaUmm"] = null;


                var model = new Buscar();
                var ListadoTemporal = _clsFacturacionManual.CargaTemporal(sucursal);

                if (TempData["mensajeFacturaOk"] is string mensajeFactura)
                {
                    model.JsFuncion = mensajeFactura;
                }
                if (TempData["Eliminar"] is string Eliminar)
                {
                    model.JsFuncion = Eliminar;
                }
                model.ListadoTemporal = ListadoTemporal;

                if (TempData["Insercion"] is string inserciontemporal)
                {
                    model.JsFuncion = inserciontemporal;
                }
                if (TempData["tablae"] is string tablae)
                {
                    model.JsFuncion = tablae;
                }
                if (TempData["CrearReceptor"] is string CrearReceptor)
                {
                    model.JsFuncion = CrearReceptor;
                }

                return View(model);
            }
            catch (Exception ex)
            {

                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult Buscar(Buscar buscar)
        {
            try
            {
                limpiarSessiones();

                var emisor = (Guid)Session["i_id_emisor"];
                var idSucursal = (Guid)Session["i_id_sucursal"];

                if (_clsFacturacionManual.ValidarResolucionFactura(emisor, DateTime.Now, "E92E2E55-C00D-430A-89FC-8864900AE1B8", idSucursal))
                {
                    var result = _clsBuscar.BuscarReceptor(buscar.CampoBuscar, emisor);

                    var model = new Buscar
                    {
                        listaResultados = result
                    };

                    return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadosBusqueda", model) });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No tiene Resoluciones Activas");
                    return Json(new { isValid = false, data = RenderPartialViewToString(this, "Buscar", buscar) });
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        public ActionResult IrIndex(Buscar buscar, string tipodocumento)
        {
            try
            {
                limpiarSessiones();

                var model = new FacturacionManual
                {
                    IdAdquiriente = buscar.Id,
                    TipoDocumento = tipodocumento
                };

                Session["sAdquiriente"] = buscar.Id;

                return RedirectToAction("Index", "FacturacionManual", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        //public ActionResult IrIndex(Buscar buscar)
        //{
        //    try
        //    {
        //        limpiarSessiones();

        //        var model = new FacturacionManual
        //        {
        //            IdAdquiriente = buscar.Id
        //        };

        //        return RedirectToAction("Index", "FacturacionManual", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"Error: {ex.ToString()}");
        //        return RedirectToAction("Error", "Error");
        //    }
        //}

        public ActionResult AgregarDetalleFactura(FacturacionManual facturacionManual)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var catalogo = _clsFacturacionManual.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));
                Session["sCatalogoDetalle"] = catalogo;
                //var emisor = "507F036A-7C1F-4436-AE2C-917590881064";
                var model = new DetallesFacturacion
                {
                    ListaUnidad = _clsFacturacionManual.ListadoUnidades(),
                    ListaImpuesto = _clsConfiguracion.ListadoImpuestoAsignado(emisor),
                    //ListaImpuesto = _clsFacturacionManual.ListadoImpuestos(),
                    ListadoCatalogo = Session["sCatalogoDetalle"] as List<Catalogo>,
                    NitBuscar = facturacionManual.NitBuscar,
                    IdEmisor = facturacionManual.IdEmisor,
                    TempFecha = facturacionManual.Fecha,
                    TempFechaVencimiento = facturacionManual.FechaVencimiento,
                    TempFormaPago = facturacionManual.FormaPago,
                    TempMoneda = facturacionManual.Moneda,
                    TempMedioPago = facturacionManual.MedioPago
                };

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Login");
            }
        }

        public ActionResult AgregarProductoFactura()
        {
            try
            {
                Session["sDatosProductos"] = null;
                var emisor = (Guid)Session["i_id_emisor"];

                var catalogo = _clsFacturacionManual.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));
                Session["sCatalogoDetalle"] = catalogo;

                var model = new ProductoFactura
                {
                    ListadoProducto = _clsFacturacionManual.CargarListadoProductos(emisor)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Login");

            }

        }

        public ActionResult GuardarDetalleFactura(DetallesFacturacion detallesFacturacion)
        {
            try
            {
                var listadoDetalles = (Session["sDetallesFactura"] != null) ? Session["sDetallesFactura"] as List<FacturacionManual.Detalle> : new List<FacturacionManual.Detalle> { };

                if (detallesFacturacion.Id != 0)
                {
                    var detalle = listadoDetalles.Where(x => x.Id == detallesFacturacion.Id).FirstOrDefault();

                    detalle.Codigo = detallesFacturacion.Codigo;
                    detalle.DescripcionDetalles = detallesFacturacion.DescripcionDetalle;
                    detalle.Cantidad = detallesFacturacion.Cantidad;
                    detalle.ValorUnidad = detallesFacturacion.ValorUnidad;
                    detalle.Descuento = detallesFacturacion.Descuento;
                    detalle.Cargo = detallesFacturacion.Cargo;
                    detalle.ValorImpuesto = detallesFacturacion.ValorImpuesto;
                    detalle.SubTotal = detallesFacturacion.SubTotal;
                    detalle.Total = detallesFacturacion.Total;
                    detalle.ListaCatalogo = Session["sCatalogoDetalle"] as List<Catalogo>;
                    detalle.IdImpuesto = detallesFacturacion.IdImpuesto;
                }
                else
                {
                    var maxid = ((listadoDetalles.Any()) ? listadoDetalles.Max(x => x.Id) : 0) + 1;

                    listadoDetalles.Add(new FacturacionManual.Detalle
                    {
                        Id = maxid,
                        Cantidad = detallesFacturacion.Cantidad,
                        Codigo = detallesFacturacion.Codigo,
                        DescripcionDetalles = detallesFacturacion.DescripcionDetalle,
                        Descuento = detallesFacturacion.Descuento,
                        Cargo = detallesFacturacion.Cargo,
                        IdImpuesto = detallesFacturacion.IdImpuesto,
                        PorcentajeImpuesto = detallesFacturacion.PorcentajeImpuesto,
                        SubTotal = detallesFacturacion.SubTotal,
                        Total = detallesFacturacion.Total,
                        Unidad = detallesFacturacion.Unidad,
                        ValorImpuesto = detallesFacturacion.ValorImpuesto,
                        ValorUnidad = detallesFacturacion.ValorUnidad,
                        ListaCatalogo = Session["sCatalogoDetalle"] as List<Catalogo>
                    });

                    Session["sCatalogoDetalle"] = null;
                }

                Session["sDetallesFactura"] = listadoDetalles;
                Session["sRetenciones"] = null;

                //FacturacionManual facturacion = new FacturacionManual();

                //facturacion.IdAdquiriente = detallesFacturacion.IdAdquiriente;
                //facturacion.NitBuscar = detallesFacturacion.NitBuscar;
                //facturacion.Fecha = detallesFacturacion.TempFecha;
                //facturacion.FechaVencimiento = detallesFacturacion.TempFechaVencimiento;
                //facturacion.IdEmisor = detallesFacturacion.IdEmisor;
                //facturacion.FormaPago = detallesFacturacion.TempFormaPago;
                //facturacion.MedioPago = detallesFacturacion.TempMedioPago;
                //facturacion.Moneda = detallesFacturacion.TempMoneda;
                //return RedirectToAction("Index", "FacturacionManual", facturacion);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CancelarAgregarDetalle(DetallesFacturacion detallesFacturacion)
        {
            try
            {
                var model = new FacturacionManual
                {
                    IdAdquiriente = detallesFacturacion.IdAdquiriente,
                    NitBuscar = detallesFacturacion.NitBuscar,
                    Fecha = detallesFacturacion.TempFecha,
                    FechaVencimiento = detallesFacturacion.TempFechaVencimiento,
                    IdEmisor = detallesFacturacion.IdEmisor,
                    FormaPago = detallesFacturacion.TempFormaPago,
                    MedioPago = detallesFacturacion.TempMedioPago,
                    Moneda = detallesFacturacion.TempMoneda
                };

                return RedirectToAction("Index", "FacturacionManual", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }



        public ActionResult EliminarDetallefactura(DetallesFacturacion detallesFacturacion)
        {
            try
            {
                var detallesFactura = Session["sDetallesFactura"] as List<FacturacionManual.Detalle>;

                var rm = detallesFactura.Where(x => x.Id == detallesFacturacion.Id).FirstOrDefault();

                detallesFactura.Remove(rm);

                Session["sDetallesFactura"] = detallesFactura;

                var model = new FacturacionManual
                {
                    NitBuscar = detallesFacturacion.NitBuscar,
                    IdAdquiriente = detallesFacturacion.IdAdquiriente,
                    Fecha = detallesFacturacion.TempFecha,
                    FechaVencimiento = detallesFacturacion.TempFechaVencimiento,
                    IdEmisor = detallesFacturacion.IdEmisor
                };

                Session["sRetenciones"] = null;

                return RedirectToAction("Index", "FacturacionManual", model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult EditarDetalleFactura(int id, Guid adquiriente, DateTime fecha, DateTime fechaVencimiento)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                //var emisor = "507F036A-7C1F-4436-AE2C-917590881064";

                var detallesFactura = Session["sDetallesFactura"] as List<FacturacionManual.Detalle>;

                var detalle = detallesFactura.Where(x => x.Id == id).FirstOrDefault();

                if (detalle.ListaCatalogo != null && detalle.ListaCatalogo.Any())
                {
                    Session["sCatalogoDetalle"] = detalle.ListaCatalogo;
                }
                else
                {
                    var catalogo = _clsFacturacionManual.CargarDatosCatalogo(emisor, Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9"));
                    detalle.ListaCatalogo = catalogo;
                    Session["sCatalogoDetalle"] = catalogo;
                }
                var model = new DetallesFacturacion
                {
                    Id = id,
                    Cantidad = detalle.Cantidad,
                    Codigo = detalle.Codigo,
                    DescripcionDetalle = detalle.DescripcionDetalles,
                    Descuento = detalle.Descuento,
                    Cargo = detalle.Cargo,
                    IdImpuesto = detalle.IdImpuesto,
                    ListaUnidad = _clsFacturacionManual.ListadoUnidades(),
                    ListaImpuesto = _clsConfiguracion.ListadoImpuestoAsignado(emisor),
                    //ListaImpuesto = _clsFacturacionManual.ListadoImpuestos(),
                    ListadoCatalogo = detalle.ListaCatalogo,
                    IdAdquiriente = adquiriente,
                    TempFecha = fecha,
                    TempFechaVencimiento = fechaVencimiento,
                    Total = detalle.Total,
                    Unidad = detalle.Unidad,
                    ValorImpuesto = detalle.ValorImpuesto,
                    ValorUnidad = detalle.ValorUnidad,
                    PorcentajeImpuesto = detalle.PorcentajeImpuesto,
                    NitBuscar = detalle.NitBuscar,

                };

                return View("AgregarDetalleFactura", model);

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CargarDatosAdquiriente(string nit)
        {
            try
            {
                var datos = _clsFacturacionManual.CargarDatosAdquiriente(nit);
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CargarPorcentajeImpuesto(Guid impuesto)
        {
            try
            {
                decimal porcentaje = _clsFacturacionManual.CargarPorcentajeImpuesto(impuesto);
                return Json(porcentaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult ActualizarCatalogoFactura(Guid idCatalogo, string valor)
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

        public ActionResult ActualizarCatalogoDetalle(Guid idCatalogo, string valor)
        {
            try
            {
                var cag = Session["sCatalogoDetalle"] as List<Catalogo>;

                var catalogo = cag.Where(x => x.Id == idCatalogo).FirstOrDefault();

                catalogo.Valor = valor;

                Session["sCatalogoDetalle"] = cag;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambioFechaVencimiento(DateTime fecha)
        {
            try
            {

                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.FechaVencimiento = fecha;

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");

            }
        }
        public string CargarPrefijo(string emisor, DateTime fecha)
        {
            try
            {
                return _clsFacturacionManual.CargarPrefijo(Guid.Parse(emisor), fecha, "E92E2E55-C00D-430A-89FC-8864900AE1B8", (Guid)Session["i_id_sucursal"]);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return "";
            }
        }
        public ActionResult CambiarContingencia(string codigo)
        {
            try
            {
                var basic = (BasicInformation)Session["sDatosBasicos"];
                basic.CodigoContingencia = codigo;
                Session["sDatosBasicos"] = basic;

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
                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.Moneda = id;

                Session["sDatosBasicos"] = basic;

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
                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.Trm = valor;

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarFechaTrm(DateTime fecha)
        {
            try
            {
                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.FechaTrm = fecha;

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarMedioPago(Guid id)
        {
            try
            {
                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.MedioPago = id;

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarFormaPago(Guid id)
        {
            try
            {
                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.FormaPago = id;

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult AgregarObservacion(string observacion)
        {
            try
            {
                var basic = (BasicInformation)Session["sDatosBasicos"];

                basic.Observacion = observacion;

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
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
                var basic = (BasicInformation)Session["sDatosBasicos"];

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

                Session["sDatosBasicos"] = basic;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        /// <summary>
        /// Convierte una vista en  string
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
        /// Construye el json para que se pueda usar en la validación y en el envío
        /// </summary>
        /// <param name="facturacionManual"></param>
        /// <returns></returns>
        public DocumentoModel ConstruirJson(FacturacionManual facturacionManual)
        {
            try
            {
                var idSucursal = (Guid)Session["i_id_sucursal"];
                var basicInfor = (BasicInformation)Session["sDatosBasicos"];

                if (basicInfor.NumeroFactura == 0)
                {
                    var datosBasicos = (BasicInformation)Session["sDatosBasicos"];
                    var numerofactura = _clsFacturacionManual.CargarNumerofactura(basicInfor.IdEmisor, basicInfor.Fecha, "E92E2E55-C00D-430A-89FC-8864900AE1B8", idSucursal);
                    datosBasicos.NumeroFactura = numerofactura;
                    Session["sDatosBasicos"] = datosBasicos;
                }

                //COMPROBANTE ***
                var comprobante = new Comprobante
                {
                    OrigenDocumento = "AE436474-1101-46B8-BC4D-C83C23542B21",
                    TipoComprobante = basicInfor.TipoDocumento,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Prefijo = facturacionManual.Prefijo,
                    Numero = basicInfor.NumeroFactura.ToString(),
                    Moneda = _clsFacturacionManual.CargarCodigoMoneda(basicInfor.Moneda),
                    Referencia = "",
                    ConceptoRef = "",
                    Observaciones = basicInfor.Observacion,
                    Usuario = Session["i_susuario"].ToString(),
                    NumeroOrden = basicInfor.OrdenCompra ?? "",
                    NumeroDespacho = basicInfor.Despacho ?? "",
                    NumeroRecepcion = basicInfor.Recepcion ?? "",
                    DocumentoAdicionalNotaCredito = (basicInfor.TipoDocumento == "03") ? "1,2,3,4,5" : "",
                    DocumentoReferenciaCodigo = (basicInfor.TipoDocumento == "01") ? "" : basicInfor.CodigoContingencia,
                    MetodoPago = new List<MetodoPago> { new MetodoPago { FormaPago = _clsFacturacionManual.CargarCodigoFormaPago(basicInfor.FormaPago), MedioPago = _clsFacturacionManual.CargarCodigoMedioPago(basicInfor.MedioPago), Fecha = DateTime.Now.ToString("yyyy-MM-dd") } },

                };

                //EMISOR ***
                var datosEmisor = _clsFacturacionManual.CargarDatosEmisorSucursal(idSucursal);
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

                //CREDENCIALES ***
                var credenciales = new Credenciales
                {
                    AccessToken = datosEmisor.AccessToken,
                    ClientToken = datosEmisor.ClientToken
                };

                //RECEPTOR ***
                var datosReceptor = (AdquirienteReceptor)Session["sDatosReceptor"];
                var receptor = new Receptor
                {
                    RazonSocial = datosReceptor.RazonSocial,
                    Identificacion = datosReceptor.NumeroIdentificacion,
                    DigitoVerificador = datosReceptor.Digito,
                    email = datosReceptor.Email,
                    Telefono = datosReceptor.Telefono,
                    Direccion = datosReceptor.Direccion,
                    Ciudad = datosReceptor.TextMunicipio,
                    Departamento = datosReceptor.TextDepartamento,
                    Pais = datosReceptor.TextPais,
                    CiudadCodigo = datosReceptor.CodigoMunicipio,
                    CodigoPostal = datosReceptor.CodigoPostal,
                    DepartamentoCodigo = datosReceptor.CodigoDepartamento,
                    Descripcion = null,
                    NombreComercial = datosReceptor.Nombre,
                    NumeroMatriculaMercantil = null,
                    PaisCodigo = datosReceptor.CodigoPais,
                    TipoIdentificacion = datosReceptor.CodigoTipoIdentificacion,
                    TipoPersona = datosReceptor.CodigoTipoPersona,
                    TipoReceptor = datosReceptor.TipoAdquiriente,
                };


                //DETALLES ***
                var detalles = new List<Detalles>();

                var listadoDetalles = Session["sDetallesFactura"] as List<FacturacionManual.Detalle>;

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

                        var (codigo, porcentaje) = _clsFacturacionManual.CargarCodigoImpuesto(item.IdImpuesto);
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
                            Base = decimal.Round((item.SubTotal), 2).ToString(),
                            CodigoImpuesto = _clsFacturacionManual.CargarCodigoImpuestoC(item.IdImpuesto), //   item.Codigo, //  codigo,
                            Impuesto = decimal.Round((item.ValorImpuesto), 2).ToString(),
                            Nombre = _clsFacturacionManual.CargarNombreImpuesto(item.IdImpuesto),
                            Porcentaje = decimal.Round((porcentaje), 2).ToString() //porcentaje
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

                        var unidad = _clsFacturacionManual.CargarCodigoUnidad(item.Unidad);

                        var listaDetalleCatalogo = new List<Models.Descripcion>();

                        if (item.ListaCatalogo != null && item.ListaCatalogo.Any())
                        {
                            foreach (var itemCat in item.ListaCatalogo)
                            {
                                var catdetalle = new Models.Descripcion
                                {
                                    Nombre = itemCat.Descripcion,
                                    Valor = itemCat.Valor
                                };
                                listaDetalleCatalogo.Add(catdetalle);
                            }
                        }

                        var det = new Detalles
                        {
                            Cantidad = item.Cantidad.ToString(),
                            codigo = item.Id.ToString(),
                            Descripcion = listaDetalleCatalogo,
                            Nombre = item.DescripcionDetalles,
                            idDetalle = Guid.NewGuid().ToString(),
                            Impuestos = impuestos,
                            Descuento = decimal.Round((item.Descuento), 2).ToString(),
                            SubTotal = decimal.Round((item.SubTotal), 2).ToString(),
                            Total = decimal.Round((item.Total), 2).ToString(),
                            AplicaImpuesto = true,
                            UnidadCodigo = unidad,
                            ValorUnitario = decimal.Round((item.ValorUnidad), 2).ToString(),
                            Cargos = decimal.Round((item.Cargo), 2).ToString(),
                            AllowanceCharge = null,
                            PricingReference = null
                        };

                        detalles.Add(det);
                    }
                }

                var retencion = new List<FacturacionManual.ImpuestoRetencion>();

                if (Session["sRetenciones"] != null)
                {
                    retencion = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>;
                }

                //TOTALES ***

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

                totales.Cargos = decimal.Round(facturacionManual.Cargo, 2).ToString();
                totales.Descuentos = decimal.Round(facturacionManual.Descuento, 2).ToString();
                totales.Total = (decimal.Parse(totales.SubTotal) + decimal.Parse(totales.IVA) + decimal.Parse(totales.Cargos) - decimal.Parse(totales.Descuentos)).ToString();
                //totales.TotalEnLetras = $"{_convertNumeroToLetra.Enletras(totales.Total)} {_clsFacturacionManual.TipoMoneda(basicInfor.Moneda)}";
                totales.TotalConRetencion = decimal.Round(decimal.Parse(totales.Total) - retencion.Sum(r => r.Valor), 2);
                totales.TotalEnLetras = $"{_convertNumeroToLetra.Enletras((decimal.Parse(totales.Total) - retencion.Sum(r => r.Valor)).ToString())} {_clsFacturacionManual.TipoMoneda(basicInfor.Moneda)}";
                totales.TotalEnLetrasSinRetencion = $"{_convertNumeroToLetra.Enletras(totales.Total)} {_clsFacturacionManual.TipoMoneda(basicInfor.Moneda)}";

                var listaAllowanceCharge = new List<Models.AllowanceCharge>();

                if (facturacionManual.Cargo > 0)
                {
                    var allowanceCharge = new AllowanceCharge
                    {
                        ID = "1",
                        ChargeIndicator = true,
                        AllowanceChargeReasonCode = "",
                        AllowanceChargeReason = "Cargo Factura",
                        Amount = facturacionManual.Cargo,
                        BaseAmount = decimal.Parse(totales.SubTotal),
                        MultiplierFactorNumeric = (decimal.Parse(totales.SubTotal) == 0 ? 0 : facturacionManual.Cargo / decimal.Parse(totales.SubTotal)) * 100 > 100 ? 100 : (decimal.Parse(totales.SubTotal) == 0 ? 0 : facturacionManual.Cargo / decimal.Parse(totales.SubTotal)) * 100
                    };

                    listaAllowanceCharge.Add(allowanceCharge);
                }

                if (facturacionManual.Descuento > 0)
                {
                    var allowanceCharge = new AllowanceCharge
                    {
                        ID = "2",
                        ChargeIndicator = false,
                        AllowanceChargeReasonCode = "11",
                        AllowanceChargeReason = "Descuento Factura",
                        Amount = facturacionManual.Descuento,
                        BaseAmount = decimal.Parse(totales.SubTotal),
                        MultiplierFactorNumeric = (decimal.Parse(totales.SubTotal) == 0 ? 0 : facturacionManual.Descuento / decimal.Parse(totales.SubTotal)) * 100 > 100 ? 100 : (decimal.Parse(totales.SubTotal) == 0 ? 0 : facturacionManual.Descuento / decimal.Parse(totales.SubTotal)) * 100
                    };

                    listaAllowanceCharge.Add(allowanceCharge);
                }

                //TOTALIMPUESTO ***
                var inc = Guid.Parse("208B7394-9916-4346-9881-86C4A92A6B12");
                var iva5 = Guid.Parse("2A7E3DD4-4AB8-447C-9224-9F4AA46B9C84");
                var noCausa = Guid.Parse("35191913-56E3-4A18-8810-9FF64A2C6C35");
                var iva19 = Guid.Parse("E2D47731-FDB3-4F2A-A899-F220942BD60E");

                var listaIva = new List<Guid> { inc, iva5, noCausa, iva19 };
                var impuestosT = new List<TotalImpuestos>();
                for (int i = 0; i < listaIva.Count; i++)
                {
                    var (codigoIM, porcentajeIM, nombreIM) = _clsFacturacionManual.ListadoImpuestisIva(listaIva[i]);

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

                var listadoDetallesImpuestos = listadoDetalles
                    .Select(d => new
                    {
                        Base = d.SubTotal,
                        CodigoImpuesto = _clsFacturacionManual.CargarCodigoImpuesto(d.IdImpuesto).codigo,
                        Impuesto = d.ValorImpuesto,
                        Nombre = _clsFacturacionManual.CargarNombreImpuesto(d.IdImpuesto),
                        Porcentaje = _clsFacturacionManual.CargarCodigoImpuesto(d.IdImpuesto).porcentaje
                    })
                    .GroupBy(x => new { x.CodigoImpuesto, x.Porcentaje, x.Nombre })
                    .Select(d => new TotalImpuestos
                    {
                        Base = decimal.Round((d.Sum(x => x.Base)), 2).ToString(),
                        CodigoImpuesto = d.Key.CodigoImpuesto,
                        Impuesto = decimal.Round((d.Sum(x => x.Impuesto)), 2).ToString(),
                        Nombre = d.Key.Nombre,
                        Porcentaje = decimal.Round((d.Key.Porcentaje), 2).ToString()
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
                                CodigoImpuesto = _clsFacturacionManual.CargarCodigoImpuesto(itemImp.IdImRe).codigo.ToString(),
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

                //CATALOGO ***
                var catalogo = new List<DetallesComprobante>();

                //List<Catalogo> listadoCatalogoDetalles = Session["sCatalogoDetalle"] as List<Catalogo>;
                List<Catalogo> listadoCatalogoGeneral = Session["sCatalogoGeneral"] as List<Catalogo>;
                var FechaVencimiento = basicInfor.FechaVencimiento;
                //if (listadoCatalogoDetalles != null && listadoCatalogoDetalles.Any())
                //{
                //    foreach (var item in listadoCatalogoDetalles)
                //    {
                //        if (item.Valor != 0)
                //        {
                //            var catD = new DetallesComprobante
                //            {
                //                Nombre = item.Descripcion,
                //                Valor = item.Valor.ToString()
                //            };

                //            catalogo.Add(catD);
                //        }
                //    }
                //}

                if (listadoCatalogoGeneral != null && listadoCatalogoGeneral.Any())
                {
                    foreach (var item in listadoCatalogoGeneral)
                    {
                        //if (item.Valor != "")
                        //{
                        var catG = new DetallesComprobante
                        {
                            Nombre = item.Descripcion,
                            Valor = item.Valor
                        };
                        catalogo.Add(catG);
                        //}
                    }
                }

                var Fvenci = new DetallesComprobante
                {
                    Nombre = "Fecha Vencimiento",
                    Valor = FechaVencimiento.ToString("yyyy-MM-dd"),
                };
                catalogo.Add(Fvenci);

                //TERMINO DE PAGO ***
                TimeSpan ts = basicInfor.Fecha - basicInfor.FechaVencimiento;
                var terminodepago = new TerminosPago
                {
                    Codigo = "2",
                    UnidadCodigo = "DAY",
                    Duracion = Math.Abs(ts.Days).ToString()
                };


                //PaymentExchangeRate
                PaymentExchangeRate paymentExchangeRate = null;
                if (basicInfor.Moneda == Guid.Parse("21EBA47A-3970-4519-8226-C607413412AB"))
                {
                    var newPaymentExchangeRate = new PaymentExchangeRate
                    {
                        SourceCurrencyCode = _clsFacturacionManual.CargarCodigoMoneda(basicInfor.Moneda),
                        TargetCurrencyCode = "COP",
                        TargetCurrencyBaseRate = 1,
                        SourceCurrencyBaseRate = 1,
                        CalculationRate = basicInfor.Trm,
                        Date = basicInfor.FechaTrm.ToString("yyyy-MM-dd")
                    };

                    paymentExchangeRate = newPaymentExchangeRate;
                }

                //DOCUMENTO MODEL ***
                var documento = new DocumentoModel
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

                return documento;
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult EnviarDatosFactura(FacturacionManual facturacionManual)
        {
            try
            {
                string respuesta = ConsumirApi(facturacionManual, "Comprobante");

                var datosBasicos = (BasicInformation)Session["sDatosBasicos"];
                var idReceptor = datosBasicos.IdReceptor;

                var estado = _clsFacturacionManual.ConsultarFacturaCreada(datosBasicos.Prefijo, datosBasicos.IdEmisor, datosBasicos.NumeroFactura, datosBasicos.Fecha);

                if (estado)
                {
                    var p = respuesta.IndexOf("Documento:");
                    var f = respuesta.IndexOf("Descripción:");
                    var len = f - p;
                    string numeroFactura = respuesta.Substring(p, len);

                    respuesta = "Factura creada satisfactoriamente. N°: " + numeroFactura;

                    string mensaje = respuesta;
                    string icon = "success";

                    TempData["mensajeFacturaOk"] = $"mensaje('{mensaje}','{icon}')";

                    Session["sDetallesFactura"] = null;
                    Session["sCatalogoDetalle"] = null;
                    Session["sCatalogoGeneral"] = null;
                    Session["sImpuestos"] = null;
                    Session["sRetenciones"] = null;
                    Session["sTotalGeneral"] = null;
                    Session["sDatosBasicos"] = null;
                    Session["sDatosReceptor"] = null;

                    return Json(new { isValid = false, data = "OK" });
                }
                else
                {
                    string facturas = $"{datosBasicos.Prefijo }{datosBasicos.NumeroFactura.ToString()} | ";
                    respuesta = facturas + respuesta;
                }

                var model = new Buscar
                {
                    CampoBuscar = respuesta
                };
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoFactura", model) });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                var model = new Buscar
                {
                    CampoBuscar = ex.ToString()
                };
                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoFactura", model) });
            }
        }

        /// <summary>
        /// Valida si el json cumple con las validaciones de Facse
        /// </summary>
        /// <param name="facturacionManual"></param>
        /// <returns></returns>
        public ActionResult ValidarJson(FacturacionManual facturacionManual)
        {
            try
            {
                string respuesta = ConsumirApi(facturacionManual, "ValidarJson");
                return Json(respuesta);
            }
            catch (Exception err)
            {
                log.Error($"Error: {err.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        /// <summary>
        /// Se usa para consumir el api con el json que se genera a partir de los datos ingresados en el formulario
        /// </summary>
        /// <param name="facturacionManual"></param>
        /// <param name="metodoApi"></param>
        /// <returns></returns>
        public string ConsumirApi(FacturacionManual facturacionManual, string metodoApi)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api"].ToString() + metodoApi;

                var json = JsonConvert.SerializeObject(ConstruirJson(facturacionManual));

                var jsonObjeto = JsonConvert.DeserializeObject<DocumentoModel>(json);
                _clsFacturacionManual.GuardarJson(json, jsonObjeto.Emisor.Identificacion?.Trim(),
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


        #region Productos
        public ActionResult AgregarProductoSession(bool check, Guid idProducto, string codigo, string descripcion, Guid unidad, decimal valor, Guid idImpuesto)
        {
            try
            {
                var datosListado = Session["sDatosProductos"] as List<DatosProductoListado>;

                if (check)
                {
                    var datos = new DatosProductoListado
                    {
                        Cantidad = 1,
                        Descripcion = descripcion,
                        IdProducto = idProducto,
                        Impuesto = idImpuesto,
                        Unidad = unidad,
                        Valor = valor,
                        Codigo = codigo,
                        Cargo = 0,
                        Descuento = 0
                    };

                    if (datosListado != null)
                    {
                        datosListado.Add(datos);
                    }
                    else
                    {
                        var newlist = new List<DatosProductoListado>();
                        newlist.Add(datos);
                        datosListado = newlist;
                    }
                }
                else
                {
                    if (datosListado != null)
                    {
                        var elimi = datosListado.Where(x => x.IdProducto == idProducto).FirstOrDefault();

                        datosListado.Remove(elimi);
                    }
                }

                Session["sDatosProductos"] = datosListado;
                var model = new ResultadoProducto
                {
                    ListadoProductosAgregados = CalcularValores(datosListado)
                };

                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoProducto", model) });
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CambiarCantidad(Guid idItem, int cantidad)
        {
            try
            {
                var datosListado = Session["sDatosProductos"] as List<DatosProductoListado>;

                var cambioListado = datosListado.Where(x => x.IdProducto == idItem).FirstOrDefault();

                cambioListado.Cantidad = cantidad;

                Session["sDatosProductos"] = datosListado;
                var model = new ResultadoProducto
                {
                    ListadoProductosAgregados = CalcularValores(datosListado)
                };

                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoProducto", model) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CambiarCargo(Guid idItem, decimal cargo)
        {
            try
            {
                var datosListado = Session["sDatosProductos"] as List<DatosProductoListado>;

                var cambioListado = datosListado.Where(x => x.IdProducto == idItem).FirstOrDefault();

                cambioListado.Cargo = cargo;

                Session["sDatosProductos"] = datosListado;
                var model = new ResultadoProducto
                {
                    ListadoProductosAgregados = CalcularValores(datosListado)
                };

                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoProducto", model) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult CambiarDescuento(Guid idItem, decimal descuento)
        {
            try
            {
                var datosListado = Session["sDatosProductos"] as List<DatosProductoListado>;

                var cambioListado = datosListado.Where(x => x.IdProducto == idItem).FirstOrDefault();

                cambioListado.Descuento = descuento;

                Session["sDatosProductos"] = datosListado;
                var model = new ResultadoProducto
                {
                    ListadoProductosAgregados = CalcularValores(datosListado)
                };

                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoProducto", model) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GuardarProductoGeneralEditado(Guid idProducto, int cantidad, decimal valor, decimal cargo, string codigo, string descripcion,
                        decimal descuento, Guid impuesto, Guid unidad)
        {
            try
            {
                var datosListado = Session["sDatosProductos"] as List<DatosProductoListado>;

                var cambioListado = datosListado.Where(x => x.IdProducto == idProducto).FirstOrDefault();

                cambioListado.Cantidad = cantidad;
                cambioListado.Cargo = cargo;
                cambioListado.Codigo = codigo;
                cambioListado.Descripcion = descripcion;
                cambioListado.Descuento = descuento;
                cambioListado.Impuesto = impuesto;
                cambioListado.Unidad = unidad;
                cambioListado.Valor = valor;

                Session["sDatosProductos"] = datosListado;

                var model = new ResultadoProducto
                {
                    ListadoProductosAgregados = CalcularValores(datosListado)
                };

                return Json(new { isValid = true, data = RenderPartialViewToString(this, "ResultadoProducto", model) });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult GuardarProductoDetalle()
        {
            try
            {
                var datosListado = Session["sDatosProductos"] as List<DatosProductoListado>;

                Session["sDetallesFactura"] = CalcularValores(datosListado);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult CancelarProductos()
        {
            try
            {
                Session["sDatosProductos"] = null;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Detalle> CalcularValores(List<DatosProductoListado> datosListado)
        {
            try
            {
                decimal subtotal, total, cargo, descuento, valorUnidad, valorImpuesto;
                int cantidad;
                var listadoNewDetalle = new List<Detalle>();

                for (int i = 0; i < datosListado.Count; i++)
                {
                    valorUnidad = datosListado[i].Valor;
                    cantidad = datosListado[i].Cantidad;
                    cargo = datosListado[i].Cargo;
                    descuento = datosListado[i].Descuento;
                    subtotal = ((valorUnidad * cantidad) + cargo) - descuento;
                    valorImpuesto = subtotal * _clsFacturacionManual.CargarCodigoImpuesto(datosListado[i].Impuesto).porcentaje / 100;
                    total = subtotal + valorImpuesto;
                    var newProducto = new Detalle
                    {
                        IdProducto = datosListado[i].IdProducto,
                        Cantidad = cantidad,
                        Codigo = datosListado[i].Codigo,
                        DescripcionDetalles = datosListado[i].Descripcion,
                        IdImpuesto = datosListado[i].Impuesto,
                        Id = i + 1,
                        Cargo = cargo,
                        Descuento = descuento,
                        SubTotal = subtotal,
                        Total = total,
                        Unidad = datosListado[i].Unidad,
                        PorcentajeImpuesto = _clsFacturacionManual.CargarCodigoImpuesto(datosListado[i].Impuesto).porcentaje,
                        ValorImpuesto = valorImpuesto,
                        ValorUnidad = valorUnidad,
                        ListaCatalogo = _clsFacturacionManual.CargarListadoCatalogoProducto(datosListado[i].IdProducto),
                    };

                    listadoNewDetalle.Add(newProducto);
                }

                return listadoNewDetalle;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ActionResult CargarDatosEditar(Guid idProduto)
        {
            try
            {
                var listadoProductos = CalcularValores(Session["sDatosProductos"] as List<DatosProductoListado>);

                var datosProducto = listadoProductos.Where(x => x.IdProducto == idProduto).FirstOrDefault();

                var listadoUnidad = _clsFacturacionManual.ListadoUnidades();
                //var listadoImpuesto = _clsFacturacionManual.ListadoImpuestos();
                var emisor = (Guid)Session["i_id_emisor"];
                var listadoImpuesto = _clsConfiguracion.ListadoImpuestoAsignado(emisor);

                return Json(new { datosProducto, listadoUnidad, listadoImpuesto });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        public void limpiarSessiones()
        {
            try
            {
                Session["sDetallesFactura"] = null;
                Session["sCatalogoDetalle"] = null;
                Session["sCatalogoGeneral"] = null;
                Session["sImpuestos"] = null;
                Session["sRetenciones"] = null;
                Session["sTotalGeneral"] = null;
                Session["sDatosBasicos"] = null;
                Session["sDatosReceptor"] = null;
                Session["sDatosFactura"] = null;
                Session["sListadoDetallesEd"] = null;
                Session["sDatosFacturaEd"] = null;
                Session["sCatalogoGeneralEd"] = null;
                Session["sCatalogoDetalleEd"] = null;
                Session["sRetencionesEd"] = null;
                Session["sImpuestosEd"] = null;
                Session["sListadoDetallesEc"] = null;
                Session["sDatosFacturaEc"] = null;
                Session["sCatalogoGeneralEc"] = null;
                Session["sListadoDetalles"] = null;
                Session["sDatosFactura"] = null;
                Session["sCatalogoGeneral"] = null;
                Session["sDatosProductos"] = null;
                Session["sTemporalJson"] = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region TemporarFactura

        [ValidateAntiForgeryToken]
        public ActionResult EditarGuardarTemporal(FacturacionManual facturacionManual)
        {
            try
            {
                var sucursal = (Guid)Session["i_id_sucursal"];
                string json = JsonConvert.SerializeObject(ConstruirJson(facturacionManual));

                if (int.Parse(Session["sTemporalJson"].ToString()) == 0)
                {
                    var idUsuario = Session["i_id_usuario"].ToString();
                    var identificacion = Guid.Parse("E92E2E55-C00D-430A-89FC-8864900AE1B8");
                    DocumentoModel cjson = JsonConvert.DeserializeObject<DocumentoModel>(json);
                    var insertar = _clsFacturacionManual.InsertarTemporal(sucursal, idUsuario, identificacion, json, facturacionManual.RazonSocial, decimal.Parse(cjson.Totales.IVA), facturacionManual.SubTotalGeneral, facturacionManual.TotalGeneral, facturacionManual.Identificacion, cjson.Comprobante.Prefijo, int.Parse(cjson.Comprobante.Numero));
                    string mensaje = (insertar != 0) ? $"Se ha guardado correctamente la informacion, con consecutivo {insertar}" : $"No se puede guardar la informacion ";
                    string icon = (insertar != 0) ? "success" : "error";
                    TempData["Insercion"] = $"mensaje('{mensaje}','{icon}')";
                }
                else
                {
                    var consecutivo = int.Parse(Session["sTemporalJson"].ToString());
                    var emisor = (Guid)Session["i_id_emisor"];
                    DocumentoModel cjson = JsonConvert.DeserializeObject<DocumentoModel>(json);
                    var insertar = _clsFacturacionManual.EditarTemporal(consecutivo, json, sucursal, facturacionManual.RazonSocial, decimal.Parse(cjson.Totales.IVA), decimal.Parse(cjson.Totales.SubTotal), decimal.Parse(cjson.Totales.Total), facturacionManual.Identificacion, cjson.Comprobante.Prefijo, int.Parse(cjson.Comprobante.Numero));
                    string mensaje = (insertar) ? $"Se ha editado correctamente la informacion, del consecutivo {consecutivo}" : $"No se puede guardar la informacion ";
                    string icon = (insertar) ? "success" : "error";
                    TempData["Insercion"] = $"mensaje('{mensaje}','{icon}')";
                }
                return Json(new { isValid = false, data = "OK" });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return Json(new { isValid = true, data = "Ha ocurrido un error" });
            }
        }

        public ActionResult EliminarTemporal(Guid Ides)
        {
            try
            {
                var eliminar = _clsFacturacionManual.EliminarTemporal(Ides);
                string mensaje = (eliminar) ? $"Se ha eliminado correctamente la informacion" : $"No se puede eliminar la informacion ";
                string icon = (eliminar) ? "success" : "error";

                TempData["Eliminar"] = $"mensaje('{mensaje}','{icon}')";

                return RedirectToAction("Buscar");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        #endregion

        #region EnviarDatosTablaTemporal
        public (string factura, string consecutivo) EnviarDatosTabla(int consecutivo)
        {
            try
            {
                var sucursal = (Guid)Session["i_id_sucursal"];
                var IdEmisor = (Guid)Session["i_id_emisor"];
                var Fecha = DateTime.Now;
                var json = _clsFacturacionManual.CargarJSON(consecutivo, sucursal);
                DocumentoModel cjson = JsonConvert.DeserializeObject<DocumentoModel>(json);

                var Numero = _clsFacturacionManual.CargarNumerofactura(IdEmisor, Fecha, "E92E2E55-C00D-430A-89FC-8864900AE1B8", sucursal);
                cjson.Comprobante.Numero = Numero.ToString();
                Numero = decimal.Parse(cjson.Comprobante.Numero);
                var jsonactualizado = JsonConvert.SerializeObject(cjson);


                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api"].ToString() + "Comprobante";

                var jsonObjeto = JsonConvert.DeserializeObject<DocumentoModel>(jsonactualizado);
                _clsFacturacionManual.GuardarJson(json, jsonObjeto.Emisor.Identificacion?.Trim(),
                                                jsonObjeto.Emisor.Sucursal?.Trim(), jsonObjeto.Comprobante.TipoComprobante?.Trim(),
                                                jsonObjeto.Comprobante.Prefijo?.Trim(), jsonObjeto.Comprobante.Numero?.Trim());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaApiCompleta);
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonactualizado);
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


                            var tablae = _clsFacturacionManual.EliminarDatosTablaEnviado(consecutivo, sucursal);

                            return (Numero.ToString(), consecutivo.ToString());

                            //return RedirectToAction("Buscar", "FacturacionManual");
                        };
                    }
                }
                return ("Ha ocurrido un error al enviar la factura", " ");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return ("Ha ocurrido un error al enviar la factura", "");
            }
        }
        public ActionResult EnviarTodos(Buscar modelo)
        {
            string mensaje = "";
            string icon = "";
            if (modelo.ListaDocumentos == null)
            {
                mensaje = $"Debe Seleccionar un documento";
                icon = "success";
                TempData["tablae"] = $"mensaje('{mensaje}','{icon}')";

                return RedirectToAction("Buscar");
            }
            var ListadoTemporal = modelo.ListaDocumentos.Split(',');
            var sucursal = (Guid)Session["i_id_sucursal"];
            var emisor = (Guid)Session["i_id_emisor"];
            var listarespuesta = new List<string>();

            var consecutivo = _clsFacturacionManual.CargarConsecutivo(sucursal);
            string numerofactura = "";
            string numeroConsecutiv = "";
            foreach (var item in ListadoTemporal)
            {
                if (int.TryParse(item, out int i))
                {
                    var (a, b) = EnviarDatosTabla(i);
                    numerofactura += ", " + a;
                    numeroConsecutiv += ", " + b;
                }
                //EnviarDatosTabla(item);
            }


            mensaje = $"Se envio el consecutivo {numeroConsecutiv}, con el numero de factura {numerofactura} a la DIAN ";
            icon = "success";
            TempData["tablae"] = $"mensaje('{mensaje}','{icon}')";

            return RedirectToAction("Buscar");
        }

        #endregion

        #region "retencion"

        public ActionResult ListadoTipoRetencion(Guid IdRetencion)
        {
            try
            {
                var listadoTipoRetencion = _clsFacturacionManual.CargarListadoTipoRetencion(IdRetencion);
                Session["facturaManualTipoRetencionSession"] = listadoTipoRetencion;
                if (!listadoTipoRetencion.Any())
                {
                    return Json(new { listado = string.Empty, _clsFacturacionManual.CargarCodigoImpuesto(IdRetencion).porcentaje }, JsonRequestBehavior.AllowGet);
                }
                var listado = (new SelectList(listadoTipoRetencion, "trf_id", "trf_concepto_retencion", listadoTipoRetencion.FirstOrDefault()?.trf_id)).ToList();
                return Json(new { listado, porcentaje = listadoTipoRetencion.FirstOrDefault()?.trf_porcentajes }, JsonRequestBehavior.AllowGet);
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
                return Json(_clsFacturacionManual.CargarPorcentaje(gTipoRetencion));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GuardarRetencionGeneral([Bind(Include = "IdRetencion, SubTotalGeneral, PorcentajeRetencion, IdTipoRetencion")]FacturacionManual facturacionManual)
        {
            try
            {
                if (Session["facturaManualRetencionSession"] != null)
                {
                    string sDescripcionRetencion = "";
                    if (Session["facturaManualTipoRetencionSession"] != null && ((List<adm_tipo_retencion_fuente>)Session["facturaManualTipoRetencionSession"]).Any())
                    {
                        sDescripcionRetencion = ((List<adm_tipo_retencion_fuente>)Session["facturaManualTipoRetencionSession"]).Where(tr => tr.trf_id == facturacionManual.IdTipoRetencion).FirstOrDefault().trf_concepto_retencion;
                    }
                    var retencion = ((List<SelectListItem>)Session["facturaManualRetencionSession"]).Where(r => r.Value == facturacionManual.IdRetencion.ToString()).FirstOrDefault();
                    decimal valor = 0;
                    if (facturacionManual.IdRetencion == Guid.Parse("ED7B87C5-8AB4-41DC-BE0C-AF7EA28CE79E"))
                    {
                        List<Detalle> listadoDetalle = Session["sDetallesFactura"] as List<Detalle>;
                        if (listadoDetalle != null && listadoDetalle.Any())
                        {
                            valor = (listadoDetalle.Where(x => _clsFacturacionManual.ListadoImpuestoIva().Contains(x.IdImpuesto)).Sum(x => x.ValorImpuesto)) * (facturacionManual.PorcentajeRetencion / 100);
                        }
                    }
                    else
                    {
                        valor = (facturacionManual.SubTotalGeneral * (facturacionManual.PorcentajeRetencion / 100));

                    }
                    var detalle = new ImpuestoRetencion
                    {
                        Id = Guid.NewGuid(),
                        IdImRe = facturacionManual.IdRetencion,
                        Descripcion = retencion.Text,
                        Base = facturacionManual.SubTotalGeneral,
                        Valor = valor,
                        PorcentajeRetencion = facturacionManual.PorcentajeRetencion,
                        DescripcionRetencion = sDescripcionRetencion

                    };
                    List<ImpuestoRetencion> listadoRetencion = new List<ImpuestoRetencion>();
                    if (!(Session["sRetenciones"] == null))
                        listadoRetencion = (List<ImpuestoRetencion>)Session["sRetenciones"];
                    listadoRetencion.Add(detalle);
                    Session["sRetenciones"] = listadoRetencion;
                }
                return Json(new { data = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

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
                var retencions = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>;

                var rm = retencions.Where(x => x.Id == id).FirstOrDefault();

                retencions.Remove(rm);

                Session["sRetenciones"] = retencions;

                return Json(new { data = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>, total = (Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>).Sum(s => s.Valor) });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        public ActionResult ValidarRetenciones()
        {
            try
            {
                var retencion = Session["sRetenciones"] as List<FacturacionManual.ImpuestoRetencion>;

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

        #endregion

    }
}