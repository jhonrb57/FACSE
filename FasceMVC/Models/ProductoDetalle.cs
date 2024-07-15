using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class ProductoDetalle
    {

        public List<DatosProductoDetalle> ListadoDetallesProducto { get; set; }

        public string Descripcion { get; set; }
        public Guid IdProducto { get; set; }
    }
}
