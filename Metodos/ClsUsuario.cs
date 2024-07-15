using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Metodos
{
    public class ClsUsuario
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsUsuario).Name);
        private readonly ClsFuncion _clsFuncion = new ClsFuncion();

        /// <summary>
        /// validar si existe el usuario con su contrasena
        /// </summary>
        /// <param name="sUsuario"></param>
        /// <param name="sContrasena"></param>
        /// <returns></returns>
        public adm_usuario ValidarUsuario(string sUsuario, string sContrasena)
        {
            try
            {
                if (sContrasena == ConfigurationManager.AppSettings["clave_super"].ToString())
                {
                    return _dbContext.adm_usuario
                    .Where(x => x.usu_usuario.Equals(sUsuario) && x.usu_activo == true).FirstOrDefault();
                }
                else
                {
                    string sContrasenaEncriptada = _clsFuncion.Encriptar(sContrasena);
                    var usuario = from u in _dbContext.adm_usuario.AsNoTracking()
                                  where u.usu_usuario.Equals(sUsuario) && u.usu_contrasena.Equals(sContrasenaEncriptada) && u.usu_activo == true
                                  select u;

                    return (usuario.FirstOrDefault());
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listado con los usuarios y perfiles y tipos
        /// </summary>
        /// <param name="gUsuario"></param>
        /// <returns></returns>
        public List<adm_usuario_perfil_tipo> ListadoUsuarioPerfil(Guid gUsuario)
        {
            try
            {
                return _dbContext.adm_usuario_perfil_tipo.AsNoTracking().Where(upt => upt.upt_activo == true && upt.upt_usuario == gUsuario).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="nuevaContrasena"></param>
        /// <returns></returns>
        public void CambiarContrasenaUsuario(Guid gUsuario, string nuevaContrasena)
        {
            try
            {
                string newPass = _clsFuncion.Encriptar(nuevaContrasena);

                var user = _dbContext.adm_usuario
                    .Where(x => x.usu_id == gUsuario)
                    .FirstOrDefault();

                user.usu_contrasena = newPass;

                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// validar si la contraseña coinciden
        /// </summary>
        /// <param name="gUsuario"></param>
        /// <param name="contrasenaAnterior"></param>
        /// <returns></returns>
        public bool ValidarContrasena(Guid gUsuario, string contrasenaAnterior)
        {
            try
            {
                string oldPass = _clsFuncion.Encriptar(contrasenaAnterior);

                var user = _dbContext.adm_usuario
                    .Where(x => x.usu_id == gUsuario)
                    .FirstOrDefault();

                if (user.usu_contrasena == oldPass)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EnvioCorreoRecuperarContrasenaUsuario(string usuario)
        {
            try
            {
                var datosUser = _dbContext.adm_usuario.
                    Where(x => x.usu_usuario.Equals(usuario))
                    .FirstOrDefault();

                if (datosUser != null)
                {
                    using (var mensaje = new MailMessage(ConfigurationManager.AppSettings["usuario"].ToString(), datosUser.usu_email))
                    {
                        mensaje.IsBodyHtml = true;
                        mensaje.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                        mensaje.Subject = $"Recuperación contraseña - {datosUser.usu_usuario}";
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
                                              $"<td><b>  Su Usuario y contraseña es el siguiente: <br><hr style= \"   border: 1px solid #CCCCCC; \" ></td>" +
                                                 $"</tr>" +
                                                 $"<tr><td style = \"padding: 20px 0 30px 0; \" > <b>Usuario: {datosUser.usu_usuario} <br> <b>Contraseña:  {_clsFuncion.Descifrar(datosUser.usu_contrasena)} </td>" +
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


                        SmtpClient smtpClient = new SmtpClient
                        {
                            Credentials = new NetworkCredential(ConfigurationManager.AppSettings["usuario"].ToString(), ConfigurationManager.AppSettings["password"].ToString()),
                            Port = int.Parse(ConfigurationManager.AppSettings["puerto"].ToString()),
                            Host = ConfigurationManager.AppSettings["servidor"].ToString(),
                            EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["ssl"].ToString())
                        };

                        smtpClient.Send(mensaje);
                    };
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return false;
            }
        }

        public adm_usuario ConsultarCorreoUsuario(string usuario)
        {
            try
            {
                return _dbContext.adm_usuario.
                    Where(x => x.usu_usuario.Equals(usuario))
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void LogIngreso(string usuario)
        {
            try
            {
                var query = new AuditLogIngreso
                {
                    IdAuditLogIngreso = Guid.NewGuid(),
                    Fecha = DateTime.Now,
                    Usuario = usuario
                };

                _dbContext.AuditLogIngreso.Add(query);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// existe usuario sucursal
        /// </summary>
        /// <param name="gUsuario"></param>
        /// <param name="gSucursal"></param>
        /// <returns></returns>
        public bool ExisteUsuarioSucursal(Guid gSucursal)
        {
            try
            {
                return _dbContext.adm_usuario_sucursal.AsNoTracking()
                    .Any(us => us.usu_sucursal == gSucursal && us.usu_activo == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
