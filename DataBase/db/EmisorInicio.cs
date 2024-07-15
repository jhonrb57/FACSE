namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmisorInicio")]
    public partial class EmisorInicio
    {
        public string dfi_json_facse { get; set; }

        [StringLength(5)]
        public string ted_codigo { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid doc_id { get; set; }

        [StringLength(5)]
        public string tdo_abreviatura { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string doc_prefijo { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal doc_numero { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string rec_nombre { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string rec_identificacion { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime doc_fecha_recepcion { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime doc_fecha_envio { get; set; }

        [Key]
        [Column(Order = 7)]
        public decimal doc_valor_total { get; set; }

        [StringLength(50)]
        public string doc_usuario { get; set; }

        [Key]
        [Column(Order = 8)]
        public Guid doc_tipo_documento { get; set; }

        [Key]
        [Column(Order = 9)]
        public Guid doc_sucursal { get; set; }

        public string dfi_xml { get; set; }

        [StringLength(200)]
        public string edf_ruta_imagen { get; set; }

        [Key]
        [Column(Order = 10)]
        public Guid doc_estado { get; set; }

        public Guid? doc_id_estado_facse { get; set; }

        [StringLength(100)]
        public string edf_descripcion { get; set; }

        public int? notificacion { get; set; }

        public int? correo { get; set; }

        public int? anexo { get; set; }
    }
}
