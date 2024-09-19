Imports System.Data
Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN
Imports System.Net
Imports System.IO

Public Class DeclaratoriaAlergia
    Inherits System.Web.UI.Page
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

    ''' <summary>
    ''' FUNCION PARA CARGAR LOS DATOS DE DECLARATORIA ALERGIA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function CargarDatos() As String
        Dim pagina As New DeclaratoriaAlergia()
        Return pagina.CargarDatos_()
    End Function

    Public Function CargarDatos_() As String
        oRceAlergiaE.IdHistoria = Session(sIdeHistoria)
        oRceAlergiaE.Orden = 1
        Dim tabla As New DataTable()
        tabla = oRceAlergiaLN.Sp_RceAlergia_Consulta(oRceAlergiaE)
        Dim Valores As String = ""

        If tabla.Rows.Count > 0 Then
            Valores = tabla.Rows(0)("flg_presentalergia").ToString().Trim() + ";" + tabla.Rows(0)("flg_representante").ToString().Trim() + ";" +
            tabla.Rows(0)("ide_numdocumento").ToString().Trim() + ";" + tabla.Rows(0)("txt_representante").ToString().Trim() + ";" + tabla.Rows(0)("txt_alimentos").ToString().Trim() + ";" +
            tabla.Rows(0)("txt_otros").ToString().Trim()
        End If

        Return Valores
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarDeclaratoriaAlergia(ByVal OtrosDeclaratoria As String, ByVal AlimentosDeclaratoria As String, ByVal PresentaAlergiaDeclaratoria As String, _
                                                      ByVal RepresentanteDeclaratoria As String, ByVal NroDocumentoDeclaratoria As String, ByVal NombreRepresentanteDeclaratoria As String) As String
        Dim pagina As New DeclaratoriaAlergia()
        Return pagina.GuardarDeclaratoriaAlergia_(OtrosDeclaratoria, AlimentosDeclaratoria, PresentaAlergiaDeclaratoria, _
                                                      RepresentanteDeclaratoria, NroDocumentoDeclaratoria, NombreRepresentanteDeclaratoria)
    End Function

    Public Function GuardarDeclaratoriaAlergia_(ByVal OtrosDeclaratoria As String, ByVal AlimentosDeclaratoria As String, ByVal PresentaAlergiaDeclaratoria As String, _
                                                      ByVal RepresentanteDeclaratoria As String, ByVal NroDocumentoDeclaratoria As String, ByVal NombreRepresentanteDeclaratoria As String) As String

        Try
            Dim tabla As New DataTable()
            tabla = CType(Session(sTablaDeclaratoriaAlergia), DataTable)
            Dim Medicamento As String = ""
            For index = 0 To tabla.Rows.Count - 1
                Medicamento += tabla.Rows(index)("codigo").ToString().Trim().ToUpper() + "*" + tabla.Rows(index)("nombre").ToString().Trim().ToUpper() + "|"
            Next

            If Medicamento.Trim() <> "" Then
                Medicamento = Medicamento.Remove(Medicamento.LastIndexOf("|"))
            End If
            oRceAlergiaE.Campo = "txt_medicamentos"
            oRceAlergiaE.Atencion = Session(sCodigoAtencion)
            oRceAlergiaE.ValorNuevo = Medicamento
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "txt_otros"
            oRceAlergiaE.ValorNuevo = OtrosDeclaratoria
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "txt_alimentos"
            oRceAlergiaE.ValorNuevo = AlimentosDeclaratoria
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "flg_presentalergia"
            oRceAlergiaE.ValorNuevo = PresentaAlergiaDeclaratoria
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "flg_representante"
            oRceAlergiaE.ValorNuevo = RepresentanteDeclaratoria
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "ide_numdocumento"
            oRceAlergiaE.ValorNuevo = NroDocumentoDeclaratoria
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "txt_representante"
            oRceAlergiaE.ValorNuevo = NombreRepresentanteDeclaratoria
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "fec_registra"
            oRceAlergiaE.ValorNuevo = ""
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "usr_registra"
            oRceAlergiaE.ValorNuevo = Session(sCodUser)
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "fec_modifica"
            oRceAlergiaE.ValorNuevo = ""
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            oRceAlergiaE.Campo = "usr_modifica"
            oRceAlergiaE.ValorNuevo = Session(sCodUser)
            oRceAlergiaLN.Sp_RceAlergia_Update(oRceAlergiaE)

            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try
    End Function

    ''' <summary>
    ''' ELIMINANDO SESSION DEL LISTADO DE ALERGIAS AL CERRAR EL POPUP
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function LimpiaListadoAlergia() As String
        Dim pagina As New DeclaratoriaAlergia()
        Return pagina.LimpiaListadoAlergia_()
    End Function

    Public Function LimpiaListadoAlergia_() As String
        If Not IsNothing(Session(sTablaDeclaratoriaAlergia)) Then
            Dim tabla_alergia As New DataTable()
            tabla_alergia.Columns.Add("codigo")
            tabla_alergia.Columns.Add("nombre")
            Session(sTablaDeclaratoriaAlergia) = tabla_alergia
        End If
        Return "OK"
    End Function

    ''' <summary>
    ''' ELIMINA LA LISTA DE ALERGIA CUANDO SALE DEL POPUP
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminaListadoAlergia() As String
        Dim pagina As New DeclaratoriaAlergia()
        Return pagina.EliminaListadoAlergia_()
    End Function

    Public Function EliminaListadoAlergia_() As String
        If Not IsNothing(Session(sTablaDeclaratoriaAlergia)) Then
            Session.Remove(sTablaDeclaratoriaAlergia)
        End If
        Return "OK"
    End Function


    ''' <summary>
    ''' FUNCION PARA VALIDAR SI EL PRINCIPIO ACTIVO YA FUE AGREGADO
    ''' </summary>
    ''' <param name="CodigoPrincipioActivo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaPrincipioActivoAgregado(ByVal CodigoPrincipioActivo As String) As String
        Dim pagina As New DeclaratoriaAlergia()
        Return pagina.ValidaPrincipioActivoAgregado_(CodigoPrincipioActivo)
    End Function

    Public Function ValidaPrincipioActivoAgregado_(ByVal CodigoPrincipioActivo As String) As String
        Dim mensaje As String = ""
        Try
            If Not IsNothing(Session(sTablaDeclaratoriaAlergia)) Then
                Dim tabla As New DataTable()
                tabla = CType(Session(sTablaDeclaratoriaAlergia), DataTable)
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("codigo").ToString().Trim() = CodigoPrincipioActivo Then
                        mensaje = "EXISTE"
                    End If
                Next
            Else
                mensaje = ""
            End If
            Return mensaje
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
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function

End Class