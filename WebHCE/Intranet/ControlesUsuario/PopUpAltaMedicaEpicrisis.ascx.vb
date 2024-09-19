Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports LogicaNegocio.MedicoLN
Imports Entidades.MedicoE

Imports System.Data
Imports Entidades.DiagnosticoE
Imports LogicaNegocio.DiagnosticoLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Public Class PopUpAltaMedicaEpicrisis
    Inherits System.Web.UI.UserControl



    Dim oMedicoE As New MedicoE
    Dim oMedicoLN As New MedicoLN
    Dim oRceDiagnosticoE As New RceDiagnosticoE()
    Dim oRceDiagnosticoLN As New RceDiagnosticoLN()
    Dim oTablasE As New TablasE
    Dim oTablasLN As New TablasLN

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim dt As New DataTable()
            dt.Columns.Add("codatencion")
            dt.Columns.Add("tipo")
            dt.Columns.Add("coddiagnostico")
            dt.Columns.Add("tipodiagnostico")
            dt.Columns.Add("clasificaciondiagnostico")
            dt.Columns.Add("fec_registro")
            dt.Columns.Add("nombre")
            dt.Columns.Add("nombretipo")
            dt.Columns.Add("nombreclasificacion")
            dt.Rows.Add()
            gvDiagnosticoAltaMedicaEP.DataSource = dt
            gvDiagnosticoAltaMedicaEP.DataBind()
            LlenarCombos()
        End If
    End Sub


    Public Sub LlenarCombos()
        oTablasE.CodTabla = "TIPODESTINO"
        oTablasE.Buscar = ""
        oTablasE.Key = 34
        oTablasE.NumeroLineas = 0
        oTablasE.Orden = -1
        Dim tabla As New DataTable()
        tabla = oTablasLN.Sp_Tablas_Consulta(oTablasE)
        cbDestinoAltaMedica1.DataSource = tabla
        cbDestinoAltaMedica1.DataValueField = "codigo"
        cbDestinoAltaMedica1.DataTextField = "nombre"
        cbDestinoAltaMedica1.DataBind()
    End Sub



End Class