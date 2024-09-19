Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports LogicaNegocio.MedicamentosLN
Imports Entidades.MedicamentosE
Imports Entidades.PatologiaE
Imports LogicaNegocio.PatologiaLN

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports
Imports Entidades.AnamnesisE
Imports LogicaNegocio.AnamnesisLN
Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN

Public Class About
    Inherits System.Web.UI.Page

    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()
    Dim xRuta As String = sRutaTemp
    Dim xNombreArchivo As String = ""
    Dim crystalreport As New ReportDocument()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim dt, tabla As New DataTable()
            dt.Columns.Add("id_documento")
            dt.Columns.Add("codatencion")
            dt.Columns.Add("tipo_doc")
            dt.Columns.Add("usuario_creacion")
            dt.Columns.Add("fecha_creacion")
            dt.Columns.Add("estado")
            Dim columna1 As DataColumn = New DataColumn("bib_documento")
            columna1.DataType = System.Type.GetType("System.Byte[]")
            dt.Columns.Add(columna1)
            dt.Columns.Add("extension_doc")
            dt.Columns.Add("descripcion_doc")
            dt.Columns.Add("usuario_eliminacion")
            dt.Columns.Add("fecha_eliminacion")
            dt.Columns.Add("flg_firma")
            dt.Columns.Add("fec_firma")
            dt.Columns.Add("usr_firma")
            dt.Columns.Add("blb_documentofirma")
            dt.Columns.Add("dsc_extensionfirma")



            Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaMedicamentoTableAdapter()
            tabla = New DataTable("Rp_RceRecetaMedicamento")
            tabla.Columns.Add("ide_medicamentorec")
            tabla.Columns.Add("ide_receta")
            tabla.Columns.Add("dsc_paciente")
            tabla.Columns.Add("cod_medico")
            tabla.Columns.Add("dsc_medico")
            tabla.Columns.Add("edad")
            tabla.Columns.Add("dias")
            tabla.Columns.Add("fec_registra")
            tabla.Columns.Add("cama")
            tabla.Columns.Add("dsc_diagnostico")
            tabla.Columns.Add("talla")
            tabla.Columns.Add("peso")
            tabla.Columns.Add("opt_ram")
            tabla.Columns.Add("dsc_ram")
            tabla.Columns.Add("telefono")
            tabla.Columns.Add("dias_post")
            tabla.Columns.Add("indicaciones")
            tabla.Columns.Add("dsc_medicamento")
            tabla.Columns.Add("via")
            tabla.Columns.Add("hora")
            tabla.Columns.Add("hora_fecha1")
            tabla.Columns.Add("hora_fecha2")
            tabla.Columns.Add("hora_fecha3")
            tabla.Columns.Add("hora_fecha4")
            tabla.Columns.Add("hora_fecha5")
            tabla.Columns.Add("hora_fecha6")
            tabla.Columns.Add("nmedico")
            tabla.Columns.Add("RNE")
            tabla.Columns.Add("CMP")

            Dim oRceMedicamentosE As New RceMedicamentosE()
            Dim oRceMedicamentosLN As New RceMedicamentosLN()
            oRceMedicamentosE.IdHistoria = 344743
            If Not IsNothing(Request.Params("Valor")) And Request.Params("Valor").Trim() <> "" Then
                If Request.Params("Valor").Split(";")(0) = "ID" Then 'si selecciono 'hora' (detalle) en el treeview
                    Dim IdRecetaCab As Integer = 0
                    Dim Convertir As Boolean
                    Convertir = Integer.TryParse(Request.Params("Valor").Split(";")(1).ToString().Trim(), IdRecetaCab)
                    If Convertir = True Then
                        oRceMedicamentosE.IdMedicamentosaCab = Request.Params("Valor").Split(";")(1).ToString().Trim()
                    Else
                        oRceMedicamentosE.IdMedicamentosaCab = 0
                    End If
                    oRceMedicamentosE.Orden = 1
                Else 'si selecciono 'fecha' en el treeview
                    oRceMedicamentosE.FecReceta = Request.Params("Valor").Split(";")(1).ToString().Trim()
                    oRceMedicamentosE.Orden = 2
                End If
            Else
                oRceMedicamentosE.Orden = 3
            End If

            tabla = oRceMedicamentosLN.Rp_RceRecetaMedicamento1(oRceMedicamentosE)





            'PREPERANDO EXPORTACION DE REPORTE A PDF
            Dim OpcionExportar As ExportOptions
            Dim OpcionDestino As New DiskFileDestinationOptions()
            Dim OpcionesFormato As New PdfRtfWordFormatOptions()
            xNombreArchivo = "IM" + "344743_" + ".pdf"
            OpcionDestino.DiskFileName = xRuta + "\" + xNombreArchivo
            OpcionExportar = crystalreport.ExportOptions
            With OpcionExportar
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
                .DestinationOptions = OpcionDestino
                .FormatOptions = OpcionesFormato
            End With
            'EXPORTANDO A PDF
            crystalreport.Export()
            'CONVIRTIENDO ARCHIVO PDF GENERADO EN BYTE()
            Dim pdf_byte As Byte() = System.IO.File.ReadAllBytes(xRuta + "\" + xNombreArchivo)
            System.IO.File.WriteAllBytes(xRuta + "\" + xNombreArchivo, pdf_byte)
            'MOSTRANDO ARCHIVO BYTE() DEL PDF EN PANTALLA
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-length", pdf_byte.Length.ToString())
            Response.BinaryWrite(pdf_byte)
            Response.Flush()

            If Request.Params("OP") = "IM" Then 'JB - 02/10/2019 - 
                If tabla.Rows.Count > 0 Then
                    Dim oRceMedicamentosE_ As New RceMedicamentosE()
                    Dim oRceMedicamentosLN_ As New RceMedicamentosLN()
                    oRceMedicamentosE_.IdHistoria = 344743
                    oRceMedicamentosE_.Detalle = ""
                    oRceMedicamentosE_.CodUser = 0
                    oRceMedicamentosE_.TipoDocumento = 2
                    oRceMedicamentosE_.Documento = pdf_byte
                    oRceMedicamentosE_.Estado = "A"
                    oRceMedicamentosE_.Codigo = "H0175959"
                    oRceMedicamentosE_.FecReporte = DateTime.Parse(tabla.Rows(0)("fec_registra2"))

                    'oRceMedicamentosLN_.Sp_RceResultadoDocumentoDet_InsertV3(oRceMedicamentosE_)
                End If
            End If

            'ELIMINANDO ARCHIVO
            System.IO.File.Delete(xRuta + "\" + xNombreArchivo)

        End If

    End Sub

End Class