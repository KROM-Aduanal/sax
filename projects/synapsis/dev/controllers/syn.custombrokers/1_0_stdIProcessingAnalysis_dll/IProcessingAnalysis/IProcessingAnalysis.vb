Public Interface IProcessingAnalysis
    Property processdate As Date
    Property confidence As Double
    Property environmentid As Integer
    Property analysis As List(Of Analysis)
End Interface