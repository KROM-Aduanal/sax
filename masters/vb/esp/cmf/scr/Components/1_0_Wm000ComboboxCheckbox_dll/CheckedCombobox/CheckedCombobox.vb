Imports System.ComponentModel
Imports System.Collections.ObjectModel

Public Class CheckedCombobox
    Inherits ComboBox

    Public Event ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

    Dim n As nWindow = Nothing

    <Browsable(False)> _
    Public Overloads ReadOnly Property Items() As ComboBox.ObjectCollection
        Get
            Return MyBase.Items
        End Get
    End Property

    Private _items As New ObservableCollection(Of String)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public ReadOnly Property CheckedComboboxItems() As ObservableCollection(Of String)
        Get
            Return _items
        End Get
    End Property

    Private _isCheckedItem As New List(Of Boolean)
    Private ReadOnly Property isCheckedItem() As List(Of Boolean)
        Get
            Return _isCheckedItem
        End Get
    End Property

    Private _checkedItems As New List(Of String)
    Public ReadOnly Property checkedItems() As List(Of String)
        Get
            Return _checkedItems
        End Get
    End Property

    Private _checkedIndices As New List(Of Integer)
    Public ReadOnly Property checkedIndices() As List(Of Integer)
        Get
            Return _checkedIndices
        End Get
    End Property

    Public Sub New()
        Me.DrawMode = Windows.Forms.DrawMode.OwnerDrawVariable
        AddHandler CheckedComboboxItems.CollectionChanged, AddressOf itemsChanged
    End Sub

    Private Sub itemsChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
        If e.Action = Specialized.NotifyCollectionChangedAction.Add Then
            If e.NewStartingIndex = isCheckedItem.Count Then
                isCheckedItem.Add(False)
                MyBase.Items.Add(e.NewItems(0))
            Else
                isCheckedItem.Insert(e.NewStartingIndex, False)
                MyBase.Items.Insert(e.NewStartingIndex, e.NewItems(0))
            End If
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Remove Then
            isCheckedItem.RemoveAt(e.OldStartingIndex)
            MyBase.Items.RemoveAt(e.OldStartingIndex)
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Move Then
            Dim tempBoolean As Boolean = isCheckedItem(e.OldStartingIndex)
            Dim tempList As New List(Of Boolean)(isCheckedItem)
            Dim tempItem As Object = MyBase.Items(e.OldStartingIndex)
            tempList.RemoveAt(e.OldStartingIndex)
            MyBase.Items.RemoveAt(e.OldStartingIndex)
            tempList.Insert(e.NewStartingIndex, tempBoolean)
            MyBase.Items.Insert(e.NewStartingIndex, tempItem)
            _isCheckedItem = tempList
        End If
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Dim items() As String = MyBase.Items.Cast(Of String).ToArray
        If Not items.SequenceEqual(Me.CheckedComboboxItems) Then
            MyBase.Items.Clear()
            MyBase.Items.AddRange(Me.CheckedComboboxItems.ToArray)
        End If
        isCheckedItem.AddRange(Enumerable.Repeat(False, MyBase.Items.Count))
    End Sub

    'Protected Overrides Sub OnCreateControl()
    '    MyBase.OnCreateControl()
    '    isCheckedItem.AddRange(Enumerable.Repeat(False, MyBase.Items.Count))
    'End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
        e.DrawBackground()
        Dim p As Point = e.Bounds.Location
        p.Offset(1, 1)
        CheckBoxRenderer.DrawCheckBox(e.Graphics, p, If(isCheckedItem(e.Index), VisualStyles.CheckBoxState.CheckedNormal, VisualStyles.CheckBoxState.UncheckedNormal))
        p.Offset(12, 0)
        e.Graphics.DrawString(MyBase.GetItemText(Me.Items(e.Index)), e.Font, New SolidBrush(e.ForeColor), p.X, p.Y)
        If e.State = DrawItemState.Selected Then
            e.DrawFocusRectangle()
        End If
        MyBase.OnDrawItem(e)
    End Sub

    Private Const WM_CTLCOLORLISTBOX As Integer = &H134

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If m.Msg = WM_CTLCOLORLISTBOX Then
            If n Is Nothing Then
                n = New nWindow(Me)
                n.AssignHandle(m.LParam)
                AddHandler n.checkedChanged, AddressOf checkedChanged
            End If
        End If
    End Sub

    Private Sub checkedChanged(ByVal index As Integer)
        Dim oldValue As CheckState = If(isCheckedItem(index), CheckState.Checked, CheckState.Unchecked)
        Dim newValue As CheckState = If(isCheckedItem(index), CheckState.Unchecked, CheckState.Checked)
        isCheckedItem(index) = Not isCheckedItem(index)
        Me.Invalidate()
        checkedIndices.Clear()
        checkedIndices.AddRange(Enumerable.Range(0, isCheckedItem.Count).Where(Function(x) isCheckedItem(x)).Select(Function(x) x).ToArray)
        checkedItems.Clear()
        checkedItems.AddRange(Enumerable.Range(0, isCheckedItem.Count).Where(Function(x) isCheckedItem(x)).Select(Function(x) MyBase.GetItemText(MyBase.Items(x))).ToArray)
        RaiseEvent ItemCheck(Me, New ItemCheckEventArgs(index, newValue, oldValue))
    End Sub

End Class

Public Class nWindow
    Inherits NativeWindow

    Private combo As CheckedCombobox

    Public Event checkedChanged(ByVal index As Integer)

    Public Sub New(ByVal cb As CheckedCombobox)
        combo = cb
    End Sub

    Private Const WM_LBUTTONDOWN As Integer = &H201

    Private lastIndex As Integer = -1

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_LBUTTONDOWN Then
            Dim itemHeight As Integer = combo.ItemHeight
            If New Point(m.LParam.ToInt32).Y \ itemHeight <= combo.Items.Count - 1 And New Point(m.LParam.ToInt32).Y \ itemHeight >= 0 Then
                lastIndex = New Point(m.LParam.ToInt32).Y \ itemHeight
                If New Point(m.LParam.ToInt32).X >= 1 And New Point(m.LParam.ToInt32).X <= 11 Then
                    RaiseEvent checkedChanged(lastIndex)
                End If
            End If
        End If
        MyBase.WndProc(m)
    End Sub

End Class

