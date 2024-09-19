Imports Entidades.PatologiaE
Imports AccesoDatos.PatologiaAD

Namespace PatologiaLN
    Public Class RcePatologiaDetPresotorLN
        Public Function Sp_RcePatologiaDetPresotor_Update(ByVal pRcePatologiaDetPresotorE As RcePatologiaDetPresotorE) As Integer
            Return New RcePatologiaDetPresotorAD().Sp_RcePatologiaDetPresotor_Update(pRcePatologiaDetPresotorE)
        End Function
    End Class
End Namespace