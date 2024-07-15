namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_documento
    {
        public int tdo_consecutivo { get; set; }

        [Key]
        public Guid tdo_id { get; set; }

        public Guid? tdo_emisor_sucursal { get; set; }

        public Guid tdo_tipo_documento { get; set; }

        [Required]
        public string tdo_json { get; set; }

        public DateTime tdo_fecha_creacion { get; set; }

        public Guid tdo_usuario { get; set; }

        public decimal? tdo_subtotal { get; set; }

        public decimal? tdo_valor_total { get; set; }

        public string tdo_id_receptor { get; set; }

        public decimal? tdo_id_impuesto { get; set; }

        public string tdo_nit { get; set; }

        [StringLength(10)]
        public string tdo_prefijo { get; set; }

        public decimal? tdo_numero { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        public virtual adm_usuario adm_usuario { get; set; }

        public virtual sys_tipo_documento sys_tipo_documento { get; set; }
    }
}
