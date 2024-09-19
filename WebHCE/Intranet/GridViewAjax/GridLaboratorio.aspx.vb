Imports System.Data
Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN
Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN


Public Class GridLaboratorio
    Inherits System.Web.UI.Page
    Dim nNumeroPagina As Integer

    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()
    Dim oRceDiagnosticoE As New RceDiagnosticoE()
    Dim oRceDiagnosticoLN As New RceDiagnosticoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaLaboratorio()
        End If

    End Sub

    Public Sub ListaLaboratorio()
        Dim tabla As New DataTable()
        tabla.Columns.Add("ide_recetadet")
        tabla.Columns.Add("dsc_analisis")
        tabla.Columns.Add("ide_analisis")
        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
        oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
        oRceLaboratioE.Orden = 2
        'JB - 28/08/2020 - COMENTADO
        'If Request.Params("Pagina") <> Nothing Then
        '    tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
        '    gvLaboratorio.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
        '    Session(sTablaAnalisisLaboratorio) = tabla
        '    gvLaboratorio.DataSource = tabla
        '    gvLaboratorio.DataBind()
        'Else
        '    tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
        '    Session(sTablaAnalisisLaboratorio) = tabla
        '    gvLaboratorio.DataSource = tabla
        '    gvLaboratorio.DataBind()

        'End If



        If Not IsNothing(Session(sTablaAnalisisLaboratorio)) Then
            gvLaboratorio.DataSource = Session(sTablaAnalisisLaboratorio)
            gvLaboratorio.DataBind()
        Else
            gvLaboratorio.DataSource = tabla
            gvLaboratorio.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' FUNCION PARA VALIDAR ANALISIS EXISTE Y VALIDAR SI HAY RELACION ENTRE EL ANALISIS SELECCIONADO Y LOS DIAGNOSTICOS
    ''' </summary>
    ''' <param name="CodigoAnalisis"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAnalisisExistente_AnalisisDiagnostico(ByVal CodigoAnalisis As String, ByVal DscAnalisis As String) As String
        Dim pagina As New GridLaboratorio()
        Return pagina.ValidaAnalisisExistente_AnalisisDiagnostico_(CodigoAnalisis, DscAnalisis)
    End Function

    Public Function ValidaAnalisisExistente_AnalisisDiagnostico_(ByVal CodigoAnalisis As String, ByVal DscAnalisis As String) As String
        Try
            If ValidaSession_() <> "" Then
                Return "EXPIRO" + "*" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
            End If
            Dim tabla As New DataTable()
            Dim tabla_diagnostico As New DataTable()
            oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
            oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
            oRceLaboratioE.Orden = 2
            tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
            Dim Mensaje As String = ""
            For index = 0 To tabla.Rows.Count - 1
                If tabla.Rows(index)("ide_analisis").ToString().Trim() = CodigoAnalisis Then
                    Mensaje = "VALIDACION1*El análisis ya se encuentra seleccionado."
                End If
            Next

            'INICIO -JB - COMENTADO - 26 / 8 / 2019
            Dim CubreDiagnostico As Boolean = False
            Dim CubreAseguradora As Boolean = False
            Dim MensajeFinalDiag As String = ""
            Dim MensajeFinalAseg As String = ""
            If Mensaje = "" Then
                oRceDiagnosticoE.Tipo = "A"
                oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
                tabla_diagnostico = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)
                For index1 = 0 To tabla_diagnostico.Rows.Count - 1 'por cada diagnostico...
                    oRceLaboratioE.IdAnalisisLaboratorio = CodigoAnalisis
                    oRceLaboratioE.IdHistoria = Session(sIdeHistoria)
                    oRceLaboratioE.Orden = 2
                    oRceLaboratioE.CodDiagnostico = tabla_diagnostico.Rows(index1)("coddiagnostico").ToString().Trim()
                    Dim tabla_ As New DataTable()
                    tabla_ = oRceLaboratorioLN.Sp_RceAnalisisxDiagnostico_Valida(oRceLaboratioE)
                    If tabla_.Rows.Count > 0 Then
                        If tabla_.Rows(0)(0).ToString() = "-2" Then
                            If CubreDiagnostico <> True Then 'JB - si ya es 'true' es por que ya lo cubre alguna diagnostico y no se le volvera a cambiar valor
                                CubreDiagnostico = False
                            End If
                        End If
                        If tabla_.Rows(0)(0).ToString() = "-1" Then
                            CubreDiagnostico = True
                            CubreAseguradora = False
                            'Exit For
                        End If
                        If tabla_.Rows(0)(0).ToString() <> "-2" And tabla_.Rows(0)(0).ToString() <> "-1" Then
                            CubreDiagnostico = True
                            CubreAseguradora = True
                            'Exit For
                        End If
                    End If
                Next
            End If
            If CubreDiagnostico = False Then
                MensajeFinalDiag += DscAnalisis.Trim() + "<br/>"
            End If

            If CubreAseguradora = False Then
                MensajeFinalAseg += DscAnalisis.Trim() + "<br/>"
                'hfCubreAseguradora.Value = "NO"
            End If
            Dim MensajeFinal As String = ""

            If MensajeFinalDiag <> "" Or MensajeFinalAseg <> "" Then
                If MensajeFinalDiag.Trim() <> "" Then 'ahora las atenciones J no mostraran ningun mensaje - 26/12/2017
                    MensajeFinal += "El/Los analisis no estan asociados a ningun diagnostico <br /><br />" + MensajeFinalDiag + "<br />"
                End If
                If MensajeFinalAseg.Trim() <> "" Then
                    'MensajeFinal += "Aseguradora no cubre analisis seleccionados <br /><br />" + MensajeFinalAseg + "<br />" 'JB - SE COMENTA, YA NO MOSTRARA COBERTURA - 02/01/2020
                End If

                If MensajeFinal <> "" Then
                    If MensajeFinalAseg.Trim() <> "" And MensajeFinalDiag.Trim() = "" Then
                    Else
                        MensajeFinal = MensajeFinal + "<br />¿Desea agregarlo?"
                    End If
                Else
                    'MensajeConfirmacion()
                End If
            End If

            'inicio - jb - 16/07/2020 - nuevo codigo
            If Mensaje <> "" Then
                Return Mensaje
            End If
            'fin - jb - 16/07/2020 - nuevo codigo

            If MensajeFinal <> "" Then
                Return "VALIDACION2*" + MensajeFinal + "*" + (IIf(CubreAseguradora = False, "NO", "SI"))
            Else
                Return ""
            End If
            'FIN -JB - COMENTADO - 26 / 8 / 2019

            'Return ""

        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function

    'Public Function ValidaAnalisisExistente_AnalisisDiagnostico_(ByVal CodigoAnalisis As String) As String
    '    Try
    '        If ValidaSession_() <> "" Then
    '            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
    '        End If
    '        Dim tabla As New DataTable()
    '        Dim tabla_diagnostico As New DataTable()
    '        oRceLaboratioE.CodAtencion = Session(sCodigoAtencion)
    '        oRceLaboratioE.IdeRecetaCab = Session(sIdRecetaCab)
    '        oRceLaboratioE.Orden = 2
    '        tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
    '        Dim Mensaje As String = ""
    '        For index = 0 To tabla.Rows.Count - 1
    '            If tabla.Rows(index)("ide_analisis").ToString().Trim() = CodigoAnalisis Then
    '                Mensaje = "1;El diagnóstico ya se encuentra seleccionado."
    '            End If
    '        Next

    '        If Mensaje = "" Then
    '            oRceLaboratioE.IdAnalisisLaboratorio = CodigoAnalisis
    '            oRceDiagnosticoE.Tipo = sTipoD
    '            oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
    '            tabla_diagnostico = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta(oRceDiagnosticoE)
    '            For index1 = 0 To tabla_diagnostico.Rows.Count - 1
    '                oRceLaboratioE.CodDiagnostico = tabla_diagnostico.Rows(index1)("coddiagnostico").ToString().Trim()
    '                If oRceLaboratorioLN.Sp_RceAnalisisxDiagnostico_Consulta(oRceLaboratioE) Then
    '                    Mensaje = ""
    '                    Exit For
    '                Else
    '                    Mensaje = "2;El analisis no esta asociado a ningun Diágnostico Seleccionado, ¿Desea añadirlo a la lista de Análisis?"
    '                End If
    '            Next
    '        End If

    '        If Mensaje = "" Then
    '            oRceLaboratioE.IdAnalisisLaboratorio = CodigoAnalisis
    '            oRceLaboratioE.IdHistoria = Session(sIdeHistoria)
    '            oRceLaboratioE.Orden = 1
    '            Dim tabla_ As New DataTable()
    '            tabla = oRceLaboratorioLN.Sp_RceAnalisisxDiagnostico_Valida(oRceLaboratioE)
    '            If tabla.Rows.Count = 0 Then
    '                Mensaje = "3;Aseguradora no cubre analisis seleccionado."
    '            End If
    '        Else
    '            oRceLaboratioE.IdAnalisisLaboratorio = CodigoAnalisis
    '            oRceLaboratioE.IdHistoria = Session(sIdeHistoria)
    '            oRceLaboratioE.Orden = 1
    '            Dim tabla_ As New DataTable()
    '            tabla = oRceLaboratorioLN.Sp_RceAnalisisxDiagnostico_Valida(oRceLaboratioE)
    '            If tabla.Rows.Count = 0 Then
    '                Mensaje = Mensaje + ";Aseguradora no cubre analisis seleccionado."
    '            End If
    '        End If

    '        Return Mensaje
    '    Catch ex As Exception
    '        Return "ERROR;" + ex.Message.ToString()
    '    End Try



    'End Function


    ''' <summary>
    ''' FUNCION PARA ELIMINAR EL ANALISIS
    ''' </summary>
    ''' <param name="RecetaDet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarAnalisisLaboratorio(ByVal RecetaDet As String) As String
        Dim pagina As New GridLaboratorio()
        Return pagina.EliminarAnalisisLaboratorio_(RecetaDet)
    End Function
    Public Function EliminarAnalisisLaboratorio_(ByVal RecetaDet As String) As String
        oRceLaboratioE.IdeRecetaDet = RecetaDet
        Try
            Dim dt As New DataTable()
            dt = CType(Session(sTablaAnalisisLaboratorio), DataTable)

            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("ide_recetadet").ToString().Trim() = RecetaDet.ToString().Trim() Then
                    dt.Rows(index).Delete()
                    Exit For
                End If
            Next
            Session(sTablaAnalisisLaboratorio) = dt
            Return "OK"
            'If oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Delete(oRceLaboratioE) Then
            '    Return "OK"
            'Else
            '    Return ConfigurationManager.AppSettings(sMensajeEliminarError).Trim().ToString()
            'End If

        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function



    Public Function ValidaSession_() As String
        If IsNothing(Session(sCodMedico)) And IsNothing(Session(sCodEnfermera)) Then
            Return "EXPIRO"
        Else
            Return ""
        End If
    End Function


End Class