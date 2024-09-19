' ***************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2024. Todos los derechos reservados.
'    Version Fecha      Autor       Requerimiento   Objetivo específico
'    1.0     02/02/2024 CRODRIGUEZ  REQ 2023-021287 Se envia correo al agregar o cambiar
'                                                   dieta del paciente 
'****************************************************************************************
Imports System.Data
Imports Entidades.ControlClinicoE
Imports AccesoDatos.ControlClinicoAD
Imports Entidades.OtrosE '1.0

Namespace ControlClinicoLN
    Public Class RceRecetaMedicamentoLN
        Dim oRceRecetaMedicamentoAD As New RceRecetaMedicamentoAD()

        Public Function Sp_RceRecetaMedicamentoCab_Insert(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaMedicamentoCab_Insert(oRceRecetaMedicamentoE)
        End Function

        Public Function Sp_RceRecetaMedicamentoDet_Insert(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaMedicamentoDet_Insert(oRceRecetaMedicamentoE)
        End Function

        Public Function Sp_RceRecetaMedicamentoCab_Update(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaMedicamentoCab_Update(oRceRecetaMedicamentoE)
        End Function

        Public Function Sp_RceRecetaMedicamentoDet_Update(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaMedicamentoDet_Update(oRceRecetaMedicamentoE)
        End Function

        Public Function Sp_RceRecetaMedicamentoCab_Consulta(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As DataTable
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaMedicamentoCab_Consulta(oRceRecetaMedicamentoE)
        End Function


        Public Function Sp_RceRecetaSuspension_Insert(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaSuspension_Insert(oRceRecetaMedicamentoE)
        End Function

        Public Function Sp_RceRecetaSuspension_Update(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As Integer
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaSuspension_Update(oRceRecetaMedicamentoE)
        End Function


        Public Function Sp_RceRecetaMedicamentoCab_ConsultaV2(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As DataTable
            Return oRceRecetaMedicamentoAD.Sp_RceRecetaMedicamentoCab_ConsultaV2(oRceRecetaMedicamentoE)
        End Function

        Public Function Sp_RceFarmacoDosis_Consulta(ByVal oRceRecetaMedicamentoE As RceRecetaMedicamentoE) As DataTable
            Return oRceRecetaMedicamentoAD.Sp_RceFarmacoDosis_Consulta(oRceRecetaMedicamentoE)
        End Function
        'INI 1.0
        Public Function Sp_Rce_Envia_Correo_Dietas(ByVal pSisCorreoE As SisCorreoE) As String
            Return oRceRecetaMedicamentoAD.Sp_Rce_Envia_Correo_Dietas(pSisCorreoE)
        End Function
        'FIN 1.0
    End Class
End Namespace
