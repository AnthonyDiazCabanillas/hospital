Imports AccesoDatos.PatologiaAD
Imports Entidades.PatologiaE


Namespace PatologiaLN
    Public Class RcePatologiaFavoritoMaeLN
        Dim oRcePatologiaFavoritoMaeE As New RcePatologiaFavoritoMaeE()
        Dim oRcePatologiaFavoritoMaeAD As New RcePatologiaFavoritoMaeAD()

        Public Function Sp_RcePatologiaFavoritoMae_Delete(ByVal oRcePatologiaFavoritoMaeE As RcePatologiaFavoritoMaeE) As Boolean
            Return oRcePatologiaFavoritoMaeAD.Sp_RcePatologiaFavoritoMae_Delete(oRcePatologiaFavoritoMaeE)
        End Function

        Public Function Sp_RcePatologiaFavoritoMae_Insert(ByVal oRcePatologiaFavoritoMaeE As RcePatologiaFavoritoMaeE) As Boolean
            Return oRcePatologiaFavoritoMaeAD.Sp_RcePatologiaFavoritoMae_Insert(oRcePatologiaFavoritoMaeE)
        End Function

        Public Function Sp_RcePatologiaFavoritoMae_Consulta(ByVal oRcePatologiaFavoritoMaeE As RcePatologiaFavoritoMaeE) As DataTable
            Return oRcePatologiaFavoritoMaeAD.Sp_RcePatologiaFavoritoMae_Consulta(oRcePatologiaFavoritoMaeE)
        End Function

    End Class
End Namespace

