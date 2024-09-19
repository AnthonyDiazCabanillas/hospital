<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridSuspensionMedicamento.aspx.vb" Inherits="WebHCE.GridSuspensionMedicamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmGridSuspensionMedicamento").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridSuspensionMedicamento").load("GridViewAjax/GridSuspensionMedicamento.aspx", {
                    Pagina: sNumeroPagina,
                    IdReceta: $("#" + "<%=hfIdReceta_.ClientID %>").val()
                }, function () {
                    fn_LOAD_GRID_OCUL();


                    $.ajax({
                        url: "GridViewAjax/GridSuspensionMedicamento.aspx/ListaMedicamentosSuspender",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d != "") {
                            var xmlDoc = $.parseXML(oOB_JSON.d);
                            var xml = $(xmlDoc);
                            var ListaMedicamentosSuspender = xml.find("TablaListaMedicamentoSuspender");
                            $(ListaMedicamentosSuspender).each(function () {
                                var objetoLista = $(this);
                                $("[id*=gvSuspensionMedicamento]").find("tr").each(function () {
                                    var objeto = $(this);
                                    var IdMedicamento = objeto.find("td").eq(6).text().trim();
                                    if (IdMedicamento == objetoLista.find("IdeMedicamento").text()) {
                                        objeto.find(".JCHECK-SUSPENSION").prop("checked", true);
                                    }
                                });
                            });
                        }

                    });

                });
            });


            $("#frmGridSuspensionMedicamento").find(".JCHECK-SUSPENSION").click(function () {
                var IdeMedicamentoRec = $(this).parent().parent().find("td").eq(6).text().trim();
                var Tipo = "";

                if ($(this).prop("checked")) {
                    Tipo = "SI";
                } else {
                    Tipo = "NO";
                }


                $.ajax({
                    url: "GridViewAjax/GridSuspensionMedicamento.aspx/AgregarMedicamentosSuspender",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        IdeMedicamento: IdeMedicamentoRec,
                        Tipo: Tipo
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
    <form id="frmGridSuspensionMedicamento" runat="server" class="JFORM-CONTENEDOR-GRID">
        <input type="hidden" runat="server" id="hfIdReceta_" />
        <div class="JSBDIV_TABLA">
            <asp:GridView ID="gvSuspensionMedicamento" runat="server" 
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
                        <ItemStyle  CssClass="JCOL-OCULTA" Width="10%"  />
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                    </asp:BoundField>                                
                    <asp:BoundField DataField="num_cantidad" HeaderText="Cantidad" >
                        <ItemStyle  CssClass="JCOL-OCULTA" Width="10%"  />
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                    </asp:BoundField>
                    <asp:BoundField DataField="num_dosis" HeaderText="Dosis" >
                        <ItemStyle  CssClass="JCOL-OCULTA" Width="10%"   />
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_receta" HeaderText="">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA ide_receta" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ide_medicamentorec" HeaderText="">
                        <HeaderStyle CssClass="JCOL-OCULTA" />
                        <ItemStyle CssClass="JCOL-OCULTA ide_medicamentorec" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Suspender">
                        <ItemTemplate>
                            <input type="checkbox" id="chkSuspender" runat="server" class="JCHECK-SUSPENSION" />
                        </ItemTemplate>                                                                
                        <ItemStyle CssClass="Suspender" Width="5%"/>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
