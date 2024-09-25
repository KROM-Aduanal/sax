Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Public Class Analysis
    <BsonIgnoreIfNull>
    Property processdate As Date
    <BsonIgnoreIfNull>
    Property environmentid As Integer
    Property confidence As Double
    ''' DATOS CHATGPT
    <BsonIgnoreIfNull>
    Property gptanalysis As Boolean
    <BsonIgnoreIfNull>
    Property gpttokensupload As Integer
    <BsonIgnoreIfNull>
    Property gpttokensdownload As Integer
    ''' DATOS TEXTRACT
    <BsonIgnoreIfNull>
    Property textractanalysis As Boolean
    <BsonIgnoreIfNull>
    Property textractpages As Integer
    Property quantitydifferences As Integer
    Property temperature As Integer
    <BsonIgnoreIfNull>
    Property datasource As Datasource
    <BsonIgnoreIfNull>
    Property mesaages As List(Of Messages)
    <BsonIgnoreIfNull>
    Property otherfields As List(Of Otherfields)
End Class


Public Class Datasource
    Property user As String
    Property integrationdate As Date
    <BsonIgnoreIfNull>
    Property integrationdetails As IntegrationDetails
End Class

Public Class Messages
    <BsonIgnoreIfNull>
    Property id As Integer
    Property type As String ''PUEDE SER UN ENUM <WARNING, ALERT>
    Property action As String ''PUEDE SER UN ENUM <info, review, info>
    <BsonIgnoreIfNull>
    Property _object As Object
    Property field As String
    Property value As String
    Property message As String
    Property confidence As Double
    Property source As String
End Class

Public Class Otherfields
    <BsonIgnoreIfNull>
    Property id As Integer
    Property field As String
    Property value As String
    Property message As String
    Property confidence As Double
    Property source As String
End Class

Public Class IntegrationDetails
    <BsonIgnoreIfNull>
    Property idfile As ObjectId
    Property documenttype As String
    Property filename As String
End Class
