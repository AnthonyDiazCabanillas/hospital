Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN
Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE

Public Class InterconsultaRespuesta
    Inherits System.Web.UI.Page
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()
    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()
    Dim oMedicoE As New MedicoE()
    Dim oMedicoLN As New MedicoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ObtieneDatos()
            ListaRespuestaInterconsulta()
        End If
    End Sub

    Public Sub ListaRespuestaInterconsulta()
        oInterconsultaE.Atencion = Session(sCodigoAtencion)
        oInterconsultaE.Orden = 1
        If Not IsNothing(Request.Params("Pagina")) And Request.Params("Pagina") <> Nothing Then
            gvInterconsultaR.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
            gvInterconsultaR.DataSource = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
            gvInterconsultaR.DataBind()
        Else
            gvInterconsultaR.DataSource = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
            gvInterconsultaR.DataBind()
        End If
    End Sub
    ''cambio
    Public Sub ObtieneDatos()
        'LISTANDO MOTIVOS
        oTablasE.CodTabla = "INTERCONSULTA_MOTIVO"
        oTablasE.Buscar = ""
        oTablasE.Key = 0
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = 5
        Dim tabla_ As New DataTable()
        tabla_ = oTablasLN.Sp_Tablas_Consulta(oTablasE)
        ddlMotivoR.DataSource = tabla_
        ddlMotivoR.DataTextField = "nombre"
        ddlMotivoR.DataValueField = "codigo"
        ddlMotivoR.DataBind()

        'CARGANDO PARAMETROS RECIBIDOS 
        If Not IsNothing(Request.Params("Parametro[]")) Then
            hfParametrosInterconsulta.Value = Request.Params("Parametro[]")
            Dim valores As String()
            'Observaciones Cmendez 02/05/2022
            valores = New String(Request.Params("Parametro[]").Split("|").Length) {}
            valores = Request.Params("Parametro[]").Split("|")

            hfIdInterconsultaRespuesta.Disabled = True
            ddlMotivoR.Enabled = False
            txtDescripcionInterconsultaR.Disabled = True
            txtEspecialidadInterconsultaR.Disabled = True
            txtMedicoInterconsultaR.Disabled = True
            hfIdInterconsultaRespuesta.Value = valores(0).Replace(",", "")
            ddlMotivoR.SelectedValue = valores(1).Replace(",", "")
            hfCodEspecialidadInterconsultaR.Value = valores(2).Replace(",", "")
            txtDescripcionInterconsultaR.Value = valores(3).Substring(1, valores(3).Length - 1)
            txtEspecialidadInterconsultaR.Value = valores(4).Replace(",", "")
            txtMedicoInterconsultaR.Value = valores(5).Replace(",", "")


            txtFechaInterconsultRespuesta.Value = Format(Date.Now, "dd/MM/yyyy").ToString()
            txtHoraInterconsultRespuesta.Value = Format(DateAndTime.Now, "hh:mm").ToString()

            If valores.Length > 8 Then 'despues de enviar al respuesta, se recibiran estos valores adicionales
                txtDescripcionInterconsulta2R.Value = valores(6)
                txtFechaInterconsultRespuesta.Value = valores(7)
                txtHoraInterconsultRespuesta.Value = valores(8)
            End If


            txtMedicoInterconsulta2R.Disabled = True
            txtEspecialidadInterconsulta2R.Disabled = True
            txtEspecialidadInterconsulta2R.Value = valores(4).Replace(",", "")

            Dim dt As New DataTable()
            oMedicoE.CodMedico = Session(sCodMedico)
            oMedicoE.Atencion = Session(sCodigoAtencion)
            oMedicoE.Orden = 1
            dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
            txtMedicoInterconsulta2R.Value = dt.Rows(0)("nombres").ToString().Trim()
        End If
    End Sub

    ''' <summary>
    ''' FUNCION PARA GUARDAR RESPUESTA DE INTERCONSULTA
    ''' </summary>
    ''' <param name="Respuesta"></param>
    ''' <param name="IdInterconsultaR"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarRespuestaInterconsulta(ByVal Respuesta As String, ByVal IdInterconsultaR As String, ByVal FechaInterconsultaR As String, ByVal HoraInterconsultaR As String) As String
        Dim pagina As New InterconsultaRespuesta()
        Return pagina.GuardarRespuestaInterconsulta_(Respuesta, IdInterconsultaR, FechaInterconsultaR, HoraInterconsultaR)
    End Function

    Public Function GuardarRespuestaInterconsulta_(ByVal Respuesta As String, ByVal IdInterconsultaR As String, ByVal FechaInterconsultaR As String, ByVal HoraInterconsultaR As String) As String
        Try
            oInterconsultaE.IdeInterConsulta = IdInterconsultaR
            oInterconsultaE.Campo = "txt_respuesta"
            oInterconsultaE.ValorNuevo = Respuesta.Trim().ToUpper()
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)

            oInterconsultaE.Campo = "fec_respuesta"
            oInterconsultaE.ValorNuevo = Format(DateTime.Now, "MM/dd/yyyy H:mm:ss")
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)

            oInterconsultaE.Campo = "ide_solicitado"
            oInterconsultaE.ValorNuevo = Session(sCodMedico)
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)

            oInterconsultaE.Campo = "fec_modifica"
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)

            oInterconsultaE.Campo = "usr_modifica"
            oInterconsultaE.ValorNuevo = Session(sCodUser)
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)


            oInterconsultaE.Campo = "fec_interconsulta"
            oInterconsultaE.ValorNuevo = FechaInterconsultaR
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)

            oInterconsultaE.Campo = "hor_interconsulta"
            oInterconsultaE.ValorNuevo = HoraInterconsultaR
            oInterconsultaLN.Sp_RceInterconsulta_Update(oInterconsultaE)

            'Sp_RceInterconsulta_Insert "fec_interconsulta/ hor_interconsulta", @codigo, @valor_nuevo

            Return ""
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    Protected Sub gvInterconsultaR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInterconsultaR.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgIntercon As New System.Web.UI.HtmlControls.HtmlImage

            If e.Row.Cells(8).Text.Trim() = "P" Then
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/InterconRojo.png"
            ElseIf e.Row.Cells(8).Text.Trim() = "C" Then
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = "../Imagenes/InterconVerde.png"
            Else
                imgIntercon = CType(e.Row.Cells(0).FindControl("imgEstado"), System.Web.UI.HtmlControls.HtmlImage)
                imgIntercon.Src = ""
            End If

        End If
    End Sub


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
            Return "EXPIRO" + "|" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function

End Class