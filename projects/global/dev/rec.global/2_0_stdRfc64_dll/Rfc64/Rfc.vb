Imports MongoDB.Bson

Public Class Rfc

    Public Sub New()

    End Sub

    Public Sub New(ByVal rfc_ As String,
                   Optional ByVal sec_ As Int32 = 1)

        Me.idrfc = ObjectId.GenerateNewId
        Me.sec = sec_
        Me.rfc = rfc_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property idrfc As ObjectId

    Property sec As Integer

    Property rfc As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class