using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataBase
{
    public class adm_documentoMetadata
    {
        [DisplayName("Numero")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal doc_numero { get; set; }

        [DisplayName("Prefijo")]
        public string doc_prefijo { get; set; }

        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime doc_fecha_recepcion { get; set; }

        [DisplayName("Fecha Envio")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime doc_fecha_envio { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public string doc_valor_total { get; set; }

        [DisplayName("Usuario")]
        public string doc_usuario { get; set; }
    }

    public class adm_emisorMetadata
    {
        [IsNotEmpty]
        [DisplayName("Grupo")]
        public Guid emi_grupo { get; set; }

        [IsNotEmpty]
        [DisplayName("Tipo Persona")]
        public Guid emi_tipo_persona { get; set; }

        [IsNotEmpty]
        [DisplayName("Tipo Identificacion")]
        public Guid emi_tipo_identificacion { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("Identificacion")]
        public string emi_identificacion { get; set; }

        [Required]
        [StringLength(1)]
        [DisplayName("Digito")]
        public string emi_digito { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Nombre")]
        public string emi_nombre { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Razon Social")]
        public string emi_razon_social { get; set; }

        [IsNotEmpty]
        [DisplayName("Pais")]
        public Guid? emi_pais { get; set; }

        [IsNotEmpty]
        [DisplayName("Departamento")]
        public Guid? emi_departamento { get; set; }

        [IsNotEmpty]
        [DisplayName("Municipio")]
        public Guid? emi_municipio { get; set; }

        [DisplayName("Codigo Postal")]
        [StringLength(50)]
        public string emi_codigo_posta { get; set; }

        [Required]
        [DisplayName("Correo")]
        [StringLength(80)]
        public string emi_correo { get; set; }

        [DisplayName("Direccion")]
        [StringLength(200)]
        public string emi_direccion { get; set; }

        [DisplayName("Telefono")]
        [StringLength(50)]
        public string emi_telefono { get; set; }

        [DisplayName("Tipo Emisor")]
        public Guid? emi_tipo_emisor { get; set; }

        [DisplayName("Distribuidor")]
        public Guid? emi_distribuidor { get; set; }

        [DisplayName("Logo")]
        [StringLength(50)]
        public string emi_logo { get; set; }

        [DisplayName("Test ID")]
        [StringLength(50)]
        public string emi_test_id { get; set; }

        [DisplayName("Cliente Token")]
        [StringLength(80)]
        public string emi_cliente_token { get; set; }

        [DisplayName("Access Token")]
        [StringLength(80)]
        public string emi_access_token { get; set; }

        [DisplayName("Activo")]
        public bool emi_activo { get; set; }

        [IsNotEmpty]
        [DisplayName("Software")]
        public Guid? emi_sofware { get; set; }

        [IsNotEmpty]
        [DisplayName("Ciiu")]
        public Guid? emi_ciiu { get; set; }

        [DisplayName("Matricula Mercantil")]
        [StringLength(20)]
        public string emi_matricula_mercantil { get; set; }

        [DisplayName("Fecha Creacion")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? emi_fecha_creacion { get; set; }

    }

    public class IsNotEmptyAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {

            if (value == null) return false;

            var valueType = value.GetType();
            var emptyField = valueType.GetField("Empty");

            if (emptyField == null) return true;

            var emptyValue = emptyField.GetValue(null);

            return !value.Equals(emptyValue);

        }

        public class adm_emisor_catalogoMetadata
        {
            [IsNotEmpty]
            [DisplayName("Tipo Dato")]
            public Guid eca_tipo_dato { get; set; }

            [Required]
            [StringLength(100)]
            [DisplayName("Nombre")]
            public string eca_nombre { get; set; }

            [IsNotEmpty]
            [DisplayName("Tipo Catalogo")]
            public Guid eca_tipo_catalogo { get; set; }

            [DisplayName("Lista")]
            public bool eca_lista { get; set; }
        }

        public class adm_emisor_catalogo_listaMetadata
        {
            [DisplayName("Descripcion")]
            [StringLength(200)]
            public string ecl_descripcion { get; set; }
        }

        public class adm_emisor_certificadoMetadata
        {
            [Required]
            [DisplayName("Archivo")]
            [StringLength(200)]
            public string ece_archivo { get; set; }

            [Required]
            [DisplayName("Certificado")]
            public string ece_certificado { get; set; }

            [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime ece_fecha_vegencia { get; set; }

            [DisplayName("Activo")]
            public bool ece_activo { get; set; }

            [DisplayName("Contraseña")]
            [StringLength(50)]
            public string ece_contrasena { get; set; }
        }

        public class adm_emisor_notificacionMetadata
        {
            [IsNotEmpty]
            [DisplayName("Tipo Notificacion")]
            public Guid? eno_id_tipo_notificacion { get; set; }

            [Required]
            [DisplayName("Descripcion")]
            [StringLength(500)]
            public string eno_descripcion { get; set; }

            [DisplayName("Fecha")]
            [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime? eno_fecha { get; set; }

            [DisplayName("Leido")]
            public bool? eno_leido { get; set; }
        }

        public class adm_emisor_correoMetadata
        {
            [DisplayName("Emisor")]
            public Guid eco_emisor { get; set; }

            [IsNotEmpty]
            [DisplayName("Tipo Correo")]
            public Guid? eco_tipo_correo { get; set; }

            [Required]
            [DisplayName("Nombre")]
            [StringLength(50)]
            public string eco_nombre { get; set; }

            [DisplayName("Servidor")]
            [Required]
            [StringLength(100)]
            public string eco_servidor { get; set; }

            [DisplayName("Puerto")]
            [Required]
            [StringLength(50)]
            public string eco_puerto { get; set; }

            [DisplayName("Usuario")]
            [Required]
            [StringLength(100)]
            public string eco_usuario { get; set; }

            [DisplayName("Contraseña")]
            [Required]
            [StringLength(100)]
            public string eco_contrasena { get; set; }

            [DisplayName("Correo")]
            [Required]
            [StringLength(100)]
            public string eco_correo { get; set; }

            [DisplayName("Ssl")]
            [Required]
            public bool? eco_ssl { get; set; }

            [DisplayName("Correo Html")]
            [Required]
            [StringLength(100)]
            public string eco_correo_html { get; set; }

            [DisplayName("Activo")]
            public bool eco_activo { get; set; }
        }

        public class adm_emisor_resolucionMetadata
        {
            [IsNotEmpty]
            [DisplayName("Tipo Documento")]
            public Guid ere_tipo_documento { get; set; }

            [Required]
            [StringLength(5)]
            [DisplayName("Prefijo")]
            public string ere_prefijo { get; set; }

            [Required]
            [StringLength(500)]
            [DisplayName("Numero Resolucion")]
            public string ere_numero_resolucion { get; set; }

            [Required]
            [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [DisplayName("Fecha")]
            public DateTime ere_fecha { get; set; }

            [Required]
            [StringLength(50)]
            [DisplayName("Numero Inicial")]
            public string ere_numero_inicial { get; set; }

            [Required]
            [StringLength(50)]
            [DisplayName("Numero Final")]
            public string ere_numero_final { get; set; }

            [Required]
            [StringLength(300)]
            [DisplayName("Clave Tecnica")]
            public string ere_clave_tecnica { get; set; }

            [Required]
            [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [DisplayName("Fecha Inicio")]
            public DateTime ere_fecha_inicio { get; set; }

            [Required]
            [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [DisplayName("Fecha Final")]
            public DateTime ere_fecha_final { get; set; }

            [DisplayName("Activo")]
            public bool ere_activo { get; set; }
        }

        public class adm_emisor_sucursalMetadata
        {
            [Required]
            [StringLength(80)]
            [DisplayName("Nombre")]
            public string esu_nombre { get; set; }

            [Required]
            [StringLength(20)]
            [DisplayName("Abreviatura")]
            public string esu_abreviatura { get; set; }

            [IsNotEmpty]
            [DisplayName("Departamento")]
            public Guid? esu_departamento { get; set; }

            [IsNotEmpty]
            [DisplayName("Municipio")]
            public Guid? esu_municipio { get; set; }

            [Required]
            [DisplayName("Correo")]
            [StringLength(100)]
            public string esu_correo { get; set; }

            [DisplayName("Direccion")]
            [StringLength(100)]
            public string esu_direccion { get; set; }

            [DisplayName("Codigo Postal")]
            [StringLength(80)]
            public string esu_codigo_postal { get; set; }

            [DisplayName("Telefono")]
            [StringLength(40)]
            public string esu_telefono { get; set; }

            [DisplayName("Correo Entrada")]
            public Guid esu_correo_entrada { get; set; }

            [DisplayName("Correo Saldida")]
            public Guid esu_correo_salida { get; set; }

            [DisplayName("Activo")]
            public bool esu_activo { get; set; }

        }
    }

    public class adm_documento_correoMetadata
    {
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dco_fecha { get; set; }
    }

    public class adm_documentoproveedorMetadata
    {
        [DisplayName("Numero")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal dpr_numero { get; set; }

        [DisplayName("Prefijo")]
        public string dpr_prefijo { get; set; }

        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dpr_fecha_recepcion { get; set; }

        [DisplayName("Fecha Recibido")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dpr_fecha_recibido { get; set; }

        [DisplayName("Fecha Envio")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dpr_fecha_envio { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public string dpr_valor_total { get; set; }

        [DisplayName("Usuario")]
        public string dpr_usuario { get; set; }
    }
}
