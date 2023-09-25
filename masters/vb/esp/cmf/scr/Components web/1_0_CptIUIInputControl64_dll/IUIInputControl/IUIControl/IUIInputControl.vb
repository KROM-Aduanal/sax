Imports System.Web.UI.HtmlControls

Public Interface IUIInputControl

#Region "Enum"
#End Region

#Region "Propiedades"

    Property Value As String

    Property Rules As String

    Property Locked As Boolean

#End Region

#Region "Metodos"

    Function SetLockedControl() As Object

    Sub SetInputRules(_validationsElements As HtmlGenericControl)

#End Region

End Interface

