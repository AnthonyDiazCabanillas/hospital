Public Class PoputKardexIndicacionesMedicas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Response.Redirect("ConsultaPacienteHospitalizado.aspx")
        End If
        If Not Page.IsPostBack Then

        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSession() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaSession_()
    End Function

    Public Function ValidaSession_() As String
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function

End Class