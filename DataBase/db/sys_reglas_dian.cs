namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_reglas_dian
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_reglas_dian()
        {
            adm_documento_notificacion = new HashSet<adm_documento_notificacion>();
        }

        [Key]
        public Guid rdi_id { get; set; }

        [StringLength(20)]
        public string rdi_regla { get; set; }

        [StringLength(2000)]
        public string rdi_descripcion_dian { get; set; }

        [StringLength(2000)]
        public string rdi_descripcion_facse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_notificacion> adm_documento_notificacion { get; set; }
    }
}
