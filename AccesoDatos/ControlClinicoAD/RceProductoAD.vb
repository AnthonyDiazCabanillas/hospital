Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Entidades.ControlClinicoE

Namespace ControlClinicoAD
    Public Class RceProductoAD
        Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
        Private CnnBD_Logistica As String = ConfigurationManager.ConnectionStrings("CnnBD_Logistica").ConnectionString
        Private BDHANNA As String = ConfigurationManager.AppSettings("BDHANNA").Trim()
        ''' <summary>
        ''' FUNCION PARA LA BUSQUEDA DE PRODUCTO/MEDICAMENTO
        ''' </summary>
        ''' <param name="oRceProductoE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Log_ConsultaProductosPedidosxAlmacen(ByVal oRceProductoE As RceProductoE) As DataTable
            Dim cn
            Dim cmd
            If BDHANNA = "SI" Then
                cn = New SqlConnection(CnnBD)
                cmd = New SqlCommand("Log_ConsultaProductosPedidosxAlmacen", cn) 'Sp_ConsultaProductosPedidosxAlmacen
            Else
                cn = New SqlConnection(CnnBD)
                cmd = New SqlCommand("Fa_ConsultaProductosPedidosxAlmacen", cn) 'Cmendez  Revisar Fa_ConsultaProductosPedidosxAlmacen
            End If

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oRceProductoE.Producto)
            cmd.Parameters.AddWithValue("@codalmacen", oRceProductoE.CodAlmacen)
            cmd.Parameters.AddWithValue("@orden", oRceProductoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt


            'Dim cn As New SqlConnection(CnnBD)
            'Dim cmd As New SqlCommand("Fa_ConsultaProductosPedidosxAlmacen", cn)
            'cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.AddWithValue("@buscar", oRceProductoE.Producto)
            'cmd.Parameters.AddWithValue("@codalmacen", oRceProductoE.CodAlmacen)
            'cmd.Parameters.AddWithValue("@orden", oRceProductoE.Orden)
            'cn.Open()
            'Dim da As New SqlDataAdapter(cmd)
            'Dim dt As New DataTable()
            'da.Fill(dt)
            'cn.Close()
            'Return dt            
        End Function

        Public Function Sp_Almacenes_Consulta(ByVal oRceProductoE As RceProductoE) As DataTable
            Dim cn
            Dim cmd

            If BDHANNA = "SI" Then
                cn = New SqlConnection(CnnBD) 'CnnBD_Logistica
                cmd = New SqlCommand("Log_Almacenes_Consulta", cn) 'Sp_Almacenes_Consulta
                
            Else
                cn = New SqlConnection(CnnBD_Logistica)
                cmd = New SqlCommand("Sp_Almacenes_Consulta", cn)
            End If

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@buscar", oRceProductoE.Nombre)
            cmd.Parameters.AddWithValue("@key", oRceProductoE.Key)
            cmd.Parameters.AddWithValue("@numerolineas", oRceProductoE.NumeroDeLineas)
            cmd.Parameters.AddWithValue("@orden", oRceProductoE.Orden)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt

            'Dim cn As New SqlConnection(CnnBD) 'CnnBD_Logistica
            'Dim cmd As New SqlCommand("Log_Almacenes_Consulta", cn) 'Sp_Almacenes_Consulta
            'cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.AddWithValue("@buscar", oRceProductoE.Nombre)
            'cmd.Parameters.AddWithValue("@key", oRceProductoE.Key)
            'cmd.Parameters.AddWithValue("@numerolineas", oRceProductoE.NumeroDeLineas)
            'cmd.Parameters.AddWithValue("@orden", oRceProductoE.Orden)
            'cn.Open()
            'Dim da As New SqlDataAdapter(cmd)
            'Dim dt As New DataTable()
            'da.Fill(dt)
            'cn.Close()

            'Return dt
        End Function


        Public Function Sp_CentroxHabitacion_Consulta(ByVal oRceProductoE As RceProductoE) As DataTable
            Dim cn As New SqlConnection(CnnBD)
            Dim cmd As New SqlCommand("Sp_CentroxHabitacion_Consulta", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@codatencion", oRceProductoE.CodAtencion)
            cn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cn.Close()

            Return dt
        End Function

    End Class
End Namespace

