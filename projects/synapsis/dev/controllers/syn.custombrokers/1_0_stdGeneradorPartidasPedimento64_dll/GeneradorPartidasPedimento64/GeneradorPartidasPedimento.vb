
Imports Wma.Exceptions

Public Class GeneradorPartidasPedimento
    Implements IGeneradorPartidasPedimento

#Region "Propiedades"

    Public Property TipoAgrupacion As IGeneradorPartidasPedimento.TipoAgrupaciones Implements IGeneradorPartidasPedimento.TipoAgrupacion

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As IGeneradorPartidasPedimento.TipoAgrupaciones)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property ItemsFacturaComercial As List(Of IItemFacturaComercial) Implements IGeneradorPartidasPedimento.ItemsFacturaComercial

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As List(Of Controllers.IItemFacturaComercial))

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property PartidasPedimento As List(Of IPartidaPedimento) Implements IGeneradorPartidasPedimento.PartidasPedimento

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As List(Of Controllers.IPartidaPedimento))

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property AgrupacionesDisponibles As List(Of IGeneradorPartidasPedimento.TipoAgrupaciones) Implements IGeneradorPartidasPedimento.AgrupacionesDisponibles

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As List(Of IGeneradorPartidasPedimento.TipoAgrupaciones))

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Estado As TagWatcher Implements IGeneradorPartidasPedimento.Estado

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As TagWatcher)

            Throw New NotImplementedException()

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function AnalisisConsistencia(PartidasPedimento As List(Of IPartidaPedimento)) As TagWatcher Implements IGeneradorPartidasPedimento.AnalisisConsistencia

        Throw New NotImplementedException()

    End Function

    Public Function GeneraOpcionesAgrupacion(ItemsFactura As List(Of IItemFacturaComercial)) As TagWatcher Implements IGeneradorPartidasPedimento.GeneraOpcionesAgrupacion

        Throw New NotImplementedException()

    End Function

    Public Function AgruparItemsFacturaPor(TipoAgrupacionSeleccionada As IGeneradorPartidasPedimento.TipoAgrupaciones, ItemsFactura As List(Of Controllers.IItemFacturaComercial)) As TagWatcher Implements IGeneradorPartidasPedimento.AgruparItemsFacturaPor

        Throw New NotImplementedException()

    End Function

#End Region

End Class
