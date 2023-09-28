Imports MongoDB.Bson

Public Class Validador

    Property _idvalidador As ObjectId
    Property _idclaveregimen As ObjectId
    Property secuenciavalidador As Int16
    Property tipooperacion As Int16
    Property clavepedimento As String
    Property regimen As String
    Property fechaentrada As Boolean
    Property fechadespacho As Boolean
    Property fechavigencia As DateTime
    Property contribucionestipotasa As List(Of ContribucionTipoTasa)
    Property terminosfacturacion As List(Of TerminoFacturacion)
    Property contribucionestasa As List(Of ContribucionTasa)
    Property archivado As Boolean
    Property estado As Int16

End Class

Public Class ContribucionTipoTasa

    Property _idcontribuciontipotasa As ObjectId
    Property contribucion As String
    Property cvecontribucion As Int16
    Property tipotasa As Int16
    Property cvetipotasa As Int16
    Property notasimportacion As String
    Property notasexportacion As String
    Property notasexepciones As String
    Property fechavigencia As DateTime
    Property fechaentrada As Boolean
    Property fechadespacho As Boolean
    Property archivado As Boolean
    Property estatus As Int16

End Class

Public Class TerminoFacturacion

    Property _idterminofacturacion As ObjectId
    Property terminofacturacion As String
    Property seguroincrementable As Boolean
    Property fleteincrementable As Boolean
    Property embalajeincrementable As Boolean
    Property otroincrementable As Boolean
    Property transporteincrementable As Boolean
    Property segurodecrementable As Boolean
    Property cargadecrementable As Boolean
    Property descargadecrementable As Boolean
    Property otrodecrementable As Boolean
    Property fechavigencia As DateTime
    Property fechaentrada As Boolean
    Property fechadespacho As Boolean
    Property archivado As Boolean

End Class

Public Class ContribucionTasa

    Property _idcontribuciontasa As ObjectId
    Property contribucion As String
    Property tipotasa As Int16
    Property tasa As Double
    Property vigencia As DateTime
    Property archivado As Boolean
    Property estado As Int16

End Class