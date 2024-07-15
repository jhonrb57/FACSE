namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_receptor__
    {
        [Key]
        [Column(Order = 0)]
        public Guid rec_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid rec_emisor { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid rec_tipo_receptor { get; set; }

        public Guid? rec_tipo_persona { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid rec_tipo_identificacion { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string rec_identificacion { get; set; }

        [StringLength(5)]
        public string rec_digito { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(200)]
        public string rec_nombre { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(200)]
        public string rec_razon_social { get; set; }

        public Guid? rec_pais { get; set; }

        public Guid? rec_departamento { get; set; }

        public Guid? rec_municipio { get; set; }

        [StringLength(10)]
        public string rec_codigo_postal { get; set; }

        [StringLength(100)]
        public string rec_correo { get; set; }

        [StringLength(150)]
        public string rec_direccion { get; set; }

        [StringLength(50)]
        public string rec_telefono { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime rec_fecha_receccion { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool rec_activo { get; set; }

        [StringLength(20)]
        public string rec_matricula_mercantil { get; set; }

        [StringLength(200)]
        public string rec_contrasena { get; set; }
    }
}
