<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridLaboratorioCompletado.aspx.vb" Inherits="WebHCE.GridLaboratorioCompletado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridLaboratorioCompletado").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridLaboratorioCompletado").load("GridViewAjax/GridLaboratorioCompletado.aspx", {
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
    <form id="frmGridLaboratorioCompletado" runat="server" class="JFORM-CONTENEDOR-GRID">
    <input type="hidden" runat="server" id="hfIdReceta_" />
        <div class="JSBDIV_TABLA">
            <asp:GridView ID="gvLaboratorioCompletado" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay laboratorio disponibles." >
                <Columns>
                    <asp:BoundField DataField="ide_recetadet" HeaderText="Codigo" >
                        <HeaderStyle  CssClass="JCOL-OCULTA" />
                        <ItemStyle  CssClass="JCOL-OCULTA ide_recetadet" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_analisis" HeaderText="Examen RX" >
                        <ItemStyle  CssClass="dsc_analisis" Width="60%"  />
                    </asp:BoundField>

                    <asp:BoundField DataField="fec_modifica" HeaderText="Fecha" >
                        <ItemStyle  CssClass="fec_modifica" Width="10%"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="hor_modifica" HeaderText="Hora" >
                        <ItemStyle  CssClass="hor_modifica" Width="10%"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_recetacab" HeaderText="">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA ide_recetacab" />
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
