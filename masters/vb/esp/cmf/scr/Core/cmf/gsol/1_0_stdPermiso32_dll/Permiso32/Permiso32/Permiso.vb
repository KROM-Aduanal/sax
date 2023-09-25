Namespace gsol
    <Serializable()>
       Public Class Permiso
        Implements IPermisos

#Region "Atributos"

        Private _identificador As Int32
        Private _dependencia As Int32
        Private _nombreSector As String
        Private _componente As Componente
        Private _funcion As Funcion
        Private _accion As Accion
        Private _entidades As Dictionary(Of Int32, IEntidad)

#End Region

#Region "Constructores"

        Sub New()
            _identificador = -1
            _dependencia = -1
            _nombreSector = Nothing
            _componente = New Componente
            _funcion = New Funcion
            _accion = New Accion
            _entidades = New Dictionary(Of Int32, IEntidad)
        End Sub

        Sub New(
            ByVal identificador_ As Int32, _
            ByVal dependencia_ As Int32, _
            ByVal nombreSector_ As String, _
            ByVal componente_ As Componente, _
            ByVal funcion_ As Funcion, _
            ByVal accion_ As Accion, _
            ByVal entidad_ As Entidad
        )
            _identificador = identificador_
            _dependencia = dependencia_
            _nombreSector = nombreSector_
            _componente = New Componente
            _funcion = New Funcion
            _accion = New Accion
            _entidades = New Dictionary(Of Int32, IEntidad)
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property Descripcion As String _
            Implements IPermisos.Descripcion
            Get
                Return _nombreSector
            End Get
            Set(ByVal value As String)
                _nombreSector = value
            End Set
        End Property

        Public Property Identificador As Integer _
            Implements IPermisos.Identificador
            Get
                Return _identificador
            End Get
            Set(ByVal value As Integer)
                _identificador = value
            End Set
        End Property

        Public Property Dependencia As Integer _
            Implements IPermisos.Dependencia
            Get
                Return _dependencia
            End Get
            Set(ByVal value As Integer)
                _dependencia = value
            End Set
        End Property

        Public Property Componente As Componente _
            Implements IPermisos.Componente
            Get
                Return _componente
            End Get
            Set(ByVal value As Componente)
                _componente = value
            End Set
        End Property

        Public Property Funcion As Funcion _
            Implements IPermisos.Funcion
            Get
                Return _funcion
            End Get
            Set(ByVal value As Funcion)
                _funcion = value
            End Set
        End Property

        Public Property Acciones As Accion _
            Implements IPermisos.Acciones
            Get
                Return _accion
            End Get
            Set(ByVal value As Accion)
                _accion = value
            End Set
        End Property

        Public Property Entidades As Dictionary(Of Int32, IEntidad) _
            Implements IPermisos.Entidades
            Get
                Return _entidades
            End Get
            Set(ByVal value As Dictionary(Of Int32, IEntidad))
                _entidades = value
            End Set
        End Property

        Public Property TipoAplicacion As String Implements IPermisos.TipoAplicacion
        Public Property NombreEnsamblado As String Implements IPermisos.NombreEnsamblado
        Public Property NombreContenedor As String Implements IPermisos.NombreContenedor

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace
