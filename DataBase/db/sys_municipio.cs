namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_municipio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_municipio()
        {
            adm_distriduidor = new HashSet<adm_distriduidor>();
            adm_emisor = new HashSet<adm_emisor>();
            adm_emisor_sucursal = new HashSet<adm_emisor_sucursal>();
            adm_grupo_emisor = new HashSet<adm_grupo_emisor>();
            adm_proveedor = new HashSet<adm_proveedor>();
            adm_receptor = new HashSet<adm_receptor>();
            AuditLogReceptor = new HashSet<AuditLogReceptor>();
        }

        [Key]
        public Guid mun_id { get; set; }

        [Required]
        [StringLength(10)]
        public string mun_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string mun_nombre { get; set; }

        public Guid? mun_id_dpto { get; set; }

        public Guid? mun_id_pais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_distriduidor> adm_distriduidor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal> adm_emisor_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_grupo_emisor> adm_grupo_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_proveedor> adm_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_receptor> adm_receptor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditLogReceptor> AuditLogReceptor { get; set; }

        public virtual sys_departamento sys_departamento { get; set; }

        public virtual sys_pais sys_pais { get; set; }
    }
}
