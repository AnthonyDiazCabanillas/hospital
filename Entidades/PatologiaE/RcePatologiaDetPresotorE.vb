Namespace PatologiaE
    Public Class RcePatologiaDetPresotorE
        
        Private _IdePatologiaDetPres As Integer
        Public Property IdePatologiaDetPres() As Integer
            Get
                Return _IdePatologiaDetPres
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaDetPres = value
            End Set
        End Property

        Private _IdePatologiaDet As Integer
        Public Property IdePatologiaDet() As Integer
            Get
                Return _IdePatologiaDet
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaDet = value
            End Set
        End Property

        Private _CodPresotor As String
        Public Property CodPresotor() As String
            Get
                Return _CodPresotor
            End Get
            Set(ByVal value As String)
                _CodPresotor = value
            End Set
        End Property

        Private _FlgEnvioExamen As String
        Public Property FlgEnvioExamen() As String
            Get
                Return _FlgEnvioExamen
            End Get
            Set(ByVal value As String)
                _FlgEnvioExamen = value
            End Set
        End Property

        Private _FecRegistro As DateTime
        Public Property FecRegistro() As DateTime
            Get
                Return _FecRegistro
            End Get
            Set(ByVal value As DateTime)
                _FecRegistro = value
            End Set
        End Property

        Private _IdeDocumentoRes As Integer
        Public Property IdeDocumentoRes() As Integer
            Get
                Return _IdeDocumentoRes
            End Get
            Set(ByVal value As Integer)
                _IdeDocumentoRes = value
            End Set
        End Property

        'Extensiones
        Private _NuevoValor As String
        Public Property NuevoValor() As String
            Get
                Return _NuevoValor
            End Get
            Set(ByVal value As String)
                _NuevoValor = value
            End Set
        End Property

        Private _Campo As String
        Public Property Campo() As String
            Get
                Return _Campo
            End Get
            Set(ByVal value As String)
                _Campo = value
            End Set
        End Property


    End Class
End Namespace