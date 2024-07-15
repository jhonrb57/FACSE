using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class Sucursal
    {
        public List<DatosSucursal> ListadoSucursal { get; set; }

        public Guid Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Abreviatura { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        [Required]
        public string Telefono { get; set; }
        public List<SelectListItem> ListadoPais { get; set; }
        public List<SelectListItem> ListadoDepartamento { get; set; }
        public List<SelectListItem> ListadoMunicipio { get; set; }
        [Required]
        public Guid? Pais { get; set; }
        public Guid? Departamento { get; set; }
        public Guid? Municipio { get; set; }

        public List<SelectListItem> ListadoCorreoEntrada { get; set; }
        public List<SelectListItem> ListadoCorreoSalida { get; set; }
        [Required]
        public Guid CorreoEntrada { get; set; }
        [Required]
        public Guid CorreoSalida { get; set; }

        public bool Estado { get; set; }
        public bool HasResolution { get; set; }
        public string Titulo { get; set; }
        public string JsFuncion { get; set; }
    }
}