using Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class NotaCredito
    {
        public List<SelectListItem> ListadoConceptos { get; set; }
        public List<Detalle> ListadoDetalles { get; set; }
        public List<Catalogo> ListadoCatalogoGeneral { get; set; }
        public string ResFiscal { get; set; }

        [Required]
        public string Prefijo { get; set; }
        [Required]
        public decimal NumeroFactura { get; set; }
        public Guid Concepto { get; set; }
        public string Moneda { get; set; }
        public string FormaPago { get; set; }
        public string MedioPago { get; set; }
        public string OrdenCompra { get; set; }
        public string Despacho { get; set; }
        public string Recepcion { get; set; }
        public decimal Trm { get; set; }
        public string RazonSocial { get; set; }
        public string Identificacion { get; set; }
        public int Digito { get; set; }
        public string Correo { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaTrm { get; set; }
        public string Referencia { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Cargo { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Descuento { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Impuesto { get; set; }
        public List<SelectListItem> ListaRetenciones { get; set; }
        public Guid IdRetencion { get; set; }

        public string Observacion { get; set; }
        public string JsFuncion { get; set; }


        public List<SelectListItem> ListadoMoneda { get; set; }
        public List<SelectListItem> ListadoFormaPago { get; set; }
        public List<SelectListItem> ListadoMedioPago { get; set; }
        public Guid gMoneda { get; set; }
        public Guid gFormaPago { get; set; }
        public Guid gMedioPago { get; set; }
        public decimal PorcentajeRetencion { get; set; }
        public decimal TotalFinalRetencion { get; set; }
        public List<SelectListItem> ListadoTipoRetenciones { get; set; }
        public Guid IdTipoRetencion { get; set; }
        public decimal TotalRetencion { get; set; }

        public class Detalle
        {
            public int Id { get; set; }
            public Guid IdDetalle { get; set; }
            public decimal Cantidad { get; set; }
            public string Codigo { get; set; }
            public string Descripcion { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal ValorUnidad { get; set; }
            [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
            public decimal Descuento { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal Cargo { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal ValorImpuesto { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal Total { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal SubTotal { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public string CodigoImpuesto { get; set; }


            public decimal Porcentaje { get; set; }

            public List<SelectListItem> ListadoImpuestos { get; set; }
            public List<SelectListItem> ListadoUnidad { get; set; }
            public Guid Impuesto { get; set; }
            public Guid Unidad { get; set; }

            public List<Catalogo> ListadoCatalogo { get; set; }
            public List<Descripcion> ListadoDescripcion { get; set; }
            public bool AplicaImpuesto { get; set; }

        }

        public class DatosFactura
        {
            public decimal NumeroFactura { get; set; }
            public string Prefijo { get; set; }
            public Guid Concepto { get; set; }
            public decimal NumeroNotaCredito { get; set; }
            public string PrefijoNotaCredito { get; set; }
            public string OrdenCompra { get; set; }
            public string Despacho { get; set; }
            public string Recepcion { get; set; }
            public decimal Trm { get; set; }
            public Guid Moneda { get; set; }
            public DateTime FechaTrm { get; set; }
        }

        public class ImpuestoRetencionD
        {
            public int Id { get; set; }
            public Guid IdImRe { get; set; }
            public decimal Base { get; set; }
            public string Descripcion { get; set; }
            public decimal Porcentaje { get; set; }
            public decimal Valor { get; set; }
        }
    }
}