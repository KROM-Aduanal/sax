Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Rec.Globals.Contacto64
Imports Rec.Globals.PaisDomicilio64

Public Interface IEmpresa

    Property _id As ObjectId

    Property _idempresa As Int32

    <BsonIgnoreIfNull>
    Property _idempresakb As Int32?

    Property razonsocial As String

    Property razonsocialcorto As String

    <BsonIgnoreIfNull>
    Property abreviatura As String

    Property nombrecomercial As String

    <BsonIgnoreIfNull>
    Property paisesdomicilios As List(Of PaisDomicilio)

    <BsonIgnoreIfNull>
    Property girocomercial As String

    <BsonIgnoreIfNull>
    Property _idgrupocomercial As ObjectId?

    Property contactos As List(Of Contacto)

    Property abierto As Boolean

    Property estado As Int16

    <BsonIgnoreIfNull>
    Property estatus As Int16

    Property archivado As Boolean

    <BsonIgnore>
    Property esNuevoDomicilio As Boolean?

    <BsonIgnore>
    Property tipoEmpresa As Boolean?

End Interface