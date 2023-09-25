Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class CampoFecha
        Inherits Campo

#Region "Attributes"


#End Region

#Region "Builders"
        Public Sub New()

            Me.IDUnico = -1

        End Sub

#End Region

#Region "Properties"

        <BsonElement("Valor")>
        <BsonRepresentation(BsonType.DateTime)>
        Public Property Valor As DateTime


#End Region

#Region "Methods"
        Public Overrides Sub Display()

        End Sub

#End Region

    End Class

End Namespace