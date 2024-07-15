namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_proveedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_proveedor()
        {
            adm_documento_proveedor = new HashSet<adm_documento_proveedor>();
        }

        [Key]
        public Guid pro_id { get; set; }

        public Guid pro_emisor { get; set; }

        [Required]
        [StringLength(100)]
        public string pro_tipo_receptor { get; set; }

        public Guid? pro_tipo_persona { get; set; }

        public Guid pro_tipo_identificacion { get; set; }

        [Required]
        [StringLength(100)]
        public string pro_identificacion { get; set; }

        [StringLength(5)]
        public string pro_digito { get; set; }

        [Required]
        [StringLength(200)]
        public string pro_nombre { get; set; }

        [Required]
        [StringLength(200)]
        public string pro_razon_social { get; set; }

        public Guid? pro_pais { get; set; }

        public Guid? pro_departamento { get; set; }

        public Guid? pro_municipio { get; set; }

        [StringLength(10)]
        public string pro_codigo_postal { get; set; }

        [StringLength(200)]
        public string pro_correo { get; set; }

        [StringLength(150)]
        public string pro_direccion { get; set; }

        [StringLength(50)]
        public string pro_telefono { get; set; }

        public DateTime pro_fecha_recepcion { get; set; }

        public bool pro_activo { get; set; }

        [StringLength(20)]
        public string pro_matricula_mercantil { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor> adm_documento_proveedor { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual sys_departamento sys_departamento { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }

        public virtual sys_pais sys_pais { get; set; }

        public virtual sys_tipo_identificacion sys_tipo_identificacion { get; set; }

        public virtual sys_tipo_persona sys_tipo_persona { get; set; }
    }
}
