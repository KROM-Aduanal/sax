Namespace gsol
    <Serializable()>
        Public Class AtributoEntidad
        Implements IAtributo

#Region "Atributos"

        Private _aplicacion As Int32
        Private _claveEntidad As Int32
        Private _claveAtributo As Int32
        Private _nombreAtributo As String
        Private _valor As String
        Private _idioma As Int32

#End Region

#Region "Constructores"

        Sub New()
            _aplicacion = -1
            _claveEntidad = -1
            _claveAtributo = -1
            _nombreAtributo = Nothing
            _valor = Nothing
            _idioma = -1

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

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property Descripcion As String _
            Implements IAtributo.Descripcion
            Get
                Return _nombreAtributo
            End Get
            Set(ByVal value As String)
                _nombreAtributo = value
            End Set
        End Property

        Public Property Identificador As Integer _
            Implements IAtributo.Identificador
            Get
                Return _claveAtributo
            End Get
            Set(ByVal value As Integer)
                _claveAtributo = value
            End Set
        End Property

        Public Property Idioma As Integer _
            Implements IAtributo.Idioma
            Get
                Return _idioma
            End Get
            Set(ByVal value As Integer)
                _idioma = value
            End Set
        End Property

        Public Property Valor As String _
            Implements IAtributo.Valor
            Get
                Return _valor
            End Get
            Set(ByVal value As String)
                _valor = value
            End Set
        End Property

        Public Property Aplicacion As Integer _
            Implements IAtributo.Aplicacion
            Get
                Return _aplicacion
            End Get
            Set(ByVal value As Integer)
                _aplicacion = value
            End Set
        End Property

        Public Property Entidad As Integer _
            Implements IAtributo.Entidad
            Get
                Return _claveEntidad
            End Get
            Set(ByVal value As Integer)
                _claveEntidad = value
            End Set
        End Property

#End Region

#Region "Metodos"

#End Region
    End Class

End Namespace
