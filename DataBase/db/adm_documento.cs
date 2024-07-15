namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_documento()
        {
            adm_documento_file_historico = new HashSet<adm_documento_file_historico>();
            adm_documento_correo = new HashSet<adm_documento_correo>();
            adm_documento_estado_facse = new HashSet<adm_documento_estado_facse>();
            adm_documento_eventos = new HashSet<adm_documento_eventos>();
            adm_documento_file = new HashSet<adm_documento_file>();
            adm_documento_notificacion = new HashSet<adm_documento_notificacion>();
            adm_documentos_anexo = new HashSet<adm_documentos_anexo>();
            log_receptor_nombre = new HashSet<log_receptor_nombre>();
        }

        [Key]
        public Guid doc_id { get; set; }

        [Required]
        [StringLength(10)]
        public string doc_prefijo { get; set; }

        public Guid doc_tipo_documento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal doc_numero { get; set; }

        public Guid doc_emisor { get; set; }

        public Guid doc_receptor { get; set; }

        [Required]
        [StringLength(20)]
        public string doc_plantilla_version { get; set; }

        public DateTime doc_fecha_documento { get; set; }

        public DateTime doc_fecha_recepcion { get; set; }

        public DateTime doc_fecha_envio { get; set; }

        public Guid doc_moneda { get; set; }

        public Guid doc_sucursal { get; set; }

        public Guid doc_resolucion { get; set; }

        [StringLength(800)]
        public string doc_cufe { get; set; }

        [StringLength(100)]
        public string doc_clave { get; set; }

        [StringLength(100)]
        public string doc_validacion_dian { get; set; }

        [StringLength(500)]
        public string doc_respuesta_dian { get; set; }

        [StringLength(200)]
        public string doc_zipkey { get; set; }

        [StringLength(200)]
        public string doc_trackid { get; set; }

        public decimal doc_valor_total { get; set; }

        public decimal doc_valor_impuestos { get; set; }

        public Guid doc_estado { get; set; }

        [StringLength(50)]
        public string doc_usuario { get; set; }

        [StringLength(1000)]
        public string doc_observacion { get; set; }

        public Guid? doc_id_estado_facse { get; set; }

        public Guid? doc_emisor_certificado { get; set; }

        public Guid? doc_id_origen_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_file_historico> adm_documento_file_historico { get; set; }

        public virtual adm_emisor_resolucion adm_emisor_resolucion { get; set; }

        public virtual sys_estado_documento_facse sys_estado_documento_facse { get; set; }

        public virtual sys_origen_documento sys_origen_documento { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual adm_emisor_certificado adm_emisor_certificado { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        public virtual adm_receptor adm_receptor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_correo> adm_documento_correo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_estado_facse> adm_documento_estado_facse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_eventos> adm_documento_eventos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_file> adm_documento_file { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_notificacion> adm_documento_notificacion { get; set; }

        public virtual sys_estado_documento sys_estado_documento { get; set; }

        public virtual sys_moneda sys_moneda { get; set; }

        public virtual sys_tipo_documento sys_tipo_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documentos_anexo> adm_documentos_anexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<log_receptor_nombre> log_receptor_nombre { get; set; }
    }
}
