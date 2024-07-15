namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_negociacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_negociacion()
        {
            adm_emisor_contrato = new HashSet<adm_emisor_contrato>();
        }

        [Key]
        public Guid tne_id { get; set; }

        [Required]
        [StringLength(50)]
        public string tne_descripcion { get; set; }

        public int tne_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_contrato> adm_emisor_contrato { get; set; }
    }
}
