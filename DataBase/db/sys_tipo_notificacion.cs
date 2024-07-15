namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_notificacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_notificacion()
        {
            adm_emisor_notificacion = new HashSet<adm_emisor_notificacion>();
        }

        [Key]
        public Guid tno_id { get; set; }

        [Required]
        [StringLength(100)]
        public string tno_descripcion { get; set; }

        [StringLength(100)]
        public string tno_ruta_imagen { get; set; }

        public bool tno_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_notificacion> adm_emisor_notificacion { get; set; }
    }
}
