Imports AccesoDatos.PatologiaAD
Imports Entidades.PatologiaE


Namespace PatologiaLN
    Public Class RcePatologiaCabLN
        Dim oRcePatologiaCabAD As New RcePatologiaCabAD()

        Public Function Sp_RcePatologiaCab_Insert(ByVal pRcePatologiaCabE As RcePatologiaCabE) As Integer
            Return oRcePatologiaCabAD.Sp_RcePatologiaCab_Insert(pRcePatologiaCabE)
        End Function

    End Class
End Namespace

