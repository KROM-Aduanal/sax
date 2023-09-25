Imports gsol.BaseDatos.Operaciones

Namespace gsol.krom

    Public Class CampoVirtual

#Region "Atributos"

        Private _atributo As Object

        Private _valor As String

        Private _orden As Int32

        Private _tipoDato As IEntidadDatos.TiposDatos

        Private _descripcion As String

        Private _atributoLLave As Boolean

        Private _esFiltro As Boolean

        Private _atributoFiltro As Object

        Private _tipoRigorDato As IEntidadDatos.TiposRigorDato

        Private _tipoLLave As IEntidadDatos.TiposLlave

        Private _tipoDatoHTML As IEntidadDatos.TiposDatosHTML

        Private _estiloCCS As IEntidadDatos.EstilossCCS

        Private _esAgrupador As Boolean

        Private _funcionAgregacion As IEntidadDatos.TiposFuncionesAgregacion

        Private _tipoOrdenamiento As IEntidadDatos.TiposOrdenamiento

        Private _visible As ICaracteristica.TiposVisible

        Private _puedeInsertar As ICaracteristica.TiposRigorDatos

        Private _puedeModificar As ICaracteristica.TiposRigorDatos

        Private _nameAsKey As String

        Private _interfaz As String

        Private _idPermiso As String

        Private _tipoFiltro As String

#End Region

#Region "Constructores"

        Sub New()

            _atributo = Nothing

            _atributoFiltro = Nothing

            _valor = Nothing

            _orden = 0

            _atributoLLave = False

            _esFiltro = False

            _tipoDato = IEntidadDatos.TiposDatos.SinDefinir

            _descripcion = Nothing

            _tipoRigorDato = IEntidadDatos.TiposRigorDato.SinDefinir

            _tipoLLave = IEntidadDatos.TiposLlave.SinDefinir

            _tipoDatoHTML = IEntidadDatos.TiposDatosHTML.Texto

            _estiloCCS = IEntidadDatos.EstilossCCS.SinDefinir

            _esAgrupador = False

            _funcionAgregacion = IEntidadDatos.TiposFuncionesAgregacion.SinDefinir

            _tipoOrdenamiento = IEntidadDatos.TiposOrdenamiento.SinDefinir

            _idPermiso = Nothing

            _tipoFiltro = Nothing

            '_visible = True

            '_mostrarCampo = IEntidadDatos.MostrarCampo.No

        End Sub

#End Region

#Region "Propiedades"

        Property TipoFiltro As String

            Get

                Return _tipoFiltro

            End Get

            Set(value As String)

                _tipoFiltro = value

            End Set

        End Property

        Property IDPermiso As String

            Get
                Return _idPermiso
            End Get
            Set(value As String)
                _idPermiso = value
            End Set

        End Property

        Property NameAsKey As String

            Get
                Return _nameAsKey
            End Get
            Set(value As String)
                _nameAsKey = value
            End Set

        End Property

        Property Interfaz As String

            Get
                Return _interfaz
            End Get
            Set(value As String)
                _interfaz = value
            End Set

        End Property

        Property Visible As ICaracteristica.TiposVisible

            Get
                Return _visible
            End Get
            Set(value As ICaracteristica.TiposVisible)
                _visible = value
            End Set

        End Property

        Property PuedeInsertar As ICaracteristica.TiposRigorDatos
            Get
                Return _puedeInsertar
            End Get
            Set(value As ICaracteristica.TiposRigorDatos)
                _puedeInsertar = value
            End Set
        End Property

        Property PuedeModificar As ICaracteristica.TiposRigorDatos
            Get
                Return _puedeModificar
            End Get
            Set(value As ICaracteristica.TiposRigorDatos)
                _puedeModificar = value
            End Set
        End Property

        Property Atributo As Object
            Get
                Return _atributo
            End Get
            Set(value As Object)
                _atributo = value
            End Set
        End Property

        Property AtributoFiltro As Object
            Get
                Return _atributoFiltro
            End Get
            Set(value As Object)
                _atributoFiltro = value
            End Set
        End Property

        Property AtributoLLave As Integer
            Get
                Return _atributoLLave
            End Get
            Set(value As Integer)
                _atributoLLave = value
            End Set
        End Property

        Property Valor As String
            Get
                Return _valor
            End Get
            Set(value As String)
                _valor = value
            End Set
        End Property

        Property Orden As Int32
            Get
                Return _orden
            End Get
            Set(value As Int32)
                _orden = value
            End Set
        End Property

        Property TipoDato As IEntidadDatos.TiposDatos
            Get
                Return _tipoDato
            End Get
            Set(value As IEntidadDatos.TiposDatos)
                _tipoDato = value
            End Set
        End Property

        Property Descripcion As String
            Get
                Return _descripcion
            End Get
            Set(value As String)
                _descripcion = value
            End Set
        End Property

        Property TipoRigorDato As IEntidadDatos.TiposRigorDato
            Get
                Return _tipoRigorDato
            End Get
            Set(value As IEntidadDatos.TiposRigorDato)
                _tipoRigorDato = value
            End Set
        End Property

        Property TipoLLave As IEntidadDatos.TiposLlave
            Get
                Return _tipoLLave
            End Get
            Set(value As IEntidadDatos.TiposLlave)
                _tipoDato = value
            End Set
        End Property

        Property TipoDatoHTML As IEntidadDatos.TiposDatosHTML
            Get
                Return _tipoDatoHTML
            End Get
            Set(value As IEntidadDatos.TiposDatosHTML)
                _tipoDatoHTML = value
            End Set
        End Property

        Property EstiloCCS As IEntidadDatos.EstilossCCS
            Get
                Return _estiloCCS
            End Get
            Set(value As IEntidadDatos.EstilossCCS)
                _estiloCCS = value
            End Set
        End Property

        Property EsFiltro As Boolean
            Get
                Return _esFiltro
            End Get
            Set(value As Boolean)
                _esFiltro = value
            End Set
        End Property

        Property EsAgrupador As Boolean
            Get
                Return _esAgrupador
            End Get
            Set(value As Boolean)
                _esAgrupador = value
            End Set
        End Property

        Property FuncionAgregacion As IEntidadDatos.TiposFuncionesAgregacion
            Get
                Return _funcionAgregacion
            End Get
            Set(value As IEntidadDatos.TiposFuncionesAgregacion)
                _funcionAgregacion = value
            End Set
        End Property

        Property TipoOrdenamiento As IEntidadDatos.TiposOrdenamiento
            Get
                Return _tipoOrdenamiento
            End Get
            Set(value As IEntidadDatos.TiposOrdenamiento)
                _tipoOrdenamiento = value
            End Set
        End Property

#End Region

    End Class

End Namespace


