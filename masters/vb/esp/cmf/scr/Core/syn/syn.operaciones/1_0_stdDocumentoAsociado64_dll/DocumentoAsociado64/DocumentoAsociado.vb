
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento.Componentes

<Serializable()>
Public Class DocumentoAsociado

    <BsonIgnore>
    Property idcampo As Integer = 0
    <BsonIgnore>
    Property idsection As Integer = 0

    <BsonElement("_iddocumentoasociado")>
    Property _iddocumentoasociado As Object

    <BsonElement("identificadorrecurso")>
    Property identificadorrecurso As String

    <BsonElement("idcoleccion")>
    Property idcoleccion As String

    <BsonElement("firmaelectronica")>
    Property firmaelectronica As String

    <BsonIgnoreIfNull>
    <BsonElement("metadatos")>
    Property metadatos As List(Of CampoGenerico)

    <BsonElement("analisisconsistencia")>
    Property analisisconsistencia As Integer

End Class
