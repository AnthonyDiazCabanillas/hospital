Imports System.Data
Imports System.IO
Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN
Imports Entidades.EvolucionE
Imports LogicaNegocio.EvolucionLN

Public Class PetitorioLab
    Inherits System.Web.UI.Page

    Dim oRceEvolucionE As New RceEvolucionE()
    Dim oRceEvolucionLN As New RceEvolucionLN()
    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarGrid()
        End If

        'If Not IsNothing(Request.Params("HiddenCodigosPetitorio")) Then
        '    hfCodigosPetitorioLab.Value = Request.Params("HiddenCodigosPetitorio")
        '    hfDescripcionPetitorioLab.Value = Request.Params("HiddenDescripcion")
        'End If

    End Sub

    Public Sub CargarGrid()
        Dim tablaPetitorio As New DataTable()
        tablaPetitorio.Columns.Add("dsc_analisis")
        tablaPetitorio.Columns.Add("ide_analisis")

        If Not IsNothing(Request.Params("Parametro[]")) Then
            For index = 0 To Request.Params("Parametro[]").Trim().Split("|")(0).Split(";").Length - 1
                Dim Fila1 As String = Request.Params("Parametro[]").Trim().Split("|")(0)
                Dim Fila2 As String = Request.Params("Parametro[]").Trim().Split("|")(1)
                Dim Descripcion As String = Request.Params("Parametro[]").Trim().Split("|")(2)
                hfCodigosPetitorioLab.Value = Request.Params("Parametro[]").Trim().Split("|")(0)
                hfDescripcionPetitorioLab.Value = Descripcion

                'INICIO - JB - 18/08/2020
                Dim tablaL As New DataTable()
                Dim DscLab As String = ""
                Dim oRceLaboratioE_ As New RceLaboratioE()
                Dim oRceLaboratorioLN_ As New RceLaboratorioLN()
                oRceLaboratioE_.IdAnalisisLaboratorio = Fila1.Split(";")(index)
                oRceLaboratioE_.Orden = 0
                oRceLaboratioE_.TipoDeAtencion = ""
                tablaL = oRceLaboratorioLN_.Sp_RceAnalisisEmergenciaMae_Consulta(oRceLaboratioE_)
                If tablaL.Rows.Count > 0 Then
                    DscLab = tablaL.Rows(0)("dsc_analisis").ToString().Trim()
                End If
                'FIN - JB - 18/08/2020

                tablaPetitorio.Rows.Add(DscLab, Fila1.Split(";")(index))
                gvPetitorioLaboratorio.DataSource = Nothing
                gvPetitorioLaboratorio.DataBind()

                If Not IsNothing(Request.Params("Pagina")) Then
                    gvPetitorioLaboratorio.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
                End If
                Session("TablaPetitorio") = tablaPetitorio
                gvPetitorioLaboratorio.DataSource = tablaPetitorio
                gvPetitorioLaboratorio.DataBind()
            Next
        End If

        If Not IsNothing(Session("TablaPetitorio")) Then
            If Not IsNothing(Request.Params("Pagina")) Then
                gvPetitorioLaboratorio.PageIndex = (CType(Request.Params("Pagina"), Integer) - 1)
                If Request.Params("CheckMarcado") = "SI" Then
                    chkProgramarHoraLab.Checked = True
                    txtFechaProgramarHoraLab.Value = Request.Params("Fecha")
                    txtFechaProgramarHoraLab.Disabled = False
                    txtHoraProgramarHoraLab.Value = Request.Params("Hora")
                    txtHoraProgramarHoraLab.Disabled = False
                    hfCodigosPetitorioLab.Value = Request.Params("HiddenCodigosPetitorio")
                    hfDescripcionPetitorioLab.Value = Request.Params("HiddenDescripcion")
                End If
            End If
            gvPetitorioLaboratorio.DataSource = Session("TablaPetitorio")
            gvPetitorioLaboratorio.DataBind()

            If Not IsNothing(Request.Params("AnalisisMarcado")) Then

                If Request.Params("AnalisisMarcado") <> "" Then 'JB - NUEVA CONDICION, SE MANDARA VACIO EN ALGUNOS CASOS - 03/01/2020
                    If Not IsNothing(Session("TablaAnalisisMarcados")) Then
                        If Request.Params("AnalisisMarcado").Trim() <> "" Then
                            Session("TablaAnalisisMarcados") = Session("TablaAnalisisMarcados") + Request.Params("AnalisisMarcado").Trim() '.Remove(Request.Params("AnalisisMarcado").Trim().Length - 1)
                        End If
                    Else
                        If Request.Params("AnalisisMarcado").Trim() <> "" Then
                            Session("TablaAnalisisMarcados") = Request.Params("AnalisisMarcado").Trim()
                        Else
                            Session("TablaAnalisisMarcados") = ""
                        End If
                    End If
                    'MARCANDO LOS CHECKBOX QUE HABIAN SIDO SELECCIONADOS
                    For index = 0 To gvPetitorioLaboratorio.Rows.Count - 1
                        For index1 = 0 To Session("TablaAnalisisMarcados").Trim().Split(";").Length - 1
                            If gvPetitorioLaboratorio.Rows(index).Cells(1).Text.Trim() = Session("TablaAnalisisMarcados").Trim().Split(";")(index1).Trim() Then
                                Dim chkAnalisis As New System.Web.UI.HtmlControls.HtmlInputCheckBox
                                chkAnalisis = CType(gvPetitorioLaboratorio.Rows(index).Cells(2).FindControl("chkHoraProg"), System.Web.UI.HtmlControls.HtmlInputCheckBox)
                                chkAnalisis.Checked = True
                            End If
                            Dim chkAnalisisx As New System.Web.UI.HtmlControls.HtmlInputCheckBox
                            chkAnalisisx = CType(gvPetitorioLaboratorio.Rows(index).Cells(2).FindControl("chkHoraProg"), System.Web.UI.HtmlControls.HtmlInputCheckBox)
                            chkAnalisisx.Disabled = False
                        Next
                    Next
                End If
                
            End If

        End If


    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function CerrarPetitorioLab() As String
        Dim pagina As New PetitorioLab()
        Return pagina.CerrarPetitorioLab_()
    End Function

    Public Function CerrarPetitorioLab_()
        Session.Remove("TablaAnalisisMarcados")
        Session.Remove("TablaPetitorio")
        Return "OK"
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarCheckSession(ByVal CodigoDesmarcado As String) As String
        Dim pagina As New PetitorioLab()
        Return pagina.EliminarCheckSession_(CodigoDesmarcado)
    End Function

    Public Function EliminarCheckSession_(ByVal CodigoDesmarcado As String)
        If Not IsNothing(Session("TablaAnalisisMarcados")) Then
            Session("TablaAnalisisMarcados") = Session("TablaAnalisisMarcados").replace(CodigoDesmarcado + ";", "")
        End If
        Return "OK"
    End Function
End Class