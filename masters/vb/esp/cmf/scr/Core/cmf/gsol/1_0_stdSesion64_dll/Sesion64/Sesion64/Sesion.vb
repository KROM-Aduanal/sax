Namespace gsol

    Public Class Sesion
        'Inherits Organismo 'MOP, Se quito la herencia porque ya no se utilizarán los Logs habituales. 18122021
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
            ByVal usuario_ As String,
            ByVal contrasena_ As String,
            ByVal grupoempresarial_ As Integer,
            ByVal divisionempresarial_ As Integer,
            ByVal aplicacion_ As Integer,
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

        Sub iniciar() Implements ISesion.Iniciar


            ' MsgBox("Yaaas2")

            Try


                'Reclama estatus de argumentos, si logro autenticarse o no 
                If (Me.ValidarIdentificador(Me._usuario, Me._caracteresUsuario, Me._numeroUsuario) And
                    Me.ValidarContrasenia(Me._contrasenia, Me._caracteresContrasenia, Me._mayusculasContrasenia, Me._minusculasContrasenia, Me._numerosContrasenia)
                ) = True Then

                    'Identiica si los datos son suficientes para iniciar la autenticacion o si es necesario devolver opciones a las que tiene permiso
                    If (Me._grupoEmpresarial = 0 And Me._divisionEmpresarial = 0 And Me._aplicacion = 0) Then
                        'Inicia la sesion basica
                        Me._credenciales.CredencialUsuario = Me._usuario
                        Me._credenciales.ContraseniaUsuario = Me._contrasenia
                        Me._credenciales.CargarGruposEmpresariales()

                        If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                            Me._gruposEmpresariales = _credenciales.GruposEmpresariales
                            Me._estadoSesion = True
                        End If

                    Else
                        'Inicia la sesion completa
                        Me._credenciales.CredencialUsuario = Me._usuario
                        Me._credenciales.ContraseniaUsuario = Me._contrasenia
                        Me._credenciales.GrupoEmpresarial = Me._grupoEmpresarial
                        Me._credenciales.DivisionEmpresaria = Me._divisionEmpresarial
                        Me._credenciales.Aplicacion = Me._aplicacion
                        'Me._credenciales.Idioma = Me._idioma
                        Me._credenciales.IniciarSesion()

                        'Verifica si la autenticacion fue exitosa para iniciar la contruccion del espacio de trabajo
                        If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                            Me._estadoSesion = True
                            Me._nombreAutenticacion = Me._credenciales.NombreAutenticacion
                            Me._claveUsuario = Me._credenciales.ClaveUsuario
                            Me._espacioTrabajo = New EspacioTrabajo
                            Me._espacioTrabajo.Usuario = Me._usuario

                            Me._espacioTrabajo.ClaveEjecutivo = _credenciales.ClaveEjecutivo


                            Me._espacioTrabajo.GrupoEmpresarial = Me._grupoEmpresarial
                            Me._espacioTrabajo.DivisionEmpresarial = Me._divisionEmpresarial
                            Me._espacioTrabajo.Aplicacion = Me._aplicacion
                            Me._espacioTrabajo.Idioma = Me.Idioma
                            Me._espacioTrabajo.MisCredenciales = _credenciales


                            ''::: SUSPENDIDO PARA REVISIÓN :::::02/07/2020 MOP
                            Dim hilo_ As New Threading.Thread(AddressOf Me._espacioTrabajo.GenerarEspacioTrabajo)
                            hilo_.IsBackground = True
                            hilo_.Start()
                            hilo_.Priority = Threading.ThreadPriority.Highest
                            hilo_.Join()

                            'Me._espacioTrabajo.GenerarEspacioTrabajo()



                            If Me._espacioTrabajo.SectorEntorno.Count = 0 Or
                                Me._espacioTrabajo.SectorEntorno Is Nothing Then
                                'No se generaron los sectores, la sesión no pudo crearse
                                'Dim respuesta_ As Boolean = False

                                Me._estadoSesion = False

                            Else

                                Me._estadoSesion = True

                                'Documentar la validacion 
                                'Log("Autenticacion exitosa de: " & Me._usuario,
                                '    monitoreo.IBitacoras.TiposBitacora.Informacion,
                                '    monitoreo.IBitacoras.TiposSucesos.Consultar,
                                '    _credenciales, Nothing, "NombreAutenticacion: " & NombreAutenticacion & "; Usuario: " _
                                '    & Me._usuario _
                                '    & "; GrupoEmpresarial: " & Me._grupoEmpresarial & "; DivisionEmpresarial: " _
                                '    & Me._divisionEmpresarial _
                                '    & "; Aplicacion: " & Me._aplicacion, "InicioSesion", NombreAutenticacion)

                            End If


                        Else

                            Me._estadoSesion = False

                        End If

                    End If


                Else
                    Me._estadoSesion = False

                    Me._notificacion = ISesion.Notificaciones.CredencialesIncorrectas
                End If

            Catch ex As Exception

                'Log(ex.ToString,
                '    monitoreo.IBitacoras.TiposBitacora.Errores,
                '    monitoreo.IBitacoras.TiposSucesos.Consultar,
                '    _credenciales,
                '    Nothing,
                '    "_usuario: " & _usuario & "; _contrasenia: " & _contrasenia & "; _grupoEmpresarial: " & _grupoEmpresarial _
                '    & "; _divisionEmpresarial: " & "; _aplicacion: " & _aplicacion)
            End Try


            ' _estadoSesion = estadoSesion


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
                'MsgBox("Yaaas")

                Try


                    'Reclama estatus de argumentos, si logro autenticarse o no 
                    If (Me.ValidarIdentificador(Me._usuario, Me._caracteresUsuario, Me._numeroUsuario) And
                        Me.ValidarContrasenia(Me._contrasenia, Me._caracteresContrasenia, Me._mayusculasContrasenia, Me._minusculasContrasenia, Me._numerosContrasenia)
                    ) = True Then

                        'Identiica si los datos son suficientes para iniciar la autenticacion o si es necesario devolver opciones a las que tiene permiso
                        If (Me._grupoEmpresarial = 0 And Me._divisionEmpresarial = 0 And Me._aplicacion = 0) Then
                            'Inicia la sesion basica
                            Me._credenciales.CredencialUsuario = Me._usuario
                            Me._credenciales.ContraseniaUsuario = Me._contrasenia
                            Me._credenciales.CargarGruposEmpresariales()

                            If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                                Me._gruposEmpresariales = _credenciales.GruposEmpresariales
                                Me._estadoSesion = True
                            End If

                        Else
                            'Inicia la sesion completa
                            Me._credenciales.CredencialUsuario = Me._usuario
                            Me._credenciales.ContraseniaUsuario = Me._contrasenia
                            Me._credenciales.GrupoEmpresarial = Me._grupoEmpresarial
                            Me._credenciales.DivisionEmpresaria = Me._divisionEmpresarial
                            Me._credenciales.Aplicacion = Me._aplicacion
                            'Me._credenciales.Idioma = Me._idioma
                            Me._credenciales.IniciarSesion()

                            'Verifica si la autenticacion fue exitosa para iniciar la contruccion del espacio de trabajo
                            If Me._credenciales.EstadoAutenticacion = IIniciaSesion.StatusAutenticacion.Aceptado Then
                                Me._estadoSesion = True
                                Me._nombreAutenticacion = Me._credenciales.NombreAutenticacion
                                Me._claveUsuario = Me._credenciales.ClaveUsuario
                                Me._espacioTrabajo = New EspacioTrabajo
                                Me._espacioTrabajo.Usuario = Me._usuario

                                Me._espacioTrabajo.ClaveEjecutivo = _credenciales.ClaveEjecutivo


                                Me._espacioTrabajo.GrupoEmpresarial = Me._grupoEmpresarial
                                Me._espacioTrabajo.DivisionEmpresarial = Me._divisionEmpresarial
                                Me._espacioTrabajo.Aplicacion = Me._aplicacion
                                Me._espacioTrabajo.Idioma = Me.Idioma
                                Me._espacioTrabajo.MisCredenciales = _credenciales


                                ''::: SUSPENDIDO PARA REVISIÓN :::::02/07/2020 MOP
                                Dim hilo_ As New Threading.Thread(AddressOf Me._espacioTrabajo.GenerarEspacioTrabajo)
                                hilo_.IsBackground = True
                                hilo_.Start()
                                hilo_.Priority = Threading.ThreadPriority.Highest
                                hilo_.Join()

                                'Me._espacioTrabajo.GenerarEspacioTrabajo()



                                If Me._espacioTrabajo.SectorEntorno.Count = 0 Or
                                    Me._espacioTrabajo.SectorEntorno Is Nothing Then
                                    'No se generaron los sectores, la sesión no pudo crearse
                                    'Dim respuesta_ As Boolean = False

                                    Me._estadoSesion = False

                                Else

                                    Me._estadoSesion = True

                                    'Documentar la validacion 
                                    'Log("Autenticacion exitosa de: " & Me._usuario,
                                    '    monitoreo.IBitacoras.TiposBitacora.Informacion,
                                    '    monitoreo.IBitacoras.TiposSucesos.Consultar,
                                    '    _credenciales, Nothing, "NombreAutenticacion: " & NombreAutenticacion & "; Usuario: " _
                                    '    & Me._usuario _
                                    '    & "; GrupoEmpresarial: " & Me._grupoEmpresarial & "; DivisionEmpresarial: " _
                                    '    & Me._divisionEmpresarial _
                                    '    & "; Aplicacion: " & Me._aplicacion, "InicioSesion", NombreAutenticacion)

                                End If


                            Else

                                Me._estadoSesion = False

                            End If

                        End If


                    Else
                        Me._estadoSesion = False

                        Me._notificacion = ISesion.Notificaciones.CredencialesIncorrectas
                    End If

                Catch ex As Exception

                    'Log(ex.ToString,
                    '    monitoreo.IBitacoras.TiposBitacora.Errores,
                    '    monitoreo.IBitacoras.TiposSucesos.Consultar,
                    '    _credenciales,
                    '    Nothing,
                    '    "_usuario: " & _usuario & "; _contrasenia: " & _contrasenia & "; _grupoEmpresarial: " & _grupoEmpresarial _
                    '    & "; _divisionEmpresarial: " & "; _aplicacion: " & _aplicacion)
                End Try

                Return Me._estadoSesion
                ' Return Me._notificacion
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
            ' 2021-10-04 - JC - Se comentan estas dos validaciones debido a que no deja pasar a los usuarios que contienen números, por ejemplo: practicante.finanzas02@kromaduanal.com
            'If Me._numeroUsuario = 0 Then
            '    If numero_.Matches(identificador_).Count > minimoNumeros_ Then Return False
            'End If

            'If _numeroUsuario > 0 Then
            '    If numero_.Matches(identificador_).Count < minimoNumeros_ Then Return False
            'End If

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


        Public Function GetProfile(IDRequiredUser As String, WebServiceUserID As String, WebServicePasswordID As String, Optional IdRequiredApplication As Integer = 4, Optional CorporateNumber As Integer = 1, Optional CompanyId As Integer = 0, Optional FullAuthentication As Boolean = False) As Wma.WebServices.UserProfile Implements ISesion.GetProfile

        End Function

        Public Function SetURLUserPicture(MobileUserID As String, WebServiceUserID As String, WebServicePasswordID As String, URLPicture_ As String) As Wma.Exceptions.TagWatcher Implements ISesion.SetURLUserPicture

        End Function

        Public Function GetProfileWorkSpace(ByRef EspacioTrabajo As IEspacioTrabajo, IDRequiredUser As String, WebServiceUserID As String, WebServicePasswordID As String, Optional IdRequiredApplication As Integer = 4, Optional CorporateNumber As Integer = 1, Optional CompanyId As Integer = 0, Optional FullAuthentication As Boolean = False) As Wma.WebServices.UserProfile Implements ISesion.GetProfileWorkSpace

        End Function

    End Class

End Namespace
