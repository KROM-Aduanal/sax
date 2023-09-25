<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GsCatalogo
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GsCatalogo))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TsOpciones = New System.Windows.Forms.ToolStrip()
        Me.TsAgregar = New System.Windows.Forms.ToolStripButton()
        Me.TsModificar = New System.Windows.Forms.ToolStripButton()
        Me.TsActualizar = New System.Windows.Forms.ToolStripButton()
        Me.TsEliminar = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsTbBuscar = New System.Windows.Forms.ToolStripTextBox()
        Me.TsBuscar = New System.Windows.Forms.ToolStripButton()
        Me.TsFiltro = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbModo = New System.Windows.Forms.ToolStripButton()
        Me.tsSeparador2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsExportCSV = New System.Windows.Forms.ToolStripButton()
        Me.TsExportExcel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmsMenuContextual = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmSeleccionMultiple = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmModificar = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmEliminar = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsPedimento = New System.Windows.Forms.ToolStripMenuItem()
        Me.TsCuentaDeGastos = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.DgvCatalogo = New System.Windows.Forms.DataGridView()
        Me.ssEstado = New System.Windows.Forms.StatusStrip()
        Me.tslRegistros = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.Cargador = New System.Windows.Forms.Button()
        Me.pnlGestor = New System.Windows.Forms.Panel()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.TsOpciones.SuspendLayout
        Me.cmsMenuContextual.SuspendLayout
        Me.Panel1.SuspendLayout
        Me.TableLayoutPanel1.SuspendLayout
        CType(Me.DgvCatalogo,System.ComponentModel.ISupportInitialize).BeginInit
        Me.ssEstado.SuspendLayout
        Me.pnlGrid.SuspendLayout
        Me.SuspendLayout
        '
        'TsOpciones
        '
        Me.TsOpciones.BackColor = System.Drawing.Color.White
        Me.TsOpciones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TsAgregar, Me.TsModificar, Me.TsActualizar, Me.TsEliminar, Me.ToolStripSeparator1, Me.TsTbBuscar, Me.TsBuscar, Me.TsFiltro, Me.ToolStripSeparator6, Me.tsbModo, Me.tsSeparador2, Me.TsExportCSV, Me.TsExportExcel, Me.ToolStripSeparator5})
        Me.TsOpciones.Location = New System.Drawing.Point(0, 0)
        Me.TsOpciones.Name = "TsOpciones"
        Me.TsOpciones.Size = New System.Drawing.Size(883, 25)
        Me.TsOpciones.TabIndex = 4
        Me.TsOpciones.Text = "ToolStrip1"
        '
        'TsAgregar
        '
        Me.TsAgregar.BackColor = System.Drawing.Color.White
        Me.TsAgregar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsAgregar.Image = CType(resources.GetObject("TsAgregar.Image"),System.Drawing.Image)
        Me.TsAgregar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsAgregar.Name = "TsAgregar"
        Me.TsAgregar.Size = New System.Drawing.Size(23, 22)
        Me.TsAgregar.Text = "Agregar registro"
        '
        'TsModificar
        '
        Me.TsModificar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsModificar.Enabled = false
        Me.TsModificar.Image = CType(resources.GetObject("TsModificar.Image"),System.Drawing.Image)
        Me.TsModificar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsModificar.Name = "TsModificar"
        Me.TsModificar.Size = New System.Drawing.Size(23, 22)
        Me.TsModificar.Text = "Editar registro"
        Me.TsModificar.ToolTipText = "[Presione F2] Editar registro"
        '
        'TsActualizar
        '
        Me.TsActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsActualizar.Enabled = false
        Me.TsActualizar.Image = CType(resources.GetObject("TsActualizar.Image"),System.Drawing.Image)
        Me.TsActualizar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsActualizar.Name = "TsActualizar"
        Me.TsActualizar.Size = New System.Drawing.Size(23, 22)
        Me.TsActualizar.Text = "Actualizar consulta"
        Me.TsActualizar.ToolTipText = "[Presione F5] Actualizar consulta"
        '
        'TsEliminar
        '
        Me.TsEliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsEliminar.Enabled = false
        Me.TsEliminar.Image = CType(resources.GetObject("TsEliminar.Image"),System.Drawing.Image)
        Me.TsEliminar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsEliminar.Name = "TsEliminar"
        Me.TsEliminar.Size = New System.Drawing.Size(23, 22)
        Me.TsEliminar.Text = "Eliminar registro selecionado"
        Me.TsEliminar.ToolTipText = "[Presione DEL]  Eliminar registro selecionado"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'TsTbBuscar
        '
        Me.TsTbBuscar.BackColor = System.Drawing.Color.PowderBlue
        Me.TsTbBuscar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TsTbBuscar.Margin = New System.Windows.Forms.Padding(1)
        Me.TsTbBuscar.Name = "TsTbBuscar"
        Me.TsTbBuscar.Size = New System.Drawing.Size(198, 23)
        Me.TsTbBuscar.ToolTipText = "[Presione F3] Busqueda Rápida"
        '
        'TsBuscar
        '
        Me.TsBuscar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsBuscar.Image = CType(resources.GetObject("TsBuscar.Image"), System.Drawing.Image)
        Me.TsBuscar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsBuscar.Name = "TsBuscar"
        Me.TsBuscar.Size = New System.Drawing.Size(23, 22)
        Me.TsBuscar.Text = "Búsqueda"
        '
        'TsFiltro
        '
        Me.TsFiltro.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsFiltro.Image = CType(resources.GetObject("TsFiltro.Image"), System.Drawing.Image)
        Me.TsFiltro.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsFiltro.Name = "TsFiltro"
        Me.TsFiltro.Size = New System.Drawing.Size(23, 22)
        Me.TsFiltro.Text = "Búsqueda avanzada"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'tsbModo
        '
        Me.tsbModo.Checked = True
        Me.tsbModo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tsbModo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbModo.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsbModo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbModo.Name = "tsbModo"
        Me.tsbModo.Size = New System.Drawing.Size(23, 22)
        Me.tsbModo.Text = "Modalidad"
        '
        'tsSeparador2
        '
        Me.tsSeparador2.Name = "tsSeparador2"
        Me.tsSeparador2.Size = New System.Drawing.Size(6, 25)
        '
        'TsExportCSV
        '
        Me.TsExportCSV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsExportCSV.Enabled = False
        Me.TsExportCSV.Image = CType(resources.GetObject("TsExportCSV.Image"), System.Drawing.Image)
        Me.TsExportCSV.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsExportCSV.Name = "TsExportCSV"
        Me.TsExportCSV.Size = New System.Drawing.Size(23, 22)
        Me.TsExportCSV.ToolTipText = "Exportar en formato CSV"
        '
        'TsExportExcel
        '
        Me.TsExportExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsExportExcel.Enabled = False
        Me.TsExportExcel.Image = CType(resources.GetObject("TsExportExcel.Image"), System.Drawing.Image)
        Me.TsExportExcel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsExportExcel.Name = "TsExportExcel"
        Me.TsExportExcel.Size = New System.Drawing.Size(23, 22)
        Me.TsExportExcel.Text = "Exportar resultados a MS Excel"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'cmsMenuContextual
        '
        Me.cmsMenuContextual.BackColor = System.Drawing.Color.White
        Me.cmsMenuContextual.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmSeleccionMultiple, Me.ToolStripSeparator3, Me.tsmModificar, Me.tsmEliminar, Me.ToolStripSeparator2, Me.tsmItem1, Me.tsmItem2, Me.tsmItem3, Me.tsmItem4, Me.ToolStripSeparator4, Me.TsPedimento, Me.TsCuentaDeGastos, Me.tsmItem5, Me.tsmItem6, Me.tsmItem7, Me.tsmItem8})
        Me.cmsMenuContextual.Name = "ContextMenuStrip1"
        Me.cmsMenuContextual.Size = New System.Drawing.Size(225, 308)
        '
        'tsmSeleccionMultiple
        '
        Me.tsmSeleccionMultiple.CheckOnClick = True
        Me.tsmSeleccionMultiple.Name = "tsmSeleccionMultiple"
        Me.tsmSeleccionMultiple.Size = New System.Drawing.Size(224, 22)
        Me.tsmSeleccionMultiple.Text = "Selección múltiple"
        Me.tsmSeleccionMultiple.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(221, 6)
        '
        'tsmModificar
        '
        Me.tsmModificar.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.application_edit
        Me.tsmModificar.Name = "tsmModificar"
        Me.tsmModificar.Size = New System.Drawing.Size(224, 22)
        Me.tsmModificar.Text = "M&odificar"
        '
        'tsmEliminar
        '
        Me.tsmEliminar.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.cancel
        Me.tsmEliminar.Name = "tsmEliminar"
        Me.tsmEliminar.Size = New System.Drawing.Size(224, 22)
        Me.tsmEliminar.Text = "E&liminar"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(221, 6)
        '
        'tsmItem1
        '
        Me.tsmItem1.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem1.Name = "tsmItem1"
        Me.tsmItem1.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem1.Text = "Item1"
        Me.tsmItem1.Visible = False
        '
        'tsmItem2
        '
        Me.tsmItem2.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem2.Name = "tsmItem2"
        Me.tsmItem2.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem2.Text = "Item2"
        Me.tsmItem2.Visible = False
        '
        'tsmItem3
        '
        Me.tsmItem3.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem3.Name = "tsmItem3"
        Me.tsmItem3.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem3.Text = "Item3"
        Me.tsmItem3.Visible = False
        '
        'tsmItem4
        '
        Me.tsmItem4.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem4.Name = "tsmItem4"
        Me.tsmItem4.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem4.Text = "Item4"
        Me.tsmItem4.Visible = False
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(221, 6)
        Me.ToolStripSeparator4.Visible = False
        '
        'TsPedimento
        '
        Me.TsPedimento.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.doc_pdf
        Me.TsPedimento.Name = "TsPedimento"
        Me.TsPedimento.Size = New System.Drawing.Size(224, 22)
        Me.TsPedimento.Text = "Documentos tráfico"
        '
        'TsCuentaDeGastos
        '
        Me.TsCuentaDeGastos.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.doc_pdf
        Me.TsCuentaDeGastos.Name = "TsCuentaDeGastos"
        Me.TsCuentaDeGastos.Size = New System.Drawing.Size(224, 22)
        Me.TsCuentaDeGastos.Text = "Documentos administración"
        '
        'tsmItem5
        '
        Me.tsmItem5.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem5.Name = "tsmItem5"
        Me.tsmItem5.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem5.Text = "Item5"
        Me.tsmItem5.Visible = False
        '
        'tsmItem6
        '
        Me.tsmItem6.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem6.Name = "tsmItem6"
        Me.tsmItem6.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem6.Text = "Item6"
        Me.tsmItem6.Visible = False
        '
        'tsmItem7
        '
        Me.tsmItem7.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem7.Name = "tsmItem7"
        Me.tsmItem7.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem7.Text = "Item7"
        Me.tsmItem7.Visible = False
        '
        'tsmItem8
        '
        Me.tsmItem8.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.books
        Me.tsmItem8.Name = "tsmItem8"
        Me.tsmItem8.Size = New System.Drawing.Size(224, 22)
        Me.tsmItem8.Text = "Item8"
        Me.tsmItem8.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.DarkGray
        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(883, 127)
        Me.Panel1.TabIndex = 5
        Me.Panel1.Visible = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoScroll = True
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.947644!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.05235!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 232.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 233.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox2, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancelar, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOk, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox1, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Button3, 3, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.28358!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.71642!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(883, 127)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Label1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Label2"
        '
        'TextBox2
        '
        Me.TextBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox2.Location = New System.Drawing.Point(275, 64)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(139, 20)
        Me.TextBox2.TabIndex = 3
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(652, 64)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(63, 23)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(652, 29)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(63, 23)
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(275, 29)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(139, 20)
        Me.TextBox1.TabIndex = 2
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(420, 29)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(26, 23)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = true
        '
        'DgvCatalogo
        '
        Me.DgvCatalogo.AllowDrop = true
        Me.DgvCatalogo.AllowUserToAddRows = false
        Me.DgvCatalogo.AllowUserToDeleteRows = false
        Me.DgvCatalogo.AllowUserToOrderColumns = true
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.DgvCatalogo.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DgvCatalogo.BackgroundColor = System.Drawing.Color.LightGray
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DgvCatalogo.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DgvCatalogo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvCatalogo.ContextMenuStrip = Me.cmsMenuContextual
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkSeaGreen
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DgvCatalogo.DefaultCellStyle = DataGridViewCellStyle3
        Me.DgvCatalogo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DgvCatalogo.GridColor = System.Drawing.Color.Silver
        Me.DgvCatalogo.Location = New System.Drawing.Point(0, 0)
        Me.DgvCatalogo.MultiSelect = false
        Me.DgvCatalogo.Name = "DgvCatalogo"
        Me.DgvCatalogo.ReadOnly = true
        Me.DgvCatalogo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DgvCatalogo.Size = New System.Drawing.Size(883, 343)
        Me.DgvCatalogo.TabIndex = 6
        '
        'ssEstado
        '
        Me.ssEstado.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslRegistros})
        Me.ssEstado.Location = New System.Drawing.Point(0, 495)
        Me.ssEstado.Name = "ssEstado"
        Me.ssEstado.Size = New System.Drawing.Size(883, 22)
        Me.ssEstado.TabIndex = 8
        Me.ssEstado.Text = "StatusStrip1"
        '
        'tslRegistros
        '
        Me.tslRegistros.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tslRegistros.Name = "tslRegistros"
        Me.tslRegistros.Size = New System.Drawing.Size(113, 17)
        Me.tslRegistros.Text = "(v1.0.0.5) Registros(*)"
        '
        'pnlGrid
        '
        Me.pnlGrid.AutoScroll = true
        Me.pnlGrid.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.pnlGrid.Controls.Add(Me.DgvCatalogo)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 152)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(883, 343)
        Me.pnlGrid.TabIndex = 9
        '
        'Cargador
        '
        Me.Cargador.AllowDrop = true
        Me.Cargador.BackColor = System.Drawing.Color.White
        Me.Cargador.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Cargador.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Cargador.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.Cargador.FlatAppearance.BorderSize = 0
        Me.Cargador.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Cargador.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.Cargador.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cargador.ForeColor = System.Drawing.Color.DimGray
        Me.Cargador.Image = CType(resources.GetObject("Cargador.Image"),System.Drawing.Image)
        Me.Cargador.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Cargador.Location = New System.Drawing.Point(0, 152)
        Me.Cargador.Name = "Cargador"
        Me.Cargador.Size = New System.Drawing.Size(883, 343)
        Me.Cargador.TabIndex = 7
        Me.Cargador.Text = "Cargando..."
        Me.Cargador.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Cargador.UseVisualStyleBackColor = true
        Me.Cargador.Visible = false
        '
        'pnlGestor
        '
        Me.pnlGestor.AutoScroll = true
        Me.pnlGestor.BackColor = System.Drawing.Color.White
        Me.pnlGestor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGestor.Location = New System.Drawing.Point(0, 152)
        Me.pnlGestor.Name = "pnlGestor"
        Me.pnlGestor.Size = New System.Drawing.Size(883, 343)
        Me.pnlGestor.TabIndex = 10
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Splitter1.Location = New System.Drawing.Point(880, 152)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 343)
        Me.Splitter1.TabIndex = 13
        Me.Splitter1.TabStop = false
        '
        'GsCatalogo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.pnlGestor)
        Me.Controls.Add(Me.Cargador)
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.ssEstado)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TsOpciones)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Name = "GsCatalogo"
        Me.Size = New System.Drawing.Size(883, 517)
        Me.TsOpciones.ResumeLayout(false)
        Me.TsOpciones.PerformLayout
        Me.cmsMenuContextual.ResumeLayout(false)
        Me.Panel1.ResumeLayout(false)
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.TableLayoutPanel1.PerformLayout
        CType(Me.DgvCatalogo,System.ComponentModel.ISupportInitialize).EndInit
        Me.ssEstado.ResumeLayout(false)
        Me.ssEstado.PerformLayout
        Me.pnlGrid.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Public WithEvents TsOpciones As System.Windows.Forms.ToolStrip
    Public WithEvents TsAgregar As System.Windows.Forms.ToolStripButton
    Public WithEvents TsModificar As System.Windows.Forms.ToolStripButton
    Public WithEvents TsActualizar As System.Windows.Forms.ToolStripButton
    Public WithEvents TsEliminar As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents TsTbBuscar As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TsBuscar As System.Windows.Forms.ToolStripButton
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Public WithEvents DgvCatalogo As System.Windows.Forms.DataGridView
    Friend WithEvents TsFiltro As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsExportExcel As System.Windows.Forms.ToolStripButton
    Friend WithEvents Cargador As System.Windows.Forms.Button
    Friend WithEvents ssEstado As System.Windows.Forms.StatusStrip
    Friend WithEvents tslRegistros As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents TsExportCSV As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsmModificar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmEliminar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItem2 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents cmsMenuContextual As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmSeleccionMultiple As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TsPedimento As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TsCuentaDeGastos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents pnlGestor As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbModo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsSeparador2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItem7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItem8 As System.Windows.Forms.ToolStripMenuItem

End Class
