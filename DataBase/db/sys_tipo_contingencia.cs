namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_tipo_contingencia
    {
        [Key]
        public Guid tco_id { get; set; }

        public Guid? tco_id_software { get; set; }

        [Required]
        [StringLength(300)]
        public string tco_descripcion { get; set; }

        [Required]
        [StringLength(10)]
        public string tco_codigo { get; set; }

        public bool tco_activo { get; set; }

        public virtual sys_software sys_software { get; set; }
    }
}
