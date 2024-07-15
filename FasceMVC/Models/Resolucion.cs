using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class Resolucion
    {
        public List<DatosResolucion> ListadoResolucion { get; set; }
        public List<SelectListItem> ListadoTipoDocumento { get; set; }

        public Guid Id { get; set; }
        [Required]
        [MaxLength(5)]
        public string Prefijo { get; set; }
        [Required]
        public Guid TipoDocumento { get; set; }
        [Required]
        public string NumeroResolucion { get; set; }
        public DateTime Fecha { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode =true)]
        public int NumeroInicial { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public int NumeroFinal { get; set; }
        [Required]
        public string ClaveTecnica { get; set; }
        [Required]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }
        public bool Estado { get; set; }

        public string JsFuncion { get; set; }
        public string Titulo { get; set; }

        public List<SelectListItem> ListadoPlantilla { get; set; }
        public Guid? Plantilla { get; set; } 
        public string Ruta { get; set; }
    }
}