Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN

Public Class Interconsulta
    Inherits System.Web.UI.Page
    Dim oInterconsultaE As New InterconsultaE
    Dim oInterconsultaLN As New InterconsultaLN
    Dim Atencion As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaInterconsultaAtencion()
        End If
    End Sub
    'cambio
    Public Sub ListaInterconsultaAtencion()
        If Request.Params("Atencion") <> Nothing Then
            hfCodigoAtencion_Interconsulta.Value = Request.Params("Atencion").ToString().Trim()
            oInterconsultaE.Atencion = Request.Params("Atencion").ToString().Trim()
            oInterconsultaE.Orden = 1

            If Not IsNothing(Request.Params("Pagina")) And Request.Params("Pagina") <> Nothing Then
                gvInterconsultaPopUp.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
                gvInterconsultaPopUp.DataSource = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
                gvInterconsultaPopUp.DataBind()
            Else
                gvInterconsultaPopUp.DataSource = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
                gvInterconsultaPopUp.DataBind()
            End If

        End If
    End Sub

    Protected Sub gvInterconsultaPopUp_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInterconsultaPopUp.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgIntercon As New System.Web.UI.HtmlControls.HtmlImage

            If e.Row.Cells(8).Text.Trim() = "P" Then
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/InterconRojo.png"
            ElseIf e.Row.Cells(8).Text.Trim() = "C" Then
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/InterconVerde.png"
            Else
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = ""
            End If

        End If
    End Sub
End Class