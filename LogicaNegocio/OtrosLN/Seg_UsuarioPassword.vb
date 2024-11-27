' ***************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2024. Todos los derechos reservados.
'    Version  Fecha       Autor       Requerimiento   Objetivo específico
'    1.0      17/11/2024  FCHUJE      REQ 2024-010476 CONFIGURACION DE POLITICA DE CONTRASEÑAS_v2
'****************************************************************************************

Imports AccesoDatos

Public Class Seg_UsuarioPassword
    Dim oseguridad As New Seg_UsuarioPasswordAD()
    Public Function usp_seg_usuario_password_cambio_tmp_validar(ByVal codmedico As String, ByVal coduser As String) As DataTable
        Dim tipouser As String = "M"
        If codmedico = "0" Then
            tipouser = "U"
        End If

        Return oseguridad.usp_seg_usuario_password_cambio_tmp_validar(codmedico, coduser, tipouser)
    End Function
End Class
