Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class SeccionPedimento
        Inherits DecoradorSeccion

#Region "Attributes"

        Private _seccionComercioExterior As SeccionesPedimento
#End Region

#Region "Builders"
        Public Sub New(ByVal seccion_ As Seccion)

            MyBase.New(seccion_)

            _seccionComercioExterior = SeccionesPedimento.UNS00

            TipoNodo = TiposNodo.Seccion

        End Sub
        Public Sub New()

            MyBase.New()

            _seccionComercioExterior = SeccionesPedimento.UNS00

            TipoNodo = TiposNodo.Seccion

        End Sub

#End Region

#Region "Properties"

        <BsonElement("SeccionPedimento")>
        <BsonRepresentation(BsonType.String)>
        Public Property SeccionPedimento As SeccionesPedimento

            Get

                Return _seccionComercioExterior

            End Get

            Set(value As SeccionesPedimento)

                _seccionComercioExterior = value

                IDUnico = Convert.ToInt32(_seccionComercioExterior)

                Nombre = GetEnumDescription(DirectCast(Convert.ToInt32(_seccionComercioExterior), Nucleo.RecursosComercioExterior.SeccionesPedimento))

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace