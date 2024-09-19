Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN

Public Class BusquedaMedico
    Inherits System.Web.UI.Page
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaMedico()
        End If
    End Sub


    Public Sub ListaMedico()
        oInterconsultaE.Orden = 5
        If Request.Params("Nombre") <> Nothing Then
            oInterconsultaE.Nombre = Request.Params("Nombre").Trim().ToUpper()
        Else
            oInterconsultaE.Nombre = ""
        End If
        oInterconsultaE.CodMedico = Session(sCodMedico)
        oInterconsultaE.TipoDeAtencion = Session(sTipoAtencion)
        'INICIO - JB - 28/05/2021
        If Request.Params("Especialidad") <> Nothing Then
            oInterconsultaE.Orden = 11
            oInterconsultaE.Atencion = Request.Params("Especialidad").Trim().ToUpper()
        Else
            oInterconsultaE.Atencion = ""
        End If
        'FIN - JB - 28/05/2021
        Dim tabla As New DataTable()
        tabla = oInterconsultaLN.Sp_RceBuscar_Consulta(oInterconsultaE)
        gvBusquedaMedico.DataSource = tabla
        gvBusquedaMedico.DataBind()
    End Sub

    Protected Sub gvBusquedaMedico_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaMedico.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim Texto As String
        '    Texto = e.Row.Cells(1).Text().Trim().ToUpper()

        '    If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
        '        e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
        '    End If
        'End If
    End Sub
End Class