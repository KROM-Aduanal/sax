Imports System.Text
Imports Gsol.Reportes
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

''' <summary>
''' Creado y modificado por Ivan Juarez 15 Oct 2014
''' Convierte un objeto cualquiera asignado en un documento XML en memoria por medio de dos metodos
''' Un metodo manual que lo conviert en un stringbuilder y 
'''  El metodo manual requiere que el objeto a convertir posea ciertas propiedades como son:
'''    -Una propiedad NombreXML.- que contiene el nombre del elemento inicial del XML
'''    -Una propiedad TextoXML.- que contiene el elemento Texto de un XML
'''    -Una coleccion de objetos hijos llamada Hijos
'''    -Una coleccion de Atributos llamada Atributos que contenga
'''       *Una propiedad llamada Nombre con el nombre del Atributo
'''       *Una propiedad llamada Valor con el valor del Atributo
''' Un metodo por medio de serializacion que lo convierte en un xmlDocument
''' </summary>
''' <remarks></remarks>
Public Class clsDocumentoXML
    Implements IDocumento

#Region "Atributos"

    Private _formatoDocumento As IDocumento.enmTiposDocumento
    Private _tipoNameSpace As IDocumento.enmTipoNameSpace
    Private _metodoCreacion As IDocumento.enmMetodoCreacion
    Private _tipoCodificacion As IDocumento.enmTipoCodificacion
    Public _objetoFuente As Object
    Private _documentoEletronico As Object
    Private _documentoBuilder As StringBuilder
    Private _rutaFisica As String


#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Formato en el que se generará el documento
    ''' </summary>
    Public Property FormatoDocumento As IDocumento.enmTiposDocumento Implements IDocumento.FormatoDocumento
        Get
            FormatoDocumento = _formatoDocumento
        End Get
        Set(value As IDocumento.enmTiposDocumento)
            _formatoDocumento = value
        End Set
    End Property

    ''' <summary>
    ''' Si el encabezado lleva el namespace default, ninguno u otro namespace
    ''' PORHACER  crear almacen de otros namespaces y agregarlos
    ''' </summary>
    Public Property TipoNameSpace As IDocumento.enmTipoNameSpace Implements IDocumento.TipoNameSpace
        Get
            TipoNameSpace = _tipoNameSpace
        End Get
        Set(value As IDocumento.enmTipoNameSpace)
            _tipoNameSpace = value
        End Set
    End Property

    ''' <summary>
    ''' Por el método creado o por serialización
    ''' </summary>
    Public Property MetodoCreacion As IDocumento.enmMetodoCreacion Implements IDocumento.MetodoCreacion
        Get
            MetodoCreacion = _metodoCreacion
        End Get
        Set(value As IDocumento.enmMetodoCreacion)
            _metodoCreacion = value
        End Set
    End Property

    ''' <summary>
    ''' Tipo de codificación que se utilizará para el archivo
    ''' </summary>
    Public Property TipoCodificacion As IDocumento.enmTipoCodificacion Implements IDocumento.TipoCodificacion
        Get
            TipoCodificacion = _tipoCodificacion
        End Get
        Set(value As IDocumento.enmTipoCodificacion)
            _tipoCodificacion = value
        End Set
    End Property

    ''' <summary>
    ''' Objeto a convertir en el tipo de formato solicitado
    ''' </summary>
    Public WriteOnly Property ObjetoFuente As Object Implements IDocumento.ObjetoFuente
        Set(value As Object)
            _objetoFuente = value
        End Set
    End Property

    ''' <summary>
    ''' Documento XML generado como un string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DocumentoElectronico As Object Implements IDocumento.DocumentoElectronico
        Get
            Select Case _metodoCreacion 'Si fue metodo manual se creo un stringbuilter y se asigna convertido en string
                Case IDocumento.enmMetodoCreacion.Manual
                    _documentoEletronico = _documentoBuilder.ToString()
            End Select
            DocumentoElectronico = _documentoEletronico
        End Get
    End Property

    ''' <summary>
    ''' Ruta en donde se almacenará fisicamente el archivo genereado
    ''' </summary>
    Public WriteOnly Property RutaFisica As String Implements IDocumento.RutaFisica
        Set(value As String)
            _rutaFisica = value
        End Set
    End Property

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comienza y termina con la generacion del documento XML por medio del método manual (el método se detalla al inicio de la clase)
    ''' </summary>
    Public Sub GeneraDocumento(ArchivocConRuta As String, Optional namespaces_ As XmlSerializerNamespaces = Nothing) Implements IDocumento.GeneraDocumento

        If _documentoEletronico <> vbNullString Then
            _documentoEletronico = vbNullString
        End If
        Select Case FormatoDocumento

            Case IDocumento.enmTiposDocumento.XML
                Select Case _metodoCreacion

                    Case IDocumento.enmMetodoCreacion.Manual
                        'Se genera el archivo XML
                        GeneraDocumentoXML(_objetoFuente)
                        'Se le agrega el encabezado
                        Select Case _tipoCodificacion
                            Case IDocumento.enmTipoCodificacion.UTF8
                                _documentoBuilder.Insert(0, "<?xml version=""1.0"" encoding=""utf-8""?>", 1)
                        End Select

                    Case IDocumento.enmMetodoCreacion.Serializacion
                        Select Case _tipoCodificacion
                            Case IDocumento.enmTipoCodificacion.UTF8
                                SerializaObjeto(Encoding.UTF8, namespaces_)
                        End Select
                    Case IDocumento.enmMetodoCreacion.SerializacionStreamWritter
                        SerializaObjetoStreamWritter(ArchivocConRuta, namespaces_)

                    Case IDocumento.enmMetodoCreacion.SerializacionStringWritter
                        SerializaObjetoStringWritter(ArchivocConRuta, namespaces_)
                End Select

        End Select

    End Sub

    ''' <summary>
    ''' Recursivamente Recorre un objeto en busca de sus textos, atributos e hijos para generar por medio de uniones de cadenas
    ''' mediante un stringbuilder un archivo XML, se requiere que el objeto contega lo siguiente:
    '''    -Una propiedad NombreXML.- que contiene el nombre del elemento inicial del XML
    '''    -Una propiedad TextoXML.- que contiene el elemento Texto de un XML
    '''    -Una coleccion de objetos hijos llamada Hijos
    '''    -Una coleccion de Atributos llamada Atributos que contenga
    '''       *Una propiedad llamada Nombre con el nombre del Atributo
    '''       *Una propiedad llamada Valor con el valor del Atributo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GeneraDocumentoXML(objetoNodoActual_ As Object) As Object

        'If objetoHijo Is Nothing Then
        '    _objetoTrabajo = _objetoFuente
        'Else
        '    _objetoTrabajo = objetoHijo
        'End If

        'Se inicia la creacion del xml con la apertura del mismo
        _documentoBuilder.Append("<" & objetoNodoActual_.NombreXML)

        'Si tiene atributos se agregan al nodo
        If objetoNodoActual_.Atributos.count > 0 Then
            For Each atributo_ In objetoNodoActual_.Atributos
                _documentoBuilder.Append(" " & atributo_.Nombre & "='" & atributo_.Valor & "'")
            Next
        End If

        'se cierra el nodo
        'si despues de los atributos no tiene ni texto ni hijos, se ciera el nodo de manera <nodo/>
        If objetoNodoActual_.TextoXML = vbNullString And objetoNodoActual_.Hijos Is Nothing Then
            _documentoBuilder.Append("/>")
        Else
            _documentoBuilder.Append(">")
        End If

        'Si tiene texto el objeto
        If objetoNodoActual_.TextoXML <> vbNullString Then
            _documentoBuilder.Append(objetoNodoActual_.TextoXML)
        End If

        'Si tiene hijo se agregaran los nodos de manera recursiva
        If Not objetoNodoActual_.Hijos Is Nothing Then
            For Each elementoHijo_ In objetoNodoActual_.Hijos
                elementoHijo_ = GeneraDocumentoXML(elementoHijo_)
            Next
        End If

        'Else
        '    If _objetoTrabajo.TieneHijos Then
        '        For Each ElementoHijo In _objetoTrabajo.Hijos
        '            _objetoTrabajo = ElementoHijo
        '            GeneraDocumento()
        '        Next
        '    Else
        '        _documentoEletronico.Append("</" & _objetoTrabajo.NombreXML & ">")
        '    End If
        'End If

        'Else ' NO TIENE ATRIBUTOS
        '_documentoEletronico.Append(">")

        'If _objetoTrabajo.TieneTexto Then
        '    _documentoEletronico.Append(_objetoTrabajo.TextoXML)

        '    If _objetoTrabajo.TieneHijos Then
        '        For Each ElementoHijo In _objetoTrabajo.Hijos
        '            _objetoTrabajo = ElementoHijo
        '            GeneraDocumento()
        '        Next
        '    Else
        '        _documentoEletronico.Append("</" & _objetoTrabajo.NombreXML & ">")
        '    End If
        'Else ' NO TIENE TEXTO
        '    If _objetoTrabajo.TieneHijos Then
        '        For Each ElementoHijo In _objetoTrabajo.Hijos
        '            _objetoTrabajo = ElementoHijo
        '            GeneraDocumento()
        '        Next
        '    Else ' NO TIENE HIJOS
        '        _documentoEletronico.Append("</" & _objetoTrabajo.NombreXML & ">")
        '    End If
        'End If
        'End If

        'Se van cerrando los nodos
        'si despues de los atributos tuvo texto o hijos, se cierra el nodo completo de modo </nodo>
        If objetoNodoActual_.TextoXML <> vbNullString Or (Not objetoNodoActual_.Hijos Is Nothing) Then
            _documentoBuilder.Append("</" & objetoNodoActual_.NombreXML & ">")

        End If

        Return objetoNodoActual_
    End Function

    ''' <summary>
    ''' Serializa un objeto por medio del XMLSerializer, MemoryStream, xmlTexWriter
    ''' Y lo convierte en cadena con formato UTF-8
    ''' </summary>
    Private Sub SerializaObjeto(Encoding_ As Encoding, Optional namespaces_ As XmlSerializerNamespaces = Nothing)
        'Public Function SerializaObjeto(Of T)(ByVal obj As T) As String Implements IDocumento.SerializaObjeto
        Try
            Dim ns_ As XmlSerializerNamespaces = Nothing
            Dim xmlDoc As New XmlDocument
            Select Case _tipoNameSpace
                Case IDocumento.enmTipoNameSpace.SinNameSpace
                    'Para Quitar namespaces se agregan namespaces vacios
                    ns_ = New XmlSerializerNamespaces()
                    ns_.Add("", "")
                Case IDocumento.enmTipoNameSpace.OtrosNameSpaces
                    ns_ = namespaces_
            End Select

            Dim memoryStream_ As New MemoryStream()
            'Dim xsSerializador_ As New XmlSerializer(GetType(T))
            Dim xsSerializador_ As New XmlSerializer(_objetoFuente.GetType)
            Dim xmlTextWriter_ As New XmlTextWriter(memoryStream_, Encoding_)
            xmlTextWriter_.Formatting = Formatting.Indented
            'Dim xmlTextWriter_ As New XmlTextWriter("C:\C\X.xml", Encoding.UTF8)

            xsSerializador_.Serialize(xmlTextWriter_, _objetoFuente, ns_)
            '_documentoEletronico = xmlTextWriter_

            'memoryStream_.Seek(0, SeekOrigin.Begin)
            'xmlDoc.Load(memoryStream_)
            'xmlDoc.Save("C:\C\Y.xml")

            memoryStream_ = CType(xmlTextWriter_.BaseStream, MemoryStream)
            _documentoEletronico = ByteArrayToString(memoryStream_.ToArray(), Encoding_)

            memoryStream_.Dispose()
            xmlTextWriter_.Close()
        Catch

            _documentoEletronico = vbNullString
        End Try
    End Sub

    Private Sub SerializaObjetoStreamWritter(ArchivocConRuta_ As String, Optional namespaces_ As XmlSerializerNamespaces = Nothing)
        Try
            Dim ns_ As XmlSerializerNamespaces = Nothing
            Select Case _tipoNameSpace
                Case IDocumento.enmTipoNameSpace.SinNameSpace
                    'Para Quitar namespaces se agregan namespaces vacios
                    ns_ = New XmlSerializerNamespaces()
                    ns_.Add("", "")
                Case IDocumento.enmTipoNameSpace.OtrosNameSpaces
                    ns_ = namespaces_
            End Select

            Dim objStreamWriter As New StreamWriter(ArchivocConRuta_)
            Dim xsSerializador_ As New XmlSerializer(_objetoFuente.GetType)
            xsSerializador_.Serialize(objStreamWriter, _objetoFuente, ns_)

            objStreamWriter.Dispose()
            objStreamWriter.Close()

            Using sr As StreamReader = New StreamReader(ArchivocConRuta_)
                _documentoEletronico = sr.ReadToEnd()
            End Using
        Catch
            _documentoEletronico = Nothing
        End Try
    End Sub

    Private Sub SerializaObjetoStringWritter(ArchivocConRuta_ As String, Optional namespaces_ As XmlSerializerNamespaces = Nothing)
        Try
            Dim ns_ As XmlSerializerNamespaces = Nothing
            Select Case _tipoNameSpace
                Case IDocumento.enmTipoNameSpace.SinNameSpace
                    'Para Quitar namespaces se agregan namespaces vacios
                    ns_ = New XmlSerializerNamespaces()
                    ns_.Add("", "")
                Case IDocumento.enmTipoNameSpace.OtrosNameSpaces
                    ns_ = namespaces_
            End Select

            Dim objStringWriter As New StringWriter()
            Dim xsSerializador_ As New XmlSerializer(_objetoFuente.GetType)
            xsSerializador_.Serialize(objStringWriter, _objetoFuente, ns_)

            _documentoEletronico = objStringWriter.ToString

            objStringWriter.Dispose()
            objStringWriter.Close()

        Catch
            _documentoEletronico = Nothing
        End Try
    End Sub

    Private Function ByteArrayToString(ByVal characters_ As Byte(), Encoding_ As Encoding) As String

        'Dim encoding As New UTF8Encoding
        Dim strGenerado As String = Encoding_.GetString(characters_)
        Return strGenerado

    End Function
    Private Function xmlTextWriterAMemoryStream(elTextWriter_ As XmlTextWriter) As MemoryStream
        Return CType(elTextWriter_.BaseStream, MemoryStream)
    End Function
    Public Sub AlmacenaEnFisico() Implements IDocumento.AlmacenaEnFisico
        'If desdeString_ Then
        File.WriteAllText(_rutaFisica, _documentoEletronico)
        'Else
        '    Dim wFile As System.IO.FileStream
        '    Dim memoryStream_ As New MemoryStream()

        '    memoryStream_ = xmlTextWriterAMemoryStream(_documentoEletronico)

        '    wFile = New FileStream(_rutaFisica, FileMode.Create)
        '    wFile.Write(memoryStream_.ToArray, 0, memoryStream_.ToArray.Length)
        '    wFile.Close()
        'End If


    End Sub

#End Region

#Region "Constructores"

    Sub New()

        _formatoDocumento = IDocumento.enmTiposDocumento.SinDefinir
        _tipoNameSpace = IDocumento.enmTipoNameSpace.SinNameSpace
        _metodoCreacion = IDocumento.enmMetodoCreacion.Serializacion
        _objetoFuente = New Object
        _documentoEletronico = Nothing
        _documentoBuilder = New StringBuilder
        _rutaFisica = vbNullString
    End Sub

#End Region

End Class

