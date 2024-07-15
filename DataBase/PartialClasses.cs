using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using static DataBase.IsNotEmptyAttribute;

namespace DataBase
{
    public partial class adm_usuario_perfil_tipo
    {
        [NotMapped]
        public List<adm_usuario_perfil_tipo> ListadoUsuarioPerfil { get; set; }
    }

    [MetadataType(typeof(adm_documentoMetadata))]
    public partial class adm_documento
    {
        [NotMapped]
        [DisplayName("Fecha Inicial")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaInicial { get; set; }

        [NotMapped]
        [DisplayName("Fecha Final")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaFinal { get; set; }

        [NotMapped]
        [DisplayName("Identificación")]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [NotMapped]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [NotMapped]
        [DisplayName("Tipo")]
        public Guid? IdComprobante { get; set; }

        [NotMapped]
        [DisplayName("Estado Documento")]
        public Guid? IdEstado { get; set; }

        [NotMapped]
        [DisplayName("Estado Facse")]
        public Guid? IdEstadoFacse { get; set; }

        [NotMapped]
        public string NombreComprobante { get; set; }

        [NotMapped]
        [DisplayName("Prefijo")]
        public string Prefijo { get; set; }

        [NotMapped]
        [DisplayName("Número")]
        public int? Numero { get; set; }

        [NotMapped]
        [DisplayName("Usuario")]
        public string NombreUsuario { get; set; }

        [NotMapped]
        public List<adm_documento> ListadoDocumento { get; set; }

        [NotMapped]
        public int PaginaActual { get; set; }

        [NotMapped]
        public int RegistrosPorPagina { get; set; }

        [NotMapped]
        public int TotalRegistros { get; set; }

        [NotMapped]
        public int TotalPaginas { get; set; }

        [NotMapped]
        [DisplayName("Nit Emisor")]
        public string NitEmisor { get; set; }

        [NotMapped]
        public string CodigoTipoDocumento { get; set; }

        [NotMapped]
        public string Abreviatura { get; set; }


        [NotMapped]
        public string RutaXml { get; set; }

        [NotMapped]
        public string RutaZip { get; set; }

        [NotMapped]
        public string TipoDocumentoCompleto { get; set; }

        [NotMapped]
        public string UrlImportar { get; set; }

        [NotMapped]
        public string CodigoEstado { get; set; }

        [NotMapped]
        public string RutaEstadoFacse { get; set; }

        [NotMapped]
        public string RutaJson { get; set; }

        [NotMapped]
        [EmailAddress(ErrorMessage = "Email Invalido")]
        public string Email { get; set; }

        [NotMapped]
        public string CorreoReceptor { get; set; }
    }

    [MetadataType(typeof(adm_emisor_resolucionMetadata))]
    public partial class adm_emisor_resolucion
    {
        [NotMapped]
        [DisplayName("Tipo Documento")]
        public string NombreTipoDocumento { get; set; }

        [NotMapped]
        public string NombreDocumento { get; set; }
    }

    [MetadataType(typeof(adm_emisor_catalogoMetadata))]
    public partial class adm_emisor_catalogo
    {
        [NotMapped]
        [DisplayName("Tipo Dato")]
        public string NombreTipoDato { get; set; }

        [NotMapped]
        [DisplayName("Tipo Catalogo")]
        public string NombreTipoCatalogo { get; set; }
    }

    [MetadataType(typeof(adm_emisor_producto))]
    public partial class adm_emisor_producto
    {
        [NotMapped]
        public bool CheckListProducto { get; set; }
    }

    public partial class AdquirienteReceptor
    {
        public Guid IdReceptor { get; set; }
        public Guid TipoPersona { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string TipoAdquiriente { get; set; }
        //public string CodigoTipoReceptor { get; set; }
        public Guid TipoIdentificacion { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Digito { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public Guid IdPais { get; set; }
        public string TextPais { get; set; }
        public string CodigoPais { get; set; }
        public Guid? IdDepartamento { get; set; }
        public string TextDepartamento { get; set; }
        public string CodigoDepartamento { get; set; }
        public Guid? IdMunicipio { get; set; }
        public string TextMunicipio { get; set; }
        public string CodigoMunicipio { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string MatriculaMercantil { get; set; }
    }

    public partial class DatosEmisor
    {
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
        public string AccessToken { get; set; }
        public string ClientToken { get; set; }
    }

    public class Descripcion
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }

    public class Notificaciones
    {
        public Guid Id { get; set; }
        public Guid Tipo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public string Color { get; set; }
    }

    public class DatosReceptor
    {
        public Guid Id { get; set; }
        public string Identificacion { get; set; }
        public string Digito { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int CantidadFacturas { get; set; }
    }

    public partial class DatosUsuario
    {
        public Guid Id { get; set; }
        public string sUsuario { get; set; }
        public string Contrasena { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public bool DirectorioActivo { get; set; }
        public bool Activo { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }

        public List<SelectListItem> ListadoTipo { get; set; }
        public Guid Tipo { get; set; }

        public List<SelectListItem> ListadoPerfil { get; set; }
        public Guid Perfil { get; set; }

        public List<SelectListItem> ListadoSucursales { get; set; }
        public IEnumerable<Guid> Sucursales { get; set; }
    }


    public partial class Insertar
    {
        public string Nit { get; set; }
        public string Receptor { get; set; }
        public decimal? Impuesto { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Ides { get; set; }
        public Guid emisor { get; set; }
        public string Identificacion { get; set; }
        public string Json { get; set; }
        public DateTime Fecha { get; set; }
        public string idUsuario { get; set; }
        public int Consecutivo { get; set; }
        public List<Insertar> ListadoTemporal { get; set; }
        public string prefijo { get; set; }
        public decimal? numero { get; set; }
    }

    [MetadataType(typeof(adm_emisorMetadata))]
    public partial class adm_emisor
    {
        [NotMapped]
        [DisplayName("Nombre Identificacion")]
        public string NombreTipoIdentificacion { get; set; }

        [NotMapped]
        [DisplayName("Departamento")]
        public string NombreDepartamento { get; set; }

        [NotMapped]
        [DisplayName("Municipio")]
        public string NombreMunicipio { get; set; }

        [NotMapped]
        public List<adm_emisor> ListadoEmisor { get; set; }

        [NotMapped]
        public int PaginaActual { get; set; }

        [NotMapped]
        public int RegistrosPorPagina { get; set; }

        [NotMapped]
        public int TotalRegistros { get; set; }

        [NotMapped]
        public int TotalPaginas { get; set; }

        [NotMapped]
        public string Titulo { get; set; }

        [NotMapped]
        public string JsFuncion { get; set; }

        [NotMapped]
        [DisplayName("Fecha Ultimo Documento")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaUltimoDocumento { get; set; }
    }

    [MetadataType(typeof(adm_emisor_catalogo_listaMetadata))]
    public partial class adm_emisor_catalogo_lista
    {

        [NotMapped]
        public List<adm_emisor_catalogo_lista> ListadoEmisorCatalogo { get; set; }
    }

    [MetadataType(typeof(adm_emisor_certificadoMetadata))]
    public partial class adm_emisor_certificado
    {

    }

    [MetadataType(typeof(adm_emisor_notificacionMetadata))]
    public partial class adm_emisor_notificacion
    {
        [NotMapped]
        [DisplayName("Tipo Notificacion")]
        public string NombreTipoNotificacion { get; set; }
    }

    [MetadataType(typeof(adm_emisor_correoMetadata))]
    public partial class adm_emisor_correo
    {
        [NotMapped]
        [DisplayName("Tipo Correo")]
        public string NombreTipoCorreo { get; set; }


    }

    [MetadataType(typeof(adm_emisor_sucursalMetadata))]
    public partial class adm_emisor_sucursal
    {
        [NotMapped]
        public string NombreDepartamento { get; set; }

        [NotMapped]
        public string NombreMunicipio { get; set; }
    }

    [MetadataType(typeof(adm_documento_correoMetadata))]
    public partial class adm_documento_correo
    {

    }

    [MetadataType(typeof(adm_documentoproveedorMetadata))]
    public partial class adm_documento_proveedor
    {
        [NotMapped]
        [DisplayName("Fecha Inicial")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaInicial { get; set; }

        [NotMapped]
        [DisplayName("Fecha Final")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaFinal { get; set; }

        [NotMapped]
        [DisplayName("Identificación")]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [NotMapped]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [NotMapped]
        [DisplayName("Tipo")]
        public Guid? IdComprobante { get; set; }

        [NotMapped]
        [DisplayName("Estado Facse")]
        public Guid? IdEstadoFacse { get; set; }

        [NotMapped]
        public string NombreComprobante { get; set; }

        [NotMapped]
        [DisplayName("Prefijo")]
        public string Prefijo { get; set; }

        [NotMapped]
        [DisplayName("Número")]
        public int? Numero { get; set; }

        [NotMapped]
        [DisplayName("Usuario")]
        public string NombreUsuario { get; set; }

        [NotMapped]
        [DisplayName("Nit Emisor")]
        public string NitEmisor { get; set; }

        [NotMapped]
        public string CodigoTipoDocumento { get; set; }

        [NotMapped]
        public string Abreviatura { get; set; }


        [NotMapped]
        public string RutaXml { get; set; }

        [NotMapped]
        public string RutaZip { get; set; }

        [NotMapped]
        public string TipoDocumentoCompleto { get; set; }

        [NotMapped]
        public string UrlImportar { get; set; }

        [NotMapped]
        public string RutaEstadoFacse { get; set; }

        [NotMapped]
        public string RutaJson { get; set; }

        [NotMapped]
        [EmailAddress(ErrorMessage = "Email Invalido")]
        public string Email { get; set; }

        [NotMapped]
        public string CorreoReceptor { get; set; }
    }

    #region HelpClass

    public partial class DatosSucursal
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
        public Guid? Pais { get; set; }
        public Guid? Departamento { get; set; }
        public Guid? Municipio { get; set; }
        public Guid CorreoEntrada { get; set; }
        public Guid CorreoSalida { get; set; }
        public bool HasResolution { get; set; }
    }

    public partial class DatosResolucion
    {
        public Guid Id { get; set; }
        public string Prefijo { get; set; }
        public Guid TipoDocumento { get; set; }
        public string NumeroResolucion { get; set; }
        public DateTime Fecha { get; set; }
        public string NumeroInicial { get; set; }
        public string NumeroFinal { get; set; }
        public string ClaveTecnica { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Estado { get; set; }

        public Guid? Plantilla { get; set; }

        public string Ruta { get; set; }
    }

    public partial class DatosSucursalResolucion
    {
        public Guid Id { get; set; }
        public string NumeroResolucion { get; set; }
        public string Prefijo { get; set; }
        public string NumeroInicial { get; set; }
        public string NumeroFinal { get; set; }
        public string TipoDocumento { get; set; }
        public Guid IdResSuc { get; set; }
    }

    public partial class DatosUsuarioSucursal
    {
        public Guid IdSucursal { get; set; }
        public string NombreSuc { get; set; }
        public string Abreviatura { get; set; }
    }

    public partial class DatosCorreo
    {
        public Guid TipoCorreo { get; set; }
        public string TipoCorreoText { get; set; }
        public string Nombre { get; set; }
        public string Servidor { get; set; }
        public string Puerto { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Correo { get; set; }
        public bool Ssl { get; set; }
        public string CorreoHtml { get; set; }
        public bool Estado { get; set; }
        public Guid IdCorreo { get; set; }

    }

    public partial class DatosCertificado
    {
        public Guid IdCertificado { get; set; }
        public string Archivo { get; set; }
        public string Certificado { get; set; }
        public DateTime FechaVigencia { get; set; }
        public bool Estado { get; set; }
        public string Contrasena { get; set; }
    }
    public partial class DatosCatalogo
    {
        public Guid IdCatalogo { get; set; }
        public Guid TipoDato { get; set; }
        public Guid TipoCatalogo { get; set; }
        public string Nombre { get; set; }
        public bool Lista { get; set; }
        public string TipoCatalogoText { get; set; }
        public string TipoDatoText { get; set; }
        public bool Estado { get; set; }
        public int ValorUnitario { get; set; }
    }

    public partial class DatosCatalogoDetalle
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
    }

    public partial class DatosProducto
    {
        public Guid Id { get; set; }
        public Guid IdImpuesto { get; set; }
        public Guid IdProducto { get; set; }
        public Guid IdUnidad { get; set; }
        public Guid Emisor { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public decimal ValorUnidad { get; set; }
        public decimal ValorUnitario { get; set; }

        public bool Activo { get; set; }
        public Guid TipoProducto { get; set; }
        public Guid TipoDato { get; set; }
        public List<SelectListItem> ListadoTipoUnidad { get; set; }
        public List<SelectListItem> ListadoTipoImpuesto { get; set; }
        //public List<Catalogo> ListadoCatalogo { get; set; }    

    }

    public partial class DatosProductoDetalle
    {
        public string Producto { get; set; }
        public string Catalogo { get; set; }
        public string Descripcion { get; set; }
    }

    public partial class DatosProductoListado
    {
        public Guid IdProducto { get; set; }
        public Guid Unidad { get; set; }
        public Guid Impuesto { get; set; }
        public string Catalogo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal Cargo { get; set; }
        public decimal Descuento { get; set; }
        public string Codigo { get; set; }
    }

    public partial class DatosPlantilla
    {
        public Guid IdEmisorSucursalPlantilla { get; set; }
        public Guid IdEmisorSucursal { get; set; }
        public Guid IdUsuarioCreacion { get; set; }
        public string EmisorSucursal { get; set; }
        public string Logo { get; set; }
        public string sLogo { get; set; }
        public string Archivo { get; set; }
        public string PrimerMensaje { get; set; }
        public string SegundoMensaje { get; set; }
        public string TercerMensaje { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string JsFuncion { get; set; }
        public Guid EsuEmisor { get; set; }
    }
    public class ListadoEmisor
    {
        public Guid Emisor { get; set; }

        public Guid Id { get; set; }

        public Guid Pais { get; set; }

        [NotMapped]
        public List<adm_emisor_correo> ListadoEmisorCorreo { get; set; }

        [NotMapped]
        public List<adm_emisor_notificacion> ListadoEmisorNotificacion { get; set; }

        [NotMapped]
        public List<adm_emisor_certificado> ListadoEmisorCertificado { get; set; }


        [NotMapped]
        public List<adm_emisor_catalogo> ListadoEmisorCatalogo { get; set; }

        [NotMapped]
        public List<adm_emisor_resolucion> ListadoEmisorResolucion { get; set; }

        [NotMapped]
        public List<adm_emisor_sucursal> ListadoEmisorSucursal { get; set; }

        [NotMapped]
        public string JsFuncion { get; set; }
    }

    #endregion

}
