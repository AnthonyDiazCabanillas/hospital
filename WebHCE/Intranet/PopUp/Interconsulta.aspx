<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Interconsulta.aspx.vb" Inherits="WebHCE.Interconsulta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#frnInterconsulta").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_POPU_VISI();
                $(".JCUERPO-POPUP").load("PopUp/Interconsulta.aspx?Atencion=" + $("#" + "<%= hfCodigoAtencion_Interconsulta.ClientID %>").val().trim(), {
                    Pagina: sNumeroPagina
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });


            $("#frnInterconsulta").find(".JIMG-GENERAL").click(function () {
                /*$("#hfInterconsulta").val("INTER");
                $("#frnInterconsulta").prop("action", "../../Login.aspx");
                $("#frnInterconsulta").prop("method", "POST");
                $("#frnInterconsulta").submit();*/
                if ($(this).parent().parent().find("td").eq(0).find("img").attr("src") == "../Imagenes/InterconRojo.png") {
                    IdInterconsultaR = $(this).parent().parent().find("td").eq(9).html().trim();
                    IdMotivoR = $(this).parent().parent().find("td").eq(10).html().trim();
                    CodEspecialidadR = $(this).parent().parent().find("td").eq(7).html().trim();
                    DescripcionSolicitudR = $(this).parent().parent().find("td").eq(11).html().trim();
                    DescripcionEspecialidadR = $(this).parent().parent().find("td").eq(4).html().trim();
                    NombreMedicoR = $(this).parent().parent().find("td").eq(12).html().trim().replace("&nbsp;", ""); //3

                    EspecialidadInter = $(this).parent().parent().find("td").eq(7).html().trim();
                    AtencionInter = $("#" + "<%= hfCodigoAtencion_Interconsulta.ClientID %>").val().trim();

                    aValores = [IdInterconsultaR + "|", IdMotivoR + "|", CodEspecialidadR + "|", DescripcionSolicitudR + "|", DescripcionEspecialidadR + "|", NombreMedicoR + "|", AtencionInter + "|", EspecialidadInter];
                    //fn_oculta_popup("fn_AbreVentanaAcceso");  JB                  
                }

            });


            $("#frnInterconsulta").find(".JSBTABLA tr td").dblclick(function () {
                if ($(this).index() != 0 && $(this).index() != 15) {
//                    codigo = $(this).parent().find("td").eq(10).html().trim();
//                    var aValores = [codigo];
                    //                    $.JPopUp("Detalle Interconsulta", "PopUp/DetalleInterconsulta.aspx", "1", "Cerrar", "fn_CierraPopupInterconsulta()", 50, "", aValores);                    
                    codigox = $(this).parent().find("td").eq(9).html().trim();
                    //fn_oculta_popup("fn_AbreDetalle");     JB
                }
                //
            });

        });
        //cambio
        
    </script>
</head>
<body>
<form id="frnInterconsulta" runat="server" class="JFORM-CONTENEDOR-POPUP">
    <input type="hidden" id="hfCodigoAtencion_Interconsulta" name="hfCodigoAtencion_Interconsulta" runat="server" />    
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_3">Lista de Interconsultas</span>
            </div>
        </div>        
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <div class="JSBDIV_TABLA" style="max-height:300px; min-height:100px; overflow-y:auto;">   
                    <asp:GridView ID="gvInterconsultaPopUp" runat="server" 
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                        GridLines="None" PagerStyle-CssClass="JPAGINADO" AllowPaging="True" 
                        PageSize="5" >
                        <Columns>
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <%--<asp:ImageButton ID="imbInfo" runat="server" CausesValidation="False" CommandName="Interconsulta"
                                        ImageUrl="~/Imagenes/Punto_Rojo.png" />--%>
                                    <img alt="" id="imgEstado" runat="server" src="" class="JIMG-GENERAL" />
                                </ItemTemplate>                                                                
                            </asp:TemplateField>
                            <asp:BoundField DataField="fec_solicitud" HeaderText="Fecha Sol." >
                                <ItemStyle CssClass="Fecha Sol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fec_respuesta" HeaderText="Fecha Resp." >
                                <ItemStyle CssClass="Fecha Sol" />
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
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>        
    </div>
</form>
</body>
</html>
