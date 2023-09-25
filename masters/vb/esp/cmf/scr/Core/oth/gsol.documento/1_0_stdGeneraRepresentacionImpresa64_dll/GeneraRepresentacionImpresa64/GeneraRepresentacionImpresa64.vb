Imports gsol.BaseDatos.Operaciones
Imports Wma.Exceptions
Imports tagcode.view
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render
Imports System.IO
Imports System.Drawing
Imports System.Xml
Imports System.Threading

Namespace gsol.documento

    Public Class GeneraRepresentacionImpresa64
        Implements IGeneraRepresentacionImpresa



#Region "Atributos"

        Private _ioperacionesCatalogo As OperacionesCatalogo

        Private _espacioTrabajo As EspacioTrabajo

        Private _sistema As Organismo

        Private _estatus As TagWatcher

        Private _i_Cve_Documento As Int32

        Private _t_RutaSalida As String

        Private _t_RutaArchivoExportado As String

        Private _procesarConHilo As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property CveDocumento As Integer Implements IGeneraRepresentacionImpresa.CveDocumento
            Get
                Return _i_Cve_Documento
            End Get
        End Property

        Public Property EspacioTrabajo As IEspacioTrabajo Implements IGeneraRepresentacionImpresa.EspacioTrabajo
            Set(value As IEspacioTrabajo)
                _espacioTrabajo = value
            End Set
            Get
                Return _espacioTrabajo
            End Get
        End Property

        Public ReadOnly Property Estatus As TagWatcher Implements IGeneraRepresentacionImpresa.Estatus
            Get
                Return _estatus
            End Get
        End Property

        Public Property OperacionesCatalogo As IOperacionesCatalogo Implements IGeneraRepresentacionImpresa.OperacionesCatalogo
            Set(value As IOperacionesCatalogo)
                _ioperacionesCatalogo = value
            End Set
            Get
                Return _ioperacionesCatalogo
            End Get
        End Property

        Public Property RutaSalida As String Implements IGeneraRepresentacionImpresa.RutaSalida
            Set(value As String)
                _t_RutaSalida = value
            End Set
            Get
                Return _t_RutaSalida
            End Get
        End Property

        Public Property ProcesarConHilo As Boolean Implements IGeneraRepresentacionImpresa.ProcesarConHilo

            Set(value As Boolean)

                _procesarConHilo = value

            End Set

            Get

                Return _procesarConHilo

            End Get

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _ioperacionesCatalogo = New OperacionesCatalogo

            _espacioTrabajo = New EspacioTrabajo

            _sistema = New Organismo

            _estatus = New TagWatcher

            _estatus.SetOK()

            _procesarConHilo = True

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo,
                ByVal espacioTrabajo_ As EspacioTrabajo)

            Me.New()

            _ioperacionesCatalogo = ioperaciones_

            _espacioTrabajo = espacioTrabajo_

            _estatus = New TagWatcher

            _estatus.SetOK()

            _procesarConHilo = True

        End Sub

#End Region

#Region "Métodos"

        Public Function ProcesarDocumento(ByVal i_Cve_Documento_ As Integer,
                                           ByVal t_RutaArchivo_ As String,
                                           ByVal i_Cve_DivisionMiEmpresa_ As Integer,
                                           ByVal i_TipoProcesable_ As IDocumento.TiposProcesable,
                                           ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                           Optional i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                           Optional i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF) As TagWatcher Implements IGeneraRepresentacionImpresa.ProcesarDocumento

            Dim t_RutaPlantilla_ As String = Nothing

            If File.Exists(t_RutaArchivo_) Then

                Select Case i_TipoProcesable_

                    Case IDocumento.TiposProcesable.XML

                        _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                    Case IDocumento.TiposProcesable.XMLCFDI

                        t_RutaPlantilla_ = ObtenerPlantillaDocumentoCFDI(i_TipoPlantilla_,
                                                                         i_TipoEmisorDocumento_,
                                                                         i_Cve_DivisionMiEmpresa_)

                        If t_RutaPlantilla_ <> Nothing Then

                            Select Case i_TipoPlantilla_

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.FacturaCFDI33

                                    If _procesarConHilo Then

                                        Dim hilo As New Thread(Sub() Me.GenerarRepresentacionFacturaCFDI33(t_RutaArchivo_,
                                                                   t_RutaPlantilla_,
                                                                   i_TipoEmisorDocumento_,
                                                                   i_TipoSalida_,
                                                                   i_Cve_Documento_,
                                                                   i_Cve_DivisionMiEmpresa_))

                                        hilo.Start()

                                        hilo.Join()

                                        While hilo.IsAlive

                                        End While

                                    Else

                                        GenerarRepresentacionFacturaCFDI33(t_RutaArchivo_,
                                                                           t_RutaPlantilla_,
                                                                           i_TipoEmisorDocumento_,
                                                                           i_TipoSalida_,
                                                                           i_Cve_Documento_,
                                                                           i_Cve_DivisionMiEmpresa_)

                                    End If

                                    _estatus.FlagReturned = _t_RutaArchivoExportado

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.NotaCredito

                                    _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.ComplementoPago

                                    If _procesarConHilo Then

                                        Dim hilo As New Thread(Sub() Me.GenerarRepresentacionImpresaComplementoPago33(t_RutaArchivo_,
                                                                   t_RutaPlantilla_,
                                                                   i_TipoEmisorDocumento_,
                                                                   i_TipoSalida_,
                                                                   i_Cve_Documento_,
                                                                   i_Cve_DivisionMiEmpresa_))

                                        hilo.Start()

                                        hilo.Join()

                                        While hilo.IsAlive

                                        End While

                                    Else

                                        GenerarRepresentacionImpresaComplementoPago33(t_RutaArchivo_,
                                                                                      t_RutaPlantilla_,
                                                                                      i_TipoEmisorDocumento_,
                                                                                      i_TipoSalida_,
                                                                                      i_Cve_Documento_,
                                                                                      i_Cve_DivisionMiEmpresa_)

                                    End If

                                    _estatus.FlagReturned = _t_RutaArchivoExportado

                            End Select

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        End If

                    Case IDocumento.TiposProcesable.XMLCFDIComplementoPago

                        t_RutaPlantilla_ = ObtenerPlantillaDocumentoCFDI(i_TipoPlantilla_,
                                                                       i_TipoEmisorDocumento_,
                                                                       i_Cve_DivisionMiEmpresa_)

                        If t_RutaPlantilla_ <> Nothing Then

                            Select Case i_TipoPlantilla_

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.FacturaCFDI33

                                    _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.NotaCredito

                                    _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.ComplementoPago

                                    If _procesarConHilo Then

                                        Dim hilo As New Thread(Sub() Me.GenerarRepresentacionImpresaComplementoPago33(t_RutaArchivo_,
                                                                   t_RutaPlantilla_,
                                                                   i_TipoEmisorDocumento_,
                                                                   i_TipoSalida_,
                                                                   i_Cve_Documento_,
                                                                   i_Cve_DivisionMiEmpresa_))

                                        hilo.Start()

                                        hilo.Join()

                                        While hilo.IsAlive

                                        End While

                                    Else

                                        GenerarRepresentacionImpresaComplementoPago33(t_RutaArchivo_,
                                                                                      t_RutaPlantilla_,
                                                                                      i_TipoEmisorDocumento_,
                                                                                      i_TipoSalida_,
                                                                                      i_Cve_Documento_,
                                                                                      i_Cve_DivisionMiEmpresa_)

                                    End If

                                    _estatus.FlagReturned = _t_RutaArchivoExportado

                            End Select

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        End If

                    Case IDocumento.TiposProcesable.INDEFINIDO

                        _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                End Select

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1104)
                'A

            End If

            Return Estatus

        End Function

        ''' <summary>
        ''' Esta función genera la representación impresa de documento por medio de la clave de documento
        ''' <param name="i_Cve_Documento_"></param>
        ''' <param name="i_TipoPlantilla_"></param>
        ''' <param name="i_TipoEmisorDocumento_"></param>
        ''' <param name="i_TipoSalida_"></param>
        ''' <returns> Retorna TagWatcher </returns>
        ''' <remarks></remarks>
        Public Function ProcesarDocumento(ByVal i_Cve_Documento_ As Integer,
                                         ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                         Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                         Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF) As TagWatcher Implements IGeneraRepresentacionImpresa.ProcesarDocumento

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

            Return Estatus

        End Function

        ''' <summary>
        ''' Esta función genera la representación impresa de objeto fuente
        ''' </summary>
        ''' <param name="o_Fuente_"></param>
        ''' <param name="i_TipoProcesable_"></param>
        ''' <param name="i_TipoPlantilla_"></param>
        ''' <param name="i_TipoEmisorDocumento_"></param>
        ''' <param name="i_TipoSalida_"></param>
        ''' <returns> Retorna TagWatcher </returns>
        ''' <remarks></remarks>
        Public Function ProcesarDocumento(ByVal o_Fuente_ As Object,
                                         ByVal i_TipoProcesable_ As IDocumento.TiposProcesable,
                                         ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                         Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                         Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF) As TagWatcher Implements IGeneraRepresentacionImpresa.ProcesarDocumento

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

            Return Estatus

        End Function

        ''' <summary>
        ''' Esta función genera la representación impresa de un archivo por medio de la ruta
        ''' </summary>
        ''' <param name="t_RutaArchivo_"></param>
        ''' <param name="i_TipoProcesable_"></param>
        ''' <param name="i_TipoPlantilla_"></param>
        ''' <param name="i_TipoEmisorDocumento_"></param>
        ''' <param name="i_TipoSalida_"></param>
        ''' <returns> Retorna TagWatcher </returns>
        ''' <remarks></remarks>
        Public Function ProcesarDocumento(ByVal t_RutaArchivo_ As String,
                                         ByVal i_TipoProcesable_ As IDocumento.TiposProcesable,
                                         ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                         Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                         Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF) As TagWatcher Implements IGeneraRepresentacionImpresa.ProcesarDocumento

            Dim t_RutaPlantilla_ As String = Nothing

            'Dim t_RutaArchivoExportado_ As String = Nothing

            If File.Exists(t_RutaArchivo_) Then

                Select Case i_TipoProcesable_

                    Case IDocumento.TiposProcesable.XML

                        _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                    Case IDocumento.TiposProcesable.XMLCFDI

                        t_RutaPlantilla_ = ObtenerPlantillaDocumentoCFDI(i_TipoPlantilla_, i_TipoEmisorDocumento_)

                        If t_RutaPlantilla_ <> Nothing Then

                            Select Case i_TipoPlantilla_

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.FacturaCFDI33

                                    Dim hilo As New Thread(Sub() Me.GenerarRepresentacionFacturaCFDI33(t_RutaArchivo_,
                                                                                    t_RutaPlantilla_,
                                                                                    i_TipoEmisorDocumento_,
                                                                                    i_TipoSalida_))

                                    hilo.Start()

                                    While hilo.IsAlive

                                        ' Espera a que termine el hilo

                                    End While

                                    _estatus.FlagReturned = _t_RutaArchivoExportado

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.NotaCredito

                                    _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                                Case IGeneraRepresentacionImpresa.TiposPlantillas.ComplementoPago

                                    _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                            End Select

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        End If


                        'If t_RutaPlantilla_ <> Nothing Then

                        '    Select Case i_TipoPlantilla_

                        '        Case IGeneraRepresentacionImpresa.TiposPlantillas.FacturaCFDI33


                        '            GenerarRepresentacionFacturaCFDI33(t_RutaArchivo_,
                        '                                               t_RutaPlantilla_,
                        '                                               i_TipoEmisorDocumento_,
                        '                                               i_TipoSalida_)

                        '            _tagWatcher.FlagReturned = _t_RutaArchivoExportado

                        '        Case IGeneraRepresentacionImpresa.TiposPlantillas.NotaCredito

                        '            _tagWatcher.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        '        Case IGeneraRepresentacionImpresa.TiposPlantillas.ComplementoPago

                        '            _tagWatcher.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        '    End Select

                        'Else

                        '    _tagWatcher.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        'End If

                    Case IDocumento.TiposProcesable.INDEFINIDO

                        _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                End Select

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1104)

            End If

            Return Estatus

        End Function


        ''' <summary>
        ''' Esta función genera el código QR de acuerdo a los parámetros proporcionados
        ''' </summary>
        ''' <param name="t_RutaArchivo_"></param>
        ''' <param name="t_NombreArchivo_"></param>
        ''' <param name="i_TamanoCodigo_"></param>
        ''' <param name="i_DimensionX_"></param>
        ''' <param name="i_DimensionY_"></param>
        ''' <param name="t_CadenaQR_"></param>
        ''' <returns> Retorna la ruta del códigoQR generado </returns>
        ''' <remarks></remarks>
        Private Function GenerarCodigoQR(ByVal t_RutaArchivo_ As String,
                                    ByVal t_NombreArchivo_ As String,
                                    ByVal i_TamanoCodigo_ As Int32,
                                    ByVal i_DimensionX_ As Int32,
                                    ByVal i_DimensionY_ As Int32,
                                    ByVal t_CadenaQR_ As String) As String

            Dim t_RutaCodigoExportado_ As String = Nothing

            Dim qrEncoder_ As QrEncoder

            Dim qrCode_ As QrCode

            Dim render_ As GraphicsRenderer

            Dim stream_ As MemoryStream

            Dim imageTemporal_ As Bitmap

            Dim image_ As Bitmap

            qrEncoder_ = New QrEncoder(ErrorCorrectionLevel.H)

            qrCode_ = New QrCode()

            qrEncoder_.TryEncode(t_CadenaQR_, qrCode_)

            render_ = New GraphicsRenderer(New FixedCodeSize(i_TamanoCodigo_, QuietZoneModules.Zero), Brushes.Black, Brushes.White)

            stream_ = New MemoryStream()

            render_.WriteToStream(qrCode_.Matrix, Drawing.Imaging.ImageFormat.Png, stream_)

            imageTemporal_ = New Bitmap(stream_)

            image_ = New Bitmap(imageTemporal_, New Size(New Point(i_DimensionX_, i_DimensionY_)))

            t_RutaCodigoExportado_ = t_RutaArchivo_ & t_NombreArchivo_ & ".png"

            image_.Save(t_RutaCodigoExportado_, Drawing.Imaging.ImageFormat.Png)

            If Not File.Exists(t_RutaCodigoExportado_) Then

                t_RutaCodigoExportado_ = Nothing

            End If

            Return t_RutaCodigoExportado_

        End Function

        ''' <summary>
        ''' Esta función genera la representación impresa para los documentos Tipo Factura CFDI 3.3
        ''' </summary>
        ''' <param name="t_RutaArchivo_"></param>
        ''' <param name="t_RutaPlantilla_"></param>
        ''' <param name="i_TipoEmisorDocumento_"></param>
        ''' <param name="i_TipoSalida_"></param>
        ''' <returns> Retorna la ruta del archivo exportado </returns>
        ''' <remarks></remarks>
        Private Sub GenerarRepresentacionFacturaCFDI33(ByVal t_RutaArchivo_ As String,
                                                       ByVal t_RutaPlantilla_ As String,
                                                       Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                                       Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF,
                                                       Optional ByVal i_Cve_Documento_ As Integer = 0,
                                                       Optional ByVal i_Cve_DivisionMiEmpresa As Integer = 0)


            Dim t_NombreArchivo_ As String = Nothing

            Dim t_ExtensionArchivo_ As String = Nothing

            Dim t_TotalComprobante_ As String = Nothing

            Dim t_SelloComprobante_ As String = Nothing

            Dim t_VersionCFDI_ As String = Nothing

            Dim t_RFCEmisor_ As String = Nothing

            Dim t_NombreEmisor_ As String = Nothing

            Dim t_RFCReceptor_ As String = Nothing

            Dim t_UUID_ As String = Nothing

            Dim licencia_ As New LicenciasViewXML

            Dim vXML_ As New ViewXML

            Dim t_CadenaQR_ As String = Nothing

            Dim t_RutaCodigoQR_ As String = Nothing

            Dim fileInfo_ = New FileInfo(t_RutaArchivo_)

            If fileInfo_.Length > 0 Then

                t_NombreArchivo_ = Path.GetFileNameWithoutExtension(t_RutaArchivo_)

                t_ExtensionArchivo_ = Path.GetExtension(t_RutaArchivo_)

                ' Verifica el archivo fuente sea un xml

                If (t_ExtensionArchivo_ = ".xml" Or t_ExtensionArchivo_ = ".XML" Or t_ExtensionArchivo_ = ".Xml") Then

                    If _t_RutaSalida = Nothing Or Trim(_t_RutaSalida) = "" Then

                        _t_RutaSalida = Path.GetDirectoryName(t_RutaArchivo_) & "/"

                    End If

                    ' 27/Septiembre/2017, CFDi 3.3
                    licencia_.Licencia("WS19Fld3+b/JM72J+FuQaYLeB4iCUQDAklIEvRDprQtQTBKQtkQj4Q==")

                    vXML_.DecimalesCantidad(ViewXML.opDecimales.Automatico)

                    vXML_.DecimalesImportes(ViewXML.opDecimales.Automatico)

                    ' Obtiene el Nodo Addenda generado en el draft y lo coloca en el XML del CFDi Original

                    Dim docXmlFile_ As XmlDocument = New XmlDocument()

                    docXmlFile_.Load(t_RutaArchivo_)

                    Select Case i_TipoEmisorDocumento_

                        Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario

                            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Terceros

                            ' Lee el XML y obtiene los datos necesarios para la generación del CodigoQR

                            Dim nodoComprobante_ As XmlNode = docXmlFile_.DocumentElement.ParentNode.Item("cfdi:Comprobante")

                            If nodoComprobante_.Name.Contains("Comprobante") Then

                                t_TotalComprobante_ = nodoComprobante_.Attributes.GetNamedItem("Total").Value

                                t_SelloComprobante_ = nodoComprobante_.Attributes.GetNamedItem("Sello").Value

                                t_VersionCFDI_ = nodoComprobante_.Attributes.GetNamedItem("Version").Value

                            End If

                            If docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Emisor").Count > 0 Then

                                t_RFCEmisor_ = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Emisor").
                                    Item(0).Attributes.GetNamedItem("Rfc").Value

                                Try

                                    t_NombreEmisor_ = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Emisor").
                                        Item(0).Attributes.GetNamedItem("Nombre").Value

                                Catch ex As Exception

                                    t_NombreEmisor_ = "SIN DEFINIR"

                                End Try

                                vXML_.CampoExtra1(t_NombreEmisor_.ToUpper())

                            End If


                            If docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Receptor").Count > 0 Then

                                t_RFCReceptor_ = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Receptor").
                                    Item(0).Attributes.GetNamedItem("Rfc").Value

                            End If

                            If docXmlFile_.DocumentElement.GetElementsByTagName("tfd:TimbreFiscalDigital").Count > 0 Then

                                t_UUID_ = docXmlFile_.DocumentElement.GetElementsByTagName("tfd:TimbreFiscalDigital").
                                    Item(0).Attributes.GetNamedItem("UUID").Value

                            End If


                            ' Genera CódigoQR válido
                            t_CadenaQR_ = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?&id=" & t_UUID_ & _
                                "&re=" & t_RFCEmisor_ & "&rr=" & t_RFCReceptor_ & "&tt=" & t_TotalComprobante_ & _
                                "&fe=" & Mid(t_SelloComprobante_, (Len(t_SelloComprobante_) - 7), 8)

                            t_RutaCodigoQR_ = GenerarCodigoQR(_t_RutaSalida, t_UUID_, 400, 325, 325, t_CadenaQR_)

                            If File.Exists(t_RutaCodigoQR_) Then

                                vXML_.CodigoDeBarrasQR(t_RutaCodigoQR_)

                            End If

                    End Select

                    vXML_.FormatoPersonalizado(t_RutaPlantilla_)

                    ' Valida que se trate de un CFDI 3.3
                    If t_VersionCFDI_ = "3.3" Then

                        Select Case i_TipoSalida_

                            Case IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF

                                _t_RutaArchivoExportado = _t_RutaSalida & t_NombreArchivo_ & ".pdf"

                                vXML_.Exportar(t_RutaArchivo_, ViewXML.opExportar.PDF, _t_RutaArchivoExportado)

                            Case IGeneraRepresentacionImpresa.TiposSalida.ExportarJPG

                                _t_RutaArchivoExportado = _t_RutaSalida & t_NombreArchivo_ & ".jpg"

                                vXML_.Exportar(t_RutaArchivo_, ViewXML.opExportar.JPEG, _t_RutaArchivoExportado)

                            Case IGeneraRepresentacionImpresa.TiposSalida.ExportarHTML

                                _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                            Case IGeneraRepresentacionImpresa.TiposSalida.ExportarXLS

                                _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                            Case IGeneraRepresentacionImpresa.TiposSalida.VistaPreviaImpresion

                                _t_RutaArchivoExportado = _t_RutaSalida & t_NombreArchivo_ & ".pdf"

                                vXML_.Exportar(t_RutaArchivo_, ViewXML.opExportar.PDF, _t_RutaArchivoExportado)

                                vXML_.VistaPrevia(t_RutaArchivo_)

                        End Select

                    Else

                        _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1103)

                    End If


                    If t_RutaCodigoQR_ <> Nothing Then

                        If File.Exists(t_RutaCodigoQR_) Then

                            File.Delete(t_RutaCodigoQR_) ' Se elimina la imagen del codigoQR

                        End If

                    End If


                    ' Verifica si existe el archivo exportado
                    If _t_RutaArchivoExportado <> Nothing Then

                        If Not File.Exists(_t_RutaArchivoExportado) Then

                            _t_RutaArchivoExportado = Nothing

                            _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1101)

                        Else

                            If i_Cve_Documento_ > 0 Then

                                Dim ioSoportePDF_ = New OperacionesCatalogo

                                ioSoportePDF_.EspacioTrabajo = _ioperacionesCatalogo.EspacioTrabajo

                                ioSoportePDF_ = _sistema.ConsultaModulo(ioSoportePDF_.EspacioTrabajo,
                                                                        "DigitalizacionSoportesSinPDF",
                                                                        " ")
                                With ioSoportePDF_

                                    .TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                                    .CampoPorNombre("i_Cve_Documento") = i_Cve_Documento_

                                    .CampoPorNombre("i_EstatusRepresentacionImpresa") = 1

                                    .EditaCampoPorNombre("t_DirectorioArchivo").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                                    .EditaCampoPorNombre("i_Cve_DivisionMiEmpresa").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                                    If Not (ioSoportePDF_.Modificar(i_Cve_Documento_) = IOperacionesCatalogo.EstadoOperacion.COk) Then

                                        'Error de no modificación

                                    End If

                                End With


                            End If

                        End If

                    End If

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1102)

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1102)

            End If

        End Sub

        Private Sub GenerarRepresentacionImpresaComplementoPago33(ByVal t_RutaArchivo_ As String,
                                                                 ByVal t_RutaPlantilla_ As String,
                                                                 Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                                                 Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF,
                                                                 Optional ByVal i_Cve_Documento_ As Integer = 0,
                                                                 Optional ByVal i_Cve_DivisionMiEmpresa As Integer = 0)
            Dim lic As New LicenciasViewXML

            lic.Licencia("WS19Fld3+b/JM72J+FuQaYLeB4iCUQDAklIEvRDprQtQTBKQtkQj4Q==")

            Dim vp As New ViewXML

            Dim t_NombreArchivo_ As String = Nothing

            Dim t_ExtensionArchivo_ As String = Nothing

            't_RutaArchivo_ = "C:\Users\alexishg\Downloads\Ejemplos\RKU18-GRKCP-02346.xml"

            t_NombreArchivo_ = Path.GetFileNameWithoutExtension(t_RutaArchivo_)

            t_ExtensionArchivo_ = Path.GetExtension(t_RutaArchivo_)

            vp.DecimalesCantidad(ViewXML.opDecimales.TresDecimales)

            vp.DecimalesImportes(ViewXML.opDecimales.Automatico)

            Dim t_TotalComprobante_ = Nothing

            Dim t_SelloComprobante_ = Nothing

            Dim t_VersionCFDI_ = Nothing

            Dim t_RFCEmisor_ = Nothing

            Dim t_RFCReceptor_ = Nothing

            Dim t_NombreEmisor_ = Nothing

            Dim t_UUID_ = Nothing

            Dim IdDocumento_ As String = Nothing

            Dim Folio_ As String = Nothing

            Dim MonedaDR_ As String = Nothing

            Dim TipoCambioDR_ As String = Nothing

            Dim ImpSaldoAnt_ As String = Nothing

            Dim ImpSaldoInsoluto_ As String = Nothing

            Dim ImpPagado_ As String = Nothing

            Dim MetodoDePagoDR_ As String = Nothing

            Dim Parcialidad_ As String = Nothing

            Dim TotalPagado_ As Decimal = 0

            Dim DatosPago1_ As String = Nothing

            Dim t_RutaCodigoQR_ As String = Nothing

            If (t_ExtensionArchivo_ = ".xml" Or t_ExtensionArchivo_ = ".XML") Then

                If _t_RutaSalida = Nothing Or Trim(_t_RutaSalida) = "" Then

                    _t_RutaSalida = Path.GetDirectoryName(t_RutaArchivo_) & "\"

                End If

                Dim docXmlFile_ As XmlDocument = New XmlDocument()

                docXmlFile_.Load(t_RutaArchivo_)



                Select Case i_TipoEmisorDocumento_

                    Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario

                        _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                    Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Terceros

                        ' Lee el XML y obtiene los datos necesarios para la generación del CodigoQR

                        Dim nodoComprobante_ As XmlNode = docXmlFile_.DocumentElement.ParentNode.Item("cfdi:Comprobante")

                        If nodoComprobante_.Name.Contains("Comprobante") Then

                            t_TotalComprobante_ = nodoComprobante_.Attributes.GetNamedItem("Total").Value

                            t_SelloComprobante_ = nodoComprobante_.Attributes.GetNamedItem("Sello").Value

                            t_VersionCFDI_ = nodoComprobante_.Attributes.GetNamedItem("Version").Value

                        End If

                        If docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Emisor").Count > 0 Then

                            t_RFCEmisor_ = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Emisor").
                                Item(0).Attributes.GetNamedItem("Rfc").Value

                            Try

                                t_NombreEmisor_ = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Emisor").
                                    Item(0).Attributes.GetNamedItem("Nombre").Value

                            Catch ex As Exception

                                t_NombreEmisor_ = "SIN DEFINIR"

                            End Try

                        End If


                        If docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Receptor").Count > 0 Then

                            t_RFCReceptor_ = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:Receptor").
                                Item(0).Attributes.GetNamedItem("Rfc").Value

                        End If

                        If docXmlFile_.DocumentElement.GetElementsByTagName("tfd:TimbreFiscalDigital").Count > 0 Then

                            t_UUID_ = docXmlFile_.DocumentElement.GetElementsByTagName("tfd:TimbreFiscalDigital").
                                Item(0).Attributes.GetNamedItem("UUID").Value

                        End If

                        Dim nodoPagos_ As XmlNode = docXmlFile_.DocumentElement.GetElementsByTagName("pago10:Pagos").Item(0)

                        For Each nodoPago_ As XmlNode In nodoPagos_

                            If DatosPago1_ = Nothing Then

                                Try

                                    'DatosPago1_ = "Fecha pago: " & nodoPago_.Item(0).Attributes.GetNamedItem("FechaPago").Value.ToString

                                    DatosPago1_ = "Fecha pago: " & nodoPago_.Attributes.GetNamedItem("FechaPago").Value.ToString()

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          FormaPago: " & nodoPago_.Attributes.GetNamedItem("FormaDePagoP").Value.ToString

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          Moneda Pago: " & nodoPago_.Attributes.GetNamedItem("MonedaP").Value.ToString

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          Tipo de cambio: " & nodoPago_.Attributes.GetNamedItem("TipoCambioP").Value.ToString

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          Monto Pago: " & FormatCurrency(nodoPago_.Attributes.GetNamedItem("Monto").Value.ToString, 2)

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          Num Operacion: " & nodoPago_.Attributes.GetNamedItem("NumOperacion").Value.ToString

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          CtaOrdenante: " & nodoPago_.Attributes.GetNamedItem("CtaOrdenante").Value.ToString

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          RFCEmisorCtaOrd: " & nodoPago_.Attributes.GetNamedItem("RfcEmisorCtaOrd").Value.ToString

                                Catch ex As Exception

                                End Try

                                Try

                                    DatosPago1_ = DatosPago1_ & "          NomBancoOrdExt: " & nodoPago_.Attributes.GetNamedItem("NomBancoOrdExt").Value.ToString

                                Catch ex As Exception

                                End Try

                            End If

                            For Each nodoDoctoRelacinado_ As XmlNode In nodoPago_

                                If IdDocumento_ Is Nothing Then

                                    Try

                                        MetodoDePagoDR_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("MetodoDePagoDR").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        IdDocumento_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("IdDocumento").Value.ToString & _
                                            "(" & nodoDoctoRelacinado_.Attributes.GetNamedItem("Serie").Value.ToString & _
                                            "-" & nodoDoctoRelacinado_.Attributes.GetNamedItem("Folio").Value.ToString & ")"

                                    Catch ex As Exception

                                        IdDocumento_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("IdDocumento").Value.ToString & _
                                                    "(" & nodoDoctoRelacinado_.Attributes.GetNamedItem("Folio").Value.ToString & ")"

                                    End Try

                                    Try

                                        Folio_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("Folio").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        MonedaDR_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("MonedaDR").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        TipoCambioDR_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("TipoCambioDR").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        ImpSaldoAnt_ = FormatCurrency(nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpSaldoAnt").Value.ToString, 2)

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        ImpSaldoInsoluto_ = FormatCurrency(nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpSaldoInsoluto").Value.ToString, 2)

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        ImpPagado_ = FormatCurrency(nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpPagado").Value.ToString, 2)

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        Parcialidad_ = nodoDoctoRelacinado_.Attributes.GetNamedItem("NumParcialidad").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                Else

                                    Try

                                        MetodoDePagoDR_ = MetodoDePagoDR_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("MetodoDePagoDR").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        IdDocumento_ = IdDocumento_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("IdDocumento").Value.ToString & _
                                            "(" & nodoDoctoRelacinado_.Attributes.GetNamedItem("Serie").Value.ToString & _
                                            "-" & nodoDoctoRelacinado_.Attributes.GetNamedItem("Folio").Value.ToString & ")"

                                    Catch ex As Exception

                                        IdDocumento_ = IdDocumento_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("IdDocumento").Value.ToString & _
                                                            "(" & nodoDoctoRelacinado_.Attributes.GetNamedItem("Folio").Value.ToString & ")"

                                    End Try

                                    Try

                                        Folio_ = Folio_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("Folio").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        MonedaDR_ = MonedaDR_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("MonedaDR").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        TipoCambioDR_ = TipoCambioDR_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("TipoCambioDR").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        ImpSaldoAnt_ = ImpSaldoAnt_ & vbCrLf & FormatCurrency(nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpSaldoAnt").Value.ToString, 2)

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        ImpSaldoInsoluto_ = ImpSaldoInsoluto_ & vbCrLf & FormatCurrency(nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpSaldoInsoluto").Value.ToString, 2)

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        ImpPagado_ = ImpPagado_ & vbCrLf & FormatCurrency(nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpPagado").Value.ToString, 2)

                                    Catch ex As Exception

                                    End Try

                                    Try

                                        Parcialidad_ = Parcialidad_ & vbCrLf & nodoDoctoRelacinado_.Attributes.GetNamedItem("NumParcialidad").Value.ToString

                                    Catch ex As Exception

                                    End Try

                                End If

                                TotalPagado_ += nodoDoctoRelacinado_.Attributes.GetNamedItem("ImpPagado").Value.ToString

                            Next

                        Next

                        TotalPagado_ = 0

                        vp.CampoExtra1(IdDocumento_)

                        vp.CampoExtra2(MonedaDR_)

                        vp.CampoExtra3(Parcialidad_)

                        vp.CampoExtra4(IIf(IsNothing(TipoCambioDR_), 0, TipoCambioDR_))

                        vp.CampoExtra5(ImpSaldoAnt_)

                        vp.CampoExtra6(ImpPagado_)

                        vp.CampoExtra7(ImpSaldoInsoluto_)

                        vp.CampoExtra8(MetodoDePagoDR_)

                        vp.CampoExtra9(DatosPago1_)

                        Dim UUIDDocumentoRelacionado_ = ""

                        If docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:CfdiRelacionados").Count > 0 Then

                            Dim nodoCFDIRelacionados_ As XmlNode = docXmlFile_.DocumentElement.GetElementsByTagName("cfdi:CfdiRelacionados").Item(0)

                            For Each nodoCFDI_ As XmlNode In nodoCFDIRelacionados_

                                Try

                                    If UUIDDocumentoRelacionado_ = "" Then

                                        UUIDDocumentoRelacionado_ = "UUID: " & nodoCFDI_.Attributes.GetNamedItem("UUID").Value.ToString()

                                    Else

                                        UUIDDocumentoRelacionado_ = UUIDDocumentoRelacionado_ & "       UUID: " & nodoCFDI_.Attributes.GetNamedItem("UUID").Value.ToString()

                                    End If

                                Catch ex As Exception

                                End Try

                            Next

                        End If

                        If UUIDDocumentoRelacionado_ <> Nothing And UUIDDocumentoRelacionado_ <> "" Then

                            vp.CampoExtra10(UUIDDocumentoRelacionado_)

                        End If

                        Dim t_CadenaQR_ As String = ""

                        t_CadenaQR_ = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?&id=" & t_UUID_ & _
                                            "&re=" & t_RFCEmisor_ & "&rr=" & t_RFCReceptor_ & "&tt=0" & _
                                            "&fe=" & Mid(t_SelloComprobante_, (Len(t_SelloComprobante_) - 7), 8)

                        t_RutaCodigoQR_ = GenerarCodigoQR(_t_RutaSalida, t_UUID_, 400, 325, 325, t_CadenaQR_)

                        If t_RutaCodigoQR_ <> Nothing Then

                            If File.Exists(t_RutaCodigoQR_) Then

                                vp.CodigoDeBarrasQR(t_RutaCodigoQR_)

                            End If

                        End If


                End Select


                vp.FormatoPersonalizado(t_RutaPlantilla_)

                If t_VersionCFDI_ = "3.3" Then

                    Select Case i_TipoSalida_

                        Case IGeneraRepresentacionImpresa.TiposSalida.ExportarPDF

                            _t_RutaArchivoExportado = _t_RutaSalida & t_NombreArchivo_ & ".pdf"

                            vp.Exportar(t_RutaArchivo_, ViewXML.opExportar.PDF, _t_RutaArchivoExportado)

                        Case IGeneraRepresentacionImpresa.TiposSalida.ExportarJPG

                            _t_RutaArchivoExportado = _t_RutaSalida & t_NombreArchivo_ & ".jpg"

                            vp.Exportar(t_RutaArchivo_, ViewXML.opExportar.JPEG, _t_RutaArchivoExportado)

                        Case IGeneraRepresentacionImpresa.TiposSalida.ExportarHTML

                            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        Case IGeneraRepresentacionImpresa.TiposSalida.ExportarXLS

                            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                        Case IGeneraRepresentacionImpresa.TiposSalida.VistaPreviaImpresion

                            _t_RutaArchivoExportado = _t_RutaSalida & t_NombreArchivo_ & ".pdf"

                            vp.Exportar(t_RutaArchivo_, ViewXML.opExportar.PDF, _t_RutaArchivoExportado)

                            vp.VistaPrevia(t_RutaArchivo_)

                    End Select

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1103)

                End If

                If t_RutaCodigoQR_ <> Nothing Then

                    If File.Exists(t_RutaCodigoQR_) Then

                        File.Delete(t_RutaCodigoQR_) ' Se elimina la imagen del codigoQR

                    End If

                End If

                ' Verifica si existe el archivo exportado
                If _t_RutaArchivoExportado <> Nothing Then

                    If Not File.Exists(_t_RutaArchivoExportado) Then

                        _t_RutaArchivoExportado = Nothing

                        _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1101)

                    Else

                        If i_Cve_Documento_ > 0 Then

                            Dim ioSoportePDF_ = New OperacionesCatalogo

                            ioSoportePDF_.EspacioTrabajo = _ioperacionesCatalogo.EspacioTrabajo

                            ioSoportePDF_ = _sistema.ConsultaModulo(ioSoportePDF_.EspacioTrabajo,
                                                                    "ComplementosPagoTercerosSinPDF",
                                                                    " ")
                            With ioSoportePDF_

                                .TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                                .CampoPorNombre("i_Cve_Documento") = i_Cve_Documento_

                                .CampoPorNombre("i_EstatusRepresentacionImpresa") = 1

                                .EditaCampoPorNombre("t_DirectorioArchivo").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                                .EditaCampoPorNombre("i_Cve_DivisionMiEmpresa").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                                If Not (ioSoportePDF_.Modificar(i_Cve_Documento_) = IOperacionesCatalogo.EstadoOperacion.COk) Then

                                    'Error de no modificación

                                End If

                            End With


                        End If

                    End If

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C5_012_1102)

            End If

        End Sub

        ''' <summary>
        ''' Esta función obtiene la ruta de la plantilla a utilizar para los documentos CFDI, de acuerdo al tipo de plantilla y al emisor del documento
        ''' </summary>
        ''' <param name="i_TipoPlantilla_"></param>
        ''' <param name="i_TipoEmisorDocumento_"></param>
        ''' <returns> Retorna la ruta de la plantilla </returns>
        ''' <remarks></remarks>
        Private Function ObtenerPlantillaDocumentoCFDI(ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                                       Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario,
                                                       Optional ByVal i_Cve_DivisionMiEmpresa_ As Integer = 0) As String

            Dim t_RutaPlantilla_ As String = Nothing

            Dim t_CampoPlantila_ As String = Nothing

            Dim ioperacionesPlantillas_ = New OperacionesCatalogo

            Select Case i_TipoPlantilla_

                Case IGeneraRepresentacionImpresa.TiposPlantillas.FacturaCFDI33

                    Select Case i_TipoEmisorDocumento_

                        Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario

                            t_CampoPlantila_ = "t_RutaConfiguracionComprobantesCFDi"

                        Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Terceros

                            t_CampoPlantila_ = "t_RutaConfiguracionComprobantesCFDiTerceros"

                    End Select

                Case IGeneraRepresentacionImpresa.TiposPlantillas.NotaCredito

                    Select Case i_TipoEmisorDocumento_

                        Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Propietario

                            t_CampoPlantila_ = "t_RutaConfiguracionComprobantesCFDiNotaCredito"

                    End Select

                Case IGeneraRepresentacionImpresa.TiposPlantillas.ComplementoPago

                    Select Case i_TipoEmisorDocumento_

                        Case IGeneraRepresentacionImpresa.TiposEmisorDocumento.Terceros

                            t_CampoPlantila_ = "t_RutaConfiguracionComplementosPagoTerceros"

                    End Select

            End Select

            If t_CampoPlantila_ <> Nothing Then

                If i_Cve_DivisionMiEmpresa_ > 0 Then

                    ioperacionesPlantillas_ = _sistema.ConsultaModulo(_ioperacionesCatalogo.EspacioTrabajo,
                                             "MisEmpresas",
                                             " AND i_Cve_DivisionMiEmpresa = " & i_Cve_DivisionMiEmpresa_)

                Else

                    ioperacionesPlantillas_ = _sistema.ConsultaModulo(_ioperacionesCatalogo.EspacioTrabajo,
                                              "MisEmpresas",
                                              " AND i_Cve_DivisionMiEmpresa = " & _
                                                     _ioperacionesCatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria)

                End If

                If _sistema.TieneResultados(ioperacionesPlantillas_) Then

                    For Each fila_ As DataRow In ioperacionesPlantillas_.Vista.Tables(0).Rows

                        t_RutaPlantilla_ = _sistema.ValidarVacios(fila_.Item(t_CampoPlantila_).ToString,
                                                                  Organismo.VerificarTipoDatoDBNULL.Cadena)

                    Next

                End If

            End If

            Return t_RutaPlantilla_

        End Function

#End Region

    End Class

End Namespace

