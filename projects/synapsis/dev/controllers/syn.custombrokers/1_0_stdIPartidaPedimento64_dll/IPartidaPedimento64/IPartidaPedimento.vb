
Imports Wma.Exceptions

Public Interface IPartidaPedimento

#Region "Propiedades"

    Property SecuenciaPartida As Integer

    Property FraccionArancelaria As String

    Property Nico As String

    Property Vinculacion As Object

    Property MetodoValoracion As Object

    Property UnidadMedidaComercial As Object

    Property PaisVentaCompra As Object

    Property PaisOrigenDestino As Object

    Property CantidadUMC As Object

    Property UnidadMedidaTarifa As Object

    Property CantidadUMT As Object

    Property Descipcion As String

    Property Marca As String

    Property Modelo As String

    Property CodigoProducto As String

    Property ValorAduanalDolares As Object

    Property ImportePrecioPagado As Object

    Property Observaciones As String

    Property ResumenImpuestos As List(Of ImpuestoPartidaPedimento)

    Property Permisos As List(Of PermisoPartidaPedimento)

    Property Identificadores As List(Of IdentificadorPartidaPedimento)

    Property Serie As Object

    Property Kilometraje As Object

    Property ValorAgregado As Object

    Property Estado As Integer

#End Region

#Region "Metodos"

    Function AgregarPartidaPedimento() As TagWatcher

    Function ActualizarPartidaPedimento() As TagWatcher

    Function EliminarPartidaPedimento() As TagWatcher

#End Region

End Interface
