<!-- ' **********************************************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2023. Todos los derechos reservados.
'    Version     Fecha           Autor       Requerimiento
'    1.1         19/06/2024      FGUEVARA    REQ-2024-011009  RESULTADOS ROE - HC
'*********************************************************************************************************************** -->

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Laboratorio.aspx.vb" Inherits="WebHCE.Laboratorio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            //LLAMANDO A LA FUNCION QUE CARGARA EL TREEVIEW DE LABORATORIO
            //fn_CrearTreeViewLaboratorioPopUp();
            fn_CrearTreeViewAnalisis2("1", "", "0");
            fn_LOAD_OCUL();
        });

        function fn_CrearTreeViewLaboratorioPopUp() {
            $.ajax({
                url: "PopUp/Laboratorio.aspx/TreeViewLaboratorio",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    CodigoAtencion: $("#" + "<%=hfCodAtencionLabPopUp.ClientID %>").val().trim()
                }),
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")(1), "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmLaboratorioPopUp");
                    } else {
                        $("#divLaboratorioPopUp").html("");
                        $("#divLaboratorioPopUp").append(oOB_JSON.d);
                        fn_CreaEventosTreeViewAI("divLaboratorioPopUp", "hfLaboratorioSeleccionadoPopUp", "CENTRAR");
                        fn_CrearEventoTreeViewAnalisisPopUp();
                    }
                }
            });
        }

        function fn_CrearEventoTreeViewAnalisisPopUp() {
            $("#divLaboratorioPopUp a").click(function () {
                $("#divLaboratorioPopUp").find("a").css("color", "#134B8D");
                $(this).css("color", "#8DC73F");
                $("#hfLaboratorioSeleccionadoPopUp").val($(this).next().val().trim());
            });
        }

        function fn_VerInformeLabPopUp() {
            if ($("#hfLaboratorioSeleccionadoPopUp").val().trim() != "") {
                $.ajax({
                    url: "PopUp/Laboratorio.aspx/VerInformeAnalisis",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        IdRecetaCab: $("#hfLaboratorioSeleccionadoPopUp").val().trim()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL();
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                        $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()", "frmLaboratorioPopUp");
                    } else {
                        var ventana_popup = window.open(oOB_JSON.d, "_blank");
                        if (ventana_popup == null || typeof (ventana_popup) == undefined) {
                            //ventana popup bloqueada
                        } else {
                            //ventana_popup.focus();
                        }
                    }
                });
            } else {
                $.JMensajePOPUP("Aviso", "Debe seleccionar un analisis.", "", "Cerrar", "fn_oculta_mensaje()", "frmLaboratorioPopUp");
            }
        }














        /*JB - 04/08/2020 - NUEVO CODIGO QUE REEMPLAZARA EL ANTERIOR*/
        function fn_CrearTreeViewAnalisis2(OrdenEjecutar, FechaMostrar, IdeRecetaCabMostrar, objeto) {
            $.ajax({
                url: "PopUp/Laboratorio.aspx/TreeViewAnalisis2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    orden: OrdenEjecutar,
                    fec_receta: FechaMostrar,
                    ide_recetacab: IdeRecetaCabMostrar,
                    CodigoAtencion: $("#" + "<%=hfCodAtencionLabPopUp.ClientID %>").val().trim()
                }),
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    if (OrdenEjecutar == "1") {
                        $("#divLaboratorioPopUp").html("");
                        $("#divLaboratorioPopUp").append(oOB_JSON.d);
                    }
                    if (OrdenEjecutar == "2") {
                        objeto.parent().find(".JTREE3-HORA").html("");
                        objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                    }
                    if (OrdenEjecutar == "3") {
                        objeto.next().html("");
                        objeto.next().append(oOB_JSON.d);
                    }

                    fn_CrearEventoTreeAnalisis2();
                }
            });
        }


        function fn_CrearEventoTreeAnalisis2() {
            $("#divLaboratorioPopUp").find(".JFILA-FECHA").unbind("click");
            $("#divLaboratorioPopUp").find(".JFILA-FECHA").click(function () {
                var oObjeto = $(this);


                $("#divLaboratorioPopUp").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                $("#divLaboratorioPopUp").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                $("#divLaboratorioPopUp").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                oObjeto.addClass("JTREE3-SELECCIONADO");
                oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                var Fecha = oObjeto.find("> input").val();
                if (CadenaClase.includes("JTREE3-PLUS")) {
                    fn_CrearTreeViewAnalisis2("2", Fecha, "0", oObjeto);
                    oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                } else {
                    oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                }


            });
            $("#divLaboratorioPopUp").find(".JFILA-HORA").unbind("click");
            $("#divLaboratorioPopUp").find(".JFILA-HORA").click(function () {
                var oObjeto = $(this);


                $("#divLaboratorioPopUp").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                $("#divLaboratorioPopUp").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                $("#divLaboratorioPopUp").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                oObjeto.addClass("JTREE3-SELECCIONADO");
                oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                var IdRecetaCab = oObjeto.find("> input").val();
                if (CadenaClase.includes("JTREE3-PLUS")) {
                    fn_CrearTreeViewAnalisis2("3", "", IdRecetaCab, oObjeto);
                    oObjeto.next().css("display", "block");
                } else {
                    oObjeto.next().css("display", "none");
                }

            });
            $("#divLaboratorioPopUp").find(".JTREE3-DETALLE").unbind("click");
            $("#divLaboratorioPopUp").find(".JTREE3-DETALLE").click(function () {
                var oObjeto = $(this);


                $("#divLaboratorioPopUp").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                $("#divLaboratorioPopUp").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                $("#divLaboratorioPopUp").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                oObjeto.addClass("JTREE3-SELECCIONADO");

            });
        }



        function fn_VerInformeLaboratorio() {
            var IdRecetaCab = $("#divLaboratorioPopUp").find(".JTREE3-SELECCIONADO > input").eq(0).val();
            if (isNaN(IdRecetaCab)) {
                return;
            } else {
            }


            if (IdRecetaCab != undefined && IdRecetaCab != null && IdRecetaCab != "") {

                $.ajax({
                    url: "PopUp/Laboratorio.aspx/VerInformeAnalisis2",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        IdRecetaCab: IdRecetaCab
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL();
                    //fn_GuardaLog("LABORATORIO", "Se visualizo informe " + IdRecetaCab);
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                        $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                        window.open("VisorReporte.aspx?OP=ANALISISLABORATORIO&Valor=" + IdRecetaCab.toString()); //1.1
                        // INI 1.1
                        //var ventana_popup = window.open(oOB_JSON.d, "_blank");
                        //if (ventana_popup == null || typeof (ventana_popup) == undefined) {
                        //    //ventana popup bloqueada
                        //} else {
                        //    //ventana_popup.focus();
                        //}
                        // FIN 1.1
                    }
                });


            } else {
                $.JMensajePOPUP("Aviso", "Debe seleccionar un análisis.", "", "Cerrar", "fn_oculta_mensaje()");
            }
        }


    </script>
</head>
<body>
    <form runat="server" id="frmLaboratorioPopUp">
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_3" id="spEstadoExamen" runat="server">Estado Examenes de Laboratorio</span>
            </div>
        </div>        
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <input type="hidden" id="hfCodAtencionLabPopUp" runat="server" />
                <input type="hidden" id="hfLaboratorioSeleccionadoPopUp" />
                <div style="border:1px solid Black;width:100%;height:100%; min-height:100px; min-width:200px; max-height:300px; overflow:auto;background-color:White;" id="divLaboratorioPopUp">
                    
                </div>
            </div>
        </div>
    </div> 
    </form>   
</body>
</html>
