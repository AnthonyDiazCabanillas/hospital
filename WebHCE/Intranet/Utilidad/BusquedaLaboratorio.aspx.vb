Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN

Public Class BusquedaLaboratorio
    Inherits System.Web.UI.Page
    Dim oRceLaboratioE As New RceLaboratioE
    Dim oRceLaboratorioLN As New RceLaboratorioLN

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Response.Redirect("ConsultaPacienteHospitalizado.aspx")
        End If
        If Not Page.IsPostBack Then
            ListaAnalisisLaboratorio()
        End If

    End Sub

    Public Sub ListaAnalisisLaboratorio()

        If Request.Params("TipoBusqueda") <> Nothing Then
            If Request.Params("TipoBusqueda") = "G" Then 'G -> busqued general
                divAnalisis.Visible = True
                divAnalisisFavorito.Visible = False
                oRceLaboratioE.Orden = Request.Params("Orden")
                If Request.Params("Nombre") <> Nothing Then
                    oRceLaboratioE.Nombre = Request.Params("Nombre").Trim().ToUpper()
                Else
                    oRceLaboratioE.Nombre = ""
                End If
                oRceLaboratioE.CodMedico = Session(sCodMedico)
                oRceLaboratioE.TipoDeAtencion = "A" 'Session(sTipoAtencion) se usara tipo "A" - 05/08/2019
                Dim tabla As New DataTable()
                tabla = oRceLaboratorioLN.Sp_RceBuscar_Consulta(oRceLaboratioE)
                gvBusquedaAnalisisLaboratorio.DataSource = tabla
                gvBusquedaAnalisisLaboratorio.DataBind()
            Else 'sino busqueda de favoritos
                divAnalisis.Visible = False
                divAnalisisFavorito.Visible = True
                oRceLaboratioE.Orden = Request.Params("Orden")
                If Request.Params("Nombre") <> Nothing Then
                    oRceLaboratioE.Nombre = Request.Params("Nombre").Trim().ToUpper()
                Else
                    oRceLaboratioE.Nombre = ""
                End If
                oRceLaboratioE.CodMedico = Session(sCodMedico)
                oRceLaboratioE.TipoDeAtencion = "A" 'Session(sTipoAtencion) se usara tipo "A" - 05/08/2019
                Dim tabla As New DataTable()
                tabla = oRceLaboratorioLN.Sp_RceAnalisisFavoritoMae_Consulta(oRceLaboratioE)
                gvBusquedaAnalisisFavorito.DataSource = tabla
                gvBusquedaAnalisisFavorito.DataBind()
            End If

        End If
        
    End Sub

    Protected Sub gvBusquedaAnalisisLaboratorio_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaAnalisisLaboratorio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Texto As String
            Texto = e.Row.Cells(1).Text().Trim().ToUpper()

            If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
                e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
            End If
        End If
    End Sub

    Protected Sub gvBusquedaAnalisisFavorito_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaAnalisisFavorito.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Texto As String
            Texto = e.Row.Cells(1).Text().Trim().ToUpper()

            If Request.Params("Nombre").Trim().ToUpper() <> "" Then
                If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
                    e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
                End If
            End If
        End If
    End Sub
End Class