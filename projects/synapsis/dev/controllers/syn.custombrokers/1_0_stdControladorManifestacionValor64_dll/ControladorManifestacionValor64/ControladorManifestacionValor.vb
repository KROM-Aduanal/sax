Imports System.IO
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Bson.Serialization.IdGenerators
Imports MongoDB.Driver
Imports MongoDB.Driver.WriteConcern
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.CustomBrokers.Controllers.ControladorFacturaComercial
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

    'Private _manifestacionesValor As List(Of ConstructorManifestacionValor)

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

    Public Sub Inicializa(ByVal entorno_ As Int32)

        _Estado = New TagWatcher

        _entorno = entorno_ 'Veracruz

        _controladorFacturaComercial = New ControladorFacturaComercial(entorno_, True)

        _ManifestacionesValor = New List(Of ConstructorManifestacionValor)

        _enlaceDatos = New EnlaceDatos

    End Sub

#End Region

#Region "Funciones"

    Public Function Consultar(objectid_ As ObjectId) As TagWatcher Implements IControladorManifestacionValor.Consultar
        Throw New NotImplementedException()
    End Function

    Public Function Descargar(objectId_ As ObjectId,
                              Optional tipoDocumento As IControladorManifestacionValor.TiposDocumento = IControladorManifestacionValor.TiposDocumento.Ambos) As FileStream _
                              Implements IControladorManifestacionValor.Descargar
        Throw New NotImplementedException()
    End Function

    Public Function Descargar(ObjectIds_ As List(Of ObjectId), Optional tipoDocumento As IControladorManifestacionValor.TiposDocumento = 1) As FileStream Implements IControladorManifestacionValor.Descargar
        Throw New NotImplementedException()
    End Function

    Public Function Descargar(pedimentos_ As List(Of String), Optional tipoDocumento As IControladorManifestacionValor.TiposDocumento = 1) As FileStream Implements IControladorManifestacionValor.Descargar
        Throw New NotImplementedException()
    End Function

    Public Function Descargar(pedimentos As String, Optional tipoDocumento As IControladorManifestacionValor.TiposDocumento = 1) As FileStream Implements IControladorManifestacionValor.Descargar
        Throw New NotImplementedException()
    End Function

    Public Function DescargarPDF(pedimentos As String, Optional tipoDocumento As IControladorManifestacionValor.TiposDocumento = 1) As List(Of String) Implements IControladorManifestacionValor.DescargarPDF

        Dim manifestacionValor_ As DocumentoElectronico

        Dim generaMV = New ConstructorManifestacionValorPDF()

        Dim generaHC = New ConstructorManifestacionValorPDF()

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(New ConstructorManifestacionValor().GetType.Name)

        Dim filtro_ As BsonDocument = New BsonDocument().Add("_id", New ObjectId("64da6056c841cbc19e5f94ff"))

        Dim items_ = operacionesDB_.Find(filtro_).ToList

        If items_.Count Then

            Dim item_ As OperacionGenerica = items_(0)

            manifestacionValor_ = item_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Return New List(Of String) From {generaHC.ImprimirEncabezadoMV(manifestacionValor_)}

        End If

    End Function
    Public Function Descarga(pedimentos As String, Optional tipoDocumento As IControladorManifestacionValor.TiposDocumento = 1) As List(Of String)
        If pedimentos IsNot Nothing Then
            Dim docElectronico_ = New DocumentoElectronico()
            Dim constructorPedimento = New ConstructorManifestacionValorPDF()

            constructorPedimento.ImprimirEncabezadoMV(docElectronico_)


        End If
    End Function

    Public Function Generar(objectIdPedimento_ As ObjectId) As TagWatcher Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim filtro_ As BsonDocument = New BsonDocument().Add("_id", objectIdPedimento_)

        Dim items = operacionesDB_.Find(filtro_).ToList

        If items.Count Then

            Dim item As OperacionGenerica = items(0)

            Dim _pedimento As DocumentoElectronico = item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Dim _FacturasIds = New List(Of ObjectId) From {New ObjectId("64e7cc2b4c203fa0dcb2124a"),
                                                           New ObjectId("64e7cead4c203fa0dcb2124b"),
                                                           New ObjectId("64f124d4323eecf4270209c5")}

            _Estado = _controladorFacturaComercial.ListaCamposFacturaComercial(_FacturasIds,
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

                _Estado = Guardar(_facturasComercial, _pedimento)

            ElseIf _Estado.Status = TagWatcher.TypeStatus.OkBut Then

                _Estado.SetError(Me, "Fallé")

            ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

                _Estado.SetError(Me, "Medio Fallé")
            End If

        End If

        Return _Estado

    End Function

    Public Function Generar(objectIdPedimentos_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim filtro_ As BsonDocument = New BsonDocument().Add("_ids", objectIdPedimentos_(0))

        Dim items_ = operacionesDB_.Find(filtro_).ToList

        If items_.Count Then

            Dim item_ As OperacionGenerica = items_(0)

            Dim pedimento_ As DocumentoElectronico = item_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Dim FacturasIds_ = New List(Of ObjectId) From {New ObjectId("636d960d635a642f40780bff"),
                                                           New ObjectId("6419c3805354a4068f25cdb8"),
                                                           New ObjectId("6373d949af37871d9ef4deb0")}

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

                _Estado = Guardar(_facturasComercial, pedimento_)

            End If

        ElseIf _Estado.Status = TagWatcher.TypeStatus.OkBut Then

            _Estado.SetError(Me, "Fallé")

        ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

            _Estado.SetError(Me, "Medio Fallé")

        End If

        Return _Estado
    End Function

    Public Function Generar(pedimento_ As String) As TagWatcher Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim filtro_ As BsonDocument = New BsonDocument().Add("FolioDocumento", pedimento_)

        Dim items_ = operacionesDB_.Find(filtro_).ToList

        If items_.Count Then
            Dim item_ As OperacionGenerica = items_(0)

            Dim _pedimento As DocumentoElectronico = item_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Dim FacturasIds_ = New List(Of ObjectId) From {New ObjectId("636d960d635a642f40780bff"),
                                                           New ObjectId("6419c3805354a4068f25cdb8"),
                                                           New ObjectId("6373d949af37871d9ef4deb0")}

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

                _Estado = Guardar(_facturasComercial, _pedimento)

            End If

        ElseIf _Estado.Status = TagWatcher.TypeStatus.OkBut Then

            _Estado.SetError(Me, "Fallé")

        ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

            _Estado.SetError(Me, "Medio Fallé")

        End If

        Return _Estado

    End Function

    Public Function Generar(pedimentos_ As List(Of String)) As TagWatcher Implements IControladorManifestacionValor.Generar

        Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal()).GetType.Name)

        Dim filtro_ As BsonDocument = New BsonDocument().Add("FolioDocumento", pedimentos_(0))

        Dim items = operacionesDB_.Find(filtro_).ToList

        If items.Count Then
            Dim item As OperacionGenerica = items(0)

            Dim _pedimento As DocumentoElectronico = item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Dim _FacturasIds = New List(Of ObjectId) From {New ObjectId("636d960d635a642f40780bff"),
                                                           New ObjectId("6419c3805354a4068f25cdb8"),
                                                           New ObjectId("6373d949af37871d9ef4deb0")}

            _Estado = _controladorFacturaComercial.ListaCamposFacturaComercial(_FacturasIds,
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

                _Estado = Guardar(_facturasComercial, _pedimento)

            End If

        ElseIf _Estado.Status = TagWatcher.TypeStatus.OkBut Then

            _Estado.SetError(Me, "Fallé")

        ElseIf _Estado.Status = TagWatcher.TypeStatus.OkInfo Then

            _Estado.SetError(Me, "Medio Fallé")

        End If

        Return _Estado

    End Function

    Private Function Guardar(facturasComercial_ As Dictionary(Of ObjectId, List(Of Nodo)),
                             pedimento_ As DocumentoElectronico) As TagWatcher

        Dim operacionesGenerica_ = New List(Of OperacionGenerica)

        Dim iterador_ As Integer = 0

        Dim nombreCte_ As String = ""

        For Each _facturaComercial As KeyValuePair(Of ObjectId, List(Of Nodo)) In facturasComercial_.OrderBy(Function(g) DirectCast(g.Value(20), Campo).Valor)

            If nombreCte_ <> DirectCast(_facturaComercial.Value(20), Campo).Valor Then

                _ManifestacionesValor.Add(New ConstructorManifestacionValor())

                If nombreCte_ <> "" Then

                    operacionesGenerica_.Add(New OperacionGenerica(_ManifestacionesValor(iterador_)))

                    iterador_ = iterador_ + 1

                Else

                    iterador_ = 0

                End If

                With _ManifestacionesValor(iterador_)

                    .Seccion(SMV4).Attribute(CamposPedimento.CA_NUM_PEDIMENTO_COMPLETO).Valor =
                                                        pedimento_.Seccion(ANS1).Attribute(CamposPedimento.CA_NUM_PEDIMENTO_COMPLETO).Valor

                    .Seccion(SMV4).Attribute(CamposPedimento.CA_FECHA_PEDIM_ORIGINAL).Valor =
                                                        pedimento_.Seccion(ANS14).Attribute(CamposPedimento.CA_FECHA_ENTRADA).Valor

                    .FolioDocumento = "PRUEBA" + iterador_.ToString()

                    .FolioOperacion = "PRUEBA FOLIO OP"

                    .NombreCliente = DirectCast(_facturaComercial.Value(0), Campo).Valor

                End With

                nombreCte_ = DirectCast(_facturaComercial.Value(20), Campo).Valor

            End If

            For Each _campo As Nodo In _facturaComercial.Value

                With _ManifestacionesValor(iterador_)

                    If .Seccion(SMV2).Attribute(DirectCast(_campo, Campo).IDUnico) IsNot Nothing Then

                        .Seccion(SMV2).Attribute(DirectCast(_campo, Campo).IDUnico).Valor = DirectCast(_campo, Campo).Valor

                    End If

                    If .Seccion(SMV3).Attribute(DirectCast(_campo, Campo).IDUnico) IsNot Nothing Then

                        .Seccion(SMV3).Attribute(DirectCast(_campo, Campo).IDUnico).Valor = DirectCast(_campo, Campo).Valor

                    End If

                    If .Seccion(SMV4).Attribute(DirectCast(_campo, Campo).IDUnico) IsNot Nothing Then

                        .Seccion(SMV4).Attribute(DirectCast(_campo, Campo).IDUnico).Valor = DirectCast(_campo, Campo).Valor

                    End If

                    If .Seccion(SMV5).Attribute(DirectCast(_campo, Campo).IDUnico) IsNot Nothing Then

                        Select Case DirectCast(_campo, Campo).IDUnico

                            Case CA_FLETES, CA_SEGURO, CA_EMBALAJES, CA_OTROS_INCREMENTABLES, CA_DESCUENTOS

                                .Seccion(SMV5).Attribute(DirectCast(_campo, Campo).IDUnico).Valor += IIf(DirectCast(_campo, Campo).Valor Is Nothing, 0, DirectCast(_campo, Campo).Valor)

                            Case Else

                                .Seccion(SMV5).Attribute(DirectCast(_campo, Campo).IDUnico).Valor = DirectCast(_campo, Campo).Valor

                        End Select

                    End If

                End With

            Next

        Next

        operacionesGenerica_.Add(New OperacionGenerica(_ManifestacionesValor(iterador_)))

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
                {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorManifestacionValor).GetType.Name)

                operationsDB_.InsertMany(operacionesGenerica_)

                .SetOK()

            End Using

        End With

        Return _Estado

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

#End Region

End Class

Friend Class Pedimento
    Public Property Id As ObjectId
End Class
