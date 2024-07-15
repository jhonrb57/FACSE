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
    public class EmisorConfiguracionController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmisorConfiguracionController).Name);
        private readonly ClsEmisor _clsEmisor = new ClsEmisor();
        private readonly ClsAdquiriente _clsAdquiriente = new ClsAdquiriente();


        #region EmisorConfiguracion

        // GET: EmisorConfiguracion
        public ActionResult Index()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];
                var model = new adm_emisor
                {
                    ListadoEmisor = _clsEmisor.ListadoEmisores(emisor)
                };

                if (TempData["sMensajeEmi"] is string mensaje)
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


        public ActionResult EditarEmisor(Guid idEmisor)
        {
            try
            {
                var model = _clsEmisor.CargarInformacionEditar(idEmisor);

                ViewBag.ListadoTipoPersona = _clsAdquiriente.ListadoTipoPersona();
                ViewBag.ListadoTipoEmisor = _clsAdquiriente.ListadoTipoEmisor();
                ViewBag.ListadoTipoIdentificacion = _clsAdquiriente.ListadOTipoIdentificacion();
                ViewBag.ListadoPaises = _clsAdquiriente.ListadoPaises();
                ViewBag.ListadoDepartamentos = _clsAdquiriente.ListadoDptos(model.emi_pais);
                ViewBag.ListadoMunicipios = _clsAdquiriente.ListadoMunicipios(model.emi_departamento);
                ViewBag.ListadoCiiu = _clsAdquiriente.ListadoCiiu();

                model.Titulo = "Editar emisor";
                return View(model);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        //_clsEmisor.ActualizarEmisor(adm_Emisor, User.Identity.Name, Request.UserHostAddress);
        //User.Identity.Name, Request.UserHostAddress

        public ActionResult ActualizarEmisor(adm_emisor emisor)
        {
            try
            {

                _clsEmisor.ActualizarEmisor(emisor, "", "");

                TempData["sMensajeEmi"] = $"mensaje('Emisor editado satisfactoriamente.','success')";

                return RedirectToAction("Index", "Configuracion", null);
            }
            catch (CustomException ex)
            {
                TempData["sMensajeEmi"] = $"mensaje('{ex.Message}','error')";
                return RedirectToAction("Index", "Configuracion", null);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }
        #endregion

        #region ClaveTecnica

        public ActionResult ConsultarClaveTecnica()
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var datos = _clsEmisor.CargarInformacionEditar(emisor);

                var obj = new
                {
                    NitEmisor = datos.emi_identificacion,
                    ClientToken = datos.emi_cliente_token,
                    AccessToken = datos.emi_access_token
                };

                dynamic result = ConsumirApi(obj);

                if (Convert.ToBoolean(result.Respuesta))
                {
                    var listado = new List<ClaveTecnica.DetalleClaveTecnica>();


                    listado = JsonConvert.DeserializeObject<List<ClaveTecnica.DetalleClaveTecnica>>(result.Contenido.ToString());

                    var model = new ClaveTecnica
                    {
                        ListadoClaveTecnica = listado,
                        Emisor = emisor
                    };

                    return View(model);
                }
                else
                {
                    TempData["sMensajeEmi"] = $"mensaje('No cuenta con clave técnica o se presentó un problema al consultar la información','error')";
                    return RedirectToAction("Index", "Configuracion", null);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }

        public dynamic ConsumirApi(Object emisor)
        {
            try
            {
                string rutaApiCompleta = ConfigurationManager.AppSettings["ruta_clave_tecnica"].ToString();

                var json = JsonConvert.SerializeObject(emisor);

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
                            return resultObjeto;

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

        #endregion
    }
}