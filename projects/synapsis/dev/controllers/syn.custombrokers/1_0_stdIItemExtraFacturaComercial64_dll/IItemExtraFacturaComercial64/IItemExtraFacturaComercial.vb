Imports Wma.Exceptions

Public Interface IItemExtraFacturaComercial
    Inherits ICloneable, IDisposable

#Region "Propiedades"

    Property CodigoProducto As String

    Property SecuenciaProducto As Integer

    Property Marca As String

    Property Modelo As String

    Property Submodelo As String

    Property Serie As String

    Property Estado As Integer

    Property Kilometraje As Integer

#End Region

End Interface