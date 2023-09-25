Imports System.IO
Imports System.Security
Imports System.Security.Permissions
Imports System.Threading


Public Class FileWatcher64

#Region "Atributos"

    Public Event Change(path As String)

    Public Event Create(path As String)

    Public Event Delete(path As String)

    Public Event Renamed(oldpath As String, newpath As String)

    Private _watched As String

    Private _listDocuments As List(Of String)

#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

#End Region

#Region "Propiedades"

    Public ReadOnly Property Count As Integer

        Get
            Return _listDocuments.Count
        End Get

    End Property

    Public ReadOnly Property ListDocuments As List(Of String)

        Get
            Return _listDocuments
        End Get

    End Property

#End Region

#Region "Metodos"

    Public Function ListDirectory(ByVal path_ As String) As List(Of String)

        _listDocuments = New List(Of String)

        _watched = path_

        Try

            Dim directory_ As DirectoryInfo = New DirectoryInfo(path_)

            For Each file_ In directory_.EnumerateFiles("*", SearchOption.AllDirectories).AsParallel()

                If Not file_.FullName.Contains("Thumbs.db") Then

                    _listDocuments.Add(file_.FullName)

                End If

            Next

        Catch ex As DirectoryNotFoundException
            EscribirEnLog("Directory not found: " & ex.Message)

        Catch ex As SecurityException
            EscribirEnLog("Security Exception:\n\n " & ex.Message)

        Catch ex As Exception
            EscribirEnLog("Exception occurred:" & ex.Message)

        End Try

        Return _listDocuments

    End Function

    <PermissionSet(SecurityAction.Demand, Name:="FullTrust")>
    Public Sub WatchDirectory()

        Dim fswatcher As New System.IO.FileSystemWatcher()

        fswatcher.Path = _watched

        fswatcher.Filter = "*.*"

        fswatcher.IncludeSubdirectories = True

        fswatcher.InternalBufferSize = 65536

        AddHandler fswatcher.Changed, AddressOf OnChanged

        AddHandler fswatcher.Created, AddressOf OnCreated

        AddHandler fswatcher.Deleted, AddressOf OnDeleted

        'AddHandler fswatcher.Renamed, AddressOf OnRenamed

        AddHandler fswatcher.Error, AddressOf OnError

        fswatcher.EnableRaisingEvents = True

    End Sub

    Private Sub EscribirEnLog(ByVal t_Texto_ As String)

        Dim t_Ruta_ As String = "C:\logs\RecolectorDocumentos\fileWatcher.txt"

        Dim escritor_ As StreamWriter

        Try

            escritor_ = File.AppendText(t_Ruta_)

            escritor_.WriteLine(t_Texto_)

            escritor_.Flush()

            escritor_.Close()

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Eventos"

    Private Sub OnChanged(src As Object, evt As FileSystemEventArgs)

        RaiseEvent Change(evt.FullPath)

    End Sub

    Private Sub OnDeleted(src As Object, evt As FileSystemEventArgs)

        Dim tList As List(Of String) = New List(Of String)

        Dim thepath As String = evt.FullPath

        For Each path As String In _listDocuments

            If path.StartsWith(thepath) Then

                tList.Add(path)

            End If

        Next

        For Each path As String In tList

            _listDocuments.Remove(path)

            RaiseEvent Delete(path)

        Next

    End Sub

    Private Sub OnCreated(src As Object, evt As FileSystemEventArgs)

        If Not evt.FullPath.Contains("Thumbs.db") Then

            If File.Exists(evt.FullPath) Then

                _listDocuments.Add(evt.FullPath)

                EscribirEnLog(System.DateTime.Now & " Documento creado: " & evt.FullPath.ToString())

            End If

        End If

        RaiseEvent Create(evt.FullPath)

    End Sub

    Private Sub OnRenamed(src As Object, evt As RenamedEventArgs)

        Dim thepath As String = evt.OldFullPath

        Dim fname As String = ""

        Dim dicRename As Dictionary(Of String, String) = New Dictionary(Of String, String)

        For Each path As String In _listDocuments

            If path.StartsWith(thepath) Then

                If Equals(path, thepath) Then

                    dicRename.Add(path, evt.FullPath)

                Else

                    fname = path.Substring(path.LastIndexOf("\") + 1)

                    dicRename.Add(path, evt.FullPath & "\" & fname)

                End If

            End If

        Next

        For Each KVPair As KeyValuePair(Of String, String) In dicRename

            _listDocuments.Remove(KVPair.Key)

            _listDocuments.Add(KVPair.Value)

            RaiseEvent Renamed(KVPair.Key, KVPair.Value)

        Next

    End Sub

    Private Sub OnError(src As Object, evt As ErrorEventArgs)

        Dim ex As Exception = evt.GetException()

        If Not ex.InnerException Is Nothing Then

            EscribirEnLog(System.DateTime.Now & " Error: " & ex.InnerException.ToString())

        End If

    End Sub

#End Region

End Class
