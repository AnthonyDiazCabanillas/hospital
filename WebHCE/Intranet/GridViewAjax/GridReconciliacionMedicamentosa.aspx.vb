Imports Entidades.MedicamentosE
Imports LogicaNegocio.MedicamentosLN

Public Class GridAntecedentesPatologicos
    Inherits System.Web.UI.Page
    Dim oRceMedicamentosE As New RceMedicamentosE()
    Dim oRceMedicamentosLN As New RceMedicamentosLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Request.Params("Medicamentos")) Then '***
            If Request.Params("Medicamentos") <> "" Then
                hfOpcion.Value = Request.Params("Medicamentos")
                hfIdPatologia.Value = Request.Params("IdPatologia")
            End If
        End If
        If Not Page.IsPostBack Then
            ListaGridReconciliacionMedicamentosa()
        End If
    End Sub

    Public Sub ListaGridReconciliacionMedicamentosa()
        Dim tabla As New DataTable()
        oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
        oRceMedicamentosE.Orden = 1
        If hfOpcion.Value.Trim() <> "" Then '***
            oRceMedicamentosE.Orden = 3
            oRceMedicamentosE.IdPatologia = hfIdPatologia.Value.Trim()
        End If

        tabla = oRceMedicamentosLN.Sp_RceMedicamentosaCab_Consulta(oRceMedicamentosE)

        If Not IsNothing(Request.Params("Pagina")) Then
            gvReconciliacionMedicamentosa.PageIndex = CType(Request.Params("Pagina"), Integer) - 1
        End If


        gvReconciliacionMedicamentosa.DataSource = tabla
        gvReconciliacionMedicamentosa.DataBind()
    End Sub

    ''' <summary>
    ''' FUNCION PARA ELIMINAR MEDICAMENTO DE RECONCILIACION
    ''' </summary>
    ''' <param name="IdMedicamentosaDet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarReconciliacionMedicamento(ByVal IdMedicamentosaDet As Integer) As String
        Dim pagina As New GridAntecedentesPatologicos()
        Return pagina.EliminarReconciliacionMedicamento_(IdMedicamentosaDet)
    End Function
    
    Public Function EliminarReconciliacionMedicamento_(ByVal IdMedicamentosaDet As Integer) As String
        Try
            oRceMedicamentosE.IdMedicamentosaDet = IdMedicamentosaDet
            oRceMedicamentosE.Campo = "flg_activo"
            oRceMedicamentosE.ValorNuevo = "0"
            oRceMedicamentosLN.Sp_RceMedicamentosaDet_Update(oRceMedicamentosE)
            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function


End Class