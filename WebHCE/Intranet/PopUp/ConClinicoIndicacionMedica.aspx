<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConClinicoIndicacionMedica.aspx.vb" Inherits="WebHCE.ConClinicoIndicacionMedica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".JIMG-BUSQUEDA").click(function () {                
                //$.JMensajePOPUP("ALERTA", "Click en imagen", "2", "Aceptar;Cancelar", "funcion_acepta;fn_oculta_mensaje", $(document));
            });
        });
    </script>
</head>
<body>    
    <form id="frmConCLinicoIndicacionMedica" runat="server">
        <div class="JFILA">
            <div class="JCELDA-9">
                <div class="JFILA">
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Nombres y Apellidos</span>
                        </div>
                    </div>
                    <div class="JCELDA-3"> <%--style="border:1px solid Black;"--%>
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosNombreApellido_Con">SUAREZ ACEVEDO ROWEL</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Edad</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosEdad_Con">45 AÑOS 10 MESES 21 DIAS</span>
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
                            <span class="JETIQUETA_2" runat="server" id="spDatosDiagnostico_Con">Colera</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Código Atencion:</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosCodigoAtencion_Con">H1113255</span>
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
                            <span class="JETIQUETA_2" runat="server" id="spDatosHD_Con">397454</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Fono Contacto</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spFonoContacto_Con">5532875</span>
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
                            <span class="JETIQUETA_2" runat="server" id="spDatosAseguradora_Con">Rimac</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA">Especialidad:</span>
                        </div>
                    </div>
                    <div class="JCELDA-3">
                        <div class="JDIV-CONTROLES">
                            <span class="JETIQUETA_2" runat="server" id="spDatosEspecialidad_Con">CARDIOLOGIA</span>
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
                            <span class="JETIQUETA_2" runat="server" id="spDatosMedico_Con">ALEGRE CHANG</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="JCELDA-3">
                <div class="JDIV-CONTROLES">
                    <div style="height:auto; color:Red; font-weight:700; text-align:center; width:100%; border:1px solid Red;">
                        <span runat="server" id="spDiasHospitalizado">3</span> <span style="color:Red;">Dias Hospitalizados</span>
                    </div>
                </div>
            </div>
        </div>

        <div class="JFILA">
            <div class="JCELDA-12">
                <div class="JDIV-CONTROLES JDIV-GRUPO" style="text-align:center;">
                    CONTROL CLINICO - INDICACIONES MEDICAS
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Producto:</span>
                </div>
            </div>
            <div class="JCELDA-8">
                <div class="JDIV-CONTROLES">
                    <input type="text" runat="server" id="txtProducto_Con" class="JTEXTO" />
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <img src="../../Imagenes/Buscar.png" id="imgBuscarAnalisisLaboratorio" alt="" class="JIMG-BUSQUEDA" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Via:</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" runat="server" id="txtVia_Con" class="JTEXTO" />
                </div>
            </div>
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Uni. Medida:</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" runat="server" id="txtUniMedida_Con" class="JTEXTO" />
                </div>
            </div> 
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Dosis:</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" runat="server" id="txtDosis_Con" class="JTEXTO" />
                </div>
            </div> 
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Cada (Hrs.)</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" runat="server" id="txtCadaHora_Con" class="JTEXTO" />
                </div>
            </div> 
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Cantidad:</span>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <input type="text" runat="server" id="txtCantidad_Con" class="JTEXTO" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-2">
                <div class="JDIV-CONTROLES">
                    <span class="JETIQUETA">Observacion:</span>
                </div>
            </div>
            <div class="JCELDA-8">
                <div class="JDIV-CONTROLES">
                    <textarea runat="server" rows="3" cols="1" id="txtObservacion_Con" class="JTEXTO"></textarea>
                </div>
            </div>
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <img src="../../Imagenes/Agregar.png" id="imgAgregar_Con" alt="" class="JIMG-BUSQUEDA" />
                </div>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-12">
                <span class="JETIQUETA_3">Indicaciones Selecionadas</span>
            </div>
        </div>
        <div class="JFILA">
            <div class="JCELDA-11">
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
            <div class="JCELDA-1">
                <div class="JDIV-CONTROLES">
                    <img src="../../Imagenes/Enviar_Solicitud.png" id="img1" alt="" class="JIMG-BUSQUEDA" />
                </div>
            </div>
        </div>

        <div class="JFILA">
            <div class="JCELDA-12">
                <div style="border:1px solid Black;width:100%;max-height:200px;overflow-y:auto;"> <%--CONTENEDOR DE EVOLUCION CLINICA--%>
                    <div class="JFILA"> 
                        <div class="JCELDA-5">
                            <span class="JETIQUETA" runat="server" id="spDoctorFecha_CCIM1">ALEGRE CHANG | 27/09/2015 10:35 a.m</span>
                        </div>                                                             
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <span class="JETIQUETA_2" runat="server" id="spDescripcion1_CCIM1">
                                Ibuprofeno 10MG/2ML x 4 AMP
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="spDescripcion2_CCIM1">
                                500mg
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="spDescripcion3_CCIM1">
                                Oral
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="spDescripcion4_CCIM1">
                                3 horas
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="spDescripcion5_CCIM1">
                                3
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="spDescripcion6_CCIM1">
                                Oral
                            </span>
                        </div>
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <span class="JETIQUETA_2" runat="server" id="Span1">
                                Acarbosa Apotex Comp.
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span2">
                                100mg
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span3">
                                Oral
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span4">
                                4 horas
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span5">
                                2
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span6">
                                Oral
                            </span>
                        </div>
                    </div>
                    <div class="JFILA"> 
                        <div class="JCELDA-5">
                            <span class="JETIQUETA" runat="server" id="Span13">ALEGRE CHANG | 20/09/2015 09:35 a.m</span>
                        </div>                                                             
                    </div>
                    <div class="JFILA">
                        <div class="JCELDA-4">
                            <span class="JETIQUETA_2" runat="server" id="Span7">
                                Izitromicina Mabo EFG
                            </span> 
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span8">
                                500mg
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span9">
                                Oral
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span10">
                                8 horas
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span11">
                                2
                            </span>
                        </div>
                        <div class="JCELDA-1">
                            <span class="JETIQUETA_2" runat="server" id="Span12">
                                Oral
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
