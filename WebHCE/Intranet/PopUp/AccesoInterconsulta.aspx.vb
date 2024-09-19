Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE
Imports System.Net

Imports LogicaNegocio.HospitalLN
Imports Entidades.HospitalE

Public Class AccesoInterconsulta
    Inherits System.Web.UI.Page
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Dim oInterconsultaE As New InterconsultaE
    Dim oInterconsultaLN As New InterconsultaLN
    Dim oMedicoE As New MedicoE
    Dim oMedicoLN As New MedicoLN
    Dim oHospitalLN As New HospitalLN()
    Dim oHospitalE As New HospitalE()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not IsNothing(Request.Params("Parametro[]")) And Request.Params("Parametro[]") <> Nothing Then
                Session(sCodigoAtencion) = Request.Params("Parametro[]").Trim().Split("|")(6)
                Session("TempEsp") = Request.Params("Parametro[]").Trim().Split("|")(7) 'SESSION TEMPORAL PARA VALIDAR LA ESPECIALIDAD
                hfInterconsulta.Value = Request.Params("Parametro[]") 'SESSION TEMPORAL PARA PASAR LOS VALORES A LA PANTALLA DE RESPUESTA INTERCONSULTA
                RegistrarHistoriaClinica(Request.Params("Parametro[]").Trim().Split("|")(6))
            End If
        End If
    End Sub

    Public Function RegistrarHistoriaClinica(ByVal CodigoAtencion As String) As String
        oInterconsultaE.Atencion = CodigoAtencion
        Session(sCodigoAtencion) = CodigoAtencion
        Session(sCodigoAtencion_Auxiliar) = CodigoAtencion '21/06/2016
        oInterconsultaE = oInterconsultaLN.Sp_RceHistoriaClinica_Insert(oInterconsultaE)
        If oInterconsultaE.IdeHistoria <> 0 And oInterconsultaE.IdeHistoria <> Nothing Then
            If (String.IsNullOrWhiteSpace(Session(sIdeHistoria))) Then
                Session(sIdeHistoria) = oInterconsultaE.IdeHistoria
                Return ""
            Else
                Return "Ya existe una sesion abierta. Cierre las otras pestañas y/o actualice la pestaña (F5)"
            End If
        Else
            Return "No se pudo Iniciar el acceso al sistema - Sp_RceHistoriaClinica_Insert"
        End If
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSesion(ByVal Usuario As String, ByVal clave As String) As String
        Dim pagina As New AccesoInterconsulta
        Dim inicio As String = pagina.ValidarInicioSesion(Usuario, clave)

        Return inicio
    End Function

    Public Function ValidarInicioSesion(ByVal NombreUsuario As String, ByVal password As String) As String
        'Obtener Datos de PC Cliente - Jonathan B.
        'INICIO - JB - 22/06/2020 - COMENTADO
        Dim nom_cliente As String = ""
        Dim ip_cliente As String = ""
        Dim mensaje As String = ""
        'ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        'If ip_cliente = Nothing Then
        '    ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        'End If
        'Dim sIP_ENTRY As IPHostEntry = Dns.GetHostEntry(ip_cliente)
        'nom_cliente = Convert.ToString(sIP_ENTRY.HostName)

        'oRceInicioSesionE = New RceInicioSesionE
        'oRceInicioSesionE.DocIdentidad = NombreUsuario
        'oRceInicioSesionE.Clave = password
        'oRceInicioSesionE.DscIpPC = ip_cliente
        'oRceInicioSesionE.DscPcName = nom_cliente
        'oRceInicioSesionE = oRceInicioSesionLN.Sp_RCEAmbulatorio_IniciarSesion(oRceInicioSesionE)
        'FIN - JB - 22/06/2020 - COMENTADO

        Dim tabla As New DataTable()
        oRceInicioSesionE.DocIdentidad = NombreUsuario
        oRceInicioSesionE.CodigoUsuario = ""
        oRceInicioSesionE.Clave = password
        oRceInicioSesionE.Orden = 1
        tabla = oRceInicioSesionLN.Sp_Usuarios_IniciarSesion2(oRceInicioSesionE)

        If tabla.Rows.Count > 0 Then
            If Trim(tabla.Rows(0)("ide_sesion").ToString().Trim()) <> "" Then
                Session(sIdeSesion) = tabla.Rows(0)("ide_sesion").ToString().Trim()

                Session(sNombreUsuario) = tabla.Rows(0)("login").ToString().Trim()
                Session(sCodMedico) = tabla.Rows(0)("cod_medico").ToString().Trim()

                Session(sCodUser) = tabla.Rows(0)("cod_user").ToString().Trim()
                Session(sDscPcName) = nom_cliente

                Session(sPerfilUsuario) = tabla.Rows(0)("txt_perfil").ToString().Trim()

                'validar especialidad Session("TempEsp") -> cod_especialidad
                Dim dt As New DataTable()
                oMedicoE.CodMedico = Session(sCodMedico)
                oMedicoE.Atencion = Session(sCodigoAtencion)
                oMedicoE.Orden = 2
                dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
                Dim especial As Boolean = False

                For index = 0 To dt.Rows.Count - 1
                    'If dt.Rows(index)("codespecialidad").ToString().Trim() <> Session("TempEsp") Then
                    '    mensaje = "Esta interconsulta no corresponde a su especialidad."
                    '    Exit For
                    'End If
                    If dt.Rows(index)("codespecialidad").ToString().Trim() = Session("TempEsp") Then
                        especial = True
                        Exit For
                    End If
                Next
                If especial <> True Then
                    mensaje = "Esta interconsulta no corresponde a su especialidad."
                End If

                Return mensaje


            Else
                Return tabla.Rows(0)("mensaje").ToString().Trim()
            End If
        Else
            Return "El usuario no existe"
        End If

        'INICIO - JB - 22/06/2020 - COMENTADO
        'Dim mensaje As String = ""
        'If Trim(oRceInicioSesionE.IdeSesion) <> "" Then
        '    Session(sIdeSesion) = oRceInicioSesionE.IdeSesion
        '    Session(sNombreUsuario) = oRceInicioSesionE.Login
        '    Session(sCodMedico) = oRceInicioSesionE.CodMedico
        '    Session(sCodUser) = oRceInicioSesionE.CodUser

        '    'validar especialidad Session("TempEsp") -> cod_especialidad
        '    Dim dt As New DataTable()
        '    oMedicoE.CodMedico = Session(sCodMedico)
        '    oMedicoE.Atencion = Session(sCodigoAtencion)
        '    oMedicoE.Orden = 2
        '    dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
        '    Dim especial As Boolean = False

        '    For index = 0 To dt.Rows.Count - 1
        '        'If dt.Rows(index)("codespecialidad").ToString().Trim() <> Session("TempEsp") Then
        '        '    mensaje = "Esta interconsulta no corresponde a su especialidad."
        '        '    Exit For
        '        'End If
        '        If dt.Rows(index)("codespecialidad").ToString().Trim() = Session("TempEsp") Then
        '            especial = True
        '            Exit For
        '        End If
        '    Next
        '    If especial <> True Then
        '        mensaje = "Esta interconsulta no corresponde a su especialidad."
        '    End If

        '    Return mensaje
        'Else
        '    Return oRceInicioSesionE.Mensaje
        'End If
        'FIN - JB - 22/06/2020 - COMENTADO
    End Function


    '30/01/2017
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSesion2(ByVal Usuario As String, ByVal clave As String) As String
        Dim pagina As New AccesoInterconsulta()
        Dim inicio As String = pagina.ValidaSesion2_(Usuario, clave)

        Return inicio
    End Function

    Public Function ValidaSesion2_(ByVal NombreUsuario As String, ByVal password As String) As String
        Dim nom_cliente, ip_cliente As String
        nom_cliente = ""
        ip_cliente = ""
        'ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        'If ip_cliente = Nothing Then
        '    ip_cliente = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        'End If
        'Dim sIP_ENTRY As IPHostEntry = Dns.GetHostEntry(ip_cliente)
        'nom_cliente = Convert.ToString(sIP_ENTRY.HostName)

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

        '        'INICIO - 30/01/2017 - JB
        '        Dim mensaje As String = ""
        '        Dim dt As New DataTable()
        '        oMedicoE.CodMedico = Session(sCodMedico)
        '        oMedicoE.Atencion = Session(sCodigoAtencion)
        '        oMedicoE.Orden = 2
        '        dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
        '        Dim especial As Boolean = False

        '        For index = 0 To dt.Rows.Count - 1
        '            If dt.Rows(index)("codespecialidad").ToString().Trim() = Session("TempEsp") Then
        '                especial = True
        '                Exit For
        '            End If
        '        Next
        '        If especial <> True Then
        '            mensaje = "Esta interconsulta no corresponde a su especialidad."
        '        End If
        '        If mensaje <> "" Then
        '            Return mensaje
        '        End If
        '        'FIN - 30/01/2017 - JB

        '        Return ""
        '    Else
        '        Return "Usuario o Clave incorrecta"
        '    End If
        'Catch ex As Exception
        '    Return ex.Message.ToString()
        'End Try

        Try
            Dim tabla As New DataTable()
            oRceInicioSesionE.DocIdentidad = ""
            oRceInicioSesionE.CodigoUsuario = NombreUsuario.ToUpper().Trim()
            oRceInicioSesionE.Clave = password
            oRceInicioSesionE.Orden = 1
            tabla = oRceInicioSesionLN.Sp_Usuarios_IniciarSesion2(oRceInicioSesionE)

            If tabla.Rows.Count > 0 Then
                Session(sCodEnfermera) = tabla.Rows(0)("login").ToString().Trim()
                Session(sIdeSesion) = tabla.Rows(0)("ide_sesion").ToString().Trim()
                Session(sNombreUsuario) = tabla.Rows(0)("nom_usuario").ToString().Trim()
                Session(sCodMedico) = 0
                Session(sCodUser) = tabla.Rows(0)("cod_user").ToString().Trim()

                Dim tabla_ As New DataTable()
                oHospitalE.CodAtencion = Session(sCodigoAtencion)
                tabla_ = oHospitalLN.Sp_RceHistoriaClinica_Consulta(oHospitalE)
                Session(sIdeHistoria) = CType(tabla_.Rows(0)("ide_historia").ToString().Trim(), Integer)

                Session(sPerfilUsuario) = tabla.Rows(0)("txt_perfil").ToString().Trim()


                'INICIO - 30/01/2017 - JB
                Dim mensaje As String = ""
                Dim dt As New DataTable()
                oMedicoE.CodMedico = Session(sCodMedico)
                oMedicoE.Atencion = Session(sCodigoAtencion)
                oMedicoE.Orden = 2
                dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
                Dim especial As Boolean = False

                For index = 0 To dt.Rows.Count - 1
                    If dt.Rows(index)("codespecialidad").ToString().Trim() = Session("TempEsp") Then
                        especial = True
                        Exit For
                    End If
                Next
                If especial <> True Then
                    mensaje = "Esta interconsulta no corresponde a su especialidad."
                End If
                If mensaje <> "" Then
                    Return mensaje
                End If
                'FIN - 30/01/2017 - JB


                Return ""
            Else
                Return "Usuario o Clave incorrecta"
            End If
        Catch ex As Exception

        End Try

    End Function
    'FIN - 30/01/2017 - JB
End Class