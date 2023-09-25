
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.RecursosComercioExterior


Namespace Syn.Documento.Componentes

    <Serializable()>
    Public Class CampoPedimento
        Inherits DecoradorCampo

#Region "Attributes"

        Private _campoPedimento As CamposPedimento

        Protected ReadOnly _votoDeCampo As List(Of String) = New List(Of String)()

#End Region

#Region "Builders"
        Public Sub New(ByVal campo_ As Campo)

            MyBase.New(campo_)

            _campoPedimento = CamposPedimento.SIN_DEFINIR

            TipoNodo = TiposNodo.Campo

        End Sub

#End Region

#Region "Properties"

        '<BsonElement("CampoPedimento")>
        '<BsonRepresentation(BsonType.String)>
        '<NonSerialized>
        Public Property CampoPedimento As CamposPedimento

            Get

                Return _campoPedimento

            End Get

            Set(value As CamposPedimento)

                _campoPedimento = value

                IDUnico = Convert.ToInt32(_campoPedimento)

                Nombre = _campoPedimento.ToString

                'NombrePresentacion = GetEnumDescription(DirectCast(Convert.ToInt32(_campoPedimento), CamposPedimento))

            End Set

        End Property

#End Region

#Region "Methods"
        Public Sub VotacionCampo(ByVal name As String)
            _votoDeCampo.Add(name)
            _campo.IDUnico -= 1
        End Sub

        Public Sub RemueveVoto(ByVal name As String)
            _votoDeCampo.Remove(name)
            _campo.IDUnico += 1
        End Sub

        Public Overrides Sub Display()
            MyBase.Display()

            For Each voto_ As String In _votoDeCampo
                Console.WriteLine(" Votación: " & voto_)
            Next

        End Sub

#End Region

    End Class

End Namespace