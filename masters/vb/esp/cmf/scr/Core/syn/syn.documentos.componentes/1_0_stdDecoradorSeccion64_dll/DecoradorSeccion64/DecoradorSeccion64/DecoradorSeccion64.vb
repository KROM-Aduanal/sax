Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes


Namespace Syn.Documento.Componentes

    '<BsonKnownTypes(GetType(SeccionPedimento))>

    <BsonKnownTypes(GetType(SeccionGenerica))>
    <Serializable()>
    Public MustInherit Class DecoradorSeccion
        Inherits Seccion

#Region "Attributes"

        Protected _seccion As Seccion

#End Region

#Region "Builders"
        Public Sub New(ByVal seccion_ As Seccion)

            MyBase.New()

            Me._seccion = seccion_

        End Sub

        Public Sub New()

            MyBase.New()

        End Sub

#End Region

#Region "Methods"


#End Region

    End Class

End Namespace
