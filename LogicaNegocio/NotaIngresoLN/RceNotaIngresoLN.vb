Imports AccesoDatos.NotaIngresoAD
Imports Entidades.NotaIngresoE
Imports System.Data

Namespace NotaIngresoLN
    Public Class RceNotaIngresoLN
        Dim oRceNotaIngresoAD As New RceNotaIngresoAD

        Public Function Sp_RceNotaIngreso_Consulta(ByVal oRceNotaIngresoE As RceNotaIngresoE) As DataTable
            Return oRceNotaIngresoAD.Sp_RceNotaIngreso_Consulta(oRceNotaIngresoE)
        End Function


        Public Function Sp_RceNotaIngreso_Insert(ByVal oRceNotaIngresoE As RceNotaIngresoE) As Integer
            Return oRceNotaIngresoAD.Sp_RceNotaIngreso_Insert(oRceNotaIngresoE)
        End Function


        Public Function Rp_NotaIngreso(ByVal oRceNotaIngresoE As RceNotaIngresoE) As DataTable
            Return oRceNotaIngresoAD.Rp_NotaIngreso(oRceNotaIngresoE)
        End Function
    End Class
End Namespace

