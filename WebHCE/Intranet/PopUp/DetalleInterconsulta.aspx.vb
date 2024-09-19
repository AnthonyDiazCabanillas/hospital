Imports Entidades.InterconsultaE
Imports LogicaNegocio.InterconsultaLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE

Public Class DetalleInterconsulta
    Inherits System.Web.UI.Page

    Dim oInterconsultaE As New InterconsultaE()
    Dim oInterconsultaLN As New InterconsultaLN()
    Dim oMedicoE As New MedicoE()
    Dim oMedicoLN As New MedicoLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Not IsNothing(Request.Params("Parametro[]")) Then

                oInterconsultaE.Atencion = Session(sCodigoAtencion)
                oInterconsultaE.Orden = 1
                'cambio
                Dim tabla As New DataTable()
                tabla = oInterconsultaLN.Sp_RceInterconsulta_Consulta(oInterconsultaE)
                If tabla.Rows.Count > 0 Then
                    For index = 0 To tabla.Rows.Count - 1
                        'Observaciones Cmendez 02/05/2022
                        If tabla.Rows(index)("ide_interconsulta").ToString() = Request.Params("Parametro[]").Split("|")(0) Then
                            ddlMotivoR.Items.Insert(0, tabla.Rows(index)("dsc_motivo").ToString())
                            txtEspecialidadInterconsultaR.Value = tabla.Rows(index)("especialidad").ToString()
                            txtMedicoInterconsultaR.Value = tabla.Rows(index)("medico_solicita").ToString()
                            txtEspecialidadInterconsultaR.Value = tabla.Rows(index)("especialidad").ToString()
                            txtDescripcionInterconsultaR.Value = tabla.Rows(index)("txt_solicitud").ToString()

                            txtEspecialidadInterconsulta2R.Value = tabla.Rows(index)("especialidad").ToString()
                            txtDescripcionInterconsulta2R.Value = tabla.Rows(index)("txt_respuesta").ToString()

                            Dim dt As New DataTable()

                            If IsNothing(Session(sCodMedico)) Then
                                oMedicoE.CodMedico = 0
                            Else
                                oMedicoE.CodMedico = Session(sCodMedico)
                            End If
                            oMedicoE.Atencion = Session(sCodigoAtencion)
                            oMedicoE.Orden = 1
                            dt = oMedicoLN.Sp_RceMedicos_Consulta(oMedicoE)
                            'txtMedicoInterconsulta2R.Value = dt.Rows(0)("nombres").ToString().Trim() JB - 22/06/2020 - COMENTADO
                            txtMedicoInterconsulta2R.Value = tabla.Rows(index)("medico").ToString().Trim()


                        End If
                    Next
                End If
                'Observaciones Cmendez 02/05/2022  
                If Request.Params("Parametro[]").Split("|").Length > 1 Then

                    If Request.Params("Parametro[]").Split("|")(1) = "D" Then
                        ddlMotivoR.Enabled = False
                        txtEspecialidadInterconsultaR.Disabled = True
                        txtMedicoInterconsultaR.Disabled = True
                        txtEspecialidadInterconsultaR.Disabled = True
                        txtDescripcionInterconsultaR.Disabled = True

                        txtEspecialidadInterconsulta2R.Disabled = True
                        txtDescripcionInterconsulta2R.Disabled = True
                        txtMedicoInterconsulta2R.Disabled = True
                        hfVariableDesa.Value = "D"
                    End If

                End If

            End If

        End If
    End Sub

End Class