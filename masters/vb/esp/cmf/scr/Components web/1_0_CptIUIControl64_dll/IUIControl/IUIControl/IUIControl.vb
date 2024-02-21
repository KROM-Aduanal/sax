Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

Public Interface IUIControl

#Region "Enum"

    Enum Languages
        Undefined = 0
        Spanish = 1
        English = 2
    End Enum

    Enum ToolTipModalities
        Classic = 1
        Ondemand = 2
        Interactive = 3
    End Enum

    Enum ToolTipTypeStatus
        Ok = 1
        OkInfo = 2
        OkBut = 3
        Errors = 4
    End Enum

#End Region

#Region "Propiedades"

    Property Language As Languages
    Property Label As String
    Property IdPermiso As Integer
    Property WorksWith As [Enum]

    Property ToolTipExpireTime As Integer

    Property ToolTipStatus As ToolTipTypeStatus

    Property ToolTipModality As ToolTipModalities

    Property ToolTipIsVisible As Boolean

    Property Signature As String

#End Region

End Interface
