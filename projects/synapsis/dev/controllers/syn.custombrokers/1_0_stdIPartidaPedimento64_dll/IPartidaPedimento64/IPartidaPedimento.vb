Imports MongoDB.Bson
Imports Wma.Exceptions

Public Interface IPartidaPedimento
    Inherits ICloneable, IDisposable

#Region "Propiedades"

    Property _idPartidaPedimento As ObjectId

    Property SecuenciaPartida As Integer

    Property FraccionArancelaria As String

    Property DescripcionFraccionArancelaria As String

    Property Nico As String

    Property DescripcionNico As String

    Property Vinculacion As Integer

    Property DescripcionVinculacion As String

    Property MetodoValoracion As Integer

    Property DescripcionMetodoValoracion As String

    Property UnidadMedidaComercial As Integer

    Property DescripcionUnidadMedidaComercial As String

    Property PrecioUnitario As Double

    Property PaisVendedor As String

    Property DescripcionPaisVendedor As String

    Property PaisComprador As String

    Property DescripcionPaisComprador As String

    Property PaisOrigen As String

    Property DescripcionPaisOrigen As String

    Property PaisDestino As String

    Property DescripcionPaisDestino As String

    Property CantidadUMC As Double

    Property UnidadMedidaTarifa As Integer

    Property DescripcionUnidadMedidaTarifa As String

    Property CantidadUMT As Double

    Property Descripcion As String

    Property Marca As String

    Property Modelo As String

    Property CodigoProducto As String

    Property ValorAduanal As Double

    Property ValorDolares As Double

    Property ImportePrecioPagado As Double

    Property ValorComercial As Double

    Property Observaciones As String

    Property ResumenImpuestos As List(Of ImpuestoPartidaPedimento)

    Property Permisos As List(Of PermisoPartidaPedimento)

    Property Identificadores As List(Of IdentificadorPartidaPedimento)

    Property Serie As String

    Property Kilometraje As Integer

    Property ValorAgregado As Double

    Property PrecioUnitarioUSD As Double

    Property Archivo As Boolean

    Property Estado As Integer

#End Region

End Interface