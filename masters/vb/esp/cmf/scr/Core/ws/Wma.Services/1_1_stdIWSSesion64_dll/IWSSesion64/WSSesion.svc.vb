Imports System.ServiceModel
Imports gsol.Organismo
Imports gsol.BaseDatos.Operaciones
Imports System.ComponentModel
'Imports System.Xml.Serialization
'Imports System.Reflection
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions
Imports gsol
Imports System.Globalization

Namespace Wma.WebServices

    Public Class WSSesion
        Implements IWSSesion

#Region "Attributes"

        Private _iOperations As IOperacionesCatalogo

        Private _system As New Organismo

        Public Shared _sesion As ISesionWcf

        Private _userProfile As UserProfile

        Private _sessionStatus As Boolean

#End Region

#Region "Builders"

        Sub New()

            _sesion = New SesionWcf

            _iOperations = New OperacionesCatalogo

            _userProfile = New UserProfile

        End Sub

#End Region

#Region "Methods"

        Private Sub SessionPrepare(ByVal user_ As String, _
                                   ByVal password_ As String)

            'Sign-In
            _sesion = New SesionWcf

            _sesion.MininimoCaracteresUsuario = 7

            _sesion.MinimoNumerosUsuario = 0

            _sesion.MinimoCaracteresContrasena = 7

            _sesion.MinimoMayusculasContrasena = 2

            _sesion.MinimoMinusculasContrasena = 2

            _sesion.MinimoNumerosContrasena = 2

            _sesion.GrupoEmpresarial = 1 'Nothing 'RIGG

            _sesion.DivisionEmpresarial = 1 'Nothing 'RIGG

            _sesion.Aplicacion = 4 'Nothing 'RIGG

            _sesion.IdentificadorUsuario = Trim(user_)

            _sesion.ContraseniaUsuario = Trim(password_)

        End Sub

        Public Function GetProfile(ByVal IDRequiredUser As String, _
                                    ByVal WebServiceUserID As String, _
                                    ByVal WebServicePasswordID As String, _
                                    Optional ByVal IdRequiredApplication As String = Nothing, _
                                    Optional ByVal CorporateNumber As String = Nothing, _
                                    Optional ByVal CompanyId As String = Nothing, _
                                    Optional ByVal FullAuthentication_ As String = Nothing) As UserProfile Implements IWSSesion.GetProfile

            _sessionStatus = False

            'Preparing session 

            '_system.Log("Prepara session de  " & IDRequiredUser, 14, 1)

            SessionPrepare(WebServiceUserID, _
                           WebServicePasswordID)

            'First recognition
            If _sesion.StatusArgumentos Then

                '_system.Log("Sesion Iniciada en WCF " & IDRequiredUser, 14, 1)

                _sessionStatus = True

                _sesion.GrupoEmpresarial = CorporateNumber

                _sesion.DivisionEmpresarial = CompanyId

                _sesion.Aplicacion = IdRequiredApplication

                _sesion.Idioma = ISesionWcf.Idiomas.Espaniol

                '_sesion.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Produccion


                'Obligo por autenticacion basica
                Dim FullAuthentication As Boolean = False  'RIGG

                If Not FullAuthentication Then

                    '_system.EnsamblaModulo("Usuarios")

                    'Dim buscadorDatos_ As IOperacionesCatalogo

                    _iOperations = _system.EnsamblaModulo("Usuarios")



                    'Dim system_ As New Organismo
                    'system_.Log("Sesion Iniciada en WCF " & IDRequiredUser, 14, 1)

                    '_system.Log("hola", monitoreo.IBitacoras.TiposBitacora.Informacion, monitoreo.IBitacoras.TiposSucesos.Otros)

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                    temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    _iOperations.EspacioTrabajo = temporalWorkSpace_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.ClausulasLibres = " and t_usuario = '" & IDRequiredUser & "'"

                    'Try
                    '_iOperations.GenerarVista()

                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        If _system.TieneResultados(_iOperations) Then
                            '_userProfile = New UserProfile

                            '_userProfile.Name = Nothing

                            '_userProfile.MLastName = Nothing

                            '_userProfile.PLastName = Nothing

                            _userProfile.SessionType = UserProfile.SessionTypes.Standard

                            _userProfile.Name = _iOperations.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            _userProfile.MLastName = _iOperations.Vista(0, "t_ApellidoMaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            _userProfile.PLastName = _iOperations.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            _userProfile.RegisterDate = _iOperations.Vista(0, "f_FechaRegistro", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            'Additional information

                            If Not String.IsNullOrEmpty(_iOperations.Vista(0, "f_FechaNacimiento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)) Then
                                Dim Aux As Date = Nothing
                                Date.TryParse(_iOperations.Vista(0, "f_FechaNacimiento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico), Aux)
                                _userProfile.BirthDate = Aux.ToString("dd-MM-yyyy")
                            Else : _userProfile.BirthDate = Nothing
                            End If

                            '_userProfile.BirthDate = _iOperations.Vista(0, "f_FechaNacimiento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            '_userProfile.BirthDate = Now

                            _userProfile.Phone = _iOperations.Vista(0, "t_TelefonoUno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                            '_userProfile.Phone = "UNDEFINED"

                            _userProfile.CompanyName = _iOperations.Vista(0, "t_NombreEmpresa", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                            '_userProfile.CompanyName = "UNDEFINED"

                            _userProfile.EMail = _iOperations.Vista(0, "t_CorreoElectronico", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                            '_userProfile.EMail = "UNDEFINED"

                            _userProfile.URLUserPicture = _iOperations.Vista(0, "t_URLFotografia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                            '_userProfile.URLUserPicture = "UNDEFINED"

                            _userProfile.Result.SetOK()

                        Else

                            _userProfile.Result.SetError(ErrorTypes.WS001)

                            '_userProfile.ResponseStatus = "ERR250:Information not available"

                        End If
                    Else
                        _userProfile.Result.SetError(ErrorTypes.WS001)
                    End If




                    ' Catch ex As Exception
                    '_userProfile.Result.SetError(ErrorTypes.WS001)
                    'End Try




                Else

                    'Second regnition
                    _sessionStatus = _sesion.StatusArgumentos

                    If _sessionStatus Then

                        If _sesion.StatusArgumentos = True And _
                            _sesion.EspacioTrabajo IsNot Nothing Then

                            '_userProfile = New UserProfile

                            '_userProfile.Name = Nothing

                            '_userProfile.MLastName = Nothing

                            '_userProfile.PLastName = Nothing


                            'Initializing
                            _iOperations = New OperacionesCatalogo

                            _iOperations = _system.ConsultaModulo(_sesion.EspacioTrabajo, "Usuarios", " and t_usuario = '" & IDRequiredUser & "'").Clone

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

                            Else

                                _userProfile.Result.SetError(ErrorTypes.WS002)

                            End If

                        Else

                            _userProfile.Result.SetError(ErrorTypes.WS003)

                        End If


                    Else

                        _userProfile.Result.SetError(ErrorTypes.WS004)

                    End If

                End If
            Else

                'Response for bad recognition
                _userProfile.Result.SetError(ErrorTypes.WS003)

            End If

            Return _userProfile

        End Function

        Public Function TesterService(YourName As String) As String Implements IWSSesion.TesterService

            Return "It Works " & YourName

        End Function


        Private Function IsValidAuthentication(ByVal ID As String, _
                                               ByVal PWD As String) As Boolean


            Dim answer_ As Boolean = _sessionStatus

            If _sessionStatus = False Then


                'Preparing session 
                SessionPrepare(ID, PWD)

                'First recognition
                answer_ = _sesion.StatusArgumentos

                If answer_ Then

                    _sesion.GrupoEmpresarial = 1

                    _sesion.DivisionEmpresarial = 1

                    _sesion.Aplicacion = 4

                    _sesion.Idioma = ISesionWcf.Idiomas.Espaniol

                    'Second regnition
                    'If _sesion.StatusArgumentos = True And _
                    '    _sesion.EspacioTrabajo IsNot Nothing Then

                    _sessionStatus = True

                    answer_ = _sessionStatus

                    'Else

                    '    answer_ = False

                    'End If

                Else

                    answer_ = False

                End If

            End If

            Return answer_

        End Function



        Private Sub ModifyData(ByVal SetValue_ As String, _
                               ByVal FieldName_ As String, _
                                    ByVal MobileUserID_ As String, _
                                    ByVal WSUser_ As String, _
                                    ByVal WSPassword_ As String, _
                                    ByRef result_ As TagWatcher)

            Dim maintainfields_ As New List(Of String)

            If IsValidAuthentication(WSUser_, WSPassword_) Then

                Dim i_Cve_Persona_ As String = Nothing

                Dim i_Cve_InformacionPersonal_ As String = Nothing

                Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas




                Dim _iOperacionsVerifyMobileUser_ As IOperacionesCatalogo

                _iOperacionsVerifyMobileUser_ = _system.EnsamblaModulo("Usuarios")

                _iOperacionsVerifyMobileUser_.EspacioTrabajo = temporalWorkSpace_
                '_sesion.EspacioTrabajo
                ' temporalWorkSpace_

                _iOperacionsVerifyMobileUser_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                _iOperacionsVerifyMobileUser_.ClausulasLibres = " and t_Usuario = '" & MobileUserID_ & "'"

                _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

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



                    _iOperacionsHelper_.ClausulasLibres = " and i_Cve_Estado = 1 and i_Cve_Persona = '" & i_Cve_Persona_ & "'"

                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

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

            Else

                result_.SetError(ErrorTypes.WS012)

            End If
        End Sub


        Public Function SetBirthDate(ByVal MobileUserID As String, _
                                     ByVal WebServiceUserID As String, _
                                     ByVal WebServicePasswordID As String, _
                                     ByVal BirthDate_ As String) As TagWatcher _
                                 Implements IWSSesion.SetBirthDate

            Dim _result As TagWatcher = New TagWatcher

            ModifyData(BirthDate_, "f_FechaNacimiento", MobileUserID, WebServiceUserID, WebServicePasswordID, _result)

            Return _result

        End Function


        Public Function SetEMail(ByVal MobileUserID As String, _
                                 ByVal WebServiceUserID As String, _
                                 ByVal WebServicePasswordID As String, _
                                 ByVal EMail_ As String) As TagWatcher _
                             Implements IWSSesion.SetEMail

            Dim _result As TagWatcher = New TagWatcher

            ModifyData(EMail_, "t_CorreoElectronico", MobileUserID, WebServiceUserID, WebServicePasswordID, _result)

            Return _result


        End Function

        Public Function SetPhone(ByVal MobileUserID As String, _
                                 ByVal WebServiceUserID As String, _
                                 ByVal WebServicePasswordID As String, _
                                 ByVal Phone_ As String) As TagWatcher _
                             Implements IWSSesion.SetPhone

            Dim _result As TagWatcher = New TagWatcher

            ModifyData(Phone_, "t_TelefonoUno", MobileUserID, WebServiceUserID, WebServicePasswordID, _result)

            Return _result

        End Function

        Public Function SetURLUserPicture(ByVal MobileUserID As String, _
                                          ByVal WebServiceUserID As String, _
                                          ByVal WebServicePasswordID As String, _
                                          ByVal URLPicture_ As String) As TagWatcher _
                                      Implements IWSSesion.SetURLUserPicture
            Dim _result As TagWatcher = New TagWatcher

            ModifyData(URLPicture_, "t_URLFotografia", MobileUserID, WebServiceUserID, WebServicePasswordID, _result)

            Return _result

        End Function

#End Region

    End Class

End Namespace

