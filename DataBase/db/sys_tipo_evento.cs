namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_evento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_evento()
        {
            adm_documento_eventos = new HashSet<adm_documento_eventos>();
        }

        [Key]
        public Guid tev_id { get; set; }

        [Required]
        [StringLength(10)]
        public string tev_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string tev_descripcion { get; set; }

        [Required]
        [StringLength(150)]
        public string tev_responsable_registro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_eventos> adm_documento_eventos { get; set; }
    }
}
