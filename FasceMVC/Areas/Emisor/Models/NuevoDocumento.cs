using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FasceMVC.Area.Emisor.Models
{
    public class NuevoDocumento
    {
        [StringLength(50)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [DisplayName("Identifición")]
        public string Identificacion { get; set; }
        public int IdAdquiriente { get; set; }

        [DisplayName("Prefijo")]
        public string Prefijo { get; set; }

        [DisplayName("Fecha Recepción")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaRecepcion { get; set; }
        public List<adm_receptor> ListadoReceptor { get; set; }
    }
}