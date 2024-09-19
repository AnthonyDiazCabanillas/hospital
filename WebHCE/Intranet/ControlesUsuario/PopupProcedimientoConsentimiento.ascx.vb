Imports System.Data
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN

Public Class PopupProcedimientoConsentimiento
    Inherits System.Web.UI.UserControl

    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListarProcedimientos()
        End If

    End Sub

    Public Sub ListarProcedimientos()
        oHospitalE.CodAtencion = Session(sCodigoAtencion)
        oHospitalE.Orden = 2
        Dim dt As New DataTable()
        dt = oHospitalLN.Sp_HospitalProcedimiento_Consulta(oHospitalE)
        gvListaProcedimientoConsentimiento.DataSource = dt
        gvListaProcedimientoConsentimiento.DataBind()
    End Sub

End Class