Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    '<BsonKnownTypes(GetType(NodoPedimento))>

    <BsonKnownTypes(GetType(NodoGenerico))>
    <Serializable()>
    Public MustInherit Class DecoradorNodo
        Inherits Nodo

#Region "Attributes"

        Protected _nodo As Nodo

#End Region

#Region "Builders"
        Public Sub New(ByVal nodo_ As Nodo)

            MyBase.New()

            Me._nodo = nodo_

        End Sub

        Public Sub New()

            MyBase.New()

        End Sub

#End Region

#Region "Methods"


#End Region

    End Class

End Namespace
