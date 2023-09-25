
Imports Wma.Exceptions

Public Interface IItemExtraFacturaComercial


#Region "Propiedades"

    Property CodigoProducto As String

    Property Marca As String

    Property Modelo As String

    Property Serie As String

    Property Estado As String

#End Region

#Region "Metodos"

    Function AgregarOtrosDatos() As TagWatcher

    Function ActualizarOtrosDatos() As TagWatcher

    Function EliminarOtrosDatos() As TagWatcher

#End Region

End Interface
