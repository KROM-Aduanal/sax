Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface ISecuencia

    <BsonId>
    Property _id As ObjectId

    <BsonIgnoreIfNull>
    Property secuenciaAnterior As Int32

    Property sec As Int32

    <BsonIgnoreIfNull>
    Property compania As Int32

    <BsonIgnoreIfNull>
    Property area As Int32

    Property environment As Int32

    <BsonIgnoreIfNull>
    Property anio As Int32

    <BsonIgnoreIfNull>
    Property mes As Int32

    Property nombre As String

    Property tiposecuencia As Int32

    <BsonIgnoreIfNull>
    Property subtiposecuencia As Int32

    <BsonIgnoreIfNull>
    Property prefijo As String

    <BsonIgnoreIfNull>
    Property sufijo As String

    Property estado As Int32

    Property archivado As Boolean

End Interface
