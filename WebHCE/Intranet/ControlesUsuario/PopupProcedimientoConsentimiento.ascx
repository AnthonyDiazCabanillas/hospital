<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PopupProcedimientoConsentimiento.ascx.vb" Inherits="WebHCE.PopupProcedimientoConsentimiento" %>


<script type="text/javascript">
    $(document).ready(function () {
        $("[id*=gvListaProcedimientoConsentimiento]").find("tr").click(function () {
            $("[id*=gvListaProcedimientoConsentimiento]").find("tr").each(function () {
                $(this).find("td").removeClass("JFILA-SELECCIONADA");
            });
            if ($(this).attr("class") != undefined && $(this).attr("class") != "") {
                return;
            }
            $(this).find("td").addClass("JFILA-SELECCIONADA");

            $("[id*=hfProcedimientoSeleccionado]").val($(this).find(".IdPlantilla").html().trim() + ";" + $(this).find(".NumProc").html().trim()); //JB - CAMPO EN IMPRESIONREPORTE.ASPX                        
        });
    });


    function fn_CancelaSeleccionConsentimiento() {
        $("[id*=gvListaProcedimientoConsentimiento]").find("tr").each(function () {
            $(this).find("td").removeClass("JFILA-SELECCIONADA");
        });
        $("[id*=hfProcedimientoSeleccionado]").val("");
        fn_OcultarPopup2("divPopUpProcedimientoConsentimiento");
    }

    function fn_AceptarSeleccionConsentimiento() {
        $("[id*=gvListaProcedimientoConsentimiento]").find("tr").each(function () {
            $(this).find("td").removeClass("JFILA-SELECCIONADA");
        });
        fn_OcultarPopup2("divPopUpProcedimientoConsentimiento");
        window.open("VisorReporte.aspx?OP=" + "CI" + "&Valor=" + $("[id*=hfProcedimientoSeleccionado]").val());
    }

</script>

<asp:GridView ID="gvListaProcedimientoConsentimiento" runat="server" AlternatingRowStyle-CssClass="altrowstyle"
    AutoGenerateColumns="False" CssClass="JSBTABLA" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="codprocedimiento">
    <Columns>
        <asp:BoundField DataField="codprocedimiento" HeaderText="Num. Proc.">
            <ItemStyle Width="10%" HorizontalAlign="Center" CssClass="NumProc" />
            <HeaderStyle Width="10%" />
        </asp:BoundField>
        <asp:BoundField DataField="plantilla" HeaderText="Nombre Proc.">
            <ItemStyle HorizontalAlign="Left" Width="10%" />
            <HeaderStyle Width="10%" />
        </asp:BoundField>
        <asp:BoundField DataField="fechagenera" HeaderText="Fecha">
            <ItemStyle HorizontalAlign="Left" Width="10%" />
            <HeaderStyle Width="10%" />
        </asp:BoundField>
        <asp:BoundField DataField="nomrepresentante" HeaderText="Nombre">
            <ItemStyle HorizontalAlign="Left" Width="20%" />
            <HeaderStyle Width="20%" />
        </asp:BoundField>
        <asp:BoundField DataField="nom_medico" HeaderText="Médico">
            <ItemStyle HorizontalAlign="Left" Width="20%" />
            <HeaderStyle Width="20%" />
        </asp:BoundField>
        <asp:BoundField DataField="coddiagnostico" HeaderText="Cod. Diagnostico">
            <ItemStyle HorizontalAlign="Left" Width="10%" />
            <HeaderStyle Width="10%" />
        </asp:BoundField>
        <asp:BoundField DataField="nom_diagnostico" HeaderText="Diagnostico">
            <ItemStyle HorizontalAlign="Left" Width="30%" />
            <HeaderStyle Width="30%" />
        </asp:BoundField>                
        <asp:BoundField DataField="IdPlantilla" HeaderText="Plantilla">
            <ItemStyle HorizontalAlign="Left" CssClass="IdPlantilla JCOL-OCULTA" />
            <HeaderStyle CssClass="JCOL-OCULTA" />
        </asp:BoundField>
    </Columns>
</asp:GridView>