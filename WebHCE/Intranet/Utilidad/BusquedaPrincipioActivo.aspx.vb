Imports System.Data
Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN

Public Class BusquedaPrincipioActivo
    Inherits System.Web.UI.Page
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            ListaPrincipioActivo()
        End If
    End Sub


    Public Sub ListaPrincipioActivo()
        If Request.Params("Buscar") <> Nothing Then
            oRceAlergiaE.Buscar = Request.Params("Buscar").Trim().ToUpper()
        Else
            oRceAlergiaE.Buscar = ""
        End If
        oRceAlergiaE.Key = 33
        oRceAlergiaE.NumeroLineas = 20
        oRceAlergiaE.Orden = 2
        Dim tabla As New DataTable
        tabla = oRceAlergiaLN.Sp_Genericos_Consulta(oRceAlergiaE)
        gvBusquedaPrincipioActivo.DataSource = tabla
        gvBusquedaPrincipioActivo.DataBind()

        'Sp_Genericos_Consulta '', 33, 20, 2

    End Sub


    Protected Sub gvBusquedaPrincipioActivo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaPrincipioActivo.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim Texto As String
        '    Texto = e.Row.Cells(1).Text().Trim().ToUpper()

        '    If Texto.Contains(Request.Params("Buscar").Trim().ToUpper()) And Request.Params("Buscar").Trim() <> "" Then
        '        e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Buscar").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Buscar").Trim().ToUpper() + "</span></b>")
        '    End If
        'End If
    End Sub
End Class