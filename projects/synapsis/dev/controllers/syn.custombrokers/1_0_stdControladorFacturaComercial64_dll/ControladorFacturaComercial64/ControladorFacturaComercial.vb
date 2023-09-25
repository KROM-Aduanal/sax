Imports System.IO
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesFacturaComercial
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Wma.Exceptions
Imports Syn.CustomBrokers.Controllers.IControladorFacturaComercial.Disponibilidades
Imports Syn.CustomBrokers.Controllers.IControladorFacturaComercial.Modalidades
Imports Rec.Globals.Controllers
Imports Syn.Utils
Imports System.Collections.Specialized.BitVector32

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class ControladorFacturaComercial
    Implements IControladorFacturaComercial, ICloneable, IDisposable

#Region "Atributos"

    Private disposedValue As Boolean

    Private _listaFacturas As List(Of ConstructorFacturaComercial)

    Private _listaObjetos As List(Of ObjectId)

    Private _listaFolios As List(Of String)

    Private _listadoCampos As Dictionary(Of ObjectId, Object)

    Private _listadoValoresObject As Dictionary(Of ObjectId, List(Of Nodo))

    Private _listadoValoresString As Dictionary(Of String, List(Of Nodo))

    Private _diccionarioValores As Dictionary(Of ObjectId, Dictionary(Of String, Object))

    Private _diccionarioValoresString As Dictionary(Of String, Dictionary(Of String, List(Of Nodo)))

    Private _listado As List(Of Object)

    Private _listaFactoresMoneda As Dictionary(Of String, Double)

    Private _totalValorIncrementables As Double = 0

    Private _totalValorFletes As Double = 0

    Private _totalValorSeguros As Double = 0

    Private _totalValorEmbalajes As Double = 0

    Private _totalValorOtrosIncrementables As Double = 0

    Private _totalValorDescuentos As Double = 0

    Private _iControladorMonedas As IControladorMonedas

    Private _rOrganismo As Organismo

    Private _numeroAcuseValor As String

    Private _fechaAcuseValor? As Date

    Private _entorno As Int32

#End Region

#Region "Propiedades"
    Public Property FacturasComerciales As List(Of ConstructorFacturaComercial) _
        Implements IControladorFacturaComercial.FacturasComerciales

    Public ReadOnly Property Documento As DocumentoElectronico _
        Implements IControladorFacturaComercial.Documento

    Public ReadOnly Property Documentos As List(Of DocumentoElectronico) _
        Implements IControladorFacturaComercial.Documentos

    Public Property Estado As TagWatcher _
        Implements IControladorFacturaComercial.Estado

    Public Property ModalidadTrabajo As IControladorFacturaComercial.Modalidades _
        Implements IControladorFacturaComercial.ModalidadTrabajo

    Public Property ConservarFacturas As Boolean _
        Implements IControladorFacturaComercial.ConservarFacturas

    Public Property Entorno As Integer _
        Implements IControladorFacturaComercial.Entorno

        Get

            Return _entorno

        End Get

        Set(value As Integer)

            _entorno = value

            ReiniciarControlador(_entorno)

        End Set

    End Property

    Public Property DisponibilidadRecurso As IControladorFacturaComercial.Disponibilidades _
        Implements IControladorFacturaComercial.DisponibilidadRecurso

    Public ReadOnly Property FactorConfiabilidadIA As Double _
        Implements IControladorFacturaComercial.FactorConfiabilidadIA

#End Region

#Region "Constructores"
    Sub New(ByVal entorno_ As Integer,
        ByVal conservarFacturas_ As Boolean)

        _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        Inicializa(Nothing,
               Externo,
               conservarFacturas_,
               entorno_,
               SinDefinir)

    End Sub

    Sub New(ByVal facturasComerciales_ As ConstructorFacturaComercial,
            ByVal entorno_ As Integer)

        _FacturasComerciales = New List(Of ConstructorFacturaComercial) From {facturasComerciales_}

        Inicializa(Nothing,
               Interno,
               True,
               entorno_,
               SinDefinir)

    End Sub

    Sub New(ByVal facturasComerciales_ As List(Of ConstructorFacturaComercial),
        ByVal entorno_ As Integer)

        _FacturasComerciales = facturasComerciales_

        Inicializa(Nothing,
               Interno,
               True,
               entorno_,
               SinDefinir)

    End Sub

    Sub New(ByVal idFactura_ As ObjectId,
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True)

        Inicializa(idFactura_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   SinDefinir)

    End Sub

    Sub New(ByVal folioFactura_ As String,
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True)

        Inicializa(folioFactura_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   SinDefinir)

    End Sub

    Sub New(ByVal idsFacturas_ As List(Of ObjectId),
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True)

        Inicializa(idsFacturas_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   SinDefinir)

    End Sub


    Sub New(ByVal foliosFacturas_ As List(Of String),
            ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
            ByVal entorno_ As Integer,
            Optional conservarFacturas_ As Boolean = True)

        Inicializa(foliosFacturas_,
                   modalidadTrabajo_,
                   conservarFacturas_,
                   entorno_,
                   SinDefinir)


    End Sub

    Public Sub Inicializa(ByVal factura_ As Object,
                          ByVal modalidadTrabajo_ As IControladorFacturaComercial.Modalidades,
                          ByVal conservarFacturas_ As Boolean,
                          ByVal entorno_ As Integer,
                          ByVal disponibilidadRecurso_ As IControladorFacturaComercial.Disponibilidades)

        _Estado = New TagWatcher

        _ModalidadTrabajo = modalidadTrabajo_

        _ConservarFacturas = conservarFacturas_

        _entorno = entorno_

        _DisponibilidadRecurso = disponibilidadRecurso_

        If factura_ IsNot Nothing Then

            _Estado = ListaFacturas(factura_)

            If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                _FacturasComerciales = _Estado.ObjectReturned

            Else

                _Estado.SetOKInfo(Me, "No existen facturas cargadas en batería")

                _FacturasComerciales = Nothing

            End If

        End If


        If _FacturasComerciales Is Nothing Then

            _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        End If

    End Sub

#End Region

#Region "Métodos"

    Public Sub ReiniciarControlador(Optional ByVal entorno_ As Integer = 1) _
        Implements IControladorFacturaComercial.ReiniciarControlador

        _FacturasComerciales = New List(Of ConstructorFacturaComercial)

        Inicializa(Nothing,
               IControladorFacturaComercial.Modalidades.Externo,
               True,
               entorno_,
               SinDefinir)


        _listaFacturas = New List(Of ConstructorFacturaComercial)

        _listaObjetos = New List(Of ObjectId)

        _listaFolios = New List(Of String)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        _listadoValoresObject = New Dictionary(Of ObjectId, List(Of Nodo))

        _listadoValoresString = New Dictionary(Of String, List(Of Nodo))

        _diccionarioValores = New Dictionary(Of ObjectId, Dictionary(Of String, Object))

        _diccionarioValoresString = New Dictionary(Of String, Dictionary(Of String, List(Of Nodo)))

        _listado = New List(Of Object)

        _listaFactoresMoneda = New Dictionary(Of String, Double)

        _totalValorIncrementables = 0

        _totalValorFletes = 0

        _totalValorSeguros = 0

        _totalValorEmbalajes = 0

        _totalValorOtrosIncrementables = 0

        _totalValorDescuentos = 0

        _iControladorMonedas = New ControladorMonedas

        _rOrganismo = New Organismo

        _numeroAcuseValor = Nothing

        _fechaAcuseValor = Nothing

    End Sub

    Public Sub CargaFacturas(documentoDigital_ As MemoryStream) _
        Implements IControladorFacturaComercial.CargaFacturas

        _Estado.
            SetError(Me, "Función aún no implementada")

    End Sub

    Public Sub CargaFacturas(documentoDigital_ As List(Of MemoryStream)) _
        Implements IControladorFacturaComercial.CargaFacturas

        _Estado.
            SetError(Me, "Función aún no implementada")

    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
                _listaFacturas = Nothing

                _listaObjetos = Nothing

                _listaFolios = Nothing

                _listadoCampos = Nothing

                _listadoValoresObject = Nothing

                _listadoValoresString = Nothing

                _diccionarioValores = Nothing

                _diccionarioValoresString = Nothing

                _listado = Nothing

                _listaFactoresMoneda = Nothing

                _totalValorIncrementables = Nothing

                _totalValorFletes = Nothing

                _totalValorSeguros = Nothing

                _totalValorEmbalajes = Nothing

                _totalValorOtrosIncrementables = Nothing

                _totalValorDescuentos = Nothing

                _iControladorMonedas.Dispose()

                _rOrganismo.Dispose()

                _numeroAcuseValor = Nothing

                _fechaAcuseValor = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True

        End If

    End Sub

    ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    ' Protected Overrides Sub Finalize()
    '     ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "Funciones privadas"

    Private Function ConsultaExterior(foliosFacturas_ As Object,
                                      tipoConsulta_ As Boolean) As TagWatcher

        With _Estado

            If _FacturasComerciales IsNot Nothing Then

                _FacturasComerciales = IIf(_ConservarFacturas,
                                       _FacturasComerciales,
                                       New List(Of ConstructorFacturaComercial))
            Else

                _FacturasComerciales = New List(Of ConstructorFacturaComercial)

            End If

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
               {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ = iEnlace_.
                                GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).
                                GetType.Name)

                _listaFacturas = New List(Of ConstructorFacturaComercial)

                Dim auxCount As Integer

                If tipoConsulta_ Then

                    Dim folioFactura_ As List(Of ObjectId) = foliosFacturas_

                    auxCount = folioFactura_.Count

                    operationsDB_.
                    Aggregate().
                    Match(Function(a) folioFactura_.Contains(a.Id)).
                    ToList().
                    ForEach(Sub(item)

                                item.
                                Borrador.
                                Folder.
                                ArchivoPrincipal.
                                Dupla.
                                Fuente.
                                Id = item.Id.ToString

                                _listaFacturas.
                                Add(New ConstructorFacturaComercial(True,
                                                                    item.
                                                                    Borrador.
                                                                    Folder.
                                                                    ArchivoPrincipal.
                                                                    Dupla.
                                                                    Fuente))
                            End Sub)
                Else

                    Dim folioFactura_ As List(Of String) = foliosFacturas_

                    auxCount = folioFactura_.Count

                    operationsDB_.
                    Aggregate().
                    Match(Function(a) folioFactura_.Contains(a.
                                                             Borrador.
                                                             Folder.
                                                             ArchivoPrincipal.
                                                             Dupla.
                                                             Fuente.
                                                             FolioDocumento)).
                   ToList().
                   ForEach(Sub(item)

                               item.
                               Borrador.
                               Folder.
                               ArchivoPrincipal.
                               Dupla.
                               Fuente.
                               Id = item.Id.ToString

                               _listaFacturas.
                               Add(New ConstructorFacturaComercial(True,
                                                                    item.
                                                                    Borrador.
                                                                    Folder.
                                                                    ArchivoPrincipal.
                                                                    Dupla.
                                                                    Fuente))
                           End Sub)
                End If


                If _listaFacturas.Count > 0 Then

                    If _listaFacturas.Count = auxCount Then

                        For Each item_ In _listaFacturas

                            _FacturasComerciales.Add(item_)

                        Next

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Facturas comerciales no encontradas")

                    End If

                Else

                    .SetOKBut(Me, "Facturas comerciales no encontradas")

                End If

                .ObjectReturned = _FacturasComerciales

            End Using

            Return _Estado

        End With

    End Function

    Private Function ObtenerFacturas(idFacturas_ As List(Of ObjectId)) _
        As TagWatcher

        With _Estado

            Select Case _ModalidadTrabajo

                Case IControladorFacturaComercial.Modalidades.Interno

                    If Not _FacturasComerciales Is Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            Dim facturas_ = IIf(_ConservarFacturas,
                                             _FacturasComerciales,
                                             New List(Of ConstructorFacturaComercial))

                            For Each item_ In _FacturasComerciales.
                                Where(Function(f) idFacturas_.Contains(New ObjectId(f.Id)))

                                facturas_.Add(item_)

                            Next

                            If facturas_.Count > 0 Then

                                If facturas_.Count = idFacturas_.Count Then

                                    .SetOK()

                                    .ObjectReturned = facturas_

                                Else

                                    .SetError(Me, "No existe factura en la batería actual")

                                End If

                            Else

                                .SetOKBut(Me, "No se encuentran facturas en la batería actual")

                            End If

                        Else

                            .SetOKBut(Me, "No se cargaron las facturas")

                        End If

                    End If

                Case IControladorFacturaComercial.Modalidades.Externo

                    _Estado = ConsultaExterior(idFacturas_, True) 'True es para object

            End Select

            Return _Estado

        End With

    End Function

    Private Function ObtenerFacturas(foliosFacturas_ As List(Of String)) _
                                      As TagWatcher

        With _Estado

            Select Case _ModalidadTrabajo

                Case IControladorFacturaComercial.Modalidades.Interno

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            Dim facturas_ = IIf(_ConservarFacturas,
                                            _FacturasComerciales,
                                            New List(Of ConstructorFacturaComercial))

                            For Each item_ In _FacturasComerciales.
                                                Where(Function(f) foliosFacturas_.
                                                Contains(f.Seccion(SFAC1).
                                                Attribute(CA_NUMERO_FACTURA).
                                                Valor))

                                facturas_.Add(item_)

                            Next

                            If facturas_.Count > 0 Then

                                .SetOK()

                                .ObjectReturned = facturas_

                            Else

                                .SetOKBut(Me, "Ocurrió un problema al cargar las facturas en batería")

                            End If

                        Else

                            .SetOKBut(Me, "No hay facturas disponibles")

                        End If

                    Else

                        .SetOKBut(Me, "No se encuentran facturas en la batería actual")

                    End If

                Case IControladorFacturaComercial.Modalidades.Externo

                    _Estado = ConsultaExterior(foliosFacturas_, False)

            End Select

            Return _Estado

        End With

    End Function

    Private Function ObtenerFactorPorMoneda(ByRef monedaIncrementable_ As String,
                                            ByRef fechaMoneda_ As Date) _
                                            As Dictionary(Of String, Double)

        _iControladorMonedas = New ControladorMonedas

        Dim factorTipoCambio_ = DirectCast(_iControladorMonedas.
                                ObtenerFactorTipodeCambio(monedaIncrementable_,
                                                          fechaMoneda_).ObjectReturned, Object)
        _listaFactoresMoneda = New Dictionary(Of String, Double)

        With _Estado

            If factorTipoCambio_.Count > 0 Then

                If factorTipoCambio_(0) IsNot Nothing And
                   factorTipoCambio_(1) IsNot Nothing Then

                    _listaFactoresMoneda.Add("tipoCambioMoneda_", factorTipoCambio_(1).tipocambio)

                    _listaFactoresMoneda.Add("factorMoneda_", factorTipoCambio_(0).factor)

                Else

                    .SetError(Me, "No se encontró el tipo de cambio")

                End If

            Else

                .SetError(Me, "No se encontró el factor para esta fecha")

            End If

        End With

        Return _listaFactoresMoneda


    End Function

    Private Function CalcularValorIncrementable(ByRef incrementable_ As Double,
                                                ByRef monedaIncrementable_ As String,
                                                ByRef fechaMoneda_ As Date) As Double

        Dim valoresMoneda_ = ObtenerFactorPorMoneda(monedaIncrementable_,
                                                    fechaMoneda_)

        If valoresMoneda_.Count > 0 Then

            incrementable_ *= (valoresMoneda_("tipoCambioMoneda_") *
                               valoresMoneda_("factorMoneda_"))

            incrementable_ = IIf(incrementable_ > 0.0, incrementable_, 0.0)

        Else

            incrementable_ = 0.0

        End If

        Return incrementable_

    End Function

    Private Function ObtenerValorIncrementable(ByRef valorSeccion_ As Double,
                                               ByRef monedaSeccion_ As String,
                                               ByRef fechaMoneda_ As Date) As Double

        Dim valorIncrementable_ As Double

        If valorSeccion_ > 0 And
            monedaSeccion_ IsNot Nothing Then

            valorIncrementable_ = CalcularValorIncrementable(valorSeccion_,
                                                             monedaSeccion_,
                                                             fechaMoneda_)
        Else

            valorIncrementable_ = 0.00

        End If

        Return valorIncrementable_

    End Function

    Private Function ObtenerSeccionPartidas(ByRef listaFacturas_ _
                                            As List(Of ConstructorFacturaComercial)) _
                                            As Dictionary(Of ObjectId, Object)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        For Each factura_ In _listaFacturas

            With factura_.Seccion(SFAC4)

                Dim partida_ As New List(Of Object)

                For Each item_ In .Nodos

                    Dim listaCampos_ As New Dictionary(Of String, Object)


                    For Each i_ In item_.Nodos

                        Dim items_ = DirectCast(i_.Nodos(0), Campo)

                        Dim aux_ As New Dictionary(Of String, Object) _
                                                    From {
                                                        {"Valor", items_.Valor},
                                                        {"ValorPresentacion", items_.ValorPresentacion}
                                                    }

                        listaCampos_.Add(items_.Nombre, aux_)

                    Next

                    partida_.Add(listaCampos_)

                Next

                _listadoCampos.Add(ObjectId.Parse(factura_.Id), partida_)

            End With

        Next

        Return _listadoCampos

    End Function

    Private Function ObtenerSeccionFactura(ByRef listaFacturas_ _
                                           As List(Of ConstructorFacturaComercial),
                                           ByRef seccion_ As Integer) _
                                           As Dictionary(Of ObjectId, Object)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        For Each factura_ In _listaFacturas

            With factura_.Seccion(seccion_)

                Dim listaCampos_ As New Dictionary(Of String, Object)

                For Each item_ In .Nodos

                    Dim items_ = DirectCast(item_.Nodos(0), Campo)

                    Dim aux_ As New Dictionary(Of String, Object) _
                                              From {
                                                {"Valor", items_.Valor},
                                                {"ValorPresentacion", items_.ValorPresentacion}
                                              }

                    listaCampos_.Add(items_.Nombre, aux_)

                Next

                _listadoCampos.Add(ObjectId.Parse(factura_.Id), listaCampos_)

            End With

        Next

        Return _listadoCampos

    End Function

    Private Function ObtenerSeccionFactura(ByRef listaFacturas_ As List(Of ConstructorFacturaComercial),
                                           ByRef seccion_ As Integer,
                                           ByRef listaNodos_ As List(Of Integer)) _
                                           As Dictionary(Of ObjectId, Object)

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        For Each factura_ In _listaFacturas

            With factura_.Seccion(seccion_)

                Dim listaCampos_ As New Dictionary(Of String, Object)

                For Each item_ In listaNodos_

                    Dim aux_ As New Dictionary(Of String, Object) _
                                              From {
                                                {"Valor", .Attribute(item_).Valor},
                                                {"ValorPresentacion", .Attribute(item_).ValorPresentacion}
                                              }

                    listaCampos_.Add(.Attribute(item_).Nombre, aux_)

                Next

                _listadoCampos.Add(ObjectId.Parse(factura_.Id), listaCampos_)

            End With

        Next

        Return _listadoCampos

    End Function

    Private Function SeccionesCampos(ByRef facturaComercial_ As TagWatcher,
                                     ByRef seccion_ As Integer,
                                     Optional listaNodos_ As List(Of Integer) = Nothing) _
                                     As Dictionary(Of ObjectId, Object)


        _listaFacturas = DirectCast(facturaComercial_.ObjectReturned, List(Of ConstructorFacturaComercial))

        _listadoCampos = New Dictionary(Of ObjectId, Object)

        With _Estado

            If seccion_ = 4 Then

                _listadoCampos = ObtenerSeccionPartidas(_listaFacturas)

            ElseIf listaNodos_ IsNot Nothing Then

                _listadoCampos = ObtenerSeccionFactura(_listaFacturas, seccion_, listaNodos_)

            Else

                _listadoCampos = ObtenerSeccionFactura(_listaFacturas, seccion_)

            End If


            If _listadoCampos.Count > 0 Then

                .SetOK()

                _listadoCampos = _listadoCampos

            Else

                .SetError(Me, "Ha ocurrido en el llenado del listado")

            End If

            Return _listadoCampos

        End With

    End Function

    Private Function ObtenerListaIconterms(ByRef _facturasComerciales As TagWatcher) _
        As TagWatcher

        With _Estado

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            _listadoCampos = SeccionesCampos(_facturasComerciales,
                                           SFAC1,
                                           New List(Of Integer) From {CA_CVE_INCOTERM})

            If _listadoCampos.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista") : End If

            .ObjectReturned = _listadoCampos

            Return _Estado

        End With

    End Function

    Private Function ObtenerListaIncrementables(ByVal _facturasComerciales As TagWatcher) _
                                                As TagWatcher

        With _Estado

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            _listadoCampos = SeccionesCampos(_facturasComerciales, SFAC5)

            If _listadoCampos.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista") : End If

            .ObjectReturned = _listadoCampos

            Return _Estado

        End With

    End Function

    Private Function ObtenerTotalIncrementables(ByRef facturasDisponibles_ As TagWatcher,
                                                ByRef fechaMoneda_ As Date) _
                                                As TagWatcher

        With _Estado

            _listaFacturas = DirectCast(facturasDisponibles_.ObjectReturned, List(Of ConstructorFacturaComercial))

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            For Each facturaComercial_ In _listaFacturas

                With facturaComercial_.Seccion(SFAC5)

                    _totalValorSeguros += ObtenerValorIncrementable(.Attribute(CA_SEGURO).Valor,
                                                                    .Attribute(CA_MONEDA_SEGUROS).ValorPresentacion,
                                                                    fechaMoneda_)

                    _totalValorFletes += ObtenerValorIncrementable(.Attribute(CA_FLETES).Valor,
                                                                   .Attribute(CA_MONEDA_FLETES).ValorPresentacion,
                                                                   fechaMoneda_)

                    _totalValorEmbalajes += ObtenerValorIncrementable(.Attribute(CA_EMBALAJES).Valor,
                                                                  .Attribute(CA_MONEDA_EMBALAJES).ValorPresentacion,
                                                                  fechaMoneda_)

                    _totalValorOtrosIncrementables += ObtenerValorIncrementable(.Attribute(CA_OTROS_INCREMENTABLES).Valor,
                                                                           .Attribute(CA_MONEDA_OTROS_INCREMENTABLES).ValorPresentacion,
                                                                           fechaMoneda_)

                    _totalValorDescuentos += ObtenerValorIncrementable(.Attribute(CA_DESCUENTOS).Valor,
                                                                  .Attribute(CA_MONEDA_DESCUENTOS).ValorPresentacion,
                                                                  fechaMoneda_)
                End With

                _totalValorIncrementables = (_totalValorFletes +
                                        _totalValorSeguros +
                                        _totalValorEmbalajes +
                                        _totalValorOtrosIncrementables +
                                        _totalValorDescuentos)

                _listadoCampos.Add(ObjectId.Parse(facturaComercial_.Id), _totalValorIncrementables)

            Next

            If _listadoCampos.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista") : End If

            .ObjectReturned = _listadoCampos

            Return _Estado

        End With

    End Function

    Private Function ObtenerPartidas(ByVal _facturasComerciales As TagWatcher) _
                                 As TagWatcher

        With _Estado

            _listadoCampos = New Dictionary(Of ObjectId, Object)

            _listadoCampos = SeccionesCampos(_facturasComerciales, SFAC4)

            If _listadoCampos.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista") : End If

            .ObjectReturned = _listadoCampos

            Return _Estado

        End With

    End Function

    Private Function ObtenerListaValores(idsFacturas_ As List(Of ObjectId),
                                        seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                        As TagWatcher

        With _Estado

            Try

                _listadoValoresObject = New Dictionary(Of ObjectId, List(Of Nodo))

                Dim diccionarioValoresObjectId As New Dictionary(Of ObjectId, List(Of Nodo))

                _rOrganismo = New Organismo

                _listadoValoresObject = _rOrganismo.ObtenerCamposSeccionExterior(idsFacturas_,
                                                                                New ConstructorFacturaComercial,
                                                                                seccionesCampos_)

                If _listadoValoresObject.Count > 0 Then

                    For Each listaValor_ In _listadoValoresObject

                        diccionarioValoresObjectId.Add(listaValor_.Key, listaValor_.Value)

                    Next

                Else

                    .SetError(Me, "No se encontraron campos en el listado de facturas")

                End If

                If diccionarioValoresObjectId.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de valores") : End If

                .ObjectReturned = diccionarioValoresObjectId


            Catch ex As Exception

                .SetError(Me, ex.Message)

            End Try

            Return _Estado

        End With

    End Function

    Private Function ObtenerListaValores(foliosFacturas_ As List(Of String),
                                         seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                         As TagWatcher

        With _Estado

            Try
                _listadoValoresString = New Dictionary(Of String, List(Of Nodo))

                Dim diccionarioValoresString As New Dictionary(Of String, List(Of Nodo))

                _rOrganismo = New Organismo

                _listadoValoresString = _rOrganismo.ObtenerCamposSeccionExterior(foliosFacturas_,
                                                                  New ConstructorFacturaComercial,
                                                                  seccionesCampos_)

                If _listadoValoresString.Count > 0 Then

                    For Each listaValor_ In _listadoValoresString

                        diccionarioValoresString.Add(listaValor_.Key, listaValor_.Value)

                    Next

                Else

                    .SetError(Me, "No se encontraron campos en el listado de facturas")

                End If

                If diccionarioValoresString.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de valores") : End If

                .ObjectReturned = diccionarioValoresString


            Catch ex As Exception

                .SetError(Me, ex.Message)

            End Try

            Return Estado

        End With

    End Function

    Private Function ObtenerValorDolaresFactura(ByRef facturasDisponibles_ As TagWatcher,
                                                ByRef fechaMoneda_ As Date) _
                                                As TagWatcher

        With _Estado

            _listaFacturas = DirectCast(facturasDisponibles_.ObjectReturned, List(Of ConstructorFacturaComercial))

            _diccionarioValores = New Dictionary(Of ObjectId, Dictionary(Of String, Object))

            For Each facturaComercial_ In _listaFacturas

                With facturaComercial_.Seccion(SFAC1)

                    If .Attribute(CA_MONEDA_FACTURACION).ValorPresentacion IsNot Nothing _
                        And .Attribute(CP_VALOR_FACTURA).Valor IsNot Nothing Then

                        Dim factorMonedaFactura_ = ObtenerFactorPorMoneda(.Attribute(CA_MONEDA_FACTURACION).ValorPresentacion,
                                                                          fechaMoneda_)

                        If factorMonedaFactura_.Count > 0 Then

                            Dim valorDolares_ = .Attribute(CP_VALOR_FACTURA).Valor *
                                               factorMonedaFactura_("factorMoneda_")

                            _diccionarioValores.Add(ObjectId.Parse(facturaComercial_.Id),
                                                        New Dictionary(Of String, Object) _
                                                        From {
                                                            {"valorFactura_", .Attribute(CP_VALOR_FACTURA).Valor},
                                                            {"monedaFactura_", .Attribute(CA_MONEDA_FACTURACION).ValorPresentacion},
                                                            {"factorMonedaFactura_", factorMonedaFactura_("factorMoneda_")},
                                                            {"totalValorDolaresFactura_", valorDolares_}
                                                        })

                        Else

                            _Estado.SetError(Me, "No se encontró el factor moneda")

                            Return _Estado

                        End If

                    Else

                        _Estado.SetError(Me, "Valores no encontrados en la sección actual")

                        Return _Estado

                    End If

                End With

            Next

            If _diccionarioValores.Count > 1 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de valor dólares") : End If

            .ObjectReturned = _diccionarioValores

            Return _Estado

        End With

    End Function

#End Region

#Region "Funciones"

    'Interface
    Public Function ActualizarDatosAcuseValor(idFactura_ As ObjectId,
                                              valoresAcuseValor_ As List(Of String)) As TagWatcher _
                                              Implements IControladorFacturaComercial.ActualizarDatosAcuseValor

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty And
                    Not valoresAcuseValor_.Count = 0 Then

                    If Not String.IsNullOrEmpty(valoresAcuseValor_(0)) And
                   Not String.IsNullOrEmpty(valoresAcuseValor_(1)) Then

                        If IsDate(valoresAcuseValor_(1)) Then

                            _numeroAcuseValor = valoresAcuseValor_(0)

                            _fechaAcuseValor = Convert.
                                           ToDateTime(valoresAcuseValor_(1)).
                                           Date.
                                           ToString("yyyy-MM-dd")

                            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
                                                         {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}


                                Dim operationsDB_ = iEnlace_.
                                                GetMongoCollection(Of OperacionGenerica)((New ConstructorFacturaComercial).
                                                GetType.Name)


                                Dim setStructureOfSubs_ = Builders(Of OperacionGenerica).Update.
                                                      Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.1.Nodos.0.Valor",
                                                                     _numeroAcuseValor).
                                                      Set(Of Date)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.3.Nodos.0.Valor",
                                                                   _fechaAcuseValor)

                                Dim result_ = operationsDB_.UpdateOne(Function(x) x.Id = idFactura_,
                                                                  setStructureOfSubs_)

                                If result_.MatchedCount <> 0 Then

                                    .SetOK()

                                ElseIf result_.UpsertedId IsNot Nothing Then

                                    .SetOK()

                                Else

                                    .SetError(Me, "No se generaron cambios")

                                End If

                            End Using

                        Else

                            .SetOKBut(Me, "Se esperaba una fecha válida")

                        End If

                    Else

                        .SetOKBut(Me, "Se esperaban valores válidos")

                    End If

                Else

                    .SetOKBut(Me, "No se encontraron valores disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaFacturas(idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _listaObjetos = New List(Of ObjectId) From {idFactura_}

                    _Estado = ObtenerFacturas(_listaObjetos)

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaFacturas(idsFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count > 0 Then

                    _Estado = ObtenerFacturas(idsFacturas_)

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaFacturas(folio_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        With _Estado

            If _entorno <> 0 Then

                If folio_ IsNot Nothing Then

                    _Estado = ObtenerFacturas(New List(Of String) From {folio_})

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaFacturas(folios_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaFacturas

        With _Estado

            If _entorno <> 0 Then

                If folios_.Count <> 0 Then

                    _Estado = ObtenerFacturas(folios_)

                Else

                    .SetError(Me, "Facturas no disponibles")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function CargaFacturas(listaFacturas_ As List(Of ConstructorFacturaComercial)) As TagWatcher _
        Implements IControladorFacturaComercial.CargaFacturas

        With _Estado

            .SetOKBut(Me, "Función sin implementar")

            Return _Estado

        End With

    End Function

    Public Function FirmaDigital(Of T)(_id As ObjectId) As String _
        Implements IControladorFacturaComercial.FirmaDigital

        With _Estado

            .SetOKBut(Me, "Función sin implementar")

            Return "Sin firma"

        End With

    End Function

    Public Function FacturaDisponible(idFactura_ As ObjectId) As Boolean _
        Implements IControladorFacturaComercial.FacturaDisponible

        With _Estado

            .SetOKBut(Me, "Función sin implementar")

            Return False

        End With

    End Function

    Public Function FacturaDisponible(folioFactura_ As String) As Boolean _
        Implements IControladorFacturaComercial.FacturaDisponible

        With _Estado

            .SetOKBut(Me, "Función sin implementar")

            Return False

        End With

    End Function

    'Total incrementables
    Function TotalIncrementables(fechaMoneda_ As Date) As TagWatcher _
        Implements IControladorFacturaComercial.TotalIncrementables

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                        Else

                            .SetError(Me, "No se encontraron facturas")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If


                Else

                    .SetOKBut(Me, "Método aplicado solo a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function TotalIncrementables(ByVal idFactura_ As ObjectId,
                                 ByVal fechaMoneda_ As Date) As TagWatcher _
                                 Implements IControladorFacturaComercial.TotalIncrementables

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura sin definir")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function TotalIncrementables(ByVal idsFacturas_ As List(Of ObjectId),
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.TotalIncrementables

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ObtenerFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de ObjectId sin definir")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function TotalIncrementables(ByVal folioFactura_ As String,
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.TotalIncrementables

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura no disponible")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function TotalIncrementables(ByVal foliosFacturas_ As List(Of String),
                             ByVal fechaMoneda_ As Date) As TagWatcher _
                             Implements IControladorFacturaComercial.TotalIncrementables

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_ IsNot Nothing Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerTotalIncrementables(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura no disponible")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    'Lista incrementables
    Function ListaIncrementables() As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncrementables

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerListaIncrementables(_Estado)

                        Else

                            .SetError(Me, "Ha ocurrido un error al obtener la lista de incrementables")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Método aplicado solo a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncrementables(ByVal idFactura_ As ObjectId) As TagWatcher _
                                 Implements IControladorFacturaComercial.ListaIncrementables

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncrementables(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher _
                                Implements IControladorFacturaComercial.ListaIncrementables

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncrementables(ByVal folioFactura_ As String) As TagWatcher _
                                 Implements IControladorFacturaComercial.ListaIncrementables

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncrementables(ByVal foliosFacturas_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncrementables

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_ IsNot Nothing Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        Return ObtenerListaIncrementables(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    'Lista Icoterms
    Function ListaIncoterms() As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerListaIconterms(_Estado)

                        Else

                            .SetError(Me, "Ha ocurrido un error al obtener la lista de incoterms")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Función disponible para modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ObtenerFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de Objectsids de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal folioFactura_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaIncoterms(ByVal foliosFacturas_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaIncoterms

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerListaIconterms(_Estado)

                    Else

                        .SetError(Me, "Lista de facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folios de facturas vacíos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    'Partidas
    Function ListaPartidas() As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 1 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerPartidas(_Estado)

                        Else

                            .SetError(Me, "Ha ocurrido un error al obtener la lista de partidas")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Función disponible para modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaPartidas(ByVal idFactura_ As ObjectId) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura vacío")

                End If


            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If


        End With

        Return _Estado



    End Function

    Function ListaPartidas(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de objectsids de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaPartidas(ByVal folioFactura_ As String) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Function ListaPartidas(ByVal foliosFacturas_ As List(Of String)) As TagWatcher _
        Implements IControladorFacturaComercial.ListaPartidas

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerPartidas(_Estado)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de folios de facturas vacíos")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    'Navegacion
    Function ListaCamposFacturaComercial(ByVal idFactura_ As ObjectId,
                                         seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                         As TagWatcher _
                                         Implements IControladorFacturaComercial.ListaCamposFacturaComercial

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ObtenerListaValores(New List(Of ObjectId) _
                                                  From {idFactura_},
                                                  seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Function ListaCamposFacturaComercial(ByVal idsFacturas_ As List(Of ObjectId),
                                         ByVal seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                         As TagWatcher _
                                         Implements IControladorFacturaComercial.ListaCamposFacturaComercial
        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ObtenerListaValores(idsFacturas_, seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaCamposFacturaComercial(ByVal folioFactura_ As String,
                                                ByVal seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                                As TagWatcher _
                                                Implements IControladorFacturaComercial.ListaCamposFacturaComercial

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ObtenerListaValores(New List(Of String) _
                                                  From {folioFactura_},
                                                  seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ListaCamposFacturaComercial(ByVal foliosFacturas_ As List(Of String),
                                                ByVal seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                                As TagWatcher _
                                                Implements IControladorFacturaComercial.ListaCamposFacturaComercial

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ObtenerListaValores(foliosFacturas_, seccionesCampos_)

                Else

                    .SetOKBut(Me, "Facturas no disponibles en batería")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            'Return _Estado

        End With

    End Function

    'Consulta valor dolares a facturas
    Public Function ConsultaValorDolaresFactura(fechaMoneda_ As Date) As TagWatcher _
        Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        With _Estado

            If _entorno <> 0 Then

                If _ModalidadTrabajo <> 0 Then

                    If _FacturasComerciales IsNot Nothing Then

                        If _FacturasComerciales.Count > 0 Then

                            .ObjectReturned = _FacturasComerciales

                            _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                        Else

                            .SetOKBut(Me, "Facturas no disponibles en batería")

                        End If

                    Else

                        .SetOKBut(Me, "Facturas no disponibles en batería")

                    End If

                Else

                    .SetOKBut(Me, "Función aplicada a modalidad interna")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal idFactura_ As ObjectId,
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        With _Estado

            If _entorno <> 0 Then

                If Not idFactura_ = ObjectId.Empty Then

                    _Estado = ListaFacturas(idFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetError(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "ObjectId de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal idsFacturas_ As List(Of ObjectId),
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        With _Estado

            If _entorno <> 0 Then

                If idsFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(idsFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de ObjectsId de factura vacía")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal folioFactura_ As String,
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        With _Estado

            If _entorno <> 0 Then

                If folioFactura_ IsNot Nothing Then

                    _Estado = ListaFacturas(folioFactura_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Folio de factura vacío")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

        End With

        Return _Estado

    End Function

    Public Function ConsultaValorDolaresFactura(ByVal foliosFacturas_ As List(Of String),
                                                fechaMoneda_ As Date) As TagWatcher _
                                                Implements IControladorFacturaComercial.ConsultaValorDolaresFactura

        With _Estado

            If _entorno <> 0 Then

                If foliosFacturas_.Count() > 0 Then

                    _Estado = ListaFacturas(foliosFacturas_)

                    If _Estado.Status = TagWatcher.TypeStatus.Ok Then

                        _Estado = ObtenerValorDolaresFactura(_Estado, fechaMoneda_)

                    Else

                        .SetOKBut(Me, "Facturas no disponibles")

                    End If

                Else

                    .SetOKBut(Me, "Lista de folios de factura vacía")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function ConsultaPLNFactura(consulta_ As String) As BsonDocument _
    Implements IControladorFacturaComercial.ConsultaPLNFactura
        Throw New NotImplementedException()
    End Function

    Public Function Clone() As Object _
    Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Private Function GetDebuggerDisplay() As String
        Return ToString()
    End Function

#End Region
End Class