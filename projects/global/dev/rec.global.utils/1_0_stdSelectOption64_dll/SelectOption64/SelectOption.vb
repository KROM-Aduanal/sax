Public Class SelectOption

    Public Sub New()
        MyBase.New()
    End Sub

    'Default properties
    Public Property Value As String

    Public Property Text As String

    Public Property Signature As String = Nothing

    'Exclusive for ListBox
    Public Property Indice As Integer = 0

    Public Property Delete As Boolean = False

    'Exclusive for CollectionView
    Public Property Dropdowm As Boolean = False

End Class
