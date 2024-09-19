<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteHM.aspx.vb" Inherits="WebHCE.ReporteHM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".JFECHA").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#txtFechaInicioReporteHM").blur(function () {
                if ($(this).val().trim() != "") {
                    var fecha2;
                    var fecha1;
                    fecha1 = Date.parse(($("#txtFechaInicioReporteHM").val().split("/")[2] + "-" + $("#txtFechaInicioReporteHM").val().split("/")[1] + "-" + $("#txtFechaInicioReporteHM").val().split("/")[0]));
                    fecha2 = Date.parse(($("#txtFechaFinReporteHM").val().split("/")[2] + "-" + $("#txtFechaFinReporteHM").val().split("/")[1] + "-" + $("#txtFechaFinReporteHM").val().split("/")[0]));

                    if (fecha1 > fecha2) {
                        $.JMensajePOPUP("Aviso", "La fecha de inicio debe ser menor a la fecha fin", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        $("#txtFechaInicioReporteHM").val("");
                    }
                }
            });
            $("#txtFechaFinReporteHM").blur(function () {
                if ($(this).val().trim() != "") {
                    var fecha2;
                    var fecha1;
                    fecha1 = Date.parse(($("#txtFechaInicioReporteHM").val().split("/")[2] + "-" + $("#txtFechaInicioReporteHM").val().split("/")[1] + "-" + $("#txtFechaInicioReporteHM").val().split("/")[0]));
                    fecha2 = Date.parse(($("#txtFechaFinReporteHM").val().split("/")[2] + "-" + $("#txtFechaFinReporteHM").val().split("/")[1] + "-" + $("#txtFechaFinReporteHM").val().split("/")[0]));

                    if (fecha1 > fecha2) {
                        $.JMensajePOPUP("Aviso", "La fecha de inicio debe ser menor a la fecha fin", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                        $("#txtFechaFinReporteHM").val("");
                    }
                }
            });

            $("#rbControlEvolucionClinicaHM, #rbControlProcedimientoHM, #rbControlInterconsultaHM, #rbGestionClinicaHM, #rbLibroHospitalizacionHM").click(function () {
                if ($(this).prop("checked")) {
                    $("#txtFechaInicioReporteHM").removeAttr("disabled");
                    $("#txtFechaFinReporteHM").removeAttr("disabled");
                }
            });
        });

        function fn_ImprimirReporteHM() {
            //window.open("VisorReporte.aspx?OP=" + DatoEnviar + "&Valor=" + oOB_JSON.d.split(";")[x].toString());
            var OpcionReporte = "";            

            if ($("#rbControlEvolucionClinicaHM").prop("checked")) {
                OpcionReporte = "EC";
            }
            if ($("#rbControlProcedimientoHM").prop("checked")) {
                OpcionReporte = "PR";
            }
            if ($("#rbControlInterconsultaHM").prop("checked")) {
                OpcionReporte = "IN";
            }
            if ($("#rbGestionClinicaHM").prop("checked")) {
                OpcionReporte = "GC";
            }
            if ($("#rbLibroHospitalizacionHM").prop("checked")) {
                OpcionReporte = "LH";
            }

            if ($("#txtFechaInicioReporteHM").val() == "") {
                $.JMensajePOPUP("Aviso", "Debe ingresar campo Fecha Inicio", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                return;
            }
            if ($("#txtFechaFinReporteHM").val() == "") {
                $.JMensajePOPUP("Aviso", "Debe ingresar campo Fecha Fin", "", "Cerrar", "fn_oculta_mensaje()", "frmReceta");
                return;
            }

            window.open("VisorReporteHM2.aspx?OP=" + OpcionReporte + "&FI=" + $("#txtFechaInicioReporteHM").val() + "&FF=" + $("#txtFechaFinReporteHM").val());
        }

        function fn_CierraReporteHM() {
        //JB - 30/11/2021 - COMENTADO, SE USARA UNA VISTA EN C#
//            $.ajax({
//                url: "VisorReporteHM2.aspxx/CerrarReporteHM",
//                type: "POST",
//                contentType: "application/json; charset=utf-8",
//                dataType: "json"                
//            }).done(function (oOB_JSON) {
//                fn_oculta_popup();
            //            });

            fn_oculta_popup();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width:90%;height:90%;border:1px solid #8DC73F">
            <div style="width:100%;height:100%;border-bottom:1px solid #8DC73F">
                <div class="JFILA">
                    <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;">
                        <label class="JETIQUETA">Sel.</label>
                    </div>
                    <div class="JCELDA-10">
                        <label class="JETIQUETA">Reporte</label>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;border-top: 1px solid #8DC73F;">
                        <input type="radio" id="rbControlEvolucionClinicaHM" name="ReporteHM" />
                    </div>
                    <div class="JCELDA-11" style="border-top: 1px solid #8DC73F;">
                        <label for="rbControlEvolucionClinicaHM" class="JETIQUETA">Control de Evolucion Clínica</label>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;">
                        <input type="radio" id="rbControlProcedimientoHM" name="ReporteHM" />
                    </div>
                    <div class="JCELDA-10">
                        <label for="rbControlProcedimientoHM" class="JETIQUETA">Control de Procedimiento</label>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;">
                        <input type="radio" id="rbControlInterconsultaHM" name="ReporteHM" />
                    </div>
                    <div class="JCELDA-10">
                        <label for="rbControlInterconsultaHM" class="JETIQUETA">Control de Interconsulta</label>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;">
                        <input type="radio" id="rbGestionClinicaHM" name="ReporteHM" />
                    </div>
                    <div class="JCELDA-10">
                        <label for="rbGestionClinicaHM" class="JETIQUETA">Gestión Clínica</label>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-1" style="border-right: 1px solid #8DC73F;">
                        <input type="radio" id="rbLibroHospitalizacionHM" name="ReporteHM" />
                    </div>
                    <div class="JCELDA-10">
                        <label for="rbLibroHospitalizacionHM" class="JETIQUETA">Libro Hospitalizacion</label>
                    </div>
                </div>
            </div>
            
            
            
        </div>
        <br />
        <div style="width:90%;height:90%;border:1px solid #8DC73F">
            <div class="JFILA">
                <div class="JCELDA-3">
                    <span class="JETIQUETA">Selección</span>
                </div>                
            </div>
            <div class="JFILA">
                <div class="JCELDA-3">
                    <span class="JETIQUETA">Fecha Inicio</span>
                </div>
                <div class="JCELDA-8">
                    <input type="text" class="JFECHA" id="txtFechaInicioReporteHM" disabled="disabled" />
                </div>
            </div>
            <div class="JFILA">
                <div class="JCELDA-3">
                    <span class="JETIQUETA">Fecha Fin</span>
                </div>
                <div class="JCELDA-8">
                    <input type="text" class="JFECHA" id="txtFechaFinReporteHM" disabled="disabled" />        
                </div>
            </div>
            
            
        </div>
    </div>
    </form>
</body>
</html>
