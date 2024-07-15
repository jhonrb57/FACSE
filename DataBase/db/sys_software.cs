namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_software
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_software()
        {
            adm_emisor = new HashSet<adm_emisor>();
            sys_tipo_contingencia = new HashSet<sys_tipo_contingencia>();
        }

        [Key]
        public Guid sof_id { get; set; }

        [StringLength(200)]
        public string sof_pin { get; set; }

        [Required]
        [StringLength(200)]
        public string sof_url { get; set; }

        [StringLength(200)]
        public string sof_contrasena { get; set; }

        public Guid? sof_usuario { get; set; }

        public DateTime sof_fecha { get; set; }

        [Required]
        [StringLength(80)]
        public string sof_nombre { get; set; }

        [Required]
        [StringLength(200)]
        public string sof_id_sofware { get; set; }

        public bool sof_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sys_tipo_contingencia> sys_tipo_contingencia { get; set; }
    }
}
