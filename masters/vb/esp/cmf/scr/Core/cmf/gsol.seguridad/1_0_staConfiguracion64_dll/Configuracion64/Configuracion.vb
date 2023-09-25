Imports Wma.Exceptions

Namespace gsol.seguridad

    Public Class Configuracion

#Region "Constantes"

#End Region

#Region "Enums"

        Public Enum DatosGlobalesSistema
            LlaveCifrado = 1
            ClaveWebservices = 2
            ModalidadServicio = 3
            DireccionIPServidorSQLServerGeneralProduccion = 4
            UsuarioSQLServerGeneralProduccion = 5
            ClaveSQLServerGeneralProduccion = 6
            BaseDatosSQLServerProduccion = 7
            Oficina = 8
            DireccionIPServidorMYSQLGeneralProduccion = 9
            UsuarioMYSQLGeneralProduccion = 10
            ClaveMYSQLGeneralProduccion = 11
            BaseDatosMYSQLProduccion = 12
            DireccionServidorVFPGeneralProduccion = 13
            Usuario = 14
            Contrasena = 15
            GrupoEmpresarial = 16
            Aplicacion = 17
            EndPoint = 18
            ContrasenaWebService = 19
            LicenciaTagCode = 20
            B1DireccionIPSQLServer = 21
            B1NombreSQLServer = 22
            B1UsuarioSQLServer = 23
            B1PasswordSQLServer = 24
            BDIDireccionIPSQLServer = 25
            BDINombreSQLServer = 26
            BDIUsuarioSQLServer = 27
            BDIPasswordSQLServer = 28
            BMDDireccionIPMongoDB = 29
            BMDNombreMongoDB = 30
            BMDUsuarioMongoDB = 31
            BMDPasswordMongoDB = 32
            ControlarBitacoraDesdeLlaveCentral = 33
            AplicacionInicial = 34
            ActivarLogGeneral = 35
            RutaLogGeneral = 36
            ActivarLogAdministrativo = 37
            RutaLogAdministrativo = 38
            ActivarBitacoraBasica = 39
            ActivarBitacoraGeneral = 40
        End Enum

#End Region

#Region "Atributos"

        Public Shared _modalidadServicio As String

        Public Shared _direccionIPServidorSQLServerGeneralProduccion As String

        Public Shared _usuarioSQLServerGeneralProduccion As String

        Public Shared _claveSQLServerGeneralProduccion As String

        Public Shared _baseDatosSQLServerProduccion As String

        Public Shared _oficina As String

        Public Shared _direccionIPServidorMYSQLGeneralProduccion As String

        Public Shared _usuarioMYSQLGeneralProduccion As String

        Public Shared _claveMYSQLGeneralProduccion As String

        Public Shared _baseDatosMYSQLProduccion As String

        Public Shared _direccionServidorVFPGeneralProduccion As String

        Public Shared _usuario As String

        Public Shared _contrasena As String

        Public Shared _grupoEmpresarial As String

        Public Shared _aplicacion As String

        Public Shared _endPoint As String

        Public Shared _contrasenaWebService As String

        Public Shared _configuracion As LeerArchivoXML32

        Public Shared _status As TagWatcher

        Public Shared _licenciaTagCode As String

        'nuevos elementos MOP 13/02/21

        Public Shared _B1DireccionIPSQLServer As String
        Public Shared _B1NombreSQLServer As String
        Public Shared _B1UsuarioSQLServer As String
        Public Shared _B1PasswordSQLServer As String

        Public Shared _BDIDireccionIPSQLServer As String
        Public Shared _BDINombreSQLServer As String
        Public Shared _BDIUsuarioSQLServer As String
        Public Shared _BDIPasswordSQLServer As String

        Public Shared _BMDDireccionIPMongoDB As String
        Public Shared _BMDNombreMongoDB As String
        Public Shared _BMDUsuarioMongoDB As String
        Public Shared _BMDPasswordMongoDB As String

        Public Shared _ControlarBitacoraDesdeLlaveCentral As String
        Public Shared _AplicacionInicial As String
        Public Shared _ActivarLogGeneral As String
        Public Shared _RutaLogGeneral As String
        Public Shared _ActivarLogAdministrativo As String
        Public Shared _RutaLogAdministrativo As String
        Public Shared _ActivarBitacoraBasica As String
        Public Shared _ActivarBitacoraGeneral As String

        Private Shared _instancia As Configuracion = Nothing

#End Region

#Region "Constructores"

        Sub New()

            Inicializar()

        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property Status As TagWatcher

            Get
                Return _status

            End Get

        End Property


#End Region

#Region "Metodos"

        Public Shared Function ObtenerInstancia() As Configuracion

            If _instancia Is Nothing Then

                _instancia = New Configuracion()

            Else

                Return _instancia

            End If

        End Function

        Public Sub Inicializar()

            _status = New TagWatcher

            Try

                _configuracion = New LeerArchivoXML32

                'KcjaSJElOTn2pIXa6qAVufUABuiXSbNb16KfjWjsMEQ=

                With _configuracion

                    .Encriptado = ILeerArchivo.TiposCifrado.RijndaelManaged


                    .Llave = "KcjaSJElOTn2pIXa6qAVufUABuiXSbNb16KfjWjsMEQ="

                    .LeerXML()

                    _modalidadServicio = _configuracion.RegresaValor("_ModalidadServicio")

                    _direccionIPServidorSQLServerGeneralProduccion = _configuracion.RegresaValor("_DireccionIPServidorSQLServerGeneralProduccion")

                    _usuarioSQLServerGeneralProduccion = _configuracion.RegresaValor("_UsuarioSQLServerGeneralProduccion")

                    _claveSQLServerGeneralProduccion = _configuracion.RegresaValor("_ClaveSQLServerGeneralProduccion")

                    _baseDatosSQLServerProduccion = _configuracion.RegresaValor("_BaseDatosSQLServerProduccion")

                    _oficina = _configuracion.RegresaValor("_Oficina")

                    _direccionIPServidorMYSQLGeneralProduccion = _configuracion.RegresaValor("_DireccionIPServidorMYSQLGeneralProduccion")

                    _usuarioMYSQLGeneralProduccion = _configuracion.RegresaValor("_UsuarioMYSQLGeneralProduccion")

                    _claveMYSQLGeneralProduccion = _configuracion.RegresaValor("_ClaveMYSQLGeneralProduccion")

                    _baseDatosMYSQLProduccion = _configuracion.RegresaValor("_BaseDatosMYSQLProduccion")

                    _direccionServidorVFPGeneralProduccion = _configuracion.RegresaValor("_DireccionServidorVFPGeneralProduccion")

                    _usuario = _configuracion.RegresaValor("_Usuario")

                    _contrasena = _configuracion.RegresaValor("_Contrasena")

                    _grupoEmpresarial = _configuracion.RegresaValor("_GrupoEmpresarial")

                    _aplicacion = _configuracion.RegresaValor("_Aplicacion")

                    _endPoint = _configuracion.RegresaValor("_EndPoint")

                    _contrasenaWebService = _configuracion.RegresaValor("_ContrasenaWebService")

                    _licenciaTagCode = _configuracion.RegresaValor("_LicenciaTagCode")


                    _B1DireccionIPSQLServer = _configuracion.RegresaValor("_B1DireccionIPSQLServer")
                    _B1NombreSQLServer = _configuracion.RegresaValor("_B1NombreSQLServer")
                    _B1UsuarioSQLServer = _configuracion.RegresaValor("_B1UsuarioSQLServer")
                    _B1PasswordSQLServer = _configuracion.RegresaValor("_B1PasswordSQLServer")

                    _BDIDireccionIPSQLServer = _configuracion.RegresaValor("_BDIDireccionIPSQLServer")
                    _BDINombreSQLServer = _configuracion.RegresaValor("_BDINombreSQLServer")
                    _BDIUsuarioSQLServer = _configuracion.RegresaValor("_BDIUsuarioSQLServer")
                    _BDIPasswordSQLServer = _configuracion.RegresaValor("_BDIPasswordSQLServer")

                    _BMDDireccionIPMongoDB = _configuracion.RegresaValor("_BMDDireccionIPMongoDB")
                    _BMDNombreMongoDB = _configuracion.RegresaValor("_BMDNombreMongoDB")
                    _BMDUsuarioMongoDB = _configuracion.RegresaValor("_BMDUsuarioMongoDB")
                    _BMDPasswordMongoDB = _configuracion.RegresaValor("_BMDPasswordMongoDB")

                    _ControlarBitacoraDesdeLlaveCentral = _configuracion.RegresaValor("_ControlarBitacoraDesdeLlaveCentral")
                    _AplicacionInicial = _configuracion.RegresaValor("_AplicacionInicial")
                    _ActivarLogGeneral = _configuracion.RegresaValor("_ActivarLogGeneral")
                    _RutaLogGeneral = _configuracion.RegresaValor("_RutaLogGeneral")
                    _ActivarLogAdministrativo = _configuracion.RegresaValor("_ActivarLogAdministrativo")
                    _RutaLogAdministrativo = _configuracion.RegresaValor("_RutaLogAdministrativo")
                    _ActivarBitacoraBasica = _configuracion.RegresaValor("_ActivarBitacoraBasica")
                    _ActivarBitacoraGeneral = _configuracion.RegresaValor("_ActivarBitacoraGeneral")

                    _status.SetOK()

                End With

            Catch ex As Exception

                _status.SetError(TagWatcher.ErrorTypes.C2_000_2004)

            End Try

        End Sub




        Public Shared Function ConstanteGlobal(ByVal token_ As DatosGlobalesSistema) As String
            Dim valor_ As String = Nothing

            'ya tiene singleton, ya no lo necesita MOP

            'Inicializar()

            Select Case token_

                Case DatosGlobalesSistema.LlaveCifrado : Return _configuracion.Llave
                Case DatosGlobalesSistema.Oficina : Return _oficina
                Case DatosGlobalesSistema.Aplicacion : Return _aplicacion

                Case DatosGlobalesSistema.ModalidadServicio : Return _modalidadServicio

                Case DatosGlobalesSistema.DireccionIPServidorSQLServerGeneralProduccion : Return _direccionIPServidorSQLServerGeneralProduccion
                Case DatosGlobalesSistema.UsuarioSQLServerGeneralProduccion : Return _usuarioSQLServerGeneralProduccion
                Case DatosGlobalesSistema.ClaveSQLServerGeneralProduccion : Return _claveSQLServerGeneralProduccion
                Case DatosGlobalesSistema.BaseDatosSQLServerProduccion : Return _baseDatosSQLServerProduccion


                Case DatosGlobalesSistema.BaseDatosMYSQLProduccion : Return _baseDatosMYSQLProduccion
                Case DatosGlobalesSistema.DireccionIPServidorMYSQLGeneralProduccion : Return _direccionIPServidorMYSQLGeneralProduccion
                Case DatosGlobalesSistema.UsuarioMYSQLGeneralProduccion : Return _usuarioMYSQLGeneralProduccion
                Case DatosGlobalesSistema.ClaveMYSQLGeneralProduccion : Return _claveMYSQLGeneralProduccion
                Case DatosGlobalesSistema.BaseDatosMYSQLProduccion : Return _baseDatosMYSQLProduccion
                Case DatosGlobalesSistema.DireccionServidorVFPGeneralProduccion : Return _direccionServidorVFPGeneralProduccion

                Case DatosGlobalesSistema.Usuario : Return _usuario
                Case DatosGlobalesSistema.Contrasena : Return _contrasena

                Case DatosGlobalesSistema.GrupoEmpresarial : Return _grupoEmpresarial
                Case DatosGlobalesSistema.Aplicacion : Return _aplicacion

                Case DatosGlobalesSistema.EndPoint : Return _endPoint
                Case DatosGlobalesSistema.ContrasenaWebService : Return _contrasenaWebService
                Case DatosGlobalesSistema.LicenciaTagCode : Return _licenciaTagCode

                Case DatosGlobalesSistema.B1DireccionIPSQLServer : Return _B1DireccionIPSQLServer
                Case DatosGlobalesSistema.B1NombreSQLServer : Return _B1NombreSQLServer
                Case DatosGlobalesSistema.B1UsuarioSQLServer : Return _B1UsuarioSQLServer
                Case DatosGlobalesSistema.B1PasswordSQLServer : Return _B1PasswordSQLServer

                Case DatosGlobalesSistema.BDIDireccionIPSQLServer : Return _BDIDireccionIPSQLServer
                Case DatosGlobalesSistema.BDINombreSQLServer : Return _BDINombreSQLServer
                Case DatosGlobalesSistema.BDIUsuarioSQLServer : Return _BDIUsuarioSQLServer
                Case DatosGlobalesSistema.BDIPasswordSQLServer : Return _BDIPasswordSQLServer

                Case DatosGlobalesSistema.BMDDireccionIPMongoDB : Return _BMDDireccionIPMongoDB
                Case DatosGlobalesSistema.BMDNombreMongoDB : Return _BMDNombreMongoDB
                Case DatosGlobalesSistema.BMDUsuarioMongoDB : Return _BMDUsuarioMongoDB
                Case DatosGlobalesSistema.BMDPasswordMongoDB : Return _BMDPasswordMongoDB

                Case DatosGlobalesSistema.ControlarBitacoraDesdeLlaveCentral : Return _ControlarBitacoraDesdeLlaveCentral
                Case DatosGlobalesSistema.AplicacionInicial : Return _AplicacionInicial
                Case DatosGlobalesSistema.ActivarLogGeneral : Return _ActivarLogGeneral
                Case DatosGlobalesSistema.RutaLogGeneral : Return _RutaLogGeneral
                Case DatosGlobalesSistema.ActivarLogAdministrativo : Return _ActivarLogAdministrativo
                Case DatosGlobalesSistema.RutaLogAdministrativo : Return _RutaLogAdministrativo
                Case DatosGlobalesSistema.ActivarBitacoraBasica : Return _ActivarBitacoraBasica
                Case DatosGlobalesSistema.ActivarBitacoraGeneral : Return _ActivarBitacoraGeneral

            End Select

            Return valor_

        End Function

#End Region


    End Class

End Namespace
