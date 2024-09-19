Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class VerificarMedicamento
    Inherits System.Web.UI.Page
    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Params("Parametro[]") Then
                hfIdReceta.Value = Request.Params("Parametro[]").Trim().Split(",")(0)
                CargarDatosCabecera()
            End If
        End If
    End Sub

    Public Sub CargarDatosCabecera()
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdRecetaDet = 0
        oRceRecetaMedicamentoE.Orden = 10 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
        Dim tabla As New DataTable()
        tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        For index = 0 To tabla.Rows.Count - 1
            If tabla.Rows(index)("ide_receta").ToString().Trim() = hfIdReceta.Value.Trim() Then
                spDatosCabecera.InnerHtml = tabla.Rows(index)("nmedico").ToString().ToUpper().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().ToUpper().Trim()
            End If
        Next
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function VerificarMedicamento(ByVal IdMedicamentoSuspendido As String) As String
        Dim pagina As New VerificarMedicamento()
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
            oRceRecetaMedicamentoE.ValorNuevo = Format(CDate(Date.Now), "MM/dd/yyyy h:mm:ss") ' Date.Now.ToString()  'Format(CDate(Date.Now), "dd/MM/yyyy h:mm:ss")
            oRceRecetaMedicamentoLN.Sp_RceRecetaSuspension_Update(oRceRecetaMedicamentoE)

            Return "OK"

        Catch ex As Exception
            Return ex.Message.ToString()
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

End Class