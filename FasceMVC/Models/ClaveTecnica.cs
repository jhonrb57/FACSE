using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class ClaveTecnica
    {

        public List<DetalleClaveTecnica> ListadoClaveTecnica { get; set; }
        public Guid Emisor { get; set; }

        public class DetalleClaveTecnica
        {
            public string Prefijo { get; set; }
            public string NumeroInicial { get; set; }
            public string NumeroFinal { get; set; }
            public string FechaResolucion { get; set; }
            public string NumeroResolucion { get; set; }
            public string ClaveTecnica { get; set; }
            public string FechaInicial { get; set; }
            public string FechaFinal { get; set; }
        }
    }
}