<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PopUpFechaReceta.ascx.vb" Inherits="WebHCE.PopUpFechaReceta" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("[id*=gvListaFechaReceta]").find("tr").click(function () {
            $("[id*=gvListaFechaReceta]").find("tr").each(function () {
                $(this).find("td").removeClass("JFILA-SELECCIONADA");
            });
            if ($(this).attr("class") != undefined && $(this).attr("class") != "") {
                return;
            }
            $(this).find("td").addClass("JFILA-SELECCIONADA");

            $("[id*=hfFechaRecetaSeleccionado]").val($(this).find(".FEC_REGISTRO").html().trim()); //JB - CAMPO EN IMPRESIONREPORTE.ASPX                        
        });
    });

    function fn_CancelarSeleccionFechaReceta() {
        $("[id*=gvListaFechaReceta]").find("tr").each(function () {
            $(this).find("td").removeClass("JFILA-SELECCIONADA");
        });
        $("[id*=hfFechaRecetaSeleccionado]").val("");
        fn_OcultarPopup2("divPopUpFechaReceta");
    }

    function fn_AceptarSeleccionFechaReceta() {
        if ($("[id*=gvListaFechaReceta]").find(".JFILA-SELECCIONADA").length > 0) {
            $("[id*=gvListaFechaReceta]").find("tr").each(function () {
                $(this).find("td").removeClass("JFILA-SELECCIONADA");
            });
            fn_OcultarPopup2("divPopUpFechaReceta");
            window.open("VisorReporte.aspx?OP=" + "IM" + "&Valor=FE2;" + $("[id*=hfFechaRecetaSeleccionado]").val());
        }        
    }
</script>

<asp:GridView ID="gvListaFechaReceta" runat="server" 
    AutoGenerateColumns="False" CssClass="JSBTABLA" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="FEC_REGISTRO">
    <Columns>
        <asp:BoundField DataField="FEC_REGISTRO" HeaderText="Fecha de Receta">
            <ItemStyle Width="10%" HorizontalAlign="Center" CssClass="FEC_REGISTRO" />
            <HeaderStyle Width="10%" />
        </asp:BoundField>        
    </Columns>
</asp:GridView>