Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.MedicamentosE
Imports LogicaNegocio.MedicamentosLN

Public Class PopUpFechaReceta
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            CargarFechasReceta()
        End If

    End Sub

    Public Sub CargarFechasReceta()
        Dim oRceMedicamentosE As New RceMedicamentosE()
        Dim oRceMedicamentosLN As New RceMedicamentosLN()
        oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
        oRceMedicamentosE.Orden = 7
        Dim dt As New DataTable()
        dt = oRceMedicamentosLN.Rp_RceRecetaMedicamento1(oRceMedicamentosE)
        gvListaFechaReceta.DataSource = dt
        gvListaFechaReceta.DataBind()
    End Sub



End Class