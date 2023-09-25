Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

Public Class Empresa
    Public Enum TiposPersona
        Moral = 1
        Fisica = 2
    End Enum

    Public Enum TiposEmpresa
        Nacional = 1
        Extranjera = 2
    End Enum

    Public Sub New()

    End Sub
    Public Sub New(ByVal razonSocial_ As String,
                   ByVal rfc_ As rfc,
                   ByVal tipoPersona_ As TiposPersona,
                   ByVal tipoEmpresa_ As TiposEmpresa,
                   ByVal domicilio_ As domicilio,
                   Optional ByVal curp_ As curp = Nothing)

        Dim secuencia_ As New Secuencia _
                  With {.anio = 0,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "Empresas",
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
        abierto = True
        _idempresa = sec_
        _idempresakb = 0
        razonsocial = razonSocial_
        razonessociales = Nothing

        _idrfc = rfc_._idrfc
        rfc = rfc_.rfc
        rfcs = New List(Of rfc) From {rfc_}

        _idcurp = curp_._idcurp
        curp = curp_.curp
        curps = New List(Of curp) From {curp_}

        _tipopersona = tipoPersona_
        _tipoempresa = tipoEmpresa_

        domicilios = New List(Of domicilio) From {domicilio_}

        regimenfiscal = Nothing
        girocomercial = Nothing
        _idgrupocomercial = Nothing
        contactos = Nothing
        unidadesnegocios = Nothing

    End Sub

    Property _id As ObjectId

    Property estado As Int16
    Property abierto As Boolean = True
    Property _idempresa As Int32

    <BsonIgnoreIfNull>
    Property _idempresakb As Int32
    Property razonsocial As String
    Property razonessociales As List(Of razonsocial)
    Property _idrfc As ObjectId
    Property rfc As String
    Property rfcs As List(Of rfc)

    <BsonIgnoreIfNull>
    Property _idcurp As ObjectId
    <BsonIgnoreIfNull>
    Property curp As String
    <BsonIgnoreIfNull>
    Property curps As List(Of curp)

    <BsonIgnoreIfNull>
    Property regimenfiscal As List(Of regimenfiscal)
    Property tipopersona As TiposPersona
    Property tipoempresa As TiposEmpresa

    <BsonIgnoreIfNull>
    Property domicilios As List(Of domicilio)
    <BsonIgnoreIfNull>
    Property girocomercial As String

    <BsonIgnore>
    Property _idgrupocomercial As ObjectId

    <BsonIgnoreIfNull>
    Property contactos As List(Of contacto)
    <BsonIgnoreIfNull>
    Property unidadesnegocios As List(Of unidadnegocio)

End Class

Public Class razonsocial
    Property _idrazonsocial As ObjectId
    Property razonsocial As String

End Class

Public Class rfc
    Property _idrfc As ObjectId
    Property rfc As String

End Class

Public Class curp
    Property _idcurp As ObjectId
    Property curp As String

End Class

Public Class domicilio
    Property _iddomicilio As ObjectId
    <BsonIgnoreIfNull>
    Property iddivisionkb As Int32

    Property sec As Int32
    <BsonIgnoreIfNull>
    Property nombredivision As String
    <BsonIgnoreIfNull>
    Property calle As String
    <BsonIgnoreIfNull>
    Property numeroexterior As String
    <BsonIgnoreIfNull>
    Property numerointerior As String
    <BsonIgnoreIfNull>
    Property colonia As String
    <BsonIgnoreIfNull>
    Property fraccionamiento As String
    <BsonIgnoreIfNull>
    Property cp As String
    <BsonIgnoreIfNull>
    Property ciudad As String
    <BsonIgnoreIfNull>
    Property estadorepublica As String
    <BsonIgnoreIfNull>
    Property localidad As String
    <BsonIgnoreIfNull>
    Property municipio As String
    <BsonIgnoreIfNull>
    Property entidadfederativa As String
    <BsonIgnoreIfNull>
    Property pais As String
    <BsonIgnoreIfNull>
    Property avenida As String
    Property archivado As Boolean = False

End Class

Public Class regimenfiscal
    Property id As String
    Property regimen As String
    Property archivado As Boolean = False

End Class

Public Class contacto
    Property _idejecutivo As ObjectId
    Property nombrecompleto As String
    <BsonIgnoreIfNull>
    Property info As String
    Property archivado As Boolean = False

End Class

Public Class unidadnegocio

    Property _idunidadnegocio As ObjectId
    Property nombreunidad As String
    <BsonIgnoreIfNull>
    Property info As String
    Property archivado As Boolean = False

End Class
