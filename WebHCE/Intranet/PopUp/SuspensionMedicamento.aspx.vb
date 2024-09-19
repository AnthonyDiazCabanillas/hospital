Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class SuspensionMedicamento
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
    Public Shared Function SuspenderMedicamento() As String
        Dim pagina As New SuspensionMedicamento()
        Return pagina.SuspenderMedicamento_()
    End Function

    Public Function SuspenderMedicamento_() As String
        Try
            'JB - COMENTADO - 05/05/2021
            'oRceRecetaMedicamentoE.IdRecetaDet = IdMedicamentoRec
            'oRceRecetaMedicamentoE.CodMedico = Session(sCodMedico)
            'Dim ejecutado As Integer
            'ejecutado = oRceRecetaMedicamentoLN.Sp_RceRecetaSuspension_Insert(oRceRecetaMedicamentoE)
            'Return "OK"

            If Not IsNothing(Session(sTablaMedicamentosSuspender)) Then
                Dim tabla As New DataTable()
                tabla = CType(Session(sTablaMedicamentosSuspender), DataTable)
                If tabla.Rows.Count > 0 Then
                    For index = 0 To tabla.Rows.Count - 1
                        Dim ejecutado As Integer
                        oRceRecetaMedicamentoE.IdRecetaDet = tabla.Rows(index)("IdeMedicamento").ToString().Trim()
                        oRceRecetaMedicamentoE.CodMedico = Session(sCodMedico)
                        ejecutado = oRceRecetaMedicamentoLN.Sp_RceRecetaSuspension_Insert(oRceRecetaMedicamentoE)
                    Next
                End If
            End If
            Session.Remove(sTablaMedicamentosSuspender)
            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function




    <System.Web.Services.WebMethod()>
    Public Shared Function CancelaSuspensinMedicamento() As String
        Dim pagina As New SuspensionMedicamento()
        Return pagina.CancelaSuspensinMedicamento_()
    End Function

    Public Function CancelaSuspensinMedicamento_() As String
        Session.Remove(sTablaMedicamentosSuspender)
        Return "OK"
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