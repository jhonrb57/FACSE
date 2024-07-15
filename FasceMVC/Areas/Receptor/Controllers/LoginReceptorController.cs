using DataBase;
using FasceMVC.Models;
using log4net;
using Metodos;
using System;
using System.Web.Mvc;

namespace FasceMVC.Areas.Receptor.Controllers
{
    public class LoginReceptorController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginReceptorController).Name);
        private readonly ClsReceptor _clsReceptor = new ClsReceptor();
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
        [HttpPost, ActionName("LoginReceptor")]
        [ValidateAntiForgeryToken]
        public ActionResult LoginReceptor([Bind(Include = "Usuario, Contrasena")]Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    adm_usuario_receptor usuarioReceptor = _clsReceptor.ValidarReceptor(login.Usuario, login.Contrasena);

                    if (usuarioReceptor != null)
                    {

                        //return RedirectToAction("Index", new { IdCuentaContable = cuentaContable.IdCuentaContable.ToString() });
                        Session["i_id_receptor"] = usuarioReceptor.ure_id;
                        Session["i_snombrereceptor"] = $"{usuarioReceptor.ure_nombre} {usuarioReceptor.ure_apellido}";
                        Session["i_susuario_receptor"] = usuarioReceptor.ure_usuario;

                        log.Info($"Inicio de Sesión el Receptor: {login.Usuario}");

                        return RedirectToAction("Index", "ReceptorInicio");

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
    }
}