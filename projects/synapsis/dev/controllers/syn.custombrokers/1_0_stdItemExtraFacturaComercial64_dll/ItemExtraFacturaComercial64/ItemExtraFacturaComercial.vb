Imports Wma.Exceptions

Public Class ItemExtraFacturaComercial
    Implements IItemExtraFacturaComercial

#Region "Atributos"

    Private _codigoProducto As String

    Private _secuenciaProducto As Integer

    Private _marca As String

    Private _modelo As String

    Private _serie As String

    Private _submodelo As String

    Private _kilometraje As Integer

    Private _estado As Integer

#End Region

#Region "Propiedades"

    Public Property CodigoProducto As String Implements IItemExtraFacturaComercial.CodigoProducto

        Get

            Return _codigoProducto

        End Get

        Set(value As String)

            _codigoProducto = value

        End Set

    End Property

    Public Property SecuenciaProducto As Integer Implements IItemExtraFacturaComercial.SecuenciaProducto

        Get

            Return _secuenciaProducto

        End Get

        Set(value As Integer)

            _secuenciaProducto = value

        End Set

    End Property

    Public Property Marca As String Implements IItemExtraFacturaComercial.Marca

        Get

            Return _marca

        End Get

        Set(value As String)

            _marca = value

        End Set

    End Property

    Public Property Modelo As String Implements IItemExtraFacturaComercial.Modelo

        Get

            Return _modelo

        End Get

        Set(value As String)

            _modelo = value

        End Set

    End Property

    Public Property Serie As String Implements IItemExtraFacturaComercial.Serie

        Get

            Return _serie

        End Get

        Set(value As String)

            _serie = value

        End Set

    End Property

    Public Property Submodelo As String Implements IItemExtraFacturaComercial.Submodelo

        Get

            Return _submodelo

        End Get

        Set(value As String)

            _submodelo = value

        End Set

    End Property

    Public Property Kilometraje As Integer Implements IItemExtraFacturaComercial.Kilometraje

        Get

            Return _kilometraje

        End Get

        Set(ByVal value As Integer)

            _kilometraje = value

        End Set

    End Property

    Public Property Estado As Integer Implements IItemExtraFacturaComercial.Estado

        Get

            Return _estado

        End Get

        Set(value As Integer)

            _estado = value

        End Set

    End Property

    Private Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

#End Region

#Region "Metodos"

    Private Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class