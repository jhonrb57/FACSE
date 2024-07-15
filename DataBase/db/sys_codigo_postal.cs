namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_codigo_postal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cpo_id { get; set; }

        [Required]
        [StringLength(80)]
        public string cpo_codigo { get; set; }
    }
}
