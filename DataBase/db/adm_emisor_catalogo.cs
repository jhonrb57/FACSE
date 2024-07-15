namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_catalogo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor_catalogo()
        {
            adm_emisor_catalogo_lista = new HashSet<adm_emisor_catalogo_lista>();
            adm_emisor_producto_catalogo = new HashSet<adm_emisor_producto_catalogo>();
        }

        [Key]
        public Guid eca_id { get; set; }

        public Guid eca_emisor { get; set; }

        public Guid eca_tipo_dato { get; set; }

        [Required]
        [StringLength(100)]
        public string eca_nombre { get; set; }

        public Guid eca_tipo_catalogo { get; set; }

        public bool eca_lista { get; set; }

        public bool? eca_activo { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_catalogo_lista> adm_emisor_catalogo_lista { get; set; }

        public virtual sys_tipo_catalogo sys_tipo_catalogo { get; set; }

        public virtual sys_tipo_dato sys_tipo_dato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_producto_catalogo> adm_emisor_producto_catalogo { get; set; }
    }
}
