Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE

Public Class MedicoAtencion
    Inherits System.Web.UI.Page

    Dim oMedicoE As New MedicoE
    Dim oMedicoLN As New MedicoLN
    Dim CodigoMedico As String = ""
    Dim CodigoAtencion As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Request.Params("Codigo") <> Nothing Then
                CodigoMedico = Request.Params("Codigo").ToString().Trim()
                CodigoAtencion = Request.Params("Atencion").ToString().Trim()
                ObtenerDatosMedico()
            End If

        End If
    End Sub

    Public Sub ObtenerDatosMedico()
        Try
            Dim dt As New DataTable()
            oMedicoE.CodMedico = CodigoMedico
            oMedicoE.Atencion = CodigoAtencion
            oMedicoE.Orden = 1
            dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)

            spMedicoAtencion_PopUp.InnerText = dt.Rows(0)("nombres").ToString().Trim()
            spEspecialidadMedico_PopUp.InnerText = dt.Rows(0)("especialidad").ToString().Trim()
        Catch ex As Exception

        End Try
    End Sub

End Class