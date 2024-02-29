Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Bus64
Imports Rec.Globals.Contacto64
Imports Rec.Globals.Empresa64
Imports Rec.Globals.IEmpresa64
Imports Rec.Globals.IEmpresaInternacional64
Imports Rec.Globals.PaisDomicilio64
Imports Rec.Globals.TaxId64

Public Class EmpresaInternacional : Inherits Empresa
    Implements IEmpresa, IEmpresaInternacional, IDisposable

    <BsonIgnoreIfNull>
    Public Property taxids As List(Of TaxId) _
        Implements IEmpresaInternacional.taxids

    <BsonIgnoreIfNull>
    Public Property _idbu As ObjectId? _
        Implements IEmpresaInternacional._idbu

    <BsonIgnoreIfNull>
    Public Property bu As String _
        Implements IEmpresaInternacional.bu

    <BsonIgnoreIfNull>
    Public Property bus As List(Of Bus) _
        Implements IEmpresaInternacional.bus

End Class