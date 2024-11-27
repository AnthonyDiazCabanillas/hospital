' ***************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2024. Todos los derechos reservados.
'    Version  Fecha       Autor       Requerimiento   Objetivo específico
'    1.0      05/11/2024  CRODRIGUEZ  REQ 2024-023820 Informe repetido en SIC
'****************************************************************************************
Imports System.Data
Imports System.IO
Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN
Imports Entidades.EvolucionE 'TMACASSI 25/10/2016
Imports LogicaNegocio.EvolucionLN 'TMACASSI 25/10/2016
Imports System.Data.SqlClient
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports

Public Class PetitorioLaboratorio
    Inherits System.Web.UI.Page

    Dim oRceEvolucionE As New RceEvolucionE() 'TMACASSI 25/10/2016
    Dim oRceEvolucionLN As New RceEvolucionLN() 'TMACASSI 25/10/2016
    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Response.Redirect("ConsultaPacienteHospitalizado.aspx")
        End If
        If Not Page.IsPostBack() Then
            ListaPetitorioGeneral_()
        End If
    End Sub


    Public Sub ListaPetitorioGeneral_()
        Dim tabs As String = ""
        Dim estructura As String = ""
        Dim tabla1, tabla2 As New DataTable()
        oRceLaboratioE.Orden = 3
        oRceLaboratioE.TipoDeAtencion = "A" 'Session(sTipoAtencion) se usara tipo "A" - 05/08/2019
        tabla1 = oRceLaboratorioLN.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE)
        Dim ContadorFila As Integer = 0
        Dim CantidadColumna As Integer = 38 'TMACASSI 05/09/2016
        Dim wTabla As Integer = 250         'TMACASSI 05/09/2016

        tabs += "<div class='JSBTABS'>"
        tabs += "<label for='chkTABS' class='JSBMOSTRAR_TABS'></label>"
        tabs += "<input type='checkbox' id='chkTABS' class='chkTAB-CHECK' />"
        tabs += "<ul>"

        For index = 0 To tabla1.Rows.Count - 1
            estructura += "<div class='JCUERPO' style='float:none;font-size:0.88em;'>"
            estructura += "<div class='JFILA JDIVCHECK' style='overflow:auto;width:1234px;'>"
            estructura += "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA

            oRceLaboratioE.TipoDeAtencion = "A" 'Session(sTipoAtencion) se usara tipo "A" - 05/08/2019
            oRceLaboratioE.IdlaboratorioTitulo = tabla1.Rows(index)("ide_laboratorio_titulo").ToString().Trim()
            oRceLaboratioE.Orden = 4
            tabla2 = oRceLaboratorioLN.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE)

            For index1 = 0 To tabla2.Rows.Count - 1

                If tabla2.Rows(index1)("ide_analisis").ToString().Trim() <> tabla2.Rows(index1)("ide_laboratorio_titulo").ToString().Trim() Then 'cuenta todo menos los tabs
                    ContadorFila += 1
                End If

                If tabla2.Rows(index1)("ide_analisis").ToString().Trim() = tabla2.Rows(index1)("ide_laboratorio_titulo").ToString().Trim() Then
                    If index = 0 Then 'TMACASSI 07/09/2016 Se cambia de index1 a index
                        tabs += "<li class='JSBTAB_ACTIVO'>"
                    Else
                        tabs += "<li>"
                    End If

                    tabs += "<a>" + tabla2.Rows(index1)("dsc_analisis").ToString().Trim() + "</a>"
                    tabs += "</li>"
                ElseIf tabla2.Rows(index1)("ide_analisis").ToString().Trim() = tabla2.Rows(index1)("ide_analisis_padre").ToString().Trim() Then
                    estructura += "<div class='JFILA'>"
                    estructura += "<div class='JCELDAPETITORIO-12'>"
                    estructura += "<span class='JETIQUETA_5'>" + tabla2.Rows(index1)("dsc_analisis").ToString().Trim() + "</span>"
                    estructura += "</div>"
                    estructura += "</div>"
                Else
                    estructura += "<div class='JFILA'>"
                    estructura += "<div class='JCELDAPETITORIO-12'>"
                    If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                        estructura += "<input type='checkbox' disabled='disabled' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_analisis").ToString().Trim() + "</span>"
                    Else
                        If tabla2.Rows(index1)("perfil").ToString().Trim() <> "" Then
                            estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis").ToString().Trim() + "' name='" + tabla2.Rows(index1)("perfil").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_analisis").ToString().Trim() + "</span>"
                        Else
                            estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis").ToString().Trim() + "' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_analisis").ToString().Trim() + "</span>"
                        End If
                    End If

                    If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                        estructura += "<div style='float:right;overflow:hidden;'>"
                    End If
                    If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Then
                        estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis_sub1").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis_sub1").ToString().Trim() + "' name='0' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() + "</span>"
                    End If
                    If tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Then
                        estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis_sub2").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis_sub2").ToString().Trim() + "' name='0' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() + "</span>"
                    End If
                    If tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                        estructura += "<input type='checkbox' id='chkAnalisis_" + tabla2.Rows(index1)("ide_analisis_sub3").ToString().Trim() + "' value='" + tabla2.Rows(index1)("ide_analisis_sub3").ToString().Trim() + "' name='0' /><span class='JETIQUETA_4'>" + tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() + "</span>"
                    End If
                    If tabla2.Rows(index1)("dsc_tipoanalisis1").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis2").ToString().Trim() <> "" Or tabla2.Rows(index1)("dsc_tipoanalisis3").ToString().Trim() <> "" Then
                        estructura += "</div>"
                    End If

                    estructura += "</div>"
                    estructura += "</div>"
                End If
                If ContadorFila = CantidadColumna Then
                    estructura += "</div>" 'CIERRA COLUMNA
                    estructura += "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA
                    ContadorFila = 0
                End If

                If ContadorFila <> CantidadColumna And index1 = tabla2.Rows.Count - 1 Then
                    estructura += "</div>" 'CIERRA COLUMNA
                End If

            Next
            ContadorFila = 0
            Dim widthTabla As Integer
            Dim rMod As Integer
            widthTabla = ((Math.Ceiling(tabla2.Rows.Count / CantidadColumna) * wTabla))
            rMod = tabla2.Rows.Count Mod CantidadColumna

            'SI ES MENOR A LA CANTIDAD DE FILAS
            If tabla2.Rows.Count < CantidadColumna Then
                estructura = estructura.Replace("width:1234", ("width:" + widthTabla.ToString()))
            ElseIf rMod = 1 Then
                estructura = estructura.Replace("width:1234", ("width:" + (((tabla2.Rows.Count / CantidadColumna) * wTabla)).ToString()))
            Else
                estructura = estructura.Replace("width:1234", ("width:" + ((Math.Ceiling(tabla2.Rows.Count / CantidadColumna) * wTabla)).ToString()))
            End If
            'estructura = estructura.Replace("width:1234", ("width:" + ((Math.Ceiling(tabla2.Rows.Count / CantidadColumna) * wTabla)).ToString()))
            'estructura = estructura.Replace("width:1234", ("width:" + (((tabla2.Rows.Count / CantidadColumna) * wTabla)).ToString()))
            estructura += "</div>"
            estructura += "</div>"
        Next

        tabs += "</ul>"
        tabs += "</div>"

        'Return tabs + estructura
        divContenedorPetitorio.InnerHtml = tabs + estructura
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function EnviarSolicitudPetitorio(ByVal CodigosPetitorioLaboratorio As String, ByVal Descripcion As String, ByVal Fecha As String, ByVal Hora As String, ByVal CodigoMarcado As String) As String
        Dim pagina As New PetitorioLaboratorio()
        Return pagina.EnviarSolicitudPetitorio_(CodigosPetitorioLaboratorio, Descripcion, Fecha, Hora, CodigoMarcado)
    End Function


    Public Function EnviarSolicitudPetitorio_(ByVal CodigosPetitorioLaboratorio As String, ByVal Descripcion As String, ByVal Fecha As String, ByVal Hora As String, ByVal CodigoMarcado As String) As String
        Try
            Dim IdeRecetaCabA As Integer = 0
            Dim IdeRecetaCabG As Integer = 0
            For index = 0 To CodigosPetitorioLaboratorio.Trim().Split(";").Length - 1
                Dim EstadoAnalisis As String = "A"
                Dim oRceLaboratioE_ As New RceLaboratioE
                Dim oRceLaboratorioLN_ As New RceLaboratorioLN
                oRceLaboratioE_.Orden = 10
                oRceLaboratioE_.Nombre = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
                oRceLaboratioE_.CodMedico = Session(sCodMedico)
                oRceLaboratioE_.TipoDeAtencion = "A"
                Dim tablax1 As New DataTable()
                tablax1 = oRceLaboratorioLN.Sp_RceBuscar_Consulta(oRceLaboratioE_)
                If tablax1.Rows.Count > 0 Then
                    EstadoAnalisis = "G"
                End If

                If EstadoAnalisis = "A" Then
                    If IdeRecetaCabA = 0 Then 'se insertara nueva cabecera para este estado
                        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
                        oRceLaboratioE.CodMedico = Session(sCodMedico)
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.DscReceta = Descripcion.Trim().ToUpper()
                        oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis

                        If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE) Then
                            IdeRecetaCabA = oRceLaboratioE.IdeRecetaCab
                            oRceLaboratioE.IdAnalisisLaboratorio = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
                            oRceLaboratioE.EstAnalisis = EstadoAnalisis
                            oRceLaboratioE.UsrRegistra = Session(sCodUser)
                            oRceLaboratioE.FlgCubierto = True
                            oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)
                        End If
                    Else 'se usara el id de la cabecera que ya se inserto
                        IdeRecetaCabA = oRceLaboratioE.IdeRecetaCab
                        oRceLaboratioE.IdAnalisisLaboratorio = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.FlgCubierto = True
                        oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)
                    End If

                    If Fecha <> "" Then 'And Hora <> ""
                        For index1 = 0 To Session("TablaAnalisisMarcados").Split(";").Length - 1
                            If CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim() = Session("TablaAnalisisMarcados").Split(";")(index1).ToString().Trim() Then
                                oRceLaboratioE.Campo = "fec_programado"
                                oRceLaboratioE.ValorNuevo = Format(Convert.ToDateTime((Fecha.Trim() + " " + Hora.Trim())), "MM/dd/yyyy HH:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
                                oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
                            End If
                        Next
                    End If
                Else
                    If IdeRecetaCabG = 0 Then 'se insertara nueva cabecera para este estado
                        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
                        oRceLaboratioE.CodMedico = Session(sCodMedico)
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.DscReceta = Descripcion.Trim().ToUpper()
                        oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis

                        If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE) Then
                            IdeRecetaCabG = oRceLaboratioE.IdeRecetaCab
                            oRceLaboratioE.IdAnalisisLaboratorio = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
                            oRceLaboratioE.EstAnalisis = EstadoAnalisis
                            oRceLaboratioE.UsrRegistra = Session(sCodUser)
                            oRceLaboratioE.FlgCubierto = True
                            oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)
                        End If
                    Else 'se usara el id de la cabecera que ya se inserto
                        IdeRecetaCabG = oRceLaboratioE.IdeRecetaCab
                        oRceLaboratioE.IdAnalisisLaboratorio = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.FlgCubierto = True
                        oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)
                    End If

                    If Fecha <> "" Then 'And Hora <> ""
                        For index1 = 0 To Session("TablaAnalisisMarcados").Split(";").Length - 1
                            If CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim() = Session("TablaAnalisisMarcados").Split(";")(index1).ToString().Trim() Then
                                oRceLaboratioE.Campo = "fec_programado"
                                oRceLaboratioE.ValorNuevo = Format(Convert.ToDateTime((Fecha.Trim() + " " + Hora.Trim())), "MM/dd/yyyy HH:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
                                oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
                            End If
                        Next
                    End If
                End If
            Next

            If IdeRecetaCabA <> 0 Then
                GuardarLogLab(IdeRecetaCabA)
            End If
            If IdeRecetaCabG <> 0 Then
                GuardarLogLab(IdeRecetaCabG)
            End If



            ''GUARDANDO CABECERA ANALISIS
            'If oRceLaboratioE.IdeRecetaCab = 0 Then
            '    oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
            '    oRceLaboratioE.CodMedico = Session(sCodMedico)
            '    oRceLaboratioE.UsrRegistra = Session(sCodUser)
            '    oRceLaboratioE.DscReceta = Descripcion.Trim().ToUpper()
            '    oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
            '    oRceLaboratioE.EstAnalisis = "A" 'JB - 13/07/2020 - SE CAMBIA DE ESTADO G -> A
            '    oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE)
            'End If

            'For index = 0 To CodigosPetitorioLaboratorio.Trim().Split(";").Length - 1
            '    If oRceLaboratioE.IdeRecetaCab <> 0 Then
            '        'INICIO - JB - NUEVO CODIGO - 27/08/2020
            '        Dim EstadoAnalisis As String = "A"
            '        Dim oRceLaboratioE_ As New RceLaboratioE
            '        Dim oRceLaboratorioLN_ As New RceLaboratorioLN
            '        oRceLaboratioE_.Orden = 10
            '        oRceLaboratioE_.Nombre = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
            '        oRceLaboratioE_.CodMedico = Session(sCodMedico)
            '        oRceLaboratioE_.TipoDeAtencion = "A"
            '        Dim tablax1 As New DataTable()
            '        tablax1 = oRceLaboratorioLN.Sp_RceBuscar_Consulta(oRceLaboratioE_)
            '        If tablax1.Rows.Count > 0 Then
            '            EstadoAnalisis = "G"
            '        End If
            '        'FIN - JB - NUEVO CODIGO - 27/08/2020

            '        oRceLaboratioE.IdAnalisisLaboratorio = CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim()
            '        oRceLaboratioE.EstAnalisis = EstadoAnalisis '"A" -> 27/08/2020 cambia de valor fijo 'a' a variable EstadoAnalisis 'JB - 13/07/2020 - SE CAMBIA DE ESTADO G -> A
            '        oRceLaboratioE.UsrRegistra = Session(sCodUser)
            '        oRceLaboratioE.FlgCubierto = True 'JB - 31/07/2020 - SE LE ASIGNA VALOR 1/TRUE
            '        oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)

            '        'INICIO - 19/01/2017
            '        If Fecha <> "" Then 'And Hora <> ""
            '            For index1 = 0 To Session("TablaAnalisisMarcados").Split(";").Length - 1
            '                If CodigosPetitorioLaboratorio.Trim().Split(";")(index).Trim() = Session("TablaAnalisisMarcados").Split(";")(index1).ToString().Trim() Then
            '                    oRceLaboratioE.Campo = "fec_programado"
            '                    oRceLaboratioE.ValorNuevo = Format(Convert.ToDateTime((Fecha.Trim() + " " + Hora.Trim())), "MM/dd/yyyy HH:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
            '                    oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
            '                End If
            '            Next
            '        End If
            '        'FIN - 19/01/2017
            '    End If
            'Next


            ''<I-TMACASSI> 25/10/2016
            'oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
            'oRceEvolucionE.IdeOrdenCab = oRceLaboratioE.IdeRecetaCab 'Session(sIdRecetaCab)
            'oRceEvolucionE.Orden = 2
            'oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

            'If oRceEvolucionE.CodigoEvolucion <> 0 Then
            '    'INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            '    Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

            '    Dim pdf_byte As Byte() = ExportaPDF("DA")
            '    Dim oHospitalE As New HospitalE()
            '    Dim oHospitalLN As New HospitalLN()
            '    Dim cn As New SqlConnection(CnnBD)
            '    'Paso 1
            '    oHospitalE.TipoDoc = 10
            '    oHospitalE.CodAtencion = Session(sCodigoAtencion)
            '    oHospitalE.CodUser = Session(sCodUser)
            '    oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
            '    oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            '    'Paso 2
            '    Dim cmd2 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
            '    cmd2.CommandType = CommandType.Text
            '    cmd2.Parameters.AddWithValue("@bib_documento", pdf_byte)
            '    cmd2.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            '    cmd2.Parameters.AddWithValue("@flg_reqfirma", "0")

            '    Dim num1 As Integer
            '    cn.Open()
            '    num1 = cmd2.ExecuteNonQuery()
            '    cn.Close()

            '    'Paso 3
            '    oHospitalE.IdeHistoria = Session(sIdeHistoria)
            '    oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
            '    oHospitalE.TipoDoc = 10
            '    oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
            '    'FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'Else
            '    Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
            'End If
            ''<F-TMACASSI> 25/10/2016
            ''INICIO - 20/01/2017
            'Session.Remove("TablaAnalisisMarcados")
            'Session.Remove("TablaPetitorio")
            ''FIN - 20/01/2017

            Return "OK"
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GuardarLogLab(ByVal IdeRecetaP As Integer) As String
        '<I-TMACASSI> 25/10/2016
        oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
        oRceEvolucionE.IdeOrdenCab = IdeRecetaP
        oRceEvolucionE.Orden = 2
        oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

        If oRceEvolucionE.CodigoEvolucion <> 0 Then
            'INI 1.0
            ''INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

            'Dim pdf_byte As Byte() = ExportaPDF("DA")
            'Dim oHospitalE As New HospitalE()
            'Dim oHospitalLN As New HospitalLN()
            'Dim cn As New SqlConnection(CnnBD)
            ''Paso 1
            'oHospitalE.TipoDoc = 10
            'oHospitalE.CodAtencion = Session(sCodigoAtencion)
            'oHospitalE.CodUser = Session(sCodUser)
            'oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
            'oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            ''Paso 2
            'Dim cmd2 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
            'cmd2.CommandType = CommandType.Text
            'cmd2.Parameters.AddWithValue("@bib_documento", pdf_byte)
            'cmd2.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            'cmd2.Parameters.AddWithValue("@flg_reqfirma", "0")

            'Dim num1 As Integer
            'cn.Open()
            'num1 = cmd2.ExecuteNonQuery()
            'cn.Close()

            ''Paso 3
            'oHospitalE.IdeHistoria = Session(sIdeHistoria)
            'oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
            'oHospitalE.TipoDoc = 10
            'oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
            ''FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'FIN 1.0
        Else
            Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
        End If
        '<F-TMACASSI> 25/10/2016
        'INICIO - 20/01/2017
        Session.Remove("TablaAnalisisMarcados")
        Session.Remove("TablaPetitorio")
        'FIN - 20/01/2017
        Return "OK"
    End Function


    'INICIO - JB -31/01/2017
    Public Function ExportaPDF(ByVal Tipo As String) As Byte()
        Dim pdf_byte As Byte() = Nothing
        Dim xRuta As String = sRutaTemp
        Dim crystalreport As New ReportDocument()

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
            Dim distinctValues = tabla.AsEnumerable().[Select](Function(row) New With { _
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
        System.IO.File.Delete(xRuta + "\" + NombreArchivo)
        'System.IO.File.WriteAllBytes(xRuta + "\" + NombreArchivo, pdf_byte)

        crystalreport.Close()
        crystalreport.Dispose()

        Return pdf_byte
    End Function
    'FIN - JB - 31/01/2017

    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarPerfil(ByVal Codigo As String) As String
        Dim pagina As New PetitorioLaboratorio()
        Return pagina.VerificarPerfil_(Codigo)
    End Function

    Public Function VerificarPerfil_(ByVal Codigo As String) As String
        Try
            Dim tabla As New DataTable()
            oRceLaboratioE.IdAnalisisLaboratorio = Codigo
            oRceLaboratioE.Orden = 5
            oRceLaboratioE.TipoDeAtencion = "A" 'Session(sTipoAtencion) se usara tipo "A" - 05/08/2019
            tabla = oRceLaboratorioLN.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE)
            Dim CheckActivar As String = ""

            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    CheckActivar += tabla.Rows(index)("ide_analisis").ToString().Trim() + ";"
                Next
            End If
            Return CheckActivar
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSession() As String
        Dim pagina As New PetitorioLaboratorio()
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
End Class