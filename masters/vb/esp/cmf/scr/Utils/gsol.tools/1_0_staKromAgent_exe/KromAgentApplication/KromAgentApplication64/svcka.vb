Imports Microsoft.Win32
Imports System.Data.SqlClient
Imports KromAgentApplication64.gsol.Tools
Imports System.Management

Public Class svchostka

#Region "Attributes"

    Private _userdata As New UserInformation

    Private _segundosEspera As Double = 0

    Private _fechaLimiteRegistro As Date = Now

    Private _toleranciaActiva As Boolean = False

    Public Enum TypesOfValidation
        Basic
        Medium
        High
    End Enum

    Public Enum QueryModes
        Query
        Excecute
    End Enum

#End Region


#Region "Methods"

    Private Sub Form1_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.HandleCreated
        HookKeyboard.Jam()
    End Sub
    Private Sub Form1_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.HandleDestroyed
        HookKeyboard.UnJam()
    End Sub

    Private Sub cierraAplicacionPorExcepcion()
        If EnableTaskManager(True) Then

        End If

        Process.Start("explorer.exe")

        Me.Close()
    End Sub

    Private Function RevisionPasswordTemporal() As Boolean

        'hoy es  sabado 05 de mayo 2018

        Dim primerCaracter_ As String = Now.Day.ToString  ' día de la semana, numero natural sin ceros a la izquierda = 5
        Dim segundoCaracter_ As String = UCase(Mid(Now.ToString("dddd"), Now.ToString("dddd").Length - 1, 2))  ' dos últimos ´digitos del día, mayuscula ( lunES)  = DO
        Dim tercerCaracter_ As String = (Now.DayOfWeek * Now.Day).ToString ' día de la semana multiplicado por día del mes = 6*5 = 30
        Dim cuartoCaracter_ As String = LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) ' ultima letra minuscula del mes ej. Mayo, = o
        Dim quintoCaracter_ As String = Now.DayOfWeek ' ultima letra minuscula del mes ej. Mayo, = o
        Dim sextoCaracter_ As String = LCase(Mid(Now.ToString("dddd"), 1, 1)) & LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) 'primera letra del dia y penultima letra del mes minusculas ej. Sábado y Mayo = so

        'password= 5ES10090oso

        If tbPassword.Text = primerCaracter_ & _
               segundoCaracter_ & _
               tercerCaracter_ & _
               cuartoCaracter_ & _
               quintoCaracter_ & _
               sextoCaracter_ Then
            Return True
        Else
            Return False

        End If


    End Function

    Private Sub btnDesbloquear_Click(sender As Object, e As EventArgs) Handles btnDesbloquear.Click

        If _toleranciaActiva Then

            Dim finalDate_ As New Date
            finalDate_ = _fechaLimiteRegistro

            If finalDate_ > Now Then
                MsgBox("Estimado usuario, este equipo no se encontró en el control de inventario de Krom Aduanal, temporalmente podrá desbloquearlo sin embargo, a partir del día " & _fechaLimiteRegistro & " todos los equipos sin registro serán bloqueados, por favor solicite el registro correcto de su equipo de cómputo a la jefatura de infomática local, gracias.")

                cierraAplicacionPorExcepcion()

            Else

                If tbPassword.Text Is Nothing Or tbPassword.Text = "" Then

                    MsgBox("La prórroga activa ya no es válida, el equipo no puede ser desbloqueado por favor registrelo adecuadamente e intente con el botón verificar ")

                ElseIf RevisionPasswordTemporal() Then

                    MsgBox("Estimado usuario, este equipo no se encontró en el control de inventario de Krom Aduanal, temporalmente podrá desbloquearlo, por favor solicite el registro correcto de su equipo de cómputo a la jefatura de infomática local, gracias.")

                    'Cierra la aplicación
                    cierraAplicacionPorExcepcion()


                Else

                    MsgBox("La contraseña es incorrecta.")

                End If

            End If


        Else

            If tbPassword.Text Is Nothing Or tbPassword.Text = "" Then

                MsgBox("Actualmente no existe prórroga configurada, el equipo no puede ser desbloqueado por favor registrelo adecuadamente e intente con el botón verificar")

            ElseIf RevisionPasswordTemporal() Then
                'Cierra la aplicación

                MsgBox("Estimado usuario, este equipo no se encontró en el control de inventario de Krom Aduanal, temporalmente podrá desbloquearlo, por favor solicite el registro correcto de su equipo de cómputo a la jefatura de infomática local, gracias.")

                cierraAplicacionPorExcepcion()

            Else

                MsgBox("La contraseña es incorrecta.")

            End If

        End If

    End Sub

    Sub Verify(ByVal hide_ As Boolean,
               ByVal useServerParameter_ As Boolean)

        'Temporal
        If hide_ Then
            Me.Hide()
        End If

        SetOnStartUp(True)

        Dim _userInformation As New UserInformation

        Dim userRegistered_ As String =
            UserDataValidation(_userInformation,
                               TypesOfValidation.High,
                               True)

        If Not userRegistered_ Is Nothing Then ' Try high

            Me.Close()

            'Temporal
            If hide_ = False Then
                Process.Start("explorer.exe")
            End If

        Else

            userRegistered_ =
                UserDataValidation(_userInformation,
                                   TypesOfValidation.Medium,
                                   True)

            If Not userRegistered_ Is Nothing Then 'Try Medium

                Me.Close()

                'Temporal
                If hide_ = False Then

                    Process.Start("explorer.exe")
                End If


            ElseIf userRegistered_ = "Unconnect" Then ' Not found

                'MsgBox("Unconnected")

            Else

                RegistrarIntentoFallido(_userdata)

                NotifyUser(_userdata)

                'Temporal
                Me.Visible = True

                Me.Show()

                If Me.EnableTaskManager(False) Then
                    'Disabled ok
                End If

                Process.Start("taskkill.exe", " -im explorer.exe -f")

            End If

        End If

    End Sub

    Public Sub RegistrarIntentoFallido(ByVal userdata_ As UserInformation)

        Dim ips_ As String = Nothing
        Dim macs_ As String = Nothing
        Dim totalRAM_ As String = Nothing
        Dim totalDiscoDuro_ As String = Nothing
        Dim totalDiscoDuroEspacioLibre_ As String = Nothing
        Dim nombreDominio_ As String = Nothing
        Dim tipoInterface_ As String = Nothing


        Try
            tipoInterface_ = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).NetworkInterfaceType

        Catch ex As Exception

            tipoInterface_ = "Error al detectar el Eth Type"

        End Try

        Try
            nombreDominio_ = userdata_.GetNetWorkInterfaces.DomainName

        Catch ex As Exception

            nombreDominio_ = "Error al detectar el dominio"

        End Try

        Try

            'For Each ip_ As String In userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP

            ips_ = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP

            'Next

        Catch ex As Exception
            ips_ = "Error al dectectar la IP"
        End Try

        Try

            'For Each mac_ As String In userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress

            macs_ = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress

            'Next

        Catch ex As Exception

            ips_ = "Error al dectectar MAC"

        End Try

        Try

            totalRAM_ = Math.Round((((userdata_.TotalPhysicalMemory / 1024) / 1024) / 1024), 0).ToString

            totalRAM_ = totalRAM_.Replace(",", Nothing)


        Catch ex As Exception

            totalRAM_ = "-1"

        End Try

        Try

            totalDiscoDuro_ = Math.Round((((userdata_.HardDiskTotalFreeSpace / 1024) / 1024) / 1024), 0).ToString
            totalDiscoDuroEspacioLibre_ = Math.Round((((userdata_.HardDiskTotalSize / 1024) / 1024) / 1024), 0).ToString

            totalDiscoDuro_ = totalDiscoDuro_.Replace(",", Nothing)

            totalDiscoDuroEspacioLibre_ = totalDiscoDuroEspacioLibre_.Replace(",", Nothing)

        Catch ex As Exception

            totalDiscoDuro_ = "-1"
            totalDiscoDuroEspacioLibre_ = "-1"

        End Try

        Dim dataTable_ As New DataTable

        Dim query2_ As String = _
        " insert into Bit000MonitoreoEquipos " & _
        " (i_Cve_EncKardexEquipoComputo, " & _
        " f_FechaHoraRevision, " & _
        " i_Cve_EjecutivosMisEmpresas, " & _
        " t_Usuario, " & _
        " t_NombreEquipo, " & _
        " t_Dominio, " & _
        " t_TipoInterfaz, " & _
        " t_IP, " & _
        " t_ClaveMAC, " & _
        " i_TotalMemoriaFisica, " & _
        " i_TotalEspacioLibreDiscoDuro, " & _
        " i_TotalEspacioDiscoDuro, " & _
        " t_SistemaOperativoDetectado, " & _
        " t_Procesador, " & _
        " t_FamiliaProcesador, " & _
        " t_FabricanteProcesador, " & _
        " t_FabricantePlacaBase, " & _
        " t_NumeroSeriePlacaBase, " & _
        " t_ModeloDiscoDuro, " & _
        " t_InterfazDiscoDuro, " & _
        " t_NumeroSerieDiscoDuro " & _
        " ) " & _
        " Values " & _
        " ( NULL,  getdate(),NULL,  '" & userdata_.GetNetWorkInterfaces.UserName & "', " & _
                                   "'" & userdata_.ComputerName & "', " & _
                                   "'" & nombreDominio_ & "', " & _
                                   "'" & tipoInterface_ & "', " & _
 _
                                   "'" & ips_ & "', " & _
                                   "'" & macs_ & "', " & _
        "" & totalRAM_ & ", " & _
        "" & totalDiscoDuro_ & ", " & _
        "" & totalDiscoDuroEspacioLibre_ & ", " & _
        "'" & userdata_.OSManufacturer & "', " & _
        "'" & userdata_.CPUName & "', " & _
        "'" & userdata_.CPUDescription & "', " & _
        "'" & userdata_.CPUManufacturer & "', " & _
        "'" & userdata_.BaseBoardManufacturer & "', " & _
        "'" & userdata_.BaseBoardSerialNumber & "', " & _
        "'" & userdata_.HardDiskModel & "', " & _
        "'" & userdata_.HardDiskInterfaceType & "', " & _
        "'" & userdata_.HardDiskSerialNumber & "' " & _
        ");"

        If ExecuteRemoteQuery(query2_, dataTable_, QueryModes.Excecute) <> "Unconnected" Then

        End If

        'Dim ips_ As String = Nothing
        'Dim macs_ As String = Nothing
        'Dim totalRAM_ As String = Nothing
        'Dim totalDiscoDuro_ As String = Nothing
        'Dim totalDiscoDuroEspacioLibre_ As String = Nothing

    End Sub

    Private Sub startAgent()

        Dim config_ As String = LoadMonitorConfig()

        If config_ <> "Unconnected" Then

            If config_ = "OK" Then

                ' lblMensaje.Text = "Se le otorgarán, " & _segundosEspera & " segundos para registrar su equipo, a partir del día " & _fechaLimiteRegistro & " todos los equipos sin registro serán bloqueados sin prorroga"

                lblMensaje.Text = _fechaLimiteRegistro & ", fecha de prórroga para registro"

                Verify(True, True)

            Else

                Verify(True, False)

            End If

        Else

            Me.Close()

        End If

    End Sub

    Private Sub svchostka_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Set Off Temporally
        startAgent()

    End Sub

    Private Function LoadMonitorConfig() As String

        Dim query1_ As String = _
                    "Select " & _
                    " i_Cve_AgenteEquipoComputo," & _
                    " i_SegundosEspera," & _
                    " f_FechaLimiteBloqueoDefinitivo," & _
                    " i_Cve_Estado," & _
                    " i_Cve_Estatus " & _
                    " from Con000AgenteEquiposComputo"

        Dim dataTable_ As New DataTable

        If ExecuteRemoteQuery(query1_, dataTable_, QueryModes.Query) <> "Unconnected" Then

            If Not dataTable_.Rows Is Nothing Then

                If dataTable_.Rows.Count > 0 Then

                    _segundosEspera = dataTable_(0).Item("i_SegundosEspera")

                    _fechaLimiteRegistro = dataTable_(0).Item("f_FechaLimiteBloqueoDefinitivo")

                    If dataTable_(0).Item("i_Cve_Estatus") = "1" Then
                        _toleranciaActiva = True
                    Else
                        _toleranciaActiva = False

                    End If

                    Return "OK"

                End If
            Else

                Return Nothing

            End If

        Else

            Return "Unconnected"

        End If

        Return Nothing

    End Function

    Private Function NotifyUser(ByVal userdata_ As UserInformation) As Boolean

        Dim query_ As String =
        "DECLARE @tableHTML  NVARCHAR(MAX);  " & _
        " SET @tableHTML =  " & _
        " N'<H1>El equipo [" & userdata_.ComputerName & "] está tratando de autenticar en  KROM, sin embargo no se encontró coincidencia en el Kardex</H1>' +  " & _
        " N'<table border=1>' +  " & _
        " N'<tr><th>Nombre Equipo</th><th>" & userdata_.ComputerName & "</th></tr>' +  " & _
        " N'<tr><th>Usuario Windows</th><th>" & userdata_.GetNetWorkInterfaces.UserName & "</th></tr>' +  " & _
        " N'<tr><th>Dominio</th><th>" & userdata_.GetNetWorkInterfaces.DomainName & "</th></tr>' +  " & _
        " N'<tr><th>Tipo de interfaz</th><th>" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).NetworkInterfaceType & "</th></tr>' +  " & _
        " N'<tr><th>Número IP</th><th>" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP & "</th></tr>' +  " & _
        " N'<tr><th>Clave MAC</th><th>" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress & "</th></tr>' +  " & _
        " N'<tr><th>Fabricante S.O.</th><th>" & userdata_.OSManufacturer & "</th></tr>' +  " & _
        " N'<tr><th>Procesador</th><th>" & userdata_.CPUName & "</th></tr>' +  " & _
        " N'<tr><th>Familia Procesador</th><th>" & userdata_.CPUDescription & "</th></tr>' +  " & _
        " N'<tr><th>Fabricante Procesador</th><th>" & userdata_.CPUManufacturer & "</th></tr>' +  " & _
        " N'<tr><th>Fabricante Placa Base</th><th>" & userdata_.BaseBoardManufacturer & "</th></tr>' +  " & _
        " N'<tr><th>Número serie Placa Base</th><th>" & userdata_.BaseBoardSerialNumber & "</th></tr>' +  " & _
        " N'<tr><th>Memoria RAM</th><th>" & Math.Round((((userdata_.TotalPhysicalMemory / 1024) / 1024) / 1024), 2).ToString & " GB</th></tr>' +  " & _
        " N'<tr><th>Espacio Libre HD</th><th>" & Math.Round((((userdata_.HardDiskTotalFreeSpace / 1024) / 1024) / 1024), 2).ToString & " GB</th></tr>' +  " & _
        " N'<tr><th>Espacio Total HD</th><th>" & Math.Round((((userdata_.HardDiskTotalSize / 1024) / 1024) / 1024), 2).ToString & " GB</th></tr>' +  " & _
        " N'<tr><th>Modelo HD</th><th>" & userdata_.HardDiskModel & "</th></tr>' +  " & _
        " N'<tr><th>Interfaz HD</th><th>" & userdata_.HardDiskInterfaceType & "</th></tr>' +  " & _
        " N'<tr><th>Número de Serie HD</th><th>" & userdata_.HardDiskSerialNumber & "</th></tr>' +  " & _
        " N'</table>' ;  " & _
        " EXEC msdb.dbo.sp_send_dbmail @recipients='encargadosti@kromaduanal.com',  " & _
        " @subject = 'Alerta, Equipo [" & userdata_.ComputerName & "] sin registrar o sin actualizar',  " & _
        " @body = @tableHTML,  " & _
        " @body_format = 'HTML' , " & _
        " @profile_name='KROM';"


        Dim datatable_ As New DataTable

        If ExecuteRemoteQuery(query_, datatable_, QueryModes.Query) <> "Unconnected" Then

            Return True

        End If

        Return False

    End Function

    Public Function UserDataValidation( _
                  ByVal userdata_ As UserInformation,
                  ByVal validationStrictMode_ As TypesOfValidation,
                  ByVal setLastDateOfReview_ As Boolean) As String

        'Set values

        Dim response_ As String = "Unconnected"

        tbClaveMAC.Text = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress
        tbDescripcionRed.Text = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).Description
        tbDominio.Text = userdata_.GetNetWorkInterfaces.DomainName
        tbIdiomaSO.Text = userdata_.InstalledUICulture
        tbNombreEquipo.Text = userdata_.ComputerName
        tbNombreRed.Text = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).Name
        tbNombreSO.Text = userdata_.OSFullName
        tbNumeroIP.Text = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP
        tbPlataformaSO.Text = userdata_.OSPlatform
        tbTipoConexion.Text = userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).NetworkInterfaceType
        tbUsuarioEquipo.Text = userdata_.GetNetWorkInterfaces.UserName
        tbVersionSO.Text = userdata_.OSVersion


        'bautismen()
        tb_i_TotalMemoriaFisica.Text = Math.Round((((userdata_.TotalPhysicalMemory / 1024) / 1024) / 1024), 2).ToString
        tb_i_TotalEspacioLibreDiscoDuro.Text = Math.Round((((userdata_.HardDiskTotalFreeSpace / 1024) / 1024) / 1024), 2).ToString
        tb_i_TotalEspacioDiscoDuro.Text = Math.Round((((userdata_.HardDiskTotalSize / 1024) / 1024) / 1024), 2).ToString
        tb_t_SistemaOperativoDetectado.Text = userdata_.OSManufacturer
        tb_t_Procesador.Text = userdata_.CPUName
        tb_t_FamiliaProcesador.Text = userdata_.CPUDescription

        tb_t_FabricanteProcesador.Text = userdata_.CPUManufacturer
        tb_t_FabricantePlacaBase.Text = userdata_.BaseBoardManufacturer
        tb_t_NumeroSeriePlacaBase.Text = userdata_.BaseBoardSerialNumber

        tb_t_ModeloDiscoDuro.Text = userdata_.HardDiskModel
        tb_t_InterfazDiscoDuro.Text = userdata_.HardDiskInterfaceType
        tb_t_NumeroSerieDiscoDuro.Text = userdata_.HardDiskSerialNumber

        _userdata = userdata_

        Dim additionalValidations_ As String = Nothing

        Select Case validationStrictMode_

            Case TypesOfValidation.Basic

                lblMensaje.Text = "Rev. Basic"

                additionalValidations_ = Nothing

            Case TypesOfValidation.Medium

                lblMensaje.Text = "Rev. Medium (" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).NetworkInterfaceType & ")"

                If Not userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).NetworkInterfaceType = "Wireless80211" Then

                    additionalValidations_ =
                    "   and replace(ku.t_ip,' ','') = '" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP & "' " & _
                    "   and upper(ku.t_Dominio) = upper('" & userdata_.GetNetWorkInterfaces.DomainName & "') "
                Else

                    additionalValidations_ = Nothing

                End If

            Case TypesOfValidation.High

                lblMensaje.Text = "Rev. High"
                additionalValidations_ =
                    "   and replace(ku.t_ip,' ','') = '" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP & "' " & _
                    "   and upper(ku.t_Dominio) = upper('" & userdata_.GetNetWorkInterfaces.DomainName & "') " & _
                    "   and upper(replace(replace(replace(ku.t_Mac,':',''),'-',''),' ','')) = '" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress & "' "
        End Select

        Dim query1_ As String = _
                    " Select ku.i_Cve_EjecutivosMisEmpresas, ku.i_Cve_EncKardexEquipoEjecutivo, ku.t_NombreCompleto, " & _
                    " ku.i_Cve_EncKardexEquipoEjecutivo, ku.t_Departamento, ku.t_puesto, " & _
                    " ku.t_Dominio, ku.t_Usuario, ku.t_Ip, ku.t_Mac " & _
                    " from _Vt016ReporteKardexEquipoComputoActual as ku " & _
                    " where ku.t_Usuario = '" & userdata_.GetNetWorkInterfaces.UserName & "' " & _
                    "   and t_Equipo = '" & userdata_.ComputerName & "' " & _
                    "   and ku.t_Estatus = 'Activo'" & _
                    additionalValidations_

        Dim dataTable_ As New DataTable

        If ExecuteRemoteQuery(query1_, dataTable_, QueryModes.Query) <> "Unconnected" Then

            If Not dataTable_.Rows Is Nothing Then

                If dataTable_.Rows.Count > 0 Then

                    Dim encounteredKey_ As String = dataTable_(0).Item("i_Cve_EncKardexEquipoEjecutivo")

                    Dim encounteredName_ As String = dataTable_(0).Item("t_NombreCompleto")

                    Dim i_Cve_EjecutivosMisEmpresas_ As String = dataTable_(0).Item("i_Cve_EjecutivosMisEmpresas")

                    Dim t_Usuario_ As String = dataTable_(0).Item("t_Usuario")

                    If setLastDateOfReview_ And
                       IsNumeric(encounteredKey_) Then

                        dataTable_.Clear()

                        Dim query2_ As String = _
                       " Update Det000KardexEquipoEjecutivo " & _
                       " set f_FechaRevision = getdate()" & _
                       " where i_Cve_EncKardexEquipoComputo = " & encounteredKey_ & "; " &
                      " insert into Bit000MonitoreoEquipos " & _
                        " (i_Cve_EncKardexEquipoComputo, " & _
                        " f_FechaHoraRevision, " & _
                        " i_Cve_EjecutivosMisEmpresas, " & _
                        " t_Usuario, " & _
                        " t_NombreEquipo, " & _
                        " t_Dominio, " & _
                        " t_TipoInterfaz, " & _
                        " t_IP, " & _
                        " t_ClaveMAC, " & _
                        " i_TotalMemoriaFisica, " & _
                        " i_TotalEspacioLibreDiscoDuro, " & _
                        " i_TotalEspacioDiscoDuro, " & _
                        " t_SistemaOperativoDetectado, " & _
                        " t_Procesador, " & _
                        " t_FamiliaProcesador, " & _
                        " t_FabricanteProcesador, " & _
                        " t_FabricantePlacaBase, " & _
                        " t_NumeroSeriePlacaBase, " & _
                        " t_ModeloDiscoDuro, " & _
                        " t_InterfazDiscoDuro, " & _
                        " t_NumeroSerieDiscoDuro " & _
                        " ) " & _
                        " Values " & _
                        " ( " & encounteredKey_ & ",  getdate()," & i_Cve_EjecutivosMisEmpresas_ & ",  '" & t_Usuario_ & "', " & _
                                                   "'" & userdata_.ComputerName & "', " & _
                                                   "'" & userdata_.GetNetWorkInterfaces.DomainName & "', " & _
                                                   "'" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).NetworkInterfaceType & "', " & _
                                                   "'" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).IP & "', " & _
                                                   "'" & userdata_.GetNetWorkInterfaces.NetworkInterfaces(0).PhysicalAddress & "', " & _
                        "" & Math.Round((((userdata_.TotalPhysicalMemory / 1024) / 1024) / 1024), 2).ToString & ", " & _
                        "" & Math.Round((((userdata_.HardDiskTotalFreeSpace / 1024) / 1024) / 1024), 2).ToString & ", " & _
                        "" & Math.Round((((userdata_.HardDiskTotalSize / 1024) / 1024) / 1024), 2).ToString & ", " & _
                        "'" & userdata_.OSManufacturer & "', " & _
                        "'" & userdata_.CPUName & "', " & _
                        "'" & userdata_.CPUDescription & "', " & _
                        "'" & userdata_.CPUManufacturer & "', " & _
                        "'" & userdata_.BaseBoardManufacturer & "', " & _
                        "'" & userdata_.BaseBoardSerialNumber & "', " & _
                        "'" & userdata_.HardDiskModel & "', " & _
                        "'" & userdata_.HardDiskInterfaceType & "', " & _
                        "'" & userdata_.HardDiskSerialNumber & "' " & _
                        ");"

                        If ExecuteRemoteQuery(query2_, dataTable_, QueryModes.Excecute) <> "Unconnected" Then

                            Return encounteredName_

                        Else

                            Return "Unconnected"

                        End If

                    Else

                        Return encounteredName_

                    End If

                Else

                    Return Nothing

                End If

            Else
                Return Nothing

            End If

        Else

            Return "Unconnected"

        End If

        Return "Unconnected"

    End Function

    Private Function ExecuteRemoteQuery(ByVal query_ As String,
                                        ByRef dataTableResult_ As DataTable,
                                        ByVal QueryMode_ As QueryModes) As String
        Try

            Dim stringConnection_ As String = "Data Source=187.157.188.149;Initial Catalog=solium;Persist Security Info=True;User ID=krombase;Password=K1s45gri"

            Using connection_ As New SqlConnection(stringConnection_)

                If QueryMode_ = QueryModes.Query Then

                    connection_.Open()

                    Using dataAdapter_ As New SqlDataAdapter(query_, connection_)

                        dataAdapter_.Fill(dataTableResult_)

                    End Using

                    connection_.Close()


                ElseIf QueryMode_ = QueryModes.Excecute Then

                    ' Dim _conexion As IDbConnection

                    Dim command_ As IDbCommand

                    '_conexion = New SqlConnection

                    command_ = New SqlCommand

                    command_.Connection = connection_

                    'connection_.ConnectionString = stringConnection_

                    connection_.Open()


                    command_.CommandText = query_

                    command_.ExecuteNonQuery()

                    connection_.Close()

                End If

            End Using

            Return "Done"

        Catch ex As Exception

            MsgBox(ex.Message & "  SQL('" & query_ & "')")

            Return "Unconnected"

        End Try

    End Function

    Function SetOnStartUp(ByVal modal_ As Boolean) As Boolean

        Dim applicationName As String = Application.ProductName
        Dim applicationPath As String = Application.ExecutablePath

        Try
            Select Case modal_

                Case True 'Install


                    Dim regKey As Microsoft.Win32.RegistryKey
                    regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                    regKey.SetValue(applicationName, """" & applicationPath & """")
                    regKey.Close()

                Case Else 'Uninstall

                    Dim regKey As Microsoft.Win32.RegistryKey
                    regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                    regKey.DeleteValue(applicationName, False)
                    regKey.Close()

            End Select

        Catch ex As Exception

        End Try



    End Function


    Function EnableTaskManager(ByVal isEnabled_ As Boolean) As Boolean

        Select Case isEnabled_

            Case True ' Enable

                Try

                    Registry.CurrentUser.DeleteSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System")

                    Return True

                Catch ex As Exception

                    ' MsgBox(ex.Message)

                End Try

            Case Else

                Try ' Disable

                    Dim REGISTRADOR As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System")

                    REGISTRADOR.SetValue("DisableTaskMgr", 1)

                    REGISTRADOR.Close()

                    Return True

                Catch ex As Exception

                    ' MsgBox(ex.Message)

                End Try

        End Select

        Return False

    End Function

    Private Sub svchostka_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        If EnableTaskManager(True) Then
            'Enabled ok
        End If

        ' Process.Start("explorer.exe")

    End Sub



#End Region


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim config_ As String = LoadMonitorConfig()

        If config_ <> "Unconnected" Then

            If config_ = "OK" Then

                lblMensaje.Text = _fechaLimiteRegistro & ", fecha de prórroga para registro"


                If _toleranciaActiva Then

                    Verify(False, True)

                Else

                    Verify(False, False)

                End If

            End If

        Else

            Me.Close()

        End If

    End Sub

End Class





Public Class UserInformation

#Region "Attributes"

    Private _networkData As EthernetInterface

    Public OSVersion As String

    Public OSPlatform As String

    Public OSFullName As String

    Public TotalPhysicalMemory As String

    'New
    Public AvailablePhysicalMemory As String

    Public TotalVirtualMemory As String

    Public InstalledUICulture As String

    Public AvailableFreeSpace As String

    'New
    Public HardDiskTotalFreeSpace As String
    'New
    Public HardDiskVolumeLabel As String
    'New
    Public HardDiskTotalSize As String

    Public OSVersion2 As String
    Public OSServicePackMajorVersion As String
    Public OSServicePackMinorVersion As String
    Public OSManufacturer As String

    Public CPUName As String
    Public CPUDescription As String
    Public CPUFamily As String
    Public CPUArchitecture As String
    Public CPUManufacturer As String
    Public CPUProcessorType As String
    Public CPUSocketDesignation As String
    Public CPUVersion As String

    Public BaseBoardName As String
    Public BaseBoardDescription As String
    Public BaseBoardManufacturer As String
    Public BaseBoardPoweredOn As String
    Public BaseBoardProduct As String
    Public BaseBoardSerialNumber As String

    Public HardDiskModel As String
    Public HardDiskInterfaceType As String
    Public HardDiskSerialNumber As String

    Public ComputerName As String

    Public Sub New()

        _networkData = New EthernetInterface

        OSVersion = My.Computer.Info.OSVersion

        OSPlatform = My.Computer.Info.OSPlatform

        OSFullName = My.Computer.Info.OSFullName

        TotalPhysicalMemory = My.Computer.Info.TotalPhysicalMemory

        AvailablePhysicalMemory = My.Computer.Info.AvailablePhysicalMemory

        TotalVirtualMemory = My.Computer.Info.TotalVirtualMemory

        InstalledUICulture = My.Computer.Info.InstalledUICulture.Name

        AvailableFreeSpace = My.Computer.FileSystem.Drives(0).AvailableFreeSpace

        HardDiskTotalFreeSpace = My.Computer.FileSystem.Drives(0).TotalFreeSpace

        HardDiskVolumeLabel = My.Computer.FileSystem.Drives(0).VolumeLabel

        HardDiskTotalSize = My.Computer.FileSystem.Drives(0).TotalSize

        'Proccessor = My.Computer.

        Dim operatingSystem_ As New Management.ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")

        For Each i As ManagementObject In operatingSystem_.Get()

            OSServicePackMajorVersion = i("ServicePackMajorVersion").ToString

            OSServicePackMinorVersion = i("ServicePackMinorVersion").ToString

            OSManufacturer = i("Manufacturer").ToString()

        Next

        Dim procesador As New Management.ManagementObjectSearcher("SELECT * FROM Win32_Processor")

        For Each i As ManagementObject In procesador.Get()
            
            CPUName = LTrim(i("Name").ToString())

            CPUDescription = i("Description").ToString()

            CPUFamily = i("Family").ToString()

            CPUArchitecture = i("Architecture").ToString()

            CPUManufacturer = i("Manufacturer").ToString()

            CPUProcessorType = i("ProcessorType").ToString()

            CPUSocketDesignation = i("SocketDesignation").ToString()

            CPUVersion = i("Version").ToString()
        Next

        Dim placa As New Management.ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard")

        For Each i As ManagementObject In placa.Get()

            BaseBoardName = i("Name").ToString()

            BaseBoardDescription = i("Description").ToString()

            BaseBoardManufacturer = i("Manufacturer").ToString()

            BaseBoardPoweredOn = i("PoweredOn").ToString()

            BaseBoardProduct = i("Product").ToString()

            BaseBoardSerialNumber = i("SerialNumber").ToString()

        Next

        Dim disco As New Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")

        For Each i As ManagementObject In disco.Get()

            Try
                HardDiskModel = i("Model").ToString()

                HardDiskInterfaceType = i("InterfaceType").ToString()

                HardDiskSerialNumber = i("SerialNumber").ToString()

            Catch ex As Exception

            End Try

        Next

        ComputerName = My.Computer.Name

    End Sub

    Public Function GetProcessorId() As String
        Dim manClass As ManagementClass = New ManagementClass("Win32_Processor")
        Dim manObjCol As ManagementObjectCollection = manClass.GetInstances()
        Dim ProcessorId As String = String.Empty
        For Each manObj As ManagementObject In manObjCol
            ProcessorId = manObj.Properties("ProcessorId").Value.ToString()
        Next
        Return ProcessorId
    End Function


#End Region

#Region "Properties"

    ReadOnly Property GetNetWorkInterfaces As EthernetInterface

        Get

            Return _networkData

        End Get

    End Property

#End Region

End Class
