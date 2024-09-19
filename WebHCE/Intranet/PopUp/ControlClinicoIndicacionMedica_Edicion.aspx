<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ControlClinicoIndicacionMedica_Edicion.aspx.vb" Inherits="WebHCE.ControlClinicoIndicacionMedica_Edicion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmCCIM_Edicion" runat="server">
        <div class="JFILA">
            <div class="JCELDA-10">
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Nombres y Apellidos</span>
                        </div>
                    </div>
                    <div class="JCELDA-3"> <%--style="border:1px solid Black;"--%>
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosNombreApellido_CCIM_E">SUAREZ ACEVEDO ROWEL</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Edad</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosEdad_CCIM_E">45 AÑOS 10 MESES 21 DIAS</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Diagnostico</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosDiagnostico_CCIM_E">Colera</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Código Atencion:</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosCodigoAtencion_CCIM_E">H1113255</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">H.C</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosHD_CCIM_E">397454</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Fono Contacto</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spFonoContacto_CCIM_E">5532875</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Aseguradora:</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosAseguradora_CCIM_E">Rimac</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Especialidad:</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosEspecialidad_CCIM_E">CARDIOLOGIA</span>
                        </div>
                    </div>
                </div>
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Médico Tratante</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosMedico_CCIM_E">ALEGRE CHANG</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="JCELDA-2">
                <div class="JFILA">
                    <div class="JCELDA-12">
                        <div class="JDIV-CONTROLES">
                            <div style="height:auto; color:Red; font-weight:700; text-align:center; width:100%; border:1px solid Red;">
                                <span id="Span1" runat="server" sp="spDiasHospitalizado">3</span> <span style="color:Red;">Dias Hospitalizados</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES JDIV-GRUPO" style="text-align:center;">
                    CONTROL CLINICO - INDICACIONES MEDICAS - ENFERMERAS
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_3">Indicación Médica Seleccionada</span>
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES">
                    <div class="JSBDIV_TABLA">   
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="JSBTABLA" GridLines="None" >
                            <Columns>
                                <asp:BoundField DataField="Producto" HeaderText="Producto" >
                                    <ItemStyle  CssClass="Producto" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UnidMedida" HeaderText="Unid. Medida" >
                                    <ItemStyle  CssClass="Unid. Medida" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Via" HeaderText="Via" >
                                    <ItemStyle  CssClass="Via" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CadaHora" HeaderText="Cada (hrs)" >
                                    <ItemStyle  CssClass="Cada (hrs)" />
                                </asp:BoundField>                                
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" >
                                    <ItemStyle  CssClass="Cantidad" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Dosis" HeaderText="Dosis" >
                                    <ItemStyle  CssClass="Dosis" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Ingrese hora aplicar medicamento</span>
                </div>
            </div>
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <input type="time" id="txtHora_CCIM_E" class="JTEXTO" />
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <input type="button" id="btnProgramarAplicacionDosis" value="Programar Aplicacion Dosis" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-4">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">Los Horarios a aplicar la dosis son los siguientes:</span>
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2 JESPACIO-IZQ-2" style="border:1px solid Black;">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">06:50:00 PM</span>
                    <span class="JETIQUETA_2">Dosis 1</span>
                </div>
            </div>
            <div class="JCELDA-2" style="border:1px solid Black;">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">02:50:00 AM</span>
                    <span class="JETIQUETA_2">Dosis 2</span>
                </div>
            </div>
            <div class="JCELDA-2" style="border:1px solid Black;">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">10:50:00 AM</span>
                    <span class="JETIQUETA_2">Dosis 3</span>
                </div>
            </div>
            <div class="JCELDA-2" style="border:1px solid Black;">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA_2">06:50:00 PM</span>
                    <span class="JETIQUETA_2">Dosis 3</span>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
