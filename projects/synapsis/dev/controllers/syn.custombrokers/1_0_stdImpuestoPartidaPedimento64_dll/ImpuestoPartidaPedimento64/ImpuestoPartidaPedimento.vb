Imports Wma.Exceptions

Public Class ImpuestoPartidaPedimento
    Inherits OperacionesPartidas

#Region "Atributos"

    Private _idImpuesto As Integer

    Private _contribucion As Integer

    Private _tasa As Double

    Private _tipoTasa As Integer

    Private _formaPago As Integer

    Private _importe As Double

    Private _archivado As Boolean

    Private _estado As Integer

#End Region

#Region "Propiedades"

    Property IdImpuesto As Integer

        Get

            Return _idImpuesto

        End Get

        Set(value As Integer)

            _idImpuesto = value

        End Set

    End Property

    Property Contribucion As Integer

        Get

            Return _contribucion

        End Get

        Set(value As Integer)

            _contribucion = value

        End Set

    End Property

    Property Tasa As Double

        Get

            Return _tasa

        End Get

        Set(value As Double)

            _tasa = value

        End Set

    End Property

    Property TipoTasa As Integer

        Get

            Return _tipoTasa

        End Get

        Set(value As Integer)

            _tipoTasa = value

        End Set

    End Property

    Property FormaPago As Integer

        Get

            Return _formaPago

        End Get

        Set(value As Integer)

            _formaPago = value

        End Set

    End Property

    Property Importe As Double

        Get

            Return _importe

        End Get

        Set(value As Double)

            _importe = value

        End Set

    End Property

    Property Archivado As Boolean

        Get

            Return _archivado

        End Get

        Set(value As Boolean)

            _archivado = value

        End Set

    End Property

    Property Estado As Integer

        Get

            Return _estado

        End Get

        Set(value As Integer)

            _estado = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Overrides Function Agregar() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Overrides Function Actualizar() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Overrides Function Eliminar() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Overrides Function Archivar() As TagWatcher

        Throw New NotImplementedException()

    End Function

#End Region

End Class