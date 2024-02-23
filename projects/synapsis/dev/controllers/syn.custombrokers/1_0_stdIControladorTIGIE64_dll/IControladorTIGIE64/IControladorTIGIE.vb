Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Syn.Nucleo.RecursosComercioExterior.CamposTarifaArancelaria

Public Interface IControladorTIGIE

#Region "Enums"

    Enum TipoOperacion

        Importacion = 1
        Exportacion = 2

    End Enum

#End Region

#Region "Propiedades"

    Property Estado As TagWatcher

#End Region

#Region "Funciones"

    Function EnlistarFracciones(ByVal texto_ As String) As TagWatcher
    Function EnlistarNicosFraccion(ByVal fraccion_ As String) As TagWatcher
    Function BuscarNico(ByVal id_ As ObjectId,
                        Optional ByVal operationType_ As TipoOperacion = TipoOperacion.Importacion,
                        Optional ByVal date_ As Date = Nothing,
                        Optional ByVal country_ As String = Nothing) As TagWatcher
    Function GetHsCode(Of T)(hsCode As String,
                                      operationType_ As IControladorTIGIE.TipoOperacion,
                                      country_ As String,
                                      Optional ByVal date_ As Date = Nothing) As TagWatcher

    Function GetHsCode(Of T)(hsCodes As List(Of String),
                                      operationType_ As IControladorTIGIE.TipoOperacion,
                                      country_ As String,
                                       Optional ByVal date_ As Date = Nothing) As TagWatcher

    Function Pruebas()
    Function Pruebas2()


#End Region

End Interface