Public Class EscalaEIndicacionesE
    Private _groupcab As Integer
    Private _dsc_detalle As String
    Private _itemcab As Integer
    Private _detalleitem As String
    Private _ide_escaladet As Integer
    Private _i_order As Integer
    Private _descripcion_det As String
    Private _descripcion_det2 As String
    Private _puntuacion As Integer
    Private _imagencab As String
    Private _imagendet As String

    Public Property detalleitem() As String
        Get
            Return _detalleitem
        End Get
        Set(value As String)
            _detalleitem = value
        End Set
    End Property

    Public Property groupcab() As Integer
        Get
            Return _groupcab
        End Get
        Set(value As Integer)
            _groupcab = value
        End Set
    End Property

    Public Property dsc_detalle() As String
        Get
            Return _dsc_detalle
        End Get
        Set(value As String)
            _dsc_detalle = value
        End Set
    End Property
    Public Property itemcab() As Integer
        Get
            Return _itemcab
        End Get
        Set(value As Integer)
            _itemcab = value
        End Set
    End Property
    Public Property ide_escaladet() As Integer
        Get
            Return _ide_escaladet
        End Get
        Set(value As Integer)
            _ide_escaladet = value
        End Set
    End Property
    Public Property i_order() As Integer
        Get
            Return _i_order
        End Get
        Set(value As Integer)
            _i_order = value
        End Set
    End Property
    Public Property descripcion_det() As String
        Get
            Return _descripcion_det
        End Get
        Set(value As String)
            _descripcion_det = value
        End Set
    End Property
    Public Property descripcion_det2() As String
        Get
            Return _descripcion_det2
        End Get
        Set(value As String)
            _descripcion_det2 = value
        End Set
    End Property
    Public Property puntuacion() As Integer
        Get
            Return _puntuacion
        End Get
        Set(value As Integer)
            _puntuacion = value
        End Set
    End Property
    Public Property imagencab() As String
        Get
            Return _imagencab
        End Get
        Set(value As String)
            _imagencab = value
        End Set
    End Property
    Public Property imagendet() As String
        Get
            Return _imagendet
        End Get
        Set(value As String)
            _imagendet = value
        End Set
    End Property
End Class


Public Class EscalaEIndicacionesActividadE
    Private _item1 As Integer
    Private _codigo1 As Integer
    Private _actividad1 As String
    Private _detalle1 As String
    Private _ckeck As Boolean

    Public Property Item As Integer
        Get
            Return _item1
        End Get
        Set(value As Integer)
            _item1 = value
        End Set
    End Property

    Public Property Codigo As Integer
        Get
            Return _codigo1
        End Get
        Set(value As Integer)
            _codigo1 = value
        End Set
    End Property

    Public Property Actividad As String
        Get
            Return _actividad1
        End Get
        Set(value As String)
            _actividad1 = value
        End Set
    End Property

    Public Property Detalle As String
        Get
            Return _detalle1
        End Get
        Set(value As String)
            _detalle1 = value
        End Set
    End Property

    Public Property Ckeck As Boolean
        Get
            Return _ckeck
        End Get
        Set(value As Boolean)
            _ckeck = value
        End Set
    End Property
End Class

Public Class PuntuacionEscalaE
    Private _Tipo As Integer
    Private _puntaje1 As Integer
    Private _puntaje2 As Integer
    Private _puntaje3 As Integer
    Private _puntaje4 As Integer
    Private _puntaje5 As Integer
    Private _puntaje6 As Integer
    Private _puntaje7 As Integer
    Private _total As Integer
    Private _NomUser As String
    Private _CodUser As String
    Private _Perfil As String
    Private _CodMedico As String
    Private _CodigoAtencion As String
    Private _IdeHistoria As String
    Private _CodPaciente As String
    Private _Valor As String
    Public Property Tipo As Integer
        Get
            Return _Tipo
        End Get
        Set(value As Integer)
            _Tipo = value
        End Set
    End Property

    Public Property Puntaje1 As Integer
        Get
            Return _puntaje1
        End Get
        Set(value As Integer)
            _puntaje1 = value
        End Set
    End Property

    Public Property Puntaje2 As Integer
        Get
            Return _puntaje2
        End Get
        Set(value As Integer)
            _puntaje2 = value
        End Set
    End Property

    Public Property Puntaje3 As Integer
        Get
            Return _puntaje3
        End Get
        Set(value As Integer)
            _puntaje3 = value
        End Set
    End Property

    Public Property Puntaje4 As Integer
        Get
            Return _puntaje4
        End Get
        Set(value As Integer)
            _puntaje4 = value
        End Set
    End Property

    Public Property Puntaje5 As Integer
        Get
            Return _puntaje5
        End Get
        Set(value As Integer)
            _puntaje5 = value
        End Set
    End Property

    Public Property Puntaje6 As Integer
        Get
            Return _puntaje6
        End Get
        Set(value As Integer)
            _puntaje6 = value
        End Set
    End Property

    Public Property Puntaje7 As Integer
        Get
            Return _puntaje7
        End Get
        Set(value As Integer)
            _puntaje7 = value
        End Set
    End Property

    Public Property Total As Integer
        Get
            Return _total
        End Get
        Set(value As Integer)
            _total = value
        End Set
    End Property

    Public Property NomUser As String
        Get
            Return _NomUser
        End Get
        Set(value As String)
            _NomUser = value
        End Set
    End Property

    Public Property CodUser As String
        Get
            Return _CodUser
        End Get
        Set(value As String)
            _CodUser = value
        End Set
    End Property

    Public Property Perfil As String
        Get
            Return _Perfil
        End Get
        Set(value As String)
            _Perfil = value
        End Set
    End Property

    Public Property CodMedico As String
        Get
            Return _CodMedico
        End Get
        Set(value As String)
            _CodMedico = value
        End Set
    End Property

    Public Property CodigoAtencion As String
        Get
            Return _CodigoAtencion
        End Get
        Set(value As String)
            _CodigoAtencion = value
        End Set
    End Property

    Public Property IdeHistoria As String
        Get
            Return _IdeHistoria
        End Get
        Set(value As String)
            _IdeHistoria = value
        End Set
    End Property

    Public Property CodPaciente As String
        Get
            Return _CodPaciente
        End Get
        Set(value As String)
            _CodPaciente = value
        End Set
    End Property

    Public Property Valor As String
        Get
            Return _Valor
        End Get
        Set(value As String)
            _Valor = value
        End Set
    End Property
End Class

Public Class EscalaeIndicacionesHistoricoE
    'Public DateTime    Fecha       { Get; Set; }
    'Public int         Ide_Escala  { Get; Set; }
    'Public String      Escala      { Get; Set; }
    'Public String      Hora        { Get; Set; }
    'Public int         Puntaje     { Get; Set; }
    'Public String      Encargado   { Get; Set; }
    Private _Fecha As DateTime
    Private _Ide_Escala As Integer
    Private _Escala As String
    Private _Hora As String
    Private _Puntaje As Integer
    Private _Encargado As String
    Private _ide_escalaeintervenciondet As Integer

    Public Property Fecha As Date
        Get
            Return _Fecha
        End Get
        Set(value As Date)
            _Fecha = value
        End Set
    End Property

    Public Property Ide_Escala As Integer
        Get
            Return _Ide_Escala
        End Get
        Set(value As Integer)
            _Ide_Escala = value
        End Set
    End Property

    Public Property Escala As String
        Get
            Return _Escala
        End Get
        Set(value As String)
            _Escala = value
        End Set
    End Property

    Public Property Hora As String
        Get
            Return _Hora
        End Get
        Set(value As String)
            _Hora = value
        End Set
    End Property

    Public Property Puntaje As Integer
        Get
            Return _Puntaje
        End Get
        Set(value As Integer)
            _Puntaje = value
        End Set
    End Property

    Public Property Encargado As String
        Get
            Return _Encargado
        End Get
        Set(value As String)
            _Encargado = value
        End Set
    End Property

    Public Property ide_escalaeintervenciondet As Integer
        Get
            Return _ide_escalaeintervenciondet
        End Get
        Set(value As Integer)
            _ide_escalaeintervenciondet = value
        End Set
    End Property

End Class

Public Class EscalaEIndicacionesHistoriaDetalladoE
    'Public String Concepto { Get; Set; }
    '    Public String Descripcion { Get; Set; }
    '    Public String Observacion { Get; Set; }
    '    Public int Puntaje { Get; Set; }

    Private _Concepto As String
    Private _Descripcion As String
    Private _Observacion As String
    Private _Puntaje As Integer

    Public Property Concepto As String
        Get
            Return _Concepto
        End Get
        Set(value As String)
            _Concepto = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(value As String)
            _Descripcion = value
        End Set
    End Property

    Public Property Observacion As String
        Get
            Return _Observacion
        End Get
        Set(value As String)
            _Observacion = value
        End Set
    End Property

    Public Property Puntaje As Integer
        Get
            Return _Puntaje
        End Get
        Set(value As Integer)
            _Puntaje = value
        End Set
    End Property
End Class