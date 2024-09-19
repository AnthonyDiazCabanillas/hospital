/*CODIGO JAVASCRIPT/JQUERY PARA EL FUNCIONAMIENTO DEL MENSAJE POPUP - CREADO POR JONATHAN BARRETO.*/
/*FUNCION PARA OCULTAR POPUP*/
function fn_oculta_mensaje() {
    $(".JFONDO-POPUP").animate({ opacity: 0 }, 0);
    $(".JFONDO-POPUP").css("display", "none");
    /*PARA POPUP GENERAL*/
    $(".JCONTENIDO-POPUP").animate({ top: "-" + $(window).height() + "px",
        opacity: 0
    }, 0, function () {//TMACASSI  }, 400, function () {
        $("body").find(".JFONDO-POPUP").remove();
        $("body").find(".JCONTENIDO-POPUP").remove();
    });
    /*PARA POPUP DE ERROR*/
    $(".JCONTENIDO-POPUP-ERROR").animate({ top: "-" + $(window).height() + "px",
        opacity: 0
    }, 0, function () {//TMACASSI  }, 600, function () {
        $("body").find(".JFONDO-POPUP").remove();
        $("body").find(".JCONTENIDO-POPUP-ERROR").remove();
    });

    /*PARA POPUP DE ADVERTENCIA*/
    $(".JCONTENIDO-POPUP-ADVERTENCIA").animate({ top: "-" + $(window).height() + "px",
        opacity: 0
    }, 0, function () { //TMACASSI }, 400, function () {
        $("body").find(".JFONDO-POPUP").remove();
        $("body").find(".JCONTENIDO-POPUP-ADVERTENCIA").remove();
    });
    /*PARA POPUP OK*/
    $(".JCONTENIDO-POPUP-OK").animate({ top: "-" + $(window).height() + "px",
        opacity: 0
    }, 600, function () {
        $("body").find(".JFONDO-POPUP").remove();
        $("body").find(".JCONTENIDO-POPUP-OK").remove();
    });
}

/*FUNCION QUE OCULTA LOS POPUP DE MANERA INSTANTANEA, CREADA PARA LOS CASOS EN QUE SE REQUIERA MOSTRAR MAS DE 1 POPUP SEGUIDO*/
function fn_oculta_mensaje_rapido() {
    $("body").find(".JFONDO-POPUP").remove();
    $("body").find(".JCONTENIDO-POPUP").remove();
    $("body").find(".JCONTENIDO-POPUP-ERROR").remove();
    $("body").find(".JCONTENIDO-POPUP-ADVERTENCIA").remove();
    $("body").find(".JCONTENIDO-POPUP-OK").remove();
}

/*
AUTOR: JONATHAN B.
ESTA FUNCION CREA UN MENSAJE POPUP QUE MUESTRA UN MENSAJE ESPECIFICADO
PARAMETRO 1(JTITULO_POPUP) -> TITULO DEL MENSAJE
PARAMETRO 2(JCUERPO_POPUP) -> CUERPO DEL MENSAJE
PARAMETRO 3(JTIPO_POPUP) -> TIPO DE MENSAJE (ERROR -> ROJO, ADVERTENCIA -> NARANJA, OK -> VERDE, "" -> AZUL POR DEFECTO)
PARAMETRO 4(JTEXTO_BOTONES) -> TEXTO QUE TENDRA LOS BOTONES DEL MENSAJE. EJEM. ACEPTAR, CANCELAR, SI, NO, ETC.
PARAMETRO 5(JFUNCION_BOTONES) -> FUNCION (JAVASCRIPT) QUE EJECUTARA LOS BOTONES, ESTOS DEBES SER ENVIADOS EN EL MISMO ORDEN QUE LOS TEXTO DE LOS BOTONES
**PARAMETRO 6(objetod) -> PARAMETRO OPCIONAL, SE ENVIA EL ID DEL FORMULARIO, ACTUALMENTE USADO SOLO SI EL MENSAJE SE LLAMA DESDE UNA VENTANA POPUP
*/
$.JMensajePOPUP = function (JTITULO_POPUP, JCUERPO_POPUP, JTIPO_POPUP, JTEXTO_BOTONES, JFUNCION_BOTONES, objetod) { //JBOTONES
    if (JTIPO_POPUP == "ERROR") {
        var sDIV_FONDO_POPUP = "<div class='JFONDO-POPUP'></div>";
        var sDIV_CONTENEDOR_POPUP = "<div class='JCONTENIDO-POPUP-ERROR'>" +
                                            "<header>" + JTITULO_POPUP +
                                                "</header>" +
                                                "<div> " +
                                                    JCUERPO_POPUP +
                                                "</div>" +
                                                "<footer>";

        var aTE_BOTO = new Array();
        if (JTEXTO_BOTONES != undefined) {
            aTE_BOTO = JTEXTO_BOTONES.split(";");
        }

        var aFU_BOTO = new Array();
        if (JFUNCION_BOTONES != undefined) {
            aFU_BOTO = JFUNCION_BOTONES.split(";");
        }

        for (var i = 0; i < aTE_BOTO.length; i++) { //  parseInt(JBOTONES == "" ? 0 : JBOTONES)
            sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP +
                    "<input type='button' value='" + aTE_BOTO[i] + "' onclick='" + aFU_BOTO[i] + "' />";
        }

        sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP + "</footer>" + "</div>";
        //alert($(objetod).find("form").index());
        if (objetod != undefined && objetod != null && objetod != "") { //esta valor debera recibirse si el mensaje popup sera abierto desde una ventana popup
            $("#" + objetod).append(sDIV_FONDO_POPUP);
        } else {
            $("body").append(sDIV_FONDO_POPUP);
        }
        $("body").append(sDIV_CONTENEDOR_POPUP);

        //$(".JCONTENIDO-POPUP").css("top", (($(window).height() - $(".JCONTENIDO-POPUP").outerHeight()) / 2).toString() + "px");
        $(".JCONTENIDO-POPUP-ERROR").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-ERROR").outerWidth()) / 2).toString() + "px");

        /*OCULTANDO POPUP*/
        $(".JCONTENIDO-POPUP-ERROR").css("top", "-" + $(window).height() + "px"); /*$(".JCONTENIDO-POPUP").outerHeight()*/
        $(".JCONTENIDO-POPUP-ERROR").css("opacity", "0");
        $(".JFONDO-POPUP").css("opacity", "0");

        /*MOSTRANDO EL POPUP*/
        $(".JFONDO-POPUP").animate({ opacity: 1 }, 0);
        $(".JCONTENIDO-POPUP-ERROR").animate({ top: (($(window).height() - $(".JCONTENIDO-POPUP-ERROR").outerHeight()) / 2).toString() + "px",
            opacity: 1
        }, 0); //tmacassi 400

        $(".JCONTENIDO-POPUP-ERROR").find(":button:last").focus(); /*15/06/2016*/

        $(window).resize(function () {
            if ($(".JFONDO-POPUP").css("display") != "none") {
                $(".JCONTENIDO-POPUP-ERROR").css("top", (($(window).height() - $(".JCONTENIDO-POPUP-ERROR").outerHeight()) / 2).toString() + "px");
                $(".JCONTENIDO-POPUP-ERROR").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-ERROR").outerWidth()) / 2).toString() + "px");
            }
        });
    } else if (JTIPO_POPUP == "ADVERTENCIA") {
        var sDIV_FONDO_POPUP = "<div class='JFONDO-POPUP'></div>";
        var sDIV_CONTENEDOR_POPUP = "<div class='JCONTENIDO-POPUP-ADVERTENCIA'>" +
                                            "<header>" + JTITULO_POPUP +
                                                "</header>" +
                                                "<div> " +
                                                    JCUERPO_POPUP +
                                                "</div>" +
                                                "<footer>";

        var aTE_BOTO = new Array();
        if (JTEXTO_BOTONES != undefined) {
            aTE_BOTO = JTEXTO_BOTONES.split(";");
        }

        var aFU_BOTO = new Array();
        if (JFUNCION_BOTONES != undefined) {
            aFU_BOTO = JFUNCION_BOTONES.split(";");
        }

        for (var i = 0; i < aTE_BOTO.length; i++) { //  parseInt(JBOTONES == "" ? 0 : JBOTONES)
            sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP +
                    "<input type='button' value='" + aTE_BOTO[i] + "' onclick='" + aFU_BOTO[i] + "' />";
        }

        sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP + "</footer>" + "</div>";
        //alert($(objetod).find("form").index());
        if (objetod != undefined && objetod != null && objetod != "") { //esta valor debera recibirse si el mensaje popup sera abierto desde una ventana popup
            $("#" + objetod).append(sDIV_FONDO_POPUP);
        } else {
            $("body").append(sDIV_FONDO_POPUP);
        }
        $("body").append(sDIV_CONTENEDOR_POPUP);

        //$(".JCONTENIDO-POPUP").css("top", (($(window).height() - $(".JCONTENIDO-POPUP").outerHeight()) / 2).toString() + "px");
        $(".JCONTENIDO-POPUP-ADVERTENCIA").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-ADVERTENCIA").outerWidth()) / 2).toString() + "px");

        /*OCULTANDO POPUP*/
        $(".JCONTENIDO-POPUP-ADVERTENCIA").css("top", "-" + $(window).height() + "px"); /*$(".JCONTENIDO-POPUP").outerHeight()*/
        $(".JCONTENIDO-POPUP-ADVERTENCIA").css("opacity", "0");
        $(".JFONDO-POPUP").css("opacity", "0");

        /*MOSTRANDO EL POPUP*/
        $(".JFONDO-POPUP").animate({ opacity: 1 }, 0);
//        $(".JCONTENIDO-POPUP-ADVERTENCIA").animate({ top: (($(window).height() - $(".JCONTENIDO-POPUP-ADVERTENCIA").outerHeight()) / 2).toString() + "px",
//            opacity: 1
        //        }, 400);

        //TMACASSI 07/09/2016

        $(".JCONTENIDO-POPUP-ADVERTENCIA").animate({ top: (($(window).height() - $(".JCONTENIDO-POPUP-ADVERTENCIA").outerHeight()) / 2).toString() + "px",
            opacity: 1
        }, 0);

        $(".JCONTENIDO-POPUP-ADVERTENCIA").find(":button:last").focus(); /*15/06/2016*/

        $(window).resize(function () {
            if ($(".JFONDO-POPUP").css("display") != "none") {
                $(".JCONTENIDO-POPUP-ADVERTENCIA").css("top", (($(window).height() - $(".JCONTENIDO-POPUP-ADVERTENCIA").outerHeight()) / 2).toString() + "px");
                $(".JCONTENIDO-POPUP-ADVERTENCIA").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-ADVERTENCIA").outerWidth()) / 2).toString() + "px");
            }
        });
    } else if (JTIPO_POPUP == "OK") {
        var sDIV_FONDO_POPUP = "<div class='JFONDO-POPUP'></div>";
        var sDIV_CONTENEDOR_POPUP = "<div class='JCONTENIDO-POPUP-OK'>" +
                                            "<header>" + JTITULO_POPUP +
                                                "</header>" +
                                                "<div> " +
                                                    JCUERPO_POPUP +
                                                "</div>" +
                                                "<footer>";

        var aTE_BOTO = new Array();
        if (JTEXTO_BOTONES != undefined) {
            aTE_BOTO = JTEXTO_BOTONES.split(";");
        }

        var aFU_BOTO = new Array();
        if (JFUNCION_BOTONES != undefined) {
            aFU_BOTO = JFUNCION_BOTONES.split(";");
        }

        for (var i = 0; i < aTE_BOTO.length; i++) { //  parseInt(JBOTONES == "" ? 0 : JBOTONES)
            sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP +
                    "<input type='button' value='" + aTE_BOTO[i] + "' onclick='" + aFU_BOTO[i] + "' />";
        }

        sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP + "</footer>" + "</div>";
        //alert($(objetod).find("form").index());
        if (objetod != undefined && objetod != null && objetod != "") { //esta valor debera recibirse si el mensaje popup sera abierto desde una ventana popup
            $("#" + objetod).append(sDIV_FONDO_POPUP);
        } else {
            $("body").append(sDIV_FONDO_POPUP);
        }
        $("body").append(sDIV_CONTENEDOR_POPUP);

        //$(".JCONTENIDO-POPUP").css("top", (($(window).height() - $(".JCONTENIDO-POPUP").outerHeight()) / 2).toString() + "px");
        $(".JCONTENIDO-POPUP-OK").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-OK").outerWidth()) / 2).toString() + "px");

        /*OCULTANDO POPUP*/
        $(".JCONTENIDO-POPUP-OK").css("top", "-" + $(window).height() + "px"); /*$(".JCONTENIDO-POPUP").outerHeight()*/
        $(".JCONTENIDO-POPUP-OK").css("opacity", "0");
        $(".JFONDO-POPUP").css("opacity", "0");

        /*MOSTRANDO EL POPUP*/
        $(".JFONDO-POPUP").animate({ opacity: 1 }, 0);
        $(".JCONTENIDO-POPUP-OK").animate({ top: (($(window).height() - $(".JCONTENIDO-POPUP-OK").outerHeight()) / 2).toString() + "px",
            opacity: 1
        }, 0); //tmacassi 400

        $(".JCONTENIDO-POPUP-OK").find(":button:last").focus(); /*15/06/2016*/

        $(window).resize(function () {
            if ($(".JFONDO-POPUP").css("display") != "none") {
                $(".JCONTENIDO-POPUP-OK").css("top", (($(window).height() - $(".JCONTENIDO-POPUP-OK").outerHeight()) / 2).toString() + "px");
                $(".JCONTENIDO-POPUP-OK").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-OK").outerWidth()) / 2).toString() + "px");
            }
        });
    } else {
        var sDIV_FONDO_POPUP = "<div class='JFONDO-POPUP'></div>";
        var sDIV_CONTENEDOR_POPUP = "<div class='JCONTENIDO-POPUP'>" +
                                            "<header>" + JTITULO_POPUP +
                                                "</header>" +
                                                "<div> " +
                                                    JCUERPO_POPUP +
                                                "</div>" +
                                                "<footer>";

        var aTE_BOTO = new Array();
        if (JTEXTO_BOTONES != undefined) {
            aTE_BOTO = JTEXTO_BOTONES.split(";");
        }

        var aFU_BOTO = new Array();
        if (JFUNCION_BOTONES != undefined) {
            aFU_BOTO = JFUNCION_BOTONES.split(";");
        }

        for (var i = 0; i < aTE_BOTO.length; i++) { //  parseInt(JBOTONES == "" ? 0 : JBOTONES)
            sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP +
                    "<input type='button' value='" + aTE_BOTO[i] + "' onclick='" + aFU_BOTO[i] + "' />";
        }

        sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP + "</footer>" + "</div>";
        //alert($(objetod).find("form").index());
        if (objetod != undefined && objetod != null && objetod != "") { //esta valor debera recibirse si el mensaje popup sera abierto desde una ventana popup
            $("#" + objetod).append(sDIV_FONDO_POPUP);
        } else {
            $("body").append(sDIV_FONDO_POPUP);
        }
        $("body").append(sDIV_CONTENEDOR_POPUP);

        //$(".JCONTENIDO-POPUP").css("top", (($(window).height() - $(".JCONTENIDO-POPUP").outerHeight()) / 2).toString() + "px");
        $(".JCONTENIDO-POPUP").css("left", (($(window).width() - $(".JCONTENIDO-POPUP").outerWidth()) / 2).toString() + "px");

        /*OCULTANDO POPUP*/
        $(".JCONTENIDO-POPUP").css("top", "-" + $(window).height() + "px"); /*$(".JCONTENIDO-POPUP").outerHeight()*/
        $(".JCONTENIDO-POPUP").css("opacity", "0");
        $(".JFONDO-POPUP").css("opacity", "0");

        /*MOSTRANDO EL POPUP*/
        $(".JFONDO-POPUP").animate({ opacity: 1 }, 250);
        $(".JCONTENIDO-POPUP").animate({ top: (($(window).height() - $(".JCONTENIDO-POPUP").outerHeight()) / 2).toString() + "px",
            opacity: 1
        }, 0); //tmacassi 400

        $(".JCONTENIDO-POPUP").find(":button:last").focus(); /*15/06/2016*/

        $(window).resize(function () {
            if ($(".JFONDO-POPUP").css("display") != "none") {
                $(".JCONTENIDO-POPUP").css("top", (($(window).height() - $(".JCONTENIDO-POPUP").outerHeight()) / 2).toString() + "px");
                $(".JCONTENIDO-POPUP").css("left", (($(window).width() - $(".JCONTENIDO-POPUP").outerWidth()) / 2).toString() + "px");
            }
        });
    }
}