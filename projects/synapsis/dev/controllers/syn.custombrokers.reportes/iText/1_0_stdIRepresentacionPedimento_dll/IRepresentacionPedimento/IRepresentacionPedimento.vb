Imports iText.Layout
Imports iText.Layout.Element.Image
Imports iText.Layout.Borders.Border
Imports iText.Layout.Element
Imports iText.Layout.Properties
Imports iText.Kernel.Pdf
Imports iText.Kernel.Geom
Imports iText.Kernel.Colors
Imports iText.Kernel.Font
Imports System.IO
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Syn.Nucleo.RecursosComercioExterior
Imports iText.IO.Font
Imports iText.Commons.Actions
Imports iText.Layout.Borders
Imports iText.IO.Image
Imports System.Runtime.Remoting.Messaging
Imports iText.Kernel.Events
Imports iText.Kernel.Pdf.Canvas.Draw
Imports iText.Kernel.Pdf.Canvas
Imports iText.Kernel.Pdf.Extgstate
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Syn.CustomBrokers.Controllers
Imports iText.IO.Font.Constants

Public Interface IRepresentacionPedimento

    Enum TipoOperacion
        Importacion = 1
        Exportacion = 2
    End Enum

#Region "Funciones"
    Function ImprimirPedimentoNormal(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirRectificacion(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoComplementario(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoGlobal(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoSimplificado(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoConsolidado(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirFormatoPartesII(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

#End Region

End Interface