Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN

Public Class Pedido
    Inherits System.Web.UI.Page
    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            divPedidoTreeview1.InnerHtml = ConsultaTab1_()
            divPedidoTreeview2.InnerHtml = ConsultaControlClinico2_("1", "", "0")
        End If

    End Sub

    Public Function ConsultaTab2_() As String
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdRecetaDet = 0
        oRceRecetaMedicamentoE.Orden = 10 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
        Dim tabla_cabecera As New DataTable()
        tabla_cabecera = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        Dim CadenaEstructuraCC As String = ""

        CadenaEstructuraCC += "<ul>"
        If tabla_cabecera.Rows.Count > 0 Then
            For index_cabecera = 0 To tabla_cabecera.Rows.Count - 1
                CadenaEstructuraCC += "<li>"
                CadenaEstructuraCC += "<a><img alt='' src='../Imagenes/Pastilla.png' />" + tabla_cabecera.Rows(index_cabecera)("nmedico").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim() + "</a><input type='hidden' value='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "' />"
                oRceRecetaMedicamentoE.IdRecetaDet = CType(tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString(), Integer)
                oRceRecetaMedicamentoE.Orden = 2
                Dim tabla_detalle As New DataTable()
                tabla_detalle = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
                If tabla_detalle.Rows.Count > 0 Then
                    CadenaEstructuraCC += "<ul>"
                    For index_detalle = 0 To tabla_detalle.Rows.Count - 1
                        CadenaEstructuraCC += "<li>"
                        'CadenaEstructuraCC += "<a><span class='JETIQUETA_2'>" + tabla_detalle.Rows(index_detalle)("nproducto").ToString().Trim() + "  UNIDADMEDIDA  " + "  VIA  " + _
                        '    tabla_detalle.Rows(index_detalle)("num_frecuencia").ToString().Trim() + " horas " + tabla_detalle.Rows(index_detalle)("num_cantidad").ToString().Trim() + "</span></a>"  'Ibuprofeno 10MG/2ML x 4 AMP  500mg  Oral  3 horas  3  Oral
                        'ide_receta ide_medicamentorec cod_producto
                        CadenaEstructuraCC += "<a><a>" + tabla_detalle.Rows(index_detalle)("nproducto").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;'>UNIDADMEDIDA</a>" + "<a style='margin-left:20px;'>VIA</a>" + _
                                "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("num_frecuencia").ToString().Trim() + "</a>" + " <span class='JETIQUETA_2' style='font-size:1em'>horas</span>" + _
                                "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("num_cantidad").ToString().Trim() + "</a></a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("cod_producto").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("num_dosis").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("txt_detalle").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("num_duracion").ToString().Trim() + "</a>"

                        CadenaEstructuraCC += "</li>"
                    Next
                    CadenaEstructuraCC += "</ul>"
                End If

                CadenaEstructuraCC += "</li>"
            Next
        End If
        CadenaEstructuraCC += "</ul>"

        'REGRESO LA ESTRUCTURA DEL TREEVIEW
        Return CadenaEstructuraCC

    End Function

    Public Function ConsultaTab1_() As String
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdRecetaDet = 0
        oRceRecetaMedicamentoE.Orden = 13  '16/01/2020 - JB - SE CAMBIA ORDEN DE 3 A 13. EL ORDEN 3 SOLO MUESTRA LOS QUE ESTAN EN ESTADO EST_ESTADO = 'G'
        Dim tabla_cabecera As New DataTable()
        tabla_cabecera = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        Dim CadenaEstructuraCC As String = ""

        CadenaEstructuraCC += "<ul>"
        If tabla_cabecera.Rows.Count > 0 Then
            For index_cabecera = 0 To tabla_cabecera.Rows.Count - 1
                CadenaEstructuraCC += "<li>"
                CadenaEstructuraCC += "<a><img alt='' src='../Imagenes/Pastilla.png' />" + tabla_cabecera.Rows(index_cabecera)("nmedico").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index_cabecera)("fec_registra").ToString().ToUpper().Trim() + "</a><input type='hidden' value='" + tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString() + "' />"
                oRceRecetaMedicamentoE.IdRecetaDet = CType(tabla_cabecera.Rows(index_cabecera)("ide_receta").ToString(), Integer)
                oRceRecetaMedicamentoE.Orden = 4
                Dim tabla_detalle As New DataTable()
                tabla_detalle = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
                If tabla_detalle.Rows.Count > 0 Then
                    CadenaEstructuraCC += "<ul>"
                    For index_detalle = 0 To tabla_detalle.Rows.Count - 1
                        CadenaEstructuraCC += "<li>"
                        'CadenaEstructuraCC += "<a><span class='JETIQUETA_2'>" + tabla_detalle.Rows(index_detalle)("nproducto").ToString().Trim() + "  UNIDADMEDIDA  " + "  VIA  " + _
                        '    tabla_detalle.Rows(index_detalle)("num_frecuencia").ToString().Trim() + " horas " + tabla_detalle.Rows(index_detalle)("num_cantidad").ToString().Trim() + "</span></a>"  'Ibuprofeno 10MG/2ML x 4 AMP  500mg  Oral  3 horas  3  Oral
                        'ide_receta ide_medicamentorec cod_producto
                        CadenaEstructuraCC += "<a><a>" + tabla_detalle.Rows(index_detalle)("nproducto").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;'>UNIDADMEDIDA</a>" + "<a style='margin-left:20px;'>VIA</a>" + _
                                "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("num_frecuencia").ToString().Trim() + "</a>" + " <span class='JETIQUETA_2' style='font-size:1em'>horas</span>" + _
                                "<a style='margin-left:20px;'>" + tabla_detalle.Rows(index_detalle)("num_cantidad").ToString().Trim() + "</a></a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("cod_producto").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("num_dosis").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("txt_detalle").ToString().Trim() + "</a>" + _
                                "<a style='margin-left:20px;display:none'>" + tabla_detalle.Rows(index_detalle)("num_duracion").ToString().Trim() + "</a>"

                        CadenaEstructuraCC += "</li>"
                    Next
                    CadenaEstructuraCC += "</ul>"
                End If

                CadenaEstructuraCC += "</li>"
            Next
        End If
        CadenaEstructuraCC += "</ul>"

        'REGRESO LA ESTRUCTURA DEL TREEVIEW
        Return CadenaEstructuraCC

    End Function



    Public Function ConsultaControlClinico_() As String
        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
        oRceRecetaMedicamentoE.IdRecetaDet = 0
        oRceRecetaMedicamentoE.Orden = 11 'JB - mostrara flg_alta = 0 (anteriormente orden 1)
        Dim tabla_cabecera As New DataTable()
        tabla_cabecera = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        Dim CadenaEstructuraCC As String = ""

        Dim contFecRegistro As Integer = 0
        Dim contHorRegistro As Integer = 0

        CadenaEstructuraCC += "<ul class='JTreeView'>"
        If tabla_cabecera.Rows.Count > 0 Then

            For index = 0 To tabla_cabecera.Rows.Count - 1
                If tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() <> "" Then
                    If contFecRegistro > 0 Then
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                        CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1

                        CadenaEstructuraCC += "</li>" 'cerrando nodo
                        contHorRegistro = 0
                    End If
                    CadenaEstructuraCC += "<li>" 'abriendo nodo
                    CadenaEstructuraCC += "<input type='hidden' value='" + tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() + "' class='JFECTREE' />"
                    CadenaEstructuraCC += "<span class='nudo'><img alt = '' src='../Imagenes/Pastilla.png' />"
                    CadenaEstructuraCC += "<span class='JTREE2-FECHA' >" + tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() + "</span>"
                    CadenaEstructuraCC += "</span>"
                    CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 1
                    contFecRegistro += 1
                End If
                If tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() <> "" Then
                    If contHorRegistro > 0 Then
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                        CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                        'CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                    End If
                    CadenaEstructuraCC += "<li>" 'abriendo sub nodo 2
                    CadenaEstructuraCC += "<span class='nudo'>" + tabla_cabecera.Rows(index)("NMEDICO").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla_cabecera.Rows(index)("ID_RECETA").ToString().Trim() + "' /><input type='hidden' value='" + tabla_cabecera.Rows(index)("TIPO").ToString().Trim() + "' />"
                    CadenaEstructuraCC += "<ul class='anidado'>" 'abriendo sub nodo 3

                    contHorRegistro += 1
                End If
                If tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() = "" And tabla_cabecera.Rows(index)("FEC_REGISTRO").ToString().Trim() = "" Then
                    Dim CadenaSuspendido As String = ""
                    If tabla_cabecera.Rows(index)("FLG_SUSPENSION").ToString().Trim() = "S" Then
                        CadenaSuspendido = "<span class='JETIQUETA_TREE_SUSPENDIDO'>Suspendido</span>"
                    End If

                    If tabla_cabecera.Rows(index)("TIPO") = "F" Then
                        CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' /><span class='JETIQUETA_TREE0' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla_cabecera.Rows(index)("DSC_VIA").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla_cabecera.Rows(index)("NUM_FRECUENCIA").ToString().Trim() + " horas </span><span class='JETIQUETA_TREE2' >" + tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                    End If
                    If tabla_cabecera.Rows(index)("TIPO").ToString().Trim() = "N" Then
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "NUTRICION" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />NUTRICION: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "TERAPIA" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />TERAPIA FISICA Y REHABILITACION: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "CUIDADOS" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />CUIDADOS DE ENFERMERIA: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                        If tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() = "OTROS" Then
                            'CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "" + CadenaSuspendido + "</li>"
                            CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />OTROS: <span class='JETIQUETA_TREE1' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                        End If
                    End If
                    If tabla_cabecera.Rows(index)("TIPO") = "I" Then
                        CadenaEstructuraCC += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + " " + CadenaSuspendido + "</li>"
                    End If
                End If
                '<li Class='JTree-Element'>Black Tea</li>


                If index = (tabla_cabecera.Rows.Count - 1) Then 'si llego al ultimo registro...
                    If contHorRegistro > 0 Then
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 3
                        CadenaEstructuraCC += "</li>" 'cerrando sub nodo 2
                        CadenaEstructuraCC += "</ul>" 'cerrando sub nodo 1
                    End If
                    If contFecRegistro > 0 Then
                        CadenaEstructuraCC += "</li>" 'cerrando nodo
                    End If
                End If
            Next
        End If

        CadenaEstructuraCC += "</ul>"

        'REGRESO LA ESTRUCTURA DEL TREEVIEW
        Return CadenaEstructuraCC

    End Function


    'JB - 06/08/2020 - NUEVO CODIGO
    Public Function ConsultaControlClinico2_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim oRceRecetaMedicamentoE_ As New RceRecetaMedicamentoE()
        Dim oRceRecetaMedicamentoLN_ As New RceRecetaMedicamentoLN()

        oRceRecetaMedicamentoE_.CodAtencion = Session(sCodigoAtencion)
        oRceRecetaMedicamentoE_.IdReceta = ide_recetacab
        oRceRecetaMedicamentoE_.FecReceta = fec_receta
        oRceRecetaMedicamentoE_.HorReceta = ""
        oRceRecetaMedicamentoE_.Orden = orden
        Dim ValorDevolver As String = ""
        Try
            Dim tabla As New DataTable()
            tabla = oRceRecetaMedicamentoLN_.Sp_RceRecetaMedicamentoCab_ConsultaV2(oRceRecetaMedicamentoE_)

            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JCCLI'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HOR_REGISTRO").ToString().Trim() + "</div><input type='hidden' class='IdeRecetaCab' value='" + tabla.Rows(index1)("IDE_RECETA").ToString().Trim() + "' /><input type='hidden' class='TipoRecetaCC' value='" + tabla.Rows(index1)("TIPO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                    'CadenaEstructuraCC += "<span class='nudo'>" + tabla_cabecera.Rows(index)("NMEDICO").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla_cabecera.Rows(index)("ID_RECETA").ToString().Trim() + "' /><input type='hidden' value='" + tabla_cabecera.Rows(index)("TIPO").ToString().Trim() + "' />"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaSuspendido As String = ""
                    If tabla.Rows(index1)("flg_suspension").ToString().Trim() = "S" Then
                        CadenaSuspendido = "<span class='JETIQUETA_TREE_SUSPENDIDO'>Suspendido</span>"
                    End If


                    'If tabla_cabecera.Rows(index)("TIPO") = "F" Then
                    '    CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' /><span class='JETIQUETA_TREE0' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla_cabecera.Rows(index)("DSC_VIA").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla_cabecera.Rows(index)("NUM_FRECUENCIA").ToString().Trim() + " horas </span><span class='JETIQUETA_TREE2' >" + tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                    'End If
                    If tabla.Rows(index1)("tipo") = "F" Then
                        ValorDevolver += "<div class='JTREE3-DETALLE'>"
                        ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla.Rows(index1)("dsc_via").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla.Rows(index1)("num_frecuencia").ToString().Trim() + " horas </span>" + "<span class='JETIQUETA_TREE2' >" + tabla.Rows(index1)("txt_detalle").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                        ValorDevolver += "</div>"
                    End If
                    If tabla.Rows(index1)("tipo").ToString().Trim() = "N" Then
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "NUTRICION" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />NUTRICION: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "NUTRICION: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "TERAPIA" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />TERAPIA FISICA Y REHABILITACION: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "TERAPIA FISICA Y REHABILITACION: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "CUIDADOS" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />CUIDADOS DE ENFERMERIA: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "CUIDADOS DE ENFERMERIA: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "OTROS" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />OTROS: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "OTROS: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                    End If
                    If tabla.Rows(index1)("tipo") = "I" Then
                        'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + " " + CadenaSuspendido + "</li>"
                        ValorDevolver += "<div class='JTREE3-DETALLE'>"
                        ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                        ValorDevolver += "</div>"
                    End If

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ConsultaControlClinico2PopupPedido(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim pagina As New Pedido()
        Return pagina.ConsultaControlClinico2PopupPedido_(orden, fec_receta, ide_recetacab)
    End Function
    Public Function ConsultaControlClinico2PopupPedido_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String) As String
        Dim oRceRecetaMedicamentoE_ As New RceRecetaMedicamentoE()
        Dim oRceRecetaMedicamentoLN_ As New RceRecetaMedicamentoLN()

        oRceRecetaMedicamentoE_.CodAtencion = Session(sCodigoAtencion)
        oRceRecetaMedicamentoE_.IdReceta = ide_recetacab
        oRceRecetaMedicamentoE_.FecReceta = fec_receta
        oRceRecetaMedicamentoE_.HorReceta = ""
        oRceRecetaMedicamentoE_.Orden = orden
        Dim ValorDevolver As String = ""
        Try
            Dim tabla As New DataTable()
            tabla = oRceRecetaMedicamentoLN_.Sp_RceRecetaMedicamentoCab_ConsultaV2(oRceRecetaMedicamentoE_)

            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JCCLI'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HOR_REGISTRO").ToString().Trim() + "</div><input type='hidden' class='IdeRecetaCab' value='" + tabla.Rows(index1)("IDE_RECETA").ToString().Trim() + "' /><input type='hidden' class='TipoRecetaCC' value='" + tabla.Rows(index1)("TIPO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                    'CadenaEstructuraCC += "<span class='nudo'>" + tabla_cabecera.Rows(index)("NMEDICO").ToString().ToUpper().Trim() + " | " + tabla_cabecera.Rows(index)("HOR_REGISTRO").ToString().Trim() + "</span><input type='hidden' value='" + tabla_cabecera.Rows(index)("ID_RECETA").ToString().Trim() + "' /><input type='hidden' value='" + tabla_cabecera.Rows(index)("TIPO").ToString().Trim() + "' />"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaSuspendido As String = ""
                    If tabla.Rows(index1)("flg_suspension").ToString().Trim() = "S" Then
                        CadenaSuspendido = "<span class='JETIQUETA_TREE_SUSPENDIDO'>Suspendido</span>"
                    End If


                    'If tabla_cabecera.Rows(index)("TIPO") = "F" Then
                    '    CadenaEstructuraCC += "<li Class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' /><span class='JETIQUETA_TREE0' >" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla_cabecera.Rows(index)("DSC_VIA").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla_cabecera.Rows(index)("NUM_FRECUENCIA").ToString().Trim() + " horas </span><span class='JETIQUETA_TREE2' >" + tabla_cabecera.Rows(index)("TXT_DETALLE").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                    'End If
                    If tabla.Rows(index1)("tipo") = "F" Then
                        ValorDevolver += "<div class='JTREE3-DETALLE'>"
                        ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span><span class='JETIQUETA_TREE2'> " + tabla.Rows(index1)("dsc_via").ToString().Trim() + "</span><span class='JETIQUETA_TREE2' > " + tabla.Rows(index1)("num_frecuencia").ToString().Trim() + " horas </span>" + "<span class='JETIQUETA_TREE2' >" + tabla.Rows(index1)("txt_detalle").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                        ValorDevolver += "</div>"
                    End If
                    If tabla.Rows(index1)("tipo").ToString().Trim() = "N" Then
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "NUTRICION" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />NUTRICION: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "NUTRICION: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "TERAPIA" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />TERAPIA FISICA Y REHABILITACION: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "TERAPIA FISICA Y REHABILITACION: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "CUIDADOS" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />CUIDADOS DE ENFERMERIA: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "CUIDADOS DE ENFERMERIA: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                        If tabla.Rows(index1)("TXT_DETALLE").ToString().Trim() = "OTROS" Then
                            'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla.Rows(index1)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />OTROS: <span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("NPRODUCTO").ToString().Trim() + "</span> " + CadenaSuspendido + "</li>"
                            ValorDevolver += "<div class='JTREE3-DETALLE'>"
                            ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + "OTROS: " + "</span><span class='JETIQUETA_TREE1' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                            ValorDevolver += "</div>"
                        End If
                    End If
                    If tabla.Rows(index1)("tipo") = "I" Then
                        'ValorDevolver += "<li class='JTree-Element'><input type='hidden' value='" + tabla_cabecera.Rows(index)("IDE_MEDICAMENTOREC").ToString().Trim() + "' />" + tabla_cabecera.Rows(index)("NPRODUCTO").ToString().Trim() + " " + CadenaSuspendido + "</li>"
                        ValorDevolver += "<div class='JTREE3-DETALLE'>"
                        ValorDevolver += "<div class='JVALOR-HORA'>" + "<span class='JETIQUETA_TREE0' >" + tabla.Rows(index1)("nproducto").ToString().Trim() + "</span>" + CadenaSuspendido + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_medicamentorec").ToString().Trim() + "' /> "
                        ValorDevolver += "</div>"
                    End If

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try

    End Function

End Class