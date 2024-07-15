using FasceMVC.App_Start;
using FasceMVC.Models;
using log4net;
using Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    public class AdquirienteController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdquirienteController).Name);
        private readonly ClsAdquiriente _clsAdquiriente = new ClsAdquiriente();
        // GET: Adquiriente
        public ActionResult CrearAdquiriente(Guid? idReceptor)
        {
            try
            {
                var model = new Adquiriente() { };
                if (idReceptor != null && idReceptor != Guid.Empty)
                {
                    var result = _clsAdquiriente.CargarDatosReceptor(idReceptor);

                    model.CodigoPostal = result.CodigoPostal;
                    model.Digito = string.IsNullOrWhiteSpace(result.Digito) ? 0 : int.Parse(result.Digito);
                    model.Direccion = result.Direccion;
                    model.Email = result.Email;
                    model.IdReceptor = result.IdReceptor;
                    model.MatriculaMercantil = result.MatriculaMercantil;
                    model.Municipio = result.IdMunicipio;
                    model.Departamento = result.IdDepartamento;
                    model.Pais = result.IdPais;
                    model.RazonSocial = result.RazonSocial;
                    model.Telefono = result.Telefono;
                    model.TipoAdquiriente = result.TipoAdquiriente;
                    model.TipoIdentificacion = result.TipoIdentificacion;
                    model.TipoPersona = result.TipoPersona;
                    model.NumeroIdentificacion = result.NumeroIdentificacion;
                    model.Nombre = result.Nombre;
                    model.ListaPais = _clsAdquiriente.ListadoPaises();
                    model.ListaDepartamento = _clsAdquiriente.ListadoDptos(result.IdPais);
                    model.ListaMunicipio = (model.ListaDepartamento.Count > 0) ? _clsAdquiriente.ListadoMunicipios(result.IdDepartamento) : new List<SelectListItem> { };
                    model.ResponsabilidadFiscal = _clsAdquiriente.ResponsabilidadFiscal();

                    var listaResponsabilidad = model.TipoAdquiriente.Split(';');
                    List<string> responsabilidad = listaResponsabilidad.OfType<string>().ToList();
                    model.ResponsabilidadFiscal.ForEach(item => item.Selected = responsabilidad.Contains(item.Text.Trim()));
                }
                else
                {
                    model.ListaPais = _clsAdquiriente.ListadoPaises();
                    model.ListaDepartamento = _clsAdquiriente.ListadoDptos(Guid.Parse(model.ListaPais[0].Value));
                    model.ListaMunicipio = (model.ListaDepartamento.Count > 1) ? _clsAdquiriente.ListadoMunicipios(Guid.Parse(model.ListaDepartamento[0].Value)) : new List<SelectListItem> { new SelectListItem { Value = null, Text = "No aplica" } };
                    model.ResponsabilidadFiscal = _clsAdquiriente.ResponsabilidadFiscal();
                }


                model.ListaTipoIdentificacion = _clsAdquiriente.ListadOTipoIdentificacion();
                model.ListaTipoPersona = _clsAdquiriente.ListadoTipoPersona();



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
                if (dpto != null)
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

        public ActionResult GuardarAdquiriente(Adquiriente adquiriente)
        {
            try
            {

                var emisor = (Guid)Session["i_id_emisor"];


                if (adquiriente.IdReceptor != Guid.Empty)
                {

                    var receptor = new DataBase.adm_receptor
                    {
                        rec_id = adquiriente.IdReceptor,
                        rec_codigo_postal = adquiriente.CodigoPostal ?? "",
                        rec_correo = adquiriente.Email,
                        rec_departamento = adquiriente.Departamento == Guid.Empty ? null : adquiriente.Departamento,
                        rec_digito = adquiriente.Digito.ToString(),
                        rec_direccion = adquiriente.Direccion,
                        rec_emisor = emisor,
                        rec_identificacion = adquiriente.NumeroIdentificacion,
                        rec_municipio = adquiriente.Municipio == Guid.Empty ? null : adquiriente.Municipio,
                        rec_nombre = adquiriente.Nombre,
                        rec_pais = adquiriente.Pais,
                        rec_razon_social = adquiriente.RazonSocial,
                        rec_telefono = adquiriente.Telefono,
                        rec_tipo_identificacion = adquiriente.TipoIdentificacion,
                        rec_tipo_persona = adquiriente.TipoPersona,
                        rec_tipo_receptor = adquiriente.TipoAdquiriente,
                        rec_matricula_mercantil = adquiriente.MatriculaMercantil
                    };

                    if (adquiriente.Municipio == Guid.Empty)
                    {
                        receptor.rec_municipio = null;
                    }

                    if (adquiriente.Departamento == Guid.Empty)
                    {
                        receptor.rec_departamento = null;
                    }

                    var result = _clsAdquiriente.ActualizarReceptor(receptor);

                    _clsAdquiriente.GuardarLogReceptor(receptor, Guid.Parse("2B5F05A5-2983-4CE0-885F-0FF0900324E0"), emisor, receptor.rec_id);

                    string mensaje = (result) ? "Se actualizo correctamente!" : "No se ha podido actualizar";
                    string icon = (result) ? "success" : "error";

                    TempData["tempCrearReceptor"] = $"mensaje('{mensaje}','{icon}')";


                    return RedirectToAction("Index", "ConsultarClientes", null);
                }
                else
                {
                    var receptor = new DataBase.adm_receptor
                    {
                        rec_codigo_postal = adquiriente.CodigoPostal ?? "",
                        rec_correo = adquiriente.Email,
                        rec_departamento = adquiriente.Departamento == Guid.Empty ? null : adquiriente.Departamento,
                        rec_digito = adquiriente.Digito.ToString(),
                        rec_direccion = adquiriente.Direccion,
                        rec_emisor = emisor,
                        rec_fecha_receccion = DateTime.Now,
                        rec_identificacion = adquiriente.NumeroIdentificacion,
                        rec_municipio = adquiriente.Municipio == Guid.Empty ? null : adquiriente.Municipio,
                        rec_nombre = adquiriente.Nombre,
                        rec_pais = adquiriente.Pais,
                        rec_razon_social = adquiriente.RazonSocial,
                        rec_telefono = adquiriente.Telefono,
                        rec_tipo_identificacion = adquiriente.TipoIdentificacion,
                        rec_tipo_persona = adquiriente.TipoPersona,
                        rec_tipo_receptor = adquiriente.TipoAdquiriente,
                        rec_matricula_mercantil = adquiriente.MatriculaMercantil
                    };

                    if (adquiriente.Municipio == Guid.Empty)
                    {
                        receptor.rec_municipio = null;
                    }

                    if (adquiriente.Departamento == Guid.Empty)
                    {
                        receptor.rec_departamento = null;
                    }

                    var result = _clsAdquiriente.GuardarAdquiriente(receptor);
                    _clsAdquiriente.GuardarLogReceptor(receptor, Guid.Parse("6F0FAA6D-F1BD-4183-92E1-EE61C695095B"), emisor, result.rec_id);
                    var model = new FacturacionManual
                    {
                        IdAdquiriente = result.rec_id,
                        NitBuscar = result.rec_identificacion,
                        Fecha = DateTime.Now,
                        FechaVencimiento = DateTime.Now.AddDays(30)
                    };
                    string mensaje = (result != null) ? "Se creo correctamente el receptor" : "No se ha podido crear el receptor";
                    string icon = (result != null) ? "success" : "error";

                    TempData["CrearReceptor"] = $"mensaje('{mensaje}','{icon}')";

                    return RedirectToAction("Buscar", "FacturacionManual", model);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult ValidarReceptor(string identificacion)
        {
            try
            {
                var emisor = (Guid)Session["i_id_emisor"];

                var result = _clsAdquiriente.ValidarReceptor(identificacion, emisor);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}