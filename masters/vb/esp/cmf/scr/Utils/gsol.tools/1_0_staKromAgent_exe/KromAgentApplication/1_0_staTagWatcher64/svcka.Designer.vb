<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class svchostka
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(svchostka))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnDesbloquear = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.tbNumeroIP = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbNombreRed = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbDescripcionRed = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tbClaveMAC = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbNombreEquipo = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbDominio = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tbUsuarioEquipo = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tbTipoConexion = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.tbIdiomaSO = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.tbPlataformaSO = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.tbVersionSO = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.tbNombreSO = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblNumeroIP = New System.Windows.Forms.PictureBox()
        Me.lblClaveMAC = New System.Windows.Forms.PictureBox()
        Me.lblUsuarioEquipo = New System.Windows.Forms.PictureBox()
        Me.lblDominio = New System.Windows.Forms.PictureBox()
        Me.lblNombreEquipo = New System.Windows.Forms.PictureBox()
        Me.lblSistemaOperativo = New System.Windows.Forms.PictureBox()
        Me.lblVersion = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblMensaje = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.tbPassword = New System.Windows.Forms.TextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblNumeroIP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblClaveMAC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblUsuarioEquipo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblDominio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblNombreEquipo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSistemaOperativo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblVersion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(604, 142)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(318, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Este equipo no se encuentra registrado en el corporativo"
        '
        'btnDesbloquear
        '
        Me.btnDesbloquear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDesbloquear.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDesbloquear.Location = New System.Drawing.Point(651, 636)
        Me.btnDesbloquear.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.btnDesbloquear.Name = "btnDesbloquear"
        Me.btnDesbloquear.Size = New System.Drawing.Size(131, 45)
        Me.btnDesbloquear.TabIndex = 1
        Me.btnDesbloquear.Text = "Desbloquear temporalmente"
        Me.btnDesbloquear.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(605, 160)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(238, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Será bloqueado hasta concluir su registro"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.White
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(605, 126)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(169, 19)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "IMPORTANTE AVISO."
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(609, 23)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(315, 96)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'tbNumeroIP
        '
        Me.tbNumeroIP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbNumeroIP.BackColor = System.Drawing.Color.Honeydew
        Me.tbNumeroIP.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNumeroIP.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbNumeroIP.Location = New System.Drawing.Point(693, 242)
        Me.tbNumeroIP.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbNumeroIP.Name = "tbNumeroIP"
        Me.tbNumeroIP.ReadOnly = True
        Me.tbNumeroIP.Size = New System.Drawing.Size(209, 22)
        Me.tbNumeroIP.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(611, 241)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 20)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Número IP:"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(611, 216)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(106, 19)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Datos de red"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(611, 267)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 20)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Nombre:"
        '
        'tbNombreRed
        '
        Me.tbNombreRed.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbNombreRed.BackColor = System.Drawing.Color.Honeydew
        Me.tbNombreRed.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNombreRed.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbNombreRed.Location = New System.Drawing.Point(693, 268)
        Me.tbNombreRed.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbNombreRed.Name = "tbNombreRed"
        Me.tbNombreRed.ReadOnly = True
        Me.tbNombreRed.Size = New System.Drawing.Size(209, 22)
        Me.tbNombreRed.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(611, 293)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 20)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Descripción:"
        '
        'tbDescripcionRed
        '
        Me.tbDescripcionRed.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDescripcionRed.BackColor = System.Drawing.Color.Honeydew
        Me.tbDescripcionRed.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbDescripcionRed.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbDescripcionRed.Location = New System.Drawing.Point(693, 294)
        Me.tbDescripcionRed.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbDescripcionRed.Name = "tbDescripcionRed"
        Me.tbDescripcionRed.ReadOnly = True
        Me.tbDescripcionRed.Size = New System.Drawing.Size(209, 22)
        Me.tbDescripcionRed.TabIndex = 11
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(611, 319)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 20)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Clave MAC:"
        '
        'tbClaveMAC
        '
        Me.tbClaveMAC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbClaveMAC.BackColor = System.Drawing.Color.Honeydew
        Me.tbClaveMAC.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbClaveMAC.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbClaveMAC.Location = New System.Drawing.Point(693, 320)
        Me.tbClaveMAC.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbClaveMAC.Name = "tbClaveMAC"
        Me.tbClaveMAC.ReadOnly = True
        Me.tbClaveMAC.Size = New System.Drawing.Size(209, 22)
        Me.tbClaveMAC.TabIndex = 13
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(611, 450)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(98, 20)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Nombre equipo:"
        '
        'tbNombreEquipo
        '
        Me.tbNombreEquipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbNombreEquipo.BackColor = System.Drawing.Color.Honeydew
        Me.tbNombreEquipo.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNombreEquipo.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbNombreEquipo.Location = New System.Drawing.Point(709, 451)
        Me.tbNombreEquipo.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbNombreEquipo.Name = "tbNombreEquipo"
        Me.tbNombreEquipo.ReadOnly = True
        Me.tbNombreEquipo.Size = New System.Drawing.Size(193, 22)
        Me.tbNombreEquipo.TabIndex = 21
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(611, 424)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(59, 20)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Dominio:"
        '
        'tbDominio
        '
        Me.tbDominio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDominio.BackColor = System.Drawing.Color.Honeydew
        Me.tbDominio.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbDominio.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbDominio.Location = New System.Drawing.Point(709, 425)
        Me.tbDominio.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbDominio.Name = "tbDominio"
        Me.tbDominio.ReadOnly = True
        Me.tbDominio.Size = New System.Drawing.Size(193, 22)
        Me.tbDominio.TabIndex = 19
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(611, 398)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 20)
        Me.Label11.TabIndex = 18
        Me.Label11.Text = "Usuario equipo:"
        '
        'tbUsuarioEquipo
        '
        Me.tbUsuarioEquipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbUsuarioEquipo.BackColor = System.Drawing.Color.Honeydew
        Me.tbUsuarioEquipo.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbUsuarioEquipo.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbUsuarioEquipo.Location = New System.Drawing.Point(709, 399)
        Me.tbUsuarioEquipo.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbUsuarioEquipo.Name = "tbUsuarioEquipo"
        Me.tbUsuarioEquipo.ReadOnly = True
        Me.tbUsuarioEquipo.Size = New System.Drawing.Size(193, 22)
        Me.tbUsuarioEquipo.TabIndex = 17
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(611, 345)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(94, 20)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Tipo Conexión:"
        '
        'tbTipoConexion
        '
        Me.tbTipoConexion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbTipoConexion.BackColor = System.Drawing.Color.Honeydew
        Me.tbTipoConexion.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbTipoConexion.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbTipoConexion.Location = New System.Drawing.Point(709, 346)
        Me.tbTipoConexion.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbTipoConexion.Name = "tbTipoConexion"
        Me.tbTipoConexion.ReadOnly = True
        Me.tbTipoConexion.Size = New System.Drawing.Size(193, 22)
        Me.tbTipoConexion.TabIndex = 15
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(611, 590)
        Me.Label14.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(50, 20)
        Me.Label14.TabIndex = 31
        Me.Label14.Text = "Idioma:"
        '
        'tbIdiomaSO
        '
        Me.tbIdiomaSO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbIdiomaSO.BackColor = System.Drawing.Color.Honeydew
        Me.tbIdiomaSO.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbIdiomaSO.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbIdiomaSO.Location = New System.Drawing.Point(693, 591)
        Me.tbIdiomaSO.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbIdiomaSO.Name = "tbIdiomaSO"
        Me.tbIdiomaSO.ReadOnly = True
        Me.tbIdiomaSO.Size = New System.Drawing.Size(209, 22)
        Me.tbIdiomaSO.TabIndex = 30
        '
        'Label15
        '
        Me.Label15.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(611, 564)
        Me.Label15.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(72, 20)
        Me.Label15.TabIndex = 29
        Me.Label15.Text = "Plataforma:"
        '
        'tbPlataformaSO
        '
        Me.tbPlataformaSO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPlataformaSO.BackColor = System.Drawing.Color.Honeydew
        Me.tbPlataformaSO.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbPlataformaSO.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbPlataformaSO.Location = New System.Drawing.Point(693, 565)
        Me.tbPlataformaSO.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbPlataformaSO.Name = "tbPlataformaSO"
        Me.tbPlataformaSO.ReadOnly = True
        Me.tbPlataformaSO.Size = New System.Drawing.Size(209, 22)
        Me.tbPlataformaSO.TabIndex = 28
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(611, 538)
        Me.Label16.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(53, 20)
        Me.Label16.TabIndex = 27
        Me.Label16.Text = "Versión:"
        '
        'tbVersionSO
        '
        Me.tbVersionSO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbVersionSO.BackColor = System.Drawing.Color.Honeydew
        Me.tbVersionSO.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbVersionSO.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbVersionSO.Location = New System.Drawing.Point(693, 539)
        Me.tbVersionSO.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbVersionSO.Name = "tbVersionSO"
        Me.tbVersionSO.ReadOnly = True
        Me.tbVersionSO.Size = New System.Drawing.Size(209, 22)
        Me.tbVersionSO.TabIndex = 26
        '
        'Label17
        '
        Me.Label17.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.White
        Me.Label17.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(611, 487)
        Me.Label17.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(144, 19)
        Me.Label17.TabIndex = 25
        Me.Label17.Text = "Datos del sistema"
        '
        'Label18
        '
        Me.Label18.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(611, 512)
        Me.Label18.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(57, 20)
        Me.Label18.TabIndex = 24
        Me.Label18.Text = "Nombre:"
        '
        'tbNombreSO
        '
        Me.tbNombreSO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbNombreSO.BackColor = System.Drawing.Color.Honeydew
        Me.tbNombreSO.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNombreSO.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbNombreSO.Location = New System.Drawing.Point(693, 513)
        Me.tbNombreSO.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.tbNombreSO.Name = "tbNombreSO"
        Me.tbNombreSO.ReadOnly = True
        Me.tbNombreSO.Size = New System.Drawing.Size(209, 22)
        Me.tbNombreSO.TabIndex = 23
        '
        'Label19
        '
        Me.Label19.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.White
        Me.Label19.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(611, 377)
        Me.Label19.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(149, 19)
        Me.Label19.TabIndex = 34
        Me.Label19.Text = "Datos del usuario:"
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.White
        Me.Label13.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(606, 196)
        Me.Label13.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(296, 18)
        Me.Label13.TabIndex = 35
        Me.Label13.Text = "________________________________"
        '
        'lblNumeroIP
        '
        Me.lblNumeroIP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNumeroIP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblNumeroIP.Image = CType(resources.GetObject("lblNumeroIP.Image"), System.Drawing.Image)
        Me.lblNumeroIP.Location = New System.Drawing.Point(591, 243)
        Me.lblNumeroIP.Name = "lblNumeroIP"
        Me.lblNumeroIP.Size = New System.Drawing.Size(18, 16)
        Me.lblNumeroIP.TabIndex = 36
        Me.lblNumeroIP.TabStop = False
        '
        'lblClaveMAC
        '
        Me.lblClaveMAC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblClaveMAC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblClaveMAC.Image = CType(resources.GetObject("lblClaveMAC.Image"), System.Drawing.Image)
        Me.lblClaveMAC.Location = New System.Drawing.Point(591, 322)
        Me.lblClaveMAC.Name = "lblClaveMAC"
        Me.lblClaveMAC.Size = New System.Drawing.Size(18, 16)
        Me.lblClaveMAC.TabIndex = 37
        Me.lblClaveMAC.TabStop = False
        '
        'lblUsuarioEquipo
        '
        Me.lblUsuarioEquipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUsuarioEquipo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblUsuarioEquipo.Image = CType(resources.GetObject("lblUsuarioEquipo.Image"), System.Drawing.Image)
        Me.lblUsuarioEquipo.Location = New System.Drawing.Point(591, 401)
        Me.lblUsuarioEquipo.Name = "lblUsuarioEquipo"
        Me.lblUsuarioEquipo.Size = New System.Drawing.Size(18, 16)
        Me.lblUsuarioEquipo.TabIndex = 38
        Me.lblUsuarioEquipo.TabStop = False
        '
        'lblDominio
        '
        Me.lblDominio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDominio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblDominio.Image = CType(resources.GetObject("lblDominio.Image"), System.Drawing.Image)
        Me.lblDominio.Location = New System.Drawing.Point(591, 427)
        Me.lblDominio.Name = "lblDominio"
        Me.lblDominio.Size = New System.Drawing.Size(18, 16)
        Me.lblDominio.TabIndex = 39
        Me.lblDominio.TabStop = False
        '
        'lblNombreEquipo
        '
        Me.lblNombreEquipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNombreEquipo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblNombreEquipo.Image = CType(resources.GetObject("lblNombreEquipo.Image"), System.Drawing.Image)
        Me.lblNombreEquipo.Location = New System.Drawing.Point(591, 453)
        Me.lblNombreEquipo.Name = "lblNombreEquipo"
        Me.lblNombreEquipo.Size = New System.Drawing.Size(18, 16)
        Me.lblNombreEquipo.TabIndex = 40
        Me.lblNombreEquipo.TabStop = False
        '
        'lblSistemaOperativo
        '
        Me.lblSistemaOperativo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSistemaOperativo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblSistemaOperativo.Image = CType(resources.GetObject("lblSistemaOperativo.Image"), System.Drawing.Image)
        Me.lblSistemaOperativo.Location = New System.Drawing.Point(591, 514)
        Me.lblSistemaOperativo.Name = "lblSistemaOperativo"
        Me.lblSistemaOperativo.Size = New System.Drawing.Size(18, 16)
        Me.lblSistemaOperativo.TabIndex = 41
        Me.lblSistemaOperativo.TabStop = False
        '
        'lblVersion
        '
        Me.lblVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblVersion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.lblVersion.Image = CType(resources.GetObject("lblVersion.Image"), System.Drawing.Image)
        Me.lblVersion.Location = New System.Drawing.Point(591, 541)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(18, 16)
        Me.lblVersion.TabIndex = 42
        Me.lblVersion.TabStop = False
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.Color.SeaGreen
        Me.Button1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(786, 635)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(116, 46)
        Me.Button1.TabIndex = 43
        Me.Button1.Text = "Verificar nuevamente"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'lblMensaje
        '
        Me.lblMensaje.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMensaje.AutoSize = True
        Me.lblMensaje.BackColor = System.Drawing.Color.Transparent
        Me.lblMensaje.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMensaje.ForeColor = System.Drawing.Color.Maroon
        Me.lblMensaje.Location = New System.Drawing.Point(605, 181)
        Me.lblMensaje.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblMensaje.Name = "lblMensaje"
        Me.lblMensaje.Size = New System.Drawing.Size(12, 16)
        Me.lblMensaje.TabIndex = 44
        Me.lblMensaje.Text = "*"
        '
        'Label20
        '
        Me.Label20.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(857, 684)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(45, 15)
        Me.Label20.TabIndex = 45
        Me.Label20.Text = "V 1.0.0.2"
        '
        'lblMsg
        '
        Me.lblMsg.AutoSize = True
        Me.lblMsg.Location = New System.Drawing.Point(12, 9)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(28, 15)
        Me.lblMsg.TabIndex = 46
        Me.lblMsg.Text = "msg:"
        '
        'tbPassword
        '
        Me.tbPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPassword.BackColor = System.Drawing.Color.White
        Me.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbPassword.Location = New System.Drawing.Point(567, 638)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.Size = New System.Drawing.Size(79, 13)
        Me.tbPassword.TabIndex = 47
        '
        'svchostka
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(5.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(944, 706)
        Me.ControlBox = False
        Me.Controls.Add(Me.tbPassword)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.lblMensaje)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblSistemaOperativo)
        Me.Controls.Add(Me.lblNombreEquipo)
        Me.Controls.Add(Me.lblDominio)
        Me.Controls.Add(Me.lblUsuarioEquipo)
        Me.Controls.Add(Me.lblClaveMAC)
        Me.Controls.Add(Me.lblNumeroIP)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.tbIdiomaSO)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.tbPlataformaSO)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.tbVersionSO)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.tbNombreSO)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.tbNombreEquipo)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.tbDominio)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.tbUsuarioEquipo)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.tbTipoConexion)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.tbClaveMAC)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbDescripcionRed)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbNombreRed)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbNumeroIP)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnDesbloquear)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.AppStarting
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Arial Narrow", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.GrayText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "svchostka"
        Me.Opacity = 0.95R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Krom Authentication"
        Me.TopMost = True
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblNumeroIP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblClaveMAC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblUsuarioEquipo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblDominio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblNombreEquipo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSistemaOperativo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblVersion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnDesbloquear As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents tbNumeroIP As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbNombreRed As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbDescripcionRed As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbClaveMAC As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbNombreEquipo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbDominio As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbUsuarioEquipo As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tbTipoConexion As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents tbIdiomaSO As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents tbPlataformaSO As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents tbVersionSO As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tbNombreSO As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblNumeroIP As System.Windows.Forms.PictureBox
    Friend WithEvents lblClaveMAC As System.Windows.Forms.PictureBox
    Friend WithEvents lblUsuarioEquipo As System.Windows.Forms.PictureBox
    Friend WithEvents lblDominio As System.Windows.Forms.PictureBox
    Friend WithEvents lblNombreEquipo As System.Windows.Forms.PictureBox
    Friend WithEvents lblSistemaOperativo As System.Windows.Forms.PictureBox
    Friend WithEvents lblVersion As System.Windows.Forms.PictureBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblMensaje As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox

End Class
