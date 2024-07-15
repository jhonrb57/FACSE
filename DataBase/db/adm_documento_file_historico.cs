namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_file_historico
    {
        [Key]
        public Guid dfh_id { get; set; }

        public Guid? dfh_id_documento { get; set; }

        [StringLength(500)]
        public string dfh_xml { get; set; }

        public string dfh_json_facse { get; set; }

        public DateTime? dfi_fecha { get; set; }

        public virtual adm_documento adm_documento { get; set; }
    }
}
