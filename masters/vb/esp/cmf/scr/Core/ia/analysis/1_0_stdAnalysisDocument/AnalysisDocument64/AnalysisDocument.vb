Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class AnalysisDocument
    Implements IAnalysisDocument
    Public Property processdate As Date _
        Implements IAnalysisDocument.processdate
    Public Property environmentid As Integer _
        Implements IAnalysisDocument.environmentid
    Public Property confidence As Double _
        Implements IAnalysisDocument.confidence
    Public Property info As String _
        Implements IAnalysisDocument.info
    Public Property score As Double _
        Implements IAnalysisDocument.score
    Public Property analysis As Analysis _
        Implements IAnalysisDocument.analysis
End Class





