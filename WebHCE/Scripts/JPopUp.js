/*CODIGO JAVASCRIPT/JQUERY PARA EL FUNCIONAMIENTO DEL POPUP - CREADO POR JONATHAN BARRETO.*/
/*FUNCIONAMIENTO MUY SIMILAR AL MENSAJE POPUP*/

function fn_oculta_popup(funcion) {
    $(".JCONTENIDO-POPUP-1").stop().animate({ top: "-" + $(window).height() + "px",
        opacity: 0
    }, 500, function () {
        $(".JFONDO-POPUP-1").stop().animate({ opacity: 0 }, 50);
        $(".JFONDO-POPUP-1").css("display", "none");

        $("body").find(".JFONDO-POPUP-1").remove();
        $("body").find(".JCONTENIDO-POPUP-1").remove();
        if (funcion != undefined) { //SI SE RECIBE ESTE PARAMETRO(ES EL NOMBRE DE UNA FUNCION EN JAVASCRIPT) LO EJECUTARA NI BIEN OCULTE EL POPUP.
            eval("" + funcion + "()");
        }        
    });
}

function fn_oculta_popup_rapido() {
    $("body").find(".JFONDO-POPUP-1").remove();
    $("body").find(".JCONTENIDO-POPUP-1").remove();
}

/*
Jonathan B. - Descripcion de los parametros de la funcion

JTITULO_POPUP -> titulo del popup
JCUERPO_POPUP -> contenido del popup (una pagina aspx)
JBOTONES -> cantidad de botones que tendra el popup
JTEXTO_BOTONES -> descripcion que tendran los botones del popup (si hay mas de uno deben ir separados por ';'  ejem:  boton1;boton2;...)
JFUNCION_BOTONES ->  funcion que ejecutaran los botones del popup (si hay mas de uno deben ir separados por ';' ejem:  funcion1;funcion2;...)
JANCHO_POPUP -> Ancho del popup en Porcentaje (Valor por defecto 30% en caso no se le envie este valor)
JLOAD_UTILIDAD ->  Array que contiene el nombre de una Clase CSS O ID correspondiente a un contenedor que llamara a un formulario(PAGINA) con el evento load
    y tambien contiene el nombre del formulario(PAGINA) que invocara y por ultimo parametros si es que se desean enviar. ejemplo:  [".DatosUsuario", "ruta/Pagina.aspx", "Parametro1;Parametro2"]
    NOTA: ESTO ES PARA EL CASO EN QUE EL POPUP CARGUE CONTENIDO DE OTRA PANTALLA
    (no relizara ninguna accion en caso no se le envie este valor)
JARRAY_PARAMETROS -> ARRAY DE PARAMETROS QUE SE ENVIARAN AL POPUP *opcional
*/

$.JPopUp = function (JTITULO_POPUP, JCUERPO_POPUP, JBOTONES, JTEXTO_BOTONES, JFUNCION_BOTONES, JANCHO_POPUP, JLOAD_UTILIDAD, JARRAY_PARAMETROS) {
    var sDIV_FONDO_POPUP = "<div class='JFONDO-POPUP-1' style='opacity:0;'></div>";
    var sDIV_CONTENEDOR_POPUP = "<div class='JCONTENIDO-POPUP-1'>" +
                                        "<header>" + JTITULO_POPUP +
                                            "</header>" +
                                            "<div class='JCUERPO-POPUP'> " +

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

    for (var i = 0; i < parseInt(JBOTONES == "" ? 0 : JBOTONES); i++) {
        sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP +
                "<input type='button' value='" + aTE_BOTO[i] + "' onclick='" + aFU_BOTO[i] + "' />";
    }

    sDIV_CONTENEDOR_POPUP = sDIV_CONTENEDOR_POPUP + "</footer>" + "</div>";

    //INSERTANDO EL POPUP EN EL DOCUMENTO
    $("body").append(sDIV_FONDO_POPUP);
    $("body").append(sDIV_CONTENEDOR_POPUP);

    //PREGUNTANDO SI SE LE ASIGNARA UN ANCHO DEFINIDO, SI NO SE INDICA ESTE VALOR SE LE ASIGNARA POR DEFAULT 30%
    if (JANCHO_POPUP != undefined && JANCHO_POPUP != 0 && JANCHO_POPUP != null && JANCHO_POPUP != "") {
        $("body").find(".JCONTENIDO-POPUP-1").css("width", JANCHO_POPUP + "%");
    } else {
        $("body").find(".JCONTENIDO-POPUP-1").css("width", "30%");
    }

    /*OCULTANDO POPUP*/
    $(".JCONTENIDO-POPUP-1").css("top", "-" + $(window).height() + "px");
    $(".JCONTENIDO-POPUP-1").css("opacity", "0");

    //CARGANDO LA PAGINA DENTRO DEL POPUP       
    if (JARRAY_PARAMETROS != undefined && JARRAY_PARAMETROS != null && JARRAY_PARAMETROS != "") {
        $(".JCUERPO-POPUP").load(JCUERPO_POPUP, { "Parametro[]": JARRAY_PARAMETROS }, function () {
            if (JLOAD_UTILIDAD != undefined && JLOAD_UTILIDAD != "") {
                $(JLOAD_UTILIDAD[0]).load(JLOAD_UTILIDAD[1], { Parametro: JLOAD_UTILIDAD[2] }, function () {
                    $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");

                    /*MOSTRANDO EL POPUP*/
                    $(".JFONDO-POPUP-1").stop().animate({ opacity: 1 }, 50);
                    $(".JCONTENIDO-POPUP-1").stop().delay(200).animate({
                        opacity: 0.5
                    }, 100, function () {
                        if (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) > 0) { //SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
                            $(".JCONTENIDO-POPUP-1").stop().animate({
                                opacity: 1,
                                top: (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) / 2).toString() + "px",
                                left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                            }, 400);
                        } else { //SINO
                            $(".JCONTENIDO-POPUP-1").stop().animate({
                                opacity: 1,
                                top: "1px",
                                left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                            }, 400);
                            //28/11/2016
                            $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
                            $(".JCUERPO-POPUP").css("overflow-y", "auto");
                        }

                    });
                });
            } else {
                $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");

                /*MOSTRANDO EL POPUP*/
                $(".JFONDO-POPUP-1").stop().animate({ opacity: 1 }, 50);
                $(".JCONTENIDO-POPUP-1").stop().delay(200).animate({
                    opacity: 0.5
                }, 100, function () {
                    if (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) > 0) { //SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
                        $(".JCONTENIDO-POPUP-1").stop().animate({
                            opacity: 1,
                            top: (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) / 2).toString() + "px",
                            left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                        }, 400);
                    } else { //SINO
                        $(".JCONTENIDO-POPUP-1").stop().animate({
                            opacity: 1,
                            top: "1px",
                            left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                        }, 400);
                        //28/11/2016
                        $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
                        $(".JCUERPO-POPUP").css("overflow-y", "auto");
                    }
                });
            }
            $(".JCONTENIDO-POPUP-1").find(":button:last").focus(); /*13/07/2016*/
        });
    } else {
        $(".JCUERPO-POPUP").load(JCUERPO_POPUP, function () {
            if (JLOAD_UTILIDAD != undefined && JLOAD_UTILIDAD != "") {
                $(JLOAD_UTILIDAD[0]).load(JLOAD_UTILIDAD[1], { Parametro: JLOAD_UTILIDAD[2] }, function () {
                    $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");

                    /*MOSTRANDO EL POPUP*/
                    $(".JFONDO-POPUP-1").stop().animate({ opacity: 1 }, 50);
                    $(".JCONTENIDO-POPUP-1").stop().delay(200).animate({
                        opacity: 0.5
                    }, 100, function () {
                        if (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) > 0) { //SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
                            $(".JCONTENIDO-POPUP-1").stop().animate({
                                opacity: 1,
                                top: (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) / 2).toString() + "px",
                                left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                            }, 400);
                        } else { //SINO
                            $(".JCONTENIDO-POPUP-1").stop().animate({
                                opacity: 1,
                                top: "1px",
                                left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                            }, 400);
                            //28/11/2016
                            $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
                            $(".JCUERPO-POPUP").css("overflow-y", "auto");
                        }

                    });
                });
            } else {
                $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");

                /*MOSTRANDO EL POPUP*/
                $(".JFONDO-POPUP-1").stop().animate({ opacity: 1 }, 50);
                $(".JCONTENIDO-POPUP-1").stop().delay(200).animate({
                    opacity: 0.5
                }, 100, function () {
                    if (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) > 0) { //SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
                        $(".JCONTENIDO-POPUP-1").stop().animate({
                            opacity: 1,
                            top: (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) / 2).toString() + "px",
                            left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                        }, 400);
                    } else { //SINO
                        $(".JCONTENIDO-POPUP-1").stop().animate({
                            opacity: 1,
                            top: "1px",
                            left: (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px"
                        }, 400);
                        //28/11/2016
                        $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
                        $(".JCUERPO-POPUP").css("overflow-y", "auto");
                    }

                });
            }
            $(".JCONTENIDO-POPUP-1").find(":button:last").focus(); /*13/07/2016*/
        });
    }




    $(window).resize(function () {
        if ($(".JFONDO-POPUP-1").css("display") != "none") {
            if (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) > 0) {//SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
                $(".JCONTENIDO-POPUP-1").css("top", (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) / 2).toString() + "px");
                $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");
                //
                $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
                $(".JCUERPO-POPUP").css("overflow-y", "auto");
            } else {
                $(".JCONTENIDO-POPUP-1").css("top", "1px");
                $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");
                //28/11/2016
                $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
                $(".JCUERPO-POPUP").css("overflow-y", "auto");
            }
        }
    });
}


function fn_CentrarPopUp() {
    if ($(".JFONDO-POPUP-1").css("display") != "none") {
        if (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) > 0) {//SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
            $(".JCONTENIDO-POPUP-1").css("top", (($(window).height() - $(".JCONTENIDO-POPUP-1").outerHeight()) / 2).toString() + "px");
            $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");
        } else {
            $(".JCONTENIDO-POPUP-1").css("top", "1px");
            $(".JCONTENIDO-POPUP-1").css("left", (($(window).width() - $(".JCONTENIDO-POPUP-1").outerWidth()) / 2).toString() + "px");
            //28/11/2016
            $(".JCUERPO-POPUP").css("max-height", ($(window).height() - 150) + "px");
            $(".JCUERPO-POPUP").css("overflow-y", "auto");
        }
    }
}


//JB - POPUP v2
function fn_oculta_popup2(IdPopup, funcion) {
    $("#" + IdPopup).stop().animate({ top: "-" + $(window).height() + "px",
        opacity: 0
    }, 500, function () {
        $(".JFONDO-POPUP-2").stop().animate({ opacity: 0 }, 50);
        $(".JFONDO-POPUP-2").css("display", "none");

        $("body").find(".JFONDO-POPUP-2").remove();
        $("body").find("#" + IdPopup).remove();
        if (funcion != undefined) { //SI SE RECIBE ESTE PARAMETRO(ES EL NOMBRE DE UNA FUNCION EN JAVASCRIPT) LO EJECUTARA NI BIEN OCULTE EL POPUP.
            eval("" + funcion + "()");
        }
    });
}



function fn_CentrarPopUp2(IdPopup) {
    if ($(".JFONDO-POPUP-2").css("display") != "none") {
        if (($(window).height() - $("#" + IdPopup).outerHeight()) > 0) {//SI EL ALTO DEL POPUP NO ES MAS GRANDE AL ALTO DE LA PANTALLA...
            $("#" + IdPopup).css("top", (($(window).height() - $("#" + IdPopup).outerHeight()) / 2).toString() + "px");
            $("#" + IdPopup).css("left", (($(window).width() - $("#" + IdPopup).outerWidth()) / 2).toString() + "px");
        } else {
            $("#" + IdPopup).css("top", "20px");
            $("#" + IdPopup).css("left", (($(window).width() - $("#" + IdPopup).outerWidth()) / 2).toString() + "px");
            
        }
    }
}

function fn_CentrarPopUp2_(IdPopup) {
    if ($(".JFONDO-POPUP-2").css("display") != "none") {
        $("#" + IdPopup).css("top", (($(window).height() - $("#" + IdPopup).outerHeight()) / 2).toString() + "px");
        $("#" + IdPopup).css("left", (($(window).width() - $("#" + IdPopup).outerWidth()) / 2).toString() + "px");
    }
}

function fn_MostrarPopup2(IdPopup, centrar) {
    $("body").append("<div class='JFONDO-POPUP-2' style='z-index:9998;'></div>");
    $("#" + IdPopup).fadeIn("slow", function () {

    });
          
    if (centrar == true) {
        fn_CentrarPopUp2(IdPopup);
    }

    $(window).resize(function () {
        fn_CentrarPopUp2(IdPopup);
    });
    
}

function fn_OcultarPopup2(IdPopup) {
    $(".JFONDO-POPUP-2").stop().animate({ opacity: 0 }, 50);
    $(".JFONDO-POPUP-2").css("display", "none");    
    $("#" + IdPopup).fadeOut("slow", function () {
        $("body").find(".JFONDO-POPUP-2").remove();
    });

}