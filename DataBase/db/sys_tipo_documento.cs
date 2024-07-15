namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_tipo_documento()
        {
            adm_documento = new HashSet<adm_documento>();
            adm_documento_proveedor = new HashSet<adm_documento_proveedor>();
            adm_emisor_resolucion = new HashSet<adm_emisor_resolucion>();
            adm_plantilla = new HashSet<adm_plantilla>();
            tmp_documento = new HashSet<tmp_documento>();
        }

        [Key]
        public Guid tdo_id { get; set; }

        [Required]
        [StringLength(80)]
        public string tdo_codigo { get; set; }

        [Required]
        [StringLength(200)]
        public string tdo_descripcion { get; set; }

        [Required]
        [StringLength(80)]
        public string tdo_uso { get; set; }

        [StringLength(5)]
        public string tdo_abreviatura { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor> adm_documento_proveedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_resolucion> adm_emisor_resolucion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_plantilla> adm_plantilla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tmp_documento> tmp_documento { get; set; }
    }
}
