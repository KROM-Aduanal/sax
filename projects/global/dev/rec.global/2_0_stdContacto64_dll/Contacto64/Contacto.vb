Imports MongoDB.Bson

Public Class Contacto

    Property idcontacto As ObjectId

    Property sec As Integer

    Property nombrecompleto As String

    Property correoelectronico As String

    Property telefono As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class
