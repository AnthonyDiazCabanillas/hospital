''**************************************************************************************************************************
''Objetivo General:
''------------------------------------------------------------
''VERSIÓN    FECHA			AUTOR       REQUERIMIENTO		DESCRIPCIÓN
''1.1		 17/09/2024		MBARDALES	REQ 2024-010476		CONFIGURACION DE POLITICA DE CONTRASEÑAS
''*****************************************************************************************************************************

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
        Public Function Sp_RCEAmbulatorio_ObtenerRutaApiPassword() As DataTable
            Return oRceInicioSesionAD.Sp_RCEAmbulatorio_ObtenerRutaApiPassword()
        End Function
        Public Function sp_seglogclave_sel_clinica(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Return oRceInicioSesionAD.sp_seglogclave_sel_clinica(oRceInicioSesionE)
        End Function

        Public Function Sp_ParamSeguridad_Sel() As DataTable
            Return oRceInicioSesionAD.Sp_ParamSeguridad_Sel()
        End Function

        Public Function Sp_Usuarios_IniciarSesion2(ByVal oRceInicioSesionE As RceInicioSesionE) As DataTable
            Return oRceInicioSesionAD.Sp_Usuarios_IniciarSesion2(oRceInicioSesionE)
        End Function

        '1.1 INI
        Public Function Sp_RCEAmbulatorio_ActualizaSesionBloqueo(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Return oRceInicioSesionAD.Sp_RCEAmbulatorio_ActualizaSesionBloqueo(oRceInicioSesionE)
        End Function
        '1.1 FIN

        '1.1 INI
        Public Function Sp_RCEAmbulatorio_bloqueoAsistencial(ByVal oRceInicioSesionE As RceInicioSesionE) As Boolean
            Return oRceInicioSesionAD.Sp_RCEAmbulatorio_bloqueoAsistencial(oRceInicioSesionE)
        End Function
        '1.1 FIN

    End Class
End Namespace


