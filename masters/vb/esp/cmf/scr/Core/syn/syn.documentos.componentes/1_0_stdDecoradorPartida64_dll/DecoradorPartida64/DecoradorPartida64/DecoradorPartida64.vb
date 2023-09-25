Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes


Namespace Syn.Documento.Componentes
    '<BsonKnownTypes(GetType(PartidaPedimento))>

    <BsonKnownTypes(GetType(PartidaGenerica))>
    <Serializable()>
    Public MustInherit Class DecoradorPartida
        Inherits Partida

#Region "Attributes"

        Protected _partida As Partida

#End Region

#Region "Builders"
        Public Sub New(ByVal partida_ As Partida)

            MyBase.New()

            Me._partida = partida_

        End Sub

        Public Sub New()

            MyBase.New()

        End Sub

#End Region

#Region "Methods"


#End Region

    End Class

End Namespace
