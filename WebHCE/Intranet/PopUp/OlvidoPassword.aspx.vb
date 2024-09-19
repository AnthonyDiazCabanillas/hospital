Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE

Public Class OlvidoPassword
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function RecuperaPassword(ByVal Correo As String) As String
        Dim pagina As New OlvidoPassword
        Dim mensaje As String = pagina.RecuperaPassword_(Correo)

        Return mensaje
    End Function

    Public Function RecuperaPassword_(ByVal Correo As String) As String
        Try
            If Correo.Trim() = "" Then
                Return "Ingresar el correo."
            End If
            oRceInicioSesionE.Login = Correo
            oRceInicioSesionLN.Sp_RCEAmbulatorio_ObtenerClave(oRceInicioSesionE)
            Return oRceInicioSesionE.Mensaje.ToString().Trim().Replace("\n", " ")
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
        

    End Function

End Class