Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class BusquedaMedicamento
    Inherits System.Web.UI.Page
    Dim oRceProductoE As New RceProductoE()
    Dim oRceProductoLN As New RceProductoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ListaMedicamento()
        End If
    End Sub

    Public Sub ListaMedicamento()
        Dim tablaLogistica As New DataTable()
        'oRceProductoE.Nombre = Session(sCodigoAtencion)
        'oRceProductoE.Key = 0
        'oRceProductoE.NumeroDeLineas = 0
        'oRceProductoE.Orden = 9
        'tablaLogistica = oRceProductoLN.Sp_Almacenes_Consulta(oRceProductoE)
        'oRceProductoE.CodAlmacen = tablaLogistica.Rows(0)(0).ToString()
        'oRceProductoE.Orden = tablaLogistica.Rows(0)(1).ToString()
        oRceProductoE.CodAtencion = Session(sCodigoAtencion)
        tablaLogistica = oRceProductoLN.Sp_CentroxHabitacion_Consulta(oRceProductoE)
        oRceProductoE.CodAlmacen = tablaLogistica.Rows(0)("codalmacen").ToString()
        oRceProductoE.Orden = 0


        If Request.Params("Nombre") <> Nothing Then
            oRceProductoE.Producto = Request.Params("Nombre").Trim().ToUpper()
            'oRceProductoE.CodAlmacen = "0000004" JB - COMENTADO
            'oRceProductoE.Orden = 11 JB - COMENTADO
            If Not IsNothing(Request.Params("Orden")) Then 'JB - NUEVO
                If Request.Params("Orden").Trim().ToUpper() <> "" Then
                    oRceProductoE.Orden = Request.Params("Orden").Trim().ToUpper()
                End If
            End If
            Dim tabla As New DataTable()
            tabla = oRceProductoLN.Log_ConsultaProductosPedidosxAlmacen(oRceProductoE)
            'INICIO - JB - 28/04/2021 - nuevo codigo
            If tabla.Rows.Count > 0 Then
                Dim fila As DataRow()
                Dim tabla2 As New DataTable()
                fila = tabla.Select("stkalm_m > 0")
                If fila.Length > 0 Then
                    tabla = fila.CopyToDataTable()
                Else
                    tabla.Clear()
                End If
            End If
            'FIN - JB - 28/04/2021 - nuevo codigo
            gvBusquedaProducto.DataSource = tabla
            gvBusquedaProducto.DataBind()
        Else
            oRceProductoE.Producto = ""
            'oRceProductoE.CodAlmacen = "0000004" JB - COMENTADO
            'oRceProductoE.Orden = 11 JB - COMENTADO
            If Not IsNothing(Request.Params("Orden")) Then 'JB - NUEVO
                If Request.Params("Orden").Trim().ToUpper() <> "" Then
                    oRceProductoE.Orden = Request.Params("Orden").Trim().ToUpper()
                End If
            End If
            Dim tabla As New DataTable()
            tabla = oRceProductoLN.Log_ConsultaProductosPedidosxAlmacen(oRceProductoE)
            'INICIO - JB - 28/04/2021 - nuevo codigo
            If tabla.Rows.Count > 0 Then
                Dim fila As DataRow()
                Dim tabla2 As New DataTable()
                fila = tabla.Select("stkalm_m > 0")
                If fila.Length > 0 Then
                    tabla = fila.CopyToDataTable()
                Else
                    tabla.Clear()
                End If
            End If
            'FIN - JB - 28/04/2021 - nuevo codigo
            gvBusquedaProducto.DataSource = tabla
            gvBusquedaProducto.DataBind()
        End If
    End Sub

    Protected Sub gvBusquedaProducto_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBusquedaProducto.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim Texto As String
        '    Texto = e.Row.Cells(1).Text().Trim().ToUpper()

        '    If Texto.Contains(Request.Params("Nombre").Trim().ToUpper()) Then
        '        e.Row.Cells(1).Text = e.Row.Cells(1).Text.Trim().ToUpper().Replace(Request.Params("Nombre").Trim().ToUpper(), "<b><span style='color:red;'>" + Request.Params("Nombre").Trim().ToUpper() + "</span></b>")
        '    End If
        'End If
    End Sub
End Class