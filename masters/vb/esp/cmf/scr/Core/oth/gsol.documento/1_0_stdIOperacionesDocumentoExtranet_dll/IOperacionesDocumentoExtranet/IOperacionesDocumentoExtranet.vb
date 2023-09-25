Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones

Namespace gsol.documento

    Public Interface IOperacionesDocumentoExtranet

#Region "Propiedades"

        Property Operaciones As IOperacionesCatalogo

        ReadOnly Property Estatus As TagWatcher

        ReadOnly Property DocumentosExtranet As List(Of DocumentoExtranet)

#End Region

#Region "Enum"

        Enum TipoClaseDocumento
            SinDocumentosSoporte = 0
            DocumentosSoporte = 1
            ComplementosPago = 2
            ComplementosPagoTerceros = 3
            ClaveDocumento = 4
        End Enum

        Enum TiposDocumentoExtranet
            Operativo
            Administrativo
            DocumentosMaestroDocumentos
            DocumentosMaestroDocumentosZIP
            ClaveDocumentosMaestroDocumentos
        End Enum

        Enum OperacionesDocumentoExtranetSiNo
            No = 0
            Si = 1
        End Enum

        Enum TiposArchivosEsperado
            PDF = 0
            XML = 1
            ZIP = 2
            Todos = 3
        End Enum

#End Region

#Region "Metodos"

        Function ObtenerDocumento(ByVal documentos_ As List(Of DocumentoExtranet),
                                  Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) As TagWatcher

        Function ObtenerDocumento(ByVal documento_ As DocumentoExtranet,
                                  Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) As TagWatcher

        Function ObtenerDocumento(ByVal claves_ As List(Of Int64)) As TagWatcher

#End Region

    End Interface

    Public Class DocumentoExtranet

#Region "Atributos"

        Private _clave As Integer

        Private _nombreTecnicoBusqueda As String

        Private _valorBusqueda As String

        Private _tokenBusqueda As String

        Private _folio As String

        Private _rutaCompleta As String

        Private _tipoArchivoEsperado As IOperacionesDocumentoExtranet.TiposArchivosEsperado

        Private _documentoEncontrado As IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo

        Private _consultarDocumentosSoporteExtranet As IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo

        Public _documentosSoporteExtranet As List(Of DocumentoExtranet)

        Private _tipoDocumentoExtranet As IOperacionesDocumentoExtranet.TiposDocumentoExtranet

        Private _ClaveTipoDocumentosDescargables As String

        Private _consultarTipoClaseDocumento As IOperacionesDocumentoExtranet.TipoClaseDocumento

#End Region

#Region "Propiedades"

        Property ConsultarTipoClaseDocumento As IOperacionesDocumentoExtranet.TipoClaseDocumento

            Get

                Return _consultarTipoClaseDocumento

            End Get

            Set(value As IOperacionesDocumentoExtranet.TipoClaseDocumento)

                _consultarTipoClaseDocumento = value

            End Set

        End Property

        Property Clave As Integer

            Get

                Return _clave

            End Get

            Set(value As Integer)

                _clave = value

            End Set

        End Property

        ReadOnly Property NombreTecnicoBusqueda As String

            Get

                Return _nombreTecnicoBusqueda

            End Get

        End Property

        Property ValorBusqueda As String

            Get

                Return _valorBusqueda

            End Get

            Set(value As String)

                _valorBusqueda = value

            End Set

        End Property

        ReadOnly Property TokenBusqueda As String

            Get

                Return _tokenBusqueda

            End Get

        End Property

        Property Folio As String

            Get

                Return _folio

            End Get

            Set(value As String)

                _folio = value

            End Set

        End Property

        Property RutaCompleta As String

            Get

                Return _rutaCompleta

            End Get

            Set(value As String)

                _rutaCompleta = value

            End Set

        End Property

        Property TipoArchivoEsperado As IOperacionesDocumentoExtranet.TiposArchivosEsperado

            Get

                Return _tipoArchivoEsperado

            End Get

            Set(value As IOperacionesDocumentoExtranet.TiposArchivosEsperado)

                _tipoArchivoEsperado = value

            End Set

        End Property

        Property DocumentoEncontrado As IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo

            Get

                Return _documentoEncontrado

            End Get

            Set(value As IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo)

                _documentoEncontrado = value

            End Set

        End Property

        Property ConsultarDocumentosSoporteExtranet As IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo

            Get

                Return _consultarDocumentosSoporteExtranet

            End Get

            Set(value As IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo)

                _consultarDocumentosSoporteExtranet = value

            End Set

        End Property

        Property DocumentosSoporteExtranet As List(Of DocumentoExtranet)

            Get

                Return _documentosSoporteExtranet

            End Get

            Set(value As List(Of DocumentoExtranet))

                _documentosSoporteExtranet = value

            End Set

        End Property

        Property TipoDocumentoExtranet As IOperacionesDocumentoExtranet.TiposDocumentoExtranet

            Get

                Return _tipoDocumentoExtranet

            End Get

            Set(value As IOperacionesDocumentoExtranet.TiposDocumentoExtranet)

                _tipoDocumentoExtranet = value

                Select Case value

                    Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.Operativo

                        _nombreTecnicoBusqueda = "t_NumeroReferencia"

                        _tokenBusqueda = "DigitalizacionArchivoEspecialAntigua"

                    Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.Administrativo

                        _nombreTecnicoBusqueda = "i_Cve_Factura"

                        _tokenBusqueda = "DigitalizacionFacturas"

                    Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.ClaveDocumentosMaestroDocumentos

                        _nombreTecnicoBusqueda = "i_Cve_Documento"

                        _tokenBusqueda = "DigitalizacionMaestroDocumentosKBWeb"

                    Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.DocumentosMaestroDocumentos

                        _nombreTecnicoBusqueda = "t_NombreDocumento"

                        _tokenBusqueda = "DigitalizacionMaestroDocumentosKBWeb"

                    Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.DocumentosMaestroDocumentosZIP

                        _nombreTecnicoBusqueda = "t_NumeroReferencia"

                        _tokenBusqueda = "DigitalizacionArchivoEspecialAntigua"

                End Select

            End Set

        End Property

        Property TiposDocumentosDescargables As String

            Get

                Return _ClaveTipoDocumentosDescargables

            End Get

            Set(value As String)

                _ClaveTipoDocumentosDescargables = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _clave = 0

            _nombreTecnicoBusqueda = Nothing

            _valorBusqueda = Nothing

            _tokenBusqueda = Nothing

            _folio = Nothing

            _rutaCompleta = Nothing

            _consultarDocumentosSoporteExtranet = IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo.No

            _documentosSoporteExtranet = New List(Of DocumentoExtranet)

            _documentoEncontrado = IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo.No

            _tipoArchivoEsperado = IOperacionesDocumentoExtranet.TiposArchivosEsperado.PDF

            _consultarTipoClaseDocumento = IOperacionesDocumentoExtranet.TipoClaseDocumento.ClaveDocumento

        End Sub

#End Region

    End Class


End Namespace
