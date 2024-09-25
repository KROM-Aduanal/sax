Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface IAnalysisDocument
    <BsonIgnoreIfNull>
    Property processdate As Date
    <BsonIgnoreIfNull>
    Property environmentid As Integer
    Property confidence As Double
    <BsonIgnoreIfNull>
    Property info As String
    <BsonIgnoreIfNull>
    Property score As Double
    <BsonIgnoreIfNull>
    Property analysis As Ia.Analysis.Analysis
End Interface


