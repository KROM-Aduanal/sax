Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Rec.Globals.Utils

Public Interface IControladorSecuencia

#Region "ENUMS"

#End Region

#Region "PROPIEDADES"

    Property _Enviroment As Int32

    Property _TipoSecuencia As Int32

    Property _SubtipoSecuencia As Int32

    Property _Secuencia As ISecuencia

    Property _Estado As TagWatcher

#End Region

#Region "MÉTODOS"
    Function Generar(ByVal nombre_ As String,
                     ByVal tipoSecuencia_ As Int32,
                     ByVal compania_ As Int32,
                     ByVal area_ As Int32,
                     Optional session_ As IClientSessionHandle = Nothing) _
                     As TagWatcher
    Function Generar(ByVal nombre_ As String,
                     ByVal tipoSecuencia_ As Int32,
                     ByVal compania_ As Int32,
                     ByVal area_ As Int32,
                     ByVal subtipoSecuencia_ As Int32,
                     Optional session_ As IClientSessionHandle = Nothing) _
                     As TagWatcher
    Function Generar(ByVal nombre_ As String,
                     ByVal tipoSecuencia_ As Int32,
                     ByVal compania_ As Int32,
                     ByVal area_ As Int32,
                     ByVal subtipoSecuencia_ As Int32,
                     ByVal enviroment_ As Int32,
                     Optional session_ As IClientSessionHandle = Nothing) _
                     As TagWatcher
    Function Generar(ByVal nombre_ As String,
                     ByVal tipoSecuencia_ As Int32,
                     ByVal compania_ As Int32,
                     ByVal area_ As Int32,
                     ByVal subtipoSecuencia_ As Int32,
                     ByVal prefijo_ As String,
                     ByVal sufijo_ As String,
                     Optional session_ As IClientSessionHandle = Nothing) _
                     As TagWatcher
    Function Generar(ByVal secuencia_ As ISecuencia,
                 Optional session_ As IClientSessionHandle = Nothing) _
                 As TagWatcher
#End Region
End Interface
