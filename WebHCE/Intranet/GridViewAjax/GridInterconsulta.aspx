<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridInterconsulta.aspx.vb" Inherits="WebHCE.GridInterconsulta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var codigo = "";

        $(document).ready(function () {
            $("#frmGridInterconsulta").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                    fn_CreaEventoGridInterconsulta();
                });
            });

            $("#frmGridInterconsulta").find(".JIMG-ELIMINAR").click(function () {
                codigo = $(this).parent().parent().find("td").eq(10).html().trim();
                $.JMensajePOPUP("Aviso", "¿Desea Eliminar la Interconsulta?", "ADVERTENCIA", "Si;No", "fn_EliminaInterconsulta();fn_oculta_mensaje()");
            });

            $("#frmGridInterconsulta").find(".JSBTABLA tr td").dblclick(function () {
                codigo = $(this).parent().find("td").eq(10).html().trim();
                if ($(this).index() != 0 && $(this).index() != 15) {
                    var aValores = [codigo];
                    $.JPopUp("Detalle Interconsulta", "PopUp/DetalleInterconsulta.aspx", "1", "Cerrar", "fn_CierraPopupInterconsulta()", 50, "", aValores);
                    fn_GuardaLog("INTERCONSULTA", "Visualizo detalle de interconsulta " + codigo);
                }
                //
            });



            if ('<%=Session("CodMedico") %>' == "0") {
                $("#frmGridInterconsulta").find(".JIMG-ELIMINAR").unbind("click");
                $("#frmGridInterconsulta").find(".JIMG-ESTADO").unbind("click");                
            }
        });


        function fn_EliminaInterconsulta() {
            if ('<%=Session("CodMedico") %>' == "0") {
                return;
            }
            fn_oculta_mensaje_rapido();
            fn_LOAD_GRID_VISI();
            //$(this).parent().parent().remove();
            $.ajax({
                url: "GridViewAjax/GridInterconsulta.aspx/EliminarInterconsulta",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    IdInterconsulta: codigo
                }),
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    //
                    $.JMensajePOPUP("Error", "Se elimino la interconsula correctamente.", "OK", "Cerrar", "fn_oculta_mensaje()");
                    fn_GuardaLog("INTERCONSULTA", "Se eliminó interconsulta nro " + codigo);
                    $("#divGridInterconsulta").load("GridViewAjax/GridInterconsulta.aspx", function () {
                        fn_LOAD_GRID_OCUL();
                    });
                } else {
                    $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                }
            });
        }
    </script>
</head>
<body>
    <form id="frmGridInterconsulta" runat="server" class="JFORM-CONTENEDOR-GRID">
    <div>
        <asp:GridView ID="gvInterconsulta" runat="server" AutoGenerateColumns="False" 
            ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" GridLines="None" 
            AllowPaging="True" PageSize="5" PagerStyle-CssClass="JPAGINADO" >
            <Columns>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <%--<asp:ImageButton ID="imbInfo" runat="server" CausesValidation="False" CommandName="Interconsulta"
                            ImageUrl="~/Imagenes/Punto_Rojo.png" />--%>
                        <img alt="" id="imgEstado" runat="server" src="" name="07/01/02" class="JIMG-GENERAL JIMG-ESTADO" />
                    </ItemTemplate>                                                                
                </asp:TemplateField>
                <asp:BoundField DataField="fec_solicitud" HeaderText="Fecha Sol." >
                    <ItemStyle CssClass="Fecha Sol" />
                </asp:BoundField>
                <asp:BoundField DataField="fec_respuesta" HeaderText="Fecha Resp." >
                    <ItemStyle CssClass="Fecha Sol" />
                </asp:BoundField>
                <asp:BoundField DataField="fec_real" HeaderText="Fecha Médico" >
                    <ItemStyle CssClass="Fecha Medico" />
                </asp:BoundField>
                <asp:BoundField DataField="medico" HeaderText="Medico Especialidad" >
                    <ItemStyle CssClass="MedicoEspecialidad" />
                </asp:BoundField>
                <asp:BoundField DataField="especialidad" HeaderText="Especialidad" >
                    <ItemStyle  CssClass="Especialidad" />
                </asp:BoundField>
                <asp:BoundField DataField="dsc_motivo" HeaderText="Motivo" >
                    <ItemStyle  CssClass="Motivo" />
                </asp:BoundField>
                <asp:BoundField DataField="ide_solicitado" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:BoundField DataField="codespecialidad" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:BoundField DataField="estado" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:BoundField DataField="ide_interconsulta" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:BoundField DataField="ide_motivo" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>       
                <asp:BoundField DataField="txt_solicitud" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:BoundField DataField="medico_solicita" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:BoundField DataField="ide_solicitante" HeaderText="" >
                    <ItemStyle  CssClass="JCOL-OCULTA" />
                    <HeaderStyle CssClass="JCOL-OCULTA"  />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <img alt="" src="../Imagenes/anular.gif" class="JIMG-GENERAL JIMG-ELIMINAR" />
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="Eliminar" Width="5%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
