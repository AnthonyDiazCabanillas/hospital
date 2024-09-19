Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN

Public Class GridLaboratorioCompletado
    Inherits System.Web.UI.Page
    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaLaboratorioCompletado()
        End If
    End Sub

    Public Sub ListaLaboratorioCompletado()
        If Not IsNothing(Request.Params("IdReceta")) Then
            hfIdReceta_.Value = Request.Params("IdReceta").Trim()
            oRceLaboratioE.IdeRecetaCab = hfIdReceta_.Value
            oRceLaboratioE.Orden = 4
            oRceLaboratioE.CodAtencion = ""
            gvLaboratorioCompletado.DataSource = oRceLaboratorioLN.Sp_RceRecetaAnalisis_Consulta(oRceLaboratioE)
            gvLaboratorioCompletado.DataBind()
        End If
    End Sub


End Class