Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.LaboratorioE

Namespace LaboratorioAD
    Public Class RceLaboratioAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
        ''' <summary>
        ''' FUNCION PARA LA BUSQUEDA DE LABORATORIO
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceBuscar_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceBuscar_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oRceLaboratioE.Nombre)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceLaboratioE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceLaboratioE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_atencion", oRceLaboratioE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function
        ''' <summary>
        ''' FUNCION PARA LA BUSQUEDA FAVORITO DE LABORATORIO
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAnalisisFavoritoMae_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAnalisisFavoritoMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceLaboratioE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceLaboratioE.CodMedico)
            cmd.Parameters.AddWithValue("@dsc_nombres", oRceLaboratioE.Nombre)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        ''' <summary>
        ''' FUNCION USADA PARA OBTENER LOS SUB-DIAGNOSTICOS DE UN DIAGNOSTICO QUE TENGA PERFIL
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAnalisisEmergenciaMae_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAnalisisEmergenciaMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@idanalisis", oRceLaboratioE.IdAnalisisLaboratorio)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceLaboratioE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@ide_laboratorio_titulo", oRceLaboratioE.IdlaboratorioTitulo)
            cmd.Parameters.AddWithValue("@dsc_analisis", oRceLaboratioE.Nombre)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' GUARDANDO ANALISIS EN FAVORITO
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAnalisisFavoritoMae_Insert(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAnalisisFavoritoMae_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_analisis", oRceLaboratioE.IdAnalisisLaboratorio)
            cmd.Parameters.AddWithValue("@cod_medico", oRceLaboratioE.CodMedico)
            cmd.Parameters.AddWithValue("@usr_registra", oRceLaboratioE.UsrRegistra)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceLaboratioE.TipoDeAtencion)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_analisisfav"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceLaboratioE.IdAnalisisFavorito
            cmd.Parameters.Add(ParametroSalida)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                oRceLaboratioE.IdAnalisisFavorito = cmd.Parameters("@ide_analisisfav").Value
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA AGREGAR UN ANALISIS
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaAnalisisCab_InsertV2(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisCab_InsertV2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@cod_atencion", oRceLaboratioE.CodAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceLaboratioE.CodMedico)
            cmd.Parameters.AddWithValue("@usr_registra", oRceLaboratioE.UsrRegistra)
            cmd.Parameters.AddWithValue("@dsc_receta", oRceLaboratioE.DscReceta)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceLaboratioE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@est_analisis", oRceLaboratioE.EstAnalisis)
            cmd.Parameters.AddWithValue("@flg_anticipado", oRceLaboratioE.FlgAnticipado)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_recetacab"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceLaboratioE.IdeRecetaCab
            cmd.Parameters.Add(ParametroSalida)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                oRceLaboratioE.IdeRecetaCab = cmd.Parameters("@ide_recetacab").Value
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR ANALISIS - CABECERA
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaAnalisisCab_Update(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisCab_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_recetacab", oRceLaboratioE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceLaboratioE.ValorNuevo)
            cmd.Parameters.AddWithValue("@campo", oRceLaboratioE.Campo)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR ANALISIS - DETALLE
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaAnalisisDet_Update(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisDet_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_recetadet", oRceLaboratioE.IdeRecetaDet)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceLaboratioE.ValorNuevo)
            cmd.Parameters.AddWithValue("@campo", oRceLaboratioE.Campo)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function


        ''' <summary>
        ''' FUNCION PARA INSERTAR DETALLE DE ANALISIS
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaAnalisisDet_Insert(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisDet_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_recetacab", oRceLaboratioE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@est_analisis", oRceLaboratioE.EstAnalisis)
            cmd.Parameters.AddWithValue("@usr_registra", oRceLaboratioE.UsrRegistra)
            cmd.Parameters.AddWithValue("@ide_analisis", oRceLaboratioE.IdAnalisisLaboratorio)
            cmd.Parameters.AddWithValue("@flg_cubierto", oRceLaboratioE.FlgCubierto)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_recetadet"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceLaboratioE.IdeRecetaDet
            cmd.Parameters.Add(ParametroSalida)
            cn.Open()

            If cmd.ExecuteNonQuery() >= 1 Then
                oRceLaboratioE.IdeRecetaDet = cmd.Parameters("@ide_recetadet").Value
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR UN ANALISIS 
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaAnalisisDet_Delete(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisDet_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_recetadet", oRceLaboratioE.IdeRecetaDet)
            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()
            Return True
        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR ANALISIS DE FAVORITO
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAnalisisFavoritoMae_Delete(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAnalisisFavoritoMae_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_analisisfav", oRceLaboratioE.IdAnalisisFavorito)
            cn.Open()
            If cmd.ExecuteNonQuery() = 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        Public Function Sp_RceRecetaAnalisisCab_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisCab_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceLaboratioE.CodAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceLaboratioE.CodMedico)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        Public Function Sp_RceRecetaAnalisisDet_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisDet_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceLaboratioE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_recetacab", oRceLaboratioE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@ide_usr", oRceLaboratioE.IdeUsr)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA VALIDAR SI EL ANALISIS ESTA ASOCIADO A UN DIAGNOSTICO
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAnalisisxDiagnostico_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAnalisisxDiagnostico_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_analisis", oRceLaboratioE.IdAnalisisLaboratorio)
            cmd.Parameters.AddWithValue("@cod_diagnostico", oRceLaboratioE.CodDiagnostico)
            cn.Open()
            Dim NumeroFilas As Integer
            NumeroFilas = cmd.ExecuteScalar
            If cmd.ExecuteScalar >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA LISTAR LOS ANALISIS Y LLENAR EL TREEVIEW
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceResultadoAnalisisCab_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceResultadoAnalisisCab_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_recetacab", oRceLaboratioE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' NUEVA FUNCION PARA LAS ALERTAS...
        ''' </summary>
        ''' <param name="oRceLaboratioE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaAnalisis_Consulta(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisis_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceLaboratioE.CodAtencion)
            cmd.Parameters.AddWithValue("@receta", oRceLaboratioE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function


        Public Function Sp_RceAnalisisxDiagnostico_Valida(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAnalisisxDiagnostico_Valida", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_analisis", oRceLaboratioE.IdAnalisisLaboratorio)
            cmd.Parameters.AddWithValue("@ide_historia", oRceLaboratioE.IdHistoria)
            cmd.Parameters.AddWithValue("@cod_diagnostico", oRceLaboratioE.CodDiagnostico) 'JB - 26/08/2019
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function




        Public Function Sp_RceRecetaAnalisisDet_ConsultaV2(ByVal oRceLaboratioE As RceLaboratioE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaAnalisisDet_ConsultaV2", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", oRceLaboratioE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_recetacab", oRceLaboratioE.IdeRecetaCab)
            cmd.Parameters.AddWithValue("@fec_receta", oRceLaboratioE.FechaReceta)
            cmd.Parameters.AddWithValue("@hor_receta", oRceLaboratioE.HoraReceta)
            cmd.Parameters.AddWithValue("@orden", oRceLaboratioE.Orden)
            cn.Open()

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function


    End Class
End Namespace


