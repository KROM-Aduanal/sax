Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Operaciones
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports Wma.Exceptions
Imports gsol.krom
Imports MongoDB.Bson

Namespace Syn.Documento

    <BsonKnownTypes(GetType(EntidadDatosDocumento))>
    <BsonKnownTypes(GetType(ConstructorCliente))>
    <BsonKnownTypes(GetType(ConstructorProducto))>
    <BsonKnownTypes(GetType(ConstructorReferencia))>
    <BsonKnownTypes(GetType(ConstructorPedimentoNormal))>
    <BsonKnownTypes(GetType(ConstructorProveedoresOperativos))>
    <BsonKnownTypes(GetType(ConstructorRevalidacion))>
    <BsonKnownTypes(GetType(ConstructorViajes))>
    <BsonKnownTypes(GetType(ConstructorTIGIE))>
    <BsonKnownTypes(GetType(ConstructorFacturaComercial))>
    <BsonKnownTypes(GetType(ConstructorManifestacionValor))>
    <Serializable()>
    Public Class DocumentoElectronico
        Inherits Documento
        Implements IDisposable, ICloneable

#Region "Attribute"

        'Campos especiales del expediente o legajo

        Private _folioOperacion As String

        Private _tipoDocumentoElectronico As TiposDocumentoElectronico 'PedimentoNormal, PedimentoComplementario, Rectificación, Aviso de consolidado, Partes II Impo..Expo

        Private _nombreCliente As String

        Private _idCliente As Int32

        'Para evaluar si deben ser o no colecciones simplemente.

        Private _idCorporativo As Int32 '1

        Private _nombreCorporativoEmpresarial As String 'KROM Aduanal

        Private _idSucursal As Int32 '1 | 3 | 8 | 7 ...

        Private _localidad As String 'Veracruz | México | Toluca | Manzanillo | Altamira | Laredo | Nuevo Laredo | Lázaro Cárdenas | CD Juarez | Colombia | Nogales

        Private _nombreSucursal As String 'Grupo Reyes Kuri S.C. | Despachos Aereos Internacional S. C. ...

        Private _Aduanaseccion As String '430|470|650...

        Private _relacionInterna As String ' TiposRelacionCorporativa: SucursalKROM, Corresponsal, ClienteSAAS...

        Private disposedValue As Boolean

        Protected _operacionesNodo As OperacionesNodos

        Private _tipoPropietario As String

        Private _nombrePropietario As String

        Private _idPropietario As Int32

        Private _objectIdPropietario As ObjectId

        Protected _metadatos As List(Of CampoGenerico)

        ' Private _operacionesNodo As OperacionesNodos

        <NonSerialized>
        Protected _tagWatcher As TagWatcher

        <NonSerialized>
        Private _suscriptions As List(Of subscriptionsgroup) '= New subscriptionsgroup

        <NonSerialized>
        Private _relatedFields As List(Of relatedfield)

        <NonSerialized>
        Private _documentosasociados As List(Of DocumentoAsociado)

#End Region

#Region "Properties"

        <BsonIgnore>
        Public Property RelatedFields As List(Of relatedfield)
            Get
                Return _relatedFields
            End Get
            Set(value As List(Of relatedfield))
                _relatedFields = value
            End Set
        End Property


        <BsonIgnore>
        Public Property SubscriptionsGroup As List(Of subscriptionsgroup)
            Get
                Return _suscriptions
            End Get
            Set(value As List(Of subscriptionsgroup))
                _suscriptions = value
            End Set
        End Property

        <BsonIgnore>
        Public Property DocumentosAsociados As List(Of DocumentoAsociado)
            Get
                Return _documentosasociados
            End Get
            Set(value As List(Of DocumentoAsociado))
                _documentosasociados = value
            End Set
        End Property

        <BsonElement("FolioOperacion")>
        Public Property FolioOperacion As String
            Get
                Return _folioOperacion
            End Get
            Set(value As String)
                _folioOperacion = value
            End Set
        End Property

        <BsonElement("TipoDocumentoElectronico")>
        Public Property TipoDocumentoElectronico As TiposDocumentoElectronico
            Get
                Return _tipoDocumentoElectronico
            End Get
            Set(value As TiposDocumentoElectronico)
                _tipoDocumentoElectronico = value

                _DescripcionTipoDocumentoElectronico = GetEnumDescription(DirectCast(Convert.ToInt32(value), TiposDocumentoElectronico))

            End Set
        End Property

        <BsonElement("DescripcionTipoDocumentoElectronico")>
        Public Property DescripcionTipoDocumentoElectronico As String

        <BsonElement("NombreCliente")>
        Public Property NombreCliente As String
            Get
                Return _nombreCliente
            End Get
            Set(value As String)
                _nombreCliente = value
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


        <BsonElement("TipoPropietario")>
        Public Property TipoPropietario As String

            Get

                Return _tipoPropietario

            End Get

            Set(value As String)

                _tipoPropietario = value

            End Set

        End Property

        <BsonElement("NombrePropietario")>
        Public Property NombrePropietario As String

            Get

                Return _nombrePropietario

            End Get

            Set(value As String)

                _nombrePropietario = value

            End Set

        End Property

        <BsonElement("IdPropietario")>
        Public Property IdPropietario As Int32

            Get

                Return _idPropietario

            End Get

            Set(value As Int32)

                _idPropietario = value

            End Set

        End Property

        <BsonElement("ObjectIdPropietario")>
        Public Property ObjectIdPropietario As ObjectId

            Get

                Return _objectIdPropietario

            End Get

            Set(value As ObjectId)

                _objectIdPropietario = value

            End Set

        End Property

        Public Property Seccion(ByVal claveSeccion_ As Integer) As Seccion

            Get

                Return ObtenerSeccion(claveSeccion_)

            End Get

            Set(value As Seccion)

                ActualizarSeccion(claveSeccion_, value)

            End Set

        End Property

        Public ReadOnly Property SeccionPorCampo(ByVal claveCampo_ As Integer, ByVal tipoNodo_ As TiposNodo) As Nodo

            Get

                Return ObtenerSeccionPorCampo(claveCampo_, tipoNodo_)

            End Get

        End Property

        Public Property Campo(ByVal claveCampo_ As Integer) As Campo

            Get

                Return ObtenerCampo(claveCampo_)

            End Get

            Set(value As Campo)

                ActualizarCampo(claveCampo_, value)

            End Set

        End Property
        Public Property Attribute(ByVal claveCampo_ As Integer,
                                  Optional ByVal direccionArray_ As Object(,) = Nothing) As Campo

            Get

                Return ObtenerCampo(claveCampo_)

            End Get

            Set(value As Campo)

                ActualizarCampo(claveCampo_, value)

            End Set

        End Property


        <BsonElement("Metadatos")>
        <BsonIgnoreIfDefault>
        Public Property Metadatos As List(Of CampoGenerico)

            Get

                Return _metadatos

            End Get

            Set(value As List(Of CampoGenerico))

                _metadatos = value

            End Set

        End Property
#End Region

#Region "Builders"

        Sub New()

            IdDocumento = 0

            FolioDocumento = Nothing

            UsuarioGenerador = Nothing

            FolioOperacion = Nothing

            IdCliente = 0

            NombreCliente = Nothing

            EstatusDocumento = "0"

            FechaCreacion = Now()

            _operacionesNodo = New OperacionesNodos()

        End Sub
        Sub New(ByVal referencia_ As String, ByVal tipoDocumentoElectronico_ As TiposDocumentoElectronico, ByVal nombreCliente_ As String,
                ByVal idCliente_ As Int32, ByVal idCorporativo_ As Int32, ByVal nombreCorporativoEmpresarial_ As String, ByVal idSucursal_ As Int32,
                ByVal localidad_ As String, ByVal nombreSucursal_ As String, ByVal aduanaSeccion_ As String, ByVal relacionInterna_ As String,
                ByVal Id_ As String, ByVal idDocumento_ As Int32, ByVal folioDocumento_ As String, ByVal fechaCreacion_ As Date, ByVal usuarioGenerador_ As String,
                ByVal estatusDocumento_ As Int32, ByVal documento_ As EstructuraDocumento) ', ByVal listaSecciones_ As List(Of String))

            _folioOperacion = referencia_

            _tipoDocumentoElectronico = tipoDocumentoElectronico_

            _nombreCliente = nombreCliente_

            _idCliente = idCliente_

            _idCorporativo = idCorporativo_

            _nombreCorporativoEmpresarial = nombreCorporativoEmpresarial_

            _idSucursal = idSucursal_

            _localidad = localidad_

            _nombreSucursal = nombreSucursal_

            _Aduanaseccion = aduanaSeccion_

            _relacionInterna = relacionInterna_

            Id = Id_

            IdDocumento = idDocumento_

            FolioDocumento = folioDocumento_

            FechaCreacion = fechaCreacion_

            UsuarioGenerador = usuarioGenerador_

            EstatusDocumento = estatusDocumento_

            _estructuraDocumento = documento_

            _operacionesNodo = New OperacionesNodos()


        End Sub

        Sub New(ByVal referencia_ As String, ByVal tipoDocumentoElectronico_ As TiposDocumentoElectronico, ByVal tipoPropietario_ As String,
                ByVal nombrePropietario_ As String, ByVal objectIdPropietario_ As ObjectId, ByVal idPropietario_ As Int32, ByVal metadatos_ As List(Of CampoGenerico),
                ByVal idCorporativo_ As Int32, ByVal nombreCorporativoEmpresarial_ As String, ByVal idSucursal_ As Int32, ByVal localidad_ As String, ByVal nombreSucursal_ As String,
                ByVal aduanaSeccion_ As String, ByVal relacionInterna_ As String, ByVal Id_ As String, ByVal idDocumento_ As Int32, ByVal folioDocumento_ As String,
                ByVal fechaCreacion_ As Date, ByVal usuarioGenerador_ As String, ByVal estatusDocumento_ As Int32, ByVal documento_ As EstructuraDocumento)

            _folioOperacion = referencia_

            _tipoDocumentoElectronico = tipoDocumentoElectronico_

            _tipoPropietario = tipoPropietario_

            _nombrePropietario = nombrePropietario_

            _objectIdPropietario = objectIdPropietario_

            _idPropietario = idPropietario_

            _idCorporativo = idCorporativo_

            _metadatos = metadatos_

            _nombreCorporativoEmpresarial = nombreCorporativoEmpresarial_

            _idSucursal = idSucursal_

            _localidad = localidad_

            _nombreSucursal = nombreSucursal_

            _Aduanaseccion = aduanaSeccion_

            _relacionInterna = relacionInterna_

            Id = Id_

            IdDocumento = idDocumento_

            FolioDocumento = folioDocumento_

            FechaCreacion = fechaCreacion_

            UsuarioGenerador = usuarioGenerador_

            EstatusDocumento = estatusDocumento_

            _estructuraDocumento = documento_

            _operacionesNodo = New OperacionesNodos()

        End Sub

#End Region

#Region "Methods"
        Sub Inicializa(ByRef documentoElectronico_ As DocumentoElectronico,
                        ByVal tipoDocumento_ As TiposDocumentoElectronico,
                        ByVal construir_ As Boolean)

            TipoDocumentoElectronico = tipoDocumento_

            _tagWatcher = New TagWatcher

            _operacionesNodo = New OperacionesNodos()

            _estructuraDocumento = New EstructuraDocumento(tipoDocumento_.ToString)

            If construir_ Then

                Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

                'Autoconstruccion
                ensamblador_.Construye(Me)

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

                        Me.TipoDocumentoElectronico = tipoDocumento_



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

                    TipoDocumentoElectronico = tipoDocumento_

                End If

            End If

        End Sub


        Public Sub Inicializa(ByVal folioDocumento_ As String,
                        ByVal folioOperacion_ As String,
                        ByVal idCliente_ As Integer,
                        ByVal nombreCliente_ As String,
                        ByVal tipoDocumento_ As TiposDocumentoElectronico)

            _tagWatcher = New TagWatcher

            _operacionesNodo = New OperacionesNodos()

            _estructuraDocumento = New EstructuraDocumento(TipoDocumentoElectronico.ToString)

            FolioDocumento = folioDocumento_

            FolioOperacion = folioOperacion_

            IdCliente = idCliente_ 'Campo obsoletos

            NombreCliente = nombreCliente_ 'Campo obsoleto

            EstatusDocumento = "1"

            FechaCreacion = Now()

            TipoDocumentoElectronico = tipoDocumento_

            Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

            'Autoconstruccion
            ensamblador_.Construye(Me)

        End Sub

        Public Sub Inicializa(ByVal folioDocumento_ As String,
                              ByVal folioOperacion_ As String,
                              ByVal tipoPropietario_ As String,
                              ByVal nombrePropietario_ As String,
                              ByVal idPropietario_ As Integer,
                              ByVal objectIdPropietario_ As ObjectId,
                              ByVal metadatos_ As List(Of CampoGenerico),
                              ByVal tipoDocumento_ As TiposDocumentoElectronico)

            _tagWatcher = New TagWatcher

            _operacionesNodo = New OperacionesNodos()

            _estructuraDocumento = New EstructuraDocumento(TipoDocumentoElectronico.ToString)

            FolioDocumento = folioDocumento_

            FolioOperacion = folioOperacion_

            TipoPropietario = tipoPropietario_

            IdPropietario = idPropietario_

            ObjectIdPropietario = objectIdPropietario_

            Metadatos = metadatos_

            NombrePropietario = nombrePropietario_

            EstatusDocumento = "1"

            FechaCreacion = Now()

            TipoDocumentoElectronico = tipoDocumento_

            Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

            'Autoconstruccion
            ensamblador_.Construye(Me)

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

        'Propiedades para memento
        Public Function GuardarInstantanea() As Object ' MementoObject

            'Return New MementoObject(_referencia,
            '                         _tipoDocumentoElectronico,
            '                         _nombreCliente,
            '                         _idCliente,
            '                         _idCorporativo,
            '                         _nombreCorporativoEmpresarial,
            '                         _idSucursal,
            '                         _localidad,
            '                         _nombreSucursal,
            '                         _Aduanaseccion,
            '                         _relacionInterna,
            '                         Id,
            '                         IdDocumento,
            '                         FolioDocumento,
            '                         FechaCreacion,
            '                         UsuarioGenerador,
            '                         EstatusDocumento,
            '                         Documento,
            '                         ListaSecciones)

        End Function
        Public Sub RestauraInstantanea(ByVal memento_ As Object) ' MementoObject)

            '_referencia = memento_.Referencia

            '_tipoDocumentoElectronico = memento_.TipoDocumentoElectronico

            '_nombreCliente = memento_.NombreCliente

            '_idCliente = memento_.IdCliente

            '''Para evaluar si deben ser o no colecciones simplemente.

            ''_idCorporativo = memento_.IDCorporativo

            ''_nombreCorporativoEmpresarial = memento_.nombreCorporativoEmpresarial

            ''_idSucursal = memento_.idSucursal

            ''_localidad = memento_.localidad

            ''_nombreSucursal = memento_.nombreSucursal

            ''_Aduanaseccion = memento_.aduanaSeccion

            ''_relacionInterna = memento_.relacionInterna


            'Id = memento_.Id

            'IdDocumento = memento_.IdDocumento

            'FolioDocumento = memento_.FolioDocumento

            'FechaCreacion = memento_.FechaCreacion

            'UsuarioGenerador = memento_.UsuarioGenerador

            'EstatusDocumento = memento_.EstatusDocumento

            'Documento = memento_.Documento

            'ListaSecciones = memento_.ListaSecciones

        End Sub

        'TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
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
            If Not Me.disposedValue Then
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

                    .IdCliente = Nothing

                    .IdDocumento = Nothing

                    .NombreCliente = Nothing

                    .FolioOperacion = Nothing

                    .TipoDocumentoElectronico = Nothing

                    .UsuarioGenerador = Nothing

                    .TipoPropietario = Nothing

                    .NombrePropietario = Nothing

                    .IdPropietario = Nothing

                    .ObjectIdPropietario = Nothing

                    .Metadatos = Nothing

                End With

            End If

            Me.disposedValue = True

        End Sub

        '**** temporalmente deshabilitadas ***
        'Public Function Clone() As Object Implements ICloneable.Clone

        '    Dim ms_ As New MemoryStream

        '    Dim objResult_ As Object = Nothing

        '    Try

        '        Dim bf As New BinaryFormatter

        '        bf.Serialize(ms_, Me)

        '        ms_.Position = 0

        '        objResult_ = bf.Deserialize(ms_)

        '    Catch ex As Exception

        '        MsgBox(ex.Message)

        '    Finally

        '        ms_.Close()

        '    End Try

        '    Return objResult_

        'End Function
        'Public Function Clone(ByVal documentoClonado_ As Object) As Object

        '    documentoClonado_ = Me.Clone()

        '    Return documentoClonado_

        'End Function

        '***** Importadas desde un descendiente ( ConstructorReferencia)

        Public Function Clone() As Object Implements ICloneable.Clone

            Dim ms_ As New MemoryStream

            Dim objResult_ As Object = Nothing

            Try

                Dim bf As New BinaryFormatter

                bf.Serialize(ms_, Me)

                ms_.Position = 0

                objResult_ = bf.Deserialize(ms_)

            Catch ex As Exception

                '_tagWatcher.SetError(Me, "Alguna clase no esta serializada, a continuación el error de excepción:" & ex.Message)

                Dim tagwatcher_ As New TagWatcher

                With tagwatcher_

                    .SetError(Me, "Alguna clase no esta serializada, a continuación el error de excepción:" & ex.Message)

                End With

            Finally

                ms_.Close()

            End Try

            Return objResult_

        End Function
        Public Function Clone(ByVal documentoClonado_ As Object) As Object

            documentoClonado_ = Me.Clone()

            Return documentoClonado_

        End Function

#End Region

#Region "Functions"


        '************* Refactory *****************
        Public Overridable Sub ConstruyeSeccion(ByVal seccionEnum_ As [Enum],
                                    ByVal tipoBloque_ As TiposBloque,
                                    ByVal conCampos_ As Boolean)

            If Not ExisteSeccion(seccionEnum_,
                                 tipoBloque_) Then

                With _estructuraDocumento(tipoBloque_)

                    .Add(New NodoGenerico() With {
                                   .Nodos = EnsamblarSeccion(seccionEnum_:=seccionEnum_,
                                                             conCampos_:=conCampos_)
                         })

                End With

            End If

        End Sub

        Public Overridable Function EnsamblarSeccion(ByVal seccionEnum_ As [Enum],
                                                    Optional conCampos_ As Boolean = True) As List(Of Nodo)

            Dim seccionDocumento_ = New SeccionGenerica()

            Dim nodos_ = New List(Of Nodo)

            With seccionDocumento_

                .SeccionGenerica = seccionEnum_

                .TipoDocumentoElectronico = TipoDocumentoElectronico

                If conCampos_ Then

                    .Nodos = ObtenerCamposSeccion(Convert.ToInt32(seccionEnum_))

                Else

                    .Nodos = New List(Of Nodo)

                End If

            End With

            nodos_.Add(seccionDocumento_)

            Return nodos_

        End Function

        Public Overridable Function Item(ByVal campoEnum_ As [Enum],
                                        ByVal tipoDato_ As Campo.TiposDato,
                                        Optional ByVal longitud_ As Int32? = 10,
                                        Optional ByVal cantidadEnteros_ As Int32? = 0,
                                        Optional ByVal cantidadDecimales_ As Int32? = 0,
                                        Optional ByVal tipoRedondeo_ As Syn.Documento.Componentes.Campo.TiposRedondeos = Syn.Documento.Componentes.Campo.TiposRedondeos.SinDefinir,
                                        Optional ByVal useAsMetadata_ As Boolean = False) As NodoGenerico


            Return New NodoGenerico With {.Nodos = EnsamblarCampo(campoEnum_, tipoDato_, longitud_, cantidadEnteros_, cantidadDecimales_, tipoRedondeo_, useAsMetadata_)}

        End Function


        Public Overridable Function Item(ByVal seccionEnum_ As [Enum],
                                         Optional conCampos_ As Boolean = True) As NodoGenerico

            Return New NodoGenerico With {.Nodos = EnsamblarSeccion(seccionEnum_, conCampos_)}

        End Function

        Public Overridable Function Def(ByVal campoEnum_ As [Enum],
                                        ByVal tipo_ As Syn.Documento.Componentes.Campo.TiposDato,
                                        Optional ByVal longitud_ As Int32 = 10,
                                        Optional ByVal cantidadEnteros_ As Int32 = 0,
                                        Optional ByVal cantidadDecimales_ As Int32 = 0,
                                        Optional ByVal tipoRedondeo_ As Syn.Documento.Componentes.Campo.TiposRedondeos = Syn.Documento.Componentes.Campo.TiposRedondeos.SinDefinir,
                                        Optional ByVal useAsMetadata_ As Boolean = False) As CampoGenerico

            Select Case tipo_

                Case Entero
                    Return New CampoGenerico(New CampoEntero()) With {.CampoGenerico = campoEnum_, .TipoDato = tipo_, .CantidadEnteros = cantidadEnteros_, .UseAsMetadata = useAsMetadata_}

                Case Booleano
                    Return New CampoGenerico(New CampoEntero()) With {.CampoGenerico = campoEnum_, .TipoDato = tipo_, .UseAsMetadata = useAsMetadata_}

                Case Fecha
                    Return New CampoGenerico(New CampoFecha()) With {.CampoGenerico = campoEnum_, .TipoDato = tipo_, .UseAsMetadata = useAsMetadata_}

                Case Real
                    Return New CampoGenerico(New CampoReal()) With {.CampoGenerico = campoEnum_, .TipoDato = tipo_, .CantidadEnteros = cantidadEnteros_, .CantidadDecimales = cantidadDecimales_, .TipoRedondeo = tipoRedondeo_, .UseAsMetadata = useAsMetadata_}

                Case Texto
                    Return New CampoGenerico(New CampoTexto()) With {.CampoGenerico = campoEnum_, .TipoDato = tipo_, .Longitud = longitud_, .UseAsMetadata = useAsMetadata_}

                Case IdObject
                    Return New CampoGenerico(New CampoTexto()) With {.CampoGenerico = campoEnum_, .TipoDato = tipo_, .UseAsMetadata = useAsMetadata_}

            End Select

            Dim tagwatcher_ As New TagWatcher

            With tagwatcher_

                .SetError(Me, "¡No se ha definido el tipo de dato para: " & tipo_.ToString)

            End With

            Return Nothing

        End Function

        '**************Overrides****************

        Public Overridable Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

        End Function

        'Public Overridable Function EnsamblarCampo(ByVal idCampo_ As Integer) As List(Of Nodo)


        'End Function

        Public Overridable Function EnsamblarCampo(ByVal campoEnum_ As [Enum],
                                                   ByVal tipo_ As Campo.TiposDato,
                                                   Optional ByVal longitud_ As Int32? = 10,
                                                   Optional ByVal cantidadEnteros_ As Int32? = 0,
                                                   Optional ByVal cantidadDecimales_ As Int32? = 0,
                                                   Optional ByVal tipoRedondeo_ As Syn.Documento.Componentes.Campo.TiposRedondeos = Syn.Documento.Componentes.Campo.TiposRedondeos.SinDefinir,
                                                   Optional ByVal useAsMetadata_ As Boolean = False) As List(Of Nodo)

            Dim campoDocumento_ = Def(campoEnum_, tipo_, longitud_, cantidadEnteros_, cantidadDecimales_, tipoRedondeo_, useAsMetadata_)

            Dim nodos_ = New List(Of Nodo)

            nodos_.Add(campoDocumento_) : Return nodos_

        End Function



        '**************Overrides****************

        Protected Function ExisteSeccion(ByVal seccionEnum_ As [Enum],
                                      ByVal tipoBloque_ As TiposBloque)

            For Each nodo_ As Nodo In _estructuraDocumento(tipoBloque_)

                For Each nodoSeccion_ As Nodo In nodo_.Nodos

                    If nodoSeccion_.TipoNodo = TiposNodo.Seccion Then

                        Dim seccion_ = CType(nodoSeccion_, DecoradorSeccion)

                        If seccion_.IDUnico = Convert.ToInt32(seccionEnum_) Then

                            Return True

                        End If

                    End If

                Next

            Next

            Return False

        End Function
        Protected Function ObtenerSeccion(ByVal claveSeccion_ As Object) As Seccion

            Dim seccion_ = Nothing

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In _estructuraDocumento.Parts

                seccion_ = _operacionesNodo.ObtenerNodo(nodos_:=parDatos_.Value,
                                             IdUnico_:=claveSeccion_,
                                             tipoNodo_:=TiposNodo.Seccion)

                If Not IsNothing(seccion_) Then

                    Return seccion_

                End If

            Next

            Return Nothing

        End Function

        Public Function ObtenerSeccionPorCampo(ByVal claveCampo_ As Int16, ByVal tipoNodo_ As TiposNodo) As Nodo

            Dim seccion_ = Nothing

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In _estructuraDocumento.Parts

                seccion_ = _operacionesNodo.ObtenerSeccionPorCampo(nodos_:=parDatos_.Value,
                                                                        IdUnico_:=claveCampo_,
                                                                        tipoNodo_:=tipoNodo_)

                If Not IsNothing(seccion_) Then

                    Return seccion_

                End If

            Next

            Return Nothing

        End Function
        Protected Function ObtenerCampo(ByVal claveCampo_ As Integer) As Campo

            Dim campo_ = Nothing

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In _estructuraDocumento.Parts

                campo_ = _operacionesNodo.ObtenerNodo(nodos_:=parDatos_.Value,
                                             IdUnico_:=claveCampo_,
                                             tipoNodo_:=TiposNodo.Campo)

                If Not IsNothing(campo_) Then

                    Return campo_

                End If

            Next

            Return Nothing

        End Function
        Private Function ActualizarSeccion(ByVal claveSeccion_ As Object,
                                           ByVal seccionNueva_ As Object) As Boolean

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In _estructuraDocumento.Parts

                Dim respuesta_ = _operacionesNodo.ActualizarNodo(nodos_:=parDatos_.Value,
                                                                 IdUnico_:=claveSeccion_,
                                                                 tipoNodo_:=TiposNodo.Seccion,
                                                                 nodoNuevo_:=seccionNueva_)

                If (respuesta_) Then

                    Return respuesta_

                End If

            Next

            Return False

        End Function
        Private Function ActualizarCampo(ByVal claveCampo_ As Integer,
                                    ByVal campoNuevo_ As Object) As Boolean

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In _estructuraDocumento.Parts

                Dim respuesta_ = _operacionesNodo.ActualizarNodo(nodos_:=parDatos_.Value,
                                                                 IdUnico_:=claveCampo_,
                                                                 tipoNodo_:=TiposNodo.Campo,
                                                                 nodoNuevo_:=campoNuevo_)

                If (respuesta_) Then

                    Return respuesta_

                End If

            Next

            Return False

        End Function

        Function nsp(ByVal id_ As Int32, ByVal path_ As String) As [namespace]

            Return New [namespace] With {._id = id_, .path = path_}

        End Function

        Function field(ByVal fld As [Enum], Optional ByVal nsp As Int32? = Nothing, Optional ByVal attr As String = Nothing,
                       Optional ByVal ffld As [Enum] = Nothing, Optional ByVal fnsp As Int32? = Nothing, Optional ByVal fattr As String = Nothing,
                       Optional ByVal arrayfilters As String = Nothing) As fieldInfo

            Try
                Dim ffldstr_ As String = Nothing

                If Not ffld Is Nothing Then : ffldstr_ = ffld.ToString : End If

                Return New fieldInfo With {
                            ._enum = fld,
                            ._fenum = ffld,
                            ._id = Convert.ToInt32(fld),
                            .name = fld.ToString,
                            .nsp = nsp,
                            .attr = attr,
                            ._fid = Convert.ToInt32(ffld),
                            .fname = ffldstr_,
                            .fnsp = fnsp,
                            .fattr = fattr,
                            .arrayfilters = arrayfilters
                          }

            Catch ex As Exception

                Dim tagwatcher_ As New TagWatcher : With tagwatcher_ : .SetError(Me, "¡Error al momento de establecer el campo (field)!") : End With

                Return Nothing

            End Try

        End Function
        Function field(ByVal name As String, Optional ByVal id As Int32? = Nothing, Optional ByVal nsp As Int32? = Nothing, Optional ByVal attr As String = Nothing,
                       Optional ByVal fname As String = Nothing, Optional ByVal fid As Int32? = Nothing, Optional ByVal fnsp As Int32? = Nothing, Optional ByVal fattr As String = Nothing,
                       Optional ByVal arrayfilters As String = Nothing) As fieldInfo
            Try
                Return New fieldInfo With {
                            ._id = id,
                            .name = name,
                            .nsp = nsp,
                            .attr = attr,
                            ._fid = fid,
                            .fname = fname,
                            .fnsp = fnsp,
                            .fattr = fattr,
                            .arrayfilters = arrayfilters
                          }

            Catch ex As Exception

                Dim tagwatcher_ As New TagWatcher : With tagwatcher_ : .SetError(Me, "¡Error al momento de establecer el campo (field)!") : End With

                Return Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace