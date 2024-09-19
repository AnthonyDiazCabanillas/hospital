<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImprimirReporte.aspx.vb" Inherits="WebHCE.ImprimirReporte" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        var ReporteImprimir = "";
        var Reportes = "";

        $(document).ready(function () {
            $.ajax({
                url: "PopUp/ImprimirReporte.aspx/VerificarReportesData",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.split(";")[0] == "ERROR") {

                } else {
                    if (oOB_JSON.d.toString().trim() != "") {
                        for (var i = 0; i < oOB_JSON.d.toString().split(";").length; i++) {
                            if (oOB_JSON.d.toString().split(";")[i].trim() != "") {
                                $("#" + oOB_JSON.d.toString().split(";")[i]).attr("disabled", "disabled");
                                $("#" + oOB_JSON.d.toString().split(";")[i]).next().css("color", "#bababa");
                            }
                        }
                    }
                }
            });
        });

        function fn_Imprimir() {
            ReporteImprimir = "";
            if ($("#chkConsentimientoInformadoReporte").prop("checked")) {
                ReporteImprimir += "CI;";
            }
            if ($("#chkDeclaratorioAlergiaReporte").prop("checked")) {
                ReporteImprimir += "DA;"; //declaratori alergia
            }
            if ($("#chkRegistroMedicoReconsiliacionReporte").prop("checked")) {
                ReporteImprimir += "ME;"; //Medicamentosa
            }
            if ($("#chkIndicacionMedicaReporte").prop("checked")) {
                ReporteImprimir += "PE1;";
            }
            if ($("#chkEvolucionClinicaReporte").prop("checked")) {
                ReporteImprimir += "EC1;";
            }
            if ($("#chkInterconsultaReporte").prop("checked")) {
                ReporteImprimir += "IN1;";
            }
            //TMACASSI 27/10/2016 EPICRISIS
            if ($("#chkInformeMedicoReporte").prop("checked")) {
                ReporteImprimir += "IM1;";
            } 
            if ($("#chkRecetaAlta").prop("checked")) {
                ReporteImprimir += "RA1"; 
            }
            //window.location.href = "Reporte.aspx?OP=" + ReporteImprimir;
            /*if (ReporteImprimir.trim() == "") {
                $.JMensajePOPUP("Aviso", oOB_JSON.d.split(";")[1], "1", "Cerrar", "fn_oculta_mensaje()", "frmImprimirReporte");
                return;
            }*/
            for (var i = 0; i < ReporteImprimir.split(";").length; i++) {
                if (ReporteImprimir.split(";")[i] != "") {
                    if (ReporteImprimir.split(";")[i] != "CI") {
                        window.open("Reporte.aspx?OP=" + ReporteImprimir.split(";")[i]);
                    } else {
                        $.ajax({
                            url: "PopUp/ImprimirReporte.aspx/VerificarCantidadReportes",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d.split(";")[0] == "ERROR") {

                            } else {
                                if (oOB_JSON.d.toString().trim() != "") { //SOLO SI LA CONSULTA DEVUELVE REGISTROS
                                    var aArchivos = oOB_JSON.d.split("*")[0];
                                    var aCodProcedimiento = oOB_JSON.d.split("*")[1];
                                    for (var i = 0; i < aArchivos.split(";").length; i++) {
                                        if (aArchivos.split(";")[i].toString().trim() != "") {
                                            window.open("Reporte.aspx?OP=" + ReporteImprimir + "&Reporte=" + aArchivos.split(";")[i].toString().trim() + "&CodProc=" + aCodProcedimiento.split(";")[i].toString().trim());
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }

            
        }
    </script>

</head>
<body>
<form runat="server" id="frmImprimirReporte">
    <div class="JFILA">
        <div class="JCELDA-5">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA">Seleccione Fecha a Imprimir</span>
            </div>
        </div>
        <div class="JCELDA-5"> <%--style="border:1px solid Black;"--%>
            <div class="JDIV-CONTROLES">
                <input type="date" id="txtFechaImprimir" />
            </div>
        </div>        
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkConsentimientoInformadoReporte" /><label for="chkConsentimientoInformadoReporte" class="JETIQUETA_2">Consentimiento Informado</label>
            </div>
        </div>             
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkInformeMedicoReporte" /><label for="chkInformeMedicoReporte" class="JETIQUETA_2">Epicrisis</label>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkDeclaratorioAlergiaReporte" /><label for="chkDeclaratorioAlergiaReporte" class="JETIQUETA_2">Declaratoria de Alergia</label>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkEvolucionClinicaReporte" /><label for="chkEvolucionClinicaReporte" class="JETIQUETA_2">Evolución Clínica</label>
            </div>
        </div>
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkRegistroMedicoReconsiliacionReporte" /><label for="chkRegistroMedicoReconsiliacionReporte" class="JETIQUETA_2">Registro medico de reconciliación medicamentosa</label>
            </div>
        </div>             
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkInterconsultaReporte" /><label for="chkInterconsultaReporte" class="JETIQUETA_2">Interconsulta</label>
            </div>
        </div>             
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkIndicacionMedicaReporte" /><label for="chkIndicacionMedicaReporte" class="JETIQUETA_2">Indicaciones Médicas</label>
            </div>
        </div>             
    </div>
    <div class="JFILA">
        <div class="JCELDA-7">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkRecetaAlta" /><label for="chkRecetaAlta" class="JETIQUETA_2">Receta Alta</label>
            </div>
        </div>             
    </div>
</form>
</body>
</html>
