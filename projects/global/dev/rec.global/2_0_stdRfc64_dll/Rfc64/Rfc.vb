Imports MongoDB.Bson

Public Class Rfc

    Property idrfc As ObjectId

    Property sec As Integer

    Property rfc As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class