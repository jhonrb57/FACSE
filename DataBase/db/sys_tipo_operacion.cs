namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_operacion
    {
        [Key]
        public Guid top_id { get; set; }

        [Required]
        [StringLength(10)]
        public string top_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string top_descripcion { get; set; }
    }
}
