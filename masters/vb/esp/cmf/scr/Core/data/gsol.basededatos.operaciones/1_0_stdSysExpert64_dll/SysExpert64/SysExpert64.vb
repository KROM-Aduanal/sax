Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports Gsol.Componentes.SistemaBase.GsDialogo

Namespace Gsol.BaseDatos

    Public Class SysExpert64

#Region "Atributos"

        Private _index As Integer

        Private _referencia As String

        Private _cvedetalle As Integer

        Private _concepto As Integer

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

        Private _Proveedor As Proveedores

        Private _VinculacionesClienteProveedor As List(Of VinculacionClienteProveedor)

        Private _VinculacionClienteProveedor As VinculacionClienteProveedor

        Private _Partes As List(Of Partes)

        Private _auxTagWatcher As TagWatcher

        Private _sql As String

        Private _insercionTrafico As Boolean

        Private _ControlContenedores As List(Of ControlContenedores)

        Private _ControlContenedoresDetalle As List(Of ControlContenedoresDetalle)

        Private _TraficoVal As List(Of Trafico)

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

        Property Revalidaciones As List(Of RevalidacionBl)

            Get

                Return _Revalidaciones

            End Get

            Set(value As List(Of RevalidacionBl))

                _Revalidaciones = value

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

            Dim TagLocal_ = New TagWatcher

        End Sub

        Sub New(ByVal referencia_ As String, ByVal CveDet_ As Integer, ByVal concepto_ As Integer, ByVal ioperacionescatalogo_ As OperacionesCatalogo)

            _ioperacionescatalogo = ioperacionescatalogo_

            _referencia = referencia_

            _cvedetalle = CveDet_

            _concepto = concepto_

            _insercionTrafico = False

            _sistema = New Organismo

            _auxTagWatcher = New TagWatcher

            Dim TagLocal_ = New TagWatcher

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

                        _auxTagWatcher = ActualizarFechaEntrada()

                        If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                            _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                        Else

                            _sistema.GsDialogo("¡Información sincronizada con SysExpert!", TipoDialogo.Aviso)

                        End If

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

                TagLocal_ = EliminarContenedoresMarcas()

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

            Else

                Return TagLocal_

            End If

        End Function

        Private Function ObtenerInformacionRevalidacionKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para revalidación y la guardamos en una lista.

            _sql = "select voaa.i_Cve_ReferenciaExterna, " &
                              "vi.i_Cve_Transbordo, " &
                              "vi.f_ETA, " &
                              "vi.i_Cve_Reexpedidora, " &
                              "vi.RFC_Reexpedidora, " &
                              "vi.Domicilio_Reexpedidora, " &
                              "vi.i_Cve_Naviera, " &
                              "vi.t_caat, " &
                              "vi.i_Cve_Nave, " &
                              "vi.i_Cve_Naviera, " &
                              "vi.i_Cve_Origen, " &
                              "p.t_Cve_Puerto, " &
                              "p.t_Nombre NombrePuerto, " &
                              "vi.f_SalidaOrigen, " &
                              "vi.f_Atraque, " &
                              "case when rci.t_Cve_TipoCarga = 'P' then 'C' else rci.t_Cve_TipoCarga end t_Cve_TipoCarga, " &
                              "(select  TOP 1     case when mi.t_ClaseCarga = 'CARGA GENERAL' and rci.t_Cve_TipoCarga = 'S' then '3' " &
                              "                        when mi.t_ClaseCarga = 'PRODUCTOS DE ACERO' and rci.t_Cve_TipoCarga = 'S' then '4' " &
                              "                        when mi.t_ClaseCarga = 'GRANEL MINERAL' and rci.t_Cve_TipoCarga = 'S' then '5' " &
                              "                        when mi.t_ClaseCarga = 'GRANEL AGRICOLA' and rci.t_Cve_TipoCarga = 'S' then '6' " &
                              "                        when mi.t_ClaseCarga = 'FLUIDOS' and rci.t_Cve_TipoCarga = 'S' then '7' " &
                              "                        when mi.t_ClaseCarga = 'CARGA GENERAL' and rci.t_Cve_TipoCarga = 'L' then '8' " &
                              "                        when mi.t_ClaseCarga = 'PRODUCTOS DE ACERO' and rci.t_Cve_TipoCarga = 'L' then '9' " &
                              "                        when mi.t_ClaseCarga = 'GRANEL MINERAL' and rci.t_Cve_TipoCarga = 'L' then '10' " &
                              "                        when mi.t_ClaseCarga = 'GRANEL AGRICOLA' and rci.t_Cve_TipoCarga = 'L' then '11' " &
                              "                        when mi.t_ClaseCarga = 'FLUIDOS' and rci.t_Cve_TipoCarga = 'L' then '12' " &
                              "                        when mi.t_ClaseCarga = 'LAMINA EN ROLLOS' and rci.t_Cve_TipoCarga = 'S' then '41' " &
                              "                        else null " &
                              "                     end  " &
                              "from VT003MarcasImpo as mi where mi.i_Cve_RevalidacionImpo = rci.i_Cve_RevalidacionImpo) idCargaClase,  " &
                              "rci.f_Revalidacion, " &
                              "vi.Reexpedidora, " &
                              "vi.Naviera, " &
                              "vi.Nave, " &
                              "vi.t_Observaciones, " &
                              "rci.i_Cve_RevalidacionImpo, " &
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
                              "when voaa.i_AduanaSeccion = 421 and voaa.i_Patente = 3210 then 46 " &
                               "End idAgencia, " &
                               "case when vi.TipoNave = 2 then case when vi.f_Fondeo is null then case when vi.f_Atraque is null then null else replace(cast(vi.f_atraque as varchar(10)),'-','') end else replace(cast(vi.f_Fondeo as varchar(10)),'-','') end else null end EntradaReal " &
                       "from VT003ViajesImpo as vi " &
                       "inner join VT003ViajesRevalidacionImpo as vir on vir.i_Cve_Viaje = vi.i_Cve_Viaje and vir.i_Cve_DivisionMiEmpresa = vi.i_Cve_DivisionMiEmpresa   " &
                       "inner join VT003RevalidacionImpo as rci on rci.i_Cve_RevalidacionImpo = vir.i_Cve_RevalidacionImpo and rci.i_Cve_DivisionMiEmpresa = vir.i_Cve_DivisionMiEmpresa  " &
                       "inner join VT003OperacionesAgenciasAduanales as voaa on voaa.i_Cve_VinOperacionesAgenciasAduanales = rci.i_Cve_VinOperacionesAgenciasAduanales and voaa.i_Cve_DivisionMiEmpresa	= rci.i_Cve_DivisionMiEmpresa " &
                       "left join Vt003Puertos as p on p.i_cve_Puerto = vi.i_cve_Origen " &
                       "where vi.i_Cve_Viaje = " & _index

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9003)

                    Return TagLocal_

                Else

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _Revalidaciones

                            Dim Revalidacion_ = New RevalidacionBl(_ioperacionescatalogo)

                            Revalidacion_.IdReferencia = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ReferenciaExterna"))

                            Revalidacion_.Trasbordo = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_Transbordo"))

                            Revalidacion_.FechaETA = VerificarDBNull(cursorDataTable_(contador_)("f_ETA"))

                            Revalidacion_.IdReexpedidora = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_Reexpedidora"))

                            Revalidacion_.IdPuerto = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_Origen"))

                            Revalidacion_.IdAgencia = VerificarDBNull(cursorDataTable_(contador_)("idAgencia"))

                            Revalidacion_.FechaEmbarque = VerificarDBNull(cursorDataTable_(contador_)("f_SalidaOrigen"))

                            Revalidacion_.FechaETALAX = VerificarDBNull(cursorDataTable_(contador_)("f_Atraque"))

                            Revalidacion_.ClaveCargaTipo = VerificarDBNull(cursorDataTable_(contador_)("t_Cve_TipoCarga"))

                            Revalidacion_.IdCargaClase = VerificarDBNull(cursorDataTable_(contador_)("idCargaClase"))

                            Revalidacion_.FechaRevalidacion = VerificarDBNull(cursorDataTable_(contador_)("f_Revalidacion"))

                            Revalidacion_.Observaciones = VerificarDBNull(cursorDataTable_(contador_)("t_Observaciones"))

                            Revalidacion_.IdBuque = cursorDataTable_(contador_)("i_Cve_Nave")

                            Revalidacion_.IdNaviera = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_Naviera"))

                            Revalidacion_.ReexpedidoraKB = VerificarDBNull(cursorDataTable_(contador_)("Reexpedidora"))

                            Revalidacion_.RfcReexpedidoraKB = VerificarDBNull(cursorDataTable_(contador_)("RFC_Reexpedidora"))

                            Revalidacion_.DomicilioReexpedidoraKB = VerificarDBNull(cursorDataTable_(contador_)("Domicilio_Reexpedidora"))

                            Revalidacion_.NaveKB = VerificarDBNull(cursorDataTable_(contador_)("Nave"))

                            Revalidacion_.NavieraKB = VerificarDBNull(cursorDataTable_(contador_)("Naviera"))

                            Revalidacion_.CAATNavieraKB = VerificarDBNull(cursorDataTable_(contador_)("t_caat"))

                            Revalidacion_.ClavePuertoKB = VerificarDBNull(cursorDataTable_(contador_)("t_Cve_Puerto"))

                            Revalidacion_.NombrePuertoKB = VerificarDBNull(cursorDataTable_(contador_)("NombrePuerto"))

                            Revalidacion_.i_Cve_RevalidacionImpoKB = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_RevalidacionImpo"))

                            Revalidacion_.EntradaRealKB = VerificarDBNull(cursorDataTable_(contador_)("EntradaReal"))

                            .Add(Revalidacion_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

        Private Function EliminarContenedoresMarcas() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Trafico = New Trafico(_ioperacionescatalogo)

            'Cargamos los datos de cada ua de las referencias en un objeto trafico
            For contador_ = 0 To _Revalidaciones.Count - 1

                Dim _sql = "USE SysExpert; DECLARE @Valor INT; EXEC @Valor = SP003BorrarContenedoresMarcas " & _Revalidaciones(contador_).IdReferencia & ";  select @Valor USE SOLIUM;"

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    Return TagLocal_

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoPuertosRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Puerto = New Puertos(_ioperacionescatalogo)

            'En caso de que no exista el Puerto mandará un error y se acabará la transacción, solo debería aparecer un registro por todos los viajes

            _sql = "Select IdPuerto from [SysExpert].[dbo].[Puertos] where Puerto = '" & _Revalidaciones(0).NombrePuertoKB & "' and Clave = '" & _Revalidaciones(0).ClavePuertoKB & "'"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                    Return TagLocal_

                Else

                    _Puerto.IdPuerto = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            If Not _Puerto.IdPuerto Is Nothing Then

                For contador_ = 0 To _Revalidaciones.Count - 1

                    _Revalidaciones(contador_).IdPuerto = _Puerto.IdPuerto

                Next contador_
            End If

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoNavierasRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Naviera = New Navieras(_ioperacionescatalogo)

            If Not _Revalidaciones(0).CAATNavieraKB Is Nothing Then

                'En caso de que no exista la Naviera mandará un error y se acabará la transacción, solo debería aparecer un registro por todos los viajes

                _sql = "Select IdNaviera from [SysExpert].[dbo].[Navieras] where CAAT= '" & _Revalidaciones(0).CAATNavieraKB & "'"

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        _Naviera.CAAT = _Revalidaciones(0).CAATNavieraKB

                        _Naviera.Naviera = _Revalidaciones(0).NavieraKB

                        _Naviera.PuedeInsertar = True

                    Else

                        _Naviera.IdNaviera = VerificarDBNull(cursorDataTable_(0)(0))

                        _Naviera.PuedeInsertar = False

                    End If

                End If

                If _Naviera.PuedeInsertar Then

                    'Procedemos con la inserción de de la naviera
                    TagLocal_ = InsercionNavieraSysExpert(_Naviera)

                    'Si se incertó de forma correcto se procede con la actualización del campo en la lista en caso contrario se acaba la transacción
                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        For contador_ = 0 To _Revalidaciones.Count - 1

                            _Revalidaciones(contador_).IdNaviera = _Naviera.IdNaviera

                        Next contador_

                        Return TagLocal_

                    End If

                End If

                If Not _Naviera.IdNaviera Is Nothing Then

                    For contador_ = 0 To _Revalidaciones.Count - 1

                        _Revalidaciones(contador_).IdNaviera = _Naviera.IdNaviera

                    Next contador_

                End If

            End If

            Return TagLocal_

        End Function

        Private Function InsercionNavieraSysExpert(ByRef Naviera_ As Navieras) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción de la naviera

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Naviera_.SentenciaInsert())

            Return TagLocal_

        End Function


        Private Function VerificarCatalogoReexpedidorasRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Reexpedidora = New Reexpedidoras(_ioperacionescatalogo)

            If Not _Revalidaciones(0).RfcReexpedidoraKB Is Nothing Then

                'En caso de que no exista la reexpedidora se insertará en SysExpert, solo debería aparecer un registro por todos los viajes

                _sql = "Select IdReexpedidora from [SysExpert].[dbo].[Reexpedidoras] where RFC = '" & _Revalidaciones(0).RfcReexpedidoraKB & "'"

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        _Reexpedidora.NombreReexpedidora = _Revalidaciones(0).ReexpedidoraKB

                        _Reexpedidora.Domicilio = _Revalidaciones(0).DomicilioReexpedidoraKB

                        _Reexpedidora.RFC = _Revalidaciones(0).RfcReexpedidoraKB

                        _Reexpedidora.PuedeInsertar = True

                        _Reexpedidora.ClaveReexpedidora = _Revalidaciones(0).ReexpedidoraKB.Substring(0, 4)

                    Else

                        _Reexpedidora.IdReexpedidora = VerificarDBNull(cursorDataTable_(0)(0))

                        _Reexpedidora.PuedeInsertar = False

                    End If

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

                If Not _Reexpedidora.IdReexpedidora Is Nothing Then

                    For contador_ = 0 To _Revalidaciones.Count - 1

                        _Revalidaciones(contador_).IdReexpedidora = _Reexpedidora.IdReexpedidora

                    Next contador_

                End If

            End If

            Return TagLocal_

        End Function

        Private Function InsercionReexpedidoraSysExpert(ByRef Reexpedidora_ As Reexpedidoras) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción de la reexpedidora 

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Reexpedidora_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoBuqueRevalidacionSysexpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Buque = New Buques(_ioperacionescatalogo)

            'En caso de que no exista el buque se insertará en SysExpert, solo debería aparecer un registro por todos los viajes

            _sql = "Select IdBuque from [SysExpert].[dbo].[Buques] where Buque = '" & _Revalidaciones(0).NaveKB & "'"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    _Buque.Buque = _Revalidaciones(0).NaveKB

                    _Buque.ClaveBuque = _Revalidaciones(0).NaveKB.Substring(0, 3)

                    _Buque.PuedeInsertar = True

                Else

                    '_Revalidaciones(0).IdBuque = VerificarDBNull(cursorDataTable_(0)(0))

                    _Buque.IdBuque = VerificarDBNull(cursorDataTable_(0)(0))

                    _Buque.PuedeInsertar = False

                End If

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

            If Not _Buque.IdBuque Is Nothing Then

                For contador_ = 0 To _Revalidaciones.Count - 1

                    _Revalidaciones(contador_).IdBuque = _Buque.IdBuque

                Next contador_

            End If

            Return TagLocal_

            Return TagLocal_

        End Function

        Private Function InsercionBuqueSysExpert(ByRef Buque_ As Buques) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción del buque

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Buque_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function VerificarRevalidacionSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Verificamos si ya existe un registro de revalidación de cada unas de las referencias de los viajes involucrados.
            For contador_ = 0 To _Revalidaciones.Count - 1

                _sql = "Select IdRevalidacionBL from [SysExpert].[dbo].[RevalidacionBL] where idReferencia = " & _Revalidaciones(contador_).IdReferencia

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        _Revalidaciones(contador_).PuedeInsertar = True

                        _Revalidaciones(contador_).PuedeActualizar = False

                    Else

                        _Revalidaciones(contador_).IdRevalidacionBL = VerificarDBNull(cursorDataTable_(0)(0))

                        _Revalidaciones(contador_).PuedeInsertar = False

                        _Revalidaciones(contador_).PuedeActualizar = True

                    End If

                End If

            Next contador_

            Return TagLocal_

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

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción de la revalidación

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Revalidacion_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function ActualizacionRevalidacionSysExpert(ByRef Revalidacion_ As RevalidacionBl) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la actualización de la revalidación

            Dim sql_ = Revalidacion_.SentenciaUpdate()

            TagLocal_ = VerificarWhere(sql_)

            If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            End If

            Return TagLocal_

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
                       "ci.i_Peso, " &
                       "NULL FechaLiberacion," &
                       "ci.t_NumeroContenedor," &
                       "tco.i_Pies," &
                       "NULL idContenedorRevalidacion, " &
                       "tco.i_Cve_idContenedor, " &
                       "tco.i_Cve_idContenedorClase, " &
                       "tco.i_Cve_idContenedorTipo " &
                "from VT003RevalidacionImpo as rci " &
                "inner join VT003ContenedoresImpo as ci on ci.i_Cve_DivisionMiEmpresa = rci.i_Cve_DivisionMiEmpresa and rci.i_Cve_RevalidacionImpo = ci.i_Cve_RevalidacionImpo " &
                "inner join VT003TamañoContenedor as tco on tco.i_Cve_TamañoContenedor = ci.i_Cve_TamañoContenedor " &
                "where rci.i_Cve_RevalidacionImpo  = " & _Revalidaciones(contador_).i_Cve_RevalidacionImpoKB & " " &
                "union " &
                "select '' LLave," &
                       "'' idrevalidacion," &
                       "NULL," &
                       "ci.i_Cantidad Cantidad," &
                       "ci.i_Peso, " &
                       "NULL FechaLiberacion," &
                       "ci.t_Marca," &
                       "NULL," &
                       "NULL idContenedorRevalidacion, " &
                       "NULL, " &
                       "tco.i_Cve_ClaseMarcas, " &
                       "NULL " &
                "from VT003RevalidacionImpo as rci  " &
                "inner join VT003MarcasImpo as ci on ci.i_Cve_DivisionMiEmpresa = rci.i_Cve_DivisionMiEmpresa and rci.i_Cve_RevalidacionImpo = ci.i_Cve_RevalidacionImpo   " &
                "inner join VT003ClaseMarcas as tco on tco.i_Cve_ClaseMarcas = ci.i_Cve_ClaseMarcas " &
                "where rci.i_Cve_RevalidacionImpo  = " & _Revalidaciones(contador_).i_Cve_RevalidacionImpoKB

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        Continue For

                    Else

                        For contador2_ = 0 To sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count - 1

                            With _RevalidacionesBlContenedor

                                Dim RevalidacionblContenedor_ = New RevalidacionBLContenedor(_ioperacionescatalogo)

                                RevalidacionblContenedor_.IdRevalidacionBL = _Revalidaciones(contador_).IdRevalidacionBL

                                RevalidacionblContenedor_.IdContenedor = VerificarDBNull(cursorDataTable_(contador2_)("i_Cve_idContenedor"))

                                RevalidacionblContenedor_.Cantidad = VerificarDBNull(cursorDataTable_(contador2_)("Cantidad"))

                                RevalidacionblContenedor_.Peso = VerificarDBNull(cursorDataTable_(contador2_)("i_Peso"))

                                RevalidacionblContenedor_.FechaLiberacion = VerificarDBNull(cursorDataTable_(contador2_)("FechaLiberacion"))

                                RevalidacionblContenedor_.NumeroContenedor = VerificarDBNull(cursorDataTable_(contador2_)("t_NumeroContenedor"))

                                RevalidacionblContenedor_.IdContenedorTipo = VerificarDBNull(cursorDataTable_(contador2_)("i_Cve_idContenedorTipo"))

                                RevalidacionblContenedor_.IdContenedorClase = VerificarDBNull(cursorDataTable_(contador2_)("i_Cve_idContenedorClase"))

                                RevalidacionblContenedor_.Pies = VerificarDBNull(cursorDataTable_(contador2_)("i_Pies"))

                                RevalidacionblContenedor_.IdContenedorRevalidacion = VerificarDBNull(cursorDataTable_(contador2_)("idContenedorRevalidacion"))

                                .Add(RevalidacionblContenedor_)

                            End With

                        Next contador2_

                    End If

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function VerificarRevalidacionBlContenedorSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Verificamos si ya existe un registro de contenedores de cada unas de las referencias de los viajes involucrados.
            For contador_ = 0 To _RevalidacionesBlContenedor.Count - 1

                _sql = "Select IdRevalidacionBLContenedor from [SysExpert].[dbo].[RevalidacionBLContenedor] where IdRevalidacionBL = " & _RevalidacionesBlContenedor(contador_).IdRevalidacionBL & " and NumeroContenedor='" & _RevalidacionesBlContenedor(contador_).NumeroContenedor & "'"

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        _RevalidacionesBlContenedor(contador_).PuedeInsertar = True

                        _RevalidacionesBlContenedor(contador_).PuedeActualizar = False

                    Else

                        _RevalidacionesBlContenedor(contador_).IdRevalidacionBLContenedor = VerificarDBNull(cursorDataTable_(0)(0))

                        _RevalidacionesBlContenedor(contador_).PuedeInsertar = False

                        _RevalidacionesBlContenedor(contador_).PuedeActualizar = True

                    End If

                End If

            Next contador_

            Return TagLocal_

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

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción de la revalidación bl contenedor
            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(RevalidacionBLContenedor_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function ActualizacionRevalidacionSysExpert(ByRef RevalidacionBLContenedor_ As RevalidacionBLContenedor) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción de la revalidación bl contenedor

            Dim sql_ = RevalidacionBLContenedor_.SentenciaUpdate()

            TagLocal_ = VerificarWhere(sql_)

            If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            End If

            Return TagLocal_

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

                _Trafico.Observaciones = "Actualización mediante el módulo de viajes KromBase a la referencia con idReferencia: " & _Revalidaciones(contador_).IdReferencia & ", el usuario responsable fue: " & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.CredencialUsuario

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

                        '_insercionTrafico = True

                        _Revalidaciones(contador_).seInserto = True

                    Else

                        '_insercionTrafico = False

                        _Revalidaciones(contador_).seInserto = False

                    End If

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function InsercionTraficoSysExpert(ByRef Trafico_ As Trafico) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Iniciamos con el proceso de inserción en tráfico
            'MsgBox(Trafico_.SentenciaInsert())
            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Trafico_.SentenciaInsert())

            Return TagLocal_

        End Function

        ' Se creo una conexion esta función par auso del KBW, utiliza conexión libre
        Private Function InsercionTraficoSysExpertConexionLibre(ByRef Trafico_ As Trafico) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Iniciamos con el proceso de inserción en tráfico
            'MsgBox(Trafico_.SentenciaInsert())
            'TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Trafico_.SentenciaInsert())

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            TagLocal_ = sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(Trafico_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function ActualizarFechaEntrada() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Trafico = New Trafico(_ioperacionescatalogo)

            'Cargamos los datos de cada ua de las referencias en un objeto trafico
            For contador_ = 0 To _Revalidaciones.Count - 1

                'En caso de que tenga fecha de entrada se procedé con la actualización
                If Not _Revalidaciones(contador_).EntradaRealKB Is Nothing And _Revalidaciones(contador_).seInserto Then

                    Dim _sql = "USE SysExpert; DECLARE @Valor INT; EXEC @Valor = spActualizaFechaEntrada  '" & _Revalidaciones(contador_).EntradaRealKB & "'," & _Revalidaciones(contador_).IdReferencia & ";  select @Valor USE SOLIUM;"

                    TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                        Return TagLocal_

                    Else

                        Dim cursorDataTable_ As New DataTable

                        cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                        If cursorDataTable_.Rows.Count > 0 Then

                            'Verifica que ya este pagado
                            If VerificarDBNull(cursorDataTable_(0)(0)) = 9005 Then

                                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9005)

                                Return TagLocal_

                                'Verifica que no haya generado ningun otro error
                            ElseIf VerificarDBNull(cursorDataTable_(0)(0)) = 9006 Then

                                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9006)

                                Return TagLocal_

                            End If

                        End If

                    End If

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function VerificarDBNull(ByVal campo_ As Object) As String

            If campo_ Is DBNull.Value Then

                Return Nothing

            End If

            Return campo_

        End Function

        Private Function VerificarWhere(ByVal sql_ As String) As TagWatcher

            If sql_.Contains("where") Or sql_.Contains("WHERE") Then

                Return New TagWatcher

            Else

                Dim TagLocal_ = New TagWatcher

                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                Return TagLocal_

            End If


        End Function

        Sub RevalidacionContenedor()

            _Revalidaciones = New List(Of RevalidacionBl)

            _auxTagWatcher.Status = TagWatcher.TypeStatus.Ok

            'Este método obtiene la información de la referecia que se pretende actualizar sus contenedores

            _auxTagWatcher = ObtenerInformacionRevalidacionKromBaseRevalidado()

            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

            Else

                'Actualizamos el tipo de carga de la referencia
                _auxTagWatcher = ActualizarTipoCargaRevalidacionSysExpert()

                If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                Else

                    'Verificamos que ya tenga registro en revalidación
                    _auxTagWatcher = VerificarRevalidacionSysExpert()

                    If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                        _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                    Else

                        'este metodo elimina los contenedores/marcas de una ree
                        _auxTagWatcher = EliminarContenedoresMarcas()

                        If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                            _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                        Else

                            'Este método contienen todo para insertar insertar/actualizar a revalidación por contenedor
                            _auxTagWatcher = iniciarProcesoRevalidacionBlContenedor()

                            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                            Else

                                '_sistema.GsDialogo("¡Información sincronizada con SysExpert!", TipoDialogo.Aviso)
                                ControlContenedores()

                            End If

                        End If

                    End If

                End If

            End If

        End Sub

        Private Function ActualizarTipoCargaRevalidacionSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Verificamos si ya existe un registro de revalidación de cada unas de las referencias de los viajes involucrados.
            For contador_ = 0 To _Revalidaciones.Count - 1

                _sql = "update [SysExpert].[dbo].[RevalidacionBL] set ClaveCargaTipo = '" & _Revalidaciones(contador_).ClaveCargaTipo & "' where idReferencia = " & _Revalidaciones(contador_).IdReferencia

                TagLocal_ = VerificarWhere(_sql)

                If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                    If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                        Return TagLocal_

                    End If

                Else

                    Return TagLocal_

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function ObtenerInformacionRevalidacionKromBaseRevalidado() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para revalidación y la guardamos en una lista.

            _sql = "select voaa.i_Cve_ReferenciaExterna, " &
                              "rci.i_Cve_RevalidacionImpo, " &
                              "case when rci.t_Cve_TipoCarga = 'P' then 'C' else rci.t_Cve_TipoCarga end t_Cve_TipoCarga " &
                       "from  VT003RevalidacionImpo as rci  " &
                       "inner join VT003OperacionesAgenciasAduanales as voaa on voaa.i_Cve_VinOperacionesAgenciasAduanales = rci.i_Cve_VinOperacionesAgenciasAduanales and voaa.i_Cve_DivisionMiEmpresa	= rci.i_Cve_DivisionMiEmpresa     " &
                       "where rci.i_Cve_Estatus in (2,3) and rci.i_Cve_RevalidacionImpo = " & _index

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9007)

                    Return TagLocal_

                Else

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _Revalidaciones

                            Dim Revalidacion_ = New RevalidacionBl(_ioperacionescatalogo)

                            Revalidacion_.IdReferencia = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ReferenciaExterna"))

                            Revalidacion_.i_Cve_RevalidacionImpoKB = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_RevalidacionImpo"))

                            Revalidacion_.ClaveCargaTipo = VerificarDBNull(cursorDataTable_(contador_)("t_Cve_TipoCarga"))

                            .Add(Revalidacion_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoPaisesSysexpert(ByVal t_ClavePaisKB_ As String) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'En caso de que no exista el Pais mandará un error y se acabará la transacción

            _sql = "SELECT IdPais FROM [SysExpert].[dbo].[Paises] WITH (NOLOCK) WHERE ClaveM3 = '" & t_ClavePaisKB_ & "'"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9009)

                    Return TagLocal_

                Else

                    TagLocal_.FlagReturned = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoEntidadesFederativasSysexpert(ByVal t_EntidadFederativa_ As String, ByVal t_idPais_ As String) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'En caso de que no exista la Entidad Federativa mandará un error y se acabará la transacción

            _sql = "SELECT IdEntidad FROM [SysExpert].[dbo].[EntidadesFederativas] WITH (NOLOCK) WHERE EntidadFederativa LIKE '" & t_EntidadFederativa_ & "' COLLATE Traditional_Spanish_CI_AI AND idPais = " & t_idPais_

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9014)

                    Return TagLocal_

                Else

                    TagLocal_.FlagReturned = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoIncotermsSysexpert(ByVal t_ClaveIncoterm_ As String) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'En caso de que no exista el Incoterm mandará un error y se acabará la transacción

            _sql = "SELECT IdIncoterm FROM [SysExpert].[dbo].[Incoterms] WITH (NOLOCK) WHERE Incoterm = '" & t_ClaveIncoterm_ & "'"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9010)

                    Return TagLocal_

                Else

                    TagLocal_.FlagReturned = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoMetodosValoracionSysexpert(ByVal t_ClaveValoracion_ As String) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'En caso de que no exista el Método de Valoración mandará un error y se acabará la transacción

            _sql = "SELECT IdValoracion FROM [SysExpert].[dbo].[MetodosValoracion] WITH (NOLOCK) WHERE ClaveValoracion = " & t_ClaveValoracion_

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9011)

                    Return TagLocal_

                Else

                    TagLocal_.FlagReturned = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoClientesSysexpert(ByVal t_ClaveCliente_ As String) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'En caso de que no exista el Cliente mandará un error y se acabará la transacción

            _sql = "SELECT IdCliente FROM [SysExpert].[dbo].[Clientes] WITH (NOLOCK) WHERE ClaveCliente = '" & t_ClaveCliente_ & "'"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9001)

                    Return TagLocal_

                Else

                    TagLocal_.FlagReturned = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            Return TagLocal_

        End Function

        Public Function AgregarProveedorSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            Dim TagLocalAux_

            _Proveedor = New Proveedores(_ioperacionescatalogo)

            'Obtenemos datos del proveedor en KromBase
            TagLocal_ = ObtenerInformacionProveedorKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                'Verificamos el catálogo de entidades federativas 
                TagLocal_ = VerificarCatalogoPaisesSysexpert(_Proveedor.ClavePaisKB)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    _Proveedor.IdPais = TagLocal_.FlagReturned

                    'Verificamos el catálogo de entidades federativas
                    TagLocalAux_ = VerificarCatalogoEntidadesFederativasSysexpert(_Proveedor.EntidadFederativaKB, _Proveedor.IdPais)

                    If Not TagLocalAux_.Status = TagWatcher.TypeStatus.Errors Then

                        _Proveedor.IdEntidad = TagLocalAux_.FlagReturned

                    End If

                    'If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    If _Proveedor.ClaveIncotermKB <> "" Then

                        'Verificamos el catálogo de incoterms
                        TagLocal_ = VerificarCatalogoIncotermsSysexpert(_Proveedor.ClaveIncotermKB)

                        If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                            _Proveedor.IdIncoterm = TagLocal_.FlagReturned

                        End If

                    End If

                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        If _Proveedor.ClaveValoracionKB <> "" Then

                            'Verificamos el catálogo de métodos de valoración
                            TagLocal_ = VerificarCatalogoMetodosValoracionSysexpert(_Proveedor.ClaveValoracionKB)

                            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                                _Proveedor.IdValoracion = TagLocal_.FlagReturned

                            End If

                        End If

                        If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                            TagLocal_ = InsercionProveedorSysExpert(_Proveedor)

                            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                                If _Proveedor.ClaveProveedorImportadoKB <> "" Then

                                    TagLocal_ = DeshabilitarProveedorImportadoSysExpert(_Proveedor)

                                End If

                                TagLocal_.FlagReturned = _Proveedor.IdProveedor

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

        End Function

        Public Function ActualizarProveedorSysExpert(ByVal i_Cve_Proveedor_ As Int32) As TagWatcher

            Dim TagLocal_ = New TagWatcher

            _Proveedor = New Proveedores(_ioperacionescatalogo)

            _Proveedor.IdProveedor = i_Cve_Proveedor_

            'Obtenemos datos del proveedor en KromBase
            TagLocal_ = ObtenerInformacionProveedorKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                If _Proveedor.ClaveIncotermKB <> "" Then

                    'Verificamos el catálogo de incoterms
                    TagLocal_ = VerificarCatalogoIncotermsSysexpert(_Proveedor.ClaveIncotermKB)

                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        _Proveedor.IdIncoterm = TagLocal_.FlagReturned

                    End If

                End If

                If _Proveedor.ClaveValoracionKB <> "" Then

                    'Verificamos el catálogo de métodos de valoración
                    TagLocal_ = VerificarCatalogoMetodosValoracionSysexpert(_Proveedor.ClaveValoracionKB)

                    If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        _Proveedor.IdValoracion = TagLocal_.FlagReturned

                    End If

                End If

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    TagLocal_ = ActualizacionProveedorSysExpert(_Proveedor)

                    Return TagLocal_

                Else

                    Return TagLocal_

                End If

            Else

                Return TagLocal_

            End If

        End Function

        Private Function ObtenerInformacionProveedorKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para el alta de proveedor

            _sql = "SELECT	Prov.i_Cve_Proveedor, " &
                    "Prov.t_NombreEmpresa, " &
                    "CASE WHEN Prov.t_TaxNumber IS NULL THEN Prov.t_RFC ELSE Prov.t_TaxNumber END t_TaxNumber, " &
                    "CASE WHEN Prov.t_RFC = 'XEXX010101000' THEN '' ELSE Prov.t_RFC END t_RFC, " &
                    "Prov.t_CURP, " &
                    "Dom.t_Calle, " &
                    "Dom.t_NumeroExterior, " &
                    "Dom.t_NumeroInterior, " &
                    "Dom.t_Colonia, " &
                    "Dom.t_CodigoPostal, " &
                    "Dom.t_Ciudad, " &
                    "Prov.t_CorreoElectronico, " &
                    "CONVERT(DATE,Prov.f_FechaAlta) AS f_FechaAlta, " &
                    "CONVERT(DATE,Prov.f_FechaModificacion) AS f_FechaModificacion, " &
                    "CASE Prov.i_Cve_Estatus " &
                        "WHEN 1 THEN 1 " &
                        "WHEN 2 THEN 1 " &
                        "WHEN 3 THEN 0 " &
                        "WHEN 4 THEN 0 " &
                    "END AS i_Habilitado, " &
                    "Prov.t_ApellidoPaterno, " &
                    "Prov.t_ApellidoMaterno, " &
                    "Dom.t_Municipio, " &
                    "Prov.t_Nombre, " &
                    "Prov.t_Telefono, " &
                    "Prov.t_Incoterm, " &
                    "MetVal.i_ClaveValoracion, " &
                    "Dom.t_Estado, " &
                    "Dom.t_Cve_Pais, " &
                    "Prov.i_Cve_ProveedorImportado, " &
                    "Prov.i_Cve_TipoIdentificadorCOVE " &
                    "FROM dbo.Vt003ProveedoresOperativos AS Prov " &
                    "INNER JOIN dbo.Vt000Domicilios AS Dom ON Dom.i_Cve_Domicilio = Prov.i_Cve_DomicilioActual " &
                    "LEFT OUTER JOIN dbo.Vt003MetodosValoracion AS MetVal ON MetVal.i_Cve_MetodoValoracion = Prov.i_Cve_MetodoValoracion " &
                    "WHERE Prov.i_Cve_Proveedor = " & _index

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9015)

                    Return TagLocal_

                Else

                    With _Proveedor

                        .RazonSocial = VerificarDBNull(cursorDataTable_(0)("t_NombreEmpresa"))

                        .TaxNumber = VerificarDBNull(cursorDataTable_(0)("t_TaxNumber"))

                        .RFC = VerificarDBNull(cursorDataTable_(0)("t_RFC"))

                        .CURP = VerificarDBNull(cursorDataTable_(0)("t_CURP"))

                        .Calle = VerificarDBNull(cursorDataTable_(0)("t_Calle"))

                        .NumeroExterior = VerificarDBNull(cursorDataTable_(0)("t_NumeroExterior"))

                        .NumeroInterior = VerificarDBNull(cursorDataTable_(0)("t_NumeroInterior"))

                        .Colonia = VerificarDBNull(cursorDataTable_(0)("t_Colonia"))

                        .CodigoPostal = VerificarDBNull(cursorDataTable_(0)("t_CodigoPostal"))

                        .Ciudad = VerificarDBNull(cursorDataTable_(0)("t_Ciudad"))

                        .Municipio = VerificarDBNull(cursorDataTable_(0)("t_Municipio"))

                        .Telefono = VerificarDBNull(cursorDataTable_(0)("t_Telefono"))

                        .CorreoElectronico = VerificarDBNull(cursorDataTable_(0)("t_CorreoElectronico"))

                        .FechaAlta = VerificarDBNull(cursorDataTable_(0)("f_FechaAlta"))

                        .FechaModificacion = VerificarDBNull(cursorDataTable_(0)("f_FechaModificacion"))

                        .Habilitado = VerificarDBNull(cursorDataTable_(0)("i_Habilitado"))

                        .ApellidoPaterno = VerificarDBNull(cursorDataTable_(0)("t_ApellidoPaterno"))

                        .ApellidoMaterno = VerificarDBNull(cursorDataTable_(0)("t_ApellidoMaterno"))

                        .Nombre = VerificarDBNull(cursorDataTable_(0)("t_Nombre"))

                        .ClaveIncotermKB = VerificarDBNull(cursorDataTable_(0)("t_Incoterm"))

                        .ClaveValoracionKB = VerificarDBNull(cursorDataTable_(0)("i_ClaveValoracion"))

                        .EntidadFederativaKB = VerificarDBNull(cursorDataTable_(0)("t_Estado"))

                        .ClavePaisKB = VerificarDBNull(cursorDataTable_(0)("t_Cve_Pais"))

                        .ClaveProveedorImportadoKB = VerificarDBNull(cursorDataTable_(0)("i_Cve_ProveedorImportado"))

                        .TipoIdentificadorCOVEKB = VerificarDBNull(cursorDataTable_(0)("i_Cve_TipoIdentificadorCOVE"))

                    End With

                End If

            End If

            Return TagLocal_

        End Function

        Private Function InsercionProveedorSysExpert(ByRef Proveedor_ As Proveedores) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción del proveedor

            Dim sql_ = Proveedor_.SentenciaInsert()

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9012)

            End If

            Return TagLocal_

        End Function

        Private Function DeshabilitarProveedorImportadoSysExpert(ByRef Proveedor_ As Proveedores) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            Dim sql_ As String = ""

            ' Deshabilitar proveedor importado
            If Proveedor_.ClaveProveedorImportadoKB <> "" Then

                sql_ = Proveedor_.SentenciaDeshabilitarImportado()

                TagLocal_ = VerificarWhere(sql_)

                If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

                    If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9013)

                    End If

                End If

            End If

            Return TagLocal_

        End Function

        Public Function DeshabilitarProveedorSysExpert(ByRef i_Cve_Proveedor_ As Int32) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _Proveedor = New Proveedores(_ioperacionescatalogo)

            _Proveedor.IdProveedor = i_Cve_Proveedor_

            Dim sql_ As String = ""

            ' Deshabilitar proveedor 
            If _Proveedor.IdProveedor > 0 Then

                sql_ = _Proveedor.SentenciaDeshabilitar()

                TagLocal_ = VerificarWhere(sql_)

                If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

                    If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9017)

                    End If

                End If

            End If

            Return TagLocal_

        End Function

        'Public Function HabilitarProveedorSysExpert(ByRef i_Cve_Proveedor_ As Int32) As TagWatcher

        '    Dim sistemaLocal_ = New Organismo

        '    Dim TagLocal_ = New TagWatcher

        '    _Proveedor = New Proveedores(_ioperacionescatalogo)

        '    _Proveedor.IdProveedor = i_Cve_Proveedor_

        '    Dim sql_ As String = ""

        '    ' Deshabilitar proveedor 
        '    If _Proveedor.IdProveedor > 0 Then

        '        sql_ = _Proveedor.SentenciaHabilitar()

        '        TagLocal_ = VerificarWhere(sql_)

        '        If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

        '            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

        '            If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

        '                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9018)

        '            End If

        '        End If

        '    End If

        '    Return TagLocal_

        'End Function

        Private Function ActualizacionProveedorSysExpert(ByRef Proveedor_ As Proveedores) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la actualización del proveedor

            Dim sql_ = Proveedor_.SentenciaUpdate()

            TagLocal_ = VerificarWhere(sql_)

            If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

                If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9016)

                End If

            End If

            Return TagLocal_

        End Function

        Private Function ObtenerInformacionVinculacionesClienteProveedorKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para registrar vinculación proveedores clientes

            _sql = "SELECT Vin.i_Cve_ProveedorClienteVinculado, " &
                   "Ext.i_Cve_Externa AS i_Cve_ProveedorExterno, " &
                   "Vin.i_Cve_ClienteExterno, " &
                   "Vin.i_ClaveVinculacion, " &
                   "(Vin.d_PorcentajeVinculacion * 100) AS d_PorcentajeVinculacion " &
                   "FROM dbo.Vt003ProveedoresClientesVinculados AS Vin WITH (NOLOCK) " &
                   "INNER JOIN dbo.Vin003ProveedoresDomicilios AS Dom WITH (NOLOCK) ON Dom.i_Cve_Proveedor = Vin.i_Cve_Proveedor AND Dom.i_Cve_Estatus = 1 AND Dom.i_Cve_Estado = 1 " &
                   "INNER JOIN dbo.Ext015ProveedoresDomicilios AS Ext WITH (NOLOCK) ON Ext.i_Cve_ProveedorDomicilio = Dom.i_Cve_ProveedorDomicilio  AND Ext.i_Cve_Sistema = Vin.i_Cve_Sistema AND Ext.i_Cve_Estado = 1 " &
                   "WHERE Vin.i_Cve_Proveedor = " & _index & " AND Vin.i_Cve_Sistema = 1 AND Vin.i_Cve_Estatus = 1 "
            '"INNER JOIN dbo.Ext015ProveedoresDomicilios AS Ext ON Ext.i_Cve_ProveedorDomicilio = Dom.i_Cve_ProveedorDomicilio  AND Ext.i_Cve_Sistema = Vin.i_Cve_Sistema AND Ext.i_Cve_Estatus = 1 AND Ext.i_Cve_Estado = 1 " &
            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count > 0 Then

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _VinculacionesClienteProveedor

                            Dim vinculacion_ = New VinculacionClienteProveedor(_ioperacionescatalogo)

                            With vinculacion_

                                .IdProveedorClienteVinculadoKB = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ProveedorClienteVinculado"))

                                .IdProveedor = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ProveedorExterno"))

                                .IdCliente = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ClienteExterno"))

                                .ClaveVinculacionKB = VerificarDBNull(cursorDataTable_(contador_)("i_ClaveVinculacion"))

                                .Porcentaje = VerificarDBNull(cursorDataTable_(contador_)("d_PorcentajeVinculacion"))

                            End With

                            .Add(vinculacion_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

        Public Function CompararVinculacionesClienteProveedor(ByRef i_Cve_Proveedor_ As Int32) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Ejecuta consulta para comparar si existen diferencias entre las vinculaciones que existen activas en KB y las que están registradas en SysExpert

            _sql = "( SELECT Ext.i_Cve_Externa AS i_Cve_ProveedorExterno, " &
                        "Vin.i_Cve_ClienteExterno, " &
                        "Vin.i_ClaveVinculacion, " &
                        "(Vin.d_PorcentajeVinculacion*100) AS d_PorcentajeVinculacion " &
                    "FROM Solium.dbo.Vt003ProveedoresClientesVinculados AS Vin WITH (NOLOCK) " &
                    "INNER JOIN Solium.dbo.Vin003ProveedoresDomicilios AS Dom WITH (NOLOCK) ON Dom.i_Cve_Proveedor = Vin.i_Cve_Proveedor AND Dom.i_Cve_Estatus = 1 AND Dom.i_Cve_Estado = 1 " &
                    "INNER JOIN Solium.dbo.Ext015ProveedoresDomicilios AS Ext WITH (NOLOCK) ON Ext.i_Cve_ProveedorDomicilio = Dom.i_Cve_ProveedorDomicilio  AND Ext.i_Cve_Sistema = Vin.i_Cve_Sistema AND Ext.i_Cve_Estatus = 1 AND Ext.i_Cve_Estado = 1 " &
                    "WHERE Vin.i_Cve_Proveedor = " & _index & " AND Vin.i_Cve_Sistema = 1 AND Vin.i_Cve_Estatus = 1 " &
                    "EXCEPT " &
                    "SELECT Vin.idProveedor AS i_Cve_ProveedorExterno, " &
                        "Vin.idCliente AS i_Cve_ClienteExterno, " &
                        "TipVin.Clave AS i_ClaveVinculacion, " &
                        "Vin.Porcentaje AS d_PorcentajeVinculacion " &
                    "FROM SysExpert.dbo.VinculacionCliPro AS Vin WITH (NOLOCK) " &
                    "INNER JOIN SysExpert.dbo.TiposVinculacion AS TipVin WITH (NOLOCK) ON TipVin.idVinculacion = Vin.idVinculacion " &
                    "WHERE Vin.idProveedor = " & i_Cve_Proveedor_ & " ) " &
                    "UNION  ALL " &
                    "( SELECT Vin.idProveedor AS i_Cve_ProveedorExterno, " &
                        "Vin.idCliente AS i_Cve_ClienteExterno, " &
                        "TipVin.Clave AS i_ClaveVinculacion, " &
                        "Vin.Porcentaje AS d_PorcentajeVinculacion " &
                    "FROM SysExpert.dbo.VinculacionCliPro AS Vin WITH (NOLOCK) " &
                    "INNER JOIN SysExpert.dbo.TiposVinculacion AS TipVin WITH (NOLOCK) ON TipVin.idVinculacion = Vin.idVinculacion " &
                    "WHERE Vin.idProveedor = " & i_Cve_Proveedor_ & " " &
                    "EXCEPT " &
                    "SELECT Ext.i_Cve_Externa AS i_Cve_ProveedorExterno, " &
                        "Vin.i_Cve_ClienteExterno, " &
                        "Vin.i_ClaveVinculacion, " &
                        "(Vin.d_PorcentajeVinculacion*100) AS d_PorcentajeVinculacion " &
                    "FROM Solium.dbo.Vt003ProveedoresClientesVinculados AS Vin WITH (NOLOCK) " &
                    "INNER JOIN Solium.dbo.Vin003ProveedoresDomicilios AS Dom WITH (NOLOCK) ON Dom.i_Cve_Proveedor = Vin.i_Cve_Proveedor AND Dom.i_Cve_Estatus = 1 AND Dom.i_Cve_Estado = 1 " &
                    "INNER JOIN Solium.dbo.Ext015ProveedoresDomicilios AS Ext WITH (NOLOCK) ON Ext.i_Cve_ProveedorDomicilio = Dom.i_Cve_ProveedorDomicilio  AND Ext.i_Cve_Sistema = Vin.i_Cve_Sistema AND Ext.i_Cve_Estatus = 1 AND Ext.i_Cve_Estado = 1 " &
                    "WHERE Vin.i_Cve_Proveedor = " & _index & " AND Vin.i_Cve_Sistema = 1 AND Vin.i_Cve_Estatus = 1 ) "

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then  'Si no encontro diferencias

                    TagLocal_.FlagReturned = False

                Else

                    TagLocal_.FlagReturned = True ' Si encontró diferencias

                End If

            End If

            Return TagLocal_

        End Function

        Public Function EliminarVinculacionesClienteProveedorSysExpert(ByRef i_Cve_Proveedor_ As Int32) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _VinculacionClienteProveedor = New VinculacionClienteProveedor(_ioperacionescatalogo)

            Dim sql_ As String = ""

            sql_ = _VinculacionClienteProveedor.SentenciaDelete() & i_Cve_Proveedor_

            TagLocal_ = VerificarWhere(sql_)

            If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

                If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9020)

                End If

            End If

            Return TagLocal_

        End Function

        Public Function AgregarVinculacionesClienteProveedorSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            Dim auxTagLocal_ = New TagWatcher

            _VinculacionesClienteProveedor = New List(Of VinculacionClienteProveedor)()

            'Obtener datos de las vinculaciones en KromBase
            TagLocal_ = ObtenerInformacionVinculacionesClienteProveedorKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                'Recorre listado de vinculaciones pendientes de insertar
                For contador_ = 0 To _VinculacionesClienteProveedor.Count - 1

                    auxTagLocal_ = VerificarCatalogoTiposVinculacionSysexpert(_VinculacionesClienteProveedor(contador_).ClaveVinculacionKB)

                    If Not auxTagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        _VinculacionesClienteProveedor(contador_).IdVinculacion = auxTagLocal_.FlagReturned

                        'Se procedé con la inserción en caso de marcar error se terminará la transacción
                        TagLocal_ = InsercionVinculacionClienteProveedorSysExpert(_VinculacionesClienteProveedor(contador_))

                        If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                            Exit For

                        End If

                    Else

                        Return TagLocal_

                    End If

                Next contador_

                Return TagLocal_

            Else

                Return TagLocal_

            End If

        End Function

        Private Function InsercionVinculacionClienteProveedorSysExpert(ByRef Vinculacion_ As VinculacionClienteProveedor) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede a la inserción de la vinculación cliente proveedor
            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(Vinculacion_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function VerificarCatalogoTiposVinculacionSysexpert(ByVal t_ClaveVinculacion_ As String) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'En caso de que no exista la clave de vinculación mandará un error y se acabará la transacción
            _sql = "SELECT idVinculacion FROM [SysExpert].[dbo].[TiposVinculacion] WITH (NOLOCK) WHERE Clave =  '" & t_ClaveVinculacion_ & "'"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9010)

                    Return TagLocal_

                Else

                    TagLocal_.FlagReturned = VerificarDBNull(cursorDataTable_(0)(0))

                End If

            End If

            Return TagLocal_

        End Function

        Private Function ObtenerInformacionPartesKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para el alta de partes

            _sql = "SELECT	Pro.i_Cve_Producto, " &
                    "Pro.t_CodigoProducto, " &
                    "Pro.t_DescripcionGeneralEsp AS t_Descripcion, " &
                    "Pro.t_Fraccion AS t_CveFraccionKB, " &
                    "ExtFracc.i_Cve_Externa AS i_Cve_ExternaFraccion, " &
                    "Pro.t_CveUnidadComercial AS t_CveUnidadKB, " &
                    "ExtUni.i_Cve_Externa AS i_Cve_ExternaUnidad, " &
                    "Pro.t_CveClasificacion AS t_CveTipoKB, " &
                    "ExtCla.i_Cve_Externa AS i_Cve_ExternaTipo, " &
                    "CASE Pro.i_Cve_Estatus " &
                        "WHEN 1 THEN 'Preliminar' " &
                        "WHEN 2 THEN 'Autorizado' " &
                        "WHEN 3 THEN 'Autorizado' " &
                    "END AS t_Validado, " &
                    "Pro.t_Observaciones, " &
                    "ExtFracc.i_TipoOperacion AS i_TipoImpExp, " &
                    "CASE Pro.i_Cve_Estatus " &
                        "WHEN 1 THEN 1 " &
                        "WHEN 2 THEN 1 " &
                        "WHEN 3 THEN 0 " &
                    "END AS i_Habilitado " &
                    "FROM dbo.Vt003ProductosOperativos AS Pro " &
                    "LEFT OUTER JOIN dbo.Vt015ExtFraccionesArancelarias AS ExtFracc ON ExtFracc.i_Cve_FraccionArancelaria = Pro.i_Cve_FraccionArancelaria AND ExtFracc.i_Cve_Sistema = 1 AND ExtFracc.i_Cve_Estatus = 1 " &
                    "LEFT OUTER JOIN dbo.Vt015ExtUnidadesMedida AS ExtUni ON ExtUni.i_Cve_UnidadMedida = Pro.i_Cve_UnidadComercial AND ExtUni.i_Cve_Sistema = 1 AND ExtUni.i_Cve_Estatus = 1 " &
                    "LEFT OUTER JOIN dbo.Vt015ExtClasificacionProductos AS ExtCla ON ExtCla.i_Cve_ClasificacionProducto = Pro.i_Cve_ClasificacionProducto AND ExtCla.i_Cve_Sistema = 1 AND ExtCla.i_Cve_Estatus = 1 " &
                    "WHERE Pro.i_Cve_Producto = " & _index

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9015)

                    Return TagLocal_

                Else

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _Partes

                            Dim Parte_ = New Partes(_ioperacionescatalogo)

                            With Parte_

                                .Clave = VerificarDBNull(cursorDataTable_(contador_)("t_CodigoProducto"))

                                .Pt_Descripcion = VerificarDBNull(cursorDataTable_(contador_)("t_Descripcion"))

                                .IdFraccion = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ExternaFraccion"))

                                .IdUnidad = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ExternaUnidad"))

                                .IdTipo = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ExternaTipo"))

                                .Validado = VerificarDBNull(cursorDataTable_(contador_)("t_Validado"))

                                .DescripcionAdicional = VerificarDBNull(cursorDataTable_(contador_)("t_Observaciones"))

                                .TipoImpExp = VerificarDBNull(cursorDataTable_(contador_)("i_TipoImpExp"))

                                .Habilitado = VerificarDBNull(cursorDataTable_(contador_)("i_Habilitado"))

                                .DescripcionCOVE = VerificarDBNull(cursorDataTable_(contador_)("t_Descripcion"))

                                .ClaveFraccionKB = VerificarDBNull(cursorDataTable_(contador_)("t_CveFraccionKB"))

                                .ClaveUnidadKB = VerificarDBNull(cursorDataTable_(contador_)("t_CveUnidadKB"))

                                .ClaveTipoKB = VerificarDBNull(cursorDataTable_(contador_)("t_CveTipoKB"))

                            End With

                            .Add(Parte_)

                        End With

                    Next contador_

                End If

            End If

            Return TagLocal_

        End Function

        Public Function AgregarPartesSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            _Partes = New List(Of Partes)

            'Obtenemos datos de la parte en KromBase
            TagLocal_ = ObtenerInformacionPartesKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                For contador_ = 0 To _Partes.Count - 1

                    TagLocal_ = InsercionParteSysExpert(_Partes(contador_))

                    If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        Return TagLocal_

                    End If

                Next

                TagLocal_.ObjectReturned = _Partes

                Return TagLocal_

            Else

                Return TagLocal_

            End If

        End Function

        Private Function InsercionParteSysExpert(ByRef Partes_ As Partes) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción de la parte

            Dim sql_ = Partes_.SentenciaInsert()

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9021)

            End If

            Return TagLocal_

        End Function

        Public Function ActualizarPartesSysExpert(ByVal arr_ClavesExternas_ As Int32()) As TagWatcher

            Dim TagLocal_ = New TagWatcher

            _Partes = New List(Of Partes)

            'Obtenemos datos de la parte en KromBase
            TagLocal_ = ObtenerInformacionPartesKromBase()

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                For contador_ = 0 To _Partes.Count - 1

                    _Partes(contador_).IdParte = arr_ClavesExternas_(_Partes(contador_).TipoImpExp - 1)

                    TagLocal_ = ActualizacionParteSysExpert(_Partes(contador_))

                    If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                        Return TagLocal_

                    End If

                Next

                TagLocal_.ObjectReturned = _Partes

                Return TagLocal_

            Else

                Return TagLocal_

            End If

        End Function

        Private Function ActualizacionParteSysExpert(ByRef Partes_ As Partes) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la actualización de la parte

            Dim sql_ = Partes_.SentenciaUpdate()

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9022)

            End If

            Return TagLocal_

        End Function

        Public Sub InsertarSituacionTraficoSysExpert(ByVal dato_ As String, ByVal observaciones_ As String)

            _TraficoVal = New List(Of Trafico)

            _auxTagWatcher.Status = TagWatcher.TypeStatus.Ok

            _auxTagWatcher = ObtenerInformacionReferencia()

            Dim TagLocal_ = New TagWatcher

            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

            Else

                For contador_ = 0 To _TraficoVal.Count - 1

                    Dim IdReferencia_ As String = _TraficoVal(contador_).idReferencia

                    _Trafico = New Trafico(_ioperacionescatalogo)

                    _Trafico.PuedeInsertar = True

                    _Trafico.idSituacion = _TraficoVal(contador_).idSituacion

                    _Trafico.idReferencia = IdReferencia_

                    _Trafico.FechaMovimiento = Date.Now.Date

                    _Trafico.idUsuario = 2 '_ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ClaveUsuario

                    _Trafico.Dato = dato_

                    _Trafico.ProcesadoWeb = 0

                    _Trafico.Observaciones = observaciones_

                    _Trafico.NumeroReferencia = _TraficoVal(contador_).NumeroReferencia

                    '_Trafico.idAgencia = _TraficoVal(contador_).idAgencia

                    _Trafico.HoraMovimiento = AgregaCero(DateTime.Now.Hour) & ":" & AgregaCero(DateTime.Now.Minute) & ":" & AgregaCero(DateTime.Now.Second)

                    _Trafico.FechaAlta = Date.Now.Date

                    If _Trafico.PuedeInsertar Then

                        TagLocal_ = InsercionTraficoSysExpert(_Trafico)

                        If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                            _sistema.GsDialogo(TagLocal_.ErrorDescription, TipoDialogo.Aviso)

                        End If

                    End If

                Next

            End If

        End Sub

        ' Se creo una conexion esta función par auso del KBW, utiliza conexión libre
        Public Sub InsertarSituacionTraficoSysExpertConexionLibre(ByVal dato_ As String, ByVal observaciones_ As String)

            _TraficoVal = New List(Of Trafico)

            _auxTagWatcher.Status = TagWatcher.TypeStatus.Ok

            _auxTagWatcher = ObtenerInformacionReferenciaConexionLibre()

            Dim TagLocal_ = New TagWatcher

            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

            Else

                For contador_ = 0 To _TraficoVal.Count - 1

                    Dim IdReferencia_ As String = _TraficoVal(contador_).idReferencia

                    _Trafico = New Trafico(_ioperacionescatalogo)

                    _Trafico.PuedeInsertar = True

                    _Trafico.idSituacion = _TraficoVal(contador_).idSituacion

                    _Trafico.idReferencia = IdReferencia_

                    _Trafico.FechaMovimiento = Date.Now.Date

                    _Trafico.idUsuario = 2 '_ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ClaveUsuario

                    _Trafico.Dato = dato_

                    _Trafico.ProcesadoWeb = 0

                    _Trafico.Observaciones = observaciones_

                    _Trafico.NumeroReferencia = _TraficoVal(contador_).NumeroReferencia

                    '_Trafico.idAgencia = _TraficoVal(contador_).idAgencia

                    _Trafico.HoraMovimiento = AgregaCero(DateTime.Now.Hour) & ":" & AgregaCero(DateTime.Now.Minute) & ":" & AgregaCero(DateTime.Now.Second)

                    _Trafico.FechaAlta = Date.Now.Date

                    If _Trafico.PuedeInsertar Then

                        TagLocal_ = InsercionTraficoSysExpert(_Trafico)

                        If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                            _sistema.GsDialogo(TagLocal_.ErrorDescription, TipoDialogo.Aviso)

                        End If

                    End If

                Next

            End If

        End Sub

        Private Function ObtenerInformacionReferencia() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para revalidación y la guardamos en una lista.

            _sql = "select pg.idReferencia, pg.idAgencia, pg.NumeroReferencia, st.idSituacion " &
                    "from Sysexpert..PED_GRALES pg WITH (NOLOCK) " &
                    "LEFT OUTER JOIN Sysexpert..SituacionesTrafico st WITH (NOLOCK) ON pg.idAgencia=st.idAgencia and st.ClaveSituacion=225 " &
                    "where pg.NumeroReferencia='" & _referencia & "'"

            'MsgBox(_sql)

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9007)

                    Return TagLocal_

                Else

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _TraficoVal

                            Dim Validacion_ = New Trafico(_ioperacionescatalogo)

                            Validacion_.idReferencia = VerificarDBNull(cursorDataTable_(contador_)("idReferencia"))

                            Validacion_.idAgencia = VerificarDBNull(cursorDataTable_(contador_)("idAgencia"))

                            Validacion_.NumeroReferencia = VerificarDBNull(cursorDataTable_(contador_)("NumeroReferencia"))

                            If (VerificarDBNull(cursorDataTable_(contador_)("idSituacion")) = Nothing) Then

                                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9023)

                                Return TagLocal_

                            Else
                                Validacion_.idSituacion = VerificarDBNull(cursorDataTable_(contador_)("idSituacion"))
                            End If

                            .Add(Validacion_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

        ' Se creo una conexion esta función par auso del KBW, utiliza conexión libre
        Private Function ObtenerInformacionReferenciaConexionLibre() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Obtenemos la información de KromBase para revalidación y la guardamos en una lista.

            _sql = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                "SELECT pg.idReferencia, pg.idAgencia, pg.NumeroReferencia, st.idSituacion " &
                    "FROM Sysexpert..PED_GRALES pg WITH (NOLOCK) " &
                    "LEFT OUTER JOIN Sysexpert..SituacionesTrafico st WITH (NOLOCK) ON pg.idAgencia=st.idAgencia and st.ClaveSituacion=225 " &
                    "WHERE pg.NumeroReferencia='" & _referencia & "'"


            'TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            TagLocal_ = sistemaLocal_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9007)

                    Return TagLocal_

                Else

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _TraficoVal

                            Dim Validacion_ = New Trafico(_ioperacionescatalogo)

                            Validacion_.idReferencia = VerificarDBNull(cursorDataTable_(contador_)("idReferencia"))

                            Validacion_.idAgencia = VerificarDBNull(cursorDataTable_(contador_)("idAgencia"))

                            Validacion_.NumeroReferencia = VerificarDBNull(cursorDataTable_(contador_)("NumeroReferencia"))

                            If (VerificarDBNull(cursorDataTable_(contador_)("idSituacion")) = Nothing) Then

                                TagLocal_.Status = TagWatcher.TypeStatus.Errors

                                TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9023)

                                Return TagLocal_

                            Else
                                Validacion_.idSituacion = VerificarDBNull(cursorDataTable_(contador_)("idSituacion"))
                            End If

                            .Add(Validacion_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

#Region "ControlContenedores y ControlContenedoresDetalle"

        Sub ControlContenedores()

            _ControlContenedores = New List(Of ControlContenedores)

            _auxTagWatcher.Status = TagWatcher.TypeStatus.Ok

            _auxTagWatcher = ObtenerInformacionControlContenedoresKromBase()

            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

            Else

                _auxTagWatcher = VerificarControlContenedoresSysExpert()

                If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                Else

                    _auxTagWatcher = RealizarOperacionControlContenedoresSysExpert()

                    If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                        _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                    Else

                        ControlContenedoresDetalle()

                    End If

                End If

            End If

        End Sub

        Private Function ObtenerInformacionControlContenedoresKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se Obtiene la Referencia externa del ControlContenedores
            _sql = "    SELECT " &
                "           opeAdu.i_Cve_ReferenciaExterna" &
                "       FROM " &
                "           Solium.dbo.vt003revalidacionimpo AS revImpo " &
                "           INNER JOIN solium.dbo.vt003operacionesagenciasaduanales AS opeAdu " &
                "           ON opeAdu.i_cve_vinoperacionesagenciasaduanales = revImpo.i_cve_vinoperacionesagenciasaduanales " &
                "           AND opeAdu.i_cve_divisionmiempresa = revImpo.i_cve_divisionmiempresa " &
                "       WHERE " &
                "           revImpo.i_Cve_Estatus in (2,3) AND revImpo.i_cve_revalidacionimpo =  " & _index

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count = 0 Then

                    TagLocal_.Status = TagWatcher.TypeStatus.Errors

                    TagLocal_.SetError(TagWatcher.ErrorTypes.C6_003_9007)

                    Return TagLocal_

                Else

                    For contador_ = 0 To cursorDataTable_.Rows.Count - 1

                        With _ControlContenedores

                            Dim ControlContenedores_ = New ControlContenedores(_ioperacionescatalogo)

                            ControlContenedores_.IdReferencia = VerificarDBNull(cursorDataTable_(contador_)("i_Cve_ReferenciaExterna"))

                            .Add(ControlContenedores_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarControlContenedoresSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            For contador_ = 0 To _ControlContenedores.Count - 1

                _sql = "Select IdControlContenedor from [SysExpert].[dbo].[ControlContenedores] where idReferencia = " & _ControlContenedores(contador_).IdReferencia

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        _ControlContenedores(contador_).PuedeInsertar = True

                        _ControlContenedores(contador_).PuedeActualizar = False

                    Else

                        _ControlContenedores(contador_).IdControlContenedor = VerificarDBNull(cursorDataTable_(0)(0))

                        _ControlContenedores(contador_).PuedeInsertar = False

                        _ControlContenedores(contador_).PuedeActualizar = True

                    End If

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function RealizarOperacionControlContenedoresSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            'Se recorre toda la lista de Control de Contenedores y se va realizando la operación correspondiente en caso de marcar error se termina  la transsación
            For contador_ = 0 To _ControlContenedores.Count - 1

                If _ControlContenedores(contador_).PuedeInsertar Then

                    'Se procedé con la inserción en caso de marcar error se terminará la transacción
                    TagLocal_ = InsercionControlContenedoresSysExpert(_ControlContenedores(contador_))

                Else

                    'Se procedé con la actualización en caso de marcar error se terminará la transacción
                    TagLocal_ = ActualizacionControlContenedoresSysExpert(_ControlContenedores(contador_))

                End If

                If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    Return TagLocal_

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function InsercionControlContenedoresSysExpert(ByRef ControlContenedores_ As ControlContenedores) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción del Control de Contenedores

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(ControlContenedores_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function ActualizacionControlContenedoresSysExpert(ByRef ControlContenedores_ As ControlContenedores) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la actualización del Control de Contenedores

            Dim sql_ = ControlContenedores_.SentenciaUpdate()

            TagLocal_ = VerificarWhere(sql_)

            If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            End If

            Return TagLocal_

        End Function

        Sub ControlContenedoresDetalle()

            _ControlContenedoresDetalle = New List(Of ControlContenedoresDetalle)

            _auxTagWatcher.Status = TagWatcher.TypeStatus.Ok

            _auxTagWatcher = ObtenerInformacionControlContenedoresDetalleKromBase()

            If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

            Else

                _auxTagWatcher = VerificarControlContenedoresDetalleSysExpert()

                If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                Else

                    _auxTagWatcher = RealizarOperacionControlContenedoresDetalleSysExpert()

                    If _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                        _sistema.GsDialogo(_auxTagWatcher.ErrorDescription, TipoDialogo.Aviso)

                    Else

                        _sistema.GsDialogo("¡Información sincronizada con SysExpert!", TipoDialogo.Aviso)

                    End If

                End If

            End If

        End Sub

        Private Function ObtenerInformacionControlContenedoresDetalleKromBase() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            _ControlContenedoresDetalle = New List(Of ControlContenedoresDetalle)

            _sql = "    SELECT " &
                "           ctrImpo.t_NumeroContenedor" &
                "           ,ctrImpo.f_EIR " &
                "           ,ctrImpo.f_Vacio " &
                "       FROM " &
                "           Solium.dbo.vt003revalidacionimpo AS revImpo " &
                "           INNER JOIN Solium.dbo.VT003ContenedoresImpo AS ctrImpo " &
                "           ON ctrImpo.i_Cve_RevalidacionImpo = revImpo.i_Cve_RevalidacionImpo " &
                "           INNER JOIN solium.dbo.vt003operacionesagenciasaduanales AS opeAdu " &
                "           ON opeAdu.i_cve_vinoperacionesagenciasaduanales = revImpo.i_cve_vinoperacionesagenciasaduanales " &
                "           AND opeAdu.i_cve_divisionmiempresa = revImpo.i_cve_divisionmiempresa " &
                "       WHERE " &
                "           revImpo.i_Cve_Estatus in (2,3) AND opeAdu.i_Cve_ReferenciaExterna = " & _ControlContenedores(0).IdReferencia & " " &
                "       ORDER BY ctrImpo.i_Cve_ContenedoresImpo desc"

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

            If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                Return TagLocal_

            Else

                Dim cursorDataTable_ As New DataTable

                cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                If cursorDataTable_.Rows.Count > 0 Then

                    For Each fila_ As DataRow In cursorDataTable_.Rows

                        With _ControlContenedoresDetalle

                            Dim ControlContenedoresDetalle_ = New ControlContenedoresDetalle(_ioperacionescatalogo)

                            'ControlContenedoresDetalle_.IdControlContenedorDetalle = _Revalidaciones(contador_).IdRevalidacionBL

                            ControlContenedoresDetalle_.IdControlContenedor = _ControlContenedores(0).IdControlContenedor

                            For counter_ As Integer = 0 To _RevalidacionesBlContenedor.Count

                                If (_RevalidacionesBlContenedor(counter_).NumeroContenedor = VerificarDBNull(fila_.Item("t_NumeroContenedor")).ToString) Then

                                    ControlContenedoresDetalle_.IdRevalidacionBLContenedor = _RevalidacionesBlContenedor(counter_).IdRevalidacionBLContenedor

                                    Exit For

                                Else

                                    Continue For

                                End If

                            Next counter_

                            'ControlContenedoresDetalle_.LLenoOVacio = VerificarDBNull(DBNull.Value)

                            'ControlContenedoresDetalle_.FechaLiberacion = VerificarDBNull(DBNull.Value)

                            ControlContenedoresDetalle_.FechaEntrega = VerificarDBNull(fila_.Item("f_EIR"))

                            ControlContenedoresDetalle_.FechaCartaVacio = VerificarDBNull(fila_.Item("f_Vacio"))

                            'ControlContenedoresDetalle_.FechaDesconsolidacion = VerificarDBNull(DBNull.Value)

                            'ControlContenedoresDetalle_.DesconsolidacionStatus = VerificarDBNull(DBNull.Value)

                            'ControlContenedoresDetalle_.Imprimir = VerificarDBNull(DBNull.Value)

                            .Add(ControlContenedoresDetalle_)

                        End With

                    Next

                End If

            End If

            Return TagLocal_

        End Function

        Private Function VerificarControlContenedoresDetalleSysExpert() As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            For contador_ = 0 To _ControlContenedoresDetalle.Count - 1

                _sql = "SELECT  ccd.IdControlContenedorDetalle " &
                        "FROM   SysExpert.dbo.ControlContenedoresDetalle ccd " &
                        "INNER JOIN SysExpert.dbo.ControlContenedores cc ON ccd.IdControlContenedor = cc.IdControlContenedor " &
                        "INNER JOIN SysExpert.dbo.RevalidacionBLContenedor rblc ON ccd.IdRevalidacionBLContenedor = rblc.IdRevalidacionBLContenedor " &
                        "WHERE  ccd.IdControlContenedor = " & _ControlContenedoresDetalle(contador_).IdControlContenedor & " AND rblc.IdRevalidacionBLContenedor = " & _ControlContenedoresDetalle(contador_).IdRevalidacionBLContenedor

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(_sql)

                If Not TagLocal_.Status = TagWatcher.TypeStatus.Ok Then

                    Return TagLocal_

                Else

                    Dim cursorDataTable_ As New DataTable

                    cursorDataTable_ = DirectCast(TagLocal_.ObjectReturned, DataTable)

                    If cursorDataTable_.Rows.Count = 0 Then

                        _ControlContenedoresDetalle(contador_).PuedeInsertar = True

                        _ControlContenedoresDetalle(contador_).PuedeActualizar = False

                    Else

                        _ControlContenedoresDetalle(contador_).IdControlContenedorDetalle = VerificarDBNull(cursorDataTable_(0)(0))

                        _ControlContenedoresDetalle(contador_).PuedeInsertar = False

                        _ControlContenedoresDetalle(contador_).PuedeActualizar = True

                    End If

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function RealizarOperacionControlContenedoresDetalleSysExpert() As TagWatcher

            Dim TagLocal_ = New TagWatcher

            'Se recorre toda la lista de contenedores y se va realizando la operación correspondiente en caso de marcar error se termina  la transsación

            For contador_ = 0 To _ControlContenedoresDetalle.Count - 1

                If _ControlContenedoresDetalle(contador_).PuedeInsertar Then

                    'Se procede con la inserción en caso de marcar error se terminará la transacción
                    TagLocal_ = InsercionControlContenedoresDetalleSysExpert(_ControlContenedoresDetalle(contador_))

                Else

                    'Se procede con la actualización en caso de marcar error se terminará la transacción
                    TagLocal_ = ActualizacionControlContenedoresDetalleSysExpert(_ControlContenedoresDetalle(contador_))

                End If

                If TagLocal_.Status = TagWatcher.TypeStatus.Errors Then

                    Return TagLocal_

                End If

            Next contador_

            Return TagLocal_

        End Function

        Private Function InsercionControlContenedoresDetalleSysExpert(ByRef ControlContenedoresDetalle_ As ControlContenedoresDetalle) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la inserción del ControlContenedoresDetalle            

            TagLocal_ = sistemaLocal_.ComandosSingletonSQL(ControlContenedoresDetalle_.SentenciaInsert())

            Return TagLocal_

        End Function

        Private Function ActualizacionControlContenedoresDetalleSysExpert(ByRef ControlContenedoresDetalle_ As ControlContenedoresDetalle) As TagWatcher

            Dim sistemaLocal_ = New Organismo

            Dim TagLocal_ = New TagWatcher

            'Se procede con la actualización del ControlContenedoresDetalle

            Dim sql_ = ControlContenedoresDetalle_.SentenciaUpdate()

            TagLocal_ = VerificarWhere(sql_)

            If Not _auxTagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                TagLocal_ = sistemaLocal_.ComandosSingletonSQL(sql_)

            End If

            Return TagLocal_

        End Function



#End Region

#End Region

#Region "Funciones"

        Function AgregaCero(ByVal numero As String)

            If (Convert.ToInt32(numero) < 10) Then

                numero = "0" & numero

            End If

            Return numero

        End Function

#End Region

    End Class

End Namespace