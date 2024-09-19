Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Public Class AltaMedica
    Inherits System.Web.UI.Page
    Dim oHospitalLN As New HospitalLN
    Dim oHospitalE As New HospitalE
    Dim oMedicoE As New MedicoE
    Dim oMedicoLN As New MedicoLN
    Dim oTablasE As New TablasE
    Dim oTablasLN As New TablasLN

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ConsultaPacienteHospitalizado()
            LlenarCombos()
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

        If tabla.Rows.Count > 0 Then
            spDatosNombreApellido.InnerText = tabla.Rows(0)("nombres").ToString().Trim()
            Session(sCodPaciente) = tabla.Rows(0)("codpaciente").ToString().Trim()
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
            spNombreMedico.InnerText = dt.Rows(0)("nombres").ToString().Trim()
        End If
    End Sub


    Public Sub LlenarCombos()
        oTablasE.CodTabla = "TIPODESTINO"
        oTablasE.Buscar = ""
        oTablasE.Key = 34
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = -1
        Dim tabla As New DataTable()
        tabla = oTablasLN.Sp_Tablas_Consulta(oTablasE)
        cbDestinoAltaMedica.DataSource = tabla
        cbDestinoAltaMedica.DataValueField = "codigo"
        cbDestinoAltaMedica.DataTextField = "nombre"
        cbDestinoAltaMedica.DataBind()

        oTablasE.CodTabla = "CONDICION_ALTA"
        oTablasE.Buscar = ""
        oTablasE.Key = 34
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = -1
        tabla = oTablasLN.Sp_Tablas_Consulta(oTablasE)
        cbCondicionAltaMedica.DataSource = tabla
        cbCondicionAltaMedica.DataValueField = "codigo"
        cbCondicionAltaMedica.DataTextField = "nombre"
        cbCondicionAltaMedica.DataBind()

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function DarAltaMedica(ByVal CodigoDestino As String, ByVal CondicionAlta As String, ByVal Necropcia As String) As String
        Dim pagina As New AltaMedica()
        Return pagina.DarAltaMedica_(CodigoDestino, CondicionAlta, Necropcia)
    End Function


    Public Function DarAltaMedica_(ByVal CodigoDestino As String, ByVal CondicionAlta As String, ByVal Necropcia As String) As String
        Try
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


            If CondicionAlta.Trim() = "F" Then
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

            Return "OK"

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

End Class