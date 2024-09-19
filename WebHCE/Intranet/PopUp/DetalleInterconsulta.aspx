<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleInterconsulta.aspx.vb" Inherits="WebHCE.DetalleInterconsulta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>
</head>
<body>
    <form id="frmDetalleInter" runat="server">
    <div class="JFILA">
        <div class="JCELDA-12">
            <fieldset>
                <legend>Solicita</legend>
                <div class="JDIV-CONTROLES">
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Motivo:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <asp:DropDownList runat="server" ID="ddlMotivoR" CssClass="JSELECT" Enabled="false"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Especialidad:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtEspecialidadInterconsultaR" class="JTEXTO" disabled="disabled" />
                                <input type="hidden" id="hfCodEspecialidadInterconsultaR" runat="server" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <%--<div class="JDIV-CONTROLES"> 
                                <img src="../Imagenes/Buscar.png" id="imgBusquedaEspecialidadInterconsultaR" alt="" class="JIMG-BUSQUEDA" style="opacity:0.6" />
                            </div>--%>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Medico que solicita:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtMedicoInterconsultaR" class="JTEXTO" disabled="disabled" />
                                <input type="hidden" runat="server" id="txtCodMedicoInterconsultaR" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <%--<div class="JDIV-CONTROLES">
                                <img src="../Imagenes/Buscar.png" id="imgBusquedaMedicoInterconsultaR" alt="" class="JIMG-BUSQUEDA" style="opacity:0.6" />
                            </div>--%>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <div class="JDIV-CONTROLES">
                                <textarea rows="5" cols="1" runat="server" id="txtDescripcionInterconsultaR" class="JTEXTO" disabled="disabled"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Respuesta</legend>
                <div class="JDIV-CONTROLES">
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Especialidad:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtEspecialidadInterconsulta2R" class="JTEXTO" disabled="disabled" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <div class="JDIV-CONTROLES">
                                <%--<img src="../Imagenes/Buscar.png" id="img1" alt="" class="JIMG-BUSQUEDA" />--%>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <div class="JDIV-CONTROLES">
                                <span class="JETIQUETA_2">Medico Interconsulta:</span>
                            </div>                                                
                        </div>
                        <div class="JCELDA-7">
                            <div class="JDIV-CONTROLES">
                                <input type="text" runat="server" id="txtMedicoInterconsulta2R" class="JTEXTO" disabled="disabled" />
                            </div>                                                
                        </div>
                        <div class="JCELDA-1">
                            <div class="JDIV-CONTROLES">
                                <%--<img src="../Imagenes/Buscar.png" id="img2" alt="" class="JIMG-BUSQUEDA" />--%>
                            </div>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-12">
                            <div class="JDIV-CONTROLES">
                                <textarea rows="5" cols="1" runat="server" id="txtDescripcionInterconsulta2R" class="JTEXTO" disabled="disabled"></textarea>
                            </div>
                        </div>
                    </div>
                    <%--<div class="JFILA">
                        <div class="JCELDA-12">
                            <div class="JDIV-CONTROLES">
                                <input type="button" value="Enviar" id="btnResponderInterconsultaR" />
                            </div>
                        </div>
                    </div>--%>
                </div>
            </fieldset>
        </div>
    </div>
    <input type="hidden" runat="server" id="hfVariableDesa" />
    </form>
</body>
</html>
