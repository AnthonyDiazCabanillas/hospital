Public Class RptLogConsultaReportesE


    Private _ide_log_consulta As Integer
    Private _ide_usuario As Integer
    Private _dsc_login As String
    Private _dsc_sistema As String
    Private _dsc_archivo_rpt As String
    Private _tip_reporte As String
    Private _dsc_parametros_valores As String
    Private _rptInformesMaeE As RptInformesMaeE
    Private _Parametros As List(Of ParametroE)

    Public Property ide_log_consulta As Integer
        Get
            Return _ide_log_consulta
        End Get
        Set(value As Integer)
            _ide_log_consulta = value
        End Set
    End Property

    Public Property ide_usuario As Integer
        Get
            Return _ide_usuario
        End Get
        Set(value As Integer)
            _ide_usuario = value
        End Set
    End Property

    Public Property dsc_login As String
        Get
            Return _dsc_login
        End Get
        Set(value As String)
            _dsc_login = value
        End Set
    End Property

    Public Property dsc_sistema As String
        Get
            Return _dsc_sistema
        End Get
        Set(value As String)
            _dsc_sistema = value
        End Set
    End Property
    Public Property dsc_archivo_rpt As String
        Get
            Return _dsc_archivo_rpt
        End Get
        Set(value As String)
            _dsc_archivo_rpt = value
        End Set
    End Property

    Public Property tip_reporte As String
        Get
            Return _tip_reporte
        End Get
        Set(value As String)
            _tip_reporte = value
        End Set
    End Property

    Public Property dsc_parametros_valores As String
        Get
            Return _dsc_parametros_valores
        End Get
        Set(value As String)
            _dsc_parametros_valores = value
        End Set
    End Property

    Public Property Parametros As List(Of ParametroE)
        Get
            Return _Parametros
        End Get
        Set(value As List(Of ParametroE))
            _Parametros = value
        End Set
    End Property

    Public Property rptInformesMaeE As RptInformesMaeE
        Get
            Return _rptInformesMaeE
        End Get
        Set(value As RptInformesMaeE)
            _rptInformesMaeE = value
        End Set
    End Property
End Class



Public Class ParametroE
    Private _Dsc_Parametro As String
    Private _Valor_Parametro As String
    Private _Dsc_archivo_rpt As String

    Public Sub New(dsc_Parametro As String, valor_Parametro As String, dsc_archivo_rpt As String)
        _Dsc_Parametro = dsc_Parametro
        _Valor_Parametro = valor_Parametro
        _Dsc_archivo_rpt = dsc_archivo_rpt
    End Sub

    Public Property Dsc_Parametro As String
        Get
            Return _Dsc_Parametro
        End Get
        Set(value As String)
            _Dsc_Parametro = value
        End Set
    End Property

    Public Property Valor_Parametro As String
        Get
            Return _Valor_Parametro
        End Get
        Set(value As String)
            _Valor_Parametro = value
        End Set
    End Property

    Public Property Dsc_archivo_rpt As String
        Get
            Return _Dsc_archivo_rpt
        End Get
        Set(value As String)
            _Dsc_archivo_rpt = value
        End Set
    End Property
End Class


Public Class RptInformesMaeE
    Private _ide_informe As Integer

    Public Property ide_informe As Integer
        Get
            Return _ide_informe
        End Get
        Set(value As Integer)
            _ide_informe = value
        End Set
    End Property
End Class


Public Class RespuestaArchivoJsonE
    Private _exito As Boolean = False
    Private _message As String
    Private _ArchivoByte As Byte()

    Public Property exito As Boolean
        Get
            Return _exito
        End Get
        Set(value As Boolean)
            _exito = value
        End Set
    End Property

    Public Property message As String
        Get
            Return _message
        End Get
        Set(value As String)
            _message = value
        End Set
    End Property

    Public Property ArchivoByte As Byte()
        Get
            Return _ArchivoByte
        End Get
        Set(value As Byte())
            _ArchivoByte = value
        End Set
    End Property
End Class

'Public Class RespuestaArchivoJsonE
'    {
'        Public bool exito { Get; Set; }
'        Public String message { Get; Set; } = "";
'        Public Byte[] ArchivoByte { Get; Set; }
'    }