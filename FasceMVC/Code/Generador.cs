using DataBase;
using FasceMVC.Models;
using log4net;
using Metodos;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Web;
using ZXing;

namespace FasceMVC.Code
{
    public class Generador
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Generador).Name);

        private readonly ClsDocumento _clsDocumento = new ClsDocumento();

        public static ReportViewer FileDetail;
        public Generador()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-CO");
            FileDetail = new ReportViewer();
        }

        public byte[] GenerarPdf(Guid gIdDocumento)
        {
            try
            {
                adm_documento_file documento = _clsDocumento.ArchivoDocumento(gIdDocumento);
                var _modelo = JsonConvert.DeserializeObject<DocumentoModel>(documento.dfi_json_facse);

                //string reportPath = DirectorioRdlc(_modelo.Emisor.Identificacion, _modelo.Comprobante.TipoComprobante, _modelo.Emisor.Sucursal);
                //FileDetail.LocalReport.ReportPath = reportPath;

                FileDetail.LocalReport.LoadReportDefinition(GetReport(_modelo.Emisor.Identificacion, _modelo.Comprobante.TipoComprobante, _modelo.Emisor.Sucursal));
                FileDetail.LocalReport.EnableExternalImages = true;

                var qr = $"" +
                  $"NumFac: {_modelo.Comprobante.Prefijo}-{_modelo.Comprobante.Numero} {Environment.NewLine}" +
                  $"FecFac: {Convert.ToDateTime(_modelo.Comprobante.Fecha).ToString("yyyyMMddHHmmss")} {Environment.NewLine}" +
                  $"NitFac: {_modelo.Emisor.Identificacion} {Environment.NewLine}" +
                  $"DocAdq: {/*Numero de adquiriente*/_modelo.Receptor.Identificacion} {Environment.NewLine}" +
                  $"ValFac: {/*Valor de la factura con firmado #.##*/Convert.ToDecimal(_modelo.Totales.SubTotal).ToString("###0")} {Environment.NewLine}" +
                  $"ValIva: {/*Valor del IVA*/Convert.ToDecimal(_modelo.Totales.IVA).ToString("###0")} {Environment.NewLine}" +
                  $"ValOtroIm: {/*Valor otros impuestos*/(Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("###0")} {Environment.NewLine}" +
                  $"ValFacIm: {/*Valor Total de la Factura*/Convert.ToDecimal(_modelo.Totales.Total).ToString("###0")} {Environment.NewLine}" +
                  $"CUFE: {/*Cufe*/documento.adm_documento.doc_cufe}";


                ///$"{_doc_ref.serie}-{_doc_ref.folio}{Convert.ToDateTime(_doc_ref.fecha).ToString("yyyy/MM/dd HH:mm:ss")}{comprobante.Emisor.Identificacion}{/*Numero de adquiriente*/comprobante.Receptor.Identificacion}{/*Valor de la factura con firmado #.##*/Convert.ToDecimal(comprobante.Totales.SubTotal).ToString("0.00")}{/*Valor del IVA*/Convert.ToDecimal(comprobante.Totales.IVA).ToString("0.00")}{/*Valor otros impuestos*/(Convert.ToDecimal(0.00) + Convert.ToDecimal(0.00)).ToString("0.00")}{/*Valor Total de la Factura*/Convert.ToDecimal(comprobante.Totales.Total).ToString("0.00")}{/*Cufe*/comprobante.cufe}"

                byte[] imgdata = null;
                if (documento.adm_documento.sys_estado_documento.ted_codigo != "1")
                {
                    imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~\\Content\\Images\\sin_validacion.png"));
                }

                _modelo.QR = GenQR(qr);

                var emisorResolucion = new EmisorRecepcion
                {
                    Cufe = documento.adm_documento.doc_cufe,
                    NumeroResolucion = documento.adm_documento.adm_emisor_resolucion?.ere_numero_resolucion,
                    NumeroInicio = documento.adm_documento.adm_emisor_resolucion?.ere_numero_inicial,
                    NumeroFinal = documento.adm_documento.adm_emisor_resolucion?.ere_numero_final,
                    FechaFinal = documento.adm_documento.adm_emisor_resolucion.ere_fecha_final,
                    FechaInicial = documento.adm_documento.adm_emisor_resolucion.ere_fecha_inicio,
                    FirmaDigital = (documento.adm_documento.adm_emisor_certificado == null ? "" : documento.adm_documento.adm_emisor_certificado.ece_certificado),
                    ImagenFondo = imgdata
                };


                List<EmisorRecepcion> listadoEmisorRecepcion = new List<EmisorRecepcion>
                {
                    emisorResolucion
                };

                List<DocumentoModel> listadoDocumento = new List<DocumentoModel>
                {
                    _modelo
                };

                List<Comprobante> listadoComprobante = new List<Comprobante>
                {
                    _modelo.Comprobante
                };

                List<Emisor> listadoEmisor = new List<Emisor>
                {
                    _modelo.Emisor
                };

                List<Receptor> listadoReceptor = new List<Receptor>
                {
                    _modelo.Receptor
                };

                List<Totales> listadoTotales = new List<Totales>
                {
                    _modelo.Totales
                };

                List<Credenciales> listadoCredenciales = new List<Credenciales>
                {
                    _modelo.Credenciales
                };

                List<TerminosPago> listadoTerminosPago = new List<TerminosPago>
                {
                    _modelo.TerminosPago
                };

                List<DetallesComprobante> listadoDetalleComprobante = new List<DetallesComprobante>();

                if (_modelo.DetallesComprobante != null)
                {
                    listadoDetalleComprobante = _modelo.DetallesComprobante;
                }
                List<DescripcionDetalles> listadoDescripcionDetalles = new List<DescripcionDetalles>();

                if (_modelo.DescripcionDetalles != null)
                {
                    listadoDescripcionDetalles = _modelo.DescripcionDetalles;
                }

                List<Models.Descripcion> listadoDescripcion = new List<Models.Descripcion>
                {
                    _modelo.Descripcion
                };

                List<Detalles> listadoDetalles = new List<Detalles>();

                if (_modelo.Detalles != null)
                {
                    listadoDetalles = _modelo.Detalles;
                }

                List<TotalImpuestos> listadoTotalImpuesto = new List<TotalImpuestos>();

                if (_modelo.TotalImpuestos != null)
                {
                    listadoTotalImpuesto = _modelo.TotalImpuestos;
                }

                List<MetodoPago> listadoMetodoPago = new List<MetodoPago>();

                if (_modelo.MetodoPago != null)
                {
                    listadoMetodoPago = _modelo.MetodoPago;
                }


                //Detalles
                //TotalImpuestos
                //DetallesComprobante

                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("DocumentoModel", listadoDocumento));

                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Detalles", listadoDetalles));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Emisor", listadoEmisor));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Comprobante", listadoComprobante));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Receptor", listadoReceptor));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Totales", listadoTotales));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Credenciales", listadoCredenciales));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("TerminosPago", listadoTerminosPago));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("TotalImpuestos", listadoTotalImpuesto));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("DetallesComprobante", listadoDetalleComprobante));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("DescripcionDetalles", listadoDescripcionDetalles));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Descripcion", listadoDescripcion));
                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("MetodoPago", listadoMetodoPago));


                FileDetail.LocalReport.DataSources.Add(new ReportDataSource("EmisorResolucion", listadoEmisorRecepcion));

                FileDetail.ReportError += new ReportErrorEventHandler(ReportError);
                FileDetail.LocalReport.EnableHyperlinks = true;

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                FileDetail.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions);
                FileDetail.ReportError += new ReportErrorEventHandler(ReportError);

                return ExportPdf(FileDetail.LocalReport, 38, 29);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private byte[] ExportPdf(LocalReport report, int width, int heigth)
        {
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>" + 22 + "cm</PageWidth>" +
            "  <PageHeight>" + 37 + "cm</PageHeight>" +
            "  <MarginTop>0.5cm</MarginTop>" +
            "  <MarginLeft>0.5cm</MarginLeft>" +
            "  <MarginRight>0.5cm</MarginRight>" +
            "  <MarginBottom>0.5cm</MarginBottom>" +
            "</DeviceInfo>";




            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            log.Debug("Renderizando reporte");
            try
            {
                renderedBytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                log.Debug("Finalizando render reporte");
                if (warnings != null)
                    for (int i = 0; i < warnings.Count(); ++i)
                    {
                        log.Debug("Warning " + warnings[i].Code + " " + warnings[i].Message + " " +
                                    warnings[i].ObjectName + " " + warnings[i].ObjectType);
                    }
            }
            catch (Exception e)
            {
                log.Error("Error renderizando reporte: ", e);
                throw e;

            }
            return renderedBytes;
        }

        private byte[] ExportExcel(LocalReport report)
        {
            string reportType = "EXCELOPENXML";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +

            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            log.Debug("Renderizando reporte");
            renderedBytes = report.Render(
               reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            log.Debug("Finalizando render reporte");

            return renderedBytes;
        }
        void ReportError(object sender, ReportErrorEventArgs e)
        {
            log.Debug("Error al generar el reporte " + e.ToString());
        }
        public Stream GetReport(string sIdentificacion, string sTipo, string sSucursal)
        {
            Assembly loadedAssembly = Assembly.GetAssembly(typeof(Generador));

            String resourceName = null;

            resourceName = $"{loadedAssembly.GetName(true).Name}.Content.Report.{sTipo}.rdlc";

            if (loadedAssembly.GetManifestResourceStream($@"{loadedAssembly.GetName(true).Name}.Content.Report._{sIdentificacion}.{sIdentificacion}_{sTipo}_{sSucursal}.rdlc") != null)
            {
                resourceName = $@"{loadedAssembly.GetName(true).Name}.Content.Report._{sIdentificacion}.{sIdentificacion}_{sTipo}_{sSucursal}.rdlc";
            }
            else if (loadedAssembly.GetManifestResourceStream($"{loadedAssembly.GetName(true).Name}.Content.Report._{sIdentificacion}.{sIdentificacion}_{sTipo}.rdlc") != null)
            {
                resourceName = $@"{loadedAssembly.GetName(true).Name}.Content.Report._{sIdentificacion}.{sIdentificacion}_{sTipo}.rdlc";
            }
            //resourceName = resourceName.Replace(' ', '_');
            Stream stream = loadedAssembly.GetManifestResourceStream(resourceName);
            return stream;
        }
        public Stream GetReportGeneral(String reportName)
        {
            Assembly loadedAssembly = Assembly.GetAssembly(typeof(Generador));

            String resourceName = $"{loadedAssembly.GetName(true).Name}.Content.ReportGenerales.{reportName}.rdlc";
            //resourceName = resourceName.Replace(' ', '_');
            Stream stream = loadedAssembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                string[] resourceNames = loadedAssembly.GetManifestResourceNames();
                throw new FileNotFoundException("el archivo " + resourceName + " no se encontró. " +
                                                 " Los posibles archivos son: " + String.Join("\r\n\t", resourceNames));
            }
            return stream;
        }
        public byte[] GenQR(string valor)
        {

            IBarcodeWriter qr = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions()
                {
                    Margin = 0 // el margen que tendra el codigo con el restro de la imagen
                }
            };
            var result = qr.Write(valor.Replace(" ", ""));
            return ImageToByte(result);
        }
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public byte[] GetInicioReport(DateTime dFechaInicial, DateTime dFechaFinal, string sIdentificacion, string sNombre, Guid? gIdComprobante, int? iNumero, Guid gsucursal, Guid? gEstado, Guid? gEstadoFacse)
        {
            ClsDocumento _clsDocumento = new ClsDocumento();

            List<EmisorInicio> Results = _clsDocumento.ListadoDocumentoExcel(dFechaInicial, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, gsucursal, gEstado, gEstadoFacse);

            var ResultsDocument = Results.Select(d => new adm_documento
            {
                CodigoTipoDocumento = d.tdo_abreviatura,
                doc_prefijo = d.doc_prefijo,
                doc_numero = d.doc_numero,
                Identificacion = d.rec_identificacion,
                Nombre = d.rec_nombre,
                doc_fecha_recepcion = d.doc_fecha_recepcion,
                doc_fecha_envio = d.doc_fecha_envio,
                doc_valor_total = d.doc_valor_total,
                doc_usuario = d.doc_usuario
            }).OrderByDescending(d => d.doc_fecha_recepcion).ToList();

            FileDetail.LocalReport.LoadReportDefinition(GetReportGeneral("Inicio"));

            FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Inicio", ResultsDocument));

            FileDetail.ReportError += new ReportErrorEventHandler(ReportError);
            FileDetail.LocalReport.EnableHyperlinks = true;

            PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
            permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            FileDetail.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions);

            return ExportExcel(FileDetail.LocalReport);
        }

        public byte[] GetInicioReportProveedor(DateTime dFechaInicial, DateTime dFechaFinal, string sIdentificacion, string sNombre, Guid? gIdComprobante, int? iNumero, Guid gsucursal, Guid? gEstadoFacse)
        {
            ClsDocumentoProveedor _clsDocumentoProveedor = new ClsDocumentoProveedor();

            List<EmisorInicioProveedor> Results = _clsDocumentoProveedor.ListadoDocumentoExcel(dFechaInicial, dFechaFinal, sIdentificacion, sNombre, gIdComprobante, iNumero, gsucursal, gEstadoFacse);

            var ResultsDocument = Results.Select(d => new adm_documento_proveedor
            {
                CodigoTipoDocumento = d.tdo_abreviatura,
                dpr_prefijo = d.dpr_prefijo,
                dpr_numero = d.dpr_numero,
                Identificacion = d.pro_identificacion,
                Nombre = d.pro_nombre,
                dpr_fecha_recepcion = d.dpr_fecha_recepcion,
                dpr_fecha_envio = d.dpr_fecha_envio,
                dpr_valor_total = d.dpr_valor_total,
                dpr_usuario = d.dpr_usuario,
                dpr_fecha_recibido = d.dpr_fecha_recibido
            }).OrderByDescending(d => d.dpr_fecha_recepcion).ToList();

            FileDetail.LocalReport.LoadReportDefinition(GetReportGeneral("Proveedor"));

            FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Inicio", ResultsDocument));

            FileDetail.ReportError += new ReportErrorEventHandler(ReportError);
            FileDetail.LocalReport.EnableHyperlinks = true;

            PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
            permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            FileDetail.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions);

            return ExportExcel(FileDetail.LocalReport);
        }

        public byte[] GetInicioReportReceptor(adm_documento adm_Documento, Guid gReceptor)
        {
            ClsDocumento _clsDocumento = new ClsDocumento();

            List<InicioReport> Results = _clsDocumento.ListadoDocumentoExcelReceptor(adm_Documento, gReceptor);

            FileDetail.LocalReport.LoadReportDefinition(GetReportGeneral("Inicio"));

            FileDetail.LocalReport.DataSources.Add(new ReportDataSource("Inicio", Results));

            FileDetail.ReportError += new ReportErrorEventHandler(ReportError);
            FileDetail.LocalReport.EnableHyperlinks = true;

            PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
            permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            FileDetail.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions);

            return ExportExcel(FileDetail.LocalReport);
        }
    }
}