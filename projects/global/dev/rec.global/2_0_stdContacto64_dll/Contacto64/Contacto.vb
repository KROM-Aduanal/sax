Imports MongoDB.Bson

Public Class Contacto

    Public Sub New()

    End Sub

    Public Sub New(ByVal nombrecompleto_ As String,
                   ByVal correoelectronico_ As String,
                   ByVal telefono_ As String,
                   Optional ByVal sec_ As Int32 = 1)

        Me.idcontacto = ObjectId.GenerateNewId
        Me.sec = sec_
        Me.nombrecompleto = nombrecompleto_
        Me.correoelectronico = correoelectronico_
        Me.telefono = telefono_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property idcontacto As ObjectId

    Property sec As Integer

    Property nombrecompleto As String

    Property correoelectronico As String

    Property telefono As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class
