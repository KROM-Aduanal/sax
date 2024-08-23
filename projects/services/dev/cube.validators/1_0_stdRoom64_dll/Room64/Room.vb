Imports System.ComponentModel
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

#Region "Enum"


#End Region

Public Class room
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
    Public Property addresses As List(Of roomcontext)
    <BsonIgnoreIfNull>
    Public Property messages As List(Of String)
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property type As String
    <BsonIgnoreIfNull>
    Public Property contenttype As String
    <BsonIgnoreIfNull>
    Public Property awaitingupdates As List(Of roomhistory)
    <BsonIgnoreIfNull>
    Public Property historical As List(Of roomhistory)

End Class

Public Class roomcontext

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

Public Class runnedsurround

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

Public Class roomhistory

    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property roomname As String
    <BsonIgnoreIfNull>
    Public Property description As String
    <BsonIgnoreIfNull>
    Public Property rules As String
    <BsonIgnoreIfNull>
    Public Property addresses As List(Of roomcontext)
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


Public Class roomresource

    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property roomname As String
    <BsonIgnoreIfNull>
    Public Property description As String
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property contenttype As String
    <BsonIgnoreIfNull>
    Public Property createat As DateTime
    <BsonIgnoreIfNull>
    Public Property rolid As Int32
    <BsonIgnoreIfNull>
    Public Property branchname As String
    <BsonIgnoreIfNull>
    Public Property iduser As ObjectId
    <BsonIgnoreIfNull>
    Public Property username As String
    <BsonIgnoreIfNull>
    Public Property idroom As ObjectId
    <BsonIgnoreIfNull>
    Public Property areatype As Int32
    <BsonIgnoreIfNull>
    Public Property companyid As Int32
    <BsonIgnoreIfNull>
    Public Property enviroment As Int32
    <BsonIgnoreIfNull>
    Public Property valorpresentacion As String

End Class


Public Class validfields

    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property section As String
    <BsonIgnoreIfNull>
    Public Property sectionexcel As String
    <BsonIgnoreIfNull>
    Public Property sectionfield As String
    <BsonIgnoreIfNull>
    Public Property valorpresentacion As String
    <BsonIgnoreIfNull>
    Public Property details As Nullable

    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property contentype As String
    <BsonIgnoreIfNull>
    Public Property archivado As Boolean
    <BsonIgnoreIfNull>
    Public Property estado As Int32


End Class






