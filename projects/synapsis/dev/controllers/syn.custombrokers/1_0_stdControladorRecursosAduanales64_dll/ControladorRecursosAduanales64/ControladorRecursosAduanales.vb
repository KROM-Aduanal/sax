Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Controllers.ControladorRecursosAduanales
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales

Public Class ControladorRecursosAduanales
    Public Enum TiposRecurso
        Generales = 1
        EjecutivosDeCuenta = 2
        FirmaElectronicaAduanal = 3
    End Enum

    Public Enum TiposOperacionAduanal
        SinDefinir = 0
        Importacion = 1
        Exportacion = 2
    End Enum

    Public Enum TiposModalidad
        SinDefinir = 0
        Maritimo = 1
        Aereo = 2
        Terrestre = 3
        Multimodal = 4
    End Enum

    Public Enum TiposDepartamento
        SinDefinir = 0
        Trafico = 1
        Administracion = 2
        Informatica = 3
        Facturacion = 4
        RecursosHumanos = 5
        Contabilidad = 6
        Glosa = 7
        Clasificacion = 8
    End Enum

    Public Enum TiposPrefijosEnviroment
        SinDefinir = 0
        ReferenciaOperativaNormal
        ReferenciaOperativaCorresponsalia
        ReferenciaOperativaCorresponsaliasTerceros
        ReferenciaAdministrativaNormal
        PolizaDiario
        PolizaIngreso
        PolizaEgreso
    End Enum

    Public Enum TiposReferenciasOperativas
        SinDefinir = 0
        Operativas
        Corresponsalias
        CorresponsaliasTerceros
    End Enum

    Public Enum TiposEstatus
        UND = 0
        DOC = 1
        FAC = 2
        PAG = 3
        DES = 4
        CGS = 5
    End Enum

    Property _id As ObjectId

    <BsonIgnoreIfNull>
    Property ambiente As Int32? = Nothing

    <BsonRepresentation(BsonType.String)>
    Property tiporecurso As TiposRecurso

    <BsonIgnoreIfNull>
    Property aduanasmodalidad As List(Of AduanaSeccionModalidad)

    <BsonIgnoreIfNull>
    Property patentes As List(Of PatenteAduanal)

    <BsonIgnoreIfNull>
    Property tiposoperacion As TiposOperacionAduanal

    <BsonIgnoreIfNull>
    Property aduanaseccionambientes As List(Of AduanaSeccionAmbiente)


    <BsonIgnoreIfNull>
    Property ejecutivoscuenta As List(Of EjecutivoDeCuenta)

    <BsonIgnoreIfNull>
    Property prefijosenviroment As List(Of prefijosenviroment)


    Sub New()

    End Sub
    Public Shared Function BuscarRecursosAduanales(ByVal tipoRecurso_ As ControladorRecursosAduanales.TiposRecurso) As ControladorRecursosAduanales

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of ControladorRecursosAduanales)("Reg007RecursosAduanales")

        Dim recursosAduanales_ As ControladorRecursosAduanales

        Dim filter_ = Builders(Of ControladorRecursosAduanales).Filter.Eq(Function(x) x.tiporecurso, tipoRecurso_)

        Dim recursos_ As New List(Of ControladorRecursosAduanales)

        Dim listEmpresas_ As New Dictionary(Of Object, Object)

        recursos_ = operationsDB_.Find(filter_).ToList()

        recursosAduanales_ = recursos_(0)

        Return recursosAduanales_

    End Function

End Class

Public Class AduanaSeccionAmbiente
    Property _idaduanaseccion As Int32
    Property ambiente As Int32
    'ReadOnly Property modalidad As String
    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class

Public Class AduanaSeccionModalidad
    Property _idaduanaseccion As Int32
    Property ciudad As String

    <BsonElement("modalidad")>
    <BsonRepresentation(BsonType.String)>
    Property modalidad As TiposModalidad
    'ReadOnly Property modalidad As String
    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class


Public Class PatenteAduanal
    Property _idpatente As Int32
    Property agenteaduanal As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class EjecutivoDeCuenta
    Property _id As ObjectId

    Property nombre As String
    Property apellidopaterno As String
    Property apellidomaterno As String

    <BsonRepresentation(BsonType.String)>
    Property departamento As TiposDepartamento

    Property archivado As Boolean = False

End Class

Public Class prefijosenviroment

    Property _idenviroment As Integer
    <BsonIgnoreIfNull>
    Property prefijosoperativos As List(Of prefijosoperativos)
    <BsonIgnoreIfNull>
    Property prefijosadministrativos As List(Of prefijosadministrativos)

End Class

Public MustInherit Class baseprefijo

    Property _idprefijo As Int16
    Property _idtipoprefijo As Int16
    Property prefijo As String
    <BsonIgnoreIfNull>
    Property info As String
    Property [default] As Boolean
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class prefijosoperativos
    Inherits baseprefijo

End Class

Public Class prefijosadministrativos
    Inherits baseprefijo

End Class

'----Experimental
Public Class ClavesDocumentoRecientes
    Property _id As ObjectId
    Property clavedocumento As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class