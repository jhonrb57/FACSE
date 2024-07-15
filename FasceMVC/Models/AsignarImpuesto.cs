using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class AsignarImpuesto
    {
        public List<SelectListItem> ImpuestoDisponible { get; set; }
        public List<SelectListItem> ImpuestoAsignado { get; set; }        
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string JsFuncion { get; set; }
    }
}