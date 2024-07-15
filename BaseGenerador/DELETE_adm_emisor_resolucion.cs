namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_emisor_resolucion
    {
        [Key]
        [Column(Order = 0)]
        public Guid ere_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ere_emisor { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ere_tipo_documento { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string ere_prefijo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(500)]
        public string ere_numero_resolucion { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime ere_fecha { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string ere_numero_inicial { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string ere_numero_final { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(300)]
        public string ere_clave_tecnica { get; set; }

        [Key]
        [Column(Order = 9)]
        public DateTime ere_fecha_inicio { get; set; }

        [Key]
        [Column(Order = 10)]
        public DateTime ere_fecha_final { get; set; }

        [Key]
        [Column(Order = 11)]
        public bool ere_activo { get; set; }
    }
}
