Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.CustomBrokers.Controllers
Imports System.IO
Imports MongoDB.Driver
Imports MongoDB.Bson.Serialization.Attributes

Public Class Referencia
    Implements ICloneable
    <BsonIgnoreIfNull>
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property _referencia As String
    <BsonIgnoreIfNull>
    Public Property _documentoAduanero As DocumentoAduanero
    <BsonIgnoreIfNull>
    Public Property _operacion As Operacion
    <BsonIgnoreIfNull>
    Public Property _cliente As Cliente
    <BsonIgnoreIfNull>
    Public Property _importacion As Importacion
    <BsonIgnoreIfNull>
    Public Property _exportacion As Exportacion
    <BsonIgnoreIfNull>
    Public Property _documentos As List(Of Documentos)
    <BsonIgnoreIfNull>
    Public Property _confiabilidad As String

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function
End Class

Public Class DocumentoAduanero
    <BsonIgnoreIfNull>
    Public Property _tipoOperacion As Int32
    <BsonIgnoreIfNull>
    Public Property _regimen As String
    <BsonIgnoreIfNull>
    Public Property _clavePedimento As String

End Class
Public Class Operacion
    <BsonIgnoreIfNull>
    Public Property _modalidadAduanaPatente As Int32
    <BsonIgnoreIfNull>
    Public Property _tipoPedimento As String
    <BsonIgnoreIfNull>
    Public Property _materialPeligroso As Boolean
    <BsonIgnoreIfNull>
    Public Property _ejecuvidoCuenta As Int32

End Class
Public Class Cliente

    <BsonIgnoreIfNull>
    Public Property _idCliente As ObjectId
    <BsonIgnoreIfNull>
    Public Property _razonSocial As String
    <BsonIgnoreIfNull>
    Public Property _rfcCliente As String
    <BsonIgnoreIfNull>
    Public Property _rfcFacturacion As String
    <BsonIgnoreIfNull>
    Public Property _bancoPago As String

End Class
Public Class Importacion

    <BsonIgnoreIfNull>
    Public Property _tracking As TrackingImportacion
    <BsonIgnoreIfNull>
    Public Property _fechas As FechasImportacion
    <BsonIgnoreIfNull>
    Public Property _guia As Guia

End Class
Public Class Exportacion

    <BsonIgnoreIfNull>
    Public Property _tracking As TrackingExportacion
    <BsonIgnoreIfNull>
    Public Property _fechas As FechasExportacion
End Class
Public Class Documentos

    <BsonIgnoreIfNull>
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property _archivo As String
    <BsonIgnoreIfNull>
    Public Property _tipo As Int32
End Class
Public Class TrackingImportacion

    <BsonIgnoreIfNull>
    Public Property _apertura As Date
    <BsonIgnoreIfNull>
    Public Property _entrada As Date
    <BsonIgnoreIfNull>
    Public Property _proforma As Date
    <BsonIgnoreIfNull>
    Public Property _pago As Date
    <BsonIgnoreIfNull>
    Public Property _despacho As Date

End Class
Public Class FechasImportacion

    <BsonIgnoreIfNull>
    Public Property _eta As Date
    <BsonIgnoreIfNull>
    Public Property _revalidacion As Date
    <BsonIgnoreIfNull>
    Public Property _previo As Date

End Class
Public Class Guia

    <BsonIgnoreIfNull>
    Public Property _recintoFiscal As String
    <BsonIgnoreIfNull>
    Public Property _listaGuias As List(Of GuiaDetalle)

End Class
Public Class GuiaDetalle

    <BsonIgnoreIfNull>
    Public Property _guia As String
    <BsonIgnoreIfNull>
    Public Property _transportista As String
    <BsonIgnoreIfNull>
    Public Property _tipoCarga As String
    <BsonIgnoreIfNull>
    Public Property _pais As String
    <BsonIgnoreIfNull>
    Public Property perBruto As Decimal
    <BsonIgnoreIfNull>
    Public Property _unidadMedida As String
    <BsonIgnoreIfNull>
    Public Property _tipoGuia As String
    <BsonIgnoreIfNull>
    Public Property _salidaOrigen As String
    <BsonIgnoreIfNull>
    Public Property _descripcionMercancia As String
    <BsonIgnoreIfNull>
    Public Property _consignatario As String
End Class
Public Class TrackingExportacion

    <BsonIgnoreIfNull>
    Public Property _apertura As Date
    <BsonIgnoreIfNull>
    Public Property _proforma As Date
    <BsonIgnoreIfNull>
    Public Property cierre As Date
    <BsonIgnoreIfNull>
    Public Property _pago As Date
    <BsonIgnoreIfNull>
    Public Property _despacho As Date

End Class
Public Class FechasExportacion

    <BsonIgnoreIfNull>
    Public Property _presentacion As Date
    <BsonIgnoreIfNull>
    Public Property _salida As Date
    <BsonIgnoreIfNull>
    Public Property _previo As Date
    <BsonIgnoreIfNull>
    Public Property _etd As Date
    <BsonIgnoreIfNull>
    Public Property _cierreFisico As Date

End Class
