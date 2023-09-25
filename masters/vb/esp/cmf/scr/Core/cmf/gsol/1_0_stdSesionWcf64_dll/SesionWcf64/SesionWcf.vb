Imports Wma.WebServices
Imports gsol.BaseDatos.Operaciones
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions
Imports gsol.seguridad
Imports Wma

Namespace gsol

    Public Class SesionWcf
        Inherits Organismo
        Implements ISesion


#Region "Atributos"

        'Sesión
        Private _claveUsuario As Int32
        Private _usuario As String
        Private _contrasenia As String
        Private _estadoSesion As Boolean
        Private _grupoEmpresarial As Integer
        Private _divisionEmpresarial As Integer
        Private _aplicacion As Integer
        Private _idioma As ISesion.Idiomas
        Private _nombreAutenticacion As String
        Private _gruposEmpresariales As DataSet

        'Configuraciónes adicionales ( opcionales )
        Private _caracteresUsuario As Integer
        Private _numeroUsuario As Integer
        Private _caracteresContrasenia As Integer
        Private _mayusculasContrasenia As Integer
        Private _minusculasContrasenia As Integer
        Private _numerosContrasenia As Integer

        'Emquetado y prepación 
        Private _credenciales As ICredenciales

        'Espacio de trabajo
        Private _espacioTrabajo As IEspacioTrabajo

        'Notificaciones 
        Private _notificacion As ISesion.Notificaciones

#End Region

#Region "Constructores"

        Sub New()
            'Identificación
            'NumeroParte = "001"
            'NombreObjeto = "Sesion "
            'NombreArchivo = "Sesion "

            'Sesion
            _claveUsuario = Nothing
            _usuario = Nothing
            _contrasenia = Nothing
            _estadoSesion = Nothing
            _grupoEmpresarial = Nothing
            _divisionEmpresarial = Nothing
            _aplicacion = Nothing
            _idioma = ISesion.Idiomas.Espaniol
            _gruposEmpresariales = Nothing
            _nombreAutenticacion = "Fallo"
            _notificacion = ISesion.Notificaciones.SinNotificacion

            'Configuraciones adicionales
            _caracteresUsuario = 6
            _numeroUsuario = 0
            _caracteresContrasenia = 6
            _mayusculasContrasenia = 1
            _minusculasContrasenia = 1
            _numerosContrasenia = 1
            'Empaqueta y preparación
            _credenciales = New Credenciales
        End Sub

        Sub New(
            ByVal usuario_ As String, _
            ByVal contrasena_ As String, _
            ByVal grupoempresarial_ As Integer, _
            ByVal divisionempresarial_ As Integer, _
            ByVal aplicacion_ As Integer, _
            ByVal idioma_ As ISesion.Idiomas
        )
            _claveUsuario = Nothing
            _usuario = Nothing
            _contrasenia = Nothing
            _grupoEmpresarial = Nothing
            _divisionEmpresarial = Nothing
            _aplicacion = Nothing
            _idioma = idioma_
            _estadoSesion = Nothing
            _gruposEmpresariales = Nothing

            'Configuración por defecto
            _caracteresUsuario = 6
            _numeroUsuario = 0
            _caracteresContrasenia = 6
            _mayusculasContrasenia = 1
            _minusculasContrasenia = 1
            _numerosContrasenia = 1
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property ContraseniaUsuario As String _
           Implements ISesion.ContraseniaUsuario
            Get
                Return Me._contrasenia
            End Get
            Set(ByVal value As String)
                Me._contrasenia = value
            End Set
        End Property

        Public Property IdentificadorUsuario As String _
            Implements ISesion.IdentificadorUsuario
            Get
                Return Me._usuario
            End Get
            Set(ByVal value As String)
                Me._usuario = value
            End Set
        End Property

        Public Property MininimoCaracteresUsuario As Integer _
            Implements ISesion.MininimoCaracteresUsuario
            Get
                Return Me._caracteresUsuario
            End Get
            Set(ByVal value As Integer)
                Me._caracteresUsuario = value
            End Set
        End Property

        Public Property MinimoNumerosUsuario As Integer _
            Implements ISesion.MinimoNumerosUsuario
            Get
                Return Me._numeroUsuario
            End Get
            Set(ByVal value As Integer)
                Me._numeroUsuario = value
            End Set
        End Property

        Public Property MinimoCaracteresContrasenia As Integer _
            Implements ISesion.MinimoCaracteresContrasena
            Get
                Return Me._caracteresContrasenia
            End Get
            Set(ByVal value As Integer)
                Me._caracteresContrasenia = value
            End Set
        End Property

        Public Property MinimoMayusculasContrasenia As Integer _
            Implements ISesion.MinimoMayusculasContrasena
            Get
                Return Me._mayusculasContrasenia
            End Get
            Set(ByVal value As Integer)
                Me._mayusculasContrasenia = value
            End Set
        End Property

        Public Property MinimoMinusculasContrasenia As Integer _
            Implements ISesion.MinimoMinusculasContrasena
            Get
                Return Me._minusculasContrasenia
            End Get
            Set(ByVal value As Integer)
                Me._minusculasContrasenia = value
            End Set
        End Property

        Public Property MinimoNumerosContrasenia As Integer _
            Implements ISesion.MinimoNumerosContrasena
            Get
                Return Me._numerosContrasenia
            End Get
            Set(ByVal value As Integer)
                Me._numerosContrasenia = value
            End Set
        End Property

        Public ReadOnly Property NombreAutenticacion As String _
            Implements ISesion.NombreAutenticacion
            Get
                Return Me._nombreAutenticacion
            End Get
        End Property

        Public ReadOnly Property ClaveUsuario As Integer _
            Implements ISesion.ClaveUsuario
            Get
                Return Me._claveUsuario
            End Get
        End Property

        Public ReadOnly Property EstatusArgumentos As Boolean _
            Implements ISesion.StatusArgumentos

            Get

                Try


                    ''Reclama estatus de argumentos, si logro autenticarse o no 
                    'If (Me.ValidarIdentificador(Me._usuario, Me._caracteresUsuario, Me._numeroUsuario) And _
                    '    Me.ValidarContrasenia(Me._contrasenia, Me._caracteresContrasenia, Me._mayusculasContrasenia, Me._minusculasContrasenia, Me._numerosContrasenia)
                    ') = True Then


                    '    'Inicia la sesion completa
                    '    Me._credenciales.CredencialUsuario = Me._usuario
                    '    Me._credenciales.ContraseniaUsuario = Me._contrasenia
                    '    Me._credenciales.GrupoEmpresarial = Me._grupoEmpresarial
                    '    Me._credenciales.DivisionEmpresaria = Me._divisionEmpresarial
                    '    Me._credenciales.Aplicacion = Me._aplicacion
                    '    'Me._credenciales.Idioma = Me._idioma
                    '    Me._credenciales.IniciarSesion()

                    '    'Verifica si la autenticacion fue exitosa para iniciar la contruccion del espacio de trabajo
                    '    If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                    '        Me._estadoSesion = True

                    '    Else
                    '        Me._estadoSesion = False
                    '    End If

                    'Else
                    '    Me._estadoSesion = False
                    '    Me._notificacion = ISesion.Notificaciones.CredencialesIncorrectas
                    'End If

                Catch ex As Exception

                End Try

                Return Me._estadoSesion

            End Get
        End Property

        Public Property DivisionEmpresarial As Integer _
            Implements ISesion.DivisionEmpresarial
            Get
                Return Me._divisionEmpresarial
            End Get
            Set(ByVal value As Integer)
                Me._divisionEmpresarial = value
            End Set
        End Property

        Public Property GrupoEmpresarial As Integer _
            Implements ISesion.GrupoEmpresarial
            Get
                Return Me._grupoEmpresarial
            End Get
            Set(ByVal value As Integer)
                Me._grupoEmpresarial = value
            End Set
        End Property

        Public Property Aplicacion As Integer _
            Implements ISesion.Aplicacion
            Get
                Return Me._aplicacion
            End Get
            Set(ByVal value As Integer)
                Me._aplicacion = value
            End Set
        End Property

        Public ReadOnly Property EspacioTrabajo As IEspacioTrabajo _
            Implements ISesion.EspacioTrabajo
            Get
                Return Me._espacioTrabajo
            End Get
        End Property

        Public ReadOnly Property GruposEmpresariales As DataSet _
            Implements ISesion.GruposEmpresariales
            Get
                Return Me._gruposEmpresariales
            End Get
        End Property

        Public Property Idioma As ISesion.Idiomas _
            Implements ISesion.Idioma
            Get
                Return _idioma
            End Get
            Set(value As ISesion.Idiomas)
                _idioma = value
            End Set
        End Property

        Public ReadOnly Property Notificacion As ISesion.Notificaciones _
            Implements ISesion.Notificacion
            Get
                Select Case _credenciales.EstadoAutenticacion
                    Case IIniciaSesion.StatusAutenticacion.Aceptado
                        _notificacion = ISesion.Notificaciones.SinNotificacion
                    Case IIniciaSesion.StatusAutenticacion.FalloConexion
                        _notificacion = ISesion.Notificaciones.FalloConexion
                    Case IIniciaSesion.StatusAutenticacion.Rechazado
                        _notificacion = ISesion.Notificaciones.CredencialesIncorrectas
                    Case IIniciaSesion.StatusAutenticacion.SinDatos
                        _notificacion = ISesion.Notificaciones.SinEspacioAsignado
                End Select

                Return _notificacion

            End Get
        End Property

#End Region

#Region "Metodos"

        Public Function SetURLUserPicture(ByVal MobileUserID As String, _
                                  ByVal WebServiceUserID As String, _
                                  ByVal WebServicePasswordID As String, _
                                  ByVal URLPicture_ As String) As Exceptions.TagWatcher _
                              Implements ISesion.SetURLUserPicture
            Dim _result As TagWatcher = New TagWatcher

            ModifyData(URLPicture_, "t_URLFotografia", MobileUserID, WebServiceUserID, WebServicePasswordID, _result)

            Return _result

        End Function

        Private Sub ModifyData(ByVal SetValue_ As String, _
                           ByVal FieldName_ As String, _
                                ByVal MobileUserID_ As String, _
                                ByVal WSUser_ As String, _
                                ByVal WSPassword_ As String, _
                                ByRef result_ As TagWatcher)

            Dim maintainfields_ As New List(Of String)

            Dim i_Cve_Persona_ As String = Nothing

            Dim i_Cve_InformacionPersonal_ As String = Nothing

            Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

            Dim _system As New Organismo

            temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

            Dim _iOperacionsVerifyMobileUser_ As IOperacionesCatalogo

            _iOperacionsVerifyMobileUser_ = _system.EnsamblaModulo("Usuarios")

            _iOperacionsVerifyMobileUser_.EspacioTrabajo = temporalWorkSpace_
            '_sesion.EspacioTrabajo
            ' temporalWorkSpace_

            _iOperacionsVerifyMobileUser_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            _iOperacionsVerifyMobileUser_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

            _iOperacionsVerifyMobileUser_.ClausulasLibres = " and t_Usuario = '" & MobileUserID_ & "'"

            _iOperacionsVerifyMobileUser_.GenerarVista()


            If _system.TieneResultados(_iOperacionsVerifyMobileUser_) Then
                'El usuario del móvil si existe

                'Buscamos el id de la persona
                'i_Cve_Persona_ = ReturnPersonID(MobileUserID_)

                i_Cve_Persona_ = _iOperacionsVerifyMobileUser_.Vista(0, "i_Cve_Persona", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                Dim _iOperacionsHelper_ As IOperacionesCatalogo

                _iOperacionsHelper_ = _system.EnsamblaModulo("InformacionPersonal")


                _iOperacionsHelper_.EspacioTrabajo = temporalWorkSpace_
                '_sesion.EspacioTrabajo
                ' temporalWorkSpace_

                _iOperacionsHelper_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                _iOperacionsHelper_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                _iOperacionsHelper_.ClausulasLibres = " and i_Cve_Estado = 1 and i_Cve_Persona = '" & i_Cve_Persona_ & "'"

                _iOperacionsHelper_.GenerarVista()

                'Verificamos si tiene registros o si hay que crear uno nuevo

                If _system.TieneResultados(_iOperacionsHelper_) Then

                    'En este caso solo hay que actualizar lo que se encuentra
                    'Definimos los campos requeridos
                    maintainfields_.Clear()

                    'campos_.Add("i_Cve_Persona")
                    maintainfields_.Add(FieldName_)

                    i_Cve_InformacionPersonal_ = _iOperacionsHelper_.Vista(0, "i_Cve_InformacionPersonal", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    _iOperacionsHelper_.PreparaCatalogo()

                    _system.OptimizaOperacion(_iOperacionsHelper_, maintainfields_, IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    _iOperacionsHelper_.CampoPorNombre(FieldName_) = SetValue_

                    'Console.Write("llege...")

                    If _iOperacionsHelper_.Modificar(i_Cve_InformacionPersonal_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        result_.SetOK()

                    Else

                        result_.SetError(ErrorTypes.WS010)


                    End If

                Else
                    'Definimos los campos requeridos
                    maintainfields_.Clear()

                    maintainfields_.Add("i_Cve_Persona")

                    maintainfields_.Add(FieldName_)

                    maintainfields_.Add("i_Cve_Estado")

                    maintainfields_.Add("i_Cve_Tipo")

                    'En este caso no hay registros en 'Informacion Personal' por lo que hay que ingresar una partida nueva

                    _iOperacionsHelper_.PreparaCatalogo()

                    _system.OptimizaOperacion(_iOperacionsHelper_, maintainfields_, IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    _iOperacionsHelper_.CampoPorNombre("i_Cve_Persona") = i_Cve_Persona_

                    _iOperacionsHelper_.CampoPorNombre(FieldName_) = SetValue_

                    _iOperacionsHelper_.CampoPorNombre("i_Cve_Estado") = 1

                    _iOperacionsHelper_.CampoPorNombre("i_Cve_Tipo") = 0


                    If _iOperacionsHelper_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        result_.SetOK()

                    Else

                        result_.SetError(ErrorTypes.WS011)

                    End If

                End If


            Else
                'El usuario del móvil no esta registrado

                result_.SetError(ErrorTypes.WS013)


            End If


        End Sub

        Public Function GetProfileWorkSpace(ByRef EspacioTrabajo_ As IEspacioTrabajo, _
                                            ByVal IDRequiredUser As String, _
                                ByVal WebServiceUserID As String, _
                                ByVal WebServicePasswordID As String, _
                                Optional ByVal IdRequiredApplication As Integer = 4, _
                                Optional ByVal CorporateNumber As Integer = 1, _
                                Optional ByVal CompanyId As Integer = 0, _
                                Optional ByVal FullAuthentication As Boolean = False) As UserProfile Implements ISesion.GetProfileWorkSpace ' Implements IWSSesion.GetProfile


            Dim _iOperations As IOperacionesCatalogo

            Dim cifrado256_ As ICifrado = New gsol.seguridad.Cifrado256()

            Dim _system As New Organismo

            Dim _userProfile As New UserProfile

            _iOperations = _system.EnsamblaModulo("Usuarios")

            Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

            temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

            _iOperations.EspacioTrabajo = temporalWorkSpace_

            _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

            _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            Dim contraseniaCifrada_ As String = cifrado256_.CifraCadena(WebServicePasswordID, ICifrado.Metodos.SHA1)

            _iOperations.ClausulasLibres = " and t_usuario = '" & IDRequiredUser & "' and t_contrasena = '" & contraseniaCifrada_ & "' "

            _iOperations.GenerarVista()

            If _system.TieneResultados(_iOperations) Then

                With _userProfile

                    .SessionType = UserProfile.SessionTypes.Standard

                    .Name = _iOperations.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    .MLastName = _iOperations.Vista(0, "t_ApellidoMaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    .PLastName = _iOperations.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    .RegisterDate = _iOperations.Vista(0, "f_FechaRegistro", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    'Additional information

                    .BirthDate = _iOperations.Vista(0, "f_FechaNacimiento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    '_userProfile.BirthDate = Now

                    .Phone = _iOperations.Vista(0, "t_TelefonoUno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                    '_userProfile.Phone = "UNDEFINED"

                    .CompanyName = _iOperations.Vista(0, "t_NombreEmpresa", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                    '_userProfile.CompanyName = "UNDEFINED"

                    .EMail = _iOperations.Vista(0, "t_CorreoElectronico", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                    '_userProfile.EMail = "UNDEFINED"

                    .URLUserPicture = _iOperations.Vista(0, "t_URLFotografia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                    '_userProfile.URLUserPicture = "UNDEFINED"

                End With

                If FullAuthentication = True Then

                    With _credenciales

                        'Inicia la sesion completa
                        .CredencialUsuario = IDRequiredUser
                        .ContraseniaUsuario = WebServicePasswordID
                        .GrupoEmpresarial = 1
                        .DivisionEmpresaria = 1
                        .Aplicacion = IdRequiredApplication ' JCCS 2022-03-30 Antes estaba hardcodiado la aplicación 12
                        'Me._credenciales.Idioma = Me._idioma
                        Me._credenciales.IniciarSesion()

                    End With


                    'Verifica si la autenticacion fue exitosa para iniciar la contruccion del espacio de trabajo
                    If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                        Me._estadoSesion = True
                        Me._nombreAutenticacion = Me._credenciales.NombreAutenticacion
                        Me._claveUsuario = Me._credenciales.ClaveUsuario

                        With EspacioTrabajo_

                            .Usuario = IDRequiredUser
                            .GrupoEmpresarial = 1
                            .DivisionEmpresarial = 1
                            .Aplicacion = IdRequiredApplication ' JCCS 2022-03-30 Antes estaba hardcodiado la aplicación 12
                            .Idioma = 1
                            .MisCredenciales = _credenciales

                        End With

                        '::: SUSPENDIDO PARA REVISIÓN :::::
                        Dim hilo_ As New Threading.Thread(AddressOf EspacioTrabajo_.GenerarEspacioTrabajo)
                        hilo_.IsBackground = True
                        hilo_.Start()
                        hilo_.Join()

                        'EspacioTrabajo_.GenerarEspacioTrabajo()


                        _iOperations.EspacioTrabajo = EspacioTrabajo_

                        _userProfile.Result.SetOK()

                    Else

                        _userProfile.Result.SetError(ErrorTypes.KBW_040_0000)

                    End If

                End If

            Else

                _userProfile.Result.SetError(ErrorTypes.KBW_040_0000)

            End If


            Return _userProfile

        End Function


        Public Function GetProfile(ByVal IDRequiredUser As String, _
                                ByVal WebServiceUserID As String, _
                                ByVal WebServicePasswordID As String, _
                                Optional ByVal IdRequiredApplication As Integer = 4, _
                                Optional ByVal CorporateNumber As Integer = 1, _
                                Optional ByVal CompanyId As Integer = 0, _
                                Optional ByVal FullAuthentication As Boolean = False) As UserProfile Implements ISesion.GetProfile ' Implements IWSSesion.GetProfile


            Dim _iOperations As IOperacionesCatalogo

            Dim cifrado256_ As ICifrado = New gsol.seguridad.Cifrado256()

            Dim _system As New Organismo

            Dim _userProfile As New UserProfile

            _iOperations = _system.EnsamblaModulo("Usuarios")

            Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

            temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

            _iOperations.EspacioTrabajo = temporalWorkSpace_

            _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

            _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            Dim contraseniaCifrada_ As String = cifrado256_.CifraCadena(WebServicePasswordID, ICifrado.Metodos.SHA1)

            _iOperations.ClausulasLibres = " and t_usuario = '" & IDRequiredUser & "' and t_contrasena = '" & contraseniaCifrada_ & "' "

            _iOperations.GenerarVista()

            If _system.TieneResultados(_iOperations) Then

                _userProfile.SessionType = UserProfile.SessionTypes.Standard

                _userProfile.Name = _iOperations.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                _userProfile.MLastName = _iOperations.Vista(0, "t_ApellidoMaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                _userProfile.PLastName = _iOperations.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                _userProfile.RegisterDate = _iOperations.Vista(0, "f_FechaRegistro", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                'Additional information

                _userProfile.BirthDate = _iOperations.Vista(0, "f_FechaNacimiento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                '_userProfile.BirthDate = Now

                _userProfile.Phone = _iOperations.Vista(0, "t_TelefonoUno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                '_userProfile.Phone = "UNDEFINED"

                _userProfile.CompanyName = _iOperations.Vista(0, "t_NombreEmpresa", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                '_userProfile.CompanyName = "UNDEFINED"

                _userProfile.EMail = _iOperations.Vista(0, "t_CorreoElectronico", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                '_userProfile.EMail = "UNDEFINED"

                _userProfile.URLUserPicture = _iOperations.Vista(0, "t_URLFotografia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                '_userProfile.URLUserPicture = "UNDEFINED"


                '_userProfile.Result.SetOK()



                If FullAuthentication = True Then

                    'Inicia la sesion completa
                    Me._credenciales.CredencialUsuario = IDRequiredUser
                    Me._credenciales.ContraseniaUsuario = WebServicePasswordID
                    Me._credenciales.GrupoEmpresarial = 1
                    Me._credenciales.DivisionEmpresaria = 1
                    Me._credenciales.Aplicacion = IdRequiredApplication
                    'Me._credenciales.Idioma = Me._idioma
                    Me._credenciales.IniciarSesion()

                    'Verifica si la autenticacion fue exitosa para iniciar la contruccion del espacio de trabajo
                    If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                        Me._estadoSesion = True
                        Me._nombreAutenticacion = Me._credenciales.NombreAutenticacion
                        Me._claveUsuario = Me._credenciales.ClaveUsuario

                        Me._espacioTrabajo = New EspacioTrabajo
                        Me._espacioTrabajo.Usuario = IDRequiredUser
                        Me._espacioTrabajo.GrupoEmpresarial = 1
                        Me._espacioTrabajo.DivisionEmpresarial = 1
                        Me._espacioTrabajo.Aplicacion = IdRequiredApplication
                        Me._espacioTrabajo.Idioma = 1
                        Me._espacioTrabajo.MisCredenciales = _credenciales


                        '::: SUSPENDIDO PARA REVISIÓN :::::
                        Dim hilo_ As New Threading.Thread(AddressOf Me._espacioTrabajo.GenerarEspacioTrabajo)
                        hilo_.IsBackground = True
                        hilo_.Start()
                        hilo_.Join()

                        'Me._espacioTrabajo.GenerarEspacioTrabajo()

                        _iOperations.EspacioTrabajo = _espacioTrabajo

                        _userProfile.Result.SetOK()

                    Else

                        _userProfile.Result.SetError(ErrorTypes.KBW_040_0000)

                    End If

                End If

            Else

                _userProfile.Result.SetError(ErrorTypes.KBW_040_0000)

            End If


            Return _userProfile

        End Function

        Private Function ValidarIdentificador(
            ByRef identificador_ As String,
            ByRef minimoCaracteres_ As Integer,
            ByRef minimoNumeros_ As Integer
        ) As Boolean
            '  Permite determinar si la cadena usuario_ contiene algun numero
            Dim numero_ As New System.Text.RegularExpressions.Regex("[0-9]")

            ' Se compara el numero de caracteres de la cadena usuario_  cuneta con el minimo permitido 6
            If Len(identificador_) < minimoCaracteres_ Then Return False

            ' Se comparan si se cumplen con el minimo permitido de numeros.
            If Me._numeroUsuario = 0 Then
                If numero_.Matches(identificador_).Count > minimoNumeros_ Then Return False
            End If

            If _numeroUsuario > 0 Then
                If numero_.Matches(identificador_).Count < minimoNumeros_ Then Return False
            End If

            ' Si cumple con todos los parametros retorna Verdadero.
            Return True
        End Function

        Private Function ValidarContrasenia(
            ByVal contrasenia_ As String,
            ByRef minimoCaracteres_ As Integer,
            ByRef minimoMayusculas_ As Integer,
            ByRef minimoMinusculas_ As Integer,
            ByRef minimoNumeros_ As Integer
        ) As Boolean
            '  Permite determinar si la cadena contrasena_ contiene alguna mayuscula, minuscula o numero
            Dim mayusculas_ As New System.Text.RegularExpressions.Regex("[A-Z]")
            Dim minusculas_ As New System.Text.RegularExpressions.Regex("[a-z]")
            Dim numeros_ As New System.Text.RegularExpressions.Regex("[0-9]")

            ' Se compara el numero de caracteres de la cadena contrasena_  cuneta con el minimo permitido 6
            If Len(contrasenia_) < minimoCaracteres_ Then Return False
            ' Se comparan si se cumplen con el minimo permitido de mayusculas, minusculas o numeros.
            If mayusculas_.Matches(contrasenia_).Count < minimoMayusculas_ Then Return False
            If minusculas_.Matches(contrasenia_).Count < minimoMinusculas_ Then Return False
            If numeros_.Matches(contrasenia_).Count < minimoNumeros_ Then Return False

            ' Si cumple con todos los parametros retorna Verdadero.
            Return True
        End Function

#End Region

        Public Sub iniciar() Implements ISesion.iniciar

        End Sub
    End Class

End Namespace
