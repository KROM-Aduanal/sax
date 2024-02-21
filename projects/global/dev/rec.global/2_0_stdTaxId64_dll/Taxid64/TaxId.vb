Imports MongoDB.Bson

Public Class TaxId

    Property idtaxid As ObjectId

    Property sec As Integer

    Property taxid As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class
