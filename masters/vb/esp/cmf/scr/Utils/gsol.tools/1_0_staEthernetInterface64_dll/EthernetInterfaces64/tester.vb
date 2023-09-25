Imports EthernetInterfaces64.gsol.Tools
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Data.SqlClient

Module tester

    Public Enum TypesOfValidation
        Basic
        Medium
        High
    End Enum

    Public Enum QueryModes
        Query
        Excecute
    End Enum

    Sub main()

        Dim _userInformation As New UserInformation

        'MsgBox("ok")
        Dim userRegistered_ As String =
            UserDataValidation(_userInformation,
                               TypesOfValidation.High,
                               True)

        If Not userRegistered_ Is Nothing Then

            MsgBox("Welcome: " & userRegistered_)

        Else

            MsgBox("Sorry your your profile was not found!, please contact Krom technical support")

        End If



    End Sub



    Private Function UserDataValidation( _
                     ByVal userdata_ As UserInformation,
                     ByVal validationStrictMode_ As TypesOfValidation,
                     ByVal setLastDateOfReview_ As Boolean) As String

        Dim additionalValidations_ As String = Nothing

        Select Case validationStrictMode_

            Case TypesOfValidation.Basic

                additionalValidations_ = Nothing

            Case TypesOfValidation.Medium

                additionalValidations_ =
                    "   and replace(ku.t_ip,' ','') = '" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP & "' " & _
                    "   and upper(ku.t_Dominio) = upper('" & userdata_.GetNetWorkInterfaces.DomainName & "') "

            Case TypesOfValidation.High

                additionalValidations_ =
                    "   and replace(ku.t_ip,' ','') = '" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP & "' " & _
                    "   and upper(ku.t_Dominio) = upper('" & userdata_.GetNetWorkInterfaces.DomainName & "') " & _
                    "   and upper(replace(replace(replace(ku.t_Mac,':',''),'-',''),' ','')) = '" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress & "' "
        End Select

        Dim query1_ As String = _
                    " Select ku.i_Cve_EncKardexEquipoEjecutivo, ku.t_NombreCompleto, " & _
                    " ku.i_Cve_EncKardexEquipoEjecutivo, ku.t_Departamento, ku.t_puesto, " & _
                    " ku.t_Dominio, ku.t_Usuario, ku.t_Ip, ku.t_Mac " & _
                    " from _Vt016ReporteKardexEquipoComputoActual as ku " & _
                    " where ku.t_Usuario = '" & userdata_.GetNetWorkInterfaces.UserName & "' " & _
                    "   and t_Equipo = '" & userdata_.ComputerName & "' " & _
                    "   and ku.t_Estatus = 'Activo'" & _
                    additionalValidations_

        Dim dataTable_ As New DataTable

        If ExecuteRemoteQuery(query1_, dataTable_, QueryModes.Query) Then

            If Not dataTable_.Rows Is Nothing Then

                If dataTable_.Rows.Count > 0 Then

                    Dim encounteredKey_ As String = dataTable_(0).Item("i_Cve_EncKardexEquipoEjecutivo")
                    Dim encounteredName_ As String = dataTable_(0).Item("t_NombreCompleto")

                    If setLastDateOfReview_ And
                       IsNumeric(encounteredKey_) Then

                        dataTable_.Clear()

                        Dim query2_ As String = _
                           " Update Det000KardexEquipoEjecutivo " & _
                           " f_FechaRevision = getdate()" & _
                           " where i_Cve_EncKardexEquipoComputo = " & encounteredKey_

                        If ExecuteRemoteQuery(query2_, dataTable_, QueryModes.Excecute) Then

                            'MsgBox(encounteredKey_ & " " & encounteredName_ & ", Updated last review.")

                            Return encounteredName_

                        End If

                    Else

                        'MsgBox(dataTable_(0).Item("i_Cve_EncKardexEquipoComputo") & " " & dataTable_(0).Item("t_NombreCompleto"))

                        Return encounteredName_

                    End If

                End If

            End If

        End If

        Return Nothing

    End Function

    Private Function ExecuteRemoteQuery(ByVal query_ As String,
                                        ByRef dataTableResult_ As DataTable,
                                        ByVal QueryMode_ As QueryModes) As Boolean
        Try

            Using connection_ As New SqlConnection( _
    "Data Source=187.157.188.149;Initial Catalog=solium;Persist Security Info=True;User ID=krombase;Password=K1s45gri")

                connection_.Open()

                Using dataAdapter_ As New SqlDataAdapter(query_, connection_)

                    If QueryMode_ = QueryModes.Query Then

                        dataAdapter_.Fill(dataTableResult_)

                    End If

                End Using

                connection_.Close()

            End Using

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Class UserInformation

#Region "Attributes"

        Private _networkData As EthernetInterface

        Public OSVersion As String

        Public OSPlatform As String

        Public OSFullName As String

        Public TotalPhysicalMemory As String

        Public TotalVirtualMemory As String

        Public InstalledUICulture As String

        Public AvailableFreeSpace As String

        Public ComputerName As String

        Sub New()

            _networkData = New EthernetInterface

            OSVersion = My.Computer.Info.OSVersion

            OSPlatform = My.Computer.Info.OSPlatform

            OSFullName = My.Computer.Info.OSFullName

            TotalPhysicalMemory = My.Computer.Info.TotalPhysicalMemory

            TotalVirtualMemory = My.Computer.Info.TotalVirtualMemory

            InstalledUICulture = My.Computer.Info.InstalledUICulture.Name

            AvailableFreeSpace = My.Computer.FileSystem.Drives(0).AvailableFreeSpace

            ComputerName = My.Computer.Name

        End Sub

#End Region

#Region "Properties"

        ReadOnly Property GetNetWorkInterfaces As EthernetInterface

            Get

                Return _networkData

            End Get

        End Property

#End Region

    End Class

End Module
