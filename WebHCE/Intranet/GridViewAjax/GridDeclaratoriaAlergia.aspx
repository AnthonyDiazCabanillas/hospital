<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridDeclaratoriaAlergia.aspx.vb" Inherits="WebHCE.GridDeclaratoriaAlergia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            //PAGINADO
            $("#frmDeclaratoriaAlergia").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });


            $("#frmDeclaratoriaAlergia").find(".JIMG-ELIMINAR").click(function (event) {
//                if (Editar == false) {
//                    return;
//                }
                var codigo = $(this).parent().parent().find("td").eq(1).html().trim();
                fn_LOAD_GRID_VISI();
                $.ajax({
                    url: "GridViewAjax/GridDeclaratoriaAlergia.aspx/EliminarAlergia",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        CodigoAlergia: codigo
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d != "OK") {
                        fn_LOAD_GRID_OCUL();
                        $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmDeclaratoriaAlergia");
                    } else {
                        $("#divGridDeclaratoriaAlergia").load("GridViewAjax/GridDeclaratoriaAlergia.aspx", {
                            Pagina: "1"
                        }, function () {
                            fn_LOAD_GRID_OCUL();
                        });
                    }
                });
            });

        });

    </script>
</head>
<body>
    <form id="frmGridDeclaratoriaAlergia" runat="server" class="JFORM-CONTENEDOR-GRID">
        <div class="JSBDIV_TABLA">
            <asp:GridView ID="gvDeclaratoriaAlergia" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                <Columns>                                    
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                        <ItemStyle  CssClass="Nombre" />
                    </asp:BoundField>
                    <asp:BoundField DataField="codigo" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA codigo" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="alimento" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA alimento" />
                    </asp:BoundField>
                    <asp:BoundField DataField="otros" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA otros" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flg_presentaalergia" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA flg_presentaalergia" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flg_representante" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA flg_representante" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nro_documento" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA nro_documento" />
                    </asp:BoundField>
                    <asp:BoundField DataField="txt_representante" HeaderStyle-CssClass="JCOL-OCULTA" >
                        <HeaderStyle CssClass="JCOL-OCULTA"></HeaderStyle>
                        <ItemStyle  CssClass="JCOL-OCULTA txt_representante" />
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Quitar">
                        <ItemStyle Width="60px" HorizontalAlign="Center" CssClass="Eliminar" />
                        <ItemTemplate>
                            <%--<asp:ImageButton ID="imgAnularRegMedico" runat="server" ImageUrl="~/Imagenes/anular.gif"
                                CommandName="Borrar" />--%>
                            <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL JIMG-ELIMINAR" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
