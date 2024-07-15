namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor()
        {
            adm_documento = new HashSet<adm_documento>();
            adm_documento_proveedor = new HashSet<adm_documento_proveedor>();
            AuditLogReceptor = new HashSet<AuditLogReceptor>();
            adm_emisor_catalogo = new HashSet<adm_emisor_catalogo>();
            adm_emisor_certificado = new HashSet<adm_emisor_certificado>();
            adm_emisor_correo = new HashSet<adm_emisor_correo>();
            adm_emisor_impuesto = new HashSet<adm_emisor_impuesto>();
            adm_emisor_producto = new HashSet<adm_emisor_producto>();
            adm_emisor_resolucion = new HashSet<adm_emisor_resolucion>();
            adm_emisor_sucursal = new HashSet<adm_emisor_sucursal>();
            adm_emisor_sucursal_resolucion = new HashSet<adm_emisor_sucursal_resolucion>();
            adm_proveedor = new HashSet<adm_proveedor>();
            adm_receptor = new HashSet<adm_receptor>();
        }

        [Key]
        public Guid emi_id { get; set; }

        public Guid emi_grupo { get; set; }

        public Guid emi_tipo_persona { get; set; }

        public Guid emi_tipo_identificacion { get; set; }

        [Required]
        [StringLength(15)]
        public string emi_identificacion { get; set; }

        [StringLength(1)]
        public string emi_digito { get; set; }

        [Required]
        [StringLength(150)]
        public string emi_nombre { get; set; }

        [Required]
        [StringLength(150)]
        public string emi_razon_social { get; set; }

        public Guid? emi_pais { get; set; }

        public Guid? emi_departamento { get; set; }

        public Guid? emi_municipio { get; set; }

        [StringLength(50)]
        public string emi_codigo_posta { get; set; }

        [StringLength(80)]
        public string emi_correo { get; set; }

        [StringLength(200)]
        public string emi_direccion { get; set; }

        [StringLength(50)]
        public string emi_telefono { get; set; }

        public Guid? emi_tipo_emisor { get; set; }

        public Guid? emi_distribuidor { get; set; }

        [StringLength(50)]
        public string emi_logo { get; set; }

        [StringLength(50)]
        public string emi_test_id { get; set; }

        [StringLength(200)]
        public string emi_cliente_token { get; set; }

        [StringLength(200)]
        public string emi_access_token { get; set; }

        public bool emi_activo { get; set; }

        public DateTime? emi_fecha_creacion { get; set; }

        public Guid? emi_sofware { get; set; }

        public Guid? emi_ciiu { get; set; }

        [StringLength(20)]
        public string emi_matricula_mercantil { get; set; }

        [StringLength(20)]
        public string emi_identificacion_alterna { get; set; }

        public Guid? emi_mandato { get; set; }
        public bool emi_correo_automatico { get; set; }

        public virtual adm_distriduidor adm_distriduidor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor> adm_documento_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditLogReceptor> AuditLogReceptor { get; set; }

        public virtual adm_mandato adm_mandato { get; set; }

        public virtual adm_grupo_emisor adm_grupo_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_catalogo> adm_emisor_catalogo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_certificado> adm_emisor_certificado { get; set; }

        public virtual adm_emisor_contrato adm_emisor_contrato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_correo> adm_emisor_correo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_impuesto> adm_emisor_impuesto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_producto> adm_emisor_producto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_resolucion> adm_emisor_resolucion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal> adm_emisor_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal_resolucion> adm_emisor_sucursal_resolucion { get; set; }

        public virtual sys_ciiu sys_ciiu { get; set; }

        public virtual sys_departamento sys_departamento { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }

        public virtual sys_pais sys_pais { get; set; }

        public virtual sys_software sys_software { get; set; }

        public virtual sys_tipo_emisor sys_tipo_emisor { get; set; }

        public virtual sys_tipo_identificacion sys_tipo_identificacion { get; set; }

        public virtual sys_tipo_persona sys_tipo_persona { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_proveedor> adm_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_receptor> adm_receptor { get; set; }
    }
}
