using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class Correo
    {
        public List<DatosCorreo> ListadoCorreo { get; set; }

        public List<SelectListItem> ListaTipoCorreo { get; set; }
        public Guid TipoCorreo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Servidor { get; set; }
        [Required]
        public string Puerto { get; set; }
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
        [Required]
        [EmailAddress]
        public string dCorreo { get; set; }
        public bool Ssl { get; set; }
        public string CorreoHtml { get; set; }
        public bool Estado { get; set; }

        public Guid IdCorreo { get; set; }
        public string JsFuncion { get; set; }
        public string Titulo { get; set; }
    }
}