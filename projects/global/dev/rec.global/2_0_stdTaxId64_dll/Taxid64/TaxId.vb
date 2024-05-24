Imports MongoDB.Bson

Public Class TaxId
    Public Sub New()

    End Sub

    Public Sub New(ByVal taxid_ As String, Optional ByVal sec_ As Int32 = 1)

        Me.idtaxid = ObjectId.GenerateNewId
        Me.sec = sec_
        Me.taxid = taxid_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property idtaxid As ObjectId

    Property sec As Integer

    Property taxid As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class
