<!-- **********************************************************************************************************************
    Copyright Clinica San Felipe S.A.C 2023. Todos los derechos reservados.
    Version     Fecha           Autor       Requerimiento
    1.1         20/10/2023      AROMERO     REQ-2023-017255:  REPORTE HISTORIA CLINICA HOPITAL
**********************************************************************************************************************  -->

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImpresionReporte.aspx.vb" Inherits="WebHCE.ImpresionReporte" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        var ReporteImprimir = "";
        var ReporteImprimir2 = "";
        var aValorRedir = "";

        $(document).ready(function () {
            if ($("#" + "<%= hfReporteExistente.ClientID %>").val() != "") {

                $("#frmImpresionReporte").find("input[type='checkbox']").each(function () {
                    for (var i = 0; i < $("#" + "<%= hfReporteExistente.ClientID %>").val().split(";").length; i++) {
                        if ($(this).attr("name") == $("#" + "<%= hfReporteExistente.ClientID %>").val().split(";")[i].toString().trim()) {
                            $(this).attr("disabled", true);
                        }
                    }
                });

                $("#frmImpresionReporte").find("input[type='button']").each(function () {
                    for (var i = 0; i < $("#" + "<%= hfReporteExistente.ClientID %>").val().split(";").length; i++) {
                        if ($(this).attr("name") == $("#" + "<%= hfReporteExistente.ClientID %>").val().split(";")[i].toString().trim()) {
                            $(this).attr("disabled", true);
                        }
                    }
                });

            }

            //$("#frmImpresionReporte").parent().find("footer").find("input[type='button']").removeAttr("disabled");
            //siempre deben estar habilitado estos botones
            $("#frmImpresionReporte").parent().parent().find("footer").find("input[type='button']").each(function () {                
                $(this).removeAttr("disabled");
            });

            $("#chkMarcarTodo").click(function () {
                if ($(this).prop("checked")) {
                    $("#frmImpresionReporte").find("input[type='checkbox']:enabled").prop("checked", true);
                } else {
                    $("#frmImpresionReporte").find("input[type='checkbox']:enabled").prop("checked", false);
                }
            });

            $("#imgConsentimientoInformadoReporte").click(function () {
                //ReporteImprimir = "CI";
                //fn_Imprimir("NO");
                //fn_oculta_popup("fn_MostrarPopUpConsentimiento");
                fn_MostrarPopUpConsentimiento();
            });
            $("#imgInformeMedicoReporte").click(function () {
                ReporteImprimir = "EP";
                fn_Imprimir2("NO");
            });
            $("#imgDeclaratorioAlergiaReporte").click(function () {
                ReporteImprimir = "DA";
                fn_Imprimir2("NO");
            });
            $("#imgEvolucionClinicaReporte").click(function () {
                ReporteImprimir = "EC";
                fn_Imprimir2("NO");
            });
            $("#imgRegistroMedicoReconsiliacionReporte").click(function () {
                ReporteImprimir = "ME";
                fn_Imprimir2("NO");
            });
            $("#imgInterconsultaReporte").click(function () {
                ReporteImprimir = "IN";
                fn_Imprimir2("NO");
            });
            $("#imgIndicacionMedicaReporte").click(function () {
                //                ReporteImprimir = "IM";
                //                fn_Imprimir("NO");
                fn_MostrarPopUpFechaReceta();
            });
            $("#imgRecetaAlta").click(function () {
                ReporteImprimir = "RA";
                fn_Imprimir2("NO");
            });

            $("#imgJuntaMedica").click(function () {
                ReporteImprimir = "JM";
                fn_Imprimir2("NO");
            });
            $("#imgNotaIngreso").click(function () {
                ReporteImprimir = "NI";
                fn_Imprimir2("NO");
            });
            //1.1 INI
            $("#imgHistoriaClinica").click(function () {
                debugger;
                ReporteImprimir = "HC";
                fn_Imprimir2("NO");
            });
            //1.1 FIN
        });

        function fn_Imprimir(Limpiar) {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA
                    if (Limpiar == undefined || Limpiar == null) {
                        ReporteImprimir = "";
                    }

                    if ($("#chkConsentimientoInformadoReporte").prop("checked")) {
                        //ReporteImprimir += "CI;";
                    }
                    if ($("#chkDeclaratorioAlergiaReporte").prop("checked")) {
                        ReporteImprimir += "DA;"; //declaratoria alergia
                    }
                    if ($("#chkRegistroMedicoReconsiliacionReporte").prop("checked")) {
                        ReporteImprimir += "ME;"; //Medicamentosa
                    }
                    if ($("#chkIndicacionMedicaReporte").prop("checked")) {
                        //ReporteImprimir += "IM;"; //PE1
                    }
                    if ($("#chkEvolucionClinicaReporte").prop("checked")) {
                        ReporteImprimir += "EC;"; //EC1
                    }
                    if ($("#chkInterconsultaReporte").prop("checked")) {
                        ReporteImprimir += "IN;"; //IN1
                    }
                    //TMACASSI 27/10/2016 EPICRISIS
                    if ($("#chkInformeMedicoReporte").prop("checked")) {
                        ReporteImprimir += "EP;"; //EPICRISIS IM1
                    }
                    if ($("#chkRecetaAlta").prop("checked")) {
                        ReporteImprimir += "RA;";
                    }
                    if ($("#chkReporteJuntaMedica").prop("checked")) {
                        ReporteImprimir += "JM;";
                    }
                    if ($("#chkReporteNotaIngreso").prop("checked")) {
                        ReporteImprimir += "NI;";
                    }
                    //1.1 INI
                    if ($("#chkHistoriaClinica").prop("checked")) {
                        ReporteImprimir += "HC;";
                    }
                    //1.1 FIN
                    
                    for (var i = 0; i < ReporteImprimir.split(";").length; i++) {
                        if (ReporteImprimir.split(";")[i] == "CI") { //JB - 22/04/2019 - ReporteImprimir.split(";")[i] == "CI" || ReporteImprimir.split(";")[i] == "EC"
                            //alert(ReporteImprimir.split(";")[i] + "-" + i);
                            var DatoEnviar = ReporteImprimir.split(";")[i];
                            $.ajax({
                                url: "PopUp/ImpresionReporte.aspx/VerificarCantidadReportes",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({
                                    ReporteImprimir: ReporteImprimir.split(";")[i]
                                }),
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d.split(";")[0] == "ERROR") {
                                    $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmImpresionReporte");
                                } else {
                                    if (oOB_JSON.d.toString().trim() != "") { //SOLO SI LA CONSULTA DEVUELVE REGISTROS                                
                                        for (var x = 0; x < oOB_JSON.d.split(";").length; x++) {
                                            if (oOB_JSON.d.split(";")[x].toString().trim() != "") {
                                                window.open("VisorReporte.aspx?OP=" + DatoEnviar + "&Valor=" + oOB_JSON.d.split(";")[x].toString());
                                            }
                                        }
                                    }
                                }
                            });
                        } else {
                            if (ReporteImprimir.split(";")[i] != "") {
                                window.open("VisorReporte.aspx?OP=" + ReporteImprimir.split(";")[i] + "&Valor=");
                            }
                        }
                    }
                } else {
                    aValorRedir = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe - Advertencia", "Su sesion ha expirado.", "ADVERTENCIA", "Aceptar", "fn_SessionExpiroReporte()", "frmImpresionReporte");
                }
            });
        }

        function fn_Imprimir2(Limpiar) {
            $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //AQUI EL CODIGO SI LA SESSION AUN NO EXPIRA                    
                    for (var i = 0; i < ReporteImprimir.split(";").length; i++) {
                        if (ReporteImprimir.split(";")[i] == "CI") { //JB - 22/04/2019 - ReporteImprimir.split(";")[i] == "CI" || ReporteImprimir.split(";")[i] == "EC"
                            //alert(ReporteImprimir.split(";")[i] + "-" + i);
                            var DatoEnviar = ReporteImprimir.split(";")[i];
                            $.ajax({
                                url: "PopUp/ImpresionReporte.aspx/VerificarCantidadReportes",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({
                                    ReporteImprimir: ReporteImprimir.split(";")[i]
                                }),
                                error: function (dato1, datos2, dato3) {
                                }
                            }).done(function (oOB_JSON) {
                                if (oOB_JSON.d.split(";")[0] == "ERROR") {
                                    $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe - Error", oOB_JSON.d.split(";")[1], "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmImpresionReporte");
                                } else {
                                    if (oOB_JSON.d.toString().trim() != "") { //SOLO SI LA CONSULTA DEVUELVE REGISTROS                                
                                        for (var x = 0; x < oOB_JSON.d.split(";").length; x++) {
                                            if (oOB_JSON.d.split(";")[x].toString().trim() != "") {
                                                window.open("VisorReporte.aspx?OP=" + DatoEnviar + "&Valor=" + oOB_JSON.d.split(";")[x].toString());
                                            }
                                        }
                                    }
                                }
                            });
                        } else {
                            if (ReporteImprimir.split(";")[i] != "") {
                                window.open("VisorReporte.aspx?OP=" + ReporteImprimir.split(";")[i] + "&Valor=");
                            }
                        }
                    }
                } else {
                    aValorRedir = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Mensaje del sistema Clinica San Felipe - Advertencia", "Su sesion ha expirado.", "ADVERTENCIA", "Aceptar", "fn_SessionExpiroReporte()", "frmImpresionReporte");
                }
            });
        }

        function fn_SessionExpiroReporte() {
            window.location.href = aValorRedir[1];
        }

        function fn_MostrarPopUpConsentimiento() {
            //$("#divContenedorProcedimientoConsentimiento").css("display", "inline");
            fn_MostrarPopup2("divPopUpProcedimientoConsentimiento", true);
        }

        function fn_MostrarPopUpFechaReceta() {
            //$("#divContenedorProcedimientoConsentimiento").css("display", "inline");
            fn_MostrarPopup2("divPopUpFechaReceta", true);
        }
    </script>
</head>
<body>
    <form id="frmImpresionReporte" runat="server">
     <div class="JFILA">
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgConsentimientoInformadoReporte" class="JBOTON-IMAGEN" name="CI" style="background-image:url(../Imagenes/Consentimiento1.png);width:64px;height:64px;background-size:64px 64px;" />  <%--ico_consentimiento.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkConsentimientoInformadoReporte" disabled="disabled" name="CI" style="display:none;" />&nbsp;&nbsp;&nbsp;&nbsp;<label for="chkConsentimientoInformadoReporte" class="JETIQUETA_2" >Consentimiento Informado</label>&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>  
                </div>
            </div>            
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgInformeMedicoReporte" class="JBOTON-IMAGEN" name="EP"  style="background-image:url(../Imagenes/Epicrisis1.png);width:64px;height:64px;background-size:64px 64px;" /> <%--ico_epicrisis.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkInformeMedicoReporte" name="EP" /><label for="chkInformeMedicoReporte" class="JETIQUETA_2">Epicrisis</label>
                    </div>  
                </div>
            </div>                 
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgDeclaratorioAlergiaReporte" class="JBOTON-IMAGEN" name="DA" style="background-image:url(../Imagenes/Alergia1.png);width:64px;height:64px;background-size:64px 64px;" /> <%--Pastilla_abierta.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkDeclaratorioAlergiaReporte" name="DA" /><label for="chkDeclaratorioAlergiaReporte" class="JETIQUETA_2">Declaratoria de Alergia</label>
                    </div>
                </div>
            </div>    
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgEvolucionClinicaReporte" class="JBOTON-IMAGEN" name="EC" style="background-image:url(../Imagenes/Evolucion1.png);width:64px;height:64px;background-size:64px 64px;" />  <%--ico_evolucion.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkEvolucionClinicaReporte" name="EC" /><label for="chkEvolucionClinicaReporte" class="JETIQUETA_2">Evolución Clínica</label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgRegistroMedicoReconsiliacionReporte" name="ME" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/Medicamentosa1.png);width:64px;height:64px;background-size:64px 64px;" /> <%--Edit.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkRegistroMedicoReconsiliacionReporte" name="ME" /><label for="chkRegistroMedicoReconsiliacionReporte" class="JETIQUETA_2">Rec. medicamentosa</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgInterconsultaReporte" class="JBOTON-IMAGEN" name="IN" style="background-image:url(../Imagenes/Interconsulta1.png);width:64px;height:64px;background-size:64px 64px;" />  <%--ico_interconsulta.jpg--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkInterconsultaReporte" name="IN" /><label for="chkInterconsultaReporte" class="JETIQUETA_2">Interconsulta</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">
                    <input type="button" id="imgIndicacionMedicaReporte" class="JBOTON-IMAGEN" name="IM" style="background-image:url(../Imagenes/Tratamiento1.png);width:64px;height:64px;background-size:64px 64px;"  />  <%--lista_pedidos.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkIndicacionMedicaReporte" name="IM" style="display:none;" /><label for="chkIndicacionMedicaReporte" class="JETIQUETA_2">Indicaciones Médicas</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgRecetaAlta" class="JBOTON-IMAGEN" name="RA" style="background-image:url(../Imagenes/RecetaAlta1.png);width:64px;height:64px;background-size:64px 64px;" />  <%--Receta.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkRecetaAlta" name="RA" /><label for="chkRecetaAlta" class="JETIQUETA_2">Receta Alta</label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgJuntaMedica" name="JM" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/JuntaMedica.png);width:64px;height:64px;background-size:64px 64px;" /> <%--Edit.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkReporteJuntaMedica" name="JM" /><label for="chkReporteJuntaMedica" class="JETIQUETA_2">Junta Médica</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgNotaIngreso" name="NI" class="JBOTON-IMAGEN" style="background-image:url(../Imagenes/NotaIngreso.png);width:64px;height:64px;background-size:64px 64px;" /> <%--Edit.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkReporteNotaIngreso" name="NI" /><label for="chkReporteNotaIngreso" class="JETIQUETA_2">Nota Ingreso</label>
                    </div>
                </div>
            </div>
        </div>
        <%-- 1.1 INI --%>
        <div class="JCELDA-3">
            <div class="JFILA">
                <div class="JCELDA-12" style="text-align:center;">                    
                    <input type="button" id="imgHistoriaClinica" class="JBOTON-IMAGEN" name="HC" style="background-image:url(../Imagenes/HistoriaClinica.png);width:64px;height:64px;background-size:64px 64px;" />  <%--Receta.png--%>
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-12">
                    <div class="JDIV-CONTROLES" style="text-align:center;">
                        <input type="checkbox" id="chkHistoriaClinica" name="HC" /><label for="chkHistoriaClinica" class="JETIQUETA_2">Historia Clinica</label>
                    </div>
                </div>
            </div>
        </div>
        <%-- 1.1 FIN --%>
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JFILA" style="text-align:center;">
                <div class="JDIV-CONTROLES">
                    <input type="checkbox" id="chkMarcarTodo" /><label for="chkMarcarTodo" class="JETIQUETA_2">Marcar Todo</label>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" runat="server" id="hfReporteExistente" />
    <input type="hidden" runat="server" id="hfProcedimientoSeleccionado" />
    <input type="hidden" runat="server" id="hfFechaRecetaSeleccionado" />
    </form>
</body>
</html>
