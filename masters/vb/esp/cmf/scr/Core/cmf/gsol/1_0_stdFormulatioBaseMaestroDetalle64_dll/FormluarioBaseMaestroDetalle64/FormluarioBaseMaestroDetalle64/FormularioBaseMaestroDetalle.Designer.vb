<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormularioBaseMaestroDetalle
    Inherits FormularioBase64

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormularioBaseMaestroDetalle))
        Dim OperacionesCatalogo1 As Gsol.BaseDatos.Operaciones.OperacionesCatalogo = New Gsol.BaseDatos.Operaciones.OperacionesCatalogo()
        Dim EspacioTrabajo2 As gsol.EspacioTrabajo = New gsol.EspacioTrabajo()
        Me.GsCatalogo1 = New gsol.Componentes.SistemaBase.GsCatalogo()
        Me.SuspendLayout()
        '
        'LblMensaje
        '
        Me.LblMensaje.Location = New System.Drawing.Point(24, 405)
        '
        'btnCancelar
        '
        Me.btnCancelar.FlatAppearance.BorderSize = 0
        '
        'btnAceptar
        '
        Me.btnAceptar.FlatAppearance.BorderSize = 0
        '
        'GsCatalogo1
        '

        EspacioTrabajo1.Idioma = 1
        EspacioTrabajo1.MisCredenciales = Nothing
        EspacioTrabajo1.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        'EspacioTrabajo1.SectorEntorno = CType(resources.GetObject("EspacioTrabajo1.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))


        OperacionesCatalogo1.CantidadVisibleRegistros = 100
        'OperacionesCatalogo1.Caracteristicas = CType(resources.GetObject("OperacionesCatalogo1.Caracteristicas"), System.Collections.Generic.Dictionary(Of Integer, Gsol.BaseDatos.Operaciones.ICaracteristica))
        OperacionesCatalogo1.ClausulasAutoFiltros = Nothing
        OperacionesCatalogo1.ClausulasLibres = Nothing
        OperacionesCatalogo1.EntradaLlaves = "-1"

        EspacioTrabajo2.Idioma = 1
        EspacioTrabajo2.MisCredenciales = Nothing
        EspacioTrabajo2.ModalidadEspacio = gsol.IEspacioTrabajo.ModalidadesEspacio.Produccion
        'EspacioTrabajo2.SectorEntorno = CType(resources.GetObject("EspacioTrabajo2.SectorEntorno"), System.Collections.Generic.Dictionary(Of Integer, gsol.ISectorEntorno))
        OperacionesCatalogo1.EspacioTrabajo = EspacioTrabajo2
        OperacionesCatalogo1.IdentificadorCatalogo = "-1"
        OperacionesCatalogo1.IdentificadorEmpresa = Nothing
        OperacionesCatalogo1.IndicePaginacion = 100
        OperacionesCatalogo1.Nombre = Nothing
        OperacionesCatalogo1.NoMostrarRegistrosInsertados = False
        OperacionesCatalogo1.OperacionAnterior = Nothing
        OperacionesCatalogo1.OperadorCatalogoBorrado = Nothing
        OperacionesCatalogo1.OperadorCatalogoConsulta = Nothing
        OperacionesCatalogo1.OperadorCatalogoInsercion = Nothing
        OperacionesCatalogo1.OperadorCatalogoModificacion = Nothing
        OperacionesCatalogo1.RegistrosTemporales = CType(resources.GetObject("OperacionesCatalogo1.RegistrosTemporales"), System.Collections.Generic.Dictionary(Of Integer, String))
        OperacionesCatalogo1.SalidaLlaves = "-1"
        OperacionesCatalogo1.SerieCatalogo = 0
        'OperacionesCatalogo1.SQLTransaccion = CType(resources.GetObject("OperacionesCatalogo1.SQLTransaccion"), System.Collections.Generic.Dictionary(Of String, String))
        OperacionesCatalogo1.SQLTransaccion = New Dictionary(Of String, String)
        OperacionesCatalogo1.TablaEdicion = Nothing
        'OperacionesCatalogo1.TipoEscritura = Gsol.BaseDatos.Operaciones.IOperacionesCatalogo.TiposEscritura.Inmediata
        OperacionesCatalogo1.ValorIndice = Nothing
        OperacionesCatalogo1.VistaEncabezados = Nothing

        Me.GsCatalogo1.ArchivoEnsamblado = ""
        Me.GsCatalogo1.TipoEnsamblado = ""

        Me.GsCatalogo1.EspacioTrabajo = EspacioTrabajo1
        Me.GsCatalogo1.IdentificadorCatalogo = "-1"
        Me.GsCatalogo1.IdentificadorEmpresa = ""
        Me.GsCatalogo1.Location = New System.Drawing.Point(27, 137)
        Me.GsCatalogo1.Name = "GsCatalogo1"
        Me.GsCatalogo1.NombreCatalogo = Nothing

        Me.GsCatalogo1.OperacionesCatalogo = OperacionesCatalogo1
        Me.GsCatalogo1.OperadorCatalgosConsulta = Nothing
        Me.GsCatalogo1.OperadorCatalogosBorrado = Nothing
        Me.GsCatalogo1.OperadorCatalogosInsercion = Nothing
        Me.GsCatalogo1.OperadorCatalogosModificaciones = Nothing
        Me.GsCatalogo1.PathSaveExcel = ""
        Me.GsCatalogo1.SerieCatalogo = "0"
        Me.GsCatalogo1.Size = New System.Drawing.Size(477, 231)
        Me.GsCatalogo1.TabIndex = 359
        Me.GsCatalogo1.TablaEdicion = Nothing
        Me.GsCatalogo1.TagActualizarConsulta = Nothing
        Me.GsCatalogo1.TagAgregar = Nothing
        Me.GsCatalogo1.TagBuscar = Nothing
        Me.GsCatalogo1.TagEliminar = Nothing
        Me.GsCatalogo1.TagModificar = Nothing
        Me.GsCatalogo1.TemplateExcel = ""
        Me.GsCatalogo1.VistaEncabezados = Nothing
        '
        'FormularioBaseMaestroDetalle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(531, 493)
        Me.Controls.Add(Me.GsCatalogo1)
        Me.Name = "FormularioBaseMaestroDetalle"
        Me.Text = "Form1"
        Me.Controls.SetChildIndex(Me.LblAccion, 0)
        Me.Controls.SetChildIndex(Me.btnAceptar, 0)
        Me.Controls.SetChildIndex(Me.btnCancelar, 0)
        Me.Controls.SetChildIndex(Me.Label6, 0)
        Me.Controls.SetChildIndex(Me.LblMensaje, 0)
        Me.Controls.SetChildIndex(Me.GsCatalogo1, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents GsCatalogo1 As gsol.Componentes.SistemaBase.GsCatalogo

End Class
