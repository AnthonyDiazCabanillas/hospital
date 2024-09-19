
Namespace PatologiaE
    Public Class RcePatologiaFavoritoMaeE
        Private _IdePatologiaFavorito As Integer = 0
        Private _IdePatologiaMae As Integer = 0
        Private _CodMedico As String = ""
        Private _FlgEstado As String = ""
        Private _Orden As Integer = 0


        Public Property IdePatologiaMae() As Integer
            Get
                Return _IdePatologiaMae
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaMae = value
            End Set
        End Property

        Public Property IdePatologiaFavorito() As Integer
            Get
                Return _IdePatologiaFavorito
            End Get
            Set(ByVal value As Integer)
                _IdePatologiaFavorito = value
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

        Public Property CodMedico() As String
            Get
                Return _CodMedico
            End Get
            Set(ByVal value As String)
                _CodMedico = value
            End Set
        End Property

        Public Property FlgEstado() As String
            Get
                Return _FlgEstado
            End Get
            Set(ByVal value As String)
                _FlgEstado = value
            End Set
        End Property

    End Class
End Namespace

