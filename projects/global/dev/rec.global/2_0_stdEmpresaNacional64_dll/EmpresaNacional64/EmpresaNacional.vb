Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Contacto64
Imports Rec.Globals.Curp64
Imports Rec.Globals.Empresa64
Imports Rec.Globals.IEmpresa64
Imports Rec.Globals.IEmpresaNacional64
Imports Rec.Globals.PaisDomicilio64
Imports Rec.Globals.RegimenFiscal64
Imports Rec.Globals.Rfc64

Public Class EmpresaNacional : Inherits Empresa
    Implements IEmpresa, IEmpresaNacional, IDisposable

    <BsonIgnore>
    Public Property rfcNuevo As Boolean? _
        Implements IEmpresaNacional.rfcNuevo

    Public Property _idrfc As ObjectId _
        Implements IEmpresaNacional._idrfc

    Public Property rfc As String _
        Implements IEmpresaNacional.rfc

    Public Property rfcs As List(Of Rfc) _
        Implements IEmpresaNacional.rfcs

    <BsonIgnore>
    Public Property curpNuevo As Boolean? _
        Implements IEmpresaNacional.curpNuevo

    <BsonIgnoreIfNull>
    Public Property _idcurp As ObjectId? _
        Implements IEmpresaNacional._idcurp

    <BsonIgnoreIfNull>
    Public Property curp As String _
        Implements IEmpresaNacional.curp

    <BsonIgnoreIfNull>
    Public Property curps As List(Of Curp) _
        Implements IEmpresaNacional.curps

    <BsonIgnoreIfNull>
    Public Property regimenesfiscales As List(Of RegimenFiscal) _
        Implements IEmpresaNacional.regimenesfiscales

    Public Property tipopersona As IEmpresaNacional.TiposPersona _
        Implements IEmpresaNacional.tipopersona

End Class