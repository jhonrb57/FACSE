using System;
using System.Collections.Generic;



namespace Report.Models
{
    public class DocumentoModel
    {
        public DocumentoModel()
        {

            Comprobante = new Comprobante();
            Emisor = new Emisor();
            Receptor = new Receptor();
            Detalles = new List<Detalles>();
            Totales = new Totales();
            TotalImpuestos = new List<TotalImpuestos>();
            DetallesComprobante = new List<DetallesComprobante>();
            DescripcionDetalles = new List<DescripcionDetalles>();
            Descripcion = new Descripcion();
            Credenciales = new Credenciales();
            AllowanceCharge = new List<AllowanceCharge>();
            PaymentExchangeRate = null;
            TerminosPago = new TerminosPago();
            MetodoPago = new List<MetodoPago>();
        }
        public Comprobante Comprobante { get; set; }
        public Emisor Emisor { get; set; }
        public Receptor Receptor { get; set; }
        public List<Detalles> Detalles { get; set; }
        public Totales Totales { get; set; }
        public List<TotalImpuestos> TotalImpuestos { get; set; }
        public List<DetallesComprobante> DetallesComprobante { get; set; }
        public List<DescripcionDetalles> DescripcionDetalles { get; set; }
        public Descripcion Descripcion { get; set; }
        public Credenciales Credenciales { get; set; }
        public List<AllowanceCharge> AllowanceCharge { get; set; }
        public PaymentExchangeRate PaymentExchangeRate { get; set; }
        public TerminosPago TerminosPago { get; set; }
        public byte[] QR { get; set; }
        public List<MetodoPago> MetodoPago { get; set; }
    }



    public class Comprobante
    {
        public Comprobante()
        {
            OrigenDocumento = null;
            TipoComprobante = null;
            Fecha = null;
            Prefijo = null;
            Numero = null;
            Moneda = null;
            Referencia = null;
            ConceptoRef = null;
            Observaciones = null;
            Usuario = null;
            Descripcion = new List<Descripcion>();
            MetodoPago = new List<MetodoPago>();
            NumeroOrden = null;
            NumeroDespacho = null;
            NumeroRecepcion = null;
            DocumentoAdicionalNotaCredito = null;
            DocumentoReferenciaCodigo = null;
        }
        public string OrigenDocumento { get; set; }
        public string TipoComprobante { get; set; }
        public string Fecha { get; set; }
        public string Prefijo { get; set; }
        public string Numero { get; set; }
        public string Moneda { get; set; }
        public string Referencia { get; set; }
        public string ConceptoRef { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }
        public List<Descripcion> Descripcion { get; set; }
        public List<MetodoPago> MetodoPago { get; set; }
        public string NumeroOrden { get; set; }
        public string NumeroDespacho { get; set; }
        public string NumeroRecepcion { get; set; }
        public string DocumentoAdicionalNotaCredito { get; set; }
        public string DocumentoReferenciaCodigo { get; set; }
    }


    public class Descripcion
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
    public class DetallesComprobante
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string idDetalle { get; set; }
    }

    public class DescripcionDetalles
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string idDetalle { get; set; }
    }

    public class MetodoPago
    {
        public string FormaPago { get; set; }
        public string MedioPago { get; set; }
        public string Fecha { get; set; }
    }

    public class AllowanceCharge
    {
        public string ID { get; set; }
        public bool ChargeIndicator { get; set; }
        public string AllowanceChargeReasonCode { get; set; }
        public string AllowanceChargeReason { get; set; }
        public decimal MultiplierFactorNumeric { get; set; }
        public decimal Amount { get; set; }
        public decimal BaseAmount { get; set; }
    }

    public class PaymentExchangeRate
    {
        public string SourceCurrencyCode { get; set; }
        public decimal SourceCurrencyBaseRate { get; set; }
        public string TargetCurrencyCode { get; set; }
        public decimal TargetCurrencyBaseRate { get; set; }
        public decimal CalculationRate { get; set; }
        public string Date { get; set; }
    }

    public class Emisor
    {
        public Emisor()
        {
            Identificacion = null;
            DigitoVerificador = null;
            TipoPersona = null;
            TipoIdentificacion = null;
            TipoEmisor = null;
            RazonSocial = null;
            NombreComercial = null;
            Sucursal = null;
            Direccion = null;
            Telefono = null;
            email = null;
            Pais = null;
            PaisCodigo = null;
            Departamento = null;
            DepartamentoCodigo = null;
            Ciudad = null;
            CiudadCodigo = null;
            CodigoPostal = null;
            Descripcion = new List<Descripcion>();
            NumeroMatriculaMercantil = null;
        }
        public string Identificacion { get; set; }
        public string DigitoVerificador { get; set; }
        public string TipoPersona { get; set; }
        public string TipoIdentificacion { get; set; }
        public string TipoEmisor { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string Sucursal { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string email { get; set; }
        public string Pais { get; set; }
        public string PaisCodigo { get; set; }
        public string Departamento { get; set; }
        public string DepartamentoCodigo { get; set; }
        public string Ciudad { get; set; }
        public string CiudadCodigo { get; set; }
        public string CodigoPostal { get; set; }
        public List<Descripcion> Descripcion { get; set; }
        public string NumeroMatriculaMercantil { get; set; }
    }
    public class Receptor
    {
        public Receptor()
        {
            Identificacion = null;
            DigitoVerificador = null;
            TipoPersona = null;
            TipoIdentificacion = null;
            TipoReceptor = null;
            RazonSocial = null;
            NombreComercial = null;
            Direccion = null;
            Telefono = null;
            email = null;
            Pais = null;
            PaisCodigo = null;
            Departamento = null;
            DepartamentoCodigo = null;
            Ciudad = null;
            CiudadCodigo = null;
            CodigoPostal = null;
            Descripcion = new List<Descripcion>();
            NumeroMatriculaMercantil = null;
        }
        public string Identificacion { get; set; }
        public string DigitoVerificador { get; set; }
        public string TipoPersona { get; set; }
        public string TipoIdentificacion { get; set; }
        public string TipoReceptor { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string email { get; set; }
        public string Pais { get; set; }
        public string PaisCodigo { get; set; }
        public string Departamento { get; set; }
        public string DepartamentoCodigo { get; set; }
        public string Ciudad { get; set; }
        public string CiudadCodigo { get; set; }
        public string CodigoPostal { get; set; }
        public List<Descripcion> Descripcion { get; set; }
        public string NumeroMatriculaMercantil { get; set; }
    }
    public class Detalles
    {
        public Detalles()
        {
            idDetalle = null;
            Nombre = null;
            UnidadCodigo = null;
            Cantidad = null;
            ValorUnitario = null;
            SubTotal = null;
            Total = null;
            codigo = null;
            Impuestos = new List<Impuestos>();
            Descripcion = null;
            AllowanceCharge = new AllowanceCharge();
            PricingReference = null;
            Descuento = null;
            Cargos = null;
            AplicaImpuesto = true;
            Fecha = null;
            ValorDebito = null;
            ValorCredito = null;
            Base_Impuesto = 0;
            Codigo_Impuesto = null;
            Nombre_Impuesto = null;
            Porcentaje_Impuesto = 0;
            Valor_Impuesto = 0;


        }
        public string idDetalle { get; set; }
        public string Nombre { get; set; }
        public string UnidadCodigo { get; set; }
        public string Cantidad { get; set; }
        public string ValorUnitario { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public string codigo { get; set; }
        public List<Impuestos> Impuestos { get; set; }
            public string Descripcion { get; set; }
        public AllowanceCharge AllowanceCharge { get; set; }
        public string PricingReference { get; set; }
        public string Descuento { get; set; }
        public string Cargos { get; set; }
        public bool AplicaImpuesto { get; set; }

        public string Fecha { get; set; }
        public string ValorDebito { get; set; }
        public string ValorCredito { get; set; }


        public bool NotRG { get; set; }


        public Decimal Base_Impuesto { get; set; }
        public string Codigo_Impuesto { get; set; }
        public string Nombre_Impuesto { get; set; }
        public Decimal Porcentaje_Impuesto { get; set; }
        public Decimal Valor_Impuesto { get; set; }

    }
    public class Impuestos
    {
        public Impuestos()
        {
            Base = null;
            CodigoImpuesto = null;
            Nombre = null;
            Porcentaje = null;
            Impuesto = null;
        }
        public string Base { get; set; }
        public string CodigoImpuesto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Porcentaje { get; set; }
        public string Impuesto { get; set; }
    }
    public class TotalImpuestos
    {
        public TotalImpuestos()
        {
            Base = null;
            CodigoImpuesto = null;
            Nombre = null;
            Porcentaje = null;
            Impuesto = null;
            Descripcion = null;
            PorcentajeRetencion = 0;
        }
        public string Base { get; set; }
        public string CodigoImpuesto { get; set; }
        public string Nombre { get; set; }
        public string Porcentaje { get; set; }
        public string Impuesto { get; set; }
        public string Descripcion { get; set; }
        public decimal PorcentajeRetencion { get; set; }
    }
    public class Totales
    {
        public Totales()
        {
            Total = null;
            TotalEnLetras = null;
            SubTotal = null;
            Cargos = null;
            Descuentos = null;
            SubTotalSinCargosDescuentos = null;
            IVA = null;
            TotalCantidad = null;
            TotalConRetencion = null;
        }
        public string Total { get; set; }
        public string TotalEnLetras { get; set; }
        public string SubTotal { get; set; }
        public string Cargos { get; set; }
        public string Descuentos { get; set; }
        public string SubTotalSinCargosDescuentos { get; set; }
        public string IVA { get; set; }
        public string TotalCantidad { get; set; }
        public string TotalConRetencion { get; set; }
    }
    public class Credenciales
    {
        public Credenciales()
        {
            ClientToken = null;
            AccessToken = null;
        }
        public string ClientToken { get; set; }
        public string AccessToken { get; set; }
    }
    public class TerminosPago
    {
        public TerminosPago()
        {
            Codigo = null;
            UnidadCodigo = null;
            Duracion = null;
        }
        public string Codigo { get; set; }
        public string UnidadCodigo { get; set; }
        public string Duracion { get; set; }
    }

    public class EmisorRecepcion
    {
        public EmisorRecepcion()
        {
            NumeroResolucion = null;
            NumeroInicio = null;
            NumeroFinal = null;
            FechaInicial = DateTime.Now;
            FechaFinal = DateTime.Now;
            Cufe = null;
            FirmaDigital = null;
            ImagenFondo = null;
        }
        public string NumeroResolucion { get; set; }
        public string NumeroInicio { get; set; }
        public string NumeroFinal { get; set; }
 public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }
        public string Cufe { get; set; }
        public string FirmaDigital { get; set; }
        public byte[] ImagenFondo { get; set; }
    }
}