
Imports Parse

Namespace Wma.WebServices

    Public Class ParseNotification

#Region "Attributes"

        Private _applicationID As String

        Private _windowsKey As String

        Private _server As String

        Private _token As String

        Private _message As String

        Private _status As Boolean

        Private _reference As String

        Private _alert As String

#End Region

#Region "Builders"


        Sub New(ByVal reference_ As String, _
                ByVal message_ As String)

            _status = False

            _applicationID = "BrounieApp"

            _windowsKey = "C4suYZKkyRMYPGR7fEae"

            _server = "http://krom.brounieapps.com/parse/"

            Dim conf_ As ParseClient.Configuration

            conf_.ApplicationId = _applicationID

            conf_.WindowsKey = _windowsKey

            conf_.Server = _server

            ParseClient.Initialize(conf_)

            PushKromessage(reference_, message_)

        End Sub

#End Region

#Region "Methods"

        Public Async Sub PushKromessage(ByVal reference_ As String, _
                                        ByVal message_ As String)

            Try

                Dim push As ParseObject = New ParseObject("Notificacion")

                push("referencia") = reference_

                push("alert") = message_

                Await push.SaveAsync()

                _status = True

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

#End Region

#Region "Properties"

        WriteOnly Property SetReference As String

            Set(value As String)

                _reference = value

            End Set

        End Property

        WriteOnly Property SetAlert As String

            Set(value As String)

                _alert = value

            End Set

        End Property

        Property GetStatus As Boolean

            Get
                Return _status

            End Get

            Set(value As Boolean)

                _status = value

            End Set

        End Property

#End Region

    End Class

End Namespace