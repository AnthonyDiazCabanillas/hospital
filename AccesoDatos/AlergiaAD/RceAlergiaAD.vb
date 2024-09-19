Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.AlergiaE

Namespace AlergiaAD
    Public Class RceAlergiaAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString

        ''' <summary>
        ''' FUNCION PARA LISTAR PRINCIPIO ACTIVO
        ''' </summary>
        ''' <param name="oRceAlergiaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_Genericos_Consulta(ByVal oRceAlergiaE As RceAlergiaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_Genericos_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oRceAlergiaE.Buscar)
            cmd.Parameters.AddWithValue("@key", oRceAlergiaE.Key)
            cmd.Parameters.AddWithValue("@numerolineas", oRceAlergiaE.NumeroLineas)
            cmd.Parameters.AddWithValue("@orden", oRceAlergiaE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)

            cn.Close()
            Return tabla
        End Function

        ''' <summary>
        ''' PROCEDIMIENTO PARA AGREGAR DATOS EN DECLARATORIA ALERGIA
        ''' </summary>
        ''' <param name="oRceAlergiaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAlergia_Update(ByVal oRceAlergiaE As RceAlergiaE) As Boolean
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAlergia_Update", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@campo", oRceAlergiaE.Campo)
            cmd.Parameters.AddWithValue("@codigo", oRceAlergiaE.Atencion)
            cmd.Parameters.AddWithValue("@valor_nuevo", oRceAlergiaE.ValorNuevo)
            cn.Open()
            Dim actualizar As Integer
            actualizar = cmd.ExecuteNonQuery()
            'If cmd.ExecuteNonQuery() >= 1 Then
            '    Return True
            'Else
            '    Return False
            'End If
            cn.Close()
            Return True
        End Function

        ''' <summary>
        ''' FUNCION PARA LISTAR LOS PRINCIPIOS ACTIVOS GUARDADOS
        ''' </summary>
        ''' <param name="oRceAlergiaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAlergia_Consulta(ByVal oRceAlergiaE As RceAlergiaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAlergia_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@idHistoria", oRceAlergiaE.IdHistoria)
            cmd.Parameters.AddWithValue("@orden", oRceAlergiaE.Orden)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="oRceAlergiaE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Sp_RceAlergia_Validar(ByVal oRceAlergiaE As RceAlergiaE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_RceAlergia_Validar", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codproducto", oRceAlergiaE.CodProducto)
            cmd.Parameters.AddWithValue("@his_clinica", oRceAlergiaE.IdHistoria)
            cn.Open()
            Dim tabla As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(tabla)
            cn.Close()
            Return tabla
        End Function

    End Class
End Namespace


