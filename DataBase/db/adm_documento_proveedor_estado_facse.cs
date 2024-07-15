namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_proveedor_estado_facse
    {
        [Key]
        public Guid dpe_id { get; set; }

        public Guid? dpe_documento_proveedor { get; set; }

        public Guid? dpe_id_estado { get; set; }

        public DateTime? dpe_fecha { get; set; }

        public virtual adm_documento_proveedor adm_documento_proveedor { get; set; }

        public virtual sys_estado_documento_facse sys_estado_documento_facse { get; set; }
    }
}
