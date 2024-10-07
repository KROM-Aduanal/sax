Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class AdditionalDetails
    <BsonIgnoreIfNull>
    Property purchaseorder As String
    Property totalweight As Double
    <BsonIgnoreIfNull>
    Property packages As String
    Property incoterm As String
    <BsonIgnoreIfNull>
    Property customerreference As String
    <BsonIgnoreIfNull>
    Property incrementalvalues As List(Of IncrementalValue)
End Class

Public Class IncrementalValue
    <BsonIgnoreIfNull>
    Property id As Integer
    Property incremental As String
    Property currency As String
    <BsonIgnoreIfNull>
    Property info As String
End Class

