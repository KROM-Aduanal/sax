Imports gsol.BaseDatos.Operaciones
Imports Gsol



<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm000Generic
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
        Dim EspacioTrabajo1 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm000Generic))
        Dim OperacionesCatalogo1 As gsol.BaseDatos.Operaciones.IOperacionesCatalogo = New gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo2 As Gsol.EspacioTrabajo = New Gsol.EspacioTrabajo()
        Me.GsCatalogo1 = New Gsol.Componentes.SistemaBase.GsCatalogo()
        Me.SuspendLayout()
        '
        'GsCatalogo1
        '
        Me.GsCatalogo1.ArchivoEnsamblado = Nothing
        Me.GsCatalogo1.BackColor = System.Drawing.Color.White
        Me.GsCatalogo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GsCatalogo1.Dock = System.Windows.Forms.DockStyle.Fill
        EspacioTrabajo1.Idioma = 1
        EspacioTrabajo1.MisCredenciales = Nothing
        EspacioTrabajo1.ModalidadEspacio = Gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo1.SectorEntorno = CType(resources.GetObject("EspacioTrabajo1.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, Gsol.ISectorEntorno))
        Me.GsCatalogo1.EspacioTrabajo = EspacioTrabajo1
        Me.GsCatalogo1.ForeColor = System.Drawing.Color.Black
        Me.GsCatalogo1.IdentificadorCatalogo = "i_Cve_Temporal"
        Me.GsCatalogo1.IdentificadorEmpresa = "-1"
        Me.GsCatalogo1.Location = New System.Drawing.Point(0, 0)
        Me.GsCatalogo1.Name = "GsCatalogo1"
        Me.GsCatalogo1.NombreCatalogo = "Título del catálogo"
        OperacionesCatalogo1.CantidadVisibleRegistros = 100
        OperacionesCatalogo1.Caracteristicas = CType(resources.GetObject("OperacionesCatalogo1.Caracteristicas"), System.Collections.Generic.Dictionary(Of Integer, Gsol.BaseDatos.Operaciones.ICaracteristica))
        OperacionesCatalogo1.ClausulasAutoFiltros = Nothing
        OperacionesCatalogo1.ClausulasLibres = Nothing
        OperacionesCatalogo1.EntradaLlaves = "-1"
        EspacioTrabajo2.Idioma = 1
        EspacioTrabajo2.MisCredenciales = Nothing
        EspacioTrabajo2.ModalidadEspacio = Gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        EspacioTrabajo2.SectorEntorno = CType(resources.GetObject("EspacioTrabajo2.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, Gsol.ISectorEntorno))
        OperacionesCatalogo1.EspacioTrabajo = EspacioTrabajo2
        OperacionesCatalogo1.IdentificadorCatalogo = "i_Cve_Temporal"
        OperacionesCatalogo1.IdentificadorEmpresa = "-1"
        OperacionesCatalogo1.IDNivelTransaccional = "1"
        OperacionesCatalogo1.IDObjetoTransaccional = "###"
        OperacionesCatalogo1.IndicePaginacion = 100
        OperacionesCatalogo1.IndiceTablaTemporal = 1
        OperacionesCatalogo1.IndiceTablaTemporalLlamante = Nothing
        OperacionesCatalogo1.Nombre = "Título del catálogo"
        OperacionesCatalogo1.NoMostrarRegistrosInsertados = False
        OperacionesCatalogo1.OperacionAnterior = Nothing
        OperacionesCatalogo1.OperadorCatalogoBorrado = "Sp000OperadorCatalogosBorrado"
        OperacionesCatalogo1.OperadorCatalogoConsulta = "Vt000Temp"
        OperacionesCatalogo1.OperadorCatalogoInsercion = "Sp000OperadorCatalogosInserciones"
        OperacionesCatalogo1.OperadorCatalogoModificacion = "Sp000OperadorCatalogosModificaciones"
        OperacionesCatalogo1.RegistrosTemporales = CType(resources.GetObject("OperacionesCatalogo1.RegistrosTemporales"), System.Collections.Generic.Dictionary(Of Integer, String))
        OperacionesCatalogo1.SalidaLlaves = "-1"
        OperacionesCatalogo1.SerieCatalogo = 0
        OperacionesCatalogo1.SQLTransaccion = CType(resources.GetObject("OperacionesCatalogo1.SQLTransaccion"), System.Collections.Generic.Dictionary(Of String, String))
        OperacionesCatalogo1.TablaEdicion = "Cat000Temp"
        OperacionesCatalogo1.TipoEscritura = Gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo1.ValorIndice = Nothing
        OperacionesCatalogo1.VistaEncabezados = "Ve000IUTemp"
        Me.GsCatalogo1.OperacionesCatalogo = OperacionesCatalogo1
        Me.GsCatalogo1.OperadorCatalgosConsulta = "Vt000Temp"
        Me.GsCatalogo1.OperadorCatalogosBorrado = "Sp000OperadorCatalogosBorrado"
        Me.GsCatalogo1.OperadorCatalogosInsercion = "Sp000OperadorCatalogosInserciones"
        Me.GsCatalogo1.OperadorCatalogosModificaciones = "Sp000OperadorCatalogosModificaciones"
        Me.GsCatalogo1.PathSaveExcel = ""
        Me.GsCatalogo1.SerieCatalogo = "0"
        Me.GsCatalogo1.Size = New System.Drawing.Size(507, 300)
        Me.GsCatalogo1.TabIndex = 0
        Me.GsCatalogo1.TablaEdicion = "Cat000Temp"
        Me.GsCatalogo1.TagActualizarConsulta = "118"
        Me.GsCatalogo1.TagAgregar = "118"
        Me.GsCatalogo1.TagBuscar = "118"
        Me.GsCatalogo1.TagEliminar = "118"
        Me.GsCatalogo1.TagModificar = Nothing
        Me.GsCatalogo1.TemplateExcel = ""
        Me.GsCatalogo1.TipoEnsamblado = "Gsol.Generico.Catalogo.Frm000Generico"
        Me.GsCatalogo1.VistaEncabezados = "Ve000IUTemp"
        '
        'Frm000Generic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(507, 300)
        Me.Controls.Add(Me.GsCatalogo1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "Frm000Generic"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GsCatalogo1 As gsol.Componentes.SistemaBase.GsCatalogo

End Class
