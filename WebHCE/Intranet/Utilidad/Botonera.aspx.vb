Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Public Class Botonera
    Inherits System.Web.UI.Page

    Dim oHospitalLN As New HospitalLN()
    Dim oHospitalE As New HospitalE()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


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
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function Validacion(ByVal Tipo As String) As String
        Dim pagina As New Botonera()
        Return pagina.Validacion_(Tipo)
    End Function


    Public Function Validacion_(ByVal Tipo As String) As String
        oHospitalE.TipoValidacion = Tipo
        oHospitalE.CodUser = Session(sCodUser)
        oHospitalE.IdeHistoria = Session(sIdeHistoria)
        Dim tabla As New DataTable()
        tabla = oHospitalLN.Sp_RceValidacion(oHospitalE)
        Dim mensaje = ""

        If tabla.Rows.Count > 0 Then
            If tabla.Rows(0)("error").ToString().Trim() <> "0" Then
                mensaje = tabla.Rows(0)("dsc_mensaje").ToString().Trim()
            End If
        End If

        Return mensaje
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarAlertas() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarAlertas_()
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAlta() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaAlta_()
    End Function




    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerUrlRoe() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ObtenerUrlRoe_()
    End Function


    Public Function ObtenerUrlRoe_() As String
        Try
            Dim wLink As String = ""
            Dim CodigoAtencion2 As String = ""
            Dim oHospitalERoe As New HospitalE()
            Dim oHospitalLNRoe As New HospitalLN()
            Dim oTablasE As New TablasE()
            Dim oTablasLN As New TablasLN()
            Dim tabla1, tabla2, tabla3 As New DataTable()
            oHospitalERoe.CodAtencion = Session(sCodigoAtencion)
            oHospitalERoe.Orden = 1
            tabla1 = oHospitalLNRoe.Sp_Hospital_Traslado_Consulta(oHospitalERoe)

            If tabla1.Rows.Count > 0 Then
                CodigoAtencion2 = tabla1.Rows(0)("codatencionorigen")
            End If

            If CodigoAtencion2 = "" Then 'SIN TRASLADO
                oTablasE.CodTabla = "ROELINK"
                oTablasE.Buscar = "01"
                oTablasE.Key = 50
                oTablasE.NumeroLineas = 1
                oTablasE.Orden = -1
                tabla2 = oTablasLN.Sp_Tablas_Consulta(oTablasE)

                If tabla2.Rows.Count > 0 Then
                    wLink = Replace(Trim(tabla2.Rows(0)("nombre").ToString().Trim() & ""), "%codatencion%", Session(sCodigoAtencion), 1)
                    'If Not wAdo.EOF Then
                    '    wLink = Replace(Trim(wAdo!Nombre & ""), "%codatencion%", wCodAtencion, 1)
                    '    ' <I.CMEDRANO> 09/05/2019
                    '    wRuta = cRuta & "browser.exe " & wLink & ",0"
                    '    Shell(wRuta, vbMaximizedFocus)
                    '    ' Shell "C:\Archivos de programa\InternetExplorer\iexplore.exe " & wLink
                    '    ' <F.CMEDRANO> 09/05/2019
                    'End If
                End If
            Else 'CON TRASLADO
                oTablasE.CodTabla = "ROELINK"
                oTablasE.Buscar = "02"
                oTablasE.Key = 50
                oTablasE.NumeroLineas = 1
                oTablasE.Orden = -1
                tabla3 = oTablasLN.Sp_Tablas_Consulta(oTablasE)

                If tabla3.Rows.Count > 0 Then
                    wLink = Replace(Trim(tabla3.Rows(0)("nombre").ToString().Trim() & ""), "%a1%", Session(sCodigoAtencion), 1)
                    wLink = Replace(Trim(wLink), "%a2%", CodigoAtencion2, 1)

                    'If Not wAdo.EOF Then
                    '    wLink = Replace(Trim(wAdo!Nombre & ""), "%a1%", wCodAtencion, 1)
                    '    wLink = Replace(Trim(wLink), "%a2%", wCodAtencion2, 1)
                    '    ' <I.CMEDRANO> 09/05/2019
                    '    wRuta = cRuta & "browser.exe " & wLink & ",0"
                    '    Shell(wRuta, vbMaximizedFocus)
                    '    ' Shell "C:\Archivos de programa\Internet Explorer\iexplore.exe " & wLink
                    '    ' <F.CMEDRANO> 09/05/2019
                    'End If
                End If

            End If


            Return "OK*" + wLink
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function

End Class