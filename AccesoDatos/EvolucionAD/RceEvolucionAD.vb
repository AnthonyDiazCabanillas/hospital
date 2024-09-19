Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.EvolucionE

Namespace EvolucionAD
    Public Class RceEvolucionAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION PARA LISTAR EVOLUCION CLINICA
        ''' </summary>
        ''' <param name="oRceEvolucionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceEvolucion_Consulta(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceEvolucion_Consulta", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codigo", oRceEvolucionE.CodigoAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function

        ''' <summary>
        ''' FUNCION PARA GUARDAR EVOLUCION CLINICA
        ''' </summary>
        ''' <param name="oRceEvolucionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceEvolucion_Insert(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim guardado As Integer
            Dim cmd As New SqlCommand("Sp_RceEvolucion_Insert", cn) 'Sp_RceEvolucion_Insert2 05/08/2015
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceEvolucionE.IdHistoria)
            cmd.Parameters.AddWithValue("@cod_medico", oRceEvolucionE.CodMedico)
            cmd.Parameters.AddWithValue("@observacion", oRceEvolucionE.Observacion)
            'cmd.Parameters.AddWithValue("@tipo", oRceEvolucionE.TipoEducacionInforme) 05/08/2016

            Dim oOutParameter1 As New SqlParameter("@ide_evolucion", SqlDbType.Int, 8, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@ide_evolucion"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceEvolucionE.CodigoEvolucion
            cmd.Parameters.Add(oOutParameter1)

            cn.Open()
            guardado = cmd.ExecuteNonQuery()
            oRceEvolucionE.CodigoEvolucion = cmd.Parameters("@ide_evolucion").Value

            cn.Close()
            Return guardado
        End Function

        ''' <summary>
        ''' FUNCIION PARA ACTUALIZAR EVOLUCION CLINICA
        ''' </summary>
        ''' <param name="oRceEvolucionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceEvolucion_Update(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim guardado As Integer
            Dim cmd As New SqlCommand("Sp_RceEvolucion_Update", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@campo", oRceEvolucionE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oRceEvolucionE.CodigoEvolucion)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceEvolucionE.ValorNuevo)
            cn.Open()
            guardado = cmd.ExecuteNonQuery()
            cn.Close()
            Return guardado
        End Function

        ''' <summary>
        ''' FUNCION PARA GUARDAR EN EVOLUCION CLINICA CUANDO SE ENVIE SOLCIDITUDES DE LABORATORIO/IMAGENES
        ''' </summary>
        ''' <param name="oRceEvolucionE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceEvolucionLog_Insert(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim guardado As Integer
            Dim cmd As New SqlCommand("Sp_RceEvolucionLog_Insert", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceEvolucionE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_ordencab", oRceEvolucionE.IdeOrdenCab)
            Dim ParametroSalida As New SqlParameter()
            ParametroSalida.ParameterName = "@flg_exito"
            ParametroSalida.SqlDbType = SqlDbType.Int
            ParametroSalida.Size = 8
            ParametroSalida.Direction = ParameterDirection.InputOutput
            ParametroSalida.Value = oRceEvolucionE.CodigoEvolucion
            cmd.Parameters.Add(ParametroSalida)

            'cmd.Parameters.AddWithValue("@flg_exito", oRceEvolucionE.CodigoEvolucion)
            cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)
            cn.Open()
            guardado = cmd.ExecuteNonQuery()
            oRceEvolucionE.CodigoEvolucion = cmd.Parameters("@flg_exito").Value
            cn.Close()
            Return guardado
        End Function


        Public Function Sp_RceEvolucionLog_InsertV2(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim guardado As Integer
            Dim cmd As New SqlCommand("Sp_RceEvolucionLog_InsertV2", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceEvolucionE.IdHistoria)
            cmd.Parameters.AddWithValue("@cod_medico", oRceEvolucionE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_diagnostico", oRceEvolucionE.CodDiagnostico)
            'Dim ParametroSalida As New SqlParameter()
            'ParametroSalida.ParameterName = "@flg_exito"
            'ParametroSalida.SqlDbType = SqlDbType.Int
            'ParametroSalida.Size = 8
            'ParametroSalida.Direction = ParameterDirection.InputOutput
            'ParametroSalida.Value = oRceEvolucionE.CodigoEvolucion
            'cmd.Parameters.Add(ParametroSalida)
            'cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)

            Dim oOutParameter4 As New SqlParameter()
            oOutParameter4.ParameterName = "@flg_exito"
            oOutParameter4.Direction = ParameterDirection.InputOutput
            oOutParameter4.SqlDbType = SqlDbType.Int
            oOutParameter4.Size = 8
            oOutParameter4.Value = oRceEvolucionE.CodigoEvolucion
            cmd.Parameters.Add(oOutParameter4)

            cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)
            cn.Open()
            guardado = cmd.ExecuteNonQuery()
            oRceEvolucionE.CodigoEvolucion = cmd.Parameters("@flg_exito").Value
            cn.Close()
            Return guardado
        End Function



        Public Function Rp_RceEvolucion(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_RceEvolucion", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceEvolucionE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_evolucion", oRceEvolucionE.CodigoEvolucion)
            cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function





        Public Function Sp_RceEvolucion_ConsultaV2(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceEvolucion_ConsultaV2", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceEvolucionE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_evolucion", oRceEvolucionE.CodigoEvolucion)
            cmd.Parameters.AddWithValue("@fec_evolucion", oRceEvolucionE.FecEvolucion)
            cmd.Parameters.AddWithValue("@hor_evolucion", oRceEvolucionE.HorEvolucion)
            cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function




        Public Function Rp_EvolucionClinicaHM(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_EvolucionClinicaHM", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@FecIni", oRceEvolucionE.FecInicio)
            cmd.Parameters.AddWithValue("@FecFin", oRceEvolucionE.FecFin)
            cmd.Parameters.AddWithValue("@CodServicio", oRceEvolucionE.CodServicio)
            cmd.Parameters.AddWithValue("@orden", oRceEvolucionE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function
    End Class
End Namespace
