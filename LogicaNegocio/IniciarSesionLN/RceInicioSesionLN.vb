Imports System.Data
Imports AccesoDatos.InicioSesionAD
Imports Entidades.InicioSesionE

Namespace InicioSesionLN
    Public Class RceInicioSesionLN
        Dim oRceInicioSesionAD As New RceInicioSesionAD()

        Public Function Sp_RCEAmbulatorio_IniciarSesion(ByVal oRceInicioSesionE As RceInicioSesionE) As RceInicioSesionE
            Return oRceInicioSesionAD.Sp_RCEAmbulatorio_IniciarSesion(oRceInicioSesionE)
        End Function

        Public Function Sp_Usuarios_IniciarSesion(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Return oRceInicioSesionAD.Sp_Usuarios_IniciarSesion(oRceInicioSesionE)
        End Function

        Public Function Sp_RceValidaPermiso(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Return oRceInicioSesionAD.Sp_RceValidaPermiso(oRceInicioSesionE)
        End Function


        Public Function Sp_RceInicioSesion_Update(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Return oRceInicioSesionAD.Sp_RceInicioSesion_Update(oRceInicioSesionE)
        End Function

        Public Function Sp_Rcelogs_Insert(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Return oRceInicioSesionAD.Sp_Rcelogs_Insert(oRceInicioSesionE)
        End Function

        Public Function Sp_RCEAmbulatorio_ObtenerClave(ByVal oRceInicioSesionE As RceInicioSesionE) As RceInicioSesionE
            Return oRceInicioSesionAD.Sp_RCEAmbulatorio_ObtenerClave(oRceInicioSesionE)
        End Function

        Public Function Sp_RCESegUsuario_Update(ByVal oRceInicioSesionE As RceInicioSesionE) As RceInicioSesionE
            Return oRceInicioSesionAD.Sp_RCESegUsuario_Update(oRceInicioSesionE)
        End Function

        Public Function Sp_Usuarios_IniciarSesion2(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Return oRceInicioSesionAD.Sp_Usuarios_IniciarSesion2(oRceInicioSesionE)
        End Function

    End Class
End Namespace


