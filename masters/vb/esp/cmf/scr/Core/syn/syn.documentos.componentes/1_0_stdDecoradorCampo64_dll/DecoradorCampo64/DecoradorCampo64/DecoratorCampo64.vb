

Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    '    <BsonKnownTypes(GetType(CampoPedimento))>
    '<BsonKnownTypes(GetType(CampoPedimento))>

    <BsonKnownTypes(GetType(CampoGenerico))>
    <Serializable()>
    Public MustInherit Class DecoradorCampo
        Inherits Campo

#Region "Attributes"

        Protected _campo As Campo

#End Region

#Region "Builders"
        Public Sub New(ByVal campo_ As Campo)

            Me._campo = campo_

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub Display()

            _campo.Display()

        End Sub

#End Region

    End Class

End Namespace
