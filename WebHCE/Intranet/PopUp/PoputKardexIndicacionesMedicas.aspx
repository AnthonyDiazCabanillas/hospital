<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PoputKardexIndicacionesMedicas.aspx.vb" Inherits="WebHCE.PoputKardexIndicacionesMedicas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>    
      <script type="text/javascript">
          $(document).ready(function () {
              fn_RenderizarTabs();

            var IdTreeview1 = $("#" + "<%=divPedidoTreeview1.ClientID %>").attr("id");
            fn_CreaEventosTreeViewAI(IdTreeview1, "hfRecetaAlta");          
            fn_CrearEventoControlClinico2PopUpPedido();
        });

          function fn_ImprimirPedido() {
              if ($("#frmPedido").find(".JSBTABS .JSBTAB_ACTIVO a").html().trim() == "Receta de Alta") {
                  if ($("#hfRecetaAlta").val().trim() == "") {
                      $.JMensajePOPUP("Validación", "No hay registro seleccionado para imprimir.", "ADVERTENCIA", "Aceptar", "fn_oculta_mensaje()", "frmPedido");
                      return;
                  }
                  window.open("VisorReporte.aspx?OP=RA2&Valor=" + $("#hfRecetaAlta").val().trim());
              }
              if ($("#frmPedido").find(".JSBTABS .JSBTAB_ACTIVO a").html().trim() == "Indicaciones Medicas") {
                

                  var IdReceta = $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-SELECCIONADO > .IdeRecetaCab").val(); 
                if (IdReceta != undefined && IdReceta != null && IdReceta != "") {
                    window.open("VisorReporte.aspx?OP=IM2&Valor=ID;" + IdReceta.toString());

                } else {
                    var FecReceta = $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-SELECCIONADO > .FecRegistro").val(); 
                      window.open("VisorReporte.aspx?OP=IM2&Valor=ID;" + FecReceta.toString());
                  }
              }              
          }

          function fn_CargarControlClinicoIM2PopUpPedido(OrdenEjecutar, FechaMostrar, IdeRecetaCabMostrar, objeto) {
              $.ajax({
                  url: "PopUp/Pedido.aspx/ConsultaControlClinico2PopupPedido",
                  type: "POST",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  data: JSON.stringify({
                      orden: OrdenEjecutar,
                      fec_receta: FechaMostrar,
                      ide_recetacab: IdeRecetaCabMostrar
                  }),
                  error: function (dato1, datos2, dato3) {

                  },
                  success: function (oOB_JSON) {
                      if (oOB_JSON.d != "") {
                          if (OrdenEjecutar == "1") {
                              $("#" + "<%=divPedidoTreeview2.ClientID %>").html("");
                            $("#" + "<%=divPedidoTreeview2.ClientID %>").append(oOB_JSON.d);
                        }
                        if (OrdenEjecutar == "2") {
                            objeto.parent().find(".JTREE3-HORA").html("");
                            objeto.parent().find(".JTREE3-HORA").append(oOB_JSON.d);
                        }
                        if (OrdenEjecutar == "3") {
                            objeto.next().html("");
                            objeto.next().append(oOB_JSON.d);
                        }
                        fn_CrearEventoControlClinico2PopUpPedido();
                    } else {

                    }
                }

            });
          }


          function fn_CrearEventoControlClinico2PopUpPedido() {
              $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-FECHA").unbind("click");
            $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-FECHA").click(function () {
                var oObjeto = $(this);

                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                        oObjeto.addClass("JTREE3-SELECCIONADO");
                        oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                        var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                        var Fecha = oObjeto.find("> input").val();
                        if (CadenaClase.includes("JTREE3-PLUS")) {
                            fn_CargarControlClinicoIM2PopUpPedido("2", Fecha, "0", oObjeto);
                            oObjeto.parent().find(".JTREE3-HORA").css("display", "block");
                        } else {
                            oObjeto.parent().find(".JTREE3-HORA").css("display", "none");
                        }
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });
            $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-HORA").unbind("click");
            $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-HORA").click(function () {
                var oObjeto = $(this);

                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                        oObjeto.addClass("JTREE3-SELECCIONADO");
                        oObjeto.find(" > .JTREE3-SIGNO").toggleClass("JTREE3-PLUS");
                        var CadenaClase = oObjeto.find(" > .JTREE3-SIGNO").attr("class");
                        var IdRecetaCab = oObjeto.find("> input").val();
                        if (CadenaClase.includes("JTREE3-PLUS")) {
                            fn_CargarControlClinicoIM2PopUpPedido("3", "", IdRecetaCab, oObjeto);
                            oObjeto.next().css("display", "block");
                        } else {
                            oObjeto.next().css("display", "none");
                        }
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });
            $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-DETALLE").unbind("click");
            $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-DETALLE").click(function () {
                var oObjeto = $(this);

                $.ajax({ url: "InformacionPaciente.aspx/ValidaSession", type: "POST", contentType: "application/json; charset=utf-8", dataType: "json" }).done(function (oOB_JSON) {
                    if (oOB_JSON.d == "") {
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-FECHA").removeClass("JTREE3-SELECCIONADO");
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JFILA-HORA").removeClass("JTREE3-SELECCIONADO");
                        $("#" + "<%=divPedidoTreeview2.ClientID %>").find(".JTREE3-DETALLE").removeClass("JTREE3-SELECCIONADO");

                        oObjeto.addClass("JTREE3-SELECCIONADO");
                    } else {
                        aValores = oOB_JSON.d.toString().split(";");
                        $.JMensajePOPUP("Aviso", "Su Sesión a expirado.", "ADVERTENCIA", "Cerrar", "fn_ExpiraSession()");
                    }
                });
            });
          }

      </script>
</head>
<body>
    <form id="frmPedido" runat="server">
        <div class="JFILA"> 
            <div class="JCELDA-12">
                <div class="DatosUsuario" id="DatosUsuarioPedido">
                                
                </div>
            </div>
        </div>
        <div class="JCONTENEDOR-TAB8">
            <div class="JSBTABS"> 
                <label for="chkTABS" class="JSBMOSTRAR_TABS"></label>
		        <input type="checkbox" id="chkTABS" class="chkTAB-CHECK" />
                <ul>
                    <li class="JSBTAB_ACTIVO">
                        <a>Receta de Alta</a>
                    </li>
                    <li>
                        <a>Indicaciones Medicas</a>
                    </li>
                </ul>
            </div>
            <div class="JCUERPO">
                <div class="JFILA">
                    <div class="JCELDA">
                        <div style="border:1px solid #4BACFF;width:100%;max-height:300px;overflow-y:auto;height:300px;" class="JTREE" id="divPedidoTreeview1" runat="server">
                            
                        </div>
                    </div>
                </div>
            </div>
            <div class="JCUERPO">
                <div class="JFILA">
                    <div class="JCELDA">
                        <div style="border:1px solid #4BACFF;width:100%;max-height:300px;overflow-y:auto;height:300px;" id="divPedidoTreeview2" runat="server">
                            
                        </div>
                    </div>                
                </div>
            </div>
        </div>
        <input type="hidden" id="hfIndicacionMedica" />
        <input type="hidden" id="hfRecetaAlta" />
    </form>
</body>
</html>
