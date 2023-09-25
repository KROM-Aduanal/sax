Public Class ComprobanteDocumentosRelacionados

#Region "Atributos"

    Private _idDocumento As String

    Private _serie As String

    Private _folio As String

    Private _monedaDR As String

    Private _tipoCambioDR As Decimal

    Private _metodoDePagoDR As String

    Private _numParcialidad As String

    Private _impSaldoAnt As Decimal

    Private _impPagado As Decimal

    Private _impSaldoInsoluto As Decimal

#End Region

#Region "Propiedades"

    Public Property idDocumento As String
        Get
            Return _idDocumento
        End Get
        Set(value As String)
            _idDocumento = value
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

    Public Property MonedaDR As String
        Get
            Return _monedaDR
        End Get
        Set(value As String)
            _monedaDR = value
        End Set
    End Property

    Public Property TipoCambioDR As Decimal
        Get
            Return _tipoCambioDR
        End Get
        Set(value As Decimal)
            _tipoCambioDR = value
        End Set
    End Property

    Public Property MetodoDePagoDR As String
        Get
            Return _metodoDePagoDR
        End Get
        Set(value As String)
            _metodoDePagoDR = value
        End Set
    End Property

    Public Property NumParcialidad As String
        Get
            Return _numParcialidad
        End Get
        Set(value As String)
            _numParcialidad = value
        End Set
    End Property

    Public Property SaldoAnterior As Decimal
        Get
            Return _impSaldoAnt
        End Get
        Set(value As Decimal)
            _impSaldoAnt = value
        End Set
    End Property

    Public Property ImportePagado As Decimal
        Get
            Return _impPagado
        End Get
        Set(value As Decimal)
            _impPagado = value
        End Set
    End Property

    Public Property SaldoInsoluto As Decimal
        Get
            Return _impSaldoInsoluto
        End Get
        Set(value As Decimal)
            _impSaldoInsoluto = value
        End Set
    End Property

#End Region

End Class

