Namespace gsol
    <Serializable()>
          Public Class EventoEntidades
        Implements IEvento

#Region "Atributos"

        Private _aplicacion As Int32
        Private _claveEntidad As Int32
        Private _claveEvento As Int32
        Private _nombreEvento As String
        Private _valor As String

#End Region

#Region "Constructores"

        Sub New()
            _aplicacion = -1
            _claveEntidad = -1
            _claveEntidad = -1
            _nombreEvento = Nothing
            _valor = Nothing

        End Sub

        Sub New(
            ByVal claveAplicacion_ As Int32,
            ByVal claveEntidad_ As Int32
        )
            _aplicacion = claveAplicacion_
            _claveEntidad = claveEntidad_
            _claveEntidad = -1
            _nombreEvento = Nothing
            _valor = Nothing

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property Aplicacion As Integer _
            Implements IEvento.Aplicacion
            Get
                Return _aplicacion
            End Get
            Set(value As Integer)
                _aplicacion = value
            End Set
        End Property

        Public Property Descripcion As String _
            Implements IEvento.Descripcion
            Get
                Return _nombreEvento
            End Get
            Set(value As String)
                _nombreEvento = value
            End Set
        End Property

        Public Property Entidad As Integer _
            Implements IEvento.Entidad
            Get
                Return _claveEntidad
            End Get
            Set(value As Integer)
                _claveEntidad = value
            End Set
        End Property

        Public Property Identificador As Integer _
            Implements IEvento.Identificador
            Get
                Return _claveEvento
            End Get
            Set(value As Integer)
                _claveEvento = value
            End Set
        End Property

        Public Property Valor As String _
            Implements IEvento.Valor
            Get
                Return _valor
            End Get
            Set(value As String)
                _valor = value
            End Set
        End Property

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace

