<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridReconciliacionMedicamentosa.aspx.vb" Inherits="WebHCE.GridAntecedentesPatologicos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridGridReconciliacionMedicamentosa").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridReconciliacionMedicamentosa").load("GridViewAjax/GridReconciliacionMedicamentosa.aspx", {
                    Pagina: sNumeroPagina,
                    Medicamentos: $("#" + "<%=hfOpcion.ClientID %>").val().trim(),
                    IdPatologia: $("#" + "<%=hfIdPatologia.ClientID %>").val().trim()
                }, function () {
                    fn_LOAD_GRID_OCUL();
                    fn_CreaEventoGridReconciliacion();
                });
            });


            $("#frmGridGridReconciliacionMedicamentosa").find(".JIMG-GENERAL").click(function () {
                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(0).html().trim();
                $(this).parent().parent().remove();
                $.ajax({
                    url: "GridViewAjax/GridReconciliacionMedicamentosa.aspx/EliminarReconciliacionMedicamento",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        IdMedicamentosaDet: codigo
                    }),
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "OK") {
                        $("#divGridReconciliacionMedicamentosa").load("GridViewAjax/GridReconciliacionMedicamentosa.aspx", { Medicamentos: $("#" + "<%=hfOpcion.ClientID %>").val().trim(), IdPatologia: $("#" + "<%=hfIdPatologia.ClientID %>").val().trim() }, function () {
                            fn_LOAD_GRID_OCUL();
                            fn_CreaEventoGridReconciliacion();
                        });
                    } else {
                        $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    }

                });
            });



            //            $("#frmGridGridReconciliacionMedicamentosa").find(".JIMG-GENERAL").click(function () {
            //                fn_LOAD_GRID_VISI();
            //                var codigo = $(this).parent().parent().find("td").eq(0).html().trim();
            //                $(this).parent().parent().remove();
            //                $.ajax({
            //                    url: "GridViewAjax/GridProductoMedicamento.aspx/EliminarProductoMedicamento",
            //                    type: "POST",
            //                    contentType: "application/json; charset=utf-8",
            //                    dataType: "json",
            //                    data: JSON.stringify({
            //                        CodigoProductoMedicamento: codigo
            //                    }),
            //                    error: function (dato1, datos2, dato3) {

            //                    }
            //                }).done(function (oOB_JSON) {
            //                    fn_LOAD_GRID_OCUL();
            //                });
            //            });
        });
    </script>

</head>
<body>
    <form id="frmGridGridReconciliacionMedicamentosa" runat="server" class="JFORM-CONTENEDOR-GRID">
        <input type="hidden" id="hfOpcion" runat="server" />
        <input type="hidden" id="hfIdPatologia" runat="server" />
        <div class="JSBDIV_TABLA">   
            <asp:GridView ID="gvReconciliacionMedicamentosa" runat="server" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay registros disponibles..." >
                <Columns>
                    <asp:BoundField DataField="ide_medicamentosa_det" HeaderText="Codigo" >
                        <ItemStyle  CssClass="Codigo JCOL-OCULTA" />
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                    </asp:BoundField>
                    <asp:BoundField DataField="cod_producto" HeaderText="Codigo" >
                        <ItemStyle  CssClass="Codigo JCOL-OCULTA" />
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_producto" HeaderText="Medicamento" >
                        <ItemStyle  CssClass="Medicamento" />
                    </asp:BoundField>
                    <asp:BoundField DataField="num_dosis" HeaderText="Dosis" >
                        <ItemStyle  CssClass="Dosis" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_via" HeaderText="Via" >
                        <ItemStyle  CssClass="Via" />
                    </asp:BoundField>
                    <asp:BoundField DataField="num_frecuencia" HeaderText="Frecuencia" >
                        <ItemStyle  CssClass="Frecuencia" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dsc_accion" HeaderText="Acción" >
                        <ItemStyle  CssClass="Accion" />
                    </asp:BoundField>

                    <%--<asp:BoundField DataField="dsc_patologia" HeaderText="Antecedente" >
                        <ItemStyle  CssClass="Accion" />
                    </asp:BoundField>JB - COMENTADO - 24/01/2020--%>


                    <%--<asp:TemplateField HeaderText="Continua" >
                        <ItemStyle Width="60px" HorizontalAlign="Center" CssClass="Continua" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkContinua" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Suspende" >
                        <ItemStyle Width="60px" HorizontalAlign="Center" CssClass="Suspende" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSuspende" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Modifica" >
                        <ItemStyle Width="60px" HorizontalAlign="Center" CssClass="Modifica" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkModifica" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:BoundField DataField="flg_medicamento" HeaderText="Porta" >
                        <ItemStyle  CssClass="Porta JCOL-OCULTA" />
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                    </asp:BoundField>


                    <asp:BoundField DataField="ultima_dosis" HeaderText="Fecha y Hora" >
                        <ItemStyle  CssClass="Fecha y Hora" />
                    </asp:BoundField>
                    <%--<asp:TemplateField HeaderText="Eliminar">
                        <ItemStyle Width="60px" HorizontalAlign="Center" CssClass="Eliminar" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgAnularRegMedico" runat="server" ImageUrl="~/Imagenes/anular.gif"
                                CommandName="Borrar" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>                            
                            <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL" />                                    
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
