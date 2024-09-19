Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN
Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN
Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN

Public Class Alerta
    Inherits System.Web.UI.Page
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaAlerta()
        End If

    End Sub

    Public Sub ListaAlerta()
        Dim tabla, tabla1 As New DataTable()
        oTablasE.IdeHistoria = Session(sIdeHistoria)  '188833
        oTablasE.IdeUsuario = Session(sCodUser)
        oTablasE.Orden = 1
        oTablasE.IdeAlerta = 3 'LABORATORIO
        Dim CadenaTreeview As String = ""

        tabla1 = oTablasLN.Sp_RceAlerta(oTablasE)
        If tabla1.Rows.Count > 0 Then
            CadenaTreeview += "<input type='checkbox' id='chkAlertaLaboratorioPendiente' /><label for='chkAlertaLaboratorioPendiente' class='JETIQUETA' style='font-size: 1em;'>Resultados de Laboratorio pendiente de visualizar</label><br/>"
        End If


        oTablasE.Orden = 1
        oTablasE.IdeAlerta = 2 'IMAGENES
        tabla = oTablasLN.Sp_RceAlerta(oTablasE)
        If tabla.Rows.Count > 0 Then
            CadenaTreeview += "<input type='checkbox' id='chkAlertaImagenPendiente' /><label for='chkAlertaImagenPendiente' class='JETIQUETA' style='font-size: 1em;'>Resultados de Imágenes pendiente de visualizar</label>"
        End If

        divAlertaTreeview.InnerHtml = CadenaTreeview

        'INICIO - JBARRETO - 05/08/2019 - COMENTADO
        'Dim tabla, tabla1 As New DataTable()
        'oTablasE.IdeHistoria = Session(sIdeHistoria)
        'oTablasE.IdeUsuario = Session(sCodUser)
        'oTablasE.Orden = 0
        'tabla = oTablasLN.Sp_RceAlerta(oTablasE)
        'Dim CadenaTreeview As String = ""

        'If tabla.Rows.Count > 0 Then
        '    CadenaTreeview += "<ul>"

        '    For index = 0 To tabla.Rows.Count - 1
        '        CadenaTreeview += "<li>"
        '        If index = 0 Then
        '            CadenaTreeview += "<a><img alt='' src='../Imagenes/Pastilla.png' /><span>" + tabla.Rows(index)("dsc_alerta").ToString() + "</span></a>"
        '        End If
        '        If index = 1 Then
        '            CadenaTreeview += "<a><img alt='' src='../Imagenes/Res_Imagenes_Verde.jpg' /><span>" + tabla.Rows(index)("dsc_alerta").ToString() + "</span></a>"
        '        End If
        '        If index = 2 Then
        '            CadenaTreeview += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Verde.jpg' /><span>" + tabla.Rows(index)("dsc_alerta").ToString() + "</span></a>"
        '        End If

        '        'nodos hijos
        '        oTablasE.Orden = 1
        '        oTablasE.IdeAlerta = CType(tabla.Rows(index)("ide_alerta").ToString(), Integer)
        '        tabla1 = oTablasLN.Sp_RceAlerta(oTablasE)
        '        CadenaTreeview += "<ul>"
        '        If tabla1.Rows.Count > 0 Then
        '            For index1 = 0 To tabla1.Rows.Count - 1
        '                CadenaTreeview += "<li>"
        '                CadenaTreeview += "<a class='ALERTA'>" + tabla1.Rows(index1)("nmedico").ToString() + " - " + tabla1.Rows(index1)("fec_registra").ToString() + "</a><input type='hidden' value='" + tabla1.Rows(index1)(0).ToString() + "' />"
        '                CadenaTreeview += "</li>"
        '            Next
        '        End If
        '        CadenaTreeview += "</ul>"
        '        'fin nodos hijos

        '        CadenaTreeview += "</li>"
        '    Next
        '    CadenaTreeview += "</ul>"
        'End If
        'divAlertaTreeview.InnerHtml = CadenaTreeview
        'FIN - JBARRETO - 05/08/2019 - COMENTADO
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function LaboratorioImagenCompletado(ByVal LabCompletado As String, ByVal ImgCompletado As String) As String
        Dim pagina As New Alerta()
        Return pagina.LaboratorioImagenCompletado_(LabCompletado, ImgCompletado)
    End Function

    Public Function LaboratorioImagenCompletado_(ByVal LabCompletado As String, ByVal ImgCompletado As String) As String
        Try
            Dim oRceLaboratioE As New RceLaboratioE()
            Dim oRceLaboratorioLN As New RceLaboratorioLN()
            Dim oRceImagenesE As New RceImagenesE()
            Dim oRceImagenLN As New RceImagenLN()

            If LabCompletado = "L" Then
                Dim tabla, tabla1 As New DataTable()
                oTablasE.IdeHistoria = Session(sIdeHistoria)
                oTablasE.IdeUsuario = Session(sCodUser)
                oTablasE.Orden = 1
                oTablasE.IdeAlerta = 3 'LABORATORIO                
                tabla1 = oTablasLN.Sp_RceAlerta(oTablasE)
                If tabla1.Rows.Count > 0 Then
                    For index = 0 To tabla1.Rows.Count - 1
                        oRceLaboratioE.IdeRecetaCab = CType(tabla1.Rows(index)("ide_recetacab").ToString(), Integer)
                        oRceLaboratioE.Orden = 3
                        oRceLaboratioE.CodAtencion = ""
                        tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisis_Consulta(oRceLaboratioE)

                        If tabla.Rows.Count > 0 Then
                            For index1 = 0 To tabla.Rows.Count - 1
                                If tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                                    oRceLaboratioE.IdeRecetaDet = CType(tabla.Rows(index1)("ide_recetadet").ToString().Trim(), Integer)
                                    oRceLaboratioE.Campo = "flg_revisado"
                                    oRceLaboratioE.ValorNuevo = "1"
                                    oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Update(oRceLaboratioE)
                                End If
                            Next
                        End If
                    Next
                End If
            End If
            If ImgCompletado = "I" Then
                Dim tabla, tabla1 As New DataTable()
                oTablasE.IdeHistoria = Session(sIdeHistoria)
                oTablasE.IdeUsuario = Session(sCodUser)
                oTablasE.Orden = 1
                oTablasE.IdeAlerta = 2 'LABORATORIO                
                tabla1 = oTablasLN.Sp_RceAlerta(oTablasE)
                If tabla1.Rows.Count > 0 Then
                    For index = 0 To tabla1.Rows.Count - 1

                        oRceImagenesE.CodAtencion = ""
                        oRceImagenesE.IdeRecetaCab = CType(tabla1.Rows(index)("ide_recetacab").ToString(), Integer)
                        oRceImagenesE.IdeImagen = 0
                        oRceImagenesE.Orden = 5
                        tabla = oRceImagenLN.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)

                        If tabla.Rows.Count > 0 Then
                            For index1 = 0 To tabla.Rows.Count - 1
                                If tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" Then
                                    oRceImagenesE.IdeRecetaDet = CType(tabla.Rows(index1)("ide_recetadet").ToString().Trim(), Integer)
                                    oRceImagenesE.Campo = "flg_revisado"
                                    oRceImagenesE.ValorNuevo = "1"
                                    oRceImagenLN.Sp_RceRecetaImagenDet_Update(oRceImagenesE)
                                End If
                            Next
                        End If

                    Next
                End If

            End If



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