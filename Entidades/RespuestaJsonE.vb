Imports Dynamitey

Public Class RespuestaJsonE
    '  public string status { get; set; }
    'Public bool exito { Get; Set; }
    '    Public String message { Get; Set; }
    '    Public dynamic Result { Get; Set; }
    '
    '    Public RespuestaJsonE()
    '    {
    '        status = "success";
    '    }

    Private _status As String
    Private _exito As Boolean
    Private _message As String

    Public Property Status As String
        Get
            Return _status
        End Get
        Set(value As String)
            _status = value
        End Set
    End Property

    Public Property Exito As Boolean
        Get
            Return _exito
        End Get
        Set(value As Boolean)
            _exito = value
        End Set
    End Property

    Public Property Message As String
        Get
            Return _message
        End Get
        Set(value As String)
            _message = value
        End Set
    End Property

End Class
