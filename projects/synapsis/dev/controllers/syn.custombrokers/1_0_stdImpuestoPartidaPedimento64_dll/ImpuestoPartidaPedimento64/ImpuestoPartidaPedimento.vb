Imports Wma.Exceptions

Public Class ImpuestoPartidaPedimento
    Inherits OperacionesPartidas

#Region "Atributos"

    Private _idImpuesto As Integer

    Private _contribucion As Integer

    Private _descripcionContribucion As String

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

    Property DescripcionContribucion As String

        Get

            Return _descripcionContribucion

        End Get

        Set(value As String)

            _descripcionContribucion = value

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

    Public Overloads Function Agregar(ByVal impuestoNuevo_ As ImpuestoPartidaPedimento) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If impuestoNuevo_ IsNot Nothing Then

            'Dim partida_ = New ImpuestoPartidaPedimento With {
            '            .IdImpuesto = 1,
            '            .Contribucion = impuestoNuevo_.FraccionArancelaria,
            '            .DescripcionContribucion = impuestoNuevo_.DescripcionFraccionArancelaria,
            '            .Tasa = impuestoNuevo_.Nico,
            '            .TipoTasa = impuestoNuevo_.DescripcionNico,
            '            .FormaPago = impuestoNuevo_.PrecioUnitario,
            '            .Importe = impuestoNuevo_.PaisOrigen,
            '            .Archivado = impuestoNuevo_.DescripcionPaisOrigen,
            '            .Estado = impuestoNuevo_.PaisDestino
            '        }

            tagwatcher_.ObjectReturned = impuestoNuevo_
            tagwatcher_.SetOK()

        Else

            tagwatcher_.SetError(Me, "No se tienen la información correcta, verificar.")

        End If

        Return tagwatcher_

    End Function

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