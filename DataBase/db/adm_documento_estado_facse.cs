namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_estado_facse
    {
        [Key]
        public Guid def_id { get; set; }

        public Guid? def_id_documento { get; set; }

        public Guid? def_id_estado { get; set; }

        public DateTime? def_fecha { get; set; }

        public virtual adm_documento adm_documento { get; set; }

        public virtual sys_estado_documento_facse sys_estado_documento_facse { get; set; }
    }
}
