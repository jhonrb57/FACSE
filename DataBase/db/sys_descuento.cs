namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_descuento
    {
        [Key]
        public Guid des_id { get; set; }

        [Required]
        [StringLength(10)]
        public string des_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string des_descripcion { get; set; }
    }
}
