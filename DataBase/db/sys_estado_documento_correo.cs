namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_estado_documento_correo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sys_estado_documento_correo()
        {
            adm_documento_correo = new HashSet<adm_documento_correo>();
        }

        [Key]
        public Guid edc_id { get; set; }

        [StringLength(200)]
        public string edc_descripcion { get; set; }

        public bool? edc_activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_correo> adm_documento_correo { get; set; }
    }
}
