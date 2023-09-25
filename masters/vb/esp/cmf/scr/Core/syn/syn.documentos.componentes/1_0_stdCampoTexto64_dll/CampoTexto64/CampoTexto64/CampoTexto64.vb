Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class CampoTexto
        Inherits Campo

#Region "Attributes"


#End Region

#Region "Builders"
        Public Sub New()

            Me.IDUnico = -1

            Me.TipoDato = TiposDato.Texto

        End Sub

        Public Sub New(ByVal idUnico_ As Integer, ByVal incluirCaracteresEspeciales_ As Boolean)

            Me.IDUnico = idUnico_

            Me.TipoDato = TiposDato.Texto

        End Sub

#End Region

#Region "Properties"

        <BsonElement("Valor")>
        <BsonRepresentation(BsonType.String)>
        Public Property Valor As String

#End Region

#Region "Methods"
        Public Overrides Sub Display()

        End Sub

#End Region


    End Class

End Namespace
