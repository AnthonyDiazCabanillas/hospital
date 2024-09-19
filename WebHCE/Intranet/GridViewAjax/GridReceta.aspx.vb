Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class GridReceta
    Inherits System.Web.UI.Page
    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaReceta()
        End If
    End Sub

    Public Sub ListaReceta()
        Dim tabla As New DataTable()
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)

        If Not IsNothing(Request.Params("IdReceta")) Then
            hfIdRecetaG.Value = Request.Params("IdReceta")
            oRceRecetaMedicamentoE.IdRecetaDet = Request.Params("IdReceta")
            oRceRecetaMedicamentoE.Orden = 2
            tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
            gvProductoMedicamento.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
            gvProductoMedicamento.DataSource = tabla
            gvProductoMedicamento.DataBind()
        Else
            oRceRecetaMedicamentoE.IdRecetaDet = 0 'cargara vacio
            oRceRecetaMedicamentoE.Orden = 2
            tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
            gvProductoMedicamento.DataSource = tabla
            gvProductoMedicamento.DataBind()
        End If
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarReceta(ByVal Codigo As String) As String
        Dim pagina As New GridReceta()
        Return pagina.EliminarReceta_(Codigo)
    End Function

    Public Function EliminarReceta_(ByVal Codigo As String) As String
        Try
            'Sp_RceRecetaMedicamentoDet_Update flg_estado',@iderecetadet,'0'
            oRceRecetaMedicamentoE.Campo = "flg_estado"
            oRceRecetaMedicamentoE.IdRecetaDet = Codigo
            oRceRecetaMedicamentoE.ValorNuevo = "0"
            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function

End Class