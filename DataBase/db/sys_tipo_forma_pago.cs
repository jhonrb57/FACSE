namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_forma_pago
    {
        [Key]
        public Guid tfp_id { get; set; }

        [Required]
        [StringLength(10)]
        public string tfp_codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string tfp_descripcion { get; set; }
    }
}
