namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmisorInicioProveedor")]
    public partial class EmisorInicioProveedor
    {
        [StringLength(200)]
        public string dpf_pdf { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid dpr_id { get; set; }

        [StringLength(5)]
        public string tdo_abreviatura { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string dpr_prefijo { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal dpr_numero { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string pro_nombre { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string pro_identificacion { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime dpr_fecha_recepcion { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime dpr_fecha_recibido { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime dpr_fecha_envio { get; set; }

        [Key]
        [Column(Order = 8)]
        public DateTime dpr_fecha_documento { get; set; }

        public DateTime? dpr_fecha_acuse { get; set; }

        [Key]
        [Column(Order = 9)]
        public decimal dpr_valor_total { get; set; }

        [StringLength(50)]
        public string dpr_usuario { get; set; }

        [Key]
        [Column(Order = 10)]
        public Guid dpr_tipo_documento { get; set; }

        [Key]
        [Column(Order = 11)]
        public Guid dpr_sucursal { get; set; }

        [StringLength(200)]
        public string dpf_xml { get; set; }

        [StringLength(200)]
        public string edf_ruta_imagen { get; set; }

        public Guid? dpr_id_estado_facse { get; set; }

        [Key]
        [Column(Order = 12)]
        public bool dpr_acuse { get; set; }

        [StringLength(10)]
        public string edf_codigo { get; set; }

        [StringLength(100)]
        public string edf_descripcion { get; set; }

        public int? anexo { get; set; }
    }
}