using DataBase;
using Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class ProductoModel
    {
        public List<DatosProducto> ListadoProducto { get; set; }
        public Guid IdProducto { get; set; }


        public List<SelectListItem> ListadoTipoUnidad { get; set; }
        public Guid IdUnidad { get; set; }


        public Guid CatalogoId { get; set; }
        public Guid CatalogoDescripcion { get; set; }

        public List<SelectListItem> ListadoTipoImpuesto { get; set; }
        public Guid IdImpuesto { get; set; }


        public List<Catalogo> ListadoCatalogo { get; set; }


        //[StringLength(50, ErrorMessage ="El campo solo es de 50 caracteres")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Codigo { get; set; }
        public Guid Emisor { get; set; }

        //[StringLength(9, ErrorMessage = "El campo solo es de 9 caracteres")]
        [Range(1, Double.PositiveInfinity)]
        [Required(ErrorMessage = "Debe de ingresar un numero mayor a cero")]
        public decimal ValorUnitario { get; set; }

        //[System.ComponentModel.DataAnnotations.Compare("Descripcion", ErrorMessage = "The password and confirmation password do not match.")]
        //[StringLength(100, ErrorMessage = "El campo solo es de 100 caracteres")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public string Titulo { get; set; }

        public string JsFuncion { get; set; }
    }
}