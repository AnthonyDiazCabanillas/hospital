
Namespace InterconsultaE
    Public Class InterconsultaE
        Private _Orden As Integer
        Private _Atencion As String = ""
        Private _IdeHistoria As Integer

        Private _TipoDeAtencion As String
        Private _CodMedico As String
        Private _Nombre As String
        Private _Campo As String = ""
        Private _ValorNuevo As String = ""
        Private _IdeInterConsulta As Integer
        Private _IdeSolicitante As String = ""
        Private _IdeSolicitado As String = ""


        Private _Enviara As String = ""
        Private _Copiara As String = ""
        Private _Copiarh As String = ""
        Private _Asunto As String = ""
        Private _Cuerpo As String = ""
        Private _File As String = ""

        Private _Buscar As String = ""
        Private _Key As String = ""
        Private _Numerolineas As String = ""

        Private _FecInicio As String = ""
        Private _FecFin As String = ""

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


        Public Property Buscar() As String
            Get
                Return _Buscar
            End Get
            Set(ByVal value As String)
                _Buscar = value
            End Set
        End Property
        Public Property Key() As String
            Get
                Return _Key
            End Get
            Set(ByVal value As String)
                _Key = value
            End Set
        End Property
        Public Property Numerolineas() As String
            Get
                Return _Numerolineas
            End Get
            Set(ByVal value As String)
                _Numerolineas = value
            End Set
        End Property



        Public Property File() As String
            Get
                Return _File
            End Get
            Set(ByVal value As String)
                _File = value
            End Set
        End Property
        Public Property Cuerpo() As String
            Get
                Return _Cuerpo
            End Get
            Set(ByVal value As String)
                _Cuerpo = value
            End Set
        End Property
        Public Property Asunto() As String
            Get
                Return _Asunto
            End Get
            Set(ByVal value As String)
                _Asunto = value
            End Set
        End Property
        Public Property Copiarh() As String
            Get
                Return _Copiarh
            End Get
            Set(ByVal value As String)
                _Copiarh = value
            End Set
        End Property
        Public Property Copiara() As String
            Get
                Return _Copiara
            End Get
            Set(ByVal value As String)
                _Copiara = value
            End Set
        End Property
        Public Property Enviara() As String
            Get
                Return _Enviara
            End Get
            Set(ByVal value As String)
                _Enviara = value
            End Set
        End Property



        Public Property IdeSolicitante() As String
            Get
                Return _IdeSolicitante
            End Get
            Set(ByVal value As String)
                _IdeSolicitante = value
            End Set
        End Property

        Public Property IdeSolicitado() As String
            Get
                Return _IdeSolicitado
            End Get
            Set(ByVal value As String)
                _IdeSolicitado = value
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
        Public Property ValorNuevo() As String
            Get
                Return _ValorNuevo
            End Get
            Set(ByVal value As String)
                _ValorNuevo = value
            End Set
        End Property
        Public Property IdeInterConsulta() As Integer
            Get
                Return _IdeInterConsulta
            End Get
            Set(ByVal value As Integer)
                _IdeInterConsulta = value
            End Set
        End Property

        Public Property TipoDeAtencion() As String
            Get
                Return _TipoDeAtencion
            End Get
            Set(ByVal value As String)
                _TipoDeAtencion = value
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
        Public Property Nombre() As String
            Get
                Return _Nombre
            End Get
            Set(ByVal value As String)
                _Nombre = value
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
        Public Property Orden() As Integer
            Get
                Return _Orden
            End Get
            Set(ByVal value As Integer)
                _Orden = value
            End Set
        End Property
        Public Property Atencion() As String
            Get
                Return _Atencion
            End Get
            Set(ByVal value As String)
                _Atencion = value
            End Set
        End Property

    End Class
End Namespace


