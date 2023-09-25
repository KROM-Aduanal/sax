Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports Gsol.Componentes.SistemaBase.GsDialogo

Namespace Gsol.BaseDatos

    Public Class SysExpert64_

#Region "Atributos"

        Private _index As Integer

        Private _ioperacionescatalogo As OperacionesCatalogo

        Private _sistema As Organismo

        Private _tablaSysexpert As String

        Private _Revalidaciones As List(Of RevalidacionBl)

        Private _RevalidacionesBlContenedor As List(Of RevalidacionBLContenedor)

        Private _Buques As List(Of Buques)

        Private _Buque As Buques

        Private _Puerto As Puertos

        Private _Naviera As Navieras

        Private _Reexpedidora As Reexpedidoras

        Private _Trafico As Trafico

        Private _auxTagWatcher As TagWatcher

        Private _sql As String

        Private _insercionTrafico As Boolean

#End Region

#Region "Propiedades"

        Property insercionTrafico As String

            Get

                Return _insercionTrafico

            End Get

            Set(value As String)

                _insercionTrafico = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New(ByVal index_ As Integer,
                ByVal ioperacionescatalogo_ As OperacionesCatalogo)

            _ioperacionescatalogo = ioperacionescatalogo_

            _index = index_

            _insercionTrafico = False

            _sistema = New Organismo

            _auxTagWatcher = New TagWatcher

        End Sub

#End Region

#Region "Metodos"

        Sub Revalidacion()

            _auxTagWatcher.Status = TagWatcher.TypeStatus.Ok

            'Este método contienen todo para insertar/actualizar lo referente a revalidación

            _auxTagWatcher = inciarProcesoRevalidacion()

            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

            Else

                'Este método contienen todo para insertar insertar/actualizar a revalidación por contenedor
                _auxTagWatcher = iniciarProcesoRevalidacionBlContenedor()

                If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                Else

                    'Este método contienen todo para insertar/actualizar lo referente a la situación de tráfico de revalidación
                    _auxTagWatcher = iniciarProcesoTrafico()

                    If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                        _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                    Else

                        _sistema.GsDialogo("¡Información sincronizada con SysExpert!", TipoDialogo.Aviso)

                    End If

                End If

            End If

        End Sub

        Private Function inciarProcesoRevalidacion() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            _Revalidaciones = New List(Of RevalidacionBl)

            'Obtenemos la información de KromBase para revalidación
            TagLocal_ = ObtenerInformacionRevalidacionKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                'Verificamos el catálogo de puertos de sysexpert contra el de KromBase
                TagLocal_ = VerificarCatalogoPuertosRevalidacionSysexpert()

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    'Verificamos el catálogo de navieras de sysExpert contras el de KromBase
                    TagLocal_ = VerificarCatalogoNavierasRevalidacionSysexpert()

                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        'Verificamos el catálogo de reexpedidoras contra el de krombase
                        TagLocal_ = VerificarCatalogoReexpedidorasRevalidacionSysexpert()

                        If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                            'Verificamos el catálogo de buques contra el de krombase
                            TagLocal_ = VerificarCatalogoBuqueRevalidacionSysexpert()

                            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                                'Verificamos la existencia de un registro en RebalidacionBL de SysExpert
                                TagLocal_ = VerificarRevalidacionSysExpert()

                                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                                    'Se procederá con el tipo de operación por cada una de las referencias
                                    TagLocal_ = RealizarOperacionRevalidacionSysExpert()

                                    Return TagLocal_

                                Else

                                    Return TagLocal_

                                End If

                            Else

                                Return TagLocal_

                            End If

                        Else

                            Return TagLocal_

                        End If

                    Else

                        Return TagLocal_

                    End If

                Else

                    Return TagLocal_

                End If

            Else

                Return TagLocal_

            End If

        End Function

        Private Function ObtenerInformacionRevalidacionKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Obtenemos la información de KromBase para revalidación y la guardamos en una lista.

            _sql = "select voaa.i_Cve_ReferenciaExterna, " &
                              "vi.i_Cve_Transbordo, " &
                              "vi.f_ETA, " &
                              "vi.i_Cve_Reexpedidora, " &
                              "vi.RFC_Reexpedidora, " &
                              "vi.Domicilio_Reexpedidora, " &
                              "vi.i_Cve_Naviera, " &
                              "vi.i_Cve_Nave, " &
                              "vi.i_Cve_Naviera, " &
                              "vi.i_Cve_Origen, " &
                              "p.t_Cve_Puerto, " &
                              "p.t_Nombre NombrePuerto, " &
                              "vi.f_SalidaOrigen, " &
                              "vi.f_Atraque, " &
                              "rci.t_Cve_TipoCarga, " &
                              "NULL idCargaClase, " &
                              "vi.f_Revalidacion, " &
                              "vi.Reexpedidora, " &
                              "vi.Naviera, " &
                              "vi.Nave, " &
                              "vi.t_Observaciones, " &
                              "rci.i_Cve_ReferenciasContenedoresImpo, " &
                              "case when voaa.i_AduanaSeccion = 430 and voaa.i_Patente = 3210 then 1 " &
                              "when voaa.i_AduanaSeccion = 160 and voaa.i_Patente = 3210 then 2  " &
                              "when voaa.i_AduanaSeccion = 430 and voaa.i_Patente = 3921 then 3 " &
                              "when voaa.i_AduanaSeccion = 430 and voaa.i_Patente = 3945 then 4 " &
                              "when voaa.i_AduanaSeccion = 430 and voaa.i_Patente = 3931 then 5 " &
                              "when voaa.i_AduanaSeccion = 432 and voaa.i_Patente = 3921 then 6 " &
                              "when voaa.i_AduanaSeccion = 432 and voaa.i_Patente = 3945 then 7 " &
                              "when voaa.i_AduanaSeccion = 650 and voaa.i_Patente = 3931 then 8 " &
                              "when voaa.i_AduanaSeccion = 510 and voaa.i_Patente = 3921 then 12 " &
                              "when voaa.i_AduanaSeccion = 510 and voaa.i_Patente = 3931 then 13 " &
                              "when voaa.i_AduanaSeccion = 160 and voaa.i_Patente = 3945 then 14 " &
                              "when voaa.i_AduanaSeccion = 160 and voaa.i_Patente = 3921 then 15 " &
                              "when voaa.i_AduanaSeccion = 470 and voaa.i_Patente = 3921 then 16 " &
                              "when voaa.i_AduanaSeccion = 470 and voaa.i_Patente = 3945 then 17 " &
                              "when voaa.i_AduanaSeccion = 200 and voaa.i_Patente = 3945 then 18 " &
                              "when voaa.i_AduanaSeccion = 810 and voaa.i_Patente = 3210 then 19 " &
                              "when voaa.i_AduanaSeccion = 810 and voaa.i_Patente = 3931 then 20 " &
                              "when voaa.i_AduanaSeccion = 470 and voaa.i_Patente = 3210 then 21 " &
                              "when voaa.i_AduanaSeccion = 421 and voaa.i_Patente = 3945 then 26 " &
                              "when voaa.i_AduanaSeccion = 160 and voaa.i_Patente = 3931 then 31 " &
                              "when voaa.i_AduanaSeccion = 650 and voaa.i_Patente = 3210 then 36 " &
                               "End idAgencia " &
                       "from VT003ViajesImpo as vi " &
                       "inner join VT003ViajesImpoReferencias as vir on vir.i_Cve_Viaje = vi.i_Cve_Viaje and vir.i_Cve_DivisionMiEmpresa = vi.i_Cve_DivisionMiEmpresa " &
                       "inner join VT003ReferenciasContenedoresImpo as rci on rci.i_Cve_ReferenciasContenedoresImpo = vir.i_Cve_ReferenciasContenedoresImpo and rci.i_Cve_DivisionMiEmpresa = vir.i_Cve_DivisionMiEmpresa " &
                       "inner join VT003OperacionesAgenciasAduanales as voaa on voaa.i_Cve_VinOperacionesAgenciasAduanales = rci.i_Cve_VinOperacionesAgenciasAduanales and voaa.i_Cve_DivisionMiEmpresa	= rci.i_Cve_DivisionMiEmpresa " &
                       "left join Vt003Puertos as p on p.i_cve_Puerto = vi.i_cve_Origen " &
                       "where vi.i_Cve_Viaje = " & _index

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                            For contador_ = 0 To sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count - 1

                                With _Revalidaciones

                                    Dim Revalidacion_ = New RevalidacionBl(_ioperacionescatalogo)

                                    Revalidacion_.IdReferencia = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_ReferenciaExterna"))

                                    Revalidacion_.Trasbordo = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_Transbordo"))

                                    Revalidacion_.FechaETA = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("f_ETA"))

                                    Revalidacion_.IdReexpedidora = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_Reexpedidora"))

                                    Revalidacion_.IdPuerto = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_Origen"))

                                    Revalidacion_.IdAgencia = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("idAgencia"))

                                    Revalidacion_.FechaEmbarque = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("f_SalidaOrigen"))

                                    Revalidacion_.FechaETALAX = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("f_Atraque"))

                                    Revalidacion_.ClaveCargaTipo = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("t_Cve_TipoCarga"))

                                    Revalidacion_.IdCargaClase = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("idCargaClase"))

                                    Revalidacion_.FechaRevalidacion = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("f_Revalidacion"))

                                    Revalidacion_.Observaciones = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("t_Observaciones"))

                                    Revalidacion_.IdBuque = sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_Nave")

                                    Revalidacion_.IdNaviera = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_Naviera"))

                                    Revalidacion_.ReexpedidoraKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("Reexpedidora"))

                                    Revalidacion_.RfcReexpedidoraKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("RFC_Reexpedidora"))

                                    Revalidacion_.DomicilioReexpedidoraKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("Domicilio_Reexpedidora"))

                                    Revalidacion_.NaveKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("Nave"))

                                    Revalidacion_.NavieraKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("Naviera"))

                                    Revalidacion_.ClavePuertoKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("t_Cve_Puerto"))

                                    Revalidacion_.NombrePuertoKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("NombrePuerto"))

                                    Revalidacion_.i_Cve_RevalidacionImpoKB = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador_)("i_Cve_ReferenciasContenedoresImpo"))

                                    .Add(Revalidacion_)

                                End With

                            Next contador_

                        End If

                    End If

                End If

            End If

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarCatalogoPuertosRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Puerto = New Puertos(_ioperacionescatalogo)

            'En caso de que no exista el Puerto mandará un error y se acabará la transacción, solo debería aparecer un registro por todos los viajes

            _sql = "Select IdPuerto from [SysExpert].[dbo].[Puertos] where Puerto = '" & _Revalidaciones(0).NombrePuertoKB & "' and Clave = '" & _Revalidaciones(0).ClavePuertoKB & "'"

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                            _Puerto.IdPuerto = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

                        Else

                            TagLocal_.Status = TagWatcher.TypeStatus.Errors

                            TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                            Return TagLocal_


                        End If

                    Else

                        TagLocal_.Status = TagWatcher.TypeStatus.Errors

                        TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                        Return TagLocal_

                    End If

                Else

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                    Return TagLocal_

                End If

            Else

                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                Return TagLocal_

            End If

            If Not _Puerto.IdPuerto Is Nothing Then

                For contador_ = 0 To _Revalidaciones.Count - 1

                    _Revalidaciones(contador_).IdPuerto = _Puerto.IdPuerto

                Next contador_
            End If

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarCatalogoNavierasRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Naviera = New Navieras(_ioperacionescatalogo)

            'En caso de que no exista la Naviera mandará un error y se acabará la transacción, solo debería aparecer un registro por todos los viajes

            _sql = "Select IdNaviera from [SysExpert].[dbo].[Navieras] where Naviera = '" & _Revalidaciones(0).NavieraKB & "'"

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                            _Naviera.IdNaviera = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

                        Else

                            TagLocal_.Status = TagWatcher.TypeStatus.Errors

                            TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9002)

                            Return TagLocal_


                        End If

                    Else

                        TagLocal_.Status = TagWatcher.TypeStatus.Errors

                        TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9002)

                        Return TagLocal_

                    End If

                Else

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9002)

                    Return TagLocal_

                End If

            Else

                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9002)

                Return TagLocal_

            End If

            If Not _Naviera.IdNaviera Is Nothing Then

                For contador_ = 0 To _Revalidaciones.Count - 1

                    _Revalidaciones(contador_).IdNaviera = _Naviera.IdNaviera

                Next contador_

            End If

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarCatalogoReexpedidorasRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Reexpedidora = New Reexpedidoras(_ioperacionescatalogo)

            'En caso de que no exista la reexpedidora se insertará en SysExpert, solo debería aparecer un registro por todos los viajes

            _sql = "Select IdReexpedidora from [SysExpert].[dbo].[Reexpedidoras] where NombreReexpedidora = '" & _Revalidaciones(0).ReexpedidoraKB & "'"

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                            _Revalidaciones(0).IdReexpedidora = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

                            _Reexpedidora.PuedeInsertar = False
                        Else

                            _Reexpedidora.NombreReexpedidora = _Revalidaciones(0).ReexpedidoraKB

                            _Reexpedidora.Domicilio = _Revalidaciones(0).DomicilioReexpedidoraKB

                            _Reexpedidora.RFC = _Revalidaciones(0).RfcReexpedidoraKB

                            _Reexpedidora.PuedeInsertar = True

                            _Reexpedidora.ClaveReexpedidora = _Revalidaciones(0).ReexpedidoraKB.Substring(0, 4)

                        End If


                    Else

                        _Reexpedidora.NombreReexpedidora = _Revalidaciones(0).ReexpedidoraKB

                        _Reexpedidora.Domicilio = _Revalidaciones(0).DomicilioReexpedidoraKB

                        _Reexpedidora.RFC = _Revalidaciones(0).RfcReexpedidoraKB

                        _Reexpedidora.PuedeInsertar = True

                        _Reexpedidora.ClaveReexpedidora = _Revalidaciones(0).ReexpedidoraKB.Substring(0, 4)

                    End If

                Else

                    _Reexpedidora.NombreReexpedidora = _Revalidaciones(0).ReexpedidoraKB

                    _Reexpedidora.Domicilio = _Revalidaciones(0).DomicilioReexpedidoraKB

                    _Reexpedidora.RFC = _Revalidaciones(0).RfcReexpedidoraKB

                    _Reexpedidora.PuedeInsertar = True

                    _Reexpedidora.ClaveReexpedidora = _Revalidaciones(0).ReexpedidoraKB.Substring(0, 4)

                End If


            Else

                _Reexpedidora.NombreReexpedidora = _Revalidaciones(0).ReexpedidoraKB

                _Reexpedidora.Domicilio = _Revalidaciones(0).DomicilioReexpedidoraKB

                _Reexpedidora.RFC = _Revalidaciones(0).RfcReexpedidoraKB

                _Reexpedidora.PuedeInsertar = True

                _Reexpedidora.ClaveReexpedidora = _Revalidaciones(0).ReexpedidoraKB.Substring(0, 4)

            End If

            If _Reexpedidora.PuedeInsertar Then

                'Procedemos con la inserción de de la reexpedidora
                TagLocal_ = InsercionReexpedidoraSysExpert(_Reexpedidora)

                'Si se incertó de forma correcto se procede con la actualización del campo en la lista en caso contrario se acaba la transacción
                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    For contador_ = 0 To _Revalidaciones.Count - 1

                        _Revalidaciones(contador_).IdReexpedidora = _Reexpedidora.IdReexpedidora

                    Next contador_

                    Return TagLocal_

                End If

            End If

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function InsercionReexpedidoraSysExpert(ByRef Reexpedidora_ As Reexpedidoras) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Se procede con la inserción de la reexpedidora 

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(Reexpedidora_.SentenciaInsert())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarCatalogoBuqueRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Buque = New Buques(_ioperacionescatalogo)

            'En caso de que no exista el buque se insertará en SysExpert, solo debería aparecer un registro por todos los viajes

            _sql = "Select IdBuque from [SysExpert].[dbo].[Buques] where Buque = '" & _Revalidaciones(0).NaveKB & "'"

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                            _Revalidaciones(0).IdBuque = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

                            _Buque.PuedeInsertar = False

                        Else

                            _Buque.Buque = _Revalidaciones(0).NaveKB

                            _Buque.ClaveBuque = _Revalidaciones(0).NaveKB.Substring(0, 9)

                            _Buque.PuedeInsertar = True

                        End If


                    Else

                        _Buque.Buque = _Revalidaciones(0).NaveKB

                        _Buque.ClaveBuque = _Revalidaciones(0).NaveKB.Substring(0, 9)

                        _Buque.PuedeInsertar = True

                    End If


                Else

                    _Buque.Buque = _Revalidaciones(0).NaveKB

                    _Buque.ClaveBuque = _Revalidaciones(0).NaveKB.Substring(0, 9)

                    _Buque.PuedeInsertar = True

                End If


            Else

                _Buque.Buque = _Revalidaciones(0).NaveKB

                _Buque.ClaveBuque = _Revalidaciones(0).NaveKB.Substring(0, 9)

                _Buque.PuedeInsertar = True

            End If

            If _Buque.PuedeInsertar Then

                'Procedemos con la inserción del buque
                TagLocal_ = InsercionBuqueSysExpert(_Buque)

                'Si se incertó de forma correcto se procede con la actualización del campo en la lista en caso contrario se acaba la transacción
                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    For contador_ = 0 To _Revalidaciones.Count - 1

                        _Revalidaciones(contador_).IdBuque = _Buque.IdBuque

                    Next contador_

                    Return TagLocal_

                End If

            End If

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function InsercionBuqueSysExpert(ByRef Buque_ As Buques) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Se procede con la inserción del buque

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(Buque_.SentenciaInsert())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarRevalidacionSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Verificamos si ya existe un registro de revalidación de cada unas de las referencias de los viajes involucrados.
            For contador_ = 0 To _Revalidaciones.Count - 1

                _sql = "Select IdRevalidacionBL from [SysExpert].[dbo].[RevalidacionBL] where idReferencia = " & _Revalidaciones(contador_).IdReferencia

                sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                                _Revalidaciones(contador_).IdRevalidacionBL = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

                                _Revalidaciones(contador_).PuedeInsertar = False

                                _Revalidaciones(contador_).PuedeActualizar = True

                            Else

                                _Revalidaciones(contador_).PuedeInsertar = True

                                _Revalidaciones(contador_).PuedeActualizar = False

                            End If

                        Else


                            _Revalidaciones(contador_).PuedeInsertar = True

                            _Revalidaciones(contador_).PuedeActualizar = False

                        End If

                    Else

                        _Revalidaciones(contador_).PuedeInsertar = True

                        _Revalidaciones(contador_).PuedeActualizar = False

                    End If

                Else

                    _Revalidaciones(contador_).PuedeInsertar = True

                    _Revalidaciones(contador_).PuedeActualizar = False

                End If

            Next contador_

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function RealizarOperacionRevalidacionSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            'Se recorre toda la lista de revalidación y se va realizando la operación correspondiente en caso de marcar error se termina  la transsación
            For contador_ = 0 To _Revalidaciones.Count - 1

                If _Revalidaciones(contador_).PuedeInsertar Then

                    'Se procedé con la inserción en caso de marcar error se terminará la transacción
                    TagLocal_ = InsercionRevalidacionSysExpert(_Revalidaciones(contador_))

                Else

                    'Se procedé con la actualización en caso de marcar error se terminará la transacción
                    TagLocal_ = ActualizacionRevalidacionSysExpert(_Revalidaciones(contador_))

                End If

                If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    Return TagLocal_

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function InsercionRevalidacionSysExpert(ByRef Revalidacion_ As RevalidacionBl) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Se procede con la inserción de la revalidación

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(Revalidacion_.SentenciaInsert())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function ActualizacionRevalidacionSysExpert(ByRef Revalidacion_ As RevalidacionBl) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Se procede con la actualización de la revalidación

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(Revalidacion_.SentenciaUpdate())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function iniciarProcesoRevalidacionBlContenedor() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para revalidación bl contenedor
            TagLocal_ = ObtenerInformacionRevalidacionBLContenedorKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                'Verificamos la existencia de un registro en RebalidacionBLContenedor de SysExpert
                TagLocal_ = VerificarRevalidacionBlContenedorSysExpert()

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    'Se procederá con el tipo de operación por cada una de las referencias y contenedores
                    TagLocal_ = RealizarOperacionRevalidacionBlContenedorSysExpert()

                    Return TagLocal_

                Else

                    Return TagLocal_

                End If

            Else

                Return TagLocal_

            End If

        End Function

        Private Function ObtenerInformacionRevalidacionBLContenedorKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos toda la información para insertarla en revalidacón y lo guardamos en una lista
            _RevalidacionesBlContenedor = New List(Of RevalidacionBLContenedor)

            For contador_ = 0 To _Revalidaciones.Count - 1

                _sql = "select '' LLave," &
                       "'' idrevalidacion," &
                       "tco.i_Cve_TamañoContenedor," &
                       "1 Cantidad," &
                       "ci.i_Peso," &
                       "NULL FechaLiberacion," &
                       "ci.t_NumeroContenedor," &
                       "ci.i_Cve_TipoContenedor," &
                       "case when tc.t_Cve_ClaseContenedor = 'CON' then 1" &
                       "     when tc.t_Cve_ClaseContenedor = 'ISO' then 7 end i_Cve_ClaseContenedor," &
                       "ci.i_Pies," &
                       "NULL idContenedorRevalidacion " &
                "from VT003ReferenciasContenedoresImpo as rci " &
                "inner join VT003ContenedoresImpo as ci on ci.i_Cve_DivisionMiEmpresa = rci.i_Cve_DivisionMiEmpresa and rci.i_Cve_ReferenciasContenedoresImpo = ci.i_Cve_ReferenciasContenedoresImpo " &
                "inner join VT003TipoContenedor as tc on tc.i_Cve_TipoContenedor = ci.i_Cve_TipoContenedor " &
                "inner join VT003TamañoContenedor as tco on tco.i_Cve_TamañoContenedor = ci.i_Cve_TamañoContenedor " &
                "where rci.i_Cve_ReferenciasContenedoresImpo = " & _Revalidaciones(contador_).i_Cve_RevalidacionImpoKB

                sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                                For contador2_ = 0 To sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count - 1

                                    With _RevalidacionesBlContenedor

                                        Dim RevalidacionblContenedor_ = New RevalidacionBLContenedor(_ioperacionescatalogo)

                                        RevalidacionblContenedor_.IdRevalidacionBL = _Revalidaciones(contador_).IdRevalidacionBL

                                        RevalidacionblContenedor_.IdContenedor = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("i_Cve_TamañoContenedor"))

                                        RevalidacionblContenedor_.Cantidad = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("Cantidad"))

                                        RevalidacionblContenedor_.Peso = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("i_Peso"))

                                        RevalidacionblContenedor_.FechaLiberacion = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("FechaLiberacion"))

                                        RevalidacionblContenedor_.NumeroContenedor = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("t_NumeroContenedor"))

                                        RevalidacionblContenedor_.IdContenedorTipo = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("i_Cve_TipoContenedor"))

                                        RevalidacionblContenedor_.IdContenedorClase = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("i_Cve_ClaseContenedor"))

                                        RevalidacionblContenedor_.Pies = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("i_Pies"))

                                        RevalidacionblContenedor_.IdContenedorRevalidacion = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(contador2_)("idContenedorRevalidacion"))

                                        .Add(RevalidacionblContenedor_)

                                    End With

                                Next contador2_

                            End If

                        End If

                    End If

                End If

            Next contador_

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarRevalidacionBlContenedorSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Verificamos si ya existe un registro de contenedores de cada unas de las referencias de los viajes involucrados.
            For contador_ = 0 To _RevalidacionesBlContenedor.Count - 1

                _sql = "Select IdRevalidacionBLContenedor from [SysExpert].[dbo].[RevalidacionBLContenedor] where IdRevalidacionBL = " & _RevalidacionesBlContenedor(contador_).IdRevalidacionBL & " and NumeroContenedor='" & _RevalidacionesBlContenedor(contador_).NumeroContenedor & "'"

                sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

                If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                    If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                        If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                            If Not sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                                _RevalidacionesBlContenedor(contador_).IdRevalidacionBLContenedor = VerificarDBNull(sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0))

                                _RevalidacionesBlContenedor(contador_).PuedeInsertar = False

                                _RevalidacionesBlContenedor(contador_).PuedeActualizar = True

                            Else

                                _RevalidacionesBlContenedor(contador_).PuedeInsertar = True

                                _RevalidacionesBlContenedor(contador_).PuedeActualizar = False

                            End If

                        Else


                            _RevalidacionesBlContenedor(contador_).PuedeInsertar = True

                            _RevalidacionesBlContenedor(contador_).PuedeActualizar = False

                        End If

                    Else

                        _RevalidacionesBlContenedor(contador_).PuedeInsertar = True

                        _RevalidacionesBlContenedor(contador_).PuedeActualizar = False

                    End If

                Else

                    _RevalidacionesBlContenedor(contador_).PuedeInsertar = True

                    _RevalidacionesBlContenedor(contador_).PuedeActualizar = False

                End If

            Next contador_

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function RealizarOperacionRevalidacionBlContenedorSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            'Se recorre toda la lista de revalidación bl contenedor y se va realizando la operación correspondiente en caso de marcar error se termina  la transsación

            For contador_ = 0 To _RevalidacionesBlContenedor.Count - 1

                If _RevalidacionesBlContenedor(contador_).PuedeInsertar Then

                    'Se procedé con la inserción en caso de marcar error se terminará la transacción
                    TagLocal_ = InsercionRevalidacionSysExpert(_RevalidacionesBlContenedor(contador_))

                Else

                    'Se procedé con la actualización en caso de marcar error se terminará la transacción
                    TagLocal_ = ActualizacionRevalidacionSysExpert(_RevalidacionesBlContenedor(contador_))

                End If


                If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    Return TagLocal_

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function InsercionRevalidacionSysExpert(ByRef RevalidacionBLContenedor_ As RevalidacionBLContenedor) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Se procede con la inserción de la revalidación bl contenedor

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(RevalidacionBLContenedor_.SentenciaInsert())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function ActualizacionRevalidacionSysExpert(ByRef RevalidacionBLContenedor_ As RevalidacionBLContenedor) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Se procede con la actualización de la revalidación bl contenedor

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(RevalidacionBLContenedor_.SentenciaUpdate())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function iniciarProcesoTrafico() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Trafico = New Trafico(_ioperacionescatalogo)

            'Cargamos los datos de cada ua de las referencias en un objeto trafico
            For contador_ = 0 To _Revalidaciones.Count - 1

                _Trafico.idReferencia = _Revalidaciones(contador_).IdReferencia

                _Trafico.idAgencia = _Revalidaciones(contador_).IdAgencia

                _Trafico.FechaMovimiento = Date.Now.Date

                _Trafico.Dato = "Campos del módulo de revalidación capturados"

                _Trafico.ProcesadoWeb = 0

                _Trafico.Observaciones = "Actualización mediante el módulo de viajes KromBase a la referencia con idReferencia: " & _Revalidaciones(contador_).IdReferencia

                _Trafico.GuiaHouse = " "

                _Trafico.GuiaMaster = " "

                _Trafico.NumeroFactura = " "

                _Trafico.HoraMovimiento = DateTime.Now.Hour & ":" & DateTime.Now.Minute & ":" & DateTime.Now.Second

                _Trafico.FechaAlta = Date.Now.Date

                'En caso de que tenga fecha de revalidación entonces se procede con la inserción, de otra forma no se hará
                If _Revalidaciones(contador_).FechaRevalidacion Is Nothing Then

                    _Trafico.PuedeInsertar = False

                Else

                    _Trafico.PuedeInsertar = True

                End If

                If _Trafico.PuedeInsertar Then

                    TagLocal_ = InsercionTraficoSysExpert(_Trafico)

                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        _insercionTrafico = True

                    Else

                        _insercionTrafico = False

                    End If

                End If

            Next contador_

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function InsercionTraficoSysExpert(ByRef Trafico_ As Trafico) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            'Iniciamos con el proceso de inserción en tráfico
            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(Trafico_.SentenciaInsert())

            Return sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.GetTagWatcherObject

        End Function

        Private Function VerificarDBNull(ByVal campo_ As Object) As String

            If campo_ Is DBNull.Value Then

                Return Nothing

            End If

            Return campo_

        End Function

#End Region

    End Class

End Namespace
