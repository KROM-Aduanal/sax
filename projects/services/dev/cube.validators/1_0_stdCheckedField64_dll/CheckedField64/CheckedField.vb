Imports Syn.Nucleo.RecursosComercioExterior

Public Class CheckedField

    Property value As String

    Property found As Boolean

    Property required As Boolean

    Property validate As Boolean

    Property roomname As String

    Property formulafieldname As String

    Property requiredfields As List(Of String)

    Property conditions As List(Of String)

    Property errormessages As List(Of String)

    Property dependencies As List(Of Boolean)

    Property childfields As List(Of CheckedField)

End Class

Public Class MultiKeyItem

    Public fieldpedimento As CamposPedimento

    Public indexplus As String

End Class
