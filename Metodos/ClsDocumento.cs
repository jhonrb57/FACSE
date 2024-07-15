using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Metodos
{
    public class ClsDocumento
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsDocumento).Name);

        #region EmisorInicio

        /// <summary>
        /// listado de documentpso con filtros
        /// </summary>
        /// <param name="admDocumento"></param>
        /// <returns></returns>
        public List<EmisorInicio> ListadoDocumento(DateTime dFechaInicial, DateTime dFechaFinal, string sIdentificacion, string sNombre, Guid? gIdComprobante, int? iNumero, string orderColumn, String sortOrder, int MaximumRows, int StartRowIndex, ref int count, Guid gsucursal, Guid? gEstado, Guid? gEstadoFacse)
        {
            try
            {
                var listadoDocumento = (from d in _dbContext.EmisorInicio.AsNoTracking() select d);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) >= dFechaInicial.Date);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) <= dFechaFinal.Date);

                if (sIdentificacion != null && sIdentificacion != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.rec_identificacion.Contains(sIdentificacion));
                }
                if (sNombre != null && sNombre != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.rec_nombre.Contains(sNombre));
                }
                if (gIdComprobante != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_tipo_documento == gIdComprobante);
                }
                if (iNumero != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_numero == iNumero);
                }
                if (gEstado != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_estado == gEstado);
                }
                if (gEstadoFacse != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_id_estado_facse == gEstadoFacse);
                }
                listadoDocumento = listadoDocumento.Where(d => d.doc_sucursal.Equals(gsucursal));

                switch (orderColumn)
                {
                    case "doc_fecha_recepcion":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.doc_fecha_recepcion);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);
                        }
                        break;
                    case "Nombre":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.rec_nombre);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.rec_nombre);
                        }
                        break;
                    case "Identificacion":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.rec_identificacion);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.rec_identificacion);
                        }
                        break;
                    case "doc_fecha_envio":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.doc_fecha_envio);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_envio);
                        }
                        break;
                    case "doc_valor_total":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.doc_valor_total);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_valor_total);
                        }
                        break;
                    case "doc_numero":
                        if (sortOrder == "asc")
                        {
                            listadoDocumento = listadoDocumento.OrderBy(d => d.doc_numero);
                        }
                        else
                        {
                            listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_numero);
                        }
                        break;
                    default:
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);
                        break;
                }

                //var sortedFiles = SortIQueryable<adm_documento>(listadoDocumento, orderColumn, sortOrder);

                var documentoFiltrado = listadoDocumento.Skip(StartRowIndex).Take(MaximumRows);
                count = listadoDocumento.Count();

                return documentoFiltrado.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private IQueryable<T> SortIQueryable<T>(IQueryable<T> data,
      string fieldName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = Expression.Parameter(typeof(T), "i");
            Expression conversion = Expression.Convert
        (Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
                : data.OrderBy(mySortExpression);
        }


        ///// <summary>
        ///// listado de documentpso con filtros
        ///// </summary>
        ///// <param name="admDocumento"></param>
        ///// <returns></returns>
        //public List<adm_documento> ListadoDocumento(adm_documento documento, int StartRowIndex, int MaximumRows, ref int count, Guid gsucursal, string sOrdenar)
        //{
        //    try
        //    {
        //        var dfechaInicial = (documento.FechaInicial ?? DateTime.Now).Date;
        //        var dFechaFinal = (documento.FechaFinal ?? DateTime.Now).Date;

        //        _dbContext.sys_estado_documento_facse.Load();

        //        var listadoDocumento = (from d in _dbContext.adm_documento.AsNoTracking() select d);
        //        if (documento.FechaInicial.HasValue == true)
        //        {
        //            //listadoDocumento = listadoDocumento.Where(d => d.doc_fecha_recepcion >= dfechaInicial);
        //            listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) >= dfechaInicial);

        //        }
        //        if (documento.FechaFinal.HasValue == true)
        //        {
        //            //listadoDocumento = listadoDocumento.Where(d => d.doc_fecha_recepcion <= dFechaFinal);
        //            listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) <= dFechaFinal);

        //        }
        //        if (documento.Identificacion != null && documento.Identificacion != "")
        //        {
        //            listadoDocumento = listadoDocumento.Where(d => d.adm_receptor.rec_identificacion.Contains(documento.Identificacion));
        //        }
        //        if (documento.Nombre != null && documento.Nombre != "")
        //        {
        //            listadoDocumento = listadoDocumento.Where(d => d.adm_receptor.rec_nombre.Contains(documento.Nombre));
        //        }
        //        if (documento.IdComprobante != null)
        //        {
        //            listadoDocumento = listadoDocumento.Where(d => d.doc_tipo_documento == documento.IdComprobante);
        //        }
        //        if (documento.Numero != null)
        //        {
        //            listadoDocumento = listadoDocumento.Where(d => d.doc_numero == documento.Numero);
        //        }
        //        listadoDocumento = listadoDocumento.Where(d => d.doc_sucursal.Equals(gsucursal));

        //        //listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);

        //        switch (sOrdenar)
        //        {
        //            case "FechaRecepcion":
        //                listadoDocumento = listadoDocumento.OrderBy(d => d.doc_fecha_recepcion);
        //                break;
        //            case "Nombre":
        //                listadoDocumento = listadoDocumento.OrderBy(d => d.adm_receptor.rec_nombre);
        //                break;
        //            case "Nombre_desc":
        //                listadoDocumento = listadoDocumento.OrderByDescending(d => d.adm_receptor.rec_nombre);
        //                break;
        //            case "Identificacion":
        //                listadoDocumento = listadoDocumento.OrderBy(d => d.adm_receptor.rec_identificacion);
        //                break;
        //            case "Identificacion_desc":
        //                listadoDocumento = listadoDocumento.OrderByDescending(d => d.adm_receptor.rec_identificacion);
        //                break;
        //            case "FechaEnvio":
        //                listadoDocumento = listadoDocumento.OrderBy(d => d.doc_fecha_envio);
        //                break;
        //            case "FechaEnvio_desc":
        //                listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_envio);
        //                break;
        //            case "Total":
        //                listadoDocumento = listadoDocumento.OrderBy(d => d.doc_valor_total);
        //                break;
        //            case "Total_desc":
        //                listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_valor_total);
        //                break;
        //            case "Numero":
        //                listadoDocumento = listadoDocumento.OrderBy(d => d.doc_numero);
        //                break;
        //            case "Numero_desc":
        //                listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_numero);
        //                break;
        //            default:
        //                listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);
        //                break;
        //        }

        //        var documentoFiltrado = listadoDocumento.Skip((StartRowIndex - 1) * MaximumRows).Take(MaximumRows);
        //        count = listadoDocumento.Count();

        //        return documentoFiltrado.ToList();
        //        //return documentoFiltrado.AsEnumerable().Select(d => new adm_documento
        //        //{
        //        //    doc_id = d.doc_id,
        //        //    NombreComprobante = d.sys_tipo_documento == null ? "" : d.sys_tipo_documento.tdo_abreviatura.ToLower(),
        //        //    doc_prefijo = d.doc_prefijo,
        //        //    doc_numero = d.doc_numero,
        //        //    Identificacion = d.adm_receptor == null ? "" : d.adm_receptor.rec_identificacion,
        //        //    Nombre = d.adm_receptor == null ? "" : d.adm_receptor.rec_nombre,
        //        //    NitEmisor = d.adm_emisor == null ? "" : d.adm_emisor.emi_identificacion,
        //        //    doc_receptor = d.doc_receptor,
        //        //    doc_fecha_envio = d.doc_fecha_envio,
        //        //    doc_valor_total = d.doc_valor_total,
        //        //    NombreUsuario = d.doc_usuario,
        //        //    doc_fecha_recepcion = d.doc_fecha_recepcion,
        //        //    Abreviatura = d.adm_emisor_sucursal == null ? "" : d.adm_emisor_sucursal.esu_abreviatura,
        //        //    RutaXml = d.adm_documento_file.FirstOrDefault()?.dfi_xml,
        //        //    RutaZip = d.adm_documento_file.FirstOrDefault()?.dfi_ruta_zip,
        //        //    TipoDocumentoCompleto = d.sys_tipo_documento == null ? "" : d.sys_tipo_documento.tdo_descripcion.ToLower(),
        //        //    CodigoEstado = d.sys_estado_documento == null ? "" : d.sys_estado_documento.ted_codigo,
        //        //    adm_documento_notificacion = d.adm_documento_notificacion,
        //        //    doc_usuario = d.doc_usuario,
        //        //    RutaEstadoFacse = (d.sys_estado_documento_facse == null ? "" : d.sys_estado_documento_facse.edf_ruta_imagen),
        //        //    adm_documento_estado_facse = d.adm_documento_estado_facse,
        //        //    adm_documento_correo = d.adm_documento_correo,
        //        //    RutaJson = d.adm_documento_file.FirstOrDefault()?.dfi_json_facse,
        //        //    CorreoReceptor = d.adm_receptor?.rec_correo,
        //        //    adm_documentos_anexo = d.adm_documentos_anexo
        //        //}).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// listado de documentpso con excel
        /// </summary>
        /// <param name="admDocumento"></param>
        /// <returns></returns>
        public List<EmisorInicio> ListadoDocumentoExcel(DateTime dFechaInicial, DateTime dFechaFinal, string sIdentificacion, string sNombre, Guid? gIdComprobante, int? iNumero, Guid gsucursal, Guid? gEstado, Guid? gEstadoFacse)
        {
            try
            {
                var listadoDocumento = (from d in _dbContext.EmisorInicio.AsNoTracking() select d);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) >= dFechaInicial.Date);
                listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) <= dFechaFinal.Date);

                if (sIdentificacion != null && sIdentificacion != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.rec_identificacion.Contains(sIdentificacion));
                }
                if (sNombre != null && sNombre != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.rec_nombre.Contains(sNombre));
                }
                if (gIdComprobante != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_tipo_documento == gIdComprobante);
                }
                if (iNumero != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_numero == iNumero);
                }
                if (gEstado != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_estado == gEstado);
                }
                if (gEstadoFacse != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_id_estado_facse == gEstadoFacse);
                }
                listadoDocumento = listadoDocumento.Where(d => d.doc_sucursal.Equals(gsucursal));
                listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);

                return listadoDocumento.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cargar informacion factura general
        /// </summary>
        /// <param name="iSucursal"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<adm_documento> GeneralDocumento(Guid gSucursal)
        {
            try
            {
                var documento = _dbContext.adm_documento.AsNoTracking()
                    .Where(d => d.doc_sucursal == gSucursal && d.doc_fecha_recepcion.Month == DateTime.Now.Month
                    && d.doc_fecha_recepcion.Year == DateTime.Now.Year)
                    .GroupBy(d => d.sys_tipo_documento.tdo_codigo)
                    .AsEnumerable()
                    .Select(d => new adm_documento { CodigoTipoDocumento = d.Key, doc_fecha_recepcion = d.Max(m => m.doc_fecha_recepcion), doc_numero = d.Count() })
                    .ToList();

                return documento;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public decimal FacturasRestantes(Guid gEmisor, Guid gTipoDocuemto, Guid igSucursal)
        {
            try
            {
                decimal totalDocumento = 0;
                var documento = (_dbContext.adm_documento.AsNoTracking()
                .Where(d => d.doc_emisor == gEmisor && d.doc_tipo_documento == gTipoDocuemto && d.doc_sucursal == igSucursal));

                if (documento.Any())
                {
                    totalDocumento = documento.Max(d => d.doc_numero);
                }

                var emisorresolucion = _dbContext.adm_emisor_sucursal_resolucion.AsNoTracking()
                 .Where(es => es.adm_emisor_resolucion.ere_activo && es.adm_emisor_resolucion.ere_tipo_documento == gTipoDocuemto
                         && es.esr_sucursal == igSucursal && es.esr_emisor == gEmisor);

                var resutadoFinal = "0";

                if (emisorresolucion.Any())
                {
                    resutadoFinal = emisorresolucion.FirstOrDefault().adm_emisor_resolucion.ere_numero_final;
                }

                return (decimal.Parse(resutadoFinal ?? "0")) - decimal.Parse(totalDocumento.ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// validar resolucion vencer
        /// </summary>
        /// <param name="gEmisor"></param>
        /// <returns></returns>
        public List<adm_emisor_resolucion> ResolucionVencer(Guid gEmisor, Guid igSucursal, Guid tipo)
        {
            try
            {

                var emisorresolucion = _dbContext.adm_emisor_sucursal_resolucion.AsNoTracking()
                    .Where(es => es.adm_emisor_resolucion.ere_activo && es.adm_emisor_resolucion.ere_tipo_documento == tipo
                            && es.esr_sucursal == igSucursal && es.esr_emisor == gEmisor)
                    .AsEnumerable();

                return emisorresolucion.GroupBy(es => es.adm_emisor_resolucion.ere_prefijo).Select(es => new adm_emisor_resolucion
                {
                    ere_fecha_final = es.Min(min => min.adm_emisor_resolucion.ere_fecha_final),
                    NombreDocumento = es.Key
                }).OrderBy(es => es.ere_fecha_final)
                .ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        /// <summary>
        /// traer informacion de documento file
        /// </summary>
        /// <param name="gDocumento"></param>
        /// <returns></returns>
        public adm_documento_file ArchivoDocumento(Guid gDocumento)
        {
            try
            {
                var file = from df in _dbContext.adm_documento_file.AsNoTracking()
                           where df.dfi_documento == gDocumento
                           select df;

                return file.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// resolucion activa
        /// </summary>
        /// <param name="gDocumento"></param>
        /// <param name="gTipoDocumento"></param>
        /// <returns></returns>
        public adm_emisor_resolucion ResolucionActiva(Guid gTipoDocumento, Guid gEmisor)
        {
            try
            {
                var resolucionEmisor = from re in _dbContext.adm_emisor_resolucion.AsNoTracking()
                                       where re.ere_emisor == gEmisor && re.ere_tipo_documento == gTipoDocumento
                                       && re.ere_activo == true
                                       select re;

                return resolucionEmisor.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region ViewPdf

        public bool ValidarEstadoDocumento(Guid documento)
        {
            try
            {
                Guid estado10 = Guid.Parse("C05F87B3-ABF6-475B-80B4-1520376E4531");
                Guid estadoOk = Guid.Parse("EBF6D2FE-9AD5-4728-B0BA-4846E3F89147");
                var query = _dbContext.adm_documento.Where(x => x.doc_id == documento && (x.doc_estado == estadoOk || x.doc_estado == estado10))
                    .Any();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public string CargarUrlDocumentoXml(Guid gdocumento)
        {
            try
            {
                var docuet = _dbContext.adm_documento_file
                    .Where(x => x.dfi_documento == gdocumento)
                    .FirstOrDefault();

                return docuet.dfi_ruta_zip;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        public string WriteFileFinal(Guid gDocumento, string sRuta)
        {
            string outFile = sRuta;
            try
            {
                var sJson = ArchivoDocumento(gDocumento).dfi_json_facse;

                //se agrega el contenido 
                List<String> fileToWrite = new List<String>
                {
                    sJson
                };


                try
                {
                    using (FileStream fileStream = File.Create(outFile))
                    {
                        using (StreamWriter sw = new StreamWriter(fileStream, Encoding.GetEncoding(1252), 1024))
                        {
                            foreach (var x in fileToWrite)
                            {
                                sw.WriteLine(x.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error escribiendo registros en " + outFile + " " + ex);

                    throw ex;

                }
            }
            catch (Exception e)
            {
                log.Error("Error escribiendo archivo unificado: " + e);

                throw;
            }

            return outFile;
            //return outFile;
        }

        #region  "Receptor Inicio"

        /// <summary>
        /// listado de documentpso con filtros
        /// </summary>
        /// <param name="admDocumento"></param>
        /// <returns></returns>
        public List<adm_documento> ListadoDocumentoReceptor(adm_documento documento, int StartRowIndex, int MaximumRows, ref int count, Guid gReceptor, string sOrdenar)
        {
            try
            {
                var dfechaInicial = (documento.FechaInicial ?? DateTime.Now).Date;
                var dFechaFinal = (documento.FechaFinal ?? DateTime.Now).Date;

                var listadoDocumento = (from d in _dbContext.adm_documento.AsNoTracking() select d);
                if (documento.FechaInicial.HasValue == true)
                {
                    listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) >= dfechaInicial);
                }
                if (documento.FechaFinal.HasValue == true)
                {
                    listadoDocumento = listadoDocumento.Where(d => DbFunctions.TruncateTime(d.doc_fecha_recepcion) <= dFechaFinal);
                }
                if (documento.Identificacion != null && documento.Identificacion != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.adm_receptor.rec_identificacion.Contains(documento.Identificacion));
                }
                if (documento.Nombre != null && documento.Nombre != "")
                {
                    listadoDocumento = listadoDocumento.Where(d => d.adm_receptor.rec_nombre.Contains(documento.Nombre));
                }
                if (documento.IdComprobante != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_tipo_documento == documento.IdComprobante);
                }
                if (documento.Numero != null)
                {
                    listadoDocumento = listadoDocumento.Where(d => d.doc_numero == documento.Numero);
                }
                listadoDocumento = listadoDocumento.Where(d => d.adm_receptor.adm_usuario_receptor.ure_id.Equals(gReceptor));

                //listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);

                switch (sOrdenar)
                {
                    case "FechaRecepcion":
                        listadoDocumento = listadoDocumento.OrderBy(d => d.doc_fecha_recepcion);
                        break;
                    case "Nombre":
                        listadoDocumento = listadoDocumento.OrderBy(d => d.adm_receptor.rec_nombre);
                        break;
                    case "Nombre_desc":
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.adm_receptor.rec_nombre);
                        break;
                    case "Identificacion":
                        listadoDocumento = listadoDocumento.OrderBy(d => d.adm_receptor.rec_identificacion);
                        break;
                    case "Identificacion_desc":
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.adm_receptor.rec_identificacion);
                        break;
                    case "FechaEnvio":
                        listadoDocumento = listadoDocumento.OrderBy(d => d.doc_fecha_envio);
                        break;
                    case "FechaEnvio_desc":
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_envio);
                        break;
                    case "Total":
                        listadoDocumento = listadoDocumento.OrderBy(d => d.doc_valor_total);
                        break;
                    case "Total_desc":
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_valor_total);
                        break;
                    case "Numero":
                        listadoDocumento = listadoDocumento.OrderBy(d => d.doc_numero);
                        break;
                    case "Numero_desc":
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_numero);
                        break;
                    default:
                        listadoDocumento = listadoDocumento.OrderByDescending(d => d.doc_fecha_recepcion);
                        break;
                }

                var documentoFiltrado = listadoDocumento.Skip((StartRowIndex - 1) * MaximumRows).Take(MaximumRows);
                count = listadoDocumento.Count();

                return documentoFiltrado.AsEnumerable().Select(d => new adm_documento
                {
                    doc_id = d.doc_id,
                    NombreComprobante = d.sys_tipo_documento == null ? "" : d.sys_tipo_documento.tdo_abreviatura.ToLower(),
                    doc_prefijo = d.doc_prefijo,
                    doc_numero = d.doc_numero,
                    Identificacion = d.adm_receptor == null ? "" : d.adm_receptor.rec_identificacion,
                    Nombre = d.adm_receptor == null ? "" : d.adm_receptor.rec_nombre,
                    NitEmisor = d.adm_emisor == null ? "" : d.adm_emisor.emi_identificacion,
                    doc_receptor = d.doc_receptor,
                    doc_fecha_envio = d.doc_fecha_envio,
                    doc_valor_total = d.doc_valor_total,
                    NombreUsuario = d.doc_usuario,
                    doc_fecha_recepcion = d.doc_fecha_recepcion,
                    Abreviatura = d.adm_emisor_sucursal == null ? "" : d.adm_emisor_sucursal.esu_abreviatura,
                    RutaXml = d.adm_documento_file.FirstOrDefault()?.dfi_xml,
                    TipoDocumentoCompleto = d.sys_tipo_documento == null ? "" : d.sys_tipo_documento.tdo_descripcion.ToLower(),
                    CodigoEstado = d.sys_estado_documento == null ? "" : d.sys_estado_documento.ted_codigo,
                    adm_documento_notificacion = d.adm_documento_notificacion,
                    doc_usuario = d.doc_usuario,
                    RutaEstadoFacse = (d.sys_estado_documento_facse == null ? "" : d.sys_estado_documento_facse.edf_ruta_imagen),
                    adm_documento_estado_facse = d.adm_documento_estado_facse,
                    adm_documento_correo = d.adm_documento_correo,
                    RutaJson = d.adm_documento_file.FirstOrDefault()?.dfi_json_facse,
                    CorreoReceptor = d.adm_receptor?.rec_correo,
                    adm_documentos_anexo = d.adm_documentos_anexo
                }).ToList();
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
        public List<InicioReport> ListadoDocumentoExcelReceptor(adm_documento documento, Guid gReceptor)
        {
            try
            {

                var dfechaInicial = (documento.FechaInicial ?? DateTime.Now).Date;
                var dFechaFinal = (documento.FechaFinal ?? DateTime.Now).Date;

                var listadoDocumento = (from d in _dbContext.adm_documento.AsNoTracking()
                                        where (!documento.FechaInicial.HasValue ? 1 == 1 : DbFunctions.TruncateTime(d.doc_fecha_recepcion) >= dfechaInicial)
                                        && (!documento.FechaFinal.HasValue ? 1 == 1 : DbFunctions.TruncateTime(d.doc_fecha_recepcion) <= dFechaFinal)
                                        && (documento.Identificacion == null || documento.Identificacion == "" ? 1 == 1 : d.adm_receptor.rec_identificacion.Contains(documento.Identificacion))
                                        && (documento.Nombre == null || documento.Nombre == "" ? 1 == 1 : d.adm_receptor.rec_nombre.Contains(documento.Nombre))
                                        && (documento.IdComprobante == null ? 1 == 1 : d.doc_tipo_documento == documento.IdComprobante)
                                        && (documento.Numero == null ? 1 == 1 : d.doc_numero == documento.Numero)
                                        && d.doc_receptor.Equals(gReceptor)
                                        select d).AsEnumerable();

                return listadoDocumento.Select(d => new InicioReport
                {
                    FechaEmision = d.doc_fecha_recepcion,
                    FechaEnvio = d.doc_fecha_envio,
                    Identificacion = d.adm_receptor == null ? "" : d.adm_receptor.rec_identificacion,
                    NitEmisor = d.adm_emisor == null ? "" : d.adm_emisor.emi_identificacion,
                    Nombre = d.adm_receptor == null ? "" : d.adm_receptor.rec_nombre,
                    Numero = d.doc_numero.ToString(),
                    Prefijo = d.doc_prefijo,
                    Tipo = d.sys_tipo_documento == null ? "" : d.sys_tipo_documento.tdo_abreviatura.ToLower(),
                    Total = d.doc_valor_total,
                    Usuario = d.doc_usuario
                }).OrderByDescending(d => d.Numero).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Listado estado fasce de documento
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public List<adm_documento_estado_facse> ListadoEstadoFacse(Guid gId)
        {
            try
            {
                return _dbContext.adm_documento_estado_facse.AsNoTracking().Where(e => e.def_id_documento == gId).OrderByDescending(n => n.def_fecha).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_documento_notificacion> ListadoNotificacion(Guid gId)
        {
            try
            {
                return _dbContext.adm_documento_notificacion.AsNoTracking().Where(e => e.dno_documento == gId).OrderByDescending(n => n.dno_fecha).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_documento_correo> ListadoCorreo(Guid gId)
        {
            try
            {
                return _dbContext.adm_documento_correo.AsNoTracking().Where(e => e.dco_documento == gId).OrderByDescending(n => n.dco_fecha).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_documentos_anexo> ListadoAnexo(Guid gId)
        {
            try
            {
                return _dbContext.adm_documentos_anexo.AsNoTracking().Where(e => e.dan_documento == gId).OrderByDescending(n => n.dan_nombre).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
