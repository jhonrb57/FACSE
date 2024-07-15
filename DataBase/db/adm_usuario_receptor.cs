namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_usuario_receptor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_usuario_receptor()
        {
            adm_receptor = new HashSet<adm_receptor>();
        }

        [Key]
        public Guid ure_id { get; set; }

        [Required]
        [StringLength(50)]
        public string ure_usuario { get; set; }

        [Required]
        [StringLength(200)]
        public string ure_contrasena { get; set; }

        [Required]
        [StringLength(200)]
        public string ure_nombre { get; set; }

        [StringLength(200)]
        public string ure_apellido { get; set; }

        public bool ure_activo { get; set; }

        public bool ure_directorio_activo { get; set; }

        [StringLength(100)]
        public string ure_email { get; set; }

        [StringLength(100)]
        public string ure_direccion { get; set; }

        [StringLength(100)]
        public string ure_telefono { get; set; }

        public DateTime ure_fecha_creacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_receptor> adm_receptor { get; set; }
    }
}
