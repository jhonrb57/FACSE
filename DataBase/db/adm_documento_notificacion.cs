namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_notificacion
    {
        [Key]
        public Guid dno_id { get; set; }

        public Guid dno_documento { get; set; }

        public DateTime dno_fecha { get; set; }

        public Guid? dno_id_regla { get; set; }

        public virtual adm_documento adm_documento { get; set; }

        public virtual sys_reglas_dian sys_reglas_dian { get; set; }
    }
}
