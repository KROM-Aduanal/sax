Imports MongoDB.Bson
Imports Wma.Exceptions

Public Class ItemPartida
    Inherits OperacionesPartidas

#Region "Atributos"

    Property _iditempartida As ObjectId

    Property secuenciaitempartida As Integer

    Property _iditemfactura As ObjectId 'Object Id del item de la factura no esta a nivel factura implementado

    Property _idfacturacomercial As ObjectId

    Property _idpartidapedimento As ObjectId

    Property secuenciaitemfactura As Integer

    Property secuenciapartidapedimento As Integer

    Property fraccionarancelaria As String

    Property descripcionfraccionarancelaria As String

    Property nico As String

    Property descripcionnico As String

    Property unidadmedidacomercial As Integer

    Property descripcionunidadmedidacomercial As String

    Property unidadmedidatarifa As Integer

    Property descripcionunidadmedidatarifa As String

    Property cantidadcomercial As Double

    Property cantidadtarifa As Double

    Property paisvendedor As String

    Property descripcionpaisvendedor As String

    Property paiscomprador As String

    Property descripcionpaiscomprador As String

    Property paisorigen As String

    Property descripcionpaisorigen As String

    Property paisdestino As String

    Property descripcionpaisdestino As String

    Property preciounitario As Double

    Property valormercancia As Double

    Property codigoproducto As String

    Property secuenciaproducto As Integer

    Property valoragregado As Integer

    Property archivado As Boolean

    Property estado As Integer

#End Region

#Region "Metodos"

    Public Overloads Function Agregar(ByVal itemFactura_ As ItemFacturaComercial,
                                      ByVal partida_ As PartidaPedimento,
                                      Optional ByVal secuenciaPartida_ As Integer = 0) _
                                      As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If itemFactura_ IsNot Nothing And partida_ IsNot Nothing Then

            Dim itemPartida_ As New ItemPartida With {
                        ._iditempartida = ObjectId.GenerateNewId,
                        ._idpartidapedimento = partida_.idPartidaPedimento,
                        ._idfacturacomercial = itemFactura_.idFacturaComercial,
                        ._iditemfactura = itemFactura_.idItemFactura,
                        .secuenciaitemfactura = itemFactura_.SecuenciaItem,
                        .secuenciapartidapedimento = secuenciaPartida_,
                        .fraccionarancelaria = itemFactura_.FraccionArancelaria,
                        .descripcionfraccionarancelaria = itemFactura_.DescripcionFraccionArancelaria,
                        .nico = itemFactura_.Nico,
                        .descripcionnico = itemFactura_.DescripcionNico,
                        .unidadmedidacomercial = partida_.UnidadMedidaComercial,
                        .cantidadcomercial = itemFactura_.CantidadUMC,
                        .descripcionunidadmedidacomercial = partida_.DescripcionUnidadMedidaComercial,
                        .cantidadtarifa = itemFactura_.CantidadUMT,
                        .unidadmedidatarifa = partida_.UnidadMedidaTarifa,
                        .descripcionunidadmedidatarifa = partida_.DescripcionUnidadMedidaTarifa,
                        .paisvendedor = itemFactura_.PaisVendedor,
                        .descripcionpaisvendedor = itemFactura_.DescripcionPaisVendedor,
                        .paiscomprador = itemFactura_.PaisComprador,
                        .descripcionpaiscomprador = itemFactura_.DescripcionPaisComprador,
                        .paisorigen = itemFactura_.PaisOrigen,
                        .descripcionpaisorigen = itemFactura_.DescripcionPaisOrigen,
                        .paisdestino = itemFactura_.PaisDestino,
                        .descripcionpaisdestino = itemFactura_.DescripcionPaisDestino,
                        .preciounitario = itemFactura_.PrecioUnitario,
                        .valormercancia = itemFactura_.ValorMercancia,
                        .codigoproducto = itemFactura_.CodigoProducto,
                        .secuenciaproducto = itemFactura_.SecuenciaProducto,
                        .valoragregado = itemFactura_.ValorAgregado,
                        .archivado = False,
                        .estado = 1
            }

            '_itempartida.SecuenciaProducto = 'Debe venir de FC pero no se tiene implementado
            '_itempartida.ValorAgregado = 'Debe venir de FC pero no se tiene implementado

            tagwatcher_.ObjectReturned = itemPartida_

            tagwatcher_.SetOK()

        Else

            tagwatcher_.SetError(Me, "La información está incompleta para asociar el item con la partida.")

        End If

        Return tagwatcher_

    End Function

    Public Overrides Function Agregar() As TagWatcher

        Throw New NotImplementedException()

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

#End Region

End Class