Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.CustomBrokers.Controllers
Imports System.IO

Public Interface IControladorManifestacionValor

#Region "Enums"
    Enum TiposDocumento
        Ambos = 1
        MV = 2
        HC = 3
    End Enum

#End Region
#Region "Propiedades"
    Property ManifestacionesValor As List(Of ConstructorManifestacionValor) ' Implements IControladorManifestacionValor.ManifestacionesValor
    Property Estado As TagWatcher 'Implements IControladorManifestacionValor.Estado
    WriteOnly Property Entorno As Int32 'Implements IControladorManifestacionValor.Entorno

#End Region

#Region "Funciones"
    Function Consultar(ByVal objectid_ As ObjectId) As TagWatcher
    Function Descargar(objectId_ As ObjectId, Optional ByVal tipoDocumento As TiposDocumento = 1) As FileStream
    Function Descargar(ObjectIds_ As List(Of ObjectId), Optional ByVal tipoDocumento As TiposDocumento = 1) As FileStream
    Function Descargar(pedimentos_ As List(Of String), Optional ByVal tipoDocumento As TiposDocumento = 1) As FileStream
    Function Descargar(pedimentos As String, Optional ByVal tipoDocumento As TiposDocumento = 1) As FileStream
    Function DescargarPDF(pedimentos As String, Optional ByVal tipoDocumento As TiposDocumento = 1) As List(Of String)
    Function Generar(objectId_ As ObjectId) As TagWatcher
    Function Generar(objectIds_ As List(Of ObjectId)) As TagWatcher
    Function Generar(pedimento_ As String) As TagWatcher
    Function Generar(pedimentos_ As List(Of String)) As TagWatcher

#End Region

End Interface
