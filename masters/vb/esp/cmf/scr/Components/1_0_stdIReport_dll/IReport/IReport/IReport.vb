Imports Gsol.BaseDatos.Operaciones
Imports gsol

Namespace Wma.Reports

    Public Interface IReportw

#Region "Attributes"

        Enum Extensions
            und = 0
            xls = 1
            xlsx = 2
            txt = 3
            log = 4
            csv = 5
        End Enum

        Enum MakerModes

            ThroughPackage
            ThroughCollections 'Stack
            ThroughPackageLaunchPreview
            Manual

        End Enum

#End Region

#Region "Properties"

        Property FilePath As String

        Property FileName As String

        Property FileExtension As Extensions

        Property MakerMode As MakerModes

        Property PackageIOperations As IOperacionesCatalogo

        WriteOnly Property setWorkSpace As IEspacioTrabajo

        ReadOnly Property getReportObject As Object

        WriteOnly Property setReportTheme As ExcelApplicationReports.WorksheetThemes

        WriteOnly Property setPassword As String

        Property PathTemplate As String

        'Customize reports


#End Region

#Region "Methods"

        Function AddFullRowsBellowOf(ByVal positionxy_ As String,
                                     ByVal quantity_ As Int32) As Boolean

        Function SetDataValueGlobalAttributes(ByVal index_ As Int32,
                                              ByVal datavalue_ As String,
                                              ByVal positionxy_ As String) As Boolean

        Sub GenerateReport()


#End Region



    End Interface



End Namespace



