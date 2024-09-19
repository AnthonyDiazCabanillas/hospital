Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN

Public Class BusquedaDiagnostico
    Inherits System.Web.UI.Page
    Dim oRceDiagnosticoE As New RceDiagnosticoE()
    Dim oRceDiagnosticoLN As New RceDiagnosticoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        If Not Page.IsPostBack Then
            ListaDiagnosticos()
        End If

    End Sub

    Public Sub ListaDiagnosticos()
        If Request.Params("TipoBusqueda") <> Nothing Then
            If Request.Params("TipoBusqueda") = "G" Then 'G -> busqued general
                divDiagnostico.Visible = True
                divDiagnosticoFavorito.Visible = False
                oRceDiagnosticoE.Orden = Request.Params("Orden")
                If Request.Params("Nombre") <> Nothing Then
                    oRceDiagnosticoE.Nombre = Request.Params("Nombre").Trim().ToUpper()
                Else
                    oRceDiagnosticoE.Nombre = ""
                End If
                oRceDiagnosticoE.CodMedico = Session(sCodMedico)
                oRceDiagnosticoE.TipoDeAtencion = Session(sTipoAtencion)
                Dim tabla As New DataTable()
                tabla = oRceDiagnosticoLN.Sp_RceBuscar_Consulta(oRceDiagnosticoE)
                gvBusquedaDiagnostico.DataSource = tabla
                gvBusquedaDiagnostico.DataBind()
            Else
                divDiagnostico.Visible = False
                divDiagnosticoFavorito.Visible = True
                oRceDiagnosticoE.Orden = Request.Params("Orden")
                If Request.Params("Nombre") <> Nothing Then
                    oRceDiagnosticoE.Nombre = Request.Params("Nombre").Trim().ToUpper()
                Else
                    oRceDiagnosticoE.Nombre = ""
                End If
                oRceDiagnosticoE.CodMedico = Session(sCodMedico)
                oRceDiagnosticoE.TipoDeAtencion = Session(sTipoAtencion)
                Dim tabla As New DataTable()
                tabla = oRceDiagnosticoLN.Sp_RceDiagnosticoFavoritoMae_Consulta(oRceDiagnosticoE)
                gvBusquedaDiagnosticoFavorito.DataSource = tabla
                gvBusquedaDiagnosticoFavorito.DataBind()
            End If
        End If
    End Sub

    Protected Sub gvBusquedaDiagnostico_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaDiagnostico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Texto As String
            Texto = e.Row.Cells(1).Text().Trim().ToUpper()

            If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
                e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
            End If
        End If
        
    End Sub

    Protected Sub gvBusquedaDiagnosticoFavorito_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaDiagnosticoFavorito.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Texto As String
            Texto = e.Row.Cells(1).Text().Trim().ToUpper()

            If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then

                If Request.Params("Nombre") <> "" And Not IsNothing(Request.Params("Nombre")) Then
                    e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
                End If


            End If
        End If
    End Sub
End Class