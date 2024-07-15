using System;
using System.ComponentModel.DataAnnotations;

namespace DataBase
{
    public class InicioReport
    {
        public string Tipo { get; set; }
        public string Prefijo { get; set; }
        public string Numero { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string NitEmisor { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaEmision { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaEnvio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }
        public string Usuario { get; set; }
    }
}
