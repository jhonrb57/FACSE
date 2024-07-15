using System;
using System.ComponentModel.DataAnnotations;

namespace FasceMVC.Models
{
    public class ViewPdf
    {
        [Required]
        public Guid IdDocumento { get; set; }
        [Required]
        public string PdfBase64 { get; set; }
        public string RutaArchivo { get; set; }
        [Required]
        public string Email { get; set; }
        public bool Estado { get; set; }
        public string Alerta { get; set; }
        public bool EstadoJson { get; set; }
        public string RutaPdf { get; set; }
    }
}