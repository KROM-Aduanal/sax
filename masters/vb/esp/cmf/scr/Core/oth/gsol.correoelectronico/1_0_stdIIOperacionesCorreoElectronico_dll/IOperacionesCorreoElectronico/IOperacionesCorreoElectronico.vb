Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.documento

Namespace gsol.correoelectronico

    Public Interface IOperacionesCorreoElectronico

#Region "Enum"

        Enum Modalidades
            Individual = 1
            Masivo = 2
        End Enum

        Enum Prioridades
            Alta = 2
            Normal = 1
            Baja = 0
        End Enum

        Enum CantidadIntentosEnvio
            Maxima = 5
            Normal = 3
            Baja = 1
        End Enum

        Enum PaqueteDocumentos
            Administrativo = 1
            Operativo = 2
        End Enum

        Enum EstatusMensajeCorreoElectronicoSalida
            NoIdentificado = 0
            Enviado = 1
            NoEnviado = 2
        End Enum

        Enum EstatusMensajeBandejaEntrada
            NoIdentificado = 0
            Pendiente = 1
            Leido = 2
            Eliminado = 3
        End Enum

        Enum TiposMensajes
            NoIdentificado = 0
            Entrada = 1
            Salida = 2
        End Enum

#End Region

#Region "Propiedades"

        Property MensajeCorreoElectronico As MensajeCorreoElectronico

        ReadOnly Property Estatus As TagWatcher

        Property Operaciones As IOperacionesCatalogo

#End Region

#Region "Metodos"

        Sub EnviarCorreo(ByVal ioperacionescatalogo_ As IOperacionesCatalogo,
                         ByVal mensajeCorreoElectronico_ As MensajeCorreoElectronico,
                         Optional prioridad_ As IOperacionesCorreoElectronico.Prioridades = Prioridades.Normal,
                         Optional cantidadIntentosEnvio_ As IOperacionesCorreoElectronico.CantidadIntentosEnvio = CantidadIntentosEnvio.Normal,
                         Optional modalidad_ As IOperacionesCorreoElectronico.Modalidades = Modalidades.Individual)

        Sub EnviarCorreo(ByVal mensajeCorreoElectronico_ As MensajeCorreoElectronico,
                         Optional prioridad_ As IOperacionesCorreoElectronico.Prioridades = Prioridades.Normal,
                         Optional cantidadIntentosEnvio_ As IOperacionesCorreoElectronico.CantidadIntentosEnvio = CantidadIntentosEnvio.Normal,
                         Optional modalidad_ As IOperacionesCorreoElectronico.Modalidades = Modalidades.Individual)

        Sub LeerBuzonCorreo(ByVal ioperacionescatalogo_ As IOperacionesCatalogo,
                            ByVal cuentaCorreo_ As CuentaCorreo)

        Sub LeerBuzonCorreo(ByVal cuentaCorreo_ As CuentaCorreo)

#End Region

    End Interface

    Public Class MensajeCorreoElectronico

#Region "Atributos"

        Private _clave As Integer

        Private _claveBandeja As Integer

        Private _subject As String

        Private _senderFrom As String

        Private _senderFromDomain As String

        Private _too As List(Of String)

        Private _cc As List(Of String)

        Private _bcc As List(Of String)

        Private _body As String

        Private _fechaEnvioRecibido As DateTime

        Private _estatusMensajeSalida As IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida

        Private _estatusBandejaEntrada As IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada

        Private _cantidadIntentos As Integer

        Private _adjuntos As List(Of documento.Documento)

        Private _estado As Integer

        Private _estatus As Integer

        Private _claveDivisionMiEmpresa As Integer

        Private _tipoMensaje As IOperacionesCorreoElectronico.TiposMensajes

        Private _claveCorreoElectronicoRelacionado As Integer

        Private _esAttachment As Boolean

        Private _correoRecepcionUID As String

#End Region

#Region "Constructores"

        Sub New()

            _clave = 0

            _claveBandeja = 0

            _subject = Nothing

            _senderFrom = Nothing

            _senderFromDomain = Nothing

            _too = New List(Of String)

            _cc = New List(Of String)

            _bcc = New List(Of String)

            _body = Nothing

            _fechaEnvioRecibido = Nothing

            _estatusMensajeSalida = IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida.NoIdentificado

            _estatusBandejaEntrada = IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.NoIdentificado

            _cantidadIntentos = 0

            _adjuntos = New List(Of documento.Documento)

            _estado = 1

            _estatus = 1

            _claveDivisionMiEmpresa = 0

            _tipoMensaje = IOperacionesCorreoElectronico.TiposMensajes.NoIdentificado

            _claveCorreoElectronicoRelacionado = 0

            _esAttachment = False

            _correoRecepcionUID = Nothing

        End Sub

#End Region

#Region "Propiedades"

        Public Property Clave As Integer

            Get

                Return _clave

            End Get

            Set(value As Integer)

                _clave = value

            End Set

        End Property

        Public Property ClaveBandeja As Integer

            Get

                Return _claveBandeja

            End Get

            Set(value As Integer)

                _claveBandeja = value

            End Set

        End Property

        Public Property Subject As String

            Get

                Return _subject

            End Get

            Set(value As String)

                _subject = value

            End Set

        End Property

        Public Property SenderFrom As String

            Get

                Return _senderFrom

            End Get

            Set(value As String)

                _senderFrom = value

            End Set

        End Property

        Public Property SenderFromDomain As String

            Get

                Return _senderFromDomain

            End Get

            Set(value As String)

                _senderFromDomain = value

            End Set

        End Property

        Public Property Too As List(Of String)

            Get

                Return _too

            End Get

            Set(value As List(Of String))

                _too = value

            End Set

        End Property

        Public Property CC As List(Of String)

            Get

                Return _cc

            End Get

            Set(value As List(Of String))

                _cc = value

            End Set

        End Property

        Public Property BCC As List(Of String)

            Get

                Return _bcc

            End Get

            Set(value As List(Of String))

                _bcc = value

            End Set

        End Property

        Public Property Body As String

            Get

                Return _body

            End Get

            Set(value As String)

                _body = value

            End Set

        End Property

        Public Property FechaEnvioRecibido As DateTime

            Get

                Return _fechaEnvioRecibido

            End Get

            Set(value As DateTime)

                _fechaEnvioRecibido = value

            End Set

        End Property

        Public Property CantidadIntentos As Integer

            Get

                Return _cantidadIntentos

            End Get

            Set(value As Integer)

                _cantidadIntentos = value

            End Set

        End Property

        Public Property EstatusMensajeSalida As IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida

            Get

                Return _estatusMensajeSalida

            End Get

            Set(value As IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida)

                _estatusMensajeSalida = value

            End Set

        End Property

        Public Property EstatusBandejaEntrada As IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada

            Get

                Return _estatusBandejaEntrada

            End Get

            Set(value As IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada)

                _estatusBandejaEntrada = value

            End Set

        End Property

        Public Property Adjuntos As List(Of documento.Documento)

            Get

                Return _adjuntos

            End Get

            Set(value As List(Of documento.Documento))

                _adjuntos = value

            End Set

        End Property

        Public Property Estado As Integer

            Get

                Return _estado

            End Get

            Set(value As Integer)

                _estado = value

            End Set

        End Property

        Public Property Estatus As Integer

            Get

                Return _estatus

            End Get

            Set(value As Integer)

                _estatus = value

            End Set

        End Property

        Public Property ClaveDivisionMiEmpresa As Integer

            Get

                Return _claveDivisionMiEmpresa

            End Get

            Set(value As Integer)

                _claveDivisionMiEmpresa = value

            End Set

        End Property

        Public Property TipoMensaje As IOperacionesCorreoElectronico.TiposMensajes

            Get

                Return _tipoMensaje

            End Get

            Set(value As IOperacionesCorreoElectronico.TiposMensajes)

                _tipoMensaje = value

            End Set

        End Property

        Public Property ClaveCorreoElectronicoRelacionado As Integer

            Get

                Return _claveCorreoElectronicoRelacionado

            End Get

            Set(value As Integer)

                _claveCorreoElectronicoRelacionado = value

            End Set

        End Property

        Public Property EsAttachment As Boolean

            Get

                Return _esAttachment

            End Get

            Set(value As Boolean)

                _esAttachment = value

            End Set

        End Property

        Public Property CorreoRecepcionUID As String

            Get

                Return _correoRecepcionUID

            End Get

            Set(value As String)

                _correoRecepcionUID = value

            End Set

        End Property

#End Region

    End Class

    Public Class CuentaCorreo

#Region "Atributos"

        Private _claveCuentaCorreo As Integer

        Private _correo As String

        Private _contrasenia As String

        Private _nombreServidor As String

        Private _puerto As String

        Private _estado As Integer

        Private _estatus As Integer

        Private _claveDivisionMiEmpresa As Integer

        Private _permiteMarcarCorreosComoLeidos As Boolean

#End Region

#Region "Enum"

        Enum MetodosCifrado
            TLS
            STARTTLS
        End Enum

        Enum Tipos
            Enviar
            Recibir
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            _claveCuentaCorreo = 0

            _correo = Nothing

            _contrasenia = Nothing

            _nombreServidor = Nothing

            _puerto = Nothing

            _estado = 1

            _estatus = 1

            _claveDivisionMiEmpresa = 0

            _permiteMarcarCorreosComoLeidos = False

        End Sub

#End Region

#Region "Propiedades"

        Public Property ClaveCuentaCorreo As Integer

            Get

                Return _claveCuentaCorreo

            End Get

            Set(value As Integer)

                _claveCuentaCorreo = value

            End Set

        End Property

        Public Property Correo As String

            Get

                Return _correo

            End Get

            Set(value As String)

                _correo = value

            End Set

        End Property

        Public Property Contrasenia As String

            Get

                Return _contrasenia

            End Get

            Set(value As String)

                _contrasenia = value

            End Set

        End Property

        Public Property NombreServidor As String

            Get

                Return _nombreServidor

            End Get

            Set(value As String)

                _nombreServidor = value

            End Set

        End Property

        Public Property Puerto As String

            Get

                Return _puerto

            End Get

            Set(value As String)

                _puerto = value

            End Set

        End Property

        Public Property Estado As Integer

            Get

                Return _estado

            End Get

            Set(value As Integer)

                _estado = value

            End Set

        End Property

        Public Property Estatus As Integer

            Get

                Return _estatus

            End Get

            Set(value As Integer)

                _estatus = value

            End Set

        End Property

        Public Property ClaveDivisionMiEmpresa As Integer

            Get

                Return _claveDivisionMiEmpresa

            End Get

            Set(value As Integer)

                _claveDivisionMiEmpresa = value

            End Set

        End Property

        Public Property PermiteMarcarCorreosComoLeidos As Boolean

            Get

                Return _permiteMarcarCorreosComoLeidos

            End Get

            Set(value As Boolean)

                _permiteMarcarCorreosComoLeidos = value

            End Set

        End Property


#End Region

    End Class

    Public Class BitacoraLecturaBuzonEntrada

#Region "Atributos"

        Private _claveBitacoraLecturaBuzonEntrada As Integer

        Private _claveBuzonEntrada As Integer

        Private _fechaInicioActualizacion As DateTime

        Private _fechaFinActualizacion As DateTime

        Private _estado As Integer

        Private _estatus As Integer

        Private _claveDivisionMiEmpresa As Integer

#End Region

#Region "Constructores"

        Sub New()

            _claveBitacoraLecturaBuzonEntrada = 0

            _claveBuzonEntrada = 0

            _fechaInicioActualizacion = Nothing

            _fechaFinActualizacion = Nothing

            _estado = 1

            _estatus = 1

            _claveDivisionMiEmpresa = 0

        End Sub

#End Region

#Region "Propiedades"

        Public Property ClaveBitacoraLecturaBuzonEntrada As Integer

            Get

                Return _claveBitacoraLecturaBuzonEntrada

            End Get

            Set(value As Integer)

                _claveBitacoraLecturaBuzonEntrada = value

            End Set

        End Property

        Public Property ClaveBuzonEntrada As Integer

            Get

                Return _claveBuzonEntrada

            End Get

            Set(value As Integer)

                _claveBuzonEntrada = value

            End Set

        End Property

        Public Property FechaInicioActualizacion As DateTime

            Get

                Return _fechaInicioActualizacion

            End Get

            Set(value As DateTime)

                _fechaInicioActualizacion = value

            End Set

        End Property

        Public Property FechaFinActualizacion As DateTime

            Get

                Return _fechaFinActualizacion

            End Get

            Set(value As DateTime)

                _fechaFinActualizacion = value

            End Set

        End Property

        Public Property Estado As Integer

            Get

                Return _estado

            End Get

            Set(value As Integer)

                _estado = value

            End Set

        End Property

        Public Property Estatus As Integer

            Get

                Return _estatus

            End Get

            Set(value As Integer)

                _estatus = value

            End Set

        End Property

        Public Property ClaveDivisionMiEmpresa As Integer

            Get

                Return _claveDivisionMiEmpresa

            End Get

            Set(value As Integer)

                _claveDivisionMiEmpresa = value

            End Set

        End Property

#End Region

    End Class

End Namespace
