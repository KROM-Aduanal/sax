Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class PartidaPedimento
        Inherits DecoradorPartida

#Region "Attributes"


#End Region

#Region "Builders"
        Public Sub New(ByVal partida_ As Partida)

            MyBase.New(partida_)

            TipoNodo = TiposNodo.Partida

        End Sub
        Public Sub New()

            MyBase.New()

            TipoNodo = TiposNodo.Partida

        End Sub

#End Region

#Region "Properties"

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace