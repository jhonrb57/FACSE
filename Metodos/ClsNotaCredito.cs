using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Metodos
{
    public class ClsNotaCredito
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsAdquiriente).Name);

        public List<SelectListItem> CargarComboConcepto()
        {
            try
            {
                var query = _dbContext.sys_tipo_correcion_nota_credito
                    .Select(x => new SelectListItem
                    {
                        Value = x.cnc_id.ToString(),
                        Text = x.cnc_descripcion
                    }).OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public (string json, Guid? origen) ConsultarJsonFactura(Guid idEmisor, string prefijo, decimal numeroFactura)
        {
            try
            {
                var query = _dbContext.adm_documento_file
                    .Where(x => x.adm_documento.doc_emisor == idEmisor && x.adm_documento.doc_prefijo == prefijo && x.adm_documento.doc_numero == numeroFactura)
                    .Select(d => new { d.dfi_json_facse, d.adm_documento.doc_id_origen_documento }).FirstOrDefault();
                if (query != null)
                {
                    return (query.dfi_json_facse, query.doc_id_origen_documento);
                }
                else
                {
                    return (null, Guid.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ConsultarFormaPago(string codigo)
        {
            try
            {
                var query = _dbContext.sys_tipo_forma_pago
                    .Where(x => x.tfp_codigo == codigo)
                    .Select(f => f.tfp_descripcion).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ConsultarMedioPago(string codigo)
        {
            try
            {
                var query = _dbContext.sys_tipo_medio_pago
                    .Where(x => x.tmp_codigo == codigo)
                    .Select(f => f.tmp_descripcion).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ValidarNotaCredito(string prefijo, Guid emisor, decimal numero)
        {
            try
            {
                //Guid origen = Guid.Parse("AE436474-1101-46B8-BC4D-C83C23542B21");
                //Guid estado = Guid.Parse("C05F87B3-ABF6-475B-80B4-1520376E4531");

                var query = _dbContext.adm_documento
                    .Where(x => (x.sys_estado_documento.ted_codigo == "1" || x.sys_estado_documento.ted_codigo == "10") && x.doc_prefijo == prefijo && x.doc_numero == numero && x.doc_emisor == emisor)
                    .Any();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public adm_receptor ReceptorDocumento(string prefijo, Guid emisor, decimal numero)
        {
            try
            {
                var query = _dbContext.adm_documento
                    .Where(x => x.doc_prefijo == prefijo && x.doc_numero == numero && x.doc_emisor == emisor)
                    .FirstOrDefault().adm_receptor;

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #region TemporalFactura
        public string CargarJsonTemporal(Guid sucursal, int Consecutivo)
        {
            try
            {
                var query = _dbContext.tmp_documento
                    .Where(x => x.tdo_emisor_sucursal == sucursal && x.tdo_consecutivo == Consecutivo)
                    .Select(d => d.tdo_json).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region validacacionresolucion
        public bool ValidarResolucion(Guid idTipoDoc, Guid emisor, Guid idSucursal)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_emisor == emisor &&
                            x.adm_emisor_resolucion.ere_activo == true &&
                            x.adm_emisor_resolucion.ere_tipo_documento == idTipoDoc &&
                            x.esr_sucursal == idSucursal)
                           .Select(r => r.adm_emisor_resolucion.ere_prefijo.Trim())
                           .Any();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
