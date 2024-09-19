' **********************************************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2023. Todos los derechos reservados.
'    Version     Fecha           Autor       Requerimiento
'    1.1         20/10/2023      AROMERO     REQ-2023-017255:  REPORTE HISTORIA CLINICA HOPITAL
'***********************************************************************************************************************
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.EvolucionE
Imports LogicaNegocio.EvolucionLN

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports
Imports System.Data.SqlClient
Imports Entidades.JuntaMedicaE
Imports LogicaNegocio.JuntaMedicaLN
Imports Entidades.NotaIngresoE
Imports LogicaNegocio.NotaIngresoLN
Imports CrystalDecisions.[Shared].Json

Public Class ImpresionReporte
    Inherits System.Web.UI.Page

    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()
    Dim xRuta As String = sRutaTemp
    Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            VerificarReportesExistente()
        End If
    End Sub


    Public Sub VerificarReportesExistente()
        oHospitalE.IdeHistoria = Session(sIdeHistoria)
        Dim tabla As New DataTable()

        oHospitalE.Tipo = "CI"
        'tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE) JB - COMENTADO - 24/01/2020

        'INICIO - JB - NUEVO CODIGO - 24/01/2020
        oHospitalE.CodAtencion = Session(sCodigoAtencion)
        oHospitalE.Orden = 2
        Dim dt As New DataTable()
        dt = oHospitalLN.Sp_HospitalProcedimiento_Consulta(oHospitalE)
        If dt.Rows.Count = 0 Then
            hfReporteExistente.Value = hfReporteExistente.Value + "CI" + ";"
        End If
        'FIN - JB - NUEVO CODIGO - 24/01/2020

        oHospitalE.Tipo = "DA"
        tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE)
        If tabla.Rows(0)("total").ToString().Trim() = "0" Then
            hfReporteExistente.Value = hfReporteExistente.Value + "DA" + ";"
        End If

        oHospitalE.Tipo = "EC"
        tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE)
        If tabla.Rows(0)("total").ToString().Trim() = "0" Then
            hfReporteExistente.Value = hfReporteExistente.Value + "EC" + ";"
        End If

        'INICIO - JB - EVOLUCION - 07/02/2020
        'Dim crystalreport As New ReportDocument()
        'Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
        'Dim tablaev As New DataTable("Rp_RceEvolucion")
        'tablaev.Columns.Add("ide_historia")
        'tablaev.Columns.Add("ide_evolucion")
        'tablaev.Columns.Add("medico")
        'tablaev.Columns.Add("flg_educacion")
        'tablaev.Columns.Add("flg_informe")
        'tablaev.Columns.Add("TipoEvolucion")
        'tablaev.Columns.Add("txt_detalle")
        'tablaev.Columns.Add("cama")
        'tablaev.Columns.Add("cuarto")
        'tablaev.Columns.Add("dni")
        'tablaev.Columns.Add("fec_nacimiento")
        'tablaev.Columns.Add("fec_ingreso")
        'tablaev.Columns.Add("ape_paterno")
        'tablaev.Columns.Add("ape_materno")
        'tablaev.Columns.Add("nombres")
        'tablaev.Columns.Add("fec_registro")
        'tablaev.Columns.Add("hora_registro")
        ''tabla.Columns.Add("firma_medico")
        'Dim columna_firma As DataColumn = New DataColumn("firma_medico")
        'columna_firma.DataType = System.Type.GetType("System.Byte[]")
        'tablaev.Columns.Add(columna_firma)
        'tablaev.Columns.Add("nmedico")
        'tablaev.Columns.Add("RNE")
        'tablaev.Columns.Add("CMP")

        'Dim oRceEvolucionE_ As New RceEvolucionE
        'Dim oRceEvolucionLN As New RceEvolucionLN
        'oRceEvolucionE_.IdHistoria = Session(sIdeHistoria)
        'oRceEvolucionE_.CodigoEvolucion = 0
        'oRceEvolucionE_.Orden = 1
        'tabla = oRceEvolucionLN.Rp_RceEvolucion(oRceEvolucionE_)

        'If tabla.Rows.Count > 0 Then 'SI HAY REGISTROS GENERARA EL REPORTE
        '    Dim pdf_byte As Byte() = Nothing

        '    Dim distinctValues = tablaev.AsEnumerable().[Select](Function(row) New With { _
        '     Key .ide_evolucion = row.Field(Of Integer)("ide_evolucion"),
        '         .ide_historia = row.Field(Of Integer)("ide_historia"),
        '         .medico = row.Field(Of String)("medico"),
        '         .flg_educacion = row.Field(Of String)("flg_educacion"),
        '         .flg_informe = row.Field(Of String)("flg_informe"),
        '         .TipoEvolucion = row.Field(Of String)("TipoEvolucion"),
        '         .txt_detalle = row.Field(Of String)("txt_detalle"),
        '         .cama = row.Field(Of String)("cama"),
        '         .cuarto = row.Field(Of String)("cuarto"),
        '         .dni = row.Field(Of String)("dni"),
        '         .fec_nacimiento = row.Field(Of DateTime)("fec_nacimiento"),
        '         .fec_ingreso = row.Field(Of DateTime)("fec_ingreso"),
        '         .ape_paterno = row.Field(Of String)("ape_paterno"),
        '         .ape_materno = row.Field(Of String)("ape_materno"),
        '         .nombres = row.Field(Of String)("nombres"),
        '         .fec_registro = row.Field(Of String)("fec_registro"),
        '         .hora_registro = row.Field(Of String)("hora_registro"),
        '         .firma_medico = row.Field(Of Byte())("firma_medico"),
        '         .nmedico = row.Field(Of String)("nmedico"),
        '         .RNE = row.Field(Of String)("RNE"),
        '    .CMP = row.Field(Of String)("CMP")
        '    }).Distinct()
        '    'INICIO - JB - NUEVO - 24/09/2019

        '    crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinica2.rpt"))
        '    crystalreport.SetDataSource(distinctValues)

        '    Dim OpcionExportar As ExportOptions
        '    Dim OpcionDestino As New DiskFileDestinationOptions()
        '    Dim OpcionesFormato As New PdfRtfWordFormatOptions()
        '    Dim NombreArchivo As String = "DA" + Session(sIdeHistoria).ToString() + ".pdf"
        '    OpcionDestino.DiskFileName = xRuta + "\" + NombreArchivo
        '    OpcionExportar = crystalreport.ExportOptions
        '    With OpcionExportar
        '        .ExportDestinationType = ExportDestinationType.DiskFile
        '        .ExportFormatType = ExportFormatType.PortableDocFormat
        '        .DestinationOptions = OpcionDestino
        '        .FormatOptions = OpcionesFormato
        '    End With
        '    crystalreport.Export()

        '    pdf_byte = System.IO.File.ReadAllBytes(xRuta + "\" + NombreArchivo)
        '    'System.IO.File.WriteAllBytes(xRuta + "\" + NombreArchivo, pdf_byte)
        '    crystalreport.Close()
        '    crystalreport.Dispose()




        '    Dim cn As New SqlConnection(CnnBD)
        '    'Paso 1
        '    oHospitalE.TipoDoc = 10
        '    oHospitalE.CodAtencion = Session(sCodigoAtencion)
        '    oHospitalE.CodUser = Session(sCodUser)
        '    oHospitalE.Descripcion = "" 'oRceEvolucionE.CodigoEvolucion.ToString()
        '    oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

        '    'Paso 2
        '    Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
        '    cmd1.CommandType = CommandType.Text
        '    cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
        '    cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
        '    cmd1.Parameters.AddWithValue("@flg_reqfirma", "0")

        '    Dim num1 As Integer
        '    cn.Open()
        '    num1 = cmd1.ExecuteNonQuery()
        '    cn.Close()

        '    'Paso 3
        '    oHospitalE.IdeHistoria = Session(sIdeHistoria)
        '    oHospitalE.IdeGeneral = 0 'oRceEvolucionE.CodigoEvolucion
        '    oHospitalE.TipoDoc = 10
        '    oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
        'Else
        '    hfReporteExistente.Value = hfReporteExistente.Value + "EC" + ";"
        'End If
        'FIN - EVOLUCIONA - 07/02/2020



        oHospitalE.Tipo = "EP"
        oHospitalE.Orden = 2
        'INICIO - JB - NUEVO CODIGO - 29/01/2020
        'tabla = oHospitalLN.Rp_RceInformeMedico(oHospitalE)   JB - 22/06/2020 - COMENTADO
        Dim oHospitalE_Inter As New HospitalE()
        oHospitalE_Inter.IdeHistoria = Session(sIdeHistoria)
        oHospitalE_Inter.CodAtencion = Session(sCodigoAtencion)
        oHospitalE_Inter.Orden = 1
        tabla = oHospitalLN.Sp_RceEpicrisis_Consulta(oHospitalE_Inter)

        If tabla.Rows.Count = 0 Then
            hfReporteExistente.Value = hfReporteExistente.Value + "EP" + ";"
        End If
        'FIN - JB - NUEVO CODIGO - 29/01/2020

        oHospitalE.Tipo = "IM"
        tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE)
        If tabla.Rows(0)("total").ToString().Trim() = "0" Then
            hfReporteExistente.Value = hfReporteExistente.Value + "IM" + ";"
        End If

        oHospitalE.Tipo = "IN"
        tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE)
        If tabla.Rows(0)("total").ToString().Trim() = "0" Then
            hfReporteExistente.Value = hfReporteExistente.Value + "IN" + ";"
        End If

        oHospitalE.Tipo = "ME"
        tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE)
        If tabla.Rows(0)("total").ToString().Trim() = "0" Then
            hfReporteExistente.Value = hfReporteExistente.Value + "ME" + ";"
        End If

        oHospitalE.Tipo = "RA"
        tabla = oHospitalLN.Sp_RCEReportes_Consulta(oHospitalE)
        If tabla.Rows(0)("total").ToString().Trim() = "0" Then
            hfReporteExistente.Value = hfReporteExistente.Value + "RA" + ";"
        End If



        Dim oRceJuntaMedicaE As New RceJuntaMedicaE()
        Dim oRceJuntaMedicaLN As New RceJuntaMedicaLN()
        oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
        oRceJuntaMedicaE.Orden = 1
        tabla = oRceJuntaMedicaLN.Rp_JuntaMedica(oRceJuntaMedicaE)
        If tabla.Rows.Count = 0 Then
            hfReporteExistente.Value = hfReporteExistente.Value + "JM" + ";"
        End If


        Dim oRceNotaIngresoE As New RceNotaIngresoE()
        Dim oRceNotaIngresoLN As New RceNotaIngresoLN()
        oRceNotaIngresoE.IdHistoria = Session(sIdeHistoria)
        oRceNotaIngresoE.Orden = 1
        tabla = oRceNotaIngresoLN.Rp_NotaIngreso(oRceNotaIngresoE)
        If tabla.Rows.Count = 0 Then
            hfReporteExistente.Value = hfReporteExistente.Value + "NI" + ";"
        End If


        '1.1 INI
        If Session(sCodigoAtencion).ToString().Substring(0, 1) = "H" Or Session(sCodigoAtencion).ToString().Substring(0, 1) = "Q" Then
            oHospitalE.NombrePaciente = Session(sCodigoAtencion)
            oHospitalE.Pabellon = ""
            oHospitalE.Servicio = ""
            oHospitalE.Orden = 5
            Dim tabla_atenciones As New DataTable()
            tabla_atenciones = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)
            Dim diferencia As String
            diferencia = DateDiff(DateInterval.Minute, Date.Parse(tabla_atenciones.Rows(0)("fec_registra")), Date.Parse(DateTime.Now.ToString()))

            If IsDBNull(tabla_atenciones.Rows(0)("fechaaltamedica")) Then 'No fue dado de alta
                If diferencia <= 1440 Then 'si es menor a un dia no puede imprimir Historia Clinica
                    hfReporteExistente.Value = hfReporteExistente.Value + "HC" + ";"
                End If
            End If
        Else
            hfReporteExistente.Value = hfReporteExistente.Value + "HC" + ";"
        End If
        '1.1 FIN

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarCantidadReportes(ByVal ReporteImprimir As String) As String
        Dim pagina As New ImpresionReporte()
        Return pagina.VerificarCantidadReportes_(ReporteImprimir)
    End Function

    Public Function VerificarCantidadReportes_(ByVal ReporteImprimir As String) As String
        Try
            Dim CodigoDevolver As String = ""
            If ReporteImprimir = "CI" Then 'consentimiento informado
                oHospitalE.IdeHistoria = Session(sIdeHistoria)
                oHospitalE.TipoDoc = 2
                Dim dt As New DataTable()
                dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

                If dt.Rows.Count > 0 Then
                    For index = 0 To dt.Rows.Count - 1
                        CodigoDevolver += dt.Rows(index)("id_documento").ToString().Trim() + ";"
                    Next
                End If
            ElseIf ReporteImprimir = "EC" Then 'evolucion clinica
                oHospitalE.IdeHistoria = Session(sIdeHistoria)
                oHospitalE.TipoDoc = 10
                Dim dt As New DataTable()
                dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)

                If dt.Rows.Count > 0 Then
                    For index = 0 To dt.Rows.Count - 1
                        CodigoDevolver += dt.Rows(index)("id_documento").ToString().Trim() + ";"
                    Next
                End If
            End If

            Return CodigoDevolver
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString() + "<br />Procedimiento Ejecutado: Sp_RceHospitalDoc_Consulta(" + oHospitalE.IdeHistoria.ToString() + "," + oHospitalE.IdeGeneral.ToString() + "," + oHospitalE.TipoDoc.ToString() + ")"
        End Try
    End Function


End Class