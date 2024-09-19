Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.ImagenesE

Namespace ImagenesAD
    Public Class RceImagenesAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
        ''' <summary>
        ''' FUNCION QUE LISTA LA BUSQUEDA DE IMAGENENES
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceBuscar_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceBuscar_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oRceImagenesE.Nombre)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceImagenesE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceImagenesE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_atencion", oRceImagenesE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        ''' <summary>
        ''' FUNCION QUE LISTA LA BUSQUEDA DA FAVORITO IMAGEN
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceImagenFavoritoMae_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceImagenFavoritoMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_medico", oRceImagenesE.CodMedico)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceImagenesE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)

            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA OBTENER DATOS DE LA CABECERA DE IMAGEN
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenCab_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenCab_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceImagenesE.CodAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceImagenesE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_paciente", oRceImagenesE.CodPaciente)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function


        ''' <summary>
        ''' FUNCION QUE LISTA LAS IMAGENES SELECCIONADAS
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenDet_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenDet_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceImagenesE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_recetacab", oRceImagenesE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@ide_imagen", oRceImagenesE.IdeImagen)
            cmd.Parameters.AddWithValue("@ide_usr", oRceImagenesE.IdeUsr) '
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)

            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA INSERTAR CABECERA DETALLE
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenCab_InsertV2(ByVal oRceImagenesE As RceImagenesE) As RceImagenesE
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenCab_InsertV2", cn)
            Dim exito As Integer
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceImagenesE.CodAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceImagenesE.CodMedico)
            cmd.Parameters.AddWithValue("@est_imagen", oRceImagenesE.EstImagen)
            cmd.Parameters.AddWithValue("@usr_registra", oRceImagenesE.UsrRegistra)
            cmd.Parameters.AddWithValue("@dsc_receta", oRceImagenesE.DscReceta)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceImagenesE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@ide_tipoexamen", oRceImagenesE.TipoExamen)

            Dim oOutParameter1 As New SqlParameter()
            oOutParameter1.ParameterName = "@ide_recetacab"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceImagenesE.IdeRecetaCab
            cmd.Parameters.Add(oOutParameter1)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            oRceImagenesE.IdeRecetaCab = cmd.Parameters("@ide_recetacab").Value
            cn.Close()

            Return oRceImagenesE
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR CABECERA IMAGEN
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenCab_Update(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenCab_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@iderecetaimagencab", oRceImagenesE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@campo", oRceImagenesE.Campo)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceImagenesE.ValorNuevo)

            cn.Open()
            cmd.ExecuteNonQuery()
            Return True
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA AGREGAR DETALLE DE IMAGENES
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenDet_InsertV2(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenDet_InsertV2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim ParametroSalida1 As New SqlParameter()
            ParametroSalida1.ParameterName = "@codigo"
            ParametroSalida1.SqlDbType = SqlDbType.Int
            ParametroSalida1.Size = 8
            ParametroSalida1.Direction = ParameterDirection.InputOutput
            ParametroSalida1.Value = oRceImagenesE.IdeRecetaDet
            cmd.Parameters.Add(ParametroSalida1)
            Dim ParametroSalida2 As New SqlParameter()
            ParametroSalida2.ParameterName = "@presotor"
            ParametroSalida2.SqlDbType = SqlDbType.Char
            ParametroSalida2.Size = 12
            ParametroSalida2.Direction = ParameterDirection.InputOutput
            ParametroSalida2.Value = oRceImagenesE.CodPresotor
            cmd.Parameters.Add(ParametroSalida2)
            Dim ParametroSalida3 As New SqlParameter()
            ParametroSalida3.ParameterName = "@codprestacion"
            ParametroSalida3.SqlDbType = SqlDbType.Char
            ParametroSalida3.Size = 9
            ParametroSalida3.Direction = ParameterDirection.InputOutput
            ParametroSalida3.Value = oRceImagenesE.CodPrestacion
            cmd.Parameters.Add(ParametroSalida3)

            cmd.Parameters.AddWithValue("@ide_recetacab", oRceImagenesE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@ide_imagen", oRceImagenesE.IdeImagen)
            cmd.Parameters.AddWithValue("@usr_registra", oRceImagenesE.UsrRegistra)
            cmd.Parameters.AddWithValue("@cod_medico", oRceImagenesE.CodMedico)
            cmd.Parameters.AddWithValue("@est_imagen", oRceImagenesE.EstImagen)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceImagenesE.TipoDeAtencion)

            cn.Open()
            cmd.ExecuteNonQuery()
            oRceImagenesE.IdeRecetaDet = cmd.Parameters("@codigo").Value
            oRceImagenesE.CodPresotor = cmd.Parameters("@presotor").Value
            oRceImagenesE.CodPrestacion = cmd.Parameters("@codprestacion").Value
            Return True
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR DETALLE DE IMAGENES
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenDet_Update(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenDet_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_recetadet", oRceImagenesE.IdeRecetaDet)
            cmd.Parameters.AddWithValue("@campo", oRceImagenesE.Campo)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceImagenesE.ValorNuevo)

            cn.Open()
            cmd.ExecuteNonQuery()
            Return True
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA AGREGAR UNA IMAGEN A FAVORITO
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceImagenFavoritoMae_Insert(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceImagenFavoritoMae_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_imagen", oRceImagenesE.IdeImagen)
            cmd.Parameters.AddWithValue("@cod_medico", oRceImagenesE.CodMedico)
            cmd.Parameters.AddWithValue("@usr_registra", oRceImagenesE.UsrRegistra)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceImagenesE.TipoDeAtencion)

            Dim ParametroSalida1 As New SqlParameter()
            ParametroSalida1.ParameterName = "@ide_imagenfav"
            ParametroSalida1.SqlDbType = SqlDbType.Int
            ParametroSalida1.Size = 8
            ParametroSalida1.Direction = ParameterDirection.InputOutput
            ParametroSalida1.Value = oRceImagenesE.IdImagenFavorito
            cmd.Parameters.Add(ParametroSalida1)

            cn.Open()
            cmd.ExecuteNonQuery()
            oRceImagenesE.IdImagenFavorito = cmd.Parameters("@ide_imagenfav").Value
            Return True
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR IMAGEN DE FAVORITO
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceImagenFavoritoMae_Delete(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceImagenFavoritoMae_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_imagenfav", oRceImagenesE.IdImagenFavorito)

            cn.Open()
            cmd.ExecuteNonQuery()
            If cmd.ExecuteNonQuery() = 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR UNA IMAGEN
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaImagenDet_Delete(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenDet_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_recetadet", oRceImagenesE.IdeRecetaDet)
            cn.Open()
            cmd.ExecuteNonQuery()
            Return True
            cn.Close()
            cmd.Dispose()
        End Function

        ''' <summary>
        ''' FUNCION USADA PARA VER INFORME DE IMAGENES
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Presotor_Pdf_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Presotor_Pdf_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codpresotor", oRceImagenesE.CodPresotor)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA VER IMAGEN
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Tablas_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Tablas_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@codtabla", oRceImagenesE.CodTabla)
            cmd.Parameters.AddWithValue("@buscar", oRceImagenesE.Buscar)
            cmd.Parameters.AddWithValue("@key", oRceImagenesE.Key)
            cmd.Parameters.AddWithValue("@numerolineas", oRceImagenesE.NumeroLineas)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)

            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA CONSULTAR/CARGAR PETITORIO DE IMAGENES
        ''' </summary>
        ''' <param name="oRceImagenesE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceImagenMae_ConsultaV2(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceImagenMae_ConsultaV2", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ideimagen", oRceImagenesE.IdeImagen)
            cmd.Parameters.AddWithValue("@idepadre", oRceImagenesE.IdeImagenPadre)
            cmd.Parameters.AddWithValue("@idetitulo", oRceImagenesE.IdeImagenTitulo)
            cmd.Parameters.AddWithValue("@idtipoatencion", oRceImagenesE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function









        Public Function Sp_Ris_EvaluaAtencion(ByVal oRceImagenesE As RceImagenesE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_EvaluaAtencion", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceImagenesE.CodAtencion)
            cmd.Parameters.AddWithValue("@codprestacion", oRceImagenesE.CodPrestacion)
            cmd.Parameters.AddWithValue("@codlocal", oRceImagenesE.CodLocal)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)
            cn.Open()

            Dim ValorRetorno As New SqlParameter()
            ValorRetorno = cmd.Parameters.Add("RetVal", SqlDbType.VarChar)
            ValorRetorno.Direction = ParameterDirection.ReturnValue
            cmd.ExecuteNonQuery()
            Dim ValorDevuelto As String
            ValorDevuelto = ValorRetorno.Value

            Return ValorDevuelto
            'Dim da As New SqlDataAdapter(cmd)
            'Dim dt As New DataTable()
            'da.Fill(dt)
            'cn.Close()
            'Return dt
        End Function


        Public Function Sp_Ris_ListaDatosPresotor(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_ListaDatosPresotor", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@CodPresotor", oRceImagenesE.CodPresotor)
            cmd.Parameters.AddWithValue("@Tipo", oRceImagenesE.TipoDeAtencion)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        Public Function Sp_Ris_Sala_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_Sala_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@CodPresotor", oRceImagenesE.CodPresotor)
            cmd.Parameters.AddWithValue("@codprestacion", oRceImagenesE.CodPrestacion)
            cmd.Parameters.AddWithValue("@codlocal", oRceImagenesE.CodLocal)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        Public Function Sp_Ris_Consulta_WS(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_Consulta_WS", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codtabla", "RIS_PACS_WS")
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        Public Function Sp_Ris_Oracle_His_Xml_Events(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_Oracle_His_Xml_Events", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ORACLE", oRceImagenesE.ORACLE)
            cmd.Parameters.AddWithValue("@X_TIPOMSG", oRceImagenesE.X_TIPOMSG)
            cmd.Parameters.AddWithValue("@T_COD_EMPRESA", oRceImagenesE.T_COD_EMPRESA)
            cmd.Parameters.AddWithValue("@T_COD_SUCURSAL", oRceImagenesE.T_COD_SUCURSAL)
            cmd.Parameters.AddWithValue("@T_EVENT_ID", oRceImagenesE.T_EVENT_ID)
            cmd.Parameters.AddWithValue("@T_EVENT_DATETIME", oRceImagenesE.T_EVENT_DATETIME)
            cmd.Parameters.AddWithValue("@T_EVENT_TYPE_ID", oRceImagenesE.T_EVENT_TYPE_ID)
            cmd.Parameters.AddWithValue("@X_ID_PACIENTE", oRceImagenesE.X_ID_PACIENTE)
            cmd.Parameters.AddWithValue("@X_RUT_PACIENTE", oRceImagenesE.X_RUT_PACIENTE)
            cmd.Parameters.AddWithValue("@X_TIPO_PACIENTE", oRceImagenesE.X_TIPO_PACIENTE)
            cmd.Parameters.AddWithValue("@X_DEATH_INDICATOR", oRceImagenesE.X_DEATH_INDICATOR)
            cmd.Parameters.AddWithValue("@X_CAT_NAME", oRceImagenesE.X_CAT_NAME)
            cmd.Parameters.AddWithValue("@X_LAST_NAME", oRceImagenesE.X_LAST_NAME)
            cmd.Parameters.AddWithValue("@X_FIRST_NAME", oRceImagenesE.X_FIRST_NAME)
            cmd.Parameters.AddWithValue("@X_BIRTH_DATE", oRceImagenesE.X_BIRTH_DATE)
            cmd.Parameters.AddWithValue("@X_GENDER_KEY", oRceImagenesE.X_GENDER_KEY)
            cmd.Parameters.AddWithValue("@X_LAST_UPDATED", oRceImagenesE.X_LAST_UPDATED)
            cmd.Parameters.AddWithValue("@X_STREET_ADDRESS", oRceImagenesE.X_STREET_ADDRESS)
            cmd.Parameters.AddWithValue("@X_CITY", oRceImagenesE.X_CITY)
            cmd.Parameters.AddWithValue("@X_COUNTRY", oRceImagenesE.X_COUNTRY)
            cmd.Parameters.AddWithValue("@X_PHONE_NUMBER", oRceImagenesE.X_PHONE_NUMBER)
            cmd.Parameters.AddWithValue("@X_VISIT_NUMBER", oRceImagenesE.X_VISIT_NUMBER)
            cmd.Parameters.AddWithValue("@X_START_DATETIME", oRceImagenesE.X_START_DATETIME)
            cmd.Parameters.AddWithValue("@X_DURATION", oRceImagenesE.X_DURATION)
            cmd.Parameters.AddWithValue("@X_STATUS_KEY", oRceImagenesE.X_STATUS_KEY)
            cmd.Parameters.AddWithValue("@X_STATUS", oRceImagenesE.X_STATUS)
            cmd.Parameters.AddWithValue("@X_PROCEDURE_CODE", oRceImagenesE.X_PROCEDURE_CODE)
            cmd.Parameters.AddWithValue("@X_PROCEDURE_DESCRIPTION", oRceImagenesE.X_PROCEDURE_DESCRIPTION)
            cmd.Parameters.AddWithValue("@X_ROOM_CODE", oRceImagenesE.X_ROOM_CODE)
            cmd.Parameters.AddWithValue("@X_REQUESTED_BY", oRceImagenesE.X_REQUESTED_BY)
            cmd.Parameters.AddWithValue("@X_MESSAGE_TYPE", oRceImagenesE.X_MESSAGE_TYPE)
            cmd.Parameters.AddWithValue("@X_PACS_SPS_ID", oRceImagenesE.X_PACS_SPS_ID)
            cmd.Parameters.AddWithValue("@MSG_STATUS", oRceImagenesE.MSG_STATUS)
            cn.Open()
            cmd.ExecuteNonQuery()
            Return True
            cn.Close()
        End Function

        Public Function Sp_PresotorImagen_Insert(ByVal oRceImagenesE As RceImagenesE) As String
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_PresotorImagen_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim ParametroSalida1 As New SqlParameter()
            ParametroSalida1.ParameterName = "@codpresotor"
            ParametroSalida1.SqlDbType = SqlDbType.VarChar
            ParametroSalida1.Size = 12
            ParametroSalida1.Direction = ParameterDirection.InputOutput
            ParametroSalida1.Value = oRceImagenesE.CodPresotor
            cmd.Parameters.Add(ParametroSalida1)
            cmd.Parameters.AddWithValue("@codOrden", oRceImagenesE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@codOrden_det", oRceImagenesE.IdeRecetaDet)
            cn.Open()
            cmd.ExecuteNonQuery()
            oRceImagenesE.CodPresotor = cmd.Parameters("@codpresotor").Value
            Return oRceImagenesE.CodPresotor
            cn.Close()
        End Function

        Public Function RIS_PACS_WS() As DataTable
            Dim dt As New DataTable()
            Using cnn As New SqlConnection(CnnBD)
                Using cmd As New SqlCommand("Sp_Ris_Consulta_WS", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    'Parametros del Store
                    cmd.Parameters.AddWithValue("@codtabla", "RIS_PACS_WS")
                    cnn.Open()
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                    cnn.Close()
                    Return dt
                End Using
            End Using
        End Function

        'Public Function Sp_Ris_Oracle_His_Xml_Events(ByVal oRceImagenesE As RceImagenesE) As Boolean
        '    Dim cn As New SqlConnection(CnnBD)
        '    Dim cmd As New SqlCommand("Sp_Ris_Oracle_His_Xml_Events", cn)
        '    cmd.CommandType = CommandType.StoredProcedure
        '    cmd.Parameters.AddWithValue("@ORACLE", oRceImagenesE.ORACLE)
        '    cmd.Parameters.AddWithValue("@X_TIPOMSG", oRceImagenesE.X_TIPOMSG)
        '    cmd.Parameters.AddWithValue("@T_COD_EMPRESA", oRceImagenesE.T_COD_EMPRESA)
        '    cmd.Parameters.AddWithValue("@T_COD_SUCURSAL", oRceImagenesE.T_COD_SUCURSAL)
        '    cmd.Parameters.AddWithValue("@T_EVENT_ID", oRceImagenesE.T_EVENT_ID)
        '    cmd.Parameters.AddWithValue("@T_EVENT_DATETIME", oRceImagenesE.T_EVENT_DATETIME)
        '    cmd.Parameters.AddWithValue("@T_EVENT_TYPE_ID", oRceImagenesE.T_EVENT_TYPE_ID)
        '    cmd.Parameters.AddWithValue("@X_ID_PACIENTE", oRceImagenesE.X_ID_PACIENTE)
        '    cmd.Parameters.AddWithValue("@X_RUT_PACIENTE", oRceImagenesE.X_RUT_PACIENTE)
        '    cmd.Parameters.AddWithValue("@X_TIPO_PACIENTE", oRceImagenesE.X_TIPO_PACIENTE)
        '    cmd.Parameters.AddWithValue("@X_DEATH_INDICATOR", oRceImagenesE.X_DEATH_INDICATOR)
        '    cmd.Parameters.AddWithValue("@X_CAT_NAME", oRceImagenesE.X_CAT_NAME)
        '    cmd.Parameters.AddWithValue("@X_LAST_NAME", oRceImagenesE.X_LAST_NAME)
        '    cmd.Parameters.AddWithValue("@X_FIRST_NAME", oRceImagenesE.X_FIRST_NAME)
        '    cmd.Parameters.AddWithValue("@X_BIRTH_DATE", oRceImagenesE.X_BIRTH_DATE)
        '    cmd.Parameters.AddWithValue("@X_GENDER_KEY", oRceImagenesE.X_GENDER_KEY)
        '    cmd.Parameters.AddWithValue("@X_LAST_UPDATED", oRceImagenesE.X_LAST_UPDATED)
        '    cmd.Parameters.AddWithValue("@X_STREET_ADDRESS", oRceImagenesE.X_STREET_ADDRESS)
        '    cmd.Parameters.AddWithValue("@X_CITY", oRceImagenesE.X_CITY)
        '    cmd.Parameters.AddWithValue("@X_COUNTRY", oRceImagenesE.X_COUNTRY)
        '    cmd.Parameters.AddWithValue("@X_PHONE_NUMBER", oRceImagenesE.X_PHONE_NUMBER)
        '    cmd.Parameters.AddWithValue("@X_VISIT_NUMBER", oRceImagenesE.X_VISIT_NUMBER)
        '    cmd.Parameters.AddWithValue("@X_START_DATETIME", oRceImagenesE.X_START_DATETIME)
        '    cmd.Parameters.AddWithValue("@X_DURATION", oRceImagenesE.X_DURATION)
        '    cmd.Parameters.AddWithValue("@X_STATUS_KEY", oRceImagenesE.X_STATUS_KEY)
        '    cmd.Parameters.AddWithValue("@X_STATUS", oRceImagenesE.X_STATUS)
        '    cmd.Parameters.AddWithValue("@X_PROCEDURE_CODE", oRceImagenesE.X_PROCEDURE_CODE)
        '    cmd.Parameters.AddWithValue("@X_PROCEDURE_DESCRIPTION", oRceImagenesE.X_PROCEDURE_DESCRIPTION)
        '    cmd.Parameters.AddWithValue("@X_ROOM_CODE", oRceImagenesE.X_ROOM_CODE)
        '    cmd.Parameters.AddWithValue("@X_REQUESTED_BY", oRceImagenesE.X_REQUESTED_BY)
        '    cmd.Parameters.AddWithValue("@X_MESSAGE_TYPE", oRceImagenesE.X_MESSAGE_TYPE)
        '    cmd.Parameters.AddWithValue("@X_PACS_SPS_ID", oRceImagenesE.X_PACS_SPS_ID)
        '    cn.Open()
        '    cmd.ExecuteNonQuery()
        '    Return True
        '    cn.Close()
        'End Function

        'TMACASSI 31/10/2016
        Public Function Sp_Ris_Consulta_RisPacs(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_Consulta_RisPacs", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@parametro", oRceImagenesE.CodTabla)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function
        'TMACASSI 31/10/2016


        Public Function Sp_RceRecetaImagenDet_ConsultaV2(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaImagenDet_ConsultaV2", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceImagenesE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_recetacab", oRceImagenesE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@fec_receta", oRceImagenesE.FecReceta)
            cmd.Parameters.AddWithValue("@hor_receta", oRceImagenesE.HorReceta) '
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)

            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function



        Public Function Sp_Presotor_Consulta2(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Presotor_Consulta2", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codpresotor", oRceImagenesE.CodPresotor)
            cmd.Parameters.AddWithValue("@opcion", oRceImagenesE.Orden)

            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        Public Function Sp_Presotor_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Presotor_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codpresotor", oRceImagenesE.CodPresotor)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        Public Function Sp_Presotor_DeleteNewv3(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Presotor_DeleteNewv3", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codpresotor", oRceImagenesE.CodPresotor)
            cmd.Parameters.AddWithValue("@codusuario", oRceImagenesE.IdeUsr)

            Dim ParametroSalida1 As New SqlParameter()
            ParametroSalida1.ParameterName = "@codpresotornew"
            ParametroSalida1.SqlDbType = SqlDbType.VarChar
            ParametroSalida1.Size = 12
            ParametroSalida1.Direction = ParameterDirection.InputOutput
            ParametroSalida1.Value = oRceImagenesE.CodPresotorNew
            cmd.Parameters.Add(ParametroSalida1)

            cn.Open()
            cmd.ExecuteNonQuery()
            oRceImagenesE.CodPresotorNew = cmd.Parameters("@codpresotornew").Value
            cn.Close()
            Return True
        End Function

        Public Function Sp_Ris_AgendamientoAmbulatorio_Delete(ByVal oRceImagenesE As RceImagenesE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Ris_AgendamientoAmbulatorio_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codpresotor", oRceImagenesE.CodPresotor)
            cn.Open()
            Dim i As Integer
            i = cmd.ExecuteNonQuery()
            cn.Close()
            If i > 0 Then
                Return True
            Else
                Return False
            End If

        End Function


        Public Function ConsultaRisOracle(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Select * from ris_oracle_his_xml_events where event_id=@codpresotor and oracle='A'", cn)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("@codpresotor", oRceImagenesE.CodPresotor)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        Public Function Sp_Pacientes_Consulta(ByVal oRceImagenesE As RceImagenesE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Pacientes_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@buscar", oRceImagenesE.Buscar)
            cmd.Parameters.AddWithValue("@key", oRceImagenesE.Key)
            cmd.Parameters.AddWithValue("@numerolineas", oRceImagenesE.NumeroLineas)
            cmd.Parameters.AddWithValue("@orden", oRceImagenesE.Orden)
            cmd.Parameters.AddWithValue("@pdocidentidad", oRceImagenesE.DocIdentidad)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function
    End Class
End Namespace


