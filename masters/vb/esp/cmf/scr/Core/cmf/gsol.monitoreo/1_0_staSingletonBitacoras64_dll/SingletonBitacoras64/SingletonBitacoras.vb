Imports Wma.Exceptions
'Imports System.Data.SqlClient
Imports gsol.seguridad
Imports System.Data.SqlClient

Public Class SingletonBitacoras

#Region "Atributos"
    ' Nuevos atributos

    Private _i_controlarBitacoraDesdeLlaveCentral As Int32 = 0

    ' Singleton configuración central
    Public Shared _singletonConfiguracion As Configuracion = Nothing

    Private Shared _instancia As SingletonBitacoras = Nothing

    Private _i_Cve_Aplicacion As Int32

    Private _i_ActivarLogGeneral As Boolean
    Private _t_RutaLogGeneral As String

    Private _i_ActivarLogAdministrativo As Boolean
    Private _t_RutaLogAdministrativo As String

    'Básica
    Private _i_ActivarBitacoraBasica As Boolean
    'Avanzada
    Private _i_ActivarBitacoraGeneral As Boolean

#End Region

#Region "Constructores"

    Sub New(ByVal i_Cve_Aplicacion_ As Int32)
        '******* MOP, nuevas caracteristicas 2021

        _i_controlarBitacoraDesdeLlaveCentral = 0


        '**********************

        _i_Cve_Aplicacion = i_Cve_Aplicacion_

        'bitacora de segunda opcion, archivo texto
        _i_ActivarLogGeneral = True
        _t_RutaLogGeneral = "c:\logs\"

        'bitacora de transacciones contables a log en filesystem
        _i_ActivarLogAdministrativo = True
        _t_RutaLogAdministrativo = "c:\TransaccionesContables\"

        'sucesos en transacciones a base de datos, nivel usuario
        _i_ActivarBitacoraBasica = True
        'sucesos generales en base de datos, nivel desarrollo
        _i_ActivarBitacoraGeneral = True

        'Por defecto aplicación 4, Krombase Desktop

        'aplicación 12, Krombase Web

        ConsultarDatosConfiguracionAplicacion(i_Cve_Aplicacion_)


    End Sub

#End Region

#Region "Propiedades"

    Property ClaveAplicacion As Int32
        Get
            Return _i_Cve_Aplicacion
        End Get
        Set(value As Int32)
            _i_Cve_Aplicacion = value
        End Set
    End Property

    Property ActivarLogGeneral As Boolean
        Get
            Return _i_ActivarLogGeneral
        End Get
        Set(value As Boolean)
            _i_ActivarLogGeneral = value
        End Set
    End Property

    Property ActivarLogAdministrativo As Boolean
        Get
            Return _i_ActivarLogAdministrativo
        End Get
        Set(value As Boolean)
            _i_ActivarLogAdministrativo = value
        End Set
    End Property



    Property RutaLocalLog As String
        Get
            Return _t_RutaLogGeneral
        End Get
        Set(value As String)
            _t_RutaLogGeneral = value
        End Set
    End Property

    Property RutaLocalTransaccionesLog As String
        Get
            Return _t_RutaLogAdministrativo
        End Get
        Set(value As String)
            _t_RutaLogAdministrativo = value
        End Set
    End Property

    Property ActivarBitacoraSimple As String
        Get
            Return _i_ActivarBitacoraBasica
        End Get
        Set(value As String)
            _i_ActivarBitacoraBasica = value
        End Set
    End Property

    Property ActivarBitacoraAvanzada As String
        Get
            Return _i_ActivarBitacoraGeneral
        End Get
        Set(value As String)
            _i_ActivarBitacoraGeneral = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub ConsultarDatosConfiguracionAplicacion(ByVal i_Cve_Aplicacion_ As Int32)

        'Vamos a revisar que dice la configuración centrar de la instancia primero, de no existir indicaciones centrales dependerá de del control vía interfaz del módulo de aplicaciones.
        '_singletonConfiguracion =
        Configuracion.ObtenerInstancia()

        Dim controlCentralBitacoras_ As Int32 = "0"

        controlCentralBitacoras_ = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ControlarBitacoraDesdeLlaveCentral)

        If controlCentralBitacoras_ = "1" Then

            CargaDatosBitacoraCentralAplicacion(i_Cve_Aplicacion_)

        Else
            'En caso que no haya razón en el archivo de configuración del CMF, revisaremos bajo demanda en la base de datos.
            CargaDatosBitacoraPorAplicacion(i_Cve_Aplicacion_)

        End If

    End Sub

    Private Sub CargaDatosBitacoraCentralAplicacion(ByVal i_cve_aplicacion_ As Int32)

        'Aquí la aplicación no es importante aún, la configuración se aplicará a todas las aplicaciones y divisiones del sistema del dominio de la llave central "Archivo cifrado"

        _singletonConfiguracion = Configuracion.ObtenerInstancia()

        With _singletonConfiguracion

            _i_Cve_Aplicacion = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.AplicacionInicial).ToString

            If Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ActivarLogGeneral).ToString = "1" Then

                _i_ActivarLogGeneral = True

            Else

                _i_ActivarLogGeneral = False

            End If

            _t_RutaLogGeneral = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.RutaLogGeneral).ToString

            If Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ActivarLogAdministrativo).ToString = "1" Then

                _i_ActivarLogAdministrativo = True

            Else

                _i_ActivarLogAdministrativo = False

            End If

            _t_RutaLogAdministrativo = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.RutaLogAdministrativo).ToString


            If Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ActivarBitacoraBasica).ToString = "1" Then

                _i_ActivarBitacoraBasica = True

            Else

                _i_ActivarBitacoraBasica = False

            End If

            If Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ActivarBitacoraGeneral).ToString = "1" Then

                _i_ActivarBitacoraGeneral = True

            Else

                _i_ActivarBitacoraGeneral = False

            End If


            '_i_Cve_Aplicacion = _singletonConfiguracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.AplicacionInicial).ToString

            '_i_ActivarLogGeneral = True

            '_t_RutaLogGeneral = "c:\logs"

            '_i_ActivarLogAdministrativo = True

            '_t_RutaLogAdministrativo = "c:\TransaccionesContables"

            '_i_ActivarBitacoraBasica = True

            '_i_ActivarBitacoraGeneral = True

        End With


    End Sub

    Private Sub CargaDatosBitacoraPorAplicacion(ByVal i_cve_Aplicacion_ As Int32)

        'Entrando al singleton para decidir sobre las posibilidades de la bitacora
        '******************************************************************************

        Dim cadenaConexion_ As String
        Dim conexion_ As SqlConnection
        Dim comando_ As SqlCommand
        Dim sql_ As String

        'Dim _singletonConfiguracion As New Configuracion
        _singletonConfiguracion = Configuracion.ObtenerInstancia()

        Dim contrasena_ As String = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)
        Dim usuario_ As String = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)
        Dim baseDatos_ As String = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)
        Dim servidor_ As String = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

        cadenaConexion_ = "Data Source=" & servidor_ & _
                            ";Initial Catalog=" & baseDatos_ & _
                            ";User ID=" & usuario_ & _
                            ";Password=" & contrasena_

        sql_ = "select * from Vt000Aplicaciones where i_Cve_Estado = 1 and i_Cve_Aplicacion = " & i_cve_Aplicacion_

        conexion_ = New SqlConnection(cadenaConexion_)
        Try
            conexion_.Open()

            comando_ = New SqlCommand(sql_, conexion_)

            Dim sqlReader As SqlDataReader = comando_.ExecuteReader()

            While sqlReader.Read()

                _i_Cve_Aplicacion = i_cve_Aplicacion_

                _i_Cve_Aplicacion = i_cve_Aplicacion_

                '_i_Cve_Aplicacion = 12

                If sqlReader.Item("i_ActivarLogGeneral") = 1 Then
                    _i_ActivarLogGeneral = True
                Else
                    _i_ActivarLogGeneral = False
                End If

                _t_RutaLogGeneral = sqlReader.Item("t_RutaLogGeneral")


                If sqlReader.Item("i_ActivarLogAdministrativo") = 1 Then
                    _i_ActivarLogAdministrativo = True
                Else
                    _i_ActivarLogAdministrativo = False
                End If

                _t_RutaLogAdministrativo = sqlReader.Item("t_RutaLogAdministrativo")


                'Básica
                If sqlReader.Item("i_ActivarBitacoraBasica") = 1 Then

                    _i_ActivarBitacoraBasica = True
                Else
                    _i_ActivarBitacoraBasica = False

                End If

                'Avanzada
                If sqlReader.Item("i_ActivarBitacoraGeneral") = 1 Then

                    _i_ActivarBitacoraGeneral = True
                Else
                    _i_ActivarBitacoraGeneral = False

                End If
            End While

            sqlReader.Close()

            comando_.Dispose()

            conexion_.Close()

        Catch ex As Exception

            ' NOT IMPLEMENTED

        End Try
    End Sub



    Public Sub ConsultarDatosConfiguracionAplicacionStopped(ByVal i_Cve_Aplicacion_ As Int32)

        'Entrando al singleton para decidir sobre las posibilidades de la bitacora
        '******************************************************************************

        'Dim cadenaConexion_ As String
        'Dim conexion_ As SqlConnection
        'Dim comando_ As SqlCommand
        'Dim sql_ As String

        'Dim configuracion_ As New Configuracion

        'Dim contrasena_ As String = configuracion_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)
        'Dim usuario_ As String = configuracion_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)
        'Dim baseDatos_ As String = configuracion_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)
        'Dim servidor_ As String = configuracion_.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

        'cadenaConexion_ = "Data Source=" & servidor_ & _
        '                    ";Initial Catalog=" & baseDatos_ & _
        '                    ";User ID=" & usuario_ & _
        '                    ";Password=" & contrasena_

        'sql_ = "select * from Vt000Aplicaciones where i_Cve_Estado = 1 and i_Cve_Aplicacion = " & i_Cve_Aplicacion_

        'conexion_ = New SqlConnection(cadenaConexion_)
        'Try
        '    conexion_.Open()

        '    comando_ = New SqlCommand(sql_, conexion_)

        '    Dim sqlReader As SqlDataReader = comando_.ExecuteReader()

        '    While sqlReader.Read()

        '        _i_Cve_Aplicacion = i_Cve_Aplicacion_

        _i_Cve_Aplicacion = i_Cve_Aplicacion_
        '_i_Cve_Aplicacion = 12

        '        If sqlReader.Item("i_ActivarLogGeneral") = 1 Then
        '            _i_ActivarLogGeneral = True
        '        Else
        '            _i_ActivarLogGeneral = False
        '        End If

        '        _t_RutaLogGeneral = sqlReader.Item("t_RutaLogGeneral")

        _i_ActivarLogGeneral = True

        _t_RutaLogGeneral = "c:\logs"


        '        If sqlReader.Item("i_ActivarLogAdministrativo") = 1 Then
        '            _i_ActivarLogAdministrativo = True
        '        Else
        '            _i_ActivarLogAdministrativo = False
        '        End If

        '        _t_RutaLogAdministrativo = sqlReader.Item("t_RutaLogAdministrativo")

        _i_ActivarLogAdministrativo = True

        _t_RutaLogAdministrativo = "c:\TransaccionesContables"


        '        If sqlReader.Item("i_ActivarBitacoraBasica") = 1 Then

        '            _i_ActivarBitacoraBasica = True
        '        Else
        '            _i_ActivarBitacoraBasica = False

        '        End If

        '        If sqlReader.Item("i_ActivarBitacoraGeneral") = 1 Then

        '            _i_ActivarBitacoraGeneral = True
        '        Else
        '            _i_ActivarBitacoraGeneral = False

        '        End If
        '    End While

        _i_ActivarBitacoraBasica = True

        _i_ActivarBitacoraGeneral = True

        '    sqlReader.Close()
        '    comando_.Dispose()
        '    conexion_.Close()

        'Catch ex As Exception

        'NOT IMPLEMENTED

        'End Try

        '******************************************************************************

        '-*****************************************************************************

    End Sub

    Public Shared Function ObtenerInstancia(ByVal ClaveAplicacion As Int32) As SingletonBitacoras

        If _instancia Is Nothing Then

            _instancia = New SingletonBitacoras(ClaveAplicacion)

        Else

            If _instancia.ClaveAplicacion = ClaveAplicacion Then

                Return _instancia

            Else

                _instancia = New SingletonBitacoras(ClaveAplicacion)

            End If

        End If

        Return _instancia

    End Function

#End Region

End Class
