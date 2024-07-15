namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_impuesto
    {
        [Key]
        [Column(Order = 0)]
        public Guid emi_id_emisor { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid emi_id_impuesto { get; set; }

        public DateTime? emi_fecha { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual sys_tipo_impuesto sys_tipo_impuesto { get; set; }
    }
}
