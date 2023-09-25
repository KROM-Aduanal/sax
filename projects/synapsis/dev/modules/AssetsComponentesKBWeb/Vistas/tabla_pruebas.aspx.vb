Imports System.Data.SqlClient


Public Class tabla_pruebas
    Inherits System.Web.UI.Page

    Dim dic1 As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

    Public ReadOnly Property lista1 As List(Of Dictionary(Of String, Object))

        Get

            Return dic1

        End Get

    End Property

    Dim dic2 As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

    Public ReadOnly Property lista2 As List(Of Dictionary(Of String, Object))

        Get

            Return dic2

        End Get

    End Property

    

    Dim conn As New SqlConnection

    Public Sub connect()
        Dim DatabaseName As String = "Solium"
        Dim server As String = "10.66.2.102"
        Dim userName As String = "sa"
        Dim password As String = "S0l1umF0rW"
        If Not conn Is Nothing Then conn.Close()
        conn.ConnectionString = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false", server, userName, password, DatabaseName)
        Try
            conn.Open()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        'conn.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'connect()

        ''and table_name like '%001%'
        'Dim strSQL = "SELECT table_name FROM information_schema.tables WHERE table_type = 'base table'"
        'Dim da As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)

        'Dim ds As New DataSet()

        'da.Fill(ds)

        'For Each row As DataRow In ds.Tables(0).Rows

        '    Dim table As String = row.Item("table_name")

        '    If table.Contains("001") Or table.Contains("002") Then

        '        Dim campos As List(Of Dictionary(Of String, String)) = New List(Of Dictionary(Of String, String))

        '        Dim strSQL2 = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" & table & "'"

        '        Dim da2 As SqlDataAdapter = New SqlDataAdapter(strSQL2, conn)

        '        Dim ds2 As New DataSet()

        '        da2.Fill(ds2)

        '        For Each fila As DataRow In ds2.Tables(0).Rows


        '            Dim _fila As Dictionary(Of String, String) = New Dictionary(Of String, String)

        '            _fila.Add("name", fila("COLUMN_NAME").ToString)

        '            _fila.Add("type", fila("DATA_TYPE").ToString)

        '            campos.Add(_fila)

        '        Next

        '        Dim dic As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        '        dic.Add("tabla", table)
        '        dic.Add("campos", campos)

        '        If table.Contains("001") Then

        '            dic1.Add(dic)

        '        ElseIf table.Contains("002") Then

        '            dic2.Add(dic)

        '        End If





        '    End If

        'Next row

        'conn.Close()
    End Sub

End Class