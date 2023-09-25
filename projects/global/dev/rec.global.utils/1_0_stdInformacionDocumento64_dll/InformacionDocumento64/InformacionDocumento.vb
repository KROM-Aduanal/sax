Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

<Serializable>
Public Class InformacionDocumento

#Region "Enums"

    Enum TiposDocumento
        SinDefinir = 0
        BL = 1
    End Enum

#End Region

#Region "Propiedades"

    Property foliodocumento As String

    Property tipodocumento As TiposDocumento

    Property datospropietario As InformacionPropietario

    'Property procesado As Boolean

#End Region

End Class

<Serializable>
Public Class InformacionPropietario

    <BsonRepresentation(BsonType.ObjectId)>
    Property _id As String

    Property nombrepropietario As String

End Class