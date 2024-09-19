Imports Entidades.EvolucionE
Imports AccesoDatos.EvolucionAD
Imports System.Data

Namespace EvolucionLN
    Public Class RceEvolucionLN
        Dim oRceEvolucionAD As New RceEvolucionAD

        Public Function Sp_RceEvolucion_Consulta(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Return oRceEvolucionAD.Sp_RceEvolucion_Consulta(oRceEvolucionE)
        End Function

        Public Function Sp_RceEvolucion_Insert(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Return oRceEvolucionAD.Sp_RceEvolucion_Insert(oRceEvolucionE)
        End Function

        Public Function Sp_RceEvolucion_Update(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Return oRceEvolucionAD.Sp_RceEvolucion_Update(oRceEvolucionE)
        End Function

        Public Function Sp_RceEvolucionLog_Insert(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Return oRceEvolucionAD.Sp_RceEvolucionLog_Insert(oRceEvolucionE)
        End Function

        Public Function Rp_RceEvolucion(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Return oRceEvolucionAD.Rp_RceEvolucion(oRceEvolucionE)
        End Function



        Public Function Sp_RceEvolucion_ConsultaV2(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Return oRceEvolucionAD.Sp_RceEvolucion_ConsultaV2(oRceEvolucionE)
        End Function
        Public Function Sp_RceEvolucionLog_InsertV2(ByVal oRceEvolucionE As RceEvolucionE) As Integer
            Return oRceEvolucionAD.Sp_RceEvolucionLog_InsertV2(oRceEvolucionE)
        End Function



        Public Function Rp_EvolucionClinicaHM(ByVal oRceEvolucionE As RceEvolucionE) As DataTable
            Return oRceEvolucionAD.Rp_EvolucionClinicaHM(oRceEvolucionE)
        End Function


    End Class
End Namespace

