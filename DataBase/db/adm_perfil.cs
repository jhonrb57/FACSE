namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_perfil
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_perfil()
        {
            adm_usuario_perfil_tipo = new HashSet<adm_usuario_perfil_tipo>();
            adm_perfil_usuario_receptor = new HashSet<adm_perfil_usuario_receptor>();
        }

        [Key]
        public Guid per_id { get; set; }

        [Required]
        [StringLength(50)]
        public string per_descripcion { get; set; }

        public bool per_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_usuario_perfil_tipo> adm_usuario_perfil_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_perfil_usuario_receptor> adm_perfil_usuario_receptor { get; set; }
    }
}
