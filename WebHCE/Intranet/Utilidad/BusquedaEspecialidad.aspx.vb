Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN

Public Class BusquedaEspecialidad
    Inherits System.Web.UI.Page
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaEspecialidad()
        End If

    End Sub

    Public Sub ListaEspecialidad()
        oInterconsultaE.Orden = 12
        If Request.Params("Nombre") <> Nothing Then
            oInterconsultaE.Nombre = Request.Params("Nombre").Trim().ToUpper()
        Else
            oInterconsultaE.Nombre = ""
        End If
        'INICIO - JB - 07/06/2021
        If Request.Params("Medico") <> Nothing Then
            oInterconsultaE.CodMedico = Request.Params("Medico").Trim().ToUpper()
        Else
            oInterconsultaE.CodMedico = ""
            oInterconsultaE.Orden = 4
        End If
        'FIN - JB - 07/06/2021

        'oInterconsultaE.CodMedico = Session(sCodMedico) 07/06/2021
        oInterconsultaE.TipoDeAtencion = Session(sTipoAtencion)
        Dim tabla As New DataTable()
        tabla = oInterconsultaLN.Sp_RceBuscar_Consulta(oInterconsultaE)
        gvBusquedaEspecialidad.DataSource = tabla
        gvBusquedaEspecialidad.DataBind()
    End Sub

    Protected Sub gvBusquedaEspecialidad_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaEspecialidad.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim Texto As String
        '    Texto = e.Row.Cells(1).Text().Trim().ToUpper()

        '    If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
        '        e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
        '    End If
        'End If
    End Sub
End Class