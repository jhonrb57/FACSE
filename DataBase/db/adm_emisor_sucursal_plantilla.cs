namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_sucursal_plantilla
    {
        [Key]
        public Guid esp_id { get; set; }

        public Guid esp_emisor_sucursal { get; set; }

        [Required]
        public string esp_logo { get; set; }

        [Required]
        [StringLength(2000)]
        public string esp_primer_mensaje { get; set; }

        [StringLength(2000)]
        public string esp_segundo_mensaje { get; set; }

        [StringLength(2000)]
        public string esp_tercer_mensaje { get; set; }

        public Guid esp_usuario_creacion { get; set; }

        public DateTime esp_fecha_creacion { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        public virtual adm_usuario adm_usuario { get; set; }
    }
}
