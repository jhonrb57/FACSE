namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditLogIngreso")]
    public partial class AuditLogIngreso
    {
        [Key]
        public Guid IdAuditLogIngreso { get; set; }

        [StringLength(300)]
        public string Usuario { get; set; }

        public DateTime? Fecha { get; set; }
    }
}
