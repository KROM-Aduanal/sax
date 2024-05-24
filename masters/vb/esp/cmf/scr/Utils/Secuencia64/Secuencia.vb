Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq

Public Class Secuencia
    Implements ISecuencia

    Public Property _id As ObjectId _
        Implements ISecuencia._id

    Public Property secuenciaAnterior As Integer _
        Implements ISecuencia.secuenciaAnterior

    Public Property sec As Integer _
        Implements ISecuencia.sec

    Public Property compania As Integer _
        Implements ISecuencia.compania

    Public Property area As Integer _
        Implements ISecuencia.area

    Public Property environment As Integer _
        Implements ISecuencia.environment

    Public Property anio As Integer _
        Implements ISecuencia.anio

    Public Property mes As Integer _
        Implements ISecuencia.mes

    Public Property nombre As String _
        Implements ISecuencia.nombre

    Public Property tiposecuencia As Integer _
        Implements ISecuencia.tiposecuencia

    Public Property subtiposecuencia As Integer _
        Implements ISecuencia.subtiposecuencia

    Public Property prefijo As String _
        Implements ISecuencia.prefijo

    Public Property sufijo As String _
        Implements ISecuencia.sufijo

    Public Property archivado As Boolean _
        Implements ISecuencia.archivado

    Public Property estado As Integer _
        Implements ISecuencia.estado

End Class
