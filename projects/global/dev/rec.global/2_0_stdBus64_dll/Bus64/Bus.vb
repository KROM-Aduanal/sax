Imports MongoDB.Bson
Public Class Bus

    Public Sub New()

    End Sub

    Public Sub New(ByVal unidadNegocio_ As String, Optional ByVal sec_ As Int32 = 1)

        Me.idunidadnegocio = ObjectId.GenerateNewId
        Me.sec = sec_
        Me.unidadnegocio = unidadNegocio_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property idunidadnegocio As ObjectId

    Property sec As Integer

    Property unidadnegocio As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class