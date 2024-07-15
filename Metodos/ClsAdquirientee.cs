using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Metodos
{
    public class ClsAdquiriente
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsAdquiriente).Name);

        /// <summary>
        /// cargar listado adquirientes
        /// </summary>
        /// <param name="sNombre"></param>
        /// <param name="sIdentificacion"></param>
        /// <returns></returns>
        public List<adm_receptor> ListadoAdquiriente(string sNombre, string sIdentificacion)
        {
            try
            {
                var adquiriente = from a in _dbContext.adm_receptor.AsNoTracking()
                                  where (sNombre == null || sNombre == "" ? 1 == 1 : a.rec_nombre.Contains(sNombre) &&
                                  sIdentificacion == null || sIdentificacion == "" ? 1 == 1 : a.rec_identificacion.Equals(sIdentificacion))
                                  select a;

                return adquiriente.AsEnumerable().Select(a => new adm_receptor
                {
                    rec_nombre = a.rec_nombre.ToLower(),
                    rec_identificacion = a.rec_identificacion,
                    rec_id = a.rec_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Carga el listado para combo de tipo personas
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListadoTipoPersona()
        {
            try
            {
                var query = _dbContext.sys_tipo_persona
                    .Select(x => new SelectListItem
                    {
                        Value = x.tpe_id.ToString(),
                        Text = x.tpe_descripcion
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Carga el listado para combo de tipo emisor
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListadoTipoEmisor()
        {
            try
            {
                var query = _dbContext.sys_tipo_emisor
                    .Select(x => new SelectListItem
                    {
                        Value = x.tem_id.ToString(),
                        Text = x.tem_descripcion
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cargar listado de paises
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListadoPaises()
        {
            try
            {
                var query = _dbContext.sys_pais
                    .Select(x => new SelectListItem
                    {
                        Value = x.pai_id.ToString(),
                        Text = x.pai_nombre_comun
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
        /// Carga listado de depoartamentos a partir de un pais
        /// </summary>
        /// <param name="pais"></param>
        /// <returns></returns>
        public List<SelectListItem> ListadoDptos(Guid? pais)
        {
            try
            {
                var query = _dbContext.sys_departamento
                    .Where(d => d.dep_id_pais == pais)
                    .Select(x => new SelectListItem
                    {
                        Value = x.dep_id.ToString(),
                        Text = x.dep_nombre
                    })
                    .OrderBy(x => x.Text).ToList();

                if (query.Count == 0)
                {
                    query.Insert(0, new SelectListItem { Value = null, Text = "No aplica" });
                }

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga listado de municiopios a partir del departamento
        /// </summary>
        /// <param name="dpto"></param>
        /// <returns></returns>
        public List<SelectListItem> ListadoMunicipios(Guid? dpto)
        {
            try
            {
                var query = _dbContext.sys_municipio
                    .Where(x => x.mun_id_dpto == dpto)
                    .Select(m => new SelectListItem
                    {
                        Value = m.mun_id.ToString(),
                        Text = m.mun_nombre
                    })
                    .OrderBy(x => x.Text).ToList();

                if (query.Count == 0)
                {
                    query.Insert(0, new SelectListItem { Value = null, Text = "No aplica" });
                }

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// carga listado de tipos identificaciones
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListadOTipoIdentificacion()
        {
            try
            {
                var query = _dbContext.sys_tipo_identificacion
                    .Select(x => new SelectListItem
                    {
                        Value = x.tid_id.ToString(),
                        Text = x.tid_descripcion
                    })
                    .OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public adm_receptor GuardarAdquiriente(adm_receptor receptor)
        {
            try
            {
                var query = new adm_receptor
                {
                    rec_activo = true,
                    rec_codigo_postal = receptor.rec_codigo_postal,
                    rec_correo = receptor.rec_correo,
                    rec_departamento = receptor.rec_departamento,
                    rec_digito = receptor.rec_digito,
                    rec_direccion = receptor.rec_direccion,
                    rec_emisor = receptor.rec_emisor,
                    rec_fecha_receccion = receptor.rec_fecha_receccion,
                    rec_id = Guid.NewGuid(),
                    rec_identificacion = receptor.rec_identificacion,
                    rec_municipio = receptor.rec_municipio,
                    rec_nombre = receptor.rec_nombre,
                    rec_pais = receptor.rec_pais,
                    rec_razon_social = receptor.rec_razon_social,
                    rec_telefono = receptor.rec_telefono,
                    rec_tipo_identificacion = receptor.rec_tipo_identificacion,
                    rec_tipo_persona = receptor.rec_tipo_persona,
                    rec_tipo_receptor = receptor.rec_tipo_receptor,
                    rec_matricula_mercantil = receptor.rec_matricula_mercantil
                };

                _dbContext.adm_receptor.Add(query);
                _dbContext.SaveChanges();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ActualizarReceptor(adm_receptor receptor)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => x.rec_id == receptor.rec_id).FirstOrDefault();

                query.rec_tipo_receptor = receptor.rec_tipo_receptor;
                query.rec_tipo_persona = receptor.rec_tipo_persona;
                query.rec_tipo_identificacion = receptor.rec_tipo_identificacion;
                query.rec_identificacion = receptor.rec_identificacion;
                query.rec_digito = receptor.rec_digito;
                query.rec_nombre = receptor.rec_nombre;
                query.rec_razon_social = receptor.rec_razon_social;
                query.rec_pais = receptor.rec_pais;
                query.rec_departamento = receptor.rec_departamento;
                query.rec_municipio = receptor.rec_municipio;
                query.rec_codigo_postal = receptor.rec_codigo_postal;
                query.rec_correo = receptor.rec_correo;
                query.rec_direccion = receptor.rec_direccion;
                query.rec_telefono = receptor.rec_telefono;
                query.rec_matricula_mercantil = receptor.rec_matricula_mercantil;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ValidarReceptor(string identificacion, Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => x.rec_identificacion == identificacion.Trim() && x.rec_emisor == emisor)
                    .Count();

                if (query == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdquirienteReceptor CargarDatosReceptor(Guid? idReceptor)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => x.rec_id == idReceptor)
                    .Select(r => new AdquirienteReceptor
                    {
                        IdReceptor = r.rec_id,
                        Digito = r.rec_digito,
                        Direccion = r.rec_direccion,
                        Email = r.rec_correo,
                        IdDepartamento = r.rec_departamento ?? Guid.Empty,
                        IdMunicipio = r.rec_municipio ?? Guid.Empty,
                        IdPais = r.rec_pais ?? Guid.Empty,
                        Nombre = r.rec_nombre,
                        NumeroIdentificacion = r.rec_identificacion,
                        RazonSocial = r.rec_razon_social,
                        Telefono = r.rec_telefono,
                        TipoAdquiriente = r.rec_tipo_receptor,
                        TipoIdentificacion = r.rec_tipo_identificacion,
                        TipoPersona = r.rec_tipo_persona ?? Guid.Empty,
                        CodigoPostal = r.rec_codigo_postal,
                        MatriculaMercantil = r.rec_matricula_mercantil
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> ListadoCiiu()
        {
            try
            {
                var query = _dbContext.sys_ciiu
                    .Select(x => new SelectListItem
                    {
                        Text = x.cii_descripcion,
                        Value = x.cii_id.ToString()
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void GuardarLogReceptor(adm_receptor receptor, Guid tipoAccion, Guid emisor, Guid idreceptor)
        {
            try
            {
                var query = new AuditLogReceptor
                {
                    alc_activo = true,
                    alc_codigo_postal = receptor.rec_codigo_postal,
                    alc_correo = receptor.rec_correo,
                    alc_departamento = receptor.rec_departamento,
                    alc_digito = receptor.rec_digito,
                    alc_direccion = receptor.rec_direccion,
                    alc_emisor_accion = emisor,
                    alc_fecha_accion = DateTime.Now,
                    alc_id = Guid.NewGuid(),
                    alc_identificacion = receptor.rec_identificacion,
                    alc_matricula_mercantil = receptor.rec_matricula_mercantil,
                    alc_municipio = receptor.rec_municipio,
                    alc_nombre = receptor.rec_nombre,
                    alc_pais = receptor.rec_pais,
                    alc_razon_social = receptor.rec_razon_social,
                    alc_tipo_accion = tipoAccion,
                    alc_telefono = receptor.rec_telefono,
                    alc_tipo_identificacion = receptor.rec_tipo_identificacion,
                    alc_tipo_persona = receptor.rec_tipo_persona,
                    alc_tipo_receptor = receptor.rec_tipo_receptor,
                    alc_receptor = idreceptor

                };
                _dbContext.AuditLogReceptor.Add(query);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SelectListItem> ResponsabilidadFiscal()
        {
            try
            {
                var res = _dbContext.sys_tipo_emisor
                    .Select(r => new SelectListItem
                    {
                        Text = r.tem_descripcion,
                        Value = r.tem_codigo
                    }).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
