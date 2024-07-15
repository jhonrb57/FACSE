namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_file
    {
        [Key]
        public Guid dfi_id { get; set; }

        public Guid dfi_documento { get; set; }

        public string dfi_xml { get; set; }

        public string dfi_json_facse { get; set; }

        public DateTime dfi_fecha { get; set; }

        [StringLength(200)]
        public string dfi_ruta_zip { get; set; }

        public virtual adm_documento adm_documento { get; set; }
    }
}
