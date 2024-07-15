using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Metodos
{
    public class ClsDocumentoProveedor
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsDocumentoProveedor).Name);

        /// <summary>
        /// listado de documentpso con filtros
        /// </summary>
        /// <param name="admDocumento"></param>
        /// <returns></returns>
        public List<EmisorInicioProveedor> ListadoDocumento(DateTime dFechaInicial, DateTime dFechaFinal, string sIdentificacion, string sNombre, Guid? gIdComprobante, int? iNumero, string orderColumn, String sortOrder, int MaximumRows, int StartRowIndex, ref int count, Guid gsucursal, Guid? gEstadoFacse)
        {
            try
            {
                var listadoDocumento = (from d in _dbContext.EmisorInicioProveedor.AsNoTracking() select d);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.dpr_fecha_recepcion) >= dFechaInicial.Date);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.dpr_fecha_recepcion) <= dFechaFinal.Date);

                if (sIdentificacion != null && sIdentificacion != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.pro_identificacion.Contains(sIdentificacion));
                }
                if (sNombre != null && sNombre != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.pro_nombre.Contains(sNombre));
                }
                if (gIdComprobante != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.dpr_tipo_documento == gIdComprobante);
                }
                if (iNumero != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.dpr_numero == iNumero);
                }
                if (gEstadoFacse != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.dpr_id_estado_facse == gEstadoFacse);
                }
                listadoDocumento = listadoDocumento.Where(d => d.dpr_sucursal.Equals(gsucursal));

                switch (orderColumn)
                {
                    case "dpr_fecha_recepcion":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.dpr_fecha_recepcion);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.dpr_fecha_recepcion);
                        }
                        break;
                    case "Nombre":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.pro_nombre);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.pro_nombre);
                        }
                        break;
                    case "Identificacion":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.pro_identificacion);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.pro_identificacion);
                        }
                        break;
                    case "dpr_fecha_envio":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.dpr_fecha_envio);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.dpr_fecha_envio);
                        }
                        break;
                    case "dpr_valor_total":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.dpr_valor_total);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.dpr_valor_total);
                        }
                        break;
                    case "dpr_numero":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.dpr_numero);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.dpr_numero);
                        }
                        break;
                    default:
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.dpr_fecha_recepcion);
                        break;
                }

                //var sortedFiles = SortIQueryable<adm_documento>(listadoDocumento, orderColumn, sortOrder);

                var documentoFiltrado = listadoDocumento.Skip(StartRowIndex).Take(MaximumRows);
                count = listadoDocumento.Count();

                return documentoFiltrado.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado de documentpso con excel
        /// </summary>
        /// <param name="admDocumento"></param>
        /// <returns></returns>
        public List<EmisorInicioProveedor> ListadoDocumentoExcel(DateTime dFechaInicial, DateTime dFechaFinal, string sIdentificacion, string sNombre, Guid? gIdComprobante, int? iNumero, Guid gsucursal, Guid? gEstadoFacse)
        {
            try
            {
                var listadoDocumento = (from d in _dbContext.EmisorInicioProveedor.AsNoTracking() select d);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.dpr_fecha_recepcion) >= dFechaInicial.Date);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.dpr_fecha_recepcion) <= dFechaFinal.Date);

                if (sIdentificacion != null && sIdentificacion != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.pro_identificacion.Contains(sIdentificacion));
                }
                if (sNombre != null && sNombre != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.pro_nombre.Contains(sNombre));
                }
                if (gIdComprobante != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.dpr_tipo_documento == gIdComprobante);
                }
                if (iNumero != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.dpr_numero == iNumero);
                }
                if (gEstadoFacse != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.dpr_id_estado_facse == gEstadoFacse);
                }
                listadoDocumento = listadoDocumento.Where(d => d.dpr_sucursal.Equals(gsucursal));
                listadoDocumento = listadoDocumento.OrderByDescending(d => d.dpr_fecha_recepcion);

                return listadoDocumento.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// aceptar o rechazar documento
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="bValidacion"></param>
        public void AceptadoRechazo(Guid idDocumento, bool bValidacion)
        {
            try
            {
                var estadoFasce = _dbContext.sys_estado_documento_facse.AsNoTracking();
                var documentoProveeedor = _dbContext.adm_documento_proveedor.Where(dp => dp.dpr_id == idDocumento).SingleOrDefault();

                Guid idEstadoFasce;
                if (bValidacion)
                {
                    idEstadoFasce = estadoFasce.Where(e => e.edf_codigo == "03").FirstOrDefault().edf_id;
                }
                else
                {
                    idEstadoFasce = estadoFasce.Where(e => e.edf_codigo == "06").FirstOrDefault().edf_id;
                }

                documentoProveeedor.dpr_id_estado_facse = idEstadoFasce;

                var documentoEstado = new adm_documento_proveedor_estado_facse
                {
                    dpe_id = Guid.NewGuid(),
                    dpe_fecha = DateTime.Now,
                    dpe_id_estado = idEstadoFasce,
                    dpe_documento_proveedor = idDocumento
                };

                _dbContext.adm_documento_proveedor_estado_facse.Add(documentoEstado);

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Listado estado fasce de documento proveedores
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public List<adm_documento_proveedor_estado_facse> ListadoEstadoFacse(Guid gId)
        {
            try
            {
                return _dbContext.adm_documento_proveedor_estado_facse.AsNoTracking().Where(e => e.dpe_documento_proveedor == gId).OrderByDescending(n => n.dpe_fecha).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado de anexos documentos
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public List<adm_documento_proveedor_anexo> ListadoAnexo(Guid gId)
        {
            try
            {
                return _dbContext.adm_documento_proveedor_anexo.AsNoTracking().Where(e => e.dpa_documento_proveedor == gId).OrderByDescending(n => n.dpa_nombre).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
