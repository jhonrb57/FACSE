using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Metodos
{
    public class ClsConfiguracion
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsAdquiriente).Name);
        public static readonly ClsFuncion _clsFuncion = new ClsFuncion();

        #region Usuario
        public List<DatosUsuario> CargarListadoUsuarios(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_usuario
                    .Where(x => x.adm_emisor_sucursal.esu_emisor == emisor)
                    .Select(u => new DatosUsuario
                    {
                        Activo = u.usu_activo,
                        Apellido = u.usu_apellido,
                        Contrasena = u.usu_contrasena,
                        Direccion = u.usu_direccion,
                        DirectorioActivo = u.usu_directorio_activo,
                        Email = u.usu_email,
                        FechaCreacion = u.usu_fecha_creacion,
                        Id = u.usu_id,
                        Nombre = u.usu_nombre,
                        sUsuario = u.usu_usuario,
                        Telefono = u.usu_telefono
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoTipoUsuario()
        {
            try
            {
                var query = _dbContext.sys_tipo_usuario
                    .Select(x => new SelectListItem
                    {
                        Text = x.tus_descripcion,
                        Value = x.tus_id.ToString()
                    }).OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> CargarListadoPerfiles()
        {
            try
            {
                var query = _dbContext.adm_perfil
                    .Select(x => new SelectListItem
                    {
                        Text = x.per_descripcion,
                        Value = x.per_id.ToString()
                    }).OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DescripcionPerfil(Guid perfil)
        {
            try
            {
                var query = _dbContext.adm_perfil
                    .Where(x => x.per_id == perfil)
                    .Select(x => x.per_descripcion).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoSucursales(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal
                    .Where(x => x.esu_emisor == emisor)
                    .Select(x => new SelectListItem
                    {
                        Text = x.esu_nombre,
                        Value = x.esu_id.ToString()
                    }).OrderBy(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatosUsuario CargarDatosUsuario(Guid idUsuario)
        {
            try
            {
                var query = _dbContext.adm_usuario
                    .Where(u => u.usu_id == idUsuario)
                    .Select(x => new DatosUsuario
                    {
                        Activo = x.usu_activo,
                        Apellido = x.usu_apellido,
                        Contrasena = x.usu_contrasena,
                        Direccion = x.usu_direccion,
                        DirectorioActivo = x.usu_directorio_activo,
                        Email = x.usu_email,
                        FechaCreacion = x.usu_fecha_creacion,
                        Id = x.usu_id,
                        Nombre = x.usu_nombre,
                        Perfil = x.adm_usuario_perfil_tipo.Select(a => a.upt_perfil).FirstOrDefault(),
                        Sucursales = x.adm_usuario_sucursal.Select(a => a.usu_sucursal).ToList(),
                        sUsuario = x.usu_usuario,
                        Telefono = x.usu_telefono,
                        Tipo = x.adm_usuario_perfil_tipo.Select(a => a.upt_tipo_usuario).FirstOrDefault()
                    })
                    .FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevoUsuario(DatosUsuario datosUsuario, Guid emisor)
        {
            try
            {
                var newId = Guid.NewGuid();
                var pass = _clsFuncion.Encriptar(datosUsuario.Contrasena);
                var newusuario = new adm_usuario
                {
                    usu_id = newId,
                    usu_activo = true,
                    usu_apellido = datosUsuario.Apellido,
                    usu_contrasena = pass,
                    usu_direccion = datosUsuario.Direccion,
                    usu_directorio_activo = false,
                    usu_email = datosUsuario.Email,
                    usu_emisor_sucursal = datosUsuario.Sucursales.FirstOrDefault(),
                    usu_fecha_creacion = DateTime.Now,
                    usu_nombre = datosUsuario.Nombre,
                    usu_telefono = datosUsuario.Telefono,
                    usu_usuario = datosUsuario.sUsuario
                };

                var newtipousuario = new adm_usuario_perfil_tipo
                {
                    upt_activo = true,
                    upt_perfil = datosUsuario.Perfil,
                    upt_persona = emisor,
                    upt_tipo_usuario = datosUsuario.Tipo,
                    upt_usuario = newId
                };

                _dbContext.adm_usuario.Add(newusuario);
                _dbContext.adm_usuario_perfil_tipo.Add(newtipousuario);

                foreach (var item in datosUsuario.Sucursales)
                {
                    var uSucursal = new adm_usuario_sucursal
                    {
                        usu_activo = true,
                        usu_sucursal = item,
                        usu_usuario = newId
                    };

                    _dbContext.adm_usuario_sucursal.Add(uSucursal);
                }

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarUsuario(DatosUsuario datosUsuario, Guid emisor)
        {
            try
            {
                var usuario = _dbContext.adm_usuario
                    .Where(x => x.usu_id == datosUsuario.Id)
                    .FirstOrDefault();

                usuario.usu_activo = datosUsuario.Activo;
                usuario.usu_apellido = datosUsuario.Apellido;
                usuario.usu_direccion = datosUsuario.Direccion;
                usuario.usu_email = datosUsuario.Email;
                usuario.usu_nombre = datosUsuario.Nombre;
                usuario.usu_telefono = datosUsuario.Telefono;

                var usuarioTipo = _dbContext.adm_usuario_perfil_tipo
                    .Where(x => x.upt_usuario == datosUsuario.Id)
                    .FirstOrDefault();

                _dbContext.adm_usuario_perfil_tipo.Remove(usuarioTipo);

                var newUsuarioTipo = new adm_usuario_perfil_tipo
                {
                    upt_activo = true,
                    upt_perfil = datosUsuario.Perfil,
                    upt_persona = emisor,
                    upt_usuario = datosUsuario.Id,
                    upt_tipo_usuario = datosUsuario.Tipo
                };

                _dbContext.adm_usuario_perfil_tipo.Add(newUsuarioTipo);

                var sucursales = _dbContext.adm_usuario_sucursal
                    .Where(x => x.usu_usuario == datosUsuario.Id)
                    .ToList();

                if (datosUsuario.Sucursales != null && datosUsuario.Sucursales.Any())
                {

                    _dbContext.adm_usuario_sucursal.RemoveRange(sucursales);

                    foreach (var item in datosUsuario.Sucursales)
                    {
                        var uSucursal = new adm_usuario_sucursal
                        {
                            usu_activo = true,
                            usu_sucursal = item,
                            usu_usuario = datosUsuario.Id
                        };

                        _dbContext.adm_usuario_sucursal.Add(uSucursal);
                    }
                }

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public int CalcularCantidadUsuarios(Guid emisor)
        {
            try
            {
                var cantidad = _dbContext.adm_usuario_perfil_tipo
                 .Where(x => x.upt_persona == emisor)
                 .Count();

                return cantidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public (List<DatosUsuarioSucursal> asignadas, List<DatosUsuarioSucursal> disponibles) SucursalesUsuarioAsignadasDisponibles(Guid usuario, Guid emisor)
        {
            try
            {
                var asignadas = _dbContext.adm_usuario_sucursal
                    .Where(x => x.usu_usuario == usuario)
                    .Select(x => new DatosUsuarioSucursal
                    {
                        IdSucursal = x.usu_sucursal,
                        Abreviatura = x.adm_emisor_sucursal.esu_abreviatura,
                        NombreSuc = x.adm_emisor_sucursal.esu_nombre
                    }).ToList();

                var disponibles = _dbContext.adm_emisor_sucursal
                    .Where(x => x.esu_emisor == emisor && x.esu_activo == true)
                    .Select(x => new DatosUsuarioSucursal
                    {
                        IdSucursal = x.esu_id,
                        Abreviatura = x.esu_abreviatura,
                        NombreSuc = x.esu_nombre
                    }).ToList()
                    .Where(x => !asignadas.Select(s => s.IdSucursal).ToList().Contains(x.IdSucursal)).ToList();

                return (asignadas, disponibles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AsignarSucursalUsuario(Guid idUsuario, Guid idSucursal)
        {
            try
            {
                var newSucUsu = new adm_usuario_sucursal
                {
                    usu_activo = true,
                    usu_sucursal = idSucursal,
                    usu_usuario = idUsuario
                };

                _dbContext.adm_usuario_sucursal.Add(newSucUsu);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool EliminarSucursalUsuario(Guid idUsuario, Guid idSucursal)
        {
            try
            {
                var query = _dbContext.adm_usuario_sucursal
                    .Where(x => x.usu_usuario == idUsuario && x.usu_sucursal == idSucursal)
                    .FirstOrDefault();

                _dbContext.adm_usuario_sucursal.Remove(query);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public List<SelectListItem> CargarListadoCorreo(Guid emisor, Guid tipo)
        {
            try
            {
                var query = _dbContext.adm_emisor_correo
                    .Where(x => x.eco_emisor == emisor && x.eco_activo == true && x.eco_tipo_correo == tipo)
                    .Select(x => new SelectListItem
                    {
                        Value = x.eco_id.ToString(),
                        Text = x.eco_correo
                    }).ToList();

                return query.OrderBy(x => x.Value).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sucursal
        public List<DatosSucursal> ListadoSucursales(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal
                    .Where(x => x.esu_emisor == emisor)
                    .Select(s => new DatosSucursal
                    {
                        Abreviatura = s.esu_abreviatura,
                        CodigoPostal = s.esu_codigo_postal,
                        Direccion = s.esu_direccion,
                        Email = s.esu_correo,
                        Estado = s.esu_activo,
                        Id = s.esu_id,
                        Nombre = s.esu_nombre,
                        Telefono = s.esu_telefono,
                        HasResolution = s.adm_emisor_sucursal_resolucion.Any()
                    }).ToList();

                return query;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DatosSucursal CargarDatosSucursal(Guid idSucursal)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal
                    .Where(x => x.esu_id == idSucursal)
                    .Select(x => new DatosSucursal
                    {
                        Abreviatura = x.esu_abreviatura,
                        CodigoPostal = x.esu_codigo_postal,
                        Email = x.esu_correo,
                        Direccion = x.esu_direccion,
                        Estado = x.esu_activo,
                        Id = x.esu_id,
                        Nombre = x.esu_nombre,
                        Telefono = x.esu_telefono,
                        Departamento = x.esu_departamento,
                        Municipio = x.esu_municipio,
                        Pais = x.sys_departamento.dep_id_pais.Value,
                        CorreoEntrada = x.esu_correo_entrada ?? Guid.Empty,
                        CorreoSalida = x.esu_correo_salida ?? Guid.Empty
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevaSucursal(DatosSucursal sucursal, Guid emisor)
        {
            try
            {
                var newSucursal = new adm_emisor_sucursal
                {
                    esu_id = Guid.NewGuid(),
                    esu_abreviatura = sucursal.Abreviatura,
                    esu_activo = true,
                    esu_codigo_postal = sucursal.CodigoPostal,
                    esu_correo = sucursal.Email,
                    esu_departamento = sucursal.Departamento,
                    esu_direccion = sucursal.Direccion,
                    esu_emisor = emisor,
                    esu_municipio = sucursal.Municipio,
                    esu_nombre = sucursal.Nombre,
                    esu_telefono = sucursal.Telefono,
                    esu_correo_entrada = sucursal.CorreoEntrada,
                    esu_correo_salida = sucursal.CorreoSalida
                };

                if (newSucursal.esu_departamento == Guid.Empty)
                {
                    newSucursal.esu_departamento = null;
                }

                if (newSucursal.esu_municipio == Guid.Empty)
                {
                    newSucursal.esu_municipio = null;
                }
                _dbContext.adm_emisor_sucursal.Add(newSucursal);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarSucursal(DatosSucursal sucursal)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal
                    .Where(x => x.esu_id == sucursal.Id).FirstOrDefault();

                query.esu_abreviatura = sucursal.Abreviatura;
                query.esu_activo = sucursal.Estado;
                query.esu_codigo_postal = sucursal.CodigoPostal;
                query.esu_correo = sucursal.Email;
                query.esu_departamento = sucursal.Departamento;
                query.esu_direccion = sucursal.Direccion;
                query.esu_municipio = sucursal.Municipio;
                query.esu_nombre = sucursal.Nombre;
                query.esu_telefono = sucursal.Telefono;
                query.esu_correo_entrada = sucursal.CorreoEntrada;
                query.esu_correo_salida = sucursal.CorreoSalida;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public (List<DatosSucursalResolucion> asig, List<DatosSucursalResolucion> dis) ResolucionesAsignadasDisponibles(Guid idSucursal, Guid idEmisor)
        {
            try
            {
                var asig = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_sucursal == idSucursal)
                    .Select(x => new DatosSucursalResolucion
                    {
                        Id = x.adm_emisor_resolucion.ere_id,
                        NumeroFinal = x.adm_emisor_resolucion.ere_numero_final,
                        NumeroInicial = x.adm_emisor_resolucion.ere_numero_inicial,
                        NumeroResolucion = x.adm_emisor_resolucion.ere_numero_resolucion,
                        Prefijo = x.adm_emisor_resolucion.ere_prefijo,
                        TipoDocumento = x.adm_emisor_resolucion.sys_tipo_documento.tdo_descripcion,
                        IdResSuc = x.esr_id
                    }).ToList();

                var disponibles = _dbContext.adm_emisor_resolucion
                   .Where(x => x.ere_emisor == idEmisor && x.ere_activo)
                   .Select(x => new DatosSucursalResolucion
                   {
                       Id = x.ere_id,
                       NumeroFinal = x.ere_numero_final,
                       NumeroInicial = x.ere_numero_inicial,
                       NumeroResolucion = x.ere_numero_resolucion,
                       Prefijo = x.ere_prefijo,
                       TipoDocumento = x.sys_tipo_documento.tdo_descripcion
                   })
                   .ToList()
                   .Where(x => !asig.Select(a => a.Id).ToList().Contains(x.Id)).ToList();

                return (asig, disponibles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool ValidarResolucion(Guid idSucursal, Guid idEmisor, Guid resolucion)
        {
            try
            {
                var tipo = _dbContext.adm_emisor_resolucion
                    .Where(x => x.ere_emisor == idEmisor && x.ere_id == resolucion)
                    .Select(x => x.ere_tipo_documento).FirstOrDefault();


                var query = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_sucursal == idSucursal && x.adm_emisor_resolucion.ere_tipo_documento == tipo)
                    .Any();

                return !query;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AsignarResolucionSucursal(Guid emisor, Guid sucursal, Guid resolucion)
        {
            try
            {
                var res = new adm_emisor_sucursal_resolucion
                {
                    esr_activo = true,
                    esr_emisor = emisor,
                    esr_id = Guid.NewGuid(),
                    esr_resolucion = resolucion,
                    esr_sucursal = sucursal
                };

                _dbContext.adm_emisor_sucursal_resolucion.Add(res);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool EliminarResolucionSucursal(Guid id, Guid emisor, Guid sucursal, Guid resolucion)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal_resolucion
                    .Where(x => x.esr_id == id && x.esr_emisor == emisor && x.esr_sucursal == sucursal && x.esr_resolucion == resolucion)
                    .FirstOrDefault();

                _dbContext.adm_emisor_sucursal_resolucion.Remove(query);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Resolucion

        public List<DatosResolucion> CargarListadoResolucion(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_resolucion.
                    Where(x => x.ere_emisor == emisor)
                    .Select(r => new DatosResolucion
                    {
                        ClaveTecnica = r.ere_clave_tecnica,
                        Estado = r.ere_activo,
                        Fecha = r.ere_fecha,
                        FechaFin = r.ere_fecha_final,
                        FechaInicio = r.ere_fecha_inicio,
                        Id = r.ere_id,
                        NumeroFinal = r.ere_numero_final,
                        NumeroInicial = r.ere_numero_inicial,
                        NumeroResolucion = r.ere_numero_resolucion,
                        Prefijo = r.ere_prefijo,
                        TipoDocumento = r.ere_tipo_documento
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatosResolucion ConsultarDatosResolucion(Guid idResolucion)
        {
            try
            {
                var query = _dbContext.adm_emisor_resolucion
                    .Where(x => x.ere_id == idResolucion)
                    .Select(x => new DatosResolucion
                    {
                        ClaveTecnica = x.ere_clave_tecnica,
                        Estado = x.ere_activo,
                        Fecha = x.ere_fecha,
                        FechaFin = x.ere_fecha_final,
                        FechaInicio = x.ere_fecha_inicio,
                        Id = x.ere_id,
                        NumeroFinal = x.ere_numero_final,
                        NumeroInicial = x.ere_numero_inicial,
                        NumeroResolucion = x.ere_numero_resolucion,
                        Prefijo = x.ere_prefijo,
                        TipoDocumento = x.ere_tipo_documento,
                        Plantilla = x.ere_plantilla
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoTipoDocumento()
        {
            try
            {
                var query = _dbContext.sys_tipo_documento
                    .Select(x => new SelectListItem
                    {
                        Text = x.tdo_descripcion,
                        Value = x.tdo_id.ToString(),

                    }).OrderBy(c => c.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoPlantilla(Guid? gDocumento)
        {
            try
            {
                var query = (from q in _dbContext.adm_plantilla.AsNoTracking() select q);

                if (gDocumento != null)
                {
                    query = query.Where(x => x.prg_tipo_documento == gDocumento);
                }

                var queryFinal = query.Select(x => new SelectListItem
                {
                    Text = x.prg_nombre,
                    Value = x.prg_id.ToString()
                }).OrderBy(c => c.Text).ToList();

                var itemNull = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Seleccione plantilla",
                        Value = null
                    }
                };

                List<SelectListItem> listaFinal = new List<SelectListItem>();

                foreach (var i in itemNull)
                {
                    listaFinal.Add(i);
                }

                foreach (var i in queryFinal)
                {
                    listaFinal.Add(i);
                }

                return listaFinal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevaResolucion(DatosResolucion resolucion, Guid emisor)
        {
            try
            {
                var newResolucion = new adm_emisor_resolucion
                {
                    ere_id = Guid.NewGuid(),
                    ere_activo = true,
                    ere_clave_tecnica = resolucion.ClaveTecnica,
                    ere_emisor = emisor,
                    ere_fecha = DateTime.Now,
                    ere_fecha_final = resolucion.FechaFin,
                    ere_fecha_inicio = resolucion.FechaInicio,
                    ere_numero_final = resolucion.NumeroFinal.ToString(),
                    ere_numero_inicial = resolucion.NumeroInicial.ToString(),
                    ere_numero_resolucion = resolucion.NumeroResolucion,
                    ere_prefijo = resolucion.Prefijo,
                    ere_tipo_documento = resolucion.TipoDocumento
                };

                _dbContext.adm_emisor_resolucion.Add(newResolucion);
                _dbContext.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarDatosResolucion(DatosResolucion resolucion)
        {
            try
            {
                var query = _dbContext.adm_emisor_resolucion
                    .Where(x => x.ere_id == resolucion.Id)
                    .FirstOrDefault();

                query.ere_activo = resolucion.Estado;
                query.ere_clave_tecnica = resolucion.ClaveTecnica;
                query.ere_fecha_final = resolucion.FechaFin;
                query.ere_fecha_inicio = resolucion.FechaInicio;
                query.ere_numero_final = resolucion.NumeroFinal.ToString();
                query.ere_numero_inicial = resolucion.NumeroInicial.ToString();
                query.ere_numero_resolucion = resolucion.NumeroResolucion;
                query.ere_prefijo = resolucion.Prefijo;
                query.ere_tipo_documento = resolucion.TipoDocumento;
                query.ere_plantilla = resolucion.Plantilla;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public string RutaPlantilla()
        {
            try
            {
                var query = _dbContext.adm_emisor_resolucion
                    .Join(_dbContext.adm_plantilla,
                            aer => aer.ere_plantilla,
                            ap => ap.prg_id,
                            (aer, ap) => new { aer, ap }
                        )
                    .Select(x => x.ap.prg_direccion_pdf).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Correo

        public List<DatosCorreo> CargarlistadoCorreo(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_correo
                    .Where(x => x.eco_emisor == emisor)
                    .Select(x => new DatosCorreo
                    {
                        Contrasena = x.eco_contrasena,
                        Correo = x.eco_correo,
                        CorreoHtml = x.eco_correo_html,
                        Estado = x.eco_activo,
                        Nombre = x.eco_nombre,
                        Puerto = x.eco_puerto,
                        Servidor = x.eco_servidor,
                        Ssl = x.eco_ssl,
                        TipoCorreo = x.eco_tipo_correo ?? Guid.Empty,
                        Usuario = x.eco_usuario,
                        IdCorreo = x.eco_id,
                        TipoCorreoText = _dbContext.sys_tipo_correo.Where(a => a.tco_id == x.eco_tipo_correo).Select(f => f.tco_descripcion).FirstOrDefault()
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SelectListItem> CargarListadoTipoCorreo()
        {
            try
            {
                var query = _dbContext.sys_tipo_correo
                    .Select(x => new SelectListItem
                    {
                        Value = x.tco_id.ToString(),
                        Text = x.tco_descripcion
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatosCorreo CargarDatosCorreo(Guid idCorreo)
        {
            try
            {
                var query = _dbContext.adm_emisor_correo
                    .Where(x => x.eco_id == idCorreo)
                    .Select(x => new DatosCorreo
                    {
                        Contrasena = x.eco_contrasena,
                        Correo = x.eco_correo,
                        CorreoHtml = x.eco_correo_html,
                        Estado = x.eco_activo,
                        IdCorreo = x.eco_id,
                        Nombre = x.eco_nombre,
                        Puerto = x.eco_puerto,
                        Servidor = x.eco_servidor,
                        Ssl = x.eco_ssl,
                        TipoCorreo = x.eco_tipo_correo ?? Guid.Empty,
                        Usuario = x.eco_usuario
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevoCorreo(DatosCorreo datosCorreo, Guid emisor)
        {
            try
            {
                var correo = new adm_emisor_correo
                {
                    eco_activo = true,
                    eco_contrasena = datosCorreo.Contrasena,
                    eco_correo = datosCorreo.Correo,
                    eco_correo_html = datosCorreo.CorreoHtml,
                    eco_emisor = emisor,
                    eco_id = Guid.NewGuid(),
                    eco_nombre = datosCorreo.Nombre,
                    eco_puerto = datosCorreo.Puerto,
                    eco_servidor = datosCorreo.Servidor,
                    eco_ssl = datosCorreo.Ssl,
                    eco_tipo_correo = datosCorreo.TipoCorreo,
                    eco_usuario = datosCorreo.Usuario
                };

                _dbContext.adm_emisor_correo.Add(correo);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarDatosCorreo(DatosCorreo datosCorreo)
        {
            try
            {
                var correo = _dbContext.adm_emisor_correo
                    .Where(x => x.eco_id == datosCorreo.IdCorreo)
                    .FirstOrDefault();

                correo.eco_activo = datosCorreo.Estado;
                correo.eco_contrasena = datosCorreo.Contrasena;
                correo.eco_correo = datosCorreo.Correo;
                correo.eco_correo_html = datosCorreo.CorreoHtml;
                correo.eco_nombre = datosCorreo.Nombre;
                correo.eco_puerto = datosCorreo.Puerto;
                correo.eco_servidor = datosCorreo.Servidor;
                correo.eco_ssl = datosCorreo.Ssl;
                correo.eco_tipo_correo = datosCorreo.TipoCorreo;
                correo.eco_usuario = datosCorreo.Usuario;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ValidarCorreo(string correo, string contrasena, string usuario, int puerto, string servidor, bool ssl, bool sUser)
        {
            try
            {
                using (var mensaje = new MailMessage(usuario, correo))
                {
                    mensaje.IsBodyHtml = true;
                    mensaje.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    mensaje.Subject = $"Mensaje Prueba";

                    string codigoHtml = $"<!DOCTYPE html>" +
                       $"<html>" +
                       $"<head>" +
                       $"<meta http - equiv = \"Content-Type\" content = \"text/html; charset=UTF-8\" />" +
                       $"<title> Demystifying Email Design</title>" +
                       $"<meta name = \"viewport\" content = \"width=device-width, initial-scale=1.0\" />" +
                       $"</head>" +
                       $"<body style = \"margin: 0; padding: 0;\" >" +
                       $"<table align = \"center\"  cellpadding = \"0\" cellspacing = \"0\" width = \"600\" >" +
                     $"<tr>" +
                     $"<td align = \"center\" bgcolor = \"#FFFFFF\" style = \"padding: 40px 0 30px 0;\" >" +
                          $"<img src = \"http://facse.net/Content/Images/logo_facse.png \" alt = \"Creating Email Magic\" width = \"397px\" height = \"47px\" style = \"display: block;\" /><hr style= \"  border-top: 1px solid #CCCCCC \" >" +
                                   $"</td>" +
                                   $"</tr>" +
                                   $"<tr>" +
                                   $"<td bgcolor = \"#ffffff\" style = \"padding: 40px 30px 40px 30px;\" >" +
                                      $"<table  cellpadding = \"0\" cellspacing = \"0\" width = \"100% \" >" +
                                             $"<tr>" +
                                             $"<td><b>  Correo de verificación de credenciales <br><hr style= \"   border: 1px solid #CCCCCC; \" ></td>" +
                                                $"</tr>" +
                                                $"<tr><td style = \"padding: 20px 0 30px 0; \" >  </td>" +
                                                       $"</tr>" +
                                                      $"</table>" +
                                                      $"</td>" +
                                                      $"</tr>" +
                                                      $"<tr>" +
                                                      $"<td>" +
                                                                                                     $"</td>" +
                                                                                                     $"</tr>" +
                                                                                                     $"</table>" +
                                                                                                     $"</body>" +
                                                                                                     $"</html>";

                    AlternateView vistaHtml = AlternateView.CreateAlternateViewFromString(codigoHtml, null, MediaTypeNames.Text.Html);
                    mensaje.AlternateViews.Add(vistaHtml);

                    SmtpClient smtpClient = new SmtpClient(servidor)
                    {
                        Port = int.Parse(puerto.ToString())
                    };
                    if (servidor.Contains("outlook") || servidor.Contains("mi.com.co") || servidor.Contains("live.com"))
                    {
                        smtpClient.UseDefaultCredentials = false;
                    }

                    NetworkCredential credential = new NetworkCredential(usuario.Trim(), contrasena.Trim());
                    smtpClient.EnableSsl = ssl;
                    smtpClient.Credentials = credential;

                    smtpClient.Send(mensaje);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Certificado

        public List<DatosCertificado> CargarCertificados(Guid idEmisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_certificado
                    .Where(x => x.ece_emisor == idEmisor)
                    .Select(x => new DatosCertificado
                    {
                        Archivo = x.ece_archivo,
                        Certificado = x.ece_certificado,
                        Contrasena = x.ece_contrasena,
                        Estado = x.ece_activo,
                        FechaVigencia = x.ece_fecha_vegencia,
                        IdCertificado = x.ece_id
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatosCertificado CargarDatosCertificado(Guid idCertificado)
        {
            try
            {
                var query = _dbContext.adm_emisor_certificado
                    .Where(x => x.ece_id == idCertificado)
                    .Select(x => new DatosCertificado
                    {
                        Archivo = x.ece_archivo,
                        Certificado = x.ece_certificado,
                        Contrasena = x.ece_contrasena,
                        Estado = x.ece_activo,
                        FechaVigencia = x.ece_fecha_vegencia,
                        IdCertificado = x.ece_id
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevoCertificado(DatosCertificado certificado, Guid emisor)
        {
            try
            {
                var newCertificado = new adm_emisor_certificado
                {
                    ece_activo = certificado.Estado,
                    ece_archivo = certificado.Archivo,
                    ece_certificado = certificado.Certificado,
                    ece_contrasena = certificado.Contrasena,
                    ece_emisor = emisor,
                    ece_fecha_vegencia = certificado.FechaVigencia,
                    ece_id = Guid.NewGuid()
                };

                _dbContext.adm_emisor_certificado.Add(newCertificado);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarCertificado(DatosCertificado certificado)
        {
            try
            {
                var query = _dbContext.adm_emisor_certificado
                    .Where(x => x.ece_id == certificado.IdCertificado)
                    .FirstOrDefault();

                query.ece_activo = certificado.Estado;
                query.ece_archivo = certificado.Archivo;
                query.ece_certificado = certificado.Certificado;
                query.ece_contrasena = certificado.Contrasena;
                query.ece_fecha_vegencia = certificado.FechaVigencia;


                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ValidarActivoCertificado(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_certificado
                    .Where(x => x.ece_emisor == emisor && x.ece_activo == true)
                    .Any();

                return !query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Catalogo

        public List<DatosCatalogo> CargarListadoCatalogo(Guid emisor, Guid tipoCat)
        {
            try
            {
                var query = _dbContext.adm_emisor_catalogo
                    .Where(x => x.eca_emisor == emisor && x.eca_tipo_catalogo == tipoCat)
                    .Select(x => new DatosCatalogo
                    {
                        IdCatalogo = x.eca_id,
                        Lista = x.eca_lista,
                        Nombre = x.eca_nombre,
                        TipoCatalogo = x.eca_tipo_catalogo,
                        TipoDato = x.eca_tipo_dato,
                        TipoCatalogoText = x.sys_tipo_catalogo.tca_nombre,
                        TipoDatoText = x.sys_tipo_dato.tda_descripcion,
                        Estado = x.eca_activo ?? false
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoTipoDato()
        {
            try
            {
                var query = _dbContext.sys_tipo_dato
                    .Select(x => new SelectListItem
                    {
                        Text = x.tda_descripcion,
                        Value = x.tda_id.ToString()
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoTipoCatalogo()
        {
            try
            {
                var query = _dbContext.sys_tipo_catalogo
                    .Select(x => new SelectListItem
                    {
                        Value = x.tca_id.ToString(),
                        Text = x.tca_nombre
                    }).OrderByDescending(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatosCatalogo CargarDatosCatalogo(Guid idCatalogo)
        {
            try
            {
                var query = _dbContext.adm_emisor_catalogo
                    .Where(x => x.eca_id == idCatalogo)
                    .Select(x => new DatosCatalogo
                    {
                        IdCatalogo = x.eca_id,
                        Lista = x.eca_lista,
                        Nombre = x.eca_nombre,
                        TipoCatalogo = x.eca_tipo_catalogo,
                        TipoDato = x.eca_tipo_dato,
                        Estado = x.eca_activo ?? false
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevoCatalogo(DatosCatalogo catalogo, Guid emisor)
        {
            try
            {
                var newCatalogo = new adm_emisor_catalogo
                {
                    eca_emisor = emisor,
                    eca_id = Guid.NewGuid(),
                    eca_lista = catalogo.Lista,
                    eca_nombre = catalogo.Nombre,
                    eca_tipo_catalogo = catalogo.TipoCatalogo,
                    eca_tipo_dato = catalogo.TipoDato,
                    eca_activo = true
                };

                _dbContext.adm_emisor_catalogo.Add(newCatalogo);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarDatosCatalogo(DatosCatalogo catalogo)
        {
            try
            {
                var query = _dbContext.adm_emisor_catalogo
                    .Where(x => x.eca_id == catalogo.IdCatalogo)
                    .FirstOrDefault();

                query.eca_lista = catalogo.Lista;
                query.eca_nombre = catalogo.Nombre;
                query.eca_tipo_catalogo = catalogo.TipoCatalogo;
                query.eca_tipo_dato = catalogo.TipoDato;
                query.eca_activo = catalogo.Estado;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<DatosCatalogoDetalle> CargarListadoCatalogoDetalles(Guid idCatalogo)
        {
            try
            {
                var query = _dbContext.adm_emisor_catalogo_lista
                    .Where(x => x.ecl_emisor_catalogo == idCatalogo)
                    .Select(x => new DatosCatalogoDetalle
                    {
                        Descripcion = x.ecl_descripcion,
                        Id = x.ecl_id
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CrearDetalleCatalogo(Guid idCatalogo, string descripcion)
        {
            try
            {
                var newCatalogoDetalle = new adm_emisor_catalogo_lista
                {
                    ecl_descripcion = descripcion,
                    ecl_emisor_catalogo = idCatalogo,
                    ecl_id = Guid.NewGuid()
                };

                _dbContext.adm_emisor_catalogo_lista.Add(newCatalogoDetalle);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool EliminarDetalleCatalogo(Guid idDetalle)
        {
            try
            {
                var query = _dbContext.adm_emisor_catalogo_lista
                    .Where(x => x.ecl_id == idDetalle).FirstOrDefault();

                _dbContext.adm_emisor_catalogo_lista.Remove(query);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Producto

        public List<DatosProducto> CargarListaProducto(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_producto
                    .Where(x => x.epr_emisor == emisor)
                    .Select(x => new DatosProducto
                    {
                        IdProducto = x.epr_id,
                        Emisor = x.epr_emisor,
                        Codigo = x.epr_codigo,
                        Descripcion = x.epr_descripcion,
                        IdUnidad = x.epr_unidad,
                        ValorUnidad = x.epr_valor_unitario,
                        IdImpuesto = x.epr_tipo_impuesto,
                        Activo = x.epr_activo
                    }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<SelectListItem> CargarTipoCatalogo()
        {
            try
            {
                var query = _dbContext.sys_tipo_catalogo
                    .Select(x => new SelectListItem
                    {
                        Value = x.tca_id.ToString(),
                        Text = x.tca_nombre
                    }).OrderByDescending(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DatosProducto CargarDatosProducto(Guid IdProducto)
        {
            try
            {
                var query = _dbContext.adm_emisor_producto
                    .Where(x => x.epr_id == IdProducto)
                    .Select(x => new DatosProducto
                    {
                        IdProducto = x.epr_id,
                        //Emisor = x.epr_emisor,
                        Codigo = x.epr_codigo,
                        Descripcion = x.epr_descripcion,
                        IdUnidad = x.epr_unidad,
                        ValorUnidad = x.epr_valor_unitario,
                        IdImpuesto = x.epr_tipo_impuesto,
                        Activo = x.epr_activo
                    }).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarProducto(DatosProducto producto, Guid emisor, List<Catalogo> listadoCatalogo)
        {
            try
            {
                if (listadoCatalogo != null)
                {
                    var listadoCatalogoNew = listadoCatalogo
                                         .Select(c => new adm_emisor_producto_catalogo
                                         {
                                             epc_catalogo = c.Id,
                                             epc_valor = c.Valor,

                                         }).ToList();
                    var newProducto = new adm_emisor_producto()
                    {
                        epr_id = Guid.NewGuid(),
                        epr_emisor = emisor,
                        epr_codigo = producto.Codigo,
                        epr_descripcion = producto.Descripcion,
                        epr_unidad = producto.IdUnidad,
                        epr_valor_unitario = producto.ValorUnitario,
                        epr_tipo_impuesto = producto.IdImpuesto,
                        epr_activo = true,
                        adm_emisor_producto_catalogo = listadoCatalogoNew
                    };

                    _dbContext.adm_emisor_producto.Add(newProducto);

                    _dbContext.SaveChanges();

                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public bool ActualizarProducto(DatosProducto producto, List<Catalogo> listadoCatalogo)
        {
            try
            {
                if (listadoCatalogo != null)
                {
                    var borrar = _dbContext.adm_emisor_producto_catalogo
                        .Where(b => b.epc_producto == producto.Id).ToList();
                    _dbContext.adm_emisor_producto_catalogo.RemoveRange(borrar);

                    var listadoCatalogoExistente = listadoCatalogo
                                                 .Select(c => new adm_emisor_producto_catalogo
                                                 {
                                                     epc_catalogo = c.Id,
                                                     epc_valor = c.Valor,

                                                 }).ToList();

                    var query = _dbContext.adm_emisor_producto
                            .Where(x => x.epr_id == producto.Id)
                            .FirstOrDefault();
                    query.epr_codigo = producto.Codigo;
                    query.epr_descripcion = producto.Descripcion;
                    query.epr_unidad = producto.IdUnidad;
                    query.epr_valor_unitario = producto.ValorUnitario;
                    query.epr_tipo_impuesto = producto.IdImpuesto;
                    query.epr_activo = producto.Activo;
                    query.adm_emisor_producto_catalogo = listadoCatalogoExistente;

                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<DatosProductoDetalle> CargarProductoDetalle(Guid Id)
        {
            try
            {
                var query = _dbContext.adm_emisor_producto
                    .Where(x => x.epr_id == Id)
                    .Select(x => new DatosProductoDetalle
                    {
                        Descripcion = x.epr_descripcion
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoTipoImpuesto()
        {
            try
            {
                var IdClasificacion = Guid.Parse("318E6846-C1C4-4152-BB9A-3C51F6285891");
                var query = _dbContext.sys_tipo_impuesto
                .Where(x => x.tim_clasificacion == IdClasificacion)
                .Select(x => new SelectListItem
                {
                    Value = x.tim_id.ToString(),
                    Text = x.tim_nombre
                }).OrderByDescending(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> CargarListadoTipoUnidad()
        {
            try
            {
                var query = _dbContext.sys_tipo_unidad_cantidad
                 .Select(x => new SelectListItem
                 {
                     Value = x.tuc_id.ToString(),
                     Text = x.tuc_descripcion
                 }).OrderByDescending(x => x.Text).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Catalogo> CatalogoAsignado(Guid idProducto, List<Catalogo> ListadoCatalogo)
        {
            try
            {
                var listaAsignado = _dbContext.adm_emisor_producto_catalogo
               .Where(x => x.epc_producto == idProducto)
                   .Select(x => new Catalogo
                   {
                       Descripcion = x.adm_emisor_catalogo.eca_nombre,
                       Id = x.epc_catalogo,
                       Valor = x.epc_valor,
                       ListadoCag = x.adm_emisor_catalogo.adm_emisor_catalogo_lista.Select(c => new SelectListItem
                       {
                           Text = c.ecl_descripcion,
                           Value = c.ecl_descripcion,
                           Selected = c.ecl_descripcion == x.epc_valor
                       }).ToList(),
                       Lista = x.adm_emisor_catalogo.eca_lista
                   }).ToList();

                var listaFiltro = ListadoCatalogo.Where(c => !listaAsignado.Select(q => q.Id).Contains(c.Id)).ToList();

                listaAsignado = listaAsignado.Union(listaFiltro).ToList();

                return listaAsignado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool Eliminar(Guid catalogo)
        {
            try
            {
                var query = _dbContext.adm_emisor_producto_catalogo
                    .Where(x => x.epc_catalogo == catalogo).FirstOrDefault();
                _dbContext.adm_emisor_producto_catalogo.Remove(query);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Impuesto
        public List<SelectListItem> ListadoImpuestoAsignado(Guid emisor)
        {
            try
            {
                var query = _dbContext.adm_emisor_impuesto
                    .Where(x => x.emi_id_emisor == emisor)
                    .Join(_dbContext.sys_tipo_impuesto,
                            eim => eim.emi_id_impuesto,
                            tim => tim.tim_id,
                            (eim, tim) => new { eim, tim }
                        )
                    .Select(x => new SelectListItem
                    {
                        Text = x.tim.tim_nombre,
                        Value = x.tim.tim_id.ToString(),
                    }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> ListadoImpuestoDisponible(List<SelectListItem> ImpuestoAsignado)
        {
            try
            {
                var query = _dbContext.sys_tipo_impuesto
                     .Where(x => x.tim_clasificacion.ToString() == "318E6846-C1C4-4152-BB9A-3C51F6285891")
                    .Select(x => new SelectListItem
                    {
                        Text = x.tim_nombre,
                        Value = x.tim_id.ToString()
                    }).ToList()
                    .Where(x => !ImpuestoAsignado.Select(a => a.Value).ToList().Contains(x.Value)).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AsignarImpuesto(Guid emisor, Guid impuesto)
        {
            try
            {
                var impu = new adm_emisor_impuesto
                {
                    emi_id_emisor = emisor,
                    emi_id_impuesto = impuesto,
                    emi_fecha = DateTime.Now,
                };
                _dbContext.adm_emisor_impuesto.Add(impu);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool EliminarImpuesto(Guid emisor, Guid impuesto)
        {
            try
            {
                var eli = _dbContext.adm_emisor_impuesto
                    .Where(x => x.emi_id_emisor == emisor && x.emi_id_impuesto == impuesto).FirstOrDefault();

                _dbContext.adm_emisor_impuesto.Remove(eli);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Plantilla

        public List<DatosPlantilla> CargarlistadoPlantilla()
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal_plantilla
                    .Select(x => new DatosPlantilla
                    {
                        IdEmisorSucursalPlantilla = x.esp_id,
                        IdEmisorSucursal = x.esp_emisor_sucursal,
                        EmisorSucursal = _dbContext.adm_emisor_sucursal.Where(a => a.esu_id == x.esp_emisor_sucursal).Select(f => f.esu_nombre).FirstOrDefault(),
                        Logo = x.esp_logo,
                        PrimerMensaje = x.esp_primer_mensaje,
                        SegundoMensaje = x.esp_segundo_mensaje,
                        TercerMensaje = x.esp_tercer_mensaje,
                        IdUsuarioCreacion = x.esp_usuario_creacion,
                        UsuarioCreacion = _dbContext.adm_usuario.Where(a => a.usu_id == x.esp_usuario_creacion).Select(f => f.usu_nombre).FirstOrDefault(),
                        FechaCreacion = x.esp_fecha_creacion
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatosPlantilla CargarDatosPlantilla(Guid idPlantilla)
        {
            try
            {
                var query = _dbContext.adm_emisor_sucursal_plantilla
                    .Where(p => p.esp_id == idPlantilla)
                    .Select(x => new DatosPlantilla
                    {
                        IdEmisorSucursalPlantilla = x.esp_id,
                        IdEmisorSucursal = x.adm_emisor_sucursal.esu_id,
                        EmisorSucursal = _dbContext.adm_emisor_sucursal.Where(a => a.esu_id == x.esp_emisor_sucursal).Select(f => f.esu_nombre).FirstOrDefault(),
                        Logo = x.esp_logo,
                        PrimerMensaje = x.esp_primer_mensaje,
                        SegundoMensaje = x.esp_segundo_mensaje,
                        TercerMensaje = x.esp_tercer_mensaje,
                        IdUsuarioCreacion = x.adm_usuario.usu_id,
                        UsuarioCreacion = _dbContext.adm_usuario.Where(a => a.usu_id == x.esp_usuario_creacion).Select(f => f.usu_nombre).FirstOrDefault(),
                        FechaCreacion = x.esp_fecha_creacion
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarNuevoPlantilla(DatosPlantilla datosPlantilla)
        {
            try
            {
                var plantilla = new adm_emisor_sucursal_plantilla
                {
                    esp_id = datosPlantilla.IdEmisorSucursalPlantilla,
                    esp_emisor_sucursal = datosPlantilla.IdEmisorSucursal,
                    esp_logo = datosPlantilla.Logo,
                    esp_primer_mensaje = datosPlantilla.PrimerMensaje,
                    esp_segundo_mensaje = datosPlantilla.SegundoMensaje,
                    esp_tercer_mensaje = datosPlantilla.TercerMensaje,
                    esp_usuario_creacion = datosPlantilla.IdUsuarioCreacion,
                    esp_fecha_creacion = DateTime.Now,
                    adm_emisor_sucursal = _dbContext.adm_emisor_sucursal.Where(x => x.esu_emisor == datosPlantilla.EsuEmisor).FirstOrDefault(),
                    adm_usuario = _dbContext.adm_usuario.Where(x => x.usu_id == datosPlantilla.IdUsuarioCreacion).FirstOrDefault()
                };

                _dbContext.adm_emisor_sucursal_plantilla.Add(plantilla);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarDatosPlantilla(DatosPlantilla datosPlantilla, bool bImagen)
        {
            try
            {
                var plantilla = _dbContext.adm_emisor_sucursal_plantilla
                    .Where(x => x.esp_id == datosPlantilla.IdEmisorSucursalPlantilla)
                    .FirstOrDefault();

                plantilla.esp_id = datosPlantilla.IdEmisorSucursalPlantilla;
                plantilla.esp_emisor_sucursal = datosPlantilla.IdEmisorSucursal;
                if (bImagen)
                    plantilla.esp_logo = datosPlantilla.Logo;
                plantilla.esp_primer_mensaje = datosPlantilla.PrimerMensaje;
                plantilla.esp_segundo_mensaje = datosPlantilla.SegundoMensaje;
                plantilla.esp_tercer_mensaje = datosPlantilla.TercerMensaje;
                plantilla.esp_usuario_creacion = datosPlantilla.IdUsuarioCreacion;
                plantilla.esp_fecha_creacion = datosPlantilla.FechaCreacion;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<dynamic> ArchivoPlantilla(string rutaLogo, Guid idESPlantilla)
        {
            try
            {

                return _dbContext.adm_emisor_sucursal_plantilla.AsNoTracking()
                    .Where(e => e.esp_id == idESPlantilla)
                    .Select(e => new
                    {
                        Ruta = e.esp_logo,
                        Logo = rutaLogo
                    }).ToList<dynamic>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ValidarExistenciaImagen(string fileName)
        {
            bool existe = false;

            var rutaImagen = _dbContext.adm_emisor_sucursal_plantilla
                    .Select(x => x.esp_logo).ToList();

            if (rutaImagen != null)
            {
                foreach (var i in rutaImagen)
                {
                    string[] rutaImagenChar = i.Split('/');

                    if (rutaImagenChar.LastOrDefault() == fileName)
                    {
                        existe = true;
                        break;
                    }
                }
            }

            return existe;
        }

        public string TraerRuta(Guid gPlantilla)
        {
            try
            {
                var ruta = _dbContext.adm_plantilla.AsNoTracking().Where(p => p.prg_id == gPlantilla).FirstOrDefault()?.prg_direccion_pdf;
                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}
