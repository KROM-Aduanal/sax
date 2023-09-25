
Imports Wma.Exceptions

Public Class PartidaPedimento
    Implements IPartidaPedimento

#Region "Propiedades"

    Public Property SecuenciaPartida As Integer Implements IPartidaPedimento.SecuenciaPartida

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Integer)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property FraccionArancelaria As String Implements IPartidaPedimento.FraccionArancelaria
        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Nico As String Implements IPartidaPedimento.Nico

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Vinculacion As Object Implements IPartidaPedimento.Vinculacion

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property MetodoValoracion As Object Implements IPartidaPedimento.MetodoValoracion

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property UnidadMedidaComercial As Object Implements IPartidaPedimento.UnidadMedidaComercial

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property PaisVentaCompra As Object Implements IPartidaPedimento.PaisVentaCompra

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property PaisOrigenDestino As Object Implements IPartidaPedimento.PaisOrigenDestino

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property CantidadUMC As Object Implements IPartidaPedimento.CantidadUMC

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property UnidadMedidaTarifa As Object Implements IPartidaPedimento.UnidadMedidaTarifa

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property CantidadUMT As Object Implements IPartidaPedimento.CantidadUMT

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Descipcion As String Implements IPartidaPedimento.Descipcion

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Marca As String Implements IPartidaPedimento.Marca

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Modelo As String Implements IPartidaPedimento.Modelo

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property CodigoProducto As String Implements IPartidaPedimento.CodigoProducto

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property ValorAduanalDolares As Object Implements IPartidaPedimento.ValorAduanalDolares

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property ImportePrecioPagado As Object Implements IPartidaPedimento.ImportePrecioPagado

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Observaciones As String Implements IPartidaPedimento.Observaciones

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property ResumenImpuestos As List(Of ImpuestoPartidaPedimento) Implements IPartidaPedimento.ResumenImpuestos

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As List(Of ImpuestoPartidaPedimento))

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Permisos As List(Of PermisoPartidaPedimento) Implements IPartidaPedimento.Permisos

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As List(Of PermisoPartidaPedimento))

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Identificadores As List(Of IdentificadorPartidaPedimento) Implements IPartidaPedimento.Identificadores

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As List(Of IdentificadorPartidaPedimento))

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Serie As Object Implements IPartidaPedimento.Serie

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Kilometraje As Object Implements IPartidaPedimento.Kilometraje

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property ValorAgregado As Object Implements IPartidaPedimento.ValorAgregado

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Object)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Estado As Integer Implements IPartidaPedimento.Estado

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Integer)

            Throw New NotImplementedException()
        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function AgregarPartidaPedimento() As TagWatcher Implements IPartidaPedimento.AgregarPartidaPedimento

        Throw New NotImplementedException()

    End Function

    Public Function ActualizarPartidaPedimento() As TagWatcher Implements IPartidaPedimento.ActualizarPartidaPedimento

        Throw New NotImplementedException()

    End Function

    Public Function EliminarPartidaPedimento() As TagWatcher Implements IPartidaPedimento.EliminarPartidaPedimento

        Throw New NotImplementedException()

    End Function

#End Region

End Class
