<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm000FormularioBaseCatalogo
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
        Dim EspacioTrabajo1 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm000FormularioBaseCatalogo))
        Dim OperacionesCatalogo1 As gsol.BaseDatos.Operaciones.IOperacionesCatalogo = New gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo2 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Me.GsCatalogo1 = New gsol.Componentes.SistemaBase.GsCatalogo()
        Me.SuspendLayout()
        '
        'GsCatalogo1
        '
        Me.GsCatalogo1.ActivarControlVentanasIndependientes = False
        Me.GsCatalogo1.ActivarGestor = True
        Me.GsCatalogo1.ArchivoEnsamblado = Nothing
        Me.GsCatalogo1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.GsCatalogo1.CatalogoOpcional = False
        Me.GsCatalogo1.ContinuarOperacionActual = True
        Me.GsCatalogo1.Dock = System.Windows.Forms.DockStyle.Fill
        EspacioTrabajo1.ClaveEjecutivo = 0
        EspacioTrabajo1.Idioma = 1
        EspacioTrabajo1.MisCredenciales = Nothing
        EspacioTrabajo1.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo1.SectorEntorno = CType(resources.GetObject("EspacioTrabajo1.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        Me.GsCatalogo1.EspacioTrabajo = EspacioTrabajo1
        Me.GsCatalogo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GsCatalogo1.IdentificadorCatalogo = "<campo llave>"
        Me.GsCatalogo1.IdentificadorEmpresa = "-1"
        Me.GsCatalogo1.IgnorarDobleClickContenidoCelda = False
        Me.GsCatalogo1.Location = New System.Drawing.Point(0, 0)
        Me.GsCatalogo1.LongitudBusquedaAutomatica = True
        Me.GsCatalogo1.ModalidadBusqueda = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir
        Me.GsCatalogo1.MostrarBarraEstado = False
        Me.GsCatalogo1.Name = "GsCatalogo1"
        Me.GsCatalogo1.NombreCatalogo = "<Título del catálogo>"
        OperacionesCatalogo1.AdvertenciasIndicador = True
        OperacionesCatalogo1.CadenaEncabezados = Nothing
        OperacionesCatalogo1.CantidadVisibleRegistros = 100
        OperacionesCatalogo1.Caracteristicas = CType(resources.GetObject("OperacionesCatalogo1.Caracteristicas"), System.Collections.Generic.Dictionary(Of Integer, gsol.BaseDatos.Operaciones.ICaracteristica))
        OperacionesCatalogo1.ClausulasAutoFiltros = Nothing
        OperacionesCatalogo1.ClausulasLibres = Nothing
        OperacionesCatalogo1.ClaveDivisionMiEmpresa = 0
        OperacionesCatalogo1.ComplejoTransaccional = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2
        OperacionesCatalogo1.DeclaracionesAdicionalesUsuario = Nothing
        OperacionesCatalogo1.Dimension = Nothing
        OperacionesCatalogo1.EjecutarPlanEjecucionTransaccional = True
        OperacionesCatalogo1.Entidad = Nothing
        OperacionesCatalogo1.EntradaLlaves = "-1"
        EspacioTrabajo2.ClaveEjecutivo = 0
        EspacioTrabajo2.Idioma = 1
        EspacioTrabajo2.MisCredenciales = Nothing
        EspacioTrabajo2.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo2.SectorEntorno = CType(resources.GetObject("EspacioTrabajo2.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        OperacionesCatalogo1.EspacioTrabajo = EspacioTrabajo2
        OperacionesCatalogo1.EstructuraConsulta = Nothing
        OperacionesCatalogo1.Granularidad = Nothing
        OperacionesCatalogo1.IDAplicacion = gsol.monitoreo.IBitacoras.ClaveTiposAplicacion.SinDefinir
        OperacionesCatalogo1.IdentificadorCatalogo = "<campo llave>"
        OperacionesCatalogo1.IdentificadorEmpresa = "-1"
        OperacionesCatalogo1.IDNivelTransaccional = "1"
        OperacionesCatalogo1.IDObjetoTransaccional = "###"
        OperacionesCatalogo1.IDRecursoSolicitante = 0
        OperacionesCatalogo1.IDUsuario = 0
        OperacionesCatalogo1.IndicePaginacion = 100
        OperacionesCatalogo1.IndiceTablaTemporal = 1
        OperacionesCatalogo1.IndiceTablaTemporalLlamante = Nothing
        OperacionesCatalogo1.InstruccionesAdicionalesPieTransaccion = Nothing
        OperacionesCatalogo1.InstruccionesSQLAntesIniciarTransaccion = Nothing
        OperacionesCatalogo1.ModalidadConsulta = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta.SinDefinir
        OperacionesCatalogo1.Nombre = "<Título del catálogo>"
        OperacionesCatalogo1.NombreClaveUpsert = Nothing
        OperacionesCatalogo1.NoMostrarRegistrosInsertados = False
        OperacionesCatalogo1.ObjetoRepositorio = Nothing
        OperacionesCatalogo1.OperacionAnterior = Nothing
        OperacionesCatalogo1.OperadorCatalogoBorrado = "dbo.Sp000OperadorCatalogosBorrado"
        OperacionesCatalogo1.OperadorCatalogoConsulta = ""
        OperacionesCatalogo1.OperadorCatalogoInsercion = "dbo.Sp000OperadorCatalogosInserciones"
        OperacionesCatalogo1.OperadorCatalogoModificacion = "dbo.Sp000OperadorCatalogosModificaciones"
        OperacionesCatalogo1.PlanEjecucionSQL = Nothing
        OperacionesCatalogo1.RegistrosTemporales = CType(resources.GetObject("OperacionesCatalogo1.RegistrosTemporales"), System.Collections.Generic.Dictionary(Of Integer, String))
        OperacionesCatalogo1.SalidaLlaves = "-1"
        OperacionesCatalogo1.SerieCatalogo = 0
        OperacionesCatalogo1.SQLTransaccion = CType(resources.GetObject("OperacionesCatalogo1.SQLTransaccion"), System.Collections.Generic.Dictionary(Of String, String))
        OperacionesCatalogo1.TablaEdicion = Nothing
        OperacionesCatalogo1.TipoEscritura = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo1.TipoInstrumentacion = gsol.monitoreo.IBitacoras.TiposInstrumentacion.GestorIOperaciones
        OperacionesCatalogo1.TipoOperacionSQL = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposOperacionSQL.Insercion
        OperacionesCatalogo1.ValorIndice = Nothing
        OperacionesCatalogo1.Version = Nothing
        OperacionesCatalogo1.VistaEncabezados = Nothing
        OperacionesCatalogo1.VisualizacionCamposConfigurada = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposVisualizacionCampos.PresentarTodos
        Me.GsCatalogo1.OperacionesCatalogo = OperacionesCatalogo1
        Me.GsCatalogo1.OperadorCatalgosConsulta = ""
        Me.GsCatalogo1.OperadorCatalogosBorrado = "dbo.Sp000OperadorCatalogosBorrado"
        Me.GsCatalogo1.OperadorCatalogosInsercion = "dbo.Sp000OperadorCatalogosInserciones"
        Me.GsCatalogo1.OperadorCatalogosModificaciones = "dbo.Sp000OperadorCatalogosModificaciones"
        Me.GsCatalogo1.PathSaveExcel = ""
        Me.GsCatalogo1.ProcesoBusqueda = Nothing
        Me.GsCatalogo1.SeleccionMultiple = False
        Me.GsCatalogo1.SerieCatalogo = "0"
        Me.GsCatalogo1.Size = New System.Drawing.Size(591, 458)
        Me.GsCatalogo1.TabIndex = 0
        Me.GsCatalogo1.TablaEdicion = Nothing
        Me.GsCatalogo1.TagActualizarConsulta = Nothing
        Me.GsCatalogo1.TagAgregar = Nothing
        Me.GsCatalogo1.TagBuscar = Nothing
        Me.GsCatalogo1.TagEliminar = Nothing
        Me.GsCatalogo1.TagModificar = Nothing
        Me.GsCatalogo1.TemplateExcel = ""
        Me.GsCatalogo1.TipoEnsamblado = Nothing
        Me.GsCatalogo1.VentanasIndependientes = True
        Me.GsCatalogo1.VistaEncabezados = Nothing
        '
        'Frm000FormularioBaseCatalogo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(591, 458)
        Me.Controls.Add(Me.GsCatalogo1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "Frm000FormularioBaseCatalogo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "<Sin Nombre>"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents GsCatalogo1 As gsol.Componentes.SistemaBase.GsCatalogo

End Class
