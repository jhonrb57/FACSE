namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_proveedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_documento_proveedor()
        {
            adm_documento_proveedor_anexo = new HashSet<adm_documento_proveedor_anexo>();
            adm_documento_proveedor_estado_facse = new HashSet<adm_documento_proveedor_estado_facse>();
            adm_documento_proveedor_file = new HashSet<adm_documento_proveedor_file>();
        }

        [Key]
        public Guid dpr_id { get; set; }

        [Required]
        [StringLength(10)]
        public string dpr_prefijo { get; set; }

        public Guid dpr_tipo_documento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal dpr_numero { get; set; }

        public Guid dpr_emisor { get; set; }

        public Guid dpr_proveedor { get; set; }

        [Required]
        [StringLength(20)]
        public string dpr_plantilla_version { get; set; }

        public DateTime dpr_fecha_documento { get; set; }

        public DateTime dpr_fecha_recepcion { get; set; }

        public DateTime dpr_fecha_recibido { get; set; }

        public DateTime dpr_fecha_envio { get; set; }

        public Guid dpr_moneda { get; set; }

        public Guid dpr_sucursal { get; set; }

        [StringLength(800)]
        public string dpr_cufe { get; set; }

        [StringLength(100)]
        public string dpr_clave { get; set; }

        [StringLength(100)]
        public string dpr_validacion_dian { get; set; }

        [StringLength(500)]
        public string dpr_respuesta_dian { get; set; }

        [StringLength(200)]
        public string dpr_zipkey { get; set; }

        [StringLength(200)]
        public string dpr_trackid { get; set; }

        public decimal dpr_valor_total { get; set; }

        public decimal dpr_valor_impuestos { get; set; }

        [StringLength(50)]
        public string dpr_usuario { get; set; }

        [StringLength(1000)]
        public string dpr_observacion { get; set; }

        public Guid? dpr_id_estado_facse { get; set; }

        public Guid? dpr_id_origen_documento { get; set; }

        public bool dpr_acuse { get; set; }

        public DateTime? dpr_fecha_acuse { get; set; }

        public virtual adm_proveedor adm_proveedor { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor_anexo> adm_documento_proveedor_anexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor_estado_facse> adm_documento_proveedor_estado_facse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor_file> adm_documento_proveedor_file { get; set; }

        public virtual sys_moneda sys_moneda { get; set; }

        public virtual sys_tipo_documento sys_tipo_documento { get; set; }

        public virtual sys_estado_documento_facse sys_estado_documento_facse { get; set; }

        public virtual sys_origen_documento sys_origen_documento { get; set; }
    }
}
