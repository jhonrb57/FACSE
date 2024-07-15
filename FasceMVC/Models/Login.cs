using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FasceMVC.Models
{
    public class Login
    {
        [Required]
        [DisplayName("Usuario")]
        public string Usuario { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}