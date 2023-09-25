Imports System.IO
Imports MongoDB.Bson
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IControladorFacturaComercial : Inherits IDisposable

#Region "Enum"
    Enum Disponibilidades

        SinDefinir = 0

        Cerrado = 1

        Abierto = 2

    End Enum

    Enum Modalidades

        Interno = 0

        Externo = 1

    End Enum
#End Region

#Region "Propiedades"
    Property FacturasComerciales As List(Of ConstructorFacturaComercial)

        ReadOnly Property Documento As DocumentoElectronico

        ReadOnly Property Documentos As List(Of DocumentoElectronico)

        WriteOnly Property Estado As TagWatcher

        Property ModalidadTrabajo As Modalidades

        WriteOnly Property ConservarFacturas As Boolean

        Property Entorno As Integer

        Property DisponibilidadRecurso As Disponibilidades

        ReadOnly Property FactorConfiabilidadIA As Double
#End Region

#Region "Métodos"
    Sub ReiniciarControlador(Optional ByVal entorno_ As Integer = 1)

    Sub CargaFacturas(ByVal documentoDigital_ As MemoryStream)
    Sub CargaFacturas(ByVal documentoDigital_ As List(Of MemoryStream))

#End Region

#Region "Funciones"
    Function ActualizarDatosAcuseValor(ByVal idFactura_ As ObjectId,
                                       ByVal valoresAcuseValor_ As List(Of String)) As TagWatcher
    Function ListaFacturas(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaFacturas(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaFacturas(ByVal folioFactura_ As String) As TagWatcher

    Function ListaFacturas(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function CargaFacturas(ByVal listaFacturas_ As List(Of ConstructorFacturaComercial)) As TagWatcher

    Function FirmaDigital(Of T)(ByVal idFactura As ObjectId) As String

    Function FacturaDisponible(ByVal idFactura_ As ObjectId) As Boolean

    Function FacturaDisponible(ByVal folioFactura_ As String) As Boolean

    Function TotalIncrementables(fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal idFactura_ As ObjectId,
                                 fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal idsFacturas_ As List(Of ObjectId),
                                 fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal folioFactura_ As String,
                                 fechaMoneda_ As Date) As TagWatcher

    Function TotalIncrementables(ByVal foliosFacturas_ As List(Of String),
                                 fechaEntrada_ As Date) As TagWatcher

    Function ListaIncrementables() As TagWatcher

    Function ListaIncrementables(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaIncrementables(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaIncrementables(ByVal folioFactura_ As String) As TagWatcher

    Function ListaIncrementables(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function ListaIncoterms() As TagWatcher

    Function ListaIncoterms(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaIncoterms(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaIncoterms(ByVal folioFactura_ As String) As TagWatcher

    Function ListaIncoterms(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function ListaPartidas() As TagWatcher

    Function ListaPartidas(ByVal idFactura_ As ObjectId) As TagWatcher

    Function ListaPartidas(ByVal idsFacturas_ As List(Of ObjectId)) As TagWatcher

    Function ListaPartidas(ByVal folioFactura_ As String) As TagWatcher

    Function ListaPartidas(ByVal foliosFacturas_ As List(Of String)) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal idFactura_ As ObjectId,
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal idsFacturas_ As List(Of ObjectId),
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal folioFactura_ As String,
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ListaCamposFacturaComercial(ByVal foliosFacturas_ As List(Of String),
                         ByVal seccionesCampos_ As Dictionary(Of [Enum],
                         List(Of [Enum]))) As TagWatcher

    Function ConsultaValorDolaresFactura(fechaMoneda_ As Date) As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal idFactura_ As ObjectId,
                                         fechaMoneda_ As Date) _
                                         As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal idsFacturas_ As List(Of ObjectId),
                                         fechaMoneda_ As Date) _
                                         As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal folioFactura_ As String,
                                         fechaMoneda_ As Date) _
                                         As TagWatcher

    Function ConsultaValorDolaresFactura(ByVal foliosFacturas_ As List(Of String),
                                         fechaMoneda_ As Date) _
                                         As TagWatcher

    Function ConsultaPLNFactura(ByVal consulta_ As String) As BsonDocument

#End Region

End Interface