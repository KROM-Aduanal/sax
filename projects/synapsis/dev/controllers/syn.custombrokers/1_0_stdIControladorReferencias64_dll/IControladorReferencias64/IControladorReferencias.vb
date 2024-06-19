Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.CustomBrokers.Controllers
Imports System.IO
Imports MongoDB.Driver
Imports Ia.Pln

Public Interface IControladorReferencias

    Enum ModalidadesOperativas
        Intrinseca = 1
        Extrinseca = 2
    End Enum

#Region "Propiedades"
    Property Referencia As List(Of Referencia)

    Property Estado As TagWatcher


#End Region

#Region "Funciones"

    Function GeneraSecuencia(ByVal nombre_ As String,
                                       Optional ByVal enviroment_ As Int16 = 0,
                                       Optional ByVal anio_ As Int16 = 0,
                                       Optional ByVal mes_ As Int16 = 0,
                                       Optional ByVal tipoSecuencia_ As Integer = 0,
                                       Optional ByVal subTipoSecuencia_ As Integer = 0,
                                       Optional ByVal prefijo As String = Nothing
                                       ) As Int32
    Function CrearPrereferencia(referencia_ As MemoryStream, Optional ByVal documetoCargado As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL) As TagWatcher
    Function CrearPrereferencia(documento_ As Referencia) As TagWatcher
    Function CrearPrereferencias(referencia_ As List(Of Referencia)) As TagWatcher
    Function CrearPrereferencias(documento_ As List(Of MemoryStream), Optional ByVal documetoCargado As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL) As TagWatcher
    Function AgregarGuia(documento_ As MemoryStream, ByVal id_ As ObjectId, ByVal modalidad As ModalidadesOperativas) As TagWatcher
    Function AgregarGuia(guia_ As Guia, ByVal id_ As ObjectId, ByVal modalidad As ModalidadesOperativas) As TagWatcher
    Function AgregarGuias(documento_ As List(Of MemoryStream), ByVal id_ As ObjectId, ByVal modalidad As ModalidadesOperativas) As TagWatcher

#End Region

End Interface
