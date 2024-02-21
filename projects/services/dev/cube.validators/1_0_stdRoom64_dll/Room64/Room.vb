Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class Room
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property roomname As String
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
