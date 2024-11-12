' ***************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2024. Todos los derechos reservados.
'    Version  Fecha       Autor       Requerimiento   Objetivo específico
'    1.0      15/01/2024  CRODRIGUEZ  REQ 2023-012268 Filtrar envío correos para interconsultas
'    1.1      02/02/2024  CRODRIGUEZ  REQ 2023-021287 Se envia correo al agregar o cambiar dieta del paciente 
'    1.2      29/04/2024  GLluncor    REQ 2024-008691 - Actualizar URL ROE
'    1.3      19/06/2024  FGUEVARA    REQ-2024-011009 RESULTADOS ROE - HC
'    1.4      31/10/2024  CRODRIGUEZ  REQ 2024-023820 Informe repetido en SIC
'****************************************************************************************
Imports System.Data
Imports System.IO
Imports Entidades.AnamnesisE
Imports System.Web.Services
Imports LogicaNegocio.AnamnesisLN
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.NotaIngresoE
Imports LogicaNegocio.NotaIngresoLN
Imports Entidades.EvolucionE
Imports LogicaNegocio.EvolucionLN
Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN
Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN
Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN
Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN
Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE
Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN
Imports LogicaNegocio.InicioSesionLN
Imports Entidades.InicioSesionE
Imports LogicaNegocio.MedicamentosLN
Imports Entidades.MedicamentosE
Imports System.Data.SqlClient
Imports System.Globalization
Imports CrystalDecisions.CrystalReports.Engine
'PARA EXPORTAR A PDF
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports
Imports LogicaNegocio.PatologiaLN
Imports Entidades.PatologiaE
Imports Entidades.JuntaMedicaE
Imports LogicaNegocio.JuntaMedicaLN
'FIN EXPORTAR A PDF
Imports RestSharp
Imports LogicaNegocio
Imports Entidades
Imports Newtonsoft.Json

Public Class InformacionPaciente
    Inherits System.Web.UI.Page
    Dim oRceAnamnesisE As New RceAnamnesisE()
    Dim oRceAnamnesisLN As New RceAnamnesisLN()
    Dim oRceAtencionesE As New RceAtencionesE
    Dim oRceAtencionesLN As New RceAtencionesLN()
    Dim oHospitalLN As New HospitalLN()
    Dim oHospitalE As New HospitalE()
    Dim oRceNotaIngresoE As New RceNotaIngresoE()
    Dim oRceNotaIngresoLN As New RceNotaIngresoLN()
    Dim oRceEvolucionE As New RceEvolucionE()
    Dim oRceEvolucionLN As New RceEvolucionLN()
    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()
    Dim oRceDiagnosticoE As New RceDiagnosticoE()
    Dim oRceDiagnosticoLN As New RceDiagnosticoLN()
    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()
    Dim oMedicoE As New MedicoE()
    Dim oMedicoLN As New MedicoLN()
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()
    Dim oRceInicioSesionLN As New RceInicioSesionLN()
    Dim oRceInicioSesionE As New RceInicioSesionE()
    Dim oRcePatologiaMaeE As New RcePatologiaMaeE()
    Dim oRcePatologiaMaeLN As New RcePatologiaMaeLN()
    Dim CodigoFormulario As String = "86"
    Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
    Private Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

    'Dim crystalreport As New ReportDocument() '31/01/2017
    Dim xRuta As String = sRutaTemp

    Dim ListGeneralIndicacionesMedicas As List(Of IndicacionesMedicasE)
    Dim ListIndicacionesCabecera As List(Of IndicacionesMedicasE)
    Dim ListIndicacionesdetalle As List(Of IndicacionesMedicasE)

    Dim ListIndicacionesMedicasGeneral As List(Of IndicacionesMedicaDetalleE)
    Dim ListIndicacionesMedicasFecha As List(Of IndicacionesMedicaDetalleE)
    Dim ListIndicacionesMedicastab As List(Of IndicacionesMedicaDetalleE)
    Dim ListIndicacionesMedicastabdetalle As List(Of IndicacionesMedicaDetalleE)
    Dim ListIndicacionesMedicastabdetallexproducto As List(Of IndicacionesMedicaDetalleE)
    Dim _listEscala As List(Of EscalaEIndicacionesE)
    Dim _Encript As Criptography = New Criptography()



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Response.Redirect("ConsultaPacienteHospitalizado.aspx")
        End If
        If Not Page.IsPostBack Then
            Session.Remove(sTablaMedicamentosSuspender) 'JB - 05/05/2021
            Session.Remove(sTablaProductoMedicamento)
            Session.Remove(sIdRecetaCab)
            Session.Remove(sIdeRecetaImagenCab)

            ObtenerValoresGenerales()
            divUsuarioConexion.InnerHtml = "Usuario: " + Session(sNombreUsuario)
            CargaDatoViaAdministracion()
            ListarInfusiones()
            CargarPatologiasCheck()
            ListarPatologiasSeleccionadas() 'JB

            GuardarAntecedentes()


            'ObtenerDatosIndicaciones()
            ObtenerEscalaEIndicacionesEnfermeria()

            'ArmandoTabPrincipalV2(Session(sCodigoAtencion).ToString().Substring(0, 1))
            'Dim texto As String = ""
            'texto = System.IO.File.ReadAllText("C:\\TEMP\\CuerpoHTML.txt")
            'divContenedorDinamico.InnerHtml = texto

            Dim Estructura As String = ArmandoTabPrincipalV2(Session(sCodigoAtencion).ToString().Substring(0, 1)) 'JB - comentado temporalmente
            divContenedorDinamico.InnerHtml = Estructura 'JB - comentado temporalmente

            If Not IsNothing(Session("AcordeonAbierto")) Then
                hfAcordeonAbierto.Value = Session("AcordeonAbierto")
                Session.Remove("AcordeonAbierto")
            End If
            HistoriaClinica24Horas()


            'If Not IsNothing(Session(sCodEnfermera)) Then 'JB - NUEVO CODIGO - 13/05/2020 - SE DESHABILITA SI ES ADMINISTRATIVO / 11/06/2020 - se reemplazara por codigo lineas abajo
            '    hfAdministrativo.Value = "SI"
            'End If

            hfAdministrativo.Value = Session(sPerfilUsuario)
        End If
    End Sub

    Public Sub GuardarAntecedentes()
        Try
            Dim tabla As New DataTable()
            tabla.Columns.Add("dsc_txtidcampo", System.Type.GetType("System.String"))
            tabla.Columns.Add("valor_resultado_detalle", System.Type.GetType("System.String"))
            tabla.Columns.Add("ide_examenfisicores", System.Type.GetType("System.Int32"))

            oRceAnamnesisE.IdeHistoria = Session(sIdeHistoria)
            oRceAnamnesisE.CodigoUsuario = Session(sCodUser)
            oRceAnamnesisE.IdeTipoAtencion = Session(sCodigoAtencion).ToString().Substring(0, 1)
            oRceAnamnesisE.RceTabla = tabla 'sera una tabla vacia
            oRceAnamnesisE.DscTxtIdCampo = ""
            oRceAnamnesisE.IdeExamenFisico = 0
            oRceAnamnesisE.TxtDetalle = ""
            oRceAnamnesisE.Orden = 3

            Dim i As Integer = oRceAnamnesisLN.Sp_RceResultadoExamenFisicoDet_Insert4(oRceAnamnesisE)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ObtenerEscalaEIndicacionesEnfermeria()
        Try

            'string order, string variable, string val, string val1, string val2, string val3
            Dim order As String = _Encript.EncryptConectionString("1")
            Dim variable As String = _Encript.EncryptConectionString("0")
            Dim val As String = _Encript.EncryptConectionString("")
            Dim val1 As String = _Encript.EncryptConectionString("")
            Dim val2 As String = _Encript.EncryptConectionString("")
            Dim val3 As String = _Encript.EncryptConectionString("")
            Dim rutaApi As String = Apiruta
            Dim _cliente As RestClient = New RestClient(rutaApi + "EscalaEIndicacionesEnfermeria/API/Clinica/ConsultaEscalaEIndicaciones?order=" + order + "&variable=" + variable + "&val=" + val + "&val1=" + val1 + "&val2=" + val2 + "&val3=" + val3)
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _listEscala = JsonConvert.DeserializeObject(Of List(Of EscalaEIndicacionesE))(_Response.Content)

            Dim _escalaEintervencionesTab As List(Of EscalaEIndicacionesE) = (From n In _listEscala Group n By n.groupcab, n.dsc_detalle Into Group Select New EscalaEIndicacionesE With {.groupcab = groupcab, .dsc_detalle = dsc_detalle}).ToList
            lvEscalaeIntervenciones.DataSource = _escalaEintervencionesTab
            lvEscalaeIntervenciones.DataBind()


            lvEscalaeIntervencionedetalle.DataSource = _escalaEintervencionesTab
            lvEscalaeIntervencionedetalle.DataBind()

        Catch ex As Exception
            Dim a As String = ex.Message
        End Try
    End Sub



    'Private Sub ObtenerDatosIndicaciones()
    '    Try
    '
    '        ListGeneralIndicacionesMedicas = New List(Of IndicacionesMedicasE)
    '        Dim codatencio As String = Session(sCodigoAtencion).ToString()
    '        Dim rutaApi As String = Apiruta
    '        Dim _cliente As RestClient = New RestClient(rutaApi + "KardexHospitalizacion/API/Clinica/PacienteIndicacionesMedica?_Key=" + _Encript.EncryptConectionString(codatencio))
    '        Dim _request As RestRequest = New RestRequest()
    '        _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
    '        _request.Method = Method.Get
    '        Dim _Response As RestResponse = _cliente.Execute(_request)
    '        ListGeneralIndicacionesMedicas = JsonConvert.DeserializeObject(Of List(Of IndicacionesMedicasE))(_Response.Content)
    '        ListIndicacionesCabecera = (From n In ListGeneralIndicacionesMedicas Group n By n.IdTipo, n.NombreTipo, n.Icons Into Group Select New IndicacionesMedicasE With {.IdTipo = IdTipo, .NombreTipo = NombreTipo, .Icons = Icons}).ToList
    '
    '        gvlistcabeceraDatosclinico.DataSource = ListIndicacionesCabecera
    '        gvlistcabeceraDatosclinico.DataBind()
    '    Catch ex As Exception
    '        Dim a As String = ex.Message
    '    End Try
    'End Sub


    ''' <summary>
    ''' LA SECCION DE HISTORIA CLINICA SOLO ESTARA DISPONIBLE SOLO POR 24 HORAS PARA SU REGISTRO.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HistoriaClinica24Horas()
        Try

            If Session(sPerfilUsuario) = "MEDICOS" And Session(sCodigoAtencion).ToString().Substring(0, 1) <> "E" Then
                oHospitalE.NombrePaciente = Session(sCodigoAtencion)
                oHospitalE.Pabellon = ""
                oHospitalE.Servicio = ""
                oHospitalE.Orden = 4
                Dim tabla_atenciones As New DataTable()
                tabla_atenciones = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)
                Dim diferencia As String
                diferencia = DateDiff(DateInterval.Minute, Date.Parse(tabla_atenciones.Rows(0)("fec_registra")), Date.Parse(DateTime.Now.ToString()))


                If diferencia > 1440 Then 'si es mayor a un dia
                    hfHistoriaClinicaHoras.Value = diferencia
                Else 'sino sera 0 (como 0 dias de diferencia)
                    hfHistoriaClinicaHoras.Value = 0
                End If


                If Session(sCodMedico) = tabla_atenciones.Rows(0)("cod_medico").ToString() Then
                    hfHistoriaClinicaMedico.Value = "HABILITAR"
                Else
                    hfHistoriaClinicaMedico.Value = "DESHABILITAR"
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub


    Public Sub ObtenerValoresGenerales()
        'LABORATORIO


        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
        oRceLaboratioE.CodMedico = Session(sCodMedico)
        oRceLaboratioE.Orden = 1
        Dim tabla As New DataTable()
        tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_Consulta(oRceLaboratioE)
        If tabla.Rows.Count > 0 Then
            Session(sIdRecetaCab) = CType(tabla.Rows(0)("ide_recetacab").ToString(), Integer)
        End If

        'IMAGENES
        oRceImagenesE.CodAtencion = Session(sCodigoAtencion)
        oRceImagenesE.CodMedico = Session(sCodMedico)
        oRceImagenesE.Orden = 1
        Dim TablaImagenes As New DataTable()
        TablaImagenes = oRceImagenLN.Sp_RceRecetaImagenCab_Consulta(oRceImagenesE)
        If TablaImagenes.Rows.Count > 0 Then
            Session(sIdeRecetaImagenCab) = CType(TablaImagenes.Rows(0)("ide_recetacab").ToString(), Integer)
        End If

        'INTERCONSULTA
        oTablasE.CodTabla = "INTERCONSULTA_MOTIVO"
        oTablasE.Buscar = ""
        oTablasE.Key = 0
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = 5
        Dim tabla_ As New DataTable()
        tabla_ = oTablasLN.Sp_Tablas_Consulta(oTablasE)
        ddlMotivo.DataSource = tabla_
        ddlMotivo.DataTextField = "nombre"
        ddlMotivo.DataValueField = "codigo"
        ddlMotivo.DataBind()
        '"INTERCONSULTA_MOTIVO", "", 0, 0, 5

        'CONTROL CLINICO - CALCULADORA
        Dim TablaFarmacoCalculadora As New DataTable()
        Dim oRceRecetaMedicamentoE1 As New RceRecetaMedicamentoE()
        Dim oRceRecetaMedicamentoLN1 As New RceRecetaMedicamentoLN()
        oRceRecetaMedicamentoE1.Orden = 1
        TablaFarmacoCalculadora = oRceRecetaMedicamentoLN1.Sp_RceFarmacoDosis_Consulta(oRceRecetaMedicamentoE1)
        ddlFarmacoCalculadora.DataSource = TablaFarmacoCalculadora
        ddlFarmacoCalculadora.DataTextField = "dsc_dci"
        ddlFarmacoCalculadora.DataValueField = "id_farmaco_dosis"
        ddlFarmacoCalculadora.DataBind()

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function CargarAtencionesAnteriores() As Array
        Dim pagina As New InformacionPaciente()
        Return pagina.CargarAtencionesAnteriores_()
    End Function

    Public Function CargarAtencionesAnteriores_() As Array
        oHospitalE.NombrePaciente = Session(sCodigoAtencion)
        oHospitalE.Pabellon = ""
        oHospitalE.Servicio = ""
        oHospitalE.Orden = 3
        Dim tabla_atenciones As New DataTable()
        tabla_atenciones = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)

        If tabla_atenciones.Rows.Count() > 0 Then
            Session(sCodPaciente) = tabla_atenciones.Rows(0)("codpaciente").ToString().Trim()
        End If

        Dim tabla As New DataTable()
        Dim Atenciones As String()
        oRceAtencionesE.CodPaciente = Session(sCodPaciente)
        oRceAtencionesE.Orden = 3
        tabla = oRceAtencionesLN.Sp_Hospital_ConsultaV2RCE(oRceAtencionesE)
        Atenciones = New String(tabla.Rows.Count) {}
        For index = 0 To tabla.Rows.Count - 1
            'Atenciones(index) = tabla.Rows(index)("fec_hora_atencion").ToString.Trim() + " - " + tabla.Rows(index)("cod_atencion").ToString.Trim() + " - " + tabla.Rows(index)("dsc_nombres_medico").ToString.Trim() + " - " + tabla.Rows(index)("dsc_diagnostico").ToString.Trim() + "_" + tabla.Rows(index)("ide_hc_rce").ToString.Trim()
            Atenciones(index) = tabla.Rows(index)("cod_atencion").ToString.Trim() + " - " + tabla.Rows(index)("dsc_nombres_medico").ToString.Trim() + " - " + tabla.Rows(index)("dsc_diagnostico").ToString.Trim() + " - " + tabla.Rows(index)("fec_hora_atencion").ToString.Trim() + "_" + tabla.Rows(index)("ide_hc_rce").ToString.Trim()
            'tabla.Rows(index)("cod_atencion").ToString.Trim(+"-" + tabla.Rows(index)("ide_hc_rce").ToString.Trim())
        Next
        Return Atenciones

    End Function


    'VERSION ACTUAL - ARMANDO ESTRUCTURA HTML DINAMICA
    <System.Web.Services.WebMethod()>
    Public Shared Function ControlesDinamicos_2(ByVal TipoAtencion As String) As String
        Dim pagina As New InformacionPaciente()

        Return pagina.ArmandoTabPrincipalV2(TipoAtencion)
    End Function

    Public Function ArmandoTabPrincipal(ByVal TipoDeAtencion As String) As String
        Dim tabla As New DataTable()
        Dim TablaGrupo As New DataTable()
        Dim CadenaHTML As String = ""
        Dim CuerpoTab As String = ""
        Dim TipoAtencion As String = Session(sCodigoAtencion).ToString().Substring(0, 1)
        If TipoDeAtencion.Trim() <> "" Then
            TipoAtencion = TipoDeAtencion
        End If
        If TipoAtencion = "E" Then 'Para atencion E se usara un SP distinto
            Return CargandoControlesDinamicosE()
        End If


        Session(sTipoAtencion) = TipoAtencion
        oRceAnamnesisE.IdeExamenFisico = 0
        oRceAnamnesisE.IdeHistoria = Session(sIdeHistoria)
        oRceAnamnesisE.IdeTipoAtencion = TipoAtencion

        If IsNothing(Session(sCodMedico)) Then
            oRceAnamnesisE.CodMedico = ""
        Else
            oRceAnamnesisE.CodMedico = Session(sCodMedico)
        End If

        'oRceAnamnesisE.CodMedico = Session(sCodMedico)
        oRceAnamnesisE.FlgEstado = "A"
        oRceAnamnesisE.Orden = 1
        tabla = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

        CadenaHTML = "<div class='JSBTABS'><label for='chkTABS' class='JSBMOSTRAR_TABS'></label><input type='checkbox' id='chkTABS' class='chkTAB-CHECK' /><ul>"
        For index = 0 To tabla.Rows.Count() - 1
            If index = 0 Then
                CadenaHTML += "<li class='JSBTAB_ACTIVO'><a>" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "</a></li>"
            Else
                CadenaHTML += "<li><a>" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "</a></li>"
            End If

        Next
        CadenaHTML += "</ul></div>"

        For index = 0 To tabla.Rows.Count() - 1
            '*****************GRUPO************************
            'CARGANDO LOS VALORES DE LOS GRUPOS
            Dim IdExaFisicoPadre As Integer
            Dim resultado As Boolean = Int32.TryParse(tabla.Rows(index)("ide_examenfisico").ToString().Trim(), IdExaFisicoPadre)
            If resultado = False Then
                oRceAnamnesisE.IdeExamenFisico = 0
            Else
                oRceAnamnesisE.IdeExamenFisico = IdExaFisicoPadre
            End If
            oRceAnamnesisE.IdeHistoria = 0
            oRceAnamnesisE.FlgEstado = "A"
            oRceAnamnesisE.Orden = 2
            TablaGrupo = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

            CuerpoTab = "<div class='JCUERPO'>"
            For indice = 0 To TablaGrupo.Rows.Count() - 1
                CuerpoTab += "<div class='JFILA'><div class='JCELDA-12'><div class='JDIV-CONTROLES JDIV-GRUPO' id='" +
                        TablaGrupo.Rows(indice)("ide_examenfisico").ToString().Trim() + "' name='" + TablaGrupo.Rows(indice)("cnt_minmedida").ToString().Trim() + "-" + TablaGrupo.Rows(indice)("est_tipomedida").ToString().Trim() + "'>"
                CuerpoTab += TablaGrupo.Rows(indice)("txt_detalle").ToString().Trim()
                CuerpoTab += "</div></div></div>"
                'CuerpoTab += CuerpoGrupo(TablaGrupo, indice)
            Next
            CuerpoTab += "</div>"
            '***************FIN GRUPO**********************
            CadenaHTML += CuerpoTab
        Next

        Return CadenaHTML

    End Function

    Public Function CargandoControlesDinamicosE() As String
        Dim tabla, tabla2, tabla3, tabla4 As New DataTable()
        Dim oRceExamenfisicoMaeE As New RceAnamnesisE()
        Dim oRceExamenFisicoMaeLN As New RceAnamnesisLN()
        Dim oTablasE As New TablasE()
        Dim oTablasLN As New TablasLN()
        Dim CadenaHTML As String = ""
        Dim TipoAtencion As String = Session(sCodigoAtencion).ToString().Substring(0, 1)
        Dim CantidadColumnasGrupo As Integer
        Dim CantidadFilaV As Integer = 6

        oRceExamenfisicoMaeE.IdeExamenFisico = 0
        oRceExamenfisicoMaeE.IdeHistoria = Session(sIdeHistoria) '**************
        oRceExamenfisicoMaeE.IdeTipoAtencion = TipoAtencion
        If IsNothing(Session(sCodMedico)) Then
            oRceExamenfisicoMaeE.CodMedico = ""
        Else
            oRceExamenfisicoMaeE.CodMedico = Session(sCodMedico)
        End If
        oRceExamenfisicoMaeE.FlgEstado = "A"
        oRceExamenfisicoMaeE.Orden = 1 'se utilizara un orden nuevo para atencion E
        tabla = oRceExamenFisicoMaeLN.Sp_RceExamenFisicoMae_Consulta4(oRceExamenfisicoMaeE)

        CadenaHTML += "<div class='JTABS' style='width:100%;'>"
        If tabla.Rows.Count > 0 Then
            For index = 0 To tabla.Rows.Count - 1
                CadenaHTML += "<input type='radio' id='TabPrincipalNro" + index.ToString() + "' name='TabPrincipal' class='JCHEK-TABS' />"
                CadenaHTML += "<label for='TabPrincipalNro" + index.ToString() + "' class='JTABS-LABEL'>" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "</label>"
            Next
            For index = 0 To tabla.Rows.Count - 1
                CadenaHTML += "<div class='JCONTENIDO-TAB'>" 'inicio html div contenido
                'contenido de los tabs - secciones
                oRceExamenfisicoMaeE.Orden = 2
                oRceExamenfisicoMaeE.IdeExamenFisico = tabla.Rows(index)("ide_examenfisico").ToString().Trim()
                tabla2 = oRceExamenFisicoMaeLN.Sp_RceExamenFisicoMae_Consulta4(oRceExamenfisicoMaeE)
                If tabla2.Rows.Count > 0 Then
                    For index1 = 0 To tabla2.Rows.Count - 1
                        CadenaHTML += "<div class='JFILA'><div class='JCELDA-12'>" 'INICIO DIV *
                        CadenaHTML += "<div class='JDIV-CONTROLES JDIV-GRUPO' id='" + tabla2.Rows(index1)("ide_examenfisico").ToString().Trim() + "' name='" + tabla2.Rows(index1)("cnt_minmedida").ToString().Trim() + "-" + tabla2.Rows(index1)("est_tipomedida").ToString().Trim() + "'>"
                        CadenaHTML += tabla2.Rows(index1)("txt_detalle").ToString().Trim()
                        CadenaHTML += "</div>"
                        CadenaHTML += "</div></div>" 'FIN DIV *
                        CantidadColumnasGrupo = tabla2.Rows(index1)("cnt_minmedida").ToString().Trim()
                        If tabla2.Rows(index1)("est_tipomedida").ToString().Trim() = "G" Then 'TABS
                            'ejecutar orden 3, mandar ide_examenfisico
                        Else 'NO TABS
                            Dim ContadorColumna As Integer = 0
                            oRceExamenfisicoMaeE.Orden = 4
                            oRceExamenfisicoMaeE.IdeExamenFisico = tabla2.Rows(index1)("ide_examenfisico").ToString().Trim()
                            tabla3 = oRceExamenFisicoMaeLN.Sp_RceExamenFisicoMae_Consulta4(oRceExamenfisicoMaeE)
                            CadenaHTML += "<div class='JFILA'>"

                            If tabla3.Rows.Count > 0 Then
                                For index2 = 0 To tabla3.Rows.Count - 1 'cargando controles
                                    If tabla3.Rows(index2)("txt_detalle").ToString().Trim().ToUpper() <> "" And tabla3.Rows(index2)("est_tipomedida").ToString().Trim() <> "L" And tabla3.Rows(index2)("est_tipomedida").ToString().Trim() <> "D" _
                                                And tabla3.Rows(index2)("est_tipomedida").ToString().Trim() <> "B" And tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() <> "V" And tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() <> "X" And tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() <> "R" Then 'C y L es una caso especial y las etiquetas seran diferentes B es un boton
                                        CadenaHTML += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"
                                        CadenaHTML += "<span class='JETIQUETA'>" + tabla3.Rows(index2)("txt_detalle").ToString().Trim() + "</span>"
                                        CadenaHTML += "</div></div>"
                                        ContadorColumna += 1
                                    End If
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() = "R" And tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() <> "X" Then
                                        CadenaHTML += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"
                                        CadenaHTML += "<span class='JETIQUETA'>" + tabla3.Rows(index2)("txt_detalle").ToString().Trim().Split(" ")(0) + "</span>"
                                        CadenaHTML += "</div></div>"
                                        ContadorColumna += 1
                                    End If
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() = "D" Then
                                        CadenaHTML += "<div class='JCELDA-12'><div class='JDIV-CONTROLES'>"
                                        CadenaHTML += "<span class='JETIQUETA'>" + tabla3.Rows(index2)("txt_detalle").ToString().Trim() + "</span>"
                                        CadenaHTML += "</div></div>"
                                        ContadorColumna += 1
                                    End If

                                    'caja de texto
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() = "T" Or tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() = "N" Then
                                        CadenaHTML += "<div class='JCELDA-" + IIf(tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(0) = "0", 1, tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                        Dim CadenaValidacionControl As String = ""
                                        'Observaciones Cmendez 02/05/2022
                                        If tabla3.Rows(index2)("cnt_minmedida").ToString().Trim() <> "0" Or tabla3.Rows(index2)("cnt_maxmedida").ToString().Trim() <> "0" Then
                                            'CadenaValidacionControl += "onblur=" + "fn_ValidaControl(" + tabla3.Rows(index2)("cnt_minmedida").ToString().Trim() + "," + tabla3.Rows(index2)("cnt_maxmedida").ToString().Trim() + ",'" + tabla3.Rows(index2)("dsc_msjvalidacion").ToString().Trim().Replace(" ", "&nbsp;") + "','" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "');" + " "
                                            CadenaValidacionControl += "onblur=" + "fn_ValidaControl(" + tabla3.Rows(index2)("cnt_minmedida").ToString().Trim() + "|" + tabla3.Rows(index2)("cnt_maxmedida").ToString().Trim() + "|'" + tabla3.Rows(index2)("dsc_msjvalidacion").ToString().Trim().Replace(" ", "&nbsp;") + "'|'" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "');" + " "
                                        End If
                                        If tabla3.Rows(index2)("dsc_formula").ToString().Trim() <> "" Then
                                            CadenaValidacionControl += "onkeyup=" + tabla3.Rows(index2)("dsc_formula").ToString().Trim() + ""
                                        End If

                                        If tabla3.Rows(index2)("flg_enabled") = True Then
                                            CadenaHTML += "<input " + CadenaValidacionControl + " maxlength='" + tabla3.Rows(index2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' type='text' onkeypress='return DesactivarEnterKey(event);' class='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(2) + "' value='" + tabla3.Rows(index2)("valor_control").ToString().Trim().Replace(":", " ") + "' />" + "<span class='JETIQUETA_4'>" + tabla3.Rows(index2)("dsc_medida").ToString().Trim() + "</span>"
                                        Else
                                            CadenaHTML += "<input disabled='disabled' maxlength='" + tabla3.Rows(index2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' type='text' onkeypress='return DesactivarEnterKey(event);' class='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(2) + "' value='" + tabla3.Rows(index2)("valor_control").ToString().Trim().Replace(":", " ") + "' />" + "<span class='JETIQUETA_4'>" + tabla3.Rows(index2)("dsc_medida").ToString().Trim() + "</span>"
                                        End If
                                        CadenaHTML += "</div></div>"
                                        ContadorColumna += 1
                                    End If

                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim().ToUpper() = "C" Then
                                        CadenaHTML += "<div class='JCELDA-" + "10" + "'><div class='JDIV-CONTROLES'>"
                                        Dim CadenaValidacionControl As String = ""

                                        If tabla3.Rows(index2)("flg_enabled") = True Then
                                            CadenaHTML += "<input " + CadenaValidacionControl + " id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' type='text' onkeypress='return DesactivarEnterKey(event);' class='" + "JTEXTO-10" + "' value='" + tabla3.Rows(index2)("valor_control").ToString().Trim() + "' />" + ""
                                        Else
                                            CadenaHTML += "<input disabled='disabled' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' type='text' onkeypress='return DesactivarEnterKey(event);' class='" + "JTEXTO-10" + "' value='" + tabla3.Rows(index2)("valor_control").ToString().Trim() + "' />" + ""
                                        End If
                                        CadenaHTML += "</div></div>"
                                        ContadorColumna += 1
                                    End If
                                    'radio button
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim() = "R" Then
                                        Dim DataR As DataRow()
                                        DataR = tabla3.Select("ide_examenfisico_titulo = " + tabla3.Rows(index2)("ide_examenfisico_titulo").ToString().Trim() + "")

                                        tabla4.Columns.Add("ide_examenfisico")
                                        tabla4.Columns.Add("txt_detalle")
                                        tabla4.Columns.Add("cnt_minmedida")
                                        tabla4.Columns.Add("cnt_maxmedida")
                                        tabla4.Columns.Add("cnt_maxcaracteres")
                                        tabla4.Columns.Add("dsc_medida")
                                        tabla4.Columns.Add("est_tipomedida")
                                        tabla4.Columns.Add("dsc_formula")
                                        tabla4.Columns.Add("dsc_txtidcampo")
                                        tabla4.Columns.Add("dsc_msjvalidacion")
                                        tabla4.Columns.Add("flg_enabled")
                                        tabla4.Columns.Add("orden")
                                        tabla4.Columns.Add("dsc_variables")
                                        tabla4.Columns.Add("valor_control")
                                        tabla4.Columns.Add("ide_examenfisico_titulo")

                                        For ind = 0 To DataR.Length - 1
                                            tabla4.ImportRow(DataR(ind))
                                        Next

                                        For index3 = 0 To tabla4.Rows.Count() - 1
                                            If tabla4.Rows(index3)("est_tipomedida").ToString().Trim() = "R" Then
                                                CadenaHTML += "<div class='JCELDA-" + "2" + "'><div class='JDIV-CONTROLES'>"

                                                If tabla4.Rows(index3)("valor_control").ToString().Trim() = "Verdadero" Then
                                                    If tabla4.Rows(index3)("flg_enabled") = True Then
                                                        CadenaHTML += "<input id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + tabla4.Rows(index3)("ide_examenfisico").ToString().Trim() + "' checked='checked'  />" +
                                                         "<span class='JETIQUETA_2'>" + tabla4.Rows(index3)("txt_detalle").ToString().Trim().Trim().Split(" ")(1) + "</span>"
                                                    Else
                                                        CadenaHTML += "<input disabled='disabled' id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + tabla4.Rows(index3)("ide_examenfisico").ToString().Trim() + "' checked='checked'  />" +
                                                         "<span class='JETIQUETA_2'>" + tabla4.Rows(index3)("txt_detalle").ToString().Trim().Trim().Split(" ")(1) + "</span>"
                                                    End If
                                                ElseIf tabla4.Rows(index3)("valor_control").ToString().Trim() = "" Then 'JB - 18/10/2017 - se agreso el ElseIF
                                                    If tabla4.Rows(index3)("flg_enabled") = True Then
                                                        CadenaHTML += "<input id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + tabla4.Rows(index3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                                         "<span class='JETIQUETA_2'>" + tabla4.Rows(index3)("txt_detalle").ToString().Trim().Trim().Split(" ")(1) + "</span>"
                                                    Else
                                                        CadenaHTML += "<input disabled='disabled' id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + tabla4.Rows(index3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                                         "<span class='JETIQUETA_2'>" + tabla4.Rows(index3)("txt_detalle").ToString().Trim().Trim().Split(" ")(1) + "</span>"
                                                    End If
                                                Else
                                                    If tabla4.Rows(index3)("flg_enabled") = True Then
                                                        CadenaHTML += "<input id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + tabla4.Rows(index3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                                         "<span class='JETIQUETA_2'>" + tabla4.Rows(index3)("txt_detalle").ToString().Trim().Trim().Split(" ")(1) + "</span>"
                                                    Else
                                                        CadenaHTML += "<input disabled='disabled' id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + tabla4.Rows(index3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                                         "<span class='JETIQUETA_2'>" + tabla4.Rows(index3)("txt_detalle").ToString().Trim().Trim().Split(" ")(1) + "</span>"
                                                    End If
                                                End If
                                                CadenaHTML += "</div></div>"
                                            ElseIf tabla4.Rows(index3)("est_tipomedida").ToString().Trim() = "T" Then
                                                CadenaHTML += "<div class='JCELDA-" + IIf(tabla4.Rows(index3)("dsc_variables").ToString().Split(";")(0) = "0", 1, tabla4.Rows(index3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                                If tabla4.Rows(index3)("flg_enabled") = True Then
                                                    CadenaHTML += "<input maxlength='" + tabla4.Rows(index3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='text' onkeypress='return DesactivarEnterKey(event);' class='" + tabla4.Rows(index3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + tabla4.Rows(index3)("valor_control").ToString().Trim() + "' /> " + tabla4.Rows(index3)("dsc_medida").ToString().Trim()
                                                Else
                                                    CadenaHTML += "<input disabled='disabled' maxlength='" + tabla4.Rows(index3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla4.Rows(index3)("dsc_txtidcampo").ToString().Trim() + "' type='text' onkeypress='return DesactivarEnterKey(event);' class='" + tabla4.Rows(index3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + tabla4.Rows(index3)("valor_control").ToString().Trim() + "' /> " + tabla4.Rows(index3)("dsc_medida").ToString().Trim()
                                                End If
                                                CadenaHTML += "</div></div>"
                                            End If
                                        Next
                                        ContadorColumna += 1

                                        For ind2 = 0 To tabla3.Rows.Count - 1 'este bloque de codigo es para que no vuelve a generar html de los radiobuton
                                            If tabla3.Rows(ind2)("ide_examenfisico_titulo").ToString().Trim() = tabla3.Rows(index2)("ide_examenfisico_titulo").ToString().Trim() And tabla3.Rows(ind2)("est_tipomedida").ToString().Trim() = "R" Then
                                                tabla3.Rows(ind2)("est_tipomedida") = "X"
                                            End If
                                        Next
                                    End If
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim() = "D" Then 'D - DESCRIPCION/TEXTAREA
                                        CadenaHTML += "<div class='JCELDA-" + IIf(tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(0) = "0", 1, tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                        If tabla3.Rows(index2)("flg_enabled") = True Then
                                            If tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(3).ToString().Trim() <> "" Then
                                                CadenaHTML += "<textarea maxlength='" + tabla3.Rows(index2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' rows='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(3).ToString().Trim() + "' cols='1' runat='server' class='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(2) + "' >" + tabla3.Rows(index2)("valor_control").ToString().Trim() + "</textarea>"
                                            Else
                                                CadenaHTML += "<textarea maxlength='" + tabla3.Rows(index2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' rows='5' cols='1' runat='server' class='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(2) + "' >" + tabla3.Rows(index2)("valor_control").ToString().Trim() + "</textarea>"
                                            End If
                                        Else
                                            If tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(3).ToString().Trim() <> "" Then
                                                CadenaHTML += "<textarea disabled='disabled' maxlength='" + tabla3.Rows(index2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' rows='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(3).ToString().Trim() + "' cols='1' runat='server' class='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(2) + "' >" + tabla3.Rows(index2)("valor_control").ToString().Trim() + "</textarea>"
                                            Else
                                                CadenaHTML += "<textarea disabled='disabled' maxlength='" + tabla3.Rows(index2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + tabla3.Rows(index2)("dsc_txtidcampo").ToString().Trim() + "' rows='5' cols='1' runat='server' class='" + tabla3.Rows(index2)("dsc_variables").ToString().Split(";")(2) + "' >" + tabla3.Rows(index2)("valor_control").ToString().Trim() + "</textarea>"
                                            End If
                                        End If

                                        CadenaHTML += "</div></div>"
                                        ContadorColumna += 1
                                    End If
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim() = "K" Then 'checkbox *****************
                                        oRceExamenfisicoMaeE.IdeExamenFisico = CType(tabla3.Rows(index2)("ide_examenfisico").ToString().Trim(), Integer)
                                        oRceExamenfisicoMaeE.Orden = 5
                                        tabla4 = oRceExamenFisicoMaeLN.Sp_RceExamenFisicoMae_Consulta4(oRceExamenfisicoMaeE)
                                        For index4 = 0 To tabla4.Rows.Count() - 1
                                            CadenaHTML += "<div class='JCELDA-" + IIf(tabla4.Rows(index4)("dsc_variables").ToString().Split(";")(0) = "0", 1, tabla4.Rows(index4)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                            If tabla4.Rows(index4)("dsc_txtidcampo").ToString().Trim() = "1" Then
                                                If tabla4.Rows(index4)("flg_enabled") = True Then
                                                    CadenaHTML += "<input id='" + tabla4.Rows(index4)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='true'  />" +
                                                    "<span class='JETIQUETA_4'>" + tabla4.Rows(index4)("txt_detalle").ToString().Trim() + "</span>"
                                                Else
                                                    CadenaHTML += "<input disabled='disabled' id='" + tabla4.Rows(index4)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='true'  />" +
                                                    "<span class='JETIQUETA_4'>" + tabla4.Rows(index4)("txt_detalle").ToString().Trim() + "</span>"
                                                End If
                                            Else
                                                If tabla4.Rows(index4)("flg_enabled") = True Then
                                                    CadenaHTML += "<input id='" + tabla4.Rows(index4)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server'   />" +
                                                    "<span class='JETIQUETA_4'>" + tabla4.Rows(index4)("txt_detalle").ToString().Trim() + "</span>"
                                                Else
                                                    CadenaHTML += "<input disabled='disabled' id='" + tabla4.Rows(index4)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server'   />" +
                                                    "<span class='JETIQUETA_4'>" + tabla4.Rows(index4)("txt_detalle").ToString().Trim() + "</span>"
                                                End If

                                            End If
                                            CadenaHTML += "</div></div>"
                                        Next
                                        ContadorColumna += 1
                                    End If
                                    If tabla3.Rows(index2)("est_tipomedida").ToString().Trim() = "V" Then
                                        'CONSULA PARA LISTADO
                                        Dim dtV As New DataTable()
                                        'LLENAR EL DT CON LOS DATOS DEL SP *DANTE

                                        CadenaHTML += "</div><div style='border:1px solid #4BACFF;width:100%;'><div class='JFILA'>" 'se empieza cerrando la fila de los otros controles para iniciar con los controles V en una nueva fila
                                        CadenaHTML += "<div class='JCELDA-6'><div class='JDIV-CONTROLES'>" 'inicio celda 6/controles
                                        If tabla3.Rows(index2)("valor_control").ToString().Trim() <> "" Then 'si el campo tiene valores
                                            CadenaHTML += "<div class='JFILA'>"
                                            CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>"
                                            CadenaHTML += "<span class='JETIQUETA_5'>Partes del Cuerpo</span>"
                                            CadenaHTML += "</div>"
                                            CadenaHTML += "</div>"
                                            'Observaciones Cmendez 02/05/2022
                                            Dim CodigoV As String() = tabla3.Rows(index2)("valor_control").ToString().Trim().Split("|")

                                            For IndiceV = 0 To CodigoV.Length - 1 'recorrido de los controles V
                                                If CodigoV(IndiceV).Trim() <> "" Then
                                                    'aqui se crea cada registro del control V
                                                    Dim dt As New DataTable()
                                                    oTablasE.CodTabla = "HCE_PARTECUERPO"
                                                    oTablasE.Buscar = CodigoV(IndiceV).Trim()
                                                    oTablasE.Orden = 8
                                                    dt = oTablasLN.Sp_Tablas_Consulta(oTablasE)
                                                    If dt.Rows.Count > 0 Then
                                                        CadenaHTML += "<div class='JFILA'>"
                                                        CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>"
                                                        CadenaHTML += "<input type='checkbox' checked='checked' id='chkParteCuerpo_" + dt.Rows(0)("codigo").ToString().Trim() + "' value='" + dt.Rows(0)("codigo").ToString().Trim() + "'>"
                                                        CadenaHTML += "<label for='chkParteCuerpo_" + dt.Rows(0)("codigo").ToString().Trim() + "' class='JETIQUETA_4'>" + dt.Rows(0)("nombre").ToString().Trim() + "</label>"
                                                        CadenaHTML += "</div>"
                                                        CadenaHTML += "</div>"
                                                    End If
                                                    If IndiceV Mod CantidadFilaV = 0 And IndiceV <> 0 Then
                                                        CadenaHTML += "</div>" 'fin celda 6/controles
                                                        CadenaHTML += "<div class='JCELDA-6'><div class='JDIV-CONTROLES'>" 'inicio celda 6/controles
                                                    End If
                                                End If
                                            Next
                                        End If
                                        CadenaHTML += "</div></div>" 'fin celda 6/controles
                                        CadenaHTML += "</div></div>"
                                    End If
                                    If ContadorColumna = CantidadColumnasGrupo Then
                                        CadenaHTML += "</div>"
                                        CadenaHTML += "<div class='JFILA'>"
                                        ContadorColumna = 0
                                    End If

                                Next

                            End If
                        End If
                    Next
                End If
                CadenaHTML += "</div></div>" 'cierra fila / fin html div contenido
            Next
        End If
        CadenaHTML += "</div>"

        'divContenedorDinamico.InnerHtml = CadenaHTML
        'ScriptManager.RegisterStartupScript(Page, Me.GetType(), "Script_Deshabilita", "DeshabilitaE();", True)
        Return CadenaHTML
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function CargarDatosControlesDinamicoGrupo(ByVal IdeExamenFisico As Integer, ByVal CantidadColumnas As String, ByVal IdeHistoria As String, ByVal TipoDeAtencion As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CargarDatosControlesDinamicoGrupo_(IdeExamenFisico, CantidadColumnas, IdeHistoria, TipoDeAtencion)
    End Function

    Public Function CargarDatosControlesDinamicoGrupo_(ByVal IdeExamenFisico As Integer, ByVal CantidadColumnas As String, Optional ByVal IdeHistoria As String = "", Optional ByVal TipoDeAtencion As String = "", Optional ByVal Secciondata As String = "") As String
        If Not IsNothing(Session(sCodigoAtencion)) Then
            Dim aCantidadColTipo(2) As String
            aCantidadColTipo = CantidadColumnas.Split("-")

            Dim CantidadColumnasGrupo As Integer
            CantidadColumnasGrupo = CType(aCantidadColTipo(0), Integer)

            Dim CuerpoControles As String = ""
            Dim CuerpoTabHijo As String = ""
            Dim TablaControles As New DataTable()
            Dim TablaGrupoTab As New DataTable()
            Dim TablaGrupoControles_Auxiliar As New DataTable()
            Dim Historia As Integer
            Dim TipoAtencion As String = Session(sCodigoAtencion).ToString().Substring(0, 1) '17-06-2016 JB

            If TipoDeAtencion.Trim() <> "" Then
                TipoAtencion = TipoDeAtencion
                Session(sTipoAtencion) = TipoAtencion  '17-06-2016 JB
                'Else
                '    TipoAtencion = Session(sCodigoAtencion) '*********************
            End If
            If IdeHistoria = "" Then
                Historia = Session(sIdeHistoria) '*****************
            Else
                Historia = CType(IdeHistoria, Integer)
                Session(sIdeHistoria) = Historia '17-06-2016 JB
            End If



            If aCantidadColTipo(1) = "G" Then 'SI ES G TENDRA TABS Y CONTROLES
                oRceAnamnesisE.IdeExamenFisico = IdeExamenFisico
                oRceAnamnesisE.IdeHistoria = 0
                oRceAnamnesisE.IdeTipoAtencion = TipoAtencion
                If IsNothing(Session(sCodMedico)) Then
                    oRceAnamnesisE.CodMedico = ""
                Else
                    oRceAnamnesisE.CodMedico = Session(sCodMedico)
                End If
                oRceAnamnesisE.FlgEstado = "A"
                oRceAnamnesisE.Orden = 3
                TablaGrupoTab = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

                'INICIO - JB - 29/04/2019 - nuevo codigo para control fuera de los tabs
                Dim TalbaControles1 As New DataTable()
                For indice_ = 0 To TablaGrupoTab.Rows.Count() - 1
                    oRceAnamnesisE.IdeExamenFisico = CType(TablaGrupoTab.Rows(indice_)("ide_examenfisico").ToString().Trim(), Integer)
                    oRceAnamnesisE.IdeHistoria = Historia
                    oRceAnamnesisE.Orden = 4
                    TalbaControles1 = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

                    For indice2_ = 0 To TalbaControles1.Rows.Count() - 1
                        If TalbaControles1.Rows(indice2_)("est_tipomedida").ToString().Trim() = "U" Then

                            CuerpoTabHijo += "<div class='JFILA'>"
                            CuerpoTabHijo += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"
                            CuerpoTabHijo += "<span class='JETIQUETA'>" + TalbaControles1.Rows(indice2_)("txt_detalle").ToString().Trim() + "</span>"
                            CuerpoTabHijo += "</div></div>"

                            CuerpoTabHijo += "<div class='JCELDA-" + IIf(TalbaControles1.Rows(indice2_)("dsc_variables").ToString().Split(";")(0) = "0", 1, TalbaControles1.Rows(indice2_)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                            Dim CadenaValidacionControl As String = ""
                            If TalbaControles1.Rows(indice2_)("cnt_minmedida").ToString().Trim() <> "0" Or TalbaControles1.Rows(indice2_)("cnt_maxmedida").ToString().Trim() <> "0" Then
                                'Observaciones Cmendez 02/05/2022
                                CadenaValidacionControl += "onblur=" + "fn_ValidaControl(" + TalbaControles1.Rows(indice2_)("cnt_minmedida").ToString().Trim() + "|" + TalbaControles1.Rows(indice2_)("cnt_maxmedida").ToString().Trim() + "|'" + TalbaControles1.Rows(indice2_)("dsc_msjvalidacion").ToString().Trim().Replace(" ", "&nbsp;") + "'|'" + TalbaControles1.Rows(indice2_)("dsc_txtidcampo").ToString().Trim() + "');" + " "
                            End If
                            If TalbaControles1.Rows(indice2_)("dsc_formula").ToString().Trim() <> "" Then
                                CadenaValidacionControl += "onkeyup=" + TalbaControles1.Rows(indice2_)("dsc_formula").ToString().Trim() + ""
                            End If

                            If TalbaControles1.Rows(indice2_)("flg_enabled") = True Then
                                CuerpoTabHijo += "<input " + CadenaValidacionControl + "  maxlength='" + TalbaControles1.Rows(indice2_)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TalbaControles1.Rows(indice2_)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TalbaControles1.Rows(indice2_)("dsc_variables").ToString().Split(";")(2) + "' value='" + TalbaControles1.Rows(indice2_)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TalbaControles1.Rows(indice2_)("dsc_medida").ToString().Trim() + "</span>"
                            Else
                                CuerpoTabHijo += "<input disabled='disabled' maxlength='" + TalbaControles1.Rows(indice2_)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TalbaControles1.Rows(indice2_)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TalbaControles1.Rows(indice2_)("dsc_variables").ToString().Split(";")(2) + "' value='" + TalbaControles1.Rows(indice2_)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TalbaControles1.Rows(indice2_)("dsc_medida").ToString().Trim() + "</span>"
                            End If
                            CuerpoTabHijo += "</div></div>"
                            CuerpoTabHijo += "</div>"

                        End If
                    Next
                Next
                'FIN - JB - 29/04/2019 - nuevo codigo para control fuera de los tabs

                CuerpoTabHijo += "<div class='JFILA'><div class='JCELDA-12'><div class='JDIV-CONTROLES'>"
                CuerpoTabHijo += "<div class='JCONTENEDOR-TAB" + (IdeExamenFisico).ToString() + "'>"
                CuerpoTabHijo += "<div class='JSBTABS'><label for='chkTABS2' class='JSBMOSTRAR_TABS'></label><input type='checkbox' id='chkTABS2' class='chkTAB-CHECK' /><ul>"
                For indice1 = 0 To TablaGrupoTab.Rows.Count() - 1
                    If indice1 = 0 Then
                        CuerpoTabHijo += "<li class='JSBTAB_ACTIVO tabs_historiaclinica' id='" + Secciondata + " - " + TablaGrupoTab.Rows(indice1)("txt_detalle").ToString().Trim() + "'><a>" + TablaGrupoTab.Rows(indice1)("txt_detalle").ToString().Trim() + "</a></li>"
                    Else
                        CuerpoTabHijo += "<li class='tabs_historiaclinica' id='" + Secciondata + " - " + TablaGrupoTab.Rows(indice1)("txt_detalle").ToString().Trim() + "'><a>" + TablaGrupoTab.Rows(indice1)("txt_detalle").ToString().Trim() + "</a></li>"
                    End If

                    oRceAnamnesisE.IdeExamenFisico = CType(TablaGrupoTab.Rows(indice1)("ide_examenfisico").ToString().Trim(), Integer)
                    oRceAnamnesisE.IdeHistoria = Historia
                    oRceAnamnesisE.Orden = 4
                    TablaControles = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                    CantidadColumnasGrupo = CType(TablaGrupoTab.Rows(indice1)("cnt_minmedida").ToString().Trim(), Integer)
                    Dim ContadorColumna As Integer = 0
                    CuerpoControles += "<div class='JCUERPO'>"

                    'INICIO - JB - NUEVO FOR - 10/04/2019
                    Dim bFloatamiento As Boolean = False
                    Dim Division As String = ""
                    For indicex = 0 To TablaControles.Rows.Count() - 1
                        If TablaControles.Rows(indicex)("est_tipomedida").ToString().Trim() = "L" Then
                            Dim cadena As String = ""
                            cadena = TablaControles.Rows(indicex)("dsc_variables").ToString().Trim().Split(";")(2)
                            If TablaControles.Rows(indicex)("dsc_variables").ToString().Trim().Split(";")(2) = "JFLOTA-I" Or
                                TablaControles.Rows(indicex)("dsc_variables").ToString().Trim().Split(";")(2) = "JFLOTA-D" Then
                                If bFloatamiento = False Then
                                    CuerpoControles += "<div class='JCELDA-6'><span class='" + TablaControles.Rows(indicex)("dsc_variables").ToString().Trim().Split(";")(2) + "'>"
                                    bFloatamiento = True
                                    Division = TablaControles.Rows(indicex)("dsc_variables").ToString().Trim().Split(";")(2)
                                End If
                            End If
                        End If
                    Next
                    'FIN - JB - NUEVO FOR - 10/04/2019

                    CuerpoControles += "<div class='JFILA'>"

                    For indice2 = 0 To TablaControles.Rows.Count() - 1
                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "C" And TablaControles.Rows(indice2)("txt_detalle").ToString().Trim().ToUpper() <> "" And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() <> "L" And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() <> "M" _
                            And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "B" And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "Y" And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "Z" _
                            And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "X" And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "W" And TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() <> "U" Then 'C y L es una caso especial y las etiquetas seran diferentes  - U WXYZ nuevo tipo para controles nuevo tipo check 10/04/2019
                            If TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().IndexOf("JCOL-OCULTA") > 0 Then 'INICIO - JB - 01/05/2019 - nueva condicion para campos ocultos

                            Else
                                CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>" 'AQUI POSIBLE ETIQUETA DINAMICA
                                CuerpoControles += "<span class='JETIQUETA'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                                CuerpoControles += "</div></div>"
                            End If
                            ContadorColumna += 1
                        End If

                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "L" Then

                            CuerpoControles += "<div class='JCELDA-12'><div class='JDIV-CONTROLES'>"
                            If TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(1) = "N" Then
                                CuerpoControles += "<span class='JETIQUETA_3' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "'><strong>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</strong></span>"
                            ElseIf TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(1) = "S" Then
                                CuerpoControles += "<span class='JETIQUETA_3' style='text-decoration:underline;'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            Else
                                CuerpoControles += "<span class='JETIQUETA_3'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            End If
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1

                        End If

                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "M" Then

                            CuerpoControles += "<div class='JCELDA-" + TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(0) + "'><div class='JDIV-CONTROLES'>"
                            If TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(1) = "N" Then
                                CuerpoControles += "<span class='JETIQUETA_3 " + TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(2) + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' style='width:120px;'><strong>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</strong></span>"
                            ElseIf TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(1) = "S" Then
                                CuerpoControles += "<span class='JETIQUETA_3 " + TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().Split(";")(2) + "' style='text-decoration:underline;width:120px;'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            Else
                                CuerpoControles += "<span class='JETIQUETA_3'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            End If
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1

                        End If


                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() = "T" Or TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() = "N" Then
                            If TablaControles.Rows(indice2)("dsc_variables").ToString().Trim().IndexOf("JCOL-OCULTA") > 0 Then 'INICIO - JB - 01/05/2019 - nueva condicion para campos ocultos

                            Else
                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                Dim CadenaValidacionControl As String = ""
                                If TablaControles.Rows(indice2)("cnt_minmedida").ToString().Trim() <> "0" Or TablaControles.Rows(indice2)("cnt_maxmedida").ToString().Trim() <> "0" Then
                                    'Observaciones Cmendez 02/05/2022
                                    CadenaValidacionControl += "onblur=" + "fn_ValidaControl(" + TablaControles.Rows(indice2)("cnt_minmedida").ToString().Trim() + "|" + TablaControles.Rows(indice2)("cnt_maxmedida").ToString().Trim() + "|'" + TablaControles.Rows(indice2)("dsc_msjvalidacion").ToString().Trim().Replace(" ", "&nbsp;") + "'|'" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "');" + " "
                                End If
                                If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                    CadenaValidacionControl += "onkeyup=" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + ""
                                End If

                                If TablaControles.Rows(indice2)("flg_enabled") = True Then
                                    CuerpoControles += "<input " + CadenaValidacionControl + "  maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' value='" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TablaControles.Rows(indice2)("dsc_medida").ToString().Trim() + "</span>"
                                Else
                                    CuerpoControles += "<input disabled='disabled' maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' value='" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TablaControles.Rows(indice2)("dsc_medida").ToString().Trim() + "</span>"
                                End If
                                CuerpoControles += "</div></div>"
                                'CuerpoControles += "</div>" 'cerrando jfila *********************************
                            End If
                            ContadorColumna += 1
                        End If
                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "R" Then
                            oRceAnamnesisE.IdeExamenFisico = CType(TablaControles.Rows(indice2)("ide_examenfisico").ToString().Trim(), Integer)
                            oRceAnamnesisE.IdeHistoria = Historia
                            oRceAnamnesisE.Orden = 5
                            TablaGrupoControles_Auxiliar = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                            Dim ValorAuxiliarImagen As String = "" '08/09/2016

                            'INICIO - JB - NUEVO CODIGO - 29/04/2019
                            Dim CadenaEvento As String = ""
                            If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                CadenaEvento = "onblur='" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + "'"
                            End If
                            'FIN - JB - NUEVO CODIGO - 29/04/2019

                            'INICIO  - JB - NUEVO CODIGO - 22/07/2019
                            Dim CadenaEvento2 As String = ""
                            If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                CadenaEvento2 = "onchange='" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + "'"
                            End If
                            'FIN - JB - NUEVO CODIGO - 27/07/2019

                            For indice3 = 0 To TablaGrupoControles_Auxiliar.Rows.Count() - 1


                                'INICIO - JB - nuevo control para boton + en cirugias - 09/01/2020
                                If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "P" Then
                                    CuerpoControles += "<div class='JDIV-CIRUGIA'>"
                                    CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                    CuerpoControles += "<input type='button' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' style='background-image:url(../Imagenes/Agregar.png);' />"
                                    CuerpoControles += "</div></div>"
                                    CuerpoControles += "</div>"
                                End If
                                'FIN - JB - nuevo control para boton + en cirugias - 09/01/2020



                                If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "I" Then
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "S" And indice3 = 0 Then
                                        ValorAuxiliarImagen += "<img src='../Imagenes/ico_data.png' alt='' class='JIMG-GENERAL JIMGRADIO' style='height:16px;'>"
                                    Else
                                        ValorAuxiliarImagen += "<img src='../Imagenes/ico_vacio.png' alt='' class='JIMG-GENERAL JIMGRADIO' style='height:16px;'>"
                                    End If
                                End If

                                If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "R" Then
                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                    '08/09/2016
                                    'CuerpoControles += ValorAuxiliarImagen 'JB - COMENTADO TEMPORALMENTE IMG... - 21/01/2020
                                    ValorAuxiliarImagen = "" 'JB - TEMPORALMENTE - 21/01/2020
                                    '08/09/2016

                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input " + CadenaEvento2 + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input " + CadenaEvento2 + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    Else
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input " + CadenaEvento2 + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input " + CadenaEvento2 + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    End If
                                    CuerpoControles += "</div></div>"
                                ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "T" Then
                                    'INICIO -JB - 06/05/2020
                                    Dim espacio As String = ""
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Contains("JESPACIO-") Then
                                        espacio = TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim()
                                        espacio = espacio.Substring(espacio.IndexOf("JESPACIO-"))
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables") = TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Replace(espacio, "").Trim()
                                    End If
                                    'FIN -JB - 06/05/2020




                                    'INICIO - JB - 09/04/2019 - PARA CIRUGIA APLICAR UN SALTO DE LINEA
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() = "Cirugia" And indice3 = 0 Then 'JB - 09/01/2020 - se agrega a la condicion *indice3 = 0*
                                        CuerpoControles += "<div class='JCELDA-12'></div>"
                                    End If
                                    'FIN - JB - 09/04/2019
                                    'INICIO - JB - 10/01/2020 - SE AGREGARA UN DIV QUE CONTENDRA LOS CONTROLES DE CIRUGIA, ESTO AYUDARA IDENTIFICAR Y OCULTAR LOS CONTROLES DESDE LA VISTA
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_CirugiaCirugia") Or
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_FechaCirugia") Or
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_DiagnosticoCirugia") Or
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_DetalleCirugia") Then
                                        CuerpoControles += "<div class='JDIV-CIRUGIA'>"
                                    End If
                                    'INICIO - JB - 10/01/2020 - SE AGREGARA UN DIV QUE CONTENDRA LOS CONTROLES DE CIRUGIA, ESTO AYUDARA IDENTIFICAR Y OCULTAR LOS CONTROLES DESDE LA VISTA

                                    'INICIO - JB - 09/04/2019 - validando para no generar etiqueta si esta vacio
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() <> "" Then
                                        CuerpoControles += "<div class='JCELDA-1 " + espacio + "'><div class='JDIV-CONTROLES'>"
                                        CuerpoControles += "<span class='JETIQUETA'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        CuerpoControles += "</div></div>"
                                    End If
                                    'FIN - JB - 09/04/2019

                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                        CuerpoControles += "<input maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                    Else
                                        CuerpoControles += "<input disabled='disabled' maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                    End If
                                    'INICIO - JB - 10/01/2020 - SE AGREGARA UN DIV QUE CONTENDRA LOS CONTROLES DE CIRUGIA, ESTO AYUDARA IDENTIFICAR Y OCULTAR LOS CONTROLES DESDE LA VISTA
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_CirugiaCirugia") Or
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_FechaCirugia") Or
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_DiagnosticoCirugia") Or
                                        TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("txt_DetalleCirugia") Then
                                        CuerpoControles += "</div>"
                                    End If
                                    'INICIO - JB - 10/01/2020 - SE AGREGARA UN DIV QUE CONTENDRA LOS CONTROLES DE CIRUGIA, ESTO AYUDARA IDENTIFICAR Y OCULTAR LOS CONTROLES DESDE LA VISTA
                                    CuerpoControles += "</div></div>"
                                ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "O" Then

                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("cbo_BasalRankinModificado") Then
                                        CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                        CuerpoControles += "<span class='JETIQUETA'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        CuerpoControles += "</div></div>"

                                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<select " + CadenaEvento.Replace("onblur", "onchange") + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "
                                            'llenado de select aqui
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "0", " selected='selected' ", "") + " value='0'>0 - Asintomático</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1", " selected='selected' ", "") + " value='1'>1 - Sin discapacidad significativa</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "2", " selected='selected' ", "") + " value='2'>2 - Incapacidad minima</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "3", " selected='selected' ", "") + " value='3'>3 - Incapacidad moderada</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "4", " selected='selected' ", "") + " value='4'>4 - Incapacidad moderada - severa</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "5", " selected='selected' ", "") + " value='5'>5 - Incapacidad severa</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "6", " selected='selected' ", "") + " value='6'>6 - Muerto</option>"

                                            CuerpoControles += "</select>"
                                        Else
                                            CuerpoControles += "<select " + CadenaEvento.Replace("onblur", "onchange") + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "
                                            'llenado de select aqui
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "0", " selected='selected' ", "") + " value='0'>0 - Asintomático</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1", " selected='selected' ", "") + " value='1'>1 - Sin discapacidad significativa</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "2", " selected='selected' ", "") + " value='2'>2 - Incapacidad minima</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "3", " selected='selected' ", "") + " value='3'>3 - Incapacidad moderada</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "4", " selected='selected' ", "") + " value='4'>4 - Incapacidad moderada - severa</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "5", " selected='selected' ", "") + " value='5'>5 - Incapacidad severa</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "6", " selected='selected' ", "") + " value='6'>6 - Muerto</option>"

                                            CuerpoControles += "</select>"
                                        End If
                                        CuerpoControles += "</div></div>"
                                    End If
                                    'INICIO - JB - 05/05/2020
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("cbo_Ppl") Then
                                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<select " + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "

                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "0", " selected='selected' ", "") + " value='0'>Derecha</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1", " selected='selected' ", "") + " value='1'>Izquierda</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "2", " selected='selected' ", "") + " value='2'>S/A</option>"

                                            CuerpoControles += "</select>"
                                        Else
                                            CuerpoControles += "<select " + "disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "

                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "0", " selected='selected' ", "") + " value='0'>Derecha</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1", " selected='selected' ", "") + " value='1'>Izquierda</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "2", " selected='selected' ", "") + " value='2'>S/A</option>"

                                            CuerpoControles += "</select>"
                                        End If
                                        CuerpoControles += "</div></div>"
                                    End If
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim().Contains("cbo_Prus") Then
                                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<select " + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "

                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "0", " selected='selected' ", "") + " value='0'>Derecha</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1", " selected='selected' ", "") + " value='1'>Izquierda</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "2", " selected='selected' ", "") + " value='2'>S/A</option>"

                                            CuerpoControles += "</select>"
                                        Else
                                            CuerpoControles += "<select " + "disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "

                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "0", " selected='selected' ", "") + " value='0'>Derecha</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1", " selected='selected' ", "") + " value='1'>Izquierda</option>"
                                            CuerpoControles += "<option" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "2", " selected='selected' ", "") + " value='2'>S/A</option>"

                                            CuerpoControles += "</select>"
                                        End If
                                        CuerpoControles += "</div></div>"
                                    End If
                                    'FIN - JB - 05/05/2020


                                ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "K" Then
                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input " + CadenaEvento2 + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='true'  />" +
                                             "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input " + CadenaEvento2 + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='true'  />" +
                                            "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    Else
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input " + CadenaEvento2 + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />" +
                                            "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input " + CadenaEvento2 + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />" +
                                            "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If

                                    End If
                                    CuerpoControles += "</div></div>"
                                End If
                            Next
                            'CuerpoControles += "</div>" 'cerrando jfila *********************************
                            ContadorColumna += 1
                        End If


                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "C" Then
                            Dim aDetalle(3) As String
                            aDetalle = TablaControles.Rows(indice2)("txt_detalle").ToString().Trim().Split("*")

                            For indice_array = 0 To aDetalle.Length - 1
                                If indice_array = aDetalle.Length - 1 Then 'el ultimo valor es uan descripcion larga por lo que tendra un tamaño de celda mayor
                                    CuerpoControles += "<div class='JCELDA-4'><div class='JDIV-CONTROLES'>"
                                    CuerpoControles += "<span class='JETIQUETA_2'>" + aDetalle(indice_array).ToString().Trim() + "</span>"
                                    CuerpoControles += "</div></div>"
                                ElseIf indice_array = 0 Then
                                    CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                    CuerpoControles += "<span class='JETIQUETA_2'>" + aDetalle(indice_array).ToString().Trim() + "</span>"
                                    CuerpoControles += "</div></div>"
                                Else
                                    CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"
                                    CuerpoControles += "<span class='JETIQUETA_2'>" + aDetalle(indice_array).ToString().Trim() + "</span>"
                                    CuerpoControles += "</div></div>"
                                End If
                                ContadorColumna += 1
                            Next
                            'CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>" PROBANDO...

                            'INICIO - JB - NUEVO - 16/04/2019 - PROBANDO...
                            oRceAnamnesisE.IdeExamenFisico = CType(TablaControles.Rows(indice2)("ide_examenfisico").ToString().Trim(), Integer)
                            oRceAnamnesisE.IdeHistoria = Historia
                            oRceAnamnesisE.Orden = 5
                            TablaGrupoControles_Auxiliar = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                            Dim ValorAuxiliarImagen As String = ""

                            'INICIO -JB - 19/07/2019
                            Dim CadenaEvento As String = ""
                            If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                CadenaEvento = "onchange='" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + "'" 'JB - se cambia en onblur por onchange - 26/12/2019
                            End If
                            'FIN -JB - 19/07/2019
                            For indice3 = 0 To TablaGrupoControles_Auxiliar.Rows.Count() - 1
                                If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "R" Then
                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input " + CadenaEvento + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input " + CadenaEvento + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    Else
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input " + CadenaEvento + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input " + CadenaEvento + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                             "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    End If
                                    CuerpoControles += "</div></div>"
                                End If
                            Next
                            'FIN - JB - NUEVO - 16/04/2019 - PROBANDO...


                            'INICIO - JB - COMENTADO - PROBANDO...
                            'If TablaControles.Rows(indice2)("flg_enabled") = True Then
                            '    If TablaControles.Rows(indice2)("valor_control").ToString().Trim() = "1" Then
                            '        CuerpoControles += "<input id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked' />"
                            '    Else
                            '        CuerpoControles += "<input id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />"
                            '    End If
                            'Else
                            '    If TablaControles.Rows(indice2)("valor_control").ToString().Trim() = "1" Then
                            '        CuerpoControles += "<input disabled='disabled' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked' />"
                            '    Else
                            '        CuerpoControles += "<input disabled='disabled' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />"
                            '    End If
                            'End If
                            'CuerpoControles += "</div></div>"
                            'CuerpoControles += "<br />"
                            'FIN - JB - COMENTADO - PROBANDO...

                            ContadorColumna += 1
                        End If

                        'INICIO - JB - NUEVO CODIGO - 10/04/2019
                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "Y" Then 'CONTROLES CHECK ESPECIALES PARA AGRUPARLOS
                            CuerpoControles += "<div class='JCELDA-6'><div class='JDIV-CONTROLES'>"
                            CuerpoControles += "<span class='JETIQUETA_2'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            CuerpoControles += "</div></div>"

                            CuerpoControles += "<div class='JCELDA-" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) + "'><div class='JDIV-CONTROLES'>"

                            Dim CadenaEvento As String = ""

                            If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                'CadenaEvento = "onblur=""" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + """"
                                CadenaEvento = "onchange='" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + "'"  'JB - Se cambio de onblur a onchange - 26/12/2019
                                '"onkeyup=" + TablaControles.Rows(indice)("dsc_formula").ToString().Trim() + ""
                            End If

                            If TablaControles.Rows(indice2)("flg_enabled") = True Then
                                If TablaControles.Rows(indice2)("valor_control").ToString().Trim() = "1" Then
                                    CuerpoControles += "<input " + CadenaEvento + " id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked' />"
                                Else
                                    CuerpoControles += "<input " + CadenaEvento + " id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server'  />"
                                End If
                            Else
                                If TablaControles.Rows(indice2)("valor_control").ToString().Trim() = "1" Then
                                    CuerpoControles += "<input " + CadenaEvento + " disabled='disabled' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked' />"
                                Else
                                    CuerpoControles += "<input " + CadenaEvento + " disabled='disabled' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server'  />"
                                End If
                            End If
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1
                        End If
                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim().ToUpper() = "Z" Then 'CONTROL CAJA TEXTO PARA 'PUNTAJE FINAL'
                            CuerpoControles += "</div><br></span></div>"
                            CuerpoControles += "<div class='JFILA'>"
                            bFloatamiento = False

                            CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"
                            CuerpoControles += "<span class='JETIQUETA'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            CuerpoControles += "</div></div>"

                            CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                            Dim CadenaValidacionControl As String = ""
                            If TablaControles.Rows(indice2)("cnt_minmedida").ToString().Trim() <> "0" Or TablaControles.Rows(indice2)("cnt_maxmedida").ToString().Trim() <> "0" Then
                                'Observaciones Cmendez 02/05/2022
                                CadenaValidacionControl += "onblur=" + "fn_ValidaControl(" + TablaControles.Rows(indice2)("cnt_minmedida").ToString().Trim() + "|" + TablaControles.Rows(indice2)("cnt_maxmedida").ToString().Trim() + "|'" + TablaControles.Rows(indice2)("dsc_msjvalidacion").ToString().Trim().Replace(" ", "&nbsp;") + "'|'" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "');" + " "
                            End If
                            If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                CadenaValidacionControl += "onkeyup=" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + ""
                            End If

                            If TablaControles.Rows(indice2)("flg_enabled") = True Then
                                CuerpoControles += "<input " + CadenaValidacionControl + "  maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' value='" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TablaControles.Rows(indice2)("dsc_medida").ToString().Trim() + "</span>"
                            Else
                                CuerpoControles += "<input disabled='disabled' maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' value='" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TablaControles.Rows(indice2)("dsc_medida").ToString().Trim() + "</span>"
                            End If
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1
                        End If

                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "X" Then 'ETIQUETAS COLOR VERDE  CON ANCHO DINAMICO
                            CuerpoControles += "<div class='JCELDA-" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) + "'><div class='JDIV-CONTROLES'>"
                            CuerpoControles += "<span class='JETIQUETA_3'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1
                        End If

                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "W" Then 'PARECIDO AL RADIOTBUTTON 'R' PERO CON ETIQUETA DINAMICO(ANCHO DINAMICO)
                            CuerpoControles += "<div class='JCELDA-" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) + "'><div class='JDIV-CONTROLES'>"
                            CuerpoControles += "<span class='JETIQUETA_2'>" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "</span>"
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1


                            oRceAnamnesisE.IdeExamenFisico = CType(TablaControles.Rows(indice2)("ide_examenfisico").ToString().Trim(), Integer)
                            oRceAnamnesisE.IdeHistoria = Historia
                            oRceAnamnesisE.Orden = 5
                            TablaGrupoControles_Auxiliar = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                            Dim ValorAuxiliarImagen As String = "" '08/09/2016


                            'INICIO - JB - NUEVO - 16/04/2019
                            Dim CadenaEvento As String = ""
                            If TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() <> "" Then
                                CadenaEvento = "onchange='" + TablaControles.Rows(indice2)("dsc_formula").ToString().Trim() + "'" 'JB - se cambia el onblur por onchange - 26/12/2019
                            End If
                            'FIN - JB - NUEVO - 16/04/2019


                            For indice3 = 0 To TablaGrupoControles_Auxiliar.Rows.Count() - 1
                                If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "R" Then
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";").Length > 3 Then
                                    Else
                                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                    End If

                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";").Length > 3 Then
                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                                CuerpoControles += "</div></div>"

                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<input " + CadenaEvento + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />"
                                                CuerpoControles += "</div></div>"
                                            Else
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>" +
                                                "<input " + CadenaEvento + "  id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />"
                                            End If
                                        Else
                                            If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";").Length > 3 Then
                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                                CuerpoControles += "</div></div>"

                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<input " + CadenaEvento + "  disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />"
                                                CuerpoControles += "</div></div>"
                                            Else
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>" +
                                                "<input " + CadenaEvento + "  disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />"
                                            End If

                                        End If
                                    Else
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";").Length > 3 Then
                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                                CuerpoControles += "</div></div>"

                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<input " + CadenaEvento + "  id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />"
                                                CuerpoControles += "</div></div>"
                                            Else
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>" +
                                                "<input " + CadenaEvento + "  id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />"
                                            End If
                                        Else
                                            If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";").Length > 3 Then
                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                                CuerpoControles += "</div></div>"

                                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(3)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                                CuerpoControles += "<input " + CadenaEvento + "  disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />"
                                                CuerpoControles += "</div></div>"
                                            Else
                                                CuerpoControles += "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>" +
                                                "<input " + CadenaEvento + "  disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />"
                                            End If

                                        End If
                                    End If
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";").Length > 3 Then
                                    Else
                                        CuerpoControles += "</div></div>"
                                    End If
                                ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "T" Then
                                    'INICIO - JB - 09/04/2019 - PARA CIRUGIA APLICAR UN SALTO DE LINEA
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() = "Cirugia" Then
                                        CuerpoControles += "<div class='JCELDA-12'></div>"
                                    End If
                                    'FIN - JB - 09/04/2019

                                    'INICIO - JB - 09/04/2019 - validando para no generar etiqueta si esta vacio
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() <> "" Then
                                        CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                        CuerpoControles += "<span class='JETIQUETA'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        CuerpoControles += "</div></div>"
                                    End If
                                    'FIN - JB - 09/04/2019

                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                        CuerpoControles += "<input maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                    Else
                                        CuerpoControles += "<input disabled='disabled' maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                    End If
                                    CuerpoControles += "</div></div>"
                                ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "O" Then
                                    CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                    CuerpoControles += "<span class='JETIQUETA'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    CuerpoControles += "</div></div>"

                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                        CuerpoControles += "<select id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "
                                        'llenado de select aqui                                    
                                        CuerpoControles += "</select>"
                                    Else
                                        CuerpoControles += "<select disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' style='height:20px;' > "
                                        'llenado de select aqui                                    
                                        CuerpoControles += "</select>"
                                    End If
                                    CuerpoControles += "</div></div>"
                                End If
                            Next
                            ContadorColumna += 1
                        End If
                        'FIN - JB - NUEVO CODIGO - 10/04/2019


                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "D" Then 'D - DESCRIPCION/TEXTAREA
                            CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                            If TablaControles.Rows(indice2)("flg_enabled") = True Then
                                If TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(3).Trim() <> "" Then
                                    CuerpoControles += "<textarea maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' rows='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(3).Trim() + "' cols='1' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "</textarea>"
                                Else
                                    CuerpoControles += "<textarea maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' rows='5' cols='1' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "</textarea>"
                                End If
                            Else
                                If TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(3).Trim() <> "" Then
                                    CuerpoControles += "<textarea disabled='disabled' maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' rows='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(3).Trim() + "' cols='1' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "</textarea>"
                                Else
                                    CuerpoControles += "<textarea disabled='disabled' maxlength='" + TablaControles.Rows(indice2)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' rows='5' cols='1' runat='server' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice2)("valor_control").ToString().Trim() + "</textarea>"
                                End If

                            End If
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1
                        End If
                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "B" Then 'B - BOTONES *************************
                            CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                            If TablaControles.Rows(indice2)("flg_enabled") = True Then
                                CuerpoControles += "<input type='button' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "' id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' value='" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "' />"
                            Else
                                CuerpoControles += "<input disabled='disabled' type='button' class='" + TablaControles.Rows(indice2)("dsc_variables").ToString().Split(";")(2) + "'  id='" + TablaControles.Rows(indice2)("dsc_txtidcampo").ToString().Trim() + "' value='" + TablaControles.Rows(indice2)("txt_detalle").ToString().Trim() + "' />"
                            End If
                            CuerpoControles += "</div></div>"
                            ContadorColumna += 1
                        End If

                        If TablaControles.Rows(indice2)("est_tipomedida").ToString().Trim() = "K" Then 'checkbox
                            oRceAnamnesisE.IdeExamenFisico = CType(TablaControles.Rows(indice2)("ide_examenfisico").ToString().Trim(), Integer)
                            oRceAnamnesisE.IdeHistoria = Historia
                            oRceAnamnesisE.Orden = 5
                            TablaGrupoControles_Auxiliar = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                            For indice3 = 0 To TablaGrupoControles_Auxiliar.Rows.Count() - 1
                                If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "K" Then 'JB - 18/07/2019 - NUEVA CONDICION
                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked'  />" +
                                             "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked'  />" +
                                            "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    Else
                                        If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                            CuerpoControles += "<input id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />" +
                                            "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        Else
                                            CuerpoControles += "<input disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />" +
                                            "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        End If
                                    End If

                                    'If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                    '    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                    '        CuerpoControles += "<input " + CadenaEvento2 + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='true'  />" +
                                    '         "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    '    Else
                                    '        CuerpoControles += "<input " + CadenaEvento2 + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='true'  />" +
                                    '        "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    '    End If
                                    'Else
                                    '    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                    '        CuerpoControles += "<input " + CadenaEvento2 + " id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />" +
                                    '        "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    '    Else
                                    '        CuerpoControles += "<input " + CadenaEvento2 + " disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />" +
                                    '        "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    '    End If
                                    'End If

                                    CuerpoControles += "</div></div>"
                                ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "T" Then
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() <> "" Then
                                        CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                        CuerpoControles += "<span class='JETIQUETA'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                        CuerpoControles += "</div></div>"
                                    End If

                                    CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                        CuerpoControles += "<input maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                    Else
                                        CuerpoControles += "<input disabled='disabled' maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                    End If
                                    CuerpoControles += "</div></div>"
                                End If

                            Next
                            ContadorColumna += 1
                        End If


                        If ContadorColumna = CantidadColumnasGrupo Then
                            CuerpoControles += "</div>"

                            'INICIO - JB - NUEVA CONDICION - 10/04/2019
                            If (indice2 + 1) < TablaControles.Rows.Count Then
                                If TablaControles.Rows(indice2 + 1)("dsc_variables").ToString().Trim().Split(";")(2) = "JFLOTA-I" Or
                                TablaControles.Rows(indice2 + 1)("dsc_variables").ToString().Trim().Split(";")(2) = "JFLOTA-D" Then
                                    If Division = TablaControles.Rows(indice2 + 1)("dsc_variables").ToString().Trim().Split(";")(2) Then
                                        CuerpoControles += "</span>"
                                        CuerpoControles += "<span class='" + TablaControles.Rows(indice2 + 1)("dsc_variables").ToString().Trim().Split(";")(2) + "'>"
                                    Else
                                        CuerpoControles += "</span></div>"
                                        CuerpoControles += "<div class='JCELDA-6'><span class='" + TablaControles.Rows(indice2 + 1)("dsc_variables").ToString().Trim().Split(";")(2) + "'>"
                                        Division = TablaControles.Rows(indice2 + 1)("dsc_variables").ToString().Trim().Split(";")(2)
                                    End If

                                End If
                            Else
                                If bFloatamiento = True Then
                                    CuerpoControles += "</span></div>"
                                    bFloatamiento = False
                                End If
                            End If
                            'FIN - JB - NUEVA CONDICION - 10/04/2019

                            CuerpoControles += "<div class='JFILA'>"
                            ContadorColumna = 0
                        End If
                    Next
                    If bFloatamiento = True Then 'JB - NUEVO CODIGO - 10/04/2019
                        CuerpoControles += "</span>" 'cerrando jfloat
                    End If
                    CuerpoControles += "</div>" 'cerrando fila
                    CuerpoControles += "</div>"
                Next
                CuerpoTabHijo += "</ul></div>"
                CuerpoTabHijo += CuerpoControles
                CuerpoTabHijo += "</div></div></div></div>"

                Return CuerpoTabHijo
            Else 'SI NO ES G SOLO TENDRA CONTROLES
                oRceAnamnesisE.IdeExamenFisico = IdeExamenFisico
                oRceAnamnesisE.IdeHistoria = Historia
                oRceAnamnesisE.IdeTipoAtencion = TipoAtencion
                If IsNothing(Session(sCodMedico)) Then
                    oRceAnamnesisE.CodMedico = ""
                Else
                    oRceAnamnesisE.CodMedico = Session(sCodMedico)
                End If
                oRceAnamnesisE.FlgEstado = "A"
                oRceAnamnesisE.Orden = 4
                TablaControles = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

                CuerpoControles += "<div class='JFILA'>"
                Dim ContadorColumna As Integer = 0

                For indice = 0 To TablaControles.Rows.Count() - 1
                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim().ToUpper() <> "C" And TablaControles.Rows(indice)("txt_detalle").ToString().Trim().ToUpper() <> "" And TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() <> "L" _
                            And TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() <> "B" And TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() <> "R" Then 'C y L es una caso especial y las etiquetas seran diferentes / B es un boton / R ahora tendra etiqueta dinamica 26/12/2019
                        CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>" 'AQUI POSIBLE ETIQUETA DINAMICA
                        CuerpoControles += "<span class='JETIQUETA'>" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "</span>"
                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1
                    End If

                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim().ToUpper() = "T" Or TablaControles.Rows(indice)("est_tipomedida").ToString().Trim().ToUpper() = "N" Then
                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                        Dim CadenaValidacionControl As String = ""

                        If TablaControles.Rows(indice)("cnt_minmedida").ToString().Trim() <> "0" Or TablaControles.Rows(indice)("cnt_maxmedida").ToString().Trim() <> "0" Then
                            'Observaciones Cmendez 02/05/2022
                            CadenaValidacionControl += "onblur=" + "fn_ValidaControl(" + TablaControles.Rows(indice)("cnt_minmedida").ToString().Trim() + "|" + TablaControles.Rows(indice)("cnt_maxmedida").ToString().Trim() + "|'" + TablaControles.Rows(indice)("dsc_msjvalidacion").ToString().Trim().Replace(" ", "&nbsp;") + "'|'" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "');" + " "
                        End If
                        If TablaControles.Rows(indice)("dsc_formula").ToString().Trim() <> "" Then
                            CadenaValidacionControl += "onkeyup=" + TablaControles.Rows(indice)("dsc_formula").ToString().Trim() + ""
                        End If

                        If TablaControles.Rows(indice)("flg_enabled") = True Then
                            CuerpoControles += "<input " + CadenaValidacionControl + " maxlength='" + TablaControles.Rows(indice)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' value='" + TablaControles.Rows(indice)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TablaControles.Rows(indice)("dsc_medida").ToString().Trim() + "</span>"
                        Else
                            CuerpoControles += "<input disabled='disabled' maxlength='" + TablaControles.Rows(indice)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' value='" + TablaControles.Rows(indice)("valor_control").ToString().Trim() + "' />" + "<span class='JETIQUETA_4'>" + TablaControles.Rows(indice)("dsc_medida").ToString().Trim() + "</span>"
                        End If
                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1
                    End If
                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "R" Then
                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>" 'AQUI POSIBLE ETIQUETA DINAMICA
                        CuerpoControles += "<span class='JETIQUETA'>" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "</span>"
                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1



                        oRceAnamnesisE.IdeExamenFisico = CType(TablaControles.Rows(indice)("ide_examenfisico").ToString().Trim(), Integer)
                        oRceAnamnesisE.IdeHistoria = Historia
                        oRceAnamnesisE.Orden = 5
                        TablaGrupoControles_Auxiliar = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                        For indice3 = 0 To TablaGrupoControles_Auxiliar.Rows.Count() - 1
                            Dim Clase As String = "" 'JB - 30/12/2019 - NUEVA LINEA
                            If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(2).Split(" ").Length > 1 Then
                                Clase = TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(2).Split(" ")(1)
                            End If



                            If TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "R" Then
                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + " " + Clase + "'><div class='JDIV-CONTROLES'>"

                                If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                        CuerpoControles += "<input id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />" +
                                         "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    Else
                                        CuerpoControles += "<input disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "' checked='true'  />" +
                                         "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    End If
                                Else
                                    If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                        CuerpoControles += "<input id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                         "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    Else
                                        CuerpoControles += "<input disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='radio' runat='server' name='" + TablaGrupoControles_Auxiliar.Rows(indice3)("ide_examenfisico").ToString().Trim() + "'  />" +
                                         "<span class='JETIQUETA_2'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                    End If
                                End If
                                CuerpoControles += "</div></div>"
                            ElseIf TablaGrupoControles_Auxiliar.Rows(indice3)("est_tipomedida").ToString().Trim() = "T" Then
                                CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>" 'JB - 23/12/2019 - se cambio JCELDA-1 a JCELDA-2
                                CuerpoControles += "<span class='JETIQUETA'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                CuerpoControles += "</div></div>"

                                CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"

                                If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                    CuerpoControles += "<input maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                Else
                                    CuerpoControles += "<input disabled='disabled' maxlength='" + TablaGrupoControles_Auxiliar.Rows(indice3)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='text' runat='server' class='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Trim().Split(";")(2) + "' value='" + TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() + "' /> " + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_medida").ToString().Trim()
                                End If
                                CuerpoControles += "</div></div>"
                            End If
                        Next
                        ContadorColumna += 1
                    End If
                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "C" Then
                        Dim aDetalle(3) As String
                        aDetalle = TablaControles.Rows(indice)("txt_detalle").ToString().Trim().Split("*")

                        For indice_array = 0 To aDetalle.Length - 1
                            If indice_array = aDetalle.Length - 1 Then 'el ultimo valor es uan descripcion larga por lo que tendra un tamaño de celda mayor
                                CuerpoControles += "<div class='JCELDA-4'><div class='JDIV-CONTROLES'>"
                                CuerpoControles += "<span class='JETIQUETA_2'>" + aDetalle(indice_array).ToString().Trim() + "</span>"
                                CuerpoControles += "</div></div>"
                            ElseIf indice_array = 0 Then
                                CuerpoControles += "<div class='JCELDA-1'><div class='JDIV-CONTROLES'>"
                                CuerpoControles += "<span class='JETIQUETA_2'>" + aDetalle(indice_array).ToString().Trim() + "</span>"
                                CuerpoControles += "</div></div>"
                            Else
                                CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"
                                CuerpoControles += "<span class='JETIQUETA_2'>" + aDetalle(indice_array).ToString().Trim() + "</span>"
                                CuerpoControles += "</div></div>"
                            End If
                            ContadorColumna += 1
                        Next
                        CuerpoControles += "<div class='JCELDA-2'><div class='JDIV-CONTROLES'>"

                        If TablaControles.Rows(indice)("flg_enabled") = True Then
                            If TablaControles.Rows(indice)("valor_control").ToString().Trim() = "1" Then
                                CuerpoControles += "<input id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked' />"
                            Else
                                CuerpoControles += "<input id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />"
                            End If
                        Else
                            If TablaControles.Rows(indice)("valor_control").ToString().Trim() = "1" Then
                                CuerpoControles += "<input disabled='disabled' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked' />"
                            Else
                                CuerpoControles += "<input disabled='disabled' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' />"
                            End If
                        End If
                        CuerpoControles += "</div></div>"
                        CuerpoControles += "<br />"
                        ContadorColumna += 1
                    End If

                    'INICIO - JB - NUEVO CODIGO
                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "X" Then 'ETIQUETAS COLOR VERDE  CON ANCHO DINAMICO
                        CuerpoControles += "<div class='JCELDA-" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0) + "'><div class='JDIV-CONTROLES'>"
                        CuerpoControles += "<span class='JETIQUETA_3'>" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "</span>"
                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1
                    End If
                    'FIN - JB - NUEVO CODIGO

                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "D" Then 'D - DESCRIPCION/TEXTAREA
                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                        If TablaControles.Rows(indice)("flg_enabled") = True Then
                            If TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(3).ToString().Trim() <> "" Then
                                CuerpoControles += "<textarea maxlength='" + TablaControles.Rows(indice)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' rows='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(3).ToString().Trim() + "' cols='1' runat='server' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice)("valor_control").ToString().Trim() + "</textarea>"
                            Else
                                CuerpoControles += "<textarea maxlength='" + TablaControles.Rows(indice)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' rows='5' cols='1' runat='server' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice)("valor_control").ToString().Trim() + "</textarea>"
                            End If
                        Else
                            If TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(3).ToString().Trim() <> "" Then
                                CuerpoControles += "<textarea disabled='disabled' maxlength='" + TablaControles.Rows(indice)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' rows='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(3).ToString().Trim() + "' cols='1' runat='server' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice)("valor_control").ToString().Trim() + "</textarea>"
                            Else
                                CuerpoControles += "<textarea disabled='disabled' maxlength='" + TablaControles.Rows(indice)("cnt_maxcaracteres").ToString().Trim() + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' rows='5' cols='1' runat='server' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' >" + TablaControles.Rows(indice)("valor_control").ToString().Trim() + "</textarea>"
                            End If
                        End If

                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1
                    End If
                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "B" Then 'B - BOTONES
                        CuerpoControles += "<div class='JCELDA-" + IIf(TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(0)).ToString() + "'><div class='JDIV-CONTROLES'>"
                        If TablaControles.Rows(indice)("flg_enabled") = True Then
                            CuerpoControles += "<input type='button' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' value='" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "' />"
                        Else
                            CuerpoControles += "<input disabled='disabled' class='" + TablaControles.Rows(indice)("dsc_variables").ToString().Split(";")(2) + "' type='button' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "' value='" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "' />"
                        End If
                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1
                    End If
                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "K" Then 'checkbox *****************
                        oRceAnamnesisE.IdeExamenFisico = CType(TablaControles.Rows(indice)("ide_examenfisico").ToString().Trim(), Integer)
                        oRceAnamnesisE.IdeHistoria = Historia
                        oRceAnamnesisE.Orden = 5
                        TablaGrupoControles_Auxiliar = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)
                        For indice3 = 0 To TablaGrupoControles_Auxiliar.Rows.Count() - 1
                            Dim Clase As String = "" 'JB - 15/08/2019 - NUEVA LINEA
                            If TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(2).Split(" ").Length > 1 Then
                                Clase = TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(2).Split(" ")(1)
                            End If

                            CuerpoControles += "<div class='JCELDA-" + IIf(TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0) = "0", 1, TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_variables").ToString().Split(";")(0)).ToString() + " " + Clase + "'><div class='JDIV-CONTROLES'>"

                            If TablaGrupoControles_Auxiliar.Rows(indice3)("valor_control").ToString().Trim() = "1" Then
                                If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                    CuerpoControles += "<input id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked'  />" +
                                    "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                Else
                                    CuerpoControles += "<input disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server' checked='checked'  />" +
                                    "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                End If
                            Else
                                If TablaGrupoControles_Auxiliar.Rows(indice3)("flg_enabled") = True Then
                                    CuerpoControles += "<input id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server'   />" +
                                    "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                Else
                                    CuerpoControles += "<input disabled='disabled' id='" + TablaGrupoControles_Auxiliar.Rows(indice3)("dsc_txtidcampo").ToString().Trim() + "' type='checkbox' runat='server'   />" +
                                    "<span class='JETIQUETA_4'>" + TablaGrupoControles_Auxiliar.Rows(indice3)("txt_detalle").ToString().Trim() + "</span>"
                                End If

                            End If
                            CuerpoControles += "</div></div>"
                        Next
                        ContadorColumna += 1
                    End If

                    If TablaControles.Rows(indice)("est_tipomedida").ToString().Trim() = "L" Then
                        CuerpoControles += "<div class='JCELDA-12'><div class='JDIV-CONTROLES'>"
                        If TablaControles.Rows(indice)("dsc_variables").ToString().Trim().Split(";")(1) = "N" Then
                            CuerpoControles += "<span class='JETIQUETA_3' id='" + TablaControles.Rows(indice)("dsc_txtidcampo").ToString().Trim() + "'><strong>" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "</strong></span>"
                        ElseIf TablaControles.Rows(indice)("dsc_variables").ToString().Trim().Split(";")(1) = "S" Then
                            CuerpoControles += "<span class='JETIQUETA_3' style='text-decoration:underline;'>" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "</span>"
                        Else
                            CuerpoControles += "<span class='JETIQUETA_3'>" + TablaControles.Rows(indice)("txt_detalle").ToString().Trim() + "</span>"
                        End If
                        CuerpoControles += "</div></div>"
                        ContadorColumna += 1
                    End If

                    If ContadorColumna = CantidadColumnasGrupo Then
                        CuerpoControles += "</div>"
                        CuerpoControles += "<div class='JFILA'>"
                        ContadorColumna = 0
                    End If
                Next
                Return CuerpoControles
            End If

        End If
    End Function

    ''' <summary>
    ''' GUARDANDO INFORMACION DE LOS CONTROLES DINAMICOS
    ''' </summary>
    ''' <param name="IdControles"></param>
    ''' <param name="ValorControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarActualizarDatos(ByVal IdControles As String, ByVal ValorControl As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarActualizarDatos_(IdControles, ValorControl)
    End Function

    Public Function GuardarActualizarDatos_(ByVal IdControles As String, ByVal ValorControl As String) As String
        'JB - INICIO - COMENTADO
        'Dim tabla As New DataTable()
        'tabla.Columns.Add("dsc_txtidcampo", System.Type.GetType("System.String"))
        'tabla.Columns.Add("valor_resultado_detalle", System.Type.GetType("System.String"))
        'tabla.Columns.Add("ide_examenfisicores", System.Type.GetType("System.Int32"))

        'For index = 0 To IdControles.Split(";").Length - 1
        '    tabla.Rows.Add(IdControles.Split(";")(index), ValorControl.Split(";")(index).Trim().ToUpper(), 0)
        'Next

        'oRceAnamnesisE.IdeHistoria = Session(sIdeHistoria)
        'oRceAnamnesisE.CodigoUsuario = Session(sCodUser)
        'oRceAnamnesisE.IdeTipoAtencion = Session(sTipoAtencion)
        'oRceAnamnesisE.RceTabla = tabla

        'oRceAnamnesisE = oRceAnamnesisLN.Sp_RceResultadoExamenFisicoDet_Insert2(oRceAnamnesisE)
        'Return oRceAnamnesisE.Resultado.ToString()
        'JB - FIN - COMENTADO

        Dim tabla As New DataTable()
        tabla.Columns.Add("dsc_txtidcampo", System.Type.GetType("System.String"))
        tabla.Columns.Add("valor_resultado_detalle", System.Type.GetType("System.String"))
        tabla.Columns.Add("ide_examenfisicores", System.Type.GetType("System.Int32"))

        'Comentado Christian Méndez
        'Validar caracter extraño; por palote |
        'Se cambio el Split(";") por el Split("|")
        For index = 0 To IdControles.Split("|").Length - 1
            If IdControles.Split("|").Length > 1 Then
                tabla.Rows.Add(IdControles.Split("|")(index), ValorControl.Split("|")(index).Trim().ToUpper(), 0)
            End If
        Next
        'Fin

        If tabla.Rows.Count > 1 Then
            oRceAnamnesisE.IdeHistoria = Session(sIdeHistoria)
            oRceAnamnesisE.CodigoUsuario = Session(sCodUser)
            oRceAnamnesisE.IdeTipoAtencion = Session(sTipoAtencion)
            oRceAnamnesisE.RceTabla = tabla
            oRceAnamnesisE.IdeExamenFisico = 0
            oRceAnamnesisE.TxtDetalle = ""
            oRceAnamnesisE.Orden = 2

            Dim i As Integer = oRceAnamnesisLN.Sp_RceResultadoExamenFisicoDet_Insert4(oRceAnamnesisE)
            Return "-9" 'i.ToString()

        Else
            oRceAnamnesisE.IdeHistoria = Session(sIdeHistoria)
            oRceAnamnesisE.CodigoUsuario = Session(sCodUser)
            oRceAnamnesisE.IdeTipoAtencion = Session(sTipoAtencion)
            oRceAnamnesisE.RceTabla = tabla 'sera una tabla vacia
            oRceAnamnesisE.DscTxtIdCampo = IdControles.Split("-")(0)
            oRceAnamnesisE.IdeExamenFisico = IdControles.Split("-")(1)
            oRceAnamnesisE.TxtDetalle = ValorControl.Trim().ToUpper()
            oRceAnamnesisE.Orden = 1

            Dim i As Integer = oRceAnamnesisLN.Sp_RceResultadoExamenFisicoDet_Insert4(oRceAnamnesisE)
            'Return i.ToString()
            Return "-9"
        End If

    End Function



    Public Function ArmandoTabPrincipalV2(ByVal TipoDeAtencion As String) As String
        Dim tabla As New DataTable()
        Dim TablaGrupo As New DataTable()
        Dim CadenaHTML As String = ""
        Dim CuerpoTab As String = ""
        Dim TipoAtencion As String = Session(sCodigoAtencion).ToString().Substring(0, 1)
        If TipoDeAtencion.Trim() <> "" Then
            TipoAtencion = TipoDeAtencion
        End If
        If TipoAtencion = "E" Then 'Para atencion E se usara un SP distinto
            Return CargandoControlesDinamicosE()
        End If


        Session(sTipoAtencion) = TipoAtencion
        oRceAnamnesisE.IdeExamenFisico = 0
        oRceAnamnesisE.IdeHistoria = Session(sIdeHistoria)
        oRceAnamnesisE.IdeTipoAtencion = TipoAtencion

        If IsNothing(Session(sCodMedico)) Then
            oRceAnamnesisE.CodMedico = ""
        Else
            oRceAnamnesisE.CodMedico = Session(sCodMedico)
        End If

        'oRceAnamnesisE.CodMedico = Session(sCodMedico)
        oRceAnamnesisE.FlgEstado = "A"
        oRceAnamnesisE.Orden = 1
        tabla = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

        CadenaHTML = "<div class='JSBTABS'><label for='chkTABS' class='JSBMOSTRAR_TABS'></label><input type='checkbox' id='chkTABS' class='chkTAB-CHECK' /><ul>"
        For index = 0 To tabla.Rows.Count() - 1
            If index = 0 Then
                CadenaHTML += "<li class='JSBTAB_ACTIVO tabs_historiaclinica' id='" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "'><a>" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "</a></li>"
            Else
                CadenaHTML += "<li class='tabs_historiaclinica'  id='" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "'><a>" + tabla.Rows(index)("txt_detalle").ToString().Trim() + "</a></li>"
            End If

        Next
        CadenaHTML += "</ul></div>"

        For index = 0 To tabla.Rows.Count() - 1
            '*****************GRUPO************************
            'CARGANDO LOS VALORES DE LOS GRUPOS
            Dim IdExaFisicoPadre As Integer
            Dim resultado As Boolean = Int32.TryParse(tabla.Rows(index)("ide_examenfisico").ToString().Trim(), IdExaFisicoPadre)
            If resultado = False Then
                oRceAnamnesisE.IdeExamenFisico = 0
            Else
                oRceAnamnesisE.IdeExamenFisico = IdExaFisicoPadre
            End If
            oRceAnamnesisE.IdeHistoria = 0
            oRceAnamnesisE.FlgEstado = "A"
            oRceAnamnesisE.Orden = 2
            TablaGrupo = oRceAnamnesisLN.Sp_RceExamenFisicoMae_Consulta5(oRceAnamnesisE)

            Dim CadenaCuerpo As String = ""
            CuerpoTab = "<div class='JCUERPO'>"
            For indice = 0 To TablaGrupo.Rows.Count() - 1
                CuerpoTab += "<div class='JFILA'><div class='JCELDA-12'><div class='JDIV-CONTROLES JDIV-GRUPO' id='" +
                        TablaGrupo.Rows(indice)("ide_examenfisico").ToString().Trim() + "' name='" + TablaGrupo.Rows(indice)("cnt_minmedida").ToString().Trim() + "-" + TablaGrupo.Rows(indice)("est_tipomedida").ToString().Trim() + "'>"
                CuerpoTab += TablaGrupo.Rows(indice)("txt_detalle").ToString().Trim()
                CuerpoTab += "</div></div></div>"
                'CuerpoTab += CuerpoGrupo(TablaGrupo, indice)

                Dim valor1 As String = ""
                valor1 = TablaGrupo.Rows(indice)("ide_examenfisico").ToString().Trim()
                Dim valor2 As String = ""
                valor2 = TablaGrupo.Rows(indice)("cnt_minmedida").ToString().Trim() + "-" + TablaGrupo.Rows(indice)("est_tipomedida").ToString().Trim()
                Dim secciondata As String = TablaGrupo.Rows(indice)("txt_detalle").ToString().Trim()
                CadenaCuerpo = CargarDatosControlesDinamicoGrupo_(valor1, valor2, Session(sIdeHistoria), TipoAtencion, secciondata)
                CuerpoTab += "<div class='JGRUPO-CTRL'>" + CadenaCuerpo + "</div>"

                If indice > 0 Then
                    CuerpoTab += "</div>"
                End If

            Next
            'Dim ValorExtra As String = ""
            'ValorExtra = "</div>"
            'CuerpoTab += "</div>" + ValorExtra
            '***************FIN GRUPO**********************
            CadenaHTML += CuerpoTab
        Next

        'INICIO - JB - ESTE CODIGO GUARDA LA ESTRUCTURA EN UN BLOCK DE NOTAS
        'Dim sw As New StreamWriter("C:\\TEMP\\CuerpoHTML.txt")
        'sw.WriteLine(CadenaHTML + "</div></div>")
        'sw.Close()
        'FIN - JB

        Return CadenaHTML + "</div></div>"
    End Function


    ''' <summary>
    ''' FUNCION PARA CONSULTA NOTA DE INGRESO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultarNotaIngreso() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultarNotaIngreso_
    End Function
    Public Function ConsultarNotaIngreso_() As String
        oRceNotaIngresoE.IdHistoria = Session(sIdeHistoria)
        Dim tabla As New DataTable()
        tabla = oRceNotaIngresoLN.Sp_RceNotaIngreso_Consulta(oRceNotaIngresoE)
        Dim Cadena As String = ""
        If tabla.Rows.Count > 0 Then
            For index = 0 To tabla.Rows.Count - 1
                Cadena += "<div class='JFILA'>"
                Cadena += "<div class='JCELDA-3'>"
                Cadena += "<span class='JETIQUETA_3'>" + tabla.Rows(index)("Fecha").ToString() + "<span>"
                Cadena += "</div>"
                Cadena += "<div class='JCELDA-5'>"
                Cadena += "<span class='JETIQUETA_3'>DR(a) " + tabla.Rows(index)("NombreMedico").ToString() + "<span>"
                Cadena += "</div>"
                Cadena += "<div class='JCELDA-3'>"
                Cadena += "<span class='JETIQUETA_3'>" + IIf(tabla.Rows(index)("dsc_medico_tratante").ToString() = "", "Intensivista", "Médico tratante") + "<span>"
                Cadena += "</div>"
                Cadena += "</div>"
                Cadena += "<div class='JFILA'>"
                Cadena += "<div class='JCELDA-12'>"
                Cadena += "<pre style='margin:0;white-space:pre-wrap;'><code class='JETIQUETA_2'> " + (IIf(tabla.Rows(index)("dsc_medico_tratante").ToString() = "", tabla.Rows(index)("dsc_intensivista").ToString(), tabla.Rows(index)("dsc_medico_tratante").ToString())).ToString().Replace("<", "&lt;").Replace(">", "&gt;") 'dsc_ingreso
                Cadena += "</code></pre>"
                Cadena += "</div>"
                Cadena += "</div>"

            Next
        End If

        Return Cadena
    End Function


    ''' <summary>
    ''' FUNCION PARA GUARDAR NOTA DE INGRESO
    ''' </summary>
    ''' <param name="IdControl"></param>
    ''' <param name="ValorControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarNotaIngreso(ByVal IdControl As String, ByVal ValorControl As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarNotaIngreso_(IdControl, ValorControl)
    End Function
    Public Function GuardarNotaIngreso_(ByVal IdControl As String, ByVal ValorControl As String) As String
        'INICIO - 16/01/2017 - JB  -  COMENTADO - 09/06/2020
        'Dim ResultadoValidacion As String = ValidaEventoBoton("11/01/01")
        'If ResultadoValidacion <> "OK" Then
        '    If ResultadoValidacion <> "" Then
        '        Return ResultadoValidacion
        '    Else
        '        Return "V;El registro es no editable"
        '    End If

        'End If
        'FIN - 16/01/2017 - JB -  COMENTADO - 09/06/2020
        oRceNotaIngresoE.IdHistoria = Session(sIdeHistoria)
        oRceNotaIngresoE.CodMedico = Session(sCodMedico)
        oRceNotaIngresoE.IdUsuario = Session(sCodUser)
        oRceNotaIngresoE.IdCampo = IdControl
        oRceNotaIngresoE.ValorCampo = ValorControl.Trim().ToUpper()
        Dim RegistrosGuardados As Integer
        RegistrosGuardados = oRceNotaIngresoLN.Sp_RceNotaIngreso_Insert(oRceNotaIngresoE)

        oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
        oRceInicioSesionE.CodUser = Session(sCodUser)
        oRceInicioSesionE.Formulario = "InformacionPaciente"
        oRceInicioSesionE.Control = "NOTA DE INGRESO"
        oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
        oRceInicioSesionE.DscPcName = Session(sDscPcName)
        oRceInicioSesionE.DscLog = "Se agrego la nota de ingreso NRO " + oRceNotaIngresoE.IdNotaIngreso.ToString()
        oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)

        'SI SE GUARDARON LOS DATOS, LLAMAREMOS AL METODO QUE CARGA LA NOTA DE INGRESO PARA MOSTRARLO
        If RegistrosGuardados <> 0 Then
            Return ConsultarNotaIngreso_()
        Else
            Return ""
        End If
    End Function



    ''' <summary>
    ''' FUNCION QUE CARGA DATOS DE EVOLUCION CLINICA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultarEvolucionClinica() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultarEvolucionClinica_
    End Function
    Public Function ConsultarEvolucionClinica_() As String
        oRceEvolucionE.CodigoAtencion = Session(sCodigoAtencion)
        oRceEvolucionE.Orden = 0
        Dim tabla As New DataTable()
        tabla = oRceEvolucionLN.Sp_RceEvolucion_Consulta(oRceEvolucionE)
        Dim CadenaEstructura As String = ""
        If tabla.Rows.Count > 0 Then
            For index = 0 To tabla.Rows.Count - 1
                CadenaEstructura += "<div class='JFILA'>"
                CadenaEstructura += "<div class='JFILA-SELECCIONEVOLUCION' style='cursor:pointer;'>"
                CadenaEstructura += "<div class='JCELDA-3'>"
                CadenaEstructura += "<span class='JETIQUETA_3' id='spFechaEvolucionClinica" + index.ToString() + "'>" + tabla.Rows(index)("fec_registra").ToString().Trim() + "</span>"
                CadenaEstructura += "</div>"
                CadenaEstructura += "<div class='JCELDA-5'>"
                CadenaEstructura += "<span class='JETIQUETA_3' id='spDoctorEvolucionClinica" + index.ToString() + "'>DR (a) " + tabla.Rows(index)("MedicoRegistra").ToString().Trim() + "</span>"
                CadenaEstructura += "</div>"
                CadenaEstructura += "<div class='JCELDA-3'>"
                CadenaEstructura += "<span class='JETIQUETA_3' id='spTipoEvolucionClinica" + index.ToString() + "'>" + tabla.Rows(index)("TipoEvolucion").ToString().Trim() + "</span>"   'TipoEvolucion
                CadenaEstructura += "</div>"
                CadenaEstructura += "<div class='JCELDA-1' style='display:none;'>"
                CadenaEstructura += "<span class='JIDE_EVOLUCION' id='spIdeEvolucion" + index.ToString() + "'>" + tabla.Rows(index)("ide_evolucion").ToString().Trim() + "</span>"
                CadenaEstructura += "</div>"
                CadenaEstructura += "</div>"
                CadenaEstructura += "</div>"

                CadenaEstructura += "<div class='JFILA'>"
                CadenaEstructura += "<div class='JCELDA-10'>"
                CadenaEstructura += "<pre style='margin:0;white-space:pre-wrap;'><code class='JETIQUETA_2' id='spDescripcionEvolucionClinica" + index.ToString() + "'>" + tabla.Rows(index)("txt_resumen").ToString().Trim().Replace("<br>", "SGL_BERE").Replace("<b>", "SGL_BEBE").Replace("<", "&lt;").Replace(">", "&gt;").Replace("SGL_BERE", "<br>").Replace("SGL_BEBE", "<b>") + "</code></pre>"
                CadenaEstructura += "</div>"
                CadenaEstructura += "</div>"
            Next
        End If
        Return CadenaEstructura
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultarEvolucionClinica2(ByVal Valor As String, ByVal Orden As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultarEvolucionClinica2_(Valor, Orden)
    End Function

    Public Function ConsultarEvolucionClinica2_(ByVal Valor As String, ByVal Orden As String) As String
        Try
            Dim oRceEvolucionE_ As New RceEvolucionE
            Dim oRceEvolucionLN_ As New RceEvolucionLN

            If Orden = 1 Then
                oRceEvolucionE_.IdHistoria = Session(sIdeHistoria)
            End If
            If Orden = 2 Then
                oRceEvolucionE_.IdHistoria = Session(sIdeHistoria)
                oRceEvolucionE_.FecEvolucion = Valor
            End If
            If Orden = 3 Then
                oRceEvolucionE_.IdHistoria = Session(sIdeHistoria)
                oRceEvolucionE_.CodigoEvolucion = Valor
            End If
            oRceEvolucionE_.Orden = Orden

            Dim tabla As New DataTable()
            tabla = oRceEvolucionLN_.Sp_RceEvolucion_ConsultaV2(oRceEvolucionE_)
            Dim ValorDevolver As String = ""

            If tabla.Rows.Count > 0 And Orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-FECHA' style='color:#8DC73F;display:inline-block;'>" + tabla.Rows(index1)("FEC_REGISTRO").ToString() + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And Orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA' style='white-space:pre-wrap;'>" + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " - " + tabla.Rows(index1)("ESPECIALIDAD").ToString().Trim() + " | " + tabla.Rows(index1)("HOR_REGISTRO").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("IDE_EVOLUCION").ToString().Trim() + "'  />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And Orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("txt_resumen").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_evolucion").ToString().Trim() + "'  /> "
                    ValorDevolver += "</div>"

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try


        'Try
        '    Dim oRceJuntaMedicaE As New RceJuntaMedicaE()
        '    Dim oRceJuntaMedicaLN As New RceJuntaMedicaLN()

        '    If Orden = 1 Then
        '        oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
        '    End If
        '    If Orden = 2 Then
        '        oRceJuntaMedicaE.CodAtencion = ""
        '        oRceJuntaMedicaE.IdeJuntaMedica = Valor
        '    End If
        '    oRceJuntaMedicaE.Orden = Orden
        '    Dim tabla As New DataTable()
        '    tabla = oRceJuntaMedicaLN.Sp_RceJuntaMedica_Consulta(oRceJuntaMedicaE)
        '    Dim ValorDevolver As String = ""

        '    If tabla.Rows.Count > 0 And Orden = 1 Then
        '        For index1 = 0 To tabla.Rows.Count - 1
        '            ValorDevolver += "<div class='JTREE3-FECHA'>"

        '            ValorDevolver += "<div class='JFILA-FECHA'>"
        '            ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-FECHA' style='color:#8DC73F;display:inline-block;'>" + tabla.Rows(index1)("fec_registra") + "</div><div style='color:#8DC73F;margin-left:50px;display:inline-block;'>" + tabla.Rows(index1)("nom_medico") + "</div><input type='hidden' class='IdeJuntaMedica' value='" + tabla.Rows(index1)("ide_juntamedica").ToString().Trim() + "' />"
        '            ValorDevolver += "</div>"
        '            ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
        '            ValorDevolver += "</div>"

        '            ValorDevolver += "</div>"
        '        Next
        '    End If
        '    If tabla.Rows.Count > 0 And Orden = 2 Then
        '        For index1 = 0 To tabla.Rows.Count - 1

        '            ValorDevolver += "<div class='JFILA-HORA'>"
        '            ValorDevolver += "<div class='JVALOR-HORA' style='white-space:pre-wrap;'>" + tabla.Rows(index1)("dsc_juntamedica").ToString().Trim() + "</div>"
        '            ValorDevolver += "</div>"
        '            ValorDevolver += "<div class='JFILA-DETALLE'>"
        '            ValorDevolver += "</div>"
        '        Next
        '    End If

        '    Return ValorDevolver
        'Catch ex As Exception
        '    Return "ERROR;" + ex.Message.ToString()
        'End Try
    End Function


    ''' <summary>
    ''' 18/10/2016
    ''' </summary>
    ''' <param name="IdeEvolucion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function CargarDatosEvolucionClinica(ByVal IdeEvolucion As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CargarDatosEvolucionClinica_(IdeEvolucion)
    End Function

    Public Function CargarDatosEvolucionClinica_(ByVal IdeEvolucion As String) As String
        Try
            oRceEvolucionE.CodigoAtencion = IdeEvolucion
            oRceEvolucionE.Orden = 4
            Dim tabla As New DataTable()
            tabla = oRceEvolucionLN.Sp_RceEvolucion_Consulta(oRceEvolucionE)
            Dim ValorDevolver As String = ""
            ValorDevolver += tabla.Rows(0)("txt_subjetiva").ToString.Trim() + ";" + tabla.Rows(0)("txt_objetiva").ToString.Trim() + ";" +
                tabla.Rows(0)("txt_apreciacion").ToString.Trim() + ";" + tabla.Rows(0)("txt_plan").ToString.Trim() + ";" +
                tabla.Rows(0)("cod_tipo").ToString.Trim() + ";" + tabla.Rows(0)("flg_firma").ToString.Trim() + ";" + tabla.Rows(0)("cod_condicion").ToString.Trim()
            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function



    ''' <summary>
    ''' FUNCION PARA GUARDAR LA EVOLUCION CLINICA
    ''' </summary>
    ''' <param name="Analitica"></param>
    ''' <param name="TipoEducacionInforme"></param>
    ''' <param name="Subjetiva"></param>
    ''' <param name="Objetiva"></param>
    ''' <param name="Plan"></param>
    ''' <param name="RequiereFirma"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarEvolucionClinica(ByVal Analitica As String, ByVal TipoEducacionInforme As String, ByVal Subjetiva As String, ByVal Objetiva As String, ByVal Plan As String, ByVal RequiereFirma As String, ByVal TipoEvolucion As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarEvolucionClinica_(Analitica, TipoEducacionInforme, Subjetiva, Objetiva, Plan, RequiereFirma, TipoEvolucion)
    End Function

    Public Function GuardarEvolucionClinica_(ByVal Analitica As String, ByVal TipoEducacionInforme As String, ByVal Subjetiva As String, ByVal Objetiva As String, ByVal Plan As String, ByVal RequiereFirma As String, ByVal TipoEvolucion As String) As String
        Try
            'INICIO - 16/01/2017 - JB -  COMENTADO - 09/06/2020
            'Dim ResultadoValidacion As String = ValidaEventoBoton("02/01/01")
            'If ResultadoValidacion <> "OK" Then
            '    If ResultadoValidacion <> "" Then
            '        Return ResultadoValidacion
            '    Else
            '        Return "V;El registro es no editable"
            '    End If

            'End If
            'FIN - 16/01/2017 - JB -  COMENTADO - 09/06/2020

            oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
            oRceEvolucionE.CodMedico = Session(sCodMedico)
            oRceEvolucionE.Observacion = Analitica.Trim().ToUpper()
            Dim exito As Integer
            exito = oRceEvolucionLN.Sp_RceEvolucion_Insert(oRceEvolucionE)

            If oRceEvolucionE.CodigoEvolucion <> 0 Then
                oRceEvolucionE.Campo = "txt_subjetiva"
                oRceEvolucionE.ValorNuevo = Subjetiva.Trim().ToUpper()
                oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)

                oRceEvolucionE.Campo = "txt_objetiva"
                oRceEvolucionE.ValorNuevo = Objetiva.Trim().ToUpper()
                oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)

                oRceEvolucionE.Campo = "txt_apreciacion"
                oRceEvolucionE.ValorNuevo = Analitica.Trim().ToUpper()
                oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)

                oRceEvolucionE.Campo = "txt_plan"
                oRceEvolucionE.ValorNuevo = Plan.Trim().ToUpper()
                oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)

                oRceEvolucionE.Campo = "cod_tipo"
                oRceEvolucionE.ValorNuevo = TipoEducacionInforme
                oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)

                '14/11/2016
                oRceEvolucionE.Campo = "cod_condicion"
                oRceEvolucionE.ValorNuevo = TipoEvolucion
                oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)
                'FIN 14/11/2016


                'INICIO -JB - 30/11/2020
                'oRceEvolucionE.Campo = "codespecialidad"
                'oRceEvolucionE.ValorNuevo = Session(sCodEspecialidad)
                'oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)
                'FIN - JB - 30/11/2020


                'GUARDANDO LOG
                '11/11/2016
                oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
                oRceInicioSesionE.CodUser = Session(sCodUser)
                oRceInicioSesionE.Formulario = "InformacionPaciente"
                oRceInicioSesionE.Control = "EVOLUCION"
                oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
                oRceInicioSesionE.DscPcName = Session(sDscPcName)
                oRceInicioSesionE.DscLog = "Se agrego la evolucion NRO " + oRceEvolucionE.CodigoEvolucion.ToString()
                oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
                'FIN 11/11/2016

                If RequiereFirma <> "" Then
                    oRceEvolucionE.Campo = "flg_firma"
                    oRceEvolucionE.ValorNuevo = RequiereFirma
                    oRceEvolucionLN.Sp_RceEvolucion_Update(oRceEvolucionE)
                End If
            End If

            'INI 1.4
            ''INICIO - JB - 31/01/2017
            'Dim pdf_byte As Byte() = ExportaPDF("DA")
            ''INICIO - JB - 31/01/2017

            'Dim cn As New SqlConnection(CnnBD)

            ''Paso 1
            'oHospitalE.TipoDoc = 10
            'oHospitalE.CodAtencion = Session(sCodigoAtencion)
            'oHospitalE.CodUser = Session(sCodUser)
            'oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
            'oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            ''Paso 2
            'Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
            'cmd1.CommandType = CommandType.Text
            'cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
            'cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            'cmd1.Parameters.AddWithValue("@flg_reqfirma", RequiereFirma)

            'Dim num1 As Integer
            'cn.Open()
            'num1 = cmd1.ExecuteNonQuery()
            'cn.Close()

            ''Paso 3
            'oHospitalE.IdeHistoria = Session(sIdeHistoria)
            'oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
            'oHospitalE.TipoDoc = 10
            'oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
            'FIN 1.4

            Return ConsultarEvolucionClinica_()
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA GUARDAR CONTROL CLINICO E INDICACIONES MEDICAS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarControlClinico() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarControlClinico_()
    End Function

    Public Function GuardarControlClinico_() As String
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
        Dim aCodigoProducto As String()
        Dim aValoresRegistro As String()
        Dim tabla_p As New DataTable()


        If IsNothing(Session(sTablaProductoMedicamento)) Then
            tabla_p.Columns.Add("Codigo")
            tabla_p.Columns.Add("Producto")
            tabla_p.Columns.Add("Via")
            tabla_p.Columns.Add("CadaHora")
            'tabla_p.Columns.Add("Cantidad")
            tabla_p.Columns.Add("Dosis")
            tabla_p.Columns.Add("Indicacion")
            'tabla_p.Columns.Add("Dia")
            tabla_p.Columns.Add("Cantidad")
            tabla_p.Columns.Add("TipoPedido")
            tabla_p.Columns.Add("TipoPedido2")
            tabla_p.Columns.Add("Prn")
        Else
            tabla_p = CType(Session(sTablaProductoMedicamento), DataTable)
        End If


        aCodigoProducto = New String(tabla_p.Rows.Count - 1) {}
        aValoresRegistro = New String(tabla_p.Rows.Count - 1) {}
        For indice = 0 To tabla_p.Rows.Count - 1
            aCodigoProducto(indice) = tabla_p.Rows(indice)("Codigo").ToString().Trim()
            aValoresRegistro(indice) = IIf(tabla_p.Rows(indice)("CadaHora").ToString().Trim() = "PRN", "", tabla_p.Rows(indice)("CadaHora").ToString().Trim()) _
                + "|" + tabla_p.Rows(indice)("Dosis").ToString().Trim() + "|" + tabla_p.Rows(indice)("Indicacion").ToString().Trim() + "|" + tabla_p.Rows(indice)("Producto").ToString().Trim() + "|" + tabla_p.Rows(indice)("Via").ToString().Trim() _
                + "|" + tabla_p.Rows(indice)("Cantidad").ToString().Trim() + "|" + tabla_p.Rows(indice)("TipoPedido").ToString().Trim()
        Next

        If tabla_p.Rows.Count > 0 Then
            Dim valor As String = aValoresRegistro(aValoresRegistro.Length - 1)
            valor = valor.Remove(valor.Length - 1)
            'aValoresRegistro(aValoresRegistro.Length - 1) = valor JB - COMENTADO - 26/08/2019
        End If

        Dim aCamposAgregar() As String = {"num_frecuencia", "num_dosis", "txt_detalle", "dsc_producto", "dsc_via", "num_cantidad", "tipopedido"}
        Dim MensajeGrabado As String = ""

        Dim dv As New DataView(tabla_p)
        Dim dtTipoPedido As New DataTable
        dtTipoPedido = dv.ToTable(True, "TipoPedido")



        For indexTP = 0 To dtTipoPedido.Rows.Count - 1 'POR CADA TIPO DE PEDIDO GENERARA UNA CABECERA CON SU DETALLE
            Dim InsertCab As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
            If InsertCab > 0 Then
                oRceRecetaMedicamentoE.Campo = "tipopedido"
                oRceRecetaMedicamentoE.ValorNuevo = dtTipoPedido.Rows(indexTP)("TipoPedido").ToString()
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                If dtTipoPedido.Rows(indexTP)("TipoPedido").ToString().Trim() <> "" Then
                    oRceRecetaMedicamentoE.Campo = "pedidogenerado"
                    oRceRecetaMedicamentoE.ValorNuevo = "N"
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                End If

                oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
                For index = 0 To aCodigoProducto.Length - 1
                    If aValoresRegistro(index).Split("|")(6) = dtTipoPedido.Rows(indexTP)("TipoPedido").ToString() Then
                        oRceRecetaMedicamentoE.CodProducto = aCodigoProducto(index)
                        Dim InsertDet As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                        If InsertDet > 0 Then
                            For index1 = 0 To aCamposAgregar.Length - 1
                                oRceRecetaMedicamentoE.Campo = aCamposAgregar(index1)
                                oRceRecetaMedicamentoE.ValorNuevo = aValoresRegistro(index).Split("|")(index1).Trim().ToUpper()
                                Dim UpdateDet As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                                If UpdateDet > 0 Then
                                    MensajeGrabado = "OK"
                                End If
                            Next
                        End If
                    End If

                Next

                oRceRecetaMedicamentoE.Campo = "est_estado"
                oRceRecetaMedicamentoE.ValorNuevo = "A"
                oRceRecetaMedicamentoE.IdReceta = oRceRecetaMedicamentoE.IdReceta
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "tipo"
                oRceRecetaMedicamentoE.ValorNuevo = "F"
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)


                'GUARDANDO LOG
                '11/11/2016
                oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
                oRceInicioSesionE.CodUser = Session(sCodUser)
                oRceInicioSesionE.Formulario = "InformacionPaciente"
                oRceInicioSesionE.Control = "CONTROL CLINICO"
                oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
                oRceInicioSesionE.DscPcName = Session(sDscPcName)
                oRceInicioSesionE.DscLog = "Se agrego la receta Nro " + oRceRecetaMedicamentoE.IdReceta.ToString()
                oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
                'FIN 11/11/2016

            End If
        Next


        Dim tabla_producto As New DataTable()
        'SI YA INSERTE LOS DATOS DEBO LIMPIAR LA TABLA CON LOS PRODUCTOS QUE SE AÑADIERON
        If Not IsNothing(Session(sTablaProductoMedicamento)) Then
            tabla_producto = CType(Session(sTablaProductoMedicamento), DataTable)
            tabla_producto.Rows.Clear()
            Session(sTablaProductoMedicamento) = tabla_producto
        End If


        Return MensajeGrabado
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarNoFarmacologico(ByVal Nutricion As String, ByVal TerapiaFisica As String, ByVal CuidadosEnfermeria As String, ByVal OtrosNoFarmacologico As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarNoFarmacologico_(Nutricion, TerapiaFisica, CuidadosEnfermeria, OtrosNoFarmacologico)
    End Function

    Public Function GuardarNoFarmacologico_(ByVal Nutricion As String, ByVal TerapiaFisica As String, ByVal CuidadosEnfermeria As String, ByVal OtrosNoFarmacologico As String)
        Try
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
            Dim dt As New DataTable()
            Dim InsertCab As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
            If InsertCab > 0 Then
                oRceRecetaMedicamentoE.CodProducto = ""
                'Dim InsertDetN As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)

                If Nutricion.Trim() <> "" Then
                    Dim InsertDetN As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                    oRceRecetaMedicamentoE.Campo = "txt_detalle"
                    oRceRecetaMedicamentoE.ValorNuevo = "NUTRICION"
                    Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                    oRceRecetaMedicamentoE.Campo = "dsc_producto"
                    oRceRecetaMedicamentoE.ValorNuevo = Nutricion.Trim().ToUpper()
                    Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                    oRceRecetaMedicamentoLN.Sp_Rce_Envia_Correo_Dietas(New SisCorreoE(1, Session(sCodPaciente))) '1.1
                End If

                'Dim InsertDetT As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                If TerapiaFisica.Trim() <> "" Then
                    Dim InsertDetT As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                    oRceRecetaMedicamentoE.Campo = "txt_detalle"
                    oRceRecetaMedicamentoE.ValorNuevo = "TERAPIA"
                    Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                    oRceRecetaMedicamentoE.Campo = "dsc_producto"
                    oRceRecetaMedicamentoE.ValorNuevo = TerapiaFisica.Trim().ToUpper()
                    Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                End If

                'Dim InsertDetC As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                If CuidadosEnfermeria.Trim() <> "" Then
                    Dim InsertDetC As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                    oRceRecetaMedicamentoE.Campo = "txt_detalle"
                    oRceRecetaMedicamentoE.ValorNuevo = "CUIDADOS"
                    Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                    oRceRecetaMedicamentoE.Campo = "dsc_producto"
                    oRceRecetaMedicamentoE.ValorNuevo = CuidadosEnfermeria.Trim().ToUpper()
                    Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                End If

                'Dim InsertDetO As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                If OtrosNoFarmacologico.Trim() <> "" Then
                    Dim InsertDetO As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                    oRceRecetaMedicamentoE.Campo = "txt_detalle"
                    oRceRecetaMedicamentoE.ValorNuevo = "OTROS"
                    Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                    oRceRecetaMedicamentoE.Campo = "dsc_producto"
                    oRceRecetaMedicamentoE.ValorNuevo = OtrosNoFarmacologico.Trim().ToUpper()
                    Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                End If
                'If UpdateDet > 0 Then

                'End If
                oRceRecetaMedicamentoE.Campo = "est_estado"
                oRceRecetaMedicamentoE.ValorNuevo = "A"
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "tipo"
                oRceRecetaMedicamentoE.ValorNuevo = "N"
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
            End If

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try



        'Try
        '    oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        '    oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
        '    Dim tabla_p As New DataTable()

        '    Dim InsertCab As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
        '    If InsertCab > 0 Then
        '        oRceRecetaMedicamentoE.Campo = "est_estado"
        '        oRceRecetaMedicamentoE.ValorNuevo = "A"
        '        oRceRecetaMedicamentoE.IdReceta = oRceRecetaMedicamentoE.IdReceta
        '        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

        '        oRceRecetaMedicamentoE.Campo = "dsc_nutricion"
        '        oRceRecetaMedicamentoE.ValorNuevo = Nutricion.Trim().ToUpper()
        '        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

        '        oRceRecetaMedicamentoE.Campo = "dsc_terapiafisica"
        '        oRceRecetaMedicamentoE.ValorNuevo = TerapiaFisica.Trim().ToUpper()
        '        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

        '        oRceRecetaMedicamentoE.Campo = "dsc_cuidadosenfermeria"
        '        oRceRecetaMedicamentoE.ValorNuevo = CuidadosEnfermeria.Trim().ToUpper()
        '        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

        '        oRceRecetaMedicamentoE.Campo = "otras_recomendaciones"
        '        oRceRecetaMedicamentoE.ValorNuevo = OtrosNoFarmacologico.Trim().ToUpper()
        '        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

        '        oRceRecetaMedicamentoE.Campo = "tipo"
        '        oRceRecetaMedicamentoE.ValorNuevo = "N"
        '        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)


        '        'GUARDANDO LOG
        '        '11/11/2016
        '        oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
        '        oRceInicioSesionE.CodUser = Session(sCodUser)
        '        oRceInicioSesionE.Formulario = "InformacionPaciente"
        '        oRceInicioSesionE.Control = "CONTROL CLINICO"
        '        oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
        '        oRceInicioSesionE.DscPcName = Session(sDscPcName)
        '        oRceInicioSesionE.DscLog = "Se agrego la receta Nro " + oRceRecetaMedicamentoE.IdReceta.ToString()
        '        oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
        '        'FIN 11/11/2016
        '    End If
        '    Return "OK"
        'Catch ex As Exception
        '    Return "ERROR" + ";" + ex.Message.ToString()
        'End Try

    End Function



    ' <summary>
    ' FUNCION QUE CARGA DATOS DE CONTROL CLINICO E INDICACIONES MEDICAS - JB -SE COMENTA Y SE REEMPLAZA POR CODIGO NUEVO - 20/08/2019
    ' </summary>
    ' <returns></returns>
    ' <remarks></remarks>
    '<System.Web.Services.WebMethod()>
    'Public Shared Function ConsultaControlClinico() As String
    '    Dim pagina As New InformacionPaciente()
    '    Return pagina.ConsultaControlClinico_()
    'End Function
    'Public Function ConsultaControlClinico_() As String
    '    oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
    '    oRceRecetaMedicamentoE.IdRecetaDet = 0
    '    oRceRecetaMedicamentoE.Orden = 10 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
    '    Dim tabla_cabecera As New DataTable()
    '    tabla_cabecera = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
    '    Dim CadenaEstructuraCC As String = ""
    '    Dim FechaReceta As String = ""
    '    Dim NuevoGrupoFecha As String = ""

    '    CadenaEstructuraCC += "<ul>"
    '    If tabla_cabecera.Rows.Count > 0 Then
    '        FechaReceta = tabla_cabecera.Rows(0)("fec_registra").ToString().ToUpper().Trim()

    '        For index_cabecera = 0 To tabla_cabecera.Rows.Count - 1
    '            'INICIO - JB - NUEVAS LINEAS DE CODIGO - 22/04/2019    
    '            If index_cabecera = 0 Then
    '                CadenaEstructuraCC += "<li>" 'abriendo li cabecera
    '                CadenaEstructuraCC += "<a><img alt='' src='../Imagenes/Pastilla.png' />" + tabla_cabecera.Rows(index_cabecera)("nmedico").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim() + "</a><input type='hidden' value='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "'></input>"
    '                NuevoGrupoFecha = "S"
    '            Else
    '                If FechaReceta <> tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim() Then
    '                    FechaReceta = tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim()
    '                    CadenaEstructuraCC += "</ul>" 'cerrando ul de detalle
    '                    CadenaEstructuraCC += "</li>" 'cerrando li cabecera
    '                    CadenaEstructuraCC += "<li>" 'abriendo li cabecera
    '                    CadenaEstructuraCC += "<a><img alt='' src='../Imagenes/Pastilla.png' />" + tabla_cabecera.Rows(index_cabecera)("nmedico").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim() + "</a><input type='hidden' value='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "'></input>" '<span class='SELECCIONCC' style='margin-left:15px;font-weight:bold;cursor:pointer;text-decoration:underline;color:#134B8D;'>COPIAR</span><span class='SELECCIONCS' id='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "' style='margin-left:15px;font-weight:bold;cursor:pointer;text-decoration:underline;color:#134B8D;'>SUSPENDER</span>                    
    '                    NuevoGrupoFecha = "S"
    '                Else
    '                    NuevoGrupoFecha = "N"
    '                End If
    '            End If
    '            'FIN - JB - NUEVAS LINEAS DE CODIGO - 22/04/2019

    '            'INICIO - JB - COMENTADO - 22/04/2019 - cabecera
    '            'CadenaEstructuraCC += "<li>"
    '            'CadenaEstructuraCC += "<a><img alt='' src='../Imagenes/Pastilla.png' />" + tabla_cabecera.Rows(index_cabecera)("nmedico").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim() + "</a><input type='hidden' value='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "'></input>" '<span class='SELECCIONCC' style='margin-left:15px;font-weight:bold;cursor:pointer;text-decoration:underline;color:#134B8D;'>COPIAR</span><span class='SELECCIONCS' id='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "' style='margin-left:15px;font-weight:bold;cursor:pointer;text-decoration:underline;color:#134B8D;'>SUSPENDER</span>
    '            'FIN - JB - COMENTADO - 22/04/2019
    '            oRceRecetaMedicamentoE.IdRecetaDet = CType(tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString(), Integer)
    '            oRceRecetaMedicamentoE.Orden = 2
    '            Dim tabla_detalle As New DataTable()
    '            tabla_detalle = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
    '            If tabla_detalle.Rows.Count > 0 Then

    '                If NuevoGrupoFecha = "S" Then
    '                    CadenaEstructuraCC += "<ul>" 'abriendo ul de detalle
    '                End If
    '                'CadenaEstructuraCC += "<ul>" JB  - COMENTADO - 22/04/2019 - detalle
    '                For index_detalle = 0 To tabla_detalle.Rows.Count - 1
    '                    CadenaEstructuraCC += "<li>"
    '                    CadenaEstructuraCC += "<a><a>" + tabla_detalle.Rows(index_detalle)("nproducto").ToString().Trim() + "</a>" + _
    '                                "<a style='margin-left:20px;'></a>" + "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("dsc_via").ToString().Trim() + "</a>" + _
    '                                "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("num_frecuencia").ToString().Trim() + "</a>" + " <span class='JETIQUETA_2' style='font-size:1em'>horas</span>" + _
    '                                "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("num_cantidad").ToString().Trim() + "</a></a>" + _
    '                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("cod_producto").ToString().Trim() + "</a>" + _
    '                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("num_dosis").ToString().Trim() + "</a>" + _
    '                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("txt_detalle").ToString().Trim() + "</a>" + _
    '                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("num_duracion").ToString().Trim() + "</a>" + _
    '                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("ide_suspension").ToString().Trim() + "</a>"

    '                    If tabla_detalle.Rows(index_detalle)("flg_suspension").ToString().Trim() = "V" Then
    '                        CadenaEstructuraCC += "<span style='font-size:1em;color: red;font-weight: bold;font-size:1em'>(Verificado)</span>"
    '                    End If
    '                    If tabla_detalle.Rows(index_detalle)("flg_suspension").ToString().Trim() = "S" Then
    '                        CadenaEstructuraCC += "<input type='checkbox' class='CheckCC' id='CheckCC" + index_detalle.ToString() + "' style='margin-left:5px;' /><label for='CheckCC" + index_detalle.ToString() + "' style='margin-left:0px;'>suspendido</label>"
    '                    End If
    '                    CadenaEstructuraCC += "</li>"
    '                Next
    '                'CadenaEstructuraCC += "</ul>" JB - COMENTADO - 22/04/2019 - detalle
    '            End If

    '            'CadenaEstructuraCC += "</li>" JB - COMENTADO- 22/04/2019 - cabecera
    '        Next
    '    End If
    '    CadenaEstructuraCC += "</ul>" 'JB - NUEVA LINEA- 22/04/2019 - detalle
    '    CadenaEstructuraCC += "</li>" 'JB - NUEVA LINEA- 22/04/2019 - cabecera
    '    CadenaEstructuraCC += "</ul>"

    '    'REGRESO LA ESTRUCTURA DEL TREEVIEW
    '    Return CadenaEstructuraCC

    'End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaControlClinico() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaControlClinico_()
    End Function
    Public Function ConsultaControlClinico_() As String
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdRecetaDet = 0
        oRceRecetaMedicamentoE.Orden = 11 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
        Dim tabla_cabecera As New DataTable()
        tabla_cabecera = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        Dim CadenaEstructuraCC As String = ""

        Dim contFecRegistro As Integer = 0
        Dim contHorRegistro As Integer = 0

        CadenaEstructuraCC += "<ul class='JTreeView'>"
        If tabla_cabecera.Rows.Count > 0 Then

            For index = 0 To tabla_cabecera.Rows.Count - 1
                If tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() <> "" Then
                    If contFecRegistro > 0 Then
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                        CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1

                        CadenaEstructuraCC += "</li>" 'cerrando nodo
                        contHorRegistro = 0
                    End If
                    CadenaEstructuraCC += "<li>" 'abriendo nodo
                    CadenaEstructuraCC += "<input type='hidden' value='" + tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() + "' class='JFECTREE' />"
                    CadenaEstructuraCC += "<span class='nudo'><img alt = '' src='../Imagenes/Pastilla.png' style='width:35px;height:35px;' />"
                    CadenaEstructuraCC += "<span class='JTREE2-FECHA' >" + tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() + "</span>"
                    CadenaEstructuraCC += "</span>"
                    CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 1
                    contFecRegistro += 1
                End If
                If tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() <> "" Then
                    If contHorRegistro > 0 Then
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                        CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                        'CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                    End If
                    CadenaEstructuraCC += "<li>" 'abriendo sub nodo 2
                    CadenaEstructuraCC += "<span class='nudo'>" + tabla_cabecera.Rows(index)("NMEDICO").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla_cabecera.Rows(index)("ID_RECETA").ToString().Trim() + "' /><input type='hidden' value='" + tabla_cabecera.Rows(index)("TIPO").ToString().Trim() + "' />"
                    CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 3

                    contHorRegistro += 1
                End If
                If tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() = "" And tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() = "" Then
                    Dim CadenaSuspendido As String = ""
                    If tabla_cabecera.Rows(index)("FLG_SUSPENSION").ToString().Trim() = "S" Then
                        CadenaSuspendido = "<span class='JETIQUETA_TREE_SUSPENDIDO'>Suspendido</span>"
                    End If

                    If tabla_cabecera.Rows(index)("TIPO") = "F" Then
                        CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' /><span class='JETIQUETA_TREE0' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla_cabecera.Rows(index)("DSC_VIA").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla_cabecera.Rows(index)("NUM_FRECUENCIA").ToString().Trim() + " horas </span><span class='JETIQUETA_TREE2' >" + tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                    End If
                    If tabla_cabecera.Rows(index)("TIPO").ToString().Trim() = "N" Then
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "NUTRICION" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />NUTRICION: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "TERAPIA" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />TERAPIA FISICA Y REHABILITACION: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "CUIDADOS" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />CUIDADOS DE ENFERMERIA: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "OTROS" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />OTROS: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If


                        'If tabla_cabecera.Rows(index)("FLG_NOFARMACO").ToString().Trim() = "NUT" Then
                        '    If tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() <> "" Then
                        '        CadenaEstructuraCC += "<li class='JTree-Element'>NUTRICION: <span class='JETIQUETA_TREE1' ><input type='hidden' value='' />" + tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() + "</span></li>"
                        '    End If
                        'End If
                        'If tabla_cabecera.Rows(index)("FLG_NOFARMACO").ToString().Trim() = "TER" Then
                        '    If tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() <> "" Then
                        '        CadenaEstructuraCC += "<li class='JTree-Element'>TERAPIA FISICA Y REHABILITACION: <span class='JETIQUETA_TREE1' ><input type='hidden' value='' />" + tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() + "</span></li>"
                        '    End If
                        'End If
                        'If tabla_cabecera.Rows(index)("FLG_NOFARMACO").ToString().Trim() = "CUI" Then
                        '    If tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() <> "" Then
                        '        CadenaEstructuraCC += "<li class='JTree-Element'>CUIDADOS DE ENFERMERIA: <span class='JETIQUETA_TREE1' ><input type='hidden' value='' />" + tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() + "</pan></li>"
                        '    End If
                        'End If
                        'If tabla_cabecera.Rows(index)("FLG_NOFARMACO").ToString().Trim() = "OTR" Then
                        '    If tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() <> "" Then
                        '        CadenaEstructuraCC += "<li class='JTree-Element'>OTROS: <span class='JETIQUETA_TREE1' ><input type='hidden' value='' />" + tabla_cabecera.Rows(index)("DSC_OTROS").ToString().Trim() + "</span></li>"
                        '    End If
                        'End If
                    End If
                    If tabla_cabecera.Rows(index)("TIPO") = "I" Then
                        CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + " " + CadenaSuspendido + "</li>"
                    End If
                End If
                '<li Class='JTree-Element'>Black Tea</li>


                If index = (tabla_cabecera.Rows.Count - 1) Then 'si llego al ultimo registro...
                    If contHorRegistro > 0 Then
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                        CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                    End If
                    If contFecRegistro > 0 Then
                        CadenaEstructuraCC += "</li>" 'cerrando nodo
                    End If
                End If
            Next

            '<ul Class='JTreeView'>
            '   <li>
            '       <span Class='nudo'>
            '            <img alt = '' src='../Imagenes/Pastilla.png' />
            '            <span style = 'padding-top:5px;' > Beverages</span>
            '        </span>
            '       <ul Class='anidado'>
            '           <li>
            '               <span Class='nudo'>Cafe</span>
            '                <ul Class='anidado'>
            '                    <li Class='JTree-Element'>Black Tea</li>
            '                    <li Class='JTree-Element'>White Tea</li>                                                                                
            '                </ul>                                                                            
            '            </li>  
            '            <li>
            '                <span Class='nudo'>Tea</span>
            '                <ul Class='anidado'>
            '                    <li Class='JTree-Element'>Black Tea</li>
            '                    <li Class='JTree-Element'>White Tea</li>                                                                                
            '                </ul>                                                                            
            '            </li>  
            '        </ul>                                                                 
            '   </li>

            '</ul>

            '    <li>
            '    <span Class='nudo'>
            '            <img alt = '' src='../Imagenes/Pastilla.png' />
            '            <span style = 'padding-top:5px;' > OPCION 2</span>
            '        </span>
            '        <ul Class='anidado'>
            '            <li Class='JTree-Element'>Sub opcion 2</li>
            '            <li Class='JTree-Element'>Sub opcion 3</li>
            '            <li>
            '                    <span Class='nudo'>sub opcion 4</span>
            '                <ul Class='anidado'>
            '                    <li Class='JTree-Element'>Black Tea</li>
            '                    <li Class='JTree-Element'>White Tea</li>
            '                    <li> <span Class='nudo'>Green Tea</span>
            '                        <ul Class='anidado'>
            '                            <li Class='JTree-Element'>Sencha</li>
            '                            <li Class='JTree-Element'>Gyokuro</li>
            '                            <li Class='JTree-Element'>Matcha</li>
            '                            <li Class='JTree-Element'>Pi Lo Chun</li>
            '                        </ul>
            '                    </li>
            '                </ul>
            '            </li>  
            '        </ul>
            '    </li>
            '</ul>


            '<li>
            '   <span Class='nudo'>
            '       <img alt = '' src='../Imagenes/Pastilla.png' />
            '       <span style='padding-top:5px;'> Beverages</span>
            '   </span>
            '   <ul Class='anidado'>
            '       <li>
            '           <span Class='nudo'>Cafe</span>
            '           <ul Class='anidado'>
            '               <li Class='JTree-Element'>Black Tea</li>
            '               <li Class='JTree-Element'>White Tea</li>                                                                                
            '           </ul>                                                                            
            '       </li>  
            '       <li>
            '           <span Class='nudo'>Tea</span>
            '           <ul Class='anidado'>
            '               <li Class='JTree-Element'>Black Tea</li>
            '               <li Class='JTree-Element'>White Tea</li>                                                                                
            '           </ul>                                                                            
            '        </li>  
            '    </ul> 
            '</li>



        End If

        CadenaEstructuraCC += "</ul>"

        'REGRESO LA ESTRUCTURA DEL TREEVIEW
        Return CadenaEstructuraCC

    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaControlClinico2(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaControlClinico2_(orden, fec_receta, ide_recetacab)
    End Function
    Public Function ConsultaControlClinico2_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim oRceRecetaMedicamentoE_ As New RceRecetaMedicamentoE()
        Dim oRceRecetaMedicamentoLN_ As New RceRecetaMedicamentoLN()

        oRceRecetaMedicamentoE_.CodAtencion = Session(sCodigoAtencion)
        oRceRecetaMedicamentoE_.IdReceta = ide_recetacab
        oRceRecetaMedicamentoE_.FecReceta = fec_receta
        oRceRecetaMedicamentoE_.HorReceta = ""
        oRceRecetaMedicamentoE_.Orden = orden
        Dim ValorDevolver As String = ""
        Try
            Dim tabla As New DataTable()
            tabla = oRceRecetaMedicamentoLN_.Sp_RceRecetaMedicamentoCab_ConsultaV2(oRceRecetaMedicamentoE_)

            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JCCLI'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HOR_REGISTRO").ToString().Trim() + "</div><input type='hidden' class='IdeRecetaCab' value='" + tabla.Rows(index1)("IDE_RECETA").ToString().Trim() + "' /><input type='hidden' class='TipoRecetaCC' value='" + tabla.Rows(index1)("TIPO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                    'CadenaEstructuraCC += "<span class='nudo'>" + tabla_cabecera.Rows(index)("NMEDICO").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla_cabecera.Rows(index)("ID_RECETA").ToString().Trim() + "' /><input type='hidden' value='" + tabla_cabecera.Rows(index)("TIPO").ToString().Trim() + "' />"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaSuspendido As String = ""
                    If tabla.Rows(index1)("flg_suspension").ToString().Trim() = "S" Then
                        CadenaSuspendido = "<span class='JETIQUETA_TREE_SUSPENDIDO'> Suspendido</span>"
                    End If


                    'If tabla_cabecera.Rows(index)("TIPO") = "F" Then
                    '    CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' /><span class='JETIQUETA_TREE0' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla_cabecera.Rows(index)("DSC_VIA").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla_cabecera.Rows(index)("NUM_FRECUENCIA").ToString().Trim() + " horas </span><span class='JETIQUETA_TREE2' >" + tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                    'End If
                    If tabla.Rows(index1)("tipo") = "F" Then
                        'INICIO - JB - 10/02/2021 - LOS PEDIDOS URGENTE TENDRAN EL TEXTO 'STAT' EN ROJO AL COSTADO
                        Dim CadenaPedidoUrgente As String = ""
                        If tabla.Rows(index1)("tipopedido").ToString().Trim() = "03" Then
                            CadenaPedidoUrgente = "<span class='JETIQUETA_TREE_SUSPENDIDO'> STAT</span>"
                        End If
                        'FIN - JB - 10/02/2021 - LOS PEDIDOS URGENTE TENDRAN EL TEXTO 'STAT' EN ROJO AL COSTADO

                        'INICIO - JB - 24/05/2021 - NUEVO CODIGO PARA VISUALIZAR TEXTO PRN 
                        Dim sNumFrecuencia As String = ""
                        If tabla.Rows(index1)("num_frecuencia").ToString().Trim() = "" Then
                            sNumFrecuencia = "<span style='color:red;'>PRN<span>"
                        Else
                            sNumFrecuencia = tabla.Rows(index1)("num_frecuencia").ToString().Trim() + " horas"
                        End If
                        'FIN - JB - 24/05/2021 - NUEVO CODIGO PARA VISUALIZAR TEXTO PRN 

                        ValorDevolver += "<div class='JTREE3-DETALLE'>"
                        ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla.Rows(index1)("dsc_via").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + sNumFrecuencia + " </span>" + "<span class='JETIQUETA_TREE2' >" + tabla.Rows(index1)("txt_detalle").ToString().Trim() + "</span>" + CadenaSuspendido + CadenaPedidoUrgente + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                        ValorDevolver += "</div>"
                    End If
                    If tabla.Rows(index1)("tipo").ToString().Trim() = "N" Then
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "NUTRICION" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />NUTRICION: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "NUTRICION: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "TERAPIA" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />TERAPIA FISICA Y REHABILITACION: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "TERAPIA FISICA Y REHABILITACION: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "CUIDADOS" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />CUIDADOS DE ENFERMERIA: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "CUIDADOS DE ENFERMERIA: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "OTROS" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />OTROS: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "OTROS: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                    End If
                    If tabla.Rows(index1)("tipo") = "I" Then
                        'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + " " + CadenaSuspendido + "</li>"
                        ValorDevolver += "<div class='JTREE3-DETALLE'>"
                        ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                        ValorDevolver += "</div>"
                    End If

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try

    End Function




    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarMedicamento(ByVal IdMedicamentoSuspendido As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarMedicamento_(IdMedicamentoSuspendido)
    End Function

    Public Function VerificarMedicamento_(ByVal IdMedicamentoSuspendido As String) As String
        Try
            oRceRecetaMedicamentoE.IdRecetaDet = IdMedicamentoSuspendido
            oRceRecetaMedicamentoE.Campo = "flg_verifica"
            oRceRecetaMedicamentoE.ValorNuevo = "1"
            'oRceRecetaMedicamentoE.CodMedico = Session(sCodMedico)
            oRceRecetaMedicamentoLN.Sp_RceRecetaSuspension_Update(oRceRecetaMedicamentoE)

            oRceRecetaMedicamentoE.Campo = "usr_verifica"
            oRceRecetaMedicamentoE.ValorNuevo = Session(sCodMedico)
            oRceRecetaMedicamentoLN.Sp_RceRecetaSuspension_Update(oRceRecetaMedicamentoE)

            oRceRecetaMedicamentoE.Campo = "fec_verifica"
            oRceRecetaMedicamentoE.ValorNuevo = Format(CDate(Date.Now), "MM/dd/yyyy h:mm:ss") 'Date.Now.ToString()  'Format(CDate(Date.Now), "dd/MM/yyyy h:mm:ss")
            oRceRecetaMedicamentoLN.Sp_RceRecetaSuspension_Update(oRceRecetaMedicamentoE)

            Return "OK"

        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA VALIDAR SI EL PRODUCTO/MEDICAMENTO YA EXISTE AGREGADO EN EL LISTADO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarProductoAgregado(ByVal IdeReceta As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarProductoAgregado_(IdeReceta)
    End Function
    Public Function ValidarProductoAgregado_(ByVal IdeReceta As String) As String
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdRecetaDet = IdeReceta
        oRceRecetaMedicamentoE.Orden = 14 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
        Dim tabla_medicamentos As New DataTable()
        tabla_medicamentos = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        Dim TablaMedicamentosAgregados As New DataTable()

        If Not IsNothing(Session(sTablaProductoMedicamento)) Then
            TablaMedicamentosAgregados = CType(Session(sTablaProductoMedicamento), DataTable)
        Else
            TablaMedicamentosAgregados.Columns.Add("Codigo")
            TablaMedicamentosAgregados.Columns.Add("Producto")
            TablaMedicamentosAgregados.Columns.Add("Via")
            TablaMedicamentosAgregados.Columns.Add("CadaHora")
            TablaMedicamentosAgregados.Columns.Add("Dosis")
            TablaMedicamentosAgregados.Columns.Add("Indicacion")
            TablaMedicamentosAgregados.Columns.Add("Cantidad")
            TablaMedicamentosAgregados.Columns.Add("TipoPedido")
            TablaMedicamentosAgregados.Columns.Add("TipoPedido2")
            TablaMedicamentosAgregados.Columns.Add("Prn")
        End If

        'INICIO - JB - 13/08/2021
        Dim oRceProductoE As New RceProductoE()
        Dim oRceProductoLN As New RceProductoLN()
        Dim tablaLogistica As New DataTable()
        oRceProductoE.CodAtencion = Session(sCodigoAtencion)
        tablaLogistica = oRceProductoLN.Sp_CentroxHabitacion_Consulta(oRceProductoE)
        Dim CodAlmacen As String = tablaLogistica.Rows(0)("codalmacen").ToString()
        Dim ProdActivo As Boolean = True
        Dim MensajeMedicamentoInactivo As String = ""
        'FIN - JB - 13/08/2021

        Dim TablaMedicamentosAgregados2 As New DataTable()
        TablaMedicamentosAgregados2 = TablaMedicamentosAgregados

        Dim existe As Boolean = False

        If tabla_medicamentos.Rows.Count > 0 Then
            For index1 = 0 To tabla_medicamentos.Rows.Count - 1
                existe = False
                If TablaMedicamentosAgregados.Rows.Count > 0 Then


                    For index = 0 To TablaMedicamentosAgregados.Rows.Count - 1 'por cada producto agregado en el listado
                        'LOS MEDICAMENTOS DIGITADOS (COD PRODUCTO VACIO) NO HARAN NINGUNA VALIDACION - 27/08/2021
                        If TablaMedicamentosAgregados.Rows(index)("Codigo").ToString().Trim() = "" And tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim() = "" Then

                        Else
                            If TablaMedicamentosAgregados.Rows(index)("Codigo").ToString().Trim() = tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim() Then
                                existe = True
                            End If
                        End If
                    Next
                End If

                If existe = False Then
                    Dim CantidadN As Integer = 0
                    CantidadN = IIf(tabla_medicamentos.Rows(index1)("num_cantidad").ToString().Trim() = "", 0, tabla_medicamentos.Rows(index1)("num_cantidad").ToString().Trim())

                    'INICIO - JB - 13/08/2021
                    If tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim() <> "" Then
                        Dim tablaProductoAct As New DataTable()
                        oRceProductoE.Producto = tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim()
                        oRceProductoE.CodAlmacen = CodAlmacen
                        oRceProductoE.Orden = 1
                        tablaProductoAct = oRceProductoLN.Log_ConsultaProductosPedidosxAlmacen(oRceProductoE)

                        If tablaProductoAct.Rows.Count > 0 Then
                            ProdActivo = True
                        Else
                            ProdActivo = False
                        End If

                    End If
                    'FIN - JB - 13/08/2021


                    If tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim() = "" Or CantidadN = 0 Then

                        If tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim() <> "" And ProdActivo = False Then
                            'PRODUCTO INACTIVO
                            MensajeMedicamentoInactivo += tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim() + "<br>"
                        Else
                            TablaMedicamentosAgregados2.Rows.Add(tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim(), tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim(),
                            tabla_medicamentos.Rows(index1)("dsc_via").ToString().Trim(), tabla_medicamentos.Rows(index1)("num_frecuencia").ToString().Trim(),
                            tabla_medicamentos.Rows(index1)("num_dosis").ToString().Trim(), tabla_medicamentos.Rows(index1)("txt_detalle").ToString().Trim(), CantidadN, "", "")

                        End If
                    Else
                        If tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim() <> "" And ProdActivo = False Then
                            'PRODUCTO INACTIVO
                            MensajeMedicamentoInactivo += tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim() + "<br>"
                        Else
                            TablaMedicamentosAgregados2.Rows.Add(tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim(), tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim(),
                            tabla_medicamentos.Rows(index1)("dsc_via").ToString().Trim(), tabla_medicamentos.Rows(index1)("num_frecuencia").ToString().Trim(),
                            tabla_medicamentos.Rows(index1)("num_dosis").ToString().Trim(), tabla_medicamentos.Rows(index1)("txt_detalle").ToString().Trim(), CantidadN, "05", "05")
                            ''Agregado Paul Bardales
                            'TablaMedicamentosAgregados2.Rows.Add(tabla_medicamentos.Rows(index1)("cod_producto").ToString().Trim(), tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim(),
                            'tabla_medicamentos.Rows(index1)("dsc_via").ToString().Trim(), tabla_medicamentos.Rows(index1)("num_frecuencia").ToString().Trim(),
                            'tabla_medicamentos.Rows(index1)("num_dosis").ToString().Trim(), tabla_medicamentos.Rows(index1)("txt_detalle").ToString().Trim(), CantidadN,
                            'IIf(tabla_medicamentos.Rows(index1)("tipopedido").ToString().Trim() = "", "", "05"), IIf(tabla_medicamentos.Rows(index1)("tipopedido").ToString().Trim() = "", "", "05"))
                            ''(cambios pbardales)
                        End If

                    End If

                End If

            Next
        End If

        Session(sTablaProductoMedicamento) = TablaMedicamentosAgregados2

        If MensajeMedicamentoInactivo <> "" Then
            Return "Los siguientes medicamentos no se pudieron copiar por que ya no estan disponibles: <br><br> " + MensajeMedicamentoInactivo
        End If

        Return "OK"
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarProductoAgregado2(ByVal CodigoProducto As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarProductoAgregado2_(CodigoProducto)
    End Function
    Public Function ValidarProductoAgregado2_(ByVal CodigoProducto As String) As String
        Dim tabla As New DataTable()
        Dim existe As String = ""
        If Not IsNothing(Session(sTablaProductoMedicamento)) Then
            tabla = CType(Session(sTablaProductoMedicamento), DataTable)
            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("Codigo").ToString().Trim() = CodigoProducto Then
                        existe = "SI"
                    End If
                Next
            End If
        End If
        Return existe
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarNoFarmacologico(ByVal IdeReceta As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarNoFarmacologico_(IdeReceta)
    End Function

    Public Function ValidarNoFarmacologico_(ByVal IdeReceta As String) As String
        Try
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            oRceRecetaMedicamentoE.IdRecetaDet = IdeReceta
            oRceRecetaMedicamentoE.Orden = 15 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
            Dim tabla_medicamentos As New DataTable()
            tabla_medicamentos = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)

            'Dim Lista = tabla_medicamentos.AsEnumerable().[Select](Function(x) New With {
            '    .ID_RECETA = x.Field(Of Integer)("ID_RECETA"),
            '    .IDE_MEDICAMENTOREC = x.Field(Of Integer)("IDE_MEDICAMENTOREC"),
            '    .NPRODUCTO = x.Field(Of String)("NPRODUCTO"),
            '    .TIPO = x.Field(Of String)("TIPO"),
            '    .TXT_DETALLE = x.Field(Of String)("TXT_DETALLE")
            '}).Where(Function(x) x.ID_RECETA = CType(IdeReceta, Integer) And x.TIPO = "N" And x.IDE_MEDICAMENTOREC <> 0).ToList()

            Dim Valores As String = ""
            If tabla_medicamentos.Rows.Count > 0 Then
                For index = 0 To tabla_medicamentos.Rows.Count - 1
                    Valores += tabla_medicamentos.Rows(index)("txt_detalle").ToString().Trim().ToUpper() + ";" + tabla_medicamentos.Rows(index)("nproducto").ToString().Trim().ToUpper() + "*"
                Next
            End If
            Return Valores
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarInfusionAgregado(ByVal IdeReceta As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarInfusionAgregado_(IdeReceta)
    End Function

    Public Function ValidarInfusionAgregado_(ByVal IdeReceta As String) As String
        Try
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            oRceRecetaMedicamentoE.IdRecetaDet = IdeReceta
            oRceRecetaMedicamentoE.Orden = 16 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
            Dim tabla_medicamentos As New DataTable()
            tabla_medicamentos = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)


            Dim TablaInfusionAgregados As New DataTable()
            If Not IsNothing(Session(sTablaInfusiones)) Then
                TablaInfusionAgregados = CType(Session(sTablaInfusiones), DataTable)
            Else
                TablaInfusionAgregados.Columns.Add("Item")
                TablaInfusionAgregados.Columns.Add("Infusion")
            End If

            Dim TablaInfusionAgregados2 As New DataTable()
            TablaInfusionAgregados2 = TablaInfusionAgregados

            Dim Existe As Boolean = False
            Dim Valores As String = ""

            If tabla_medicamentos.Rows.Count > 0 Then
                For index1 = 0 To tabla_medicamentos.Rows.Count - 1
                    Existe = False
                    If TablaInfusionAgregados.Rows.Count > 0 Then
                        For index = 0 To TablaInfusionAgregados.Rows.Count - 1 '
                            If TablaInfusionAgregados.Rows(index)("Infusion").ToString().Trim() = tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim() Then
                                Existe = True
                            End If
                        Next
                    End If

                    If Existe = False Then
                        TablaInfusionAgregados2.Rows.Add((TablaInfusionAgregados.Rows.Count + 1), tabla_medicamentos.Rows(index1)("nproducto").ToString().Trim().ToUpper())
                    End If
                Next
            End If


            'Dim Lista = tabla_medicamentos.AsEnumerable().[Select](Function(x) New With {
            '    .ID_RECETA = x.Field(Of Integer)("ID_RECETA"),
            '    .IDE_MEDICAMENTOREC = x.Field(Of Integer)("IDE_MEDICAMENTOREC"),
            '    .NPRODUCTO = x.Field(Of String)("NPRODUCTO"),
            '    .TIPO = x.Field(Of String)("TIPO")
            '}).Where(Function(x) x.TIPO = "I" And x.ID_RECETA = CType(IdeReceta, Integer) And x.IDE_MEDICAMENTOREC <> 0 And x.NPRODUCTO.Trim() <> "").ToList()

            'Dim Valores As String = ""

            'If Lista.Count > 0 Then
            '    If TablaInfusionAgregados.Rows.Count > 0 Then 'si ya hay ya agregada en el listado validara que no este duplicado
            '        For index1 = 0 To Lista.Count - 1
            '            Existe = False
            '            For index = 0 To TablaInfusionAgregados.Rows.Count - 1 '
            '                If TablaInfusionAgregados.Rows(index)("Infusion").ToString().Trim() = Lista.Item(index1).NPRODUCTO.ToString().Trim() Then
            '                    Existe = True
            '                End If
            '            Next
            '            If Existe = False Then
            '                TablaInfusionAgregados2.Rows.Add((TablaInfusionAgregados.Rows.Count + 1), Lista.Item(index1).NPRODUCTO.ToString().Trim().ToUpper())
            '            End If
            '        Next
            '    Else
            '        For index1 = 0 To Lista.Count - 1 'por cada producto que desea agregar...
            '            TablaInfusionAgregados2.Rows.Add((index1 + 1), Lista.Item(index1).NPRODUCTO.ToString().Trim())
            '        Next
            '    End If
            'End If
            'AgregarInfusion
            Session(sTablaInfusiones) = TablaInfusionAgregados2
            For index = 0 To TablaInfusionAgregados2.Rows.Count - 1
                If TablaInfusionAgregados2.Rows(index)("Infusion").ToString().Trim() <> "" Then
                    Valores += TablaInfusionAgregados2.Rows(index)("Infusion").ToString().Trim().ToUpper() + ";"
                End If
            Next

            Return Valores

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA VALIDAR SI EL PACIENTE ES ALERGICO AL PRODUCTO SELECCIONADO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAlergiaPaciente(ByVal CodigoProducto As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaAlergiaPaciente_(CodigoProducto)
    End Function

    Public Function ValidaAlergiaPaciente_(ByVal CodigoProducto As String) As String
        Try
            Dim tabla As New DataTable()
            Dim Alergico As String = ""
            oRceAlergiaE.IdHistoria = Session(sIdeHistoria)
            oRceAlergiaE.CodProducto = CodigoProducto
            tabla = oRceAlergiaLN.Sp_RceAlergia_Validar(oRceAlergiaE)

            If tabla.Rows.Count > 0 Then
                If tabla.Rows(0)(0).ToString().Trim() = "1" Then
                    Alergico = "SI"
                End If
            End If

            Return Alergico
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try
    End Function



    ''' <summary>
    ''' FUNCION PARA VALIDAR SI EL DIAGNOSTICO YA SE ENCUENTRA AGREGADO
    ''' </summary>
    ''' <param name="CodDiagnostico"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarDiagnosticoAgregado(ByVal CodDiagnostico As String, ByVal Tipo As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarDiagnosticoAgregado_(CodDiagnostico, Tipo)
    End Function
    Public Function ValidarDiagnosticoAgregado_(ByVal CodDiagnostico As String, ByVal Tipo As String) As String
        oRceDiagnosticoE.Tipo = sTipoD
        oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
        Dim existe As String = ""
        Dim tabla As New DataTable()
        tabla = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)
        If tabla.Rows.Count > 0 Then
            For index = 0 To tabla.Rows.Count - 1
                If tabla.Rows(index)("coddiagnostico").ToString().Trim() = CodDiagnostico And tabla.Rows(index)("tipo").ToString().Trim() = Tipo Then
                    existe = "SI"
                End If
            Next
        End If
        Return existe
    End Function


    ''' <summary>
    ''' FUNCION PARA ELIMINAR UN DIAGNOSTICO DE FAVORITOS
    ''' </summary>
    ''' <param name="IdeFavoritoDiagnostico"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarDiagnosticoFavorito(ByVal IdeFavoritoDiagnostico As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EliminarDiagnosticoFavorito_(IdeFavoritoDiagnostico)
    End Function
    Public Function EliminarDiagnosticoFavorito_(ByVal IdeFavoritoDiagnostico As String) As String
        Try
            oRceDiagnosticoE.IdeDiagnosticoFavorito = CType(IdeFavoritoDiagnostico, Integer)
            Dim bEliminado As Boolean = oRceDiagnosticoLN.Sp_RceDiagnosticoFavoritoMae_Delete(oRceDiagnosticoE)
            If bEliminado = True Then
                Return "OK"
            Else
                Return ConfigurationManager.AppSettings(sMensajeEliminarError).ToString().Trim()
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA AGREGAR DIAGNOSTICO
    ''' </summary>
    ''' <param name="TipoDiagnostico"></param>
    ''' <param name="CodDiagnostico"></param>
    ''' <param name="Favorito"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function AgregarDiagnostico(ByVal TipoDiagnostico As String, ByVal CodDiagnostico As String, ByVal Favorito As String, ByVal Tipo As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.AgregarDiagnostico_(TipoDiagnostico, CodDiagnostico, Favorito, Tipo)
    End Function
    Public Function AgregarDiagnostico_(ByVal TipoDiagnostico As String, ByVal CodDiagnostico As String, ByVal Favorito As String, ByVal Tipo As String) As String
        oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
        oRceDiagnosticoE.TipoDeAtencion = Session(sTipoAtencion)
        oRceDiagnosticoE.Tipo = Tipo 'sTipoD 08/09/2016
        oRceDiagnosticoE.CodDiagnostico = CodDiagnostico
        Try
            Dim bInserto As Boolean = oRceDiagnosticoLN.Sp_Diagxhospital_Insert(oRceDiagnosticoE)
            If bInserto Then
                oRceDiagnosticoE.NuevoValor = TipoDiagnostico
                oRceDiagnosticoE.Campo = "tipodiagnostico"
                Dim bActualizo As Boolean = oRceDiagnosticoLN.Sp_Diagxhospital_Update(oRceDiagnosticoE)

                oRceDiagnosticoE.NuevoValor = Session(sCodUser)
                oRceDiagnosticoE.Campo = "usr_registra"
                Dim bActualizo2 As Boolean = oRceDiagnosticoLN.Sp_Diagxhospital_Update(oRceDiagnosticoE)

                If Favorito = "SI" Then
                    oRceDiagnosticoE.CodMedico = Session(sCodMedico)
                    oRceDiagnosticoE.UsrRegistra = Session(sCodUser)
                    oRceDiagnosticoLN.Sp_RceDiagnosticoFavoritoMae_Insert(oRceDiagnosticoE)
                End If
            End If


            If TipoDiagnostico = "D" Then
                GuardarLogEvolucionClinicaV2(CodDiagnostico, 1)
            End If

            GuardarLog_("DIAGNOSTICO", "Se agrego diagnóstico con codigo " + CodDiagnostico)

            ''GUARDANDO LOG
            ''11/11/2016
            'oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
            'oRceInicioSesionE.CodUser = Session(sCodUser)
            'oRceInicioSesionE.Formulario = "InformacionPaciente"
            'oRceInicioSesionE.Control = "DIAGNOSTICO"
            'oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
            'oRceInicioSesionE.DscPcName = Session(sDscPcName)
            'oRceInicioSesionE.DscLog = "Se agrego diagnóstico Nro " + CodDiagnostico
            'oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
            ''FIN 11/11/2016

            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function


    ''' <summary>
    ''' FUNCION PARA AGREGAR ANALISIS (DEBE GUARDAR EN CABECERA Y DETALLA)
    ''' </summary>
    ''' <param name="DescripcionReceta"></param>
    ''' <param name="IdAnalisis"></param>
    ''' <param name="FlgCubierto"></param>
    ''' <param name="flgFavoritoLaboratorio"></param>
    ''' <param name="Perfil"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function AgregarAnalisis(ByVal DescripcionReceta As String, ByVal IdAnalisis As String, ByVal FlgCubierto As Boolean, ByVal flgFavoritoLaboratorio As Boolean, ByVal Perfil As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.AgregarAnalisis_Cab(DescripcionReceta, IdAnalisis, FlgCubierto, flgFavoritoLaboratorio, Perfil)
    End Function

    Public Function AgregarAnalisis_Cab(ByVal DescripcionReceta As String, ByVal IdAnalisis As String, ByVal FlgCubierto As Boolean, ByVal flgFavoritoLaboratorio As Boolean, ByVal Perfil As String) As String
        Try
            If ValidaSession_() <> "" Then
                Return "EXPIRO" + "*" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
            End If

            If flgFavoritoLaboratorio = True Then
                Dim mensaje As String = AgregarAnalisisFavorito(IdAnalisis)
                If mensaje <> "OK" Then
                    Return mensaje
                End If
            End If

            'JB - NUEVO - 28/08/2020
            Dim tabla As New DataTable()
            Dim AnalisisExiste As String = ""

            oRceLaboratioE.IdAnalisisLaboratorio = IdAnalisis
            oRceLaboratioE.TipoDeAtencion = "A"
            oRceLaboratioE.Orden = 0
            Dim Tabla2 As New DataTable()
            Tabla2 = oRceLaboratorioLN.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE)
            If Not IsNothing(Session(sTablaAnalisisLaboratorio)) Then
                tabla = Session(sTablaAnalisisLaboratorio)
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("ide_analisis").ToString() = IdAnalisis.Trim() Then
                        AnalisisExiste = "El analisis ya se encuentra agregado."
                        Exit For
                    End If
                Next

                If AnalisisExiste <> "" Then
                    Return "ERROR;" + AnalisisExiste
                End If


                tabla.Rows.Add(IdAnalisis, Tabla2.Rows(0)("dsc_analisis").ToString(), IdAnalisis)
                Session(sTablaAnalisisLaboratorio) = tabla
                GuardarLog_("LABORATORIO", "Se agrega analisis con codigo " + IdAnalisis.ToString())
            Else
                tabla.Columns.Add("ide_recetadet")
                tabla.Columns.Add("dsc_analisis")
                tabla.Columns.Add("ide_analisis")
                tabla.Rows.Add(IdAnalisis, Tabla2.Rows(0)("dsc_analisis").ToString(), IdAnalisis)
                Session(sTablaAnalisisLaboratorio) = tabla
                GuardarLog_("LABORATORIO", "Se agrega analisis con codigo " + IdAnalisis.ToString())
            End If


            ''GUARDANDO CABECERA
            'If IsNothing(Session(sIdRecetaCab)) Then
            '    oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
            '    oRceLaboratioE.CodMedico = Session(sCodMedico)
            '    oRceLaboratioE.UsrRegistra = Session(sCodUser)
            '    oRceLaboratioE.DscReceta = DescripcionReceta.Trim().ToUpper()
            '    oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
            '    oRceLaboratioE.EstAnalisis = "A" 'jb - 13/07/2020 - se envia estado
            '    If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE) Then
            '        Session(sIdRecetaCab) = oRceLaboratioE.IdeRecetaCab
            '        'GUARDANDO DETALLE
            '        Return AgregarAnalisis_Det(IdAnalisis, FlgCubierto, Perfil)
            '    Else
            '        Return ConfigurationManager.AppSettings(sMensajeGuardarError).Trim().ToString()
            '    End If
            'Else
            '    oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
            '    oRceLaboratioE.ValorNuevo = DescripcionReceta.Trim().ToUpper()
            '    oRceLaboratioE.Campo = "dsc_receta"
            '    If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_Update(oRceLaboratioE) Then
            '        'GUARDANDO DETALLE
            '        Return AgregarAnalisis_Det(IdAnalisis, FlgCubierto, Perfil)
            '    Else
            '        Return ConfigurationManager.AppSettings(sMensajeActualizarError).Trim().ToString()
            '    End If
            'End If
            Return "OK"
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try

    End Function

    Public Function AgregarAnalisis_Det(ByVal IdAnalisis As String, ByVal FlgCubierto As Boolean, ByVal Perfil As String) As String
        Try
            If Not IsNothing(Session(sIdRecetaCab)) Then
                If Perfil.Trim() <> "" And Perfil.Trim() <> "0" Then
                    oRceLaboratioE.IdAnalisisLaboratorio = IdAnalisis
                    oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
                    oRceLaboratioE.Orden = 5
                    Dim tabla As New DataTable()
                    Dim tabla_Analisis As New DataTable()
                    Dim existe As Boolean = False
                    Dim existe_2 As Boolean = False
                    tabla = oRceLaboratorioLN.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE)
                    Dim Mensaje As String = "2;No se pudo guardar el/los analisis por que ya se encuentra(n) agregado(s) "

                    If tabla.Rows.Count > 0 Then
                        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
                        oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
                        oRceLaboratioE.Orden = 2
                        tabla_Analisis = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)

                        For index = 0 To tabla.Rows.Count - 1
                            For index1 = 0 To tabla_Analisis.Rows.Count - 1
                                If tabla.Rows(index)(0).ToString() = tabla_Analisis.Rows(index1)("ide_analisis").ToString() Then
                                    Mensaje += tabla_Analisis.Rows(index1)("dsc_analisis").ToString() + ", "
                                    existe_2 = True
                                    existe = True
                                End If
                            Next
                            If existe = False Then
                                oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
                                oRceLaboratioE.EstAnalisis = "A" 'JB - 13/07/2020 - SE CAMBIA DE ESTADO G -> A
                                oRceLaboratioE.IdAnalisisLaboratorio = CType(tabla.Rows(index)(0).ToString(), Integer)
                                oRceLaboratioE.UsrRegistra = Session(sCodUser)
                                oRceLaboratioE.FlgCubierto = FlgCubierto
                                oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE)
                            Else
                                existe = False
                            End If
                        Next
                    End If

                    If existe_2 = True Then
                        Mensaje = Mensaje.Trim()
                        Return Mensaje.Remove(Mensaje.Length - 1)
                    Else
                        Return "OK"
                    End If
                Else
                    oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
                    oRceLaboratioE.EstAnalisis = "A" 'JB - 13/07/2020 - SE CAMBIA DE ESTADO G -> A
                    oRceLaboratioE.IdAnalisisLaboratorio = IdAnalisis
                    oRceLaboratioE.UsrRegistra = Session(sCodUser)
                    oRceLaboratioE.FlgCubierto = FlgCubierto
                    If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE) Then
                        Return "OK"
                    Else
                        Return "1;" + ConfigurationManager.AppSettings(sMensajeGuardarError).Trim().ToString()
                    End If
                End If
            Else
                Return ""
            End If
        Catch ex As Exception
            Return "1;" + ex.Message.ToString()
        End Try
    End Function




    Public Function AgregarAnalisisFavorito(ByVal IdAnalisis As String) As String
        oRceLaboratioE.IdAnalisisLaboratorio = IdAnalisis
        oRceLaboratioE.CodMedico = Session(sCodMedico)
        oRceLaboratioE.UsrRegistra = Session(sCodUser)
        oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)

        Try
            If oRceLaboratorioLN.Sp_RceAnalisisFavoritoMae_Insert(oRceLaboratioE) Then
                Return "OK"
            Else
                Return ConfigurationManager.AppSettings(sMensajeGuardarError).Trim().ToString()
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function


    ''' <summary>
    ''' FUNCION PARA ELIMINAR UN ANALISIS DE FAVORITO
    ''' </summary>
    ''' <param name="IdeFavoritoAnalisisLab"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarFavoritoAnalisis(ByVal IdeFavoritoAnalisisLab As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EliminarFavoritoAnalisis_(IdeFavoritoAnalisisLab)
    End Function

    Public Function EliminarFavoritoAnalisis_(ByVal IdeFavoritoAnalisisLab As String) As String
        oRceLaboratioE.IdAnalisisFavorito = IdeFavoritoAnalisisLab
        Try
            If oRceLaboratorioLN.Sp_RceAnalisisFavoritoMae_Delete(oRceLaboratioE) Then
                Return "OK"
            Else
                Return ConfigurationManager.AppSettings(sMensajeEliminarError).Trim().ToString()
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA ENVIAR SOLICITUD DE ANALISIS
    ''' </summary>
    ''' <param name="Descripcion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarSolicitudAnalisis(ByVal Descripcion As String, ByVal Fecha As String, ByVal Hora As String, ByVal CodigoMarcado As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarSolicitudAnalisis_(Descripcion, Fecha, Hora, CodigoMarcado)
    End Function
    Public Function GuardarSolicitudAnalisis_(ByVal Descripcion As String, ByVal Fecha As String, ByVal Hora As String, ByVal CodigoMarcado As String) As String
        Try
            Dim tabla As New DataTable()
            tabla = CType(Session(sTablaAnalisisLaboratorio), DataTable)
            Dim IdeRecetaCabA As Integer = 0
            Dim IdeRecetaCabG As Integer = 0

            For index = 0 To tabla.Rows.Count - 1
                Dim EstadoAnalisis As String = "A"
                Dim oRceLaboratioE_ As New RceLaboratioE
                Dim oRceLaboratorioLN_ As New RceLaboratorioLN
                oRceLaboratioE_.Orden = 10
                oRceLaboratioE_.Nombre = tabla.Rows(index)("ide_analisis").ToString()
                oRceLaboratioE_.CodMedico = Session(sCodMedico)
                oRceLaboratioE_.TipoDeAtencion = "A"
                Dim tablax1 As New DataTable()
                tablax1 = oRceLaboratorioLN.Sp_RceBuscar_Consulta(oRceLaboratioE_)
                If tablax1.Rows.Count > 0 Then
                    EstadoAnalisis = "G"
                End If

                If EstadoAnalisis = "A" Then
                    If IdeRecetaCabA = 0 Then 'SI NO SE HA GRABADO AUN INSERTARA NUEVA CABECERA
                        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
                        oRceLaboratioE.CodMedico = Session(sCodMedico)
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.DscReceta = Descripcion.Trim().ToUpper()
                        oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis
                        If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE) Then
                            IdeRecetaCabA = oRceLaboratioE.IdeRecetaCab
                            oRceLaboratioE.IdeRecetaCab = IdeRecetaCabA
                            oRceLaboratioE.EstAnalisis = EstadoAnalisis
                            oRceLaboratioE.IdAnalisisLaboratorio = tabla.Rows(index)("ide_analisis").ToString()
                            oRceLaboratioE.UsrRegistra = Session(sCodUser)
                            oRceLaboratioE.FlgCubierto = True
                            If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE) Then
                            End If
                        Else

                        End If
                    Else 'SI YA SE GRABO CABECERA SOLO REGISTRARA EL DETALLE
                        oRceLaboratioE.IdeRecetaCab = IdeRecetaCabA
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis
                        oRceLaboratioE.IdAnalisisLaboratorio = tabla.Rows(index)("ide_analisis").ToString()
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.FlgCubierto = True
                        If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE) Then
                        End If
                    End If

                    If Fecha <> "" Then 'And Hora <> "" 
                        For index1 = 0 To CodigoMarcado.Split(";").Length - 1
                            If tabla.Rows(index)("ide_analisis").ToString().Trim() = CodigoMarcado.Split(";")(index1).ToString().Trim() Then
                                oRceLaboratioE.Campo = "fec_programado"
                                oRceLaboratioE.ValorNuevo = Format(Convert.ToDateTime((Fecha.Trim() + " " + Hora.Trim())), "MM/dd/yyyy HH:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
                                oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
                            End If
                        Next
                    End If
                Else 'ESTADO G
                    If IdeRecetaCabG = 0 Then 'SI NO SE HA GRABADO AUN INSERTARA NUEVA CABECERA
                        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
                        oRceLaboratioE.CodMedico = Session(sCodMedico)
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.DscReceta = Descripcion.Trim().ToUpper()
                        oRceLaboratioE.TipoDeAtencion = Session(sTipoAtencion)
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis
                        If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_InsertV2(oRceLaboratioE) Then
                            IdeRecetaCabG = oRceLaboratioE.IdeRecetaCab
                            oRceLaboratioE.IdeRecetaCab = IdeRecetaCabG
                            oRceLaboratioE.EstAnalisis = EstadoAnalisis
                            oRceLaboratioE.IdAnalisisLaboratorio = tabla.Rows(index)("ide_analisis").ToString()
                            oRceLaboratioE.UsrRegistra = Session(sCodUser)
                            oRceLaboratioE.FlgCubierto = True
                            If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE) Then
                            End If
                        Else

                        End If
                    Else 'SI YA SE GRABO CABECERA SOLO REGISTRARA EL DETALLE
                        oRceLaboratioE.IdeRecetaCab = IdeRecetaCabG
                        oRceLaboratioE.EstAnalisis = EstadoAnalisis
                        oRceLaboratioE.IdAnalisisLaboratorio = tabla.Rows(index)("ide_analisis").ToString()
                        oRceLaboratioE.UsrRegistra = Session(sCodUser)
                        oRceLaboratioE.FlgCubierto = True
                        If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Insert(oRceLaboratioE) Then
                        End If
                    End If

                    If Fecha <> "" Then 'And Hora <> "" 
                        For index1 = 0 To CodigoMarcado.Split(";").Length - 1
                            If tabla.Rows(index)("ide_analisis").ToString().Trim() = CodigoMarcado.Split(";")(index1).ToString().Trim() Then
                                oRceLaboratioE.Campo = "fec_programado"
                                oRceLaboratioE.ValorNuevo = Format(Convert.ToDateTime((Fecha.Trim() + " " + Hora.Trim())), "MM/dd/yyyy HH:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
                                oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
                            End If
                        Next
                    End If
                End If
            Next




            'GUARDANDO LOG
            If IdeRecetaCabA <> 0 Then
                GuardarLogLab(IdeRecetaCabA)
                GuardarLog_("LABORATORIO", "Se guardo analisis de laboratorio nro " + IdeRecetaCabA.ToString())
            End If
            If IdeRecetaCabG <> 0 Then
                GuardarLogLab(IdeRecetaCabG)
                GuardarLog_("LABORATORIO", "Se guardo analisis de laboratorio nro " + IdeRecetaCabG.ToString())
            End If

            'LIMPIANDO DATOS DE LA TABLA
            Dim tabla2 As New DataTable()
            tabla2.Columns.Add("ide_recetadet")
            tabla2.Columns.Add("dsc_analisis")
            tabla2.Columns.Add("ide_analisis")
            Session(sTablaAnalisisLaboratorio) = tabla2



            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try



        'Dim Mensaje As String = "No se pudo registrar el Analisis "
        'Dim fallo As Boolean = False
        'Try
        '    oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
        '    oRceLaboratioE.ValorNuevo = "A" 'ESTADO ANALISIS   JB - 13/07/2020  - se cambia de estado G -> A
        '    oRceLaboratioE.Campo = "est_analisis"
        '    If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_Update(oRceLaboratioE) Then
        '        oRceLaboratioE.ValorNuevo = Descripcion.Trim().ToUpper()
        '        oRceLaboratioE.Campo = "dsc_receta"
        '        If oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_Update(oRceLaboratioE) Then
        '            Dim tabla As New DataTable()
        '            tabla = CType(Session(sTablaAnalisisLaboratorio), DataTable)
        '            For index = 0 To tabla.Rows.Count - 1
        '                oRceLaboratioE.IdeRecetaDet = CType(tabla.Rows(index)("ide_recetadet").ToString(), Integer)

        '                'INICIO - JB - NUEVO CODIGO - 27/08/2020
        '                Dim EstadoAnalisis As String = "A"
        '                Dim oRceLaboratioE_ As New RceLaboratioE
        '                Dim oRceLaboratorioLN_ As New RceLaboratorioLN
        '                oRceLaboratioE_.Orden = 10
        '                oRceLaboratioE_.Nombre = tabla.Rows(index)("ide_analisis").ToString()
        '                oRceLaboratioE_.CodMedico = Session(sCodMedico)
        '                oRceLaboratioE_.TipoDeAtencion = "A"
        '                Dim tablax1 As New DataTable()
        '                tablax1 = oRceLaboratorioLN.Sp_RceBuscar_Consulta(oRceLaboratioE_)
        '                If tablax1.Rows.Count > 0 Then
        '                    EstadoAnalisis = "G"
        '                End If
        '                'FIN - JB - NUEVO CODIGO - 27/08/2020

        '                oRceLaboratioE.ValorNuevo = EstadoAnalisis 'JB - 27/08/2020 - estado es ahora una variable. 'ESTADO ANALISIS   JB - 13/07/2020  - se cambia de estado G -> A
        '                oRceLaboratioE.Campo = "est_analisis"
        '                If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE) Then
        '                Else
        '                    fallo = True
        '                    Mensaje += tabla.Rows(index)("ide_recetadet").ToString() + ", "
        '                End If

        '                'INICIO - 19/01/2017
        '                'oRceLaboratioE.Campo = "fec_programado"
        '                'oRceLaboratioE.ValorNuevo = Format(DateTime.Now, "MM/dd/yyyy h:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
        '                'oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
        '                If Fecha <> "" Then 'And Hora <> "" 
        '                    For index1 = 0 To CodigoMarcado.Split(";").Length - 1
        '                        If tabla.Rows(index)("ide_analisis").ToString().Trim() = CodigoMarcado.Split(";")(index1).ToString().Trim() Then
        '                            oRceLaboratioE.Campo = "fec_programado"
        '                            oRceLaboratioE.ValorNuevo = Format(Convert.ToDateTime((Fecha.Trim() + " " + Hora.Trim())), "MM/dd/yyyy HH:mm:ss") 'DateTime.ParseExact(Fecha.Trim() + " " + Hora.Trim(), "MM/dd/yyyy HH:mm", New CultureInfo("es-PE"))
        '                            oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
        '                        End If
        '                    Next
        '                End If
        '                'FIN - 19/01/2017
        '            Next
        '            If fallo Then
        '                Mensaje = Mensaje.Trim()
        '                Return Mensaje.Remove(Mensaje.Length - 1)
        '            Else
        '                oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
        '                oRceEvolucionE.IdeOrdenCab = Session(sIdRecetaCab)
        '                oRceEvolucionE.Orden = 2
        '                oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

        '                If oRceEvolucionE.CodigoEvolucion <> 0 Then

        '                    'INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
        '                    Dim pdf_byte As Byte() = ExportaPDF("DA")
        '                    Dim cn As New SqlConnection(CnnBD)
        '                    'Paso 1
        '                    oHospitalE.TipoDoc = 10
        '                    oHospitalE.CodAtencion = Session(sCodigoAtencion)
        '                    oHospitalE.CodUser = Session(sCodUser)
        '                    oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
        '                    oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

        '                    'Paso 2
        '                    Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
        '                    cmd1.CommandType = CommandType.Text
        '                    cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
        '                    cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
        '                    cmd1.Parameters.AddWithValue("@flg_reqfirma", "0")

        '                    Dim num1 As Integer
        '                    cn.Open()
        '                    num1 = cmd1.ExecuteNonQuery()
        '                    cn.Close()

        '                    'Paso 3
        '                    oHospitalE.IdeHistoria = Session(sIdeHistoria)
        '                    oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
        '                    oHospitalE.TipoDoc = 10
        '                    oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
        '                    'FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA


        '                Else
        '                    Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
        '                End If

        '                Session.Remove(sIdRecetaCab)
        '                Return "OK"
        '            End If
        '        Else
        '            Return ConfigurationManager.AppSettings(sMensajeActualizarError) + " - Sp_RceRecetaAnalisisCab_Update"
        '        End If
        '    Else
        '        Return ConfigurationManager.AppSettings(sMensajeActualizarError) + " - Sp_RceRecetaAnalisisCab_Update"
        '    End If

        'Catch ex As Exception
        '    Return ex.Message.ToString()
        'End Try

    End Function


    Function GuardarLogLab(ByVal IdeRecetaP As Integer) As String
        oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
        oRceEvolucionE.IdeOrdenCab = IdeRecetaP
        oRceEvolucionE.Orden = 2
        oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)
        If oRceEvolucionE.CodigoEvolucion <> 0 Then
            'INI 1.4
            ''INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'Dim pdf_byte As Byte() = ExportaPDF("DA")
            'Dim cn As New SqlConnection(CnnBD)
            ''Paso 1
            'oHospitalE.TipoDoc = 10
            'oHospitalE.CodAtencion = Session(sCodigoAtencion)
            'oHospitalE.CodUser = Session(sCodUser)
            'oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
            'oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            ''Paso 2
            'Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
            'cmd1.CommandType = CommandType.Text
            'cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
            'cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            'cmd1.Parameters.AddWithValue("@flg_reqfirma", "0")

            'Dim num1 As Integer
            'cn.Open()
            'num1 = cmd1.ExecuteNonQuery()
            'cn.Close()

            ''Paso 3
            'oHospitalE.IdeHistoria = Session(sIdeHistoria)
            'oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
            'oHospitalE.TipoDoc = 10
            'oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
            ''FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'FIN 1.4
        Else
            Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
        End If
        Return "OK"
    End Function

    ''' <summary>
    ''' CARGANDO DATOS DEL TREEVIEW DE ANALISIS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewAnalisis() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.TreeViewAnalisis_()
    End Function

    Public Function TreeViewAnalisis_() As String
        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
        oRceLaboratioE.IdeUsr = Session(sCodUser)
        oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
        oRceLaboratioE.Orden = 3
        Dim ValorDevolver As String = ""

        Dim LaboratorioR As String = ConfigurationManager.AppSettings("LABORATORIO_ROJO").Trim()
        Dim LaboratorioA As String = ConfigurationManager.AppSettings("LABORATORIO_AMARILLO").Trim()
        Dim LaboratorioV As String = ConfigurationManager.AppSettings("LABORATORIO_VERDE").Trim()

        Try
            Dim tabla As New DataTable()
            tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
            Dim CadenaEstructuraCC As String = ""
            Dim contFecRegistro As Integer = 0
            Dim contHorRegistro As Integer = 0
            CadenaEstructuraCC += "<ul class='JTreeView'>"
            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() <> "" Then
                        If contFecRegistro > 0 Then
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                            CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1

                            CadenaEstructuraCC += "</li>" 'cerrando nodo
                            contHorRegistro = 0
                        End If
                        CadenaEstructuraCC += "<li>" 'abriendo nodo
                        CadenaEstructuraCC += "<input type='hidden' value='" + tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() + "' class='JFECTREE' />"
                        CadenaEstructuraCC += "<span class='nudo'><img alt = '' src='../Imagenes/" + LaboratorioR + "' style='width:35px;height:35px;' />"
                        CadenaEstructuraCC += "<span class='JTREE2-FECHA' >" + tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() + "</span>"
                        CadenaEstructuraCC += "</span>"
                        CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 1
                        contFecRegistro += 1
                    End If
                    If tabla.Rows(index)("HOR_REGISTRO").ToString().Trim() <> "" Then 'CABECERA DE RECETA ANALISIS
                        Dim CadenaImgLag As String = ""
                        If tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "T" Then
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + LaboratorioV + "' style='width:35px;height:35px;' />"
                        ElseIf tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "A" Then  'jb - 13/07/2020 - se cambia de estado G -> A
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + LaboratorioR + "' style='width:35px;height:35px;'/>"
                        Else
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + LaboratorioA + "' style='width:35px;height:35px;'/>"
                        End If

                        If contHorRegistro > 0 Then
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                            CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                            'CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                        End If
                        CadenaEstructuraCC += "<li>" 'abriendo sub nodo 2
                        CadenaEstructuraCC += "<span class='nudo'>" + CadenaImgLag + " " + tabla.Rows(index)("IDE_RECETACAB").ToString().Trim() + " - " + tabla.Rows(index)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla.Rows(index)("IDE_RECETACAB").ToString().Trim() + "' />"
                        CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 3

                        contHorRegistro += 1
                    End If
                    If tabla.Rows(index)("HOR_REGISTRO").ToString().Trim() = "" And tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() = "" Then
                        Dim ProgramarHora As String = ""
                        If tabla.Rows(index)("DSC_PROGRAMACION").ToString().Trim() <> "" Then
                            ProgramarHora += " - Programado " + tabla.Rows(index)("DSC_PROGRAMACION").ToString().Trim()
                        End If
                        Dim CadenaImgLag As String = ""
                        If tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "T" Then
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + LaboratorioV + "' style='width:35px;height:35px;' />"
                        ElseIf tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "A" Then 'jb - 13/07/2020 - se cambia de estado G -> A
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + LaboratorioR + "' style='width:35px;height:35px;' />"
                        Else
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + LaboratorioA + "' style='width:35px;height:35px;' />"
                        End If
                        Dim CadenaFlgVerificar As String = ""
                        Dim CadenaFlgVerificar2 As String = ""
                        If tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "P" Or tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "T" Then
                            CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarLab' value='" + tabla.Rows(index)("FLG_VERIFICAR").ToString() + "' />"
                        Else
                            CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarLab' value='' />"
                        End If
                        If tabla.Rows(index)("FLG_VISTO").ToString().Trim() = "S" And tabla.Rows(index)("EST_ANALISIS").ToString().Trim() = "T" Then
                            CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
                        End If

                        CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index)("IDE_RECETACAB").ToString().Trim() + "' />" + CadenaFlgVerificar + CadenaImgLag + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index)("DSC_ANALISIS").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + ProgramarHora + "</span> " + CadenaFlgVerificar2 + "</li>"
                        ''INICIO 27/10/2016
                        'If tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
                        '    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'>G</span>"
                        'Else
                        '    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'></span>"
                        'End If
                        ''FIN 27/10/2016
                    End If


                    If index = (tabla.Rows.Count - 1) Then 'si llego al ultimo registro...
                        If contHorRegistro > 0 Then
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                            CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                        End If
                        If contFecRegistro > 0 Then
                            CadenaEstructuraCC += "</li>" 'cerrando nodo
                        End If
                    End If
                Next
            End If

            CadenaEstructuraCC += "</ul>"
            'REGRESO LA ESTRUCTURA DEL TREEVIEW
            Return CadenaEstructuraCC
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function

    'Public Function TreeViewAnalisis_() As String
    '    oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
    '    '@id_usr int
    '    oRceLaboratioE.IdeUsr = Session(sCodUser)
    '    oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
    '    oRceLaboratioE.Orden = 1
    '    Dim ValorDevolver As String = ""
    '    Try
    '        Dim tabla As New DataTable()
    '        tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
    '        ValorDevolver += "<ul>"
    '        For index = 0 To tabla.Rows.Count - 1
    '            If tabla.Rows(index)("ide_recetadet").ToString() = 0 Then
    '                If index > 0 Then
    '                    ValorDevolver += "</ul></li>"
    '                End If
    '                ValorDevolver += "<li>"

    '                If tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
    '                    ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Verde.jpg' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
    '                ElseIf tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
    '                    ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Rojo.png' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
    '                Else
    '                    ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Amarillo.jpg' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
    '                End If
    '                '14/10/2016
    '                If tabla.Rows(index)("est_analisis").ToString().Trim() = "P" Or tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
    '                    ValorDevolver += "<input type='hidden' class='FlgVerificarLab' value='" + tabla.Rows(index)("flg_verificar").ToString() + "' />"
    '                Else
    '                    ValorDevolver += "<input type='hidden' class='FlgVerificarLab' value='' />"
    '                End If
    '                'FIN 14/10/2016
    '                'INICIO 27/10/2016
    '                If tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
    '                    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'>G</span>"
    '                Else
    '                    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'></span>"
    '                End If
    '                'FIN 27/10/2016

    '                ValorDevolver += "<ul>"
    '            Else
    '                ValorDevolver += "<li>"
    '                Dim ProgramarHora As String = ""
    '                If tabla.Rows(index)("dsc_programacion").ToString().Trim() <> "" Then
    '                    ProgramarHora += " - Programado " + tabla.Rows(index)("dsc_programacion").ToString().Trim()
    '                End If

    '                If tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
    '                    ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Verde.jpg' />" + tabla.Rows(index)("dsc_analisis").ToString() + ProgramarHora + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
    '                ElseIf tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
    '                    ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Rojo.png' />" + tabla.Rows(index)("dsc_analisis").ToString() + ProgramarHora + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
    '                Else
    '                    ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Amarillo.jpg' />" + tabla.Rows(index)("dsc_analisis").ToString() + ProgramarHora + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
    '                End If
    '                'INICIO 14/09/2016 - JB
    '                If tabla.Rows(index)("est_analisis").ToString().Trim() = "P" Or tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
    '                    ValorDevolver += "<input type='hidden' class='FlgVerificarLab' value='" + tabla.Rows(index)("flg_verificar").ToString() + "' />"
    '                Else
    '                    ValorDevolver += "<input type='hidden' class='FlgVerificarLab' value='' />"
    '                End If
    '                If tabla.Rows(index)("flg_visto").ToString().Trim() = "S" And tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
    '                    ValorDevolver += " <img alt='' src='../Imagenes/ico_visto.png' />"
    '                End If
    '                'FIN 14/09/2016
    '                'INICIO 27/10/2016
    '                If tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
    '                    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'>G</span>"
    '                Else
    '                    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'></span>"
    '                End If
    '                'FIN 27/10/2016

    '                ValorDevolver += "</li>"
    '            End If
    '        Next
    '        ValorDevolver += "</ul>"
    '        Return ValorDevolver
    '    Catch ex As Exception
    '        ValorDevolver = "ERROR;" + ex.Message.ToString()
    '        Return ValorDevolver
    '    End Try

    'End Function

    'INICIO - JB - 22/07/2020
    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewAnalisis2(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.TreeViewAnalisisV2_(orden, fec_receta, ide_recetacab)
    End Function

    Public Function TreeViewAnalisisV2_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
        oRceLaboratioE.IdeRecetaCab = ide_recetacab
        oRceLaboratioE.FechaReceta = fec_receta
        oRceLaboratioE.HoraReceta = ""
        oRceLaboratioE.Orden = orden
        Dim ValorDevolver As String = ""

        Dim LaboratorioR As String = ConfigurationManager.AppSettings("LABORATORIO_ROJO").Trim()
        Dim LaboratorioA As String = ConfigurationManager.AppSettings("LABORATORIO_AMARILLO").Trim()
        Dim LaboratorioV As String = ConfigurationManager.AppSettings("LABORATORIO_VERDE").Trim()

        Try
            Dim tabla As New DataTable()
            tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_ConsultaV2(oRceLaboratioE)


            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    'INICIO - JB - 11/08/2020
                    Dim CadenaImgLag As String = ""

                    If Integer.Parse(tabla.Rows(index1)("A").ToString().Trim()) > 0 Then
                        CadenaImgLag += "JLAB-ROJO"
                    End If
                    If Integer.Parse(tabla.Rows(index1)("N").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("A").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JLAB-AMARILLO"
                    End If

                    If Integer.Parse(tabla.Rows(index1)("T").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("A").ToString().Trim()) = 0 And Integer.Parse(tabla.Rows(index1)("N").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JLAB-VERDE"
                    End If
                    'FIN - JB - 11/08/2020


                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaImgLag += "JLAB-VERDE"
                    ElseIf tabla.Rows(index1)("est_analisis").ToString().Trim() = "A" Or tabla.Rows(index1)("est_analisis").ToString().Trim() = "G" Then
                        CadenaImgLag += "JLAB-ROJO"
                    Else
                        CadenaImgLag += "JLAB-AMARILLO"
                    End If


                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + " - " + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HORA_RECETA").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim ProgramarHora As String = ""
                    If tabla.Rows(index1)("dsc_programacion").ToString().Trim() <> "" Then
                        ProgramarHora += " - Programado " + tabla.Rows(index1)("dsc_programacion").ToString().Trim()
                    End If
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaImgLag += "JLAB-VERDE"
                    ElseIf tabla.Rows(index1)("est_analisis").ToString().Trim() = "A" Or tabla.Rows(index1)("est_analisis").ToString().Trim() = "G" Then
                        CadenaImgLag += "JLAB-ROJO"
                    Else
                        CadenaImgLag += "JLAB-AMARILLO"
                    End If

                    Dim CadenaFlgVerificar As String = ""
                    Dim CadenaFlgVerificar2 As String = ""
                    If tabla.Rows(index1)("est_analisis").ToString().Trim() = "P" Or tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarLab' value='" + tabla.Rows(index1)("flg_verificar").ToString() + "' />"
                    Else
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarLab' value='' />"
                    End If
                    If tabla.Rows(index1)("flg_visto").ToString().Trim() = "S" And tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
                    End If

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("dsc_analisis").ToString().Trim() + " <span class='JETIQUETA_TREE2'>" + ProgramarHora + "</span> " + CadenaFlgVerificar2 + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + "'  /> " + CadenaFlgVerificar
                    ValorDevolver += "</div>"

                Next
            End If



            '<div id='divFecha2' class='JTREE3-FECHA'>
            '    <div class='JFILA-FECHA'>
            '        <div class='JTREE3-SIGNO'></div><div class='JLAB-ROJO'></div><div class='JVALOR-FECHA'>18/07/2020</div>
            '    </div> 
            '    <div class='JTREE3-HORA'> ******************************
            '        <div class='JFILA-HORA'>
            '            <div class='JTREE3-SIGNO'></div><div class='JLAB-ROJO'></div><div class='JVALOR-HORA'> 167254 - DIANA ELVIA | 01:15 PM</div><input type='hidden' id='Hidden3' value='167254' />
            '        </div>
            '        <div class='JFILA-DETALLE'> *******************
            '            <div class='JTREE3-DETALLE'>
            '                <div class='JTREE3-SIGNO'></div><div class='JLAB-VERDE'></div><div class='JVALOR-HORA'>UROCULTIVO</div><input type='hidden' id='Hidden4' value='167254'  /><input type='hidden' id='Hidden5' value='' />
            '            </div>
            '            <div class='JTREE3-DETALLE'>
            '                <div class='JTREE3-SIGNO'></div><div class='JLAB-VERDE'></div><div class='JVALOR-HORA'>GASES ARTERIALES</div><input type='hidden' id='Hidden6' value='167254'  /><input type='hidden' id='Hidden7' value='' />
            '            </div>
            '        </div>                                                                        
            '    </div>                                                                                                                                      
            '</div>

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function
    'FIN - JB - 22/07/2020


    <System.Web.Services.WebMethod()>
    Public Shared Function LaboratorioCompletado(ByVal IdReceta As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.LaboratorioCompletado_(IdReceta)
    End Function

    Public Function LaboratorioCompletado_(ByVal IdReceta As String) As String
        Try
            oRceLaboratioE.IdeRecetaCab = IdReceta
            oRceLaboratioE.Campo = "flg_revisado"
            oRceLaboratioE.ValorNuevo = "1"
            oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_Update(oRceLaboratioE)

            Return "OK"

        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function





    ''' <summary>
    ''' FUNCION PARA VER INFORME DE ANALISIS
    ''' </summary>
    ''' <param name="IdRecetaCab"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function VerInformeAnalisis(ByVal IdRecetaCab As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerInformeAnalisis_(IdRecetaCab)
    End Function
    Public Function VerInformeAnalisis_(ByVal IdRecetaCab As String) As String
        Dim NombreArchivo As String
        Dim Ruta As String = ""
        Dim RutaArchivo As String
        Dim bRutaWebRce As Boolean = False
        oRceLaboratioE.IdeRecetaCab = IdRecetaCab
        oRceLaboratioE.Orden = 1
        NombreArchivo = oRceLaboratioE.IdeRecetaCab.ToString() + ".PDF"
        Dim Mensaje As String = ""
        Dim tabla As New DataTable()

        Try
            'INI 1.3
            'bRutaWebRce = HttpContext.Current.Server.MapPath("/").Contains("wwwroot")

            'If bRutaWebRce Then
            '    Ruta = HttpContext.Current.Server.MapPath("/") + "\Archivos\" + NombreArchivo
            'Else
            '    Ruta = HttpContext.Current.Server.MapPath("\Archivos\" + NombreArchivo)
            'End If
            'FIN 1.3

            'TMACASSI 14/09/2016
            'Ruta = sRutaArchivos + NombreArchivo
            tabla = oRceLaboratorioLN.Sp_RceResultadoAnalisisCab_Consulta(oRceLaboratioE)

            For index = 0 To tabla.Rows.Count - 1
                If IsNothing(tabla.Rows(index)("blb_resultado")) Or IsDBNull(tabla.Rows(index)("blb_resultado")) Then
                    Mensaje = "ERROR;El informe se encuentra en proceso."
                    Return Mensaje
                End If
                'File.WriteAllBytes(Ruta, tabla.Rows(index)("blb_resultado")) '1.3 
            Next
            'RutaArchivo = ("/Archivos/" + NombreArchivo).Replace("//", "/") ' 1.3

            'Dim ie
            'ie = CreateObject("internetexplorer.application")
            'ie.Navigate(RutaArchivo)
            'ie.Visible = True

            'Return RutaArchivo '.13
            Return "" ' 1.3

        Catch ex As Exception
            Return ex.Message.ToString() + "****" + Ruta + "****" + CType(tabla.Rows.Count, String)
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA VALIDAR SI LA IMAGEN YA FUE AGREGADA
    ''' </summary>
    ''' <param name="IdImagen"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarImagenAgregado(ByVal IdImagen As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarImagenAgregado_(IdImagen)
    End Function
    Public Function ValidarImagenAgregado_(ByVal IdImagen As String) As String
        Dim tabla As New DataTable()
        Dim existe As String = ""
        If Not IsNothing(Session(sTablaImagenes)) Then
            tabla = CType(Session(sTablaImagenes), DataTable)
            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("ide_imagen").ToString().Trim() = IdImagen Then
                        existe = "SI"
                    End If
                Next
            End If
        End If
        Return existe
    End Function

    ''' <summary>
    ''' FUNCION PARA AGREGAR UNA IMAGEN
    ''' </summary>
    ''' <param name="DescripcionRecetaImagen"></param>
    ''' <param name="IdImagen"></param>
    ''' <param name="FlgFavorito"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function AgregarImagen(ByVal DescripcionRecetaImagen As String, ByVal IdImagen As String, ByVal FlgFavorito As Boolean) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.AgregarImagenCab(DescripcionRecetaImagen, IdImagen, FlgFavorito)
    End Function

    Public Function AgregarImagenCab(ByVal DescripcionRecetaImagen As String, ByVal IdImagen As String, ByVal FlgFavorito As Boolean) As String
        Try
            Dim InsertoFavorito As String
            InsertoFavorito = AgregarImagenFavorito(IdImagen, FlgFavorito)
            If InsertoFavorito <> "" And InsertoFavorito <> "OK" Then
                Return InsertoFavorito
            End If

            If IsNothing(Session(sIdeRecetaImagenCab)) Or Session(sIdeRecetaImagenCab) = 0 Then
                oRceImagenesE.CodAtencion = Session(sCodigoAtencion)
                oRceImagenesE.CodMedico = Session(sCodMedico)
                oRceImagenesE.UsrRegistra = Session(sCodUser)
                oRceImagenesE.DscReceta = DescripcionRecetaImagen.Trim().ToUpper()
                oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
                oRceImagenesE.IdeRecetaCab = 0
                oRceImagenLN.Sp_RceRecetaImagenCab_InsertV2(oRceImagenesE)
                Session(sIdeRecetaImagenCab) = oRceImagenesE.IdeRecetaCab
                Return AgregarImagenDet(IdImagen)
            Else
                oRceImagenesE.IdeRecetaCab = Session(sIdeRecetaImagenCab)
                oRceImagenesE.ValorNuevo = DescripcionRecetaImagen.Trim().ToUpper()
                oRceImagenesE.Campo = "dsc_receta"
                oRceImagenLN.Sp_RceRecetaImagenCab_Update(oRceImagenesE)
                Return AgregarImagenDet(IdImagen)
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    Public Function AgregarImagenDet(ByVal IdImagen As String) As String
        Try
            If Not IsNothing(Session(sIdeRecetaImagenCab)) Then
                oRceImagenesE.IdeRecetaCab = Session(sIdeRecetaImagenCab)
                oRceImagenesE.IdeImagen = IdImagen
                oRceImagenesE.UsrRegistra = Session(sCodUser)
                oRceImagenesE.CodMedico = Session(sCodMedico)
                oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
                oRceImagenLN.Sp_RceRecetaImagenDet_InsertV2(oRceImagenesE)
                If oRceImagenesE.IdeRecetaDet <> 0 And oRceImagenesE.IdeRecetaDet <> Nothing Then
                    Session(sEntidadImagenes) = oRceImagenesE
                    Return "OK"
                Else
                    Return "ERROR"
                End If
            Else
                Return "ERROR"
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    Public Function AgregarImagenFavorito(ByVal IdImagen As String, ByVal FlgFavorito As Boolean) As String
        Try
            If FlgFavorito = True Then
                oRceImagenesE.IdeImagen = IdImagen
                oRceImagenesE.CodMedico = Session(sCodMedico)
                oRceImagenesE.UsrRegistra = Session(sCodUser)
                oRceImagenesE.TipoDeAtencion = Session(sTipoAtencion)
                oRceImagenLN.Sp_RceImagenFavoritoMae_Insert(oRceImagenesE)

                If oRceImagenesE.IdImagenFavorito <> 0 Then
                    Return "OK"
                Else
                    Return "ERROR"
                End If
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA ENVIAR SOLICITUD DE IMAGENES
    ''' </summary>
    ''' <param name="Descripcion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarSolicitudImagenes(ByVal Descripcion As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarSolicitudImagenes_(Descripcion)
    End Function
    Public Function GuardarSolicitudImagenes_(ByVal Descripcion As String) As String
        Dim Mensaje As String = "No se pudo registrar el Analisis "
        Dim fallo As Boolean = False

        Try
            Dim TablaDatosP As New DataTable
            Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
            Dim cn1 As New SqlConnection(CnnBD)
            Dim cmd1 As New SqlCommand("Select  isnull(p.codpaciente,'') as codpac, " +
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
                                        "From   pacientes p, pacientesdet d " +
                                        "Where  p.codpaciente=d.codpaciente and " +
                                        "p.codpaciente=@cod_paciente", cn1)
            cmd1.Parameters.AddWithValue("@cod_paciente", Session(sCodPaciente))
            cmd1.Parameters.AddWithValue("@cod_medico", Session(sCodMedico))
            cmd1.CommandType = System.Data.CommandType.Text
            cn1.Open()
            Dim da1 As New SqlDataAdapter(cmd1)
            da1.Fill(TablaDatosP)
            cn1.Close()



            oRceImagenesE.IdeRecetaCab = Session(sIdeRecetaImagenCab)
            oRceImagenesE.ValorNuevo = "A"
            oRceImagenesE.Campo = "est_imagen"
            oRceImagenLN.Sp_RceRecetaImagenCab_Update(oRceImagenesE)

            If oRceImagenLN.Sp_RceRecetaImagenCab_Update(oRceImagenesE) Then
                oRceImagenesE.ValorNuevo = Descripcion
                oRceImagenesE.Campo = "dsc_receta"
                If oRceImagenLN.Sp_RceRecetaImagenCab_Update(oRceImagenesE) Then
                    Dim tabla As New DataTable()
                    tabla = CType(Session(sTablaImagenes), DataTable)
                    For index = 0 To tabla.Rows.Count - 1
                        oRceImagenesE.IdeRecetaDet = CType(tabla.Rows(index)("ide_recetadet").ToString(), Integer)
                        oRceImagenesE.ValorNuevo = "A"
                        oRceImagenesE.Campo = "est_imagen"
                        If oRceImagenLN.Sp_RceRecetaImagenDet_Update(oRceImagenesE) Then
                            GuardarRceRecetaImagenDet(TablaDatosP, oRceImagenesE)
                            'LLAMAR STORE DE TANIA - INSERTA PRESTACION 
                            'RECIBE CODIGO DE ORDEN, Y CODIGO DE PRESTACION
                            ' H015425 (2do digito es el local)
                            'LLAMAR Sp_Ris_EvaluaAtencion(ATENCION, CODIGO DE PRESTACION, local, 1)
                            'Devuelve -5, -10 (Si es -5 llamar web service)
                        Else
                            fallo = True
                            Mensaje += tabla.Rows(index)("ide_recetadet").ToString() + ", "
                        End If
                    Next
                    If fallo Then
                        Mensaje = Mensaje.Trim()
                        Return Mensaje.Remove(Mensaje.Length - 1)
                    Else

                        oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
                        oRceEvolucionE.IdeOrdenCab = Session(sIdeRecetaImagenCab)
                        oRceEvolucionE.Orden = 1
                        oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)

                        If oRceEvolucionE.CodigoEvolucion <> 0 Then
                            'INI 1.4
                            ''INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
                            'Dim pdf_byte As Byte() = ExportaPDF("DA")
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
                            'FIN 1.4
                        Else
                            Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
                        End If

                        Session.Remove(sIdeRecetaImagenCab)
                        Return "OK"
                    End If
                Else
                    Return ConfigurationManager.AppSettings(sMensajeActualizarError).ToString().Trim() + " - Sp_RceRecetaImagenCab_Update 2"
                End If
            Else
                Return ConfigurationManager.AppSettings(sMensajeActualizarError).ToString().Trim() + " - Sp_RceRecetaImagenCab_Update 1"
            End If

            'OrdenImagen(Session(sCodigoAtencion), Session(sIdeRecetaImagenCab), "")
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try

    End Function


    Public Function GuardarRceRecetaImagenDet(ByVal TablaDatosPaciente As DataTable, ByVal oRceImagenesE_1 As RceImagenesE) As String
        Try
            Dim oRceImagenesE_ As New RceImagenesE()
            Dim oRceImagenLN_ As New RceImagenLN()

            If Not IsNothing(Session(sEntidadImagenes)) Then
                oRceImagenesE_ = CType(Session(sEntidadImagenes), RceImagenesE)

                oRceImagenLN.Sp_PresotorImagen_Insert(oRceImagenesE_1)

                'Return "OK" 'JB - COMENTAR ESTA LINEA LUEGO - 03/09/2019
                Dim dt1 As New DataTable() 'JB - NUEVO - 17/10/2019
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
                        Respuesta = ObjetoServicioRisPacs.Insert_HIS("XML", "20", "0", oRceImagenesE_1.CodPresotor, EventDateTime, "1012", Trim(TablaDatosPaciente.Rows(0)("codpac")), Trim(TablaDatosPaciente.Rows(0)("dni")), + _
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
                    oRceImagenesE_.T_EVENT_ID = oRceImagenesE_1.CodPresotor 'oRceImagenesE_.IdeRecetaDet.ToString()
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
                Return "OK"
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try


    End Function

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
            cmd.Parameters.AddWithValue("@orden", 3) 'cambiar a orden 2     0
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

                Dim crystalreport As New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                crystalreport.Load(Server.MapPath("~/Intranet/Reportes/RpOrdenImagen.rpt"))
                crystalreport.SetDataSource(tabla2)

                Dim xNombreArchivo As String = ""
                Dim xRuta As String = ConfigurationManager.AppSettings("RutaOrdenImagen").Trim()
                'PREPERANDO EXPORTACION DE REPORTE A PDF
                Dim OpcionExportar As CrystalDecisions.Shared.ExportOptions
                Dim OpcionDestino As New CrystalDecisions.Shared.DiskFileDestinationOptions()
                Dim OpcionesFormato As New CrystalDecisions.Shared.PdfRtfWordFormatOptions()
                xNombreArchivo = Session(sCodigoAtencion) + "_" + tabla2.Rows(0)("ide_recetadet1").ToString().Trim() + ".pdf" 'cod_presotor (index + 1).ToString() + "_" 
                OpcionDestino.DiskFileName = xRuta + "\" + xNombreArchivo
                OpcionExportar = crystalreport.ExportOptions
                With OpcionExportar
                    .ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
                    .ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                    .DestinationOptions = OpcionDestino
                    .FormatOptions = OpcionesFormato
                End With
                'EXPORTANDO A PDF
                crystalreport.Export()
                'CONVIRTIENDO ARCHIVO PDF GENERADO EN BYTE()
                Dim pdf_byte As Byte() = System.IO.File.ReadAllBytes(xRuta + "\" + xNombreArchivo)
                System.IO.File.WriteAllBytes(xRuta + "\" + xNombreArchivo, pdf_byte)
                crystalreport.Close()
                crystalreport.Dispose()
            Next
        End If
    End Sub

    ''' <summary>
    ''' FUNCION PARA ELIMINAR LA IMAGEN DE FAVORITO
    ''' </summary>
    ''' <param name="IdeFavoritoImagen"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarFavoritoImagenes(ByVal IdeFavoritoImagen As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EliminarFavoritoImagenes_(IdeFavoritoImagen)
    End Function
    Public Function EliminarFavoritoImagenes_(ByVal IdeFavoritoImagen As String) As String
        Try
            oRceImagenesE.IdImagenFavorito = CType(IdeFavoritoImagen, Integer)
            Dim bEliminado As Boolean = oRceImagenLN.Sp_RceImagenFavoritoMae_Delete(oRceImagenesE)
            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA CARGAR EL TREEVIEW DE IMAGENES
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewImagenes() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.TreeViewImagenes_()
    End Function
    Public Function TreeViewImagenes_() As String
        oRceImagenesE.CodAtencion = Session(sCodigoAtencion)
        oRceImagenesE.IdeUsr = Session(sCodUser)
        oRceImagenesE.Orden = 7

        Dim ImagenR As String = ConfigurationManager.AppSettings("IMAGEN_ROJO").Trim()
        Dim ImagenA As String = ConfigurationManager.AppSettings("IMAGEN_AMARILLO").Trim()
        Dim ImagenV As String = ConfigurationManager.AppSettings("IMAGEN_VERDE").Trim()

        Try
            Dim tabla As New DataTable()
            tabla = oRceImagenLN.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)

            Dim CadenaEstructuraCC As String = ""
            Dim contFecRegistro As Integer = 0
            Dim contHorRegistro As Integer = 0
            CadenaEstructuraCC += "<ul class='JTreeView'>"
            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() <> "" Then
                        If contFecRegistro > 0 Then
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                            CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1

                            CadenaEstructuraCC += "</li>" 'cerrando nodo
                            contHorRegistro = 0
                        End If
                        CadenaEstructuraCC += "<li>" 'abriendo nodo
                        CadenaEstructuraCC += "<input type='hidden' value='" + tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() + "' class='JFECTREE' />"
                        CadenaEstructuraCC += "<span class='nudo'><img alt = '' src='../Imagenes/" + ImagenR + "' style='width:30px;height:30px;' />"
                        CadenaEstructuraCC += "<span class='JTREE2-FECHA' >" + tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() + "</span>"
                        CadenaEstructuraCC += "</span>"
                        CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 1
                        contFecRegistro += 1
                    End If
                    If tabla.Rows(index)("HOR_REGISTRO").ToString().Trim() <> "" Then 'CABECERA DE RECETA ANALISIS
                        Dim CadenaImgLag As String = ""
                        If tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "T" Then
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenV + "' style='width:30px;height:30px;' />"
                        ElseIf tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "P" Then
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenA + "' style='width:30px;height:30px;' />"
                        Else
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenR + "' style='width:30px;height:30px;' />"
                        End If

                        If contHorRegistro > 0 Then
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                            CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                            'CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                        End If
                        CadenaEstructuraCC += "<li>" 'abriendo sub nodo 2
                        CadenaEstructuraCC += "<span class='nudo'>" + CadenaImgLag + " " + tabla.Rows(index)("IDE_RECETACAB").ToString().Trim() + " - " + tabla.Rows(index)("MEDICO").ToString().Trim() + " | " + tabla.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla.Rows(index)("COD_PRESOTOR").ToString().Trim() + "_" + tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() + "' />"

                        CadenaEstructuraCC += "<input type='hidden' class='FlgVerificarIma' value='' />" 'siempre vacio
                        CadenaEstructuraCC += "<input type='hidden' class='IdeImagenDet' value='' />" 'siempre vacio

                        CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 3
                        contHorRegistro += 1
                    End If
                    If tabla.Rows(index)("HOR_REGISTRO").ToString().Trim() = "" And tabla.Rows(index)("FEC_REGISTRO").ToString().Trim() = "" Then
                        Dim CadenaImgLag As String = ""
                        If tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "" And tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "T" Then 'tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "0" And tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "C"
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenV + "' style='width:30px;height:30px;' />"
                        ElseIf tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "" Then
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenA + "' style='width:30px;height:30px;' />"
                        Else
                            CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenR + "' style='width:30px;height:30px;' />"
                        End If

                        Dim CadenaFlgVerificar As String = ""
                        Dim CadenaFlgVerificar2 As String = ""
                        If tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "T" And tabla.Rows(index)("FLG_VERIFICAR").ToString().Trim() = "S" Then
                            CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='" + tabla.Rows(index)("FLG_VERIFICAR").ToString() + "' />"
                        Else
                            CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='' />"
                        End If
                        If tabla.Rows(index)("FLG_VISTO").ToString().Trim() = "S" Then
                            CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
                        End If
                        CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index)("COD_PRESOTOR").ToString().Trim() + "_" + tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() + "' />" + CadenaFlgVerificar + "<input type='hidden' class='IdeImagenDet' value='" + tabla.Rows(index)("IDE_RECETADET").ToString().Trim() + "' />" + CadenaImgLag + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index)("COD_PRESTACION").ToString().Trim() + " - " + tabla.Rows(index)("DSC_IMAGEN").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'></span> " + CadenaFlgVerificar2 + "</li>"
                        ''INICIO 27/10/2016
                        'If tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
                        '    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'>G</span>"
                        'Else
                        '    ValorDevolver += "<span class='ESTADOANALISIS' style='display:none;'></span>"
                        'End If
                        ''FIN 27/10/2016
                    End If
                    If index = (tabla.Rows.Count - 1) Then 'si llego al ultimo registro...
                        If contHorRegistro > 0 Then
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                            CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                            CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                        End If
                        If contFecRegistro > 0 Then
                            CadenaEstructuraCC += "</li>" 'cerrando nodo
                        End If
                    End If
                Next
            End If

            CadenaEstructuraCC += "</ul>"
            'REGRESO LA ESTRUCTURA DEL TREEVIEW
            Return CadenaEstructuraCC
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewImagenes2(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.TreeViewImagenes2_(orden, fec_receta, ide_recetacab)
    End Function

    Public Function TreeViewImagenes2_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        oRceImagenesE.CodAtencion = Session(sCodigoAtencion)
        oRceImagenesE.IdeRecetaCab = ide_recetacab
        oRceImagenesE.FecReceta = fec_receta
        oRceImagenesE.HorReceta = ""
        oRceImagenesE.Orden = orden
        Dim ValorDevolver As String = ""
        Try
            Dim tabla As New DataTable()
            tabla = oRceImagenLN.Sp_RceRecetaImagenDet_ConsultaV2(oRceImagenesE)


            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    'INICIO - JB - 11/08/2020
                    Dim CadenaImgLag As String = ""

                    If Integer.Parse(tabla.Rows(index1)("G").ToString().Trim()) > 0 Then
                        CadenaImgLag += "JIMAG-ROJO"
                    End If
                    If Integer.Parse(tabla.Rows(index1)("P").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("G").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JIMAG-AMARILLO"
                    End If

                    If Integer.Parse(tabla.Rows(index1)("T").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("G").ToString().Trim()) = 0 And Integer.Parse(tabla.Rows(index1)("P").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JIMAG-VERDE"
                    End If
                    'FIN - JB - 11/08/2020



                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" Then
                        CadenaImgLag += "JIMAG-VERDE"
                    ElseIf tabla.Rows(index1)("est_imagen").ToString().Trim() = "P" Then
                        CadenaImgLag += "JIMAG-AMARILLO"
                    Else
                        CadenaImgLag += "JIMAG-ROJO"
                    End If


                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + " - " + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HORA_RECETA").ToString().Trim() + "</div><input type='hidden' class='IdeImagenCab' value='" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + "' />"  'COD_PRESOTOR + _ + SPS_ID_KEY
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("sps_id_key").ToString().Trim() <> "" And tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" Then 'tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "0" And tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "C"
                        CadenaImgLag += "JIMAG-VERDE"
                    ElseIf tabla.Rows(index1)("sps_id_key").ToString().Trim() <> "" Then
                        CadenaImgLag += "JIMAG-AMARILLO"
                    Else
                        CadenaImgLag += "JIMAG-ROJO"
                    End If

                    Dim CadenaFlgVerificar As String = ""
                    Dim CadenaFlgVerificar2 As String = ""
                    If tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" And tabla.Rows(index1)("flg_verificar").ToString().Trim() = "S" Then
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='" + tabla.Rows(index1)("flg_verificar").ToString() + "' />"
                    Else
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='' />"
                    End If
                    If tabla.Rows(index1)("flg_visto").ToString().Trim() = "S" Then
                        CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
                    End If

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("cod_prestacion").ToString().Trim() + " - " + tabla.Rows(index1)("dsc_imagen").ToString().Trim() + " " + CadenaFlgVerificar2 + "</div><input type='hidden' value='" + tabla.Rows(index1)("cod_presotor").ToString().Trim() + "_" + tabla.Rows(index1)("sps_id_key").ToString().Trim() + "' /> " + CadenaFlgVerificar + " <input type='hidden' class='IdeImagenDet' value='" + tabla.Rows(index1)("ide_recetadet").ToString().Trim() + "' /> "
                    ValorDevolver += "</div>"

                Next
            End If



            'Dim CadenaImgLag As String = ""
            'If tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "" And tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "T" Then 'tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "0" And tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "C"
            '    CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenV + "' style='width:30px;height:30px;' />"
            'ElseIf tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "" Then
            '    CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenA + "' style='width:30px;height:30px;' />"
            'Else
            '    CadenaImgLag += "<img alt='' src='../Imagenes/" + ImagenR + "' style='width:30px;height:30px;' />"
            'End If

            'Dim CadenaFlgVerificar As String = ""
            'Dim CadenaFlgVerificar2 As String = ""
            'If tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "T" And tabla.Rows(index)("FLG_VERIFICAR").ToString().Trim() = "S" Then
            '    CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='" + tabla.Rows(index)("FLG_VERIFICAR").ToString() + "' />"
            'Else
            '    CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='' />"
            'End If
            'If tabla.Rows(index)("FLG_VISTO").ToString().Trim() = "S" Then
            '    CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
            'End If
            'CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index)("COD_PRESOTOR").ToString().Trim() + "_" + tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() + "' />" + CadenaFlgVerificar + "<input type='hidden' class='IdeImagenDet' value='" + tabla.Rows(index)("IDE_RECETADET").ToString().Trim() + "' />" + CadenaImgLag + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index)("COD_PRESTACION").ToString().Trim() + " - " + tabla.Rows(index)("DSC_IMAGEN").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'></span> " + CadenaFlgVerificar2 + "</li>"



            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function ExamenCompletado(ByVal IdReceta As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ExamenCompletado_(IdReceta)
    End Function

    Public Function ExamenCompletado_(ByVal IdReceta As String) As String
        Try
            oRceImagenesE.IdeRecetaDet = IdReceta
            oRceImagenesE.Campo = "flg_revisado"
            oRceImagenesE.ValorNuevo = "1"
            oRceImagenLN.Sp_RceRecetaImagenDet_Update(oRceImagenesE)

            Return "OK"

        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function


    ''' <summary>
    ''' FUNCION PARA VER INFORME DE IMAGEN
    ''' </summary>
    ''' <param name="PresotorSps"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function VerInformeImagen(ByVal PresotorSps As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerInformeImagen_(PresotorSps)
    End Function
    Public Function VerInformeImagen_(ByVal PresotorSps As String) As String
        Try
            Dim xSpsIdKeyRis As String
            Dim xNombreArchivo As String
            Dim bRutaWebRce As Boolean
            Dim xRuta As String
            Dim Mensaje As String = ""
            Dim RutaArchivo As String

            xSpsIdKeyRis = PresotorSps.Trim().Split("_")(1).Trim()
            oRceImagenesE.CodPresotor = PresotorSps.Trim().Split("_")(0).Trim()
            xNombreArchivo = oRceImagenesE.CodPresotor + ".PDF"

            If oRceImagenesE.CodPresotor <> "" Then
                bRutaWebRce = HttpContext.Current.Server.MapPath("/").Contains("wwwroot")
                If bRutaWebRce Then
                    xRuta = HttpContext.Current.Server.MapPath("/") + "\Archivos\" + xNombreArchivo
                Else
                    xRuta = HttpContext.Current.Server.MapPath("\Archivos\" + xNombreArchivo)
                End If
                'TMACASSI 14/09/2016
                'xRuta = sRutaArchivos + xNombreArchivo
                Dim tabla As New DataTable()
                tabla = oRceImagenLN.Sp_Presotor_Pdf_Consulta(oRceImagenesE)

                If tabla.Rows.Count = 0 Then
                    Mensaje += "ERROR;No existe informe para el registro seleccionado."
                    Return Mensaje
                Else
                    For index = 0 To tabla.Rows.Count - 1
                        If Not IsNothing(tabla.Rows(index)("blob")) And Not IsDBNull(tabla.Rows(index)("blob")) Then
                            File.WriteAllBytes(xRuta, tabla.Rows(index)("blob"))
                        Else
                            Mensaje += "ERROR;El informe se encuentra en proceso."
                            Return Mensaje
                        End If
                    Next
                End If
            Else
                Mensaje += "ERROR;No existe informe para el registro seleccionado."
                Return Mensaje
            End If
            RutaArchivo = ("/Archivos/" + xNombreArchivo).Replace("//", "/")

            Return RutaArchivo
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    ''' <summary>
    ''' FUNCION PARA VER IMAGEN
    ''' </summary>
    ''' <param name="PresotorSps"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function VerImagen(ByVal PresotorSps As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerImagen_(PresotorSps)
    End Function
    Public Function VerImagen_(ByVal PresotorSps As String) As String
        'Dim wUrl As String = ""
        'Dim wUrl1 As String = ""
        'Dim wUrl2 As String = ""
        'Dim wUsuario As String = ""
        'Dim wContrasena As String = ""
        'Dim xSpsIdKeyRis As String
        Try
            'oRceImagenesE.CodTabla = "RIS_URL"
            'oRceImagenesE.Buscar = ""
            'oRceImagenesE.Key = 34
            'oRceImagenesE.NumeroLineas = 10
            'oRceImagenesE.Orden = 0
            'Dim tabla As New DataTable()
            'tabla = oRceImagenLN.Sp_Tablas_Consulta(oRceImagenesE)

            'For index = 0 To tabla.Rows.Count - 1
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "01" Then wUrl1 = tabla.Rows(index)("nombre").ToString().Trim()
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "02" Then wUrl2 = tabla.Rows(index)("nombre").ToString().Trim()
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "03" Then wUsuario = tabla.Rows(index)("nombre").ToString().Trim()
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "04" Then wContrasena = tabla.Rows(index)("nombre").ToString().Trim()
            'Next
            'xSpsIdKeyRis = PresotorSps.Trim().Split("_")(1).Trim()
            'wUrl = wUrl1 + wUrl2
            'wUrl = Replace(wUrl, "%U", wUsuario)
            'wUrl = Replace(wUrl, "%P", wContrasena)
            'wUrl = Replace(wUrl, "%O", xSpsIdKeyRis)

            'comentado 08/11/2016
            'Dim ie
            'ie = CreateObject("internetexplorer.application")
            'ie.Navigate(wUrl)
            'ie.Visible = True
            'fin comentado 08/11/2016


            Dim oTablasE As New TablasE
            Dim oListTablasE As New List(Of TablasE)
            Dim dt As New DataTable()
            Dim oTablasLN As New TablasLN
            oTablasE.CodTabla = "RIS_URL_RCE"
            oTablasE.Buscar = ""
            oTablasE.Key = 54
            oTablasE.NumeroLineas = 1
            oTablasE.Orden = 2
            dt = oTablasLN.Sp_Tablas_Consulta(oTablasE)
            Dim Dato01 = "", Dato02 = "", Dato03 = "", Dato04 = "", ImagenURL = "", ParametroURL As String

            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("codigo").ToString().Trim() = "01" Then
                    Dato01 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "02" Then
                    Dato02 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "03" Then
                    Dato03 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "04" Then
                    Dato04 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "05" Then
                    ImagenURL = dt.Rows(index)("nombre").ToString()
                End If
            Next
            'Dato01 = sRutaServicioRes 'JB - nueva linea, ruta ahora vendra de variables globales

            Dim xCadena As String
            Dim xSpsIdKeyRis As String
            Dim xPosicionRaya As Integer
            xCadena = PresotorSps
            xPosicionRaya = InStrRev(xCadena, "_")
            xSpsIdKeyRis = xCadena.Split("_")(1).Trim()

            ParametroURL = Dato02
            ParametroURL = Replace(ParametroURL, "%U", Dato03) 'Dato03  emr_csf
            ParametroURL = Replace(ParametroURL, "%P", Dato04) 'Dato04  csf123
            ParametroURL = Replace(ParametroURL, "%H", Session(sCodPaciente)) ' tabla1.Rows(0)("cod_paciente").ToString().Trim())
            ParametroURL = Replace(ParametroURL, "%O", xSpsIdKeyRis) 'JCAICEDO.28/03/2019 Se restauro el SpIdKey '"9275000605840") 'sps_id hardcore temporalmente para pruebas *xSpsIdKeyRis*
            ParametroURL = """" + ParametroURL + """"

            Dim CadenaA As String = ""
            CadenaA = Dato01 + Dato02.Substring(0, Dato02.LastIndexOf("%P") + 2)
            CadenaA = CadenaA.Replace("%U", Dato03) 'Dato03  emr_csf
            CadenaA = CadenaA.Replace("%P", Dato04) 'Dato04  csf123

            Dim client = New RestClient(CadenaA)
            Dim request = New RestRequest(Method.POST)
            request.AddHeader("cache-control", "no-cache")
            request.AddHeader("content-type", "application/json")
            'request.AddBody("""user_name=emr_csf&password=csf123&patient_id=406363&accssion_number=9275000574535""")
            request.AddParameter("application/json", ParametroURL, RestSharp.ParameterType.RequestBody)

            Dim response As RestResponse = client.Execute(request)
            Dim encriptado As String = response.Content.Replace("""", "")


            'Return ImagenURL + encriptado JB - COMENTADO - 01/03/2021
            Dim linkURL As String = ""
            linkURL = Pacs(Dato03, Dato04, Session(sCodPaciente), xSpsIdKeyRis, ImagenURL, Dato01)


            Return linkURL

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function



    Function Pacs(ByVal wUser As String, ByVal wPass As String, ByVal wIdPatient As String, ByVal wOrden As String, ByVal wLink1 As String, ByVal wUrl1 As String) As String

        Dim wLink As String = wLink1 ' "https://pacs.clinicasanfelipe.com/portal/default.aspx?urltoken="
        Dim Url As String = wUrl1 '"https://pacs.clinicasanfelipe.com/CSPublicQueryService/CSPublicQueryService.svc/json/EncryptQS?"
        Dim Params As String = "user_name=%U&password=%P&patient_id=%H&accession_number=%O"



        Params = Replace(Trim(Params & ""), "%U", wUser, 1)
        Params = Replace(Trim(Params & ""), "%P", wPass, 1)
        Params = Replace(Trim(Params & ""), "%O", wOrden, 1)
        Params = Replace(Trim(Params & ""), "%H", wIdPatient, 1)

        Dim client = New RestClient(Url & Params)
        Dim request = New RestRequest(Method.POST)

        request.AddHeader("cache-control", "no-cache")
        request.AddHeader("content-type", "application/json")

        request.AddParameter("application/json", """user_name=" & wUser & "&password=" & wPass & "&patient_id=" & wIdPatient & "&accession_number=" & wOrden & """", RestSharp.ParameterType.RequestBody)

        If Url.IndexOf("https") >= 0 Then
            System.Net.ServicePointManager.SecurityProtocol = 3072
        End If

        Dim response As RestResponse = client.Execute(request)

        wLink = wLink & Mid(response.Content, 2, Len(response.Content) - 2) 'response.Content

        Return wLink

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaCancelarOrden(ByVal PresotorSps As String, ByVal IdeImagenDet As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaCancelarOrden_(PresotorSps, IdeImagenDet)
    End Function

    Public Function ValidaCancelarOrden_(ByVal PresotorSps As String, ByVal IdeImagenDet As String) As String
        Dim Mensaje As String = ""
        Dim oRceImagenesE_ As New RceImagenesE()
        Dim oRceImagenLN_ As New RceImagenLN()

        Dim CodPresotor As String = ""
        Dim SpsKey As String = ""
        CodPresotor = PresotorSps.Trim().Split("_")(0)
        SpsKey = PresotorSps.Trim().Split("_")(1)

        Dim Tabla1 As New DataTable()
        oRceImagenesE_.CodTabla = "01"
        Tabla1 = oRceImagenLN_.Sp_Ris_Consulta_RisPacs(oRceImagenesE_)

        If Tabla1.Rows.Count > 0 Then 'si hay data
            If Tabla1.Rows(0)("estado").ToString().Trim() = "A" Then
                Dim Tabla2 As New DataTable()
                oRceImagenesE_.CodTabla = "10"
                Tabla2 = oRceImagenLN_.Sp_Ris_Consulta_RisPacs(oRceImagenesE_)
                If Tabla2.Rows.Count > 0 Then 'si hay data
                    If Tabla2.Rows(0)("estado").ToString().Trim() = "A" Then
                        Dim Tabla3 As New DataTable()
                        oRceImagenesE_.CodPresotor = CodPresotor
                        oRceImagenesE_.Orden = 3
                        Tabla3 = oRceImagenLN_.Sp_Presotor_Consulta2(oRceImagenesE_)
                        If Tabla3.Rows.Count > 0 Then 'si hay data
                            Dim Tabla4 As New DataTable()
                            oRceImagenesE_.CodPresotor = CodPresotor
                            Tabla4 = oRceImagenLN_.Sp_Presotor_Consulta(oRceImagenesE_)

                            If Tabla4.Rows.Count > 0 Then
                                Mensaje = "Examen " + CodPresotor + "-" + Tabla4.Rows(0)("nombreprestacion") + " está realizado. No se puede eliminar."
                            End If

                        End If

                    End If
                End If
            End If
        End If
        Return Mensaje
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function AnularOrdenImagen(ByVal PresotorSps As String, ByVal IdeImagenDet As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.AnularOrdenImagen_(PresotorSps, IdeImagenDet)
    End Function

    Public Function AnularOrdenImagen_(ByVal PresotorSps As String, ByVal IdeImagenDet As String) As String
        Try
            Dim Mensaje As String = ""
            Dim oRceImagenesE_ As New RceImagenesE()
            Dim oRceImagenLN_ As New RceImagenLN()
            Dim CodPresotor As String = ""
            Dim SpsKey As String = ""
            CodPresotor = PresotorSps.Trim().Split("_")(0)
            SpsKey = PresotorSps.Trim().Split("_")(1)

            oRceImagenesE_.CodPresotor = CodPresotor
            oRceImagenesE_.IdeUsr = Session(sCodUser)
            oRceImagenLN_.Sp_Presotor_DeleteNewv3(oRceImagenesE_)

            'If oRceImagenesE_.CodPresotorNew <> "" Then
            'End If
            Dim Tabla1 As New DataTable()
            oRceImagenesE_.CodTabla = "01"
            Tabla1 = oRceImagenLN_.Sp_Ris_Consulta_RisPacs(oRceImagenesE_)
            If Tabla1.Rows.Count > 0 Then 'si hay data
                If Tabla1.Rows(0)("estado").ToString().Trim() = "A" Then
                    Dim Tabla2 As New DataTable()
                    oRceImagenesE_.CodTabla = "11"
                    Tabla2 = oRceImagenLN_.Sp_Ris_Consulta_RisPacs(oRceImagenesE_)
                    If Tabla2.Rows.Count > 0 Then 'si hay data
                        If Tabla2.Rows(0)("estado").ToString().Trim() = "A" Then
                            Dim retorno As Integer
                            retorno = EnviarCancel_a_RisPacs(CodPresotor)

                            If retorno = 1 Or retorno = 2 Then
                                Mensaje = "No se pudo eliminar examen en RISPACS."
                            End If

                            If retorno = 3 Then
                                Mensaje = "Orden no se llego a enviar a RIS"
                            End If

                        End If
                    End If

                    If Mid(CodPresotor, 1, 1) <> "E" And Mid(CodPresotor, 1, 1) <> "H" And Mid(CodPresotor, 1, 1) <> "Q" Then
                        oRceImagenesE_.CodPresotor = CodPresotor
                        oRceImagenLN_.Sp_Ris_AgendamientoAmbulatorio_Delete(oRceImagenesE_)
                    End If
                End If
            End If

            Return Mensaje
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function


    Public Function EnviarCancel_a_RisPacs(ByVal CodPresotor As String) As Integer
        Try
            Dim result As Integer = 0
            Dim oRceImagenesE_ As New RceImagenesE()
            Dim oRceImagenLN_ As New RceImagenLN()
            oRceImagenesE_.CodPresotor = CodPresotor
            Dim tabla As New DataTable()
            tabla = oRceImagenLN_.ConsultaRisOracle(oRceImagenesE_)

            If tabla.Rows.Count = 0 Then 'si no hay data
                result = 3
                Return result
            End If

            Dim wCodPaciente As String = ""
            Dim wDocIdentidad As String
            Dim wNombre As String
            Dim wApPaterno As String
            Dim wApMaterno As String
            Dim wSexo As String
            Dim wFechaNacimiento As String
            Dim wCodPrestacion As String
            Dim wDesPrestacion As String
            Dim wSpsId As String
            Dim wSequenceId As String
            Dim wRoomCode As String
            Dim wEstado As String
            Dim MedSolicita As String 'TMACASSI 22/08/2018
            Dim xEmail As String 'TMACASSI 22/08/2018
            Dim xMovil As String 'TMACASSI 22/08/2018
            Dim wCodError As Long
            Dim RequestBy As String
            Dim Mensaje As String 'TMACASSI 21/06/2019

            Mensaje = "XML-CANCEL"
            RequestBy = "" 'TMACASSI 22/08/2018
            xMovil = "" 'TMACASSI 22/08/2018
            Dim xDireccion As String = "" 'JB

            Dim Tabla2 As New DataTable()
            Tabla2 = oRceImagenLN_.Sp_Presotor_Consulta(oRceImagenesE_)

            If Tabla2.Rows.Count > 0 Then
                wCodPrestacion = Trim(Tabla2.Rows(0)("codprestacion").ToString().Trim())
                wDesPrestacion = Trim(Tabla2.Rows(0)("nombreprestacion").ToString())
                wCodPaciente = Trim(Tabla2.Rows(0)("codpaciente").ToString())
                wSpsId = Trim(Tabla2.Rows(0)("pacs_sps_id").ToString())
                wRoomCode = Trim(Tabla2.Rows(0)("codsala").ToString())
            End If

            oRceImagenesE_.CodPaciente = wCodPaciente
            oRceImagenesE_.Key = 50
            oRceImagenesE_.NumeroLineas = 1
            oRceImagenesE_.Orden = -1
            oRceImagenesE_.DocIdentidad = ""
            Dim Tabla3 As New DataTable()
            Tabla3 = oRceImagenLN_.Sp_Pacientes_Consulta(oRceImagenesE_)

            If Tabla3.Rows.Count > 0 Then
                wDocIdentidad = Trim(Tabla3.Rows(0)("docidentidad").ToString())
                wNombre = Trim(Tabla3.Rows(0)("nombre").ToString())
                wApPaterno = Trim(Tabla3.Rows(0)("appaterno").ToString())
                wApMaterno = Trim(Tabla3.Rows(0)("apmaterno").ToString())
                wSexo = Trim(Tabla3.Rows(0)("sexo").ToString())
                'wFechaNacimiento = Trim(wAdo("fechanacimiento").Value & "")
                wFechaNacimiento = Format(Trim(Tabla3.Rows(0)("fechanacimiento").ToString()), "YYYYMMDD") 'TMACASSI 23/08/2018
                wEstado = Trim(Tabla3.Rows(0)("estado").ToString())
                xEmail = Trim(Tabla3.Rows(0)("CORREO").ToString()) 'TMACASSI 22/08/2018
                xMovil = Trim(Tabla3.Rows(0)("telefono2").ToString()) 'TMACASSI 22/08/2018
                xDireccion = Trim(Tabla3.Rows(0)("direccion").ToString()) 'JBx

            End If

            Dim Empresa As String
            Dim Sucursal As String
            Dim EventDateTimeOracle As String
            Dim EventDateTimeSQL As String
            Dim EventTypeID As String
            Dim TipoPaciente As String

            Dim DeathIndicator As String
            Dim LastName As String
            Dim FirstName As String
            Dim BirthDate As String
            Dim GenderKey As String = ""

            Dim ProcedureCode As String
            Dim ProcedureDescription As String
            Dim SpsId As String
            Dim SequenceId As String
            Dim RoomCode As String
            Dim Status As String

            Dim Ahora As String
            Ahora = DateTime.Now.ToString()  '*****************************QUE ES NOW?

            Empresa = "20"
            Sucursal = "0" 'TMACASSI 12/09/2018 Se cambia a 0 por nueva version

            EventDateTimeOracle = Format(CDate(Date.Now), "dd/MM/yyyy h:mm:ss") 'Format(Format(CDate(Ahora), "yyyy/mm/dd h:mm:ss"), "dd/mm/yyyy h:mm:ss")
            EventDateTimeSQL = Format(CDate(Date.Now), "MM/dd/yyyy h:mm:ss") 'Format(Format(CDate(Ahora), "yyyy/mm/dd h:mm:ss"), "mm/dd/yyyy h:mm:ss")
            EventTypeID = "1090"


            If Mid(CodPresotor, 1, 1) = "E" Or Mid(CodPresotor, 1, 1) = "Q" Then
                TipoPaciente = "1"
            Else
                If Mid(CodPresotor, 1, 1) = "H" Then
                    TipoPaciente = "2"
                Else
                    TipoPaciente = "3"
                End If
            End If
            If wEstado = "F" Then
                DeathIndicator = "Y"
            Else
                DeathIndicator = "N"
            End If
            LastName = wApPaterno & " " & wApMaterno
            FirstName = wNombre



            '18/12/2009
            If wFechaNacimiento <> "" Then
                BirthDate = wFechaNacimiento 'TMACASSI 07/12/2018 Format(Format(CDate(wFechaNacimiento), "dd/mm/yyyy"), "yyyymmdd")
            End If

            If wSexo = "F" Then
                GenderKey = "1"
            Else
                If wSexo = "M" Then
                    GenderKey = "2"
                Else
                    GenderKey = "4" ' <TMACASSI> 21/01/2013 genero no conocido = 4
                End If
            End If
            ProcedureCode = wCodPrestacion
            ProcedureDescription = wDesPrestacion
            SpsId = wSpsId
            SequenceId = "1"
            RoomCode = wRoomCode
            Status = "Cancelled"

            If Trim(SpsId) = "" Then
                SpsId = CodPresotor
            End If



            Dim strSoap As String
            Dim strSoapAction As String
            Dim strWsdl As String
            Dim Resultado As String
            Dim vacio As String


            strSoapAction = ""
            strWsdl = ""
            Resultado = ""
            vacio = ""


            Dim dt1 As New DataTable()
            dt1 = oRceImagenLN_.RIS_PACS_WS()

            Dim ObjetoServicioRisPacs As New WsRisPacs.HisXmlEvents()
            ObjetoServicioRisPacs.Url = dt1.Rows(0)("nombre").ToString().Trim()

            Resultado = ObjetoServicioRisPacs.Insert_HIS_CANCEL(Empresa, Sucursal, CodPresotor, EventDateTimeOracle, EventTypeID, wCodPaciente, wDocIdentidad, TipoPaciente,
                                                                DeathIndicator, LastName, FirstName, xMovil, xEmail, BirthDate, GenderKey, ProcedureCode, ProcedureDescription,
                                                                SpsId, SequenceId, wSpsId, RoomCode, RequestBy, Status) 'JB - COMENTADO PARA PRUEBAS EN PRE TEMPORALMENTE

            'Resultado = "OK-WS-ORM" 'JB - hardcode temporalmente


            'QUE ES    InvokeWebService(Trim(strSoap), strSoapAction, strWsdl, xmlResponse)  ???
            If Resultado = "OK-WS-ORM" Then
                result = 0

                oRceImagenesE_.ORACLE = "A"
                oRceImagenesE_.X_TIPOMSG = Mensaje
                oRceImagenesE_.T_COD_EMPRESA = Empresa
                oRceImagenesE_.T_COD_SUCURSAL = Sucursal
                oRceImagenesE_.T_EVENT_ID = CodPresotor
                oRceImagenesE_.T_EVENT_DATETIME = EventDateTimeSQL
                oRceImagenesE_.T_EVENT_TYPE_ID = EventTypeID
                oRceImagenesE_.X_ID_PACIENTE = Trim(wCodPaciente)
                oRceImagenesE_.X_RUT_PACIENTE = Trim(wDocIdentidad)
                oRceImagenesE_.X_TIPO_PACIENTE = Trim(TipoPaciente)
                oRceImagenesE_.X_DEATH_INDICATOR = "N"
                oRceImagenesE_.X_CAT_NAME = Session(sCodUser)
                oRceImagenesE_.X_LAST_NAME = Trim(LastName)
                oRceImagenesE_.X_FIRST_NAME = Trim(FirstName)
                oRceImagenesE_.X_BIRTH_DATE = BirthDate
                oRceImagenesE_.X_GENDER_KEY = Trim(GenderKey)
                oRceImagenesE_.X_LAST_UPDATED = ""
                oRceImagenesE_.X_STREET_ADDRESS = Trim(xDireccion) 'xDireccion - JB
                oRceImagenesE_.X_CITY = "LIMA"
                oRceImagenesE_.X_COUNTRY = "PERU"
                oRceImagenesE_.X_PHONE_NUMBER = Trim(xMovil)
                oRceImagenesE_.X_VISIT_NUMBER = ""
                oRceImagenesE_.X_START_DATETIME = EventDateTimeSQL
                oRceImagenesE_.X_DURATION = ""
                oRceImagenesE_.X_STATUS_KEY = Trim(EventTypeID)
                oRceImagenesE_.X_STATUS = Trim(Status)
                oRceImagenesE_.X_PROCEDURE_CODE = Trim(ProcedureCode)
                oRceImagenesE_.X_PROCEDURE_DESCRIPTION = Trim(ProcedureDescription)
                oRceImagenesE_.X_ROOM_CODE = RoomCode
                oRceImagenesE_.X_REQUESTED_BY = Trim(RequestBy)
                oRceImagenesE_.X_MESSAGE_TYPE = ""
                oRceImagenesE_.X_PACS_SPS_ID = ""
                oRceImagenesE_.MSG_STATUS = Resultado

                oRceImagenLN_.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE_)
            Else
                result = 1

                oRceImagenesE_.ORACLE = "A"
                oRceImagenesE_.X_TIPOMSG = Mensaje
                oRceImagenesE_.T_COD_EMPRESA = Empresa
                oRceImagenesE_.T_COD_SUCURSAL = Sucursal
                oRceImagenesE_.T_EVENT_ID = CodPresotor
                oRceImagenesE_.T_EVENT_DATETIME = EventDateTimeSQL
                oRceImagenesE_.T_EVENT_TYPE_ID = EventTypeID
                oRceImagenesE_.X_ID_PACIENTE = Trim(wCodPaciente)
                oRceImagenesE_.X_RUT_PACIENTE = Trim(wDocIdentidad)
                oRceImagenesE_.X_TIPO_PACIENTE = Trim(TipoPaciente)
                oRceImagenesE_.X_DEATH_INDICATOR = "N"
                oRceImagenesE_.X_CAT_NAME = Session(sCodUser)
                oRceImagenesE_.X_LAST_NAME = Trim(LastName)
                oRceImagenesE_.X_FIRST_NAME = Trim(FirstName)
                oRceImagenesE_.X_BIRTH_DATE = BirthDate
                oRceImagenesE_.X_GENDER_KEY = Trim(GenderKey)
                oRceImagenesE_.X_LAST_UPDATED = ""
                oRceImagenesE_.X_STREET_ADDRESS = Trim(xDireccion) 'xDireccion - JB
                oRceImagenesE_.X_CITY = "LIMA"
                oRceImagenesE_.X_COUNTRY = "PERU"
                oRceImagenesE_.X_PHONE_NUMBER = Trim(xMovil)
                oRceImagenesE_.X_VISIT_NUMBER = ""
                oRceImagenesE_.X_START_DATETIME = EventDateTimeSQL
                oRceImagenesE_.X_DURATION = ""
                oRceImagenesE_.X_STATUS_KEY = Trim(EventTypeID)
                oRceImagenesE_.X_STATUS = Trim(Status)
                oRceImagenesE_.X_PROCEDURE_CODE = Trim(ProcedureCode)
                oRceImagenesE_.X_PROCEDURE_DESCRIPTION = Trim(ProcedureDescription)
                oRceImagenesE_.X_ROOM_CODE = RoomCode
                oRceImagenesE_.X_REQUESTED_BY = Trim(RequestBy)
                oRceImagenesE_.X_MESSAGE_TYPE = ""
                oRceImagenesE_.X_PACS_SPS_ID = ""
                oRceImagenesE_.MSG_STATUS = Resultado
                oRceImagenLN_.Sp_Ris_Oracle_His_Xml_Events(oRceImagenesE_)
            End If

            Return result
        Catch ex As Exception
            Return 0
        End Try
    End Function


    ''' <summary>
    ''' FUNCION PARA GUARDAR LOS DATOS DE INTERCONSULTA
    ''' </summary>
    ''' <param name="IdInterconsulta"></param>
    ''' <param name="IdMotivo"></param>
    ''' <param name="Descripcion"></param>
    ''' <param name="CodEspecialidad"></param>
    ''' <param name="CodMedico"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarInterconsulta(ByVal IdInterconsulta As String, ByVal IdMotivo As String, ByVal Descripcion As String, ByVal CodEspecialidad As String, ByVal CodMedico As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarInterconsulta_(IdInterconsulta, IdMotivo, Descripcion, CodEspecialidad, CodMedico)
    End Function
    Public Function GuardarInterconsulta_(ByVal IdInterconsulta As String, ByVal IdMotivo As String, ByVal Descripcion As String, ByVal CodEspecialidad As String, ByVal CodMedico As String) As String
        Dim ValorDevolver As String = ""
        Try
            oInterconsultaE.IdeHistoria = Session(sIdeHistoria)
            'oInterconsultaE.Atencion = Session(sCodigoAtencion)
            oInterconsultaE.IdeSolicitante = Session(sCodMedico)
            If IdInterconsulta.Trim() = "" Then
                oInterconsultaLN.Sp_RceInterconsulta_Insert(oInterconsultaE)
                'oInterconsultaE.IdeInterConsulta
                If oInterconsultaE.IdeInterConsulta = 0 Or oInterconsultaE.IdeInterConsulta = Nothing Or IsNothing(oInterconsultaE.IdeInterConsulta) Then
                    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Insert"
                End If

                'INICIO - JB - NUEVO CODIGO - 21/07/2020
                oInterconsultaE.Campo = "fec_registra"
                oInterconsultaE.ValorNuevo = Session(sCodUser)
                oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
                'FIN - JB - NUEVO CODIGO


                oInterconsultaE.Campo = "usr_registra"
                oInterconsultaE.ValorNuevo = Session(sCodUser)
                oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
                'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
                '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-usr_registra2"
                'End If
                oInterconsultaE.Campo = "fec_solicitud"
                oInterconsultaE.ValorNuevo = ""
                oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
                'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
                '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-fec_solicitud2"
                'End If
            Else
                oInterconsultaE.IdeInterConsulta = CType(IdInterconsulta.Trim(), Integer)
            End If

            oInterconsultaE.Campo = "usr_modifica"
            oInterconsultaE.ValorNuevo = Session(sCodUser)
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
            'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
            '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-usr_modifica2"
            'End If
            oInterconsultaE.Campo = "ide_motivo"
            oInterconsultaE.ValorNuevo = IdMotivo
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
            'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
            '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-ide_motivo"
            'End If
            oInterconsultaE.Campo = "txt_solicitud"
            oInterconsultaE.ValorNuevo = Descripcion.Trim().ToUpper()
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
            'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
            '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-txt_solicitud"
            'End If
            oInterconsultaE.Campo = "codespecialidad"
            oInterconsultaE.ValorNuevo = CodEspecialidad
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
            'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
            '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-codespecialidad"
            'End If
            oInterconsultaE.Campo = "ide_solicitado"
            oInterconsultaE.ValorNuevo = CodMedico
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)
            'If oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE) Then
            '    Return "ERROR;" + ConfigurationManager.AppSettings(sMensajeGuardarError).ToString().Trim() + " - Sp_RceInterconsulta_Update-ide_solicitado"
            'End If

            'TMACASSI 07/09/2016 Registrar log de interconsulta
            oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
            oRceEvolucionE.IdeOrdenCab = oInterconsultaE.IdeInterConsulta
            oRceEvolucionE.Orden = 3
            oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)


            'INICIO - JB - ENVIAR CORREO

            If CodMedico <> "" Then
                oInterconsultaE.Buscar = CodMedico
                oInterconsultaE.Orden = 16
                Dim dt As New DataTable()
                dt = oInterconsultaLN.Sp_Medicos_Consulta(oInterconsultaE)

                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("email1").ToString().Trim() = "" Then
                        oInterconsultaE.Enviara = dt.Rows(0)("email2").ToString().Trim()
                    Else
                        oInterconsultaE.Enviara = dt.Rows(0)("email1").ToString().Trim()
                    End If

                    If dt.Rows(0)("email1").ToString().Trim() <> "" And dt.Rows(0)("email2").ToString().Trim() <> "" Then
                        oInterconsultaE.Copiara = dt.Rows(0)("email2").ToString().Trim()
                    End If

                    oTablasE.CodTabla = "INTERCONSULTA_MOTIVO"
                    oTablasE.Buscar = ""
                    oTablasE.Key = 0
                    oTablasE.NumeroLineas = 0
                    oTablasE.Orden = 5
                    Dim tabla_ As New DataTable()
                    Dim DescripcionMotivo As String = ""
                    tabla_ = oTablasLN.Sp_Tablas_Consulta(oTablasE)
                    Dim rows() As DataRow = tabla_.Select("codigo = '" + IdMotivo + "'")
                    If rows.Count > 0 Then
                        DescripcionMotivo = rows(0).Item("nombre")
                    End If

                    oInterconsultaE.Asunto = "Interconsulta - " + DescripcionMotivo
                    oInterconsultaE.Cuerpo = Descripcion.Trim().ToUpper()

                    'INICIO - JB - NUEVO - 03/08/2020
                    'CAPTURANDO LOS VALORES DE LA INTERCONSULTA REGISTRADA
                    Dim FechaSolicitud As String = ""
                    Dim MotivoInterconsulta_ As String = ""
                    Dim EspecialidadInterconsulta_ As String = ""
                    Dim MedicoInterconsulta_ As String = ""
                    oInterconsultaE.Atencion = Session(sCodigoAtencion)
                    oInterconsultaE.Orden = 1
                    Dim TablaInterconsulta As New DataTable()
                    TablaInterconsulta = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)

                    Dim fila() As DataRow
                    fila = TablaInterconsulta.Select("ide_interconsulta = " + oInterconsultaE.IdeInterConsulta.ToString() + "")
                    If fila.Count > 0 Then
                        FechaSolicitud = fila(0).Item("fec_solicitud").ToString().Trim()
                        MotivoInterconsulta_ = fila(0).Item("dsc_motivo").ToString().Trim()
                        EspecialidadInterconsulta_ = fila(0).Item("especialidad").ToString().Trim()
                        MedicoInterconsulta_ = fila(0).Item("medico").ToString().Trim()
                    End If

                    'OBTENIENDO DATOS PARA EL ENVIO DE CORREO
                    Dim oHospitalE_ As New HospitalE()
                    Dim oHospitalLN_ As New HospitalLN()
                    oHospitalE_.NombrePaciente = Session(sCodigoAtencion)
                    oHospitalE_.Pabellon = ""
                    oHospitalE_.Servicio = ""
                    oHospitalE_.Orden = 3
                    Dim tabla As New DataTable()
                    tabla = oHospitalLN_.Sp_RceHospital_Consulta(oHospitalE_)

                    'oInterconsultaE.Enviara = "jbarreto@clinicasanfelipe.com"
                    oInterconsultaE.Cuerpo = "<div style='border-radius:10px;border:5px solid #134B8D;padding:10px;color:#134B8D;font-family:calibri;font-size:14px;background-color:#EDEDED;'>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Atencion: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("codatencion").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>HC: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("codpaciente").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Apellidos y Nombres: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("nombres").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Edad: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("edad").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Género: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("sexo").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Estado Civil: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + IIf(tabla.Rows(0)("codcivil").ToString().Trim() = "S", "SOLTERO", "CASADO") + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Télefono: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("telefono").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Habitacion: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("cama").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Médico: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("NombreMedico").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Aseguradora: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + tabla.Rows(0)("NombreAseguradora").ToString().Trim() + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Fecha Solicitud: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + FechaSolicitud + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Motivo: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + MotivoInterconsulta_ + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Especialidad: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + EspecialidadInterconsulta_ + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Medico: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;'>" + MedicoInterconsulta_ + "</div></div>" +
                                            "<div><div style='width: 175px;display:inline-block;font-weight:bold;color:#8DC73F;'>Detalle: </div><div style='display:inline-block;font-weight:bold;color:#134B8D !important;white-space:pre-line;'>" + Descripcion.Trim().ToUpper() + "</div></div></div>"
                    'FIN - JB - NUEVO - 03/08/2020
                    'INI 1.0
                    If (EspecialidadInterconsulta_ = "MEDICINA INTENSIVA") Then
                        If Trim(oInterconsultaE.Copiara) = "" Then
                            oInterconsultaE.Copiara = "intensivistasadulto@clinicasanfelipe.com;oquinones@clinicasanfelipe.com;mcontardo@clinicasanfelipe.com"
                        Else
                            oInterconsultaE.Copiara = oInterconsultaE.Copiara & ";intensivistasadulto@clinicasanfelipe.com;oquinones@clinicasanfelipe.com;mcontardo@clinicasanfelipe.com"
                        End If
                    End If
                    'FIN 1.0

                    Try
                        If oInterconsultaE.Enviara <> "" Then
                            oInterconsultaLN.Ut_EnviarCorreov3(oInterconsultaE)
                        End If
                    Catch ex As Exception

                    End Try
                End If

            End If
            'FIN - JB - ENVIAR CORREO

            If oRceEvolucionE.CodigoEvolucion <> 0 Then
                'INI 1.4
                ''INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
                'Dim pdf_byte As Byte() = ExportaPDF("DA")
                'Dim cn As New SqlConnection(CnnBD)
                ''Paso 1
                'oHospitalE.TipoDoc = 10
                'oHospitalE.CodAtencion = Session(sCodigoAtencion)
                'oHospitalE.CodUser = Session(sCodUser)
                'oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
                'oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

                ''Paso 2
                'Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
                'cmd1.CommandType = CommandType.Text
                'cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
                'cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
                'cmd1.Parameters.AddWithValue("@flg_reqfirma", "0")

                'Dim num1 As Integer
                'cn.Open()
                'num1 = cmd1.ExecuteNonQuery()
                'cn.Close()

                ''Paso 3
                'oHospitalE.IdeHistoria = Session(sIdeHistoria)
                'oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
                'oHospitalE.TipoDoc = 10
                'oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
                ''FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
                'FIN 1.4
            Else
                Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
            End If

            GuardarLog_("INTERCONSULTA", "Se guardo interconsulta nro " + oInterconsultaE.IdeInterConsulta.ToString())

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaEspecialidadInterconsulta(ByVal CodigoEspecialidad As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaEspecialidadInterconsulta_(CodigoEspecialidad)
    End Function

    Public Function ValidaEspecialidadInterconsulta_(ByVal CodigoEspecialidad As String) As String
        Dim dt As New DataTable()
        oMedicoE.CodMedico = Session(sCodMedico)
        oMedicoE.Atencion = Session(sCodigoAtencion)
        oMedicoE.Orden = 2
        Dim mensaje As String = ""
        Dim especial As Boolean = False
        Try
            dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
            If dt.Rows.Count > 0 Then
                For index = 0 To dt.Rows.Count - 1
                    'If dt.Rows(index)("codespecialidad").ToString().Trim() <> CodigoEspecialidad Then
                    '    mensaje = "Esta interconsulta no corresponde a su especialidad."
                    '    Exit For
                    'End If
                    If dt.Rows(index)("codespecialidad").ToString().Trim() = CodigoEspecialidad Then
                        especial = True
                        Exit For
                    End If
                Next
                If especial <> True Then
                    mensaje = "Esta interconsulta no corresponde a su especialidad."
                End If

            Else
                mensaje = "No se encontro el medico."
            End If

            Return mensaje
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    'VerificarAlertas
    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarAlertas() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarAlertas_()
    End Function

    Public Function VerificarAlertas_() As String
        Dim tabla As New DataTable()
        oTablasE.IdeHistoria = Session(sIdeHistoria)
        oTablasE.Orden = -1
        oTablasE.IdeUsuario = Session(sCodUser)
        oTablasE.IdeAlerta = 0
        tabla = oTablasLN.Sp_RceAlerta(oTablasE)
        Dim Cantidad As String = ""
        Dim STipoUsuario As String = ""
        'If tabla.Rows.Count > 0 Then  'JB - 25/06/2020 - COMENTADO, SE UTILIZARA LAS LINEAS DEBAJO
        '    If CType(tabla.Rows(0)(0).ToString(), Integer) > 0 Then
        '        Cantidad = "ALERTA"
        '    End If
        '    STipoUsuario = tabla.Rows(0)(1).ToString()
        'End If

        'INICIO - JB - 25/06/2020 - SE UTILIZA EN REEMPLAZO DE LAS LINEAS COMENTADAS ARRIBA
        If tabla.Rows(0)("tipo_usuario").ToString().Trim() <> "U" Then
            If tabla.Rows.Count > 0 Then
                If CType(tabla.Rows(0)(0).ToString(), Integer) > 0 Then
                    Cantidad = "ALERTA"
                End If
                STipoUsuario = tabla.Rows(0)("tipo_usuario").ToString()
            End If
        End If
        'FIN - JB - 25/06/2020 - SE UTILIZA EN REEMPLAZO DE LAS LINEAS COMENTADAS ARRIBA
        Return Cantidad + ";" + STipoUsuario.Trim().ToString()
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarAlertas2() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarAlertas2_()
    End Function

    Public Function VerificarAlertas2_() As String
        Dim tabla As New DataTable()
        oTablasE.IdeHistoria = Session(sIdeHistoria)
        oTablasE.Orden = 2
        oTablasE.IdeUsuario = Session(sCodUser)
        oTablasE.IdeAlerta = 0
        tabla = oTablasLN.Sp_RceAlerta(oTablasE)

        Dim MensajeTitle As String = ""
        Dim STipoUsuario As String = ""
        If tabla.Rows.Count > 0 Then
            MensajeTitle = tabla.Rows(0)(0).ToString().Trim()
        End If
        Return MensajeTitle
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarAlertas3() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarAlertas3_()
    End Function

    Public Function VerificarAlertas3_() As String
        Try
            Dim tabla, tabla1 As New DataTable()
            oTablasE.IdeHistoria = Session(sIdeHistoria)  '188833
            oTablasE.IdeUsuario = Session(sCodUser)
            oTablasE.Orden = 1
            oTablasE.IdeAlerta = 3 'LABORATORIO
            Dim CadenaMensajeAlertas As String = ""

            tabla1 = oTablasLN.Sp_RceAlerta(oTablasE)
            If tabla1.Rows.Count > 0 Then
                CadenaMensajeAlertas += "Tiene resultados de Laboratorio pendientes de visualizar"
            End If


            oTablasE.Orden = 1
            oTablasE.IdeAlerta = 2 'IMAGENES
            tabla = oTablasLN.Sp_RceAlerta(oTablasE)
            If tabla.Rows.Count > 0 Then
                If CadenaMensajeAlertas <> "" Then
                    CadenaMensajeAlertas += "</br>Tiene resultados de Imágenes pendientes de visualizar"
                Else
                    CadenaMensajeAlertas += "Tiene resultados de Imágenes pendientes de visualizar"
                End If

            End If

            Return CadenaMensajeAlertas
        Catch ex As Exception

        End Try
    End Function


    ''' <summary>
    ''' FUNCION QUE SE EJECUTA AL CERRAR SESSION
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function CerrarSesion() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CerrarSesion_()
    End Function

    Public Function CerrarSesion_() As String
        Try
            oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
            oRceInicioSesionE.Campo = "fec_fin"
            oRceInicioSesionE.Valor = Format(CDate(Date.Now), "MM/dd/yyyy h:mm:ss")
            oRceInicioSesionLN.Sp_RceInicioSesion_Update(oRceInicioSesionE)

            oRceInicioSesionE.Campo = "flg_estado_sesion"
            oRceInicioSesionE.Valor = "C"
            oRceInicioSesionLN.Sp_RceInicioSesion_Update(oRceInicioSesionE)

            Session.Abandon() 'JB - 15/10/2020
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

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

    ''' <summary>
    ''' FUNCION ADICIONAL PARA OBTENER EL CODIGO DE ATENCION Y ACTUALIZAR LA VARIABLE DE SESSION DE CODIGO DE ATENCION POR EL NUEVO VALOR RECIBIDO
    ''' ESTO SE DARA PARA EL CASO QUE SELECCIONE UNA ATENCION ANTERIOR
    ''' </summary>
    ''' <param name="CodigoAtencion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerCodigoAtencion(ByVal CodigoAtencion As String) As String
        Dim pagina As New InformacionPaciente()
        pagina.Session(sCodigoAtencion) = CodigoAtencion
        Return "OK"
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerValoresAtencionAnterior(ByVal TipoDeAtencion As String, ByVal IdHistoria As String) As String
        Dim pagina As New InformacionPaciente()
        'pagina.Session(sCodigoAtencion) = TipoDeAtencion
        pagina.Session(sIdeHistoria) = IdHistoria

        Dim TipoAtencion As String = pagina.Session(sCodigoAtencion).ToString().Substring(0, 1)
        If TipoDeAtencion.Trim() <> "" Then
            pagina.Session(sTipoAtencion) = TipoDeAtencion
        End If

        Return "OK"
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerPatologia(ByVal Codigo As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ObtenerPatologia_(Codigo)
    End Function

    Public Function ObtenerPatologia_(ByVal Codigo As String) As String
        Try
            Dim tabla As New DataTable()
            oTablasE.IdeHistoria = Session(sIdeHistoria)
            oTablasE.IdeExamen = Codigo
            oTablasE.Orden = 2
            tabla = oTablasLN.Sp_RceResultadoExamenFisico_Consulta(oTablasE)
            'tabla.Columns(1).ColumnName = "nombre"

            If tabla.Rows.Count > 0 Then
                Return tabla.Rows(0)("ide_patologia").ToString().Trim() + ";" + tabla.Rows(0)("dsc_patologia").ToString().Trim()
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function CargaPermiso() As String
        Dim IdeModulo As String = "103"
        Dim pagina As New InformacionPaciente()
        Return pagina.CargaPermiso_(IdeModulo)
    End Function

    Public Function CargaPermiso_(ByVal IdeModulo As String) As String
        Try
            Dim tabla As New DataTable()
            oRceInicioSesionE.CodUser = Session(sCodUser)
            oRceInicioSesionE.IdeOpcionSupe = 0
            oRceInicioSesionE.IdeModulo = IdeModulo
            oRceInicioSesionE.CodOpcion = ""
            oRceInicioSesionE.Orden = 4
            tabla = oRceInicioSesionLN.Sp_RceValidaPermiso(oRceInicioSesionE)
            Dim Permiso As String = ""

            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    Permiso += tabla.Rows(index)("Perfil").ToString().Trim()
                Next
            End If
            Return Permiso
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function




    Public Function ValidaEventoBoton(ByVal Codigo As String) As String
        Try
            Dim tabla As New DataTable()
            oRceInicioSesionE.CodUser = Session(sCodUser)
            oRceInicioSesionE.IdeOpcionSupe = 0
            oRceInicioSesionE.IdeModulo = CodigoFormulario
            oRceInicioSesionE.CodOpcion = ""
            oRceInicioSesionE.Orden = 3
            tabla = oRceInicioSesionLN.Sp_RceValidaPermiso(oRceInicioSesionE)
            Dim Permiso As String = ""

            If tabla.Rows.Count > 0 Then
                For index = 0 To tabla.Rows.Count - 1
                    'Permiso += tabla.Rows(index)("cod_opcion").ToString().Trim() + ";"
                    If tabla.Rows(index)("cod_opcion").ToString().Trim() = Codigo Then
                        Permiso = "OK"
                    End If
                Next
            End If
            Return Permiso
        Catch ex As Exception
            Return "V;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarLog(ByVal Control As String, ByVal Mensaje As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarLog_(Control, Mensaje)
    End Function


    Public Function GuardarLog_(ByVal Control As String, ByVal Mensaje As String) As String
        Try
            oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
            oRceInicioSesionE.CodUser = Session(sCodUser)
            oRceInicioSesionE.Formulario = "InformacionPaciente"
            oRceInicioSesionE.Control = Control.ToUpper()
            oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
            oRceInicioSesionE.DscPcName = Session(sDscPcName)
            oRceInicioSesionE.DscLog = Mensaje
            oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    'INICIO - JB -31/01/2017
    Public Function ExportaPDF(ByVal Tipo As String) As Byte()
        Dim pdf_byte As Byte() = Nothing
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
        System.IO.File.WriteAllBytes(xRuta + "\" + NombreArchivo, pdf_byte)
        System.IO.File.Delete(xRuta + "\" + NombreArchivo) 'eliminando el archivo - jb - 15/07/2020
        crystalreport.Close()
        crystalreport.Dispose()

        Return pdf_byte
    End Function
    'FIN - JB - 31/01/2017



    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAlta() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaAlta_()
    End Function

    Public Function ValidaAlta_() As String
        Dim oHospitalLN1 As New HospitalLN()
        Dim oHospitalE1 As New HospitalE()

        oHospitalE1.NombrePaciente = Session(sCodigoAtencion)
        oHospitalE1.Pabellon = ""
        oHospitalE1.Servicio = ""
        oHospitalE1.Orden = 3
        Dim tabla_atenciones As New DataTable()
        tabla_atenciones = oHospitalLN1.Sp_RceHospital_Consulta(oHospitalE1)


        If tabla_atenciones.Rows.Count > 0 Then
            Return tabla_atenciones.Rows(0)("Flg_alta").ToString().Trim()
        Else
            Return ""
        End If



    End Function

    Public Shared Function ConsultaPacienteHospitalizado() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaPacienteHospitalizado_()
    End Function

    Public Function ConsultaPacienteHospitalizado_() As String
        'SI ES ORDEN 3, SE USARA EL PARAMETRO NombrePaciente PARA ENVIAR EL CODIGO DE ATENCION
        oHospitalE.NombrePaciente = Session(sCodigoAtencion) '21/06/2016
        oHospitalE.Pabellon = ""
        oHospitalE.Servicio = ""
        oHospitalE.Orden = 3
        Dim tabla As New DataTable()
        tabla = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)

        Dim age As Integer = 0
        If tabla.Rows.Count > 0 Then
            age = Today.Year - CType(tabla.Rows(0)("fechanacimiento").ToString().Trim(), Date).Year
            If (CType(tabla.Rows(0)("fechanacimiento").ToString().Trim(), Date) > Today.AddYears(-age)) Then age -= 1
        End If
        Return age.ToString()
    End Function



    Public Sub CargaDatoViaAdministracion()
        Dim oRceMedicamentosE As New RceMedicamentosE()
        Dim oRceMedicamentosLN As New RceMedicamentosLN()
        oRceMedicamentosE.Nombre = ""
        oRceMedicamentosE.Modulo = "ESTANDARVIAADMINISTRACION"
        oRceMedicamentosE.Ordenx = "NOMBRE"
        oRceMedicamentosE.Estado = "NO_X"
        oRceMedicamentosE.Codigo = ""
        Dim dt As New DataTable()
        'INICIO - JB - 14/07/2020 - COMENTADO, YA NO SERA UN COMBO SINO UNA CAJA DE TEXTO
        'dt = oRceMedicamentosLN.Sp_Buscar(oRceMedicamentosE)
        'ddlVia_Con.DataSource = dt
        'ddlVia_Con.DataTextField = "nombre"
        'ddlVia_Con.DataValueField = "codigo"
        'ddlVia_Con.DataBind()
        'FIN - JB - 14/07/2020 - COMENTADO, YA NO SERA UN COMBO SINO UNA CAJA DE TEXTO

        'INICIO - JB - 27/04/2021 - VUELVE A SER UN LISTADO
        oTablasE.CodTabla = "VIAADMINISTRACIONHOSPITAL"
        oTablasE.Buscar = ""
        oTablasE.Key = 0
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = 5
        Dim tabla_ As New DataTable()
        tabla_ = oTablasLN.Sp_Tablas_Consulta(oTablasE)

        Dim FilaVacia As DataRow
        FilaVacia = tabla_.NewRow()
        FilaVacia(0) = "VIAADMINISTRACIONHOSPITAL"
        FilaVacia(1) = ""
        FilaVacia(2) = ""
        FilaVacia(3) = "0"
        FilaVacia(4) = ""
        tabla_.Rows.InsertAt(FilaVacia, 0)
        'DataRow newRow = myDataTable.NewRow();
        'newRow[0] = "0";
        'newRow[1] = "Select one";
        'myDataTable.Rows.InsertAt(newRow, 0);

        ddlVia_Con.DataSource = tabla_
        ddlVia_Con.DataTextField = "nombre"
        ddlVia_Con.DataValueField = "codigo"
        ddlVia_Con.DataBind()
        'FIN - JB - 27/04/2021 - VUELVE A SER UN LISTADO
    End Sub

    'TAB INFUSION - CONTROL CLINICO
    <System.Web.Services.WebMethod()>
    Public Shared Function AgregarInfusion(ByVal DscInfusion As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.AgregarInfusion_(DscInfusion)
    End Function

    Public Function AgregarInfusion_(ByVal DscInfusion As String) As String
        Try
            If Not IsNothing(Session(sTablaInfusiones)) Then
                Dim dt As New DataTable()
                dt = CType(Session(sTablaInfusiones), DataTable)
                Dim existe As Boolean = False

                For index = 0 To (dt.Rows.Count - 1)
                    If dt.Rows(index)("Infusion").ToString().Trim().ToUpper() = DscInfusion.Trim().ToUpper() Then
                        existe = True
                        Exit For
                    End If
                Next
                If existe = False Then
                    dt.Rows.Add((dt.Rows.Count + 1), DscInfusion.Trim().ToUpper().Trim())
                    Session(sTablaInfusiones) = dt
                    Return "OK"
                Else
                    Return "EXISTE"
                End If
            Else
                Dim dt As New DataTable()
                dt.Columns.Add("Item")
                dt.Columns.Add("Infusion")
                dt.Rows.Add("1", DscInfusion.Trim().ToUpper().Trim())
                Session(sTablaInfusiones) = dt
                Return "OK"
            End If

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarInfusiones(ByVal CodigoItem As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EliminarInfusiones_(CodigoItem)
    End Function

    Public Function EliminarInfusiones_(ByVal CodigoItem As String) As String
        Dim tabla As New DataTable()
        Dim item_borrado As String = ""
        If Not IsNothing(Session(sTablaInfusiones)) Then
            tabla = CType(Session(sTablaInfusiones), DataTable)
            For index = 0 To (tabla.Rows.Count - 1)
                If tabla.Rows(index)("Infusion").ToString().Trim().ToUpper() = CodigoItem.Trim().ToUpper() Then
                    tabla.Rows.RemoveAt(index)
                    item_borrado = "SI"
                    Session(sTablaInfusiones) = tabla
                    Exit For
                End If
            Next
        End If
        Return item_borrado
    End Function

    Protected Sub gvListadoInfusiones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            gvListadoInfusiones.PageIndex = e.NewPageIndex
            ListarInfusiones()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ListarInfusiones()
        If Not IsNothing(Session(sTablaInfusiones)) Then
            Dim dt As New DataTable()
            dt = CType(Session(sTablaInfusiones), DataTable)
            Session(sTablaInfusiones) = dt
            gvListadoInfusiones.DataSource = dt
            gvListadoInfusiones.DataBind()
        Else
            Dim dt As New DataTable()
            dt.Columns.Add("Item")
            dt.Columns.Add("Infusion")
            dt.Rows.Add() 'fila falsa
            Session(sTablaInfusiones) = dt
            gvListadoInfusiones.DataSource = dt
            gvListadoInfusiones.DataBind()
        End If
    End Sub

    Public Sub LimpiarInfusiones()
        Dim dt As New DataTable()
        dt.Columns.Add("Item")
        dt.Columns.Add("Infusion")
        dt.Rows.Add() 'fila falsa
        Session(sTablaInfusiones) = dt
        gvListadoInfusiones.DataSource = dt
        gvListadoInfusiones.DataBind()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarInfusion() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarInfusion_() ''sin parametros
    End Function

    Public Function GuardarInfusion_() As String
        Try
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
            Dim dt As New DataTable()
            Dim InsertCab As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
            If InsertCab > 0 Then
                If Not IsNothing(Session(sTablaInfusiones)) Then
                    dt = CType(Session(sTablaInfusiones), DataTable)
                    For index = 0 To dt.Rows.Count - 1
                        oRceRecetaMedicamentoE.CodProducto = ""

                        If dt.Rows(index)("Infusion").ToString().Trim() <> "" Then
                            Dim InsertDet As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                            If InsertDet > 0 Then
                                '{"num_frecuencia", "num_dosis", "txt_detalle", "dsc_producto", "dsc_via"}
                                oRceRecetaMedicamentoE.Campo = "dsc_producto"
                                oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Infusion").ToString().Trim().ToUpper()
                                Dim UpdateDet As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                                If UpdateDet > 0 Then

                                End If
                            End If
                        End If

                    Next
                End If

                oRceRecetaMedicamentoE.Campo = "est_estado"
                oRceRecetaMedicamentoE.ValorNuevo = "A"
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "tipo"
                oRceRecetaMedicamentoE.ValorNuevo = "I"
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
            End If
            LimpiarInfusiones()
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function




    Public Sub LimpiarCamposPatologia()
        'txtDatoClinicoPatologia.Text = ""
        hfCheckPatologiaSeleccionado.Value = ""
        hfIdPatologiaSeleccionado.Value = ""
    End Sub

    Public Sub ListarPatologiasSeleccionadas()
        Dim dt As New DataTable()
        If Not IsNothing(Session(sTablaPatologiasSeleccionados)) Then
            dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)

            'INICIO - JB - actualizando los datos del grid(del cliente) desde el servidor
            If hfOrganosSeleccionados.Value.Trim() <> "" Then
                For index = 0 To dt.Rows.Count - 1
                    If index = hfOrganosSeleccionados.Value.Trim().Split("_")(0) - 1 Then
                        dt.Rows(index)("dsc_muestra2") = hfOrganosSeleccionados.Value.Trim().Split("_")(1).Trim()
                    End If
                Next
            End If
            If hfCantidadSeleccionados.Value.Trim() <> "" Then
                For index = 0 To dt.Rows.Count - 1
                    If index = hfCantidadSeleccionados.Value.Trim().Split("_")(0) - 1 Then
                        dt.Rows(index)("cnt_examen2") = hfCantidadSeleccionados.Value.Trim().Split("_")(1).Trim()
                    End If
                Next
            End If
            hfOrganosSeleccionados.Value = ""
            hfCantidadSeleccionados.Value = ""
            'FIN - JB - actualizando...

            gvPatologia.DataSource = dt
            gvPatologia.DataBind()
        Else
            dt.Columns.Add("cod_prestacion")
            dt.Columns.Add("dsc_prestacion")
            dt.Columns.Add("ide_patologia_mae")
            dt.Columns.Add("cnt_examen")
            dt.Columns.Add("cnt_examen2")
            dt.Columns.Add("cod_patologico")
            dt.Columns.Add("cod_presotor")
            dt.Columns.Add("dsc_muestra")
            dt.Columns.Add("dsc_muestra2")
            dt.Columns.Add("dsc_datoclinico")
            dt.Rows.Add() 'fila falsa
            Session(sTablaPatologiasSeleccionados) = dt
            gvPatologia.DataSource = dt
            gvPatologia.DataBind()
        End If
        'If dt.Rows.Count < 1 Then
        '    imgEnviarPatologia.Enabled = False
        'Else
        '    imgEnviarPatologia.Enabled = True
        'End If
    End Sub


    Protected Sub gvPatologia_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvPatologia.RowDeleting
        Dim dt As New DataTable()
        dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)

        For index = 0 To dt.Rows.Count - 1
            If dt.Rows(index)("ide_patologia_mae") = e.Keys(0).ToString().Trim() Then
                'INICIO - JB - 01/02/2019 - Log de eventos
                Dim oRceInicioSesionE As New RceInicioSesionE()
                Dim oRceInicioSesionLN As New RceInicioSesionLN()
                oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
                oRceInicioSesionE.CodUser = Session(sCodUser)
                oRceInicioSesionE.Formulario = "InformacionPaciente"
                oRceInicioSesionE.Control = "PATOLOGIA"
                oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
                oRceInicioSesionE.DscPcName = Session(sDscPcName)
                oRceInicioSesionE.DscLog = "Se elimino la patologia " + e.Keys(0).ToString().Trim()
                oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
                'FIN - JB - 01/02/2019 - Log de eventos
                dt.Rows(index).Delete()
                Exit For
            End If
        Next

        Session(sTablaPatologiasSeleccionados) = dt
        gvPatologia.DataSource = dt
        gvPatologia.DataBind()


        If dt.Rows.Count = 0 Then
            'imgEnviarPatologia.Enabled = False
        End If

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popupEliminarPatologiaSeleccionado", "RecargaEventoValores();", True)
    End Sub

    'Protected Sub imgEnviarPatologia_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEnviarPatologia.Click

    '    'Call EnviarWebPatologia(1) 'JB - comentado temporalmente 06/11/2018 
    '    'Exit Sub 'JB - comentado temporalmente 06/11/2018

    '    'JB - 25/07/2019 - validar que tenga diagnostico agregado
    '    'If gvDiagnosticos.Rows.Count() > 0 Then
    '    'Else
    '    '    cuMensaje1.Mensaje("Aviso", "Debe ingresar primero un Diagnóstico", cuMensaje.TipoMensaje.No, "", "Aceptar", "CiePetitorio_2_BusquedaA")
    '    '    Exit Sub
    '    'End If


    '    Dim oRcePatologiaCabLN As New RcePatologiaCabLN()
    '    Dim oRcePatologiaCabE As New RcePatologiaCabE()
    '    Dim oRcePatologiaDetLN As New RcePatologiaDetLN()
    '    Dim oRcePatologiaDetE As New RcePatologiaDetE()

    '    oRcePatologiaCabE.CodAtencion = Session(sCodigoAtencion)
    '    oRcePatologiaCabE.IdeHistoria = Session(sIdeHistoria)
    '    oRcePatologiaCabE.CodMedico = Session(sCodMedico)
    '    oRcePatologiaCabE.EstExamen = "A"
    '    oRcePatologiaCabE.Muestra = "" 'txtMuestraPatologia.Text.Trim().ToUpper() JB - se guardara en el det y no en cab
    '    oRcePatologiaCabE.DatosClinico = txtDatoClinicoPatologia.Text.Trim().ToUpper()
    '    Dim dt As New DataTable()
    '    dt = CType(gvPatologia.DataSource, DataTable)


    '    If dt.Rows.Count < 1 Then
    '        'cuMensaje1.Mensaje("Confirmacion", "Debe agregar patologías.", cuMensaje.TipoMensaje.No, "", "Aceptar", "CiePetitorio_2_PatologiaV1")
    '        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Debe agregar patologías.""" + ");", True)
    '        Exit Sub
    '    End If

    '    'JB - ORGANO OBLIGATORIO - 28/06/2019
    '    For indexv = 0 To dt.Rows.Count - 1
    '        If dt.Rows(indexv)("dsc_muestra2").ToString().Trim() = "" Then
    '            'cuMensaje1.Mensaje("Confirmacion", "Debe ingresar organo.", cuMensaje.TipoMensaje.No, "", "Aceptar", "CiePetitorio_2_PatologiaV1")
    '            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Debe ingresar organo.""" + ");", True)
    '            Exit Sub
    '        End If
    '    Next


    '    If TxtFechaUltimaRegla.Text.Trim() <> "" Then
    '        If IsDate(TxtFechaUltimaRegla.Text.Trim()) Then
    '            oRcePatologiaCabE.FecUltimaRegla = TxtFechaUltimaRegla.Text.Trim()
    '        Else
    '            'cuMensaje1.Mensaje("Error", "Formato de fecha incorrecta.", cuMensaje.TipoMensaje.No, "", "Aceptar", "CiePetitorio_2_PatologiaV1")
    '            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Formato de fecha incorrecta.""" + ");", True)
    '            Exit Sub
    '        End If
    '    End If
    '    oRcePatologiaCabE.UsrRegistra = Session(sCodUser)
    '    Dim IdPatologiaCab As Integer = 0
    '    IdPatologiaCab = oRcePatologiaCabLN.Sp_RcePatologiaCab_Insert(oRcePatologiaCabE)


    '    If IdPatologiaCab <> 0 Then 'si grabo la cabecera correctamente traera un ID
    '        oRcePatologiaDetE.IdePatologiaCab = IdPatologiaCab

    '        For index = 0 To dt.Rows.Count - 1
    '            oRcePatologiaDetE.IdePatologiaMae = dt.Rows(index)("ide_patologia_mae").ToString().Trim()
    '            oRcePatologiaDetE.CodPrestacion = dt.Rows(index)("cod_prestacion").ToString().Trim()
    '            oRcePatologiaDetE.Muestra = dt.Rows(index)("dsc_muestra2").ToString().Trim()
    '            oRcePatologiaDetE.DatoClinico = dt.Rows(index)("dsc_datoclinico").ToString().Trim()
    '            oRcePatologiaDetE.CodPatologia = dt.Rows(index)("cod_patologico").ToString().Trim()
    '            oRcePatologiaDetE.CntExamen = dt.Rows(index)("cnt_examen2").ToString().Trim()
    '            oRcePatologiaDetE.CodPresotor = dt.Rows(index)("cod_presotor").ToString().Trim()
    '            oRcePatologiaDetE.FlgEstado = "A"
    '            oRcePatologiaDetLN.Sp_RcePatologiaDet_Insert(oRcePatologiaDetE)

    '            If oRcePatologiaDetE.IdePatologiaDet <> 0 Then
    '                'Call EnviarWebPatologia(oRcePatologiaDetE.IdePatologiaDet) JB - comentado temporalmente
    '            End If
    '        Next
    '        'INICIO - JB - 01/02/2019 - Log de eventos
    '        Dim oRceInicioSesionE As New RceInicioSesionE()
    '        Dim oRceInicioSesionLN As New RceInicioSesionLN()
    '        oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
    '        oRceInicioSesionE.CodUser = Session(sCodUser)
    '        oRceInicioSesionE.Formulario = "CiePetitorio_2"
    '        oRceInicioSesionE.Control = "PATOLOGIA"
    '        oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
    '        oRceInicioSesionE.DscPcName = Session(sDscPcName)
    '        oRceInicioSesionE.DscLog = "Se envio la patologia " + IdPatologiaCab.ToString()
    '        oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
    '        'FIN - JB - 01/02/2019 - Log de eventos
    '    End If

    '    Session.Remove(sTablaPatologiasSeleccionados)
    '    ListarPatologiasSeleccionadas()
    '    CargarPatologias()
    '    LimpiarCamposPatologia()
    '    txtDatoClinicoPatologia.Text = ""
    'End Sub

    'Inicio - Web Services
    Private Sub EnviarWebPatologia(ByVal pIdePatologiaDet As Integer)
        Dim oList As DataTable
        Dim oRcePatologiaDetE As New RcePatologiaDetE()
        Dim oRcePatologiaDetLN As New RcePatologiaDetLN()

        Dim oWsUnilabsPatologia As New WsUnilabsPatologia.SoaServiceSoapClient '.SoaServiceSoapClient
        Dim objSecured As New WsUnilabsPatologia.SecuredTockenwebservice
        Dim objPreAdmision As New WsUnilabsPatologia.ADBE_PreAdmision


        oRcePatologiaDetE.CodAtencion = ""
        oRcePatologiaDetE.IdePatologiaDet = pIdePatologiaDet
        oRcePatologiaDetE.Orden = 4
        oList = oRcePatologiaDetLN.Sp_RcePatologiaDet_ConsultaV1(oRcePatologiaDetE)

        If oList.Rows.Count <> 0 Then
            objSecured.UserName = "20100162742"
            objSecured.Password = "pmMpLQiMkeo"
            objSecured.Sucursal = "CSFJ"

            'objPreAdmision.EmpresaProveedor = oList.Rows(0).Item("EmpresaProveedor")

            'objPreAdmision.FechaAdmision = oList.Rows(0).Item("FechaAdmision")
            'objPreAdmision.HistorialPaciente = oList.Rows(0).Item("HistorialPaciente")
            'objPreAdmision.Procedimiento = oList.Rows(0).Item("Procedimiento")
            'objPreAdmision.Organo = oList.Rows(0).Item("Organo")
            'objPreAdmision.PacienteAPPaterno = oList.Rows(0).Item("PacienteAPPaterno")
            'objPreAdmision.PacienteAPMaterno = oList.Rows(0).Item("PacienteAPMaterno")
            'objPreAdmision.PacienteNombres = oList.Rows(0).Item("PacienteNombre")

            'objPreAdmision.PacienteTelefono = oList.Rows(0).Item("PacienteTelefono")
            'objPreAdmision.Sexo = oList.Rows(0).Item("Sexo")
            'objPreAdmision.Especialidad_Nombre = oList.Rows(0).Item("EspecialidadNombres")
            'objPreAdmision.TipoDocumento = oList.Rows(0).Item("TipoDocumento")
            'objPreAdmision.Documento = oList.Rows(0).Item("Documento")
            'objPreAdmision.PacienteMail = oList.Rows(0).Item("PacienteMail")
            'objPreAdmision.DxPresuntivo = oList.Rows(0).Item("DxPresuntivo")
            'objPreAdmision.CMP = oList.Rows(0).Item("CMP")

            'objPreAdmision.MedicoAPPaterno = oList.Rows(0).Item("MedicoAPPaterno")
            'objPreAdmision.MedicoAPMaterno = oList.Rows(0).Item("MedicoAPMaterno")
            'objPreAdmision.UnidadReplicacion = oList.Rows(0).Item("UnidadReplicacion")
            'objPreAdmision.FechaLimiteAtencion = oList.Rows(0).Item("FechaLimiteAtencion")
            'objPreAdmision.CodigoHC = oList.Rows(0).Item("CodigoHC")
            'objPreAdmision.Numerocama = oList.Rows(0).Item("Numerocama")
            'objPreAdmision.Servicio = oList.Rows(0).Item("Servicio")
            'objPreAdmision.Tarifario = Convert.ToDecimal(oList.Rows(0).Item("Tarifario"))
            'objPreAdmision.CodigoOA = oList.Rows(0).Item("CodigoOA")
            'objPreAdmision.TipoOrden = oList.Rows(0).Item("TipoOrden")

            'objPreAdmision.MedicoNombres = oList.Rows(0).Item("MedicoNombres")
            'objPreAdmision.IdOrdenAtencion = oList.Rows(0).Item("IdOrdenAtencion")
            'objPreAdmision.CantidadSolicitada = oList.Rows(0).Item("CantidadSolicitada")
            'objPreAdmision.TipoAtencion = oList.Rows(0).Item("TipoAtencion")
            'objPreAdmision.Linea = oList.Rows(0).Item("Linea")
            'objPreAdmision.UsuarioCreacion = oList.Rows(0).Item("UsuarioCreacion")
            'objPreAdmision.FechaCreacion = oList.Rows(0).Item("FechaCreacion")
            'objPreAdmision.IpCreacion = oList.Rows(0).Item("IpCreacion")
            'objPreAdmision.Componente = oList.Rows(0).Item("Componente")
            'objPreAdmision.FechaNacimiento = oList.Rows(0).Item("FechaNacimiento")
            'objPreAdmision.ComponenteNombre = oList.Rows(0).Item("ComponenteNombre")

            'objPreAdmision.Empleadora_Nombre = oList.Rows(0).Item("Empleadora_Nombre")
            'objPreAdmision.Empleadora_RUC = oList.Rows(0).Item("Empleadora_RUC")
            'objPreAdmision.Aseguradora_Nombre = oList.Rows(0).Item("Aseguradora_Nombre")
            'objPreAdmision.Aseguradora_RUC = oList.Rows(0).Item("Aseguradora_RUC")
            'objPreAdmision.EmpresaProveedor = oList.Rows(0).Item("EmpresaProveedor")

            objPreAdmision.EmpresaProveedor = "20100162742"
            objPreAdmision.Acceso = "fYpiB/d+oH+R7gq+ePfWNQ=="
            objPreAdmision.Servicio = "PATOLOGIA"
            objPreAdmision.UnidadReplicacion = "CSFJ"
            objPreAdmision.CodigoOA = "0000001329"
            objPreAdmision.Tarifario = 0
            objPreAdmision.Documento = "08068398"
            objPreAdmision.TipoDocumento = "01"
            objPreAdmision.PacienteNombres = "ALFREDO"
            objPreAdmision.PacienteAPMaterno = "LAY"
            objPreAdmision.PacienteAPPaterno = "LIPE"
            objPreAdmision.PacienteMail = ""
            objPreAdmision.PacienteTelefono = "2139422"
            objPreAdmision.Sexo = "M"
            objPreAdmision.HistorialPaciente = "NINGUN EXAMEN"
            objPreAdmision.Aseguradora_Nombre = "NO ASEGURADO"
            objPreAdmision.Aseguradora_RUC = "0"
            objPreAdmision.Componente = "210201"
            objPreAdmision.ComponenteNombre = "BIOPSIA POR UNIDAD TISULAR"
            objPreAdmision.CantidadSolicitada = 1
            objPreAdmision.CMP = "5861"
            objPreAdmision.MedicoAPMaterno = "MAURTUA"
            objPreAdmision.MedicoAPPaterno = "SAFRA"
            objPreAdmision.MedicoNombres = "CHRISTIAN"
            objPreAdmision.Numerocama = ""
            objPreAdmision.TipoAtencion = 1
            objPreAdmision.IdOrdenAtencion = 501
            objPreAdmision.Linea = 1
            objPreAdmision.Organo = "PRÓSTATA"
            objPreAdmision.Empleadora_RUC = ""
            objPreAdmision.Empleadora_Nombre = ""
            objPreAdmision.DxPresuntivo = "DESCARTE"
            objPreAdmision.Especialidad_Nombre = "SIN ESPECIALIDAD"
            objPreAdmision.Procedimiento = "TERCIO MEDIO DERECHO."
            objPreAdmision.TipoOrden = "URG"
            objPreAdmision.CodigoHC = "08068398"
            objPreAdmision.FechaNacimiento = Convert.ToDateTime("1946/05/13")
            objPreAdmision.FechaLimiteAtencion = Convert.ToDateTime("2018/10/30")
        End If

        Dim xRpta As String
        'Enviar IP real del Servidor
        xRpta = oWsUnilabsPatologia.PreAdmisionRegistro(objSecured, objSecured.UserName, "192.168.22.47", objSecured.Sucursal, objPreAdmision)
    End Sub
    'Fin - Web Services

    'Public Sub CargarPatologias()
    '    Dim oRcePatologiaDetE As New RcePatologiaDetE()
    '    Dim oRcePatologiaDetLN As New RcePatologiaDetLN()

    '    Dim dt As New DataTable()
    '    oRcePatologiaDetE.CodAtencion = Session(sCodigoAtencion)
    '    oRcePatologiaDetE.Orden = 1
    '    dt = oRcePatologiaDetLN.Sp_RcePatologiaDet_Consulta(oRcePatologiaDetE)

    '    If dt.Rows.Count > 0 Then
    '        tvPatologia.Nodes.Clear()
    '        Dim xNodoPadre As New TreeNode()
    '        For index = 0 To dt.Rows.Count - 1
    '            Dim xNodoHijo As New TreeNode()

    '            If dt.Rows(index)("ide_patologia_det").ToString() = "0" Then
    '                xNodoPadre = New TreeNode()
    '                'xNodPadre.ImageUrl = "~/Imagenes/Pastilla.png"
    '                xNodoPadre.Text = dt.Rows(index)("ide_patologia_cab").ToString() + " - " + dt.Rows(index)("NombreMedico").ToString() + " | " + dt.Rows(index)("fec_registra").ToString()
    '                xNodoPadre.Value = "0" 'dt.Rows(index)("ide_patologia_cab").ToString()

    '                'INICIO - JB - nuevo codigo para colocar imagenes por estado - 12/06/2019
    '                If dt.Rows(index)("flg_enviarexamen").ToString() = "R" Then
    '                    xNodoPadre.ImageUrl = "~/Imagenes/InformePatologiaV.png"
    '                End If
    '                If dt.Rows(index)("flg_enviarexamen").ToString() = "E" Then
    '                    xNodoPadre.ImageUrl = "~/Imagenes/InformePatologiaA.png"
    '                End If
    '                If dt.Rows(index)("flg_enviarexamen").ToString() = "P" Then
    '                    xNodoPadre.ImageUrl = "~/Imagenes/InformePatologiaR.png"
    '                End If
    '                'cualquiero otro estado se mostrara en gris - 14/06/2019
    '                If dt.Rows(index)("flg_enviarexamen").ToString() <> "P" And dt.Rows(index)("flg_enviarexamen").ToString() <> "R" And dt.Rows(index)("flg_enviarexamen").ToString() <> "E" Then
    '                    xNodoPadre.ImageUrl = "~/Imagenes/InformePatologiaD.png"
    '                End If
    '                'FIN - JB - nuevo codigo para colocar imagenes por estado - 12/06/2019

    '                tvPatologia.Nodes.Add(xNodoPadre)
    '            Else
    '                'INICIO - JB - nuevo codigo para colocar imagenes por estado - 12/06/2019
    '                If dt.Rows(index)("flg_enviarexamen").ToString() = "R" Then
    '                    xNodoHijo.ImageUrl = "~/Imagenes/InformePatologiaV.png"
    '                End If
    '                If dt.Rows(index)("flg_enviarexamen").ToString() = "E" Then
    '                    xNodoHijo.ImageUrl = "~/Imagenes/InformePatologiaA.png"
    '                End If
    '                If dt.Rows(index)("flg_enviarexamen").ToString() = "P" Then
    '                    xNodoHijo.ImageUrl = "~/Imagenes/InformePatologiaR.png"
    '                End If
    '                'cualquiero otro estado se mostrara en gris - 14/06/2019
    '                If dt.Rows(index)("flg_enviarexamen").ToString() <> "P" And dt.Rows(index)("flg_enviarexamen").ToString() <> "R" And dt.Rows(index)("flg_enviarexamen").ToString() <> "E" Then
    '                    xNodoHijo.ImageUrl = "~/Imagenes/InformePatologiaD.png"
    '                End If
    '                'FIN - JB - nuevo codigo para colocar imagenes por estado - 12/06/2019

    '                xNodoHijo.Text = " --- " + dt.Rows(index)("ide_patologia_det").ToString() + " - " + dt.Rows(index)("DscPrestacion").ToString()
    '                xNodoHijo.Value = dt.Rows(index)("ide_patologia_det").ToString()
    '                xNodoPadre.ChildNodes.Add(xNodoHijo)
    '            End If
    '        Next
    '        tvPatologia.ExpandAll()
    '    End If
    '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_LOAD_OCUL();", True)
    'End Sub


    Public Sub CargarPatologiasCheck()
        Dim oRcePatologiaMaeE As New RcePatologiaMaeE()
        Dim oRcePatologiaMaeLN As New RcePatologiaMaeLN()

        oRcePatologiaMaeE.IdeTipoAtencion = "A" 'Session(sCodigoAtencion).Substring(0, 1).Trim()
        oRcePatologiaMaeE.Orden = 4
        Dim tabla1 As New DataTable()
        tabla1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
        Dim ContadorFila As Integer = 0
        Dim CantidadColumna As Integer = 15
        Dim wTabla As Integer = 370 '280 475

        Dim tabla2 As New DataTable()
        oRcePatologiaMaeE.Orden = 5
        tabla2 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)

        Dim CadenaHTML As String = ""
        'CadenaHTML = CadenaHTML + "<div class='JTABS' style='width:100%;'>"

        'creando los TABS
        'CadenaHTML = CadenaHTML + "<input type='radio' id='" + "tabPatologia" + "' name='TabPrincipal' class='JCHEK-TABS' />"
        'CadenaHTML = CadenaHTML + "<label for='" + "tabPatologia" + "' class='JTABS-LABEL'>" + "Patologia" + "</label>"

        'creando div para el contenido de cada tab(1 div con la clase 'JCONTENIDO-TAB' por cada tab)
        CadenaHTML = CadenaHTML + "<div class='JCONTENIDO-TAB' style='font-size:0.8em;display:block;'>"

        If tabla1.Rows.Count > 0 Then
            CadenaHTML = CadenaHTML + "<div><div class='JFILA JDIVCHECK' style='width:1234px;'>"
            CadenaHTML = CadenaHTML + "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA

            For index = 0 To tabla1.Rows.Count - 1
                ContadorFila += 1

                If tabla1.Rows(index)("cod_patologico").ToString().Trim() = "0" Then
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>"
                    CadenaHTML += "<span class='JETIQUETA_5'>" + tabla1.Rows(index)("dsc_prestacion").ToString().Trim() + "</span>"
                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                Else
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>" 'height: 16px;
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>"
                    CadenaHTML += "<input type='checkbox' id='chkPatologia_" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' value='" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' /><label for='chkPatologia_" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' class='JETIQUETA_4'>" + tabla1.Rows(index)("dsc_prestacion").ToString().Trim() + "</label>"
                    CadenaHTML += "</div>"
                    'CadenaHTML += "<div class='JCELDA-1'>"
                    'CadenaHTML += "<input type='text' id='txt" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' class='JNUMERO' style='border: none;border-bottom: 1px solid;border-radius: 0;border-color: #4BACFF;float:right;margin-right:10px;width:50px;' />"
                    'CadenaHTML += "</div>"
                    'CadenaHTML += "<div class='JCELDA-3'>"
                    'CadenaHTML += "<select multiple class='select-patologia' placeholder=''></select>" '<option value='O1'>Opcion 1</option><option value='O2'>Opcion 2</option><option value='O3'>Opcion 3</option>
                    'CadenaHTML += "</div>"
                    CadenaHTML += "</div>"

                    'CadenaHTML += "<input type='checkbox' id='chkPatologia_" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' value='" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' /><label for='chkPatologia_" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' class='JETIQUETA_4'>" + tabla1.Rows(index)("dsc_prestacion").ToString().Trim() + "</label><input type='text' id='txt" + tabla1.Rows(index)("ide_patologia_mae").ToString().Trim() + "' class='JNUMERO' style='border: none;border-bottom: 1px solid;border-radius: 0;border-color: #4BACFF;float:right;margin-right:10px;width:50px;' />" + _
                    '"<select name='state[]' multiple class='demo-default select-state' style='float:right;' placeholder='Seleccione...'><option value='O1'>Opcion 1</option><option value='O2'>Opcion 2</option><option value='O3'>Opcion 3</option></select>"

                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                End If

                If ContadorFila = CantidadColumna Then '25
                    CadenaHTML += "</div>" 'CIERRA COLUMNA
                    CadenaHTML += "<div class='JCELDA-2' style='width:" + wTabla.ToString().Trim + "px;'>" 'ABRE COLUMNA
                    ContadorFila = 0
                End If

                If index = tabla1.Rows.Count - 1 Then 'PARA 'OTROS'
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>"
                    CadenaHTML += "<span class='JETIQUETA_5'>OTROS</span>"
                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"

                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-12' style='padding:0;'>" 'height: 16px;
                    CadenaHTML += "<div class='JFILA'>"
                    CadenaHTML += "<div class='JCELDA-1' style='padding:0;'>"
                    CadenaHTML += "<input type='checkbox' id='chkPatologia_otros' />"
                    CadenaHTML += "</div>"
                    CadenaHTML += "<div class='JCELDA-11' style='padding:0;'>"
                    CadenaHTML += "<select multiple class='demo-default select-otrospatologia' id='ddlOtrosPatologia' style='float:right;' placeholder='Seleccione...'>"
                    For index2 = 0 To tabla2.Rows.Count - 1
                        CadenaHTML += "<option value='" + tabla2.Rows(index2)("ide_patologia_mae").ToString() + "'>" + tabla2.Rows(index2)("dsc_prestacion").ToString() + "</option>"
                    Next

                    CadenaHTML += "</select>"
                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                    CadenaHTML += "</div>"
                End If

                If ContadorFila <> CantidadColumna And index = tabla1.Rows.Count - 1 Then
                    CadenaHTML += "</div>" 'CIERRA COLUMNA
                End If
            Next
            CadenaHTML = CadenaHTML + "</div></div>"
        End If
        CadenaHTML = CadenaHTML.Replace("width:1234", ("width:" + ((Math.Ceiling(tabla1.Rows.Count / CantidadColumna) * wTabla) + 10).ToString()))
        CadenaHTML = CadenaHTML + "</div>" 'cerrando div del contenido de los tab(JCONTENIDO-TAB)
        'CadenaHTML = CadenaHTML + "</div>" 'cerrando div de los tabs(JTABS)

        DivTabs.InnerHtml = CadenaHTML

    End Sub
    Protected Sub btnAgregarPatologia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarPatologia.Click
        'VALIDANDO
        If hfIdPatologiaSeleccionado.Value = "" And hfCheckPatologiaSeleccionado.Value = "" Then
            'cuMensaje1.Mensaje("Aviso", "Seleccionar una patología", cuMensaje.TipoMensaje.No, "", "Aceptar", "CiePetitorio_2_BusquedaI")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Seleccionar una patología.""" + ");", True)
            Exit Sub
        End If

        Dim oRcePatologiaMaeE As New RcePatologiaMaeE()
        Dim oRcePatologiaMaeLN As New RcePatologiaMaeLN()

        Dim CadenaSelectOrgano As String = ""
        Dim tabla As New DataTable()
        oRcePatologiaMaeE.Orden = 1
        tabla = oRcePatologiaMaeLN.Sp_RcePatologiaOrganosMae_Consulta(oRcePatologiaMaeE)
        CadenaSelectOrgano += "<select multiple class='select-patologia' placeholder=''>"
        For index2 = 0 To tabla.Rows.Count - 1
            CadenaSelectOrgano += "<option value='" + tabla.Rows(index2)("dsc_organos").ToString() + "'>" + tabla.Rows(index2)("dsc_organos").ToString() + "</option>"
        Next
        CadenaSelectOrgano += "</select>" '<input type='hidden' class='ORGANO-OCULTO' />

        Dim dt As New DataTable()
        'AGREGANDO A TABLA TEMPORAL
        If IsNothing(Session(sTablaPatologiasSeleccionados)) Then
            'Dim dt As New DataTable()
            dt.Columns.Add("cod_prestacion")
            dt.Columns.Add("dsc_prestacion")
            dt.Columns.Add("ide_patologia_mae")
            dt.Columns.Add("cnt_examen")
            dt.Columns.Add("cnt_examen2")
            dt.Columns.Add("cod_patologico")
            dt.Columns.Add("cod_presotor")
            dt.Columns.Add("dsc_muestra") 'JB - 07/11/2018
            dt.Columns.Add("dsc_muestra2") 'JB - 07/11/2018
            dt.Columns.Add("dsc_datoclinico") 'JB - 07/11/2018

            'Observaciones Cmendez 02/05/2022
            For index = 0 To hfIdPatologiaSeleccionado.Value.Split("|").Length - 1
                If hfIdPatologiaSeleccionado.Value.Split("|")(index).ToString().Trim() <> "" Then
                    oRcePatologiaMaeE.IdePatologiaMae = hfIdPatologiaSeleccionado.Value.Split("|")(index)
                    'Fin
                    oRcePatologiaMaeE.Orden = 3
                    Dim dt1 As New DataTable()
                    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                    'Observaciones Cmendez 02/05/2022
                    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfIdPatologiaSeleccionado.Value.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                End If
            Next
            'Observaciones Cmendez 02/05/2022
            For index = 0 To hfCheckPatologiaSeleccionado.Value.Split("|").Length - 1
                If hfCheckPatologiaSeleccionado.Value.Split("|")(index).ToString().Trim() <> "" Then
                    oRcePatologiaMaeE.IdePatologiaMae = hfCheckPatologiaSeleccionado.Value.Split("|")(index)
                    'Fin
                    oRcePatologiaMaeE.Orden = 3
                    Dim dt1 As New DataTable()
                    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                    'Observaciones Cmendez 02/05/2022
                    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfCheckPatologiaSeleccionado.Value.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                End If
            Next


            Session(sTablaPatologiasSeleccionados) = dt
            gvPatologia.DataSource = dt
            gvPatologia.DataBind()
        Else
            'Dim dt As New DataTable()
            dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)
            Dim dtFila() As DataRow
            'Observaciones Cmendez 02/05/2022
            For index = 0 To hfIdPatologiaSeleccionado.Value.Split("|").Length - 1
                If hfIdPatologiaSeleccionado.Value.Split("|")(index).ToString().Trim() <> "" Then
                    dtFila = dt.Select("ide_patologia_mae = " + hfIdPatologiaSeleccionado.Value.Split("|")(index))
                    oRcePatologiaMaeE.IdePatologiaMae = hfIdPatologiaSeleccionado.Value.Split("|")(index)
                    oRcePatologiaMaeE.Orden = 3
                    Dim dt1 As New DataTable()
                    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                    'Observaciones Cmendez 02/05/2022
                    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfIdPatologiaSeleccionado.Value.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                    'If dtFila.Length > 0 Then
                    '    'ya existe
                    'Else
                    '    oRcePatologiaMaeE.IdePatologiaMae = hfIdPatologiaSeleccionado.Value.Split(",")(index)
                    '    oRcePatologiaMaeE.Orden = 3
                    '    Dim dt1 As New DataTable()
                    '    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                    '    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfIdPatologiaSeleccionado.Value.Split(",")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                    'End If
                End If
            Next
            'Observaciones Cmendez 02/05/2022
            For index = 0 To hfCheckPatologiaSeleccionado.Value.Split("|").Length - 1
                If hfCheckPatologiaSeleccionado.Value.Split("|")(index).ToString().Trim() <> "" Then
                    dtFila = dt.Select("ide_patologia_mae = " + hfCheckPatologiaSeleccionado.Value.Split("|")(index))
                    oRcePatologiaMaeE.IdePatologiaMae = hfCheckPatologiaSeleccionado.Value.Split("|")(index)
                    oRcePatologiaMaeE.Orden = 3
                    Dim dt1 As New DataTable()
                    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                    'Observaciones Cmendez 02/05/2022
                    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfCheckPatologiaSeleccionado.Value.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                    'If dtFila.Length > 0 Then
                    '    'ya existe
                    'Else
                    '    oRcePatologiaMaeE.IdePatologiaMae = hfCheckPatologiaSeleccionado.Value.Split(",")(index)
                    '    oRcePatologiaMaeE.Orden = 3
                    '    Dim dt1 As New DataTable()
                    '    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                    '    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfCheckPatologiaSeleccionado.Value.Split(",")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                    'End If
                End If
            Next
            Session(sTablaPatologiasSeleccionados) = dt
            gvPatologia.DataSource = dt
            gvPatologia.DataBind()
        End If
        'INICIO - JB - 01/02/2019 - Log de eventos
        Dim oRceInicioSesionE As New RceInicioSesionE()
        Dim oRceInicioSesionLN As New RceInicioSesionLN()
        oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
        oRceInicioSesionE.CodUser = Session(sCodUser)
        oRceInicioSesionE.Formulario = "InformacionPaciente"
        oRceInicioSesionE.Control = "PATOLOGIA"
        oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
        oRceInicioSesionE.DscPcName = Session(sDscPcName)
        oRceInicioSesionE.DscLog = "Se agrega patologia " + hfIdPatologiaSeleccionado.Value.ToString() + hfCheckPatologiaSeleccionado.Value.ToString()
        oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
        'FIN - JB - 01/02/2019 - Log de eventos

        'imgEnviarPatologia.Enabled = True 'habilitando control ahora que ya hay data para enviar
        LimpiarCamposPatologia()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LimpiarPatologia", "fn_LimpiarCheckOtrosPatologia();fn_LOAD_OCUL();", True)
    End Sub



    <System.Web.Services.WebMethod()>
    Public Shared Function AgregarPatologia(ByVal IdePatologiaSeleccionado As String, ByVal CheckPatologiaSeleccionado As String, ByVal DatoClinico As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.AgregarPatologia_(IdePatologiaSeleccionado, CheckPatologiaSeleccionado, DatoClinico)
    End Function

    Public Function AgregarPatologia_(ByVal IdePatologiaSeleccionado As String, ByVal CheckPatologiaSeleccionado As String, ByVal DatoClinico As String) As String
        Try
            Dim oRcePatologiaMaeE As New RcePatologiaMaeE()
            Dim oRcePatologiaMaeLN As New RcePatologiaMaeLN()
            Dim ds As New DataSet()

            Dim CadenaSelectOrgano As String = ""
            Dim tabla As New DataTable()
            oRcePatologiaMaeE.Orden = 1
            tabla = oRcePatologiaMaeLN.Sp_RcePatologiaOrganosMae_Consulta(oRcePatologiaMaeE)
            CadenaSelectOrgano += "<select multiple class='select-patologia' placeholder=''>"
            For index2 = 0 To tabla.Rows.Count - 1
                CadenaSelectOrgano += "<option value='" + tabla.Rows(index2)("dsc_organos").ToString() + "'>" + tabla.Rows(index2)("dsc_organos").ToString() + "</option>"
            Next
            CadenaSelectOrgano += "</select>" '<input type='hidden' class='ORGANO-OCULTO' />

            Dim dt As New DataTable()
            'AGREGANDO A TABLA TEMPORAL
            If IsNothing(Session(sTablaPatologiasSeleccionados)) Then
                'Dim dt As New DataTable()
                dt.Columns.Add("cod_prestacion")
                dt.Columns.Add("dsc_prestacion")
                dt.Columns.Add("ide_patologia_mae")
                dt.Columns.Add("cnt_examen")
                dt.Columns.Add("cnt_examen2")
                dt.Columns.Add("cod_patologico")
                dt.Columns.Add("cod_presotor")
                dt.Columns.Add("dsc_muestra") 'JB - 07/11/2018
                dt.Columns.Add("dsc_muestra2") 'JB - 07/11/2018
                dt.Columns.Add("dsc_datoclinico") 'JB - 07/11/2018
                'Observaciones Cmendez 02/05/2022
                For index = 0 To IdePatologiaSeleccionado.Split("|").Length - 1
                    If IdePatologiaSeleccionado.Split("|")(index).ToString().Trim() <> "" Then
                        oRcePatologiaMaeE.IdePatologiaMae = IdePatologiaSeleccionado.Split("|")(index)
                        oRcePatologiaMaeE.Orden = 3
                        Dim dt1 As New DataTable()
                        dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                        'Observaciones Cmendez 02/05/2022
                        dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), IdePatologiaSeleccionado.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", DatoClinico.Trim.ToUpper())
                    End If
                Next
                'Observaciones Cmendez 02/05/2022
                For index = 0 To CheckPatologiaSeleccionado.Split("|").Length - 1
                    If CheckPatologiaSeleccionado.Split("|")(index).ToString().Trim() <> "" Then
                        oRcePatologiaMaeE.IdePatologiaMae = CheckPatologiaSeleccionado.Split("|")(index)
                        oRcePatologiaMaeE.Orden = 3
                        Dim dt1 As New DataTable()
                        dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                        'Observaciones Cmendez 02/05/2022
                        dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), CheckPatologiaSeleccionado.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", DatoClinico.Trim.ToUpper())
                    End If
                Next


                Session(sTablaPatologiasSeleccionados) = dt
                dt.TableName = "TablaPatologia"
                Dim dsx = dt.DataSet
                If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
                    dsx.Tables.Remove(dt.TableName)
                End If
                ds.Tables.Add(dt)
                'gvPatologia.DataSource = dt
                'gvPatologia.DataBind()
            Else
                'Dim dt As New DataTable()
                dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)(0).ToString().Trim() = "" Then
                        dt.Rows.RemoveAt(0)
                    End If
                End If
                Dim dtFila() As DataRow
                'Observaciones Cmendez 02/05/2022
                For index = 0 To IdePatologiaSeleccionado.Split("|").Length - 1
                    If IdePatologiaSeleccionado.Split("|")(index).ToString().Trim() <> "" Then
                        dtFila = dt.Select("ide_patologia_mae = " + IdePatologiaSeleccionado.Split("|")(index))
                        oRcePatologiaMaeE.IdePatologiaMae = IdePatologiaSeleccionado.Split("|")(index)
                        oRcePatologiaMaeE.Orden = 3
                        Dim dt1 As New DataTable()
                        dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                        'Observaciones Cmendez 02/05/2022 
                        dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), IdePatologiaSeleccionado.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", DatoClinico.Trim.ToUpper())
                        'If dtFila.Length > 0 Then
                        '    'ya existe
                        'Else
                        '    oRcePatologiaMaeE.IdePatologiaMae = hfIdPatologiaSeleccionado.Value.Split(",")(index)
                        '    oRcePatologiaMaeE.Orden = 3
                        '    Dim dt1 As New DataTable()
                        '    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                        '    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfIdPatologiaSeleccionado.Value.Split(",")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                        'End If
                    End If
                Next
                'Observaciones Cmendez 02/05/2022
                For index = 0 To CheckPatologiaSeleccionado.Split("|").Length - 1
                    If CheckPatologiaSeleccionado.Split("|")(index).ToString().Trim() <> "" Then
                        dtFila = dt.Select("ide_patologia_mae = " + CheckPatologiaSeleccionado.Split("|")(index))
                        oRcePatologiaMaeE.IdePatologiaMae = CheckPatologiaSeleccionado.Split("|")(index)
                        oRcePatologiaMaeE.Orden = 3
                        Dim dt1 As New DataTable()
                        dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                        'Observaciones Cmendez 02/05/2022
                        dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), CheckPatologiaSeleccionado.Split("|")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", DatoClinico.Trim.ToUpper())
                        'If dtFila.Length > 0 Then
                        '    'ya existe
                        'Else
                        '    oRcePatologiaMaeE.IdePatologiaMae = hfCheckPatologiaSeleccionado.Value.Split(",")(index)
                        '    oRcePatologiaMaeE.Orden = 3
                        '    Dim dt1 As New DataTable()
                        '    dt1 = oRcePatologiaMaeLN.Sp_RcePatologiaMae_Consulta(oRcePatologiaMaeE)
                        '    dt.Rows.Add(dt1.Rows(0)("cod_prestacion").ToString(), dt1.Rows(0)("dsc_prestacion").ToString(), hfCheckPatologiaSeleccionado.Value.Split(",")(index), "<input type='text' class='TEXTO-CANTIDAD JNUMERO' value='1' style='font-size:1em;width:50px;' />", "1", dt1.Rows(0)("cod_patologico").ToString(), "", CadenaSelectOrgano, "", txtDatoClinicoPatologia.Text.Trim.ToUpper())
                        'End If
                    End If
                Next
                Session(sTablaPatologiasSeleccionados) = dt
                dt.TableName = "TablaPatologia"
                Dim dsx = dt.DataSet
                If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
                    dsx.Tables.Remove(dt.TableName)
                End If
                ds.Tables.Add(dt)
                'gvPatologia.DataSource = dt
                'gvPatologia.DataBind()
            End If
            'INICIO - JB - 01/02/2019 - Log de eventos
            Dim oRceInicioSesionE As New RceInicioSesionE()
            Dim oRceInicioSesionLN As New RceInicioSesionLN()
            oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
            oRceInicioSesionE.CodUser = Session(sCodUser)
            oRceInicioSesionE.Formulario = "InformacionPaciente"
            oRceInicioSesionE.Control = "PATOLOGIA"
            oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
            oRceInicioSesionE.DscPcName = Session(sDscPcName)
            oRceInicioSesionE.DscLog = "Se agrega patologia " + IdePatologiaSeleccionado.ToString() + CheckPatologiaSeleccionado.ToString()
            oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
            'FIN - JB - 01/02/2019 - Log de eventos

            'imgEnviarPatologia.Enabled = True 'habilitando control ahora que ya hay data para enviar
            'LimpiarCamposPatologia()
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LimpiarPatologia", "fn_LimpiarCheckOtrosPatologia();fn_LOAD_OCUL();", True)
            Return ds.GetXml()
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    ''' <summary>
    ''' JB - LISTA LAS PATOLOGIAS SELECCINADAS - ACTUALMENTE NO SE USA PERO PODRIA SERVIR POSTERIORMENTE  - 09/01/2020
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ListarPatologiasSeleccionadas1() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ListarPatologiasSeleccionadas1_()
    End Function

    Public Function ListarPatologiasSeleccionadas1_() As String
        Try
            Dim ds As New DataSet()
            Dim dt As New DataTable()
            If Not IsNothing(Session(sTablaPatologiasSeleccionados)) Then
                dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)

                Session(sTablaPatologiasSeleccionados) = dt
                dt.TableName = "TablaPatologia"
                Dim dsx = dt.DataSet
                If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
                    dsx.Tables.Remove(dt.TableName)
                End If
                ds.Tables.Add(dt)

            Else
                dt.Columns.Add("cod_prestacion")
                dt.Columns.Add("dsc_prestacion")
                dt.Columns.Add("ide_patologia_mae")
                dt.Columns.Add("cnt_examen")
                dt.Columns.Add("cnt_examen2")
                dt.Columns.Add("cod_patologico")
                dt.Columns.Add("cod_presotor")
                dt.Columns.Add("dsc_muestra")
                dt.Columns.Add("dsc_muestra2")
                dt.Columns.Add("dsc_datoclinico")
                dt.Rows.Add() 'fila falsa
                Session(sTablaPatologiasSeleccionados) = dt


                Session(sTablaPatologiasSeleccionados) = dt
                dt.TableName = "TablaPatologia"
                Dim dsx = dt.DataSet
                If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
                    dsx.Tables.Remove(dt.TableName)
                End If
                ds.Tables.Add(dt)

            End If


            Return ds.GetXml()
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    Protected Sub btnActualizarGridPatologias_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnActualizarGridPatologias.Click
        ListarPatologiasSeleccionadas()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ActualizarGridPato(ByVal OrganosSeleccionados As String, ByVal CantidadSeleccionada As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ActualizarGridPato_(OrganosSeleccionados, CantidadSeleccionada)
    End Function

    Public Function ActualizarGridPato_(ByVal OrganosSeleccionados As String, ByVal CantidadSeleccionada As String) As String
        Dim dt As New DataTable()
        If Not IsNothing(Session(sTablaPatologiasSeleccionados)) Then
            dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)

            'INICIO - JB - actualizando los datos del grid(del cliente) desde el servidor
            If OrganosSeleccionados.Trim() <> "" Then
                For index = 0 To dt.Rows.Count - 1
                    If index = OrganosSeleccionados.Trim().Split("_")(0) - 1 Then
                        dt.Rows(index)("dsc_muestra2") = OrganosSeleccionados.Trim().Split("_")(1).Trim()
                    End If
                Next
            End If
            If CantidadSeleccionada.Trim() <> "" Then
                For index = 0 To dt.Rows.Count - 1
                    If index = CantidadSeleccionada.Trim().Split("_")(0) - 1 Then
                        dt.Rows(index)("cnt_examen2") = CantidadSeleccionada.Trim().Split("_")(1).Trim()
                    End If
                Next
            End If
        Else
            dt.Columns.Add("cod_prestacion")
            dt.Columns.Add("dsc_prestacion")
            dt.Columns.Add("ide_patologia_mae")
            dt.Columns.Add("cnt_examen")
            dt.Columns.Add("cnt_examen2")
            dt.Columns.Add("cod_patologico")
            dt.Columns.Add("cod_presotor")
            dt.Columns.Add("dsc_muestra")
            dt.Columns.Add("dsc_muestra2")
            dt.Columns.Add("dsc_datoclinico")
            Session(sTablaPatologiasSeleccionados) = dt
            'gvPatologia.DataSource = dt
            'gvPatologia.DataBind()
        End If

        'If dt.Rows.Count < 1 Then
        '    imgEnviarPatologia.Enabled = False
        'Else
        '    imgEnviarPatologia.Enabled = True
        'End If

        Return "OK"
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarPatologia(ByVal IdePatologiaMae As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EliminarPatologia_(IdePatologiaMae)
    End Function

    Public Function EliminarPatologia_(ByVal IdePatologiaMae As String) As String
        Try
            Dim dt As New DataTable()
            dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)

            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("ide_patologia_mae") = IdePatologiaMae Then
                    'INICIO - JB - 01/02/2019 - Log de eventos
                    Dim oRceInicioSesionE As New RceInicioSesionE()
                    Dim oRceInicioSesionLN As New RceInicioSesionLN()
                    oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
                    oRceInicioSesionE.CodUser = Session(sCodUser)
                    oRceInicioSesionE.Formulario = "InformacionPaciente"
                    oRceInicioSesionE.Control = "PATOLOGIA"
                    oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
                    oRceInicioSesionE.DscPcName = Session(sDscPcName)
                    oRceInicioSesionE.DscLog = "Se elimino la patologia " + IdePatologiaMae
                    oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
                    'FIN - JB - 01/02/2019 - Log de eventos
                    dt.Rows(index).Delete()
                    Exit For
                End If
            Next
            Session(sTablaPatologiasSeleccionados) = dt
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function EnviarPatologia(ByVal DatosClinico As String, ByVal FechaUltimaRegla As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EnviarPatologia_(DatosClinico, FechaUltimaRegla)
    End Function


    Public Function EnviarPatologia_(ByVal DatosClinico As String, ByVal FechaUltimaRegla As String) As String
        Try
            'Call EnviarWebPatologia(1) 'JB - comentado temporalmente 06/11/2018 
            'Exit Sub 'JB - comentado temporalmente 06/11/2018

            Dim oRcePatologiaCabLN As New RcePatologiaCabLN()
            Dim oRcePatologiaCabE As New RcePatologiaCabE()
            Dim oRcePatologiaDetLN As New RcePatologiaDetLN()
            Dim oRcePatologiaDetE As New RcePatologiaDetE()

            oRcePatologiaCabE.CodAtencion = Session(sCodigoAtencion)
            oRcePatologiaCabE.IdeHistoria = Session(sIdeHistoria)
            oRcePatologiaCabE.CodMedico = Session(sCodMedico)
            oRcePatologiaCabE.EstExamen = "A"
            oRcePatologiaCabE.Muestra = "" 'txtMuestraPatologia.Text.Trim().ToUpper() JB - se guardara en el det y no en cab
            oRcePatologiaCabE.DatosClinico = DatosClinico
            Dim dt As New DataTable()
            dt = CType(Session(sTablaPatologiasSeleccionados), DataTable)


            If dt.Rows.Count < 1 Then
                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Debe agregar patologías.""" + ");", True)
                Return "ERROR;" + "Debe agregar patologías."
            End If
            For indexv = 0 To dt.Rows.Count - 1
                If dt.Rows(indexv)("dsc_muestra2").ToString().Trim() = "" Then
                    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Debe ingresar organo.""" + ");", True)
                    Return "ERROR;" + "Debe ingresar organo."
                End If
            Next

            If FechaUltimaRegla <> "" Then
                If IsDate(FechaUltimaRegla) Then
                    oRcePatologiaCabE.FecUltimaRegla = FechaUltimaRegla
                Else
                    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Loader", "fn_MESG_POPU(" + """Formato de fecha incorrecta.""" + ");", True)
                    Return "ERROR;" + "Formato de fecha incorrecta."
                End If
            End If
            oRcePatologiaCabE.UsrRegistra = Session(sCodUser)
            Dim IdPatologiaCab As Integer = 0
            IdPatologiaCab = oRcePatologiaCabLN.Sp_RcePatologiaCab_Insert(oRcePatologiaCabE)


            If IdPatologiaCab <> 0 Then 'si grabo la cabecera correctamente traera un ID
                oRcePatologiaDetE.IdePatologiaCab = IdPatologiaCab

                For index = 0 To dt.Rows.Count - 1
                    oRcePatologiaDetE.IdePatologiaMae = dt.Rows(index)("ide_patologia_mae").ToString().Trim()
                    oRcePatologiaDetE.CodPrestacion = dt.Rows(index)("cod_prestacion").ToString().Trim()
                    oRcePatologiaDetE.Muestra = dt.Rows(index)("dsc_muestra2").ToString().Trim()
                    oRcePatologiaDetE.DatoClinico = dt.Rows(index)("dsc_datoclinico").ToString().Trim()
                    oRcePatologiaDetE.CodPatologia = dt.Rows(index)("cod_patologico").ToString().Trim()
                    oRcePatologiaDetE.CntExamen = dt.Rows(index)("cnt_examen2").ToString().Trim()
                    oRcePatologiaDetE.CodPresotor = dt.Rows(index)("cod_presotor").ToString().Trim()
                    oRcePatologiaDetE.FlgEstado = "A"
                    oRcePatologiaDetLN.Sp_RcePatologiaDet_Insert(oRcePatologiaDetE)

                    If oRcePatologiaDetE.IdePatologiaDet <> 0 Then
                        'Call EnviarWebPatologia(oRcePatologiaDetE.IdePatologiaDet) JB - comentado temporalmente
                    End If
                Next
                'INICIO - JB - 01/02/2019 - Log de eventos
                Dim oRceInicioSesionE As New RceInicioSesionE()
                Dim oRceInicioSesionLN As New RceInicioSesionLN()
                oRceInicioSesionE.IdeHistoria = Session(sIdeHistoria)
                oRceInicioSesionE.CodUser = Session(sCodUser)
                oRceInicioSesionE.Formulario = "InformacionPaciente"
                oRceInicioSesionE.Control = "PATOLOGIA"
                oRceInicioSesionE.IdeSesion = Session(sIdeSesion)
                oRceInicioSesionE.DscPcName = Session(sDscPcName)
                oRceInicioSesionE.DscLog = "Se envio la patologia " + IdPatologiaCab.ToString()
                oRceInicioSesionLN.Sp_Rcelogs_Insert(oRceInicioSesionE)
                'FIN - JB - 01/02/2019 - Log de eventos
            End If

            Session.Remove(sTablaPatologiasSeleccionados)
            'ListarPatologiasSeleccionadas() deshabilitar boton
            'CargarPatologias() lista patologias
            'LimpiarCamposPatologia()
            'hfCheckPatologiaSeleccionado.Value = ""
            'hfIdPatologiaSeleccionado.Value = ""

            'txtDatoClinicoPatologia.Text = ""
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function CargaPatologias() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CargaPatologias_()
    End Function
    Public Function CargaPatologias_() As String
        Try
            Dim oRcePatologiaDetE As New RcePatologiaDetE()
            Dim oRcePatologiaDetLN As New RcePatologiaDetLN()
            Dim dt As New DataTable()
            oRcePatologiaDetE.CodAtencion = Session(sCodigoAtencion)
            oRcePatologiaDetE.Orden = 1
            dt = oRcePatologiaDetLN.Sp_RcePatologiaDet_Consulta(oRcePatologiaDetE)
            Dim CadenaPatologia As String = ""

            If dt.Rows.Count > 0 Then
                CadenaPatologia += "<ul class='JTreeView'>" 'A

                Dim xNodoPadre As New TreeNode()
                For index = 0 To dt.Rows.Count - 1
                    Dim xNodoHijo As New TreeNode()
                    If dt.Rows(index)("ide_patologia_det").ToString() = "0" Then
                        Dim CadenaImagenPatologia As String = ""
                        If dt.Rows(index)("flg_enviarexamen").ToString() = "R" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaV.png"
                        End If
                        If dt.Rows(index)("flg_enviarexamen").ToString() = "E" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaA.png"
                        End If
                        If dt.Rows(index)("flg_enviarexamen").ToString() = "P" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaR.png"
                        End If
                        'cualquiero otro estado se mostrara en gris - 14/06/2019
                        If dt.Rows(index)("flg_enviarexamen").ToString() <> "P" And dt.Rows(index)("flg_enviarexamen").ToString() <> "R" And dt.Rows(index)("flg_enviarexamen").ToString() <> "E" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaD.png"
                        End If


                        If index > 0 Then
                            CadenaPatologia += "</ul>" 'C
                            CadenaPatologia += "</li>" 'B
                            CadenaPatologia += "<li>" 'B
                            CadenaPatologia += "<span class='nudo'><img alt='' src='" + CadenaImagenPatologia + "'>" + dt.Rows(index)("ide_patologia_cab").ToString() + " - " + dt.Rows(index)("NombreMedico").ToString() + " | " + dt.Rows(index)("fec_registra").ToString() + "</span>"
                            CadenaPatologia += "<input type='hidden' value='0'>"
                            CadenaPatologia += "<ul class='anidado'>" 'C
                        Else
                            CadenaPatologia += "<li>" 'B
                            CadenaPatologia += "<span class='nudo'><img alt='' src='" + CadenaImagenPatologia + "'>" + dt.Rows(index)("ide_patologia_cab").ToString() + " - " + dt.Rows(index)("NombreMedico").ToString() + " | " + dt.Rows(index)("fec_registra").ToString() + "</span>"
                            CadenaPatologia += "<input type='hidden' value='0'>"
                            CadenaPatologia += "<ul class='anidado'>" 'C
                        End If
                    Else
                        Dim CadenaImagenPatologia As String = ""
                        If dt.Rows(index)("flg_enviarexamen").ToString() = "R" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaV.png"
                        End If
                        If dt.Rows(index)("flg_enviarexamen").ToString() = "E" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaA.png"
                        End If
                        If dt.Rows(index)("flg_enviarexamen").ToString() = "P" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaR.png"
                        End If
                        'cualquiero otro estado se mostrara en gris - 14/06/2019
                        If dt.Rows(index)("flg_enviarexamen").ToString() <> "P" And dt.Rows(index)("flg_enviarexamen").ToString() <> "R" And dt.Rows(index)("flg_enviarexamen").ToString() <> "E" Then
                            CadenaImagenPatologia = "../Imagenes/InformePatologiaD.png"
                        End If
                        CadenaPatologia += "<li class='JTree-Element'>"
                        CadenaPatologia += "<input type='hidden' value='" + dt.Rows(index)("ide_patologia_det").ToString() + "' />"
                        CadenaPatologia += "<img alt = '' src='" + CadenaImagenPatologia + "'>"
                        CadenaPatologia += "<span class='JETIQUETA_TREE0'>" + dt.Rows(index)("ide_patologia_det").ToString() + " - " + dt.Rows(index)("DscPrestacion").ToString() + "</span>"

                        CadenaPatologia += "</li>"
                    End If

                    If index = (dt.Rows.Count - 1) Then 'si llego al ultimo registro...
                        CadenaPatologia += "</ul>" 'C
                        CadenaPatologia += "</li>" 'B
                    End If
                Next

                CadenaPatologia += "</ul>" 'A
            End If

            '<ul Class='JTreeView'>
            '   <li>
            '       <span Class='nudo nudo-down JTREE2-SELECCIONADO'><img alt='' src='../Imagenes/Res_Laboratorio_Rojo.png'> 120959 - LITA | 12:51 PM</span>
            '        <input type = 'hidden' value='120959'>
            '        <ul class='anidado active'>
            '            <li class='JTree-Element'>
            '                <input type='hidden' value='120959'>
            '                <input type='hidden' class='FlgVerificarLab' value=''>
            '                <img alt = '' src='../Imagenes/Res_Laboratorio_Rojo.png'>
            '                <span Class='JETIQUETA_TREE0'>ARSENICO SANGRE ORINA</span>
            '                <span Class='JETIQUETA_TREE2'> </span> 
            '            </li>
            '            <li class='JTree-Element'>
            '                <input type='hidden' value='120959'>
            '                <input type='hidden' class='FlgVerificarLab' value=''>
            '                <img alt = '' src='../Imagenes/Res_Laboratorio_Rojo.png'>
            '                <span class='JETIQUETA_TREE0'>COLESTEROL TOTAL</span>
            '                <span class='JETIQUETA_TREE2'> </span>
            '            </li>
            '        </ul>
            '    </li>
            '</ul>



            Return CadenaPatologia
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function VerInformePatologia(ByVal IdePatologiaDet As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerInformePatologia_(IdePatologiaDet)
    End Function

    Public Function VerInformePatologia_(ByVal IdePatologiaDet As String) As String
        Try
            If IdePatologiaDet <> "0" Then
                Dim oRcePatologiaDetE As New RcePatologiaDetE()
                Dim oRcePatologiaDetLN As New RcePatologiaDetLN()

                Dim dt1 As New DataTable()
                oRcePatologiaDetE.CodAtencion = Session(sCodigoAtencion)
                oRcePatologiaDetE.Orden = 2
                dt1 = oRcePatologiaDetLN.Sp_RcePatologiaDetPresotor_Consulta(oRcePatologiaDetE)

                Dim lista = dt1.AsEnumerable().[Select](Function(x) New With {
                    .cod_presotor = x.Field(Of String)("cod_presotor"),
                    .cod_prestacion = x.Field(Of String)("cod_prestacion"),
                    .dsc_muestra = x.Field(Of String)("dsc_muestra"),
                    .flg_envioexamen = x.Field(Of String)("flg_envioexamen"),
                    Key .ide_documento_res = x.Field(Of Int32?)("ide_documento_res"),
                    .ide_patologia_det = x.Field(Of Int32)("ide_patologia_det"),
                    .ide_patologia_cab = x.Field(Of Int32)("ide_patologia_cab")
                }).Where(Function(s) s.ide_patologia_det.ToString() = IdePatologiaDet).Distinct().ToList() 'AndAlso s.cod_presotor = "X").[Select](Function(h) h.Avg).firsordefault();

                Dim CodigoDocumento As String = ""

                If lista.Count > 0 Then 'si hay mas de un documento para el detalle de patologia
                    For index = 0 To lista.Count - 1
                        If lista.Item(index).ide_documento_res.ToString() <> "" And lista.Item(index).ide_documento_res.ToString() <> "0" Then 'si existe informe query(index).Item("ide_documento_res").ToString().Trim()
                            CodigoDocumento += lista.Item(index).ide_documento_res.ToString() + "_"
                        End If
                    Next
                    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LimpiarPatologia", "fn_AbrirInformePatologia('" + CodigoDocumento + "');", True)
                    Return "OK;" + CodigoDocumento
                Else
                    'cuMensaje1.Mensaje("Informacion", "No hay informe para esta patologia", cuMensaje.TipoMensaje.No, "", "Aceptar", "CuResultadoPetitorio_Imagen")
                    Return "ERROR;" + "No hay informe para esta patologia"
                End If
            Else
                Return "ERROR;" + "Seleccione una patologia para visualizar el informe"
                'cuMensaje1.Mensaje("Informacion", "Seleccione una patologia para visualizar el informe", cuMensaje.TipoMensaje.No, "", "Aceptar", "CuResultadoPetitorio_Imagen")
            End If

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarReporteCC(ByVal IdeReceta As String, FecReceta As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarReporteCC_(IdeReceta, FecReceta)
    End Function

    Public Function VerificarReporteCC_(ByVal IdeReceta As String, FecReceta As String) As String
        Try
            Dim oRceMedicamentosE As New RceMedicamentosE()
            Dim oRceMedicamentosLN As New RceMedicamentosLN()
            Dim tabla As New DataTable()
            oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)

            If IdeReceta <> "0" Then 'si selecciono 'hora' (detalle) en el treeview
                Dim IdRecetaCab As Integer = 0
                Dim Convertir As Boolean
                Convertir = Integer.TryParse(IdeReceta, IdRecetaCab)
                If Convertir = True Then
                    oRceMedicamentosE.IdMedicamentosaCab = IdeReceta
                Else
                    oRceMedicamentosE.IdMedicamentosaCab = 0
                End If
                oRceMedicamentosE.Orden = 1
            Else 'si selecciono 'fecha' en el treeview
                oRceMedicamentosE.FecReceta = FecReceta
                oRceMedicamentosE.Orden = 2
            End If

            tabla = oRceMedicamentosLN.Rp_RceRecetaMedicamento1(oRceMedicamentosE)

            If tabla.Rows.Count > 0 Then
                Return "OK"
            Else
                ''INICIO - JB - OPCIONAL
                'Dim crystalreport As New ReportDocument()
                'If IdeReceta <> "0" Then 'si selecciono 'hora' (detalle) en el treeview
                '    Dim IdRecetaCab As Integer = 0
                '    Dim Convertir As Boolean
                '    Convertir = Integer.TryParse(IdeReceta, IdRecetaCab)
                '    If Convertir = True Then
                '        oRceMedicamentosE.IdMedicamentosaCab = IdeReceta
                '    Else
                '        oRceMedicamentosE.IdMedicamentosaCab = 0
                '    End If
                'Else 'si selecciono 'fecha' en el treeview
                '    oRceMedicamentosE.FecReceta = FecReceta
                'End If
                'oRceMedicamentosE.Orden = 4
                'tabla = oRceMedicamentosLN.Rp_RceRecetaMedicamento1(oRceMedicamentosE)
                'crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpIndicacionMedica.rpt"))
                'crystalreport.SetDataSource(tabla)

                ''PREPERANDO EXPORTACION DE REPORTE A PDF
                'Dim OpcionExportar As ExportOptions
                'Dim OpcionDestino As New DiskFileDestinationOptions()
                'Dim OpcionesFormato As New PdfRtfWordFormatOptions()
                'Dim xNombreArchivo As String = "IM" + Session(sIdeHistoria).ToString() + ".pdf"
                'OpcionDestino.DiskFileName = xRuta + "\" + xNombreArchivo
                'OpcionExportar = crystalreport.ExportOptions
                'With OpcionExportar
                '    .ExportDestinationType = ExportDestinationType.DiskFile
                '    .ExportFormatType = ExportFormatType.PortableDocFormat
                '    .DestinationOptions = OpcionDestino
                '    .FormatOptions = OpcionesFormato
                'End With
                ''EXPORTANDO A PDF
                'crystalreport.Export()
                ''CONVIRTIENDO ARCHIVO PDF GENERADO EN BYTE()
                'Dim pdf_byte As Byte() = System.IO.File.ReadAllBytes(xRuta + "\" + xNombreArchivo)


                'oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
                'oRceMedicamentosE.Detalle = ""
                'oRceMedicamentosE.CodUser = Session(sCodUser)
                'oRceMedicamentosE.TipoDocumento = 2
                'oRceMedicamentosE.Documento = pdf_byte
                'oRceMedicamentosE.Estado = "A"
                'oRceMedicamentosE.Codigo = Session(sCodigoAtencion)
                'oRceMedicamentosE.FecReporte = DateTime.Parse(tabla.Rows(0)("fec_registra2"))
                'oRceMedicamentosLN.Sp_RceResultadoDocumentoDet_InsertV3(oRceMedicamentosE)
                ''FIN - JB - OPCIONAL

                Return ""
            End If

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarEdadNewPew() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerificarEdadNewPew_()
    End Function

    Public Function VerificarEdadNewPew_() As String
        Try
            Dim oHospitalLN_ As New HospitalLN
            Dim oHospitalE_ As New HospitalE

            oHospitalE_.NombrePaciente = Session(sCodigoAtencion) '21/06/2016
            oHospitalE_.Pabellon = ""
            oHospitalE_.Servicio = ""
            oHospitalE_.Orden = 3
            Dim tabla As New DataTable()
            tabla = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE_)

            If tabla.Rows.Count > 0 Then
                Return tabla.Rows(0)("edad").ToString().Trim()
            Else
                Return ""
            End If

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function




    <System.Web.Services.WebMethod()>
    Public Shared Function VolverPantallaAbrirAcordeon(ByVal SeccionAbrir As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VolverPantallaAbrirAcordeon_(SeccionAbrir)
    End Function

    Public Function VolverPantallaAbrirAcordeon_(ByVal SeccionAbrir As String) As String
        Try
            Session("AcordeonAbierto") = SeccionAbrir

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function CargarAlergia1() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CargarAlergia1_()
    End Function

    Public Function CargarAlergia1_() As String
        Try
            Dim oHospitalLN1 As New HospitalLN
            Dim oHospitalE1 As New HospitalE
            oHospitalE1.NombrePaciente = Session(sCodigoAtencion)
            oHospitalE1.Pabellon = ""
            oHospitalE1.Servicio = ""
            oHospitalE1.Orden = 3
            Dim tabla As New DataTable()
            tabla = oHospitalLN1.Sp_RceHospital_Consulta(oHospitalE1)

            If tabla.Rows(0)("Flg_alergia") = True Then
                'spPresentaAlergia.InnerText = "Presenta Alergia"
                'divPresentaAlergia.Visible = True
                Return "OK"
            Else
                Return ""
            End If


        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    'Protected Sub btnCargarPopUpAltaMedicaEP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCargarPopUpAltaMedicaEP.Click
    '    Try
    '        cuAltaMedicaEpicrisis1.ConsultaPacienteHospitalizado()
    '        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "MostrarPopUp", "fn_InvocaPopUpAltaMedica();", True)
    '    Catch ex As Exception

    '    End Try
    'End Sub





    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaPacienteHospitalizadoEP() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaPacienteHospitalizadoEP_()
    End Function

    Public Function ConsultaPacienteHospitalizadoEP_() As String
        Dim oHospitalLN_ As New HospitalLN
        Dim oHospitalE_ As New HospitalE
        Dim ValorRetorno As String = ""

        'SI ES ORDEN 3, SE USARA EL PARAMETRO NombrePaciente PARA ENVIAR EL CODIGO DE ATENCION
        oHospitalE_.NombrePaciente = Session(sCodigoAtencion) '21/06/2016
        oHospitalE_.Pabellon = ""
        oHospitalE_.Servicio = ""
        oHospitalE_.Orden = 3
        Dim tabla As New DataTable()
        tabla = oHospitalLN_.Sp_RceHospital_Consulta(oHospitalE_)


        If tabla.Rows.Count > 0 Then
            Dim dt As New DataTable()
            'USUARIO DIFERENTE A MEDICO
            If Session(sCodMedico).ToString().Trim() = "0" Then
                oMedicoE.CodMedico = Session(sCodUser)
            Else
                oMedicoE.CodMedico = Session(sCodMedico)
            End If
            oMedicoE.Atencion = Session(sCodigoAtencion)
            oMedicoE.Orden = 1
            dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)

            ValorRetorno = tabla.Rows(0)("nombres").ToString().Trim() + ";" + tabla.Rows(0)("FechaIngresoEmergencia").ToString().Trim() + ";" + dt.Rows(0)("nombres").ToString().Trim()
        End If

        Return ValorRetorno
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ListarDiagnosticosAltaMedicaEP() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ListarDiagnosticosAltaMedicaEP_()
    End Function

    Public Function ListarDiagnosticosAltaMedicaEP_() As String
        oRceDiagnosticoE.Tipo = "A"
        oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
        Dim tabla As New DataTable()
        Dim ds As New DataSet()
        tabla = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)

        If tabla.Rows.Count > 0 Then

            For index = 0 To tabla.Rows.Count - 1

                If tabla.Rows(index)("tipo") = "I" Then
                    tabla.Rows(index)("tipo") = "ENTRADA"
                Else
                    tabla.Rows(index)("tipo") = "SALIDA"
                End If

                If tabla.Rows(index)("tipodiagnostico") = "P" Then
                    tabla.Rows(index)("tipodiagnostico") = "P - PRESUNTIVO"
                ElseIf tabla.Rows(index)("tipodiagnostico") = "R" Then
                    tabla.Rows(index)("tipodiagnostico") = "R - REPETIDO"
                Else
                    tabla.Rows(index)("tipodiagnostico") = "D - DEFINITIVO"
                End If

            Next

        End If

        tabla.TableName = "TablaDiagnostico"
        Dim dsx = tabla.DataSet
        If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
            dsx.Tables.Remove(tabla.TableName)
        End If
        ds.Tables.Add(tabla)

        Return ds.GetXml()

    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function CargarAntecedentesAltaMedica() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CargarAntecedentesAltaMedica_()
    End Function

    Public Function CargarAntecedentesAltaMedica_() As String
        Try
            Dim tablaControlesVisibles As New DataTable()
            tablaControlesVisibles.Columns.Add("id")
            tablaControlesVisibles.Columns.Add("valor")
            Dim ds As New DataSet()

            Dim oRceExamenfisicoMaeE_ As New RceAnamnesisE()
            Dim oRceExamenFisicoMaeLN_ As New RceAnamnesisLN()
            oRceExamenfisicoMaeE_.IdeExamenFisico = 110 'para antecedentes
            oRceExamenfisicoMaeE_.IdeHistoria = Session(sIdeHistoria)
            oRceExamenfisicoMaeE_.IdeTipoAtencion = Session(sCodigoAtencion).ToString().Substring(0, 1)
            oRceExamenfisicoMaeE_.CodMedico = Session(sCodMedico)
            oRceExamenfisicoMaeE_.FlgEstado = "A"
            oRceExamenfisicoMaeE_.Orden = 4
            Dim dt, dt2 As New DataTable()
            dt = oRceExamenFisicoMaeLN_.Sp_RceExamenFisicoMae_Consulta5(oRceExamenfisicoMaeE_)
            Dim agregar As Boolean = False

            If dt.Rows.Count > 0 Then
                For index = 0 To dt.Rows.Count - 1
                    oRceExamenfisicoMaeE_.IdeExamenFisico = dt.Rows(index)("ide_examenfisico").ToString()
                    oRceExamenfisicoMaeE_.Orden = 5
                    dt2 = oRceExamenFisicoMaeLN_.Sp_RceExamenFisicoMae_Consulta5(oRceExamenfisicoMaeE_)

                    If dt2.Rows.Count > 0 Then
                        For index2 = 0 To dt2.Rows.Count - 1 'si hay detalles...
                            If dt2.Rows(index2)("txt_detalle").ToString().ToUpper().Contains("SI") And dt2.Rows(index2)("dsc_txtidcampo").ToString().ToUpper().Contains("SI") And
                                dt2.Rows(index2)("valor_control").ToString().Trim() = "1" Then 'si esta marcado el *si*
                                agregar = True 'los valores del detalle se agregaran a la tabla que se devolvera
                            End If
                        Next
                        If agregar = True Then
                            For index2 = 0 To dt2.Rows.Count - 1 'agregando valores a la tabla
                                If dt2.Rows(index2)("est_tipomedida").ToString() <> "I" Then
                                    tablaControlesVisibles.Rows.Add(dt2.Rows(index2)("dsc_txtidcampo").ToString(), dt2.Rows(index2)("valor_control").ToString())
                                End If
                            Next
                        End If
                    End If
                    agregar = False
                Next
            End If



            tablaControlesVisibles.TableName = "TablaAntecedentes"
            Dim dsx = tablaControlesVisibles.DataSet
            If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
                dsx.Tables.Remove(tablaControlesVisibles.TableName)
            End If
            ds.Tables.Add(tablaControlesVisibles)

            Return ds.GetXml()

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarAltaMedicaEpicrisis(ByVal EnfermedadActualAltaMedica As String, ByVal pa152 As String, ByVal talla155 As String, ByVal fc150 As String, ByVal fr151 As String, ByVal peso154 As String,
                                                      ByVal ExamenFisicoAltaMedica As String, ByVal ExamenesAuxiliaresAltaMedica As String, ByVal EvolucionAltaMedica As String, ByVal TratamientoAltaMedica As String,
                                                      ByVal ObservacionesAltaMedica As String, ByVal CondicionAlta As String, ByVal Necropcia As String, ByVal CodigoDestino As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarAltaMedicaEpicrisis_(EnfermedadActualAltaMedica, pa152, talla155, fc150, fr151, peso154,
                                                       ExamenFisicoAltaMedica, ExamenesAuxiliaresAltaMedica, EvolucionAltaMedica, TratamientoAltaMedica,
                                                       ObservacionesAltaMedica, CondicionAlta, Necropcia, CodigoDestino)
    End Function

    Public Function GuardarAltaMedicaEpicrisis_(ByVal EnfermedadActualAltaMedica As String, ByVal pa152 As String, ByVal talla155 As String, ByVal fc150 As String, ByVal fr151 As String, ByVal peso154 As String,
                                                      ByVal ExamenFisicoAltaMedica As String, ByVal ExamenesAuxiliaresAltaMedica As String, ByVal EvolucionAltaMedica As String, ByVal TratamientoAltaMedica As String,
                                                      ByVal ObservacionesAltaMedica As String, ByVal CondicionAlta As String, ByVal Necropcia As String, ByVal CodigoDestino As String) As String
        Try
            Dim oHospitalE_ As New HospitalE()
            Dim oHospitalLN_ As New HospitalLN()

            oHospitalE_.IdeHistoria = Session(sIdeHistoria)
            oHospitalE_.CodAtencion = Session(sCodigoAtencion)
            oHospitalE_.CodUser = Session(sCodUser)
            oHospitalE_.dsc_enfermedad_actual = EnfermedadActualAltaMedica.Trim().ToUpper()
            oHospitalE_.dsc_presionarterial = pa152
            oHospitalE_.dsc_talla = IIf(talla155 = "", 0, talla155)
            oHospitalE_.dsc_frecuenciacardiaca = IIf(fc150 = "", 0, fc150)
            oHospitalE_.dsc_frecuenciarespiratoria = IIf(fr151 = "", 0, fr151)
            oHospitalE_.dsc_peso = IIf(peso154 = "", 0, peso154)
            oHospitalE_.dsc_examenfisico = ExamenFisicoAltaMedica.Trim().ToUpper()
            oHospitalE_.dsc_examenauxiliar = ExamenesAuxiliaresAltaMedica.Trim().ToUpper()
            oHospitalE_.dsc_evolucion = EvolucionAltaMedica.Trim().ToUpper()
            oHospitalE_.dsc_tratamiento = TratamientoAltaMedica.Trim().ToUpper()
            oHospitalE_.dsc_observacion = ObservacionesAltaMedica.Trim().ToUpper()
            oHospitalE_.cod_altamedica = CondicionAlta
            oHospitalE_.flg_necropsia = Necropcia
            Dim ejecucion As Integer
            ejecucion = oHospitalLN_.Sp_RceEpicrisis_Insert(oHospitalE_)

            'inicio - jb - 25/06/2020
            oHospitalE.Campo = "fechaaltamedica"
            oHospitalE.CodAtencion = Session(sCodigoAtencion)
            oHospitalE.ValorNuevo = Format(CDate(Date.Now), "MM/dd/yyyy h:mm:ss")
            oHospitalLN.Sp_Hospital_Update(oHospitalE)

            oHospitalE.Campo = "tipodestino"
            oHospitalE.ValorNuevo = CodigoDestino
            oHospitalLN.Sp_Hospital_Update(oHospitalE)

            oHospitalE.Campo = "usr_altamedica"
            oHospitalE.ValorNuevo = Session(sCodUser)
            oHospitalLN.Sp_Hospital_Update(oHospitalE)

            oHospitalE.Campo = "condicion_alta"
            oHospitalE.ValorNuevo = CondicionAlta
            oHospitalLN.Sp_Hospital_Update(oHospitalE)

            If CondicionAlta.Trim() = "" Then
                oHospitalE.Campo = "flg_necropsia"
                oHospitalE.ValorNuevo = Necropcia
                oHospitalLN.Sp_Hospital_Update(oHospitalE)
            End If

            oHospitalE.IdeHistoria = Session(sIdeHistoria)
            oHospitalE.CodAtencion = Session(sCodigoAtencion)
            oHospitalE.CodPaciente = "0"
            oHospitalE.CodMedico = "0"
            oHospitalE.Campo = "est_atencion"
            oHospitalE.ValorNuevo = "T"
            oHospitalE.CodUser = Session(sCodUser)
            oHospitalLN.Sp_RceHistoriaClinicaCab_Update(oHospitalE)


            oHospitalE.IdeHistoria = Session(sIdeHistoria)
            oHospitalE.CodAtencion = Session(sCodigoAtencion)
            oHospitalE.CodPaciente = "0"
            oHospitalE.CodMedico = "0"
            oHospitalE.Campo = "fin"
            oHospitalE.ValorNuevo = ""
            oHospitalE.CodUser = Session(sCodUser)
            oHospitalLN.Sp_RceHistoriaClinicaCab_Update(oHospitalE)
            'inicio - jb - 25/06/2020


            Try
                oHospitalE_.CodAtencion = Session(sCodigoAtencion)
                oHospitalE_.Orden = 1
                oHospitalLN_.Sp_CorreosAltaMedica(oHospitalE_)
            Catch ex As Exception

            End Try



            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function




    <System.Web.Services.WebMethod()>
    Public Shared Function CargarAltaMedicaEP() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CargarAltaMedicaEP_()
    End Function

    Public Function CargarAltaMedicaEP_() As String
        'Dim tabla As New DataTable()
        'Dim ds As New DataSet()
        'Dim oHospitalE_ As New HospitalE()
        'Dim oHospitalLN_ As New HospitalLN()

        'oHospitalE_.IdeHistoria = Session(sIdeHistoria)
        'oHospitalE_.Orden = 2
        'tabla = oHospitalLN.Rp_RceInformeMedico(oHospitalE_)

        'tabla.TableName = "TablaAltaMedicaEP"
        'Dim dsx = tabla.DataSet
        'If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
        '    dsx.Tables.Remove(tabla.TableName)
        'End If
        'ds.Tables.Add(tabla)

        'Return ds.GetXml()

        Dim tabla As New DataTable()
        Dim ds As New DataSet()
        Dim oHospitalE_ As New HospitalE()
        Dim oHospitalLN_ As New HospitalLN()

        oHospitalE_.IdeHistoria = Session(sIdeHistoria)
        oHospitalE_.CodAtencion = Session(sCodigoAtencion)  'Orden 2
        oHospitalE_.Orden = 1
        tabla = oHospitalLN.Sp_RceEpicrisis_Consulta(oHospitalE_)  'Rp_RceInformeMedico


        If tabla.Rows.Count > 0 Then
            Return tabla.Rows(0)("dsc_enfermedad_actual").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_presionarterial").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_talla").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_frecuenciacardiaca").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_frecuenciarespiratoria").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_peso").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_examenfisico").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_examenauxiliar").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_evolucion").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_tratamiento").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_observacion").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_altamedica").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("flg_necropsia").ToString().Trim().ToUpper()
        Else
            Return ""
        End If


    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerUrlRoe() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ObtenerUrlRoe_()
    End Function

    'INI 1.2
    Public Function ObtenerUrlRoe_() As String
        Try
            'Dim wLink As String = ""
            'Dim CodigoAtencion2 As String = ""
            'Dim oHospitalERoe As New HospitalE()
            'Dim oHospitalLNRoe As New HospitalLN()
            'Dim tabla1, tabla2, tabla3 As New DataTable()
            'oHospitalERoe.CodAtencion = Session(sCodigoAtencion)
            'oHospitalERoe.Orden = 1
            'tabla1 = oHospitalLNRoe.Sp_Hospital_Traslado_Consulta(oHospitalERoe)

            'If tabla1.Rows.Count > 0 Then
            '    CodigoAtencion2 = tabla1.Rows(0)("codatencionorigen")
            'End If

            'If CodigoAtencion2 = "" Then 'SIN TRASLADO
            '    oTablasE.CodTabla = "ROELINK"
            '    oTablasE.Buscar = "01"
            '    oTablasE.Key = 50
            '    oTablasE.NumeroLineas = 1
            '    oTablasE.Orden = -1
            '    tabla2 = oTablasLN.Sp_Tablas_Consulta(oTablasE)

            '    If tabla2.Rows.Count > 0 Then
            '        wLink = Replace(Trim(tabla2.Rows(0)("nombre").ToString().Trim() & ""), "%codatencion%", Session(sCodigoAtencion), 1)
            '        'If Not wAdo.EOF Then
            '        '    wLink = Replace(Trim(wAdo!Nombre & ""), "%codatencion%", wCodAtencion, 1)
            '        '    ' <I.CMEDRANO> 09/05/2019
            '        '    wRuta = cRuta & "browser.exe " & wLink & ",0"
            '        '    Shell(wRuta, vbMaximizedFocus)
            '        '    ' Shell "C:\Archivos de programa\InternetExplorer\iexplore.exe " & wLink
            '        '    ' <F.CMEDRANO> 09/05/2019
            '        'End If
            '    End If
            'Else 'CON TRASLADO
            '    oTablasE.CodTabla = "ROELINK"
            '    oTablasE.Buscar = "02"
            '    oTablasE.Key = 50
            '    oTablasE.NumeroLineas = 1
            '    oTablasE.Orden = -1
            '    tabla3 = oTablasLN.Sp_Tablas_Consulta(oTablasE)

            '    If tabla3.Rows.Count > 0 Then
            '        wLink = Replace(Trim(tabla3.Rows(0)("nombre").ToString().Trim() & ""), "%a1%", Session(sCodigoAtencion), 1)
            '        wLink = Replace(Trim(wLink), "%a2%", CodigoAtencion2, 1)

            '        'If Not wAdo.EOF Then
            '        '    wLink = Replace(Trim(wAdo!Nombre & ""), "%a1%", wCodAtencion, 1)
            '        '    wLink = Replace(Trim(wLink), "%a2%", wCodAtencion2, 1)
            '        '    ' <I.CMEDRANO> 09/05/2019
            '        '    wRuta = cRuta & "browser.exe " & wLink & ",0"
            '        '    Shell(wRuta, vbMaximizedFocus)
            '        '    ' Shell "C:\Archivos de programa\Internet Explorer\iexplore.exe " & wLink
            '        '    ' <F.CMEDRANO> 09/05/2019
            '        'End If
            '    End If

            'End If
            Dim RceImagenes As New RceImagenesE()
            Dim oHospitalLNRoe As New RceImagenLN()

            Dim wLink As String = ""
            Dim wCodPaciente As String = Session(sCodPaciente)
            Dim wDocIdentidad As String = ""
            Dim wEmpresa As String = "7354"
            Dim wTipoDoc As String = "4"
            Dim tabla1, tabla2 As New DataTable()

            RceImagenes.Buscar = wCodPaciente
            RceImagenes.Key = 50
            RceImagenes.NumeroLineas = 1
            RceImagenes.Orden = -1
            RceImagenes.DocIdentidad = ""
            tabla1 = oHospitalLNRoe.Sp_Pacientes_Consulta(RceImagenes)
            If tabla1.Rows.Count > 0 Then
                wDocIdentidad = Trim(tabla1.Rows(0)("DocIdentidad"))
            End If

            oTablasE.Key = 50
            oTablasE.NumeroLineas = 1
            oTablasE.Orden = -1
            oTablasE.CodTabla = "ROELINK"

            oTablasE.Buscar = "03"
            tabla2 = oTablasLN.Sp_Tablas_Consulta(oTablasE)
            wEmpresa = tabla2.Rows(0)("nombre").ToString().Trim() & ""

            oTablasE.Buscar = "04"
            tabla2 = oTablasLN.Sp_Tablas_Consulta(oTablasE)
            wTipoDoc = tabla2.Rows(0)("nombre").ToString().Trim() & ""

            oTablasE.Buscar = "01"
            tabla2 = oTablasLN.Sp_Tablas_Consulta(oTablasE)

            Dim CadenaEncriptada = ObtenerUrlRoeVersion2(wEmpresa, wTipoDoc, wDocIdentidad)

            If tabla2.Rows.Count > 0 Then
                wLink = Replace(Trim(tabla2.Rows(0)("nombre").ToString().Trim() & ""), "%em%", CadenaEncriptada(0), 1)
                wLink = Replace(Trim(wLink), "%td%", CadenaEncriptada(1), 1)
                wLink = Replace(Trim(wLink), "%di%", CadenaEncriptada(2), 1)
            End If

            Return "OK*" + wLink
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function
    'FIN 1.2


    'INICIO - JB - 12/08/2020 - NUEVA SECCION JUNTA MEDICA
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarJuntaMedica(ByVal DscJuntaMedica As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarJuntaMedica_(DscJuntaMedica)
    End Function

    Public Function GuardarJuntaMedica_(ByVal DscJuntaMedica As String) As String
        Try
            Dim oRceJuntaMedicaE As New RceJuntaMedicaE()
            Dim oRceJuntaMedicaLN As New RceJuntaMedicaLN()
            oRceJuntaMedicaE.DscJuntaMedica = DscJuntaMedica.Trim.ToUpper()
            oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
            oRceJuntaMedicaE.CodMedico = Session(sCodMedico)
            oRceJuntaMedicaE.IdUsuario = Session(sCodUser)
            Dim dt As New DataTable()
            Dim exito As Integer = 0
            exito = oRceJuntaMedicaLN.Sp_RceJuntaMedica_Insert(oRceJuntaMedicaE)

            If exito > 0 Then
                GuardarLog_("JUNTA MEDICA", "Se guardo junta medica nro " + exito.ToString())
                GuardarLogEvolucionClinica(exito, 7)
                Return "OK"
            Else
                Return "ERROR;" + "No se puedo registrar los datos"
            End If


        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaJuntaMedica(ByVal Valor As String, ByVal Orden As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaJuntaMedica_(Valor, Orden)
    End Function

    Public Function ConsultaJuntaMedica_(ByVal Valor As String, ByVal Orden As String) As String
        Try
            Dim oRceJuntaMedicaE As New RceJuntaMedicaE()
            Dim oRceJuntaMedicaLN As New RceJuntaMedicaLN()

            If Orden = 1 Then
                oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
            End If
            If Orden = 2 Then
                oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
                oRceJuntaMedicaE.FecJuntaMedica = Valor
            End If
            If Orden = 3 Then
                oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
                oRceJuntaMedicaE.IdeJuntaMedica = Valor
            End If
            oRceJuntaMedicaE.Orden = Orden
            Dim tabla As New DataTable()
            tabla = oRceJuntaMedicaLN.Sp_RceJuntaMedica_Consulta(oRceJuntaMedicaE)
            Dim ValorDevolver As String = ""

            If tabla.Rows.Count > 0 And Orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-FECHA' style='color:#8DC73F;display:inline-block;'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And Orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA' style='white-space:pre-wrap;'>" + tabla.Rows(index1)("IDE_JUNTAMEDICA").ToString().Trim() + " - " + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HOR_REGISTRO").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("IDE_JUNTAMEDICA").ToString().Trim() + "'  />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And Orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div></div><div class='JVALOR-HORA' style='white-space:pre-wrap;'>" + tabla.Rows(index1)("dsc_juntamedica").ToString().Trim() + "</div>"
                    ValorDevolver += "</div>"
                Next
            End If



            'If tabla.Rows.Count > 0 And Orden = 1 Then
            '    For index1 = 0 To tabla.Rows.Count - 1
            '        ValorDevolver += "<div class='JTREE3-FECHA'>"

            '        ValorDevolver += "<div class='JFILA-FECHA'>"
            '        ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-FECHA' style='color:#8DC73F;display:inline-block;'>" + tabla.Rows(index1)("FEC_REGISTRO").ToString() + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
            '        ValorDevolver += "</div>"
            '        ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
            '        ValorDevolver += "</div>"

            '        ValorDevolver += "</div>"
            '    Next
            'End If
            'If tabla.Rows.Count > 0 And Orden = 2 Then
            '    For index1 = 0 To tabla.Rows.Count - 1
            '        ValorDevolver += "<div class='JFILA-HORA'>"
            '        ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA' style='white-space:pre-wrap;'>" + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HOR_REGISTRO").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("IDE_EVOLUCION").ToString().Trim() + "'  />"
            '        ValorDevolver += "</div>"
            '        ValorDevolver += "<div class='JFILA-DETALLE'>"
            '        ValorDevolver += "</div>"
            '    Next
            'End If
            'If tabla.Rows.Count > 0 And Orden = 3 Then
            '    For index1 = 0 To tabla.Rows.Count - 1

            '        ValorDevolver += "<div class='JTREE3-DETALLE'>"
            '        ValorDevolver += "<div></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("txt_resumen").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_evolucion").ToString().Trim() + "'  /> "
            '        ValorDevolver += "</div>"

            '    Next
            'End If




            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    'FIN - JB - 12/08/2020 - NUEVA SECCION JUNTA MEDICA

    'INICIO - JB - 13/08/2020 - NUEVA SECCION PROCEDIMIENTOS MEDICOS
    'GuardarProcedimientoMedico
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarProcedimientoMedico(ByVal Descripcion As String, ByVal CodProcedimiento As String, ByVal NomProcedimiento As String, ByVal Relato As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.GuardarProcedimientoMedico_(Descripcion, CodProcedimiento, NomProcedimiento, Relato)
    End Function

    Public Function GuardarProcedimientoMedico_(ByVal Descripcion As String, ByVal CodProcedimiento As String, ByVal NomProcedimiento As String, ByVal Relato As String) As String
        Try
            Dim oHospitalLN2_ As New HospitalLN()
            Dim oHospitalE2_ As New HospitalE()
            oHospitalE2_.CodAtencion = Session(sCodigoAtencion)
            Dim CodProcedimientoCab As String = ""
            Dim CodProcedimientoDet As String = ""
            CodProcedimientoCab = oHospitalLN2_.Sp_ordenprocedimientosCab_Insert(oHospitalE2_)

            If CodProcedimientoCab.Trim() <> "" Then
                oHospitalE2_.CodMedico = Session(sCodMedico)
                oHospitalE2_.CodigoProcedimientoCab = CodProcedimientoCab.Trim()
                oHospitalE2_.Orden = 2 'JB - 26/10/2020 - SE CAMBIA DE ORDEN 1 A 2
                CodProcedimientoDet = oHospitalLN2_.Sp_ordenprocedimientosDet_Insert(oHospitalE2_)

                'oHospitalE2_.Campo = "codespecialidad" 'JB - 30/11/2020
                'oHospitalE2_.CodigoProcedimientoCab = CodProcedimientoCab.Trim()
                'oHospitalE2_.ValorNuevo = Session(sCodEspecialidad)
                'oHospitalLN2_.Sp_ordenprocedimientosCab_Update(oHospitalE2_)

                oHospitalE2_.Campo = "cpt_generado"
                oHospitalE2_.ValorNuevo = "N"
                oHospitalLN2_.Sp_OrdenprocedimientosCab_Update_(oHospitalE2_)

                oHospitalE2_.Campo = "usr_registra"
                oHospitalE2_.ValorNuevo = Session(sCodUser)
                oHospitalLN2_.Sp_OrdenprocedimientosCab_Update_(oHospitalE2_)
            End If

            If CodProcedimientoDet.Trim() <> "" Then
                oHospitalE2_.CodigoProcedimientoDet = CodProcedimientoDet.Trim()


                oHospitalE2_.Campo = "ide_procedimiento"
                oHospitalE2_.ValorNuevo = CodProcedimiento.Trim().ToUpper()
                oHospitalLN2_.Sp_Ordenprocedimientos_Update(oHospitalE2_)

                'oHospitalE2_.Campo = "nom_procedimiento"
                'oHospitalE2_.ValorNuevo = NomProcedimiento.Trim().ToUpper()
                'oHospitalLN2_.Sp_Ordenprocedimientos_Update(oHospitalE2_)

                oHospitalE2_.Campo = "dsc_procedimiento"
                oHospitalE2_.ValorNuevo = Descripcion.Trim().ToUpper()
                oHospitalLN2_.Sp_Ordenprocedimientos_Update(oHospitalE2_)

                oHospitalE2_.Campo = "flg_medico"
                oHospitalE2_.ValorNuevo = "1"
                oHospitalLN2_.Sp_Ordenprocedimientos_Update(oHospitalE2_)

                'JB - 03/05/2021 - nuevo campo dsc_relato
                oHospitalE2_.Campo = "dsc_relato"
                oHospitalE2_.ValorNuevo = Relato.Trim.ToUpper()
                oHospitalLN2_.Sp_Ordenprocedimientos_Update(oHospitalE2_)


                'JB - 07/10/2021 - nuevo campo usr_registra
                oHospitalE2_.Campo = "usr_registra"
                oHospitalE2_.ValorNuevo = Session(sCodUser)
                oHospitalLN2_.Sp_Ordenprocedimientos_Update(oHospitalE2_)


                'INICIO - JB - 01/02/2019 - Log de eventos
                Dim oRceInicioSesionE_ As New RceInicioSesionE()
                Dim oRceInicioSesionLN_ As New RceInicioSesionLN()
                oRceInicioSesionE_.IdeHistoria = Session(sIdeHistoria)
                oRceInicioSesionE_.CodUser = Session(sCodUser)
                oRceInicioSesionE_.Formulario = "InformacionPaciente"
                oRceInicioSesionE_.Control = "PROCEDIMIENTOS MEDICOS"
                oRceInicioSesionE_.IdeSesion = Session(sIdeSesion)
                oRceInicioSesionE_.DscPcName = ""
                oRceInicioSesionE_.DscLog = "Se guardo el procedimiento medico " + CodProcedimientoCab
                oRceInicioSesionLN_.Sp_Rcelogs_Insert(oRceInicioSesionE_)
                'FIN - JB - 01/02/2019 - Log de eventos
            End If

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaSeccionProcedimiento(ByVal seccion As String, ByVal subseccion As String, ByVal orden As String, ByVal CodCpt As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaSeccionProcedimiento_(seccion, subseccion, orden, CodCpt)
    End Function

    Public Function ConsultaSeccionProcedimiento_(ByVal seccion As String, ByVal subseccion As String, ByVal orden As String, ByVal CodCpt As String) As String
        Dim oHospitalLN2_ As New HospitalLN()
        Dim oHospitalE2_ As New HospitalE()
        Dim ds As New DataSet()

        oHospitalE2_.CodProcedimiento = CodCpt
        oHospitalE2_.CodSeccion = seccion
        oHospitalE2_.CodSubSeccion = subseccion

        Dim dt As New DataTable()

        If orden = 1 Or orden = 2 Then
            oHospitalE2_.Orden = orden
            dt = oHospitalLN2_.Sp_RceSeccioncpt_Consulta(oHospitalE2_)
        Else
            oHospitalE2_.Orden = 2
            dt = oHospitalLN2_.Sp_RceCpt_Consulta(oHospitalE2_)
        End If




        dt.TableName = "TablaProcedimientoMedico"
        Dim dsx = dt.DataSet
        If dsx IsNot Nothing Then
            dsx.Tables.Remove(dt.TableName)
        End If
        ds.Tables.Add(dt)

        Return ds.GetXml()
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaCpt(ByVal CodCpt As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ConsultaCpt_(CodCpt)
    End Function

    Public Function ConsultaCpt_(ByVal CodCpt As String) As String
        Dim oHospitalLN2_ As New HospitalLN()
        Dim oHospitalE2_ As New HospitalE()
        Dim ds As New DataSet()

        oHospitalE2_.CodProcedimiento = CodCpt

        Dim dt As New DataTable()

        oHospitalE2_.Orden = 1
        dt = oHospitalLN2_.Sp_RceCpt_Consulta(oHospitalE2_)




        dt.TableName = "TablaProcedimientoMedicoConsulta"
        Dim dsx = dt.DataSet
        If dsx IsNot Nothing Then
            dsx.Tables.Remove(dt.TableName)
        End If
        ds.Tables.Add(dt)

        Return ds.GetXml()
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function InsertarFavoritoCpt(ByVal CodCpt As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.InsertarFavoritoCpt_(CodCpt)
    End Function

    Public Function InsertarFavoritoCpt_(ByVal CodCpt As String) As String
        Try
            Dim oHospitalLN2_ As New HospitalLN()
            Dim oHospitalE2_ As New HospitalE()
            Dim ds As New DataSet()

            oHospitalE2_.CodProcedimiento = CodCpt
            oHospitalE2_.CodMedico = Session(sCodMedico)
            oHospitalE2_.TipoAtencion = Session(sCodigoAtencion).ToString().Substring(0, 1)

            Dim dt As New DataTable()

            oHospitalLN2_.Sp_RceCptFavorito_Insert(oHospitalE2_)

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminaFavoritoCpt(ByVal CodCpt As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.EliminaFavoritoCpt_(CodCpt)
    End Function

    Public Function EliminaFavoritoCpt_(ByVal CodCpt As String) As String
        Try
            Dim oHospitalLN2_ As New HospitalLN()
            Dim oHospitalE2_ As New HospitalE()
            Dim ds As New DataSet()

            oHospitalE2_.CodProcedimiento = CodCpt
            oHospitalE2_.CodMedico = Session(sCodMedico)
            oHospitalE2_.TipoAtencion = Session(sCodigoAtencion).ToString().Substring(0, 1)

            Dim dt As New DataTable()

            oHospitalLN2_.Sp_RceCptFavorito_Delete(oHospitalE2_)

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewProcedimientoMedico(ByVal orden As String, ByVal fec_orden As String, ByVal ide_orden As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.TreeViewProcedimientoMedico_(orden, fec_orden, ide_orden)
    End Function

    Public Function TreeViewProcedimientoMedico_(ByVal orden As String, ByVal fec_orden As String, ByVal ide_orden As String) As String
        Try
            Dim oHospitalLN2_ As New HospitalLN()
            Dim oHospitalE2_ As New HospitalE()
            oHospitalE2_.CodAtencion = Session(sCodigoAtencion)
            oHospitalE2_.IdeOrden = ide_orden
            oHospitalE2_.FecOrden = fec_orden
            oHospitalE2_.HorOrden = ""
            oHospitalE2_.Orden = orden
            Dim ValorDevolver As String = ""
            Dim tabla As New DataTable()
            tabla = oHospitalLN2_.Sp_RceProcedimientos_ConsultaV2(oHospitalE2_)
            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JTREE3-FECHA'>"
                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecOrdenProcedimiento' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " - " + tabla.Rows(index1)("ESPECIALIDAD").ToString().Trim() + " | " + tabla.Rows(index1)("HORA_RECETA").ToString().Trim() + "</div><input type='hidden' class='IdeOrden' value='" + tabla.Rows(index1)("ide_orden").ToString().Trim() + "' />"  'tabla.Rows(index1)("ide_orden").ToString().Trim() + " - " 
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div class='JVALOR-HORA' style='margin-top: 5px;'>" + tabla.Rows(index1)("ide_procedimiento").ToString().Trim() + " - " + tabla.Rows(index1)("COD_PRESTACION").ToString().Trim() + " - <span style='font-weight:normal;'>" + tabla.Rows(index1)("dsc_procedimiento").ToString().Trim() + "</span></div><div style='font-weight: normal;margin-top: 5px;'>" + "" + "</div>" + " <input type='hidden' class='IdeOrdenDet' value='" + tabla.Rows(index1)("ide_ordendet").ToString().Trim() + "' /> "
                    ValorDevolver += "</div>"

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div class='JVALOR-HORA' style='margin-top: 5px;'>" + tabla.Rows(index1)("dsc_relato").ToString().Trim() + "</div>"
                    ValorDevolver += "</div>"

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function


    'FIN - JB - 13/08/2020 - NUEVA SECCION PROCEDIMIENTOS MEDICOS


    Function GuardarLogEvolucionClinica(ByVal IdeRecetaP As Integer, ByVal Orden As Integer) As String
        oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
        oRceEvolucionE.IdeOrdenCab = IdeRecetaP
        oRceEvolucionE.Orden = Orden
        oRceEvolucionLN.Sp_RceEvolucionLog_Insert(oRceEvolucionE)
        If oRceEvolucionE.CodigoEvolucion <> 0 Then
            'INI 1.4
            ''INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'Dim pdf_byte As Byte() = ExportaPDF("DA")
            'Dim cn As New SqlConnection(CnnBD)
            ''Paso 1
            'oHospitalE.TipoDoc = 10
            'oHospitalE.CodAtencion = Session(sCodigoAtencion)
            'oHospitalE.CodUser = Session(sCodUser)
            'oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
            'oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            ''Paso 2
            'Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
            'cmd1.CommandType = CommandType.Text
            'cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
            'cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            'cmd1.Parameters.AddWithValue("@flg_reqfirma", "0")

            'Dim num1 As Integer
            'cn.Open()
            'num1 = cmd1.ExecuteNonQuery()
            'cn.Close()

            ''Paso 3
            'oHospitalE.IdeHistoria = Session(sIdeHistoria)
            'oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
            'oHospitalE.TipoDoc = 10
            'oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
            ''FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            'FIN 1.4
        Else
            Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
        End If
        Return "OK"
    End Function

    Function GuardarLogEvolucionClinicaV2(ByVal CodDiagnostico As String, ByVal Orden As Integer) As String
        oRceEvolucionE.IdHistoria = Session(sIdeHistoria)
        oRceEvolucionE.CodDiagnostico = CodDiagnostico
        oRceEvolucionE.Orden = Orden
        oRceEvolucionE.CodMedico = Session(sCodMedico)
        oRceEvolucionLN.Sp_RceEvolucionLog_InsertV2(oRceEvolucionE)

        If oRceEvolucionE.CodigoEvolucion <> 0 Then
            '1.4 INI
            '    'INICIO - JB - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            '    Dim pdf_byte As Byte() = ExportaPDF("DA")
            '    Dim cn As New SqlConnection(CnnBD)
            '    'Paso 1
            '    oHospitalE.TipoDoc = 10
            '    oHospitalE.CodAtencion = Session(sCodigoAtencion)
            '    oHospitalE.CodUser = Session(sCodUser)
            '    oHospitalE.Descripcion = oRceEvolucionE.CodigoEvolucion.ToString()
            '    oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            '    'Paso 2
            '    Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento,flg_reqfirma=@flg_reqfirma, extension_doc='PDF',flg_firma=NULL, fec_firma=NULL, usr_firma=NULL  where id_documento=@id_documento", cn)
            '    cmd1.CommandType = CommandType.Text
            '    cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte)
            '    cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            '    cmd1.Parameters.AddWithValue("@flg_reqfirma", "0")

            '    Dim num1 As Integer
            '    cn.Open()
            '    num1 = cmd1.ExecuteNonQuery()
            '    cn.Close()

            '    'Paso 3
            '    oHospitalE.IdeHistoria = Session(sIdeHistoria)
            '    oHospitalE.IdeGeneral = oRceEvolucionE.CodigoEvolucion
            '    oHospitalE.TipoDoc = 10
            '    oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)
            '    'FIN - NUEVO CODIGO - 07/02/2020 - PARA EVOLUCION CLINICA
            '1.4 FIN
        Else
            Return ConfigurationManager.AppSettings(sMensajeGuardarError) + " - Sp_RceEvolucionLog_Insert"
        End If
        Return "OK"
    End Function





    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarVentanaMultiple() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidarVentanaMultiple_()
    End Function

    Public Function ValidarVentanaMultiple_() As String

        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            'IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera))
            Return "Se abrio el sistema en otra ventana"
        Else
            Return ""
        End If

    End Function



    'NUEVA PESTAÑA EN SECCION CONTROL CLINICA - CALCULADORA
    <System.Web.Services.WebMethod()>
    Public Shared Function CalcularDosis(ByVal PesoCalculadora As String, ByVal FarmacoCalculadora As String, ByVal VelocidadInfusionCalculadora As String, ByVal Tipo As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.CalcularDosis_(PesoCalculadora, FarmacoCalculadora, VelocidadInfusionCalculadora, Tipo)
    End Function
    Public Function CalcularDosis_(ByVal PesoCalculadora As String, ByVal FarmacoCalculadora As String, ByVal VelocidadInfusionCalculadora As String, ByVal Tipo As String) As String

        Try
            Dim Peso, VelocidadInfusion As Decimal
            VelocidadInfusion = VelocidadInfusionCalculadora
            Peso = PesoCalculadora
            Dim oRceRecetaMedicamentoE1 As New RceRecetaMedicamentoE()
            Dim oRceRecetaMedicamentoLN1 As New RceRecetaMedicamentoLN()
            oRceRecetaMedicamentoE1.Orden = 2
            oRceRecetaMedicamentoE1.IdFarmaco = FarmacoCalculadora
            Dim TablaFarmaco As New DataTable()
            TablaFarmaco = oRceRecetaMedicamentoLN1.Sp_RceFarmacoDosis_Consulta(oRceRecetaMedicamentoE1)

            Dim CamposHTML As String = ""

            If TablaFarmaco.Rows.Count > 0 Then
                Dim DisolucionEstandar As Decimal
                DisolucionEstandar = TablaFarmaco.Rows(0)("disolucion_standar").ToString()
                Dim Cantidad As Integer
                Cantidad = TablaFarmaco.Rows(0)("cantidad").ToString()

                Dim CampoDosisPorKgHora As Decimal
                CampoDosisPorKgHora = (DisolucionEstandar * VelocidadInfusion) / Peso
                Dim CampoDosisPorDia As Decimal
                CampoDosisPorDia = CampoDosisPorKgHora * Peso * 24
                Dim CampoPrescripcion As Decimal
                CampoPrescripcion = CampoDosisPorDia / Cantidad
                Dim CampoSemaforo As String

                If CampoDosisPorDia > (TablaFarmaco.Rows(0)("dosis_maxima").ToString() * Peso) Then
                    CampoSemaforo = "<span class='JETIQUETA_2' style='color:red'>DOSIS EXCESIVA</span>"
                Else
                    CampoSemaforo = "<span class='JETIQUETA_2'>DOSIS CORRECTA</span>"
                End If



                CamposHTML += "<div class='JFILA JFILA-ESTILO-ARRIBACERO'>"


                CamposHTML += "<div class='JCELDA-1 JCELDA-ESTILO-DERECHA'>"
                CamposHTML += "<span class='JETIQUETA_2'></span>"
                CamposHTML += "</div>"
                CamposHTML += "<div class='JCELDA-2 JCELDA-ESTILO-DERECHA'>"
                CamposHTML += "<span class='JETIQUETA_2'>" + TablaFarmaco.Rows(0)("dsc_dci").ToString() + "</span>"
                CamposHTML += "</div>"
                CamposHTML += "<div class='JCELDA-2 JCELDA-ESTILO-DERECHA'>"
                CamposHTML += "<span class='JETIQUETA_2'>" + (Math.Round(CampoDosisPorKgHora, 3)).ToString() + " </span>"
                CamposHTML += "<span class='JETIQUETA_2'>" + TablaFarmaco.Rows(0)("unidad_medida").ToString() + "</span>"
                CamposHTML += "</div>"

                CamposHTML += "<div class='JCELDA-2 JCELDA-ESTILO-DERECHA'>"
                CamposHTML += "<span class='JETIQUETA_2'>" + (Math.Round(CampoDosisPorDia, 3)).ToString() + " </span>"
                CamposHTML += "<span class='JETIQUETA_2'>" + TablaFarmaco.Rows(0)("unidad_medida").ToString() + "</span>"
                CamposHTML += "</div>"
                CamposHTML += "<div class='JCELDA-2 JCELDA-ESTILO-DERECHA'>"
                CamposHTML += "<span class='JETIQUETA_2'>" + (Math.Round(CampoPrescripcion, 3)).ToString() + " </span>"
                CamposHTML += "<span class='JETIQUETA_2'>" + "AMPOLLAS" + "</span>"
                CamposHTML += "</div>"
                CamposHTML += "<div class='JCELDA-2 JCELDA-ESTILO-DERECHA'>"
                CamposHTML += CampoSemaforo
                CamposHTML += "</div>"
                CamposHTML += "<div class='JCOL-OCULTA'>"
                CamposHTML += "<span class='JETIQUETA_2'>" + (Math.Round(Peso, 1)).ToString() + "</span>"
                CamposHTML += "</div>"
                CamposHTML += "<div class='JCELDA-1'>"
                CamposHTML += "<img alt='' src='../Imagenes/anular.gif' class='JIMG-GENERAL JELIMINAR-CALCULO' style='height:14px;'>"
                CamposHTML += "</div>"


                CamposHTML += "</div>"


            End If

            Return CamposHTML
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function

    'NUEVA PESTAÑA DE ESCALA E INTERVENCIONES ENFERMERIA
    Protected Sub IDTabDatosclinico_ItemDataBound(sender As Object, e As ListViewItemEventArgs)
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Try
                Dim _Id As Label = (e.Item.FindControl("lblidtab"))
                Dim _Key As Integer = Convert.ToInt32(_Id.Text)
                ListIndicacionesdetalle = New List(Of IndicacionesMedicasE)
                ListIndicacionesdetalle = (From n In ListGeneralIndicacionesMedicas Where n.IdTipo = _Key Select n).ToList
                Dim _listView As ListView = (e.Item.FindControl("lvdetalletabclinico"))
                _listView.DataSource = ListIndicacionesdetalle
                _listView.DataBind()
            Catch ex As Exception
                Dim menssaje As String = ex.Message
            End Try
        End If
    End Sub

    Protected Sub lvEscalaeIntervencionedetalle_ItemDataBound(sender As Object, e As ListViewItemEventArgs)
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Try
                Dim _Id As Label = (e.Item.FindControl("lblIdEscalas"))
                Dim _Key As Integer = Convert.ToInt32(_Id.Text)
                Dim _listView As ListView = (e.Item.FindControl("lvdetalleEscalaEIndicaciones"))
                Select Case _Key
                    Case 1
                        tablaEscalaGlascow(_listView)
                    Case 2
                        tablaEscalaDolor(_listView)
                    Case 3
                        tablaEscalaMaDDOX(_listView)
                    Case 4
                        tablaEscalaBraden(_listView)
                    Case 5
                        tablaEscalaRiegoCaida(_listView)
                End Select

            Catch ex As Exception
                Dim menssaje As String = ex.Message
            End Try
        End If
    End Sub

    Private Sub tablaEscalaGlascow(_ListView As ListView)
        Try
            Dim _escalalGLASGOW As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 1 Group n By n.itemcab, n.detalleitem Into Group Select New EscalaEIndicacionesE With {.itemcab = itemcab, .detalleitem = detalleitem}).ToList()
            Dim AperturaHtmlInicio As String = "<asp:Label runat='server' ID='lbldetalleIdEscalas' Visible='false' Text='1'></asp:Label>  <table class='JSBTABLA JSTABLASCALA' id='tablaescala1'> <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _TrHtml As String = ""
            Dim _TrHtml2 As String = ""
            Dim _div As String = "<div style='text-align: center;height: 80px;padding-top: 21px;background: white;'> <label style='font-size: 20px;font-weight: bold;'>Puntaje total: </label><input readonly value='0' style='font-size: 25px;margin-left: 10px;border: 0px;' type='text' id='divtotalglascow'/> </div>"

            Dim _button As String = "<div class='JCELDA-12' id='btnbuttonescala1'><input type='button' class='buttonescalaseindicaciones' value='Guardar' id='btnGuardarGlacow' onclick='btnGuardarGlascowClick()' style='font-size:18px;'></div>"

            Dim _descripcionesGlasgow As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 1 Select n).ToList()

            For Each _item As EscalaEIndicacionesE In _escalalGLASGOW
                _TrHtml2 = "<tr><td style='text-align: center;'><label style='font-size: 18px;font-weight: bold;'>" + _item.detalleitem + "</label></td> <td style='padding: 0px;'>"
                Dim _HtmlInicio As String = "<table class='JSBTABLA' style = 'border: 0px;'> <tbody><tr style = 'border: 0px;'>"
                Dim _htmlFin As String = "</tr></tbody></table>"
                Dim _td As String = ""
                Dim _IdItemCab As String = ""
                For Each _det As EscalaEIndicacionesE In (From n In _descripcionesGlasgow Where n.itemcab = _item.itemcab Select n).ToList()
                    _td = _td + "<td style='border: 0px solid #8DC73F;'> <table class='JSBTABLA' style='border: 0px solid #8DC73F;'> <tbody> <tr style='border: 0px solid #8DC73F; border-bottom: 1px solid #8DC73F;height: 45px;'><td style='border: 0px solid #8DC73F; text-align: center;'> <input runat='server' id='checkGlascow_1_" + _det.itemcab.ToString() + "_" + _det.puntuacion.ToString() + "'   onchange='clickGlascow(this)' style='width: 25px;height: 25px;' type='checkbox' value= '1_" + _det.itemcab.ToString() + "_" + _det.puntuacion.ToString() + "'/>  </td> </tr> <tr style='border: 0px solid #8DC73F;'><td style='text-align: center; border: 0px solid #8DC73F;'>" + "<img style='width: 100px;height: 100px;' src='../Imagenes/Escala/" + _det.imagendet + "'/>" + " </br><label style='font-size: 18px;font-weight: bold;'>" + _det.descripcion_det.ToString() + "</label></br><label style = 'font-size: 14px;'>" + _det.descripcion_det2.ToString() + "</label></br><label>Puntos: " + _det.puntuacion.ToString() + "</label></td></tr></tbody></table></td>"
                    _IdItemCab = _det.itemcab
                Next
                _TrHtml = _TrHtml + _TrHtml2 + _HtmlInicio + _td + _htmlFin + "</td><td style='width: 65px; text-align: center;'><input type='text' value='0' readonly runat='server' id='lblfilatotal_" + _IdItemCab + "' style='font-size: 18px;width: 60px; text-align: center; font-weight: bold; border: 0px;'/> </td></tr>"
            Next

            Dim HtmlFinal As String = AperturaHtmlInicio + _TrHtml + FinalizacionHtml + _div + _button


            Dim _ListString As List(Of EscalaEIndicacionesE) = New List(Of EscalaEIndicacionesE)
            Dim _objstring As EscalaEIndicacionesE = New EscalaEIndicacionesE()
            _objstring.descripcion_det = HtmlFinal
            _ListString.Add(_objstring)

            _ListView.DataSource = _ListString
            _ListView.DataBind()



        Catch ex As Exception
            Dim menssaje As String = ex.Message
        End Try
    End Sub

    Private Sub tablaEscalaDolor(_ListView As ListView)
        Try
            Dim _escalaDOlor As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 2 Select n).ToList()

            Dim _escaladolorcabecera As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 2 Group n By n.i_order, n.descripcion_det Into Group Select New EscalaEIndicacionesE With {.i_order = i_order, .descripcion_det = descripcion_det}).ToList()
            Dim _escalaDolorImagen As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 2 Group n By n.itemcab, n.imagencab, n.descripcion_det2, n.i_order Into Group Select New EscalaEIndicacionesE With {.itemcab = itemcab, .imagencab = imagencab, .descripcion_det2 = descripcion_det2, .i_order = i_order}).ToList()

            Dim AperturaHtmlInicio As String = " <asp:Label runat='server' ID='lbldetalleIdEscalas' Visible='false' Text='2'></asp:Label> <table class='JSBTABLA JSTABLASCALA' style = 'text-align: center;' id='tablaescala2'>  <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _tr As String = ""
            Dim _div As String = "<div style='text-align: center;height: 80px;padding-top: 21px;background: white;'> <label style='font-size: 20px;font-weight: bold;'>Puntaje total: </label><input readonly value='' style='font-size: 25px;margin-left: 10px;border: 0px;' type='text' id='divtotalDolor'/> </div>"
            Dim _button As String = "<div class='JCELDA-12'  id='btnbuttonescala2'><input type='button' class='buttonescalaseindicaciones' onclick='btnGuardarEscalaDolor()' value='Guardar' id='btnGuardarescaladolor' style='font-size:18px;'></div>"


            For Each _item As EscalaEIndicacionesE In _escaladolorcabecera
                Dim _Dtr As String = "<tr><td> <label style='font-size: 20px;font-weight: bold;'>" + _item.descripcion_det + "</label></td><td style='padding: 0px;'> <table class='JSBTABLA' style='border: 0px solid #8DC73F;'>  <tbody>"
                Dim _Dtd As String = ""
                Dim _ddtd As String = "</tbody></table></td></tr>"
                For Each _img As EscalaEIndicacionesE In (From n In _escalaDolorImagen Where n.i_order = _item.i_order Select n).ToList()
                    Dim _var As String = "<tr style='border: 0px solid #8DC73F; border-bottom: 1px solid #8DC73F;'><td style='border: 0px solid #8DC73F;' >   <div><img style='width: 60px;height: 60px;' src='../Imagenes/Escala/" + _img.imagencab + "'/></div> <label style='font-size: 18px;'> " + _img.descripcion_det2 + " </label></td> <td style ='width: 280px; padding: 0px;'>  <div style='height: 150px;'>"
                    Dim _var2 = ""
                    For Each _punt As EscalaEIndicacionesE In (From n In _escalaDOlor Where n.i_order = _item.i_order And n.itemcab = _img.itemcab Select n).ToList()
                        If (_punt.puntuacion.ToString().Equals("0")) Then
                            _var2 = _var2 + "<div style='height: 150px;padding-top: 70px;'> <input runat='server' id='checDolor_2_" + _img.itemcab.ToString() + "_" + _punt.puntuacion.ToString() + "'   onchange='clickDolor(this)' style='width: 25px;height: 25px;' type='checkbox' value= '2_" + _img.itemcab.ToString() + "_" + _punt.puntuacion.ToString() + "'/> <div><label> Puntos: " + _punt.puntuacion.ToString() + "</label></div></div>"
                        Else
                            _var2 = _var2 + "<div style='height: 75px;padding-top: 20px;'> <input runat='server' id='checDolor_2_" + _img.itemcab.ToString() + "_" + _punt.puntuacion.ToString() + "'   onchange='clickDolor(this)' style='width: 25px;height: 25px;' type='checkbox' value= '2_" + _img.itemcab.ToString() + "_" + _punt.puntuacion.ToString() + "'/> <div> <label> Puntos: " + _punt.puntuacion.ToString() + "</div></label></div>"
                        End If
                    Next
                    _Dtd = _Dtd + _var + _var2 + "</div></td></tr>"
                Next
                _tr = _tr + _Dtr + _Dtd + _ddtd
            Next


            Dim htmlfinal As String = AperturaHtmlInicio + _tr + FinalizacionHtml + _div + _button

            Dim _ListString As List(Of EscalaEIndicacionesE) = New List(Of EscalaEIndicacionesE)
            Dim _objstring As EscalaEIndicacionesE = New EscalaEIndicacionesE()
            _objstring.descripcion_det = htmlfinal
            _ListString.Add(_objstring)

            _ListView.DataSource = _ListString
            _ListView.DataBind()
        Catch ex As Exception
            Dim menssaje As String = ex.Message
        End Try
    End Sub


    Private Sub tablaEscalaMaDDOX(_ListView As ListView)
        Try
            Dim _escalaMaddox As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 3 Select n).ToList()
            Dim AperturaHtmlInicio As String = "<asp:Label runat='server' ID='lbldetalleIdEscalas' Visible='false' Text='3'></asp:Label> <table class='JSBTABLA JSTABLASCALA' style='font-size: 15px;' id='tablaescala3'> <thead><tr><th>Descripción</th><th>Descripción</th><th>Puntos</th></tr> </thead><tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _TrHtml As String = ""

            Dim _div As String = "<div style='text-align: center;height: 80px;padding-top: 21px;background: white;'> <label style='font-size: 20px;font-weight: bold;'>Puntaje total: </label><input readonly value='' style='font-size: 25px;margin-left: 10px;border: 0px;' type='text' id='divtotalMaddox'/> </div>"

            'Pestaña de actividades
            Dim _actividades As String = "<div class='JTABS'  style='width:100%'><table class='JSBTABLA' id='tablaActividadesMaddox' style='font-size: 18px; visibility: hidden;'><thead><tr><th>N°</th><th>Código</th><th>Actividad</th><th>Confirmar</th></tr></thead><tbody id='tablaactividadMaddox'></tbody> </table></div>"
            Dim _button As String = "<div class='JCELDA-12'  id='btnbuttonescala3'><input type='button' class='buttonescalaseindicaciones' onclick='btnGuardarMaddoxClick()' value='Guardar' id='btnGuardarescalamaddox' style='font-size:18px;'></div>"

            For Each _item As EscalaEIndicacionesE In _escalaMaddox
                _TrHtml = _TrHtml + "<tr><td><label>" + _item.detalleitem + "</label></td> <td style='text-align: center;width: 550px;'>  <div><img style='width: 60px;height: 60px;' src='../Imagenes/Escala/" + _item.imagencab + "'/></div> <div> <label>" + _item.descripcion_det + " </label></div> <label> " + _item.descripcion_det2 + " </label> </td><td style='text-align: center;width: 130px;'><div> <input runat='server' id='checkMaddox_3_" + _item.itemcab.ToString() + "_" + _item.puntuacion.ToString() + "'   onchange='clickMaddox(this)' style='width: 25px;height: 25px;' type='checkbox' value= '3_" + _item.itemcab.ToString() + "_" + _item.puntuacion.ToString() + "'/> <div><label style='font-size: 11px;'> Puntos: " + _item.puntuacion.ToString() + "</label></td></tr>"
            Next


            Dim HtmlFinal As String = AperturaHtmlInicio + _TrHtml + FinalizacionHtml + _div + _actividades + _button

            Dim _ListString As List(Of EscalaEIndicacionesE) = New List(Of EscalaEIndicacionesE)
            Dim _objstring As EscalaEIndicacionesE = New EscalaEIndicacionesE()
            _objstring.descripcion_det = HtmlFinal
            _ListString.Add(_objstring)

            _ListView.DataSource = _ListString
            _ListView.DataBind()
        Catch ex As Exception
            Dim message As String = ex.Message
        End Try
    End Sub

    Private Sub tablaEscalaBraden(_ListView As ListView)
        Try
            Dim _listBradden As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 4 Select n).ToList()
            Dim _ListBraddenCabecera As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 4 Group n By n.itemcab, n.detalleitem Into Group Select New EscalaEIndicacionesE With {.itemcab = itemcab, .detalleitem = detalleitem}).ToList()
            Dim AperturaHtmlInicio As String = " <asp:Label runat='server' ID='lbldetalleIdEscalas' Visible='false' Text='4'></asp:Label> <table class='JSBTABLA JSTABLASCALA' style='text-align: center;' id='tablaescala4'> <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _TrHtml As String = ""
            Dim _TrHtml2 As String = ""
            Dim _div As String = "<div style='text-align: center;height: 80px;padding-top: 21px;background: white;'> <label style='font-size: 20px;font-weight: bold;'>Puntaje total: </label><input readonly value='0' style='font-size: 25px;margin-left: 10px;border: 0px;' type='text' id='divtotalbradden'/> </div>"
            'niveles de riesgo
            Dim _niveles = "<div class='JCELDA-12'><div class='JCELDA-4'><label style='font-size:17px; font-weight:bold;'>Niveles de Riego</label><div><table class='JSBTABLA' style='font-size: 18px; '><thead><tr><th>Nivel</th><th>Valor</th></tr></thead><tbody><tr id='traltoriesgo'><td><label>Alto riesgo</label></td><td><=12</td></tr><tr id='trriesgomoderado'><td><label>Riego Moderado</label></td><td><=14</td></tr><tr id='trriesgobajo'><td><label>Riesgo bajo</label></td><td><=23</td></tr></tbody></table></div></div></div>"
            'Pestaña de actividades
            Dim _actividades As String = "<div class='JTABS'  style='width:100%'><table class='JSBTABLA' id='tablaActividadesBradden' style='font-size: 18px; visibility: hidden;'><thead><tr><th>N°</th><th>Código</th><th>Actividad</th><th>Confirmar</th></tr></thead><tbody id='bodyactividadBradden'></tbody> </table></div>"
            Dim _button As String = "<div class='JCELDA-12'  id='btnbuttonescala4'><input type='button' class='buttonescalaseindicaciones' onclick='btnGuardarEscalaBradden2()' value='Guardar' id='btnGuardarescalabraddeb' style='font-size:18px;'></div>"

            For Each _item As EscalaEIndicacionesE In _ListBraddenCabecera
                _TrHtml2 = "<tr><td><label style='font-size: 18px;font-weight: bold;'>" + _item.detalleitem + "</label></td> <td> <table class='JSBTABLA' style='border: 0px solid #8DC73F;'> <tbody> <tr style='border: 0px solid #8DC73F;'>"
                Dim _th As String = ""
                Dim _itemID As String = ""
                For Each _detEscala As EscalaEIndicacionesE In (From n In _listBradden Where n.itemcab = _item.itemcab Select n).ToList()
                    Dim imge As String = ""
                    Dim puntuacion As String = ""
                    Dim _input As String = ""
                    If (String.IsNullOrWhiteSpace(_detEscala.imagendet)) Then
                        imge = ""
                    Else
                        imge = "<img style='width: 100px;height: 100px;' src='../Imagenes/Escala/" + _detEscala.imagendet + "'/>"
                        puntuacion = "Puntaje: " + _detEscala.puntuacion.ToString()
                        _input = " <div>  <input runat='server' id='checkbraden_4_" + _detEscala.itemcab.ToString() + "_" + _detEscala.puntuacion.ToString() + "'   onchange='clickBradden(this)' style='width: 25px;height: 25px;' type='checkbox' value= '4_" + _detEscala.itemcab.ToString() + "_" + _detEscala.puntuacion.ToString() + "'/>  </div>"
                    End If
                    _itemID = _detEscala.itemcab.ToString()
                    _th = _th + "<td style='border: 0px solid #8DC73F;'>" + _input + " <div> <label style='font-size: 16px; font-weight: bold;'>" + _detEscala.descripcion_det + "</label></div><div><label style='font-size: 14px;'>" + _detEscala.descripcion_det2 + "</label></div><div> " + imge + " </div><div> " + puntuacion + " </div> </td> "
                Next
                _TrHtml = _TrHtml + _TrHtml2 + _th + "</tr></tbody></table></td><td style='width: 65px; text-align: center;'><input type='text' value='0' readonly runat='server' id='lblfilatotalbradden_" + _itemID + "' style='font-size: 18px;width: 60px; text-align: center; font-weight: bold; border: 0px;'/> </td></tr>"
            Next

            Dim HtmlFinal As String = AperturaHtmlInicio + _TrHtml + FinalizacionHtml + _div + _niveles + _actividades + _button


            Dim _ListString As List(Of EscalaEIndicacionesE) = New List(Of EscalaEIndicacionesE)
            Dim _objstring As EscalaEIndicacionesE = New EscalaEIndicacionesE()
            _objstring.descripcion_det = HtmlFinal
            _ListString.Add(_objstring)

            _ListView.DataSource = _ListString
            _ListView.DataBind()

        Catch ex As Exception
            Dim message As String = ex.Message
        End Try
    End Sub

    Private Sub tablaEscalaRiegoCaida(_ListView As ListView)
        Try
            Dim _listRiego As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 5 Select n).ToList()
            Dim _listRiegocabecera As List(Of EscalaEIndicacionesE) = (From n In _listEscala Where n.groupcab = 5 Group n By n.itemcab, n.detalleitem Into Group Select New EscalaEIndicacionesE With {.itemcab = itemcab, .detalleitem = detalleitem}).ToList()
            Dim AperturaHtmlInicio As String = " <asp:Label runat='server' ID='lbldetalleIdEscalas' Visible='false' Text='5'></asp:Label> <table class='JSBTABLA JSTABLASCALA' style='text-align: center;'  id='tablaescala5'> <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _TrHtml As String = ""
            Dim _TrHtml2 As String = ""
            Dim _div As String = "<div style='text-align: center;height: 80px;padding-top: 21px;background: white;'> <label style='font-size: 20px;font-weight: bold;'>Puntaje total: </label><input readonly value='0' style='font-size: 25px;margin-left: 10px;border: 0px;' type='text' id='divtotalriesgocaida'/> </div>"
            Dim _button As String = "<div class='JCELDA-12'  id='btnbuttonescala5'><input type='button' class='buttonescalaseindicaciones' onclick='btnGuardarEscalariesgocaida()' value='Guardar' style='font-size:18px;'></div>"


            'Pestaña de actividades
            Dim _actividades As String = "<div class='JTABS'  style='width:100%'><table class='JSBTABLA' id='tablaActividadesRiesgoCaida' style='font-size: 18px; visibility: hidden;'><thead><tr><th>N°</th><th>Código</th><th>Actividad</th><th>Confirmar</th></tr></thead><tbody id='bodyactividadRiesgoCaida'></tbody> </table></div>"


            For Each _item As EscalaEIndicacionesE In _listRiegocabecera
                _TrHtml2 = "<tr><td><label style='font-size: 18px;font-weight: bold;'>" + _item.detalleitem + "</label></td> <td style='padding: 0px;'> <table class='JSBTABLA' style='border: 0px solid #8DC73F;'> <tbody>"
                Dim _th As String = ""
                For Each _detalleRiesgo As EscalaEIndicacionesE In (From n In _listRiego Where n.itemcab = _item.itemcab)
                    _th = _th + " <tr style='border: 0px solid #8DC73F;border-bottom: 1px solid #8DC73F;'><td style='border: 0px solid #8DC73F; border-right: 1px solid #8DC73F;height: 50px;'><label style='font-size: 16px;'>" + _detalleRiesgo.descripcion_det + "</label> <td style='width: 150px;border: 0px solid #8DC73F;height: 50px;'> " + " <div>  <input runat='server' id='checkriesgocaida_5_" + _detalleRiesgo.itemcab.ToString() + "_" + _detalleRiesgo.i_order.ToString() + "_" + _detalleRiesgo.puntuacion.ToString() + "'   onchange='clickRiegoCaida(this)' style='width: 25px;height: 25px;' type='checkbox' value= '5_" + _detalleRiesgo.itemcab.ToString() + "_" + _detalleRiesgo.i_order.ToString() + "_" + _detalleRiesgo.puntuacion.ToString() + "'/>  </div>" + " <label> Puntos" + _detalleRiesgo.puntuacion.ToString() + "</label></td></tr>"
                Next
                _TrHtml = _TrHtml + _TrHtml2 + _th + "</tbody></table></td> <td style='width: 65px; text-align: center;'><input type='text' value='' readonly runat='server' id='lblfilatotalRiesgoCaida_" + _item.itemcab.ToString() + "' style='font-size: 18px;width: 60px; text-align: center; font-weight: bold; border: 0px;'/> <input type='text' value='' readonly runat='server' id='lblfilatotalRiesgoCaida2_" + _item.itemcab.ToString() + "' style='font-size: 1px;width: 2px; text-align: center; font-weight: bold; border: 0px;'/> </td> </tr>"
            Next

            Dim HtmlFinal As String = AperturaHtmlInicio + _TrHtml + FinalizacionHtml + _div + _actividades + _button

            Dim _ListString As List(Of EscalaEIndicacionesE) = New List(Of EscalaEIndicacionesE)
            Dim _objstring As EscalaEIndicacionesE = New EscalaEIndicacionesE()
            _objstring.descripcion_det = HtmlFinal
            _ListString.Add(_objstring)

            _ListView.DataSource = _ListString
            _ListView.DataBind()

        Catch ex As Exception
            Dim message As String = ex.Message
        End Try
    End Sub


    Protected Sub lvdetalleEscalaEIndicaciones_ItemDataBound(sender As Object, e As ListViewItemEventArgs)
        Try
            Dim _Id As Label = (e.Item.FindControl("lbldetalleIdEscalas"))
            Dim _Key As Integer = Convert.ToInt32(_Id.Text)
            Select Case _Key
                Case 1
                    Dim _ckeckbox As CheckBox = (e.Item.FindControl("checkGlascow"))

            End Select
        Catch ex As Exception
            Dim messaje As String = ex.Message
        End Try
    End Sub
    Protected Sub txt_CheckedChanged(sender As Object, e As EventArgs)
        Dim ee As String = e.ToString()
    End Sub



    'WEB SERVICE PARA LAS ACTIVIDADES'
    <WebMethod()>
    Public Shared Function ActividaesEscalaEIndicaciones(_Order As String, _Codigo As String, _valor As String) As List(Of EscalaEIndicacionesActividadE)

        Dim _listaActividad As List(Of EscalaEIndicacionesActividadE) = New List(Of EscalaEIndicacionesActividadE)()
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim order As String = _Encript.EncryptConectionString(_Order)
            Dim variable As String = _Encript.EncryptConectionString(_Codigo)
            Dim val As String = _Encript.EncryptConectionString(_valor)
            Dim val1 As String = _Encript.EncryptConectionString("")
            Dim val2 As String = _Encript.EncryptConectionString("")
            Dim val3 As String = _Encript.EncryptConectionString("")
            Dim rutaApi As String = Apiruta
            Dim _cliente As RestClient = New RestClient(rutaApi + "EscalaEIndicacionesEnfermeria/API/Clinica/ActividadEscalaEindicaciones?order=" + order + "&variable=" + variable + "&val=" + val + "&val1=" + val1 + "&val2=" + val2 + "&val3=" + val3)
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _listaActividad = JsonConvert.DeserializeObject(Of List(Of EscalaEIndicacionesActividadE))(_Response.Content)

            Return _listaActividad

        Catch ex As Exception
            Dim _message As String = ex.Message
            _listaActividad = New List(Of EscalaEIndicacionesActividadE)()
            Return _listaActividad
        End Try
    End Function

    'WEB SERVICE PARA LOS REGISTRO 
    <WebMethod()>
    Public Shared Function RegistrowebGlascow(Tipo As Integer, puntaje1 As Integer, puntaje2 As Integer, puntaje3 As Integer, total As Integer) As String
        Dim _mensagge As String
        Try
            'Dim NombreUser As String = Session(sNombreUsuario)
            'Dim codUser As String = Session(sCodUser)
            Dim _obj As PuntuacionEscalaE = New PuntuacionEscalaE()
            _obj.Tipo = Tipo
            _obj.Puntaje1 = puntaje1
            _obj.Puntaje2 = puntaje2
            _obj.Puntaje3 = puntaje3
            _obj.Puntaje4 = 0
            _obj.Puntaje5 = 0
            _obj.Puntaje6 = 0
            _obj.Puntaje7 = 0
            _obj.Total = total
            _obj.NomUser = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            _obj.CodUser = CType(HttpContext.Current.Session(sCodUser), String)
            _obj.Perfil = CType(HttpContext.Current.Session(sPerfilUsuario), String)
            _obj.CodMedico = CType(HttpContext.Current.Session(sCodMedico), String)
            _obj.CodigoAtencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            _obj.IdeHistoria = CType(HttpContext.Current.Session(sIdeHistoria), String)
            _obj.CodPaciente = CType(HttpContext.Current.Session(sCodPaciente), String)
            _obj.Valor = ""
            Dim respuesta As String = Registrar_EscalasEIndicacionesEnfermeria(_obj)
            _mensagge = respuesta
            Return _mensagge
        Catch ex As Exception
            _mensagge = ex.Message
            Return _mensagge
        End Try
    End Function

    <WebMethod>
    Public Shared Function RegistrowebDolor(Tipo As Integer, puntaje1 As Integer, total As Integer)
        Dim _mensagge As String
        Try
            Dim _obj As PuntuacionEscalaE = New PuntuacionEscalaE()
            _obj.Tipo = Tipo
            _obj.Puntaje1 = puntaje1
            _obj.Puntaje2 = 0
            _obj.Puntaje3 = 0
            _obj.Puntaje4 = 0
            _obj.Puntaje5 = 0
            _obj.Puntaje6 = 0
            _obj.Puntaje7 = 0
            _obj.Total = total
            _obj.NomUser = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            _obj.CodUser = CType(HttpContext.Current.Session(sCodUser), String)
            _obj.Perfil = CType(HttpContext.Current.Session(sPerfilUsuario), String)
            _obj.CodMedico = CType(HttpContext.Current.Session(sCodMedico), String)
            _obj.CodigoAtencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            _obj.IdeHistoria = CType(HttpContext.Current.Session(sIdeHistoria), String)
            _obj.CodPaciente = CType(HttpContext.Current.Session(sCodPaciente), String)
            _obj.Valor = ""
            Dim respuesta As String = Registrar_EscalasEIndicacionesEnfermeria(_obj)
            _mensagge = respuesta
            Return _mensagge
        Catch ex As Exception
            _mensagge = ex.Message
            Return _mensagge
        End Try
    End Function

    <WebMethod()>
    Public Shared Function RegistrowebBradden(Tipo As Integer, puntaje1 As Integer, puntaje2 As Integer, puntaje3 As Integer, puntaje4 As Integer, puntaje5 As Integer, puntaje6 As Integer, total As Integer) As String
        Dim _mensagge As String
        Try
            'Dim NombreUser As String = Session(sNombreUsuario)
            'Dim codUser As String = Session(sCodUser)
            Dim _obj As PuntuacionEscalaE = New PuntuacionEscalaE()
            _obj.Tipo = Tipo
            _obj.Puntaje1 = puntaje1
            _obj.Puntaje2 = puntaje2
            _obj.Puntaje3 = puntaje3
            _obj.Puntaje4 = puntaje4
            _obj.Puntaje5 = puntaje5
            _obj.Puntaje6 = puntaje6
            _obj.Puntaje7 = 0
            _obj.Total = total
            _obj.NomUser = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            _obj.CodUser = CType(HttpContext.Current.Session(sCodUser), String)
            _obj.Perfil = CType(HttpContext.Current.Session(sPerfilUsuario), String)
            _obj.CodMedico = CType(HttpContext.Current.Session(sCodMedico), String)
            _obj.CodigoAtencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            _obj.IdeHistoria = CType(HttpContext.Current.Session(sIdeHistoria), String)
            _obj.CodPaciente = CType(HttpContext.Current.Session(sCodPaciente), String)
            _obj.Valor = ""
            Dim respuesta As String = Registrar_EscalasEIndicacionesEnfermeria(_obj)
            _mensagge = respuesta
            Return _mensagge
        Catch ex As Exception
            _mensagge = ex.Message
            Return _mensagge
        End Try
    End Function

    <WebMethod()>
    Public Shared Function RegistrowebRiesgocaida(Tipo As Integer, puntaje1 As Integer, puntaje2 As Integer, puntaje3 As Integer, puntaje4 As Integer, puntaje5 As Integer, puntaje6 As Integer, puntaje7 As Integer, valor As String, total As Integer) As String
        Dim _mensagge As String
        Try
            'Dim NombreUser As String = Session(sNombreUsuario)
            'Dim codUser As String = Session(sCodUser)
            Dim _obj As PuntuacionEscalaE = New PuntuacionEscalaE()
            _obj.Tipo = Tipo
            _obj.Puntaje1 = puntaje1
            _obj.Puntaje2 = puntaje2
            _obj.Puntaje3 = puntaje3
            _obj.Puntaje4 = puntaje4
            _obj.Puntaje5 = puntaje5
            _obj.Puntaje6 = puntaje6
            _obj.Puntaje7 = puntaje7
            _obj.Total = total
            _obj.NomUser = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            _obj.CodUser = CType(HttpContext.Current.Session(sCodUser), String)
            _obj.Perfil = CType(HttpContext.Current.Session(sPerfilUsuario), String)
            _obj.CodMedico = CType(HttpContext.Current.Session(sCodMedico), String)
            _obj.CodigoAtencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            _obj.IdeHistoria = CType(HttpContext.Current.Session(sIdeHistoria), String)
            _obj.CodPaciente = CType(HttpContext.Current.Session(sCodPaciente), String)
            _obj.Valor = valor
            Dim respuesta As String = Registrar_EscalasEIndicacionesEnfermeria(_obj)
            _mensagge = respuesta
            Return _mensagge
        Catch ex As Exception
            _mensagge = ex.Message
            Return _mensagge
        End Try
    End Function

    Private Shared Function Registrar_EscalasEIndicacionesEnfermeria(obj As PuntuacionEscalaE) As String
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim _Json As String = JsonConvert.SerializeObject(obj)
            Dim _client As RestClient = New RestClient(Apiruta + "EscalaEIndicacionesEnfermeria/API/Clinica/ActividadEscalaEindicacionesRegister")
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.POST
            _request.AddHeader("Accept", "application/json")
            _request.AddJsonBody(obj)
            Dim _Response As RestResponse = _client.Execute(_request)
            Dim _JsonGet As RespuestaJsonE = JsonConvert.DeserializeObject(Of RespuestaJsonE)(_Response.Content)

            Return _JsonGet.Message

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'LISTADO DE DATOS HISTORICO
    <WebMethod>
    Public Shared Function Listado_datos_historicos_escala(Valor1 As String, Valor2 As String) As String
        Dim Listhistorico As List(Of EscalaeIndicacionesHistoricoE) = New List(Of EscalaeIndicacionesHistoricoE)()
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim _codigoAtencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)

            Dim order As String = _Encript.EncryptConectionString("5")
            Dim variable As String = _Encript.EncryptConectionString("0")
            Dim val As String = _Encript.EncryptConectionString(_codigoAtencion)
            Dim val1 As String = _Encript.EncryptConectionString(Valor1)
            Dim val2 As String = _Encript.EncryptConectionString(Valor2)
            Dim val3 As String = _Encript.EncryptConectionString("")
            Dim rutaApi As String = Apiruta
            Dim _cliente As RestClient = New RestClient(rutaApi + "EscalaEIndicacionesEnfermeria/API/Clinica/ActividadEscalaEindicacionesDatoshistoricos?order=" + order + "&variable=" + variable + "&val=" + val + "&val1=" + val1 + "&val2=" + val2 + "&val3=" + val3)
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            Listhistorico = JsonConvert.DeserializeObject(Of List(Of EscalaeIndicacionesHistoricoE))(_Response.Content)

            Dim _fechavalor = (From n In Listhistorico Group n By n.Fecha Into Group Select New EscalaeIndicacionesHistoricoE With {.Fecha = Fecha}).ToList()
            Dim _escalavalor = (From n In Listhistorico Group n By n.Ide_Escala, n.Escala, n.Fecha Into Group Select New EscalaeIndicacionesHistoricoE With {.Ide_Escala = Ide_Escala, .Escala = Escala, .Fecha = Fecha}).ToList()
            Dim AperturaHtml As String = "<table class='JSBTABLA JSTABLASCALA' style='font-size: 18px;text-align: center;'> <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _tdApertura As String = ""

            For Each _item As EscalaeIndicacionesHistoricoE In _fechavalor
                Dim tdvalor1 As String = "<tr><td style='width: 200px;font-weight: bold;'>" + _item.Fecha.ToShortDateString() + "</td><td>"
                Dim aperturaHtmltd As String = "<table class='JSBTABLA JSTABLASCALA' style='font-size: 15px;'> <tbody>"
                Dim tdvalor2 As String = ""
                Dim finalHtmltd As String = "</tbody></table>"
                Dim tdfinvalor1 As String = "</td></tr>"

                For Each _itemtd As EscalaeIndicacionesHistoricoE In (From n In _escalavalor Where n.Fecha = _item.Fecha Select n).ToList()
                    Dim td2inicio As String = "<tr><td style='width: 250px;font-weight: bold;'>" + _itemtd.Escala + "</td><td>"
                    Dim aperturaHtmltd2 As String = "<table class='JSBTABLA JSTABLASCALA' style='font-size: 14px;'><thead><tr><th>Usuario</th><th>Hora</th><th>Resultado</th><th>opción</th></tr></thead> <tbody>"
                    Dim tdFInal As String = ""
                    Dim finalHtmltd2 As String = "</tbody></table>"
                    Dim td2fin As String = "</td></tr>"

                    For Each _tdItem As EscalaeIndicacionesHistoricoE In (From n In Listhistorico Where n.Fecha = _itemtd.Fecha And n.Ide_Escala = _itemtd.Ide_Escala Select n).ToList()
                        tdFInal = tdFInal + "<tr><td style='width: 200px;'>" + _tdItem.Encargado + "</td><td style='width: 200px;'>" + _tdItem.Hora + "</td><td style='width: 200px;'>" + _tdItem.Puntaje.ToString() + "</td><td style='width: 150px;'><div class='tooltip' style='margin-left: 30px;'><img runat='server' src='../Imagenes/lista_pedidos.png' alt='' class='JIMG-GENERAL'   onclick='EscalaEIntervencionVerResumen(" + _tdItem.ide_escalaeintervenciondet.ToString() + ", " + _tdItem.Ide_Escala.ToString() + ", " + _tdItem.Puntaje.ToString() + ")' />  <span tooltip-direccion='abajo'>Ver resumen</span></div></td></tr>"
                    Next
                    tdvalor2 = tdvalor2 + td2inicio + aperturaHtmltd2 + tdFInal + finalHtmltd2 + td2fin
                Next

                _tdApertura = _tdApertura + tdvalor1 + aperturaHtmltd + tdvalor2 + finalHtmltd + tdfinvalor1
            Next

            'Return Listhistorico
            Dim htmlFinal As String = AperturaHtml + _tdApertura + FinalizacionHtml
            Return htmlFinal

        Catch ex As Exception
            Dim _message As String = ex.Message

            Return ""
        End Try
    End Function


    <WebMethod>
    Public Shared Function Listado_datos_historicos_escalaDetallado(_variable As String, IDEscala As String, Total As String) As String
        Dim Listhistorico As List(Of EscalaEIndicacionesHistoriaDetalladoE) = New List(Of EscalaEIndicacionesHistoriaDetalladoE)()
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim _codigoAtencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)

            Dim order As String = _Encript.EncryptConectionString("6")
            Dim variable As String = _Encript.EncryptConectionString(_variable)
            Dim val As String = _Encript.EncryptConectionString("")
            Dim val1 As String = _Encript.EncryptConectionString("")
            Dim val2 As String = _Encript.EncryptConectionString("")
            Dim val3 As String = _Encript.EncryptConectionString("")
            Dim rutaApi As String = Apiruta
            Dim _cliente As RestClient = New RestClient(rutaApi + "EscalaEIndicacionesEnfermeria/API/Clinica/ActividadEscalaEindicacionesDatoshistoricosDetallado?order=" + order + "&variable=" + variable + "&val=" + val + "&val1=" + val1 + "&val2=" + val2 + "&val3=" + val3)
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            Listhistorico = JsonConvert.DeserializeObject(Of List(Of EscalaEIndicacionesHistoriaDetalladoE))(_Response.Content)

            Dim AperturaHtml As String = "<table class='JSBTABLA JSTABLASCALA' style='text-align: center;'><thead><tr><th>Concepto</th><th>Detalle</th><th>Puntaje</th></tr></thead> <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim _tdApertura As String = ""
            For Each _item As EscalaEIndicacionesHistoriaDetalladoE In Listhistorico
                Dim _style As String = " style = 'background:#dbdbdb;'"
                If (_item.Concepto.Trim().ToLower().Contains("total")) Then
                    _tdApertura = _tdApertura + "<tr " + _style + "><td>" + _item.Concepto + "</td><td>" + _item.Descripcion + " <br> Observación: " + _item.Observacion + "</td><td>" + _item.Puntaje.ToString() + "</td></tr>"
                Else
                    _tdApertura = _tdApertura + "<tr><td>" + _item.Concepto + "</td><td>" + _item.Descripcion + " <br> Observación: " + _item.Observacion + "</td><td>" + _item.Puntaje.ToString() + "</td></tr>"
                End If

            Next

            '12  MADDOX
            '19  BRADEN
            '26  RIESGOS DE CAIDAS 
            Dim variableescala As Integer = 0 'order

            If (IDEscala = "12") Then
                variableescala = 2
            End If
            If (IDEscala = "19") Then
                variableescala = 3
            End If
            If (IDEscala = "26") Then
                variableescala = 4
            End If
            Dim Actividad As String = ""

            If (variableescala > 0) Then
                Actividad = ExtraerActividades(variableescala.ToString(), IDEscala, Total)
            End If


            '

            Dim htmlFinal As String = AperturaHtml + _tdApertura + FinalizacionHtml + Actividad
            Return htmlFinal

        Catch ex As Exception
            Dim _message As String = ex.Message
            Return ""
        End Try
    End Function

    Private Shared Function ExtraerActividades(_Order As String, _Codigo As String, _valor As String) As String
        Dim _listaActividad As List(Of EscalaEIndicacionesActividadE) = New List(Of EscalaEIndicacionesActividadE)()
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim order As String = _Encript.EncryptConectionString(_Order)
            Dim variable As String = _Encript.EncryptConectionString(_Codigo)
            Dim val As String = _Encript.EncryptConectionString(_valor)
            Dim val1 As String = _Encript.EncryptConectionString("")
            Dim val2 As String = _Encript.EncryptConectionString("")
            Dim val3 As String = _Encript.EncryptConectionString("")
            Dim rutaApi As String = Apiruta
            Dim _cliente As RestClient = New RestClient(rutaApi + "EscalaEIndicacionesEnfermeria/API/Clinica/ActividadEscalaEindicaciones?order=" + order + "&variable=" + variable + "&val=" + val + "&val1=" + val1 + "&val2=" + val2 + "&val3=" + val3)
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _listaActividad = JsonConvert.DeserializeObject(Of List(Of EscalaEIndicacionesActividadE))(_Response.Content)

            Dim NivelRiesgo As String = ""
            If (_Codigo = "19") Then
                Dim _obj As EscalaEIndicacionesActividadE = (From n In _listaActividad Select n).Take(1).FirstOrDefault()
                NivelRiesgo = " <p>    <label style='font-weight:bold; font-size:18px;'>Nivel de Riesgo:</label><label style=' font-size:18px;'> " + _obj.Detalle + "</label></p>"
            End If

            Dim _divOpen As String = "<p><label style='font-size:18px;'> Actividades</label> </p>"

            Dim AperturaHtml As String = "<table class='JSBTABLA JSTABLASCALA' style='text-align: center;'><thead><tr><th>N°</th><th>Actividad</th></tr></thead> <tbody>"
            Dim FinalizacionHtml As String = "</tbody></table>"
            Dim tdOpen As String = ""
            For Each _item As EscalaEIndicacionesActividadE In _listaActividad
                tdOpen = tdOpen + "<tr><td>" + _item.Item.ToString() + "</td><td>" + _item.Actividad + "</td></tr>"
            Next

            Dim htmlfinal As String = _divOpen + NivelRiesgo + AperturaHtml + tdOpen + FinalizacionHtml

            Return htmlfinal

        Catch ex As Exception

            Return ""
        End Try
    End Function

    'WEB METODO PARA KARDEX HOSPITALARIO
    <WebMethod>
    Public Shared Function Listar_Kardex_Hospitalarios() As String
        Dim List_general_Kardex As List(Of IndicacionesMedicasE) = ObtenerDatosIndicacionesMedicas()
        Dim _list_ProgramacionKardexE As List(Of ProgramacionKardexE) = ObtenerDatosProgramados()

        Dim _Listcabecera As List(Of IndicacionesMedicasE) = (From n In List_general_Kardex Group n By n.IdTipo, n.NombreTipo, n.Icons Into Group Select New IndicacionesMedicasE With {.IdTipo = IdTipo, .NombreTipo = NombreTipo, .Icons = Icons}).ToList()
        Dim CabeceraString As String = ""

        For Each _item As IndicacionesMedicasE In _Listcabecera
            CabeceraString = CabeceraString + "<input type='radio' onchange='clickInputKardexValidation(this)' value='" + _item.IdTipo.ToString() + "' id='tabclinico" + _item.IdTipo.ToString() + "' name='TabNro2' class='JCHEK-TABS'/>  <label for='tabclinico" + _item.IdTipo.ToString() + "' class='JTABS-LABEL'>" + _item.NombreTipo + "</label> "
        Next

        Dim _html As String = ""
        Dim _htmlFinalMostrar As String = ""


        For Each _item As IndicacionesMedicasE In _Listcabecera
            Dim _div1Inicio As String = " <div class='JCONTENIDO-TAB' id='Contenidotabclinico" + _item.IdTipo.ToString() + "'> "
            Dim _div2inicio As String = "  <div class='JFILA'> <div class='JCELDA-12'> "

            Dim _tablaInicio As String = " <table class='JSBTABLA JSTABLASCALA' style='font-size:16px;' id='tablekardexenfermeria" + _item.IdTipo.ToString() + "'>  <thead> <tr class='fw-bolder'> <th scope='col' class='text-center'>Registro</th> <th scope='col' class='text-center'>Indicaciones</th> <th scope='col' class='text-center'>Confirmar</th></tr> </thead> <tbody>"

            Dim _tdMostrar As String = ""
            Dim _listdetalle As List(Of IndicacionesMedicasE) = (From n In List_general_Kardex Where n.IdTipo = _item.IdTipo Select n).ToList()
            For Each _det As IndicacionesMedicasE In _listdetalle
                Dim _tr1Inicio As String = "<tr>"
                Dim _tdinterno As String = ""

                If (_det.flg_suspendido = "0") Then

                    Dim _td1 As String = " <td style='color: red; text-decoration: line-through;'>  <label style='font-weight:bold;'>Médico:&ensp;</label><label>" + _det.NombreMedico + "<br /> <label style='font-weight:bold;'>Fecha:&ensp;</label>" + _det.Fecha + "<br /><label style='font-weight:bold;'>Hora:&ensp;</label> " + _det.HOR_REGISTRO + " </td>"
                    Dim _td2 As String = ""
                    If (_det.IdTipo = 3) Then
                        _td2 = " <td style='color: red; text-decoration: line-through;'>  <label style='font-weight:bold;'> DCI: </label>" + _det.dsc_producto + "  <br /> <label style='font-weight:bold;'>  Dosis: </label>" + _det.num_dosis + "  &ensp;<label style='font-weight:bold;'> Vía: </label>" + _det.dsc_via + "  &ensp;"
                        Dim stat As String = ""
                        If (_det.num_frecuencia.Trim() = "STAT" Or _det.num_frecuencia.Trim() = "PRN") Then
                            stat = " <label style='font-weight:bold; color:red;'> " + _det.num_frecuencia + " </label> <br /><label style='font-weight:bold;'> Detalle: </label>" + _det.txt_detalle + "  <br /> </td>"
                        Else
                            stat = " <label style='font-weight:bold;'> Cada (Hrs): </label>" + _det.num_frecuencia + "  <br /><label style='font-weight:bold;'> Detalle: </label>" + _det.txt_detalle + "  <br /> </td>"
                        End If
                        _td2 = _td2 + stat
                    Else
                        _td2 = " <td style='color: red; text-decoration: line-through;'> " + _det.dsc_producto + "</td>"
                    End If
                    Dim _td3 As String = " <td style='text-align: center;'> <label style='font-weight:bold; color:white;background: red;padding: 8px;border-radius: 7px; font-size: 13px;'>Suspendido</label> </td> "
                    _tdinterno = _td1 + _td2 + _td3

                Else
                    Dim _td1 As String = " <td>  <label style='font-weight:bold;'>Médico:&ensp;</label><label>" + _det.NombreMedico + "<br /> <label style='font-weight:bold;'>Fecha:&ensp;</label>" + _det.Fecha + "<br /><label style='font-weight:bold;'>Hora:&ensp;</label> " + _det.HOR_REGISTRO + " </td>"
                    Dim _td2 As String = ""

                    If (_det.IdTipo = 3) Then
                        _td2 = " <td >  <label style='font-weight:bold;'> DCI: </label>" + _det.dsc_producto + "  <br /> <label style='font-weight:bold;'>  Dosis: </label>" + _det.num_dosis + "  &ensp;<label style='font-weight:bold;'> Vía: </label>" + _det.dsc_via + "  &ensp;"
                        Dim stat As String = ""
                        If (_det.num_frecuencia.Trim() = "STAT" Or _det.num_frecuencia.Trim() = "PRN") Then
                            stat = " <label style='font-weight:bold; color:red;'> " + _det.num_frecuencia + " </label> <br /><label style='font-weight:bold;'> Detalle: </label>" + _det.txt_detalle + "  <br /> </td>"
                        Else
                            stat = " <label style='font-weight:bold;'> Cada (Hrs): </label>" + _det.num_frecuencia + "  <br /><label style='font-weight:bold;'> Detalle: </label>" + _det.txt_detalle + "  <br /> </td>"
                        End If
                        _td2 = _td2 + stat
                    Else
                        _td2 = " <td> " + _det.dsc_producto + "</td>"

                    End If

                    Dim _td3 As String = ""

                    If (_det.IdTipo = 3 And _det.TotDetalle = 0 And _det.NumeracionFrecuencia > 0 And _det.num_frecuencia <> "STAT" And _det.num_frecuencia <> "PRN") Then
                        Dim _variable As String = "&#39;" + _det.ide_medicamentorec.ToString() + "_" + _det.dsc_producto + "_" + _det.NumeracionFrecuencia.ToString() + "_" + _det.num_frecuencia.ToString() + "&#39;"
                        _td3 = "<td style=' text-align: center; width: 550px;'> <input style='width: 25px;height: 25px;' type='button' value='Programar'  onclick = 'ProgramarHorarios(" + _variable + ")' class='btn btn-sm btn-primary'/> </td>"

                    ElseIf (_det.IdTipo = 3 And _det.NumeracionFrecuencia > 0 And _det.TotDetalle > 0 And _det.num_frecuencia <> "STAT" And _det.num_frecuencia <> "PRN") Then

                        Dim _inputprogramado As String = ""


                        If (_det._item = 0 Or _det.SumEstado < _det.TotDetalle) Then

                            Dim _tdporgramado As String = ""

                            For Each _programado As ProgramacionKardexE In (From n In _list_ProgramacionKardexE Where n.Ide_medicamentorec = _det.ide_medicamentorec Select n).ToList()

                                Dim _td1prInicio As String = "<td style='border: 0px solid #8DC73F;'>"
                                Dim _td1prFin As String = "</td>"

                                Dim _inputpr As String = ""
                                If (_programado.I_estadoadministrado = 0) Then
                                    If (_programado.Item = 1) Then

                                        _inputpr = "<input type='checkbox' id='InputKardexhospitalarioenfermeria' name='InputKardexhospitalarioenfermeria' style='width: 25px;height: 25px;' onchange='Kardex_RegistrosDetalle(this)' value='" + _det.ide_medicamentorec.ToString() + "_" + _det.ide_receta.ToString() + "_" + _det.cod_atencion.ToString() + "_" + _det.IdTipo.ToString() + "_" + _det.NombreTipo + "' />   <p>" + _programado.Fechaprogramada + "<br><label style='font-weight:bold;'>" + _programado.Horaprogramada + "</label></p> <p style='color:white;'><label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label><br><label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label></p>"
                                    Else
                                        _inputpr = "<input type='checkbox' readonly  style='width: 25px;height: 25px;'  class='checkBox-Confirms' disabled='disabled' /> <p>" + _programado.Fechaprogramada + "<br><label style='font-weight:bold;'>  " + _programado.Horaprogramada + " </label></p>  <p style='color:white;'><label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label><br><label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label></p>"
                                    End If
                                Else
                                    _inputpr = "<input type = 'checkbox' ReadOnly  style='width: 25px;height: 25px;'  checked='checked' disabled='disabled' />  <p style = 'font-weight:bold;' >" + _programado.Fechaprogramada + "<br><label style='font-weight:bold;'> " + _programado.Horaprogramada + " </label></p>  <p style='font-weight:bold;'><label> " + _programado.Usr_adminstra + " </label><br> " + _programado.FechaAdministrada + "</p> "

                                End If
                                _tdporgramado = _tdporgramado + _td1prInicio + _inputpr + _td1prFin

                            Next
                            _inputprogramado = "<table style='width: 100%;'><tbody><tr style='overflow-y: auto; display: block;  width: 550px; border: 0px; padding: 0px;'>" + _tdporgramado + "</tr></tbody></table>"
                        Else
                            _inputprogramado = "<label style='font-weight:bold; color:white;background: blue;padding: 8px;border-radius: 7px; font-size: 13px;'>ADMINISTRACIÓN COMPLETA</label>"
                        End If
                        _td3 = "<td style=' text-align: center;  width: 450px;'>" + _inputprogramado + "</td> "

                    Else
                        Dim check_input As String = "<input style='width: 25px;height: 25px;'  id='InputKardexhospitalarioenfermeria'  name='InputKardexhospitalarioenfermeria' type = 'checkbox' onchange='Kardex_RegistrosDetalle(this)' value='" + _det.ide_medicamentorec.ToString() + "_" + _det.ide_receta.ToString() + "_" + _det.cod_atencion.ToString() + "_" + _det.IdTipo.ToString() + "_" + _det.NombreTipo + "' />"
                        Dim _pstring As String = ""
                        If (_det.UltimoUserRegistro <> "") Then
                            If (_det.IdTipo = 2) Then
                                _pstring = "<p><label style = 'color:darkred; font-weight: bold;' > Hora Inicio:</label><br><label style = 'font-weight:bold;' >" + _det.UltimoUserRegistro.ToString() + " &nbsp;</label><br><label style = 'font-weight:bold;' >" + _det.UltimoFechaRegistro.ToString() + "&nbsp;</label></p>"
                            Else
                                _pstring = "<p><label style = 'color:darkred; font-weight: bold;' > Ultima administración:</label><br><label style = 'font-weight:bold;' >" + _det.UltimoUserRegistro.ToString() + " &nbsp;</label><br><label style = 'font-weight:bold;' >" + _det.UltimoFechaRegistro.ToString() + "&nbsp;</label></p>"

                            End If

                            _td3 = "<td style=' text-align: center;  width: 550px;'>" + check_input + _pstring + "</td> "
                        End If
                    End If
                    _tdinterno = _td1 + _td2 + _td3

                End If

                Dim _tr1Fin As String = "</tr>"

                _tdMostrar = _tdMostrar + _tr1Inicio + _tdinterno + _tr1Fin

            Next

            Dim _tablaFin As String = " </tbody> </table>"
            Dim _div1Fin As String = " </div> </div> </div>"

            _html = _html + _div1Inicio + _div2inicio + _tablaInicio + _tdMostrar + _tablaFin + _div1Fin
        Next

        ' Dim List_general_Kardex As List(Of IndicacionesMedicasE)
        Dim idekardex As IndicacionesMedicasE = (From n In List_general_Kardex Group n By n.ide_KardexHospitalario Into Group Select New IndicacionesMedicasE With {.ide_KardexHospitalario = ide_KardexHospitalario}).FirstOrDefault()

        Dim _peso As HospitalE = obtnerPesopaciente()
        Dim _ide_KardexHospitalario As String = "<div><input id='ide_KardexHospitalarioItem' type='text' value='" + _peso.Ide_kardexhospitalizacion.ToString() + "' disabled='disabled' style='visibility: hidden;'/></div><div><input id='ide_pesopaciente' type='text' value='" + _peso.PesoPaciente.ToString() + "' disabled='disabled' style='visibility: hidden;'/></div>"

        _htmlFinalMostrar = CabeceraString + _html + _ide_KardexHospitalario

        Return _htmlFinalMostrar

    End Function

    Private Shared Function obtnerPesopaciente() As HospitalE
        Dim codatencion As String = CType(HttpContext.Current.Session(sCodigoAtencion), String)
        Return New HospitalLN().Datos_PacienteHospitalizado(codatencion)
    End Function

    Private Shared Function ObtenerDatosProgramados() As List(Of ProgramacionKardexE)
        Try
            Dim _list As List(Of ProgramacionKardexE) = New List(Of ProgramacionKardexE)()
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString
            Dim codatencio As String = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            Dim rutaApi As String = Apiruta


            Dim _cliente As RestClient = New RestClient(rutaApi + "KardexHospitalizacion/API/Clinica/KardexHistoricoPacienteHospitalarioPRogramado?_Key=" + _Encript.EncryptConectionString(codatencio))
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _list = JsonConvert.DeserializeObject(Of List(Of ProgramacionKardexE))(_Response.Content)
            Return _list

        Catch ex As Exception
            Return New List(Of ProgramacionKardexE)()
        End Try
    End Function

    Private Shared Function ObtenerDatosIndicacionesMedicas() As List(Of IndicacionesMedicasE)
        Dim _list As List(Of IndicacionesMedicasE) = New List(Of IndicacionesMedicasE)()
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim codatencio As String = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            Dim rutaApi As String = Apiruta
            Dim _cliente As RestClient = New RestClient(rutaApi + "KardexHospitalizacion/API/Clinica/PacienteIndicacionesMedica?_Key=" + _Encript.EncryptConectionString(codatencio))
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _list = JsonConvert.DeserializeObject(Of List(Of IndicacionesMedicasE))(_Response.Content)
            Return _list
        Catch ex As Exception
            Return New List(Of IndicacionesMedicasE)()
        End Try
    End Function

    <WebMethod>
    Public Shared Function obtenerfechasprogramadas(_numeracionfrecuencia As Integer, _num_frecuencia As Integer, _horainicio As Integer, _tipohora As String) As String
        Try
            Dim _td As String = ""
            If (_tipohora = "PM") Then
                If (_horainicio = 12) Then
                    _horainicio = 12
                Else
                    _horainicio = _horainicio + 12

                End If
            End If

            If (_tipohora = "AM" And _horainicio = 12) Then
                _horainicio = 0
            End If
            Dim culture As CultureInfo = CultureInfo.GetCultureInfo("es-PE")
            Dim _time As DateTime = DateTime.Parse(_horainicio.ToString() + ":00:00", culture)


            For i As Integer = 1 To _numeracionfrecuencia
                If (i > 1) Then
                    _time = _time.AddHours(_num_frecuencia)
                End If
                _td = _td + "<tr><td>" + _time.ToString("hh:mm:ss tt") + "</td></tr>"
            Next

            Return _td

        Catch ex As Exception
            Return ""
        End Try

    End Function

    <WebMethod>
    Public Shared Function registrar_horas_programadas(peso As Decimal) As String
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString

            Dim obj As KardexHospitalarioE = New KardexHospitalarioE()
            obj.ide_kardexhospitalario = 0
            obj.codatencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            obj.codpaciente = CType(HttpContext.Current.Session(sCodPaciente), String)
            obj.peso = peso
            obj.usr_registra = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            obj.estado = "0"
            obj.eliminado = 0
            obj.order = 1
            obj.fecharegistro = DateTime.Now
            obj.fechainicio = DateTime.Now
            obj.fechafin = DateTime.Now

            Dim _Json As String = JsonConvert.SerializeObject(obj)
            Dim _client As RestClient = New RestClient(Apiruta + "KardexHospitalizacion/API/Clinica/RegistroKardexCabecera")
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.POST
            _request.AddHeader("Accept", "application/json")
            _request.AddJsonBody(obj)
            Dim _Response As RestResponse = _client.Execute(_request)
            Dim _JsonGet As RespuestaJsonE = JsonConvert.DeserializeObject(Of RespuestaJsonE)(_Response.Content)

            Return _JsonGet.Message
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function registrar_programacion_horas_N(ide_kardexhospitalario As Integer, horaInicio As Integer, ide_MedicamentoRec As Integer) As String
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString
            Dim obj As IndicacionesMedicasE = New IndicacionesMedicasE()

            obj.UserRegistro = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            obj.HoraSecundaria = horaInicio
            obj.horaAtencion = "PROG"
            obj.i_horasugerida = 3
            obj.ide_KardexHospitalario = ide_kardexhospitalario
            obj.cod_atencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            obj.IdTipo = 3
            obj.NombreTipo = "Farmacologico"
            obj.ide_medicamentorec = ide_MedicamentoRec

            'Dim _Json As String = JsonConvert.SerializeObject(obj)
            Dim _client As RestClient = New RestClient(Apiruta + "KardexHospitalizacion/API/Clinica/RegistroKardexDetalle")
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.POST
            _request.AddHeader("Accept", "application/json")
            _request.AddJsonBody(obj)
            Dim _Response As RestResponse = _client.Execute(_request)
            Dim _JsonGet As RespuestaJsonE = JsonConvert.DeserializeObject(Of RespuestaJsonE)(_Response.Content)
            Return _JsonGet.Message
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function RegistrarSeleccionKardexHospitalario(IdeMedicamentorec As Integer, IdeReceta As Integer, CodAtencion As String, IdeTipo As Integer, NombreTipos As String, IdeKardexHospitalario As Integer) As String
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString
            Dim obj As IndicacionesMedicasE = New IndicacionesMedicasE()

            obj.UserRegistro = CType(HttpContext.Current.Session(ScodigoAccesoLogin), String)
            obj.HoraSecundaria = 0
            obj.horaAtencion = ""
            obj.i_horasugerida = 0
            obj.ide_KardexHospitalario = IdeKardexHospitalario
            obj.cod_atencion = CType(HttpContext.Current.Session(sCodigoAtencion), String)
            obj.IdTipo = IdeTipo
            obj.NombreTipo = NombreTipos
            obj.ide_medicamentorec = IdeMedicamentorec

            Dim _Json As String = JsonConvert.SerializeObject(obj)
            Dim _client As RestClient = New RestClient(Apiruta + "KardexHospitalizacion/API/Clinica/RegistroKardexDetalle")
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.POST
            _request.AddHeader("Accept", "application/json")
            _request.AddJsonBody(obj)
            Dim _Response As RestResponse = _client.Execute(_request)
            Dim _JsonGet As RespuestaJsonE = JsonConvert.DeserializeObject(Of RespuestaJsonE)(_Response.Content)
            Return _JsonGet.Message
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function MostrarDastosHistoricosKardexEnfermeria(Valor1 As String, Valor2 As String) As String
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString
            Dim _List As List(Of IndicacionesMedicaDetalleE) = New List(Of IndicacionesMedicaDetalleE)

            _List = ObtenerDatoshistoricosKardexEnfermeria(Valor1, Valor2)

            Dim fechacabecera As List(Of IndicacionesMedicaDetalleE) = (From n In _List Group n By n.Fecha, n.peso Into Group Select New IndicacionesMedicaDetalleE With {.Fecha = Fecha, .peso = peso}).ToList()
            Dim olistTipo As List(Of IndicacionesMedicaDetalleE) = (From n In _List Group n By n.Fecha, n.i_Idtipo, n.dsc_tipo Into Group Select New IndicacionesMedicaDetalleE With {.Fecha = Fecha, .i_Idtipo = i_Idtipo, .dsc_tipo = dsc_tipo}).ToList()

            '


            Dim _trPrincipal As String = ""

            For Each _item As IndicacionesMedicaDetalleE In fechacabecera
                Dim _trSecundarioinicio As String = "<tr><td><div><label>" + _item.Fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "</label></div><div>Peso: " + _item.peso.ToString() + " KG</div></td><td>"
                Dim aperturaHtml1Inicio As String = "<table class='JSBTABLA JSTABLASCALA' style='font-size: 15px;'> <tbody>"
                Dim tdvalor1 As String = ""

                For Each _item1 As IndicacionesMedicaDetalleE In (From n In olistTipo Where n.Fecha = _item.Fecha Select n).ToList()
                    Dim _tr1inicio As String = "<tr><td><div><label>" + _item1.dsc_tipo.ToString() + "</label></div></td><td>"
                    Dim tdvalor2 As String = ""

                    Dim oListGeneralCabaeceraDetalle As List(Of IndicacionesMedicaDetalleE) = New List(Of IndicacionesMedicaDetalleE)()
                    oListGeneralCabaeceraDetalle = (From n In _List Where n.Fecha = _item1.Fecha And n.i_Idtipo = _item1.i_Idtipo Group n By n.ide_medicamentorec, n.dsc_producto Into Group Select New IndicacionesMedicaDetalleE With {.ide_medicamentorec = ide_medicamentorec, .dsc_producto = dsc_producto}).ToList()

                    For Each _item2 As IndicacionesMedicaDetalleE In oListGeneralCabaeceraDetalle
                        Dim aperturaHtml2Inicio As String = "<table class='JSBTABLA JSTABLASCALA' style='font-size: 15px;'><thead><tr><th>" + _item2.dsc_producto + "</th></tr></thead> <tbody>"

                        Dim tdvalor3 As String = ""
                        Dim oListGeneralDetalle As List(Of IndicacionesMedicaDetalleE) = New List(Of IndicacionesMedicaDetalleE)()
                        oListGeneralDetalle = (From n In _List Where n.ide_medicamentorec = _item2.ide_medicamentorec Order By n.dsc_producto, n.i_Correlativo Select n).ToList()

                        For Each _item3 As IndicacionesMedicaDetalleE In oListGeneralDetalle
                            Dim _tr3inicio As String = "<tr><td>"
                            Dim _label As String = ""
                            If (_item3.i_Idtipo = 3 And _item3.dsc_tipoAdminstracio = "") Then
                                _label = "<label style='font-weight:bold;'>Administración N°:&ensp;</label>	" + _item3.i_Correlativo.ToString() + "  <label>&ensp;</label><label style='font-weight:bold;'>Fecha y hora Programada:&ensp;</label> " + _item3.fechaprogramada.ToString() + " <label>&ensp;</label> " + _item3.HInsert.ToString() + " <label>&ensp;</label><label style='font-weight:bold;'>Encargado(a) Programación:&ensp;</label> " + _item3.usr_registra.ToString() + " <label>&ensp;</label> <label style='font-weight:bold;'>Fecha y hora administración:&ensp;</label> " + _item3.fechaadministrada.ToString() + " <label>&ensp;</label> <label style='font-weight:bold;'>Encargado(a) Administración:&ensp;</label> " + _item3.usr_adminstra.ToString() + " <label>&ensp;</label>  "
                            Else
                                _label = "<label style='font-weight:bold;'>Administración N°:&ensp;</label> " + _item3.i_Correlativo.ToString() + "  <label>&ensp;</label> <label style='font-weight:bold;'>Encargado(a):&ensp;</label> " + _item3.usr_registra.ToString() + " <label>&ensp;</label> <label style='font-weight:bold;'>Fecha y hora atención:&ensp;</label>  " + _item3.FInsert.ToString() + "  <label>&ensp;</label>  " + _item3.HInsert.ToString()
                                If (_item3.dsc_tipoAdminstracio <> "") Then
                                    _label = _label + "<label>&ensp;</label><label>&ensp;</label><label style='font-weight:bold; color:darkred;'>( " + _item3.dsc_tipoAdminstracio.ToString() + " )</label>"
                                End If
                            End If
                            Dim _tr3fin As String = "</td></tr>"
                            tdvalor3 = tdvalor3 + _tr3inicio + _label + _tr3fin
                        Next

                        Dim aperturaHtml2final As String = "</tbody></table>"
                        tdvalor2 = tdvalor2 + aperturaHtml2Inicio + tdvalor3 + aperturaHtml2final
                    Next

                    Dim _tr1fin As String = "</td></tr>"
                    tdvalor1 = tdvalor1 + _tr1inicio + tdvalor2 + _tr1fin
                Next

                Dim aperturaHtml1final As String = "</tbody></table>"
                Dim _trSecundariofin As String = "</td></tr>"

                _trPrincipal = _trPrincipal + _trSecundarioinicio + aperturaHtml1Inicio + tdvalor1 + aperturaHtml1final + _trSecundariofin
            Next

            Return "<table class='JSBTABLA JSTABLASCALA' style='font-size: 15px;'> <tbody>" + _trPrincipal + "</tbody></table>"

        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Shared Function ObtenerDatoshistoricosKardexEnfermeria(valor1 As String, valor2 As String) As List(Of IndicacionesMedicaDetalleE)
        Try
            Dim _Encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString
            Dim _list As List(Of IndicacionesMedicaDetalleE) = New List(Of IndicacionesMedicaDetalleE)
            Dim rutaApi As String = Apiruta
            Dim codatencio As String = CType(HttpContext.Current.Session(sCodigoAtencion), String)

            Dim Busq As String = valor1 + "_" + valor2


            Dim _cliente As RestClient = New RestClient(rutaApi + "KardexHospitalizacion/API/Clinica/KardexHistoricoPacienteHospitalario2?_Key=" + _Encript.EncryptConectionString(codatencio) + "&valor=" + _Encript.EncryptConectionString(Busq))
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            _list = JsonConvert.DeserializeObject(Of List(Of IndicacionesMedicaDetalleE))(_Response.Content)
            Return _list
        Catch ex As Exception
            Return New List(Of IndicacionesMedicaDetalleE)
        End Try
    End Function

    'INI 1.2
    Public Function ObtenerUrlRoeVersion2(ByVal empresa As String, ByVal tipoDoc As String, ByVal numDoc As String)
        Try
            Dim _encript As Criptography = New Criptography()
            Dim Apiruta As String = ConfigurationManager.ConnectionStrings("ApiClinica").ConnectionString
            ' se obtiene la url del api clinica
            Dim rutaApi As String = Apiruta
            Dim _keydata = empresa + ";" + tipoDoc + ";" + numDoc
            Dim url As String = rutaApi + "DatosControlWeb/API/Clinica/DatoValidarAccesoROE?_Key=" + _keydata
            Dim _header = _encript.EncryptConectionString(_encript._ApiCLinica)
            Dim _cliente As New RestClient(url)
            Dim _request As RestRequest = New RestRequest()
            _request.AddHeader("Authorization", _header)
            _request.Method = Method.GET
            Dim _Response As RestResponse = _cliente.Execute(_request)
            Dim JsonGet As List(Of String) = JsonConvert.DeserializeObject(Of List(Of String))(_Response.Content)

            Dim Data = JsonGet

            Return Data

        Catch ex As Exception
            Dim message As String = ex.Message
        End Try
    End Function
    'FIN 1.2

    <WebMethod>
    Public Shared Function MostrarTimeOutInactividad() As Int32
        Return ObtenerTimeOutInactividad()
    End Function
    Private Shared Function ObtenerTimeOutInactividad() As Int32
        Try
            Dim objtabla As TablasE = New TablasE()
            objtabla.CodTabla = "HCEINACTIVIDADTIME"
            objtabla.Buscar = ""
            objtabla.Key = 0
            objtabla.NumeroLineas = 0
            objtabla.Orden = 24
            Dim tabla_ As New DataTable()
            Dim oTablasLN As New TablasLN()
            tabla_ = oTablasLN.Sp_Tablas_Consulta(objtabla)
            If tabla_.Rows.Count() > 0 Then
                Dim time As Int32 = 0
                For Each row As DataRow In tabla_.Rows
                    time = Convert.ToInt32(row("valor").ToString())
                Next
                Return time
            Else
                Return 0
            End If

        Catch ex As Exception
            Return 0
        End Try
    End Function

End Class