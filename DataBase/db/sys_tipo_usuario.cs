namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_usuario()
        {
            adm_usuario_perfil_tipo = new HashSet<adm_usuario_perfil_tipo>();
        }

        [Key]
        public Guid tus_id { get; set; }

        [Required]
        [StringLength(50)]
        public string tus_descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_usuario_perfil_tipo> adm_usuario_perfil_tipo { get; set; }
    }
}
