Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports System.Net

Public Class LoginEnfermera
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Request.Form("hfAuxiliar") <> Nothing Then
                Session(sCodigoAtencion) = Request.Form("hfAuxiliar").Trim()
            End If

        End If

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSesion(ByVal Usuario As String, ByVal clave As String) As String
        Dim pagina As New LoginEnfermera
        Dim inicio As String = pagina.ValidarInicioSesion(Usuario, clave)

        Return inicio
    End Function

    Public Function ValidarInicioSesion(ByVal NombreUsuario As String, ByVal password As String) As String
        Try
            Dim tabla As New DataTable()
            oRceInicioSesionE.CodigoUsuario = NombreUsuario.ToUpper()
            oRceInicioSesionE.Clave = password
            oRceInicioSesionE.Orden = 1

            tabla = oRceInicioSesionLN.Sp_Usuarios_IniciarSesion(oRceInicioSesionE)

            If tabla.Rows.Count > 0 Then
                Session(sCodEnfermera) = tabla.Rows(0)("login").ToString().Trim()
                Return ""
            Else
                Return "Usuario o Clave incorrecta"
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function

End Class