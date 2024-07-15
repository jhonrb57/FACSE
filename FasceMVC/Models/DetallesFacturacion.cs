using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class DetallesFacturacion
    {

        public List<SelectListItem> ListaUnidad { get; set; }
        public List<SelectListItem> ListaImpuesto { get; set; }
        public List<Metodos.Catalogo> ListadoCatalogo { get; set; }

        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public string DescripcionDetalle { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public decimal Cantidad { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal ValorUnidad { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal PorcentajeImpuesto { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal ValorImpuesto { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal Descuento { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal Cargo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal SubTotal { get; set; }
        public Guid IdImpuesto { get; set; }
        public Guid Unidad { get; set; }

        public string NitBuscar { get; set; }
        public Guid IdEmisor { get; set; }
        public Guid IdAdquiriente { get; set; }
        [NotMapped]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TempFecha { get; set; }
        [NotMapped]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TempFechaVencimiento { get; set; }
        public Guid TempMoneda { get; set; }
        public Guid TempFormaPago { get; set; }
        public Guid TempMedioPago { get; set; }

        
    }
}