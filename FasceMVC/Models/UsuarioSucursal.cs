using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class UsuarioSucursal
    {
        public List<DatosUsuarioSucursal> ListAsignada { get; set; }
        public List<DatosUsuarioSucursal> ListDisponible { get; set; }

        public Guid IdUsuario { get; set; }
        public string JsFuncion { get; set; }
    }
}