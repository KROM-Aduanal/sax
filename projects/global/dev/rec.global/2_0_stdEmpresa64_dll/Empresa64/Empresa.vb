Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports Wma.Exceptions
Imports Rec.Globals.IEmpresa64
Imports Rec.Globals.PaisDomicilio64
Imports Rec.Globals.Contacto64


#Region "CLASE EMPRESA"
Public MustInherit Class Empresa
    Implements IEmpresa, IDisposable

    Private disposedValue As Boolean

    Public Property _id As ObjectId _
        Implements IEmpresa._id

    Public Property _idempresa As Integer _
        Implements IEmpresa._idempresa

    <BsonIgnoreIfNull>
    Public Property _idempresakb As Integer? _
        Implements IEmpresa._idempresakb

    Public Property razonsocial As String _
        Implements IEmpresa.razonsocial

    Public Property razonsocialcorto As String _
        Implements IEmpresa.razonsocialcorto

    <BsonIgnoreIfNull>
    Public Property abreviatura As String _
        Implements IEmpresa.abreviatura

    Public Property nombrecomercial As String _
        Implements IEmpresa.nombrecomercial

    <BsonIgnoreIfNull>
    Public Property paisesdomicilios As List(Of PaisDomicilio) _
        Implements IEmpresa.paisesdomicilios

    <BsonIgnoreIfNull>
    Public Property girocomercial As String _
        Implements IEmpresa.girocomercial

    <BsonIgnoreIfNull>
    Public Property _idgrupocomercial As ObjectId? _
        Implements IEmpresa._idgrupocomercial

    <BsonIgnoreIfNull>
    Public Property contactos As List(Of Contacto) _
        Implements IEmpresa.contactos

    Public Property abierto As Boolean _
        Implements IEmpresa.abierto

    Public Property estado As Short _
        Implements IEmpresa.estado

    <BsonIgnoreIfNull>
    Public Property estatus As Short _
        Implements IEmpresa.estatus

    Public Property archivado As Boolean _
        Implements IEmpresa.archivado

    <BsonIgnore>
    Public Property esNuevoDomicilio As Boolean? _
        Implements IEmpresa.esNuevoDomicilio

    <BsonIgnore>
    Public Property tipoEmpresa As Boolean? _
        Implements IEmpresa.tipoEmpresa


    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True
        End If
    End Sub

    ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    ' Protected Overrides Sub Finalize()
    '     ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

End Class

#End Region
