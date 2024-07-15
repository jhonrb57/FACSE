namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class log_receptor_nombre
    {
        [Key]
        public Guid rno_id { get; set; }

        public Guid? rno_id_receptor { get; set; }

        public Guid? rno_id_documento { get; set; }

        [StringLength(200)]
        public string rno_nombre { get; set; }

        public DateTime? rno_fecha { get; set; }

        public virtual adm_documento adm_documento { get; set; }

        public virtual adm_receptor adm_receptor { get; set; }
    }
}
