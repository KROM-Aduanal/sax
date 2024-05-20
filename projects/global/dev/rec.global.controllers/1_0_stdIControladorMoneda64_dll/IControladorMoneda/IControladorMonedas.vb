
Imports MongoDB.Bson
Imports Rec.Globals
Imports Rec.Globals.Utils
Imports Wma.Exceptions

Public Interface IControladorMonedas : Inherits IDisposable

#Region "Enum"
    Enum CamposBusquedaSimple
        SinDefinir = 0
        IDS = 1
        NOMBREESP = 2
        NOMBREING = 3
        CLAVES = 4
        PRESENTACION = 5
        FACTORDOLAR = 6
    End Enum
#End Region


#Region "Atributos"

#End Region


#Region "Propiedades"

    Property Monedas As List(Of MonedaGlobal)

    Property TiposdeCambio As List(Of tipodecambioreciente)
    Property FactoresCambioRecientes As List(Of factorreciente)

    Property Consulta(icampo_ As [Enum], Optional ByVal tipocve As String = "cvedefault", Optional ByVal ilimit_ As Int32 = 5) As List(Of String)

    Property Estado As TagWatcher

    Property CamposMonedas As Dictionary(Of String, List(Of String))

    Property UltimoMatch As String


#End Region



#Region "Métodos"

    'Sub EstableceContextoActual(contextoactual_ As String)
    'Sub ActualizaUltimaMoneda(MonedaId As ObjectId)

    Sub ActualizaListMonedas(monedas_ As List(Of MonedaGlobal))

    Sub ActualizaListMonedasOnline(monedas_ As List(Of MonedaGlobal))
#End Region

#Region "Funciones"
    'Function Consulta(Claves As List(Of String)) As List(Of Moneda)
    'Function Consulta(IDS As List(Of ObjectId)) As List(Of Moneda)
    'Function Consulta(token_ As String) As List(Of Moneda)
    'Function Consulta(Claves As List(Of String)) As List(Of Moneda)
    Function ConsultaMoneda(campo_ As [Enum],
                            Optional ByVal formato_ As String = "cvedefault",
                            Optional condicion_ As String = "",
                            Optional ByVal limite_ As Int32 = 5) As List(Of String)
    Function ConsultaMoneda(campos_ As List(Of [Enum]),
                            Optional ByVal formato_ As String = "cvedefault",
                            Optional condicion_ As String = "",
                            Optional ByVal limite_ As Int32 = 5) As List(Of String)
    Function BuscarMonedas(lttoken_ As List(Of String),
                           Optional ltObjectId As List(Of ObjectId) = Nothing,
                           Optional formato_ As String = "cvedefault",
                           Optional ByVal ilimit_ As Int32 = 5) As List(Of MonedaGlobal)
    Function BuscarMonedas(token_ As String, Optional monedaId_ As ObjectId = Nothing,
                           Optional formato_ As String = "cvedefault",
                           Optional ByVal ilimit_ As Int32 = 5) As List(Of MonedaGlobal)

    Function GetCveMoneys(Optional formato_ As String = "cvedefault",
                          Optional ByVal ilimit_ As Int32 = 5) As List(Of String)
    'Function DevuelveUltimaMoneda() As MonedaGlobal
    Function ObtenerTipodeCambio(clave_ As String, Optional monedaId_ As ObjectId = Nothing, Optional monedaCambio_ As String = "USD", Optional nombre_ As String = "",
                                 Optional inicial_ As DateTime = Nothing,
                                 Optional final_ As DateTime = Nothing,
                                 Optional limit_ As Int32 = 10) As List(Of tipodecambioreciente)

    Function ObtenerFactorCambio(clave_ As String, Optional monedaId_ As ObjectId = Nothing, Optional monedaCambio_ As String = "USD", Optional nombre_ As String = "",
                                 Optional fechaCambio_ As DateTime = Nothing) As List(Of factorreciente)

    Function ObtenerFactorTipodeCambio(clave_ As String, Optional fechaCambio_ As DateTime = Nothing, Optional monedaFactor_ As String = "USD", Optional monedaTipoCambio_ As String = "MXP", Optional nombre_ As String = "") As TagWatcher
#End Region

End Interface

