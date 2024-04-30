Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class Room
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property roomname As String
    <BsonIgnoreIfNull>
    Public Property description As String
    <BsonIgnoreIfNull>
    Public Property rules As String
    <BsonIgnoreIfNull>
    Public Property required As Boolean
    <BsonIgnoreIfNull>
    Public Property fieldsrequired As List(Of String)
    <BsonIgnoreIfNull>
    Public Property addresses As List(Of Framing)
    <BsonIgnoreIfNull>
    Public Property messages As List(Of String)
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property type As String
    <BsonIgnoreIfNull>
    Public Property contenttype As String
    <BsonIgnoreIfNull>
    Public Property awaitingupdate As List(Of RoomHistory)
    <BsonIgnoreIfNull>
    Public Property historical As List(Of RoomHistory)

End Class

Public Class Framing

    <BsonIgnoreIfNull>
    Public Property _idcontext As ObjectId

    <BsonIgnoreIfNull>
    Public Property context As String

    <BsonIgnoreIfNull>
    Public Property firmacontext As String

    <BsonIgnoreIfNull>
    Public Property rules As String
    <BsonIgnoreIfNull>
    Public Property ref As List(Of String)
    <BsonIgnoreIfNull>
    Public Property loc As List(Of Int32)
    <BsonIgnoreIfNull>
    Public Property cached As Boolean
    <BsonIgnoreIfNull>
    Public Property result As String
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property timelife As DateTime



End Class

Public Class RunnedSurround

    <BsonIgnoreIfNull>
    Public Property _id As ObjectId

    <BsonIgnoreIfNull>
    Public Property firmacontext As String

    <BsonIgnoreIfNull>
    Public Property rules As String

    <BsonIgnoreIfNull>
    Public Property roomname As String

    Public Property ref As List(Of String)

    <BsonIgnoreIfNull>
    Public Property cached As Boolean

    <BsonIgnoreIfNull>
    Public Property result As String

    <BsonIgnoreIfNull>
    Public Property status As String

    <BsonIgnoreIfNull>
    Public Property timelife As DateTime

End Class

Public Class RoomHistory

    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property roomname As String
    <BsonIgnoreIfNull>
    Public Property description As String
    <BsonIgnoreIfNull>
    Public Property rules As String
    <BsonIgnoreIfNull>
    Public Property addresses As List(Of Framing)
    <BsonIgnoreIfNull>
    Public Property messages As List(Of String)
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property contenttype As String
    <BsonIgnoreIfNull>
    Public Property createat As DateTime
    <BsonIgnoreIfNull>
    Public Property reason As String
    <BsonIgnoreIfNull>
    Public Property _iduser As ObjectId
    Public Property username As String
    Public Property allowwableusername As String

End Class


Public Class RoomResource

    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property roomname As String
    <BsonIgnoreIfNull>
    Public Property description As String
    <BsonIgnoreIfNull>
    Public Property rules As String
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property contenttype As String
    <BsonIgnoreIfNull>
    Public Property createat As DateTime
    <BsonIgnoreIfNull>
    Public Property rolId_ As Int32
    <BsonIgnoreIfNull>
    Public Property cubeSource_ As String
    <BsonIgnoreIfNull>
    Public Property _iduser As ObjectId
    <BsonIgnoreIfNull>
    Public Property username As String
    <BsonIgnoreIfNull>
    Public Property _idroom As ObjectId
    <BsonIgnoreIfNull>
    Public Property valorpresentacion As String

End Class