namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_notificacion
    {
        [Key]
        public Guid eno_id { get; set; }

        public Guid eno_id_emisor { get; set; }

        public Guid eno_id_tipo_notificacion { get; set; }

        [Required]
        [StringLength(500)]
        public string eno_descripcion { get; set; }

        public DateTime? eno_fecha { get; set; }

        public bool? eno_leido { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        public virtual sys_tipo_notificacion sys_tipo_notificacion { get; set; }
    }
}
