Imports Wma.Components
Imports Gsol.BaseDatos.Operaciones
Imports Wma.Operations

Public Class WclPoll

#Region "Enums"

#End Region

#Region "Atributos"

    Private _formatoPregunta As Wma.Components.IOperationsDynamicForm.FormatosPregunta

    Private WithEvents _objetoPregunta As ObjetoPregunta

    Private _mascara As String

#End Region

#Region "Eventos"

    Public Event AlResponderCorrectamentePregunta(ByRef objetoPregunta_ As ObjetoPregunta)

#End Region

#Region "Constructores"

    Sub New(ByVal icaracteristica_ As ICaracteristica)

        InitializeComponent()

        _objetoPregunta = New ObjetoPregunta(icaracteristica_,
                                             pnlEncabezado,
                                             pnlCuerpo,
                                             pnlInformacion,
                                             lblComentarios)

        lblTitulo.Text = _objetoPregunta.Titulo

        lblComentarios.Text = Nothing

        _mascara = _objetoPregunta.Mascara

        'Obligatoria
        If LCase(Trim(_objetoPregunta.Obligatoria)) = "sí" Or LCase(Trim(_objetoPregunta.Obligatoria)) = "si" Then : lblObligatoria.Text = "*" : Else : lblObligatoria.Text = Nothing : End If

    End Sub

    Sub New()

        InitializeComponent()

    End Sub

#End Region

#Region "Propiedades"

    Public Property ConfiguracionPregunta As ObjetoPregunta

        Get

            Return _objetoPregunta

        End Get

        Set(value As ObjetoPregunta)

            _objetoPregunta = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Sub AlResponderCorrectamentePoll() Handles _objetoPregunta.AlResponderCorrectamente

        RaiseEvent AlResponderCorrectamentePregunta(_objetoPregunta)

    End Sub

#End Region

End Class
