namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_sucursal_resolucion
    {
        [Key]
        public Guid esr_id { get; set; }

        public Guid esr_emisor { get; set; }

        public Guid esr_sucursal { get; set; }

        public Guid esr_resolucion { get; set; }

        public bool esr_activo { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual adm_emisor_resolucion adm_emisor_resolucion { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }
    }
}
