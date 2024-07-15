namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_perfil_usuario_receptor
    {
        [Key]
        [Column(Order = 0)]
        public Guid pur_perfil { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid pur_usuario { get; set; }

        public bool pur_activo { get; set; }

        public virtual adm_perfil adm_perfil { get; set; }
    }
}
