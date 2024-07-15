namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_certificado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor_certificado()
        {
            adm_documento = new HashSet<adm_documento>();
        }

        [Key]
        public Guid ece_id { get; set; }

        public Guid ece_emisor { get; set; }

        [Required]
        [StringLength(200)]
        public string ece_archivo { get; set; }

        [Required]
        public string ece_certificado { get; set; }

        public DateTime ece_fecha_vegencia { get; set; }

        public bool ece_activo { get; set; }

        [StringLength(50)]
        public string ece_contrasena { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }
    }
}
