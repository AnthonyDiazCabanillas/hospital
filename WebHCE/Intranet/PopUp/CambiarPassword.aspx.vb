Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports Entidades
Imports Newtonsoft.Json '1.1
Imports System.Web.Services
Imports RestSharp '1.1
Public Class CambiarPassword
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Dim _reporteApi As RespuestaArchivoJsonE '1.1
    Private dtParam As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.hfCambiarPass.Value = Session(sClave)

            If (Session(sCambioClave) = "1") Then
                Me.lblMsjCambioPassword.Text = "Su contraseña ha expirado!. Ingrese una nueva contraseña ...!"
            Else
                Me.lblMsjCambioPassword.Text = ""
            End If

            Sp_ParamSeguridad_SelData()

        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function CambiarPassword(ByVal NuevaPassword As String, ByVal RepetirPassword As String, ByVal AnteriorPassword As String) As String
        Dim pagina As New CambiarPassword
        Dim mensaje As String = pagina.CambiarPassword_(NuevaPassword, RepetirPassword, AnteriorPassword)

        Return mensaje
    End Function

    '1.1 INI
    Private Function ObtenerLstadoValidadorPassword(ByVal ppassword1 As String, ByVal ppassword2 As String)
        'Dim _rutaApi As String
        'Dim dtRuta As New DataTable

        'dtRuta = oRceInicioSesionLN.Sp_RCEAmbulatorio_ObtenerRutaApiPassword()
        '_rutaApi = dtRuta(0)("nombre").ToString()
        Dim _rutaApi As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

        Dim _resultado As New RespuestaArchivoJsonE()
        Try
            Dim rutaFinal As String
            rutaFinal = _rutaApi & "SeguridadDePassword/Api/Clinica/ValidarPassword"
            Dim _cliente As RestClient = New RestClient(rutaFinal)
            Dim _request As RestRequest = New RestRequest()
            _request.Method = Method.Get
            _request.AddParameter("password1", ppassword1)
            _request.AddParameter("password2", ppassword2)
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _resultado = JsonConvert.DeserializeObject(Of RespuestaArchivoJsonE)(_Response.Content)


            Return _resultado
        Catch
            Return _resultado
        End Try

    End Function
    '1.1 FIN


    <WebMethod>
    Public Shared Function Sp_ParamSeguridad_SelData() As String
        Dim JSONresult As String
        Dim dt As New DataTable
        Try

            Dim oRceInicioSesionLN2 As New RceInicioSesionLN()
            dt = oRceInicioSesionLN2.Sp_ParamSeguridad_Sel()

            JSONresult = JsonConvert.SerializeObject(dt)

            Return JSONresult
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    Public Function CambiarPassword_(ByVal NuevaPassword As String, ByVal RepetirPassword As String, ByVal AnteriorPassword As String) As String
        Try

            If (Me.Session(sPerfilUsuario) = "ADMINISTRATIVOS") Then
                Return "No tiene acceso para cambiar contraseña"
            End If

            If NuevaPassword = "" Then
                Return "Escribir la nueva contraseña."
            End If
            If RepetirPassword = "" Then
                Return "Escribir la verficación de la contraseña."
            End If

            If NuevaPassword <> RepetirPassword Then
                Return "La Nueva Contraseña y la Contraseña de Verificación no coinciden"
            End If

            '1.1 INI
            Dim oRceInicioSesionE As New RceInicioSesionE
            oRceInicioSesionE.Ide_Usuario = Session(sCodUser)
            oRceInicioSesionE.Txtclave = NuevaPassword
            oRceInicioSesionE.TipoSistema = "003"
            Dim dt As New DataTable
            dt = oRceInicioSesionLN.sp_seglogclave_sel_clinica(oRceInicioSesionE)


            If (dt.Rows.Count > 0) Then
                Return "La nueva contraseña ya ha sido usada anteriormente ...! Ingrese otra contraseña diferente ...!"
            End If

            _reporteApi = ObtenerLstadoValidadorPassword(NuevaPassword, RepetirPassword)

            If (_reporteApi.exito = False) Then
                Return Mid(_reporteApi.message, 4, _reporteApi.message.ToString().Length)
            End If
            '1.1 FIN

            oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
            oRceInicioSesionE.Campo = "txt_clave"
            oRceInicioSesionE.Valor = NuevaPassword
            oRceInicioSesionE.Clave = AnteriorPassword
            oRceInicioSesionE.TipoSistema = "003"
            oRceInicioSesionLN.Sp_RCESegUsuario_Update(oRceInicioSesionE)
            Session(sClave) = RepetirPassword
            Session(sCambioClave) = "0"
            Return oRceInicioSesionE.Mensaje
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function

End Class