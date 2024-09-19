Imports Entidades.HospitalE
Imports LogicaNegocio.HospitalLN
Imports Entidades.OtrosE
Imports LogicaNegocio.OtrosLN

Public Class BusquedaProcedimientoMedico
    Inherits System.Web.UI.Page

    Dim oHospitalLN As New HospitalLN
    Dim oHospitalE As New HospitalE
    Dim oTablasE As New TablasE()
    Dim oTablasLN As New TablasLN()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim cama As String = ""
            cama = ObtenerCama()
            Dim servicio As String = ""
            oHospitalE.Cama = cama
            Dim dt As New DataTable()
            dt = oHospitalLN.ObtenerServicio(oHospitalE)

            If dt.Rows.Count > 0 Then
                servicio = dt.Rows(0)("servicio").ToString().Trim()
                ListaProcedimientos(servicio)
            End If

        End If

    End Sub

    Public Sub ListaProcedimientos(ByVal servicio As String)
        Dim dt As New DataTable()
        Dim nombre As String = ""
        Dim TipoBusqueda As String = ""
        If Request.Params("Nombre") <> Nothing Then
            nombre = Request.Params("Nombre").Trim().ToUpper()
        Else
            nombre = ""
        End If



        If Request.Params("TipoBusqueda") <> Nothing Then
            TipoBusqueda = Request.Params("TipoBusqueda").Trim().ToUpper()
            If TipoBusqueda = "F" Then
                oHospitalE.CodProcedimiento = nombre
                oHospitalE.CodMedico = Session(sCodMedico)
                oHospitalE.TipoAtencion = Session(sCodigoAtencion).ToString().Substring(0, 1)
                oHospitalE.Orden = 1
                dt = oHospitalLN.Sp_RceCptFavorito_Consulta(oHospitalE)
            Else

            End If

        End If


        'INICIO - JB - COMENTADO - 18/12/2020
        'oHospitalE.Servicio = servicio
        'oHospitalE.CodAtencion = Session(sCodigoAtencion)
        'oHospitalE.Orden = 5
        'dt = oHospitalLN.Sp_RceProcedimientos_Consulta(oHospitalE)


        'Dim items = (From p In dt.AsEnumerable()
        '       Select New With {.ide_procedimiento = p.Field(Of Integer)("ide_procedimiento"),
        '                        .dsc_procedimiento = p.Field(Of String)("dsc_procedimiento"),
        '                        .cod_prestacion = p.Field(Of String)("cod_prestacion"),
        '                        .servicio = p.Field(Of String)("servicio"),
        '                        .ide_procedimiento_padre = p.Field(Of Integer)("ide_procedimiento_padre")
        '                       }).ToList()
        'Dim filtered = items.Where(Function(x) x.dsc_procedimiento.Contains(nombre)).ToList()
        'FIN - JB - COMENTADO - 18/12/2020

        gvBusquedaProcedimientoMedico.DataSource = dt
        gvBusquedaProcedimientoMedico.DataBind()
        'Sp_RceProcedimientos_Consulta
    End Sub

    Public Function ObtenerCama() As String
        'SI ES ORDEN 3, SE USARA EL PARAMETRO NombrePaciente PARA ENVIAR EL CODIGO DE ATENCION
        oHospitalE.NombrePaciente = Session(sCodigoAtencion)
        oHospitalE.Pabellon = ""
        oHospitalE.Servicio = ""
        oHospitalE.Orden = 3
        Dim tabla As New DataTable()
        tabla = oHospitalLN.Sp_RceHospital_Consulta(oHospitalE)

        If tabla.Rows.Count > 0 Then
            Return tabla.Rows(0)("cama").ToString().Trim()
        Else
            Return ""

        End If

    End Function

End Class