namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_receptor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_receptor()
        {
            adm_documento = new HashSet<adm_documento>();
            AuditLogReceptor = new HashSet<AuditLogReceptor>();
            log_receptor_nombre = new HashSet<log_receptor_nombre>();
        }

        [Key]
        public Guid rec_id { get; set; }

        public Guid rec_emisor { get; set; }

        [Required]
        [StringLength(100)]
        public string rec_tipo_receptor { get; set; }

        public Guid? rec_tipo_persona { get; set; }

        public Guid rec_tipo_identificacion { get; set; }

        [Required]
        [StringLength(100)]
        public string rec_identificacion { get; set; }

        [StringLength(5)]
        public string rec_digito { get; set; }

        [Required]
        [StringLength(200)]
        public string rec_nombre { get; set; }

        [Required]
        [StringLength(200)]
        public string rec_razon_social { get; set; }

        public Guid? rec_pais { get; set; }

        public Guid? rec_departamento { get; set; }

        public Guid? rec_municipio { get; set; }

        [StringLength(10)]
        public string rec_codigo_postal { get; set; }

        [StringLength(200)]
        public string rec_correo { get; set; }

        [StringLength(150)]
        public string rec_direccion { get; set; }

        [StringLength(50)]
        public string rec_telefono { get; set; }

        public DateTime rec_fecha_receccion { get; set; }

        public bool rec_activo { get; set; }

        [StringLength(20)]
        public string rec_matricula_mercantil { get; set; }

        [StringLength(200)]
        public string rec_contrasena { get; set; }

        public Guid? rec_usuario_receptor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditLogReceptor> AuditLogReceptor { get; set; }

        public virtual adm_usuario_receptor adm_usuario_receptor { get; set; }

        public virtual sys_departamento sys_departamento { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }

        public virtual sys_pais sys_pais { get; set; }

        public virtual sys_tipo_identificacion sys_tipo_identificacion { get; set; }

        public virtual sys_tipo_persona sys_tipo_persona { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<log_receptor_nombre> log_receptor_nombre { get; set; }
    }
}
