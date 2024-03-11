Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Controllers
Imports Sax
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Wma.Exceptions

Public Class GeneradorPartidasPedimento
    Implements IGeneradorPartidasPedimento

#Region "Atributos"

    Private _tipoAgrupacion As IGeneradorPartidasPedimento.TipoAgrupaciones

    Private _itemsFacturaComercial As List(Of IItemFacturaComercial)

    Private _partidasPedimento As New List(Of IPartidaPedimento)

    Private _agrupacionesDisponibles As New List(Of IGeneradorPartidasPedimento.TipoAgrupaciones)

    Private _informacionAgrupacion As New InformacionAgrupacion

    Private ReadOnly _tieneCuentasAduaneras As Boolean = False

    Private _tieneCuotaCompensatoria As Boolean = False

    Private ReadOnly _esIMMEX As Boolean = False

    Private ReadOnly _esRetorno As Boolean = False

    Private ReadOnly _idPedimentoModulo As ObjectId

    Private ReadOnly _pedimentoDocumento As DocumentoElectronico

    Private ReadOnly _entorno As Integer

    Private ReadOnly _session As IClientSessionHandle

    Private _estado As TagWatcher

#End Region

#Region "Propiedades"

    Public Overloads Property Estado As TagWatcher Implements IGeneradorPartidasPedimento.Estado

        Get

            Return _estado

        End Get

        Set(value As TagWatcher)

            _estado = value

        End Set

    End Property

    Private Property TipoAgrupacion As IGeneradorPartidasPedimento.TipoAgrupaciones Implements IGeneradorPartidasPedimento.TipoAgrupacion

        Get

            Return _tipoAgrupacion

        End Get

        Set(value As IGeneradorPartidasPedimento.TipoAgrupaciones)

            _tipoAgrupacion = value

        End Set

    End Property

    Private Property ItemsFacturaComercial As List(Of IItemFacturaComercial) Implements IGeneradorPartidasPedimento.ItemsFacturaComercial

        Get

            Return _itemsFacturaComercial

        End Get

        Set(value As List(Of Controllers.IItemFacturaComercial))

            _itemsFacturaComercial = value

        End Set

    End Property

    Private Property PartidasPedimento As List(Of IPartidaPedimento) Implements IGeneradorPartidasPedimento.PartidasPedimento

        Get

            Return _partidasPedimento

        End Get

        Set(value As List(Of Controllers.IPartidaPedimento))

            _partidasPedimento = value

        End Set

    End Property

    Private Property AgrupacionesDisponibles As List(Of IGeneradorPartidasPedimento.TipoAgrupaciones) Implements IGeneradorPartidasPedimento.AgrupacionesDisponibles

        Get

            Return _agrupacionesDisponibles

        End Get

        Set(value As List(Of IGeneradorPartidasPedimento.TipoAgrupaciones))

            _agrupacionesDisponibles = value

        End Set

    End Property

    Private Property InformacionAgrupacion As InformacionAgrupacion Implements IGeneradorPartidasPedimento.InformacionAgrupacion

        Get

            Return _informacionAgrupacion

        End Get

        Set(value As InformacionAgrupacion)

            _informacionAgrupacion = value

        End Set

    End Property

    Private Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

#End Region

#Region "Constructores"

    Sub New(ByVal entorno_ As Integer,
            ByVal facturasComerciales_ As List(Of DocumentoElectronico),
            ByVal pedimento_ As DocumentoElectronico,
            Optional ByVal idPedimento_ As ObjectId = Nothing,
            Optional ByVal session_ As IClientSessionHandle = Nothing
            )

        _session = session_

        _idPedimentoModulo = idPedimento_

        _entorno = entorno_

        _pedimentoDocumento = pedimento_

        _estado = New TagWatcher()

        _estado = ProcesarItemsFactura(facturasComerciales_)

        _informacionAgrupacion.tipooperacion = _pedimentoDocumento.Attribute(CA_TIPO_OPERACION).Valor

        If _informacionAgrupacion.tipooperacion = 1 Then

            'importaciones
            'Después que se tenga mejorada la interfaz se debe ajustar para revisar el componente o algún otro campo
            _tieneCuentasAduaneras = IIf(_pedimentoDocumento.Seccion(SeccionesPedimento.ANS19).CantidadPartidas > 0, True, False)

        Else

            'Exportaciones
            'Se revisa si tiene descargos porque aplica a empresas IMMEX (hasta el momento es lo mejor que se aproxima)
            'Se sugiere revisar el identificador general "IM" Para detectar si es o no IMMEX
            _esIMMEX = IIf(_pedimentoDocumento.Seccion(SeccionesPedimento.ANS20).CantidadPartidas > 0, True, False)

            'Se valida que sea una operación de retorno para aplicar valor agregado
            _esRetorno = IIf(_pedimentoDocumento.Attribute(CA_CVE_PEDIMENTO).Valor = 32, True, False)

        End If

        If _estado.Status = TagWatcher.TypeStatus.Ok Then

            _itemsFacturaComercial = _estado.ObjectReturned

        Else

            _estado.SetError(Me, "Ocurrio un detalle al procesar las facturas recibidas, verifique la información.")

        End If

    End Sub

    Sub New(ByVal entorno_ As Integer,
            Optional ByVal session_ As IClientSessionHandle = Nothing
            )

        _session = session_

        _entorno = entorno_

        _estado = New TagWatcher()

    End Sub

#End Region

#Region "Metodos"

    Public Function GeneraOpcionesAgrupacion(ByVal itemsFactura_ As List(Of IItemFacturaComercial)) As List(Of IGeneradorPartidasPedimento.TipoAgrupaciones) Implements IGeneradorPartidasPedimento.GeneraOpcionesAgrupacion

        Dim listaCampos_ As New List(Of String)
        Dim listaFraccionesPais_ As New List(Of String)

        If itemsFactura_.Count > 0 And itemsFactura_ IsNot Nothing Then

            For Each iItemFacturaComercial_ In itemsFactura_

                If iItemFacturaComercial_.FraccionArancelaria <> "" And iItemFacturaComercial_.Nico <> "" Then

                    If Not listaCampos_.Contains(iItemFacturaComercial_.FraccionArancelaria And iItemFacturaComercial_.Nico) Then

                        listaCampos_.Add(iItemFacturaComercial_.FraccionArancelaria)

                        listaCampos_.Add(iItemFacturaComercial_.Nico)

                        If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.FraccionNico) Then

                            _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.FraccionNico)

                        End If

                        _estado.SetOK()

                    Else

                        _estado.SetOKInfo(Me, "Ocurrio un detalle al agregar el tipo de agrupación: FraccionNico, no se cumple con lo solicitado.")

                    End If

                Else

                    _estado.SetError(Me, "No se cuenta con Fracción arancelaria y Nico.")

                End If

                If iItemFacturaComercial_.UnidadMedidaComercial > 0 Then

                    If Not listaCampos_.Contains(iItemFacturaComercial_.UnidadMedidaComercial) Then

                        listaCampos_.Add(iItemFacturaComercial_.UnidadMedidaComercial)

                        If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.UMCPrecioUnitarioCalculado) Then

                            _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.UMCPrecioUnitarioCalculado)

                        End If

                        _estado.SetOK()

                    Else

                        _estado.SetOKInfo(Me, "Ocurrio un detalle al agregar el tipo de agrupación: UMCPrecioUnitarioPromedio, no se cumple con lo solicitado.")

                    End If

                Else

                    _estado.SetError(Me, "No se cuenta con Unidad de medida comercial.")

                End If

                If _informacionAgrupacion.tipooperacion = 1 Then

                    'Para importaciones
                    If iItemFacturaComercial_.PaisVendedor <> "" And iItemFacturaComercial_.PaisOrigen <> "" Then

                        If Not listaCampos_.Contains(iItemFacturaComercial_.PaisVendedor) Or listaCampos_.Contains(iItemFacturaComercial_.PaisOrigen) Then

                            listaCampos_.Add(iItemFacturaComercial_.PaisVendedor)

                            listaCampos_.Add(iItemFacturaComercial_.PaisOrigen)

                            If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.PaisVentaCompraOrigenDestino) Then

                                _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.PaisVentaCompraOrigenDestino)

                            End If

                            _estado.SetOK()

                        Else

                            _estado.SetOKInfo(Me, "Ocurrio un detalle al agregar el tipo de agrupación: PaisVentaCompraOrigenDestino, no se cumple con lo solicitado.")


                        End If

                    Else

                        _estado.SetError(Me, "No se cuenta con País vendedor y origen.")

                    End If

                    'En este caso se requiere saber si tiene cuenta aduanera solo aplica Importaciones
                    If _tieneCuentasAduaneras = True Then

                        If iItemFacturaComercial_.PrecioUnitario > 0 Then

                            If Not listaCampos_.Contains(iItemFacturaComercial_.PrecioUnitario) Then

                                listaCampos_.Add(iItemFacturaComercial_.PrecioUnitario)

                                If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.PrecioUnitario) Then

                                    _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.PrecioUnitario)

                                End If

                                _estado.SetOK()

                            Else


                                _estado.SetOKInfo(Me, "No se puede aplicar al tipo de agrupación: PrecioUnitario, no se cumple con lo solicitado sobre precios estimados.")

                            End If

                        Else

                            _estado.SetOKBut(Me, "No se cuenta con Precio unitario disponible para precios estimados.")

                        End If

                    End If

                    'Agregamos el listado de Fracciones, para revisar Cuotas compensatorias (Precios de referencia)
                    If Not listaFraccionesPais_.Contains(iItemFacturaComercial_.FraccionArancelaria + iItemFacturaComercial_.Nico) Then

                        listaFraccionesPais_.Add(iItemFacturaComercial_.FraccionArancelaria + iItemFacturaComercial_.Nico)

                    End If

                    '************
                    'Agregamos el listado de Fracciones con País de Origen, para revisar Cuotas compensatorias (Precios de referencia) Pendiente por si se ocupa
                    'If Not listaFraccionesPais_.Contains(iItemFacturaComercial_.FraccionArancelaria + iItemFacturaComercial_.Nico + "|" + iItemFacturaComercial_.PaisOrigen) Then

                    '    listaFraccionesPais_.Add(iItemFacturaComercial_.FraccionArancelaria + iItemFacturaComercial_.Nico + "|" + iItemFacturaComercial_.PaisOrigen)

                    'End If
                    '************

                    'Para este se requiere validar el tipo de operación (Impo -> pueden variar & Expo -> tiene valores fijos )
                    If iItemFacturaComercial_.MetodoValoracion > 0 And iItemFacturaComercial_.Vinculacion > 0 Then

                        If Not listaCampos_.Contains(iItemFacturaComercial_.MetodoValoracion And iItemFacturaComercial_.Vinculacion) Then

                            listaCampos_.Add(iItemFacturaComercial_.MetodoValoracion)

                            listaCampos_.Add(iItemFacturaComercial_.Vinculacion)

                            If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.MetodoValoracionVinculacion) Then

                                _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.MetodoValoracionVinculacion)

                            End If

                            _estado.SetOK()

                        Else

                            _estado.SetOKInfo(Me, "Ocurrio un detalle al agregar el tipo de agrupación: MetodoValoracionVinculacion, no se cumple con lo solicitado.")


                        End If

                    Else

                        _estado.SetError(Me, "No se cuenta con Método de valoración y vinculación.")

                    End If

                Else

                    'Para exportaciones
                    If iItemFacturaComercial_.PaisComprador <> "" And iItemFacturaComercial_.PaisDestino <> "" Then

                        If Not listaCampos_.Contains(iItemFacturaComercial_.PaisComprador) Or listaCampos_.Contains(iItemFacturaComercial_.PaisDestino) Then

                            listaCampos_.Add(iItemFacturaComercial_.PaisComprador)

                            listaCampos_.Add(iItemFacturaComercial_.PaisDestino)

                            If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.PaisVentaCompraOrigenDestino) Then

                                _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.PaisVentaCompraOrigenDestino)

                            End If

                            _estado.SetOK()

                        Else

                            _estado.SetOKInfo(Me, "Ocurrio un detalle al agregar el tipo de agrupación: PaisVentaCompraOrigenDestino, no se cumple con lo solicitado.")


                        End If

                    Else

                        _estado.SetError(Me, "No se cuenta con País comprador y destino.")

                    End If

                End If

            Next

            'Solo exportaciones, si es immex y retorno
            If _informacionAgrupacion.tipooperacion = 2 Then

                If _esIMMEX = True And _esRetorno = True Then

                    If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.ValorAgregado) Then

                        _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.ValorAgregado)

                        _estado.SetOK()

                    End If

                Else

                    _estado.SetOKInfo(Me, "No se puede aplicar al tipo de agrupación: ValorAgregado, no se cumple con lo solicitado.")

                End If

            End If

            'Solo si son importaciones - Precio de Referencia
            'se consultan las fracciones y se revisan si tienen C.C. con uno que tenga se debe validar el Precio unitario sea igual en los items
            'Se deja esto como apoyo pero se debe revisar detenidamente lo de consultar "n" fracciones con Países de origen diferentes por que ahorita solo se puede
            'mandar de un solo país de origen de "n" fracciones.
            If listaFraccionesPais_.Count > 0 And _informacionAgrupacion.tipooperacion = 1 Then

                Dim tigie_ As New ControladorTIGIE()

                tigie_.ConsultaFraccionArancelaria(Of ConstructorTIGIE)(listaFraccionesPais_, IControladorTIGIE.TipoOperacion.Importacion, "DEU", DateTime.Now.Date)

                If tigie_.Estado.Status = TagWatcher.TypeStatus.Ok Then

                    'PENDIENTE CORRECCIÓN DE ANTONIO ZAMORA PARA LEER LA LISTA (ahora manda error).
                    Dim listaFraccionesArancelarias_ As List(Of hsCode) = tigie_.Estado.ObjectReturned

                    For Each fraccionArancelaria_ In listaFraccionesArancelarias_

                        If fraccionArancelaria_.regulacionesArancelarias.cuotasCompensatorias.Count > 0 Then

                            _tieneCuotaCompensatoria = True

                            Exit For

                        End If

                    Next

                End If

                If _tieneCuotaCompensatoria = True Then


                    If Not _agrupacionesDisponibles.Contains(IGeneradorPartidasPedimento.TipoAgrupaciones.PrecioUnitario) Then

                        _agrupacionesDisponibles.Add(IGeneradorPartidasPedimento.TipoAgrupaciones.PrecioUnitario)

                    End If

                Else

                    _estado.SetOKInfo(Me, "Ocurrio un error al agregar el tipo de agrupación: PrecioUnitario, no se cumple con lo solicitado sobre precios de referencia.")


                End If


            End If

        End If

        Return _agrupacionesDisponibles

    End Function

    Public Function AgruparItemsFacturaPor(ByVal tipoAgrupacionSeleccionada_ As IGeneradorPartidasPedimento.TipoAgrupaciones,
                                           ByVal itemsFactura_ As List(Of IItemFacturaComercial)) _
                                           As TagWatcher Implements IGeneradorPartidasPedimento.AgruparItemsFacturaPor

        Dim partidas_ As New List(Of IPartidaPedimento)
        Dim partida_ = New PartidaPedimento()
        Dim itemsAsociados_ = New List(Of ItemPartida)
        Dim existePartida_ = True
        Dim secuenciapartida_ = 0

        ProcesarEntradas(tipoAgrupacionSeleccionada_)

        'Comprobamos que tenga un item y que no este vacía
        If itemsFactura_.Count > 0 And itemsFactura_ IsNot Nothing Then

            For Each itemFactura_ In itemsFactura_

                Dim secpartida_ As IEnumerable(Of Integer)
                'Comprobamos que el nuevo item de factura no este en alguna partida
                If partidas_.Count > 0 Then

                    If _tieneCuentasAduaneras = True Or _tieneCuotaCompensatoria = True Then

                        'Obtenemos la secuencia de la partida validando el precio unitario
                        secpartida_ = From partidaRegistrada_ In partidas_
                                      Where partidaRegistrada_.FraccionArancelaria = itemFactura_.FraccionArancelaria _
                                                And partidaRegistrada_.Nico = itemFactura_.Nico _
                                                And partidaRegistrada_.UnidadMedidaComercial = itemFactura_.UnidadMedidaComercial _
                                                And partidaRegistrada_.UnidadMedidaTarifa = itemFactura_.UnidadMedidaTarifa _
                                                And partidaRegistrada_.PrecioUnitario = itemFactura_.PrecioUnitario _
                                                And partidaRegistrada_.PaisOrigen = itemFactura_.PaisOrigen _
                                                And partidaRegistrada_.PaisDestino = itemFactura_.PaisDestino _
                                                And partidaRegistrada_.PaisVendedor = itemFactura_.PaisVendedor _
                                                And partidaRegistrada_.PaisComprador = itemFactura_.PaisComprador _
                                                And partidaRegistrada_.MetodoValoracion = itemFactura_.MetodoValoracion _
                                                And partidaRegistrada_.Vinculacion = itemFactura_.Vinculacion
                                      Select partidaRegistrada_.SecuenciaPartida

                        'Falta validar el valor agregado porque no se tiene implementado en la factura comercial
                    ElseIf True Then

                        secpartida_ = From partidaRegistrada_ In partidas_
                                      Where partidaRegistrada_.FraccionArancelaria = itemFactura_.FraccionArancelaria _
                                                And partidaRegistrada_.Nico = itemFactura_.Nico _
                                                And partidaRegistrada_.UnidadMedidaComercial = itemFactura_.UnidadMedidaComercial _
                                                And partidaRegistrada_.UnidadMedidaTarifa = itemFactura_.UnidadMedidaTarifa _
                                                And partidaRegistrada_.PaisOrigen = itemFactura_.PaisOrigen _
                                                And partidaRegistrada_.PaisDestino = itemFactura_.PaisDestino _
                                                And partidaRegistrada_.PaisVendedor = itemFactura_.PaisVendedor _
                                                And partidaRegistrada_.PaisComprador = itemFactura_.PaisComprador _
                                                And partidaRegistrada_.MetodoValoracion = itemFactura_.MetodoValoracion _
                                                And partidaRegistrada_.Vinculacion = itemFactura_.Vinculacion _
                                                And partidaRegistrada_.ValorAgregado = itemFactura_.ValorAgregado
                                      Select partidaRegistrada_.SecuenciaPartida

                    Else

                        'Obtenemos la secuencia de la partida
                        secpartida_ = From partidaRegistrada_ In partidas_
                                      Where partidaRegistrada_.FraccionArancelaria = itemFactura_.FraccionArancelaria _
                                                And partidaRegistrada_.Nico = itemFactura_.Nico _
                                                And partidaRegistrada_.UnidadMedidaComercial = itemFactura_.UnidadMedidaComercial _
                                                And partidaRegistrada_.UnidadMedidaTarifa = itemFactura_.UnidadMedidaTarifa _
                                                And partidaRegistrada_.PaisOrigen = itemFactura_.PaisOrigen _
                                                And partidaRegistrada_.PaisDestino = itemFactura_.PaisDestino _
                                                And partidaRegistrada_.PaisVendedor = itemFactura_.PaisVendedor _
                                                And partidaRegistrada_.PaisComprador = itemFactura_.PaisComprador _
                                                And partidaRegistrada_.MetodoValoracion = itemFactura_.MetodoValoracion _
                                                And partidaRegistrada_.Vinculacion = itemFactura_.Vinculacion
                                      Select partidaRegistrada_.SecuenciaPartida

                    End If

                    If secpartida_.Count > 0 Then

                        secuenciapartida_ = secpartida_.ToList(0)
                        existePartida_ = True

                    Else

                        secuenciapartida_ = 0
                        existePartida_ = False

                    End If

                    If secuenciapartida_ > 0 Then

                        _estado = partida_.Agregar(itemFactura_)

                        If _estado.Status = TagWatcher.TypeStatus.Ok Then

                            partida_ = _estado.ObjectReturned
                            partida_.SecuenciaPartida = secuenciapartida_

                        End If

                    Else

                        existePartida_ = False

                    End If

                Else

                    existePartida_ = False

                End If

                'Sino existe la partida se debe mandar a llenar con los nuevos datos
                If existePartida_ = False Then

                    _estado = partida_.Agregar(itemFactura_)

                    If _estado.Status = TagWatcher.TypeStatus.Ok Then

                        partida_ = _estado.ObjectReturned
                        partida_.SecuenciaPartida = secuenciapartida_

                    End If

                End If

                'Se tiene en la lista de partidas sino agregarlo
                If partidas_.Count > 0 Then

                    'Esta parte se deja para los casos que son especiales
                    Select Case tipoAgrupacionSeleccionada_

                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.FraccionNico


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.UMCPrecioUnitarioCalculado


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.PaisVentaCompraOrigenDestino


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.ContribucionFormaPago


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.TasaTipoTasa


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.MetodoValoracionVinculacion


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.ValorAgregado


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.PrecioUnitario


                        Case IGeneradorPartidasPedimento.TipoAgrupaciones.SinAgrupacion


                        Case Else

                    End Select

                    If secuenciapartida_ = 0 Then

                        partida_.SecuenciaPartida = partidas_.Count + 1
                        secuenciapartida_ = partida_.SecuenciaPartida
                        partidas_.Add(partida_)

                    End If

                ElseIf partidas_.Count = 0 Then

                    partida_.SecuenciaPartida = 1
                    secuenciapartida_ = 1
                    partidas_.Add(partida_)

                End If

                'Se manda la información de la partida asociada
                If secuenciapartida_ > 0 Then

                    Dim itemPartida_ As New ItemPartida

                    _estado = itemPartida_.Agregar(itemFactura_, partida_, secuenciapartida_)

                    If _estado.Status = TagWatcher.TypeStatus.Ok Then

                        itemsAsociados_.Add(_estado.ObjectReturned)

                    End If

                End If

            Next

            _informacionAgrupacion.itemsasociados() = itemsAsociados_

            'Se procesa la información de las facturas
            ProcesarFacturas(itemsAsociados_)

            If _estado.Status = TagWatcher.TypeStatus.Ok Then

                'Se procesan los cálculos con los valores generados
                ProcesarCalculos(partidas_, _informacionAgrupacion.recursossasociadas, itemsAsociados_)

                Dim partidasNuevo_ As IEnumerable(Of IPartidaPedimento)

                partidasNuevo_ = partidas_.OrderBy(Function(x) x.FraccionArancelaria And x.Nico).ToList()

                Reordernar(partidasNuevo_, itemsAsociados_)

                _informacionAgrupacion.numerototalpartidas = partidasNuevo_.Count

                If _estado.Status = TagWatcher.TypeStatus.Ok Then

                    _estado.SetOK()
                    _estado.ObjectReturned = partidasNuevo_

                Else

                    _estado.SetError(Me, _estado.ErrorDescription)

                End If

            Else

                _estado.SetError("Externo", _estado.ErrorDescription)

            End If

            'Se deben procesar los campos de validación al momento (SOLO AQUELLOS QUE SE TENGAN Y QUE SE PUEDAN VERIFICAR)

        Else

            _estado.SetError(Me, "No cuenta con Items de factura, que se puedan procesar, verifique su información.")

        End If

        Return _estado

    End Function

    Public Function GuardarInformacionAgrupaciones(ByVal session_ As IClientSessionHandle,
                                            ByVal informacionAgrupacion_ As InformacionAgrupacion,
                                            Optional ByVal entorno_ As Integer = Nothing) As TagWatcher Implements IGeneradorPartidasPedimento.GuardarInformacionAgrupaciones

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of InformacionAgrupacion)((New InformacionAgrupacion).GetType.Name)

        Dim result_ = operationsDB_.InsertOneAsync(session_, informacionAgrupacion_).ConfigureAwait(False)

        Dim filter_ = Builders(Of InformacionAgrupacion).Filter.Eq(Function(x) x._id, informacionAgrupacion_._id)

        _estado.ObjectReturned = operationsDB_.Find(session_, filter_).CountDocuments

        If _estado.ObjectReturned > 0 Then

            _estado.SetOK()

            _estado.ObjectReturned = informacionAgrupacion_

        Else

            _estado.SetError(Me, "No se inserto correctamente la información de Agrupación de partidas, favor de comprobar los datos de partidas.")

        End If

        Return _estado

    End Function

    Private Function ProcesarItemsFactura(ByVal constructoresFacturaComercial_ As List(Of DocumentoElectronico)) As TagWatcher

        Dim items_ = New List(Of IItemFacturaComercial)

        If constructoresFacturaComercial_ IsNot Nothing And constructoresFacturaComercial_.Count > 0 Then

            constructoresFacturaComercial_.ForEach(Sub(ByVal facturaComercial_ As DocumentoElectronico)

                                                       Dim procesarFactura = New ItemFacturaComercial()

                                                       _estado = procesarFactura.Agregar(facturaComercial_)

                                                       If _estado.Status = TagWatcher.TypeStatus.Ok Then

                                                           If _estado.ObjectReturned.Count > 0 Then

                                                               For Each itemFactura_ In _estado.ObjectReturned

                                                                   items_.Add(itemFactura_)

                                                               Next

                                                           Else

                                                               _estado.SetError(Me, "No se encontraron items en la factura comercial: " + facturaComercial_.FolioDocumento)

                                                           End If

                                                       Else

                                                           _estado.SetError(Me, "Hubo un error al procesar los items de las factura comercial: " + facturaComercial_.FolioDocumento)

                                                       End If

                                                   End Sub)

            If _estado.Status = TagWatcher.TypeStatus.Ok Then

                _estado.ObjectReturned = items_

                _estado.SetOK()

            Else

                _estado.SetError(Me, "No se pueden procesar las facturas recibidas.")

            End If

        Else

            _estado.SetError(Me, "La factura comercial que se recibio esta vacía y en borrador.")

        End If

        Return _estado

    End Function

    Private Function ProcesarEntradas(ByVal agrupacionSeleccionada_ As IGeneradorPartidasPedimento.TipoAgrupaciones) As InformacionAgrupacion

        _informacionAgrupacion = New InformacionAgrupacion With {
            .tipooperacion = _pedimentoDocumento.Attribute(CA_TIPO_OPERACION).Valor,
            ._id = ObjectId.GenerateNewId(),
            ._idpedimento = _idPedimentoModulo, 'Actualmente no se tiene valor en el campo y se decidio traerlo antes por si aplica 
            .agrupacionseleccionada = agrupacionSeleccionada_,
            .fechatipocambio = IIf(_informacionAgrupacion.tipooperacion = 1, _pedimentoDocumento.Attribute(CA_FECHA_ENTRADA).Valor, _pedimentoDocumento.Attribute(CA_FECHA_PRESENTACION).Valor),
            .tipocambio = _pedimentoDocumento.Attribute(CA_TIPO_CAMBIO).Valor,
            .fechageneracionagrupacion = Date.Now,
            .pedimento = New DatosPedimento With {
                .numeropedimentocompleto = _pedimentoDocumento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor,
                .aduanaseccion = _pedimentoDocumento.Attribute(CA_CLAVE_SAD).Valor,
                .anio = _pedimentoDocumento.Attribute(CA_ANIO_VALIDACION).ValorPresentacion,
                .patente = _pedimentoDocumento.Attribute(CA_PATENTE).Valor,
                .cvepedimento = _pedimentoDocumento.Attribute(CA_CVE_PEDIMENTO).Valor,
                .regimen = _pedimentoDocumento.Attribute(CA_REGIMEN).Valor,
                .fechaentrada = IIf(_informacionAgrupacion.tipooperacion = 1, _pedimentoDocumento.Attribute(CA_FECHA_ENTRADA).Valor, Nothing),
                .fechapresentacion = IIf(_informacionAgrupacion.tipooperacion = 2, _pedimentoDocumento.Attribute(CA_FECHA_PRESENTACION).Valor, Nothing)
            },
            .archivado = False,
            .estado = 1
        }
        '.numeropedimentocompleto = CA_NUM_PEDIMENTO_COMPLETO 'Actualmente no se tiene valor se debe implementar
        '.anio = 'Actualmente no se tiene campo para el propio año se debera extraer del campo CA_NUM_PEDIMENTO_COMPLETO

        Return _informacionAgrupacion

    End Function

    Private Sub ProcesarFacturas(itemsAsociados_ As List(Of ItemPartida))

        Dim listaFacturas_ = New List(Of DocumentoElectronico)
        Dim valoresFacturas_ As TagWatcher
        Dim index_ = 1
        Dim fechaConsiderar_ As Date

        If itemsAsociados_ IsNot Nothing Then

            fechaConsiderar_ = _informacionAgrupacion.fechatipocambio

            Dim listaObjetcFactura_ = From items_ In itemsAsociados_
                                      Select items_._idfacturacomercial Distinct

            Dim controladorFacturaComercial_ As IControladorFacturaComercial = New ControladorFacturaComercial(listaObjetcFactura_.ToList, IControladorFacturaComercial.Modalidades.Externo, _entorno)

            If controladorFacturaComercial_.Estado.Status = TagWatcher.TypeStatus.Ok Then

                If controladorFacturaComercial_.FacturasComerciales.Count > 0 Then

                    valoresFacturas_ = controladorFacturaComercial_.ConsultaValorDolaresFactura(fechaConsiderar_)

                    If valoresFacturas_.Status = TagWatcher.TypeStatus.Ok Then

                        Dim valorDolaresFacturas_ = valoresFacturas_.ObjectReturned

                        _informacionAgrupacion.recursossasociadas = New List(Of DocumentoAsociado)

                        controladorFacturaComercial_.ModalidadTrabajo = IControladorFacturaComercial.Modalidades.Interno

                        'Regresa la suma de todos los incrementables sin importar el número de facturas por lo que solo se tomara el primero
                        'Cuando se corrija se debe mover aquí
                        Dim incrementables_ = controladorFacturaComercial_.TotalIncrementables(fechaConsiderar_).ObjectReturned

                        If controladorFacturaComercial_.Estado.Status = TagWatcher.TypeStatus.Ok Then

                            Dim _statements As SaxStatements = SaxStatements.GetInstance()
                            Dim root_ As root = _statements.GetLinkedResourceInfo("ConstructorFacturaComercial", "project")
                            Dim recurso_ = From b_ In root_.linkedresources
                                           Where b_.info = "Facturas comerciales"
                                           Select b_.name

                            For Each facturaComercial_ As ConstructorFacturaComercial In controladorFacturaComercial_.FacturasComerciales

                                Dim facturaAgregada_ As New DocumentoAsociado With {
                                    ._iddocumentoasociado = New ObjectId(facturaComercial_.Id),
                                    .identificadorrecurso = recurso_.ToList(0),
                                    .idcoleccion = root_.collection,
                                    .analisisconsistencia = 1
                                }

                                'facturaAgregada_.firmadigital = factura_.FirmaDigital, No se tiene implementado.

                                Dim valorDolarFactura_ = valorDolaresFacturas_(facturaAgregada_._iddocumentoasociado)

                                facturaAgregada_.metadatos = New InformacionAdicionalFactura With {
                                    .fechafactormoneda = fechaConsiderar_,
                                    .foliodocumento = facturaComercial_.FolioDocumento,
                                    .clavemoneda = valorDolarFactura_("monedaFactura_"),
                                    .factormoneda = valorDolarFactura_("factorMonedaFactura_"),
                                    .totalincrementables = IIf(index_ = 1, incrementables_(facturaAgregada_._iddocumentoasociado), 0),
                                    .valorfactura = valorDolarFactura_("valorFactura_")
                                }

                                _informacionAgrupacion.recursossasociadas.Add(facturaAgregada_)

                                index_ += 1

                            Next

                        Else

                            _estado.SetError("ControladorFacturaComercial", controladorFacturaComercial_.Estado.ErrorDescription)

                        End If

                    Else

                        _estado.SetError("ControladorFacturaComercial", controladorFacturaComercial_.Estado.LastMessage)

                    End If

                Else

                    _estado.SetError("ControladorFacturaComercial", controladorFacturaComercial_.Estado.ErrorDescription)

                End If

            Else

                _estado.SetError("ControladorFacturaComercial", controladorFacturaComercial_.Estado.ErrorDescription)

            End If

        End If

    End Sub

    Private Sub ProcesarCalculos(ByRef listaPartidas_ As List(Of IPartidaPedimento),
                                 ByVal facturasAsociadas_ As List(Of DocumentoAsociado),
                                 ByVal itemsAsociados_ As List(Of ItemPartida))

        Dim indice_ As Integer = 1
        Dim items_ As IEnumerable(Of ItemPartida)
        Dim sumaUMC_ As Double = 0
        Dim sumaUMT_ As Double = 0
        Dim sumaVA_ As Double = 0
        Dim precioPagado_ As Double = 0
        Dim sumaMercancia_ As Double = 0
        Dim factormonedaFC_ As Double = 0
        Dim datosfraccion As New hsCode

        If itemsAsociados_ IsNot Nothing Then

            If listaPartidas_ IsNot Nothing Then

                For Each partida_ In listaPartidas_

                    'Items asociados a la partida.
                    items_ = From item_ In itemsAsociados_
                             Order By item_.secuenciaitempartida
                             Where item_.secuenciapartidapedimento = indice_

                    'Sumar la CANTIDAD UMC y CANTIDAD UMT.

                    sumaUMC_ = Aggregate asociados_ In items_
                                        Into Sum(asociados_.cantidadcomercial)

                    sumaUMT_ = Aggregate asociados_ In items_
                                        Into Sum(asociados_.cantidadtarifa)

                    If sumaUMC_ > 0 And sumaUMT_ > 0 Then

                        'Asignar la suma de las cantidades.
                        partida_.CantidadUMC = sumaUMC_
                        partida_.CantidadUMT = sumaUMT_

                        _estado.SetOK()

                    Else

                        _estado.SetError(Me, "Ha ocurrido un detalle: No se calcularón las cantidades correctamente.")

                    End If

                    'PRECIO PAGADO: (Valor mercancía * FACTOR_MONEDA <por factura>)) * TIPO_CAMBIO
                    'Extraer solo las facturas involucradas.
                    Dim facturas_ As IEnumerable(Of Object)

                    If facturasAsociadas_.Count > 1 Then

                        facturas_ = From item_ In items_, fact_ In facturasAsociadas_
                                    Where item_._idfacturacomercial = fact_._iddocumentoasociado
                                    Select fact_._iddocumentoasociado, fact_.metadatos
                    Else

                        facturas_ = From fact_ In facturasAsociadas_
                                    Select fact_._iddocumentoasociado, fact_.metadatos

                    End If

                    Dim sumaTotalPrecioPagado_ As Double = 0

                    If facturas_ IsNot Nothing And facturas_.Count > 0 Then

                        For Each factura_ In facturas_

                            'Extraer la suma del valor de los items asociados que pertenecen a esa factura.
                            sumaMercancia_ = Aggregate asociados_ In items_
                                                 Where asociados_._idfacturacomercial = factura_._iddocumentoasociado
                                                     Into Sum(asociados_.valormercancia)

                            Dim informacionAdicionalFactura_ = From fac_ In facturasAsociadas_
                                                               Where fac_._iddocumentoasociado = factura_._iddocumentoasociado
                                                               Select fac_.metadatos Distinct

                            Dim informacionFC_ As InformacionAdicionalFactura = informacionAdicionalFactura_.ToList(0)

                            'Obtener el valor del factor moneda de la factura y extraerlo para que respete el tipo Double.
                            factormonedaFC_ = informacionFC_.factormoneda

                            'Se hace el calculo de la suma del valor por el factor moneda por el tipo de cambio.
                            precioPagado_ = (sumaMercancia_ * factormonedaFC_) * _informacionAgrupacion.tipocambio

                            'Se suma al valor anterior si es el primero será 0.
                            sumaTotalPrecioPagado_ = precioPagado_ + sumaTotalPrecioPagado_

                        Next

                        If sumaTotalPrecioPagado_ > 0 Then

                            'Se asigna el valor final del precio pagado a la partida.
                            partida_.ImportePrecioPagado = sumaTotalPrecioPagado_
                            _estado.SetOK()

                        Else

                            _estado.SetError(Me, "Ha ocurrido un detalle: No se obtuvo el precio pagado correctamente.")

                        End If

                    End If

                    'PRECIO UNITARIO | PrecioPagado / cantidadUMC |
                    'Si se considera en la malla entonces no debe calcularse pero sino si.
                    'Se debe validar que sea Precio estimado (Cuenta aduanera) o de referencia (Cuota compensatoria) para no aplicar la formula.
                    If _tieneCuentasAduaneras = False And _tieneCuotaCompensatoria = False Then

                        partida_.PrecioUnitario = sumaTotalPrecioPagado_ / partida_.CantidadUMC

                    Else

                        partida_.PrecioUnitario = items_(0).preciounitario

                    End If

                    If _informacionAgrupacion.tipooperacion = 2 Then

                        'Exportaciones
                        'VALOR COMERCIAL | Se tiene fórmula: (Precio pagado part. / TIPO_CAMBIO)|
                        Dim valorComercial_ = sumaTotalPrecioPagado_ / _informacionAgrupacion.tipocambio

                        If valorComercial_ > 0 Then

                            partida_.ValorComercial = valorComercial_
                            _estado.SetOK()

                        Else

                            _estado.SetError(Me, "Ha ocurrido un detalle: No se obtuvo el valor comercial correctamente.")

                        End If

                        'Suma del Valor agregado
                        If _esIMMEX = True And _esRetorno = True Then

                            sumaVA_ = Aggregate asociados_ In items_
                                        Into Sum(asociados_.valoragregado)

                            If sumaVA_ > 0 Then

                                partida_.ValorAgregado = sumaVA_

                            Else

                                _estado.SetOKInfo(Me, "Es apta para valor agregado pero no tiene.")

                            End If

                        End If

                    ElseIf _informacionAgrupacion.tipooperacion = 1 Then

                        'Importaciones
                        Dim sumaIncrementables_ As Double = 0
                        Dim sumaValorTotal_ As Double = 0

                        'Extraemos la información adicional de las facturas relacionadas
                        Dim informacionAdicionalFacturas_ = From fac_ In facturasAsociadas_
                                                            Select fac_.metadatos Distinct

                        For Each fact_ As InformacionAdicionalFactura In informacionAdicionalFacturas_

                            sumaIncrementables_ += fact_.totalincrementables
                            sumaValorTotal_ += fact_.valorfactura

                        Next

                        If sumaValorTotal_ > 0 Then

                            If listaPartidas_.Count > 1 Then
                                'Que pasa sino se tienen incrementables revisar 
                                If sumaIncrementables_ > 0 Then

                                    partida_.ValorAduanal = partida_.ImportePrecioPagado * sumaIncrementables_ / sumaValorTotal_
                                    partida_.ValorAduanal += partida_.ImportePrecioPagado

                                Else

                                    partida_.ValorAduanal = partida_.ImportePrecioPagado * _informacionAgrupacion.tipocambio

                                End If

                                _estado.SetOK()

                            ElseIf listaPartidas_.Count = 1 Then

                                If sumaIncrementables_ > 0 Then

                                    partida_.ValorAduanal = partida_.ImportePrecioPagado + sumaIncrementables_

                                Else

                                    partida_.ValorAduanal = sumaValorTotal_ * _informacionAgrupacion.tipocambio

                                End If

                                _estado.SetOK()

                            End If

                        Else

                            _estado.SetError(Me, "Ha ocurrido un detalle: No se obtuvo el valor aduanal correctamente.")

                        End If

                        'Si tiene precios estimados o precios de referencia se debe sacar el precio unitario en USD 
                        'Por sugerencia de Juridico (Jovana) se saca con el valor aduanal entre el tipo de cambio (recordando que aquí ya se incluyen incrementables)
                        If _tieneCuentasAduaneras = True Or _tieneCuotaCompensatoria = True Then

                            partida_.PrecioUnitarioUSD = partida_.ValorAduanal / _informacionAgrupacion.tipocambio

                        End If

                    End If

                    'Vamos a intentar consultar una fracción por país y fecha
                    Dim fraccionArancelaria_ = ObtenerDatosFraccionArancelaria(partida_.FraccionArancelaria + partida_.Nico, partida_.PaisOrigen)

                    If fraccionArancelaria_.Status = TagWatcher.TypeStatus.Ok Then

                        datosfraccion = fraccionArancelaria_.ObjectReturned

                        Dim datosFA_ = ProcesarDatosFraccionArancelaria(datosfraccion, partida_)

                        'Miau

                        If datosFA_.Status = TagWatcher.TypeStatus.Ok Then

                            _estado.SetOK()

                        ElseIf datosFA_.Status = TagWatcher.TypeStatus.OkInfo Then

                            _estado.SetOKInfo("ControladorTIGIE", "Se reviso todo sin detalle, pero no se tiene toda la información.")

                        Else

                            _estado.SetError("ControladorTIGIE", "Ha ocurrido un detalle: No se procesarón los datos de la fracción arancelaria.")

                        End If

                    Else

                        _estado.SetError("ControladorTIGIE", "Ha ocurrido un detalle: No se obtuvo la información de la fracción arancelaria " + partida_.FraccionArancelaria + partida_.Nico + " correctamente.")

                    End If

                    indice_ += 1

                Next

                If _estado.Status = TagWatcher.TypeStatus.Ok Or _estado.Status = TagWatcher.TypeStatus.OkInfo Then

                    _estado.SetOK()

                End If

                'Obtener valores generales de valor aduana, precio pagado y valor dolares.
                If _estado.Status = TagWatcher.TypeStatus.Ok Then

                    'VALOR ADUANA GENERAL: SUMAR ( (N) (MONTO_USD * TIPO_CAMBIO) ) + ( INCREMENTABLES ) - ( DECREMENTABLES )
                    'PRECIO PAGADO GENERAL: SUMAR ( PRECIO_PAGADO_PARTIDA ) o SUMAR ( N ) ( VALOR_DOLARES * TIPO_CAMBIO )
                    Dim montoDolares As Double = 0
                    Dim sumaPrecioPagado_ As Double = 0

                    'Se obtiene el total del precio pagado de todas las partidas
                    sumaPrecioPagado_ = Aggregate partidas In listaPartidas_
                                Into Sum(partidas.ImportePrecioPagado)

                    If _informacionAgrupacion.tipooperacion = 1 Then

                        'Importaciones

                        Dim sumaValorAduanal_ = Aggregate partidas In listaPartidas_
                                        Into Sum(partidas.ValorAduanal)

                        montoDolares = sumaValorAduanal_ * _informacionAgrupacion.tipocambio

                    ElseIf _informacionAgrupacion.tipooperacion = 2 Then

                        'Exportaciones
                        Dim sumaValorComercial_ = Aggregate partidas In listaPartidas_
                                        Into Sum(partidas.ValorComercial)

                        montoDolares = sumaValorComercial_ * _informacionAgrupacion.tipocambio

                    End If


                ElseIf _estado.Status = TagWatcher.TypeStatus.OkInfo Then

                    _estado.SetOKBut(Me, "Algo sucedio en la información de la Fracción arancelaria.")

                Else

                    _estado.SetError(Me, "Ha ocurrido un detalle: No se tienen los cálculos de las partidas correctamente.")

                End If

            Else

                _estado.SetError(Me, "Ha ocurrido un detalle: No se tienen las partidas completas.")

            End If

        Else

            _estado.SetError(Me, "Ha ocurrido un detalle: No se tienen las asociaciones completas.")

        End If

    End Sub

    Private Function ObtenerDatosFraccionArancelaria(ByVal _fraccionarancelaria As String, ByVal _pais As String) As TagWatcher

        Dim tigie_ As IControladorTIGIE = New ControladorTIGIE()

        Dim fracciones_ = New List(Of String) From {_fraccionarancelaria}

        tigie_.GetHsCode(Of ConstructorTIGIE)(_fraccionarancelaria, IControladorTIGIE.TipoOperacion.Importacion, _pais, DateTime.Now.Date)

        Return tigie_.Estado

    End Function

    Private Function ProcesarDatosFraccionArancelaria(ByVal _fraccionarancelaria As hsCode, ByRef _partidaActual As IPartidaPedimento) As TagWatcher

        If _partidaActual.UnidadMedidaTarifa = _fraccionarancelaria.claveUnidadMedida Then

            _estado.SetOK()

        Else

            _estado.SetError(Me, "Ha ocurrido un detalle: No coindiden las unidades de tarifa.")

        End If

        _partidaActual.ResumenImpuestos = New List(Of ImpuestoPartidaPedimento)
        _partidaActual.Identificadores = New List(Of IdentificadorPartidaPedimento)
        _partidaActual.Permisos = New List(Of PermisoPartidaPedimento)

        'Aquí vamos a pasarle los impuestos
        If _fraccionarancelaria.impuestos.Count > 0 Then

            Dim index_ = 1

            For Each _impuestorequerido In _fraccionarancelaria.impuestos

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                .IdImpuesto = index_,
                .DescripcionContribucion = _impuestorequerido.impuesto,
                .TipoTasa = _impuestorequerido.tipoTasa,
                .Tasa = _impuestorequerido.tasa
            }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron impuestos.")

        End If

        'Comenzamos a revisar las regulaciones arancelarias
        If _fraccionarancelaria.regulacionesArancelarias.tratadosComerciales.Count > 0 Then

            Dim index_ = 1

            For Each _tratadosdisponibles In _fraccionarancelaria.regulacionesArancelarias.tratadosComerciales

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _tratadosdisponibles.tratado,
                    .Tasa = _tratadosdisponibles.tasa,
                    .TipoTasa = _tratadosdisponibles.tipoTasa,
                    .Archivado = 0,
                    .Estado = 1
                }

                Dim _identificadorposible As New IdentificadorPartidaPedimento With {
                    .Identificador = _tratadosdisponibles.identificador,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)
                _partidaActual.Identificadores.Add(_identificadorposible)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron tratados comerciales.")

        End If

        If _fraccionarancelaria.regulacionesArancelarias.aladis.Count > 0 Then

            Dim index_ = 1

            For Each _aldisdisponibles In _fraccionarancelaria.regulacionesArancelarias.aladis

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _aldisdisponibles.aladi,
                    .Tasa = _aldisdisponibles.descuento,
                    .TipoTasa = _aldisdisponibles.identificador,
                    .Archivado = 0,
                    .Estado = 1
                }

                Dim _identificadorposible As New IdentificadorPartidaPedimento With {
                    .Identificador = _aldisdisponibles.identificador,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)
                _partidaActual.Identificadores.Add(_identificadorposible)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron aladis.")

        End If

        If _fraccionarancelaria.regulacionesArancelarias.cuposArancel.Count > 0 Then

            Dim index_ = 1

            For Each _cuposaranceldisponible In _fraccionarancelaria.regulacionesArancelarias.cuposArancel

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _cuposaranceldisponible.pais,
                    .Tasa = _cuposaranceldisponible.arancel,
                    .TipoTasa = _cuposaranceldisponible.arancelFuera,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron cupos arancel.")

        End If

        If _fraccionarancelaria.regulacionesArancelarias.ieps.Count > 0 Then

            Dim index_ = 1

            For Each _iepsdisponible In _fraccionarancelaria.regulacionesArancelarias.ieps

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _iepsdisponible.tipo,
                    .Tasa = _iepsdisponible.tasa,
                    .TipoTasa = _iepsdisponible.medida,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron cupos ieps.")

        End If

        If _fraccionarancelaria.regulacionesArancelarias.cuotasCompensatorias.Count > 0 Then

            Dim index_ = 1

            'Para las Unidades medidas lo que se pueda convertir hacerlo y sino indicar que se debe hacer la conversión
            'Considerar que esto se hace por producto así que aunque este requerido puede no aplicar
            'Se van a colocar pero al final el usuario tiene la ultima palabra dependiendo de las características del producto

            For Each _cuotacompensatoriadisponible In _fraccionarancelaria.regulacionesArancelarias.cuotasCompensatorias

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _cuotacompensatoriadisponible.acotacion,
                    .Tasa = _cuotacompensatoriadisponible.cuota,
                    .TipoTasa = _cuotacompensatoriadisponible.tipo,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron cuotas compensatorias.")

        End If

        If _fraccionarancelaria.regulacionesArancelarias.preciosEstimados.Count > 0 Then

            Dim index_ = 1

            For Each _preciosestimadosdisponible In _fraccionarancelaria.regulacionesArancelarias.preciosEstimados

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _preciosestimadosdisponible.descripcion,
                    .Tasa = _preciosestimadosdisponible.precio,
                    .TipoTasa = _preciosestimadosdisponible.unidad,
                    .Archivado = 0,
                    .Estado = 1
                }

                'Comparar el precio y sacar la conversión (dolares)

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron precios estimados.")

        End If

        'Comenzamos a revisar las regulaciones y restricciones no arancelarias
        If _fraccionarancelaria.regulacionesNoArancelarias.anexos.Count > 0 Then

            Dim index_ = 1

            For Each _anexosdisponibles In _fraccionarancelaria.regulacionesNoArancelarias.anexos

                'Dim _permisonuevo As New PermisoPartidaPedimento With {
                '    .IdPermiso = index_,
                '    . = _tratadosdisponibles.tratado,
                '    .Tasa = _tratadosdisponibles.tasa,
                '    .TipoTasa = _tratadosdisponibles.tipoTasa,
                '    .Archivado = 0,
                '    .Estado = 1
                '}

                '_partidaActual.ResumenImpuestos.Add(_impuestonuevo)
                '_partidaActual.Identificadores.Add(_identificadorposible)

                'index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron anexos.")

        End If

        If _fraccionarancelaria.regulacionesNoArancelarias.cuposMinimos.Count > 0 Then

            Dim index_ = 1

            For Each _cuposminimosdisponibles In _fraccionarancelaria.regulacionesNoArancelarias.cuposMinimos

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _cuposminimosdisponibles.descripcion,
                    .Tasa = _cuposminimosdisponibles.cupo,
                    .TipoTasa = _cuposminimosdisponibles.unidad,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron cupos mínimos.")

        End If

        If _fraccionarancelaria.regulacionesNoArancelarias.embargos.Count > 0 Then

            Dim index_ = 1

            For Each _embargosdisponibles In _fraccionarancelaria.regulacionesNoArancelarias.embargos

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _embargosdisponibles.aplicacion,
                    .Tasa = _embargosdisponibles.acotacion,
                    .TipoTasa = _embargosdisponibles.mercancia,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron embargos.")

        End If

        If _fraccionarancelaria.regulacionesNoArancelarias.normas.Count > 0 Then

            Dim index_ = 1

            For Each _normasdisponibles In _fraccionarancelaria.regulacionesNoArancelarias.normas

                Dim _permisonuevo As New PermisoPartidaPedimento With {
                    .IdPermiso = index_,
                    .Clave = _normasdisponibles.norma,
                    .Numero = _normasdisponibles.identificadores(0),
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.Permisos.Add(_permisonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron normas.")

        End If

        If _fraccionarancelaria.regulacionesNoArancelarias.permisos.Count > 0 Then

            Dim index_ = 1

            For Each _permisosdisponibles In _fraccionarancelaria.regulacionesNoArancelarias.permisos

                Dim _permisonuevo As New PermisoPartidaPedimento With {
                    .IdPermiso = index_,
                    .Clave = _permisosdisponibles.clave,
                    .Numero = _permisosdisponibles.acotacion,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.Permisos.Add(_permisonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron permisos.")

        End If

        If _fraccionarancelaria.regulacionesNoArancelarias.padronSectorial.Count > 0 Then

            Dim index_ = 1

            For Each _padrondisponibles In _fraccionarancelaria.regulacionesNoArancelarias.padronSectorial

                Dim _impuestonuevo As New ImpuestoPartidaPedimento With {
                    .IdImpuesto = index_,
                    .DescripcionContribucion = _padrondisponibles.anexo,
                    .Tasa = _padrondisponibles.acotacion,
                    .TipoTasa = _padrondisponibles.descripcion,
                    .Archivado = 0,
                    .Estado = 1
                }

                _partidaActual.ResumenImpuestos.Add(_impuestonuevo)

                index_ += 1

            Next

        Else

            _estado.SetOKInfo(Me, "No se detectaron padron sectorial.")

        End If

        Return _estado

    End Function

    Private Sub Reordernar(ByRef partidas_ As List(Of IPartidaPedimento), ByRef itemsAsociados_ As List(Of ItemPartida))

        Dim index_ = 1

        For Each partida_ In partidas_

            Dim asociadas = From asoc_ In itemsAsociados_
                            Where asoc_.fraccionarancelaria = partida_.FraccionArancelaria And asoc_.nico = partida_.Nico

            partida_.SecuenciaPartida = index_

            For Each asociacion_ In asociadas

                asociacion_.secuenciapartidapedimento = index_

            Next

            index_ += 1

        Next

        Dim indexAsociado_ = 1

        For Each itemAsociado_ In itemsAsociados_

            itemAsociado_.secuenciaitempartida = indexAsociado_

            indexAsociado_ += 1

        Next

    End Sub

    Private Function AnalisisConsistencia(ByVal _informacionagrupaciones As InformacionAgrupacion) As TagWatcher

        'Primero se debe consultar la fecha de la ultima edición o algo que indique si fue actualizada
        'Se debe hacer con el controlador de factura o en su caso la busqueda general (se dijo que en F&F)

        'Se extrae la información de las facturas fecha de actualización estado y object
        'Esto viene de la información de agrupaciones y se debe usar lo de F&F según comento el inge

        'Se deben comparar y preguntar ¿alguna cambio? ¿Requiere generación?

        'Si se cambia algo entonces se debe regenerar ¿De que forma se regenera?
        'Método de reagrupar (trabajando en eso) mantener lo que más se pueda

        'Luego se debe mandar a hacer la agrupación

        Return _estado

    End Function

    Private Function AnalisisConsistencia(PartidasPedimento As List(Of IPartidaPedimento)) As TagWatcher _
                                                Implements IGeneradorPartidasPedimento.AnalisisConsistencia

        Return _estado

    End Function

    Private Function ICloneable_Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class