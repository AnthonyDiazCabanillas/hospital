'----------------------------------------------------------------
'Version    Fecha		    Autor		REQUERIMIENTO			Comentario
'1.0        11/11/2024  	GLLUNCOR	REQ 2024-026424			Restringir acceso a los HC Hospital por médico
'----------------------------------------------------------------

Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports LogicaNegocio.HospitalLN
Imports Entidades.HospitalE
Imports System.Net

Public Class AccesoEnfermera
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Dim oHospitalLN As New HospitalLN()
    Dim oHospitalE As New HospitalE()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Params("Parametro[]") <> Nothing Then
                Session(sCodigoAtencion) = Request.Params("Parametro[]").Split(",")(0).Trim()

                If Request.Params("Parametro[]").Split(",")(1).Trim() <> "" Then
                    Session(sIdeHistoria) = CType(Request.Params("Parametro[]").Split(",")(1).Trim(), Integer)
                Else
                    Session(sIdeHistoria) = 0
                End If
            End If

        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSesion(ByVal Usuario As String, ByVal clave As String) As String
        Dim pagina As New AccesoEnfermera()
        Dim inicio As String = pagina.ValidarInicioSesion(Usuario, clave)

        Return inicio
    End Function

    Public Function ValidarInicioSesion(ByVal NombreUsuario As String, ByVal password As String) As String
        If IsNothing(Session(sCodigoAtencion)) Then 'JB - 09/06/2017
            Return "EXPIRO_ATENCION"
        End If

        Dim nom_cliente, ip_cliente As String
        'INICIO - JB - 25/05/2020 - COMENTADO, EMPEZO A DAR ERROR
        'ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        'If ip_cliente = Nothing Then
        '    ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        'End If
        'Dim sIP_ENTRY As IPHostEntry = Dns.GetHostEntry(ip_cliente)
        'nom_cliente = Convert.ToString(sIP_ENTRY.HostName)
        'FIN - JB - 25/05/2020 - COMENTADO, EMPEZO A DAR ERROR
        ip_cliente = ""
        nom_cliente = ""

        'INICIO - JB - COMENTADO - 11/06/2020
        'Try
        '    Dim tabla As New DataTable()
        '    oRceInicioSesionE.CodigoUsuario = NombreUsuario.ToUpper()
        '    oRceInicioSesionE.Clave = password
        '    oRceInicioSesionE.Orden = 1
        '    oRceInicioSesionE.DscIpPC = ip_cliente
        '    oRceInicioSesionE.DscPcName = nom_cliente
        '    tabla = oRceInicioSesionLN.Sp_Usuarios_IniciarSesion(oRceInicioSesionE)

        '    If tabla.Rows.Count > 0 Then
        '        Session(sCodEnfermera) = tabla.Rows(0)("login").ToString().Trim()
        '        Session(sIdeSesion) = tabla.Rows(0)("id_sesion").ToString().Trim()
        '        Session(sNombreUsuario) = tabla.Rows(0)("nom_usuario").ToString().Trim()
        '        Session(sCodMedico) = 0
        '        Session(sCodUser) = tabla.Rows(0)("coduser").ToString().Trim()

        '        Dim tabla_ As New DataTable()
        '        oHospitalE.CodAtencion = Session(sCodigoAtencion)
        '        tabla_ = oHospitalLN.Sp_RceHistoriaClinica_Consulta(oHospitalE)
        '        Session(sIdeHistoria) = CType(tabla_.Rows(0)("ide_historia").ToString().Trim(), Integer)
        '        Return ""
        '    Else
        '        Return "Usuario o Clave incorrecta"
        '    End If
        'Catch ex As Exception
        '    Return ex.Message.ToString()
        'End Try
        'FIN - JB - COMENTADO - 11/06/2020
        Try
            Dim tabla As New DataTable()
            oRceInicioSesionE.DocIdentidad = ""
            oRceInicioSesionE.CodigoUsuario = NombreUsuario.ToUpper().Trim()
            oRceInicioSesionE.Clave = password
            oRceInicioSesionE.Orden = 1
            '1.0 INI
            oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
            '1.0 FIN
            tabla = oRceInicioSesionLN.Sp_Usuarios_IniciarSesion2(oRceInicioSesionE)

            If tabla.Rows.Count > 0 Then
                Session(sCodEnfermera) = tabla.Rows(0)("login").ToString().Trim()
                Session(ScodigoAccesoLogin) = tabla.Rows(0)("login").ToString().Trim()
                Session(sIdeSesion) = tabla.Rows(0)("ide_sesion").ToString().Trim()
                Session(sNombreUsuario) = tabla.Rows(0)("nom_usuario").ToString().Trim()
                Session(sCodMedico) = 0
                Session(sCodUser) = tabla.Rows(0)("cod_user").ToString().Trim()
                Session(sCodEspecialidad) = tabla.Rows(0)("cod_especialidad").ToString().Trim()
                Dim tabla_ As New DataTable()
                oHospitalE.CodAtencion = Session(sCodigoAtencion)
                tabla_ = oHospitalLN.Sp_RceHistoriaClinica_Consulta(oHospitalE)
                Session(sIdeHistoria) = CType(tabla_.Rows(0)("ide_historia").ToString().Trim(), Integer)

                Session(sPerfilUsuario) = tabla.Rows(0)("txt_perfil").ToString().Trim()
                Return ""
            Else
                Return "Usuario o Clave incorrecta"
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function


End Class