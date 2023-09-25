Imports System.Xml.Serialization
Imports System.Web
Imports System.Text

Imports gsol.basededatos
Imports gsol.seguridad
Imports gsol.BaseDatos.Operaciones
Imports System.IO
Imports Wma.Exceptions

Namespace Wma.Operations

    Public Class EnvironmentViews

        Enum LanguagueType
            ES
            US
        End Enum

#Region "Attributes"

        Private Shared _instance As EnvironmentViews = Nothing

        Private _tablesOfEnvironmentView As List(Of TableEnviromentView)

        Private _mainDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\temp\EnvironmentViews\"

#End Region

#Region "Builders"

        Sub New()

        End Sub

        Sub New(ByVal languagueType_ As LanguagueType)

            'Table EV List
            Dim _tableEnvironmentView As New List(Of TableEnviromentView)

            'Single table ev
            Dim singleTableEV_ As New TableEnviromentView

            With singleTableEV_

                .TableName = "Ve009IUMaestroOperaciones"

            End With

            Select Case languagueType_

                Case LanguagueType.ES

                    '1.
                    Dim _itemInterfazEntorno1 As New ItemEnvironmentView

                    With _itemInterfazEntorno1

                        .Nombre = "i_Cve_Referencia"
                        .Llave = 1
                        .Longitud = 11
                        .TipoDato = 0
                        .Visible = 1
                        .NombreColumna = "Clave Referencia"
                        .PuedeInsertar = 0
                        .PuedeModificar = 0
                        .ValorDefault = Nothing
                        .TipoFiltro = 0
                        .NumeroPermiso = Nothing
                        .NameAsKey = Nothing
                        .Interfaz = Nothing
                        .PermisoConsulta = Nothing
                        .Reflejar = 0

                    End With

                    singleTableEV_.ItemsOfTableEnvironmentView.Add(_itemInterfazEntorno1)

                    '2.
                    Dim _itemInterfazEntorno2 As New ItemEnvironmentView

                    With _itemInterfazEntorno2

                        .Nombre = "i_Cve_MaestroOperaciones"
                        .Llave = 0
                        .Longitud = 11
                        .TipoDato = 0
                        .Visible = 1
                        .NombreColumna = "Clave MaestroOperaciones"
                        .PuedeInsertar = 0
                        .PuedeModificar = 0
                        .ValorDefault = Nothing
                        .TipoFiltro = 0
                        .NumeroPermiso = Nothing
                        .NameAsKey = Nothing
                        .Interfaz = Nothing
                        .PermisoConsulta = Nothing
                        .Reflejar = 0
                    End With

                    singleTableEV_.ItemsOfTableEnvironmentView.Add(_itemInterfazEntorno2)

                    _tableEnvironmentView.Add(singleTableEV_)


                Case LanguagueType.US



            End Select


        End Sub

        'Private Sub ServeralTesters()

        '    Dim x As New Xml.Serialization.XmlSerializer(_tableEnvironmentView.GetType)

        '    x.Serialize(Console.Out, _tableEnvironmentView)


        '    'Dim countries As New List(Of Country)()
        '    ' Make sure you add the countries to the list

        '    'Dim serializer As New XmlSerializer(GetType(List(Of Country)), New XmlRootAttribute("Registros"))


        '    '::::::::::::Through XMl Format

        '    'Serializando...
        '    Dim serializer As New XmlSerializer(_tableEnvironmentView.GetType, New XmlRootAttribute("Registros"))

        '    Using file As System.IO.FileStream = System.IO.File.Open("c:\logs\obj.xml", IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
        '        serializer.Serialize(file, _tableEnvironmentView)
        '    End Using


        '    'Deserializando...
        '    Dim deserializer As New XmlSerializer(_tableEnvironmentView.GetType, New XmlRootAttribute("Registros"))
        '    Dim deserializedObjetc_ As New List(Of TableEnviromentView)
        '    Using file = System.IO.File.OpenRead("c:\logs\obj.xml")
        '        deserializedObjetc_ = DirectCast(deserializer.Deserialize(file), List(Of TableEnviromentView))
        '    End Using

        '    '::::::::::::Through XMl Format


        '    'Dim seat As New Coche

        '    'seat.Marca = "León"

        '    'seat.Deportivo = False

        '    'seat.Caballos = 110



        '    '::::::::::::Through JSON Format

        '    Dim serializador As New System.Web.Script.Serialization.JavaScriptSerializer

        '    Dim sb As New System.Text.StringBuilder

        '    'serializador.Serialize(_tableEnvironmentView, sb)
        '    'serializador.Serialize(_tableEnvironmentView, sb)
        '    'Dim serializer As New XmlSerializer(_tableEnvironmentView.GetType, New XmlRootAttribute("Registros"))

        '    'Using file As System.IO.FileStream = System.IO.File.Open("c:\logs\obj.json", IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
        '    serializador.Serialize(_tableEnvironmentView, sb)


        '    'Este es mejor!
        '    Dim FILE_NAME As String = "C:\logs\object1.json"

        '    'If System.IO.File.Exists(FILE_NAME) = True Then

        '    Dim objWriter As New System.IO.StreamWriter(FILE_NAME)

        '    objWriter.Write(sb)
        '    objWriter.Close()
        '    MsgBox("json done!")
        '    'MessageBox.Show("Text written to file")

        '    'Else
        '    'MsgBox("json not!")

        '    'MessageBox.Show("File Does Not Exist")

        '    'End If



        '    'Segundo DataContractJsonSerializer

        '    Dim serializador3 As New System.Runtime.Serialization.Json.DataContractJsonSerializer(_tableEnvironmentView.GetType)

        '    Dim ms As New System.IO.MemoryStream

        '    serializador3.WriteObject(ms, _tableEnvironmentView)

        '    Dim textoJson As String = System.Text.Encoding.UTF8.GetString(ms.ToArray)

        '    Dim FILE_NAME3 As String = "C:\logs\object3.json"

        '    'If System.IO.File.Exists(FILE_NAME) = True Then

        '    Dim objWriter3 As New System.IO.StreamWriter(FILE_NAME3)

        '    objWriter3.Write(textoJson)
        '    objWriter3.Close()
        '    MsgBox("json done3!")


        '    Dim serializador4 As New System.Web.Script.Serialization.JavaScriptSerializer

        '    'String builder
        '    Dim jsonFile_ As String = Nothing

        '    jsonFile_ = GetTextForOutput("C:\logs\object3.json")


        '    'Conozco el tipo coche... it works!!
        '    'Dim diccionario2_ As List(Of TableEnviromentView) = serializador4.Deserialize(Of List(Of TableEnviromentView))(sb.ToString)
        '    Dim diccionario2_ As List(Of TableEnviromentView) = serializador4.Deserialize(Of List(Of TableEnviromentView))(jsonFile_)


        '    ''NEW Kind
        '    MsgBox(diccionario2_.Count.ToString)


        '    'No conozco el tipo coche...
        '    'Dim diccionario As List(Of TableEnviromentView) = serializador4.DeserializeObject(sb.ToString)
        '    'MsgBox(diccionario.Count.ToString)

        '    '::::::::::::Through JSON Format

        'End Sub

#End Region

#Region "Properties"

#End Region

#Region "Methods"

        Private Function ConvertJSONToDataTable(ByVal tableEnvironmentViewList_ As List(Of TableEnviromentView)) As DataTable

            Dim temporalDataTable_ As New DataTable

            With temporalDataTable_.Columns
                .Add("Llave")
                .Add("Longitud")
                .Add("Nombre")
                .Add("NombreColumna")
                .Add("NumeroPermiso")
                .Add("PuedeInsertar")
                .Add("PuedeModificar")
                .Add("TipoDato")
                .Add("TipoFiltro")
                .Add("ValorDefault")
                .Add("Visible")
                .Add("NameAsKey")
                .Add("Interfaz")
                .Add("PermisoConsulta")
                .Add("Reflejar")
            End With

            For Each item_ As ItemEnvironmentView In tableEnvironmentViewList_(0).ItemsOfTableEnvironmentView

                Dim dataRow_ As DataRow

                dataRow_ = temporalDataTable_.NewRow

                dataRow_("Llave") = item_.Llave
                dataRow_("Longitud") = item_.Longitud
                dataRow_("Nombre") = item_.Nombre
                dataRow_("NombreColumna") = item_.NombreColumna
                dataRow_("NumeroPermiso") = item_.NumeroPermiso
                dataRow_("PuedeInsertar") = item_.PuedeInsertar
                dataRow_("PuedeModificar") = item_.PuedeModificar
                dataRow_("TipoDato") = item_.TipoDato
                dataRow_("TipoFiltro") = item_.TipoFiltro
                dataRow_("ValorDefault") = item_.ValorDefault
                dataRow_("Visible") = item_.Visible
                dataRow_("NameAsKey") = item_.NameAsKey
                dataRow_("Interfaz") = item_.Interfaz
                dataRow_("PermisoConsulta") = item_.PermisoConsulta
                dataRow_("Reflejar") = item_.Reflejar

                temporalDataTable_.Rows.Add(dataRow_)

            Next

            Return temporalDataTable_

        End Function

        Public Function GetEnvironmentViewAsJSON(ByVal environmentViewTableName_ As String,
                                                    Optional ByVal createIfNotExists_ As Boolean = False) As TagWatcher

            Dim tagwatcherMessage_ As New TagWatcher

            Dim jsonFile_ As String = Nothing


            If Directory.Exists(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\temp\EnvironmentViews") Then

                _mainDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\temp\EnvironmentViews\"

            Else

                Try

                    Directory.CreateDirectory(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\temp\EnvironmentViews")

                    _mainDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\temp\EnvironmentViews\"

                Catch ex As Exception

                    If Not Directory.Exists(_mainDirectory) Then

                        Directory.CreateDirectory(_mainDirectory)

                    End If

                End Try

            End If

            jsonFile_ = GetTextForOutput(_mainDirectory & environmentViewTableName_ & ".json")

            If Not jsonFile_ Is Nothing Then

                Dim serialJSON_ As New System.Web.Script.Serialization.JavaScriptSerializer

                Dim requestedObject_ As List(Of TableEnviromentView) = serialJSON_.Deserialize(Of List(Of TableEnviromentView))(jsonFile_)

                Dim datatable_ As New DataTable

                datatable_ = ConvertJSONToDataTable(requestedObject_)

                With tagwatcherMessage_

                    .ObjectReturned = datatable_

                    .SetOK()

                End With

            Else

                tagwatcherMessage_.SetError(TagWatcher.ErrorTypes.C1_000_0001)

            End If

            Return tagwatcherMessage_

        End Function

        Public Function ConvertEVTableToStatic(ByVal environmentViewTableName_ As String) As Boolean

            Dim conexion_ As IConexiones = New Conexiones

            'Dim configuracionaux_ As New Configuracion

            Dim configuracionaux_ As Configuracion

            configuracionaux_ = Configuracion.ObtenerInstancia()

            With conexion_

                .ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

                .Contrasena = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)
                .Usuario = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)
                .NombreBaseDatos = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)
                .IpServidor = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

                .IDUsuario = 139
                .IDAplicacion = 4
                .IDRecursoSolicitante = 0
                .ClaveDivisionMiEmpresa = 1
                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
                .TipoInstrumentacion = gsol.monitoreo.IBitacoras.TiposInstrumentacion.ConsultaLibre

                .NombreEnsamblado = environmentViewTableName_
                .NombreVT = "Convertidor de Vistas"
                .NombreUsuarioCliente = "auto"
                .CuentaPublicaUsuario = "auto"

                .ObjetoDatos = IConexiones.TipoConexion.SqlCommand

                .DataSetReciente.Tables.Clear()

                .EjecutaConsultaIndividual("select * from " & environmentViewTableName_)

            End With

            Dim datasetRequest_ As New DataSet

            Try

                datasetRequest_ = conexion_.DataSetReciente.Copy()

                Return ConvertDataTableToJSON(environmentViewTableName_, datasetRequest_.Tables(0))

            Catch ex As Exception

                Return False

            End Try

        End Function

        Private Function ConvertDataTableToJSON(ByVal tableName_ As String,
                                                ByVal dataTable_ As DataTable) As Boolean

            Try

                Dim tableEnvironmentView_ As New List(Of TableEnviromentView)

                Dim singleTableEV_ As New TableEnviromentView

                With singleTableEV_

                    .TableName = tableName_

                End With

                For Each item_ As DataRow In dataTable_.Rows

                    Dim _itemInterfazEntorno As New ItemEnvironmentView

                    With _itemInterfazEntorno

                        .Nombre = item_("Nombre")
                        .Llave = item_("Llave")
                        .Longitud = item_("Longitud")
                        .TipoDato = item_("TipoDato")
                        .Visible = item_("Visible")
                        .NombreColumna = item_("NombreColumna")
                        .PuedeInsertar = item_("PuedeInsertar")
                        .PuedeModificar = item_("PuedeModificar")
                        .ValorDefault = item_("ValorDefault")

                        Dim tipoFiltro_ As Short = 0

                        If Not item_("TipoFiltro") Is Nothing And
                            Not item_("TipoFiltro") = "" Then
                            tipoFiltro_ = Convert.ToInt16(item_("TipoFiltro"))
                            .TipoFiltro = tipoFiltro_

                        Else
                            .TipoFiltro = 0

                        End If


                        Try

                            .Interfaz = item_("Interfaz")
                            .NameAsKey = item_("NameAsKey")

                        Catch ex As Exception

                            .Interfaz = Nothing
                            .NameAsKey = Nothing
                        End Try

                        Try

                            .PermisoConsulta = item_("PermisoConsulta")

                        Catch ex As Exception

                            .PermisoConsulta = Nothing

                        End Try

                        Try

                            .Reflejar = item_("Reflejar")

                        Catch ex As Exception

                            .Reflejar = Nothing

                        End Try
                        '.NumeroPermiso = item_("NumeroPermiso")

                    End With

                    singleTableEV_.ItemsOfTableEnvironmentView.Add(_itemInterfazEntorno)
                Next

                tableEnvironmentView_.Add(singleTableEV_)

                Dim serialJSON_ As New System.Runtime.Serialization.Json.DataContractJsonSerializer(tableEnvironmentView_.GetType)

                Dim memoryStream_ As New System.IO.MemoryStream

                serialJSON_.WriteObject(memoryStream_, tableEnvironmentView_)

                Dim textoJson As String = System.Text.Encoding.UTF8.GetString(memoryStream_.ToArray)

                Dim fileName_ As String = _mainDirectory & tableName_ & ".json"

                Dim objWriter_ As New System.IO.StreamWriter(fileName_)

                objWriter_.Write(textoJson)

                objWriter_.Close()

                Return True

            Catch ex As Exception

                Return False

            End Try

        End Function

        Private Function GetTextForOutput(ByVal filePath As String) As String
            ' Verify that the file exists.
            If My.Computer.FileSystem.FileExists(filePath) = False Then
                'Throw New Exception("File Not Found: " & filePath)
                Return Nothing

            End If

            ' Create a new StringBuilder, which is used
            ' to efficiently build strings.
            Dim sb As New System.Text.StringBuilder()

            ' Obtain file information.
            Dim thisFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(filePath)

            ' Add file attributes.
            'sb.Append("File: " & thisFile.FullName)
            'sb.Append(vbCrLf)
            'sb.Append("Modified: " & thisFile.LastWriteTime.ToString)
            'sb.Append(vbCrLf)
            'sb.Append("Size: " & thisFile.Length.ToString & " bytes")
            'sb.Append(vbCrLf)

            ' Open the text file.
            Dim sr As System.IO.StreamReader =
                My.Computer.FileSystem.OpenTextFileReader(filePath)

            ' Add the first line from the file.
            If sr.Peek() >= 0 Then
                'sb.Append("First Line: " & sr.ReadLine())
                sb.Append(sr.ReadLine())
            End If
            sr.Close()

            Return sb.ToString

        End Function

        Public Shared Function GetInstance() As EnvironmentViews

            If _instance Is Nothing Then

                _instance = New EnvironmentViews(LanguagueType.ES)

            End If

            Return _instance

        End Function

#End Region

    End Class


    Public Class ComponentView

#Region "Attributes"

        Private _tipo As String

        Private _vinculacion As String

        Private _auxiliares As List(Of ParAuxiliares)

        Private _clausulasLibres As String

        Private _campoDesplegar As String

        Private _campoLlave As String

        Private _valores As List(Of ParValores)

        Private _valoresString As List(Of ParValoresString)

        Private _valorDefault As String

        'Extension

        Private _mascara As String

        Private _titulo As String

        Private _obligatoria As String

        Private _tiempoRespuesta As String

        Private _validar As String

        Private _validaciones As List(Of ParValoresString)

        Private _numeroIntentos As String

        Private _id As String

        Private _visible As String

#End Region

#Region "Builders"

        Sub New()

            _tipo = Nothing

            _vinculacion = Nothing

            _clausulasLibres = Nothing

            _campoDesplegar = Nothing

            _campoLlave = Nothing

            _valores = New List(Of ParValores)

            _valoresString = New List(Of ParValoresString)

            _valorDefault = Nothing

            'extension

            _mascara = Nothing

            _titulo = Nothing

            _obligatoria = Nothing

            _tiempoRespuesta = Nothing

            _validar = Nothing

            _validaciones = New List(Of ParValoresString)

        End Sub

#End Region

#Region "Properties"

        '"Id":"5",
        Public Property Id As String
            Get
                Return _id
            End Get
            Set(value As String)
                _id = value
            End Set
        End Property

        '"Visible":"1",
        Public Property Visible As String
            Get
                Return _visible
            End Get
            Set(value As String)
                _visible = value
            End Set
        End Property

        '"NumeroIntentos":"1",
        Public Property NumeroIntentos As String
            Get
                Return _numeroIntentos
            End Get
            Set(value As String)
                _numeroIntentos = value
            End Set
        End Property

        '        _mascara = Nothing
        Public Property Mascara As String
            Get
                Return _mascara
            End Get
            Set(value As String)
                _mascara = value
            End Set
        End Property

        '_titulo = Nothing
        Public Property Titulo As String
            Get
                Return _titulo
            End Get
            Set(value As String)
                _titulo = value
            End Set
        End Property

        '_obligatoria = Nothing
        Public Property Obligatoria As String
            Get
                Return _obligatoria
            End Get
            Set(value As String)
                _obligatoria = value
            End Set
        End Property

        '_tiempoRespuesta = Nothing
        Public Property TiempoRespuesta As String
            Get
                Return _tiempoRespuesta
            End Get
            Set(value As String)
                _tiempoRespuesta = value
            End Set
        End Property

        '_validar = Nothing
        Public Property Validar As String
            Get
                Return _validar
            End Get
            Set(value As String)
                _validar = value
            End Set
        End Property

        '_validaciones = Nothing
        Public Property Validaciones As List(Of ParValoresString)
            Get
                Return _validaciones
            End Get
            Set(value As List(Of ParValoresString))
                _validaciones = value
            End Set
        End Property

        Public Property ValorDefault As String
            Get
                Return _valorDefault
            End Get
            Set(value As String)
                _valorDefault = value
            End Set
        End Property


        Public Property Tipo As String
            Get
                Return _tipo
            End Get
            Set(value As String)
                _tipo = value
            End Set

        End Property

        Public Property Vinculacion As String
            Get
                Return _vinculacion
            End Get
            Set(value As String)
                _vinculacion = value
            End Set

        End Property

        Public Property ClausulasLibres As String
            Get
                Return _clausulasLibres
            End Get
            Set(value As String)
                _clausulasLibres = value
            End Set

        End Property

        Public Property CampoDesplegar As String
            Get
                Return _campoDesplegar
            End Get
            Set(value As String)
                _campoDesplegar = value
            End Set

        End Property

        Public Property CampoLLave As String
            Get
                Return _campoLlave
            End Get
            Set(value As String)
                _campoLlave = value
            End Set

        End Property

        Public Property Valores As List(Of ParValores)

            Get
                Return _valores

            End Get

            Set(value As List(Of ParValores))

                _valores = value

            End Set

        End Property



        Public Property ValoresString As List(Of ParValoresString)

            Get
                Return _valoresString

            End Get

            Set(value As List(Of ParValoresString))

                _valoresString = value

            End Set

        End Property

        Public Property Auxiliares As List(Of ParAuxiliares)

            Get
                Return _auxiliares

            End Get

            Set(value As List(Of ParAuxiliares))

                _auxiliares = value

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    Public Class ParValoresString
        Public Descripcion As String
        Public Valor As String
    End Class

    Public Class ParValores
        Public Descripcion As String
        Public Valor As Int32
    End Class

    Public Class ParAuxiliares
        Public Campo As String
        Public Asignacion As String
    End Class

    Public Class TableEnviromentView

#Region "Attributes"

        Private _tableEnvironmentView As List(Of ItemEnvironmentView)

        Private _nameEnvironmentView As String


#End Region

#Region "Builders"

        Sub New()

            _tableEnvironmentView = New List(Of ItemEnvironmentView)

        End Sub

#End Region

#Region "Properties"

        Public Property TableName As String
            Get
                Return _nameEnvironmentView
            End Get
            Set(value As String)
                _nameEnvironmentView = value
            End Set

        End Property

        Public Property ItemsOfTableEnvironmentView As List(Of ItemEnvironmentView)

            Get
                Return _tableEnvironmentView

            End Get

            Set(value As List(Of ItemEnvironmentView))

                _tableEnvironmentView = value

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class


    'Public Interface IItemEnvironmentView

    '    Property Name As String

    '    Property Key As Int16

    '    Property Length As Int32

    '    Property DataType As Int16

    '    Property Visible As Int16

    '    Property ColumnName As String

    '    Property CanInsert As Int16

    '    Property CanModify As Int16

    '    Property DefaultValue As String

    '    Property FilterType As Int16

    '    Property PermissionID As Int32

    'End Interface


    'Public Interface IItemInterfazEntorno

    '    Property Nombre As String

    '    Property Llave As Int16

    '    Property Longitud As Int32

    '    Property TipoDato As Int16

    '    Property Visible As Int16

    '    Property NombreColumna As String

    '    Property PuedeInsertar As Int16

    '    Property PuedeModificar As Int16

    '    Property ValorDefault As String

    '    Property TipoFiltro As Int16

    '    Property NumeroPermiso As Int32

    '    Property NameAsKey As String

    '    Property Interfaz As String

    'End Interface

    Public Class ItemEnvironmentView
        '        Implements 
        'IItemEnvironmentView, 
        '            IItemInterfazEntorno


#Region "Enums"

#End Region

#Region "Attributes"

        Private _name As String

        Private _key As Int16

        Private _length As Int32

        Private _dataType As Int16

        Private _visible As Int16

        Private _columnName As String

        Private _canInsert As Int16

        Private _canModify As Int16

        Private _defaultValue As String

        Private _filterType As Int16

        Private _permissionID As Int32

        Private _nameAsKey As String

        Private _interfaz As String

        Private _permisoConsulta As String

        Private _mirror As Int16

#End Region

#Region "Builders"

        Sub New()

            _name = Nothing

            _key = 0

            _length = 0

            _dataType = 0

            _visible = 0

            _columnName = Nothing

            _canInsert = 0

            _canModify = 0

            _defaultValue = Nothing

            _filterType = 0

            _permissionID = 0

            _permisoConsulta = 118

            _mirror = 0

        End Sub

#End Region

#Region "Propierties"

        Property PermisoConsulta As String
            Get
                Return _permisoConsulta
            End Get
            Set(value As String)
                _permisoConsulta = value
            End Set
        End Property

        Property Nombre As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Property Llave As Int16
            Get
                Return _key
            End Get
            Set(value As Int16)
                _key = value
            End Set
        End Property

        Property Longitud As Integer
            Get
                Return _length
            End Get
            Set(value As Integer)
                _length = value
            End Set
        End Property

        Property TipoDato As Int16
            Get
                Return _dataType
            End Get
            Set(value As Int16)
                _dataType = value
            End Set
        End Property

        Property Visible As Int16
            Get
                Return _visible
            End Get
            Set(value As Int16)
                _visible = value
            End Set
        End Property

        Property NombreColumna As String
            Get
                Return _columnName
            End Get
            Set(value As String)
                _columnName = value
            End Set
        End Property

        Property PuedeInsertar As Int16
            Get
                Return _canInsert
            End Get
            Set(value As Int16)
                _canInsert = value
            End Set
        End Property

        Property PuedeModificar As Int16
            Get
                Return _canModify
            End Get
            Set(value As Int16)
                _canModify = value
            End Set
        End Property

        Property ValorDefault As String

            Get
                Return _defaultValue
            End Get
            Set(value As String)
                _defaultValue = value
            End Set
        End Property

        Property TipoFiltro As Int16
            Get
                Return _filterType
            End Get
            Set(value As Int16)
                _filterType = value
            End Set
        End Property

        Property NumeroPermiso As Integer
            Get
                Return _permissionID
            End Get
            Set(value As Integer)
                _permissionID = value
            End Set

        End Property

        Property NameAsKey As String
            Get
                Return _nameAsKey
            End Get
            Set(value As String)
                _nameAsKey = value
            End Set

        End Property

        Property Interfaz As String
            Get
                Return _interfaz
            End Get
            Set(value As String)
                _interfaz = value
            End Set

        End Property

        Property Reflejar As Int16

            Get

                Return _mirror

            End Get

            Set(value_ As Int16)

                _mirror = value_

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class


End Namespace

