namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_usuario()
        {
            adm_emisor_sucursal_plantilla = new HashSet<adm_emisor_sucursal_plantilla>();
            adm_usuario_perfil_tipo = new HashSet<adm_usuario_perfil_tipo>();
            adm_usuario_sucursal = new HashSet<adm_usuario_sucursal>();
            tmp_documento = new HashSet<tmp_documento>();
        }

        [Key]
        public Guid usu_id { get; set; }

        [Required]
        [StringLength(50)]
        public string usu_usuario { get; set; }

        [Required]
        [StringLength(200)]
        public string usu_contrasena { get; set; }

        [Required]
        [StringLength(70)]
        public string usu_nombre { get; set; }

        [StringLength(70)]
        public string usu_apellido { get; set; }

        public bool usu_activo { get; set; }

        public bool usu_directorio_activo { get; set; }

        [StringLength(50)]
        public string usu_email { get; set; }

        [StringLength(100)]
        public string usu_direccion { get; set; }

        [StringLength(50)]
        public string usu_telefono { get; set; }

        public DateTime usu_fecha_creacion { get; set; }

        public Guid? usu_emisor_sucursal { get; set; }

        public virtual adm_emisor_sucursal adm_emisor_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal_plantilla> adm_emisor_sucursal_plantilla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_usuario_perfil_tipo> adm_usuario_perfil_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_usuario_sucursal> adm_usuario_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tmp_documento> tmp_documento { get; set; }
    }
}
