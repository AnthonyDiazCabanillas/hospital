' **********************************************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2023. Todos los derechos reservados.
'    Version     Fecha           Autor       Requerimiento
'    1.1         19/06/2024      FGUEVARA    REQ-2024-011009:  RESULTADOS ROE - HC
'***********************************************************************************************************************

Imports System.Data
Imports System.IO
Imports Entidades.LaboratorioE
Imports LogicaNegocio.LaboratorioLN

Public Class Laboratorio
    Inherits System.Web.UI.Page

    Dim oRceLaboratioE As New RceLaboratioE()
    Dim oRceLaboratorioLN As New RceLaboratorioLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session(sIdeHistoria) = 0 '1.1
            If Not IsNothing(Request.Params("CodigoAtencion")) Then
                hfCodAtencionLabPopUp.Value = Request.Params("CodigoAtencion").Trim()
            End If

        End If
    End Sub

    ''' <summary>
    ''' CARGANDO DATOS DEL TREEVIEW DE ANALISIS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewLaboratorio(ByVal CodigoAtencion As String) As String
        Dim pagina As New Laboratorio()
        Return pagina.TreeViewLaboratorio_(CodigoAtencion)
    End Function
    Public Function TreeViewLaboratorio_(ByVal CodigoAtencion As String) As String
        Dim IdRecetaCab As Integer = 0
        'oRceLaboratioE.CodAtencion = CodigoAtencion
        'oRceLaboratioE.CodMedico = "00000590"  'CAMBIARLO LUEGO POR EL VALOR REAL
        'oRceLaboratioE.Orden = 1
        'Dim tabla_ As New DataTable()
        'tabla_ = oRceLaboratorioLN.Sp_RceRecetaAnalisisCab_Consulta(oRceLaboratioE)
        'If tabla_.Rows.Count > 0 Then
        '    IdRecetaCab = CType(tabla_.Rows(0)("ide_recetacab").ToString(), Integer)
        'End If
        oRceLaboratioE.CodAtencion = CodigoAtencion 'Session(sCodigoAtencion)
        oRceLaboratioE.IdeRecetaCab = IdRecetaCab
        oRceLaboratioE.Orden = 1
        Dim ValorDevolver As String = ""
        Try
            Dim tabla As New DataTable()
            tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_Consulta(oRceLaboratioE)
            ValorDevolver += "<ul>"
            For index = 0 To tabla.Rows.Count - 1
                If tabla.Rows(index)("ide_recetadet").ToString() = 0 Then
                    If index > 0 Then
                        ValorDevolver += "</ul></li>"
                    End If
                    ValorDevolver += "<li>"

                    If tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Verde.jpg' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
                    ElseIf tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Rojo.png' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
                    Else
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Amarillo.jpg' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
                    End If
                    ValorDevolver += "<ul>"
                Else
                    ValorDevolver += "<li>"
                    If tabla.Rows(index)("est_analisis").ToString().Trim() = "T" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Verde.jpg' />" + tabla.Rows(index)("dsc_analisis").ToString() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
                    ElseIf tabla.Rows(index)("est_analisis").ToString().Trim() = "G" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Rojo.png' />" + tabla.Rows(index)("dsc_analisis").ToString() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
                    Else
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Amarillo.jpg' />" + tabla.Rows(index)("dsc_analisis").ToString() + "</a><input type='hidden' value='" + tabla.Rows(index)("ide_recetacab").ToString() + "' />"
                    End If
                    ValorDevolver += "</li>"
                End If
            Next
            ValorDevolver += "</ul>"
            Return ValorDevolver
        Catch ex As Exception
            ValorDevolver = "ERROR;" + ex.Message.ToString()
            Return ValorDevolver
        End Try

    End Function


    ''' <summary>
    ''' FUNCION PARA VER INFORME DE ANALISIS
    ''' </summary>
    ''' <param name="IdRecetaCab"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function VerInformeAnalisis(ByVal IdRecetaCab As String) As String
        Dim pagina As New Laboratorio()
        Return pagina.VerInformeAnalisis_(IdRecetaCab)
    End Function
    Public Function VerInformeAnalisis_(ByVal IdRecetaCab As String) As String
        Dim NombreArchivo As String
        Dim Ruta As String
        Dim RutaArchivo As String
        Dim bRutaWebRce As Boolean = False
        oRceLaboratioE.IdeRecetaCab = IdRecetaCab
        oRceLaboratioE.Orden = 1
        NombreArchivo = oRceLaboratioE.IdeRecetaCab.ToString() + ".PDF"
        Dim Mensaje As String = ""

        bRutaWebRce = HttpContext.Current.Server.MapPath("/").Contains("wwwroot")

        If bRutaWebRce Then
            Ruta = HttpContext.Current.Server.MapPath("/") + "\Archivos\" + NombreArchivo
        Else
            Ruta = HttpContext.Current.Server.MapPath("\Archivos\" + NombreArchivo)
        End If

        Dim tabla As New DataTable()
        tabla = oRceLaboratorioLN.Sp_RceResultadoAnalisisCab_Consulta(oRceLaboratioE)

        For index = 0 To tabla.Rows.Count - 1
            If IsNothing(tabla.Rows(index)("blb_resultado")) Or IsDBNull(tabla.Rows(index)("blb_resultado")) Then
                Mensaje = "ERROR;El informe se encuentra en proceso."
                Return Mensaje
            End If
            File.WriteAllBytes(Ruta, tabla.Rows(index)("blb_resultado"))
        Next
        RutaArchivo = ("/Archivos/" + NombreArchivo).Replace("//", "/")

        Return RutaArchivo
    End Function




    'INICIO - JB - 04/08/2020 - NUEVO CODIGO
    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewAnalisis2(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String, ByVal CodigoAtencion As String) As String
        Dim pagina As New Laboratorio()
        Return pagina.TreeViewAnalisisV2_(orden, fec_receta, ide_recetacab, CodigoAtencion)
    End Function

    Public Function TreeViewAnalisisV2_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String, ByVal CodigoAtencion As String) As String
        oRceLaboratioE.CodAtencion = CodigoAtencion
        oRceLaboratioE.IdeRecetaCab = ide_recetacab
        oRceLaboratioE.FechaReceta = fec_receta
        oRceLaboratioE.HoraReceta = ""
        oRceLaboratioE.Orden = orden
        Dim ValorDevolver As String = ""

        Dim LaboratorioR As String = ConfigurationManager.AppSettings("LABORATORIO_ROJO").Trim()
        Dim LaboratorioA As String = ConfigurationManager.AppSettings("LABORATORIO_AMARILLO").Trim()
        Dim LaboratorioV As String = ConfigurationManager.AppSettings("LABORATORIO_VERDE").Trim()

        Try
            Dim tabla As New DataTable()
            tabla = oRceLaboratorioLN.Sp_RceRecetaAnalisisDet_ConsultaV2(oRceLaboratioE)


            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    'INICIO - JB - 11/08/2020
                    Dim CadenaImgLag As String = ""

                    If Integer.Parse(tabla.Rows(index1)("A").ToString().Trim()) > 0 Then
                        CadenaImgLag += "JLAB-ROJO"
                    End If
                    If Integer.Parse(tabla.Rows(index1)("N").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("A").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JLAB-AMARILLO"
                    End If

                    If Integer.Parse(tabla.Rows(index1)("T").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("A").ToString().Trim()) = 0 And Integer.Parse(tabla.Rows(index1)("N").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JLAB-VERDE"
                    End If
                    'FIN - JB - 11/08/2020


                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaImgLag += "JLAB-VERDE"
                    ElseIf tabla.Rows(index1)("est_analisis").ToString().Trim() = "A" Or tabla.Rows(index1)("est_analisis").ToString().Trim() = "G" Then
                        CadenaImgLag += "JLAB-ROJO"
                    Else
                        CadenaImgLag += "JLAB-AMARILLO"
                    End If


                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + " - " + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HORA_RECETA").ToString().Trim() + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim ProgramarHora As String = ""
                    If tabla.Rows(index1)("dsc_programacion").ToString().Trim() <> "" Then
                        ProgramarHora += " - Programado " + tabla.Rows(index1)("dsc_programacion").ToString().Trim()
                    End If
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaImgLag += "JLAB-VERDE"
                    ElseIf tabla.Rows(index1)("est_analisis").ToString().Trim() = "A" Or tabla.Rows(index1)("est_analisis").ToString().Trim() = "G" Then
                        CadenaImgLag += "JLAB-ROJO"
                    Else
                        CadenaImgLag += "JLAB-AMARILLO"
                    End If

                    Dim CadenaFlgVerificar As String = ""
                    Dim CadenaFlgVerificar2 As String = ""
                    If tabla.Rows(index1)("est_analisis").ToString().Trim() = "P" Or tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarLab' value='" + tabla.Rows(index1)("flg_verificar").ToString() + "' />"
                    Else
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarLab' value='' />"
                    End If
                    If tabla.Rows(index1)("flg_visto").ToString().Trim() = "S" And tabla.Rows(index1)("est_analisis").ToString().Trim() = "T" Then
                        CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
                    End If

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("dsc_analisis").ToString().Trim() + " <span class='JETIQUETA_TREE2'>" + ProgramarHora + "</span> " + CadenaFlgVerificar2 + "</div><input type='hidden' value='" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + "'  /> " + CadenaFlgVerificar
                    ValorDevolver += "</div>"

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function VerInformeAnalisis2(ByVal IdRecetaCab As String) As String
        Dim pagina As New Laboratorio()
        Return pagina.VerInformeAnalisis2_(IdRecetaCab)
    End Function
    Public Function VerInformeAnalisis2_(ByVal IdRecetaCab As String) As String
        Dim NombreArchivo As String
        Dim Ruta As String = ""
        Dim RutaArchivo As String
        Dim bRutaWebRce As Boolean = False
        oRceLaboratioE.IdeRecetaCab = IdRecetaCab
        oRceLaboratioE.Orden = 1
        NombreArchivo = oRceLaboratioE.IdeRecetaCab.ToString() + ".PDF"
        Dim Mensaje As String = ""
        Dim tabla As New DataTable()

        Try
            'INI 1.1
            'bRutaWebRce = HttpContext.Current.Server.MapPath("/").Contains("wwwroot")

            'If bRutaWebRce Then
            '    Ruta = HttpContext.Current.Server.MapPath("/") + "\Archivos\" + NombreArchivo
            'Else
            '    Ruta = HttpContext.Current.Server.MapPath("\Archivos\" + NombreArchivo)
            'End If
            'FIN 1.1

            'TMACASSI 14/09/2016
            'Ruta = sRutaArchivos + NombreArchivo
            tabla = oRceLaboratorioLN.Sp_RceResultadoAnalisisCab_Consulta(oRceLaboratioE)

            For index = 0 To tabla.Rows.Count - 1
                If IsNothing(tabla.Rows(index)("blb_resultado")) Or IsDBNull(tabla.Rows(index)("blb_resultado")) Then
                    Mensaje = "ERROR;El informe se encuentra en proceso."
                    Return Mensaje
                End If
                'File.WriteAllBytes(Ruta, tabla.Rows(index)("blb_resultado")) '1.1
            Next
            'RutaArchivo = ("/Archivos/" + NombreArchivo).Replace("//", "/") '1.1

            'Dim ie
            'ie = CreateObject("internetexplorer.application")
            'ie.Navigate(RutaArchivo)
            'ie.Visible = True

            'Return RutaArchivo '1.1
            Return "" '1.1

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString() + "****" + Ruta + "****" + CType(tabla.Rows.Count, String)
        End Try
    End Function
    'INICIO - JB - 04/08/2020 - NUEVO CODIGO

End Class