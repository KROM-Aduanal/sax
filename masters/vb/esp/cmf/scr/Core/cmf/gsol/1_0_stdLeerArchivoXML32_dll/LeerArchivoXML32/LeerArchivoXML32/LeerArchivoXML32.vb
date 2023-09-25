Imports System.Xml
Imports gsol.seguridad
Imports System.Security.Cryptography
Imports System.Configuration
Imports MongoDB.Bson
Imports LanguageExt
Imports System.IO
Imports System.Reflection
Imports MongoDB.Bson.IO
Imports MongoDB.Bson.Serialization
Imports Wma.Exceptions

Imports System.Xml.Serialization
Imports System.Web
Imports System.Text
Imports System.Runtime.Serialization.Json
Imports gsol.krom

Namespace gsol

    Public Class LeerArchivoXML32
        Implements ILeerArchivo

#Region "Atributos"

        Private _rutaArchivo As String
        Private _propiedades As Dictionary(Of String, String)
        Private _encriptado As ILeerArchivo.TiposCifrado
        Private _llave As String
        Private _statusarchivo As ILeerArchivo.StatusArchivos

        Private _tipoArchivoAutomatico As ILeerArchivo.TiposAutomaticos = ILeerArchivo.TiposAutomaticos.OtroTipo

        Private _modalidadServicio As String
        Private _direccionIPServidorSQLServerGeneralProduccion As String
        Private _usuarioSQLServerGeneralProduccion As String
        Private _claveSQLServerGeneralProduccion As String
        Private _baseDatosSQLServerProduccion As String
        Private _oficina As String
        Private _direccionIPServidorMYSQLGeneralProduccion As String
        Private _usuarioMYSQLGeneralProduccion As String
        Private _claveMYSQLGeneralProduccion As String
        Private _baseDatosMYSQLProduccion As String
        Private _direccionServidorVFPGeneralProduccion As String
        Private _usuario As String
        Private _contrasena As String
        Private _grupoEmpresarial As String
        Private _aplicacion As String
        Private _endPoint As String
        Private _contrasenaWebService As String
        Private _configuracion As LeerArchivoXML32
        Private _status As TagWatcher
        Private _licenciaTagCode As String

        'nuevos elementos MOP 13/02/21

        Private _B1DireccionIPSQLServer As String
        Private _B1NombreSQLServer As String
        Private _B1UsuarioSQLServer As String
        Private _B1PasswordSQLServer As String

        Private _BDIDireccionIPSQLServer As String
        Private _BDINombreSQLServer As String
        Private _BDIUsuarioSQLServer As String
        Private _BDIPasswordSQLServer As String

        Private _BMDDireccionIPMongoDB As String
        Private _BMDNombreMongoDB As String
        Private _BMDUsuarioMongoDB As String
        Private _BMDPasswordMongoDB As String

        Private _ControlarBitacoraDesdeLlaveCentral As String
        Private _AplicacionInicial As String
        Private _ActivarLogGeneral As String
        Private _RutaLogGeneral As String
        Private _ActivarLogAdministrativo As String
        Private _RutaLogAdministrativo As String
        Private _ActivarBitacoraBasica As String
        Private _ActivarBitacoraGeneral As String

        Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

#End Region

#Region "Constructores"

        Sub New()

            _rutaArchivo = Nothing
            _propiedades = New Dictionary(Of String, String)
            _encriptado = ILeerArchivo.TiposCifrado.SinDefinir
            _llave = Nothing
            _statusarchivo = ILeerArchivo.StatusArchivos.SinDefinir

        End Sub

#End Region

#Region "Propiedades"

        Public Property TiposArchivosAutomaticos As ILeerArchivo.TiposAutomaticos _
            Implements ILeerArchivo.TipoArchivoAutomatico
            Get
                Return _tipoArchivoAutomatico
            End Get
            Set(value As ILeerArchivo.TiposAutomaticos)
                _tipoArchivoAutomatico = value
            End Set

        End Property

        Public ReadOnly Property Propiedades As System.Collections.Generic.Dictionary(Of String, String) _
            Implements ILeerArchivo.Propiedades
            Get
                Return _propiedades
            End Get
        End Property

        Public ReadOnly Property StatusArchivo As ILeerArchivo.StatusArchivos _
            Implements ILeerArchivo.StatusArchivo
            Get
                Return _statusarchivo
            End Get
        End Property

        WriteOnly Property RutaArchivo As String _
            Implements ILeerArchivo.RutaArchivo
            Set(ByVal value As String)
                _rutaArchivo = value
            End Set
        End Property

        Public Property Encriptado As ILeerArchivo.TiposCifrado _
            Implements ILeerArchivo.Encriptado
            Get
                Return _encriptado
            End Get
            Set(ByVal value As ILeerArchivo.TiposCifrado)
                _encriptado = value
            End Set
        End Property

        Public Property Llave As String _
            Implements ILeerArchivo.Llave
            Get
                Return _llave
            End Get
            Set(ByVal value As String)
                _llave = value
            End Set
        End Property
#End Region

#Region "Métodos"
        Public Sub ObtenerDatosJSON(ByVal token_ As String,
                                         Optional ByVal isCore_ As Boolean = True)

            With _statements

                Select Case token_

                'Core: database ip/host
                    Case "_DireccionIPServidorSQLServerGeneralProduccion"
                        _direccionIPServidorSQLServerGeneralProduccion =
                            .GetEndPoint("core", .GetRol("core", "master", "sql", "mssql").endpointId).ip

                        _propiedades.Add(token_, _direccionIPServidorSQLServerGeneralProduccion)

                    'Core: database user
                    Case "_UsuarioSQLServerGeneralProduccion"
                        _usuarioSQLServerGeneralProduccion =
                            .GetCredentials("core", .GetRol("core", "master", "sql", "mssql").credentialId).user

                        _propiedades.Add(token_, _usuarioSQLServerGeneralProduccion)

                    'Core: database password
                    Case "_ClaveSQLServerGeneralProduccion"
                        _claveSQLServerGeneralProduccion =
                            .GetCredentials("core", .GetRol("core", "master", "sql", "mssql").credentialId).password

                        _propiedades.Add(token_, _claveSQLServerGeneralProduccion)

                    'Core: database name
                    Case "_BaseDatosSQLServerProduccion"
                        _baseDatosSQLServerProduccion =
                            .GetRol("core", "master", "sql", "mssql").name

                        _propiedades.Add(token_, _baseDatosSQLServerProduccion)

                    'NA
                    Case "_Oficina" 'NA
                        _oficina = "NA"

                        _propiedades.Add(token_, _oficina)

                    'Core additional: database  ip/host
                    Case "_DireccionIPServidorMYSQLGeneralProduccion"
                        _direccionIPServidorMYSQLGeneralProduccion =
                            .GetEndPoint("core", .GetRol("core", "master", "sql", "mysql").endpointId).ip

                        _propiedades.Add(token_, _direccionIPServidorMYSQLGeneralProduccion)

                    'Core additional: user
                    Case "_UsuarioMYSQLGeneralProduccion"
                        _usuarioMYSQLGeneralProduccion =
                            .GetCredentials("core", .GetRol("core", "master", "sql", "mysql").credentialId).user

                        _propiedades.Add(token_, _usuarioMYSQLGeneralProduccion)

                    'Core additional: password
                    Case "_ClaveMYSQLGeneralProduccion"
                        _claveMYSQLGeneralProduccion =
                            .GetCredentials("core", .GetRol("core", "master", "sql", "mysql").credentialId).password

                        _propiedades.Add(token_, _claveMYSQLGeneralProduccion)


                    'Core additional: name
                    Case "_BaseDatosMYSQLProduccion"
                        _baseDatosMYSQLProduccion =
                            .GetRol("core", "master", "sql", "mysql").name

                        _propiedades.Add(token_, _baseDatosMYSQLProduccion)

                    Case "_DireccionServidorVFPGeneralProduccion"
                        _direccionServidorVFPGeneralProduccion = "NA"

                        _propiedades.Add(token_, _direccionServidorVFPGeneralProduccion)

                    Case "_Usuario"
                        _usuario = "NA"

                        _propiedades.Add(token_, _usuario)

                    Case "_Contrasena"
                        _contrasena = "NA"

                        _propiedades.Add(token_, _contrasena)

                    Case "_GrupoEmpresarial"
                        _grupoEmpresarial = "NA"

                        _propiedades.Add(token_, _grupoEmpresarial)

                    Case "_Aplicacion"
                        _aplicacion = "NA"

                        _propiedades.Add(token_, _aplicacion)

                    Case "_EndPoint"
                        _endPoint = "NA"

                        _propiedades.Add(token_, _endPoint)

                    Case "_ContrasenaWebService"
                        _contrasenaWebService = "NA"

                        _propiedades.Add(token_, _contrasenaWebService)

                    Case "_LicenciaTagCode"
                        '_configuracion As LeerArchivoXML32
                        '_status As TagWatcher
                        _licenciaTagCode = "WS19Fld3+b/JM72J+FuQaYLeB4iCUQDAklIEvRDprQtQTBKQtkQj4Q=="

                        _propiedades.Add(token_, _licenciaTagCode)

                    'Selected Project: IP, master, dimensional, log
                    Case "_B1DireccionIPSQLServer"
                        _B1DireccionIPSQLServer =
                            .GetEndPoint("project", .GetRol("project", "log", "sql", "mssql").endpointId).ip

                        _propiedades.Add(token_, _B1DireccionIPSQLServer)

                    'Selected Project: IP, master, dimensional, log
                    Case "_B1NombreSQLServer"
                        _B1NombreSQLServer =
                            .GetRol("project", "log", "sql", "mssql").name

                        _propiedades.Add(token_, _B1NombreSQLServer)

                    Case "_B1UsuarioSQLServer"
                        _B1UsuarioSQLServer =
                            .GetCredentials("project", .GetRol("project", "log", "sql", "mssql").credentialId).user

                        _propiedades.Add(token_, _B1UsuarioSQLServer)

                    Case "_B1PasswordSQLServer"
                        _B1PasswordSQLServer =
                            .GetCredentials("project", .GetRol("project", "log", "sql", "mssql").credentialId).password

                        _propiedades.Add(token_, _B1PasswordSQLServer)


                    Case "_BDIDireccionIPSQLServer"
                        _BDIDireccionIPSQLServer =
                            .GetEndPoint("project", .GetRol("project", "dimensional", "sql", "mssql").endpointId).ip

                        _propiedades.Add(token_, _BDIDireccionIPSQLServer)

                    Case "_BDINombreSQLServer"
                        _BDINombreSQLServer =
                            .GetRol("project", "dimensional", "sql", "mssql").name

                        _propiedades.Add(token_, _BDINombreSQLServer)

                    Case "_BDIUsuarioSQLServer"
                        _BDIUsuarioSQLServer =
                            .GetCredentials("project", .GetRol("project", "dimensional", "sql", "mssql").credentialId).user

                        _propiedades.Add(token_, _BDIUsuarioSQLServer)

                    Case "_BDIPasswordSQLServer"
                        _BDIPasswordSQLServer =
                            .GetCredentials("project", .GetRol("project", "dimensional", "sql", "mssql").credentialId).password

                        _propiedades.Add(token_, _BDIPasswordSQLServer)

                    Case "_BMDDireccionIPMongoDB"
                        _BMDDireccionIPMongoDB =
                            .GetEndPoint("project", .GetRol("project", "bigdata", "nosql", "mongodb").endpointId).ip

                        _propiedades.Add(token_, _BMDDireccionIPMongoDB)

                    Case "_BMDNombreMongoDB"
                        _BMDNombreMongoDB =
                            .GetRol("project", "bigdata", "nosql", "mongodb").name

                        _propiedades.Add(token_, _BMDNombreMongoDB)

                    Case "_BMDUsuarioMongoDB"
                        _BMDUsuarioMongoDB = .GetCredentials("project", .GetRol("project", "bigdata", "nosql", "mongodb").credentialId).user

                        _propiedades.Add(token_, _BMDUsuarioMongoDB)

                    Case "_BMDPasswordMongoDB"
                        _BMDPasswordMongoDB = .GetCredentials("project", .GetRol("project", "bigdata", "nosql", "mongodb").credentialId).password

                        _propiedades.Add(token_, _BMDPasswordMongoDB)

                    Case "_ControlarBitacoraDesdeLlaveCentral"

                        If (.GetSettings("core").logs IsNot Nothing) Then

                            _ControlarBitacoraDesdeLlaveCentral = CStr(IIf(.GetSettings("core").logs.logcontrol = True, 1, 0))

                            _propiedades.Add(token_, _ControlarBitacoraDesdeLlaveCentral.ToString)

                        End If

                    Case "_AplicacionInicial"

                        If (.GetSettings("core").globals IsNot Nothing) Then

                            _AplicacionInicial = .GetSettings("core").globals.startAppId.ToString

                            _propiedades.Add(token_, _AplicacionInicial)

                        End If

                    Case "_ActivarLogGeneral"
                        _ActivarLogGeneral = CStr(IIf(.GetSlot("core", "fsbasic1").activated = True, 1, 0))

                        _propiedades.Add(token_, _ActivarLogGeneral.ToString)

                    Case "_RutaLogGeneral"
                        _RutaLogGeneral = .GetSlot("core", "fsbasic1").pathlog

                        _propiedades.Add(token_, _RutaLogGeneral)

                    Case "_ActivarLogAdministrativo"
                        _ActivarLogAdministrativo = CStr(IIf(.GetSlot("core", "fsbasic2").activated = True, 1, 0))

                        _propiedades.Add(token_, _ActivarLogAdministrativo.ToString)

                    Case "_RutaLogAdministrativo"
                        _RutaLogAdministrativo = .GetSlot("core", "fsbasic2").pathlog

                        _propiedades.Add(token_, _RutaLogAdministrativo)

                    Case "_ActivarBitacoraBasica"
                        _ActivarBitacoraBasica = CStr(IIf(.GetSlot("core", "basic").activated = True, 1, 0))

                        _propiedades.Add(token_, _ActivarBitacoraBasica.ToString)

                    Case "_ActivarBitacoraGeneral"
                        _ActivarBitacoraGeneral = CStr(IIf(.GetSlot("core", "full").activated = True, 1, 0))

                        _propiedades.Add(token_, _ActivarBitacoraGeneral.ToString)

                End Select

            End With

        End Sub

        Sub CargarArchivo(ByVal tipoArchivoSistema_ As ILeerArchivo.TiposAutomaticos)

            Try

                Select Case tipoArchivoSistema_

                    Case ILeerArchivo.TiposAutomaticos.ConfiguracionCifradoInstancia
                        ObtenerDatosJSON("_TipoArchivoAutomatico")
                        ObtenerDatosJSON("_ModalidadServicio")
                        ObtenerDatosJSON("_DireccionIPServidorSQLServerGeneralProduccion")
                        ObtenerDatosJSON("_UsuarioSQLServerGeneralProduccion")
                        ObtenerDatosJSON("_ClaveSQLServerGeneralProduccion")
                        ObtenerDatosJSON("_BaseDatosSQLServerProduccion")
                        ObtenerDatosJSON("_Oficina")
                        ObtenerDatosJSON("_DireccionIPServidorMYSQLGeneralProduccion")
                        ObtenerDatosJSON("_UsuarioMYSQLGeneralProduccion")
                        ObtenerDatosJSON("_ClaveMYSQLGeneralProduccion")
                        ObtenerDatosJSON("_BaseDatosMYSQLProduccion")
                        ObtenerDatosJSON("_DireccionServidorVFPGeneralProduccion")
                        ObtenerDatosJSON("_Usuario")
                        ObtenerDatosJSON("_Contrasena")
                        ObtenerDatosJSON("_GrupoEmpresarial")
                        ObtenerDatosJSON("_Aplicacion")
                        ObtenerDatosJSON("_EndPoint")
                        ObtenerDatosJSON("_ContrasenaWebService")

                        ObtenerDatosJSON("_LicenciaTagCode")
                        'Nuevos elementos MOP 13/02/21
                        ObtenerDatosJSON("_B1DireccionIPSQLServer")
                        ObtenerDatosJSON("_B1NombreSQLServer")
                        ObtenerDatosJSON("_B1UsuarioSQLServer")
                        ObtenerDatosJSON("_B1PasswordSQLServer")

                        ObtenerDatosJSON("_BDIDireccionIPSQLServer")
                        ObtenerDatosJSON("_BDINombreSQLServer")
                        ObtenerDatosJSON("_BDIUsuarioSQLServer")
                        ObtenerDatosJSON("_BDIPasswordSQLServer")

                        ObtenerDatosJSON("_BMDDireccionIPMongoDB")
                        ObtenerDatosJSON("_BMDNombreMongoDB")
                        ObtenerDatosJSON("_BMDUsuarioMongoDB")
                        ObtenerDatosJSON("_BMDPasswordMongoDB")

                        ObtenerDatosJSON("_ControlarBitacoraDesdeLlaveCentral")
                        ObtenerDatosJSON("_AplicacionInicial")
                        ObtenerDatosJSON("_ActivarLogGeneral")
                        ObtenerDatosJSON("_RutaLogGeneral")
                        ObtenerDatosJSON("_ActivarLogAdministrativo")
                        ObtenerDatosJSON("_RutaLogAdministrativo")
                        ObtenerDatosJSON("_ActivarBitacoraBasica")
                        ObtenerDatosJSON("_ActivarBitacoraGeneral")

                    Case ILeerArchivo.TiposAutomaticos.ConfiguracionModulos

                        If _propiedades Is Nothing Or _propiedades.Count = 0 Then

                            If _statements.ProjectStructure Is Nothing Then
                                CargarArchivoAnterior(tipoArchivoSistema_)
                            ElseIf _statements.ProjectStructure.workspace.modules.Count = 0 Then
                                CargarArchivoAnterior(tipoArchivoSistema_)
                            End If


                            For Each module_ As Sax.projectmodule In _statements.ProjectStructure.workspace.modules

                                If Not module_.config.formname Is Nothing Then
                                    Dim recordKey1_ As String = "Modulo." & module_.tokenid : Dim recordContent1_ As String = module_.config.formname
                                    _propiedades.Add(recordKey1_, recordContent1_)
                                End If

                                If Not module_.config.assemblyname Is Nothing Then
                                    Dim recordKey2_ As String = "Ensamblado." & module_.tokenid : Dim recordContent2_ As String = "..\modules\" & module_.config.assemblyname
                                    _propiedades.Add(recordKey2_, recordContent2_)

                                    Dim recordKey3_ As String = "NombreEnsamblado." & module_.tokenid : Dim recordContent3_ As String = module_.config.assemblyname
                                    _propiedades.Add(recordKey3_, recordContent3_)

                                End If

                                If Not module_.config.EV Is Nothing Then
                                    Dim recordKey4_ As String = "VE." & module_.tokenid : Dim recordContent4_ As String = module_.config.EV
                                    _propiedades.Add(recordKey4_, recordContent4_)
                                End If


                                If Not module_.config.WV Is Nothing Then
                                    Dim recordKey5_ As String = "VT." & module_.tokenid : Dim recordContent5_ As String = module_.config.WV
                                    _propiedades.Add(recordKey5_, recordContent5_)
                                End If

                                If Not module_.config.keyfield Is Nothing Then
                                    Dim recordKey6_ As String = "CampoLlave." & module_.tokenid : Dim recordContent6_ As String = module_.config.keyfield
                                    _propiedades.Add(recordKey6_, recordContent6_)
                                End If

                                If Not module_.config.table Is Nothing Then
                                    Dim recordKey7_ As String = "TablaEdicion." & module_.tokenid : Dim recordContent7_ As String = module_.config.table
                                    _propiedades.Add(recordKey7_, recordContent7_)
                                End If

                                If Not module_.title Is Nothing Then
                                    Dim recordKey8_ As String = "Descripcion." & module_.tokenid : Dim recordContent8_ As String = module_.title
                                    _propiedades.Add(recordKey8_, recordContent8_)
                                End If

                                If Not module_.config.allenvironments Is Nothing Then
                                    Dim recordKey9_ As String = "Multiempresa." & module_.tokenid : Dim recordContent9_ As String = module_.config.allenvironments
                                    _propiedades.Add(recordKey9_, recordContent9_)
                                End If

                                If Not module_.datamirrors Is Nothing Then
                                    If module_.datamirrors.Count > 0 Then
                                        Dim recordKey19_ As String = "ReflejarEn." & module_.tokenid : Dim recordContent19_ As String = module_.datamirrors(0).database
                                        _propiedades.Add(recordKey19_, recordContent19_)
                                    End If
                                End If

                                If Not module_.dboperators Is Nothing Then
                                    If Not module_.dboperators.deleteoperator Is Nothing Then
                                        Dim recordKey10_ As String = "OperadorBorrado." & module_.tokenid : Dim recordContent10_ As String = module_.dboperators.deleteoperator
                                        _propiedades.Add(recordKey10_, NombreOperadorAuto("Eliminacion", recordContent10_))
                                    End If
                                    If Not module_.dboperators.insertoperator Is Nothing Then

                                        Dim recordKey11_ As String = "OperadorInserciones." & module_.tokenid
                                        Dim recordContent11_ As String = module_.dboperators.insertoperator

                                        _propiedades.Add(recordKey11_, NombreOperadorAuto("Insercion", recordContent11_))
                                    End If
                                    If Not module_.dboperators.updateoperator Is Nothing Then
                                        Dim recordKey12_ As String = "OperadorModificaciones." & module_.tokenid : Dim recordContent12_ As String = module_.dboperators.updateoperator
                                        _propiedades.Add(recordKey12_, NombreOperadorAuto("Modificacion", recordContent12_))
                                    End If
                                End If

                                If Not module_.permissions Is Nothing Then
                                    Dim recordKey13_ As String = "PermisoActualizar." & module_.tokenid : Dim recordContent13_ As String = module_.permissions.refresh

                                    If LCase(recordContent13_) = "auto" Then : recordContent13_ = "118" : End If
                                    _propiedades.Add(recordKey13_, recordContent13_)

                                    Dim recordKey14_ As String = "PermisoInsertar." & module_.tokenid : Dim recordContent14_ As String = module_.permissions.insert
                                    If LCase(recordContent14_) = "auto" Then : recordContent14_ = "118" : End If
                                    _propiedades.Add(recordKey14_, recordContent14_)

                                    Dim recordKey15_ As String = "PermisoBuscar." & module_.tokenid : Dim recordContent15_ As String = module_.permissions.query
                                    If LCase(recordContent15_) = "auto" Then : recordContent15_ = "118" : End If
                                    _propiedades.Add(recordKey15_, recordContent15_)

                                    Dim recordKey16_ As String = "PermisoEliminar." & module_.tokenid : Dim recordContent16_ As String = module_.permissions.delete
                                    If LCase(recordContent16_) = "auto" Then : recordContent16_ = "118" : End If
                                    _propiedades.Add(recordKey16_, recordContent16_)

                                    Dim recordKey17_ As String = "PermisoModificar." & module_.tokenid : Dim recordContent17_ As String = module_.permissions.update
                                    If LCase(recordContent17_) = "auto" Then : recordContent17_ = "118" : End If
                                    _propiedades.Add(recordKey17_, recordContent17_)
                                End If

                                If Not module_.operatorform Is Nothing Then
                                    If Not module_.operatorform.formname Is Nothing Then
                                        Dim recordKey18_ As String = "TipoEnsambladoGestor." & module_.tokenid : Dim recordContent18_ As String = module_.operatorform.formname
                                        _propiedades.Add(recordKey18_, recordContent18_)
                                    End If

                                    If Not module_.operatorform.assembly Is Nothing Then
                                        Dim recordKey19_ As String = "ArchivoGestor." & module_.tokenid
                                        'bautismen
                                        Dim recordContent19_ As String = ".\" & _statements.SaxProjects.projects(0).modulesbinaries & "\" & module_.operatorform.assembly

                                        recordContent19_ = recordContent19_.Replace("\\", "\")

                                        _propiedades.Add(recordKey19_, recordContent19_)
                                    End If

                                    If Not module_.operatorform.showsinglewindow.ToString Is Nothing Then
                                        Dim recordKey20_ As String = "VentanasIndependientes." & module_.tokenid
                                        Dim recordContent20_ As String
                                        If module_.operatorform.showsinglewindow = False Then
                                            recordContent20_ = "true"
                                        Else
                                            recordContent20_ = "false"
                                        End If

                                        _propiedades.Add(recordKey20_, recordContent20_)
                                    End If

                                    If Not module_.operatorform.cancontrolsw.ToString Is Nothing Then
                                        Dim recordKey21_ As String = "ActivarControlVentanasIndependientes." & module_.tokenid : Dim recordContent21_ As String = module_.operatorform.cancontrolsw.ToString
                                        _propiedades.Add(recordKey21_, recordContent21_)
                                    End If
                                End If


                                Dim recordKey22_ As String = "Version." & module_.tokenid : Dim recordContent22_ As String = module_.version
                                _propiedades.Add(recordKey22_, recordContent22_)

                                If Not module_.lnksets Is Nothing Then

                                    Dim recordKey23_ As String = "Granularidad." & module_.tokenid : Dim recordContent23_ As String = module_.lnksets.granularity
                                    _propiedades.Add(recordKey23_, recordContent23_)

                                    Dim recordKey24_ As String = "Dimension." & module_.tokenid : Dim recordContent24_ As String = module_.lnksets.dimension
                                    _propiedades.Add(recordKey24_, recordContent24_)

                                    Dim recordKey25_ As String = "Entidad." & module_.tokenid : Dim recordContent25_ As String = module_.lnksets.entity
                                    _propiedades.Add(recordKey25_, recordContent25_)

                                End If

                                If Not module_.config.upsertkeyname Is Nothing Then
                                    Dim recordKey26_ As String = "NombreClaveUpsert." & module_.tokenid : Dim recordContent26_ As String = module_.config.upsertkeyname
                                    _propiedades.Add(recordKey26_, recordContent26_)
                                End If

                                If Not module_.customcolumns Is Nothing Then
                                    For Each column_ As Sax.customcolumns In module_.customcolumns
                                        Dim recordKey27_ As String = "MarcadorColumna" & column_._id & "." & module_.tokenid : Dim recordContent27_ As String = column_.cond
                                        _propiedades.Add(recordKey27_, recordContent27_)
                                    Next
                                End If

                                If Not module_.datasource Is Nothing Then
                                    Dim recordKey28_ As String = "OrigenDatos." & module_.tokenid : Dim recordContent28_ As String = module_.datasource
                                    _propiedades.Add(recordKey28_, recordContent28_)
                                End If

                                If Not module_.alternativedatasource Is Nothing Then
                                    Dim recordKey29_ As String = "OrigenDatosAlternativo." & module_.tokenid : Dim recordContent29_ As String = module_.alternativedatasource
                                    _propiedades.Add(recordKey29_, recordContent29_)
                                End If

                            Next

                        End If

                End Select

                _tipoArchivoAutomatico = tipoArchivoSistema_

                _statusarchivo = ILeerArchivo.StatusArchivos.Cargado

            Catch ex As Exception

                _tipoArchivoAutomatico = ILeerArchivo.TiposAutomaticos.OtroTipo

                _statusarchivo = ILeerArchivo.StatusArchivos.ConError

            End Try

        End Sub

        Function NombreOperadorAuto(ByVal origen_ As String,
                                    ByVal nombreOperador_ As String) As String

            Select Case origen_
                Case "Insercion"
                    Select Case nombreOperador_
                        Case "auto" : Return "Sp000OperadorCatalogosInserciones"
                        Case "automax" : Return "Sp000OperadorCatalogosInsercionesMax"
                    End Select
                Case "Modificacion"
                    Select Case nombreOperador_
                        Case "auto" : Return "Sp000OperadorCatalogosModificaciones"
                        Case "automax" : Return "Sp000OperadorCatalogosModificacionesMax"
                    End Select
                Case "Eliminacion"
                    Select Case nombreOperador_
                        Case "auto" : Return "Sp000OperadorCatalogosBorrado"
                    End Select
            End Select

            Return nombreOperador_

        End Function
        Sub CargarArchivoAnterior(ByVal tipoArchivoSistema_ As ILeerArchivo.TiposAutomaticos)

            Dim listaAuxiliar_ As New Dictionary(Of String, String)

            _rutaArchivo = ".\..\settings\ConfiguracionGeneralModulos.xml"

            _encriptado = ILeerArchivo.TiposCifrado.SinDefinir


            If Not (File.Exists(_rutaArchivo)) Then

                Dim tagwatcher_ As New TagWatcher

                tagwatcher_.SetError()

                Exit Sub

            End If

            Try

                Dim xmlDoc_ As New XmlDocument

                Dim elementoRaiz_ As XmlElement

                Dim coleccionNodos_ As XmlNodeList

                xmlDoc_.PreserveWhitespace = False

                xmlDoc_.Load(_rutaArchivo)

                If _encriptado <> ILeerArchivo.TiposCifrado.SinDefinir Then

                    Dim cifrado_ As ICifrado = New Cifrado256

                    Dim metodo_ As SymmetricAlgorithm

                    If _encriptado = ILeerArchivo.TiposCifrado.RijndaelManaged Then

                        metodo_ = New RijndaelManaged

                        cifrado_.DescifraCadena(xmlDoc_.InnerText, metodo_, _llave)

                    End If

                    xmlDoc_.LoadXml(cifrado_.Cadena)

                End If

                elementoRaiz_ = xmlDoc_.DocumentElement

                coleccionNodos_ = elementoRaiz_.ChildNodes

                For Each nodo_ As XmlNode In coleccionNodos_

                    If TypeOf nodo_ Is XmlComment Then Continue For

                    If Not nodo_.Attributes Is Nothing Then

                        For Each atributo_ As XmlAttribute In nodo_.Attributes

                            If Not listaAuxiliar_.ContainsKey(atributo_.Name) Then

                                listaAuxiliar_.Add(atributo_.Name, atributo_.Value)

                            Else

                                MsgBox("El Atributo {" & atributo_.Name & "} existe mas de una vez en el archivo de configuración, se omitiran las replicas, por favor verifique!")

                            End If


                        Next

                    Else

                        _statusarchivo = ILeerArchivo.StatusArchivos.ConError

                        MsgBox("El Nodo no tiene datos.. (Error fatal!)")

                    End If

                Next

                _tipoArchivoAutomatico = tipoArchivoSistema_

                _statusarchivo = ILeerArchivo.StatusArchivos.Cargado
                'La lista generada construirá el json nuevo para la configuración de los mods

                Dim listaord_ = (From entry In listaAuxiliar_ Order By entry.Value Ascending).ToDictionary(Function(pair) pair.Key, Function(pair) pair.Value)

                Dim projectstructure_ As New Sax.projectstructure

                With projectstructure_

                    .about = "Krombase"

                    .version = "5.0.0.0"

                    .scope = "project"

                    'all modules
                    .workspace = New Sax.workspace With {.modules = New List(Of Sax.projectmodule)}

                    'Setting default values
                    .workspace.defaults = New Sax.projectmodule With {
                        ._id = 0,
                        .tokenid = Nothing,
                        .title = "New module",
                        .type = "module",
                        .version = "0.0.0.0",
                        .activated = True,
                        .autosets = False,
                        .info = "",
                        .config = New Sax.projectconfig With {
                                .formname = "Gsol.Modulos.GenericCatalog64.Frm000Generic",
                                .assemblyname = "Gsol.Modulos.GenericCatalog64.1.0.0.0.dll",
                                .EV = Nothing,
                                .WV = Nothing,
                                .keyfield = Nothing,
                                .table = Nothing,
                                .allenvironments = "-1",
                                .upsertkeyname = Nothing},
                         .dboperators = New Sax.dboperators With {
                                .deleteoperator = "Sp000OperadorCatalogosBorrado",
                                .insertoperator = "Sp000OperadorCatalogosInserciones",
                                .updateoperator = "Sp000OperadorCatalogosModificaciones"
                         },
                         .permissions = New Sax.permissions With {
                                 .refresh = "118",
                                 .insert = "118",
                                 .query = "118",
                                 .delete = "118",
                                 .update = "118"
                         },
                         .operatorform = New Sax.operatorform With {
                                .formname = "Gsol.Modulos.GenericOperators64.frm000Formulario",
                                .assembly = "Gsol.Modulos.GenericOperators64.1.0.0.0.dll",
                                .showsinglewindow = False,
                                .cancontrolsw = True
                         },
                         .lnksets = New Sax.lnksets With {
                                .granularity = Nothing,
                                .dimension = "auto",
                                .entity = Nothing
                    }}



                End With

                Dim listaUnicaTokens_ As New List(Of String)

                Dim tokenword_ As String = Nothing

                For Each parDatos_ As KeyValuePair(Of String, String) In listaord_

                    If Not tokenword_ = parDatos_.Key Then

                        tokenword_ = parDatos_.Key

                        Dim tokenarr1_ As String() = tokenword_.Split(CChar("."))

                        'Token único
                        If Not listaUnicaTokens_.Contains(tokenarr1_(1)) Then
                            listaUnicaTokens_.Add(tokenarr1_(1))
                        End If

                    End If

                Next

                Dim idx_ As Int32 = 0

                For Each keyword_ As String In listaUnicaTokens_

                    Dim module_ As New Sax.projectmodule

                    module_.config = New Sax.projectconfig

                    If (listaAuxiliar_.ContainsKey("OperadorInserciones." & keyword_) Or
                        listaAuxiliar_.ContainsKey("OperadorModificaciones." & keyword_) Or
                        listaAuxiliar_.ContainsKey("OperadorInsercionesMax." & keyword_) Or
                        listaAuxiliar_.ContainsKey("OperadorModificacionesMax." & keyword_) Or
                        listaAuxiliar_.ContainsKey("OperadorBorrado." & keyword_)) Then

                        module_.dboperators = New Sax.dboperators

                    End If

                    If (listaAuxiliar_.ContainsKey("PermisoActualizar." & keyword_) Or
                        listaAuxiliar_.ContainsKey("PermisoInsertar." & keyword_) Or
                        listaAuxiliar_.ContainsKey("PermisoBuscar." & keyword_) Or
                        listaAuxiliar_.ContainsKey("PermisoEliminar." & keyword_) Or
                        listaAuxiliar_.ContainsKey("PermisoModificar." & keyword_)) Then

                        module_.permissions = New Sax.permissions

                    End If

                    If listaAuxiliar_.ContainsKey("TipoEnsambladoGestor." & keyword_) Or
                        listaAuxiliar_.ContainsKey("ArchivoGestor." & keyword_) Then

                        module_.operatorform = New Sax.operatorform

                    End If

                    If listaAuxiliar_.ContainsKey("MarcadorColumna1." & keyword_) Or
                        listaAuxiliar_.ContainsKey("MarcadorColumna2." & keyword_) Or
                        listaAuxiliar_.ContainsKey("MarcadorColumna3." & keyword_) Or
                        listaAuxiliar_.ContainsKey("MarcadorColumna4." & keyword_) Then

                        module_.customcolumns = New List(Of Sax.customcolumns)

                    End If

                    If listaAuxiliar_.ContainsKey("Granularidad." & keyword_) Or
                        listaAuxiliar_.ContainsKey("Dimension." & keyword_) Or
                        listaAuxiliar_.ContainsKey("Entidad." & keyword_) Then

                        module_.lnksets = New Sax.lnksets

                    End If

                    With module_
                        ._id = idx_
                        .tokenid = keyword_
                        If (listaAuxiliar_.ContainsKey("Descripcion." & keyword_)) Then : .title = listaAuxiliar_("Descripcion." & keyword_) : End If
                        .type = "generated"
                        If (listaAuxiliar_.ContainsKey("Version." & keyword_)) Then : .version = listaAuxiliar_("Version." & keyword_) : End If
                        .activated = True
                        .autosets = False
                        If (listaAuxiliar_.ContainsKey("Descripcion." & keyword_)) Then : .info = listaAuxiliar_("Descripcion." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("Modulo." & keyword_)) Then : .config.formname = listaAuxiliar_("Modulo." & keyword_) : End If

                        If (listaAuxiliar_.ContainsKey("Ensamblado." & keyword_)) Then

                            Dim path_ As String() = listaAuxiliar_("Ensamblado." & keyword_).Split(CChar("\"))

                            If UBound(path_) <> 0 AndAlso UBound(path_) <> -1 Then
                                .config.assemblyname = path_(UBound(path_))
                            End If


                        End If

                        If (listaAuxiliar_.ContainsKey("VE." & keyword_)) Then : .config.EV = listaAuxiliar_("VE." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("VT." & keyword_)) Then : .config.WV = listaAuxiliar_("VT." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("CampoLlave." & keyword_)) Then : .config.keyfield = listaAuxiliar_("CampoLlave." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("TablaEdicion." & keyword_)) Then : .config.table = listaAuxiliar_("TablaEdicion." & keyword_) : End If

                        If (listaAuxiliar_.ContainsKey("Multiempresa." & keyword_)) Then : .config.allenvironments = listaAuxiliar_("Multiempresa." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("NombreClaveUpsert." & keyword_)) Then : .config.upsertkeyname = listaAuxiliar_("NombreClaveUpsert." & keyword_) : End If

                        If (listaAuxiliar_.ContainsKey("OperadorBorrado." & keyword_)) Then

                            Dim operator_ As String = listaAuxiliar_("OperadorBorrado." & keyword_)

                            If LCase(operator_) = "sp000operadorcatalogosborrado" Then

                                .dboperators.deleteoperator = "auto"
                            Else

                                .dboperators.deleteoperator = listaAuxiliar_("OperadorBorrado." & keyword_)

                            End If

                        End If

                        If (listaAuxiliar_.ContainsKey("OperadorInserciones." & keyword_)) Then

                            Dim operator_ As String = listaAuxiliar_("OperadorInserciones." & keyword_)

                            If LCase(operator_) = "sp000operadorcatalogosinserciones" Then

                                .dboperators.insertoperator = "auto"

                            ElseIf LCase(operator_) = "sp000operadorcatalogosinsercionesmax" Then

                                .dboperators.insertoperator = "automax"

                            Else

                                .dboperators.insertoperator = listaAuxiliar_("OperadorInserciones." & keyword_)

                            End If

                        End If

                        If (listaAuxiliar_.ContainsKey("OperadorModificaciones." & keyword_)) Then

                            Dim operator_ As String = listaAuxiliar_("OperadorModificaciones." & keyword_)

                            If LCase(operator_) = "sp000operadorcatalogosmodificaciones" Then

                                .dboperators.updateoperator = "auto"
                            ElseIf LCase(operator_) = "sp000operadorcatalogosmodificaciones" Then

                                .dboperators.updateoperator = "automax"

                            Else

                                .dboperators.updateoperator = listaAuxiliar_("OperadorModificaciones." & keyword_)

                            End If

                        End If

                        If (listaAuxiliar_.ContainsKey("PermisoActualizar." & keyword_)) Then
                            .permissions.refresh = listaAuxiliar_("PermisoActualizar." & keyword_)
                            If .permissions.refresh = "118" Then : .permissions.refresh = "auto" : End If
                        End If
                        If (listaAuxiliar_.ContainsKey("PermisoInsertar." & keyword_)) Then
                            .permissions.insert = listaAuxiliar_("PermisoInsertar." & keyword_)
                            If .permissions.insert = "118" Then : .permissions.insert = "auto" : End If
                        End If
                        If (listaAuxiliar_.ContainsKey("PermisoBuscar." & keyword_)) Then
                            .permissions.query = listaAuxiliar_("PermisoBuscar." & keyword_)
                            If .permissions.query = "118" Then : .permissions.query = "auto" : End If
                        End If
                        If (listaAuxiliar_.ContainsKey("PermisoEliminar." & keyword_)) Then
                            .permissions.delete = listaAuxiliar_("PermisoEliminar." & keyword_)
                            If .permissions.delete = "118" Then : .permissions.delete = "auto" : End If
                        End If
                        If (listaAuxiliar_.ContainsKey("PermisoModificar." & keyword_)) Then
                            .permissions.update = listaAuxiliar_("PermisoModificar." & keyword_)
                            If .permissions.update = "118" Then : .permissions.update = "auto" : End If
                        End If

                        If (listaAuxiliar_.ContainsKey("TipoEnsambladoGestor." & keyword_)) Then
                            .operatorform.formname = listaAuxiliar_("TipoEnsambladoGestor." & keyword_)

                            If LCase(.operatorform.formname) = "gsol.modulos.genericcatalog64.frm000generic" Then
                                .operatorform.formname = "auto"
                                .operatorform.assembly = "auto"
                            End If

                            .type = "module"
                        Else
                            .type = "script"

                        End If
                        If (listaAuxiliar_.ContainsKey("ArchivoGestor." & keyword_)) Then

                            Dim path_ As String() = listaAuxiliar_("ArchivoGestor." & keyword_).Split(CChar("\"))

                            If UBound(path_) <> 0 AndAlso UBound(path_) <> -1 Then
                                .operatorform.assembly = path_(UBound(path_))
                            End If


                        End If

                        If (listaAuxiliar_.ContainsKey("VentanasIndependientes." & keyword_)) Then
                            Dim respuesta_ As String = listaAuxiliar_("VentanasIndependientes." & keyword_)
                            If LCase(respuesta_) = "true" Then
                                .operatorform.showsinglewindow = True
                            Else
                                .operatorform.showsinglewindow = False
                            End If

                        End If
                        If (listaAuxiliar_.ContainsKey("ActivarControlVentanasIndependientes." & keyword_)) Then
                            Dim respuesta_ As String = listaAuxiliar_("ActivarControlVentanasIndependientes." & keyword_)
                            If LCase(respuesta_) = "true" Then
                                .operatorform.cancontrolsw = True
                            Else
                                .operatorform.cancontrolsw = False
                            End If
                        End If

                        If (listaAuxiliar_.ContainsKey("Granularidad." & keyword_)) Then : .lnksets.granularity = listaAuxiliar_("Granularidad." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("Dimension." & keyword_)) Then : .lnksets.dimension = listaAuxiliar_("Dimension." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("Entidad." & keyword_)) Then : .lnksets.entity = listaAuxiliar_("Entidad." & keyword_) : End If
                        If (listaAuxiliar_.ContainsKey("MarcadorColumna1." & keyword_)) Then
                            .customcolumns.Add(New Sax.customcolumns With {._id = "1", .cond = listaAuxiliar_("MarcadorColumna1." & keyword_)})
                        End If
                        If (listaAuxiliar_.ContainsKey("MarcadorColumna2." & keyword_)) Then
                            .customcolumns.Add(New Sax.customcolumns With {._id = "2", .cond = listaAuxiliar_("MarcadorColumna2." & keyword_)})
                        End If
                        If (listaAuxiliar_.ContainsKey("MarcadorColumna3." & keyword_)) Then
                            .customcolumns.Add(New Sax.customcolumns With {._id = "3", .cond = listaAuxiliar_("MarcadorColumna3." & keyword_)})
                        End If
                        If (listaAuxiliar_.ContainsKey("MarcadorColumna4." & keyword_)) Then
                            .customcolumns.Add(New Sax.customcolumns With {._id = "4", .cond = listaAuxiliar_("MarcadorColumna4." & keyword_)})
                        End If

                        If (listaAuxiliar_.ContainsKey("OrigenDatos." & keyword_)) Then : .datasource = listaAuxiliar_("OrigenDatos." & keyword_) : End If

                        If (listaAuxiliar_.ContainsKey("OrigenDatosAlternativo." & keyword_)) Then : .alternativedatasource = listaAuxiliar_("OrigenDatosAlternativo." & keyword_) : End If

                    End With

                    With projectstructure_

                        .workspace.modules.Add(module_)

                    End With

                    idx_ += 1

                Next

                'Entregamos los módulos al singleton
                _statements.ProjectStructure = New Sax.projectstructure With {.workspace = New Sax.workspace With {.modules = New List(Of Sax.projectmodule)}}

                _statements.ProjectStructure.workspace.modules = projectstructure_.workspace.modules

                '***** Obtenemos el nombre asignado del archivo de este proyecto por la configuración ***

                Dim filename_ As String = "noname.json"

                For Each project_ As Sax.project In _statements.SaxProjects.projects

                    If project_.online = True Then

                        filename_ = project_.modulessettings

                        Exit For

                    End If

                Next

                '***** Guardando objeto importado en JSON format *******************

                Dim serialJSON_ As New DataContractJsonSerializer(projectstructure_.GetType)

                Dim memoryStream_ As New System.IO.MemoryStream

                serialJSON_.WriteObject(memoryStream_, projectstructure_)

                Dim textoJson As String = System.Text.Encoding.UTF8.GetString(memoryStream_.ToArray)

                Dim objWriter_ As New System.IO.StreamWriter(filename_)

                objWriter_.Write(textoJson)

                objWriter_.Close()

            Catch ex As Exception

                _tipoArchivoAutomatico = ILeerArchivo.TiposAutomaticos.OtroTipo

                _statusarchivo = ILeerArchivo.StatusArchivos.ConError

            End Try


        End Sub

        Sub CargarArchivoObsoleto(ByVal tipoArchivoSistema_ As ILeerArchivo.TiposAutomaticos)

            Try

                Dim xmlDoc_ As New XmlDocument

                Dim elementoRaiz_ As XmlElement

                Dim coleccionNodos_ As XmlNodeList

                xmlDoc_.PreserveWhitespace = False

                xmlDoc_.Load(_rutaArchivo)

                If _encriptado <> ILeerArchivo.TiposCifrado.SinDefinir Then

                    Dim cifrado_ As ICifrado = New Cifrado256

                    Dim metodo_ As SymmetricAlgorithm

                    If _encriptado = ILeerArchivo.TiposCifrado.RijndaelManaged Then

                        metodo_ = New RijndaelManaged

                        cifrado_.DescifraCadena(xmlDoc_.InnerText, metodo_, _llave)

                    End If

                    xmlDoc_.LoadXml(cifrado_.Cadena)

                End If

                elementoRaiz_ = xmlDoc_.DocumentElement

                coleccionNodos_ = elementoRaiz_.ChildNodes

                For Each nodo_ As XmlNode In coleccionNodos_

                    If TypeOf nodo_ Is XmlComment Then Continue For

                    If Not nodo_.Attributes Is Nothing Then

                        For Each atributo_ As XmlAttribute In nodo_.Attributes

                            If Not _propiedades.ContainsKey(atributo_.Name) Then

                                _propiedades.Add(atributo_.Name, atributo_.Value)

                            Else

                                MsgBox("El Atributo {" & atributo_.Name & "} existe mas de una vez en el archivo de configuración, se omitiran las replicas, por favor verifique!")

                            End If


                        Next

                    Else

                        _statusarchivo = ILeerArchivo.StatusArchivos.ConError

                        MsgBox("El Nodo no tiene datos.. (Error fatal!)")

                    End If

                Next

                _tipoArchivoAutomatico = tipoArchivoSistema_

                _statusarchivo = ILeerArchivo.StatusArchivos.Cargado

            Catch ex As Exception

                _tipoArchivoAutomatico = ILeerArchivo.TiposAutomaticos.OtroTipo

                _statusarchivo = ILeerArchivo.StatusArchivos.ConError

            End Try

        End Sub

        Public Sub LeerXML(ByVal tipoArchivoSistema_ As ILeerArchivo.TiposAutomaticos) _
            Implements ILeerArchivo.LeerXML

            CargarArchivo(tipoArchivoSistema_)

        End Sub

        Public Sub LeerXML() _
            Implements ILeerArchivo.LeerXML

            CargarArchivo(ILeerArchivo.TiposAutomaticos.ConfiguracionCifradoInstancia)

        End Sub
        Public Sub LeerXMLObsoleto()

            ' las cosas buenas son solo el principio, el principio del fin, Federico Nietszche, Pedro BM
            Dim xmlDoc_ As New XmlDocument

            Dim elementoRaiz_ As XmlElement

            Dim coleccionNodos_ As XmlNodeList

            xmlDoc_.PreserveWhitespace = False

            xmlDoc_.Load(_rutaArchivo)

            If _encriptado <> ILeerArchivo.TiposCifrado.SinDefinir Then

                Dim cifrado_ As ICifrado = New Cifrado256

                Dim metodo_ As SymmetricAlgorithm

                If _encriptado = ILeerArchivo.TiposCifrado.RijndaelManaged Then

                    metodo_ = New RijndaelManaged

                    cifrado_.DescifraCadena(xmlDoc_.InnerText, metodo_, _llave)

                End If

                xmlDoc_.LoadXml(cifrado_.Cadena)

            End If

            elementoRaiz_ = xmlDoc_.DocumentElement

            coleccionNodos_ = elementoRaiz_.ChildNodes

            For Each nodo_ As XmlNode In coleccionNodos_

                If TypeOf nodo_ Is XmlComment Then Continue For

                If Not nodo_.Attributes Is Nothing Then

                    For Each atributo_ As XmlAttribute In nodo_.Attributes

                        If Not _propiedades.ContainsKey(atributo_.Name) Then

                            _propiedades.Add(atributo_.Name, atributo_.Value)

                        Else

                            MsgBox("El Atributo {" & atributo_.Name & "} existe mas de una vez en el archivo de configuración, se omitiran las replicas, por favor verifique!")

                        End If


                    Next

                Else

                    _statusarchivo = ILeerArchivo.StatusArchivos.ConError

                    MsgBox("El Nodo no tiene datos.. (Error fatal!)")

                End If

            Next

            _statusarchivo = ILeerArchivo.StatusArchivos.Cargado

        End Sub

        Public Function RegresaValor(
            ByVal atributo_ As String
        ) As String _
            Implements ILeerArchivo.RegresaValor


            If _propiedades.ContainsKey(atributo_) Then

                Return Propiedades(atributo_)

            Else

                Return Nothing

            End If

        End Function

#End Region

    End Class

End Namespace

