'----------------------------------------------------------------
'Version    Fecha		    Autor		REQUERIMIENTO			Comentario
'1.0        11/11/2024  	GLLUNCOR	REQ 2024-026424			Restringir acceso a los HC Hospital por médico
'----------------------------------------------------------------

Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN
Imports System.Net
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN
Imports LogicaNegocio.HospitalLN
Imports Entidades.HospitalE

Public Class Acceso
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Not IsNothing(Request.Params("Parametro[]")) Then '06/02/2017
                hfParametro.Value = Request.Params("Parametro[]")
            End If
            'JB - 23/12
            'oTablasE.CodTabla = "RCE_LOCAL"
            'oTablasE.Orden = 5
            'Dim dt As New DataTable()
            'dt = oTablasLN.Sp_Tablas_Consulta(oTablasE)
            'ddlSede.DataSource = dt
            'ddlSede.DataTextField = "nombre"
            'ddlSede.DataValueField = "codigo"
            'ddlSede.DataBind()
        End If
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSesion(ByVal Usuario As String, ByVal clave As String, ByVal sede As String) As String
        Dim pagina As New Acceso
        Dim inicio As String = pagina.ValidarInicioSesion(Usuario, clave, sede)

        Return inicio
    End Function

    Public Function ValidarInicioSesion(ByVal NombreUsuario As String, ByVal password As String, ByVal sede As String) As String
        Try
            If IsNothing(Session(sCodigoAtencion)) Then 'JB - 09/06/2017
                Return "EXPIRO_ATENCION"
            End If

            'Obtener Datos de PC Cliente - Jonathan B.
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
            'Session(sSede) = sede
            'oRceInicioSesionE = New RceInicioSesionE
            'oRceInicioSesionE.DocIdentidad = NombreUsuario
            'oRceInicioSesionE.Clave = password
            'oRceInicioSesionE.DscIpPC = ip_cliente
            'oRceInicioSesionE.DscPcName = nom_cliente
            'oRceInicioSesionE.Sede = sede
            'oRceInicioSesionE = oRceInicioSesionLN.Sp_RCEAmbulatorio_IniciarSesion(oRceInicioSesionE)

            'If Trim(oRceInicioSesionE.IdeSesion) <> "" Then
            '    Session(sIdeSesion) = oRceInicioSesionE.IdeSesion

            '    Session(sNombreUsuario) = oRceInicioSesionE.Login
            '    Session(sCodMedico) = oRceInicioSesionE.CodMedico

            '    Session(sCodUser) = oRceInicioSesionE.CodUser
            '    Session(sDscPcName) = nom_cliente
            '    Return ""
            'Else
            '    Return oRceInicioSesionE.Mensaje
            'End If
            'FIN - JB - COMENTADO - 11/06/2020

            Dim tabla As New DataTable()
            oRceInicioSesionE.DocIdentidad = NombreUsuario
            oRceInicioSesionE.CodigoUsuario = ""
            oRceInicioSesionE.Clave = password
            '1.0 INI
            oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
            '1.0 FIN
            oRceInicioSesionE.Orden = 1
            tabla = oRceInicioSesionLN.Sp_Usuarios_IniciarSesion2(oRceInicioSesionE)

            If Not IsNothing(Session(sCodMedico)) Or Not IsNothing(Session(sCodEnfermera)) Then
                Return "Ya existe una sesion abierta. Cierre las otras pestañas y/o actualice la pestaña (F5)"
            End If

            If tabla.Rows.Count > 0 Then
                If Trim(tabla.Rows(0)("ide_sesion").ToString().Trim()) <> "" Then
                    'INICIO - JB - 24/06/2020 - SE COMENTA ESTE CODIGO
                    'Session(sIdeSesion) = tabla.Rows(0)("ide_sesion").ToString().Trim()

                    'Session(sNombreUsuario) = tabla.Rows(0)("login").ToString().Trim()
                    'Session(sCodMedico) = tabla.Rows(0)("cod_medico").ToString().Trim()

                    'Session(sCodUser) = tabla.Rows(0)("cod_user").ToString().Trim()
                    'Session(sDscPcName) = nom_cliente

                    'Session(sPerfilUsuario) = tabla.Rows(0)("txt_perfil").ToString().Trim()
                    'Return ""
                    'FIN - JB - 24/06/2020 - SE COMENTA ESTE CODIGO

                    'INICIO - JB - 24/06/2020 - NUEVO CODIGO
                    Session(sIdeSesion) = tabla.Rows(0)("ide_sesion").ToString().Trim()
                    Session(sNombreUsuario) = tabla.Rows(0)("txt_usuario").ToString().Trim()
                    Session(ScodigoAccesoLogin) = tabla.Rows(0)("cod_usuario").ToString().Trim()
                    Session(sCodMedico) = tabla.Rows(0)("cod_medico").ToString().Trim()
                    Session(sCodUser) = tabla.Rows(0)("cod_user").ToString().Trim()
                    Session(sDscPcName) = nom_cliente
                    Session(sPerfilUsuario) = tabla.Rows(0)("txt_perfil").ToString().Trim()
                    Session(sCodEspecialidad) = tabla.Rows(0)("cod_especialidad").ToString().Trim()

                    Dim oHospitalLN As New HospitalLN
                    Dim oHospitalE As New HospitalE
                    Dim dt, dt2 As New DataTable()
                    oHospitalE.NombrePaciente = Session(sCodigoAtencion)
                    oHospitalE.Pabellon = ""
                    oHospitalE.Servicio = ""
                    oHospitalE.Estado = ""
                    oHospitalE.Orden = 3
                    dt = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE) 'CONSULTANDO DATOS DE LA ATENCION
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0)("activo").ToString().Trim() = "N" Or dt.Rows(0)("fechaaltamedica").ToString().Trim() <> "" Then 'VERIFICANDO SI ESTA DADO DE ALTA O NO ACTIVA

                            If Session(sPerfilUsuario) = "MEDICOS" And dt.Rows(0)("fechaaltamedica").ToString().Trim() <> "" Then 'SI ES MEDICO MOSTRARA MENSAJE Y SE BLOQUEA ACCESO
                                Return "El paciente se encuentra de alta"
                            End If
                            If Session(sPerfilUsuario) = "MEDICOS" And dt.Rows(0)("activo").ToString().Trim() = "N" Then 'SI ES MEDICO MOSTRARA MENSAJE Y SE BLOQUEA ACCESO
                                Return "Esta atencion no esta activa"
                            End If

                        End If
                    End If
                    Return ""
                    'FIN - JB - 24/06/2020 - NUEVO CODIGO
                Else
                    Return tabla.Rows(0)("mensaje").ToString().Trim()
                End If
            Else
                Return "El usuario no existe"
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function

End Class