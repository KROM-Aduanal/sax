Imports MongoDB.Bson
Imports Wma.Exceptions

Public Class PartidaPedimento
    Inherits OperacionesPartidas
    Implements IPartidaPedimento

#Region "Atributos"

    Private _idPartidaPedimento As ObjectId

    Private _secuenciaPartida As Integer

    Private _fraccionArancelaria As String

    Private _descripcionFraccionArancelaria As String

    Private _nico As String

    Private _descripcionNico As String

    Private _vinculacion As Integer

    Private _descripcionVinculacion As String

    Private _metodoValoracion As Integer

    Private _descripcionMetodoValoracion As String

    Private _unidadMedidaComercial As Integer

    Private _descripcionUnidadMedidaComercial As String

    Private _precioUnitario As Double

    Private _paisVendedor As String

    Private _descripcionPaisVendedor As String

    Private _paisComprador As String

    Private _descripcionPaisComprador As String

    Private _paisOrigen As String

    Private _descripcionPaisOrigen As String

    Private _paisDestino As String

    Private _descripcionPaisDestino As String

    Private _cantidadUMC As Double

    Private _unidadMedidaTarifa As Integer

    Private _descripcionUnidadMedidaTarifa As String

    Private _cantidadUMT As Double

    Private _descripcion As String

    Private _marca As String

    Private _modelo As String

    Private _codigoProducto As String

    Private _valorAduanal As Double

    Private _valorDolares As Double

    Private _importePrecioPagado As Double

    Private _valorComercial As Double

    Private _observaciones As String

    Private _resumenImpuestos As List(Of ImpuestoPartidaPedimento)

    Private _permisos As List(Of PermisoPartidaPedimento)

    Private _identificadores As List(Of IdentificadorPartidaPedimento)

    Private _serie As String

    Private _kilometraje As Integer

    Private _valorAgregado As Double

    Private _precioUnitarioUSD As Double

    Private _archivo As Boolean

    Private _estado As Integer

#End Region

#Region "Propiedades"

    Public Property idPartidaPedimento As ObjectId Implements IPartidaPedimento._idPartidaPedimento

        Get

            Return _idPartidaPedimento

        End Get

        Set(value As ObjectId)

            _idPartidaPedimento = value

        End Set

    End Property

    Public Property SecuenciaPartida As Integer Implements IPartidaPedimento.SecuenciaPartida

        Get

            Return _secuenciaPartida

        End Get

        Set(value As Integer)

            _secuenciaPartida = value

        End Set

    End Property

    Public Property FraccionArancelaria As String Implements IPartidaPedimento.FraccionArancelaria

        Get

            Return _fraccionArancelaria

        End Get

        Set(value As String)

            _fraccionArancelaria = value

        End Set

    End Property

    Public Property DescripcionFraccionArancelaria As String Implements IPartidaPedimento.DescripcionFraccionArancelaria

        Get

            Return _descripcionFraccionArancelaria

        End Get

        Set(value As String)

            _descripcionFraccionArancelaria = value

        End Set

    End Property

    Public Property Nico As String Implements IPartidaPedimento.Nico

        Get

            Return _nico

        End Get

        Set(value As String)

            _nico = value

        End Set

    End Property

    Public Property DescripcionNico As String Implements IPartidaPedimento.DescripcionNico

        Get

            Return _descripcionNico

        End Get

        Set(value As String)

            _descripcionNico = value

        End Set

    End Property

    Public Property Vinculacion As Integer Implements IPartidaPedimento.Vinculacion

        Get

            Return _vinculacion

        End Get

        Set(value As Integer)

            _vinculacion = value

        End Set

    End Property

    Public Property DescripcionVinculacion As String Implements IPartidaPedimento.DescripcionVinculacion

        Get

            Return _descripcionVinculacion

        End Get

        Set(value As String)

            _descripcionVinculacion = value

        End Set

    End Property

    Public Property MetodoValoracion As Integer Implements IPartidaPedimento.MetodoValoracion

        Get

            Return _metodoValoracion

        End Get

        Set(value As Integer)

            _metodoValoracion = value

        End Set

    End Property

    Public Property DescripcionMetodoValoracion As String Implements IPartidaPedimento.DescripcionMetodoValoracion

        Get

            Return _descripcionMetodoValoracion

        End Get

        Set(value As String)

            _descripcionMetodoValoracion = value

        End Set

    End Property

    Public Property UnidadMedidaComercial As Integer Implements IPartidaPedimento.UnidadMedidaComercial

        Get

            Return _unidadMedidaComercial

        End Get

        Set(value As Integer)

            _unidadMedidaComercial = value

        End Set

    End Property

    Public Property DescripcionUnidadMedidaComercial As String Implements IPartidaPedimento.DescripcionUnidadMedidaComercial

        Get

            Return _descripcionUnidadMedidaComercial

        End Get

        Set(value As String)

            _descripcionUnidadMedidaComercial = value

        End Set

    End Property

    Public Property PrecioUnitario As Double Implements IPartidaPedimento.PrecioUnitario

        Get

            Return _precioUnitario

        End Get

        Set(value As Double)

            _precioUnitario = value

        End Set

    End Property

    Public Property PaisVendedor As String Implements IPartidaPedimento.PaisVendedor

        Get

            Return _paisVendedor

        End Get

        Set(value As String)

            _paisVendedor = value

        End Set

    End Property

    Public Property DescripcionPaisVendedor As String Implements IPartidaPedimento.DescripcionPaisVendedor

        Get

            Return _descripcionPaisVendedor

        End Get

        Set(value As String)

            _descripcionPaisVendedor = value

        End Set

    End Property

    Public Property PaisComprador As String Implements IPartidaPedimento.PaisComprador

        Get

            Return _paisComprador

        End Get

        Set(value As String)

            _paisComprador = value

        End Set

    End Property

    Public Property DescripcionPaisComprador As String Implements IPartidaPedimento.DescripcionPaisComprador

        Get

            Return _descripcionPaisComprador

        End Get

        Set(value As String)

            _descripcionPaisComprador = value

        End Set

    End Property

    Public Property PaisOrigen As String Implements IPartidaPedimento.PaisOrigen

        Get

            Return _paisOrigen

        End Get

        Set(value As String)

            _paisOrigen = value

        End Set

    End Property

    Public Property DescripcionPaisOrigen As String Implements IPartidaPedimento.DescripcionPaisOrigen

        Get

            Return _descripcionPaisOrigen

        End Get

        Set(value As String)

            _descripcionPaisOrigen = value

        End Set

    End Property

    Public Property PaisDestino As String Implements IPartidaPedimento.PaisDestino

        Get

            Return _paisDestino

        End Get

        Set(value As String)

            _paisDestino = value

        End Set

    End Property

    Public Property DescripcionPaisDestino As String Implements IPartidaPedimento.DescripcionPaisDestino

        Get

            Return _descripcionPaisDestino

        End Get

        Set(value As String)

            _descripcionPaisDestino = value

        End Set

    End Property

    Public Property CantidadUMC As Double Implements IPartidaPedimento.CantidadUMC

        Get

            Return _cantidadUMC

        End Get

        Set(value As Double)

            _cantidadUMC = value

        End Set

    End Property

    Public Property UnidadMedidaTarifa As Integer Implements IPartidaPedimento.UnidadMedidaTarifa

        Get

            Return _unidadMedidaTarifa

        End Get

        Set(value As Integer)

            _unidadMedidaTarifa = value

        End Set

    End Property

    Public Property DescripcionUnidadMedidaTarifa As String Implements IPartidaPedimento.DescripcionUnidadMedidaTarifa

        Get

            Return _descripcionUnidadMedidaTarifa

        End Get

        Set(value As String)

            _descripcionUnidadMedidaTarifa = value

        End Set

    End Property

    Public Property CantidadUMT As Double Implements IPartidaPedimento.CantidadUMT

        Get

            Return _cantidadUMT

        End Get

        Set(value As Double)

            _cantidadUMT = value

        End Set

    End Property

    Public Property Descripcion As String Implements IPartidaPedimento.Descripcion

        Get

            Return _descripcion

        End Get

        Set(value As String)

            _descripcion = value

        End Set

    End Property

    Public Property Marca As String Implements IPartidaPedimento.Marca

        Get

            Return _marca

        End Get

        Set(value As String)

            _marca = value

        End Set

    End Property

    Public Property Modelo As String Implements IPartidaPedimento.Modelo

        Get

            Return _modelo

        End Get

        Set(value As String)

            _modelo = value

        End Set

    End Property

    Public Property CodigoProducto As String Implements IPartidaPedimento.CodigoProducto

        Get

            Return _codigoProducto

        End Get

        Set(value As String)

            _codigoProducto = value

        End Set

    End Property

    Public Property ValorAduanal As Double Implements IPartidaPedimento.ValorAduanal

        Get

            Return _valorAduanal

        End Get

        Set(value As Double)

            _valorAduanal = value

        End Set

    End Property

    Public Property ValorDolares As Double Implements IPartidaPedimento.ValorDolares

        Get

            Return _valorDolares

        End Get

        Set(value As Double)

            _valorDolares = value

        End Set

    End Property

    Public Property ImportePrecioPagado As Double Implements IPartidaPedimento.ImportePrecioPagado

        Get

            Return _importePrecioPagado

        End Get

        Set(value As Double)

            _importePrecioPagado = value

        End Set

    End Property

    Public Property ValorComercial As Double Implements IPartidaPedimento.ValorComercial

        Get

            Return _valorComercial

        End Get

        Set(value As Double)

            _valorComercial = value

        End Set

    End Property

    Public Property Observaciones As String Implements IPartidaPedimento.Observaciones

        Get

            Return _observaciones

        End Get

        Set(value As String)

            _observaciones = value

        End Set

    End Property

    Public Property ResumenImpuestos As List(Of ImpuestoPartidaPedimento) Implements IPartidaPedimento.ResumenImpuestos

        Get

            Return _resumenImpuestos

        End Get

        Set(value As List(Of ImpuestoPartidaPedimento))

            _resumenImpuestos = value

        End Set

    End Property

    Public Property Permisos As List(Of PermisoPartidaPedimento) Implements IPartidaPedimento.Permisos

        Get

            Return _permisos

        End Get

        Set(value As List(Of PermisoPartidaPedimento))

            _permisos = value

        End Set

    End Property

    Public Property Identificadores As List(Of IdentificadorPartidaPedimento) Implements IPartidaPedimento.Identificadores

        Get

            Return _identificadores

        End Get

        Set(value As List(Of IdentificadorPartidaPedimento))

            _identificadores = value

        End Set

    End Property

    Public Property Serie As String Implements IPartidaPedimento.Serie

        Get

            Return _serie

        End Get

        Set(value As String)

            _serie = value

        End Set

    End Property

    Public Property Kilometraje As Integer Implements IPartidaPedimento.Kilometraje

        Get

            Return _kilometraje

        End Get

        Set(value As Integer)

            _kilometraje = value

        End Set

    End Property

    Public Property ValorAgregado As Double Implements IPartidaPedimento.ValorAgregado

        Get

            Return _valorAgregado

        End Get

        Set(value As Double)

            _valorAgregado = value

        End Set

    End Property

    Public Property PrecioUnitarioUSD As Double Implements IPartidaPedimento.PrecioUnitarioUSD

        Get

            Return _precioUnitarioUSD

        End Get

        Set(value As Double)

            _precioUnitarioUSD = value

        End Set

    End Property

    Public Property Archivo As Boolean Implements IPartidaPedimento.Archivo

        Get

            Return _archivo

        End Get

        Set(value As Boolean)

            _archivo = value

        End Set

    End Property

    Public Property Estado As Integer Implements IPartidaPedimento.Estado

        Get

            Return _estado

        End Get

        Set(value As Integer)

            _estado = value

        End Set

    End Property

    Public Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

#End Region

#Region "Metodos"

    Private Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

    Public Overloads Function Agregar(ByVal itemFactura_ As ItemFacturaComercial) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If itemFactura_ IsNot Nothing Then

            Dim partida_ = New PartidaPedimento With {
                        .idPartidaPedimento = ObjectId.GenerateNewId,
                        .FraccionArancelaria = itemFactura_.FraccionArancelaria,
                        .DescripcionFraccionArancelaria = itemFactura_.DescripcionFraccionArancelaria,
                        .Nico = itemFactura_.Nico,
                        .DescripcionNico = itemFactura_.DescripcionNico,
                        .PrecioUnitario = itemFactura_.PrecioUnitario,
                        .PaisOrigen = itemFactura_.PaisOrigen,
                        .DescripcionPaisOrigen = itemFactura_.DescripcionPaisOrigen,
                        .PaisDestino = itemFactura_.PaisDestino,
                        .DescripcionPaisDestino = itemFactura_.DescripcionPaisDestino,
                        .PaisVendedor = itemFactura_.PaisVendedor,
                        .DescripcionPaisVendedor = itemFactura_.DescripcionPaisVendedor,
                        .PaisComprador = itemFactura_.PaisComprador,
                        .DescripcionPaisComprador = itemFactura_.DescripcionPaisComprador,
                        .UnidadMedidaComercial = itemFactura_.UnidadMedidaComercial,
                        .DescripcionUnidadMedidaComercial = itemFactura_.DescripcionUnidadMedidaComercial,
                        .UnidadMedidaTarifa = itemFactura_.UnidadMedidaTarifa,
                        .DescripcionUnidadMedidaTarifa = itemFactura_.DescripcionUnidadMedidaTarifa,
                        .MetodoValoracion = itemFactura_.MetodoValoracion,
                        .DescripcionMetodoValoracion = itemFactura_.DescripcionMetodoValoracion,
                        .Vinculacion = itemFactura_.Vinculacion,
                        .DescripcionVinculacion = itemFactura_.DescripcionVinculacion,
                        .Modelo = itemFactura_.Modelo,
                        .Descripcion = itemFactura_.Descripcion,
                        .Marca = itemFactura_.Marca,
                        .ValorAgregado = itemFactura_.ValorAgregado
                    }

            'partida_.CodigoProducto = _itemfactura.CodigoProducto
            'partida_.Serie = _itemfactura.Serie
            'partida_.Kilometraje = _itemfactura.Kilometraje
            'partida_.ValorAgregado = _itemfactura. '¿Dónde esta el valor agregado?

            tagwatcher_.ObjectReturned = partida_
            tagwatcher_.SetOK()

        Else

            tagwatcher_.SetError(Me, "No cuenta con información el item de la factura, verificar.")

        End If

        Return tagwatcher_

    End Function

    Public Overrides Function Actualizar() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Overrides Function Eliminar() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Overrides Function Archivar() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Overrides Function Agregar() As TagWatcher

        Throw New NotImplementedException()

    End Function

#End Region

End Class