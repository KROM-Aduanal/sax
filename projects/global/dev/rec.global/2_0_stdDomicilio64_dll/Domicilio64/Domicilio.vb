Imports System.Runtime.CompilerServices
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class Domicilio

    Public Sub New()

    End Sub

    Public Sub New(ByVal calle_ As String,
                    ByVal sec_ As Int32,
                    ByVal numeroexterior_ As String,
                    Optional ByVal numerointerior_ As String = Nothing,
                    Optional ByVal colonia_ As String = Nothing,
                    Optional ByVal codigopostal_ As String = Nothing,
                    Optional ByVal ciudad_ As String = Nothing,
                    Optional ByVal localidad_ As String = Nothing,
                    Optional ByVal cveMunicipio_ As String = Nothing,
                    Optional ByVal municipio_ As String = Nothing,
                    Optional ByVal cveEntidadfederativa_ As String = Nothing,
                    Optional ByVal entidadfederativa_ As String = Nothing,
                    Optional ByVal domicilioPresentacion_ As String = Nothing,
                    Optional ByVal idDivisionKb_ As Int32 = Nothing,
                    Optional ByVal pais_ As String = Nothing)

        If domicilioPresentacion_ Is Nothing Then

            domicilioPresentacion_ = calle_ & " " &
                                     numeroexterior_ & " - INT " &
                                     numerointerior_ & " CP " &
                                     codigopostal_ & " " &
                                     colonia_ & " " &
                                     localidad_ & " " &
                                     municipio_ & " " &
                                     ciudad_ & " " &
                                     entidadfederativa_ & " " &
                                     pais_

        End If

        Me._iddomicilio = ObjectId.GenerateNewId
        Me.iddivisionkb = idDivisionKb_
        Me.sec = sec_
        Me.calle = calle_
        Me.numeroexterior = numeroexterior_
        Me.numerointerior = numerointerior_
        Me.colonia = colonia_
        Me.codigopostal = codigopostal_
        Me.ciudad = ciudad_
        Me.localidad = localidad_
        Me.cveMunicipio = cveMunicipio_
        Me.municipio = municipio_
        Me.cveEntidadfederativa = cveEntidadfederativa_
        Me.entidadfederativa = entidadfederativa_
        Me.domicilioPresentacion = domicilioPresentacion_
        Me.estado = 1
        Me.archivado = False

    End Sub

    Property _iddomicilio As ObjectId

    <BsonIgnoreIfNull>
    Property iddivisionkb As Integer?

    Property sec As Integer

    Property calle As String

    Property numeroexterior As String

    <BsonIgnoreIfNull>
    Property numerointerior As String

    <BsonIgnoreIfNull>
    Property colonia As String

    <BsonIgnoreIfNull>
    Property codigopostal As String

    Property ciudad As String

    <BsonIgnoreIfNull>
    Property localidad As String

    <BsonIgnoreIfNull>
    Property cveMunicipio As String

    <BsonIgnoreIfNull>
    Property municipio As String

    <BsonIgnoreIfNull>
    Property cveEntidadfederativa As String

    <BsonIgnoreIfNull>
    Property entidadfederativa As String

    Property domicilioPresentacion As String

    Property estado As Int16 = 1

    Property archivado As Boolean = False

End Class