<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CambiarPassword.aspx.vb" Inherits="WebHCE.CambiarPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            javascript: window.history.forward(1);

            var xPassword = ""
            var blMinLongitud = false;
            var blMinLetraMayus = false;
            var blMinLetraMinus = false;
            var blMinNumero = false;
            var blMinEspecial = false;

            var blMinLongitud2 = false;
            var blMinLetraMayus2 = false;
            var blMinLetraMinus2 = false;
            var blMinNumero2 = false;
            var blMinEspecial2 = false;
            var blIguales = false;

            var strCadenaP1;
            var strCadenaP2;
            var minCaracter;
            var maxCaracter;
            var minLetraMayus;
            var minLetraMinus;
            var minNumeros;
            var minEspecial;
            var caracteresnovalidos = ['\''];

            mostrarTabla();

            var pNuevoPassword = document.getElementById("txtNuevoPassword");
            pNuevoPassword.disabled = false;
            var pRepetirPassword = document.getElementById("txtRepetirPassword");
            pRepetirPassword.disabled = false;
            var pImgOk1 = document.getElementById("imgOk1");
            var pImgOk2 = document.getElementById("imgOk2");
            var pImgOk3 = document.getElementById("imgOk3");

            validaPassAnterior(pImgOk1, 1)
            validaPassAnterior(pImgOk2, 1)
            validaPassAnterior(pImgOk3, 1)

            var pButtonCambiarContrasenia = document.getElementById("btn_CambiarContrasenia");
            pButtonCambiarContrasenia.disabled = true;


            //******************************************************** */
            document.getElementById('txtAnteriorPassword').addEventListener('keydown', function (e)
            {
                const teclaPresionada = event.key.toLowerCase();
                if (caracteresnovalidos.includes(teclaPresionada))
                {
                    e.preventDefault();
                }

                else if (e.keyCode === 13)
                {
                    e.preventDefault();
                    var pButtonCambiarContrasenia = document.getElementById("btn_CambiarContrasenia");

                    if (validaPassFinal() == 1) {
                        pButtonCambiarContrasenia.disabled = true;
                    }
                    else
                    {
                        pButtonCambiarContrasenia.disabled = false;
                        fn_CambiarContraseña();
                    } 
                   
                    //var k = null;
                    //(e.keyCode) ? k = e.keyCode : k = e.which;
                    //var pImgOk1 = document.getElementById("imgOk1");
                    //var pImgOk2 = document.getElementById("imgOk2");
                    //var pImgOk3 = document.getElementById("imgOk3");

                    //var protocol = window.location.protocol;
                    //var url = window.location.host;
                    //var path = window.location.pathname;
                    //var parts = path.substr(1).split('/');
                    //var raiz = parts[0];

                    ////if (pOjo.src == protocol + '//' + url + '/' + raiz + "/Imagenes/eye_si.png") {
                    //if (pImgOk1.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png" || pImgOk2.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png" || pImgOk3.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png") {
                    ////if (pImgOk1.src == "../Imagenes/noSeg.png" || pImgOk2.src == "../Imagenes/noSeg.png" || pImgOk3.src == "../Imagenes/noSeg.png") {
                    //    $.JMensajePOPUP("Validación", "Los datos ingresados no son válidos.", "1", "Cerrar", "fn_oculta_mensaje()");
                    //    e.preventDefault();
                    //}
                    //else
                    //{
                    //    if (k == 13)
                    //    {
                    //        //alert("HOLI ENTER - OK");
                    //        fn_CambiarContraseña();
                    //        return true;
                    //    }
                    //}
                }
            });
            //***********************************************************/
            document.getElementById('txtNuevoPassword').addEventListener('keydown', function (e)
            {                
                const teclaPresionada = event.key.toLowerCase();
                if (caracteresnovalidos.includes(teclaPresionada))
                {
                    e.preventDefault();
                }
                else if (e.keyCode === 13)
                {
                    e.preventDefault();

                    var pButtonCambiarContrasenia = document.getElementById("btn_CambiarContrasenia");

                    if (validaPassFinal() == 1)
                    {
                        pButtonCambiarContrasenia.disabled = true;
                    }
                    else
                    {
                        pButtonCambiarContrasenia.disabled = false;
                        fn_CambiarContraseña();
                    }                     
                    //var k = null;
                    //(e.keyCode) ? k = e.keyCode : k = e.which;
                    //var pImgOk1 = document.getElementById("imgOk1");
                    //var pImgOk2 = document.getElementById("imgOk2");
                    //var pImgOk3 = document.getElementById("imgOk3");

                    //var protocol = window.location.protocol;
                    //var url = window.location.host;
                    //var path = window.location.pathname;
                    //var parts = path.substr(1).split('/');
                    //var raiz = parts[0];

                    ////if (pOjo.src == protocol + '//' + url + '/' + raiz + "/Imagenes/eye_si.png") {

                    //if (pImgOk1.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png" || pImgOk2.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png" || pImgOk3.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png") {
                    ////if (pImgOk1.src == "../Imagenes/noSeg.png" || pImgOk2.src == "../Imagenes/noSeg.png" || pImgOk3.src == "../Imagenes/noSeg.png") {
                    //    $.JMensajePOPUP("Validación", "Los datos ingresados no son válidos.", "1", "Cerrar", "fn_oculta_mensaje()");
                    //    e.preventDefault();
                    //}
                    //else
                    //{
                    //    if (k == 13)
                    //    {
                    //        fn_CambiarContraseña();
                    //        return true;
                    //    }
                    //}
                }
            });
            //***********************************************************/
            document.getElementById('txtRepetirPassword').addEventListener('keydown', function (e)
            {                
                const teclaPresionada = event.key.toLowerCase();
                if (caracteresnovalidos.includes(teclaPresionada)) {
                    e.preventDefault();
                }

                else if (e.keyCode === 13)
                {
                    e.preventDefault();
                    var pButtonCambiarContrasenia = document.getElementById("btn_CambiarContrasenia");

                    if (validaPassFinal() == 1)
                    {
                        pButtonCambiarContrasenia.disabled = true;
                    }
                    else
                    {
                        pButtonCambiarContrasenia.disabled = false;
                        fn_CambiarContraseña();
                    }
                    
                    //var k = null;
                    //(e.keyCode) ? k = e.keyCode : k = e.which;
                    //var pImgOk1 = document.getElementById("imgOk1");
                    //var pImgOk2 = document.getElementById("imgOk2");
                    //var pImgOk3 = document.getElementById("imgOk3");

                    //var protocol = window.location.protocol;
                    //var url = window.location.host;
                    //var path = window.location.pathname;
                    //var parts = path.substr(1).split('/');
                    //var raiz = parts[0];

                    ////if (pOjo.src == protocol + '//' + url + '/' + raiz + "/Imagenes/eye_si.png") {

                    //if (pImgOk1.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png" || pImgOk2.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png" || pImgOk3.src == protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png")
                    //{
                    ////if (pImgOk1.src == "../Imagenes/noSeg.png" || pImgOk2.src == "../Imagenes/noSeg.png" || pImgOk3.src == "../Imagenes/noSeg.png") {
                    //    $.JMensajePOPUP("Validación", "Los datos ingresados no son válidos.", "1", "Cerrar", "fn_oculta_mensaje()");
                    //    e.preventDefault();
                    //}
                    //else {
                    //    if (k == 13)
                    //    {                            
                    //        fn_CambiarContraseña();
                    //        return true;
                    //    }
                    //}
                }
            });
            //****************************************************************************************************** */
            /*Eventos paste */
            //****************************************************************************************************** */
            document.getElementById("txtAnteriorPassword").addEventListener("paste", function (event) {
                event.preventDefault();
            });
            //****************************************************************************************************** */
            document.getElementById("txtNuevoPassword").addEventListener("paste", function (event) {
                event.preventDefault();
            });
            //****************************************************************************************************** */
            document.getElementById("txtRepetirPassword").addEventListener("paste", function (event) {
                event.preventDefault();
            });
            //****************************************************************************************************** */
            $("#imgEye1").click(function () {
                mostrarContrasena(document.getElementById("txtAnteriorPassword"));
                cambiarOjo(this);
                return false;
            });

            $("#imgEye2").click(function () {
                mostrarContrasena(document.getElementById("txtNuevoPassword"));
                cambiarOjo(this);
                return false;
            });

            $("#imgEye3").click(function () {
                mostrarContrasena(document.getElementById("txtRepetirPassword"));
                cambiarOjo(this);
                return false;
            });

        });

        //*******AQUÍ TERMINA EL READY*/
        var data2;
        function removeSpaces(input)
        {
            input.value = input.value.replace(/\s/g, ''); // Elimina todos los espacios
        }

        // Aplica la función `removeSpaces` en cada campo de contraseña al escribir
        $("#txtAnteriorPassword, #txtNuevoPassword, #txtRepetirPassword").on("input", function () {
            removeSpaces(this);
        });
        /* Evento cuando digitan*/
        $("#txtAnteriorPassword").keyup(function ()
        {
            var pImgOk1 = document.getElementById("imgOk1");
            var pImgOk2 = document.getElementById("imgOk2");
            var sClave = document.getElementById("hfCambiarPass");
            if (this.value == sClave.value) {
                validaPassAnterior(pImgOk1, 0)
                var protocol = window.location.protocol;
                var url = window.location.host;
                var path = window.location.pathname;
                var parts = path.substr(1).split('/');
                var raiz = parts[0];
                if (pImgOk2.src == protocol + '//' + url + '/' + raiz + "/Imagenes/siSeg.png") {
                }
            }
            else {
                validaPassAnterior(pImgOk1, 1)
            }
            validatosActivabotom();           
        });


        $("#txtNuevoPassword").keyup(function () {
            var valCaracteres = 0;
            $.each(data2, function (i, item) {
                if (item.estado == 'A') {
                    if (item.codigo == '02') {
                        valCaracteres = item.valor;
                    }
                }
            });


            var pNuevoPassword = document.getElementById("txtNuevoPassword");
            var pRepetirPassword = document.getElementById("txtRepetirPassword");
            var pImgOk2 = document.getElementById("imgOk2");
            var pImgOk3 = document.getElementById("imgOk3");

            const clave = this.value;
            //const ochocaracteres = /.{8,}/.test(clave)
            const ochocaracteres = new RegExp(`^.{${valCaracteres},${30}}$`).test(clave);
            const mayuscula = /(?:[A-Z])/.test(clave)
            const minuscula = /(?:[a-z])/.test(clave)
            const numeros = /(?:\d)/.test(clave)
            const noespecial = /[`!@#$%^&*()_\-+=\[\]{};':"\\|,.<>\/?~ ]/.test(clave)

            var cantPolit = data2.length;
            var cantPolitIni = 0;

            $.each(data2, function (i, item) {
                if (item.estado == 'A') {
                    if (item.codigo == '02') {
                        if (ochocaracteres == true) {
                            var plblLongitud = document.getElementById("lbl02");
                            plblLongitud.style.color = "#22B14C";

                            cantPolitIni = cantPolitIni + 1

                            blMinLongitud = true;

                        } else {
                            var plblLongitud = document.getElementById("lbl02");
                            plblLongitud.style.color = "#FF0000";
                            blMinLongitud = false;
                        }
                    } else if (item.codigo == '03') {
                        if (minuscula == true) {
                            var plblMinus = document.getElementById("lbl03");
                            plblMinus.style.color = "#22B14C";


                            cantPolitIni = cantPolitIni + 1

                            blMinLetraMinus = true;
                        } else {
                            var plblMinus = document.getElementById("lbl03");
                            plblMinus.style.color = "#FF0000";
                            blMinLetraMinus = false;
                        }
                    }
                    else if (item.codigo == '04') {
                        if (mayuscula == true) {
                            var plblMayus = document.getElementById("lbl04");
                            plblMayus.style.color = "#22B14C";


                            cantPolitIni = cantPolitIni + 1

                            blMinLetraMayus = true;
                        }
                        else {
                            var plblMayus = document.getElementById("lbl04");
                            plblMayus.style.color = "#FF0000";
                            blMinLetraMayus = false;
                        }
                    } else if (item.codigo == '05') {
                        if (numeros == true) {
                            var plblNumero = document.getElementById("lbl05");
                            plblNumero.style.color = "#22B14C";

                            cantPolitIni = cantPolitIni + 1

                            blMinNumero = true;
                        } else {
                            var plblNumero = document.getElementById("lbl05");
                            plblNumero.style.color = "#FF0000";
                            blMinNumero = false;
                        }
                    } else if (item.codigo == '06')
                    {

                        if (noespecial == true) {
                            var plblEspecial = document.getElementById("lbl06");
                            plblEspecial.style.color = "#22B14C";

                            cantPolitIni = cantPolitIni + 1


                            blMinEspecial = true;
                        }
                        else {
                            var plblEspecial = document.getElementById("lbl06");
                            plblEspecial.style.color = "#FF0000";
                            blMinEspecial = false;
                        }
                    }
                }
            });

            if (cantPolitIni == cantPolit) {
                if (pNuevoPassword.value.trim() != "" && pRepetirPassword.value.trim() == "") {
                    if ((pNuevoPassword.value.trim().length > pRepetirPassword.value.trim().length) && pRepetirPassword.value.trim().length > 0) {
                        validaPassAnterior(pImgOk2, 1)
                    }
                    else {
                        validaPassAnterior(pImgOk2, 0)
                    }

                    var plblIguales = document.getElementById("lblIguales");
                    plblIguales.style.color = "#FF0000";

                    pRepetirPassword.disabled = false;
                }
                else if (pNuevoPassword.value.trim() != pRepetirPassword.value.trim() && pRepetirPassword.value.trim() != "") {
                    if ((pNuevoPassword.value.trim().length > pRepetirPassword.value.trim().length) && pRepetirPassword.value.trim().length > 0) {
                        validaPassAnterior(pImgOk2, 1)
                    }
                    else if ((pNuevoPassword.value.trim().length < pRepetirPassword.value.trim().length) && pRepetirPassword.value.trim().length > 0) {
                        validaPassAnterior(pImgOk2, 1)
                    }
                    else {
                        validaPassAnterior(pImgOk2, 0)
                    }

                    var plblIguales = document.getElementById("lblIguales");
                    plblIguales.style.color = "#FF0000";

                    pRepetirPassword.disabled = false;
                }
                else if (pNuevoPassword.value.trim() == pRepetirPassword.value.trim()) {

                    if (pNuevoPassword.value.length > 0 && pRepetirPassword.value.length > 0) {
                        validaPassAnterior(pImgOk2, 0)
                        validaPassAnterior(pImgOk3, 0)

                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#22B14C";
                        blIguales = true;
                        pRepetirPassword.disabled = false;
                    }
                    else {
                        validaPassAnterior(pImgOk2, 1)
                        validaPassAnterior(pImgOk3, 1)
                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#FF0000";
                    }
                }
                else {
                    var plblIguales = document.getElementById("lblIguales");
                    plblIguales.style.color = "#FF0000";
                }
            }
            else {
                if (pNuevoPassword.value == pRepetirPassword.value) {
                    if (pNuevoPassword.value.length > 0 && pRepetirPassword.value.length > 0) {
                        validaPassAnterior(pImgOk2, 0)
                        validaPassAnterior(pImgOk3, 0)

                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#22B14C";
                        blIguales = true;
                    }
                    else {
                        validaPassAnterior(pImgOk2, 1)
                        validaPassAnterior(pImgOk3, 1)
                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#FF0000";
                        blIguales = false;
                    }
                    //validaPassAnterior(pImgOk2, 0)
                    //var plblIguales = document.getElementById("lblIguales");
                    //plblIguales.style.color = "#22B14C";
                    //blIguales = true;
                }
                else {
                    blIguales = false;
                    var plblIguales = document.getElementById("lblIguales");
                    plblIguales.style.color = "#FF0000";
                }
                validaPassAnterior(pImgOk2, 1)
            }
            validatosActivabotom();

        });


        $("#txtRepetirPassword").keyup(function () {

            var valCaracteres = 0;
            $.each(data2, function (i, item) {
                if (item.estado == 'A') {
                    if (item.codigo == '02') {
                        valCaracteres = item.valor;
                    }
                }
            });

            var pNuevoPassword = document.getElementById("txtNuevoPassword");
            var pRepetirPassword = document.getElementById("txtRepetirPassword");
            var pImgOk2 = document.getElementById("imgOk2");
            var pImgOk3 = document.getElementById("imgOk3");

            const clave = this.value;
            //const ochocaracteres = /.{8,}/.test(clave)
            const ochocaracteres = new RegExp(`^.{${valCaracteres},${30}}$`).test(clave);
            const mayuscula = /(?:[A-Z])/.test(clave)
            const minuscula = /(?:[a-z])/.test(clave)
            const numeros = /(?:\d)/.test(clave)
            const noespecial = /[`!@#$%^&*()_\-+=\[\]{};':"\\|,.<>\/?~ ]/.test(clave)


            var cantPolit = data2.length;
            var cantPolitIni = 0;
            $.each(data2, function (i, item) {
                if (item.estado == 'A') {
                    if (item.codigo == '02') {
                        if (ochocaracteres == true) {
                            blMinLongitud2 = true;
                            cantPolitIni = cantPolitIni + 1
                        } else {
                            blMinLongitud2 = false;
                        }
                    } else if (item.codigo == '03') {
                        if (minuscula == true) {
                            blMinLetraMinus2 = true;
                            cantPolitIni = cantPolitIni + 1
                        } else {
                            blMinLetraMinus2 = false;
                        }
                    } else if (item.codigo == '04') {
                        if (mayuscula == true) {
                            blMinLetraMayus2 = true;
                            cantPolitIni = cantPolitIni + 1
                        } else {
                            blMinLetraMayus2 = false;
                        }
                    } else if (item.codigo == '05') {
                        if (numeros == true) {
                            blMinNumero2 = true;
                            cantPolitIni = cantPolitIni + 1
                        } else {
                            blMinNumero2 = false;
                        }
                    } else if (item.codigo == '06') {
                        if (noespecial == true) {
                            blMinEspecial2 = true;
                            cantPolitIni = cantPolitIni + 1
                        } else {
                            blMinEspecial2 = false;
                        }
                    }
                }
            });

            if (cantPolitIni == cantPolit) {
                if (pNuevoPassword.value == pRepetirPassword.value) {

                    if (pNuevoPassword.value.length > 0 && pRepetirPassword.value.length > 0) {
                        validaPassAnterior(pImgOk3, 0)
                        validaPassAnterior(pImgOk2, 0)

                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#22B14C";
                        blIguales = true;

                    }
                    else {
                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#FF0000";
                        validaPassAnterior(pImgOk3, 2)
                        validaPassAnterior(pImgOk3, 1)
                    }

                } else {

                    blIguales = false;
                    var plblIguales = document.getElementById("lblIguales");
                    plblIguales.style.color = "#FF0000";
                    validaPassAnterior(pImgOk3, 1)

                }
            }
            else {
                validaPassAnterior(pImgOk3, 1)

                if (pNuevoPassword.value == pRepetirPassword.value) {
                    if (pNuevoPassword.value.length > 0 && pRepetirPassword.value.length > 0) {
                        validaPassAnterior(pImgOk2, 0)
                        validaPassAnterior(pImgOk3, 0)

                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#22B14C";
                        blIguales = true;
                    }
                    else {
                        validaPassAnterior(pImgOk2, 1)
                        validaPassAnterior(pImgOk3, 1)
                        var plblIguales = document.getElementById("lblIguales");
                        plblIguales.style.color = "#FF0000";
                        blIguales = false;
                    }
                }
                else {
                    var plblIguales = document.getElementById("lblIguales");
                    plblIguales.style.color = "#FF0000";
                    blIguales = false;
                }
            }

            validatosActivabotom();

        });

        function mostrarTabla() {
            $.ajax({
                type: "POST",
                url: "PopUp/CambiarPassword.aspx/Sp_ParamSeguridad_SelData",
                data: null,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                },
                success: function (data) {
                    data2 = JSON.parse(data.d)
                    let tabla = '<table style="border: 0px solid #8DC73F; width: 100 %; text-align: center; width: 100%; margin-top: -30px;">'
                    tabla += '<tbody>'


                    //tabla += '<tr style="border: 0px solid #8DC73F; border - bottom: 1px solid #8DC73F; height: 45px;">'
                    //tabla += '<td style="border: 0px solid #8DC73F; text - align: center;"></td>'
                    //tabla += '</tr>'


                    $.each(data2, function (i, item) {

                        if (item.estado == 'A') {
                            tabla += '<tr style="border: 0px solid #8DC73F;">'
                            tabla += '<td style="text - align: center; border: 0px solid #8DC73F;">'
                            tabla += '<label id="lbl' + item.codigo + '" style="font-size: 0.8em;color:#FF0000; background-color:white;font-weight: bold;"> ' + item.nombre + ' </label>'
                            tabla += '</td>'
                            tabla += '</tr>'
                        }
                    });


                    tabla += '<tr style="border: 0px solid #8DC73F;">'
                    tabla += '<td style="text - align: center; border: 0px solid #8DC73F;">'
                    tabla += '<label id="lblIguales" style="font-size: 0.8em;color:#FF0000; background-color:white;font-weight: bold;">La contraseñas deben ser iguales.</label>'
                    tabla += '</td>'
                    tabla += '</tr>'


                    tabla += '</tbody>'
                    tabla += '</table>'
                    document.querySelector("#tabla").innerHTML = tabla

                }
            });
        }

        function validatosActivabotom()
        {
            var pButtonCambiarContrasenia = document.getElementById("btn_CambiarContrasenia");
            
            if (validaPassFinal() == 1)
            {
                pButtonCambiarContrasenia.disabled = true;
            }
            else {
                pButtonCambiarContrasenia.disabled = false;
            }          
        }

        function mostrarContrasena(input) {
            input.type = input.type === "password" ? "text" : "password";
           
        }

        // Alternar el ícono del ojo
        function cambiarOjo(pOjo) {
            const eyeNo = "eye_no.png";
            const eyeSi = "eye_si.png";

            if (pOjo.src.includes(eyeNo)) {
                pOjo.src = "../Imagenes/" + eyeSi;
            } else {
                pOjo.src = "../Imagenes/" + eyeNo;
            }
        }

        function etiquetaColor(lblEtiqueta, val) {
            var eqtiqueta = document.getElementById(lblEtiqueta);
            if (val == 1) {
                eqtiqueta.style.color = "#22B14C";
                eqtiqueta = true;
            }
            else {
                eqtiqueta.style.color = "#FF0000";
                eqtiqueta = false;
            }
        }

        function validaPassAnterior(pPassAnt, val)
        {
            var protocol = window.location.protocol;
            var url = window.location.host;
            var path = window.location.pathname;
            var parts = path.substr(1).split('/');
            var raiz = parts[0];

            //if (pOjo.src == protocol + '//' + url + '/' + raiz + "/Imagenes/eye_si.png") {

            if (val == 0) {
                //pPassAnt.src = protocol + '//' + url + '/' + raiz + "/Imagenes/siSeg.png"
                pPassAnt.src = "../Imagenes/siSeg.png"
            }
            else {
                //pPassAnt.src = protocol + '//' + url + '/' + raiz + "/Imagenes/noSeg.png"
                pPassAnt.src = "../Imagenes/noSeg.png"
            }
        }

        function validaPassFinal() {
            var protocol = window.location.protocol;
            var url = window.location.host;
            var path = window.location.pathname;
            var parts = path.substr(1).split('/');
            var raiz = parts[0];

            //if (pOjo.src == protocol + '//' + url + '/' + raiz + "/Imagenes/eye_si.png") {

            var vImagen = 0;

            var valor_srcOk1J = $("#imgOk1").attr("src");
            var valor_srcOk2J = $("#imgOk2").attr("src");
            var valor_srcOk3J = $("#imgOk3").attr("src");

            //if (valor_srcOk1J != protocol + '//' + url + '/' + raiz + "/Imagenes/siSeg.png") { vImagen = 1 }
            //if (valor_srcOk2J != protocol + '//' + url + '/' + raiz + "/Imagenes/siSeg.png") { vImagen = 1 }
            //if (valor_srcOk3J != protocol + '//' + url + '/' + raiz + "/Imagenes/siSeg.png") { vImagen = 1 }

            if (valor_srcOk1J != "../Imagenes/siSeg.png") { vImagen = 1 }
            if (valor_srcOk2J != "../Imagenes/siSeg.png") { vImagen = 1 }
            if (valor_srcOk3J != "../Imagenes/siSeg.png") { vImagen = 1 }


            return vImagen;
        }

        function fn_CambiarContraseña()
        {
            if (validaPassFinal() == 1)
            {
                return false;
            }

            if ($("#txtNuevoPassword").val().trim() == "")
            {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Escribir la nueva contraseña.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }

            if ($("#txtRepetirPassword").val().trim() == "")
            {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Escribir la verficación de la contraseña.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }

            if ($("#txtNuevoPassword").val().trim() != $("#txtRepetirPassword").val().trim()) {
                $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "La Nueva Contraseña y la Contraseña de Verificación no coinciden.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                return;
            }


            $.ajax({
                url: "PopUp/CambiarPassword.aspx/CambiarPassword",
                type: "POST",
                data: JSON.stringify({
                    NuevaPassword: $("#txtNuevoPassword").val(),
                    RepetirPassword: $("#txtRepetirPassword").val(),
                    AnteriorPassword: $("#txtAnteriorPassword").val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (error, abc, def) {
                    //alert(def);
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d != "") {
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", oOB_JSON.d, "ERROR", "Cerrar", "fn_oculta_mensaje()", "frmCambiarPassword");
                } else {
                    document.getElementById("hfCambiarPass").value = $("#txtRepetirPassword").val();
                    $.JMensajePOPUP("Mensaje del Sistema Clínica San Felipe", "Se realizo el cambio correctamente", "OK", "Cerrar", "fn_OcultarCambiarPass()", "frmCambiarPassword");
                    location.reload();
                }
            });

            /*
            If TxtClaveNueva1.Text.Trim = "" Then
                retorna = "Escribir la nueva contraseña"
                Return retorna
            End If

            If TxtClaveNueva2.Text.Trim = "" Then
                retorna = "Escribir la verficación de la contraseña"
                Return retorna
            End If

            If TxtClaveNueva1.Text.Length < 4 Then
                retorna = "La nueva contraseña debe tener como minimo 4 caracteres"
                Return retorna
            End If

            If TxtClaveNueva1.Text.Trim <> TxtClaveNueva2.Text.Trim Then
                retorna = "La Nueva Contraseña y la Contraseña de Verificación no coinciden"
                Return retorna
            End If
            */
        }

        function fn_cierra_sistema() {
            $.ajax({
                url: "InformacionPaciente.aspx/CerrarSesion",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.split(";")[0] != "OK") {
                    //alert(oOB_JSON.d.split(";")[1]);
                } else {
                    //window.location.href = "ConsultaPacienteHospitalizado.aspx" JB - 15/10/2020
                    window.close();
                }
            });

        }

        function fn_OcultarCambiarPass() {
            fn_oculta_mensaje();
            fn_oculta_popup();
        }

    </script>
</head>
<body>
    <form id="frmCambiarPassword" runat="server">
        <input type="hidden" id="hfCambiarPass" name="hfCambiarPass" runat="server" />
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Anterior Contraseña</span>
                </div>
            </div>
            <div class="JCELDA-6">
                <div class="JDIV-CONTROLES">
                    <input type="password" class="JTEXTO-SEGURIDAD" id="txtAnteriorPassword" maxlength="20" />
                </div>
            </div>

            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="image" id="imgOk1" src="../Imagenes/noSeg.png" style="width: 1em; margin-top: 0.3em; height: 1em; pointer-events: none;" />
                </div>
            </div>

            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="image" id="imgEye1" src="../Imagenes/eye_no.png" style="width: 1.7em; height: 1.7em;" />
                </div>
            </div>

        </div>
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Nueva Contraseña</span>
                </div>
            </div>
            <div class="JCELDA-6">
                <div class="JDIV-CONTROLES">
                    <input type="password" class="JTEXTO-SEGURIDAD" id="txtNuevoPassword" maxlength="15"/>
                </div>
            </div>

            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="image" id="imgOk2" src="../Imagenes/siSeg.png" style="width: 1em; margin-top: 0.3em; height: 1em; pointer-events: none;" />
                </div>
            </div>

            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="image" id="imgEye2" src="../Imagenes/eye_no.png" style="width: 1.7em; height: 1.7em;" />
                </div>
            </div>

        </div>
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Repetir Contraseña</span>
                </div>
            </div>
            <div class="JCELDA-6">
                <div class="JDIV-CONTROLES">
                    <input type="password" class="JTEXTO-SEGURIDAD" id="txtRepetirPassword" maxlength="15"/>
                </div>
            </div>

            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="image" id="imgOk3" src="../Imagenes/siSeg.png" style="width: 1em; margin-top: 0.3em; height: 1em; pointer-events: none;" />
                </div>
            </div>

            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="image" id="imgEye3" src="../Imagenes/eye_no.png" style="width: 1.7em; height: 1.7em;" />
                </div>
            </div>

        </div>

        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES" style="text-align:center;" >
                    <asp:Label ID="lblMsjCambioPassword" class="JETIQUETA_2" style="font-size:small;" runat="server"></asp:Label>          
                </div>
            </div>
        </div>

        <div id="tabla">
        </div>

    </form>
</body>
</html>
