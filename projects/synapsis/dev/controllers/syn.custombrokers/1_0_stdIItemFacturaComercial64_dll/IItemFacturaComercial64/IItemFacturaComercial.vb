Imports MongoDB.Bson

Public Interface IItemFacturaComercial
    Inherits ICloneable, IDisposable

#Region "Propiedades"

    Property SecuenciaItem As Integer

    Property _idItemFactura As ObjectId

    Property _idFacturaComercial As ObjectId

    Property FolioFacturaComercial As String

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

    Property PaisVendedor As String

    Property DescripcionPaisVendedor As String

    Property PaisComprador As String

    Property DescripcionPaisComprador As String

    Property PaisOrigen As String

    Property DescripcionPaisOrigen As String

    Property PaisDestino As String

    Property DescripcionPaisDestino As String

    Property PrecioUnitario As Double

    Property CantidadUMC As Double

    Property UnidadMedidaTarifa As Integer

    Property DescripcionUnidadMedidaTarifa As String

    Property CantidadUMT As Double

    Property Descripcion As String

    Property ValorMercancia As Double

    Property ValorAgregado As Integer

    Property Estado As Integer

#End Region

End Interface
