<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridDiagnostico.aspx.vb" Inherits="WebHCE.GridDiagnostico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#frmDiagnostico").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_GRID_VISI();
                $("#divGridDiagnosticos").load("GridViewAjax/GridDiagnostico.aspx", {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });

            if ('<%=Session("CodMedico") %>' == "0") {
                $("#frmDiagnostico").find("input[type='radio']").attr("disabled", "disabled");                
            }


            $("#frmDiagnostico").find(".JIMG-GENERAL").click(function () {
                if ('<%=Session("CodMedico") %>' == "0") {
                    return;
                }

                fn_LOAD_GRID_VISI();
                var codigo = $(this).parent().parent().find("td").eq(0).html().trim();
                var objeto = $(this);
                var sTipoD = $(this).parent().parent().find("td").eq(6).html().trim();

                $.ajax({
                    url: "GridViewAjax/GridDiagnostico.aspx/EliminarDiagnostico",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        CodigoDiagnostico: codigo,
                        TipoD: sTipoD
                    }),
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    objeto.parent().parent().remove();
                    $("#divGridDiagnosticos").load("GridViewAjax/GridDiagnostico.aspx", function () {
                        fn_LOAD_GRID_OCUL();
                    });
                    /*if (oOB_JSON.d != "") {
                    $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                    $(this).parent().parent().remove();
                    }*/
                });
            });



            $("#frmDiagnostico").find("input[type='radio']").click(function () {
                var TipoDiag = "";
                var CodigoDiag = "";
                CodigoDiag = $(this).parent().parent().find("td").eq(0).html().trim();
                var TipoIE = $(this).parent().parent().find("td").eq(6).html().trim();
                if ($(this).val().trim() == "chkPresuntivo") {
                    TipoDiag = "P";
                }
                if ($(this).val().trim() == "chkRepetido") {
                    TipoDiag = "R";
                }
                if ($(this).val().trim() == "chkDefinitivo") {
                    TipoDiag = "D";
                }
                if (TipoDiag != "") {
                    $.ajax({
                        url: "GridViewAjax/GridDiagnostico.aspx/ActualizarTipoDiagnostico",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            TipoDiagnostico: TipoDiag,
                            CodigoDiagnostico: CodigoDiag,
                            TipoIE: TipoIE
                        }),
                        error: function (dato1, datos2, dato3) {

                        }
                    }).done(function (oOB_JSON) {
                        fn_LOAD_GRID_OCUL();
                        if (oOB_JSON.d != "OK") {
                            $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()");
                        }

                    });
                }

            });

        });
    </script>
</head>
<body>
    <form id="frmDiagnostico" runat="server" class="JFORM-CONTENEDOR-GRID">
    <div class="JSBDIV_TABLA">
        <asp:GridView ID="gvDiagnosticos" runat="server"
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
            GridLines="None" AllowPaging="True" PageSize="5" emptydatatext="No hay Productos agregados." >
            <Columns>
                <asp:BoundField DataField="coddiagnostico" HeaderText="CIE-10">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="nombre" HeaderText="Diagnóstico">
                    <ItemStyle HorizontalAlign="Left" Width="65%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="P">
                    <ItemTemplate>                                    
                        <%--<input type="checkbox" id="chkPresuntivo" runat="server" />--%>
                        <input type="radio" id="chkPresuntivo" runat="server" name="TipoD" /><%--07/02/2017 deshabilitado--%>
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="P"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="R">
                    <ItemTemplate>                                    
                        <%--<input type="checkbox" id="chkRepetido" runat="server" /> --%>
                        <input type="radio" id="chkRepetido" runat="server" name="TipoD" /><%--07/02/2017 deshabilitado--%>
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="R"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="D">
                    <ItemTemplate>                                    
                        <%--<input type="checkbox" id="chkDefinitivo" runat="server" />--%>                                 
                        <input type="radio" id="chkDefinitivo" runat="server" name="TipoD" /><%--07/02/2017 deshabilitado--%>
                    </ItemTemplate>                                                                
                    <ItemStyle CssClass="D"/>
                </asp:TemplateField>
                <asp:BoundField DataField="tipodiagnostico" HeaderText="tipodiagnostico">
                    <HeaderStyle CssClass="JCOL-OCULTA" />
                    <ItemStyle CssClass="JCOL-OCULTA tipodiagnostico" />
                </asp:BoundField>
                <asp:BoundField DataField="tipo" HeaderText="tipo">
                    <ItemStyle HorizontalAlign="Left" Width="10%" CssClass="tipo" />
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
