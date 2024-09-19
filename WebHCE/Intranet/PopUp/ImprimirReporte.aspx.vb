Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN

Public Class ImprimirReporte
    Inherits System.Web.UI.Page
    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarReportesData() As String
        Dim pagina As New ImprimirReporte()
        Return pagina.VerificarReportesData_()
    End Function

    Public Function VerificarReportesData_() As String
        Dim CadenaCheck As String = ""
        Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceAlergiaTableAdapter()
        If Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkDeclaratorioAlergiaReporte" + ";"
        End If

        Dim Reporte2 As New dsHospital_ReporteTableAdapters.Rp_RceMedicamentosaTableAdapter()
        If Reporte2.GetData(Session(sIdeHistoria), 1).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkRegistroMedicoReconsiliacionReporte" + ";"
        End If

        Dim Reporte3 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaMedicamentoTableAdapter()
        If Reporte3.GetData(Session(sIdeHistoria), 0, "", 3).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkIndicacionMedicaReporte" + ";"
        End If

        Dim Reporte4 As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
        If Reporte4.GetData(Session(sIdeHistoria), 0, 1).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkEvolucionClinicaReporte" + ";"
        End If

        Dim Reporte5 As New dsHospital_ReporteTableAdapters.Rp_RceInterconsultaTableAdapter()
        If Reporte5.GetData(Session(sIdeHistoria), 0, 1).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkInterconsultaReporte" + ";"
        End If

        Dim Reporte6 As New dsHospital_ReporteTableAdapters.Rp_RceInformeMedicoTableAdapter()
        If Reporte6.GetData(Session(sIdeHistoria), 0).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkInformeMedicoReporte" + ";"
        End If

        Dim Reporte7 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaAltaTableAdapter()
        If Reporte7.GetData(Session(sIdeHistoria), 0, 1).Rows.Count > 0 Then
        Else
            CadenaCheck += "chkRecetaAlta" + ";"
        End If
        Return CadenaCheck
    End Function

    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarCantidadReportes() As String
        Dim pagina As New ImprimirReporte()
        Return pagina.VerificarCantidadReportes_()
    End Function

    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function VerificarCantidadReportes_() As String
        Try
            Dim tabla As New DataTable()
            oHospitalE.CodAtencion = Session(sCodigoAtencion)
            oHospitalE.Orden = 2
            tabla = oHospitalLN.Sp_HospitalProcedimiento_Consulta(oHospitalE)
            Dim Reportes As String = ""
            Dim CodProcedimientos As String = ""

            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    oHospitalE.IdePlantilla = tabla.Rows(index)("IdPlantilla").ToString()
                    oHospitalE.Orden = 0
                    Dim tabla_plantilla As New DataTable()
                    tabla_plantilla = oHospitalLN.Sp_CIFormatos_Consulta(oHospitalE)
                    If tabla_plantilla.Rows.Count > 0 Then
                        For index_plantilla = 0 To tabla_plantilla.Rows.Count - 1
                            Reportes += tabla_plantilla.Rows(index_plantilla)("archivo").ToString().Trim() + ";"
                        Next
                    End If
                    CodProcedimientos += tabla.Rows(index)("codprocedimiento").ToString().Trim() + ";"
                Next
            End If
            'Reportes = "CIFormatoQuimio.rpt;CIFormatoRLDP.rpt" 'ELIMINAR ESTA LINEA DE CODIGO

            Return Reportes + "*" + CodProcedimientos
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function
End Class