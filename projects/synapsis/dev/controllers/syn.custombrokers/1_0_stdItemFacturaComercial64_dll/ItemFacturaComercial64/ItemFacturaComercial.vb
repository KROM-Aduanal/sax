Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Wma.Exceptions

Public Class ItemFacturaComercial
    Inherits OperacionesPartidas
    Implements IItemFacturaComercial, IItemExtraFacturaComercial

#Region "Atributos"

    Private _secuenciaItem As Integer

    Private _idFacturaComercial As ObjectId

    Private _idItemFactura As ObjectId

    Private _folioFacturaComercial As String

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

    Private _paisVendedor As String

    Private _descripcionPaisVendedor As String

    Private _paisComprador As String

    Private _descripcionPaisComprador As String

    Private _paisOrigen As String

    Private _descripcionPaisOrigen As String

    Private _paisDestino As String

    Private _descripcionPaisDestino As String

    Private _precioUnitario As Double

    Private _cantidadUMC As Double

    Private _unidadMedidaTarifa As Integer

    Private _descripcionUnidadMedidaTarifa As String

    Private _cantidadUMT As Double

    Private _descripcion As String

    Private _valorMercancia As Double

    Private _codigoProducto As String

    Private _secuenciaProducto As Integer

    Private _marca As String

    Private _modelo As String

    Private _kilometraje As Integer

    Private _valoragregado As Integer

    Private _estado As Integer

    Private _tagwatcher As TagWatcher

#End Region

#Region "Propiedades"

    Public Property SecuenciaItem As Integer Implements IItemFacturaComercial.SecuenciaItem

        Get

            Return _secuenciaItem

        End Get

        Set(value As Integer)

            _secuenciaItem = value

        End Set

    End Property

    Public Property idItemFactura As Global.MongoDB.Bson.ObjectId Implements IItemFacturaComercial._idItemFactura

        Get

            Return _idItemFactura

        End Get

        Set(value As Global.MongoDB.Bson.ObjectId)

            _idItemFactura = value

        End Set

    End Property

    Public Property idFacturaComercial As Global.MongoDB.Bson.ObjectId Implements IItemFacturaComercial._idFacturaComercial

        Get

            Return _idFacturaComercial

        End Get

        Set(value As Global.MongoDB.Bson.ObjectId)

            _idFacturaComercial = value

        End Set

    End Property

    Public Property FolioFacturaComercial As String Implements IItemFacturaComercial.FolioFacturaComercial

        Get

            Return _folioFacturaComercial

        End Get

        Set(value As String)

            _folioFacturaComercial = value

        End Set

    End Property

    Public Property FraccionArancelaria As String Implements IItemFacturaComercial.FraccionArancelaria

        Get

            Return _fraccionArancelaria

        End Get

        Set(value As String)

            _fraccionArancelaria = value

        End Set

    End Property

    Public Property DescripcionFraccionArancelaria As String Implements IItemFacturaComercial.DescripcionFraccionArancelaria

        Get

            Return _descripcionFraccionArancelaria

        End Get

        Set(value As String)

            _descripcionFraccionArancelaria = value

        End Set

    End Property

    Public Property Nico As String Implements IItemFacturaComercial.Nico

        Get

            Return _nico

        End Get

        Set(value As String)

            _nico = value

        End Set

    End Property

    Public Property DescripcionNico As String Implements IItemFacturaComercial.DescripcionNico

        Get

            Return _descripcionNico

        End Get

        Set(value As String)

            _descripcionNico = value

        End Set

    End Property

    Public Property Vinculacion As Integer Implements IItemFacturaComercial.Vinculacion

        Get

            Return _vinculacion

        End Get

        Set(value As Integer)

            _vinculacion = value

        End Set

    End Property

    Public Property DescripcionVinculacion As String Implements IItemFacturaComercial.DescripcionVinculacion

        Get

            Return _descripcionVinculacion

        End Get

        Set(value As String)

            _descripcionVinculacion = value

        End Set

    End Property

    Public Property MetodoValoracion As Integer Implements IItemFacturaComercial.MetodoValoracion

        Get

            Return _metodoValoracion

        End Get

        Set(value As Integer)

            _metodoValoracion = value

        End Set

    End Property

    Public Property DescripcionMetodoValoracion As String Implements IItemFacturaComercial.DescripcionMetodoValoracion

        Get

            Return _descripcionMetodoValoracion

        End Get

        Set(value As String)

            _descripcionMetodoValoracion = value

        End Set

    End Property

    Public Property UnidadMedidaComercial As Integer Implements IItemFacturaComercial.UnidadMedidaComercial

        Get

            Return _unidadMedidaComercial

        End Get

        Set(value As Integer)

            _unidadMedidaComercial = value

        End Set

    End Property

    Public Property DescripcionUnidadMedidaComercial As String Implements IItemFacturaComercial.DescripcionUnidadMedidaComercial

        Get

            Return _descripcionUnidadMedidaComercial

        End Get

        Set(value As String)

            _descripcionUnidadMedidaComercial = value

        End Set

    End Property

    Public Property PaisVendedor As String Implements IItemFacturaComercial.PaisVendedor

        Get

            Return _paisVendedor

        End Get

        Set(value As String)

            _paisVendedor = value

        End Set

    End Property

    Public Property DescripcionPaisVendedor As String Implements IItemFacturaComercial.DescripcionPaisVendedor

        Get

            Return _descripcionPaisVendedor

        End Get

        Set(value As String)

            _descripcionPaisVendedor = value

        End Set

    End Property

    Public Property PaisComprador As String Implements IItemFacturaComercial.PaisComprador

        Get

            Return _paisComprador

        End Get

        Set(value As String)

            _paisComprador = value

        End Set

    End Property

    Public Property DescripcionPaisComprador As String Implements IItemFacturaComercial.DescripcionPaisComprador

        Get

            Return _descripcionPaisComprador

        End Get

        Set(value As String)

            _descripcionPaisComprador = value

        End Set

    End Property

    Public Property PaisOrigen As String Implements IItemFacturaComercial.PaisOrigen

        Get

            Return _paisOrigen

        End Get

        Set(value As String)

            _paisOrigen = value

        End Set

    End Property

    Public Property DescripcionPaisOrigen As String Implements IItemFacturaComercial.DescripcionPaisOrigen

        Get

            Return _descripcionPaisOrigen

        End Get

        Set(value As String)

            _descripcionPaisOrigen = value

        End Set

    End Property

    Public Property PaisDestino As String Implements IItemFacturaComercial.PaisDestino

        Get

            Return _paisDestino

        End Get

        Set(value As String)

            _paisDestino = value

        End Set

    End Property

    Public Property DescripcionPaisDestino As String Implements IItemFacturaComercial.DescripcionPaisDestino

        Get

            Return _descripcionPaisDestino

        End Get

        Set(value As String)

            _descripcionPaisDestino = value

        End Set

    End Property

    Public Property PrecioUnitario As Double Implements IItemFacturaComercial.PrecioUnitario

        Get

            Return _precioUnitario

        End Get

        Set(value As Double)

            _precioUnitario = value

        End Set

    End Property

    Public Property CantidadUMC As Double Implements IItemFacturaComercial.CantidadUMC

        Get

            Return _cantidadUMC

        End Get

        Set(value As Double)

            _cantidadUMC = value

        End Set

    End Property

    Public Property UnidadMedidaTarifa As Integer Implements IItemFacturaComercial.UnidadMedidaTarifa

        Get

            Return _unidadMedidaTarifa

        End Get

        Set(value As Integer)

            _unidadMedidaTarifa = value

        End Set

    End Property

    Public Property DescripcionUnidadMedidaTarifa As String Implements IItemFacturaComercial.DescripcionUnidadMedidaTarifa

        Get

            Return _descripcionUnidadMedidaTarifa

        End Get

        Set(value As String)

            _descripcionUnidadMedidaTarifa = value

        End Set

    End Property

    Public Property CantidadUMT As Double Implements IItemFacturaComercial.CantidadUMT

        Get

            Return _cantidadUMT

        End Get

        Set(value As Double)

            _cantidadUMT = value

        End Set

    End Property

    Public Property Descripcion As String Implements IItemFacturaComercial.Descripcion

        Get

            Return _descripcion

        End Get

        Set(value As String)

            _descripcion = value

        End Set

    End Property

    Public Property ValorMercancia As Double Implements IItemFacturaComercial.ValorMercancia

        Get

            Return _valorMercancia

        End Get

        Set(value As Double)

            _valorMercancia = value

        End Set

    End Property

    Public Property Estado As Integer Implements IItemFacturaComercial.Estado

        Get

            Return _estado

        End Get

        Set(value As Integer)

            _estado = value

        End Set

    End Property

    Public Property CodigoProducto As String Implements IItemExtraFacturaComercial.CodigoProducto

        Get

            Return _codigoProducto

        End Get

        Set(value As String)

            _codigoProducto = value

        End Set

    End Property

    Public Property SecuenciaProducto As Integer Implements IItemExtraFacturaComercial.SecuenciaProducto

        Get

            Return _secuenciaProducto

        End Get

        Set(value As Integer)

            _secuenciaProducto = value

        End Set

    End Property

    Public Property Marca As String Implements IItemExtraFacturaComercial.Marca

        Get

            Return _marca

        End Get

        Set(value As String)

            _marca = value

        End Set

    End Property

    Public Property Modelo As String Implements IItemExtraFacturaComercial.Modelo
        Get

            Return _modelo

        End Get

        Set(value As String)

            _modelo = value

        End Set
    End Property

    Public Property Submodelo As String Implements IItemExtraFacturaComercial.Submodelo

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Serie As String Implements IItemExtraFacturaComercial.Serie

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As String)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Kilometraje As Integer Implements IItemExtraFacturaComercial.Kilometraje

        Get

            Return _kilometraje

        End Get

        Set(value As Integer)

            _kilometraje = value

        End Set

    End Property

    Public Property ValorAgregado As Integer Implements IItemFacturaComercial.ValorAgregado

        Get

            Return _valoragregado

        End Get

        Set(value As Integer)

            _valoragregado = value

        End Set

    End Property

    Public Property IItemExtraFacturaComercial_Estado As Integer Implements IItemExtraFacturaComercial.Estado

        Get

            Throw New NotImplementedException()

        End Get

        Set(value As Integer)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property Tagwatcher() As TagWatcher

        Get

            Return _tagwatcher

        End Get

        Set(ByVal value As TagWatcher)

            _tagwatcher = value

        End Set

    End Property

    Private Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

#End Region

#Region "Constructores"

    Sub New()

        _tagwatcher = New TagWatcher()

    End Sub

#End Region

#Region "Metodos"

    Private Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

    Public Overloads Function Agregar(ByVal constructorFacturaComercial_ As DocumentoElectronico) As TagWatcher

        Dim lista_ As New List(Of IItemFacturaComercial)

        If constructorFacturaComercial_ IsNot Nothing Then

            For indice_ As Int32 = 1 To constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC4).CantidadPartidas

                With constructorFacturaComercial_.Seccion(SeccionesFacturaComercial.SFAC4).Partida(indice_)

                    If .estado = 1 And .archivado = False Then

                        Dim item_ As New ItemFacturaComercial With {
                            .idFacturaComercial = New ObjectId(constructorFacturaComercial_.Id),
                            .FolioFacturaComercial = constructorFacturaComercial_.FolioDocumento,
                            .PaisVendedor = Mid(constructorFacturaComercial_.Attribute(CA_PAIS_FACTURACION).Valor, 1, 3),
                            .DescripcionPaisVendedor = constructorFacturaComercial_.Attribute(CA_PAIS_FACTURACION).Valor,
                            .PaisComprador = Mid(constructorFacturaComercial_.Attribute(CA_PAIS_FACTURACION).Valor, 1, 3),
                            .DescripcionPaisComprador = constructorFacturaComercial_.Attribute(CA_PAIS_FACTURACION).Valor,
                            .Vinculacion = constructorFacturaComercial_.Attribute(CA_CVE_VINCULACION).Valor,
                            .Estado = 1
                        }
                        'item_.idItemFactura = .Attribute() No se encuentra actualmente implementado a nivel de factura comercial
                        item_.CantidadUMC = .Attribute(CA_CANTIDAD_COMERCIAL_PARTIDA).Valor
                        item_.CantidadUMT = .Attribute(CA_CANTIDAD_TARIFA_PARTIDA).Valor
                        item_.Descripcion = .Attribute(CA_DESCRIPCION_PARTE_PARTIDA).Valor
                        item_.MetodoValoracion = .Attribute(CA_CVE_METODO_VALORACION_PARTIDA).Valor
                        item_.DescripcionMetodoValoracion = .Attribute(CA_CVE_METODO_VALORACION_PARTIDA).ValorPresentacion
                        item_.FraccionArancelaria = Mid(.Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).ValorPresentacion, 1, 8)
                        item_.DescripcionFraccionArancelaria = Mid(.Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).ValorPresentacion, 10, .Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).ValorPresentacion.Length)
                        item_.Nico = Mid(.Attribute(CA_FRACCION_NICO_PARTIDA).ValorPresentacion, 1, 2)
                        item_.DescripcionNico = Mid(.Attribute(CA_FRACCION_NICO_PARTIDA).ValorPresentacion, 4, .Attribute(CA_FRACCION_NICO_PARTIDA).ValorPresentacion.Length)
                        item_.PrecioUnitario = .Attribute(CA_PRECIO_UNITARIO_PARTIDA).Valor
                        'Para los paises se tiene guardado el _id o la clave con nombre se requiere solo la cve y es necesario separarlos
                        'Aplica al país origen y al país facturación para el CA_PAIS_FACTURACION - se pueden guardar en separado y en valor presentación
                        'Para CA_PAIS_ORIGEN_PARTIDA en valor presentación se puede guardar el cve
                        item_.PaisOrigen = Mid(.Attribute(CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion, 1, 3)
                        item_.DescripcionPaisOrigen = Mid(.Attribute(CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion, 1, .Attribute(CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion.Length)
                        'Split(3, .Attribute(CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion, "-", CompareMethod.Text).ToString
                        item_.PaisDestino = Mid(.Attribute(CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion, 1, 3)
                        item_.DescripcionPaisDestino = .Attribute(CA_PAIS_ORIGEN_PARTIDA).ValorPresentacion
                        item_.SecuenciaItem = .Attribute(CP_NUMERO_PARTIDA).Valor 'Actualmente viene vacio se debe implementar
                        item_.UnidadMedidaComercial = .Attribute(CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).Valor
                        item_.DescripcionUnidadMedidaComercial = .Attribute(CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA).ValorPresentacion
                        item_.UnidadMedidaTarifa = .Attribute(CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).Valor
                        item_.DescripcionUnidadMedidaTarifa = .Attribute(CA_UNIDAD_MEDIDA_TARIFA_PARTIDA).ValorPresentacion
                        item_.ValorMercancia = .Attribute(CA_VALOR_MERCANCIA_PARTIDA).Valor
                        item_.Modelo = .Attribute(CA_MODELO_PARTIDA).Valor
                        item_.Marca = .Attribute(CA_MARCA_PARTIDA).Valor
                        item_.ValorAgregado = 10
                        'La líne 804 es de prueba para considerar valor agregado eliminar sino se requiere o no se usara
                        'item_.ValorAgregado = .Attribute(CA_VALOR_AGREGADO).Valor 'No se tiene agregado actualmente en FC
                        'item_.SecuenciaProducto = .Attribute(CA_SECUENCIA_PRODUCTO).Valor 'No se tiene agregado actualmente en FC
                        'item_.Submodelo = .Attribute(CA_SUBMODELO_PARTIDA).Valor

                        lista_.Add(item_)

                        _tagwatcher.SetOK()

                    Else

                        _tagwatcher.SetError(Me, "El ítem de la factura: " + constructorFacturaComercial_.FolioDocumento +
                                             " con secuencia: " + indice_ + " se encuentra inabilitado y archivado. No se agregará.")

                    End If

                End With

            Next

        End If

        If _tagwatcher.Status = TagWatcher.TypeStatus.Ok Then

            _tagwatcher.ObjectReturned = lista_

        End If

        Return _tagwatcher

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