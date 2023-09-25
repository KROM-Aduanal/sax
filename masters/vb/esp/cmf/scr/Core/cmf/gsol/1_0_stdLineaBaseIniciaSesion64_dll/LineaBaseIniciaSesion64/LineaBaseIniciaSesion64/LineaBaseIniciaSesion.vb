Imports gsol.basededatos
Imports gsol.basedatos.Operaciones
Imports gsol.seguridad
Imports System.IO

'Imports System.Data.SqlClient

Namespace gsol

    Public Class LineaBaseIniciaSesion
        Inherits Organismo

#Region "Atributos"

        Private _claveEjecutivo As String
        Private _claveUsuario As Int32
        Private _usuario As String
        Private _contrasenia As String
        Private _grupoEmpresarial As Integer
        Private _divisionEmpresarial As Integer
        Private _aplicacion As Integer
        Private _idioma As Integer

        Private _gruposEmpresariales As DataSet
        Private _credenciales As ICredenciales
        Private _estadoAutenticacion As IIniciaSesion.StatusAutenticacion
        Private _nombreAutenticacion As String

        Private _ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta



#End Region

#Region "Contructores"

        Sub New()
            _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.SinDefinir
            _nombreAutenticacion = Nothing
            _claveEjecutivo = Nothing
            _claveUsuario = Nothing
            ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.SinDefinir
        End Sub

#End Region

#Region "Propiedades"

        Property Credenciales As ICredenciales
            Get
                Return _credenciales
            End Get
            Set(ByVal value As ICredenciales)
                _credenciales = value
            End Set
        End Property

        Property Usuario As String
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

        Property Contrasena As String
            Get
                Return _contrasenia
            End Get
            Set(ByVal value As String)
                _contrasenia = value
            End Set
        End Property

        Property GrupoEmpresarial As Integer
            Get
                Return _grupoEmpresarial
            End Get
            Set(ByVal value As Integer)
                _grupoEmpresarial = value
            End Set
        End Property

        Property DivisionEmpresarial As Integer
            Get
                Return _divisionEmpresarial
            End Get
            Set(ByVal value As Integer)
                _divisionEmpresarial = value
            End Set
        End Property

        Property Aplicacion As Integer
            Get
                Return _aplicacion
            End Get
            Set(ByVal value As Integer)
                _aplicacion = value
            End Set
        End Property

        ReadOnly Property EstadoAutenticacion
            Get
                Return _estadoAutenticacion
            End Get
        End Property

        Property NombreAutenticacion As String
            Get
                Return _nombreAutenticacion
            End Get
            Set(ByVal value As String)
                _nombreAutenticacion = value
            End Set
        End Property

        Property GruposEmpresariales As DataSet
            Get
                Return _gruposEmpresariales
            End Get
            Set(ByVal value As DataSet)
                _gruposEmpresariales = value
            End Set
        End Property

        Property Idioma As Integer
            Get
                Return _idioma
            End Get
            Set(ByVal value As Integer)
                _idioma = value
            End Set
        End Property

        Property ClaveEjecutivo As String
            Get
                Return _claveEjecutivo
            End Get
            Set(ByVal value As String)
                _claveEjecutivo = value
            End Set
        End Property

        Property ClaveUsuario As Integer
            Get
                Return _claveUsuario
            End Get
            Set(ByVal value As Integer)
                _claveUsuario = value
            End Set
        End Property

        Public Property ModalidadConsulta As String
            Get
                Return _ModalidadConsulta
            End Get
            Set(value As String)
                _ModalidadConsulta = value
            End Set
        End Property

#End Region

#Region "Metodos"

        'Ejecuta una autenticacion simple solo con usuario y contraseña para obtener los grupos empresariales, divisiones y empresas a los que tiene acceso
        Public Sub ObtenerGrupos()

            Dim respuesta_ As String = Nothing
            Dim disparo_ As String = Nothing
            'Dim ConexionSingleton.SQLServerSingletonConexion As IConexiones = New Conexiones

            Try
                'Inicia conexion
                'ConectarBaseDatos(ConexionSingleton.SQLServerSingletonConexion)
                'ConexionSingleton.SQLServerSingletonConexion.IniciaConexion()

                ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()

                ConexionSingleton.UsuarioCliente = _usuario

                ConexionSingleton.ModuloCliente = "Inicio sesión"


                disparo_ = (
                            "sp000cargagrupos '" & _usuario &
                            "', '" & _contrasenia &
                            "';"
                            )


                'Si se obtuvieron resultados se llena un dataset con la informacion que se obtuvo 
                If ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_) And
                    ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count >= 1 Then
                    _gruposEmpresariales = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente

                End If

                'Termina conexion la conexion 
                'ConexionSingleton.SQLServerSingletonConexion.TerminaConexion()

                'Asigna el estado de la autenticacion dependiendo del resultado obtenido
                If _gruposEmpresariales IsNot Nothing Then
                    _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado
                Else
                    _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.Rechazado
                End If

                'Control de excepciones 
            Catch ex As SqlClient.SqlException
                _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.FalloConexion

                'Log(
                '    ex.ToString, _
                '    monitoreo.IBitacoras.TiposBitacora.Errores, _
                '    monitoreo.IBitacoras.TiposSucesos.Consultar, _
                '    )

            End Try


        End Sub

        'Autenticación sin singleton para Webservices, o procesos Web
        Public Sub Autenticar()

            Dim respuesta_ As String = Nothing
            Dim claveUsuario_ As Int32 = Nothing
            Dim disparo_ As String = Nothing
            'Dim ConexionSingleton.SQLServerSingletonConexion As IConexiones = New Conexiones

            Try

                'Inicia conexion
                'ConectarBaseDatos(ConexionSingleton.SQLServerSingletonConexion)
                'ConexionSingleton.SQLServerSingletonConexion.IniciaConexion()

                ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                ' ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()

                '                    " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;" & _
                disparo_ = (
                  " sp000autenticacion '" & _usuario &
                   "', '" & _contrasenia &
                   "', '" & _grupoEmpresarial &
                   "', '" & _divisionEmpresarial &
                   "', '" & _aplicacion &
                   "';"
               )

                ConexionSingleton.UsuarioCliente = _usuario

                ConexionSingleton.ModuloCliente = "Inicio sesión"

                '---------------------------------------
                'Modifiaciones Webservices.
                '-----------------------------------------


                Dim dataset_ As New DataSet

                Dim conexion_ As IConexiones = New Conexiones

                conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

                Dim configuracion_ As Configuracion

                configuracion_ = Configuracion.ObtenerInstancia()

                'controlCentralBitacoras_ = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ControlarBitacoraDesdeLlaveCentral)


                conexion_.Contrasena = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.ClaveSQLServerGeneralProduccion)
                conexion_.Usuario = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion)
                conexion_.NombreBaseDatos = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)
                conexion_.IpServidor = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion)

                conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand
                conexion_.DataSetReciente.Tables.Clear()

                conexion_.ClaveDivisionMiEmpresa = _divisionEmpresarial
                conexion_.IDUsuario = 0
                conexion_.IDAplicacion = _aplicacion
                conexion_.IDRecursoSolicitante = 0
                conexion_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
                conexion_.TipoInstrumentacion = monitoreo.IBitacoras.TiposInstrumentacion.GestorIOperaciones

                conexion_.EjecutaConsultaIndividual(disparo_)
                dataset_ = conexion_.DataSetReciente.Copy()

                With dataset_
                    If Not .Tables Is Nothing Then
                        If Not .Tables.Count <= 0 Then
                            If Not .Tables(0).Rows Is Nothing Then
                                If Not .Tables(0).Rows.Count <= 0 Then
                                    If .Tables(0).Rows.Count >= 1 Then
                                        respuesta_ = .Tables(0).Rows(0)("t_NombrePersona").ToString
                                        claveUsuario_ = .Tables(0).Rows(0)("i_Cve_Usuario").ToString

                                        'MOP, 25/05/2020
                                        _claveEjecutivo = .Tables(0).Rows(0)("i_Cve_EjecutivosMisEmpresas").ToString

                                    End If
                                End If
                            End If
                        End If
                    End If
                End With

                If respuesta_ IsNot Nothing Then
                    _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado
                    _nombreAutenticacion = respuesta_
                    _claveUsuario = claveUsuario_
                Else
                    _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.Rechazado
                    _nombreAutenticacion = "Fallo autenticacion"

                End If

                'Control de excepciones
            Catch ex As Exception
                ''RIGG Se reactiva log, para efecto de WFC presentaba fallas.                
                Log(ex.ToString,
                    monitoreo.IBitacoras.TiposBitacora.Errores,
                    monitoreo.IBitacoras.TiposSucesos.Consultar,
                    _credenciales,
                    Nothing,
                    "_usuario: " & _usuario & "; _contrasenia: " & _contrasenia & "; _grupoEmpresarial: " & _grupoEmpresarial _
                    & "; _divisionEmpresarial: " & "; _aplicacion: " & _aplicacion,
                    disparo_)

                _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.FalloConexion

            End Try

        End Sub

        'Ejecuta una autenticacion completa con usuario, contraseña, grupo empresarial, division empresarial y la aplicacion a la cual intentan accesar 
        Public Sub AutenticarObsoleto()
            Dim respuesta_ As String = Nothing
            Dim claveUsuario_ As Int32 = Nothing
            Dim disparo_ As String = Nothing
            'Dim ConexionSingleton.SQLServerSingletonConexion As IConexiones = New Conexiones

            Try

                'Inicia conexion
                'ConectarBaseDatos(ConexionSingleton.SQLServerSingletonConexion)
                'ConexionSingleton.SQLServerSingletonConexion.IniciaConexion()

                ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()
                '                    "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;" & _
                disparo_ = ("sp000autenticacion '" & _usuario &
                   "', '" & _contrasenia &
                   "', '" & _grupoEmpresarial &
                   "', '" & _divisionEmpresarial &
                   "', '" & _aplicacion &
                   "';"
               )

                ConexionSingleton.UsuarioCliente = _usuario

                ConexionSingleton.ModuloCliente = "Inicio sesión"

                '---------------------------------------

                ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)

                'Si se obtuvieron resultados se regresa el nombre del usuario que se autentico 
                If ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count >= 1 Then
                    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)("t_NombrePersona").ToString
                    claveUsuario_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)("i_Cve_Usuario").ToString
                End If

                'Modifiaciones Webservices.
                '-----------------------------------------

                '-------------------

                'Se termina la conexion
                'ConexionSingleton.SQLServerSingletonConexion.TerminaConexion()

                'Asigna el estado de la autenticacion dependiendo del resultado obtenido
                If respuesta_ IsNot Nothing Then
                    _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado
                    _nombreAutenticacion = respuesta_
                    _claveUsuario = claveUsuario_
                Else
                    _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.Rechazado
                    _nombreAutenticacion = "Fallo autenticacion"

                End If

                'Control de excepciones
            Catch ex As Exception
                ''RIGG Se reactiva log, para efecto de WFC presentaba fallas.                
                Log(ex.ToString,
                    monitoreo.IBitacoras.TiposBitacora.Errores,
                    monitoreo.IBitacoras.TiposSucesos.Consultar,
                    _credenciales,
                    Nothing,
                    "_usuario: " & _usuario & "; _contrasenia: " & _contrasenia & "; _grupoEmpresarial: " & _grupoEmpresarial _
                    & "; _divisionEmpresarial: " & "; _aplicacion: " & _aplicacion,
                    disparo_)

                _estadoAutenticacion = IIniciaSesion.StatusAutenticacion.FalloConexion

            End Try

        End Sub

        Private Function GuardaFechaEdicionBinariaEnPerfilOficina(ByVal usuario_ As String,
                                                                  ByVal divisionEmpresarial_ As Int32,
                                                                  ByVal f_fechaEdicionBinaria_ As Date,
                                                                  ByVal aplicacion_ As Int32) As Boolean

            Dim disparo_ As String = Nothing

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            disparo_ = (
            " UPDATE R " &
            " SET R.f_FechaEdicionPerfilBinario = '" & f_fechaEdicionBinaria_.ToString("yyyy-MM-ddTHH:mm:ss") & "'  " &
            " FROM Reg000Perfiles AS R " &
            " INNER JOIN dbo.Cat000Usuarios AS P  " &
                   " ON R.i_cve_usuario = P.i_cve_usuario  " &
            " WHERE P.t_Usuario = '" & usuario_ & "' " &
            " and R.i_Cve_DivisionEmpresarial = " & divisionEmpresarial_ & " " &
            " and R.i_Cve_Aplicacion = " & aplicacion_ & " " &
            " and R.i_Cve_Estado = 1")

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            If ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_) Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Function VerificaVigenciaBinarioPerfil(ByVal usuario_ As String,
                                                       ByVal divisionEmpresarial_ As Int32,
                                                       ByVal fechaBinario_ As Date,
                                                       ByVal aplicacion_ As Int32)

            Dim disparo_ As String = Nothing

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()
            '" Select rp.f_FechaEdicionPerfilBinario " & _

            disparo_ = (
            " select FORMAT(CONVERT(DATETIME,rp.f_FechaEdicionPerfilBinario ,108),'yyyy-MM-dd HH:mm:ss','en-us') as f_FechaEdicionPerfilBinario " &
            " from Reg000Perfiles as rp " &
            " inner join Cat000Usuarios as cu on cu.i_Cve_Usuario = rp.i_cve_usuario " &
            " where " &
            " cu.t_Usuario = '" & usuario_ & "' " &
            " and rp.i_Cve_DivisionEmpresarial = " & divisionEmpresarial_ & " " &
            " and rp.i_Cve_Aplicacion = " & aplicacion_ & " " &
            " and FORMAT(CONVERT(DATETIME,rp.f_FechaEdicionPerfilBinario ,108),'yyyy-MM-dd HH:mm:ss','en-us') = '" & fechaBinario_.ToString("yyyy-MM-dd HH:mm:ss") & "'" &
            " and rp.i_Cve_Estado = 1")


            '" and rp.f_FechaEdicionPerfilBinario = '17/01/2019 17:23:09.813'" & _

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)

            Dim resultadoSpSectoresUsuarios_ As DataSet = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

            With resultadoSpSectoresUsuarios_

                If Not .Tables Is Nothing Then

                    If .Tables.Count > 0 Then

                        If Not .Tables(0).Rows Is Nothing Then

                            If .Tables(0).Rows.Count > 0 Then

                                If Not .Tables(0).Rows(0)("f_FechaEdicionPerfilBinario").ToString Is Nothing Then

                                    Return True

                                End If

                            End If

                        End If

                    End If

                End If

            End With


            Log("Actualización Binario [" & usuario_ & "][" & fechaBinario_.ToString("dd/MM/yyyy HH:mm:ss") & "]", monitoreo.IBitacoras.TiposBitacora.Informacion, monitoreo.IBitacoras.TiposSucesos.Otros, _credenciales, 0)


            Return False

        End Function

        Public Function ObtenerEspacioTrabajo_obsoleto(
            ByVal usuario_ As String,
            ByVal grupoEmpresarial_ As Int32,
            ByVal divisionEmpresarial_ As Int32,
            ByVal aplicacion_ As Int32
        ) As Dictionary(Of Integer, ISectorEntorno)

            Dim sectores_ As Dictionary(Of Int32, ISectorEntorno) = New Dictionary(Of Int32, ISectorEntorno)
            Dim disparo_ As String = Nothing

            'Actualizado para producción
            Dim rutaBinario_ As String = Environ("USERPROFILE") & "\" & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"

            '
            'Dim rutaBinario_ As String = "c:\logs\" & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"

            'Si existe el mapeo de sesión
            If System.IO.File.Exists(rutaBinario_) = True Then

                Dim sectoresBinarios_ As Dictionary(Of Integer, gsol.ISectorEntorno) = ReadMultiSerializedDict(rutaBinario_)

                'Si se cargó correctamente la instancia
                If Not sectoresBinarios_ Is Nothing Then

                    Dim pairKey_ As KeyValuePair(Of Integer, ISectorEntorno)

                    'Verificamos el primer sector, para saber si corresponde al inicio de sesión solicitado
                    For Each pairKey_ In sectoresBinarios_

                        If LCase(pairKey_.Value.Usuario) = LCase(usuario_) And
                            pairKey_.Value.DivisionEmpresarial = _divisionEmpresarial And
                            pairKey_.Value.Idioma = _idioma Then 'And
                            'pairKey_.Value.FechaBinario = _fechaBinario Then
                            'pairKey_.Value.Password = _contrasenia And

                            'Verifica Vigencia Serialización Binaria
                            If VerificaVigenciaBinarioPerfil(usuario_, _divisionEmpresarial, pairKey_.Value.FechaBinario, aplicacion_) Then

                                sectores_ = sectoresBinarios_

                                Return sectores_

                            Else

                                'En caso de no coincidir, procedemos a borrar el binario por seguridad
                                System.IO.File.Delete(rutaBinario_)

                                GeneradorEspacioTrabajo(usuario_,
                                                        grupoEmpresarial_,
                                                        divisionEmpresarial_,
                                                        aplicacion_,
                                                        rutaBinario_,
                                                        sectores_)

                            End If

                        Else
                            'En caso de no coincidir, procedemos a borrar el binario por seguridad
                            System.IO.File.Delete(rutaBinario_)

                            GeneradorEspacioTrabajo(usuario_,
                                                    grupoEmpresarial_,
                                                    divisionEmpresarial_,
                                                    aplicacion_,
                                                    rutaBinario_,
                                                    sectores_)


                        End If

                        Exit For

                    Next

                End If

            Else

                'Solo vamos a generar los binarios para la aplicación de escritorio.
                If aplicacion_ = 4 Then

                    GeneradorEspacioTrabajo(usuario_,
                                        grupoEmpresarial_,
                                        divisionEmpresarial_,
                                        aplicacion_,
                                        rutaBinario_,
                                        sectores_)

                Else

                    'En caso de una sesión Web y de no encontrar los binarios, no podrá iniciar sesión
                    Return Nothing

                End If


            End If

            Return sectores_

        End Function

        Public Function ObtenerEspacioTrabajo(
            ByVal usuario_ As String,
            ByVal grupoEmpresarial_ As Int32,
            ByVal divisionEmpresarial_ As Int32,
            ByVal aplicacion_ As Int32
        ) As Dictionary(Of Integer, ISectorEntorno)

            Dim sectores_ As Dictionary(Of Int32, ISectorEntorno) = New Dictionary(Of Int32, ISectorEntorno)

            Dim disparo_ As String = Nothing

            Dim rutaBinario_ As String

            Dim organismo_ As New Organismo

            'Sesión completamente dinámica Krombase
            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            '" SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;" & Chr(13) & _
            disparo_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED" &
                       " select t_RutaArchivoBinario from Cat000Aplicaciones with(nolock) where i_Cve_Aplicacion = " & aplicacion_

            'ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)

            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaIndividual(disparo_)

            rutaBinario_ = IIf(IsDBNull(ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)), Environ("USERPROFILE"), ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

            If aplicacion_ = 4 Then
                'Actualizado para producción
                rutaBinario_ = rutaBinario_ & "\" & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"

            Else
                ' Se es web IIS se guardará en ruta de logs
                'Actualizado para producción
                'rutaBinario_ = Environ("USERPROFILE") & "\" & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"
                rutaBinario_ = rutaBinario_ & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"

            End If

            Try

                'Si existe el mapeo de sesión
                If System.IO.File.Exists(rutaBinario_) = True Then

                    Dim sectoresBinarios_ As Dictionary(Of Integer, gsol.ISectorEntorno) = ReadMultiSerializedDict(rutaBinario_)

                    'Si se cargó correctamente la instancia
                    If Not sectoresBinarios_ Is Nothing Then

                        'Solo vamos a generar los binarios para la aplicación de escritorio.
                        If aplicacion_ = 4 Then

                            'En caso se una aplicación de escritorio
                            Dim pairKey_ As KeyValuePair(Of Integer, ISectorEntorno)

                            'Verificamos el primer sector, para saber si corresponde al inicio de sesión solicitado
                            For Each pairKey_ In sectoresBinarios_

                                If LCase(pairKey_.Value.Usuario) = LCase(usuario_) And
                                pairKey_.Value.DivisionEmpresarial = _divisionEmpresarial And
                                pairKey_.Value.Idioma = _idioma Then 'And
                                    'pairKey_.Value.FechaBinario = _fechaBinario Then
                                    'pairKey_.Value.Password = _contrasenia And

                                    'Verifica Vigencia Serialización Binaria
                                    If VerificaVigenciaBinarioPerfil(usuario_, _divisionEmpresarial, pairKey_.Value.FechaBinario, aplicacion_) Then

                                        sectores_ = sectoresBinarios_

                                        Return sectores_

                                    Else

                                        'En caso de no coincidir, procedemos a borrar el binario por seguridad
                                        System.IO.File.Delete(rutaBinario_)

                                        GeneradorEspacioTrabajo(usuario_,
                                                            grupoEmpresarial_,
                                                            divisionEmpresarial_,
                                                            aplicacion_,
                                                            rutaBinario_,
                                                            sectores_)

                                    End If





                                Else
                                    'En caso de no coincidir, procedemos a borrar el binario por seguridad
                                    System.IO.File.Delete(rutaBinario_)

                                    GeneradorEspacioTrabajo(usuario_,
                                                        grupoEmpresarial_,
                                                        divisionEmpresarial_,
                                                        aplicacion_,
                                                        rutaBinario_,
                                                        sectores_)


                                End If

                                Exit For

                            Next


                        Else

                            'Si se trata de una aplicación Web, únicamente enviaremos los sectores.

                            sectores_ = sectoresBinarios_

                            Return sectores_


                        End If

                    End If

                Else

                    'Solo vamos a generar los binarios para la aplicación de escritorio.
                    If aplicacion_ = 4 Then

                        GeneradorEspacioTrabajo(usuario_,
                                        grupoEmpresarial_,
                                        divisionEmpresarial_,
                                        aplicacion_,
                                        rutaBinario_,
                                        sectores_)

                    Else

                        'En caso de una sesión Web y de no encontrar los binarios, no podrá iniciar sesión
                        'Return sectores_

                        Return Nothing

                    End If


                End If

            Catch ex As Exception

                Dim texto_ As String = "Menssage: " & ex.Message & vbCrLf & vbCrLf & "StackTrace: " & ex.StackTrace

                organismo_.GeneraLog("C:\logs\ObtenerEspacioTrabajo.log", texto_)

            End Try

            Return sectores_

        End Function

        Public Function ObtenerEspacioTrabajoEstandar(
                                                         ByVal usuario_ As String,
                                                         ByVal grupoEmpresarial_ As Int32,
                                                         ByVal divisionEmpresarial_ As Int32,
                                                         ByVal aplicacion_ As Int32
                                                        ) As Dictionary(Of Integer, ISectorEntorno)

            Dim sectores_ As Dictionary(Of Int32, ISectorEntorno) = New Dictionary(Of Int32, ISectorEntorno)

            Dim disparo_ As String = Nothing

            Dim rutaBinario_ As String

            'Sesión completamente dinámica Krombase
            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            '" SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;" & Chr(13) & _
            disparo_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED" &
                       " select t_RutaArchivoBinario from Cat000Aplicaciones with(nolock) where i_Cve_Aplicacion = " & aplicacion_

            'ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)
            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaIndividual(disparo_)

            rutaBinario_ = IIf(IsDBNull(ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)), Environ("USERPROFILE"), ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

            If aplicacion_ = 4 Then
                'Actualizado para producción
                rutaBinario_ = rutaBinario_ & "\" & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"

            Else
                ' Se es web IIS se guardará en ruta de logs
                'Actualizado para producción
                'rutaBinario_ = Environ("USERPROFILE") & "\" & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"
                rutaBinario_ = rutaBinario_ & usuario_ & "_" & aplicacion_ & "_" & divisionEmpresarial_ & "_blasterlogon.bin"

            End If

            'Si existe el mapeo de sesión
            If System.IO.File.Exists(rutaBinario_) = True Then

                Dim sectoresBinarios_ As Dictionary(Of Integer, gsol.ISectorEntorno) = ReadMultiSerializedDict(rutaBinario_)

                'Si se cargó correctamente la instancia
                If Not sectoresBinarios_ Is Nothing Then

                    'En caso se una aplicación de escritorio
                    Dim pairKey_ As KeyValuePair(Of Integer, ISectorEntorno)

                    'Verificamos el primer sector, para saber si corresponde al inicio de sesión solicitado
                    For Each pairKey_ In sectoresBinarios_

                        If LCase(pairKey_.Value.Usuario) = LCase(usuario_) And
                            pairKey_.Value.DivisionEmpresarial = _divisionEmpresarial And
                            pairKey_.Value.Idioma = _idioma Then 'And
                            'pairKey_.Value.FechaBinario = _fechaBinario Then
                            'pairKey_.Value.Password = _contrasenia And

                            'Verifica Vigencia Serialización Binaria
                            If VerificaVigenciaBinarioPerfil(usuario_, _divisionEmpresarial, pairKey_.Value.FechaBinario, aplicacion_) Then

                                sectores_ = sectoresBinarios_

                                Return sectores_

                            Else

                                'En caso de no coincidir, procedemos a borrar el binario por seguridad
                                System.IO.File.Delete(rutaBinario_)

                                GeneradorEspacioTrabajo(usuario_,
                                                        grupoEmpresarial_,
                                                        divisionEmpresarial_,
                                                        aplicacion_,
                                                        rutaBinario_,
                                                        sectores_)

                            End If

                        Else

                            'En caso de no coincidir, procedemos a borrar el binario por seguridad
                            System.IO.File.Delete(rutaBinario_)

                            GeneradorEspacioTrabajo(usuario_,
                                                    grupoEmpresarial_,
                                                    divisionEmpresarial_,
                                                    aplicacion_,
                                                    rutaBinario_,
                                                    sectores_)


                        End If

                        Exit For

                    Next

                End If

            Else

                GeneradorEspacioTrabajo(usuario_,
                                        grupoEmpresarial_,
                                        divisionEmpresarial_,
                                        aplicacion_,
                                        rutaBinario_,
                                        sectores_)

            End If

            Return sectores_

        End Function

        Private Function GeneradorEspacioTrabajo(ByVal usuario_ As String,
                                                 ByVal grupoEmpresarial_ As String,
                                                 ByVal divisionEmpresarial_ As String,
                                                 ByVal aplicacion_ As String,
                                                 ByRef rutaBinario_ As String,
                                                 ByRef sectores_ As Dictionary(Of Int32, ISectorEntorno))


            Dim disparo_ As String = Nothing

            'Sesión completamente dinámica Krombase
            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            disparo_ = (
            "Sp000SectoresUsuario '" & usuario_ &
            "',  '" & grupoEmpresarial_ &
            "', '" & divisionEmpresarial_ &
            "', '" & aplicacion_ &
            "' ;")

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)

            Dim resultadoSpSectoresUsuarios_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

            Dim f_fechaEdicionBinaria_ As New Date

            f_fechaEdicionBinaria_ = Now

            For _indice As Int32 = 0 To resultadoSpSectoresUsuarios_.Tables(0).Rows.Count - 1
                Dim claveModulo_ As Int32 = resultadoSpSectoresUsuarios_.Tables(0).Rows(_indice)("i_Cve_Modulo").ToString
                Dim sector_ As ISectorEntorno = New SectorEntorno(
                    usuario_,
                    grupoEmpresarial_,
                    divisionEmpresarial_,
                    aplicacion_,
                    claveModulo_,
                    resultadoSpSectoresUsuarios_.Tables(0).Rows(_indice)("t_NombreModulo").ToString()
                )

                sector_.Usuario = usuario_
                sector_.Password = _contrasenia
                sector_.DivisionEmpresarial = divisionEmpresarial_
                sector_.FechaBinario = f_fechaEdicionBinaria_
                sector_.Idioma = _idioma

                sector_.Permisos = Me.ObtenerPermisos(usuario_, grupoEmpresarial_, divisionEmpresarial_, aplicacion_, claveModulo_)

                If Not sectores_.ContainsKey(claveModulo_) Then

                    sectores_.Add(claveModulo_, sector_)

                End If

            Next

            'Aquí creamos guardamos la fecha de creación para la serialización binaria
            Try

                If GuardaFechaEdicionBinariaEnPerfilOficina(usuario_, divisionEmpresarial_, f_fechaEdicionBinaria_, aplicacion_) Then

                    Dim WriteResult = WriteMultiSerializedDict(rutaBinario_, sectores_)

                Else

                    ' MsgBox("No se ha podido guardar la fecha para la edición binaria, iniciará sesión sin un perfil auxiliar, por favor notifiquelo a sistemas")

                End If

            Catch ex As Exception

                'MsgBox(ex.Message)

            End Try


        End Function

        Public Function WriteMultiSerializedDict(ByVal FullPath As String, ByVal DataDict As Dictionary(Of Integer, gsol.ISectorEntorno)) As Boolean
            Try
                Dim FileStream As IO.FileStream = New FileStream(FullPath, IO.FileMode.OpenOrCreate)
                Dim BinFormatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                BinFormatter.Serialize(FileStream, DataDict)
                FileStream.Close()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function ReadMultiSerializedDict(ByVal FullPath As String) As Dictionary(Of Integer, gsol.ISectorEntorno)
            Try
                Dim DataDict As New Dictionary(Of Integer, gsol.ISectorEntorno)
                Dim FileStream As IO.FileStream = New FileStream(FullPath, IO.FileMode.Open)

                Dim BinFormatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                DataDict = BinFormatter.Deserialize(FileStream)
                FileStream.Close()
                Return DataDict
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Private Function ObtenerPermisos(
            ByVal usuario_ As String,
            ByVal grupoEmpresarial_ As Int32,
            ByVal divisionEmpresarial_ As Int32,
            ByVal aplicacion_ As Int32,
            ByVal claveSector_ As Int32
        ) As Dictionary(Of Int32, IPermisos)

            Dim permisos_ As Dictionary(Of Int32, IPermisos) = New Dictionary(Of Int32, IPermisos)
            Dim disparo_ As String = Nothing

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()
            disparo_ = (
                "Sp000SectoresPermisos '" & usuario_ &
                "',  '" & grupoEmpresarial_ &
                "', '" & divisionEmpresarial_ &
                "', '" & aplicacion_ &
                "', " & claveSector_.ToString &
                " ;")
            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            Dim resultadoSpSectoresPermisos_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()


            For indice_ As Int32 = 0 To resultadoSpSectoresPermisos_.Tables(0).Rows.Count - 1
                Dim permiso_ As IPermisos = New Permiso

                'Try
                permiso_.Identificador = resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_Permiso").ToString
                permiso_.Descripcion = resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_Permiso").ToString
                permiso_.Dependencia = resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_PermisoDependencia").ToString
                permiso_.Componente = New Componente(
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_Componente").ToString(),
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_NombreComponente").ToString()
                )
                permiso_.Funcion = New Funcion(
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_Funcion").ToString(),
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_NombreFuncion").ToString()
                )
                permiso_.Acciones = New Accion(
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_Accion").ToString(),
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_NombreAccion").ToString()
                )
                permiso_.Entidades = Me.ObtenerEntidades(
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_Aplicacion").ToString,
                    resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("i_Cve_Entidad").ToString,
                    _idioma
                )
                permiso_.TipoAplicacion = resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_TipoAplicacion").ToString
                permiso_.NombreEnsamblado = resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_Ensamblado").ToString
                permiso_.NombreContenedor = resultadoSpSectoresPermisos_.Tables(0).Rows(indice_)("t_Contenedor").ToString


                'Catch ex As Exception
                '    Log(ex.ToString, _
                '        monitoreo.IBitacoras.TiposBitacora.Errores, _
                '        monitoreo.IBitacoras.TiposSucesos.Consultar, _
                '        _credenciales, _
                '        permiso_.Identificador, _
                '        "usuario_: " & _usuario & ";  _grupoEmpresarial: " & _grupoEmpresarial _
                '        & "; _divisionEmpresarial: " & "; _aplicacion: " & _aplicacion & "; claveSector_:" & claveSector_ & "; permiso_.Identificador:" & permiso_.Identificador, _
                '        disparo_)
                'End Try

                If Not permisos_.ContainsKey(permiso_.Identificador) Then
                    permisos_.Add(permiso_.Identificador, permiso_)
                End If


            Next

            Return permisos_
        End Function

        Private Function ObtenerEntidades(
            ByVal claveAplicacion_ As Int32,
            ByVal claveEntidad_ As Int32,
            ByVal idioma_ As Int32
        ) As Dictionary(Of Int32, IEntidad)
            Dim entidades_ As Dictionary(Of Integer, IEntidad) = New Dictionary(Of Integer, IEntidad)
            Dim disparo_ As String = Nothing


            'Try

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()
            disparo_ = ("dbo.Sp000Entidad '" & claveAplicacion_ & "', '" & claveEntidad_ & "' ;")

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(disparo_)
            Dim resultadoSpEntidad_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()


            For indice_ As Int32 = 0 To resultadoSpEntidad_.Tables(0).Rows.Count - 1
                Dim entidad_ As IEntidad = New Entidad

                entidad_.Aplicacion = resultadoSpEntidad_.Tables(0).Rows(indice_)("i_Cve_Aplicacion").ToString
                entidad_.Entidad = resultadoSpEntidad_.Tables(0).Rows(indice_)("i_Cve_Entidad").ToString
                entidad_.Eventos = Me.ObtenerEventosEntidades(claveAplicacion_, claveEntidad_)
                entidad_.Atributos = Me.ObtenerAtributosEntidades(claveAplicacion_, claveEntidad_, idioma_)

                If Not entidades_.ContainsKey(entidad_.Entidad) Then

                    entidades_.Add(entidad_.Entidad, entidad_)

                End If

            Next

            'Catch ex As SqlClient.SqlException
            '    Log(ex.ToString, _
            '        monitoreo.IBitacoras.TiposBitacora.Errores, _
            '        monitoreo.IBitacoras.TiposSucesos.Consultar, _
            '        _credenciales, _
            '        Nothing, _
            '        "usuario_: " & _usuario & ";  _grupoEmpresarial: " & _grupoEmpresarial _
            '        & "; _divisionEmpresarial: " & "; _aplicacion: " & _aplicacion & "; claveEntidad_:" & claveEntidad_, _
            '        disparo_)
            'End Try

            Return entidades_
        End Function

        Private Function ObtenerEventosEntidades(
          ByVal claveAplicacion_ As Int32,
          ByVal claveEntidad_ As Int32
          ) As Dictionary(Of Int32, IEvento)
            Dim eventos_ As Dictionary(Of Int32, IEvento) = New Dictionary(Of Int32, IEvento)


            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(
                "dbo.Sp000EventosEntidad '" & claveAplicacion_ & "', '" & claveEntidad_ & "' ;"
            )
            Dim resultadoSpEventosEntidad_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

            For indice_ As Int32 = 0 To resultadoSpEventosEntidad_.Tables(0).Rows.Count - 1
                Dim evento_ As IEvento = New EventoEntidades

                evento_.Aplicacion = resultadoSpEventosEntidad_.Tables(0).Rows(indice_)("i_Cve_Aplicacion").ToString
                evento_.Entidad = resultadoSpEventosEntidad_.Tables(0).Rows(indice_)("i_Cve_Entidad").ToString
                evento_.Identificador = resultadoSpEventosEntidad_.Tables(0).Rows(indice_)("i_Cve_Evento").ToString
                evento_.Descripcion = resultadoSpEventosEntidad_.Tables(0).Rows(indice_)("t_Nombre").ToString
                evento_.Valor = resultadoSpEventosEntidad_.Tables(0).Rows(indice_)("t_Valor").ToString

                If Not eventos_.ContainsKey(evento_.Identificador) Then

                    eventos_.Add(evento_.Identificador, evento_)

                End If

            Next

            'lectorDatos_.Close()
            'ConexionSingleton.SQLServerSingletonConexion.TerminaConexion()
            Return eventos_
        End Function

        Private Function ObtenerAtributosEntidades(
           ByVal claveAplicacion_ As Int32,
           ByVal claveEntidad_ As Int32,
           ByVal idioma_ As Int32
       ) As Dictionary(Of Int32, IAtributo)
            Dim atributos_ As Dictionary(Of Integer, IAtributo) = New Dictionary(Of Integer, IAtributo)

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()

            ConexionSingleton.UsuarioCliente = _usuario

            ConexionSingleton.ModuloCliente = "Inicio sesión"

            ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(
                "dbo.Sp000AtributosEntidad '" & claveAplicacion_ & "', '" & claveEntidad_ & "' , '" & idioma_ & "' ;"
            )


            Dim resultadoSpAtributosEntidad_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Copy()

            For indice_ As Int32 = 0 To resultadoSpAtributosEntidad_.Tables(0).Rows.Count - 1
                Dim atributo_ As IAtributo = New AtributoEntidad

                atributo_.Aplicacion = resultadoSpAtributosEntidad_.Tables(0).Rows(indice_)("i_Cve_Aplicacion").ToString
                atributo_.Entidad = resultadoSpAtributosEntidad_.Tables(0).Rows(indice_)("i_Cve_Entidad").ToString
                atributo_.Identificador = resultadoSpAtributosEntidad_.Tables(0).Rows(indice_)("i_Cve_Atributo").ToString
                atributo_.Descripcion = resultadoSpAtributosEntidad_.Tables(0).Rows(indice_)("t_NombreAtributo").ToString
                atributo_.Valor = resultadoSpAtributosEntidad_.Tables(0).Rows(indice_)("t_Valor").ToString
                atributo_.Idioma = resultadoSpAtributosEntidad_.Tables(0).Rows(indice_)("i_Cve_Idioma").ToString

                If Not atributos_.ContainsKey(atributo_.Identificador) Then

                    atributos_.Add(atributo_.Identificador, atributo_)

                End If

            Next

            Return atributos_
        End Function

        'Private Sub ConectarBaseDatos(
        '   ByVal ConexionSingleton.SQLServerSingletonConexion As IConexiones
        '  )
        '	Dim xml_ As ILeerArchivo = New LeerArchivoXML32

        '	xml_.Encriptado = ILeerArchivo.TiposCifrado.RijndaelManaged
        '	xml_.Llave = "KcjaSJElOTn2pIXa6qAVufUABuiXSbNb16KfjWjsMEQ="
        '	xml_.LeerXML()
        '	ConexionSingleton.SQLServerSingletonConexion.Usuario = xml_.RegresaValor("_UsuarioSQLServerGeneralProduccion")
        '	ConexionSingleton.SQLServerSingletonConexion.Contrasena = xml_.RegresaValor("_ClaveSQLServerGeneralProduccion")
        '	ConexionSingleton.SQLServerSingletonConexion.NombreBaseDatos = xml_.RegresaValor("_BaseDatosSQLServerProduccion")
        '	ConexionSingleton.SQLServerSingletonConexion.IpServidor = xml_.RegresaValor("_DireccionIPServidorSQLServerGeneralProduccion")
        '	'Configuración
        '	ConexionSingleton.SQLServerSingletonConexion.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008
        '	ConexionSingleton.SQLServerSingletonConexion.ObjetoDatos = IConexiones.TipoConexion.SqlCommand
        'End Sub

#End Region

    End Class

End Namespace
