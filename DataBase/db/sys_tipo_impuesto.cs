namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_impuesto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_impuesto()
        {
            adm_emisor_impuesto = new HashSet<adm_emisor_impuesto>();
            adm_emisor_producto = new HashSet<adm_emisor_producto>();
            adm_tipo_retencion_fuente = new HashSet<adm_tipo_retencion_fuente>();
        }

        [Key]
        public Guid tim_id { get; set; }

        [Required]
        [StringLength(10)]
        public string tim_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string tim_nombre { get; set; }

        [StringLength(200)]
        public string tim_descripcion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tim_porcentaje { get; set; }

        public Guid? tim_clasificacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_impuesto> adm_emisor_impuesto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_producto> adm_emisor_producto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_tipo_retencion_fuente> adm_tipo_retencion_fuente { get; set; }

        public virtual sys_tipo_clasificacion_impuesto sys_tipo_clasificacion_impuesto { get; set; }
    }
}
