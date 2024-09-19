Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN
Imports Entidades.MedicamentosE
Imports LogicaNegocio.MedicamentosLN

Imports System.Data.SqlClient
Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN

Public Class Medicamentos
    Inherits System.Web.UI.Page
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()
    Dim oRceMedicamentosE As New RceMedicamentosE()
    Dim oRceMedicamentosLN As New RceMedicamentosLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Not IsNothing(Request.Params("Parametro[]")) And Request.Params("Parametro[]") <> Nothing Then
                IdExamenFisicoCores.Value = Request.Params("Parametro[]").Trim().Split(",")(0)
                DescripcionExamenFisicoCore.Value = Request.Params("Parametro[]").Trim().Split(",")(1)
            End If
        End If

    End Sub

    ''' <summary>
    ''' FUNCION PARA VALIDAR SI EL PACIENTE ES ALERGICO AL PRODUCTO SELECCIONADO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAlergiaPaciente(ByVal CodigoProducto As String) As String
        Dim pagina As New Medicamentos()
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
    ''' FUNCION PARA OBTENER LOS DATOS DE UN REGISTRO SELECCIONADO
    ''' </summary>
    ''' <param name="IdMedicamentosaDet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerMedicamentosaDet(ByVal IdMedicamentosaDet As Integer) As String
        Dim pagina As New Medicamentos()
        Return pagina.ObtenerMedicamentosaDet_(IdMedicamentosaDet)
    End Function

    Public Function ObtenerMedicamentosaDet_(ByVal IdMedicamentosaDet As Integer) As String
        Dim tabla As New DataTable()
        oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
        oRceMedicamentosE.Orden = 2
        oRceMedicamentosE.IdMedicamentosaDet = IdMedicamentosaDet
        tabla = oRceMedicamentosLN.Sp_RceMedicamentosaCab_Consulta(oRceMedicamentosE)
        Dim DatosDevolver As String = ""

        If tabla.Rows.Count > 0 Then
            For index = 0 To tabla.Columns.Count - 1
                DatosDevolver += tabla.Rows(0)(index).ToString().Trim() + ";"
            Next
        End If

        If DatosDevolver <> "" Then
            DatosDevolver = DatosDevolver.Substring(0, DatosDevolver.LastIndexOf(";"))
        End If

        Return DatosDevolver
        'ide_medicamentosa_cab;cod_medico;medico;cod_producto;dsc_producto;dsc_via;num_dosis;num_frecuencia;fec_ultima;hor_ultima;cod_accion;dsc_accion;fec_modifica
    End Function



    ''' <summary>
    ''' FUNCION PARA GUARDAR RECONCILIACION MEDICAMENTOSA
    ''' </summary>
    ''' <param name="CodigoProducto"></param>
    ''' <param name="NombreProducto"></param>
    ''' <param name="Dosis"></param>
    ''' <param name="Via"></param>
    ''' <param name="Frecuencia"></param>
    ''' <param name="IdeExamenFisicoCore"></param>
    ''' <param name="CodigoAccion"></param>
    ''' <param name="FechaUltima"></param>
    ''' <param name="HoraUltima"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarReconciliacionMedicamentosa(ByVal CodigoProducto As String, ByVal NombreProducto As String, ByVal Dosis As String, _
                                                              ByVal Via As String, ByVal Frecuencia As String, ByVal IdeExamenFisicoCore As String, _
                                                              ByVal CodigoAccion As String, ByVal FechaUltima As String, ByVal HoraUltima As String, ByVal IdMedicamentosaDet As String, ByVal PortaMedicamento As String) As String
        Dim pagina As New Medicamentos()
        Return pagina.GuardarReconciliacionMedicamentosa_(CodigoProducto, NombreProducto, Dosis, Via, Frecuencia, IdeExamenFisicoCore, _
                                                              CodigoAccion, FechaUltima, HoraUltima, IdMedicamentosaDet, PortaMedicamento)
    End Function

    Public Function GuardarReconciliacionMedicamentosa_(ByVal CodigoProducto As String, ByVal NombreProducto As String, ByVal Dosis As String, _
                                                              ByVal Via As String, ByVal Frecuencia As String, ByVal IdeExamenFisicoCore As String, _
                                                              ByVal CodigoAccion As String, ByVal FechaUltima As String, ByVal HoraUltima As String, ByVal IdMedicamentosaDet As String, ByVal PortaMedicamento As String) As String

        Try
            Dim mensaje As String = ""

            If NombreProducto = "" Or Dosis = "" Or Via = "" Or Frecuencia = "" Or IdeExamenFisicoCore = "" Or CodigoAccion = "" Or FechaUltima = "" Or HoraUltima = "" Then
                mensaje = "VALIDACION;Ingrese los campos en rojo."
                Return mensaje
            End If


            If IdMedicamentosaDet = "" Then 'SI ES NUEVO REGISTRO INSERTARA
                oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
                oRceMedicamentosE.CodMedico = Session(sCodMedico)
                oRceMedicamentosLN.Sp_RceMedicamentosaCab_Insert(oRceMedicamentosE)

                If oRceMedicamentosE.IdMedicamentosaCab <> 0 Then
                    oRceMedicamentosLN.Sp_RceMedicamentosaDet_Insert(oRceMedicamentosE)
                End If
            Else
                oRceMedicamentosE.IdMedicamentosaDet = CType(IdMedicamentosaDet, Integer)
            End If

            If CodigoProducto <> "" Then
                oRceMedicamentosE.Campo = "cod_producto"
                oRceMedicamentosE.ValorNuevo = CodigoProducto
                oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)
            End If

            oRceMedicamentosE.Campo = "dsc_producto"
            oRceMedicamentosE.ValorNuevo = NombreProducto.Trim().ToUpper()
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "num_dosis"
            oRceMedicamentosE.ValorNuevo = Dosis.Trim().ToUpper()
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "dsc_via"
            oRceMedicamentosE.ValorNuevo = Via.Trim().ToUpper()
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "num_frecuencia"
            oRceMedicamentosE.ValorNuevo = Frecuencia.Trim().ToUpper()
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "ide_examenfisicores"
            oRceMedicamentosE.ValorNuevo = IdeExamenFisicoCore.Trim().ToUpper()
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "cod_accion"
            oRceMedicamentosE.ValorNuevo = CodigoAccion
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "fec_ultima"
            'Dim fecha As DateTime
            'fecha = CType(FechaUltima, DateTime)
            oRceMedicamentosE.ValorNuevo = FechaUltima
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "hor_ultima"
            oRceMedicamentosE.ValorNuevo = HoraUltima
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "fec_modifica"
            oRceMedicamentosE.ValorNuevo = ""
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            oRceMedicamentosE.Campo = "usr_modifica"
            oRceMedicamentosE.ValorNuevo = Session(sCodUser)
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)

            '16/09/2016
            oRceMedicamentosE.Campo = "flg_medicamento"
            oRceMedicamentosE.ValorNuevo = PortaMedicamento
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)


            If mensaje = "" Then
                mensaje = "OK;" + oRceMedicamentosE.IdMedicamentosaDet.ToString()
            End If

            Return mensaje
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
        If IsNothing(Session(sCodMedico)) Then
            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function


    'INICIO - JB - 10/02/2017
    <System.Web.Services.WebMethod()>
    Public Shared Function GenerarPDF() As String
        Dim pagina As New Medicamentos()
        Return pagina.GenerarPDF_()
    End Function

    Public Function GenerarPDF_() As String
        Try
            Dim CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
            Dim oHospitalLN As New HospitalLN()
            Dim oHospitalE As New HospitalE()

            Dim cn As New SqlConnection(CnnBD)

            Dim pagina As New InformacionPaciente() '31/01/2017
            Dim pdf_byte As Byte() = pagina.ExportaPDF("ME") '31/01/2017

            'Paso 1
            oHospitalE.TipoDoc = 11
            oHospitalE.CodAtencion = Session(sCodigoAtencion)
            oHospitalE.CodUser = Session(sCodUser)
            oHospitalE.Descripcion = Session(sIdeHistoria)
            oHospitalLN.Sp_HospitalDoc_Insert(oHospitalE)

            'Paso 2
            Dim cmd1 As New SqlCommand("update hospital_doc set bib_documento=@bib_documento, extension_doc='PDF' where id_documento=@id_documento", cn)
            cmd1.CommandType = CommandType.Text
            cmd1.Parameters.AddWithValue("@bib_documento", pdf_byte) 'INICIO - JB - 31/01/2017
            cmd1.Parameters.AddWithValue("@id_documento", oHospitalE.IdDocumento)
            Dim num1 As Integer
            cn.Open()
            num1 = cmd1.ExecuteNonQuery()
            cn.Close()

            'Paso 3
            oHospitalE.IdeHistoria = Session(sIdeHistoria)
            oHospitalE.TipoDoc = 11
            oHospitalE.IdeGeneral = Session(sIdeHistoria)
            oHospitalLN.Sp_RceHospitalDoc_Insert(oHospitalE)

            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function
    'FIN - JB - 30/02/2017

End Class