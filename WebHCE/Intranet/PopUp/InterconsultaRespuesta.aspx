<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InterconsultaRespuesta.aspx.vb" Inherits="WebHCE.InterconsultaRespuesta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            fn_CampoBase();
            $.JValidaCampoObligatorio("<%=txtDescripcionInterconsulta2R.ClientID %>");

            $("#frmInterconsultaRespuesta").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));

                fn_LOAD_POPU_VISI();
                $(".JCUERPO-POPUP").load("PopUp/InterconsultaRespuesta.aspx", {
                    Pagina: sNumeroPagina,
                    "Parametro[]": $("#" + "<%=hfParametrosInterconsulta.ClientID %>").val()
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });


            $("#btnResponderInterconsultaR").click(function () {
                $.ajax({ url: "PopUp/InterconsultaRespuesta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        if ($("#" + "<%=txtDescripcionInterconsulta2R.ClientID %>").val().trim() == "" || $("#" + "<%=txtFechaInterconsultRespuesta.ClientID %>").val().trim() == "" || $("#" + "<%=txtHoraInterconsultRespuesta.ClientID %>").val().trim() == "") {
                            return;
                        }
                        fn_LOAD_POPU_VISI();
                        $.ajax({
                            url: "PopUp/InterconsultaRespuesta.aspx/GuardarRespuestaInterconsulta",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                Respuesta: $("#" + "<%=txtDescripcionInterconsulta2R.ClientID %>").val(),
                                IdInterconsultaR: $("#" + "<%=hfIdInterconsultaRespuesta.ClientID %>").val(),
                                FechaInterconsultaR: $("#" + "<%=txtFechaInterconsultRespuesta.ClientID %>").val(),
                                HoraInterconsultaR: $("#" + "<%=txtHoraInterconsultRespuesta.ClientID %>").val()
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {

                            }
                        }).done(function (oOB_JSON) {
                            fn_LOAD_POPU_OCUL();
                            if (oOB_JSON.d != "") {
                                $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmInterconsultaRespuesta");
                            } else {
                                $.JMensajePOPUP("Aviso", "Se guardó la respuesta correctamente.", "OK", "Cerrar", "fn_CierraPopUpRespuestaInter()", "frmInterconsultaRespuesta");
                            }
                        });
                    } else {
                        //Observaciones Cmendez 02/05/2022
                        aValores = oOB_JSON.d.toString().split("|");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_InterconsultaRespuesta()", "frmInterconsultaRespuesta");
                    }
                });
            });


            $("#frmInterconsultaRespuesta").find(".JIMG-ESTADO").click(function () {
                var objeto = $(this);
                var estado = objeto.parent().parent().find("td").eq(8).html().trim();
                if (estado != "P") {
                    return;
                }
                var CodigoEsp = objeto.parent().parent().find("td").eq(7).html().trim();
                $.ajax({ url: "PopUp/InterconsultaRespuesta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA
                        fn_LOAD_POPU_VISI();
                        $.ajax({
                            url: "InformacionPaciente.aspx/ValidaEspecialidadInterconsulta",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({
                                CodigoEspecialidad: CodigoEsp
                            }),
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d != "") {
                                fn_LOAD_POPU_OCUL();
                                //Observaciones Cmendez 02/05/2022
                                if (oOB_JSON.d.toString().split("|").length > 1) {
                                    $.JMensajePOPUP("Error", oOB_JSON.d.toString().split("|")[2], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmInterconsultaRespuesta");
                                } else {
                                    $.JMensajePOPUP("Aviso", oOB_JSON.d, "", "Cerrar", "fn_oculta_mensaje()", "frmInterconsultaRespuesta");
                                }
                            } else {
                                var IdInterconsultaR = objeto.parent().parent().find("td").eq(9).html().trim();
                                var IdMotivoR = objeto.parent().parent().find("td").eq(10).html().trim();
                                var CodEspecialidadR = objeto.parent().parent().find("td").eq(7).html().trim();
                                var DescripcionSolicitudR = objeto.parent().parent().find("td").eq(11).html().trim();
                                var DescripcionEspecialidadR = objeto.parent().parent().find("td").eq(4).html().trim();
                                var NombreMedicoR = objeto.parent().parent().find("td").eq(12).html().trim().replace("&nbsp;", "");   //3                         
                                var aValores = [IdInterconsultaR + "|", IdMotivoR + "|", CodEspecialidadR + "|", DescripcionSolicitudR + "|", DescripcionEspecialidadR + "|", NombreMedicoR]
                                //cambio
                                //$.JPopUp("Respuesta  ", "PopUp/InterconsultaRespuesta.aspx", "1", "Cerrar", "fn_oculta_popup()", 80, "", aValores);
                                $(".JCUERPO-POPUP").load("PopUp/InterconsultaRespuesta.aspx", {
                                    Pagina: 1,
                                    "Parametro[]": aValores
                                }, function () {
                                    fn_LOAD_GRID_OCUL();
                                });
                            }
                        });
                    } else {
                        //Observaciones Cmendez 02/05/2022
                        aValores = oOB_JSON.d.toString().split("|");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession_InterconsultaRespuesta()", "frmInterconsultaRespuesta");
                    }
                });
            });

        });

        function fn_CierraPopUpRespuestaInter() {
            fn_oculta_mensaje();
            fn_LOAD_POPU_VISI();
            var sValoresNuevos = "";
            //Observaciones Cmendez 02/05/2022 
            sValoresNuevos = $("#" + "<%=hfParametrosInterconsulta.ClientID %>").val() + "|" + $("#" + "<%=txtDescripcionInterconsulta2R.ClientID %>").val() + "|" + $("#" + "<%=txtFechaInterconsultRespuesta.ClientID %>").val() + "|" + $("#" + "<%=txtHoraInterconsultRespuesta.ClientID %>").val();
            $("#" + "<%=hfParametrosInterconsulta.ClientID %>").val(sValoresNuevos);

            $(".JCUERPO-POPUP").load("PopUp/InterconsultaRespuesta.aspx", {
                Pagina: 1,
                "Parametro[]": $("#" + "<%=hfParametrosInterconsulta.ClientID %>").val()
            }, function () {
                $("#" + "<%=txtFechaInterconsultRespuesta.ClientID %>").prop("disabled", "disabled");
                $("#" + "<%=txtHoraInterconsultRespuesta.ClientID %>").prop("disabled", "disabled");
                $("#" + "<%=txtDescripcionInterconsulta2R.ClientID %>").attr("disabled", "disabled");
                $("#btnResponderInterconsultaR").prop("disabled", "disabled");
                fn_LOAD_GRID_OCUL();
            });
        }

        function fn_ExpiraSession_InterconsultaRespuesta() {
            window.location.href = aValores[1];
        }

        function fn_CierraDetalle() {
            CargarGrilla(1);
            fn_oculta_popup();            
        }

        //Cmendez 03/05/2022 
        $(".JTEXTO").keypress(function (e) {
            var ValidaTilde = /[|'><&]/;
            if (ValidaTilde.test(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
        $(".JTEXTO").blur(function () {
            for (var i = 0; i < this.value.length; i++) {
                if (this.value.includes('|')) {
                    $(this).val($(this).val().replace("|", ""));
                    i--;
                }
                if (this.value.includes("'")) {
                    $(this).val($(this).val().replace("'", ""));
                    i--;
                }
                if (this.value.includes("<")) {
                    $(this).val($(this).val().replace("<", ""));
                    i--;
                }
                if (this.value.includes(">")) {
                    $(this).val($(this).val().replace(">", ""));
                    i--;
                }
                if (this.value.includes("&")) {
                    $(this).val($(this).val().replace("&", ""));
                    i--;
                }
            }
        });
        //Fin
    </script>
</head>
<body>
    <form id="frmInterconsultaRespuesta" runat="server" class="JFORM-CONTENEDOR-POPUP">
    <input type="hidden" id="hfIdInterconsultaRespuesta" runat="server" />
    <input type="hidden" id="hfParametrosInterconsulta" runat="server" />
    <div class="JFILA">
        <div class="JCELDA-5">
            <fieldset>
                <legend>Solicita</legend>
                <div class="JDIV-CONTROLES">
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Motivo:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <asp:DropDownList runat="server" ID="ddlMotivoR" CssClass="JSELECT"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Especialidad:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtEspecialidadInterconsultaR" class="JTEXTO" />
                                <input type="hidden" id="hfCodEspecialidadInterconsultaR" runat="server" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <div class="JDIV-CONTROLES">
                                <img src="../Imagenes/Buscar.png" id="imgBusquedaEspecialidadInterconsultaR" alt="" class="JIMG-BUSQUEDA" style="opacity:0.6" />
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Medico Interconsulta:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtMedicoInterconsultaR" class="JTEXTO" />
                                <input type="hidden" runat="server" id="txtCodMedicoInterconsultaR" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <div class="JDIV-CONTROLES">
                                <img src="../Imagenes/Buscar.png" id="imgBusquedaMedicoInterconsultaR" alt="" class="JIMG-BUSQUEDA" style="opacity:0.6" />
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <div class="JDIV-CONTROLES">
                                <textarea rows="5" cols="1" runat="server" id="txtDescripcionInterconsultaR" class="JTEXTO"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Respuesta</legend>
                <div class="JDIV-CONTROLES">
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Fecha / Hora:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-3">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtFechaInterconsultRespuesta" class="JFECHA" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtHoraInterconsultRespuesta" class="JHORA" />
                            </div>                                                
                        </div>


                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Especialidad:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtEspecialidadInterconsulta2R" class="JTEXTO" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <div class="JDIV-CONTROLES">
                                <%--<img src="../Imagenes/Buscar.png" id="img1" alt="" class="JIMG-BUSQUEDA" />--%>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Medico Interconsulta:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtMedicoInterconsulta2R" class="JTEXTO" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <div class="JDIV-CONTROLES">
                                <%--<img src="../Imagenes/Buscar.png" id="img2" alt="" class="JIMG-BUSQUEDA" />--%>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <div class="JDIV-CONTROLES">
                                <textarea rows="5" cols="1" runat="server" id="txtDescripcionInterconsulta2R" class="JTEXTO"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <div class="JDIV-CONTROLES">
                                <input type="button" value="Enviar" id="btnResponderInterconsultaR" />
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        
        </div>
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <div class="JFILA">
                    <div class="JCELDA-6">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_3">Historial Interconsultas</span>
                        </div>                                                
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                           <asp:GridView ID="gvInterconsultaR" runat="server" AutoGenerateColumns="False" 
                                ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" GridLines="None" 
                                AllowPaging="True" PageSize="5" PagerStyle-CssClass="JPAGINADO" >
                               <Columns>
                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton ID="imbInfo" runat="server" CausesValidation="False" CommandName="Interconsulta"
                                                ImageUrl="~/Imagenes/Punto_Rojo.png" />--%>
                                            <img alt="" id="imgEstado" runat="server" src="" class="JIMG-GENERAL JIMG-ESTADO" />
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
        </div>
    </div>
    </form>
</body>
</html>
