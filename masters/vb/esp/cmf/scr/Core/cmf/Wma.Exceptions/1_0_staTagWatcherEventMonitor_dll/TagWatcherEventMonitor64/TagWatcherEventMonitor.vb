Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization

Imports Wma.Exceptions.TagWatcherEventMonitor

Namespace Wma.Exceptions


    <DataContract()>
<XmlSerializerFormat>
Public Class TagWatcherEventMonitor

#Region "Enums"

        <DataContract()>
        Enum CMFLayers
            L00Undefined
            L01Core
            L02DBOperations
            L03BaseLines
            L04VisualComponents
            L05Development
            L06ExtraControllers
            L07DataLinks
        End Enum

#End Region

#Region "Attributes"

        'Instancia singleton
        Private Shared _instance As TagWatcherEventMonitor = Nothing

        'Monitor de sucesos TagWatcher
        Private _monitor As List(Of EventTagWatcher)

        Private _lastMessage As TagWatcher

        Private _quantityMessages As Int32 = 0

        Private _showmeOnErrors As Boolean

        Private _showmeDialogOnErrors As Boolean

        Private _showmeAllErrorsAtTheEnd As Boolean

#End Region

#Region "Builders"

        Sub New(Optional ByVal launchOnErrors_ As Boolean = False)

            'Monitor de sucesos TagWatcher
            _monitor = New List(Of EventTagWatcher)

            _lastMessage = New TagWatcher

            _quantityMessages = 0

            _showmeOnErrors = False

            _showmeAllErrorsAtTheEnd = False

            _showmeDialogOnErrors = launchOnErrors_

        End Sub

#End Region

#Region "Properties"

        WriteOnly Property ShowMeOnErrors As Boolean

            Set(value As Boolean)

                _showmeOnErrors = value

            End Set

        End Property

        WriteOnly Property ShowMeDialogBoxOnErrors As Boolean

            Set(value As Boolean)

                _showmeDialogOnErrors = value

            End Set

        End Property

        WriteOnly Property ShowMeAllErrorsAtTheEnd As Boolean

            Set(value As Boolean)

                _showmeAllErrorsAtTheEnd = value

            End Set

        End Property

        ReadOnly Property EventMonitor As List(Of EventTagWatcher)

            Get
                Return _monitor

            End Get

        End Property

        ReadOnly Property LastMessage As TagWatcher

            Get

                Return _lastMessage

            End Get

        End Property

        ReadOnly Property Count As Int32

            Get

                Return _quantityMessages

            End Get

        End Property

#End Region

#Region "Methods"

        Public Sub Add(ByVal status_ As Wma.Exceptions.TagWatcher.TypeStatus,
                       ByVal error_ As Wma.Exceptions.TagWatcher.ErrorTypes,
                       ByVal description_ As String,
                       ByVal idLayer_ As CMFLayers,
                       ByVal CMFResourceName_ As String)

            Dim tagWatcher_ As New TagWatcher

            With tagWatcher_
                .Status = status_
                .Errors = error_
                .ErrorDescription = CMFResourceName_ & ": " & description_
            End With

            Dim newMessage_ As New EventTagWatcher

            _quantityMessages += 1

            With newMessage_
                .DateTime = Now
                .IDEvent = _quantityMessages
                .IDLayer = idLayer_
                .TagWatcherObject = tagWatcher_
                .CMFResourceName = CMFResourceName_
                .Info = description_
            End With

            _monitor.Add(newMessage_)

            'Setting the lastone

            _lastMessage = tagWatcher_

            If _showmeOnErrors Then

                Console.Write(CMFResourceName_ & ": " & description_)
                'Console.Write(tagWatcher_.ErrorDescription)
                ' MsgBox(tagWatcher_.ErrorDescription)

            ElseIf _showmeDialogOnErrors Then

                Console.Write(CMFResourceName_ & ": " & description_)
                'Console.Write(tagWatcher_.ErrorDescription)
                ' MsgBox(tagWatcher_.ErrorDescription)

            End If

        End Sub
        Public Sub Add(ByVal status_ As Wma.Exceptions.TagWatcher.TypeStatus,
                       ByVal resultsCont_ As Int32,
                       ByVal error_ As Wma.Exceptions.TagWatcher.ErrorTypes,
                       ByVal description_ As String,
                       Optional ByVal idLayer_ As CMFLayers = CMFLayers.L00Undefined,
                       Optional ByVal CMFResource_ As IRecursosSistemas.RecursosCMF = IRecursosSistemas.RecursosCMF.SinDefinir)

            Dim tagWatcher_ As New TagWatcher

            With tagWatcher_
                .Status = status_
                .ResultsCount = resultsCont_
                .Errors = error_
                .ErrorDescription = description_
            End With

            Dim newMessage_ As New EventTagWatcher

            _quantityMessages += 1

            With newMessage_
                .DateTime = Now
                .IDEvent = _quantityMessages
                .IDLayer = idLayer_
                .TagWatcherObject = tagWatcher_
                .CMFResource = CMFResource_
                .CMFResourceName = CMFResource_.ToString
            End With

            _monitor.Add(newMessage_)

            'Setting the lastone

            _lastMessage = tagWatcher_

            If _showmeOnErrors Then

                Console.Write(tagWatcher_.ErrorDescription)
                MsgBox(tagWatcher_.ErrorDescription)

            ElseIf _showmeDialogOnErrors Then

                Console.Write(tagWatcher_.ErrorDescription) '
                MsgBox(tagWatcher_.ErrorDescription)

            End If

        End Sub

        Public Sub Add(ByVal tagWatcher_ As TagWatcher,
                       Optional ByVal idLayer_ As CMFLayers = CMFLayers.L00Undefined)

            Dim newMessage_ As New EventTagWatcher

            _quantityMessages += 1

            With newMessage_
                .DateTime = Now
                .IDEvent = _quantityMessages
                .IDLayer = idLayer_
                .TagWatcherObject = tagWatcher_
            End With

            _monitor.Add(newMessage_)

        End Sub

        Public Sub AddEventTagWatcher(ByVal newMessage_ As EventTagWatcher)

            _monitor.Add(newMessage_)

        End Sub

        Public Shared Function ObtainInstance(Optional ByVal launchOnErrors_ As Boolean = False) As TagWatcherEventMonitor

            If _instance Is Nothing Then

                _instance = New TagWatcherEventMonitor(launchOnErrors_)

            End If

            Return _instance

        End Function

#End Region

    End Class

    Public Class EventTagWatcher

#Region "Attributes"

        Private _idEvent As Int32

        Private _tagWatcherObject As TagWatcher

        Private _CMFResource As IRecursosSistemas.RecursosCMF

        Private _CMFResourceName As String

        Private _idLayer As CMFLayers

        Private _dateTime As DateTime

        Private _info As String

#End Region

#Region "Builders"

        Sub New()

            _idEvent = 0

            _tagWatcherObject = New TagWatcher

            _CMFResource = IRecursosSistemas.RecursosCMF.SinDefinir

            _CMFResourceName = Nothing

            _idLayer = CMFLayers.L00Undefined

            _dateTime = Now

            _info = Nothing

        End Sub

#End Region

#Region "Properties"

        Property IDEvent As Int32

            Get

                Return _idEvent

            End Get

            Set(ByVal value As Int32)

                _idEvent = value

            End Set

        End Property

        Property TagWatcherObject As TagWatcher

            Get

                Return _tagWatcherObject

            End Get

            Set(ByVal value As TagWatcher)

                _tagWatcherObject = value

            End Set

        End Property

        Property CMFResource As IRecursosSistemas.RecursosCMF

            Get

                Return _CMFResource

            End Get

            Set(ByVal value As IRecursosSistemas.RecursosCMF)

                _CMFResource = value

            End Set

        End Property

        Property IDLayer As CMFLayers

            Get

                Return _idLayer

            End Get

            Set(ByVal value As CMFLayers)

                _idLayer = value

            End Set

        End Property

        Property DateTime As DateTime

            Get

                Return _dateTime

            End Get

            Set(ByVal value As DateTime)

                _dateTime = value

            End Set

        End Property

        Public Property CMFResourceName As String
            Get
                Return _CMFResourceName
            End Get
            Set(value As String)
                _CMFResourceName = value
            End Set
        End Property

        Public Property Info As String
            Get
                Return _info
            End Get
            Set(value As String)
                _info = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace

