
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

<Serializable()>
Public Class InstitucionBancaria

#Region "Enum"
    Public Enum TiposBanco
        Nacional = 1
        Extranjero = 2
    End Enum

#End Region

#Region "Propiedades"
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property _idempresa As ObjectId
    <BsonIgnoreIfNull>
    Public Property _idinstitucionbancaria As Int32
    <BsonIgnoreIfNull>
    Public Property uso As List(Of UsoIdentificador)
    <BsonIgnoreIfNull>
    Public Property otrosaliasinstitucion As List(Of AliasBancos)
    <BsonIgnoreIfNull>
    Public Property razonsocialespaniol As String
    <BsonIgnoreIfNull>
    Public Property _iddomicilio As ObjectId
    <BsonIgnoreIfNull>
    Public Property domiciliofiscal As String
    <BsonIgnoreIfNull>
    Public Property metadatos As List(Of AliasBancos)
    <BsonIgnoreIfNull>
    Public Property tipobanco As Int16
    <BsonIgnoreIfNull>
    Public Property estado As Int16
    Public Property archivado As Boolean


    'Function obtenervalor() As AliasBancos
    '    Return otrosaliasinstitucion.Item(4)
    'End Function

#End Region

#Region "Constructor"
    Public Sub New()

    End Sub
    Public Sub New(ByVal uso_ As List(Of UsoIdentificador),
                   ByVal otrosAliasInstitucion_ As List(Of AliasBancos),
                   ByVal razonSocial_ As String,
                   ByVal domicilioFiscal_ As String,
                   ByVal tipoBanco_ As TiposBanco,
                   ByVal metaDatos_ As List(Of AliasBancos))

        uso = uso_

        otrosaliasinstitucion = otrosAliasInstitucion_

        razonsocialespaniol = razonSocial_

        domiciliofiscal = domicilioFiscal_

        tipobanco = tipoBanco_


        Dim secuencia_ As New Secuencia _
                  With {.anio = 0,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "InstitucionesBancarias",
                        .tiposecuencia = 1,
                        .subtiposecuencia = 0
                        }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        _idinstitucionbancaria = 1

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                _idinstitucionbancaria = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        metadatos = metaDatos_

        _id = ObjectId.GenerateNewId

        estado = 1


    End Sub
#End Region

End Class


Public Class AliasBancos

    <BsonIgnoreIfNull>
    Property tipoalias As String

    <BsonIgnoreIfNull>
    Property valor As String

End Class

Public Class UsoIdentificador

    <BsonIgnoreIfNull>
    Property clave As String

    <BsonIgnoreIfNull>
    Property info As String

End Class
