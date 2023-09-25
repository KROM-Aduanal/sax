Imports wma.Exceptions
Imports gsol

Public Class ShellWma

#Region "Attributes"

    Private _system As Organismo

    Private _tagwatcher As TagWatcher


#End Region

#Region "Builders"

    Sub New()

        _system = New Organismo

        _tagwatcher = New TagWatcher

    End Sub

#End Region

#Region "Properties"

#End Region

#Region "Methods"

    Public Function CreatePackage(ByVal idFamilyTableGroup_ As String,
                                  ByVal tableName_ As String,
                                  Optional ByVal dataBaseName_ As String = "Solium.",
                                  Optional ByVal idUser_ As Int32 = 139,
                                  Optional ByVal idApplication_ As Int32 = 4,
                                  Optional ByVal idPermission_ As Int32 = 118,
                                  Optional ByVal primaryKeyName_ As String = "null") As TagWatcher

        Dim script_ As String =
                            " declare @t_familia as varchar(3) " & _
                            " declare @t_Tabla as varchar(50)" & _
                            " declare @t_VT as varchar(100)" & _
                            " declare @t_Ve as varchar(100)" & _
                            " declare @t_BD as varchar(100)" & _
                            " set @t_familia = '" & idFamilyTableGroup_ & "'" & _
                            " set @t_Tabla  = 'Ext" & idFamilyTableGroup_ & tableName_ & "'" & _
                            " set @t_VT = 'Vt" & idFamilyTableGroup_ & tableName_ & "'" & _
                            " set @t_Ve = 'Ve" & idFamilyTableGroup_ & "IU" & tableName_ & "'" & _
                            " set @t_BD = '" & dataBaseName_ & "'" & _
                            " exec Sp000EVAutomaticGenerator " & primaryKeyName_ & "," & _
                                             " @t_Tabla," & _
                                               " @t_Ve," & _
                                               " 0," & _
                                               " @t_BD, " & _
                                               " @t_VT,  " & _
                                               " 0," & _
                                               " 0," & _
                                               " 1," & _
                                               " " & idUser_ & ", " & _
                                                " " & idApplication_ & ",  " & _
                                                " ''," & _
                                                " ''," & _
                                                " " & idPermission_

        _tagwatcher = _system.ComandosSingletonSQL(script_)

        If _tagwatcher.Status = TagWatcher.TypeStatus.Ok Then

            '  _tagwatcher.ObjectReturned =
            '
        End If

    End Function

#End Region


End Class
