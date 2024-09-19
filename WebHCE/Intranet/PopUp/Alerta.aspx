<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Alerta.aspx.vb" Inherits="WebHCE.Alerta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajaxSetup({ cache: false });
            fn_RenderizarTabs();

            var IdTreeview1 = $("#" + "<%=divAlertaTreeview.ClientID %>").attr("id");
            fn_CreaEventosTreeViewAI(IdTreeview1);


            $(".ALERTA").click(function () {
                /*$(".ALERTA").css("font-size", "1em");
                $(".ALERTA").css("font-weight", "normal");
                $(this).css("font-weight", "bold");
                $(this).css("font-size", "1.2em");*/
                //8DC73F

                $(".ALERTA").css("color", "#134B8D");
                $(this).css("color", "#8DC73F");

                $("#hfOpcionAlerta").val($(this).parent().parent().prev().find("span").html().trim());
                $("#hfAlerta").val($(this).next().val());
            });
        });

        function fn_VerificarAlerta() { 
            var sLabP = "";
            var sImgP = "";
            if ($("#chkAlertaLaboratorioPendiente").prop("checked")) {
                sLabP = "L";
            }

            if ($("#chkAlertaImagenPendiente").prop("checked")) {
                sImgP = "I";
            }

            $.ajax({
                url: "PopUp/Alerta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "") {
                    //fn_oculta_popup("fn_InvocaLaboratorioCompletado");
                    $.ajax({
                        url: "PopUp/Alerta.aspx/LaboratorioImagenCompletado", type: "POST", data: JSON.stringify({
                            LabCompletado: sLabP,
                            ImgCompletado: sImgP
                        }), contentType: "application/json; charset=utf-8", dataType: "json"
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "OK") {
                            //fn_oculta_popup("fn_InvocaLaboratorioCompletado");
                            $.JMensajePOPUP("Aviso", "Examenes verificados.", "ADVERTENCIA", "Cerrar", "fn_OcultaMensajeAlerta()");
                        } else {
                            $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_popup()");
                        }
                    });

                } else {
                    aValores = oOB_JSON.d.toString().split(";");
                    $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                }
            });

            /*
             data: JSON.stringify({
                TipoAtencion: ""
            }),
             */

            /*INICIO - JB - 05/08/2019 - COMENTADO
             * if ($("#hfOpcionAlerta").val().trim() != "" && $("#hfAlerta").val().trim() != "") {                
                if ($("#hfOpcionAlerta").val().trim() == "RECETA SUSPENDIDA") { //SI ES RECETA SUSPENDIDA
                    $.ajax({ url: "PopUp/Alerta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            fn_oculta_popup("fn_InvocaAlertaSuspension");//ocultando este popup y ejecutando funcion que mostrara el sgte popup
                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                        }
                    });
                }

                if ($("#hfOpcionAlerta").val().trim() == "PETITORIO EXAMENES") { //SI ES
                    $.ajax({ url: "PopUp/Alerta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            fn_oculta_popup("fn_InvocaExamenCompletado"); //ocultando este popup y ejecutando funcion que mostrara el sgte popup
                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                        }
                    });
                }

                if ($("#hfOpcionAlerta").val().trim() == "PETITORIO LABORATORIO") { //SI ES 
                    $.ajax({ url: "PopUp/Alerta.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "") {
                            fn_oculta_popup("fn_InvocaLaboratorioCompletado");
                        } else {
                            aValores = oOB_JSON.d.toString().split(";");
                            $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                        }
                    });
                }
            }*/ 
        }

        function fn_OcultaMensajeAlerta() {
            fn_oculta_mensaje_rapido();
            fn_oculta_popup();
            fn_AlertaImgLagPendiente();
        }
    </script>
</head>
<body>
    <form id="frmAlerta" runat="server">
        <div class="JFILA"> 
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="DatosUsuarioAlerta">
                                
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div style="border:1px solid #4BACFF;width:100%;max-height:300px;overflow-y:auto;height:300px;" class="JTREE" id="divAlertaTreeview" runat="server">
                            
                </div>
            </div>
        </div>
        
    </form>
</body>
</html>
