Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Configuration
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
'PARA EXPORTAR A PDF
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports
'FIN EXPORTAR A PDF
Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN
Imports System.Data.SqlClient

Imports System.Drawing.Printing 'TMACASSI 23/01/2017

Imports System.IO 'TMACASSI 04/11/2016

Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Net

Public Class Reporte
    Inherits System.Web.UI.Page
    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
    Dim crystalreport As New ReportDocument()
    Dim xRuta As String = sRutaTemp
    Dim xNombreArchivo As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session(sIdeHistoria)) Then
            Response.Redirect("ConsultaPacienteHospitalizado.aspx")
        End If
        If Not Page.IsPostBack Then
            VisorReporte.HasExportButton = True
            VisorReporte.AllowedExportFormats = True
            VisorReporte.HasPrintButton = True
            'VisorReporte.PrintMode = PrintMode.Pdf

            VisorReporte.ParameterFieldInfo.Clear()

            Reporte()
        End If
    End Sub

    Protected Sub VisorReporte_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles VisorReporte.Navigate
        Reporte()
    End Sub

    Public Sub Reporte()
        Dim xParamOP As String = Request.Params("OP")
        xParamOP = xParamOP.Replace(";", "")
        Dim xParamEX As Integer = Request.Params("EX")
        xParamEX = xParamEX
        'Request.Params("OP") = Request.Params("OP").Replace(";", "")
        If xParamOP = "DA" Then
            rptDeclaratoriaAlergia(0, xParamEX, "DA_" + Session(sIdeHistoria).ToString())
            RegistrarLog("Se imprimio reporte: Declaratoria de alergia")
        ElseIf xParamOP = "ME" Then
            rptMedicamentosa(0, 0, "")
            RegistrarLog("Se imprimio reporte: Reconciliacion medicamentosa")
        ElseIf xParamOP = "PE1" Then
            rptIndicacionMedica(0, 0, "")
            RegistrarLog("Se imprimio reporte: Indicaciones medicas")
        ElseIf xParamOP = "PE2" Then
            rptIndicacionMedica2(0, 0, "")
            RegistrarLog("Se imprimio reporte: Indicaciones medicas")
        ElseIf xParamOP = "RA1" Then
            rptRecetaAlta(0, 0, "")
            RegistrarLog("Se imprimio reporte: Receta de alta")
        ElseIf xParamOP = "RA2" Then
            rptRecetaAlta2(0, 0, "")
            RegistrarLog("Se imprimio reporte: Receta de alta")
        ElseIf xParamOP = "EC1" Then
            rptEvolucionClinica(0, 0, "")
            RegistrarLog("Se imprimio reporte: Evolucion Clinica")
        ElseIf xParamOP = "EC2" Then
            rptEvolucionClinica2(0, 0, "")
            RegistrarLog("Se imprimio reporte: Evolucion Clinica")
        ElseIf xParamOP = "IN1" Then
            rptInterconsulta(0, 0, "")
            RegistrarLog("Se imprimio reporte: Interconsulta")
        ElseIf xParamOP = "IN2" Then
            rptInterconsulta2(0, 0, "")
            RegistrarLog("Se imprimio reporte: Interconsulta")
        ElseIf xParamOP = "CI" Then
            rptHojaConsentimiento(0, 0, "")
            RegistrarLog("Se imprimio reporte: Consentimiento Informado")
        ElseIf xParamOP = "IM1" Then
            rptInformeMedico(0, 0, "")
            RegistrarLog("Se imprimio reporte: Informe Medico (Epicrisis)")
        Else
        End If

    End Sub

    Public Sub RegistrarLog(ByVal Mensaje As String)
        oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
        oRceInicioSesionE.CodUser = Session(sCodUser)
        oRceInicioSesionE.Formulario = "InformacionPaciente"
        oRceInicioSesionE.Control = "REPORTES"
        oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
        'oRceInicioSesionE.DscPcName = Session(sDscPcName) 22/02/2017
        If Not IsNothing(Session(sDscPcName)) Then
            oRceInicioSesionE.DscPcName = Session(sDscPcName)
        Else
            Dim nom_cliente, ip_cliente As String
            ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If ip_cliente = Nothing Then
                ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            End If
            Dim sIP_ENTRY As IPHostEntry = Dns.GetHostEntry(ip_cliente)
            nom_cliente = Convert.ToString(sIP_ENTRY.HostName)
            oRceInicioSesionE.DscPcName = nom_cliente
        End If
        oRceInicioSesionE.DscLog = Mensaje
        oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        'Dim instance As New PrinterSettings
        'Dim impresorpred As String = instance.PrinterName
        'For Each Impresora As String In PrinterSettings.InstalledPrinters.
        '    MsgBox(Impresora)
        'Next

        'INICIO - JB - 22/07/2019 - COMENTADO
        'If Request.Params("OP") = "DA" Then rptDeclaratoriaAlergia(1, 0, "")
        'If Request.Params("OP") = "ME" Then rptMedicamentosa(1, 0, "")
        'If Request.Params("OP") = "PE1" Then rptIndicacionMedica(1, 0, "")
        'If Request.Params("OP") = "PE2" Then rptIndicacionMedica2(1, 0, "")
        'If Request.Params("OP") = "RA1" Then rptRecetaAlta(1, 0, "")
        'If Request.Params("OP") = "RA2" Then rptRecetaAlta2(1, 0, "")
        'If Request.Params("OP") = "EC1" Then rptEvolucionClinica(1, 0, "")
        'If Request.Params("OP") = "EC2" Then rptEvolucionClinica2(1, 0, "")
        'If Request.Params("OP") = "IN1" Then rptInterconsulta(1, 0, "")
        'If Request.Params("OP") = "IN2" Then rptInterconsulta2(1, 0, "")
        'If Request.Params("OP") = "CI" Then rptHojaConsentimiento(1, 0, "")
        'If Request.Params("OP") = "IM1" Then rptInformeMedico(1, 0, "")
        'FIN - JB - 22/07/2019 - COMENTADO

        If Request.Params("OP") = "DA" Then rptDeclaratoriaAlergia(0, 0, "")
        If Request.Params("OP") = "ME" Then rptMedicamentosa(0, 0, "")
        If Request.Params("OP") = "PE1" Then rptIndicacionMedica(0, 0, "")
        If Request.Params("OP") = "PE2" Then rptIndicacionMedica2(0, 0, "")
        If Request.Params("OP") = "RA1" Then rptRecetaAlta(0, 0, "")
        If Request.Params("OP") = "RA2" Then rptRecetaAlta2(0, 0, "")
        If Request.Params("OP") = "EC1" Then rptEvolucionClinica(0, 0, "")
        If Request.Params("OP") = "EC2" Then rptEvolucionClinica2(0, 0, "")
        If Request.Params("OP") = "IN1" Then rptInterconsulta(0, 0, "")
        If Request.Params("OP") = "IN2" Then rptInterconsulta2(0, 0, "")
        If Request.Params("OP") = "CI" Then rptHojaConsentimiento(0, 0, "")
        If Request.Params("OP") = "IM1" Then rptInformeMedico(0, 0, "")
    End Sub
    '**
    Public Sub rptDeclaratoriaAlergia(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceAlergiaTableAdapter()
        Dim tabla As New DataTable("Rp_RceAlergia")
        tabla.Columns.Add("ide_alergia")
        tabla.Columns.Add("ide_historia")
        tabla.Columns.Add("flg_presentalergia")
        tabla.Columns.Add("flg_representante")
        tabla.Columns.Add("ide_numdocumento")
        tabla.Columns.Add("txt_representante")
        tabla.Columns.Add("txt_medicamentos")
        tabla.Columns.Add("txt_alimentos")
        tabla.Columns.Add("txt_otros")
        tabla.Columns.Add("fec_registra")
        tabla.Columns.Add("usr_registra")
        tabla.Columns.Add("fec_modifica")
        tabla.Columns.Add("usr_modifica")
        tabla.Columns.Add("dsc_medicamentos")
        tabla.Columns.Add("paciente")
        tabla.Columns.Add("cama")
        'tabla.Columns.Add("flg_firma")
        Dim columna_firma As DataColumn = New DataColumn("flg_firma")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Item(index).ItemArray)
        Next

        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpAlergia.rpt"))
        crystalreport.SetDataSource(tabla)
        'crystalreport.SetDatabaseLogon("sa", "sa", ".", "carRent")
        'crystalreport.DataSourceConnections(CnnBD)



        ''INICIO - JB - 30/01/2017
        'oHospitalE.IdeHistoria = Session(sIdeHistoria)
        'oHospitalE.TipoDoc = 9
        'Dim dt As New DataTable()
        'dt.Columns.Add("id_documento")
        'dt.Columns.Add("codatencion")
        'dt.Columns.Add("tipo_doc")
        'dt.Columns.Add("usuario_creacion")
        'dt.Columns.Add("fecha_creacion")
        'dt.Columns.Add("estado")
        'Dim columna1 As DataColumn = New DataColumn("bib_documento")
        'columna1.DataType = System.Type.GetType("System.Byte[]")
        'dt.Columns.Add(columna1)
        'dt.Columns.Add("extension_doc")
        'dt.Columns.Add("descripcion_doc")
        'dt.Columns.Add("usuario_eliminacion")
        'dt.Columns.Add("fecha_eliminacion")
        'dt.Columns.Add("flg_firma")
        'dt.Columns.Add("fec_firma")
        'dt.Columns.Add("usr_firma")
        'dt.Columns.Add("blb_documentofirma")
        'dt.Columns.Add("dsc_extensionfirma")
        'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

        'Dim docu As Byte()
        'Response.Clear()
        ''Response.Buffer = True
        ''Response.Charset = ""
        ''Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/pdf"
        ''la variable nombre es el nombre del archivo .pdf
        'Response.AddHeader("Content-disposition", "attachment; filename=" & dt.Rows(0)("descripcion_doc").ToString() + ".pdf")
        'docu = DirectCast(dt.Rows(0)("bib_documento"), Byte())
        'Response.BinaryWrite(docu)
        'Response.Flush()
        'Response.End()
        ''FIN - JB - 30/01/2017



        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
        'EXPORTAR 
        If xExport = 1 Then
            Try
                If Directory.Exists(xRuta) Then

                Else
                    Directory.CreateDirectory(xRuta)
                End If

                Dim OpcionExportar As ExportOptions
                Dim OpcionDestino As New DiskFileDestinationOptions()
                Dim OpcionesFormato As New PdfRtfWordFormatOptions()
                'xNombreArchivo = "DA_" + Session(sIdeHistoria).ToString() + ".pdf"
                xNombreArchivo = xNombreArchivo + ".pdf"
                OpcionDestino.DiskFileName = xRuta + "\" + xNombreArchivo
                OpcionExportar = crystalreport.ExportOptions
                With OpcionExportar
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                    .DestinationOptions = OpcionDestino
                    .FormatOptions = OpcionesFormato
                End With
                crystalreport.Export()

                'GUARDANDO REPORTE 31/20/2016
                'INICIO - JB - 27/01/2017 - CODIGO COMENTADO (excepto conexion cn)
                Dim pdf_byte As Byte() = System.IO.File.ReadAllBytes(xRuta + "\" + xNombreArchivo)
                System.IO.File.WriteAllBytes(xRuta + "\" + xNombreArchivo, pdf_byte)
                Dim cn As New SqlConnection(CnnBD)
                'Dim cmd As New SqlCommand("update rce_alergia set blob_firma = @DeclaratoriaAlergiaPDF, flg_firma=NULL where ide_historia = @historia", cn)
                'cmd.CommandType = CommandType.Text
                'cmd.Parameters.AddWithValue("@DeclaratoriaAlergiaPDF", pdf_byte)
                'cmd.Parameters.AddWithValue("@historia", Session(sIdeHistoria))
                'Dim num As Integer
                'cn.Open()
                'num = cmd.ExecuteNonQuery()
                'cn.Close()


                oHospitalE.CodAtencion = Session(sCodigoAtencion)
                'oHospitalE.IdHistoria = Session(sIdeHistoria)
                oHospitalE.CodUser = Session(sCodUser)
                oHospitalE.TipoDoc = 9
                oHospitalE.Descripcion = Session(sIdeHistoria)
                oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

                'Paso 2
                Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento, extension_doc='PDF' ,flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
                cmd1.CommandType = CommandType.Text
                cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
                cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
                Dim num1 As Integer
                cn.Open()
                num1 = cmd1.ExecuteNonQuery()
                cn.Close()

                'Paso 3
                oHospitalE.IdeHistoria = Session(sIdeHistoria)
                oHospitalE.IdeGeneral = Session(sIdeHistoria)
                oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
                'FIN - JB - 27/01/2017

                'FIN GUARDANDO REPORTE

                'OPCION 1
                Response.Clear()
                'Response.Buffer = True
                'Response.Charset = ""
                'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                'aqui va la ruta del archivo
                Response.ContentType = "application/pdf"
                'la variable nombre es el nombre del archivo .pdf
                Response.AddHeader("Content-disposition", "attachment; filename=" & xNombreArchivo)
                Response.WriteFile(xRuta + "\" + xNombreArchivo)
                Response.Flush()
                crystalreport.Close()
                crystalreport.Dispose()
                System.IO.File.Delete(xRuta + "\" + xNombreArchivo)


                Response.End() 'End() Close()                
            Catch ex As Exception
                'MsgBox(ex.ToString)
            End Try
        End If
    End Sub
    '**
    Public Sub rptMedicamentosa(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceMedicamentosaTableAdapter()
        Dim tabla As New DataTable("Rp_RceMedicamentosa")
        tabla.Columns.Add("ide_medicamentosa_det")
        tabla.Columns.Add("ide_medicamentosa_cab")
        tabla.Columns.Add("cod_producto")
        tabla.Columns.Add("dsc_producto")
        tabla.Columns.Add("num_dosis")
        tabla.Columns.Add("dsc_via")
        tabla.Columns.Add("num_frecuencia")
        tabla.Columns.Add("cod_accion")
        tabla.Columns.Add("ide_examenfisicores")
        tabla.Columns.Add("fec_ultima")
        tabla.Columns.Add("hor_ultima")
        tabla.Columns.Add("flg_medicamento")
        tabla.Columns.Add("flg_activo")
        tabla.Columns.Add("fec_registra")
        tabla.Columns.Add("usr_registra")
        tabla.Columns.Add("fec_modifica")
        tabla.Columns.Add("usr_modifica")
        tabla.Columns.Add("dsc_paciente")
        tabla.Columns.Add("servicio")
        tabla.Columns.Add("dsc_servicio")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("fecha")
        'tabla.Columns.Add("firma_tratante")
        Dim columna_firma As DataColumn = New DataColumn("firma_tratante")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        'tabla.Columns.Add("firma_hospitalario")
        Dim columna_firma1 As DataColumn = New DataColumn("firma_hospitalario")
        columna_firma1.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma1)
        'tabla.Columns.Add("nmedico")
        'tabla.Columns.Add("RNE")
        'tabla.Columns.Add("CMP")

        ''INICIO - JB - 30/01/2017
        'oHospitalE.IdeHistoria = Session(sIdeHistoria)
        'oHospitalE.TipoDoc = 11
        'Dim dt As New DataTable()
        'dt.Columns.Add("id_documento")
        'dt.Columns.Add("codatencion")
        'dt.Columns.Add("tipo_doc")
        'dt.Columns.Add("usuario_creacion")
        'dt.Columns.Add("fecha_creacion")
        'dt.Columns.Add("estado")
        'Dim columna1 As DataColumn = New DataColumn("bib_documento")
        'columna1.DataType = System.Type.GetType("System.Byte[]")
        'dt.Columns.Add(columna1)
        'dt.Columns.Add("extension_doc")
        'dt.Columns.Add("descripcion_doc")
        'dt.Columns.Add("usuario_eliminacion")
        'dt.Columns.Add("fecha_eliminacion")
        'dt.Columns.Add("flg_firma")
        'dt.Columns.Add("fec_firma")
        'dt.Columns.Add("usr_firma")
        'dt.Columns.Add("blb_documentofirma")
        'dt.Columns.Add("dsc_extensionfirma")
        'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

        'Dim docu As Byte()
        'Response.Clear()
        ''Response.Buffer = True
        ''Response.Charset = ""
        ''Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/pdf"
        ''la variable nombre es el nombre del archivo .pdf
        'Response.AddHeader("Content-disposition", "attachment; filename=" & dt.Rows(0)("descripcion_doc").ToString() + ".pdf")
        'docu = DirectCast(dt.Rows(0)("bib_documento"), Byte())
        'Response.BinaryWrite(docu)
        'Response.Flush()
        'Response.End()
        ''FIN - JB - 30/01/2017


        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Item(index).ItemArray)
        Next
        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpMedicamentosa.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub
    '*****
    Public Sub rptIndicacionMedica(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaMedicamentoTableAdapter()
        Dim tabla As New DataTable("Rp_RceRecetaMedicamento")
        tabla.Columns.Add("ide_medicamentorec")
        tabla.Columns.Add("ide_receta")
        tabla.Columns.Add("dsc_paciente")
        tabla.Columns.Add("cod_medico")
        tabla.Columns.Add("dsc_medico")
        tabla.Columns.Add("edad")
        tabla.Columns.Add("dias")
        tabla.Columns.Add("fec_registra")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("dsc_diagnostico")
        tabla.Columns.Add("talla")
        tabla.Columns.Add("peso")
        tabla.Columns.Add("opt_ram")
        tabla.Columns.Add("dsc_ram")
        tabla.Columns.Add("telefono")
        tabla.Columns.Add("dias_post")
        tabla.Columns.Add("indicaciones")
        tabla.Columns.Add("dsc_medicamento")
        tabla.Columns.Add("via")
        tabla.Columns.Add("hora")
        tabla.Columns.Add("hora_fecha1")
        tabla.Columns.Add("hora_fecha2")
        tabla.Columns.Add("hora_fecha3")
        tabla.Columns.Add("hora_fecha4")
        tabla.Columns.Add("hora_fecha5")
        tabla.Columns.Add("hora_fecha6")
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, "", 3).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, "", 3).Rows.Item(index).ItemArray)
        Next
        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpIndicacionMedica.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub
    '*****
    Public Sub rptIndicacionMedica2(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaMedicamentoTableAdapter()
        Dim tabla As New DataTable("Rp_RceRecetaMedicamento")
        tabla.Columns.Add("ide_medicamentorec")
        tabla.Columns.Add("ide_receta")
        tabla.Columns.Add("dsc_paciente")
        tabla.Columns.Add("cod_medico")
        tabla.Columns.Add("dsc_medico")
        tabla.Columns.Add("edad")
        tabla.Columns.Add("dias")
        tabla.Columns.Add("fec_registra")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("dsc_diagnostico")
        tabla.Columns.Add("talla")
        tabla.Columns.Add("peso")
        tabla.Columns.Add("opt_ram")
        tabla.Columns.Add("dsc_ram")
        tabla.Columns.Add("telefono")
        tabla.Columns.Add("dias_post")
        tabla.Columns.Add("indicaciones")
        tabla.Columns.Add("dsc_medicamento")
        tabla.Columns.Add("via")
        tabla.Columns.Add("hora")
        tabla.Columns.Add("hora_fecha1")
        tabla.Columns.Add("hora_fecha2")
        tabla.Columns.Add("hora_fecha3")
        tabla.Columns.Add("hora_fecha4")
        tabla.Columns.Add("hora_fecha5")
        tabla.Columns.Add("hora_fecha6")
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        If Not IsNothing(Request.Params("IM")) Then
            For index = 0 To Reporte1.GetData(Session(sIdeHistoria), Request.Params("IM"), "", 1).Rows.Count - 1
                tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), Request.Params("IM"), "", 1).Rows.Item(index).ItemArray)
            Next
        End If
        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpIndicacionMedica.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub

    Public Sub rptRecetaAlta(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaAltaTableAdapter()
        Dim tabla As New DataTable("Rp_RceRecetaAlta")
        tabla.Columns.Add("ide_medicamentorec")
        tabla.Columns.Add("ide_receta")
        tabla.Columns.Add("dsc_paciente")
        tabla.Columns.Add("flg_asegurado")
        tabla.Columns.Add("flg_alergias")
        tabla.Columns.Add("dsc_alergias")
        tabla.Columns.Add("dsc_medico")
        tabla.Columns.Add("telefono")
        tabla.Columns.Add("fec_receta")
        tabla.Columns.Add("fec_cita")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("Rp")
        tabla.Columns.Add("dsc_diagnostico")
        tabla.Columns.Add("dsc_indicaciones")
        tabla.Columns.Add("dsc_medicamento")
        tabla.Columns.Add("dsc_dci")
        tabla.Columns.Add("dsc_presentacion")
        tabla.Columns.Add("cantidad")
        tabla.Columns.Add("tiempo")
        'tabla.Columns.Add("firma_medico")
        Dim columna_firma As DataColumn = New DataColumn("firma_medico")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
        Next

        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpRecetaAlta.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If

    End Sub

    Public Sub rptRecetaAlta2(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaAltaTableAdapter()
        Dim tabla As New DataTable("Rp_RceRecetaAlta")
        tabla.Columns.Add("ide_medicamentorec")
        tabla.Columns.Add("ide_receta")
        tabla.Columns.Add("dsc_paciente")
        tabla.Columns.Add("flg_asegurado")
        tabla.Columns.Add("flg_alergias")
        tabla.Columns.Add("dsc_alergias")
        tabla.Columns.Add("dsc_medico")
        tabla.Columns.Add("telefono")
        tabla.Columns.Add("fec_receta")
        tabla.Columns.Add("fec_cita")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("Rp")
        tabla.Columns.Add("dsc_diagnostico")
        tabla.Columns.Add("dsc_indicaciones")
        tabla.Columns.Add("dsc_medicamento")
        tabla.Columns.Add("dsc_dci")
        tabla.Columns.Add("dsc_presentacion")
        tabla.Columns.Add("cantidad")
        tabla.Columns.Add("tiempo")
        'tabla.Columns.Add("firma_medico")
        Dim columna_firma As DataColumn = New DataColumn("firma_medico")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        If Not IsNothing(Request.Params("IR")) Then
            For index = 0 To Reporte1.GetData(Session(sIdeHistoria), Request.Params("IR"), 2).Rows.Count - 1
                tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), Request.Params("IR"), 2).Rows.Item(index).ItemArray)
            Next
        End If
        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpRecetaAlta.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub

    Public Sub rptEvolucionClinica(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
        Dim tabla As New DataTable("Rp_RceEvolucion")
        tabla.Columns.Add("ide_historia")
        tabla.Columns.Add("ide_evolucion")
        tabla.Columns.Add("medico")
        tabla.Columns.Add("flg_educacion")
        tabla.Columns.Add("flg_informe")
        tabla.Columns.Add("TipoEvolucion")
        tabla.Columns.Add("txt_detalle")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("cuarto")
        tabla.Columns.Add("dni")
        tabla.Columns.Add("fec_nacimiento")
        tabla.Columns.Add("fec_ingreso")
        tabla.Columns.Add("ape_paterno")
        tabla.Columns.Add("ape_materno")
        tabla.Columns.Add("nombres")
        tabla.Columns.Add("fec_registro")
        tabla.Columns.Add("hora_registro")
        'tabla.Columns.Add("firma_medico")
        Dim columna_firma As DataColumn = New DataColumn("firma_medico")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")


        ''INICIO - JB - 30/01/2017
        'oHospitalE.IdeHistoria = Session(sIdeHistoria)
        'oHospitalE.TipoDoc = 10
        'Dim dt As New DataTable()
        'dt.Columns.Add("id_documento")
        'dt.Columns.Add("codatencion")
        'dt.Columns.Add("tipo_doc")
        'dt.Columns.Add("usuario_creacion")
        'dt.Columns.Add("fecha_creacion")
        'dt.Columns.Add("estado")
        'Dim columna1 As DataColumn = New DataColumn("bib_documento")
        'columna1.DataType = System.Type.GetType("System.Byte[]")
        'dt.Columns.Add(columna1)
        'dt.Columns.Add("extension_doc")
        'dt.Columns.Add("descripcion_doc")
        'dt.Columns.Add("usuario_eliminacion")
        'dt.Columns.Add("fecha_eliminacion")
        'dt.Columns.Add("flg_firma")
        'dt.Columns.Add("fec_firma")
        'dt.Columns.Add("usr_firma")
        'dt.Columns.Add("blb_documentofirma")
        'dt.Columns.Add("dsc_extensionfirma")
        'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

        'Dim docu As Byte()
        'Response.Clear()
        ''Response.Buffer = True
        ''Response.Charset = ""
        ''Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/pdf"
        ''la variable nombre es el nombre del archivo .pdf
        'Response.AddHeader("Content-disposition", "attachment; filename=" & dt.Rows(0)("descripcion_doc").ToString() + ".pdf")
        'docu = DirectCast(dt.Rows(0)("bib_documento"), Byte())
        'Response.BinaryWrite(docu)
        'Response.Flush()
        'Response.End()
        ''FIN - JB - 30/01/2017


        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
        Next

        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinica.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub

    Public Sub rptEvolucionClinica2(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
        Dim tabla As New DataTable("Rp_RceEvolucion")
        tabla.Columns.Add("ide_evolucion")
        tabla.Columns.Add("medico")
        tabla.Columns.Add("flg_educacion")
        tabla.Columns.Add("flg_informe")
        tabla.Columns.Add("TipoEvolucion")
        tabla.Columns.Add("txt_detalle")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("cuarto")
        tabla.Columns.Add("dni")
        tabla.Columns.Add("fec_nacimiento")
        tabla.Columns.Add("fec_ingreso")
        tabla.Columns.Add("ape_paterno")
        tabla.Columns.Add("ape_materno")
        tabla.Columns.Add("nombres")
        tabla.Columns.Add("fec_registro")
        tabla.Columns.Add("hora_registro")
        'tabla.Columns.Add("firma_medico")
        Dim columna_firma As DataColumn = New DataColumn("firma_medico")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        ''INICIO - JB - 30/01/2017
        'oHospitalE.IdeHistoria = Session(sIdeHistoria)
        'oHospitalE.TipoDoc = 10
        'Dim dt As New DataTable()
        'dt.Columns.Add("id_documento")
        'dt.Columns.Add("codatencion")
        'dt.Columns.Add("tipo_doc")
        'dt.Columns.Add("usuario_creacion")
        'dt.Columns.Add("fecha_creacion")
        'dt.Columns.Add("estado")
        'Dim columna1 As DataColumn = New DataColumn("bib_documento")
        'columna1.DataType = System.Type.GetType("System.Byte[]")
        'dt.Columns.Add(columna1)
        'dt.Columns.Add("extension_doc")
        'dt.Columns.Add("descripcion_doc")
        'dt.Columns.Add("usuario_eliminacion")
        'dt.Columns.Add("fecha_eliminacion")
        'dt.Columns.Add("flg_firma")
        'dt.Columns.Add("fec_firma")
        'dt.Columns.Add("usr_firma")
        'dt.Columns.Add("blb_documentofirma")
        'dt.Columns.Add("dsc_extensionfirma")
        'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

        'Dim docu As Byte()
        'Response.Clear()
        ''Response.Buffer = True
        ''Response.Charset = ""
        ''Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/pdf"
        ''la variable nombre es el nombre del archivo .pdf
        'Response.AddHeader("Content-disposition", "attachment; filename=" & dt.Rows(0)("descripcion_doc").ToString() + ".pdf")
        'docu = DirectCast(dt.Rows(0)("bib_documento"), Byte())
        'Response.BinaryWrite(docu)
        'Response.Flush()
        'Response.End()
        ''FIN - JB - 30/01/2017

        If Not IsNothing(Request.Params("IE")) Then
            For index = 0 To Reporte1.GetData(Session(sIdeHistoria), Request.Params("IE"), 1).Rows.Count - 1
                tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), Request.Params("IE"), 1).Rows.Item(index).ItemArray)
            Next
        End If

        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinica.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If

    End Sub

    Public Sub rptInterconsulta(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceInterconsultaTableAdapter()
        Dim tabla As New DataTable("Rp_RceInterconsulta")
        tabla.Columns.Add("hc")
        tabla.Columns.Add("ide_interconsulta")
        tabla.Columns.Add("ape_paterno")
        tabla.Columns.Add("ape_materno")
        tabla.Columns.Add("nombre")
        tabla.Columns.Add("cuarto")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("dsc_de")
        tabla.Columns.Add("dsc_a")
        tabla.Columns.Add("fec_solicita")
        tabla.Columns.Add("hora_solicita")
        tabla.Columns.Add("dsc_solicita")
        'tabla.Columns.Add("firma_solicita")
        Dim columna_firma1 As DataColumn = New DataColumn("firma_solicita")
        columna_firma1.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma1)
        tabla.Columns.Add("fec_responde")
        tabla.Columns.Add("hora_responde")
        tabla.Columns.Add("dsc_responde")
        'tabla.Columns.Add("firma_responde")
        Dim columna_firma As DataColumn = New DataColumn("firma_responde")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico1")
        tabla.Columns.Add("RNE1")
        tabla.Columns.Add("CMP1")
        tabla.Columns.Add("nmedico2")
        tabla.Columns.Add("RNE2")
        tabla.Columns.Add("CMP2")

        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
        Next

        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpInterconsulta.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub

    Public Sub rptInterconsulta2(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceInterconsultaTableAdapter()
        Dim tabla As New DataTable("Rp_RceInterconsulta")
        tabla.Columns.Add("hc")
        tabla.Columns.Add("ide_interconsulta")
        tabla.Columns.Add("ape_paterno")
        tabla.Columns.Add("ape_materno")
        tabla.Columns.Add("nombre")
        tabla.Columns.Add("cuarto")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("dsc_de")
        tabla.Columns.Add("dsc_a")
        tabla.Columns.Add("fec_solicita")
        tabla.Columns.Add("hora_solicita")
        tabla.Columns.Add("dsc_solicita")
        'tabla.Columns.Add("firma_solicita")
        Dim columna_firma1 As DataColumn = New DataColumn("firma_solicita")
        columna_firma1.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma1)
        tabla.Columns.Add("fec_responde")
        tabla.Columns.Add("hora_responde")
        tabla.Columns.Add("dsc_responde")
        'tabla.Columns.Add("firma_responde")
        Dim columna_firma As DataColumn = New DataColumn("firma_responde")
        columna_firma.DataType = System.Type.GetType("System.Byte[]")
        tabla.Columns.Add(columna_firma)
        tabla.Columns.Add("nmedico1")
        tabla.Columns.Add("RNE1")
        tabla.Columns.Add("CMP1")
        tabla.Columns.Add("nmedico2")
        tabla.Columns.Add("RNE2")
        tabla.Columns.Add("CMP2")

        If Not IsNothing(Request.Params("II")) Then
            For index = 0 To Reporte1.GetData(Session(sIdeHistoria), Request.Params("II"), 1).Rows.Count - 1
                tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), Request.Params("II"), 1).Rows.Item(index).ItemArray)
            Next
        End If

        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpInterconsulta.rpt"))
        crystalreport.SetDataSource(tabla)
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub

    Public Sub rptHojaConsentimiento(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceHojadeconsentimientoTableAdapter()
        'Dim tabla As New DataTable("Sp_HojadeConsentimiento")
        Dim tabla As New DataTable("Rp_RceHojadeconsentimiento")
        tabla.Columns.Add("codatencion")
        tabla.Columns.Add("codpaciente")
        tabla.Columns.Add("cama")
        tabla.Columns.Add("nombres")
        tabla.Columns.Add("docidentidad")
        tabla.Columns.Add("titular")
        tabla.Columns.Add("fechanacimiento")
        tabla.Columns.Add("edad")
        tabla.Columns.Add("direccion")
        tabla.Columns.Add("telefono")
        tabla.Columns.Add("fechahora")
        tabla.Columns.Add("medicoprocedimiento")
        tabla.Columns.Add("docidentidadmedico")
        tabla.Columns.Add("colmedico")
        tabla.Columns.Add("medtelefono")
        tabla.Columns.Add("Lcodtiporepresentante")
        tabla.Columns.Add("Lnombrerepresentante")
        tabla.Columns.Add("Ltipdocidentidad")
        tabla.Columns.Add("Ldocidentidad")
        tabla.Columns.Add("Lnombretiporepresentante")
        tabla.Columns.Add("Lnombretipdocidentidad")
        tabla.Columns.Add("nombrediag")
        tabla.Columns.Add("nombretratamiento")
        tabla.Columns.Add("nombrecirugia")
        tabla.Columns.Add("plantilla")

        tabla.Columns.Add("Titulo")
        tabla.Columns.Add("Servicio")
        tabla.Columns.Add("Nombre")
        tabla.Columns.Add("FlagDiagnostico")
        tabla.Columns.Add("Diagnostico")
        tabla.Columns.Add("FlagTratamiento")
        tabla.Columns.Add("Tratamiento_Quirurgico")
        tabla.Columns.Add("Molestias")
        tabla.Columns.Add("Riesgos_Complicaciones")
        tabla.Columns.Add("Ventajas_Beneficios")
        tabla.Columns.Add("Riesgos_Personalizados")
        tabla.Columns.Add("Tratamiento_Alternativo")
        tabla.Columns.Add("Riesgos_No_Operacion")
        tabla.Columns.Add("Estado")
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        ''INICIO - JB - 30/01/2017
        'oHospitalE.IdeHistoria = Session(sIdeHistoria)
        'oHospitalE.TipoDoc = 2
        'Dim dt As New DataTable()
        'dt.Columns.Add("id_documento")
        'dt.Columns.Add("codatencion")
        'dt.Columns.Add("tipo_doc")
        'dt.Columns.Add("usuario_creacion")
        'dt.Columns.Add("fecha_creacion")
        'dt.Columns.Add("estado")
        'Dim columna1 As DataColumn = New DataColumn("bib_documento")
        'columna1.DataType = System.Type.GetType("System.Byte[]")
        'dt.Columns.Add(columna1)
        'dt.Columns.Add("extension_doc")
        'dt.Columns.Add("descripcion_doc")
        'dt.Columns.Add("usuario_eliminacion")
        'dt.Columns.Add("fecha_eliminacion")
        'dt.Columns.Add("flg_firma")
        'dt.Columns.Add("fec_firma")
        'dt.Columns.Add("usr_firma")
        'dt.Columns.Add("blb_documentofirma")
        'dt.Columns.Add("dsc_extensionfirma")
        'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

        'Dim docu As Byte()
        'Response.Clear()
        ''Response.Buffer = True
        ''Response.Charset = ""
        ''Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/pdf"
        ''la variable nombre es el nombre del archivo .pdf
        'Response.AddHeader("Content-disposition", "attachment; filename=" & dt.Rows(0)("descripcion_doc").ToString() + ".pdf")
        'docu = DirectCast(dt.Rows(0)("bib_documento"), Byte())
        'Response.BinaryWrite(docu)
        'Response.Flush()
        'Response.End()
        ''FIN - JB - 30/01/2017


        For index = 0 To Reporte1.GetData(Request.Params("CodProc")).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Request.Params("CodProc")).Rows.Item(index).ItemArray)
        Next
        Dim crystalreport As New ReportDocument()
        'crystalreport.("09.Usuario") = Session(sCodUser)
        'crystalreport.RecordSelectionFormula

        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/" + Request.Params("Reporte")))
        'TMACASSI 08/11/2016 ENVIAR PARAMETROS AL REPORTE
        'crystalreport.ParameterFields("Usuario").DefaultValues = "TMACASSI"
        'crystalreport.SetParameterValue("Usuario", "TMACASSI")
        'crystalreport.DataDefinition..Name("Usuario") = "TMACASSI"
        crystalreport.SetDataSource(tabla)
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub

    Public Sub rptInformeMedico(ByVal xPrint As Integer, ByVal xExport As Integer, ByVal xNombreArchivo As String)
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceInformeMedicoTableAdapter()
        Dim tabla As New DataTable("Rp_RceInformeMedico")
        tabla.Columns.Add("ide_historia")
        tabla.Columns.Add("cod_atencion")
        tabla.Columns.Add("npaciente")
        tabla.Columns.Add("edad")
        tabla.Columns.Add("dni_paciente")
        tabla.Columns.Add("telefono")
        tabla.Columns.Add("direccion")
        tabla.Columns.Add("medico")
        tabla.Columns.Add("aseguradora")
        tabla.Columns.Add("contratante")
        tabla.Columns.Add("poliza")
        tabla.Columns.Add("tarifa")
        tabla.Columns.Add("fecha_atencion")
        tabla.Columns.Add("diasatencion")
        tabla.Columns.Add("condicionalta")
        tabla.Columns.Add("tipoalta")
        tabla.Columns.Add("necropsia")
        tabla.Columns.Add("patologia")
        tabla.Columns.Add("enf_actual")
        tabla.Columns.Add("ef_FC")
        tabla.Columns.Add("ef_FE")
        tabla.Columns.Add("ef_PA")
        tabla.Columns.Add("ef_TEMP")
        tabla.Columns.Add("ef_PESO")
        tabla.Columns.Add("ef_TALLA")
        tabla.Columns.Add("ef_IMC")
        tabla.Columns.Add("ev_s")
        tabla.Columns.Add("ev_o")
        tabla.Columns.Add("ev_a")
        tabla.Columns.Add("ev_p")
        tabla.Columns.Add("ev_tipo")
        tabla.Columns.Add("diagnostico")
        tabla.Columns.Add("cie10")
        tabla.Columns.Add("nmedico")
        tabla.Columns.Add("RNE")
        tabla.Columns.Add("CMP")

        For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0).Rows.Count - 1
            tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0).Rows.Item(index).ItemArray)
        Next

        'Dim RpExamenes As New dsHospital_ReporteTableAdapters.Rp_RceInformeExamenesTableAdapter()
        'Dim dtExamenes As New DataTable("Rp_RceInformeExamenes")
        'dtExamenes.Columns.Add("tiporeceta")
        'dtExamenes.Columns.Add("dsc_analisis")
        'dtExamenes.Columns.Add("fec_registra")

        'For index = 0 To RpExamenes.GetData(Session(sIdeHistoria), 0).Rows.Count - 1
        '    dtExamenes.Rows.Add(RpExamenes.GetData(Session(sIdeHistoria), 0).Rows.Item(index).ItemArray)
        'Next

        'Dim RpEvolucion As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
        'Dim dtEvolucion As New DataTable("Rp_RceEvolucion")
        'dtEvolucion.Columns.Add("ide_historia")
        'dtEvolucion.Columns.Add("ide_evolucion")
        'dtEvolucion.Columns.Add("medico")
        'dtEvolucion.Columns.Add("flg_educacion")
        'dtEvolucion.Columns.Add("flg_informe")
        'dtEvolucion.Columns.Add("TipoEvolucion")
        'dtEvolucion.Columns.Add("txt_detalle")
        'dtEvolucion.Columns.Add("cama")
        'dtEvolucion.Columns.Add("cuarto")
        'dtEvolucion.Columns.Add("dni")
        'dtEvolucion.Columns.Add("fec_nacimiento")
        'dtEvolucion.Columns.Add("fec_ingreso")
        'dtEvolucion.Columns.Add("ape_paterno")
        'dtEvolucion.Columns.Add("ape_materno")
        'dtEvolucion.Columns.Add("nombres")
        'dtEvolucion.Columns.Add("fec_registro")
        'dtEvolucion.Columns.Add("hora_registro")
        ''tabla.Columns.Add("firma_medico")
        'Dim columna_firma As DataColumn = New DataColumn("firma_medico")
        'columna_firma.DataType = System.Type.GetType("System.Byte[]")
        'dtEvolucion.Columns.Add(columna_firma)

        'For index = 0 To RpEvolucion.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
        '    dtEvolucion.Rows.Add(RpEvolucion.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
        'Next

        Dim crystalreport As New ReportDocument()
        crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEpicrisis.rpt"))
        crystalreport.SetDataSource(tabla)
        'crystalreport.Subreports.Item(0).SetDataSource(dtExamenes)
        'crystalreport.Subreports.Item(1).SetDataSource(dtEvolucion)
        'VisorReporte.RefreshReport()
        'VisorReporte.ReportSource = crystalreport
        If xPrint = 1 Then
            crystalreport.PrintToPrinter(1, False, 0, 0)
        Else
            VisorReporte.ReportSource = crystalreport
        End If
    End Sub


End Class