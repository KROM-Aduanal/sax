Imports Gsol.BaseDatos.Operaciones
Imports Gsol


Public Class Frm000Generic

#Region "Constructores"

    Sub New(ByVal iespaciotrabajo As Object)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        GsCatalogo1.EspacioTrabajo = iespaciotrabajo

    End Sub

    Sub New(ByVal iespaciotrabajo As Object, _
            ByVal modulo_ As String)

        InitializeComponent()

        Dim sistema_ As New Organismo

        'GsCatalogo1.OperacionesCatalogo.EspacioTrabajo = iespaciotrabajo
        GsCatalogo1.EspacioTrabajo = iespaciotrabajo

        sistema_.EnsamblaModulo(modulo_, GsCatalogo1)

        'GsCatalogo1.EspacioTrabajo = iespaciotrabajo

        Me.Text = "{Auto} Módulo " & modulo_

    End Sub

#End Region

#Region "Propiedades"

    Public Property OperacionesCatalogo As IOperacionesCatalogo
        Get
            Return GsCatalogo1.OperacionesCatalogo
        End Get
        Set(value As IOperacionesCatalogo)

            GsCatalogo1.OperacionesCatalogo = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Private Sub Frm000CatPermisos_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyData
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub Frm000CatPermisos_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        GsCatalogo1.TsTbBuscar.Focus()

    End Sub

#End Region



End Class
