' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
'Public Class OperacionesCatalogo
'    Implements IWSOperacionesCatalogo

'    Public Function GetData(ByVal value As Integer) As String Implements IWSOperacionesCatalogo.GetData
'        Return String.Format("You entered: {0}", value)
'    End Function

'    Public Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType Implements IWSOperacionesCatalogo.GetDataUsingDataContract
'        If composite Is Nothing Then
'            Throw New ArgumentNullException("composite")
'        End If
'        If composite.BoolValue Then
'            composite.StringValue &= "Suffix"
'        End If
'        Return composite
'    End Function

'End Class

'Imports System.Windows.Forms

Namespace Gsol.BaseDatos.Operaciones
    Public Class WSOperacionesCatalogo
        Implements IWSOperacionesCatalogo, ICloneable





#Region "Atributos"

        'Identificador de la empresa
        'Private _identificadorempresa As String

        '-------------------
        'Nombre común del catálogo
        Private _nombre As String

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

#End Region

#Region "Constructores"

        Sub New()
            ' _identificadorempresa = Nothing

            _nombre = Nothing
            _seriecatalogo = Nothing

            _tablaedicion = Nothing

            _vistaencabezados = Nothing

            _operadorcatalogoconsultas = Nothing

            _operadorcatalogoborrar = Nothing

            _operadorcatalogoinsercion = Nothing

            _operadorcatalogomodificacion = Nothing

            _cantidadvisibleregistros = 100

            _identificadorcatalogo = "-1"

            _indicepaginacion = 100

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

        Public Property InstruccionesAdicionalesPieTransaccion As String _
            Implements IWSOperacionesCatalogo.InstruccionesAdicionalesPieTransaccion

            Get
                Return _lineabasecatalogos._declaracionesAdicionalesPieTransaccion

            End Get

            Set(value As String)

                _lineabasecatalogos._declaracionesAdicionalesPieTransaccion = value

            End Set
        End Property

        Public Property InstruccionesSQLAntesIniciarTransaccion As String _
            Implements IWSOperacionesCatalogo.InstruccionesSQLAntesIniciarTransaccion

            Get

                Return _lineabasecatalogos._instruccionesSQLAntesIniciarTransaccion

            End Get

            Set(value As String)

                _lineabasecatalogos._instruccionesSQLAntesIniciarTransaccion = value

            End Set

        End Property

        Public Property DeclaracionesAdicionalesUsuario As String _
            Implements IWSOperacionesCatalogo.DeclaracionesAdicionalesUsuario

            Get

                Return _lineabasecatalogos._declaracionesAdicionalesUsuario

            End Get

            Set(value As String)

                _lineabasecatalogos._declaracionesAdicionalesUsuario = value

            End Set
        End Property


        Public Property EjecutarPlanEjecucionTransaccional As Boolean _
            Implements IWSOperacionesCatalogo.EjecutarPlanEjecucionTransaccional

            Get

                Return _lineabasecatalogos._ejecutarPlanEjecucion

            End Get

            Set(value As Boolean)

                _lineabasecatalogos._ejecutarPlanEjecucion = value

            End Set

        End Property

        Public Property PlanEjecucionSQL As String _
            Implements IWSOperacionesCatalogo.PlanEjecucionSQL

            Get
                Return _lineabasecatalogos._planEjecucionSQL

            End Get

            Set(value As String)

                _lineabasecatalogos._planEjecucionSQL = value

            End Set

        End Property


        Public Property ComplejoTransaccional As IWSOperacionesCatalogo.ComplexTypes _
            Implements IWSOperacionesCatalogo.ComplejoTransaccional

            Get
                Return _lineabasecatalogos.ComplejoTransaccional

            End Get

            Set(value As IWSOperacionesCatalogo.ComplexTypes)

                _lineabasecatalogos.ComplejoTransaccional = value

            End Set

        End Property


        Public WriteOnly Property SetVista As DataSet _
            Implements IWSOperacionesCatalogo.SetVista

            Set(value As DataSet)

                _datasetvista = value

            End Set

        End Property

        Public Property AdvertenciasIndicador As Boolean _
            Implements IWSOperacionesCatalogo.AdvertenciasIndicador

            Get
                Return _advertenciasIndicador
            End Get

            Set(value As Boolean)
                _advertenciasIndicador = value
            End Set

        End Property

        Public Property TipoOperacionSQL As IWSOperacionesCatalogo.TiposOperacionSQL _
            Implements IWSOperacionesCatalogo.TipoOperacionSQL
            Get
                Return _lineabasecatalogos.TipoOperacionSQL
            End Get
            Set(value As IWSOperacionesCatalogo.TiposOperacionSQL)
                _lineabasecatalogos.TipoOperacionSQL = value
            End Set
        End Property

        Public Property IndiceTablaTemporalLlamante As String _
            Implements IWSOperacionesCatalogo.IndiceTablaTemporalLlamante
            Get
                Return _lineabasecatalogos.IndiceTablaTemporalLLamante
            End Get
            Set(value As String)
                _lineabasecatalogos.IndiceTablaTemporalLLamante = value
            End Set
        End Property


        Public Property IDNivelTransaccional As String _
            Implements IWSOperacionesCatalogo.IDNivelTransaccional
            Get
                Return _lineabasecatalogos.IDNivelTransaccional
            End Get
            Set(value As String)
                _lineabasecatalogos.IDNivelTransaccional = value
            End Set
        End Property

        Public Property IDObjetoTransaccional As String _
            Implements IWSOperacionesCatalogo.IDObjetoTransaccional
            Get
                Return _lineabasecatalogos.IDObjectoTransaccional
            End Get
            Set(value As String)
                _lineabasecatalogos.IDObjectoTransaccional = value
            End Set
        End Property

        Public Property IndiceTablaTemporal As Integer _
            Implements IWSOperacionesCatalogo.IndiceTablaTemporal
            Get
                Return _lineabasecatalogos.IndiceTablaTemporal
            End Get
            Set(value As Integer)
                _lineabasecatalogos.IndiceTablaTemporal = value
            End Set
        End Property



        Public Property SQLTransaccion As Dictionary(Of String, String) _
            Implements IWSOperacionesCatalogo.SQLTransaccion
            Get
                Return _lineabasecatalogos.SQLTransacccion
            End Get
            Set(value As Dictionary(Of String, String))
                _lineabasecatalogos.SQLTransacccion = value
            End Set
        End Property


        Public ReadOnly Property ColeccionInstrucciones As Dictionary(Of String, IWSOperacionesCatalogo) Implements IWSOperacionesCatalogo.ColeccionInstrucciones
            Get
                Return Nothing
            End Get
        End Property

        'Bautismen
        'Public ReadOnly Property ColeccionInstrucciones As Dictionary(Of String, IWSOperacionesCatalogo) _
        '    Implements IWSOperacionesCatalogo.ColeccionInstrucciones
        '    Get
        '        Return _lineabasecatalogos.ColeccionInstrucciones
        '    End Get

        'End Property

        Public Property ClausulasAutoFiltros As String _
            Implements IWSOperacionesCatalogo.ClausulasAutoFiltros
            Get
                Return _lineabasecatalogos.ClausulasAutoFiltro
            End Get
            Set(value As String)
                _lineabasecatalogos.ClausulasAutoFiltro = value
            End Set
        End Property

        'Nodo anterior
        Public Property OperacionAnterior As IWSOperacionesCatalogo _
        Implements IWSOperacionesCatalogo.OperacionAnterior

            Get

                Return _ioperacionanterior

            End Get

            Set(value As IWSOperacionesCatalogo)

                _ioperacionanterior = value

            End Set

        End Property

        Public Property CampoPorNombreAvanzado( _
                nombrecampo_ As String, _
                Optional tipoBusquedaCampo_ As IWSOperacionesCatalogo.TiposAccesoCampo = IWSOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) As String _
         Implements IWSOperacionesCatalogo.CampoPorNombreAvanzado

            Get

                Return GesionCampo(nombrecampo_, tipoBusquedaCampo_)

            End Get

            Set(value As String)

                GesionCampo(nombrecampo_, value, tipoBusquedaCampo_)

            End Set

        End Property


        Public Property CampoPorNombre(ByVal nombrecampo_ As String) As String _
            Implements IWSOperacionesCatalogo.CampoPorNombre
            Get
                Return GesionCampo(nombrecampo_)

            End Get

            Set(value As String)

                GesionCampo(nombrecampo_, value)

            End Set
        End Property


        Public Property EntradaLlaves As String _
            Implements IWSOperacionesCatalogo.EntradaLlaves
            Get
                Return _lineabasecatalogos.EntradaLlaves
            End Get
            Set(value As String)
                _lineabasecatalogos.EntradaLlaves = value
            End Set
        End Property

        Public Property NoMostrarRegistrosInsertados As Boolean _
            Implements IWSOperacionesCatalogo.NoMostrarRegistrosInsertados
            Get
                Return _lineabasecatalogos.NoMostrarRegistrosInsertados
            End Get
            Set(value As Boolean)
                _lineabasecatalogos.NoMostrarRegistrosInsertados = value
            End Set
        End Property

        Public Property SalidaLlaves As String Implements _
            IWSOperacionesCatalogo.SalidaLlaves
            Get
                Return _lineabasecatalogos.SalidaLlaves
            End Get
            Set(value As String)
                _lineabasecatalogos.SalidaLlaves = value
            End Set
        End Property


        'Pila de registros remporales
        Public Property RegistrosTemporales As Dictionary(Of Integer, String) _
            Implements IWSOperacionesCatalogo.RegistrosTemporales
            Get
                Return _lineabasecatalogos.RegistrosTemporales
            End Get
            Set(value As Dictionary(Of Integer, String))
                _lineabasecatalogos.RegistrosTemporales = value
            End Set
        End Property

        'Pila de registros remporales, DataTable
        Public Property RegistrosTemporalesDataTable As DataTable _
            Implements IWSOperacionesCatalogo.RegistrosTemporalesDataTable
            Get
                Return _lineabasecatalogos.RegistrosTemporalesDataTable
            End Get
            Set(value As DataTable)
                _lineabasecatalogos.RegistrosTemporalesDataTable = value
            End Set
        End Property

        Public Property TipoEscritura As IWSOperacionesCatalogo.TiposEscritura _
            Implements IWSOperacionesCatalogo.TipoEscritura
            Get
                Return _lineabasecatalogos.TipoEscritura
            End Get
            Set(value As IWSOperacionesCatalogo.TiposEscritura)
                _lineabasecatalogos.TipoEscritura = value
            End Set
        End Property

        Public Property ClausulasLibres As String _
            Implements IWSOperacionesCatalogo.ClausulasLibres
            Get
                Return _lineabasecatalogos.ClausulasLibres
            End Get
            Set(value As String)
                _lineabasecatalogos.ClausulasLibres = value
            End Set

        End Property


        Public Property EspacioTrabajo As IEspacioTrabajo Implements IWSOperacionesCatalogo.EspacioTrabajo
            Get
                Return _lineabasecatalogos.EspacioTrabajo
            End Get
            Set(value As IEspacioTrabajo)
                _lineabasecatalogos.EspacioTrabajo = value
            End Set
        End Property
        Public Property ValorIndice As String _
            Implements IWSOperacionesCatalogo.ValorIndice
            Get
                Return _lineabasecatalogos.ValorIndice
            End Get
            Set(value As String)
                _lineabasecatalogos.ValorIndice = value
            End Set
        End Property

        Public Property IdentificadorEmpresa As String _
          Implements IWSOperacionesCatalogo.IdentificadorEmpresa
            Get
                Return _lineabasecatalogos.IdentificadorEmpresa
            End Get
            Set(ByVal value As String)
                _lineabasecatalogos.IdentificadorEmpresa = value
            End Set
        End Property


        Public Property TablaEdicion As String _
         Implements IWSOperacionesCatalogo.TablaEdicion
            Get
                Return _tablaedicion
            End Get
            Set(ByVal value As String)
                _tablaedicion = value
            End Set
        End Property

        Public Property OperadorCatalogoBorrado As String _
            Implements IWSOperacionesCatalogo.OperadorCatalogoBorrado
            Get
                Return _operadorcatalogoborrar
            End Get
            Set(ByVal value As String)
                _operadorcatalogoborrar = value
            End Set
        End Property

        Public Property OperadorCatalogoInsercion As String _
            Implements IWSOperacionesCatalogo.OperadorCatalogoInsercion
            Get
                Return _operadorcatalogoinsercion
            End Get
            Set(ByVal value As String)
                _operadorcatalogoinsercion = value
            End Set
        End Property

        Public Property OperadorCatalogoModificacion As String _
            Implements IWSOperacionesCatalogo.OperadorCatalogoModificacion
            Get
                Return _operadorcatalogomodificacion
            End Get
            Set(ByVal value As String)
                _operadorcatalogomodificacion = value
            End Set
        End Property

        Public Property OperadorCatalogoConsulta As String _
            Implements IWSOperacionesCatalogo.OperadorCatalogoConsulta
            Get
                Return _operadorcatalogoconsultas
            End Get
            Set(ByVal value As String)
                _operadorcatalogoconsultas = value
            End Set
        End Property

        Public Property VistaEncabezados As String _
            Implements IWSOperacionesCatalogo.VistaEncabezados
            Get
                Return _vistaencabezados
            End Get
            Set(ByVal value As String)
                _vistaencabezados = value
            End Set
        End Property

        Public Property SerieCatalogo As Integer _
            Implements IWSOperacionesCatalogo.SerieCatalogo
            Get
                Return _seriecatalogo
            End Get
            Set(ByVal value As Integer)
                _seriecatalogo = value
            End Set
        End Property

        Public Property CantidadVisibleRegistros As Integer _
          Implements IWSOperacionesCatalogo.CantidadVisibleRegistros
            Get
                Return _cantidadvisibleregistros
            End Get
            Set(ByVal value As Integer)
                _cantidadvisibleregistros = value
            End Set
        End Property


        Public Property IdentificadorCatalogo As String _
        Implements IWSOperacionesCatalogo.IdentificadorCatalogo
            Get
                Return _identificadorcatalogo
            End Get
            Set(ByVal value As String)
                _identificadorcatalogo = value

            End Set

        End Property


        Public Property IndicePaginacion As Integer _
            Implements IWSOperacionesCatalogo.IndicePaginacion
            Get
                Return _indicepaginacion
            End Get
            Set(ByVal value As Integer)
                _indicepaginacion = value
            End Set
        End Property

        Public Property Nombre As String _
            Implements IWSOperacionesCatalogo.Nombre
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property


        Public Property ObjetoRepositorio As Object _
            Implements IWSOperacionesCatalogo.ObjetoRepositorio

            Get
                Return _objetoRepositorio

            End Get

            Set(value As Object)

                _objetoRepositorio = value

            End Set
        End Property



        Function EjecutaInstrucciones() As Boolean Implements IWSOperacionesCatalogo.EjecutaInstrucciones

            Return _lineabasecatalogos.EjecutaInstrucciones()

        End Function

        Private Function CampoPorNombreVista(ByVal posicion_ As Int32, _
                                             ByVal columna_ As String, _
                                             Optional ByVal tipoAcceso_ As IWSOperacionesCatalogo.TiposAccesoCampo = _
                                                IWSOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) As String
            Dim resultado_ As String = Nothing

            If Me.Vista.Tables.Count > 0 Then

                If Me.Vista.Tables(0).Rows.Count > 0 Then

                    Select Case tipoAcceso_
                        Case IWSOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado

                            resultado_ = Me.Vista.Tables(0).Rows(posicion_)(columna_)

                        Case Else
                            'NOT IMPLEMENTED
                            Dim conversionColumna_ As String = Nothing

                            conversionColumna_ = RetornaNombreDesplegable(columna_)

                            If Not conversionColumna_ = "-1" Then

                                If Not conversionColumna_ Is Nothing Then

                                    If Not IsDBNull(Me.Vista.Tables(0).Rows(posicion_)(conversionColumna_)) Then

                                        resultado_ = Me.Vista.Tables(0).Rows(posicion_)(conversionColumna_)

                                    Else

                                        resultado_ = ""

                                    End If




                                Else

                                    MsgBox("AVISO:No se encontró columna en la Vista [" & _
                                           Me.OperadorCatalogoConsulta & "], con el nombre técnico[" _
                                           & columna_ & "], revise sus datos por favor!")

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

                MsgBox("AVISO: No se encontraron características cargadas en el objeto batería (Características) vacía[" & _
                       "revise sus datos por favor!")

            End If

            Return respuesta_

        End Function


        Public ReadOnly Property Vista(ByVal numeroFila_ As Int32, _
                               ByVal columna_ As String, _
                               Optional ByVal tipoAcceso_ As IWSOperacionesCatalogo.TiposAccesoCampo = _
                                        IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico
                               ) As String _
                           Implements IWSOperacionesCatalogo.Vista
            Get

                Return CampoPorNombreVista( _
                    numeroFila_, _
                    columna_, _
                    tipoAcceso_)

            End Get

        End Property



        Public ReadOnly Property Vista As DataSet _
            Implements IWSOperacionesCatalogo.Vista
            Get
                Return _datasetvista
            End Get
        End Property

        Public Property Caracteristicas As Dictionary(Of Integer, ICaracteristica) _
            Implements IWSOperacionesCatalogo.Caracteristicas
            Get
                Return _lineabasecatalogos.Caracteristicas
            End Get
            Set(ByVal value As Dictionary(Of Integer, ICaracteristica))
                _lineabasecatalogos.Caracteristicas = value
            End Set
        End Property


        Public WriteOnly Property OrdenarResultados(ByVal columnaNumero_ As Int32) As IWSOperacionesCatalogo.OrdenConsulta _
            Implements IWSOperacionesCatalogo.OrdenarResultados

            Set(value As IWSOperacionesCatalogo.OrdenConsulta)

                _lineabasecatalogos.OrdenarResultadosPorColumna = SetOrdenar(columnaNumero_, value)

            End Set

        End Property

        Private Function SetOrdenar(ByVal columnaNumero_ As Int32, _
                                    ByVal value_ As IWSOperacionesCatalogo.OrdenConsulta)

            If columnaNumero_ > 0 And _
                (value_ = IWSOperacionesCatalogo.OrdenConsulta.ASC Or _
                 value_ = IWSOperacionesCatalogo.OrdenConsulta.DESC) Then

                Return " ORDER BY " & columnaNumero_ & " " & value_.ToString

            Else

                Return Nothing

            End If

        End Function

#End Region

#Region "Métodos"




        Public Function EjecutaPlanSQL() As Boolean _
            Implements IWSOperacionesCatalogo.EjecutaPlanSQL

            Return _lineabasecatalogos.EjecutaPlanSQL

        End Function


        'ReadOnly Property EditaCampoPorNombre(ByVal nombreCampo_ As String) As ICaracteristica
        '    Implements IWSOperacionesCatalogo.EditaCampoPorNombre
        '    Get
        '        Return RetornaCampoPorNombre(nombreCampo_)
        '    End Get

        '    'Set(value As ICaracteristica)

        '    '    AplicaCampoPorNombre(value)

        '    'End Set

        'End Property

        Function EditaCampoPorNombre(ByVal nombrecampo_ As String) As ICaracteristica _
            Implements IWSOperacionesCatalogo.EditaCampoPorNombre

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

        Public Sub EliminaCampoPorNombre(ByVal nombrecampo_ As String, _
                                         Optional ByVal esVisible As Boolean = False) _
            Implements IWSOperacionesCatalogo.EliminaCampoPorNombre

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


        Public Function GrabarDatosEnDisco(Optional ByVal pilallaves_ As Dictionary(Of Integer, ICaracteristica) = Nothing, _
                                           Optional ByVal tipoescritura_ As IWSOperacionesCatalogo.TiposEscritura = IWSOperacionesCatalogo.TiposEscritura.MemoriaIntermedia) As Boolean _
            Implements IWSOperacionesCatalogo.GrabarDatosEnDisco

            Return _lineabasecatalogos.GrabarDatosEnDisco(pilallaves_,
                                                          tipoescritura_)

        End Function

        'Public Sub OcultaCamposGridView(ByRef gridview_ As DataGridView) _
        '    Implements IWSOperacionesCatalogo.OcultaCamposGridView

        '    _lineabasecatalogos.OcultaCamposGridView(gridview_)

        'End Sub

        Private Function CargaCaracteristicas(ByRef datasetcaracteristicas_ As DataSet) As IWSOperacionesCatalogo.EstadoOperacion
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

                    'Avanzada
                    icaracteristica_.PuedeInsertar = registro_("PuedeInsertar")
                    icaracteristica_.PuedeModificar = registro_("PuedeModificar")

                    Try
                        icaracteristica_.TipoFiltro = registro_("TipoFiltro")
                        icaracteristica_.ValorFiltro = Nothing
                        'icaracteristica_.PermisoConsulta = registro_("PermisoConsulta")
                        icaracteristica_.PermisoConsulta = Nothing

                    Catch ex As Exception
                        icaracteristica_.TipoFiltro = ICaracteristica.TiposFiltro.SinDefinir
                        icaracteristica_.ValorFiltro = Nothing
                        icaracteristica_.PermisoConsulta = Nothing

                    End Try


                    icaracteristica_.ValorDefault = registro_("ValorDefault")
                    icaracteristica_.ValorAsignado = Nothing

                    'Añadiendo a la colección.
                    _lineabasecatalogos.Caracteristicas.Add(indice_, icaracteristica_)
                    indice_ += 1
                Next
                Return IWSOperacionesCatalogo.EstadoOperacion.COk
            Else
                Return IWSOperacionesCatalogo.EstadoOperacion.CVacio
            End If

        End Function

        Public Function PreparaCatalogo(Optional ByVal operacion_ As IWSOperacionesCatalogo.TiposOperacionSQL = IWSOperacionesCatalogo.TiposOperacionSQL.SinDefinir) As IWSOperacionesCatalogo.EstadoOperacion _
            Implements IWSOperacionesCatalogo.PreparaCatalogo

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

                If _lineabasecatalogos.Caracteristicas.Values.Count > 0 And _
                    operacion_ <> IWSOperacionesCatalogo.TiposOperacionSQL.ConsultaCaracteristicas Then

                    Return IWSOperacionesCatalogo.EstadoOperacion.COk

                Else

                    _lineabasecatalogos.Caracteristicas.Clear()

                    datasetencabezado_.Tables.Clear()

                    'Verifica que exista lo necesario para invocar al SP en la capa de datos
                    If (_vistaencabezados <> Nothing And _
                        _operadorcatalogoconsultas <> Nothing) Then

                        datasetencabezado_ = _lineabasecatalogos.GeneraEncabezadosCatalogo()

                        If datasetencabezado_.Tables(0).Rows.Count > 0 Then

                            If CargaCaracteristicas(datasetencabezado_) = IWSOperacionesCatalogo.EstadoOperacion.COk Then

                                Return IWSOperacionesCatalogo.EstadoOperacion.COk
                            Else
                                Return IWSOperacionesCatalogo.EstadoOperacion.CVacio
                            End If
                        Else
                            Return IWSOperacionesCatalogo.EstadoOperacion.CVacio
                        End If
                    Else
                        Return IWSOperacionesCatalogo.EstadoOperacion.CError
                    End If

                End If

            Else
                '_lineabasecatalogos.Caracteristicas.Clear()

                datasetencabezado_.Tables.Clear()

                'Verifica que exista lo necesario para invocar al SP en la capa de datos
                If (_vistaencabezados <> Nothing And _
                    _operadorcatalogoconsultas <> Nothing) Then

                    datasetencabezado_ = _lineabasecatalogos.GeneraEncabezadosCatalogo()

                    If datasetencabezado_.Tables(0).Rows.Count > 0 Then

                        If CargaCaracteristicas(datasetencabezado_) = IWSOperacionesCatalogo.EstadoOperacion.COk Then

                            Return IWSOperacionesCatalogo.EstadoOperacion.COk
                        Else
                            Return IWSOperacionesCatalogo.EstadoOperacion.CVacio
                        End If
                    Else
                        Return IWSOperacionesCatalogo.EstadoOperacion.CVacio
                    End If
                Else
                    Return IWSOperacionesCatalogo.EstadoOperacion.CError
                End If

            End If




        End Function



        Public Function GenerarVista(Optional ByVal valordefaultbuscado_ As String = "%", _
                                     Optional ByVal filtrarcampo_ As String = Nothing, _
                                     Optional ByVal rigor_ As String = "like") As IWSOperacionesCatalogo.EstadoOperacion _
        Implements IWSOperacionesCatalogo.GenerarVista

            _datasetvista.Tables.Clear()

            If PreparaCatalogo(IWSOperacionesCatalogo.TiposOperacionSQL.Consulta) = IWSOperacionesCatalogo.EstadoOperacion.COk Then

                'NO es necesaria la transmision
                '_lineabasecatalogos.Caracteristicas = _icaracteristicas

                _lineabasecatalogos.CantidadVisibleRegistros = _cantidadvisibleregistros

                'Cursor para GridView
                '_datasetvista = New DataSet
                _datasetvista = _lineabasecatalogos.ConsultaProduccion(valordefaultbuscado_,
                                                                       filtrarcampo_, _
                                                                       rigor_)



                '_lineabasecatalogos.ConexionSingleton = _lineabasecatalogos.ConexionSingleton.ObtenerInstancia
                '_lineabasecatalogos.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(Nothing)

                '_datasetvista = _lineabasecatalogos.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente
                Return IWSOperacionesCatalogo.EstadoOperacion.COk
            Else
                Return IWSOperacionesCatalogo.EstadoOperacion.CError

            End If

            '_lineabasecatalogos.CortaConexion()
        End Function

        Public Function Eliminar(ByVal llaveprimaria_ As String) As IWSOperacionesCatalogo.EstadoOperacion _
            Implements IWSOperacionesCatalogo.Eliminar

            If _lineabasecatalogos.BorrarProduccion(llaveprimaria_, _lineabasecatalogos.IdentificadorEmpresa) Then

                Return IWSOperacionesCatalogo.EstadoOperacion.COk
            Else

                Return IWSOperacionesCatalogo.EstadoOperacion.CError
            End If

        End Function

        Public Function ValidaValoresCampos(ByVal tipooperacion_ As IWSOperacionesCatalogo.TiposOperacionSQL) As String _
            Implements IWSOperacionesCatalogo.ValidaValoresCampos

            For Each caracteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                Select Case tipooperacion_
                    Case IWSOperacionesCatalogo.TiposOperacionSQL.Insercion
                        If Not caracteristica_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Then
                            Continue For
                        End If

                    Case IWSOperacionesCatalogo.TiposOperacionSQL.Modificar
                        If Not caracteristica_.PuedeModificar = ICaracteristica.TiposRigorDatos.Si Then
                            Continue For
                        End If

                End Select


                If (caracteristica_.ValorAsignado Is Nothing Or _
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

        Public Function Agregar3() As IWSOperacionesCatalogo.EstadoOperacion _
            Implements IWSOperacionesCatalogo.Agregar3

            'Entrega de caracteristicas a la linea base
            _lineabasecatalogos.Caracteristicas = _lineabasecatalogos.Caracteristicas

            If _lineabasecatalogos.InsertarProduccion(_lineabasecatalogos.IdentificadorEmpresa, False) Then
                Return IWSOperacionesCatalogo.EstadoOperacion.COk
            Else
                Return IWSOperacionesCatalogo.EstadoOperacion.CError
            End If

        End Function

        Public Function Agregar(ByVal grabarPlanEjecucion_ As Boolean) As IWSOperacionesCatalogo.EstadoOperacion _
         Implements IWSOperacionesCatalogo.Agregar

            'Entrega de caracteristicas a la linea base
            _lineabasecatalogos.Caracteristicas = _lineabasecatalogos.Caracteristicas


            If _lineabasecatalogos.InsertarProduccion(_lineabasecatalogos.IdentificadorEmpresa, grabarPlanEjecucion_) Then
                Return IWSOperacionesCatalogo.EstadoOperacion.COk
            Else
                Return IWSOperacionesCatalogo.EstadoOperacion.CError
            End If


        End Function

        Public Function Modificar(ByVal identificador_ As String) As IWSOperacionesCatalogo.EstadoOperacion _
         Implements IWSOperacionesCatalogo.Modificar

            If _lineabasecatalogos.ModificarProduccion(identificador_) Then

                Return IWSOperacionesCatalogo.EstadoOperacion.COk



            Else

                Return IWSOperacionesCatalogo.EstadoOperacion.CError
            End If
        End Function

        Public Sub GesionCampo(ByVal nombrecampo_ As String, _
                               ByVal valorcampo_ As String, _
                               Optional ByVal tipoAccesoCampo_ As IWSOperacionesCatalogo.TiposAccesoCampo = IWSOperacionesCatalogo.TiposAccesoCampo.SinDefinir)

            Dim revision_ As Boolean = False

            For Each caraceteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                Select Case tipoAccesoCampo_

                    Case IWSOperacionesCatalogo.TiposAccesoCampo.SinDefinir, _
                         IWSOperacionesCatalogo.TiposAccesoCampo.NombreTecnico

                        If UCase(caraceteristica_.Nombre) = UCase(nombrecampo_) Then
                            caraceteristica_.ValorAsignado = valorcampo_
                            revision_ = True
                            Exit For
                        End If

                    Case IWSOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado

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
                                    Optional ByVal tipoAccesoCampo_ As IWSOperacionesCatalogo.TiposAccesoCampo = IWSOperacionesCatalogo.TiposAccesoCampo.SinDefinir)

            Dim respuesta_ As String = Nothing

            For Each caraceteristica_ As ICaracteristica In _lineabasecatalogos.Caracteristicas.Values

                Select Case tipoAccesoCampo_

                    Case IWSOperacionesCatalogo.TiposAccesoCampo.SinDefinir, _
                         IWSOperacionesCatalogo.TiposAccesoCampo.NombreTecnico

                        If UCase(caraceteristica_.Nombre) = UCase(nombrecampo_) Then

                            respuesta_ = caraceteristica_.ValorAsignado

                            Exit For

                        End If

                    Case IWSOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado

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

#End Region

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


        'Public Function CloneComplete() As Object Implements IWSOperacionesCatalogo.CloneComplete, ICloneable.Clone

        '    Dim OperacionesCatalogoClonada As IWSOperacionesCatalogo = New OperacionesCatalogo

        '    'OperacionesCatalogoClonada = Me

        '    OperacionesCatalogoClonada = Me

        '    Return OperacionesCatalogoClonada

        'End Function

        Public Function Clone() As Object Implements IWSOperacionesCatalogo.Clone, ICloneable.Clone

            'Creamos una instancia de mi mismo, es perfectamente posible

            Dim OperacionesCatalogoClonada As IWSOperacionesCatalogo = New OperacionesCatalogo



            'La copia tiene que ser miembro a miembro

            'OperacionesCatalogoClonada = Me

            OperacionesCatalogoClonada.Caracteristicas = Me.Caracteristicas

            OperacionesCatalogoClonada.EspacioTrabajo = Me.EspacioTrabajo

            OperacionesCatalogoClonada.OperadorCatalogoBorrado = Me.OperadorCatalogoBorrado

            OperacionesCatalogoClonada.OperadorCatalogoConsulta = Me.OperadorCatalogoConsulta

            OperacionesCatalogoClonada.OperadorCatalogoInsercion = Me.OperadorCatalogoInsercion

            OperacionesCatalogoClonada.OperadorCatalogoModificacion = Me.OperadorCatalogoModificacion

            OperacionesCatalogoClonada.ClausulasLibres = Me.ClausulasLibres

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

            Return OperacionesCatalogoClonada
        End Function









    End Class
End Namespace

