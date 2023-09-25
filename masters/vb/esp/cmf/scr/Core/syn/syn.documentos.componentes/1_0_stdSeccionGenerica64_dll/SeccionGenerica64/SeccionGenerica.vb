Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class SeccionGenerica
        Inherits DecoradorSeccion

#Region "Attributes"

        Private _seccionGenerico As [Enum]
#End Region

#Region "Builders"
        Public Sub New(ByVal seccion_ As Seccion)

            MyBase.New(seccion_)

            '_seccionGenerico = SeccionesGenericas.SGS0

            TipoNodo = TiposNodo.Seccion

        End Sub
        Public Sub New()

            MyBase.New()

            '_seccionGenerico = SeccionesGenericas.SGS0

            TipoNodo = TiposNodo.Seccion

        End Sub

#End Region

#Region "Properties"

        '<BsonElement("SeccionGenerica")>
        '<BsonRepresentation(BsonType.String)>
        <BsonIgnore>
        Public Property SeccionGenerica As [Enum]

            Get

                Return _seccionGenerico

            End Get

            Set(value As [Enum])

                _seccionGenerico = value

                IDUnico = Convert.ToInt32(_seccionGenerico)

                Nombre = GetEnumDescription(DirectCast(_seccionGenerico, [Enum]))

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace