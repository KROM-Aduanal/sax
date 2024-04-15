﻿Imports System.ComponentModel
Imports System.Drawing
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
    DefaultEvent("CheckedChanged"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class GroupControl
    Inherits UIInputControl

#Region "Enums"

    Enum TypeControl
        Checkbox = 1
        Radio = 2
    End Enum

    Enum ColumnNumber
        X1 = 1
        X2 = 2
        X3 = 3
        X4 = 4
        X6 = 5
    End Enum

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventCheckedChange As New Object()

    Property Columns As ColumnNumber = ColumnNumber.X1

    Property Type As TypeControl = TypeControl.Checkbox

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property ListItems As List(Of Item) = New List(Of Item)

    Public Property Bordered As Boolean = True

    Public Property SelectedIndex As Integer

    Public Property IsCheckedSelectedIndex As Boolean

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Public Custom Event CheckedChanged As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventCheckedChange, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventCheckedChange, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventCheckedChange), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnCheckedChanged(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventCheckedChange), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

    Private Sub GroupItemSelected(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        SelectedIndex = Integer.Parse(source.InputAttributes.Item("value"))

        IsCheckedSelectedIndex = source.Checked

        OnCheckedChanged(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"
#End Region

#Region "Renderizado"

    Protected Overrides Sub CreateChildControls()

        Dim container_ = New HtmlGenericControl("div")

        container_.Attributes.Add("class", CssClass)

        Dim group_ = New HtmlGenericControl("div")

        With group_

            Dim bordered_ = If(_Bordered, " group-boders", "")

            .Attributes.Add("class", "wc-group position-relative" & bordered_)

            If Not Label Is Nothing Then

                .Attributes.Add("group-title", Label)

            End If

            Dim columnWidth_ As String = "100"

            If _Columns Then

                If _Columns = ColumnNumber.X1 Then
                    columnWidth_ = "100"
                End If

                If _Columns = ColumnNumber.X2 Then
                    columnWidth_ = "50"
                End If

                If _Columns = ColumnNumber.X3 Then
                    columnWidth_ = "33.333333"
                End If

                If _Columns = ColumnNumber.X4 Then
                    columnWidth_ = "25"
                End If

                If _Columns = ColumnNumber.X6 Then
                    columnWidth_ = "16.666666"
                End If

            End If

            .Attributes.Add("style", "--items-width: " & columnWidth_ & "%")

        End With


        If _ListItems.Count Then

            Dim index_ = 0

            For Each control_ As Item In _ListItems

                If _Type = TypeControl.Checkbox Then

                    Dim componente_ As New HtmlGenericControl("label")

                    With componente_

                        .Attributes.Add("class", "wc-checkbox d-flex position-relative mb-3")

                        If Not ForeColor = Color.Empty Then

                            .Attributes.Add("style", "--tintColor: '" & ForeColor.ToHex & "'")

                        End If

                        Dim userControl_ As New CheckBox

                        With userControl_

                            .CausesValidation = False

                            .AutoPostBack = True

                            .InputAttributes.Add("value", index_)

                            .InputAttributes.Add("class", "d-none")

                            AddHandler .CheckedChanged, AddressOf GroupItemSelected

                        End With

                        .Controls.Add(userControl_)

                        .Controls.Add(New LiteralControl("    <span text='" & control_.Text & "' class='d-flex align-items-center'></span>"))

                    End With

                    group_.Controls.Add(componente_)

                Else

                    Dim componente_ As New HtmlGenericControl("label")

                    With componente_

                        .Attributes.Add("class", "wc-radio d-flex position-relative mb-3")

                        If Not ForeColor = Color.Empty Then

                            .Attributes.Add("style", "--tintColor: '" & ForeColor.ToHex & "'")

                        End If

                        Dim userControl_ As New RadioButton

                        With userControl_

                            .CausesValidation = False

                            .AutoPostBack = True

                            .InputAttributes.Add("value", index_)

                            .InputAttributes.Add("class", "d-none")

                            AddHandler .CheckedChanged, AddressOf GroupItemSelected

                        End With

                        .Controls.Add(userControl_)

                        .Controls.Add(New LiteralControl("    <span text='" & control_.Text & "' class='d-flex align-items-center'></span>"))

                    End With

                    group_.Controls.Add(componente_)

                End If

                index_ += 1

            Next

        End If

        container_.Controls.Add(group_)

        Me.Controls.Add(container_)

    End Sub


    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class

Public Class Item

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Text As String

End Class