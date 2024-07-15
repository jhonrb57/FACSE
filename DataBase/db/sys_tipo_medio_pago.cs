namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_medio_pago
    {
        [Key]
        public Guid tmp_id { get; set; }

        [Required]
        [StringLength(10)]
        public string tmp_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string tmp_descripcion { get; set; }
    }
}
