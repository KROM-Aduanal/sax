Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Rec.Globals.Domicilio64

Public Class PaisDomicilio

    Property idpais As ObjectId

    Property sec As Integer

    <BsonIgnoreIfNull>
    Property domicilios As List(Of Domicilio)

    Property pais As String

    Property paisPresentacion As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class