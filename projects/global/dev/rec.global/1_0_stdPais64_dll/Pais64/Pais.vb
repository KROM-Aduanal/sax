Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

Public Class Pais

    Public Sub New()

    End Sub

    Public Sub New(ByVal t_claveComercioMX As String,
                   ByVal t_cveISOnum As Int32,
                   ByVal t_cveISO2 As String,
                   ByVal t_cveISO3 As String,
                   ByVal t_nombrepaisesp As String,
                   ByVal t_nombrepaising As String,
                   ByVal t_nombrepaiscortoesp As String,
                   ByVal t_nombrepaiscortoing As String,
                   ByVal monedas As moneda)

        Dim secuencia_ As New Secuencia _
                    With {.anio = 0,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "Paises",
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
        _idPais = sec_
        cvecomercioMX = t_claveComercioMX
        cveISOnum = t_cveISOnum
        cveISO2 = t_cveISO2
        cveISO3 = t_cveISO3
        nombrepaisesp = t_nombrepaisesp
        nombrepaising = t_nombrepaising
        nombrepaiscortoesp = t_nombrepaiscortoesp
        nombrepaiscortoing = t_nombrepaiscortoing
        _idmonedacurso = monedas._idmoneda
        cvemonedacurso = monedas.cvemonedaA05
        nombremonedacurso = monedas.nombremoneda

        monedasoficiales = New List(Of moneda) From {monedas}

        _idcontinente = Nothing
        _idbandera = Nothing
        _idcapital = Nothing
        _idtratadoscomerciales = Nothing
        archivado = False
        estado = 1

    End Sub

    Property _id As ObjectId
    Property _idPais As Int32?
    Property cvecomercioMX As String
    Property cveISOnum As Int32?
    Property cveISO2 As String
    Property cveISO3 As String
    Property nombrepaisesp As String
    Property nombrepaising As String
    Property nombrepaiscortoesp As String
    Property nombrepaiscortoing As String
    Property _idmonedacurso As ObjectId
    Property cvemonedacurso As String
    Property nombremonedacurso As String
    <BsonIgnoreIfNull>
    Property monedasoficiales As List(Of moneda)
    <BsonIgnore>
    Property _idcontinente As ObjectId
    <BsonIgnore>
    Property _idbandera As ObjectId
    <BsonIgnore>
    Property _idcapital As ObjectId
    <BsonIgnore>
    Property _idtratadoscomerciales As ObjectId
    Property archivado As Boolean? = False
    Property estado As Int16

End Class

Public Class moneda

    Property _idmoneda As ObjectId
    <BsonIgnoreIfNull>
    Property secmoneda As Int32
    Property cvemonedaA05 As String
    <BsonIgnoreIfNull>
    Property nombremoneda As String
    <BsonIgnoreIfNull>
    Property claveISO As String
    <BsonIgnoreIfNull>
    Property codigoISO As Int32
    Property archivado As Boolean = False
    Property estado As Int16

End Class