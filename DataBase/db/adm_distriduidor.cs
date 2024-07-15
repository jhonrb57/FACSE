namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_distriduidor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_distriduidor()
        {
            adm_emisor = new HashSet<adm_emisor>();
        }

        [Key]
        public Guid dis_id { get; set; }

        public Guid dis_tipo_identificacion { get; set; }

        [Required]
        [StringLength(20)]
        public string dis_identificacion { get; set; }

        [Required]
        [StringLength(200)]
        public string dis_nombre { get; set; }

        [StringLength(200)]
        public string dis_email { get; set; }

        [StringLength(200)]
        public string dis_contacto { get; set; }

        [StringLength(100)]
        public string dis_telefono { get; set; }

        public Guid dis_municipio { get; set; }

        public bool dis_activo { get; set; }

        public DateTime dis_fecha_creacion { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }

        public virtual sys_tipo_identificacion sys_tipo_identificacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }
    }
}
