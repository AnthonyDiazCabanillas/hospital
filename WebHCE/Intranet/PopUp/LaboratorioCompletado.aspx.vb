Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN

Public Class LaboratorioCompletado
    Inherits System.Web.UI.Page
    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Params("Parametro[]") Then
                hfIdReceta.Value = Request.Params("Parametro[]").Trim().Split(",")(0)
            End If
        End If
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function LaboratorioCompletado(ByVal IdReceta As String) As String
        Dim pagina As New LaboratorioCompletado()
        Return pagina.LaboratorioCompletado_(IdReceta)
    End Function

    Public Function LaboratorioCompletado_(ByVal IdReceta As String) As String
        Try
            oRceLaboratioE.IdeRecetaDet = IdReceta
            oRceLaboratioE.Campo = "flg_revisado"
            oRceLaboratioE.ValorNuevo = "1"
            oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)

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