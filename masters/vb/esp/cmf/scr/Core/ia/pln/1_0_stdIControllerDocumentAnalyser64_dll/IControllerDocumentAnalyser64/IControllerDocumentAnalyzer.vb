Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports MongoDB.Bson
Imports System.IO
Imports Wma.Exceptions

Public Interface IControllerDocumentAnalyzer

#Region "Enums"


#End Region

#Region "Properties"

    Property Status As TagWatcher

#End Region

#Region "Funciones"

    Function DocumentAnalyzer(Of T)(ByVal document_ As List(Of MemoryStream)) As TagWatcher
    Function GetResponse(ByVal operationNumber_ As ObjectId) As TagWatcher

#End Region

End Interface
