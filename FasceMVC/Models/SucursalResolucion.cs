using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class SucursalResolucion
    {

        public List<DatosSucursalResolucion> Disponibles { get; set; }
        public List<DatosSucursalResolucion> Asignados { get; set; }
        public Guid IdSucursal { get; set; }

        public string JsFuncion { get; set; }
    }
}