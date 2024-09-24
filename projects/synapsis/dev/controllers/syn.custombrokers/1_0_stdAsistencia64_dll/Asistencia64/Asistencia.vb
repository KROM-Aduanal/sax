Imports Wma.Exceptions

Public Class Asistencia

#Region "Enums"

    Enum TiposAsistencia

        SinDefinir = 0
        Acotacion = 1
        Advertencia = 2
        ValorFijo = 3
        Sugerencia = 4

    End Enum

#End Region

#Region "Atributos"

    Private _tiposAsistencia As TiposAsistencia

    Private _camposAfectados As Dictionary(Of String, Array)

    Private _estado As Integer

#End Region

#Region "Propiedades"

    Public Property TipoAsistencia As TiposAsistencia

        Get

            Return _tiposAsistencia

        End Get

        Set(value As TiposAsistencia)

            _tiposAsistencia = value

        End Set

    End Property

    Public Property CamposAfectados As Dictionary(Of String, Array)

        Get

            Return _camposAfectados

        End Get

        Set(value As Dictionary(Of String, Array))

            _camposAfectados = value

        End Set

    End Property

    Public Property Estado As Integer

        Get

            Return _estado

        End Get

        Set(value As Integer)

            _estado = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Sub Add()

    End Sub

    Public Sub Update()

    End Sub

#End Region

End Class