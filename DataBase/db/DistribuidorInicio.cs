namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DistribuidorInicio")]
    public partial class DistribuidorInicio
    {
        [Key]
        [Column(Order = 0)]
        public Guid emi_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string tid_descripcion { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string emi_identificacion { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(150)]
        public string emi_nombre { get; set; }

        [StringLength(100)]
        public string dep_nombre { get; set; }

        [StringLength(100)]
        public string mun_nombre { get; set; }

        [StringLength(80)]
        public string emi_correo { get; set; }

        [StringLength(50)]
        public string emi_telefono { get; set; }

        public DateTime? emi_fecha_creacion { get; set; }

        public Guid? emi_distribuidor { get; set; }
        public bool emi_correo_automatico { get; set; }
    }
}
