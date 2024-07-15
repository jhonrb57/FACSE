using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class Plantilla
    {
        public List<DatosPlantilla> ListadoPlantilla { get; set; }
        public Guid IdEmisorSucursalPlantilla { get; set; }
        public Guid IdEmisorSucursal { get; set; }
        public Guid IdUsuarioCreacion { get; set; }
        public string EmisorSucursal { get; set; }
        public string Logo { get; set; }
        public string sLogo { get; set; }
        [Required(ErrorMessage = "El primer mensaje es requerido")]
        public string PrimerMensaje { get; set; }
        public string SegundoMensaje { get; set; }
        public string TercerMensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string JsFuncion { get; set; }
        public string Titulo { get; set; }
        public Guid EsuEmisor { get; set; }
        [Required(ErrorMessage = "La imagen es requerido")]
        public string Image { get; set; }
        public Guid IdEmisorSucursalLogueo { get; set; }
    }
}