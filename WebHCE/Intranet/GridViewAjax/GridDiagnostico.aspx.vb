Imports System.Data
Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN
Imports System.Configuration

Public Class GridDiagnostico
    Inherits System.Web.UI.Page
    Dim oRceDiagnosticoE As New RceDiagnosticoE()
    Dim oRceDiagnosticoLN As New RceDiagnosticoLN()
    Dim nNumeroPagina As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListarDiagnosticos()
        End If
    End Sub

    Public Sub ListarDiagnosticos()
        oRceDiagnosticoE.Tipo = "A"
        oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
        Dim tabla As New DataTable()
        If IsNothing(Request.Params("Pagina")) Then
            gvDiagnosticos.PageIndex = 0
            tabla = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)
            gvDiagnosticos.DataSource = tabla
            gvDiagnosticos.DataBind()
        Else
            nNumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
            gvDiagnosticos.PageIndex = (nNumeroPagina - 1)
            tabla = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)
            gvDiagnosticos.DataSource = tabla
            gvDiagnosticos.DataBind()
        End If
    End Sub


    Protected Sub gvDiagnosticos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDiagnosticos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(5).Text.Trim() = "P" Then
                Dim ctrl As New System.Web.UI.HtmlControls.HtmlInputRadioButton
                ctrl = CType(e.Row.FindControl("chkPresuntivo"), System.Web.UI.HtmlControls.HtmlInputRadioButton)
                If Not IsNothing(ctrl) Then
                    ctrl.Checked = True
                End If
            End If
            If e.Row.Cells(5).Text.Trim() = "R" Then
                Dim ctrl As New System.Web.UI.HtmlControls.HtmlInputRadioButton
                ctrl = CType(e.Row.FindControl("chkRepetido"), System.Web.UI.HtmlControls.HtmlInputRadioButton)
                If Not IsNothing(ctrl) Then
                    ctrl.Checked = True
                End If
            End If
            If e.Row.Cells(5).Text.Trim() = "D" Then
                Dim ctrl As New System.Web.UI.HtmlControls.HtmlInputRadioButton
                ctrl = CType(e.Row.FindControl("chkDefinitivo"), System.Web.UI.HtmlControls.HtmlInputRadioButton)
                If Not IsNothing(ctrl) Then
                    ctrl.Checked = True
                End If
            End If
        End If
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function ActualizarTipoDiagnostico(ByVal TipoDiagnostico As String, ByVal CodigoDiagnostico As String, ByVal TipoIE As String) As String
        Dim pagina As New GridDiagnostico()
        Return pagina.ActualizarTipoDiagnostico_(TipoDiagnostico, CodigoDiagnostico, TipoIE)
    End Function

    Public Function ActualizarTipoDiagnostico_(ByVal TipoDiagnostico As String, ByVal CodigoDiagnostico As String, ByVal TipoIE As String) As String
        Try
            oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
            oRceDiagnosticoE.Tipo = IIf(TipoIE = "I", "E", "S")   'sTipoD JB - ESTE VALOR AHORA VIENE DE LA PANTALLA I-INGRESO E-EGRESO   / en pantalla esta como ingreso(I) o egreso(E), en la BD esta como Entrada(E) o Salida(S)
            oRceDiagnosticoE.CodDiagnostico = CodigoDiagnostico
            oRceDiagnosticoE.Campo = "tipodiagnostico"
            oRceDiagnosticoE.NuevoValor = TipoDiagnostico
            Dim Actualizo As Boolean = oRceDiagnosticoLN.Sp_Diagxhospital_Update(oRceDiagnosticoE)
            If Actualizo = True Then
                Dim pagina As New InformacionPaciente()
                pagina.GuardarLog_("DIAGNOSTICO", "Se cambio clasificacion al diagnostico " + CodigoDiagnostico)

                Return "OK"
            Else
                Return ConfigurationManager.AppSettings(sMensajeActualizarError).ToString().Trim() + " - Tipo de Diagnóstico"
            End If
        Catch ex As Exception
            Return ex.Message.ToUpper()
        End Try
    End Function


    ''' <summary>
    ''' FUNCION PARA ELIMINAR DIAGNOSTICO
    ''' </summary>
    ''' <param name="CodigoDiagnostico"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarDiagnostico(ByVal CodigoDiagnostico As String, ByVal TipoD As String) As String
        Dim pagina As New GridDiagnostico()
        Return pagina.EliminarDiagnostico_(CodigoDiagnostico, TipoD)
    End Function

    Public Function EliminarDiagnostico_(ByVal CodigoDiagnostico As String, ByVal TipoD As String) As String
        oRceDiagnosticoE.CodDiagnostico = CodigoDiagnostico
        oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
        oRceDiagnosticoE.Tipo = IIf(TipoD = "I", "E", "S") 'sTipoD 19/01/2017 - comentado, el parametro se obtiene del grip y no de la variable global
        Try
            Dim Elimino As Boolean = oRceDiagnosticoLN.Sp_Diagxhospital_Delete(oRceDiagnosticoE)

            If Elimino = False Then
                Dim pInformacionPaciente As New InformacionPaciente()
                pInformacionPaciente.GuardarLog_("DIAGNOSTICO", "Se elimino el diagnostico " + CodigoDiagnostico)
                Return "No se pudo Eliminar el Registro"
            Else
                Dim pInformacionPaciente As New InformacionPaciente()
                pInformacionPaciente.GuardarLog_("DIAGNOSTICO", "Se elimino el diagnostico " + CodigoDiagnostico)
                Return ""
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

End Class