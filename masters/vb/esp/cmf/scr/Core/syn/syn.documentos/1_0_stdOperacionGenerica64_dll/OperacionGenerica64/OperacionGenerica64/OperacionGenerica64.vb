
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos

Namespace Syn.Documento

    <Serializable()>
    Public Class OperacionGenerica
        ' Inherits EntidadDatosDocumento
        Implements IDisposable

#Region "Atributos"

        '<BsonRepresentation(BsonType.ObjectId)>
        'Public Property Id As String
        <BsonId>
        <BsonRepresentation(BsonType.ObjectId)>
        Public Property Id As ObjectId

        Private disposedValue As Boolean


#End Region

#Region "Builders"

        Sub New(ByVal documentoElectronico_ As DocumentoElectronico)

            _soloLectura = False

            _idPermisoConsulta = Nothing

            _publicado = False

            '_seccionesActivas = New List(Of Seccion)

            _Borrador = New Borrador

            _borrador.Folder.ArchivoPrincipal.Dupla.Fuente = documentoElectronico_

            _Borrador.Folder.DocumentosAsociados = New List(Of DocumentoAsociado)

            _Publicaciones = New Publicaciones

            _estado = 1

            _abierto = True

        End Sub

#End Region

#Region "Properties"

        <BsonElement("FolioOperacion")>
        Public Property FolioOperacion As String
        Public Property SoloLectura As Boolean
        Public Property IdPermisoConsulta As Integer
        Public Property Publicado As Boolean
        '<BsonIgnoreIfNull>
        'Public Property SeccionesActivas As List(Of Seccion)
        Public Property Borrador As Borrador
        <BsonIgnoreIfNull>
        Public Property Publicaciones As Publicaciones

        <BsonElement("estado")>
        Public Property estado As Integer

        <BsonElement("abierto")>
        Public Property abierto As Boolean

        <BsonIgnoreIfNull>
        Public Property FirmaElectronica As String

        Protected Overridable Sub Dispose(disposing As Boolean)

            If Not disposedValue Then

                If disposing Then

                    Id = Nothing

                    _SoloLectura = False

                    _Publicado = False

                    '_SeccionesActivas = Nothing

                    _Borrador = Nothing

                    _Publicaciones = Nothing

                    _FolioOperacion = Nothing

                    _IdPermisoConsulta = Nothing

                End If

                disposedValue = True

            End If

        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose

            ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
            Dispose(disposing:=True)

            GC.SuppressFinalize(Me)

        End Sub

        Public Shared Widening Operator CType(v As List(Of OperacionGenerica)) As OperacionGenerica

            Throw New NotImplementedException()

        End Operator

        Public Shared Widening Operator CType(v As OperacionGenerica) As List(Of Object)

            Throw New NotImplementedException()

        End Operator


#End Region

#Region "Methods"

#End Region

    End Class
    <Serializable()>
    Public Class Borrador

        Sub New()

            '_ResumenMovimientos = New List(Of RegistroMovimiento)

            _AnaliticaDatos = New AnaliticaDatosGeneral

            _Folder = New Folder

        End Sub

        '<BsonIgnoreIfNull>
        'Public Property Usosycostumbres As UsosCostumbres

        '<BsonIgnoreIfNull>
        'Public Property ResumenMovimientos As List(Of RegistroMovimiento)

        <BsonIgnoreIfNull>
        Public Property AnaliticaDatos As AnaliticaDatosGeneral
        Public Property Folder As Folder

    End Class


    <Serializable()>
    Public Class UsosCostumbres

        Private _id As Int32

        Sub New()

        End Sub

    End Class

    <Serializable()>
    Public Class RegistroMovimiento

        Public Property Evento As EventosGenericos
        Public Property DescripcionEvento As String
        Public Property IDUsuario As Integer
        Public Property NombreUsuario As String
        Public Property FechaEvento As Date
        Public Property Texto As String

    End Class

    <Serializable()>
    Public Class AnaliticaDatosGeneral

        Private _cantidadPersonasInvolucradas As Int32

    End Class

    <Serializable()>
    Public Class Folder

        Sub New()

            _ArchivoPrincipal = New ArchivoPrincipal

            _ObjetosRelacionados = New List(Of ObjetoRelacionado)

            _DocumentosAsociados = New List(Of DocumentoAsociado)

        End Sub

        Public Property ArchivoPrincipal As ArchivoPrincipal
        <BsonIgnoreIfNull>
        Public Property ObjetosRelacionados As List(Of ObjetoRelacionado)
        <BsonIgnoreIfNull>
        Public Property DocumentosAsociados As List(Of DocumentoAsociado)

    End Class

    <Serializable()>
    Public Class ObjetoRelacionado

        Sub New()

            _Id = Nothing

            _Db = Nothing

            _Ref = Nothing

        End Sub

        Public Property Id As Integer

        Public Property Db As String

        Public Property Ref As String

    End Class

    <Serializable()>
    Public Class ArchivoPrincipal

        Sub New()

            _Dupla = New Dupla

            _ObjetosDerivados = New List(Of Dupla)

        End Sub

        Public Property Dupla As Dupla

        Public Property ObjetosDerivados As List(Of Dupla)

    End Class

    <Serializable()>
    Public Class Dupla

        Sub New()

            _fuente = New DocumentoElectronico

            _derivado = New DocumentoDigital

        End Sub

        Public Property Fuente As DocumentoElectronico
        Public Property Derivado As DocumentoDigital

    End Class


    <Serializable()>
    Public Class Publicaciones

        Sub New()

        End Sub
        Public Property PedimentoNormal As String
        Public Property PedimentoNormalPDF As String

    End Class

End Namespace

