Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls

Public Class UIInputControl
    Inherits UIControl
    Implements IUIInputControl, IPostBackEventHandler

#Region "Propiedades"

    Public Overridable Property Value As String Implements IUIInputControl.Value

    Public Property Rules As String Implements IUIInputControl.Rules

    Public Property Locked As Boolean Implements IUIInputControl.Locked

        Get

            Return ViewState("Locked")

        End Get

        Set(value As Boolean)

            ViewState("Locked") = value

        End Set

    End Property

    Public Overridable Property Signature As String Implements IUIInputControl.Signature

        Get

            Return ViewState("Signature")

        End Get

        Set(value As String)

            ViewState("Signature") = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Sub RaisePostBackEvent(eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent

        'proccess to validate perms

        Locked = False

        Enabled = True

    End Sub

    Public Function SetLockedControl() As Object Implements IUIInputControl.SetLockedControl

        Enabled = False

        Dim lockedButton_ = New LinkButton

        With lockedButton_

            .ID = "_locked"

            .Attributes.Add("class", "lock-input")

            .Attributes.Add("pbref", Page.ClientScript.GetPostBackEventReference(Me, "__argument"))

        End With

        Return lockedButton_

    End Function

    Protected Sub SetInputRules(_validationsElements As HtmlGenericControl) Implements IUIInputControl.SetInputRules

        If Rules IsNot Nothing Then

            Dim rules_ = Rules.Split("|")

            For Each dataRule_ As String In rules_

                Dim rule_ As String = Nothing

                Dim value_ As String = Nothing

                If dataRule_.Contains("[") Then

                    Dim dataRuleAr = dataRule_.Split("[")

                    rule_ = dataRuleAr(0)

                    value_ = dataRuleAr(1).Replace("]", "")

                Else

                    rule_ = dataRule_

                End If


                Select Case rule_

                    Case = "required"

                        Dim validation_ = New RequiredFieldValidator

                        With validation_

                            .ErrorMessage = "Este campo es requerido."

                            .ControlToValidate = ID

                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "onlynumber"

                        Dim validation_ = New RegularExpressionValidator

                        With validation_

                            .ErrorMessage = "Este campo solo puede contener números."

                            .ControlToValidate = ID

                            .ValidationExpression = "^[0-9.]*$"

                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "onlytext"

                        Dim validation_ = New RegularExpressionValidator

                        With validation_

                            .ErrorMessage = "Este campo solo puede contener letras."

                            .ControlToValidate = ID

                            .ValidationExpression = "^[a-zA-ZáéíóúÁÉÍÓÚÑñ ]*$"

                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "alphanumeric"

                        Dim validation_ = New RegularExpressionValidator

                        With validation_

                            .ErrorMessage = "Este campo solo puede contener letras y números."

                            .ControlToValidate = ID

                            .ValidationExpression = "^[0-9a-zA-ZáéíóúÁÉÍÓÚÑñ ]*$"

                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "valid_email"

                        Dim validation_ = New RegularExpressionValidator

                        With validation_

                            .ErrorMessage = "El correo electrónico no es válido."

                            .ControlToValidate = ID

                            .ValidationExpression = "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"


                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "maxlength"

                        Dim validation_ = New RegularExpressionValidator

                        With validation_

                            .ControlToValidate = ID

                            .ErrorMessage = "El campo debe contener máximo [" & value_ & "] caracteres."

                            .ValidationExpression = "^[\s\S]{0," & value_ & "}$"

                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "minlength"

                        Dim validation_ = New RegularExpressionValidator

                        With validation_

                            .ControlToValidate = ID

                            .ErrorMessage = "El campo debe contener mínimo [" & value_ & "] caracteres."

                            .ValidationExpression = "^[\s\S]{" & value_ & ",}$"

                        End With

                        _validationsElements.Controls.Add(validation_)

                    Case = "match"

                        Dim validation_ = New CompareValidator

                        With validation_

                            .ControlToValidate = value_

                            .Operator = ValidationCompareOperator.Equal

                            .ControlToCompare = ID

                        End With

                        _validationsElements.Controls.Add(validation_)

                End Select

            Next

        End If

    End Sub

#End Region

End Class

