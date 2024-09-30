Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Public Interface IItem
    <BsonIgnoreIfNull>
    Property sec As Integer
    <BsonIgnoreIfNull>
    Property productid As String
    <BsonIgnoreIfNull>
    Property sku As String
    Property partnumber As String
    Property quantity As Integer
    Property unit As String
    Property description As String
    Property total As Double
    Property currency As String
    Property usdvalue As Double
    Property value As Double
    <BsonIgnoreIfNull>
    Property discount As Decimal
    Property unitprice As Double
    <BsonIgnoreIfNull>
    Property netweight As Double
    <BsonIgnoreIfNull>
    Property purchaseorder As String
    <BsonIgnoreIfNull>
    Property destinationcountry As String
    Property origincountry As String
End Interface
