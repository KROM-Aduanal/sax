Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Controllers.ControladorRecursosAduanalesGral

Public Class ControladorRecursosAduanalesGral

#Region "Enums"

    Public Enum TiposRecurso
        Generales = 1
        Pedimento = 2
        Anexo22 = 3
    End Enum

    Public Enum ClavesVinculacion
        NE = 0      'No existe
        SENA = 1    'Sí existe y no afecta
        SESA = 2    'Sí existe y si afecta
    End Enum

#End Region

#Region "Propiedades"
    <BsonId>
    Property _id As ObjectId

    <BsonRepresentation(BsonType.String)>
    Property tiporecurso As TiposRecurso

    <BsonIgnoreIfNull>
    Property tiposvinculacion As List(Of Vinculacion)

    <BsonIgnoreIfNull>
    Property tiposdocumento As List(Of TipoDocumento)

    <BsonIgnoreIfNull>
    Property tiposoperacion As List(Of TipoOperacion)

    <BsonIgnoreIfNull>
    Property tiposcuentaaduanera As List(Of TipoCuentaAduanera)

    <BsonIgnoreIfNull>
    Property clavesgarantia As List(Of ClaveGarantia)

    <BsonIgnoreIfNull>
    Property institucionesemisoras As List(Of InstitucionEmisora)

    <BsonIgnoreIfNull>
    Property tiposcargalote As List(Of TipoCargaLote)

    <BsonIgnoreIfNull>
    Property tiposprevios As List(Of TipoPrevio)

#End Region

#Region "Constructores"

    Sub New()

    End Sub

#End Region

#Region "Funciones"

    Public Shared Function Buscar(ByVal tipoRecurso_ As ControladorRecursosAduanalesGral.TiposRecurso) As ControladorRecursosAduanalesGral

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of ControladorRecursosAduanalesGral)("Reg007ConfiguracionPedimentosAduanales")

        Dim recursosAduanales_ As ControladorRecursosAduanalesGral

        Dim filter_ = Builders(Of ControladorRecursosAduanalesGral).Filter.Eq(Function(x) x.tiporecurso, tipoRecurso_)

        Dim recursos_ As New List(Of ControladorRecursosAduanalesGral)

        recursos_ = operationsDB_.Find(filter_).ToList()

        If recursos_ IsNot Nothing Then

            recursosAduanales_ = recursos_(0)

            Return recursosAduanales_
        Else

            Return Nothing

        End If

    End Function

#End Region

End Class

Public Class Vinculacion

    Property _idvinculacion As Int32

    <BsonRepresentation(BsonType.String)>
    Property descripcion As String
    <BsonIgnoreIfNull>
    Property descripcioncorta As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class TipoDocumento

    Property _idtipodocumento As Int32

    <BsonRepresentation(BsonType.String)>
    Property descripcion As String

    <BsonIgnoreIfNull>
    Property descripcioncorta As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class TipoOperacion

    Property _idtipooperacion As Int32

    <BsonRepresentation(BsonType.String)>
    Property descripcion As String

    <BsonIgnoreIfNull>
    Property descripcioncorta As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class TipoCuentaAduanera

    Property _idtipocuenta As Int32

    <BsonRepresentation(BsonType.String)>
    Property descripcion As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class ClaveGarantia

    Property _idclavegarantia As Int32

    <BsonRepresentation(BsonType.String)>
    Property descripcion As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class InstitucionEmisora

    Property _idinstitucionemisora As Int32

    <BsonRepresentation(BsonType.String)>
    Property descripcion As String

    <BsonIgnoreIfNull>
    Property descripcioncorta As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class TipoCargaLote

    Property _idtipocargalote As Int32
    Property descripcion As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class

Public Class TipoPrevio

    Property _idtipoprevio As Int32
    Property descripcion As String
    Property estado As Int16 = 1
    Property archivado As Boolean = False

End Class