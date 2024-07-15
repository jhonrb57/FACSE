using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Metodos
{
    public class ClsEmisor
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsEmisor).Name);
        private readonly ClsFuncion clsFuncion = new ClsFuncion();

        #region EmisorInicio

        /// <summary>
        /// validar existencia de emisor
        /// </summary>
        /// <param name="gEmisor"></param>
        /// <returns></returns>
        public bool EmisorExistente(Guid? gEmisor)
        {
            try
            {
                return _dbContext.adm_emisor.AsNoTracking().Where(e => e.emi_id == gEmisor).Any();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// informacion de emiosr
        /// </summary>
        /// <param name="gEmisor"></param>
        /// <returns></returns>
        public adm_emisor InformacionEmisor(Guid gEmisor)
        {
            try
            {
                return _dbContext.adm_emisor.AsNoTracking().Where(e => e.emi_id == gEmisor).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// guardar emisor
        /// </summary>
        /// <param name="adm_Emisor"></param>
        public void GuardarEmisor(adm_emisor emisor, string sUser, string sIp)
        {
            try
            {
                if (ValidarIdentificacion(emisor.emi_identificacion))
                {
                    if (!ValidarIdentificacionActivo(emisor.emi_identificacion))
                    {
                        throw new CustomException("La identificación ya existe en facse, y esta inactivo");
                    }
                    throw new CustomException("La identificación ya existe en facse");
                }

                if (emisor.emi_grupo == Guid.Empty)
                {
                    var grupo = new adm_grupo_emisor
                    {
                        gen_id = Guid.NewGuid(),
                        gen_nombre = emisor.emi_nombre,
                        gen_direccion = emisor.emi_direccion,
                        gen_email = emisor.emi_correo,
                        gen_telefono = emisor.emi_telefono,
                        gen_municipio = emisor.emi_municipio ?? Guid.Empty,
                        gen_activo = true
                    };
                    _dbContext.adm_grupo_emisor.Add(grupo);
                    emisor.emi_grupo = grupo.gen_id;
                }

                var emisorSucursal = new adm_emisor_sucursal
                {
                    esu_id = Guid.NewGuid(),
                    esu_emisor = emisor.emi_id,
                    esu_nombre = "PRINCIPAL",
                    esu_abreviatura = "PRIN",
                    esu_departamento = emisor.emi_departamento,
                    esu_municipio = emisor.emi_municipio,
                    esu_correo = emisor.emi_correo,
                    esu_direccion = emisor.emi_direccion,
                    esu_codigo_postal = emisor.emi_codigo_posta,
                    esu_telefono = emisor.emi_telefono,
                    esu_activo = true
                };
                _dbContext.adm_emisor_sucursal.Add(emisorSucursal);

                var usuario = new adm_usuario
                {
                    usu_id = Guid.NewGuid(),
                    usu_usuario = emisor.emi_identificacion,
                    usu_contrasena = clsFuncion.Encriptar(emisor.emi_identificacion),
                    usu_nombre = emisor.emi_nombre,
                    usu_apellido = "",
                    usu_activo = true,
                    usu_directorio_activo = false,
                    usu_email = emisor.emi_correo,
                    usu_direccion = emisor.emi_direccion,
                    usu_telefono = emisor.emi_telefono,
                    usu_fecha_creacion = DateTime.Now,
                    usu_emisor_sucursal = emisorSucursal.esu_id
                };
                _dbContext.adm_usuario.Add(usuario);

                var usuarioSucursal = new adm_usuario_sucursal
                {
                    usu_usuario = usuario.usu_id,
                    usu_sucursal = emisorSucursal.esu_id,
                    usu_activo = true
                };
                _dbContext.adm_usuario_sucursal.Add(usuarioSucursal);

                var perfil = _dbContext.adm_perfil.Where(p => p.per_descripcion.ToUpper().Contains("ADMINISTRADOR"));
                var tipoUsuario = _dbContext.sys_tipo_usuario.Where(u => u.tus_descripcion.ToUpper().Contains("EMISOR"));
                if (perfil.Any() && tipoUsuario.Any())
                {
                    var usuarioTipo = new adm_usuario_perfil_tipo
                    {
                        upt_perfil = perfil.FirstOrDefault().per_id,
                        upt_tipo_usuario = tipoUsuario.FirstOrDefault().tus_id,
                        upt_usuario = usuario.usu_id,
                        upt_persona = emisor.emi_id,
                        upt_activo = true
                    };
                    _dbContext.adm_usuario_perfil_tipo.Add(usuarioTipo);
                }

                emisor.emi_cliente_token = GetSHA1($"{emisor.emi_identificacion}{DateTime.Now.ToString("yyyyMMddHHmmss")}");
                emisor.emi_access_token = GetSHA1($"{emisor.emi_digito}{emisor.emi_correo}{DateTime.Now.ToString("yyyyMMddHHmmss")}");
                emisor.emi_fecha_creacion = DateTime.Now;
                emisor.emi_activo = true;
                _dbContext.adm_emisor.Add(emisor);

                _dbContext.SaveChanges(sUser, sIp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static string GetSHA1(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Validar si ya existe el emisor con el distribuidor
        /// </summary>
        /// <param name="sIdentificacion"></param>
        /// <param name="gDistribuidor"></param>
        /// <returns></returns>
        public bool ValidarIdentificacion(string sIdentificacion)
        {
            try
            {
                var identificacion = from e in _dbContext.adm_emisor.AsNoTracking()
                                     where e.emi_identificacion.Equals(sIdentificacion)
                                     select e;

                return identificacion.Any();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validar si ya existe el emisor con el distribuidor
        /// </summary>
        /// <param name="sIdentificacion"></param>
        /// <param name="gDistribuidor"></param>
        /// <returns></returns>
        public bool ValidarIdentificacionActivo(string sIdentificacion)
        {
            try
            {
                var identificacion = from e in _dbContext.adm_emisor.AsNoTracking()
                                     where e.emi_identificacion.Equals(sIdentificacion)
                                     select e;

                return identificacion.FirstOrDefault().emi_activo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// listado de emisor
        /// </summary>
        /// <param name="gDistribuidor"></param>
        /// <returns></returns>
        public List<DistribuidorInicio> ListadoEmisor(Guid gDistribuidor, int StartRowIndex, int MaximumRows, ref int count, string orderColumn, String sortOrder)
        {

            try
            {
                var emisor = (from e in _dbContext.DistribuidorInicio.AsNoTracking()
                              where e.emi_distribuidor == gDistribuidor
                              select e);

                switch (orderColumn)
                {
                    case "tid_descripcion":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.tid_descripcion);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.tid_descripcion);
                        }
                        break;
                    case "emi_identificacion":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.emi_identificacion);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.emi_identificacion);
                        }
                        break;
                    case "emi_nombre":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.emi_nombre);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.emi_nombre);
                        }
                        break;
                    case "dep_nombre":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.dep_nombre);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.dep_nombre);
                        }
                        break;
                    case "mun_nombre":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.mun_nombre);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.mun_nombre);
                        }
                        break;
                    case "emi_correo":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.emi_correo);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.emi_correo);
                        }
                        break;
                    case "emi_telefono":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.emi_telefono);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.emi_telefono);
                        }
                        break;
                    case "emi_fecha_creacion":
                        if (sortOrder == "asc")
                        {
                            emisor = emisor.OrderBy(e => e.emi_fecha_creacion);
                        }
                        else
                        {
                            emisor = emisor.OrderByDescending(e => e.emi_fecha_creacion);
                        }
                        break;
                    default:
                        emisor = emisor.OrderBy(e => e.emi_nombre);
                        break;
                }

                var emisorFiltrado = emisor.Skip(StartRowIndex).Take(MaximumRows);
                count = emisor.Count();

                return emisorFiltrado.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cargar informacion unica para editar
        /// </summary>
        /// <param name="gEmisor"></param>
        /// <returns></returns>
        public adm_emisor CargarInformacionEditar(Guid gEmisor)
        {
            try
            {
                return _dbContext.adm_emisor.AsNoTracking().Where(e => e.emi_id == gEmisor).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarEmisor(adm_emisor emisor, string sUser, string sIp)
        {
            try
            {

                var emisorExistente = (from e in _dbContext.adm_emisor
                                       where e.emi_id == emisor.emi_id
                                       select e).SingleOrDefault();

                if (emisorExistente != null)
                {
                    if (!(emisorExistente.emi_identificacion.ToUpper().Trim().Equals(emisor.emi_identificacion.ToUpper().Trim()) && emisorExistente.emi_tipo_identificacion.Equals(emisor.emi_tipo_identificacion)))
                    {
                        if (ValidarIdentificacion(emisor.emi_identificacion))
                        {
                            if (emisorExistente.emi_identificacion != emisor.emi_identificacion)
                            {
                                if (!ValidarIdentificacionActivo(emisor.emi_identificacion))
                                {
                                    log.Error("La identificación ya existe en facse, y esta inactivo");
                                    throw new CustomException("La identificación ya existe en facse, y esta inactivo");
                                }
                                log.Error("La identificación ya existe en facse");
                                throw new CustomException("La identificación ya existe en facse");
                            }
                        }
                    }

                    emisorExistente.emi_grupo = emisor.emi_grupo;
                    emisorExistente.emi_tipo_persona = emisor.emi_tipo_persona;
                    emisorExistente.emi_tipo_identificacion = emisor.emi_tipo_identificacion;
                    emisorExistente.emi_identificacion = emisor.emi_identificacion;
                    emisorExistente.emi_digito = emisor.emi_digito;
                    emisorExistente.emi_nombre = emisor.emi_nombre;
                    emisorExistente.emi_razon_social = emisor.emi_razon_social;
                    emisorExistente.emi_pais = emisor.emi_pais;
                    emisorExistente.emi_departamento = emisor.emi_departamento;
                    emisorExistente.emi_municipio = emisor.emi_municipio;
                    emisorExistente.emi_codigo_posta = emisor.emi_codigo_posta;
                    emisorExistente.emi_correo = emisor.emi_correo;
                    emisorExistente.emi_telefono = emisor.emi_telefono;
                    emisorExistente.emi_direccion = emisor.emi_direccion;
                    emisorExistente.emi_tipo_emisor = emisor.emi_tipo_emisor;
                    emisorExistente.emi_logo = emisor.emi_logo;
                    emisorExistente.emi_test_id = emisor.emi_test_id;
                    emisorExistente.emi_sofware = emisor.emi_sofware;
                    emisorExistente.emi_correo_automatico = emisor.emi_correo_automatico;

                    if (emisor.emi_ciiu != null && emisor.emi_ciiu != Guid.Empty)
                    {
                        emisorExistente.emi_ciiu = emisor.emi_ciiu;

                    }
                    else
                    {
                        emisorExistente.emi_ciiu = (Guid?)null;
                    }
                    emisorExistente.emi_matricula_mercantil = emisor.emi_matricula_mercantil;
                    emisorExistente.emi_activo = emisor.emi_activo;
                    //emisorExistente.emi_cliente_token = GetSHA1($"{emisor.emi_identificacion}{DateTime.Now.ToString("yyyyMMddHHmmss")}");
                    //emisorExistente.emi_access_token = GetSHA1($"{emisor.emi_digito}{emisor.emi_correo}{DateTime.Now.ToString("yyyyMMddHHmmss")}");
                }

                _dbContext.SaveChanges(sUser, sIp);

            }
            catch (CustomException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// guardar emisor catalogo
        /// </summary>
        /// <param name="emisor_Catalogo"></param>
        public List<adm_emisor_catalogo> GuardarEmisorCatalogo(adm_emisor_catalogo emisor_Catalogo, string sUser, string sIp)
        {
            try
            {
                emisor_Catalogo.eca_id = Guid.NewGuid();
                _dbContext.adm_emisor_catalogo.Add(emisor_Catalogo);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorCatalogo(emisor_Catalogo.eca_emisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_emisor_catalogo> ListadoEmisorCatalogo(Guid gEmisor)
        {
            try
            {
                var listadoEmisorCatalogo = _dbContext.adm_emisor_catalogo.AsNoTracking().Where(ec => ec.eca_emisor == gEmisor);

                return listadoEmisorCatalogo.AsEnumerable().Select(ec => new adm_emisor_catalogo
                {
                    eca_id = ec.eca_id,
                    eca_emisor = ec.eca_emisor,
                    eca_lista = ec.eca_lista,
                    eca_nombre = ec.eca_nombre,
                    NombreTipoCatalogo = ec.sys_tipo_catalogo.tca_nombre,
                    NombreTipoDato = ec.sys_tipo_dato.tda_descripcion
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor catalogo
        /// </summary>
        /// <param name="gEmisorCatalogo"></param>
        public List<adm_emisor_catalogo> EliminarEmisorCatalogo(Guid gEmisorCatalogo, Guid gEmisor)
        {
            try
            {
                var emisorCatalogo = (from ec in _dbContext.adm_emisor_catalogo
                                      where ec.eca_id == gEmisorCatalogo
                                      select ec).FirstOrDefault();

                _dbContext.adm_emisor_catalogo.Remove(emisorCatalogo);

                _dbContext.SaveChanges();

                return ListadoEmisorCatalogo(gEmisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// guardar emisor catalogo lista
        /// </summary>
        ///<param name="emisor_Catalogo_Lista"></param>
        ///<param name="sIp"></param>
        ///<param name="sUser"></param>
        public List<adm_emisor_catalogo_lista> GuardarEmisorCatalogoLista(adm_emisor_catalogo_lista emisor_Catalogo_Lista, string sUser, string sIp)
        {
            try
            {
                emisor_Catalogo_Lista.ecl_id = Guid.NewGuid();
                _dbContext.adm_emisor_catalogo_lista.Add(emisor_Catalogo_Lista);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorCatalogoLista(emisor_Catalogo_Lista.ecl_emisor_catalogo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_emisor_catalogo_lista> ListadoEmisorCatalogoLista(Guid gIdCatalogo)
        {
            try
            {
                var listadoEmisorCatalogoLista = _dbContext.adm_emisor_catalogo_lista.AsNoTracking().Where(ecl => ecl.ecl_emisor_catalogo == gIdCatalogo);

                return listadoEmisorCatalogoLista.AsEnumerable().Select(ec => new adm_emisor_catalogo_lista
                {
                    ecl_id = ec.ecl_id,
                    ecl_descripcion = ec.ecl_descripcion,
                    ecl_emisor_catalogo = ec.ecl_emisor_catalogo
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor catalogo lista
        /// </summary>
        /// <param name="gIdLista"></param>
        public List<adm_emisor_catalogo_lista> EliminarEmisorCatalogoLista(Guid gIdLista, Guid gCatalogo)
        {
            try
            {
                var emisorCatalogo = (from ec in _dbContext.adm_emisor_catalogo_lista
                                      where ec.ecl_id == gIdLista
                                      select ec).FirstOrDefault();

                _dbContext.adm_emisor_catalogo_lista.Remove(emisorCatalogo);

                _dbContext.SaveChanges();

                return ListadoEmisorCatalogoLista(gCatalogo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// guardar emisor certificado
        /// </summary>
        /// <param name="emisor_Certificado"></param>
        public List<adm_emisor_certificado> GuardarEmisorCertificado(adm_emisor_certificado emisor_Certificado, string sUser, string sIp)
        {
            try
            {
                emisor_Certificado.ece_id = Guid.NewGuid();
                _dbContext.adm_emisor_certificado.Add(emisor_Certificado);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorCertificado(emisor_Certificado.ece_emisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_emisor_certificado> ListadoEmisorCertificado(Guid gEmisor)
        {
            try
            {
                var listadoEmisorCertificado = _dbContext.adm_emisor_certificado.AsNoTracking().Where(ec => ec.ece_emisor == gEmisor);

                return listadoEmisorCertificado.AsEnumerable().Select(ec => new adm_emisor_certificado
                {
                    ece_id = ec.ece_id,
                    ece_emisor = ec.ece_emisor,
                    ece_archivo = ec.ece_archivo,
                    ece_certificado = ec.ece_certificado,
                    ece_fecha_vegencia = ec.ece_fecha_vegencia,
                    ece_activo = ec.ece_activo,
                    ece_contrasena = ec.ece_contrasena
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor certificado
        /// </summary>
        /// <param name="gEmisorCertificado"></param>
        public List<adm_emisor_certificado> EliminarEmisorCertificado(Guid gEmisorCertificado, Guid gEmisor)
        {
            try
            {
                var emisorCertificado = (from ec in _dbContext.adm_emisor_certificado
                                         where ec.ece_id == gEmisorCertificado
                                         select ec).FirstOrDefault();

                _dbContext.adm_emisor_certificado.Remove(emisorCertificado);

                _dbContext.SaveChanges();

                return ListadoEmisorCertificado(gEmisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// guardar emisor notificacion
        /// </summary>
        /// <param name="emisor_Notificacion"></param>
        public List<adm_emisor_notificacion> GuardarEmisorNotificacion(adm_emisor_notificacion emisor_Notificacion, string sUser, string sIp)
        {
            try
            {
                emisor_Notificacion.eno_id = Guid.NewGuid();
                _dbContext.adm_emisor_notificacion.Add(emisor_Notificacion);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorNotificacion(emisor_Notificacion.eno_id_emisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_emisor_notificacion> ListadoEmisorNotificacion(Guid gEmisor)
        {
            try
            {
                var listadoEmisorNotificacion = _dbContext.adm_emisor_notificacion.AsNoTracking().Where(ec => ec.eno_id_emisor == gEmisor);

                return listadoEmisorNotificacion.AsEnumerable().Select(ec => new adm_emisor_notificacion
                {
                    eno_id = ec.eno_id,
                    eno_id_emisor = ec.eno_id_emisor,
                    NombreTipoNotificacion = ec.sys_tipo_notificacion.tno_descripcion,
                    eno_descripcion = ec.eno_descripcion,
                    eno_fecha = ec.eno_fecha,
                    eno_leido = ec.eno_leido
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor certificado
        /// </summary>
        /// <param name="gEmisorNotificacion"></param>
        public List<adm_emisor_notificacion> EliminarEmisorNotificacion(Guid gEmisorNotificacion, Guid gEmisor)
        {
            try
            {
                var emisorNotificacion = (from ec in _dbContext.adm_emisor_notificacion
                                          where ec.eno_id == gEmisorNotificacion
                                          select ec).FirstOrDefault();

                _dbContext.adm_emisor_notificacion.Remove(emisorNotificacion);

                _dbContext.SaveChanges();

                return ListadoEmisorNotificacion(gEmisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// guardar emisor correo
        /// </summary>
        /// <param name="emisor_Correo"></param>
        public List<adm_emisor_correo> GuardarEmisorCorreo(adm_emisor_correo emisor_Correo, string sUser, string sIp)
        {
            try
            {
                emisor_Correo.eco_id = Guid.NewGuid();
                emisor_Correo.eco_correo_html = "No aplica";
                _dbContext.adm_emisor_correo.Add(emisor_Correo);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorCorreo(emisor_Correo.eco_emisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_emisor_correo> ListadoEmisorCorreo(Guid gEmisor)
        {
            try
            {
                var listadoEmisorCorreo = _dbContext.adm_emisor_correo.AsNoTracking().Where(ec => ec.eco_emisor == gEmisor);

                return listadoEmisorCorreo.AsEnumerable().Select(ec => new adm_emisor_correo
                {
                    eco_id = ec.eco_id,
                    eco_emisor = ec.eco_emisor,
                    NombreTipoCorreo = ec.sys_tipo_correo.tco_descripcion,
                    eco_nombre = ec.eco_nombre,
                    eco_servidor = ec.eco_servidor,
                    eco_puerto = ec.eco_puerto,
                    eco_usuario = ec.eco_usuario,
                    eco_contrasena = ec.eco_contrasena,
                    eco_correo = ec.eco_correo,
                    eco_ssl = ec.eco_ssl,
                    eco_correo_html = ec.eco_correo_html,
                    eco_activo = ec.eco_activo
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor correo
        /// </summary>
        /// <param name="gEmisorCorreo"></param>
        public List<adm_emisor_correo> EliminarEmisorCorreo(Guid gEmisorCorreo, Guid gEmisor)
        {
            try
            {
                var emisorCorreo = (from ec in _dbContext.adm_emisor_correo
                                    where ec.eco_id == gEmisorCorreo
                                    select ec).FirstOrDefault();

                _dbContext.adm_emisor_correo.Remove(emisorCorreo);

                _dbContext.SaveChanges();

                return ListadoEmisorCorreo(gEmisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// guardar emisor resolucion
        /// </summary>
        /// <param name="emisor_Resolucion"></param>
        public List<adm_emisor_resolucion> GuardarEmisorResolucion(adm_emisor_resolucion emisor_Resolucion, string sUser, string sIp)
        {
            try
            {
                emisor_Resolucion.ere_id = Guid.NewGuid();
                _dbContext.adm_emisor_resolucion.Add(emisor_Resolucion);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorResolucion(emisor_Resolucion.ere_emisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<adm_emisor_resolucion> ListadoEmisorResolucion(Guid gEmisor)
        {
            try
            {
                var listadoEmisorResolucion = _dbContext.adm_emisor_resolucion.AsNoTracking().Where(ec => ec.ere_emisor == gEmisor);

                return listadoEmisorResolucion.AsEnumerable().Select(ec => new adm_emisor_resolucion
                {
                    ere_id = ec.ere_id,
                    ere_emisor = ec.ere_emisor,
                    NombreTipoDocumento = ec.sys_tipo_documento.tdo_descripcion,
                    ere_prefijo = ec.ere_prefijo,
                    ere_numero_resolucion = ec.ere_numero_resolucion,
                    ere_fecha = ec.ere_fecha,
                    ere_numero_inicial = ec.ere_numero_inicial,
                    ere_numero_final = ec.ere_numero_final,
                    ere_clave_tecnica = ec.ere_clave_tecnica,
                    ere_fecha_inicio = ec.ere_fecha_inicio,
                    ere_fecha_final = ec.ere_fecha_final,
                    ere_activo = ec.ere_activo
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor resolucion
        /// </summary>
        /// <param name="gEmisorResolucion"></param>
        public List<adm_emisor_resolucion> EliminarEmisorResolucion(Guid gEmisorResolucion, Guid gEmisor)
        {
            try
            {
                var emisorResolucion = (from ec in _dbContext.adm_emisor_resolucion
                                        where ec.ere_id == gEmisorResolucion
                                        select ec).FirstOrDefault();

                _dbContext.adm_emisor_resolucion.Remove(emisorResolucion);

                _dbContext.SaveChanges();

                return ListadoEmisorResolucion(gEmisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        ///// <summary>
        ///// prefijo resolucion
        ///// </summary>
        ///// <param name="iEmisor"></param>
        ///// <returns></returns>
        //public string PrefijoResolucion(int iEmisor)
        //{
        //    try
        //    {
        //        var prefijo = _dbContext.AdmSoftwareResolucion.Where(sr => sr.SreEmisor == iEmisor);

        //        if (prefijo.Any())
        //        {
        //            return prefijo.FirstOrDefault().SrePrefijo;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //} 
        #endregion

        public List<adm_emisor_sucursal> ListadoEmisorSucursal(Guid gEmisor)
        {
            try
            {
                var listadoEmisorSucursal = _dbContext.adm_emisor_sucursal.AsNoTracking().Where(ec => ec.esu_emisor == gEmisor);

                return listadoEmisorSucursal.AsEnumerable().Select(ec => new adm_emisor_sucursal
                {
                    esu_id = ec.esu_id,
                    esu_emisor = ec.esu_emisor,
                    esu_nombre = ec.esu_nombre,
                    esu_abreviatura = ec.esu_abreviatura,
                    esu_correo = ec.esu_correo,
                    esu_direccion = ec.esu_direccion,
                    esu_codigo_postal = ec.esu_codigo_postal,
                    esu_telefono = ec.esu_telefono,
                    esu_correo_entrada = ec.esu_correo_entrada,
                    esu_correo_salida = ec.esu_correo_salida,
                    esu_activo = ec.esu_activo,
                    NombreDepartamento = ec.sys_departamento == null ? "" : ec.sys_departamento.dep_nombre, // ec.sys_departamento.dep_nombre,
                    NombreMunicipio = ec.sys_municipio == null ? "" : ec.sys_municipio.mun_nombre
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// eliminar emisor sucursal
        /// </summary>
        /// <param name="gEmisorSucursal"></param>
        public List<adm_emisor_sucursal> EliminarEmisorSucursal(Guid gEmisorSucursal, Guid gEmisor)
        {
            try
            {
                var emisorSucursal = (from ec in _dbContext.adm_emisor_sucursal
                                      where ec.esu_id == gEmisorSucursal
                                      select ec).FirstOrDefault();

                _dbContext.adm_emisor_sucursal.Remove(emisorSucursal);

                _dbContext.SaveChanges();

                return ListadoEmisorSucursal(gEmisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// guardar emisor sucursal
        /// </summary>
        /// <param name="emisor_Sucursal"></param>
        public List<adm_emisor_sucursal> GuardarEmisorSucursal(adm_emisor_sucursal emisor_Sucursal, string sUser, string sIp)
        {
            try
            {
                emisor_Sucursal.esu_id = Guid.NewGuid();
                _dbContext.adm_emisor_sucursal.Add(emisor_Sucursal);
                _dbContext.SaveChanges(sUser, sIp);

                return ListadoEmisorSucursal(emisor_Sucursal.esu_emisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region EmisorConfiguracion

        public List<adm_emisor> ListadoEmisores(Guid emisor)
        {
            try
            {
                var grupo = _dbContext.adm_emisor
                    .Where(x => x.emi_id == emisor)
                    .Select(x => x.emi_grupo).FirstOrDefault();


                var query = _dbContext.adm_emisor
                    .Where(x => x.emi_grupo == grupo)
                    .AsEnumerable().Select(e => new adm_emisor
                    {
                        emi_id = e.emi_id,
                        NombreTipoIdentificacion = e.sys_tipo_identificacion.tid_descripcion.ToLower(),
                        emi_identificacion = e.emi_identificacion,
                        emi_nombre = e.emi_nombre.ToLower(),
                        NombreDepartamento = e.sys_departamento == null ? "" : e.sys_departamento.dep_nombre.ToLower(),
                        NombreMunicipio = e.sys_municipio == null ? "" : e.sys_municipio.mun_nombre.ToLower(),
                        emi_correo = e.emi_correo,
                        emi_telefono = e.emi_telefono,
                        emi_fecha_creacion = e.emi_fecha_creacion,
                        emi_activo = e.emi_activo
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string NombreSucusarEmisor(Guid gEmisorSucusal)
        {
            try
            {
                var emisorSucusarl = _dbContext.adm_emisor_sucursal.Find(gEmisorSucusal);

                return $"{emisorSucusarl.adm_emisor.emi_identificacion}{emisorSucusarl.esu_abreviatura}";

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        #endregion
    }
}
