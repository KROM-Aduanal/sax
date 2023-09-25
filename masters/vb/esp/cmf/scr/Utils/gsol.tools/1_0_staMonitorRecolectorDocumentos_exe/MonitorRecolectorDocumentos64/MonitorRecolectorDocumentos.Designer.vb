<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MonitorRecolectorDocumentos
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim EspacioTrabajo3 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MonitorRecolectorDocumentos))
        Dim OperacionesCatalogo2 As gsol.BaseDatos.Operaciones.OperacionesCatalogo = New gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo4 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim EspacioTrabajo5 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim OperacionesCatalogo3 As gsol.BaseDatos.Operaciones.OperacionesCatalogo = New gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo6 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim EspacioTrabajo7 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim OperacionesCatalogo4 As gsol.BaseDatos.Operaciones.OperacionesCatalogo = New gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo8 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Me.GsCatalogo2 = New gsol.Componentes.SistemaBase.GsCatalogo()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tbPageResumen = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnPausar = New System.Windows.Forms.Button()
        Me.btnReiniciar = New System.Windows.Forms.Button()
        Me.btnDetener = New System.Windows.Forms.Button()
        Me.btnIniciar = New System.Windows.Forms.Button()
        Me.GsCatalogo1 = New gsol.Componentes.SistemaBase.GsCatalogo()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.pResumen = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lbFechaAct = New System.Windows.Forms.Label()
        Me.lbFechaActualizacion = New System.Windows.Forms.Label()
        Me.lbResumen = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.rshResumen = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.tbPageBitacora = New System.Windows.Forms.TabPage()
        Me.tbPageErrores = New System.Windows.Forms.TabPage()
        Me.GsCatalogo3 = New gsol.Componentes.SistemaBase.GsCatalogo()
        Me.lbModulo = New System.Windows.Forms.Label()
        Me.lbVersion = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.timerResumen = New System.Windows.Forms.Timer(Me.components)
        Me.timerLiberarMemoria = New System.Windows.Forms.Timer(Me.components)
        Me.btnLiberarMemoria = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.tbPageResumen.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.pResumen.SuspendLayout()
        Me.tbPageBitacora.SuspendLayout()
        Me.tbPageErrores.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GsCatalogo2
        '
        Me.GsCatalogo2.ActivarGestor = True
        Me.GsCatalogo2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GsCatalogo2.ArchivoEnsamblado = Nothing
        Me.GsCatalogo2.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.GsCatalogo2.CatalogoOpcional = False
        Me.GsCatalogo2.ContinuarOperacionActual = True
        EspacioTrabajo3.Idioma = 1
        EspacioTrabajo3.MisCredenciales = Nothing
        EspacioTrabajo3.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo3.SectorEntorno = CType(resources.GetObject("EspacioTrabajo3.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        Me.GsCatalogo2.EspacioTrabajo = EspacioTrabajo3
        Me.GsCatalogo2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GsCatalogo2.IdentificadorCatalogo = "-1"
        Me.GsCatalogo2.IdentificadorEmpresa = "-1"
        Me.GsCatalogo2.IgnorarDobleClickContenidoCelda = False
        Me.GsCatalogo2.Location = New System.Drawing.Point(1, 1)
        Me.GsCatalogo2.ModalidadBusqueda = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir
        Me.GsCatalogo2.MostrarBarraEstado = True
        Me.GsCatalogo2.Name = "GsCatalogo2"
        Me.GsCatalogo2.NombreCatalogo = Nothing
        OperacionesCatalogo2.AdvertenciasIndicador = True
        OperacionesCatalogo2.CadenaEncabezados = Nothing
        OperacionesCatalogo2.CantidadVisibleRegistros = 200
        OperacionesCatalogo2.Caracteristicas = CType(resources.GetObject("OperacionesCatalogo2.Caracteristicas"), System.Collections.Generic.Dictionary(Of Integer, gsol.BaseDatos.Operaciones.ICaracteristica))
        OperacionesCatalogo2.ClausulasAutoFiltros = Nothing
        OperacionesCatalogo2.ClausulasLibres = Nothing
        OperacionesCatalogo2.ClaveDivisionMiEmpresa = 0
        OperacionesCatalogo2.ComplejoTransaccional = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2
        OperacionesCatalogo2.DeclaracionesAdicionalesUsuario = Nothing
        OperacionesCatalogo2.Dimension = Nothing
        OperacionesCatalogo2.EjecutarPlanEjecucionTransaccional = True
        OperacionesCatalogo2.Entidad = Nothing
        OperacionesCatalogo2.EntradaLlaves = "-1"
        EspacioTrabajo4.Idioma = 1
        EspacioTrabajo4.MisCredenciales = Nothing
        EspacioTrabajo4.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo4.SectorEntorno = CType(resources.GetObject("EspacioTrabajo4.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        OperacionesCatalogo2.EspacioTrabajo = EspacioTrabajo4
        OperacionesCatalogo2.EstructuraConsulta = Nothing
        OperacionesCatalogo2.Granularidad = Nothing
        OperacionesCatalogo2.IDAplicacion = gsol.monitoreo.IBitacoras.ClaveTiposAplicacion.SinDefinir
        OperacionesCatalogo2.IdentificadorCatalogo = "-1"
        OperacionesCatalogo2.IdentificadorEmpresa = "-1"
        OperacionesCatalogo2.IDNivelTransaccional = "1"
        OperacionesCatalogo2.IDObjetoTransaccional = "###"
        OperacionesCatalogo2.IDRecursoSolicitante = 0
        OperacionesCatalogo2.IDUsuario = 0
        OperacionesCatalogo2.IndicePaginacion = 100
        OperacionesCatalogo2.IndiceTablaTemporal = 1
        OperacionesCatalogo2.IndiceTablaTemporalLlamante = Nothing
        OperacionesCatalogo2.InstruccionesAdicionalesPieTransaccion = Nothing
        OperacionesCatalogo2.InstruccionesSQLAntesIniciarTransaccion = Nothing
        OperacionesCatalogo2.ModalidadConsulta = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta.SinDefinir
        OperacionesCatalogo2.Nombre = Nothing
        OperacionesCatalogo2.NombreClaveUpsert = Nothing
        OperacionesCatalogo2.NoMostrarRegistrosInsertados = False
        OperacionesCatalogo2.ObjetoRepositorio = Nothing
        OperacionesCatalogo2.OperacionAnterior = Nothing
        OperacionesCatalogo2.OperadorCatalogoBorrado = Nothing
        OperacionesCatalogo2.OperadorCatalogoConsulta = Nothing
        OperacionesCatalogo2.OperadorCatalogoInsercion = Nothing
        OperacionesCatalogo2.OperadorCatalogoModificacion = Nothing
        OperacionesCatalogo2.PlanEjecucionSQL = Nothing
        OperacionesCatalogo2.RegistrosTemporales = CType(resources.GetObject("OperacionesCatalogo2.RegistrosTemporales"), System.Collections.Generic.Dictionary(Of Integer, String))
        OperacionesCatalogo2.SalidaLlaves = "-1"
        OperacionesCatalogo2.SerieCatalogo = 0
        OperacionesCatalogo2.SQLTransaccion = CType(resources.GetObject("OperacionesCatalogo2.SQLTransaccion"), System.Collections.Generic.Dictionary(Of String, String))
        OperacionesCatalogo2.TablaEdicion = Nothing
        OperacionesCatalogo2.TipoEscritura = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo2.TipoInstrumentacion = gsol.monitoreo.IBitacoras.TiposInstrumentacion.GestorIOperaciones
        OperacionesCatalogo2.TipoOperacionSQL = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposOperacionSQL.SinDefinir
        OperacionesCatalogo2.ValorIndice = Nothing
        OperacionesCatalogo2.Version = Nothing
        OperacionesCatalogo2.VistaEncabezados = Nothing
        OperacionesCatalogo2.VisualizacionCamposConfigurada = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos
        Me.GsCatalogo2.OperacionesCatalogo = OperacionesCatalogo2
        Me.GsCatalogo2.OperadorCatalgosConsulta = Nothing
        Me.GsCatalogo2.OperadorCatalogosBorrado = Nothing
        Me.GsCatalogo2.OperadorCatalogosInsercion = Nothing
        Me.GsCatalogo2.OperadorCatalogosModificaciones = Nothing
        Me.GsCatalogo2.PathSaveExcel = ""
        Me.GsCatalogo2.ProcesoBusqueda = Nothing
        Me.GsCatalogo2.SeleccionMultiple = False
        Me.GsCatalogo2.SerieCatalogo = "0"
        Me.GsCatalogo2.Size = New System.Drawing.Size(998, 586)
        Me.GsCatalogo2.TabIndex = 0
        Me.GsCatalogo2.TablaEdicion = Nothing
        Me.GsCatalogo2.TagActualizarConsulta = Nothing
        Me.GsCatalogo2.TagAgregar = Nothing
        Me.GsCatalogo2.TagBuscar = Nothing
        Me.GsCatalogo2.TagEliminar = Nothing
        Me.GsCatalogo2.TagModificar = Nothing
        Me.GsCatalogo2.TemplateExcel = ""
        Me.GsCatalogo2.TipoEnsamblado = Nothing
        Me.GsCatalogo2.VistaEncabezados = Nothing
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tbPageResumen)
        Me.TabControl1.Controls.Add(Me.tbPageBitacora)
        Me.TabControl1.Controls.Add(Me.tbPageErrores)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1007, 613)
        Me.TabControl1.TabIndex = 2
        '
        'tbPageResumen
        '
        Me.tbPageResumen.Controls.Add(Me.SplitContainer1)
        Me.tbPageResumen.Location = New System.Drawing.Point(4, 22)
        Me.tbPageResumen.Name = "tbPageResumen"
        Me.tbPageResumen.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPageResumen.Size = New System.Drawing.Size(999, 587)
        Me.tbPageResumen.TabIndex = 1
        Me.tbPageResumen.Text = "Resumen"
        Me.tbPageResumen.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel4)
        Me.SplitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel3)
        Me.SplitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SplitContainer1.Size = New System.Drawing.Size(993, 581)
        Me.SplitContainer1.SplitterDistance = 481
        Me.SplitContainer1.TabIndex = 51
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Panel2)
        Me.Panel4.Controls.Add(Me.GsCatalogo1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(481, 581)
        Me.Panel4.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.SystemColors.Window
        Me.Panel2.Controls.Add(Me.btnPausar)
        Me.Panel2.Controls.Add(Me.btnReiniciar)
        Me.Panel2.Controls.Add(Me.btnDetener)
        Me.Panel2.Controls.Add(Me.btnIniciar)
        Me.Panel2.Location = New System.Drawing.Point(0, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(481, 40)
        Me.Panel2.TabIndex = 8
        '
        'btnPausar
        '
        Me.btnPausar.BackColor = System.Drawing.Color.Transparent
        Me.btnPausar.Enabled = False
        Me.btnPausar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPausar.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPausar.ForeColor = System.Drawing.Color.Gray
        Me.btnPausar.Image = CType(resources.GetObject("btnPausar.Image"), System.Drawing.Image)
        Me.btnPausar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPausar.Location = New System.Drawing.Point(231, 3)
        Me.btnPausar.Name = "btnPausar"
        Me.btnPausar.Size = New System.Drawing.Size(94, 34)
        Me.btnPausar.TabIndex = 6
        Me.btnPausar.Text = "Pausar   recolección"
        Me.btnPausar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPausar.UseVisualStyleBackColor = False
        '
        'btnReiniciar
        '
        Me.btnReiniciar.BackColor = System.Drawing.Color.Transparent
        Me.btnReiniciar.Enabled = False
        Me.btnReiniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReiniciar.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReiniciar.ForeColor = System.Drawing.Color.Gray
        Me.btnReiniciar.Image = CType(resources.GetObject("btnReiniciar.Image"), System.Drawing.Image)
        Me.btnReiniciar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReiniciar.Location = New System.Drawing.Point(119, 3)
        Me.btnReiniciar.Name = "btnReiniciar"
        Me.btnReiniciar.Size = New System.Drawing.Size(94, 34)
        Me.btnReiniciar.TabIndex = 3
        Me.btnReiniciar.Text = "Reiniciar recolección"
        Me.btnReiniciar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReiniciar.UseVisualStyleBackColor = False
        '
        'btnDetener
        '
        Me.btnDetener.BackColor = System.Drawing.Color.Transparent
        Me.btnDetener.Enabled = False
        Me.btnDetener.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDetener.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDetener.ForeColor = System.Drawing.Color.Gray
        Me.btnDetener.Image = CType(resources.GetObject("btnDetener.Image"), System.Drawing.Image)
        Me.btnDetener.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDetener.Location = New System.Drawing.Point(343, 3)
        Me.btnDetener.Name = "btnDetener"
        Me.btnDetener.Size = New System.Drawing.Size(94, 34)
        Me.btnDetener.TabIndex = 5
        Me.btnDetener.Text = "Detener recolección"
        Me.btnDetener.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDetener.UseVisualStyleBackColor = False
        '
        'btnIniciar
        '
        Me.btnIniciar.BackColor = System.Drawing.Color.Transparent
        Me.btnIniciar.Enabled = False
        Me.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnIniciar.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnIniciar.ForeColor = System.Drawing.Color.Gray
        Me.btnIniciar.Image = CType(resources.GetObject("btnIniciar.Image"), System.Drawing.Image)
        Me.btnIniciar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIniciar.Location = New System.Drawing.Point(5, 3)
        Me.btnIniciar.Name = "btnIniciar"
        Me.btnIniciar.Size = New System.Drawing.Size(94, 34)
        Me.btnIniciar.TabIndex = 7
        Me.btnIniciar.Text = "Iniciar      recolección"
        Me.btnIniciar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIniciar.UseVisualStyleBackColor = False
        '
        'GsCatalogo1
        '
        Me.GsCatalogo1.ActivarGestor = False
        Me.GsCatalogo1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GsCatalogo1.ArchivoEnsamblado = "C:\SVN\SVN QA\Modulos\Gsol.Modulos.GestionRecolectorDocumentos64.0.0.0.0.dll"
        Me.GsCatalogo1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.GsCatalogo1.CatalogoOpcional = False
        Me.GsCatalogo1.ContinuarOperacionActual = True
        EspacioTrabajo5.Idioma = 1
        EspacioTrabajo5.MisCredenciales = Nothing
        EspacioTrabajo5.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo5.SectorEntorno = CType(resources.GetObject("EspacioTrabajo5.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        Me.GsCatalogo1.EspacioTrabajo = EspacioTrabajo5
        Me.GsCatalogo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GsCatalogo1.IdentificadorCatalogo = "i_Cve_RecolectorDocumentos"
        Me.GsCatalogo1.IdentificadorEmpresa = "-1"
        Me.GsCatalogo1.IgnorarDobleClickContenidoCelda = False
        Me.GsCatalogo1.Location = New System.Drawing.Point(2, 50)
        Me.GsCatalogo1.ModalidadBusqueda = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir
        Me.GsCatalogo1.MostrarBarraEstado = True
        Me.GsCatalogo1.Name = "GsCatalogo1"
        Me.GsCatalogo1.NombreCatalogo = Nothing
        OperacionesCatalogo3.AdvertenciasIndicador = True
        OperacionesCatalogo3.CadenaEncabezados = Nothing
        OperacionesCatalogo3.CantidadVisibleRegistros = 200
        OperacionesCatalogo3.Caracteristicas = CType(resources.GetObject("OperacionesCatalogo3.Caracteristicas"), System.Collections.Generic.Dictionary(Of Integer, gsol.BaseDatos.Operaciones.ICaracteristica))
        OperacionesCatalogo3.ClausulasAutoFiltros = Nothing
        OperacionesCatalogo3.ClausulasLibres = Nothing
        OperacionesCatalogo3.ClaveDivisionMiEmpresa = 0
        OperacionesCatalogo3.ComplejoTransaccional = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2
        OperacionesCatalogo3.DeclaracionesAdicionalesUsuario = Nothing
        OperacionesCatalogo3.Dimension = Nothing
        OperacionesCatalogo3.EjecutarPlanEjecucionTransaccional = True
        OperacionesCatalogo3.Entidad = Nothing
        OperacionesCatalogo3.EntradaLlaves = "-1"
        EspacioTrabajo6.Idioma = 1
        EspacioTrabajo6.MisCredenciales = Nothing
        EspacioTrabajo6.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo6.SectorEntorno = CType(resources.GetObject("EspacioTrabajo6.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        OperacionesCatalogo3.EspacioTrabajo = EspacioTrabajo6
        OperacionesCatalogo3.EstructuraConsulta = Nothing
        OperacionesCatalogo3.Granularidad = Nothing
        OperacionesCatalogo3.IDAplicacion = gsol.monitoreo.IBitacoras.ClaveTiposAplicacion.SinDefinir
        OperacionesCatalogo3.IdentificadorCatalogo = "i_Cve_RecolectorDocumentos"
        OperacionesCatalogo3.IdentificadorEmpresa = "-1"
        OperacionesCatalogo3.IDNivelTransaccional = "1"
        OperacionesCatalogo3.IDObjetoTransaccional = "###"
        OperacionesCatalogo3.IDRecursoSolicitante = 0
        OperacionesCatalogo3.IDUsuario = 0
        OperacionesCatalogo3.IndicePaginacion = 100
        OperacionesCatalogo3.IndiceTablaTemporal = 1
        OperacionesCatalogo3.IndiceTablaTemporalLlamante = Nothing
        OperacionesCatalogo3.InstruccionesAdicionalesPieTransaccion = Nothing
        OperacionesCatalogo3.InstruccionesSQLAntesIniciarTransaccion = Nothing
        OperacionesCatalogo3.ModalidadConsulta = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta.SinDefinir
        OperacionesCatalogo3.Nombre = Nothing
        OperacionesCatalogo3.NombreClaveUpsert = Nothing
        OperacionesCatalogo3.NoMostrarRegistrosInsertados = False
        OperacionesCatalogo3.ObjetoRepositorio = Nothing
        OperacionesCatalogo3.OperacionAnterior = Nothing
        OperacionesCatalogo3.OperadorCatalogoBorrado = "Sp000OperadorCatalogosBorrado"
        OperacionesCatalogo3.OperadorCatalogoConsulta = "Vt030RecolectorDocumentos"
        OperacionesCatalogo3.OperadorCatalogoInsercion = "Sp000OperadorCatalogosInserciones"
        OperacionesCatalogo3.OperadorCatalogoModificacion = "Sp000OperadorCatalogosModificaciones"
        OperacionesCatalogo3.PlanEjecucionSQL = Nothing
        OperacionesCatalogo3.RegistrosTemporales = CType(resources.GetObject("OperacionesCatalogo3.RegistrosTemporales"), System.Collections.Generic.Dictionary(Of Integer, String))
        OperacionesCatalogo3.SalidaLlaves = "-1"
        OperacionesCatalogo3.SerieCatalogo = 0
        OperacionesCatalogo3.SQLTransaccion = CType(resources.GetObject("OperacionesCatalogo3.SQLTransaccion"), System.Collections.Generic.Dictionary(Of String, String))
        OperacionesCatalogo3.TablaEdicion = "Reg030RecolectorDocumentos"
        OperacionesCatalogo3.TipoEscritura = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo3.TipoInstrumentacion = gsol.monitoreo.IBitacoras.TiposInstrumentacion.GestorIOperaciones
        OperacionesCatalogo3.TipoOperacionSQL = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposOperacionSQL.SinDefinir
        OperacionesCatalogo3.ValorIndice = Nothing
        OperacionesCatalogo3.Version = Nothing
        OperacionesCatalogo3.VistaEncabezados = "Ve030IURecolectorDocumentos"
        OperacionesCatalogo3.VisualizacionCamposConfigurada = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos
        Me.GsCatalogo1.OperacionesCatalogo = OperacionesCatalogo3
        Me.GsCatalogo1.OperadorCatalgosConsulta = "Vt030RecolectorDocumentos"
        Me.GsCatalogo1.OperadorCatalogosBorrado = "Sp000OperadorCatalogosBorrado"
        Me.GsCatalogo1.OperadorCatalogosInsercion = "Sp000OperadorCatalogosInserciones"
        Me.GsCatalogo1.OperadorCatalogosModificaciones = "Sp000OperadorCatalogosModificaciones"
        Me.GsCatalogo1.PathSaveExcel = ""
        Me.GsCatalogo1.ProcesoBusqueda = Nothing
        Me.GsCatalogo1.SeleccionMultiple = False
        Me.GsCatalogo1.SerieCatalogo = "0"
        Me.GsCatalogo1.Size = New System.Drawing.Size(476, 531)
        Me.GsCatalogo1.TabIndex = 0
        Me.GsCatalogo1.TablaEdicion = "Reg030RecolectorDocumentos"
        Me.GsCatalogo1.TagActualizarConsulta = "1016"
        Me.GsCatalogo1.TagAgregar = "0"
        Me.GsCatalogo1.TagBuscar = "1016"
        Me.GsCatalogo1.TagEliminar = "0"
        Me.GsCatalogo1.TagModificar = "0"
        Me.GsCatalogo1.TemplateExcel = ""
        Me.GsCatalogo1.TipoEnsamblado = "Gsol.Modulos.GestionRecolectorDocumentos64.frm030GestorRecolectorDocumentos"
        Me.GsCatalogo1.VistaEncabezados = "Ve030IURecolectorDocumentos"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.Controls.Add(Me.pResumen)
        Me.Panel3.Controls.Add(Me.lbFechaAct)
        Me.Panel3.Controls.Add(Me.lbFechaActualizacion)
        Me.Panel3.Controls.Add(Me.lbResumen)
        Me.Panel3.Controls.Add(Me.ShapeContainer1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(508, 581)
        Me.Panel3.TabIndex = 50
        '
        'pResumen
        '
        Me.pResumen.AutoScroll = True
        Me.pResumen.BackColor = System.Drawing.SystemColors.Window
        Me.pResumen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pResumen.Controls.Add(Me.Label1)
        Me.pResumen.Controls.Add(Me.TableLayoutPanel1)
        Me.pResumen.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pResumen.Location = New System.Drawing.Point(0, 28)
        Me.pResumen.Name = "pResumen"
        Me.pResumen.Size = New System.Drawing.Size(508, 553)
        Me.pResumen.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(506, 551)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = " Seleccione (doble clic) algún recolector de documentos para ver su resumen"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.75283!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.24717!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87.0!))
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(506, 0)
        Me.TableLayoutPanel1.TabIndex = 56
        '
        'lbFechaAct
        '
        Me.lbFechaAct.AutoSize = True
        Me.lbFechaAct.BackColor = System.Drawing.Color.DarkGray
        Me.lbFechaAct.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbFechaAct.ForeColor = System.Drawing.Color.White
        Me.lbFechaAct.Location = New System.Drawing.Point(192, 6)
        Me.lbFechaAct.Name = "lbFechaAct"
        Me.lbFechaAct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbFechaAct.Size = New System.Drawing.Size(156, 15)
        Me.lbFechaAct.TabIndex = 48
        Me.lbFechaAct.Text = "Fecha última actualización:"
        '
        'lbFechaActualizacion
        '
        Me.lbFechaActualizacion.AutoSize = True
        Me.lbFechaActualizacion.BackColor = System.Drawing.Color.DarkGray
        Me.lbFechaActualizacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbFechaActualizacion.ForeColor = System.Drawing.Color.DarkRed
        Me.lbFechaActualizacion.Location = New System.Drawing.Point(349, 6)
        Me.lbFechaActualizacion.Name = "lbFechaActualizacion"
        Me.lbFechaActualizacion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbFechaActualizacion.Size = New System.Drawing.Size(12, 16)
        Me.lbFechaActualizacion.TabIndex = 49
        Me.lbFechaActualizacion.Text = "-"
        '
        'lbResumen
        '
        Me.lbResumen.AutoSize = True
        Me.lbResumen.BackColor = System.Drawing.Color.DarkGray
        Me.lbResumen.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbResumen.ForeColor = System.Drawing.Color.White
        Me.lbResumen.Location = New System.Drawing.Point(5, 5)
        Me.lbResumen.Name = "lbResumen"
        Me.lbResumen.Size = New System.Drawing.Size(85, 20)
        Me.lbResumen.TabIndex = 48
        Me.lbResumen.Text = "Resumen"
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.rshResumen})
        Me.ShapeContainer1.Size = New System.Drawing.Size(508, 581)
        Me.ShapeContainer1.TabIndex = 0
        Me.ShapeContainer1.TabStop = False
        '
        'rshResumen
        '
        Me.rshResumen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rshResumen.BackColor = System.Drawing.Color.Transparent
        Me.rshResumen.FillColor = System.Drawing.Color.DarkGray
        Me.rshResumen.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.rshResumen.Location = New System.Drawing.Point(0, 0)
        Me.rshResumen.Name = "rshResumen"
        Me.rshResumen.Size = New System.Drawing.Size(507, 28)
        '
        'tbPageBitacora
        '
        Me.tbPageBitacora.Controls.Add(Me.GsCatalogo2)
        Me.tbPageBitacora.Location = New System.Drawing.Point(4, 22)
        Me.tbPageBitacora.Name = "tbPageBitacora"
        Me.tbPageBitacora.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPageBitacora.Size = New System.Drawing.Size(999, 587)
        Me.tbPageBitacora.TabIndex = 0
        Me.tbPageBitacora.Text = "Bitácora"
        Me.tbPageBitacora.UseVisualStyleBackColor = True
        '
        'tbPageErrores
        '
        Me.tbPageErrores.Controls.Add(Me.GsCatalogo3)
        Me.tbPageErrores.Location = New System.Drawing.Point(4, 22)
        Me.tbPageErrores.Name = "tbPageErrores"
        Me.tbPageErrores.Size = New System.Drawing.Size(999, 587)
        Me.tbPageErrores.TabIndex = 3
        Me.tbPageErrores.Text = "Errores"
        Me.tbPageErrores.UseVisualStyleBackColor = True
        '
        'GsCatalogo3
        '
        Me.GsCatalogo3.ActivarGestor = True
        Me.GsCatalogo3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GsCatalogo3.ArchivoEnsamblado = Nothing
        Me.GsCatalogo3.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.GsCatalogo3.CatalogoOpcional = False
        Me.GsCatalogo3.ContinuarOperacionActual = True
        EspacioTrabajo7.Idioma = 1
        EspacioTrabajo7.MisCredenciales = Nothing
        EspacioTrabajo7.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo7.SectorEntorno = Nothing
        Me.GsCatalogo3.EspacioTrabajo = EspacioTrabajo7
        Me.GsCatalogo3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GsCatalogo3.IdentificadorCatalogo = "-1"
        Me.GsCatalogo3.IdentificadorEmpresa = "-1"
        Me.GsCatalogo3.IgnorarDobleClickContenidoCelda = False
        Me.GsCatalogo3.Location = New System.Drawing.Point(1, 1)
        Me.GsCatalogo3.ModalidadBusqueda = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir
        Me.GsCatalogo3.MostrarBarraEstado = True
        Me.GsCatalogo3.Name = "GsCatalogo3"
        Me.GsCatalogo3.NombreCatalogo = Nothing
        OperacionesCatalogo4.AdvertenciasIndicador = True
        OperacionesCatalogo4.CadenaEncabezados = Nothing
        OperacionesCatalogo4.CantidadVisibleRegistros = 200
        OperacionesCatalogo4.Caracteristicas = Nothing
        OperacionesCatalogo4.ClausulasAutoFiltros = Nothing
        OperacionesCatalogo4.ClausulasLibres = Nothing
        OperacionesCatalogo4.ClaveDivisionMiEmpresa = 0
        OperacionesCatalogo4.ComplejoTransaccional = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2
        OperacionesCatalogo4.DeclaracionesAdicionalesUsuario = Nothing
        OperacionesCatalogo4.Dimension = Nothing
        OperacionesCatalogo4.EjecutarPlanEjecucionTransaccional = True
        OperacionesCatalogo4.Entidad = Nothing
        OperacionesCatalogo4.EntradaLlaves = "-1"
        EspacioTrabajo8.Idioma = 1
        EspacioTrabajo8.MisCredenciales = Nothing
        EspacioTrabajo8.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo8.SectorEntorno = Nothing
        OperacionesCatalogo4.EspacioTrabajo = EspacioTrabajo8
        OperacionesCatalogo4.EstructuraConsulta = Nothing
        OperacionesCatalogo4.Granularidad = Nothing
        OperacionesCatalogo4.IDAplicacion = gsol.monitoreo.IBitacoras.ClaveTiposAplicacion.SinDefinir
        OperacionesCatalogo4.IdentificadorCatalogo = "-1"
        OperacionesCatalogo4.IdentificadorEmpresa = "-1"
        OperacionesCatalogo4.IDNivelTransaccional = "1"
        OperacionesCatalogo4.IDObjetoTransaccional = "###"
        OperacionesCatalogo4.IDRecursoSolicitante = 0
        OperacionesCatalogo4.IDUsuario = 0
        OperacionesCatalogo4.IndicePaginacion = 100
        OperacionesCatalogo4.IndiceTablaTemporal = 1
        OperacionesCatalogo4.IndiceTablaTemporalLlamante = Nothing
        OperacionesCatalogo4.InstruccionesAdicionalesPieTransaccion = Nothing
        OperacionesCatalogo4.InstruccionesSQLAntesIniciarTransaccion = Nothing
        OperacionesCatalogo4.ModalidadConsulta = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta.SinDefinir
        OperacionesCatalogo4.Nombre = Nothing
        OperacionesCatalogo4.NombreClaveUpsert = Nothing
        OperacionesCatalogo4.NoMostrarRegistrosInsertados = False
        OperacionesCatalogo4.ObjetoRepositorio = Nothing
        OperacionesCatalogo4.OperacionAnterior = Nothing
        OperacionesCatalogo4.OperadorCatalogoBorrado = Nothing
        OperacionesCatalogo4.OperadorCatalogoConsulta = Nothing
        OperacionesCatalogo4.OperadorCatalogoInsercion = Nothing
        OperacionesCatalogo4.OperadorCatalogoModificacion = Nothing
        OperacionesCatalogo4.PlanEjecucionSQL = Nothing
        OperacionesCatalogo4.RegistrosTemporales = Nothing
        OperacionesCatalogo4.SalidaLlaves = "-1"
        OperacionesCatalogo4.SerieCatalogo = 0
        OperacionesCatalogo4.SQLTransaccion = Nothing
        OperacionesCatalogo4.TablaEdicion = Nothing
        OperacionesCatalogo4.TipoEscritura = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo4.TipoInstrumentacion = gsol.monitoreo.IBitacoras.TiposInstrumentacion.GestorIOperaciones
        OperacionesCatalogo4.TipoOperacionSQL = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposOperacionSQL.SinDefinir
        OperacionesCatalogo4.ValorIndice = Nothing
        OperacionesCatalogo4.Version = Nothing
        OperacionesCatalogo4.VistaEncabezados = Nothing
        OperacionesCatalogo4.VisualizacionCamposConfigurada = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos
        Me.GsCatalogo3.OperacionesCatalogo = OperacionesCatalogo4
        Me.GsCatalogo3.OperadorCatalgosConsulta = Nothing
        Me.GsCatalogo3.OperadorCatalogosBorrado = Nothing
        Me.GsCatalogo3.OperadorCatalogosInsercion = Nothing
        Me.GsCatalogo3.OperadorCatalogosModificaciones = Nothing
        Me.GsCatalogo3.PathSaveExcel = ""
        Me.GsCatalogo3.ProcesoBusqueda = Nothing
        Me.GsCatalogo3.SeleccionMultiple = False
        Me.GsCatalogo3.SerieCatalogo = "0"
        Me.GsCatalogo3.Size = New System.Drawing.Size(998, 586)
        Me.GsCatalogo3.TabIndex = 1
        Me.GsCatalogo3.TablaEdicion = Nothing
        Me.GsCatalogo3.TagActualizarConsulta = Nothing
        Me.GsCatalogo3.TagAgregar = Nothing
        Me.GsCatalogo3.TagBuscar = Nothing
        Me.GsCatalogo3.TagEliminar = Nothing
        Me.GsCatalogo3.TagModificar = Nothing
        Me.GsCatalogo3.TemplateExcel = ""
        Me.GsCatalogo3.TipoEnsamblado = Nothing
        Me.GsCatalogo3.VistaEncabezados = Nothing
        '
        'lbModulo
        '
        Me.lbModulo.AutoSize = True
        Me.lbModulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbModulo.ForeColor = System.Drawing.Color.DimGray
        Me.lbModulo.Location = New System.Drawing.Point(10, 11)
        Me.lbModulo.Name = "lbModulo"
        Me.lbModulo.Size = New System.Drawing.Size(471, 31)
        Me.lbModulo.TabIndex = 6
        Me.lbModulo.Text = "Monitor de Recolector de documentos"
        '
        'lbVersion
        '
        Me.lbVersion.AutoSize = True
        Me.lbVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbVersion.ForeColor = System.Drawing.Color.DimGray
        Me.lbVersion.Location = New System.Drawing.Point(481, 22)
        Me.lbVersion.Name = "lbVersion"
        Me.lbVersion.Size = New System.Drawing.Size(69, 17)
        Me.lbVersion.TabIndex = 7
        Me.lbVersion.Text = "VERSION"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Window
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 50)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1007, 613)
        Me.Panel1.TabIndex = 8
        '
        'timerResumen
        '
        Me.timerResumen.Enabled = True
        Me.timerResumen.Interval = 180000
        '
        'timerLiberarMemoria
        '
        Me.timerLiberarMemoria.Interval = 86400000
        '
        'btnLiberarMemoria
        '
        Me.btnLiberarMemoria.BackColor = System.Drawing.Color.Transparent
        Me.btnLiberarMemoria.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnLiberarMemoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLiberarMemoria.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLiberarMemoria.ForeColor = System.Drawing.Color.Gray
        Me.btnLiberarMemoria.Image = Global.MonitorRecolectorDocumentos64.My.Resources.Resources.broom__2_
        Me.btnLiberarMemoria.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLiberarMemoria.Location = New System.Drawing.Point(903, 0)
        Me.btnLiberarMemoria.Name = "btnLiberarMemoria"
        Me.btnLiberarMemoria.Size = New System.Drawing.Size(104, 50)
        Me.btnLiberarMemoria.TabIndex = 8
        Me.btnLiberarMemoria.Text = "Liberar memoria"
        Me.btnLiberarMemoria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLiberarMemoria.UseVisualStyleBackColor = False
        '
        'MonitorRecolectorDocumentos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(1007, 663)
        Me.Controls.Add(Me.btnLiberarMemoria)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lbVersion)
        Me.Controls.Add(Me.lbModulo)
        Me.Name = "MonitorRecolectorDocumentos"
        Me.Text = "Monitor de Recolector de documentos"
        Me.TabControl1.ResumeLayout(False)
        Me.tbPageResumen.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.pResumen.ResumeLayout(False)
        Me.pResumen.PerformLayout()
        Me.tbPageBitacora.ResumeLayout(False)
        Me.tbPageErrores.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GsCatalogo2 As gsol.Componentes.SistemaBase.GsCatalogo
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tbPageBitacora As System.Windows.Forms.TabPage
    Friend WithEvents tbPageResumen As System.Windows.Forms.TabPage
    Friend WithEvents btnReiniciar As System.Windows.Forms.Button
    Friend WithEvents btnIniciar As System.Windows.Forms.Button
    Friend WithEvents btnPausar As System.Windows.Forms.Button
    Friend WithEvents btnDetener As System.Windows.Forms.Button
    Friend WithEvents pResumen As System.Windows.Forms.Panel
    Friend WithEvents GsCatalogo1 As gsol.Componentes.SistemaBase.GsCatalogo
    Friend WithEvents lbModulo As System.Windows.Forms.Label
    Friend WithEvents lbResumen As System.Windows.Forms.Label
    Friend WithEvents rshResumen As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents lbFechaActualizacion As System.Windows.Forms.Label
    Friend WithEvents lbFechaAct As System.Windows.Forms.Label
    Friend WithEvents tbPageErrores As System.Windows.Forms.TabPage
    Friend WithEvents GsCatalogo3 As gsol.Componentes.SistemaBase.GsCatalogo
    Friend WithEvents lbVersion As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents timerResumen As System.Windows.Forms.Timer
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents timerLiberarMemoria As System.Windows.Forms.Timer
    Friend WithEvents btnLiberarMemoria As System.Windows.Forms.Button

End Class
