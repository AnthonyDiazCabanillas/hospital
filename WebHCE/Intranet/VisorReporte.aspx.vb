' **********************************************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2023. Todos los derechos reservados.
'    Version     Fecha           Autor       Requerimiento
'    1.1         20/10/2023      AROMERO     REQ-2023-017255:  REPORTE HISTORIA CLINICA HOPITAL
'    1.2         19/06/2024      FGUEVARA    REQ-2024-011009:  RESULTADOS ROE - HC
'***********************************************************************************************************************
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


Imports Entidades.EvolucionE
Imports LogicaNegocio.EvolucionLN
Imports Entidades.JuntaMedicaE
Imports LogicaNegocio.JuntaMedicaLN
Imports Entidades.NotaIngresoE
Imports LogicaNegocio.NotaIngresoLN
Imports LogicaNegocio.ControlClinicoLN
Imports Entidades.ControlClinicoE
Imports Entidades
Imports Newtonsoft.Json
Imports RestSharp

Public Class VisorReporte
    Inherits System.Web.UI.Page

    Dim oHospitalE As New HospitalE()
    Dim oHospitalLN As New HospitalLN()
    Dim xRuta As String = sRutaTemp
    Dim xNombreArchivo As String = ""
    Dim crystalreport As New ReportDocument()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Not IsNothing(Session(sIdeHistoria)) Then
                Reportes()
                OtrosReportes()
            Else

            End If
        End If

    End Sub

    Public Sub Reportes()
        Dim _reporteApi As RespuestaArchivoJsonE
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


        Dim DtCI As New DataTable()

        'INICIO - JB - 30/01/2017
        If Not IsNothing(Request.Params("OP")) Then
            If Request.Params("OP") = "CI" Then 'CONSENTIMIENTO INFORMADO
                'INICIO - JB - COMENTADO - 23/01/2020
                'oHospitalE.IdeHistoria = Session(sIdeHistoria)
                'oHospitalE.TipoDoc = 2
                'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)
                'FIN - JB - COMENTADO - 23/01/2020


                'INICIO - JB - NUEVO CODIGO - 23/01/2020
                Dim ConvertirCI As Boolean
                Dim IdePlantilla As Integer = 0
                ConvertirCI = Integer.TryParse(Request.Params("Valor").Split(";")(0).ToString().Trim(), IdePlantilla)
                If ConvertirCI Then
                    oHospitalE.IdePlantilla = IdePlantilla
                End If
                oHospitalE.Orden = 0
                DtCI = oHospitalLN.Sp_CIFormatos_Consulta(oHospitalE) 'devuelve el formato que se debe mostrar




                Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceHojadeconsentimientoTableAdapter()
                tabla = New DataTable("Rp_RceRecetaMedicamento")
                tabla.Columns.Add("codatencion")
                tabla.Columns.Add("codpaciente")
                tabla.Columns.Add("cama")
                tabla.Columns.Add("nombres")
                tabla.Columns.Add("docidentidad")
                tabla.Columns.Add("titular")
                tabla.Columns.Add("fechanacimiento")
                tabla.Columns.Add("edad")
                tabla.Columns.Add("direccion")
                tabla.Columns.Add("telefono")
                tabla.Columns.Add("fechahora")
                tabla.Columns.Add("medicoprocedimiento")
                tabla.Columns.Add("docidentidadmedico")
                tabla.Columns.Add("colmedico")
                tabla.Columns.Add("medtelefono")
                tabla.Columns.Add("Lcodtiporepresentante")
                tabla.Columns.Add("Lnombrerepresentante")
                tabla.Columns.Add("Ltipdocidentidad")
                tabla.Columns.Add("Ldocidentidad")
                tabla.Columns.Add("Lnombretiporepresentante")
                tabla.Columns.Add("Lnombretipdocidentidad")
                tabla.Columns.Add("nombrediag")
                tabla.Columns.Add("nombretratamiento")
                tabla.Columns.Add("nombrecirugia")
                tabla.Columns.Add("plantilla")
                tabla.Columns.Add("Titulo")
                tabla.Columns.Add("Servicio")
                tabla.Columns.Add("Nombre")
                tabla.Columns.Add("FlagDiagnostico")
                tabla.Columns.Add("Diagnostico")
                tabla.Columns.Add("FlagTratamiento")
                tabla.Columns.Add("Tratamiento_Quirurgico")
                tabla.Columns.Add("Molestias")
                tabla.Columns.Add("Riesgos_Complicaciones")
                tabla.Columns.Add("Ventajas_Beneficios")
                tabla.Columns.Add("Riesgos_Personalizados")
                tabla.Columns.Add("Tratamiento_Alternativo")
                tabla.Columns.Add("Riesgos_No_Operacion")
                tabla.Columns.Add("Estado")
                tabla.Columns.Add("nmedico")
                tabla.Columns.Add("RNE")
                tabla.Columns.Add("CMP")

                oHospitalE.CodProcedimiento = Request.Params("Valor").Split(";")(1).ToString().Trim()
                tabla = oHospitalLN.Rp_RceHojadeconsentimiento(oHospitalE)

                Dim TipoReporteCI As String = "CIFormatoRL.rpt"

                If DtCI.Rows(0)(0).ToString().Trim() <> "" Then
                    TipoReporteCI = DtCI.Rows(0)(0).ToString().Trim()
                End If

                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/" + TipoReporteCI + ""))
                crystalreport.SetDataSource(tabla)
                'Sp_CIFormatos_Consulta
                'Rp_RceHojadeconsentimiento
                'FIN - NUEVO CODIGO - 23/01/2020

            ElseIf Request.Params("OP") = "EC" Then 'EVOLUCION CLINICA
                Dim oRceEvolucionLN1 As New RceEvolucionLN()
                Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceEvolucionTableAdapter()
                Dim tablaX1 As New DataTable("Rp_RceEvolucion")
                tablaX1.Columns.Add("ide_historia")
                tablaX1.Columns.Add("ide_evolucion")
                tablaX1.Columns.Add("medico")
                tablaX1.Columns.Add("flg_educacion")
                tablaX1.Columns.Add("flg_informe")
                tablaX1.Columns.Add("TipoEvolucion")
                tablaX1.Columns.Add("txt_detalle")
                tablaX1.Columns.Add("cama")
                tablaX1.Columns.Add("cuarto")
                tablaX1.Columns.Add("dni")
                tablaX1.Columns.Add("fec_nacimiento")
                tablaX1.Columns.Add("fec_ingreso")
                tablaX1.Columns.Add("ape_paterno")
                tablaX1.Columns.Add("ape_materno")
                tablaX1.Columns.Add("nombres")
                tablaX1.Columns.Add("fec_registro")
                tablaX1.Columns.Add("hora_registro")
                'tabla.Columns.Add("firma_medico")
                Dim columna_firma As DataColumn = New DataColumn("firma_medico")
                columna_firma.DataType = System.Type.GetType("System.Byte[]")
                tablaX1.Columns.Add(columna_firma)
                tablaX1.Columns.Add("nmedico")
                tablaX1.Columns.Add("RNE")
                tablaX1.Columns.Add("CMP")
                tablaX1.Columns.Add("codpaciente")
                'INICIO - JB - COMENTADO - 24/09/2019
                'For index = 0 To Reporte1.GetData(Session(sIdeHistoria), oRceEvolucionE.CodigoEvolucion, 1).Rows.Count - 1
                '    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), oRceEvolucionE.CodigoEvolucion, 1).Rows.Item(index).ItemArray)
                'Next
                'FIN - JB - COMENTADO - 24/09/2019

                'INICIO - JB - NUEVO - 24/09/2019
                Dim oRceEvolucionE_ As New RceEvolucionE
                oRceEvolucionE_.IdHistoria = Session(sIdeHistoria)
                oRceEvolucionE_.CodigoEvolucion = 0
                oRceEvolucionE_.Orden = 1
                tablaX1 = oRceEvolucionLN1.Rp_RceEvolucion(oRceEvolucionE_)
                Dim distinctValues = tablaX1.AsEnumerable().[Select](Function(row) New With {
                 Key .ide_evolucion = row.Field(Of Integer)("ide_evolucion"),
                     .ide_historia = row.Field(Of Integer)("ide_historia"),
                     .medico = row.Field(Of String)("medico"),
                     .flg_educacion = row.Field(Of String)("flg_educacion"),
                     .flg_informe = row.Field(Of String)("flg_informe"),
                     .TipoEvolucion = row.Field(Of String)("TipoEvolucion"),
                     .txt_detalle = row.Field(Of String)("txt_detalle"),
                     .cama = row.Field(Of String)("cama"),
                     .cuarto = row.Field(Of String)("cuarto"),
                     .dni = row.Field(Of String)("dni"),
                     .fec_nacimiento = row.Field(Of DateTime)("fec_nacimiento"),
                     .fec_ingreso = row.Field(Of DateTime)("fec_ingreso"),
                     .ape_paterno = row.Field(Of String)("ape_paterno"),
                     .ape_materno = row.Field(Of String)("ape_materno"),
                     .nombres = row.Field(Of String)("nombres"),
                     .fec_registro = row.Field(Of String)("fec_registro"),
                     .hora_registro = row.Field(Of String)("hora_registro"),
                     .firma_medico = row.Field(Of Byte())("firma_medico"),
                     .nmedico = row.Field(Of String)("nmedico"),
                     .RNE = row.Field(Of String)("RNE"),
                     .codpaciente = row.Field(Of String)("codpaciente"),
                .CMP = row.Field(Of String)("CMP")
                }).Distinct()
                'INICIO - JB - NUEVO - 24/09/2019

                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinica2.rpt"))
                crystalreport.SetDataSource(distinctValues)

                'oHospitalE.IdeHistoria = Session(sIdeHistoria)
                'oHospitalE.TipoDoc = 10
                'dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)
            ElseIf Request.Params("OP") = "DA" Then 'DECLARATORIA ALERGIA
                oHospitalE.IdeHistoria = Session(sIdeHistoria)
                oHospitalE.TipoDoc = 9
                dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)
            ElseIf Request.Params("OP") = "ME" Then 'MEDICAMENTOSA
                oHospitalE.IdeHistoria = Session(sIdeHistoria)
                oHospitalE.TipoDoc = 11
                dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE)
            ElseIf Request.Params("OP") = "ANALISISLABORATORIO" Then '1.2
                oHospitalE.IdeGeneral = Request.Params("Valor") '1.2
                oHospitalE.TipoDoc = 13 '1.2
                dt = oHospitalLN.Sp_RceHospitalDoc_Consulta(oHospitalE) '1.2
            ElseIf Request.Params("OP") = "IM" Then
                'Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaMedicamentoTableAdapter()
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
                oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
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
                    ElseIf Request.Params("Valor").Split(";")(0) = "FE2" Then 'JB - 22/06/2020 - NUEVA CONDICION PARA MOSTRAR MEDICAMENTOS DE LA ATENCION POR UNA FECHA ESPECIFICA
                        oRceMedicamentosE.FecReceta = Request.Params("Valor").Split(";")(1).ToString().Trim()
                        oRceMedicamentosE.Orden = 8
                    Else 'si selecciono 'fecha' en el treeview
                        oRceMedicamentosE.FecReceta = Request.Params("Valor").Split(";")(1).ToString().Trim()
                        oRceMedicamentosE.Orden = 2
                    End If
                Else
                    oRceMedicamentosE.Orden = 3
                End If



                tabla = oRceMedicamentosLN.Rp_RceRecetaMedicamento1(oRceMedicamentosE)

                'For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1 'jb cambiar a orden 4 para que no repita(de ser necesario)
                '    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
                'Next
                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpIndicacionMedicaV2.rpt"))
                crystalreport.SetDataSource(tabla)
            ElseIf Request.Params("OP") = "IM2" Or Request.Params("OP") = "IM3" Then 'SIMILAR AL IM, ESTE SE EJECUTA DESDE EL POPUP DE PEDIDO
                'Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaMedicamentoTableAdapter()
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

                tabla.Columns.Add("dsc_dci")
                tabla.Columns.Add("num_dosis")
                tabla.Columns.Add("num_frecuencia")
                tabla.Columns.Add("txt_detalle")

                Dim oRceMedicamentosE As New RceMedicamentosE()
                Dim oRceMedicamentosLN As New RceMedicamentosLN()
                oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
                If Not IsNothing(Request.Params("Valor")) And Request.Params("Valor").Trim() <> "" Then
                    Dim IdRecetaCab As Integer = 0
                    Dim Convertir As Boolean
                    Convertir = Integer.TryParse(Request.Params("Valor").Split(";")(1).ToString().Trim(), IdRecetaCab)
                    If Convertir = True Then
                        oRceMedicamentosE.IdMedicamentosaCab = Request.Params("Valor").Split(";")(1).ToString().Trim()
                    Else
                        oRceMedicamentosE.IdMedicamentosaCab = 0
                        oRceMedicamentosE.FecReceta = Request.Params("Valor").Split(";")(1).ToString().Trim()
                    End If

                    'If Request.Params("Valor").Split(";")(0) = "ID" Then 'si selecciono 'hora' (detalle) en el treeview
                    '    Dim IdRecetaCab As Integer = 0
                    '    Dim Convertir As Boolean
                    '    Convertir = Integer.TryParse(Request.Params("Valor").Split(";")(1).ToString().Trim(), IdRecetaCab)
                    '    If Convertir = True Then
                    '        oRceMedicamentosE.IdMedicamentosaCab = Request.Params("Valor").Split(";")(1).ToString().Trim()
                    '    Else
                    '        oRceMedicamentosE.IdMedicamentosaCab = 0
                    '    End If
                    '    oRceMedicamentosE.Orden = 1
                    'Else 'si selecciono 'fecha' en el treeview
                    '    oRceMedicamentosE.FecReceta = Request.Params("Valor").Split(";")(1).ToString().Trim()
                    '    oRceMedicamentosE.Orden = 2
                    'End If
                End If
                oRceMedicamentosE.Orden = 5
                If Request.Params("OP") = "IM3" Then
                    oRceMedicamentosE.Orden = 6
                End If

                tabla = oRceMedicamentosLN.Rp_RceRecetaMedicamento1(oRceMedicamentosE)

                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpIndicacionMedicaV2.rpt"))
                crystalreport.SetDataSource(tabla)
            ElseIf Request.Params("OP") = "IN" Then
                Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceInterconsultaTableAdapter()
                tabla = New DataTable("Rp_RceInterconsulta")
                tabla.Columns.Add("hc")
                tabla.Columns.Add("ide_interconsulta")
                tabla.Columns.Add("ape_paterno")
                tabla.Columns.Add("ape_materno")
                tabla.Columns.Add("nombre")
                tabla.Columns.Add("cuarto")
                tabla.Columns.Add("cama")
                tabla.Columns.Add("dsc_de")
                tabla.Columns.Add("dsc_a")
                tabla.Columns.Add("fec_solicita")
                tabla.Columns.Add("hora_solicita")
                tabla.Columns.Add("dsc_solicita")
                Dim columna_firma1 As DataColumn = New DataColumn("firma_solicita")
                columna_firma1.DataType = System.Type.GetType("System.Byte[]")
                tabla.Columns.Add(columna_firma1)
                tabla.Columns.Add("fec_responde")
                tabla.Columns.Add("hora_responde")
                tabla.Columns.Add("dsc_responde")
                Dim columna_firma As DataColumn = New DataColumn("firma_responde")
                columna_firma.DataType = System.Type.GetType("System.Byte[]")
                tabla.Columns.Add(columna_firma)
                tabla.Columns.Add("nmedico1")
                tabla.Columns.Add("RNE1")
                tabla.Columns.Add("CMP1")
                tabla.Columns.Add("nmedico2")
                tabla.Columns.Add("RNE2")
                tabla.Columns.Add("CMP2")
                For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
                    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
                Next
                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpInterconsulta.rpt"))
                crystalreport.SetDataSource(tabla)
            ElseIf Request.Params("OP") = "EP" Then
                Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceInformeMedicoTableAdapter()
                tabla = New DataTable("Rp_RceInformeMedico")
                'INICIO - JB - COMENTADO
                'tabla.Columns.Add("ide_historia")
                'tabla.Columns.Add("cod_atencion")
                'tabla.Columns.Add("npaciente")
                'tabla.Columns.Add("edad")
                'tabla.Columns.Add("dni_paciente")
                'tabla.Columns.Add("telefono")
                'tabla.Columns.Add("direccion")
                'tabla.Columns.Add("medico")
                'tabla.Columns.Add("aseguradora")
                'tabla.Columns.Add("contratante")
                'tabla.Columns.Add("poliza")
                'tabla.Columns.Add("tarifa")
                'tabla.Columns.Add("fecha_atencion")
                'tabla.Columns.Add("diasatencion")
                'tabla.Columns.Add("condicionalta")
                'tabla.Columns.Add("tipoalta")
                'tabla.Columns.Add("necropsia")
                'tabla.Columns.Add("patologia")
                'tabla.Columns.Add("enf_actual")
                'tabla.Columns.Add("ef_FC")
                'tabla.Columns.Add("ef_FE")
                'tabla.Columns.Add("ef_PA")
                'tabla.Columns.Add("ef_TEMP")
                'tabla.Columns.Add("ef_PESO")
                'tabla.Columns.Add("ef_TALLA")
                'tabla.Columns.Add("ef_IMC")
                'tabla.Columns.Add("ev_s")
                'tabla.Columns.Add("ev_o")
                'tabla.Columns.Add("ev_a")
                'tabla.Columns.Add("ev_p")
                'tabla.Columns.Add("ev_tipo")
                'tabla.Columns.Add("diagnostico")
                'tabla.Columns.Add("cie10")
                'tabla.Columns.Add("nmedico")
                'tabla.Columns.Add("RNE")
                'tabla.Columns.Add("CMP")
                'For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0).Rows.Count - 1
                '    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0).Rows.Item(index).ItemArray)
                'Next
                'crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpEpicrisis.rpt"))
                'crystalreport.SetDataSource(tabla)
                'FIN - JB - COMENTADO

                tabla.Columns.Add("ide_historia")
                tabla.Columns.Add("cod_atencion")
                tabla.Columns.Add("npaciente")
                tabla.Columns.Add("edad")
                tabla.Columns.Add("dni_paciente")
                tabla.Columns.Add("telefono")
                tabla.Columns.Add("direccion")
                tabla.Columns.Add("medico")
                tabla.Columns.Add("aseguradora")
                tabla.Columns.Add("contratante")
                tabla.Columns.Add("poliza")
                tabla.Columns.Add("tarifa")
                tabla.Columns.Add("fecha_atencion")
                tabla.Columns.Add("diasatencion")
                tabla.Columns.Add("condicionalta")
                tabla.Columns.Add("diagnostico")
                tabla.Columns.Add("cie10")
                tabla.Columns.Add("TipoES")
                tabla.Columns.Add("TipoDiagnostico")
                tabla.Columns.Add("nmedico")
                tabla.Columns.Add("RNE")
                tabla.Columns.Add("CMP")
                tabla.Columns.Add("DiasHospitalizados")
                tabla.Columns.Add("EnfermedadActual")
                tabla.Columns.Add("ExamenFisico")
                tabla.Columns.Add("PresionArterial")
                tabla.Columns.Add("Talla")
                tabla.Columns.Add("FrecuenciaCardiaca")
                tabla.Columns.Add("FrecuenciaRespiratoria")
                tabla.Columns.Add("Peso")
                tabla.Columns.Add("ExamenAuxiliar")
                tabla.Columns.Add("Evolucion")
                tabla.Columns.Add("Tratamiento")
                tabla.Columns.Add("Observacion")
                tabla.Columns.Add("Necropsia")
                tabla.Columns.Add("especialidad")
                tabla.Columns.Add("fec_fin")
                tabla.Columns.Add("codpaciente") 'JB - nuevo campo - 06/10/2020

                Dim columna_firma1 As DataColumn = New DataColumn("firma_medico")
                columna_firma1.DataType = System.Type.GetType("System.Byte[]")
                tabla.Columns.Add(columna_firma1)


                oHospitalE.IdeHistoria = Session(sIdeHistoria)
                oHospitalE.Orden = 2
                tabla = oHospitalLN.Rp_RceInformeMedico(oHospitalE)
                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpAltaMedicaEpicrisis.rpt"))
                crystalreport.SetDataSource(tabla)

                Dim TablaSubReporte1, TablaSubReporte2 As New DataTable()
                TablaSubReporte1.Columns.Add("DescripcionCampo")
                TablaSubReporte1.Columns.Add("ValorRadio")
                TablaSubReporte1.Columns.Add("ValorCampo1")
                TablaSubReporte1.Columns.Add("ValorCampo2")

                TablaSubReporte2.Columns.Add("codatencion")
                TablaSubReporte2.Columns.Add("tipo")
                TablaSubReporte2.Columns.Add("coddiagnostico")
                TablaSubReporte2.Columns.Add("tipodiagnostico")
                TablaSubReporte2.Columns.Add("clasificaciondiagnostico")
                TablaSubReporte2.Columns.Add("fec_registro")
                TablaSubReporte2.Columns.Add("nombre")
                TablaSubReporte2.Columns.Add("nombretipo")
                TablaSubReporte2.Columns.Add("nombreclasificacion")


                'INICIO - JB - CAPTURANDO LOS ANTECEDENTES 'SI'
                Dim oRceExamenfisicoMaeE_ As New RceAnamnesisE()
                Dim oRceExamenFisicoMaeLN_ As New RceAnamnesisLN()
                oRceExamenfisicoMaeE_.IdeExamenFisico = 110 'para antecedentes
                oRceExamenfisicoMaeE_.IdeHistoria = Session(sIdeHistoria)
                oRceExamenfisicoMaeE_.IdeTipoAtencion = Session(sCodigoAtencion).ToString().Substring(0, 1)
                oRceExamenfisicoMaeE_.CodMedico = Session(sCodMedico)
                oRceExamenfisicoMaeE_.FlgEstado = "A"
                oRceExamenfisicoMaeE_.Orden = 4
                Dim dtx, dt2 As New DataTable()
                dtx = oRceExamenFisicoMaeLN_.Sp_RceExamenFisicoMae_Consulta5(oRceExamenfisicoMaeE_)
                Dim agregar As Boolean = False

                If dtx.Rows.Count > 0 Then
                    For index = 0 To dtx.Rows.Count - 1
                        oRceExamenfisicoMaeE_.IdeExamenFisico = dtx.Rows(index)("ide_examenfisico").ToString()
                        oRceExamenfisicoMaeE_.Orden = 5
                        dt2 = oRceExamenFisicoMaeLN_.Sp_RceExamenFisicoMae_Consulta5(oRceExamenfisicoMaeE_)

                        If dt2.Rows.Count > 0 Then
                            For index2 = 0 To dt2.Rows.Count - 1 'si hay detalles...
                                If dt2.Rows(index2)("txt_detalle").ToString().ToUpper().Contains("SI") And dt2.Rows(index2)("dsc_txtidcampo").ToString().ToUpper().Contains("SI") And
                                    dt2.Rows(index2)("valor_control").ToString().Trim() = "1" Then 'si esta marcado el *si*
                                    agregar = True 'los valores del detalle se agregaran a la tabla que se devolvera
                                End If
                            Next
                            If agregar = True Then
                                For index2 = 0 To dt2.Rows.Count - 1 'agregando valores a la tabla
                                    If dt2.Rows(index2)("est_tipomedida").ToString() <> "I" Then
                                        If dt2.Rows.Count = 3 Then
                                            TablaSubReporte1.Rows.Add(dtx.Rows(index)("txt_detalle").ToString().Trim(), "Si", dt2.Rows(index2 + 2)("valor_control").ToString().Trim(), "")
                                            Exit For
                                        End If
                                        If dt2.Rows.Count = 4 Then
                                            Dim valor_final As String = ""
                                            If dt2.Rows(index2)("valor_control").ToString().Trim() = "1" Then
                                                valor_final = dt2.Rows(index2 + 2)("valor_control").ToString().Trim()
                                            Else
                                                valor_final = ""
                                            End If


                                            TablaSubReporte1.Rows.Add(dtx.Rows(index)("txt_detalle").ToString().Trim(), "Si", valor_final, "") 'dt2.Rows(index2 + 1)("valor_control").ToString().Trim(), dt2.Rows(index2 + 2)("valor_control").ToString().Trim()
                                            Exit For
                                        End If
                                        If dt2.Rows.Count = 5 Then
                                            TablaSubReporte1.Rows.Add(dtx.Rows(index)("txt_detalle").ToString().Trim(), "Si", dt2.Rows(index2 + 2)("valor_control").ToString().Trim(), dt2.Rows(index2 + 3)("valor_control").ToString().Trim())
                                            Exit For
                                        End If
                                    End If
                                Next
                            End If
                        End If
                        agregar = False
                    Next
                End If
                'FIN - JB - CAPTURANDO LOS ANTECEDENTES 'SI'


                crystalreport.Subreports("RpAltaMedicaEpicrisisAntecedentes.rpt").SetDataSource(TablaSubReporte1)


                Dim oRceDiagnosticoE As New RceDiagnosticoE()
                Dim oRceDiagnosticoLN As New RceDiagnosticoLN()
                oRceDiagnosticoE.Tipo = "A"
                oRceDiagnosticoE.CodAtencion = Session(sCodigoAtencion)
                Dim ds As New DataSet()
                TablaSubReporte2 = oRceDiagnosticoLN.Sp_Diagxhospital_Consulta1(oRceDiagnosticoE)

                If TablaSubReporte2.Rows.Count > 0 Then

                    For index = 0 To TablaSubReporte2.Rows.Count - 1

                        If TablaSubReporte2.Rows(index)("tipo") = "I" Then
                            TablaSubReporte2.Rows(index)("tipo") = "ENTRADA"
                        Else
                            TablaSubReporte2.Rows(index)("tipo") = "SALIDA"
                        End If

                        If TablaSubReporte2.Rows(index)("tipodiagnostico") = "P" Then
                            TablaSubReporte2.Rows(index)("tipodiagnostico") = "P - PRESUNTIVO"
                        ElseIf TablaSubReporte2.Rows(index)("tipodiagnostico") = "R" Then
                            TablaSubReporte2.Rows(index)("tipodiagnostico") = "R - REPETIDO"
                        Else
                            TablaSubReporte2.Rows(index)("tipodiagnostico") = "D - DEFINITIVO"
                        End If

                    Next

                End If
                crystalreport.Subreports("RpAltaMedicaEpicrisisDiagnostico.rpt").SetDataSource(TablaSubReporte2)
                'TablaSubReporte2

            ElseIf Request.Params("OP") = "RA" Then 'RECETA DE ALTA
                Dim _parameter As List(Of ParametroE) = New List(Of ParametroE)()
                Dim rptLogConsultaReportesE As RptLogConsultaReportesE = New RptLogConsultaReportesE()

                'Lista
                _parameter.Add(New ParametroE("@ide_historia", Session(sIdeHistoria), ""))
                _parameter.Add(New ParametroE("@ide_receta", 0, ""))
                _parameter.Add(New ParametroE("@orden", 4, ""))
                _parameter.Add(New ParametroE("@codmedico", "00000590", "")) 'MBARDALES - 30/05/2024
                _parameter.Add(New ParametroE("@codigo", Session(sCodigoAtencion), "RpRecetaAltaDiag.rpt"))
                _parameter.Add(New ParametroE("@tipo", "S", "RpRecetaAltaDiag.rpt"))

                _parameter.Add(New ParametroE("@coddiagnostico", "", "RpRecetaAltaDiag.rpt"))
                'Fin Lista
                '
                Dim _rptInformesMaeE As RptInformesMaeE = New RptInformesMaeE()
                _rptInformesMaeE.ide_informe = 8
                '
                rptLogConsultaReportesE.dsc_archivo_rpt = "RpRecetaAlta.rpt"
                rptLogConsultaReportesE.tip_reporte = "pdf"
                rptLogConsultaReportesE.ide_usuario = Integer.Parse(Session(sCodUser))
                rptLogConsultaReportesE.dsc_login = Session("NombreUsuario")
                rptLogConsultaReportesE.dsc_sistema = "WEB HCE"
                rptLogConsultaReportesE.rptInformesMaeE = _rptInformesMaeE
                rptLogConsultaReportesE.Parametros = _parameter

                _reporteApi = ObtenerReporteApiReporteria(rptLogConsultaReportesE)


                'Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaAltaTableAdapter()
                'tabla = New DataTable("Rp_RceRecetaTratamiento")
                'tabla.Columns.Add("ide_medicamentorec")
                'tabla.Columns.Add("ide_receta")
                'tabla.Columns.Add("dsc_paciente")

                'tabla.Columns.Add("tip_documento")
                'tabla.Columns.Add("nro_identidad")
                'tabla.Columns.Add("sexo")
                'tabla.Columns.Add("Edad")
                'tabla.Columns.Add("cod_atencion")
                'tabla.Columns.Add("cod_paciente")
                'tabla.Columns.Add("fec_inicio")
                'tabla.Columns.Add("fec_fin")
                'tabla.Columns.Add("Aseguradora")

                'tabla.Columns.Add("flg_asegurado")
                'tabla.Columns.Add("dsc_alergias")
                'tabla.Columns.Add("dsc_medico")
                'tabla.Columns.Add("telefono")
                'tabla.Columns.Add("fec_receta")
                'tabla.Columns.Add("fec_cita")
                'tabla.Columns.Add("cama")
                'tabla.Columns.Add("Rp")
                'tabla.Columns.Add("dsc_diagnostico")
                'tabla.Columns.Add("dsc_indicaciones")
                'tabla.Columns.Add("dsc_medicamento")
                'tabla.Columns.Add("dsc_dci")
                'tabla.Columns.Add("dsc_presentacion")
                'tabla.Columns.Add("cantidad")
                'tabla.Columns.Add("tiempo")
                'Dim columna_firma As DataColumn = New DataColumn("firma_medico")
                'columna_firma.DataType = System.Type.GetType("System.Byte[]")
                'tabla.Columns.Add(columna_firma)
                'tabla.Columns.Add("nmedico")
                'tabla.Columns.Add("RNE")
                'tabla.Columns.Add("CMP")
                'tabla.Columns.Add("Vigencia")
                'tabla.Columns.Add("Via")
                ''For index = 0 To Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
                ''    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
                ''Next

                'Dim oRceMedicamentosE As New RceMedicamentosE()
                'Dim oRceMedicamentosLN As New RceMedicamentosLN()
                'oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
                'oRceMedicamentosE.IdMedicamentosaCab = 0
                'oRceMedicamentosE.Orden = 4
                'tabla = oRceMedicamentosLN.Rp_RceRecetaTratamiento(oRceMedicamentosE)

                'crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpRecetaAlta.rpt"))
                'crystalreport.SetDataSource(tabla)

                ''DIAGNOSTICOS
                ''Dim Reporte2 As New dsAmbulatorio_ReporteTableAdapters.Sp_Diagxhospital_ConsultaTableAdapter()
                'Dim dt7 As New DataTable()
                'dt7 = New DataTable("Sp_Diagxhospital_Consulta")
                'dt7.Columns.Add("codatencion")
                'dt7.Columns.Add("tipo")
                'dt7.Columns.Add("coddiagnostico")
                'dt7.Columns.Add("tipodiagnostico")
                'dt7.Columns.Add("clasificaciondiagnostico")
                'dt7.Columns.Add("fec_registro")
                'dt7.Columns.Add("nombre")
                'dt7.Columns.Add("nombretipo")
                'dt7.Columns.Add("nombreclasificacion")

                'Dim oDiagnosticoxHospitalLN_ As New RceDiagnosticoLN()
                'Dim oDiagnosticoxHospitalE_ As New RceDiagnosticoE()
                'oDiagnosticoxHospitalE_.CodAtencion = Session(sCodigoAtencion)
                'oDiagnosticoxHospitalE_.Tipo = "S"
                'oDiagnosticoxHospitalE_.CodDiagnostico = ""
                'dt7 = oDiagnosticoxHospitalLN_.Rp_Diagxhospital_Consulta(oDiagnosticoxHospitalE_)
                'crystalreport.Subreports("RpRecetaAltaDiag.rpt").SetDataSource(dt7)


                ''OTRAS RECOMENDACIONES
                'Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
                'Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()
                ''Dim Reporte3 As New dsAmbulatorio_ReporteTableAdapters.Sp_RceRecetaMedicamentoCab_ConsultaTableAdapter()
                'Dim dt8 As New DataTable()
                'dt8 = New DataTable("Sp_RceRecetaMedicamentoCab_Consulta")
                'dt8.Columns.Add("ide_receta")
                'dt8.Columns.Add("ide_medicamentorec")
                'dt8.Columns.Add("cod_medico")
                'dt8.Columns.Add("fec_registra")
                'dt8.Columns.Add("nmedico")
                'dt8.Columns.Add("flg_verificar")
                'dt8.Columns.Add("otras_recomendaciones")

                ''For index = 0 To Reporte3.GetData(Session(sIdeHistoria), 0, 1).Rows.Count - 1
                ''    dt8.Rows.Add(Reporte3.GetData(Session(sIdeHistoria), 0, 1).Rows.Item(index).ItemArray)
                ''Next
                'oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
                'oRceRecetaMedicamentoE.IdRecetaDet = 0
                'oRceRecetaMedicamentoE.Orden = 1
                'dt8 = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
                ''dt8 = EliminarRegistroVacio(dt8, "otras_recomendaciones", "ide_receta") 'JB - nueva linea de codigo para eliminar registros vacios - 12/07/2018
                'crystalreport.Subreports("RpRecetaAltaRecomendaciones.rpt").SetDataSource(dt8)



            ElseIf Request.Params("OP") = "RA2" Then 'JB - 08/06/2020 - IMPRESO DESDE EL POPUP DE PEDIDO
                Dim Reporte1 As New dsHospital_ReporteTableAdapters.Rp_RceRecetaAltaTableAdapter()
                tabla = New DataTable("Rp_RceRecetaAlta")
                tabla.Columns.Add("ide_medicamentorec")
                tabla.Columns.Add("ide_receta")
                tabla.Columns.Add("dsc_paciente")
                tabla.Columns.Add("flg_asegurado")
                tabla.Columns.Add("flg_alergias")
                tabla.Columns.Add("dsc_alergias")
                tabla.Columns.Add("dsc_medico")
                tabla.Columns.Add("telefono")
                tabla.Columns.Add("fec_receta")
                tabla.Columns.Add("fec_cita")
                tabla.Columns.Add("cama")
                tabla.Columns.Add("Rp")
                tabla.Columns.Add("dsc_diagnostico")
                tabla.Columns.Add("dsc_indicaciones")
                tabla.Columns.Add("dsc_medicamento")
                tabla.Columns.Add("dsc_dci")
                tabla.Columns.Add("dsc_presentacion")
                tabla.Columns.Add("cantidad")
                tabla.Columns.Add("tiempo")
                Dim columna_firma As DataColumn = New DataColumn("firma_medico")
                columna_firma.DataType = System.Type.GetType("System.Byte[]")
                tabla.Columns.Add(columna_firma)
                tabla.Columns.Add("nmedico")
                tabla.Columns.Add("RNE")
                tabla.Columns.Add("CMP")
                'For index = 0 To Reporte1.GetData(Session(sIdeHistoria), Request.Params("Valor"), 2).Rows.Count - 1
                '    tabla.Rows.Add(Reporte1.GetData(Session(sIdeHistoria), Request.Params("Valor"), 2).Rows.Item(index).ItemArray)
                'Next
                Dim oRceMedicamentosE As New RceMedicamentosE()
                Dim oRceMedicamentosLN As New RceMedicamentosLN()
                oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
                oRceMedicamentosE.IdMedicamentosaCab = Request.Params("Valor")
                oRceMedicamentosE.Orden = 2
                tabla = oRceMedicamentosLN.Rp_RceRecetaAlta(oRceMedicamentosE)

                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpRecetaAlta.rpt"))
                crystalreport.SetDataSource(tabla)
            ElseIf Request.Params("OP") = "JM" Then
                tabla = New DataTable("Rp_JuntaMedica")
                tabla.Columns.Add("ide_juntamedica")
                tabla.Columns.Add("cod_atencion")
                tabla.Columns.Add("cod_medico")
                tabla.Columns.Add("dsc_juntamedica")
                tabla.Columns.Add("fec_registra")
                tabla.Columns.Add("ape_paterno")
                tabla.Columns.Add("ape_materno")
                tabla.Columns.Add("nom_paciente")
                tabla.Columns.Add("nom_medico")
                tabla.Columns.Add("cama")
                tabla.Columns.Add("cod_paciente")

                tabla.Columns.Add("dni")
                tabla.Columns.Add("fec_nacimiento")
                tabla.Columns.Add("fec_ingreso")
                tabla.Columns.Add("RNE")
                tabla.Columns.Add("CMP")
                Dim columna_firma As DataColumn = New DataColumn("firma_medico")
                columna_firma.DataType = System.Type.GetType("System.Byte[]")
                tabla.Columns.Add(columna_firma)

                Dim oRceJuntaMedicaE As New RceJuntaMedicaE()
                Dim oRceJuntaMedicaLN As New RceJuntaMedicaLN()
                oRceJuntaMedicaE.CodAtencion = Session(sCodigoAtencion)
                oRceJuntaMedicaE.Orden = 1
                tabla = oRceJuntaMedicaLN.Rp_JuntaMedica(oRceJuntaMedicaE)

                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpJuntaMedica.rpt"))
                crystalreport.SetDataSource(tabla)
            ElseIf Request.Params("OP") = "NI" Then
                tabla = New DataTable("Rp_NotaIngreso")
                tabla.Columns.Add("ide_notaingreso")
                tabla.Columns.Add("ide_historia")
                tabla.Columns.Add("cod_medico")
                tabla.Columns.Add("dsc_medico_tratante")
                tabla.Columns.Add("dsc_intensivista")
                tabla.Columns.Add("fec_registra")
                tabla.Columns.Add("tipo")
                tabla.Columns.Add("dsc_notaingreso")

                tabla.Columns.Add("ape_paterno")
                tabla.Columns.Add("ape_materno")
                tabla.Columns.Add("nom_paciente")
                tabla.Columns.Add("nom_medico")
                tabla.Columns.Add("cama")
                tabla.Columns.Add("cod_paciente")

                tabla.Columns.Add("dni")
                tabla.Columns.Add("fec_nacimiento")
                tabla.Columns.Add("fec_ingreso")
                tabla.Columns.Add("RNE")
                tabla.Columns.Add("CMP")
                Dim columna_firma As DataColumn = New DataColumn("firma_medico")
                columna_firma.DataType = System.Type.GetType("System.Byte[]")
                tabla.Columns.Add(columna_firma)

                Dim oRceNotaIngresoE As New RceNotaIngresoE()
                Dim oRceNotaIngresoLN As New RceNotaIngresoLN()
                oRceNotaIngresoE.IdHistoria = Session(sIdeHistoria)
                oRceNotaIngresoE.Orden = 1
                tabla = oRceNotaIngresoLN.Rp_NotaIngreso(oRceNotaIngresoE)

                crystalreport.Load(Server.MapPath("~/Intranet/Reporte/RpNotaIngreso.rpt"))
                crystalreport.SetDataSource(tabla)
                '1.1 INI
            ElseIf Request.Params("OP") = "HC" Then 'HISTORIA CLINICA
                Dim _parameter As List(Of ParametroE) = New List(Of ParametroE)()
                Dim rptLogConsultaReportesE As RptLogConsultaReportesE = New RptLogConsultaReportesE()

                'Lista
                _parameter.Add(New ParametroE("@cod_atencion", Session(sCodigoAtencion), ""))
                _parameter.Add(New ParametroE("@orden", 1, ""))
                'Fin Lista
                '
                Dim _rptInformesMaeE As RptInformesMaeE = New RptInformesMaeE()
                _rptInformesMaeE.ide_informe = 32
                '
                rptLogConsultaReportesE.dsc_archivo_rpt = "RpHistoriaClinicaHospital.rpt"
                rptLogConsultaReportesE.tip_reporte = "pdf"
                rptLogConsultaReportesE.ide_usuario = Integer.Parse(Session(sCodUser))
                rptLogConsultaReportesE.dsc_login = Session("NombreUsuario")
                rptLogConsultaReportesE.dsc_sistema = "WEB HCE"
                rptLogConsultaReportesE.rptInformesMaeE = _rptInformesMaeE
                rptLogConsultaReportesE.Parametros = _parameter

                _reporteApi = ObtenerReporteApiReporteria(rptLogConsultaReportesE)
                '1.1 FIN
            End If

            If Request.Params("OP") = "CI" Then 'JB - Or Request.Params("OP") = "EC" - 22/04/2019 - EC ahora solo sera todo en un reporte. PARA LOS CASOS DE EVOLUCION CLINICA/CONSENTIMIENTO INFORMADO HABRA MAS DE 1 REPORTE EN PDF
                'INICIO - JB  - COMENTADO - 23/01/2020
                'For index = 0 To dt.Rows.Count - 1
                '    If Not IsNothing(Request.Params("Valor")) Then
                '        If dt.Rows(index)("id_documento").ToString().Trim() = Request.Params("Valor") Then
                '            Dim docu As Byte()
                '            Response.Clear()
                '            Response.Buffer = True
                '            Response.Charset = ""
                '            Response.Cache.SetCacheability(HttpCacheability.NoCache)
                '            Response.ContentType = "application/pdf"
                '            'Response.AddHeader("Content-disposition", "attachment; filename=" & dt.Rows(index)("descripcion_doc").ToString() + ".pdf") esta linea es para descargar archivo y no visualizar
                '            docu = DirectCast(dt.Rows(index)("bib_documento"), Byte())
                '            Response.AddHeader("content-length", docu.Length.ToString()) 'esta linea es para que se visualize el archivo y no se descargue
                '            Response.BinaryWrite(docu)
                '            Response.Flush()
                '            'Response.Close()
                '        End If
                '    End If
                'Next
                'FIN - JB - COMENTADO - 23/01/2020

                'INICIO - JB - NUEVO CODIGO - 23/01/2020
                'PREPERANDO EXPORTACION DE REPORTE A PDF
                Dim OpcionExportar As ExportOptions
                Dim OpcionDestino As New DiskFileDestinationOptions()
                Dim OpcionesFormato As New PdfRtfWordFormatOptions()
                xNombreArchivo = Request.Params("OP") + Session(sIdeHistoria).ToString() + ".pdf"
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

                System.IO.File.Delete(xRuta + "\" + xNombreArchivo)
                'FIN - JB - NUEVO CODIGO - 23/01/2020
            ElseIf Request.Params("OP") = "DA" Then
                Dim docu As Byte()
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = "application/pdf"
                docu = DirectCast(dt.Rows(dt.Rows.Count - 1)("bib_documento"), Byte()) 'dt.Rows.Count - 1 -> el ultimo registro
                Response.AddHeader("content-length", docu.Length.ToString())
                Response.BinaryWrite(docu)
                Response.Flush()
                'Response.Close()
                'INI 1.2
            ElseIf Request.Params("OP") = "ANALISISLABORATORIO" Then
                Dim docu As Byte()
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = "application/pdf"
                docu = DirectCast(dt.Rows(0)("blb_resultado"), Byte())
                Response.AddHeader("content-length", docu.Length.ToString())
                Response.BinaryWrite(docu)
                Response.Flush()
                'FIN 1.2
            ElseIf Request.Params("OP") = "ME" Then 'Or Request.Params("OP") = "EC" 23/09/2020 comentado
                Dim docu As Byte()
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = "application/pdf"
                docu = DirectCast(dt.Rows(0)("bib_documento"), Byte())
                Response.AddHeader("content-length", docu.Length.ToString())
                Response.BinaryWrite(docu)
                Response.Flush()
                'Response.Close()
            ElseIf Request.Params("OP") = "RA" Then

                ''PREPERANDO EXPORTACION DE REPORTE A PDF
                'Dim OpcionExportar As ExportOptions
                'Dim OpcionDestino As New DiskFileDestinationOptions()
                'Dim OpcionesFormato As New PdfRtfWordFormatOptions()
                'xNombreArchivo = Request.Params("OP") + Session(sIdeHistoria).ToString() + ".pdf"
                'OpcionDestino.DiskFileName = xRuta + "\" + xNombreArchivo
                'OpcionExportar = crystalreport.ExportOptions
                'With OpcionExportar
                '    .ExportDestinationType = ExportDestinationType.DiskFile
                '    .ExportFormatType = ExportFormatType.PortableDocFormat
                '    .DestinationOptions = OpcionDestino
                '    .FormatOptions = OpcionesFormato
                'End With
                ''EXPORTANDO A PDF
                'crystalreport.Export()
                ''CONVIRTIENDO ARCHIVO PDF GENERADO EN BYTE()
                'Dim pdf_byte As Byte() = System.IO.File.ReadAllBytes(xRuta + "\" + xNombreArchivo)
                'System.IO.File.WriteAllBytes(xRuta + "\" + xNombreArchivo, pdf_byte)

                'MOSTRANDO ARCHIVO BYTE() DEL PDF EN PANTALLA
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", _reporteApi.ArchivoByte.Length.ToString())
                Response.BinaryWrite(_reporteApi.ArchivoByte)
                Response.Flush()

                'ELIMINANDO ARCHIVO
                'System.IO.File.Delete(xRuta + "\" + xNombreArchivo)
                '1.1 INI
            ElseIf Request.Params("OP") = "HC" Then
                'MOSTRANDO ARCHIVO BYTE() DEL PDF EN PANTALLA
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", _reporteApi.ArchivoByte.Length.ToString())
                Response.BinaryWrite(_reporteApi.ArchivoByte)
                Response.Flush()
                '1.1 FIN
                '1.1 INI
            ElseIf Request.Params("OP") = "IM" Or Request.Params("OP") = "EC" Or Request.Params("OP") = "IN" Or Request.Params("OP") = "EP" Or Request.Params("OP") = "RA" Or Request.Params("OP") = "IM2" Or Request.Params("OP") = "IM3" Or Request.Params("OP") = "RA2" Or Request.Params("OP") = "JM" Or Request.Params("OP") = "NI" Or Request.Params("OP") = "HC" Then
                '1.1 FIN
                'PREPERANDO EXPORTACION DE REPORTE A PDF
                Dim OpcionExportar As ExportOptions
                Dim OpcionDestino As New DiskFileDestinationOptions()
                Dim OpcionesFormato As New PdfRtfWordFormatOptions()
                xNombreArchivo = Request.Params("OP") + Session(sIdeHistoria).ToString() + ".pdf"
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
                        Dim oRceMedicamentosE As New RceMedicamentosE()
                        Dim oRceMedicamentosLN As New RceMedicamentosLN()
                        oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
                        oRceMedicamentosE.Detalle = ""
                        oRceMedicamentosE.CodUser = Session(sCodUser)
                        oRceMedicamentosE.TipoDocumento = 2
                        oRceMedicamentosE.Documento = pdf_byte
                        oRceMedicamentosE.Estado = "A"
                        oRceMedicamentosE.Codigo = Session(sCodigoAtencion)
                        oRceMedicamentosE.FecReporte = DateTime.Parse(tabla.Rows(0)("fec_registra2"))

                        oRceMedicamentosLN.Sp_RceResultadoDocumentoDet_InsertV3(oRceMedicamentosE)
                    End If
                End If

                'ELIMINANDO ARCHIVO
                System.IO.File.Delete(xRuta + "\" + xNombreArchivo)
            End If
        End If
        crystalreport.Close()
        crystalreport.Dispose()
        'FIN - JB - 30/01/2017

    End Sub

    Public Sub OtrosReportes()
        If Request.Params("OP") = "INFORME_PATOLOGIA" Then 'RECETA DE ALTA
            If Request.Params("Valor") <> "" Then
                Dim dt As New DataTable()
                Dim oRcePatologiaDetE As New RcePatologiaDetE
                Dim oRcePatologiaDetLN As New RcePatologiaDetLN()
                oRcePatologiaDetE.Orden = 3
                oRcePatologiaDetE.CodAtencion = Request.Params("Valor")
                dt = oRcePatologiaDetLN.Sp_RceResultadoDocumentoDet_Consulta(oRcePatologiaDetE)

                Dim pdf_byte As Byte()
                pdf_byte = DirectCast(dt(0).Item("blb_archivo"), Byte())
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", pdf_byte.Length.ToString())
                Response.BinaryWrite(pdf_byte)
                Response.Flush()
            End If
        End If
    End Sub

    Private Function ObtenerReporteApiReporteria(ByVal rptLogConsultaReportesE As RptLogConsultaReportesE)
        Dim _rutaApi As String = ConfigurationManager.AppSettings("RutaApiReporteria")
        Dim _resultado As New RespuestaArchivoJsonE()
        Try
            'Dim JsonString As String = JsonConvert.SerializeObject(rptLogConsultaReportesE)
            Dim _client As RestClient = New RestClient(_rutaApi)
            Dim _request As RestRequest = New RestRequest()
            '_request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.POST
            _request.AddHeader("Accept", "application/json")
            _request.AddJsonBody(rptLogConsultaReportesE)
            Dim _Response As RestResponse = _client.Execute(_request)
            _resultado = JsonConvert.DeserializeObject(Of RespuestaArchivoJsonE)(_Response.Content)

            Return _resultado
        Catch
            Return _resultado
        End Try

    End Function


End Class