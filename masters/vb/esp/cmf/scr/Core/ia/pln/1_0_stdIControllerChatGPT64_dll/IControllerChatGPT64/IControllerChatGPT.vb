﻿Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.CustomBrokers.Controllers
Imports System.IO
Imports MongoDB.Driver

Public Interface IControllerChatGPT

    Enum DocumentoCargado
        BL = 1
        FacturaImportacion = 2
        FacturaExportacion = 3
    End Enum

#Region "Properties"

    Property Status As TagWatcher

#End Region

#Region "Funciones"

    Function DocumentAnalyzer(Of T)(ByVal document_ As List(Of Byte())) As TagWatcher
    Function GetResponse(ByVal operationNumber_ As ObjectId) As TagWatcher

#End Region

End Interface
