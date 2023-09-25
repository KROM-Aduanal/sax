Imports gsol.krom

Public Class Peticiones

#Region "Atributos"

    Private _clausula As String

    Private _granularidad As IEnlaceDatos.TiposDimension

    Private _localizacion As IEnlaceDatos.TiposDimension

    Private _campoBusqueda As String

#End Region

#Region "Propiedades"

    Public Property Clausula As String

        Get

            Return _clausula

        End Get

        Set(value As String)

            _clausula = value

        End Set

    End Property

    Public Property Granularidad As IEnlaceDatos.TiposDimension

        Get

            Return _granularidad

        End Get

        Set(value As IEnlaceDatos.TiposDimension)

            _granularidad = value

        End Set

    End Property

    Public Property Localizacion As IEnlaceDatos.TiposDimension

        Get

            Return _localizacion

        End Get

        Set(value As IEnlaceDatos.TiposDimension)

            _localizacion = value

        End Set

    End Property

    Public Property CampoBusqueda As String

        Get

            Return _campoBusqueda

        End Get

        Set(value As String)

            _campoBusqueda = value

        End Set

    End Property

#End Region

End Class