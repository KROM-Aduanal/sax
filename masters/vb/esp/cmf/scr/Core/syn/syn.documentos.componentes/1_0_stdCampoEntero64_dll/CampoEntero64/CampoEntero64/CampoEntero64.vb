Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class CampoEntero
        Inherits Campo

#Region "Attributes"

#End Region

#Region "Builders"
        Public Sub New()

            Me.IDUnico = 0

        End Sub
        Public Sub New(ByVal idUnico_ As Integer)

            Me.IDUnico = idUnico_

        End Sub

#End Region

#Region "Properties"


        <BsonElement("Valor")>
        <BsonRepresentation(BsonType.Int32)>
        Public Property Valor As Int32


#End Region

#Region "Methods"
        Public Overrides Sub Display()

        End Sub

#End Region

    End Class

End Namespace