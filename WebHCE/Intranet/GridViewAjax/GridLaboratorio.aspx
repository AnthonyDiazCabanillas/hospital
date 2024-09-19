<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridLaboratorio.aspx.vb" Inherits="WebHCE.GridLaboratorio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridLaboratorio").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });

            $("#frmGridLaboratorio").find(".JIMG-GENERAL").click(function () {
                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(0).html().trim();
                $(this).parent().parent().remove();
                $.ajax({
                    url: "GridViewAjax/GridLaboratorio.aspx/EliminarAnalisisLaboratorio",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        RecetaDet: codigo
                    }),
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "OK") {
                        $("#divGridLaboratorio").load("GridViewAjax/GridLaboratorio.aspx", function () {
                            fn_LOAD_GRID_OCUL();
                        });
                    } else {
                        $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    }

                });
            });

        });
    </script>
</head>
<body>
    <form id="frmGridLaboratorio" runat="server" class="JFORM-CONTENEDOR-GRID">
    <div class="JSBDIV_TABLA">
        <asp:GridView ID="gvLaboratorio" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" PageSize="99" 
            emptydatatext="No hay analisis agregados." >
            <Columns>
                <asp:BoundField DataField="ide_recetadet" HeaderText="Codigo">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="dsc_analisis" HeaderText="Analisis">
                    <ItemStyle HorizontalAlign="Left" Width="65%" />
                </asp:BoundField>
                <asp:BoundField DataField="ide_analisis" HeaderText="ide_analisis">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA ide_analisis" />
                    </asp:BoundField>
                <asp:TemplateField HeaderText="Hora Prog.">
                    <ItemTemplate>                                    
                        <%--<input type="checkbox" id="chkDefinitivo" runat="server" />--%>                                 
                        <input type="checkbox" id="chkHoraProg" runat="server" disabled="disabled" />
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="HoraProg" Width="10%"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>                            
                        <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL JIMG-ELIMINAR" />                                    
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="Eliminar" Width="5%" />
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="JPAGINADO" />
            <%--<EmptyDataTemplate>ide_analisis
                <asp:Label ID="Label9" runat="server" meta:resourcekey="Label7Resource2" SkinID="skinLabelAzul"
                    Text="No se agregó ningún análisis."></asp:Label></EmptyDataTemplate>--%>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
