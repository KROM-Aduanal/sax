Namespace gsol

    <Serializable()>
           Public Class Componente

#Region "Atributos"

        Private _claveComponente As Int32
        Private _nombreComponente As String

#End Region

#Region "Constructores"

        Sub New()
            _claveComponente = -1
            _nombreComponente = Nothing
        End Sub

        Sub New(
            ByVal clavecomponente_ As Int32,
            ByVal nombrecomponente_ As String
        )
            _claveComponente = clavecomponente_
            _nombreComponente = nombrecomponente_
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property Identificador As Object
            Get
                Return _claveComponente
            End Get
            Set(ByVal value As Object)
                _claveComponente = value
            End Set
        End Property

        Property Descripcion As Object
            Get
                Return _nombreComponente
            End Get
            Set(ByVal value As Object)
                _nombreComponente = value
            End Set
        End Property

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace

