Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN

Public Class GridExamenCompletado
    Inherits System.Web.UI.Page
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaExamenCompletado()
        End If

    End Sub

    Public Sub ListaExamenCompletado()
        If Not IsNothing(Request.Params("IdReceta")) Then
            hfIdReceta_.Value = Request.Params("IdReceta").Trim()
            oRceImagenesE.CodAtencion = ""
            oRceImagenesE.IdeRecetaCab = hfIdReceta_.Value.Trim()
            oRceImagenesE.IdeImagen = 0
            oRceImagenesE.Orden = 5
            gvExamenCompletado.DataSource = oRceImagenLN.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)
            gvExamenCompletado.DataBind()
        End If

    End Sub

End Class