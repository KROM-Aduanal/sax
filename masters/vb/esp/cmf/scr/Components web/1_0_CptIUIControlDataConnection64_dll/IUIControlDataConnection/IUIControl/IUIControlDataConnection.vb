Imports Gsol.krom

Public Interface IUIControlDataConnection

#Region "Enum"

#End Region

#Region "Propiedades"

    Property DataEntity As IEntidadDatos

    Property Dimension As IEnlaceDatos.TiposDimension

    Property Granularity As IEnlaceDatos.TiposDimension

    Property FreeClauses As String

#End Region

End Interface

