Imports System.IO
Imports gsol.basededatos
Imports gsol.BaseDatos.Operaciones

Imports Wma.Exceptions
Imports System.Net.NetworkInformation
Imports gsol.monitoreo.SingletonBitacoras64
Imports gsol.seguridad


Namespace gsol.monitoreo

    Public Class BitacoraCapaDatos
        Implements IBitacoras


#Region "Atributos"

        ' Singleton bitacóra
        Private _singletonBitacora As SingletonBitacoras

        'Funcionalidad básica

        Private _disparo As String

        Private _misCredenciales As ICredenciales

        Private _permiso As Int32

        Private _mensaje As String

        Private _parametro As String

        Private _tipoBitacora As IBitacoras.TiposBitacora

        Private _tipoSuceso As IBitacoras.TiposSucesos

        Private _estatusInsercion As IBitacoras.EstatusInsercion

        Private _lineaBaseBitacora As LineaBaseBitacora

        Private _modulo As String = Nothing

        Private _nombreUsuario As String

        'Funcionaliada avanzada

        'i_Cve_BitacoraAvanzadaConsulta PrimaryKey
        Private _i_Cve_Usuario As Int32
        Private _i_Cve_DivisionMiEmpresa As Int32
        Private _i_DiaSemana As Int16 'i_DiaSemana 1,2...7
        Private _i_Mes As Int16 'i_Mes 1,2...12
        Private _i_Anio As Int32 'i_Anio 2018
        Private _f_FechaHoraInicio As DateTime
        Private _f_FechaHoraFinal As DateTime
        Private _i_Cve_Aplicacion As IBitacoras.ClaveTiposAplicacion
        Private _i_Cve_Estatus As IBitacoras.EstadosConsultaAvanzada
        Private _tagWatcher As TagWatcher
        Private _i_TiempoRespuestaTotal As Double
        Private _i_Instrumentacion As IBitacoras.TiposInstrumentacion
        Private _modalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta
        Private _i_MemoriaRAMDisponibleGB As Double
        Private _i_MemoriaRAMTotalGB As Double
        Private _i_Cve_RecursoSolicitante As Int32
        Private _t_IP As String 't_IP
        Private _t_Pais As String 't_Pais
        Private _t_EstadoCiudad As String 't_EstadoCiudad

        Private _t_IDTransaccion As String = Nothing

#End Region

#Region "Constructores"

        Sub New()

            _estatusInsercion = IBitacoras.EstatusInsercion.SinEstatus

            _singletonBitacora = SingletonBitacoras.ObtenerInstancia(4)

        End Sub

        'Funcionalidad básica

        Sub New(ByVal disparo_ As String, _
                ByVal misCredenciales_ As ICredenciales, _
                ByVal permiso_ As Int32, _
                ByVal mensaje_ As String, _
                ByVal parametro_ As String, _
                ByVal tipoBitacora_ As IBitacoras.TiposBitacora, _
                ByVal tipoSuceso_ As IBitacoras.TiposSucesos
                )


            _singletonBitacora = SingletonBitacoras.ObtenerInstancia(4)

            _disparo = disparo_

            _misCredenciales = misCredenciales_

            _permiso = permiso_

            _mensaje = mensaje_

            _parametro = parametro_

            _tipoBitacora = tipoBitacora_

            _tipoSuceso = tipoSuceso_

            _lineaBaseBitacora = New LineaBaseBitacora

            DocumentaBitacoraCapaDatos()

        End Sub

        'Bitacora básica, posible desactivación 13/02/21 MOP
        Sub New(ByVal mensaje_ As String, _
                ByVal tipoBitacora_ As IBitacoras.TiposBitacora, _
                ByVal tipoSuceso_ As IBitacoras.TiposSucesos, _
                ByVal nombreUsuarioCliente_ As String, _
                ByVal moduloCliente_ As String
                )

            _singletonBitacora = SingletonBitacoras.ObtenerInstancia(4)

            _mensaje = mensaje_

            _tipoBitacora = tipoBitacora_

            _tipoSuceso = tipoSuceso_

            _nombreUsuario = nombreUsuarioCliente_

            _modulo = moduloCliente_

            _lineaBaseBitacora = New LineaBaseBitacora

            DocumentaBitacoraArchivoLocal()

        End Sub


        'Funcionalidad avanzada
        'BitacoraCapaDatos
        Sub New(ByVal disparo_ As String, _
                ByVal misCredenciales_ As ICredenciales, _
                ByVal permiso_ As Int32, _
                ByVal mensaje_ As String, _
                ByVal parametro_ As String, _
                ByVal tipoBitacora_ As IBitacoras.TiposBitacora, _
                ByVal tipoSuceso_ As IBitacoras.TiposSucesos, _
 _
                ByVal i_Cve_Usuario_ As Int32, _
                ByVal i_Cve_DivisionMiEmpresa_ As Int32, _
                ByVal i_Cve_Estatus_ As IBitacoras.EstadosConsultaAvanzada, _
                ByVal f_FechaInicio_ As DateTime, _
                ByVal i_Cve_TipoAplicacion_ As IBitacoras.ClaveTiposAplicacion, _
                ByVal i_Instrumentacion_ As IBitacoras.TiposInstrumentacion, _
                ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta, _
                ByVal i_Cve_RecursoSolicitante_ As Int32
                )

            'sdsad()

            Dim claveAplicacion_ As Int32 = 0

            Select Case i_Cve_TipoAplicacion_

                Case IBitacoras.ClaveTiposAplicacion.AplicacionEscritorioKrombase

                    claveAplicacion_ = 4

                Case IBitacoras.ClaveTiposAplicacion.AplicacionMovilTracking

                    claveAplicacion_ = 11

                Case IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                    claveAplicacion_ = 12

                Case IBitacoras.ClaveTiposAplicacion.SinDefinir

                    claveAplicacion_ = 4

                Case Else

                    claveAplicacion_ = 4

            End Select

            _singletonBitacora = SingletonBitacoras.ObtenerInstancia(claveAplicacion_)

            _misCredenciales = misCredenciales_

            _disparo = disparo_ 'DINAMICO

            _permiso = permiso_ 'DINAMICO

            _mensaje = mensaje_ 'DINAMICO

            _parametro = parametro_ 'DINAMICO

            _tipoBitacora = tipoBitacora_ 'DINAMICO

            _tipoSuceso = tipoSuceso_  'DINAMICO


            'Lo nuevo

            _f_FechaHoraInicio = f_FechaInicio_ 'DINAMICO
            _i_DiaSemana = _f_FechaHoraInicio.DayOfWeek 'DINAMICO
            _i_Mes = _f_FechaHoraInicio.Month 'DINAMICO
            _i_Anio = _f_FechaHoraInicio.Year 'DINAMICO
            _i_Cve_Estatus = i_Cve_Estatus_ 'DINAMICO


            _i_Cve_Usuario = i_Cve_Usuario_

            _i_Cve_RecursoSolicitante = i_Cve_RecursoSolicitante_

            _i_Cve_DivisionMiEmpresa = i_Cve_DivisionMiEmpresa_


            _i_Cve_Aplicacion = i_Cve_TipoAplicacion_

            _i_Instrumentacion = i_Instrumentacion_
            _modalidadConsulta = modalidadConsulta_

            Try

                _t_IP = ObtenerNumeroIP()
                _t_Pais = Nothing
                _t_EstadoCiudad = Nothing
                _i_MemoriaRAMDisponibleGB = 0 ' My.Computer.Info.AvailablePhysicalMemory   'DINAMICO
                _i_MemoriaRAMTotalGB = 0 'My.Computer.Info.TotalPhysicalMemory 'DINAMICO

                'TotalVirtualMemory = My.Computer.Info.TotalVirtualMemory

            Catch ex As Exception

                _t_IP = "0.0.0.0"
                _t_Pais = Nothing
                _t_EstadoCiudad = Nothing
                _i_MemoriaRAMDisponibleGB = 0
                _i_MemoriaRAMTotalGB = 0

            End Try

            _f_FechaHoraFinal = Nothing 'DINAMICO
            _tagWatcher = Nothing  'DINAMICO
            _i_TiempoRespuestaTotal = Nothing 'DINAMICO

            'DocumentaBitacoraCapaDatos()

            _lineaBaseBitacora = New LineaBaseBitacora

        End Sub




        Private Function ObtenerNumeroIP() As String

            Dim index_ As Int32 = 0

            For Each nic As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()

                If ((nic.NetworkInterfaceType = NetworkInterfaceType.Ethernet) Or
                    (nic.NetworkInterfaceType = NetworkInterfaceType.Wireless80211)) Then

                    If nic.OperationalStatus = OperationalStatus.Up Then

                        'Dim interface_ As New ManifestInterface

                        'With interface_

                        '.Order = index_

                        '.Id = nic.Id

                        Return GetIpNumberOfDescription(nic.Description)

                        '.Name = nic.Name

                        '.Description = nic.Description

                        '.PhysicalAddress = nic.GetPhysicalAddress.ToString

                        '.OperationalStatus = nic.OperationalStatus.ToString

                        '.Speed = nic.Speed.ToString

                        '.NetworkInterfaceType = nic.NetworkInterfaceType.ToString

                        '_networkInterfaces.Add(interface_)

                        'End With

                    End If

                End If

            Next

        End Function

        Private Function GetIpNumberOfDescription(ByVal descriptionInterface_ As String) As String

            For Each nic As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()

                For Each ip As UnicastIPAddressInformation In nic.GetIPProperties().UnicastAddresses

                    If (nic.Description = descriptionInterface_) Then

                        If (ip.Address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork) Then

                            Return ip.Address.ToString()

                        End If

                    End If

                Next

            Next

            Return Nothing

        End Function


#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        '._t_NombreEnsamblado = _
        Property NombreEnsamblado As String Implements IBitacoras.NombreEnsamblado
            Get
                Return _lineaBaseBitacora._t_NombreEnsamblado
            End Get
            Set(value As String)
                _lineaBaseBitacora._t_NombreEnsamblado = value
            End Set
        End Property

        '._t_NombreVT()
        Property NombreVT As String Implements IBitacoras.NombreVT
            Get
                Return _lineaBaseBitacora._t_NombreVT
            End Get
            Set(value As String)
                _lineaBaseBitacora._t_NombreVT = value
            End Set
        End Property

        '._t_NombreUsuario()
        Property NombreCompletoUsuario As String Implements IBitacoras.NombreCompletoUsuario
            Get
                Return _lineaBaseBitacora._t_NombreUsuario
            End Get
            Set(value As String)
                _lineaBaseBitacora._t_NombreUsuario = value
            End Set
        End Property

        '._t_CuentaUsuario()
        Property CuentaPublicaUsuario As String Implements IBitacoras.CuentaPublicaUsuario
            Get
                Return _lineaBaseBitacora._t_CuentaUsuario
            End Get
            Set(value As String)
                _lineaBaseBitacora._t_CuentaUsuario = value
            End Set
        End Property

        'Funcionalidad básica

        Public Property IDTransaccionAbierta As String Implements IBitacoras.IDTransaccionAbierta
            Get
                Return _t_IDTransaccion
            End Get
            Set(value As String)
                _t_IDTransaccion = value
            End Set
        End Property

        Public Property NombreUsuario As String Implements IBitacoras.NombreUsuario
            Get
                Return _nombreUsuario
            End Get
            Set(value As String)
                _nombreUsuario = value
            End Set
        End Property

        Public Property Modulo As String Implements IBitacoras.Modulo
            Get
                Return _modulo
            End Get
            Set(value As String)
                _modulo = value
            End Set
        End Property

        Public Property MisCredenciales As ICredenciales _
            Implements IBitacoras.MisCredenciales
            Get
                Return _misCredenciales
            End Get
            Set(value As ICredenciales)
                _misCredenciales = value
            End Set
        End Property

        Public Property TipoBitacora As IBitacoras.TiposBitacora _
            Implements IBitacoras.TipoBitacora
            Get
                Return TipoBitacora
            End Get
            Set(value As IBitacoras.TiposBitacora)
                _tipoBitacora = value
            End Set
        End Property

        Public Property TipoSuceso As IBitacoras.TiposSucesos _
            Implements IBitacoras.TipoSuceso
            Get
                Return _tipoSuceso
            End Get
            Set(value As IBitacoras.TiposSucesos)
                _tipoSuceso = value
            End Set
        End Property

        Public Property Disparo As String _
            Implements IBitacoras.Disparo
            Get
                Return _disparo
            End Get
            Set(value As String)
                _disparo = value
            End Set
        End Property

        Public Property Mensaje As String _
            Implements IBitacoras.Mensaje
            Get
                Return _mensaje
            End Get
            Set(value As String)
                _mensaje = value
            End Set
        End Property

        Public Property Parametros As String _
            Implements IBitacoras.Parametros
            Get
                Return _parametro
            End Get
            Set(value As String)
                _parametro = value
            End Set
        End Property

        Public Property Permiso As Integer _
            Implements IBitacoras.Permiso
            Get
                Return _permiso
            End Get
            Set(value As Integer)
                _permiso = value
            End Set
        End Property

        Public ReadOnly Property Estatus As IBitacoras.EstatusInsercion _
            Implements IBitacoras.Estatus
            Get
                Return _estatusInsercion
            End Get
        End Property

        'Funcionalidad Avanzada

        Public Property Anio As Integer Implements IBitacoras.Anio
            Get
                Return _i_Anio
            End Get
            Set(value As Integer)
                _i_Anio = value
            End Set
        End Property

        Public Property ClaveAplicacion As IBitacoras.ClaveTiposAplicacion Implements IBitacoras.ClaveAplicacion
            Get
                Return _i_Cve_Aplicacion
            End Get
            Set(value As IBitacoras.ClaveTiposAplicacion)
                _i_Cve_Aplicacion = value
            End Set
        End Property

        Public Property ClaveDivisionMiEmpresa As Integer Implements IBitacoras.ClaveDivisionMiEmpresa
            Get
                Return _i_Cve_DivisionMiEmpresa
            End Get
            Set(value As Integer)
                _i_Cve_DivisionMiEmpresa = value
            End Set
        End Property

        Public Property ClaveUsuario As Integer Implements IBitacoras.ClaveUsuario
            Get
                Return _i_Cve_Usuario
            End Get
            Set(value As Integer)
                _i_Cve_Usuario = value
            End Set
        End Property

        Public Property DiaSemana As Short Implements IBitacoras.DiaSemana
            Get
                Return _i_DiaSemana
            End Get
            Set(value As Short)
                _i_DiaSemana = value
            End Set
        End Property

        Public Property EstadoCiudad As String Implements IBitacoras.EstadoCiudad
            Get
                Return _t_EstadoCiudad
            End Get
            Set(value As String)
                _t_EstadoCiudad = value
            End Set
        End Property

        Public Property EstadoConsulta As IBitacoras.EstadosConsultaAvanzada Implements IBitacoras.EstadoConsulta
            Get
                Return _i_Cve_Estatus
            End Get
            Set(value As IBitacoras.EstadosConsultaAvanzada)
                _i_Cve_Estatus = value
            End Set
        End Property

        Public Property FechaHoraFinal As Date Implements IBitacoras.FechaHoraFinal
            Get
                Return _f_FechaHoraFinal
            End Get
            Set(value As Date)
                _f_FechaHoraFinal = _f_FechaHoraFinal
            End Set
        End Property

        Public Property FechaHoraInicio As Date Implements IBitacoras.FechaHoraInicio
            Get
                Return _f_FechaHoraInicio
            End Get
            Set(value As Date)
                _f_FechaHoraInicio = value
            End Set
        End Property

        Public Property MemoriaRAMDisponibleGB As Double Implements IBitacoras.MemoriaRAMDisponibleGB
            Get
                Return _i_MemoriaRAMDisponibleGB
            End Get
            Set(value As Double)
                _i_MemoriaRAMDisponibleGB = value
            End Set
        End Property

        Public Property MemoriaRAMTotalGB As Double Implements IBitacoras.MemoriaRAMTotalGB
            Get
                Return _i_MemoriaRAMTotalGB
            End Get
            Set(value As Double)
                _i_MemoriaRAMTotalGB = value
            End Set
        End Property

        Public Property MensajeTagWatcher As Wma.Exceptions.TagWatcher Implements IBitacoras.MensajeTagWatcher
            Get
                Return _tagWatcher
            End Get
            Set(value As Wma.Exceptions.TagWatcher)
                _tagWatcher = value
            End Set
        End Property

        Public Property Mes As Short Implements IBitacoras.Mes
            Get
                Return _i_Mes
            End Get
            Set(value As Short)
                _i_Mes = value
            End Set
        End Property

        Public Property NumeroIP As String Implements IBitacoras.NumeroIP
            Get
                Return _t_IP
            End Get
            Set(value As String)
                _t_IP = value
            End Set
        End Property

        Public Property PaisSolicitud As String Implements IBitacoras.PaisSolicitud
            Get
                Return _t_Pais
            End Get
            Set(value As String)
                _t_Pais = value
            End Set
        End Property

        Public Property RecursoSolicitante As Integer Implements IBitacoras.RecursoSolicitante
            Get
                Return _i_Cve_RecursoSolicitante
            End Get
            Set(value As Integer)
                _i_Cve_RecursoSolicitante = value
            End Set
        End Property

        Public Property TiempoRespuestaTotal As Double Implements IBitacoras.TiempoRespuestaTotal
            Get
                Return _i_TiempoRespuestaTotal
            End Get
            Set(value As Double)
                _i_TiempoRespuestaTotal = value
            End Set
        End Property

        Public Property TipoInstrumentacion As IBitacoras.TiposInstrumentacion Implements IBitacoras.TipoInstrumentacion
            Get
                Return _i_Instrumentacion
            End Get
            Set(value As IBitacoras.TiposInstrumentacion)
                _i_Instrumentacion = value
            End Set
        End Property

        Public Property ModalidadConsulta As BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta Implements IBitacoras.ModalidadConsulta
            Get
                Return _modalidadConsulta
            End Get
            Set(value As BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta)
                _modalidadConsulta = value
            End Set

        End Property
#End Region

#Region "Metodos"

        'Master Of puppets 12/19
        'Bitácora avanzada, se activará y destinará a otra base de datos.
        Public Sub DocumentaBitacoraCapaDatos(ByVal consulta_ As String,
                                              ByVal estatus_ As IBitacoras.EstadosConsultaAvanzada,
                                              ByVal f_Fecha_ As DateTime,
                                              Optional ByVal tiempoTotalRespuesta_ As Double = 0,
                                              Optional ByVal tagWatcherMensaje_ As TagWatcher = Nothing) Implements IBitacoras.DocumentaBitacoraCapaDatos

            '_lineaBaseBitacora = New LineaBaseBitacora

            If _singletonBitacora.ActivarBitacoraAvanzada = False Then

                Exit Sub

            End If

            Select Case estatus_

                Case IBitacoras.EstadosConsultaAvanzada.EnProceso

                    With _lineaBaseBitacora

                        If Not _misCredenciales Is Nothing Then

                            .Division = _misCredenciales.DivisionEmpresaria

                            .Aplicacion = _misCredenciales.Aplicacion

                            .Usuario = _misCredenciales.CredencialUsuario

                            .Grupo = _misCredenciales.GrupoEmpresarial

                            'Else

                            '    .Division = 0

                            '    .Aplicacion = 0

                            '    .Usuario = 0

                            '    .Grupo = 0

                        End If

                        .Disparo = consulta_

                        _disparo = consulta_

                        .TipoSuceso = _tipoSuceso

                        .Permiso = _permiso

                        .Mensaje = _mensaje

                        .Parametros = _parametro

                        .TipoBitacora = _tipoBitacora

                        _estatusInsercion = .EstatusInsercion

                        'Lo nuevo

                        'CORREGIR bautismen

                        _f_FechaHoraInicio = f_Fecha_

                        ._f_FechaHoraInicio = _f_FechaHoraInicio '= f_FechaInicio_ 'DINAMICO

                        ._i_DiaSemana = _f_FechaHoraInicio.DayOfWeek 'DINAMICO
                        ._i_Mes = _f_FechaHoraInicio.Month 'DINAMICO
                        ._i_Anio = _f_FechaHoraInicio.Year 'DINAMICO
                        ._i_Cve_Usuario = _i_Cve_Usuario 'DINAMICO

                        ._i_Cve_RecursoSolicitante = _i_Cve_RecursoSolicitante

                        ._i_Cve_DivisionMiEmpresa = _i_Cve_DivisionMiEmpresa

                        ._i_Cve_Aplicacion = _i_Cve_Aplicacion

                        ._i_Instrumentacion = _i_Instrumentacion

                        ._modalidadConsulta = _modalidadConsulta

                        '._t_NombreEnsamblado = _
                        '._t_NombreVT()
                        '._t_NombreUsuario()
                        '._t_CuentaUsuario()


                        Try

                            ._t_IP = _t_IP
                            ._t_Pais = _t_Pais
                            ._t_EstadoCiudad = _t_EstadoCiudad
                            ._i_MemoriaRAMDisponibleGB = _i_MemoriaRAMDisponibleGB
                            ._i_MemoriaRAMTotalGB = _i_MemoriaRAMTotalGB 'DINAMICO

                        Catch ex As Exception

                            ._t_IP = "0.0.0.0"
                            ._t_Pais = Nothing
                            ._t_EstadoCiudad = Nothing
                            ._i_MemoriaRAMDisponibleGB = 0
                            ._i_MemoriaRAMTotalGB = 0 'DINAMICO

                        End Try

                        _f_FechaHoraFinal = Nothing 'DINAMICO
                        _tagWatcher = Nothing  'DINAMICO
                        _i_TiempoRespuestaTotal = Nothing 'DINAMICO

                        If _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton Then
                            .InsertaBitacoraAvanzada(consulta_, False)
                        Else
                            .InsertaBitacoraAvanzada(consulta_, True)
                        End If

                        'Retorna transacción
                        _t_IDTransaccion = ._idTransaccion

                    End With

                Case IBitacoras.EstadosConsultaAvanzada.Concluida

                    With _lineaBaseBitacora

                        'CORREGIR bautismen

                        If Not IDTransaccionAbierta Is Nothing Then 'obtiene el ID de la última operación

                            _lineaBaseBitacora._idTransaccion = IDTransaccionAbierta

                            'Lo nuevo

                            ._f_FechaHoraFinal = f_Fecha_

                            ._i_TiempoRespuestaTotal = tiempoTotalRespuesta_

                            ._tagWatcherMensaje = tagWatcherMensaje_

                            ._i_Cve_RecursoSolicitante = _i_Cve_RecursoSolicitante

                            ._i_Cve_DivisionMiEmpresa = _i_Cve_DivisionMiEmpresa

                            ._i_Cve_Aplicacion = _i_Cve_Aplicacion

                            ._i_Instrumentacion = _i_Instrumentacion

                            ._modalidadConsulta = _modalidadConsulta

                            Try

                                ._i_MemoriaRAMDisponibleGB = _i_MemoriaRAMDisponibleGB
                                ._i_MemoriaRAMTotalGB = _i_MemoriaRAMTotalGB 'DINAMICO

                            Catch ex As Exception

                                ._i_MemoriaRAMDisponibleGB = 0
                                ._i_MemoriaRAMTotalGB = 0 'DINAMICO

                            End Try

                            _f_FechaHoraFinal = f_Fecha_ 'DINAMICO
                            _tagWatcher = tagWatcherMensaje_  'DINAMICO
                            _i_TiempoRespuestaTotal = tiempoTotalRespuesta_ 'DINAMICO

                            '.ActualizaBitacoraAvanzada(_lineaBaseBitacora._idTransaccion)
                            '.ActualizaBitacoraAvanzada(IDTransaccionAbierta)

                            If _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton Then
                                .ActualizaBitacoraAvanzada(consulta_, False)
                            Else
                                .ActualizaBitacoraAvanzada(consulta_, True)
                            End If

                        Else

                            'No se encontró transacción para cerrar

                        End If

                    End With

                Case IBitacoras.EstadosConsultaAvanzada.AbortadaFallo

            End Select

        End Sub

        'Bitácora tradicional, controlada por archivo de cifrado,  se propone desactivarla 13/02/2021 MOP, para consumir únicamente la más detallada
        Public Sub DocumentaBitacoraCapaDatos()

            If _singletonBitacora.ActivarBitacoraSimple = False Then

                Exit Sub

            End If

            _lineaBaseBitacora = New LineaBaseBitacora

            With _lineaBaseBitacora

                If Not _misCredenciales Is Nothing Then

                    .Usuario = _misCredenciales.CredencialUsuario

                    .Grupo = _misCredenciales.GrupoEmpresarial

                    .Division = _misCredenciales.DivisionEmpresaria

                    .Aplicacion = _misCredenciales.Aplicacion

                    .TipoSuceso = _tipoSuceso

                    .Permiso = _permiso

                    .Mensaje = _mensaje

                    .Parametros = _parametro

                    .TipoBitacora = _tipoBitacora

                    .InsertaBitacora()

                    _estatusInsercion = .EstatusInsercion

                Else

                    .Usuario = Nothing

                    .Grupo = Nothing

                    .Division = Nothing

                    .Aplicacion = Nothing

                End If


            End With

        End Sub

        'Bitácora tradicional, controlada por archivo de cifrado,  se propone desactivarla 13/02/2021 MOP, para consumir únicamente la más detallada
        Public Sub DocumentaBitacoraArchivoLocal()

            If _singletonBitacora.ActivarLogGeneral = False Then
                Exit Sub
            End If

            Dim _numerounicoguid As String

            Dim escritor_ As StreamWriter

            Dim _rutabitacora As String = Nothing

            '*********************** Consultamos la ruta para los archivos LOG de la bitacora **************

            _rutabitacora = _singletonBitacora.RutaLocalLog

            '***********************************************************************************************
            Try

                If _singletonBitacora.ActivarLogGeneral Then

                    If Not _misCredenciales Is Nothing Then
                        _nombreUsuario = _misCredenciales.NombreAutenticacion
                    End If

                    'Número único
                    _numerounicoguid = System.Guid.NewGuid.ToString()

                    Dim FilePath As String = _rutabitacora & _
                                            "\" & _
                                            _nombreUsuario & "-" & _
                                            _modulo & "-" & _
                                            _tipoBitacora.ToString & _
                                            " [" & _numerounicoguid & "]" & _
                                            Now.ToString.Replace(":", "").Replace(".", "").Replace("/", "-") & ".log"

                    'Crea el directorio si no existe 
                    If Not Directory.Exists(_rutabitacora) Then
                        Directory.CreateDirectory(_rutabitacora)
                    End If

                    escritor_ = File.AppendText(FilePath)
                    escritor_.WriteLine("{" & _tipoBitacora.ToString & "} ")
                    escritor_.WriteLine("Suceso: " & _tipoSuceso.ToString)
                    'escritor_.WriteLine("Usuario: " & _misCredenciales.CredencialUsuario.ToString)
                    escritor_.WriteLine("Mensaje:" & _mensaje)
                    'escritor_.WriteLine("Parametros:  " & _parametro)
                    'escritor_.WriteLine("Aplicacion: " & _misCredenciales.Aplicacion)
                    'escritor_.WriteLine("Grupo Empresarial: " & _misCredenciales.GrupoEmpresarial)
                    'escritor_.WriteLine("Division Empresarial : " & _misCredenciales.DivisionEmpresaria)
                    escritor_.Flush()
                    escritor_.Close()

                End If

            Catch excepcion__ As Exception

                '._. si la bitacora tiene error, no hay mas en que creer!

            End Try

        End Sub

#End Region



    End Class

End Namespace

