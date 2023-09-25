Public Class Form1

    Private Sub CheckedCombobox1_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedCombobox1.ItemCheck
        'this displays some information in the listboxes
        Dim items() As String = CheckedCombobox1.CheckedComboboxItems.Cast(Of String).ToArray
        ListBox1.Items.Clear()
        For x As Integer = 0 To items.GetUpperBound(0)
            ListBox1.Items.Add(items(x) & " - Checked = " & CheckedCombobox1.checkedIndices.Contains(x).ToString)
        Next
        ListBox2.Items.Clear()
        ListBox2.Items.AddRange(CheckedCombobox1.checkedItems.ToArray)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        MsgBox(CheckedCombobox1.checkedIndices.Contains(0))

        'this demonstrates adding, inserting, + removing items
        If CheckedCombobox1.CheckedComboboxItems.Count = 4 Then
            CheckedCombobox1.CheckedComboboxItems.Add("five")
        ElseIf CheckedCombobox1.CheckedComboboxItems.Count = 5 Then
            CheckedCombobox1.CheckedComboboxItems.Insert(1, "test")
        Else
            CheckedCombobox1.CheckedComboboxItems.RemoveAt(1)
        End If
    End Sub

End Class
