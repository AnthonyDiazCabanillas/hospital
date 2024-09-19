Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN

Public Class ExamenCompletado
    Inherits System.Web.UI.Page
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Request.Params("Parametro[]") Then
                hfIdReceta.Value = Request.Params("Parametro[]").Trim().Split(",")(0)
            End If
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ExamenCompletado(ByVal IdReceta As String) As String
        Dim pagina As New ExamenCompletado()
        Return pagina.ExamenCompletado_(IdReceta)
    End Function

    Public Function ExamenCompletado_(ByVal IdReceta As String) As String
        Try
            oRceImagenesE.IdeRecetaDet = IdReceta
            oRceImagenesE.Campo = "flg_revisado"
            oRceImagenesE.ValorNuevo = "1"
            oRceImagenLN.Sp_RceRecetaImagenDet_Update(oRceImagenesE)

            Return "OK"

        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function


    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSession() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaSession_()
    End Function

    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidaSession_() As String
        If IsNothing(Session(sCodMedico)) Then
            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function

End Class