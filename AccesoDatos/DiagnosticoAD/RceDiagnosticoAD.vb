Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.DiagnosticoE

Namespace DiagnosticoAD
    Public Class RceDiagnosticoAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION PARA LA BUSQUEDA DE DIAGNOSTICO
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceBuscar_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceBuscar_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oRceDiagnosticoE.Nombre)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceDiagnosticoE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@cod_medico", oRceDiagnosticoE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_atencion", oRceDiagnosticoE.CodAtencion)
            cmd.Parameters.AddWithValue("@orden", oRceDiagnosticoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA LA BUSQUEDA DE FAVORITOS DE DIAGNOSTICO
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceDiagnosticoFavoritoMae_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnosticoFavoritoMae_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_medico", oRceDiagnosticoE.CodMedico)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceDiagnosticoE.TipoDeAtencion)
            cmd.Parameters.AddWithValue("@dsc_diagnostico", oRceDiagnosticoE.Nombre)
            cmd.Parameters.AddWithValue("@orden", oRceDiagnosticoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA LISTAR LOS DIAGNOSTICOS AGREGADOS
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Diagxhospital_Consulta1(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnostico_Consulta2", cn)  'Sp_RceDiagnostico_Consulta     ***** Sp_Diagxhospital_Consulta1******
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codigo", oRceDiagnosticoE.CodAtencion)
            cmd.Parameters.AddWithValue("@tipo", oRceDiagnosticoE.Tipo)
            cmd.Parameters.AddWithValue("@coddiagnostico", oRceDiagnosticoE.CodDiagnostico)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt
        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR UN FAVORITO DE DIAGNOSTICO
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceDiagnosticoFavoritoMae_Delete(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnosticoFavoritoMae_Delete", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_diagnosticofav", oRceDiagnosticoE.IdeDiagnosticoFavorito)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA INSERTAR UN DIAGNOSTICO A FAVORITOS
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceDiagnosticoFavoritoMae_Insert(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnosticoFavoritoMae_Insert", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_medico", oRceDiagnosticoE.CodMedico)
            cmd.Parameters.AddWithValue("@cod_diagnostico", oRceDiagnosticoE.CodDiagnostico)
            cmd.Parameters.AddWithValue("@usr_registra", oRceDiagnosticoE.UsrRegistra)
            cmd.Parameters.AddWithValue("@ide_tipoatencion", oRceDiagnosticoE.TipoDeAtencion)

            Dim oParametroSalida As New SqlParameter()
            oParametroSalida.ParameterName = "@ide_diagnosticofav"
            oParametroSalida.SqlDbType = SqlDbType.Int
            oParametroSalida.Size = 8
            oParametroSalida.Direction = ParameterDirection.InputOutput
            oParametroSalida.Value = oRceDiagnosticoE.IdeDiagnosticoFavorito
            cmd.Parameters.Add(oParametroSalida)

            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                oRceDiagnosticoE.IdeDiagnosticoFavorito = cmd.Parameters("@ide_diagnosticofav").Value
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function


        ''' <summary>
        ''' FUNCION PARA INSERTAR UN NUEVO DIAGNOSTICO
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Diagxhospital_Insert(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnostico_Insert", cn) 'Sp_Diagxhospital_Insert
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceDiagnosticoE.CodAtencion)
            cmd.Parameters.AddWithValue("@tipo", oRceDiagnosticoE.Tipo)
            cmd.Parameters.AddWithValue("@coddiagnostico", oRceDiagnosticoE.CodDiagnostico)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        ''' <summary>
        ''' FUNCION PARA ACTUALIZR DIAGNOSTICO
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Diagxhospital_Update(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnostico_Update", cn)  'Sp_Diagxhospital_Update
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceDiagnosticoE.CodAtencion)
            cmd.Parameters.AddWithValue("@tipo", oRceDiagnosticoE.Tipo)
            cmd.Parameters.AddWithValue("@coddiagnostico", oRceDiagnosticoE.CodDiagnostico)
            cmd.Parameters.AddWithValue("@campo", oRceDiagnosticoE.Campo)
            cmd.Parameters.AddWithValue("@nuevovalor", oRceDiagnosticoE.NuevoValor)
            cn.Open()
            If cmd.ExecuteNonQuery() >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()

        End Function

        ''' <summary>
        ''' FUNCION PARA ELIMINAR UN DIAGNOSTICO SELECCIONADO
        ''' </summary>
        ''' <param name="oRceDiagnosticoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Diagxhospital_Delete(ByVal oRceDiagnosticoE As RceDiagnosticoE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnostico_Delete", cn)  'Sp_Diagxhospital_Delete
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceDiagnosticoE.CodAtencion)
            cmd.Parameters.AddWithValue("@tipo", oRceDiagnosticoE.Tipo)
            cmd.Parameters.AddWithValue("@coddiagnostico", oRceDiagnosticoE.CodDiagnostico)
            cn.Open()
            Dim num As Integer
            num = cmd.ExecuteNonQuery()
            If num >= 1 Then
                Return True
            Else
                Return False
            End If
            cn.Close()
        End Function

        Public Function Sp_RceDiagnostico_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceDiagnostico_Consulta1", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ide_historia", oRceDiagnosticoE.IdeHistoria)
            cmd.Parameters.AddWithValue("@tipo", oRceDiagnosticoE.Tipo)
            cmd.Parameters.AddWithValue("@orden", oRceDiagnosticoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function

        Public Function Rp_Diagxhospital_Consulta(ByVal oRceDiagnosticoE As RceDiagnosticoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Diagxhospital_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codigo", oRceDiagnosticoE.CodAtencion)
            cmd.Parameters.AddWithValue("@tipo", oRceDiagnosticoE.Tipo)
            cmd.Parameters.AddWithValue("@coddiagnostico", oRceDiagnosticoE.CodDiagnostico)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()
            Return dt

        End Function

    End Class
End Namespace

