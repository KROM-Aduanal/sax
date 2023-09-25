Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Wma.Exceptions

Namespace Wma.WebServices

    <DataContract()>
    <XmlSerializerFormat>
    Public Class UserProfile

#Region "Attributes"

        Public Enum SessionTypes
            Undefined
            Standard
            Platino
            Golden
        End Enum

        Private _nombre As String

        Private _apellidoPaterno As String

        Private _apellidoMaterno As String

        Private _fechaRegistro As Date

        Private _result As TagWatcher

        Private _nombreEmpresa As String

        Private _telefono As String

        Private _fechaNacimiento As String

        Private _correoElectronico As String

        Private _urlFotografia As String

        Private _tipoSesion As SessionTypes

#End Region

#Region "Builders"


        Sub New()

            _result = Nothing

            _nombre = Nothing

            _apellidoPaterno = Nothing

            _apellidoMaterno = Nothing

            _fechaRegistro = Nothing

            _tipoSesion = SessionTypes.Undefined

            _result = New TagWatcher

            _nombreEmpresa = Nothing

            _telefono = Nothing

            _fechaNacimiento = Nothing

            _correoElectronico = Nothing

        End Sub

#End Region

#Region "Properties"


        <DataMember(Name:="TagWatcher")> _
        Public Property Result As TagWatcher

            Set(value As TagWatcher)

                _result = value

            End Set

            Get
                Return _result

            End Get

        End Property

        <DataMember()>
        Public Property Name As String
            Set(value As String)
                _nombre = value
            End Set
            Get
                Return _nombre
            End Get
        End Property

        <DataMember()>
        Public Property PLastName As String
            Set(value As String)
                _apellidoMaterno = value
            End Set
            Get
                Return _apellidoMaterno
            End Get
        End Property

        <DataMember()>
        Public Property MLastName As String
            Set(value As String)
                _apellidoPaterno = value
            End Set
            Get
                Return _apellidoPaterno
            End Get

        End Property

        <DataMember()>
        Public Property RegisterDate As Date

            Set(value As Date)
                _fechaRegistro = value
            End Set
            Get
                Return _fechaRegistro
            End Get

        End Property


        <DataMember()>
        Public Property CompanyName As String

            Set(value As String)
                _nombreEmpresa = value
            End Set
            Get
                Return _nombreEmpresa
            End Get

        End Property

        <DataMember()>
        Public Property Phone As String

            Set(value As String)
                _telefono = value
            End Set
            Get
                Return _telefono
            End Get

        End Property

        <DataMember()>
        Public Property BirthDate As String

            Set(value As String)
                _fechaNacimiento = value
            End Set
            Get
                Return _fechaNacimiento
            End Get

        End Property

        <DataMember()>
        Public Property EMail As String

            Set(value As String)

                _correoElectronico = value

            End Set

            Get

                Return _correoElectronico

            End Get

        End Property


        <DataMember()>
        Public Property URLUserPicture As String

            Set(value As String)

                _urlFotografia = value

            End Set

            Get

                Return _urlFotografia

            End Get

        End Property


        <DataMember()>
        Public Property SessionType As SessionTypes

            Set(value As SessionTypes)

                _tipoSesion = value

            End Set

            Get

                Return _tipoSesion

            End Get

        End Property



#End Region


#Region "Methods"



#End Region

    End Class


End Namespace

'Imports System.ServiceModel
'Imports System.Runtime.Serialization

'Namespace Wma.WebServices

'    <ServiceContract(Namespace:="Wma.WebServices")>
'    Public Class UserProfile

'#Region "Attributes"

'        Public Enum SessionTypes
'            Standard
'            Platino
'            Golden
'        End Enum

'        Private _nombre As String

'        Private _apellidoPaterno As String

'        Private _apellidoMaterno As String

'        Private _fechaRegistro As Date

'        Private _tipoSesion As SessionTypes

'        Private _responseStatus As String

'#End Region

'#Region "Properties"

'        'setters

'        WriteOnly Property SetNombre As String
'            Set(value As String)

'                _nombre = value

'            End Set
'        End Property

'        WriteOnly Property SetApellidoMaterno As String
'            Set(value As String)
'                _apellidoMaterno = value

'            End Set
'        End Property

'        WriteOnly Property SetApellidoParterno As String
'            Set(value As String)

'                _apellidoPaterno = value

'            End Set

'        End Property

'        WriteOnly Property SetFechaRegistro As Date
'            Set(value As Date)

'                _fechaRegistro = value

'            End Set

'        End Property

'        WriteOnly Property SetResponseStatus As String
'            Set(value As String)

'                _responseStatus = value

'            End Set
'        End Property
'        'getters

'        '[DataContract]
'        'Public Class Resultado
'        '{
'        '<DataContract()>
'        Public ReadOnly Property ResponseStatus As String

'            Get
'                Return _responseStatus

'            End Get

'        End Property

'        Public ReadOnly Property Nombre As String
'            Get
'                Return _nombre
'            End Get
'        End Property

'        Public ReadOnly Property ApellidoMaterno As String
'            Get
'                Return _apellidoMaterno
'            End Get
'        End Property

'        Public ReadOnly Property ApellidoParterno As String
'            Get

'                Return _apellidoPaterno
'            End Get

'        End Property

'        Public ReadOnly Property FechaRegistro As Date
'            Get

'                Return _fechaRegistro
'            End Get

'        End Property

'#End Region


'#Region "Methods"

'        Sub New()

'            _responseStatus = Nothing

'            _nombre = Nothing

'            _apellidoPaterno = Nothing

'            _apellidoMaterno = Nothing

'            _fechaRegistro = Now

'            _tipoSesion = SessionTypes.Standard

'            _responseStatus = Nothing

'        End Sub
'#End Region

'    End Class

'End Namespace