namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_documento_correo
    {
        [Key]
        public Guid dco_id { get; set; }

        public Guid dco_documento { get; set; }

        [Required]
        [StringLength(200)]
        public string dco_correo { get; set; }

        public DateTime dco_fecha { get; set; }

        public Guid? dco_estado { get; set; }

        [StringLength(500)]
        public string dco_mensaje { get; set; }

        public virtual adm_documento adm_documento { get; set; }

        public virtual sys_estado_documento_correo sys_estado_documento_correo { get; set; }
    }
}
