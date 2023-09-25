Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class CampoReal
        Inherits Campo

#Region "Attributes"


#End Region

#Region "Builders"
        Public Sub New(ByVal idUnico_ As Integer)

            Me.IDUnico = idUnico_

        End Sub

        Public Sub New()

            Me.IDUnico = 0

        End Sub

#End Region

#Region "Properties"

        <BsonElement("Valor")>
        <BsonRepresentation(BsonType.Decimal128)>
        Public Property Valor As Decimal128

#End Region

#Region "Methods"
        Public Overrides Sub Display()

        End Sub

#End Region

    End Class

End Namespace
