Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN

Public Class GridImagen
    Inherits System.Web.UI.Page
    Dim nNumeroPagina As Integer
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaImagenes()
        End If
    End Sub


    Public Sub ListaImagenes()
        oRceImagenesE.IdeRecetaCab = Session(sIdeRecetaImagenCab)
        oRceImagenesE.Orden = 2
        Dim tabla As New DataTable()
        If Not IsNothing(Request.Params("Pagina")) Then
            tabla = oRceImagenLN.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)
            Session(sTablaImagenes) = tabla
            gvImagenesSeleccionados.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
            gvImagenesSeleccionados.DataSource = tabla
            gvImagenesSeleccionados.DataBind()
        Else
            tabla = oRceImagenLN.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)
            Session(sTablaImagenes) = tabla
            gvImagenesSeleccionados.DataSource = tabla
            gvImagenesSeleccionados.DataBind()
        End If
        
    End Sub

    ''' <summary>
    ''' FUNCION PARA ELIMINAR LA IMAGEN
    ''' </summary>
    ''' <param name="RecetaDet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarImagen(ByVal RecetaDet As String) As String
        Dim pagina As New GridImagen()
        Return pagina.EliminarImagen_(RecetaDet)
    End Function

    Public Function EliminarImagen_(ByVal RecetaDet As String) As String
        Try
            If RecetaDet.Trim() = "" Then
                Return "Seleccione una Imagen"
            Else
                oRceImagenesE.IdeRecetaDet = RecetaDet
                Dim elimino As Boolean = oRceImagenLN.Sp_RceRecetaImagenDet_Delete(oRceImagenesE)

                If elimino <> True Then
                    Return ConfigurationManager.AppSettings(sMensajeEliminarError).Trim()
                Else
                    Return "OK"
                End If
            End If
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try
        
    End Function


End Class