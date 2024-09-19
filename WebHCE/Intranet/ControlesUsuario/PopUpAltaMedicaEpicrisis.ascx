<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PopUpAltaMedicaEpicrisis.ascx.vb" Inherits="WebHCE.PopUpAltaMedicaEpicrisis" %>

<script type="text/javascript">
    $(document).ready(function () {
        //Utilidad/DatosUsuarioPopUp.aspx
        fn_LimpiarGridDiagnosticoAltaMedica();
        fn_CentrarPopUp2_("divPopUpAltaMedicaEpicrisis");
        $("#btnCargarDatosAltaMedicaEP").click(function () {
            $.ajax({
                url: "InformacionPaciente.aspx/ConsultaPacienteHospitalizadoEP",
                type: "POST",
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.toString().split(";").length > 1) {
                    $("#spDatosNombreApellidoEP").html(oOB_JSON.d.toString().split(";")[0]);
                    $("#spDatosIngresoxEmergenciaEP").html(oOB_JSON.d.toString().split(";")[1]);
                    $("#spNombreMedicoEP").html(oOB_JSON.d.toString().split(";")[2]);

                } else {

                }
            });
        });

        $("#btnCargarDiagnosticoAltaMedicaEP").click(function () {
            $.ajax({
                url: "InformacionPaciente.aspx/ListarDiagnosticosAltaMedicaEP",
                type: "POST",
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.toString().split(";")[0] != "ERROR") {

                    var xmlDoc = $.parseXML(oOB_JSON.d);
                    var xml = $(xmlDoc);
                    var TablaDiagnostico = xml.find("TablaDiagnostico");
                    var row = $("[id*=gvDiagnosticoAltaMedicaEP] tr:last").clone();
                    $("[id*=gvDiagnosticoAltaMedicaEP] tr:gt(0)").remove();

                    $(TablaDiagnostico).each(function () {
                        $("td:nth-child(1)", row).html($(this).find("tipo").text());
                        $("td:nth-child(2)", row).html($(this).find("nombre").text());
                        $("td:nth-child(3)", row).html($(this).find("tipodiagnostico").text());
                        $("td:nth-child(4)", row).html($(this).find("coddiagnostico").text());
                        row.css("display", "table-row");
                        $("[id*=gvDiagnosticoAltaMedicaEP] tbody").append(row);
                        row = $("[id*=gvDiagnosticoAltaMedicaEP] tr:last").clone();

                    });

                } else {

                }
            });
        });


        $("#btnCargarAntecenteAltaMedica").click(function () {
            $.ajax({
                url: "InformacionPaciente.aspx/CargarAntecedentesAltaMedica",
                type: "POST",
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                error: function (dato1, datos2, dato3) {

                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d.toString().split(";")[0] != "ERROR") {
                    var xmlDoc = $.parseXML(oOB_JSON.d);
                    var xml = $(xmlDoc);
                    var TablaAntecedente = xml.find("TablaAntecedentes");
                    $(TablaAntecedente).each(function () {
                        var objeto = $(this);
                        var IdCOntrol = objeto.find("id").text();

                        $("#_" + IdCOntrol).parent().parent().parent().css("display", "inline");

                        if ($("#_" + IdCOntrol).attr("type") == "radio") {
                            if (objeto.find("valor").text() == "1") {
                                $("#_" + IdCOntrol).prop("checked", true);
                            }
                        } else {
                            $("#_" + IdCOntrol).val($(this).find("valor").text());
                        }
                    });
                } else {

                }
            });
        });
        //$("[id*=btnCargarAntecenteAltaMedica]").trigger("click");

        //    data: JSON.stringify({
        //        IdRecetaCab: IdRecetaCab
        //    }),
    });


    function fn_AceptarAltaMedicaEpicrisis() {
        fn_LOAD_VISI(); //JB - NUEVO - 20/07/2020
        $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
            if (oOB_JSON.d == "") {
                $(".JCONTENIDO-POPUP-2").find("input[type='button']").attr("disabled", "disabled");

                var CondicionAlta = "";
                var Necropcia = "0";

                if ($("#rbAliviadoAltaMedica").prop("checked")) {
                    CondicionAlta = "A";
                }
                if ($("#rbCuradoAltaMedica").prop("checked")) {
                    CondicionAlta = "M";
                }
                if ($("#rbFallecidoAltaMedica").prop("checked")) {
                    CondicionAlta = "F";
                }


                if ($("#rbNecropciaAltaMedicaSi").prop("checked")) {
                    Necropcia = "1";
                }
                if ($("#rbNecropciaAltaMedicaNo").prop("checked")) {
                    Necropcia = "0";
                }


                $.ajax({
                    url: "InformacionPaciente.aspx/GuardarAltaMedicaEpicrisis",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        EnfermedadActualAltaMedica: $("#txtEnfermedadActualAltaMedica").val(),
                        pa152: $("#_txt_pa-152").val(),
                        talla155: $("#_txt_talla-155").val(),
                        fc150: $("#_txt_fc-150").val(),
                        fr151: $("#_txt_fr-151").val(),
                        peso154: $("#_txt_peso-154").val(),
                        ExamenFisicoAltaMedica: $("#txtExamenFisicoAltaMedica").val(),
                        ExamenesAuxiliaresAltaMedica: $("#txtExamenesAuxiliaresAltaMedica").val(),
                        EvolucionAltaMedica: $("#txtEvolucionAltaMedica").val(),
                        TratamientoAltaMedica: $("#txtTratamientoAltaMedica").val(),
                        ObservacionesAltaMedica: $("#txtObservacionesAltaMedica").val(),
                        CondicionAlta: CondicionAlta,
                        Necropcia: Necropcia,
                        CodigoDestino: $("#" + "<%=cbDestinoAltaMedica1.ClientID %>").val()
                    }),
                    dataType: "json",
                    error: function (dato1, datos2, dato3) {

                    }
                }).done(function (oOB_JSON) {
                    fn_LOAD_OCUL(); //JB - NUEVO - 20/07/2020
                    if (oOB_JSON.d.toString().split(";")[0] != "ERROR") {
                        $(".JCONTENIDO-POPUP-2").find("input[type='button']").removeAttr("disabled");
                        $.JMensajePOPUP("Aviso", "Se guardaron los datos correctamente", "", "Cerrar", "fn_DadoAltaPaciente()", "");
                    } else {
                        $(".JCONTENIDO-POPUP-2").find("input[type='button']").removeAttr("disabled");
                        $.JMensajePOPUP("Error", oOB_JSON.d.toString().split(";")[1], "", "Cerrar", "fn_oculta_mensaje()", "");
                    }
                });
            } else {                
                $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_oculta_mensaje()");
            }
        });
    }


    function fn_CargarAltaMedica() {
        $.ajax({
            url: "InformacionPaciente.aspx/CargarAltaMedicaEP",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (dato1, datos2, dato3) {

            }
        }).done(function (oOB_JSON) {
            if (oOB_JSON.d.toString().split(";")[0] == "ERROR") {

            } else {
                if (oOB_JSON.d.toString() != "") {
                    $("#txtEnfermedadActualAltaMedica").val(oOB_JSON.d.toString().split("|")[0]);

                    $("#_txt_pa-152").val(oOB_JSON.d.toString().split("|")[1]);
                    $("#_txt_talla-155").val(oOB_JSON.d.toString().split("|")[2]);
                    $("#_txt_fc-150").val(oOB_JSON.d.toString().split("|")[3]);
                    $("#_txt_fr-151").val(oOB_JSON.d.toString().split("|")[4]);
                    $("#_txt_peso-154").val(oOB_JSON.d.toString().split("|")[5]);
                    $("#txtExamenFisicoAltaMedica").val(oOB_JSON.d.toString().split("|")[6]);

                    $("#txtExamenesAuxiliaresAltaMedica").val(oOB_JSON.d.toString().split("|")[7]);

                    $("#txtEvolucionAltaMedica").val(oOB_JSON.d.toString().split("|")[8]);

                    $("#txtTratamientoAltaMedica").val(oOB_JSON.d.toString().split("|")[9]);

                    $("#txtObservacionesAltaMedica").val(oOB_JSON.d.toString().split("|")[10]);


                    if (oOB_JSON.d.toString().split("|")[11] == "ALIVIADO") {
                        $("#rbAliviadoAltaMedica").prop("checked", true);
                    }
                    if (oOB_JSON.d.toString().split("|")[11] == "MEJORADO") {
                        $("#rbCuradoAltaMedica").prop("checked", true);
                    }
                    if (oOB_JSON.d.toString().split("|")[11] == "FALLECIDO") {
                        $("#rbFallecidoAltaMedica").prop("checked", true);
                    }
                    if (oOB_JSON.d.toString().split("|")[11] == "") {
                        if (oOB_JSON.d.toString().split("|")[12] == "1") {
                            $("#rbNecropciaAltaMedicaSi").prop("checked", true);
                        } else {
                            $("#rbNecropciaAltaMedicaNo").prop("checked", true);
                        }
                    }
                }
                /*
                tabla.Rows(0)("dsc_enfermedad_actual").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_presionarterial").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_talla").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_frecuenciacardiaca").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_frecuenciarespiratoria").ToString().Trim().ToUpper() + "|" +

                tabla.Rows(0)("dsc_peso").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_examenfisico").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_examenauxiliar").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_evolucion").ToString().Trim().ToUpper() + "|" +

                tabla.Rows(0)("dsc_tratamiento").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_observacion").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("dsc_altamedica").ToString().Trim().ToUpper() + "|" +
                tabla.Rows(0)("flg_necropsia").ToString().Trim().ToUpper()
                */


                //                var xmlDoc = $.parseXML(oOB_JSON.d);
                //                var xml = $(xmlDoc);
                //                var TablaAntecedente = xml.find("TablaAltaMedicaEP");
                //                $(TablaAntecedente).each(function () {
                //                    var objeto = $(this);
                //                    $("#txtEnfermedadActualAltaMedica").val(objeto.find("dsc_enfermedad_actual").text());

                //                    $("#_txt_pa-152").val(objeto.find("dsc_presionarterial").text());
                //                    $("#_txt_talla-155").val(objeto.find("dsc_talla").text());
                //                    $("#_txt_fc-150").val(objeto.find("dsc_frecuenciacardiaca").text());
                //                    $("#_txt_fr-151").val(objeto.find("dsc_frecuenciarespiratoria").text());
                //                    $("#_txt_peso-154").val(objeto.find("dsc_peso").text());
                //                    $("#txtExamenFisicoAltaMedica").val(objeto.find("dsc_examenfisico").text());

                //                    $("#txtExamenesAuxiliaresAltaMedica").val(objeto.find("dsc_examenauxiliar").text());

                //                    $("#txtEvolucionAltaMedica").val(objeto.find("dsc_evolucion").text());

                //                    $("#txtTratamientoAltaMedica").val(objeto.find("dsc_tratamiento").text());

                //                    $("#txtObservacionesAltaMedica").val(objeto.find("dsc_observacion").text());


                //                    if (objeto.find("dsc_altamedica").text() == "ALIVIADO") {
                //                        $("#rbAliviadoAltaMedica").prop("checked", true);
                //                    }
                //                    if (objeto.find("dsc_altamedica").text() == "MEJORADO") {
                //                        $("#rbCuradoAltaMedica").prop("checked", true);
                //                    }
                //                    if (objeto.find("dsc_altamedica").text() == "FALLECIDO") {
                //                        $("#rbFallecidoAltaMedica").prop("checked", true);
                //                    }
                //                    if (objeto.find("dsc_altamedica").text() == "") {
                //                        if (objeto.find("flg_necropsia").text() == "1") {
                //                            $("#rbNecropciaAltaMedicaSi").prop("checked", true);
                //                        } else {
                //                            $("#rbNecropciaAltaMedicaNo").prop("checked", true);
                //                        }
                //                    }
                //                });
            }
        });

    }

    function fn_CancelaAltaMedicaEpicrisis() {
        $("#divAntecedentesPatologicoSi").find(".JFILA").css("display", "none");
        fn_OcultarPopup2("divPopUpAltaMedicaEpicrisis");




        //LIMPIANDO LOS CAMPOS AL CERRAR POPUP
        $("#EnfermedadActualAltaMedica").val("");
        $("#_txt_pa-152").val("");
        $("#_txt_talla-155").val("");
        $("#_txt_fc-150").val("");
        $("#_txt_fr-151").val("");
        $("#_txt_peso-154").val("");
        $("#txtExamenFisicoAltaMedica").val("");
        $("#txtExamenesAuxiliaresAltaMedica").val("");
        $("#txtEvolucionAltaMedica").val("");
        $("#txtTratamientoAltaMedica").val("");
        $("#txtObservacionesAltaMedica").val();
        $("#rbAliviadoAltaMedica").prop("checked", true)

        
    }


    function fn_LimpiarGridDiagnosticoAltaMedica() {
        var row = $("[id*=gvDiagnosticoAltaMedicaEP] tr:last").clone();
        $("[id*=gvDiagnosticoAltaMedicaEP] tr:gt(0)").remove();
        $("td:nth-child(1)", row).html("");
        $("td:nth-child(2)", row).html("");
        $("td:nth-child(3)", row).html("");
        $("td:nth-child(4)", row).html("");
        row.css("display", "none");
        $("[id*=gvDiagnosticoAltaMedicaEP] tbody").append(row);
    }

    //Cmendez 16/05/2022 
    //Act
    $(".JTEXTO").keypress(function (e) {
        var ValidaTilde = /[|<>'&]/;
        if (ValidaTilde.test(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });

    $(".JTEXTO").blur(function () {
        for (var i = 0; i < this.value.length; i++) {
            if (this.value.includes('|')) {
                $(this).val($(this).val().replace("|", ""));
                i--;
            }
            if (this.value.includes("'")) {
                $(this).val($(this).val().replace("'", ""));
                i--;
            }
            if (this.value.includes("<")) {
                $(this).val($(this).val().replace("<", ""));
                i--;
            }
            if (this.value.includes(">")) {
                $(this).val($(this).val().replace(">", ""));
                i--;
            }
            if (this.value.includes("&")) {
                $(this).val($(this).val().replace("&", ""));
                i--;
            }
        }
    });
    /*----------------------------------------------------------------------------------*/
    //$(".JDECIMAL-10").blur(function () {
    //    var ValorDecimal = /^[0-9]+(.[0-9]+)?$/;
    //    if (!ValorDecimal.test($(this).val())) {
    //        $(this).val("");
    //    }
    //});

    ////CAMPO NUMERICO
    //$(".JNUMERO").keypress(function (event) {
    //    var ValidacionNumerica = /[0-9]/;
    //    if (!ValidacionNumerica.test(String.fromCharCode(event.which))) {
    //        event.preventDefault();
    //    }
    //});

</script>

<input type="button" id="btnCargarDatosAltaMedicaEP" style="display:none" />
<input type="button" id="btnCargarDiagnosticoAltaMedicaEP" style="display:none" />
<input type="button" id="btnCargarAntecenteAltaMedica" style="display:none" />
<div class="DatosUsuario">
    <div class="JFILA"> 
        <div class="JCELDA-12">
            <div class="JFILA">
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Paciente :</span>
                    </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" id="spDatosNombreApellidoEP"></span>
                    </div>
                </div>
                <div class="JCELDA-1">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Médico :</span> </div>
                </div>
                <div class="JCELDA-3">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" id="spNombreMedicoEP"></span>
                    </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA">Ingreso x Emergencia :</span> </div>
                </div>
                <div class="JCELDA-2">
                    <div class="JDIV-CONTROLES">
                        <span class="JETIQUETA_2" id="spDatosIngresoxEmergenciaEP"></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="JFILA" id="divAntecedentesPatologicoSi">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">ANTECEDENTES PATOLOGICOS</span>
        </div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">RAM</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptRAM_SI-107" type="radio"  name="_112" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptRAM_NO-108" type="radio"  name="_112" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-8"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtRAM-109" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Transfusiones</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptTransfusiones_SI-111" type="radio"  name="_113" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptTransfusiones_NO-112" type="radio"  name="_113" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-8"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtTransfusiones-113" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Hipertension Arterial</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptHipertensionArteficial_SI-115" type="radio"  name="_114" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptHipertensionArteficial_NO-116" type="radio"  name="_114" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaHipertensionArterial-413" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtHipertensionArteficial-117" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Diabetes mellitus</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptDiabetesMellitus_SI-119" type="radio"  name="_115" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptDiabetesMellitus_NO-120" type="radio"  name="_115" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaDiabetesMellitus-414" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtDiabetesMellitus-121" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Asma</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptAsma_SI-123" type="radio"  name="_116" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptAsma_NO-124" type="radio"  name="_116" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaAsma-416" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtAsma-125" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Infección VSR</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_SiInfeccionVSR-600" type="radio"  name="_818" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_NoInfeccionVSR-601" type="radio"  name="_818" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_FechaInfeccionVSR-602" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_InfeccionVSR-603" type="text"  class="JTEXTO-12" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Enf. Cardiaca</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptEnfCardiaca_SI-127" type="radio"  name="_117" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptEnfCardiaca_NO-128" type="radio"  name="_117" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaEnfCardiaca-417" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtEnfCardiaca-129" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Enf. Renal</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptEnfRenal_SI-131" type="radio"  name="_118" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptEnfRenal_NO-132" type="radio"  name="_118" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaEnfRenal-418" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtEnfRenal-133" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Gastritis / Ulcera</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptGastritisUlceras_SI-135" type="radio"  name="_119" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptGastritisUlceras_NO-136" type="radio"  name="_119" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaGastritisUlcera-419" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtGastritisUlceras-137" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Viajes 3m</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptViajesm_SI-139" type="radio"  name="_120" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptViajesm_NO-140" type="radio"  name="_120" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaViajes3m-420" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtViajesm-141" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Fiebre Malta</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptFiebreMalta_SI-143" type="radio"  name="_121" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptFiebreMalta_NO-144" type="radio"  name="_121" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaFiebreMalta-421" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtFiebreMalta-145" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Hepatitis Viral</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptHepatitisViral_SI-147" type="radio"  name="_122" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptHepatitisViral_NO-148" type="radio"  name="_122" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaHepatitisViral-422" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtHepatitisViral-149" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Tuberculosis</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptTubercolisis_SI-151" type="radio"  name="_123" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptTubercolisis_NO-152" type="radio"  name="_123" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaTuberculosis-423" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtTubercolisis-153" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Enf. Tiroidea</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptEnfTiroidea_SI-155" type="radio"  name="_124" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_OptEnfTiroidea_NO-156" type="radio"  name="_124" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_txt_FechaEnfTiroideas-424" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="50" id="_TxtEnfTiroidea-157" type="text"  class="JTEXTO" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Neoplasia</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_SiNeoplasia-425" type="radio"  name="_691" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_NoNeoplasia-426" type="radio"  name="_691" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_FechaNeoplasia-427" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_Neoplasia-428" type="text"  class="JTEXTO-12" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Demencia</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_SiDemencia-429" type="radio"  name="_692" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_NoDemencia-430" type="radio"  name="_692" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_FechaDemencia-431" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_Demencia-432" type="text"  class="JTEXTO-12" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Enf. Mentales</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_SiEnfMentales-433" type="radio"  name="_693" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_NoEnfMentales-434" type="radio"  name="_693" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_FechaEnfMentales-435" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_EnfMentalesPatologico-436" type="text"  class="JTEXTO-12" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Osteoporosis</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_SiOsteoporosis-437" type="radio"  name="_694" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_NoOsteoporosis-438" type="radio"  name="_694" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_FechaOsteoporosis-439" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_OsteoporosisPatologico-440" type="text"  class="JTEXTO-12" value="" /> </div></div>
    </div>
    <div class="JFILA" style="display:none;">
        <div class="JCELDA-2"><div class="JDIV-CONTROLES"><span class="JETIQUETA">Polifarmacia</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_SiPolifarmacia-441" type="radio"  name="_695" /><span class="JETIQUETA_2">SI</span></div></div>
        <div class="JCELDA-1"><div class="JDIV-CONTROLES"><input disabled="disabled" id="_Opt_NoPolifarmacia-442" type="radio"  name="_695" /><span class="JETIQUETA_2">NO</span></div></div>
        <div class="JCELDA-2" style="display:none;"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_FechaPolifarmacia-443" type="text"  class="JFECHA hasDatepicker" value="" /> </div></div>
        <div class="JCELDA-6"><div class="JDIV-CONTROLES"><input disabled="disabled" maxlength="100" id="_txt_PolifarmaciaPatologico-444" type="text"  class="JTEXTO-12" value="" /> </div></div>
    </div>
</div>


<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">ENFERMEDAD ACTUAL</span>
        </div>
    </div>
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <textarea id="txtEnfermedadActualAltaMedica" rows="5" cols="1" class="JTEXTO" maxlength="5000"></textarea>
        </div>
    </div>
</div>



<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">EXAMEN FISICO</span>
        </div>
    </div>    
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA">PA</span>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <input maxlength="8" id="_txt_pa-152" type="text" class="JTEXTO-10" />
            <%--<span class="JETIQUETA_4">mmHg</span>--%>
        </div>
    </div>

    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA">T</span>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <input onblur="fn_ValidaControl(0,250,'Rango&nbsp;permitido&nbsp;entre&nbsp;15&nbsp;-&nbsp;250','_txt_talla-155');" maxlength="6" id="_txt_talla-155" type="text" class="JDECIMAL-10" />
            <%--<span class="JETIQUETA_4">Cms</span>--%>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA">FC</span>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <input onblur="fn_ValidaControl(0,200,'Valor&nbsp;no&nbsp;puede&nbsp;exceder&nbsp;a&nbsp;200.','_txt_fc-150');" maxlength="8" id="_txt_fc-150" type="text" class="JDECIMAL-10" />
            <%--<span class="JETIQUETA_4">pm</span>--%>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA">FR</span>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <input onblur="fn_ValidaControl(0,100,'Valor&nbsp;no&nbsp;puede&nbsp;exceder&nbsp;a&nbsp;100.','_txt_fr-151');" maxlength="8" id="_txt_fr-151" type="text" class="JDECIMAL-10" />
            <%--<span class="JETIQUETA_4">pm</span>--%>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA">Peso</span>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <input onblur="fn_ValidaControl(0,250,'Valor&nbsp;no&nbsp;puede&nbsp;exceder&nbsp;a&nbsp;250.','_txt_peso-154');" maxlength="6" id="_txt_peso-154" type="text" class="JDECIMAL-10" />
            <%--<span class="JETIQUETA_4">Kg</span>--%>
        </div>
    </div>
    
    
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <textarea id="txtExamenFisicoAltaMedica" rows="5" cols="1" class="JTEXTO" maxlength="5000"></textarea>
        </div>
    </div>
</div>


<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">EXAMENES AUXILIARES</span>
        </div>
    </div>
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <textarea id="txtExamenesAuxiliaresAltaMedica" rows="5" cols="1" class="JTEXTO" maxlength="5000"></textarea>
        </div>
    </div>
</div>



<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">EVOLUCION</span>
        </div>
    </div>
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <textarea id="txtEvolucionAltaMedica" rows="5" cols="1" class="JTEXTO" maxlength="5000"></textarea>
        </div>
    </div>
</div>



<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">TRATAMIENTO</span>
        </div>
    </div>
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <textarea id="txtTratamientoAltaMedica" rows="5" cols="1" class="JTEXTO" maxlength="5000"></textarea>
        </div>
    </div>
</div>



<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">DIAGNOSTICO</span>
        </div>
    </div>
     <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <asp:GridView ID="gvDiagnosticoAltaMedicaEP" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                GridLines="None" AllowPaging="false" PageSize="25" emptydatatext="No hay diagnosticos." >
                <Columns>
                    <asp:BoundField DataField="tipo" HeaderText="Tipo">
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombre" HeaderText="Diagnóstico">
                        <ItemStyle HorizontalAlign="Left" Width="65%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="tipodiagnostico" HeaderText="Tipo de Diagnóstico">
                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="coddiagnostico" HeaderText="CIE-10">
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle CssClass="JPAGINADO" />                                                                                 
            </asp:GridView>
        </div>
    </div>
</div>



<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">OBSERVACIONES</span>
        </div>
    </div>
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <textarea id="txtObservacionesAltaMedica" rows="5" cols="1" class="JTEXTO" maxlength="5000"></textarea>
        </div>
    </div>
</div>



<div class="JFILA">
    <div class="JCELDA-12">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_3">CONDICIONES DE ALTA</span>
        </div>
    </div>
</div>
<div class="JFILA">
    <div class="JCELDA-2">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_2">Aliviado: </span><input id="rbAliviadoAltaMedica" type="radio" name="rbCondicionAlta" checked="checked" />
        </div>
    </div>
    <div class="JCELDA-2">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_2">Curado: </span><input id="rbCuradoAltaMedica" type="radio" name="rbCondicionAlta" />
        </div>
    </div>
    <div class="JCELDA-2">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_2">Fallecido: </span><input id="rbFallecidoAltaMedica" type="radio" name="rbCondicionAlta" />
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_2">Necropsia:</span>
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_2">Si</span><input id="rbNecropciaAltaMedicaSi" type="radio" name="rbCondicionAlta" /> <%--rbNecropciaAltaMedica--%>   
        </div>
    </div>
    <div class="JCELDA-1">
        <div class="JDIV-CONTROLES">
            <span class="JETIQUETA_2">No</span><input id="rbNecropciaAltaMedicaNo" type="radio" name="rbCondicionAlta" />      
        </div>
    </div>
</div>
<div class="JFILA">
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_2">Destino :</span>
            </div>
        </div>
        <div class="JCELDA-3">
            <div class="JDIV-CONTROLES">
                <asp:DropDownList runat="server" CssClass="JSELECT" ID="cbDestinoAltaMedica1"></asp:DropDownList>
            </div>
        </div>
</div>