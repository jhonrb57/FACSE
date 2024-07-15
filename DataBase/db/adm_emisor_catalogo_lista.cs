namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_catalogo_lista
    {
        [Key]
        public Guid ecl_id { get; set; }

        public Guid ecl_emisor_catalogo { get; set; }

        [Required]
        [StringLength(200)]
        public string ecl_descripcion { get; set; }

        public virtual adm_emisor_catalogo adm_emisor_catalogo { get; set; }
    }
}
