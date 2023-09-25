Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes


Namespace Syn.Documento.Componentes

    <BsonKnownTypes(GetType(DecoradorSeccion))>
    <Serializable()>
    Public MustInherit Class Seccion
        Inherits Nodo

#Region "Attributes"


#End Region

#Region "Builders"

        Public Sub New()

            TipoNodo = TiposNodo.Seccion

        End Sub

#End Region

#Region "Methos"

        Public Sub ReorganizarPartidas()

            OperacionesNodo.AjustarConteoNodosPartidas(Me.Nodos)

        End Sub

#End Region

#Region "Properties"

        <BsonElement("IDUnico")>
        Public Property IDUnico As Integer

        <BsonElement("Nombre")>
        Public Property Nombre As String

        <BsonElement("Orden")>
        Public Property Orden As Integer

        <BsonElement("VisibleCaptura")>
        Public Property VisibleCaptura As Boolean

        <BsonElement("VisibleImpresion")>
        Public Property VisibleImpresion As Boolean

        <BsonIgnore>
        Public ReadOnly Property CantidadPartidas As Integer

            Get
                Return OperacionesNodo.ContarNodosPartidas(Me.Nodos)

            End Get

        End Property

    End Class

#End Region



End Namespace