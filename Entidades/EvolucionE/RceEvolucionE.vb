Namespace EvolucionE
    Public Class RceEvolucionE
        Private _CodigoAtencion As String
        Private _IdHistoria As Integer
        Private _CodMedico As String
        Private _Observacion As String

        Private _Campo As String
        Private _ValorNuevo As String
        Private _TipoEducacionInforme As String
        Private _CodigoEvolucion As Integer = 0
        Private _IdeOrdenCab As Integer = 0
        Private _Orden As Integer = 0


        Private _FecEvolucion As String = ""
        Private _HorEvolucion As String = ""

        Private _CodDiagnostico As String = ""


        Private _FecInicio As String = ""
        Private _FecFin As String = ""
        Private _CodServicio As String = ""

        Public Property CodServicio() As String
            Get
                Return _CodServicio
            End Get
            Set(ByVal value As String)
                _CodServicio = value
            End Set
        End Property
        Public Property FecInicio() As String
            Get
                Return _FecInicio
            End Get
            Set(ByVal value As String)
                _FecInicio = value
            End Set
        End Property
        Public Property FecFin() As String
            Get
                Return _FecFin
            End Get
            Set(ByVal value As String)
                _FecFin = value
            End Set
        End Property


        Public Property CodDiagnostico() As String
            Get
                Return _CodDiagnostico
            End Get
            Set(ByVal value As String)
                _CodDiagnostico = value
            End Set
        End Property

        Public Property HorEvolucion() As String
            Get
                Return _HorEvolucion
            End Get
            Set(ByVal value As String)
                _HorEvolucion = value
            End Set
        End Property

        Public Property FecEvolucion() As String
            Get
                Return _FecEvolucion
            End Get
            Set(ByVal value As String)
                _FecEvolucion = value
            End Set
        End Property


        Public Property IdeOrdenCab() As Integer
            Get
                Return _IdeOrdenCab
            End Get
            Set(ByVal value As Integer)
                _IdeOrdenCab = value
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

        Public Property CodigoEvolucion() As Integer
            Get
                Return _CodigoEvolucion
            End Get
            Set(ByVal value As Integer)
                _CodigoEvolucion = value
            End Set
        End Property
        Public Property TipoEducacionInforme() As String
            Get
                Return _TipoEducacionInforme
            End Get
            Set(ByVal value As String)
                _TipoEducacionInforme = value
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
        Public Property Observacion() As String
            Get
                Return _Observacion
            End Get
            Set(ByVal value As String)
                _Observacion = value
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
        Public Property IdHistoria() As Integer
            Get
                Return _IdHistoria
            End Get
            Set(ByVal value As Integer)
                _IdHistoria = value
            End Set
        End Property
        Public Property CodigoAtencion() As String
            Get
                Return _CodigoAtencion
            End Get
            Set(ByVal value As String)
                _CodigoAtencion = value
            End Set
        End Property
    End Class
End Namespace

