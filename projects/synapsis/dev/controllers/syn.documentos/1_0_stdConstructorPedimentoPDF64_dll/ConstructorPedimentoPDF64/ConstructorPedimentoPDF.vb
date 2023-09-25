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

Public Class ConstructorPedimentoPDF


#Region "Atributos"

    Public _ms As MemoryStream = New MemoryStream()
    Public _pw As PdfWriter = New PdfWriter(_ms)
    Public _pdfDocument As PdfDocument = New PdfDocument(_pw)
    Public _doc As Document = New Document(_pdfDocument, PageSize.LETTER)
    Public _stylecell As Style = New Style
    Public _estiloSinBordes As Style = New Style
    Public _arialBold As PdfFont
    Public _arial As PdfFont
    Public _file As File
    Public _image As Image
    Private record As Object
    Public _watermark As Image
    Public _celdas As Dictionary(Of String, String)
    Public _celdas2 As Dictionary(Of String, Dictionary(Of String, List(Of Int64)))

    Private _controladorPDF As ControladorPDF

#End Region

    Enum TiposBordes
        Niniguno = 0
        Derecha = 1
        DerechaAbajo = 2
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

    Public Function ImprimirEncabezadoPedimentoNormal(ByVal documento_ As DocumentoElectronico) As String

        Dim Nivel1_ As Table

        Dim tablalayout_ As Table

        Dim layout_ As Table

        If documento_ IsNot Nothing Then

            If documento_.Attribute(CA_FECHA_PAGO).Valor IsNot Nothing Then

                _pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, New WatherMark(_watermark))

            End If

        End If

        Dim Nivel2_ As Table = New Table({1000.0F}) '.UseAllAvailableWidth

        Dim Nivel3_ As Table = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        Dim dimensiones_ As Single()

        '====================================ENCABEZADO==============================================
        dimensiones_ = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 5.0F, 1.0F, 5.0F}
#Region "Encabezado"

        _celdas = New Dictionary(Of String, String) From {{"NUM. PEDIMENTO: ", documento_.FolioDocumento.ToString},
                                                          {"T.OPERA: ", IIf(documento_.Attribute(CA_T_OPER).Valor IsNot Nothing, documento_.Attribute(CA_T_OPER).Valor, " ")},
                                                          {"CVE. PEDIMENTO: ", IIf(documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion, "A1")},
                                                          {"REGIMEN: ", IIf(documento_.Attribute(CA_REGIMEN).Valor IsNot Nothing, documento_.Attribute(CA_REGIMEN).Valor, "IMD")}}

        Nivel1_ = _controladorPDF.FolioDoc(documento_.FolioOperacion)
        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "PEDIMENTO")

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        dimensiones_ = {2.0F, 2.0F, 1.0F, 1.0F, 2.0F, 3.0F, 1.0F, 2.0F}

        _celdas = New Dictionary(Of String, String) From {{"DESTINO: ", IIf(documento_.Attribute(CA_DESTINO_ORIGEN).Valor IsNot Nothing, documento_.Attribute(CA_DESTINO_ORIGEN).Valor, " ")},
                                                          {"TIPO CAMBIO: ", IIf(documento_.Attribute(CA_TIPO_CAMBIO).Valor IsNot Nothing, documento_.Attribute(CA_TIPO_CAMBIO).Valor, " ")},
                                                          {"PESO BRUTO: ", IIf(documento_.Attribute(CA_PESO_BRUTO).Valor IsNot Nothing, documento_.Attribute(CA_PESO_BRUTO).Valor, " ")},
                                                          {"ADUANA E/S: ", IIf(documento_.Attribute(CA_ADUANA_E_S).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_E_S).Valor, " ")}}


        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------------


        '-------------------MEDIOS DE TRANSPORTE--------------------------------------
        dimensiones_ = {1.0F, 1.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"ENTRADA/SALIDA:", "VACIO"},
                                                          {"ARRIBO:", "VACIO"},
                                                          {"SALIDA:", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_MEDIO_DE_TRANSPORTE).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_DE_TRANSPORTE).Valor, " ")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO).Valor, " ")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_SALIDA).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_SALIDA).Valor, " ")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "MEDIOS DE TRANSPORTE", False)

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '------------VALORES ADUANA, ETC------------------------------------------------
        dimensiones_ = {2.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"VALOR DOLARES:", IIf(documento_.Attribute(CA_VALOR_DOLARES).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_DOLARES).Valor, "26,015.14")},
                                                          {"VALOR ADUANA:", IIf(documento_.Attribute(CA_VALOR_ADUANA).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_ADUANA).Valor, "524,460.00")},
                                                          {"PRECIO PAGADO/VALOR COMERCIAL:", IIf(documento_.Attribute(CA_PRECIO_PAGADO_O_VALOR_COMERCIAL).Valor IsNot Nothing, documento_.Attribute(CA_PRECIO_PAGADO_O_VALOR_COMERCIAL).Valor, "524,460.00")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel2_.AddCell(New Cell().Add(Nivel3_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '--------------------------------------------------------------------------------


        '-------------DATOS DEL IMPORTADOR/EXPORTADOR------------------------------------
        dimensiones_ = {100.0F, 100.0F, 800.0F}

        _celdas = New Dictionary(Of String, String) From {{"RFC:", IIf(documento_.Attribute(CA_RFC_DEL_IOE).Valor IsNot Nothing, documento_.Attribute(CA_RFC_DEL_IOE).Valor, " ")},
                                                          {"NOMBRE, DENOMINACION O RAZON SOCIAL:", "VACIO"},
                                                          {"CURP:", IIf(documento_.Attribute(CA_CURP_DEL_IOE).Valor IsNot Nothing, documento_.Attribute(CA_CURP_DEL_IOE).Valor, " ")},
                                                          {"VACIO", IIf(documento_.Attribute(CA_RAZON_SOCIAL_IOE).ValorPresentacion IsNot Nothing, documento_.Attribute(CA_RAZON_SOCIAL_IOE).ValorPresentacion, " ")},
                                                          {"VACIO ", "VACIO"},
                                                          {"DOMICILIO:", IIf(documento_.Attribute(CA_DOMICILIO_IOE).Valor IsNot Nothing, documento_.Attribute(CA_DOMICILIO_IOE).Valor, "CARRETERA JOROBAS - TULA KM. 3.5 MANZ. 5 LOTE 1 FRACC. PARQUE INDUSTRIAL HUEHUETOCA 54680 HUEHUETOCA ESTADO DE MEXICO, MEXICO (ESTADOS UNIDOS MEXICANOS)")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "DATOS DEL IMPORTADOR/EXPORTADOR")

        tablalayout_.AddStyle(_stylecell)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '--------------------------------------------------------------------------------


        '-----------VALORES INCREMENTABLES-----------------------------------------------
        dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 2.0F}

        _celdas = New Dictionary(Of String, String) From {{"VAL. SEGUROS", "VACIO"},
                                                          {"SEGUROS", "VACIO"},
                                                          {"FLETES", "VACIO"},
                                                          {"EMBALAJES", "VACIO"},
                                                          {"OTROS INCREMENTABLES", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_VAL_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_VAL_SEGUROS).Valor, "0")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_SEGUROS).Valor, "0")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_FLETES).Valor IsNot Nothing, documento_.Attribute(CA_FLETES).Valor, "0")},
                                                          {"VACIO   ", IIf(documento_.Attribute(CA_EMBALAJES).Valor IsNot Nothing, documento_.Attribute(CA_EMBALAJES).Valor, "0")},
                                                          {"VACIO    ", IIf(documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor, "0")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

        tablalayout_.AddStyle(_stylecell)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '----------------------------------------------------------------------------------


        '-------------VALORES DECREMENTABLES-----------------------------------------------
        If documento_.Attribute(CA_VAL_SEGUROS).Valor IsNot Nothing And documento_.Attribute(CA_VAL_SEGUROS).Valor <> 0 Then

            dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 2.0F}

            tablalayout_ = New Table(dimensiones_).UseAllAvailableWidth

            _celdas = New Dictionary(Of String, String) From {{"TRANSPORTE DECREMENTABLES", "VACIO"},
                                                          {"SEGURO DECREMENTABLES", "VACIO"},
                                                          {"CARGA", "VACIO"},
                                                          {"DESCARGA", "VACIO"},
                                                          {"OTROS DECREMENTABLES", "VACIO"},
                                                          {"VACIO", IIf(documento_.Attribute(CA_VAL_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_VAL_SEGUROS).Valor, "0")},
                                                          {"VACIO ", IIf(documento_.Attribute(CA_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_SEGUROS).Valor, "0")},
                                                          {"VACIO  ", IIf(documento_.Attribute(CA_FLETES).Valor IsNot Nothing, documento_.Attribute(CA_FLETES).Valor, "0")},
                                                          {"VACIO   ", IIf(documento_.Attribute(CA_EMBALAJES).Valor IsNot Nothing, documento_.Attribute(CA_EMBALAJES).Valor, "0")},
                                                          {"VACIO    ", IIf(documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor, "0")}}

            tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "VALOR DECREMENTABLES", False)

            tablalayout_.AddStyle(_estiloSinBordes)

            Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        End If

        '------------------CÓDIGO DE VALIDACIÓN--------------------------------
        dimensiones_ = {200.0F, 550.0F, 250.0F}

        _celdas = New Dictionary(Of String, String) From {{"CÓDIGO DE ACEPTACIÓN:", IIf(documento_.Attribute(CA_ACUSE_ELECTONICO_DE_VALIDACION).Valor IsNot Nothing, documento_.Attribute(CA_ACUSE_ELECTONICO_DE_VALIDACION).Valor, "U8Z7A8E9")},
                                                          {"IMG", "C:/temp/CBA_RKU2100551.png"},
                                                          {"CLAVE DE LA SECCIÓN ADUANERA DE DESPACHO:", IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430")}}

        tablalayout_ = _controladorPDF.setTablaLayoutBorder(_celdas, dimensiones_)

        tablalayout_.SetBorder(NO_BORDER)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '-------------NUMERO DE BULTOS------------------------------------------
        dimensiones_ = {450.0F, 550.0F}

        '_tablalayout = New Table(dimensiones_) '.UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"MARCAS, NUMEROS Y TOTAL DE BULTOS:", IIf(documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor IsNot Nothing, documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor, "02 CONTENEDORES")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '-----------------------------------------------------------------------


        '------------FECHAS-----------------------------------------------------
        Nivel3_ = New Table({300.0F, 700.0F}).UseAllAvailableWidth

        Dim fechaEntrada_ As DateTime = documento_.Attribute(CA_FECHA_ENTRADA).Valor

        Dim fechaPago_ As DateTime = documento_.Attribute(CA_FECHA_PAGO).Valor

        dimensiones_ = {1.0F, 1.0F}

        _celdas = New Dictionary(Of String, String) From {{"ENTRADA:", fechaEntrada_.ToString("dd/MM/yyyy")},
                                                          {"PAGO:", fechaPago_.ToString("dd/MM/yyyy")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "FECHAS", False)

        If documento_.Attribute(CA_FECHA_ORIGINAL).Valor IsNot Nothing And documento_.Attribute(CA_CVE_PEDIMENTO).Valor = "F4" Then

            Dim fechaOriginal_ As DateTime = documento_.Attribute(CA_FECHA_ORIGINAL).Valor

            _celdas.Add("ORIGINAL:", fechaOriginal_.ToString("dd/MM/yyyy"))

        End If

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '----------TASAS NIVEL PEDIMENTO--------------------------------------
        dimensiones_ = {1.0F, 1.0F, 1.0F}
        Dim espacios_ = ""
        _celdas = New Dictionary(Of String, String) From {{"CONTRIB.:", "VACIO"},
                                                          {"CVE.T.TASA:", "VACIO"},
                                                          {"TASA:", "VACIO"}}

        With documento_.Seccion(SeccionesPedimento.ANS6)

            For Each tasas_ As Nodo In .Nodos

                If tasas_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim tasa_ = CType(tasas_, PartidaGenerica)

                    _celdas.Add("VACIO" + espacios_, IIf(tasa_.Attribute(CA_CONTRIBUCION).ValorPresentacion IsNot Nothing, tasa_.Attribute(CA_CONTRIBUCION).ValorPresentacion, "DTA"))
                    _celdas.Add("VACIO " + espacios_, IIf(tasa_.Attribute(CA_CVE_T_TASA).Valor IsNot Nothing, tasa_.Attribute(CA_CVE_T_TASA).Valor, "IVA/PRV"))
                    _celdas.Add("VACIO  " + espacios_, IIf(tasa_.Attribute(CA_TASA).Valor IsNot Nothing, tasa_.Attribute(CA_TASA).Valor, "PRV"))

                    espacios_ += "   "

                End If
            Next

            tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "TASAS A NIVEL PEDIMENTO")

        End With

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel2_.AddCell(New Cell().Add(Nivel3_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER)) '.SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
        '----------------------------------------------------------------------


        '------------CUADRO DE LIQUIDACIONES------------------------------------
        Nivel3_ = New Table({700, 300.0F}).UseAllAvailableWidth

        Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=7).Add(New Paragraph("CUADRO DE LIQUIDACIÓN").SetFont(_arialBold).SetFontSize(8.0F)))

        dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        '_tablalayout = New Table(dimensiones_).UseAllAvailableWidth
        _celdas = New Dictionary(Of String, String) From {{"CONCEPTO", "VACIO"},
                                                          {"F.P.", "VACIO"},
                                                          {"IMPORTE", "VACIO"},
                                                          {"CONCEPTO ", "VACIO"},
                                                          {"F.P. ", "VACIO"},
                                                          {"IMPORTE ", "VACIO"}}

        Dim incrementableLiq_ As Integer = 0

        espacios_ = ""

        With documento_.Seccion(SeccionesPedimento.ANS7)

            With documento_.Seccion(SeccionesPedimento.ANS55)

                For Each liquidaciones_ As Nodo In .Nodos

                    If liquidaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim liquidacion_ = CType(liquidaciones_, PartidaGenerica)
                        _celdas.Add("VACIO" + espacios_, IIf(liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion IsNot Nothing, liquidacion_.Attribute(CA_CONCEPTO).ValorPresentacion, " "))
                        _celdas.Add("VACIO " + espacios_, IIf(liquidacion_.Attribute(CA_FP).Valor IsNot Nothing, liquidacion_.Attribute(CA_FP).Valor, " "))
                        _celdas.Add("VACIO  " + espacios_, IIf(liquidacion_.Attribute(CA_IMPORTE).Valor IsNot Nothing, liquidacion_.Attribute(CA_IMPORTE).Valor, " "))

                        espacios_ += "   "

                        incrementableLiq_ += 1

                    End If

                Next

                If incrementableLiq_ < 6 Then

                    espacios_ += "   "

                    _celdas.Add("VACIO" + espacios_, " ")
                    _celdas.Add("VACIO " + espacios_, "0")
                    _celdas.Add("VACIO  " + espacios_, "11053")

                    Select Case incrementableLiq_

                        Case 1

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                        Case 2

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                        Case 3

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, "0")
                            _celdas.Add("VACIO  " + espacios_, "86352")

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                        Case 4

                            espacios_ += "   "
                            _celdas.Add("VACIO" + espacios_, " ")
                            _celdas.Add("VACIO " + espacios_, " ")
                            _celdas.Add("VACIO  " + espacios_, " ")

                    End Select

                End If

            End With

        End With

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Derecha)

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER)) '.SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


        '-------------TOTALES---------------------------------------------------------
        dimensiones_ = {1.0F, 1.0F}

        '_tablalayout = New Table(dimensiones_).UseAllAvailableWidth

        _celdas = New Dictionary(Of String, String) From {{"EFECTIVO", IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "101,879")},
                                                          {"OTROS", IIf(documento_.Attribute(CA_OTROS).Valor IsNot Nothing, documento_.Attribute(CA_OTROS).Valor, "0")},
                                                          {"TOTAL", IIf(documento_.Attribute(CA_TOTAL).Valor IsNot Nothing, documento_.Attribute(CA_TOTAL).Valor, "101,879")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.DerechaAbajo, "TOTALES")

        tablalayout_.SetFontSize(7.0F)

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel2_.AddCell(New Cell().Add(Nivel3_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).AddStyle(_stylecell).SetBorder(NO_BORDER))
        '----------------------------------------------------------------------------------


        Nivel1_.AddCell(New Cell().Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '----------------CERTIFICACIONES-----------------------------------------------------
        dimensiones_ = {10.0F}

        tablalayout_ = New Table(dimensiones_).UseAllAvailableWidth

        tablalayout_.AddStyle(_stylecell)

        tablalayout_.AddHeaderCell(New Cell().Add(New Paragraph("Página 1 de 2").SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

        tablalayout_.AddHeaderCell(New Cell().Add(New Paragraph("CERTIFICACIONES").SetFont(_arialBold).SetFontSize(7.0F).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER))
        '--------------------------------------------------------------------------------------

        Nivel1_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region

        '===============================CUERPO DEL PEDIMENTO ========================================

#Region "Cuerpo del pedimento"

#Region "Depósito referenciado y código QR"

        '-------------DEPÓSITO REFERENCIADO---------------------------------------------
        Nivel2_ = New Table({1000.0F}).UseAllAvailableWidth

        Nivel3_ = New Table({900.0F, 100.0F}).UseAllAvailableWidth

        dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel2_.AddStyle(_estiloSinBordes)

        Nivel3_.AddStyle(_estiloSinBordes)

        _image = New Image(ImageDataFactory.Create("C:/temp/CB_RKU2100551.png"))

        _image.SetWidth(80)

        Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("DEPÓSITO REFERENCIADO - LÍNEA DE CAPTURA - INFORMACIÓN DEL PAGO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel3_.AddCell(New Cell(rowspan:=0, colspan:=6).Add(New Div().Add(_image).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_DEP_REFERENCIADO).Valor IsNot Nothing, documento_.Attribute(CA_DEP_REFERENCIADO).Valor, "032100D13UP130040274 101879"))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F))).SetBorder(NO_BORDER).SetMargins(1.0F, 0F, 0F, 0F).SetPaddings(2.0F, 0F, 0F, 2.0F))

        Dim fechaPagoDeposito_ As DateTime = documento_.Attribute(CA_FECHA_PAGO).Valor

        _celdas2 = New Dictionary(Of String, Dictionary(Of String, List(Of Int64)))() From {
                                                          {"*** PAGO ELECTRÓNICO ***", New Dictionary(Of String, List(Of Int64)) From {{"VACIO", New List(Of Int64) From {6, 0}}}},
                                                          {"PATENTE:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_PATENTE).Valor IsNot Nothing, documento_.Attribute(CA_PATENTE).Valor, " "), New List(Of Int64) From {1, 1}}}},
                                                          {"PEDIMENTO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUM_PEDIMENTO).Valor IsNot Nothing, documento_.Attribute(CA_NUM_PEDIMENTO).Valor, " "), New List(Of Int64) From {1, 1}}}},
                                                          {"ADUANA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_ADUANA_DESPACHO).Valor IsNot Nothing, documento_.Attribute(CA_ADUANA_DESPACHO).Valor, "430"), New List(Of Int64) From {1, 1}}}},
                                                          {"BANCO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NOMBRE_INST_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_INST_BANCARIA).Valor, "BBVA BANCOMER"), New List(Of Int64) From {1, 5}}}},
                                                          {"LÍNEA DE CAPTURA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_LINEA_CAPTURA).Valor IsNot Nothing, documento_.Attribute(CA_LINEA_CAPTURA).Valor, "032100D13UP130040274"), New List(Of Int64) From {2, 4}}}},
                                                          {"IMPORTE PAGADO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing, documento_.Attribute(CA_EFECTIVO).Valor, "$101,879"), New List(Of Int64) From {2, 1}}}},
                                                          {"FECHA DE PAGO:", New Dictionary(Of String, List(Of Int64)) From {{fechaPagoDeposito_.Day & "/" & fechaPagoDeposito_.Month & "/" & fechaPagoDeposito_.Year, New List(Of Int64) From {2, 1}}}},
                                                          {"NÚMERO DE OPERACIÓN BANCARIA:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUM_OPERACION_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NUM_OPERACION_BANCARIA).Valor, "01221029383520"), New List(Of Int64) From {3, 3}}}},
                                                          {"NÚMERO DE TRANSACCIÓN SAT:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_NUM_TRANSACCION_SAT).Valor IsNot Nothing, documento_.Attribute(CA_NUM_TRANSACCION_SAT).Valor, "40012290120211025519"), New List(Of Int64) From {3, 3}}}},
                                                          {"MEDIO DE PRESENTACIÓN:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_MEDIO_PRESENTACION).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_PRESENTACION).Valor, "Otros medios electrónicos: (pago electrónico)"), New List(Of Int64) From {2, 4}}}},
                                                          {"MEDIO DE RECEPCIÓN/COBRO:", New Dictionary(Of String, List(Of Int64)) From {{IIf(documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor, "Efectivo - Cargo a cuenta"), New List(Of Int64) From {2, 4}}}}}


        tablalayout_ = _controladorPDF.setTablaLayout(_celdas2, dimensiones_, TiposBordes.Niniguno)
        tablalayout_.SetBorderBottom(NO_BORDER)

        Nivel3_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel2_.AddCell(New Cell().Add(Nivel3_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))
        '--------------------------------------------------


        Nivel1_.AddCell(New Cell().Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))



        '--------------------CÓDIGO QR-----------------------------------------------------
        dimensiones_ = {1000.0F}

        tablalayout_ = New Table(dimensiones_).UseAllAvailableWidth

        tablalayout_.SetFontSize(7.0F)

        tablalayout_.AddStyle(_stylecell)

        _image = New Image(ImageDataFactory.Create("C:/temp/QR_RKU2100551.png"))

        _image.SetWidth(150)

        '--------------------------------------------------------------------------------------

        Nivel1_.AddCell(New Cell().Add(_image.SetHorizontalAlignment(HorizontalAlignment.CENTER)))


#End Region

#Region "Datos del proveedor"

        '-------------------Datos de los proveedores---------------------------------------------
        Nivel2_ = New Table({1000.0F}).UseAllAvailableWidth

        Dim Nivel21_ As Table

        Nivel3_ = New Table({1000.0F}).UseAllAvailableWidth

        Nivel2_.SetBorder(NO_BORDER)

        Nivel3_.SetBorder(NO_BORDER)

        Nivel2_.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("DATOS DEL PROVEEDOR O COMPRADOR").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


        With documento_.Seccion(SeccionesPedimento.ANS10)

            For Each proveedores_ As Nodo In .Nodos

                If proveedores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim proveedor_ = CType(proveedores_, PartidaGenerica)

                    Nivel21_ = New Table({550.0F, 450.0F}).UseAllAvailableWidth

                    dimensiones_ = {50.0F, 500.0F}

                    _celdas = New Dictionary(Of String, String) From {{"ID FISCAL", "VACIO"},
                                                          {"NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL", "VACIO"},
                                                          {"VACIO", IIf(proveedor_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor IsNot Nothing, proveedor_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor, " ")},
                                                          {"VACIO ", IIf(proveedor_.Attribute(CA_NOMBRE_DEN_RAZON_SOC_POC).Valor IsNot Nothing, proveedor_.Attribute(CA_NOMBRE_DEN_RAZON_SOC_POC).Valor, " ")}}

                    tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

                    tablalayout_.AddStyle(_estiloSinBordes)

                    Nivel21_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    dimensiones_ = {400.0F, 50.0F}

                    _celdas = New Dictionary(Of String, String) From {{"DOMICILIO", "VACIO"},
                                                          {"VINCULACIÓN", "VACIO"},
                                                          {"VACIO", IIf(proveedor_.Attribute(CA_DOMICILIO_POC).Valor IsNot Nothing, proveedor_.Attribute(CA_DOMICILIO_POC).Valor, " ")},
                                                          {"VACIO ", IIf(proveedor_.Attribute(CA_VINCULACION).Valor IsNot Nothing, proveedor_.Attribute(CA_VINCULACION).Valor, "SI")}}

                    tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Derecha)

                    tablalayout_.AddStyle(_estiloSinBordes)

                    Nivel21_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                    Nivel2_.AddCell(New Cell().Add(Nivel21_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    '-----Facturas--------------------------------------------
                    With documento_.Seccion(SeccionesPedimento.ANS13)
                        dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

                        _celdas = New Dictionary(Of String, String) From {{"NUM. FACTURA", "VACIO"},
                                                                          {"FECHA", "VACIO"},
                                                                          {"INCOTERM", "VACIO"},
                                                                          {"MONEDA FACT", "VACIO"},
                                                                          {"VAL. MON. FACT", "VACIO"},
                                                                          {"FACTOR MON. FACT", "VACIO"},
                                                                          {"VAL. DOLARES", "VACIO"}}

                        tablalayout_.AddStyle(_estiloSinBordes)

                        espacios_ = ""

                        For Each facturas_ As Nodo In .Nodos

                            If facturas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim factura_ = CType(facturas_, PartidaGenerica)

                                Dim fechaFactura_ As DateTime = factura_.Attribute(CA_FECHA_FACT).Valor

                                _celdas.Add("VACIO" + espacios_, IIf(factura_.Attribute(CA_CFDI_O_FACT).Valor IsNot Nothing, factura_.Attribute(CA_CFDI_O_FACT).Valor, " "))
                                _celdas.Add("VACIO " + espacios_, fechaFactura_.Day & "/" & fechaFactura_.Month & "/" & fechaFactura_.Year)
                                _celdas.Add("VACIO  " + espacios_, IIf(factura_.Attribute(CA_INCOTERM).Valor IsNot Nothing, factura_.Attribute(CA_INCOTERM).Valor, " "))
                                _celdas.Add("VACIO   " + espacios_, IIf(factura_.Attribute(CA_CVE_MONEDA_FACT).Valor IsNot Nothing, factura_.Attribute(CA_CVE_MONEDA_FACT).Valor, " "))

                                If factura_.Attribute(CA_MONTO_MONEDA_FACT).Valor IsNot Nothing And factura_.Attribute(CA_CVE_MONEDA_FACT).Valor = "USD" Then

                                    _celdas.Add("VACIO    " + espacios_, factura_.Attribute(CA_MONTO_MONEDA_FACT).Valor.ToString)

                                Else

                                    _celdas.Add("VACIO     " + espacios_, "1.00000000")

                                End If

                                _celdas.Add("VACIO      " + espacios_, IIf(factura_.Attribute(CA_FACTOR_MONEDA).Valor IsNot Nothing, factura_.Attribute(CA_FACTOR_MONEDA).Valor, " "))
                                _celdas.Add("VACIO       " + espacios_, IIf(factura_.Attribute(CA_MONTO_USD).Valor IsNot Nothing, factura_.Attribute(CA_MONTO_USD).Valor, " "))

                                espacios_ += "        "

                            End If
                        Next

                        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.DerechaAbajo)

                    End With

                    Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                End If

            Next

        End With

        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderRight(NO_BORDER))

#End Region

#Region "Acuse valor (COVE) datos del proveedor"

        Nivel2_ = New Table({1000.0F}) '.UseAllAvailableWidth

        Nivel3_ = New Table({500.0F, 500.0F}) '.UseAllAvailableWidth

        dimensiones_ = {300.0F, 300.0F, 300.0F}

        tablalayout_ = New Table(dimensiones_) '.UseAllAvailableWidth

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel2_.SetBorder(NO_BORDER)

        'Nivel2_.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("DATOS DEL PROVEEDOR O COMPRADOR").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBackgroundColor(ColorConstants.LIGHT_GRAY))

        _celdas = New Dictionary(Of String, String) From {{"NÚMERO DE ACUSE DE VALOR", "VACIO"},
                                                        {"VINCULACIÓN", "VACIO"},
                                                        {"INCOTERM", "VACIO"}}

        tablalayout_.AddStyle(_estiloSinBordes)

        espacios_ = ""

        Dim vinculacion_ As String = ""

        With documento_.Seccion(SeccionesPedimento.ANS10)

            For Each proveedores_ As Nodo In .Nodos

                If proveedores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim proveedor_ = CType(proveedores_, PartidaGenerica)

                    With proveedor_.Seccion(SeccionesPedimento.ANS13)

                        For Each facturas_ As Nodo In .Nodos

                            If facturas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim factura_ = CType(facturas_, PartidaGenerica)

                                If factura_.Attribute(CA_VINCULACION) IsNot Nothing Then

                                    vinculacion_ = factura_.Attribute(CA_VINCULACION).Valor

                                Else

                                    vinculacion_ = "SI"

                                End If

                                _celdas.Add("VACIO" + espacios_, IIf(factura_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor IsNot Nothing, factura_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor, " "))
                                _celdas.Add("VACIO " + espacios_, vinculacion_)
                                _celdas.Add("VACIO  " + espacios_, IIf(factura_.Attribute(CA_INCOTERM).Valor IsNot Nothing, factura_.Attribute(CA_INCOTERM).Valor, " "))

                                espacios_ += "   "

                            End If
                        Next

                    End With

                End If

            Next

            tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.DerechaAbajo, "DATOS DEL PROVEEDOR O COMPRADOR")

            Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        End With

        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderLeft(New SolidBorder(ColorConstants.BLACK, 0.5)))

#End Region

#Region "Transporte, candados, guías y número"

        '------------Transportes---------------------------------------

        Nivel2_ = New Table({60.0F, 790.0F, 150.0F}) '.UseAllAvailableWidth

        dimensiones_ = {140.0F, 600.0F, 50.0F}

        tablalayout_ = New Table({60.0F}) '.UseAllAvailableWidth

        tablalayout_.SetBorder(NO_BORDER)

        Nivel2_.SetBorder(NO_BORDER)

        '_tablalayout.AddCell(New Cell().Add(New Paragraph("TRANSPORTE").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY))
        _celdas = Nothing
        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, {60.0F}, TiposBordes.Niniguno, "TRANSPORTE")

        tablalayout_.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        With documento_.Seccion(SeccionesPedimento.ANS12)

            For Each transportes_ As Nodo In .Nodos

                If transportes_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim transporte_ = CType(transportes_, PartidaGenerica)

                    _celdas = New Dictionary(Of String, String) From {{"IDENTIFICACIÓN:", IIf(transporte_.Attribute(CA_NOMBRE_RAZON_SOC_TRANSP).Valor IsNot Nothing, transporte_.Attribute(CA_NOMBRE_RAZON_SOC_TRANSP).Valor, " ")},
                                                        {"PAÍS:", "VACIO"}}

                    tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

                    Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                    _celdas = New Dictionary(Of String, String) From {{"VACIO", IIf(transporte_.Attribute(CA_CVE_PAIS_TRANSP).Valor IsNot Nothing, transporte_.Attribute(CA_CVE_PAIS_TRANSP).Valor, " ")}}

                    tablalayout_ = _controladorPDF.setTablaLayout(_celdas, {150.0F}, TiposBordes.Niniguno)

                    Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                    Exit For

                End If

            Next

        End With



        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderTop(NO_BORDER))
        '-------------------------------------------------------------------


        '-------------Candados----------------------------------------------

        If documento_.Seccion(SeccionesPedimento.ANS15).Nodos IsNot Nothing Then

            Nivel2_ = New Table({340.0F, 660.0F}) '.UseAllAvailableWidth

            dimensiones_ = {110.0F, 110.0F, 110.0F, 110.0F, 110.0F, 110.0F}

            Nivel2_.SetBorder(NO_BORDER)

            _celdas = Nothing
            tablalayout_ = _controladorPDF.setTablaLayout(_celdas, {340.0F}, TiposBordes.Niniguno, "NÚMERO DE CANDADO")

            tablalayout_.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

            Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

            With documento_.Seccion(SeccionesPedimento.ANS15)

                For Each candados_ As Nodo In .Nodos

                    If candados_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim candado_ = CType(candados_, PartidaGenerica)

                        _celdas = New Dictionary(Of String, String) From {{"VACIO", IIf(candados_.Attribute(CA_NUM_CANDADO).Valor IsNot Nothing, candados_.Attribute(CA_NUM_CANDADO).Valor, " ")},
                                                                          {"VACIO ", " "},
                                                                          {"VACIO  ", " "},
                                                                          {"VACIO   ", " "},
                                                                          {"VACIO    ", " "},
                                                                          {"VACIO     ", " "}}

                        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Derecha)

                        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Nivel2_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("1RA. REVISIÓN" & CStr(IIf(candados_.Attribute(CA_NUM_CANDADO_1RA).Valor IsNot Nothing, candados_.Attribute(CA_NUM_CANDADO_1RA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        Nivel2_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("2DA. REVISIÓN" & CStr(IIf(candados_.Attribute(CA_NUM_CANDADO_2DA).Valor IsNot Nothing, candados_.Attribute(CA_NUM_CANDADO_2DA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F).SetMultipliedLeading(1)).SetBorder(NO_BORDER))

                    End If

                Next

            End With

            Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        End If



        '-------------Guía/orden de embarque-------------------------------

        If documento_.Seccion(SeccionesPedimento.ANS16).Nodos IsNot Nothing Then

            Nivel2_ = New Table({400.0F, 600.0F}) '.UseAllAvailableWidth

            dimensiones_ = {150.0F, 50.0F, 150.0F, 50.0F, 150.0F, 50.0F}

            _celdas = Nothing
            tablalayout_ = _controladorPDF.setTablaLayout(_celdas, {400.0F}, TiposBordes.Niniguno, "NÚMERO (GUÍA/ORDEN EMBARQUE)/ID:")

            tablalayout_.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

            Nivel2_.SetBorder(NO_BORDER)

            Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

            With documento_.Seccion(SeccionesPedimento.ANS16)

                For Each candados_ As Nodo In .Nodos

                    If candados_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim candado_ = CType(candados_, PartidaGenerica)

                        _celdas = New Dictionary(Of String, String) From {{"VACIO", IIf(candado_.Attribute(CA_GUIA_O_MANIF_O_BL).Valor IsNot Nothing, candado_.Attribute(CA_GUIA_O_MANIF_O_BL).Valor, " ")},
                                                                          {"VACIO ", IIf(candado_.Attribute(CA_MASTER_O_HOUSE).Valor IsNot Nothing, candado_.Attribute(CA_MASTER_O_HOUSE).Valor, " ")},
                                                                          {"VACIO  ", " "},
                                                                          {"VACIO   ", " "},
                                                                          {"VACIO    ", " "},
                                                                          {"VACIO     ", " "}}

                        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Derecha)

                        tablalayout_.SetBorderTop(NO_BORDER)

                        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                    End If

                Next

            End With

            Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderTop(NO_BORDER))

        End If



        '-------------Número o tipo-------------------------------

        If documento_.Seccion(SeccionesPedimento.ANS17).Nodos IsNot Nothing Then

            Nivel2_ = New Table({400.0F, 600.0F}) '.UseAllAvailableWidth

            dimensiones_ = {150.0F, 50.0F, 150.0F, 50.0F, 150.0F, 50.0F}

            _celdas = Nothing
            tablalayout_ = _controladorPDF.setTablaLayout(_celdas, {400.0F}, TiposBordes.Niniguno, "NÚMERO / TIPO")

            tablalayout_.SetBorder(NO_BORDER).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)

            Nivel2_.SetBorder(NO_BORDER)

            Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

            With documento_.Seccion(SeccionesPedimento.ANS17)

                Dim noPartidas_ As Integer = documento_.Seccion(SeccionesPedimento.ANS17).Nodos.Count

                _celdas = New Dictionary(Of String, String)

                espacios_ = ""

                For Each candados_ As Nodo In .Nodos

                    If candados_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim candado_ = CType(candados_, PartidaGenerica)

                        _celdas.Add("VACIO" + espacios_, IIf(candados_.Attribute(CA_NUM_CONTENEDOR_FERRO_NUM_ECON).Valor IsNot Nothing, candados_.Attribute(CA_NUM_CONTENEDOR_FERRO_NUM_ECON).Valor, " "))
                        _celdas.Add("VACIO " + espacios_, vinculacion_)

                        espacios_ += "  "

                    End If

                Next

                If noPartidas_ = 1 Then

                    _celdas.Add("VACIO" + espacios_, "         ")
                    _celdas.Add("VACIO " + espacios_, "     ")
                    _celdas.Add("VACIO  " + espacios_, "         ")
                    _celdas.Add("VACIO   " + espacios_, "     ")

                ElseIf noPartidas_ = 2 Then

                    _celdas.Add("VACIO" + espacios_, "         ")
                    _celdas.Add("VACIO " + espacios_, "     ")

                End If

                tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Derecha)

                Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

            End With

            Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        End If

#End Region

#Region "Identificadores"

        Nivel2_ = New Table({1000.0F}).UseAllAvailableWidth

        dimensiones_ = {4.0F, 1.0F, 3.0F, 3.0F, 3.0F}

        _celdas = New Dictionary(Of String, String) From {{"CLAVE / COMPL. IDENTIFICADOR", "VACIO"},
                                                        {" ", "VACIO"},
                                                        {"COMPLEMENTO 1", "VACIO"},
                                                        {"COMPLEMENTO 2", "VACIO"},
                                                        {"COMPLEMENTO 3", "VACIO"}}

        espacios_ = ""

        With documento_.Seccion(SeccionesPedimento.ANS18)

            For Each identificadores_ As Nodo In .Nodos

                If identificadores_.TipoNodo = Nodo.TiposNodo.Partida Then

                    Dim identificador_ = CType(identificadores_, PartidaGenerica)

                    _celdas.Add("VACIO" + espacios_, " ")
                    _celdas.Add("VACIO " + espacios_, IIf(identificador_.Attribute(CA_CVE_IDENTIFICADOR_G).Valor IsNot Nothing, identificador_.Attribute(CA_CVE_IDENTIFICADOR_G).Valor, " "))
                    _celdas.Add("VACIO  " + espacios_, IIf(identificador_.Attribute(CA_COMPL_1).Valor IsNot Nothing, identificador_.Attribute(CA_COMPL_1).Valor, " "))
                    _celdas.Add("VACIO   " + espacios_, IIf(identificador_.Attribute(CA_COMPL_2).Valor IsNot Nothing, identificador_.Attribute(CA_COMPL_2).Valor, " "))
                    _celdas.Add("VACIO    " + espacios_, IIf(identificador_.Attribute(CA_COMPL_3).Valor IsNot Nothing, identificador_.Attribute(CA_COMPL_3).Valor, " "))

                    espacios_ += "     "

                End If

            Next

        End With

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Derecha)

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel2_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region

#Region "Observaciones"

        dimensiones_ = {1000.0F}

        _celdas = New Dictionary(Of String, String)

        Dim observacioPartida_ As String = ""

        With documento_.Seccion(SeccionesPedimento.ANS23)

            For Each observaciones_ As Nodo In .Nodos

                If observaciones_.TipoNodo = Nodo.TiposNodo.Nodo Then

                    observacioPartida_ += CStr(IIf(documento_.Attribute(CA_OBSERV_PEDIM).Valor IsNot Nothing, documento_.Attribute(CA_OBSERV_PEDIM).Valor, " "))

                End If

            Next

            _celdas.Add("VACIO" + espacios_, observacioPartida_)

        End With

        Nivel2_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno, "OBSERVACIONES")

        'Nivel2_.AddStyle(_estiloSinBordes)

        'Nivel2_.AddCell(New Cell().Add(_tablalayout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region

#Region "Partidas"

        Nivel2_ = New Table({1000.0F}).UseAllAvailableWidth

        Nivel3_ = New Table({1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F})

        'dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

        '_tablalayout = New Table(dimensiones_).UseAllAvailableWidth

        '_tablalayout.SetBorder(NO_BORDER)

        Nivel2_.SetBorder(NO_BORDER)

        Nivel3_.AddStyle(_estiloSinBordes)

        If documento_.Seccion(SeccionesPedimento.ANS24).Nodos IsNot Nothing Then

            With documento_.Seccion(SeccionesPedimento.ANS24)

                '------------Encabezado partidas--------------------------
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=16).Add(New Paragraph("PARTIDAS").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F).SetMultipliedLeading(1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("FRACCIÓN").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("SUBD/NÚM.ID ENTIFICACIÓN COMERCIAL").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("VINC.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("MET VAL.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("UMC").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMC").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("UMT").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMT").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("P.V/C").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("P.O/D").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=5).Add(New Div().Add(New Paragraph(" ")).Add(New Paragraph(" ")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("SEC").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=10).Add(New Paragraph("DESCRIPCION (RENGLONES VARIABLES SEGUN SE REQUIERA)").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("CON.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("TASA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("T.T").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("T.P").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph("IMPORTE").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("VAL ADU/USD").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("IMP. PRECIO PAG.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("PRECIO UNIT.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph("VAL. AGREG.").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("MARCA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph("MODELO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph("CÓDIGO PRODUCTO").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))


                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                Nivel3_.AddHeaderCell(New Cell().Add(New Div().Add(New Paragraph(" ").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8.0F))).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))


                'Identificadores y NOMS

                For Each partidas_ As Nodo In .Nodos

                    dimensiones_ = {1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F}

                    tablalayout_ = New Table(dimensiones_).UseAllAvailableWidth

                    tablalayout_.SetBorder(NO_BORDER)

                    Dim Nivel4_ As Table = New Table({1000.0F}).UseAllAvailableWidth

                    Nivel4_.SetBorder(NO_BORDER)

                    If partidas_.TipoNodo = Nodo.TiposNodo.Partida Then

                        Dim partida_ = CType(partidas_, PartidaGenerica)

                        Dim noTasas_ As Integer

                        noTasas_ = partida_.Seccion(SeccionesPedimento.ANS29).Nodos.Count

                        Nivel3_.AddCell(New Cell(rowspan:=noTasas_, colspan:=0).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_NUM_SEC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_NUM_SEC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_FRACC_ARANC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_FRACC_ARANC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_NICO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_NICO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_VINCULACION).Valor IsNot Nothing, partida_.Attribute(CA_VINCULACION).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_MET_VALOR_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_MET_VALOR_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CANT_UMC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CANT_UMC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CANT_UMT_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CANT_UMT_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_PAIS_VEND_O_COMP_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_PAIS_VEND_O_COMP_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=10).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_DESCRIP_MERC_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_DESCRIP_MERC_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        tablalayout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_VAL_ADU_O_VAL_USD_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_VAL_ADU_O_VAL_USD_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_MONTO_PRECIO_UNITARIO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_MONTO_PRECIO_UNITARIO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        Dim montoValor_ As String
                        If partida_.Attribute(CA_MONTO_VALOR_AGREG_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_MONTO_VALOR_AGREG_PARTIDA).Valor > 0 Then
                            montoValor_ = partida_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor.ToString
                        Else
                            montoValor_ = "0"
                        End If

                        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(montoValor_).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Paragraph(" ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        'Marca producto
                        If partida_.Attribute(CA_NOMBRE_MARCA_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_NOMBRE_MARCA_PARTIDA).Valor <> "" Then

                            tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_NOMBRE_MARCA_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_NOMBRE_MARCA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Modelo producto
                        If partida_.Attribute(CA_CVE_MODELO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_CVE_MODELO_PARTIDA).Valor <> "" Then

                            tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CVE_MODELO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CVE_MODELO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Codigo de producto
                        If partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor IsNot Nothing And partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor <> "" Then

                            tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=3).Add(New Paragraph(CStr(IIf(partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor IsNot Nothing, partida_.Attribute(CA_CODIGO_PRODUCTO_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Regulaciones y restricciones no arancelarias
                        If partida_.Seccion(SeccionesPedimento.ANS26).Nodos IsNot Nothing Then

                            layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("CLAVE").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("NUM. PERMISO O NOM").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("FIRMA DESCARGO").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("VAL. COM. DLS.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("CANTIDAD UMT/C").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With documento_.Seccion(SeccionesPedimento.ANS26)

                                For Each regulaciones_ As Nodo In .Nodos

                                    If regulaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim regulacion_ = CType(regulaciones_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_CVE_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_CVE_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_NUM_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_NUM_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_FIRM_ELECTRON_PERMISO).Valor IsNot Nothing, regulacion_.Attribute(CA_FIRM_ELECTRON_PERMISO).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_MONTO_USD_VAL_COM).Valor IsNot Nothing, regulacion_.Attribute(CA_MONTO_USD_VAL_COM).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(regulacion_.Attribute(CA_CANT_UMT_O_UMC).Valor IsNot Nothing, regulacion_.Attribute(CA_CANT_UMT_O_UMC).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Identificadores nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS27).Nodos IsNot Nothing Then

                            layout_ = New Table({1.0F, 1.0F, 1.0F, 1.0F}).UseAllAvailableWidth

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("IDENTIF.").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 1").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 2").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("COMPLEMENTO 3").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With partida_.Seccion(SeccionesPedimento.ANS27)

                                For Each identificadores_ As Nodo In .Nodos

                                    If identificadores_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim identificador_ = CType(identificadores_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_CVE_IDENTIF_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_CVE_IDENTIF_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.CENTER).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPL_1_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPL_1_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPL_2_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPL_2_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(identificador_.Attribute(CA_COMPL_3_PARTIDA).Valor IsNot Nothing, identificador_.Attribute(CA_COMPL_3_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        End If

                        'Observaciones nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS36) IsNot Nothing Then

                            layout_ = New Table({100.0F}).UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            layout_.AddHeaderCell(New Cell().Add(New Paragraph("OBSERVACIONES A NIVEL PARTIDA").SetTextAlignment(TextAlignment.CENTER).SetFont(_arialBold).SetFontSize(8.0F)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

                            With documento_.Seccion(SeccionesPedimento.ANS36)

                                For Each observaciones_ As Nodo In .Nodos

                                    If observaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim observacion_ = CType(observaciones_, PartidaGenerica)

                                        layout_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(observacion_.Attribute(CA_OBSERV_PARTIDA).Valor IsNot Nothing, observacion_.Attribute(CA_OBSERV_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).AddStyle(_estiloSinBordes).SetBorder(NO_BORDER))

                                    End If

                                Next

                            End With

                            Nivel4_.AddCell(New Cell().Add(layout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        End If

                        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=10).Add(Nivel4_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

                        Nivel3_.AddCell(New Cell(rowspan:=noTasas_, colspan:=10).Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                        'Tasas a nivel partida
                        If partida_.Seccion(SeccionesPedimento.ANS36) IsNot Nothing Then

                            dimensiones_ = {150.0F, 150.0F, 200.0F, 200.0F, 300.0F}

                            layout_ = New Table(dimensiones_) '.UseAllAvailableWidth

                            layout_.SetBorder(NO_BORDER)

                            Dim incrementable_ As Integer = 1

                            With partida_.Seccion(SeccionesPedimento.ANS29)

                                For Each tasasPartidas_ As Nodo In .Nodos

                                    If tasasPartidas_.TipoNodo = Nodo.TiposNodo.Partida Then

                                        Dim tasaPartida_ = CType(tasasPartidas_, PartidaGenerica)

                                        If incrementable_ = 1 Then

                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CONTRIBUCION_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CONTRIBUCION_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_FP_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_FP_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                        Else

                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CONTRIBUCION_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CONTRIBUCION_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_FP_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_FP_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))
                                            Nivel3_.AddCell(New Cell().Add(New Paragraph(CStr(IIf(tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor IsNot Nothing, tasaPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(7.0F)).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

                                        End If

                                    End If

                                    incrementable_ += 1

                                Next

                            End With

                            'Nivel3_.AddCell(New Cell(rowspan:=0, colspan:=16).SetWidth(-1)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F) '.SetBorder(NO_BORDER)

                            'Nivel3_.AddCell(New Cell(rowspan:=0, colspan:=5).Add(_layout).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F)).SetBorder(NO_BORDER)

                        End If

                    End If

                Next

            End With

        End If


        '-----Agregamos el nivel 3 al nivel 2 así como, el nivel 2 al contenedor principal

        Nivel2_.AddCell(New Cell().Add(Nivel3_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region

#Region "Fin del pedimento"

        dimensiones_ = {4.0F, 2.0F, 4.0F}

        tablalayout_ = New Table(dimensiones_).UseAllAvailableWidth

        tablalayout_.AddStyle(_estiloSinBordes)

        '_tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("********* ")).Add(New Paragraph(documento_.Attribute(CA_FIN_PEDIMENTO).Valor.ToString)).Add(New Paragraph(" *********"))))
        '_tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("NUM. TOTAL DE PARTIDAS: ")).Add(New Paragraph(documento_.Attribute(CA_NUMERO_TOTAL_PARTIDAS).Valor.ToString))))
        '_tablalayout.AddCell(New Cell().Add(New Div().Add(New Paragraph("**** CLAVE DE PREVALIDADOR: ")).Add(New Paragraph(documento_.Attribute(CA_CVE_PREVAL).Valor.ToString)).Add(New Paragraph(" ****"))))

        tablalayout_.AddCell(New Cell().Add(New Div().Add(New Paragraph("********* FIN DE PEDIMENTO *********").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))
        tablalayout_.AddCell(New Cell().Add(New Div().Add(New Paragraph("NUM. TOTAL DE PARTIDAS: 13").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))
        tablalayout_.AddCell(New Cell().Add(New Div().Add(New Paragraph("**** CLAVE DE PREVALIDADOR: 010 ****").SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1))).SetBorder(NO_BORDER).SetFont(_arial).SetFontSize(9.0F))


        Nivel1_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F))

#End Region

#End Region

        '==================================PIE DEL PEDIMENTO ========================================

#Region "Pie de pagina"

        Nivel2_ = New Table({1000.0F}) '.UseAllAvailableWidth

        Nivel3_ = New Table({500.0F, 500.0F})

        dimensiones_ = {100.0F, 400.0F, 100.0F, 400.0F}

        tablalayout_ = New Table(dimensiones_)

        _celdas = New Dictionary(Of String, String) From {{"VACIO", "VACIO"},
                                                          {"AGENTE ADUANAL, AGENCIA ADUANAL, APODERADO ADUANAL O DE ALMACEN NOMBRE O RAZ. SOC.:", "VACIO"},
                                                          {"VACIO ", "VACIO"},
                                                          {"VACIO   ", IIf(documento_.Attribute(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA).Valor, "JESUS ENRIQUE GOMEZ REYES - GRUPO REYES KURI S.C. GORJ800903BA")},
                                                          {"RFC:", IIf(documento_.Attribute(CA_RFC_AA).Valor IsNot Nothing, documento_.Attribute(CA_RFC_AA).Valor, " ")},
                                                          {"CURP:", IIf(documento_.Attribute(CA_CURP_AA_O_REP_LEGAL).Valor IsNot Nothing, documento_.Attribute(CA_CURP_AA_O_REP_LEGAL).Valor, " ")},
                                                          {"VACIO    ", "VACIO"},
                                                          {"MANDATARIO/PERSONA AUTORIZADA", "VACIO"},
                                                          {"VACIO     ", "VACIO"},
                                                          {"NOMBRE:", IIf(documento_.Attribute(CA_NOMBRE_MAND_REP_AA).Valor IsNot Nothing, documento_.Attribute(CA_NOMBRE_MAND_REP_AA).Valor, " ")},
                                                          {"RFC: ", IIf(documento_.Attribute(CA_RFC_MAND_O_AGAD_REP_ALMACEN).Valor IsNot Nothing, documento_.Attribute(CA_RFC_MAND_O_AGAD_REP_ALMACEN).Valor, " ")},
                                                          {"CURP: ", IIf(documento_.Attribute(CA_CURP_MAND_O_AGAD_REP_ALMACEN).Valor IsNot Nothing, documento_.Attribute(CA_CURP_MAND_O_AGAD_REP_ALMACEN).Valor, " ")}}

        tablalayout_ = _controladorPDF.setTablaLayout(_celdas, dimensiones_, TiposBordes.Niniguno)

        tablalayout_.AddStyle(_estiloSinBordes)

        Nivel2_.SetBorder(NO_BORDER)

        tablalayout_.AddCell(New Cell(rowspan:=0, colspan:=4).Add(New Paragraph("NÚMERO DE SERIE DEL CERTIFICADO: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor IsNot Nothing, documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderTop(New SolidBorder(ColorConstants.BLACK, 0.5)))

        Nivel3_.AddCell(New Cell().Add(tablalayout_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderRight(New SolidBorder(ColorConstants.BLACK, 0.5)))

        Nivel3_.AddCell(New Cell().Add(New Paragraph("DECLARO BAJO PROTESTA DE DECIR VERDAD, EN LOS TERMINOS DE LO DISPUESTO POR EL ARTICULO 81 DE LA LEY ADUANERA: PATENTE O AUTORIZACION: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_PATENTE).Valor IsNot Nothing, documento_.Attribute(CA_PATENTE).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F)).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel2_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel3_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        Nivel2_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Div().Add(New Paragraph("e.firma: ").SetTextAlignment(TextAlignment.LEFT).SetFont(_arialBold).SetFontSize(8.0F)).Add(New Paragraph(CStr(IIf(documento_.Attribute(CA_EFIRMA).Valor IsNot Nothing, documento_.Attribute(CA_EFIRMA).Valor, " "))).SetTextAlignment(TextAlignment.LEFT).SetFont(_arial).SetFontSize(9.0F))).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER).SetBorderBottom(New SolidBorder(ColorConstants.BLACK, 0.5)))

        Nivel2_.AddCell(New Cell(rowspan:=0, colspan:=2).Add(New Div().Add(New Paragraph(" ")).Add(New Paragraph(" "))).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorder(NO_BORDER))

        Nivel1_.AddFooterCell(New Cell(rowspan:=0, colspan:=2).Add(Nivel2_).SetMargins(0F, 0F, 0F, 0F).SetPaddings(0F, 0F, 0F, 0F).SetBorderTop(NO_BORDER))

#End Region



        '-----------------------------------------finalcito :3
        _doc.Add(Nivel1_)

        'Dim n As Integer = _pdfDocument.GetNumberOfPages()

        'For i_ As Integer = 1 To n

        '    _doc.ShowTextAligned(New Paragraph("página " & i_ & " de " & n), 200, 806, i_, TextAlignment.RIGHT, VerticalAlignment.TOP, 0)

        'Next


        '------------- REVISIÓN PARA LA MARCA DE AGUA ----------------------------------------

        'Dim over_ As PdfCanvas = New PdfCanvas(_pdfDocument.GetPage(1))
        'Dim over_ As PdfCanvas = New PdfCanvas(_pdfDocument, 1)
        'Dim over_ As PdfCanvas = New PdfCanvas(_pdfDocument.GetFirstPage().NewContentStreamBefore(), New PdfResources(), _pdfDocument)
        'over_.SetFillColor(ColorConstants.BLACK)
        'Dim p As Paragraph = New Paragraph("DOCUMENTO NO OFICIAL").SetFont(_arial).SetFontSize(18)
        'over_.SaveState()
        'Dim gs1 As PdfExtGState = New PdfExtGState()
        'gs1.SetFillOpacity(0.5F)
        'over_.SetExtGState(gs1)
        'Dim rect_ As Rectangle = New Rectangle(36, 36, 559, 806)
        'Dim canva_ As Canvas = New Canvas(over_, rect_) '(over_, _doc, _pdfDocument.GetDefaultPageSize()).ShowTextAligned(p, 297, 450, 1, TextAlignment.CENTER, VerticalAlignment.TOP, 0)
        'over_.RestoreState()
        '-------------------------------------------------------------------------------------

        _doc.Close()

        Dim bytesStream_ As Byte() = _ms.ToArray
        Dim ms2 As MemoryStream = New MemoryStream()
        ms2.Write(bytesStream_, 0, bytesStream_.Length)
        ms2.Position = 0

        'Dim path = "\\10.66.1.15\c$\inetpub\wwwroot\saxtest\sax\projects\synapsis\dev\main\download\"


        Dim inputAsString = Convert.ToBase64String(ms2.ToArray())


        Return inputAsString

        'Dim file As New FileStream(path & documento_.FolioOperacion & ".pdf", FileMode.OpenOrCreate, FileAccess.Write)
        'Dim file As New FileStream("C:\Temp\pedimentoNormal" & documento_.FolioOperacion & ".pdf", FileMode.OpenOrCreate, FileAccess.Write)
        'ms2.WriteTo(file)
        'file.Close()
        '_ms.Close()

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
