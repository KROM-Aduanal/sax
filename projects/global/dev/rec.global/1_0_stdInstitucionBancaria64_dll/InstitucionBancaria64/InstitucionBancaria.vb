
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

    'Public Enum TiposNombreComercial
    '    SinDefinir = 0
    '    Actual = 1
    '    Anterior = 2
    '    Ingles = 3
    '    Abreviatura = 4
    'End Enum
#End Region

#Region "Propiedades"
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property idempresa As ObjectId
    <BsonIgnoreIfNull>
    Public Property idinstitucionbancaria As Int32
    <BsonIgnoreIfNull>
    Public Property identificadorbanco As String
    <BsonIgnoreIfNull>
    Public Property clavesat As String
    <BsonIgnoreIfNull>
    Public Property otrosaliasinstitucion As List(Of AliasBancos)
    <BsonIgnoreIfNull>
    Public Property razonsocialespaniol As String
    <BsonIgnoreIfNull>
    Public Property iddomicilio As ObjectId
    <BsonIgnoreIfNull>
    Public Property domiciliofiscal As String
    <BsonIgnoreIfNull>
    Public Property tipobanco As Int16
    Public Property estado As Int16
    Public Property archivado As Boolean


    Function obtenervalor() As AliasBancos
        Return otrosaliasinstitucion.Item(4)
    End Function

#End Region

#Region "Constructor"
    Public Sub New()

    End Sub
    Public Sub New(ByVal identificadorBanco_ As String,
                   ByVal otrosAliasInstitucion_ As List(Of AliasBancos),
                   ByVal razonSocial_ As String,
                   ByVal domicilioFiscal_ As String,
                   ByVal tipoBanco_ As TiposBanco)

        identificadorbanco = identificadorBanco_
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

        idinstitucionbancaria = 1

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                idinstitucionbancaria = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        _id = ObjectId.GenerateNewId

        estado = 1


    End Sub
#End Region

End Class


Public Class AliasBancos
    <BsonIgnoreIfNull>
    Property TipoAlias As String

    <BsonIgnoreIfNull>
    Property Valor As String


End Class
