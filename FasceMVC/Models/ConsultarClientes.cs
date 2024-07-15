using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class ConsultarClientes
    {
        public List<DatosReceptor> ListadoReceptor { get; set; }
        public string Buscar { get; set; }
        public string JsFuncion { get; set; }
    }
}