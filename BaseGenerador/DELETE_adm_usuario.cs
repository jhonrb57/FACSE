namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_usuario
    {
        [Key]
        [Column(Order = 0)]
        public Guid usu_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string usu_usuario { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string usu_contrasena { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(70)]
        public string usu_nombre { get; set; }

        [StringLength(70)]
        public string usu_apellido { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool usu_activo { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool usu_directorio_activo { get; set; }

        [StringLength(50)]
        public string usu_email { get; set; }

        [StringLength(50)]
        public string usu_direccion { get; set; }

        [StringLength(50)]
        public string usu_telefono { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime usu_fecha_creacion { get; set; }

        public Guid? usu_emisor_sucursal { get; set; }
    }
}
