Imports gsol.krom
Imports Syn.Documento

Namespace gsol.krom

    <Serializable()>
    Public Class EntidadDatosDocumentoDigital
        Inherits DocumentoDigital
        Implements IEntidadDatos, ICloneable, IDisposable

#Region "Atributos"

        Private _dimension As IEnlaceDatos.TiposDimension

        Private disposedValue As Boolean

        <NonSerialized>
        Private _newRow As DataRow

        <NonSerialized>
        Private _atributos As List(Of CampoVirtual)

        <NonSerialized>
        Private _clausulas As String

        <NonSerialized>
        Private _status As Global.Wma.Exceptions.TagWatcher

#End Region

#Region "Propiedades"

        Public Property NewRow As DataRow Implements IEntidadDatos.NewRow
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As DataRow)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Dimension As IEnlaceDatos.TiposDimension Implements IEntidadDatos.Dimension
            Get
                Return _dimension
            End Get
            Set(value As IEnlaceDatos.TiposDimension)
                _dimension = value
            End Set
        End Property

        Public ReadOnly Property CampoPorNombre(atributoDimension_ As Object) As Object Implements IEntidadDatos.CampoPorNombre
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Property Atributos As List(Of CampoVirtual) Implements IEntidadDatos.Atributos
            Get
                Return _atributos
            End Get
            Set(value As List(Of CampoVirtual))
                _atributos = value
            End Set
        End Property

        Public Property Clausulas As String Implements IEntidadDatos.Clausulas
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Status As Global.Wma.Exceptions.TagWatcher Implements IEntidadDatos.Status
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Global.Wma.Exceptions.TagWatcher)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Shadows Property Attribute(attributeName_ As Object) As Object Implements IEntidadDatos.Attribute
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Object)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property UpdateOnKeyValues As List(Of String) Implements IEntidadDatos.UpdateOnKeyValues
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As List(Of String))
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property DeteleOnKeyValues As List(Of String) Implements IEntidadDatos.DeteleOnKeyValues
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As List(Of String))
                Throw New NotImplementedException()
            End Set
        End Property

#End Region


        Public Sub Array(arrayOfDataEntity_ As List(Of IEntidadDatos)) Implements IEntidadDatos.Array
            Throw New NotImplementedException()
        End Sub

        Public Sub cmp(nombreCampo_ As Object) Implements IEntidadDatos.cmp
            Throw New NotImplementedException()
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

        'Public Function UpsertSubscriptions(toResource_ As String, fromResource_ As String, sgroup_ As subscriptionsgroup) As Task(Of Boolean) Implements IEntidadDatos.UpsertSubscriptions
        '    Throw New NotImplementedException()
        'End Function
    End Class
End Namespace
