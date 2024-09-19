Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN
Imports System.Data

Public Class GridProductoMedicamento
    Inherits System.Web.UI.Page
    Dim nNumeroPagina As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ListaProductoMedicamento()
        End If

    End Sub

    Public Sub ListaProductoMedicamento()
        Dim tabla As New DataTable()
        If Not IsNothing(Request.Params("Codigo")) Then 'SI SE RECIBEN NUEVOS VALORES ENTONCES...
            nNumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
            If Not IsNothing(Session(sTablaProductoMedicamento)) Then 'SI YA HAY DATOS EN EL GRID DE PRODUCTO, SE AÑADIRA UN NUEVO REGISTROS CON LOS VALORES RECIBIDOS
                tabla = CType(Session(sTablaProductoMedicamento), DataTable)
                tabla.Rows.Add(Request.Params("Codigo").Trim(), Request.Params("Producto").Trim(), Request.Params("Via").Trim().ToUpper(),
                               Request.Params("CadaHora").Trim(), Request.Params("Dosis").Trim().ToUpper(), Request.Params("Indicacion").Trim().ToUpper(), Request.Params("Cantidad").Trim(), Request.Params("TipoPedido").Trim(), Request.Params("TipoPedido").Trim(), Request.Params("Prn").Trim())
                gvProductoMedicamento.PageIndex = (nNumeroPagina - 1)
                gvProductoMedicamento.DataSource = tabla
                gvProductoMedicamento.DataBind()
                Session(sTablaProductoMedicamento) = tabla
            Else 'SINO SE CREARE EL PRIMER REGISTRO EN EL GRID
                tabla.Columns.Add("Codigo")
                tabla.Columns.Add("Producto")
                'tabla.Columns.Add("UnidMedida")
                tabla.Columns.Add("Via")
                tabla.Columns.Add("CadaHora")
                tabla.Columns.Add("Dosis")
                tabla.Columns.Add("Indicacion")
                tabla.Columns.Add("Cantidad") 'se re re re re re re agregra este campo..... JB
                tabla.Columns.Add("TipoPedido")
                tabla.Columns.Add("TipoPedido2")
                tabla.Columns.Add("Prn")
                tabla.Rows.Add(Request.Params("Codigo").Trim(), Request.Params("Producto").Trim(), Request.Params("Via").Trim().ToUpper(),
                               Request.Params("CadaHora").Trim(), Request.Params("Dosis").Trim().ToUpper(), Request.Params("Indicacion").Trim().ToUpper(), Request.Params("Cantidad").Trim(), Request.Params("TipoPedido").Trim(), Request.Params("TipoPedido").Trim(), Request.Params("Prn").Trim())
                gvProductoMedicamento.PageIndex = (nNumeroPagina - 1)
                gvProductoMedicamento.DataSource = tabla
                gvProductoMedicamento.DataBind()
                Session(sTablaProductoMedicamento) = tabla
            End If
        Else 'SINO ENTONCES...
            If Not IsNothing(Session(sTablaProductoMedicamento)) Then 'SI YA HAY DATOS EN LA TABLA(GUARDADOS EN LA SESSION) SOLO SE VOLVERA A CARGAR LOS DATOS CON LA PAGINA SELECCIONADA (EN CASO SE ESTE PAGINANDO)
                If Not IsNothing(Request.Params("Pagina")) Then
                    nNumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
                Else
                    nNumeroPagina = 1
                End If
                gvProductoMedicamento.PageIndex = (nNumeroPagina - 1)
                gvProductoMedicamento.DataSource = Session(sTablaProductoMedicamento)
                gvProductoMedicamento.DataBind()
            Else 'SINO MOSTRARA UN GRID VACIO
                tabla.Columns.Add("Codigo")
                tabla.Columns.Add("Producto")
                'tabla.Columns.Add("UnidMedida")
                tabla.Columns.Add("Via")
                tabla.Columns.Add("CadaHora")
                tabla.Columns.Add("Dosis")
                tabla.Columns.Add("Indicacion")
                tabla.Columns.Add("Cantidad")
                tabla.Columns.Add("TipoPedido")
                tabla.Columns.Add("TipoPedido2")
                tabla.Columns.Add("Prn")
                gvProductoMedicamento.DataSource = tabla
                gvProductoMedicamento.DataBind()
            End If
        End If

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarProductoMedicamento(ByVal CodigoProductoMedicamento As String) As String
        Dim pagina As New GridProductoMedicamento()
        Return pagina.EliminarProductoMedicamento_(CodigoProductoMedicamento)
    End Function

    Public Function EliminarProductoMedicamento_(ByVal CodigoProductoMedicamento) As String
        Dim tabla As New DataTable()
        Dim item_borrado As String = ""
        If Not IsNothing(Session(sTablaProductoMedicamento)) Then
            tabla = CType(Session(sTablaProductoMedicamento), DataTable)
            For index = 0 To (tabla.Rows.Count - 1)
                If tabla.Rows(index)("Codigo").ToString().Trim() = CodigoProductoMedicamento Then
                    tabla.Rows.RemoveAt(index)
                    item_borrado = "SI"
                    Session(sTablaProductoMedicamento) = tabla
                    Exit For
                End If
            Next
        End If
        Return item_borrado
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ActualizarTipoPedidoStat(ByVal CodigoProductoMedicamento As String, ByVal DescripcionProducto As String, ByVal TipoPedidoStat As String) As String
        Dim pagina As New GridProductoMedicamento()
        Return pagina.ActualizarTipoPedidoStat_(CodigoProductoMedicamento, DescripcionProducto, TipoPedidoStat)
    End Function

    Public Function ActualizarTipoPedidoStat_(ByVal CodigoProductoMedicamento As String, ByVal DescripcionProducto As String, ByVal TipoPedidoStat As String) As String
        Dim tabla As New DataTable()

        If Not IsNothing(Session(sTablaProductoMedicamento)) Then
            tabla = CType(Session(sTablaProductoMedicamento), DataTable)

            If TipoPedidoStat = "SI" Then
                For index = 0 To (tabla.Rows.Count - 1)
                    If tabla.Rows(index)("Codigo").ToString().Trim() = CodigoProductoMedicamento And tabla.Rows(index)("Producto").ToString().Trim() = DescripcionProducto.Trim() Then
                        tabla.Rows(index)("TipoPedido") = "03"
                        Session(sTablaProductoMedicamento) = tabla
                        Exit For
                    End If
                Next
            Else
                For index = 0 To (tabla.Rows.Count - 1)
                    If tabla.Rows(index)("Codigo").ToString().Trim() = CodigoProductoMedicamento And tabla.Rows(index)("Producto").ToString().Trim() = DescripcionProducto.Trim() Then
                        tabla.Rows(index)("TipoPedido") = tabla.Rows(index)("TipoPedido2").ToString().Trim()
                        Session(sTablaProductoMedicamento) = tabla
                        Exit For
                    End If
                Next
            End If
        End If
        Return "OK"
    End Function

    Protected Sub gvProductoMedicamento_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProductoMedicamento.PageIndexChanging
        gvProductoMedicamento.PageIndex = e.NewPageIndex
        ListaProductoMedicamento()
    End Sub
End Class