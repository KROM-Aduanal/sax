
Imports Gsol.BaseDatos.Operaciones

Namespace Wma.WebServices
    ' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    Public Class WSSesion
        Implements IWSSesion

        Private _iOperations As IOperacionesCatalogo

        Private _system As New Organismo

        Public Shared _sesion As ISesion

        Private _userProfile As UserProfile

        Private _sessionStatus As Boolean

        Sub New()

            _sesion = New Sesion

            _iOperations = New OperacionesCatalogo

            _userProfile = New UserProfile

        End Sub

        Private Sub SessionPrepare(ByVal user_ As String, _
                                   ByVal password_ As String)

            'Sign-In
            _sesion = New Sesion

            _sesion.MininimoCaracteresUsuario = 7

            _sesion.MinimoNumerosUsuario = 0

            _sesion.MinimoCaracteresContrasena = 7

            _sesion.MinimoMayusculasContrasena = 2

            _sesion.MinimoMinusculasContrasena = 2

            _sesion.MinimoNumerosContrasena = 2



            _sesion.GrupoEmpresarial = Nothing

            _sesion.DivisionEmpresarial = Nothing

            _sesion.Aplicacion = Nothing


            _sesion.IdentificadorUsuario = Trim(user_)

            _sesion.ContraseniaUsuario = Trim(password_)

        End Sub

        Public Function GetProfile(ByVal IDRequiredUser As String, _
                                    ByVal WebServiceUserID As String, _
                                    ByVal WebServicePasswordID As String, _
                                    ByVal IdRequiredApplication As Integer, _
                                    ByVal CorporateNumber As Integer, _
                                    ByVal CompanyId As Integer, _
                                    Optional ByVal FullAuthentication As Boolean = False) As UserProfile Implements IWSSesion.GetProfile

            _userProfile = New UserProfile

            _userProfile.Nombre = Nothing

            _userProfile.ApellidoMaterno = Nothing

            _userProfile.ApellidoParterno = Nothing

            _sessionStatus = False

            'Preparing session 
            SessionPrepare(WebServiceUserID, _
                           WebServicePasswordID)


            'First recognition
            If _sesion.StatusArgumentos Then

                _sessionStatus = True

                _sesion.GrupoEmpresarial = CorporateNumber

                _sesion.DivisionEmpresarial = CompanyId

                _sesion.Aplicacion = IdRequiredApplication

                _sesion.Idioma = ISesion.Idiomas.Espaniol


                If Not FullAuthentication Then

                    '_system.EnsamblaModulo("Usuarios")

                    'Dim buscadorDatos_ As IOperacionesCatalogo

                    _iOperations = _system.EnsamblaModulo("Usuarios")

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                    temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    _iOperations.EspacioTrabajo = temporalWorkSpace_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.ClausulasLibres = " and t_usuario = '" & IDRequiredUser & "'"

                    _iOperations.GenerarVista()

                    If _system.TieneResultados(_iOperations) Then

                        _userProfile.Nombre = _iOperations.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _userProfile.ApellidoMaterno = _iOperations.Vista(0, "t_ApellidoMaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _userProfile.ApellidoParterno = _iOperations.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _userProfile.FechaRegistro = _iOperations.Vista(0, "f_FechaRegistro", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _userProfile.SetOK()

                    Else

                        _userProfile.SetError(AboutAsking.ErrorTypes.WS001)

                        '_userProfile.ResponseStatus = "ERR250:Information not available"

                    End If

                Else

                    'Second regnition
                    _sessionStatus = _sesion.StatusArgumentos

                    If _sessionStatus Then

                        If _sesion.StatusArgumentos = True And _
                            _sesion.EspacioTrabajo IsNot Nothing Then

                            'Initializing
                            _iOperations = New OperacionesCatalogo

                            _iOperations = _system.ConsultaModulo(_sesion.EspacioTrabajo, "Usuarios", " and t_usuario = '" & IDRequiredUser & "'").Clone

                            If _system.TieneResultados(_iOperations) Then

                                _userProfile.Nombre = _iOperations.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                _userProfile.ApellidoMaterno = _iOperations.Vista(0, "t_ApellidoMaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                _userProfile.ApellidoParterno = _iOperations.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                _userProfile.FechaRegistro = _iOperations.Vista(0, "f_FechaRegistro", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            Else
                                '                                _userProfile.ResponseStatus = "ERR250:Information not available"
                                _userProfile.SetError(AboutAsking.ErrorTypes.WS002)

                            End If

                        Else

                            _userProfile.SetError(AboutAsking.ErrorTypes.WS003)

                        End If


                    Else

                        _userProfile.SetError(AboutAsking.ErrorTypes.WS004)

                    End If

                End If
            Else

                'Response for bad recognition
                _userProfile.SetError(AboutAsking.ErrorTypes.WS003)

            End If

            Return _userProfile

        End Function

        Public Function TesterService42(YourName As String) As String Implements IWSSesion.TesterService42

            Return "It Works " & YourName

        End Function

    End Class

End Namespace
