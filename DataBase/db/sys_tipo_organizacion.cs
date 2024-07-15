namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_organizacion
    {
        [Key]
        public Guid tor_id { get; set; }

        [Required]
        [StringLength(10)]
        public string tor_codigo { get; set; }

        [Required]
        [StringLength(200)]
        public string tor_nombre { get; set; }
    }
}
