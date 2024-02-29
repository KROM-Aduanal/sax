Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Rec.Globals.Bus64
Imports Rec.Globals.IEmpresa64
Imports Rec.Globals.TaxId64

Public Interface IEmpresaInternacional

    Property taxids As List(Of TaxId)

    <BsonIgnoreIfNull>
    Property _idbu As ObjectId?

    <BsonIgnoreIfNull>
    Property bu As String

    <BsonIgnoreIfNull>
    Property bus As List(Of Bus)

End Interface