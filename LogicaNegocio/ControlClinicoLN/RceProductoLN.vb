Imports Entidades.ControlClinicoE
Imports AccesoDatos.ControlClinicoAD

Namespace ControlClinicoLN
    Public Class RceProductoLN
        Dim oRceProductoAD As New RceProductoAD()

        Public Function Log_ConsultaProductosPedidosxAlmacen(ByVal oRceProductoE As RceProductoE) As DataTable
            Return oRceProductoAD.Log_ConsultaProductosPedidosxAlmacen(oRceProductoE)
        End Function

        Public Function Sp_Almacenes_Consulta(ByVal oRceProductoE As RceProductoE) As DataTable
            Return oRceProductoAD.Sp_Almacenes_Consulta(oRceProductoE)
        End Function

        Public Function Sp_CentroxHabitacion_Consulta(ByVal oRceProductoE As RceProductoE) As DataTable
            Return oRceProductoAD.Sp_CentroxHabitacion_Consulta(oRceProductoE)
        End Function
    End Class
End Namespace

