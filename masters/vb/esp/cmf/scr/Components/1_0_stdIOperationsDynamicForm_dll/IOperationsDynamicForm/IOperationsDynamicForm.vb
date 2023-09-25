Imports Gsol.BaseDatos.Operaciones
Imports System.Windows.Forms
Imports System.Text

Namespace Wma.Components


    Public Interface IOperationsDynamicForm

#Region "Enums"

        Enum FormatosPregunta
            Undefined = 0
            PreguntaTextBoxMask = 1
            PreguntaTexBoxLong = 2
            PreguntaDateTimePickerLongDate = 3
            PreguntaDateTimePickerShortTime = 4
            PreguntaRadioButton = 5
            PreguntaComboBox = 6
            PreguntaCheckBox = 7
            PreguntaWclTextBoxFinder = 8
        End Enum

        Enum ModeDynamicControls
            Undefined = 0
            AsSimpleInterface = 1
            AsComparisons = 2
            AsQuestions = 3
        End Enum

        Enum ModalidadesPresentacionCuestionario
            Undefined = 0
            PresentaUnaPreguntaActiva = 1
            PresentaTodoActivo = 2
        End Enum

#End Region

#Region "Attributes"

#End Region

#Region "Builders"


#End Region

#Region "Events"

        Event AfterCloseDynamicForm()

        Event OnClickAccept()

        Event OnClickCancel()

        Event OnPaint()

        Event AtEndOfPoll()

#End Region

#Region "Properties"

        Property MostrarBotonesDeAccion As Boolean

        Property MostrarBarraTituloGeneral As Boolean


        Property CollectionOfQuestions As List(Of ObjetoPregunta)

        ReadOnly Property CollectionPairsControls As IDictionary(Of Int32, OperationdPairControls)

        Property IOperations As IOperacionesCatalogo

        Property ControlPairsDisplayed As Int32

        Property LayoutColumns As Int32

        Property DynamicForm As Form

        Property ModeForm As ModeDynamicControls

        ReadOnly Property GetQueryRules As StringBuilder

        'Additional
        Property LaunchQuery As Boolean

        Property ModalidadPresentacionCuestionario As ModalidadesPresentacionCuestionario

#End Region

#Region "Methods"

        Sub ShowMyDynamicForm()

        Sub EnviarFormulario()

        Sub SalirSinEnviarFormulario()

#End Region


    End Interface


End Namespace