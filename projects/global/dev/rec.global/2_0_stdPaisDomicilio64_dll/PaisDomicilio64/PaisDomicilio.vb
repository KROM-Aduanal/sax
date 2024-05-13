Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Empresas
Imports Wma.Exceptions

Public Class PaisDomicilio

    Public Sub New()

    End Sub

    Public Sub New(ByVal idpais_ As ObjectId,
                   ByVal pais_ As String,
                   ByVal paisPresentacion_ As String,
                   Optional ByVal sec_ As Integer = 1)

        Me.idpais = idpais_
        Me.sec = sec_
        Me.domicilios = New List(Of Domicilio)
        Me.pais = pais_
        Me.paisPresentacion = paisPresentacion_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Public Sub New(ByVal idpais_ As ObjectId,
                   ByVal domicilios_ As Domicilio,
                   ByVal pais_ As String,
                   ByVal paisPresentacion_ As String,
                   Optional ByVal sec_ As Integer = 1)

        Me.idpais = idpais_
        Me.sec = sec_
        Me.domicilios = New List(Of Domicilio) From {domicilios_}
        Me.pais = pais_
        Me.paisPresentacion = paisPresentacion_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property idpais As ObjectId

    Property sec As Integer

    <BsonIgnoreIfNull>
    Property domicilios As List(Of Domicilio) = Nothing

    Property pais As String
    Property paisPresentacion As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class