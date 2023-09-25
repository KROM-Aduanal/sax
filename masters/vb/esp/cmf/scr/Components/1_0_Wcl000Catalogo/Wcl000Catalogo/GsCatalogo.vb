Imports gsol.BaseDatos.Operaciones
Imports System.Reflection
Imports gsol
Imports Wma.Reports
Imports Wma.Components
Imports System.Threading
Imports Wma.Exceptions

Public Class GsCatalogo
    Inherits UserControl


#Region "Atributos"

    Private _continuarOperacionActual As Boolean

    Private _ioperacionescatalogo As IOperacionesCatalogo

    Private _operacioninvocada As IOperacionesCatalogo.TiposOperacionSQL

    Private _preparado As Boolean

    Private _tipoensamblado As String

    Private _moduloensamblado As [Assembly]

    Private _modulodinamicoEmbebido As Object

    Private _rutamoduloensamblado As String

    Private _tipocarga As Type

    Private _instanciagenerica As Object

    Private _iespaciotrabajo As IEspacioTrabajo

    Private _dialogos As New Organismo

    Private _campollave As String

    Private _templateExcel As String

    Private _pathsaveexcel As String

    Private _activacionControles As Boolean

    Delegate Sub SetCatalogoDataSourceCallback([datatable] As DataTable)

    Delegate Sub SetVisibleCargadorCallback([visible] As Boolean)

    Private _procesoBuscar As Thread

    Private _disposed As Boolean = False

    Private _modalidadBusquedas As IOperacionesCatalogo.ModalidadesBusqueda

    Private _catalogoOpcional As Boolean

    Private _mostrarBarraEstado As Boolean

    Private _coleccionMarcadores As List(Of MarcadorCelda)

    Private _ignorarDobleClickContenidoCelda As Boolean = False

    Private _activarGestor As Boolean = True

    ' Public WithEvents _contexMenuStrip As ContextMenuStrip

    Private _campoExpedienteOperativo As String

    Private _campoExpedienteAdministrativo As String

    '12/08/2020 cambios COVID, MOP T.T seguimos en cuarentena

    Private _ventanasIndependientes As Boolean

    Private _activarcontrolventanasIndependientes As Boolean = False

    Private _longitudBusquedaAutomatica As Integer = 3

    Private _contadorDeRegistrosParaInsercion As Int32 = 0

    Private WithEvents _listaDeResultadosBusqueda As TextboxResult

    Private _sistema As New Organismo

    Public Enum OpcionesMenuContextual
        SeleccionMultiple = 0
        Espacio1 = 1

        Modificar = 2
        Eliminar = 3
        Espacio2 = 4

        OtrasOpciones1 = 5
        OtrasOpciones2 = 6
        OtrasOpciones3 = 7
        OtrasOpciones4 = 8
        Espacio3 = 9

        DocumentosTrafico = 10
        DocumentosAdministracion = 11

        OtrasOpciones5 = 12
        OtrasOpciones6 = 13
        OtrasOpciones7 = 14
        OtrasOpciones8 = 15

    End Enum

#End Region

#Region "Eventos"

    Public Event EventoAntesTsAgregar()

    Public Event EventoDespuesTsAgregar()

    Public Event EventoAntesTsModificar()

    Public Event EventoDespuesTsModificar()

    Public Event EventoDespuesTsEliminar()

    Public Event EventoAntesTsEliminar()

    Public Event EventoDespuesTsBuscar()

    Public Event EventoAntesTsBuscar()

    Public Event DobleClickContenidoCelda()


    'Menu contextual
    Public Event Item1MenuContextualClick()

    Public Event Item2MenuContextualClick()

    Public Event Item3MenuContextualClick()

    Public Event Item4MenuContextualClick()

    'Otros eventos
    Public Event Item5MenuContextualClick()

    Public Event Item6MenuContextualClick()

    Public Event Item7MenuContextualClick()

    Public Event Item8MenuContextualClick()


#End Region

#Region "Constructores"

    Public Structure MarcadorCelda
        Public NombreColumna As String
        Public CuandoValorIgualA As String
        Public AplicarColorFondoCeldas As Color
        Public AplicarColorFondoTexto As Color
    End Structure

    Sub New()

        InitializeComponent()

        _coleccionMarcadores = New List(Of MarcadorCelda)

        _continuarOperacionActual = True

        _ioperacionescatalogo = New OperacionesCatalogo

        _operacioninvocada = IOperacionesCatalogo.TiposOperacionSQL.Consulta

        _preparado = False

        _tipoensamblado = Nothing

        _rutamoduloensamblado = Nothing

        _instanciagenerica = Nothing

        _iespaciotrabajo = New EspacioTrabajo

        _campollave = Nothing

        _templateExcel = ""

        _pathsaveexcel = ""

        _activacionControles = True

        _modalidadBusquedas = IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir

        _catalogoOpcional = False

        _mostrarBarraEstado = False

        ssEstado.Visible = False

        'Selección múltiple
        Me.cmsMenuContextual.Items(0).Visible = False

        'space
        Me.cmsMenuContextual.Items(1).Visible = False


        'Modificar
        Me.cmsMenuContextual.Items(2).Visible = True

        'Eliminar
        Me.cmsMenuContextual.Items(3).Visible = True


        'space
        Me.cmsMenuContextual.Items(4).Visible = False

        Me.cmsMenuContextual.Items(5).Text = "Opcion 1"
        Me.cmsMenuContextual.Items(5).Visible = False

        Me.cmsMenuContextual.Items(6).Text = "Opcion 2"
        Me.cmsMenuContextual.Items(6).Visible = False

        Me.cmsMenuContextual.Items(7).Text = "Opcion 3"
        Me.cmsMenuContextual.Items(7).Visible = False

        Me.cmsMenuContextual.Items(8).Text = "Opcion 4"
        Me.cmsMenuContextual.Items(8).Visible = False

        'space
        Me.cmsMenuContextual.Items(9).Visible = False

        'Documentos tráfico
        Me.cmsMenuContextual.Items(10).Visible = True

        'Documentos administración
        Me.cmsMenuContextual.Items(11).Visible = True

        _campoExpedienteOperativo = "t_Referencia"

        _campoExpedienteAdministrativo = "t_FolioFactura"

        'OtrasOpciones5 = 12
        Me.cmsMenuContextual.Items(12).Text = "Opcion 12"
        Me.cmsMenuContextual.Items(12).Visible = False

        'OtrasOpciones6 = 13
        Me.cmsMenuContextual.Items(13).Text = "Opcion 13"
        Me.cmsMenuContextual.Items(13).Visible = False

        'OtrasOpciones7 = 14
        Me.cmsMenuContextual.Items(14).Text = "Opcion 14"
        Me.cmsMenuContextual.Items(14).Visible = False

        'OtrasOpciones8 = 15
        Me.cmsMenuContextual.Items(15).Text = "Opcion 15"
        Me.cmsMenuContextual.Items(15).Visible = False

        pnlGestor.Visible = False
        pnlGrid.Visible = True

        _activarcontrolventanasIndependientes = False

        tsbModo.Enabled = _activarcontrolventanasIndependientes

        _ventanasIndependientes = True

    End Sub

    Sub New(ByVal iespaciotrabajo_ As IEspacioTrabajo)

        InitializeComponent()

        _continuarOperacionActual = True

        _ioperacionescatalogo = New OperacionesCatalogo

        _operacioninvocada = IOperacionesCatalogo.TiposOperacionSQL.Consulta

        _preparado = False

        _tipoensamblado = Nothing

        _rutamoduloensamblado = Nothing

        _instanciagenerica = Nothing

        _iespaciotrabajo = New EspacioTrabajo

        _iespaciotrabajo = iespaciotrabajo_

        _campollave = Nothing

        _templateExcel = ""

        _pathsaveexcel = ""

        _activacionControles = False

        _modalidadBusquedas = IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir

        _mostrarBarraEstado = False

        ssEstado.Visible = False

        'Configuración por defecto menu contextual

        'SeleccionMultiple = 0
        'Insertar = 1
        'Modificar = 2
        'Eliminar = 3
        'Espacio1 = 4
        'DocumentoReferencia = 5
        'DocumentoCuentaGastos = 6
        'Espacio2 = 7
        'OtrasOpciones1 = 8
        'OtrasOpciones2 = 9
        'OtrasOpciones3 = 10
        'OtrasOpciones4 = 11

        'Selección múltiple
        Me.cmsMenuContextual.Items(0).Visible = False

        'space
        Me.cmsMenuContextual.Items(1).Visible = False


        'Modificar
        Me.cmsMenuContextual.Items(2).Visible = True

        'Eliminar
        Me.cmsMenuContextual.Items(3).Visible = True

        'space
        Me.cmsMenuContextual.Items(4).Visible = False

        Me.cmsMenuContextual.Items(5).Text = "Opcion 1"
        Me.cmsMenuContextual.Items(5).Visible = False

        Me.cmsMenuContextual.Items(6).Text = "Opcion 2"
        Me.cmsMenuContextual.Items(6).Visible = False

        Me.cmsMenuContextual.Items(7).Text = "Opcion 3"
        Me.cmsMenuContextual.Items(7).Visible = False

        Me.cmsMenuContextual.Items(8).Text = "Opcion 4"
        Me.cmsMenuContextual.Items(8).Visible = False

        'space
        Me.cmsMenuContextual.Items(9).Visible = False

        'Expediente inteligente

        _campoExpedienteOperativo = "t_Referencia"

        _campoExpedienteAdministrativo = "t_FolioFactura"

        '***********************
        'Documentos tráfico
        Me.cmsMenuContextual.Items(10).Visible = True

        'Documentos administración
        Me.cmsMenuContextual.Items(11).Visible = True

        _campoExpedienteOperativo = "t_Referencia"

        _campoExpedienteAdministrativo = "t_FolioFactura"

        'OtrasOpciones5 = 12
        Me.cmsMenuContextual.Items(12).Text = "Opcion 12"
        Me.cmsMenuContextual.Items(12).Visible = False

        'OtrasOpciones6 = 13
        Me.cmsMenuContextual.Items(13).Text = "Opcion 13"
        Me.cmsMenuContextual.Items(13).Visible = False

        'OtrasOpciones7 = 14
        Me.cmsMenuContextual.Items(14).Text = "Opcion 14"
        Me.cmsMenuContextual.Items(14).Visible = False

        'OtrasOpciones8 = 15
        Me.cmsMenuContextual.Items(15).Text = "Opcion 15"
        Me.cmsMenuContextual.Items(15).Visible = False

        pnlGestor.Visible = False
        pnlGrid.Visible = True

        _activarcontrolventanasIndependientes = False

        tsbModo.Enabled = _activarcontrolventanasIndependientes

        'Se crea la primera columna 
        Dim Col0 As New TextAndImageColumn
        Col0.Image = Image.FromFile("C:\svn\SVN QA\Configuracion\Recursos\accept.png")

        DgvCatalogo.Columns.Insert(0, Col0)

        _ventanasIndependientes = True

    End Sub

    Function CargaExpedienteAutomatico(ByVal campoExpediente_ As String) As String

        For Each campo_ As ICaracteristica In OperacionesCatalogo.Caracteristicas.Values

            If LCase(campo_.Nombre) = LCase(campoExpediente_) Then

                Return campoExpediente_

            End If

        Next

        Return Nothing

    End Function


#End Region

#Region "Propiedades"

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Activar el control para cambiar entre ventanas independientes y embebidas")>
    Public Property ActivarControlVentanasIndependientes As Boolean

        Get

            Return _activarcontrolventanasIndependientes

        End Get

        Set(value As Boolean)
            _activarcontrolventanasIndependientes = value

            tsbModo.Enabled = _activarcontrolventanasIndependientes

            'If value = True Then
            '    AmbienteCurado(False)
            'Else
            '    AmbienteCurado(True)
            'End If

        End Set

    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Activar las ventanas en modal")>
    Public Property VentanasIndependientes As Boolean

        Get

            Return _ventanasIndependientes

        End Get

        Set(value As Boolean)
            _ventanasIndependientes = value

            If value = True Then
                AmbienteCurado(False)
            Else
                AmbienteCurado(True)
            End If


        End Set

    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Menú contextual, opciones")>
    Public Property MenuContextual As ContextMenuStrip

        Get

            Return cmsMenuContextual

        End Get

        Set(value As ContextMenuStrip)

        End Set

    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Permite activar/mostrar ( True, False) el gestor de inserción o edición")>
    Public Property ActivarGestor As Boolean

        Get
            Return _activarGestor

        End Get

        Set(value As Boolean)

            _activarGestor = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Permite ignorar ( True, False) el doble click automático en los contenidos de las celdas del grid")>
    Public Property IgnorarDobleClickContenidoCelda As Boolean

        Get
            Return _ignorarDobleClickContenidoCelda

        End Get

        Set(value As Boolean)

            _ignorarDobleClickContenidoCelda = value

        End Set

    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Propiedad para controlar la selección múltiple por defecto es false")>
    Public Property SeleccionMultiple As Boolean

        Get
            Return DgvCatalogo.MultiSelect

        End Get

        Set(value As Boolean)

            DgvCatalogo.MultiSelect = value

        End Set

    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Propiedad para controlar el flujo de la operación actual, por defecto es True")>
    Public Property ContinuarOperacionActual As Boolean

        Get
            Return _continuarOperacionActual

        End Get

        Set(value As Boolean)

            _continuarOperacionActual = value

        End Set

    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Propiedad que define la cantidad de caracteres a partir de los cuales se activará la búsqueda")>
    Public Property LongitudBusquedaAutomatica As Boolean
        Get
            Return _longitudBusquedaAutomatica
        End Get
        Set(value As Boolean)
            _longitudBusquedaAutomatica = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Propiedad para mostrar la barra de estado, por defecto es False")>
    Public Property MostrarBarraEstado As Boolean
        Get
            Return _mostrarBarraEstado
        End Get
        Set(value As Boolean)

            ssEstado.Visible = value
            _mostrarBarraEstado = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Propiedad para indicar que la captura de este catálogo es opcional, por defecto es False")>
    Public Property CatalogoOpcional As Boolean
        Get
            Return _catalogoOpcional
        End Get
        Set(value As Boolean)
            _catalogoOpcional = value
        End Set
    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Modalidad para la búsqueda como proceso secuencial o hilo asíncrono o síncrono")>
    Public Property ModalidadBusqueda As IOperacionesCatalogo.ModalidadesBusqueda
        Get
            Return _modalidadBusquedas
        End Get
        Set(value As IOperacionesCatalogo.ModalidadesBusqueda)
            _modalidadBusquedas = value
        End Set
    End Property


    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Hilo del proceso de búsqueda")>
    Public Property ProcesoBusqueda As Thread
        Get
            Return _procesoBuscar
        End Get
        Set(value As Thread)
            _procesoBuscar = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Path para la imprimir los informes basados en un template Excel")>
    Public Property TemplateExcel As String
        Get
            Return _templateExcel
        End Get
        Set(value As String)
            _templateExcel = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Path donde se guardarán los informes generados en Excel")>
    Public Property PathSaveExcel As String
        Get
            Return _pathsaveexcel
        End Get
        Set(value As String)
            _pathsaveexcel = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Interfaz con información sobre las operaciones del catálogo IOperacionesCatalogo")>
    Public Property OperacionesCatalogo As IOperacionesCatalogo
        Get
            Return _ioperacionescatalogo
        End Get
        Set(value As IOperacionesCatalogo)
            _ioperacionescatalogo = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Espacio de trabajo, se trata del bloque de permisos, accesos y politicas con las que cuenta este módulo")>
    Public Property EspacioTrabajo As IEspacioTrabajo
        Get
            Return _iespaciotrabajo

        End Get
        Set(value As IEspacioTrabajo)

            _ioperacionescatalogo.EspacioTrabajo = value

            _iespaciotrabajo = value

            If Not _ioperacionescatalogo.EspacioTrabajo.MisCredenciales Is Nothing And
                _ioperacionescatalogo.IdentificadorEmpresa <> "-1" Then

                _ioperacionescatalogo.IdentificadorEmpresa =
                _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

        End Set

    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Identificador para botón agregar")>
    Public Property TagAgregar As String
        Get
            Return TsAgregar.Tag
        End Get
        Set(value As String)
            TsAgregar.Tag = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Identificador para botón eliminar")>
    Public Property TagEliminar As String
        Get
            Return TsEliminar.Tag
        End Get
        Set(value As String)
            TsEliminar.Tag = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Identificador para botón modificar")>
    Public Property TagModificar As String
        Get
            Return TsModificar.Tag
        End Get
        Set(value As String)
            TsModificar.Tag = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Identificador para botón consultar")>
    Public Property TagBuscar As String
        Get
            Return TsBuscar.Tag
        End Get
        Set(value As String)
            TsBuscar.Tag = value

        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Identificador para botón actualizar")>
    Public Property TagActualizarConsulta As String
        Get
            Return TsActualizar.Tag
        End Get
        Set(value As String)
            TsActualizar.Tag = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre completo del tipo de ensamblado")>
    Public Property TipoEnsamblado As String
        Get
            Return _tipoensamblado
        End Get
        Set(value As String)
            _tipoensamblado = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.ParenthesizePropertyName(True)>
    <System.ComponentModel.Description("Ruta o path del archivo DLL del ensamblado")>
    Public Property ArchivoEnsamblado As String
        Get
            Return _rutamoduloensamblado
        End Get
        Set(value As String)
            _rutamoduloensamblado = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Identificador único de la empresa")>
    Public Property IdentificadorEmpresa As String
        Get
            Return _ioperacionescatalogo.IdentificadorEmpresa
        End Get
        Set(value As String)
            _ioperacionescatalogo.IdentificadorEmpresa = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Tabla que será afectada con los cambios")>
    Public Property TablaEdicion As String
        Get
            Return _ioperacionescatalogo.TablaEdicion
        End Get
        Set(value As String)
            _ioperacionescatalogo.TablaEdicion = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Número de serie del catálogo")>
    Public Property SerieCatalogo As String
        Get
            Return _ioperacionescatalogo.SerieCatalogo
        End Get
        Set(value As String)
            _ioperacionescatalogo.SerieCatalogo = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Descripción del catalogo")>
    Public Property NombreCatalogo As String
        Get
            Return _ioperacionescatalogo.Nombre
        End Get
        Set(value As String)
            _ioperacionescatalogo.Nombre = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre del campo llave (identificador único) del catálogo")>
    Public Property IdentificadorCatalogo As String
        Get
            Return _ioperacionescatalogo.IdentificadorCatalogo
        End Get
        Set(value As String)
            _ioperacionescatalogo.IdentificadorCatalogo = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre de la vista que provee información sobre los encabezados")>
    Public Property VistaEncabezados As String

        Get

            Return _ioperacionescatalogo.VistaEncabezados

        End Get

        Set(value As String)

            _ioperacionescatalogo.VistaEncabezados = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre del operador o manejador de las consultas del catálogo (Vista)")>
    Public Property OperadorCatalgosConsulta As String
        Get
            Return _ioperacionescatalogo.OperadorCatalogoConsulta
        End Get
        Set(value As String)
            _ioperacionescatalogo.OperadorCatalogoConsulta = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre del operador que llevara acabo la operación del borrado ( Procedimiento Almacenado)")>
    Public Property OperadorCatalogosBorrado As String
        Get
            Return _ioperacionescatalogo.OperadorCatalogoBorrado
        End Get
        Set(value As String)
            _ioperacionescatalogo.OperadorCatalogoBorrado = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre del operador que llevara acabo la operación de inserción ( Procedimiento Almacenado)")>
    Public Property OperadorCatalogosInsercion As String
        Get
            Return _ioperacionescatalogo.OperadorCatalogoInsercion
        End Get

        Set(value As String)
            _ioperacionescatalogo.OperadorCatalogoInsercion = value
        End Set
    End Property

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Nombre del operador que llevara acabo la operación de modificación ( Procedimiento Almacenado)")>
    Public Property OperadorCatalogosModificaciones As String
        Get
            Return _ioperacionescatalogo.OperadorCatalogoModificacion
        End Get

        Set(value As String)
            _ioperacionescatalogo.OperadorCatalogoModificacion = value
        End Set
    End Property
#End Region

#Region "Métodos"


    Public Function ListaMarcadoresColumnas() As List(Of MarcadorCelda)

        Return _coleccionMarcadores

    End Function

    Private Function Status() As Boolean

        If Not _preparado Then

            If _ioperacionescatalogo.PreparaCatalogo() = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.EstadoOperacion.COk Then
                _preparado = True
            Else

            End If

        End If

        Return _preparado

    End Function

    <System.ComponentModel.Category("Wma")>
    <System.ComponentModel.Description("Función para preparar el catálogo")>
    Public Function PreparaCatalogo() As IOperacionesCatalogo.EstadoOperacion
        Return _ioperacionescatalogo.PreparaCatalogo() <> IOperacionesCatalogo.EstadoOperacion.COk
    End Function

    Private Sub SetCatalogoDataSource(ByVal [datatable] As DataTable)

        If Me.DgvCatalogo.InvokeRequired Then
            Dim d As New SetCatalogoDataSourceCallback(AddressOf SetCatalogoDataSource)
            Me.Invoke(d, New Object() {[datatable]})
        Else
            Me.DgvCatalogo.DataSource = [datatable]
        End If
    End Sub
    'Delegate Sub SetCatalogoDataSourceCallback([datatable] As DataTable)
    Private Sub SetVisibleCargador(ByVal [visible] As Boolean)

        If Me.Cargador.InvokeRequired Then
            Dim d As New SetVisibleCargadorCallback(AddressOf SetVisibleCargador)
            Me.Invoke(d, New Object() {[visible]})
        Else
            Me.Cargador.Visible = [visible]
        End If
    End Sub

    Public Sub ActivadorControles(ByVal activacion_ As Boolean)

        Select Case activacion_

            Case False

                TsModificar.Enabled = False

                TsEliminar.Enabled = False

                TsActualizar.Enabled = False

                TsExportExcel.Enabled = False

                TsExportCSV.Enabled = False

            Case True

                'Editar registro
                'Actualizar consulta
                'Eliminar registro selecionado
                'Búsqueda inmediata
                'Búsqueda avanzada
                'Exportar resultados a MS Excel

                If Not TsAgregar.Tag Is Nothing Then
                    TsAgregar.ToolTipText = "[Permiso:" & TsAgregar.Tag.ToString & "] Agregar registro"
                End If

                If Not TsModificar.Tag Is Nothing Then
                    TsModificar.ToolTipText = "[Permiso:" & TsModificar.Tag.ToString & "] Editar registro"
                End If

                If Not TsEliminar.Tag Is Nothing Then
                    TsEliminar.ToolTipText = "[Permiso:" & TsEliminar.Tag.ToString & "] Eliminar registro"
                End If

                If Not TsActualizar.Tag Is Nothing Then
                    TsActualizar.ToolTipText = "[Permiso:" & TsActualizar.Tag.ToString & "] Actualizar consulta/resultados"
                End If

                If Not TsExportExcel.Tag Is Nothing Then
                    TsExportExcel.ToolTipText = "[Permiso:" & TsExportExcel.Tag.ToString & "] Exportar resultados a MS Excel"
                End If

                If Not TsAgregar.Tag Is Nothing Then
                    TsAgregar.ToolTipText = "[Permiso:" & TsAgregar.Tag.ToString & "] Agregar registros"
                End If

                If Not TsBuscar.Tag Is Nothing Then
                    TsBuscar.ToolTipText = "[Permiso:" & TsBuscar.Tag.ToString & "] Búsqueda inmediata"
                End If

                If Not TsFiltro.Tag Is Nothing Then
                    TsFiltro.ToolTipText = "[Permiso:" & TsFiltro.Tag.ToString & "] Búsqueda avanzada"
                End If

                'Verificamos permiso para agregar
                If Not TsAgregar.Tag Is Nothing And IsNumeric(TsAgregar.Tag) Then
                    TsAgregar.Enabled = _iespaciotrabajo.BuscaPermiso(TsAgregar.Tag.ToString,
                                         IEspacioTrabajo.TipoModulo.Abstracto)
                Else
                    TsAgregar.Enabled = False



                End If

                'Verificamos permiso para modificar
                If Not TsModificar.Tag Is Nothing And IsNumeric(TsModificar.Tag) Then
                    TsModificar.Enabled = _iespaciotrabajo.BuscaPermiso(TsModificar.Tag.ToString,
                                         IEspacioTrabajo.TipoModulo.Abstracto)

                    'Modificar
                    Me.cmsMenuContextual.Items(2).Enabled = TsModificar.Enabled

                Else
                    TsModificar.Enabled = False

                    'Modificar
                    Me.cmsMenuContextual.Items(2).Enabled = TsModificar.Enabled

                End If

                'Verificamos permiso para eliminar
                If Not TsEliminar.Tag Is Nothing And IsNumeric(TsEliminar.Tag) Then
                    TsEliminar.Enabled = _iespaciotrabajo.BuscaPermiso(TsEliminar.Tag.ToString,
                                         IEspacioTrabajo.TipoModulo.Abstracto)

                    'Eliminar

                    Me.cmsMenuContextual.Items(3).Enabled = TsEliminar.Enabled

                Else

                    TsEliminar.Enabled = False

                    'Eliminar

                    Me.cmsMenuContextual.Items(3).Enabled = TsEliminar.Enabled

                End If

                'Verificamos permiso para actualizar
                If Not TsActualizar.Tag Is Nothing And IsNumeric(TsActualizar.Tag) Then
                    TsActualizar.Enabled = _iespaciotrabajo.BuscaPermiso(TsActualizar.Tag.ToString,
                                         IEspacioTrabajo.TipoModulo.Abstracto)
                Else
                    TsActualizar.Enabled = False
                End If


                TsExportExcel.Enabled = True

                TsExportCSV.Enabled = True

        End Select

        If Not _ioperacionescatalogo Is Nothing Then
            'MOP 15/04/2021
            '_ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

        End If


        'Aqui lanzamos las ventanas embebidas

        'bautismen()

    End Sub

    Private Sub ProcesoBuscar()

        SetVisibleCargador(True)

        If Not TsBuscar.Tag Is Nothing And IsNumeric(TsBuscar.Tag) Then

            If _iespaciotrabajo.BuscaPermiso(TsBuscar.Tag.ToString,
                                             IEspacioTrabajo.TipoModulo.Abstracto) Then
                If Status() Then

                    _ioperacionescatalogo.ClausulasAutoFiltros = Nothing

                    If _ioperacionescatalogo.GenerarVista("%" & TsTbBuscar.Text) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        If _ioperacionescatalogo.Vista.Tables.Count > 0 Then

                            Me.SetCatalogoDataSource(_ioperacionescatalogo.Vista.Tables(0))

                            _activacionControles = True

                            ObtieneValorIndice()

                            tslRegistros.Text = "[Mx=" & _ioperacionescatalogo.CantidadVisibleRegistros & "] Registros (" & _ioperacionescatalogo.Vista.Tables(0).Rows.Count.ToString & ")"


                        Else

                            _activacionControles = False

                            tslRegistros.Text = "[Mx=" & _ioperacionescatalogo.CantidadVisibleRegistros & "] Registros (0)"

                        End If

                    Else

                        _dialogos.GsDialogo("No hay resultados para mostrar")

                    End If

                End If

            Else

                _dialogos.GsDialogo("No tiene privilegios para esta operación")

            End If

        End If

        SetVisibleCargador(False)

    End Sub

    Public Sub TsBuscar_Click(sender As Object, e As EventArgs) Handles TsBuscar.Click

        RaiseEvent EventoAntesTsBuscar()

        Select Case _modalidadBusquedas

            Case IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir

                ProcesoBuscar()

                _ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

            Case IOperacionesCatalogo.ModalidadesBusqueda.HiloAsincrono

                TsActualizar.Enabled = False

                _procesoBuscar = New Threading.Thread(AddressOf ProcesoBuscar)

                _procesoBuscar.Start()

                _ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

            Case IOperacionesCatalogo.ModalidadesBusqueda.HiloSincrono

                TsActualizar.Enabled = False

                _procesoBuscar = New Threading.Thread(AddressOf ProcesoBuscar)

                _procesoBuscar.Start()

                _procesoBuscar.Join()

                _ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

        End Select

        If tsbModo.Checked = True Then
            DibujaMarcadores()

        Else
            Dim cantidadResultados_ As Int32 = CantidadResultados()

            If cantidadResultados_ = 1 Then

                '_dialogos.GsDialogo("Se arrojó " & cantidadResultados_.ToString & " resultado, listo para modificar", GsDialogo.TipoDialogo.Aviso)

                'Cambiamos el tipo de operación de inmediato

                'reiniciamos el contador del formulario
                _contadorDeRegistrosParaInsercion = 0

                _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Modificar

                LanzarGestor()

            ElseIf cantidadResultados_ = 0 Then

                _dialogos.GsDialogo("No se encontraron coincidencias con su búsqueda, intente nuevamente con otros parámetros ", GsDialogo.TipoDialogo.Aviso)

            ElseIf cantidadResultados_ > 1 Then

                '_dialogos.GsDialogo("Se han encontrado " & cantidadResultados_.ToString & " coincidencias con su búsqueda, por favor elija una de la lista ", GsDialogo.TipoDialogo.Aviso)

                _listaDeResultadosBusqueda = New TextboxResult

                _listaDeResultadosBusqueda.Caracteristicas = New Dictionary(Of Integer, ICaracteristica)

                _listaDeResultadosBusqueda.Caracteristicas = _ioperacionescatalogo.Caracteristicas

                DespliegaResultadosPosibles()


            End If
            'Aquí debemos de investigar si los resultados fueron superiores a un registro, en ese caso tenemos que exigirle más al usuario.
            'De hecho aquí ya sabemos que tiene activada la modalidad rápida
            'también tenemos que saber si encontró resultados, no puede ser 0.


        End If

        RaiseEvent EventoDespuesTsBuscar()

    End Sub

    Private Sub AlCerrarVentanaOpcionesButtonClose() Handles _listaDeResultadosBusqueda.onButtonClose

        TsTbBuscar.Text = _listaDeResultadosBusqueda.ValorIndice

    End Sub

    Private Sub AlCerrarVentanaOpcionesCloseList() Handles _listaDeResultadosBusqueda.onCloseList

        TsTbBuscar.Text = _listaDeResultadosBusqueda.ValorIndice

        _ioperacionescatalogo.ActivarComodinesDeConsulta = False
        TsBuscar.PerformClick()
        _ioperacionescatalogo.ActivarComodinesDeConsulta = True

    End Sub

    Private Function DespliegaResultadosPosibles() As Int32

        Dim dataGridView1 As New DataGridView

        With dataGridView1

            Dim contadorColumnas_ As Int32 = 0

            Dim listaNombresTecnicos_ As New List(Of String)

            'Línea que entrega las caracteristicas para la lista desplegable
            '_listaDeResultadosBusqueda.Caracteristicas = _ioperacionescatalogo.Caracteristicas


            Dim indiceCaracteristicas_ As Int32 = 0

            _listaDeResultadosBusqueda.Caracteristicas = New Dictionary(Of Integer, ICaracteristica)

            For Each item_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                If item_.Visible = ICaracteristica.TiposVisible.Si Then

                    .Columns.Add(item_.Nombre, item_.NombreMostrar)

                    listaNombresTecnicos_.Add(item_.NombreMostrar)

                    contadorColumnas_ += 1

                End If
                Dim icaracteristica_ As ICaracteristica = New CaracteristicaCatalogo

                With icaracteristica_

                    .Interfaz = item_.Interfaz

                    .Llave = item_.Llave

                    .Longitud = item_.Longitud

                    .NameAsKey = item_.NameAsKey

                    .Nombre = item_.Nombre

                    .NombreMostrar = item_.NombreMostrar

                    .PermisoConsulta = item_.PermisoConsulta

                    .PuedeInsertar = item_.PuedeInsertar

                    .PuedeModificar = item_.PuedeModificar

                    .TipoDato = item_.TipoDato

                    .TipoFiltro = item_.TipoFiltro

                    .ValorAsignado = item_.ValorAsignado

                    .ValorDefault = item_.ValorDefault

                    .ValorFiltro = item_.ValorFiltro

                    .Visible = item_.Visible

                End With


                _listaDeResultadosBusqueda.Caracteristicas.Add(indiceCaracteristicas_, icaracteristica_)

                indiceCaracteristicas_ += 1

            Next

            .RowCount = _ioperacionescatalogo.Vista.Tables(0).Rows.Count

            Dim indice_ As Int32 = 0

            For i As Integer = 0 To .RowCount - 1

                For columna_ As Integer = 0 To contadorColumnas_ - 1

                    .Item(columna_, i).Value = _ioperacionescatalogo.Vista(i, listaNombresTecnicos_.Item(columna_),
                                                                           IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                Next

            Next

        End With

        Dim textboxNew_ As New TextBox

        textboxNew_.Text = TsTbBuscar.Text
        textboxNew_.Left = 100
        Controls.Add(textboxNew_)


        ' configurar 
        With _listaDeResultadosBusqueda
            ' indicar el combo y el datagridview
            .Iniciar(textboxNew_, dataGridView1)

            .AltoLista = 400 ' alto
            .ColumnaDefault = 1 ' columna que se va a mostrar
            .ShowToolTip = True ' mostrar o no el toolTiptext
            .ShowDialog = False  ' modal o normal

        End With


        _listaDeResultadosBusqueda.showList()

        'mClass2 = New Class1

        'With mClass2
        '    .Iniciar(ComboBox2, DataGridView2)
        '    .AltoLista = 120
        '    .AnchoLista = 350
        '    .ColumnaDefault = 0
        '    .ShowDialog = True

        '    .ShowToolTip = True
        'End With

    End Function


    Private Function CantidadResultados() As Int32

        Dim cantidadResultados_ As Int32 = 0

        If Not _ioperacionescatalogo.Vista.Tables Is Nothing Then
            If Not _ioperacionescatalogo.Vista.Tables(0).Rows Is Nothing Then
                cantidadResultados_ = _ioperacionescatalogo.Vista.Tables(0).Rows.Count
            End If
        End If

        Return cantidadResultados_

    End Function

    Private Sub AccionEliminar()

        _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Eliminar

        RaiseEvent EventoAntesTsEliminar()

        If _continuarOperacionActual = True Then

            If Status() And
                _ioperacionescatalogo.ValorIndice <> "-1" Then

                Dim items_ As Integer = Me.DgvCatalogo.SelectedRows.Count
                Dim eliminoMultiplesCampos_ As Boolean = True

                If items_ > 1 Then

                    If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia Then


                        For Each dr As DataGridViewRow In Me.DgvCatalogo.SelectedRows

                            Dim campo_ As String = Nothing

                            Select Case _ioperacionescatalogo.TipoEscritura

                                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                                    campo_ = dr.Cells("Id").Value

                                Case Else

                                    campo_ = dr.Cells("Clave").Value

                            End Select

                            If Not campo_ = "" Then

                                ObtieneValorIndice()

                            End If

                            If Not campo_ = "" Then

                                If Not _ioperacionescatalogo.Eliminar(campo_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    eliminoMultiplesCampos_ = False

                                    _dialogos.GsDialogo("AVISO: No se ha podido realizar el borrado múltiple de todos los items seleccionados, intente nuevamente por favor!", GsDialogo.TipoDialogo.Aviso)

                                    Exit For

                                End If

                            Else

                                _dialogos.GsDialogo("AVISO: No se encontró ningún indice seleccionado, seleccione e intente nuevamente", GsDialogo.TipoDialogo.Aviso)

                            End If

                        Next

                        _ioperacionescatalogo.GenerarVista()

                        DgvCatalogo.DataSource = _ioperacionescatalogo.Vista.Tables(0)

                        RaiseEvent EventoDespuesTsEliminar()

                    Else

                        _dialogos.GsDialogo("AVISO: El borrado por selección múltiple no esta disponible para esta modalidad de escritura.")

                    End If

                Else 'Seleccion única

                    If _dialogos.GsDialogo("¿ {" & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.NombreAutenticacion & "}" &
                                           Chr(13) & "Esta seguro que desea eliminar el item { " & _ioperacionescatalogo.ValorIndice & "} ?.",
                              SistemaBase.GsDialogo.TipoDialogo.Pregunta) = SistemaBase.GsDialogo.Contestacion.Ok Then

                        ObtieneValorIndice()

                        If Not (_ioperacionescatalogo.ValorIndice = "" And _ioperacionescatalogo.ValorIndice = "-1") Then

                            If _ioperacionescatalogo.Eliminar(_ioperacionescatalogo.ValorIndice) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                _ioperacionescatalogo.GenerarVista()

                                DgvCatalogo.DataSource = _ioperacionescatalogo.Vista.Tables(0)

                                RaiseEvent EventoDespuesTsEliminar()

                                DibujaMarcadores()

                            End If

                        Else

                            _dialogos.GsDialogo("No se ha seleccionado correctamente el indice", GsDialogo.TipoDialogo.Aviso)

                        End If


                    End If

                End If

            End If

        End If

    End Sub


    Public Sub TsEliminar_Click(sender As Object, e As EventArgs) Handles TsEliminar.Click


        AccionEliminar()


    End Sub

    Private Sub CargaModuloDinamico(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        Try

            If _activarGestor Then 'Solo se evaluará el lanzamiento del gestor si se encuentra activo. (true)

                'Le daremos preferencia a la ruta de origen, del ejecutable para buscar, de lo contrario buscaremos la ruta configurada
                Dim _rutaConfigurada As String = _rutamoduloensamblado

                'C:\SVN\SVN QA\Modulos\Gsol.Nucleo.Modulos.GestionMonedas64.0.0.0.0.dll

                '_rutamoduloensamblado = LCase(_rutamoduloensamblado).Replace("c:\svn\svn qa", System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).Replace("modulos\", "modules\")

                Dim rutaNueva_ As String() = _rutamoduloensamblado.Split("\")

                Dim rutaPunto_ As String = Nothing

                If UBound(rutaNueva_) >= 3 Then

                    rutaPunto_ = ".\..\modules\" & rutaNueva_(UBound(rutaNueva_))

                    _rutamoduloensamblado = rutaPunto_

                End If

                If Not System.IO.File.Exists(_rutamoduloensamblado) Then

                    '_rutamoduloensamblado = _rutaConfigurada

                    If Not System.IO.File.Exists(_rutamoduloensamblado) Then

                        _dialogos.GsDialogo("Archivo no encontrado en la ruta indicada [" & _rutamoduloensamblado & "], por  favor reporte al administrador", GsDialogo.TipoDialogo.Alerta)

                        Exit Sub

                    End If

                End If

                If Not _rutamoduloensamblado Is Nothing And
                    Not _tipoensamblado Is Nothing Then

                    _moduloensamblado = [Assembly].LoadFrom(_rutamoduloensamblado)

                    _tipocarga = _moduloensamblado.GetType(_tipoensamblado)

                    _ioperacionescatalogo.EspacioTrabajo = _iespaciotrabajo

                    Dim argumentos_() As Object = {CType(_ioperacionescatalogo, Object),
                                         CType(tipooperacion_, Object)
                                         }

                    _instanciagenerica = Activator.CreateInstance(_tipocarga, argumentos_)

                    Dim _modulodinamico = _instanciagenerica

                    _modulodinamico.ShowDialog()

                    'Hoy 14/08/2020 Acabo de desactivar estas lineas, creo que debí hacerlo hace mucho, MOP, la pandemia COVID sigue T.T
                    'Hoy 25/08/2020 tuve que regresar las líneas, de otro modo no se actualiza la vista en esta modalidad y crea confusión.

                    If _ioperacionescatalogo.GenerarVista("%" & TsTbBuscar.Text) = IOperacionesCatalogo.EstadoOperacion.COk Then

                    End If

                    If _ioperacionescatalogo.Vista.Tables.Count > 0 Then

                        DgvCatalogo.DataSource = _ioperacionescatalogo.Vista.Tables(0)

                    Else

                        DgvCatalogo.DataSource = Nothing

                    End If

                Else

                    MsgBox("Por favor defina un valor para las propiedades: _rutamoduloensamblado y _tipoensamblado ")

                End If

            End If

        Catch ioe As System.IO.FileNotFoundException

            Dim mensaje_ As String = DirectCast(ioe.InnerException, System.IO.FileNotFoundException).Message

            'Dim tagwatcher_ As New TagWatcher

            'tagwatcher_.SetError(Me, mensaje_)

            MsgBox(mensaje_)


        Catch ioe As System.IO.IOException ' Este error puede ocurrir si la carpeta Temp no existe.

            'Console.WriteLine("Error de E/S." & ioe.ToString() & "," & ioe.Message)

            'MsgBox("Carga módulo virtual GsCatalogo{er02}:" & ioe.Message & "," & ioe.Source & "," & ioe.StackTrace)

            'Throw ioe
            Dim mensaje_ As String = DirectCast(ioe.InnerException, System.IO.IOException).Message

            'Dim tagwatcher_ As New TagWatcher

            'tagwatcher_.SetError(Me, mensaje_)

            MsgBox(mensaje_)

        Catch se As System.Security.SecurityException ' No tiene el permiso apropiado para

            Dim mensaje_ As String = DirectCast(se.InnerException, System.Security.SecurityException).Message

            'Dim tagwatcher_ As New TagWatcher

            'tagwatcher_.SetError(Me, mensaje_)

            MsgBox(mensaje_)

        Catch sop As System.ObjectDisposedException

            Dim mensaje_ As String = DirectCast(sop.InnerException, System.ObjectDisposedException).Message

            'Dim tagwatcher_ As New TagWatcher

            'tagwatcher_.SetError(Me, mensaje_)

            MsgBox(mensaje_)

        Catch e As Exception   ' Interceptar todas las demás excepciones.

            'Console.WriteLine("Error 1245:" & e.ToString() & "" & e.Message)  ' Imprimir la información de excepción estándar.

            'DirectCast(e.InnerException, System.IO.FileNotFoundException).Message

            Dim mensaje_ As String = DirectCast(e.InnerException, Exception).Message

            'Dim tagwatcher_ As New TagWatcher

            'tagwatcher_.SetError(Me, mensaje_)

            MsgBox(mensaje_)

            'MsgBox("Carga módulo virtual GsCatalogo{er04}:" & _
            '        " MENSAJE:{" & e.Message & "}" & Chr(13) & _
            '        " FUENTE:{" & e.Source & "}" & Chr(13) & _
            '        " TRACE:{" & e.StackTrace & "}" & Chr(13) & _
            '        " RUTA:{" & _rutamoduloensamblado & "} " & Chr(13) & _
            '        " TIPO SOLICITADO:{" & _tipoensamblado & "} " & Chr(13) & _
            '        " TIPO ENCONTRADO:{" & _tipocarga.ToString & "}")

        End Try

    End Sub

    Private Function PrerrequisitosOperaciones() As Boolean
        If Not (_tipoensamblado Is Nothing And
            _rutamoduloensamblado Is Nothing) Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Overridable Sub AntesTsAgregar()

    End Sub


    Public Overridable Sub DespuesTsAgregar()

    End Sub

    Public Overridable Sub AntesTsModificar()

    End Sub

    Public Overridable Sub DespuesTsModificar()

    End Sub

    Public Sub TsAgregar_Click(sender As Object, e As EventArgs) Handles TsAgregar.Click

        _contadorDeRegistrosParaInsercion = 1

        _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion

        RaiseEvent EventoAntesTsAgregar()

        AntesTsAgregar()

        If _continuarOperacionActual Then

            If Not TsBuscar.Tag Is Nothing And IsNumeric(TsBuscar.Tag) Then

                If _iespaciotrabajo.BuscaPermiso(TsAgregar.Tag.ToString,
                                                IEspacioTrabajo.TipoModulo.Abstracto) Then

                    If PrerrequisitosOperaciones() Then

                        If _ventanasIndependientes Then

                            CargaModuloDinamico(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                            TsBuscar.PerformClick()

                        Else

                            CargaModuloDinamicoEmbebido(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                        End If


                    Else

                        _dialogos.GsDialogo("Aún no se han definido por completo los parámetros para esta operación")

                    End If

                Else

                    _dialogos.GsDialogo("No tiene privilegios para esta operación")

                End If

            Else

                _dialogos.GsDialogo("No tiene privilegios para esta operación")

            End If

            DespuesTsAgregar()

            RaiseEvent EventoDespuesTsAgregar()

        End If

        If _ventanasIndependientes Then

            DibujaMarcadores()

        End If

    End Sub
    Private Function ObtieneValorIndiceDirecto() As String

        Dim indice_ As String = "-1"

        Try

            If _ioperacionescatalogo.Vista.Tables.Count > 0 And
                        DgvCatalogo.RowCount > 0 Then

                Select Case _ioperacionescatalogo.TipoEscritura

                    Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                        indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                    Case IOperacionesCatalogo.TiposEscritura.Inmediata

                        If _campollave Is Nothing Then

                            For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                                If caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria Then

                                    _campollave = caracteristica_.NombreMostrar

                                    Exit For

                                End If

                            Next

                        End If

                        Try

                            indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item(_campollave).Value.ToString()

                        Catch ex As Exception

                            indice_ = "-1"

                        End Try

                    Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                        indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                End Select

            Else

                indice_ = "-1"

            End If

            Return indice_.Clone

        Catch ex As Exception

            Return indice_.Clone

        End Try

    End Function

    Private Sub ObtieneValorIndice()

        Dim indice_ As String = "-1"

        Try

            If _ioperacionescatalogo.Vista.Tables.Count > 0 And
                        DgvCatalogo.RowCount > 0 Then

                Select Case _ioperacionescatalogo.TipoEscritura

                    Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                        indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                    Case IOperacionesCatalogo.TiposEscritura.Inmediata

                        If _campollave Is Nothing Then

                            For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                                If caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria Then

                                    _campollave = caracteristica_.NombreMostrar

                                    Exit For

                                End If

                            Next

                        End If

                        Try

                            indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item(_campollave).Value.ToString()

                        Catch ex As Exception

                            indice_ = "-1"

                        End Try

                    Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                        indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                End Select

            Else

                indice_ = "-1"

            End If

            _ioperacionescatalogo.ValorIndice = indice_.Clone

        Catch ex As Exception

            _ioperacionescatalogo.ValorIndice = indice_.Clone

        End Try

    End Sub

    Private Sub PreparacionModificar(Optional ByVal indiceActual_ As String = "-1")

        Dim indice_ As String = indiceActual_

        If DgvCatalogo.RowCount > 0 Then

            Select Case _ioperacionescatalogo.TipoEscritura

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                        If caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria And
                            indice_ = "-1" Then

                            Try

                                indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                            Catch ex As Exception

                                indice_ = "-1"

                            End Try

                        End If

                        If (caracteristica_.Visible = False And caracteristica_.PuedeModificar = False) Then

                            Continue For

                        End If

                        Try
                            caracteristica_.ValorAsignado = Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                        Catch ex As Exception

                            caracteristica_.ValorAsignado = caracteristica_.ValorDefault

                        End Try

                    Next

                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                        If (caracteristica_.Visible = False And caracteristica_.PuedeModificar = False) Then

                            Continue For

                        End If

                        Try
                            caracteristica_.ValorAsignado = Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                        Catch ex As Exception

                            caracteristica_.ValorAsignado = caracteristica_.ValorDefault

                        End Try

                    Next

                    indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                    '<!-- ZONA DE PRUEBA
                    For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                        If (caracteristica_.Visible = False And caracteristica_.PuedeModificar = False) Then

                            Continue For

                        End If

                        Try
                            caracteristica_.ValorAsignado = Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                        Catch ex As Exception

                            caracteristica_.ValorAsignado = caracteristica_.ValorDefault

                        End Try

                    Next

                    'ZONA DE PRUEBA-->
                    indice_ = Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                Case Else

            End Select

        End If

        _ioperacionescatalogo.ValorIndice = indice_

    End Sub

    Private Sub PreparacionModificarDesdeCursor(Optional ByVal indiceActual_ As String = "-1")

        Dim indice_ As String = indiceActual_

        'If DgvCatalogo.RowCount > 0 Then


        If _sistema.TieneResultados(_ioperacionescatalogo) Then

            Select Case _ioperacionescatalogo.TipoEscritura

                Case IOperacionesCatalogo.TiposEscritura.Inmediata

                    For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                        If caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria And
                            indice_ = "-1" Then

                            Try

                                indice_ = _ioperacionescatalogo.Vista(0, caracteristica_.NombreMostrar, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                'Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                            Catch ex As Exception

                                indice_ = "-1"

                            End Try

                        End If

                        If (caracteristica_.Visible = False And caracteristica_.PuedeModificar = False) Then

                            Continue For

                        End If

                        Try
                            caracteristica_.ValorAsignado = _ioperacionescatalogo.Vista(0, caracteristica_.NombreMostrar, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                            'Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                        Catch ex As Exception

                            caracteristica_.ValorAsignado = caracteristica_.ValorDefault

                        End Try

                    Next

                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                        If (caracteristica_.Visible = False And caracteristica_.PuedeModificar = False) Then

                            Continue For

                        End If

                        Try
                            caracteristica_.ValorAsignado = _ioperacionescatalogo.Vista(0, caracteristica_.NombreMostrar, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                            'Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                        Catch ex As Exception

                            caracteristica_.ValorAsignado = caracteristica_.ValorDefault

                        End Try

                    Next
                    'Nunca probado, habrá que revisar esta combinación, MOP 22/10/2020
                    indice_ = _ioperacionescatalogo.Vista(0, "Id", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                    'Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                    '<!-- ZONA DE PRUEBA
                    For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

                        If (caracteristica_.Visible = False And caracteristica_.PuedeModificar = False) Then

                            Continue For

                        End If

                        Try
                            caracteristica_.ValorAsignado = _ioperacionescatalogo.Vista(0, caracteristica_.NombreMostrar, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                            ' Me.DgvCatalogo.CurrentRow.Cells.Item(caracteristica_.NombreMostrar).Value.ToString()

                        Catch ex As Exception

                            caracteristica_.ValorAsignado = caracteristica_.ValorDefault

                        End Try

                    Next

                    'ZONA DE PRUEBA-->
                    'misma situación, habrá que probar, MOP 22/10/2020
                    indice_ = _ioperacionescatalogo.Vista(0, "Id", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                    'Me.DgvCatalogo.CurrentRow.Cells.Item("Id").Value.ToString()

                Case Else

            End Select

        End If

        _ioperacionescatalogo.ValorIndice = indice_

    End Sub

    Private Sub AccionModificar()
        Dim valorIndicador_ As String = "-1"

        _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Modificar

        RaiseEvent EventoAntesTsModificar()

        AntesTsModificar()

        If _continuarOperacionActual Then

            If PrerrequisitosOperaciones() And
                _ioperacionescatalogo.ValorIndice <> "-1" Then

                If _ioperacionescatalogo.Vista.Tables.Count > 0 Then



                    If _ventanasIndependientes Then

                        PreparacionModificar()

                        valorIndicador_ = ObtieneValorIndiceDirecto()

                        '******************

                        CargaModuloDinamico(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                        PosicionaUltimoIndice(valorIndicador_)

                    Else

                        PreparacionModificarDesdeCursor()

                        valorIndicador_ = ObtieneValorIndiceDirecto()

                        CargaModuloDinamicoEmbebido(IOperacionesCatalogo.TiposOperacionSQL.Modificar)


                    End If

                Else

                    _dialogos.GsDialogo("No hay datos seleccionados")

                End If

            Else

                _dialogos.GsDialogo("Aún no se han definido todos los parámetros para esta operación")

            End If

            DespuesTsModificar()

            RaiseEvent EventoDespuesTsModificar()



        End If

        'all of those things was verified on 14 Ag 2020, MOP
        If _ventanasIndependientes Then

            DibujaMarcadores()

        End If


    End Sub


    Public Sub TsModificar_Click(sender As Object, e As EventArgs) _
        Handles TsModificar.Click

        AccionModificar()

    End Sub

    Private Function EncontrarRegistro(ByRef datagridview_ As DataGridView,
                       ByVal campoindice_ As String,
                       ByVal valorindice_ As Object) As DataGridViewRow
        Dim contador_ As Int32 = 0

        Try

            For Each registro_ As DataGridViewRow In datagridview_.Rows

                If registro_.Cells.Item(campoindice_).Value = valorindice_ Then

                    registro_.Selected = True

                    If Not datagridview_.Columns.Item(campoindice_).Visible Then

                        datagridview_.Columns.Item(campoindice_).Visible = True

                        datagridview_.CurrentCell = datagridview_.Rows(registro_.Index).Cells(campoindice_)

                        datagridview_.Columns.Item(campoindice_).Visible = False

                        If _ioperacionescatalogo.Vista.Tables.Count > 0 Then

                            ObtieneValorIndice()

                            PreparacionModificar()

                        End If

                    Else

                        datagridview_.CurrentCell = datagridview_.Rows(registro_.Index).Cells(campoindice_)

                    End If

                    contador_ += 1

                    Return registro_

                End If
            Next

        Catch ex As Exception

            Return Nothing

        End Try

        Return Nothing

    End Function

    Private Sub PosicionaUltimoIndice(ByVal valorretornado_ As String)

        For Each caracteristica_ As ICaracteristica In _ioperacionescatalogo.Caracteristicas.Values

            If caracteristica_.Llave = ICaracteristica.TipoLlave.Primaria Then

                EncontrarRegistro(DgvCatalogo,
                                  caracteristica_.NombreMostrar,
                                  valorretornado_)

            End If

        Next

    End Sub


    'Public Sub DgvCatalogo_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) _
    Public Sub DgvCatalogo_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) _
    Handles DgvCatalogo.CellContentDoubleClick 'DgvCatalogo.CellDoubleClick, 

        Select Case _ignorarDobleClickContenidoCelda

            Case True

                RaiseEvent DobleClickContenidoCelda()

            Case Else 'Por defecto no lo va a ignorar

                LanzarGestor()

        End Select



    End Sub


    '2842
    Private Sub DgvCatalogo_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvCatalogo.CellDoubleClick

        Select Case _ignorarDobleClickContenidoCelda

            Case True

                RaiseEvent DobleClickContenidoCelda()

            Case Else 'Por defecto no lo va a ignorar

                '::: SUSPENDIDO PARA REVISIÓN :::::
                LanzarGestor()

        End Select

    End Sub

    Private Sub LanzarGestor()

        Dim valorIndicador_ As String = "-1"

        RaiseEvent EventoAntesTsModificar()

        AntesTsModificar()

        If _continuarOperacionActual = True Then

            If Not TsBuscar.Tag Is Nothing And IsNumeric(TsBuscar.Tag) Then

                If _iespaciotrabajo.BuscaPermiso(TsModificar.Tag.ToString,
                                                                  IEspacioTrabajo.TipoModulo.Abstracto) Then

                    If _ventanasIndependientes Then

                        PreparacionModificar()

                        valorIndicador_ = ObtieneValorIndiceDirecto()

                        '**************

                        CargaModuloDinamico(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                        PosicionaUltimoIndice(valorIndicador_)

                    Else

                        PreparacionModificarDesdeCursor()

                        valorIndicador_ = ObtieneValorIndiceDirecto()

                        CargaModuloDinamicoEmbebido(IOperacionesCatalogo.TiposOperacionSQL.Modificar)


                    End If
                    '**************
                    '//Código obsoleto, MOP 08/10/2020
                    'CargaModuloDinamico(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    'PosicionaUltimoIndice(valorIndicador_)

                Else

                    _dialogos.GsDialogo("No tiene privilegios para esta operación")

                End If

            Else

                _dialogos.GsDialogo("No tiene privilegios para esta operación")

            End If


            DespuesTsModificar()

            RaiseEvent EventoDespuesTsModificar()

        End If

        DibujaMarcadores()

    End Sub


    Private Sub ProcesoActualizar()

        SetVisibleCargador(True)

        _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Consulta

        If Status() Then

            _ioperacionescatalogo.GenerarVista()

            Me.SetCatalogoDataSource(_ioperacionescatalogo.Vista.Tables(0))

            _activacionControles = True

        End If

        SetVisibleCargador(False)

    End Sub

    Public Sub TsActualizar_Click(sender As Object, e As EventArgs) _
        Handles TsActualizar.Click

        Select Case _modalidadBusquedas

            Case IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir

                TsActualizar.Enabled = False

                _activacionControles = False

                ProcesoActualizar()

                _ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

            Case IOperacionesCatalogo.ModalidadesBusqueda.HiloAsincrono

                TsActualizar.Enabled = False

                _activacionControles = False

                If _procesoBuscar.IsAlive Or _procesoBuscar.ThreadState = ThreadState.Running Then

                    _dialogos.GsDialogo("Espere que concluya la consulta actual")

                Else

                    _procesoBuscar = New Threading.Thread(AddressOf ProcesoActualizar)

                    _procesoBuscar.Start()

                    _procesoBuscar.IsBackground = True

                End If

            Case IOperacionesCatalogo.ModalidadesBusqueda.HiloSincrono

                TsActualizar.Enabled = False

                _activacionControles = False

                If _procesoBuscar.IsAlive Or _procesoBuscar.ThreadState = ThreadState.Running Then

                    _dialogos.GsDialogo("Espere que concluya la consulta actual")

                Else

                    _procesoBuscar = New Threading.Thread(AddressOf ProcesoActualizar)

                    _procesoBuscar.Start()

                    _procesoBuscar.IsBackground = True

                    _procesoBuscar.Join()

                End If

        End Select

        DibujaMarcadores()

    End Sub

    Private Sub DgvCatalogo_SelectionChanged(sender As Object, e As EventArgs) _
        Handles DgvCatalogo.SelectionChanged

        If _ioperacionescatalogo.Vista.Tables.Count > 0 Then

            ObtieneValorIndice()

            PreparacionModificar()

        End If

    End Sub

    Private Sub TsTbBuscar_KeyDown(sender As Object, e As KeyEventArgs) _
        Handles TsTbBuscar.KeyDown

        Select Case e.KeyData

            Case Keys.Escape

                'msgBox("ESC GsCatalogo")
                TsTbBuscar.Text = Nothing

            Case Keys.Enter

                ' MsgBox("ENTER GsCatalogo")

                TsBuscar.PerformClick()

            Case Keys.F3

                If (Not String.IsNullOrEmpty(TsTbBuscar.Text)) Then
                    TsTbBuscar.SelectionStart = 0
                    TsTbBuscar.SelectionLength = TsTbBuscar.Text.Length
                End If


            Case Keys.F4
                TsAgregar.PerformClick()

        End Select

    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) _
        Handles btnCancelar.Click

        Me.Panel1.Hide()

    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) _
        Handles btnOk.Click

        Me.Panel1.Hide()

    End Sub


    Private Sub TsExportExcel_Click(sender As Object, e As EventArgs) Handles TsExportExcel.Click

        Dim exportarExcel_ As IReportw

        Dim ioperacionescopia_ As IOperacionesCatalogo = New OperacionesCatalogo

        If Not _ioperacionescatalogo Is Nothing Then

            ioperacionescopia_ = _ioperacionescatalogo.Clone

            ioperacionescopia_.Nombre = _ioperacionescatalogo.Nombre

            ioperacionescopia_.ClausulasLibres = _ioperacionescatalogo.ClausulasLibres

            ioperacionescopia_.ClausulasAutoFiltros = _ioperacionescatalogo.ClausulasAutoFiltros

            ioperacionescopia_.CantidadVisibleRegistros = _ioperacionescatalogo.CantidadVisibleRegistros

            exportarExcel_ = New DesktopReports

            exportarExcel_.MakerMode = IReportw.MakerModes.ThroughPackage

            exportarExcel_.PackageIOperations = ioperacionescopia_

            If Me._templateExcel <> "" Then

                exportarExcel_.setReportTheme = ExcelApplicationReports.WorksheetThemes.Undefined

                exportarExcel_.PathTemplate = _templateExcel

                exportarExcel_.FilePath = _pathsaveexcel

            Else

                exportarExcel_.setReportTheme = ExcelApplicationReports.WorksheetThemes.OliveWorld

                exportarExcel_.PathTemplate = _templateExcel

                exportarExcel_.FilePath = _pathsaveexcel

            End If

            exportarExcel_.setPassword = _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ContraseniaUsuario

            exportarExcel_.GenerateReport()

        End If

    End Sub

    Public Overridable Sub DgvCatalogo_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DgvCatalogo.CellEndEdit

    End Sub

    Private Sub ProcesoBuscarFiltros()

        SetVisibleCargador(True)

        _ioperacionescatalogo.GenerarVista()

        If _ioperacionescatalogo.Vista.Tables.Count >= 1 Then

            If _ioperacionescatalogo.Vista.Tables(0).Rows.Count >= 1 Then

                Me.SetCatalogoDataSource(_ioperacionescatalogo.Vista.Tables(0))

                _activacionControles = True

                tslRegistros.Text = "[Mx=" & _ioperacionescatalogo.CantidadVisibleRegistros & "] Registros (" & _ioperacionescatalogo.Vista.Tables(0).Rows.Count.ToString & ")"

            Else

                Me.SetCatalogoDataSource(_ioperacionescatalogo.Vista.Tables(0))

                _activacionControles = False

                tslRegistros.Text = "[Mx=" & _ioperacionescatalogo.CantidadVisibleRegistros & "] Registros (0)"

            End If

        Else
            _activacionControles = False

            _dialogos.GsDialogo("Esta consulta fue detenida, posiblemente sus resultados sobrepasan los margenes de esta configuración, verifique con el administrador")

        End If

        SetVisibleCargador(False)

    End Sub

    Private Sub TsFiltro_Click(sender As Object, e As EventArgs) _
        Handles TsFiltro.Click

        If Status() Then

            Dim _ioperationsdynamicform As IOperationsDynamicForm = New OperationsDynamicForm

            _ioperationsdynamicform.IOperations = _ioperacionescatalogo

            '_ioperationsdynamicform.ModeForm = IOperationsDynamicForm.ModeDynamicControls.AsComparisons
            _ioperationsdynamicform.ModeForm = IOperationsDynamicForm.ModeDynamicControls.AsComparisons

            _ioperationsdynamicform.ShowMyDynamicForm()

            _ioperacionescatalogo.ClausulasAutoFiltros = _ioperationsdynamicform.GetQueryRules.ToString

            If _ioperationsdynamicform.LaunchQuery Then

                Select Case _modalidadBusquedas

                    Case IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir

                        ProcesoBuscarFiltros()

                        'MOP, 15/04/2021
                        '_ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

                    Case IOperacionesCatalogo.ModalidadesBusqueda.HiloAsincrono

                        If Not _procesoBuscar Is Nothing Then

                            If Not _procesoBuscar.IsAlive Then

                                _procesoBuscar = New Threading.Thread(AddressOf ProcesoBuscarFiltros)

                                _procesoBuscar.Start()

                            Else
                                _dialogos.GsDialogo("Actualmente se esta procesando una consulta, espere que concluya e intente nuevamente")
                            End If

                        Else

                            _procesoBuscar = New Threading.Thread(AddressOf ProcesoBuscarFiltros)

                            _procesoBuscar.Start()

                        End If

                    Case IOperacionesCatalogo.ModalidadesBusqueda.HiloSincrono

                        If Not _procesoBuscar Is Nothing Then

                            If Not _procesoBuscar.IsAlive Then

                                _procesoBuscar = New Threading.Thread(AddressOf ProcesoBuscarFiltros)

                                _procesoBuscar.Start()

                            Else
                                _dialogos.GsDialogo("Actualmente se esta procesando una consulta, espere que concluya e intente nuevamente")
                            End If

                        Else

                            _procesoBuscar = New Threading.Thread(AddressOf ProcesoBuscarFiltros)

                            _procesoBuscar.Start()

                            _procesoBuscar.Join()

                        End If

                End Select

            End If

        End If

        _ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

        DibujaMarcadores()

    End Sub

    Private Sub Cargador_VisibleChanged(sender As Object, e As EventArgs) Handles Cargador.VisibleChanged

        ActivadorControles(_activacionControles)

    End Sub


    Private Sub Cargador_Click(sender As Object, e As EventArgs) Handles Cargador.Click

        If Not _procesoBuscar Is Nothing Then

            If _procesoBuscar.IsAlive Then

                Dim result As Integer = MessageBox.Show("¿Desea abortar la operación actual?", "Aviso", MessageBoxButtons.YesNoCancel)

                If result = DialogResult.Cancel Then

                ElseIf result = DialogResult.No Then

                ElseIf result = DialogResult.Yes Then

                    _procesoBuscar.Interrupt()

                    _procesoBuscar.Abort()

                    While _procesoBuscar.IsAlive
                    End While

                    TsExportExcel.Enabled = False

                    TsExportCSV.Enabled = False

                    _activacionControles = True

                    SetVisibleCargador(False)

                    '_ioperacionescatalogo.OcultaCamposGridView(DgvCatalogo)

                End If

            End If

        End If

    End Sub

#End Region



    Private Sub TsOpciones_KeyDown(sender As Object, e As KeyEventArgs) _
        Handles TsOpciones.KeyDown

        Select Case e.KeyData

            Case Keys.Escape

                'MsgBox("ESC TsOpciones")
                TsTbBuscar.Text = Nothing

            Case Keys.Enter

                'MsgBox("ENTER TsOpciones")

                'TsBuscar.PerformClick()

            Case Keys.F3

                TsTbBuscar.Focus()

                If (Not String.IsNullOrEmpty(TsTbBuscar.Text)) Then
                    TsTbBuscar.SelectionStart = 0
                    TsTbBuscar.SelectionLength = TsTbBuscar.Text.Length
                End If

            Case Keys.F4

                TsAgregar.PerformClick()

            Case Keys.F6

                TsFiltro.PerformClick()

            Case Keys.Delete

                TsEliminar.PerformClick()

            Case Keys.F2

                TsModificar.PerformClick()

            Case Keys.F5

                TsTbBuscar.PerformClick()


        End Select

    End Sub


    Private Sub DgvCatalogo_KeyDown(sender As Object, e As KeyEventArgs) _
        Handles DgvCatalogo.KeyDown

        Select Case e.KeyData

            Case Keys.Escape

                If Not TsTbBuscar Is Nothing Then
                    TsTbBuscar.Text = Nothing
                End If


            Case Keys.Enter

                'MsgBox("ENTER DgvCatalogo")

                'TsBuscar.PerformClick()

            Case Keys.F3

                TsTbBuscar.Focus()

                If (Not String.IsNullOrEmpty(TsTbBuscar.Text)) Then
                    TsTbBuscar.SelectionStart = 0
                    TsTbBuscar.SelectionLength = TsTbBuscar.Text.Length
                End If


            Case Keys.F4
                TsAgregar.PerformClick()


        End Select

    End Sub

    Public Sub DibujaMarcadores()

        If _coleccionMarcadores.Count < 1 Then

            Exit Sub

        End If

        Try

            For Each columnRow_ As DataGridViewRow In DgvCatalogo.Rows

                For Each marcador_ As MarcadorCelda In _coleccionMarcadores


                    'Dim row As DataGridViewRow = DgvCatalogo.Rows(DgvCatalogo.)


                    If CStr(columnRow_.Cells(marcador_.NombreColumna).Value) = marcador_.CuandoValorIgualA Then

                        columnRow_.DefaultCellStyle.BackColor = marcador_.AplicarColorFondoCeldas 'Color.LightCoral

                        columnRow_.DefaultCellStyle.ForeColor = marcador_.AplicarColorFondoTexto 'Color.Maroon

                        Exit For

                    End If


                Next

            Next


            'Añade columas con imagenes


            'MyBase.Load()
            'Se crea la primera columna 
            ' Dim Col0 As New TextAndImageColumn
            'Col0.Image = Image.FromFile("C:\svn\SVN QA\Configuracion\Recursos\accept.png")
            'DgvCatalogo.Columns.Insert(0, Col0)

            'Se crea la segunda columna 
            '  Dim Col1 As New TextAndImageColumn
            '  Col1.Image = Image.FromFile("C:\svn\svn qa\configuracion\recursos\cross.png")
            '  DgvCatalogo.Columns.Insert(1, Col1)

            'Para cambiar el texto y la imagen de una celda, debeis utilizar este codigo (yo modifico la celda 
            'que esta en la primera fila y segunda columna): 

            '----------->DirectCast(DgvCatalogo.Item(0, 0), TextAndImageCell).Value = "Nuevo texto"
            '----------> DirectCast(DgvCatalogo.Item(0, 0), TextAndImageCell).Image = Image.FromFile("C:\svn\SVN QA\Configuracion\Recursos\cross.png")


            'NOTA: Para cambiar unicamente el texto de una celda tambien podeis utilizar el codigo que se 
            'emplea para cambiarlo en una celda de una columna de texto normal (no se puede cambiar la imagen 
            'mediante este metodo): 

            ' Me.DgvCatalogo.Rows(0).Cells(1).Value = "Nuevo texto"

        Catch ex As Exception

        End Try

    End Sub


    'Private Function List(Of T)() As List(Of T)
    '    Throw New NotImplementedException
    'End Function

    Private Sub DgvCatalogo_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvCatalogo.ColumnHeaderMouseClick

        DibujaMarcadores()

    End Sub



    Private Sub TsExportCSV_Click(sender As Object, e As EventArgs) Handles TsExportCSV.Click

        Dim exportarCSV_ As IReportw

        Dim ioperacionescopia_ As IOperacionesCatalogo = New OperacionesCatalogo

        If Not _ioperacionescatalogo Is Nothing Then

            ioperacionescopia_ = _ioperacionescatalogo.Clone

            ioperacionescopia_.Nombre = _ioperacionescatalogo.Nombre

            ioperacionescopia_.ClausulasLibres = _ioperacionescatalogo.ClausulasLibres

            ioperacionescopia_.ClausulasAutoFiltros = _ioperacionescatalogo.ClausulasAutoFiltros

            ioperacionescopia_.CantidadVisibleRegistros = _ioperacionescatalogo.CantidadVisibleRegistros

            exportarCSV_ = New DesktopReports

            exportarCSV_.MakerMode = IReportw.MakerModes.ThroughPackage

            exportarCSV_.PackageIOperations = ioperacionescopia_

            exportarCSV_.FilePath = _pathsaveexcel

            exportarCSV_.setPassword = _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ContraseniaUsuario

            exportarCSV_.FileExtension = IReportw.Extensions.csv

            exportarCSV_.GenerateReport()

        End If

    End Sub

    Private Sub tsmModificar_Click(sender As Object, e As EventArgs) Handles tsmModificar.Click

        AccionModificar()

    End Sub

    Private Sub tsmEliminar_Click(sender As Object, e As EventArgs) Handles tsmEliminar.Click

        AccionEliminar()

    End Sub




    Private Sub tsmItem1_Click(sender As Object, e As EventArgs) Handles tsmItem1.Click

        RaiseEvent Item1MenuContextualClick()

    End Sub

    Private Sub tsmItem2_Click(sender As Object, e As EventArgs) Handles tsmItem2.Click

        RaiseEvent Item2MenuContextualClick()

    End Sub

    Private Sub tsmItem3_Click(sender As Object, e As EventArgs) Handles tsmItem3.Click

        RaiseEvent Item3MenuContextualClick()

    End Sub

    Private Sub tsmItem4_Click(sender As Object, e As EventArgs) Handles tsmItem4.Click

        RaiseEvent Item4MenuContextualClick()

    End Sub

    Private Sub tsmItem5_Click(sender As Object, e As EventArgs) Handles tsmItem5.Click

        RaiseEvent Item5MenuContextualClick()

    End Sub

    Private Sub tsmItem6_Click(sender As Object, e As EventArgs) Handles tsmItem6.Click

        RaiseEvent Item6MenuContextualClick()

    End Sub

    Private Sub tsmItem7_Click(sender As Object, e As EventArgs) Handles tsmItem7.Click

        RaiseEvent Item7MenuContextualClick()

    End Sub

    Private Sub tsmItem8_Click(sender As Object, e As EventArgs) Handles tsmItem8.Click

        RaiseEvent Item8MenuContextualClick()

    End Sub

    Private Sub tsmSeleccionMultiple_Click(sender As Object, e As EventArgs) Handles tsmSeleccionMultiple.Click

        If tsmSeleccionMultiple.Checked Then
            DgvCatalogo.MultiSelect = True

        Else
            DgvCatalogo.MultiSelect = False

        End If
    End Sub

    Private Sub TsPedimento_Click(sender As Object, e As EventArgs) Handles TsPedimento.Click

        'l objetivo de esto es que si le pasan una característica cuyo nombre sea t_Referencia, t_NumeroReferencia o t_FolioFactura (con su respectivo valor), en automático les mostrará los documentos de esa operación o factura, a continuación un ejemplo:


        '**************************************************************************************+

        '*Referencia*


        If Not _campoExpedienteOperativo Is Nothing Then

            Dim MyForm_ = New gsol.Modulos.VisorAsociadorMaestroDocumentos64.frm012VisorAsociadorMaestroDocumentos(_iespaciotrabajo, New CaracteristicaCatalogo With {.Nombre = _campoExpedienteOperativo, .ValorAsignado = Me.OperacionesCatalogo.CampoPorNombre(_campoExpedienteOperativo)})

            MyForm_.Show()

        End If


        '*************************************************************************

    End Sub

    Private Sub TsCuentaDeGastos_Click(sender As Object, e As EventArgs) Handles TsCuentaDeGastos.Click

        If Not _campoExpedienteAdministrativo Is Nothing Then

            Dim MyForm_ = New gsol.Modulos.VisorAsociadorMaestroDocumentos64.frm012VisorAsociadorMaestroDocumentos(_iespaciotrabajo, New CaracteristicaCatalogo With {.Nombre = _campoExpedienteAdministrativo, .ValorAsignado = Me.OperacionesCatalogo.CampoPorNombre(_campoExpedienteAdministrativo)})

            MyForm_.Show()

        End If



    End Sub

    Private Sub cmsMenuContextual_Paint(sender As Object, e As PaintEventArgs) Handles cmsMenuContextual.Paint

        _campoExpedienteOperativo = CargaExpedienteAutomatico(_campoExpedienteOperativo)

        If (_campoExpedienteOperativo Is Nothing) Then

            _campoExpedienteOperativo = CargaExpedienteAutomatico("t_NumeroReferencia")

        End If

        _campoExpedienteAdministrativo = CargaExpedienteAutomatico(_campoExpedienteAdministrativo)

        If (_campoExpedienteAdministrativo Is Nothing) Then

            _campoExpedienteAdministrativo = CargaExpedienteAutomatico("t_NumeroFactura")

        End If


        If Not _campoExpedienteOperativo Is Nothing Then
            'Documentos tráfico
            Me.cmsMenuContextual.Items(OpcionesMenuContextual.DocumentosTrafico).Visible = True
        Else

            Me.cmsMenuContextual.Items(OpcionesMenuContextual.DocumentosTrafico).Visible = False
        End If

        If Not _campoExpedienteAdministrativo Is Nothing Then

            'Documentos administración
            Me.cmsMenuContextual.Items(OpcionesMenuContextual.DocumentosAdministracion).Visible = True
        Else
            Me.cmsMenuContextual.Items(OpcionesMenuContextual.DocumentosAdministracion).Visible = False

        End If

    End Sub


    Private Sub CargaModuloDinamicoEmbebido(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        Try

            If _activarGestor Then 'Solo se evaluará el lanzamiento del gestor si se encuentra activo. (true)

                'Le daremos preferencia a la ruta de origen, del ejecutable para buscar, de lo contrario buscaremos la ruta configurada
                Dim _rutaConfigurada As String = _rutamoduloensamblado

                _rutamoduloensamblado = LCase(_rutamoduloensamblado).Replace("c:\svn\svn qa", System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).Replace("modulos\", "modules\")

                If Not System.IO.File.Exists(_rutamoduloensamblado) Then

                    _rutamoduloensamblado = _rutaConfigurada

                    If Not System.IO.File.Exists(_rutamoduloensamblado) Then

                        _dialogos.GsDialogo("Archivo no encontrado en la ruta indicada [" & _rutamoduloensamblado & "], por  favor reporte al administrador", GsDialogo.TipoDialogo.Alerta)

                        Exit Sub

                    End If

                End If

                If Not _rutamoduloensamblado Is Nothing And _
                    Not _tipoensamblado Is Nothing Then

                    _moduloensamblado = [Assembly].LoadFrom(_rutamoduloensamblado)

                    _tipocarga = _moduloensamblado.GetType(_tipoensamblado)

                    _ioperacionescatalogo.EspacioTrabajo = _iespaciotrabajo

                    Dim argumentos_() As Object = {CType(_ioperacionescatalogo, Object), _
                                         CType(tipooperacion_, Object)
                                         }

                    _instanciagenerica = Activator.CreateInstance(_tipocarga, argumentos_)

                    _modulodinamicoEmbebido = _instanciagenerica

                    '_modulodinamico.ShowDialog()

                    'Dim form2 As UsersAddCP = New UsersAddCP()
                    _modulodinamicoEmbebido.TopLevel = False
                    _modulodinamicoEmbebido.TopMost = True

                    _modulodinamicoEmbebido.Dock = DockStyle.Top


                    _modulodinamicoEmbebido.FormBorderStyle = Windows.Forms.FormBorderStyle.None

                    pnlGestor.Controls.Clear()
                    pnlGestor.Controls.Add(_modulodinamicoEmbebido)


                    _modulodinamicoEmbebido.Show()

                Else

                    MsgBox("Por favor defina un valor para las propiedades: _rutamoduloensamblado y _tipoensamblado ")

                End If

            End If



        Catch ioe As System.IO.IOException ' Este error puede ocurrir si la carpeta Temp no existe.

            Console.WriteLine("Error de E/S." & ioe.ToString() & "," & ioe.Message)

            MsgBox("Carga módulo virtual GsCatalogo{er02}:" & ioe.Message & "," & ioe.Source & "," & ioe.StackTrace)

            Throw ioe

        Catch se As System.Security.SecurityException ' No tiene el permiso apropiado para
            ' realizar esta acción.
            Console.WriteLine("No tiene los permisos de seguridad para realizar esta acción.")

            MsgBox("Carga módulo virtual GsCatalogo{er03}:" & se.Message & "," & se.Source & "," & se.StackTrace)

            Throw se

        Catch sop As System.ObjectDisposedException

            ' MsgBox("Carga módulo virtual GsCatalogo{er03}:" & se.Message & "," & se.Source & "," & se.StackTrace)

            '  Throw sop

        Catch e As Exception   ' Interceptar todas las demás excepciones.

            Console.WriteLine("Error 1245:" & e.ToString() & "" & e.Message)  ' Imprimir la información de excepción estándar.

            MsgBox("Carga módulo virtual GsCatalogo{er04}:" & _
                    " MENSAJE:{" & e.Message & "}" & Chr(13) & _
                    " FUENTE:{" & e.Source & "}" & Chr(13) & _
                    " TRACE:{" & e.StackTrace & "}" & Chr(13) & _
                    " RUTA:{" & _rutamoduloensamblado & "} " & Chr(13) & _
                    " TIPO SOLICITADO:{" & _tipoensamblado & "} " & Chr(13) & _
                    " TIPO ENCONTRADO:{" & _tipocarga.ToString & "}")

        End Try

    End Sub

    Private Sub AmbienteCurado(ByVal valorModo_ As Boolean)


        'If tsbModo.Checked Then
        If valorModo_ = True Then
            ' Uncheck, las ventanas actualmente son independientes, y se volverán internas
            tsbModo.Checked = False
            _ventanasIndependientes = False


            TsExportCSV.Visible = False
            TsExportExcel.Visible = False
            tsSeparador2.Visible = False
            TsActualizar.Visible = False
            TsEliminar.Visible = False
            TsModificar.Enabled = False

            pnlGestor.Visible = True
            pnlGrid.Visible = False

            If Not _modulodinamicoEmbebido Is Nothing Then
                _modulodinamicoEmbebido.OperacionesCatalogo.ValorIndice = Nothing
            End If

            'Bloqueando filtro avanzado

            TsFiltro.Enabled = False

            'Configuramos la búsqueda normal, la debemos límitar

            If TsTbBuscar.Text = Nothing Then
                TsBuscar.Enabled = False
            Else
                TsBuscar.Enabled = True
            End If

            TsAgregar.PerformClick()

        Else
            ' Checked, las ventanas son internas y se volverán indepedientes
            tsbModo.Checked = True
            _ventanasIndependientes = True


            TsExportCSV.Visible = True
            TsExportExcel.Visible = True
            tsSeparador2.Visible = True
            TsActualizar.Visible = True
            TsEliminar.Visible = True
            TsModificar.Enabled = True

            'Actualizamos la vista por si hubo algún cambio durante
            'ModalidadBusqueda = IOperacionesCatalogo.ModalidadesBusqueda.HiloAsincrono
            pnlGestor.Visible = False
            pnlGrid.Visible = True

            'Reactivamos filtro avanzado

            TsFiltro.Enabled = True

            'Reactivamos la búsqueda normal

            TsBuscar.Enabled = True

            'If _ioperacionescatalogo.GenerarVista("%" & TsTbBuscar.Text) = IOperacionesCatalogo.EstadoOperacion.COk Then

            'End If

            'If _ioperacionescatalogo.Vista.Tables.Count > 0 Then

            '    DgvCatalogo.DataSource = _ioperacionescatalogo.Vista.Tables(0)

            'Else

            '    DgvCatalogo.DataSource = Nothing

            'End If


        End If
    End Sub

    Private Sub tsbModo_Click(sender As Object, e As EventArgs) Handles tsbModo.Click

        Dim valorModo_ As Boolean = tsbModo.Checked

        AmbienteCurado(valorModo_)

    End Sub



    Private Sub pnlGestor_ControlRemoved(sender As Object, e As ControlEventArgs) Handles pnlGestor.ControlRemoved

        If _ventanasIndependientes = False Then

            If _modulodinamicoEmbebido.OperacionesCatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion Then
                If _modulodinamicoEmbebido.OperacionesCatalogo.ValorIndice <> "" And _modulodinamicoEmbebido.OperacionesCatalogo.ValorIndice <> -1 Then
                    ' MsgBox("Se agregó correctamente la clave: " & _modulodinamicoEmbebido.OperacionesCatalogo.ValorIndice)

                    If _contadorDeRegistrosParaInsercion = 0 Then

                        TsAgregar.PerformClick()

                        _contadorDeRegistrosParaInsercion += 1

                    ElseIf _contadorDeRegistrosParaInsercion > 0 Then

                        If MsgBox("Fue agregado exitosamente la clave[" & _modulodinamicoEmbebido.OperacionesCatalogo.ValorIndice & "],¿Deseas agregar otro?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                            TsAgregar.PerformClick()

                            _contadorDeRegistrosParaInsercion += 1

                        End If

                    End If

                    If Not _modulodinamicoEmbebido Is Nothing Then
                        _modulodinamicoEmbebido.OperacionesCatalogo.ValorIndice = Nothing
                    End If

                Else

                End If

            End If

        End If

    End Sub


    Private Sub TsTbBuscar_TextChanged(sender As Object, e As EventArgs) Handles TsTbBuscar.TextChanged

        If _ventanasIndependientes = False Then

            If Trim(TsTbBuscar.Text).Length >= _longitudBusquedaAutomatica Then
                TsBuscar.Enabled = True
            Else
                TsBuscar.Enabled = False
            End If

        End If

    End Sub


    'Private Sub DgvCatalogo_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvCatalogo.CellMouseClick

    '    '::: SUSPENDIDO PARA REVISIÓN :::::

    '    'Dim valorIndicador_ As String = "-1"

    '    'If _ventanasIndependientes Then

    '    '    valorIndicador_ = ObtieneValorIndiceDirecto()

    '    '    PosicionaUltimoIndice(valorIndicador_)

    '    'Else

    '    '    'PENDIENTE IMPLEMENTAR

    '    '    valorIndicador_ = ObtieneValorIndiceDirecto()


    '    'End If


    'End Sub

End Class




Public Class TextAndImageColumn
    Inherits DataGridViewTextBoxColumn

    Private imageValue As Image

    Private imageSize As Size

    Public Sub New()
        Me.CellTemplate = New TextAndImageCell
    End Sub

    Public Overloads Overrides Function Clone() As Object

        Dim c As TextAndImageColumn = CType(TryCast(MyBase.Clone, TextAndImageColumn), TextAndImageColumn)

        c.imageValue = Me.imageValue

        c.imageSize = Me.imageSize

        Return c

    End Function

    Public Property Image() As Image

        Get
            Return Me.imageValue
        End Get

        Set(ByVal value As Image)
            If Not Me.Image Is value Then
                Me.imageValue = value
                Me.imageSize = value.Size
                If Not (Me.InheritedStyle Is Nothing) Then
                    Dim inheritedPadding As Padding = Me.InheritedStyle.Padding
                    Me.DefaultCellStyle.Padding = New Padding(imageSize.Width, inheritedPadding.Top, inheritedPadding.Right, inheritedPadding.Bottom)
                End If
            End If
        End Set

    End Property

    Private ReadOnly Property TextAndImageCellTemplate() As TextAndImageCell

        Get
            Return CType(TryCast(Me.CellTemplate, TextAndImageCell), TextAndImageCell)
        End Get

    End Property

    Friend ReadOnly Property ImageSize_() As Size
        Get
            Return imageSize
        End Get

    End Property

End Class



Public Class TextAndImageCell
    Inherits DataGridViewTextBoxCell

    Private imageValue As Image

    Private imageSize As Size

    Public Overloads Overrides Function Clone() As Object

        Dim c As TextAndImageCell = CType(TryCast(MyBase.Clone, TextAndImageCell), TextAndImageCell)

        c.imageValue = Me.imageValue

        c.imageSize = Me.imageSize

        Return c

    End Function

    Public Property Image() As Image
        Get
            If Me.OwningColumn Is Nothing OrElse Me.OwningTextAndImageColumn Is Nothing Then
                Return imageValue
            Else
                If Not (Me.imageValue Is Nothing) Then
                    Return Me.imageValue
                Else
                    Return Me.OwningTextAndImageColumn.Image
                End If
            End If
        End Get
        Set(ByVal value As Image)
            If Not Me.Image Is value Then
                Me.imageValue = value
                Me.imageSize = value.Size
                Dim inheritedPadding As Padding = Me.InheritedStyle.Padding
                Me.Style.Padding = New Padding(imageSize.Width, inheritedPadding.Top + 5, inheritedPadding.Right, inheritedPadding.Bottom)
            End If
        End Set
    End Property

    Protected Overloads Overrides Sub Paint(ByVal graphics As Graphics, ByVal clipBounds As Rectangle, ByVal cellBounds As Rectangle, ByVal rowIndex As Integer, ByVal cellState As DataGridViewElementStates, ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String, ByVal cellStyle As DataGridViewCellStyle, ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle, ByVal paintParts As DataGridViewPaintParts)
        MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)

        If Not (Me.Image Is Nothing) Then

            Dim container As System.Drawing.Drawing2D.GraphicsContainer = graphics.BeginContainer

            graphics.SetClip(cellBounds)

            graphics.DrawImage(Me.Image, cellBounds.Location.X, cellBounds.Location.Y, Me.Image.Width, Me.Image.Height)

            graphics.EndContainer(container)

        End If

    End Sub

    Private ReadOnly Property OwningTextAndImageColumn() As TextAndImageColumn
        Get
            Return CType(TryCast(Me.OwningColumn, TextAndImageColumn), TextAndImageColumn)
        End Get
    End Property

End Class
