Namespace OtrosE
    Public Class SisCorreoE


#Region "Extensiones"
        'Extensiones
        Private _Orden As Integer
        Public Property Orden() As Integer
            Get
                Return _Orden
            End Get
            Set(ByVal value As Integer)
                _Orden = value
            End Set
        End Property

        Private _CodPaciente As String
        Public Property CodPaciente() As String
            Get
                Return _CodPaciente
            End Get
            Set(ByVal value As String)
                _CodPaciente = value
            End Set
        End Property

        Private _DscErrorMensaje As String = ""
        Public Property DscErrorMensaje() As String
            Get
                Return _DscErrorMensaje
            End Get
            Set(ByVal value As String)
                _DscErrorMensaje = value
            End Set
        End Property

#End Region

#Region "Constructores"

#Region "New()"
        Public Sub New()
        End Sub
#End Region

#Region "New(pOrden, pCodPaciente)"
        ''' <summary>
        ''' New(pOrden, pCodPaciente)
        ''' </summary>
        ''' <param name="pOrden"></param>
        ''' <param name="pCodPaciente"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pOrden As Integer,
                       ByVal pCodPaciente As String)
            Orden = pOrden
            CodPaciente = pCodPaciente
        End Sub
#End Region

#Region "New(pOrden, pCodPaciente, pDscErrorMensaje)"
        ''' <summary>
        ''' New(pOrden, pCodPaciente)
        ''' </summary>
        ''' <param name="pOrden"></param>
        ''' <param name="pCodPaciente"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pOrden As Integer,
                       ByVal pCodPaciente As String,
                       ByVal pDscErrorMensaje As String)
            Orden = pOrden
            CodPaciente = pCodPaciente
            DscErrorMensaje = pDscErrorMensaje
        End Sub
#End Region

#End Region

    End Class
End Namespace