
Namespace PatologiaE
    Public Class RcePatologiaCabE
        Private _CodAtencion As String = ""
        Private _IdeHistoria As Integer
        Private _EstExamen As String = ""
        Private _Muestra As String = ""
        Private _DatosClinico As String = ""
        Private _FecUltimaRegla As DateTime?
        Private _UsrRegistra As Integer
        Private _CodMedico As String


        Public Property CodMedico() As String
            Get
                Return _CodMedico
            End Get
            Set(ByVal value As String)
                _CodMedico = value
            End Set
        End Property
        Public Property UsrRegistra() As Integer
            Get
                Return _UsrRegistra
            End Get
            Set(ByVal value As Integer)
                _UsrRegistra = value
            End Set
        End Property
        Public Property FecUltimaRegla() As DateTime?
            Get
                Return _FecUltimaRegla
            End Get
            Set(ByVal value As DateTime?)
                _FecUltimaRegla = value
            End Set
        End Property
        Public Property DatosClinico() As String
            Get
                Return _DatosClinico
            End Get
            Set(ByVal value As String)
                _DatosClinico = value
            End Set
        End Property
        Public Property Muestra() As String
            Get
                Return _Muestra
            End Get
            Set(ByVal value As String)
                _Muestra = value
            End Set
        End Property
        Public Property EstExamen() As String
            Get
                Return _EstExamen
            End Get
            Set(ByVal value As String)
                _EstExamen = value
            End Set
        End Property
        Public Property IdeHistoria() As Integer
            Get
                Return _IdeHistoria
            End Get
            Set(ByVal value As Integer)
                _IdeHistoria = value
            End Set
        End Property
        Public Property CodAtencion() As String
            Get
                Return _CodAtencion
            End Get
            Set(ByVal value As String)
                _CodAtencion = value
            End Set
        End Property

    End Class
End Namespace

