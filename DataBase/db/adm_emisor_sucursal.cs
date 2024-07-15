namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class adm_emisor_sucursal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public adm_emisor_sucursal()
        {
            adm_documento = new HashSet<adm_documento>();
            adm_documento_proveedor = new HashSet<adm_documento_proveedor>();
            adm_emisor_notificacion = new HashSet<adm_emisor_notificacion>();
            adm_emisor_sucursal_json = new HashSet<adm_emisor_sucursal_json>();
            adm_emisor_sucursal_plantilla = new HashSet<adm_emisor_sucursal_plantilla>();
            adm_emisor_sucursal_resolucion = new HashSet<adm_emisor_sucursal_resolucion>();
            adm_usuario = new HashSet<adm_usuario>();
            adm_usuario_sucursal = new HashSet<adm_usuario_sucursal>();
            tmp_documento = new HashSet<tmp_documento>();
        }

        [Key]
        public Guid esu_id { get; set; }

        public Guid esu_emisor { get; set; }

        [Required]
        [StringLength(80)]
        public string esu_nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string esu_abreviatura { get; set; }

        public Guid? esu_departamento { get; set; }

        public Guid? esu_municipio { get; set; }

        [StringLength(100)]
        public string esu_correo { get; set; }

        [StringLength(100)]
        public string esu_direccion { get; set; }

        [StringLength(80)]
        public string esu_codigo_postal { get; set; }

        [StringLength(40)]
        public string esu_telefono { get; set; }

        public Guid? esu_correo_entrada { get; set; }

        public bool esu_activo { get; set; }

        public Guid? esu_correo_salida { get; set; }

        public virtual adm_centinela adm_centinela { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento> adm_documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_documento_proveedor> adm_documento_proveedor { get; set; }

        public virtual adm_emisor adm_emisor { get; set; }

        public virtual adm_emisor_correo adm_emisor_correo { get; set; }

        public virtual adm_emisor_correo adm_emisor_correo1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_notificacion> adm_emisor_notificacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal_json> adm_emisor_sucursal_json { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal_plantilla> adm_emisor_sucursal_plantilla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_emisor_sucursal_resolucion> adm_emisor_sucursal_resolucion { get; set; }

        public virtual sys_departamento sys_departamento { get; set; }

        public virtual sys_municipio sys_municipio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_usuario> adm_usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adm_usuario_sucursal> adm_usuario_sucursal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tmp_documento> tmp_documento { get; set; }
    }
}
