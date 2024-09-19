Namespace AnamnesisE
    Public Class RceAnamnesisE
        Private _IdeExamenFisico As Integer
        Private _IdeExamenFisicoPadre As Integer
        Private _IdeHistoria As Integer
        Private _IdeTipoAtencion As String = ""
        Private _FlgEstado As String = ""
        Private _Orden As Integer
        Private _CodMedico As String = ""
        Private _CodigoUsuario As Integer
        Private _RceTabla As DataTable
        Private _Resultado As Integer

        Private _DscTxtIdCampo As String = ""
        Private _TxtDetalle As String = ""


        Public Property TxtDetalle() As String
            Get
                Return _TxtDetalle
            End Get
            Set(ByVal value As String)
                _TxtDetalle = value
            End Set
        End Property
        Public Property DscTxtIdCampo() As String
            Get
                Return _DscTxtIdCampo
            End Get
            Set(ByVal value As String)
                _DscTxtIdCampo = value
            End Set
        End Property

        Public Property Resultado() As Integer
            Get
                Return _Resultado
            End Get
            Set(ByVal value As Integer)
                _Resultado = value
            End Set
        End Property

        Public Property RceTabla() As DataTable
            Get
                Return _RceTabla
            End Get
            Set(ByVal value As DataTable)
                _RceTabla = value
            End Set
        End Property

        Public Property CodigoUsuario() As Integer
            Get
                Return _CodigoUsuario
            End Get
            Set(ByVal value As Integer)
                _CodigoUsuario = value
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

        Public Property IdeExamenFisico() As Integer
            Get
                Return _IdeExamenFisico
            End Get
            Set(ByVal value As Integer)
                _IdeExamenFisico = value
            End Set
        End Property
        Public Property IdeExamenFisicoPadre() As Integer
            Get
                Return _IdeExamenFisicoPadre
            End Get
            Set(ByVal value As Integer)
                _IdeExamenFisicoPadre = value
            End Set
        End Property
        Public Property IdeTipoAtencion() As String
            Get
                Return _IdeTipoAtencion
            End Get
            Set(ByVal value As String)
                _IdeTipoAtencion = value
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
    End Class
End Namespace


