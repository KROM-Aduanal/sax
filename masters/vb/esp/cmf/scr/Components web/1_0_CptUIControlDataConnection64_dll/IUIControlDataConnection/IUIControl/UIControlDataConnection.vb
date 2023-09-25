Imports Gsol.krom

Public Class UIControlDataConnection
    Inherits UIControl
    Implements IUIControlDataConnection

#Region "Propiedades"

    Public Property DataEntity As IEntidadDatos Implements IUIControlDataConnection.DataEntity

    Public Property Dimension As IEnlaceDatos.TiposDimension Implements IUIControlDataConnection.Dimension

    Public Property Granularity As IEnlaceDatos.TiposDimension Implements IUIControlDataConnection.Granularity

    Public Property FreeClauses As String Implements IUIControlDataConnection.FreeClauses

#End Region


End Class

