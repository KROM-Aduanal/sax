
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class CampoGenerico
        Inherits DecoradorCampo

#Region "Attributes"

        Private _campoGenerico As [Enum] '[Enum] 'CamposPedimento

        Protected ReadOnly _votoDeCampo As List(Of String) = New List(Of String)()

#End Region

#Region "Builders"
        Public Sub New(ByVal campo_ As Campo)

            MyBase.New(campo_)

            '_campoGenerico = 0 'CamposPedimento.SIN_DEFINIR

            TipoNodo = TiposNodo.Campo

        End Sub

#End Region

#Region "Properties"

        '<BsonElement("CampoGenerico")>
        '<BsonRepresentation(BsonType.String)>

        '<BsonElement("CampoGenerico")>
        '<BsonRepresentation(BsonType.String)>

        <BsonIgnore>
        Public Property CampoGenerico As [Enum] 'CamposReferencia 'Object ' CamposReferencia ' [Enum]


            Get

                Return _campoGenerico

            End Get

            Set(value As [Enum]) 'CamposReferencia) 'Object) 'CamposReferencia) '[Enum])

                _campoGenerico = value

                IDUnico = Convert.ToInt32(_campoGenerico)

                Nombre = _campoGenerico.ToString

                ' NombrePresentacion = _campoGenerico.ToString 'GetEnumDescription(DirectCast(Convert.ToInt32(_campoGenerico), CamposPedimento))
                ' NombrePresentacion = GetEnumDescription(DirectCast(Convert.ToInt32(_campoGenerico), [Enum]))

            End Set

        End Property

#End Region

    End Class

End Namespace