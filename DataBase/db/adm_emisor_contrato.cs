namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_contrato
    {
        [Key]
        public Guid eco_emisor { get; set; }

        public Guid eco_tipo_contrato { get; set; }

        public Guid eco_tipo_negociacion { get; set; }

        public DateTime eco_fecha_inicio { get; set; }

        public DateTime eco_fecha_fin { get; set; }

        [Column(TypeName = "numeric")]
        public decimal eco_valor { get; set; }

        public int eco_cantidad_documento { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual sys_tipo_contrato sys_tipo_contrato { get; set; }

        public virtual sys_tipo_negociacion sys_tipo_negociacion { get; set; }
    }
}
