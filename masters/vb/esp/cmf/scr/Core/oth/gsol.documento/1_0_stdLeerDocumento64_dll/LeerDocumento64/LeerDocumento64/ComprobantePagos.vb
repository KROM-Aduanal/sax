Public Class ComprobantePagos

#Region "Atributos"

    'Private _version As String

    Private _fechaPago As Date

    Private _formaPagoP As String

    Private _monedaP As String

    Private _tipoCambioP As Decimal

    Private _Monto As Decimal

    Private _numOperacion As String

    Private _rfcEmisorCtaOrd As String

    Private _nomBancoOrdExt As String

    Private _ctaOrdenante As String

    Private _rfcEmisorCtaBen As String

    Private _ctaBeneficiario As String



    Private _listaDocumentosRelacionados As List(Of ComprobanteDocumentosRelacionados)

#End Region

#Region "Propiedades"

    Public Property FechaPago As Date
        Get
            Return _fechaPago
        End Get
        Set(value As Date)
            _fechaPago = value
        End Set
    End Property

    Public Property FormaPagoP As String
        Get
            Return _formaPagoP
        End Get
        Set(value As String)
            _formaPagoP = value
        End Set
    End Property

    Public Property MonedaP As String
        Get
            Return _monedaP
        End Get
        Set(value As String)
            _monedaP = value
        End Set
    End Property

    Public Property TipoCambioP As Decimal
        Get
            Return _tipoCambioP
        End Get
        Set(value As Decimal)
            _tipoCambioP = value
        End Set
    End Property

    Public Property Monto As Decimal
        Get
            Return _Monto
        End Get
        Set(value As Decimal)
            _Monto = value
        End Set
    End Property

    Public Property NumOperacion As String
        Get
            Return _numOperacion
        End Get
        Set(value As String)
            _numOperacion = value
        End Set
    End Property

    Public Property RFCEmisorCtaOrd As String
        Get
            Return _rfcEmisorCtaOrd
        End Get
        Set(value As String)
            _rfcEmisorCtaOrd = value
        End Set
    End Property

    Public Property BancoOrdExt As String
        Get
            Return _nomBancoOrdExt
        End Get
        Set(value As String)
            _nomBancoOrdExt = value
        End Set
    End Property

    Public Property CuentaOrdenante As String
        Get
            Return _ctaOrdenante
        End Get
        Set(value As String)
            _ctaOrdenante = value
        End Set
    End Property

    Public Property RFCEmisorCtaBen As String
        Get
            Return _rfcEmisorCtaBen
        End Get
        Set(value As String)
            _rfcEmisorCtaBen = value
        End Set
    End Property

    Public Property CuentaBeneficiario As String
        Get
            Return _ctaBeneficiario
        End Get
        Set(value As String)
            _ctaBeneficiario = value
        End Set
    End Property

    Public Property ListaDocumentosRelacionados As List(Of ComprobanteDocumentosRelacionados)
        Get
            Return _listaDocumentosRelacionados
        End Get
        Set(value As List(Of ComprobanteDocumentosRelacionados))
            _listaDocumentosRelacionados = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Sub New()

        _listaDocumentosRelacionados = New List(Of ComprobanteDocumentosRelacionados)

    End Sub

#End Region

End Class

