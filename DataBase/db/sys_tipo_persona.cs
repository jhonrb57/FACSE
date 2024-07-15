namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_persona
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_persona()
        {
            adm_emisor = new HashSet<adm_emisor>();
            adm_proveedor = new HashSet<adm_proveedor>();
            adm_receptor = new HashSet<adm_receptor>();
            AuditLogReceptor = new HashSet<AuditLogReceptor>();
        }

        [Key]
        public Guid tpe_id { get; set; }

        [Required]
        [StringLength(5)]
        public string tpe_codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string tpe_descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_proveedor> adm_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_receptor> adm_receptor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditLogReceptor> AuditLogReceptor { get; set; }
    }
}
