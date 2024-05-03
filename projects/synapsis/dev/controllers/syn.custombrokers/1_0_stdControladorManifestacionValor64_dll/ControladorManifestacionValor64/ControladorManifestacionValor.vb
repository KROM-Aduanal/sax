Imports System.IO
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Bson.Serialization.IdGenerators
Imports MongoDB.Driver
Imports MongoDB.Driver.WriteConcern
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.CustomBrokers.Controllers
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesPedimento
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesManifestacionValor
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Syn.Nucleo.Recursos.CamposDomicilio
Imports Syn.Utils
Imports Wma.Exceptions
Imports Syn.Nucleo
Imports System.Web.UI

Public Class ControladorManifestacionValor
    Implements IControladorManifestacionValor, ICloneable, IDisposable

    Private _organismo As Organismo

    Private _controladorFacturaComercial As IControladorFacturaComercial

    Private _entorno As Int32

    Private _enlaceDatos As IEnlaceDatos

#Region "Enums"

#End Region

#Region "Propiedades"

    Public Property ManifestacionesValor As List(Of ConstructorManifestacionValor) Implements IControladorManifestacionValor.ManifestacionesValor
    Public Property Estado As TagWatcher Implements IControladorManifestacionValor.Estado
    Public WriteOnly Property Entorno As Int32 Implements IControladorManifestacionValor.Entorno

        Set(value As Int32)

            _entorno = value

        End Set

    End Property

#End Region

#Region "Constructores"


    Sub New(ByVal entorno_ As Int32)

        Inicializa(entorno_)

    End Sub

    Private Sub Inicializa(ByVal entorno_ As Int32)

        _Estado = New TagWatcher

        _entorno = entorno_ 'Veracruz

        _controladorFacturaComercial = New ControladorFacturaComercial(entorno_, True)

        _ManifestacionesValor = New List(Of ConstructorManifestacionValor)

        _enlaceDatos = New EnlaceDatos

    End Sub

#End Region

#Region "Funciones"

    Public Function Consultar(Of T)(objectId_ As ObjectId) As TagWatcher _
                                                Implements IControladorManifestacionValor.Consultar

        With _Estado

            If _entorno <> 0 Then

                Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(Activator.CreateInstance(Of T)().GetType.Name)

                Dim items_ = operacionesDB_.Aggregate().Match(Function(a) objectId_.Equals(a.Id)).ToList()

                If items_.Count Then

                    Dim item_ = items_(0)

                    Dim manifestacionValor_ = item_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                    .SetOK()

                    .ObjectReturned = DirectCast(CObj(manifestacionValor_), T)

                Else

                    .SetOKBut(Me, "No se encontró la manifestación de valor")

                End If

            Else

                .SetOKBut(Me, "Entorno no puede ser 0")

            End If

            Return _Estado

        End With

    End Function

    Public Function RepresentacionImpresa(objectId_ As ObjectId,
                              Optional tipoDocumento_ As IControladorManifestacionValor.TiposDocumento = IControladorManifestacionValor.TiposDocumento.Ambos) As List(Of String) _
                              Implements IControladorManifestacionValor.RepresentacionImpresa

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(New ConstructorManifestacionValor().GetType.Name)

        Dim items_ = operacionesDB_.Aggregate().Match(Function(a) objectId_.Equals(a.Id)).ToList()

        If items_.Count > 0 Then

            Dim item_ = items_(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Return ConstruirPDF(item_, tipoDocumento_)

        End If



    End Function

    Public Function RepresentacionImpresa(ObjectIds_ As List(Of ObjectId), Optional tipoDocumento_ As IControladorManifestacionValor.TiposDocumento = 1) As List(Of String) _
                                                                                        Implements IControladorManifestacionValor.RepresentacionImpresa

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(New ConstructorManifestacionValor().GetType.Name)

        Dim representacionPDF_ As List(Of String) = New List(Of String)

        operacionesDB_.Aggregate().Match(Function(a) ObjectIds_.Contains(a.Id)).ToList().
            ForEach(Sub(item)

                        item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString

                        ManifestacionesValor.Add(New ConstructorManifestacionValor(True, item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))

                    End Sub)


        For Each manifestacionValor_ As ConstructorManifestacionValor In ManifestacionesValor

            representacionPDF_.AddRange(ConstruirPDF(manifestacionValor_, tipoDocumento_))

        Next

        Return representacionPDF_

    End Function

    Public Function RepresentacionImpresa(pedimentos_ As List(Of String), Optional tipoDocumento_ As IControladorManifestacionValor.TiposDocumento = 1) As List(Of String) _
                                                                                                            Implements IControladorManifestacionValor.RepresentacionImpresa

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(New ConstructorManifestacionValor().GetType.Name)

        Dim representacionPDF_ As List(Of String) = New List(Of String)

        operacionesDB_.Aggregate().Match(Function(a) pedimentos_.Contains(a.FolioOperacion)).ToList().
            ForEach(Sub(item)

                        item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString

                        ManifestacionesValor.Add(New ConstructorManifestacionValor(True, item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))

                    End Sub)


        For Each manifestacionValor_ As ConstructorManifestacionValor In ManifestacionesValor

            representacionPDF_.AddRange(ConstruirPDF(manifestacionValor_, tipoDocumento_))

        Next

        Return representacionPDF_

    End Function

    Public Function RepresentacionImpresa(pedimento_ As String, Optional tipoDocumento_ As IControladorManifestacionValor.TiposDocumento = 1) As List(Of String) _
                                                                                                            Implements IControladorManifestacionValor.RepresentacionImpresa

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(New ConstructorManifestacionValor().GetType.Name)


        Dim items_ = operacionesDB_.Aggregate().Match(Function(a) pedimento_.Equals(a.FolioOperacion)).ToList()

        If items_.Count > 0 Then

            Dim item_ = items_(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Return ConstruirPDF(item_, tipoDocumento_)

        End If

    End Function

    Public Function Generar(objectIdPedimento_ As ObjectId, session_ As IClientSessionHandle) As TagWatcher _
                                                        Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim items_ = operacionesDB_.Aggregate().Match(Function(a) objectIdPedimento_.Equals(a.Id)).ToList()

        If items_.Count Then

            Dim item As OperacionGenerica = items_(0)

            _Estado = TraeDatosFactura(New List(Of ObjectId), item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, session_)

            If _Estado.Status = TagWatcher.TypeStatus.OkBut Then

                _Estado.SetError(Me, "Fallé")

            ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

                _Estado.SetError(Me, "Medio Fallé")

            End If

        End If

        Return _Estado

    End Function

    Public Function Generar(objectIdPedimentos_ As List(Of ObjectId), session_ As IClientSessionHandle) As TagWatcher _
                                                            Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim items_ = operacionesDB_.Aggregate().Match(Function(a) objectIdPedimentos_.Contains(a.Id)).ToList()

        If items_.Count Then

            Dim item As OperacionGenerica = items_(0)

            _Estado = TraeDatosFactura(New List(Of ObjectId), item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, session_)

            If _Estado.Status = TagWatcher.TypeStatus.OkBut Then

                _Estado.SetError(Me, "Fallé")

            ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

                _Estado.SetError(Me, "Medio Fallé")

            End If

        End If

        Return _Estado

    End Function

    Public Function Generar(pedimento_ As String, session_ As IClientSessionHandle) As TagWatcher _
                                        Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim items_ = operacionesDB_.Aggregate().Match(Function(a) pedimento_.Equals(a.FolioOperacion)).ToList()

        If items_.Count Then

            Dim item As OperacionGenerica = items_(0)

            _Estado = TraeDatosFactura(New List(Of ObjectId), item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, session_)

            If _Estado.Status = TagWatcher.TypeStatus.OkBut Then

                _Estado.SetError(Me, "Fallé")

            ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

                _Estado.SetError(Me, "Medio Fallé")

            End If

        End If

        Return _Estado

    End Function

    Public Function Generar(pedimentos_ As List(Of String), session_ As IClientSessionHandle) As TagWatcher _
                                                            Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim items_ = operacionesDB_.Aggregate().Match(Function(a) pedimentos_.Contains(a.FolioOperacion)).ToList()

        If items_.Count Then

            Dim item As OperacionGenerica = items_(0)

            _Estado = TraeDatosFactura(New List(Of ObjectId), item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, session_)

            If _Estado.Status = TagWatcher.TypeStatus.OkBut Then

                _Estado.SetError(Me, "Fallé")

            ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

                _Estado.SetError(Me, "Medio Fallé")

            End If

        End If

        Return _Estado

    End Function

    Private Function Guardar(Of T)(facturasComercial_ As Dictionary(Of ObjectId, List(Of Nodo)),
                             pedimento_ As DocumentoElectronico, session_ As IClientSessionHandle) As TagWatcher

        Dim operacionesGenerica_ = New List(Of OperacionGenerica)

        Dim iterador_ As Integer = 0

        For Each proveedor As String In facturasComercial_.Select(Function(pair) pair.Value(20)).Select(Function(nodo) DirectCast(nodo, Campo).Valor).Distinct()

            _ManifestacionesValor.Add(New ConstructorManifestacionValor())

            For Each _facturaComercial As KeyValuePair(Of ObjectId, List(Of Nodo)) In facturasComercial_.Where(Function(g) DirectCast(g.Value(20), Campo).Valor = proveedor)

                With _ManifestacionesValor(iterador_)

                    .Seccion(SMV4).Attribute(CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO).Valor =
                                                        pedimento_.Seccion(ANS1).Attribute(CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO).Valor

                    .Seccion(SMV4).Attribute(CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL).Valor =
                                                        pedimento_.Seccion(ANS14).Attribute(CamposPedimento.CA_FECHA_ENTRADA).Valor

                    .FolioDocumento = "PRUEBA" + iterador_.ToString()

                    .FolioOperacion = "PRUEBA FOLIO OP"

                    .NombreCliente = DirectCast(_facturaComercial.Value(0), Campo).Valor

                End With

                For Each _campo As Nodo In _facturaComercial.Value

                    With _ManifestacionesValor(iterador_)

                        .Seccion(SMV2) = AsignaValor(.Seccion(SMV2), _campo)

                        .Seccion(SMV3) = AsignaValor(.Seccion(SMV3), _campo)

                        .Seccion(SMV4) = AsignaValor(.Seccion(SMV4), _campo)

                        .Seccion(SMV5) = AcumulaValor(.Seccion(SMV5), _campo)

                    End With

                Next

            Next

            operacionesGenerica_.Add(New OperacionGenerica(_ManifestacionesValor(iterador_)))

            iterador_ = iterador_ + 1

        Next

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
                {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(Activator.CreateInstance(Of T)().GetType.Name)

                Dim result_ = operationsDB_.InsertManyAsync(session_, operacionesGenerica_)

                If Not result_.IsFaulted Then

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se pudo guardar la información")

                End If

            End Using

        End With

        Return _Estado

    End Function

    Private Function TraeDatosFactura(FacturasIds_ As List(Of ObjectId), pedimento_ As DocumentoElectronico, session_ As IClientSessionHandle) As TagWatcher

        FacturasIds_ = New List(Of ObjectId) From {New ObjectId("64e7cc2b4c203fa0dcb2124a"),
                                                       New ObjectId("64e7cead4c203fa0dcb2124b")}

        _Estado = _controladorFacturaComercial.ListaCamposFacturaComercial(FacturasIds_,
                                                            New Dictionary(Of [Enum], List(Of [Enum])) From {{SeccionesFacturaComercial.SFAC1,
                                                            New List(Of [Enum]) From {CA_RAZON_SOCIAL, CA_TAX_ID, CA_RFC_CLIENTE,
                                                                                        Recursos.CamposDomicilio.CA_DOMICILIO_FISCAL,
                                                                                        CA_CALLE, CA_NUMERO_EXTERIOR, CA_NUMERO_INTERIOR, CA_CODIGO_POSTAL,
                                                                                        CA_COLONIA, CA_LOCALIDAD, CA_CIUDAD, CA_MUNICIPIO,
                                                                                        CA_CVE_ENTIDAD_FEDERATIVA, CA_ENTIDAD_FEDERATIVA,
                                                                                        CA_CVE_PAIS, CA_PAIS, CA_NUMERO_FACTURA, CA_FECHA_FACTURA,
                                                                                        CA_CVE_PAIS_FACTURACION, CA_PAIS_FACTURACION}},
                                                            {SeccionesFacturaComercial.SFAC2,
                                                            New List(Of [Enum]) From {CA_RAZON_SOCIAL_PROVEEDOR, CA_TAX_ID_PROVEEDOR, CA_RFC_PROVEEDOR,
                                                                                        Recursos.CamposDomicilio.CA_DOMICILIO_FISCAL, CA_CALLE, CA_NUMERO_EXTERIOR,
                                                                                        CA_NUMERO_INTERIOR, CA_NUMERO_EXT_INT, CA_CODIGO_POSTAL, CA_COLONIA,
                                                                                        CA_LOCALIDAD, CA_CIUDAD, CA_MUNICIPIO, CA_CVE_ENTIDAD_FEDERATIVA,
                                                                                        CA_ENTIDAD_FEDERATIVA, CA_CVE_PAIS, CA_PAIS, CP_CVE_METODO_VALORACION,
                                                                                        RecursosComercioExterior.CamposFacturaComercial.CA_CVE_VINCULACION}},
                                                            {SeccionesFacturaComercial.SFAC5,
                                                            New List(Of [Enum]) From {CA_FLETES, CA_MONEDA_FLETES, CA_SEGURO, CA_MONEDA_SEGUROS,
                                                                                        CA_EMBALAJES, CA_MONEDA_EMBALAJES, CA_OTROS_INCREMENTABLES,
                                                                                        CA_MONEDA_OTROS_INCREMENTABLES, CA_DESCUENTOS, CA_MONEDA_DESCUENTOS}}})

        If _Estado.Status = TagWatcher.TypeStatus.Ok Then

            Dim _facturasComercial As Dictionary(Of ObjectId, List(Of Nodo)) = _Estado.ObjectReturned

            _Estado = Guardar(Of ConstructorManifestacionValor)(_facturasComercial, pedimento_, session_)

        End If

        Return _Estado

    End Function

    Private Function ConstruirPDF(manifestacionValor_ As DocumentoElectronico,
                                  tipoDocumento_ As IControladorManifestacionValor.TiposDocumento) As List(Of String)

        Select Case tipoDocumento_

            Case IControladorManifestacionValor.TiposDocumento.Ambos

                Return New List(Of String) From {New ConstructorManifestacionValorPDF().ImprimirEncabezadoMV(manifestacionValor_),
                       New ConstructorManifestacionValorPDF().ImprimirEncabezadoHC(manifestacionValor_)}

            Case IControladorManifestacionValor.TiposDocumento.MV

                Return New List(Of String) From {New ConstructorManifestacionValorPDF().ImprimirEncabezadoMV(manifestacionValor_)}

            Case IControladorManifestacionValor.TiposDocumento.HC

                Return New List(Of String) From {New ConstructorManifestacionValorPDF().ImprimirEncabezadoHC(manifestacionValor_)}

        End Select

    End Function

    Private Function AsignaValor(seccion_ As Seccion, nodo_ As Nodo) As Seccion

        If seccion_.Attribute(DirectCast(nodo_, Campo).IDUnico) IsNot Nothing Then

            seccion_.Attribute(DirectCast(nodo_, Campo).IDUnico).Valor = DirectCast(nodo_, Campo).Valor

        End If

        Return seccion_

    End Function

    Private Function AcumulaValor(seccion_ As Seccion, nodo_ As Nodo) As Seccion

        If seccion_.Attribute(DirectCast(nodo_, Campo).IDUnico) IsNot Nothing Then

            Select Case DirectCast(nodo_, Campo).IDUnico

                Case CA_FLETES, CA_SEGURO, CA_EMBALAJES, CA_OTROS_INCREMENTABLES, CA_DESCUENTOS

                    seccion_.Attribute(DirectCast(nodo_, Campo).IDUnico).Valor += IIf(DirectCast(nodo_, Campo).Valor Is Nothing, 0, DirectCast(nodo_, Campo).Valor)

                Case Else

                    seccion_.Attribute(DirectCast(nodo_, Campo).IDUnico).Valor = DirectCast(nodo_, Campo).Valor

            End Select

        End If

        Return seccion_

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

#End Region

End Class


