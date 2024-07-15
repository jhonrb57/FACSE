using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FasceMVC.Models
{
    public class Buscar
    {
        public string ListaDocumentos { get; set; }
        public string Tipodocumento { get; set; }

        public List<Insertar> ListadoTemporal { get; set; }
        public string CampoBuscar { get; set; }

        public List<AdquirienteReceptor> listaResultados { get; set; }

        public Guid Id { get; set; }

        public string JsFuncion { get; set; }

    }
}