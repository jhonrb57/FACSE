using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FasceMVC.Models.FacturacionManual;

namespace FasceMVC.Models
{
    public class ResultadoProducto
    {
        public List<DatosProductoListado> ListadoProductosRes { get; set; }
        public List<Detalle> ListadoProductosAgregados { get; set; }



    }
}