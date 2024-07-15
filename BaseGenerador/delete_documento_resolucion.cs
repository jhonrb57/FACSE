namespace BaseGenerador
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class delete_documento_resolucion
    {
        [Key]
        [Column(Order = 0)]
        public Guid doc_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string doc_prefijo { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid doc_tipo_documento { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "numeric")]
        public decimal doc_numero { get; set; }

        [Key]
        [Column(Order = 4)]
        public Guid doc_emisor { get; set; }

        [Key]
        [Column(Order = 5)]
        public Guid doc_receptor { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string doc_plantilla_version { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime doc_fecha_documento { get; set; }

        [Key]
        [Column(Order = 8)]
        public DateTime doc_fecha_recepcion { get; set; }

        [Key]
        [Column(Order = 9)]
        public DateTime doc_fecha_envio { get; set; }

        [Key]
        [Column(Order = 10)]
        public Guid doc_moneda { get; set; }

        [Key]
        [Column(Order = 11)]
        public Guid doc_sucursal { get; set; }

        [Key]
        [Column(Order = 12)]
        public Guid doc_resolucion { get; set; }

        [StringLength(800)]
        public string doc_cufe { get; set; }

        [StringLength(100)]
        public string doc_clave { get; set; }

        [StringLength(100)]
        public string doc_validacion_dian { get; set; }

        [StringLength(500)]
        public string doc_respuesta_dian { get; set; }

        [StringLength(200)]
        public string doc_zipkey { get; set; }

        [StringLength(200)]
        public string doc_trackid { get; set; }

        [Key]
        [Column(Order = 13)]
        public decimal doc_valor_total { get; set; }

        [Key]
        [Column(Order = 14)]
        public decimal doc_valor_impuestos { get; set; }

        [Key]
        [Column(Order = 15)]
        public Guid doc_estado { get; set; }

        [StringLength(50)]
        public string doc_usuario { get; set; }

        [StringLength(1000)]
        public string doc_observacion { get; set; }

        public Guid? doc_id_estado_facse { get; set; }

        public Guid? doc_emisor_certificado { get; set; }

        public Guid? doc_id_origen_documento { get; set; }

        [Key]
        [Column(Order = 16)]
        public Guid ere_id { get; set; }

        [Key]
        [Column(Order = 17)]
        public Guid ere_emisor { get; set; }

        [Key]
        [Column(Order = 18)]
        public Guid ere_tipo_documento { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(5)]
        public string ere_prefijo { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(500)]
        public string ere_numero_resolucion { get; set; }

        [Key]
        [Column(Order = 21)]
        public DateTime ere_fecha { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(50)]
        public string ere_numero_inicial { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(50)]
        public string ere_numero_final { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(300)]
        public string ere_clave_tecnica { get; set; }

        [Key]
        [Column(Order = 25)]
        public DateTime ere_fecha_inicio { get; set; }

        [Key]
        [Column(Order = 26)]
        public DateTime ere_fecha_final { get; set; }

        [Key]
        [Column(Order = 27)]
        public bool ere_activo { get; set; }
    }
}
