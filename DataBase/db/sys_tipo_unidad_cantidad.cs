namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_unidad_cantidad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_unidad_cantidad()
        {
            adm_emisor_producto = new HashSet<adm_emisor_producto>();
        }

        [Key]
        public Guid tuc_id { get; set; }

        [Required]
        [StringLength(5)]
        public string tuc_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string tuc_descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_producto> adm_emisor_producto { get; set; }
    }
}
