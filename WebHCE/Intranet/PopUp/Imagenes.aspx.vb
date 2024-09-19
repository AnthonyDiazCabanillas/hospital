Imports System.Data
Imports System.IO
Imports Entidades.ImagenesE
Imports LogicaNegocio.ImagenLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN
Imports RestSharp

Public Class Imagenes
    Inherits System.Web.UI.Page
    Dim oRceImagenesE As New RceImagenesE()
    Dim oRceImagenLN As New RceImagenLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Not IsNothing(Request.Params("CodigoAtencion")) Then
                hfCodAtencionImgPopUp.Value = Request.Params("CodigoAtencion").Trim()
            End If

        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewImagenes(ByVal CodigoAtencion As String) As String
        Dim pagina As New Imagenes()
        Return pagina.TreeViewImagenes_(CodigoAtencion)
    End Function
    Public Function TreeViewImagenes_(ByVal CodigoAtencion As String) As String
        oRceImagenesE.CodAtencion = CodigoAtencion 'Session(sCodigoAtencion)
        oRceImagenesE.Orden = 1
        Dim ValorDevolver As String = ""
        Dim tabla As New DataTable()
        tabla = oRceImagenLN.Sp_RceRecetaImagenDet_Consulta(oRceImagenesE)
        ValorDevolver += "<ul>"
        Try
            For index = 0 To tabla.Rows.Count - 1
                If tabla.Rows(index)("ide_recetadet").ToString() = 0 Then
                    If index > 0 Then
                        ValorDevolver += "</ul></li>"
                    End If
                    ValorDevolver += "<li>"

                    If tabla.Rows(index)("est_imagen").ToString().Trim() = "T" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Imagenes_Verde.jpg' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("cod_presotor").ToString() + "_" + tabla.Rows(index)("sps_id_key").ToString().Trim() + "' />"
                    Else
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Imagenes_Rojo.png' />" + tabla.Rows(index)("ide_recetacab").ToString() + " - " + tabla.Rows(index)("medico").ToString().Trim() + " | " + tabla.Rows(index)("fec_registra").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("cod_presotor").ToString() + "_" + tabla.Rows(index)("sps_id_key").ToString().Trim() + "' />"
                    End If
                    ValorDevolver += "<ul>"
                Else
                    ValorDevolver += "<li>"
                    If tabla.Rows(index)("sps_id_key").ToString().Trim() <> "" And tabla.Rows(index)("est_imagen").ToString().Trim() = "T" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Laboratorio_Verde.jpg' />" + tabla.Rows(index)("cod_prestacion").ToString() + " - " + tabla.Rows(index)("dsc_imagen").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("cod_presotor").ToString() + "_" + tabla.Rows(index)("sps_id_key").ToString().Trim() + "' />"
                    ElseIf tabla.Rows(index)("sps_id_key").ToString().Trim() <> "" Then
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Imagenes_Amarillo.jpg' />" + tabla.Rows(index)("cod_prestacion").ToString() + " - " + tabla.Rows(index)("dsc_imagen").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("cod_presotor").ToString() + "_" + tabla.Rows(index)("sps_id_key").ToString().Trim() + "' />"
                    Else
                        ValorDevolver += "<a><img alt='' src='../Imagenes/Res_Imagenes_Rojo.png' />" + tabla.Rows(index)("cod_prestacion").ToString() + " - " + tabla.Rows(index)("dsc_imagen").ToString().Trim() + "</a><input type='hidden' value='" + tabla.Rows(index)("cod_presotor").ToString() + "_" + tabla.Rows(index)("sps_id_key").ToString().Trim() + "' />"
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



    <System.Web.Services.WebMethod()>
    Public Shared Function VerInformeImagen(ByVal PresotorSps As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerInformeImagen_(PresotorSps)
    End Function
    Public Function VerInformeImagen_(ByVal PresotorSps As String) As String
        Try
            Dim xSpsIdKeyRis As String
            Dim xNombreArchivo As String
            Dim bRutaWebRce As Boolean
            Dim xRuta As String
            Dim Mensaje As String = ""
            Dim RutaArchivo As String

            xSpsIdKeyRis = PresotorSps.Trim().Split("_")(1).Trim()
            oRceImagenesE.CodPresotor = PresotorSps.Trim().Split("_")(0).Trim()
            xNombreArchivo = oRceImagenesE.CodPresotor + ".PDF"

            If oRceImagenesE.CodPresotor <> "" Then
                bRutaWebRce = HttpContext.Current.Server.MapPath("/").Contains("wwwroot")
                If bRutaWebRce Then
                    xRuta = HttpContext.Current.Server.MapPath("/") + "\Archivos\" + xNombreArchivo
                Else
                    xRuta = HttpContext.Current.Server.MapPath("\Archivos\" + xNombreArchivo)
                End If
                Dim tabla As New DataTable()
                tabla = oRceImagenLN.Sp_Presotor_Pdf_Consulta(oRceImagenesE)

                If tabla.Rows.Count = 0 Then
                    Mensaje += "ERROR;No existe informe para el registro seleccionado."
                    Return Mensaje
                Else
                    For index = 0 To tabla.Rows.Count - 1
                        If Not IsNothing(tabla.Rows(index)("blob")) And Not IsDBNull(tabla.Rows(index)("blob")) Then
                            File.WriteAllBytes(xRuta, tabla.Rows(index)("blob"))
                        Else
                            Mensaje += "ERROR;El informe se encuentra en proceso."
                            Return Mensaje
                        End If
                    Next
                End If
            Else
                Mensaje += "ERROR;No existe informe para el registro seleccionado."
                Return Mensaje
            End If
            RutaArchivo = ("/Archivos/" + xNombreArchivo).Replace("//", "/")

            Return RutaArchivo
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function




    'INICIO - JB - 04/08/2020 - NUEVO CODIGO
    <System.Web.Services.WebMethod()>
    Public Shared Function TreeViewImagenes2(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String, ByVal CodigoAtencion As String) As String
        Dim pagina As New Imagenes()
        Return pagina.TreeViewImagenes2_(orden, fec_receta, ide_recetacab, CodigoAtencion)
    End Function

    Public Function TreeViewImagenes2_(ByVal orden As String, ByVal fec_receta As String, ByVal ide_recetacab As String, ByVal CodigoAtencion As String) As String
        oRceImagenesE.CodAtencion = CodigoAtencion
        oRceImagenesE.IdeRecetaCab = ide_recetacab
        oRceImagenesE.FecReceta = fec_receta
        oRceImagenesE.HorReceta = ""
        oRceImagenesE.Orden = orden
        Dim ValorDevolver As String = ""
        Try
            Dim tabla As New DataTable()
            tabla = oRceImagenLN.Sp_RceRecetaImagenDet_ConsultaV2(oRceImagenesE)


            If tabla.Rows.Count > 0 And orden = 1 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    'INICIO - JB - 11/08/2020
                    Dim CadenaImgLag As String = ""

                    If Integer.Parse(tabla.Rows(index1)("G").ToString().Trim()) > 0 Then
                        CadenaImgLag += "JIMAG-ROJO"
                    End If
                    If Integer.Parse(tabla.Rows(index1)("P").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("G").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JIMAG-AMARILLO"
                    End If

                    If Integer.Parse(tabla.Rows(index1)("T").ToString().Trim()) > 0 And Integer.Parse(tabla.Rows(index1)("G").ToString().Trim()) = 0 And Integer.Parse(tabla.Rows(index1)("P").ToString().Trim()) = 0 Then
                        CadenaImgLag += "JIMAG-VERDE"
                    End If
                    'FIN - JB - 11/08/2020

                    ValorDevolver += "<div class='JTREE3-FECHA'>"

                    ValorDevolver += "<div class='JFILA-FECHA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-FECHA'>" + tabla.Rows(index1)("FEC_REGISTRO") + "</div><input type='hidden' class='FecRegistro' value='" + tabla.Rows(index1)("FEC_REGISTRO").ToString().Trim() + "' />"
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JTREE3-HORA' style='display:none;'>"  'aqui ira el contenido de las horas
                    ValorDevolver += "</div>"

                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 2 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" Then
                        CadenaImgLag += "JIMAG-VERDE"
                    ElseIf tabla.Rows(index1)("est_imagen").ToString().Trim() = "P" Then
                        CadenaImgLag += "JIMAG-AMARILLO"
                    Else
                        CadenaImgLag += "JIMAG-ROJO"
                    End If


                    ValorDevolver += "<div class='JFILA-HORA'>"
                    ValorDevolver += "<div class='JTREE3-SIGNO'></div><div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + " - " + tabla.Rows(index1)("NMEDICO").ToString().Trim() + " | " + tabla.Rows(index1)("HORA_RECETA").ToString().Trim() + "</div><input type='hidden' class='IdeImagenCab' value='" + tabla.Rows(index1)("ide_recetacab").ToString().Trim() + "' />"  'COD_PRESOTOR + _ + SPS_ID_KEY
                    ValorDevolver += "</div>"
                    ValorDevolver += "<div class='JFILA-DETALLE'>"
                    ValorDevolver += "</div>"
                Next
            End If
            If tabla.Rows.Count > 0 And orden = 3 Then
                For index1 = 0 To tabla.Rows.Count - 1
                    Dim CadenaImgLag As String = ""
                    If tabla.Rows(index1)("sps_id_key").ToString().Trim() <> "" And tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" Then 'tabla.Rows(index)("SPS_ID_KEY").ToString().Trim() <> "0" And tabla.Rows(index)("EST_IMAGEN").ToString().Trim() = "C"
                        CadenaImgLag += "JIMAG-VERDE"
                    ElseIf tabla.Rows(index1)("sps_id_key").ToString().Trim() <> "" Then
                        CadenaImgLag += "JIMAG-AMARILLO"
                    Else
                        CadenaImgLag += "JIMAG-ROJO"
                    End If

                    Dim CadenaFlgVerificar As String = ""
                    Dim CadenaFlgVerificar2 As String = ""
                    If tabla.Rows(index1)("est_imagen").ToString().Trim() = "T" And tabla.Rows(index1)("flg_verificar").ToString().Trim() = "S" Then
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='" + tabla.Rows(index1)("flg_verificar").ToString() + "' />"
                    Else
                        CadenaFlgVerificar += "<input type='hidden' class='FlgVerificarIma' value='' />"
                    End If
                    If tabla.Rows(index1)("flg_visto").ToString().Trim() = "S" Then
                        CadenaFlgVerificar2 += " <img alt='' src='../Imagenes/ico_visto.png' />"
                    End If

                    ValorDevolver += "<div class='JTREE3-DETALLE'>"
                    ValorDevolver += "<div class='" + CadenaImgLag + "'></div><div class='JVALOR-HORA'>" + tabla.Rows(index1)("cod_prestacion").ToString().Trim() + " - " + tabla.Rows(index1)("dsc_imagen").ToString().Trim() + " " + CadenaFlgVerificar2 + "</div><input type='hidden' value='" + tabla.Rows(index1)("cod_presotor").ToString().Trim() + "_" + tabla.Rows(index1)("sps_id_key").ToString().Trim() + "' /> " + CadenaFlgVerificar + " <input type='hidden' class='IdeImagenDet' value='" + tabla.Rows(index1)("ide_recetadet").ToString().Trim() + "' /> "
                    ValorDevolver += "</div>"

                Next
            End If

            Return ValorDevolver
        Catch ex As Exception
            Return "ERROR*" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function VerImagen(ByVal PresotorSps As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.VerImagen_(PresotorSps)
    End Function
    Public Function VerImagen_(ByVal PresotorSps As String) As String
        'Dim wUrl As String = ""
        'Dim wUrl1 As String = ""
        'Dim wUrl2 As String = ""
        'Dim wUsuario As String = ""
        'Dim wContrasena As String = ""
        'Dim xSpsIdKeyRis As String
        Try
            'oRceImagenesE.CodTabla = "RIS_URL"
            'oRceImagenesE.Buscar = ""
            'oRceImagenesE.Key = 34
            'oRceImagenesE.NumeroLineas = 10
            'oRceImagenesE.Orden = 0
            'Dim tabla As New DataTable()
            'tabla = oRceImagenLN.Sp_Tablas_Consulta(oRceImagenesE)

            'For index = 0 To tabla.Rows.Count - 1
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "01" Then wUrl1 = tabla.Rows(index)("nombre").ToString().Trim()
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "02" Then wUrl2 = tabla.Rows(index)("nombre").ToString().Trim()
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "03" Then wUsuario = tabla.Rows(index)("nombre").ToString().Trim()
            '    If tabla.Rows(index)("codigo").ToString().Trim() = "04" Then wContrasena = tabla.Rows(index)("nombre").ToString().Trim()
            'Next
            'xSpsIdKeyRis = PresotorSps.Trim().Split("_")(1).Trim()
            'wUrl = wUrl1 + wUrl2
            'wUrl = Replace(wUrl, "%U", wUsuario)
            'wUrl = Replace(wUrl, "%P", wContrasena)
            'wUrl = Replace(wUrl, "%O", xSpsIdKeyRis)

            'comentado 08/11/2016
            'Dim ie
            'ie = CreateObject("internetexplorer.application")
            'ie.Navigate(wUrl)
            'ie.Visible = True
            'fin comentado 08/11/2016


            Dim oTablasE As New TablasE
            Dim oListTablasE As New List(Of TablasE)
            Dim dt As New DataTable()
            Dim oTablasLN As New TablasLN
            oTablasE.CodTabla = "RIS_URL"
            oTablasE.Buscar = ""
            oTablasE.Key = 54
            oTablasE.NumeroLineas = 1
            oTablasE.Orden = 2
            dt = oTablasLN.Sp_Tablas_Consulta(oTablasE)
            Dim Dato01 = "", Dato02 = "", Dato03 = "", Dato04 = "", ImagenURL = "", ParametroURL As String

            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("codigo").ToString().Trim() = "01" Then
                    Dato01 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "02" Then
                    Dato02 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "03" Then
                    Dato03 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "04" Then
                    Dato04 = dt.Rows(index)("nombre").ToString()
                End If
                If dt.Rows(index)("codigo").ToString().Trim() = "05" Then
                    ImagenURL = dt.Rows(index)("nombre").ToString()
                End If
            Next
            Dato01 = sRutaServicioRes 'JB - nueva linea, ruta ahora vendra de variables globales

            Dim xCadena As String
            Dim xSpsIdKeyRis As String
            Dim xPosicionRaya As Integer
            xCadena = PresotorSps
            xPosicionRaya = InStrRev(xCadena, "_")
            xSpsIdKeyRis = xCadena.Split("_")(1).Trim()

            ParametroURL = Dato02
            ParametroURL = Replace(ParametroURL, "%U", Dato03) 'Dato03  emr_csf
            ParametroURL = Replace(ParametroURL, "%P", Dato04) 'Dato04  csf123
            ParametroURL = Replace(ParametroURL, "%H", Session(sCodPaciente)) ' tabla1.Rows(0)("cod_paciente").ToString().Trim())
            ParametroURL = Replace(ParametroURL, "%O", xSpsIdKeyRis) 'JCAICEDO.28/03/2019 Se restauro el SpIdKey '"9275000605840") 'sps_id hardcore temporalmente para pruebas *xSpsIdKeyRis*
            ParametroURL = """" + ParametroURL + """"

            Dim CadenaA As String = ""
            CadenaA = Dato01 + Dato02.Substring(0, Dato02.LastIndexOf("%P") + 2)
            CadenaA = CadenaA.Replace("%U", Dato03) 'Dato03  emr_csf
            CadenaA = CadenaA.Replace("%P", Dato04) 'Dato04  csf123

            Dim client = New RestClient(CadenaA)
            Dim request = New RestRequest(Method.POST)
            request.AddHeader("cache-control", "no-cache")
            request.AddHeader("content-type", "application/json")
            'request.AddBody("""user_name=emr_csf&password=csf123&patient_id=406363&accssion_number=9275000574535""")
            request.AddParameter("application/json", ParametroURL, RestSharp.ParameterType.RequestBody)

            Dim response As RestResponse = client.Execute(request)
            Dim encriptado As String = response.Content.Replace("""", "")

            Return ImagenURL + encriptado
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function


    'FIN - JB - 04/08/2020 - NUEVO CODIGO
End Class