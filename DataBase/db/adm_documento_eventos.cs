namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_eventos
    {
        [Key]
        public Guid dev_id { get; set; }

        public Guid dev_documento { get; set; }

        public Guid dev_evento { get; set; }

        public DateTime dev_fecha { get; set; }

        public virtual adm_documento adm_documento { get; set; }

        public virtual sys_tipo_evento sys_tipo_evento { get; set; }
    }
}
