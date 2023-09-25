Imports gsol.BaseDatos.Operaciones

Public Class Frm000FormularioBaseCatalogo

#Region "Constructores"

    Sub New(ByVal iespaciotrabajo As Object)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        GsCatalogo1.EspacioTrabajo = iespaciotrabajo

    End Sub

    Sub New()

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

#End Region

#Region "Propiedades"

    Public Overridable Property OperacionesCatalogo As IOperacionesCatalogo
        Get
            Return GsCatalogo1.OperacionesCatalogo
        End Get
        Set(value As IOperacionesCatalogo)

            GsCatalogo1.OperacionesCatalogo = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Overridable Sub Frm000FormularioBaseCatalogo_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        Select Case e.KeyData

            Case Keys.Escape

                Me.Close()

        End Select

    End Sub

#End Region

End Class
