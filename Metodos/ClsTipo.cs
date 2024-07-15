using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metodos
{
    public class ClsTipo
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsTipo).Name);

        /// <summary>
        /// listado tipo comprobante
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_documento> ListadoTipocomprobante()
        {
            try
            {
                var comprobante = from c in _dbContext.sys_tipo_documento.AsNoTracking()
                                  orderby c.tdo_descripcion
                                  select c;

                return comprobante.AsEnumerable().Select(c => new sys_tipo_documento
                {
                    tdo_id = c.tdo_id,
                    tdo_descripcion = c.tdo_descripcion.ToLower()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado grupo emisor
        /// </summary>
        /// <returns></returns>
        public List<adm_grupo_emisor> ListadoGrupoEmisor(Guid gDistribuidor)
        {
            try
            {
                var grupoEmisor = _dbContext.adm_emisor.Where(e => e.emi_distribuidor == gDistribuidor).Select(e => e.emi_grupo);

                var grupo = from g in _dbContext.adm_grupo_emisor.AsNoTracking()
                            where g.gen_activo == true && grupoEmisor.Contains(g.gen_id)
                            orderby g.gen_nombre
                            select g;

                return grupo.AsEnumerable().Select(g => new adm_grupo_emisor
                {
                    gen_nombre = g.gen_nombre,
                    gen_id = g.gen_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado tipo persona
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_persona> ListadoTipoPersona()
        {
            try
            {
                var tipoPersona = from tp in _dbContext.sys_tipo_persona.AsNoTracking()
                                  orderby tp.tpe_descripcion
                                  select tp;

                return tipoPersona.AsEnumerable().Select(tp => new sys_tipo_persona
                {
                    tpe_descripcion = tp.tpe_descripcion,
                    tpe_id = tp.tpe_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado tipo identificacion
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_identificacion> ListadoTipoIdentificacion()
        {
            try
            {
                var tipoIdentificacion = from ti in _dbContext.sys_tipo_identificacion.AsNoTracking()
                                         orderby ti.tid_descripcion
                                         select ti;

                return tipoIdentificacion.AsEnumerable().Select(ti => new sys_tipo_identificacion
                {
                    tid_descripcion = ti.tid_descripcion,
                    tid_id = ti.tid_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado pais
        /// </summary>
        /// <returns></returns>
        public List<sys_pais> ListadoPais()
        {
            try
            {
                var pais = from p in _dbContext.sys_pais.AsNoTracking()
                           orderby p.pai_nombre_comun
                           select p;

                return pais.AsEnumerable().Select(p => new sys_pais
                {
                    pai_nombre_comun = p.pai_nombre_comun,
                    pai_id = p.pai_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado departamento
        /// </summary>
        /// <param name="gPais"></param>
        /// <returns></returns>
        public List<sys_departamento> ListadoDepartamento(Guid gPais)
        {
            try
            {
                var departamento = from d in _dbContext.sys_departamento.AsNoTracking()
                                   where d.dep_id_pais == gPais
                                   orderby d.dep_nombre
                                   select d;

                return departamento.AsEnumerable().Select(d => new sys_departamento
                {
                    dep_nombre = d.dep_nombre,
                    dep_id = d.dep_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado departamento
        /// </summary>
        /// <param name="gDepartamento"></param>
        /// <returns></returns>
        public List<sys_municipio> ListadoMunicipio(Guid gDepartamento)
        {
            try
            {
                var municipio = from m in _dbContext.sys_municipio.AsNoTracking()
                                where m.mun_id_dpto == gDepartamento
                                orderby m.mun_nombre
                                select m;

                return municipio.AsEnumerable().Select(m => new sys_municipio
                {
                    mun_nombre = m.mun_nombre,
                    mun_id = m.mun_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado tipo emisor
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_emisor> ListadoTipoEmisor()
        {
            try
            {
                var tipoEmisor = from te in _dbContext.sys_tipo_emisor.AsNoTracking()
                                 where te.tem_activo == true
                                 orderby te.tem_descripcion
                                 select te;

                return tipoEmisor.AsEnumerable().Select(te => new sys_tipo_emisor
                {
                    tem_descripcion = te.tem_descripcion,
                    tem_id = te.tem_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado software
        /// </summary>
        /// <returns></returns>
        public List<sys_software> ListadoSoftware()
        {
            try
            {
                var software = from s in _dbContext.sys_software
                               orderby s.sof_nombre
                               select s;

                return software.AsEnumerable().Select(s => new sys_software
                {
                    sof_nombre = s.sof_nombre,
                    sof_id = s.sof_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado ciiu
        /// </summary>
        /// <returns></returns>
        public List<sys_ciiu> ListadoCiiu()
        {
            try
            {
                var ciiu = from c in _dbContext.sys_ciiu.AsNoTracking()
                           orderby c.cii_descripcion
                           select c;

                return ciiu.AsEnumerable().Select(c => new sys_ciiu
                {
                    cii_descripcion = c.cii_descripcion,
                    cii_id = c.cii_id
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado tipo catalogo
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_catalogo> ListadoTipoCatalogo()
        {
            try
            {
                var tipoCatalogo = from tc in _dbContext.sys_tipo_catalogo.AsNoTracking()
                                   orderby tc.tca_nombre
                                   select tc;

                return tipoCatalogo.AsEnumerable().Select(tc => new sys_tipo_catalogo
                {
                    tca_id = tc.tca_id,
                    tca_nombre = tc.tca_nombre.ToLower()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado tipo dato
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_dato> ListadoTipoDato()
        {
            try
            {
                var tipoDato = from td in _dbContext.sys_tipo_dato.AsNoTracking()
                               orderby td.tda_descripcion
                               select td;

                return tipoDato.AsEnumerable().Select(td => new sys_tipo_dato
                {
                    tda_id = td.tda_id,
                    tda_descripcion = td.tda_descripcion.ToLower()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado tipo notificacion
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_notificacion> ListadoTipoNotificacion()
        {
            try
            {
                var tipoNotificacion = from tn in _dbContext.sys_tipo_notificacion.AsNoTracking()
                                       where tn.tno_activo == true
                                       orderby tn.tno_descripcion
                                       select tn;

                return tipoNotificacion.AsEnumerable().Select(tn => new sys_tipo_notificacion
                {
                    tno_id = tn.tno_id,
                    tno_descripcion = tn.tno_descripcion
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado tipo correo
        /// </summary>
        /// <returns></returns>
        public List<sys_tipo_correo> ListadoTipoCorreo()
        {
            try
            {
                var tipoCorreo = from tc in _dbContext.sys_tipo_correo.AsNoTracking()
                                 orderby tc.tco_descripcion
                                 select tc;
                return tipoCorreo.AsEnumerable().Select(tc => new sys_tipo_correo
                {
                    tco_descripcion = tc.tco_descripcion,
                    tco_id = tc.tco_id
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado tipo estado documento
        /// </summary>
        /// <returns></returns>
        public List<sys_estado_documento> ListadoTipoEstadoDocumento()
        {
            try
            {
                return _dbContext.sys_estado_documento.AsNoTracking().ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado tipo estado facse
        /// </summary>
        /// <returns></returns>
        public List<sys_estado_documento_facse> ListadoTipoEstadoFacse()
        {
            try
            {
                return _dbContext.sys_estado_documento_facse.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
