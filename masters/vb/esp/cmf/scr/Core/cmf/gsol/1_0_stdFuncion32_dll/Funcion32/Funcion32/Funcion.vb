Namespace gsol

    <Serializable()>
           Public Class Funcion

#Region "Atributos"

        Private _claveFuncion As Int32
        Private _nombreFuncion As String

#End Region

#Region "Constructores"

        Sub New()
            _claveFuncion = -1
            _nombreFuncion = Nothing
        End Sub

        Sub New(
            ByVal clavefuncion_ As Int32,
            ByVal nombrefuncion_ As String
        )
            _claveFuncion = clavefuncion_
            _nombreFuncion = nombrefuncion_
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property Identificador As Object
            Get
                Return _claveFuncion
            End Get
            Set(ByVal value As Object)
                _claveFuncion = value
            End Set
        End Property

        Property Descripcion As Object
            Get
                Return _nombreFuncion
            End Get
            Set(ByVal value As Object)
                _nombreFuncion = value
            End Set
        End Property

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace

