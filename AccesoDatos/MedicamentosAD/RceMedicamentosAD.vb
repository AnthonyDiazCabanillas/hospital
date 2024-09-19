Imports Entidades.MedicamentosE
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace MedicamentosAD
    Public Class RceMedicamentosAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
        Private CnnBD_Logistica As String = ConfigurationManager.ConnectionStrings("CnnBD_Logistica").ConnectionString

        ''' <summary>
        ''' 1- LISTA TOTAL DE MEDICAMENTOSA INGRESADA / 2- OBTIENE DATOS AL SELECCIONAR UN REGISTRO DEBE REGISTRAR 
        ''' </summary>
        ''' <param name="oRceMedicamentosE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceMedicamentosaCab_Consulta(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceMedicamentosaCab_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceMedicamentosE.IdHistoria)
            cmd.Parameters.AddWithValue("@id_medicamentosa", oRceMedicamentosE.IdMedicamentosaDet)
            cmd.Parameters.AddWithValue("@id_patologia", oRceMedicamentosE.IdPatologia)
            cmd.Parameters.AddWithValue("@orden", oRceMedicamentosE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' INSERTAR CABECERA
        ''' </summary>
        ''' <param name="oRceMedicamentosE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceMedicamentosaCab_Insert(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceMedicamentosaCab_Insert", cn)
            Dim exito As Integer

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@ide_historia", oRceMedicamentosE.IdHistoria)
            cmd.Parameters.AddWithValue("@cod_medico", oRceMedicamentosE.CodMedico)

            Dim oOutParameter1 As New SqlParameter()
            oOutParameter1.ParameterName = "@ide_medicamentosa"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceMedicamentosE.IdMedicamentosaCab
            cmd.Parameters.Add(oOutParameter1)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            'Obtener Resultados del Store
            oRceMedicamentosE.IdMedicamentosaCab = cmd.Parameters("@ide_medicamentosa").Value
            Return exito
            cn.Close()
        End Function


        Public Function Sp_RceMedicamentosaDet_Insert(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceMedicamentosaDet_Insert", cn)
            Dim exito As Integer

            cmd.CommandType = System.Data.CommandType.StoredProcedure
            'Parametros del Store
            cmd.Parameters.AddWithValue("@ide_medicamentosacab", oRceMedicamentosE.IdMedicamentosaCab)
            cmd.Parameters.AddWithValue("@cod_medico", oRceMedicamentosE.CodMedico)

            Dim oOutParameter1 As New SqlParameter()
            oOutParameter1.ParameterName = "@ide_medicamentosadet"
            oOutParameter1.SqlDbType = SqlDbType.Int
            oOutParameter1.Size = 8
            oOutParameter1.Direction = ParameterDirection.InputOutput
            oOutParameter1.Value = oRceMedicamentosE.IdMedicamentosaDet
            cmd.Parameters.Add(oOutParameter1)

            cn.Open()
            exito = cmd.ExecuteNonQuery()
            'Obtener Resultados del Store
            oRceMedicamentosE.IdMedicamentosaDet = cmd.Parameters("@ide_medicamentosadet").Value
            Return exito
            cn.Close()
        End Function

        Public Function Sp_RceMedicamentosaDet_Update(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceMedicamentosaDet_Update", cn)
            Dim exito As Integer
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@campo", oRceMedicamentosE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oRceMedicamentosE.IdMedicamentosaDet)
            cmd.Parameters.AddWithValue("@valor_nuevo ", oRceMedicamentosE.ValorNuevo)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            cn.Close()
            Return exito
        End Function




        Public Function Sp_Buscar(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Dim cn As New SqlConnection(CnnBD_Logistica)
            Dim cmd As New SqlCommand("Sp_Buscar", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@busqueda", oRceMedicamentosE.Nombre)
            cmd.Parameters.AddWithValue("@donde", oRceMedicamentosE.Modulo)
            cmd.Parameters.AddWithValue("@orden", oRceMedicamentosE.Ordenx)
            cmd.Parameters.AddWithValue("@estado", oRceMedicamentosE.Estado)
            cmd.Parameters.AddWithValue("@codigo", oRceMedicamentosE.Codigo)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

        '
        Public Function Rp_RceRecetaMedicamento1(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_RceRecetaMedicamento1", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceMedicamentosE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_receta", oRceMedicamentosE.IdMedicamentosaCab)
            cmd.Parameters.AddWithValue("@fec_receta", oRceMedicamentosE.FecReceta)
            cmd.Parameters.AddWithValue("@orden", oRceMedicamentosE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function



        Public Function Sp_RceResultadoDocumentoDet_InsertV3(ByVal oRceMedicamentosE As RceMedicamentosE) As Integer
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceResultadoDocumentoDet_InsertV3", cn)
            Dim exito As Integer
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceMedicamentosE.IdHistoria)
            cmd.Parameters.AddWithValue("@txt_detalle", oRceMedicamentosE.Detalle)
            cmd.Parameters.AddWithValue("@blb_documento", oRceMedicamentosE.Documento)
            cmd.Parameters.AddWithValue("@usr_registra", oRceMedicamentosE.CodUser)
            cmd.Parameters.AddWithValue("@cod_tipodocumento", oRceMedicamentosE.TipoDocumento)
            cmd.Parameters.AddWithValue("@flg_estado", oRceMedicamentosE.Estado)
            cmd.Parameters.AddWithValue("@cod_atencion", oRceMedicamentosE.Codigo)
            cmd.Parameters.AddWithValue("@flg_existehc", oRceMedicamentosE.FlgExisteHc)
            cmd.Parameters.AddWithValue("@fec_reporte", oRceMedicamentosE.FecReporte)
            cn.Open()
            exito = cmd.ExecuteNonQuery()
            cn.Close()
            Return exito
        End Function



        Public Function Rp_RceRecetaAlta(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_RceRecetaAlta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceMedicamentosE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_receta", oRceMedicamentosE.IdMedicamentosaCab)
            cmd.Parameters.AddWithValue("@orden", oRceMedicamentosE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function


        Public Function Rp_RceRecetaTratamiento(ByVal oRceMedicamentosE As RceMedicamentosE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Rp_RceRecetaTratamiento", cn)
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceMedicamentosE.IdHistoria)
            cmd.Parameters.AddWithValue("@ide_receta", "")
            cmd.Parameters.AddWithValue("@orden", oRceMedicamentosE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

    End Class
End Namespace

