$(document).ready(function () {
    $('.JTREE li').each(function () {
        if ($(this).children('ul').length > 0) {
            $(this).addClass('parent');
        }
    });

    $('.JTREE li.parent > a').click(function () {
        $(this).parent().toggleClass('active');
        $(this).parent().children('ul').slideToggle('fast');
    });

    $('#all').click(function () {
        $('.JTREE li').each(function () {
            $(this).toggleClass('active');
            $(this).children('ul').slideToggle('fast');
        });
    });

        

    $(".JTree-Element").click(function () {
        //alert($(this).html());
    });
});

function fn_RenderizaTreeView2(IdContenedorTree, SelecionarItem) {
    var toggler = $("#" + IdContenedorTree).find(".nudo");    
    var i;
    //a cada nodo se le agregara el evento click    
    for (i = 0; i < toggler.length; i++) {
        toggler[i].addEventListener("click", function () {
            $("#" + IdContenedorTree).find(".nudo").removeClass("JTREE2-SELECCIONADO"); //limpiando registro seleccionado
            $("#" + IdContenedorTree).find(".JTREE2-SELECCIONADO").removeClass("JTREE2-SELECCIONADO"); 
            this.parentElement.querySelector(".anidado").classList.toggle("active");
            this.classList.toggle("nudo-down");
            $(this).addClass("JTREE2-SELECCIONADO");

            //if ($(this).attr("class").includes("nudo-down")) { //si abre el nodo aplicar el color verde para identificar el registro seleccionado
            //    $(this).addClass("JTREE2-SELECCIONADO");
            //}
        });
        //toggler[0].classList.toggle("nudo-down");
        //toggler[0].parentElement.querySelector(".anidado").classList.toggle("active");
    }
    $("#" + IdContenedorTree).find(".nudo").eq(0).trigger("click");
    //$("#" + IdContenedorTree).find(".nudo").eq(1).trigger("click"); // JB  - 15/01/2020 - PARA QUE SE VISUALICE EL PRIMER REGISTRO DE ESA FECHA

    if (SelecionarItem === true) {
        $("#" + IdContenedorTree).find(".JTree-Element").click(function () {
            $("#" + IdContenedorTree).find(".JTREE2-SELECCIONADO").removeClass("JTREE2-SELECCIONADO");
            $(this).find("span").addClass("JTREE2-SELECCIONADO");
        });
    }
    


    //var toggler = document.getElementsByClassName("nudo");
    //var i;    
    ////a cada nodo se le agregara el evento click    
    //for (i = 0; i < toggler.length; i++) {        
    //    toggler[i].addEventListener("click", function () {
    //        $(".nudo").removeClass("JTREE2-SELECCIONADO"); //limpiando registro seleccionado
    //        this.parentElement.querySelector(".anidado").classList.toggle("active");
    //        this.classList.toggle("nudo-down");
    //        $(this).addClass("JTREE2-SELECCIONADO");

    //        //if ($(this).attr("class").includes("nudo-down")) { //si abre el nodo aplicar el color verde para identificar el registro seleccionado
    //        //    $(this).addClass("JTREE2-SELECCIONADO");
    //        //}
    //    });
    //    toggler[i].classList.toggle("nudo-down");
    //    toggler[i].parentElement.querySelector(".anidado").classList.toggle("active");
    //}
    

    //$(".JTree-Element").click(function () {
    //    //alert($(this).html());
    //});
}



//function fn_CreaEventosTreeView() {
//    $('.JTREE li').each(function () {
//        if ($(this).children('ul').length > 0) {
//            $(this).addClass('parent');
//        }
//    });

//    $('.JTREE li.parent > a').unbind("click");
//    $('.JTREE li.parent > a').click(function () {
//        $(this).parent().toggleClass('active');
//        $(this).parent().children('ul').slideToggle('fast');
//    });

//    $('#all').unbind("click");
//    $('#all').click(function () {
//        $('.JTREE li').each(function () {
//            $(this).toggleClass('active');
//            $(this).children('ul').slideToggle('fast');
//        });
//    });
//}


/*CLINICA*/
/*
IdControlT -> ID DEL CONTROL QUE CONTIENE EL TREEVIE (CONTENEDOR)
IdControlHidden -> PARAMETRO OPCIONAL, REPRESENTA EL ID DE UN CONTROL DONDE SE GUARDARA EL VALOR SELECCIONADO DEL TREEVIEW
*/
function fn_CreaEventosTreeViewAI(IdControlT, IdControlHidden, Centrado) {    
    $('#' + IdControlT + ' li').each(function () {
        if ($(this).children('ul').length > 0) {
            $(this).addClass('parent');
        }
    });

    $('#' + IdControlT + ' li.parent > a').unbind("click");
    $('#' + IdControlT + ' li.parent > a').click(function () {
        $(this).parent().toggleClass('active');
        $(this).parent().children('ul').slideToggle('fast').promise().done(function () {
            /*CLINICA SF*/
            if (Centrado != undefined && Centrado != null && Centrado != "") {
                fn_CentrarPopUp();
            }
            /*FIN CLINICA SF*/
        });
        /*CLINICA SF*/
        if (IdControlT != undefined && IdControlT != null && IdControlT != "") {
            $("#" + IdControlT).find("a").css("color", "#134B8D");
        }
        $(this).css("color", "#8DC73F");
        if (IdControlHidden != undefined && IdControlHidden != null && IdControlHidden != "") {
            $("#" + IdControlHidden).val($(this).next().val().trim());
        }
        /*FIN CLINICA SF*/
    });

    $('#all').unbind("click");
    $('#all').click(function () {
        $('.JTREE li').each(function () {
            $(this).toggleClass('active');
            $(this).children('ul').slideToggle('fast');
        });
    });
}

