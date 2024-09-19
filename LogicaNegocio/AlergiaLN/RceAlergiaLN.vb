Imports Entidades.AlergiaE
Imports AccesoDatos.AlergiaAD

Namespace AlergiaLN
    Public Class RceAlergiaLN
        Dim oRceAlergiaAD As New RceAlergiaAD

        Public Function Sp_Genericos_Consulta(ByVal oRceAlergiaE As RceAlergiaE) As DataTable
            Return oRceAlergiaAD.Sp_Genericos_Consulta(oRceAlergiaE)
        End Function

        Public Function Sp_RceAlergia_Update(ByVal oRceAlergiaE As RceAlergiaE) As Boolean
            Return oRceAlergiaAD.Sp_RceAlergia_Update(oRceAlergiaE)
        End Function

        Public Function Sp_RceAlergia_Consulta(ByVal oRceAlergiaE As RceAlergiaE) As DataTable
            Return oRceAlergiaAD.Sp_RceAlergia_Consulta(oRceAlergiaE)
        End Function

        Public Function Sp_RceAlergia_Validar(ByVal oRceAlergiaE As RceAlergiaE) As DataTable
            Return oRceAlergiaAD.Sp_RceAlergia_Validar(oRceAlergiaE)
        End Function

    End Class
End Namespace


