namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_tipo_retencion_fuente
    {
        [Key]
        public Guid trf_id { get; set; }

        [Required]
        public string trf_concepto_retencion { get; set; }

        public int trf_base_minima_UVT { get; set; }

        public int trf_base_minima_pesos { get; set; }

        [Column(TypeName = "numeric")]
        public decimal trf_porcentajes { get; set; }

        public Guid trf_id_impuesto { get; set; }

        public virtual sys_tipo_impuesto sys_tipo_impuesto { get; set; }
    }
}
