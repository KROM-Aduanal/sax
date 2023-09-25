Imports System.Text
Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.monitoreo
Imports gsol.basededatos


Namespace gsol.krom

    Public Class AdaptadorDatos
        Implements IAdaptadorDatos

#Region "Atributos"

        Private _exportarResultadosAutomaticamente As IAdaptadorDatos.TiposFormatoSalida

        Private _activarBitacoras As Boolean

        Private _activarReplicacion As Boolean

        Private _tiempoTotalTranscurrido As Int32

        Private _espacioTrabajo As IEspacioTrabajo

        Private _procesamientoAsincrono As Boolean

        Private _status As TagWatcher

        'Resultado

        Private _pilaDatos As List(Of IEntidadDatos)

        Private _entidadTabla As DataTable

        Private _limiteRegistros As Int32

        Private _iOperaciones As IOperacionesCatalogo

        Private _modalidadPresentacion As IEnlaceDatos.ModalidadPresentacionEncabezados

        Private _limiteRegistrosPresentacion As Int32

        Private _filtrosAvanzados As String

        'MongoBD

        'PBM12

#End Region

#Region "Constructores"

        Sub New()

            _exportarResultadosAutomaticamente = IAdaptadorDatos.TiposFormatoSalida.ObjetoAutomatico

            _activarBitacoras = True

            _activarReplicacion = False

            _tiempoTotalTranscurrido = 0

            _procesamientoAsincrono = False

            _pilaDatos = New List(Of IEntidadDatos)

            _entidadTabla = New DataTable

            _status = New TagWatcher

            _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo


        End Sub

#End Region

#Region "Propiedades"


        Public Property ModalidadPresentacion As IEnlaceDatos.ModalidadPresentacionEncabezados _
            Implements IAdaptadorDatos.ModalidadPresentacion

            Get

                Return _modalidadPresentacion

            End Get

            Set(value As IEnlaceDatos.ModalidadPresentacionEncabezados)

                _modalidadPresentacion = value

            End Set

        End Property

        Public Property IOperaciones As IOperacionesCatalogo _
            Implements IAdaptadorDatos.IOperaciones

            Get

                Return _iOperaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _iOperaciones = value

            End Set

        End Property

        Public Property LimiteRegistros As Integer _
            Implements IAdaptadorDatos.LimiteRegistros

            Get

                Return _limiteRegistros

            End Get

            Set(value As Integer)

                _limiteRegistros = value

            End Set

        End Property

        Public ReadOnly Property Registros As List(Of IEntidadDatos) _
            Implements IAdaptadorDatos.Registros

            Get

                Return _pilaDatos

            End Get

        End Property

        Public ReadOnly Property Tabla As DataTable _
            Implements IAdaptadorDatos.Tabla

            Get

                Return _entidadTabla

            End Get

        End Property

        Public Property ActivarBitacoras As Boolean _
            Implements IAdaptadorDatos.ActivarBitacoras

            Get

                Return _activarBitacoras

            End Get

            Set(value As Boolean)

                _activarBitacoras = value

            End Set

        End Property

        Public Property ActivarReplicacion As Boolean _
        Implements IAdaptadorDatos.ActivarReplicacion

            Get

                Return _activarReplicacion

            End Get

            Set(value As Boolean)

                _activarReplicacion = value

            End Set

        End Property


        Public Property ExportarResultadosAutomaticamente As IAdaptadorDatos.TiposFormatoSalida _
            Implements IAdaptadorDatos.ExportarResultadosAutomaticamente

            Get

                Return _exportarResultadosAutomaticamente

            End Get

            Set(value As IAdaptadorDatos.TiposFormatoSalida)

                _exportarResultadosAutomaticamente = value

            End Set

        End Property

        Public ReadOnly Property TiempoTotalTranscurrido As Integer _
            Implements IAdaptadorDatos.TiempoTotalTranscurrido

            Get

                Return _tiempoTotalTranscurrido

            End Get

        End Property

        Public Property EspacioTrabajo As IEspacioTrabajo _
            Implements IAdaptadorDatos.EspacioTrabajo

            Get

                Return _espacioTrabajo

            End Get

            Set(value As IEspacioTrabajo)

                _espacioTrabajo = value

            End Set

        End Property

#End Region

#Region "Metodos"

        Private Function ConversionExternaCampo(ByVal campoInterno_ As Object,
                                                ByVal dimension_ As IEnlaceDatos.TiposDimension) As String
            Dim campoNombreExterno_ As String = Nothing

            Select Case dimension_

                Case IEnlaceDatos.TiposDimension.Referencias

                    Select Case campoInterno_

                        Case Referencia.AtributosDimensionReferencias.i_Cve_Referencia
                            campoNombreExterno_ = "idReferencia"

                        Case Referencia.AtributosDimensionReferencias.t_NumeroReferencia
                            campoNombreExterno_ = "NumeroReferencia"

                        Case Referencia.AtributosDimensionReferencias.i_Pago
                            campoNombreExterno_ = "Pago"

                        Case Referencia.AtributosDimensionReferencias.f_FechaEntrada
                            campoNombreExterno_ = "Entrada"

                        Case Referencia.AtributosDimensionReferencias.f_FechaDespacho
                            campoNombreExterno_ = "Despacho"

                        Case Referencia.AtributosDimensionReferencias.i_Patente
                            campoNombreExterno_ = "Patente"

                        Case Referencia.AtributosDimensionReferencias.i_ClaveAduana
                            campoNombreExterno_ = "ClaveAduana"

                        Case Referencia.AtributosDimensionReferencias.i_ValorAduana
                            campoNombreExterno_ = "ValorAduana"

                        Case Referencia.AtributosDimensionReferencias.t_Mercancia
                            campoNombreExterno_ = "Mercancia"

                            'Case Referencia.AtributosDimensionReferencias.t_
                            'campoNombreExterno_ = "Contenedores"
                            'Case Referencia.AtributosDimensionReferencias.
                            'campoNombreExterno_ = "PesoBruto"
                            'Case Referencia.AtributosDimensionReferencias.
                            '   campoNombreExterno_ = "PO"
                        Case Referencia.AtributosDimensionReferencias.i_Cve_TipoOperacion
                            campoNombreExterno_ = "TipoOperacion"
                            'Case Referencia.AtributosDimensionReferencias.i_Patente
                            '   campoNombreExterno_ = "BL"
                        Case Referencia.AtributosDimensionReferencias.i_Cve_MaestroOperaciones
                            campoNombreExterno_ = "Clave"
                        Case Else
                            campoNombreExterno_ = Nothing


                    End Select


                Case IEnlaceDatos.TiposDimension.PagosDeTercerosAddendas

                    Select Case campoInterno_

                        Case PagosTerceros.AtributosDimensionPagosTerceros.i_Cve_Factura
                            campoNombreExterno_ = "i_Cve_Factura"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.i_Cve_PagoHecho
                            campoNombreExterno_ = "i_Cve_PagoHecho"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.i_Cve_DetallePagoHecho
                            campoNombreExterno_ = "i_Cve_DetallePagoHecho"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.f_FechaRegistro
                            campoNombreExterno_ = "f_FechaRegistro"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.i_Cve_Beneficiario
                            campoNombreExterno_ = "i_Cve_Beneficiario"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.t_NombreBeneficiario
                            campoNombreExterno_ = "t_NombreBeneficiario"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.i_Cve_MaestroOperaciones
                            campoNombreExterno_ = "i_Cve_MaestroOperaciones"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.i_Cve_DivisionMiEmpresa
                            campoNombreExterno_ = "i_Cve_DivisionMiEmpresa"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.t_Patente
                            campoNombreExterno_ = "t_Patente"

                        Case PagosTerceros.AtributosDimensionPagosTerceros.t_Referencia
                            campoNombreExterno_ = "t_Referencia"
                        Case Else
                            campoNombreExterno_ = Nothing

                    End Select

                Case Else
                    'NO IMPLEMENTADO, retorna el mismo campo recibido, supone que no hay conversión por atender

                    Return campoInterno_.ToString

            End Select

            Return campoNombreExterno_

        End Function

        Private Sub RenombraEncabezados(ByRef tabla_ As DataTable, ByVal estructuraRequerida_ As IEntidadDatos)

            For Each campo_ As CampoVirtual In estructuraRequerida_.Atributos

                For indice_ As Int32 = 0 To tabla_.Columns.Count - 1 Step 1

                    If tabla_.Columns(indice_).ColumnName = campo_.Atributo.ToString Then

                        If Not campo_.Descripcion Is Nothing Then

                            If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                                tabla_.Columns(indice_).ColumnName = campo_.Descripcion

                            ElseIf _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesAutomaticas Or
                            _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo Then

                                tabla_.Columns(indice_).ColumnName = campo_.Atributo.ToString

                            End If

                        End If

                    End If

                Next

            Next

        End Sub

        Private Sub RenombraEncabezados(ByRef tabla_ As DataTable, ByVal estructuraRequerida_ As IOperacionesCatalogo)

            For Each campo_ As ICaracteristica In estructuraRequerida_.Caracteristicas.Values

                If Not campo_.Visible = ICaracteristica.TiposVisible.No And
                      Not campo_.Visible = ICaracteristica.TiposVisible.Virtual And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Informacion And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Impresion Then

                    If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                        If tabla_.Columns.Contains(campo_.Nombre) Then

                            tabla_.Columns(campo_.Nombre).ColumnName = campo_.NombreMostrar

                        End If

                    ElseIf _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesAutomaticas Or
                            _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo Then

                        'El dataset para el origen de datos mongo ya viene en nombres técnicos.
                        If Not _OrigenDatos = IConexiones.Controladores.MongoDB Then

                            If tabla_.Columns.Contains(campo_.NombreMostrar) Then

                                tabla_.Columns(campo_.NombreMostrar).ColumnName = campo_.Nombre

                            End If

                        End If

                    End If

                End If

            Next

        End Sub

        '16/02/2021 Para un paquete de datos, 05/02/2021, MOP Entrada de transacciones.
        Public Function OperacionDatos(ByVal estructuraRequerida_ As IEntidadDatos,
                                       ByVal tipoOperacionDatos_ As IOperacionesCatalogo.TiposOperacionSQL,
                                       ByVal clausulasLibres_ As String,
                                       ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher Implements IAdaptadorDatos.OperacionDatos


            Dim sistema_ As New Organismo

            If _espacioTrabajo Is Nothing Then

                'NO IMPLEMENTADO
                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                Return _status

            End If

            _pilaDatos.Clear()

            _entidadTabla.Rows.Clear()

            If _iOperaciones Is Nothing Then

                'NOT DENIFED
                _status.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                '            Dim _IOperaciones As IOperacionesCatalogo =
                'New OperacionesCatalogo With {.CantidadVisibleRegistros = _limiteRegistros,
                '                              .EspacioTrabajo = _espacioTrabajo,
                '                              .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles,
                '                              .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_,
                '                              .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre,
                '                              .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos,
                '                              .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet}

                Return _status

            Else

                With _iOperaciones
                    .CantidadVisibleRegistros = _limiteRegistros
                    .EspacioTrabajo = _espacioTrabajo
                    .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles
                    .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_
                    .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre
                    .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos
                    .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                    .TipoConexion = _TipoConexion
                    .ObjetoDatos = _ObjetoDatos
                    .OrigenDatos = _OrigenDatos
                End With

            End If

            'Declaramos la cantidad de registros que queremos visualizar.
            '' ''Dim _IOperaciones As IOperacionesCatalogo =
            '' ''    New OperacionesCatalogo With {.CantidadVisibleRegistros = _limiteRegistros,
            '' ''                                  .EspacioTrabajo = _espacioTrabajo,
            '' ''                                  .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles,
            '' ''                                  .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_,
            '' ''                                  .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre,
            '' ''                                  .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos,
            '' ''                                  .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet}
            '.IdentificadorEmpresa = _iOperaciones.IdentificadorEmpresa,

            'objetoDatos_.ModalidadConsulta = modalidadConsultas_

            'objetoDatos_.ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_

            ''Indicando que será por conexión libre
            'objetoDatos_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            ''Busqueda por enlace
            'objetoDatos_.TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

            ''Estableciendo que el consumo de datos es vía extranet
            'objetoDatos_.IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

            'Determina las propiedades completas de la propuesta del enlace desde el punto de vista de IOperaciones
            '_iOperaciones = sistema_.EnsamblaModulo(_iOperaciones, estructuraRequerida_.Dimension.ToString)

            sistema_.EnsamblaModulo(_iOperaciones, estructuraRequerida_.Dimension.ToString)

            If Not IsNothing(_OrigenDatos) And Not IsNothing(_TipoConexion) Then

                If _OrigenDatos <> IConexiones.Controladores.SinDefinir And
                   _TipoConexion <> IConexiones.TipoConexion.SinDefinir Then

                    With _iOperaciones

                        .TipoConexion = _TipoConexion

                        .ObjetoDatos = _ObjetoDatos

                        .OrigenDatos = _OrigenDatos

                    End With

                End If

            End If

            _iOperaciones.PreparaCatalogo()

            If Not _iOperaciones Is Nothing Then

                If Not _iOperaciones.Caracteristicas Is Nothing Then

                    If _iOperaciones.Caracteristicas.Count > 0 Then

                        sistema_.TraduceDatos(_iOperaciones,
                                                 estructuraRequerida_,
                                                 tipoOperacionDatos_)

                        Dim listaLlaves_ As New List(Of String)

                        listaLlaves_ = estructuraRequerida_.UpdateOnKeyValues

                        If listaLlaves_ Is Nothing Then

                            _status.SetError(TagWatcher.ErrorTypes.C3_001_3001)

                            Return _status

                        End If

                        Select Case tipoOperacionDatos_

                            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion
                                'Realizadmos la ejecución, es decir ejecutamos Agregar
                                If _iOperaciones.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    'Se carga el dato en IOperaciones, en caso que haya algún resultado
                                    _iOperaciones = _iOperaciones

                                    'Se carga el dataset, esto estará aprueba realmente no es necesario
                                    If sistema_.TieneResultados(_iOperaciones) Then

                                        _entidadTabla = _iOperaciones.Vista.Tables(0).Copy

                                    End If

                                    'NO IMPLEMENTADO
                                    _status.SetOK()

                                    _status.ObjectReturned = _iOperaciones.ValorIndice

                                Else
                                    'NO IMPLEMENTADO
                                    _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                                End If

                            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar


                                If _iOperaciones.Modificar(Nothing) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    _status.SetOK()

                                    _status.ObjectReturned = _iOperaciones.ValorIndice

                                Else

                                    _status.SetError(TagWatcher.ErrorTypes.C3_001_3002)

                                End If

                            Case IOperacionesCatalogo.TiposOperacionSQL.Eliminar

                                If _iOperaciones.Eliminar(Nothing) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    _status.SetOK()

                                Else

                                    _status.SetError(TagWatcher.ErrorTypes.C3_001_3003)

                                End If

                        End Select

                    Else

                        _status.SetError(TagWatcher.ErrorTypes.C3_001_3004)

                    End If

                Else

                    _status.SetError(TagWatcher.ErrorTypes.C3_001_3004)

                End If

            Else

                _status.SetError(TagWatcher.ErrorTypes.C3_001_3005)

            End If

            Return _status

        End Function

        '16/02/2021 Para un bulk de datos, 05/02/2021, MOP Entrada de transacciones.
        Public Function OperacionDatos(ByVal bulkRequerida_ As List(Of IEntidadDatos),
                                       ByVal tipoOperacionDatos_ As IOperacionesCatalogo.TiposOperacionSQL,
                                       ByVal clausulasLibres_ As String,
                                       ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher Implements IAdaptadorDatos.OperacionDatos

            Dim sistema_ As New Organismo

            If _espacioTrabajo Is Nothing Then

                'NO IMPLEMENTADO
                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                Return _status

            End If

            _pilaDatos.Clear()

            _entidadTabla.Rows.Clear()

            'Verificaremos si se generó la instancia anteriormente para poder trabajar.
            If _iOperaciones Is Nothing Then

                'NOT DENIFED
                _status.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                Return _status

            End If
            'Dim _iOperaciones As IOperacionesCatalogo = New OperacionesCatalogo

            'Declaramos la cantidad de registros que queremos visualizar.

            With _iOperaciones

                .CantidadVisibleRegistros = _limiteRegistros

                .EspacioTrabajo = _espacioTrabajo   'Le entregamos credenciales

                .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles



                .ModalidadConsulta = modalidadConsultas_

                .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_

                'Indicando que será por conexión libre
                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                'Busqueda por enlace
                .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

                'Estableciendo que el consumo de datos es vía extranet
                .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                .TipoConexion = _TipoConexion
                .ObjetoDatos = _ObjetoDatos

                .OrigenDatos = _OrigenDatos

            End With

            'Determina las propiedades completas de la propuesta del enlace desde el punto de vista de IOperaciones
            sistema_.EnsamblaModulo(_iOperaciones, bulkRequerida_(0).Dimension.ToString)

            _iOperaciones.PreparaCatalogo()

            If Not _iOperaciones Is Nothing Then

                If Not _iOperaciones.Caracteristicas Is Nothing Then

                    If _iOperaciones.Caracteristicas.Count > 0 Then

                        'Agrega a IOperaciones.AgregaRegistroBulk que es lineabaseIop->_listaRegistros
                        sistema_.TraduceDatos(_iOperaciones,
                                                 bulkRequerida_,
                                                 tipoOperacionDatos_)

                        Select Case tipoOperacionDatos_

                            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion
                                'Realizadmos la ejecución, es decir ejecutamos Agregar
                                If _iOperaciones.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    'Se carga el dato en IOperaciones, en caso que haya algún resultado
                                    _iOperaciones = _iOperaciones

                                    'Se carga el dataset, esto estará aprueba realmente no es necesario
                                    If sistema_.TieneResultados(_iOperaciones) Then

                                        _entidadTabla = _iOperaciones.Vista.Tables(0).Copy

                                    End If

                                    'NO IMPLEMENTADO
                                    _status.SetOK()

                                Else
                                    'NO IMPLEMENTADO
                                    _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                                End If

                            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                                If _iOperaciones.Modificar(Nothing) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    'NO IMPLEMENTADO
                                    _status.SetOK()

                                Else
                                    'NO IMPLEMENTADO
                                    _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                                End If

                            Case IOperacionesCatalogo.TiposOperacionSQL.Eliminar

                                If _iOperaciones.Eliminar(Nothing) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    'NO IMPLEMENTADO
                                    _status.SetOK()

                                Else
                                    'NO IMPLEMENTADO
                                    _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                                End If

                        End Select

                    Else

                        'NO IMPLEMENTADO
                        _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                    End If

                Else

                    'NO IMPLEMENTADO
                    _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)


                End If

            Else
                'NO IMPLEMENTADO
                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

            End If


            Return _status

        End Function

        Public Function ProcesaConsulta(ByVal estructuraRequerida_ As IEntidadDatos,
                                        ByVal clausulasLibres_ As String,
                                        ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher _
            Implements IAdaptadorDatos.ProcesaConsulta

            Dim sistema_ As New Organismo

            If _espacioTrabajo Is Nothing Then

                'NO IMPLEMENTADO
                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                Return _status

            End If

            _pilaDatos.Clear()

            _entidadTabla.Rows.Clear()

            'Verificaremos si se generó la instancia anteriormente para poder trabajar.
            If _iOperaciones Is Nothing Then

                'NOT DENIFED
                _status.SetError(TagWatcher.ErrorTypes.C1_000_0001)

                Return _status

            End If

            'Declaramos la cantidad de registros que queremos visualizar.
            With _iOperaciones

                .CantidadVisibleRegistros = _limiteRegistros

                .EspacioTrabajo = _espacioTrabajo   'Le entregamos credenciales

                .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles

                .ModalidadConsulta = modalidadConsultas_

                .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_

                'Indicando que será por conexión libre
                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                'Busqueda por enlace
                .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

                'Estableciendo que el consumo de datos es vía extranet
                .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                'Propiedades especiales para fuente y formato de datos
                .OrigenDatos = _OrigenDatos

                .TipoConexion = _TipoConexion

                .ObjetoDatos = _ObjetoDatos

            End With

            'MOP Obsoleto 03/03/2021
            '_iOperaciones = sistema_.ConsultaModulo(_iOperaciones,
            '                                         estructuraRequerida_.Dimension.ToString,
            '                                         estructuraRequerida_)

            sistema_.ConsultaModuloDirecto(_iOperaciones,
                                           estructuraRequerida_.Dimension.ToString,
                                           estructuraRequerida_)

            Select Case _OrigenDatos
                    'para RDBMS SQL
                Case IConexiones.Controladores.SQLServer2008,
                         IConexiones.Controladores.MySQL51,
                         IConexiones.Controladores.Automatico

                    'Tipo de conexion siempre es automática
                    _TipoConexion = IConexiones.TipoConexion.SqlCommand

                    Select Case _ObjetoDatos

                            'Tipos de objeto soportados para RDMS
                        Case IConexiones.TiposRepositorio.Automatico,
                                 IConexiones.TiposRepositorio.DataSetObject,
                                 IConexiones.TiposRepositorio.DataTableObject


                        Case IConexiones.TiposRepositorio.BSONDocumentObject
                            'NOT IMPLEMENTED

                    End Select


                        'para NoSQL

                Case IConexiones.Controladores.MongoDB

                    'Tipo de conexion siempre es automática
                    _TipoConexion = IConexiones.TipoConexion.DirectMongoDB

                    Select Case _ObjetoDatos

                            'Tipos de objeto soportados para RDMS
                        Case IConexiones.TiposRepositorio.Automatico,
                                 IConexiones.TiposRepositorio.DataSetObject,
                                 IConexiones.TiposRepositorio.DataTableObject
                                'NOT IMPLEMENTED

                        Case IConexiones.TiposRepositorio.BSONDocumentObject
                            'NOT IMPLEMENTED

                    End Select

                Case Else


            End Select



            If Not _iOperaciones Is Nothing Then

                'Aquí generaremos los objetos solicitados como respuesta

                '############  F O R M A T E A N D O   O B J E T O   D E   D A T O S  ############

                If sistema_.TieneResultados(_iOperaciones) Then

                    'Se carga el dato en IOperaciones
                    '_iOperaciones = _iOperaciones

                    _entidadTabla = _iOperaciones.Vista.Tables(0)

                    If (_OrigenDatos = IConexiones.Controladores.SQLServer2008 Or
                        _OrigenDatos = IConexiones.Controladores.MySQL51 Or
                        _OrigenDatos = IConexiones.Controladores.Automatico) And
                        (_TipoConexion = IConexiones.TipoConexion.SqlCommand Or
                         _TipoConexion = IConexiones.TipoConexion.MySQLCommand Or
                         _TipoConexion = IConexiones.TipoConexion.Automatico) Then

                        'Asigna nombre de presentación 
                        If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                            If _ObjetoDatos = IConexiones.TiposRepositorio.Automatico Or
                           _ObjetoDatos = IConexiones.TiposRepositorio.DataSetObject Then

                                RenombraEncabezados(_entidadTabla, estructuraRequerida_)

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.BSONDocumentObject Then
                                'NOT IMPLEMENTED

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.DataTableObject Then

                                'NOT IMPLEMENTED

                            End If

                        ElseIf _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo Then

                            If _ObjetoDatos = IConexiones.TiposRepositorio.Automatico Or
                               _ObjetoDatos = IConexiones.TiposRepositorio.DataSetObject Then

                                RenombraEncabezados(_entidadTabla, _iOperaciones)

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.BSONDocumentObject Then
                                'NOT IMPLEMENTED

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.DataTableObject Then

                                'NOT IMPLEMENTED

                            End If

                        End If


                    ElseIf (_OrigenDatos = IConexiones.Controladores.MongoDB) And
                        (_TipoConexion = IConexiones.TipoConexion.Automatico Or
                         _TipoConexion = IConexiones.TipoConexion.DirectMongoDB) Then

                        If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                            If _ObjetoDatos = IConexiones.TiposRepositorio.Automatico Or
                               _ObjetoDatos = IConexiones.TiposRepositorio.BSONDocumentObject Then

                                'NOT IMPLEMENTED

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.DataSetObject Then
                                'Asigna nombre de presentación 
                                RenombraEncabezados(_entidadTabla, _iOperaciones)

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.DataTableObject Then

                                'NOT IMPLEMENTED

                            End If

                        ElseIf _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo Then

                            'RenombraEncabezados(_entidadTabla, estructuraRequerida_)
                            If _ObjetoDatos = IConexiones.TiposRepositorio.Automatico Or
                               _ObjetoDatos = IConexiones.TiposRepositorio.BSONDocumentObject Then

                                'NOT IMPLEMENTED

                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.DataSetObject Then
                                'Asigna nombre de presentación 
                                'NOT REQUIRED... RenombraEncabezados(_entidadTabla, estructuraRequerida_)


                            ElseIf _ObjetoDatos = IConexiones.TiposRepositorio.DataTableObject Then

                                'NOT IMPLEMENTED

                            End If

                        End If


                    End If




                    For indice_ As Int32 = 0 To _iOperaciones.Vista.Tables(0).Rows.Count - 1

                        'Creamos el espacio de un registro nuevo, tipo Referencia
                        Dim pckRegistroNuevo_ As IEntidadDatos = New Referencia

                        Dim contador_ As Int32 = 1

                        'Rellenamos los campos uno a uno
                        For Each campoAuxiliar_ As CampoVirtual In estructuraRequerida_.Atributos

                            Dim campoProduccion_ As New CampoVirtual

                            With campoProduccion_

                                If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                                    If Not campoAuxiliar_.Descripcion Is Nothing Then

                                        .Descripcion = campoAuxiliar_.Descripcion

                                    Else

                                        .Descripcion = RetornaDescripcionVistaEntorno(_iOperaciones.Caracteristicas, campoAuxiliar_.Atributo.ToString)

                                    End If

                                Else

                                    .Descripcion = RetornaDescripcionVistaEntorno(_iOperaciones.Caracteristicas, campoAuxiliar_.Atributo.ToString) '.Caracteristicas(contador_).NombreMostrar

                                End If

                                .Orden = campoAuxiliar_.Orden

                                .TipoDato = campoAuxiliar_.TipoDato

                                .Atributo = campoAuxiliar_.Atributo

                                .Valor = _iOperaciones.Vista(indice_,
                                                        ConversionExternaCampo(campoAuxiliar_.Atributo,
                                                        estructuraRequerida_.Dimension),
                                                        IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)


                                .EsAgrupador = campoAuxiliar_.EsAgrupador

                                .TipoOrdenamiento = campoAuxiliar_.TipoOrdenamiento

                                .FuncionAgregacion = campoAuxiliar_.FuncionAgregacion


                            End With

                            'Agregamos los campos cargadod en el registro nuevo!
                            pckRegistroNuevo_.Atributos.Add(campoProduccion_)

                            contador_ += 1

                        Next

                        'Agregamos a la pila de registros
                        _pilaDatos.Add(pckRegistroNuevo_)

                    Next

                    'NO IMPLEMENTADO
                    _status.SetOK()

                Else
                    'NO IMPLEMENTADO
                    _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                End If

            End If

            Return _status

        End Function

        Private Function RetornaDescripcionVistaEntorno(ByRef icaracteristicas_ As Dictionary(Of Integer, ICaracteristica),
                                                        ByVal nombreCaracteristica_ As String) As String

            For Each caracteristica_ As KeyValuePair(Of Integer, ICaracteristica) In icaracteristicas_

                If caracteristica_.Value.Nombre = nombreCaracteristica_ Then

                    Return caracteristica_.Value.NombreMostrar

                    Exit For

                End If

            Next

            Return Nothing

        End Function

        Public Function GeneraEstructuraResultados(ByRef estructuraRequerida_ As IEntidadDatos,
                                                   ByVal clausulasLibres_ As String,
                                                   ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher _
          Implements IAdaptadorDatos.GeneraEstructuraResultados

            Dim sistema_ As New Organismo

            Dim estructuraResultados_ As New List(Of IEntidadDatos)

            If _espacioTrabajo Is Nothing Then

                'NO IMPLEMENTADO
                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                Return _status

            End If

            Dim buscadorEncabezados_ As IOperacionesCatalogo = New OperacionesCatalogo

            With buscadorEncabezados_

                .CantidadVisibleRegistros = _limiteRegistros

                .EspacioTrabajo = _espacioTrabajo

                .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles

                .ModalidadConsulta = modalidadConsultas_

                .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_

                .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                .IDRecursoSolicitante = 0

                .IDUsuario = 0

                .ClaveDivisionMiEmpresa = 0

                .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

            End With

            buscadorEncabezados_ = sistema_.EnsamblaModulo(estructuraRequerida_.Dimension.ToString)

            buscadorEncabezados_.PreparaCatalogo()

            If Not buscadorEncabezados_ Is Nothing Then

                If Not buscadorEncabezados_.Caracteristicas Is Nothing Then

                    If buscadorEncabezados_.Caracteristicas.Count > 0 Then

                        'Creamos el espacio de un registro nuevo, tipo Referencia
                        Dim pckResultados_ As IEntidadDatos = New Referencia

                        Dim posicion_ As Int32? = Nothing

                        Dim indiceSolicitud_ As Int32 = 1

                        Dim listaNegra_ As New List(Of Int32)

                        'Rellenamos los campos uno a uno
                        For Each campoAuxiliar_ As CampoVirtual In estructuraRequerida_.Atributos

                            Dim campoResultado_ As New CampoVirtual

                            With campoResultado_

                                'Verificamos si tiene acceso al campo en caso de que el campo requiera un permiso especial
                                posicion_ = ObtenerPosicionCaracteristica(buscadorEncabezados_.Caracteristicas, campoAuxiliar_.Atributo.ToString)

                                If posicion_ Is Nothing Then

                                    'Si no existe la caracteristica habrá que removerla apesar de la solicitud
                                    'Registramos los elementos no encontrados en la lista para ser borrados fuera del ciclo
                                    listaNegra_.Add(indiceSolicitud_)

                                ElseIf posicion_ > 0 Then

                                    If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                                        If Not campoAuxiliar_.Descripcion Is Nothing Then

                                            .Descripcion = campoAuxiliar_.Descripcion

                                        Else

                                            .Descripcion = buscadorEncabezados_.Caracteristicas(posicion_).NombreMostrar

                                        End If

                                    Else

                                        .Descripcion = buscadorEncabezados_.Caracteristicas(posicion_).NombreMostrar

                                    End If

                                    .Orden = campoAuxiliar_.Orden

                                    .Atributo = campoAuxiliar_.Atributo

                                    .TipoDato = ObtenerTipoDatoEntidad(buscadorEncabezados_.Caracteristicas(posicion_).TipoDato)

                                    .TipoDatoHTML = ObtenerTipoDatoHTML(buscadorEncabezados_.Caracteristicas(posicion_).TipoDato)

                                    .PuedeInsertar = buscadorEncabezados_.Caracteristicas(posicion_).PuedeInsertar

                                    .PuedeModificar = buscadorEncabezados_.Caracteristicas(posicion_).PuedeModificar

                                    .Visible = buscadorEncabezados_.Caracteristicas(posicion_).Visible

                                    .EstiloCCS = campoAuxiliar_.EstiloCCS

                                    .Interfaz = buscadorEncabezados_.Caracteristicas(posicion_).Interfaz

                                    .NameAsKey = buscadorEncabezados_.Caracteristicas(posicion_).NameAsKey

                                    .Valor = buscadorEncabezados_.Caracteristicas(posicion_).ValorDefault

                                    'Nuevo MOP bautismen 28012020
                                    .IDPermiso = buscadorEncabezados_.Caracteristicas(posicion_).PermisoConsulta

                                    Select Case buscadorEncabezados_.Caracteristicas(posicion_).TipoFiltro

                                        Case 1   ' Filtro Principal
                                            .AtributoLLave = True
                                            .EsFiltro = True
                                            .TipoFiltro = 1

                                        Case 2  ' Filtros Avanzados
                                            .EsFiltro = True
                                            .TipoFiltro = 2

                                    End Select

                                    If Not campoAuxiliar_.AtributoFiltro Is Nothing Then

                                        Dim campoFiltro_ As CampoVirtual = Nothing

                                        campoFiltro_ = ObtenerCampoVirtual(buscadorEncabezados_.Caracteristicas, campoAuxiliar_.AtributoFiltro.Atributo.ToString)

                                        .AtributoFiltro = campoFiltro_

                                    End If

                                End If

                            End With

                            If posicion_ <> 0 Then

                                'Solo serán agregados los campos localizados y permitidos
                                pckResultados_.Atributos.Add(campoResultado_)

                            End If

                            indiceSolicitud_ += 1

                        Next

                        'Agregamos a la pila de registros  (Esto está OK), son 16 y 17 por que comienza de cero, si comenzara de 1 serían 17 y 18
                        estructuraResultados_.Add(pckResultados_)

                        'Elaboramos las remociones registradas
                        Dim corrimiento_ As Int32 = 0

                        For Each item_ As Int32 In listaNegra_

                            'If corrimiento_ = 0 Then

                            estructuraRequerida_.Atributos.RemoveAt(item_ - corrimiento_ - 1)

                            corrimiento_ += 1
                            'Else

                            '    estructuraRequerida_.Atributos.RemoveAt(item_ - 1)

                            'End If

                            'estructuraRequerida_.Atributos
                        Next

                        'NO IMPLEMENTADO
                        _status.SetOK()

                        _status.ObjectReturned = estructuraResultados_

                    Else
                        '    'NO IMPLEMENTADO
                        _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                    End If

                End If

            End If

            Return _status

        End Function

        'Método obsoleto, 03/05/2021 MOP
        'Public Function GeneraEstructuraResultados(ByRef estructuraRequerida_ As IEntidadDatos,
        '                                         ByVal clausulasLibres_ As String,
        '                                         ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher _
        '  Implements IAdaptadorDatos.GeneraEstructuraResultados

        '    Dim sistema_ As New Organismo

        '    Dim estructuraResultados_ As New List(Of IEntidadDatos)

        '    If _espacioTrabajo Is Nothing Then

        '        'NO IMPLEMENTADO
        '        _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

        '        Return _status

        '    End If

        '    Dim buscadorEncabezados_ As IOperacionesCatalogo = New OperacionesCatalogo

        '    With buscadorEncabezados_

        '        .CantidadVisibleRegistros = _limiteRegistros

        '        .EspacioTrabajo = _espacioTrabajo

        '        .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles

        '        .ModalidadConsulta = modalidadConsultas_

        '        .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_

        '        .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

        '        .IDRecursoSolicitante = 0

        '        .IDUsuario = 0

        '        .ClaveDivisionMiEmpresa = 0

        '        .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

        '    End With

        '    buscadorEncabezados_ = sistema_.EnsamblaModulo(estructuraRequerida_.Dimension.ToString)

        '    buscadorEncabezados_.PreparaCatalogo()

        '    If Not buscadorEncabezados_ Is Nothing Then

        '        If Not buscadorEncabezados_.Caracteristicas Is Nothing Then

        '            If buscadorEncabezados_.Caracteristicas.Count > 0 Then

        '                'Creamos el espacio de un registro nuevo, tipo Referencia
        '                Dim pckResultados_ As IEntidadDatos = New Referencia

        '                Dim posicion_ As Int32 = 0

        '                Dim indiceSolicitud_ As Int32 = 1

        '                Dim listaNegra_ As New List(Of Int32)

        '                'Rellenamos los campos uno a uno
        '                For Each campoAuxiliar_ As CampoVirtual In estructuraRequerida_.Atributos

        '                    Dim campoResultado_ As New CampoVirtual

        '                    With campoResultado_

        '                        'Verificamos si tiene acceso al campo en caso de que el campo requiera un permiso especial



        '                        posicion_ = ObtenerPosicionCaracteristica(buscadorEncabezados_.Caracteristicas, campoAuxiliar_.Atributo.ToString)

        '                        If posicion_ = 0 Then

        '                            'Si no existe la caracteristica habrá que removerla apesar de la solicitud
        '                            'Registramos los elementos no encontrados en la lista para ser borrados fuera del ciclo
        '                            listaNegra_.Add(indiceSolicitud_)

        '                        ElseIf posicion_ > 0 Then



        '                            If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

        '                                If Not campoAuxiliar_.Descripcion Is Nothing Then

        '                                    .Descripcion = campoAuxiliar_.Descripcion

        '                                Else

        '                                    .Descripcion = buscadorEncabezados_.Caracteristicas(posicion_).NombreMostrar

        '                                End If

        '                            Else

        '                                .Descripcion = buscadorEncabezados_.Caracteristicas(posicion_).NombreMostrar

        '                            End If

        '                            .Orden = campoAuxiliar_.Orden

        '                            .Atributo = campoAuxiliar_.Atributo

        '                            .TipoDato = ObtenerTipoDatoEntidad(buscadorEncabezados_.Caracteristicas(posicion_).TipoDato)

        '                            .TipoDatoHTML = ObtenerTipoDatoHTML(buscadorEncabezados_.Caracteristicas(posicion_).TipoDato)

        '                            .PuedeInsertar = buscadorEncabezados_.Caracteristicas(posicion_).PuedeInsertar

        '                            .PuedeModificar = buscadorEncabezados_.Caracteristicas(posicion_).PuedeModificar

        '                            .Visible = buscadorEncabezados_.Caracteristicas(posicion_).Visible

        '                            .EstiloCCS = campoAuxiliar_.EstiloCCS

        '                            .Interfaz = buscadorEncabezados_.Caracteristicas(posicion_).Interfaz

        '                            .NameAsKey = buscadorEncabezados_.Caracteristicas(posicion_).NameAsKey

        '                            .Valor = buscadorEncabezados_.Caracteristicas(posicion_).ValorDefault

        '                            'Nuevo MOP bautismen 28012020
        '                            .IDPermiso = buscadorEncabezados_.Caracteristicas(posicion_).PermisoConsulta

        '                            Select Case buscadorEncabezados_.Caracteristicas(posicion_).TipoFiltro

        '                                Case 1   ' Filtro Principal
        '                                    .AtributoLLave = True
        '                                    .EsFiltro = True

        '                                Case 2  ' Filtros Avanzados
        '                                    .EsFiltro = True

        '                            End Select

        '                            If Not campoAuxiliar_.AtributoFiltro Is Nothing Then

        '                                Dim campoFiltro_ As CampoVirtual = Nothing

        '                                campoFiltro_ = ObtenerCampoVirtual(buscadorEncabezados_.Caracteristicas, campoAuxiliar_.AtributoFiltro.Atributo.ToString)

        '                                .AtributoFiltro = campoFiltro_

        '                            End If

        '                        End If

        '                    End With

        '                    If posicion_ <> 0 Then
        '                        'Solo serán agregados los campos localizados y permitidos
        '                        pckResultados_.Atributos.Add(campoResultado_)

        '                    End If

        '                    indiceSolicitud_ += 1
        '                Next

        '                'Agregamos a la pila de registros  (Esto está OK), son 16 y 17 por que comienza de cero, si comenzara de 1 serían 17 y 18
        '                estructuraResultados_.Add(pckResultados_)


        '                'Elaboramos las remociones registradas
        '                Dim corrimiento_ As Int32 = 0

        '                For Each item_ As Int32 In listaNegra_

        '                    'If corrimiento_ = 0 Then

        '                    estructuraRequerida_.Atributos.RemoveAt(item_ - corrimiento_ - 1)

        '                    corrimiento_ += 1
        '                    'Else

        '                    '    estructuraRequerida_.Atributos.RemoveAt(item_ - 1)

        '                    'End If

        '                    'estructuraRequerida_.Atributos
        '                Next

        '                'NO IMPLEMENTADO
        '                _status.SetOK()

        '                _status.ObjectReturned = estructuraResultados_

        '            Else
        '                '    'NO IMPLEMENTADO
        '                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

        '            End If

        '        End If

        '    End If

        '    Return _status

        'End Function

        Public Function GeneraEstructuraCompleta(ByVal estructuraRequerida_ As IEntidadDatos,
                                                 ByVal nombreEntidad_ As String,
                                                 ByVal clausulasLibres_ As String,
                                                 ByVal modalidadConsultas_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher _
          Implements IAdaptadorDatos.GeneraEstructuraCompleta

            Dim sistema_ As New Organismo

            Dim estructuraResultados_ As New List(Of IEntidadDatos)

            If _espacioTrabajo Is Nothing Then

                'NO IMPLEMENTADO
                _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                Return _status

            End If



            Dim buscadorEncabezados_ As IOperacionesCatalogo = New OperacionesCatalogo

            With buscadorEncabezados_

                .CantidadVisibleRegistros = _limiteRegistros

                .EspacioTrabajo = _espacioTrabajo

                .VisualizacionCamposConfigurada = IOperacionesCatalogo.TiposVisualizacionCampos.PresentarVisibles

                .ModalidadConsulta = modalidadConsultas_

                .ClausulasLibres = _filtrosAvanzados & " " & clausulasLibres_

                .IDAplicacion = IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                .IDRecursoSolicitante = 0

                .IDUsuario = 0

                .ClaveDivisionMiEmpresa = 0

                .TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

            End With

            buscadorEncabezados_ = sistema_.EnsamblaModulo(estructuraRequerida_.Dimension.ToString)

            buscadorEncabezados_.PreparaCatalogo()

            If Not buscadorEncabezados_ Is Nothing Then

                If Not buscadorEncabezados_.Caracteristicas Is Nothing Then

                    If buscadorEncabezados_.Caracteristicas.Count > 0 Then

                        'Creamos el espacio de un registro nuevo, tipo Referencia

                        Dim pckResultados_ As IEntidadDatos

                        Dim posicion_ As Int32 = 1

                        ' pckResultados_ = New Referencia

                        pckResultados_ = sistema_.Eval("New " & nombreEntidad_,
                                                       nombreEntidad_)

                        'Rellenamos los campos uno a uno
                        For Each iCaracteristica_ As ICaracteristica In buscadorEncabezados_.Caracteristicas.Values


                            Dim campoVirtual_ As New CampoVirtual

                            With campoVirtual_

                                'posicion_ = posicion_ = buscadorEncabezados_.Caracteristicas.Keys(0) '.Key ' ObtenerPosicionCaracteristica(buscadorEncabezados_.Caracteristicas, campoAuxiliar_.Atributo.ToString)

                                If posicion_ > 0 Then

                                    If _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme Then

                                        If Not iCaracteristica_.Nombre Is Nothing Then

                                            .Descripcion = iCaracteristica_.Nombre

                                        Else

                                            .Descripcion = iCaracteristica_.NombreMostrar

                                        End If

                                    Else

                                        .Descripcion = iCaracteristica_.NombreMostrar

                                    End If

                                    .Orden = posicion_ 'campoAuxiliar_..Orden

                                    .Atributo = iCaracteristica_.Nombre

                                    .TipoDato = ObtenerTipoDatoEntidad(iCaracteristica_.TipoDato)

                                    .TipoDatoHTML = ObtenerTipoDatoHTML(iCaracteristica_.TipoDato)

                                    .PuedeInsertar = iCaracteristica_.PuedeInsertar

                                    .PuedeModificar = iCaracteristica_.PuedeModificar

                                    'Caracteristicas especiales

                                    .NameAsKey = iCaracteristica_.NameAsKey

                                    .Interfaz = iCaracteristica_.Interfaz

                                    'Nuevo MOP 28012020, bautismen
                                    .IDPermiso = iCaracteristica_.PermisoConsulta


                                    Select Case iCaracteristica_.Visible

                                        Case ICaracteristica.TiposVisible.Si
                                            .Visible = ICaracteristica.TiposVisible.Si
                                        Case ICaracteristica.TiposVisible.Acarreo
                                            .Visible = ICaracteristica.TiposVisible.Acarreo
                                        Case ICaracteristica.TiposVisible.Impresion
                                            .Visible = ICaracteristica.TiposVisible.Impresion
                                        Case ICaracteristica.TiposVisible.No
                                            .Visible = ICaracteristica.TiposVisible.No

                                        Case ICaracteristica.TiposVisible.Virtual
                                            .Visible = ICaracteristica.TiposVisible.Virtual

                                    End Select

                                    '.TipoLLave = campoAuxiliar_.TipoLlave

                                    .EstiloCCS = IEntidadDatos.EstilossCCS.SinDefinir

                                    Select Case iCaracteristica_.Llave

                                        Case ICaracteristica.TipoLlave.Primaria   ' Filtro Principal

                                            '.TipoLLave = IEntidadDatos.TiposLlave.Primaria

                                            .AtributoLLave = 1


                                        Case ICaracteristica.TipoLlave.SinLlave  ' Filtros Avanzados

                                            .AtributoLLave = 0

                                            '.TipoLLave = IEntidadDatos.TiposLlave.SinDefinir

                                    End Select

                                    Select Case iCaracteristica_.TipoFiltro

                                        Case ICaracteristica.TiposFiltro.PorDefecto   ' Filtro Principal

                                            .EsFiltro = True

                                        Case ICaracteristica.TiposFiltro.Avanzado

                                            .EsFiltro = False

                                    End Select

                                    If Not iCaracteristica_.TipoFiltro = ICaracteristica.TiposFiltro.SinDefinir Then

                                        'Dim campoFiltro_ As New CampoVirtual

                                        Dim campoFiltro_ As New CampoVirtual

                                        Select Case iCaracteristica_.TipoFiltro

                                            Case ICaracteristica.TiposFiltro.Avanzado

                                                campoFiltro_.EsFiltro = True

                                            Case ICaracteristica.TiposFiltro.PorDefecto

                                                campoFiltro_.EsFiltro = True

                                        End Select

                                        .AtributoFiltro = campoFiltro_

                                    End If

                                End If

                            End With

                            pckResultados_.Atributos.Add(campoVirtual_)

                        Next

                        'Agregamos a la pila de registros
                        estructuraResultados_.Add(pckResultados_)

                        'NO IMPLEMENTADO
                        _status.SetOK()

                        _status.ObjectReturned = estructuraResultados_

                    Else
                        '    'NO IMPLEMENTADO
                        _status.SetError(TagWatcher.ErrorTypes.C1_000_1000)

                    End If

                End If

            End If

            Return _status

        End Function

        Private Function ObtenerTipoDatoEntidad(ByVal i_TipoDato_ As ICaracteristica.TiposCaracteristica) As IEntidadDatos.TiposDatos

            Dim i_TipoDatoEntidad_ As IEntidadDatos.TiposDatos = IEntidadDatos.TiposDatos.SinDefinir

            Select Case i_TipoDato_

                Case ICaracteristica.TiposCaracteristica.cString
                    i_TipoDatoEntidad_ = IEntidadDatos.TiposDatos.Texto

                Case ICaracteristica.TiposCaracteristica.cInt32
                    i_TipoDatoEntidad_ = IEntidadDatos.TiposDatos.Entero

                Case ICaracteristica.TiposCaracteristica.cDateTime
                    i_TipoDatoEntidad_ = IEntidadDatos.TiposDatos.Fecha

                Case ICaracteristica.TiposCaracteristica.cReal
                    i_TipoDatoEntidad_ = IEntidadDatos.TiposDatos.Real

                Case ICaracteristica.TiposCaracteristica.cBoolean
                    i_TipoDatoEntidad_ = IEntidadDatos.TiposDatos.Booleno

            End Select

            Return i_TipoDatoEntidad_

        End Function

        Private Function ObtenerTipoDatoHTML(ByVal i_TipoDato_ As ICaracteristica.TiposCaracteristica) As IEntidadDatos.TiposDatosHTML

            Dim i_TipoDatoHTML_ As IEntidadDatos.TiposDatosHTML = IEntidadDatos.TiposDatosHTML.SinDefinir

            Select Case i_TipoDato_

                Case ICaracteristica.TiposCaracteristica.cString
                    i_TipoDatoHTML_ = IEntidadDatos.TiposDatosHTML.Texto

                Case ICaracteristica.TiposCaracteristica.cInt32
                    i_TipoDatoHTML_ = IEntidadDatos.TiposDatosHTML.Entero

                Case ICaracteristica.TiposCaracteristica.cDateTime
                    i_TipoDatoHTML_ = IEntidadDatos.TiposDatosHTML.Fecha

                Case ICaracteristica.TiposCaracteristica.cReal
                    i_TipoDatoHTML_ = IEntidadDatos.TiposDatosHTML.Real

                Case ICaracteristica.TiposCaracteristica.cBoolean
                    i_TipoDatoHTML_ = IEntidadDatos.TiposDatosHTML.Booleno

            End Select

            Return i_TipoDatoHTML_

        End Function

        Private Function ObtenerCampoVirtual(ByVal o_Caracteristicas As Dictionary(Of Integer, ICaracteristica), t_NombreCaracteristica_ As String) As CampoVirtual

            Dim campo_ As New CampoVirtual

            For Each caracteristica_ As KeyValuePair(Of Integer, ICaracteristica) In o_Caracteristicas

                If caracteristica_.Value.Nombre = t_NombreCaracteristica_ Then

                    campo_.Atributo = caracteristica_.Value.Nombre

                    campo_.Descripcion = caracteristica_.Value.NombreMostrar

                    campo_.TipoDato = ObtenerTipoDatoEntidad(caracteristica_.Value.TipoDato)

                    campo_.TipoDatoHTML = ObtenerTipoDatoHTML(caracteristica_.Value.TipoDato)

                    Select Case caracteristica_.Value.TipoFiltro

                        Case 1   ' Filtro Principal                          
                            campo_.EsFiltro = True

                        Case 2  ' Filtros Avanzados
                            campo_.EsFiltro = True

                    End Select

                    Exit For

                End If

            Next

            Return campo_

        End Function

        Private Function ObtenerPosicionCaracteristica(ByVal o_Caracteristicas As Dictionary(Of Integer, ICaracteristica), t_NombreCaracteristica_ As String) As Int32

            Dim posicion_ As Int32 = 0

            For Each caracteristica_ As KeyValuePair(Of Integer, ICaracteristica) In o_Caracteristicas

                If caracteristica_.Value.Nombre = t_NombreCaracteristica_ Then

                    'Si no hay permisos sobre el campo, el valor es asignado sin problema
                    If caracteristica_.Value.PermisoConsulta Is Nothing Or
                        caracteristica_.Value.PermisoConsulta = "118" Or
                        caracteristica_.Value.PermisoConsulta = "" Then

                        posicion_ = caracteristica_.Key


                        'Si existe un permiso antes de posicionar hay que asegurarse que el usuario contenga acceso para asignar
                    ElseIf _espacioTrabajo.BuscaPermiso(caracteristica_.Value.PermisoConsulta,
                        IEspacioTrabajo.TipoModulo.Abstracto) Then

                        posicion_ = caracteristica_.Key

                    Else

                        posicion_ = 0

                    End If

                    Exit For

                End If

            Next

            Return posicion_

        End Function

#End Region

#Region "Propiedades"

        Property FiltrosAvanzados As String Implements IAdaptadorDatos.FiltrosAvanzados
            Get
                Return _filtrosAvanzados
            End Get
            Set(value As String)
                _filtrosAvanzados = value
            End Set
        End Property

        Public Property LimiteRegistrosPresentacion As Int32 Implements IAdaptadorDatos.LimiteRegistrosPresentacion
            Get
                Return _limiteRegistrosPresentacion
            End Get
            Set(value As Int32)
                _limiteRegistrosPresentacion = value
            End Set
        End Property

        Public ReadOnly Property Estatus As Wma.Exceptions.TagWatcher _
            Implements IAdaptadorDatos.Estatus

            Get

                Return _status

            End Get

        End Property

        Public Property ProcesamientoAsincrono As Boolean _
            Implements IAdaptadorDatos.ProcesamientoAsincrono

            Get

                Return _procesamientoAsincrono

            End Get

            Set(value As Boolean)

                _procesamientoAsincrono = value

            End Set

        End Property

#End Region

        Public Property ObjetoDatos As IConexiones.TiposRepositorio Implements IAdaptadorDatos.ObjetoDatos

        Public Property OrigenDatos As IConexiones.Controladores Implements IAdaptadorDatos.OrigenDatos

        Public Property TipoConexion As IConexiones.TipoConexion Implements IAdaptadorDatos.TipoConexion

    End Class

End Namespace
