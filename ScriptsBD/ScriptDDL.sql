USE [DBFACSEV2]
GO
/****** Object:  UserDefinedFunction [dbo].[getDigitoVerificacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[getDigitoVerificacion] 
( 
-- Add the parameters for the function here 
@Id varchar(20) 
) 
RETURNS varchar(1) 
AS 
BEGIN 
-- ============================================= 
-- Author:	Gerardo Sol�rzano 
-- Create date: 7 Oct 2009 
-- Description:	Calculo del digito de verificacion para NIT/Cedula en Colombia 
-- ============================================= 
-- Declare the return variable here 
DECLARE @sTMP numeric(20) 
DECLARE @sTmp1 numeric(20) 
DECLARE @sTMP2 numeric(20) 
DECLARE @i numeric(20) 
DECLARE @iResiduo numeric(20) 
DECLARE @iChequeo numeric(20) 

DECLARE @iPrimos	varchar(50), @primo int 


SET @iPrimos	= '030713171923293741434753596771\' 

DECLARE @pos int, 
@maxpos int, 
@valuelen int, @DigitoNIT int 

SELECT @pos = 0, @maxpos = Len(@Id) - 1 
SELECT @iChequeo = 0, @iResiduo = 0 
WHILE @pos <= @maxpos 
BEGIN 
SELECT @primo = cast(substring(@iPrimos,(@pos+1)*2-1,2) as int) 
SELECT @sTMP = cast(substring(@Id, Len(@Id) - @pos, 1) as numeric(20)) 
select @iChequeo = @iChequeo + @sTMP * @primo 

SELECT @pos = @pos + 1 
END 
select @iResiduo = @iChequeo % 11 

If @iResiduo = 0 set @DigitoNIT = 0 
If @iResiduo = 1 set @DigitoNIT = 1 
If @iResiduo > 1 set @DigitoNIT = 11 - @iResiduo 

-- Return the result of the function 

RETURN @DigitoNIT 




END
GO
/****** Object:  Table [dbo].[sys_municipio]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_municipio](
	[mun_id] [uniqueidentifier] NOT NULL,
	[mun_codigo] [varchar](10) NOT NULL,
	[mun_nombre] [varchar](100) NOT NULL,
	[mun_id_dpto] [uniqueidentifier] NULL,
	[mun_id_pais] [uniqueidentifier] NULL,
 CONSTRAINT [PK_sys_municipio] PRIMARY KEY CLUSTERED 
(
	[mun_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_identificacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_identificacion](
	[tid_id] [uniqueidentifier] NOT NULL,
	[tid_codigo] [varchar](200) NOT NULL,
	[tid_descripcion] [varchar](500) NOT NULL,
 CONSTRAINT [PK_sys_tipo_identificacion_1] PRIMARY KEY CLUSTERED 
(
	[tid_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor](
	[emi_id] [uniqueidentifier] NOT NULL,
	[emi_grupo] [uniqueidentifier] NOT NULL,
	[emi_tipo_persona] [uniqueidentifier] NOT NULL,
	[emi_tipo_identificacion] [uniqueidentifier] NOT NULL,
	[emi_identificacion] [varchar](15) NOT NULL,
	[emi_digito] [varchar](1) NULL,
	[emi_nombre] [varchar](150) NOT NULL,
	[emi_razon_social] [varchar](150) NOT NULL,
	[emi_pais] [uniqueidentifier] NULL,
	[emi_departamento] [uniqueidentifier] NULL,
	[emi_municipio] [uniqueidentifier] NULL,
	[emi_codigo_posta] [varchar](50) NULL,
	[emi_correo] [varchar](80) NULL,
	[emi_direccion] [varchar](200) NULL,
	[emi_telefono] [varchar](50) NULL,
	[emi_tipo_emisor] [uniqueidentifier] NULL,
	[emi_distribuidor] [uniqueidentifier] NULL,
	[emi_logo] [varchar](50) NULL,
	[emi_test_id] [varchar](50) NULL,
	[emi_cliente_token] [varchar](200) NULL,
	[emi_access_token] [varchar](200) NULL,
	[emi_activo] [bit] NOT NULL,
	[emi_fecha_creacion] [datetime] NULL,
	[emi_sofware] [uniqueidentifier] NULL,
	[emi_ciiu] [uniqueidentifier] NULL,
	[emi_matricula_mercantil] [varchar](20) NULL,
	[emi_identificacion_alterna] [varchar](20) NULL,
	[emi_mandato] [uniqueidentifier] NULL,
 CONSTRAINT [PK_adm_emisor_1] PRIMARY KEY CLUSTERED 
(
	[emi_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_departamento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_departamento](
	[dep_id] [uniqueidentifier] NOT NULL,
	[dep_codigo] [varchar](20) NOT NULL,
	[dep_nombre] [varchar](100) NOT NULL,
	[dep_iso] [varchar](20) NOT NULL,
	[dep_id_pais] [uniqueidentifier] NULL,
 CONSTRAINT [PK_sys_departamentos_1] PRIMARY KEY CLUSTERED 
(
	[dep_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_sys_departamentos] UNIQUE NONCLUSTERED 
(
	[dep_codigo] ASC,
	[dep_id_pais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[DistribuidorInicio]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[DistribuidorInicio] 
WITH SCHEMABINDING
as
select e.emi_id, i.tid_descripcion, e.emi_identificacion, e.emi_nombre, d.dep_nombre, m.mun_nombre, e.emi_correo, e.emi_telefono, e.emi_fecha_creacion, e.emi_distribuidor
from [dbo].adm_emisor e
inner join [dbo].sys_tipo_identificacion i on e.emi_tipo_identificacion=i.tid_id
left join [dbo].sys_departamento d on e.emi_departamento=d.dep_id
left join [dbo].sys_municipio m on e.emi_municipio=m.mun_id
GO
/****** Object:  Table [dbo].[sys_estado_documento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_estado_documento](
	[ted_id] [uniqueidentifier] NOT NULL,
	[ted_descripcion] [varchar](50) NOT NULL,
	[ted_activo] [bit] NOT NULL,
	[ted_codigo] [varchar](5) NULL,
 CONSTRAINT [PK_sys_estado_documento] PRIMARY KEY CLUSTERED 
(
	[ted_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_estado_documento_facse]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_estado_documento_facse](
	[edf_id] [uniqueidentifier] NOT NULL,
	[edf_codigo] [varchar](10) NOT NULL,
	[edf_descripcion] [varchar](100) NULL,
	[edf_ruta_imagen] [varchar](200) NULL,
	[edf_activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[edf_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_documento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_documento](
	[tdo_id] [uniqueidentifier] NOT NULL,
	[tdo_codigo] [varchar](80) NOT NULL,
	[tdo_descripcion] [varchar](200) NOT NULL,
	[tdo_uso] [varchar](80) NOT NULL,
	[tdo_abreviatura] [varchar](5) NULL,
 CONSTRAINT [PK_sys_tipo_documento_1] PRIMARY KEY CLUSTERED 
(
	[tdo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento](
	[doc_id] [uniqueidentifier] NOT NULL,
	[doc_prefijo] [varchar](10) NOT NULL,
	[doc_tipo_documento] [uniqueidentifier] NOT NULL,
	[doc_numero] [numeric](18, 0) NOT NULL,
	[doc_emisor] [uniqueidentifier] NOT NULL,
	[doc_receptor] [uniqueidentifier] NOT NULL,
	[doc_plantilla_version] [varchar](20) NOT NULL,
	[doc_fecha_documento] [datetime] NOT NULL,
	[doc_fecha_recepcion] [datetime] NOT NULL,
	[doc_fecha_envio] [datetime] NOT NULL,
	[doc_moneda] [uniqueidentifier] NOT NULL,
	[doc_sucursal] [uniqueidentifier] NOT NULL,
	[doc_resolucion] [uniqueidentifier] NOT NULL,
	[doc_cufe] [varchar](800) NULL,
	[doc_clave] [varchar](100) NULL,
	[doc_validacion_dian] [varchar](100) NULL,
	[doc_respuesta_dian] [varchar](500) NULL,
	[doc_zipkey] [varchar](200) NULL,
	[doc_trackid] [varchar](200) NULL,
	[doc_valor_total] [decimal](18, 2) NOT NULL,
	[doc_valor_impuestos] [decimal](18, 2) NOT NULL,
	[doc_estado] [uniqueidentifier] NOT NULL,
	[doc_usuario] [varchar](50) NULL,
	[doc_observacion] [varchar](1000) NULL,
	[doc_id_estado_facse] [uniqueidentifier] NULL,
	[doc_emisor_certificado] [uniqueidentifier] NULL,
	[doc_id_origen_documento] [uniqueidentifier] NULL,
 CONSTRAINT [PK_adm_documento_1] PRIMARY KEY CLUSTERED 
(
	[doc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_correo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_correo](
	[dco_id] [uniqueidentifier] NOT NULL,
	[dco_documento] [uniqueidentifier] NOT NULL,
	[dco_correo] [varchar](200) NOT NULL,
	[dco_fecha] [datetime] NOT NULL,
	[dco_estado] [uniqueidentifier] NULL,
	[dco_mensaje] [varchar](500) NULL,
 CONSTRAINT [PK_adm_documento_correo] PRIMARY KEY CLUSTERED 
(
	[dco_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_file]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_file](
	[dfi_id] [uniqueidentifier] NOT NULL,
	[dfi_documento] [uniqueidentifier] NOT NULL,
	[dfi_xml] [varchar](max) NULL,
	[dfi_json_facse] [varchar](max) NULL,
	[dfi_fecha] [datetime] NOT NULL,
	[dfi_ruta_zip] [varchar](200) NULL,
 CONSTRAINT [PK_adm_documento_file_1] PRIMARY KEY CLUSTERED 
(
	[dfi_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_notificacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_notificacion](
	[dno_id] [uniqueidentifier] NOT NULL,
	[dno_documento] [uniqueidentifier] NOT NULL,
	[dno_fecha] [datetime] NOT NULL,
	[dno_id_regla] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[dno_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documentos_anexo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documentos_anexo](
	[dan_id] [uniqueidentifier] NOT NULL,
	[dan_documento] [uniqueidentifier] NOT NULL,
	[dan_directorio] [varchar](200) NOT NULL,
	[dan_nombre] [varchar](100) NOT NULL,
	[dan_extension] [varchar](100) NOT NULL,
 CONSTRAINT [PK_adm_documentos_anexo_1] PRIMARY KEY CLUSTERED 
(
	[dan_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_receptor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_receptor](
	[rec_id] [uniqueidentifier] NOT NULL,
	[rec_emisor] [uniqueidentifier] NOT NULL,
	[rec_tipo_receptor] [varchar](100) NOT NULL,
	[rec_tipo_persona] [uniqueidentifier] NULL,
	[rec_tipo_identificacion] [uniqueidentifier] NOT NULL,
	[rec_identificacion] [varchar](100) NOT NULL,
	[rec_digito] [varchar](5) NULL,
	[rec_nombre] [varchar](200) NOT NULL,
	[rec_razon_social] [varchar](200) NOT NULL,
	[rec_pais] [uniqueidentifier] NULL,
	[rec_departamento] [uniqueidentifier] NULL,
	[rec_municipio] [uniqueidentifier] NULL,
	[rec_codigo_postal] [varchar](10) NULL,
	[rec_correo] [varchar](200) NULL,
	[rec_direccion] [varchar](150) NULL,
	[rec_telefono] [varchar](50) NULL,
	[rec_fecha_receccion] [datetime] NOT NULL,
	[rec_activo] [bit] NOT NULL,
	[rec_matricula_mercantil] [varchar](20) NULL,
	[rec_contrasena] [varchar](200) NULL,
	[rec_usuario_receptor] [uniqueidentifier] NULL,
 CONSTRAINT [PK_adm_receptor] PRIMARY KEY CLUSTERED 
(
	[rec_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[EmisorInicio]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




--drop view [EmisorInicio]

CREATE View [dbo].[EmisorInicio] 
WITH SCHEMABINDING
as
select dfi_json_facse, ted_codigo, d.doc_id, td.tdo_abreviatura, d.doc_prefijo, d.doc_numero, 
r.rec_nombre, r.rec_identificacion, d.doc_fecha_recepcion, d.doc_fecha_envio, d.doc_valor_total, d.doc_usuario, d.doc_tipo_documento,
d.doc_sucursal, dfi_xml, edf.edf_ruta_imagen, d.doc_estado, d.doc_id_estado_facse, edf.edf_descripcion,
(select count(*) from [dbo].adm_documento_notificacion where dno_documento=d.doc_id) notificacion,
(select count(*) from [dbo].adm_documento_correo where dco_documento=d.doc_id) correo,
(select count(*) from [dbo].adm_documentos_anexo where dan_documento=d.doc_id) anexo
from [dbo].adm_documento d
inner join [dbo].adm_documento_file df on d.doc_id=df.dfi_documento
inner join [dbo].sys_estado_documento ed on d.doc_estado=ed.ted_id
inner join [dbo].sys_tipo_documento td on d.doc_tipo_documento=td.tdo_id
inner join [dbo].adm_receptor r on doc_receptor=rec_id
left join [dbo].sys_estado_documento_facse edf  on d.doc_id_estado_facse=edf.edf_id
GO
/****** Object:  Table [dbo].[adm_documento_proveedor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_proveedor](
	[dpr_id] [uniqueidentifier] NOT NULL,
	[dpr_prefijo] [varchar](10) NOT NULL,
	[dpr_tipo_documento] [uniqueidentifier] NOT NULL,
	[dpr_numero] [numeric](18, 0) NOT NULL,
	[dpr_emisor] [uniqueidentifier] NOT NULL,
	[dpr_proveedor] [uniqueidentifier] NOT NULL,
	[dpr_plantilla_version] [varchar](20) NOT NULL,
	[dpr_fecha_documento] [datetime] NOT NULL,
	[dpr_fecha_recepcion] [datetime] NOT NULL,
	[dpr_fecha_recibido] [datetime] NOT NULL,
	[dpr_fecha_envio] [datetime] NOT NULL,
	[dpr_moneda] [uniqueidentifier] NOT NULL,
	[dpr_sucursal] [uniqueidentifier] NOT NULL,
	[dpr_cufe] [varchar](800) NULL,
	[dpr_clave] [varchar](100) NULL,
	[dpr_validacion_dian] [varchar](100) NULL,
	[dpr_respuesta_dian] [varchar](500) NULL,
	[dpr_zipkey] [varchar](200) NULL,
	[dpr_trackid] [varchar](200) NULL,
	[dpr_valor_total] [decimal](18, 2) NOT NULL,
	[dpr_valor_impuestos] [decimal](18, 2) NOT NULL,
	[dpr_usuario] [varchar](50) NULL,
	[dpr_observacion] [varchar](1000) NULL,
	[dpr_id_estado_facse] [uniqueidentifier] NULL,
	[dpr_id_origen_documento] [uniqueidentifier] NULL,
	[dpr_acuse] [bit] NOT NULL,
	[dpr_fecha_acuse] [datetime] NULL,
 CONSTRAINT [PK_documento_proveedor] PRIMARY KEY CLUSTERED 
(
	[dpr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_proveedor_anexo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_proveedor_anexo](
	[dpa_id] [uniqueidentifier] NOT NULL,
	[dpa_documento_proveedor] [uniqueidentifier] NOT NULL,
	[dpa_directorio] [varchar](200) NOT NULL,
	[dpa_nombre] [varchar](100) NOT NULL,
	[dpa_extension] [varchar](100) NOT NULL,
 CONSTRAINT [PK_adm_documento_proveedor_anexo] PRIMARY KEY CLUSTERED 
(
	[dpa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_proveedor_file]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_proveedor_file](
	[dpf_id] [uniqueidentifier] NOT NULL,
	[dpf_documento_proveedor] [uniqueidentifier] NOT NULL,
	[dpf_xml] [varchar](200) NULL,
	[dpf_pdf] [varchar](200) NULL,
	[dpf_fecha] [datetime] NOT NULL,
	[dpf_attached] [varchar](200) NULL,
 CONSTRAINT [PK_adm_documento_proveedor_file] PRIMARY KEY CLUSTERED 
(
	[dpf_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_proveedor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_proveedor](
	[pro_id] [uniqueidentifier] NOT NULL,
	[pro_emisor] [uniqueidentifier] NOT NULL,
	[pro_tipo_receptor] [varchar](100) NOT NULL,
	[pro_tipo_persona] [uniqueidentifier] NULL,
	[pro_tipo_identificacion] [uniqueidentifier] NOT NULL,
	[pro_identificacion] [varchar](100) NOT NULL,
	[pro_digito] [varchar](5) NULL,
	[pro_nombre] [varchar](200) NOT NULL,
	[pro_razon_social] [varchar](200) NOT NULL,
	[pro_pais] [uniqueidentifier] NULL,
	[pro_departamento] [uniqueidentifier] NULL,
	[pro_municipio] [uniqueidentifier] NULL,
	[pro_codigo_postal] [varchar](10) NULL,
	[pro_correo] [varchar](200) NULL,
	[pro_direccion] [varchar](150) NULL,
	[pro_telefono] [varchar](50) NULL,
	[pro_fecha_recepcion] [datetime] NOT NULL,
	[pro_activo] [bit] NOT NULL,
	[pro_matricula_mercantil] [varchar](20) NULL,
 CONSTRAINT [PK_adm_proveedor] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[EmisorInicioProveedor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE View [dbo].[EmisorInicioProveedor] 
WITH SCHEMABINDING
as
select dpf_pdf,  dp.dpr_id, td.tdo_abreviatura, dp.dpr_prefijo, dp.dpr_numero, 
p.pro_nombre, p.pro_identificacion, dp.dpr_fecha_recepcion, dp.dpr_fecha_recibido,
dp.dpr_fecha_envio,dp.dpr_fecha_documento, dp.dpr_fecha_acuse, dp.dpr_valor_total, dp.dpr_usuario, dp.dpr_tipo_documento,
dp.dpr_sucursal, dpf_xml, edf.edf_ruta_imagen, dp.dpr_id_estado_facse, dp.dpr_acuse,	
edf.edf_codigo,edf.edf_descripcion,
(select count(*) from [dbo].adm_documento_proveedor_anexo where dpa_documento_proveedor=dp.dpr_id) anexo
from [dbo].adm_documento_proveedor dp
inner join [dbo].adm_documento_proveedor_file dpf on dp.dpr_id=dpf.dpf_documento_proveedor
inner join [dbo].sys_tipo_documento td on dp.dpr_tipo_documento=td.tdo_id
inner join [dbo].adm_proveedor p on dpr_proveedor=pro_id
left join [dbo].sys_estado_documento_facse edf  on dp.dpr_id_estado_facse=edf.edf_id
GO
/****** Object:  Table [dbo].[adm_centinela]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_centinela](
	[cen_id_emisor_sucursal] [uniqueidentifier] NOT NULL,
	[cen_fecha] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cen_id_emisor_sucursal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_distriduidor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_distriduidor](
	[dis_id] [uniqueidentifier] NOT NULL,
	[dis_tipo_identificacion] [uniqueidentifier] NOT NULL,
	[dis_identificacion] [varchar](20) NOT NULL,
	[dis_nombre] [varchar](200) NOT NULL,
	[dis_email] [varchar](200) NULL,
	[dis_contacto] [varchar](200) NULL,
	[dis_telefono] [varchar](100) NULL,
	[dis_municipio] [uniqueidentifier] NOT NULL,
	[dis_activo] [bit] NOT NULL,
	[dis_fecha_creacion] [datetime] NOT NULL,
 CONSTRAINT [PK_adm_distriduidor_1] PRIMARY KEY CLUSTERED 
(
	[dis_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_estado_facse]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_estado_facse](
	[def_id] [uniqueidentifier] NOT NULL,
	[def_id_documento] [uniqueidentifier] NULL,
	[def_id_estado] [uniqueidentifier] NULL,
	[def_fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[def_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_eventos]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_eventos](
	[dev_id] [uniqueidentifier] NOT NULL,
	[dev_documento] [uniqueidentifier] NOT NULL,
	[dev_evento] [uniqueidentifier] NOT NULL,
	[dev_fecha] [datetime] NOT NULL,
 CONSTRAINT [PK_adm_documento_eventos] PRIMARY KEY CLUSTERED 
(
	[dev_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_file_historico]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_file_historico](
	[dfh_id] [uniqueidentifier] NOT NULL,
	[dfh_id_documento] [uniqueidentifier] NULL,
	[dfh_xml] [varchar](500) NULL,
	[dfh_json_facse] [varchar](max) NULL,
	[dfi_fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[dfh_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_documento_proveedor_estado_facse]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_documento_proveedor_estado_facse](
	[dpe_id] [uniqueidentifier] NOT NULL,
	[dpe_documento_proveedor] [uniqueidentifier] NULL,
	[dpe_id_estado] [uniqueidentifier] NULL,
	[dpe_fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[dpe_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_catalogo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_catalogo](
	[eca_id] [uniqueidentifier] NOT NULL,
	[eca_emisor] [uniqueidentifier] NOT NULL,
	[eca_tipo_dato] [uniqueidentifier] NOT NULL,
	[eca_nombre] [varchar](100) NOT NULL,
	[eca_tipo_catalogo] [uniqueidentifier] NOT NULL,
	[eca_lista] [bit] NOT NULL,
	[eca_activo] [bit] NULL,
 CONSTRAINT [PK_adm_emisor_catalogo_1] PRIMARY KEY CLUSTERED 
(
	[eca_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_catalogo_lista]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_catalogo_lista](
	[ecl_id] [uniqueidentifier] NOT NULL,
	[ecl_emisor_catalogo] [uniqueidentifier] NOT NULL,
	[ecl_descripcion] [varchar](200) NOT NULL,
 CONSTRAINT [PK_adm_emisor_catalogo_lista_1] PRIMARY KEY CLUSTERED 
(
	[ecl_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_certificado]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_certificado](
	[ece_id] [uniqueidentifier] NOT NULL,
	[ece_emisor] [uniqueidentifier] NOT NULL,
	[ece_archivo] [varchar](200) NOT NULL,
	[ece_certificado] [varchar](max) NOT NULL,
	[ece_fecha_vegencia] [datetime] NOT NULL,
	[ece_activo] [bit] NOT NULL,
	[ece_contrasena] [varchar](50) NULL,
 CONSTRAINT [PK_adm_emisor_certificado_1] PRIMARY KEY CLUSTERED 
(
	[ece_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_contrato]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_contrato](
	[eco_emisor] [uniqueidentifier] NOT NULL,
	[eco_tipo_contrato] [uniqueidentifier] NOT NULL,
	[eco_tipo_negociacion] [uniqueidentifier] NOT NULL,
	[eco_fecha_inicio] [datetime] NOT NULL,
	[eco_fecha_fin] [datetime] NOT NULL,
	[eco_valor] [numeric](18, 0) NOT NULL,
	[eco_cantidad_documento] [int] NOT NULL,
 CONSTRAINT [PK_adm_emisor_contrato] PRIMARY KEY CLUSTERED 
(
	[eco_emisor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_correo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_correo](
	[eco_id] [uniqueidentifier] NOT NULL,
	[eco_emisor] [uniqueidentifier] NOT NULL,
	[eco_servidor] [varchar](100) NOT NULL,
	[eco_puerto] [varchar](50) NOT NULL,
	[eco_usuario] [varchar](100) NOT NULL,
	[eco_contrasena] [varchar](100) NOT NULL,
	[eco_correo] [varchar](100) NOT NULL,
	[eco_ssl] [bit] NOT NULL,
	[eco_correo_html] [varchar](100) NOT NULL,
	[eco_activo] [bit] NOT NULL,
	[eco_tipo_correo] [uniqueidentifier] NULL,
	[eco_nombre] [varchar](50) NULL,
 CONSTRAINT [PK_adm_emisor_correo_1] PRIMARY KEY CLUSTERED 
(
	[eco_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_impuesto]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_impuesto](
	[emi_id_emisor] [uniqueidentifier] NOT NULL,
	[emi_id_impuesto] [uniqueidentifier] NOT NULL,
	[emi_fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[emi_id_emisor] ASC,
	[emi_id_impuesto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_notificacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_notificacion](
	[eno_id] [uniqueidentifier] NOT NULL,
	[eno_id_emisor] [uniqueidentifier] NOT NULL,
	[eno_id_tipo_notificacion] [uniqueidentifier] NOT NULL,
	[eno_descripcion] [varchar](500) NOT NULL,
	[eno_fecha] [datetime] NULL,
	[eno_leido] [bit] NULL,
 CONSTRAINT [PK__adm_emis__CCB1F74A6C358C06] PRIMARY KEY CLUSTERED 
(
	[eno_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_producto]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_producto](
	[epr_id] [uniqueidentifier] NOT NULL,
	[epr_emisor] [uniqueidentifier] NOT NULL,
	[epr_codigo] [varchar](50) NOT NULL,
	[epr_descripcion] [varchar](100) NOT NULL,
	[epr_unidad] [uniqueidentifier] NOT NULL,
	[epr_valor_unitario] [numeric](18, 2) NOT NULL,
	[epr_tipo_impuesto] [uniqueidentifier] NOT NULL,
	[epr_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_emisor_producto] PRIMARY KEY CLUSTERED 
(
	[epr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_producto_catalogo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_producto_catalogo](
	[epc_producto] [uniqueidentifier] NOT NULL,
	[epc_catalogo] [uniqueidentifier] NOT NULL,
	[epc_valor] [varchar](200) NULL,
 CONSTRAINT [PK_adm_emisor_producto_catalogo] PRIMARY KEY CLUSTERED 
(
	[epc_producto] ASC,
	[epc_catalogo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_resolucion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_resolucion](
	[ere_id] [uniqueidentifier] NOT NULL,
	[ere_emisor] [uniqueidentifier] NOT NULL,
	[ere_tipo_documento] [uniqueidentifier] NOT NULL,
	[ere_prefijo] [varchar](5) NOT NULL,
	[ere_numero_resolucion] [varchar](500) NOT NULL,
	[ere_fecha] [datetime] NOT NULL,
	[ere_numero_inicial] [varchar](50) NOT NULL,
	[ere_numero_final] [varchar](50) NOT NULL,
	[ere_clave_tecnica] [varchar](300) NOT NULL,
	[ere_fecha_inicio] [datetime] NOT NULL,
	[ere_fecha_final] [datetime] NOT NULL,
	[ere_activo] [bit] NOT NULL,
	[ere_plantilla_version] [varchar](10) NULL,
	[ere_plantilla] [uniqueidentifier] NULL,
 CONSTRAINT [PK_adm_emisor_resolucion_1] PRIMARY KEY CLUSTERED 
(
	[ere_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_sucursal]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_sucursal](
	[esu_id] [uniqueidentifier] NOT NULL,
	[esu_emisor] [uniqueidentifier] NOT NULL,
	[esu_nombre] [varchar](80) NOT NULL,
	[esu_abreviatura] [varchar](20) NOT NULL,
	[esu_departamento] [uniqueidentifier] NULL,
	[esu_municipio] [uniqueidentifier] NULL,
	[esu_correo] [varchar](100) NULL,
	[esu_direccion] [varchar](100) NULL,
	[esu_codigo_postal] [varchar](80) NULL,
	[esu_telefono] [varchar](40) NULL,
	[esu_correo_entrada] [uniqueidentifier] NULL,
	[esu_activo] [bit] NOT NULL,
	[esu_correo_salida] [uniqueidentifier] NULL,
 CONSTRAINT [PK_adm_emisor_sucursal] PRIMARY KEY CLUSTERED 
(
	[esu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_sucursal_json]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_sucursal_json](
	[esj_id] [uniqueidentifier] NOT NULL,
	[esj_id_emisor_sucursal] [uniqueidentifier] NULL,
	[esj_ruta] [varchar](200) NULL,
	[esj_fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[esj_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_sucursal_plantilla]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_sucursal_plantilla](
	[esp_id] [uniqueidentifier] NOT NULL,
	[esp_emisor_sucursal] [uniqueidentifier] NOT NULL,
	[esp_logo] [varchar](500) NOT NULL,
	[esp_primer_mensaje] [varchar](2000) NOT NULL,
	[esp_segundo_mensaje] [varchar](2000) NULL,
	[esp_tercer_mensaje] [varchar](2000) NULL,
	[esp_usuario_creacion] [uniqueidentifier] NOT NULL,
	[esp_fecha_creacion] [datetime] NOT NULL,
 CONSTRAINT [PK_adm_emisor_sucursal_plantilla] PRIMARY KEY CLUSTERED 
(
	[esp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_emisor_sucursal_resolucion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_emisor_sucursal_resolucion](
	[esr_id] [uniqueidentifier] NOT NULL,
	[esr_emisor] [uniqueidentifier] NOT NULL,
	[esr_sucursal] [uniqueidentifier] NOT NULL,
	[esr_resolucion] [uniqueidentifier] NOT NULL,
	[esr_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_emisor_sucursal_resolucion_1] PRIMARY KEY CLUSTERED 
(
	[esr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_grupo_emisor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_grupo_emisor](
	[gen_id] [uniqueidentifier] NOT NULL,
	[gen_nombre] [varchar](200) NOT NULL,
	[gen_direccion] [varchar](200) NULL,
	[gen_email] [varchar](200) NULL,
	[gen_telefono] [varchar](100) NULL,
	[gen_municipio] [uniqueidentifier] NOT NULL,
	[gen_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_grupo_emisor_1] PRIMARY KEY CLUSTERED 
(
	[gen_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_mandato]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_mandato](
	[man_id] [uniqueidentifier] NOT NULL,
	[man_identificacion] [varchar](15) NOT NULL,
	[man_digito] [int] NOT NULL,
	[man_tipo_identificacion] [uniqueidentifier] NOT NULL,
	[man_nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_adm_mandato] PRIMARY KEY CLUSTERED 
(
	[man_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_perfil]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_perfil](
	[per_id] [uniqueidentifier] NOT NULL,
	[per_descripcion] [varchar](50) NOT NULL,
	[per_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_perfil] PRIMARY KEY CLUSTERED 
(
	[per_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_perfil_usuario_receptor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_perfil_usuario_receptor](
	[pur_perfil] [uniqueidentifier] NOT NULL,
	[pur_usuario] [uniqueidentifier] NOT NULL,
	[pur_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_perfil_usuario_receptor] PRIMARY KEY CLUSTERED 
(
	[pur_perfil] ASC,
	[pur_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_plantilla]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_plantilla](
	[prg_id] [uniqueidentifier] NOT NULL,
	[prg_nombre] [varchar](200) NOT NULL,
	[prg_direccion_rdlc] [varchar](100) NOT NULL,
	[prg_direccion_pdf] [varchar](100) NOT NULL,
	[prg_privado] [bit] NOT NULL,
	[prg_tipo_documento] [uniqueidentifier] NOT NULL,
	[prg_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_plantilla] PRIMARY KEY CLUSTERED 
(
	[prg_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_tipo_retencion_fuente]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_tipo_retencion_fuente](
	[trf_id] [uniqueidentifier] NOT NULL,
	[trf_concepto_retencion] [varchar](max) NOT NULL,
	[trf_base_minima_UVT] [int] NOT NULL,
	[trf_base_minima_pesos] [int] NOT NULL,
	[trf_porcentajes] [numeric](18, 2) NOT NULL,
	[trf_id_impuesto] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__adm_tipo__B4BF4D55044939DA] PRIMARY KEY CLUSTERED 
(
	[trf_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_usuario]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_usuario](
	[usu_id] [uniqueidentifier] NOT NULL,
	[usu_usuario] [varchar](50) NOT NULL,
	[usu_contrasena] [varchar](200) NOT NULL,
	[usu_nombre] [varchar](70) NOT NULL,
	[usu_apellido] [varchar](70) NULL,
	[usu_activo] [bit] NOT NULL,
	[usu_directorio_activo] [bit] NOT NULL,
	[usu_email] [varchar](50) NULL,
	[usu_direccion] [varchar](100) NULL,
	[usu_telefono] [varchar](50) NULL,
	[usu_fecha_creacion] [datetime] NOT NULL,
	[usu_emisor_sucursal] [uniqueidentifier] NULL,
 CONSTRAINT [PK_dbo.AdmUsuario] PRIMARY KEY CLUSTERED 
(
	[usu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_usuario_perfil_tipo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_usuario_perfil_tipo](
	[upt_perfil] [uniqueidentifier] NOT NULL,
	[upt_tipo_usuario] [uniqueidentifier] NOT NULL,
	[upt_usuario] [uniqueidentifier] NOT NULL,
	[upt_persona] [uniqueidentifier] NULL,
	[upt_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_perfil_tipo_usuario] PRIMARY KEY CLUSTERED 
(
	[upt_perfil] ASC,
	[upt_tipo_usuario] ASC,
	[upt_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_usuario_receptor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_usuario_receptor](
	[ure_id] [uniqueidentifier] NOT NULL,
	[ure_usuario] [varchar](50) NOT NULL,
	[ure_contrasena] [varchar](200) NOT NULL,
	[ure_nombre] [varchar](200) NOT NULL,
	[ure_apellido] [varchar](200) NULL,
	[ure_activo] [bit] NOT NULL,
	[ure_directorio_activo] [bit] NOT NULL,
	[ure_email] [varchar](100) NULL,
	[ure_direccion] [varchar](100) NULL,
	[ure_telefono] [varchar](100) NULL,
	[ure_fecha_creacion] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.adm_usuario_receptor] PRIMARY KEY CLUSTERED 
(
	[ure_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adm_usuario_sucursal]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adm_usuario_sucursal](
	[usu_usuario] [uniqueidentifier] NOT NULL,
	[usu_sucursal] [uniqueidentifier] NOT NULL,
	[usu_activo] [bit] NOT NULL,
 CONSTRAINT [PK_adm_usuario_sucursal] PRIMARY KEY CLUSTERED 
(
	[usu_usuario] ASC,
	[usu_sucursal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogIngreso]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogIngreso](
	[IdAuditLogIngreso] [uniqueidentifier] NOT NULL,
	[Usuario] [varchar](300) NULL,
	[Fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAuditLogIngreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogReceptor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogReceptor](
	[alc_id] [uniqueidentifier] NOT NULL,
	[alc_tipo_accion] [uniqueidentifier] NULL,
	[alc_receptor] [uniqueidentifier] NULL,
	[alc_emisor_accion] [uniqueidentifier] NULL,
	[alc_fecha_accion] [datetime] NULL,
	[alc_tipo_receptor] [varchar](100) NULL,
	[alc_tipo_persona] [uniqueidentifier] NULL,
	[alc_tipo_identificacion] [uniqueidentifier] NULL,
	[alc_identificacion] [varchar](100) NULL,
	[alc_contrasena] [varchar](100) NULL,
	[alc_digito] [varchar](20) NULL,
	[alc_nombre] [varchar](200) NULL,
	[alc_razon_social] [varchar](200) NULL,
	[alc_pais] [uniqueidentifier] NULL,
	[alc_departamento] [uniqueidentifier] NULL,
	[alc_municipio] [uniqueidentifier] NULL,
	[alc_codigo_postal] [varchar](100) NULL,
	[alc_correo] [varchar](100) NULL,
	[alc_direccion] [varchar](100) NULL,
	[alc_telefono] [varchar](100) NULL,
	[alc_activo] [bit] NULL,
	[alc_matricula_mercantil] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[alc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[IdAuditLog] [varchar](38) NOT NULL,
	[IpUsuario] [nvarchar](255) NULL,
	[Usuario] [nvarchar](255) NULL,
	[Fecha] [datetime] NULL,
	[Accion] [nvarchar](255) NULL,
	[Tabla] [nvarchar](255) NULL,
	[RegistroTabla] [nvarchar](255) NULL,
	[Campo] [nvarchar](255) NULL,
	[ValorAnterior] [nvarchar](max) NULL,
	[ValorNuevo] [nvarchar](max) NULL,
	[PCName] [nvarchar](255) NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Created] [datetimeoffset](7) NULL,
	[ModifiedBy] [nvarchar](255) NULL,
	[Modified] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED 
(
	[IdAuditLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[delete_retencion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[delete_retencion](
	[retencion] [uniqueidentifier] NULL,
	[conceptor] [varchar](500) NULL,
	[porcentaje] [numeric](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[log_receptor_nombre]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[log_receptor_nombre](
	[rno_id] [uniqueidentifier] NOT NULL,
	[rno_id_receptor] [uniqueidentifier] NULL,
	[rno_id_documento] [uniqueidentifier] NULL,
	[rno_nombre] [varchar](200) NULL,
	[rno_fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[rno_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_ciiu]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_ciiu](
	[cii_id] [uniqueidentifier] NOT NULL,
	[cii_codigo] [varchar](80) NOT NULL,
	[cii_descripcion] [varchar](500) NOT NULL,
 CONSTRAINT [PK_sys_ciiu_1] PRIMARY KEY CLUSTERED 
(
	[cii_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_codigo_postal]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_codigo_postal](
	[cpo_id] [int] NOT NULL,
	[cpo_codigo] [varchar](80) NOT NULL,
 CONSTRAINT [PK_sys_codigo_postal] PRIMARY KEY CLUSTERED 
(
	[cpo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_condicion_entrega]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_condicion_entrega](
	[cen_id] [uniqueidentifier] NOT NULL,
	[cen_codigo] [varchar](10) NOT NULL,
	[cen_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_condicion_entrega] PRIMARY KEY CLUSTERED 
(
	[cen_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_descuento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_descuento](
	[des_id] [uniqueidentifier] NOT NULL,
	[des_codigo] [varchar](10) NOT NULL,
	[des_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_descuento] PRIMARY KEY CLUSTERED 
(
	[des_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_estado_documento_correo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_estado_documento_correo](
	[edc_id] [uniqueidentifier] NOT NULL,
	[edc_descripcion] [varchar](200) NULL,
	[edc_activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[edc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_moneda]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_moneda](
	[mon_id] [uniqueidentifier] NOT NULL,
	[mon_codigo] [varchar](10) NOT NULL,
	[mon_divisa] [varchar](100) NOT NULL,
	[mon_descripcion] [varchar](50) NOT NULL,
	[mon_activo] [bit] NOT NULL,
 CONSTRAINT [PK_sys_moneda_1] PRIMARY KEY CLUSTERED 
(
	[mon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_origen_documento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_origen_documento](
	[odo_id] [uniqueidentifier] NOT NULL,
	[odo_descripcion] [varchar](100) NULL,
	[odo_activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[odo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_pais]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_pais](
	[pai_id] [uniqueidentifier] NOT NULL,
	[pai_nombre_comun] [varchar](50) NULL,
	[pai_nombre_iso] [varchar](100) NOT NULL,
	[pai_codigo_2] [varchar](50) NOT NULL,
	[pai_codigo_3] [varchar](50) NOT NULL,
	[pai_codigo_numerico] [varchar](50) NOT NULL,
	[pai_observacion] [varchar](100) NULL,
 CONSTRAINT [PK_sys_pais_1] PRIMARY KEY CLUSTERED 
(
	[pai_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_reglas_dian]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_reglas_dian](
	[rdi_id] [uniqueidentifier] NOT NULL,
	[rdi_regla] [varchar](20) NULL,
	[rdi_descripcion_dian] [varchar](2000) NULL,
	[rdi_descripcion_facse] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[rdi_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_software]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_software](
	[sof_id] [uniqueidentifier] NOT NULL,
	[sof_pin] [varchar](200) NULL,
	[sof_url] [varchar](200) NOT NULL,
	[sof_contrasena] [varchar](200) NULL,
	[sof_usuario] [uniqueidentifier] NULL,
	[sof_fecha] [datetime] NOT NULL,
	[sof_nombre] [varchar](80) NOT NULL,
	[sof_id_sofware] [varchar](200) NOT NULL,
	[sof_activo] [bit] NOT NULL,
 CONSTRAINT [PK_sys_software_1] PRIMARY KEY CLUSTERED 
(
	[sof_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_accion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_accion](
	[tac_id] [uniqueidentifier] NOT NULL,
	[tac_descripcion] [varchar](100) NULL,
	[tac_activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[tac_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_catalogo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_catalogo](
	[tca_id] [uniqueidentifier] NOT NULL,
	[tca_nombre] [varchar](20) NOT NULL,
 CONSTRAINT [PK_sys_tipo_catalogo_1] PRIMARY KEY CLUSTERED 
(
	[tca_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_clasificacion_impuesto]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_clasificacion_impuesto](
	[tci_id] [uniqueidentifier] NOT NULL,
	[tci_descripcion] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[tci_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_contingencia]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_contingencia](
	[tco_id] [uniqueidentifier] NOT NULL,
	[tco_id_software] [uniqueidentifier] NULL,
	[tco_descripcion] [varchar](300) NOT NULL,
	[tco_codigo] [varchar](10) NOT NULL,
	[tco_activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tco_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_contrato]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_contrato](
	[tco_id] [uniqueidentifier] NOT NULL,
	[tco_descripcion] [varchar](50) NOT NULL,
	[tco_mes] [int] NOT NULL,
	[tco_activo] [int] NOT NULL,
 CONSTRAINT [PK_sys_tipo_contrato] PRIMARY KEY CLUSTERED 
(
	[tco_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_correcion_nota_credito]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_correcion_nota_credito](
	[cnc_id] [uniqueidentifier] NOT NULL,
	[cnc_codigo] [varchar](10) NOT NULL,
	[cnc_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_tipo_correcion_nota_credito] PRIMARY KEY CLUSTERED 
(
	[cnc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_correcion_nota_debito]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_correcion_nota_debito](
	[cnd_id] [uniqueidentifier] NOT NULL,
	[cnd_codigo] [varchar](10) NOT NULL,
	[cnd_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_tipo_correcion_nota_debito] PRIMARY KEY CLUSTERED 
(
	[cnd_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_correo]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_correo](
	[tco_id] [uniqueidentifier] NOT NULL,
	[tco_descripcion] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[tco_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_dato]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_dato](
	[tda_id] [uniqueidentifier] NOT NULL,
	[tda_descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_sys_tipo_dato] PRIMARY KEY CLUSTERED 
(
	[tda_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_emisor]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_emisor](
	[tem_id] [uniqueidentifier] NOT NULL,
	[tem_codigo] [varchar](80) NOT NULL,
	[tem_descripcion] [varchar](500) NOT NULL,
	[tem_activo] [bit] NOT NULL,
 CONSTRAINT [PK_sys_tipos_emisor_1] PRIMARY KEY CLUSTERED 
(
	[tem_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_evento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_evento](
	[tev_id] [uniqueidentifier] NOT NULL,
	[tev_codigo] [varchar](10) NOT NULL,
	[tev_descripcion] [varchar](100) NOT NULL,
	[tev_responsable_registro] [varchar](150) NOT NULL,
 CONSTRAINT [PK_sys_tipo_eventos] PRIMARY KEY CLUSTERED 
(
	[tev_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_forma_pago]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_forma_pago](
	[tfp_id] [uniqueidentifier] NOT NULL,
	[tfp_codigo] [varchar](10) NOT NULL,
	[tfp_descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_sys_tipo_forma_pago] PRIMARY KEY CLUSTERED 
(
	[tfp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_impuesto]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_impuesto](
	[tim_id] [uniqueidentifier] NOT NULL,
	[tim_codigo] [varchar](10) NOT NULL,
	[tim_nombre] [varchar](100) NOT NULL,
	[tim_descripcion] [varchar](200) NULL,
	[tim_porcentaje] [numeric](18, 4) NULL,
	[tim_clasificacion] [uniqueidentifier] NULL,
 CONSTRAINT [PK_sys_tipo_impuesto] PRIMARY KEY CLUSTERED 
(
	[tim_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_medio_pago]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_medio_pago](
	[tmp_id] [uniqueidentifier] NOT NULL,
	[tmp_codigo] [varchar](10) NOT NULL,
	[tmp_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_tipo_medio_pago] PRIMARY KEY CLUSTERED 
(
	[tmp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_negociacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_negociacion](
	[tne_id] [uniqueidentifier] NOT NULL,
	[tne_descripcion] [varchar](50) NOT NULL,
	[tne_activo] [int] NOT NULL,
 CONSTRAINT [PK_sys_tipo_negociacion] PRIMARY KEY CLUSTERED 
(
	[tne_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_notificacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_notificacion](
	[tno_id] [uniqueidentifier] NOT NULL,
	[tno_descripcion] [varchar](100) NOT NULL,
	[tno_ruta_imagen] [varchar](100) NULL,
	[tno_activo] [bit] NOT NULL,
 CONSTRAINT [PK__sys_tipo__FD50E04CDC54FF84] PRIMARY KEY CLUSTERED 
(
	[tno_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_operacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_operacion](
	[top_id] [uniqueidentifier] NOT NULL,
	[top_codigo] [varchar](10) NOT NULL,
	[top_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_tipo_operacion] PRIMARY KEY CLUSTERED 
(
	[top_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_organizacion]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_organizacion](
	[tor_id] [uniqueidentifier] NOT NULL,
	[tor_codigo] [varchar](10) NOT NULL,
	[tor_nombre] [varchar](200) NOT NULL,
 CONSTRAINT [PK_sys_tipo_organizacion_1] PRIMARY KEY CLUSTERED 
(
	[tor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_persona]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_persona](
	[tpe_id] [uniqueidentifier] NOT NULL,
	[tpe_codigo] [varchar](5) NOT NULL,
	[tpe_descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_sys_tipo_persona] PRIMARY KEY CLUSTERED 
(
	[tpe_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_unidad_cantidad]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_unidad_cantidad](
	[tuc_id] [uniqueidentifier] NOT NULL,
	[tuc_codigo] [varchar](5) NOT NULL,
	[tuc_descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_sys_tipo_unidad_cantidad] PRIMARY KEY CLUSTERED 
(
	[tuc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_tipo_usuario]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_tipo_usuario](
	[tus_id] [uniqueidentifier] NOT NULL,
	[tus_descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_sys_tipo_usuario] PRIMARY KEY CLUSTERED 
(
	[tus_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmp_documento]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp_documento](
	[tdo_consecutivo] [int] NOT NULL,
	[tdo_id] [uniqueidentifier] NOT NULL,
	[tdo_emisor_sucursal] [uniqueidentifier] NULL,
	[tdo_tipo_documento] [uniqueidentifier] NOT NULL,
	[tdo_json] [varchar](max) NOT NULL,
	[tdo_fecha_creacion] [datetime] NOT NULL,
	[tdo_usuario] [uniqueidentifier] NOT NULL,
	[tdo_subtotal] [decimal](18, 0) NULL,
	[tdo_valor_total] [decimal](18, 0) NULL,
	[tdo_id_receptor] [varchar](max) NULL,
	[tdo_id_impuesto] [decimal](18, 0) NULL,
	[tdo_nit] [varchar](max) NULL,
	[tdo_prefijo] [varchar](10) NULL,
	[tdo_numero] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[tdo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[adm_distriduidor] ADD  CONSTRAINT [DF_adm_distriduidor_dis_activo]  DEFAULT ((1)) FOR [dis_activo]
GO
ALTER TABLE [dbo].[adm_distriduidor] ADD  CONSTRAINT [DF_adm_distriduidor_dis_fecha_creacion]  DEFAULT (getdate()) FOR [dis_fecha_creacion]
GO
ALTER TABLE [dbo].[adm_documento] ADD  CONSTRAINT [DF_adm_documento_doc_valor_total]  DEFAULT ((0)) FOR [doc_valor_total]
GO
ALTER TABLE [dbo].[adm_documento] ADD  CONSTRAINT [DF_adm_documento_doc_valor_impuestos]  DEFAULT ((0)) FOR [doc_valor_impuestos]
GO
ALTER TABLE [dbo].[adm_documento_correo] ADD  CONSTRAINT [DF_adm_documento_correo_dco_fecha]  DEFAULT (getdate()) FOR [dco_fecha]
GO
ALTER TABLE [dbo].[adm_documento_file] ADD  CONSTRAINT [DF_adm_documento_file_dfi_fecha]  DEFAULT (getdate()) FOR [dfi_fecha]
GO
ALTER TABLE [dbo].[adm_documento_notificacion] ADD  CONSTRAINT [DF_adm_documento_notificacion_dno_fecha]  DEFAULT (getdate()) FOR [dno_fecha]
GO
ALTER TABLE [dbo].[adm_documento_proveedor] ADD  CONSTRAINT [DF_adm_documento_proveedor_dpr_valor_total]  DEFAULT ((0)) FOR [dpr_valor_total]
GO
ALTER TABLE [dbo].[adm_documento_proveedor] ADD  CONSTRAINT [DF_adm_documento_proveedor_dpr_valor_impuestos]  DEFAULT ((0)) FOR [dpr_valor_impuestos]
GO
ALTER TABLE [dbo].[adm_documento_proveedor_file] ADD  CONSTRAINT [DF_adm_documento_proveedor_file_dpf_fecha]  DEFAULT (getdate()) FOR [dpf_fecha]
GO
ALTER TABLE [dbo].[adm_emisor] ADD  CONSTRAINT [DF_adm_emisor_emi_activo]  DEFAULT ((1)) FOR [emi_activo]
GO
ALTER TABLE [dbo].[adm_emisor_certificado] ADD  CONSTRAINT [DF_adm_emisor_certificado_ece_activo]  DEFAULT ((1)) FOR [ece_activo]
GO
ALTER TABLE [dbo].[adm_emisor_contrato] ADD  CONSTRAINT [DF_adm_emisor_contrato_eco_valor]  DEFAULT ((0)) FOR [eco_valor]
GO
ALTER TABLE [dbo].[adm_emisor_contrato] ADD  CONSTRAINT [DF_adm_emisor_contrato_eco_cantidad_documento]  DEFAULT ((0)) FOR [eco_cantidad_documento]
GO
ALTER TABLE [dbo].[adm_emisor_correo] ADD  CONSTRAINT [DF_adm_emisor_correo_eco_activo]  DEFAULT ((1)) FOR [eco_activo]
GO
ALTER TABLE [dbo].[adm_emisor_producto] ADD  CONSTRAINT [DF_adm_emisor_producto_epr_valor_unitario]  DEFAULT ((0)) FOR [epr_valor_unitario]
GO
ALTER TABLE [dbo].[adm_emisor_producto] ADD  CONSTRAINT [DF_adm_emisor_producto_epr_activo]  DEFAULT ((1)) FOR [epr_activo]
GO
ALTER TABLE [dbo].[adm_emisor_resolucion] ADD  CONSTRAINT [DF_adm_emisor_resolucion_ere_activo]  DEFAULT ((1)) FOR [ere_activo]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal] ADD  CONSTRAINT [DF_adm_emisor_sucursal_esu_activo]  DEFAULT ((1)) FOR [esu_activo]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion] ADD  CONSTRAINT [DF_adm_emisor_sucursal_resolucion_esr_activo]  DEFAULT ((1)) FOR [esr_activo]
GO
ALTER TABLE [dbo].[adm_grupo_emisor] ADD  CONSTRAINT [DF_adm_grupo_emisor_gen_activo]  DEFAULT ((1)) FOR [gen_activo]
GO
ALTER TABLE [dbo].[adm_proveedor] ADD  CONSTRAINT [DF_adm_proveedor_pro_fecha_recepcion]  DEFAULT (getdate()) FOR [pro_fecha_recepcion]
GO
ALTER TABLE [dbo].[adm_proveedor] ADD  CONSTRAINT [DF_adm_proveedor_pro_activo]  DEFAULT ((1)) FOR [pro_activo]
GO
ALTER TABLE [dbo].[adm_receptor] ADD  CONSTRAINT [DF_adm_receptor_rec_fecha_receccion]  DEFAULT (getdate()) FOR [rec_fecha_receccion]
GO
ALTER TABLE [dbo].[adm_receptor] ADD  CONSTRAINT [DF_adm_receptor_rec_activo]  DEFAULT ((1)) FOR [rec_activo]
GO
ALTER TABLE [dbo].[sys_tipo_contrato] ADD  CONSTRAINT [DF_sys_tipo_contrato_tco_mes]  DEFAULT ((0)) FOR [tco_mes]
GO
ALTER TABLE [dbo].[sys_tipo_contrato] ADD  CONSTRAINT [DF_sys_tipo_contrato_tco_activo]  DEFAULT ((1)) FOR [tco_activo]
GO
ALTER TABLE [dbo].[adm_centinela]  WITH CHECK ADD  CONSTRAINT [FK_adm_centinela__adm_emisor_sucursal] FOREIGN KEY([cen_id_emisor_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_centinela] CHECK CONSTRAINT [FK_adm_centinela__adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_distriduidor]  WITH CHECK ADD  CONSTRAINT [FK_adm_distriduidor_sys_municipio] FOREIGN KEY([dis_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[adm_distriduidor] CHECK CONSTRAINT [FK_adm_distriduidor_sys_municipio]
GO
ALTER TABLE [dbo].[adm_distriduidor]  WITH CHECK ADD  CONSTRAINT [FK_adm_distriduidor_sys_tipo_identificacion] FOREIGN KEY([dis_tipo_identificacion])
REFERENCES [dbo].[sys_tipo_identificacion] ([tid_id])
GO
ALTER TABLE [dbo].[adm_distriduidor] CHECK CONSTRAINT [FK_adm_distriduidor_sys_tipo_identificacion]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento__adm_emisor_resolucion] FOREIGN KEY([doc_resolucion])
REFERENCES [dbo].[adm_emisor_resolucion] ([ere_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento__adm_emisor_resolucion]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento__sys_estado_documento_facse] FOREIGN KEY([doc_id_estado_facse])
REFERENCES [dbo].[sys_estado_documento_facse] ([edf_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento__sys_estado_documento_facse]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento__sys_origen_documento] FOREIGN KEY([doc_id_origen_documento])
REFERENCES [dbo].[sys_origen_documento] ([odo_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento__sys_origen_documento]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_adm_emisor] FOREIGN KEY([doc_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_adm_emisor]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_adm_emisor_certificado] FOREIGN KEY([doc_emisor_certificado])
REFERENCES [dbo].[adm_emisor_certificado] ([ece_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_adm_emisor_certificado]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_adm_emisor_sucursal] FOREIGN KEY([doc_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_adm_receptor] FOREIGN KEY([doc_receptor])
REFERENCES [dbo].[adm_receptor] ([rec_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_adm_receptor]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_sys_estado_documento] FOREIGN KEY([doc_estado])
REFERENCES [dbo].[sys_estado_documento] ([ted_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_sys_estado_documento]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_sys_moneda] FOREIGN KEY([doc_moneda])
REFERENCES [dbo].[sys_moneda] ([mon_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_sys_moneda]
GO
ALTER TABLE [dbo].[adm_documento]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_sys_tipo_documento] FOREIGN KEY([doc_tipo_documento])
REFERENCES [dbo].[sys_tipo_documento] ([tdo_id])
GO
ALTER TABLE [dbo].[adm_documento] CHECK CONSTRAINT [FK_adm_documento_sys_tipo_documento]
GO
ALTER TABLE [dbo].[adm_documento_correo]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_correo__sys_estado_documento_correo] FOREIGN KEY([dco_estado])
REFERENCES [dbo].[sys_estado_documento_correo] ([edc_id])
GO
ALTER TABLE [dbo].[adm_documento_correo] CHECK CONSTRAINT [FK_adm_documento_correo__sys_estado_documento_correo]
GO
ALTER TABLE [dbo].[adm_documento_correo]  WITH CHECK ADD  CONSTRAINT [FK_adm_documento_correo_adm_documento] FOREIGN KEY([dco_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documento_correo] CHECK CONSTRAINT [FK_adm_documento_correo_adm_documento]
GO
ALTER TABLE [dbo].[adm_documento_estado_facse]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_estado_facse__adm_documento] FOREIGN KEY([def_id_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documento_estado_facse] NOCHECK CONSTRAINT [FK_adm_documento_estado_facse__adm_documento]
GO
ALTER TABLE [dbo].[adm_documento_estado_facse]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_estado_facse__sys_estado_documento_facse] FOREIGN KEY([def_id_estado])
REFERENCES [dbo].[sys_estado_documento_facse] ([edf_id])
GO
ALTER TABLE [dbo].[adm_documento_estado_facse] NOCHECK CONSTRAINT [FK_adm_documento_estado_facse__sys_estado_documento_facse]
GO
ALTER TABLE [dbo].[adm_documento_eventos]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_eventos_adm_documento] FOREIGN KEY([dev_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documento_eventos] NOCHECK CONSTRAINT [FK_adm_documento_eventos_adm_documento]
GO
ALTER TABLE [dbo].[adm_documento_eventos]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_eventos_sys_tipo_eventos] FOREIGN KEY([dev_evento])
REFERENCES [dbo].[sys_tipo_evento] ([tev_id])
GO
ALTER TABLE [dbo].[adm_documento_eventos] NOCHECK CONSTRAINT [FK_adm_documento_eventos_sys_tipo_eventos]
GO
ALTER TABLE [dbo].[adm_documento_file]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_file_adm_documento] FOREIGN KEY([dfi_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documento_file] NOCHECK CONSTRAINT [FK_adm_documento_file_adm_documento]
GO
ALTER TABLE [dbo].[adm_documento_file_historico]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento__adm_documento_file_historico] FOREIGN KEY([dfh_id_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documento_file_historico] NOCHECK CONSTRAINT [FK_adm_documento__adm_documento_file_historico]
GO
ALTER TABLE [dbo].[adm_documento_notificacion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_notificacion__sys_reglas_dian] FOREIGN KEY([dno_id_regla])
REFERENCES [dbo].[sys_reglas_dian] ([rdi_id])
GO
ALTER TABLE [dbo].[adm_documento_notificacion] NOCHECK CONSTRAINT [FK_adm_documento_notificacion__sys_reglas_dian]
GO
ALTER TABLE [dbo].[adm_documento_notificacion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_notificacion_adm_documento] FOREIGN KEY([dno_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documento_notificacion] NOCHECK CONSTRAINT [FK_adm_documento_notificacion_adm_documento]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_adm_proveedor] FOREIGN KEY([dpr_proveedor])
REFERENCES [dbo].[adm_proveedor] ([pro_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_adm_documento_adm_proveedor]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_adm_emisor] FOREIGN KEY([dpr_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_adm_emisor]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_adm_emisor_sucursal] FOREIGN KEY([dpr_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_sys_moneda] FOREIGN KEY([dpr_moneda])
REFERENCES [dbo].[sys_moneda] ([mon_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_sys_moneda]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_sys_tipo_documento] FOREIGN KEY([dpr_tipo_documento])
REFERENCES [dbo].[sys_tipo_documento] ([tdo_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_sys_tipo_documento]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_documento_proveedor_sys_estado_documento_facse] FOREIGN KEY([dpr_id_estado_facse])
REFERENCES [dbo].[sys_estado_documento_facse] ([edf_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_documento_proveedor_sys_estado_documento_facse]
GO
ALTER TABLE [dbo].[adm_documento_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_documento_proveedor_sys_origen_documento] FOREIGN KEY([dpr_id_origen_documento])
REFERENCES [dbo].[sys_origen_documento] ([odo_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor] NOCHECK CONSTRAINT [FK_documento_proveedor_sys_origen_documento]
GO
ALTER TABLE [dbo].[adm_documento_proveedor_anexo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_anexo_adm_documento_proveedor] FOREIGN KEY([dpa_documento_proveedor])
REFERENCES [dbo].[adm_documento_proveedor] ([dpr_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor_anexo] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_anexo_adm_documento_proveedor]
GO
ALTER TABLE [dbo].[adm_documento_proveedor_estado_facse]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_estado_facse_adm_documento_proveedor] FOREIGN KEY([dpe_documento_proveedor])
REFERENCES [dbo].[adm_documento_proveedor] ([dpr_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor_estado_facse] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_estado_facse_adm_documento_proveedor]
GO
ALTER TABLE [dbo].[adm_documento_proveedor_estado_facse]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_estado_facse_sys_estado_documento_facse] FOREIGN KEY([dpe_id_estado])
REFERENCES [dbo].[sys_estado_documento_facse] ([edf_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor_estado_facse] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_estado_facse_sys_estado_documento_facse]
GO
ALTER TABLE [dbo].[adm_documento_proveedor_file]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documento_proveedor_file_adm_documento_proveedor] FOREIGN KEY([dpf_documento_proveedor])
REFERENCES [dbo].[adm_documento_proveedor] ([dpr_id])
GO
ALTER TABLE [dbo].[adm_documento_proveedor_file] NOCHECK CONSTRAINT [FK_adm_documento_proveedor_file_adm_documento_proveedor]
GO
ALTER TABLE [dbo].[adm_documentos_anexo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_documentos_anexo_adm_documento] FOREIGN KEY([dan_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[adm_documentos_anexo] NOCHECK CONSTRAINT [FK_adm_documentos_anexo_adm_documento]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor__adm_mandato] FOREIGN KEY([emi_mandato])
REFERENCES [dbo].[adm_mandato] ([man_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor__adm_mandato]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_adm_distriduidor] FOREIGN KEY([emi_distribuidor])
REFERENCES [dbo].[adm_distriduidor] ([dis_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_adm_distriduidor]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_adm_grupo_emisor] FOREIGN KEY([emi_grupo])
REFERENCES [dbo].[adm_grupo_emisor] ([gen_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_adm_grupo_emisor]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_ciiu] FOREIGN KEY([emi_ciiu])
REFERENCES [dbo].[sys_ciiu] ([cii_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_ciiu]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_departamentos] FOREIGN KEY([emi_departamento])
REFERENCES [dbo].[sys_departamento] ([dep_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_departamentos]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_municipio] FOREIGN KEY([emi_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_municipio]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_pais] FOREIGN KEY([emi_pais])
REFERENCES [dbo].[sys_pais] ([pai_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_pais]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_software] FOREIGN KEY([emi_sofware])
REFERENCES [dbo].[sys_software] ([sof_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_software]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_tipo_emisor] FOREIGN KEY([emi_tipo_emisor])
REFERENCES [dbo].[sys_tipo_emisor] ([tem_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_tipo_emisor]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_tipo_identificacion] FOREIGN KEY([emi_tipo_identificacion])
REFERENCES [dbo].[sys_tipo_identificacion] ([tid_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_tipo_identificacion]
GO
ALTER TABLE [dbo].[adm_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sys_tipo_persona] FOREIGN KEY([emi_tipo_persona])
REFERENCES [dbo].[sys_tipo_persona] ([tpe_id])
GO
ALTER TABLE [dbo].[adm_emisor] NOCHECK CONSTRAINT [FK_adm_emisor_sys_tipo_persona]
GO
ALTER TABLE [dbo].[adm_emisor_catalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_catalogo_adm_emisor] FOREIGN KEY([eca_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_catalogo] NOCHECK CONSTRAINT [FK_adm_emisor_catalogo_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_catalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_catalogo_sys_tipo_catalogo] FOREIGN KEY([eca_tipo_catalogo])
REFERENCES [dbo].[sys_tipo_catalogo] ([tca_id])
GO
ALTER TABLE [dbo].[adm_emisor_catalogo] NOCHECK CONSTRAINT [FK_adm_emisor_catalogo_sys_tipo_catalogo]
GO
ALTER TABLE [dbo].[adm_emisor_catalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_catalogo_sys_tipo_dato] FOREIGN KEY([eca_tipo_dato])
REFERENCES [dbo].[sys_tipo_dato] ([tda_id])
GO
ALTER TABLE [dbo].[adm_emisor_catalogo] NOCHECK CONSTRAINT [FK_adm_emisor_catalogo_sys_tipo_dato]
GO
ALTER TABLE [dbo].[adm_emisor_catalogo_lista]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_catalogo_lista_adm_emisor_catalogo] FOREIGN KEY([ecl_emisor_catalogo])
REFERENCES [dbo].[adm_emisor_catalogo] ([eca_id])
GO
ALTER TABLE [dbo].[adm_emisor_catalogo_lista] NOCHECK CONSTRAINT [FK_adm_emisor_catalogo_lista_adm_emisor_catalogo]
GO
ALTER TABLE [dbo].[adm_emisor_certificado]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_certificado_adm_emisor] FOREIGN KEY([ece_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_certificado] NOCHECK CONSTRAINT [FK_adm_emisor_certificado_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_contrato]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_contrato_adm_emisor] FOREIGN KEY([eco_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_contrato] NOCHECK CONSTRAINT [FK_adm_emisor_contrato_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_contrato]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_contrato_sys_tipo_contrato] FOREIGN KEY([eco_tipo_contrato])
REFERENCES [dbo].[sys_tipo_contrato] ([tco_id])
GO
ALTER TABLE [dbo].[adm_emisor_contrato] NOCHECK CONSTRAINT [FK_adm_emisor_contrato_sys_tipo_contrato]
GO
ALTER TABLE [dbo].[adm_emisor_contrato]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_contrato_sys_tipo_negociacion] FOREIGN KEY([eco_tipo_negociacion])
REFERENCES [dbo].[sys_tipo_negociacion] ([tne_id])
GO
ALTER TABLE [dbo].[adm_emisor_contrato] NOCHECK CONSTRAINT [FK_adm_emisor_contrato_sys_tipo_negociacion]
GO
ALTER TABLE [dbo].[adm_emisor_correo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_correo__sys_tipo_correo] FOREIGN KEY([eco_tipo_correo])
REFERENCES [dbo].[sys_tipo_correo] ([tco_id])
GO
ALTER TABLE [dbo].[adm_emisor_correo] NOCHECK CONSTRAINT [FK_adm_emisor_correo__sys_tipo_correo]
GO
ALTER TABLE [dbo].[adm_emisor_correo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_correo_adm_emisor] FOREIGN KEY([eco_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_correo] NOCHECK CONSTRAINT [FK_adm_emisor_correo_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_impuesto]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_impuesto__adm_emisor] FOREIGN KEY([emi_id_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_impuesto] NOCHECK CONSTRAINT [FK_adm_emisor_impuesto__adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_impuesto]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_impuesto__sys_tipo_impuesto] FOREIGN KEY([emi_id_impuesto])
REFERENCES [dbo].[sys_tipo_impuesto] ([tim_id])
GO
ALTER TABLE [dbo].[adm_emisor_impuesto] NOCHECK CONSTRAINT [FK_adm_emisor_impuesto__sys_tipo_impuesto]
GO
ALTER TABLE [dbo].[adm_emisor_notificacion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_notificacion__adm_emisor_sucursal] FOREIGN KEY([eno_id_emisor])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_emisor_notificacion] NOCHECK CONSTRAINT [FK_adm_emisor_notificacion__adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_emisor_notificacion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_notificacion__sys_tipo_notificacion] FOREIGN KEY([eno_id_tipo_notificacion])
REFERENCES [dbo].[sys_tipo_notificacion] ([tno_id])
GO
ALTER TABLE [dbo].[adm_emisor_notificacion] NOCHECK CONSTRAINT [FK_adm_emisor_notificacion__sys_tipo_notificacion]
GO
ALTER TABLE [dbo].[adm_emisor_producto]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_producto_adm_emisor] FOREIGN KEY([epr_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_producto] NOCHECK CONSTRAINT [FK_adm_emisor_producto_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_producto]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_producto_sys_tipo_impuesto] FOREIGN KEY([epr_tipo_impuesto])
REFERENCES [dbo].[sys_tipo_impuesto] ([tim_id])
GO
ALTER TABLE [dbo].[adm_emisor_producto] NOCHECK CONSTRAINT [FK_adm_emisor_producto_sys_tipo_impuesto]
GO
ALTER TABLE [dbo].[adm_emisor_producto]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_producto_sys_tipo_unidad_cantidad] FOREIGN KEY([epr_unidad])
REFERENCES [dbo].[sys_tipo_unidad_cantidad] ([tuc_id])
GO
ALTER TABLE [dbo].[adm_emisor_producto] NOCHECK CONSTRAINT [FK_adm_emisor_producto_sys_tipo_unidad_cantidad]
GO
ALTER TABLE [dbo].[adm_emisor_producto_catalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_producto_catalogo_adm_emisor_catalogo] FOREIGN KEY([epc_catalogo])
REFERENCES [dbo].[adm_emisor_catalogo] ([eca_id])
GO
ALTER TABLE [dbo].[adm_emisor_producto_catalogo] NOCHECK CONSTRAINT [FK_adm_emisor_producto_catalogo_adm_emisor_catalogo]
GO
ALTER TABLE [dbo].[adm_emisor_producto_catalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_producto_catalogo_adm_emisor_producto] FOREIGN KEY([epc_producto])
REFERENCES [dbo].[adm_emisor_producto] ([epr_id])
GO
ALTER TABLE [dbo].[adm_emisor_producto_catalogo] NOCHECK CONSTRAINT [FK_adm_emisor_producto_catalogo_adm_emisor_producto]
GO
ALTER TABLE [dbo].[adm_emisor_resolucion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_resolucion_adm_emisor] FOREIGN KEY([ere_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_resolucion] NOCHECK CONSTRAINT [FK_adm_emisor_resolucion_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_resolucion]  WITH CHECK ADD  CONSTRAINT [FK_adm_emisor_resolucion_adm_plantilla] FOREIGN KEY([ere_plantilla])
REFERENCES [dbo].[adm_plantilla] ([prg_id])
GO
ALTER TABLE [dbo].[adm_emisor_resolucion] CHECK CONSTRAINT [FK_adm_emisor_resolucion_adm_plantilla]
GO
ALTER TABLE [dbo].[adm_emisor_resolucion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_resolucion_sys_tipo_documento] FOREIGN KEY([ere_tipo_documento])
REFERENCES [dbo].[sys_tipo_documento] ([tdo_id])
GO
ALTER TABLE [dbo].[adm_emisor_resolucion] NOCHECK CONSTRAINT [FK_adm_emisor_resolucion_sys_tipo_documento]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK__adm_emiso__esu_c__336AA144] FOREIGN KEY([esu_correo_entrada])
REFERENCES [dbo].[adm_emisor_correo] ([eco_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal] NOCHECK CONSTRAINT [FK__adm_emiso__esu_c__336AA144]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK__adm_emiso__esu_c__345EC57D] FOREIGN KEY([esu_correo_salida])
REFERENCES [dbo].[adm_emisor_correo] ([eco_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal] NOCHECK CONSTRAINT [FK__adm_emiso__esu_c__345EC57D]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_adm_emisor] FOREIGN KEY([esu_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_sys_departamento] FOREIGN KEY([esu_departamento])
REFERENCES [dbo].[sys_departamento] ([dep_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_sys_departamento]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_sys_municipio] FOREIGN KEY([esu_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_sys_municipio]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_json]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_json__adm_emisor_sucursal] FOREIGN KEY([esj_id_emisor_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_json] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_json__adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_plantilla]  WITH CHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_plantilla_adm_emisor_sucursal_plantilla] FOREIGN KEY([esp_emisor_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_plantilla] CHECK CONSTRAINT [FK_adm_emisor_sucursal_plantilla_adm_emisor_sucursal_plantilla]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_plantilla]  WITH CHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_plantilla_adm_usuario] FOREIGN KEY([esp_usuario_creacion])
REFERENCES [dbo].[adm_usuario] ([usu_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_plantilla] CHECK CONSTRAINT [FK_adm_emisor_sucursal_plantilla_adm_usuario]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_resolucion__adm_emisor_resolucion] FOREIGN KEY([esr_resolucion])
REFERENCES [dbo].[adm_emisor_resolucion] ([ere_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_resolucion__adm_emisor_resolucion]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_resolucion_adm_emisor] FOREIGN KEY([esr_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_resolucion_adm_emisor]
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_emisor_sucursal_resolucion_adm_emisor_sucursal] FOREIGN KEY([esr_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_emisor_sucursal_resolucion] NOCHECK CONSTRAINT [FK_adm_emisor_sucursal_resolucion_adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_grupo_emisor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_grupo_emisor_sys_municipio] FOREIGN KEY([gen_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[adm_grupo_emisor] NOCHECK CONSTRAINT [FK_adm_grupo_emisor_sys_municipio]
GO
ALTER TABLE [dbo].[adm_mandato]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_mandato__sys_tipo_identificacion] FOREIGN KEY([man_tipo_identificacion])
REFERENCES [dbo].[sys_tipo_identificacion] ([tid_id])
GO
ALTER TABLE [dbo].[adm_mandato] NOCHECK CONSTRAINT [FK_adm_mandato__sys_tipo_identificacion]
GO
ALTER TABLE [dbo].[adm_perfil_usuario_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_perfil_usuario_receptor_adm_perfil] FOREIGN KEY([pur_perfil])
REFERENCES [dbo].[adm_perfil] ([per_id])
GO
ALTER TABLE [dbo].[adm_perfil_usuario_receptor] NOCHECK CONSTRAINT [FK_adm_perfil_usuario_receptor_adm_perfil]
GO
ALTER TABLE [dbo].[adm_plantilla]  WITH CHECK ADD  CONSTRAINT [FK_adm_plantilla_sys_tipo_documento] FOREIGN KEY([prg_tipo_documento])
REFERENCES [dbo].[sys_tipo_documento] ([tdo_id])
GO
ALTER TABLE [dbo].[adm_plantilla] CHECK CONSTRAINT [FK_adm_plantilla_sys_tipo_documento]
GO
ALTER TABLE [dbo].[adm_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_proveedor_adm_emisor] FOREIGN KEY([pro_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_proveedor] NOCHECK CONSTRAINT [FK_adm_proveedor_adm_emisor]
GO
ALTER TABLE [dbo].[adm_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_proveedor_sys_departamento] FOREIGN KEY([pro_departamento])
REFERENCES [dbo].[sys_departamento] ([dep_id])
GO
ALTER TABLE [dbo].[adm_proveedor] NOCHECK CONSTRAINT [FK_adm_proveedor_sys_departamento]
GO
ALTER TABLE [dbo].[adm_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_proveedor_sys_municipio] FOREIGN KEY([pro_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[adm_proveedor] NOCHECK CONSTRAINT [FK_adm_proveedor_sys_municipio]
GO
ALTER TABLE [dbo].[adm_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_proveedor_sys_pais] FOREIGN KEY([pro_pais])
REFERENCES [dbo].[sys_pais] ([pai_id])
GO
ALTER TABLE [dbo].[adm_proveedor] NOCHECK CONSTRAINT [FK_adm_proveedor_sys_pais]
GO
ALTER TABLE [dbo].[adm_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_proveedor_sys_tipo_identificacion] FOREIGN KEY([pro_tipo_identificacion])
REFERENCES [dbo].[sys_tipo_identificacion] ([tid_id])
GO
ALTER TABLE [dbo].[adm_proveedor] NOCHECK CONSTRAINT [FK_adm_proveedor_sys_tipo_identificacion]
GO
ALTER TABLE [dbo].[adm_proveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_proveedor_sys_tipo_persona] FOREIGN KEY([pro_tipo_persona])
REFERENCES [dbo].[sys_tipo_persona] ([tpe_id])
GO
ALTER TABLE [dbo].[adm_proveedor] NOCHECK CONSTRAINT [FK_adm_proveedor_sys_tipo_persona]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_adm_emisor] FOREIGN KEY([rec_emisor])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_adm_emisor]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_adm_usuario_receptor] FOREIGN KEY([rec_usuario_receptor])
REFERENCES [dbo].[adm_usuario_receptor] ([ure_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_adm_usuario_receptor]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_sys_departamento] FOREIGN KEY([rec_departamento])
REFERENCES [dbo].[sys_departamento] ([dep_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_sys_departamento]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_sys_municipio] FOREIGN KEY([rec_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_sys_municipio]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_sys_pais] FOREIGN KEY([rec_pais])
REFERENCES [dbo].[sys_pais] ([pai_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_sys_pais]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_sys_tipo_identificacion] FOREIGN KEY([rec_tipo_identificacion])
REFERENCES [dbo].[sys_tipo_identificacion] ([tid_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_sys_tipo_identificacion]
GO
ALTER TABLE [dbo].[adm_receptor]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_receptor_sys_tipo_persona] FOREIGN KEY([rec_tipo_persona])
REFERENCES [dbo].[sys_tipo_persona] ([tpe_id])
GO
ALTER TABLE [dbo].[adm_receptor] NOCHECK CONSTRAINT [FK_adm_receptor_sys_tipo_persona]
GO
ALTER TABLE [dbo].[adm_tipo_retencion_fuente]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_tipo_retencion_fuente__sys_tipo_impuesto] FOREIGN KEY([trf_id_impuesto])
REFERENCES [dbo].[sys_tipo_impuesto] ([tim_id])
GO
ALTER TABLE [dbo].[adm_tipo_retencion_fuente] NOCHECK CONSTRAINT [FK_adm_tipo_retencion_fuente__sys_tipo_impuesto]
GO
ALTER TABLE [dbo].[adm_usuario]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_usuario_adm_emisor_sucursal] FOREIGN KEY([usu_emisor_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_usuario] NOCHECK CONSTRAINT [FK_adm_usuario_adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_usuario_perfil_tipo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_perfil_tipo_usuario_adm_perfil] FOREIGN KEY([upt_perfil])
REFERENCES [dbo].[adm_perfil] ([per_id])
GO
ALTER TABLE [dbo].[adm_usuario_perfil_tipo] NOCHECK CONSTRAINT [FK_adm_perfil_tipo_usuario_adm_perfil]
GO
ALTER TABLE [dbo].[adm_usuario_perfil_tipo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_perfil_tipo_usuario_adm_usuario] FOREIGN KEY([upt_usuario])
REFERENCES [dbo].[adm_usuario] ([usu_id])
GO
ALTER TABLE [dbo].[adm_usuario_perfil_tipo] NOCHECK CONSTRAINT [FK_adm_perfil_tipo_usuario_adm_usuario]
GO
ALTER TABLE [dbo].[adm_usuario_perfil_tipo]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_perfil_tipo_usuario_sys_tipo_usuario] FOREIGN KEY([upt_tipo_usuario])
REFERENCES [dbo].[sys_tipo_usuario] ([tus_id])
GO
ALTER TABLE [dbo].[adm_usuario_perfil_tipo] NOCHECK CONSTRAINT [FK_adm_perfil_tipo_usuario_sys_tipo_usuario]
GO
ALTER TABLE [dbo].[adm_usuario_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_usuario_sucursal_adm_emisor_sucursal] FOREIGN KEY([usu_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[adm_usuario_sucursal] NOCHECK CONSTRAINT [FK_adm_usuario_sucursal_adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[adm_usuario_sucursal]  WITH NOCHECK ADD  CONSTRAINT [FK_adm_usuario_sucursal_adm_usuario] FOREIGN KEY([usu_usuario])
REFERENCES [dbo].[adm_usuario] ([usu_id])
GO
ALTER TABLE [dbo].[adm_usuario_sucursal] NOCHECK CONSTRAINT [FK_adm_usuario_sucursal_adm_usuario]
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_departamento])
REFERENCES [dbo].[sys_departamento] ([dep_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_emisor_accion])
REFERENCES [dbo].[adm_emisor] ([emi_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_municipio])
REFERENCES [dbo].[sys_municipio] ([mun_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_pais])
REFERENCES [dbo].[sys_pais] ([pai_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_receptor])
REFERENCES [dbo].[adm_receptor] ([rec_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_tipo_accion])
REFERENCES [dbo].[sys_tipo_accion] ([tac_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_tipo_persona])
REFERENCES [dbo].[sys_tipo_persona] ([tpe_id])
GO
ALTER TABLE [dbo].[AuditLogReceptor]  WITH NOCHECK ADD FOREIGN KEY([alc_tipo_identificacion])
REFERENCES [dbo].[sys_tipo_identificacion] ([tid_id])
GO
ALTER TABLE [dbo].[log_receptor_nombre]  WITH NOCHECK ADD  CONSTRAINT [FK_log_receptor_nombre__adm_documento] FOREIGN KEY([rno_id_documento])
REFERENCES [dbo].[adm_documento] ([doc_id])
GO
ALTER TABLE [dbo].[log_receptor_nombre] NOCHECK CONSTRAINT [FK_log_receptor_nombre__adm_documento]
GO
ALTER TABLE [dbo].[log_receptor_nombre]  WITH NOCHECK ADD  CONSTRAINT [FK_log_receptor_nombre__adm_receptor] FOREIGN KEY([rno_id_receptor])
REFERENCES [dbo].[adm_receptor] ([rec_id])
GO
ALTER TABLE [dbo].[log_receptor_nombre] NOCHECK CONSTRAINT [FK_log_receptor_nombre__adm_receptor]
GO
ALTER TABLE [dbo].[sys_departamento]  WITH NOCHECK ADD FOREIGN KEY([dep_id_pais])
REFERENCES [dbo].[sys_pais] ([pai_id])
GO
ALTER TABLE [dbo].[sys_municipio]  WITH NOCHECK ADD  CONSTRAINT [FK_sys_municipio__sys_departamento] FOREIGN KEY([mun_id_dpto])
REFERENCES [dbo].[sys_departamento] ([dep_id])
GO
ALTER TABLE [dbo].[sys_municipio] NOCHECK CONSTRAINT [FK_sys_municipio__sys_departamento]
GO
ALTER TABLE [dbo].[sys_municipio]  WITH NOCHECK ADD  CONSTRAINT [FK_sys_munucipio__sys_pais] FOREIGN KEY([mun_id_pais])
REFERENCES [dbo].[sys_pais] ([pai_id])
GO
ALTER TABLE [dbo].[sys_municipio] NOCHECK CONSTRAINT [FK_sys_munucipio__sys_pais]
GO
ALTER TABLE [dbo].[sys_tipo_contingencia]  WITH NOCHECK ADD  CONSTRAINT [FK_sys_tipo_contingencia__sys_software] FOREIGN KEY([tco_id_software])
REFERENCES [dbo].[sys_software] ([sof_id])
GO
ALTER TABLE [dbo].[sys_tipo_contingencia] NOCHECK CONSTRAINT [FK_sys_tipo_contingencia__sys_software]
GO
ALTER TABLE [dbo].[sys_tipo_impuesto]  WITH NOCHECK ADD FOREIGN KEY([tim_clasificacion])
REFERENCES [dbo].[sys_tipo_clasificacion_impuesto] ([tci_id])
GO
ALTER TABLE [dbo].[tmp_documento]  WITH NOCHECK ADD  CONSTRAINT [FK_tmp_documento__adm_emisor_sucursal] FOREIGN KEY([tdo_emisor_sucursal])
REFERENCES [dbo].[adm_emisor_sucursal] ([esu_id])
GO
ALTER TABLE [dbo].[tmp_documento] NOCHECK CONSTRAINT [FK_tmp_documento__adm_emisor_sucursal]
GO
ALTER TABLE [dbo].[tmp_documento]  WITH NOCHECK ADD  CONSTRAINT [FK_tmp_documento__adm_usuario] FOREIGN KEY([tdo_usuario])
REFERENCES [dbo].[adm_usuario] ([usu_id])
GO
ALTER TABLE [dbo].[tmp_documento] NOCHECK CONSTRAINT [FK_tmp_documento__adm_usuario]
GO
ALTER TABLE [dbo].[tmp_documento]  WITH NOCHECK ADD  CONSTRAINT [FK_tmp_documento__sys_tipo_documento] FOREIGN KEY([tdo_tipo_documento])
REFERENCES [dbo].[sys_tipo_documento] ([tdo_id])
GO
ALTER TABLE [dbo].[tmp_documento] NOCHECK CONSTRAINT [FK_tmp_documento__sys_tipo_documento]
GO
/****** Object:  StoredProcedure [dbo].[sp_facturas_pendientes]    Script Date: 12/07/2024 5:46:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_facturas_pendientes]

AS
BEGIN
select esu_abreviatura ,emi_identificacion, 
ROW_NUMBER() OVER(ORDER BY esu_abreviatura ASC) emisor INTO #NITS
from adm_emisor_sucursal
inner join adm_emisor on esu_emisor=emi_id 
where esu_activo=1
order by 3


DECLARE @CONSECUTIVO1 INT =(SELECT MIN(EMISOR) FROM #NITS)
DECLARE @CONSECUTIVO2 INT =(SELECT MAX(EMISOR) FROM #NITS)
--SELECT @CONSECUTIVO1,@CONSECUTIVO2

--DROP TABLE #NITS

while @CONSECUTIVO1<@CONSECUTIVO2
begin
DECLARE @NIT VARCHAR(15)=(SELECT emi_identificacion FROM #NITS where emisor=@CONSECUTIVO1 )
declare @SUCURSAL VARCHAR(30)=(SELECT esu_abreviatura FROM #NITS where emisor=@CONSECUTIVO1 )
--select @NIT
--select @SUCURSAL

dECLARE @NumeroInicial int=(select min(b.doc_numero) from (select doc_prefijo,doc_numero,emi_nombre,esu_nombre
															from adm_documento
															inner join adm_emisor on doc_emisor=emi_id
															inner join adm_emisor_sucursal on doc_sucursal=adm_emisor_sucursal.esu_id WHERE emi_identificacion=@NIT and doc_prefijo=@SUCURSAL) b);
--select @NumeroInicial
DECLARE @NumeroFinal int=(select max(b.doc_numero) from (select doc_prefijo,doc_numero,emi_nombre,esu_nombre
															from adm_documento
															inner join adm_emisor on doc_emisor=emi_id
															inner join adm_emisor_sucursal on doc_sucursal=adm_emisor_sucursal.esu_id WHERE emi_identificacion=@NIT and doc_prefijo=@SUCURSAL) b);
dECLARE @Numero AS TABLE([Numbers] INT,prefijo VARCHAR(5) ,emisor int)
--select @NumeroFinal
WHILE @NumeroInicial < @NumeroFinal


BEGIN;
	IF NOT EXISTS(SELECT 1 FROM (select doc_prefijo,doc_numero,emi_nombre,esu_nombre
									from adm_documento
									inner join adm_emisor on doc_emisor=emi_id
									inner join adm_emisor_sucursal on doc_sucursal=adm_emisor_sucursal.esu_id WHERE emi_identificacion=@NIT and doc_prefijo=@SUCURSAL) b  WHERE @NumeroInicial = b.doc_numero)
	BEGIN
		INSERT INTO @Numero values (@NumeroInicial,@SUCURSAL,@nit)
	END
	SET @NumeroInicial = @NumeroInicial + 1
END
 SELECT * into #tabla FROM @Numero 
WHERE Numbers not in (SELECT b.doc_numero FROM (select doc_prefijo,doc_numero,emi_nombre,esu_nombre
											from adm_documento
											inner join adm_emisor on doc_emisor=emi_id
											inner join adm_emisor_sucursal on doc_sucursal=adm_emisor_sucursal.esu_id WHERE emi_identificacion=@NIT and doc_prefijo=@SUCURSAL) b);

with listado as(
select  prefijo,emisor,CONVERT(varchar(5),Numbers) factura
from #tabla)
select factura,prefijo,emisor,emi_nombre ,esu_nombre,max(ENO_FECHA) fecha_envio, eno_descripcion,CONVERT(varchar,GETDATE(),5) fecha into #reportes
from listado
inner join adm_emisor on emisor=emi_identificacion
inner join adm_emisor_sucursal on  esu_abreviatura=prefijo
INNER JOIN adm_emisor_notificacion ON  ENO_ID_EMISOR=esu_id
where eno_descripcion like ('%'+prefijo+factura) or eno_descripcion like ('%'+prefijo+'-'+factura)
GROUP BY factura,prefijo,emisor,emi_nombre,esu_nombre,ENO_DESCRIPCION

iNSERT INTO DELETE_CONSECUTIVOS_FALTANTES SELECT * from #reportes

select distinct * from DELETE_CONSECUTIVOS_FALTANTES where fecha_reporte=CONVERT(varchar,getdate(),5) and prefijo_fac=@SUCURSAL ORDER BY numero_fact
set @CONSECUTIVO1=@CONSECUTIVO1+1
drop table #reportes
drop table #tabla
--delete  DELETE_CONSECUTIVOS_FALTANTES

end
END
GO
