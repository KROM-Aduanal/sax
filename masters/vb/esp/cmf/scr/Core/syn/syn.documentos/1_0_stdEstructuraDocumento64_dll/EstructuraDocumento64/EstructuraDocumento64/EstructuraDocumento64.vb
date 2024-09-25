
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos

Namespace Syn.Documento

    <Serializable()>
    Public Class EstructuraDocumento

#Region "Atributos"

        Private _tipo As String

        Private _parts As Dictionary(Of String, List(Of Nodo)) = New Dictionary(Of String, List(Of Nodo))()

#End Region

#Region "Builders"
        Public Sub New(ByVal tipo_ As String)

            Me._tipo = tipo_

        End Sub

#End Region

#Region "Properties"
        Default Public Property Item(ByVal key As TiposBloque) As List(Of Nodo)

            Get

                Return _parts(key.ToString)

            End Get

            Set(value As List(Of Nodo))

                _parts(key.ToString) = value

            End Set

        End Property

        Public Property Parts As Dictionary(Of String, List(Of Nodo))

            Get

                Return _parts

            End Get

            Set(value As Dictionary(Of String, List(Of Nodo)))

                _parts = value

            End Set

        End Property

#End Region

#Region "Methods"
        Public Sub Show()

        End Sub

#End Region


    End Class

End Namespace