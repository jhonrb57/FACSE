namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DELETE_adm_emisor
    {
        [Key]
        [Column(Order = 0)]
        public Guid emi_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid emi_grupo { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid emi_tipo_persona { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid emi_tipo_identificacion { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(15)]
        public string emi_identificacion { get; set; }

        [StringLength(1)]
        public string emi_digito { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(150)]
        public string emi_nombre { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(150)]
        public string emi_razon_social { get; set; }

        public Guid? emi_pais { get; set; }

        public Guid? emi_departamento { get; set; }

        public Guid? emi_municipio { get; set; }

        [StringLength(50)]
        public string emi_codigo_posta { get; set; }

        [StringLength(80)]
        public string emi_correo { get; set; }

        [StringLength(200)]
        public string emi_direccion { get; set; }

        [StringLength(50)]
        public string emi_telefono { get; set; }

        public Guid? emi_tipo_emisor { get; set; }

        public Guid? emi_distribuidor { get; set; }

        [StringLength(50)]
        public string emi_logo { get; set; }

        [StringLength(50)]
        public string emi_test_id { get; set; }

        [StringLength(200)]
        public string emi_cliente_token { get; set; }

        [StringLength(200)]
        public string emi_access_token { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool emi_activo { get; set; }

        public DateTime? emi_fecha_creacion { get; set; }

        public Guid? emi_sofware { get; set; }

        public Guid? emi_ciiu { get; set; }

        [StringLength(20)]
        public string emi_matricula_mercantil { get; set; }

        [StringLength(20)]
        public string emi_identificacion_alterna { get; set; }
    }
}
