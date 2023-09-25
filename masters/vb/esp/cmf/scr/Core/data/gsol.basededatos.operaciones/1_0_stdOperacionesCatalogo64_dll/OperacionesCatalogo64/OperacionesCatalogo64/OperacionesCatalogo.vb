Imports System.Windows.Forms
Imports Gsol.krom
Imports Gsol.basededatos
'Imports MongoDB.Driver
'Imports MongoDB.Bson
Imports MongoDB.Driver.Builders

Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports MongoDB.Driver.MongoDBRefSerializer
Imports System.Globalization

Namespace Gsol.BaseDatos.Operaciones
    Public Class OperacionesCatalogo
        Implements IOperacionesCatalogo, ICloneable


#Region "Atributos"

        'Identificador de la empresa
        'Private _identificadorempresa As String

        '-------------------
        'Nombre común del catálogo
        Private _nombre As String

        Private _nombreToken As String

        'Vista que entrega la información de las columnas o encabezados
        Private _vistaencabezados As String

        'Número de serie del catalogo
        Private _seriecatalogo As Int32

        'Tabla principal que será afectada con los cambios del catálogo
        Private _tablaedicion As String

        'Tabla o vista responsables del despliege de los datos para grid view
        Private _operadorcatalogoconsultas As String

        Private _operadorcatalogoinsercion As String

        Private _operadorcatalogoborrar As String

        Private _operadorcatalogomodificacion As String

        Private _cantidadvisibleregistros As Int32

        Private _identificadorcatalogo As String

        Private _indicepaginacion As Int32

        'Private _icaracteristicas As Dictionary(Of Int32, ICaracteristica)

        Private _datasetvista As DataSet

        'LineaBase
        Private _lineabasecatalogos As LineaBaseCatalogos

        'Avanzada
        'Private _valorindiceseleccionado As String

        'Operación anterior
        Private _ioperacionanterior As OperacionesCatalogo

        Private _advertenciasIndicador As Boolean

        'Repositorio para paso de datos, se debe utilizar Casting

        Private _objetoRepositorio As Object


        ''PENDIENTEEE REVISAR


        ''Private _i_Cve_Usuario As Int32 'IDUsuario = _i_Cve_Usuario
        'Public Property IDUsuario As Int32

        ''Private _i_Cve_DivisionMiEmpresa As Int32 'conexion_.ClaveDivisionMiEmpresa = _i_Cve_DivisionMiEmpresa
        'Public Property ClaveDivisionMiEmpresa As Int32

        ''Private _i_Cve_Aplicacion As Int32 'conexion_.IDAplicacion = _i_Cve_Aplicacion
        'Public Property IDAplicacion As Int32


        ''Private TipoInstrumentacion As IBitacoras.TiposInstrumentacion 'conexion_.TipoInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones
        'Public Property TipoInstrumentacion As IBitacoras.TiposInstrumentacion


        ''Private _i_Cve_RecursoSolicitante As Int32 'conexion_.IDRecursoSolicitante = _i_Clave_Modulo
        'Public Property IDRecursoSolicitante As Int32



        Public Property ClaveDivisionMiEmpresa As Integer Implements IOperacionesCatalogo.ClaveDivisionMiEmpresa
            Get
                Return _lineabasecatalogos.ClaveDivisionMiEmpresa
            End Get
            Set(value As Integer)
                _lineabasecatalogos.ClaveDivisionMiEmpresa = value
            End Set
        End Property

        Public Property IDAplicacion As monitoreo.IBitacoras.ClaveTiposAplicacion Implements IOperacionesCatalogo.IDAplicacion
            Get
                Return _lineabasecatalogos.IDAplicacion
            End Get
            Set(value As monitoreo.IBitacoras.ClaveTiposAplicacion)
                _lineabasecatalogos.IDAplicacion = value
            End Set
        End Property

        Public Property IDRecursoSolicitante As Integer Implements IOperacionesCatalogo.IDRecursoSolicitante
            Get
                Return _lineabasecatalogos.IDRecursoSolicitante
            End Get
            Set(value As Integer)
                _lineabasecatalogos.IDRecursoSolicitante = value
            End Set
        End Property

        Public Property IDUsuario As Integer Implements IOperacionesCatalogo.IDUsuario
            Get
                Return _lineabasecatalogos.IDUsuario
            End Get
            Set(value As Integer)
                _lineabasecatalogos.IDUsuario = value
            End Set
        End Property

        Public Property TipoInstrumentacion As monitoreo.IBitacoras.TiposInstrumentacion Implements IOperacionesCatalogo.TipoInstrumentacion
            Get
                Return _lineabasecatalogos.TipoInstrumentacion
            End Get
            Set(value As monitoreo.IBitacoras.TiposInstrumentacion)
                _lineabasecatalogos.TipoInstrumentacion = value
            End Set
        End Property


#End Region

#Region "Constructores"

        Sub New()
            ' _identificadorempresa = Nothing

            _nombre = Nothing

            _nombreToken = Nothing

            _seriecatalogo = Nothing

            _tablaedicion = Nothing

            _vistaencabezados = Nothing

            _operadorcatalogoconsultas = Nothing

            _operadorcatalogoborrar = Nothing

            _operadorcatalogoinsercion = Nothing

            _operadorcatalogomodificacion = Nothing

            '27/Nov/2018, Por solicitud de los usuarios, se incrementó la cuota de 100 registros a 200, yo nunca estuve de acuerdo, Ivans me convenció que era el mismo costo.
            _cantidadvisibleregistros = 200

            '07/May/2020, Por solicitud de los usuarios, se decrementó la cuota de 100 registros a 200, yo nunca estuve de acuerdo, Ivans me convenció que era el mismo costo.
            '_cantidadvisibleregistros = 30

            _identificadorcatalogo = "-1"

            _indicepaginacion = 100

            '_indicepaginacion = 50


            '_valorindiceseleccionado = Nothing

            '_icaracteristicas = New Dictionary(Of Int32, ICaracteristica)
            _datasetvista = New DataSet

            _lineabasecatalogos = New LineaBaseCatalogos

            'Obtenet instancia Singleton
            _lineabasecatalogos.ConexionSingleton = _lineabasecatalogos.ConexionSingleton.ObtenerInstancia

            _ioperacionanterior = Nothing

            _advertenciasIndicador = True

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property ReflejarEn As IEnlaceDatos.DestinosParaReplicacion Implements IOperacionesCatalogo.ReflejarEn
            Get
                Return _lineabasecatalogos.ReflejarEn
            End Get
            Set(value As IEnlaceDatos.DestinosParaReplicacion)
                _lineabasecatalogos.ReflejarEn = value
            End Set
        End Property


        Public Property Dimension As String Implements IOperacionesCatalogo.Dimension
            Get
                Return _lineabasecatalogos.Dimension
            End Get
            Set(value As String)
                _lineabasecatalogos.Dimension = value
            End Set
        End Property

        Public Property Entidad As String Implements IOperacionesCatalogo.Entidad
            Get
                Return _lineabasecatalogos.Entidad
            End Get
            Set(value As String)
                _lineabasecatalogos.Entidad = value
            End Set
        End Property

        Public Property Granularidad As String Implements IOperacionesCatalogo.Granularidad
            Get
                Return _lineabasecatalogos.Granularidad
            End Get
            Set(value As String)
                _lineabasecatalogos.Granularidad = value
            End Set
        End Property

        Public Property Version As String Implements IOperacionesCatalogo.Version
            Get
                Return _lineabasecatalogos.Version
            End Get
            Set(value As String)
                _lineabasecatalogos.Version = value
            End Set
        End Property


        Public Property EstructuraConsulta As IEntidadDatos _
            Implements IOperacionesCatalogo.EstructuraConsulta

            Get

                Return _lineabasecatalogos.EstructuraConsulta

            End Get

            Set(value As IEntidadDatos)

                _lineabasecatalogos.EstructuraConsulta = value

            End Set

        End Property

        Public Property VisualizacionCamposConfigurada As IOperacionesCatalogo.TiposVisualizacionCampos _
            Implements IOperacionesCatalogo.VisualizacionCamposConfigurada

            Get

                Return _lineabasecatalogos.VisualizacionCamposConfigurada

            End Get

            Set(value As IOperacionesCatalogo.TiposVisualizacionCampos)

                _lineabasecatalogos.VisualizacionCamposConfigurada = value

            End Set

        End Property

        Public Property CadenaEncabezados As String _
            Implements IOperacionesCatalogo.CadenaEncabezados

            Get

                Return _lineabasecatalogos.CadenaEncabezados

            End Get

            Set(value As String)

                _lineabasecatalogos.CadenaEncabezados = value

            End Set

        End Property

        Public Property InstruccionesAdicionalesPieTransaccion As String _
            Implements IOperacionesCatalogo.InstruccionesAdicionalesPieTransaccion

            Get
                Return _lineabasecatalogos._declaracionesAdicionalesPieTransaccion

            End Get

            Set(value As String)

                _lineabasecatalogos._declaracionesAdicionalesPieTransaccion = value

            End Set
        End Property

        Public Property InstruccionesSQLAntesIniciarTransaccion As String _
            Implements IOperacionesCatalogo.InstruccionesSQLAntesIniciarTransaccion

            Get

                Return _lineabasecatalogos._instruccionesSQLAntesIniciarTransaccion

            End Get

            Set(value As String)

                _lineabasecatalogos._instruccionesSQLAntesIniciarTransaccion = value

            End Set

        End Property

        Public Property DeclaracionesAdicionalesUsuario As String _
            Implements IOperacionesCatalogo.DeclaracionesAdicionalesUsuario

            Get

                Return _lineabasecatalogos._declaracionesAdicionalesUsuario

            End Get

            Set(value As String)

                _lineabasecatalogos._declaracionesAdicionalesUsuario = value

            End Set
        End Property


        Public Property EjecutarPlanEjecucionTransaccional As Boolean _
            Implements IOperacionesCatalogo.EjecutarPlanEjecucionTransaccional

            Get

                Return _lineabasecatalogos._ejecutarPlanEjecucion

            End Get

            Set(value As Boolean)

                _lineabasecatalogos._ejecutarPlanEjecucion = value

            End Set

        End Property

        Public Property PlanEjecucionSQL As String _
            Implements IOperacionesCatalogo.PlanEjecucionSQL

            Get
                Return _lineabasecatalogos._planEjecucionSQL

            End Get

            Set(value As String)

                _lineabasecatalogos._planEjecucionSQL = value

            End Set

        End Property


        Public Property ComplejoTransaccional As IOperacionesCatalogo.ComplexTypes _
            Implements IOperacionesCatalogo.ComplejoTransaccional

            Get
                Return _lineabasecatalogos.ComplejoTransaccional

            End Get

            Set(value As IOperacionesCatalogo.ComplexTypes)

                _lineabasecatalogos.ComplejoTransaccional = value

            End Set

        End Property


        Public WriteOnly Property SetVista As DataSet _
            Implements IOperacionesCatalogo.SetVista

            Set(value As DataSet)

                _datasetvista = value

            End Set

        End Property

        Public Property AdvertenciasIndicador As Boolean _
            Implements IOperacionesCatalogo.AdvertenciasIndicador

            Get
                Return _advertenciasIndicador
            End Get

            Set(value As Boolean)
                _advertenciasIndicador = value
            End Set

        End Property

        Public Property TipoOperacionSQL As IOperacionesCatalogo.TiposOperacionSQL _
            Implements IOperacionesCatalogo.TipoOperacionSQL
            Get
                Return _lineabasecatalogos.TipoOperacionSQL
            End Get
            Set(value As IOperacionesCatalogo.TiposOperacionSQL)
                _lineabasecatalogos.TipoOperacionSQL = value
            End Set
        End Property

        Public Property IndiceTablaTemporalLlamante As String _
            Implements IOperacionesCatalogo.IndiceTablaTemporalLlamante
            Get
                Return _lineabasecatalogos.IndiceTablaTemporalLLamante
            End Get
            Set(value As String)
                _lineabasecatalogos.IndiceTablaTemporalLLamante = value
            End Set
        End Property


        Public Property IDNivelTransaccional As String _
            Implements IOperacionesCatalogo.IDNivelTransaccional
            Get
                Return _lineabasecatalogos.IDNivelTransaccional
            End Get
            Set(value As String)
                _lineabasecatalogos.IDNivelTransaccional = value
            End Set
        End Property

        Public Property IDObjetoTransaccional As String _
            Implements IOperacionesCatalogo.IDObjetoTransaccional
            Get
                Return _lineabasecatalogos.IDObjectoTransaccional
            End Get
            Set(value As String)
                _lineabasecatalogos.IDObjectoTransaccional = value
            End Set
        End Property

        Public Property IndiceTablaTemporal As Integer _
            Implements IOperacionesCatalogo.IndiceTablaTemporal
            Get
                Return _lineabasecatalogos.IndiceTablaTemporal
            End Get
            Set(value As Integer)
                _lineabasecatalogos.IndiceTablaTemporal = value
            End Set
        End Property



        Public Property SQLTransaccion As Dictionary(Of String, String) _
            Implements IOperacionesCatalogo.SQLTransaccion
            Get
                Return _lineabasecatalogos.SQLTransacccion
            End Get
            Set(value As Dictionary(Of String, String))
                _lineabasecatalogos.SQLTransacccion = value
            End Set
        End Property

        Public ReadOnly Property ColeccionInstrucciones As Dictionary(Of String, IOperacionesCatalogo) _
            Implements IOperacionesCatalogo.ColeccionInstrucciones
            Get
                Return _lineabasecatalogos.ColeccionInstrucciones
            End Get

        End Property

        Public Property ClausulasAutoFiltros As String _
            Implements IOperacionesCatalogo.ClausulasAutoFiltros
            Get
                Return _lineabasecatalogos.ClausulasAutoFiltro
            End Get
            Set(value As String)
                _lineabasecatalogos.ClausulasAutoFiltro = value
            End Set
        End Property

        'Nodo anterior
        Public Property OperacionAnterior As IOperacionesCatalogo _
        Implements IOperacionesCatalogo.OperacionAnterior

            Get

                Return _ioperacionanterior

            End Get

            Set(value As IOperacionesCatalogo)

                _ioperacionanterior = value

            End Set

        End Property

        Public Property CampoPorNombreAvanzado(
                nombrecampo_ As String,
                Optional tipoBusquedaCampo_ As IOperacionesCatalogo.TiposAccesoCampo = IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) As String _
         Implements IOperacionesCatalogo.CampoPorNombreAvanzado

            Get

                Return GesionCampo(nombrecampo_, tipoBusquedaCampo_)

            End Get

            Set(value As String)

                GesionCampo(nombrecampo_, value, tipoBusquedaCampo_)

            End Set

        End Property


        Public Property CampoPorNombre(ByVal nombrecampo_ As String) As String _
            Implements IOperacionesCatalogo.CampoPorNombre
            Get
                Return GesionCampo(nombrecampo_)

            End Get

            Set(value As String)

                GesionCampo(nombrecampo_, value)

            End Set
        End Property


        Public Property EntradaLlaves As String _
            Implements IOperacionesCatalogo.EntradaLlaves
            Get
                Return _lineabasecatalogos.EntradaLlaves
            End Get
            Set(value As String)
                _lineabasecatalogos.EntradaLlaves = value
            End Set
        End Property

        Public Property NoMostrarRegistrosInsertados As Boolean _
            Implements IOperacionesCatalogo.NoMostrarRegistrosInsertados
            Get
                Return _lineabasecatalogos.NoMostrarRegistrosInsertados
            End Get
            Set(value As Boolean)
                _lineabasecatalogos.NoMostrarRegistrosInsertados = value
            End Set
        End Property

        Public Property SalidaLlaves As String Implements _
            IOperacionesCatalogo.SalidaLlaves
            Get
                Return _lineabasecatalogos.SalidaLlaves
            End Get
            Set(value As String)
                _lineabasecatalogos.SalidaLlaves = value
            End Set
        End Property


        'Pila de registros remporales
        Public Property RegistrosTemporales As Dictionary(Of Integer, String) _
            Implements IOperacionesCatalogo.RegistrosTemporales
            Get
                Return _lineabasecatalogos.RegistrosTemporales
            End Get
            Set(value As Dictionary(Of Integer, String))
                _lineabasecatalogos.RegistrosTemporales = value
            End Set
        End Property

        'Pila de registros remporales, DataTable
        Public Property RegistrosTemporalesDataTable As DataTable _
            Implements IOperacionesCatalogo.RegistrosTemporalesDataTable
            Get
                Return _lineabasecatalogos.RegistrosTemporalesDataTable
            End Get
            Set(value As DataTable)
                _lineabasecatalogos.RegistrosTemporalesDataTable = value
            End Set
        End Property

        Public Property TipoEscritura As IOperacionesCatalogo.TiposEscritura _
            Implements IOperacionesCatalogo.TipoEscritura
            Get
                Return _lineabasecatalogos.TipoEscritura
            End Get
            Set(value As IOperacionesCatalogo.TiposEscritura)
                _lineabasecatalogos.TipoEscritura = value
            End Set
        End Property

        Public Property ClausulasLibres As String _
            Implements IOperacionesCatalogo.ClausulasLibres
            Get
                Return _lineabasecatalogos.ClausulasLibres
            End Get
            Set(value As String)
                _lineabasecatalogos.ClausulasLibres = value
            End Set

        End Property


        Public Property EspacioTrabajo As IEspacioTrabajo Implements IOperacionesCatalogo.EspacioTrabajo
            Get
                Return _lineabasecatalogos.EspacioTrabajo
            End Get
            Set(value As IEspacioTrabajo)
                _lineabasecatalogos.EspacioTrabajo = value
            End Set
        End Property
        Public Property ValorIndice As String _
            Implements IOperacionesCatalogo.ValorIndice
            Get
                Return _lineabasecatalogos.ValorIndice
            End Get
            Set(value As String)
                _lineabasecatalogos.ValorIndice = value
            End Set
        End Property

        Public Property IdentificadorEmpresa As String _
          Implements IOperacionesCatalogo.IdentificadorEmpresa
            Get
                Return _lineabasecatalogos.IdentificadorEmpresa
            End Get
            Set(ByVal value As String)
                _lineabasecatalogos.IdentificadorEmpresa = value
            End Set
        End Property


        Public Property TablaEdicion As String _
         Implements IOperacionesCatalogo.TablaEdicion
            Get
                Return _tablaedicion
            End Get
            Set(ByVal value As String)
                _tablaedicion = value
            End Set
        End Property

        Public Property OperadorCatalogoBorrado As String _
            Implements IOperacionesCatalogo.OperadorCatalogoBorrado
            Get
                Return _operadorcatalogoborrar
            End Get
            Set(ByVal value As String)
                _operadorcatalogoborrar = value
            End Set
        End Property

        Public Property OperadorCatalogoInsercion As String _
            Implements IOperacionesCatalogo.OperadorCatalogoInsercion
            Get
                Return _operadorcatalogoinsercion
            End Get
            Set(ByVal value As String)
                _operadorcatalogoinsercion = value
            End Set
        End Property

        Public Property OperadorCatalogoModificacion As String _
            Implements IOperacionesCatalogo.OperadorCatalogoModificacion
            Get
                Return _operadorcatalogomodificacion
            End Get
            Set(ByVal value As String)
                _operadorcatalogomodificacion = value
            End Set
        End Property

        Public Property OperadorCatalogoConsulta As String _
            Implements IOperacionesCatalogo.OperadorCatalogoConsulta
            Get
                Return _operadorcatalogoconsultas
            End Get
            Set(ByVal value As String)
                _operadorcatalogoconsultas = value
            End Set
        End Property

        Public Property VistaEncabezados As String _
            Implements IOperacionesCatalogo.VistaEncabezados
            Get
                Return _vistaencabezados
            End Get
            Set(ByVal value As String)
                _vistaencabezados = value
            End Set
        End Property

        Public Property SerieCatalogo As Integer _
            Implements IOperacionesCatalogo.SerieCatalogo
            Get
                Return _seriecatalogo
            End Get
            Set(ByVal value As Integer)
                _seriecatalogo = value
            End Set
        End Property

        Public Property CantidadVisibleRegistros As Integer _
          Implements IOperacionesCatalogo.CantidadVisibleRegistros
            Get
                Return _cantidadvisibleregistros
            End Get
            Set(ByVal value As Integer)
                _cantidadvisibleregistros = value

                _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros
            End Set
        End Property


        Public Property IdentificadorCatalogo As String _
        Implements IOperacionesCatalogo.IdentificadorCatalogo
            Get
                Return _identificadorcatalogo
            End Get
            Set(ByVal value As String)
                _identificadorcatalogo = value

            End Set

        End Property


        Public Property IndicePaginacion As Integer _
            Implements IOperacionesCatalogo.IndicePaginacion

            Get

                Return _indicepaginacion

            End Get

            Set(ByVal value As Integer)

                _indicepaginacion = value

            End Set

        End Property

        Public Property Nombre As String _
            Implements IOperacionesCatalogo.Nombre

            Get

                Return _nombre

            End Get

            Set(ByVal value As String)

                _nombre = value

            End Set

        End Property

        Public Property NombreToken As String _
            Implements IOperacionesCatalogo.NombreToken

            Get

                Return _nombreToken

            End Get

            Set(ByVal value As String)

                _nombreToken = value

            End Set

        End Property

        Public ReadOnly Property Vista(ByVal numeroFila_ As Int32,
                                       ByVal columna_ As String,
                                       Optional ByVal tipoAcceso_ As IOperacionesCatalogo.TiposAccesoCampo =
                                                IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico
                                       ) As String _
                                   Implements IOperacionesCatalogo.Vista
            Get

                Return CampoPorNombreVista(
                    numeroFila_,
                    columna_,
                    tipoAcceso_)

            End Get

        End Property

        Public Property ObjetoRepositorio As Object _
            Implements IOperacionesCatalogo.ObjetoRepositorio

            Get
                Return _objetoRepositorio

            End Get

            Set(value As Object)

                _objetoRepositorio = value

            End Set

        End Property

        Public Property ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta _
            Implements IOperacionesCatalogo.ModalidadConsulta
            Get
                Return _lineabasecatalogos.ModalidadConsulta
            End Get
            Set(value As IOperacionesCatalogo.ModalidadesConsulta)
                _lineabasecatalogos.ModalidadConsulta = value
            End Set
        End Property

        Public Property ModalidadConsultaAlternativa As IOperacionesCatalogo.ModalidadesConsulta _
            Implements IOperacionesCatalogo.ModalidadConsultaAlternativa
            Get
                Return _lineabasecatalogos.ModalidadConsultaAlternativa
            End Get
            Set(value As IOperacionesCatalogo.ModalidadesConsulta)
                _lineabasecatalogos.ModalidadConsultaAlternativa = value
            End Set
        End Property

        Function EjecutaInstrucciones() As Boolean Implements IOperacionesCatalogo.EjecutaInstrucciones

            Return _lineabasecatalogos.EjecutaInstrucciones()

        End Function

        Private Function CampoPorNombreVista(ByVal posicion_ As Int32,
                                             ByVal columna_ As String,
                                             Optional ByVal tipoAcceso_ As IOperacionesCatalogo.TiposAccesoCampo =
                                                IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) As String
            Dim resultado_ As String = Nothing

            If Me.Vista.Tables.Count > 0 Then

                If Me.Vista.Tables(0).Rows.Count > 0 Then

                    Select Case tipoAcceso_
                        Case IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado

                            If Not IsDBNull(Me.Vista.Tables(0).Rows(posicion_)(columna_)) Then

                                resultado_ = Me.Vista.Tables(0).Rows(posicion_)(columna_)

                            Else
                                resultado_ = ""

                            End If


                        Case Else
                            'NOT IMPLEMENTED
                            Dim conversionColumna_ As String = Nothing

                            'Validación especial para Dataset NoSQL, MOP 01142021
                            ' If Not _lineabasecatalogos.OrigenDatos = IConexiones.Controladores.MongoDB Then

                            conversionColumna_ = RetornaNombreDesplegable(columna_)

                            'Else

                            '    conversionColumna_ = columna_

                            'End If


                            If Not conversionColumna_ = "-1" Then

                                Dim sistema_ As New Organismo

                                If Not conversionColumna_ Is Nothing Then

                                    If Me.Vista.Tables(0).Columns.Contains(conversionColumna_) Then

                                        If Not IsDBNull(Me.Vista.Tables(0).Rows(posicion_)(conversionColumna_)) Then

                                            resultado_ = Me.Vista.Tables(0).Rows(posicion_)(conversionColumna_)

                                        Else

                                            resultado_ = ""

                                        End If

                                    Else
                                        sistema_.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_OperacionesCatalogo64,
                                                                  Wma.Exceptions.TagWatcher.ErrorTypes.C3_000_3003,
                                                                 "[No se encontró ningún campo con el nombre  [" & columna_ & "] en la dimensión [" & Me.OperadorCatalogoConsulta & "]")

                                    End If

                                Else

                                    sistema_.Watcher.SetError(IRecursosSistemas.RecursosCMF.Gsol_BaseDatos_Operaciones_OperacionesCatalogo64,
                                                                  Wma.Exceptions.TagWatcher.ErrorTypes.C3_000_3003,
                                                                 "[No se encontró ningún campo con el nombre  [" & columna_ & "] en la dimensión [" & Me.OperadorCatalogoConsulta & "]")


                                    'MsgBox("AVISO:No se encontró columna en la Vista [" & _
                                    '       Me.OperadorCatalogoConsulta & "], con el nombre técnico[" _
                                    '       & columna_ & "], revise sus datos por favor!")

                                End If

                            End If

                    End Select

                End If

            End If

            Return resultado_

        End Function

        Private Function RetornaNombreDesplegable(ByVal columnaNombreTecnico_ As String) As String
            Dim respuesta_ As String = Nothing

            If Me._lineabasecatalogos.Caracteristicas.Count > 0 Then

                For Each item_ As ICaracteristica In Me._lineabasecatalogos.Caracteristicas.Values

                    If item_.Nombre = columnaNombreTecnico_ Then

                        respuesta_ = item_.NombreMostrar

                    End If

                Next

            Else

                respuesta_ = "-1"

                MsgBox("AVISO: No se encontraron características cargadas en el objeto batería (Características) vacía[" &
                       "revise sus datos por favor!")

            End If

            Return respuesta_

        End Function



        Public ReadOnly Property Vista As DataSet _
            Implements IOperacionesCatalogo.Vista
            Get
                Return _datasetvista
            End Get
        End Property

        Public Property Caracteristicas As Dictionary(Of Integer, ICaracteristica) _
            Implements IOperacionesCatalogo.Caracteristicas
            Get
                Return _lineabasecatalogos.Caracteristicas
            End Get
            Set(ByVal value As Dictionary(Of Integer, ICaracteristica))
                _lineabasecatalogos.Caracteristicas = value
            End Set
        End Property


        Public WriteOnly Property OrdenarResultados(ByVal columnaNumero_ As Int32) As IOperacionesCatalogo.OrdenConsulta _
            Implements IOperacionesCatalogo.OrdenarResultados

            Set(value As IOperacionesCatalogo.OrdenConsulta)

                _lineabasecatalogos.OrdenarResultadosPorColumna = SetOrdenar(columnaNumero_, value)

            End Set

        End Property

        Private Function SetOrdenar(ByVal columnaNumero_ As Int32,
                                    ByVal value_ As IOperacionesCatalogo.OrdenConsulta)

            If columnaNumero_ > 0 And
                (value_ = IOperacionesCatalogo.OrdenConsulta.ASC Or
                 value_ = IOperacionesCatalogo.OrdenConsulta.DESC) Then

                Return " ORDER BY " & columnaNumero_ & " " & value_.ToString

            Else

                Return Nothing

            End If

        End Function

#End Region

#Region "Métodos"

        Public Function EjecutaPlanSQL() As Boolean _
            Implements IOperacionesCatalogo.EjecutaPlanSQL

            Return _lineabasecatalogos.EjecutaPlanSQL

        End Function


        'ReadOnly Property EditaCampoPorNombre(ByVal nombreCampo_ As String) As ICaracteristica
        '    Implements IOperacionesCatalogo.EditaCampoPorNombre
        '    Get
        '        Return RetornaCampoPorNombre(nombreCampo_)
        '    End Get

        '    'Set(value As ICaracteristica)

        '    '    AplicaCampoPorNombre(value)

        '    'End Set

        'End Property

        Function EditaCampoPorNombre(ByVal nombrecampo_ As String) As ICaracteristica _
            Implements IOperacionesCatalogo.EditaCampoPorNombre

            For Each caracteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                If caracteristica_.Nombre = nombrecampo_ Then

                    Return caracteristica_

                    'If Not esVisible Then

                    '    caracteristica_.Visible = ICaracteristica.TiposVisible.No

                    'End If

                    'caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No

                    'caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    Exit For

                End If

            Next

            Return Nothing

        End Function

        Public Sub EliminaCampoPorNombre(ByVal nombrecampo_ As String,
                                         Optional ByVal esVisible As Boolean = False) _
            Implements IOperacionesCatalogo.EliminaCampoPorNombre

            For Each caracteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                If caracteristica_.Nombre = nombrecampo_ Then

                    If Not esVisible Then

                        caracteristica_.Visible = ICaracteristica.TiposVisible.No

                    End If

                    caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No

                    caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    Exit For

                End If

            Next

        End Sub


        Public Function GrabarDatosEnDisco(Optional ByVal pilallaves_ As Dictionary(Of Integer, ICaracteristica) = Nothing,
                                           Optional ByVal tipoescritura_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia) As Boolean _
            Implements IOperacionesCatalogo.GrabarDatosEnDisco

            Return _lineabasecatalogos.GrabarDatosEnDisco(pilallaves_,
                                                          tipoescritura_)

        End Function

        Public Sub OcultaCamposGridView(ByRef gridview_ As DataGridView) _
            Implements IOperacionesCatalogo.OcultaCamposGridView

            _lineabasecatalogos.OcultaCamposGridView(gridview_)

        End Sub

        Private Function CargaCaracteristicas(ByRef datasetcaracteristicas_ As DataSet) As IOperacionesCatalogo.EstadoOperacion
            Dim indice_ As Int32 = 0

            If Not _lineabasecatalogos.Caracteristicas Is Nothing Then

                _lineabasecatalogos.Caracteristicas.Clear()

            Else
                _lineabasecatalogos.Caracteristicas = New Dictionary(Of Integer, ICaracteristica)

            End If

            '_datasetvista = datasetcaracteristicas_

            If datasetcaracteristicas_.Tables(0).Rows.Count > 0 Then
                For Each registro_ As DataRow In datasetcaracteristicas_.Tables(0).Rows
                    Dim icaracteristica_ As ICaracteristica = New CaracteristicaCatalogo

                    icaracteristica_.Llave = registro_("Llave")
                    icaracteristica_.Longitud = registro_("Longitud").ToString
                    icaracteristica_.Nombre = registro_("Nombre").ToString
                    icaracteristica_.NombreMostrar = registro_("NombreColumna").ToString
                    icaracteristica_.TipoDato = registro_("TipoDato")
                    icaracteristica_.Visible = registro_("Visible")

                    Try

                        If Not IsDBNull(registro_("NameAsKey")) Then

                            icaracteristica_.NameAsKey = registro_("NameAsKey")

                        End If

                        If Not IsDBNull(registro_("Interfaz")) Then

                            icaracteristica_.Interfaz = registro_("Interfaz")

                        End If


                    Catch ex As Exception

                        icaracteristica_.NameAsKey = Nothing
                        icaracteristica_.Interfaz = Nothing

                    End Try

                    Try


                        If Not IsDBNull(registro_("PermisoConsulta")) Then

                            icaracteristica_.PermisoConsulta = registro_("PermisoConsulta")

                        End If

                    Catch ex As Exception

                        icaracteristica_.PermisoConsulta = Nothing

                    End Try

                    'Avanzada
                    icaracteristica_.PuedeInsertar = registro_("PuedeInsertar")
                    icaracteristica_.PuedeModificar = registro_("PuedeModificar")

                    Try
                        icaracteristica_.TipoFiltro = registro_("TipoFiltro")
                        icaracteristica_.ValorFiltro = Nothing
                        'icaracteristica_.PermisoConsulta = registro_("PermisoConsulta")
                        'icaracteristica_.PermisoConsulta = Nothing

                    Catch ex As Exception
                        icaracteristica_.TipoFiltro = ICaracteristica.TiposFiltro.SinDefinir
                        icaracteristica_.ValorFiltro = Nothing
                        'icaracteristica_.PermisoConsulta = Nothing

                    End Try

                    Try

                        Dim algo As Object = registro_("Reflejar")

                        If Not IsDBNull(registro_("Reflejar")) Then

                            icaracteristica_.Reflejar = registro_("Reflejar")

                        End If

                    Catch ex As Exception

                        icaracteristica_.Reflejar = Nothing

                    End Try

                    icaracteristica_.ValorDefault = registro_("ValorDefault")

                    icaracteristica_.ValorAsignado = Nothing

                    'Añadiendo a la colección.
                    _lineabasecatalogos.Caracteristicas.Add(indice_, icaracteristica_)

                    indice_ += 1

                Next

                Return IOperacionesCatalogo.EstadoOperacion.COk

            Else

                Return IOperacionesCatalogo.EstadoOperacion.CVacio

            End If

        End Function

        Public Function PreparaCatalogo(Optional ByVal operacion_ As IOperacionesCatalogo.TiposOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.SinDefinir) As IOperacionesCatalogo.EstadoOperacion _
            Implements IOperacionesCatalogo.PreparaCatalogo

            Dim datasetencabezado_ As New DataSet

            _lineabasecatalogos.VistaEncabezados = _vistaencabezados
            _lineabasecatalogos.TablaEdicion = _tablaedicion
            _lineabasecatalogos.IdentificadorUnico = _identificadorcatalogo

            _lineabasecatalogos.OperadorCatalogoBorrado = _operadorcatalogoborrar
            _lineabasecatalogos.OperadorCatalogoConsultas = _operadorcatalogoconsultas
            _lineabasecatalogos.OperadorCatalogoInsercion = _operadorcatalogoinsercion
            _lineabasecatalogos.OperadorCatalogoModificacion = _operadorcatalogomodificacion

            'Verifica si ya se ha cargado antes la colección de caracteristicas
            If Not _lineabasecatalogos.Caracteristicas Is Nothing Then

                If _lineabasecatalogos.Caracteristicas.Values.Count > 0 And
                    operacion_ <> IOperacionesCatalogo.TiposOperacionSQL.ConsultaCaracteristicas Then

                    Return IOperacionesCatalogo.EstadoOperacion.COk

                Else

                    _lineabasecatalogos.Caracteristicas.Clear()

                    datasetencabezado_.Tables.Clear()

                    'Verifica que exista lo necesario para invocar al SP en la capa de datos
                    If (_vistaencabezados <> Nothing And
                        _operadorcatalogoconsultas <> Nothing) Then

                        datasetencabezado_ = _lineabasecatalogos.GeneraEncabezadosCatalogo()

                        If Not datasetencabezado_ Is Nothing Then

                            If Not datasetencabezado_.Tables Is Nothing Then

                                If datasetencabezado_.Tables.Count >= 1 Then

                                    If datasetencabezado_.Tables(0).Rows.Count > 0 Then

                                        If CargaCaracteristicas(datasetencabezado_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                            Return IOperacionesCatalogo.EstadoOperacion.COk

                                        End If

                                    End If

                                End If

                            End If

                        End If

                        Return IOperacionesCatalogo.EstadoOperacion.CVacio

                    Else

                        Return IOperacionesCatalogo.EstadoOperacion.CError
                    End If

                End If

            Else
                '_lineabasecatalogos.Caracteristicas.Clear()

                datasetencabezado_.Tables.Clear()

                'Verifica que exista lo necesario para invocar al SP en la capa de datos
                If (_vistaencabezados <> Nothing And
                    _operadorcatalogoconsultas <> Nothing) Then

                    datasetencabezado_ = _lineabasecatalogos.GeneraEncabezadosCatalogo()

                    If datasetencabezado_.Tables(0).Rows.Count > 0 Then

                        If CargaCaracteristicas(datasetencabezado_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                            Return IOperacionesCatalogo.EstadoOperacion.COk
                        Else
                            Return IOperacionesCatalogo.EstadoOperacion.CVacio
                        End If
                    Else
                        Return IOperacionesCatalogo.EstadoOperacion.CVacio
                    End If
                Else
                    Return IOperacionesCatalogo.EstadoOperacion.CError
                End If

            End If

        End Function

        'Revisar aquí... 

        Public Function GenerarVista(Optional ByVal valordefaultbuscado_ As String = "%",
                                     Optional ByVal filtrarcampo_ As String = Nothing,
                                     Optional ByVal rigor_ As String = "like") As IOperacionesCatalogo.EstadoOperacion _
        Implements IOperacionesCatalogo.GenerarVista

            Select Case _lineabasecatalogos.OrigenDatos

                Case IConexiones.Controladores.Automatico,
                     IConexiones.Controladores.SQLServer2008,
                     IConexiones.Controladores.MySQL51,
                     IConexiones.Controladores.SinDefinir

                    _datasetvista.Tables.Clear()

                    If PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                        _datasetvista = _lineabasecatalogos.ConsultaProduccion(valordefaultbuscado_,
                                                                               filtrarcampo_,
                                                                               rigor_)

                        Return IOperacionesCatalogo.EstadoOperacion.COk

                    Else

                        Return IOperacionesCatalogo.EstadoOperacion.CError

                    End If

                Case IConexiones.Controladores.MongoDB

                    _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                    _lineabasecatalogos.ClausulasLibres = ClausulasLibres

                    Select Case ObjetoDatos

                        Case IConexiones.TiposRepositorio.Automatico,
                            IConexiones.TiposRepositorio.BSONDocumentObject

                            _lineabasecatalogos.BSONDocumentResult = _lineabasecatalogos.ConsultaProduccionNoSQL()

                        Case IConexiones.TiposRepositorio.DataSetObject

                            _lineabasecatalogos.BSONDocumentResult = _lineabasecatalogos.ConsultaProduccionNoSQL()

                            _datasetvista.Tables.Clear()

                            BSONDocumentToDataSet(_datasetvista, _lineabasecatalogos.BSONDocumentResult)

                        Case IConexiones.TiposRepositorio.DataTableObject
                            'NOT IMPLEMENTED

                    End Select

                    Return IOperacionesCatalogo.EstadoOperacion.COk

                Case Else
                    'NOT IMPLEMENTED

            End Select


        End Function

        Public Async Function GenerarVistaAsync(Optional ByVal valordefaultbuscado_ As String = "%",
                                     Optional ByVal filtrarcampo_ As String = Nothing,
                                     Optional ByVal rigor_ As String = "like") As Threading.Tasks.Task(Of IOperacionesCatalogo.EstadoOperacion) _
        Implements IOperacionesCatalogo.GenerarVistaAsync

            Select Case _lineabasecatalogos.OrigenDatos

                Case IConexiones.Controladores.Automatico,
                     IConexiones.Controladores.SQLServer2008,
                     IConexiones.Controladores.MySQL51,
                     IConexiones.Controladores.SinDefinir

                    _datasetvista.Tables.Clear()

                    If PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                        _datasetvista = _lineabasecatalogos.ConsultaProduccion(valordefaultbuscado_,
                                                                               filtrarcampo_,
                                                                               rigor_)

                        Return IOperacionesCatalogo.EstadoOperacion.COk

                    Else

                        Return IOperacionesCatalogo.EstadoOperacion.CError

                    End If

                Case IConexiones.Controladores.MongoDB

                    _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                    _lineabasecatalogos.ClausulasLibres = ClausulasLibres

                    Select Case ObjetoDatos

                        Case IConexiones.TiposRepositorio.Automatico,
                            IConexiones.TiposRepositorio.BSONDocumentObject

                            _lineabasecatalogos.BSONDocumentResult = Await _lineabasecatalogos.ConsultaProduccionNoSQLAsync()

                        Case IConexiones.TiposRepositorio.DataSetObject

                            _lineabasecatalogos.BSONDocumentResult = Await _lineabasecatalogos.ConsultaProduccionNoSQLAsync()

                            _datasetvista.Tables.Clear()

                            'MsgBox(_lineabasecatalogos.EstructuraConsulta.Atributos.Count())

                            'BSONDocumentToDataSet(_datasetvista, _lineabasecatalogos.BSONDocumentResult)

                            _datasetvista = Await BSONDocumentToDataSetAsync(_lineabasecatalogos.BSONDocumentResult)

                        Case IConexiones.TiposRepositorio.DataTableObject
                            'NOT IMPLEMENTED

                    End Select

                    Return IOperacionesCatalogo.EstadoOperacion.COk

                Case Else
                    'NOT IMPLEMENTED

            End Select


        End Function

        Public Function GenerarVistaRESPALDO(Optional ByVal valordefaultbuscado_ As String = "%",
                                     Optional ByVal filtrarcampo_ As String = Nothing,
                                     Optional ByVal rigor_ As String = "like") As IOperacionesCatalogo.EstadoOperacion
            'Implements IOperacionesCatalogo.GenerarVista


            Select Case _lineabasecatalogos.OrigenDatos
                'Esquema tradicional RDBMS, SQL Server/MySQL
                Case IConexiones.Controladores.Automatico,
                     IConexiones.Controladores.SQLServer2008,
                     IConexiones.Controladores.MySQL51,
                     IConexiones.Controladores.SinDefinir

                    _datasetvista.Tables.Clear()

                    If PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        'NO es necesaria la transmision
                        '_lineabasecatalogos.Caracteristicas = _icaracteristicas

                        _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                        'Cursor en dataset
                        _datasetvista = _lineabasecatalogos.ConsultaProduccion(valordefaultbuscado_,
                                                                               filtrarcampo_,
                                                                               rigor_)


                        Return IOperacionesCatalogo.EstadoOperacion.COk

                    Else

                        Return IOperacionesCatalogo.EstadoOperacion.CError

                    End If

                    'Esquema NoSQL
                Case IConexiones.Controladores.MongoDB

                    _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                    _lineabasecatalogos.ClausulasLibres = ClausulasLibres

                    ' If PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta) = IOperacionesCatalogo.EstadoOperacion.COk Then

                    'NO es necesaria la transmision
                    '_lineabasecatalogos.Caracteristicas = _icaracteristicas

                    '#########TEMPORAL DE PRUEBAS ACTIVO##################################################################################################
                    '_lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                    'Dim queryDocument_ = New QueryDocument From {{"i_Cve_Estado", 0}, {"i_Cve_DivisionMiEmpresa", 7785}}


                    'Dim stdscoreQuery As New QueryDocument()
                    'Dim lteValue_ As BsonDocument = New BsonDocument("$lte", 0)

                    'Dim queryDocument2_ As New QueryDocument
                    'queryDocument2_.Add("i_Cve_Estado", lteValue_)
                    'queryDocument2_.Add("i_Cve_Estado", 0)
                    'queryDocument2_.Add("i_Cve_DivisionMiEmpresa", 7785)




                    ' ''Establecer un valor de campo
                    'Dim client = New MongoClient()
                    'Dim db = client.GetServer().GetDatabase("test")
                    'Dim coll = db.GetCollection("vbtest")
                    'Dim productQuery = query.EQ("productName", "test")
                    'Dim updateStmt = Update.Set("cost", 3000)
                    'coll.Update(productQuery, updateStmt)

                    'Dim queryX = query.And(query.EQ("author", "Kurt Vonnegut"), query.EQ("title", "Cats Craddle"))

                    '###########################################################################################################


                    ''Actualizar
                    'Dim filterById = Builders(Of BsonDocument).Filter.Eq(Of String)("_id", userId)
                    'userCollection.UpdateOne(filterById, New BsonDocument("$set", New BsonDocument("emailField", newEmail)))

                    'Conectarse a mongo como client
                    'Public client As MongoClient = New MongoClient("mongodb://user:password@IP:27017/MyDatabase")
                    'Public mydb As IMongoDatabase = client.GetDatabase("MyDatabase")
                    'Public userCollection As IMongoCollection(Of BsonDocument) = mydb.GetCollection(Of BsonDocument)("users")

                    '---------------
                    'Dim queryDocument3_ = New QueryDocument()
                    'queryDocument3_("_id") = New ObjectId("55cd5917522e341f20a3cbed")
                    'queryDocument3_("scNo") = "88888888"
                    'Dim oneData As BsonDocument = dbGather.FindOne(bsonQuery)

                    ''Busqueda con expresion regular
                    'Dim bsonQuery = New QueryDocument()
                    'Dim scNStr As String = "210302"
                    'Dim scNQuery As BsonElement = New BsonElement("sNo", New Regex("^" + scNStr + ".*$"))
                    'bsonQuery.Add(scNQuery)
                    'Dim oneData As BsonDocument = dbGather.FindOne(bsonQuery)

                    ''Recuperar si existe el campo
                    'Dim bsonDoc As BsonDocument = New BsonDocument
                    'bsonDoc.Add("$exists", True)
                    'answerSheetErrorQuery.Add("sNo", bsonDoc)

                    'Recuperar datos entre dos fechas
                    'Dim dateTo As Date = DateTimePicker1.Value
                    'Dim dateFromstr As String = DateTimePicker2.Value.ToString.Split(" ")(0)

                    'Dim dateFrom As Date = New DateTime(dateFromstr.Split("/")(0), dateFromstr.Split("/")(1), dateFromstr.Split("/")(2), 0, 0, 0)
                    'Dim schoolNo As String = TextBox1.Text
                    'Dim testNo As String = TextBox2.Text

                    'Dim stdscoreQuery As New QueryDocument()
                    'Dim fDate As BsonDocument = New BsonDocument("$lte", dateFrom)
                    'Dim tDate As BsonDocument = New BsonDocument("$gte", dateTo)

                    'Dim dateQuery As New QueryDocument()
                    'Dim df As BsonElement = New BsonElement("$lte", dateFrom)

                    'dateQuery.Add(df)
                    'Dim dt As BsonElement = New BsonElement("$gte", dateTo)

                    'dateQuery.Add(dt)
                    'stdscoreQuery.Add("UDT", dateQuery)

                    'Dim errorQuestion As MongoCursor(Of BsonDocument) = stdscore.FindOne(stdscoreQuery)

                    '-----------


                    'Dim bsonDocumentListResult_ As MongoCursor(Of BsonDocument)



                    Select Case ObjetoDatos

                        Case IConexiones.TiposRepositorio.Automatico,
                            IConexiones.TiposRepositorio.BSONDocumentObject

                            _lineabasecatalogos.BSONDocumentResult = _lineabasecatalogos.ConsultaProduccionNoSQL()

                        Case IConexiones.TiposRepositorio.DataSetObject

                            _lineabasecatalogos.BSONDocumentResult = _lineabasecatalogos.ConsultaProduccionNoSQL()

                            _datasetvista.Tables.Clear()

                            'Asignamos el dataset en caso de ser pedido así
                            BSONDocumentToDataSet(_datasetvista, _lineabasecatalogos.BSONDocumentResult)

                        Case IConexiones.TiposRepositorio.DataTableObject
                            'NOT IMPLEMENTED

                    End Select





                    'Return bsonDocumentListResult_

                    'Cursor en Query
                    '_datasetvista = _lineabasecatalogos.ConsultaProduccion(valordefaultbuscado_,
                    '                                                       filtrarcampo_, _
                    '                                                       rigor_)


                    Return IOperacionesCatalogo.EstadoOperacion.COk

                    'Else

                    '    Return IOperacionesCatalogo.EstadoOperacion.CError

                    'End If



                Case Else
                    'NOT IMPLEMENTED
                    'Watcher.SetError(TagWatcher.ErrorTypes.C2_000_2000)

            End Select


        End Function

        Public Sub BSONDocumentToDataSet(ByRef datasetvista_ As DataSet, ByRef bsonDocumentListResult_ As List(Of BsonDocument))

            datasetvista_.Clear()

            Dim dataTableObject_ As New DataTable(OperadorCatalogoConsulta)

            For Each campoVirtual_ As CampoVirtual In _lineabasecatalogos.EstructuraConsulta.Atributos

                dataTableObject_.Columns.Add(campoVirtual_.Atributo.ToString)

            Next

            For Each item As BsonDocument In bsonDocumentListResult_

                Dim dataRow_ As DataRow

                dataRow_ = dataTableObject_.NewRow

                For Each campo_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                    If Not campo_.Visible = ICaracteristica.TiposVisible.No And
                      Not campo_.Visible = ICaracteristica.TiposVisible.Virtual And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Informacion And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Impresion Then

                        If item.Contains(campo_.Nombre) Then

                            Dim campoElement_ As BsonElement = Nothing

                            campoElement_ = item.GetElement(campo_.Nombre)

                            Select Case campo_.TipoDato
                                Case ICaracteristica.TiposCaracteristica.cInt32,
                                     ICaracteristica.TiposCaracteristica.cReal

                                    dataRow_(campo_.Nombre) = campoElement_.Value

                                Case ICaracteristica.TiposCaracteristica.cString,
                                     ICaracteristica.TiposCaracteristica.cBoolean

                                    dataRow_(campo_.Nombre) = campoElement_.Value.ToString

                                Case ICaracteristica.TiposCaracteristica.cDateTime

                                    Dim fecha_ As DateTime = DateTime.Parse(campoElement_.Value.ToString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)

                                    dataRow_(campo_.Nombre) = fecha_.ToString("dd/MM/yyyy")

                            End Select

                        End If

                    End If

                Next

                dataTableObject_.Rows.Add(dataRow_)

            Next

            datasetvista_.Tables.Add(dataTableObject_)

        End Sub

        Public Async Function BSONDocumentToDataSetAsync(bsonDocumentListResult_ As List(Of BsonDocument)) As Threading.Tasks.Task(Of DataSet) 'MongoCursor(Of BsonDocument))

            Dim datasetvista_ As New DataSet

            Dim dataTableObject_ As New DataTable(OperadorCatalogoConsulta)

            For Each campoVirtual_ As CampoVirtual In _lineabasecatalogos.EstructuraConsulta.Atributos

                dataTableObject_.Columns.Add(campoVirtual_.Atributo.ToString)

            Next

            For Each item As BsonDocument In bsonDocumentListResult_

                Dim dataRow_ As DataRow

                dataRow_ = dataTableObject_.NewRow

                For Each campo_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                    If Not campo_.Visible = ICaracteristica.TiposVisible.No And
                      Not campo_.Visible = ICaracteristica.TiposVisible.Virtual And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Informacion And
                       Not campo_.Visible = ICaracteristica.TiposVisible.Impresion Then

                        If item.Contains(campo_.Nombre) Then

                            Dim campoElement_ As BsonElement = Nothing

                            campoElement_ = item.GetElement(campo_.Nombre)

                            Select Case campo_.TipoDato
                                Case ICaracteristica.TiposCaracteristica.cInt32,
                                     ICaracteristica.TiposCaracteristica.cReal

                                    dataRow_(campo_.Nombre) = campoElement_.Value

                                Case ICaracteristica.TiposCaracteristica.cString,
                                     ICaracteristica.TiposCaracteristica.cDateTime,
                                     ICaracteristica.TiposCaracteristica.cBoolean

                                    dataRow_(campo_.Nombre) = campoElement_.Value.ToString

                            End Select

                        End If

                    End If

                Next

                dataTableObject_.Rows.Add(dataRow_)

            Next

            datasetvista_.Tables.Add(dataTableObject_)

            Return datasetvista_

        End Function

        Public Function Eliminar(ByVal llaveprimaria_ As String) As IOperacionesCatalogo.EstadoOperacion _
            Implements IOperacionesCatalogo.Eliminar

            If _lineabasecatalogos.BorrarProduccion(llaveprimaria_, _lineabasecatalogos.IdentificadorEmpresa) Then
                'If _lineabasecatalogos.BorrarProduccion(llaveprimaria_, _lineabasecatalogos.EspacioTrabajo.MisCredenciales.DivisionEmpresaria) Then

                Return IOperacionesCatalogo.EstadoOperacion.COk
            Else

                Return IOperacionesCatalogo.EstadoOperacion.CError
            End If

        End Function

        Public Function ValidaValoresCampos(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL) As String _
            Implements IOperacionesCatalogo.ValidaValoresCampos

            For Each caracteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                Select Case tipooperacion_
                    Case IOperacionesCatalogo.TiposOperacionSQL.Insercion
                        If Not caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Then
                            Continue For
                        End If

                    Case IOperacionesCatalogo.TiposOperacionSQL.Modificar
                        If Not caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.Si Then
                            Continue For
                        End If

                End Select


                If (caracteristica_.ValorAsignado Is Nothing Or
                    caracteristica_.ValorAsignado = "") Then
                    Return "Aviso: capture por favor (" & caracteristica_.NombreMostrar & ") "
                Else
                    Select Case caracteristica_.TipoDato
                        Case ICaracteristica.TiposCaracteristica.cInt32, ICaracteristica.TiposCaracteristica.cReal
                            If Not EsNumerico(caracteristica_.ValorAsignado) Then
                                Return "Aviso: capture un valor númerico para (" & caracteristica_.NombreMostrar & ")"
                            End If
                        Case Else

                    End Select

                End If

            Next

            Return Nothing
        End Function

        Private Function EsNumerico(input As String) As Boolean
            If String.IsNullOrWhiteSpace(input) Then Return False
            If IsNumeric(input) Then Return True
            Dim partes_() As String = input.Split("/"c)
            If partes_.Length <> 2 Then Return False
            Return IsNumeric(partes_(0)) AndAlso IsNumeric(partes_(1))
        End Function

        Public Function Agregar() As IOperacionesCatalogo.EstadoOperacion _
            Implements IOperacionesCatalogo.Agregar

            'Entrega de caracteristicas a la linea base
            _lineabasecatalogos.Caracteristicas = _lineabasecatalogos.Caracteristicas

            _lineabasecatalogos.NombreToken = _nombreToken

            _lineabasecatalogos.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion

            If _lineabasecatalogos.InsertarProduccion(_lineabasecatalogos.IdentificadorEmpresa, False) Then

                Return IOperacionesCatalogo.EstadoOperacion.COk

            Else

                Return IOperacionesCatalogo.EstadoOperacion.CError

            End If

        End Function

        Public Function Agregar(ByVal grabarPlanEjecucion_ As Boolean) As IOperacionesCatalogo.EstadoOperacion _
         Implements IOperacionesCatalogo.Agregar

            'Entrega de caracteristicas a la linea base
            _lineabasecatalogos.Caracteristicas = _lineabasecatalogos.Caracteristicas

            _lineabasecatalogos.NombreToken = _nombreToken

            _lineabasecatalogos.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion

            If _lineabasecatalogos.InsertarProduccion(_lineabasecatalogos.IdentificadorEmpresa, grabarPlanEjecucion_) Then

                Return IOperacionesCatalogo.EstadoOperacion.COk

            Else

                Return IOperacionesCatalogo.EstadoOperacion.CError

            End If

        End Function

        'Public Function AgregarBulk() As IOperacionesCatalogo.EstadoOperacion _
        '    Implements IOperacionesCatalogo.AgregarBulk

        '    'Entrega de caracteristicas a la linea base
        '    '_lineabasecatalogos.Caracteristicas = _lineabasecatalogos.Caracteristicas
        '    If _lineabasecatalogos.InsertarProduccion(_lineabasecatalogos.IdentificadorEmpresa, False) Then
        '        Return IOperacionesCatalogo.EstadoOperacion.COk
        '    Else
        '        Return IOperacionesCatalogo.EstadoOperacion.CError
        '    End If

        'End Function


        Public Function EliminarBulk(ByVal llavesprimarias_ As List(Of String)) As IOperacionesCatalogo.EstadoOperacion Implements IOperacionesCatalogo.EliminarBulk

        End Function

        Public Function ModificarBulk(ByVal llavesprimarias_ As List(Of String)) As IOperacionesCatalogo.EstadoOperacion Implements IOperacionesCatalogo.ModificarBulk

        End Function

        Public Function Modificar(ByVal identificador_ As String) As IOperacionesCatalogo.EstadoOperacion _
         Implements IOperacionesCatalogo.Modificar

            _lineabasecatalogos.NombreToken = _nombreToken

            _lineabasecatalogos.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Modificar

            If _lineabasecatalogos.ModificarProduccion(identificador_) Then

                Return IOperacionesCatalogo.EstadoOperacion.COk

            Else

                Return IOperacionesCatalogo.EstadoOperacion.CError
            End If
        End Function

        Public Sub GesionCampo(ByVal nombrecampo_ As String,
                               ByVal valorcampo_ As String,
                               Optional ByVal tipoAccesoCampo_ As IOperacionesCatalogo.TiposAccesoCampo = IOperacionesCatalogo.TiposAccesoCampo.SinDefinir)

            Dim revision_ As Boolean = False

            For Each caraceteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                Select Case tipoAccesoCampo_

                    Case IOperacionesCatalogo.TiposAccesoCampo.SinDefinir,
                         IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico

                        If UCase(caraceteristica_.Nombre) = UCase(nombrecampo_) Then
                            caraceteristica_.ValorAsignado = valorcampo_
                            revision_ = True
                            Exit For
                        End If

                    Case IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado

                        If UCase(caraceteristica_.NombreMostrar) = UCase(nombrecampo_) Then
                            caraceteristica_.ValorAsignado = valorcampo_
                            revision_ = True
                            Exit For
                        End If

                End Select

            Next

            If Not revision_ Then
                If _advertenciasIndicador Then
                    MsgBox("El campo {" & nombrecampo_ & "}, no se encontró en la colección.")
                End If
            End If

        End Sub

        Public Function GesionCampo(ByVal nombrecampo_ As String,
                                    Optional ByVal tipoAccesoCampo_ As IOperacionesCatalogo.TiposAccesoCampo = IOperacionesCatalogo.TiposAccesoCampo.SinDefinir)

            Dim respuesta_ As String = Nothing

            For Each caraceteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                Select Case tipoAccesoCampo_

                    Case IOperacionesCatalogo.TiposAccesoCampo.SinDefinir,
                         IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico

                        If UCase(caraceteristica_.Nombre) = UCase(nombrecampo_) Then

                            respuesta_ = caraceteristica_.ValorAsignado

                            Exit For

                        End If

                    Case IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado

                        If UCase(caraceteristica_.NombreMostrar) = UCase(nombrecampo_) Then

                            respuesta_ = caraceteristica_.ValorAsignado

                            Exit For

                        End If

                End Select

            Next

            If respuesta_ Is Nothing Then
                If _advertenciasIndicador Then

                    MsgBox("El campo {" & nombrecampo_ & "} no se encontró ó esta vacío")

                End If
            End If

            Return respuesta_

        End Function



        '      Public Sub Assign(ByVal src As IClone) Implements ICloneable.Assign
        '          '1. Make sure src is pointing to a valid object.
        '          If Nothing Is src Then
        '              Throw New COMException("Invalid object.")
        '          End If

        '          '2. Verify the type of src.
        '          If Not (TypeOf src Is ClonableObjClass) Then
        '              Throw New COMException("Bad object type.")
        '          End If

        '          '3. Assign the properties of src to the current instance.
        '          Dim srcClonable As ClonableObjClass = CType(src, ClonableObjClass)
        '          m_name = srcClonable.Name
        '          m_version = srcClonable.Version
        '          m_ID = srcClonable.ID

        '          'Use shallow cloning (use a reference to the same member object).
        'm_spatialRef = spatialRef = srcClonable.SpatialReference)
        '      End Sub


        'Public Function CloneComplete() As Object Implements IOperacionesCatalogo.CloneComplete, ICloneable.Clone

        '    Dim OperacionesCatalogoClonada As IOperacionesCatalogo = New OperacionesCatalogo

        '    'OperacionesCatalogoClonada = Me

        '    OperacionesCatalogoClonada = Me

        '    Return OperacionesCatalogoClonada

        'End Function

        Public Function Clone() As Object Implements IOperacionesCatalogo.Clone, ICloneable.Clone

            'Creamos una instancia de mi mismo, es perfectamente posible

            Dim OperacionesCatalogoClonada As IOperacionesCatalogo = New OperacionesCatalogo



            'La copia tiene que ser miembro a miembro

            'OperacionesCatalogoClonada = Me

            OperacionesCatalogoClonada.Caracteristicas = Me.Caracteristicas

            OperacionesCatalogoClonada.EspacioTrabajo = Me.EspacioTrabajo

            OperacionesCatalogoClonada.OperadorCatalogoBorrado = Me.OperadorCatalogoBorrado

            OperacionesCatalogoClonada.OperadorCatalogoConsulta = Me.OperadorCatalogoConsulta

            OperacionesCatalogoClonada.OperadorCatalogoInsercion = Me.OperadorCatalogoInsercion

            OperacionesCatalogoClonada.OperadorCatalogoModificacion = Me.OperadorCatalogoModificacion

            OperacionesCatalogoClonada.ClausulasLibres = Me.ClausulasLibres

            OperacionesCatalogoClonada.Nombre = Me.Nombre

            OperacionesCatalogoClonada.CantidadVisibleRegistros = Me.CantidadVisibleRegistros

            OperacionesCatalogoClonada.VistaEncabezados = Me.VistaEncabezados


            OperacionesCatalogoClonada.IdentificadorCatalogo = Me.IdentificadorCatalogo

            OperacionesCatalogoClonada.IdentificadorEmpresa = Me.IdentificadorEmpresa

            '???
            OperacionesCatalogoClonada.OperacionAnterior = Me.OperacionAnterior

            OperacionesCatalogoClonada.TablaEdicion = Me.TablaEdicion

            OperacionesCatalogoClonada.TipoEscritura = Me.TipoEscritura

            OperacionesCatalogoClonada.SQLTransaccion = Me.SQLTransaccion


            'Avanzada

            OperacionesCatalogoClonada.RegistrosTemporalesDataTable = Me.RegistrosTemporalesDataTable

            OperacionesCatalogoClonada.Caracteristicas = Me.Caracteristicas

            OperacionesCatalogoClonada.ClausulasAutoFiltros = Me.ClausulasAutoFiltros


            OperacionesCatalogoClonada.IDNivelTransaccional = Me.IDNivelTransaccional

            OperacionesCatalogoClonada.IDObjetoTransaccional = Me.IDObjetoTransaccional

            OperacionesCatalogoClonada.IndiceTablaTemporal = Me.IndiceTablaTemporal

            OperacionesCatalogoClonada.NoMostrarRegistrosInsertados = Me.NoMostrarRegistrosInsertados

            'OperacionesCatalogoClonada.IndicePaginacion = Me.OperacionAnterior

            'OperacionesCatalogoClonada.OrdenarResultados = Me.OrdenarResultados

            OperacionesCatalogoClonada.RegistrosTemporales = Me.RegistrosTemporales

            OperacionesCatalogoClonada.ValorIndice = Me.ValorIndice

            'Nueva propiedad
            OperacionesCatalogoClonada.SetVista = Me.Vista

            'OperacionesCatalogoClonada.Vista.Tables.Add(Me.Vista.Tables(0))

            OperacionesCatalogoClonada.VistaEncabezados = Me.VistaEncabezados

            OperacionesCatalogoClonada.IndiceTablaTemporalLlamante = Me.IndiceTablaTemporalLlamante

            'Devolvemos el objeto que acabamos de crear

            OperacionesCatalogoClonada.ObjetoRepositorio = Me.ObjetoRepositorio

            Return OperacionesCatalogoClonada

        End Function


        Public Property NombreClaveUpsert As String Implements IOperacionesCatalogo.NombreClaveUpsert

            Get
                Return _lineabasecatalogos.NombreClaveUpsert

            End Get

            Set(value As String)

                _lineabasecatalogos.NombreClaveUpsert = value

            End Set


        End Property

        Public Property ActivarComodinesDeConsulta As Boolean Implements IOperacionesCatalogo.ActivarComodinesDeConsulta
            Get
                Return _lineabasecatalogos.ActivarComodinesDeConsulta
            End Get
            Set(value As Boolean)
                _lineabasecatalogos.ActivarComodinesDeConsulta = value
            End Set

        End Property


        Public Property KeyValues As List(Of String) Implements IOperacionesCatalogo.KeyValues

            Get
                Return _lineabasecatalogos.KeyVales

            End Get

            Set(value As List(Of String))

                _lineabasecatalogos.KeyVales = value

            End Set

        End Property

        Public Sub AgregaRegistroBulk(ByVal registroNuevo_ As RegistroIOperaciones) Implements IOperacionesCatalogo.AgregaRegistroBulk

            '_lineabasecatalogos.ListaRegistrosIOperaciones.Add(registroNuevo_)

            _lineabasecatalogos.ListaRegistrosIOperacionesAdd(registroNuevo_)

        End Sub


#End Region

        Public Property ObjetoDatos As IConexiones.TiposRepositorio _
            Implements IOperacionesCatalogo.ObjetoDatos
            Get
                Return _lineabasecatalogos.ObjetoDatos
            End Get
            Set(value As IConexiones.TiposRepositorio)
                _lineabasecatalogos.ObjetoDatos = value
            End Set
        End Property

        Public Property ObjetoDatosAlternativo As IConexiones.TiposRepositorio _
            Implements IOperacionesCatalogo.ObjetoDatosAlternativo
            Get
                Return _lineabasecatalogos.ObjetoDatosAlternativo
            End Get
            Set(value As IConexiones.TiposRepositorio)
                _lineabasecatalogos.ObjetoDatosAlternativo = value
            End Set
        End Property


        Public Property OrigenDatosAlternativo As IConexiones.Controladores Implements IOperacionesCatalogo.OrigenDatosAlternativo
            Get
                Return _lineabasecatalogos.OrigenDatosAlternativo
            End Get
            Set(value As IConexiones.Controladores)
                _lineabasecatalogos.OrigenDatosAlternativo = value
            End Set
        End Property

        Public Property OrigenDatos As IConexiones.Controladores Implements IOperacionesCatalogo.OrigenDatos
            Get
                Return _lineabasecatalogos.OrigenDatos
            End Get
            Set(value As IConexiones.Controladores)
                _lineabasecatalogos.OrigenDatos = value
            End Set
        End Property

        Public Property TipoConexion As IConexiones.TipoConexion Implements IOperacionesCatalogo.TipoConexion
            Get
                Return _lineabasecatalogos.TipoConexion
            End Get
            Set(value As IConexiones.TipoConexion)
                _lineabasecatalogos.TipoConexion = value
            End Set
        End Property

        Public Property TipoConexionAlternativa As IConexiones.TipoConexion Implements IOperacionesCatalogo.TipoConexionAlternativa
            Get
                Return _lineabasecatalogos.TipoConexionAlternativa
            End Get
            Set(value As IConexiones.TipoConexion)
                _lineabasecatalogos.TipoConexionAlternativa = value
            End Set
        End Property

        Public Property BSONDocumentResult As List(Of BsonDocument) _
            Implements IOperacionesCatalogo.BSONDocumentResult
            Get
                Return _lineabasecatalogos.BSONDocumentResult
            End Get
            Set(value As List(Of BsonDocument)) 'MongoCursor(Of BsonDocument))
                _lineabasecatalogos.BSONDocumentResult = value
            End Set
        End Property

        Public Property ActivarLecturaSuciaSQL As Boolean Implements IOperacionesCatalogo.ActivarLecturaSuciaSQL
            Get
                Return _lineabasecatalogos.ActivarLecturaSuciaSQL
            End Get
            Set(value As Boolean)
                _lineabasecatalogos.ActivarLecturaSuciaSQL = value
            End Set
        End Property

    End Class

End Namespace