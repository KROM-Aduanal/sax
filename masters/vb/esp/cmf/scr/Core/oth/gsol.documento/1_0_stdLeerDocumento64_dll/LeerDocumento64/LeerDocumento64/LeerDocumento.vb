Imports System.Xml
Imports gsol.documento
Imports System.Xml.Serialization
Imports Wma.Exceptions
Imports System.Configuration

Imports System.Security.Authentication
Imports System.Net

'Imports ValidarCFDi

Namespace gsol.documento

    Public Class LeerDocumento
        Implements IDocumento

#Region "Atributos"

        Private _tipoDocumento As IDocumento.TiposDocumento

        Private _cargarDesdeRuta As String

        Private _documentoBytes As Dictionary(Of Int32, Byte)

        Private _getDocumento As Object

        Private _atributosProcesados As Dictionary(Of String, String)

        Private _tagWatcher As TagWatcher

        Private _nombre As String

        Private _estatusDocumento As String

        'Otros datos

        Private _rfcEmisor As String

        Private _rfcReceptor As String

        Private _total As String

        Private _uuid

        Private _comprobante As ComprobanteCFDi


#End Region

#Region "Constructores"

        Sub New()

            _tipoDocumento = IDocumento.TiposDocumento.INDEFINIDO

            'RutaCliente

            'If Not ConfigurationManager.AppSettings("FormularioInicial") Is Nothing Then
            '_cargarDesdeRuta = "C:\SVN\SVN QA\Repositorio"
            'Environ("USERPROFILE")

            _cargarDesdeRuta = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\Repositorio"

            _documentoBytes = New Dictionary(Of Int32, Byte)

            _getDocumento = New Object

            _atributosProcesados = New Dictionary(Of String, String)

            _tagWatcher = New TagWatcher

            _tagWatcher.SetOK()


        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property NombreArchivo As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set

        End Property

        Public Property TagWatcherActive As TagWatcher _
            Implements IDocumento.TagWatcherActive

            Get

                Return _tagWatcher

            End Get

            Set(value As TagWatcher)

                _tagWatcher = value

            End Set

        End Property


        Public ReadOnly Property AtributosProcesados As Dictionary(Of String, String) Implements IDocumento.AtributosProcesados

            Get

                Return Me._atributosProcesados

            End Get

        End Property

        Public WriteOnly Property CargarDesdeRuta As String Implements IDocumento.CargarDesdeRuta

            Set(value As String)

                Me._cargarDesdeRuta = value

            End Set

        End Property

        Public ReadOnly Property DocumentoBytes As Dictionary(Of Integer, Byte) Implements IDocumento.DocumentoBytes

            Get

                Return Me._documentoBytes

            End Get

        End Property

        Public ReadOnly Property GetDocumento As Object Implements IDocumento.GetDocumento
            Get
                Return Me._getDocumento
            End Get
        End Property

        Public Property TipoDocumento As IDocumento.TiposDocumento Implements IDocumento.TipoDocumento
            Get
                Return Me._tipoDocumento
            End Get
            Set(value As IDocumento.TiposDocumento)
                Me._tipoDocumento = value
            End Set
        End Property

        Public ReadOnly Property Comprobante As ComprobanteCFDi
            Get
                Return _comprobante
            End Get
        End Property

#End Region

#Region "Metodos"

        Public Function ProcesarDocumento(ByVal tipoProcesable_ As IDocumento.TiposProcesable) As Object _
        Implements IDocumento.ProcesarDocumento

            Select Case tipoProcesable_

                Case IDocumento.TiposProcesable.INDEFINIDO

                Case IDocumento.TiposProcesable.XML

                    'Return ProcesarXMLCFDISerializado()

                    Return ProcesarXMLCFDI()

                Case IDocumento.TiposProcesable.XMLCFDIComplementoPago

                    Return ProcesarXMLCFDIConComplementoPago()

            End Select

        End Function

        'Public Function ProcesarXMLCFDISerializado() As Object

        '    If IO.File.Exists(_cargarDesdeRuta) = True Then

        '        Dim comprobanteCFDi_ As New Comprobante()

        '        Dim cfdiSeralizado_ As New System.IO.StreamReader(_cargarDesdeRuta)

        '        Dim deserializer As New XmlSerializer(comprobanteCFDi_.GetType)

        '        comprobanteCFDi_ = deserializer.Deserialize(cfdiSeralizado_)

        '        cfdiSeralizado_.Close()

        '        'llenamos el arreglo cucho! ¬¬, pedrobm 12092016

        '        'Select Case atributo_.Name

        '        ' Case "serie"

        '        'claveCaracteristica_ = "Serie"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("Serie", comprobanteCFDi_.serie, _atributosProcesados)

        '        ' Case "folio"

        '        'claveCaracteristica_ = "Folio"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("Folio", comprobanteCFDi_.folio, _atributosProcesados)


        '        ' Case "fecha"

        '        'claveCaracteristica_ = "Fecha"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("Fecha", comprobanteCFDi_.fecha, _atributosProcesados)

        '        ' Case "subTotal"

        '        'claveCaracteristica_ = "Subtotal"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("Subtotal", comprobanteCFDi_.subTotal, _atributosProcesados)

        '        'Case "total"

        '        'claveCaracteristica_ = "Total"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("Total", comprobanteCFDi_.total, _atributosProcesados)

        '        'Case "LugarExpedicion"

        '        'claveCaracteristica_ = "LugarExpedicion"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("LugarExpedicion", comprobanteCFDi_.LugarExpedicion, _atributosProcesados)

        '        'Case "metodoDePago"

        '        'claveCaracteristica_ = "MetododePago"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("MetododePago", comprobanteCFDi_.metodoDePago, _atributosProcesados)

        '        ' Case "Moneda"

        '        'claveCaracteristica_ = "Moneda"
        '        'valor_ = atributo_.Value

        '        AgregaColeccion("Moneda", comprobanteCFDi_.Moneda, _atributosProcesados)


        '        '    coleccionNodos_ = elementoRaiz_.ChildNodes

        '        '    For Each nodo_ As XmlNode In coleccionNodos_ 'Recorrido a nodos hijos e la raíz

        '        '        Select Case nodo_.Name

        '        '            Case "Emisor", "cfdi:Emisor"

        '        '                For Each atributo_ As XmlAttribute In nodo_.Attributes

        '        '                    Select Case atributo_.Name

        '        '                        Case "rfc"

        '        '                            claveCaracteristica_ = "RFCEmisor"
        '        '                            valor_ = atributo_.Value

        '        AgregaColeccion("RFCEmisor", comprobanteCFDi_.Emisor.rfc, _atributosProcesados)


        '        '                        Case "nombre"

        '        '                            claveCaracteristica_ = "NombreEmisor"
        '        '                            valor_ = atributo_.Value

        '        AgregaColeccion("NombreEmisor", comprobanteCFDi_.Emisor.nombre, _atributosProcesados)

        '        '                        Case Else

        '        '                            claveCaracteristica_ = ""

        '        '                    End Select

        '        '                    If Not claveCaracteristica_ = "" Then

        '        '                        If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

        '        '                            _atributosProcesados.Add(claveCaracteristica_, valor_)
        '        '                            claveCaracteristica_ = 0

        '        '                        End If

        '        '                    End If

        '        '                Next

        '        '            Case "Receptor", "cfdi:Receptor"

        '        '                For Each atributo_ As XmlAttribute In nodo_.Attributes

        '        '                    Select Case atributo_.Name

        '        '                        Case "rfc"
        '        '                            claveCaracteristica_ = "RFCReceptor"
        '        '                            valor_ = atributo_.Value

        '        AgregaColeccion("RFCReceptor", comprobanteCFDi_.Receptor.rfc, _atributosProcesados)

        '        '                        Case "nombre"
        '        '                            claveCaracteristica_ = "NombreReceptor"
        '        '                            valor_ = atributo_.Value

        '        AgregaColeccion("NombreReceptor", comprobanteCFDi_.Receptor.nombre, _atributosProcesados)

        '        '                        Case Else

        '        '                            claveCaracteristica_ = ""

        '        '                    End Select

        '        '                    If Not claveCaracteristica_ = "" Then

        '        '                        If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

        '        '                            _atributosProcesados.Add(claveCaracteristica_, valor_)
        '        '                            claveCaracteristica_ = 0

        '        '                        End If

        '        '                    End If
        '        '                Next

        '        '            Case "Impuestos", "cfdi:Impuestos"

        '        '                For Each atributo_ As XmlAttribute In nodo_.Attributes

        '        '                    Select Case atributo_.Name

        '        '                        Case "totalImpuestosRetenidos"

        '        '                            claveCaracteristica_ = "ImpuestosRetenidos"

        '        '                            valor_ = atributo_.Value

        '        AgregaColeccion("ImpuestosRetenidos", comprobanteCFDi_.Impuestos.totalImpuestosRetenidos, _atributosProcesados)


        '        '                        Case "totalImpuestosTrasladados"

        '        '                            claveCaracteristica_ = "ImpuestosTraslados"

        '        '                            valor_ = atributo_.Value

        '        AgregaColeccion("ImpuestosTraslados", comprobanteCFDi_.Impuestos.totalImpuestosTrasladados, _atributosProcesados)


        '        '                        Case Else

        '        '                            claveCaracteristica_ = ""

        '        '                    End Select

        '        '                    If Not claveCaracteristica_ = "" Then

        '        '                        _atributosProcesados.Add(claveCaracteristica_, valor_)

        '        '                        claveCaracteristica_ = 0

        '        '                    End If

        '        '                Next

        '        '                For Each NodoHijo_ As XmlNode In nodo_.ChildNodes

        '        '                    Select Case NodoHijo_.Name

        '        '                        Case "Retenciones", "cfdi:Retenciones"

        '        '                            For Each NodoNieto_ As XmlNode In NodoHijo_.ChildNodes

        '        '                                Select Case NodoNieto_.Name

        '        '                                    Case "Retencion", "cfdi:Retencion"

        '        '                                        For Each atributo_ As XmlAttribute In NodoNieto_.Attributes

        '        '                                            Select Case atributo_.Name

        '        '                                                Case "importe"

        '        '                                                    valor_ = atributo_.Value
        '        '                                                    tieneImporte_ = True

        '        For item_ = 0 To comprobanteCFDi_.Impuestos.Retenciones.Count - 1

        '            Select Case comprobanteCFDi_.Impuestos.Retenciones(item_).impuesto

        '                Case ComprobanteImpuestosRetencionImpuesto.IVA

        '                    AgregaColeccion("importeivaret", comprobanteCFDi_.Impuestos.Retenciones(item_).importe, _atributosProcesados)

        '                Case ComprobanteImpuestosRetencionImpuesto.ISR

        '                    AgregaColeccion("importeisrret", comprobanteCFDi_.Impuestos.Retenciones(item_).importe, _atributosProcesados)

        '            End Select

        '        Next


        '        '                                                Case "impuesto"

        '        '                                                    Select Case atributo_.Value

        '        '                                                        Case "IVA"

        '        '                                                            claveCaracteristica_ = "importeivaret"

        '        '                                                        Case "ISR"

        '        '                                                            claveCaracteristica_ = "importeisrret"

        '        '                                                        Case Else

        '        '                                                            claveCaracteristica_ = ""

        '        '                                                    End Select

        '        '                                                Case Else

        '        '                                                    claveCaracteristica_ = ""

        '        '                                            End Select

        '        '                                            If (Not claveCaracteristica_ = "") And tieneImporte_ Then

        '        '                                                _atributosProcesados.Add(claveCaracteristica_, valor_)

        '        '                                                claveCaracteristica_ = ""

        '        '                                                tieneImporte_ = False

        '        '                                            End If

        '        '                                        Next

        '        '                                End Select

        '        '                            Next


        '        For item_ = 0 To comprobanteCFDi_.Impuestos.Traslados.Count - 1

        '            Select Case comprobanteCFDi_.Impuestos.Traslados(item_).impuesto

        '                Case ComprobanteImpuestosTrasladoImpuesto.IVA

        '                    AgregaColeccion("importeivatras", comprobanteCFDi_.Impuestos.Traslados(item_).importe, _atributosProcesados)

        '                Case ComprobanteImpuestosTrasladoImpuesto.IEPS

        '                    AgregaColeccion("importeiepstras", comprobanteCFDi_.Impuestos.Traslados(item_).importe, _atributosProcesados)

        '            End Select

        '            Select Case comprobanteCFDi_.Impuestos.Traslados(item_).tasa

        '                Case 0.16

        '                    AgregaColeccion("tasaivatras", comprobanteCFDi_.Impuestos.Traslados(item_).tasa.ToString, _atributosProcesados)

        '                Case 0

        '                    AgregaColeccion("tasaiepstras", comprobanteCFDi_.Impuestos.Traslados(item_).tasa.ToString, _atributosProcesados)

        '            End Select

        '        Next

        '        '                        Case "Traslados", "cfdi:Traslados"

        '        '                            Dim TipoImpuesto_ As String = ""

        '        '                            For Each NodoNieto_ As XmlNode In NodoHijo_.ChildNodes

        '        '                                tasa_ = ""

        '        '                                TipoImpuesto_ = ""

        '        '                                Select Case NodoNieto_.Name

        '        '                                    Case "Traslado", "cfdi:Traslado"

        '        '                                        For Each atributo_ As XmlAttribute In NodoNieto_.Attributes

        '        '                                            Select Case atributo_.Name

        '        '                                                Case "importe"

        '        '                                                    valor_ = atributo_.Value
        '        '                                                    tieneImporte_ = True

        '        '                                                Case "impuesto"

        '        '                                                    TipoImpuesto_ = atributo_.Value

        '        '                                                Case "tasa"

        '        '                                                    tasa_ = atributo_.Value

        '        '                                                Case Else

        '        '                                                    claveCaracteristica_ = ""

        '        '                                            End Select

        '        '                                            If (Not TipoImpuesto_ = "") And tieneImporte_ Then

        '        '                                                Select Case TipoImpuesto_

        '        '                                                    Case "IVA"

        '        '                                                        claveCaracteristica_ = "importeivatras"

        '        '                                                    Case "IEPS"

        '        '                                                        claveCaracteristica_ = "importeiepstras"

        '        '                                                End Select

        '        '                                                If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

        '        '                                                    _atributosProcesados.Add(claveCaracteristica_, valor_)

        '        '                                                    claveCaracteristica_ = ""

        '        '                                                    tieneImporte_ = False

        '        '                                                End If

        '        '                                            End If

        '        '                                            If (Not TipoImpuesto_ = "") And Not (tasa_ = "") Then

        '        '                                                Select Case TipoImpuesto_

        '        '                                                    Case "IVA"

        '        '                                                        claveCaracteristica_ = "tasaivatras"

        '        '                                                    Case "IEPS"

        '        '                                                        claveCaracteristica_ = "tasaiepstras"

        '        '                                                End Select

        '        '                                                If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

        '        '                                                    _atributosProcesados.Add(claveCaracteristica_, tasa_)

        '        '                                                    claveCaracteristica_ = ""

        '        '                                                End If

        '        '                                            End If

        '        '                                        Next

        '        '                                End Select

        '        '                            Next

        '        '                    End Select

        '        '                Next


        '        '            Case "Complemento", "cfdi:Complemento"


        '        '                For Each timbrado_ As XmlNode In nodo_.ChildNodes

        '        '                    Select Case timbrado_.Name

        '        '                        Case "tfd:TimbreFiscalDigital"

        '        '                            For Each atributo_ As XmlAttribute In timbrado_.Attributes

        '        '                                Select Case atributo_.Name

        '        '                                    Case "UUID"
        '        '                                        claveCaracteristica_ = "UUID"

        '        AgregaColeccion("importeivatras", comprobanteCFDi_.Complemento.Any(0).OuterXml, _atributosProcesados)



        '        '                                        valor_ = atributo_.Value

        '        '                                    Case Else

        '        '                                        claveCaracteristica_ = ""

        '        '                                End Select

        '        '                                If Not claveCaracteristica_ = "" Then

        '        '                                    _atributosProcesados.Add(claveCaracteristica_, valor_)

        '        '                                    claveCaracteristica_ = 0

        '        '                                End If
        '        '                            Next

        '        '                    End Select
        '        '                Next

        '        '        End Select

        '        '    Next


        '        Return _atributosProcesados

        '        'MsgBox("ok deserializado." & comprobanteCFDi_.folio)

        '    Else

        '        _atributosProcesados.Clear()

        '        MsgBox("No se encontró el archivo [" & _cargarDesdeRuta & "], por favor verifique e intente nuevamente")

        '        Return Nothing

        '    End If

        'End Function

        Private Sub AgregaColeccion(ByVal claveCaracteristica_ As String,
                                 ByVal valor_ As String,
                                 ByRef coleccion_ As Dictionary(Of String, String))

            If Not coleccion_.ContainsKey(claveCaracteristica_) Then

                coleccion_.Add(claveCaracteristica_, valor_)

            End If

        End Sub

        Public Function ProcesarXMLCFDI() As Object

            Dim xmlDoc_ As New XmlDocument

            Dim elementoRaiz_ As XmlElement

            Dim coleccionNodos_ As XmlNodeList

            Dim claveCaracteristica_ As String = ""

            Dim tieneImporte_ As Boolean = False

            Dim tasa_ As String = ""

            Dim valor_ As String = Nothing

            Try
                _atributosProcesados.Clear()

                xmlDoc_.PreserveWhitespace = False

                xmlDoc_.Load(_cargarDesdeRuta) '_cargarDesdeRuta = "C:\SVN\SVN QA\Repositorio"

                elementoRaiz_ = xmlDoc_.DocumentElement

                If elementoRaiz_.Name.ToString = "Comprobante" Or elementoRaiz_.Name.ToString = "cfdi:Comprobante" Then 'Si el elemento raíz es "Comprobante" es una factura CFDi

                    For Each atributo_ As XmlAttribute In elementoRaiz_.Attributes 'Recorrido de atributos en el nodo "Comprobante"

                        Select Case LCase(atributo_.Name)

                            Case "version", "Version" 'ok

                                claveCaracteristica_ = "Version"
                                valor_ = atributo_.Value

                            Case "serie", "Serie" 'ok

                                claveCaracteristica_ = "Serie"
                                valor_ = atributo_.Value

                            Case "folio", "Folio" 'ok 

                                claveCaracteristica_ = "Folio"
                                valor_ = atributo_.Value

                            Case "fecha", "Fecha" 'ok

                                claveCaracteristica_ = "Fecha"
                                valor_ = atributo_.Value

                            Case "subTotal", "subtotal" 'ok

                                claveCaracteristica_ = "Subtotal"
                                valor_ = atributo_.Value

                            Case "total" 'ok

                                claveCaracteristica_ = "Total"
                                valor_ = atributo_.Value

                                _total = valor_

                            Case "LugarExpedicion", "lugarexpedicion" 'ok

                                claveCaracteristica_ = "LugarExpedicion"
                                valor_ = atributo_.Value

                            Case "metodoDePago", "metodopago"

                                claveCaracteristica_ = "MetododePago" 'ok
                                valor_ = atributo_.Value

                            Case "FormaPago", "formapago", "Formapago", "formaPago"

                                claveCaracteristica_ = "FormaPago" 'ok
                                valor_ = atributo_.Value

                            Case "Moneda", "moneda"

                                claveCaracteristica_ = "Moneda" 'ok
                                valor_ = atributo_.Value

                            Case "TipoCambio", "tipocambio"

                                claveCaracteristica_ = "TipoCambio"
                                valor_ = atributo_.Value

                            Case "TipoDeComprobante", "tipodecomprobante"

                                claveCaracteristica_ = "TipoDeComprobante"
                                valor_ = atributo_.Value

                            Case "Descuento", "descuento"

                                claveCaracteristica_ = "Descuento"
                                valor_ = atributo_.Value

                            Case Else

                                claveCaracteristica_ = ""

                        End Select

                        If Not claveCaracteristica_ = "" Then

                            If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                _atributosProcesados.Add(claveCaracteristica_, valor_)

                                claveCaracteristica_ = 0

                            End If

                        End If

                    Next

                    coleccionNodos_ = elementoRaiz_.ChildNodes

                    For Each nodo_ As XmlNode In coleccionNodos_ 'Recorrido a nodos hijos e la raíz

                        Select Case LCase(nodo_.Name)

                            Case "Emisor", "cfdi:Emisor", "emisor", "cfdi:emisor"

                                For Each atributo_ As XmlAttribute In nodo_.Attributes

                                    Select Case atributo_.Name

                                        Case "rfc", "Rfc"

                                            claveCaracteristica_ = "RFCEmisor"
                                            valor_ = atributo_.Value

                                            _rfcEmisor = valor_

                                        Case "nombre", "Nombre"

                                            claveCaracteristica_ = "NombreEmisor"
                                            valor_ = atributo_.Value

                                        Case Else

                                            claveCaracteristica_ = ""

                                    End Select

                                    If Not claveCaracteristica_ = "" Then

                                        If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                            _atributosProcesados.Add(claveCaracteristica_, valor_)
                                            claveCaracteristica_ = 0

                                        End If

                                    End If

                                Next

                            Case "Receptor", "cfdi:Receptor", "receptor", "cfdi:receptor"

                                For Each atributo_ As XmlAttribute In nodo_.Attributes

                                    Select Case atributo_.Name

                                        Case "rfc", "Rfc"
                                            claveCaracteristica_ = "RFCReceptor"

                                            valor_ = atributo_.Value

                                            _rfcReceptor = valor_

                                        Case "nombre", "Nombre"
                                            claveCaracteristica_ = "NombreReceptor"
                                            valor_ = atributo_.Value

                                        Case Else

                                            claveCaracteristica_ = ""

                                    End Select

                                    If Not claveCaracteristica_ = "" Then

                                        If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                            _atributosProcesados.Add(claveCaracteristica_, valor_)

                                            claveCaracteristica_ = 0

                                        End If

                                    End If
                                Next

                            Case "Impuestos", "cfdi:Impuestos", "impuestos", "cfdi:impuestos"

                                For Each atributo_ As XmlAttribute In nodo_.Attributes

                                    Select Case LCase(atributo_.Name)

                                        Case "totalImpuestosRetenidos", "TotalImpuestosRetenidos", "totalimpuestosretenidos"

                                            claveCaracteristica_ = "ImpuestosRetenidos"

                                            valor_ = atributo_.Value

                                        Case "totalImpuestosTrasladados", "TotalImpuestosTrasladados", "totalimpuestostrasladados"

                                            claveCaracteristica_ = "ImpuestosTraslados"

                                            valor_ = atributo_.Value

                                        Case Else

                                            claveCaracteristica_ = ""

                                    End Select

                                    If Not claveCaracteristica_ = "" Then

                                        If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                            _atributosProcesados.Add(claveCaracteristica_, valor_)

                                            claveCaracteristica_ = 0

                                        End If


                                    End If

                                Next

                                For Each NodoHijo_ As XmlNode In nodo_.ChildNodes

                                    Select Case LCase(NodoHijo_.Name)

                                        Case "Retenciones", "cfdi:Retenciones", "retenciones", "cfdi:retenciones"

                                            For Each NodoNieto_ As XmlNode In NodoHijo_.ChildNodes

                                                Select Case LCase(NodoNieto_.Name)


                                                    Case "Retencion", "cfdi:Retencion", "retencion", "cfdi:retencion"

                                                        Dim valorTotalRetencionesIVA_ As Double = 0

                                                        Dim valorTotalRetencionesISR_ As Double = 0

                                                        For Each atributo_ As XmlAttribute In NodoNieto_.Attributes

                                                            Select Case LCase(atributo_.Name)

                                                                Case "importe"

                                                                    valor_ = atributo_.Value

                                                                    tieneImporte_ = True

                                                                Case "impuesto"

                                                                    Select Case atributo_.Value

                                                                        Case "IVA", "002" 'Nuevo esquema

                                                                            claveCaracteristica_ = "importeivaret"

                                                                        Case "ISR", "001"

                                                                            claveCaracteristica_ = "importeisrret"

                                                                        Case Else

                                                                            claveCaracteristica_ = ""

                                                                    End Select

                                                                Case Else

                                                                    claveCaracteristica_ = ""

                                                            End Select

                                                            If (Not claveCaracteristica_ = "") And tieneImporte_ Then

                                                                If IsNumeric(valor_) Then

                                                                    Select Case claveCaracteristica_

                                                                        Case "importeivaret"

                                                                            valorTotalRetencionesIVA_ += Convert.ToDouble(valor_)

                                                                        Case "importeisrret"

                                                                            valorTotalRetencionesISR_ += Convert.ToDouble(valor_)

                                                                    End Select


                                                                End If


                                                                '    'If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                                                '    '    _atributosProcesados.Add(claveCaracteristica_, valor_)

                                                                '    'End If

                                                                claveCaracteristica_ = ""

                                                                tieneImporte_ = False

                                                            End If

                                                        Next

                                                        If valorTotalRetencionesIVA_ > 0 Then

                                                            If Not _atributosProcesados.ContainsKey("importeivaret") Then

                                                                _atributosProcesados.Add("importeivaret", valorTotalRetencionesIVA_.ToString)

                                                            End If

                                                        End If

                                                        If valorTotalRetencionesISR_ > 0 Then

                                                            If Not _atributosProcesados.ContainsKey("importeisrret") Then

                                                                _atributosProcesados.Add("importeisrret", valorTotalRetencionesISR_.ToString)

                                                            End If

                                                        End If


                                                End Select

                                            Next

                                        Case "Traslados", "cfdi:Traslados", "traslados", "cfdi:traslados"

                                            Dim TipoImpuesto_ As String = ""

                                            For Each NodoNieto_ As XmlNode In NodoHijo_.ChildNodes

                                                tasa_ = ""

                                                TipoImpuesto_ = ""

                                                Dim valorTotalTrasladosIVA_ As Double = 0

                                                Dim valorTotalTrasladosIEPS_ As Double = 0

                                                Select Case LCase(NodoNieto_.Name)

                                                    Case "Traslado", "cfdi:Traslado", "traslado", "cfdi:traslado"

                                                        For Each atributo_ As XmlAttribute In NodoNieto_.Attributes

                                                            Select Case LCase(atributo_.Name)

                                                                Case "importe"

                                                                    valor_ = atributo_.Value

                                                                    tieneImporte_ = True

                                                                Case "impuesto"

                                                                    TipoImpuesto_ = atributo_.Value

                                                                Case "tasa"

                                                                    tasa_ = atributo_.Value

                                                                Case Else

                                                                    claveCaracteristica_ = ""

                                                            End Select


                                                            If (Not TipoImpuesto_ = "") And Not (tasa_ = "") Then

                                                                Select Case TipoImpuesto_

                                                                    Case "IVA", "002"

                                                                        claveCaracteristica_ = "tasaivatras"

                                                                    Case "IEPS"

                                                                        claveCaracteristica_ = "tasaiepstras"

                                                                End Select

                                                                If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                                                    _atributosProcesados.Add(claveCaracteristica_, tasa_)

                                                                End If

                                                                claveCaracteristica_ = ""

                                                            End If

                                                            If (Not TipoImpuesto_ = "") And tieneImporte_ Then

                                                                Select Case TipoImpuesto_

                                                                    Case "IVA", "002"

                                                                        claveCaracteristica_ = "importeivatras"

                                                                        If IsNumeric(valor_) Then
                                                                            valorTotalTrasladosIVA_ += Convert.ToDouble(valor_)
                                                                        End If


                                                                    Case "IEPS"

                                                                        claveCaracteristica_ = "importeiepstras"

                                                                        If IsNumeric(valor_) Then
                                                                            valorTotalTrasladosIEPS_ += Convert.ToDouble(valor_)
                                                                        End If


                                                                End Select

                                                                'If Not _atributosProcesados.ContainsKey(claveCaracteristica_) Then

                                                                '    _atributosProcesados.Add(claveCaracteristica_, valor_)

                                                                'End If

                                                                claveCaracteristica_ = ""

                                                                tieneImporte_ = False

                                                            End If


                                                            '------------


                                                            '--------------

                                                        Next

                                                        If valorTotalTrasladosIVA_ > 0 Then

                                                            If Not _atributosProcesados.ContainsKey("importeivatras") Then

                                                                _atributosProcesados.Add("importeivatras", valorTotalTrasladosIVA_.ToString)

                                                            End If

                                                        End If

                                                        If valorTotalTrasladosIEPS_ > 0 Then

                                                            If Not _atributosProcesados.ContainsKey("importeiepstras") Then

                                                                _atributosProcesados.Add("importeiepstras", valorTotalTrasladosIEPS_.ToString)

                                                            End If

                                                        End If

                                                End Select

                                            Next

                                    End Select

                                Next

                            Case "Complemento", "cfdi:Complemento", "complemento", "cfdi:complemento"

                                For Each timbrado_ As XmlNode In nodo_.ChildNodes

                                    Select Case timbrado_.Name

                                        Case "tfd:TimbreFiscalDigital"

                                            For Each atributo_ As XmlAttribute In timbrado_.Attributes

                                                Select Case atributo_.Name

                                                    Case "UUID"

                                                        claveCaracteristica_ = "UUID"

                                                        valor_ = atributo_.Value

                                                        _uuid = valor_

                                                    Case Else

                                                        claveCaracteristica_ = ""

                                                End Select

                                                If Not claveCaracteristica_ = "" Then

                                                    _atributosProcesados.Add(claveCaracteristica_, valor_)

                                                    claveCaracteristica_ = 0

                                                End If
                                            Next

                                    End Select
                                Next

                        End Select

                    Next

                    Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
                    Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)
                    ServicePointManager.SecurityProtocol = Tls12

                    Dim oConsulta As validacion_.ConsultaCFDIServiceClient = New validacion_.ConsultaCFDIServiceClient()

                    Dim oAcuse As validacion_.Acuse = New validacion_.Acuse()

                    _rfcEmisor = _rfcEmisor.Replace("&", "&amp;")

                    'If _rfcEmisor = "IF&610526C95" Then

                    '    _rfcEmisor = "IF&amp;610526C95"

                    'End If

                    _rfcReceptor = _rfcReceptor.Replace("&", "&amp;")

                    'If _rfcReceptor = "IF&610526C95" Then

                    '    _rfcReceptor = "IF&amp;610526C95"

                    'End If

                    oAcuse = oConsulta.Consulta("?re=" & _rfcEmisor & "&rr=" & _rfcReceptor & "&tt=" & _total & "&id=" & _uuid)

                    Select Case oAcuse.CodigoEstatus

                        Case "S - Comprobante obtenido satisfactoriamente."

                            If oAcuse.Estado = "Cancelado" Then

                                'comentar esta linea para permitir cargar un XML no validado correctamente
                                _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_012_1002, oAcuse.Estado)

                            End If


                        Case Else
                            '"N - 601: La expresión impresa proporcionada no es válida."
                            'comentar esta linea para permitir cargar un XML no validado correctamente
                            _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_012_1001, oAcuse.Estado)

                    End Select

                    Return _atributosProcesados

                Else

                    _atributosProcesados.Clear()

                    Return Nothing

                End If


            Catch ex As Exception

                _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_012_1000,
                                         ex.Message)

                Return Nothing

            End Try


        End Function

        Public Function ProcesarXMLCFDIConComplementoPago() As ComprobanteCFDi

            Dim xmlDoc_ As New XmlDocument

            Dim elementoRaiz_ As XmlElement

            Dim coleccionNodos_ As XmlNodeList

            'Dim claveCaracteristica_ As String = ""

            'Dim tieneImporte_ As Boolean = False

            'Dim tasa_ As String = ""

            'Dim valor_ As String = Nothing

            Try
                _comprobante = New ComprobanteCFDi
                Dim objPago_ As ComprobantePagos
                Dim objDoctoRelacionado_ As ComprobanteDocumentosRelacionados

                xmlDoc_.PreserveWhitespace = False

                xmlDoc_.Load(_cargarDesdeRuta) '_cargarDesdeRuta = "C:\SVN\SVN QA\Repositorio"

                elementoRaiz_ = xmlDoc_.DocumentElement

                If elementoRaiz_.Name.ToString = "Comprobante" Or elementoRaiz_.Name.ToString = "cfdi:Comprobante" Then 'Si el elemento raíz es "Comprobante" es una factura CFDi

                    For Each atributo_ As XmlAttribute In elementoRaiz_.Attributes 'Recorrido de atributos en el nodo "Comprobante"

                        Select Case LCase(atributo_.Name)

                            Case "version", "Version" 'ok

                                _comprobante.Version = atributo_.Value

                            Case "serie", "Serie" 'ok

                                _comprobante.Serie = atributo_.Value

                            Case "folio", "Folio" 'ok 

                                _comprobante.Folio = atributo_.Value

                            Case "fecha", "Fecha" 'ok

                                _comprobante.Fecha = atributo_.Value

                                'Case "subTotal", "subtotal" 'ok

                            Case "total", "Total" 'ok

                                _total = atributo_.Value

                            Case "LugarExpedicion", "lugarexpedicion" 'ok

                                _comprobante.LugarExpedicion = atributo_.Value

                            Case "TipoDeComprobante", "tipodecomprobante"

                                'Select Case atributo_.Value
                                '    Case "P"

                                '        _comprobante.TipoDeComprobante = c_TipoDeComprobante.P

                                'End Select
                                _comprobante.TipoDeComprobante = atributo_.Value

                                'Case "Moneda", "moneda"

                                'Case Else

                                '    claveCaracteristica_ = ""

                        End Select

                    Next

                    coleccionNodos_ = elementoRaiz_.ChildNodes

                    For Each nodo_ As XmlNode In coleccionNodos_ 'Recorrido a nodos hijos e la raíz

                        Select Case LCase(nodo_.Name)

                            Case "Emisor", "cfdi:Emisor", "emisor", "cfdi:emisor"

                                For Each atributo_ As XmlAttribute In nodo_.Attributes

                                    Select Case atributo_.Name

                                        Case "rfc", "Rfc"

                                            _comprobante.RFCEmisor = atributo_.Value


                                            _rfcEmisor = atributo_.Value

                                            Exit For
                                            'Case "nombre", "Nombre"


                                            'Case Else

                                            '    claveCaracteristica_ = ""

                                    End Select

                                Next

                            Case "Receptor", "cfdi:Receptor", "receptor", "cfdi:receptor"

                                For Each atributo_ As XmlAttribute In nodo_.Attributes

                                    Select Case atributo_.Name

                                        Case "rfc", "Rfc"

                                            _comprobante.RFCReceptor = atributo_.Value

                                            _rfcReceptor = atributo_.Value

                                            Exit For

                                            'Case "nombre", "Nombre"
                                            '    claveCaracteristica_ = "NombreReceptor"
                                            '    valor_ = atributo_.Value

                                            'Case Else

                                            '    claveCaracteristica_ = ""

                                    End Select

                                Next

                            Case "Complemento", "cfdi:Complemento", "complemento", "cfdi:complemento"

                                For Each nodopagos_ As XmlNode In nodo_.ChildNodes

                                    Select Case nodopagos_.Name

                                        Case "pago10:Pagos"

                                            For Each nodopago_ As XmlNode In nodopagos_.ChildNodes

                                                Select Case nodopago_.Name

                                                    Case "pago10:Pago"

                                                        objPago_ = New ComprobantePagos

                                                        For Each atributo_ As XmlAttribute In nodopago_.Attributes

                                                            Select Case atributo_.Name

                                                                Case "FechaPago"

                                                                    objPago_.FechaPago = atributo_.Value

                                                                Case "FormaDePagoP"

                                                                    objPago_.FormaPagoP = atributo_.Value

                                                                Case "MonedaP"

                                                                    objPago_.MonedaP = atributo_.Value

                                                                Case "TipoCambioP"

                                                                    objPago_.TipoCambioP = atributo_.Value

                                                                Case "Monto"

                                                                    objPago_.Monto = atributo_.Value

                                                                Case "NumOperacion"

                                                                    objPago_.NumOperacion = atributo_.Value

                                                                Case "RFCEmisorCtaORd"

                                                                    objPago_.RFCEmisorCtaOrd = atributo_.Value

                                                                Case "NomBancoOrdExt"

                                                                    objPago_.BancoOrdExt = atributo_.Value

                                                                Case "CtaOrdenante"

                                                                    objPago_.CuentaOrdenante = atributo_.Value

                                                                Case "RFCEmisorCtaBen"

                                                                    objPago_.RFCEmisorCtaBen = atributo_.Value

                                                                Case "CtaBeneficiario"

                                                                    objPago_.CuentaBeneficiario = atributo_.Value

                                                            End Select

                                                        Next

                                                        For Each nodoDR_ As XmlNode In nodopago_.ChildNodes

                                                            Select Case nodoDR_.Name

                                                                Case "pago10:DoctoRelacionado"

                                                                    objDoctoRelacionado_ = New ComprobanteDocumentosRelacionados

                                                                    For Each atributo_ As XmlAttribute In nodoDR_.Attributes

                                                                        Select Case atributo_.Name

                                                                            Case "IdDocumento"

                                                                                objDoctoRelacionado_.idDocumento = atributo_.Value

                                                                            Case "Serie"

                                                                                objDoctoRelacionado_.Serie = atributo_.Value

                                                                            Case "Folio"

                                                                                objDoctoRelacionado_.Folio = atributo_.Value

                                                                            Case "MonedaDR"

                                                                                objDoctoRelacionado_.MonedaDR = atributo_.Value

                                                                            Case "TipoCambioDR"

                                                                                objDoctoRelacionado_.TipoCambioDR = atributo_.Value

                                                                            Case "MetodoDePagoDR"

                                                                                objDoctoRelacionado_.MetodoDePagoDR = atributo_.Value

                                                                            Case "NumParcialidad"

                                                                                objDoctoRelacionado_.NumParcialidad = atributo_.Value

                                                                            Case "ImpSaldoAnt"

                                                                                objDoctoRelacionado_.SaldoAnterior = atributo_.Value

                                                                            Case "ImpPagado"

                                                                                objDoctoRelacionado_.ImportePagado = atributo_.Value

                                                                            Case "ImpSaldoInsoluto"

                                                                                objDoctoRelacionado_.SaldoInsoluto = atributo_.Value

                                                                        End Select

                                                                    Next

                                                                    objPago_.ListaDocumentosRelacionados.Add(objDoctoRelacionado_)

                                                            End Select

                                                        Next

                                                        _comprobante.ListaPagos.Add(objPago_)

                                                End Select

                                            Next

                                    End Select

                                Next

                                For Each timbrado_ As XmlNode In nodo_.ChildNodes

                                    Select Case timbrado_.Name

                                        Case "tfd:TimbreFiscalDigital"

                                            For Each atributo_ As XmlAttribute In timbrado_.Attributes

                                                Select Case atributo_.Name

                                                    Case "UUID"

                                                        _comprobante.UUID = atributo_.Value

                                                        _uuid = atributo_.Value

                                                End Select

                                            Next

                                    End Select

                                Next

                        End Select

                    Next

                    Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
                    Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)
                    ServicePointManager.SecurityProtocol = Tls12

                    Dim oConsulta As validacion_.ConsultaCFDIServiceClient = New validacion_.ConsultaCFDIServiceClient()

                    Dim oAcuse As validacion_.Acuse = New validacion_.Acuse()

                    _rfcEmisor = _rfcEmisor.Replace("&", "&amp;")

                    'If _rfcEmisor = "IF&610526C95" Then

                    '    _rfcEmisor = "IF&amp;610526C95"

                    'End If

                    _rfcReceptor = _rfcReceptor.Replace("&", "&amp;")

                    'If _rfcReceptor = "IF&610526C95" Then

                    '    _rfcReceptor = "IF&amp;610526C95"

                    'End If

                    oAcuse = oConsulta.Consulta("?re=" & _rfcEmisor & "&rr=" & _rfcReceptor & "&tt=" & _total & "&id=" & _uuid)

                    Select Case oAcuse.CodigoEstatus

                        Case "S - Comprobante obtenido satisfactoriamente."

                            If oAcuse.Estado = "Cancelado" Then

                                'comentar esta linea para permitir cargar un XML no validado correctamente
                                _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_012_1002, oAcuse.Estado)

                            End If


                        Case Else
                            '"N - 601: La expresión impresa proporcionada no es válida."
                            'comentar esta linea para permitir cargar un XML no validado correctamente
                            _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_012_1001, oAcuse.Estado)

                    End Select

                    Return _comprobante

                Else

                    Return Nothing

                End If


            Catch ex As Exception

                _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_012_1000,
                                         ex.Message)

                Return Nothing

            End Try

        End Function


#End Region

        Public Property NombreDocumento As String _
            Implements IDocumento.NombreDocumento
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        Public Property EstatusDocumento As IDocumento.EstatusDocumentos _
            Implements IDocumento.EstatusDocumento
            Get
                Return _estatusDocumento
            End Get
            Set(value As IDocumento.EstatusDocumentos)
                _estatusDocumento = value
            End Set
        End Property
    End Class

End Namespace
