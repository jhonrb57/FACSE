namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_moneda
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_moneda()
        {
            adm_documento = new HashSet<adm_documento>();
            adm_documento_proveedor = new HashSet<adm_documento_proveedor>();
        }

        [Key]
        public Guid mon_id { get; set; }

        [Required]
        [StringLength(10)]
        public string mon_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string mon_divisa { get; set; }

        [Required]
        [StringLength(50)]
        public string mon_descripcion { get; set; }

        public bool mon_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor> adm_documento_proveedor { get; set; }
    }
}
