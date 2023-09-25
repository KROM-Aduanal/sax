Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Net
Imports System.Net.NetworkInformation

Namespace gsol.Tools

    Public Class ManifestInterface

        Public Order As Int16 = 0

        Public IP As String = Nothing

        Public Name As String = Nothing

        Public Description As String = Nothing

        Public PhysicalAddress As String = Nothing

        Public Id As String = Nothing

        Public OperationalStatus As String = Nothing

        Public Speed As String = Nothing

        Public NetworkInterfaceType As String = Nothing

        Sub New()

            Order = 0

            IP = Nothing

            Name = Nothing

            Description = Nothing

            PhysicalAddress = Nothing

            Id = Nothing

            OperationalStatus = Nothing

            Speed = Nothing

            NetworkInterfaceType = Nothing

        End Sub

    End Class

    Public Class EthernetInterface

#Region "Attributes"

        Private _networkInterfaces As List(Of ManifestInterface)

        Private _userName As String

        Private _domainName As String

        Private _hostName As String

#End Region

#Region "Builders"


        Sub New()

            _hostName = System.Net.Dns.GetHostName()

            _userName = Environment.UserName

            _domainName = Environment.UserDomainName

            _networkInterfaces = New List(Of ManifestInterface)

            GetLANSettings()

        End Sub

#End Region

#Region "Properties"

        ReadOnly Property HostName As String

            Get
                Return _hostName

            End Get

        End Property

        ReadOnly Property UserName As String

            Get
                Return _userName

            End Get

        End Property


        ReadOnly Property NetworkInterfaces As List(Of ManifestInterface)

            Get
                Return _networkInterfaces

            End Get

        End Property

        ReadOnly Property DomainName As String

            Get
                Return _domainName

            End Get

        End Property

#End Region


#Region "Methods"

        Private Sub GetLANSettings()

            Dim index_ As Int32 = 0

            For Each nic As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()

                If ((nic.NetworkInterfaceType = NetworkInterfaceType.Ethernet) Or
                    (nic.NetworkInterfaceType = NetworkInterfaceType.Wireless80211)) Then

                    If nic.OperationalStatus = OperationalStatus.Up Then

                        Dim interface_ As New ManifestInterface

                        With interface_

                            .Order = index_

                            .Id = nic.Id

                            .IP = GetIpNumberOfDescription(nic.Description)

                            .Name = nic.Name

                            .Description = nic.Description

                            .PhysicalAddress = nic.GetPhysicalAddress.ToString

                            .OperationalStatus = nic.OperationalStatus.ToString

                            .Speed = nic.Speed.ToString

                            .NetworkInterfaceType = nic.NetworkInterfaceType.ToString

                            _networkInterfaces.Add(interface_)

                        End With

                    End If

                End If

            Next

        End Sub

        Private Function GetIpNumberOfDescription(ByVal descriptionInterface_ As String) As String

            For Each nic As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()

                For Each ip As UnicastIPAddressInformation In nic.GetIPProperties().UnicastAddresses

                    If (nic.Description = descriptionInterface_) Then

                        If (ip.Address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork) Then

                            Return ip.Address.ToString()

                        End If

                    End If

                Next

            Next

            Return Nothing

        End Function

#End Region


    End Class

End Namespace

