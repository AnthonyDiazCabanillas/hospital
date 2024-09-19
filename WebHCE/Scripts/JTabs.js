/*CODIGO JAVASCRIPT PARA EL FUNCIONAMIENTO DEL TAB - CREADO POR JONATHAN BARRETO.*/

$(document).ready(function () {
    /*OCULTANDO LOS TABS*/
    $(".JCUERPO").css("display", "none");

    /*COLOCANDO EL NOMBRE DEL TAB EN EL LABEL (SOLO SE VERA CUANDO EL ANCHO DE PANTALLA SEA MENOR A 600px) Y MOSTRANDO TAB EL ACTIVO*/
    $(".JSBTAB_ACTIVO").each(function () {
        var sDE_CLAS = $(this).parent().parent().parent().prop("class");
        var nNU_INDI_ = $(this).index();
        $(document).find("." + sDE_CLAS + " > .JCUERPO").eq(nNU_INDI_).css("display", "block");

        $(document).find("." + sDE_CLAS + " > .JSBTABS > .JSBMOSTRAR_TABS").html($(this).find("a").html().trim());
    });



    /*EVENTO CLICK EN EL TAB Y EN LA EL ICONO DEL TAB (PARTE SUPERIOR DERECHA)*/ /*actualmente no esta visible el icono 25/05/2016*/
    $(".JSBTABS ul li, .JSBTABS_NAVE").click(function () {
        var sDE_CLAS = $(this).parent().parent().parent().prop("class");
        var nNU_INDI = $(this).index();

        /*REMOVIENDO LA CLASE DEL TAB ACTIVO A TODOS LOS TABS Y ACTIVANDOLO EN EL TAB SELECCIONADO*/
        $(document).find("." + sDE_CLAS + " > .JSBTABS ul li").each(function () {
            $(this).removeClass("JSBTAB_ACTIVO");
        });
        $(document).find("." + sDE_CLAS + " > .JSBTABS ul li").eq(nNU_INDI).addClass("JSBTAB_ACTIVO");

        var obj = $(this);

        /*CODIGO SOLO CUANDO ANCHO DE PANTALLA SEA MENOR A 600PX*/
        /*SI EL LABEL ESTA VISIBLE DISPARA EL EVENTO CLICK DEL CHECK OCULTO QUE MUESTRA U OCULTA LOS TABS*/
        if ($(".JSBMOSTRAR_TABS").css("display") != "none") {
            $(document).find("." + sDE_CLAS + " > div > .chkTAB-CHECK").trigger("click");

            if ($(document).find("." + sDE_CLAS + " > div > .chkTAB-CHECK").prop("checked") == true) {
                //alert("true");
            }
            /*MOSTRANDO EL NOMBRE DEL TAB SELECCIONADO EN EL LABEL (ANCHO PANTALLA 600px O MENOS)*/
            $(document).find("." + sDE_CLAS + " > .JSBTABS > .JSBMOSTRAR_TABS").html(obj.find("a").html());


            if ($(document).find("." + sDE_CLAS + " > .JSBTABS ul li").css("display") == "block") {
                $(document).find("." + sDE_CLAS + " > div > .chkTAB-CHECK").prop("checked", false);
            } 
        }


        /*OCULTANDO LOS TABS*/
        //$(".JCUERPO").css("display", "none");
        $(document).find("." + sDE_CLAS + " > .JCUERPO").css("display", "none");

        /*MOSTRANDO SOLO EL SELECCIONADO*/
        //$(".JCUERPO").eq(nNU_INDI).css("display", "block");
        $(document).find("." + sDE_CLAS + " > .JCUERPO").eq(nNU_INDI).css("display", "block");

    });

});

/*FUNCION SOLO USADA PARA EL CASO QUE SE ARMA LOS TABS DE MANERA DINAMICA*/
function fn_RenderizarTabs() {
    /*OCULTANDO LOS TABS*/
    $(".JCUERPO").css("display", "none");

    /*COLOCANDO EL NOMBRE DEL TAB EN EL LABEL (SOLO SE VERA CUANDO EL ANCHO DE PANTALLA SEA MENOR A 600px) Y MOSTRANDO TAB EL ACTIVO*/
    $(".JSBTAB_ACTIVO").each(function () {
        var sDE_CLAS = $(this).parent().parent().parent().prop("class");
        var nNU_INDI_ = $(this).index();
        $(document).find("." + sDE_CLAS + " > .JCUERPO").eq(nNU_INDI_).css("display", "block");

        $(document).find("." + sDE_CLAS + " > .JSBTABS > .JSBMOSTRAR_TABS").html($(this).find("a").html().trim());
    });



    /*EVENTO CLICK EN EL TAB Y EN LA EL ICONO DEL TAB (PARTE SUPERIOR DERECHA)*/ /*actualmente no esta visible el icono 25/05/2016*/
    $(".JSBTABS ul li, .JSBTABS_NAVE").click(function () {
        var sDE_CLAS = $(this).parent().parent().parent().prop("class");
        var nNU_INDI = $(this).index();

        /*REMOVIENDO LA CLASE DEL TAB ACTIVO A TODOS LOS TABS Y ACTIVANDOLO EN EL TAB SELECCIONADO*/
        $(document).find("." + sDE_CLAS + " > .JSBTABS ul li").each(function () {
            $(this).removeClass("JSBTAB_ACTIVO");
        });
        $(document).find("." + sDE_CLAS + " > .JSBTABS ul li").eq(nNU_INDI).addClass("JSBTAB_ACTIVO");

        var obj = $(this);

        /*CODIGO SOLO CUANDO ANCHO DE PANTALLA SEA MENOR A 600PX*/
        /*SI EL LABEL ESTA VISIBLE DISPARA EL EVENTO CLICK DEL CHECK OCULTO QUE MUESTRA U OCULTA LOS TABS*/
        if ($(".JSBMOSTRAR_TABS").css("display") != "none") {
            $(document).find("." + sDE_CLAS + " > div > .chkTAB-CHECK").trigger("click");

            if ($(document).find("." + sDE_CLAS + " > div > .chkTAB-CHECK").prop("checked") == true) {
                //alert("true");
            }
            /*MOSTRANDO EL NOMBRE DEL TAB SELECCIONADO EN EL LABEL (ANCHO PANTALLA 600px O MENOS)*/
            $(document).find("." + sDE_CLAS + " > .JSBTABS > .JSBMOSTRAR_TABS").html(obj.find("a").html());


            if ($(document).find("." + sDE_CLAS + " > .JSBTABS ul li").css("display") == "block") {
                $(document).find("." + sDE_CLAS + " > div > .chkTAB-CHECK").prop("checked", false);
            }
        }


        /*OCULTANDO LOS TABS*/
        //$(".JCUERPO").css("display", "none");
        $(document).find("." + sDE_CLAS + " > .JCUERPO").css("display", "none");

        /*MOSTRANDO SOLO EL SELECCIONADO*/
        //$(".JCUERPO").eq(nNU_INDI).css("display", "block");
        $(document).find("." + sDE_CLAS + " > .JCUERPO").eq(nNU_INDI).css("display", "block");

    });
}