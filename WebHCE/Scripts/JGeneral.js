/*FUNCIONES EN GENERAL PARA LOS FORMULARIOS (VALIDACIONES, ETC) - CREADO BY JONATHAN B.*/

$(document).ready(function () {
    //$("input[type='text']").val("");

    $(".JFECHA").datepicker({ dateFormat: 'dd/mm/yy' });

    //CAMPO NUMERICO
    $(".JNUMERO").keypress(function (event) {
        var ValidacionNumerica = /[0-9]/;
        if (!ValidacionNumerica.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });

    $(".JTEXTO-C").keypress(function (event) { //SOLO TEXTO
        var ValidacionCaracter = /[A-Za-z\s]/;
        if (!ValidacionCaracter.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });

    //CAMPO HORA
    $(".JHORA").keypress(function (event) {
        var ValidacionFormatoHora = /[0-9:]/;
        if (!ValidacionFormatoHora.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
        var control = $(this);
        var formato = "00:00";
        var ValorCampo = "";
        var CaracterIngresado = String.fromCharCode(event.which);

        control.attr("maxlength", formato.length);
        if (control.val().trim() == "") {
            ValorCampo = String.fromCharCode(event.which);
        } else {
            ValorCampo = control.val().trim() + String.fromCharCode(event.which);
        }
        for (var i = 0; i < formato.length; i++) {
            if ((ValorCampo.length - 1) == i) {
                if (formato.substring(i, i + 1) == ":" && CaracterIngresado != ":") {
                    event.preventDefault();
                    control.val(control.val() + ":" + CaracterIngresado);
                }
                if (CaracterIngresado == ":" && formato.substring(i, i + 1) != ":") {
                    event.preventDefault();
                }
            }
        }
    });
    $(".JHORA").blur(function () {
        var ValidacionHora = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;
        if (!ValidacionHora.test($(this).val())) {
            $(this).val("");
        }
    });


    //CAMPO OBLIGATORIO
    $(".JCAMP-OBLI").blur(function () {
        if ($(this).val().trim() != "") {
            $(this).removeClass("JCAMP-OBLI-ROJO");
        } else {
            $(this).addClass("JCAMP-OBLI-ROJO");
        }
    });

    /*CODIGO PARA TOOLTIP-TITLE - 08/08/2016 - JONATHAN B*/
    /*$(".JIMG-GENERAL[title], .JBOTON-IMAGEN[title]").each(function () {
        var titulo = $(this).attr("title");
        $(this).after("<span class='TooltipClinica'>" + titulo + "</span>").removeAttr("title");
        $(this).addClass("JTITULO");
    });
    $(".JTITULO").mouseenter(function () {
        $(this).next().css("visibility", "visible");
    });
    $(".JTITULO").mouseleave(function () {
        $(this).next().css("visibility", "hidden");
    });*/
    $(".JIMG-GENERAL[title], .JBOTON-IMAGEN[title]").each(function () {
        $(this).removeAttr("title");
    });

});

function fn_FuncionAjax(vURL, vDATO, vFUNCION) {
    $.ajax({
        url: vURL,
        type: "POST",
        data: JSON.stringify(vDATO),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (dato1, datos2, dato3) {
        },
        success: function (oOB_JSON) {
            window[vFUNCION](oOB_JSON);
        }
    });
}


function fn_CampoBase() {
    $(".JFECHA").datepicker({ dateFormat: 'dd/mm/yy' });

    //CAMPO SOLO NUMERO
    $(".JNUMERO").keypress(function (event) {
        var ValidacionNumerica = /[0-9]/;
        if (!ValidacionNumerica.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });

    $(".JTEXTO-C").keypress(function (event) { //SOLO TEXTO
        var ValidacionCaracter = /[A-Za-z\s]/;        
        if (!ValidacionCaracter.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });

    //CAMPO HORA
    $(".JHORA").keypress(function (event) {
        var ValidacionFormatoHora = /[0-9:]/;
        if (!ValidacionFormatoHora.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
        var control = $(this);
        var formato = "00:00";
        var ValorCampo = "";
        var CaracterIngresado = String.fromCharCode(event.which);

        control.attr("maxlength", formato.length);
        if (control.val().trim() == "") {
            ValorCampo = String.fromCharCode(event.which);
        } else {
            ValorCampo = control.val().trim() + String.fromCharCode(event.which);
        }
        for (var i = 0; i < formato.length; i++) {
            if ((ValorCampo.length - 1) == i) {
                if (formato.substring(i, i + 1) == ":" && CaracterIngresado != ":") {
                    event.preventDefault();
                    control.val(control.val() + ":" + CaracterIngresado);
                }
                if (CaracterIngresado == ":" && formato.substring(i, i + 1) != ":") {
                    event.preventDefault();
                }
            }
        }
    });
    $(".JHORA").blur(function () {
        var ValidacionHora = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;
        if (!ValidacionHora.test($(this).val())) {
            $(this).val("");
        }
    });
}

//FUNCION PARA CREAR LOS TOOLTIP
function fn_CreaTooltip() {
    /*$(".JIMG-GENERAL[title]").each(function () {
        var titulo = $(this).attr("title");
        $(this).after("<span class='TooltipClinica'>" + titulo + "</span>").removeAttr("title");
        $(this).addClass("JTITULO");
    });
    $(".JTITULO").mouseenter(function () {
        $(this).next().css("visibility", "visible");
    });
    $(".JTITULO").mouseleave(function () {
        $(this).next().css("visibility", "hidden");
    });*/
    $(".JIMG-GENERAL[title], .JBOTON-IMAGEN[title]").each(function () {
        $(this).removeAttr("title");
    });
}

/*FUNCION PARA VALIDAR CAMPOS OBLIGATORIOS (DEBE TENER LA CLASE JCAMP-OBLI PARA RECONOCER QUE EL CAMPO SERA OBLIGATORIO) Y MARCALOS CON ROJO*/
function fn_ValidaCampoObligatorio() {
    $(".JCAMP-OBLI").blur(function () {
        if ($(this).val().trim() != "") {
            $(this).removeClass("JCAMP-OBLI-ROJO");
        } else {
            $(this).addClass("JCAMP-OBLI-ROJO");
        }
    });
}
$.JValidaCampoObligatorio = function (JCONTROLES) {
    var bValidacionOk = true;

    var aID_CTRL = new Array();
    if (JCONTROLES != undefined) {
        aID_CTRL = JCONTROLES.split(";");
    }

    for (var i = 0; i < aID_CTRL.length; i++) {
        $("#" + aID_CTRL[i]).addClass("JCAMP-OBLI");

        if ($("#" + aID_CTRL[i]).val().trim() != "") {
            $("#" + aID_CTRL[i]).removeClass("JCAMP-OBLI-ROJO");
            //$("#" + aID_CTRL[i]).attr("placeholder", "");
        } else {
            $("#" + aID_CTRL[i]).addClass("JCAMP-OBLI-ROJO");
            bValidacionOk = false;
            //$("#" + aID_CTRL[i]).attr("placeholder", "Obligatorio").val("").focus().blur();
        }
    }
    fn_ValidaCampoObligatorio();
    return bValidacionOk;
}


//FUNCION PARA DESHABILITAR LOS CONTROLES POR LOS ID
function fn_DeshabilitaControles(JCONTROLES) {
    var aID_CTRL = new Array();
    if (JCONTROLES != undefined) {
        aID_CTRL = JCONTROLES.split(";");
    }

    for (var i = 0; i < aID_CTRL.length; i++) {
        $("#" + aID_CTRL[i].trim()).prop("disabled", "disabled");
    }
}
//FUNCION PARA HABILITAR LOS CONTROLES POR LOS ID
function fn_HabilitaControles(JCONTROLES) {
    var aID_CTRL = new Array();
    if (JCONTROLES != undefined) {
        aID_CTRL = JCONTROLES.split(";");
    }

    for (var i = 0; i < aID_CTRL.length; i++) {
        $("#" + aID_CTRL[i].trim()).removeAttr("disabled");
    }
}

function fn_DeshabilitaControlIMG(JCONTROLES) {
    var aID_CTRL = new Array();
    if (JCONTROLES != undefined) {
        aID_CTRL = JCONTROLES.split(";");
    }

    for (var i = 0; i < aID_CTRL.length; i++) {
        $("#" + aID_CTRL[i].trim()).css("opacity", "0.5");
    }
}

function fn_HabilitaControlIMG(JCONTROLES) {
    var aID_CTRL = new Array();
    if (JCONTROLES != undefined) {
        aID_CTRL = JCONTROLES.split(";");
    }

    for (var i = 0; i < aID_CTRL.length; i++) {
        $("#" + aID_CTRL[i].trim()).css("opacity", "1");
    }
}









//DESHABILITA CONTROLES DEL FORMAFULARIO InformacionPaciente.aspx
function fn_DeshabilitaControlesI() {
    $(".JTEXTO, .JTEXTO-1, .JTEXTO-2, .JTEXTO-3, .JTEXTO-4, .JTEXTO-5, .JTEXTO-6, .JTEXTO-7, .JTEXTO-8, .JTEXTO-9, .JTEXTO-10, .JTEXTO-11, .JTEXTO-12, .JSELECT").prop("disabled", "disabled");
    $(".JBOTON").prop("disabled", "disabled");
    $(".JNUMERO").prop("disabled", "disabled");
    $(".JFECHA").prop("disabled", "disabled");
    $("input[type='radio']").prop("disabled", "disabled");
    $("input[type='checkbox']").prop("disabled", "disabled");
    $("input[type='button'], input[type='submit']").prop("disabled", "disabled");
    $(".JIMG-BUSQUEDA").unbind("click");
    $(".JIMG-BUSQUEDA").css("opacity", "0.6");
    $(".JIMG-BUSQUEDA").prop("disabled", "disabled");
    $(".JIMG-FAVORITO").unbind("click");
    $(".JIMG-FAVORITO").css("opacity", "0.6");
    $(".JIMG-FAVORITO").prop("disabled", "disabled");

    $(".JIMG-LABORATORIO").unbind("click");
    $(".JIMG-LABORATORIO").css("opacity", "0.6");
    $(".JIMG-LABORATORIO").prop("disabled", "disabled");
    $(".JIMG-IMAGEN").unbind("click");
    $(".JIMG-IMAGEN").css("opacity", "0.6");
    $(".JIMG-IMAGEN").prop("disabled", "disabled");
    $(".JIMG-ELIMINAR").unbind("click");
    $(".JIMG-ELIMINAR").css("opacity", "0.6");
    $(".JIMG-ELIMINAR").prop("disabled", "disabled");
    $(".JIMG-ESTADO").unbind("click");
    $(".JIMG-ESTADO").css("opacity", "0.6");
    $(".JIMG-ESTADO").prop("disabled", "disabled");
    //JIMG-LABORATORIO  JIMG-IMAGEN
}


function HabilitaControl(IdControl) {
    var Controles = new Array();
    Controles = IdControl.toString().split(";");
    for (var i = 0; i < Controles.length; i++) {
        $("#" + Controles[i].toString()).data("valores").Deshabilitado = false;
        $("#" + Controles[i].toString()).prop("disabled", false);
    }
}

function DeshabilitaControl(IdControl) {
    var Controles = new Array();
    Controles = IdControl.toString().split(";");
    for (var i = 0; i < Controles.length; i++) {
        $("#" + Controles[i].toString()).data("valores").Deshabilitado = true;
        $("#" + Controles[i].toString()).prop("disabled", true);
    }
}




function fn_InicializarCombo() {
    $(".JSELECT2").find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");

    $(".JSELECT2").find(".JSELECT2-SELECCION").unbind("click");
    $(".JSELECT2").find(".JSELECT2-SELECCION").click(function () {
        var IdControl = "";
        IdControl = $(this).parent().attr("id");
        fn_OcultarCombo(IdControl);
        var oObjeto = $(this);
        var oObjetoPadre = $(this).parent(); //JB - 03/05/2021
        if (oObjeto.parent().find(".JSELECT2-ELEMENT").length > 0) { //si hay elementos en el combo            
            if (oObjeto.parent().find(".JSELECT2-ELEMENT").attr("class").indexOf("INVISIBLE") > -1) { //si esta oculta se mostrara
                oObjeto.parent().find(".JSELECT2-ELEMENT").removeClass("JSELECT2-INVISIBLE");
                oObjeto.parent().find(".JSELECT2-ELEMENT").addClass("JSELECT2-VISIBLE");
                $(".JSELECT2").css("z-index", "1");
                oObjeto.parent().css("z-index", "999");
                oObjetoPadre.css("position", "absolute");
            } else {
                oObjeto.parent().find(".JSELECT2-ELEMENT").removeClass("JSELECT2-VISIBLE");
                oObjeto.parent().find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");
                oObjetoPadre.css("position", "relative");
            }
            oObjeto.parent().find(".JSELECT2-SELECCION").toggleClass("JSELECT2-ACTIVO");
        }
    });


    //    $(".JSELECT2").each(function () {
    //        $(this).find(".JSELECT2-ELEMENT").click(function () {
    //            $(this).parent().parent().data("codigo", $(this).next().text());
    //            $(this).parent().parent().data("descripcion", $(this).next().text());
    //            $(this).parent().find(".JSELECT2-ELEMENT").removeClass("JSELECT2-VISIBLE");
    //            $(this).parent().find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");
    //            $(this).parent().find(".JSELECT2-ITEM:eq(0)").toggleClass("JSELECT2-ACTIVO");            
    //            //removeData                    
    //        });
    //    });
}

function fn_OcultarCombo(IdControl) {
    $(".JSELECT2").each(function () {
        var oObjeto = $(this);
        if ($(this).attr("id") != IdControl) {
            if (oObjeto.find(".JSELECT2-ELEMENT").length > 0) {
                if (oObjeto.find(".JSELECT2-ELEMENT").attr("class").indexOf("-VISIBLE") > -1) {
                    oObjeto.find(".JSELECT2-ELEMENT").removeClass("JSELECT2-VISIBLE");
                    oObjeto.find(".JSELECT2-ELEMENT").addClass("JSELECT2-INVISIBLE");
                    oObjeto.find(".JSELECT2-SELECCION").toggleClass("JSELECT2-ACTIVO");                                        
                }
            }
        }
    });

}