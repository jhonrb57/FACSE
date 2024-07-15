using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Metodos
{
    public class ClsFacturacionManual
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsUsuario).Name);
        private readonly ClsFuncion _clsFuncion = new ClsFuncion();

        /// <summary>
        /// Trae lista de receptores a partir del emisor
        /// </summary>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public List<SelectListItem> BuscarAdquiriente(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => x.rec_emisor == emisor)
                    .Select(r => new SelectListItem
                    {
                        Value = r.rec_id.ToString(),
                        Text = r.rec_razon_social.ToUpper()
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga los datos de un receptor especifico
        /// </summary>
        /// <param name="adquiriente"></param>
        /// <returns></returns>
        public dynamic CargarDatosAdquiriente(string nit)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => x.rec_identificacion == nit.Trim())
                    .Select(x => new
                    {
                        id = x.rec_id,
                        razonSocial = x.rec_razon_social,
                        identificacion = x.rec_identificacion,
                        correo = x.rec_correo.Trim(),
                        direccion = x.rec_direccion.Trim(),
                        telefono = x.rec_telefono.Trim(),
                        digito = x.rec_digito.Trim(),
                        pais = x.sys_pais.pai_nombre_comun.Trim(),
                        departamento = x.sys_departamento.dep_nombre.Trim(),
                        ciudad = x.sys_municipio.mun_nombre.Trim()
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga listado de unidades
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListadoUnidades()
        {
            try
            {
                var query = _dbContext.sys_tipo_unidad_cantidad
                    .Select(x => new SelectListItem
                    {
                        Value = x.tuc_id.ToString(),
                        Text = x.tuc_descripcion.Trim()

                    })
                    .OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Carga listado de impuestos
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListadoImpuestos()
        {
            try
            {
                Guid idTipoImpuesto = Guid.Parse("318E6846-C1C4-4152-BB9A-3C51F6285891");
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_clasificacion == idTipoImpuesto)
                    .Select(x => new SelectListItem
                    {
                        Value = x.tim_id.ToString(),
                        Text = x.tim_nombre.Trim()
                    })
                    .OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga porcentaje de impuesto seleccionado 
        /// </summary>
        /// <param name="idImpuesto"></param>
        /// <returns></returns>
        public decimal CargarPorcentajeImpuesto(Guid idImpuesto)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_id == idImpuesto)
                    .Select(i => i.tim_porcentaje ?? 0).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga listdo de catalogo por emisor
        /// </summary>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public List<Catalogo> CargarDatosCatalogo(Guid emisor, Guid tipoCatalogo)
        {
            try
            {
                var item = new SelectListItem { Value = "", Text = "No aplica" };


                var query = _dbContext.adm_emisor_catalogo
                    .Where(x => x.eca_emisor == emisor && x.eca_tipo_catalogo == tipoCatalogo && x.eca_activo == true)
                    .Select(c => new Catalogo
                    {
                        Id = c.eca_id,
                        Descripcion = c.eca_nombre.Trim(),
                        Valor = "",
                        ListadoCag = c.adm_emisor_catalogo_lista.Select(x => new SelectListItem
                        {
                            Text = x.ecl_descripcion,
                            Value = x.ecl_descripcion
                        }).ToList(),
                        Lista = c.eca_lista
                    }).ToList();

                query.ForEach(itemTest =>
                {
                    //if(item)
                    itemTest.ListadoCag.Insert(0, item);
                });

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el prefijo que se encuentra en el momento ativo
        /// </summary>
        /// <param name="emisor"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string CargarPrefijo(Guid emisor, DateTime fecha, string idTipoDoc, Guid idSucursal)
        {
            try
            {
                Guid tipoDocumento = Guid.Parse(idTipoDoc);

                var query = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_emisor == emisor && x.adm_emisor_resolucion.ere_fecha_inicio <= fecha && x.adm_emisor_resolucion.ere_fecha_final >= fecha &&
                            x.adm_emisor_resolucion.ere_activo == true && x.adm_emisor_resolucion.ere_tipo_documento == tipoDocumento && x.esr_sucursal == idSucursal)
                            .Select(r => r.adm_emisor_resolucion.ere_prefijo.Trim()).FirstOrDefault();
                //var query = _dbContext.adm_emisor_resolucion
                //    .Where(x => x.ere_emisor == emisor && x.ere_fecha_inicio <= fecha
                //        && x.ere_fecha_final >= fecha && x.ere_activo == true && x.ere_tipo_documento == tipoDocumento)
                //        .Select(r => r.ere_prefijo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Carga el numerode factura 
        /// </summary>
        /// <param name="emisor"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal CargarNumerofactura(Guid emisor, DateTime fecha, string idTipoDoc, Guid sucursal)
        {
            try
            {
                decimal maxId;

                Guid tipoDocumento = Guid.Parse(idTipoDoc);

                var documento = _dbContext.adm_documento
                    .Where(x => x.doc_emisor == emisor && x.doc_sucursal == sucursal && x.doc_tipo_documento == tipoDocumento);

                if (documento != null && documento.Any())
                {
                    maxId = documento.Max(x => x.doc_numero) + 1;

                    var primeroResolucion = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_emisor == emisor && x.esr_sucursal == sucursal && x.adm_emisor_resolucion.ere_tipo_documento == tipoDocumento
                    && x.adm_emisor_resolucion.ere_fecha_inicio <= fecha && x.adm_emisor_resolucion.ere_fecha_final >= fecha && x.adm_emisor_resolucion.ere_activo == true);

                    if (!(primeroResolucion.Any()))
                        throw new CustomException("No Existe resolucion, Debe revisar la parametrizacion");

                    if (decimal.Parse(primeroResolucion.FirstOrDefault().adm_emisor_resolucion.ere_numero_inicial) > maxId)
                    {
                        maxId = decimal.Parse(primeroResolucion.Select(x => x.adm_emisor_resolucion.ere_numero_inicial).FirstOrDefault());
                    }
                }
                else
                {

                    var primeroResolucion = _dbContext.adm_emisor_sucursal_resolucion
                        .Where(x => x.esr_emisor == emisor && x.esr_sucursal == sucursal && x.adm_emisor_resolucion.ere_tipo_documento == tipoDocumento
                        && x.adm_emisor_resolucion.ere_fecha_inicio <= fecha && x.adm_emisor_resolucion.ere_fecha_final >= fecha && x.adm_emisor_resolucion.ere_activo == true);

                    if (!(primeroResolucion.Any()))
                        throw new CustomException("No Existe resolucion, Debe revisar la parametrizacion");

                    //var primeroResolucion = _dbContext.adm_emisor_resolucion
                    //    .Where(x => x.ere_emisor == emisor && x.ere_tipo_documento == tipoDocumento && x.ere_fecha_inicio <= fecha
                    //    && x.ere_fecha_final >= fecha && x.ere_activo == true);

                    maxId = decimal.Parse(primeroResolucion.Select(x => x.adm_emisor_resolucion.ere_numero_inicial).FirstOrDefault());
                }


                return maxId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el numero de factura 
        /// </summary>
        /// <param name="emisor"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool ValidarResolucionFactura(Guid emisor, DateTime fecha, string idTipoDoc, Guid sucursal)
        {
            try
            {
                Guid tipoDocumento = Guid.Parse(idTipoDoc);
                var resolucion = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_emisor == emisor && x.esr_sucursal == sucursal && x.adm_emisor_resolucion.ere_tipo_documento == tipoDocumento
                    && x.adm_emisor_resolucion.ere_fecha_inicio <= fecha && x.adm_emisor_resolucion.ere_fecha_final >= fecha && x.adm_emisor_resolucion.ere_activo == true);

                return resolucion.Any();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Consulta si la factura se creo exitosa
        /// </summary>
        /// <param name="prefijo"></param>
        /// <param name="idEmisor"></param>
        /// <param name="numero"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool ConsultarFacturaCreada(string prefijo, Guid idEmisor, decimal numero, DateTime fecha)
        {
            try
            {
                Guid estadoExitoso = Guid.Parse("C05F87B3-ABF6-475B-80B4-1520376E4531");
                DateTime fechaSinHora = DateTime.Parse(fecha.ToShortDateString());

                var query = _dbContext.adm_documento
                    .Where(x => x.doc_prefijo == prefijo && x.doc_emisor == idEmisor && x.doc_numero == numero && x.doc_fecha_envio == fechaSinHora
                                && x.doc_estado == estadoExitoso)
                    .Any();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Carga listado de impuestos, filtrado por tipo
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<SelectListItem> ListadoImpuestoRetencion(Guid tipo)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_clasificacion == tipo)
                    .Select(x => new SelectListItem
                    {
                        Value = x.tim_id.ToString(),
                        Text = x.tim_nombre.Trim()
                    })
                    .OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<adm_emisor_producto> CargarListadoProductos(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_producto.Where(x => x.epr_emisor == emisor && x.epr_activo == true).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el codigo del impuesto a partir de su id
        /// </summary>
        /// <param name="idImpuesto"></param>
        /// <returns></returns>
        public (string codigo, decimal porcentaje) CargarCodigoImpuesto(Guid idImpuesto)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_id == idImpuesto)
                    .Select(x => new
                    {
                        codigo = x.tim_codigo.Trim(),
                        porcentaje = x.tim_porcentaje ?? 0
                    })
                    .FirstOrDefault();

                if (query != null)
                {
                    return (query.codigo, query.porcentaje);
                }
                else
                {
                    return ("01", 0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public (string codigo, string porcentaje, string nombre) ListadoImpuestisIva(Guid tipo)
        {
            try
            {


                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_id == tipo)
                    .Select(x => new
                    {
                        codigo = x.tim_codigo,
                        porcetaje = x.tim_porcentaje,
                        nombre = x.tim_nombre
                    }).FirstOrDefault();


                return (query.codigo, string.Format("{0:N2}", query.porcetaje ?? 0), query.nombre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el nombre del impuesto a partir de su id
        /// </summary>
        /// <param name="idImpuesto"></param>
        /// <returns></returns>
        public string CargarNombreImpuesto(Guid idImpuesto)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_id == idImpuesto)
                    .Select(x => x.tim_nombre.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Carga el nombre del impuesto a partir de su id
        /// </summary>
        /// <param name="idImpuesto"></param>
        /// <returns></returns>
        public string CargarCodigoImpuestoC(Guid idImpuesto)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_id == idImpuesto)
                    .Select(x => x.tim_codigo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el id del impuesto a partir del codigo
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Guid CargarGuidImpuesto(string codigo, string nombre)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_codigo == codigo && x.tim_nombre.StartsWith(nombre))
                    .Select(x => x.tim_id).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el codigo del concepto a partir de su id
        /// </summary>
        /// <param name="idImpuesto"></param>
        /// <returns></returns>
        public string CargarCodigoConcepto(Guid concepto)
        {
            try
            {
                var query = _dbContext.sys_tipo_correcion_nota_credito
                    .Where(x => x.cnc_id == concepto)
                    .Select(x => x.cnc_codigo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga codigo de la unidad a partir  de su id
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public string CargarCodigoUnidad(Guid unidad)
        {
            try
            {
                var query = _dbContext.sys_tipo_unidad_cantidad
                    .Where(x => x.tuc_id == unidad)
                    .Select(u => u.tuc_codigo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga guid de la unidad a partir  de su codigo
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public Guid CargarGuidUnidad(string codigo)
        {
            try
            {
                var query = _dbContext.sys_tipo_unidad_cantidad
                    .Where(x => x.tuc_codigo == codigo)
                    .Select(u => u.tuc_id).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el codigo de la moneda a partir de su id
        /// </summary>
        /// <param name="moneda"></param>
        /// <returns></returns>
        public string CargarCodigoMoneda(Guid moneda)
        {
            try
            {
                var query = _dbContext.sys_moneda
                    .Where(x => x.mon_id == moneda)
                    .Select(u => u.mon_codigo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga el guid de la moneda a partir de su codigo
        /// </summary>
        /// <param name="moneda"></param>
        /// <returns></returns>
        public Guid CargarGuidMoneda(string codigo)
        {
            try
            {
                var query = _dbContext.sys_moneda
                    .Where(x => x.mon_codigo == codigo)
                    .Select(u => u.mon_id).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cargacodigo de forma de pago a partir de su id
        /// </summary>
        /// <param name="formaPago"></param>
        /// <returns></returns>
        public string CargarCodigoFormaPago(Guid formaPago)
        {
            try
            {
                var query = _dbContext.sys_tipo_forma_pago
                    .Where(x => x.tfp_id == formaPago)
                    .Select(u => u.tfp_codigo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// carga guid de forma de pago a partir de su codigo
        /// </summary>
        /// <param name="formaPago"></param>
        /// <returns></returns>
        public Guid CargarGuidFormaPago(string codigo)
        {
            try
            {
                var query = _dbContext.sys_tipo_forma_pago
                    .Where(x => x.tfp_codigo == codigo)
                    .Select(u => u.tfp_id).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga codigo de medio de pago a partir de su id
        /// </summary>
        /// <param name="medioPago"></param>
        /// <returns></returns>
        public string CargarCodigoMedioPago(Guid medioPago)
        {
            try
            {
                var query = _dbContext.sys_tipo_medio_pago
                    .Where(x => x.tmp_id == medioPago)
                    .Select(u => u.tmp_codigo.Trim()).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Carga guid de medio de pago a partir de su codigo
        /// </summary>
        /// <param name="medioPago"></param>
        /// <returns></returns>
        public Guid CargarGuidMedioPago(string codigo)
        {
            try
            {
                var query = _dbContext.sys_tipo_medio_pago
                    .Where(x => x.tmp_codigo == codigo)
                    .Select(u => u.tmp_id).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DatosEmisor CargarDatosEmisorSucursal(Guid sucursal)
        {
            try
            {

                var query = _dbContext.adm_emisor_sucursal
                    .Where(x => x.esu_id == sucursal)
                    .Select(s => new DatosEmisor
                    {
                        Ciudad = s.sys_municipio.mun_nombre.Trim(),
                        CiudadCodigo = s.sys_municipio.mun_codigo.Trim(),
                        CodigoPostal = s.sys_municipio.mun_codigo.Trim(),
                        Departamento = s.sys_departamento.dep_nombre.Trim(),
                        DepartamentoCodigo = s.sys_departamento.dep_codigo.Trim(),
                        DigitoVerificador = s.adm_emisor.emi_digito.Trim(),
                        Direccion = s.esu_direccion.Trim(),
                        email = s.esu_correo.Trim(),
                        Identificacion = s.adm_emisor.emi_identificacion.Trim(),
                        NombreComercial = s.adm_emisor.emi_nombre.Trim(),
                        NumeroMatriculaMercantil = s.adm_emisor.emi_matricula_mercantil.Trim(),
                        Pais = s.sys_departamento.sys_pais.pai_nombre_comun.Trim(),
                        PaisCodigo = s.sys_departamento.sys_pais.pai_codigo_2.Trim(),
                        RazonSocial = s.adm_emisor.emi_razon_social.Trim(),
                        Sucursal = s.esu_abreviatura.Trim(),
                        Telefono = s.esu_telefono.Trim(),
                        TipoEmisor = s.adm_emisor.sys_tipo_emisor.tem_codigo.Trim(),
                        TipoIdentificacion = s.adm_emisor.sys_tipo_identificacion.tid_codigo.Trim(),
                        TipoPersona = s.adm_emisor.sys_tipo_persona.tpe_codigo.Trim(),
                        AccessToken = s.adm_emisor.emi_access_token.Trim(),
                        ClientToken = s.adm_emisor.emi_cliente_token.Trim()

                    }).FirstOrDefault();



                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga listado de monedas para combo 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> CargarListadoMoneda()
        {
            try
            {
                var query = _dbContext.sys_moneda
                    .Where(x => x.mon_activo == true)
                    .OrderBy(x => x.mon_codigo)
                    .Select(x => new SelectListItem

                    {
                        Value = x.mon_id.ToString(),
                        Text = x.mon_divisa

                    })
                    //.OrderBy(x => x.Text)
                    .ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga listado de formas de pago para combo
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> CargarListadoFormaPago()
        {
            try
            {
                var query = _dbContext.sys_tipo_forma_pago
                    .Select(x => new SelectListItem
                    {
                        Value = x.tfp_id.ToString(),
                        Text = x.tfp_descripcion
                    })
                    .OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga listado de medios de pago para combo
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> CargarListadoMedioPago()
        {
            try
            {
                var query = _dbContext.sys_tipo_medio_pago
                    .Select(x => new SelectListItem
                    {
                        Value = x.tmp_id.ToString(),
                        Text = x.tmp_descripcion
                    })
                    .OrderBy(x => x.Text).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Valida que factura se haya creado desde facturacion manual
        /// </summary>
        /// <param name="prefijo"></param>
        /// <param name="emisor"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool ValidarFacturaEditar(string prefijo, Guid emisor, decimal numero)
        {
            try
            {

                Guid estado = Guid.Parse("C05F87B3-ABF6-475B-80B4-1520376E4531");
                var query = _dbContext.adm_documento
                    .Where(x => x.doc_prefijo == prefijo
                            && x.doc_emisor == emisor && x.doc_numero == numero && x.doc_estado != estado).Any();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Se usa para guardar el json que genera antes de enviarlo al api de facse
        /// </summary>
        /// <param name="jsonSerializado"></param>
        /// <param name="identificacionEmisor"></param>
        /// <param name="sucursal"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="prefijo"></param>
        /// <param name="numeroDocumento"></param>
        public void GuardarJson(string jsonSerializado, string identificacionEmisor, string sucursal, string tipoDocumento, string prefijo, string numeroDocumento)
        {
            try
            {
                string nombreJson = $"{identificacionEmisor}_{sucursal}_{tipoDocumento}_{prefijo}_{numeroDocumento}";
                string rutaCarpetaJson = $@"{AppDomain.CurrentDomain.BaseDirectory}\JSON\{identificacionEmisor}";
                if (!Directory.Exists(rutaCarpetaJson))
                {
                    Directory.CreateDirectory(rutaCarpetaJson);
                }
                File.WriteAllText($@"{rutaCarpetaJson}/{nombreJson}.json", jsonSerializado);
            }
            catch (Exception err)
            {
                log.Error("Error al guardar el json" + err.ToString());
            }
        }

        public string TipoMoneda(Guid idMoneda)
        {
            try
            {
                var query = _dbContext.sys_moneda.Where(x => x.mon_id == idMoneda).FirstOrDefault();

                return query.mon_divisa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Productos

        public List<Catalogo> CargarListadoCatalogoProducto(Guid idProducto)
        {
            try
            {
                var query = _dbContext.adm_emisor_producto_catalogo.Where(x => x.epc_producto == idProducto && x.adm_emisor_producto.epr_activo == true)
                    .Select(x => new Catalogo
                    {
                        Descripcion = x.adm_emisor_catalogo.eca_nombre,
                        Valor = x.epc_valor,
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  TemporalFacturacion
        public int InsertarTemporal(Guid sucursal, string idUsuario, Guid identificacion,
            string json, string receptor, decimal impuesto, decimal subtotal, decimal valorTotal, string nit, string prefijo, int numero)
        {
            try
            {
                var tabla = _dbContext.tmp_documento.Where(x => x.tdo_emisor_sucursal == sucursal);
                var idMax = (tabla.Any() ? tabla.Max(x => x.tdo_consecutivo) : 0) + 1;
                var temporal = new tmp_documento
                {
                    tdo_consecutivo = idMax,
                    tdo_id = Guid.NewGuid(),
                    tdo_emisor_sucursal = sucursal,
                    tdo_tipo_documento = identificacion,
                    tdo_json = json,
                    tdo_fecha_creacion = DateTime.Now,
                    tdo_usuario = Guid.Parse(idUsuario),
                    tdo_id_receptor = receptor,
                    tdo_id_impuesto = impuesto,
                    tdo_subtotal = subtotal,
                    tdo_valor_total = valorTotal,
                    tdo_nit = nit,
                    tdo_prefijo = prefijo,
                    tdo_numero = numero,
                };
                _dbContext.tmp_documento.Add(temporal);
                _dbContext.SaveChanges();
                return idMax;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool EditarTemporal(int consecutivo, string json, Guid sucursal, string receptor, decimal impuesto,
            decimal subtotal, decimal valorTotal, string nit, string prefijo, int numero)
        {
            try
            {
                var editada = _dbContext.tmp_documento
                    .Where(x => x.tdo_consecutivo == consecutivo && x.tdo_emisor_sucursal == sucursal)
                   .FirstOrDefault();

                editada.tdo_json = json;
                editada.tdo_id_impuesto = impuesto;
                editada.tdo_subtotal = subtotal;
                editada.tdo_valor_total = valorTotal;
                editada.tdo_fecha_creacion = DateTime.Now;

                _dbContext.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Insertar> CargaTemporal(Guid sucursal)
        {
            try
            {
                var Temporal = _dbContext.tmp_documento
                    .Where(x => x.tdo_emisor_sucursal == sucursal)
                    .Select(x => new Insertar
                    {
                        Consecutivo = x.tdo_consecutivo,
                        Ides = x.tdo_id.ToString(),
                        emisor = sucursal,
                        Identificacion = x.sys_tipo_documento.tdo_descripcion,
                        Json = x.tdo_json,
                        Fecha = x.tdo_fecha_creacion,
                        idUsuario = x.tdo_usuario.ToString(),
                        Receptor = x.tdo_id_receptor.ToString(),
                        Impuesto = x.tdo_id_impuesto,
                        Subtotal = x.tdo_subtotal,
                        ValorTotal = x.tdo_valor_total,
                        Nit = x.tdo_nit,
                        prefijo = x.tdo_prefijo,
                        numero = x.tdo_numero,
                    }).OrderBy(x => x.Consecutivo).ToList();
                return Temporal;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool EliminarTemporal(Guid Ides)
        {
            try
            {
                var eliminar = _dbContext.tmp_documento
                .Where(x => x.tdo_id == Ides).FirstOrDefault();

                _dbContext.tmp_documento.Remove(eliminar);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public List<Insertar> EditarTemporal(Guid emisor, int Consecutivo)
        {
            try
            {
                var Temporal = _dbContext.tmp_documento
                    .Where(x => x.tdo_emisor_sucursal == emisor && x.tdo_consecutivo == Consecutivo)
                    .Select(x => new Insertar
                    {
                        Consecutivo = x.tdo_consecutivo,
                        Ides = x.tdo_id.ToString(),
                        emisor = emisor,
                        Identificacion = x.sys_tipo_documento.tdo_descripcion,
                        Json = x.tdo_json,
                        Fecha = x.tdo_fecha_creacion,
                        idUsuario = x.tdo_usuario.ToString(),
                        Receptor = x.tdo_id_receptor,
                        Impuesto = x.tdo_id_impuesto,
                        Subtotal = x.tdo_subtotal,
                        ValorTotal = x.tdo_valor_total,
                        Nit = x.tdo_nit,
                        prefijo = x.tdo_prefijo,
                        numero = x.tdo_numero,
                    }).OrderBy(x => x.Consecutivo).ToList();
                return Temporal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AdquirienteReceptor CargarDatosReceptor(string RazonSocial, string NumeroIdentificacion, Guid gSucursal)
        {
            try
            {
                var Cargar = _dbContext.adm_receptor
                    .Where(x => x.rec_razon_social == RazonSocial && x.rec_identificacion == NumeroIdentificacion && x.adm_emisor.adm_emisor_sucursal.Any(e => e.esu_id == gSucursal))
                    .Select(x => new AdquirienteReceptor
                    {
                        IdReceptor = x.rec_id,
                        TipoPersona = x.rec_tipo_persona ?? Guid.Empty,
                        CodigoTipoPersona = x.sys_tipo_persona.tpe_codigo,
                        TipoAdquiriente = x.rec_tipo_receptor,
                        TipoIdentificacion = x.rec_tipo_identificacion,
                        CodigoTipoIdentificacion = x.sys_tipo_identificacion.tid_codigo,
                        NumeroIdentificacion = x.rec_identificacion,
                        Digito = x.rec_digito,
                        Nombre = x.rec_nombre,
                        RazonSocial = x.rec_razon_social,
                        IdPais = x.sys_pais.pai_id,
                        TextPais = x.sys_pais.pai_nombre_comun,
                        CodigoPais = x.sys_pais.pai_codigo_2,
                        IdDepartamento = x.sys_departamento.dep_id,
                        TextDepartamento = x.sys_departamento.dep_nombre,
                        CodigoDepartamento = x.sys_departamento.dep_codigo,
                        IdMunicipio = x.sys_municipio.mun_id,
                        TextMunicipio = x.sys_municipio.mun_nombre,
                        CodigoMunicipio = x.sys_municipio.mun_codigo,
                        Direccion = x.rec_direccion,
                        CodigoPostal = x.rec_codigo_postal,
                        Telefono = x.rec_telefono,
                        Email = x.rec_correo,
                        MatriculaMercantil = x.rec_matricula_mercantil,
                    }).FirstOrDefault();
                return Cargar;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public string CargarJSON(int consecutivo, Guid sucursal)
        {
            try
            {
                var json = _dbContext.tmp_documento
                    .Where(x => x.tdo_consecutivo == consecutivo && x.tdo_emisor_sucursal == sucursal)
                    .Select(x => x.tdo_json)
                    .FirstOrDefault();
                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<int> CargarConsecutivo(Guid sucursal)
        {
            try
            {
                var consecutivo = _dbContext.tmp_documento
                    //.Where(x => x.adm_emisor_sucursal.esu_emisor == sucursal && x.adm_emisor_sucursal.esu_emisor==emisor)
                    .Where(x => x.tdo_emisor_sucursal == sucursal)
                    .Select(x => x.tdo_consecutivo)
                    .ToList();

                return consecutivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EliminarDatosTablaEnviado(int Consecutivo, Guid sucursal)
        {
            try
            {
                var eliminar = _dbContext.tmp_documento
                    .Where(x => x.tdo_consecutivo == Consecutivo && x.tdo_emisor_sucursal == sucursal).FirstOrDefault();
                //.Where(x => x.tdo_consecutivo == Consecutivo && x.adm_emisor_sucursal.esu_emisor == sucursal).FirstOrDefault();

                _dbContext.tmp_documento.Remove(eliminar);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region Contingencia

        public List<SelectListItem> ListaContingencia()
        {
            try
            {
                var query = _dbContext.sys_tipo_contingencia
                    .Select(c => new SelectListItem
                    {
                        Value = c.tco_codigo,
                        Text = c.tco_descripcion
                    }).OrderBy(c => c.Value).ToList();

                return query;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retenciones
        public IEnumerable<Object> CargarListadoPorcentajeRetencion(Guid IdRetencion)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                    .Where(x => x.tim_id == IdRetencion)
                    .Select(x => new
                    {
                        id = x.tim_id,
                        porcentaje = x.tim_porcentaje
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<adm_tipo_retencion_fuente> CargarListadoTipoRetencion(Guid IdRetencion)
        {
            try
            {
                var query = _dbContext.adm_tipo_retencion_fuente
                    .Where(r => r.trf_id_impuesto == IdRetencion).ToList()
                    .OrderBy(r => r.trf_concepto_retencion).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Guid> ListadoImpuestoIva()
        {
            try
            {
                var listado = _dbContext.sys_tipo_impuesto
                    .Where(ti => ti.tim_nombre.ToUpper().Contains("IVA"))
                    .Select(ti => ti.tim_id)
                    .ToList();
                return listado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public decimal CargarPorcentaje(Guid gTipoRetencion)
        {
            try
            {
                return _dbContext.adm_tipo_retencion_fuente.AsNoTracking()
                    .Where(t => t.trf_id == gTipoRetencion)
                    .Select(t => t.trf_porcentajes).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }


    /// <summary>
    /// Clase para datos del catalogo 
    /// </summary>
    public class Catalogo
    {
        public Guid Id { get; set; }
        public Guid emisor { get; set; }
        public string Descripcion { get; set; }


        public string Valor { get; set; }

        public List<SelectListItem> ListadoCag { get; set; }
        public bool Lista { get; set; }
        public Guid IdProducto { get; set; }

        public string CatalogoDetalle { get; set; }
    }



}
