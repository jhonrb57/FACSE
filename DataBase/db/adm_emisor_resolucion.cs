namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_resolucion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor_resolucion()
        {
            adm_documento = new HashSet<adm_documento>();
            adm_emisor_sucursal_resolucion = new HashSet<adm_emisor_sucursal_resolucion>();
        }

        [Key]
        public Guid ere_id { get; set; }

        public Guid ere_emisor { get; set; }

        public Guid ere_tipo_documento { get; set; }

        [Required]
        [StringLength(5)]
        public string ere_prefijo { get; set; }

        [Required]
        [StringLength(500)]
        public string ere_numero_resolucion { get; set; }

        public DateTime ere_fecha { get; set; }

        [Required]
        [StringLength(50)]
        public string ere_numero_inicial { get; set; }

        [Required]
        [StringLength(50)]
        public string ere_numero_final { get; set; }

        [Required]
        [StringLength(300)]
        public string ere_clave_tecnica { get; set; }

        public DateTime ere_fecha_inicio { get; set; }

        public DateTime ere_fecha_final { get; set; }

        public bool ere_activo { get; set; }

        [StringLength(10)]
        public string ere_plantilla_version { get; set; }

        public Guid? ere_plantilla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual adm_plantilla adm_plantilla { get; set; }

        public virtual sys_tipo_documento sys_tipo_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal_resolucion> adm_emisor_sucursal_resolucion { get; set; }
    }
}
