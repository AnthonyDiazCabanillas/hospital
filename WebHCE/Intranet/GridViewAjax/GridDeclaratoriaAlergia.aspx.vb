Imports Entidades.AlergiaE
Imports LogicaNegocio.AlergiaLN

Public Class GridDeclaratoriaAlergia
    Inherits System.Web.UI.Page
    Dim oRceAlergiaE As New RceAlergiaE()
    Dim oRceAlergiaLN As New RceAlergiaLN()
    Dim nNumeroPagina As Integer = 1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            ListaDeclaratoriaAlergia()
        End If
    End Sub

    Public Sub ListaDeclaratoriaAlergia()
        Dim tabla As New DataTable()
        Dim tabla_alergia As New DataTable()
        If Not IsNothing(Request.Params("Codigo")) Then 'SI SE RECIBEN NUEVOS VALORES ENTONCES...
            nNumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
            If Not IsNothing(Session(sTablaDeclaratoriaAlergia)) Then 'SI YA HAY DATOS EN EL GRID DE ALERGIA, SE AÑADIRA UN NUEVO REGISTROS CON LOS VALORES RECIBIDOS
                tabla_alergia = CType(Session(sTablaDeclaratoriaAlergia), DataTable)
                tabla_alergia.Rows.Add(Request.Params("Codigo"), Request.Params("Nombre"))
                gvDeclaratoriaAlergia.PageIndex = (nNumeroPagina - 1)
                gvDeclaratoriaAlergia.DataSource = tabla_alergia
                gvDeclaratoriaAlergia.DataBind()
                Session(sTablaDeclaratoriaAlergia) = tabla_alergia
            Else
                tabla_alergia.Columns.Add("codigo")
                tabla_alergia.Columns.Add("nombre")
                tabla_alergia.Rows.Add(Request.Params("Codigo"), Request.Params("Nombre"))
                gvDeclaratoriaAlergia.PageIndex = (nNumeroPagina - 1)
                gvDeclaratoriaAlergia.DataSource = tabla_alergia
                gvDeclaratoriaAlergia.DataBind()
                Session(sTablaDeclaratoriaAlergia) = tabla_alergia
            End If
        Else 'SI NO RECIBE PARAMETROS CARGARA SOLO LA GRILLA DE LA BASE DE DATOS

            nNumeroPagina = CType(Request.Params("Pagina").ToString().Trim(), Integer)
            If Not IsNothing(Session(sTablaDeclaratoriaAlergia)) Then
                tabla_alergia = CType(Session(sTablaDeclaratoriaAlergia), DataTable)
                gvDeclaratoriaAlergia.PageIndex = (nNumeroPagina - 1)
                Session(sTablaDeclaratoriaAlergia) = tabla_alergia
                gvDeclaratoriaAlergia.DataSource = tabla_alergia
                gvDeclaratoriaAlergia.DataBind()
            Else
                'OBTENIENDO EL REGISTRO DE ALERGIA
                oRceAlergiaE.IdHistoria = Session(sIdeHistoria)
                oRceAlergiaE.Orden = 1
                tabla = oRceAlergiaLN.Sp_RceAlergia_Consulta(oRceAlergiaE)

                tabla_alergia.Columns.Add("codigo")
                tabla_alergia.Columns.Add("nombre")
                If tabla.Rows(0)("txt_medicamentos").ToString().Trim() <> "" Then
                    'PREPARANDO EL LISTADO DE ALERGIAS
                    Dim FilaAlergia As String()
                    FilaAlergia = New String(tabla.Rows(0)("txt_medicamentos").ToString().Trim().Split("|").Length) {}
                    FilaAlergia = tabla.Rows(0)("txt_medicamentos").ToString().Trim().Split("|")

                    'LLENANDO CON LOS DATOS DE ALERGIA EN EL DATATABLE            
                    For index = 0 To FilaAlergia.Length - 1
                        tabla_alergia.Rows.Add(FilaAlergia(index).Trim().Split("*")(0).Trim(), FilaAlergia(index).Trim().Split("*")(1).Trim())
                    Next
                End If

                'MOSTRANDO LOS DATOS DEL DATATABLE
                gvDeclaratoriaAlergia.PageIndex = (nNumeroPagina - 1)
                Session(sTablaDeclaratoriaAlergia) = tabla_alergia
                gvDeclaratoriaAlergia.DataSource = tabla_alergia
                gvDeclaratoriaAlergia.DataBind()
            End If
            
        End If

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarAlergia(ByVal CodigoAlergia As String) As String
        Dim pagina As New GridDeclaratoriaAlergia()
        Return pagina.EliminarAlergia_(CodigoAlergia)
    End Function

    Public Function EliminarAlergia_(ByVal CodigoAlergia As String) As String
        Try
            Dim tabla As New DataTable()
            tabla = CType(Session(sTablaDeclaratoriaAlergia), DataTable)
            'Dim ConjuntoFila As DataRowCollection = tabla.Rows

            'If ConjuntoFila.Contains(CodigoAlergia) Then
            '    Dim fila As DataRow = ConjuntoFila.Find(CodigoAlergia)
            '    ConjuntoFila.Remove(fila)
            'End If
            'Session(sTablaDeclaratoriaAlergia) = tabla

            For index = 0 To tabla.Rows.Count - 1
                If tabla.Rows(index)("codigo").ToString().Trim() = CodigoAlergia Then
                    tabla.Rows(index).Delete()
                    Exit For
                End If
            Next

            Return "OK"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function


End Class