using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class Adquiriente
    {
        public List<SelectListItem> ResponsabilidadFiscal { get; set; }
        public List<SelectListItem> ListaTipoPersona { get; set; }
        public List<SelectListItem> ListaTipoIdentificacion { get; set; }
        public List<SelectListItem> ListaPais { get; set; }
        public List<SelectListItem> ListaDepartamento { get; set; }
        public List<SelectListItem> ListaMunicipio { get; set; }

        public Guid IdReceptor { get; set; }
        public Guid TipoPersona { get; set; }
        [Required]
        public string TipoAdquiriente { get; set; }
        [Required]
        public Guid TipoIdentificacion { get; set; }
        [Required]
        public string NumeroIdentificacion { get; set; }
        [Required]
        public int Digito { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string RazonSocial { get; set; }
        [Required]
        public Guid Pais { get; set; }
        
        public Guid? Departamento { get; set; }
        
        public Guid? Municipio { get; set; }
        [Required]
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }
        [MaxLength(20)]
        public string MatriculaMercantil { get; set; }
    }
}