<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridReceta.aspx.vb" Inherits="WebHCE.GridReceta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridReceta").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#frmGridReceta").load("GridViewAjax/GridReceta.aspx", {
                    Pagina: sNumeroPagina,
                    IdReceta: $("#" + "<%=hfIdRecetaG.ClientID%>").val().trim()
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });


            $("#frmGridReceta").find(".JIMG-ELIMINAR").click(function () {
                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(7).html().trim();                
                $.ajax({
                    url: "GridViewAjax/GridReceta.aspx/EliminarReceta",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        Codigo: codigo
                    }),
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "OK") {
                        //SI SE ESTA ELIMINANDO UN CODIGO QUE YA FUE SELECCINADO PARA EDITAR, DEBERA LIMPIAR LOS CAMPOS
                        if (codigo == $("#hfIdRecetaDet").val().trim()) {
                            $("#txtCadaHoraReceta").val("");
                            $("#txtProductoReceta").val("");
                            $("#txtDosisReceta").val("");
                            $("#txtDiaReceta").val("");
                            $("#txtIndicacionesReceta").val("");
                            $("#txtCantidadReceta").val("");
                            $("#hfCodigoProductoReceta").val("");
                            $("#hfIdRecetaDet").val("");
                        }

                        $("#frmGridReceta").load("GridViewAjax/GridReceta.aspx", {
                            Pagina: 1,
                            IdReceta: $("#" + "<%=hfIdRecetaG.ClientID%>").val().trim()
                        }, function () {
                            fn_LOAD_GRID_OCUL();
                        });
                    } else {
                        $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmGridReceta");
                    }

                });
            });

        });
    </script>
</head>
<body>
    <form id="frmGridReceta" runat="server" class="JFORM-CONTENEDOR-GRID">
        <input type="hidden" id="hfIdRecetaG" runat="server" />
        <div class="JSBDIV_TABLA">   
            <asp:GridView ID="gvProductoMedicamento" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay Productos agregados." >
                <Columns>
                    <asp:BoundField DataField="cod_producto" HeaderText="Codigo" >
                        <HeaderStyle  CssClass="JCOL-OCULTA" />
                        <ItemStyle  CssClass="JCOL-OCULTA Codigo" />
                    </asp:BoundField>

                    <asp:BoundField DataField="nproducto" HeaderText="Producto" >
                        <ItemStyle  CssClass="Producto"  />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Producto" HeaderText="DCI" >
                        <ItemStyle  CssClass="DCI"  />
                    </asp:BoundField>--%>
                    <%--<asp:BoundField DataField="UnidMedida" HeaderText="Unid. Medida" >
                        <ItemStyle  CssClass="Unid. Medida" Width="10%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="num_frecuencia" HeaderText="Cada (hrs)" >
                        <ItemStyle  CssClass="Cada (hrs)" Width="10%" HorizontalAlign="Center"  />
                    </asp:BoundField>       
                    <asp:BoundField DataField="num_duracion" HeaderText="Tratamiento" >
                        <ItemStyle  CssClass="Dia" Width="10%" HorizontalAlign="Center"/>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Via" HeaderText="Via" >
                        <ItemStyle  CssClass="Via" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="num_dosis" HeaderText="Dosis" >
                        <ItemStyle  CssClass="Dosis" HorizontalAlign="Center"/>
                    </asp:BoundField>                 
                    <asp:BoundField DataField="num_cantidad" HeaderText="Cantidad" >
                        <ItemStyle  CssClass="Cantidad" Width="10%" HorizontalAlign="Center"  />
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>                    
                    <asp:BoundField DataField="txt_detalle" HeaderText="Indicaciones">
                        <ItemStyle CssClass="Indicaciones" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_medicamentorec" HeaderText="Detalle" >
                        <HeaderStyle  CssClass="JCOL-OCULTA" />
                        <ItemStyle  CssClass="JCOL-OCULTA Detalle" />
                    </asp:BoundField>
                    <%--<asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>                            
                            <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL" />
                        </ItemTemplate>
                        <ItemStyle CssClass="Eliminar" Width="5%" />
                    </asp:TemplateField>--%>
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
