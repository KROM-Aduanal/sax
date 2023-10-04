Imports Wma.Exceptions

Public Class PermisoPartidaPedimento
    Inherits OperacionesPartidas

#Region "Atributos"

    Private _idPermiso As Integer

    Private _clave As String

    Private _numero As String

    Private _firmaDescargo As String

    Private _valorComercialDolares As Double

    Private _cantidadUMTC As Double

    Private _archivado As Boolean

    Private _estado As Integer

#End Region

#Region "Propiedades"

    Property IdPermiso As Integer

        Get

            Return _idPermiso

        End Get

        Set(value As Integer)

            _idPermiso = value

        End Set

    End Property

    Property Clave As String

        Get

            Return _clave

        End Get

        Set(value As String)

            _clave = value

        End Set

    End Property

    Property Numero As String

        Get

            Return _numero

        End Get

        Set(value As String)

            _numero = value

        End Set

    End Property

    Property FirmaDescargo As String

        Get

            Return _firmaDescargo

        End Get

        Set(value As String)

            _firmaDescargo = value

        End Set

    End Property

    Property ValorComercialDolares As Double

        Get

            Return _valorComercialDolares

        End Get

        Set(value As Double)

            _valorComercialDolares = value

        End Set

    End Property

    Property CantidadUMTC As Double

        Get

            Return _cantidadUMTC

        End Get

        Set(value As Double)

            _cantidadUMTC = value

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