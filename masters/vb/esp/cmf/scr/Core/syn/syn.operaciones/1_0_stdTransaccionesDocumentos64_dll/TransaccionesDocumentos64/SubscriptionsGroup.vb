
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

<Serializable()>
Public Class subscriptionsgroup

    Property _id As ObjectId

    <BsonElement("fromcollectionname")>
    Public Property fromcollectionname As String

    <BsonElement("toresource")>
    Public Property toresource As String

    <BsonElement("active")>
    Public Property active As Boolean

    <BsonIgnoreIfNull>
    <BsonElement("defaultattribute")>
    Public Property defaultattribute As String

    <BsonElement("subscriptions")>
    Public Property subscriptions As subscriptions

    <BsonElement("_foreignkey")>
    Public Property _foreignkey As Object 'ObjectId

    <BsonElement("_foreignkeyname")>
    Public Property _foreignkeyname As String

End Class

<Serializable()>
Public Class subscriptions

    <BsonElement("namespaces")>
    Public Property namespaces As List(Of [namespace])

    <BsonElement("fields")>
    Public Property fields As List(Of fieldInfo)

    <BsonElement("followers")>
    <BsonIgnoreIfNull>
    Public Property followers As List(Of follower)

End Class


<Serializable()>
Public Class relatedfield

    <BsonElement("foreignkey")>
    Public Property foreignkey As Object 'ObjectId

    <BsonElement("foreignkeyname")>
    Public Property foreignkeyname As String

    <BsonElement("field")>
    Public Property uniquefield As fieldInfo

    <BsonElement("toresource")>
    Public Property toresource As String

    <BsonElement("fields")>
    Public Property fields As List(Of fieldInfo)

End Class

<Serializable()>
Public Class [namespace]

    Public Property _id As Int32

    Public Property path As String

End Class

<Serializable()>
Public Class follower

    Public Property _myid As Object 'ObjectId 'String

    <BsonIgnoreIfNull>
    Public Property sec As Int32

    '<BsonIgnore>
    Public Property reg As Date
    <BsonIgnoreIfNull>
    Public Property _idfriend As Object 'ObjectId

End Class

<Serializable()>
Public Class fieldInfo

    <BsonIgnore>
    Public Property _enum As [Enum]
    <BsonIgnore>
    Public Property _fenum As [Enum]

    Public Property _id As Int32?

    Public Property name As String

    <BsonIgnoreIfNull>
    Public Property nsp As Int32?

    <BsonIgnoreIfNull>
    Public Property attr As String

    <BsonIgnoreIfNull>
    Public Property _fid As Int32?

    <BsonIgnoreIfNull>
    Public Property fname As String

    <BsonIgnoreIfNull>
    Public Property fnsp As Int32?

    <BsonIgnoreIfNull>
    Public Property fattr As String

    Public Property reg As Date = Now

    '---optional--------
    <BsonIgnoreIfNull>
    Public Property type As String

    <BsonIgnoreIfNull>
    Public Property length As Int32?

    '----- news -----
    <BsonIgnoreIfNull>
    Public Property sta As Int32?

    <BsonIgnoreIfNull>
    Public Property arrayfilters As String

End Class