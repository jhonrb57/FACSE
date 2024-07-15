namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_estado_documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_estado_documento()
        {
            adm_documento = new HashSet<adm_documento>();
        }

        [Key]
        public Guid ted_id { get; set; }

        [Required]
        [StringLength(50)]
        public string ted_descripcion { get; set; }

        public bool ted_activo { get; set; }

        [StringLength(5)]
        public string ted_codigo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }
    }
}
