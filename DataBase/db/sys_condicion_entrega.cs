namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_condicion_entrega
    {
        [Key]
        public Guid cen_id { get; set; }

        [Required]
        [StringLength(10)]
        public string cen_codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string cen_descripcion { get; set; }
    }
}
