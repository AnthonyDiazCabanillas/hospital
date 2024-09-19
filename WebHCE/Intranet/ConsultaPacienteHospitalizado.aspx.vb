Imports LogicaNegocio.HospitalLN
Imports System.Data
Imports Entidades.HospitalE

Public Class ConsultaPacienteHospitalizado
    Inherits System.Web.UI.Page
    Dim oHospitalLN As New HospitalLN
    Dim oHospitalE As New HospitalE

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ddlPisosConsultaPaciente.DataSource = oHospitalLN.Sp_Tablas_Consulta("PISOS")
            ddlPisosConsultaPaciente.DataValueField = "codigo"
            ddlPisosConsultaPaciente.DataTextField = "nombre"
            ddlPisosConsultaPaciente.DataBind()
            ddlPabellonConsultaPaciente.DataSource = oHospitalLN.Sp_Tablas_Consulta("PABELLON")
            ddlPabellonConsultaPaciente.DataValueField = "codigo"
            ddlPabellonConsultaPaciente.DataTextField = "nombre"
            ddlPabellonConsultaPaciente.DataBind()
            'ListaPacienteHospitalizado()
            Session.Abandon()
        End If
    End Sub

    'LISTA PACIENTES HOSPITALIZADOS POR PISO, PABELLOS Y BUSQUEDA (SE CARGAN LOS DATOS EN EL GRIDVIEW)
    Public Sub ListaPacienteHospitalizado()
        oHospitalE.NombrePaciente = txtBuscarPacienteHospitalizado.Value.Trim().ToUpper()
        oHospitalE.Pabellon = ddlPabellonConsultaPaciente.SelectedValue.Trim() '
        oHospitalE.Servicio = ddlPisosConsultaPaciente.SelectedValue.Trim()
        oHospitalE.Orden = 1
        gvConsultaPaciente.DataSource = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)
        gvConsultaPaciente.DataBind()
    End Sub

    Protected Sub gvConsultaPaciente_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsultaPaciente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgPaciente As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(13).Text.Trim() = "M" Then
                imgPaciente = CType(e.Row.Cells(1).FindControl("imgPacienteTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgPaciente.Src = "~/Imagenes/PacienteH.png"
            Else
                imgPaciente = CType(e.Row.Cells(1).FindControl("imgPacienteTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgPaciente.Src = "~/Imagenes/PacienteM.png"
            End If

            'when 'T' then '3'		-- Verde
            'when 'P' then '2'		-- Ambar
            'when 'G' then '1'		-- Rojo
            Dim img As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(14).Text.Trim() = "T" Then
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = "~/Imagenes/Res_Laboratorio_Verde.jpg"
            ElseIf e.Row.Cells(14).Text.Trim() = "P" Then
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = "~/Imagenes/Res_Laboratorio_Amarillo.jpg"
            Else
                img = CType(e.Row.Cells(7).FindControl("imgLaboratorioTabla"), System.Web.UI.HtmlControls.HtmlImage)
                img.Src = "~/Imagenes/Res_Laboratorio_Rojo.png"
            End If

            Dim imgImagen As New System.Web.UI.HtmlControls.HtmlImage
            If e.Row.Cells(14).Text.Trim() = "T" Then
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "~/Imagenes/Res_Imagenes_Verde.jpg"
            ElseIf e.Row.Cells(14).Text.Trim() = "P" Then
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "~/Imagenes/Res_Imagenes_Amarillo.jpg"
            Else
                imgImagen = CType(e.Row.Cells(8).FindControl("imgImagenTabla"), System.Web.UI.HtmlControls.HtmlImage)
                imgImagen.Src = "~/Imagenes/Res_Imagenes_Rojo.png"
            End If

            Dim imgIntercon As New System.Web.UI.HtmlControls.HtmlImage
            imgImagen = CType(e.Row.Cells(9).FindControl("imgInterconTabla"), System.Web.UI.HtmlControls.HtmlImage)
            imgImagen.Src = "~/Imagenes/InterconVerde.png"
        End If
    End Sub

    Protected Sub ddlPabellonConsultaPaciente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPabellonConsultaPaciente.SelectedIndexChanged
        ListaPacienteHospitalizado()
    End Sub

    Protected Sub ddlPisosConsultaPaciente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPisosConsultaPaciente.SelectedIndexChanged
        ListaPacienteHospitalizado()
    End Sub

    Protected Sub gvConsultaPaciente_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConsultaPaciente.PageIndexChanging
        gvConsultaPaciente.PageIndex = e.NewPageIndex
        ListaPacienteHospitalizado()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaAlta(ByVal CodigoAtencion As String) As String
        Dim pagina As New ConsultaPacienteHospitalizado()
        Return pagina.ValidaAlta_(CodigoAtencion)
    End Function

    Public Function ValidaAlta_(ByVal CodigoAtencion As String) As String
        Dim oHospitalLN1 As New HospitalLN()
        Dim oHospitalE1 As New HospitalE()
        oHospitalE1.NombrePaciente = CodigoAtencion
        oHospitalE1.Pabellon = ""
        oHospitalE1.Servicio = ""
        oHospitalE1.Orden = 3
        Dim tabla_atenciones As New DataTable()
        tabla_atenciones = oHospitalLN1.Sp_RceHospital_Consulta(oHospitalE1)
        Return tabla_atenciones.Rows(0)("Flg_alta").ToString().Trim()
    End Function



End Class