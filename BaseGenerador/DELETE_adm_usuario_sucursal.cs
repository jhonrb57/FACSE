namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_usuario_sucursal
    {
        [Key]
        [Column(Order = 0)]
        public Guid usu_usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid usu_sucursal { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool usu_activo { get; set; }
    }
}
