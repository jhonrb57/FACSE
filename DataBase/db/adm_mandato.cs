namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_mandato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_mandato()
        {
            adm_emisor = new HashSet<adm_emisor>();
        }

        [Key]
        public Guid man_id { get; set; }

        [Required]
        [StringLength(15)]
        public string man_identificacion { get; set; }

        public int man_digito { get; set; }

        public Guid man_tipo_identificacion { get; set; }

        [Required]
        [StringLength(100)]
        public string man_nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }

        public virtual sys_tipo_identificacion sys_tipo_identificacion { get; set; }
    }
}
