namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_emisor_certificado
    {
        [Key]
        [Column(Order = 0)]
        public Guid ece_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ece_emisor { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string ece_archivo { get; set; }

        [Key]
        [Column(Order = 3)]
        public string ece_certificado { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime ece_fecha_vegencia { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool ece_activo { get; set; }

        [StringLength(50)]
        public string ece_contrasena { get; set; }
    }
}
