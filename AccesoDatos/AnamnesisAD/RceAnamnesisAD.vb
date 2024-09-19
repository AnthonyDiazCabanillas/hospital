Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.AnamnesisE

Namespace AnamnesisAD
    Public Class RceAnamnesisAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION QUE TRAE LA INFORMACION PARA CREAR LA ESTRUCTURA DE LOS CONTROLES DINAMICOS
        ''' </summary>
        ''' <param name="oRceAnamnesisE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceExamenFisicoMae_Consulta2(ByVal oRceAnamnesisE As RceAnamnesisE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceExamenFisicoMae_Consulta2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_examenfisico", oRceAnamnesisE.IdeExamenFisico)
            cmd.Parameters.AddWithValue("@ide_historia", oRceAnamnesisE.IdeHistoria)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceAnamnesisE.IdeTipoAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceAnamnesisE.CodMedico)
            cmd.Parameters.AddWithValue("@flg_estado", oRceAnamnesisE.FlgEstado)
            cmd.Parameters.AddWithValue("@orden", oRceAnamnesisE.Orden)
            'cmd.Parameters.AddWithValue("@cod_medico", oRceAnamnesisE.CodMedico)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla

        End Function

        ''' <summary>
        ''' FUNCION QUE GUARDA DATOS DE LOS CONTROLES DINAMICOS
        ''' </summary>
        ''' <param name="oRceAnamnesisE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceResultadoExamenFisicoDet_Insert2(ByVal oRceAnamnesisE As RceAnamnesisE) As RceAnamnesisE
            Dim exito As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceResultadoExamenFisicoDet_Insert2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_historia", oRceAnamnesisE.IdeHistoria)
            cmd.Parameters.AddWithValue("@usr_registra", oRceAnamnesisE.CodigoUsuario)
            cmd.Parameters.AddWithValue("@ide_atencion", oRceAnamnesisE.IdeTipoAtencion)
            'cmd.Parameters.AddWithValue("@rce_table", oRceAnamnesisE.RceTabla)
            Dim parametro As SqlParameter = cmd.Parameters.Add("@rce_table", SqlDbType.Structured)
            parametro.Value = oRceAnamnesisE.RceTabla

            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@resultado"
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.Value = oRceAnamnesisE.Resultado
            cmd.Parameters.Add(ParametroSalida)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            oRceAnamnesisE.Resultado = cmd.Parameters("@resultado").Value
            cn.Close()
            Return oRceAnamnesisE
            
        End Function





        'JB - PROBANDO - 21/01/2019 - Sp_RceResultadoExamenFisicoDet_Insert4 / Sp_RceExamenFisicoMae_Consulta5
        Public Function Sp_RceResultadoExamenFisicoDet_Insert4(ByVal oRceAnamnesisE As RceAnamnesisE) As Integer
            Dim exito As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceResultadoExamenFisicoDet_Insert4", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_historia", oRceAnamnesisE.IdeHistoria) '@ide_examenfisico
            cmd.Parameters.AddWithValue("@ide_examenfisico", oRceAnamnesisE.IdeExamenFisico)
            cmd.Parameters.AddWithValue("@dsc_idcampo", oRceAnamnesisE.DscTxtIdCampo)
            cmd.Parameters.AddWithValue("@txt_detalle", oRceAnamnesisE.TxtDetalle)
            cmd.Parameters.AddWithValue("@tip_atencion", oRceAnamnesisE.IdeTipoAtencion)
            cmd.Parameters.AddWithValue("@usr_registra", oRceAnamnesisE.CodigoUsuario)
            cmd.Parameters.AddWithValue("@rce_table", oRceAnamnesisE.RceTabla)
            cmd.Parameters.AddWithValue("@orden", oRceAnamnesisE.Orden)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            cn.Close()
            Return exito
        End Function

        Public Function Sp_RceExamenFisicoMae_Consulta5(ByVal oRceAnamnesisE As RceAnamnesisE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceExamenFisicoMae_Consulta5", cn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ide_examenfisico", oRceAnamnesisE.IdeExamenFisico)
            cmd.Parameters.AddWithValue("@ide_historia", oRceAnamnesisE.IdeHistoria)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceAnamnesisE.IdeTipoAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceAnamnesisE.CodMedico)
            cmd.Parameters.AddWithValue("@flg_estado", oRceAnamnesisE.FlgEstado)
            cmd.Parameters.AddWithValue("@orden", oRceAnamnesisE.Orden)
            'cmd.Parameters.AddWithValue("@cod_medico", oRceAnamnesisE.CodMedico)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla

        End Function

        Public Function Sp_RceExamenFisicoMae_Consulta4(ByVal pRceExamenfisicoMaeE As RceAnamnesisE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceExamenFisicoMae_Consulta4", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_examenfisico", pRceExamenfisicoMaeE.IdeExamenFisico)
            cmd.Parameters.AddWithValue("@ide_historia", pRceExamenfisicoMaeE.IdeHistoria)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", pRceExamenfisicoMaeE.IdeTipoAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", pRceExamenfisicoMaeE.CodMedico)
            cmd.Parameters.AddWithValue("@flg_estado", pRceExamenfisicoMaeE.FlgEstado)
            cmd.Parameters.AddWithValue("@orden", pRceExamenfisicoMaeE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla

        End Function

    End Class
End Namespace


