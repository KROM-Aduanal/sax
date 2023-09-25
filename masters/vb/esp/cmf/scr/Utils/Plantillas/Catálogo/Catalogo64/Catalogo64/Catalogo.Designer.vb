<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm000Catalogo
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
        Dim EspacioTrabajo3 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm000Catalogo))
        Dim OperacionesCatalogo2 As gsol.BaseDatos.Operaciones.OperacionesCatalogo = New gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo4 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Me.GsCatalogo1 = New gsol.Componentes.SistemaBase.GsCatalogo()
        Me.SuspendLayout()
        '
        'GsCatalogo1
        '
        Me.GsCatalogo1.ArchivoEnsamblado = Nothing
        Me.GsCatalogo1.Dock = System.Windows.Forms.DockStyle.Fill
        EspacioTrabajo3.Idioma = 1
        EspacioTrabajo3.MisCredenciales = Nothing
        EspacioTrabajo3.SectorEntorno = CType(resources.GetObject("EspacioTrabajo3.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        Me.GsCatalogo1.EspacioTrabajo = EspacioTrabajo3
        Me.GsCatalogo1.IdentificadorCatalogo = "i_Cve__"
        Me.GsCatalogo1.IdentificadorEmpresa = "-1"
        Me.GsCatalogo1.Location = New System.Drawing.Point(0, 0)
        Me.GsCatalogo1.Name = "GsCatalogo1"
        Me.GsCatalogo1.NombreCatalogo = "Catálogo"
        OperacionesCatalogo2.CantidadVisibleRegistros = 100
        OperacionesCatalogo2.Caracteristicas = CType(resources.GetObject("OperacionesCatalogo2.Caracteristicas"), System.Collections.Generic.Dictionary(Of Integer, gsol.BaseDatos.Operaciones.ICaracteristica))
        OperacionesCatalogo2.ClausulasLibres = Nothing
        EspacioTrabajo4.Idioma = 1
        EspacioTrabajo4.MisCredenciales = Nothing
        EspacioTrabajo4.SectorEntorno = CType(resources.GetObject("EspacioTrabajo4.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        OperacionesCatalogo2.EspacioTrabajo = EspacioTrabajo4
        OperacionesCatalogo2.IdentificadorCatalogo = "i_Cve__"
        OperacionesCatalogo2.IdentificadorEmpresa = "-1"
        OperacionesCatalogo2.IndicePaginacion = 100
        OperacionesCatalogo2.Nombre = "Catálogo"
        OperacionesCatalogo2.OperadorCatalogoBorrado = "Sp000OperadorCatalogosBorrado"
        OperacionesCatalogo2.OperadorCatalogoConsulta = "Vt000Default"
        OperacionesCatalogo2.OperadorCatalogoInsercion = "Sp000OperadorCatalogosInserciones"
        OperacionesCatalogo2.OperadorCatalogoModificacion = "Sp000OperadorCatalogosModificaciones"
        OperacionesCatalogo2.RegistrosTemporales = CType(resources.GetObject("OperacionesCatalogo2.RegistrosTemporales"), System.Collections.Generic.Dictionary(Of Integer, String))
        OperacionesCatalogo2.SerieCatalogo = 0
        OperacionesCatalogo2.TablaEdicion = "118"
        OperacionesCatalogo2.TipoEscritura = gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo2.ValorIndice = Nothing
        OperacionesCatalogo2.VistaEncabezados = "Ve000IUDefault"
        Me.GsCatalogo1.OperacionesCatalogo = OperacionesCatalogo2
        Me.GsCatalogo1.OperadorCatalgosConsulta = "Vt000Default"
        Me.GsCatalogo1.OperadorCatalogosBorrado = "Sp000OperadorCatalogosBorrado"
        Me.GsCatalogo1.OperadorCatalogosInsercion = "Sp000OperadorCatalogosInserciones"
        Me.GsCatalogo1.OperadorCatalogosModificaciones = "Sp000OperadorCatalogosModificaciones"
        Me.GsCatalogo1.SerieCatalogo = "0"
        Me.GsCatalogo1.Size = New System.Drawing.Size(610, 411)
        Me.GsCatalogo1.TabIndex = 0
        Me.GsCatalogo1.TablaEdicion = "118"
        Me.GsCatalogo1.TagActualizarConsulta = "118"
        Me.GsCatalogo1.TagAgregar = "118"
        Me.GsCatalogo1.TagBuscar = "118"
        Me.GsCatalogo1.TagEliminar = "118"
        Me.GsCatalogo1.TagModificar = "118"
        Me.GsCatalogo1.TipoEnsamblado = "Gsol.Generico.Catalogo.Frm000Generico"
        Me.GsCatalogo1.VistaEncabezados = "Ve000IUDefault"
        '
        'frm000Catalogo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(610, 411)
        Me.Controls.Add(Me.GsCatalogo1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frm000Catalogo"
        Me.Text = "Catálogo"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GsCatalogo1 As Gsol.Componentes.SistemaBase.GsCatalogo

End Class
