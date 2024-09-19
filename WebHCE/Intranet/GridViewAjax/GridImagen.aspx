<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridImagen.aspx.vb" Inherits="WebHCE.GridImagen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmgridImagen").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });


            $("#frmgridImagen").find(".JIMG-GENERAL").click(function () {
                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(2).html().trim();
                var fila = $(this);
                //$(this)
                $.ajax({
                    url: "GridViewAjax/GridImagen.aspx/EliminarImagen",
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
                        fila.parent().parent().remove();
                        $("#divGridImagen").load("GridViewAjax/GridImagen.aspx", function () {
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
    <form id="frmgridImagen" runat="server" class="JFORM-CONTENEDOR-GRID">
    <div class="JSBDIV_TABLA">
        <asp:GridView ID="gvImagenesSeleccionados" runat="server" 
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay imagenes agregadas.">
            <Columns>
                <asp:BoundField DataField="ide_imagen" HeaderText="Codigo">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="dsc_imagen" HeaderText="Examen">
                    <ItemStyle HorizontalAlign="Left" Width="75%" />
                </asp:BoundField>
                <asp:BoundField DataField="ide_recetadet" HeaderText="ide_recetadet">
                    <HeaderStyle CssClass="JCOL-OCULTA" />
                    <ItemStyle CssClass="JCOL-OCULTA ide_recetadet" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>                            
                        <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL JIMG-ELIMINAR" />                                    
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="Eliminar" Width="5%" />
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="JPAGINADO" />            
        </asp:GridView>
    </div>
    </form>
</body>
</html>
