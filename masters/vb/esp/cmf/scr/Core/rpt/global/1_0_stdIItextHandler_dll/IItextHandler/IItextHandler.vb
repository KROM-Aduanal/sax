
Imports iText.Kernel.Font
Imports iText.Layout
Imports iText.Layout.Element

Public Interface IItextHandler

#Region "Propiedades"
    Property BorderlessStyle As Style
    Property Stylecell As Style
    Property ArialBold As PdfFont
    Property Arial As PdfFont
    Property PdfDocument
    Property Document As Document

#End Region

#Region "Enums"
    Enum EdgeTypes
        None = 0
        Right = 1
        RightButton = 2
        Button = 3
    End Enum

#End Region

#Region "Functions"
    Function TableLevel1(folioOperacion_ As String, dimensions_ As Single()) As Table
    Function TableLevel1(dimensions_ As Single()) As Table
    Function SetTableLayout(layout_ As Dictionary(Of String, String),
                            dimensions_ As Single(), ByVal isBorder_ As EdgeTypes,
                            Optional ByVal header_ As String = Nothing,
                            Optional ByVal isBackground_ As Boolean = True) As Table
    Function SetTableLayout(layout_ As Dictionary(Of String, Dictionary(Of String, List(Of Int64))),
                            dimensions_ As Single(), ByVal isBorder_ As EdgeTypes,
                            Optional ByVal header_ As String = Nothing,
                            Optional ByVal isBackground_ As Boolean = True) As Table
    Function SetTableLayoutBorder(layout_ As Dictionary(Of String, String),
                                  dimensions_ As Single(),
                                  Optional ByVal header_ As String = Nothing,
                                  Optional ByVal isBackground_ As Boolean = True) As Table
    Function SetTableLayoutFixed(layout_ As Dictionary(Of String, String),
                                dimensions_ As Single(),
                                ByVal isBorder_ As EdgeTypes,
                                Optional ByVal header_ As String = Nothing,
                                Optional ByVal isBackground_ As Boolean = True) As Table
    Function ConvertToString() As String
    Function CreateHeader(celdas_ As Dictionary(Of String, String), dimensions_ As Single())

#End Region

End Interface