Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE

Public Class DatosUsuarioPopUp
    Inherits System.Web.UI.Page
    Dim oHospitalLN As New HospitalLN
    Dim oHospitalE As New HospitalE
    Dim oMedicoE As New MedicoE
    Dim oMedicoLN As New MedicoLN

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
                'spSpanDNI.Visible = True
                'spDatosDNIPaciente.Visible = True
            End If
        End If

        If tabla.Rows.Count > 0 Then
            spDatosNombreApellido.InnerText = tabla.Rows(0)("nombres").ToString().Trim()
            'spDatosEdad.InnerText = tabla.Rows(0)("edad").ToString().Trim()  18/08/2016
            spDatosIngresoxEmergencia.InnerText = tabla.Rows(0)("FechaIngresoEmergencia").ToString().Trim()
            'spDatosHC.InnerText = tabla.Rows(0)("codpaciente").ToString().Trim()
            Session(sCodPaciente) = tabla.Rows(0)("codpaciente").ToString().Trim()
            'spDatosCodigoAtencion.InnerText = tabla.Rows(0)("codatencion").ToString().Trim()
            'spDatosIngresoHabitacion.InnerText = tabla.Rows(0)("FechaIngresoHabitacion").ToString().Trim()
            'spDatosAseguradora.InnerText = tabla.Rows(0)("NombreAseguradora").ToString().Trim()
            'spDatosFonoContacto.InnerText = tabla.Rows(0)("telefono").ToString().Trim()
            'spDatosMedico.InnerText = tabla.Rows(0)("NombreMedico").ToString().Trim()
            'spDatosEspecialidad.InnerText = tabla.Rows(0)("especialidad").ToString().Trim()
            'spDatosSexo.InnerText = tabla.Rows(0)("sexo").ToString().Trim()
            'spDatosLugarNacimiento.InnerText = tabla.Rows(0)("LugarNacimiento").ToString().Trim()
            'spDiasHospitalizado.InnerText = tabla.Rows(0)("DiasHospitalizados").ToString().Trim()
            'spDatosDNIPaciente.InnerText = tabla.Rows(0)("docidentidad").ToString().Trim() '04/08/2016 - DEBE MOSTRARSE SOLO EN PANTALLA DE DECLARATORIA ALERGIA

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
            spNombreMedico.InnerText = tabla.Rows(0)("NombreMedico").ToString().Trim() 'dt.Rows(0)("nombres").ToString().Trim()
            


            'If tabla.Rows(0)("Flg_alergia") = True Then
            '    spPresentaAlergia.InnerText = "Presenta Alergia"
            '    divPresentaAlergia.Visible = True
            'Else

            'End If
        End If
    End Sub


End Class