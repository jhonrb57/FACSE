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
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    public class ConfiguracionController : BaseController
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsAdquiriente).Name);
        private readonly ClsConfiguracion _clsConfiguracion = new ClsConfiguracion();
        private readonly ClsAdquiriente _clsAdquiriente = new ClsAdquiriente();
        private readonly ClsFacturacionManual _clsFacturacionManual = new ClsFacturacionManual();
        private readonly ClsUsuario _clsUsuario = new ClsUsuario();
        private readonly ClsEmisor _clsEmisor = new ClsEmisor();
        // GET: Configuracion
        public ActionResult Index()
        {
            if (TempData["sMensajeEmi"] is string mensaje)
            {
                ViewBag.JsFuncion = mensaje;
            }

            return View();
        }

        #region Usuario

        public ActionResult Usuario()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];




                var model = new Usuario
                {
                    ListadoUsuario = _clsConfiguracion.CargarListadoUsuarios(emisor)
                };

                if (TempData["sUsuario"] is string smensaje)
                {
                    model.JsFuncion = smensaje;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }

        }

        public ActionResult CrearEditarUsuario(Guid? idUsuario)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var model = new DatosUsuario();




                if (idUsuario != null && idUsuario != Guid.Empty)
                {



                    ViewBag.Titulo = "Editar usuario";

                    model = _clsConfiguracion.CargarDatosUsuario(idUsuario ?? Guid.Empty);
                    model.ListadoTipo = _clsConfiguracion.CargarListadoTipoUsuario();
                    model.ListadoPerfil = _clsConfiguracion.CargarListadoPerfiles();
                    model.ListadoSucursales = _clsConfiguracion.CargarListadoSucursales(emisor);

                    return View(model);
                }
                else
                {
                    ViewBag.Titulo = "Crear usuario";

                    model.ListadoTipo = _clsConfiguracion.CargarListadoTipoUsuario();
                    model.ListadoPerfil = _clsConfiguracion.CargarListadoPerfiles();
                    model.ListadoSucursales = _clsConfiguracion.CargarListadoSucursales(emisor);

                    return View(model);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EnviarFormularioUsuario(DatosUsuario datosUsuario)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                datosUsuario.Tipo = Guid.Parse("6030D639-789C-4B47-9D35-015D0FD310A2");

                if (datosUsuario.Id != Guid.Empty)
                {
                    var result = _clsConfiguracion.ActualizarUsuario(datosUsuario, emisor);

                    string mensaje = (result) ? $"Usuario {datosUsuario.sUsuario} se ha editado correctamente" : $"No se ha podido editar    usuario {datosUsuario.sUsuario}";
                    string icon = (result) ? "success" : "error";
                    TempData["sUsuario"] = $"mensaje('{mensaje}','{icon}')";
                }
                else
                {
                    string usuario = NombreUsuario(datosUsuario.Perfil);
                    datosUsuario.sUsuario = usuario;
                    datosUsuario.Contrasena = usuario;
                    var result = _clsConfiguracion.GuardarNuevoUsuario(datosUsuario, emisor);

                    string mensaje = (result) ? $"Usuario {usuario} se ha guardado correctamente" : $"No se ha podido guardar usuario {usuario}";
                    string icon = (result) ? "success" : "error";
                    TempData["sUsuario"] = $"mensaje('{mensaje}','{icon}')";
                }

                return RedirectToAction("Usuario");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public string NombreUsuario(Guid perfil)
        {
            try
            {
                string usuario = "";
                string idUsuario = Session["i_susuario"].ToString();
                var emisor = (Guid)Session["i_id_emisor"];

                var descripcionPerfil = _clsConfiguracion.DescripcionPerfil(perfil);
                var consecutivo = _clsConfiguracion.CalcularCantidadUsuarios(emisor) + 1;

                usuario = idUsuario + descripcionPerfil.Substring(0, 1) + consecutivo.ToString();

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UsuarioSucursal(Guid idUsuario)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var (asignadas, disponibles) = _clsConfiguracion.SucursalesUsuarioAsignadasDisponibles(idUsuario, emisor);

                var model = new UsuarioSucursal
                {
                    ListAsignada = asignadas,
                    ListDisponible = disponibles,
                    IdUsuario = idUsuario
                };
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult AsignarSucursalUsuario(Guid idUsuario, Guid idSucursal)
        {
            try
            {
                var result = _clsConfiguracion.AsignarSucursalUsuario(idUsuario, idSucursal);

                return RedirectToAction("UsuarioSucursal", new { idUsuario });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EliminarUsuarioSucursal(Guid idUsuario, Guid idSucursal)
        {
            try
            {
                var result = _clsConfiguracion.EliminarSucursalUsuario(idUsuario, idSucursal);

                return RedirectToAction("UsuarioSucursal", new { idUsuario });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region Sucursal

        public ActionResult Sucursal()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var model = new Sucursal
                {
                    ListadoSucursal = _clsConfiguracion.ListadoSucursales(emisor)
                };

                if (TempData["sucMensaje"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarSucursal(Guid idSucursal)
        {
            try
            {


                var clavesuper = Session["clave_super"];



                var emisor = (Guid)Session["i_id_emisor"];

                var model = new Sucursal();

                if (idSucursal == Guid.Empty)//crear
                {

                    model.Titulo = "Crear Sucursal";
                    model.ListadoPais = _clsAdquiriente.ListadoPaises();
                    model.ListadoDepartamento = _clsAdquiriente.ListadoDptos(Guid.Parse(model.ListadoPais[0].Value));
                    model.ListadoMunicipio = (model.ListadoDepartamento.Count > 1) ? _clsAdquiriente.ListadoMunicipios(Guid.Parse(model.ListadoDepartamento[0].Value)) : new List<SelectListItem> { new SelectListItem { Value = null, Text = "No aplica" } };
                    model.ListadoCorreoEntrada = _clsConfiguracion.CargarListadoCorreo(emisor, Guid.Parse("F394C319-E5D4-41D2-975C-F6879BC1ADE2"));
                    model.ListadoCorreoSalida = _clsConfiguracion.CargarListadoCorreo(emisor, Guid.Parse("D9B983B0-DE11-4E26-89EF-5046EF8A006B"));
                }
                else//editar
                {
                    var result = _clsConfiguracion.CargarDatosSucursal(idSucursal);


                    model.Titulo = "Editar Sucursal";
                    model.Abreviatura = result.Abreviatura;
                    model.CodigoPostal = result.CodigoPostal;
                    model.Departamento = result.Departamento;
                    model.Direccion = result.Direccion;
                    model.Email = result.Email;
                    model.Estado = result.Estado;
                    model.Id = result.Id;
                    model.Municipio = result.Municipio;
                    model.Nombre = result.Nombre;
                    model.Pais = result.Pais;
                    model.Telefono = result.Telefono;
                    model.ListadoPais = _clsAdquiriente.ListadoPaises();
                    model.ListadoDepartamento = _clsAdquiriente.ListadoDptos(result.Pais);
                    model.ListadoMunicipio = _clsAdquiriente.ListadoMunicipios(result.Departamento);
                    model.ListadoCorreoEntrada = _clsConfiguracion.CargarListadoCorreo(emisor, Guid.Parse("F394C319-E5D4-41D2-975C-F6879BC1ADE2"));
                    model.CorreoEntrada = result.CorreoEntrada;
                    model.ListadoCorreoSalida = _clsConfiguracion.CargarListadoCorreo(emisor, Guid.Parse("D9B983B0-DE11-4E26-89EF-5046EF8A006B"));
                    model.CorreoSalida = result.CorreoSalida;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CargarListadoDepartamentos(string pais)
        {
            try
            {
                var dptos = _clsAdquiriente.ListadoDptos(Guid.Parse(pais));
                return Json(dptos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CargarListadoMunicipios(string dpto)
        {
            try
            {
                if (dpto != null && dpto != "")
                {

                    var dptos = _clsAdquiriente.ListadoMunicipios(Guid.Parse(dpto));
                    return Json(dptos, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var dptos = _clsAdquiriente.ListadoMunicipios(null);
                    return Json(dptos, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EnviarFormularioSucursal(Sucursal sucursal)
        {
            try
            {
                if (sucursal.Estado == true || (sucursal.Estado == false && (!(_clsUsuario.ExisteUsuarioSucursal(sucursal.Id)))))
                {
                    var emisor = (Guid)Session["i_id_emisor"];

                    if (sucursal.Id == null || sucursal.Id == Guid.Empty)
                    {
                        var dSuc = new DatosSucursal
                        {
                            Abreviatura = sucursal.Abreviatura,
                            CodigoPostal = sucursal.CodigoPostal,
                            Departamento = sucursal.Departamento,
                            Direccion = sucursal.Direccion,
                            Email = sucursal.Email,
                            Estado = sucursal.Estado,
                            Id = sucursal.Id,
                            Municipio = sucursal.Municipio,
                            Nombre = sucursal.Nombre,
                            Pais = sucursal.Pais,
                            Telefono = sucursal.Telefono,
                            CorreoEntrada = sucursal.CorreoEntrada,
                            CorreoSalida = sucursal.CorreoSalida
                        };
                        var result = _clsConfiguracion.GuardarNuevaSucursal(dSuc, emisor);

                        string mensaje = (result) ? "Sucursal creada satisfactoriamente" : "No se ha podido crear sucursal";
                        string icon = (result) ? "success" : "error";

                        TempData["sucMensaje"] = $"mensaje('{mensaje}','{icon}')";

                        return RedirectToAction("Sucursal");
                    }
                    else
                    {
                        var dSuc = new DatosSucursal
                        {
                            Abreviatura = sucursal.Abreviatura,
                            CodigoPostal = sucursal.CodigoPostal,
                            Departamento = sucursal.Departamento,
                            Direccion = sucursal.Direccion,
                            Email = sucursal.Email,
                            Estado = sucursal.Estado,
                            Id = sucursal.Id,
                            Municipio = sucursal.Municipio,
                            Nombre = sucursal.Nombre,
                            Pais = sucursal.Pais,
                            Telefono = sucursal.Telefono,
                            CorreoEntrada = sucursal.CorreoEntrada,
                            CorreoSalida = sucursal.CorreoSalida
                        };

                        var result = _clsConfiguracion.ActualizarSucursal(dSuc);

                        string mensaje = (result) ? "Sucursal editada satisfactoriamente" : "No se ha podido editar sucursal";
                        string icon = (result) ? "success" : "error";

                        TempData["sucMensaje"] = $"mensaje('{mensaje}','{icon}')";

                        return RedirectToAction("Sucursal");
                    }
                }
                else
                {
                    string mensaje = "Existen Usuarios Asignados No se Puede Inactivar";
                    string icon = "error";

                    TempData["sucMensaje"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Sucursal");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult SucursalResolucion(Guid idSucursal)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var (asig, dis) = _clsConfiguracion.ResolucionesAsignadasDisponibles(idSucursal, emisor);

                var model = new SucursalResolucion
                {
                    Asignados = asig,
                    Disponibles = dis,
                    IdSucursal = idSucursal
                };

                if (TempData["sMensajeResSuc"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult AsignarResolucionSucursal(Guid idResolucion, Guid idSucursal)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var result = _clsConfiguracion.ValidarResolucion(idSucursal, emisor, idResolucion);

                if (result)
                {
                    var respuesta = _clsConfiguracion.AsignarResolucionSucursal(emisor, idSucursal, idResolucion);
                }
                else
                {
                    string mensaje = "No se puede asignar dos veces la resolución con el mismo tipo de documento";
                    string icon = "warning";
                    TempData["sMensajeResSuc"] = $"mensaje('{mensaje}','{icon}')";
                }

                return RedirectToAction("SucursalResolucion", new { idSucursal });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult EliminarResolucionSucursal(Guid idResSuc, Guid idResolucion, Guid idSucursal)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];


                var res = _clsConfiguracion.EliminarResolucionSucursal(idResSuc, emisor, idSucursal, idResolucion);


                return RedirectToAction("SucursalResolucion", new { idSucursal });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region Resolucion

        public ActionResult Resolucion()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var model = new Resolucion
                {
                    ListadoResolucion = _clsConfiguracion.CargarListadoResolucion(emisor)
                };

                if (TempData["sMensajeRes"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarResolucion(Guid idResolucion)
        {
            try
            {
                var model = new Resolucion();
                if (idResolucion == Guid.Empty)
                {
                    model.Titulo = "Crear resolución";
                    model.ListadoTipoDocumento = _clsConfiguracion.CargarListadoTipoDocumento();
                    model.ListadoPlantilla = _clsConfiguracion.CargarListadoPlantilla(null);
                    model.FechaInicio = DateTime.Now;
                    model.FechaFin = DateTime.Now;
                }
                else
                {
                    var resultado = _clsConfiguracion.ConsultarDatosResolucion(idResolucion);

                    model.Titulo = "Editar resolución";
                    model.ClaveTecnica = resultado.ClaveTecnica;
                    model.Estado = resultado.Estado;
                    model.Fecha = resultado.Fecha;
                    model.FechaFin = resultado.FechaFin;
                    model.FechaInicio = resultado.FechaInicio;
                    model.Id = resultado.Id;
                    model.ListadoTipoDocumento = _clsConfiguracion.CargarListadoTipoDocumento();
                    model.TipoDocumento = resultado.TipoDocumento;
                    model.NumeroFinal = int.Parse(resultado.NumeroFinal);
                    model.NumeroInicial = int.Parse(resultado.NumeroInicial);
                    model.NumeroResolucion = resultado.NumeroResolucion;
                    model.Prefijo = resultado.Prefijo;
                    model.ListadoPlantilla = _clsConfiguracion.CargarListadoPlantilla(model.TipoDocumento);
                    model.Plantilla = resultado.Plantilla;
                    model.Ruta = _clsConfiguracion.RutaPlantilla();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult EnviarFormularioResolucion(Resolucion resolucion)
        {
            try
            {
                var nResolucion = new DatosResolucion
                {
                    ClaveTecnica = resolucion.ClaveTecnica,
                    Estado = resolucion.Estado,
                    Fecha = resolucion.Fecha,
                    FechaFin = resolucion.FechaFin,
                    FechaInicio = resolucion.FechaInicio,
                    Id = resolucion.Id,
                    NumeroFinal = resolucion.NumeroFinal.ToString(),
                    NumeroInicial = resolucion.NumeroInicial.ToString(),
                    NumeroResolucion = resolucion.NumeroResolucion,
                    Prefijo = resolucion.Prefijo,
                    TipoDocumento = resolucion.TipoDocumento,
                    Plantilla = resolucion.Plantilla
                };

                var emisor = (Guid)Session["i_id_emisor"];
                var resultado = (resolucion.Id == Guid.Empty) ? _clsConfiguracion.GuardarNuevaResolucion(nResolucion, emisor) : _clsConfiguracion.ActualizarDatosResolucion(nResolucion);


                string mensaje = (resultado) ? "Se ha guardado satisfactoriamente" : "No se ha podido guardar";
                string icon = (resultado) ? "success" : "error";


                TempData["sMensajeRes"] = $"mensaje('{mensaje}','{icon}')";

                return RedirectToAction("Resolucion");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        #endregion

        #region Correo

        public ActionResult Correo()
        {
            try
            {

                var emisor = (Guid)Session["i_id_emisor"];

                var model = new Correo
                {
                    ListadoCorreo = _clsConfiguracion.CargarlistadoCorreo(emisor)
                };

                if (TempData["sMensajeCor"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarCorreo(Guid idCorreo)
        {
            try
            {
                var model = new Correo();
                if (idCorreo == Guid.Empty)
                {
                    model.Titulo = "Crear correo";
                    model.ListaTipoCorreo = _clsConfiguracion.CargarListadoTipoCorreo();
                }
                else
                {
                    var datos = _clsConfiguracion.CargarDatosCorreo(idCorreo);

                    model.Titulo = "Editar correo";
                    model.IdCorreo = idCorreo;
                    model.Nombre = datos.Nombre;
                    model.Puerto = datos.Puerto;
                    model.Servidor = datos.Servidor;
                    model.Ssl = datos.Ssl;
                    model.TipoCorreo = datos.TipoCorreo;
                    model.Usuario = datos.Usuario;
                    model.Contrasena = datos.Contrasena;
                    model.CorreoHtml = datos.CorreoHtml;
                    model.Estado = datos.Estado;
                    model.dCorreo = datos.Correo;
                    model.ListaTipoCorreo = _clsConfiguracion.CargarListadoTipoCorreo();

                }



                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult EnviarFomularioCorreo(Correo correo)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                if (correo.IdCorreo == Guid.Empty)
                {
                    var model = new DatosCorreo
                    {
                        Correo = correo.Usuario,
                        Contrasena = correo.Contrasena,
                        CorreoHtml = "<html>",
                        Estado = correo.Estado,
                        IdCorreo = correo.IdCorreo,
                        Nombre = correo.Nombre,
                        Puerto = correo.Puerto,
                        Servidor = correo.Servidor,
                        Ssl = correo.Ssl,
                        TipoCorreo = correo.TipoCorreo,
                        Usuario = correo.Usuario
                    };

                    var result = _clsConfiguracion.GuardarNuevoCorreo(model, emisor);

                    string mensaje = (result) ? "Se guardo satisfactoriamente" : "No se pudo guardar";
                    string icon = (result) ? "success" : "error";

                    TempData["sMensajeCor"] = $"mensaje('{mensaje}','{icon}')";

                }
                else
                {
                    var model = new DatosCorreo
                    {
                        Correo = correo.Usuario,
                        Contrasena = correo.Contrasena,
                        CorreoHtml = "<html>",
                        Estado = correo.Estado,
                        IdCorreo = correo.IdCorreo,
                        Nombre = correo.Nombre,
                        Puerto = correo.Puerto,
                        Servidor = correo.Servidor,
                        Ssl = correo.Ssl,
                        TipoCorreo = correo.TipoCorreo,
                        Usuario = correo.Usuario
                    };

                    var result = _clsConfiguracion.ActualizarDatosCorreo(model);

                    string mensaje = (result) ? "Se edito satisfactoriamente" : "No se pudo editar";
                    string icon = (result) ? "success" : "error";

                    TempData["sMensajeCor"] = $"mensaje('{mensaje}','{icon}')";
                }

                return RedirectToAction("Correo");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult ValidarDatosCorreo(string correo, string contrasena, string usuario, int puerto, string servidor, bool ssl)
        {
            try
            {
                bool res = _clsConfiguracion.ValidarCorreo(correo, contrasena, usuario, puerto, servidor, ssl, false);

                return Json(new { res });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return Json(new { res = false, mensaje = ex.ToString() });
                //return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region Certificado

        public ActionResult Certificado()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var model = new Certificado
                {
                    ListadoCertificados = _clsConfiguracion.CargarCertificados(emisor)
                };

                if (TempData["sMensajeCer"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarCertificado(Guid idCertificado)
        {
            try
            {
                if (idCertificado == Guid.Empty)
                {
                    var crear = new Certificado
                    {
                        Titulo = "Crear certificado",
                        FechaVigencia = DateTime.Now
                    };
                    return View(crear);
                }
                else
                {
                    var datos = _clsConfiguracion.CargarDatosCertificado(idCertificado);

                    var editar = new Certificado
                    {
                        Titulo = "Editar certificado",
                        Archivo = datos.Archivo,
                        Contrasena = datos.Contrasena,
                        Estado = datos.Estado,
                        FechaVigencia = datos.FechaVigencia,
                        IdCertificado = datos.IdCertificado,
                        sCertificado = datos.Certificado
                    };

                    return View(editar);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EnviarFormularioCertificado(Certificado certificado)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var datos = new DatosCertificado
                {
                    Archivo = certificado.Archivo,
                    Certificado = certificado.sCertificado,
                    Contrasena = certificado.Contrasena,
                    Estado = certificado.Estado,
                    FechaVigencia = certificado.FechaVigencia,
                    IdCertificado = certificado.IdCertificado
                };

                if (certificado.IdCertificado == Guid.Empty)
                {
                    var result = _clsConfiguracion.GuardarNuevoCertificado(datos, emisor);

                    string mensaje = (result) ? "Se  guardo satisfactoriamente" : "No se pudo guardar";
                    string icon = (result) ? "success" : "error";

                    TempData["sMensajeCer"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Certificado");
                }
                else
                {
                    var result = _clsConfiguracion.ActualizarCertificado(datos);

                    string mensaje = (result) ? "Se  edito satisfactoriamente" : "No se pudo editar";
                    string icon = (result) ? "success" : "error";

                    TempData["sMensajeCer"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Certificado");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult ValidarActivoCertificado()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var result = _clsConfiguracion.ValidarActivoCertificado(emisor);

                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = false, val = ex }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Catalogo

        public ActionResult Catalogo(Guid idTipoCat)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                Guid idTipoCatalogo = (idTipoCat == Guid.Empty) ? Guid.Parse("E5077C84-C8AA-4A0D-8E4F-4491908DE7A6") : idTipoCat;
                var model = new CatalogoModel
                {
                    ListadoCatalogo = _clsConfiguracion.CargarListadoCatalogo(emisor, idTipoCatalogo),
                    ListadoTipoCatalogo = _clsConfiguracion.CargarListadoTipoCatalogo(),
                    TipoCatalogo = idTipoCatalogo
                };

                if (TempData["sMensajeCat"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarCatalogo(Guid idCatalogo)
        {
            try
            {
                if (idCatalogo == Guid.Empty)
                {
                    var crear = new CatalogoModel
                    {
                        Titulo = "Crear catalogo",
                        ListadoTipoCatalogo = _clsConfiguracion.CargarListadoTipoCatalogo(),
                        ListadoTipoDato = _clsConfiguracion.CargarListadoTipoDato()
                    };
                    return View(crear);
                }
                else
                {
                    var datos = _clsConfiguracion.CargarDatosCatalogo(idCatalogo);
                    var editar = new CatalogoModel
                    {
                        Titulo = "Editar catalogo",
                        TipoCatalogo = datos.TipoCatalogo,
                        TipoDato = datos.TipoDato,
                        ListadoTipoCatalogo = _clsConfiguracion.CargarListadoTipoCatalogo(),
                        ListadoTipoDato = _clsConfiguracion.CargarListadoTipoDato(),
                        IdCatalogo = datos.IdCatalogo,
                        Nombre = datos.Nombre,
                        Lista = datos.Lista,
                        Estado = datos.Estado
                    };
                    return View(editar);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EnviarFormularioCatalogo(CatalogoModel catalogo)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var datos = new DatosCatalogo
                {
                    IdCatalogo = catalogo.IdCatalogo,
                    Lista = catalogo.Lista,
                    Nombre = catalogo.Nombre,
                    TipoCatalogo = catalogo.TipoCatalogo,
                    TipoDato = catalogo.TipoDato,
                    Estado = catalogo.Estado
                };
                if (catalogo.IdCatalogo == Guid.Empty)
                {
                    var result = _clsConfiguracion.GuardarNuevoCatalogo(datos, emisor);

                    string mensaje = (result) ? "Se guardo satisfactoriamente" : "No se pudo guardar";
                    string icon = (result) ? "success" : "error";

                    TempData["sMensajeCat"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Catalogo", new { idTipoCat = Guid.Empty });
                }
                else
                {
                    var result = _clsConfiguracion.ActualizarDatosCatalogo(datos);

                    string mensaje = (result) ? "Se edito satisfactoriamente" : "No se pudo editar";
                    string icon = (result) ? "success" : "error";

                    TempData["sMensajeCat"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Catalogo", new { idTipoCat = Guid.Empty });
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CatalogoDetalle(Guid idCatalogo)
        {
            try
            {
                var model = new CatalogoDetalle
                {
                    ListadoDetallesCatalogo = _clsConfiguracion.CargarListadoCatalogoDetalles(idCatalogo),
                    IdCatalogo = idCatalogo
                };

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult AgregarDetalleCatalogo(Guid IdCatalogo, string Descripcion)
        {
            try
            {
                var result = _clsConfiguracion.CrearDetalleCatalogo(IdCatalogo, Descripcion);

                return RedirectToAction("CatalogoDetalle", new { idCatalogo = IdCatalogo });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EliminarDetalleCatalogo(Guid idDetalle, Guid idCatalogo)
        {
            try
            {

                var result = _clsConfiguracion.EliminarDetalleCatalogo(idDetalle);

                return RedirectToAction("CatalogoDetalle", new { idCatalogo });

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region Producto
        public ActionResult Producto()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var model = new ProductoModel
                {
                    ListadoProducto = _clsConfiguracion.CargarListaProducto(emisor),
                };

                if (TempData["sMensajeCat"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarProducto(Guid IdProducto)
        {
            try
            {
                Session["sCatalogoGeneral"] = null;
                var emisor = (Guid)Session["i_id_emisor"];
                var tipoCatalogo = Guid.Parse("DEFE9870-535B-4456-8DE3-13F40B5CCBF9");

                var model = new ProductoModel
                {
                    ListadoTipoUnidad = _clsConfiguracion.CargarListadoTipoUnidad(),
                    ListadoTipoImpuesto = _clsConfiguracion.CargarListadoTipoImpuesto(),
                    ListadoCatalogo = _clsFacturacionManual.CargarDatosCatalogo(emisor, tipoCatalogo)
                };

                if (IdProducto == Guid.Empty)
                {
                    model.Titulo = "Crear Producto";
                }
                else
                {
                    var datos = _clsConfiguracion.CargarDatosProducto(IdProducto);
                    var catalogo = _clsConfiguracion.CatalogoAsignado(IdProducto, model.ListadoCatalogo);

                    model.Titulo = "Editar Producto";
                    model.IdProducto = datos.IdProducto;
                    model.Codigo = datos.Codigo;
                    model.Descripcion = datos.Descripcion;
                    model.IdUnidad = datos.IdUnidad;
                    model.ValorUnitario = datos.ValorUnidad;
                    model.IdImpuesto = datos.IdImpuesto;
                    model.Activo = datos.Activo;
                    model.ListadoCatalogo = catalogo;
                    /*model.ListadoExistente = catalogoExistente*/
                    ;
                }
                Session["sCatalogoGeneral"] = model.ListadoCatalogo;
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EnviarFormularioProducto(ProductoModel producto)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var datos = new DatosProducto
                {
                    Id = producto.IdProducto,
                    Descripcion = producto.Descripcion,
                    IdImpuesto = producto.IdImpuesto,
                    IdUnidad = producto.IdUnidad,
                    ListadoTipoUnidad = producto.ListadoTipoUnidad,
                    ListadoTipoImpuesto = producto.ListadoTipoImpuesto,
                    Activo = producto.Activo,
                    Codigo = producto.Codigo,
                    ValorUnitario = producto.ValorUnitario
                };
                var listadoCatalogo = Session["sCatalogoGeneral"] as List<Catalogo>;

                if (producto.IdProducto == Guid.Empty)
                {
                    var resultado = _clsConfiguracion.GuardarProducto(datos, emisor, listadoCatalogo);
                    string mensaje = (resultado) ? "Se guardo satisfactoriamente" : "No se pudo guardar";
                    string icon = (resultado) ? "success" : "error";

                    TempData["sMensajeCat"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Producto", new { IdProducto = Guid.Empty });
                }
                else
                {
                    var resultado = _clsConfiguracion.ActualizarProducto(datos, listadoCatalogo);

                    string mensaje = (resultado) ? "Se edito satisfactoriamente" : "No se pudo editar";
                    string icon = (resultado) ? "success" : "error";

                    TempData["sMensajeCat"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Producto", new { IdProducto = Guid.Empty });
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        public ActionResult ProductoDetalle(Guid IdProducto)
        {
            try
            {
                var model = new ProductoDetalle
                {
                    ListadoDetallesProducto = _clsConfiguracion.CargarProductoDetalle(IdProducto),
                    IdProducto = IdProducto
                };
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }

        }

        public ActionResult ActualizarCatalogos(Guid IdCatalogo, string valor)
        {
            try
            {
                var listadoCatalogo = Session["sCatalogoGeneral"] as List<Catalogo>;

                var catalogo = listadoCatalogo.Where(x => x.Id == IdCatalogo).FirstOrDefault();

                catalogo.Valor = valor;

                Session["sCatalogoGeneral"] = listadoCatalogo;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region Impuesto
        public ActionResult Impuesto()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var ImpuestoA = _clsConfiguracion.ListadoImpuestoAsignado(emisor);
                var ImpuestoD = _clsConfiguracion.ListadoImpuestoDisponible(ImpuestoA);

                var model = new AsignarImpuesto
                {
                    ImpuestoDisponible = ImpuestoD,
                    ImpuestoAsignado = ImpuestoA,
                };
                return View(model);

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        public ActionResult AsignarImpuesto(Guid Value)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var IdSucursal = (Guid)Session["i_id_sucursal"];

                var impu = _clsConfiguracion.AsignarImpuesto(emisor, Value);

                return RedirectToAction("Impuesto", new { IdSucursal });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EliminarImpuesto(Guid Value)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var IdSucursal = (Guid)Session["i_id_sucursal"];
                var eli = _clsConfiguracion.EliminarImpuesto(emisor, Value);
                return RedirectToAction("Impuesto", new { IdSucursal });
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region Plantilla

        public ActionResult Plantilla()
        {
            try
            {
                var model = new Plantilla
                {
                    ListadoPlantilla = _clsConfiguracion.CargarlistadoPlantilla()
                };

                model.IdEmisorSucursalLogueo = (Guid)Session["i_id_sucursal"];

                if (TempData["sMensajePla"] is string mensaje)
                {
                    model.JsFuncion = mensaje;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CrearEditarPlantilla(Guid idPlantilla)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api"].ToString();

                var model = new Plantilla();
                if (idPlantilla == Guid.Empty)
                {
                    model.Titulo = "Crear plantilla sucursal";
                    model.IdEmisorSucursal = (Guid)Session["i_id_sucursal"];
                    model.IdUsuarioCreacion = (Guid)Session["i_id_usuario"];
                }
                else
                {

                    var datos = _clsConfiguracion.CargarDatosPlantilla(idPlantilla);

                    string[] logoList = datos.Logo.Split('~');

                    logoList[1] = logoList[1].Replace('\\', '/');

                    model.Titulo = "Editar plantilla sucursal";
                    model.IdEmisorSucursalPlantilla = datos.IdEmisorSucursalPlantilla;
                    model.EmisorSucursal = datos.EmisorSucursal;
                    model.IdEmisorSucursal = datos.IdEmisorSucursal;
                    model.PrimerMensaje = datos.PrimerMensaje;
                    model.SegundoMensaje = datos.SegundoMensaje;
                    model.TercerMensaje = datos.TercerMensaje;
                    model.IdUsuarioCreacion = datos.IdUsuarioCreacion;
                    model.FechaCreacion = datos.FechaCreacion;
                    model.Image = "~" + logoList[1];

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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EnviarFomularioPlantilla(Plantilla plantilla)
        {
            try
            {
                if (string.IsNullOrEmpty(plantilla.PrimerMensaje) || ((Request.Files.Count == 0 || Request.Files[0].ContentLength == 0) && plantilla.IdEmisorSucursalPlantilla == Guid.Empty))
                {
                    string mensaje = "No se pudo Guardar Revisar campos";
                    string icon = "error";

                    TempData["sMensajePla"] = $"mensaje('{mensaje}','{icon}')";
                }
                else
                {
                    string path = @"~/Content/Images/logosemisores/";
                    byte[] bytes = null;
                    HttpPostedFileBase fileup = null;
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        fileup = Request.Files[0];
                        using (var memory = new MemoryStream())
                        {
                            fileup.InputStream.CopyTo(memory);
                            bytes = memory.ToArray();
                        }
                    }

                    var nombreImagen = _clsEmisor.NombreSucusarEmisor(plantilla.IdEmisorSucursal);


                    //char[] fileArray = plantilla.Logo.ToCharArray();
                    //string fileName = String.Empty;

                    //if (plantilla.Logo.Contains('/'))
                    //{
                    //    for (int i = fileArray.Count() - 1; fileArray[i] != '/'; i--)
                    //    {
                    //        fileName = fileArray[i].ToString() + fileName;
                    //    }
                    //}
                    //else
                    //{
                    //    for (int i = fileArray.Count() - 1; fileArray[i] != '\\'; i--)
                    //    {
                    //        fileName = fileArray[i].ToString() + fileName;
                    //    }
                    //}

                    if (plantilla.IdEmisorSucursalPlantilla == Guid.Empty)
                    {

                        var model = new DatosPlantilla
                        {
                            IdEmisorSucursalPlantilla = Guid.NewGuid(),
                            IdEmisorSucursal = plantilla.IdEmisorSucursal,
                            Logo = path + nombreImagen + Path.GetExtension(fileup.FileName),
                            PrimerMensaje = plantilla.PrimerMensaje,
                            SegundoMensaje = plantilla.SegundoMensaje,
                            TercerMensaje = plantilla.TercerMensaje,
                            IdUsuarioCreacion = plantilla.IdUsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            EsuEmisor = plantilla.EsuEmisor,
                            sLogo = Convert.ToBase64String(bytes)
                        };

                        var result = _clsConfiguracion.GuardarNuevoPlantilla(model);

                        string mensaje = (result) ? "Se guardo satisfactoriamente" : "No se pudo guardar";
                        string icon = (result) ? "success" : "error";

                        TempData["sMensajePla"] = $"mensaje('{mensaje}','{icon}')";

                        var envio = (new
                        {
                            Ruta = model.Logo,
                            Logo = model.sLogo
                        });

                        ConsumirApi("RutaImagen", JsonConvert.SerializeObject(envio));
                    }
                    else
                    {

                        var model = new DatosPlantilla
                        {
                            IdEmisorSucursalPlantilla = plantilla.IdEmisorSucursalPlantilla,
                            IdEmisorSucursal = plantilla.IdEmisorSucursal,
                            Logo = path + nombreImagen + Path.GetExtension(fileup.FileName),
                            PrimerMensaje = plantilla.PrimerMensaje,
                            SegundoMensaje = plantilla.SegundoMensaje,
                            TercerMensaje = plantilla.TercerMensaje,
                            IdUsuarioCreacion = plantilla.IdUsuarioCreacion,
                            FechaCreacion = plantilla.FechaCreacion,
                            sLogo = Convert.ToBase64String(bytes)
                        };

                        bool imagen = true;
                        if (Request.Files[0].ContentLength == 0)
                        {
                            imagen = false;
                        }

                        var result = _clsConfiguracion.ActualizarDatosPlantilla(model, imagen);

                        string mensaje = (result) ? "Se edito satisfactoriamente" : "No se pudo editar";
                        string icon = (result) ? "success" : "error";

                        TempData["sMensajePla"] = $"mensaje('{mensaje}','{icon}')";

                        if (imagen)
                        {
                            var envio = (new
                            {
                                Ruta = model.Logo,
                                Logo = model.sLogo
                            });

                            ConsumirApi("RutaImagen", JsonConvert.SerializeObject(envio));
                        }
                    }
                }

                return RedirectToAction("Plantilla");
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public string ConsumirApi(string metodoApi, string json)
        {
            try
            {

                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_api"].ToString() + metodoApi;

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

        [HttpPost]
        public ActionResult TraerRuta(Guid Plantilla)
        {
            try
            {
                return Json(_clsConfiguracion.TraerRuta(Plantilla));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion
    }
}