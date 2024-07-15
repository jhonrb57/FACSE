using DataBase;
using FasceMVC.Code;
using log4net;
using Metodos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace FasceMVC.Controllers
{
    [VerifyUser]
    public class DistribuidorInicioController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DistribuidorInicioController).Name);
        private readonly ClsTipo _clsTipo = new ClsTipo();
        private readonly ClsEmisor _clsEmisor = new ClsEmisor();
        private readonly ClsConfiguracion _clsConfiguracion = new ClsConfiguracion();

        public static List<adm_grupo_emisor> listadoGrupoController;
        public static List<sys_tipo_persona> listadoTipoPersonaController;
        public static List<sys_tipo_identificacion> listadoTipoIdentificacionController;
        public static List<sys_pais> listadoPaisController;
        public static List<sys_departamento> listadoDepartamentoController;
        public static List<sys_municipio> listadoMunicipioController;
        public static List<sys_software> listadoSoftwareController;
        public static List<sys_ciiu> listadoCiiuController;
        public static List<sys_tipo_emisor> listadoTipoEmisorController;
        
        // GET: Distribuidor
        public ActionResult Index(string mensaje = null)
        {
            try
            {
                var model = new adm_emisor();
                if (mensaje != null)
                {
                    if (TempData["sMensajeEmisor"] is string smensaje)
                    {
                        model.JsFuncion = smensaje;
                    }
                }

                return View(model);

            }
            catch (Exception ex)
            {
                var model = new adm_emisor();
                log.Error($"Error al consultar distribuidor: {ex}");
                ModelState.AddModelError("", "Error al consultar distribuidor. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }

        }

        public JsonResult ListadoGrillaDatatable()
        {
            //int pageIndex = Convert.ToInt32(page) - 1;
            //int pageSize = rows;
            //int startRows = rows * (pageIndex);

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            try
            {

                int count = 0;

                var listadoEmisor = _clsEmisor.ListadoEmisor((Guid)Session["i_id_distribuidor"], skip, pageSize, ref count, sortColumn, sortColumnDir);

                int totalRecords = count;
                //var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                var jsonData = (from e in listadoEmisor
                                select new
                                {
                                    e.emi_id,
                                    e.tid_descripcion,
                                    e.emi_identificacion,
                                    e.emi_nombre,
                                    e.dep_nombre,
                                    e.mun_nombre,
                                    e.emi_correo,
                                    e.emi_telefono,
                                    e.emi_correo_automatico,
                                    emi_fecha_creacion = e.emi_fecha_creacion?.ToString("dd/MM/yyyy hh:mm:ss")                                    
                                }).ToList();

                return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = jsonData });

                //return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor();

                log.Error($"Error al buscar informacion: {ex}");
                ModelState.AddModelError("", "Error al buscar informacion. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(new { isValid = false });
        }

        /// <summary>
        /// creacion emisor
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("CreacionEmisor")]
        [ValidateAntiForgeryToken]
        public ActionResult CreacionEmisor()
        {
            var model = new adm_emisor();
            try
            {
                listadoGrupoController = _clsTipo.ListadoGrupoEmisor((Guid)Session["i_id_distribuidor"]);
                listadoGrupoController.Add(new adm_grupo_emisor { gen_id = Guid.Empty, gen_nombre = "--Selecccione--" });
                ViewBag.emi_grupo = new SelectList(listadoGrupoController, "gen_id", "gen_nombre");
                listadoTipoPersonaController = _clsTipo.ListadoTipoPersona();
                ViewBag.emi_tipo_persona = new SelectList(listadoTipoPersonaController, "tpe_id", "tpe_descripcion");
                listadoTipoIdentificacionController = _clsTipo.ListadoTipoIdentificacion();
                ViewBag.emi_tipo_identificacion = new SelectList(listadoTipoIdentificacionController, "tid_id", "tid_descripcion");
                listadoPaisController = _clsTipo.ListadoPais();
                ViewBag.emi_pais = new SelectList(listadoPaisController, "pai_id", "pai_nombre_comun", listadoPaisController.Any() ? listadoPaisController.FirstOrDefault().pai_id : Guid.Empty);
                listadoDepartamentoController = _clsTipo.ListadoDepartamento(listadoPaisController.Any() ? listadoPaisController.FirstOrDefault().pai_id : Guid.Empty);
                ViewBag.emi_departamento = new SelectList(listadoDepartamentoController, "dep_id", "dep_nombre", listadoDepartamentoController.Any() ? listadoDepartamentoController.FirstOrDefault().dep_id : Guid.Empty);
                listadoMunicipioController = _clsTipo.ListadoMunicipio(listadoDepartamentoController.Any() ? listadoDepartamentoController.FirstOrDefault().dep_id : Guid.Empty);
                ViewBag.emi_municipio = new SelectList(listadoMunicipioController, "mun_id", "mun_nombre");
                listadoSoftwareController = _clsTipo.ListadoSoftware();
                ViewBag.emi_sofware = new SelectList(listadoSoftwareController, "sof_id", "sof_nombre");
                listadoCiiuController = _clsTipo.ListadoCiiu();
                ViewBag.emi_ciiu = new SelectList(listadoCiiuController, "cii_id", "cii_descripcion");
                listadoTipoEmisorController = _clsTipo.ListadoTipoEmisor();
                ViewBag.emi_tipo_emisor = new SelectList(listadoTipoEmisorController, "tem_id", "tem_descripcion");

                //return View(certificadoSaldoCliente);
            }
            catch (Exception ex)
            {
                log.Error($"Error creacion emisor: {ex}");
                ModelState.AddModelError("", "Error creacion emisor. Intente de nuevo, si el problema persiste consulte al administrador.");

            }

            return PartialView("CreacionEmisor", model);
            //return Json(new { isValid = true, data = RenderPartialViewToString(this, "CreacionEmisor", model) });
        }

        /// <summary>
        /// convierte vista en string
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            }
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public ActionResult CargarDepartamento(Guid emi_pais)
        {
            listadoDepartamentoController = _clsTipo.ListadoDepartamento(emi_pais);
            return Json(listadoDepartamentoController, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarMunicipio(Guid emi_departamento)
        {
            listadoMunicipioController = _clsTipo.ListadoMunicipio(emi_departamento);
            return Json(listadoMunicipioController, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarMunicipioSucursal(Guid esu_departamento)
        {
            listadoMunicipioController = _clsTipo.ListadoMunicipio(esu_departamento);
            return Json(listadoMunicipioController, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GuardarEmisor")]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarEmisor(adm_emisor adm_Emisor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    adm_Emisor.emi_id = Guid.NewGuid();
                    adm_Emisor.emi_distribuidor = (Guid)Session["i_id_distribuidor"];
                    _clsEmisor.GuardarEmisor(adm_Emisor, "", "");

                    string mensaje = $"Emisor {adm_Emisor.emi_identificacion} se ha guardado correctamente";
                    string icon = "success";
                    TempData["sMensajeEmisor"] = $"mensaje('{mensaje}','{icon}')";

                    return Json(new { isValid = true });
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            ViewBag.emi_grupo = new SelectList(listadoGrupoController, "gen_id", "gen_nombre", adm_Emisor.emi_grupo);
            ViewBag.emi_tipo_persona = new SelectList(listadoTipoPersonaController, "tpe_id", "tpe_descripcion", adm_Emisor.emi_tipo_persona);
            ViewBag.emi_tipo_identificacion = new SelectList(listadoTipoIdentificacionController, "tid_id", "tid_descripcion", adm_Emisor.emi_tipo_identificacion);
            ViewBag.emi_pais = new SelectList(listadoPaisController, "pai_id", "pai_nombre_comun", adm_Emisor.emi_pais);
            ViewBag.emi_departamento = new SelectList(listadoDepartamentoController, "dep_id", "dep_nombre", adm_Emisor.emi_departamento);
            ViewBag.emi_municipio = new SelectList(listadoMunicipioController, "mun_id", "mun_nombre", adm_Emisor.emi_municipio);
            ViewBag.emi_sofware = new SelectList(listadoSoftwareController, "sof_id", "sof_nombre", adm_Emisor.emi_sofware);
            ViewBag.emi_ciiu = new SelectList(listadoCiiuController, "cii_id", "cii_descripcion", adm_Emisor.emi_ciiu);
            ViewBag.emi_tipo_emisor = new SelectList(listadoTipoEmisorController, "tem_id", "tem_descripcion", adm_Emisor.emi_tipo_emisor);

            return Json(new { isValid = false, data = RenderPartialViewToString(this, "CreacionEmisor", adm_Emisor) });
        }

        [HttpPost, ActionName("EditarEmisor")]
        public ActionResult EditarEmisor(Guid emi_id)
        {
            adm_emisor emisor = _clsEmisor.CargarInformacionEditar(emi_id);

            listadoGrupoController = _clsTipo.ListadoGrupoEmisor((Guid)Session["i_id_distribuidor"]);
            listadoGrupoController.Add(new adm_grupo_emisor { gen_id = Guid.Empty, gen_nombre = "--Selecccione--" });
            ViewBag.emi_grupo = new SelectList(listadoGrupoController, "gen_id", "gen_nombre", emisor.emi_grupo);
            listadoTipoPersonaController = _clsTipo.ListadoTipoPersona();
            ViewBag.emi_tipo_persona = new SelectList(listadoTipoPersonaController, "tpe_id", "tpe_descripcion", emisor.emi_tipo_persona);
            listadoTipoIdentificacionController = _clsTipo.ListadoTipoIdentificacion();
            ViewBag.emi_tipo_identificacion = new SelectList(listadoTipoIdentificacionController, "tid_id", "tid_descripcion", emisor.emi_tipo_identificacion);
            listadoPaisController = _clsTipo.ListadoPais();
            ViewBag.emi_pais = new SelectList(listadoPaisController, "pai_id", "pai_nombre_comun", emisor.emi_pais);
            listadoDepartamentoController = _clsTipo.ListadoDepartamento(emisor.emi_pais ?? Guid.Empty);
            ViewBag.emi_departamento = new SelectList(listadoDepartamentoController, "dep_id", "dep_nombre", emisor.emi_departamento);
            listadoMunicipioController = _clsTipo.ListadoMunicipio(emisor.emi_departamento ?? Guid.Empty);
            ViewBag.emi_municipio = new SelectList(listadoMunicipioController, "mun_id", "mun_nombre", emisor.emi_municipio);
            listadoSoftwareController = _clsTipo.ListadoSoftware();
            ViewBag.emi_sofware = new SelectList(listadoSoftwareController, "sof_id", "sof_nombre", emisor.emi_sofware);
            listadoCiiuController = _clsTipo.ListadoCiiu();
            ViewBag.emi_ciiu = new SelectList(listadoCiiuController, "cii_id", "cii_descripcion", emisor.emi_ciiu);
            listadoTipoEmisorController = _clsTipo.ListadoTipoEmisor();
            ViewBag.emi_tipo_emisor = new SelectList(listadoTipoEmisorController, "tem_id", "tem_descripcion", emisor.emi_tipo_emisor);

            return Json(RenderPartialViewToString(this, "EditarEmisor", emisor));
        }

        [HttpPost, ActionName("ActualizarEmisor")]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEmisor(adm_emisor adm_Emisor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _clsEmisor.ActualizarEmisor(adm_Emisor, "", "");

                    string mensaje = $"Emisor {adm_Emisor.emi_identificacion} se ha guardado correctamente";
                    string icon = "success";
                    TempData["sMensajeEmisor"] = $"mensaje('{mensaje}','{icon}')";

                    return Json(new { isValid = true });
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            ViewBag.emi_grupo = new SelectList(listadoGrupoController, "gen_id", "gen_nombre", adm_Emisor.emi_grupo);
            ViewBag.emi_tipo_persona = new SelectList(listadoTipoPersonaController, "tpe_id", "tpe_descripcion", adm_Emisor.emi_tipo_persona);
            ViewBag.emi_tipo_identificacion = new SelectList(listadoTipoIdentificacionController, "tid_id", "tid_descripcion", adm_Emisor.emi_tipo_identificacion);
            ViewBag.emi_pais = new SelectList(listadoPaisController, "pai_id", "pai_nombre_comun", adm_Emisor.emi_pais);
            ViewBag.emi_departamento = new SelectList(listadoDepartamentoController, "dep_id", "dep_nombre", adm_Emisor.emi_departamento);
            ViewBag.emi_municipio = new SelectList(listadoMunicipioController, "mun_id", "mun_nombre", adm_Emisor.emi_municipio);
            ViewBag.emi_sofware = new SelectList(listadoSoftwareController, "sof_id", "sof_nombre", adm_Emisor.emi_sofware);
            ViewBag.emi_ciiu = new SelectList(listadoCiiuController, "cii_id", "cii_descripcion", adm_Emisor.emi_ciiu);
            ViewBag.emi_tipo_emisor = new SelectList(listadoTipoEmisorController, "tem_id", "tem_descripcion", adm_Emisor.emi_tipo_emisor);

            return Json(new { isValid = false, data = RenderPartialViewToString(this, "EditarEmisor", adm_Emisor) });
        }

        public static List<adm_emisor_catalogo> ListadoEmisorCatalogoController;

        [HttpPost, ActionName("Catalogo")]
        [ValidateAntiForgeryToken]
        public ActionResult Catalogo(adm_emisor emisor)
        {
            try
            {
                ListadoEmisorCatalogoController = _clsEmisor.ListadoEmisorCatalogo(emisor.emi_id);

                var model = new ListadoEmisor
                {
                    Emisor = emisor.emi_id,
                    ListadoEmisorCatalogo = ListadoEmisorCatalogoController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_catalogo();
                log.Error($"Error al consultar catalogo: {ex}");
                ModelState.AddModelError("", "Error al consultar catalogo. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("AdicionarCatalogo")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCatalogo(ListadoEmisor emisor_catalogo)
        {
            try
            {
                var model = new adm_emisor_catalogo
                {
                    eca_emisor = emisor_catalogo.Emisor,
                };

                var tipoCatalogo = _clsTipo.ListadoTipoCatalogo();
                ViewBag.eca_tipo_catalogo = new SelectList(tipoCatalogo, "tca_id", "tca_nombre");
                var tipoDato = _clsTipo.ListadoTipoDato();
                ViewBag.eca_tipo_dato = new SelectList(tipoDato, "tda_id", "tda_descripcion");

                return PartialView("CatalogoNuevo", model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_catalogo();
                log.Error($"Error al consultar catalogo: {ex}");
                ModelState.AddModelError("", "Error al consultar catalogo. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        [HttpPost, ActionName("EliminarEmisorCatalogo")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisorCatalogo(ListadoEmisor emisor_Catalogo)
        {

            try
            {
                ListadoEmisorCatalogoController = _clsEmisor.EliminarEmisorCatalogo(emisor_Catalogo.Id, emisor_Catalogo.Emisor);
                emisor_Catalogo.ListadoEmisorCatalogo = ListadoEmisorCatalogoController;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            return Json(RenderPartialViewToString(this, "Catalogo", emisor_Catalogo));
        }

        [HttpPost, ActionName("CrearCatalogo")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCatalogo(adm_emisor_catalogo emisor_Catalogo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoEmisorCatalogoController = _clsEmisor.GuardarEmisorCatalogo(emisor_Catalogo, "", "");

                    var model = new ListadoEmisor
                    {
                        ListadoEmisorCatalogo = ListadoEmisorCatalogoController,
                        Emisor = emisor_Catalogo.eca_emisor
                    };

                    string mensaje = $"Catalogo {emisor_Catalogo.eca_nombre} se ha guardado correctamente";
                    string icon = "success";
                    model.JsFuncion = $"mensaje('{mensaje}','{icon}')";
                    return Json(RenderPartialViewToString(this, "Catalogo", emisor_Catalogo));
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            var tipoCatalogo = _clsTipo.ListadoTipoCatalogo();
            ViewBag.eca_tipo_catalogo = new SelectList(tipoCatalogo, "tca_id", "tca_nombre", emisor_Catalogo.eca_tipo_catalogo);
            var tipoDato = _clsTipo.ListadoTipoDato();
            ViewBag.eca_tipo_dato = new SelectList(tipoDato, "tda_id", "tda_descripcion", emisor_Catalogo.eca_tipo_dato);

            return Json(RenderPartialViewToString(this, "CatalogoNuevo", emisor_Catalogo));
        }

        public static List<adm_emisor_catalogo_lista> ListadoCatalogoListaController;

        [HttpPost, ActionName("CatalogoLista")]
        [ValidateAntiForgeryToken]
        public ActionResult CatalogoLista(ListadoEmisor emisor_Catalogo)
        {
            try
            {
                ListadoCatalogoListaController = _clsEmisor.ListadoEmisorCatalogoLista(emisor_Catalogo.Id);

                var model = new adm_emisor_catalogo_lista
                {
                    ecl_emisor_catalogo = emisor_Catalogo.Id,
                    ListadoEmisorCatalogo = ListadoCatalogoListaController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_catalogo();
                log.Error($"Error al consultar catalogo: {ex}");
                ModelState.AddModelError("", "Error al consultar catalogo. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("CrearCatalogoLista")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCatalogoLista(adm_emisor_catalogo_lista emisor_CatalogoLista)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emisor_CatalogoLista.ecl_descripcion))
                {
                    ModelState.AddModelError("ecl_descripcion", "Debe ingresar la descripcion");
                    emisor_CatalogoLista.ListadoEmisorCatalogo = ListadoCatalogoListaController;
                }
                else
                {
                    ListadoCatalogoListaController = _clsEmisor.GuardarEmisorCatalogoLista(emisor_CatalogoLista, "", "");
                    emisor_CatalogoLista.ListadoEmisorCatalogo = ListadoCatalogoListaController;
                }

            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(RenderPartialViewToString(this, "CatalogoLista", emisor_CatalogoLista));
        }

        [HttpPost, ActionName("EliminarEmisorCatalogoLista")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisorCatalogoLista(adm_emisor_catalogo_lista emisor_CatalogoLista)
        {
            try
            {
                ListadoCatalogoListaController = _clsEmisor.EliminarEmisorCatalogoLista(emisor_CatalogoLista.ecl_id, emisor_CatalogoLista.ecl_emisor_catalogo);
                emisor_CatalogoLista.ListadoEmisorCatalogo = ListadoCatalogoListaController;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            return Json(RenderPartialViewToString(this, "CatalogoLista", emisor_CatalogoLista));
        }

        public static List<adm_emisor_certificado> ListadoEmisorCertificadoController;

        [HttpPost, ActionName("Certificado")]
        [ValidateAntiForgeryToken]
        public ActionResult Certificado(adm_emisor emisor)
        {
            try
            {
                ListadoEmisorCertificadoController = _clsEmisor.ListadoEmisorCertificado(emisor.emi_id);

                var model = new ListadoEmisor
                {
                    Emisor = emisor.emi_id,
                    ListadoEmisorCertificado = ListadoEmisorCertificadoController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_certificado();
                log.Error($"Error al consultar certificado: {ex}");
                ModelState.AddModelError("", "Error al consultar certificado. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("AdicionarCertificado")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCertificado(ListadoEmisor emisor_certificado)
        {
            try
            {
                var model = new adm_emisor_certificado
                {
                    ece_emisor = emisor_certificado.Emisor,
                    ece_fecha_vegencia = DateTime.Now
                };

                return PartialView("CertificadoNuevo", model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_certificado();
                log.Error($"Error al consultar certificado: {ex}");
                ModelState.AddModelError("", "Error al consultar certificado. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        [HttpPost, ActionName("EliminarEmisorCertificado")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisorCertificado(ListadoEmisor emisor_Certificado)
        {
            try
            {
                ListadoEmisorCertificadoController = _clsEmisor.EliminarEmisorCertificado(emisor_Certificado.Id, emisor_Certificado.Emisor);
                emisor_Certificado.ListadoEmisorCertificado = ListadoEmisorCertificadoController;

            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            return Json(RenderPartialViewToString(this, "Certificado", emisor_Certificado));
        }

        [HttpPost, ActionName("CrearCertificado")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCertificado(adm_emisor_certificado emisor_Certificado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoEmisorCertificadoController = _clsEmisor.GuardarEmisorCertificado(emisor_Certificado, "", "");
                    var model = new ListadoEmisor
                    {
                        ListadoEmisorCertificado = ListadoEmisorCertificadoController,
                        Emisor = emisor_Certificado.ece_emisor
                    };
                    string mensaje = $"Certificado {emisor_Certificado.ece_certificado} se ha guardado correctamente";
                    string icon = "success";
                    model.JsFuncion = $"mensaje('{mensaje}','{icon}')";

                    return Json(RenderPartialViewToString(this, "Certificado", emisor_Certificado));
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            return Json(RenderPartialViewToString(this, "CertificadoNuevo", emisor_Certificado));

        }

        public static List<adm_emisor_notificacion> ListadoEmisorNotificacionController;

        [HttpPost, ActionName("Notificacion")]
        [ValidateAntiForgeryToken]
        public ActionResult Notificacion(adm_emisor emisor)
        {
            try
            {
                ListadoEmisorNotificacionController = _clsEmisor.ListadoEmisorNotificacion(emisor.emi_id);

                var model = new ListadoEmisor
                {
                    Emisor = emisor.emi_id,
                    ListadoEmisorNotificacion = ListadoEmisorNotificacionController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_notificacion();
                log.Error($"Error al consultar notificacion: {ex}");
                ModelState.AddModelError("", "Error al consultar notificacion. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("AdicionarNotificacion")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarNotificacion(ListadoEmisor emisor_notificacion)
        {
            try
            {
                var model = new adm_emisor_notificacion
                {
                    eno_id_emisor = emisor_notificacion.Emisor,
                    eno_fecha = DateTime.Now
                };

                var tipoNotificacion = _clsTipo.ListadoTipoNotificacion();
                ViewBag.eno_id_tipo_notificacion = new SelectList(tipoNotificacion, "tno_id", "tno_descripcion");

                return PartialView("NotificacionNuevo", model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_notificacion();
                log.Error($"Error al consultar notificacion: {ex}");
                ModelState.AddModelError("", "Error al consultar notificacion. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        [HttpPost, ActionName("EliminarEmisorNotificacion")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisorNotificacion(ListadoEmisor emisor_notificacion)
        {
            try
            {
                ListadoEmisorNotificacionController = _clsEmisor.EliminarEmisorNotificacion(emisor_notificacion.Id, emisor_notificacion.Emisor);
                emisor_notificacion.ListadoEmisorNotificacion = ListadoEmisorNotificacionController;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(RenderPartialViewToString(this, "Notificacion", emisor_notificacion));
        }

        [HttpPost, ActionName("CrearNotificacion")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNotificacion(adm_emisor_notificacion emisor_notificacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoEmisorNotificacionController = _clsEmisor.GuardarEmisorNotificacion(emisor_notificacion, "", "");

                    var model = new ListadoEmisor
                    {
                        Emisor = emisor_notificacion.eno_id_emisor,
                        ListadoEmisorNotificacion = ListadoEmisorNotificacionController
                    };

                    return Json(RenderPartialViewToString(this, "Notificacion", model));
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            var tipoNotificacion = _clsTipo.ListadoTipoNotificacion();
            ViewBag.eno_id_tipo_notificacion = new SelectList(tipoNotificacion, "tno_id", "tno_descripcion", emisor_notificacion.eno_id_tipo_notificacion);

            return Json(RenderPartialViewToString(this, "NotificacionNuevo", emisor_notificacion));

        }

        public static List<adm_emisor_correo> ListadoEmisorCorreoController;

        [HttpPost, ActionName("Correo")]
        [ValidateAntiForgeryToken]
        public ActionResult Correo(adm_emisor emisor)
        {
            try
            {
                ListadoEmisorCorreoController = _clsEmisor.ListadoEmisorCorreo(emisor.emi_id);

                var model = new ListadoEmisor
                {
                    Emisor = emisor.emi_id,
                    ListadoEmisorCorreo = ListadoEmisorCorreoController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_correo();
                log.Error($"Error al consultar correo: {ex}");
                ModelState.AddModelError("", "Error al consultar correo. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("AdicionarCorreo")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCorreo(ListadoEmisor emisor_correo)
        {
            try
            {
                var model = new adm_emisor_correo
                {
                    eco_emisor = emisor_correo.Emisor
                };

                var tipoCorreo = _clsTipo.ListadoTipoCorreo();
                ViewBag.eco_tipo_correo = new SelectList(tipoCorreo, "tco_id", "tco_descripcion");

                return PartialView("CorreoNuevo", model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_notificacion();
                log.Error($"Error al consultar correo: {ex}");
                ModelState.AddModelError("", "Error al consultar correo. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        [HttpPost, ActionName("EliminarEmisoCorreo")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisoCorreo(ListadoEmisor emisor_correo)
        {
            try
            {
                ListadoEmisorCorreoController = _clsEmisor.EliminarEmisorCorreo(emisor_correo.Id, emisor_correo.Emisor);
                emisor_correo.ListadoEmisorCorreo = ListadoEmisorCorreoController;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            return Json(RenderPartialViewToString(this, "Correo", emisor_correo));
        }

        [HttpPost, ActionName("CrearCorreo")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCorreo(adm_emisor_correo emisor_correo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoEmisorCorreoController = _clsEmisor.GuardarEmisorCorreo(emisor_correo, "", "");
                    var model = new ListadoEmisor
                    {
                        Emisor = emisor_correo.eco_emisor,
                        ListadoEmisorCorreo = ListadoEmisorCorreoController
                    };

                    string mensaje = $"Correo {emisor_correo.eco_nombre} se ha guardado correctamente";
                    string icon = "success";
                    model.JsFuncion = $"mensaje('{mensaje}','{icon}')";

                    return Json(RenderPartialViewToString(this, "Correo", model));
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            var tipoCorreo = _clsTipo.ListadoTipoCorreo();
            ViewBag.eco_tipo_correo = new SelectList(tipoCorreo, "tco_id", "tco_descripcion", emisor_correo.eco_tipo_correo);

            return Json(RenderPartialViewToString(this, "CorreoNuevo", emisor_correo));

        }

        public static List<adm_emisor_resolucion> ListadoEmisorResolucionController;

        [HttpPost, ActionName("Resolucion")]
        [ValidateAntiForgeryToken]
        public ActionResult Resolucion(adm_emisor emisor)
        {
            try
            {
                ListadoEmisorResolucionController = _clsEmisor.ListadoEmisorResolucion(emisor.emi_id);

                var model = new ListadoEmisor
                {
                    Emisor = emisor.emi_id,
                    ListadoEmisorResolucion = ListadoEmisorResolucionController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_resolucion();
                log.Error($"Error al consultar resolucion: {ex}");
                ModelState.AddModelError("", "Error al consultar resolucion. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("AdicionarResolucion")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarResolucion(ListadoEmisor emisor_Resolucion)
        {
            try
            {
                var model = new adm_emisor_resolucion
                {
                    ere_emisor = emisor_Resolucion.Emisor,
                    ere_fecha = DateTime.Now,
                    ere_fecha_final = DateTime.Now,
                    ere_fecha_inicio = DateTime.Now
                };

                var tipoDocumento = _clsTipo.ListadoTipocomprobante();
                ViewBag.ere_tipo_documento = new SelectList(tipoDocumento, "tdo_id", "tdo_descripcion");

                return PartialView("ResolucionNuevo", model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_notificacion();
                log.Error($"Error al consultar resolucion: {ex}");
                ModelState.AddModelError("", "Error al consultar resolucion. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        [HttpPost, ActionName("EliminarEmisoResolucion")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisoResolucion(ListadoEmisor emisor_resolucion)
        {
            try
            {
                ListadoEmisorResolucionController = _clsEmisor.EliminarEmisorResolucion(emisor_resolucion.Id, emisor_resolucion.Emisor);
                emisor_resolucion.ListadoEmisorResolucion = ListadoEmisorResolucionController;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            return Json(RenderPartialViewToString(this, "Resolucion", emisor_resolucion));
        }

        [HttpPost, ActionName("CrearResolucion")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearResolucion(adm_emisor_resolucion emisor_resolucion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoEmisorResolucionController = _clsEmisor.GuardarEmisorResolucion(emisor_resolucion, "", "");
                    var model = new ListadoEmisor
                    {
                        Emisor = emisor_resolucion.ere_emisor,
                        ListadoEmisorResolucion = ListadoEmisorResolucionController
                    };

                    string mensaje = $"Resolucion {emisor_resolucion.ere_numero_resolucion} se ha guardado correctamente";
                    string icon = "success";
                    model.JsFuncion = $"mensaje('{mensaje}','{icon}')";

                    return Json(RenderPartialViewToString(this, "Resolucion", model));

                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }
            var tipoDocumento = _clsTipo.ListadoTipocomprobante();
            ViewBag.ere_tipo_documento = new SelectList(tipoDocumento, "tdo_id", "tdo_descripcion", emisor_resolucion.ere_tipo_documento);

            return Json(RenderPartialViewToString(this, "ResolucionNuevo", emisor_resolucion));
        }

        public static List<adm_emisor_sucursal> ListadoEmisorSucursalController;

        [HttpPost, ActionName("Sucursal")]
        [ValidateAntiForgeryToken]
        public ActionResult Sucursal(adm_emisor emisor)
        {
            try
            {
                ListadoEmisorSucursalController = _clsEmisor.ListadoEmisorSucursal(emisor.emi_id);

                var model = new ListadoEmisor
                {
                    Pais = emisor.emi_pais ?? Guid.Empty,
                    Emisor = emisor.emi_id,
                    ListadoEmisorSucursal = ListadoEmisorSucursalController
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_sucursal();
                log.Error($"Error al consultar sucursal: {ex}");
                ModelState.AddModelError("", "Error al consultar sucursal. Intente de nuevo, si el problema persiste consulte al administrador.");
                return PartialView(model);
            }
        }

        [HttpPost, ActionName("AdicionarSucursal")]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarSucursal(ListadoEmisor emisor_Sucursal)
        {
            try
            {
                var model = new adm_emisor_sucursal
                {
                    esu_emisor = emisor_Sucursal.Emisor
                };

                listadoDepartamentoController = _clsTipo.ListadoDepartamento(emisor_Sucursal.Pais);
                ViewBag.esu_departamento = new SelectList(listadoDepartamentoController, "dep_id", "dep_nombre", listadoDepartamentoController.Any() ? listadoDepartamentoController.FirstOrDefault().dep_id : Guid.Empty);

                var municipio = _clsTipo.ListadoMunicipio(listadoDepartamentoController.Any() ? listadoDepartamentoController.FirstOrDefault().dep_id : Guid.Empty);
                ViewBag.esu_municipio = new SelectList(municipio, "mun_id", "mun_nombre");
                ViewBag.ListadoCorreoEntrada = _clsConfiguracion.CargarListadoCorreo(emisor_Sucursal.Emisor, Guid.Parse("F394C319-E5D4-41D2-975C-F6879BC1ADE2"));
                ViewBag.ListadoCorreoSalida = _clsConfiguracion.CargarListadoCorreo(emisor_Sucursal.Emisor, Guid.Parse("D9B983B0-DE11-4E26-89EF-5046EF8A006B"));

                return PartialView("SucursalNuevo", model);
            }
            catch (Exception ex)
            {
                var model = new adm_emisor_sucursal();
                log.Error($"Error al consultar sucursal: {ex}");
                ModelState.AddModelError("", "Error al consultar sucursal. Intente de nuevo, si el problema persiste consulte al administrador.");
                return View(model);
            }
        }

        [HttpPost, ActionName("EliminarEmisoSucursal")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEmisoSucursal(ListadoEmisor emisor_sucursal)
        {
            try
            {
                ListadoEmisorSucursalController = _clsEmisor.EliminarEmisorSucursal(emisor_sucursal.Id, emisor_sucursal.Emisor);
                emisor_sucursal.ListadoEmisorSucursal = ListadoEmisorSucursalController;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            return Json(RenderPartialViewToString(this, "Sucursal", emisor_sucursal));
        }

        [HttpPost, ActionName("CrearSucursal")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearSucursal(adm_emisor_sucursal emisor_sucursal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoEmisorSucursalController = _clsEmisor.GuardarEmisorSucursal(emisor_sucursal, "", "");
                    var model = new ListadoEmisor
                    {
                        Emisor = emisor_sucursal.esu_emisor,
                        ListadoEmisorSucursal = ListadoEmisorSucursalController
                    };

                    string mensaje = $"Sucursal {emisor_sucursal.esu_nombre} se ha guardado correctamente";
                    string icon = "success";
                    model.JsFuncion = $"mensaje('{mensaje}','{icon}')";
                    return Json(RenderPartialViewToString(this, "Sucursal", model));

                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Error al salvar datos. Intente de nuevo, si el problema persiste consulte al administrador.");
            }

            ViewBag.esu_departamento = new SelectList(listadoDepartamentoController, "dep_id", "dep_nombre", emisor_sucursal.esu_departamento);

            var municipio = _clsTipo.ListadoMunicipio(emisor_sucursal.esu_departamento ?? Guid.Empty);
            ViewBag.esu_municipio = new SelectList(municipio, "mun_id", "mun_nombre", emisor_sucursal.esu_municipio);

            return Json(RenderPartialViewToString(this, "SucursalNuevo", emisor_sucursal));
        }
    }
}