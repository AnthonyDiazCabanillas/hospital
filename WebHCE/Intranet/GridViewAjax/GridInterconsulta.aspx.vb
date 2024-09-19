Imports System.Data
Imports System.IO
Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN

Public Class GridInterconsulta
    Inherits System.Web.UI.Page
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()

    Dim InterconsultaR As String = ConfigurationManager.AppSettings("INTERCONSULTA_ROJO").Trim()
    Dim InterconsultaA As String = ConfigurationManager.AppSettings("INTERCONSULTA_AMARILLO").Trim()
    Dim InterconsultaV As String = ConfigurationManager.AppSettings("INTERCONSULTA_VERDE").Trim()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListarInterconsulta()
        End If

    End Sub

    Public Sub ListarInterconsulta()
        oInterconsultaE.Atencion = Session(sCodigoAtencion)
        oInterconsultaE.IdeHistoria = Session(sIdeHistoria)
        oInterconsultaE.Orden = 1
        Dim tabla As New DataTable()
        If IsNothing(Request.Params("Pagina")) Then
            gvInterconsulta.PageIndex = 0
            tabla = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
            gvInterconsulta.DataSource = tabla
            gvInterconsulta.DataBind()
        Else
            gvInterconsulta.PageIndex = (CType(Request.Params("Pagina").ToString().Trim(), Integer) - 1)
            tabla = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
            gvInterconsulta.DataSource = tabla
            gvInterconsulta.DataBind()
        End If
    End Sub

    Protected Sub gvInterconsulta_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInterconsulta.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgIntercon As New System.Web.UI.HtmlControls.HtmlImage

            If e.Row.Cells(9).Text.Trim() = "P" Then
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/" + InterconsultaR + ""
                imgIntercon.Width = 35
                imgIntercon.Height = 35
            ElseIf e.Row.Cells(9).Text.Trim() = "C" Then
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/" + InterconsultaV + ""
                imgIntercon.Width = 35
                imgIntercon.Height = 35
            Else
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = ""
            End If

        End If
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarInterconsulta(ByVal IdInterconsulta As String) As String
        Dim pagina As New GridInterconsulta()
        Return pagina.EliminarInterconsulta_(IdInterconsulta)
    End Function

    Public Function EliminarInterconsulta_(ByVal IdInterconsulta As String) As String
        Try
            oInterconsultaE.IdeInterConsulta = IdInterconsulta
            Dim elimino As Boolean = oInterconsultaLN.Sp_RceInterconsulta_Delete(oInterconsultaE)
            If elimino <> True Then
                Return ConfigurationManager.AppSettings(sMensajeEliminarError).Trim()
            Else
                Return "OK"
            End If
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try
    End Function

End Class