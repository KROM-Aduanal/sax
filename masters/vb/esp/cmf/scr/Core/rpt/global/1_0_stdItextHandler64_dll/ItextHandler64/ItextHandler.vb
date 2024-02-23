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
Imports iText.Kernel.Events
Imports iText.Kernel.Pdf.Canvas
Imports iText.Kernel.Pdf.Extgstate


Public Class ItextHandler
    Implements IItextHandler

    Private _memoryStream As MemoryStream

    Private _pdfWriter As PdfWriter

    Private _tablelayout As Table

    Private _image As Image

#Region "Propiedades"
    Property BorderlessStyle As Style Implements IItextHandler.BorderlessStyle

    Property Stylecell As Style Implements IItextHandler.Stylecell

    Property ArialBold As PdfFont Implements IItextHandler.ArialBold

    Property Arial As PdfFont Implements IItextHandler.Arial

    Property PdfDocument Implements IItextHandler.PdfDocument

    Property Document As Document Implements IItextHandler.Document

#End Region

#Region "Constructores"
    Public Sub New(watermark_ As Image)

        initialize()

    End Sub
    Private Sub initialize()

        _Arial = PdfFontFactory.CreateFont("C:/Windows/Fonts/Arial.ttf", PdfEncodings.IDENTITY_H)

        _ArialBold = PdfFontFactory.CreateFont("C:/Windows/Fonts/Arialbd.ttf", PdfEncodings.IDENTITY_H)

        _BorderlessStyle = New Style

        _BorderlessStyle.SetTextAlignment(TextAlignment.RIGHT)

        _BorderlessStyle.SetPadding(0F)

        _BorderlessStyle.SetMargin(0F)

        _BorderlessStyle.SetMarginTop(0F)

        _BorderlessStyle.SetBorder(NO_BORDER)

        _BorderlessStyle.SetBorderBottom(NO_BORDER)

        _Stylecell = New Style

        _Stylecell.SetTextAlignment(TextAlignment.CENTER)

        _memoryStream = New MemoryStream()

        _pdfWriter = New PdfWriter(_memoryStream)

        _PdfDocument = New PdfDocument(_pdfWriter)

        _Document = New Document(_PdfDocument, PageSize.LETTER)

        _Document.SetMargins(14, 20, 14, 20)

    End Sub

#End Region

#Region "Funciones"
    Function TableLevel1(folioOperacion_ As String, dimensions_ As Single()) As Table Implements IItextHandler.TableLevel1
        Dim level1_ = New Table(dimensions_).UseAllAvailableWidth

        Return level1_.AddHeaderCell(New Cell(rowspan:=0, colspan:=dimensions_.Count).Add(New Paragraph(folioOperacion_).SetTextAlignment(TextAlignment.RIGHT).SetFont(_Arial).
                                                                          SetFontSize(7.0F)).SetBorder(Border.NO_BORDER))

    End Function

    Function TableLevel1(dimensions_ As Single()) As Table Implements IItextHandler.TableLevel1

        Return New Table(dimensions_).UseAllAvailableWidth

    End Function

    Function SetTableLayout(layout_ As Dictionary(Of String, String),
                            dimensions_ As Single(), ByVal isBorder_ As IItextHandler.EdgeTypes,
                            Optional ByVal header_ As String = Nothing,
                            Optional ByVal isBackground_ As Boolean = True) As Table Implements IItextHandler.SetTableLayout

        Dim unir As Boolean = False

        _tablelayout = New Table(dimensions_).UseAllAvailableWidth

        _tablelayout.SetPaddings(0F, 0F, 0F, 0F)

        _tablelayout.SetTextAlignment(TextAlignment.CENTER)

        _tablelayout.SetMargins(0F, 0F, 0F, 0F)

        _tablelayout.SetBorder(NO_BORDER)

        If header_ IsNot Nothing Then

            If layout_ IsNot Nothing Then

                If isBackground_ Then

                    If isBorder_ <> 0 Then

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).
                                        SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                Else

                    If isBorder_ <> 0 Then

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                End If

            Else

                _tablelayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

                Select Case isBorder_

                    Case IItextHandler.EdgeTypes.RightButton

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case IItextHandler.EdgeTypes.Right

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case IItextHandler.EdgeTypes.Button

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case Else
                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).
                                        SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                End Select

            End If

        End If

        If layout_ IsNot Nothing Then

            For Each celda As KeyValuePair(Of String, String) In layout_

                If Trim(celda.Key) = "VACIO" And celda.Value = "VACIO" Then

                    unir = True

                Else

                    If Trim(celda.Key) <> "VACIO" Then

                        If celda.Value <> "VACIO" Then

                            Select Case isBorder_
                                Case IItextHandler.EdgeTypes.Right

                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                Case IItextHandler.EdgeTypes.RightButton

                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case IItextHandler.EdgeTypes.Button

                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                Case Else

                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                            End Select

                        Else

                            If unir Then

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Button

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
SetPaddings(0, 0, 0, 5).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
SetPaddings(0, 0, 0, 5))
                                End Select
                            Else
                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right
                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Button

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case Else

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                                    SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                                End Select

                            End If


                        End If

                    End If
                    If unir = False Then
                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Button

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))
                                End Select
                            Else

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Button

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
                                                    SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                    SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                                End Select

                            End If

                        End If

                    Else

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Button

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
SetPaddings(0F, 0F, 0F, 0F))

                                End Select

                            Else

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                    SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Button

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
SetPaddings(0F, 0F, 0F, 0F).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
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

        Return _tablelayout

    End Function

    Function SetTableLayout(layout_ As Dictionary(Of String, Dictionary(Of String, List(Of Int64))),
                            dimensions_ As Single(), ByVal isBorder_ As IItextHandler.EdgeTypes,
                            Optional ByVal header_ As String = Nothing,
                            Optional ByVal isBackground_ As Boolean = True) As Table Implements IItextHandler.SetTableLayout

        _tablelayout = New Table(dimensions_).UseAllAvailableWidth
        _tablelayout.SetPaddings(0F, 0F, 0F, 0F)

        _tablelayout.SetTextAlignment(TextAlignment.CENTER)

        _tablelayout.SetMargins(0F, 0F, 0F, 0F)

        If header_ IsNot Nothing Then

            If isBackground_ Then

                If isBorder_ <> 0 Then

                    _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                Else

                    _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                End If
            Else
                If isBorder_ <> 0 Then

                    _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))
                Else

                    _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                End If

            End If

        End If

        For Each celda As KeyValuePair(Of String, Dictionary(Of String, List(Of Int64))) In layout_
            If Trim(celda.Key) <> "VACIO" Then
                If celda.Value.Single.Key <> "VACIO" Then

                    If isBorder_ <> 0 Then

                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).
                                    SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    Else
                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).
                                    SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                    AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                    End If

                Else

                    If isBorder_ <> 0 Then

                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).
                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                    SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    Else

                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(0)).Add(New Paragraph(celda.Key).
                                    SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                    SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
SetPaddings(0, 0, 0, 5))
                    End If
                End If
            End If

            If celda.Value.Single.Key <> "VACIO" Then

                If Trim(celda.Key) <> "VACIO" Then

                    Select Case isBorder_

                        Case IItextHandler.EdgeTypes.RightButton

                            _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case IItextHandler.EdgeTypes.Right

                            _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case Else

                            _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))
                    End Select

                Else

                    Select Case isBorder_

                        Case IItextHandler.EdgeTypes.RightButton

                            _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case IItextHandler.EdgeTypes.Right

                            _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                        SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Case Else

                            _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=celda.Value.Single.Value(1)).Add(New Paragraph(celda.Value.Single.Key).
                                        SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).
                                        SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)
                    End Select
                End If
            End If

        Next

        Return _tablelayout

    End Function

    Function SetTableLayoutBorder(layout_ As Dictionary(Of String, String),
                                  dimensions_ As Single(),
                                  Optional ByVal header_ As String = Nothing,
                                  Optional ByVal isBackground_ As Boolean = True) As Table Implements IItextHandler.SetTableLayoutBorder

        Dim unir_ As Boolean = False

        _tablelayout = New Table(dimensions_)

        _tablelayout.SetPadding(0F)

        If header_ IsNot Nothing Then
            If isBackground_ Then

                _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                            SetBackgroundColor(ColorConstants.LIGHT_GRAY))

            Else

                _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))
            End If
        End If

        For Each celda As KeyValuePair(Of String, String) In layout_

            If Trim(celda.Key) <> "IMG" Then

                _tablelayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                            SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).Add(New Paragraph(celda.Value).
                            SetMultipliedLeading(1)).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F)).SetBorder(NO_BORDER))

            Else

                _image = New Image(ImageDataFactory.Create(celda.Value))

                _image.SetWidth(100)

                _tablelayout.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ")).Add(_image)).SetBorder(NO_BORDER).
                            SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))
            End If
        Next
        Return _tablelayout

    End Function

    Function SetTableLayoutFixed(layout_ As Dictionary(Of String, String),
                                dimensions_ As Single(),
                                ByVal isBorder_ As IItextHandler.EdgeTypes,
                                Optional ByVal header_ As String = Nothing,
                                Optional ByVal isBackground_ As Boolean = True) As Table Implements IItextHandler.SetTableLayoutFixed

        Dim unir_ As Boolean = False

        _tablelayout = New Table(dimensions_)

        _tablelayout.SetPaddings(0F, 0F, 0F, 0F)

        _tablelayout.SetTextAlignment(TextAlignment.CENTER)

        _tablelayout.SetMargins(0F, 0F, 0F, 0F)

        _tablelayout.SetBorder(NO_BORDER)
        If header_ IsNot Nothing Then
            If layout_ IsNot Nothing Then
                If isBackground_ Then

                    If isBorder_ <> 0 Then

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    End If

                Else

                    If isBorder_ <> 0 Then

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Else

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).
SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                    End If

                End If

            Else

                _tablelayout.SetBackgroundColor(ColorConstants.LIGHT_GRAY)

                Select Case isBorder_

                    Case IItextHandler.EdgeTypes.RightButton

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                        SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                    Case IItextHandler.EdgeTypes.Right

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetMargins(0F, 0F, 0F, 0F).
                                        SetPaddings(0F, 0F, 0F, 0F))

                    Case Else

                        _tablelayout.AddHeaderCell(New Cell(rowspan:=0, colspan:=8).Add(New Paragraph(header_).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                        SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetWordSpacing(0F).SetMultipliedLeading(1)).
                                        SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

                End Select

            End If

        End If

        If layout_ IsNot Nothing Then

            For Each celda As KeyValuePair(Of String, String) In layout_
                If Trim(celda.Key) = "VACIO" And celda.Value = "VACIO" Then
                    unir_ = True
                Else

                    If Trim(celda.Key) <> "VACIO" Then

                        If celda.Value <> "VACIO" Then

                            Select Case isBorder_

                                Case IItextHandler.EdgeTypes.Right
                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case IItextHandler.EdgeTypes.RightButton

                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                Case Else
                                    _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.LEFT).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))

                            End Select

                        Else

                            If unir_ Then

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0, 0, 0, 5).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case Else

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Key).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0, 0, 0, 5))

                                End Select

                            Else

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Key).SetTextAlignment(TextAlignment.CENTER).SetFont(_ArialBold).
                                                SetFontSize(8.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0, 0, 0, 5))
                                End Select
                            End If
                        End If
                    End If
                    If unir_ = False Then

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).
                                                SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case Else

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).
                                                AddStyle(_BorderlessStyle).SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F))

                                End Select

                            Else

                                Select Case isBorder_
                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell().Add(New Paragraph(celda.Value).SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).
                                                SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).
                                                SetMarginTop(0).SetMarginBottom(0).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)
                                End Select

                            End If

                        End If

                    Else

                        If celda.Value <> "VACIO" Then

                            If Trim(celda.Key) <> "VACIO" Then
                                Select Case isBorder_
                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                    Case Else

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length - 1).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.LEFT).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
SetPaddings(0F, 0F, 0F, 0F))
                                End Select
                            Else

                                Select Case isBorder_

                                    Case IItextHandler.EdgeTypes.RightButton

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)).
                                                SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case IItextHandler.EdgeTypes.Right

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
                                                SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetMarginTop(0).SetMarginBottom(0).
                                                SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                    Case Else

                                        _tablelayout.AddCell(New Cell(rowspan:=0, colspan:=dimensions_.Length).Add(New Paragraph(celda.Value).
                                                SetTextAlignment(TextAlignment.CENTER).SetFont(_Arial).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).
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

        Return _tablelayout

    End Function

    Function convertToString() As String Implements IItextHandler.ConvertToString
        Dim bytesStream_ As Byte() = _memoryStream.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0

        Return Convert.ToBase64String(ms2.ToArray())

    End Function

    Function CreateHeader(celdas_ As Dictionary(Of String, String), dimensions_ As Single()) Implements IItextHandler.CreateHeader

        _tablelayout = SetTableLayout(celdas_, dimensions_, IItextHandler.EdgeTypes.None)

        _tablelayout.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        PdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, New PageEvent(_Document, _tablelayout))

    End Function

#End Region

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
        Dim pageNumber As Integer = pdfDoc.GetPageNumber(docEvent.GetPage())
        Dim rectanguloEncabezado As Rectangle

        If pageNumber > 1 Then

            Dim xEncabezado As Single = pdfDoc.GetDefaultPageSize().GetX() + documento.GetLeftMargin()
            Dim yEncabezado As Single = pdfDoc.GetDefaultPageSize().GetTop() - documento.GetTopMargin() - 47
            Dim anchoEncabezado As Single = page.GetPageSize().GetWidth() - 107
            Dim altoEncabezado As Single = 50.0F

            rectanguloEncabezado = New Rectangle(xEncabezado, yEncabezado, anchoEncabezado, altoEncabezado)

        Else

            Dim xEncabezado As Single = pdfDoc.GetDefaultPageSize().GetX() + documento.GetLeftMargin()
            Dim yEncabezado As Single = pdfDoc.GetDefaultPageSize().GetTop() - documento.GetTopMargin() - 47
            Dim anchoEncabezado As Single = page.GetPageSize().GetWidth() - 107
            Dim altoEncabezado As Single = 50.0F

            rectanguloEncabezado = New Rectangle(xEncabezado, yEncabezado, anchoEncabezado, altoEncabezado)

        End If




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
        Dim level1_ = New Table({900.0F}).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)
        Dim _Nivel2 = New Table({150.0F}).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)


        _Nivel2.SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5))

        If pageNumber > 1 Then
            'Dim tablaEncabezado As Table = CrearEncabezadoTabla("Departamento de Recursos Humanos")
            _tablalayout.SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)
            Dim rectanguloEncabezado As Rectangle = CrearEncabezadoRectangulo(docEvent)
            Dim canvasEncabezado As New Canvas(canvas, rectanguloEncabezado)
            _Nivel2.AddCell(New Cell().Add(New Paragraph("ANEXO DEL PEDIMENTO").SetMultipliedLeading(1).SetTextAlignment(TextAlignment.CENTER).SetFont(PdfFontFactory.CreateFont("C:/Windows/Fonts/Arialbd.ttf", PdfEncodings.IDENTITY_H)).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
            level1_.AddCell(New Cell().Add(_Nivel2).SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))
            level1_.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
            level1_.SetBorder(NO_BORDER)
            canvasEncabezado.Add(level1_)
        End If

        Dim tablaNumeracion As Table = CrearPieTabla(docEvent)
        Dim rectanguloPie As Rectangle = CrearPieRectangulo(docEvent)
        Dim canvasPie As New Canvas(canvas, rectanguloPie)
        'canvasPie.Add(tablaNumeracion)
    End Sub
End Class
