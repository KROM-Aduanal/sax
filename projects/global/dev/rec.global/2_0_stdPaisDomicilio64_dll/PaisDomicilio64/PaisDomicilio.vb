Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Domicilio64
Imports Wma.Exceptions

Public Class PaisDomicilio
    Property idpais As ObjectId

    Property sec As Integer

    <BsonIgnoreIfNull>
    Property domicilios As List(Of Domicilio) = Nothing

    Property pais As String
    Property paisPresentacion As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class