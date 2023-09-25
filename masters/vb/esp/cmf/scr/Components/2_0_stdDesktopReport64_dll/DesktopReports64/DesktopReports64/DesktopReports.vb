Imports Microsoft.Office.Interop
Imports gsol.BaseDatos.Operaciones
Imports gsol
Imports System.Threading

Namespace Wma.Reports


    Public Class DesktopReports
        Implements IReportw

#Region "Attributes"

        Private _fileextension As IReportw.Extensions

        Private _filename As String

        Private _filepath As String

        Private _makermode As IReportw.MakerModes

        Private _workspace As IEspacioTrabajo

        Private _reportobject As Object

        'Objects/Reports
        Private _excelreport As ExcelApplicationReports

#End Region

#Region "Builders"

        Sub New()

            _fileextension = IReportw.Extensions.xls

            _filename = "NONAMED"

            _filepath = "%temp%\"

            _makermode = IReportw.MakerModes.ThroughPackageLaunchPreview

            _workspace = New EspacioTrabajo

            _excelreport = New ExcelApplicationReports

            _reportobject = New Object

        End Sub

#End Region

#Region "Properties"

        Public Property GuardarReporte As Boolean Implements IReportw.GuardarReporte

            Get

                Return _excelreport.GuardarReporte

            End Get

            Set(ByVal value As Boolean)

                _excelreport.GuardarReporte = value

            End Set

        End Property

        Public Property CerrarReporte As Boolean Implements IReportw.CerrarReporte

            Get

                Return _excelreport.CerrarReporte

            End Get

            Set(ByVal value As Boolean)

                _excelreport.CerrarReporte = value

            End Set

        End Property

        Public Property VisualizarReporte() As Boolean Implements IReportw.VisualizarReporte

            Get

                Return _excelreport.VisualizarReporte

            End Get

            Set(ByVal value As Boolean)

                _excelreport.VisualizarReporte = value

            End Set

        End Property

        Public WriteOnly Property setPassword As String _
            Implements IReportw.setPassword

            Set(value As String)

                _excelreport.setReportPassword(value)

            End Set

        End Property

        Public Property PathTemplate As String _
            Implements IReportw.PathTemplate

            Get
                Return _excelreport.FileTemplate
            End Get

            Set(value As String)
                _excelreport.FileTemplate = value
            End Set
        End Property

        'MasterOfPuppets
        Public WriteOnly Property setReportTheme As ExcelApplicationReports.WorksheetThemes _
            Implements IReportw.setReportTheme

            Set(value As ExcelApplicationReports.WorksheetThemes)

                _excelreport.setWorksheetTheme = value

            End Set

        End Property

        Public Property FileExtension As IReportw.Extensions _
            Implements IReportw.FileExtension
            Get
                Return _fileextension
            End Get
            Set(value As IReportw.Extensions)
                _fileextension = value
            End Set
        End Property

        Public Property FileName As String _
            Implements IReportw.FileName
            Get
                Return _filename
            End Get
            Set(value As String)
                _filename = value
            End Set
        End Property

        Public Property FilePath As String _
            Implements IReportw.FilePath
            Get
                Return _excelreport.SaveFileIn
            End Get
            Set(value As String)
                _excelreport.SaveFileIn = value
            End Set
        End Property

        Public ReadOnly Property getReportObject As Object _
            Implements IReportw.getReportObject
            Get
                Return _reportobject
            End Get
        End Property

        Public Property MakerMode As IReportw.MakerModes _
            Implements IReportw.MakerMode
            Get
                Return _makermode
            End Get
            Set(value As IReportw.MakerModes)
                _makermode = value
                _excelreport.MakerMode = value
            End Set
        End Property

        Public Property PackageIOperations As gsol.BaseDatos.Operaciones.IOperacionesCatalogo _
            Implements IReportw.PackageIOperations
            Get
                Return _excelreport.IOperationsCatalog
            End Get
            Set(value As gsol.BaseDatos.Operaciones.IOperacionesCatalogo)
                _excelreport.IOperationsCatalog = value

            End Set
        End Property

        Public WriteOnly Property setWorkSpace As gsol.IEspacioTrabajo _
            Implements IReportw.setWorkSpace
            Set(value As gsol.IEspacioTrabajo)
                ' _workspace = value
            End Set
        End Property

#End Region

#Region "Methods"

        Public Sub ShowPreviewReport()

        End Sub

        Public Sub GenerateReport() _
            Implements IReportw.GenerateReport

            Dim trd As Thread

            If (IsResourcesReady()) Then

                'Verifing supported reports
                Select Case _fileextension
                    Case IReportw.Extensions.xls, IReportw.Extensions.xlsx

                        'Verify type mode to generate report
                        Select Case _makermode

                            Case IReportw.MakerModes.ThroughPackage

                                'trd = New Thread(AddressOf _excelreport.CreateReport)
                                'trd.IsBackground = True

                                'trd.Start()
                                'trd.Join()

                                _excelreport.CreateReport()

                                _reportobject = _excelreport.getExcelBookNPOI

                                '_excelreport.Dispose()

                                'trd.Abort()

                                'Case IReportw.MakerModes.ThroughPackage

                                '    trd = New Thread(AddressOf _excelreport.CreateReport)
                                '    trd.IsBackground = True

                                '    trd.Start()
                                '    trd.Join()

                                '    _reportobject = _excelreport.getExcelBook

                                '    _excelreport.Dispose()

                                '    trd.Abort()


                            Case IReportw.MakerModes.ThroughCollections

                                trd = New Thread(AddressOf _excelreport.CreateReport)
                                trd.IsBackground = True

                                trd.Start()
                                trd.Join()

                                '_excelreport.CreateReport()

                                _reportobject = _excelreport.getExcelBook

                                '_excelreport.Dispose()
                                trd.Abort()

                            Case Else

                        End Select


                    Case IReportw.Extensions.csv

                        _excelreport.CreateReportCSV()

                        _reportobject = _excelreport.getCSVReport

                    Case Else

                        MsgBox("Not implemented yet!")

                End Select

            End If

        End Sub

        Private Function IsResourcesReady() As Boolean
            Dim answer_ As Boolean = False

            Select Case _fileextension
                Case IReportw.Extensions.xls, IReportw.Extensions.xlsx
                    If (_fileextension = IReportw.Extensions.xls Or _fileextension = IReportw.Extensions.xlsx) And _
                        Not (_makermode = IReportw.MakerModes.Manual) Then

                        Select Case _makermode
                            Case IReportw.MakerModes.Manual
                                MsgBox("Mode currently Not implemented")
                            Case IReportw.MakerModes.ThroughCollections

                                If _excelreport.GetReportCollections.GetCollectionGlobalAttributes.CollectionDataPairs.Count >= 1 Then
                                    answer_ = True
                                End If

                            Case IReportw.MakerModes.ThroughPackage
                                If PackageIOperations.Caracteristicas.Count >= 0 Then
                                    answer_ = True
                                End If
                            Case IReportw.MakerModes.ThroughPackageLaunchPreview
                                MsgBox("Mode currently Not implemented")
                        End Select

                    End If

                Case IReportw.Extensions.csv
                    If PackageIOperations.Caracteristicas.Count >= 0 Then
                        answer_ = True
                    End If

                Case Else
                    MsgBox("Currently Not supported")

            End Select

            Return answer_
        End Function

        Public Function AddFullRowsBellowOf(ByVal positionxy_ As String,
                                            ByVal quantity_ As Integer) As Boolean _
        Implements IReportw.AddFullRowsBellowOf
            Dim answer_ As Boolean = False

            'NOT IMPLEMENTED

            Return answer_
        End Function

        Public Function SetDataValueGlobalAttributes(ByVal index_ As Integer,
                                                     ByVal datavalue_ As String,
                                                     ByVal positionxy_ As String) As Boolean _
        Implements IReportw.SetDataValueGlobalAttributes
            Dim answer_ As Boolean = False

            answer_ = _excelreport.GetReportCollections.SetDataValueGlobalAttributes(index_, datavalue_, positionxy_)

            Return answer_

        End Function

        Public Sub ExcelDispose() _
            Implements IReportw.ExcelDispose

            If Not _excelreport Is Nothing Then

                _excelreport.Dispose()

            End If

        End Sub
#End Region

    End Class

End Namespace
