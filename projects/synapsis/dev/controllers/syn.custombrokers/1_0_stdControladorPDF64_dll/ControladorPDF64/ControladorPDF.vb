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
Imports iText.IO.Font
Imports iText.Commons.Actions
Imports iText.Layout.Borders
Imports iText.IO.Image


Public Class ControladorPDF

    Private _Nivel1 As Table

    Private _tablalayout As Table

    Private _layout As Table

    Private _arialBold As PdfFont

    Private _arial As PdfFont

    Private _image As Image

    Private _watermark As Image

    Private _estiloSinBordes As Style

    Enum TiposBordes
        Niniguno = 0
        Derecha = 1
        DerechaAbajo = 2
        Abajo = 3
    End Enum

    Public Sub New(arialBold_ As PdfFont, arial_ As PdfFont, watermark_ As Image, estiloSinBordes_ As Style)

        _arial = arial_

        _arialBold = arialBold_

        _watermark = watermark_

        _estiloSinBordes = estiloSinBordes_

    End Sub

    Function FolioDoc(folioOperacion_ As String) As Table

        _Nivel1 = New Table({700.0F, 300.0F}).UseAllAvailableWidth

        Return _Nivel1.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(folioOperacion_).SetTextAlignment(TextAlignment.RIGHT).SetFont(_arial).
                                                                          SetFontSize(7.0F)).SetBorder(Border.NO_BORDER))

    End Function

    Function setTablaLayout(_layout As Dictionary(Of String, String),
                            dimensiones_ As Single(), ByVal isBorder As TiposBordes,
                            Optional ByVal _header As String = Nothing,
                            Optional ByVal isBackground As Boolean = True) As Table

        Dim unir As Boolean = False

        _tablalayout = New Table(dimensiones_).UseAllAvailableWidth

        _tablalayout.SetPaddings(0F, 0F, 0F, 0F)

        _tablalayout.SetTextAlignment(TextAlignment.CENTER)

        _tablalayout.SetMargins(0F, 0F, 0F, 0F)

        _tablalayout.SetBorder(NO_BORDER)

        If _header IsNot Nothing Then

            If _layout IsNot Nothing Then

                If isBackground Then

                    If isBorder <> 0 Then

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).
                                        SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                Else

                    If isBorder <> 0 Then

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                End If

            Else

                _tablalayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

                Select Case isBorder

                    Case TiposBordes.DerechaAbajo

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case TiposBordes.Derecha

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case TiposBordes.Abajo

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case Else
                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                End Select

            End If

        End If

        If _layout IsNot Nothing Then

            For Each celda As KeyValuePair(Of String, String) In _layout

                If Trim(celda.Key) = "VACIO" And celda.Value = "VACIO" Then

                    unir = True

                Else

                    If Trim(celda.Key) <> "VACIO" Then

                        If celda.Value <> "VACIO" Then

                            Select Case isBorder

                                Case TiposBordes.Derecha

                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case TiposBordes.DerechaAbajo

                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case TiposBordes.Abajo

                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case Else

                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                            End Select

                        Else

                            If unir Then

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Abajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0, 0, 0, 5).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0, 0, 0, 5))

                                End Select

                            Else

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Abajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                                End Select

                            End If


                        End If

                    End If

                    If unir = False Then

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Abajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

                                End Select

                            Else

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Abajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                                End Select

                            End If

                        End If

                    Else

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Abajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F))

                                End Select

                            Else

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Abajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                                End Select

                            End If

                        End If

                        unir = False

                    End If

                End If

            Next

        End If

        Return _tablalayout

    End Function

    Function setTablaLayout(_layout As Dictionary(Of String, Dictionary(Of String, List(Of Int64))),
                            dimensiones_ As Single(), ByVal isBorder As TiposBordes,
                            Optional ByVal _header As String = Nothing,
                            Optional ByVal isBackground As Boolean = True) As Table

        _tablalayout = New Table(dimensiones_).UseAllAvailableWidth

        _tablalayout.SetPaddings(0F, 0F, 0F, 0F)

        _tablalayout.SetTextAlignment(TextAlignment.CENTER)

        _tablalayout.SetMargins(0F, 0F, 0F, 0F)

        If _header IsNot Nothing Then

            If isBackground Then

                If isBorder <> 0 Then

                    _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                Else

                    _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                End If

            Else

                If isBorder <> 0 Then

                    _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                Else

                    _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                End If

            End If

        End If

        For Each celda As KeyValuePair(Of String, Dictionary(Of String, List(Of Int64))) In _layout

            If Trim(celda.Key) <> "VACIO" Then

                If celda.Value.Single.Key <> "VACIO" Then

                    If isBorder <> 0 Then

                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).
                                    SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    Else
                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).
                                    SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                    AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                    End If

                Else

                    If isBorder <> 0 Then

                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).
                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                    SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    Else

                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).
                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                    SetPaddings(0, 0, 0, 5))

                    End If

                End If

            End If

            If celda.Value.Single.Key <> "VACIO" Then

                If Trim(celda.Key) <> "VACIO" Then

                    Select Case isBorder

                        Case TiposBordes.DerechaAbajo

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case TiposBordes.Derecha

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case Else

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

                    End Select

                Else

                    Select Case isBorder

                        Case TiposBordes.DerechaAbajo

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case TiposBordes.Derecha

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case Else

                            _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                    End Select

                End If

            End If

        Next

        Return _tablalayout

    End Function

    Function setTablaLayoutBorder(_layout As Dictionary(Of String, String),
                                  dimensiones_ As Single(),
                                  Optional ByVal _header As String = Nothing,
                                  Optional ByVal isBackground As Boolean = True) As Table

        Dim unir_ As Boolean = False

        _tablalayout = New Table(dimensiones_)

        _tablalayout.SetPadding(0F)

        If _header IsNot Nothing Then

            If isBackground Then

                _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                            SetBackgroundColor(ColorConstants.LIGHT_GRAY))

            Else

                _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

            End If

        End If

        For Each celda As KeyValuePair(Of String, String) In _layout

            If Trim(celda.Key) <> "IMG" Then

                _tablalayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).Add(New Paragraph(celda.Value).
                            SetMultipliedLeading(1)).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER))

            Else

                _image = New Image(ImageDataFactory.Create(celda.Value))

                _image.SetWidth(100)

                _tablalayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ")).Add(_image)).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

            End If

        Next

        Return _tablalayout

    End Function

    Function setTablaLayoutFija(_layout As Dictionary(Of String, String),
                                dimensiones_ As Single(),
                                ByVal isBorder As TiposBordes,
                                Optional ByVal _header As String = Nothing,
                                Optional ByVal isBackground As Boolean = True) As Table

        Dim unir_ As Boolean = False

        _tablalayout = New Table(dimensiones_)

        _tablalayout.SetPaddings(0F, 0F, 0F, 0F)

        _tablalayout.SetTextAlignment(TextAlignment.CENTER)

        _tablalayout.SetMargins(0F, 0F, 0F, 0F)

        _tablalayout.SetBorder(NO_BORDER)

        If _header IsNot Nothing Then

            If _layout IsNot Nothing Then

                If isBackground Then

                    If isBorder <> 0 Then

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                Else

                    If isBorder <> 0 Then

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                End If

            Else

                _tablalayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

                Select Case isBorder

                    Case TiposBordes.DerechaAbajo

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case TiposBordes.Derecha

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).
                                        SetPaddings(0F, 0F, 0F, 0F))

                    Case Else

                        _tablalayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(_header).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                End Select

            End If

        End If

        If _layout IsNot Nothing Then

            For Each celda As KeyValuePair(Of String, String) In _layout

                If Trim(celda.Key) = "VACIO" And celda.Value = "VACIO" Then

                    unir_ = True

                Else

                    If Trim(celda.Key) <> "VACIO" Then

                        If celda.Value <> "VACIO" Then

                            Select Case isBorder

                                Case TiposBordes.Derecha
                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case TiposBordes.DerechaAbajo

                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case Else
                                    _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                            End Select

                        Else

                            If unir_ Then

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Key).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0, 0, 0, 5))

                                End Select

                            Else

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                                End Select

                            End If

                        End If

                    End If

                    If unir_ = False Then

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_estiloSinBordes).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

                                End Select

                            Else

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                                End Select

                            End If

                        End If

                    Else

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length - 1).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F))

                                End Select

                            Else

                                Select Case isBorder

                                    Case TiposBordes.DerechaAbajo

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case TiposBordes.Derecha

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablalayout.AddCell(New Cell(rowspan:=0, colspan:=dimensiones_.Length).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                                End Select

                            End If

                        End If

                        unir_ = False

                    End If

                End If


            Next

        End If

        Return _tablalayout

    End Function

End Class