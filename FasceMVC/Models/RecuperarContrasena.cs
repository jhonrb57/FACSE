using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class RecuperarContrasena
    {
        [Required]
        public string Usuario { get; set; }
        public string Email { get; set; }

        public string JsFuncion { get; set; }
    }
}