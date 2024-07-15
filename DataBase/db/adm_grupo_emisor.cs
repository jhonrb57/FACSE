namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_grupo_emisor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_grupo_emisor()
        {
            adm_emisor = new HashSet<adm_emisor>();
        }

        [Key]
        public Guid gen_id { get; set; }

        [Required]
        [StringLength(200)]
        public string gen_nombre { get; set; }

        [StringLength(200)]
        public string gen_direccion { get; set; }

        [StringLength(200)]
        public string gen_email { get; set; }

        [StringLength(100)]
        public string gen_telefono { get; set; }

        public Guid gen_municipio { get; set; }

        public bool gen_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor> adm_emisor { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }
    }
}
