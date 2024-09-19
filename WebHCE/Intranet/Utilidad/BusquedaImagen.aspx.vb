Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN

Public Class BusquedaImagen
    Inherits System.Web.UI.Page
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaImagenes()
        End If

    End Sub

    Public Sub ListaImagenes()

        If Request.Params("TipoBusqueda") <> Nothing Then
            If Request.Params("TipoBusqueda") = "G" Then 'G -> busqued general
                divImagenes.Visible = True
                divImagenesFavorito.Visible = False
                oRceImagenesE.Orden = Request.Params("Orden")
                If Request.Params("Nombre") <> Nothing Then
                    oRceImagenesE.Nombre = Request.Params("Nombre").Trim().ToUpper()
                Else
                    oRceImagenesE.Nombre = ""
                End If
                oRceImagenesE.CodMedico = Session(sCodMedico)
                oRceImagenesE.TipoDeAtencion = "A" 'Session(sTipoAtencion) JB - COMENTADO - SE USARA SIEMPRE 'A'
                Dim tabla As New DataTable()
                tabla = oRceImagenLN.Sp_RceBuscar_Consulta(oRceImagenesE)
                gvBusquedaImagen.DataSource = tabla
                gvBusquedaImagen.DataBind()
            Else 'sino busqueda de favoritos
                divImagenes.Visible = False
                divImagenesFavorito.Visible = True
                oRceImagenesE.Orden = Request.Params("Orden")
                If Request.Params("Nombre") <> Nothing Then
                    oRceImagenesE.Nombre = Request.Params("Nombre").Trim().ToUpper()
                Else
                    oRceImagenesE.Nombre = ""
                End If
                oRceImagenesE.CodMedico = Session(sCodMedico)
                oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
                Dim tabla As New DataTable()
                tabla = oRceImagenLN.Sp_RceImagenFavoritoMae_Consulta(oRceImagenesE)
                gvBusquedaImagenFavorito.DataSource = tabla
                gvBusquedaImagenFavorito.DataBind()
            End If

        End If

    End Sub

    Protected Sub gvBusquedaImagen_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaImagen.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Texto As String
            Texto = e.Row.Cells(1).Text().Trim().ToUpper()

            If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
                e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
            End If
        End If
    End Sub

    Protected Sub gvBusquedaImagenFavorito_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaImagenFavorito.RowDataBound
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