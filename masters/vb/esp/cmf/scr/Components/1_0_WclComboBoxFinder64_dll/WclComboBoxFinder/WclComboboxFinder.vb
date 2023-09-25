Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization

Namespace Wma.Components

    Public Class WclComboboxFinder
        Inherits ComboBox

        Public Sub New()
            DrawMode = DrawMode.OwnerDrawVariable
        End Sub

        Public Overloads Property DrawMode As DrawMode
            Get
                Return MyBase.DrawMode
            End Get
            Set(ByVal value As DrawMode)

                If value <> DrawMode.OwnerDrawVariable Then
                    Throw New NotSupportedException("Needs to be DrawMode.OwnerDrawVariable")
                End If

                MyBase.DrawMode = value
            End Set
        End Property

        Public Overloads Property DropDownStyle As ComboBoxStyle
            Get
                Return MyBase.DropDownStyle
            End Get
            Set(ByVal value As ComboBoxStyle)

                If value = ComboBoxStyle.Simple Then
                    Throw New NotSupportedException("ComboBoxStyle.Simple not supported")
                End If

                MyBase.DropDownStyle = value
            End Set
        End Property

        Protected Overrides Sub OnDataSourceChanged(ByVal e As EventArgs)
            MyBase.OnDataSourceChanged(e)
            InitializeColumns()
        End Sub

        Protected Overrides Sub OnValueMemberChanged(ByVal e As EventArgs)
            MyBase.OnValueMemberChanged(e)
            InitializeValueMemberColumn()
        End Sub

        Protected Overrides Sub OnDropDown(ByVal e As EventArgs)
            MyBase.OnDropDown(e)
            Me.DropDownWidth = CInt(CalculateTotalWidth())
        End Sub



        Const columnPadding As Integer = 5
        Private columnWidths As Single() = New Single(-1) {}
        Private columnNames As String() = New String(-1) {}
        Private valueMemberColumnIndex As Integer = 0

        Private Sub InitializeColumns()
            Dim propertyDescriptorCollection As PropertyDescriptorCollection = DataManager.GetItemProperties()
            columnWidths = New Single(propertyDescriptorCollection.Count - 1) {}
            columnNames = New String(propertyDescriptorCollection.Count - 1) {}

            For colIndex As Integer = 0 To propertyDescriptorCollection.Count - 1
                Dim name As String = propertyDescriptorCollection(colIndex).Name
                columnNames(colIndex) = name
            Next
        End Sub

        Private Sub InitializeValueMemberColumn()
            Dim colIndex As Integer = 0

            For Each columnName As String In columnNames

                If String.Compare(columnName, ValueMember, True, CultureInfo.CurrentUICulture) = 0 Then
                    valueMemberColumnIndex = colIndex
                    Exit For
                End If

                colIndex += 1
            Next


        End Sub

        Private Function CalculateTotalWidth() As Single
            Dim totWidth As Single = 0

            For Each width As Integer In columnWidths
                totWidth += (width + columnPadding)
            Next

            Return totWidth + SystemInformation.VerticalScrollBarWidth
        End Function

        Protected Overrides Sub OnMeasureItem(ByVal e As MeasureItemEventArgs)
            MyBase.OnMeasureItem(e)
            If DesignMode Then Return

            For colIndex As Integer = 0 To columnNames.Length - 1
                Dim item As String = Convert.ToString(FilterItemOnProperty(Items(e.Index), columnNames(colIndex)))
                Dim sizeF As SizeF = e.Graphics.MeasureString(item, Font)
                columnWidths(colIndex) = Math.Max(columnWidths(colIndex), sizeF.Width)
            Next

            Dim totWidth As Single = CalculateTotalWidth()
            e.ItemWidth = CInt(totWidth)
        End Sub

        Protected Overrides Sub OnDragDrop(drgevent As DragEventArgs)
            MyBase.OnDragDrop(drgevent)

            MsgBox("OnDragDrop")

        End Sub

        Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
            MyBase.OnTextChanged(e)

            If String.IsNullOrEmpty(Me.Text) Then Return

            Dim start = Me.SelectionStart
            Dim length = Me.SelectionLength

            Dim chars = Me.Text.ToCharArray().ToList()

            If chars.Count = 3 Then
                If (chars(2) <> " "c) Then
                    chars.Add(chars(2))
                    chars(2) = " "c
                    start += 1
                End If
            ElseIf chars.Count = 6 Then
                If (chars(5) <> "-"c) Then
                    chars.Add(chars(5))
                    chars(5) = "-"c
                    start += 1
                End If
            End If

            Me.Text = New String(chars.ToArray())
            Me.SelectionStart = start
            Me.SelectionLength = length
        End Sub

        Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
            MyBase.OnDrawItem(e)

            '  MsgBox("OnDrawItem")

            If DesignMode Then Return
            e.DrawBackground()
            Dim boundsRect As Rectangle = e.Bounds
            Dim lastRight As Integer = 0

            Using linePen As Pen = New Pen(SystemColors.GrayText)

                Using brush As SolidBrush = New SolidBrush(ForeColor)

                    If columnNames.Length = 0 Then
                        e.Graphics.DrawString("@" & Convert.ToString(Items(e.Index)), Font, brush, boundsRect)
                    Else

                        For colIndex As Integer = 0 To columnNames.Length - 1
                            Dim item As String = Convert.ToString(FilterItemOnProperty(Items(e.Index), columnNames(colIndex)))
                            boundsRect.X = lastRight
                            boundsRect.Width = CInt(columnWidths(colIndex)) + columnPadding
                            lastRight = boundsRect.Right

                            If colIndex = valueMemberColumnIndex Then

                                Using boldFont As Font = New Font(Font, FontStyle.Bold)
                                    e.Graphics.DrawString("&" & item, boldFont, brush, boundsRect)
                                End Using
                            Else
                                e.Graphics.DrawString(item, Font, brush, boundsRect)
                            End If

                            If colIndex < columnNames.Length - 1 Then
                                e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom)
                            End If
                        Next
                    End If
                End Using
            End Using

            e.DrawFocusRectangle()
        End Sub

    End Class
End Namespace
