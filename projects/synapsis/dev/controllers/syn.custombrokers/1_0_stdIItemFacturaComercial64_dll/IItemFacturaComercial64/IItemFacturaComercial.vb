
Imports Wma.Exceptions

Public Interface IItemFacturaComercial

#Region "Propiedades"

    Property SecuenciaItem As Integer

    Property FacturaItem As Object

    Property FraccionArancelaria As String

    Property Nico As String

    Property Vinculacion As Object

    Property MetodoValoracion As Object

    Property UnidadMedidaComercial As Object

    Property PaisVentaCompra As Object

    Property PaisOrigenDestino As Object

    Property PrecioUnitario As Object

    Property CantidadUMC As Object

    Property UnidadMedidaTarifa As Object

    Property CantidadUMT As Object

    Property Descripcion As String

    Property ValorMercancia As Object

    Property Estado As Integer

#End Region

#Region "Metodos"

    Function AgregarItemFactura() As TagWatcher

    Function ActualizarItemFactura() As TagWatcher

    Function EliminarItemFactura() As TagWatcher

#End Region

End Interface
