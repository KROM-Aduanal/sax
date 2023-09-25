Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

<Serializable()>
Public Class MonedaGlobal

#Region "Propiedades"
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property idmoneda As Int32?
    <BsonIgnoreIfNull>
    Public Property nombremonedaesp As String
    <BsonIgnoreIfNull>
    Public nombremonedaing As String
    <BsonIgnoreIfNull>
    Public Property aliasmoneda As List(Of ClavesMoneda)
    <BsonIgnoreIfNull>
    Public Property factoresmoneda As List(Of FactordeMoneda)
    <BsonIgnoreIfNull>
    Public Property tiposdecambio As List(Of TipodeCambio)
    <BsonIgnoreIfNull>
    Public Property simbolos As List(Of String)
    Public Property aliasdesuso As List(Of ClavesMoneda)
    Public Property archivado As Boolean
    Public Property estado As Int16

#End Region

#Region "Constructor"
    Public Sub New()

    End Sub
    Public Sub New(ByVal idmoneda_ As Integer, ByVal nombremnonedaesp_ As String,
                   ByVal nombremonedaing_ As String,
                   ByVal aliasmoneda_ As List(Of ClavesMoneda),
                   ByVal factoresmomeda_ As List(Of FactordeMoneda),
                   ByVal simbolos_ As List(Of String),
                   Optional ByVal aliasdesuso_ As List(Of ClavesMoneda) = Nothing)
        idmoneda = idmoneda_
        nombremonedaesp = nombremnonedaesp_
        nombremonedaing = nombremonedaing_
        aliasmoneda = aliasmoneda_
        factoresmoneda = factoresmomeda_
        simbolos = simbolos_
        aliasdesuso = aliasdesuso_

        Dim secuencia_ As New Secuencia _
                  With {.anio = 0,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "Monedas",
                        .tiposecuencia = 1,
                        .subtiposecuencia = 0
                        }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        _id = ObjectId.GenerateNewId
        estado = 1


    End Sub
#End Region





End Class

Public Class ClavesMoneda
    <BsonIgnoreIfNull>
    Property Clave As String

    <BsonIgnoreIfNull>
    Property Valor As String

End Class

Public Class factorreciente
    Property idfactor As ObjectId
    Property serie As Int32
    Property institucion As Int32
    Property factor As Double
    Property cambio As DateTime

End Class

Public Class tipodecambioreciente
    Property idtipodecambio As ObjectId
    Property serie As Int32
    Property institucion As Int32
    Property tipocambio As Double
    Property cambio As DateTime

End Class


Public Class FactordeMoneda
    Property iddivisafactor As ObjectId
    Property iddivisa As Int32
    Property idunico As Int32
    Property divisafactor As String
    Property valordefault As Double
    Property institucion As Int32
    Property fecha As DateTime
    Property recientes As List(Of factorreciente)
    Property archivado As Boolean
    Property idhistorico As ObjectId
End Class

Public Class TipodeCambio
    Property iddivisafactor As ObjectId
    Property iddivisa As Int32
    Property idunico As Int32
    Property divisafactor As String
    Property valordefault As Double
    Property institucion As Int32
    Property fecha As DateTime
    Property recientes As List(Of tipodecambioreciente)
    Public Property archivado As Boolean
    Property idhistorico As ObjectId
End Class


Public Class FactorMonedaPrincipal
    Public Property Id As ObjectId
    Public aliasprincipal As String
    Public Presentacion As String
    Public divisafactor As String
    Public valorfactor As Double
    Public Fecha As Date

End Class
