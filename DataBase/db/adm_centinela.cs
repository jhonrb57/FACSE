namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_centinela
    {
        [Key]
        public Guid cen_id_emisor_sucursal { get; set; }

        public DateTime cen_fecha { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }
    }
}
