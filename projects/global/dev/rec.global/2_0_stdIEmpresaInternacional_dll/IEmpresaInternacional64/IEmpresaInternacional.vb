Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Rec.Globals.Empresas

Public Interface IEmpresaInternacional : Inherits IEmpresa

    Property taxids As List(Of TaxId)

    <BsonIgnoreIfNull>
    Property _idbu As ObjectId?

    <BsonIgnoreIfNull>
    Property bu As String

    <BsonIgnoreIfNull>
    Property bus As List(Of Bus)

End Interface