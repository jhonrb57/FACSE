namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_plantilla
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_plantilla()
        {
            adm_emisor_resolucion = new HashSet<adm_emisor_resolucion>();
        }

        [Key]
        public Guid prg_id { get; set; }

        [Required]
        [StringLength(200)]
        public string prg_nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string prg_direccion_rdlc { get; set; }

        [Required]
        [StringLength(100)]
        public string prg_direccion_pdf { get; set; }

        public bool prg_privado { get; set; }

        public Guid prg_tipo_documento { get; set; }

        public bool prg_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_resolucion> adm_emisor_resolucion { get; set; }

        public virtual sys_tipo_documento sys_tipo_documento { get; set; }
    }
}
