Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class Domicilio

    Property _iddomicilio As ObjectId

    <BsonIgnoreIfNull>
    Property iddivisionkb As Integer?

    Property sec As Integer

    Property calle As String

    Property numeroexterior As String

    <BsonIgnoreIfNull>
    Property numerointerior As String

    <BsonIgnoreIfNull>
    Property colonia As String

    <BsonIgnoreIfNull>
    Property codigopostal As String

    Property ciudad As String

    <BsonIgnoreIfNull>
    Property localidad As String

    <BsonIgnoreIfNull>
    Property municipio As String

    <BsonIgnoreIfNull>
    Property entidadfederativa As String

    Property pais As String

    <BsonIgnoreIfNull>
    Property paisPresentacion As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class