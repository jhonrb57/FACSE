namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_emisor_sucursal_resolucion
    {
        [Key]
        [Column(Order = 0)]
        public Guid esr_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid esr_emisor { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid esr_sucursal { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid esr_resolucion { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool esr_activo { get; set; }
    }
}
