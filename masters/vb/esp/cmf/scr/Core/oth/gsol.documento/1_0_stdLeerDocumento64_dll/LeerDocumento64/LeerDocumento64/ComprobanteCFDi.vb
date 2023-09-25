Public Class ComprobanteCFDi

#Region "Atributos"

    Private _claveDocumento As Integer

    Private _uuid As String

    Private _version As String

    Private _serie As String

    Private _folio As String

    Private _fecha As Date

    'Private FormaPago As String

    'Private SubTotal As Decimal

    'Private Moneda As c_Moneda

    Private _tipoCambio As Decimal

    'Private Total As Decimal

    Private _tipoDeComprobante As String

    'Private MetodoPago As c_MetodoPago

    Private _lugarExpedicion As c_CodigoPostal

    Private _RFCEmisor As String

    Private _RFCReceptor As String

    Private _listaPagos As List(Of ComprobantePagos)

#End Region

#Region "Propiedades"

    Public Property claveDocumento As Integer
        Get
            Return _claveDocumento
        End Get
        Set(value As Integer)
            _claveDocumento = value
        End Set
    End Property

    Public Property UUID As String
        Get
            Return _uuid
        End Get
        Set(value As String)
            _uuid = value
        End Set
    End Property

    Public Property Version As String
        Get
            Return _version
        End Get
        Set(value As String)
            _version = value
        End Set
    End Property

    Public Property Serie As String
        Get
            Return _serie
        End Get
        Set(value As String)
            _serie = value
        End Set
    End Property

    Public Property Folio As String
        Get
            Return _folio
        End Get
        Set(value As String)
            _folio = value
        End Set
    End Property

    Public Property Fecha As Date
        Get
            Return _fecha
        End Get
        Set(value As Date)
            _fecha = value
        End Set
    End Property

    'Private FormaPago As String

    'Private SubTotal As Decimal

    'Private Moneda As c_Moneda

    Public Property TipoCambio As Decimal
        Get
            Return _tipoCambio
        End Get
        Set(value As Decimal)
            _tipoCambio = value
        End Set
    End Property

    'Private Total As Decimal

    Public Property TipoDeComprobante As String
        Get
            Return _tipoDeComprobante
        End Get
        Set(value As String)
            _tipoDeComprobante = value
        End Set
    End Property

    'Private MetodoPago As c_MetodoPago

    Public Property LugarExpedicion As c_CodigoPostal
        Get
            Return _lugarExpedicion
        End Get
        Set(value As c_CodigoPostal)
            _lugarExpedicion = value
        End Set
    End Property

    Public Property RFCEmisor As String
        Get
            Return _RFCEmisor
        End Get
        Set(value As String)
            _RFCEmisor = value
        End Set
    End Property

    Public Property RFCReceptor As String
        Get
            Return _RFCReceptor
        End Get
        Set(value As String)
            _RFCReceptor = value
        End Set
    End Property

    Public Property ListaPagos As List(Of ComprobantePagos)
        Get
            Return _listaPagos
        End Get
        Set(value As List(Of ComprobantePagos))
            _listaPagos = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Sub New()

        _listaPagos = New List(Of ComprobantePagos)

    End Sub

#End Region

End Class

