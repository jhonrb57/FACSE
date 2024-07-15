using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class CatalogoModel
    {
        public List<DatosCatalogo> ListadoCatalogo { get; set; }

        public Guid IdCatalogo { get; set; }
        public List<SelectListItem> ListadoTipoDato { get; set; }
        public Guid TipoDato { get; set; }
        public List<SelectListItem> ListadoTipoCatalogo { get; set; }
        public Guid TipoCatalogo { get; set; }
        [Required]
        public string Nombre { get; set; }
        public bool Lista { get; set; }
        public bool Estado { get; set; }

        public string JsFuncion { get; set; }
        public string Titulo { get; set; }
    }
}