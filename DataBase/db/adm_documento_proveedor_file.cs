namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_proveedor_file
    {
        [Key]
        public Guid dpf_id { get; set; }

        public Guid dpf_documento_proveedor { get; set; }

        [StringLength(200)]
        public string dpf_xml { get; set; }

        [StringLength(200)]
        public string dpf_pdf { get; set; }

        public DateTime dpf_fecha { get; set; }

        [StringLength(200)]
        public string dpf_attached { get; set; }

        public virtual adm_documento_proveedor adm_documento_proveedor { get; set; }
    }
}
