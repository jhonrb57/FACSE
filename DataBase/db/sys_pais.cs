namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_pais
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_pais()
        {
            adm_emisor = new HashSet<adm_emisor>();
            adm_proveedor = new HashSet<adm_proveedor>();
            adm_receptor = new HashSet<adm_receptor>();
            AuditLogReceptor = new HashSet<AuditLogReceptor>();
            sys_departamento = new HashSet<sys_departamento>();
            sys_municipio = new HashSet<sys_municipio>();
        }

        [Key]
        public Guid pai_id { get; set; }

        [StringLength(50)]
        public string pai_nombre_comun { get; set; }

        [Required]
        [StringLength(100)]
        public string pai_nombre_iso { get; set; }

        [Required]
        [StringLength(50)]
        public string pai_codigo_2 { get; set; }

        [Required]
        [StringLength(50)]
        public string pai_codigo_3 { get; set; }

        [Required]
        [StringLength(50)]
        public string pai_codigo_numerico { get; set; }

        [StringLength(100)]
        public string pai_observacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_proveedor> adm_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_receptor> adm_receptor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditLogReceptor> AuditLogReceptor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sys_departamento> sys_departamento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sys_municipio> sys_municipio { get; set; }
    }
}
