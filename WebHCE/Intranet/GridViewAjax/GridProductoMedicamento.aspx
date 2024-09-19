<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridProductoMedicamento.aspx.vb" Inherits="WebHCE.GridProductoMedicamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridProductoMedicamento").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridProductoMedicamento").load("GridViewAjax/GridProductoMedicamento.aspx", {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });

            $("#frmGridProductoMedicamento").find(".JIMG-GENERAL").click(function () {
                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(0).html().trim();
                $(this).parent().parent().remove();
                $.ajax({
                    url: "GridViewAjax/GridProductoMedicamento.aspx/EliminarProductoMedicamento",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        CodigoProductoMedicamento: codigo
                    }),
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_GRID_OCUL();
                });
            });


            $("#frmGridProductoMedicamento").find(".JCHECK-STAT").click(function () {
                var PedidoStat = "";
                if ($(this).prop("checked")) {
                    PedidoStat = "SI";
                } else {
                    PedidoStat = "NO";
                }

                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(0).text().trim();
                var Producto = $(this).parent().parent().find("td").eq(1).html().trim();

                var TipoPedido = $(this).parent().parent().find("td").eq(7).text().trim(); //JB - 19/04/2021
                var FlgPrn = $(this).parent().parent().find("td").eq(8).text().trim(); //JB - 28/04/2021

                if (codigo == "" || TipoPedido == "") { //JB - 10/12/2020 - SOLO MEDICAMENTOS CON COD PRODUCTO SE LES HARA PEDIDO AUTOMATICO
                    if (FlgPrn == "SI") {

                    } else {
                        $(this).prop("checked", false);
                        fn_LOAD_GRID_OCUL();
                        return;
                    }                    
                }

                $.ajax({
                    url: "GridViewAjax/GridProductoMedicamento.aspx/ActualizarTipoPedidoStat",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        CodigoProductoMedicamento: codigo,
                        DescripcionProducto: Producto,
                        TipoPedidoStat: PedidoStat
                    }),
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_GRID_OCUL();
                });
            });


        });
    </script>
</head>
<body>
    <form id="frmGridProductoMedicamento" runat="server" class="JFORM-CONTENEDOR-GRID">
        <div class="JSBDIV_TABLA">   
            <asp:GridView ID="gvProductoMedicamento" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay Productos agregados." >
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" >
                        <HeaderStyle  CssClass="JCOL-OCULTA" />
                        <ItemStyle  CssClass="JCOL-OCULTA Codigo" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Producto" HeaderText="Producto" >
                        <ItemStyle  CssClass="Producto" Width="20%"  />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="UnidMedida" HeaderText="Unid. Medida" >
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle  CssClass="JCOL-OCULTA Unid. Medida" Width="10%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="Via" HeaderText="Via" >
                        <ItemStyle  CssClass="Via" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CadaHora" HeaderText="Cada (hrs)" >
                        <ItemStyle  CssClass="Cada (hrs)" Width="10%"  />
                    </asp:BoundField>                                
                    <%--<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" >
                        <ItemStyle  CssClass="Cantidad" Width="10%"  />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="Dosis" HeaderText="Dosis" >
                        <ItemStyle  CssClass="Dosis" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" >
                        <ItemStyle  CssClass="Cantidad" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Indicacion" HeaderText="Indicacion">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA Indicacion" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Dia" HeaderText="Dia">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA Dia" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="TipoPedido" HeaderText="TipoPedido">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA TipoPedido" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Prn" HeaderText="Prn">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA Prn" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>                            
                            <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL" />                                    
                        </ItemTemplate>                                                                
                        <ItemStyle CssClass="Eliminar" Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stat" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>                            
                            <input type="checkbox" id="chkStatMedicamento" runat="server" class="JCHECK-STAT" />                                   
                        </ItemTemplate>                                                                
                        <ItemStyle CssClass="Stat" Width="5%" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
