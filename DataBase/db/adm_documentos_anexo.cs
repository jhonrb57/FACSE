namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documentos_anexo
    {
        [Key]
        public Guid dan_id { get; set; }

        public Guid dan_documento { get; set; }

        [Required]
        [StringLength(200)]
        public string dan_directorio { get; set; }

        [Required]
        [StringLength(100)]
        public string dan_nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string dan_extension { get; set; }

        public virtual adm_documento adm_documento { get; set; }
    }
}
