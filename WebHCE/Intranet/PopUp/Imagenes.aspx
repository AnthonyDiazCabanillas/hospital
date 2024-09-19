<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Imagenes.aspx.vb" Inherits="WebHCE.Imagenes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            //fn_CrearTreeViewImagenesPopUp();
            fn_CrearTreeViewImagenes2("1", "", "0");
            fn_LOAD_OCUL();
        });

        function fn_CrearTreeViewImagenesPopUp() {
            $.ajax({
                url: "PopUp/Imagenes.aspx/TreeViewImagenes",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    CodigoAtencion: $("#" + "<%=hfCodAtencionImgPopUp.ClientID %>").val().trim()
                }),
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")(1), "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    } else {
                        $("#divImagenPopUp").html("");
                        $("#divImagenPopUp").append(oOB_JSON.d);
                        fn_CreaEventosTreeViewAI("divImagenPopUp", "hfImagenSeleccionadoPopUp", "CENTRAR");
                        fn_CrearEventoTreeViewImagenes();
                    }
                }
            });
        }


        function fn_CrearEventoTreeViewImagenes() {
            $("#divImagenPopUp a").click(function () {
                $("#divImagenPopUp").find("a").css("color", "#134B8D");
                $(this).css("color", "#8DC73F");
                $("#hfImagenSeleccionadoPopUp").val($(this).next().val().trim());
            });
        }


        function fn_VerInformeImgPopUp() {
            if ($("#hfImagenSeleccionadoPopUp").val().trim() != "") {
                $.ajax({
                    url: "PopUp/Imagenes.aspx/VerInformeImagen",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        PresotorSps: $("#hfImagenSeleccionadoPopUp").val().trim()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {
                    }
                }).done(function (oOB_JSON) {
                    if (oOB_JSON.d.toString().split(";").length > 1) {
                        $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()", "frmImagenesPopUp");
                    } else {
                        var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                        if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                            //ventana popup bloqueada
                        } else {
                            //ventana_popup.focus();
                        }
                    }
                });
            } else {
                //no ha seleccionado ninguna imagen para ver el informe   
                $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()", "frmImagenesPopUp");
            }
        }




        /*INICIO - JB - 04/08/2020 - NUEVO CODIGO, REEMPLAZARA AL ACTUAL*/
        function fn_CrearTreeViewImagenes2(OrdenEjecutar, FechaMostrar, IdeRecetaCabMostrar, objeto) {
            $.ajax({
                url: "PopUp/Imagenes.aspx/TreeViewImagenes2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    orden: OrdenEjecutar,
                    fec_receta: FechaMostrar,
                    ide_recetacab: IdeRecetaCabMostrar,
                    CodigoAtencion: $("#" + "<%=hfCodAtencionImgPopUp.ClientID %>").val().trim()
                }),
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    if (OrdenEjecutar == "1") {
                        $("#divImagenPopUp").html("");
                        $("#divImagenPopUp").append(oOB_JSON.d);
                    }
                    if (OrdenEjecutar == "2") {
                        objeto.parent().find(".JTREE3-HORA").html("");
                        objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                    }
                    if (OrdenEjecutar == "3") {
                        objeto.next().html("");
                        objeto.next().append(oOB_JSON.d);
                    }

                    fn_CrearEventoTreeImagenes2();
                }
            });
        }


        function fn_CrearEventoTreeImagenes2() {
            $("#divImagenPopUp").find(".JFILA-FECHA").unbind("click");
            $("#divImagenPopUp").find(".JFILA-FECHA").click(function () {
                var oObjeto = $(this);


                $("#divImagenPopUp").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                $("#divImagenPopUp").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                $("#divImagenPopUp").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");
                oObjeto.addClass("JTREE3-SELECCIONADO");
                oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                var Fecha = oObjeto.find("> input").val();
                if (CadenaClase.includes("JTREE3-PLUS")) {
                    fn_CrearTreeViewImagenes2("2", Fecha, "0", oObjeto);
                    oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                } else {
                    oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                }

            });
            $("#divImagenPopUp").find(".JFILA-HORA").unbind("click");
            $("#divImagenPopUp").find(".JFILA-HORA").click(function () {
                var oObjeto = $(this);


                $("#divImagenPopUp").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                $("#divImagenPopUp").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                $("#divImagenPopUp").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                oObjeto.addClass("JTREE3-SELECCIONADO");
                oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                var IdRecetaCab = oObjeto.find("> input").val();
                if (CadenaClase.includes("JTREE3-PLUS")) {
                    fn_CrearTreeViewImagenes2("3", "", IdRecetaCab, oObjeto);
                    oObjeto.next().css("display", "block");
                } else {
                    oObjeto.next().css("display", "none");
                }


            });
            $("#divImagenPopUp").find(".JTREE3-DETALLE").unbind("click");
            $("#divImagenPopUp").find(".JTREE3-DETALLE").click(function () {
                var oObjeto = $(this);


                $("#divImagenPopUp").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                $("#divImagenPopUp").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                $("#divImagenPopUp").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                oObjeto.addClass("JTREE3-SELECCIONADO");

            });
        }

        function fn_EventoVerInforme() {
            var FecReceta = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO > .FecRegistro").eq(0).val(); //si obtiene valor marco el nodo de fecha
            if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                var IdeImagenDet = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO > .IdeImagenDet").eq(0).val(); //si tiene valor marco detalle                        
                if (IdeImagenDet == null || IdeImagenDet == undefined || IdeImagenDet == null) {
                    $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                } else {
                    var PresotorSps = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO").find("> input").eq(0).val(); //
                    var FlgVerificar = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO").find("> .FlgVerificarIma").eq(0).val();

                    if (PresotorSps.trim() != "" && PresotorSps.trim() != "_") {
                        $.ajax({
                            url: "PopUp/Imagenes.aspx/VerInformeImagen",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                PresotorSps: PresotorSps
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d.toString().split(";").length > 1) {
                                $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                            } else {
                                //fn_GuardaLog("IMAGEN", "Se visualizo informe " + PresotorSps);
                                var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                                if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                                    //ventana popup bloqueada
                                } else {
                                    //ventana_popup.focus();
                                }
                            }
                        });    
                    }
                }
            } else {
                $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
            }
        }

        function fn_EventoVerImagen() {
            var FecReceta = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO > .FecRegistro").eq(0).val(); //si obtiene valor marco el nodo de fecha
            if (FecReceta == undefined || FecReceta == null || FecReceta == "") {
                var IdeImagenDet = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO > .IdeImagenDet").eq(0).val(); //si tiene valor marco detalle                        
                if (IdeImagenDet == null || IdeImagenDet == undefined || IdeImagenDet == null) {
                    $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
                } else {
                    var PresotorSps = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO").find("> input").eq(0).val(); //
                    var FlgVerificar = $("#divImagenPopUp").find(".JTREE3-SELECCIONADO").find("> .FlgVerificarIma").eq(0).val();

                    if (PresotorSps.trim() != "" && PresotorSps.trim() != "_") {
                        $.ajax({
                            url: "InformacionPaciente.aspx/VerImagen",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                PresotorSps: PresotorSps
                            }),
                            dataType: "json",
                            error: function (dato1, datos2, dato3) {
                            }
                        }).done(function (oOB_JSON) {
                            if (oOB_JSON.d.toString().split(";").length > 1) {
                                $.JMensajePOPUP("Aviso", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()");
                            } else {
                                //fn_GuardaLog("IMAGEN", "Se visualizo imagen de " + PresotorSps);
                                //abrira en internet explorer
                                var ventana_popup_imagen = window.open(oOB_JSON.d, "_blank");
                                if (ventana_popup_imagen == null || typeof (ventana_popup_imagen) == undefined) {
                                    //ventana popup bloqueada
                                } else {
                                    ventana_popup_imagen.focus();
                                }
                            }
                        });
                    }
                }
            } else {
                $.JMensajePOPUP("Aviso", "Debe seleccionar una imagen.", "", "Cerrar", "fn_oculta_mensaje()");
            }
        }

    </script>
</head>
<body>
    <form runat="server" id="frmImagenesPopUp">
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_3" id="spEstadoExamen" runat="server">Estado Examenes Imagenes</span>
            </div>
        </div>        
    </div>
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <input type="hidden" id="hfCodAtencionImgPopUp" runat="server" />
                <input type="hidden" id="hfImagenSeleccionadoPopUp" />
                <div style="border:1px solid Black;width:100%;height:100%; min-height:100px; min-width:200px; max-height:300px; overflow:auto;background-color:White;" id="divImagenPopUp">
                    
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
