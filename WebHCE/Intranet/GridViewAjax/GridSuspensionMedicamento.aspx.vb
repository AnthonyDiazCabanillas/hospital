Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class GridSuspensionMedicamento
    Inherits System.Web.UI.Page

    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaMedicamento()
        End If

    End Sub

    Public Sub ListaMedicamento()
        'INICIO - JB - SE COMENTO ESTE BLOQUE Y SE REEMPLAZA POR EL CODIGO DEBAJO
        'If Not IsNothing(Request.Params("IdReceta")) Then
        '    hfIdReceta_.Value = Request.Params("IdReceta").Trim()
        '    oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        '    oRceRecetaMedicamentoE.IdRecetaDet = Request.Params("IdReceta").Trim()
        '    oRceRecetaMedicamentoE.Orden = 5
        '    Dim tabla As New DataTable()
        '    tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        '    gvSuspensionMedicamento.DataSource = tabla
        '    gvSuspensionMedicamento.DataBind()
        'End If
        'FIN - JB - SE COMENTO ESTE BLOQUE Y SE REEMPLAZA POR EL CODIGO DEBAJO

        'INICIO - JB - NUEVO CODIGO
        If Not IsNothing(Request.Params("IdReceta")) Then

            hfIdReceta_.Value = Request.Params("IdReceta").Trim()
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            oRceRecetaMedicamentoE.IdRecetaDet = Request.Params("IdReceta").Trim()
            oRceRecetaMedicamentoE.Orden = 5
            Dim tabla As New DataTable()
            Dim NumeroPagina As Integer = 0
            If IsNothing(Request.Params("Pagina")) Then
                NumeroPagina = 0
                gvSuspensionMedicamento.PageIndex = NumeroPagina
            Else
                NumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
                NumeroPagina = NumeroPagina - 1
                gvSuspensionMedicamento.PageIndex = NumeroPagina
            End If

            tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
            gvSuspensionMedicamento.DataSource = tabla
            gvSuspensionMedicamento.DataBind()
        End If
        'FIN - JB - NUEVO CODIGO

    End Sub




    <System.Web.Services.WebMethod()>
    Public Shared Function AgregarMedicamentosSuspender(ByVal IdeMedicamento As String, ByVal Tipo As String) As String
        Dim pagina As New GridSuspensionMedicamento()
        Return pagina.AgregarMedicamentosSuspender_(IdeMedicamento, Tipo)
    End Function

    Public Function AgregarMedicamentosSuspender_(ByVal IdeMedicamento As String, ByVal Tipo As String) As String
        Dim tabla As New DataTable()
        If Not IsNothing(Session(sTablaMedicamentosSuspender)) Then
            tabla = CType(Session(sTablaMedicamentosSuspender), DataTable)

            If Tipo = "SI" Then
                tabla.Rows.Add(IdeMedicamento)
                Session(sTablaMedicamentosSuspender) = tabla
            Else
                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)(0).ToString().Trim() = IdeMedicamento Then
                        tabla.Rows.RemoveAt(index)
                        Session(sTablaMedicamentosSuspender) = tabla
                        Exit For
                    End If
                Next
            End If
        Else
            tabla.Columns.Add("IdeMedicamento")
            tabla.Rows.Add(IdeMedicamento)
            Session(sTablaMedicamentosSuspender) = tabla
        End If
        Return "OK"
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ListaMedicamentosSuspender() As String
        Dim pagina As New GridSuspensionMedicamento()
        Return pagina.ListaMedicamentosSuspender_()
    End Function

    Public Function ListaMedicamentosSuspender_() As String
        Dim ds As New DataSet()
        Dim tabla As New DataTable()
        If Not IsNothing(Session(sTablaMedicamentosSuspender)) Then
            tabla = CType(Session(sTablaMedicamentosSuspender), DataTable)

            tabla.TableName = "TablaListaMedicamentoSuspender"
            Dim dsx = tabla.DataSet
            If dsx IsNot Nothing Then 'codigo para eliminar tabla si ya existe en el dataset
                dsx.Tables.Remove(tabla.TableName)
            End If
            ds.Tables.Add(tabla)
            Return ds.GetXml()
        Else
            Return ""
        End If

    End Function
End Class