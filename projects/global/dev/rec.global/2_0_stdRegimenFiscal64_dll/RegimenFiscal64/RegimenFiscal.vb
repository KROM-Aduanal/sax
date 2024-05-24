Imports MongoDB.Bson

Public Class RegimenFiscal

    Public Sub New()

    End Sub

    Public Sub New(ByVal regimenfiscal_ As String,
                   Optional ByVal sec_ As Int32 = 1)

        idregimenfiscal = ObjectId.GenerateNewId
        _sec = sec_
        regimenfiscal = regimenfiscal_
        estado = 1
        archivado = False

    End Sub

    Property idregimenfiscal As ObjectId

    Property _sec As Integer

    Property regimenfiscal As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class