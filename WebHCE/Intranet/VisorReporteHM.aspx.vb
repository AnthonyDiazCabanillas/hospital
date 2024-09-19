Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Entidades.EvolucionE
Imports LogicaNegocio.EvolucionLN
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN


Public Class VisorReporteHM
    Inherits System.Web.UI.Page
    Dim oRceEvolucionE As New RceEvolucionE()
    Dim oRceEvolucionLN As New RceEvolucionLN()
    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()

    Dim rpt As New ReportDocument()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerReporte()
        'If Not Page.IsPostBack Then
        '    VerReporte()
        'Else
        '    rpt = CType(Session("ReporteHM"), ReportDocument)
        '    CrystalReportViewer1.ReportSource = rpt
        'End If
    End Sub


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CrystalReportViewer1.ReportSource.Close()
        'CrystalReportViewer1.Dispose()
    End Sub

    'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not Page.IsPostBack Then
    '        VerReporte()
    '    Else
    '        rpt = CType(Session("ReporteHM"), ReportDocument)
    '        CrystalReportViewer1.ReportSource = rpt
    '    End If
    'End Sub


    Public Sub VerReporte()
        Try
            Dim FechaInicio As String = ""
            Dim FechaFin As String = ""

            FechaInicio = Request.Params("FI")
            FechaFin = Request.Params("FF")
            oRceEvolucionE.FecInicio = Format(CDate(FechaInicio), "MM/dd/yyyy")
            oRceEvolucionE.FecFin = Format(CDate(FechaFin), "MM/dd/yyyy")

            If Request.Params("OP") = "EC" Then
                Dim dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13, dt14, dt15, dt16 As New DataTable()
                Dim dtx As New DataTable()
                dtx.Columns.Add("ide_evolucion")
                dtx.Columns.Add("codmedico")
                dtx.Columns.Add("nommedico")
                dtx.Columns.Add("especialidad")
                dtx.Columns.Add("servicio")
                dtx.Columns.Add("cod_atencion")
                dtx.Columns.Add("fec_registra")
                dtx.Columns.Add("hora_registro")
                dtx.Columns.Add("fecha_registro")

                oRceEvolucionE.Orden = 1
                dt = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinicaHM.rpt"))
                rpt.SetDataSource(dt)
                CrystalReportViewer1.ReportSource = rpt
                'Session("ReporteHM") = rpt
                'rpt.Subreports("RpEvolucionClinicaHMSub2.rpt").SetDataSource(dt)


                'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
                Dim txtFechaInicio As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text9")
                txtFechaInicio.Text = FechaInicio
                Dim txtFechaFin As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text12")
                txtFechaFin.Text = FechaFin


                oRceEvolucionE.Orden = 2
                '01
                oRceEvolucionE.CodServicio = "01"
                dt1 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_01").SetDataSource(dt1)

                If dt1.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion1 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection1")
                    DetalleSeccion1.SectionFormat.EnableSuppress = True
                End If
                '02
                oRceEvolucionE.CodServicio = "02"
                dt2 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_02").SetDataSource(dt2)


                If dt2.Rows.Count Then
                Else
                    Dim DetalleSeccion2 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection2")
                    DetalleSeccion2.SectionFormat.EnableSuppress = True
                End If
                '03
                oRceEvolucionE.CodServicio = "03"
                dt3 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_03").SetDataSource(dt3)


                If dt3.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion3 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection3")
                    DetalleSeccion3.SectionFormat.EnableSuppress = True
                End If

                '04
                oRceEvolucionE.CodServicio = "04"
                dt4 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_04").SetDataSource(dt4)


                If dt4.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion4 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection4")
                    DetalleSeccion4.SectionFormat.EnableSuppress = True
                End If

                '05
                oRceEvolucionE.CodServicio = "05"
                dt5 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_05").SetDataSource(dt5)


                If dt5.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion5 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection5")
                    DetalleSeccion5.SectionFormat.EnableSuppress = True
                End If


                '06
                oRceEvolucionE.CodServicio = "06"
                dt6 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_06").SetDataSource(dt6)


                If dt6.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion6 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection6")
                    DetalleSeccion6.SectionFormat.EnableSuppress = True
                End If

                '07
                oRceEvolucionE.CodServicio = "07"
                dt7 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_07").SetDataSource(dt7)


                If dt7.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion7 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection7")
                    DetalleSeccion7.SectionFormat.EnableSuppress = True
                End If

                '08
                oRceEvolucionE.CodServicio = "08"
                dt8 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_08").SetDataSource(dt8)


                If dt8.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion8 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection8")
                    DetalleSeccion8.SectionFormat.EnableSuppress = True
                End If

                '09
                oRceEvolucionE.CodServicio = "09"
                dt9 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_09").SetDataSource(dt9)


                If dt9.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion9 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection9")
                    DetalleSeccion9.SectionFormat.EnableSuppress = True
                End If

                '10
                oRceEvolucionE.CodServicio = "10"
                dt10 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_10").SetDataSource(dt10)


                If dt10.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion10 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection10")
                    DetalleSeccion10.SectionFormat.EnableSuppress = True
                End If

                '11
                oRceEvolucionE.CodServicio = "11"
                dt11 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_11").SetDataSource(dt11)


                If dt11.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion11 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection11")
                    DetalleSeccion11.SectionFormat.EnableSuppress = True
                End If

                '12
                oRceEvolucionE.CodServicio = "12"
                dt12 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_12").SetDataSource(dt12)


                If dt12.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion12 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection12")
                    DetalleSeccion12.SectionFormat.EnableSuppress = True
                End If

                '13
                oRceEvolucionE.CodServicio = "13"
                dt13 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_13").SetDataSource(dt13)


                If dt13.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion13 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection13")
                    DetalleSeccion13.SectionFormat.EnableSuppress = True
                End If

                '14
                oRceEvolucionE.CodServicio = "14"
                dt14 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_14").SetDataSource(dt14)


                If dt14.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion14 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection14")
                    DetalleSeccion14.SectionFormat.EnableSuppress = True
                End If

                '15
                oRceEvolucionE.CodServicio = "15"
                dt15 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_15").SetDataSource(dt15)


                If dt15.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion15 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection15")
                    DetalleSeccion15.SectionFormat.EnableSuppress = True
                End If

                '16
                oRceEvolucionE.CodServicio = "AC"
                dt16 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE)
                rpt.Subreports("RpEvolucionClinicaHMSub.rpt_16").SetDataSource(dt16)


                If dt16.Rows.Count > 0 Then
                Else
                    Dim DetalleSeccion16 As CrystalDecisions.CrystalReports.Engine.Section = rpt.ReportDefinition.Sections.Item("ReportFooterSection16")
                    DetalleSeccion16.SectionFormat.EnableSuppress = True
                End If

                'rpt.Close()
                'rpt.Dispose()
            End If
            If Request.Params("OP") = "PR" Then
                oHospitalE.FecInicio = Format(CDate(FechaInicio), "MM/dd/yyyy")
                oHospitalE.FecFin = Format(CDate(FechaFin), "MM/dd/yyyy")
                oHospitalE.Orden = 1
                Dim dt, dt1 As New DataTable()
                dt = oHospitalLN.Rp_ProcedimientosHM(oHospitalE)

                rpt.Load(Server.MapPath("~/Intranet/Reporte/RpProcedimientosHM.rpt"))
                rpt.SetDataSource(dt)
                CrystalReportViewer1.ReportSource = rpt


                'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
                Dim txtFechaInicio As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text8")
                txtFechaInicio.Text = FechaInicio
                Dim txtFechaFin As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text11")
                txtFechaFin.Text = FechaFin


                oHospitalE.Orden = 2
                dt1 = oHospitalLN.Rp_ProcedimientosHM(oHospitalE)
                rpt.Subreports("RpProcedimientosHMSub.rpt").SetDataSource(dt1)
            End If
            If Request.Params("OP") = "IN" Then 'Reporte Interconsulta
                'CONSULTANDO DATOS DEL REPORTE PRINCIPAL
                oInterconsultaE.FecInicio = Format(CDate(FechaInicio), "MM/dd/yyyy")
                oInterconsultaE.FecFin = Format(CDate(FechaFin), "MM/dd/yyyy")
                oInterconsultaE.Orden = 1
                Dim dt, dt1 As New DataTable()
                dt = oInterconsultaLN.Rp_InterconsultaHM(oInterconsultaE)

                rpt.Load(Server.MapPath("~/Intranet/Reporte/RpInterconsultaHM.rpt"))
                rpt.SetDataSource(dt)

                'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
                Dim txtFechaInicio As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text2")
                txtFechaInicio.Text = FechaInicio
                Dim txtFechaFin As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text5")
                txtFechaFin.Text = FechaFin


                'CONSULTANDO DATOS DEL SUBREPORTE (RESUMEN)
                oInterconsultaE.Orden = 2
                dt1 = oInterconsultaLN.Rp_InterconsultaHM(oInterconsultaE)
                rpt.Subreports("RpInterconsultaHMSub.rpt").SetDataSource(dt1)

                CrystalReportViewer1.ReportSource = rpt
            End If

            If Request.Params("OP") = "GC" Then 'Reporte Gestion Clinica
                'CONSULTANDO DATOS DEL REPORTE PRINCIPAL
                oHospitalE.FecInicio = Format(CDate(FechaInicio), "MM/dd/yyyy")
                oHospitalE.FecFin = Format(CDate(FechaFin), "MM/dd/yyyy")
                oHospitalE.Orden = 1
                Dim dt, dt1 As New DataTable()
                dt = oHospitalLN.Rp_GestionClinica(oHospitalE)

                rpt.Load(Server.MapPath("~/Intranet/Reporte/RpGestionClinica.rpt"))
                rpt.SetDataSource(dt)
                CrystalReportViewer1.ReportSource = rpt


                'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
                Dim txtFechaInicio As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text7")
                txtFechaInicio.Text = FechaInicio
                Dim txtFechaFin As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text5")
                txtFechaFin.Text = FechaFin

            End If

            If Request.Params("OP") = "LH" Then 'Reporte Libro Hospitalizacion
                oHospitalE.FecInicio = Format(CDate(FechaInicio), "MM/dd/yyyy")
                oHospitalE.FecFin = Format(CDate(FechaFin), "MM/dd/yyyy")
                oHospitalE.Orden = 0
                Dim dt, dt1 As New DataTable()
                dt = oHospitalLN.Rp_LibrodeHospitalizacion(oHospitalE)

                rpt.Load(Server.MapPath("~/Intranet/Reporte/RpLibroHospitalizacion.rpt"))
                rpt.SetDataSource(dt)
                CrystalReportViewer1.ReportSource = rpt


                'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
                Dim txtFechaInicio As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text8")
                txtFechaInicio.Text = FechaInicio
                Dim txtFechaFin As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text11")
                txtFechaFin.Text = FechaFin
            End If



            'Dim rpt As New ReportDocument()
            'rpt.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinicaHM.rpt"))
            'rpt.SetDataSource(dt)
            'CrystalReportViewer1.ReportSource = rpt
            'rpt.Close()
            'rpt.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub CerrarReporteHM()
        rpt.Close()
        rpt.Dispose()
        'CrystalReportViewer1.Dispose()
    End Sub


End Class