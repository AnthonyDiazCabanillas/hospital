Public Class ReporteHM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function CerrarReporteHM() As String
        Dim pagina As New ReporteHM()
        Return pagina.CerrarReporteHM_()
    End Function

    Public Function CerrarReporteHM_() As String
        Dim pagina As New VisorReporteHM()
        pagina.CerrarReporteHM()
        Return ""
    End Function

End Class