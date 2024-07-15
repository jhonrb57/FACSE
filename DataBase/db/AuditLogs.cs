namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuditLogs
    {
        [Key]
        [StringLength(38)]
        public string IdAuditLog { get; set; }

        [StringLength(255)]
        public string IpUsuario { get; set; }

        [StringLength(255)]
        public string Usuario { get; set; }

        public DateTime? Fecha { get; set; }

        [StringLength(255)]
        public string Accion { get; set; }

        [StringLength(255)]
        public string Tabla { get; set; }

        [StringLength(255)]
        public string RegistroTabla { get; set; }

        [StringLength(255)]
        public string Campo { get; set; }

        public string ValorAnterior { get; set; }

        public string ValorNuevo { get; set; }

        [StringLength(255)]
        public string PCName { get; set; }

        [StringLength(255)]
        public string CreatedBy { get; set; }

        public DateTimeOffset? Created { get; set; }

        [StringLength(255)]
        public string ModifiedBy { get; set; }

        public DateTimeOffset? Modified { get; set; }
    }
}
