
Imports Wma.Exceptions

Public Interface IGeneradorPartidasPedimento

#Region "Enums"

    Enum TipoAgrupaciones

        SinDefinir = 0

    End Enum

#End Region

#Region "Propiedades"

    Property TipoAgrupacion As TipoAgrupaciones

    Property ItemsFacturaComercial As List(Of IItemFacturaComercial)

    Property PartidasPedimento As List(Of IPartidaPedimento)

    Property AgrupacionesDisponibles As List(Of TipoAgrupaciones)

    Property Estado As TagWatcher

#End Region

#Region "Metodos"

    Function AnalisisConsistencia(PartidasPedimento As List(Of IPartidaPedimento)) As TagWatcher

    Function GeneraOpcionesAgrupacion(ItemsFactura As List(Of IItemFacturaComercial)) As TagWatcher

    Function AgruparItemsFacturaPor(TipoAgrupacionSeleccionada As TipoAgrupaciones, ItemsFactura As List(Of IItemFacturaComercial)) As TagWatcher

#End Region

End Interface
