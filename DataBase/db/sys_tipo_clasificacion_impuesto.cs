namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_clasificacion_impuesto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_clasificacion_impuesto()
        {
            sys_tipo_impuesto = new HashSet<sys_tipo_impuesto>();
        }

        [Key]
        public Guid tci_id { get; set; }

        [StringLength(20)]
        public string tci_descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sys_tipo_impuesto> sys_tipo_impuesto { get; set; }
    }
}
