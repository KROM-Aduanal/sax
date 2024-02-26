Imports MongoDB.Driver
Imports Wma.Exceptions

Public Interface IGeneradorPartidasPedimento
    Inherits ICloneable, IDisposable

#Region "Enums"

    Enum TipoAgrupaciones

        SinDefinir = 0
        FraccionNico = 1
        UMCPrecioUnitarioCalculado = 2
        PaisVentaCompraOrigenDestino = 3
        ContribucionFormaPago = 4
        TasaTipoTasa = 5
        MetodoValoracionVinculacion = 6
        ValorAgregado = 7
        PrecioUnitario = 8
        SinAgrupacion = 9

    End Enum

#End Region

#Region "Propiedades"

    Property TipoAgrupacion As TipoAgrupaciones

    Property ItemsFacturaComercial As List(Of IItemFacturaComercial)

    Property PartidasPedimento As List(Of IPartidaPedimento)

    Property AgrupacionesDisponibles As List(Of TipoAgrupaciones)

    Property InformacionAgrupacion As InformacionAgrupacion

    Property Estado As TagWatcher

#End Region

#Region "Metodos"

    Function AnalisisConsistencia(ByVal partidasPedimento_ As List(Of IPartidaPedimento)) As TagWatcher

    Function GeneraOpcionesAgrupacion(ByVal itemsFactura_ As List(Of IItemFacturaComercial)) As List(Of TipoAgrupaciones)

    Function AgruparItemsFacturaPor(ByVal tipoAgrupacionSeleccionada_ As TipoAgrupaciones, ByVal itemsFactura_ As List(Of IItemFacturaComercial)) As TagWatcher

    Function GuardarInformacionAgrupaciones(ByVal session_ As IClientSessionHandle, ByVal informacionAgrupacion_ As InformacionAgrupacion, Optional ByVal entorno_ As Integer = Nothing) As TagWatcher

#End Region

End Interface