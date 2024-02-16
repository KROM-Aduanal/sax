Imports MongoDB.Bson

Public Class CubeReport

    Property _id As ObjectId
    Property informe As Dictionary(Of String, Dictionary(Of String, String))
    Property firmaframing As String
    Property rules As String
    Property ref As String
    Property loc As String
    Property cached As String
    Property result As String
    Property status As String
    Property timelife As DateTime

End Class
