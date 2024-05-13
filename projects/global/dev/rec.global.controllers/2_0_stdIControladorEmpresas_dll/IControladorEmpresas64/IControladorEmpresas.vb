Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Rec.Globals.Empresas


Public Interface IControladorEmpresas

#Region "ENUMS"

    Enum TiposEmpresas

        Nacional = 1

        Internacional = 2

    End Enum

    Enum Modalidades

        Extrinseca = 1

        Intrinseca = 2

    End Enum

    Enum TipoEstructura

        RFC = 1

        CURP = 2

        Domicilio = 3

    End Enum

#End Region


#Region "PROPIEDADES"

    'Parámetros del controlador
    Property TipoEmpresa As TiposEmpresas

    Property Modalidad As Modalidades

    Property Empresa As IEmpresa

    Property PaisEmpresa As String

    'Salida para mantener los resultados
    Property ListaEmpresas As List(Of IEmpresa)

    Property ListaDomicilios As List(Of Domicilio)
    Property Estado As TagWatcher

#End Region

#Region "MÉTODOS"

    Sub ReiniciarControlador()

    Function Agregar(ByVal empresa_ As IEmpresaNacional,
                     Optional ByVal objetoRetorno_ As Boolean = False,
                     Optional ByVal session_ As IClientSessionHandle = Nothing) _
                     As TagWatcher


    Function Agregar(ByVal empresa_ As IEmpresaInternacional,
                     Optional ByVal objetoRetorno_ As Boolean = False,
                     Optional ByVal session_ As IClientSessionHandle = Nothing) _
                     As TagWatcher

    Function Modificar(ByVal empresa_ As IEmpresaNacional,
                       Optional ByVal session_ As IClientSessionHandle = Nothing) _
                       As TagWatcher

    Function Modificar(ByVal empresa_ As IEmpresaInternacional,
                       Optional ByVal session_ As IClientSessionHandle = Nothing) _
                       As TagWatcher

    Function Consultar(ByVal razonSocial_ As String,
                       Optional ByVal limiteResultados_ As Int32 = 10) As TagWatcher


    Function ConsultarUna(ByVal cveEmpresa_ As ObjectId) As TagWatcher


    Function ConsultarDomicilios(ByVal cveEmpresa_ As ObjectId) As TagWatcher


    Function ConsultarDomicilio(ByVal cveEmpresa_ As ObjectId,
                                ByVal cveDomicilio As ObjectId) As TagWatcher


    Function Archivar(ByVal listaObjectIdEmpresas_ As List(Of ObjectId)) _
                      As TagWatcher

    Function ArchivarDomicilios(ByVal objectIdEmpresa As ObjectId,
                                ByVal listaObjectIdDomicilio_ As List(Of ObjectId)) _
                                As TagWatcher

    Function EstructuraEmpresaNacional() _
        As IEmpresaNacional

    Function EstructuraEmpresaNacional(ByVal razonSocial_ As String,
                                       ByVal rfc_ As String,
                                       Optional ByVal tipoPersona_ As IEmpresaNacional.TiposPersona = IEmpresaNacional.TiposPersona.Moral,
                                       Optional ByVal curp_ As String = Nothing) _
                                       As IEmpresaNacional

    Function EstructuraEmpresaNacional(ByVal razonSocial_ As String,
                                       ByVal rfc_ As String,
                                       ByVal domicilio_ As Domicilio,
                                       Optional ByVal tipoPersona_ As IEmpresaNacional.TiposPersona = IEmpresaNacional.TiposPersona.Moral,
                                       Optional ByVal curp_ As String = Nothing) _
                                       As IEmpresaNacional
    Function EstructuraEmpresaInternacional() _
        As IEmpresaInternacional

    Function EstructuraEmpresaInternacional(ByVal razonSocial_ As String) _
                                        As IEmpresaInternacional

    Function EstructuraEmpresaInternacional(ByVal razonSocial_ As String,
                                            ByVal domicilio_ As Domicilio,
                                            ByVal taxid_ As String) _
                                            As IEmpresaInternacional

#End Region

End Interface