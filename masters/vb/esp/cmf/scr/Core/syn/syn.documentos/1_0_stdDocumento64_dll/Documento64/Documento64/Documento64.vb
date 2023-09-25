
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento
    <Serializable()>
    Public MustInherit Class Documento

#Region "Attributes"

        Protected Friend _estructuraDocumento As EstructuraDocumento

#End Region

#Region "Properties"

        <BsonId>
        <BsonRepresentation(BsonType.ObjectId)>
        Public Property Id As String

        <BsonElement("IdDocumento")>
        Public Property IdDocumento As Integer

        <BsonElement("FolioDocumento")>
        Public Property FolioDocumento As String

        <BsonDateTimeOptions(Kind:=DateTimeKind.Local)>
        Public Property FechaCreacion As Date

        <BsonElement("UsuarioGenerador")>
        Public Property UsuarioGenerador As String

        <BsonElement("EstatusDocumento")>
        Public Property EstatusDocumento As String

        <BsonElement("Documento")>
        Public Property EstructuraDocumento As EstructuraDocumento

            Get

                Return _estructuraDocumento

            End Get

            Set(value As EstructuraDocumento)

                _estructuraDocumento = value

            End Set

        End Property
#End Region

#Region "Methods"
        Public MustOverride Sub ConstruyeEncabezado()
        Public MustOverride Sub ConstruyeCuerpo()
        Public MustOverride Sub ConstruyeEncabezadoPaginasSecundarias()
        Public MustOverride Sub ConstruyePiePagina()
        Public MustOverride Sub GeneraDocumento()

#End Region

    End Class

End Namespace