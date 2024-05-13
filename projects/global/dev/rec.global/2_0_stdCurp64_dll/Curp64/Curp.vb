Imports MongoDB.Bson

Public Class Curp

    Public Sub New()

    End Sub

    Public Sub New(ByVal curp_ As String,
                   Optional ByVal sec_ As Int32 = 1)

        Me.idcurp = ObjectId.GenerateNewId
        Me.sec = sec_
        Me.curp = curp_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property idcurp As ObjectId

    Property sec As Integer

    Property curp As String

    Property estado As Integer = 1

    Property archivado As Boolean = False

End Class