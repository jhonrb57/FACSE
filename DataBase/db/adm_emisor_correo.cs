namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_correo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor_correo()
        {
            adm_emisor_sucursal = new HashSet<adm_emisor_sucursal>();
            adm_emisor_sucursal1 = new HashSet<adm_emisor_sucursal>();
        }

        [Key]
        public Guid eco_id { get; set; }

        public Guid eco_emisor { get; set; }

        [Required]
        [StringLength(100)]
        public string eco_servidor { get; set; }

        [Required]
        [StringLength(50)]
        public string eco_puerto { get; set; }

        [Required]
        [StringLength(100)]
        public string eco_usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string eco_contrasena { get; set; }

        [Required]
        [StringLength(100)]
        public string eco_correo { get; set; }

        public bool eco_ssl { get; set; }

        [Required]
        [StringLength(100)]
        public string eco_correo_html { get; set; }

        public bool eco_activo { get; set; }

        public Guid? eco_tipo_correo { get; set; }

        [StringLength(50)]
        public string eco_nombre { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal> adm_emisor_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal> adm_emisor_sucursal1 { get; set; }

        public virtual sys_tipo_correo sys_tipo_correo { get; set; }
    }
}
