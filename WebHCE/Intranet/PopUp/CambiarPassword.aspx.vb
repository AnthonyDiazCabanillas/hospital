Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE

Public Class CambiarPassword
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function CambiarPassword(ByVal NuevaPassword As String, ByVal RepetirPassword As String, ByVal AnteriorPassword As String) As String
        Dim pagina As New CambiarPassword
        Dim mensaje As String = pagina.CambiarPassword_(NuevaPassword, RepetirPassword, AnteriorPassword)

        Return mensaje
    End Function


    Public Function CambiarPassword_(ByVal NuevaPassword As String, ByVal RepetirPassword As String, ByVal AnteriorPassword As String) As String
        Try
            If NuevaPassword = "" Then
                Return "Escribir la nueva contraseña."
            End If
            If RepetirPassword = "" Then
                Return "Escribir la verficación de la contraseña."
            End If
            If NuevaPassword.Length < 4 Then
                Return "La nueva contraseña debe tener como minimo 4 caracteres."
            End If
            If NuevaPassword <> RepetirPassword Then
                Return "La Nueva Contraseña y la Contraseña de Verificación no coinciden"
            End If

            oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
            oRceInicioSesionE.Campo = "txt_clave"
            oRceInicioSesionE.Valor = NuevaPassword
            oRceInicioSesionE.Clave = AnteriorPassword
            oRceInicioSesionLN.Sp_RCESegUsuario_Update(oRceInicioSesionE)
            Return oRceInicioSesionE.Mensaje
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function

End Class