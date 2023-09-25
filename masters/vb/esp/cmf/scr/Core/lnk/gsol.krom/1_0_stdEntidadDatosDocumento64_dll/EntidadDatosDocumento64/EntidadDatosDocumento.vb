Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Syn.Documento
Imports Wma.Exceptions

Namespace gsol.krom

    <Serializable()>
    Public Class EntidadDatosDocumento
        Inherits DocumentoElectronico
        Implements IEntidadDatos, ICloneable, IDisposable

        Private _dimension As IEnlaceDatos.TiposDimension

        Private disposedValue As Boolean

        <NonSerialized>
        Private _newRow As DataRow

        <NonSerialized>
        Private _atributos As List(Of krom.CampoVirtual)

        <NonSerialized>
        Private _clausulas As String

        <NonSerialized>
        Private _status As Global.Wma.Exceptions.TagWatcher

        <NonSerialized>
        Private _updateOnKeyValues As List(Of String)

        <NonSerialized>
        Private _deteleOnKeyValues As List(Of String)

        '<NonSerialized>
        'Private _subscriptionsGroup As subscriptionsgroup

#Region "Properties"

        '<BsonIgnore>
        'Public Property Subscriptions As subscriptionsgroup Implements IEntidadDatos.SubscriptionsGroup
        '    Get
        '        Return MyBase.SubscriptionsGroup
        '    End Get
        '    Set(value As subscriptionsgroup)
        '        MyBase.SubscriptionsGroup = value
        '    End Set
        'End Property

        <BsonIgnore>
        Public Property NewRow As DataRow Implements IEntidadDatos.NewRow
            Get
                Return _newRow
            End Get
            Set(value As DataRow)
                _newRow = value
            End Set
        End Property

        <BsonIgnore>
        Public Property Dimension As IEnlaceDatos.TiposDimension _
            Implements IEntidadDatos.Dimension

            Get

                Return _dimension

            End Get

            Set(value As IEnlaceDatos.TiposDimension)

                _dimension = value

            End Set
        End Property

        <BsonIgnore>
        Public ReadOnly Property CampoPorNombre(atributoDimension_ As Object) As Object Implements IEntidadDatos.CampoPorNombre
            Get
                '        Throw New NotImplementedException()
            End Get
        End Property

        <BsonIgnore>
        Public Property Atributos As List(Of krom.CampoVirtual) Implements IEntidadDatos.Atributos
            Get
                Return _atributos
            End Get
            Set(value As List(Of krom.CampoVirtual))
                _atributos = value
            End Set
        End Property

        <BsonIgnore>
        Public Property Clausulas As String Implements IEntidadDatos.Clausulas
            Get
                Return _clausulas
            End Get
            Set(value As String)
                _clausulas = value
            End Set
        End Property

        <BsonIgnore>
        Public Property Status As Global.Wma.Exceptions.TagWatcher Implements IEntidadDatos.Status
            Get
                Return _status
            End Get
            Set(value As Global.Wma.Exceptions.TagWatcher)
                _status = value
            End Set
        End Property

        <BsonIgnore>
        Public Property Attribute(attributeName_ As Object) As Object Implements IEntidadDatos.Attribute
            Get
                '        Throw New NotImplementedException()
            End Get
            Set(value As Object)
                '        'Me.Dimension = IEnlaceDatos.TiposDimension.Seguimiento
                '        'Me.Referencia = ""
                '        Throw New NotImplementedException()
            End Set
        End Property

        <BsonIgnore>
        Public Property UpdateOnKeyValues As List(Of String) Implements IEntidadDatos.UpdateOnKeyValues
            Get
                Return _updateOnKeyValues
            End Get
            Set(value As List(Of String))
                _updateOnKeyValues = value
            End Set
        End Property

        <BsonIgnore>
        Public Property DeteleOnKeyValues As List(Of String) Implements IEntidadDatos.DeteleOnKeyValues
            Get
                Return _deteleOnKeyValues
            End Get
            Set(value As List(Of String))
                _deteleOnKeyValues = value
            End Set
        End Property

#End Region


#Region "Methods"

#End Region
        Public Sub Array(arrayOfDataEntity_ As List(Of IEntidadDatos)) Implements IEntidadDatos.Array
            '    Throw New NotImplementedException()
        End Sub

        Public Sub cmp(nombreCampo_ As Object) Implements IEntidadDatos.cmp
            '    Throw New NotImplementedException()
        End Sub

        Public Sub cmp(nombreCampo_ As Object, descripcion_ As String) Implements IEntidadDatos.cmp
            Throw New NotImplementedException()
        End Sub

        Public Sub cmp(nombreCampo_ As Object, Optional descripcion_ As String = Nothing, Optional estiloCCS_ As IEntidadDatos.EstilossCCS = IEntidadDatos.EstilossCCS.SinDefinir, Optional nombreCampoFiltro_ As Object = Nothing, Optional esAgrupador_ As Boolean = False, Optional funcionAgregacion_ As IEntidadDatos.TiposFuncionesAgregacion = IEntidadDatos.TiposFuncionesAgregacion.SinDefinir, Optional tipoOrdenamiento_ As IEntidadDatos.TiposOrdenamiento = IEntidadDatos.TiposOrdenamiento.SinDefinir, Optional mostrarCampo_ As IEntidadDatos.MostrarCampo = IEntidadDatos.MostrarCampo.Si) Implements IEntidadDatos.cmp
            Throw New NotImplementedException()
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Throw New NotImplementedException()
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: eliminar el estado administrado (objetos administrados)
                End If

                ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                ' TODO: establecer los campos grandes como NULL
                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

    End Class

End Namespace

