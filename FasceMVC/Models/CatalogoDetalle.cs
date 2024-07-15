using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class CatalogoDetalle
    {
        public List<DatosCatalogoDetalle> ListadoDetallesCatalogo { get; set; }

        public string Descripcion { get; set; }
        public Guid IdCatalogo { get; set; }
    }
}