namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_correcion_nota_debito
    {
        [Key]
        public Guid cnd_id { get; set; }

        [Required]
        [StringLength(10)]
        public string cnd_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string cnd_descripcion { get; set; }
    }
}
