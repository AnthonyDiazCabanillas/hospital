Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports System.Net

Public Class _Default
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Form("hfInterconsulta") <> Nothing Then 'PARAMETRO QUE SOLO SE RECIBIRA DEL POPUP DE INTERCONSULTA
                hfInterconsulta.Value = Request.Form("hfInterconsulta").Trim()
                Session(sCodigoAtencion) = Request.Form("hfCodigoAtencion_Interconsulta").Trim()
            End If
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSesion(ByVal Usuario As String, ByVal clave As String) As String
        Dim pagina As New _Default
        Dim inicio As String = pagina.ValidarInicioSesion(Usuario, clave)

        Return inicio
    End Function

    Public Function ValidarInicioSesion(ByVal NombreUsuario As String, ByVal password As String) As String
        'Obtener Datos de PC Cliente - Jonathan B.
        Dim nom_cliente, ip_cliente As String
        ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If ip_cliente = Nothing Then
            ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        End If
        Dim sIP_ENTRY As IPHostEntry = Dns.GetHostEntry(ip_cliente)
        nom_cliente = Convert.ToString(sIP_ENTRY.HostName)

        oRceInicioSesionE = New RceInicioSesionE
        oRceInicioSesionE.DocIdentidad = NombreUsuario
        oRceInicioSesionE.Clave = password
        oRceInicioSesionE.DscIpPC = ip_cliente
        oRceInicioSesionE.DscPcName = nom_cliente
        oRceInicioSesionE = oRceInicioSesionLN.Sp_RCEAmbulatorio_IniciarSesion(oRceInicioSesionE)

        If Trim(oRceInicioSesionE.IdeSesion) <> "" Then
            Session(sIdeSesion) = oRceInicioSesionE.IdeSesion

            Session(sNombreUsuario) = oRceInicioSesionE.Login
            Session(sCodMedico) = oRceInicioSesionE.CodMedico

            Session(sCodUser) = oRceInicioSesionE.CodUser
            Return ""
        Else
            Return oRceInicioSesionE.Mensaje
        End If

    End Function

End Class