Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface ICommercialInvoice
    <BsonIgnoreIfNull>
    Property _id As ObjectId
    Property invoicenumber As String
    Property invoicedate As Date
    Property invoiceseries As String
    Property customername As String
    Property suppliername As String
    Property invoicecountry As String
    Property totalinvoice As Double
    Property invoicecurrency As String
    <BsonIgnoreIfNull>
    Property customer As Customer
    <BsonIgnoreIfNull>
    Property supplier As Supplier
    Property items As List(Of Item)
    <BsonIgnoreIfNull>
    Property additionaldetails As AdditionalDetails
    <BsonIgnoreIfNull>
    Property consigneedetails As ConsigneeDetails
End Interface