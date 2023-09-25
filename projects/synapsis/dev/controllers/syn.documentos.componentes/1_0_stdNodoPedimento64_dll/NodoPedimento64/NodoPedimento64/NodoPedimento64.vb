Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Bson.Serialization.Options
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento.Componentes
    <Serializable()>
    Public Class NodoPedimento
        Inherits DecoradorNodo

#Region "Attributes"


#End Region

#Region "Builders"

        Public Sub New(ByVal nodo_ As Nodo)

            MyBase.New(nodo_)

            TipoNodo = TiposNodo.Nodo

        End Sub

        Public Sub New()

            TipoNodo = TiposNodo.Nodo

        End Sub

#End Region

#Region "Properties"



#End Region

    End Class

End Namespace