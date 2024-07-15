namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_usuario_perfil_tipo
    {
        [Key]
        [Column(Order = 0)]
        public Guid upt_perfil { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid upt_tipo_usuario { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid upt_usuario { get; set; }

        public Guid? upt_persona { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool upt_activo { get; set; }
    }
}
