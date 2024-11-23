' ***************************************************************************************
'    Copyright Clinica San Felipe S.A.C 2024. Todos los derechos reservados.
'    Version  Fecha       Autor       Requerimiento   Objetivo específico
'    1.0      17/11/2024  FCHUJE      REQ 2024-010476 CONFIGURACION DE POLITICA DE CONTRASEÑAS_v2
'****************************************************************************************

Imports System.Configuration
Imports Entidades.OtrosE
Imports System.Data.SqlClient

Public Class Seg_UsuarioPasswordAD
    Private CnnBD As String = ConfigurationManager.ConnectionStrings("CnnBD").ConnectionString
    Public Function usp_seg_usuario_password_cambio_tmp_validar(ByVal codmedico As String, ByVal coduser As String, ByVal tipouser As String) As DataTable
        Dim cn As New SqlConnection(CnnBD)
        Dim cmd As New SqlCommand("usp_seg_usuario_password_cambio_tmp_validar", cn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@codmedico", codmedico)
        cmd.Parameters.AddWithValue("@coduser", coduser)
        cmd.Parameters.AddWithValue("@tipo", tipouser)
        cn.Open()
        Dim da As New SqlDataAdapter(cmd)
        Dim dt As New DataTable()
        da.Fill(dt)
        cn.Close()
        Return dt
    End Function

End Class
