<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridVerificarMedicamento.aspx.vb" Inherits="WebHCE.GridVerificarMedicamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridVerificarMedicamento").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridVerificarMedicamento").load("GridViewAjax/GridVerificarMedicamento.aspx", {
                    Pagina: sNumeroPagina,
                    IdReceta: $("#" + "<%=hfIdReceta_.ClientID %>").val()
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });

        });
    </script>
</head>
<body>
    <form id="frmGridVerificarMedicamento" runat="server" class="JFORM-CONTENEDOR-GRID">
        <input type="hidden" runat="server" id="hfIdReceta_" />
        <div class="JSBDIV_TABLA">
            <asp:GridView ID="gvVerificarMedicamento" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay Medicamento agregados." >
                <Columns>
                    <asp:BoundField DataField="cod_producto" HeaderText="Codigo" >
                        <HeaderStyle  CssClass="JCOL-OCULTA" />
                        <ItemStyle  CssClass="JCOL-OCULTA Codigo" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nproducto" HeaderText="Producto" >
                        <ItemStyle  CssClass="Producto" Width="60%"  />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="UnidMedida" HeaderText="Unid. Medida" >
                        <ItemStyle  CssClass="Unid. Medida" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Via" HeaderText="Via" >
                        <ItemStyle  CssClass="Via" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="num_frecuencia" HeaderText="Cada (hrs)" >
                        <ItemStyle  CssClass="Cada (hrs)" Width="10%"  />
                    </asp:BoundField>                                
                    <asp:BoundField DataField="num_cantidad" HeaderText="Cantidad" >
                        <ItemStyle  CssClass="Cantidad" Width="10%"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="num_dosis" HeaderText="Dosis" >
                        <ItemStyle  CssClass="Dosis" Width="10%"   />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_receta" HeaderText="">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA ide_receta" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_medicamento_suspension" HeaderText="">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA ide_medicamento_suspension" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Verificar">
                        <ItemTemplate>
                            <input type="checkbox" id="chkVerificar" runat="server" />
                        </ItemTemplate>                                                                
                        <ItemStyle CssClass="Verificar" Width="5%"/>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
