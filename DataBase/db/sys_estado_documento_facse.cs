namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_estado_documento_facse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_estado_documento_facse()
        {
            adm_documento = new HashSet<adm_documento>();
            adm_documento_estado_facse = new HashSet<adm_documento_estado_facse>();
            adm_documento_proveedor = new HashSet<adm_documento_proveedor>();
            adm_documento_proveedor_estado_facse = new HashSet<adm_documento_proveedor_estado_facse>();
        }

        [Key]
        public Guid edf_id { get; set; }

        [Required]
        [StringLength(10)]
        public string edf_codigo { get; set; }

        [StringLength(100)]
        public string edf_descripcion { get; set; }

        [StringLength(200)]
        public string edf_ruta_imagen { get; set; }

        public bool? edf_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_estado_facse> adm_documento_estado_facse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor> adm_documento_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor_estado_facse> adm_documento_proveedor_estado_facse { get; set; }
    }
}
