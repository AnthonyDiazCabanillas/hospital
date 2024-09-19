Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class GridVerificarMedicamento
    Inherits System.Web.UI.Page
    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaMedicamento()
        End If
    End Sub

    Public Sub ListaMedicamento()

        If Not IsNothing(Request.Params("IdReceta")) Then
            hfIdReceta_.Value = Request.Params("IdReceta").Trim()
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            oRceRecetaMedicamentoE.IdRecetaDet = Request.Params("IdReceta").Trim()
            oRceRecetaMedicamentoE.Orden = 6
            Dim tabla As New DataTable()
            tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
            gvVerificarMedicamento.DataSource = tabla
            gvVerificarMedicamento.DataBind()
        End If


    End Sub


End Class