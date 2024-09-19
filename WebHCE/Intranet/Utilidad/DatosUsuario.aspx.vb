' **********************************************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2023. Todos los derechos reservados.
'    Version     Fecha           Autor       Requerimiento
'    1.1         20/10/2023      AROMERO     REQ-2023-017255:  REPORTE HISTORIA CLINICA HOPITAL
'***********************************************************************************************************************
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Public Class DatosUsuario
    Inherits System.Web.UI.Page
    Dim oHospitalLN As New HospitalLN
    Dim oHospitalE As New HospitalE
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ConsultaPacienteHospitalizado()
        End If
    End Sub

    Public Sub ConsultaPacienteHospitalizado()
        'SI ES ORDEN 3, SE USARA EL PARAMETRO NombrePaciente PARA ENVIAR EL CODIGO DE ATENCION
        oHospitalE.NombrePaciente = Session(sCodigoAtencion_Auxiliar) '21/06/2016
        oHospitalE.Pabellon = ""
        oHospitalE.Servicio = ""
        oHospitalE.Orden = 3
        Dim tabla As New DataTable()
        tabla = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)


        If Not IsNothing(Request.Params("Parametro")) Then
            If Request.Params("Parametro").Trim() = "ALERGIA" Then
                spSpanDNI.Visible = True
                spDatosDNIPaciente.Visible = True
            End If
        End If


        If tabla.Rows.Count > 0 Then
            spDatosNombreApellido.InnerText = tabla.Rows(0)("nombres").ToString().Trim()
            spDatosEdad.InnerText = tabla.Rows(0)("edad").ToString().Trim()
            spDatosIngresoxEmergencia.InnerText = tabla.Rows(0)("FechaIngresoEmergencia").ToString().Trim()
            spDatosHC.InnerText = tabla.Rows(0)("codpaciente").ToString().Trim()
            Session(sCodPaciente) = tabla.Rows(0)("codpaciente").ToString().Trim()
            spDatosCodigoAtencion.InnerText = tabla.Rows(0)("codatencion").ToString().Trim()
            spDatosIngresoHabitacion.InnerText = tabla.Rows(0)("FechaIngresoHabitacion").ToString().Trim()
            spDatosAseguradora.InnerText = tabla.Rows(0)("NombreAseguradora").ToString().Trim()
            spDatosFonoContacto.InnerText = tabla.Rows(0)("telefono").ToString().Trim()
            spDatosMedico.InnerText = tabla.Rows(0)("NombreMedico").ToString().Trim()
            spDatosEspecialidad.InnerText = tabla.Rows(0)("especialidad").ToString().Trim()
            spDatosSexo.InnerText = tabla.Rows(0)("sexo").ToString().Trim()
            spDatosLugarNacimiento.InnerText = tabla.Rows(0)("LugarNacimiento").ToString().Trim()
            spDiasHospitalizado.InnerText = tabla.Rows(0)("DiasHospitalizados").ToString().Trim()
            '1.1 INI
            spDatosEstadoCivil.InnerText = IIf(tabla.Rows(0)("codcivil").ToString().Trim() = "C", "CASADO", "SOLTERO")
            '1.1 FIN
            'INICIO PHERMENEGILDO 14/02/2023
            spDatosDireccion.InnerText = tabla.Rows(0)("direccion").ToString().Trim()
            'FIN PHERMENEGILDO 14/02/2023


            If tabla.Rows(0)("DiasHospitalizados").ToString().Trim() < 2 Then
                spDiasHospitalizado.Style.Add("color", "#0051CB")
                spTextoDiasHospitalizado.Style.Add("color", "#0051CB")

                divBordeHospitalizado.Style.Add("border", "1px solid #0051CB")
            End If
            If tabla.Rows(0)("DiasHospitalizados").ToString().Trim() > 1 And tabla.Rows(0)("DiasHospitalizados").ToString().Trim() < 8 Then
                spDiasHospitalizado.Style("color") = "#8DC73F"
                spTextoDiasHospitalizado.Style("color") = "#8DC73F"

                divBordeHospitalizado.Style("border") = "1px solid #8DC73F"
            End If
            If tabla.Rows(0)("DiasHospitalizados").ToString().Trim() > 7 Then
                spDiasHospitalizado.Style("color") = "Red"
                spTextoDiasHospitalizado.Style("color") = "Red"

                divBordeHospitalizado.Style("border") = "1px solid Red"
            End If
            spDatosHabitacion.InnerText = tabla.Rows(0)("cama").ToString().Trim()
            spDatosDNIPaciente.InnerText = tabla.Rows(0)("docidentidad").ToString().Trim() '04/08/2016 - DEBE MOSTRARSE SOLO EN PANTALLA DE DECLARATORIA ALERGIA


            If tabla.Rows(0)("Flg_alergia") = True Then
                spPresentaAlergia.InnerText = "Presenta Alergia"
                'divPresentaAlergia.Visible = True
                divPresentaAlergia.Style("display") = "block"
            Else
            End If
        End If
        'INICIO - JB - nuevo codigo - 07/08/2019
        Dim tabla1 As New DataTable()
        oTablasE.IdeHistoria = Session(sIdeHistoria)
        oTablasE.Orden = -1
        oTablasE.IdeUsuario = Session(sCodUser)
        oTablasE.IdeAlerta = 0
        tabla1 = oTablasLN.Sp_RceAlerta(oTablasE)
        Dim Cantidad As String = ""
        Dim STipoUsuario As String = ""
        If tabla1.Rows(0)("tipo_usuario").ToString().Trim() <> "U" Then 'INICIO - JB - nuevo condicion - 05/06/2020
            If tabla1.Rows.Count > 0 Then
                If CType(tabla1.Rows(0)(0).ToString(), Integer) > 0 Then
                    spExamenPendienteVerificarVisualizar.InnerText = "Resultados Pendientes"
                    divPresentaExamenVerificarVisualizar.Visible = True
                End If
            End If
        End If
        'FIN - JB - nuevo codigo - 07/08/2019
    End Sub
End Class