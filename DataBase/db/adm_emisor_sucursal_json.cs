namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_sucursal_json
    {
        [Key]
        public Guid esj_id { get; set; }

        public Guid? esj_id_emisor_sucursal { get; set; }

        [StringLength(200)]
        public string esj_ruta { get; set; }

        public DateTime? esj_fecha { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }
    }
}
