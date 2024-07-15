namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_documento_file
    {
        [Key]
        [Column(Order = 0)]
        public Guid dfi_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid dfi_documento { get; set; }

        public string dfi_xml { get; set; }

        public string dfi_json_facse { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime dfi_fecha { get; set; }

        [StringLength(200)]
        public string dfi_ruta_zip { get; set; }
    }
}
