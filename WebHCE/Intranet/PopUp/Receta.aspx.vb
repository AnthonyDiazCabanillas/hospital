Imports Entidades.ControlClinicoE
Imports LogicaNegocio.ControlClinicoLN
Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN
Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN
Imports CrystalDecisions.CrystalReports.Engine
Imports Entidades.MedicamentosE
Imports LogicaNegocio.MedicamentosLN
Imports Entidades
Imports Newtonsoft.Json
Imports RestSharp

Public Class Receta
    Inherits System.Web.UI.Page
    Dim oRceRecetaMedicamentoE As New RceRecetaMedicamentoE()
    Dim oRceRecetaMedicamentoLN As New RceRecetaMedicamentoLN()
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()
    Dim oRceDiagnosticoE As New RceDiagnosticoE()
    Dim oRceDiagnosticoLN As New RceDiagnosticoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'divDiagnosticoReceta
            Dim tabla As New DataTable()
            oRceDiagnosticoE.IdeHistoria = Session(sIdeHistoria)
            oRceDiagnosticoE.Tipo = "S"
            oRceDiagnosticoE.Orden = 1
            tabla = oRceDiagnosticoLN.Sp_RceDiagnostico_Consulta(oRceDiagnosticoE)
            If tabla.Rows.Count > 0 Then
                divDiagnosticoReceta.InnerHtml = tabla.Rows(0)(0).ToString()
            End If

            'Sp_RceAlergia_Consulta 5772,2
            oRceAlergiaE.IdHistoria = Session(sIdeHistoria)
            oRceAlergiaE.Orden = 2
            tabla = oRceAlergiaLN.Sp_RceAlergia_Consulta(oRceAlergiaE)
            If tabla.Rows.Count > 0 Then
                divAlergiaReceta.InnerHtml = tabla.Rows(0)("resumen_alergia").ToString()
            End If
            ListarRecetas()
            ListarViasRecetaAlta()
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerFechaVigencia() As String
        Dim pagina As New Receta()
        Return pagina.ObtenerFechaVigencia_()
    End Function

    Public Function ObtenerFechaVigencia_() As String
        Try
            'Dim ie
            'ie = CreateObject("internetexplorer.application")
            'ie.Navigate("http://www.google.com")
            'ie.Visible = True

            oTablasE.CodTabla = "HCE_RECETA_VIGENCIA"
            oTablasE.Buscar = ""
            oTablasE.Key = 50
            oTablasE.NumeroLineas = 1
            oTablasE.Orden = -1
            Dim tabla As New DataTable()
            tabla = oTablasLN.Sp_Tablas_Consulta(oTablasE)
            Dim dias As Integer
            dias = CType(tabla.Rows(0)("valor").ToString().Trim(), Integer)
            Dim fecha As String
            fecha = Date.Now.AddDays(dias).ToShortDateString()

            'DateAdd(DateInterval.Day, dias, Date.Now).ToString()

            Return fecha
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try

    End Function


    ''' <summary>
    ''' FUNCION PARA GUARDAR LOS DATOS DE RECETA
    ''' </summary>
    ''' <param name="FechaProximaCita"></param>
    ''' <param name="RecetaAlta"></param>
    ''' <param name="CodigoProdcuto"></param>
    ''' <param name="Cantidad"></param>
    ''' <param name="Frecuencia"></param>
    ''' <param name="Duracion"></param>
    ''' <param name="Dosis"></param>
    ''' <param name="Detalle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarReceta(ByVal FechaProximaCita As String, ByVal RecetaAlta As Integer, ByVal CodigoProdcuto As String, ByVal Cantidad As String, ByVal Frecuencia As String, ByVal Duracion As String,
                             ByVal Dosis As String, ByVal Detalle As String, ByVal IdRecetaActual As String, ByVal IdRecetaDetActual As String, ByVal DscProducto As String, ByVal FechaVigencia As String) As String
        Dim pagina As New Receta()
        Return pagina.GuardarReceta_(FechaProximaCita, RecetaAlta, CodigoProdcuto, Cantidad, Frecuencia, Duracion, Dosis, Detalle, IdRecetaActual, IdRecetaDetActual, DscProducto, FechaVigencia)
    End Function

    Public Function GuardarReceta_(ByVal FechaProximaCita As String, ByVal RecetaAlta As Integer, ByVal CodigoProdcuto As String, ByVal Cantidad As String, ByVal Frecuencia As String, ByVal Duracion As String,
                             ByVal Dosis As String, ByVal Detalle As String, ByVal IdRecetaActual As String, ByVal IdRecetaDetActual As String, ByVal DscProducto As String, ByVal FechaVigencia As String) As String

        Try
            'VALIDANDO SI EL PRODUCTO YA FUE AGREGADO
            Dim tabla As New DataTable()
            oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
            Dim Mensaje As String = ""
            If IdRecetaActual <> "" Then
                oRceRecetaMedicamentoE.IdRecetaDet = IdRecetaActual
                oRceRecetaMedicamentoE.Orden = 2
                tabla = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)

                For index = 0 To tabla.Rows.Count - 1
                    If tabla.Rows(index)("cod_producto").ToString().Trim() = CodigoProdcuto And IdRecetaDetActual = "" Then
                        Mensaje = "El producto ya se encuentra agregado."
                        Exit For
                    End If
                Next
            End If

            If Mensaje <> "" Then
                Return "AVISO;" + Mensaje
            End If


            'VERIFICANDO SI YA SE INSERTO ALGUN REGISTRO
            If IdRecetaActual = "" Then
                oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
                oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
            Else
                oRceRecetaMedicamentoE.IdReceta = IdRecetaActual
            End If


            If oRceRecetaMedicamentoE.IdReceta > 0 Then

                'Sp_RceRecetaMedicamentoCab_Update
                If FechaProximaCita.Trim() <> "" Then
                    oRceRecetaMedicamentoE.Campo = "fec_proxima_cita"
                    oRceRecetaMedicamentoE.ValorNuevo = Format(CType(FechaProximaCita, DateTime), "MM/dd/yyyy H:mm:ss")
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                End If

                oRceRecetaMedicamentoE.Campo = "flg_alta" 'se cambia "receta_alta" por "flg_alta" tmacassi 03/10/2016
                oRceRecetaMedicamentoE.ValorNuevo = RecetaAlta
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "recomendaciones"
                oRceRecetaMedicamentoE.ValorNuevo = ""
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "otras_recomendaciones"
                oRceRecetaMedicamentoE.ValorNuevo = ""
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                'TMACASSI 03/10/2016 Se actualiza campo fecha_vigencia
                oRceRecetaMedicamentoE.Campo = "fec_vigencia"
                oRceRecetaMedicamentoE.ValorNuevo = Format(CType(FechaVigencia, DateTime), "MM/dd/yyyy H:mm:ss")
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)




                If IdRecetaDetActual.Trim() = "" Then
                    oRceRecetaMedicamentoE.CodProducto = CodigoProdcuto
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                Else
                    oRceRecetaMedicamentoE.IdRecetaDet = IdRecetaDetActual
                End If

                oRceRecetaMedicamentoE.Campo = "num_cantidad"

                If Cantidad = "" Then
                    oRceRecetaMedicamentoE.ValorNuevo = 0
                Else
                    oRceRecetaMedicamentoE.ValorNuevo = Cantidad
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                End If


                oRceRecetaMedicamentoE.Campo = "num_frecuencia"
                oRceRecetaMedicamentoE.ValorNuevo = Frecuencia
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "num_duracion"
                oRceRecetaMedicamentoE.ValorNuevo = Duracion
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "num_dosis"
                oRceRecetaMedicamentoE.ValorNuevo = Dosis
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "txt_detalle"
                oRceRecetaMedicamentoE.ValorNuevo = Detalle.Trim().ToUpper()
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "fec_modifica"
                oRceRecetaMedicamentoE.ValorNuevo = Format(DateTime.Now, "MM/dd/yyyy H:mm:ss")
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "usr_modifica"
                oRceRecetaMedicamentoE.ValorNuevo = Session(sCodUser)
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                oRceRecetaMedicamentoE.Campo = "dsc_producto"
                oRceRecetaMedicamentoE.ValorNuevo = DscProducto
                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
            End If

            Return "OK;" + oRceRecetaMedicamentoE.IdReceta.ToString() + ";" + oRceRecetaMedicamentoE.IdRecetaDet.ToString()

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try

    End Function

    ''' <summary>
    ''' FUNCION QUE SE EJECUTA CUANDO PRESIONA EL BOTON GUARDAR
    ''' </summary>
    ''' <param name="IdRecetaActual"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarRecetaFinal(ByVal IdRecetaActual As String) As String
        Dim pagina As New Receta()
        Return pagina.GuardarRecetaFinal_(IdRecetaActual)
    End Function

    Public Function GuardarRecetaFinal_(ByVal IdRecetaActual As String) As String
        Try
            oRceRecetaMedicamentoE.Campo = "est_estado"
            oRceRecetaMedicamentoE.ValorNuevo = "A"
            oRceRecetaMedicamentoE.IdReceta = IdRecetaActual
            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    ''' <summary>
    ''' FUNCION QUE SE EJECUTA CUANDO PRESIONA EL BOTON SALIR DEL POPUP DE RECETA
    ''' </summary>
    ''' <param name="IdRecetaActual"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function AceptaSalirReceta() As String
        Dim pagina As New Receta()
        Return pagina.AceptaSalirReceta_()
    End Function

    Public Function AceptaSalirReceta_() As String
        Try
            'oRceRecetaMedicamentoE.Campo = "est_estado"
            'oRceRecetaMedicamentoE.ValorNuevo = "X"
            'oRceRecetaMedicamentoE.IdReceta = IdRecetaActual
            'oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

            LimpiarGridRecetas()

            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function




    ''' <summary>
    ''' FUNCION PARA VALIDAR SI EL PACIENTE ES ALERGICO AL PRODUCTO SELECCIONADO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAlergiaPaciente(ByVal CodigoProducto As String) As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaAlergiaPaciente_(CodigoProducto)
    End Function

    Public Function ValidaAlergiaPaciente_(ByVal CodigoProducto As String) As String
        Try
            Dim tabla As New DataTable()
            Dim Alergico As String = ""
            oRceAlergiaE.IdHistoria = Session(sIdeHistoria)
            oRceAlergiaE.CodProducto = CodigoProducto
            tabla = oRceAlergiaLN.Sp_RceAlergia_Validar(oRceAlergiaE)

            If tabla.Rows.Count > 0 Then
                If tabla.Rows(0)(0).ToString().Trim() = "1" Then
                    Alergico = "SI"
                End If
            End If

            Return Alergico
        Catch ex As Exception
            Return ex.Message.ToString().Trim()
        End Try
    End Function


    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaSession() As String
        Dim pagina As New InformacionPaciente()
        Return pagina.ValidaSession_()
    End Function

    ''' <summary>
    ''' FUNCION PARA VERIFICAR SI LA SESSION SIGUE ACTIVA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidaSession_() As String
        If IsNothing(Session(sCodMedico)) Then
            Return "EXPIRO" + ";" + ConfigurationManager.AppSettings(sPantallaDefault).Trim().ToString()
        Else
            Return ""
        End If
    End Function



    'JB - 08/09/2020 - NUEVO CODIGO

    Public Sub ListarRecetas()
        If Not IsNothing(Session(sTablaFarmacologicoRA)) Then
            Dim dt As New DataTable()
            dt = CType(Session(sTablaFarmacologicoRA), DataTable)
            If dt.Rows.Count = 0 Then
                dt.Rows.Add()
                Session(sTablaFarmacologicoRA) = dt
            End If
            gvProductoMedicamentoRA.DataSource = Session(sTablaFarmacologicoRA)
            gvProductoMedicamentoRA.DataBind()
        Else
            Dim dt As New DataTable()
            dt.Columns.Add("Codigo")
            dt.Columns.Add("Producto")
            dt.Columns.Add("Via")
            dt.Columns.Add("CadaHora")
            dt.Columns.Add("Cantidad")
            dt.Columns.Add("Dosis")
            dt.Columns.Add("Indicacion")
            dt.Columns.Add("Dia")

            dt.Columns.Add("FechaProximaCita")
            dt.Columns.Add("FechaVigencia")
            dt.Columns.Add("UnidadMedida")
            dt.Rows.Add()
            gvProductoMedicamentoRA.DataSource = dt
            gvProductoMedicamentoRA.DataBind()
        End If
    End Sub

    'VerificaDataReceta
    <System.Web.Services.WebMethod()>
    Public Shared Function VerificaDataReceta() As String
        Dim pagina As New Receta()
        Return pagina.VerificaDataReceta_()
    End Function

    Public Function VerificaDataReceta_() As String
        Try
            If Not IsNothing(Session(sTablaFarmacologicoRA)) Then
                Dim dt As New DataTable()
                dt = CType(Session(sTablaFarmacologicoRA), DataTable)
                If dt.Rows.Count > 0 Then
                    Return "OK;"
                End If
            End If
            If Not IsNothing(Session(sTablaNoFarmacologicoRA)) Then
                Dim dt As New DataTable()
                dt = CType(Session(sTablaNoFarmacologicoRA), DataTable)
                If dt.Rows.Count > 0 Then
                    Return "OK;"
                End If
            End If
            If Not IsNothing(Session(sTablaInfusionesRA)) Then
                Dim dt As New DataTable()
                dt = CType(Session(sTablaInfusionesRA), DataTable)
                If dt.Rows.Count > 0 Then
                    Return "OK;"
                End If
            End If
            Return "NO;"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function

    Public Sub LimpiarGridRecetas()
        Session.Remove(sTablaFarmacologicoRA)
        Session.Remove(sTablaNoFarmacologicoRA)
        Session.Remove(sTablaInfusionesRA)
    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarRecetaF(ByVal FechaProximaCita As String, ByVal RecetaAlta As Integer, ByVal CodigoProdcuto As String, ByVal Cantidad As String, ByVal Frecuencia As String, ByVal Duracion As String,
                             ByVal Dosis As String, ByVal Detalle As String, ByVal DscProducto As String, ByVal FechaVigencia As String, ByVal UnidadMedida As String, ByVal ViaReceta As String) As String
        Dim pagina As New Receta()
        Return pagina.GuardarRecetaF_(FechaProximaCita, RecetaAlta, CodigoProdcuto, Cantidad, Frecuencia, Duracion, Dosis, Detalle, DscProducto, FechaVigencia, UnidadMedida, ViaReceta)
    End Function

    Public Function GuardarRecetaF_(ByVal FechaProximaCita As String, ByVal RecetaAlta As Integer, ByVal CodigoProdcuto As String, ByVal Cantidad As String, ByVal Frecuencia As String, ByVal Duracion As String,
                             ByVal Dosis As String, ByVal Detalle As String, ByVal DscProducto As String, ByVal FechaVigencia As String, ByVal UnidadMedida As String, ByVal ViaReceta As String) As String
        Try


            Dim dt As New DataTable()
            Dim ds As New DataSet()
            If IsNothing(Session(sTablaFarmacologicoRA)) Then

                dt.Columns.Add("Codigo")
                dt.Columns.Add("Producto")
                dt.Columns.Add("Via")
                dt.Columns.Add("CadaHora")
                dt.Columns.Add("Cantidad")
                dt.Columns.Add("Dosis")
                dt.Columns.Add("Indicacion")
                dt.Columns.Add("Dia")

                dt.Columns.Add("FechaProximaCita")
                dt.Columns.Add("FechaVigencia")
                dt.Columns.Add("UnidadMedida")
                dt.Rows.Add(CodigoProdcuto, DscProducto.Trim().ToUpper(), ViaReceta, Frecuencia, Cantidad, Dosis, Detalle.Trim().ToUpper(), Duracion, FechaProximaCita, FechaVigencia, UnidadMedida)
                Session(sTablaFarmacologicoRA) = dt
            Else
                dt = CType(Session(sTablaFarmacologicoRA), DataTable)
                For index = 0 To dt.Rows.Count - 1
                    If dt.Rows(index)("Codigo").ToString().Trim() = CodigoProdcuto Then
                        Return "ERROR;" + "El producto ya se encuentra agregado."
                        Exit For
                    End If
                Next

                dt.Rows.Add(CodigoProdcuto, DscProducto.Trim().ToUpper(), ViaReceta, Frecuencia, Cantidad, Dosis, Detalle.Trim().ToUpper(), Duracion, FechaProximaCita, FechaVigencia, UnidadMedida)
                Session(sTablaFarmacologicoRA) = dt
            End If


            dt.TableName = "TablaRecetaAltaF"
            Dim ds1 = dt.DataSet
            If ds1 IsNot Nothing Then
                ds1.Tables.Remove(dt.TableName)
            End If
            ds.Tables.Add(dt)
            Return ds.GetXml()


        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString().Trim()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarRecetaAltaF(ByVal Indice As String) As String
        Dim pagina As New Receta()
        Return pagina.EliminarRecetaAltaF_(Indice)
    End Function

    Public Function EliminarRecetaAltaF_(ByVal Indice As String) As String
        Try
            Dim dt As New DataTable()
            dt = CType(Session(sTablaFarmacologicoRA), DataTable)

            dt.Rows((Indice - 1)).Delete()

            Session(sTablaFarmacologicoRA) = dt
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarRecetaN(ByVal NutricionNoFarmacologicoRA As String, ByVal TerapiaFisRehaNoFarmacologicoRA As String, ByVal CuidadosEnfermeriaNoFarmacologicoRA As String, ByVal OtrosNoFarmacologicoRA As String) As String
        Dim pagina As New Receta()
        Return pagina.GuardarRecetaN_(NutricionNoFarmacologicoRA, TerapiaFisRehaNoFarmacologicoRA, CuidadosEnfermeriaNoFarmacologicoRA, OtrosNoFarmacologicoRA)
    End Function

    Public Function GuardarRecetaN_(ByVal NutricionNoFarmacologicoRA As String, ByVal TerapiaFisRehaNoFarmacologicoRA As String, ByVal CuidadosEnfermeriaNoFarmacologicoRA As String, ByVal OtrosNoFarmacologicoRA As String) As String
        Try
            Dim dt As New DataTable()
            Dim ds As New DataSet()
            If IsNothing(Session(sTablaNoFarmacologicoRA)) Then

                dt.Columns.Add("Nutricion")
                dt.Columns.Add("Terapia")
                dt.Columns.Add("Cuidado")
                dt.Columns.Add("Otros")

                dt.Rows.Add(NutricionNoFarmacologicoRA, TerapiaFisRehaNoFarmacologicoRA, CuidadosEnfermeriaNoFarmacologicoRA, OtrosNoFarmacologicoRA)
                Session(sTablaNoFarmacologicoRA) = dt
            Else
                dt = CType(Session(sTablaNoFarmacologicoRA), DataTable)
                dt.Rows.Add(NutricionNoFarmacologicoRA, TerapiaFisRehaNoFarmacologicoRA, CuidadosEnfermeriaNoFarmacologicoRA, OtrosNoFarmacologicoRA)
                Session(sTablaNoFarmacologicoRA) = dt
            End If

            Return "OK;"
            'dt.TableName = "TablaRecetaAltaN"
            'Dim ds1 = dt.DataSet
            'If ds1 IsNot Nothing Then
            '    ds1.Tables.Remove(dt.TableName)
            'End If
            'ds.Tables.Add(dt)
            'Return ds.GetXml()
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarRecetaI(ByVal InfusionControlClinicoRA As String) As String
        Dim pagina As New Receta()
        Return pagina.GuardarRecetaI_(InfusionControlClinicoRA)
    End Function

    Public Function GuardarRecetaI_(ByVal InfusionControlClinicoRA As String) As String
        Try
            Dim dt As New DataTable()
            Dim ds As New DataSet()
            If IsNothing(Session(sTablaInfusionesRA)) Then

                dt.Columns.Add("Item")
                dt.Columns.Add("Descripcion")

                dt.Rows.Add("1", InfusionControlClinicoRA)
                Session(sTablaInfusionesRA) = dt
            Else
                dt = CType(Session(sTablaInfusionesRA), DataTable)
                dt.Rows.Add((dt.Rows.Count + 1), InfusionControlClinicoRA)
                Session(sTablaInfusionesRA) = dt
            End If

            Return "OK;"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarRecetaAltaN(ByVal Indice As String) As String
        Dim pagina As New Receta()
        Return pagina.EliminarRecetaAltaF_(Indice)
    End Function

    Public Function EliminarRecetaAltaN_(ByVal Indice As String) As String
        Try
            Dim dt As New DataTable()
            dt = CType(Session(sTablaNoFarmacologicoRA), DataTable)

            dt.Rows((Indice - 1)).Delete()

            Session(sTablaNoFarmacologicoRA) = dt
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarRecetaAltaI(ByVal Indice As String) As String
        Dim pagina As New Receta()
        Return pagina.EliminarRecetaAltaI_(Indice)
    End Function

    Public Function EliminarRecetaAltaI_(ByVal Indice As String) As String
        Try
            Dim dt As New DataTable()
            dt = CType(Session(sTablaInfusionesRA), DataTable)

            dt.Rows((Indice - 1)).Delete()

            For index = 0 To dt.Rows.Count
                dt.Rows(index)("Item") = index + 1
            Next

            Session(sTablaInfusionesRA) = dt
            Return "OK"
        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarRecetaGeneral() As String
        Dim pagina As New Receta()
        Return pagina.GuardarRecetaGeneral_()
    End Function

    Public Function GuardarRecetaGeneral_() As String

        Try
            Dim dt As New DataTable()
            If Not IsNothing(Session(sTablaFarmacologicoRA)) Then 'PESTAÑA FARMACOLOGICO
                dt = CType(Session(sTablaFarmacologicoRA), DataTable)
                If dt.Rows.Count > 0 Then 'SI HAY DATOS FARMACOLOGICOS PARA INSERTAR ENTONCES...
                    'INSERTANDO CABECERA
                    oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
                    oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)

                    For index = 0 To dt.Rows.Count - 1
                        ''INSERTANDO CABECERA - jb - 29/04/2021 - comentado
                        'oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
                        'oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
                        'oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)

                        oRceRecetaMedicamentoE.Campo = "est_estado"
                        oRceRecetaMedicamentoE.ValorNuevo = "A"
                        oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                        'INSERTANDO DETALLE
                        If oRceRecetaMedicamentoE.IdReceta > 0 Then
                            If dt.Rows(index)("FechaProximaCita").ToString().Trim() <> "" Then
                                oRceRecetaMedicamentoE.Campo = "fec_proxima_cita"
                                oRceRecetaMedicamentoE.ValorNuevo = Format(CType(dt.Rows(index)("FechaProximaCita").ToString().Trim(), DateTime), "MM/dd/yyyy H:mm:ss")
                                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                            End If

                            oRceRecetaMedicamentoE.Campo = "flg_alta"
                            oRceRecetaMedicamentoE.ValorNuevo = 1
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                            'INICIO - JB - 18/11/2020 - NUEVO CODIGO
                            oRceRecetaMedicamentoE.Campo = "tipopedido"
                            oRceRecetaMedicamentoE.ValorNuevo = "01"
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "pedidogenerado"
                            oRceRecetaMedicamentoE.ValorNuevo = "N"
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                            'FIN - JB - 18/11/2020 - NUEVO CODIGO

                            oRceRecetaMedicamentoE.Campo = "recomendaciones"
                            oRceRecetaMedicamentoE.ValorNuevo = ""
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "otras_recomendaciones"
                            oRceRecetaMedicamentoE.ValorNuevo = ""
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                            'TMACASSI 03/10/2016 Se actualiza campo fecha_vigencia
                            oRceRecetaMedicamentoE.Campo = "fec_vigencia"
                            oRceRecetaMedicamentoE.ValorNuevo = Format(CType(dt.Rows(index)("FechaVigencia").ToString().Trim(), DateTime), "MM/dd/yyyy H:mm:ss")
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)


                            oRceRecetaMedicamentoE.CodProducto = dt.Rows(index)("Codigo").ToString().Trim()
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "num_cantidad"
                            If dt.Rows(index)("Cantidad").ToString().Trim() = "" Then
                                oRceRecetaMedicamentoE.ValorNuevo = 0
                            Else
                                oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Cantidad").ToString().Trim()
                                oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                            End If


                            oRceRecetaMedicamentoE.Campo = "num_frecuencia"
                            oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("CadaHora").ToString().Trim() 'Frecuencia
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "num_duracion"
                            oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Dia").ToString().Trim() 'Duracion
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "num_dosis"
                            oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Dosis").ToString().Trim() 'Dosis
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "txt_detalle"
                            oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Indicacion").ToString().Trim() 'Detalle.Trim().ToUpper()
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "fec_modifica"
                            oRceRecetaMedicamentoE.ValorNuevo = Format(DateTime.Now, "MM/dd/yyyy H:mm:ss")
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "usr_modifica"
                            oRceRecetaMedicamentoE.ValorNuevo = Session(sCodUser)
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "dsc_producto"
                            oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Producto").ToString().Trim() 'DscProducto
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                            'JB - 13/05/2021 - NUEVO CAMPO
                            oRceRecetaMedicamentoE.Campo = "dsc_via"
                            oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Via").ToString().Trim()
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                        End If
                    Next
                End If
            End If
            If Not IsNothing(Session(sTablaNoFarmacologicoRA)) Then 'PESTAÑA NO FARMACOLOGICO
                dt = CType(Session(sTablaNoFarmacologicoRA), DataTable)
                If dt.Rows.Count > 0 Then 'SI HAY DATOS NO FARMACOLOGICOS PARA INSERTAR ENTONCES...

                    'dt.Columns.Add("Nutricion")
                    'dt.Columns.Add("Terapia")
                    'dt.Columns.Add("Cuidado")
                    'dt.Columns.Add("Otros")

                    For index = 0 To dt.Rows.Count - 1
                        oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
                        oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
                        Dim InsertCab As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
                        If InsertCab > 0 Then
                            oRceRecetaMedicamentoE.Campo = "flg_alta"
                            oRceRecetaMedicamentoE.ValorNuevo = 1
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                            oRceRecetaMedicamentoE.CodProducto = ""
                            If dt.Rows(index)("Nutricion").ToString().Trim() <> "" Then
                                Dim InsertDetN As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                                oRceRecetaMedicamentoE.Campo = "txt_detalle"
                                oRceRecetaMedicamentoE.ValorNuevo = "NUTRICION"
                                Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                                oRceRecetaMedicamentoE.Campo = "dsc_producto"
                                oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Nutricion").ToString().Trim().ToUpper()
                                Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                            End If

                            'Dim InsertDetT As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                            If dt.Rows(index)("Terapia").ToString().Trim() <> "" Then
                                Dim InsertDetT As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                                oRceRecetaMedicamentoE.Campo = "txt_detalle"
                                oRceRecetaMedicamentoE.ValorNuevo = "TERAPIA"
                                Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                                oRceRecetaMedicamentoE.Campo = "dsc_producto"
                                oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Terapia").ToString().Trim().ToUpper()
                                Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                            End If

                            'Dim InsertDetC As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                            If dt.Rows(index)("Cuidado").ToString().Trim() <> "" Then
                                Dim InsertDetC As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                                oRceRecetaMedicamentoE.Campo = "txt_detalle"
                                oRceRecetaMedicamentoE.ValorNuevo = "CUIDADOS"
                                Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                                oRceRecetaMedicamentoE.Campo = "dsc_producto"
                                oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Cuidado").ToString().Trim().ToUpper()
                                Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                            End If

                            'Dim InsertDetO As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                            If dt.Rows(index)("Otros").ToString().Trim() <> "" Then
                                Dim InsertDetO As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                                oRceRecetaMedicamentoE.Campo = "txt_detalle"
                                oRceRecetaMedicamentoE.ValorNuevo = "OTROS"
                                Dim UpdateDet1 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)

                                oRceRecetaMedicamentoE.Campo = "dsc_producto"
                                oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Otros").ToString().Trim().ToUpper()
                                Dim UpdateDet2 As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                            End If
                            'If UpdateDet > 0 Then

                            'End If
                            oRceRecetaMedicamentoE.Campo = "est_estado"
                            oRceRecetaMedicamentoE.ValorNuevo = "A"
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                            oRceRecetaMedicamentoE.Campo = "tipo"
                            oRceRecetaMedicamentoE.ValorNuevo = "N"
                            oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                        End If
                    Next
                End If
            End If

            If Not IsNothing(Session(sTablaInfusionesRA)) Then
                oRceRecetaMedicamentoE.IdHistoria = Session(sIdeHistoria)
                oRceRecetaMedicamentoE.IdUsuario = Session(sCodUser)
                Dim InsertCab As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
                If InsertCab > 0 Then
                    oRceRecetaMedicamentoE.Campo = "flg_alta"
                    oRceRecetaMedicamentoE.ValorNuevo = 1
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                    If Not IsNothing(Session(sTablaInfusionesRA)) Then
                        dt = CType(Session(sTablaInfusionesRA), DataTable)
                        For index = 0 To dt.Rows.Count - 1
                            oRceRecetaMedicamentoE.CodProducto = ""

                            If dt.Rows(index)("Descripcion").ToString().Trim() <> "" Then
                                Dim InsertDet As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
                                If InsertDet > 0 Then
                                    '{"num_frecuencia", "num_dosis", "txt_detalle", "dsc_producto", "dsc_via"}
                                    oRceRecetaMedicamentoE.Campo = "dsc_producto"
                                    oRceRecetaMedicamentoE.ValorNuevo = dt.Rows(index)("Descripcion").ToString().Trim().ToUpper()
                                    Dim UpdateDet As Integer = oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
                                    If UpdateDet > 0 Then

                                    End If
                                End If
                            End If

                        Next
                    End If

                    oRceRecetaMedicamentoE.Campo = "est_estado"
                    oRceRecetaMedicamentoE.ValorNuevo = "A"
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)

                    oRceRecetaMedicamentoE.Campo = "tipo"
                    oRceRecetaMedicamentoE.ValorNuevo = "I"
                    oRceRecetaMedicamentoLN.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
                End If
            End If


            LimpiarGridRecetas()


            Dim _reporteApi As RespuestaArchivoJsonE

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


            Dim oRceMedicamentosE As New RceMedicamentosE()
            Dim oRceMedicamentosLN As New RceMedicamentosLN()
            oRceMedicamentosE.IdHistoria = Session(sIdeHistoria)
            oRceMedicamentosE.Detalle = ""
            oRceMedicamentosE.CodUser = Session(sCodUser)
            oRceMedicamentosE.TipoDocumento = 2
            oRceMedicamentosE.Documento = _reporteApi.ArchivoByte
            oRceMedicamentosE.Estado = "A"
            oRceMedicamentosE.Codigo = Session(sCodigoAtencion)
            oRceMedicamentosE.FecReporte = DateTime.Now

            oRceMedicamentosLN.Sp_RceResultadoDocumentoDet_InsertV3(oRceMedicamentosE)


            Return "OK;"

        Catch ex As Exception
            Return "ERROR;" + ex.Message.ToString()
        End Try

    End Function
    'JB - FIN - 08/09/2020


    'JB - 13/05/2021 - NUEVOS CAMBIOS
    Public Sub ListarViasRecetaAlta()
        'INICIO - JB - 27/04/2021 - VUELVE A SER UN LISTADO
        oTablasE.CodTabla = "VIAADMINISTRACIONHOSPITAL"
        oTablasE.Buscar = ""
        oTablasE.Key = 0
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = 5
        Dim tabla_ As New DataTable()
        tabla_ = oTablasLN.Sp_Tablas_Consulta(oTablasE)

        Dim FilaVacia As DataRow
        FilaVacia = tabla_.NewRow()
        FilaVacia(0) = "VIAADMINISTRACIONHOSPITAL"
        FilaVacia(1) = ""
        FilaVacia(2) = ""
        FilaVacia(3) = "0"
        FilaVacia(4) = ""
        tabla_.Rows.InsertAt(FilaVacia, 0)
        'DataRow newRow = myDataTable.NewRow();
        'newRow[0] = "0";
        'newRow[1] = "Select one";
        'myDataTable.Rows.InsertAt(newRow, 0);

        ddlViaRecetaAlta.DataSource = tabla_
        ddlViaRecetaAlta.DataTextField = "nombre"
        ddlViaRecetaAlta.DataValueField = "codigo"
        ddlViaRecetaAlta.DataBind()
        'FIN - JB - 27/04/2021 - VUELVE A SER UN LISTADO
    End Sub

    Private Function ObtenerReporteApiReporteria(ByVal rptLogConsultaReportesE As RptLogConsultaReportesE)
        Dim _rutaApi As String = ConfigurationManager.AppSettings("RutaApiReporteria")
        Dim _resultado As New RespuestaArchivoJsonE()
        Try
            'Dim JsonString As String = JsonConvert.SerializeObject(rptLogConsultaReportesE)
            Dim _client As RestClient = New RestClient(_rutaApi)
            Dim _request As RestRequest = New RestRequest()
            '_request.AddHeader("Authorization", _Encript.EncryptConectionString(_Encript._ApiCLinica))
            _request.Method = Method.Post
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