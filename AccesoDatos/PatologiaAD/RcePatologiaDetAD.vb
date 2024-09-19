Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.PatologiaE

Namespace PatologiaAD
    Public Class RcePatologiaDetAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ToString()
        'Private oExt As New AuxiliaresAD.ExtensionesAD()

        Public Function Sp_RcePatologiaDet_Insert(ByVal pRcePatologiaDetE As RcePatologiaDetE) As Boolean
            'Sp_RcePatologiaCab_Insert
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaDet_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_patologia_cab", pRcePatologiaDetE.IdePatologiaCab)
            cmd.Parameters.AddWithValue("@ide_patologia_mae", pRcePatologiaDetE.IdePatologiaMae)
            cmd.Parameters.AddWithValue("@cod_prestacion", pRcePatologiaDetE.CodPrestacion)
            cmd.Parameters.AddWithValue("@cod_patologia", pRcePatologiaDetE.CodPatologia)
            cmd.Parameters.AddWithValue("@dsc_muestra", pRcePatologiaDetE.Muestra)
            cmd.Parameters.AddWithValue("@dsc_datosclinico", pRcePatologiaDetE.DatoClinico)
            cmd.Parameters.AddWithValue("@cnt_examen", pRcePatologiaDetE.CntExamen)
            cmd.Parameters.AddWithValue("@cod_presotor", pRcePatologiaDetE.CodPresotor)
            cmd.Parameters.AddWithValue("@flg_estado", pRcePatologiaDetE.FlgEstado)

            Dim oOutParameter1 As New SqlParameter("@ide_patologia_det", SqlDbType.Int, 8, ParameterDirection.InputOutput)
            oOutParameter1.ParameterName = "@ide_patologia_det"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = pRcePatologiaDetE.IdePatologiaDet
            cmd.Parameters.Add(oOutParameter1)
            'cmd.Parameters.Add(oExt.ParametroSQL("@ide_patologia_det", ParameterDirection.InputOutput, SqlDbType.Int, 0, pRcePatologiaDetE.IdePatologiaDet))
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            If cmd.ExecuteNonQuery() > 0 Then
                pRcePatologiaDetE.IdePatologiaDet = cmd.Parameters("@ide_patologia_det").Value
                Return True
            Else
                Return False
            End If

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Function

        Public Function Sp_RcePatologiaDet_Consulta(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaDet_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", pRcePatologiaDetE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", pRcePatologiaDetE.Orden)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If

            Return dt
        End Function

        Public Function Sp_RcePatologiaDet_ConsultaV1(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaDet_ConsultaV1", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_atencion", pRcePatologiaDetE.CodAtencion)
            cmd.Parameters.AddWithValue("@idepatologiacab", pRcePatologiaDetE.IdePatologiaCab)
            cmd.Parameters.AddWithValue("@idepatologiadet", pRcePatologiaDetE.IdePatologiaDet)
            cmd.Parameters.AddWithValue("@orden", pRcePatologiaDetE.Orden)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If

            Return dt
        End Function


        Public Function Sp_RcePatologiaDetPresotor_Consulta(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RcePatologiaDetPresotor_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@orden", pRcePatologiaDetE.Orden)
            cmd.Parameters.AddWithValue("@codatencion", pRcePatologiaDetE.CodAtencion)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If

            Return dt
        End Function

        Public Function Sp_RcePatologiaDet_Update(ByVal pRcePatologiaDetE As RcePatologiaDetE) As Integer
            Dim exito As Integer
            Using cnn As New SqlConnection(CnnBD)
                Using cmd As New SqlCommand("Sp_RcePatologiaDet_Update", cnn)
                    cmd.CommandType = System.Data.CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@ide_patologia_det", pRcePatologiaDetE.IdePatologiaDet)
                    cmd.Parameters.AddWithValue("@nuevo_valor", pRcePatologiaDetE.NuevoValor)
                    cmd.Parameters.AddWithValue("@campo", pRcePatologiaDetE.Campo)
                    cnn.Open()
                    exito = cmd.ExecuteNonQuery()

                    cmd.Dispose()
                    cnn.Dispose()
                    cnn.Close()
                End Using
            End Using

            Return exito
        End Function

        Public Function Sp_RceResultadoDocumentoDet_Consulta(ByVal pRcePatologiaDetE As RcePatologiaDetE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceResultadoDocumentoDet_Consulta", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Id_PK", pRcePatologiaDetE.CodAtencion)
            cmd.Parameters.AddWithValue("@Orden", pRcePatologiaDetE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla
        End Function

    End Class
End Namespace

