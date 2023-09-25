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
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesManifestacionValor
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

Public Class ConstructorManifestacionValorPDF


#Region "Atributos"

    Private _ms As MemoryStream = New MemoryStream()

    Private _pw As PdfWriter = New PdfWriter(_ms)

    Private _pdfDocument As PdfDocument = New PdfDocument(_pw)

    Private _doc As Document = New Document(_pdfDocument, PageSize.LETTER)

    Private _stylecell As Style = New Style

    Private _estiloSinBordes As Style = New Style

    Private _arialBold As PdfFont

    Private _arial As PdfFont

    Private _file As File

    Private _image As Image

    Private record As Object

    Private _watermark As Image

    Private _celdas As Dictionary(Of String, String)

    Private _celdas2 As Dictionary(Of String, Dictionary(Of String, List(Of Int64)))

    Private _controladorPDF As ControladorPDF

    Private _Nivel1 As Table

    Private _tablalayout As Table

    Private _Nivel2 As Table

    Private _Nivel3 As Table

    Private _dimensiones As Single()

#End Region

    Enum TiposBordes
        Ninguno = 0
        Derecha = 1
        DerechaAbajo = 2
        Abajo = 3
    End Enum

#Region "Constructores"

    Public Sub New()

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

        _controladorPDF = New ControladorPDF(_arialBold, _arial, _watermark, _estiloSinBordes)

    End Sub

#End Region

    Public Function ImprimirEncabezadoMV(ByVal documento_ As DocumentoElectronico) As String

        'Tabla principal
        _Nivel1 = New Table({1000.0F})

        _Nivel1.SetBorder(NO_BORDER)

#Region "Encabezado"

        _dimensiones = {200.0F, 600.0F, 200.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.SetBorder(NO_BORDER)

        _image = New Image(ImageDataFactory.Create("C:\TEMP\LogoSHCP.png"))

        _image.SetWidth(70)

        _tablalayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ")).Add(_image).SetHorizontalAlignment(HorizontalAlignment.CENTER).
                                                  SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                                                  SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("MANIFESTACIÓN DE VALOR").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                  SetFontSize(10.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetBorder(NO_BORDER)).SetBorder(NO_BORDER)

        _image = New Image(ImageDataFactory.Create("C:\TEMP\LogoSAT.png"))

        _image.SetWidth(80)

        _tablalayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph("")).Add(_image).SetHorizontalAlignment(HorizontalAlignment.CENTER).
                                                  SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                                                  SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(documento_.FolioDocumento).SetTextAlignment(TextAlignment.RIGHT).
                                                  SetMultipliedLeading(0.8).SetFont(_arial).SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                                                  SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("INFORMACIÓN GENERAL").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                                  SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(0.5)).
                                                  SetVerticalAlignment(VerticalAlignment.TOP).SetBorder(NO_BORDER)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("   ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(7.0F).
                                                  SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(0.5)).
                                                  SetVerticalAlignment(VerticalAlignment.TOP).SetBorder(NO_BORDER)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Número de hoja     1").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                                                  SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(0.5)).
                                                  SetVerticalAlignment(VerticalAlignment.TOP).SetBorder(NO_BORDER)).SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

#End Region

#Region "Informacion General"

#Region "A)"

        _Nivel2 = New Table({210.0F, 790.0F}).UseAllAvailableWidth

        _dimensiones = {210.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "a) Nombre o denominación social:")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY).
                                SetBorder(NO_BORDER).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Attribute(CA_RAZON_SOCIAL_PROVEEDOR).Valor IsNot Nothing,
                                                                             documento_.Attribute(CA_RAZON_SOCIAL_PROVEEDOR).Valor, ""))}}

        _dimensiones = {800.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Domicilio: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                               SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER)).SetMargins(0F, 0F, 0F, 0F).
                               SetPaddings(0F, 0F, 2.0F, 0F)

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_CALLE).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_CALLE).Valor, ""))},
                                                          {"VACIO ", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing,
                                                                              documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor, ""))},
                                                          {"VACIO  ", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing,
                                                                              documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor, ""))}}
        _dimensiones = {500.0F, 250.0F, 250.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                               SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Calle"},
                                                          {"VACIO ", "Número y/o letra exterior"},
                                                          {"VACIO  ", "Número y/o letra interior"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_CIUDAD).Valor IsNot Nothing,
                                                                        documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_CIUDAD).Valor, ""))},
                                                    {"VACIO ", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing,
                                                                        documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor, ""))},
                                                    {"VACIO  ", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor IsNot Nothing,
                                                                        documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, ""))},
                                                    {"VACIO   ", CStr(IIf(documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_PAIS).Valor IsNot Nothing,
                                                                        documento_.Seccion(SMV2).Attribute(CamposDomicilio.CA_PAIS).Valor, ""))}}
        _dimensiones = {250.0F, 100.0F, 250.0F, 400.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                               SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Ciudad"},
                                                          {"VACIO ", "Código Postal"},
                                                          {"VACIO  ", "Estado"},
                                                          {"VACIO   ", "País"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                               SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Teléfono"},
                                                          {"VACIO ", " "},
                                                          {"VACIO  ", "Correo Electrónico"},
                                                          {"VACIO   ", " "}}

        _dimensiones = {50.0F, 200.0F, 150.0F, 500.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

#End Region

#Region "B)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "b) Vinculación")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Señale con una X:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                   SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 0F, 10.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Existe vinculación entre importador y vendedor:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                   SetFontSize(7.0F).SetMultipliedLeading(1)).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 0F, 0.0F, 20.0F).SetBorder(NO_BORDER))

        _tablalayout = New Table({20.0F, 300.0F, 20.0F, 300.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(documento_.Attribute(CamposFacturaComercial.CA_CVE_VINCULACION).Valor = 1, "X", " "))).
                                   SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Si").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(CStr(IIf(documento_.Attribute(CamposFacturaComercial.CA_CVE_VINCULACION).Valor = 1, " ", "X"))).
                                   SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("No").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(5.0F, 5.0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Influyó en el valor de transacción:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                   SetMultipliedLeading(1)).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 0F, 0.0F, 20.0F).SetBorder(NO_BORDER))

        _tablalayout = New Table({20.0F, 300.0F, 20.0F, 300.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Si").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("No").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                   SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(5.0F, 5.0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "C)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "c) Datos del importador")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        _Nivel2 = New Table({200.0F, 800.0F}).UseAllAvailableWidth

        _dimensiones = {200.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "Nombre o denominación social: ")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", " "}}

        _dimensiones = {800.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor, ""))},
                                                          {"VACIO ", " "},
                                                          {"VACIO  ", " "}}

        _dimensiones = {400.0F, 300.0F, 300.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Apellido paterno"},
                                                          {"VACIO ", "Apellido materno"},
                                                          {"VACIO  ", "Nombre"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))

        '-----------RFC---------------

        Dim rfc_ = CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing,
                            documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RFC_CLIENTE).Valor, "010101010101"))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", rfc_(0)},
                                                          {"VACIO ", rfc_(1)},
                                                          {"VACIO  ", rfc_(2)},
                                                          {"VACIO   ", rfc_(3)},
                                                          {"VACIO    ", rfc_(4)},
                                                          {"VACIO     ", rfc_(5)},
                                                          {"VACIO      ", rfc_(6)},
                                                          {"VACIO       ", rfc_(7)},
                                                          {"VACIO        ", rfc_(8)},
                                                          {"VACIO         ", rfc_(9)},
                                                          {"VACIO          ", rfc_(10)},
                                                          {"VACIO           ", rfc_(11)},
                                                          {"VACIO            ", CStr(IIf(rfc_.Length = 13, rfc_(rfc_.Length - 1), ""))}}

        _dimensiones = {10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F, 10.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2 = New Table({600.0F, 400.0F}).UseAllAvailableWidth

        _Nivel2.AddCell(New Cell().Add(New Paragraph("RFC incluyendo la homoclave").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER).
                        SetHorizontalAlignment(HorizontalAlignment.LEFT))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))

        '--------------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CALLE).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CALLE).Valor, ""))},
                                                          {"VACIO ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor, ""))},
                                                          {"VACIO  ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor, ""))}}
        _dimensiones = {500.0F, 250.0F, 250.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Calle"},
                                                          {"VACIO ", "Número y/o letra exterior"},
                                                          {"VACIO  ", "Número y/o letra interior"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))
        ''''
        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_COLONIA).Valor, ""))},
                                                          {"VACIO ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor, ""))},
                                                          {"VACIO  ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_MUNICIPIO).Valor, ""))},
                                                          {"VACIO   ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, ""))}}
        _dimensiones = {250.0F, 100.0F, 250.0F, 400.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Colonia"},
                                                          {"VACIO ", "C.P."},
                                                          {"VACIO  ", "Municipio/Delegación"},
                                                          {"VACIO   ", "Entidad Federativa"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Teléfono"},
                                                          {"VACIO ", " "},
                                                          {"VACIO  ", "Correo Electrónico"},
                                                          {"VACIO   ", " "}}

        _dimensiones = {50.0F, 200.0F, 150.0F, 500.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

#End Region

#Region "D)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "d) Agente o Apoderado aduanal")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        '---

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "GOMEZ"},
                                                          {"VACIO ", "REYES"},
                                                          {"VACIO  ", "JESUS ENRIQUE"}}
        _dimensiones = {400.0F, 300.0F, 300.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "Apellido paterno"},
                                                          {"VACIO ", "Apellido materno"},
                                                          {"VACIO  ", "Nombre"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))

        '--------------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "3"},
                                                          {"VACIO ", "9"},
                                                          {"VACIO  ", "4"},
                                                          {"VACIO   ", "5"}}

        _dimensiones = {5.0F, 5.0F, 5.0F, 5.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2 = New Table({600.0F, 100.0F, 100.0F}).UseAllAvailableWidth

        _Nivel2.AddCell(New Cell().Add(New Paragraph("Número de patente o autorización").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER).
                        SetHorizontalAlignment(HorizontalAlignment.LEFT))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

#End Region

#Region "E)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "e) Datos de la factura(s)")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        For i As Int64 = 0 To 3 Step 1
            '-------------

            _celdas = New Dictionary(Of String, String) From {{"Número de factura", "A004918" & (5 + i)}}
            _dimensiones = {300.0F, 700.0F}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

            _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorder(NO_BORDER).
                                                                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                                                            SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                                                            SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

            '------------------

            Dim fechaFactura_ As DateTime = IIf(documento_.Seccion(SMV4).Attribute(CA_FECHA_FACTURA).Valor IsNot Nothing,
                                                documento_.Seccion(SMV4).Attribute(CA_FECHA_FACTURA).Valor, New DateTime(2023, 3, 10))

            _celdas = New Dictionary(Of String, String) From {{"VACIO", " "},
                                                              {"VACIO ", fechaFactura_.Day.ToString},
                                                              {"VACIO  ", fechaFactura_.Month.ToString},
                                                              {"VACIO   ", fechaFactura_.Year.ToString},
                                                              {"VACIO    ", " "}}

            _dimensiones = {70.0F, 70.0F, 70.0F, 70.0F, 720.0F}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "Fecha:"},
                                                              {"VACIO ", "día"},
                                                              {"VACIO  ", "mes"},
                                                              {"VACIO   ", "año"},
                                                              {"VACIO    ", " "}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER))


        Next

#End Region

#Region "F)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "f) Método de valoración")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        '----------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Señale con una X lo siguiente:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                       SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _dimensiones = {20.0F, 400.0F, 20.0F, 400.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                       SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Se utilizó un método de valoración.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                        SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                        SetPaddings(0F, 0F, 0.0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Se utilizó más un método de valoración.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                        SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                        SetPaddings(0F, 0F, 0.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(5.0F, 5.0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

        '---------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Indique con una X el método de valoración aplicado, en caso de haber utilizado más de uno" &
                                        " indicar el método utilizado para cada mercancía:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                        SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        _dimensiones = {30.0F, 350.0F, 620.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("Método de valoración aplicado").SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).
                                        SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 10.0F, 0.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Descripción de mercancía").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                                        SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                        SetPaddings(0F, 0F, 10.0F, 0.0F).SetBorder(NO_BORDER))
        '--
        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Valor de transacción de las mercancías").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                        SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                        SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("BARRA REDONDA DE ACERO ALEADO, LAMINADA EN CALIENTE").SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).
                                        SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Valor de transacción de las mercancías idénticas").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                        SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                        SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Valor de transacción de las mercancías similares").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                        SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).
                                        SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Valor de precio unitario de venta").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                        SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0F, 10.0F).
                                        SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Valor reconstruido").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                        SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                        SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).
                                            SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                            SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Valor conforme al artículo 78 de la Ley Aduanera").SetTextAlignment(TextAlignment.LEFT).
                                            SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).
                                            SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                            SetPaddings(0F, 0F, 0.0F, 0F).SetBorder(NO_BORDER))
        '--
        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(5.0F, 5.0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "G)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "g) Anexos")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        '------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Señale con una X en caso de presentar anexos, los cuales deberá numerarlos y foliarlos, señalando" &
                            " el número total de hojas anexas con número y letra:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                            SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2 = New Table({100.0F, 900.0F})

        _tablalayout = New Table({20.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(20.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                            SetHorizontalAlignment(HorizontalAlignment.LEFT))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(" ")).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetHeight(25.0F))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        '----------------

        _tablalayout = New Table({250.0F, 750.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Anexa documentación").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Número de hojas anexas con número y letra").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                            SetFontSize(7.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F).
                            SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

#End Region

#End Region

#Region "Valor de transacción de las mercancias"

        _Nivel1.AddCell(New Cell().Add(New Paragraph("VALOR DE TRANSACCIÓN DE LAS MERCANCIAS").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 5.0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).
                                SetPaddings(3.0F, 0F, 3.0F, 0F).SetBorder(NO_BORDER))

#Region "A)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "a) En caso de utilizar el valor de transacción de " &
                                "las mercancías indicar lo siguiente:")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        '-----

        _tablalayout = New Table({250.0F, 750.0F})

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Precio pagado en moneda de facturación, con número y letra").SetTextAlignment(TextAlignment.LEFT).
                                SetFont(_arial).SetFontSize(7.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).
                                SetPaddings(0F, 0F, 0.0F, 0.0F).SetBorder(NO_BORDER))

        '--------------------------

        Dim valor_ = CStr(IIf(documento_.Attribute(CamposFacturaComercial.CA_FLETES).Valor IsNot Nothing, documento_.Attribute(CamposFacturaComercial.CA_FLETES).Valor, "0"))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", GetSafeCharacter(valor_, 15)},
                                                          {"VACIO ", GetSafeCharacter(valor_, 14)},
                                                          {"VACIO  ", GetSafeCharacter(valor_, 13)},
                                                          {"VACIO   ", GetSafeCharacter(valor_, 12)},
                                                          {"VACIO    ", GetSafeCharacter(valor_, 11)},
                                                          {"VACIO     ", GetSafeCharacter(valor_, 10)},
                                                          {"VACIO      ", GetSafeCharacter(valor_, 9)},
                                                          {"VACIO       ", GetSafeCharacter(valor_, 8)},
                                                          {"VACIO        ", GetSafeCharacter(valor_, 7)},
                                                          {"VACIO         ", GetSafeCharacter(valor_, 6)},
                                                          {"VACIO          ", GetSafeCharacter(valor_, 5)},
                                                          {"VACIO           ", GetSafeCharacter(valor_, 4)},
                                                          {"VACIO            ", GetSafeCharacter(valor_, 3)},
                                                          {"VACIO             ", GetSafeCharacter(valor_, 2)},
                                                          {"VACIO              ", GetSafeCharacter(valor_, 1)}}

        _dimensiones = {13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F, 13.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Derecha)

        _tablalayout.SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2 = New Table({200.0F, 800.0F}).UseAllAvailableWidth

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 0F).SetBorder(NO_BORDER).
                        SetHorizontalAlignment(HorizontalAlignment.LEFT))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(NumeroEnPalabras(valor_)).SetTextAlignment(TextAlignment.LEFT).
                        SetFont(_arial).SetFontSize(7.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 0F).SetBorder(NO_BORDER))

#End Region

#Region "B)"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "b) Información conforme al artículo 66 de la" &
                                    " Ley (conceptos que no integran el valor de transacción)")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        '-------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Señale con una X los conceptos que se ajusten a su caso particular").SetTextAlignment(TextAlignment.LEFT).
                                    SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).
                                    SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        _dimensiones = {30.0F, 900.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                    SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F)).
                                    SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Es el precio previsto en la factura").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                    SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).
                                    SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                    SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Es el precios de otros documentos que se anexan a la manifestación").SetTextAlignment(TextAlignment.LEFT).
                                    SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                    SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Si existen conceptos señalados en el artículo 66 de la ley (conceptos que no se integran" &
                                    " al valor transacción)").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                    SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                    SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Los conceptos del artículo 66 de la ley aparecen desglosados o especificados en la factura comercial").
                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "C)"

        _dimensiones = {40.0F, 10.0F}

        _tablalayout = New Table(_dimensiones)

        _Nivel2 = New Table({50.0F, 950.0F})

        _celdas = Nothing

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(25.0F).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _dimensiones = {950.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "c) Indicar con la X en caso de ANEXAR documentación a la" &
                        " manifestación de valor")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("Nota: Sólo se relacionarán los documentos que anexen, correspondientes a los conceptos previstos en el" &
                        " artículo 66 de la Ley.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                        SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                        SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        '--------------------------

        _dimensiones = {150.0F, 850.0F}

        _celdas = New Dictionary(Of String, String) From {{"Numerar anexos y relacionarlos: ", "VACIO"},
                                                          {"Conceptos previstos en el artículo 66 de la Ley. Anote cada factura o documneto comercial que " &
                                                          "anexa de acuerdo al número asignado: ", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayoutFija(_celdas, _dimensiones, TiposBordes.DerechaAbajo)

        _tablalayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                            SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                            SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

#End Region

#Region "D)"

        _dimensiones = {40.0F, 10.0F}

        _tablalayout = New Table(_dimensiones)

        _Nivel2 = New Table({50.0F, 950.0F})

        _celdas = Nothing

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(25.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _dimensiones = {950.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "d)Indicar con una X en el caso de NO ANEXAR documentación" &
                            " y sólo describirán los conceptos previstos en el artículo 66 de la Ley.")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Describa la mercancía, los conceptos señalados en el artículo 66 de la Ley y el precio pagado respecto de" &
                            " cada uno, es decir, los conceptos que no integran el valor de transacción. Sólo cuando estos no aparezcan desglosados o " &
                            "especificados en la factura o documentación comercial.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                            SetFontSize(7.0F).SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                            SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        '--------------------------

        _dimensiones = {80.0F, 230.0F, 230.0F, 230.0F, 230.0F}

        _celdas = New Dictionary(Of String, String) From {{"No.", "VACIO"},
                                                          {"Mercancía", "VACIO"},
                                                          {"Factura o documentos comerciales", "VACIO"},
                                                          {"Importe y moneda de facturación", "VACIO"},
                                                          {"Concepto del cargo", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayoutFija(_celdas, _dimensiones, TiposBordes.DerechaAbajo, "Conceptos previstos en el artículo 66 de la Ley")

        _tablalayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))


        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))


        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))
        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))



        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                            SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("NOTA: Puede optar por no rellenar en rubro de ´conceptos del cargo´ si estos aparecen desglosados o" &
                            " especificados en la factura, en caso de que no aparezcan desglosados deben ser descritos.").
                            SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                            SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                            SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

#End Region

#Region "Información Art 65"

        _dimensiones = {1000.0F}

        _celdas = Nothing

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "Información conforme al artículo 65 de la Ley (conceptos que" &
                                                                                                  " integran el valor de transacción).")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        '------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("El importador debe señalar si existen cargos conforme al artículo 65 de la Ley Aduanera. Señale con una X si" &
                                                     " el precio pagado por las mercancías importadas comprende el importe de los conceptos señalados en el artículo" &
                                                     " 65 de la Ley.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                                                     SetMargins(10.0F, 0F, 0.0F, 10.0F).SetPaddings(0F, 0F, 5.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout = New Table({20.0F, 300.0F, 20.0F, 300.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("En su caso, señale con una X si el importador opta por acompañar o NO las facturas y otros documentos a su" &
                            " manifestación de valor.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                            SetMargins(5.0F, 0F, 0F, 10.0F).SetPaddings(0F, 0F, 5.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout = New Table({20.0F, 300.0F, 20.0F, 300.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

        '------------------------

        _dimensiones = {40.0F, 10.0F}

        _tablalayout = New Table(_dimensiones)

        _Nivel2 = New Table({50.0F, 950.0F})

        _celdas = Nothing

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(25.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _dimensiones = {950.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "Indicar con una X si ANEXA documentación")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("En caso de anexar documentación, señale lo siguiente:").SetTextAlignment(TextAlignment.LEFT).
                            SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        '--------------------------

        _dimensiones = {150.0F, 850.0F}

        _celdas = New Dictionary(Of String, String) From {{"Numerar anexos y relacionarlos", "VACIO"},
                                                          {"Conceptos previstos en el artículo 65 de la Ley. Anote cada factura o documento comercial que" &
                                                          " anexa de acuerdo al número asignado", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayoutFija(_celdas, _dimensiones, TiposBordes.DerechaAbajo)

        _tablalayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                            SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("  ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                            SetMargins(10.0F, 0F, 10.0F, 10.0F).SetPaddings(0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        '--------------------------

        _dimensiones = {40.0F, 10.0F}

        _tablalayout = New Table(_dimensiones)

        _Nivel2 = New Table({50.0F, 950.0F})

        _celdas = Nothing

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(25.0F).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _dimensiones = {950.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno, "Indicar con una X si NO ANEXA documentación")

        _tablalayout.SetBorder(NO_BORDER)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 2.0F, 3.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("En caso de NO anexar documentación, deberá señalar el importe de cada uno de ellos e indicará el número que" &
                            " asigne a cada uno de los anexos a que se refiere este párrafo, relacionando el número del anexo (s) en que conste los cargos de" &
                            " referencia, con la mercancía (s) a cuyo precio pagado deben incrementarse los cargos multicitados.").
                            SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 10.0F, 10.0F).
                            SetPaddings(5.0F, 0F, 10.0F, 10.0F).SetBorder(NO_BORDER))

        '--------------------------

        _dimensiones = {80.0F, 230.0F, 230.0F, 230.0F, 230.0F}

        _celdas = New Dictionary(Of String, String) From {{"No.", "VACIO"},
                                                          {"Mercancía o proveedor", "VACIO"},
                                                          {"Factura o documento", "VACIO"},
                                                          {"Importe y moneda de facturación", "VACIO"},
                                                          {"Concepto del cargo", "VACIO"}}

        _tablalayout = _controladorPDF.setTablaLayoutFija(_celdas, _dimensiones, TiposBordes.DerechaAbajo)

        _tablalayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))


        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))


        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                            SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetHeight(10.0F).SetMarginTop(0).SetMarginBottom(0).
                                            SetPaddings(0, 0, 0, 5).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBackgroundColor(ColorConstants.WHITE))



        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))


#End Region

#End Region

#Region "Valor de transacción de las mercancias"

        _Nivel1.AddCell(New Cell().Add(New Paragraph("PERIODICIDAD DE LA MANIFESTACIÓN:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(7.0F).
                                       SetMargins(0F, 0F, 0F, 5.0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(3.0F, 0F, 3.0F, 0F).
                                       SetBorder(NO_BORDER))

        '------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Señale con una X si el importador presenta la manifestación de valor por operación o por el periodo de seis meses.").
                                       SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetMargins(10.0F, 0F, 0.0F, 0.0F).
                                       SetPaddings(0F, 0F, 5.0F, 10.0F).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                        SetMargins(10.0F, 0F, 0.0F, 0.0F).SetPaddings(0F, 0F, 5.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout = New Table({20.0F, 300.0F, 20.0F, 300.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Por operación").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetHeight(15.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).
                                            SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F).SetHeight(15.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 5.0F, 0.0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("Por periodo de seis meses").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                            SetHeight(15.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).
                                            SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph("Bajo protesta de decir verdad, manifiesto que los datos asentados en el presente documento son ciertos.").
                                       SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).
                                       SetMargins(10.0F, 0F, 0.0F, 0.0F).SetPaddings(0F, 0F, 5.0F, 10.0F).SetBorder(NO_BORDER))

        '--------------------

        _dimensiones = {50.0F, 100.0F, 850.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("RFC: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).
                                            SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("PEHL700906QD4").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))).
                                            SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--------------------

        _dimensiones = {100.0F, 100.0F, 100.0F}

        _Nivel2 = New Table({50.0F, 950.0F})

        _Nivel3 = New Table({300.0F, 900.0F})

        _celdas = New Dictionary(Of String, String) From {{"VACIO ", DateTime.Now.Day},
                                                          {"VACIO  ", DateTime.Now.Month},
                                                          {"VACIO   ", DateTime.Now.Year}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _tablalayout.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))

        _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(New Paragraph("Fecha:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel3 = New Table({300.0F, 900.0F})

        _celdas = New Dictionary(Of String, String) From {{"VACIO ", "día"},
                                                          {"VACIO  ", "mes"},
                                                          {"VACIO   ", "año"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER))

        '--------------------

        _dimensiones = {500.0F}

        _Nivel2 = New Table({250.0F, 500.0F, 250.0F})

        _tablalayout = New Table(_dimensiones).UseAllAvailableWidth

        _tablalayout.AddCell(New Cell().Add(New Paragraph("LORENZO MAURICIO PEREZ HERNANDEZ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                SetFontSize(7.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5))).
                                SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NOMBRE Y FIRMA DEL IMPORTADOR O DE SU REPRESENTANTE LEGAL").SetTextAlignment(TextAlignment.CENTER).
                                SetFont(_arial).SetFontSize(7.0F).SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).
                                SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE)).SetMargins(5.0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0.0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0.0F).SetPaddings(0F, 0F, 0F, 0.0F).SetBorder(NO_BORDER).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE).SetHorizontalAlignment(HorizontalAlignment.CENTER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 10.0F).SetPaddings(0F, 5.0F, 5.0F, 20.0F).SetBorder(NO_BORDER).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE).SetHorizontalAlignment(HorizontalAlignment.CENTER))

#End Region

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0

        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString

    End Function

    Public Function ImprimirEncabezadoHC(ByVal documento_ As DocumentoElectronico) As String



        '====================================ENCABEZADO==============================================

#Region "Encabezado"

        _Nivel1 = New Table({1000.0F}).UseAllAvailableWidth

        _Nivel1.SetBorder(NO_BORDER)

        _dimensiones = {200.0F, 600.0F, 200.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.SetBorder(NO_BORDER)

        _image = New Image(ImageDataFactory.Create("C:\TEMP\LogoSHCP.png"))

        _image.SetWidth(70)

        _tablalayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ")).Add(_image).SetHorizontalAlignment(HorizontalAlignment.CENTER).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("Hoja de cálculo para la determinación del valor en aduana de mercancía de importación").
                            SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(10.0F)).SetVerticalAlignment(VerticalAlignment.TOP).
                            SetBorder(NO_BORDER)).SetBorder(NO_BORDER)

        _image = New Image(ImageDataFactory.Create("C:\TEMP\LogoSAT.png"))

        _image.SetWidth(80)

        _tablalayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph("")).Add(_image).SetHorizontalAlignment(HorizontalAlignment.CENTER).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph("SAP23-3675").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(0.8).
                            SetFont(_arial).SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _Nivel1.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

#End Region

#Region "1"

        _Nivel2 = New Table({25.0F, 980.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("1").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("DATOS DEL IMPORTADOR").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------

        _Nivel2 = New Table({950.0F})

        _celdas = New Dictionary(Of String, String) From {{"VACIO", ""},
                                                          {"VACIO ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RFC_CLIENTE).Valor, ""))}}

        _dimensiones = {725.0F, 225.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "APELLIDO PATERNO, MATERNO, NOMBRE(S),"},
                                                          {"VACIO ", "RFC"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        '-----------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor, ""))}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO ", "DENOMINACION O RAZON SOCIAL"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        '-----------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CALLE).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CALLE).Valor, ""))},
                                                          {"VACIO ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor, "")) &
                                                                              CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing,
                                                                              "/" & documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor, ""))},
                                                          {"VACIO  ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CODIGO_POSTAL).Valor, ""))},
                                                          {"VACIO   ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, ""))}}
        _dimensiones = {300.0F, 150.0F, 250.0F, 350.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "DOMICILIO CALLE"},
                                                          {"VACIO ", "No. EXTERIOR/INTERIOR"},
                                                          {"VACIO  ", "CODIGO POSTAL"},
                                                          {"VACIO   ", "ENTIDAD O MUNICIPIO"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "2"

        _Nivel2 = New Table({25.0F, 975.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("2").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("DATOS DEL VENDEDOR").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                            SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '--------------------

        _Nivel2 = New Table({950.0F})

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Attribute(CA_RAZON_SOCIAL_PROVEEDOR).Valor IsNot Nothing,
                                                                             documento_.Attribute(CA_RAZON_SOCIAL_PROVEEDOR).Valor, ""))},
                                                          {"VACIO ", CStr(IIf(documento_.Attribute(CA_TAX_ID_PROVEEDOR).Valor IsNot Nothing,
                                                                             documento_.Attribute(CA_TAX_ID_PROVEEDOR).Valor, ""))}}
        _dimensiones = {725.0F, 225.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "APELLIDO PATERNO, MATERNO, NOMBRE(S),"},
                                                          {"VACIO ", "TAX NUMBER"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        '-----------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO ", "DENOMINACION O RAZON SOCIAL"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        '-----------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CALLE).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CALLE).Valor, ""))},
                                                          {"VACIO ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor, "")) & "/" &
                                                                             CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_NUMERO_INTERIOR).Valor, ""))},
                                                          {"VACIO  ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CIUDAD).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_CIUDAD).Valor, ""))},
                                                          {"VACIO   ", CStr(IIf(documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_PAIS).Valor IsNot Nothing,
                                                                             documento_.Seccion(SMV3).Attribute(CamposDomicilio.CA_PAIS).Valor, ""))}}

        _dimensiones = {300.0F, 150.0F, 250.0F, 350.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "DOMICILIO CALLE"},
                                                          {"VACIO ", "No. EXTERIOR/INTERIOR"},
                                                          {"VACIO  ", "CIUDAD"},
                                                          {"VACIO   ", "PAÍS"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))


        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "3"

        _Nivel2 = New Table({25.0F, 975.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("3").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("DATOS DE LA MERCANCÍA").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arialBold).
                                SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).
                                SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------

        _Nivel2 = New Table({950.0F})

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "DISIPADORES DE CALOR"}}

        _dimensiones = {950.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "DESCRIPCION"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        '-----------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "7616.99.99"},
                                                          {"VACIO ", "35800.00000"}}

        _dimensiones = {475.0F, 475.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "CLASIFICACIÓN ARANCELARIA"},
                                                          {"VACIO ", "CANTIDAD"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        '-----------------------

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "PHL"},
                                                          {"VACIO ", "PHL"}}

        _dimensiones = {475.0F, 475.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "PAÍS DE PRODUCCIÓN"},
                                                          {"VACIO ", "PAÍS DE PROCEDENCIA"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '------------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "4"

        _Nivel2 = New Table({25.0F, 975.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("4").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("DETERMINACIÓN DEL MÉTODO").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arialBold).
                        SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------

        _Nivel2 = New Table({950.0F})

        _dimensiones = {600.0F, 175.0F, 175.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("1.- ¿ES COMPRAVENTA PARA IMPORTACIÓN A TERRITORIO NACIONAL?").SetTextAlignment(TextAlignment.LEFT).
                            SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).
                            SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)
        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI ( X )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO (    )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                            SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        '--
        _tablalayout.AddCell(New Cell().Add(New Paragraph("2.- ÚNICAMENTE PERSONAS VINCULADAS. ¿LA VINCULACIÓN AFECTA AL PRECIO?").SetTextAlignment(TextAlignment.LEFT).
                            SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).
                            SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI (    )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO ( X )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        '--
        _tablalayout.AddCell(New Cell().Add(New Paragraph("3.- ¿EXISTEN RESTRICCIONES?").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI (    )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO ( X )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        '--
        _tablalayout.AddCell(New Cell().Add(New Paragraph("4.- ¿EXISTEN CONTRAPRESTACIONES?").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI (    )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO ( X )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                            SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        '--
        _tablalayout.AddCell(New Cell().Add(New Paragraph("5.- ¿EXISTEN REGALIAS O REVERSIONES?").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                            SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("SI (    )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                            SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("NO ( X )").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                            SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

        '--

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("EN CASO DE HABER CONTESTADO NEGATIVAMENTE AL SUPUESTO NUMERO 1 O AFIRMATIVAMENTE EN CUALQUIERA DE LOS DEMAS " &
                            "SUPUESTOS, NO PODRÁ UTILIZAR EL MÉTODO DE VALOR DE TRANSACCIÓN, UTILICE OTRO MÉTODO").SetTextAlignment(TextAlignment.LEFT).
                            SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 7.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '------------------------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "5, 6, 7"

        _Nivel2 = New Table({333.0F, 333.0F, 334.0F})

        _tablalayout = New Table({25.0F, 308.0F})

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("5").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("PRECIO PAGADO O POR PAGAR").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                            SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout = New Table({25.0F, 308.0F})

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("6").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("AJUSTES INCREMENTABLES").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                            SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout = New Table({25.0F, 309.0F})

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("7").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("NO INCREMENTABLES").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                            SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------------

        _tablalayout = New Table({140.0F, 180.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("PAGOS DIRECTOS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("165761 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("CONTRAPRESTACIONES O PAGOS INDIRECTOS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                            SetFont(_arial).SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).
                            SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.BOTTOM).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("TOTAL %: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F).SetHeight(70.0F).SetVerticalAlignment(VerticalAlignment.BOTTOM)).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("165761 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F).SetHeight(70.0F).SetVerticalAlignment(VerticalAlignment.BOTTOM)).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 0.0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetVerticalAlignment(VerticalAlignment.TOP).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 5.0F, 0.0F))

        _tablalayout = New Table({150.0F, 170.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("COMISIONES: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("FLETES Y SEGUROS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("5416 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("CARGA Y DESCARGA: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("7587 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("MATERIALES APORTADOS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.BOTTOM).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("TECNOLOGÍA APORTADA: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.BOTTOM).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("REGALÍAS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("REVERSIONES: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).
                            SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("TOTAL %: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F).SetHeight(20.0F).SetVerticalAlignment(VerticalAlignment.BOTTOM)).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("13003 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F).SetHeight(20.0F).SetVerticalAlignment(VerticalAlignment.BOTTOM)).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetVerticalAlignment(VerticalAlignment.TOP).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)).
                            SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 5.0F, 0F))

        _tablalayout = New Table({150.0F, 180.0F})

        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("GASTOS DIVERSOS QUE SE REALICEN CON POSTERIORIDAD EN LOS SUPUESTOS A QUE SE REFIERE" &
                            " LA FRACCION I DEL ART 56 DE LA LEY").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("GASTOS NO RELACIONADOS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.BOTTOM).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("FLETES Y SEGUROS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("GASTOS DE CONSTRUCCIÓN: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.BOTTOM).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("INST. ARMADO, ETC: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("CONTRIBUCIONES: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("DIVIDENDOS: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F)).SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("TOTAL %: ").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("0 ").SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetVerticalAlignment(VerticalAlignment.TOP).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 0.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetVerticalAlignment(VerticalAlignment.TOP).SetBorder(NO_BORDER)).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(5.0F, 5.0F, 5.0F, 0F)

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "8"

        _Nivel2 = New Table({25.0F, 975.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("8").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("VALOR EN ADUANA CONFORME AL MÉTODO DE VALOR DE TRANSACCIÓN").SetTextAlignment(TextAlignment.LEFT).
                                SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F)).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------
        _Nivel2 = New Table({950.0F})

        _dimensiones = {230.0F, 85.0F, 230.0F, 85.0F, 230.0F, 85.0F}

        _tablalayout = New Table(_dimensiones)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("PRECIO PAGADO O POR PAGAR:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                                SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("165761").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("AJUSTES INCREMENTABLES:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                                SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("13003").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("VALOR EN ADUANA %:").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).
                                SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("178764").SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                SetPaddings(5.0F, 0F, 5.0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                                SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

#End Region

#Region "9"

        Dim facturas_ As List(Of String) = New List(Of String)() From {"CI01758", "CI01761"}

        For Each factura_ As String In facturas_

            _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 25.0F).SetBorder(NO_BORDER))

            _Nivel2 = New Table({25.0F, 975.0F})

            _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("9").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

            _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("LA PRESENTE DETERMINACIÓN DEL VALOR ES VALIDA PARA").SetTextAlignment(TextAlignment.LEFT).
                            SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F)).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

            _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

            '-----------------------

            _Nivel2 = New Table({200.0F, 200.0F, 200.0F, 170.0F, 230.0F})

            _dimensiones = {200.0F}

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "PEDIMENTO NÚMERO"},
                                                              {"VACIO ", "23 16 3931 3002702"}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

            '---------

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "FECHA DEL PEDIMENTO AA/MM/DD"},
                                                              {"VACIO ", "23/04/28"}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel2.AddCell(New Cell(rowspan:=2, colspan:=0).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

            '---------

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "FACTURA NUMERO"},
                                                              {"VACIO ", factura_}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

            '------

            _dimensiones = {170.0F}

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "FECHA DE LA FACTURA AAMMDD"},
                                                              {"VACIO ", "230309"}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))


            '---------

            _dimensiones = {230.0F}

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "MARQUE CON UNA X SI CUENTA CON MAS DE UN PEDIMENTO"}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel3 = New Table({20.0F})

            _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph(" ")).SetHeight(15.0F).SetWidth(15.0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

            _tablalayout.AddCell(New Cell().Add(_Nivel3).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetMargins(0F, 0F, 0F, 0F).
                            SetPaddings(5.0F, 0F, 5.0F, 60.0F).SetBorder(NO_BORDER))

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER).
                            SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

            '---------

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "LUGAR DE EMISIÓN DE LA FACTURA"},
                                                              {"VACIO ", "CALAMBIA, LAGUNA, FILIPINAS (REPUBLICA DE LAS FILIPINAS)"}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

            '---------

            _dimensiones = {400.0F}

            _celdas = New Dictionary(Of String, String) From {{"VACIO", "TIPO DE FACTURA"}}

            _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

            _Nivel3 = New Table({150.0F, 30.0F, 150.0F, 30.0F})

            _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("DOCUMENTO ÚNICO").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER))

            _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("X").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                            SetHeight(15.0F).SetWidth(15.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)).
                            SetVerticalAlignment(VerticalAlignment.MIDDLE))

            _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("SUBDIVISIONES").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                            SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                            SetBorder(NO_BORDER))

            _Nivel3.AddHeaderCell(New Cell().Add(New Paragraph("X")).SetHeight(15.0F).SetWidth(15.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                            SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

            _tablalayout.AddCell(New Cell().Add(_Nivel3).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 0F).
                            SetBorder(NO_BORDER))

            _Nivel2.AddCell(New Cell(rowspan:=0, colspan:=2).Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

            '---------

            _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        Next
#End Region

        _Nivel1.AddCell(New Cell().Add(New Paragraph("MÉTODOS DIFERENTES AL VALOR DE LA TRANSACCIÓN").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                        SetFontSize(8.0F).SetMultipliedLeading(1)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).
                        SetPaddings(5.0F, 0F, 5.0F, 25.0F).SetBorder(NO_BORDER))

#Region "10"

        _Nivel2 = New Table({25.0F, 975.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("10").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE)).
                        SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("VALOR EN ADUANA DETERMINADO SEGÚN OTROS MÉTODOS %").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "11, 12"

        _Nivel2 = New Table({500.0F, 500.0F})

        _tablalayout = New Table({25.0F, 475.0F})

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("11").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).
                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("NO UTILIZÓ EL VALOR DE TRANSACCIÓN POR").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout = New Table({25.0F, 475.0F})

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("12").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).
                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout.AddHeaderCell(New Cell().Add(New Paragraph("MÉTODO PARA LA DETERMINACIÓN DEL VALOR EN ADUANA").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout = New Table({450.0F, 50.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("1.- NO SE TRATO DE UNA COMPRAVENTA.").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                        SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("2.- LA COMPRAVENTA NO FUE PARA EXPORTACIÓN CON DESTINO A TERRITORIO NACIONAL.").
                        SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                        SetPaddings(0F, 0F, 0F, 5.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("3.- EXISTIR VINCULACIÓN QUE AFECTA EL PRECIO.").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("4.- EXISTIR RESTRICCIONES A LA ENAJENACION O UTILIZACIÓN DISTINTOS DE LOS PERMITIDOS.").
                        SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                        SetPaddings(0F, 0F, 0F, 5.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("5.- EXISTIR CONTRAPRESTACIONES O REVERSIONES NO CUANTIFICABLES.").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '----

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _tablalayout = New Table({450.0F, 50.0F})

        _tablalayout.AddCell(New Cell().Add(New Paragraph("1. VALOR DE TRANSACCIÓN DE MERCANCIAS IDÉNTICAS.").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '-----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("2. VALOR DE TRANSACCIÓN DE MERCANCIAS SIMILARES.").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '-----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("3. VALOR DE PRECIO UNITARIO DE VENTA.").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).
                        SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                        SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '-----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("4. VALOR RECONSTRUIDO.").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).
                                            SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).
                                            SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '-----
        _tablalayout.AddCell(New Cell().Add(New Paragraph("5. VALOR DETERMINADO CONFORME AL ARTÍCULO 78 DE LA LEY.").SetTextAlignment(TextAlignment.LEFT).
                        SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 5.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("|  |").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 10.0F).SetBorder(NO_BORDER))
        '-----
        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

#Region "13"

        _Nivel2 = New Table({25.0F, 975.0F})

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("13").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER).
                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 1.5)))

        _Nivel2.AddHeaderCell(New Cell().Add(New Paragraph("EL SUSCRITO MANIFIESTA BAJO PROTESTA DE DECIR VERDAD QUE LO ASENTADO EN ESTA DECLARACIÓN ES VERÍDICO").
                        SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                        SetPaddings(0F, 0F, 0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '-----------------------

        _Nivel2 = New Table({1000.0F})

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "SALGADO ARENAS JESUS ALBERTO"}}
        _dimensiones = {725.0F, 225.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 5.0F, 0F, 5.0F).SetBorder(NO_BORDER))

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "APELLIDO PATERNO, MATERNO Y NOMBRE(S) DEL REPRESENTANTE LEGAL"}}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel2.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))
        '--
        _Nivel3 = New Table({400.0F, 300.0F, 300.0F})

        _tablalayout = New Table({350})

        _tablalayout.AddCell(New Cell().Add(New Paragraph(" ")).SetHeight(50.0F).SetWidth(300.0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 0.5)))

        _tablalayout.AddCell(New Cell().Add(New Paragraph("FIRMA DEL IMPORTADOR O REPRESENTANTE LEGAL").SetTextAlignment(TextAlignment.CENTER).
                        SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(10.0F, 0F, 10.0F, 10.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 5.0F).SetBorder(NO_BORDER))
        '--
        _celdas = New Dictionary(Of String, String) From {{"VACIO", "FECHA DE ELABORACION AA/MM/DD"},
                                                          {"VACIO ", "23/04/28"}}

        _dimensiones = {250.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Ninguno)

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 5.0F).SetBorder(NO_BORDER).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE))
        '--
        _celdas = New Dictionary(Of String, String) From {{"VACIO", "SAAJ820703H27"}}

        _dimensiones = {250.0F}

        _tablalayout = _controladorPDF.setTablaLayout(_celdas, _dimensiones, TiposBordes.Abajo)

        _tablalayout.AddCell(New Cell().Add(New Paragraph("RFC").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0.0F, 0F, 10.0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _Nivel3.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 2.0F, 5.0F).SetBorder(NO_BORDER).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE))
        '--

        _Nivel2.AddCell(New Cell().Add(_Nivel3).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 5.0F, 2.0F, 5.0F).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '--------------------------

        _Nivel2 = New Table({850.0F, 150.0F})

        _Nivel2.AddCell(New Cell().Add(New Paragraph("14 SE ASENTARÁ EL NÚMERO DE PATENTE O AUTORIZACIÓN DEL AGENTE O APODERADO ADUANAL QUE REALIZARÁ EL DESPACHO " &
                        "DE LAS MERCANCIAS").SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(5.0F, 0F, 5.0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _Nivel2.AddCell(New Cell().Add(New Paragraph("3931").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1).SetFont(_arial).SetFontSize(7.0F).
                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(10.0F, 0F, 0F, 10.0F)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(NO_BORDER))

        _Nivel1.AddCell(New Cell().Add(_Nivel2).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(New SolidBorder(ColorConstants.BLACK, 1.5)))

        '--------------------------

        _Nivel1.AddCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).
                        SetVerticalAlignment(VerticalAlignment.MIDDLE).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 10.0F, 20.0F).SetBorder(NO_BORDER))

#End Region

        _doc.Add(_Nivel1)

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray

        Dim ms2 As MemoryStream = New MemoryStream()

        ms2.Write(bytesStream_, 0, bytesStream_.Length)

        ms2.Position = 0

        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())

        Return inputAsString

    End Function

    Private Function GetSafeCharacter(input As String, index As Integer) As Char
        Try
            Return input(input.Length - index)
        Catch ex As IndexOutOfRangeException
            Return " "
        End Try
    End Function

    Private Function NumeroEnPalabras(numero As Double) As String
        Dim unidades() As String = {"Cero", "Uno", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve"}
        Dim especiales() As String = {"Diez", "Once", "Doce", "Trece", "Catorce", "Quince", "Dieciséis", "Diecisiete", "Dieciocho", "Diecinueve"}
        Dim decenas() As String = {"", "", "Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesenta", "Setenta", "Ochenta", "Noventa"}
        Dim centenas() As String = {"", "Ciento", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos"}

        If numero < 10 Then
            Return unidades(numero)
        ElseIf numero < 20 Then
            Return especiales(numero - 10)
        ElseIf numero < 100 Then
            Dim decena As Integer = numero \ 10
            Dim unidad As Integer = numero Mod 10
            If unidad = 0 Then
                Return decenas(decena)
            Else
                Return decenas(decena) & " y " & unidades(unidad)
            End If
        ElseIf numero < 1000 Then
            Dim centena As Integer = numero \ 100
            Dim restoCentena As Integer = numero Mod 100
            If restoCentena = 0 Then
                Return centenas(centena)
            Else
                Return centenas(centena) & " " & NumeroEnPalabras(restoCentena)
            End If
        ElseIf numero < 1000000 Then
            Dim miles As Integer = numero \ 1000
            Dim restoMiles As Integer = numero Mod 1000
            If restoMiles = 0 Then
                Return NumeroEnPalabras(miles) & " Mil"
            Else
                Return NumeroEnPalabras(miles) & " Mil " & NumeroEnPalabras(restoMiles)
            End If
        Else
            ' Aquí puedes continuar con la lógica para números mayores
            ' que un millón si es necesario.
            Return "Número fuera de rango"
        End If
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
