﻿Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Empresas


<Serializable()>
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