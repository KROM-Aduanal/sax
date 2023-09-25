Namespace gsol
    <Serializable()>
          Public Class Accion

#Region "Atributos"

        Private _claveAccion As Int32
        Private _nombreAccion As String

#End Region

#Region "Constructores"

        Sub New()
            _claveAccion = -1
            _nombreAccion = Nothing
        End Sub

        Sub New(
            ByVal claveaccion_ As Int32,
            ByVal nombreaccion_ As String
        )
            _claveAccion = claveaccion_
            _nombreAccion = nombreaccion_
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property Identificador As Object
            Get
                Return _claveAccion
            End Get
            Set(ByVal value As Object)
                _claveAccion = value
            End Set
        End Property

        Property Descripcion As Object
            Get
                Return _nombreAccion
            End Get
            Set(ByVal value As Object)
                _nombreAccion = value
            End Set
        End Property

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace
