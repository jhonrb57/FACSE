using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class CambioContrasena
    {
        [Required]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public string AnteriorContrasena { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(100, ErrorMessage = "La contraseña debe ser de minimo {2} caracteres.", MinimumLength = 6)]
        public string NuevaContrasena { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Compare("NuevaContrasena", ErrorMessage = "Contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; }

        public string JsFuncion { get; set; }
    }
}