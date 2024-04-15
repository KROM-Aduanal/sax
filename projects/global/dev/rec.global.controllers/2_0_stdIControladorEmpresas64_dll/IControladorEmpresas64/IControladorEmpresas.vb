Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Rec.Globals.IEmpresaNacional64
Imports Rec.Globals.IEmpresaInternacional64
Imports Rec.Globals.EmpresaNacional64
Imports Rec.Globals.EmpresaInternacional64
Imports Rec.Globals.IEmpresa64
Imports Rec.Globals.Domicilio64


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

    ReadOnly Property Estado As TagWatcher

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

    'TEMPORAL
    Function ConsultarPaises(ByVal pais_ As String,
                             Optional ByVal limiteResultados_ As Int32 = 10) _
                             As TagWatcher

#End Region

End Interface