
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    <BsonKnownTypes(GetType(DecoradorPartida))>
    <Serializable()>
    Public MustInherit Class Partida
        Inherits Nodo

#Region "Attributes"

#End Region

#Region "Builders"
        Public Sub New()

            TipoNodo = Nodo.TiposNodo.Partida

        End Sub

#End Region



#Region "Properties"

        <BsonElement("NumeroSecuencia")>
        Public Property NumeroSecuencia As Integer

#End Region


    End Class

End Namespace
