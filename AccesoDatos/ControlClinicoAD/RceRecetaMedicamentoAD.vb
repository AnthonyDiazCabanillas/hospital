' ***************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2024. Todos los derechos reservados.
'    Version Fecha      Autor       Requerimiento   Objetivo específico
'    1.0     02/02/2024 CRODRIGUEZ  REQ 2023-021287 Se envia correo al agregar o cambiar
'                                                   dieta del paciente 
'****************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.ControlClinicoE
Imports Entidades.OtrosE '1.0

Namespace ControlClinicoAD
    Public Class RceRecetaMedicamentoAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
        ''' <summary>
        ''' FUNCION PARA INSERTAR RECETA MEDICA - CABECERA (CONTROL CLINICO E INDICACIONES MEDICAS)
        ''' </summary>
        ''' <param name="oRceRecetaMedicamentoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaMedicamentoCab_Insert(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaMedicamentoCab_InsertV2", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceRecetaMedicamentoE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_usuario", oRceRecetaMedicamentoE.IdUsuario)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_receta"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceRecetaMedicamentoE.IdReceta
            cmd.Parameters.Add(ParametroSalida)

            Dim insertCab As Integer = 0
            cn.Open()
            insertCab = cmd.ExecuteNonQuery()
            oRceRecetaMedicamentoE.IdReceta = cmd.Parameters("@ide_receta").Value
            cn.Close()
            Return insertCab
        End Function

        ''' <summary>
        ''' FUNCION PARA INSERTAR RECETA MEDICA - DETALLE (CONTROL CLINICO E INDICACIONES MEDICAS)
        ''' </summary>
        ''' <param name="oRceRecetaMedicamentoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaMedicamentoDet_Insert(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaMedicamentoDet_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_recetacab", oRceRecetaMedicamentoE.IdReceta)
            cmd.Parameters.AddWithValue("@cod_producto", oRceRecetaMedicamentoE.CodProducto)
            cmd.Parameters.AddWithValue("@col_medico", oRceRecetaMedicamentoE.IdUsuario)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_recetadet"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceRecetaMedicamentoE.IdRecetaDet
            cmd.Parameters.Add(ParametroSalida)

            Dim insertDet As Integer = 0
            cn.Open()
            insertDet = cmd.ExecuteNonQuery()
            oRceRecetaMedicamentoE.IdRecetaDet = cmd.Parameters("@ide_recetadet").Value
            cn.Close()
            Return insertDet

        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR RECETA MEDICA - CABECERA (CONTROL CLINICO E INDICACIONES MEDICAS)
        ''' </summary>
        ''' <param name="oRceRecetaMedicamentoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaMedicamentoCab_Update(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaMedicamentoCab_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@campo", oRceRecetaMedicamentoE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oRceRecetaMedicamentoE.IdReceta)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceRecetaMedicamentoE.ValorNuevo)

            Dim update As Integer = 0
            cn.Open()
            update = cmd.ExecuteNonQuery()
            cn.Close()
            Return update
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZAR RECETA MEDICA - DETALLE (CONTROL CLINICO E INDICACIONES MEDICAS)
        ''' </summary>
        ''' <param name="oRceRecetaMedicamentoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaMedicamentoDet_Update(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaMedicamentoDet_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@campo", oRceRecetaMedicamentoE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oRceRecetaMedicamentoE.IdRecetaDet)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceRecetaMedicamentoE.ValorNuevo)

            Dim update As Integer = 0
            cn.Open()
            update = cmd.ExecuteNonQuery()
            cn.Close()
            Return update
        End Function

        ''' <summary>
        ''' FUNCION PARA LISTAR CONTROL CLININICO - INDICACIONES MEDICAS
        ''' </summary>
        ''' <param name="oRceRecetaMedicamentoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceRecetaMedicamentoCab_Consulta(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaMedicamentoCab_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_historia", oRceRecetaMedicamentoE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_receta", oRceRecetaMedicamentoE.IdRecetaDet)
            cmd.Parameters.AddWithValue("@orden", oRceRecetaMedicamentoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function


        Public Function Sp_RceRecetaSuspension_Insert(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaSuspension_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_medicamentorec", oRceRecetaMedicamentoE.IdRecetaDet)
            cmd.Parameters.AddWithValue("@cod_medico", oRceRecetaMedicamentoE.CodMedico)

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@ide_medsuspension"
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Value = oRceRecetaMedicamentoE.IdRecetaDet
            cmd.Parameters.Add(ParametroSalida)
            '@ide_medicamentorec, @cod_medico, @ide_medsuspension output
            Dim insertDet As Integer = 0
            cn.Open()
            insertDet = cmd.ExecuteNonQuery()
            oRceRecetaMedicamentoE.IdRecetaDet = cmd.Parameters("@ide_medsuspension").Value
            cn.Close()
            Return insertDet
        End Function


        Public Function Sp_RceRecetaSuspension_Update(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaSuspension_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@campo", oRceRecetaMedicamentoE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oRceRecetaMedicamentoE.IdRecetaDet)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceRecetaMedicamentoE.ValorNuevo)

            Dim update As Integer = 0
            cn.Open()
            update = cmd.ExecuteNonQuery()
            cn.Close()
            Return update
        End Function




        Public Function Sp_RceRecetaMedicamentoCab_ConsultaV2(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceRecetaMedicamentoCab_ConsultaV2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@cod_atencion", oRceRecetaMedicamentoE.CodAtencion)
            cmd.Parameters.AddWithValue("@ide_recetacab", oRceRecetaMedicamentoE.IdReceta)
            cmd.Parameters.AddWithValue("@fec_receta", oRceRecetaMedicamentoE.FecReceta)
            cmd.Parameters.AddWithValue("@hor_receta", oRceRecetaMedicamentoE.HorReceta)
            cmd.Parameters.AddWithValue("@orden", oRceRecetaMedicamentoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function


        Public Function Sp_RceFarmacoDosis_Consulta(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceFarmacoDosis_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@id_farmaco_dosis", oRceRecetaMedicamentoE.IdFarmaco)
            cmd.Parameters.AddWithValue("@orden", oRceRecetaMedicamentoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function
        'INI 1.0
        Public Function Sp_Rce_Envia_Correo_Dietas(ByVal pSisCorreoE As SisCorreoE) As String
            Using cnn As New SqlConnection(CnnBD)
                Using cmd As New SqlCommand("Sp_Rce_Envia_Correo_Dietas", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    'Parametros del Store
                    cmd.Parameters.AddWithValue("@orden", pSisCorreoE.Orden)
                    cmd.Parameters.AddWithValue("@cod_paciente", pSisCorreoE.CodPaciente)
                    cnn.Open()
                    Dim id As Integer = 0
                    id = cmd.ExecuteNonQuery()
                    cnn.Close()
                End Using
            End Using
            Return ""
        End Function
        'FIN 1.0
    End Class
End Namespace

