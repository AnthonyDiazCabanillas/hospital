$(document).ready(function () {
    $("body").find("form").last().append("<div style='background:rgba(0,0,0,0.2); position:fixed; width:100%; height:100%; top:0; left:0;' class='Fondo_Loader'></div>" +
        "<div class='JLOADER-2'>" +
            "<div class='JARO-1'></div>" +
            "<div class='JARO-2'></div>" +
        "</div>");
});


$(window).load(function () {
    $(".Fondo_Loader").remove();
    $(".JLOADER-2").remove();
});

/*FUNCIONES PARA VISUALIZAR/OCULTA LOADER EN GENERAL 999*/
function fn_LOAD_VISI() {    
    $("body").find("form").append("<div style='background:rgba(0,0,0,0.2); position:fixed; width:100%; height:100%; top:0; left:0;z-index:9999;' class='Fondo_Loader'></div>" + /*09/06/2016*/
        "<div class='JLOADER-2' style='z-index:9999;'>" +
            "<div class='JARO-1'></div>" +
            "<div class='JARO-2'></div>" +
        "</div>");
}

function fn_LOAD_OCUL() {
    $(".Fondo_Loader").remove();
    $(".JLOADER-2").remove();
}


/*FUNCIONES PARA VISUALIZAR U OCULTAR LOADER EN LOS GRID CUANDO CARGAN*/
function fn_LOAD_GRID_VISI() {
    $(".JFORM-CONTENEDOR-GRID").append("<div style='background:rgba(0,0,0,0.2); position:absolute; width:100%; height:100%; top:0; left:0;' class='Fondo_Loader_Grid'></div>" +
        "<div class='JLOADER-3'>" +
            "<div class='JARO-1'></div>" +
            "<div class='JARO-2'></div>" +
        "</div>");
}

function fn_LOAD_GRID_OCUL() {
    $(".Fondo_Loader_Grid").remove();
    $(".JLOADER-3").remove();
}


/*FUNCION PARA VISUALIZAR U OCULTAR LOADER EN LOS GRID DEL POPUP - TAMBIEN PARA MOSTRAR EL LOADER EN CUALQUIER POPUP NO NECESARIAMENTE DEBE TENER UN GRID*/
function fn_LOAD_POPU_VISI() {
    $(".JFORM-CONTENEDOR-POPUP").append("<div style='background:rgba(0,0,0,0.2); position:absolute; width:100%; height:100%; top:0; left:0;' class='Fondo_Loader_Grid'></div>" +
        "<div class='JLOADER-3'>" +
            "<div class='JARO-1'></div>" +
            "<div class='JARO-2'></div>" +
        "</div>");
}

function fn_LOAD_POPU_OCUL() {
    $(".Fondo_Loader_Grid").remove();
    $(".JLOADER-3").remove();
}



