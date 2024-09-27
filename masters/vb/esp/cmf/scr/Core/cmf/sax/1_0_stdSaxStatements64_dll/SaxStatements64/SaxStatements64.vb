Imports System.ComponentModel.Design.Serialization
Imports System.Diagnostics.SymbolStore
Imports System.IO
Imports System.Runtime.Remoting.Contexts
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Attributes
Imports Sax.SaxStatements
Imports Wma.Exceptions

Namespace Sax
    Public Class SaxStatements
        Public Enum SettingTypes
            Globals = 0
            Projects = 1
            Core = 2
        End Enum

        Public Enum RoladminTypes
            master = 1
            slave = 2
        End Enum

        Public Enum ConnectionTypes
            Standard = 0
            StringConnection = 1
            APIRESTful = 2
        End Enum

        Public Enum CollectionTypes
            UN = 0 ' Undefined
            OO = 1 'Operations
            ODS = 2 'Dimensional Bigdata
            ODF = 3 'Dimensional Flat (Replicated from RDBMS)
            Apendices = 4
        End Enum

#Region "attributes"

        Private Shared _instance As SaxStatements = Nothing

        'List of all projects
        Private _saxprojects As SaxProjects

        'List of global settings un pos 0, & specific project in pos 1
        Private _saxsettings As List(Of SaxSettings)

        'A specific project structure. modules, binaries, etc.
        Private _projectstructure As projectstructure

        Private _tagwatcher As New TagWatcher

        Private _object As Object

        Private _saxappidMaster As Int32? = Nothing

#End Region

#Region "builders"

        Private Sub New(ByVal appId_ As Int32)

            _saxprojects = New SaxProjects

            _saxsettings = New List(Of SaxSettings)

            'Carga master
            Initialize(appId_)

        End Sub

        Private Sub New()

            _saxprojects = New SaxProjects

            _saxsettings = New List(Of SaxSettings)

            Initialize()

        End Sub

#End Region

#Region "properties"

        ReadOnly Property SaxAppIdMaster As Int32?

            Get
                Return _saxappidMaster

            End Get

        End Property
        Property ObjectSession As Object
            Get
                Return _object
            End Get
            Set(value As Object)
                _object = value
            End Set
        End Property

        Property ProjectStructure As projectstructure
            Get
                Return _projectstructure
            End Get
            Set(value As projectstructure)
                _projectstructure = value
            End Set
        End Property
        ReadOnly Property TagWatcher As TagWatcher
            Get
                Return _tagwatcher
            End Get
        End Property

        Property SaxProjects As SaxProjects
            Get
                Return _saxprojects
            End Get
            Set(value As SaxProjects)
                _saxprojects = value
            End Set
        End Property

        Property SaxSettings As List(Of SaxSettings)
            Get
                Return _saxsettings
            End Get
            Set(value As List(Of SaxSettings))
                _saxsettings = value
            End Set
        End Property

#End Region

#Region "methods"

        Public Sub Initialize(ByVal appId_ As Int32,
                              Optional ByVal roladmin_ As RoladminTypes = RoladminTypes.master)

            LoadSettings(appId_, roladmin_)

        End Sub

        Private Sub Initialize()

            LoadSettings()

        End Sub

        Public Shared Function GetInstance() As SaxStatements

            If _instance Is Nothing Then

                _instance = New SaxStatements()

            End If

            Return _instance

        End Function

        Public Shared Function GetInstance(ByVal appId_ As Int32) As SaxStatements

            Dim slaves_ As List(Of slave)

            If _instance Is Nothing Then

                _instance = New SaxStatements()

                For Each project_ As project In _instance.SaxProjects.projects

                    'Turning off & turning on current project.
                    If project_.saxidapp = appId_ Then

                        project_.online = True

                        slaves_ = project_.slaves

                    Else
                        project_.online = False

                    End If

                Next

            ElseIf Not IsOnLine(appId_) Then

                Dim nothing_ = New SaxStatements(appId_)

                For Each project_ As project In _instance.SaxProjects.projects

                    'Turning off & turning on current project.
                    If project_.saxidapp = appId_ Then

                        project_.online = True

                        slaves_ = project_.slaves

                    Else
                        project_.online = False

                    End If

                Next

            End If

            'Turning on slaves of master project

            If slaves_ IsNot Nothing Then

                For Each slave_ In slaves_

                    For Each project_ As project In _instance.SaxProjects.projects

                        'Turning off & turning on current project.
                        If project_.saxidapp = slave_.saxappid And project_.roladmin = RoladminTypes.slave.ToString Then

                            project_.online = True

                        End If

                    Next

                Next

            End If

            Return _instance

        End Function
        Public Shared Function GetInstance(ByVal appId_ As Int32,
                                           Optional ByVal roladmin_ As RoladminTypes = RoladminTypes.master) As SaxStatements

            If _instance Is Nothing Then

                _instance = New SaxStatements(appId_)

                For Each project_ As project In _instance.SaxProjects.projects

                    'Turning off & turning on current master project.
                    If project_.saxidapp = appId_ AndAlso project_.roladmin = roladmin_.ToString Then

                        project_.online = True

                    Else
                        project_.online = False

                    End If

                Next

            ElseIf Not IsOnLine(appId_, roladmin_) Then

                Dim nothing_ = New SaxStatements(appId_)

                For Each project_ As project In _instance.SaxProjects.projects

                    'Turning off & turning on current project.
                    If project_.saxidapp = appId_ AndAlso project_.roladmin = roladmin_.ToString Then

                        project_.online = True

                    Else
                        project_.online = False

                    End If

                Next

            End If

            Return _instance

        End Function

        Public Shared Function IsOnLine(ByVal appId_ As Int32,
                                        Optional ByVal roladmin_ As RoladminTypes = RoladminTypes.master) As Boolean

            For Each project_ In _instance.SaxProjects.projects

                If project_.saxidapp = appId_ AndAlso project_.roladmin = roladmin_.ToString Then

                    Return project_.online

                End If

            Next

            Return False

        End Function

        Public Shared Function GetAppOnLine() As project

            For Each project_ In _instance.SaxProjects.projects

                If project_.online And project_.roladmin = RoladminTypes.master.ToString Then

                    Return project_

                End If

            Next

            Return Nothing

        End Function


        Public Shared Function GetAppOnLine(roladminType_ As RoladminTypes) As project

            For Each project_ In _instance.SaxProjects.projects

                If project_.online And project_.roladmin = roladminType_.ToString Then

                    Return project_

                End If

            Next

            Return Nothing

        End Function

        Private Function LoadSettingsComplete(ByVal appId_ As Int32) As TagWatcher

            'Dim pathglobalp1_ As String = "c:\sax\settings\init\sax.projects.json"

            'Dim pathglobals2_ As String = "c:\sax\settings\init\sax.settings.json"

            Dim pathglobalp1_ As String = "c:\inetpub\wwwroot\saxtest\sax\settings\init\sax.projects.json"

            Dim pathglobals2_ As String = "c:\inetpub\wwwroot\saxtest\sax\settings\init\sax.settings.json"

            If Not (File.Exists(pathglobalp1_) Or
                  File.Exists(pathglobals2_)) Then

                Return New TagWatcher(0, Me, "No se localizó alguno de los archivos: sax.projects.json o sax.settings.json", TagWatcher.ErrorTypes.C1_001_20000)

            End If

            Try

                'saxprojects -- global
                Dim globalProjects_ As String = File.ReadAllText(pathglobalp1_)

                'saxsettings -- global
                Dim globalSettings_ As String = File.ReadAllText(pathglobals2_)

                'Globals
                _saxsettings.Add(BsonSerializer.Deserialize(Of SaxSettings)(New BsonDocument().Parse(globalSettings_)))

                _saxprojects = BsonSerializer.Deserialize(Of SaxProjects)(New BsonDocument().Parse(globalProjects_))

                '********************* select my project settings *********************

                Dim myprojectpath_ As String = Nothing

                Dim myprojectmodulespath_ As String = Nothing

                For Each project_ As project In _saxprojects.projects

                    If project_.activated And project_.saxidapp = appId_ Then

                        myprojectpath_ = project_.prjsettings

                        myprojectmodulespath_ = project_.modulessettings

                        Exit For

                    End If

                Next

                If Not myprojectpath_ Is Nothing Then

                    Dim pathglobalsp_ As String = myprojectpath_

                    If Not (File.Exists(pathglobalsp_)) Then

                        Return New TagWatcher(0, Me, "No se localizó el archivo: " & pathglobalsp_, TagWatcher.ErrorTypes.C1_001_20000)

                    End If

                    'saxprojects -- a project
                    Dim projectSettings_ As String = File.ReadAllText(pathglobalsp_)

                    'A project
                    _saxsettings.Add(BsonSerializer.Deserialize(Of SaxSettings)(New BsonDocument().Parse(projectSettings_)))

                    If (File.Exists(myprojectmodulespath_)) Then

                        'Loading modules of project
                        Dim projectModules_ As String = File.ReadAllText(myprojectmodulespath_)


                        _projectstructure = BsonSerializer.Deserialize(Of projectstructure)(New BsonDocument().Parse(projectModules_))

                    Else

                        Return New TagWatcher(0, Me, "No se localizó el archivo: " & myprojectmodulespath_, TagWatcher.ErrorTypes.C1_001_20000)

                    End If

                    _tagwatcher.SetOK()

                    Return New TagWatcher(1)

                Else

                    Return New TagWatcher(0, Me, "Settings of project was not found.", TagWatcher.ErrorTypes.C1_001_20001)

                End If

            Catch ex_ As Exception

                Return New TagWatcher(0, Me, ex_.Message, TagWatcher.ErrorTypes.C1_001_20002)

            End Try

        End Function
        Private Function LoadSettings() As TagWatcher

            Dim pathglobalp1_ As String = "c:\inetpub\wwwroot\saxtest\sax\settings\init\sax.projects.json"

            Dim pathglobals2_ As String = "c:\inetpub\wwwroot\saxtest\sax\settings\init\sax.settings.json"

            'Dim pathglobalp1_ As String = "c:\sax\settings\init\sax.projects.json"

            'Dim pathglobals2_ As String = "c:\sax\settings\init\sax.settings.json"

            'Dim pathglobalp1_ As String = "d:\Trabajos\krom\Sax\settings\init\sax.projects.json"

            'Dim pathglobals2_ As String = "d:\Trabajos\krom\Sax\settings\init\sax.settings.json"

            If Not (File.Exists(pathglobalp1_) Or
                  File.Exists(pathglobals2_)) Then

                Return New TagWatcher(0, Me, "File not found: " & pathglobals2_ & "," & pathglobalp1_, TagWatcher.ErrorTypes.C1_001_20002)

            End If

            Try

                'saxprojects -- global
                Dim globalProjects_ As String = File.ReadAllText(pathglobalp1_)

                'saxsettings -- global
                Dim globalSettings_ As String = File.ReadAllText(pathglobals2_)

                'Globals
                _saxsettings.Add(BsonSerializer.Deserialize(Of SaxSettings)(New BsonDocument().Parse(globalSettings_)))

                _saxprojects = BsonSerializer.Deserialize(Of SaxProjects)(New BsonDocument().Parse(globalProjects_))


            Catch ex_ As Exception

                Return New TagWatcher(0, Me, ex_.Message)

            End Try

        End Function

        Private Function LoadSlavesFromMaster(ByVal slaves_ As List(Of slave)) As TagWatcher

            Try

                Dim status_ As New TagWatcher

                For Each slave_ In slaves_

                    status_ = LoadSettings(slave_.saxappid, RoladminTypes.slave)

                    If status_.Status <> TagWatcher.TypeStatus.Ok Then

                        Exit For

                    End If

                    Return status_

                Next

            Catch ex_ As Exception

                Return New TagWatcher(0, Me, ex_.Message)

            End Try



        End Function


        Private Function LoadSettings(ByVal appId_ As Int32,
                                      Optional ByVal roladmin_ As RoladminTypes = RoladminTypes.master) As TagWatcher

            Try

                Dim slaves_ As New List(Of slave)

                Dim myprojectpath_ As String = Nothing

                Dim myprojectmodulespath_ As String = Nothing

                If Not _instance Is Nothing Then

                    If Not _instance.SaxProjects Is Nothing Then

                        For Each project_ As project In _instance.SaxProjects.projects

                            If project_.activated And project_.saxidapp = appId_ And project_.roladmin = roladmin_.ToString Then

                                _saxappidMaster = project_.saxidapp

                                myprojectpath_ = project_.prjsettings

                                slaves_ = project_.slaves

                                myprojectmodulespath_ = project_.modulessettings

                                Exit For

                            End If

                        Next

                    End If

                End If

                If Not myprojectpath_ Is Nothing Then

                    Dim pathglobalsp_ As String = myprojectpath_

                    If Not (File.Exists(pathglobalsp_)) Then

                        Return New TagWatcher(0, Me, "File not found: " & pathglobalsp_)

                    End If

                    'saxprojects -- a project
                    Dim projectSettings_ As String = File.ReadAllText(pathglobalsp_)

                    'A project
                    _instance.SaxSettings.Add(BsonSerializer.Deserialize(Of SaxSettings)(New BsonDocument().Parse(projectSettings_)))

                    If (File.Exists(myprojectmodulespath_)) Then

                        'Loading modules of project
                        Dim projectModules_ As String = File.ReadAllText(myprojectmodulespath_)

                        _instance.ProjectStructure = BsonSerializer.Deserialize(Of projectstructure)(New BsonDocument().Parse(projectModules_))

                    End If

                    If roladmin_ = RoladminTypes.master Then

                        Dim finalStatus_ As TagWatcher = LoadSlavesFromMaster(slaves_)

                        Return finalStatus_ 'New TagWatcher(1)

                    Else

                        Return New TagWatcher(1)

                    End If

                Else

                    Return New TagWatcher(0, Me, "settings of project was not found.")

                End If

            Catch ex_ As Exception

                Return New TagWatcher(0, Me, ex_.Message)

            End Try

        End Function

        Public Function GetCredentials(ByVal settingsType_ As String,
                                       ByVal credentialId_ As Int32,
                                       ByVal saxappid_ As Int32) As Sax.credential

            Dim credentialEmpty_ As New Sax.credential With {.user = Nothing, .password = Nothing, .info = Nothing}

            With GetSettings(settingsType_, saxappid_)

                If Not .credentials Is Nothing Then

                    For Each credential_ As Sax.credential In .credentials

                        If credential_._id = credentialId_ Then

                            Return credential_

                        End If

                    Next

                End If

            End With

            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontraron las credenciales!")

            Return credentialEmpty_

        End Function
        Public Function GetCredentials(ByVal settingsType_ As String,
                                       ByVal credentialId_ As Int32) As Sax.credential

            Dim credentialEmpty_ As New Sax.credential With {.user = Nothing, .password = Nothing, .info = Nothing}

            With GetSettings(settingsType_)

                If Not .credentials Is Nothing Then

                    For Each credential_ As Sax.credential In .credentials

                        If credential_._id = credentialId_ Then

                            Return credential_

                        End If

                    Next

                End If

            End With

            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontraron las credenciales!")

            Return credentialEmpty_

        End Function

        Public Function GetLinkedResourceInfo(ByVal objectName_ As String,
                                              Optional ByVal settingsType_ As String = "project",
                                          Optional ByVal rolname_ As IEnlaceDatos.RolNames = IEnlaceDatos.RolNames.bigdata) As Sax.root

            For Each root_ As Sax.root In GetSettings(settingsType_).roots

                For Each linkedresource_ As Sax.linkedresource In root_.linkedresources

                    If linkedresource_.name = objectName_ Then

                        Return root_

                    End If

                Next

            Next

            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontró una raíz relacionada con " & settingsType_ & "!")

            Return Nothing

        End Function

        Public Function GetDatabaseAndCollectionName(ByRef outputDatabaseName_ As String,
                                                     ByRef outputCollectionName_ As String,
                                                     Optional ByVal resourceName_ As String = Nothing,
                                                     Optional ByVal rootid_ As Int32? = Nothing,
                                                     Optional ByVal settingsType_ As String = "project") As Sax.rol

            Dim tagwatcher_ As New TagWatcher

            If resourceName_ Is Nothing And rootid_ Is Nothing Then

                tagwatcher_.SetError(Me, "¡No envió el nombre de un recurso o id de root para buscar!")

                Return Nothing

            End If

            For Each root_ As Sax.root In GetSettings(settingsType_).roots

                If Not resourceName_ Is Nothing Then

                    If root_.collection = resourceName_ Then

                        outputCollectionName_ = root_.collection

                        For Each rol_ As Sax.rol In GetSettings(settingsType_).servers.nosql.mongodb.rol

                            If rol_.rolname = root_.rolnamelinked Then

                                outputDatabaseName_ = rol_.name

                                Return rol_

                            End If

                        Next

                    End If

                End If

                If Not rootid_ Is Nothing Then

                    If root_._id = rootid_ Then

                        If Not (root_.collection = "auto") And Not (root_.collection Is Nothing) Then

                            outputCollectionName_ = root_.collection

                        Else

                            outputCollectionName_ = resourceName_

                        End If

                        For Each rol_ As Sax.rol In GetSettings(settingsType_).servers.nosql.mongodb.rol

                            If rol_.rolname = root_.rolnamelinked Then

                                outputDatabaseName_ = rol_.name

                                Return rol_

                            End If

                        Next

                    End If

                Else

                    For Each linkedresource_ As Sax.linkedresource In root_.linkedresources

                        If linkedresource_.name = resourceName_ Then

                            If Not (root_.collection = "auto") And Not (root_.collection Is Nothing) Then

                                outputCollectionName_ = root_.collection

                            Else

                                outputCollectionName_ = resourceName_

                            End If
                            'outputCollectionName_ = root_.collection

                            For Each rol_ As Sax.rol In GetSettings(settingsType_).servers.nosql.mongodb.rol

                                If rol_.rolname = root_.rolnamelinked Then

                                    outputDatabaseName_ = rol_.name

                                    Return rol_

                                End If

                            Next

                        End If

                    Next

                End If

            Next

            tagwatcher_.SetError(Me, "¡No se encontraron un rol que corresponda con el recurso!")

            Return Nothing

        End Function

        Public Function GetDatabaseAndCollectionNameByApplication(ByRef outputDatabaseName_ As String,
                                                                  ByRef outputCollectionName_ As String,
                                                                  ByVal saxappid_ As Int32?,
                                                                 Optional ByVal resourceName_ As String = Nothing,
                                                                 Optional ByVal rootid_ As Int32? = Nothing,
                                                                 Optional ByVal settingsType_ As String = "project") As Sax.rol

            Dim tagwatcher_ As New TagWatcher

            Dim settings_ As New SaxSettings

            If Not saxappid_ Is Nothing Then

                settings_ = GetSettings(settingsType_, saxappid_)

            Else

                settings_ = GetSettings(settingsType_)

            End If


            If resourceName_ Is Nothing And rootid_ Is Nothing Then

                tagwatcher_.SetError(Me, "¡No envió el nombre de un recurso o id de root para buscar!")

                Return Nothing

            End If

            For Each root_ As Sax.root In settings_.roots 'GetSettings(settingsType_, saxappid_).roots

                If Not resourceName_ Is Nothing Then

                    If root_.collection = resourceName_ Then

                        outputCollectionName_ = root_.collection

                        For Each rol_ As Sax.rol In settings_.servers.nosql.mongodb.rol

                            If rol_.rolname = root_.rolnamelinked Then

                                outputDatabaseName_ = rol_.name

                                Return rol_

                            End If

                        Next

                    End If

                End If

                If Not rootid_ Is Nothing Then

                    If root_._id = rootid_ Then

                        'Si hay un dbrolid se descarta todo lo demas, tendrá preferencia

                        If Not root_.dbrolid Is Nothing Then

                            outputCollectionName_ = root_.collection

                            For Each rol_ As Sax.rol In settings_.servers.nosql.mongodb.rol

                                If rol_._id = root_.dbrolid Then

                                    outputDatabaseName_ = rol_.name

                                    Return rol_

                                End If

                            Next

                        Else

                            If Not (root_.collection = "auto") And Not (root_.collection Is Nothing) Then

                                outputCollectionName_ = root_.collection

                            Else

                                outputCollectionName_ = resourceName_

                            End If

                            For Each rol_ As Sax.rol In settings_.servers.nosql.mongodb.rol

                                If rol_.rolname = root_.rolnamelinked Then

                                    outputDatabaseName_ = rol_.name

                                    Return rol_

                                End If

                            Next

                        End If

                    End If

                Else

                    For Each linkedresource_ As Sax.linkedresource In root_.linkedresources

                        If linkedresource_.name = resourceName_ Then

                            If Not (root_.collection = "auto") And Not (root_.collection Is Nothing) Then

                                outputCollectionName_ = root_.collection

                            Else

                                outputCollectionName_ = resourceName_

                            End If
                            'outputCollectionName_ = root_.collection

                            For Each rol_ As Sax.rol In settings_.servers.nosql.mongodb.rol

                                If rol_.rolname = root_.rolnamelinked Then

                                    outputDatabaseName_ = rol_.name

                                    Return rol_

                                End If

                            Next

                        End If

                    Next

                End If

            Next

            tagwatcher_.SetError(Me, "¡No se encontraron un rol que corresponda con el recurso!")

            Return Nothing

        End Function
        Public Function GetEndPoint(ByVal settingsType_ As String,
                                    ByVal endpointId_ As Int32,
                                    ByVal saxappid_ As Int32) As Sax.endpoint

            Return GetEndPointInternal(settingsType_, endpointId_, saxappid_)

        End Function

        Private Function GetEndPointInternal(settingsType_ As String,
                                    ByVal endpointId_ As Int32,
                                    Optional ByVal saxappid_ As Int32? = Nothing) As Sax.endpoint

            Dim endPoint_ As New Sax.endpoint With {.ip = "127.0.0.1", .port = Nothing, .info = Nothing}

            If Not saxappid_ Is Nothing Then

                With GetSettings(settingsType_, saxappid_)

                    If Not .endpoints Is Nothing Then

                        If Not .about Is Nothing Then

                            For Each endpointitem_ As Sax.endpoint In .endpoints

                                If endpointitem_._id = endpointId_ Then

                                    Return endpointitem_

                                End If

                            Next

                        End If

                    End If

                End With

            Else

                With GetSettings(settingsType_)

                    If Not .endpoints Is Nothing Then

                        If Not .about Is Nothing Then

                            For Each endpointitem_ As Sax.endpoint In .endpoints

                                If endpointitem_._id = endpointId_ Then

                                    Return endpointitem_

                                End If

                            Next

                        End If

                    End If

                End With

            End If


            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontraron endpoints disponibles!")

            Return endPoint_

        End Function
        Public Function GetEndPoint(ByVal settingsType_ As String,
                                    ByVal endpointId_ As Int32) As Sax.endpoint

            Return GetEndPointInternal(settingsType_, endpointId_)

        End Function

        Public Function GetSlot(ByVal settingsType_ As String,
                                 ByVal slotType_ As String) As Sax.slot

            Dim slotEmpty_ As New Sax.slot With {.activated = False, .type = Nothing, ._id = Nothing}

            With GetSettings(settingsType_)

                If Not .logs Is Nothing Then

                    For Each slotItem_ As Sax.slot In .logs.slots

                        If slotItem_.type = slotType_ Then

                            Return slotItem_

                        End If

                    Next

                End If

            End With

            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontraron slots disponibles!")

            Return slotEmpty_

        End Function
        Public Function GetRol(ByVal settingsType_ As String,
                                ByVal rolType_ As String,
                                ByVal typedb_ As String,
                                ByVal db_ As String) As Sax.rol

            Dim rol_ As New Sax.rol With {.name = Nothing,
                                          .credentialId = 0,
                                          .endpointId = 0,
                                          .rolname = Nothing}

            Dim listRoles_ As List(Of Sax.rol)

            Select Case typedb_

                Case "sql"

                    Select Case db_

                        Case "mssql"

                            With GetSettings(settingsType_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.sql.mssql.rol

                                    End If

                                End If

                            End With

                        Case "mysql"

                            With GetSettings(settingsType_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.sql.mysql.rol

                                    End If

                                End If

                            End With

                    End Select

                Case "nosql"

                    Select Case db_

                        Case "mongodb"

                            With GetSettings(settingsType_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.nosql.mongodb.rol

                                    End If

                                End If

                            End With

                        Case "firebase"

                            With GetSettings(settingsType_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.nosql.firebase.rol

                                    End If

                                End If

                            End With

                    End Select

            End Select

            If Not listRoles_ Is Nothing Then

                For Each rolItem_ As Sax.rol In listRoles_

                    If rolItem_.status = "on" And
                       rolItem_.rolname = rolType_ Then

                        Return rolItem_

                    End If

                Next

            End If

            Return rol_

        End Function

        Public Function GetDBRolFromRootId(ByVal typedb_ As String,
                                             ByVal db_ As String,
                                             ByVal rootid_ As Int32,
                                             ByVal saxappid_ As Int32?) As rol


            Dim rol_ As New Sax.rol With {.name = Nothing,
                                          .credentialId = 0,
                                          .endpointId = 0,
                                          .rolname = Nothing}

            Dim listRoles_ As List(Of Sax.rol)

            Dim settings_ As New SaxSettings

            If Not saxappid_ Is Nothing Then

                settings_ = GetSettings("project", saxappid_)

            Else

                settings_ = GetSettings("project", SaxAppIdMaster)

            End If

            Dim dbrolid_in_room_ As Int32? = Nothing

            For Each root_ As root In settings_.roots

                If root_._id = rootid_ Then

                    dbrolid_in_room_ = root_.dbrolid

                    Exit For

                End If

            Next

            If Not dbrolid_in_room_ Is Nothing Then

                'rol_ = GetRolByDBRolId("project", Nothing, "nosql", "mongodb", dbrolid_in_room_, saxappid_)

                '***************************************

                Select Case typedb_

                    Case "sql"

                        Select Case db_

                            Case "mssql"

                                With settings_

                                    If Not .servers Is Nothing Then

                                        If Not .servers Is Nothing Then

                                            listRoles_ = .servers.sql.mssql.rol

                                        End If

                                    End If

                                End With

                            Case "mysql"

                                With settings_

                                    If Not .servers Is Nothing Then

                                        If Not .servers Is Nothing Then

                                            listRoles_ = .servers.sql.mysql.rol

                                        End If

                                    End If

                                End With

                        End Select

                    Case "nosql"

                        Select Case db_

                            Case "mongodb"

                                With settings_

                                    If Not .servers Is Nothing Then

                                        If Not .servers Is Nothing Then

                                            listRoles_ = .servers.nosql.mongodb.rol

                                        End If

                                    End If

                                End With

                            Case "firebase"

                                With settings_

                                    If Not .servers Is Nothing Then

                                        If Not .servers Is Nothing Then

                                            listRoles_ = .servers.nosql.firebase.rol

                                        End If

                                    End If

                                End With

                        End Select

                End Select

                If Not listRoles_ Is Nothing Then

                    For Each rolItem_ As Sax.rol In listRoles_

                        If rolItem_.status = "on" And
                           rolItem_._id = dbrolid_in_room_ Then

                            Return rolItem_

                        End If

                    Next

                End If

                '***************************************

            Else

                Dim answer_ As New TagWatcher()

                answer_.SetError(Me, "Does not exist the roomid or dbrolid " & rootid_.ToString & " in your sax settings.")

                Return rol_

            End If

            Return rol_


        End Function

        Public Function GetRolByDBRolId(ByVal settingsType_ As String,
                               ByVal typedb_ As String,
                               ByVal db_ As String,
                               ByVal dbrolid_ As Int32,
                               Optional ByVal saxappid_ As Int32? = Nothing) As Sax.rol


            Dim rol_ As New Sax.rol With {.name = Nothing,
                                          .credentialId = 0,
                                          .endpointId = 0,
                                          .rolname = Nothing}

            Dim listRoles_ As List(Of Sax.rol)

            Dim settings_ As New SaxSettings

            If Not saxappid_ Is Nothing Then

                settings_ = GetSettings(settingsType_, saxappid_)

            Else

                settings_ = GetSettings(settingsType_)

            End If

            Select Case typedb_

                Case "sql"

                    Select Case db_

                        Case "mssql"

                            With settings_ 'GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.sql.mssql.rol

                                    End If

                                End If

                            End With

                        Case "mysql"

                            With settings_ 'GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.sql.mysql.rol

                                    End If

                                End If

                            End With

                    End Select

                Case "nosql"

                    Select Case db_

                        Case "mongodb"

                            With settings_ 'GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.nosql.mongodb.rol

                                    End If

                                End If

                            End With

                        Case "firebase"

                            With settings_ 'GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.nosql.firebase.rol

                                    End If

                                End If

                            End With

                    End Select

            End Select

            If Not listRoles_ Is Nothing Then

                For Each rolItem_ As Sax.rol In listRoles_

                    If rolItem_.status = "on" And
                        rolItem_._id = dbrolid_ Then

                        Return rolItem_

                    End If

                Next

            End If

            Return rol_

        End Function

        Public Function GetRol(ByVal settingsType_ As String,
                               ByVal rolType_ As String,
                               ByVal typedb_ As String,
                               ByVal db_ As String,
                               ByVal saxappid_ As Int32) As Sax.rol

            Dim rol_ As New Sax.rol With {.name = Nothing,
                                          .credentialId = 0,
                                          .endpointId = 0,
                                          .rolname = Nothing}

            Dim listRoles_ As List(Of Sax.rol)

            Select Case typedb_

                Case "sql"

                    Select Case db_

                        Case "mssql"

                            With GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.sql.mssql.rol

                                    End If

                                End If

                            End With

                        Case "mysql"

                            With GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.sql.mysql.rol

                                    End If

                                End If

                            End With

                    End Select

                Case "nosql"

                    Select Case db_

                        Case "mongodb"

                            With GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.nosql.mongodb.rol

                                    End If

                                End If

                            End With

                        Case "firebase"

                            With GetSettings(settingsType_, saxappid_)

                                If Not .servers Is Nothing Then

                                    If Not .servers Is Nothing Then

                                        listRoles_ = .servers.nosql.firebase.rol

                                    End If

                                End If

                            End With

                    End Select

            End Select

            If Not listRoles_ Is Nothing Then

                For Each rolItem_ As Sax.rol In listRoles_

                    If rolItem_.status = "on" And
                       rolItem_.rolname = rolType_ Then

                        Return rolItem_

                    End If

                Next

            End If

            Return rol_

        End Function
        Public Function GetSettings(ByVal scope_ As String,
                                    ByVal saxappid_ As Int32) As Sax.SaxSettings

            Dim saxsettings_ As New Sax.SaxSettings With {.about = "void"}

            For Each settingsItem_ As Sax.SaxSettings In _instance.SaxSettings

                If settingsItem_.scope = scope_ And settingsItem_.saxidapp = saxappid_ Then

                    Return settingsItem_

                End If

            Next

            Return saxsettings_

        End Function

        Public Function GetSettings(ByVal scope_ As String) As Sax.SaxSettings

            Dim saxsettings_ As New Sax.SaxSettings With {.about = "void"}

            For Each settingsItem_ As Sax.SaxSettings In _instance.SaxSettings

                If Not _saxappidMaster Is Nothing Then

                    If settingsItem_.scope = scope_ And settingsItem_.saxidapp = _saxappidMaster Then

                        Return settingsItem_

                    End If

                Else

                    If settingsItem_.scope = scope_ Then

                        Return settingsItem_

                    End If


                End If


            Next

            Return saxsettings_

        End Function

        Public Function GetEnvironments(Optional ByVal settingsType_ As String = "project") As Dictionary(Of String, String)

            Dim environmentList_ As New Dictionary(Of String, String)

            'En caso de no encontrar en el for anterior, buscará en las roots generales la comprometida con el rolname
            For Each root_ As Sax.environment In _instance.GetSettings(settingsType_).environments

                environmentList_.Add(root_._id, root_.name)

            Next

            If environmentList_.Count = 0 Then
                Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontraron ambientes disponibles!")
            End If

            Return environmentList_

        End Function
        Public Function GetSimpleListOfRoots(ByVal rolname_ As IEnlaceDatos.RolNames,
                                          Optional ByVal databaseProvider_ As String = "mongodb",
                                          Optional ByVal settingsType_ As String = "project",
                                          Optional ByVal status_ As String = "on") As Dictionary(Of String, String)

            Dim rootList_ As New Dictionary(Of String, String)

            For Each rol_ As Sax.rol In GetRols(settingsType_, databaseProvider_)

                If rol_.rolname = rolname_.ToString And rol_.status = status_ Then
                    'rol_.credentialId
                    For Each root_ As Sax.root In _instance.GetEndPoint(settingsType_, rol_.endpointId).roots

                        rootList_.Add(root_._id, root_.info)

                    Next

                    'En caso de no encontrar en el for anterior, buscará en las roots generales la comprometida con el rolname
                    For Each root_ As Sax.root In _instance.GetSettings(settingsType_).roots

                        If root_.rolnamelinked = rolname_ Then

                            rootList_.Add(root_._id, root_.info)

                        End If

                    Next

                End If

            Next

            If rootList_.Count = 0 Then
                Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontraron oficinas registradas!")
            End If

            Return rootList_

        End Function
        Public Sub SetEnvironmentOnline(ByVal environmentId_ As Int32, Optional ByVal settingsType_ As String = "project")

            For Each environment_ As Sax.environment In _instance.GetSettings(settingsType_).environments

                'Nos ponemos en linea con la oficina a la que pertenecemos
                If environment_._id = environmentId_ Then : environment_.online = True

                Else : environment_.online = False

                End If

            Next

        End Sub
        Public Function GetRols(ByVal settingsType_ As String,
                                 ByVal databaseProvider_ As String) As List(Of Sax.rol)

            Dim listOfRols_ As New List(Of Sax.rol)

            With _instance.GetSettings(settingsType_)

                If Not .servers Is Nothing Then

                    With .servers

                        Select Case databaseProvider_

                            Case "mongodb" : listOfRols_ = .nosql.mongodb.rol

                            Case "mssql" : listOfRols_ = .sql.mssql.rol

                            Case "mysql" : listOfRols_ = .sql.mysql.rol

                            Case "firebase" : listOfRols_ = .nosql.firebase.rol

                        End Select

                    End With

                Else
                    Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontró registro de servidores!")

                End If

            End With

            Return listOfRols_

        End Function

        Public Function GetRoot(ByVal rolname_ As IEnlaceDatos.RolNames,
                              Optional ByVal settingsType_ As String = "project") As Sax.root

            'En caso de no encontrar en el for anterior, buscará en las roots generales la comprometida con el rolname
            For Each root_ As Sax.root In _instance.GetSettings(settingsType_).roots

                If root_.rolnamelinked = rolname_ Then

                    Return root_

                End If

            Next

            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontró ninguna oficina online!")

            Return Nothing

        End Function

        Public Function GetOfficeOnline(Optional ByVal settingsType_ As String = "project") As Sax.environment

            For Each environment_ As Sax.environment In _instance.GetSettings(settingsType_).environments

                If environment_.online = True Then

                    Return environment_

                End If

            Next

            Dim tagwatcher_ As New TagWatcher : tagwatcher_.SetError(Me, "¡No se encontró ninguna oficina online!")

            Return Nothing

        End Function

        Public Function GetRootOfficeById(ByVal idMyOffice_ As Int32,
                              ByVal rolname_ As IEnlaceDatos.RolNames,
                              ByVal databaseProvider_ As String,
                              Optional ByVal settingsType_ As String = "project",
                              Optional ByVal status_ As String = "on") As Sax.root

            For Each rol_ As Sax.rol In GetRols(settingsType_, databaseProvider_)

                If rol_.rolname = rolname_.ToString And rol_.status = status_ Then
                    'rol_.credentialId
                    For Each root_ As Sax.root In _instance.GetEndPoint(settingsType_, rol_.endpointId).roots

                        If root_._id = idMyOffice_ Then

                            Return root_

                        End If

                    Next

                    'En caso de no encontrar en el for anterior, buscará en las roots generales la comprometida con el rolname
                    For Each root_ As Sax.root In _instance.GetSettings(settingsType_).roots

                        If root_.rolnamelinked = rolname_ Then

                            Return root_

                        End If

                    Next

                End If

            Next

            Return Nothing

        End Function

#End Region

    End Class

    Public Class Statements

        'Global of projects
        Property SaxProjects As SaxProjects

        'Global of sax & projects
        Property SettingBooks As Dictionary(Of SettingTypes, SaxSettings)

    End Class

#Region "Additional resources"
    Public Class SaxProjects
        Property about As String
        Property version As String
        Property saxsh As String
        Property defaultsettings As String
        Property workdirectory As String
        Property projects As List(Of project)

    End Class

    Public Class slave

        Property saxappid As Int32

        Property name As String
        Property typeconnection As ConnectionTypes

    End Class
    Public Class project
        Property _id As Int32
        Property saxidapp As Int32
        Property name As String
        Property info As String
        Property type As String
        Property status As String
        Property activated As Boolean
        Property online As Boolean

        <BsonIgnoreIfNull>
        Property roladmin As String

        <BsonIgnoreIfNull>
        Property slaves As List(Of slave)

        'saxappid
        Property industry As String
        Property language As String
        Property directoryname As String
        Property modulesbinaries As String
        Property prjsettings As String
        Property modulessettings As String

    End Class

    Public Class SaxSettings
        Property about As String
        Property version As String

        <BsonIgnoreIfNull>
        Property scope As String
        Property saxidapp As Int32
        Property logs As logs
        Property globals As globals
        Property credentials As List(Of credential)
        Property endpoints As List(Of endpoint)
        Property servers As servers
        Property roots As List(Of root)
        Property environments As List(Of environment)
        Property modules As List(Of moduleconfig)

    End Class

    Public Class moduleconfig
        Property assembly As String

    End Class
    Public Class logs
        Property logcontrol As Boolean
        Property slots As List(Of slot)
    End Class
    Public Class slot
        Property _id As Int32
        Property type As String
        Property activated As Boolean
        Property pathlog As String
    End Class
    Public Class globals
        Property startAppId As Int32
        Property startCompanyGroupId As Int32
        Property startCompanySucId As Int32

    End Class
    Public Class credential
        Property _id As Int32
        Property info As String
        Property user As String
        Property password As String
        Property card As String
    End Class
    Public Class endpoint
        Property _id As Int32
        Property info As String
        Property url As String
        Property ip As String
        Property stringoptions As String
        Property port As String
        Property roots As List(Of root)

    End Class

    Public Class root
        Property _id As Int32
        Property info As String
        Property name As String
        Property pref As String
        Property level As Int32?

        <BsonIgnoreIfNull>
        Property dbrolid As Int32?
        Property rolnamelinked As String
        Property officetype As String
        Property online As Boolean
        Property collection As String
        ' Property collections As List(Of collection)
        Property linkedresources As List(Of linkedresource)
        Property type As String

    End Class

    Public Class collection
        Property type As String
        Property info As String
        Property name As String
    End Class

    Public Class linkedresource
        Property type As String
        Property info As String
        Property name As String
    End Class
    Public Class servers
        Property nosql As nosql
        Property sql As sql

    End Class

    Public Class nosql
        Property mongodb As mongodb
        Property firebase As firebase

    End Class

    Public Class sql
        Property mssql As mssql
        Property mysql As mysql

    End Class

    Public Class mongodb
        Property version As String
        Property rol As List(Of rol)

    End Class

    Public Class firebase
        Property version As String
        Property rol As List(Of rol)

    End Class
    Public Class mssql
        Property version As String
        Property rol As List(Of rol)

    End Class

    Public Class mysql
        Property version As String
        Property rol As List(Of rol)

    End Class
    Public Class rol
        Property _id As Int32
        Property status As String
        Property rolname As String
        Property info As String
        Property name As String
        Property officesuffix As Boolean
        Property credentialId As Int32
        Property endpointId As Int32

    End Class

    Public Class environment
        ' "environments":[{"_id":1,"name":"Veracruz","pref":"RKU", "taxid":"GRK030919NX4", "info":"Grupo Reyes Kuri. S. C.","online": false},
        Property _id As Int32
        Property name As String
        Property pref As String
        Property taxid As String
        Property info As String
        Property type As String
        Property online As Boolean

    End Class

#End Region

    '---------------modules
#Region "Structure for modules"
    Public Class projectstructure
        Property about As String
        Property version As String
        Property scope As String
        Property workspace As workspace
    End Class

    Public Class workspace
        Property defaults As projectmodule
        Property modules As List(Of projectmodule)
    End Class

    Public Class projectmodule
        Property _id As Int32
        Property tokenid As String
        Property title As String
        Property type As String
        Property version As String
        Property activated As Boolean
        Property autosets As Boolean
        Property datamirrors As List(Of datamirror)
        Property info As String
        Property config As projectconfig
        Property dboperators As dboperators
        Property permissions As permissions
        Property operatorform As operatorform
        Property lnksets As lnksets
        Property customcolumns As List(Of customcolumns)
        Property datasource As String
        Property alternativedatasource As String

    End Class

    Public Class datamirror
        Property database As String
        Property type As String
    End Class
    Public Class projectconfig
        Property formname As String
        Property assemblyname As String
        Property EV As String
        Property WV As String
        Property keyfield As String
        Property table As String
        Property allenvironments As String
        Property upsertkeyname As String

    End Class

    Public Class dboperators
        Property deleteoperator As String
        Property insertoperator As String
        Property updateoperator As String

    End Class

    Public Class permissions
        Property refresh As String
        Property insert As String
        Property query As String
        Property delete As String
        Property update As String

    End Class


    Public Class operatorform
        Property formname As String
        Property assembly As String
        Property showsinglewindow As Boolean
        Property cancontrolsw As Boolean

    End Class

    Public Class lnksets
        Property granularity As String
        Property dimension As String
        Property entity As String

    End Class

    Public Class customcolumns
        Property _id As String
        Property cond As String

    End Class

#End Region

End Namespace