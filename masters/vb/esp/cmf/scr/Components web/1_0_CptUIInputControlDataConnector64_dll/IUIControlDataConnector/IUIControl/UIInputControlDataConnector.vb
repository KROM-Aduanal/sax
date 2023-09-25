Public Class UIInputControlDataConnector
    Inherits UIInputControlDataConnection
    Implements IUIControlDataConnector

#Region "Propiedades"

    Public Property KeyField As String Implements IUIControlDataConnector.KeyField

    Public Property DisplayField As String Implements IUIControlDataConnector.DisplayField

#End Region

End Class
