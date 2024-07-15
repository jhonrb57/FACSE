using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metodos
{
    public class ClsBuscar
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsAdquiriente).Name);


        public List<AdquirienteReceptor> BuscarReceptor(string buscar, Guid idEmisor)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => (x.rec_razon_social.Contains(buscar) || x.rec_identificacion.Contains(buscar) || x.rec_nombre.Contains(buscar)) && x.rec_emisor == idEmisor)
                    .Select(r => new AdquirienteReceptor
                    {
                        IdReceptor = r.rec_id,
                        RazonSocial = r.rec_razon_social,
                        NumeroIdentificacion = r.rec_identificacion
                    }).Take(100).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdquirienteReceptor CargarDatosReceptor(Guid id)
        {
            try
            {
                var query = _dbContext.adm_receptor
                    .Where(x => x.rec_id == id)
                    .Select(r => new AdquirienteReceptor
                    {
                        IdReceptor = r.rec_id,
                        RazonSocial = r.rec_razon_social,
                        NumeroIdentificacion = r.rec_identificacion,
                        CodigoPostal = r.rec_codigo_postal,
                        IdDepartamento = r.sys_departamento.dep_id,
                        CodigoDepartamento = r.sys_departamento.dep_codigo.Trim(),
                        CodigoMunicipio = r.sys_municipio.mun_codigo.Trim(),
                        CodigoPais = r.sys_pais.pai_codigo_2.Trim(),
                        Digito = r.rec_digito,
                        Direccion = r.rec_direccion,
                        Email = r.rec_correo,
                        IdMunicipio = r.sys_municipio.mun_id,
                        IdPais = r.sys_pais.pai_id,
                        Nombre = r.rec_nombre.Trim(),
                        Telefono = r.rec_telefono,
                        TextDepartamento = r.sys_departamento.dep_nombre.Trim(),
                        TextMunicipio = r.sys_municipio.mun_nombre.Trim(),
                        TextPais = r.sys_pais.pai_nombre_comun.Trim(),
                        TipoAdquiriente = r.rec_tipo_receptor,
                        TipoIdentificacion = r.sys_tipo_identificacion.tid_id,
                        TipoPersona = r.sys_tipo_persona.tpe_id,
                        CodigoTipoIdentificacion = r.sys_tipo_identificacion.tid_codigo.Trim(),
                        CodigoTipoPersona = r.sys_tipo_persona.tpe_codigo.Trim()


                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<DatosReceptor> CargarListadoReceptoresBusqueda(Guid emisor, string buscar)
        {
            try
            {
                Guid tipoDoc = Guid.Parse("E92E2E55-C00D-430A-89FC-8864900AE1B8");

                var receptor = _dbContext.adm_receptor.AsNoTracking()
                    .Where(r => r.rec_emisor == emisor);

                if (!(string.IsNullOrWhiteSpace(buscar)))
                {
                    receptor = receptor.Where(x => x.rec_identificacion.Contains(buscar.Trim()) || x.rec_nombre.Contains(buscar.Trim()) || x.rec_razon_social.Contains(buscar.Trim()));
                }

                return receptor.Select(r => new DatosReceptor
                {
                    Id = r.rec_id,
                    Identificacion = r.rec_identificacion,
                    Digito = r.rec_digito,
                    Nombre = r.rec_nombre.ToUpper(),
                    RazonSocial = r.rec_razon_social.ToUpper(),
                    Email = r.rec_correo.ToLower(),
                    Telefono = r.rec_telefono,
                    CantidadFacturas = r.adm_documento.Where(x => x.doc_receptor == r.rec_id && x.doc_emisor == emisor && x.doc_tipo_documento == tipoDoc).Count()

                }).Take(100).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region TemporalFactura
        public Guid CargarGuidReceptor(string Nit)
        {
            try
            {
                var IdReceptor = _dbContext.adm_receptor
                    .Where(x => x.rec_identificacion.StartsWith(Nit))
                    .Select(u => u.rec_id).FirstOrDefault();
                return IdReceptor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
