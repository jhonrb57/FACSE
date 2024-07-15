namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_usuario_sucursal
    {
        [Key]
        [Column(Order = 0)]
        public Guid usu_usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid usu_sucursal { get; set; }

        public bool usu_activo { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        public virtual adm_usuario adm_usuario { get; set; }
    }
}
