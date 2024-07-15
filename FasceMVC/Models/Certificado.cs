using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class Certificado
    {
        public List<DatosCertificado> ListadoCertificados { get; set; }

        public Guid IdCertificado { get; set; }
        [Required]
        public string Archivo { get; set; }
        [Required]
        public string sCertificado { get; set; }
        [Required]
        [NotMapped]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaVigencia { get; set; }
        public bool Estado { get; set; }
        [Required]
        public string Contrasena { get; set; }

        public string JsFuncion { get; set; }
        public string Titulo { get; set; }
    }
}