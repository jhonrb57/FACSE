using Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasceMVC.Models
{
    public class FacturacionManual
    {
        public int numerofactura { get; set; }
        public Guid IdReceptor { get; set; }
        public string Observaciones { get; set; }
        public string NumeroOrden { get; set; }
        public string NumeroDespacho { get; set; }
        public string NumeroRecepcion { get; set; }
        //public List<SelectListItem>  ListaPrefijo { get; set; }
        public List<Detalle> ListadoDetalles { get; set; }
        public List<Catalogo> ListadoCatalogo { get; set; }
        public List<SelectListItem> ListaContingencia { get; set; }
        public string CodigoContingencia { get; set; }
        public string TipoDocumento { get; set; }

        public decimal Descuento { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]

        public Guid IdTemporal { get; set; }
        //public   Guid Nit { get; set; }
        public List<ImpuestoRetencion> ImpuestosTabla { get; set; }
        public List<ImpuestoRetencion> RetencionesTabla { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Impuesto { get; set; }

        public List<SelectListItem> ListaMoneda { get; set; }
        public Guid Moneda { get; set; }
        public List<SelectListItem> ListaFormaPago { get; set; }
        public Guid FormaPago { get; set; }
        public List<SelectListItem> ListaMedioPago { get; set; }
        public Guid MedioPago { get; set; }
        public List<SelectListItem> ListaImpuestos { get; set; }
        public Guid IdImpuesto { get; set; }
        public List<SelectListItem> ListaRetenciones { get; set; }
        public Guid IdRetencion { get; set; }
        public string NitBuscar { get; set; }
        public Guid IdAdquiriente { get; set; }
        public string Prefijo { get; set; }
        public string ResFiscal { get; set; }

        public decimal Trm { get; set; }
        public string OrdenCompra { get; set; }
        public string Despacho { get; set; }
        public string Recepcion { get; set; }

        //receptor
        public string RazonSocial { get; set; }
        public string Identificacion { get; set; }
        public int Digito { get; set; }
        public string Correo { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        [NotMapped]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }
        [NotMapped]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaVencimiento { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaTrm { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal SubTotalGeneral { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal ImpuestoGeneral { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal RetencionesGeneral { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal DescuentoGeneral { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal Cargo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal TotalGeneral { get; set; }

        public string Observacion { get; set; }

        public Guid IdEmisor { get; set; }
        public decimal TotalRetencion { get; set; }

        public string DescripcionRetencion { get; set; }
        public decimal PorcentajeRetencion { get; set; }

        public decimal TotalFinalRetencion { get; set; }
        public List<SelectListItem> ListadoTipoRetenciones { get; set; }
        public Guid IdTipoRetencion { get; set; }
        public class Detalle
        {
            public Guid Impuesto { get; set; }
            public decimal Porcentaje { get; set; }
            public string CodigoImpuesto { get; set; }

            public string Descripcion { get; set; }
            public int Id { get; set; }
            public Guid IdProducto { get; set; }
            public string Codigo { get; set; }
            public string DescripcionDetalles { get; set; }
            public Guid Unidad { get; set; }
            public Guid IdImpuesto { get; set; }
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
            public decimal SubTotal { get; set; }
            [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
            public decimal Total { get; set; }
            public List<Catalogo> ListaCatalogo { get; set; }
            public string NitBuscar { get; set; }
        }
        public class ImpuestoRetencion
        {
            public Guid Id { get; set; }
            public Guid IdImRe { get; set; }
            public decimal Base { get; set; }
            public string Descripcion { get; set; }
            public decimal Valor { get; set; }
            public string DescripcionRetencion { get; set; }
            public decimal PorcentajeRetencion { get; set; }
            public string CodigoImpuesto { get; set; }
        }

        public class BasicInformation
        {
            public string CodigoContingencia { get; set; }
            public string TipoDocumento { get; set; }
            public Guid IdAdquiriente { get; set; }
            public string Observaciones { get; set; }
            public Guid IdReceptor { get; set; }
            public Guid IdEmisor { get; set; }
            public Guid MedioPago { get; set; }
            public Guid FormaPago { get; set; }
            public Guid Moneda { get; set; }
            public DateTime Fecha { get; set; }
            public DateTime FechaVencimiento { get; set; }
            public decimal NumeroFactura { get; set; }
            public string Prefijo { get; set; }
            public string Observacion { get; set; }
            public string OrdenCompra { get; set; }
            public string Despacho { get; set; }
            public string Recepcion { get; set; }
            public decimal Trm { get; set; }
            public DateTime FechaTrm { get; set; }
        }
    }
}