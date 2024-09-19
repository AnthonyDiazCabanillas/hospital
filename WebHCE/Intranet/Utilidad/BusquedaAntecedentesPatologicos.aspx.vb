Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Public Class BusquedaAntecedentesPatologicos
    Inherits System.Web.UI.Page
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaAntecedentesPatologicos()
        End If
    End Sub

    Public Sub ListaAntecedentesPatologicos()
        Dim tabla As New DataTable()
        oTablasE.IdeHistoria = Session(sIdeHistoria)
        oTablasE.Orden = 1
        tabla = oTablasLN.Sp_RceResultadoExamenFisico_Consulta(oTablasE)
        'tabla.Columns(1).ColumnName = "nombre"
        gvBusquedaAntecedentePatologico.DataSource = tabla
        gvBusquedaAntecedentePatologico.DataBind()
    End Sub

End Class