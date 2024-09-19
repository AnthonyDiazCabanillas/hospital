<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PetitorioLab.aspx.vb" Inherits="WebHCE.PetitorioLab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            fn_CampoBase();
            $("#" + "<%=chkProgramarHoraLab.ClientID %>").click(function () {
                //INICIO - 26/01/2017
                var sCheckMarcado = "";
                var sFechaSele = "";
                var sHoraSele = "";
                var sAnalisisMarcado = "";
                //FIN - 26/01/2017

                if ($(this).prop("checked")) {
                    $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").removeAttr("disabled");
                    $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").removeAttr("disabled");                   

                    $(".JSBDIV_TABLA").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                        $(this).removeAttr("disabled");
                        $(this).prop("checked", true);
                        sAnalisisMarcado += $(this).parent().parent().find(".ide_analisis").html() + ";"; //INICIO - 26/01/2017
                    });

                    //INICIO - 26/01/2017
                    var sNumeroPagina = 1;
                    sFechaSele = $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val();
                    sHoraSele = $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val();
                    fn_LOAD_POPU_VISI();
                    $(".JCUERPO-POPUP").load("PopUp/PetitorioLab.aspx", {
                        Pagina: sNumeroPagina,
                        CheckMarcado: "SI",
                        Fecha: sFechaSele,
                        Hora: sHoraSele,
                        AnalisisMarcado: sAnalisisMarcado,
                        HiddenCodigosPetitorio: $("#" + "<%=hfCodigosPetitorioLab.ClientID %>").val(),
                        HiddenDescripcion: $("#" + "<%=hfDescripcionPetitorioLab.ClientID %>").val()
                    }, function () {
                        fn_LOAD_GRID_OCUL();
                    });
                    //FIN - 26/01/2017

                } else {
                    $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").attr("disabled", "disabled");
                    $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").attr("disabled", "disabled");
                    $(".JSBDIV_TABLA").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                        $(this).attr("disabled", "disabled");
                        $(this).prop("checked", false);
                    });

                    //INICIO - 26/01/2017
                    var sNumeroPagina = 1;
                    sFechaSele = $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val();
                    sHoraSele = $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val();
                    fn_LOAD_POPU_VISI();
                    $(".JCUERPO-POPUP").load("PopUp/PetitorioLab.aspx", {
                        Pagina: sNumeroPagina,
                        CheckMarcado: "NO",
                        Fecha: sFechaSele,
                        Hora: sHoraSele,
                        AnalisisMarcado: "",
                        HiddenCodigosPetitorio: $("#" + "<%=hfCodigosPetitorioLab.ClientID %>").val(),
                        HiddenDescripcion: $("#" + "<%=hfDescripcionPetitorioLab.ClientID %>").val()
                    }, function () {
                        fn_LOAD_GRID_OCUL();
                    });
                    //FIN - 26/01/2017
                }
            });

            $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").blur(function () {
                if ($(this).val().trim() != "") {
                    var fecha_actual = new Date();
                    var fecha1;
                    if ($("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val().trim() == "") {
                        fecha1 = new Date($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[2], (parseInt($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[1]) - 1), $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[0]);
                        fecha_actual = new Date(fecha_actual.getFullYear(), fecha_actual.getMonth(), fecha_actual.getDate());

                        if (fecha1 < fecha_actual) {
                            $.JMensajePOPUP("Aviso", "La fecha a programar no debe ser menor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "");
                            $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val("");
                        }
                    } else {
                        fecha1 = new Date($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[2], (parseInt($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[1]) - 1), $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[0], $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val().split(":")[0], $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val().split(":")[1]);
                        if (fecha1 < fecha_actual) {
                            $.JMensajePOPUP("Aviso", "La fecha y hora a programar no debe ser menor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "");
                            $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val("");
                            $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val("");
                        }
                    }
                }
            });

            $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").blur(function () {
                if ($(this).val().trim() != "") {
                    var fecha_actual = new Date();
                    var fecha1;
                    if ($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().trim() != "") {
                        fecha1 = new Date($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[2], (parseInt($("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[1]) - 1), $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val().split("/")[0], $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val().split(":")[0], $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val().split(":")[1]);
                        if (fecha1 < fecha_actual) {
                            $.JMensajePOPUP("Aviso", "La fecha y hora a programar no debe ser menor a la fecha actual", "", "Cerrar", "fn_oculta_mensaje()", "");
                            $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val("");
                            $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val("");
                        }
                    }
                }
            });

            //******************************************
            $("#frmPetitorioLab").find(".JSBTABLA").find("tr:last").find("a").click(function (event) {
                event.preventDefault();
                var sNumeroPagina = $(this).prop("href").substring(($(this).prop("href").indexOf("$") + 1), $(this).prop("href").lastIndexOf("'"));
                var sCheckMarcado = "";
                var sFechaSele = "";
                var sHoraSele = "";
                var sAnalisisMarcado = "";
                if ($("#" + "<%=chkProgramarHoraLab.ClientID %>").prop("checked")) {
                    sCheckMarcado = "SI";
                }
                sFechaSele = $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val();
                sHoraSele = $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val();

                $("#frmPetitorioLab").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                    var objetoc = $(this);
                    if ($(this).prop("checked")) {
                        sAnalisisMarcado += objetoc.parent().parent().find(".ide_analisis").html() + ";";
                    }
                });

                fn_LOAD_POPU_VISI();
                $(".JCUERPO-POPUP").load("PopUp/PetitorioLab.aspx", {
                    Pagina: sNumeroPagina,
                    CheckMarcado: sCheckMarcado,
                    Fecha: sFechaSele,
                    Hora: sHoraSele,
                    AnalisisMarcado: sAnalisisMarcado,
                    HiddenCodigosPetitorio: $("#" + "<%=hfCodigosPetitorioLab.ClientID %>").val(),
                    HiddenDescripcion: $("#" + "<%=hfDescripcionPetitorioLab.ClientID %>").val()
                }, function () {
                    fn_LOAD_GRID_OCUL();
                });
            });

            $("#frmPetitorioLab").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").click(function () {
                var objetoc = $(this);
                if ($(this).prop("checked") == false) {
                    $.ajax({
                        url: "Popup/PetitorioLab.aspx/EliminarCheckSession",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            CodigoDesmarcado: objetoc.parent().parent().find(".ide_analisis").html()
                        }),
                        dataType: "json",
                        error: function (dato1, datos2, dato3) {
                        }
                    }).done(function (oOB_JSON) {
                        if (oOB_JSON.d == "OK") {
                        }
                    });
                }
            });
        });

        function fn_cerrar_petitoriolab() {
            $.ajax({
                url: "Popup/PetitorioLab.aspx/CerrarPetitorioLab",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                if (oOB_JSON.d == "OK") {
                    fn_oculta_popup();
                }
            });
        }

        function fn_AceptarPetitorioLab() {
            fn_LOAD_VISI();

            var Fecha = "";
            var Hora = "";
            if ($("#" + "<%=chkProgramarHoraLab.ClientID %>").prop("checked") == true) {
                Fecha = $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val();
                Hora = $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val();

                if (Fecha == "" || Hora == "") {
                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", "Debe ingresar una Fecha y Hora", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    fn_LOAD_OCUL();
                    return;
                }
            }
            //validando debe marcar un analisis marcado para programar
            if ($("#" + "<%=chkProgramarHoraLab.ClientID %>").prop("checked") == true) {
                var ValidaCheckLab = false;
                $(".JSBDIV_TABLA").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                    if ($(this).prop("checked")) {
                        ValidaCheckLab = true;
                    }
                });
                if (ValidaCheckLab == false) {
                    $.JMensajePOPUP("Mensaje del Sistema Clinica San Felipe - Error", "Debe marcar un analisis para programar la hora", "ERROR", "Cerrar", "fn_oculta_mensaje()");
                    fn_LOAD_OCUL();
                    return;
                }
            }

            var CheckActivo = ""
            $(".JSBDIV_TABLA").find(".JSBTABLA .HoraProg").find("input[type='checkbox']").each(function () {
                var objetoc = $(this);
                if ($(this).prop("checked")) {
                    CheckActivo += objetoc.parent().parent().find(".ide_analisis").html() + ";";
                }
            });            
            $.ajax({
                url: "PetitorioLaboratorio.aspx/EnviarSolicitudPetitorio",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    CodigosPetitorioLaboratorio: $("#" + "<%=hfCodigosPetitorioLab.ClientID %>").val(),
                    Descripcion: $("#" + "<%=hfDescripcionPetitorioLab.ClientID %>").val(),
                    Fecha: $("#" + "<%=txtFechaProgramarHoraLab.ClientID %>").val(),
                    Hora: $("#" + "<%=txtHoraProgramarHoraLab.ClientID %>").val(),
                    CodigoMarcado: CheckActivo
                }),
                dataType: "json",
                error: function (dato1, datos2, dato3) {
                }
            }).done(function (oOB_JSON) {
                fn_LOAD_OCUL();
                if (oOB_JSON.d != "OK") {
                    $.JMensajePOPUP("Error", oOB_JSON.d, "ERROR", "Aceptar", "fn_oculta_mensaje()");
                } else {
                    $.JMensajePOPUP("Exito", "Se Envió la solicitud de los análisis de laboratorio seleccionados.", "OK", "Aceptar", "fn_OCultaMensajeLimpiaChech()");
                }
            });
            fn_oculta_popup();
        }
    </script>
</head>
<body>
    <form id="frmPetitorioLab" runat="server" class="JFORM-CONTENEDOR-POPUP">
    <div class="JFILA">
        <div class="JCELDA-12">
            <div class="JDIV-CONTROLES">
                <div class="JSBDIV_TABLA">
                    <asp:GridView ID="gvPetitorioLaboratorio" runat="server" 
                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" 
                            GridLines="None" AllowPaging="True" PageSize="100" emptydatatext="No hay analisis agregados." >
                        <Columns>
                            <%--<asp:BoundField DataField="ide_recetadet" HeaderText="Codigo">
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="dsc_analisis" HeaderText="Analisis">
                                <ItemStyle HorizontalAlign="Left" Width="65%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ide_analisis" HeaderText="ide_analisis">
                                    <HeaderStyle CssClass="JCOL-OCULTA" />
                                    <ItemStyle CssClass="JCOL-OCULTA ide_analisis" />
                                </asp:BoundField>
                            <asp:TemplateField HeaderText="Hora Prog.">
                                <ItemTemplate>                                    
                                    <%--<input type="checkbox" id="chkDefinitivo" runat="server" />--%>                                 
                                    <input type="checkbox" id="chkHoraProg" runat="server" disabled="disabled" />
                                </ItemTemplate>                                                                
                                <ItemStyle CssClass="HoraProg" Width="10%"/>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="JPAGINADO" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="JFILA"> 
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <span class="JETIQUETA_3">Programar Hora</span>
            </div>
        </div>
        <div class="JCELDA-1">
            <div class="JDIV-CONTROLES">
                <input type="checkbox" id="chkProgramarHoraLab" runat="server" />
            </div>
        </div>
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <input type="text" id="txtFechaProgramarHoraLab" runat="server" disabled="disabled" class="JFECHA" />
            </div>
        </div>
        <div class="JCELDA-2">
            <div class="JDIV-CONTROLES">
                <input type="text" id="txtHoraProgramarHoraLab" runat="server" disabled="disabled" class="JHORA" />
            </div>
        </div>
    </div>
    <input type="hidden" runat="server" id="hfDescripcionPetitorioLab" />
    <input type="hidden" runat="server" id="hfCodigosPetitorioLab" />
    </form>
</body>
</html>
