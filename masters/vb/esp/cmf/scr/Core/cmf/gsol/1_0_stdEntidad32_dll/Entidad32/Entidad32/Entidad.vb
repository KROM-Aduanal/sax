Namespace gsol
    <Serializable()>
           Public Class Entidad
        Implements IEntidad

#Region "Atributos"

        Private _aplicacion As Int32
        Private _claveEntidad As Int32
        Private _claveAtributo As Int32
        Private _nombreAtributo As String
        Private _valor As String
        Private _idioma As Int32

        'Nuevos se reemplazara lo demas por estos diccionarios
        Private _atributos As Dictionary(Of Int32, IAtributo)
        Private _eventos As Dictionary(Of Int32, IEvento)

#End Region

#Region "Constructores"

        Sub New()
            _aplicacion = -1
            _claveEntidad = -1
            _claveAtributo = -1
            _nombreAtributo = Nothing
            _valor = Nothing
            _idioma = -1

            'Nuevos
            _atributos = Nothing
            _eventos = Nothing

        End Sub

        Sub New(
            ByVal claveAplicacion_ As Int32,
            ByVal claveEntidad_ As Int32
        )
            _aplicacion = claveAplicacion_
            _claveEntidad = claveEntidad_
            _claveAtributo = -1
            _nombreAtributo = Nothing
            _valor = Nothing
            _idioma = -1

            'Nuevos
            _atributos = Nothing
            _eventos = Nothing

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property Aplicacion As Integer _
            Implements IEntidad.Aplicacion
            Get
                Return _aplicacion
            End Get
            Set(ByVal value As Integer)
                _aplicacion = value
            End Set
        End Property

        Public Property Entidad As Integer _
            Implements IEntidad.Entidad
            Get
                Return _claveEntidad
            End Get
            Set(ByVal value As Integer)
                _claveEntidad = value
            End Set
        End Property

        Public Property Atributos As Dictionary(Of Integer, IAtributo) _
            Implements IEntidad.Atributos
            Get
                Return _atributos
            End Get
            Set(value As Dictionary(Of Integer, IAtributo))
                _atributos = value
            End Set
        End Property
        Public Property Eventos As Dictionary(Of Integer, IEvento) _
            Implements IEntidad.Eventos
            Get
                Return _eventos
            End Get
            Set(value As Dictionary(Of Integer, IEvento))
                _eventos = value
            End Set
        End Property
#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace


