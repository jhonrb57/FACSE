namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_ciiu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_ciiu()
        {
            adm_emisor = new HashSet<adm_emisor>();
        }

        [Key]
        public Guid cii_id { get; set; }

        [Required]
        [StringLength(80)]
        public string cii_codigo { get; set; }

        [Required]
        [StringLength(500)]
        public string cii_descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }
    }
}
