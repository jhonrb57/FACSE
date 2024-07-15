namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_proveedor_anexo
    {
        [Key]
        public Guid dpa_id { get; set; }

        public Guid dpa_documento_proveedor { get; set; }

        [Required]
        [StringLength(200)]
        public string dpa_directorio { get; set; }

        [Required]
        [StringLength(100)]
        public string dpa_nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string dpa_extension { get; set; }

        public virtual adm_documento_proveedor adm_documento_proveedor { get; set; }
    }
}
