using DataBase;
using FasceMVC.Models;
using log4net;
using Metodos;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    public class LoginController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginController).Name);
        private readonly ClsUsuario _clsUsuario = new ClsUsuario();


        public ActionResult Inicio()
        {            
            return View();
        }
        /// <summary>
        /// pantalla de login
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string id = null)
        {
            if (id != null)
                ViewBag.Mensaje = id;
            Session.Clear();
            return View();
        }

        /// <summary>
        /// ingresa el usuario la session
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]

        public ActionResult ValidarUsuario([Bind(Include = "Usuario, Contrasena")]Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    adm_usuario usuario = _clsUsuario.ValidarUsuario(login.Usuario, login.Contrasena);

                    if (usuario != null)
                    {
                        if (usuario.adm_usuario_perfil_tipo.Any(u => u.upt_activo == true))
                        {
                            Session["i_id_usuario"] = usuario.usu_id;
                            Session["i_snombreusuario"] = $"{usuario.adm_emisor_sucursal?.adm_emisor.emi_nombre}";
                            Session["i_susuario"] = usuario.usu_usuario;
                            Session["i_id_perfil"] = usuario.adm_usuario_perfil_tipo.FirstOrDefault().upt_perfil;
                            Session["i_sabreviatura"] = usuario.adm_emisor_sucursal?.esu_abreviatura;
                            log.Info($"Inicio de Sesión: {login.Usuario}");

                            if (login.Usuario.Trim() != login.Contrasena.Trim())
                            {
                                if (login.Contrasena == ConfigurationManager.AppSettings["clave_super"].ToString())
                                {
                                    Session["clave_super"] = login.Contrasena;
                                    _clsUsuario.LogIngreso(login.Usuario + "-FACSE");
                                }
                                else
                                    _clsUsuario.LogIngreso(login.Usuario);
                                return RedirectToAction("Index", "Validacion");
                            }
                            else
                            {
                                return RedirectToAction("CambioContrasena", "Login", new { usuario = login.Usuario });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "No tiene asociado ningun tipo de usuario");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Usuario o contraseña incorrecta");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
            }
            return View(login);
        }

        public ActionResult Cerrar()
        {
            Session.Clear();
            return View("Index");
        }

        public ActionResult VolverAinicio()
        {

            return RedirectToAction("Index", "Validacion");

        }


        public ActionResult CambioContrasena(string usuario)
        {
            try
            {
                if (Session["i_id_usuario"] != null && Session["i_snombreusuario"] != null && Session["i_susuario"] != null)
                {
                    var model = new CambioContrasena
                    {

                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult CambioContrasenaGeneral()
        {
            try
            {
                if (TempData["sMensajeCambio"] is string mensaje)
                {
                    ViewBag.JsFuncion2 = mensaje;
                    TempData["sMensajeCambio"] = null;
                }

                if (Session["i_id_usuario"] != null && Session["i_snombreusuario"] != null && Session["i_susuario"] != null)
                {
                    var model = new CambioContrasena
                    {
                        Usuario = Session["i_snombreusuario"].ToString()
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult GuardarNuevaContrasena(CambioContrasena cambioContrasena)
        {
            try
            {

                if (cambioContrasena.ConfirmarContrasena != cambioContrasena.AnteriorContrasena)
                {
                    if (_clsUsuario.ValidarContrasena((Guid)Session["i_id_usuario"], cambioContrasena.AnteriorContrasena))
                    {
                        _clsUsuario.CambiarContrasenaUsuario((Guid)Session["i_id_usuario"], cambioContrasena.ConfirmarContrasena);

                        return RedirectToAction("Index", "Validacion");

                    }
                    else
                    {
                        cambioContrasena.JsFuncion = "La contraseña anterior no coinciden";
                        return View("CambioContrasena", cambioContrasena);
                    }
                }
                else
                {
                    cambioContrasena.JsFuncion = "Contraseña nueva no puede ser igual a la anterior";
                    return View("CambioContrasena", cambioContrasena);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult GuardarNuevaContrasenaGeneral(CambioContrasena cambioContrasena)
        {
            try
            {

                if (cambioContrasena.ConfirmarContrasena != cambioContrasena.AnteriorContrasena)
                {
                    if (_clsUsuario.ValidarContrasena((Guid)Session["i_id_usuario"], cambioContrasena.AnteriorContrasena))
                    {
                        _clsUsuario.CambiarContrasenaUsuario((Guid)Session["i_id_usuario"], cambioContrasena.ConfirmarContrasena);

                        string mensaje = $"Cambio Contraseña se ha cambiado correctamente";
                        string icon = "success";
                        TempData["sMensajeCambio"] = $"mensaje('{mensaje}','{icon}')";

                        return RedirectToAction("CambioContrasenaGeneral", "Login");
                    }
                    else
                    {
                        cambioContrasena.JsFuncion = "La contraseña anterior no coinciden";
                        return View("CambioContrasenaGeneral", cambioContrasena);
                    }
                }
                else
                {
                    cambioContrasena.JsFuncion = "Contraseña nueva no puede ser igual a la anterior";
                    return View("CambioContrasenaGeneral", cambioContrasena);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult Recordar()
        {
            try
            {

                var model = new RecuperarContrasena
                {

                };
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult Recuperar(RecuperarContrasena recuperar)
        {
            try
            {
                var user = _clsUsuario.ConsultarCorreoUsuario(recuperar.Usuario.Trim());

                if (user != null)
                {
                    var mensaje = $"Correo sera enviado a {user.usu_email.Substring(1, user.usu_email.IndexOf('@'))}xxxxxx.com";
                    recuperar.JsFuncion = $"mensaje('{mensaje}','{recuperar.Usuario}')";

                }
                return View("Recordar", recuperar);
            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EnviarCorreoRecuperacion(string usuario)
        {
            try
            {
                var result = _clsUsuario.EnvioCorreoRecuperarContrasenaUsuario(usuario);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult Envio()
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                //var client = new SendGridClient("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");
                var from = new EmailAddress("wobando@firsoft-si.com", "Wobando");
                var subject = "OPrueba";
                var to = new EmailAddress("williames641@gmail.com", "Example User");
                var plainTextContent = $"Buena tarde Sobre la novedad de la factura 4331 en diferencia de un peso entre opera y facse. Al realizar el calculo del subtotal de la factura, sacandole el iva correspondiente nos da un peso mayor a opera, ya que opera puede redondear ese valor,  osotros no lo podemos hacer por que la dian nos rechazaria la factura por valores";
                var htmlContent = "<strong>Buena tarde Sobre la novedad de la factura 4331 en diferencia de un peso entre opera y facse. Al realizar el calculo del subtotal de la factura, sacandole el iva correspondiente nos da un peso mayor a opera, ya que opera puede redondear ese valor,  osotros no lo podemos hacer por que la dian nos rechazaria la factura por valores/strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                //var response = client.SendEmailAsync(msg).ConfigureAwait(false);



                return View("Index");
            }
            catch (Exception ex)
            {

                log.Error($"Error al validar usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
            //            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            //var client = new SendGridClient("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");
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("wobando@firsoft-si.com", "DX Team"),
            //    Subject = "Hello World from the SendGrid CSharp SDK!",
            //    PlainTextContent = "Hello, Email!",
            //    HtmlContent = "<strong>Hello, Email!</strong>"
            //};
            //msg.AddTo(new EmailAddress("williames641@gmail.com", "Test User"));
            //var response = client.SendEmailAsync(msg);


        
        }
    }
}