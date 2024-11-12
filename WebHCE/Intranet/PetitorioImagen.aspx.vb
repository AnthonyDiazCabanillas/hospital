Imports System.Data
Imports System.IO
Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN
Imports Entidades.EvolucionE 'TMACASSI 25/10/2016
Imports LogicaNegocio.EvolucionLN 'TMACASSI 25/10/2016
Imports System.Data.SqlClient
Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports LogicaNegocio.DiagnosticoLN
Imports Entidades.DiagnosticoE

Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Imports System.Web.Hosting
Imports System.Web.Configuration
Imports System.Drawing.Imaging
Imports System.Drawing

Imports Spire.Pdf

Public Class PetitorioImagen
    Inherits System.Web.UI.Page

    Dim oRceEvolucionE As New RceEvolucionE() 'TMACASSI 25/10/2016
    Dim oRceEvolucionLN As New RceEvolucionLN() 'TMACASSI 25/10/2016
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Response.Redirect("ConsultaPacienteHospitalizado.aspx")
        End If

        If Not Page.IsPostBack Then
            CargaPetitorioImagen2()
        End If

    End Sub

    Public Sub CargaPetitorioImagen()
        Dim tabs As String = ""
        Dim estructura As String = ""
        Dim tabla1, tabla2 As New DataTable()
        Dim ContadorFila As Integer = 0

        Dim CantidadColumna As Integer = 30 'TMACASSI 05/09/2016
        Dim wTabla As Integer = 280         'TMACASSI 05/09/2016

        tabs += "<div class='JSBTABS'>"
        tabs += "<label for='chkTABS' class='JSBMOSTRAR_TABS'></label>"
        tabs += "<input type='checkbox' id='chkTABS' class='chkTAB-CHECK' />"
        tabs += "<ul>"

        oRceImagenesE.Orden = 4
        oRceImagenesE.TipoDeAtencion = "A" 'Session(sTipoAtencion) JB - SE USARA VALOR 'A' PARA MOSTRAR EL PETITORIO - 02/09/2019
        tabla1 = oRceImagenLN.Sp_RceImagenMae_ConsultaV2(oRceImagenesE)

        For index = 0 To tabla1.Rows.Count - 1
            estructura += "<div class='JCUERPO' style='float:none;font-size:0.88em;'>"
            estructura += "<div class='JFILA JDIVCHECK' style='overflow:auto;width:1234px;'>"
            estructura += "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA

            oRceImagenesE.IdeImagenTitulo = tabla1.Rows(index)("ide_imagen_titulo").ToString().Trim()
            oRceImagenesE.Orden = 5
            oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
            tabla2 = oRceImagenLN.Sp_RceImagenMae_ConsultaV2(oRceImagenesE)


            For index1 = 0 To tabla2.Rows.Count - 1
                If tabla2.Rows(index1)("ide_imagen").ToString().Trim() <> tabla2.Rows(index1)("ide_imagen_titulo").ToString().Trim() Then 'se empieza a contar las filas y no el tab
                    ContadorFila += 1
                End If

                If tabla2.Rows(index1)("ide_imagen").ToString().Trim() = tabla2.Rows(index1)("ide_imagen_titulo").ToString().Trim() Then
                    If index = 0 Then
                        tabs += "<li class='JSBTAB_ACTIVO'>"
                    Else
                        tabs += "<li>"
                    End If
                    tabs += "<a>" + tabla2.Rows(index1)("dsc_imagen").ToString().Trim() + "</a>"
                    tabs += "</li>"
                ElseIf tabla2.Rows(index1)("ide_imagen").ToString().Trim() = tabla2.Rows(index1)("ide_imagen_padre").ToString().Trim() Then
                    estructura += "<div class='JFILA'>"
                    estructura += "<div class='JCELDAPETITORIO-12'>"
                    estructura += "<span class='JETIQUETA_5'>" + tabla2.Rows(index1)("dsc_imagen").ToString().Trim() + "</span>"
                    estructura += "</div>"
                    estructura += "</div>"
                Else
                    estructura += "<div class='JFILA'>"
                    estructura += "<div class='JCELDAPETITORIO-12'>"
                    estructura += "<input type='checkbox' id='chkImagen_" + tabla2.Rows(index1)("ide_imagen").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_imagen").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_imagen").ToString().Trim() + "</span>"

                    'If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                    '    estructura += "<div style='float:right;overflow:hidden;'>"
                    'End If
                    'If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Then
                    '    estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis_sub1").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis_sub1").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() + "</span>"
                    'End If
                    'If tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Then
                    '    estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis_sub2").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis_sub2").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() + "</span>"
                    'End If
                    'If tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                    '    estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis_sub3").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis_sub3").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() + "</span>"
                    'End If
                    'If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                    '    estructura += "</div>"
                    'End If

                    estructura += "</div>"
                    estructura += "</div>"
                End If

                If ContadorFila = CantidadColumna Then '25 TMACASSI 07/09/2016
                    estructura += "</div>" 'CIERRA COLUMNA
                    estructura += "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA
                    ContadorFila = 0
                End If

                If ContadorFila <> CantidadColumna And index1 = tabla2.Rows.Count - 1 Then
                    estructura += "</div>" 'CIERRA COLUMNA
                End If
            Next
            ContadorFila = 0
            estructura = estructura.Replace("width:1234", ("width:" + ((Math.Ceiling(tabla2.Rows.Count / CantidadColumna) * wTabla) + 20).ToString()))
            estructura += "</div>"
            estructura += "</div>"
        Next
        tabs += "</ul>"
        tabs += "</div>"

        divContenedorPetitorio.InnerHtml = tabs + estructura
    End Sub


    Public Sub CargaPetitorioImagen2()
        Dim tabs As String = ""
        Dim estructura As String = ""
        Dim tabla1, tabla2 As New DataTable()
        Dim ContadorFila As Integer = 0
        Dim CantidadColumna As Integer = 15  'cantidad de registros por columna
        Dim wTabla As Integer = 280

        oRceImagenesE.Orden = 6
        oRceImagenesE.TipoDeAtencion = "A"
        tabla1 = oRceImagenLN.Sp_RceImagenMae_ConsultaV2(oRceImagenesE)
        '<div class="JTABS" style="width:100%;">                
        '        <input type="radio" id="TabPrincipalNro1" name="TabPrincipal" class="JCHEK-TABS" />
        '        <label for="TabPrincipalNro1" class="JTABS-LABEL">Tab 1</label>

        '        <input type="radio" id="TabPrincipalNro2" name="TabPrincipal" class="JCHEK-TABS" />        
        '        <label for="TabPrincipalNro2" class="JTABS-LABEL">Tab 2</label>

        '        <input type="radio" id="TabPrincipalNro3" name="TabPrincipal" class="JCHEK-TABS" />
        '        <label for="TabPrincipalNro3" class="JTABS-LABEL">Tab 3</label>

        '        <input type="radio" id="TabPrincipalNro4" name="TabPrincipal" class="JCHEK-TABS" />
        '        <label for="TabPrincipalNro4" class="JTABS-LABEL">Tab 4</label>

        '        <div class="JCONTENIDO-TAB">
        '            AAA
        '        </div>
        '        <div class="JCONTENIDO-TAB">
        '            BBB
        '        </div>
        '        <div class="JCONTENIDO-TAB">
        '            CCC
        '        </div>
        '        <div class="JCONTENIDO-TAB">
        '            DDDDDDDDDDDDD
        '        </div>
        '    </div>
        '</div>
        Dim CadenaHTML As String = ""
        Dim CadenaTextAreaHTML As String = ""
        CadenaHTML = CadenaHTML + "<div class='JTABS' style='width:100%;'>"

        For index = 0 To tabla1.Rows.Count - 1
            CadenaHTML = CadenaHTML + "<input type='radio' id='" + tabla1.Rows(index)("ide_imagen").ToString().Trim() + "' name='TabPrincipal' class='JCHEK-TABS' />"
            CadenaHTML = CadenaHTML + "<label for='" + tabla1.Rows(index)("ide_imagen").ToString().Trim() + "' class='JTABS-LABEL'>" + tabla1.Rows(index)("dsc_imagen").ToString().Trim() + "</label>"
            CadenaTextAreaHTML = CadenaTextAreaHTML + "<textarea id='TxtObservacion-" + tabla1.Rows(index)("ide_imagen").ToString().Trim() + "' cols='1' rows='5' class='JTEXTO' style='display:none'></textarea>"
        Next
        divObservacion.InnerHtml = CadenaTextAreaHTML

        For index = 0 To tabla1.Rows.Count - 1
            oRceImagenesE.IdeImagenTitulo = tabla1.Rows(index)("ide_imagen_titulo").ToString().Trim()
            oRceImagenesE.Orden = 5
            oRceImagenesE.TipoDeAtencion = "A"
            tabla2 = oRceImagenLN.Sp_RceImagenMae_ConsultaV2(oRceImagenesE)

            CadenaHTML = CadenaHTML + "<div class='JCONTENIDO-TAB' style='font-size:0.8em;'>"
            CadenaHTML = CadenaHTML + "<div style='overflow-y:auto'><div class='JFILA JDIVCHECK' style='overflow:auto;width:1234px;'>"

            If tabla1.Rows(index)("dsc_imagen").ToString().Trim() = "ECOGRAFIA" Then
                CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + "320" + "px;'>" 'ABRE COLUMNA
            Else
                CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA
            End If



            For index1 = 0 To tabla2.Rows.Count - 1
                If tabla2.Rows(index1)("ide_imagen").ToString().Trim() <> tabla2.Rows(index1)("ide_imagen_titulo").ToString().Trim() Then 'se empieza a contar las filas y no el tab
                    ContadorFila += 1
                End If
                If tabla2.Rows(index1)("ide_imagen").ToString().Trim() = tabla2.Rows(index1)("ide_imagen_titulo").ToString().Trim() Then

                ElseIf tabla2.Rows(index1)("ide_imagen").ToString().Trim() = tabla2.Rows(index1)("ide_imagen_padre").ToString().Trim() Then
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>"
                    CadenaHTML += "<span class='JETIQUETA_5'>" + tabla2.Rows(index1)("dsc_imagen").ToString().Trim() + "</span>"
                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                Else
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>" 'height: 16px;
                    CadenaHTML += "<input type='checkbox' id='chkImagen_" + tabla2.Rows(index1)("ide_imagen").ToString().Trim() + "-" + tabla2.Rows(index1)("ide_imagen_titulo").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_imagen").ToString().Trim() + "' /><label for='chkImagen_" + tabla2.Rows(index1)("ide_imagen").ToString().Trim() + "-" + tabla2.Rows(index1)("ide_imagen_titulo").ToString().Trim() + "' class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_imagen").ToString().Trim() + "</label>"

                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                End If
                If tabla1.Rows(index)("dsc_imagen").ToString().Trim() = "ECOGRAFIA" Then
                    If ContadorFila = 14 Then '25
                        CadenaHTML += "</div>" 'CIERRA COLUMNA
                        If tabla1.Rows(index)("dsc_imagen").ToString().Trim() = "ECOGRAFIA" Then
                            CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + "320" + "px;'>" 'ABRE COLUMNA
                        Else
                            CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA
                        End If
                        ContadorFila = 0
                    End If

                    If ContadorFila <> 14 And index1 = tabla2.Rows.Count - 1 Then
                        CadenaHTML += "</div>" 'CIERRA COLUMNA
                    End If
                Else
                    If ContadorFila = CantidadColumna Then '25
                        CadenaHTML += "</div>" 'CIERRA COLUMNA
                        If tabla1.Rows(index)("dsc_imagen").ToString().Trim() = "ECOGRAFIA" Then
                            CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + "320" + "px;'>" 'ABRE COLUMNA
                        Else
                            CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA
                        End If
                        ContadorFila = 0
                    End If

                    If ContadorFila <> CantidadColumna And index1 = tabla2.Rows.Count - 1 Then
                        CadenaHTML += "</div>" 'CIERRA COLUMNA
                    End If
                End If

            Next
            ContadorFila = 0
            If tabla1.Rows(index)("dsc_imagen").ToString().Trim() = "ECOGRAFIA" Then
                CadenaHTML = CadenaHTML.Replace("width:1234", ("width:" + ((Math.Ceiling(tabla2.Rows.Count / CantidadColumna) * wTabla) + 190).ToString()))
            Else
                CadenaHTML = CadenaHTML.Replace("width:1234", ("width:" + ((Math.Ceiling(tabla2.Rows.Count / CantidadColumna) * wTabla) + 20).ToString()))
            End If

            CadenaHTML = CadenaHTML + "</div></div>"
            CadenaHTML = CadenaHTML + "</div>"
        Next

        CadenaHTML = CadenaHTML + "</div>"

        divContenedorPetitorio.InnerHtml = CadenaHTML
    End Sub



    ''' <summary>
    ''' ENVIAR SOLICITUD
    ''' </summary>
    ''' <param name="CodigosPetitorioImagen"></param>
    ''' <param name="Descripcion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EnviarSolicitudPetitorio(ByVal CodigosPetitorioImagen As String, ByVal Descripcion As String) As String
        Dim pagina As New PetitorioImagen()
        Return pagina.EnviarSolicitudPetitorio_(CodigosPetitorioImagen, Descripcion)
    End Function

    Public Function EnviarSolicitudPetitorio_(ByVal CodigosPetitorioImagen As String, ByVal Descripcion As String) As String
        Try
            Dim TablaDatosP As New DataTable
            Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
            Dim cn1 As New SqlConnection(CnnBD)
            Dim cmd1 As New SqlCommand("Select	isnull(p.codpaciente,'') as codpac, " +
                                        "isnull(p.docidentidad,'') as dni, " +
                                        "isnull(p.direccion,'') as direc, " +
                                        "isnull(p.telefono,'') as tel, " +
                                        "isnull(p.sexo,'') as sex, " +
                                        "isnull(p.fechanacimiento,'') as fecnac, " +
                                        "isnull(d.appaterno,'') as apepat, " +
                                        "isnull(d.apmaterno,'') as apemat, " +
                                        "isnull(d.nombre,'') as nom, " +
                                        "isnull(d.email,'') as email, " +
                                        "GETDATE() as fechahora,  " +
                                        "(select isnull(m.colmedico,'') from medicos m where m.codmedico = @cod_medico) as cmp " +
                                        "From	pacientes p, pacientesdet d " +
                                        "Where	p.codpaciente=d.codpaciente and " +
                                        "p.codpaciente=@cod_paciente", cn1)
            cmd1.Parameters.AddWithValue("@cod_paciente", Session(sCodPaciente))
            cmd1.Parameters.AddWithValue("@cod_medico", Session(sCodMedico))
            cmd1.CommandType = System.Data.CommandType.Text
            cn1.Open()
            Dim da1 As New SqlDataAdapter(cmd1)
            da1.Fill(TablaDatosP)
            cn1.Close()


            If oRceImagenesE.IdeRecetaCab = 0 Then
                oRceImagenesE.CodAtencion = Session(sCodigoAtencion)
                oRceImagenesE.EstImagen = "G"
                oRceImagenesE.CodMedico = Session(sCodMedico)
                oRceImagenesE.UsrRegistra = Session(sCodUser)
                oRceImagenesE.DscReceta = Descripcion
                oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
                oRceImagenLN.Sp_RceRecetaImagenCab_InsertV2(oRceImagenesE)
            End If
            For index = 0 To CodigosPetitorioImagen.Trim().Split(";").Length - 1
                If oRceImagenesE.IdeRecetaCab <> 0 Then
                    GuardarRceRecetaImagenDet(CodigosPetitorioImagen.Trim().Split(";")(index).Trim(), oRceImagenesE.IdeRecetaCab, TablaDatosP)


                    'oRceImagenesE.IdeImagen = CodigosPetitorioImagen.Trim().Split(";")(index).Trim()
                    'oRceImagenesE.UsrRegistra = Session(sCodUser)
                    'oRceImagenesE.CodMedico = Session(sCodMedico)
                    'oRceImagenesE.EstImagen = "G"
                    'oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
                    'oRceImagenLN.Sp_RceRecetaImagenDet_InsertV2(oRceImagenesE)
                    ''TMACASSI 31/10/2016 Si esta activo el WEBSERVICE
                    'Dim TablaDatosRIS As New DataTable()
                    'oRceImagenesE.CodTabla = "01"
                    'TablaDatosRIS = oRceImagenLN.Sp_Ris_Consulta_RisPacs(oRceImagenesE)
                    'If TablaDatosRIS.Rows.Count > 0 Then
                    '    If TablaDatosRIS.Rows(0)(4).ToString() = "A" Then
                    '        EnviarOrdenRis(oRceImagenesE)
                    '    End If
                    'End If

                End If
            Next

            '<I-TMACASSI>25/10/2016
            oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
            oRceEvolucionE.IdeOrdenCab = oRceImagenesE.IdeRecetaCab 'Session(sIdeRecetaImagenCab)
            oRceEvolucionE.Orden = 1
            oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

            If oRceEvolucionE.CodigoEvolucion <> 0 Then
            Else
                Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
            End If
            '<I-TMACASSI>25/10/2016
            Session.Remove(sIdeRecetaImagenCab)
            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    Public Function GuardarRceRecetaImagenDet(ByVal IdeImagen As Integer, ByVal IdeRecetaImagenCab As Integer, ByVal TablaDatosPaciente As DataTable) As Integer
        Dim oRceImagenesE_ As New RceImagenesE()
        Dim oRceImagenLN_ As New RceImagenLN()

        oRceImagenesE_.IdeRecetaCab = IdeRecetaImagenCab
        oRceImagenesE_.IdeImagen = IdeImagen
        oRceImagenesE_.UsrRegistra = Session(sCodUser)
        oRceImagenesE_.CodMedico = Session(sCodMedico)
        oRceImagenesE_.EstImagen = "G"
        oRceImagenesE_.TipoDeAtencion = Session(sTipoAtencion)
        oRceImagenLN_.Sp_RceRecetaImagenDet_InsertV2(oRceImagenesE_)

        'Return "OK" 'JB - COMENTAR ESTA LINEA LUEGO - 03/09/2019

        'I - GLLUNCOR -14/02/2023 - -> Valida estado del servicio de RIS - Oracle
        Dim oTablasE_ As New TablasE()
        Dim oTablasLN_ As New TablasLN()

        oTablasE_.CodTabla = "RIS_PACS_ORACLE"
        oTablasE_.Buscar = ""
        oTablasE_.Key = 0
        oTablasE_.NumeroLineas = 0
        oTablasE_.Orden = 5
        Dim dt As New DataTable()
        dt = oTablasLN_.Sp_Tablas_Consulta(oTablasE_)
        'xRuta = tabla_.Rows(0)("nombre").ToString().Trim() 'C:\TEMP\
        If dt.Rows(0)("estado") = "A" Then

            Dim dt1 As New DataTable()
            dt1 = oRceImagenLN.RIS_PACS_WS()

            Dim ObjetoServicioRisPacs As New WsRisPacs.HisXmlEvents()
            ObjetoServicioRisPacs.Url = dt1.Rows(0)("nombre").ToString().Trim()

            Dim Respuesta = ""
            Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
            Dim cn1 As New SqlConnection(CnnBD)
            Dim cmd1 As New SqlCommand("select isnull(pta.nombre,'') as nombre from prestaciones pta where pta.codprestacion = @cod_prestacion", cn1)
            cmd1.Parameters.AddWithValue("@cod_prestacion", oRceImagenesE_.CodPrestacion)
            cmd1.CommandType = System.Data.CommandType.Text
            cn1.Open()
            Dim tabla1 As New DataTable()
            Dim da1 As New SqlDataAdapter(cmd1)
            da1.Fill(tabla1)
            cn1.Close()

            Dim FechaHora As String = ""
            Dim EventDateTime As String
            Dim Cumple As String = ""
            Dim Sexo As String
            Dim FechaUltima As String
            Dim StartDateTime As String
            FechaHora = Trim(TablaDatosPaciente.Rows(0)("fechahora"))
            EventDateTime = Format(CDate(FechaHora), "yyyy-MM-dd HH:mm:ss") 'Format(CDate(FechaHora), "dd/MM/yyyy h:mm:ss")

            If Not IsNothing(TablaDatosPaciente.Rows(0)("fecnac")) And TablaDatosPaciente.Rows(0)("fecnac").ToString() <> "" Then
                Cumple = Format(CDate(Trim(TablaDatosPaciente.Rows(0)("fecnac"))), "yyyy/MM/dd") 'Format(Format(CDate(Trim(wDatosRis("FecNac"))), "dd/mm/yyyy"), "yyyymmdd")
            End If
            If Trim(TablaDatosPaciente.Rows(0)("sex")) = "M" Then
                Sexo = "2"
            ElseIf Trim(TablaDatosPaciente.Rows(0)("sex")) = "F" Then
                Sexo = "1"
            Else
                Sexo = "4"
            End If
            FechaUltima = Format(Now, "dd/MM/yyyy")

            StartDateTime = Format(CDate(FechaHora), "yyyy-MM-dd HH:mm:ss")
            Cumple = Cumple.Replace("/", "").Replace("-", "").Replace(" ", "")
            StartDateTime = StartDateTime.Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "")


            Dim TipoPaciente As String = ""
            If Mid$(oRceImagenesE_.CodPresotor, 1, 1) = "E" Or Mid$(oRceImagenesE_.CodPresotor, 1, 1) = "Q" Then  'Emergencia. - 28/06/2013 AGARCIA Se incluye tipoAtencion Q=Cirugias_Amb (es como Emergencia). 
                TipoPaciente = "1"
            ElseIf Mid$(oRceImagenesE_.CodPresotor, 1, 1) = "H" Then 'Hospitalización 
                TipoPaciente = "2"
            Else
                TipoPaciente = "3"
            End If

            Dim TablaDatosSala As New DataTable()
            oRceImagenesE_.CodLocal = Mid$(oRceImagenesE_.CodPresotor, 2, 1)
            TablaDatosSala = oRceImagenLN_.Sp_Ris_Sala_Consulta(oRceImagenesE_)

            If TablaDatosSala.Rows.Count > 0 Then
                Dim CodigoSala As String
                CodigoSala = TablaDatosSala.Rows(0)(1).ToString()

                Dim VisitNumber As String
                VisitNumber = Mid$(oRceImagenesE_.CodPresotor, 2, Len(oRceImagenesE_.CodPresotor))
                Try
                    Respuesta = ObjetoServicioRisPacs.Insert_HIS("XML", "20", "0", oRceImagenesE_.CodPresotor.ToString(), EventDateTime, "1012", Trim(TablaDatosPaciente.Rows(0)("codpac")), Trim(TablaDatosPaciente.Rows(0)("dni")), + _
                    TipoPaciente, "N", Session(sCodUser), Trim(TablaDatosPaciente.Rows(0)("apepat")) & " " & Trim(TablaDatosPaciente.Rows(0)("apemat")), Trim(TablaDatosPaciente.Rows(0)("nom")), + _
                    Cumple, Sexo, FechaUltima, Trim(TablaDatosPaciente.Rows(0)("direc")), "LIMA", "PERU", Trim(TablaDatosPaciente.Rows(0)("tel")), Trim(TablaDatosPaciente.Rows(0)("email")), + _
                    VisitNumber, StartDateTime, "", "40", "Agendado", + _
                    oRceImagenesE_.CodPrestacion, tabla1.Rows(0)("nombre").ToString().Trim(), CodigoSala, Trim(TablaDatosPaciente.Rows(0)("cmp")), "N", "", "STUDY_SCHEDULED", "")

                    oRceImagenesE_.MSG_STATUS = Respuesta
                    If Respuesta = "OK-WS-ORM" Then
                        oRceImagenesE_.ORACLE = "A"
                    Else
                        oRceImagenesE_.ORACLE = "X"
                    End If

                Catch ex As Exception
                    oRceImagenesE_.MSG_STATUS = ex.Source + ": " + ex.Message
                    oRceImagenesE_.ORACLE = "X"
                End Try

                Dim EventDateTimeSQL As String = ""
                EventDateTimeSQL = Format(CDate(FechaHora), "MM/dd/yyyy HH:mm:ss")


                oRceImagenesE_.X_TIPOMSG = "XML"
                oRceImagenesE_.T_COD_EMPRESA = "20"
                oRceImagenesE_.T_COD_SUCURSAL = "0"
                oRceImagenesE_.T_EVENT_ID = oRceImagenesE_.CodPresotor.ToString() 'cambiar por cod presotor devuelto por Sp_PresotorImagen_Insert
                oRceImagenesE_.T_EVENT_DATETIME = EventDateTimeSQL
                oRceImagenesE_.T_EVENT_TYPE_ID = "1012"
                oRceImagenesE_.X_ID_PACIENTE = Trim(TablaDatosPaciente.Rows(0)("codpac"))
                oRceImagenesE_.X_RUT_PACIENTE = Trim(TablaDatosPaciente.Rows(0)("dni"))
                oRceImagenesE_.X_TIPO_PACIENTE = TipoPaciente
                oRceImagenesE_.X_DEATH_INDICATOR = "N"
                oRceImagenesE_.X_CAT_NAME = Session(sCodUser)
                oRceImagenesE_.X_LAST_NAME = Trim(TablaDatosPaciente.Rows(0)("apepat")) & " " & Trim(TablaDatosPaciente.Rows(0)("apemat"))
                oRceImagenesE_.X_FIRST_NAME = Trim(TablaDatosPaciente.Rows(0)("nom"))
                oRceImagenesE_.X_BIRTH_DATE = Cumple
                oRceImagenesE_.X_GENDER_KEY = Trim(Sexo)
                oRceImagenesE_.X_LAST_UPDATED = FechaUltima
                oRceImagenesE_.X_STREET_ADDRESS = Trim(TablaDatosPaciente.Rows(0)("direc"))
                oRceImagenesE_.X_CITY = "LIMA"
                oRceImagenesE_.X_COUNTRY = "PERU"
                oRceImagenesE_.X_PHONE_NUMBER = Trim(TablaDatosPaciente.Rows(0)("tel"))
                oRceImagenesE_.X_VISIT_NUMBER = VisitNumber
                oRceImagenesE_.X_START_DATETIME = StartDateTime
                oRceImagenesE_.X_DURATION = ""
                oRceImagenesE_.X_STATUS_KEY = "40"
                oRceImagenesE_.X_STATUS = "Agendado"
                oRceImagenesE_.X_PROCEDURE_CODE = oRceImagenesE_.CodPrestacion
                oRceImagenesE_.X_PROCEDURE_DESCRIPTION = tabla1.Rows(0)("nombre").ToString().Trim()
                oRceImagenesE_.X_ROOM_CODE = CodigoSala
                oRceImagenesE_.X_REQUESTED_BY = Trim(TablaDatosPaciente.Rows(0)("cmp"))
                oRceImagenesE_.X_MESSAGE_TYPE = "STUDY_SCHEDULED"
                oRceImagenesE_.X_PACS_SPS_ID = ""
                'oRceImagenesE_.MSG_STATUS = ""
                oRceImagenLN_.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE_)
            End If
            'F - GLLUNCOR -14/02/2023 - -> Valida estado del servicio de RIS - Oracle
        End If

    End Function






    <System.Web.Services.WebMethod()>
    Public Shared Function EnviarSolicitudPetitorio2(ByVal CodigoImagen As String, ByVal Observacion As String, ByVal CodigoAtencion As String, ByVal IdeHistoria As String, ByVal CodMedico As String, ByVal CodPaciente As String) As String
        Dim pagina As New PetitorioImagen()
        Return pagina.EnviarSolicitudPetitorio2_(CodigoImagen, Observacion, CodigoAtencion, IdeHistoria, CodMedico, CodPaciente)
    End Function

    Public Function EnviarSolicitudPetitorio2_(ByVal CodigoImagen As String, ByVal Observacion As String, ByVal CodigoAtencion As String, ByVal IdeHistoria As String, ByVal CodMedico As String, ByVal CodPaciente As String) As String
        Try
            Dim Codigo() As String
            Dim Observacionx() As String
            'Split(New Char() {","c}).Distinct().ToArray()
            Codigo = CodigoImagen.Split("@") '146-144  *;*  147-144  *;*  292-290  *;*  293-290  *;*
            Observacionx = Observacion.Split("@") 'asdfasdfasfasdf-144  *;*  -224   *;*  trrtrtrtrt-290  *;*  -326*;*-331*;*-336*;*
            Dim tab As String = ""
            Dim GrabarCabecera As Boolean = False
            Dim IdeRceRecetaImagenCab As Integer
            Dim TablaDatosP As New DataTable


            Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
            Dim cn1 As New SqlConnection(CnnBD)
            Dim cmd1 As New SqlCommand("Select	isnull(p.codpaciente,'') as codpac, " +
                                        "isnull(p.docidentidad,'') as dni, " +
                                        "isnull(p.direccion,'') as direc, " +
                                        "isnull(p.telefono,'') as tel, " +
                                        "isnull(p.sexo,'') as sex, " +
                                        "isnull(p.fechanacimiento,'') as fecnac, " +
                                        "isnull(d.appaterno,'') as apepat, " +
                                        "isnull(d.apmaterno,'') as apemat, " +
                                        "isnull(d.nombre,'') as nom, " +
                                        "isnull(d.email,'') as email, " +
                                        "GETDATE() as fechahora,  " +
                                        "(select isnull(m.colmedico,'') from medicos m where m.codmedico = @cod_medico) as cmp " +
                                        "From	pacientes p, pacientesdet d " +
                                        "Where	p.codpaciente=d.codpaciente and " +
                                        "p.codpaciente=@cod_paciente", cn1)
            cmd1.Parameters.AddWithValue("@cod_paciente", Session(sCodPaciente))
            cmd1.Parameters.AddWithValue("@cod_medico", Session(sCodMedico))
            cmd1.CommandType = System.Data.CommandType.Text
            cn1.Open()
            Dim da1 As New SqlDataAdapter(cmd1)
            da1.Fill(TablaDatosP)
            cn1.Close()

            For index = 0 To Observacionx.Length - 1 'por cada observacion/tab
                If Observacionx(index).ToString().Trim() <> "" Then
                    tab = Observacionx(index).ToString().Split("-")(1) 'obtiene el id del tab(tipo examen)
                    GrabarCabecera = True 'se le otorgara valor true para que grabe cada cabecera(tab)

                    For index1 = 0 To Codigo.Length - 1
                        If Codigo(index1).ToString().Trim() <> "" Then
                            If tab = Codigo(index1).ToString().Split("-")(1) Then 'si se cumple esta condicion significa que se marco algun check en ese tab
                                If GrabarCabecera = True Then
                                    IdeRceRecetaImagenCab = GuardarRceRecetaImageCab(Observacionx(index).ToString().Split("-")(0), Session(sCodigoAtencion), tab)
                                    GrabarCabecera = False

                                    oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
                                    oRceInicioSesionE.CodUser = Session(sCodUser)
                                    oRceInicioSesionE.Formulario = "PetitorioImagen"
                                    oRceInicioSesionE.Control = "PETITORIO IMAGEN"
                                    oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
                                    oRceInicioSesionE.DscPcName = Session(sDscPcName)
                                    oRceInicioSesionE.DscLog = "Se envio las imagenes " + IdeRceRecetaImagenCab.ToString()
                                    oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)


                                    oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
                                    oRceEvolucionE.IdeOrdenCab = IdeRceRecetaImagenCab
                                    oRceEvolucionE.Orden = 1
                                    oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

                                    If oRceEvolucionE.CodigoEvolucion <> 0 Then
                                        'INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
                                        Dim CnnBD_ As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

                                        Dim pdf_byte As Byte() = ExportaPDF("DA")
                                        Dim oHospitalE As New HospitalE()
                                        Dim oHospitalLN As New HospitalLN()
                                        Dim cn As New SqlConnection(CnnBD_)
                                        'Paso 1
                                        oHospitalE.TipoDoc = 10
                                        oHospitalE.CodAtencion = Session(sCodigoAtencion)
                                        oHospitalE.CodUser = Session(sCodUser)
                                        oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
                                        oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

                                        'Paso 2
                                        Dim cmd2 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
                                        cmd2.CommandType = CommandType.Text
                                        cmd2.Parameters.AddWithValue("@bib_documento", pdf_byte)
                                        cmd2.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
                                        cmd2.Parameters.AddWithValue("@flg_reqfirma", "0")

                                        Dim num1 As Integer
                                        cn.Open()
                                        num1 = cmd2.ExecuteNonQuery()
                                        cn.Close()

                                        'Paso 3
                                        oHospitalE.IdeHistoria = Session(sIdeHistoria)
                                        oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
                                        oHospitalE.TipoDoc = 10
                                        oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
                                        'FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
                                    Else

                                    End If

                                End If
                                GuardarRceRecetaImagenDet(Codigo(index1).ToString().Split("-")(0), oRceImagenesE.IdeRecetaCab, TablaDatosP)
                            End If
                        End If
                    Next
                    'pdf
                End If
            Next
            'OrdenImagen(Session(sCodigoAtencion), oRceImagenesE.IdeRecetaCab, "")  JB - COMENTADO - 01/03/2021
            Return "OK"

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    Public Function GuardarRceRecetaImageCab(ByVal Observacion As String, ByVal CodigoAtencion As String, ByVal IdeTipoExamen As Integer) As Integer
        Dim IdeRceRecetaImagenCab As Integer

        oRceImagenesE.CodAtencion = CodigoAtencion
        oRceImagenesE.EstImagen = "G"
        oRceImagenesE.CodMedico = Session(sCodMedico)
        oRceImagenesE.UsrRegistra = Session(sCodUser)
        oRceImagenesE.DscReceta = Observacion.ToUpper()
        oRceImagenesE.TipoDeAtencion = "H"
        oRceImagenesE.TipoExamen = IdeTipoExamen
        oRceImagenLN.Sp_RceRecetaImagenCab_InsertV2(oRceImagenesE)
        IdeRceRecetaImagenCab = oRceImagenesE.IdeRecetaCab

        Return IdeRceRecetaImagenCab
    End Function


    'INICIO - JB -31/01/2017
    Public Function ExportaPDF(ByVal Tipo As String) As Byte()
        Dim pdf_byte As Byte() = Nothing
        Dim crystalreport As New ReportDocument()
        Dim xRuta As String = sRutaTemp

        If Tipo = "DA" Then
            Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
            Dim tabla As New DataTable("Rp_RceEvolucion")
            tabla.Columns.Add("ide_historia")
            tabla.Columns.Add("ide_evolucion")
            tabla.Columns.Add("medico")
            tabla.Columns.Add("flg_educacion")
            tabla.Columns.Add("flg_informe")
            tabla.Columns.Add("TipoEvolucion")
            tabla.Columns.Add("txt_detalle")
            tabla.Columns.Add("cama")
            tabla.Columns.Add("cuarto")
            tabla.Columns.Add("dni")
            tabla.Columns.Add("fec_nacimiento")
            tabla.Columns.Add("fec_ingreso")
            tabla.Columns.Add("ape_paterno")
            tabla.Columns.Add("ape_materno")
            tabla.Columns.Add("nombres")
            tabla.Columns.Add("fec_registro")
            tabla.Columns.Add("hora_registro")
            'tabla.Columns.Add("firma_medico")
            Dim columna_firma As DataColumn = New DataColumn("firma_medico")
            columna_firma.DataType = System.Type.GetType("System.Byte[]")
            tabla.Columns.Add(columna_firma)
            tabla.Columns.Add("nmedico")
            tabla.Columns.Add("RNE")
            tabla.Columns.Add("CMP")
            'INICIO - JB - COMENTADO - 24/09/2019
            'For index = 0 To Reporte1.GetData(Session(sIdeHistoria), oRceEvolucionE.CodigoEvolucion, 1).Rows.Count - 1
            '    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), oRceEvolucionE.CodigoEvolucion, 1).Rows.Item(index).ItemArray)
            'Next
            'FIN - JB - COMENTADO - 24/09/2019

            'INICIO - JB - NUEVO - 24/09/2019
            Dim oRceEvolucionE_ As New RceEvolucionE
            oRceEvolucionE_.IdHistoria = Session(sIdeHistoria)
            oRceEvolucionE_.CodigoEvolucion = oRceEvolucionE.CodigoEvolucion
            oRceEvolucionE_.Orden = 1
            tabla = oRceEvolucionLN.Rp_RceEvolucion(oRceEvolucionE_)
            Dim distinctValues = tabla.AsEnumerable().[Select](Function(row) New With {
             Key .ide_evolucion = row.Field(Of Integer)("ide_evolucion"),
                 .ide_historia = row.Field(Of Integer)("ide_historia"),
                 .medico = row.Field(Of String)("medico"),
                 .flg_educacion = row.Field(Of String)("flg_educacion"),
                 .flg_informe = row.Field(Of String)("flg_informe"),
                 .TipoEvolucion = row.Field(Of String)("TipoEvolucion"),
                 .txt_detalle = row.Field(Of String)("txt_detalle"),
                 .cama = row.Field(Of String)("cama"),
                 .cuarto = row.Field(Of String)("cuarto"),
                 .dni = row.Field(Of String)("dni"),
                 .fec_nacimiento = row.Field(Of DateTime)("fec_nacimiento"),
                 .fec_ingreso = row.Field(Of DateTime)("fec_ingreso"),
                 .ape_paterno = row.Field(Of String)("ape_paterno"),
                 .ape_materno = row.Field(Of String)("ape_materno"),
                 .nombres = row.Field(Of String)("nombres"),
                 .fec_registro = row.Field(Of String)("fec_registro"),
                 .hora_registro = row.Field(Of String)("hora_registro"),
                 .firma_medico = row.Field(Of Byte())("firma_medico"),
                 .nmedico = row.Field(Of String)("nmedico"),
                 .RNE = row.Field(Of String)("RNE"),
            .CMP = row.Field(Of String)("CMP")
            }).Distinct()
            'INICIO - JB - NUEVO - 24/09/2019

            crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinica2.rpt"))
            crystalreport.SetDataSource(distinctValues)
        ElseIf Tipo = "ME" Then
            Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceMedicamentosaTableAdapter()
            Dim tabla As New DataTable("Rp_RceMedicamentosa")
            tabla.Columns.Add("ide_medicamentosa_det")
            tabla.Columns.Add("ide_medicamentosa_cab")
            tabla.Columns.Add("cod_producto")
            tabla.Columns.Add("dsc_producto")
            tabla.Columns.Add("num_dosis")
            tabla.Columns.Add("dsc_via")
            tabla.Columns.Add("num_frecuencia")
            tabla.Columns.Add("cod_accion")
            tabla.Columns.Add("ide_examenfisicores")
            tabla.Columns.Add("fec_ultima")
            tabla.Columns.Add("hor_ultima")
            tabla.Columns.Add("flg_medicamento")
            tabla.Columns.Add("flg_activo")
            tabla.Columns.Add("fec_registra")
            tabla.Columns.Add("usr_registra")
            tabla.Columns.Add("fec_modifica")
            tabla.Columns.Add("usr_modifica")
            tabla.Columns.Add("dsc_paciente")
            tabla.Columns.Add("servicio")
            tabla.Columns.Add("dsc_servicio")
            tabla.Columns.Add("cama")
            tabla.Columns.Add("fecha")
            Dim columna_firma As DataColumn = New DataColumn("firma_tratante")
            columna_firma.DataType = System.Type.GetType("System.Byte[]")
            tabla.Columns.Add(columna_firma)
            Dim columna_firma1 As DataColumn = New DataColumn("firma_hospitalario")
            columna_firma1.DataType = System.Type.GetType("System.Byte[]")
            tabla.Columns.Add(columna_firma1)
            For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Count - 1
                tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 1).Rows.Item(index).ItemArray)
            Next
            crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpMedicamentosa.rpt"))
            crystalreport.SetDataSource(tabla)
        End If

        Dim OpcionExportar As ExportOptions
        Dim OpcionDestino As New DiskFileDestinationOptions()
        Dim OpcionesFormato As New PdfRtfWordFormatOptions()
        Dim NombreArchivo As String = Tipo + Session(sIdeHistoria).ToString() + ".pdf"
        OpcionDestino.DiskFileName = xRuta + "\" + NombreArchivo
        OpcionExportar = crystalreport.ExportOptions
        With OpcionExportar
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
            .DestinationOptions = OpcionDestino
            .FormatOptions = OpcionesFormato
        End With
        crystalreport.Export()

        pdf_byte = System.IO.File.ReadAllBytes(xRuta + "\" + NombreArchivo)
        'System.IO.File.WriteAllBytes(xRuta + "\" + NombreArchivo, pdf_byte)
        System.IO.File.Delete(xRuta + "\" + NombreArchivo) 'eliminando el archivo - jb - 15/07/2020

        crystalreport.Close()
        crystalreport.Dispose()

        Return pdf_byte
    End Function
    'FIN - JB - 31/01/2017


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaDiagnostico() As String
        Dim pagina As New PetitorioImagen()
        Return pagina.ValidaDiagnostico_()
    End Function

    Public Function ValidaDiagnostico_() As String
        Try
            Dim oRceDiagnosticoE As New RceDiagnosticoE()
            Dim oRceDiagnosticoLN As New RceDiagnosticoLN()
            Dim tabla As New DataTable()
            oRceDiagnosticoE.Tipo = "A"
            oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
            tabla = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)


            If tabla.Rows.Count > 0 Then
                Return "OK"
            Else
                Return ""
            End If
        Catch ex As Exception

        End Try
    End Function



    'Public Function EnviarSolicitudPetitorio_(ByVal CodigosPetitorioImagen As String, ByVal Descripcion As String) As String
    '    Try
    '        If oRceImagenesE.IdeRecetaCab = 0 Then
    '            oRceImagenesE.CodAtencion = Session(sCodigoAtencion)
    '            oRceImagenesE.EstImagen = "G"
    '            oRceImagenesE.CodMedico = Session(sCodMedico)
    '            oRceImagenesE.UsrRegistra = Session(sCodUser)
    '            oRceImagenesE.DscReceta = Descripcion
    '            oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
    '            oRceImagenLN.Sp_RceRecetaImagenCab_InsertV2(oRceImagenesE)
    '        End If
    '        For index = 0 To CodigosPetitorioImagen.Trim().Split(";").Length - 1
    '            If oRceImagenesE.IdeRecetaCab <> 0 Then
    '                oRceImagenesE.IdeImagen = CodigosPetitorioImagen.Trim().Split(";")(index).Trim()
    '                oRceImagenesE.UsrRegistra = Session(sCodUser)
    '                oRceImagenesE.CodMedico = Session(sCodMedico)
    '                oRceImagenesE.EstImagen = "G"
    '                oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
    '                oRceImagenLN.Sp_RceRecetaImagenDet_InsertV2(oRceImagenesE)


    '                'TMACASSI 31/10/2016 Si esta activo el WEBSERVICE
    '                Dim TablaDatosRIS As New DataTable()
    '                oRceImagenesE.CodTabla = "01"
    '                TablaDatosRIS = oRceImagenLN.Sp_Ris_Consulta_RisPacs(oRceImagenesE)
    '                If TablaDatosRIS.Rows.Count > 0 Then
    '                    If TablaDatosRIS.Rows(0)(4).ToString() = "A" Then
    '                        EnviarOrdenRis(oRceImagenesE)
    '                    End If
    '                End If

    '            End If
    '        Next

    '        '<I-TMACASSI>25/10/2016
    '        oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
    '        oRceEvolucionE.IdeOrdenCab = oRceImagenesE.IdeRecetaCab 'Session(sIdeRecetaImagenCab)
    '        oRceEvolucionE.Orden = 1
    '        oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

    '        If oRceEvolucionE.CodigoEvolucion <> 0 Then
    '        Else
    '            Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
    '        End If
    '        '<I-TMACASSI>25/10/2016
    '        Session.Remove(sIdeRecetaImagenCab)
    '        Return "OK"
    '    Catch ex As Exception
    '        Return ex.Message.ToString()
    '    End Try
    'End Function

    Public Function EnviarOrdenRis(ByVal oRceImagenesE As RceImagenesE)
        'PREPARANDO PARA INVOCAR SERVICIO
        Dim Valor As String
        oRceImagenesE.CodLocal = Mid$(oRceImagenesE.CodPresotor, 2, 1)
        oRceImagenesE.Orden = 1
        Valor = oRceImagenLN.Sp_Ris_EvaluaAtencion(oRceImagenesE)


        If Valor = "-5" Then  'SI ES -5 EJECUTARA EL SERVICIO

            Dim TablaDatosPresotor As New DataTable()
            TablaDatosPresotor = oRceImagenLN.Sp_Ris_ListaDatosPresotor(oRceImagenesE)

            If TablaDatosPresotor.Rows.Count > 0 Then
                Dim TablaDatosSala As New DataTable()
                oRceImagenesE.CodLocal = Mid$(oRceImagenesE.CodPresotor, 2, 1)
                TablaDatosSala = oRceImagenLN.Sp_Ris_Sala_Consulta(oRceImagenesE)

                If TablaDatosSala.Rows.Count > 0 Then
                    Dim CodigoSala As String
                    CodigoSala = TablaDatosSala.Rows(0)(1).ToString()

                    Dim Mensaje As String
                    Dim Empresa As String
                    Dim Sucursal As String
                    Dim EventTypeID As String
                    Dim Sexo As String
                    Dim TipoPaciente As String
                    Dim StatusKey As String
                    Dim Status As String
                    Dim EventDateTimeOracle As String
                    Dim EventDateTimeSQL As String
                    Dim CodPaciente As String
                    Dim DniPaciente As String
                    Dim Apellidos As String
                    Dim Nombre As String
                    Dim BirthDay As String
                    Dim LastUpdate As String
                    Dim Direccion As String
                    Dim Ciudad As String
                    Dim Pais As String
                    Dim PhoneNumber As String
                    Dim VisitNumber As String
                    Dim StartDateTime As String
                    Dim Duracion As String
                    Dim ProcedureCode As String
                    Dim ProcedureDesc As String
                    Dim RoomCode As String
                    Dim RequestBy As String
                    Dim MessageType As String
                    Dim FechaHora As String
                    Dim PacsId As String

                    Mensaje = "XML" 'AMB Mensaje para Ambulatorios
                    Empresa = "20"
                    Sucursal = "1"
                    EventTypeID = ""
                    Sexo = ""
                    TipoPaciente = ""
                    StatusKey = ""
                    Status = ""
                    FechaHora = ""
                    EventDateTimeOracle = ""
                    EventDateTimeSQL = ""
                    CodPaciente = ""
                    DniPaciente = ""
                    Apellidos = ""
                    Nombre = ""
                    BirthDay = ""
                    LastUpdate = ""
                    Direccion = ""
                    Ciudad = "LIMA"
                    Pais = "PERU"
                    PhoneNumber = ""
                    VisitNumber = ""
                    StartDateTime = ""
                    Duracion = ""
                    ProcedureCode = ""
                    ProcedureDesc = ""
                    RoomCode = ""
                    RequestBy = ""
                    MessageType = ""
                    PacsId = ""

                    If Trim(TablaDatosPresotor.Rows(0)("sex")) = "M" Then
                        Sexo = "2"
                    ElseIf Trim(TablaDatosPresotor.Rows(0)("sex")) = "F" Then
                        Sexo = "1"
                    Else
                        Sexo = "4" '<TMACASSI> 18/01/2013   4 = unknown. 
                    End If

                    If Mid$(oRceImagenesE.CodPresotor, 1, 1) = "E" Or Mid$(oRceImagenesE.CodPresotor, 1, 1) = "Q" Then  'Emergencia. - 28/06/2013 AGARCIA Se incluye tipoAtencion Q=Cirugias_Amb (es como Emergencia). 
                        TipoPaciente = "1"
                    ElseIf Mid$(oRceImagenesE.CodPresotor, 1, 1) = "H" Then 'Hospitalización 
                        TipoPaciente = "2"
                    Else
                        TipoPaciente = "3"
                    End If

                    StatusKey = "40"
                    Status = "Agendado"
                    MessageType = "STUDY_SCHEDULED"
                    EventTypeID = "1012"

                    FechaHora = Trim(TablaDatosPresotor.Rows(0)("fechahora"))
                    EventDateTimeOracle = Format(CDate(FechaHora), "dd/MM/yyyy h:mm:ss") 'Format(Format(CDate(FechaHora), "yyyy/mm/dd h:mm:ss"), "dd/mm/yyyy h:mm:ss")
                    EventDateTimeSQL = Format(CDate(FechaHora), "MM/dd/yyyy h:mm:ss") 'Format(Format(CDate(FechaHora), "yyyy/mm/dd h:mm:ss"), "mm/dd/yyyy h:mm:ss")

                    CodPaciente = Trim(TablaDatosPresotor.Rows(0)("codpac"))
                    DniPaciente = Trim(TablaDatosPresotor.Rows(0)("dni"))
                    Apellidos = Trim(TablaDatosPresotor.Rows(0)("apepat")) & " " & Trim(TablaDatosPresotor.Rows(0)("apemat"))
                    Nombre = Trim(TablaDatosPresotor.Rows(0)("nom"))
                    If Not IsNothing(TablaDatosPresotor.Rows(0)("FecNac")) And TablaDatosPresotor.Rows(0)("FecNac").ToString() <> "" Then
                        BirthDay = Format(CDate(Trim(TablaDatosPresotor.Rows(0)("FecNac"))), "yyyy/MM/dd") 'Format(Format(CDate(Trim(wDatosRis("FecNac"))), "dd/mm/yyyy"), "yyyymmdd")
                    End If
                    LastUpdate = Format(Now, "dd/MM/yyyy")
                    Direccion = Trim(TablaDatosPresotor.Rows(0)("direc"))
                    PhoneNumber = Trim(TablaDatosPresotor.Rows(0)("tel"))
                    VisitNumber = Mid$(oRceImagenesE.CodPresotor, 2, Len(oRceImagenesE.CodPresotor))
                    StartDateTime = Format(CDate(FechaHora), "yyyy-MM-dd h:mm:ss") 'Format(Format(CDate(FechaHora), "yyyy-mm-dd h:mm:ss"), "yyyymmddhhmmss")

                    ProcedureCode = Trim(TablaDatosPresotor.Rows(0)("codpres"))
                    ProcedureDesc = Trim(TablaDatosPresotor.Rows(0)("despre"))

                    RoomCode = CodigoSala
                    RequestBy = Trim(TablaDatosPresotor.Rows(0)("cmp"))

                    Dim strSoap As String
                    Dim strSOAPAction As String
                    Dim strWsdl As String
                    Dim Resultado As String
                    Dim vacio As String
                    strSOAPAction = ""
                    strWsdl = ""
                    Resultado = ""
                    vacio = ""

                    strSOAPAction = "http://clinicasanfelipe.com/Insert_HIS"
                    Dim TablaWsdl As New DataTable()
                    TablaWsdl = oRceImagenLN.Sp_Ris_Consulta_WS(oRceImagenesE)

                    If TablaWsdl.Rows.Count > 0 Then
                        strWsdl = Trim(TablaWsdl.Rows(0)("nombre").ToString())
                    End If

                    strSoap = ""
                    strSoap = strSoap & "<?xml version=""1.0"" encoding=""utf-8""?>"
                    strSoap = strSoap & "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" "
                    strSoap = strSoap & "xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><Insert_HIS xmlns=""http://clinicasanfelipe.com/"">"

                    strSoap = strSoap & "<TIPOMSG>" & Mensaje & "</TIPOMSG>"
                    strSoap = strSoap & "<COD_EMPRESA>" & Empresa & "</COD_EMPRESA>"
                    strSoap = strSoap & "<COD_SUCURSAL>" & Sucursal & "</COD_SUCURSAL>"
                    strSoap = strSoap & "<EVENT_ID>" & oRceImagenesE.CodPresotor.ToString().Trim() & "</EVENT_ID>"
                    strSoap = strSoap & "<EVENT_DATETIME>" & EventDateTimeOracle & "</EVENT_DATETIME>"
                    strSoap = strSoap & "<EVENT_TYPE_ID>" & EventTypeID & "</EVENT_TYPE_ID>"
                    strSoap = strSoap & "<ID_PACIENTE>" & CodPaciente & "</ID_PACIENTE>"
                    strSoap = strSoap & "<RUT_PACIENTE>" & DniPaciente & "</RUT_PACIENTE>"
                    strSoap = strSoap & "<TIPO_PACIENTE>" & Trim(TipoPaciente) & "</TIPO_PACIENTE>"
                    strSoap = strSoap & "<DEATH_INDICATOR>N</DEATH_INDICATOR>"
                    strSoap = strSoap & "<CAT_NAME>" & Session(sCodUser) & "</CAT_NAME>" 'zClinicac.UserId   TablaWsdl.Rows(0)("nombre").ToString()
                    strSoap = strSoap & "<LAST_NAME>" & Apellidos & "</LAST_NAME>"
                    strSoap = strSoap & "<FIRST_NAME>" & Nombre & "</FIRST_NAME>"
                    strSoap = strSoap & "<BIRTH_DATE>" & BirthDay & "</BIRTH_DATE>"
                    strSoap = strSoap & "<GENDER_KEY>" & Sexo & "</GENDER_KEY>"
                    strSoap = strSoap & "<LAST_UPDATED>" & LastUpdate & "</LAST_UPDATED>"
                    strSoap = strSoap & "<STREET_ADDRESS>" & Direccion & "</STREET_ADDRESS>"
                    strSoap = strSoap & "<CITY>" & Ciudad & "</CITY>"
                    strSoap = strSoap & "<COUNTRY>" & Pais & "</COUNTRY>"
                    strSoap = strSoap & "<PHONE_NUMBER>" & PhoneNumber & "</PHONE_NUMBER>"
                    strSoap = strSoap & "<VISIT_NUMBER>" & VisitNumber & "</VISIT_NUMBER>"
                    strSoap = strSoap & "<START_DATETIME>" & StartDateTime & "</START_DATETIME>"
                    strSoap = strSoap & "<DURATION>" & Duracion & "</DURATION>"
                    strSoap = strSoap & "<STATUS_KEY>" & Trim(StatusKey) & "</STATUS_KEY>"
                    strSoap = strSoap & "<STATUS>" & Trim(Status) & "</STATUS>"
                    strSoap = strSoap & "<PROCEDURE_CODE>" & ProcedureCode & "</PROCEDURE_CODE>"
                    strSoap = strSoap & "<PROCEDURE_DESCRIPTION>" & ProcedureDesc & "</PROCEDURE_DESCRIPTION>"
                    strSoap = strSoap & "<ROOM_CODE>" & RoomCode & "</ROOM_CODE>"
                    strSoap = strSoap & "<REQUESTED_BY>" & RequestBy & "</REQUESTED_BY>"
                    strSoap = strSoap & "<MESSAGE_TYPE>" & MessageType & "</MESSAGE_TYPE>"
                    strSoap = strSoap & "<PACS_SPS_ID>" & PacsId & "</PACS_SPS_ID>"
                    strSoap = strSoap & "</Insert_HIS></soap:Body></soap:Envelope>"

                    Dim objHttp
                    objHttp = Server.CreateObject("Msxml2.ServerXMLHTTP")
                    objHttp.Open("POST", strWsdl, False)
                    objHttp.setRequestHeader("Man", "POST " & strWsdl & " HTTP/1.1")
                    objHttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8")
                    objHttp.setRequestHeader("SOAPAction", strSOAPAction)
                    objHttp.Send(strSoap)
                    Dim bEjecuto As Boolean
                    If objHttp.status = 200 Then
                        bEjecuto = True
                    Else
                        bEjecuto = False
                    End If


                    If bEjecuto = True Then
                        If objHttp.ResponseXml.text = "OK-WS-ORM" Then
                            oRceImagenesE.ORACLE = "A"
                            oRceImagenesE.X_TIPOMSG = Mensaje
                            oRceImagenesE.T_COD_EMPRESA = Empresa
                            oRceImagenesE.T_COD_SUCURSAL = Sucursal
                            oRceImagenesE.T_EVENT_ID = oRceImagenesE.CodPresotor
                            oRceImagenesE.T_EVENT_DATETIME = EventDateTimeSQL
                            oRceImagenesE.T_EVENT_TYPE_ID = EventTypeID
                            oRceImagenesE.X_ID_PACIENTE = Trim(CodPaciente)
                            oRceImagenesE.X_RUT_PACIENTE = Trim(DniPaciente)
                            oRceImagenesE.X_TIPO_PACIENTE = Trim(TipoPaciente)
                            oRceImagenesE.X_DEATH_INDICATOR = "N"
                            oRceImagenesE.X_CAT_NAME = Session(sCodUser)
                            oRceImagenesE.X_LAST_NAME = Trim(Apellidos)
                            oRceImagenesE.X_FIRST_NAME = Trim(Nombre)
                            oRceImagenesE.X_BIRTH_DATE = BirthDay
                            oRceImagenesE.X_GENDER_KEY = Trim(Sexo)
                            oRceImagenesE.X_LAST_UPDATED = LastUpdate
                            oRceImagenesE.X_STREET_ADDRESS = Trim(Direccion)
                            oRceImagenesE.X_CITY = Ciudad
                            oRceImagenesE.X_COUNTRY = Pais
                            oRceImagenesE.X_PHONE_NUMBER = Trim(PhoneNumber)
                            oRceImagenesE.X_VISIT_NUMBER = VisitNumber
                            oRceImagenesE.X_START_DATETIME = StartDateTime
                            oRceImagenesE.X_DURATION = Duracion
                            oRceImagenesE.X_STATUS_KEY = Trim(StatusKey)
                            oRceImagenesE.X_STATUS = Trim(Status)
                            oRceImagenesE.X_PROCEDURE_CODE = Trim(ProcedureCode)
                            oRceImagenesE.X_PROCEDURE_DESCRIPTION = Trim(ProcedureDesc)
                            oRceImagenesE.X_ROOM_CODE = RoomCode
                            oRceImagenesE.X_REQUESTED_BY = Trim(RequestBy)
                            oRceImagenesE.X_MESSAGE_TYPE = Trim(MessageType)
                            oRceImagenesE.X_PACS_SPS_ID = Trim(PacsId)
                            oRceImagenLN.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE)
                            Return True
                        Else
                            oRceImagenesE.ORACLE = "X"
                            oRceImagenesE.X_TIPOMSG = Mensaje
                            oRceImagenesE.T_COD_EMPRESA = Empresa
                            oRceImagenesE.T_COD_SUCURSAL = Sucursal
                            oRceImagenesE.T_EVENT_ID = oRceImagenesE.CodPresotor
                            oRceImagenesE.T_EVENT_DATETIME = EventDateTimeSQL
                            oRceImagenesE.T_EVENT_TYPE_ID = EventTypeID
                            oRceImagenesE.X_ID_PACIENTE = Trim(CodPaciente)
                            oRceImagenesE.X_RUT_PACIENTE = Trim(DniPaciente)
                            oRceImagenesE.X_TIPO_PACIENTE = Trim(TipoPaciente)
                            oRceImagenesE.X_DEATH_INDICATOR = "N"
                            oRceImagenesE.X_CAT_NAME = Session(sCodUser)
                            oRceImagenesE.X_LAST_NAME = Trim(Apellidos)
                            oRceImagenesE.X_FIRST_NAME = Trim(Nombre)
                            oRceImagenesE.X_BIRTH_DATE = BirthDay
                            oRceImagenesE.X_GENDER_KEY = Trim(Sexo)
                            oRceImagenesE.X_LAST_UPDATED = LastUpdate
                            oRceImagenesE.X_STREET_ADDRESS = Trim(Direccion)
                            oRceImagenesE.X_CITY = Ciudad
                            oRceImagenesE.X_COUNTRY = Pais
                            oRceImagenesE.X_PHONE_NUMBER = Trim(PhoneNumber)
                            oRceImagenesE.X_VISIT_NUMBER = VisitNumber
                            oRceImagenesE.X_START_DATETIME = StartDateTime
                            oRceImagenesE.X_DURATION = Duracion
                            oRceImagenesE.X_STATUS_KEY = Trim(StatusKey)
                            oRceImagenesE.X_STATUS = Trim(Status)
                            oRceImagenesE.X_PROCEDURE_CODE = Trim(ProcedureCode)
                            oRceImagenesE.X_PROCEDURE_DESCRIPTION = Trim(ProcedureDesc)
                            oRceImagenesE.X_ROOM_CODE = RoomCode
                            oRceImagenesE.X_REQUESTED_BY = Trim(RequestBy)
                            oRceImagenesE.X_MESSAGE_TYPE = Trim(MessageType)
                            oRceImagenesE.X_PACS_SPS_ID = Trim(PacsId)
                            oRceImagenLN.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE)
                            Return True
                        End If
                    Else
                        oRceImagenesE.ORACLE = "X"
                        oRceImagenesE.X_TIPOMSG = Mensaje
                        oRceImagenesE.T_COD_EMPRESA = Empresa
                        oRceImagenesE.T_COD_SUCURSAL = Sucursal
                        oRceImagenesE.T_EVENT_ID = oRceImagenesE.CodPresotor
                        oRceImagenesE.T_EVENT_DATETIME = EventDateTimeSQL
                        oRceImagenesE.T_EVENT_TYPE_ID = EventTypeID
                        oRceImagenesE.X_ID_PACIENTE = Trim(CodPaciente)
                        oRceImagenesE.X_RUT_PACIENTE = Trim(DniPaciente)
                        oRceImagenesE.X_TIPO_PACIENTE = Trim(TipoPaciente)
                        oRceImagenesE.X_DEATH_INDICATOR = "N"
                        oRceImagenesE.X_CAT_NAME = Session(sCodUser)
                        oRceImagenesE.X_LAST_NAME = Trim(Apellidos)
                        oRceImagenesE.X_FIRST_NAME = Trim(Nombre)
                        oRceImagenesE.X_BIRTH_DATE = BirthDay
                        oRceImagenesE.X_GENDER_KEY = Trim(Sexo)
                        oRceImagenesE.X_LAST_UPDATED = LastUpdate
                        oRceImagenesE.X_STREET_ADDRESS = Trim(Direccion)
                        oRceImagenesE.X_CITY = Ciudad
                        oRceImagenesE.X_COUNTRY = Pais
                        oRceImagenesE.X_PHONE_NUMBER = Trim(PhoneNumber)
                        oRceImagenesE.X_VISIT_NUMBER = VisitNumber
                        oRceImagenesE.X_START_DATETIME = StartDateTime
                        oRceImagenesE.X_DURATION = Duracion
                        oRceImagenesE.X_STATUS_KEY = Trim(StatusKey)
                        oRceImagenesE.X_STATUS = Trim(Status)
                        oRceImagenesE.X_PROCEDURE_CODE = Trim(ProcedureCode)
                        oRceImagenesE.X_PROCEDURE_DESCRIPTION = Trim(ProcedureDesc)
                        oRceImagenesE.X_ROOM_CODE = RoomCode
                        oRceImagenesE.X_REQUESTED_BY = Trim(RequestBy)
                        oRceImagenesE.X_MESSAGE_TYPE = Trim(MessageType)
                        oRceImagenesE.X_PACS_SPS_ID = Trim(PacsId)
                        oRceImagenLN.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE)
                        Return True
                    End If
                    'objHttp.ResponseXml
                Else
                    'NO TIENE SALA ASIGNADA PARA ESTE EXAMEN
                End If
            End If

        End If
    End Function


    'Protected Sub btnEnviarSolicitud_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviarSolicitud.Click
    '    EnviarSolicitudPetitorio_(hfValoresCheck.Value.Trim(), hfObservacionPetitorio.Value.Trim())
    'End Sub


    Public Sub OrdenImagen(ByVal atencion As String, ByVal id_recetacab As Integer, ByVal pPresotor As String)
        'select * from rce_receta_imagen_det where ide_recetacab = " & pIde & ""
        Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString 'JB - 21/11/2017

        Dim cn1 As New SqlConnection(CnnBD)
        Dim cmd1 As New SqlCommand("select * from rce_receta_imagen_det where ide_recetacab=@ide_receta_cab", cn1)
        cmd1.Parameters.AddWithValue("@ide_receta_cab", id_recetacab)
        cmd1.CommandType = System.Data.CommandType.Text
        cn1.Open()
        Dim tabla1 As New DataTable()
        Dim da1 As New SqlDataAdapter(cmd1)
        da1.Fill(tabla1)
        cn1.Close()

        If tabla1.Rows.Count > 0 Then
            Dim tabla As New DataTable()
            tabla.Columns.Add("ide_recetacab1")
            tabla.Columns.Add("ide_recetadet1")
            tabla.Columns.Add("codprestacion")
            tabla.Columns.Add("codmedico")
            tabla.Columns.Add("codpresotor")
            tabla.Columns.Add("dsc_imagen")
            tabla.Columns.Add("fec_registra")
            tabla.Columns.Add("cod_atencion")
            tabla.Columns.Add("paciente")
            tabla.Columns.Add("edad")
            tabla.Columns.Add("cama")
            tabla.Columns.Add("fechaing")
            tabla.Columns.Add("cmp")
            tabla.Columns.Add("medico")
            tabla.Columns.Add("diagnostico")
            Dim columna_firma As DataColumn = New DataColumn("firma")
            columna_firma.DataType = System.Type.GetType("System.Byte[]")
            tabla.Columns.Add(columna_firma)
            tabla.Columns.Add("indicaciones")
            tabla.Columns.Add("tipo")
            tabla.Columns.Add("codespecialidad")
            tabla.Columns.Add("colegiomedico")
            tabla.Columns.Add("nomespecialidad")

            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_HCEImagenes3", cn)
            Dim TablaPrev As New DataTable()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", atencion)  'atencion    E0406487
            cmd.Parameters.AddWithValue("@iderecetacab", id_recetacab)  'id_recetacab   27540
            cmd.Parameters.AddWithValue("@codpresotor", "")
            cmd.Parameters.AddWithValue("@orden", 4) 'cambiar a orden 2     0
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)

            For index = 0 To tabla.Rows.Count - 1
                Dim tabla2 As New DataTable()
                tabla2.Columns.Add("ide_recetacab1")
                tabla2.Columns.Add("ide_recetadet1")
                tabla2.Columns.Add("codprestacion")
                tabla2.Columns.Add("codmedico")
                tabla2.Columns.Add("codpresotor")
                tabla2.Columns.Add("dsc_imagen")
                tabla2.Columns.Add("fec_registra")
                tabla2.Columns.Add("cod_atencion")
                tabla2.Columns.Add("paciente")
                tabla2.Columns.Add("edad")
                tabla2.Columns.Add("cama")
                tabla2.Columns.Add("fechaing")
                tabla2.Columns.Add("cmp")
                tabla2.Columns.Add("medico")
                tabla2.Columns.Add("diagnostico")
                Dim columna_firma1 As DataColumn = New DataColumn("firma")
                columna_firma1.DataType = System.Type.GetType("System.Byte[]")
                tabla2.Columns.Add(columna_firma1)
                tabla2.Columns.Add("indicaciones")
                tabla2.Columns.Add("tipo")
                tabla2.Columns.Add("codespecialidad")
                tabla2.Columns.Add("colegiomedico")
                tabla2.Columns.Add("nomespecialidad")
                tabla2.ImportRow(tabla.Rows(index))

                Dim crystalreport As New ReportDocument()
                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpOrdenImagen.rpt"))
                crystalreport.SetDataSource(tabla2)

                Dim xNombreArchivo As String = ""
                Dim xRuta As String = "" 'ConfigurationManager.AppSettings("RutaOrdenImagen").Trim()

                'INICIO - JB - 13/07/2020 - NUEVO CODIGO PARA OBTENER LA RUTA DONDE SE ALMACENARA LOS ARCHIVOS
                Dim oTablasE As New TablasE()
                Dim oTablasLN As New TablasLN()
                oTablasE.CodTabla = "CI_GENERAR_JPG"
                oTablasE.Buscar = ""
                oTablasE.Key = 0
                oTablasE.NumeroLineas = 0
                oTablasE.Orden = 5
                Dim tabla_ As New DataTable()
                Dim DescripcionMotivo As String = ""
                tabla_ = oTablasLN.Sp_Tablas_Consulta(oTablasE)
                xRuta = tabla_.Rows(0)("nombre").ToString().Trim() 'C:\TEMP\
                'FIN - JB - 13/07/2020 - NUEVO CODIGO PARA OBTENER LA RUTA DONDE SE ALMACENARA LOS ARCHIVOS

                'PREPERANDO EXPORTACION DE REPORTE A PDF
                Dim OpcionExportar As ExportOptions
                Dim OpcionDestino As New DiskFileDestinationOptions()
                Dim OpcionesFormato As New PdfRtfWordFormatOptions()
                xNombreArchivo = Session(sCodigoAtencion) + "_" + tabla2.Rows(0)("ide_recetadet1").ToString().Trim() + ".pdf" 'cod_presotor (index + 1).ToString() 
                OpcionDestino.DiskFileName = xRuta + xNombreArchivo
                OpcionExportar = crystalreport.ExportOptions
                With OpcionExportar
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                    .DestinationOptions = OpcionDestino
                    .FormatOptions = OpcionesFormato
                End With
                'EXPORTANDO A PDF
                crystalreport.Export()
                'CONVIRTIENDO ARCHIVO PDF GENERADO EN BYTE()
                Dim pdf_byte As Byte() = System.IO.File.ReadAllBytes(xRuta + xNombreArchivo)
                System.IO.File.WriteAllBytes(xRuta + xNombreArchivo, pdf_byte)

                'INICIO - JB - 13/07/2020 - CONVIRTIENDO PDF A JPG
                Dim doc As New PdfDocument()
                doc.LoadFromFile(xRuta + xNombreArchivo)
                Dim jpg As Image = doc.SaveAsImage(0)
                jpg.Save(xRuta + "OI_" + tabla2.Rows(0)("ide_recetadet1").ToString().Trim() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)

                System.IO.File.Delete(xRuta + xNombreArchivo)
                '        Dim img_byte1 As Byte() = System.IO.File.ReadAllBytes(xRuta1 + "\" + "OI_" + pCodImagen + ".jpg")
                'FIN - JB - 13/07/2020 - CONVIRTIENDO PDF A JPG


                crystalreport.Close()
                crystalreport.Dispose()
            Next


        End If

    End Sub

End Class