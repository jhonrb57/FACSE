namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor_producto()
        {
            adm_emisor_producto_catalogo = new HashSet<adm_emisor_producto_catalogo>();
        }

        [Key]
        public Guid epr_id { get; set; }

        public Guid epr_emisor { get; set; }

        [Required]
        [StringLength(50)]
        public string epr_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string epr_descripcion { get; set; }

        public Guid epr_unidad { get; set; }

        [Column(TypeName = "numeric")]
        public decimal epr_valor_unitario { get; set; }

        public Guid epr_tipo_impuesto { get; set; }

        public bool epr_activo { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_producto_catalogo> adm_emisor_producto_catalogo { get; set; }

        public virtual sys_tipo_impuesto sys_tipo_impuesto { get; set; }

        public virtual sys_tipo_unidad_cantidad sys_tipo_unidad_cantidad { get; set; }
    }
}
