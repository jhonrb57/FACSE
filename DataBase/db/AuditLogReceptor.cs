namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditLogReceptor")]
    public partial class AuditLogReceptor
    {
        [Key]
        public Guid alc_id { get; set; }

        public Guid? alc_tipo_accion { get; set; }

        public Guid? alc_receptor { get; set; }

        public Guid? alc_emisor_accion { get; set; }

        public DateTime? alc_fecha_accion { get; set; }

        [StringLength(100)]
        public string alc_tipo_receptor { get; set; }

        public Guid? alc_tipo_persona { get; set; }

        public Guid? alc_tipo_identificacion { get; set; }

        [StringLength(100)]
        public string alc_identificacion { get; set; }

        [StringLength(100)]
        public string alc_contrasena { get; set; }

        [StringLength(20)]
        public string alc_digito { get; set; }

        [StringLength(200)]
        public string alc_nombre { get; set; }

        [StringLength(200)]
        public string alc_razon_social { get; set; }

        public Guid? alc_pais { get; set; }

        public Guid? alc_departamento { get; set; }

        public Guid? alc_municipio { get; set; }

        [StringLength(100)]
        public string alc_codigo_postal { get; set; }

        [StringLength(100)]
        public string alc_correo { get; set; }

        [StringLength(100)]
        public string alc_direccion { get; set; }

        [StringLength(100)]
        public string alc_telefono { get; set; }

        public bool? alc_activo { get; set; }

        [StringLength(100)]
        public string alc_matricula_mercantil { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual adm_receptor adm_receptor { get; set; }

        public virtual sys_departamento sys_departamento { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }

        public virtual sys_pais sys_pais { get; set; }

        public virtual sys_tipo_accion sys_tipo_accion { get; set; }

        public virtual sys_tipo_persona sys_tipo_persona { get; set; }

        public virtual sys_tipo_identificacion sys_tipo_identificacion { get; set; }
    }
}
