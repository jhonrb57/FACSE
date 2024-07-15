using DataBase;
using FasceMVC.Code;
using log4net;
using Metodos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    [VerifyUser]
    public class ValidacionController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ValidacionController).Name);

        private readonly ClsUsuario _clsUsuario = new ClsUsuario();
        private readonly ClsEmisor _clsEmisor = new ClsEmisor();
        private readonly ClsSucursal _clsSucursal = new ClsSucursal();
        private readonly ClsDistribuidor _clsDistribuidor = new ClsDistribuidor();
        private readonly ClsNotificaciones _clsnotificaciones = new ClsNotificaciones();

        public static List<adm_usuario_perfil_tipo> ListadoUsuarioPerfilController;
        /// <summary>
        /// pantalla con los botones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string tipoNotificacion = "")
        {
            try
            {

                var usuarioPeril = _clsUsuario.ListadoUsuarioPerfil((Guid)Session["i_id_usuario"]);
                ListadoUsuarioPerfilController = usuarioPeril;
                var model = new adm_usuario_perfil_tipo
                {
                    ListadoUsuarioPerfil = ListadoUsuarioPerfilController
                };
                //adm_emisor emisor = _clsEmisor.InformacionEmisor(ListadoUsuarioPerfilController[0].upt_persona ?? Guid.Empty);
                ViewBag.ListadoNotificaciones = new List<Notificaciones>();

                if (tipoNotificacion != "" && Guid.Parse(tipoNotificacion) == Guid.Empty)
                {
                    string codigoUsuario = Session["i_susuario"].ToString();
                    var respuesta = ConsumirApi("Api/App/ListadoNotificaciones/" + codigoUsuario);

                    if ((bool)respuesta.Respuesta)
                    {
                        ViewBag.ListadoNotificaciones = JsonConvert.DeserializeObject<List<Notificaciones>>(respuesta.Contenido.ListadoNotificaciones.ToString());
                    }

                }
                else
                {
                    Guid tNot = (tipoNotificacion == "") ? Guid.Empty : Guid.Parse(tipoNotificacion);
                    Guid idSucursal = _clsnotificaciones.CaonsultarUltimaSucursal((Guid)Session["i_id_usuario"]);
                    ViewBag.ListadoNotificaciones = _clsnotificaciones.CargarNotificaciones(idSucursal, tNot);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                var model = new adm_usuario_perfil_tipo
                {
                    ListadoUsuarioPerfil = new List<adm_usuario_perfil_tipo>()
                };
                log.Error($"Error al consultar tipo usuario: {ex}");
                ModelState.AddModelError("", "Error al consultar tipo usuario. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        /// <summary>
        /// accion del boton
        /// </summary>
        /// <param name="adm_Usuario_Perfil_Tipo"></param>
        /// <param name="Emisor"></param>
        /// <param name="Adquiriente"></param>
        /// <param name="Distribuidor"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(adm_usuario_perfil_tipo usuario_Perfil_Tipo, string Emisor, string Adquiriente, string Distribuidor)
        {
            try
            {
                if (Emisor != null)
                {
                    if (_clsEmisor.EmisorExistente(usuario_Perfil_Tipo.upt_persona))
                    {
                        Guid? sucursal = _clsSucursal.ValidarEmisorSucursal((Guid)Session["i_id_usuario"], usuario_Perfil_Tipo.upt_persona);
                        if (sucursal == null)
                        {
                            ModelState.AddModelError("", "No Tiene Sucursales Asociada");
                            usuario_Perfil_Tipo.ListadoUsuarioPerfil = ListadoUsuarioPerfilController;
                            ViewBag.ListadoNotificaciones = _clsnotificaciones.CargarNotificaciones(Guid.Empty, Guid.Empty);
                            return View(usuario_Perfil_Tipo);
                        }
                        else
                        {
                            adm_emisor emisor = _clsEmisor.InformacionEmisor(usuario_Perfil_Tipo.upt_persona ?? Guid.Empty);
                            if (((string)Session["i_susuario"]).Contains(emisor.emi_identificacion))
                            {
                                Session["i_id_emisor"] = usuario_Perfil_Tipo.upt_persona;
                                Session["i_s_nombreEmisor"] = emisor.emi_nombre;
                                Session["i_id_sucursal"] = sucursal;
                                return RedirectToAction("Index", "EmisorInicio");
                            }
                            else
                            {
                                ModelState.AddModelError("", "El usuario no pertenece al emisor");
                                usuario_Perfil_Tipo.ListadoUsuarioPerfil = ListadoUsuarioPerfilController;
                                ViewBag.ListadoNotificaciones = _clsnotificaciones.CargarNotificaciones(Guid.Empty, Guid.Empty);
                                return View(usuario_Perfil_Tipo);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No existe el Emisor Asosiado");
                        usuario_Perfil_Tipo.ListadoUsuarioPerfil = ListadoUsuarioPerfilController;
                        ViewBag.ListadoNotificaciones = _clsnotificaciones.CargarNotificaciones(Guid.Empty, Guid.Empty);
                        return View(usuario_Perfil_Tipo);
                    }
                }
                else if (Adquiriente != null)
                    return RedirectToAction("Index", "AdquirienteInicio");
                else
                {
                    adm_distriduidor distriduidor = _clsDistribuidor.InformacionDistribuidor(usuario_Perfil_Tipo.upt_persona ?? Guid.Empty);

                    if (distriduidor == null)
                    {
                        ModelState.AddModelError("", "No existe el Distribuidor Asosiado");
                        usuario_Perfil_Tipo.ListadoUsuarioPerfil = ListadoUsuarioPerfilController;
                        ViewBag.ListadoNotificaciones = _clsnotificaciones.CargarNotificaciones(Guid.Empty, Guid.Empty);
                        return View(usuario_Perfil_Tipo);
                    }
                    else
                    {
                        Session["i_id_distribuidor"] = usuario_Perfil_Tipo.upt_persona;
                        Session["i_s_nombreDistribuidor"] = distriduidor.dis_nombre;
                        return RedirectToAction("Index", "DistribuidorInicio");
                    }
                }

            }
            catch (Exception ex)
            {
                var model = new adm_usuario_perfil_tipo
                {
                    ListadoUsuarioPerfil = null
                };
                log.Error($"Error al consultar tipo usuario: {ex}");
                ModelState.AddModelError("", "Error al consultar tipo usuario. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }


        public ActionResult CambiarEstadoNotificacion(string id)
        {
            try
            {
                Guid idNotificacion = Guid.Parse(id);

                var resultado = _clsnotificaciones.CambiarEstadoNotificacion(idNotificacion);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambiarTodasNotificacion()
        {
            try
            {
                Guid idSucursal = _clsnotificaciones.CaonsultarUltimaSucursal((Guid)Session["i_id_usuario"]);

                var resultado = _clsnotificaciones.CambiarTodosEstadoNotificacion(idSucursal);

                return Json(resultado, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Actualiza el leído de la notificación, consume el api de ticket
        /// Se usa en la view Validacion/Index
        /// </summary>
        /// <param name="idNotificacion"></param>
        /// <returns></returns>
        public ActionResult CambiarEstadoNotificacionTicket(Guid idNotificacion)
        {
            try
            {
                var respuesta = ConsumirApi("Api/App/ActualizarLeidoNotificacion/" + idNotificacion);

                return Json((bool)respuesta.Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                log.Error($"Error: {err.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public dynamic ConsumirApi(string metodoApi)
        {
            try
            {
                string rutaApiTicket = ConfigurationManager.AppSettings["ruta_api_ticket"].ToString() + metodoApi;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaApiTicket);
                request.Method = "GET";
                request.ContentType = "application/json";

                using (HttpWebResponse respuesta = (HttpWebResponse)request.GetResponse())
                {
                    if (respuesta.StatusCode == HttpStatusCode.Accepted || respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.Created)
                    {
                        using (StreamReader read = new StreamReader(respuesta.GetResponseStream()))
                        {
                            var result = read.ReadToEnd();
                            return JsonConvert.DeserializeObject(result);
                        };
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error al consultar las notificaciones de tickets");
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }
    }
}