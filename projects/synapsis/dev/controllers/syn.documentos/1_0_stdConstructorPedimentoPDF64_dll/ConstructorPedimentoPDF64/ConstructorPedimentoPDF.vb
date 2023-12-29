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

Public Class ConstructorPedimentoPDF


#Region "Atributos"

    Private _ms As MemoryStream

    Private _pw As PdfWriter

    Private _pdfDocument

    Private _doc As Document

    Private _stylecell As Style

    Private _estiloSinBordes As Style

    Private _arialBold As PdfFont

    Private _arial As PdfFont

    Private _file As File

    Private _image As Image

    Private record As Object

    Private _watermark As Image

    Private _celdas2 As Dictionary(Of String, Dictionary(Of String, List(Of Int64)))

    Private _Nivel1 As Table

    Private _tablalayout As Table

    Private _layout As Table

    Private _controladorPDF As Itext7Handler

    Private _celdas As Dictionary(Of String, String)

    Private _Nivel2 As Table

    Private _Nivel3 As Table

    Private _dimensiones As Single()

    Private _espacios As String

    Private _vinculacion As String

#End Region

    Enum TiposBordes
        Niniguno = 0
        Derecha = 1
        DerechaAbajo = 2
    End Enum

    Enum TipoOperacion

        Importacion = 1
        Exportacion = 2

    End Enum

#Region "Constructores"

    Public Sub New()

        _stylecell = New Style

        _estiloSinBordes = New Style

        _ms = New MemoryStream()

        _pw = New PdfWriter(_ms)

        _pdfDocument = New PdfDocument(_pw)

        _doc = New Document(_pdfDocument, PageSize.LETTER)

        _doc.SetMargins(14, 20, 14, 20)

        _stylecell.SetTextAlignment(TextAlignment.CENTER)

        _estiloSinBordes.SetTextAlignment(TextAlignment.RIGHT)

        _estiloSinBordes.SetPadding(0F)

        _estiloSinBordes.SetMargin(0F)

        _estiloSinBordes.SetMarginTop(0F)

        _estiloSinBordes.SetBorder(NO_BORDER)

        _estiloSinBordes.SetBorderBottom(NO_BORDER)

        _arial = PdfFontFactory.CreateFont("C:/Windows/Fonts/Arial.ttf", PdfEncodings.IDENTITY_H)

        _arialBold = PdfFontFactory.CreateFont("C:/Windows/Fonts/Arialbd.ttf", PdfEncodings.IDENTITY_H)

        _watermark = New Image(ImageDataFactory.Create("C:/temp/PROFORMA.png"))

        _controladorPDF = New Itext7Handler(_arialBold, _arial, _watermark, _estiloSinBordes)

        _espacios = ""

        _vinculacion = ""

    End Sub

#End Region

    Public Function ImprimirPedimentoNormal(ByVal documento_ As DocumentoElectronico) As String

        _dimensiones = {100.0F, 110.0F, 45.0F, 10.0F, 100.0F, 10.0F, 20.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_doc, _tablalayout))

        If documento_ IsNot Nothing Then

            If documento_.Attribute(CA_FECHA_PAGO).Valor IsNot Nothing Then

                _pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, New WatherMark(_watermark))

            End If

        End If

        _Nivel2 = New Table({1000.0F})

        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        EncabezadoPrincipal(documento_)

        DatosProveedor(documento_)

        AcuseValor(documento_)

        Transportes(documento_)

        Candados(documento_)

        GuiaOrdenEmbarque(documento_)

        Numero_o_Tipo(documento_)

        Identificadores(documento_)

        CuentasAduaneras(documento_)

        Descargos(documento_)

        Compensaciones(documento_)

        FormasPagoVirtuales(documento_)

        Observaciones(documento_)

        Partidas2(documento_)

        FinPedimento()

        PiePagina(documento_)

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString


    End Function

    Public Function ImprimirRectificacio(ByVal documento_ As DocumentoElectronico) As String

        _dimensiones = {100.0F, 110.0F, 45.0F, 10.0F, 100.0F, 10.0F, 20.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_doc, _tablalayout))

        _Nivel2 = New Table({1000.0F})

        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        EncabezadoPrincipal(documento_)

        Rectificacion(documento_)

        DiferenciasContribuciones(documento_)

        DatosProveedor(documento_)

        Transportes(documento_)

        Numero_o_Tipo(documento_)

        Identificadores(documento_)

        Observaciones(documento_)

        Partidas(documento_)

        FinPedimento()

        PiePagina(documento_)

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString


    End Function

    Public Function ImprimirPedimentoComplementario(ByVal documento_ As DocumentoElectronico) As String

        _dimensiones = {100.0F, 110.0F, 45.0F, 10.0F, 100.0F, 10.0F, 20.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_doc, _tablalayout))

        If documento_ IsNot Nothing Then

            If documento_.Attribute(CA_FECHA_PAGO).Valor IsNot Nothing Then

                _pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, New WatherMark(_watermark))

            End If

        End If

        _Nivel2 = New Table({1000.0F})

        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        EncabezadoPrincipal(documento_)

        PedimentoComplementario()

        Identificadores(documento_)

        Descargos(documento_)

        PruebaSuficiente(documento_)

        Observaciones(documento_)

        DeterminacionContribucionPartidaTMEC(documento_)

        DeterminacionContribucionPartidaTLCA(documento_)

        FinPedimento()

        PiePagina(documento_)

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString


    End Function

    Public Function ImprimirPedimentoGlobal(ByVal documento_ As DocumentoElectronico) As String

        _dimensiones = {100.0F, 110.0F, 45.0F, 10.0F, 100.0F, 10.0F, 20.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_doc, _tablalayout))

        _Nivel2 = New Table({1000.0F})

        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        EncabezadoPrincipal(documento_)

        Identificadores(documento_)

        Descargos(documento_)

        Observaciones(documento_)

        Partidas(documento_)

        FinPedimento()

        PiePagina(documento_)

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString


    End Function

    Public Function ImprimirPedimentoSimplificado(ByVal documento_ As DocumentoElectronico) As String

        _dimensiones = {100.0F, 110.0F, 45.0F, 10.0F, 100.0F, 10.0F, 20.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_doc, _tablalayout))

        _Nivel2 = New Table({1000.0F})

        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        EncabezadoSimplificado(documento_)

        Candados(documento_)

        GuiaOrdenEmbarque(documento_)

        Numero_o_Tipo(documento_)

        AcuseValorSimplificado(documento_)

        Identificadores(documento_)

        E_Documents(documento_)

        Transportes(documento_)

        Observaciones(documento_)

        Rectificacion(documento_)

        DiferenciasContribuciones(documento_)

        FinPedimento()

        PiePagina(documento_)

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString


    End Function

    Public Function ImprimirPedimentoConsolidado(ByVal documento_ As DocumentoElectronico) As String

        _dimensiones = {100.0F, 110.0F, 45.0F, 10.0F, 100.0F, 10.0F, 20.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_doc, _tablalayout))

        _Nivel2 = New Table({1000.0F})

        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        EncabezadoConsolidado(documento_)

        Candados(documento_)

        Observaciones(documento_)

        FinImpresion()

        PiePagina(documento_)

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0

        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())

        Return inputAsString

    End Function

    Public Function ImprimirFormatoPartesII(ByVal documento_ As DocumentoElectronico) As String

        Dim tipo_ As TipoOperacion = TipoOperacion.Importacion


        _doc.SetMargins(50, 80, 50, 80)

        _Nivel1 = New Table({1000.0F})

        _Nivel2 = New Table({600.0F, 150.0F, 250.0F})

        '--------------------- ENCABEZADO ---------------------

        If tipo_ = TipoOperacion.Importacion Then

            _Nivel1.AddHeaderCell(New Cell().Add(New Paragraph("M1.2.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 10.0F).
                    SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

            _Nivel1.AddCell(New Cell().Add(New Paragraph("Pedimento de importación. Parte II. Embarque parcial de mercancías.").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 10.0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))
        Else

            _Nivel1.AddHeaderCell(New Cell().Add(New Paragraph("M1.3.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 10.0F).
                    SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

            _Nivel1.AddCell(New Cell().Add(New Paragraph("Pedimento de exportación. Parte II. Embarque parcial de mercancías.").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 10.0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

        End If

        '--------------------- CODIGO BARRAS ---------------------

        _image = New Image(ImageDataFactory.Create("C:/temp/CBA_RKU2100551.png"))

        _image.SetWidth(100)

        _Nivel2.AddCell(New Cell().Add(New Paragraph("").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Div().Add(New Paragraph("").SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetHorizontalAlignment(HorizontalAlignment.CENTER)).
                            Add(_image).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetBorder(NO_BORDER).
                            SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetHorizontalAlignment(HorizontalAlignment.CENTER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0.0F, 10.0F, 0.0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        '--------------------- INFO PEDIMENTO ---------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Número de pedimento ________________").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetWordSpacing(0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Datos del vehículo ___________________").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetWordSpacing(0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Candados _________________________").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetWordSpacing(0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Contenedor(es) _____________________").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetWordSpacing(0F)).SetMargins(0F, 0F, 10.0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

        '--------------------- MERCANCIA ---------------------

        _Nivel2 = New Table({320.0F, 360.0F, 320.0F})

        _Nivel2.AddCell(New Cell().Add(New Paragraph("Tipo de mercancía").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("Cantidad en Unidad de Medida de Comercialización").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("Cantidad en Unidad de medida de Tarifa").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(" . ").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(" . ").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(" . ").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("Número de serie del certificado:").SetTextAlignment(TextAlignment.LEFT).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("e.firma:").SetTextAlignment(TextAlignment.LEFT).
                    SetFont(_arial).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0.0F, 10.0F, 0.0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

        '--------------------- FIRMA ---------------------

        _Nivel2 = New Table({500.0F, 250.0F, 250.0F})

        _Nivel2.AddCell(New Cell().Add(New Paragraph("").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(".").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("Nombre").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("").SetTextAlignment(TextAlignment.CENTER).
                    SetFont(_arialBold).SetFontSize(9.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F)).
                    SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0.0F, 10.0F, 0.0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString


    End Function

    Private Function EncabezadoPrincipal(ByVal documento_ As DocumentoElectronico)

        '====================================ENCABEZADO==============================================

#Region "Encabezado"

        _dimensiones = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 5.0F, 1.0F, 5.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"REGIMEN: ", IIf(documento_.Attribute(CA_REGIMEN).Valor IsNot Nothing, documento_.Attribute(CA_REGIMEN).Valor, "IMD")}}

        _Nivel1 = _controladorPDF.FolioDoc(documento_.FolioOperacion)

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "PEDIMENTO")

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _dimensiones = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 3.0F, 1.0F, 2.0F}

        _celdas = New Dictionary(Of String, String) From {{"DESTINO: ", IIf(documento_.Attribute(CA_DESTINO_ORIGEN).Valor IsNot Nothing, documento_.Attribute(CA_DESTINO_ORIGEN).Valor, " ")},
                                                          {"TIPO CAMBIO: ", IIf(documento_.Attribute(CA_TIPO_CAMBIO).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_CAMBIO).Valor, " ")},
                                                          {"PESO BRUTO: ", IIf(documento_.Attribute(CA_PESO_BRUTO).Valor IsNot Nothing, documento_.Attribute(CA_PESO_BRUTO).Valor, " ")},
                                                          {"ADUANA E/S: ", IIf(documento_.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor, " ")}}


        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------------


        '-------------------MEDIOS DE TRANSPORTE--------------------------------------
        _dimensiones = {1.0F, 1.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"ENTRADA/SALIDA:", "VACIO"},
                                                          {"ARRIBO:", "VACIO"},
                                                          {"SALIDA:", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_MEDIO_TRANSPORTE).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_TRANSPORTE).Valor, " ")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_MEDIO_TRANSPORTE_ARRIBO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_TRANSPORTE_ARRIBO).Valor, " ")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_MEDIO_TRANSPORTE_SALIDA).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_TRANSPORTE_SALIDA).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "MEDIOS DE TRANSPORTE", False)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '------------VALORES ADUANA, ETC------------------------------------------------
        _dimensiones = {2.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"VALOR DOLARES:", IIf(documento_.Attribute(CA_VALOR_DOLARES).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_DOLARES).Valor, "26,015.14")},
                                                          {"VALOR ADUANA:", IIf(documento_.Attribute(CA_VALOR_ADUANA).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_ADUANA).Valor, "524,460.00")},
                                                          {"PRECIO PAGADO/VALOR COMERCIAL:", IIf(documento_.Attribute(CA_PRECIO_PAGADO_VALOR_COMERCIAL).Valor IsNot Nothing, documento_.Attribute(CA_PRECIO_PAGADO_VALOR_COMERCIAL).Valor, "524,460.00")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '--------------------------------------------------------------------------------


        '-------------DATOS DEL IMPORTADOR/EXPORTADOR------------------------------------
        _dimensiones = {100.0F, 100.0F, 800.0F}

        _celdas = New Dictionary(Of String, String) From {{"RFC:", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"NOMBRE, DENOMINACION O RAZON SOCIAL:", "VACIO"},
                                                          {"CURP:", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")},
                                                          {"VACIO", IIf(documento_.Attribute(CA_RAZON_SOCIAL_IOE).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_RAZON_SOCIAL_IOE).ValorPresentacion, " ")},
                                                          {"VACIO ", "VACIO"},
                                                          {"DOMICILIO:", IIf(documento_.Attribute(CA_DOMICILIO_IOE).Valor IsNot Nothing, documento_.Attribute(CA_DOMICILIO_IOE).Valor, "CARRETERA JOROBAS - TULA KM. 3.5 MANZ. 5 LOTE 1 FRACC. PARQUE INDUSTRIAL HUEHUETOCA 54680 HUEHUETOCA ESTADO DE MEXICO, MEXICO (ESTADOS UNIDOS MEXICANOS)")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "DATOS DEL IMPORTADOR/EXPORTADOR")

        _tablalayout.AddStyle(_stylecell)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '--------------------------------------------------------------------------------


        '-----------VALORES INCREMENTABLES-----------------------------------------------
        _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 2.0F}

        _celdas = New Dictionary(Of String, String) From {{"VAL. SEGUROS", "VACIO"},
                                                          {"SEGUROS", "VACIO"},
                                                          {"FLETES", "VACIO"},
                                                          {"EMBALAJES", "VACIO"},
                                                          {"OTROS INCREMENTABLES", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_VALOR_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_SEGUROS).Valor, "0")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_SEGUROS).Valor, "0")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_FLETES).Valor IsNot Nothing, documento_.Attribute(CA_FLETES).Valor, "0")},
                                                          {"VACIO   ", IIf(documento_.Attribute(CA_EMBALAJES).Valor IsNot Nothing, documento_.Attribute(CA_EMBALAJES).Valor, "0")},
                                                          {"VACIO    ", IIf(documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor, "0")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.AddStyle(_stylecell)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '----------------------------------------------------------------------------------


        '-------------VALORES DECREMENTABLES-----------------------------------------------
        If documento_.Attribute(CA_VALOR_SEGUROS).Valor IsNot Nothing And documento_.Attribute(CA_VALOR_SEGUROS).Valor <> 0 Then

            _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 2.0F}

            _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

            _celdas = New Dictionary(Of String, String) From {{"TRANSPORTE DECREMENTABLES", "VACIO"},
                                                          {"SEGURO DECREMENTABLES", "VACIO"},
                                                          {"CARGA", "VACIO"},
                                                          {"DESCARGA", "VACIO"},
                                                          {"OTROS DECREMENTABLES", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_VALOR_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_SEGUROS).Valor, "0")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_SEGUROS).Valor, "0")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_FLETES).Valor IsNot Nothing, documento_.Attribute(CA_FLETES).Valor, "0")},
                                                          {"VACIO   ", IIf(documento_.Attribute(CA_EMBALAJES).Valor IsNot Nothing, documento_.Attribute(CA_EMBALAJES).Valor, "0")},
                                                          {"VACIO    ", IIf(documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor, "0")}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "VALOR DECREMENTABLES", False)

            _tablalayout.AddStyle(_estiloSinBordes)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        End If

        '------------------CÓDIGO DE VALIDACIÓN--------------------------------
        _dimensiones = {200.0F, 550.0F, 250.0F}

        _celdas = New Dictionary(Of String, String) From {{"CÓDIGO DE ACEPTACIÓN:", IIf(documento_.Attribute(CA_ACUSE_ELECTRONICO_VALIDACION).Valor IsNot Nothing, documento_.Attribute(CA_ACUSE_ELECTRONICO_VALIDACION).Valor, "U8Z7A8E9")},
                                                          {"IMG", "C:/temp/CBA_RKU2100551.png"},
                                                          {"CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO:", IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430")}}

        _tablalayout = _controladorPDF.setTablaLayoutBorder(_celdas, _dimensiones)

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '-------------NUMERO DE BULTOS------------------------------------------
        _dimensiones = {450.0F, 550.0F}

        '_tablalayout = New Table(_dimensiones) '.UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"MARCAS, NUMEROS Y TOTAL DE BULTOS:", IIf(documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor IsNot Nothing, documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor, "02 CONTENEDORES")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '------------FECHAS-----------------------------------------------------
        _Nivel3 = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        Dim fechaEntrada_ As DateTime = documento_.Attribute(CA_FECHA_ENTRADA).Valor

        Dim fechaPago_ As DateTime = documento_.Attribute(CA_FECHA_PAGO).Valor

        _dimensiones = {1.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"ENTRADA:", fechaEntrada_.ToString("dd/MM/yyyy")},
                                                          {"PAGO:", fechaPago_.ToString("dd/MM/yyyy")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "FECHAS", False)

        If documento_.Attribute(CA_FECHA_ORIGINAL).Valor IsNot Nothing And documento_.Attribute(CA_CVE_PEDIMENTO).Valor = "F4" Then

            Dim fechaOriginal_ As DateTime = documento_.Attribute(CA_FECHA_ORIGINAL).Valor

            _celdas.Add("ORIGINAL:", fechaOriginal_.ToString("dd/MM/yyyy"))

        End If

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '----------TASAS NIVEL PEDIMENTO--------------------------------------
        _dimensiones = {1.0F, 1.0F, 1.0F}
        _espacios = ""
        _celdas = New Dictionary(Of String, String) From {{"CONTRIB.:", "VACIO"},
                                                          {"CVE.T.TASA:", "VACIO"},
                                                          {"TASA:", "VACIO"}}

        With documento_.Seccion(SeccionesPedimento.ANS6)

            For Each tasas_ As Nodo In .Nodos

                If tasas_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim tasa_ = CType(tasas_, PartidaGenerica)

                    _celdas.Add("VACIO" + _espacios, IIf(tasa_.Attribute(CA_CONTRIBUCION).ValorPresentacion IsNot Nothing, tasa_.Attribute(CA_CONTRIBUCION).ValorPresentacion, "DTA"))
                    _celdas.Add("VACIO " + _espacios, IIf(tasa_.Attribute(CA_CVE_TIPO_TASA).Valor IsNot Nothing, tasa_.Attribute(CA_CVE_TIPO_TASA).Valor, "IVA/PRV"))
                    _celdas.Add("VACIO  " + _espacios, IIf(tasa_.Attribute(CA_TASA).Valor IsNot Nothing, tasa_.Attribute(CA_TASA).Valor, "PRV"))

                    _espacios += "   "

                End If
            Next

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "TASAS A NIVEL PEDIMENTO")

        End With

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER)) '.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '----------------------------------------------------------------------


        '------------CUADRO DE LIQUIDACIONES------------------------------------
        _Nivel3 = New Table({700, 300.0F}).UseAllAvailableWidth

        _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=7).Add(New Paragraph("CUADRO DE LIQUIDACIÓN").SetFont(_arialBold).SetFontSize(8.0F)))

        _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth
        _celdas = New Dictionary(Of String, String) From {{"CONCEPTO", "VACIO"},
                                                          {"F.P.", "VACIO"},
                                                          {"IMPORTE", "VACIO"},
                                                          {"CONCEPTO ", "VACIO"},
                                                          {"F.P. ", "VACIO"},
                                                          {"IMPORTE ", "VACIO"}}

        Dim incrementableLiq_ As Integer = 0

        _espacios = ""

        With documento_.Seccion(SeccionesPedimento.ANS7)

            With documento_.Seccion(SeccionesPedimento.ANS55)

                For Each liquidaciones_ As Nodo In .Nodos

                    If liquidaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim liquidacion_ = CType(liquidaciones_, PartidaGenerica)
                        _celdas.Add("VACIO" + _espacios, IIf(liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion IsNot Nothing, liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion, " "))
                        _celdas.Add("VACIO " + _espacios, IIf(liquidacion_.Attribute(CA_FORMA_PAGO).Valor IsNot Nothing, liquidacion_.Attribute(CA_FORMA_PAGO).Valor, " "))
                        _celdas.Add("VACIO  " + _espacios, IIf(liquidacion_.Attribute(CA_IMPORTE).Valor IsNot Nothing, liquidacion_.Attribute(CA_IMPORTE).Valor, " "))

                        _espacios += "   "

                        incrementableLiq_ += 1

                    End If

                Next

                If incrementableLiq_ < 6 Then

                    _espacios += "   "

                    _celdas.Add("VACIO" + _espacios, " ")
                    _celdas.Add("VACIO " + _espacios, "0")
                    _celdas.Add("VACIO  " + _espacios, "11053")

                    Select Case incrementableLiq_

                        Case 1

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 2

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 3

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, "0")
                            _celdas.Add("VACIO  " + _espacios, "86352")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 4

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                    End Select

                End If

            End With

        End With

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER)) '.SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '-------------TOTALES---------------------------------------------------------
        _dimensiones = {1.0F, 1.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"EFECTIVO", IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "101,879")},
                                                          {"OTROS", IIf(documento_.Attribute(CA_OTROS).Valor IsNot Nothing, documento_.Attribute(CA_OTROS).Valor, "0")},
                                                          {"TOTAL", IIf(documento_.Attribute(CA_TOTAL).Valor IsNot Nothing, documento_.Attribute(CA_TOTAL).Valor, "101,879")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "TOTALES")

        _tablalayout.SetFontSize(7.0F)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER))
        '----------------------------------------------------------------------------------


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '----------------CERTIFICACIONES-----------------------------------------------------
        _dimensiones = {10.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.AddStyle(_stylecell)

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("Página 1 de 2").SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("CERTIFICACIONES").SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER))
        '--------------------------------------------------------------------------------------

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#Region "Depósito referenciado y código QR"

        '-------------DEPÓSITO REFERENCIADO---------------------------------------------
        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _Nivel3 = New Table({900.0F, 100.0F}).UseAllAvailableWidth

        _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.AddStyle(_estiloSinBordes)

        _Nivel3.AddStyle(_estiloSinBordes)

        _image = New Image(ImageDataFactory.Create("C:/temp/CB_RKU2100551.png"))

        _image.SetWidth(80)

        _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("DEPÓSITO REFERENCIADO - LÍNEA DE CAPTURA - INFORMACIÓN DEL PAGO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=6).Add(New Div().Add(_image).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_DEPOSITO_REFERENCIADO).Valor IsNot Nothing, documento_.Attribute(CA_DEPOSITO_REFERENCIADO).Valor, "032100D13UP130040274 101879"))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F))).SetBorder(NO_BORDER).SetMargins(1.0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 0F, 2.0F))

        Dim fechaPagoDeposito_ As DateTime = documento_.Attribute(CA_FECHA_PAGO).Valor

        _celdas2 = New Dictionary(Of String, Dictionary(Of String, List(Of Int64)))() From {
                                                          {"*** PAGO ELECTRÓNICO ***", New Dictionary(Of String, List(Of Int64)) From {{"VACIO", New List(Of Int64) From {6, 0}}}},
                                                          {"PATENTE:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_PATENTE).Valor IsNot Nothing, documento_.Attribute(CA_PATENTE).Valor, " "), New List(Of Int64) From {1, 1}}}},
                                                          {"PEDIMENTO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUMERO_PEDIMENTO).Valor IsNot Nothing, documento_.Attribute(CA_NUMERO_PEDIMENTO).Valor, " "), New List(Of Int64) From {1, 1}}}},
                                                          {"ADUANA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430"), New List(Of Int64) From {1, 1}}}},
                                                          {"BANCO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NOMBRE_INSTITUCION_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_INSTITUCION_BANCARIA).Valor, "BBVA BANCOMER"), New List(Of Int64) From {1, 5}}}},
                                                          {"LÍNEA DE CAPTURA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_LINEA_CAPTURA).Valor IsNot Nothing, documento_.Attribute(CA_LINEA_CAPTURA).Valor, "032100D13UP130040274"), New List(Of Int64) From {2, 4}}}},
                                                          {"IMPORTE PAGADO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "$101,879"), New List(Of Int64) From {2, 1}}}},
                                                          {"FECHA DE PAGO:", New Dictionary(Of String, List(Of Int64)) From {{fechaPagoDeposito_.Day & "/" & fechaPagoDeposito_.Month & "/" & fechaPagoDeposito_.Year, New List(Of Int64) From {2, 1}}}},
                                                          {"NÚMERO DE OPERACIÓN BANCARIA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUMERO_OPERACION_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NUMERO_OPERACION_BANCARIA).Valor, "01221029383520"), New List(Of Int64) From {3, 3}}}},
                                                          {"NÚMERO DE TRANSACCIÓN SAT:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUMERO_TRANSACCION_SAT).Valor IsNot Nothing, documento_.Attribute(CA_NUMERO_TRANSACCION_SAT).Valor, "40012290120211025519"), New List(Of Int64) From {3, 3}}}},
                                                          {"MEDIO DE PRESENTACIÓN:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_MEDIO_PRESENTACION).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_PRESENTACION).Valor, "Otros medios electrónicos: (pago electrónico)"), New List(Of Int64) From {2, 4}}}},
                                                          {"MEDIO DE RECEPCIÓN/COBRO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor, "Efectivo - Cargo a cuenta"), New List(Of Int64) From {2, 4}}}}}


        _tablalayout = _controladorPDF.setTablaLayout(_celdas2, _dimensiones, TiposBordes.Niniguno)
        _tablalayout.SetBorderBottom(NO_BORDER)

        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
        '--------------------------------------------------


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '--------------------CÓDIGO QR-----------------------------------------------------
        _dimensiones = {1000.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.SetFontSize(7.0F)

        _tablalayout.AddStyle(_stylecell)

        _image = New Image(ImageDataFactory.Create("C:/temp/QR_RKU2100551.png"))

        _image.SetWidth(100)

        '--------------------------------------------------------------------------------------

        _Nivel1.AddCell(New Cell().Add(_image.SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE))


#End Region
#End Region

    End Function

    Private Function EncabezadoSimplificado(ByVal documento_ As DocumentoElectronico)

        '====================================ENCABEZADO==============================================

#Region "Encabezado"

        _dimensiones = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 5.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")}}

        _Nivel1 = _controladorPDF.FolioDoc(documento_.FolioOperacion)
        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "FORMA SIMPLIFICADA DEL PEDIMENTO")

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _dimensiones = {2.0F, 2.0F, 2.0F, 3.0F, 1.0F, 2.0F}

        _celdas = New Dictionary(Of String, String) From {{"DESTINO: ", IIf(documento_.Attribute(CA_DESTINO_ORIGEN).Valor IsNot Nothing, documento_.Attribute(CA_DESTINO_ORIGEN).Valor, " ")},
                                                          {"PESO BRUTO: ", IIf(documento_.Attribute(CA_PESO_BRUTO).Valor IsNot Nothing, documento_.Attribute(CA_PESO_BRUTO).Valor, " ")},
                                                          {"ADUANA E/S: ", IIf(documento_.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor, " ")}}


        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------------

        '-------------DATOS DEL IMPORTADOR/EXPORTADOR------------------------------------
        _dimensiones = {100.0F, 100.0F, 800.0F}

        _celdas = New Dictionary(Of String, String) From {{"RFC:", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"CURP:", IIf(documento_.Attribute(CA_CURP_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_IOE).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "DATOS DEL IMPORTADOR/EXPORTADOR")

        _tablalayout.AddStyle(_stylecell)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '--------------------------------------------------------------------------------

        '------------------CÓDIGO DE VALIDACIÓN--------------------------------
        _dimensiones = {200.0F, 550.0F, 250.0F}

        _celdas = New Dictionary(Of String, String) From {{"CÓDIGO DE ACEPTACIÓN:", IIf(documento_.Attribute(CA_ACUSE_ELECTRONICO_VALIDACION).Valor IsNot Nothing, documento_.Attribute(CA_ACUSE_ELECTRONICO_VALIDACION).Valor, "U8Z7A8E9")},
                                                          {"IMG", "C:/temp/CBA_RKU2100551.png"},
                                                          {"CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO:", IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430")}}

        _tablalayout = _controladorPDF.setTablaLayoutBorder(_celdas, _dimensiones)

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '-------------NUMERO DE BULTOS------------------------------------------
        _dimensiones = {550.0F, 450.0F}

        '_tablalayout = New Table(_dimensiones) '.UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"MARCAS, NUMEROS Y TOTAL DE BULTOS:", IIf(documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor IsNot Nothing, documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor, "02 CONTENEDORES")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '------------FECHAS-----------------------------------------------------
        _Nivel3 = New Table({1000.0F}).UseAllAvailableWidth

        Dim fechaEntrada_ As DateTime = documento_.Attribute(CA_FECHA_ENTRADA).Valor

        Dim fechaPago_ As DateTime = documento_.Attribute(CA_FECHA_PAGO).Valor

        _dimensiones = {1.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"ENTRADA:", fechaEntrada_.ToString("dd/MM/yyyy")},
                                                          {"PAGO:", fechaPago_.ToString("dd/MM/yyyy")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "FECHAS", False)

        If documento_.Attribute(CA_FECHA_ORIGINAL).Valor IsNot Nothing And documento_.Attribute(CA_CVE_PEDIMENTO).Valor = "F4" Then

            Dim fechaOriginal_ As DateTime = documento_.Attribute(CA_FECHA_ORIGINAL).Valor

            _celdas.Add("ORIGINAL:", fechaOriginal_.ToString("dd/MM/yyyy"))

        End If

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        '------------CUADRO DE LIQUIDACIONES------------------------------------
        _Nivel3 = New Table({700, 300.0F}).UseAllAvailableWidth

        _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=7).Add(New Paragraph("CUADRO DE LIQUIDACIÓN").SetFont(_arialBold).SetFontSize(8.0F)))

        _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth
        _celdas = New Dictionary(Of String, String) From {{"CONCEPTO", "VACIO"},
                                                          {"F.P.", "VACIO"},
                                                          {"IMPORTE", "VACIO"},
                                                          {"CONCEPTO ", "VACIO"},
                                                          {"F.P. ", "VACIO"},
                                                          {"IMPORTE ", "VACIO"}}

        Dim incrementableLiq_ As Integer = 0

        _espacios = ""

        With documento_.Seccion(SeccionesPedimento.ANS7)

            With documento_.Seccion(SeccionesPedimento.ANS55)

                For Each liquidaciones_ As Nodo In .Nodos

                    If liquidaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim liquidacion_ = CType(liquidaciones_, PartidaGenerica)
                        _celdas.Add("VACIO" + _espacios, IIf(liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion IsNot Nothing, liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion, " "))
                        _celdas.Add("VACIO " + _espacios, IIf(liquidacion_.Attribute(CA_FORMA_PAGO).Valor IsNot Nothing, liquidacion_.Attribute(CA_FORMA_PAGO).Valor, " "))
                        _celdas.Add("VACIO  " + _espacios, IIf(liquidacion_.Attribute(CA_IMPORTE).Valor IsNot Nothing, liquidacion_.Attribute(CA_IMPORTE).Valor, " "))

                        _espacios += "   "

                        incrementableLiq_ += 1

                    End If

                Next

                If incrementableLiq_ < 6 Then

                    _espacios += "   "

                    _celdas.Add("VACIO" + _espacios, " ")
                    _celdas.Add("VACIO " + _espacios, "0")
                    _celdas.Add("VACIO  " + _espacios, "11053")

                    Select Case incrementableLiq_

                        Case 1

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 2

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 3

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, "0")
                            _celdas.Add("VACIO  " + _espacios, "86352")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 4

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                    End Select

                End If

            End With

        End With

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER)) '.SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '-------------TOTALES---------------------------------------------------------
        _dimensiones = {1.0F, 1.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"EFECTIVO", IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "101,879")},
                                                          {"OTROS", IIf(documento_.Attribute(CA_OTROS).Valor IsNot Nothing, documento_.Attribute(CA_OTROS).Valor, "0")},
                                                          {"TOTAL", IIf(documento_.Attribute(CA_TOTAL).Valor IsNot Nothing, documento_.Attribute(CA_TOTAL).Valor, "101,879")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "TOTALES")

        _tablalayout.SetFontSize(7.0F)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER))
        '----------------------------------------------------------------------------------


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '----------------CERTIFICACIONES-----------------------------------------------------
        _dimensiones = {10.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.AddStyle(_stylecell)

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("Página 1 de 2").SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("CERTIFICACIONES").SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER))
        '--------------------------------------------------------------------------------------

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#Region "Depósito referenciado y código QR"

        '-------------DEPÓSITO REFERENCIADO---------------------------------------------
        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _Nivel3 = New Table({900.0F, 100.0F}).UseAllAvailableWidth

        _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.AddStyle(_estiloSinBordes)

        _Nivel3.AddStyle(_estiloSinBordes)

        _image = New Image(ImageDataFactory.Create("C:/temp/CB_RKU2100551.png"))

        _image.SetWidth(80)

        _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("DEPÓSITO REFERENCIADO - LÍNEA DE CAPTURA - INFORMACIÓN DEL PAGO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=6).Add(New Div().Add(_image).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_DEPOSITO_REFERENCIADO).Valor IsNot Nothing, documento_.Attribute(CA_DEPOSITO_REFERENCIADO).Valor, "032100D13UP130040274 101879"))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F))).SetBorder(NO_BORDER).SetMargins(1.0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 0F, 2.0F))

        Dim fechaPagoDeposito_ As DateTime = documento_.Attribute(CA_FECHA_PAGO).Valor

        _celdas2 = New Dictionary(Of String, Dictionary(Of String, List(Of Int64)))() From {
                                                          {"*** PAGO ELECTRÓNICO ***", New Dictionary(Of String, List(Of Int64)) From {{"VACIO", New List(Of Int64) From {6, 0}}}},
                                                          {"PATENTE:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_PATENTE).Valor IsNot Nothing, documento_.Attribute(CA_PATENTE).Valor, " "), New List(Of Int64) From {1, 1}}}},
                                                          {"PEDIMENTO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUMERO_PEDIMENTO).Valor IsNot Nothing, documento_.Attribute(CA_NUMERO_PEDIMENTO).Valor, " "), New List(Of Int64) From {1, 1}}}},
                                                          {"ADUANA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430"), New List(Of Int64) From {1, 1}}}},
                                                          {"BANCO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NOMBRE_INSTITUCION_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_INSTITUCION_BANCARIA).Valor, "BBVA BANCOMER"), New List(Of Int64) From {1, 5}}}},
                                                          {"LÍNEA DE CAPTURA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_LINEA_CAPTURA).Valor IsNot Nothing, documento_.Attribute(CA_LINEA_CAPTURA).Valor, "032100D13UP130040274"), New List(Of Int64) From {2, 4}}}},
                                                          {"IMPORTE PAGADO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "$101,879"), New List(Of Int64) From {2, 1}}}},
                                                          {"FECHA DE PAGO:", New Dictionary(Of String, List(Of Int64)) From {{fechaPagoDeposito_.Day & "/" & fechaPagoDeposito_.Month & "/" & fechaPagoDeposito_.Year, New List(Of Int64) From {2, 1}}}},
                                                          {"NÚMERO DE OPERACIÓN BANCARIA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUMERO_OPERACION_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NUMERO_OPERACION_BANCARIA).Valor, "01221029383520"), New List(Of Int64) From {3, 3}}}},
                                                          {"NÚMERO DE TRANSACCIÓN SAT:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUMERO_TRANSACCION_SAT).Valor IsNot Nothing, documento_.Attribute(CA_NUMERO_TRANSACCION_SAT).Valor, "40012290120211025519"), New List(Of Int64) From {3, 3}}}},
                                                          {"MEDIO DE PRESENTACIÓN:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_MEDIO_PRESENTACION).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_PRESENTACION).Valor, "Otros medios electrónicos: (pago electrónico)"), New List(Of Int64) From {2, 4}}}},
                                                          {"MEDIO DE RECEPCIÓN/COBRO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor, "Efectivo - Cargo a cuenta"), New List(Of Int64) From {2, 4}}}}}


        _tablalayout = _controladorPDF.setTablaLayout(_celdas2, _dimensiones, TiposBordes.Niniguno)
        _tablalayout.SetBorderBottom(NO_BORDER)

        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
        '--------------------------------------------------


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '--------------------CÓDIGO QR-----------------------------------------------------
        _dimensiones = {1000.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.SetFontSize(7.0F)

        _tablalayout.AddStyle(_stylecell)

        _image = New Image(ImageDataFactory.Create("C:/temp/QR_RKU2100551.png"))

        _image.SetWidth(150)

        '--------------------------------------------------------------------------------------

        _Nivel1.AddCell(New Cell().Add(_image.SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE))


#End Region
#End Region

    End Function

    Private Function EncabezadoConsolidado(ByVal documento_ As DocumentoElectronico)

        '====================================ENCABEZADO==============================================


        _dimensiones = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 5.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_TIPO_OPERACION).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_OPERACION).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")}}

        _Nivel1 = _controladorPDF.FolioDoc(documento_.FolioOperacion)
        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "FORMATO DE AVISO CONSOLIDADO")

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _dimensiones = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 3.0F, 1.0F, 2.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUMERO DE ACUSE DE VALOR: ", " "}}


        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------------

        _dimensiones = {1.0F, 1.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"ADUANA E/S:", "VACIO"},
                                                          {"NUM. REMESA:", "VACIO"},
                                                          {"PESO BRUTO:", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor, " ")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_MEDIO_TRANSPORTE_ARRIBO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_TRANSPORTE_ARRIBO).Valor, " ")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_PESO_BRUTO).Valor IsNot Nothing, documento_.Attribute(CA_PESO_BRUTO).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "MEDIOS DE TRANSPORTE", False)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '-------------DATOS DEL IMPORTADOR/EXPORTADOR------------------------------------
        _dimensiones = {200.0F, 800.0F}

        _celdas = New Dictionary(Of String, String) From {{"RFC:", IIf(documento_.Attribute(CA_RFC_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_IOE).Valor, " ")},
                                                          {"NOMBRE, DENOMINACION O RAZON SOCIAL:", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "DATOS DEL IMPORTADOR/EXPORTADOR")

        _tablalayout.AddStyle(_stylecell)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        '------------------CÓDIGO DE VALIDACIÓN--------------------------------
        _dimensiones = {200.0F, 550.0F, 250.0F}

        _celdas = New Dictionary(Of String, String) From {{"CÓDIGO DE ACEPTACIÓN:", IIf(documento_.Attribute(CA_ACUSE_ELECTRONICO_VALIDACION).Valor IsNot Nothing, documento_.Attribute(CA_ACUSE_ELECTRONICO_VALIDACION).Valor, "U8Z7A8E9")},
                                                          {"IMG", "C:/temp/CBA_RKU2100551.png"},
                                                          {"CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO:", IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430")}}

        _tablalayout = _controladorPDF.setTablaLayoutBorder(_celdas, _dimensiones)

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '-------------NUMERO DE BULTOS------------------------------------------
        _dimensiones = {450.0F, 550.0F}

        '_tablalayout = New Table(_dimensiones) '.UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"MARCAS, NUMEROS Y TOTAL DE BULTOS:", IIf(documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor IsNot Nothing, documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor, "02 CONTENEDORES")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '----------------CERTIFICACIONES-----------------------------------------------------
        _dimensiones = {10.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.AddStyle(_stylecell)

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("Página 1 de 2").SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("CERTIFICACIONES").SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER))
        '--------------------------------------------------------------------------------------

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function PedimentoComplementario()

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("PEDIMENTO COMPLEMENTARIO").SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetTextAlignment(TextAlignment.CENTER).
                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

    End Function

    Private Function DatosProveedor(documento_ As DocumentoElectronico)

#Region "Datos del proveedor"

        '-------------------Datos de los proveedores---------------------------------------------
        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        Dim Nivel21_ As Table

        _Nivel3 = New Table({1000.0F}).UseAllAvailableWidth

        _Nivel2.SetBorder(NO_BORDER)

        _Nivel3.SetBorder(NO_BORDER)

        _Nivel2.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("DATOS DEL PROVEEDOR O COMPRADOR").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


        With documento_.Seccion(SeccionesPedimento.ANS10)

            For Each proveedores_ As Nodo In .Nodos

                If proveedores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim proveedor_ = CType(proveedores_, PartidaGenerica)

                    Nivel21_ = New Table({550.0F, 450.0F}).UseAllAvailableWidth

                    _dimensiones = {50.0F, 500.0F}

                    _celdas = New Dictionary(Of String, String) From {{"ID FISCAL", "VACIO"},
                                                          {"NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL", "VACIO"},
                                                          {"VACIO", IIf(proveedor_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor IsNot Nothing, proveedor_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor, " ")},
                                                          {"VACIO ", IIf(proveedor_.Attribute(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC).Valor IsNot Nothing, proveedor_.Attribute(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC).Valor, " ")}}

                    _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

                    _tablalayout.AddStyle(_estiloSinBordes)

                    Nivel21_.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    _dimensiones = {400.0F, 50.0F}

                    _celdas = New Dictionary(Of String, String) From {{"DOMICILIO", "VACIO"},
                                                          {"VINCULACIÓN", "VACIO"},
                                                          {"VACIO", IIf(proveedor_.Attribute(CA_DOMICILIO_POC).Valor IsNot Nothing, proveedor_.Attribute(CA_DOMICILIO_POC).Valor, " ")},
                                                          {"VACIO ", IIf(proveedor_.Attribute(CA_VINCULACION).Valor IsNot Nothing, proveedor_.Attribute(CA_VINCULACION).Valor, "SI")}}

                    _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

                    _tablalayout.AddStyle(_estiloSinBordes)

                    Nivel21_.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                    _Nivel2.AddCell(New Cell().Add(Nivel21_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    '-----Facturas--------------------------------------------
                    With documento_.Seccion(SeccionesPedimento.ANS13)
                        _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

                        _celdas = New Dictionary(Of String, String) From {{"NUM. FACTURA", "VACIO"},
                                                                          {"FECHA", "VACIO"},
                                                                          {"INCOTERM", "VACIO"},
                                                                          {"MONEDA FACT", "VACIO"},
                                                                          {"VAL. MON. FACT", "VACIO"},
                                                                          {"FACTOR MON. FACT", "VACIO"},
                                                                          {"VAL. DOLARES", "VACIO"}}

                        _tablalayout.AddStyle(_estiloSinBordes)

                        _espacios = ""

                        For Each facturas_ As Nodo In .Nodos

                            If facturas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim factura_ = CType(facturas_, PartidaGenerica)

                                Dim fechaFactura_ As DateTime = factura_.Attribute(CA_FECHA_FACTURA).Valor

                                _celdas.Add("VACIO" + _espacios, IIf(factura_.Attribute(CA_CFDI_FACTURA).Valor IsNot Nothing, factura_.Attribute(CA_CFDI_FACTURA).Valor, " "))
                                _celdas.Add("VACIO " + _espacios, fechaFactura_.Day & "/" & fechaFactura_.Month & "/" & fechaFactura_.Year)
                                _celdas.Add("VACIO  " + _espacios, IIf(factura_.Attribute(CA_INCOTERM).Valor IsNot Nothing, factura_.Attribute(CA_INCOTERM).Valor, " "))
                                _celdas.Add("VACIO   " + _espacios, IIf(factura_.Attribute(CA_CVE_MONEDA_FACTURA).Valor IsNot Nothing, factura_.Attribute(CA_CVE_MONEDA_FACTURA).Valor, " "))

                                If factura_.Attribute(CA_CVE_MONEDA_FACTURA).Valor IsNot Nothing And factura_.Attribute(CA_CVE_MONEDA_FACTURA).Valor = "USD" Then

                                    _celdas.Add("VACIO    " + _espacios, factura_.Attribute(CA_MONTO_MONEDA_FACTURA).Valor.ToString)

                                Else

                                    _celdas.Add("VACIO     " + _espacios, "1.00000000")

                                End If

                                _celdas.Add("VACIO      " + _espacios, IIf(factura_.Attribute(CA_FACTOR_MONEDA).Valor IsNot Nothing, factura_.Attribute(CA_FACTOR_MONEDA).Valor, " "))
                                _celdas.Add("VACIO       " + _espacios, IIf(factura_.Attribute(CA_MONTO_USD).Valor IsNot Nothing, factura_.Attribute(CA_MONTO_USD).Valor, " "))

                                _espacios += "        "

                            End If
                        Next

                        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo)

                    End With

                    _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                End If

            Next

        End With

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(NO_BORDER))

#End Region

    End Function

    Private Function AcuseValor(documento_ As DocumentoElectronico)

#Region "Acuse valor (COVE) datos del proveedor"

        _Nivel2 = New Table({1000.0F}) '.UseAllAvailableWidth

        _Nivel3 = New Table({500.0F, 500.0F}) '.UseAllAvailableWidth

        _dimensiones = {300.0F, 300.0F, 300.0F}

        _tablalayout = New Table(_dimensiones) '.UseAllAvailableWidth

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.SetBorder(NO_BORDER)

        '_Nivel2.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("DATOS DEL PROVEEDOR O COMPRADOR").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        _celdas = New Dictionary(Of String, String) From {{"NÚMERO DE ACUSE DE VALOR", "VACIO"},
                                                        {"VINCULACIÓN", "VACIO"},
                                                        {"INCOTERM", "VACIO"}}

        _tablalayout.AddStyle(_estiloSinBordes)

        _espacios = ""

        With documento_.Seccion(SeccionesPedimento.ANS10)

            For Each proveedores_ As Nodo In .Nodos

                If proveedores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim proveedor_ = CType(proveedores_, PartidaGenerica)

                    With proveedor_.Seccion(SeccionesPedimento.ANS13)

                        For Each facturas_ As Nodo In .Nodos

                            If facturas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim factura_ = CType(facturas_, PartidaGenerica)

                                If factura_.Attribute(CA_VINCULACION) IsNot Nothing Then

                                    _vinculacion = factura_.Attribute(CA_VINCULACION).Valor

                                Else

                                    _vinculacion = "SI"

                                End If

                                _celdas.Add("VACIO" + _espacios, IIf(factura_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor IsNot Nothing, factura_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor, " "))
                                _celdas.Add("VACIO " + _espacios, _vinculacion)
                                _celdas.Add("VACIO  " + _espacios, IIf(factura_.Attribute(CA_INCOTERM).Valor IsNot Nothing, factura_.Attribute(CA_INCOTERM).Valor, " "))

                                _espacios += "   "

                            End If
                        Next

                    End With

                End If

            Next

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "DATOS DEL PROVEEDOR O COMPRADOR")

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        End With

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

#End Region

    End Function

    Private Function AcuseValorSimplificado(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _tablalayout = New Table({200.0F, 200.0F, 200.0F, 200.0F, 200.0F}).UseAllAvailableWidth

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NUMERO DE ACUSE DE VALOR").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        With documento_.Seccion(SeccionesPedimento.ANS10)

            For Each proveedores_ As Nodo In .Nodos

                If proveedores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim proveedor_ = CType(proveedores_, PartidaGenerica)

                    With proveedor_.Seccion(SeccionesPedimento.ANS13)

                        Dim numFacturas_ = .Nodos.Count

                        For Each facturas_ As Nodo In .Nodos

                            If facturas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim factura_ = CType(facturas_, PartidaGenerica)

                                If factura_.Attribute(CA_VINCULACION) IsNot Nothing Then

                                    _vinculacion = factura_.Attribute(CA_VINCULACION).Valor

                                Else

                                    _vinculacion = "SI"

                                End If

                                _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(factura_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor IsNot Nothing,
                                    factura_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor, " "))).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                    SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            End If
                        Next

                        For i As Int16 = 1 To numFacturas_

                            _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Next

                    End With

                End If

            Next

        End With

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function Transportes(documento_ As DocumentoElectronico)

        '------------Transportes---------------------------------------

        _Nivel2 = New Table({60.0F, 790.0F, 150.0F}) '.UseAllAvailableWidth

        _dimensiones = {140.0F, 600.0F, 50.0F}

        _tablalayout = New Table({60.0F}) '.UseAllAvailableWidth

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.SetBorder(NO_BORDER)

        '_tablalayout.AddCell(New Cell().Add(New Paragraph("TRANSPORTE").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY))
        _celdas = Nothing
        _tablalayout = _controladorPDF.setTablaLayout(_celdas, {60.0F}, TiposBordes.Niniguno, "TRANSPORTE")

        _tablalayout.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        With documento_.Seccion(SeccionesPedimento.ANS12)

            For Each transportes_ As Nodo In .Nodos

                If transportes_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim transporte_ = CType(transportes_, PartidaGenerica)

                    _celdas = New Dictionary(Of String, String) From {{"IDENTIFICACIÓN:", IIf(transporte_.Attribute(CA_NOMBRE_RAZON_SOCIAL_TRANSPORTE).Valor IsNot Nothing, transporte_.Attribute(CA_NOMBRE_RAZON_SOCIAL_TRANSPORTE).Valor, " ")},
                                                        {"PAÍS:", "VACIO"}}

                    _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

                    _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    _celdas = New Dictionary(Of String, String) From {{"VACIO", IIf(transporte_.Attribute(CA_CVE_PAIS_TRANSPORTE).Valor IsNot Nothing, transporte_.Attribute(CA_CVE_PAIS_TRANSPORTE).Valor, " ")}}

                    _tablalayout = _controladorPDF.setTablaLayout(_celdas, {150.0F}, TiposBordes.Niniguno)

                    _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                    Exit For

                End If

            Next

        End With

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderTop(NO_BORDER))
        '-------------------------------------------------------------------

    End Function

    Private Function Candados(documento_ As DocumentoElectronico)

        '-------------Candados----------------------------------------------

        If documento_.Seccion(SeccionesPedimento.ANS15).Nodos IsNot Nothing Then

            _Nivel2 = New Table({340.0F, 660.0F}) '.UseAllAvailableWidth

            _dimensiones = {110.0F, 110.0F, 110.0F, 110.0F, 110.0F, 110.0F}

            _Nivel2.SetBorder(NO_BORDER)

            _celdas = Nothing
            _tablalayout = _controladorPDF.setTablaLayout(_celdas, {340.0F}, TiposBordes.Niniguno, "NÚMERO DE CANDADO")

            _tablalayout.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

            With documento_.Seccion(SeccionesPedimento.ANS15)

                For Each candados_ As Nodo In .Nodos

                    If candados_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim candado_ = CType(candados_, PartidaGenerica)

                        _celdas = New Dictionary(Of String, String) From {{"VACIO", IIf(candados_.Attribute(CA_NUMERO_CANDADO).Valor IsNot Nothing, candados_.Attribute(CA_NUMERO_CANDADO).Valor, " ")},
                                                                          {"VACIO ", " "},
                                                                          {"VACIO  ", " "},
                                                                          {"VACIO   ", " "},
                                                                          {"VACIO    ", " "},
                                                                          {"VACIO     ", " "}}

                        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

                        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("1RA. REVISIÓN" & CStr(IIf(candados_.Attribute(CA_NUMERO_CANDADO_1RA).Valor IsNot Nothing, candados_.Attribute(CA_NUMERO_CANDADO_1RA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("2DA. REVISIÓN" & CStr(IIf(candados_.Attribute(CA_NUMERO_CANDADO_2DA).Valor IsNot Nothing, candados_.Attribute(CA_NUMERO_CANDADO_2DA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

                    End If

                Next

            End With

            _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        End If

    End Function

    Private Function GuiaOrdenEmbarque(documento_ As DocumentoElectronico)

        '-------------Guía/orden de embarque-------------------------------

        If documento_.Seccion(SeccionesPedimento.ANS16).Nodos IsNot Nothing Then

            _Nivel2 = New Table({400.0F, 600.0F}) '.UseAllAvailableWidth

            _dimensiones = {150.0F, 50.0F, 150.0F, 50.0F, 150.0F, 50.0F}

            _celdas = Nothing
            _tablalayout = _controladorPDF.setTablaLayout(_celdas, {400.0F}, TiposBordes.Niniguno, "NÚMERO (GUÍA/ORDEN EMBARQUE)/ID:")

            _tablalayout.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

            _Nivel2.SetBorder(NO_BORDER)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

            With documento_.Seccion(SeccionesPedimento.ANS16)

                For Each candados_ As Nodo In .Nodos

                    If candados_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim candado_ = CType(candados_, PartidaGenerica)

                        _celdas = New Dictionary(Of String, String) From {{"VACIO", IIf(candado_.Attribute(CA_GUIA_MANIFIESTO_BL).Valor IsNot Nothing, candado_.Attribute(CA_GUIA_MANIFIESTO_BL).Valor, " ")},
                                                                          {"VACIO ", IIf(candado_.Attribute(CA_MASTER_HOUSE).Valor IsNot Nothing, candado_.Attribute(CA_MASTER_HOUSE).Valor, " ")},
                                                                          {"VACIO  ", " "},
                                                                          {"VACIO   ", " "},
                                                                          {"VACIO    ", " "},
                                                                          {"VACIO     ", " "}}

                        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

                        _tablalayout.SetBorderTop(NO_BORDER)

                        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                    End If

                Next

            End With

            _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderTop(NO_BORDER))

        End If

    End Function

    Private Function Numero_o_Tipo(documento_ As DocumentoElectronico)

        '-------------Número o tipo-------------------------------

        If documento_.Seccion(SeccionesPedimento.ANS17).Nodos IsNot Nothing Then

            _Nivel2 = New Table({400.0F, 600.0F}) '.UseAllAvailableWidth

            _dimensiones = {150.0F, 50.0F, 150.0F, 50.0F, 150.0F, 50.0F}

            _celdas = Nothing
            _tablalayout = _controladorPDF.setTablaLayout(_celdas, {400.0F}, TiposBordes.Niniguno, "NÚMERO / TIPO")

            _tablalayout.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

            _Nivel2.SetBorder(NO_BORDER)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

            With documento_.Seccion(SeccionesPedimento.ANS17)

                Dim noPartidas_ As Integer = documento_.Seccion(SeccionesPedimento.ANS17).Nodos.Count

                _celdas = New Dictionary(Of String, String)

                _espacios = ""

                For Each candados_ As Nodo In .Nodos

                    If candados_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim candado_ = CType(candados_, PartidaGenerica)

                        _celdas.Add("VACIO" + _espacios, IIf(candados_.Attribute(CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO).Valor IsNot Nothing, candados_.Attribute(CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO).Valor, " "))
                        _celdas.Add("VACIO " + _espacios, _vinculacion)

                        _espacios += "  "

                    End If

                Next

                If noPartidas_ = 1 Then

                    _celdas.Add("VACIO" + _espacios, "         ")
                    _celdas.Add("VACIO " + _espacios, "     ")
                    _celdas.Add("VACIO  " + _espacios, "         ")
                    _celdas.Add("VACIO   " + _espacios, "     ")

                ElseIf noPartidas_ = 2 Then

                    _celdas.Add("VACIO" + _espacios, "         ")
                    _celdas.Add("VACIO " + _espacios, "     ")

                End If

                _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

                _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

            End With

            _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        End If

    End Function

    Private Function Identificadores(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {4.0F, 1.0F, 3.0F, 3.0F, 3.0F}

        _celdas = New Dictionary(Of String, String) From {{"CLAVE / COMPL. IDENTIFICADOR", "VACIO"},
                                                        {" ", "VACIO"},
                                                        {"COMPLEMENTO 1", "VACIO"},
                                                        {"COMPLEMENTO 2", "VACIO"},
                                                        {"COMPLEMENTO 3", "VACIO"}}

        _espacios = ""

        With documento_.Seccion(SeccionesPedimento.ANS18)

            For Each identificadores_ As Nodo In .Nodos

                If identificadores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim identificador_ = CType(identificadores_, PartidaGenerica)

                    _celdas.Add("VACIO" + _espacios, " ")
                    _celdas.Add("VACIO " + _espacios, IIf(identificador_.Attribute(CA_CVE_IDENTIFICADOR).Valor IsNot Nothing, identificador_.Attribute(CA_CVE_IDENTIFICADOR).Valor, " "))
                    _celdas.Add("VACIO  " + _espacios, IIf(identificador_.Attribute(CA_COMPLEMENTO_1).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_1).Valor, " "))
                    _celdas.Add("VACIO   " + _espacios, IIf(identificador_.Attribute(CA_COMPLEMENTO_2).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_2).Valor, " "))
                    _celdas.Add("VACIO    " + _espacios, IIf(identificador_.Attribute(CA_COMPLEMENTO_3).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_3).Valor, " "))

                    _espacios += "     "

                End If

            Next

        End With

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function E_Documents(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _tablalayout = New Table({200.0F, 200.0F, 200.0F, 200.0F, 200.0F}).UseAllAvailableWidth

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NUMERO DE E-DOCUMENT").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function CuentasAduaneras(ByVal documento_ As DocumentoElectronico) As String

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {200.0F, 50.0F, 200.0F, 50.0F, 200.0F, 50.0F, 200.0F, 50.0F}

        _celdas = New Dictionary(Of String, String) From {{"TIPO CUENTA: ", "1"},
                                                          {"CLAVE GARANTIA: ", "2"},
                                                          {"INSTITUCION EMISORA: ", "3"},
                                                          {"NUMERO DE CONTRATO: ", "4"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "CUENTAS ADUANERAS Y CUENTAS ADUANERAS DE GARANTIA")

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _dimensiones = {300.0F, 300.0F, 300.0F}

        _celdas = New Dictionary(Of String, String) From {{"FOLIO CONSTANCIA: ", "VACIO"},
                                                          {"TOTAL DEPOSITO: ", "VACIO"},
                                                          {"FECHA CONSTANCIA: ", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function Descargos(ByVal documento_ As DocumentoElectronico) As String

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {300.0F, 300.0F, 300.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO ORIGINAL: ", "VACIO"},
                                                          {"FECHA DE OPERACION ORIGINAL: ", "VACIO"},
                                                          {"CVE. PEDIMENTO ORIGINAL: ", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "DESCARGOS")

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "881818 11818 18"},
                                                          {"VACIO ", "25/02/2023"},
                                                          {"VACIO  ", "D3"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function Compensaciones(ByVal documento_ As DocumentoElectronico) As String

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {300.0F, 700.0F}

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO ORIGINAL: ", "VACIO"},
                                                          {"FECHA DE OPERACION ORIGINAL:  CLAVE DEL GRAVAMEN: IMPORTE DEL GRAVAMEN:", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha, "COMPENSACIONES")

        _tablalayout.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "881818 11818 18"},
                                                          {"VACIO ", "25/02/2023  DFFD55   5665"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function FormasPagoVirtuales(ByVal documento_ As DocumentoElectronico) As String

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {100.0F, 153.0F, 153.0F, 153.0F, 153.0F, 153.0F, 100.0F}

        _celdas = New Dictionary(Of String, String) From {{"FORMA DE PAGO", "VACIO"},
                                                          {"DEPENDENCIA O INSTITUCION EMISORA", "VACIO"},
                                                          {"NUMERO DE IDENTIFICACION DEL DOCUMENTO", "VACIO"},
                                                          {"FECHA DEL DOCUMENTO", "VACIO"},
                                                          {"IMPORTE DEL DOCUMENTO", "VACIO"},
                                                          {"SALDO DISPONIBLE", "VACIO"},
                                                          {"IMPORTE A PAGAR", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "FORMAS DE PAGO VIRTUALES")

        _tablalayout.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "EFEC"},
                                                          {"VACIO ", "LLOK"},
                                                          {"VACIO  ", "EFEC"},
                                                          {"VACIO   ", "LLOK"},
                                                          {"VACIO    ", "EFEC"},
                                                          {"VACIO     ", "LLOK"},
                                                          {"VACIO      ", "LLOK"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function Observaciones(documento_ As DocumentoElectronico)

        _dimensiones = {1000.0F}

        _celdas = New Dictionary(Of String, String)

        Dim observacioPartida_ As String = ""

        With documento_.Seccion(SeccionesPedimento.ANS23)

            For Each observaciones_ As Nodo In .Nodos

                If observaciones_.TipoNodo = Nodo.TiposNodo.Nodo Then

                    observacioPartida_ += CStr(IIf(documento_.Attribute(CA_OBSERVACIONES_PEDIMENTO).Valor IsNot Nothing, documento_.Attribute(CA_OBSERVACIONES_PEDIMENTO).Valor, " "))

                End If

            Next

            _celdas.Add("VACIO" + _espacios, observacioPartida_)

        End With

        _Nivel2 = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno, "OBSERVACIONES")

        '_Nivel2.AddStyle(_estiloSinBordes)

        '_Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function Partidas(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _Nivel3 = New Table({1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F})

        '_dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        '_tablalayout.SetBorder(NO_BORDER)

        _Nivel2.SetBorder(NO_BORDER)

        _Nivel3.AddStyle(_estiloSinBordes)

        If documento_.Seccion(SeccionesPedimento.ANS24).Nodos IsNot Nothing Then

            With documento_.Seccion(SeccionesPedimento.ANS24)

                '------------Encabezado partidas--------------------------
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=16).Add(New Paragraph("PARTIDASS").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetMultipliedLeading(1)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("FRACCIÓN").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("SUBD/NÚM.ID ENTIFICACIÓN COMERCIAL").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("VINC.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("MET VAL.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("UMC").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMC").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("UMT").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMT").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("P.V/C").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("P.O/D").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=5).Add(New Div().Add(New Paragraph(" ")).Add(New Paragraph(" ")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("SEC").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=10).Add(New Paragraph("DESCRIPCION (RENGLONES VARIABLES SEGUN SE REQUIERA)").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("CON.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("TASA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("T.T").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("T.P").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("IMPORTE").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("VAL ADU/USD").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("IMP. PRECIO PAG.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("PRECIO UNIT.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("VAL. AGREG.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))


                _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("MARCA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph("MODELO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("CÓDIGO PRODUCTO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                _Nivel3.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))


                'Identificadores y NOMS

                For Each partidas_ As Nodo In .Nodos

                    _dimensiones = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

                    _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

                    _tablalayout.SetBorder(NO_BORDER)

                    Dim layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                    Dim Nivel4_ As Table = New Table({1000.0F}).UseAllAvailableWidth

                    Nivel4_.SetBorder(NO_BORDER)

                    If partidas_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim partida_ = CType(partidas_, PartidaGenerica)

                        Dim noTasas_ As Integer

                        noTasas_ = partida_.Seccion(SeccionesPedimento.ANS29).Nodos.Count

                        _Nivel3.AddCell(New Cell(rowspan:=noTasas_, colspan:=0).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_SECUENCIA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_SECUENCIA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_NICO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_NICO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_VINCULACION).Valor IsNot Nothing, partida_.Attribute(CA_VINCULACION).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_METODO_VALORACION_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_METODO_VALORACION_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CANTIDAD_UMC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CANTIDAD_UMC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CANTIDAD_UMT_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CANTIDAD_UMT_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_PAIS_VENDEDOR_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_PAIS_VENDEDOR_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_PAIS_ORIGEN_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_PAIS_ORIGEN_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=10).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_DESCRIPCION_MERCANCIA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_DESCRIPCION_MERCANCIA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_VALOR_ADUANA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_VALOR_ADUANA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_PRECIO_PAGADO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_PRECIO_PAGADO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_PRECIO_UNITARIO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_PRECIO_UNITARIO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Dim montoValor_ As String
                        If partida_.Attribute(CA_VALOR_AGREGADO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_VALOR_AGREGADO_PARTIDA).Valor > 0 Then
                            montoValor_ = partida_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor.ToString
                        Else
                            montoValor_ = "0"
                        End If

                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(montoValor_).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        'Marca producto
                        If partida_.Attribute(CA_MARCA_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_MARCA_PARTIDA).Valor <> "" Then

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_MARCA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_MARCA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Modelo producto
                        If partida_.Attribute(CA_MODELO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_MODELO_PARTIDA).Valor <> "" Then

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_MODELO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_MODELO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Codigo de producto
                        If partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor <> "" Then

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Regulaciones y restricciones no arancelarias
                        If partida_.Seccion(SeccionesPedimento.ANS26).Nodos IsNot Nothing Then

                            layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("CLAVE").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("NUM. PERMISO O NOM").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("FIRMA DESCARGO").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("VAL. COM. DLS.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMT/C").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With documento_.Seccion(SeccionesPedimento.ANS26)

                                For Each regulaciones_ As Nodo In .Nodos

                                    If regulaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim regulacion_ = CType(regulaciones_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_CVE_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_CVE_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_NUMERO_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_NUMERO_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_FIRMA_ELECTRONICA_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_FIRMA_ELECTRONICA_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_VALOR_USD_PARTIDA).Valor IsNot Nothing, regulacion_.Attribute(CA_VALOR_USD_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_CANTIDAD_UMT_UMC).Valor IsNot Nothing, regulacion_.Attribute(CA_CANTIDAD_UMT_UMC).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Identificadores nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS27).Nodos IsNot Nothing Then

                            layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("IDENTIF.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 1").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 2").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 3").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With partida_.Seccion(SeccionesPedimento.ANS27)

                                For Each identificadores_ As Nodo In .Nodos

                                    If identificadores_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim identificador_ = CType(identificadores_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_CVE_IDENTIFICADOR_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_CVE_IDENTIFICADOR_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPLEMENTO_1_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_1_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPLEMENTO_2_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_2_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPLEMENTO_3_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_3_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Observaciones nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS36) IsNot Nothing Then

                            layout_ = New Table({100.0F}).UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("OBSERVACIONES A NIVEL PARTIDA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With documento_.Seccion(SeccionesPedimento.ANS36)

                                For Each observaciones_ As Nodo In .Nodos

                                    If observaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim observacion_ = CType(observaciones_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(observacion_.Attribute(CA_OBSERVACIONES_PARTIDA).Valor IsNot Nothing, observacion_.Attribute(CA_OBSERVACIONES_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_estiloSinBordes).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        End If

                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=10).Add(Nivel4_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        _Nivel3.AddCell(New Cell(rowspan:=noTasas_, colspan:=10).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        'Tasas a nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS36) IsNot Nothing Then

                            _dimensiones = {150.0F, 150.0F, 200.0F, 200.0F, 300.0F}

                            layout_ = New Table(_dimensiones) '.UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            Dim incrementable_ As Integer = 1

                            With partida_.Seccion(SeccionesPedimento.ANS29)

                                For Each tasasPartidas_ As Nodo In .Nodos

                                    If tasasPartidas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim tasaPartida_ = CType(tasasPartidas_, PartidaGenerica)

                                        If incrementable_ = 1 Then

                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CVE_CONTRIBUCION_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CVE_CONTRIBUCION_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CVE_TIPO_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CVE_TIPO_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_FORMA_PAGO_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_FORMA_PAGO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                        Else

                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CVE_CONTRIBUCION_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CVE_CONTRIBUCION_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CVE_TIPO_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CVE_TIPO_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_FORMA_PAGO_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_FORMA_PAGO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                        End If

                                    End If

                                    incrementable_ += 1

                                Next

                            End With

                        End If

                    End If

                Next

            End With

        End If


        '-----Agregamos el nivel 3 al nivel 2 así como, el nivel 2 al contenedor principal

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function Partidas2(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        Dim layout_ As Table

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("PARTIDAS").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        _Nivel3 = New Table({40.0F, 110.0F, 130.0F, 50.0F, 50.0F, 50.0F, 100.0F, 50.0F, 100.0F, 50.0F, 50.0F, 40.0F, 40.0F, 30.0F, 30.0F, 80.0F}).UseAllAvailableWidth

        If documento_.Seccion(SeccionesPedimento.ANS24).Nodos IsNot Nothing Then

            With documento_.Seccion(SeccionesPedimento.ANS24)

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("FRACCION").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("SUBD/NÚM. IDENTIFICACIÓN COMERCIAL").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("VINC.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("MET VAL").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("UMC").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("CANTIDAD UMC").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("UMT").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("CANTIDAD UMT").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("P. V/C").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("P. O/D").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                '----


                _Nivel3.AddCell(New Cell().Add(New Paragraph("SEC").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=10).Add(New Paragraph("DESCRIPCION (RENGLONES VARIABLES SEGÚN SE REQUIERA)").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("CON.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("TASA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("T.T").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("T.P").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph("IMPORTE").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                        SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                '----


                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("VAL ADU/USD").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("IMP. PRECIO PAG").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("PRECIO UNIT.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("VAL. AGREG").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                '-----

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("MARCA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph("MODELO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("CODIGO PRODUCTO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                        SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                        SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                For Each partidas_ As Nodo In .Nodos

                    Dim Nivel4_ As Table = New Table({1000.0F}).UseAllAvailableWidth

                    Nivel4_.SetBorder(NO_BORDER)

                    If partidas_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim partida_ = CType(partidas_, PartidaGenerica)

                        Dim noTasas_ = partida_.Seccion(SeccionesPedimento.ANS29).Nodos.Count

                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_SECUENCIA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_SECUENCIA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_FRACCION_ARANCELARIA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_NICO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_NICO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_VINCULACION).Valor IsNot Nothing, partida_.Attribute(CA_VINCULACION).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_METODO_VALORACION_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_METODO_VALORACION_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CANTIDAD_UMC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CANTIDAD_UMC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CANTIDAD_UMT_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CANTIDAD_UMT_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_PAIS_VENDEDOR_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_PAIS_VENDEDOR_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_PAIS_ORIGEN_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_PAIS_ORIGEN_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph("16.0000").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph("3").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=10).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_DESCRIPCION_MERCANCIA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_DESCRIPCION_MERCANCIA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph("3").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph("0").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph("119984").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_VALOR_ADUANA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_VALOR_ADUANA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_PRECIO_PAGADO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_PRECIO_PAGADO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_PRECIO_UNITARIO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_PRECIO_UNITARIO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Dim montoValor_ As String
                        If partida_.Attribute(CA_VALOR_AGREGADO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_VALOR_AGREGADO_PARTIDA).Valor > 0 Then
                            montoValor_ = partida_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor.ToString
                        Else
                            montoValor_ = "0"
                        End If

                        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(montoValor_).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        'Marca producto
                        If partida_.Attribute(CA_MARCA_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_MARCA_PARTIDA).Valor <> "" Then

                            _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_MARCA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_MARCA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Modelo producto
                        If partida_.Attribute(CA_MODELO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_MODELO_PARTIDA).Valor <> "" Then

                            _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_MODELO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_MODELO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Codigo de producto
                        If partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor <> "" Then

                            _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        End If

                        'Regulaciones y restricciones no arancelarias
                        If partida_.Seccion(SeccionesPedimento.ANS26).Nodos IsNot Nothing Then

                            layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("CLAVE").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("NUM. PERMISO O NOM").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("FIRMA DESCARGO").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("VAL. COM. DLS.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMT/C").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With documento_.Seccion(SeccionesPedimento.ANS26)

                                For Each regulaciones_ As Nodo In .Nodos

                                    If regulaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim regulacion_ = CType(regulaciones_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_CVE_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_CVE_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_NUMERO_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_NUMERO_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_FIRMA_ELECTRONICA_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_FIRMA_ELECTRONICA_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_VALOR_USD_PARTIDA).Valor IsNot Nothing, regulacion_.Attribute(CA_VALOR_USD_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_CANTIDAD_UMT_UMC).Valor IsNot Nothing, regulacion_.Attribute(CA_CANTIDAD_UMT_UMC).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Identificadores nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS27).Nodos IsNot Nothing Then

                            layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("IDENTIF.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 1").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 2").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 3").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With partida_.Seccion(SeccionesPedimento.ANS27)

                                For Each identificadores_ As Nodo In .Nodos

                                    If identificadores_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim identificador_ = CType(identificadores_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_CVE_IDENTIFICADOR_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_CVE_IDENTIFICADOR_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPLEMENTO_1_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_1_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPLEMENTO_2_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_2_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPLEMENTO_3_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPLEMENTO_3_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Observaciones nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS36) IsNot Nothing Then

                            layout_ = New Table({100.0F}).UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("OBSERVACIONES A NIVEL PARTIDA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With documento_.Seccion(SeccionesPedimento.ANS36)

                                For Each observaciones_ As Nodo In .Nodos

                                    If observaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim observacion_ = CType(observaciones_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(observacion_.Attribute(CA_OBSERVACIONES_PARTIDA).Valor IsNot Nothing, observacion_.Attribute(CA_OBSERVACIONES_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_estiloSinBordes).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        End If


                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell(rowspan:=0, colspan:=10).Add(Nivel4_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))


                    End If

                Next

            End With

        End If

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function DeterminacionContribucionPartidaTMEC(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("DETERMINACION DE CONTRIBUCIONES A NIVEL PARTIDA").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        _tablalayout = New Table({50.0F, 100.0F, 150.0F, 200.0F, 200.0F, 150.0F, 50.0F, 100.0F}).UseAllAvailableWidth

        _tablalayout.AddCell(New Cell(rowspan:=2, colspan:=0).Add(New Paragraph("SEC").SetTextAlignment(TextAlignment.CENTER).
                                SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("FRACCION").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("VALOR MERC. NO ORIG.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("MONTO IGI").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("TOTAL ARAN EUA/CAN").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("MONTO EXENT.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell(rowspan:=2, colspan:=0).Add(New Paragraph("F.P").SetTextAlignment(TextAlignment.CENTER).
                                SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell(rowspan:=2, colspan:=0).Add(New Paragraph("IMPORTE").SetTextAlignment(TextAlignment.CENTER).
                                SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("UMT").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("CANT. UMT").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("FRACC. EUA/CAN").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("TASA EUA/CAN").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("ARAN. EUA/CAN").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

    End Function

    Private Function DeterminacionContribucionPartidaTLCA(documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {50.0F, 150.0F, 400.0F, 150.0F, 100.0F, 150.0F}

        _celdas = New Dictionary(Of String, String) From {{"SEC", "VACIO"},
                                                          {"FRACCION", "VACIO"},
                                                          {"VALOR MERCADO NO ORIG", "VACIO"},
                                                          {"MONTO IGI", "VACIO"},
                                                          {"F.P", "VACIO"},
                                                          {"IMPORTE", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "DETERMINACION DE CONTRIBUCIONES A NIVEL PARTIDA")

        _tablalayout.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

    End Function

    Private Function Rectificacion(ByVal documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {250.0F, 250.0F, 250.0F, 250.0F}

        _celdas = New Dictionary(Of String, String) From {{"PEDIMENTO ORIGINAL", "VACIO"},
                                                          {"CVE. PEDIM. ORIGINAL", "VACIO"},
                                                          {"CVE. PEDIM. RECT.", "VACIO"},
                                                          {"FECHA PAGO RECT", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "RECTIFICACION")

        _tablalayout.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "3115 555 51116"},
                                                          {"VACIO ", "336"},
                                                          {"VACIO  ", "335"},
                                                          {"VACIO   ", "21/05/2023"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

    End Function

    Private Function DiferenciasContribuciones(documento_ As DocumentoElectronico)

        '------------CUADRO DE LIQUIDACIONES------------------------------------
        _Nivel3 = New Table({700.0F, 295.0F})

        _Nivel2 = New Table({1000.0F})

        _Nivel2.AddCell(New Cell().Add(New Paragraph("DIFERENCIAS DE CONTRIBUCIONES A NIVEL PEDIMENTO").SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1).SetTextAlignment(TextAlignment.CENTER).
                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _dimensiones = {150.0F, 50.0F, 150.0F, 150.0F, 50.0F, 150.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth
        _celdas = New Dictionary(Of String, String) From {{"CONCEPTO", "VACIO"},
                                                          {"F.P.", "VACIO"},
                                                          {"DIFERENCIA", "VACIO"},
                                                          {"CONCEPTO ", "VACIO"},
                                                          {"F.P. ", "VACIO"},
                                                          {"DIFERENCIA ", "VACIO"}}

        Dim incrementableLiq_ As Integer = 0

        _espacios = ""

        With documento_.Seccion(SeccionesPedimento.ANS7)

            With documento_.Seccion(SeccionesPedimento.ANS55)

                For Each liquidaciones_ As Nodo In .Nodos

                    If liquidaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim liquidacion_ = CType(liquidaciones_, PartidaGenerica)
                        _celdas.Add("VACIO" + _espacios, IIf(liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion IsNot Nothing, liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion, " "))
                        _celdas.Add("VACIO " + _espacios, IIf(liquidacion_.Attribute(CA_FORMA_PAGO).Valor IsNot Nothing, liquidacion_.Attribute(CA_FORMA_PAGO).Valor, " "))
                        _celdas.Add("VACIO  " + _espacios, IIf(liquidacion_.Attribute(CA_IMPORTE).Valor IsNot Nothing, liquidacion_.Attribute(CA_IMPORTE).Valor, " "))

                        _espacios += "   "

                        incrementableLiq_ += 1

                    End If

                Next

                If incrementableLiq_ < 6 Then

                    _espacios += "   "

                    _celdas.Add("VACIO" + _espacios, " ")
                    _celdas.Add("VACIO " + _espacios, "0")
                    _celdas.Add("VACIO  " + _espacios, "11053")

                    Select Case incrementableLiq_

                        Case 1

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 2

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 3

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, "0")
                            _celdas.Add("VACIO  " + _espacios, "86352")

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                        Case 4

                            _espacios += "   "
                            _celdas.Add("VACIO" + _espacios, " ")
                            _celdas.Add("VACIO " + _espacios, " ")
                            _celdas.Add("VACIO  " + _espacios, " ")

                    End Select

                End If

            End With

        End With

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER)) '.SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '-------------TOTALES---------------------------------------------------------
        _dimensiones = {150.0F, 150.0F}

        '_tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"EFECTIVO", IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "101,879")},
                                                          {"OTROS", IIf(documento_.Attribute(CA_OTROS).Valor IsNot Nothing, documento_.Attribute(CA_OTROS).Valor, "0")},
                                                          {"DIF. TOTALES", IIf(documento_.Attribute(CA_TOTAL).Valor IsNot Nothing, documento_.Attribute(CA_TOTAL).Valor, "101,879")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "DIFERENCIAS TOTALES")

        _tablalayout.SetFontSize(7.0F)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
        '----------------------------------------------------------------------------------

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(NO_BORDER))


    End Function

    Private Function PruebaSuficiente(ByVal documento_ As DocumentoElectronico)

        _Nivel2 = New Table({1000.0F}).UseAllAvailableWidth

        _dimensiones = {350.0F, 350.0F, 300.0F}

        _celdas = New Dictionary(Of String, String) From {{"PAIS DESTINO", "VACIO"},
                                                          {"NUM. PEDIMENTO EUA/CAN", "VACIO"},
                                                          {"PRUEBA SUFICIENTE", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "PRUEBA SUFICIENTE")

        _tablalayout.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

    End Function

    Private Function PiePagina(ByVal documento_ As DocumentoElectronico)

#Region "Pie de pagina"

        _Nivel2 = New Table({1000.0F}) '.UseAllAvailableWidth

        _Nivel3 = New Table({500.0F, 500.0F})

        _dimensiones = {100.0F, 400.0F, 100.0F, 400.0F}

        _tablalayout = New Table(_dimensiones)

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "VACIO"},
                                                          {"AGENTE ADUANAL, AGENCIA ADUANAL, APODERADO ADUANAL O DE ALMACEN NOMBRE O RAZ. SOC.:", "VACIO"},
                                                          {"VACIO ", "VACIO"},
                                                          {"VACIO   ", IIf(documento_.Attribute(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA).Valor, "JESUS ENRIQUE GOMEZ REYES - GRUPO REYES KURI S.C. GORJ800903BA")},
                                                          {"RFC:", IIf(documento_.Attribute(CA_RFC_AA).Valor IsNot Nothing, documento_.Attribute(CA_RFC_AA).Valor, " ")},
                                                          {"CURP:", IIf(documento_.Attribute(CA_CURP_AA_REPRESENTANTE_LEGAL).Valor IsNot Nothing, documento_.Attribute(CA_CURP_AA_REPRESENTANTE_LEGAL).Valor, " ")},
                                                          {"VACIO    ", "VACIO"},
                                                          {"MANDATARIO/PERSONA AUTORIZADA", "VACIO"},
                                                          {"VACIO     ", "VACIO"},
                                                          {"NOMBRE:", IIf(documento_.Attribute(CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA).Valor, " ")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN).Valor IsNot Nothing, documento_.Attribute(CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN).Valor IsNot Nothing, documento_.Attribute(CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN).Valor, " ")}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Niniguno)

        _tablalayout.AddStyle(_estiloSinBordes)

        _Nivel2.SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph("NÚMERO DE SERIE DEL CERTIFICADO: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor IsNot Nothing, documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel3.AddCell(New Cell().Add(New Paragraph("DECLARO BAJO PROTESTA DE DECIR VERDAD, EN LOS TERMINOS DE LO DISPUESTO POR EL ARTICULO 81 DE LA LEY ADUANERA: PATENTE O AUTORIZACION: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_PATENTE).Valor IsNot Nothing, documento_.Attribute(CA_PATENTE).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Div().Add(New Paragraph("e.firma: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_EFIRMA).Valor IsNot Nothing, documento_.Attribute(CA_EFIRMA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F))).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Div().Add(New Paragraph(" ")).Add(New Paragraph(" "))).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddFooterCell(New Cell(rowspan:=0, colspan:=2).Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderTop(NO_BORDER))

#End Region

    End Function

    Private Function FinPedimento()
#Region "Fin del pedimento"

        _dimensiones = {4.0F, 2.0F, 4.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.AddStyle(_estiloSinBordes)

        '_tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("********* ")).Add(New Paragraph(documento_.Attribute(CA_FIN_PEDIMENTO).Valor.ToString)).Add(New Paragraph(" *********"))))
        '_tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("NUM. TOTAL DE PARTIDAS: ")).Add(New Paragraph(documento_.Attribute(CA_NUMERO_TOTAL_PARTIDAS).Valor.ToString))))
        '_tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("**** CLAVE DE PREVALIDADOR: ")).Add(New Paragraph(documento_.Attribute(CA_CVE_PREVAL).Valor.ToString)).Add(New Paragraph(" ****"))))

        _tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("********* FIN DE PEDIMENTO *********").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))
        _tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("NUM. TOTAL DE PARTIDAS: 13").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))
        _tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("**** CLAVE DE PREVALIDADOR: 010 ****").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))


        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region
    End Function

    Private Function FinImpresion()
#Region "Fin del pedimento"

        _dimensiones = {4.0F, 2.0F, 4.0F}

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.AddStyle(_estiloSinBordes)

        _tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("************************ FIN DE LA IMPRESION ************************").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))

        _Nivel1.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region
    End Function

End Class

Public Class WatherMark
    Implements iText.Kernel.Events.IEventHandler

    Protected _image As Image
    Protected _gState As PdfExtGState

    Public Sub New(ByVal image_ As Image)

        _gState = New PdfExtGState().SetFillOpacity(0.3F)

        _image = image_

    End Sub

    Public Sub HandleEvent([event] As [Event]) Implements iText.Kernel.Events.IEventHandler.HandleEvent

        Dim pdfDocEvent_ As PdfDocumentEvent = [event]

        Dim pdfDoc_ As PdfDocument = pdfDocEvent_.GetDocument()

        Dim page_ As PdfPage = pdfDocEvent_.GetPage()

        Dim pageSize_ As Rectangle = page_.GetPageSize()

        Dim pdfCanvas_ As New PdfCanvas(page_.GetLastContentStream(), page_.GetResources(), pdfDoc_)

        pdfCanvas_.SaveState().SetExtGState(_gState)

        Dim canvas_ As New Canvas(pdfCanvas_, pageSize_) ', page_.GetPageSize())

        canvas_.Add(_image.ScaleAbsolute(pageSize_.GetWidth(), pageSize_.GetHeight()))

        pdfCanvas_.RestoreState()

        pdfCanvas_.Release()


    End Sub

End Class

Public Class PageEvent
    Implements iText.Kernel.Events.IEventHandler

    Private ReadOnly documento As Document
    Private _tablalayout As Table

    Public Sub New(doc As Document, tablalayout_ As Table)
        documento = doc
        _tablalayout = tablalayout_
    End Sub

    Private Function CrearEncabezadoRectangulo(docEvent As PdfDocumentEvent) As Rectangle
        Dim pdfDoc As PdfDocument = docEvent.GetDocument()
        Dim page As PdfPage = docEvent.GetPage()

        Dim xEncabezado As Single = pdfDoc.GetDefaultPageSize().GetX() + documento.GetLeftMargin()
        Dim yEncabezado As Single = pdfDoc.GetDefaultPageSize().GetTop() - documento.GetTopMargin() - 47
        Dim anchoEncabezado As Single = page.GetPageSize().GetWidth() - 92
        Dim altoEncabezado As Single = 50.0F

        Dim rectanguloEncabezado As New Rectangle(xEncabezado, yEncabezado, anchoEncabezado, altoEncabezado)

        Return rectanguloEncabezado
    End Function

    Private Function CrearPieRectangulo(docEvent As PdfDocumentEvent) As Rectangle
        Dim pdfDoc As PdfDocument = docEvent.GetDocument()
        Dim page As PdfPage = docEvent.GetPage()

        Dim xPie As Single = pdfDoc.GetDefaultPageSize().GetX() + documento.GetLeftMargin()
        Dim yPie As Single = pdfDoc.GetDefaultPageSize().GetBottom()
        Dim anchoPie As Single = page.GetPageSize().GetWidth() - 72
        Dim altoPie As Single = 50.0F

        Dim rectanguloPie As New Rectangle(xPie, yPie, anchoPie, altoPie)

        Return rectanguloPie
    End Function

    Private Function CrearEncabezadoTabla(mensaje As String) As Table
        Dim anchos() As Single = {1.0F}
        Dim tablaEncabezado As New Table(anchos)
        tablaEncabezado.SetWidth(527.0F)

        tablaEncabezado.AddCell(mensaje)

        Return tablaEncabezado
    End Function

    Private Function CrearPieTabla(docEvent As PdfDocumentEvent) As Table
        Dim page As PdfPage = docEvent.GetPage
        Dim anchos() As Single = {1.0F}
        Dim tablaPie As New Table(anchos)
        tablaPie.SetWidth(527.0F)
        Dim pageNum As Integer = docEvent.GetDocument.GetPageNumber(page)

        tablaPie.AddCell("Página " & pageNum)

        Return tablaPie
    End Function

    Public Sub HandleEvent([event] As [Event]) Implements iText.Kernel.Events.IEventHandler.HandleEvent
        Dim docEvent As PdfDocumentEvent = [event]
        Dim pdfDoc As PdfDocument = docEvent.GetDocument()
        Dim page As PdfPage = docEvent.GetPage()
        Dim canvas As New PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc)
        Dim pageNumber As Integer = pdfDoc.GetPageNumber(docEvent.GetPage())
        Dim Nivel1_ = New Table({1000.0F}).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)
        Dim _Nivel2 = New Table({150.0F}).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)


        _Nivel2.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        If pageNumber > 1 Then
            'Dim tablaEncabezado As Table = CrearEncabezadoTabla("Departamento de Recursos Humanos")
            _tablalayout.SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)
            Dim rectanguloEncabezado As Rectangle = CrearEncabezadoRectangulo(docEvent)
            Dim canvasEncabezado As New Canvas(canvas, rectanguloEncabezado)
            _Nivel2.AddCell(New Cell().Add(New Paragraph("ANEXO DEL PEDIMENTO").SetMultipliedLeading(1).SetTextAlignment(TextAlignment.CENTER).SetFont(PdfFontFactory.CreateFont("C:/Windows/Fonts/Arialbd.ttf", PdfEncodings.IDENTITY_H)).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
            Nivel1_.AddCell(New Cell().Add(_Nivel2).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))
            Nivel1_.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
            Nivel1_.SetBorder(NO_BORDER)
            canvasEncabezado.Add(Nivel1_)
        End If

        Dim tablaNumeracion As Table = CrearPieTabla(docEvent)
        Dim rectanguloPie As Rectangle = CrearPieRectangulo(docEvent)
        Dim canvasPie As New Canvas(canvas, rectanguloPie)
        'canvasPie.Add(tablaNumeracion)
    End Sub
End Class
