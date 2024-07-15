namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_producto_catalogo
    {
        [Key]
        [Column(Order = 0)]
        public Guid epc_producto { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid epc_catalogo { get; set; }

        [StringLength(200)]
        public string epc_valor { get; set; }

        public virtual adm_emisor_catalogo adm_emisor_catalogo { get; set; }

        public virtual adm_emisor_producto adm_emisor_producto { get; set; }
    }
}
