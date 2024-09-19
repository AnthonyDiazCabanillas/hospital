Namespace MedicamentosE
    Public Class RceMedicamentosE
        Private _IdMedicamentosaCab As Integer = 0
        Private _IdMedicamentosaDet As Integer = 0
        Private _IdHistoria As Integer
        Private _Orden As Integer
        Private _CodMedico As String
        Private _Campo As String
        Private _ValorNuevo As String
        Private _IdPatologia As Integer = 0
        Private _FecReceta As String = ""

        Private _Nombre As String
        Private _Modulo As String
        Private _Estado As String
        Private _Codigo As String
        Private _Ordenx As String


        Private _Detalle As String
        Private _Documento As Byte()
        Private _TipoDocumento As Integer
        Private _FlgExisteHc As String = "S"
        Private _CodUser As Integer
        Private _FecReporte As DateTime


        Public Property FecReporte() As DateTime
            Get
                Return _FecReporte
            End Get
            Set(ByVal value As DateTime)
                _FecReporte = value
            End Set
        End Property
        Public Property CodUser() As Integer
            Get
                Return _CodUser
            End Get
            Set(ByVal value As Integer)
                _CodUser = value
            End Set
        End Property
        Public Property TipoDocumento() As Integer
            Get
                Return _TipoDocumento
            End Get
            Set(ByVal value As Integer)
                _TipoDocumento = value
            End Set
        End Property
        Public Property FlgExisteHc() As String
            Get
                Return _FlgExisteHc
            End Get
            Set(ByVal value As String)
                _FlgExisteHc = value
            End Set
        End Property
        Public Property Documento() As Byte()
            Get
                Return _Documento
            End Get
            Set(ByVal value As Byte())
                _Documento = value
            End Set
        End Property
        Public Property Detalle() As String
            Get
                Return _Detalle
            End Get
            Set(ByVal value As String)
                _Detalle = value
            End Set
        End Property

        Public Property FecReceta() As String
            Get
                Return _FecReceta
            End Get
            Set(ByVal value As String)
                _FecReceta = value
            End Set
        End Property

        Public Property Ordenx() As String
            Get
                Return _Ordenx
            End Get
            Set(ByVal value As String)
                _Ordenx = value
            End Set
        End Property
        Public Property Nombre() As String
            Get
                Return _Nombre
            End Get
            Set(ByVal value As String)
                _Nombre = value
            End Set
        End Property
        Public Property Modulo() As String
            Get
                Return _Modulo
            End Get
            Set(ByVal value As String)
                _Modulo = value
            End Set
        End Property
        Public Property Estado() As String
            Get
                Return _Estado
            End Get
            Set(ByVal value As String)
                _Estado = value
            End Set
        End Property
        Public Property Codigo() As String
            Get
                Return _Codigo
            End Get
            Set(ByVal value As String)
                _Codigo = value
            End Set
        End Property


        Public Property IdPatologia() As Integer
            Get
                Return _IdPatologia
            End Get
            Set(ByVal value As Integer)
                _IdPatologia = value
            End Set
        End Property

        Public Property ValorNuevo() As String
            Get
                Return _ValorNuevo
            End Get
            Set(ByVal value As String)
                _ValorNuevo = value
            End Set
        End Property

        Public Property Campo() As String
            Get
                Return _Campo
            End Get
            Set(ByVal value As String)
                _Campo = value
            End Set
        End Property

        Public Property CodMedico() As String
            Get
                Return _CodMedico
            End Get
            Set(ByVal value As String)
                _CodMedico = value
            End Set
        End Property

        Public Property Orden() As Integer
            Get
                Return _Orden
            End Get
            Set(ByVal value As Integer)
                _Orden = value
            End Set
        End Property

        Public Property IdMedicamentosaCab() As Integer
            Get
                Return _IdMedicamentosaCab
            End Get
            Set(ByVal value As Integer)
                _IdMedicamentosaCab = value
            End Set
        End Property

        Public Property IdMedicamentosaDet() As Integer
            Get
                Return _IdMedicamentosaDet
            End Get
            Set(ByVal value As Integer)
                _IdMedicamentosaDet = value
            End Set
        End Property

        Public Property IdHistoria() As Integer
            Get
                Return _IdHistoria
            End Get
            Set(ByVal value As Integer)
                _IdHistoria = value
            End Set
        End Property
    End Class
End Namespace


