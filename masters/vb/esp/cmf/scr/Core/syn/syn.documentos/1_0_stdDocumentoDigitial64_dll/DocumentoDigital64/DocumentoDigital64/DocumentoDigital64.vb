Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento.Componentes
Imports Syn.Documento
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Nucleo
Imports Syn.Nucleo.Recursos
Imports Syn.Operaciones
Imports Wma.Exceptions

Namespace Syn.Documento
    <Serializable()>
    Public Class DocumentoDigital

        Inherits Documento
        Implements IDisposable

#Region "Attribute"

        Private _folioOperacion As String

        Private _tipoDocumentoDigital As TiposDocumentoDigital 'SinDefinir | PedimentoNormalPDF |  PedimentoComplementarioPDF | RectificacionPDF ...

        Private _nombreRazonSocialDocumento As String

        Private _tipoVinculacion As TiposVinculacion ' AgenciaAduanal | Sucursal | Corresponsal | Cliente...

        Private _disposedValue As Boolean

        Private _nombreCliente As String

        Private _idCliente As Int32

        'Procesamiento de entradas digitales, para OCR y M3

        Private _objetoBinarioOriginal As Byte()

        Private _procesamientoRequerido As TiposProcesamientoDocumental

        Private _documentoProcesado As Boolean

        Protected _operacionesNodo As OperacionesNodos

        <NonSerialized>
        Protected _tagWatcher As TagWatcher

#End Region

#Region "Properties"

        <BsonElement("FolioOperacion")>
        Public Property FolioOperacion As String
            Get
                Return _folioOperacion
            End Get
            Set(value As String)
                _folioOperacion = value
            End Set
        End Property

        <BsonElement("IdCliente")>
        Public Property IdCliente As Integer
            Get
                Return _idCliente
            End Get
            Set(value As Integer)
                _idCliente = value
            End Set
        End Property

        <BsonElement("TipoDocumentoDigital")>
        Public Property TipoDocumentoDigital As TiposDocumentoDigital
            Get
                Return _tipoDocumentoDigital
            End Get
            Set(value As TiposDocumentoDigital)
                _tipoDocumentoDigital = value
            End Set
        End Property

        <BsonElement("NombreRazonSocialDocumento")>
        Public Property NombreRazonSocialDocumento As String
            Get
                Return _nombreRazonSocialDocumento
            End Get
            Set(value As String)
                _nombreRazonSocialDocumento = value
            End Set
        End Property

        <BsonElement("NombreCliente")>
        Public Property NombreCliente As String
            Get
                Return _nombreCliente
            End Get
            Set(value As String)
                _nombreCliente = value
            End Set
        End Property

        <BsonElement("TipoVinculacion")>
        Public Property TipoVinculacion As String
            Get
                Return _tipoVinculacion
            End Get
            Set(value As String)
                _tipoVinculacion = value
            End Set
        End Property
#End Region

#Region "Builders"
        Sub New()

        End Sub
        Sub New(ByVal tipoDocumentoDigital_ As TiposDocumentoDigital, ByVal nombreRazonSocial_ As String, ByVal tipoVinculacion_ As String,
                ByVal Id_ As String, ByVal idDocumento_ As Int32, ByVal folioDocumento_ As String, ByVal fechaCreacion_ As Date, ByVal usuarioGenerador_ As String,
                ByVal estatusDocumento_ As Int32, ByVal documento_ As EstructuraDocumento)

            _tipoDocumentoDigital = tipoDocumentoDigital_
            _nombreRazonSocialDocumento = nombreRazonSocial_
            _tipoVinculacion = tipoVinculacion_

            'avanzado

            Id = Id_
            IdDocumento = idDocumento_
            FolioDocumento = folioDocumento_
            FechaCreacion = fechaCreacion_
            UsuarioGenerador = usuarioGenerador_
            EstatusDocumento = estatusDocumento_
            _estructuraDocumento = documento_

        End Sub

#End Region

#Region "Methods"

        Sub Inicializa(ByRef documentoElectronico_ As DocumentoElectronico,
                        ByVal tipoDocumento_ As TiposDocumentoDigital,
                        ByVal construir_ As Boolean)

            TipoDocumentoDigital = tipoDocumento_

            _tagWatcher = New TagWatcher

            _estructuraDocumento = New EstructuraDocumento(tipoDocumento_.ToString)

            If construir_ Then

                'Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

                'Autoconstruccion
                'ensamblador_.Construye(Me)

                If Not documentoElectronico_ Is Nothing Then

                    With documentoElectronico_

                        Me.IdDocumento = .IdDocumento

                        Me.FolioDocumento = .FolioDocumento

                        Me.UsuarioGenerador = .UsuarioGenerador

                        Me.FolioOperacion = .FolioOperacion

                        Me.IdCliente = .IdCliente

                        Me.NombreCliente = .NombreCliente

                        Me.EstatusDocumento = .EstatusDocumento

                        Me.FechaCreacion = .FechaCreacion

                        Me.TipoDocumentoDigital = tipoDocumento_



                        Me.EstructuraDocumento = .EstructuraDocumento

                        Me.EstatusDocumento = .EstatusDocumento

                        Me.Id = .Id

                    End With
                Else

                    IdDocumento = 0

                    FolioDocumento = Nothing

                    UsuarioGenerador = Nothing

                    FolioOperacion = Nothing

                    IdCliente = 0

                    NombreCliente = Nothing

                    EstatusDocumento = "0"

                    FechaCreacion = Now()

                    TipoDocumentoDigital = tipoDocumento_

                End If

            End If

        End Sub

        Sub Inicializa(ByVal folioDocumento_ As String,
                        ByVal folioOperacion_ As String,
                        ByVal idCliente_ As Integer,
                        ByVal nombreCliente_ As String,
                        ByVal tipoDocumento_ As TiposDocumentoDigital)

            _tagWatcher = New TagWatcher

            _estructuraDocumento = New EstructuraDocumento(TipoDocumentoDigital.ToString)

            FolioDocumento = folioDocumento_

            FolioOperacion = folioOperacion_

            IdCliente = idCliente_

            NombreCliente = nombreCliente_

            EstatusDocumento = "1"

            FechaCreacion = Now()

            TipoDocumentoDigital = tipoDocumento_

            'Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

            'Autoconstruccion
            'ensamblador_.Construye(Me)

        End Sub


        Public Overrides Sub ConstruyeEncabezado()
        End Sub

        Public Overrides Sub ConstruyeCuerpo()
        End Sub

        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()
        End Sub

        Public Overrides Sub ConstruyePiePagina()
        End Sub

        Public Overrides Sub GeneraDocumento()

        End Sub

        ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
        Protected Overrides Sub Finalize()
            ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
            Dispose(disposing:=False)
            MyBase.Finalize()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me._disposedValue Then
                If disposing Then
                    ' TODO: eliminar estado administrado (objetos administrados).
                End If

                ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                ' TODO: Establecer campos grandes como Null.

                'Recursos no adminitrados dependientes.

                With Me

                    ._estructuraDocumento = Nothing

                    .FolioDocumento = Nothing

                    .Id = Nothing

                    .IdDocumento = Nothing

                    .NombreRazonSocialDocumento = Nothing

                    .TipoDocumentoDigital = Nothing

                    .UsuarioGenerador = Nothing

                End With

            End If

            Me._disposedValue = True

        End Sub

#End Region

#Region "Funtions"

#End Region

    End Class

End Namespace