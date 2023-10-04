Imports Wma.Exceptions

Public Class IdentificadorPartidaPedimento
    Inherits OperacionesPartidas

#Region "Atributos"

    Private _idIdentificador As Integer

    Private _identificador As String

    Private _complemento1 As String

    Private _complemento2 As String

    Private _complemento3 As String

    Private _archivado As Boolean

    Private _estado As Integer

#End Region

#Region "Propiedades"

    Property IdIdentificador As Integer

        Get

            Return _idIdentificador

        End Get

        Set(value As Integer)

            _idIdentificador = value

        End Set

    End Property

    Property Identificador As String

        Get

            Return _identificador

        End Get

        Set(value As String)

            _identificador = value

        End Set

    End Property

    Property Complemento1 As String

        Get

            Return _complemento1

        End Get

        Set(value As String)

            _complemento1 = value

        End Set

    End Property

    Property Complemento2 As String

        Get

            Return _complemento2

        End Get

        Set(value As String)

            _complemento2 = value

        End Set

    End Property

    Property Complemento3 As String

        Get

            Return _complemento3

        End Get

        Set(value As String)

            _complemento3 = value

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