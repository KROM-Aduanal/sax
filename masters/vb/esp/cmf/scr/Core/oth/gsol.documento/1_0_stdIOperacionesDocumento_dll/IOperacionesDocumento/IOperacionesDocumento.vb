Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones

Namespace gsol.documento

    Public Interface IOperacionesDocumento

#Region "Propiedades"

        Property Operaciones As IOperacionesCatalogo

        ReadOnly Property Estatus As TagWatcher

        Property Documento As Documento

#End Region

#Region "Metodos"

        Sub ProcesarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

        Sub CargarRepositorios()

        Sub CargarPlantilla()

        Sub ValidarCaracteristicasPlantilla()

        Sub ValidarDocumentosDuplicados()

        Sub CrearDirectorio()

        Sub InsertarRegistros()

        Sub GuardarDocumento()

        Sub InsertarRegistroVinculacion(Optional ByVal claveModulo_ As Integer = Nothing)

        Sub InsertarRegistrosCaracteristicas()

        Sub BuscarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

        Sub BuscarPrivilegioUsuario(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

        Sub BuscarPrivilegioUsuario(ByVal ClaveDocumento_ As Integer, Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

        Sub CargarCaracteristicasConInformacion()

        Sub CargarCaracteristicasVisorMaestroDocumentos()

        Sub ModificarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

        Sub ModificarRegistros()

        Sub ModificarRegistrosCaracteristicas()

        Sub EliminarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

#End Region

    End Interface

    Public Class Documento

#Region "Enum"

        Enum VersionesCFDI
            NoAplica = Nothing
            Tres = 3
            TresDos = 3.2
            TresTres = 3.3
        End Enum

        Enum ConsultableClientes
            Si = 1
            No = 0
        End Enum

        Enum EstatusDocumento
            SinDefinir = 0
            Habilitado = 1
            Deshabilitado = 2
            Eliminado = 3
            Dañado = 4
        End Enum

        Enum EstatusLiquidacion
            NoAplica = Nothing
            Normal = 1
            Pagado = 2
            Parcialidad = 3
        End Enum

        Enum TiposArchivo
            NoDefinido = 0
            XML = 1
            PDF = 2
            JUL = 3
            JPEG = 4
            PNG = 5
            TIFF = 6
            DOC = 7
            DOCX = 8
            XLS = 9
            XLSX = 10
            PPT = 11
            PPS = 12
            PPTX = 13
            PPSX = 14
            RTF = 15
            CSV = 16
            TXT = 17
            BMP = 18
            JPG = 19
        End Enum

        Enum TiposPrivilegios
            NoIdentificado = 0
            Insertar = 1
            Modificar = 2
            Consultar = 3
            Administrador = 4
        End Enum

        Enum TiposVinculacion
            NoDefinidia = 0
            Referencia = 1
            Factura = 2
            CorreoElectronico = 3
        End Enum

#End Region

#Region "Atributos"

        'Atributos de la extensión de documentos

        Private _clave As Integer

        Private _urlPublico As String

        Private _nombreDocumento As String

        Private _tipoArchivo As TiposArchivo

        Private _claveTipoDocumento As Integer

        Private _tipoDocumento As String

        Private _documentoConsultableCliente As ConsultableClientes

        Private _fechaRegistro As DateTime

        Private _plantillaDocumento As PlantillaDocumento

        Private _claveRepositorioDigital As Integer

        Private _claveDirectorio As Integer

        Private _usuario As Integer

        Private _claveEstado As Integer

        Private _claveEstatus As Integer

        Private _claveDivisionMiEmpresa As Integer

        'Atributos del registro de documentos

        Private _rutaDocumentoOrigen As String

        Private _statusDocumento As EstatusDocumento

        Private _folioDocumento As String

        Private _documento As String

        Private _versionCFDI As VersionesCFDI

        Private _statusLiquidacion As EstatusLiquidacion

        'Atributos auxiliares

        Private _claveDocumentoPadre As Integer

        Private _nombreDocumentoOriginal As String

        Private _rutaDocumento As String

        Private _rutaDocumentoCompleto As String

        Private _rutaDocumentoContingencia As String

        Private _rutaDocumentoContingenciaCompleto As String

        Private _guardarDocumento As Boolean

        Private _extension As String

        Private _vinculacionDocumentos As VinculacionDocumentos

        Private _tipoPrivilegio As TiposPrivilegios

        Private _tienePrivilegio As Boolean

        Private _tipoVinculacion As TiposVinculacion

        Private _rutaRepositorioAlternativoExpedienteOperativo As String

#End Region

#Region "Propiedades"

        Property Clave As Integer

            Get

                Return _clave

            End Get

            Set(value As Integer)

                _clave = value

            End Set

        End Property

        Property UrlPublico As String

            Get

                Return _urlPublico

            End Get

            Set(value As String)

                _urlPublico = value

            End Set

        End Property

        Property NombreDocumento As String

            Get

                Return _nombreDocumento

            End Get

            Set(value As String)

                _nombreDocumento = value

            End Set

        End Property

        Property TipoPrivilegio As TiposPrivilegios

            Get

                Return _tipoPrivilegio

            End Get

            Set(value As TiposPrivilegios)

                _tipoPrivilegio = value

            End Set

        End Property

        Property TipoArchivo As TiposArchivo

            Get

                Return _tipoArchivo

            End Get

            Set(value As TiposArchivo)

                _tipoArchivo = value

            End Set

        End Property

        Property ClaveTipoDocumento As String

            Get

                Return _claveTipoDocumento

            End Get

            Set(value As String)

                _claveTipoDocumento = value

            End Set

        End Property

        Property TipoDocumento As String

            Get

                Return _tipoDocumento

            End Get

            Set(value As String)

                _tipoDocumento = value

            End Set

        End Property

        Property DocumentoConsultableCliente As ConsultableClientes

            Get

                Return _documentoConsultableCliente

            End Get

            Set(value As ConsultableClientes)

                _documentoConsultableCliente = value

            End Set

        End Property

        Property FechaRegistro As DateTime

            Get

                Return _fechaRegistro

            End Get

            Set(value As DateTime)

                _fechaRegistro = value

            End Set

        End Property

        Property PlantillaDocumento As PlantillaDocumento

            Get

                Return _plantillaDocumento

            End Get

            Set(value As PlantillaDocumento)

                _plantillaDocumento = value

            End Set

        End Property

        Property ClaveRepositorioDigital As Integer

            Get

                Return _claveRepositorioDigital

            End Get

            Set(value As Integer)

                _claveRepositorioDigital = value

            End Set

        End Property

        Property ClaveDirectorio As Integer

            Get

                Return _claveDirectorio

            End Get

            Set(value As Integer)

                _claveDirectorio = value

            End Set

        End Property

        Property Usuario As Integer

            Get

                Return _usuario

            End Get

            Set(value As Integer)

                _usuario = value

            End Set

        End Property

        Property ClaveEstado As Integer

            Get

                Return _claveEstado

            End Get

            Set(value As Integer)

                _claveEstado = value

            End Set

        End Property

        Property ClaveEstatus As Integer

            Get

                Return _claveEstatus

            End Get

            Set(value As Integer)

                _claveEstatus = value

            End Set

        End Property

        Property ClaveDivisionMiEmpresa As Integer

            Get

                Return _claveDivisionMiEmpresa

            End Get

            Set(value As Integer)

                _claveDivisionMiEmpresa = value

            End Set

        End Property

        Property RutaDocumentoOrigen As String

            Get

                Return _rutaDocumentoOrigen

            End Get

            Set(value As String)

                _rutaDocumentoOrigen = value

            End Set

        End Property

        Property StatusDocumento As EstatusDocumento

            Get

                Return _statusDocumento

            End Get

            Set(value As EstatusDocumento)

                _statusDocumento = value

            End Set

        End Property

        Property FolioDocumento As String

            Get

                Return _folioDocumento

            End Get

            Set(value As String)

                _folioDocumento = value

            End Set

        End Property

        Property Documento As String

            Get

                Return _documento

            End Get

            Set(value As String)

                _documento = value

            End Set

        End Property

        Property VersionCFDI As VersionesCFDI

            Get

                Return _versionCFDI

            End Get

            Set(value As VersionesCFDI)

                _versionCFDI = value

            End Set

        End Property

        Property StatusLiquidacion As EstatusLiquidacion

            Get

                Return _statusLiquidacion

            End Get

            Set(value As EstatusLiquidacion)

                _statusLiquidacion = value

            End Set

        End Property

        Property ClaveDocumentoPadre As Integer

            Get

                Return _claveDocumentoPadre

            End Get

            Set(value As Integer)

                _claveDocumentoPadre = value

            End Set

        End Property

        Property NombreDocumentoOriginal As String

            Get

                Return _nombreDocumentoOriginal

            End Get

            Set(value As String)

                _nombreDocumentoOriginal = value

            End Set

        End Property

        Property RutaDocumento As String

            Get

                Return _rutaDocumento

            End Get

            Set(value As String)

                _rutaDocumento = value

            End Set

        End Property

        Property RutaDocumentoContingencia As String

            Get

                Return _rutaDocumentoContingencia

            End Get

            Set(value As String)

                _rutaDocumentoContingencia = value

            End Set

        End Property

        Property RutaDocumentoCompleto As String

            Get

                Return _rutaDocumentoCompleto

            End Get

            Set(value As String)

                _rutaDocumentoCompleto = value

            End Set

        End Property

        Property RutaDocumentoContingenciaCompleto As String

            Get

                Return _rutaDocumentoContingenciaCompleto

            End Get

            Set(value As String)

                _rutaDocumentoContingenciaCompleto = value

            End Set

        End Property

        Property RutaRepositorioAlternativoExpedienteOperativo As String

            Get

                Return _rutaRepositorioAlternativoExpedienteOperativo

            End Get

            Set(value As String)

                _rutaRepositorioAlternativoExpedienteOperativo = value

            End Set

        End Property

        Property GuardarDocumento As Boolean

            Get

                Return _guardarDocumento

            End Get

            Set(value As Boolean)

                _guardarDocumento = value

            End Set

        End Property

        Property VinculacionDocumentos As VinculacionDocumentos

            Get

                Return _vinculacionDocumentos

            End Get

            Set(value As VinculacionDocumentos)

                _vinculacionDocumentos = value

            End Set

        End Property

        Property Extension As String

            Get

                Return _extension

            End Get

            Set(value As String)

                _extension = value

            End Set

        End Property

        Property TienePrivilegio As Boolean

            Get

                Return _tienePrivilegio

            End Get

            Set(value As Boolean)

                _tienePrivilegio = value

            End Set

        End Property

        Property TipoVinculacion As TiposVinculacion

            Get

                Return _tipoVinculacion

            End Get

            Set(value As TiposVinculacion)

                _tipoVinculacion = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _clave = 0

            _urlPublico = Nothing

            _nombreDocumento = Nothing

            _tipoArchivo = TiposArchivo.NoDefinido

            _claveTipoDocumento = 0

            _tipoDocumento = Nothing

            _documentoConsultableCliente = ConsultableClientes.No

            _fechaRegistro = Nothing

            _plantillaDocumento = New PlantillaDocumento

            _claveRepositorioDigital = 0

            _claveDirectorio = 0

            _usuario = 0

            _claveEstado = 1

            _claveEstatus = 1

            _claveDivisionMiEmpresa = 0

            _rutaDocumentoOrigen = Nothing

            _statusDocumento = EstatusDocumento.Habilitado

            _folioDocumento = Nothing

            _documento = Nothing

            _versionCFDI = VersionesCFDI.NoAplica

            _statusLiquidacion = EstatusLiquidacion.NoAplica

            _claveDocumentoPadre = 0

            _nombreDocumentoOriginal = Nothing

            _rutaDocumento = Nothing

            _rutaDocumentoContingencia = Nothing

            _rutaDocumentoCompleto = Nothing

            _rutaDocumentoContingenciaCompleto = Nothing

            _rutaRepositorioAlternativoExpedienteOperativo = Nothing

            _guardarDocumento = True

            _vinculacionDocumentos = New VinculacionDocumentos

            _extension = Nothing

            _tipoPrivilegio = TiposPrivilegios.NoIdentificado

            _tienePrivilegio = False

            _tipoVinculacion = TiposVinculacion.NoDefinidia

        End Sub

#End Region

    End Class

    Public Class VinculacionDocumentos

#Region "Atributos"

        Private _claveVinculacion As Integer

        Private _campoLLaveModulo As String

        Private _claveModulo As Integer

        Private _modulo As String

#End Region

#Region "Propiedades"

        Property ClaveVinculacion As Integer

            Get

                Return _claveVinculacion

            End Get

            Set(value As Integer)

                _claveVinculacion = value

            End Set

        End Property

        Property ClaveModulo As Integer

            Get

                Return _claveModulo

            End Get

            Set(value As Integer)

                _claveModulo = value

            End Set

        End Property

        Property Modulo As String

            Get

                Return _modulo

            End Get

            Set(value As String)

                _modulo = value

            End Set

        End Property

        Property CampoLLaveModulo As String

            Get

                Return _campoLLaveModulo

            End Get

            Set(value As String)

                _campoLLaveModulo = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _campoLLaveModulo = Nothing

            _claveVinculacion = Nothing

            _claveModulo = Nothing

            _modulo = Nothing

        End Sub

#End Region

    End Class

    Public Class PlantillaDocumento

#Region "Atributos"

        Private _clavePlantilla As Integer

        Private _nombrePlantilla As String

        Private _descripcionPlantilla As String

        Private _claveTipoDocumentoPlantilla As Integer

        '. Private _caracteristicasDocumentos As List(Of CaracteristicaDocumento)

        Private _caracteristicasDocumentos As SortedDictionary(Of Integer, CaracteristicaDocumento)

#End Region

#Region "Propiedades"

        Property ClavePlantilla As Integer

            Get

                Return _clavePlantilla

            End Get

            Set(value As Integer)

                _clavePlantilla = value

            End Set

        End Property

        Property NombrePlantilla As String

            Get

                Return _nombrePlantilla

            End Get

            Set(value As String)

                _nombrePlantilla = value

            End Set

        End Property

        Property DescripcionPlantilla As String

            Get

                Return _descripcionPlantilla

            End Get

            Set(value As String)

                _descripcionPlantilla = value

            End Set

        End Property

        Property ClaveTipoDocumentoPlantilla As Integer

            Get

                Return _ClaveTipoDocumentoPlantilla

            End Get

            Set(value As Integer)

                _ClaveTipoDocumentoPlantilla = value

            End Set

        End Property

        Property CaracteristicasDocumentos As SortedDictionary(Of Integer, CaracteristicaDocumento)

            Get

                Return _caracteristicasDocumentos

            End Get

            Set(value As SortedDictionary(Of Integer, CaracteristicaDocumento))

                _caracteristicasDocumentos = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _clavePlantilla = 0

            _nombrePlantilla = Nothing

            _descripcionPlantilla = Nothing

            _claveTipoDocumentoPlantilla = 0

            _caracteristicasDocumentos = New SortedDictionary(Of Integer, CaracteristicaDocumento)

        End Sub

#End Region

    End Class

    Public Class CaracteristicaDocumento

#Region "Enum"

        Enum TiposDatosCaracteristicas
            NumericoEntero = 1
            Texto = 2
            VerdaderoFalso = 3
            NumericoDecimal = 4
            FechaHora = 5
            Fecha = 6
            Hora = 7
            Catalogo = 8
        End Enum

        Enum Requerida
            Si = 1
            No = 0
        End Enum

        Enum RutaDinamica
            Si = 1
            No = 0
        End Enum

        Enum RenombrarArchivo
            Si = 1
            No = 0
        End Enum

        Enum Revision
            Si = 1
            No = 0
        End Enum

#End Region

#Region "Atributos"

        Private _claveCaracteristica As Integer

        Private _claveCaracteristicaVin As Integer

        Private _claveCaracteristicaSecundaria As Integer

        Private _claveCaracteristicaVinSecundaria As Integer

        Private _nombreCaracteristica As String

        Private _descripcionCaracteristica As String

        Private _tipoCaracteristica As String

        Private _tituloCaracteristica As String

        Private _titulo As String

        Private _tituloSecundaria As String

        Private _claveTipoCaracteristica As Integer

        Private _claveJerarquia As Integer

        Private _claveCaracteristicaPrimaria As Integer

        Private _claveFormatoCaracteristica As Integer

        Private _formatoCaracteristica As Integer

        Private _claveDetallePlantilla As Integer

        Private _orden As Integer

        Private _caracteristicaRequerida As Requerida

        Private _anchoVisual As Integer

        Private _caracteristicaRutaDinamica As RutaDinamica

        Private _caracteristicaRenombrarArchivo As RenombrarArchivo

        Private _tipoDatoCaracteristica As TiposDatosCaracteristicas

        Private _revision As Revision

        Private _valor As Object

        Private _valorFin As Object

        Private _valorClaveCatalogo As String

        Private _valorCatalogo As String

        Private _displayField As String

        Private _permissionNumber As String

        Private _nameAsKey As String

        Private _keyField As String

        Private _valorClaveCatalogoFin As String

        Private _valorCatalogoFin As String

#End Region

#Region "Propiedades"

        Property ClaveCaracteristica As Integer

            Get

                Return _claveCaracteristica

            End Get

            Set(value As Integer)

                _claveCaracteristica = value

            End Set

        End Property

        Property NombreCaracteristica As String

            Get

                Return _nombreCaracteristica

            End Get

            Set(value As String)

                _nombreCaracteristica = value

            End Set

        End Property

        Property DescripcionCaracteristica As String

            Get

                Return _descripcionCaracteristica

            End Get

            Set(value As String)

                _descripcionCaracteristica = value

            End Set

        End Property

        Property TipoCaracteristica As String

            Get

                Return _tipoCaracteristica

            End Get

            Set(value As String)

                _tipoCaracteristica = value

            End Set

        End Property

        Property ClaveTipoCaracteristica As Integer

            Get

                Return _claveTipoCaracteristica

            End Get

            Set(value As Integer)

                _claveTipoCaracteristica = value

            End Set

        End Property

        Property ClaveJerarquia As Integer

            Get

                Return _claveJerarquia

            End Get

            Set(value As Integer)

                _claveJerarquia = value

            End Set

        End Property

        Property ClaveCaracteristicaPrimaria As Integer

            Get

                Return _claveCaracteristicaPrimaria

            End Get

            Set(value As Integer)

                _claveCaracteristicaPrimaria = value

            End Set

        End Property

        Property ClaveFormatoCaracteristica As Integer

            Get

                Return _claveFormatoCaracteristica

            End Get

            Set(value As Integer)

                _claveFormatoCaracteristica = value

            End Set

        End Property

        Property ClaveDetallePlantilla As Integer

            Get

                Return _claveDetallePlantilla

            End Get

            Set(value As Integer)

                _claveDetallePlantilla = value

            End Set

        End Property

        Property Orden As Integer

            Get

                Return _orden

            End Get

            Set(value As Integer)

                _orden = value

            End Set

        End Property

        Property CaracteristicaRequerida As Requerida

            Get

                Return _caracteristicaRequerida

            End Get

            Set(value As Requerida)

                _caracteristicaRequerida = value

            End Set

        End Property

        Property AnchoVisual As Integer

            Get

                Return _anchoVisual

            End Get

            Set(value As Integer)

                _anchoVisual = value

            End Set

        End Property

        Property CaracteristicaRutaDinamica As RutaDinamica

            Get

                Return _caracteristicaRutaDinamica

            End Get

            Set(value As RutaDinamica)

                _caracteristicaRutaDinamica = value

            End Set

        End Property

        Property CaracteristicaRevision As Revision

            Get

                Return _revision

            End Get

            Set(value As Revision)

                _revision = value

            End Set

        End Property


        Property CaracteristicaRenombrarArchivo As RenombrarArchivo

            Get

                Return _caracteristicaRenombrarArchivo

            End Get

            Set(value As RenombrarArchivo)

                _caracteristicaRenombrarArchivo = value

            End Set

        End Property

        Property TipoDatoCaracteristica As TiposDatosCaracteristicas

            Get

                Return _tipoDatoCaracteristica

            End Get

            Set(value As TiposDatosCaracteristicas)

                _tipoDatoCaracteristica = value

            End Set

        End Property

        Property TituloCaracteristica As String

            Get

                Return _tituloCaracteristica

            End Get

            Set(value As String)

                _tituloCaracteristica = value

            End Set

        End Property

        Property FormatoCaracteristica As String

            Get

                Return _formatoCaracteristica

            End Get

            Set(value As String)

                _formatoCaracteristica = value

            End Set

        End Property

        Property Titulo As String

            Get

                Return _titulo

            End Get

            Set(value As String)

                _titulo = value

            End Set

        End Property

        Property TituloSecundaria As String

            Get

                Return _tituloSecundaria

            End Get

            Set(value As String)

                _tituloSecundaria = value

            End Set

        End Property

        Property ClaveCaracteristicaSecundaria As String

            Get

                Return _claveCaracteristicaSecundaria

            End Get

            Set(value As String)

                _claveCaracteristicaSecundaria = value

            End Set

        End Property

        Property Valor As Object

            Get

                Select Case _tipoDatoCaracteristica

                    Case TiposDatosCaracteristicas.NumericoEntero

                        If IsNumeric(_valor) Then

                            Return CInt(_valor)

                        Else

                            If _valor = "" Then

                                _valor = Nothing

                                Return _valor

                            End If

                        End If

                    Case TiposDatosCaracteristicas.Texto, TiposDatosCaracteristicas.VerdaderoFalso

                        If _valor = "" Then

                            _valor = Nothing

                            Return _valor

                        End If

                        Return CStr(_valor)

                    Case TiposDatosCaracteristicas.NumericoDecimal

                        If IsNumeric(_valor) Then

                            Return CDec(_valor)

                        Else

                            If _valor = "" Then

                                _valor = Nothing

                                Return _valor

                            End If

                        End If

                    Case TiposDatosCaracteristicas.FechaHora

                        If IsDate(_valor) Then

                            Return Convert.ToDateTime(_valor).ToString("dd/MM/yyyy H:mm:ss")

                        Else

                            If _valor = "" Then

                                _valor = Nothing

                                Return _valor

                            End If

                        End If


                    Case TiposDatosCaracteristicas.Fecha

                        If IsDate(_valor) Then

                            Return Convert.ToDateTime(_valor).ToString("dd/MM/yyyy")

                        Else

                            If _valor = "" Then

                                _valor = Nothing

                                Return _valor

                            End If

                        End If


                    Case TiposDatosCaracteristicas.Hora

                        If _valor = "" Then

                            _valor = Nothing

                            Return _valor

                        End If

                        Return Convert.ToDateTime(_valor).ToString("H:mm:ss")

                    Case TiposDatosCaracteristicas.Catalogo

                End Select

                Return _valor

            End Get

            Set(value As Object)

                _valor = value

            End Set

        End Property

        Property ValorFin As Object

            Get

                Select Case _tipoDatoCaracteristica

                    Case TiposDatosCaracteristicas.NumericoEntero

                        If _valorFin = "" Then

                            _valorFin = Nothing

                            Return _valorFin

                        End If

                        Return CInt(_valorFin)

                    Case TiposDatosCaracteristicas.Texto

                        Return CStr(_valorFin)

                    Case TiposDatosCaracteristicas.Texto, TiposDatosCaracteristicas.VerdaderoFalso

                    Case TiposDatosCaracteristicas.NumericoDecimal

                        If _valorFin = "" Then

                            _valorFin = Nothing

                            Return _valorFin

                        End If

                        Return CDec(_valorFin)

                    Case TiposDatosCaracteristicas.FechaHora

                        If _valorFin = "" Then

                            _valorFin = Nothing

                            Return _valorFin

                        End If

                        Return Convert.ToDateTime(_valorFin).ToString("dd/MM/yyyy H:mm:ss")

                    Case TiposDatosCaracteristicas.Fecha

                        If _valorFin = "" Then

                            _valorFin = Nothing

                            Return _valorFin

                        End If

                        Return Convert.ToDateTime(_valorFin).ToString("dd/MM/yyyy")

                    Case TiposDatosCaracteristicas.Hora

                        If _valorFin = "" Then

                            _valorFin = Nothing

                            Return _valorFin

                        End If

                        Return Convert.ToDateTime(_valorFin).ToString("H:mm:ss")

                    Case TiposDatosCaracteristicas.Catalogo

                End Select

                Return _valorFin

            End Get

            Set(value As Object)

                _valorFin = value

            End Set

        End Property

        Property ValorClaveCatalogo As String

            Get

                Return _valorClaveCatalogo

            End Get

            Set(value As String)

                _valorClaveCatalogo = value

            End Set

        End Property

        Property ValorCatalogo As String

            Get

                Return _valorCatalogo

            End Get

            Set(value As String)

                _valorCatalogo = value

            End Set

        End Property

        Property ValorClaveCatalogoFin As String

            Get

                Return _valorClaveCatalogoFin

            End Get

            Set(value As String)

                _valorClaveCatalogoFin = value

            End Set

        End Property

        Property ValorCatalogoFin As String

            Get

                Return _valorCatalogoFin

            End Get

            Set(value As String)

                _valorCatalogoFin = value

            End Set

        End Property

        Property ClaveCaracteristicaVin As Integer

            Get

                Return _claveCaracteristicaVin


            End Get

            Set(value As Integer)

                _claveCaracteristicaVin = value

            End Set

        End Property

        Property ClaveCaracteristicaVinSecundaria As Integer

            Get

                Return _claveCaracteristicaVinSecundaria


            End Get

            Set(value As Integer)

                _claveCaracteristicaVinSecundaria = value

            End Set

        End Property

        Property NameAsKey As String

            Get

                Return _nameAsKey


            End Get

            Set(value As String)

                _nameAsKey = value

            End Set

        End Property

        Property KeyField As String

            Get

                Return _keyField


            End Get

            Set(value As String)

                _keyField = value

            End Set

        End Property

        Property DisplayField As String

            Get

                Return _displayField


            End Get

            Set(value As String)

                _displayField = value

            End Set

        End Property

        Property PermissionNumber As String

            Get

                Return _permissionNumber


            End Get

            Set(value As String)

                _permissionNumber = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _claveCaracteristica = 0

            _claveCaracteristicaVin = 0

            _claveCaracteristicaVinSecundaria = 0

            _nombreCaracteristica = Nothing

            _descripcionCaracteristica = Nothing

            _tipoCaracteristica = Nothing

            _claveTipoCaracteristica = 0

            _claveJerarquia = 0

            _claveCaracteristicaPrimaria = 0

            _claveFormatoCaracteristica = 0

            _claveDetallePlantilla = 0

            _orden = 0

            _caracteristicaRequerida = Requerida.Si

            _anchoVisual = 0

            _caracteristicaRutaDinamica = RutaDinamica.No

            _revision = Revision.No

            _caracteristicaRenombrarArchivo = RenombrarArchivo.No

            _tipoDatoCaracteristica = TiposDatosCaracteristicas.Texto

            _valor = Nothing

            _valorFin = Nothing

            _tituloCaracteristica = Nothing

            _formatoCaracteristica = Nothing

            _titulo = Nothing

            _claveCaracteristicaSecundaria = 0

            _tituloSecundaria = Nothing

            _valorClaveCatalogo = Nothing

            _valorCatalogo = Nothing

            _valorClaveCatalogoFin = Nothing

            _valorCatalogoFin = Nothing

            _nameAsKey = Nothing

            _keyField = Nothing

            _displayField = Nothing

            _permissionNumber = Nothing

        End Sub

#End Region

    End Class

End Namespace
