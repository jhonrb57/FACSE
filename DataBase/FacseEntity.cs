
namespace DataBase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System.Net;
    using DataBase;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Infrastructure;

    public partial class FacseEntity : DbContext
    {
        public FacseEntity()
             : base("name=FacseEntity")
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 180;
        }
        public virtual DbSet<adm_centinela> adm_centinela { get; set; }
        public virtual DbSet<adm_distriduidor> adm_distriduidor { get; set; }
        public virtual DbSet<adm_documento> adm_documento { get; set; }
        public virtual DbSet<adm_documento_correo> adm_documento_correo { get; set; }
        public virtual DbSet<adm_documento_estado_facse> adm_documento_estado_facse { get; set; }
        public virtual DbSet<adm_documento_eventos> adm_documento_eventos { get; set; }
        public virtual DbSet<adm_documento_file> adm_documento_file { get; set; }
        public virtual DbSet<adm_documento_file_historico> adm_documento_file_historico { get; set; }
        public virtual DbSet<adm_documento_notificacion> adm_documento_notificacion { get; set; }
        public virtual DbSet<adm_documento_proveedor> adm_documento_proveedor { get; set; }
        public virtual DbSet<adm_documento_proveedor_anexo> adm_documento_proveedor_anexo { get; set; }
        public virtual DbSet<adm_documento_proveedor_estado_facse> adm_documento_proveedor_estado_facse { get; set; }
        public virtual DbSet<adm_documento_proveedor_file> adm_documento_proveedor_file { get; set; }
        public virtual DbSet<adm_documentos_anexo> adm_documentos_anexo { get; set; }
        public virtual DbSet<adm_emisor> adm_emisor { get; set; }
        public virtual DbSet<adm_emisor_catalogo> adm_emisor_catalogo { get; set; }
        public virtual DbSet<adm_emisor_catalogo_lista> adm_emisor_catalogo_lista { get; set; }
        public virtual DbSet<adm_emisor_certificado> adm_emisor_certificado { get; set; }
        public virtual DbSet<adm_emisor_contrato> adm_emisor_contrato { get; set; }
        public virtual DbSet<adm_emisor_correo> adm_emisor_correo { get; set; }
        public virtual DbSet<adm_emisor_impuesto> adm_emisor_impuesto { get; set; }
        public virtual DbSet<adm_emisor_notificacion> adm_emisor_notificacion { get; set; }
        public virtual DbSet<adm_emisor_producto> adm_emisor_producto { get; set; }
        public virtual DbSet<adm_emisor_producto_catalogo> adm_emisor_producto_catalogo { get; set; }
        public virtual DbSet<adm_emisor_resolucion> adm_emisor_resolucion { get; set; }
        public virtual DbSet<adm_emisor_sucursal> adm_emisor_sucursal { get; set; }
        public virtual DbSet<adm_emisor_sucursal_json> adm_emisor_sucursal_json { get; set; }
        public virtual DbSet<adm_emisor_sucursal_plantilla> adm_emisor_sucursal_plantilla { get; set; }
        public virtual DbSet<adm_emisor_sucursal_resolucion> adm_emisor_sucursal_resolucion { get; set; }
        public virtual DbSet<adm_grupo_emisor> adm_grupo_emisor { get; set; }
        public virtual DbSet<adm_mandato> adm_mandato { get; set; }
        public virtual DbSet<adm_perfil> adm_perfil { get; set; }
        public virtual DbSet<adm_perfil_usuario_receptor> adm_perfil_usuario_receptor { get; set; }
        public virtual DbSet<adm_plantilla> adm_plantilla { get; set; }
        public virtual DbSet<adm_proveedor> adm_proveedor { get; set; }
        public virtual DbSet<adm_receptor> adm_receptor { get; set; }
        public virtual DbSet<adm_tipo_retencion_fuente> adm_tipo_retencion_fuente { get; set; }
        public virtual DbSet<adm_usuario> adm_usuario { get; set; }
        public virtual DbSet<adm_usuario_perfil_tipo> adm_usuario_perfil_tipo { get; set; }
        public virtual DbSet<adm_usuario_receptor> adm_usuario_receptor { get; set; }
        public virtual DbSet<adm_usuario_sucursal> adm_usuario_sucursal { get; set; }
        public virtual DbSet<AuditLogIngreso> AuditLogIngreso { get; set; }
        public virtual DbSet<AuditLogReceptor> AuditLogReceptor { get; set; }
        public virtual DbSet<AuditLogs> AuditLogs { get; set; }
        public virtual DbSet<log_receptor_nombre> log_receptor_nombre { get; set; }
        public virtual DbSet<sys_ciiu> sys_ciiu { get; set; }
        public virtual DbSet<sys_codigo_postal> sys_codigo_postal { get; set; }
        public virtual DbSet<sys_condicion_entrega> sys_condicion_entrega { get; set; }
        public virtual DbSet<sys_departamento> sys_departamento { get; set; }
        public virtual DbSet<sys_descuento> sys_descuento { get; set; }
        public virtual DbSet<sys_estado_documento> sys_estado_documento { get; set; }
        public virtual DbSet<sys_estado_documento_correo> sys_estado_documento_correo { get; set; }
        public virtual DbSet<sys_estado_documento_facse> sys_estado_documento_facse { get; set; }
        public virtual DbSet<sys_moneda> sys_moneda { get; set; }
        public virtual DbSet<sys_municipio> sys_municipio { get; set; }
        public virtual DbSet<sys_origen_documento> sys_origen_documento { get; set; }
        public virtual DbSet<sys_pais> sys_pais { get; set; }
        public virtual DbSet<sys_reglas_dian> sys_reglas_dian { get; set; }
        public virtual DbSet<sys_software> sys_software { get; set; }
        public virtual DbSet<sys_tipo_accion> sys_tipo_accion { get; set; }
        public virtual DbSet<sys_tipo_catalogo> sys_tipo_catalogo { get; set; }
        public virtual DbSet<sys_tipo_clasificacion_impuesto> sys_tipo_clasificacion_impuesto { get; set; }
        public virtual DbSet<sys_tipo_contingencia> sys_tipo_contingencia { get; set; }
        public virtual DbSet<sys_tipo_contrato> sys_tipo_contrato { get; set; }
        public virtual DbSet<sys_tipo_correcion_nota_credito> sys_tipo_correcion_nota_credito { get; set; }
        public virtual DbSet<sys_tipo_correcion_nota_debito> sys_tipo_correcion_nota_debito { get; set; }
        public virtual DbSet<sys_tipo_correo> sys_tipo_correo { get; set; }
        public virtual DbSet<sys_tipo_dato> sys_tipo_dato { get; set; }
        public virtual DbSet<sys_tipo_documento> sys_tipo_documento { get; set; }
        public virtual DbSet<sys_tipo_emisor> sys_tipo_emisor { get; set; }
        public virtual DbSet<sys_tipo_evento> sys_tipo_evento { get; set; }
        public virtual DbSet<sys_tipo_forma_pago> sys_tipo_forma_pago { get; set; }
        public virtual DbSet<sys_tipo_identificacion> sys_tipo_identificacion { get; set; }
        public virtual DbSet<sys_tipo_impuesto> sys_tipo_impuesto { get; set; }
        public virtual DbSet<sys_tipo_medio_pago> sys_tipo_medio_pago { get; set; }
        public virtual DbSet<sys_tipo_negociacion> sys_tipo_negociacion { get; set; }
        public virtual DbSet<sys_tipo_notificacion> sys_tipo_notificacion { get; set; }
        public virtual DbSet<sys_tipo_operacion> sys_tipo_operacion { get; set; }
        public virtual DbSet<sys_tipo_organizacion> sys_tipo_organizacion { get; set; }
        public virtual DbSet<sys_tipo_persona> sys_tipo_persona { get; set; }
        public virtual DbSet<sys_tipo_unidad_cantidad> sys_tipo_unidad_cantidad { get; set; }
        public virtual DbSet<sys_tipo_usuario> sys_tipo_usuario { get; set; }
        public virtual DbSet<tmp_documento> tmp_documento { get; set; }



        public virtual DbSet<DistribuidorInicio> DistribuidorInicio { get; set; }
        public virtual DbSet<EmisorInicio> EmisorInicio { get; set; }
        public virtual DbSet<EmisorInicioProveedor> EmisorInicioProveedor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<adm_distriduidor>()
                .Property(e => e.dis_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_distriduidor>()
                .Property(e => e.dis_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_distriduidor>()
                .Property(e => e.dis_email)
                .IsUnicode(false);

            modelBuilder.Entity<adm_distriduidor>()
                .Property(e => e.dis_contacto)
                .IsUnicode(false);

            modelBuilder.Entity<adm_distriduidor>()
                .Property(e => e.dis_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_distriduidor>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.adm_distriduidor)
                .HasForeignKey(e => e.emi_distribuidor);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_prefijo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_numero)
                .HasPrecision(18, 0);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_plantilla_version)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_cufe)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_clave)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_validacion_dian)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_respuesta_dian)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_zipkey)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_trackid)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .Property(e => e.doc_observacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documento_file_historico)
                .WithOptional(e => e.adm_documento)
                .HasForeignKey(e => e.dfh_id_documento);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documento_correo)
                .WithRequired(e => e.adm_documento)
                .HasForeignKey(e => e.dco_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documento_estado_facse)
                .WithOptional(e => e.adm_documento)
                .HasForeignKey(e => e.def_id_documento);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documento_eventos)
                .WithRequired(e => e.adm_documento)
                .HasForeignKey(e => e.dev_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documento_file)
                .WithRequired(e => e.adm_documento)
                .HasForeignKey(e => e.dfi_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documento_notificacion)
                .WithRequired(e => e.adm_documento)
                .HasForeignKey(e => e.dno_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.adm_documentos_anexo)
                .WithRequired(e => e.adm_documento)
                .HasForeignKey(e => e.dan_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento>()
                .HasMany(e => e.log_receptor_nombre)
                .WithOptional(e => e.adm_documento)
                .HasForeignKey(e => e.rno_id_documento);

            modelBuilder.Entity<adm_documento_correo>()
                .Property(e => e.dco_correo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_correo>()
                .Property(e => e.dco_mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_file>()
                .Property(e => e.dfi_xml)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_file>()
                .Property(e => e.dfi_json_facse)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_file>()
                .Property(e => e.dfi_ruta_zip)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_file_historico>()
                .Property(e => e.dfh_xml)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_file_historico>()
                .Property(e => e.dfh_json_facse)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_prefijo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_numero)
                .HasPrecision(18, 0);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_plantilla_version)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_cufe)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_clave)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_validacion_dian)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_respuesta_dian)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_zipkey)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_trackid)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .Property(e => e.dpr_observacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .HasMany(e => e.adm_documento_proveedor_anexo)
                .WithRequired(e => e.adm_documento_proveedor)
                .HasForeignKey(e => e.dpa_documento_proveedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento_proveedor>()
                .HasMany(e => e.adm_documento_proveedor_estado_facse)
                .WithOptional(e => e.adm_documento_proveedor)
                .HasForeignKey(e => e.dpe_documento_proveedor);

            modelBuilder.Entity<adm_documento_proveedor>()
                .HasMany(e => e.adm_documento_proveedor_file)
                .WithRequired(e => e.adm_documento_proveedor)
                .HasForeignKey(e => e.dpf_documento_proveedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_documento_proveedor_anexo>()
                .Property(e => e.dpa_directorio)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor_anexo>()
                .Property(e => e.dpa_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor_anexo>()
                .Property(e => e.dpa_extension)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor_file>()
                .Property(e => e.dpf_xml)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor_file>()
                .Property(e => e.dpf_pdf)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documento_proveedor_file>()
                .Property(e => e.dpf_attached)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documentos_anexo>()
                .Property(e => e.dan_directorio)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documentos_anexo>()
                .Property(e => e.dan_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_documentos_anexo>()
                .Property(e => e.dan_extension)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_digito)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_razon_social)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_codigo_posta)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_correo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_logo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_test_id)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_cliente_token)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_access_token)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_matricula_mercantil)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .Property(e => e.emi_identificacion_alterna)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.doc_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.dpr_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.adm_emisor)
                .HasForeignKey(e => e.alc_emisor_accion);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_catalogo)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.eca_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_certificado)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.ece_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasOptional(e => e.adm_emisor_contrato)
                .WithRequired(e => e.adm_emisor);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_correo)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.eco_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_impuesto)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.emi_id_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_producto)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.epr_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_resolucion)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.ere_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_sucursal)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.esu_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_emisor_sucursal_resolucion)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.esr_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_proveedor)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.pro_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor>()
                .HasMany(e => e.adm_receptor)
                .WithRequired(e => e.adm_emisor)
                .HasForeignKey(e => e.rec_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_catalogo>()
                .Property(e => e.eca_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_catalogo>()
                .HasMany(e => e.adm_emisor_catalogo_lista)
                .WithRequired(e => e.adm_emisor_catalogo)
                .HasForeignKey(e => e.ecl_emisor_catalogo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_catalogo>()
                .HasMany(e => e.adm_emisor_producto_catalogo)
                .WithRequired(e => e.adm_emisor_catalogo)
                .HasForeignKey(e => e.epc_catalogo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_catalogo_lista>()
                .Property(e => e.ecl_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_certificado>()
                .Property(e => e.ece_archivo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_certificado>()
                .Property(e => e.ece_certificado)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_certificado>()
                .Property(e => e.ece_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_certificado>()
                .HasMany(e => e.adm_documento)
                .WithOptional(e => e.adm_emisor_certificado)
                .HasForeignKey(e => e.doc_emisor_certificado);

            modelBuilder.Entity<adm_emisor_contrato>()
                .Property(e => e.eco_valor)
                .HasPrecision(18, 0);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_servidor)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_puerto)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_correo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_correo_html)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .Property(e => e.eco_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_correo>()
                .HasMany(e => e.adm_emisor_sucursal)
                .WithOptional(e => e.adm_emisor_correo)
                .HasForeignKey(e => e.esu_correo_entrada);

            modelBuilder.Entity<adm_emisor_correo>()
                .HasMany(e => e.adm_emisor_sucursal1)
                .WithOptional(e => e.adm_emisor_correo1)
                .HasForeignKey(e => e.esu_correo_salida);

            modelBuilder.Entity<adm_emisor_notificacion>()
                .Property(e => e.eno_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_producto>()
                .Property(e => e.epr_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_producto>()
                .Property(e => e.epr_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_producto>()
                .HasMany(e => e.adm_emisor_producto_catalogo)
                .WithRequired(e => e.adm_emisor_producto)
                .HasForeignKey(e => e.epc_producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_producto_catalogo>()
                .Property(e => e.epc_valor)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .Property(e => e.ere_prefijo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .Property(e => e.ere_numero_resolucion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .Property(e => e.ere_numero_inicial)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .Property(e => e.ere_numero_final)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .Property(e => e.ere_clave_tecnica)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .Property(e => e.ere_plantilla_version)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.adm_emisor_resolucion)
                .HasForeignKey(e => e.doc_resolucion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_resolucion>()
                .HasMany(e => e.adm_emisor_sucursal_resolucion)
                .WithRequired(e => e.adm_emisor_resolucion)
                .HasForeignKey(e => e.esr_resolucion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .Property(e => e.esu_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .Property(e => e.esu_abreviatura)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .Property(e => e.esu_correo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .Property(e => e.esu_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .Property(e => e.esu_codigo_postal)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .Property(e => e.esu_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasOptional(e => e.adm_centinela)
                .WithRequired(e => e.adm_emisor_sucursal);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.doc_sucursal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithRequired(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.dpr_sucursal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_emisor_notificacion)
                .WithRequired(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.eno_id_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_emisor_sucursal_json)
                .WithOptional(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.esj_id_emisor_sucursal);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_emisor_sucursal_plantilla)
                .WithRequired(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.esp_emisor_sucursal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_emisor_sucursal_resolucion)
                .WithRequired(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.esr_sucursal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_usuario)
                .WithOptional(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.usu_emisor_sucursal);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.adm_usuario_sucursal)
                .WithRequired(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.usu_sucursal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_emisor_sucursal>()
                .HasMany(e => e.tmp_documento)
                .WithOptional(e => e.adm_emisor_sucursal)
                .HasForeignKey(e => e.tdo_emisor_sucursal);

            modelBuilder.Entity<adm_emisor_sucursal_json>()
                .Property(e => e.esj_ruta)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal_plantilla>()
                .Property(e => e.esp_logo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal_plantilla>()
                .Property(e => e.esp_primer_mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal_plantilla>()
                .Property(e => e.esp_segundo_mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<adm_emisor_sucursal_plantilla>()
                .Property(e => e.esp_tercer_mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<adm_grupo_emisor>()
                .Property(e => e.gen_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_grupo_emisor>()
                .Property(e => e.gen_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_grupo_emisor>()
                .Property(e => e.gen_email)
                .IsUnicode(false);

            modelBuilder.Entity<adm_grupo_emisor>()
                .Property(e => e.gen_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_grupo_emisor>()
                .HasMany(e => e.adm_emisor)
                .WithRequired(e => e.adm_grupo_emisor)
                .HasForeignKey(e => e.emi_grupo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_mandato>()
                .Property(e => e.man_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_mandato>()
                .Property(e => e.man_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_mandato>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.adm_mandato)
                .HasForeignKey(e => e.emi_mandato);

            modelBuilder.Entity<adm_perfil>()
                .Property(e => e.per_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_perfil>()
                .HasMany(e => e.adm_usuario_perfil_tipo)
                .WithRequired(e => e.adm_perfil)
                .HasForeignKey(e => e.upt_perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_perfil>()
                .HasMany(e => e.adm_perfil_usuario_receptor)
                .WithRequired(e => e.adm_perfil)
                .HasForeignKey(e => e.pur_perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_plantilla>()
                .Property(e => e.prg_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_plantilla>()
                .Property(e => e.prg_direccion_rdlc)
                .IsUnicode(false);

            modelBuilder.Entity<adm_plantilla>()
                .Property(e => e.prg_direccion_pdf)
                .IsUnicode(false);

            modelBuilder.Entity<adm_plantilla>()
                .HasMany(e => e.adm_emisor_resolucion)
                .WithOptional(e => e.adm_plantilla)
                .HasForeignKey(e => e.ere_plantilla);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_tipo_receptor)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_digito)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_razon_social)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_codigo_postal)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_correo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .Property(e => e.pro_matricula_mercantil)
                .IsUnicode(false);

            modelBuilder.Entity<adm_proveedor>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithRequired(e => e.adm_proveedor)
                .HasForeignKey(e => e.dpr_proveedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_tipo_receptor)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_digito)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_razon_social)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_codigo_postal)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_correo)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_matricula_mercantil)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .Property(e => e.rec_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<adm_receptor>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.adm_receptor)
                .HasForeignKey(e => e.doc_receptor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_receptor>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.adm_receptor)
                .HasForeignKey(e => e.alc_receptor);

            modelBuilder.Entity<adm_receptor>()
                .HasMany(e => e.log_receptor_nombre)
                .WithOptional(e => e.adm_receptor)
                .HasForeignKey(e => e.rno_id_receptor);

            modelBuilder.Entity<adm_tipo_retencion_fuente>()
                .Property(e => e.trf_concepto_retencion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_tipo_retencion_fuente>()
            .Property(e => e.trf_porcentajes)
            .HasPrecision(18, 5);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_apellido)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_email)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .Property(e => e.usu_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario>()
                .HasMany(e => e.adm_emisor_sucursal_plantilla)
                .WithRequired(e => e.adm_usuario)
                .HasForeignKey(e => e.esp_usuario_creacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_usuario>()
                .HasMany(e => e.adm_usuario_perfil_tipo)
                .WithRequired(e => e.adm_usuario)
                .HasForeignKey(e => e.upt_usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_usuario>()
                .HasMany(e => e.adm_usuario_sucursal)
                .WithRequired(e => e.adm_usuario)
                .HasForeignKey(e => e.usu_usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_usuario>()
                .HasMany(e => e.tmp_documento)
                .WithRequired(e => e.adm_usuario)
                .HasForeignKey(e => e.tdo_usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_apellido)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_email)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .Property(e => e.ure_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<adm_usuario_receptor>()
                .HasMany(e => e.adm_receptor)
                .WithOptional(e => e.adm_usuario_receptor)
                .HasForeignKey(e => e.rec_usuario_receptor);

            modelBuilder.Entity<AuditLogIngreso>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_tipo_receptor)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_digito)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_razon_social)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_codigo_postal)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_correo)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_direccion)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogReceptor>()
                .Property(e => e.alc_matricula_mercantil)
                .IsUnicode(false);

            modelBuilder.Entity<AuditLogs>()
                .Property(e => e.IdAuditLog)
                .IsUnicode(false);

            modelBuilder.Entity<log_receptor_nombre>()
                .Property(e => e.rno_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_ciiu>()
                .Property(e => e.cii_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_ciiu>()
                .Property(e => e.cii_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_ciiu>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.sys_ciiu)
                .HasForeignKey(e => e.emi_ciiu);

            modelBuilder.Entity<sys_codigo_postal>()
                .Property(e => e.cpo_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_condicion_entrega>()
                .Property(e => e.cen_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_condicion_entrega>()
                .Property(e => e.cen_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_departamento>()
                .Property(e => e.dep_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_departamento>()
                .Property(e => e.dep_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_departamento>()
                .Property(e => e.dep_iso)
                .IsUnicode(false);

            modelBuilder.Entity<sys_departamento>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.sys_departamento)
                .HasForeignKey(e => e.emi_departamento);

            modelBuilder.Entity<sys_departamento>()
                .HasMany(e => e.adm_emisor_sucursal)
                .WithOptional(e => e.sys_departamento)
                .HasForeignKey(e => e.esu_departamento);

            modelBuilder.Entity<sys_departamento>()
                .HasMany(e => e.adm_proveedor)
                .WithOptional(e => e.sys_departamento)
                .HasForeignKey(e => e.pro_departamento);

            modelBuilder.Entity<sys_departamento>()
                .HasMany(e => e.adm_receptor)
                .WithOptional(e => e.sys_departamento)
                .HasForeignKey(e => e.rec_departamento);

            modelBuilder.Entity<sys_departamento>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.sys_departamento)
                .HasForeignKey(e => e.alc_departamento);

            modelBuilder.Entity<sys_departamento>()
                .HasMany(e => e.sys_municipio)
                .WithOptional(e => e.sys_departamento)
                .HasForeignKey(e => e.mun_id_dpto);

            modelBuilder.Entity<sys_descuento>()
                .Property(e => e.des_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_descuento>()
                .Property(e => e.des_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento>()
                .Property(e => e.ted_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento>()
                .Property(e => e.ted_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.sys_estado_documento)
                .HasForeignKey(e => e.doc_estado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_estado_documento_correo>()
                .Property(e => e.edc_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento_correo>()
                .HasMany(e => e.adm_documento_correo)
                .WithOptional(e => e.sys_estado_documento_correo)
                .HasForeignKey(e => e.dco_estado);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .Property(e => e.edf_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .Property(e => e.edf_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .Property(e => e.edf_ruta_imagen)
                .IsUnicode(false);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .HasMany(e => e.adm_documento)
                .WithOptional(e => e.sys_estado_documento_facse)
                .HasForeignKey(e => e.doc_id_estado_facse);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .HasMany(e => e.adm_documento_estado_facse)
                .WithOptional(e => e.sys_estado_documento_facse)
                .HasForeignKey(e => e.def_id_estado);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithOptional(e => e.sys_estado_documento_facse)
                .HasForeignKey(e => e.dpr_id_estado_facse);

            modelBuilder.Entity<sys_estado_documento_facse>()
                .HasMany(e => e.adm_documento_proveedor_estado_facse)
                .WithOptional(e => e.sys_estado_documento_facse)
                .HasForeignKey(e => e.dpe_id_estado);

            modelBuilder.Entity<sys_moneda>()
                .Property(e => e.mon_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_moneda>()
                .Property(e => e.mon_divisa)
                .IsUnicode(false);

            modelBuilder.Entity<sys_moneda>()
                .Property(e => e.mon_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_moneda>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.sys_moneda)
                .HasForeignKey(e => e.doc_moneda)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_moneda>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithRequired(e => e.sys_moneda)
                .HasForeignKey(e => e.dpr_moneda)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_municipio>()
                .Property(e => e.mun_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_municipio>()
                .Property(e => e.mun_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.adm_distriduidor)
                .WithRequired(e => e.sys_municipio)
                .HasForeignKey(e => e.dis_municipio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.sys_municipio)
                .HasForeignKey(e => e.emi_municipio);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.adm_emisor_sucursal)
                .WithOptional(e => e.sys_municipio)
                .HasForeignKey(e => e.esu_municipio);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.adm_grupo_emisor)
                .WithRequired(e => e.sys_municipio)
                .HasForeignKey(e => e.gen_municipio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.adm_proveedor)
                .WithOptional(e => e.sys_municipio)
                .HasForeignKey(e => e.pro_municipio);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.adm_receptor)
                .WithOptional(e => e.sys_municipio)
                .HasForeignKey(e => e.rec_municipio);

            modelBuilder.Entity<sys_municipio>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.sys_municipio)
                .HasForeignKey(e => e.alc_municipio);

            modelBuilder.Entity<sys_origen_documento>()
                .Property(e => e.odo_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_origen_documento>()
                .HasMany(e => e.adm_documento)
                .WithOptional(e => e.sys_origen_documento)
                .HasForeignKey(e => e.doc_id_origen_documento);

            modelBuilder.Entity<sys_origen_documento>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithOptional(e => e.sys_origen_documento)
                .HasForeignKey(e => e.dpr_id_origen_documento);

            modelBuilder.Entity<sys_pais>()
                .Property(e => e.pai_nombre_comun)
                .IsUnicode(false);

            modelBuilder.Entity<sys_pais>()
                .Property(e => e.pai_nombre_iso)
                .IsUnicode(false);

            modelBuilder.Entity<sys_pais>()
                .Property(e => e.pai_codigo_2)
                .IsUnicode(false);

            modelBuilder.Entity<sys_pais>()
                .Property(e => e.pai_codigo_3)
                .IsUnicode(false);

            modelBuilder.Entity<sys_pais>()
                .Property(e => e.pai_codigo_numerico)
                .IsUnicode(false);

            modelBuilder.Entity<sys_pais>()
                .Property(e => e.pai_observacion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_pais>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.sys_pais)
                .HasForeignKey(e => e.emi_pais);

            modelBuilder.Entity<sys_pais>()
                .HasMany(e => e.adm_proveedor)
                .WithOptional(e => e.sys_pais)
                .HasForeignKey(e => e.pro_pais);

            modelBuilder.Entity<sys_pais>()
                .HasMany(e => e.adm_receptor)
                .WithOptional(e => e.sys_pais)
                .HasForeignKey(e => e.rec_pais);

            modelBuilder.Entity<sys_pais>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.sys_pais)
                .HasForeignKey(e => e.alc_pais);

            modelBuilder.Entity<sys_pais>()
                .HasMany(e => e.sys_departamento)
                .WithOptional(e => e.sys_pais)
                .HasForeignKey(e => e.dep_id_pais);

            modelBuilder.Entity<sys_pais>()
                .HasMany(e => e.sys_municipio)
                .WithOptional(e => e.sys_pais)
                .HasForeignKey(e => e.mun_id_pais);

            modelBuilder.Entity<sys_reglas_dian>()
                .Property(e => e.rdi_regla)
                .IsUnicode(false);

            modelBuilder.Entity<sys_reglas_dian>()
                .Property(e => e.rdi_descripcion_dian)
                .IsUnicode(false);

            modelBuilder.Entity<sys_reglas_dian>()
                .Property(e => e.rdi_descripcion_facse)
                .IsUnicode(false);

            modelBuilder.Entity<sys_reglas_dian>()
                .HasMany(e => e.adm_documento_notificacion)
                .WithOptional(e => e.sys_reglas_dian)
                .HasForeignKey(e => e.dno_id_regla);

            modelBuilder.Entity<sys_software>()
                .Property(e => e.sof_pin)
                .IsUnicode(false);

            modelBuilder.Entity<sys_software>()
                .Property(e => e.sof_url)
                .IsUnicode(false);

            modelBuilder.Entity<sys_software>()
                .Property(e => e.sof_contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<sys_software>()
                .Property(e => e.sof_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_software>()
                .Property(e => e.sof_id_sofware)
                .IsUnicode(false);

            modelBuilder.Entity<sys_software>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.sys_software)
                .HasForeignKey(e => e.emi_sofware);

            modelBuilder.Entity<sys_software>()
                .HasMany(e => e.sys_tipo_contingencia)
                .WithOptional(e => e.sys_software)
                .HasForeignKey(e => e.tco_id_software);

            modelBuilder.Entity<sys_tipo_accion>()
                .Property(e => e.tac_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_accion>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.sys_tipo_accion)
                .HasForeignKey(e => e.alc_tipo_accion);

            modelBuilder.Entity<sys_tipo_catalogo>()
                .Property(e => e.tca_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_catalogo>()
                .HasMany(e => e.adm_emisor_catalogo)
                .WithRequired(e => e.sys_tipo_catalogo)
                .HasForeignKey(e => e.eca_tipo_catalogo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_clasificacion_impuesto>()
                .Property(e => e.tci_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_clasificacion_impuesto>()
                .HasMany(e => e.sys_tipo_impuesto)
                .WithOptional(e => e.sys_tipo_clasificacion_impuesto)
                .HasForeignKey(e => e.tim_clasificacion);

            modelBuilder.Entity<sys_tipo_contingencia>()
                .Property(e => e.tco_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_contingencia>()
                .Property(e => e.tco_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_contrato>()
                .Property(e => e.tco_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_contrato>()
                .HasMany(e => e.adm_emisor_contrato)
                .WithRequired(e => e.sys_tipo_contrato)
                .HasForeignKey(e => e.eco_tipo_contrato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_correcion_nota_credito>()
                .Property(e => e.cnc_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_correcion_nota_credito>()
                .Property(e => e.cnc_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_correcion_nota_debito>()
                .Property(e => e.cnd_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_correcion_nota_debito>()
                .Property(e => e.cnd_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_correo>()
                .Property(e => e.tco_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_correo>()
                .HasMany(e => e.adm_emisor_correo)
                .WithOptional(e => e.sys_tipo_correo)
                .HasForeignKey(e => e.eco_tipo_correo);

            modelBuilder.Entity<sys_tipo_dato>()
                .Property(e => e.tda_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_dato>()
                .HasMany(e => e.adm_emisor_catalogo)
                .WithRequired(e => e.sys_tipo_dato)
                .HasForeignKey(e => e.eca_tipo_dato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .Property(e => e.tdo_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .Property(e => e.tdo_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .Property(e => e.tdo_uso)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .Property(e => e.tdo_abreviatura)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .HasMany(e => e.adm_documento)
                .WithRequired(e => e.sys_tipo_documento)
                .HasForeignKey(e => e.doc_tipo_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .HasMany(e => e.adm_documento_proveedor)
                .WithRequired(e => e.sys_tipo_documento)
                .HasForeignKey(e => e.dpr_tipo_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .HasMany(e => e.adm_emisor_resolucion)
                .WithRequired(e => e.sys_tipo_documento)
                .HasForeignKey(e => e.ere_tipo_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .HasMany(e => e.adm_plantilla)
                .WithRequired(e => e.sys_tipo_documento)
                .HasForeignKey(e => e.prg_tipo_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_documento>()
                .HasMany(e => e.tmp_documento)
                .WithRequired(e => e.sys_tipo_documento)
                .HasForeignKey(e => e.tdo_tipo_documento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_emisor>()
                .Property(e => e.tem_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_emisor>()
                .Property(e => e.tem_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_emisor>()
                .HasMany(e => e.adm_emisor)
                .WithOptional(e => e.sys_tipo_emisor)
                .HasForeignKey(e => e.emi_tipo_emisor);

            modelBuilder.Entity<sys_tipo_evento>()
                .Property(e => e.tev_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_evento>()
                .Property(e => e.tev_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_evento>()
                .Property(e => e.tev_responsable_registro)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_evento>()
                .HasMany(e => e.adm_documento_eventos)
                .WithRequired(e => e.sys_tipo_evento)
                .HasForeignKey(e => e.dev_evento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_forma_pago>()
                .Property(e => e.tfp_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_forma_pago>()
                .Property(e => e.tfp_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .Property(e => e.tid_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .Property(e => e.tid_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .HasMany(e => e.adm_distriduidor)
                .WithRequired(e => e.sys_tipo_identificacion)
                .HasForeignKey(e => e.dis_tipo_identificacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .HasMany(e => e.adm_emisor)
                .WithRequired(e => e.sys_tipo_identificacion)
                .HasForeignKey(e => e.emi_tipo_identificacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .HasMany(e => e.adm_mandato)
                .WithRequired(e => e.sys_tipo_identificacion)
                .HasForeignKey(e => e.man_tipo_identificacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .HasMany(e => e.adm_proveedor)
                .WithRequired(e => e.sys_tipo_identificacion)
                .HasForeignKey(e => e.pro_tipo_identificacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .HasMany(e => e.adm_receptor)
                .WithRequired(e => e.sys_tipo_identificacion)
                .HasForeignKey(e => e.rec_tipo_identificacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_identificacion>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.sys_tipo_identificacion)
                .HasForeignKey(e => e.alc_tipo_identificacion);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .Property(e => e.tim_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .Property(e => e.tim_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .Property(e => e.tim_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .Property(e => e.tim_porcentaje)
                .HasPrecision(18, 4);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .HasMany(e => e.adm_emisor_impuesto)
                .WithRequired(e => e.sys_tipo_impuesto)
                .HasForeignKey(e => e.emi_id_impuesto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .HasMany(e => e.adm_emisor_producto)
                .WithRequired(e => e.sys_tipo_impuesto)
                .HasForeignKey(e => e.epr_tipo_impuesto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_impuesto>()
                .HasMany(e => e.adm_tipo_retencion_fuente)
                .WithRequired(e => e.sys_tipo_impuesto)
                .HasForeignKey(e => e.trf_id_impuesto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_medio_pago>()
                .Property(e => e.tmp_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_medio_pago>()
                .Property(e => e.tmp_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_negociacion>()
                .Property(e => e.tne_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_negociacion>()
                .HasMany(e => e.adm_emisor_contrato)
                .WithRequired(e => e.sys_tipo_negociacion)
                .HasForeignKey(e => e.eco_tipo_negociacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_notificacion>()
                .Property(e => e.tno_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_notificacion>()
                .Property(e => e.tno_ruta_imagen)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_notificacion>()
                .HasMany(e => e.adm_emisor_notificacion)
                .WithRequired(e => e.sys_tipo_notificacion)
                .HasForeignKey(e => e.eno_id_tipo_notificacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_operacion>()
                .Property(e => e.top_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_operacion>()
                .Property(e => e.top_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_organizacion>()
                .Property(e => e.tor_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_organizacion>()
                .Property(e => e.tor_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_persona>()
                .Property(e => e.tpe_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_persona>()
                .Property(e => e.tpe_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_persona>()
                .HasMany(e => e.adm_emisor)
                .WithRequired(e => e.sys_tipo_persona)
                .HasForeignKey(e => e.emi_tipo_persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_persona>()
                .HasMany(e => e.adm_proveedor)
                .WithOptional(e => e.sys_tipo_persona)
                .HasForeignKey(e => e.pro_tipo_persona);

            modelBuilder.Entity<sys_tipo_persona>()
                .HasMany(e => e.adm_receptor)
                .WithOptional(e => e.sys_tipo_persona)
                .HasForeignKey(e => e.rec_tipo_persona);

            modelBuilder.Entity<sys_tipo_persona>()
                .HasMany(e => e.AuditLogReceptor)
                .WithOptional(e => e.sys_tipo_persona)
                .HasForeignKey(e => e.alc_tipo_persona);

            modelBuilder.Entity<sys_tipo_unidad_cantidad>()
                .Property(e => e.tuc_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_unidad_cantidad>()
                .Property(e => e.tuc_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_unidad_cantidad>()
                .HasMany(e => e.adm_emisor_producto)
                .WithRequired(e => e.sys_tipo_unidad_cantidad)
                .HasForeignKey(e => e.epr_unidad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_tipo_usuario>()
                .Property(e => e.tus_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<sys_tipo_usuario>()
                .HasMany(e => e.adm_usuario_perfil_tipo)
                .WithRequired(e => e.sys_tipo_usuario)
                .HasForeignKey(e => e.upt_tipo_usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_json)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_subtotal)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_valor_total)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_id_receptor)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_id_impuesto)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_nit)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_prefijo)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_documento>()
                .Property(e => e.tdo_numero)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DistribuidorInicio>()
              .Property(e => e.tid_descripcion)
              .IsUnicode(false);

            modelBuilder.Entity<DistribuidorInicio>()
                .Property(e => e.emi_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<DistribuidorInicio>()
                .Property(e => e.emi_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<DistribuidorInicio>()
                .Property(e => e.dep_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<DistribuidorInicio>()
                .Property(e => e.mun_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<DistribuidorInicio>()
                .Property(e => e.emi_correo)
                .IsUnicode(false);

            modelBuilder.Entity<DistribuidorInicio>()
                .Property(e => e.emi_telefono)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.dfi_json_facse)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.ted_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.tdo_abreviatura)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.doc_prefijo)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.doc_numero)
                .HasPrecision(18, 0);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.rec_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.rec_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.doc_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.dfi_xml)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.edf_ruta_imagen)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicio>()
                .Property(e => e.edf_descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.dpf_pdf)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.tdo_abreviatura)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.dpr_prefijo)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.dpr_numero)
                .HasPrecision(18, 0);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.pro_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.pro_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.dpr_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.dpf_xml)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.edf_ruta_imagen)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.edf_codigo)
                .IsUnicode(false);

            modelBuilder.Entity<EmisorInicioProveedor>()
                .Property(e => e.edf_descripcion)
                .IsUnicode(false);
        }


        public int SaveChanges(string userId, string ip)
        {
            bool ignoreAudit = false;
            string pcName = this.DetermineCompName(ip);

            foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                ignoreAudit = false;
                Attribute[] tableAttr = ent.Entity.GetType().GetCustomAttributes(typeof(Attribute), true) as Attribute[];

                foreach (Attribute attr in tableAttr)
                {
                    if (attr is IgnoreAuditAttribute)
                    {
                        IgnoreAuditAttribute attrTemp = (IgnoreAuditAttribute)attr;
                        ignoreAudit = attrTemp.IgnoreToAuditLogs;
                        break;
                    }
                }

                if (!ignoreAudit)
                {//Insertar registro de auditor�a por cada registro alterado en la entidad (Si no est� especificado por defecto se inserta)
                    foreach (AuditLogs x in GetAuditRecordsForChange(ent, userId, ip, pcName))
                    {
                        this.AuditLogs.Add(x);
                    }
                }
            }

            return SaveChanges();
        }

        private List<AuditLogs> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId, string ip, string pcName)
        {
            List<AuditLogs> result = new List<AuditLogs>();

            DateTime changeTime = DateTime.Now;

            //TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;

            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
            var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).ToList();//TODO: Validar si hay mas de un elemento [Key]
            string keyName = "";
            if (keyNames.Count > 0)
            {
                keyName = keyNames[0].Name;
            }

            var auditExcludedProps = dbEntry.Entity.GetType()
                                       .GetProperties()
                                       .Where(p => p.GetCustomAttributes(typeof(IgnoreAuditAttribute), false).Any())
                                       .Select(p => p.Name)
                                       .ToList();

            if (dbEntry.State == EntityState.Added)
            {
                foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                {
                    var doNotAUditDefined = auditExcludedProps.Contains(propertyName);

                    if (!doNotAUditDefined)
                    {
                        result.Add(new AuditLogs()
                        {
                            IdAuditLog = Guid.NewGuid().ToString(),
                            Usuario = userId,
                            IpUsuario = ip,
                            Fecha = changeTime,
                            Accion = "Insertar",    // Added
                            Tabla = tableName,
                            PCName = pcName,
                            RegistroTabla = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                            Campo = propertyName,
                            ValorNuevo = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                        });
                    }
                }
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                result.Add(new AuditLogs()
                {
                    IdAuditLog = Guid.NewGuid().ToString(),
                    Usuario = userId,
                    IpUsuario = ip,
                    Fecha = changeTime,
                    Accion = "Eliminar", // Deleted
                    Tabla = tableName,
                    PCName = pcName,
                    RegistroTabla = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                    Campo = "All",
                    ValorAnterior = dbEntry.OriginalValues.ToObject().ToString()
                });
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {// Para actualizaci�n, capturar columnas con cambios reales
                    if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        var doNotAUditDefined = auditExcludedProps.Contains(propertyName);

                        if (!doNotAUditDefined)
                        {
                            result.Add(new AuditLogs()
                            {
                                IdAuditLog = Guid.NewGuid().ToString(),
                                Usuario = userId,
                                IpUsuario = ip,
                                Fecha = changeTime,
                                Accion = "Actualizar",    // Updated
                                Tabla = tableName,
                                PCName = pcName,
                                RegistroTabla = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                                Campo = propertyName,
                                ValorAnterior = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                ValorNuevo = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            });
                        }
                    }
                }
            }
            //else if (dbEntry.State == EntityState.Detached)
            //{

            //}
            //else if (dbEntry.State == EntityState.Unchanged)
            //{

            //}

            return result;
        }

        private string DetermineCompName(string IP)
        {
            //Org.BouncyCastle.Utilities.Net.IPAddress myIP = Org.BouncyCastle.Utilities.Net.IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(IP);
            List<string> compName = new List<string>();
            if (!String.IsNullOrEmpty(GetIPHost.HostName))
                compName = GetIPHost.HostName.ToString().Split('.').ToList();
            else
                return null;

            return compName.First();
        }
    }
}
