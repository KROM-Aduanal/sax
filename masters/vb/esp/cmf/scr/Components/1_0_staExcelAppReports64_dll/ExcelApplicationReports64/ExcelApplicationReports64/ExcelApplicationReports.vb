Imports Microsoft.Office.Interop
Imports gsol.BaseDatos.Operaciones
Imports gsol
Imports System.Text
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports NPOI.SS.UserModel
Imports NPOI.SS.Util
Imports NPOI.HSSF.Util
Imports NPOI.POIFS.FileSystem
Imports NPOI.HPSF
Imports NPOI.XSSF.UserModel
Imports NPOI
Imports NPOI.SS.UserModel.Charts

Namespace Wma.Reports

    Public Class ExcelApplicationReports
        Implements IDisposable

#Region "Attributes"

        Private Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Long, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long

        Public Enum FieldTypes
            AsUndefined
            AsString
            AsInteger
            AsDouble
            AsDate
        End Enum

        Public Enum StyleTypes
            Undefined
            ColumnHeader
            Title
            Subtitle
            User
            NormalRow
            AlternativeRow
        End Enum

        Public Enum WorksheetThemes
            Undefined = 0
            Theme1 = 2
            Theme2 = 3
            Theme3 = 4
            Theme4 = 5
            Theme5 = 6
            Theme6 = 7
            Theme7 = 8
            Theme8 = 9
            Theme9 = 10
            OliveWorld = 11
            Theme11 = 12
            Theme12 = 13
        End Enum

        'Excel elements
        Private WithEvents _excelapplication As Microsoft.Office.Interop.Excel.Application

        'WorkSheet
        Private objHojaExcel_ As Excel.Worksheet

        'WorkBook
        Private _objExcelBook As Excel.Workbook

        'WorkBook NPOI
        Private _objExcelBookNPOI As HSSFWorkbook

        Private _ioperationscatalog As IOperacionesCatalogo

        'Report Attributes
        Private _title As String

        Private _subtitle As String

        'User experience
        Private _worksheettheme As WorksheetThemes

        Private _system As Organismo = New Organismo

        'Report Inventory
        Private _finalcolumnid As String

        Private _finalcolumnletter As StringBuilder

        Private _finalrow As Int32

        Private _listvisiblecolumns As List(Of StringBuilder)

        Private _listvisiblecolumnsNames As Dictionary(Of Int32, StringBuilder)

        Private _automaticChart As Boolean

        Private _automaticTotals As Boolean

        Private _reportpassword As String

        'Template
        Private _filetemplate As String

        Private _pathsavefile As String

        Private _makermode As IReportw.MakerModes

        'Report manual properties
        Private _reportCollections As ReportCollections

        Private _objCSVReport As StringBuilder

        Private _sistema As Organismo

#End Region

#Region "Builders"

        Sub New()

            _finalcolumnid = "A3"

            _finalcolumnletter = New StringBuilder

            _finalcolumnletter.Append("A")

            _finalrow = 1

            _listvisiblecolumns = New List(Of StringBuilder)

            _listvisiblecolumnsNames = New Dictionary(Of Int32, StringBuilder)

            _title = "{Title Report}"

            _subtitle = "{Subtitle Report}"

            _worksheettheme = WorksheetThemes.OliveWorld

            _automaticChart = False

            _automaticTotals = False

            _reportpassword = Nothing

            _filetemplate = ""

            _pathsavefile = ""

            _makermode = IReportw.MakerModes.ThroughPackage

            'Report manual properties
            _reportCollections = New ReportCollections

            _sistema = New Organismo

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property GetReportCollections As ReportCollections

            Get
                Return _reportCollections
            End Get

        End Property


        Public WriteOnly Property SetReportCollections As ReportCollections

            Set(value As ReportCollections)
                _reportCollections = value
            End Set

        End Property


        Public Property MakerMode As IReportw.MakerModes

            Get
                Return _makermode
            End Get

            Set(value As IReportw.MakerModes)
                _makermode = value
            End Set

        End Property


        Public Property SaveFileIn As String

            Get
                Return _pathsavefile
            End Get

            Set(value As String)
                _pathsavefile = value
            End Set

        End Property


        Public Property FileTemplate As String

            Get
                Return _filetemplate
            End Get

            Set(value As String)
                _filetemplate = value
            End Set

        End Property


        Public Property AutomaticChart As Boolean

            Get
                Return _automaticChart
            End Get

            Set(value As Boolean)
                _automaticChart = value
            End Set

        End Property


        Public Property AutomaticTotals As Boolean

            Get
                Return _automaticTotals
            End Get

            Set(value As Boolean)
                _automaticTotals = value
            End Set

        End Property


        Public ReadOnly Property getExcelBook As Excel.Workbook

            Get
                Return _objExcelBook
            End Get

        End Property


        Public ReadOnly Property getExcelBookNPOI As HSSFWorkbook

            Get
                Return _objExcelBookNPOI
            End Get

        End Property

        Public ReadOnly Property getCSVReport As StringBuilder

            Get
                Return _objCSVReport
            End Get

        End Property

        Public Property Title As String

            Get
                Return _title
            End Get

            Set(value As String)
                _title = value
            End Set

        End Property

        Public Property SubTitle As String

            Get
                Return _subtitle
            End Get

            Set(value As String)
                _subtitle = value
            End Set

        End Property

        Public WriteOnly Property setWorksheetTheme As WorksheetThemes

            Set(value As WorksheetThemes)
                _worksheettheme = value
            End Set

        End Property

        Public Property IOperationsCatalog As IOperacionesCatalogo

            Get
                Return _ioperationscatalog
            End Get

            Set(value As IOperacionesCatalogo)
                _ioperationscatalog = value

                If Not value Is Nothing Then
                    If Not value.Nombre Is Nothing Then
                        _title = value.Nombre
                    End If

                    If Not value.EspacioTrabajo Is Nothing And _
                        Not value.EspacioTrabajo.MisCredenciales Is Nothing Then
                        _subtitle = "Generated by:" & value.EspacioTrabajo.MisCredenciales.CredencialUsuario & _
                                    " Name:" & value.EspacioTrabajo.MisCredenciales.NombreAutenticacion
                    End If


                End If
            End Set

        End Property

#End Region

#Region "Methods"

        Private Function SetStyleNPOI(ByVal rowIndex_ As Int32, _
                             ByVal columnIndex_ As Int32, _
                             ByVal cellValue_ As Object, _
                             ByVal styleType_ As StyleTypes, _
                             ByVal fieldType_ As FieldTypes, _
                             ByRef cellStyle_ As ICellStyle, _
                             ByRef styleCellBackGroundFont_ As IFont, _
                             ByRef row_ As IRow, _
                             ByRef objLibroExcel As HSSFWorkbook, _
                             ByRef objHojaExcel As ISheet) As IRow

            Dim cell1_ As HSSFCell = row_.CreateCell(columnIndex_)

            Select Case fieldType_

                Case FieldTypes.AsDouble

                    If IsNumeric(cellValue_) Then

                        Dim value_ As Double = Convert.ToDouble(cellValue_)

                        cell1_.SetCellValue(value_)

                    Else

                        cell1_.SetCellValue(cellValue_)

                    End If

                Case FieldTypes.AsInteger

                    If IsNumeric(cellValue_) Then

                        Dim value_ As Double = Convert.ToDouble(cellValue_)

                        cell1_.SetCellValue(value_)
                    Else

                        cell1_.SetCellValue(cellValue_)

                    End If

                Case FieldTypes.AsDate

                    If IsDate(cellValue_) Then

                        Dim value_ As New Date

                        value_ = Convert.ToDateTime(cellValue_)

                        cell1_.SetCellValue(value_)

                    Else
                        cell1_.SetCellValue(cellValue_)

                    End If

                Case FieldTypes.AsUndefined

                    Try
                        cell1_.SetCellValue(cellValue_)

                    Catch ex As Exception

                        cell1_.SetCellValue("")

                    End Try

                Case Else

                    cell1_.SetCellValue(cellValue_)

            End Select

            Select Case styleType_

                Case StyleTypes.ColumnHeader

                    cellStyle_.BorderTop = BorderStyle.Thin

                    cellStyle_.BorderBottom = BorderStyle.Thin

                    cellStyle_.BorderLeft = BorderStyle.Thin

                    cellStyle_.BorderRight = BorderStyle.Thin

                    cellStyle_.BottomBorderColor = HSSFColor.DarkGreen.Index

                    cellStyle_.TopBorderColor = HSSFColor.DarkGreen.Index

                    cellStyle_.LeftBorderColor = HSSFColor.DarkGreen.Index

                    cellStyle_.RightBorderColor = HSSFColor.DarkGreen.Index

                    cellStyle_.FillBackgroundColor = HSSFColor.Green.Index

                    styleCellBackGroundFont_.FontName = HSSFFont.FONT_ARIAL

                    styleCellBackGroundFont_.Boldweight = FontBoldWeight.Bold

                    styleCellBackGroundFont_.Color = HSSFColor.Grey80Percent.Index

                    styleCellBackGroundFont_.FontHeightInPoints = 10

                    cellStyle_.SetFont(styleCellBackGroundFont_)

                    row_.Height = 350

                    objHojaExcel.AutoSizeColumn(columnIndex_)

                Case StyleTypes.NormalRow

                    If Not styleCellBackGroundFont_ Is Nothing Then

                    Else

                    End If

                Case StyleTypes.Title

                    styleCellBackGroundFont_.FontName = HSSFFont.FONT_ARIAL

                    styleCellBackGroundFont_.Boldweight = FontBoldWeight.Bold

                    styleCellBackGroundFont_.Color = HSSFColor.Green.Index

                    styleCellBackGroundFont_.FontHeightInPoints = 14

                    cellStyle_.SetFont(styleCellBackGroundFont_)

                    row_.Height = 450

                Case StyleTypes.Subtitle

                    styleCellBackGroundFont_.FontName = HSSFFont.FONT_ARIAL

                    styleCellBackGroundFont_.Boldweight = FontBoldWeight.Bold

                    styleCellBackGroundFont_.Color = HSSFColor.Black.Index

                    styleCellBackGroundFont_.FontHeightInPoints = 9

                    cellStyle_.SetFont(styleCellBackGroundFont_)

                    row_.Height = 350

                Case StyleTypes.User

                    styleCellBackGroundFont_.FontName = HSSFFont.FONT_ARIAL

                    styleCellBackGroundFont_.Boldweight = FontBoldWeight.Normal

                    styleCellBackGroundFont_.Color = HSSFColor.Grey80Percent.Index

                    styleCellBackGroundFont_.FontHeightInPoints = 9

                    styleCellBackGroundFont_.IsItalic = True

                    cellStyle_.SetFont(styleCellBackGroundFont_)

                    row_.Height = 300

            End Select

            If Not cellStyle_ Is Nothing Then

                cell1_.CellStyle = cellStyle_

            End If

            Return row_

        End Function

        Public Sub CreateReport()

            Try

                'Working with modal repot
                Select Case _makermode

                    Case IReportw.MakerModes.ThroughPackage

                        _worksheettheme = WorksheetThemes.Undefined

                        Dim objWokSheet1_ As ISheet

                        Dim _objExcelBookNPOI_WB As IWorkbook = Nothing

                        If _worksheettheme = WorksheetThemes.OliveWorld Then

                            ' Create the excel workbook instance.

                            ' Load the workbook.                          
                            Using file As New FileStream(Nothing, FileMode.Open, FileAccess.Read)

                                _objExcelBookNPOI_WB = New XSSFWorkbook(file)

                            End Using

                            ' Get the first sheet.
                            objWokSheet1_ = _objExcelBookNPOI_WB.GetSheetAt(0)

                            PrepareWorksheet(_objExcelBookNPOI_WB, objWokSheet1_)

                        Else
                            'Dim _objExcelBookNPOI As 
                            _objExcelBookNPOI = New HSSFWorkbook()

                            objWokSheet1_ = _objExcelBookNPOI.CreateSheet("KrombaseReports")

                            PrepareWorksheet(_objExcelBookNPOI, objWokSheet1_)

                        End If

                        'Creating columns
                        If CreateExcelColumns(objWokSheet1_, _objExcelBookNPOI) Then

                            If CreateContents(_ioperationscatalog.Vista, objWokSheet1_, _objExcelBookNPOI) Then

                                If Not _reportpassword Is Nothing Then

                                End If

                                If _pathsavefile <> "" Then

                                    '_objExcelBook.SaveAs(_pathsavefile, Excel.XlFileFormat.xlWorkbookDefault)

                                    'Else
                                    '_objExcelBook.SaveAs("c:\logs\Temp " & _ioperationscatalog.EspacioTrabajo.MisCredenciales.NombreAutenticacion & ".xlsx", Excel.XlFileFormat.xlWorkbookDefault)

                                End If

                                Dim savePath As String = Nothing

                                Try
                                    Using dlg As New Windows.Forms.SaveFileDialog

                                        Dim extension_ As String = Nothing

                                        If _worksheettheme = WorksheetThemes.OliveWorld Then

                                            extension_ = "Excel Files (*.xlsx)|*.xlsx"
                                        Else
                                            extension_ = "Excel Files (*.xls)|*.xls"

                                        End If

                                        'Environ("USERPROFILE")
                                        'My.Application.Info.DirectoryPath
                                        With dlg
                                            .Filter = extension_
                                            .FilterIndex = 1
                                            '.InitialDirectory = Environ("USERPROFILE") & "\Desktop\"
                                            .InitialDirectory = "c:\logs\"
                                            .FileName = _ioperationscatalog.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & "-" & _ioperationscatalog.EspacioTrabajo.MisCredenciales.ClaveUsuario & "-" & _title

                                        End With


                                        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then

                                            savePath = dlg.FileName

                                            '_system.GsDialogo("El archivo fue guardado exitosamente en: [" & savePath & "]", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                                        Else

                                            savePath = Nothing

                                        End If

                                    End Using



                                    If Not savePath Is Nothing Then

                                        Dim stream As Stream = File.OpenWrite(savePath)  'La ruta guarda


                                        If _worksheettheme = WorksheetThemes.OliveWorld Then
                                            'Escribir
                                            _objExcelBookNPOI_WB.Write(stream)

                                        Else
                                            'Escribir
                                            _objExcelBookNPOI.Write(stream)

                                        End If

                                        'Cierra el flujo de datos
                                        stream.Close()

                                        ShellExecute(2, "open", savePath, "", "", CLng(4))

                                    End If


                                Catch ex As Exception

                                    savePath = Nothing

                                End Try


                            Else
                                _system.GsDialogo("Report can't be generated, please try again!")

                            End If

                        End If

                    Case IReportw.MakerModes.ThroughCollections

                        objHojaExcel_ = New Excel.Worksheet

                        Dim objWokSheet1_ As Excel.Worksheet

                        objWokSheet1_ = StartExcelWorkSheet()

                        If Not objWokSheet1_ Is Nothing Then

                            For Each cellvalue_ As DataPair In _reportCollections.GetCollectionGlobalAttributes.CollectionDataPairs.Values

                                SetCellValue(objWokSheet1_, cellvalue_.GetDataValue, cellvalue_.GetPositionXY)

                            Next

                            objWokSheet1_.Select()

                            If Not _reportpassword Is Nothing Then

                                objWokSheet1_.Protect(Password:=_reportpassword)

                            End If

                            If _pathsavefile <> "" Then

                                '_objExcelBook.SaveAs(_pathsavefile, Excel.XlFileFormat.xlWorkbookDefault)

                            End If

                        Else

                            MsgBox("Wasn't possible to create a new Excel Application!")

                        End If

                    Case Else

                End Select

                If Not _excelapplication Is Nothing Then

                    _excelapplication.Visible = True

                End If

            Catch ex As Exception

                Windows.Forms.MessageBox.Show(ex.Message)


                If Not _objExcelBook Is Nothing Then

                    '_objExcelBook.Close()

                    '_excelapplication.Quit()

                End If

            End Try

        End Sub


        Private Function StartExcelWorkSheet() As Excel.Worksheet
            Try

                Dim answer_ As Boolean = False

                'Dim objHojaExcel_ As New Excel.Worksheet

                '' Creamos un objeto WorkSheet


                'Crear una cultura standard (en-US) inglés estados unidos
                'System.Threading.Thread.CurrentThread.CurrentCulture = _
                'New System.Globalization.CultureInfo("en-US")



                '' Iniciamos una instancia a excel

                _excelapplication = New Excel.Application

                Dim _rutaConfigurada As String = _filetemplate

                _filetemplate = LCase(_filetemplate).Replace("c:\svn\svn qa", System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString())

                If Not System.IO.File.Exists(_filetemplate) Then

                    _filetemplate = _rutaConfigurada

                    'If Not System.IO.File.Exists(_filetemplate) Then

                    '    _dialogos.GsDialogo("Archivo no encontrado en contrado en la ruta indicada [" & _rutamoduloensamblado & "], por  favor reporte al administrador", GsDialogo.TipoDialogo.Alerta)

                    '    Exit Function

                    'End If

                End If

                If Not _excelapplication Is Nothing Then

                    If _filetemplate <> "" And _pathsavefile <> "" Then



                        _objExcelBook = _excelapplication.Workbooks.Open(_filetemplate)

                        objHojaExcel_ = _objExcelBook.Worksheets(1)

                    Else
                        'Optinal
                        '_excelapplication.ScreenUpdating = True ' TEMPORAL OK

                        _objExcelBook = _excelapplication.Workbooks.Add() 'TEMPORAL OK

                        objHojaExcel_ = _objExcelBook.Worksheets(1)

                    End If


                    objHojaExcel_.Visible = Excel.XlSheetVisibility.xlSheetVisible

                    'PBM123
                    '_excelapplication.Visible = True

                    '' Hacemos esta hoja la visible en pantalla 
                    '' (como seleccionamos la primera esto no es necesario
                    '' si seleccionamos una diferente a la primera si lo
                    '' necesitariamos).

                    'PBM123
                    'objHojaExcel_.Activate()


                End If


                Return objHojaExcel_

            Catch ex As Exception

                _excelapplication.Quit()

                Return Nothing

            End Try
        End Function

        Public Sub setReportPassword(ByVal password_ As String)

            _reportpassword = password_

        End Sub


        Private Function ReturnColumnAsExcelKnows(ByVal xposition_ As Int32) As StringBuilder
            Dim answer_ As StringBuilder = New StringBuilder

            _finalcolumnletter.Clear()

            If xposition_ <= 26 Then

                answer_.Append(ChrW(64 + xposition_))

                _finalcolumnid = answer_.ToString & xposition_

                _finalcolumnletter.Append(answer_)

                _listvisiblecolumns.Add(answer_)

            Else

                If xposition_ <= 52 Then
                    Dim auxilar_ As New StringBuilder

                    auxilar_.Append(ChrW(64 + (xposition_ - 26)))

                    answer_.Append("A" & auxilar_.ToString)

                    _finalcolumnid = answer_.ToString & xposition_

                    _finalcolumnletter.Append(answer_)

                    _listvisiblecolumns.Add(answer_)

                Else

                    If xposition_ > 52 And xposition_ <= 78 Then
                        Dim auxilar_ As New StringBuilder

                        auxilar_.Append(ChrW(64 + (xposition_ - 52)))

                        answer_.Append("B" & auxilar_.ToString)

                        _finalcolumnid = answer_.ToString & xposition_

                        _finalcolumnletter.Append(answer_)

                        _listvisiblecolumns.Add(answer_)

                    Else

                        _system.GsDialogo("Quantity of columns not supported")

                        ' _system.GsDialogo("Quantity of columns not supported")

                    End If

                End If
            End If

            Return answer_

        End Function

        Private Sub PrepareWorksheet(ByRef objLibroExcel As IWorkbook,
                                     ByRef objHojaExcel As ISheet)

            Dim row_ = objHojaExcel.CreateRow(0)

            Dim cell_ As HSSFCell = row_.CreateCell(0)

            cell_.SetCellValue(_title)


            Dim row2_ = objHojaExcel.CreateRow(1)

            row2_.CreateCell(0).SetCellValue(_subtitle)

            Dim row3_ = objHojaExcel.CreateRow(2)

            Dim companyName_ As String = Nothing

            Select Case _ioperationscatalog.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                Case 1 : companyName_ = "Grupo Reyes Kuri S.C. (Veracruz)"
                Case 2 : companyName_ = "Atlas Expeditors (Veracruz)"
                Case 3 : companyName_ = "Despachos Aereos Integrados (México)"
                Case 6 : companyName_ = "Comercio Exterior del Golfo (Altamira)"
                Case 7 : companyName_ = "Comercio Exterior del Golfo (Toluca)"
                Case 8 : companyName_ = "Servicios Aduanales del Pacífico (Manzanillo)"
                Case 9 : companyName_ = "Servicios Aduanales del Pacífico (Lázaro C.)"
                Case 113 : companyName_ = "Solium Forwarding Inc."
            End Select

            row3_.CreateCell(0).SetCellValue("Date:" & Now().ToString & ", Company:" & companyName_)

        End Sub

        Private Sub PrepareWorksheet(ByRef objLibroExcel As HSSFWorkbook,
                                     ByRef objHojaExcel As ISheet)



            Dim rowTitle_ = objHojaExcel.CreateRow(0)

            Dim rowSubtitle_ = objHojaExcel.CreateRow(1)

            Dim rowUser_ = objHojaExcel.CreateRow(2)

            Dim cellStyleTitle_ = objLibroExcel.CreateCellStyle()

            Dim styleCellBackGroundFontTitle_ = objLibroExcel.CreateFont()

            Dim cellStyleSubtitle_ = objLibroExcel.CreateCellStyle()

            Dim styleCellBackGroundFontSubtitle_ = objLibroExcel.CreateFont()

            Dim cellStyleSubOther_ = objLibroExcel.CreateCellStyle()

            Dim styleCellBackGroundFontOther_ = objLibroExcel.CreateFont()

            'Title
            SetStyleNPOI(0, 0, _title, StyleTypes.Title, FieldTypes.AsString, cellStyleTitle_, styleCellBackGroundFontTitle_, rowTitle_, objLibroExcel, objHojaExcel)
            'Subtitle
            SetStyleNPOI(1, 0, _subtitle, StyleTypes.Subtitle, FieldTypes.AsString, cellStyleSubtitle_, styleCellBackGroundFontSubtitle_, rowSubtitle_, objLibroExcel, objHojaExcel)

            'Dim row_ = objHojaExcel.CreateRow(0)
            'row_.CreateCell(0).SetCellValue(_title)

            'Dim row2_ = objHojaExcel.CreateRow(1)
            'row2_.CreateCell(0).SetCellValue(_subtitle)
            'Dim row3_ = objHojaExcel.CreateRow(2)

            Dim companyName_ As String = Nothing

            Select Case _ioperationscatalog.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                Case 1 : companyName_ = "Grupo Reyes Kuri S.C. (Veracruz)"
                Case 2 : companyName_ = "Atlas Expeditors (Veracruz)"
                Case 3 : companyName_ = "Despachos Aereos Integrados (México)"
                Case 6 : companyName_ = "Comercio Exterior del Golfo (Altamira)"
                Case 7 : companyName_ = "Comercio Exterior del Golfo (Toluca)"
                Case 8 : companyName_ = "Servicios Aduanales del Pacífico (Manzanillo)"
                Case 9 : companyName_ = "Servicios Aduanales del Pacífico (Lázaro C.)"
                Case 113 : companyName_ = "Solium Forwarding Inc."
            End Select

            'row3_.CreateCell(0).SetCellValue(companyName_)

            'Other
            SetStyleNPOI(2, 0, "Date:" & Now.ToString & ", " & companyName_, StyleTypes.User, FieldTypes.AsString, cellStyleSubOther_, styleCellBackGroundFontOther_, rowUser_, objLibroExcel, objHojaExcel)

        End Sub

        Private Sub PrepareWorksheet(ByRef objLibroExcel As Excel.Workbook, _
                                     ByRef objHojaExcel As Excel.Worksheet)

            '' Crear el encabezado de nuestro informe
            objHojaExcel.Range("A1:D1").Merge()
            objHojaExcel.Range("A1:D1").Value = _title
            objHojaExcel.Range("A1:D1").Font.Bold = True
            objHojaExcel.Range("A1:D1").Font.Size = 15

            '' Crear el subencabezado de nuestro informe
            objHojaExcel.Range("A2:D2").Merge()
            objHojaExcel.Range("A2:D2").Value = _subtitle
            objHojaExcel.Range("A2:D2").Font.Italic = True
            objHojaExcel.Range("A2:D2").Font.Size = 13


        End Sub

        Private Sub SetCellValue(ByRef objHojaExcel_ As Excel.Worksheet,
                                 ByVal value_ As String,
                                 ByVal position_ As String)

            Dim objCell_ As Excel.Range = objHojaExcel_.Range(position_, Type.Missing)
            objCell_.Value = value_

        End Sub

        Private Function CreateExcelColumns(ByRef objHojaExcelNPOI_ As ISheet, ByRef objLibroExcel As HSSFWorkbook) As Boolean
            Dim answer_ As Boolean = False
            'Dim column_ As New StringBuilder
            Dim itemctrl_ As Int32 = 0

            If Not _ioperationscatalog Is Nothing Then

                Dim xposition_ As Int32 = 3

                Dim row_ = objHojaExcelNPOI_.CreateRow(xposition_)

                Dim columnsStyle_ = objLibroExcel.CreateCellStyle()

                Dim styleCellBackGroundFont_ = objLibroExcel.CreateFont()

                For Each characteristic_ As ICaracteristica In IOperationsCatalog.Caracteristicas.Values

                    If characteristic_.Visible = ICaracteristica.TiposVisible.Si Or _
                        characteristic_.Visible = ICaracteristica.TiposVisible.Impresion Then
                        '"A3"
                        'column_ = ReturnColumnAsExcelKnows(xposition_)

                        'row_.CreateCell(itemctrl_).SetCellValue(characteristic_.NombreMostrar)

                        ' Dim row_ = objHojaExcel.CreateRow(3)



                        'Headers
                        SetStyleNPOI(xposition_, itemctrl_, characteristic_.NombreMostrar, StyleTypes.ColumnHeader, FieldTypes.AsUndefined, columnsStyle_, styleCellBackGroundFont_, row_, objLibroExcel, objHojaExcelNPOI_)







                        'Dim objCell_ As Excel.Range = objHojaExcel_.Range(column_.ToString & "3", Type.Missing)

                        'objCell_.Value = characteristic_.NombreMostrar

                        'Select Case characteristic_.TipoDato
                        '    Case ICaracteristica.TiposCaracteristica.cReal
                        '        objCell_.EntireColumn.NumberFormat = "###,###,###.00"
                        '    Case ICaracteristica.TiposCaracteristica.cInt32
                        '        objCell_.EntireColumn.NumberFormat = "###,###,###"

                        'End Select

                        Dim item_ As New StringBuilder

                        item_.Append(characteristic_.NombreMostrar)

                        _listvisiblecolumnsNames.Add(itemctrl_, item_)

                        itemctrl_ += 1

                    End If

                Next

            Else
                _system.GsDialogo("Catalogs are not loaded yet, please Verify!")

            End If

            answer_ = True

            Return answer_

        End Function

        Private Function CreateExcelColumns(ByRef objHojaExcel_ As Excel.Worksheet) As Boolean
            Dim answer_ As Boolean = False
            Dim column_ As New StringBuilder
            Dim itemctrl_ As Int32 = 0

            If Not _ioperationscatalog Is Nothing Then
                Dim xposition_ As Int32 = 1

                For Each characteristic_ As ICaracteristica In IOperationsCatalog.Caracteristicas.Values



                    If characteristic_.Visible = ICaracteristica.TiposVisible.Si Or _
                        characteristic_.Visible = ICaracteristica.TiposVisible.Impresion Then
                        '"A3"
                        column_ = ReturnColumnAsExcelKnows(xposition_)

                        If Not column_ Is Nothing Then

                            Dim objCell_ As Excel.Range = objHojaExcel_.Range(column_.ToString & "3", Type.Missing)
                            objCell_.Value = characteristic_.NombreMostrar


                            Select Case characteristic_.TipoDato
                                Case ICaracteristica.TiposCaracteristica.cReal
                                    objCell_.EntireColumn.NumberFormat = "###,###,###.00"
                                Case ICaracteristica.TiposCaracteristica.cInt32
                                    objCell_.EntireColumn.NumberFormat = "###,###,###"

                                Case ICaracteristica.TiposCaracteristica.cDateTime
                                    'objCell_.EntireColumn.NumberFormat = "###,###,###.00"
                                Case ICaracteristica.TiposCaracteristica.cString
                                    'objCell_.EntireColumn.NumberFormat = "###,###,###.00"
                            End Select

                            Dim item_ As New StringBuilder
                            item_.Append(characteristic_.NombreMostrar)
                            _listvisiblecolumnsNames.Add(itemctrl_, item_)

                            itemctrl_ += 1

                        Else
                            Continue For
                        End If

                        xposition_ += 1

                    End If


                Next

            Else
                _system.GsDialogo("Catalogs are not loaded yet, please Verify!")

            End If

            answer_ = True

            Return answer_
        End Function

        Private Function CreateContentsTEMP(ByRef objDataSet As DataSet, _
                                   ByRef objWorksheetExcel_ As Excel.Worksheet
                                  ) As Boolean
            Dim answer_ As Boolean = False
            Dim CategoryName As String                  ' Variable para controlar la ruptura por nombre de categoria

            '' Creamos una variable para gualdar la cultura actual
            Dim OldCultureInfo As System.Globalization.CultureInfo = _
                                  System.Threading.Thread.CurrentThread.CurrentCulture

            Dim i As Integer = 5
            Dim j As Integer = 5

            CategoryName = ""

            For Each objRow As DataRow In objDataSet.Tables(0).Rows

                ' Si Ya se ha impreso una categoria y la proxima categoria 
                ' es diferente a la categoria a imprimir, imprimir los totales
                If CategoryName.Length > 0 AndAlso CategoryName <> objRow.Item(0) Then
                    objWorksheetExcel_.Cells(i, 1) = "Total Category " & CategoryName.Trim
                    objWorksheetExcel_.Cells(i, 2) = "=count(D" & (j + 2).ToString & ":D" & (i - 1).ToString & ")"

                    objWorksheetExcel_.Cells(i, 3) = "Subtotal Precio:"
                    objWorksheetExcel_.Cells(i, 4) = "=sum(D" & (j + 2).ToString & ":D" & (i - 1).ToString & ")"
                    objWorksheetExcel_.Range("A" & i.ToString & ":D" & i.ToString).Font.Bold = True

                    j = i
                    i += 2
                End If

                '' Asignar la categoria impresa
                CategoryName = objRow.Item(0)

                '' Asignar los valores de los registros a las celdas
                objWorksheetExcel_.Cells(i, "A") = objRow.Item(0) 'CategoryName'
                objWorksheetExcel_.Cells(i, "B") = objRow.Item(1) 'ProductID'
                objWorksheetExcel_.Cells(i, "C") = objRow.Item(2) 'ProductName'
                objWorksheetExcel_.Cells(i, "D") = objRow.Item(3) 'UnitPrice'

                '' Avanzamos una fila
                i += 1
            Next

            '* El Ultimo subtotal
            objWorksheetExcel_.Cells(i, "A") = "Total Category " & CategoryName.Trim
            objWorksheetExcel_.Cells(i, "B") = "=count(D" & (j + 2).ToString & ":D" & (i - 1).ToString & ")"

            objWorksheetExcel_.Cells(i, "C") = "Subtotal Precio:"
            objWorksheetExcel_.Cells(i, "D") = "=sum(D" & (j + 2).ToString & ":D" & (i - 1).ToString & ")"
            objWorksheetExcel_.Range("A" & i.ToString & ":D" & i.ToString).Font.Bold = True

            '' Avanzamos una fila
            i += 1

            '' Seleccionar todo el bloque desde A1 hasta D #de filas
            Dim objRango As Excel.Range = objWorksheetExcel_.Range("A3:D" & (i - 1).ToString)

            '' Selecionado todo el rango especificado
            objRango.Select()

            '' Ajustamos el ancho de las columnas al ancho máximo del
            '' contenido de sus celdas
            objRango.Columns.AutoFit()

            '' Asignar filtro por columna
            objRango.AutoFilter(1, , VisibleDropDown:=True)

            '' Asignar un formato automatico
            objRango.AutoFormat(11, Alignment:=False)

            '' Seleccionamos el total general del reporte y asignamos
            '' font a negrita e italica
            objRango = objWorksheetExcel_.Range("A" & i.ToString & ":D" & i.ToString)
            objRango.Select()
            objRango.Font.Bold = True
            objRango.Font.Italic = True

            '' Crear un total general
            objWorksheetExcel_.Cells(i, 1) = "Total "
            objWorksheetExcel_.Cells(i, 2) = "=count(A3:D" & (i - 1).ToString & ")"

            objWorksheetExcel_.Cells(i, 3) = "Total Precio:"
            objWorksheetExcel_.Cells(i, 4) = "=sum(A3:D" & (i - 1).ToString & ")"

            '' Crear un grafico estadistico
            Dim objExcelChart As Excel.Chart
            Dim xlsSeries As Excel.SeriesCollection
            Dim xlsAxisCategory, xlsAxisValue As Excel.Axes

            '' Agregamos un nuevo grafico
            objExcelChart = objWorksheetExcel_.Charts.Add

            '' Crearmos un rango con los totales de cada categoria para crear nuestro gráfico
            objRango = objWorksheetExcel_.Range("=Sheet1!$D$17,Sheet1!$D$31,Sheet1!$D$46,Sheet1!$D$58,Sheet1!$D$67,Sheet1!$D$75,Sheet1!$D$82,Sheet1!$D$96")

            With objExcelChart
                '' Asignamos el tipo de grafico
                .ChartType = Excel.XlChartType.xlColumnClustered

                '' Asignamos el total
                .SetSourceData(objRango)

                '' Seleccionamos los diferentes elementos del grafico
                xlsSeries = .SeriesCollection

                '' Desplegar los valores de cada columna al top de cada columna
                .ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowValue)

                '' Asignamos el nombre de cada serie
                xlsSeries.Item(1).Name = objWorksheetExcel_.Range("A17").Value
                xlsSeries.Item(2).Name = objWorksheetExcel_.Range("A31").Value
                xlsSeries.Item(3).Name = objWorksheetExcel_.Range("A46").Value
                xlsSeries.Item(4).Name = objWorksheetExcel_.Range("A58").Value
                xlsSeries.Item(5).Name = objWorksheetExcel_.Range("A67").Value
                xlsSeries.Item(6).Name = objWorksheetExcel_.Range("A75").Value
                xlsSeries.Item(7).Name = objWorksheetExcel_.Range("A82").Value
                xlsSeries.Item(8).Name = objWorksheetExcel_.Range("A96").Value

                xlsAxisCategory = .Axes(, Excel.XlAxisGroup.xlPrimary)
                xlsAxisCategory.Item(Excel.XlAxisType.xlCategory).HasTitle = True
                xlsAxisCategory.Item(Excel.XlAxisType.xlCategory).AxisTitle.Characters.Text = "Totales por categoria"

                xlsAxisValue = .Axes(, Excel.XlAxisGroup.xlPrimary)
                xlsAxisValue.Item(Excel.XlAxisType.xlValue).HasTitle = True
                xlsAxisValue.Item(Excel.XlAxisType.xlValue).AxisTitle.Characters.Text = "Rango de Precios"
                xlsAxisValue.Item(Excel.XlAxisType.xlValue).AxisTitle.Orientation = Excel.XlOrientation.xlVertical

                xlsAxisValue.Item(Excel.XlAxisType.xlValue).HasMajorGridlines = True

            End With

            System.Threading.Thread.CurrentThread.CurrentCulture = OldCultureInfo

            'objLibroExcel.PrintPreview()

            'objWorksheetExcel_.PrintPreview()


            'objWorksheetExcel_ = Nothing

            'bjWorksheetExcel_ = Nothing

            answer_ = True

            Return answer_
        End Function

        Private Function CreateContents(ByRef objDataSet As DataSet, _
                                   ByRef objWorksheetExcel_ As ISheet, _
                                    ByRef objExcelWorkBook As HSSFWorkbook) As Boolean
            Dim answer_ As Boolean = False

            Dim rowIndex_ As Int32 = 4

            'Style for integer

            Dim cellStyleContentInteger_ = objExcelWorkBook.CreateCellStyle()

            Dim styleCellBackGroundFontContentInteger_ = objExcelWorkBook.CreateFont()

            styleCellBackGroundFontContentInteger_.FontName = HSSFFont.FONT_ARIAL

            styleCellBackGroundFontContentInteger_.Boldweight = FontBoldWeight.Normal

            styleCellBackGroundFontContentInteger_.Color = HSSFColor.DarkGreen.Index

            styleCellBackGroundFontContentInteger_.FontHeightInPoints = 8

            cellStyleContentInteger_.DataFormat = objExcelWorkBook.CreateDataFormat().GetFormat("#,##0")

            cellStyleContentInteger_.SetFont(styleCellBackGroundFontContentInteger_)

            'Style for double, real, float

            Dim cellStyleContentDouble_ = objExcelWorkBook.CreateCellStyle()

            Dim styleCellBackGroundFontContentDouble_ = objExcelWorkBook.CreateFont()

            styleCellBackGroundFontContentDouble_.FontName = HSSFFont.FONT_ARIAL

            styleCellBackGroundFontContentDouble_.Boldweight = FontBoldWeight.Normal

            styleCellBackGroundFontContentDouble_.Color = HSSFColor.Green.Index

            styleCellBackGroundFontContentDouble_.FontHeightInPoints = 8

            cellStyleContentDouble_.DataFormat = objExcelWorkBook.CreateDataFormat().GetFormat("#,##0.##")

            cellStyleContentDouble_.SetFont(styleCellBackGroundFontContentDouble_)

            'Style for date, real, float

            Dim cellStyleContentDate_ = objExcelWorkBook.CreateCellStyle()

            Dim styleCellBackGroundFontContentDate_ = objExcelWorkBook.CreateFont()

            styleCellBackGroundFontContentDate_.FontName = HSSFFont.FONT_ARIAL

            styleCellBackGroundFontContentDate_.Boldweight = FontBoldWeight.Normal

            styleCellBackGroundFontContentDate_.Color = HSSFColor.Blue.Index

            styleCellBackGroundFontContentDate_.FontHeightInPoints = 8

            cellStyleContentDate_.DataFormat = objExcelWorkBook.CreateDataFormat().GetFormat("dd/mm/yyyy")

            cellStyleContentDate_.SetFont(styleCellBackGroundFontContentDate_)


            'Style for string, boolean

            Dim cellStyleContentString_ = objExcelWorkBook.CreateCellStyle()

            Dim styleCellBackGroundFontContentString_ = objExcelWorkBook.CreateFont()

            styleCellBackGroundFontContentString_.FontName = HSSFFont.FONT_ARIAL

            styleCellBackGroundFontContentString_.Boldweight = FontBoldWeight.Normal

            styleCellBackGroundFontContentString_.Color = HSSFColor.Grey80Percent.Index

            styleCellBackGroundFontContentString_.FontHeightInPoints = 8

            cellStyleContentString_.SetFont(styleCellBackGroundFontContentString_)

            For Each objRow As DataRow In _ioperationscatalog.Vista.Tables(0).Rows

                Dim row_ = objWorksheetExcel_.CreateRow(rowIndex_)

                Dim indiceLista_ As Int32 = 0

                Dim pair_ As KeyValuePair(Of Integer, StringBuilder)

                Dim column_ As Int32 = 0

                For Each pair_ In _listvisiblecolumnsNames

                    Dim columnTypeChar_ As FieldTypes = FieldTypes.AsUndefined

                    For Each characteristic_ As ICaracteristica In IOperationsCatalog.Caracteristicas.Values

                        If Not characteristic_.NombreMostrar = pair_.Value.ToString Then

                            Continue For

                        Else

                            Select Case characteristic_.TipoDato

                                Case ICaracteristica.TiposCaracteristica.cBoolean, ICaracteristica.TiposCaracteristica.cString

                                    columnTypeChar_ = FieldTypes.AsString

                                    SetStyleNPOI(rowIndex_, column_, objRow.Item(pair_.Value.ToString).ToString, StyleTypes.NormalRow, columnTypeChar_, cellStyleContentString_, styleCellBackGroundFontContentString_, row_, objExcelWorkBook, objWorksheetExcel_)

                                Case ICaracteristica.TiposCaracteristica.cDateTime

                                    columnTypeChar_ = FieldTypes.AsDate

                                    SetStyleNPOI(rowIndex_, column_, objRow.Item(pair_.Value.ToString).ToString, StyleTypes.NormalRow, columnTypeChar_, cellStyleContentDate_, styleCellBackGroundFontContentDate_, row_, objExcelWorkBook, objWorksheetExcel_)

                                Case ICaracteristica.TiposCaracteristica.cInt32

                                    columnTypeChar_ = FieldTypes.AsInteger

                                    SetStyleNPOI(rowIndex_, column_, objRow.Item(pair_.Value.ToString).ToString, StyleTypes.NormalRow, columnTypeChar_, cellStyleContentInteger_, styleCellBackGroundFontContentInteger_, row_, objExcelWorkBook, objWorksheetExcel_)

                                Case ICaracteristica.TiposCaracteristica.cReal

                                    columnTypeChar_ = FieldTypes.AsDouble

                                    SetStyleNPOI(rowIndex_, column_, objRow.Item(pair_.Value.ToString).ToString, StyleTypes.NormalRow, columnTypeChar_, cellStyleContentDouble_, styleCellBackGroundFontContentDouble_, row_, objExcelWorkBook, objWorksheetExcel_)

                                Case Else

                                    columnTypeChar_ = FieldTypes.AsString

                                    SetStyleNPOI(rowIndex_, column_, objRow.Item(pair_.Value.ToString).ToString, StyleTypes.NormalRow, columnTypeChar_, cellStyleContentString_, styleCellBackGroundFontContentString_, row_, objExcelWorkBook, objWorksheetExcel_)

                            End Select

                            Exit For

                        End If

                    Next

                    column_ += 1

                    indiceLista_ += 1

                Next

                indiceLista_ = 0

                rowIndex_ += 1

            Next

            Dim pair2_ As KeyValuePair(Of Integer, StringBuilder)

            For Each pair2_ In _listvisiblecolumnsNames

                If pair2_.Key > 1 Then

                    objWorksheetExcel_.AutoSizeColumn(pair2_.Key)

                Else

                    objWorksheetExcel_.SetColumnWidth(0, 800 * 4)

                End If

            Next


            answer_ = True

            Return answer_

        End Function

        Private Function CreateContents(ByRef objDataSet As DataSet, _
                                   ByVal objWorksheetExcel_ As Excel.Worksheet) As Boolean
            Dim answer_ As Boolean = False


            Dim i As Integer = 4

            Dim k_ As Int32 = 0

            For Each objRow As DataRow In _ioperationscatalog.Vista.Tables(0).Rows
                '   
                For Each visiblecolumn_ As StringBuilder In _listvisiblecolumns

                    objWorksheetExcel_.Cells(i, visiblecolumn_.ToString) = objRow.Item(_listvisiblecolumnsNames.Item(k_).ToString())

                    k_ += 1
                Next

                k_ = 0

                i += 1
            Next
            '' Avanzamos una fila
            i += 1

            '' Seleccionar todo el bloque desde A1 hasta D #de filas
            'Dim objRango As Excel.Range = objWorksheetExcel_.Range("A3:D" & (i - 1).ToString)
            Dim objRango_ As Excel.Range = objWorksheetExcel_.Range("A3:" & _finalcolumnletter.ToString & (i - 1).ToString)
            '

            '' Selecionado todo el rango especificado
            objRango_.Select()

            '' Ajustamos el ancho de las columnas al ancho máximo del
            '' contenido de sus celdas
            objRango_.Columns.AutoFit()

            '' Asignar filtro por columna
            objRango_.AutoFilter(1, , VisibleDropDown:=True)

            '' Asignar un formato automatico
            If Not _worksheettheme = WorksheetThemes.Undefined Then

                objRango_.AutoFormat(_worksheettheme, Alignment:=False)

            End If

            answer_ = True

            Return answer_

        End Function

        Private Function CreateAutomaticTotals(ByRef objRango_ As Excel.Range, _
                                     ByRef objWorksheetExcel_ As Excel.Worksheet, _
                                     ByVal position_ As Int32) As Boolean
            Dim answer_ As Boolean = False

            Try

                '' Crear un total general
                objWorksheetExcel_.Cells(position_, 1) = "Total "
                'objWorksheetExcel_.Cells(i, 2) = "=count(A3:D" & (i - 1).ToString & ")"
                objWorksheetExcel_.Cells(position_, 2) = "=count(A3:" & _finalcolumnletter.ToString & (position_ - 1).ToString & ")"

                objWorksheetExcel_.Cells(position_, 3) = "Total Precio:"
                'objWorksheetExcel_.Cells(i, 4) = "=sum(A3:D" & (i - 1).ToString & ")"
                objWorksheetExcel_.Cells(position_, 4) = "=sum(A3:" & _finalcolumnletter.ToString & (position_ - 1).ToString & ")"

            Catch ex As Exception

                answer_ = False

            End Try

            Return answer_
        End Function

        Private Function VerifyElementsForTotals() As Boolean

            Return False

        End Function

        Private Function VerifyElementForChart() As Boolean

            Return False

        End Function

        Private Function CreateChart(ByRef objRango_ As Excel.Range, _
                                     ByRef objWorksheetExcel_ As Excel.Worksheet) As Boolean
            Dim answer_ As Boolean = False

            Try

                '' Crear un grafico estadistico
                Dim objExcelChart As Excel.Chart
                Dim xlsSeries As Excel.SeriesCollection
                Dim xlsAxisCategory, xlsAxisValue As Excel.Axes

                '' Agregamos un nuevo grafico
                objExcelChart = objWorksheetExcel_.Charts.Add

                '' Crearmos un rango con los totales de cada categoria para crear nuestro gráfico
                objRango_ = objWorksheetExcel_.Range("=Sheet1!$D$17,Sheet1!$D$31,Sheet1!$D$46,Sheet1!$D$58,Sheet1!$D$67,Sheet1!$D$75,Sheet1!$D$82,Sheet1!$D$96")

                With objExcelChart
                    '' Asignamos el tipo de grafico
                    .ChartType = Excel.XlChartType.xlColumnClustered

                    '' Asignamos el total
                    .SetSourceData(objRango_)

                    '' Seleccionamos los diferentes elementos del grafico
                    xlsSeries = .SeriesCollection

                    '' Desplegar los valores de cada columna al top de cada columna
                    .ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowValue)

                    '' Asignamos el nombre de cada serie
                    xlsSeries.Item(1).Name = objWorksheetExcel_.Range("A17").Value
                    xlsSeries.Item(2).Name = objWorksheetExcel_.Range("A31").Value
                    xlsSeries.Item(3).Name = objWorksheetExcel_.Range("A46").Value
                    xlsSeries.Item(4).Name = objWorksheetExcel_.Range("A58").Value
                    xlsSeries.Item(5).Name = objWorksheetExcel_.Range("A67").Value
                    xlsSeries.Item(6).Name = objWorksheetExcel_.Range("A75").Value
                    xlsSeries.Item(7).Name = objWorksheetExcel_.Range("A82").Value
                    xlsSeries.Item(8).Name = objWorksheetExcel_.Range("A96").Value

                    xlsAxisCategory = .Axes(, Excel.XlAxisGroup.xlPrimary)
                    xlsAxisCategory.Item(Excel.XlAxisType.xlCategory).HasTitle = True
                    xlsAxisCategory.Item(Excel.XlAxisType.xlCategory).AxisTitle.Characters.Text = "Totales por categoria"

                    xlsAxisValue = .Axes(, Excel.XlAxisGroup.xlPrimary)
                    xlsAxisValue.Item(Excel.XlAxisType.xlValue).HasTitle = True
                    xlsAxisValue.Item(Excel.XlAxisType.xlValue).AxisTitle.Characters.Text = "Rango de Precios"
                    xlsAxisValue.Item(Excel.XlAxisType.xlValue).AxisTitle.Orientation = Excel.XlOrientation.xlVertical

                    xlsAxisValue.Item(Excel.XlAxisType.xlValue).HasMajorGridlines = True

                End With


                answer_ = True

            Catch ex As Exception
                answer_ = False

            End Try

            Return answer_

        End Function

        Public Sub CreateReportCSV()

            Try

                _objCSVReport = New StringBuilder()

                If _sistema.TieneResultados(_ioperationscatalog) Then

                    _objCSVReport.Append(_ioperationscatalog.Nombre)

                    _objCSVReport.Append(Environment.NewLine)

                    _objCSVReport.Append(_subtitle)

                    _objCSVReport.Append(Environment.NewLine)

                    Dim companyName_ As String = Nothing

                    Select Case _ioperationscatalog.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                        Case 1 : companyName_ = "Grupo Reyes Kuri S.C. (Veracruz)"
                        Case 2 : companyName_ = "Atlas Expeditors (Veracruz)"
                        Case 3 : companyName_ = "Despachos Aereos Integrados (México)"
                        Case 6 : companyName_ = "Comercio Exterior del Golfo (Altamira)"
                        Case 7 : companyName_ = "Comercio Exterior del Golfo (Toluca)"
                        Case 8 : companyName_ = "Servicios Aduanales del Pacífico (Manzanillo)"
                        Case 9 : companyName_ = "Servicios Aduanales del Pacífico (Lázaro C.)"
                        Case 113 : companyName_ = "Solium Forwarding Inc."
                    End Select

                    _objCSVReport.Append("Date:" & Now().ToString & "  Company:" & companyName_)

                    _objCSVReport.Append(Environment.NewLine)

                    For Each column_ As DataColumn In _ioperationscatalog.Vista.Tables(0).Columns

                        _objCSVReport.Append(column_.ColumnName & ",")

                    Next

                    _objCSVReport.Remove(_objCSVReport.Length - 1, 1)

                    _objCSVReport.Append(Environment.NewLine)

                    For Each row_ As DataRow In _ioperationscatalog.Vista.Tables(0).Rows

                        For i_ As Integer = 0 To _ioperationscatalog.Vista.Tables(0).Columns.Count - 1

                            _objCSVReport.Append(row_(i_).ToString().Replace(",", " ").Replace("\r", "").Replace("\a", "").Replace("\t", "").Replace("\n", "") & ",")

                        Next

                        _objCSVReport.Append(Environment.NewLine)

                    Next

                    If Not _reportpassword Is Nothing Then

                    End If

                    Dim savePath As String = Nothing

                    Try

                        Using dlg As New Windows.Forms.SaveFileDialog

                            Dim extension_ As String = Nothing

                            extension_ = "CSV (MS-DOS) (*.csv)|*.csv"

                            With dlg
                                .Filter = extension_
                                .FilterIndex = 1
                                .InitialDirectory = "c:\logs\"
                                .FileName = _ioperationscatalog.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & "-" & _ioperationscatalog.EspacioTrabajo.MisCredenciales.ClaveUsuario & "-" & _title

                            End With


                            If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then

                                savePath = dlg.FileName

                                '_system.GsDialogo("El archivo fue guardado exitosamente en: [" & savePath & "]", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                            Else

                                savePath = Nothing

                            End If

                        End Using

                        If Not savePath Is Nothing Then

                            File.WriteAllText(savePath, _objCSVReport.ToString())

                            ShellExecute(2, "open", savePath, "", "", CLng(4))

                        End If

                    Catch ex As Exception

                        savePath = Nothing

                    End Try

                    If Not _excelapplication Is Nothing Then

                        _excelapplication.Visible = True

                    End If

                End If

            Catch ex As Exception

                Windows.Forms.MessageBox.Show(ex.Message)

            End Try

        End Sub


#Region "IDisposable Support"
        Private disposedValue As Boolean ' Para detectar llamadas redundantes

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: eliminar estado administrado (objetos administrados).
                End If

                '_objExcelBook.Close()

                If Not _excelapplication Is Nothing Then
                    _excelapplication.Quit()
                End If


                GC.Collect()
                GC.WaitForPendingFinalizers()

                ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                ' TODO: Establecer campos grandes como Null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
        'Protected Overrides Sub Finalize()
        '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).


            Dispose(True)
            GC.SuppressFinalize(Me)


        End Sub
#End Region

#End Region

    End Class


    Public Class ReportCollections

#Region "Structures"

        Public Structure DataPairCollecion

            Public RegionName As String

            Public CollectionDataPairs As Dictionary(Of Int32, DataPair)

        End Structure

#End Region

#Region "Attributes"

        'Private _collectionGlobalAttributes As Dictionary(Of Int32, DataPair)

        Private _collectionGlobalAttributes As New DataPairCollecion

        Private _stackCollections As Dictionary(Of Int32, DataPairCollecion)


#End Region

#Region "Builders"

        Sub New()

            _collectionGlobalAttributes = New DataPairCollecion

            _collectionGlobalAttributes.RegionName = Nothing

            _collectionGlobalAttributes.CollectionDataPairs = New Dictionary(Of Int32, DataPair)


            _stackCollections = New Dictionary(Of Int32, DataPairCollecion)

        End Sub

#End Region

#Region "Properties"

        ReadOnly Property GetCollectionGlobalAttributes As DataPairCollecion
            Get
                Return _collectionGlobalAttributes
            End Get
        End Property


#End Region

#Region "Methods"

        Public Function SetDataValueGlobalAttributes(ByVal index_ As Int32,
                                                ByVal datavalue_ As String,
                                                ByVal positionxy_ As String) As Boolean
            Dim answer_ As Boolean = False
            Dim datapair_ As New DataPair(datavalue_, positionxy_)

            '_collectionGlobalAttributes.CollectionDataPairs.Add(index_,
            '                                                    datapair_)
            'answer_ = True


            ' If Not _collectionGlobalAttributes.CollectionDataPairs Is Nothing And _
            '      _collectionGlobalAttributes.CollectionDataPairs.Count >= 1 Then
            '
            If Not _collectionGlobalAttributes.CollectionDataPairs.ContainsKey(index_) Then

                _collectionGlobalAttributes.CollectionDataPairs.Add(index_,
                                                                     datapair_)
                answer_ = True
            Else

                MsgBox("Index '" & index_ & "' is not valid, exists in current collection!")

            End If

            'End If

            Return answer_
        End Function

#End Region

    End Class


    Public Class DataPair

#Region "Attributes"

        Private _positionXY As String

        Private _dataValue As String

#End Region

#Region "Builders"

        Sub New()

            _positionXY = Nothing

            _dataValue = Nothing

        End Sub

        Sub New(ByVal datavalue_ As String,
                ByVal positionxy_ As String)

            _positionXY = positionxy_

            _dataValue = datavalue_

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property GetPositionXY As String
            Get
                Return _positionXY
            End Get
        End Property

        Public WriteOnly Property SetPositionXY As String
            Set(value As String)
                _positionXY = value
            End Set
        End Property

        Public ReadOnly Property GetDataValue As String
            Get
                Return _dataValue
            End Get
        End Property

        Public WriteOnly Property SetDataValue As String
            Set(value As String)
                _dataValue = value
            End Set
        End Property

#End Region

    End Class




End Namespace

