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
                        Optional ByVal tipoOperacion_ As TipoOperacion = TipoOperacion.Importacion,
                        Optional ByVal fecha_ As Date = Nothing,
                        Optional ByVal pais_ As String = Nothing) As TagWatcher
    Function TraeDatosFraccion(Of T)(fraccion_ As String,
                                      tipoOperacion_ As IControladorTIGIE.TipoOperacion,
                                      pais_ As String,
                                      fecha_ As Date) As TagWatcher
    Function TraeDatosFraccion(Of T)(fracciones_ As List(Of String),
                                      tipoOperacion_ As IControladorTIGIE.TipoOperacion,
                                      pais_ As String,
                                      fecha_ As Date) As TagWatcher


#End Region

End Interface