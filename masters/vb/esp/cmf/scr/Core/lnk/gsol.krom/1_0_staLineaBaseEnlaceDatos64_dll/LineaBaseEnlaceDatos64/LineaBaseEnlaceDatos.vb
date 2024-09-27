Imports Wma.Exceptions
Imports gsol.krom.Referencia.AtributosDimensionReferencias
Imports gsol.krom.Fracciones
Imports gsol.krom.Facturas
Imports gsol.krom.Mercancias
Imports gsol.BaseDatos.Operaciones
Imports gsol.basededatos
Imports Syn.Documento
Imports Sax
Imports MongoDB.Driver
Imports System.Threading.Tasks
Imports MongoDB.Bson
Imports Page.Session
Imports System.Web.HttpContext.Current
Imports Rec.Globals.Controllers

Namespace gsol.krom

    Public Class LineaBaseEnlaceDatos

#Region "Atributos"

        Private _registros As List(Of IEntidadDatos)

        Private _tabla As New DataTable

        Private _espacioTrabajo As IEspacioTrabajo

        Private _limiteRegistros As Int32

        Private _iOperaciones As IOperacionesCatalogo

        Private _tiempoTranscurridoMilisegundos As Long = 0

        Private _modalidadPresentacion As IEnlaceDatos.ModalidadPresentacionEncabezados

        Private _granularidad As IEnlaceDatos.TiposDimension

        Private _limiteRegistrosColeccionMuestral As Int32

        Private _filtrosAvanzados As String

        Private _destinoParaReplicacion As IEnlaceDatos.DestinosParaReplicacion

        Private _objetoDatos As IConexiones.TiposRepositorio

        Private _origenDatos As IConexiones.Controladores

        Private _tipoConextion As IConexiones.TipoConexion

        'Ahora incluiremos el manifiesto de carga de Sax para obtener recursos de ahí mediante el singleton
        Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

        'Private _subscriptionsGroup As subscriptionsgroup

        Private _saxappid As Int32?

#End Region

#Region "Constructores"

        Sub New()

            Iniciar()

            _saxappid = Sax.SaxStatements.GetInstance.SaxAppIdMaster

        End Sub

        Sub New(ByVal saxappid_ As Int32)

            Iniciar()

            _saxappid = saxappid_

        End Sub

        Private Sub Iniciar()

            _registros = New List(Of IEntidadDatos)

            '_iOperaciones = New OperacionesCatalogo

            _limiteRegistros = 10000

            _modalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

            _limiteRegistrosColeccionMuestral = 1001

            _destinoParaReplicacion = IEnlaceDatos.DestinosParaReplicacion.SinDefinir

        End Sub


#End Region

#Region "Propiedades"

        Property SaxAppId As Int32

            Get

                Return _saxappid

            End Get

            Set(value As Int32)

                _saxappid = value

            End Set

        End Property

        'Private _objetoDatos As IConexiones.TiposRepositorio
        Property ObjetoDatos As IConexiones.TiposRepositorio

            Get

                Return _objetoDatos

            End Get

            Set(value As IConexiones.TiposRepositorio)

                _objetoDatos = value

            End Set

        End Property

        'Private _origenDatos As IConexiones.Controladores
        Property OrigenDatos As IConexiones.Controladores
            Get
                Return _origenDatos
            End Get
            Set(value As IConexiones.Controladores)
                _origenDatos = value
            End Set
        End Property
        'Private _tipoConextion As IConexiones.TipoConexion
        Property TipoConexion As IConexiones.TipoConexion
            Get
                Return _tipoConextion
            End Get
            Set(value As IConexiones.TipoConexion)
                _tipoConextion = value
            End Set
        End Property

        Property ReflejarEn As IEnlaceDatos.DestinosParaReplicacion
            Get
                Return _destinoParaReplicacion
            End Get
            Set(value As IEnlaceDatos.DestinosParaReplicacion)
                _destinoParaReplicacion = value
            End Set
        End Property


        Property FiltrosAvanzados As String
            Get
                Return _filtrosAvanzados
            End Get
            Set(value As String)
                _filtrosAvanzados = value
            End Set
        End Property

        Property LimiteRegistrosColeccionMuestral As Int32
            Get
                Return _limiteRegistrosColeccionMuestral
            End Get
            Set(value As Int32)
                _limiteRegistrosColeccionMuestral = value
            End Set
        End Property


        Public Property Granularidad As IEnlaceDatos.TiposDimension
            Get
                Return _granularidad
            End Get
            Set(value As IEnlaceDatos.TiposDimension)
                _granularidad = value
            End Set
        End Property
        Public Property ModalidadPresentacion As IEnlaceDatos.ModalidadPresentacionEncabezados
            Get
                Return _modalidadPresentacion

            End Get

            Set(value As IEnlaceDatos.ModalidadPresentacionEncabezados)

                _modalidadPresentacion = value

            End Set

        End Property

        Public ReadOnly Property TiempoTranscurridoMilisegundos As Long

            Get

                Return _tiempoTranscurridoMilisegundos

            End Get

        End Property

        Public Property IOperaciones As IOperacionesCatalogo

            Get

                Return _iOperaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _iOperaciones = value

            End Set

        End Property

        Public Property LimiteRegistros As Int32

            Get

                Return _limiteRegistros

            End Get

            Set(value As Int32)

                _limiteRegistros = value

            End Set

        End Property

        Public Property EspacioTrabajo As IEspacioTrabajo

            Get

                Return _espacioTrabajo

            End Get

            Set(value As IEspacioTrabajo)

                _espacioTrabajo = value

            End Set

        End Property

        Public ReadOnly Property Tabla As DataTable

            Get
                Return _tabla
            End Get

        End Property


        Public Property Registros As List(Of IEntidadDatos)

            Get
                Return _registros

            End Get

            Set(value As List(Of IEntidadDatos))

                _registros = value

            End Set

        End Property

#End Region

#Region "Metodos"

        Private Sub CargaCampo(ByRef registroNuevo_ As IEntidadDatos,
                           ByVal atributo_ As Object,
                           ByVal descripcion_ As String,
                           ByVal valor_ As String,
                           Optional ByVal orden_ As Int32 = 0)

            Dim campoLleno_ As New CampoVirtual


            With campoLleno_

                .Atributo = atributo_

                .Descripcion = descripcion_

                .Orden = orden_

                .Valor = valor_

            End With

            registroNuevo_.Atributos.Add(campoLleno_)

            If Not _tabla.Columns.Contains(atributo_.ToString) Then

                Select Case _modalidadPresentacion

                    Case IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

                        _tabla.Columns.Add(campoLleno_.Atributo.ToString).DataType =
                        System.Type.GetType("System.String")

                    Case IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesInforme

                        If Not campoLleno_.Descripcion Is Nothing Then

                            _tabla.Columns.Add(campoLleno_.Descripcion.ToString).DataType =
                            System.Type.GetType("System.String")

                        Else
                            _tabla.Columns.Add(campoLleno_.Atributo.ToString).DataType =
                            System.Type.GetType("System.String")

                        End If


                End Select


            End If

            registroNuevo_.NewRow(campoLleno_.Atributo.ToString) = campoLleno_.Valor

        End Sub

        Public Async Function ProduceTransaccionDocumento(ByVal documentoElectronico_ As DocumentoElectronico,
                                           ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta,
                                           ByVal clausulasLibres_ As String,
                                           Optional ByVal tipoOperacionDatosNoSQL_ As IOperacionesCatalogo.TiposOperacionNoSQL = IOperacionesCatalogo.TiposOperacionNoSQL.Consulta,
                                           Optional ByVal session_ As IClientSessionHandle = Nothing) As Task(Of TagWatcher)

            'MOP 25012022, no hemos muerto de COVID, estamos casi todos aquí ¿Quien sigue allá?
            'la asignacion de la base de datos destino será decidida en función del recurso solicitante.
            'Sax tendrá conocimiento de estas relaciones
            '--------------------------------------------------------------------------------------------------------
            'MOP 19032000, Rusia ha invadido Ucrania, en posverdad está siento especialmente dificil conocer la verdad,
            'Pero si existe la sospecha que el mundo podría estar peor cada día, después de entender que no hemos aprendido
            'nada sobre la diginidad de los seres humanos

            Dim tagwatcherlocal_ As TagWatcher

            Dim tiempoInicialL_ As Long = Nothing

            Dim tiempoFinalL_ As Long = Nothing

            Dim tiempoTranscurridoL_ As Long = 0

            tiempoInicialL_ = ObtenerMilisegundos(DateTime.Now)

            Select Case tipoOperacionDatosNoSQL_

                Case IOperacionesCatalogo.TiposOperacionNoSQL.Consulta

                    tagwatcherlocal_ = New TagWatcher

                    tagwatcherlocal_.SetError()

                Case IOperacionesCatalogo.TiposOperacionNoSQL.Insercion

                    'Dim taskbool_ As Boolean = Await Transaccion(documentoElectronico_)
                    Dim taskbool_ As Task(Of TagWatcher) = Transaccion(documentoElectronico_, False, session_)

                    tagwatcherlocal_ = taskbool_.Result

                Case Else

                    tagwatcherlocal_ = New TagWatcher

                    tagwatcherlocal_.SetError()

            End Select

            tiempoFinalL_ = ObtenerMilisegundos(DateTime.Now)

            tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

            _tiempoTranscurridoMilisegundos = tiempoTranscurridoL_

            Return tagwatcherlocal_

        End Function
        'RESPALDO
        'Private Async Sub AssignSubscriptions(func_ As Action(Of Object),
        '                                      ByVal myObjectID_ As ObjectId,
        '                                      ByVal operacionGenerica_ As Object,
        '                                      ByVal session_ As IClientSessionHandle,
        '                                      ByVal operationsDB_ As Object,
        '                                      ByVal fromResource_ As Object,
        '                                      ByVal iEnlace_ As IEnlaceDatos,
        '                                      ByVal documentoElectronico_ As DocumentoElectronico,
        '                                      ByVal status_ As TagWatcher,
        '                                      Optional ByVal createStructure_ As Boolean = False,
        '                                      Optional ByVal isClosed_ As Boolean = False)

        '    Try

        '        '------------------ We need collect all data related with our related fields after insert ------------------------

        '        documentoElectronico_.RelatedFields = New List(Of relatedfield)

        '        Dim countRelatedFields_ = 0

        '        If documentoElectronico_.SubscriptionsGroup IsNot Nothing Then

        '            For Each subscriptions_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

        '                Dim fieldsTemporal_ As New List(Of fieldInfo)

        '                For Each field_ As fieldInfo In subscriptions_.subscriptions.fields

        '                    With operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        '                        Dim campo_

        '                        If Not field_._id = 0 Then

        '                            campo_ = .Campo(field_._enum)

        '                        Else

        '                            campo_ = .Campo(field_._id)

        '                        End If

        '                        If campo_ Is Nothing Then

        '                            Continue For

        '                        End If

        '                        fieldsTemporal_.Add(field_)

        '                        AddRelatedField(campo_, field_, subscriptions_, documentoElectronico_)

        '                    End With

        '                Next

        '                subscriptions_.subscriptions.fields = fieldsTemporal_

        '            Next

        '            If Not documentoElectronico_.RelatedFields Is Nothing Then

        '                countRelatedFields_ = documentoElectronico_.RelatedFields.Count()

        '            End If

        '        End If


        '        If countRelatedFields_ > 0 Then

        '            '----------------------- W o r k s p a c e   f o r   s u b s c r i p t i o n s --------------------------
        '            'Create filter
        '            Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Function(x) x.fromcollectionname, fromResource_)

        '            'Create collection
        '            If createStructure_ Then

        '                Dim countNotUpserted_ As Int32 = 0

        '                For Each subscriptiongroupdefined_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

        '                    '------------------- Direct connection for dynamic collections --------------------------------------------
        '                    Dim toDatabaseName_ As String = Nothing : Dim toCollectionName_ As String = Nothing : Dim rol_ As Sax.rol

        '                    With _statements

        '                        rol_ = .GetDatabaseAndCollectionName(toDatabaseName_, toCollectionName_, subscriptiongroupdefined_.toresource, Nothing)

        '                        If rol_.officesuffix Then

        '                            toCollectionName_ = toCollectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

        '                            toCollectionName_ = toCollectionName_.Replace("Reg", "Vin")

        '                        End If
        '                        'temporal idea
        '                        toCollectionName_ = "FAF_" & toCollectionName_

        '                    End With

        '                    Dim subscriptionDBDirect_ = iEnlace_.
        '                                            GetMongoClient().
        '                                            GetDatabase(toDatabaseName_).
        '                                            GetCollection(Of subscriptionsgroup)(toCollectionName_)

        '                    Dim setStructureOfSubs_

        '                    Dim from_ As String

        '                    With subscriptiongroupdefined_

        '                        If .fromcollectionname Is Nothing Or
        '                       .fromcollectionname = "auto" Then

        '                            from_ = fromResource_

        '                        End If

        '                        setStructureOfSubs_ = Builders(Of subscriptionsgroup).
        '                                          Update.Set(Function(x) x.active, .active).
        '                                                 Set(Function(x) x.defaultattribute, .defaultattribute).
        '                                                 Set(Function(x) x.fromcollectionname, from_).
        '                                                 Set(Function(x) x.subscriptions, .subscriptions)
        '                    End With

        '                    Dim updateList_ = New List(Of UpdateDefinition(Of subscriptionsgroup))

        '                    updateList_.Add(setStructureOfSubs_)

        '                    Dim finalUpdate_ = Builders(Of subscriptionsgroup).
        '                                          Update.Combine(updateList_)

        '                    Dim result_ = Await subscriptionDBDirect_.UpdateOneAsync(session_,
        '                                                      filter_,
        '                                                      finalUpdate_,
        '                                                      New UpdateOptions With {.IsUpsert = True}).ConfigureAwait(False)

        '                    If result_.upsertedId Is Nothing Then

        '                        countNotUpserted_ += 1

        '                    End If

        '                Next

        '                If countNotUpserted_ > 0 Then

        '                    status_.SetError(Me, "Error upserting to MongoDB: =( ")

        '                    If isClosed_ Then : Await session_.AbortTransactionAsync() : End If

        '                Else
        '                    'here falteishon ok
        '                    status_.SetOK()

        '                    status_.ObjectReturned = operacionGenerica_

        '                    If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

        '                End If

        '            Else

        '                Dim countUndoned_ As Int32 = 0

        '                For Each subscriptiongroupdefined_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

        '                    Dim countFields_ = (From ref In documentoElectronico_.RelatedFields
        '                                        Where ref.toresource = subscriptiongroupdefined_.toresource
        '                                        Select ref.foreignkey).Count()

        '                    If countFields_ = 0 Then

        '                        Continue For

        '                    End If

        '                    Dim newListOfFollowers_ As New List(Of follower)

        '                    With newListOfFollowers_

        '                        For Each relatedField_ As relatedfield In documentoElectronico_.RelatedFields

        '                            If relatedField_.toresource = subscriptiongroupdefined_.toresource Then

        '                                .Add(
        '                                  New follower With
        '                                    {
        '                                      ._myid = myObjectID_,
        '                                      ._idfriend = relatedField_.foreignkey,
        '                                      .sec = 1
        '                                    }
        '                                )

        '                            End If

        '                        Next

        '                    End With

        '                    '----------------------------------- Group for my subscriptions --------------------------------------
        '                    Dim sgroup_ As New subscriptionsgroup _
        '                    With {
        '                          .subscriptions = New subscriptions With
        '                            {
        '                             .followers = newListOfFollowers_
        '                             }
        '                         }

        '                    '--------------------------------- Direct connection for dynamic collections --------------------------------------------
        '                    Dim toDatabaseName_ As String = Nothing : Dim toCollectionName_ As String = Nothing : Dim rol_ As Sax.rol

        '                    With _statements

        '                        rol_ = .GetDatabaseAndCollectionName(toDatabaseName_, toCollectionName_, subscriptiongroupdefined_.toresource, Nothing)

        '                        If rol_.officesuffix Then

        '                            toCollectionName_ = toCollectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

        '                            toCollectionName_ = toCollectionName_.Replace("Reg", "Vin")

        '                        End If
        '                        'idea temporal
        '                        toCollectionName_ = "FAF_" & toCollectionName_

        '                    End With

        '                    Dim subscriptionDBDirect_ = iEnlace_.
        '                                            GetMongoClient().
        '                                            GetDatabase(toDatabaseName_).
        '                                            GetCollection(Of subscriptionsgroup)(toCollectionName_)


        '                    Dim result_ = Await subscriptionDBDirect_.UpdateOneAsync(session_,
        '                                                  filter_,
        '                                                  Builders(Of subscriptionsgroup).
        '                                                  Update.AddToSetEach("subscriptions.followers",
        '                                                  sgroup_.subscriptions.followers)).ConfigureAwait(False)

        '                    subscriptiongroupdefined_.subscriptions.followers = sgroup_.subscriptions.followers

        '                    If result_.MatchedCount = 0 Then

        '                        countUndoned_ += 1

        '                    End If

        '                Next

        '                If countUndoned_ > 0 Then

        '                    If isClosed_ Then : Await session_.AbortTransactionAsync() : End If

        '                    Using session2_ = Await iEnlace_.GetMongoClient().
        '                                        StartSessionAsync().
        '                                        ConfigureAwait(False)

        '                        session2_.StartTransaction()

        '                        AssignSubscriptions(Sub()



        '                                            End Sub,
        '                                        myObjectID_,
        '                                        operacionGenerica_,
        '                                        session2_,
        '                                        operationsDB_,
        '                                        fromResource_,
        '                                        iEnlace_,
        '                                        documentoElectronico_,
        '                                        status_,
        '                                        True,
        '                                        isClosed_)


        '                        status_.SetOK()

        '                        status_.ObjectReturned = operacionGenerica_


        '                    End Using



        '                Else

        '                    status_.SetOK()

        '                    status_.ObjectReturned = operacionGenerica_

        '                    If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

        '                End If


        '            End If

        '        Else

        '            status_.SetOK()

        '            status_.ObjectReturned = operacionGenerica_

        '            If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

        '        End If

        '    Catch e As Exception

        '        status_.SetError(Me, "Error writing to MongoDB: " & e.Message)

        '        If isClosed_ Then : session_.AbortTransactionAsync() : End If


        '    End Try


        '    If Not func_ Is Nothing Then

        '        func_(Nothing)

        '    End If

        'End Sub

        Private Sub AddRelatedField(ByVal campo_ As Object,
                                    ByRef field_ As fieldInfo,
                                    ByRef subscriptions_ As subscriptionsgroup,
                                    ByRef documentoElectronico_ As DocumentoElectronico)

            With campo_

                Select Case field_.attr

                    Case "Valor"

                        If Not .Valor Is Nothing Then

                            documentoElectronico_.RelatedFields.
                                                    Add(New relatedfield With
                                                        {
                                                         .foreignkey = documentoElectronico_.Campo(subscriptions_._foreignkey).Valor, 'New ObjectId,
                                                         .foreignkeyname = subscriptions_._foreignkeyname,
                                                         .toresource = subscriptions_.toresource,
                                                         .uniquefield = New fieldInfo With
                                                            {
                                                             ._id = field_._id,
                                                             .fname = field_.name
                                                            }
                                                        }
                                                       )

                        End If

                    Case "ValorPresentacion"

                        If Not .ValorPresentacion Is Nothing Then

                            documentoElectronico_.RelatedFields.
                                                    Add(New relatedfield With
                                                        {
                                                         .foreignkey = documentoElectronico_.Campo(subscriptions_._foreignkey).ValorPresentacion, 'New ObjectId,
                                                         .toresource = subscriptions_.toresource,
                                                         .foreignkeyname = subscriptions_._foreignkeyname,
                                                         .uniquefield = New fieldInfo With
                                                            {
                                                              ._id = field_._id,
                                                             .fname = field_.name
                                                            }
                                                        }
                                                       )

                        End If

                End Select

            End With

        End Sub

        'Private Sub AddAssociatedDocuments(ByRef operacionGenerica_ As OperacionGenerica)

        '    Dim documentoElectronico_ = operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        '    With operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        '        If .DocumentosAsociados IsNot Nothing Then

        '            If .DocumentosAsociados.Count Then

        '                Dim listaDocumentosAsociados As New List(Of DocumentoAsociado)

        '                For Each documentosasociado_ As DocumentoAsociado In .DocumentosAsociados

        '                    If documentosasociado_.idsection = 0 Then

        '                        Dim campo_ As Componentes.Campo = .Campo(documentosasociado_.idcampo)

        '                        listaDocumentosAsociados.Add(New DocumentoAsociado With {
        '                                                             ._iddocumentoasociado = campo_.Valor,
        '                                                             .idcoleccion = documentosasociado_.idcoleccion,
        '                                                             .identificadorrecurso = documentosasociado_.identificadorrecurso,
        '                                                             .firmaelectronica = campo_.ValorFirma
        '                                                         })

        '                    Else

        '                        Dim seccion_ As Componentes.Seccion = .Seccion(documentosasociado_.idsection)

        '                        For indice_ As Int32 = 1 To seccion_.CantidadPartidas

        '                            Dim partida_ As Componentes.Partida = seccion_.Partida(indice_)

        '                            listaDocumentosAsociados.Add(New DocumentoAsociado With {
        '                                                             ._iddocumentoasociado = partida_.Attribute(documentosasociado_.idcampo).Valor,
        '                                                             .idcoleccion = documentosasociado_.idcoleccion,
        '                                                             .identificadorrecurso = documentosasociado_.identificadorrecurso,
        '                                                             .firmaelectronica = partida_.Attribute(documentosasociado_.idcampo).ValorFirma
        '                                                         })

        '                        Next

        '                    End If

        '                Next

        '                operacionGenerica_.Borrador.Folder.DocumentosAsociados = listaDocumentosAsociados

        '            End If

        '        End If

        '    End With

        'End Sub

        Async Function Transaccion(ByVal documentoElectronico_ As DocumentoElectronico,
                                   Optional ByVal createStructure_ As Boolean = False,
                                   Optional ByVal sessionusr_ As IClientSessionHandle = Nothing) As Task(Of TagWatcher)


            Dim status_ As New TagWatcher

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            If sessionusr_ IsNot Nothing Then

                status_ = CuerpoTransaccionAsync(sessionusr_,
                                                 iEnlace_,
                                                 documentoElectronico_,
                                                 status_,
                                                 createStructure_,
                                                 False).Result

            Else

                Using session_ = Await iEnlace_.GetMongoClient().
                                                StartSessionAsync().
                                                ConfigureAwait(False)

                    session_.StartTransaction()

                    status_ = CuerpoTransaccionAsync(session_,
                                                     iEnlace_,
                                                     documentoElectronico_,
                                                     status_,
                                                     createStructure_,
                                                     True).Result

                End Using

            End If

            Return status_

        End Function

        'Async Function CuerpoTransaccionAsync(ByVal session_ As IClientSessionHandle,
        '                                      ByVal iEnlace_ As IEnlaceDatos,
        '                                      ByVal documentoElectronico_ As DocumentoElectronico,
        '                                      ByVal status_ As TagWatcher,
        '                                      Optional ByVal createStructure_ As Boolean = False,
        '                                      Optional ByVal isClosed_ As Boolean = False) As Task(Of TagWatcher)


        '    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

        '    Dim fromResource_ As String = operationsDB_.CollectionNamespace.CollectionName.ToString

        '    Try

        '        '----------------   W o r k s p a c e   f o r   I n s e r t O n e   o r   U p d a t e  O n e  ---------------

        '        Dim myObjectID_ As ObjectId = ObjectId.GenerateNewId

        '        Dim operacionGenerica_ =
        '            New OperacionGenerica(documentoElectronico_) _
        '              With {
        '                     .IdPermisoConsulta = 118,
        '                     .Publicado = False,
        '                     .SoloLectura = False,
        '                     .FolioOperacion = documentoElectronico_.FolioOperacion,
        '                     .Id = myObjectID_
        '                   }


        '        Dim resultInsert_ = Await operationsDB_.InsertOneAsync(session_, operacionGenerica_).ConfigureAwait(False)


        '        AssignSubscriptions(Sub()



        '                            End Sub,
        '                            myObjectID_,
        '                            operacionGenerica_,
        '                            session_,
        '                            operationsDB_,
        '                            fromResource_,
        '                            iEnlace_,
        '                            documentoElectronico_,
        '                            status_,
        '                            createStructure_,
        '                            isClosed_)

        '        status_.SetOK()

        '        status_.ObjectReturned = operacionGenerica_

        '        If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

        '    Catch e As Exception

        '        status_.SetError(Me, "Error writing to MongoDB: " & e.Message)

        '        If isClosed_ Then : session_.AbortTransactionAsync() : End If

        '        Return status_

        '    End Try

        '    Return status_

        'End Function

        'RESPALDO
        Async Function CuerpoTransaccionAsync(ByVal session_ As IClientSessionHandle,
                                              ByVal iEnlace_ As IEnlaceDatos,
                                              ByVal documentoElectronico_ As DocumentoElectronico,
                                              ByVal status_ As TagWatcher,
                                              Optional ByVal createStructure_ As Boolean = False,
                                              Optional ByVal isClosed_ As Boolean = False) As Task(Of TagWatcher)


            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

            Dim fromResource_ As String = operationsDB_.CollectionNamespace.CollectionName.ToString

            Try

                '----------------   W o r k s p a c e   f o r   I n s e r t O n e   o r   U p d a t e  O n e  ---------------

                Dim myObjectID_ As ObjectId = ObjectId.GenerateNewId

                Dim operacionGenerica_ =
                    New OperacionGenerica(documentoElectronico_) _
                      With {
                             .IdPermisoConsulta = 118,
                             .Publicado = False,
                             .SoloLectura = False,
                             .FolioOperacion = documentoElectronico_.FolioOperacion,
                             .Id = myObjectID_,
                             .FirmaElectronica = Nothing
                           }

                '----------------------- We need collect all associated documents after insert -----------------------------

                'AddAssociatedDocuments(operacionGenerica_)
                operacionGenerica_.Borrador.Folder.DocumentosAsociados = documentoElectronico_.DocumentosAsociados

                '------------------ We need collect all data related with our related fields after insert ------------------------

                documentoElectronico_.RelatedFields = New List(Of relatedfield)

                Dim countRelatedFields_ = 0

                If documentoElectronico_.SubscriptionsGroup IsNot Nothing Then

                    For Each subscriptions_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

                        Dim fieldsTemporal_ As New List(Of fieldInfo)

                        'subscriptions_.subscriptions.fields.CopyTo(fieldsTemporal_.ToArray)

                        For Each field_ As fieldInfo In subscriptions_.subscriptions.fields

                            With operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                                Dim campo_

                                If Not field_._id = 0 Then

                                    campo_ = .Campo(field_._enum)

                                Else

                                    campo_ = .Campo(field_._id)

                                End If

                                If campo_ Is Nothing Then

                                    'if wasn't founded, then must be removed from the fieldInfo list
                                    'NO se si funcione :3
                                    'documentoElectronico_.SubscriptionsGroup(indice_).subscriptions.fields.Remove(field_)

                                    'fieldsTemporal_.Remove(field_)

                                    'createStructure_ = True

                                    Continue For

                                End If

                                fieldsTemporal_.Add(field_)

                                AddRelatedField(campo_, field_, subscriptions_, documentoElectronico_)

                            End With

                        Next

                        subscriptions_.subscriptions.fields = fieldsTemporal_

                    Next

                    If Not documentoElectronico_.RelatedFields Is Nothing Then

                        countRelatedFields_ = documentoElectronico_.RelatedFields.Count()

                    End If

                End If

                '------------------------------------------------------------------------------------------------------------
                If isClosed_ = False Then

                    Dim resultInsert_ = Await operationsDB_.InsertOneAsync(session_, operacionGenerica_).ConfigureAwait(False)

                End If

                If countRelatedFields_ > 0 Then

                    '----------------------- W o r k s p a c e   f o r   s u b s c r i p t i o n s --------------------------
                    'Create filter
                    Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Function(x) x.fromcollectionname, fromResource_)

                    'Create collection
                    If createStructure_ Then

                        Dim countNotUpserted_ As Int32 = 0

                        For Each subscriptiongroupdefined_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

                            '------------------- Direct connection for dynamic collections --------------------------------------------
                            Dim toDatabaseName_ As String = Nothing : Dim toCollectionName_ As String = Nothing : Dim rol_ As Sax.rol

                            With _statements

                                rol_ = .GetDatabaseAndCollectionName(toDatabaseName_, toCollectionName_, subscriptiongroupdefined_.toresource, Nothing)

                                'If rol_.officesuffix Then

                                '    toCollectionName_ = toCollectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

                                '    toCollectionName_ = toCollectionName_.Replace("Reg", "Vin")

                                'End If
                                'temporal idea
                                toCollectionName_ = "Faf" & toCollectionName_

                            End With

                            Dim subscriptionDBDirect_ = iEnlace_.
                                                    GetMongoClient().
                                                    GetDatabase(toDatabaseName_).
                                                    GetCollection(Of subscriptionsgroup)(toCollectionName_)

                            Dim setStructureOfSubs_

                            Dim from_ As String

                            With subscriptiongroupdefined_

                                If .fromcollectionname Is Nothing Or
                               .fromcollectionname = "auto" Then

                                    from_ = fromResource_

                                End If

                                setStructureOfSubs_ = Builders(Of subscriptionsgroup).
                                                  Update.Set(Function(x) x.active, .active).
                                                         Set(Function(x) x.defaultattribute, .defaultattribute).
                                                         Set(Function(x) x.fromcollectionname, from_).
                                                         Set(Function(x) x._foreignkeyname, ._foreignkeyname).
                                                         Set(Function(x) x.subscriptions, .subscriptions)
                            End With

                            Dim updateList_ = New List(Of UpdateDefinition(Of subscriptionsgroup))

                            updateList_.Add(setStructureOfSubs_)

                            Dim finalUpdate_ = Builders(Of subscriptionsgroup).
                                                  Update.Combine(updateList_)

                            '.Unset("defaultattribute")

                            Dim result_ = Await subscriptionDBDirect_.UpdateOneAsync(session_,
                                                             filter_,
                                                             finalUpdate_,
                                                             New UpdateOptions With {.IsUpsert = True}).ConfigureAwait(False)

                            If result_.upsertedId Is Nothing Then

                                countNotUpserted_ += 1

                            End If

                        Next

                        If countNotUpserted_ > 0 Then

                            status_.SetError(Me, "Error upserting to MongoDB: =( ")

                            If isClosed_ Then : Await session_.AbortTransactionAsync() : End If

                        Else
                            'here falteishon ok
                            status_.SetOK()

                            status_.ObjectReturned = operacionGenerica_

                            If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

                        End If

                    Else

                        Dim countUndoned_ As Int32 = 0

                        For Each subscriptiongroupdefined_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

                            Dim countFields_ = (From ref In documentoElectronico_.RelatedFields
                                                Where ref.toresource = subscriptiongroupdefined_.toresource
                                                Select ref.foreignkey).Count()

                            If countFields_ = 0 Then

                                Continue For

                            End If

                            Dim newListOfFollowers_ As New List(Of follower)


                            With newListOfFollowers_

                                'For Each relatedField_ As relatedfield In documentoElectronico_.RelatedFields

                                '    If relatedField_.toresource = subscriptiongroupdefined_.toresource Then



                                '    End If

                                'Next

                                Dim relatedField_ As relatedfield = documentoElectronico_.RelatedFields.GroupBy(Function(x) x.toresource = subscriptiongroupdefined_.toresource).
                                                                                                           Select(Function(y) y.First()).
                                                                                                           Where(Function(x) x.toresource = subscriptiongroupdefined_.toresource).
                                                                                                           First

                                .Add(
                                    New follower With
                                    {
                                        ._myid = myObjectID_,
                                        ._idfriend = relatedField_.foreignkey,
                                        .sec = 1
                                    }
                                )

                            End With

                            '----------------------------------- Group for my subscriptions --------------------------------------
                            Dim sgroup_ As New subscriptionsgroup _
                            With {
                                  .subscriptions = New subscriptions With
                                    {
                                     .followers = newListOfFollowers_
                                     }
                                 }

                            '--------------------------------- Direct connection for dynamic collections --------------------------------------------
                            Dim toDatabaseName_ As String = Nothing : Dim toCollectionName_ As String = Nothing : Dim rol_ As Sax.rol

                            With _statements

                                rol_ = .GetDatabaseAndCollectionName(toDatabaseName_, toCollectionName_, subscriptiongroupdefined_.toresource, Nothing)

                                'If rol_.officesuffix Then

                                '    toCollectionName_ = toCollectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

                                '    toCollectionName_ = toCollectionName_.Replace("Reg", "Vin")

                                'End If
                                'idea temporal
                                toCollectionName_ = "Faf" & toCollectionName_

                            End With

                            Dim subscriptionDBDirect_ = iEnlace_.
                                                    GetMongoClient().
                                                    GetDatabase(toDatabaseName_).
                                                    GetCollection(Of subscriptionsgroup)(toCollectionName_)

                            Dim result_ = Await subscriptionDBDirect_.UpdateOneAsync(session_,
                                                         filter_,
                                                         Builders(Of subscriptionsgroup).
                                                         Update.AddToSetEach("subscriptions.followers",
                                                         sgroup_.subscriptions.followers)).ConfigureAwait(False)

                            subscriptiongroupdefined_.subscriptions.followers = sgroup_.subscriptions.followers

                            If result_.MatchedCount = 0 Then

                                countUndoned_ += 1

                            End If

                        Next

                        If countUndoned_ > 0 Then

                            If isClosed_ Then : Await session_.AbortTransactionAsync() : End If
                            'session_.Dispose()
                            Dim isDone_ As TagWatcher = Await Transaccion(documentoElectronico_, True)

                            If Not isDone_.Status = TagWatcher.TypeStatus.Ok Then

                                status_.SetError(Me, "Error upserting to MongoDB: =( ")

                            Else

                                'here falteishon ok
                                status_.SetOK()

                                status_.ObjectReturned = operacionGenerica_

                            End If

                        Else

                            status_.SetOK()

                            status_.ObjectReturned = operacionGenerica_

                            If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

                        End If


                    End If

                Else

                    status_.SetOK()

                    status_.ObjectReturned = operacionGenerica_

                    If isClosed_ Then : Await session_.CommitTransactionAsync() : End If

                End If

            Catch e As Exception

                status_.SetError(Me, "Error writing to MongoDB: " & e.Message)

                If isClosed_ Then : session_.AbortTransactionAsync() : End If

                Return status_

            End Try

            Return status_

        End Function


        'Async Function Transaccion2(ByVal documentoElectronico_ As DocumentoElectronico,
        '                           Optional ByVal createStructure_ As Boolean = False) As Task(Of TagWatcher)

        '    Dim status_ As New TagWatcher

        '    Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        '    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

        '    Dim fromResource_ As String = operationsDB_.CollectionNamespace.CollectionName.ToString

        '    Using session_ = Await iEnlace_.GetMongoClient().StartSessionAsync().ConfigureAwait(False)

        '        session_.StartTransaction()

        '        Try

        '            '----------------   W o r k s p a c e   f o r   I n s e r t O n e   o r   U p d a t e  O n e  ---------------

        '            Dim myObjectID_ As ObjectId = ObjectId.GenerateNewId

        '            Dim operacionGenerica_ =
        '                New OperacionGenerica(documentoElectronico_) _
        '                  With {
        '                         .IdPermisoConsulta = 118,
        '                         .Publicado = False,
        '                         .SoloLectura = False,
        '                         .FolioOperacion = documentoElectronico_.FolioOperacion,
        '                         .Id = myObjectID_
        '                       }

        '            '------------------ We need collect all data related with our related fields after insert ------------------------

        '            documentoElectronico_.RelatedFields = New List(Of relatedfield)

        '            Dim countRelatedFields_ = 0

        '            If documentoElectronico_.SubscriptionsGroup IsNot Nothing Then

        '                For Each subscriptions_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

        '                    Dim fieldsTemporal_ As New List(Of fieldInfo)

        '                    'subscriptions_.subscriptions.fields.CopyTo(fieldsTemporal_.ToArray)

        '                    For Each field_ As fieldInfo In subscriptions_.subscriptions.fields

        '                        With operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        '                            Dim campo_

        '                            If Not field_._id = 0 Then

        '                                campo_ = .Campo(field_._enum)

        '                            Else

        '                                campo_ = .Campo(field_._id)

        '                            End If

        '                            If campo_ Is Nothing Then

        '                                'if wasn't founded, then must be removed from the fieldInfo list
        '                                'NO se si funcione :3
        '                                'documentoElectronico_.SubscriptionsGroup(indice_).subscriptions.fields.Remove(field_)

        '                                'fieldsTemporal_.Remove(field_)

        '                                'createStructure_ = True

        '                                Continue For

        '                            End If

        '                            fieldsTemporal_.Add(field_)

        '                            AddRelatedField(campo_, field_, subscriptions_, documentoElectronico_)

        '                        End With

        '                    Next

        '                    subscriptions_.subscriptions.fields = fieldsTemporal_

        '                Next

        '                If Not documentoElectronico_.RelatedFields Is Nothing Then

        '                    countRelatedFields_ = documentoElectronico_.RelatedFields.Count()

        '                End If

        '            End If

        '            '------------------------------------------------------------------------------------------------------------

        '            Dim resultInsert_ = Await operationsDB_.InsertOneAsync(session_, operacionGenerica_).ConfigureAwait(False)


        '            If countRelatedFields_ > 0 Then

        '                '----------------------- W o r k s p a c e   f o r   s u b s c r i p t i o n s --------------------------
        '                'Create filter
        '                Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Function(x) x.fromcollectionname, fromResource_)

        '                'Create collection
        '                If createStructure_ Then

        '                    Dim countNotUpserted_ As Int32 = 0

        '                    For Each subscriptiongroupdefined_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

        '                        '------------------- Direct connection for dynamic collections --------------------------------------------
        '                        Dim toDatabaseName_ As String = Nothing : Dim toCollectionName_ As String = Nothing : Dim rol_ As Sax.rol

        '                        With _statements

        '                            rol_ = .GetDatabaseAndCollectionName(toDatabaseName_, toCollectionName_, subscriptiongroupdefined_.toresource, Nothing)

        '                            If rol_.officesuffix Then

        '                                toCollectionName_ = toCollectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

        '                                toCollectionName_ = toCollectionName_.Replace("Reg", "Vin")

        '                            End If

        '                        End With

        '                        Dim subscriptionDBDirect_ = iEnlace_.
        '                                                GetMongoClient().
        '                                                GetDatabase(toDatabaseName_).
        '                                                GetCollection(Of subscriptionsgroup)(toCollectionName_)

        '                        Dim setStructureOfSubs_

        '                        Dim from_ As String

        '                        With subscriptiongroupdefined_

        '                            If .fromcollectionname Is Nothing Or
        '                           .fromcollectionname = "auto" Then

        '                                from_ = fromResource_

        '                            End If

        '                            setStructureOfSubs_ = Builders(Of subscriptionsgroup).
        '                                              Update.Set(Function(x) x.active, .active).
        '                                                     Set(Function(x) x.defaultattribute, .defaultattribute).
        '                                                     Set(Function(x) x.fromcollectionname, from_).
        '                                                     Set(Function(x) x.subscriptions, .subscriptions)
        '                        End With

        '                        Dim updateList_ = New List(Of UpdateDefinition(Of subscriptionsgroup))

        '                        updateList_.Add(setStructureOfSubs_)

        '                        Dim finalUpdate_ = Builders(Of subscriptionsgroup).
        '                                              Update.Combine(updateList_)

        '                        '.Unset("defaultattribute")

        '                        Dim result_ = Await subscriptionDBDirect_.UpdateOneAsync(session_,
        '                                                      filter_,
        '                                                      finalUpdate_,
        '                                                      New UpdateOptions With {.IsUpsert = True}).ConfigureAwait(False)

        '                        If result_.upsertedId Is Nothing Then

        '                            countNotUpserted_ += 1

        '                        End If

        '                    Next

        '                    If countNotUpserted_ > 0 Then

        '                        Await session_.AbortTransactionAsync()

        '                        status_.SetError(Me, "Error upserting to MongoDB: =( ")

        '                    Else

        '                        'here falteishon ok
        '                        status_.SetOK()

        '                        status_.ObjectReturned = operacionGenerica_

        '                        Await session_.CommitTransactionAsync()

        '                    End If

        '                Else

        '                    Dim countUndoned_ As Int32 = 0

        '                    For Each subscriptiongroupdefined_ As subscriptionsgroup In documentoElectronico_.SubscriptionsGroup

        '                        Dim countFields_ = (From ref In documentoElectronico_.RelatedFields
        '                                            Where ref.toresource = subscriptiongroupdefined_.toresource
        '                                            Select ref.foreignkey).Count()

        '                        If countFields_ = 0 Then

        '                            Continue For

        '                        End If

        '                        Dim newListOfFollowers_ As New List(Of follower)

        '                        With newListOfFollowers_

        '                            For Each relatedField_ As relatedfield In documentoElectronico_.RelatedFields

        '                                If relatedField_.toresource = subscriptiongroupdefined_.toresource Then

        '                                    .Add(
        '                                      New follower With
        '                                        {
        '                                          ._myid = myObjectID_,
        '                                          ._idfriend = relatedField_.foreignkey,
        '                                          .sec = 1
        '                                        }
        '                                    )

        '                                End If

        '                            Next

        '                        End With

        '                        '----------------------------------- Group for my subscriptions --------------------------------------
        '                        Dim sgroup_ As New subscriptionsgroup _
        '                        With {
        '                              .subscriptions = New subscriptions With
        '                                {
        '                                 .followers = newListOfFollowers_
        '                                 }
        '                             }

        '                        '--------------------------------- Direct connection for dynamic collections --------------------------------------------
        '                        Dim toDatabaseName_ As String = Nothing : Dim toCollectionName_ As String = Nothing : Dim rol_ As Sax.rol

        '                        With _statements

        '                            rol_ = .GetDatabaseAndCollectionName(toDatabaseName_, toCollectionName_, subscriptiongroupdefined_.toresource, Nothing)

        '                            If rol_.officesuffix Then

        '                                toCollectionName_ = toCollectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

        '                                toCollectionName_ = toCollectionName_.Replace("Reg", "Vin")

        '                            End If

        '                        End With

        '                        Dim subscriptionDBDirect_ = iEnlace_.
        '                                                GetMongoClient().
        '                                                GetDatabase(toDatabaseName_).
        '                                                GetCollection(Of subscriptionsgroup)(toCollectionName_)


        '                        Dim result_ = Await subscriptionDBDirect_.UpdateOneAsync(session_,
        '                                                  filter_,
        '                                                  Builders(Of subscriptionsgroup).
        '                                                  Update.AddToSetEach("subscriptions.followers",
        '                                                  sgroup_.subscriptions.followers)).ConfigureAwait(False)

        '                        subscriptiongroupdefined_.subscriptions.followers = sgroup_.subscriptions.followers

        '                        If result_.MatchedCount = 0 Then

        '                            countUndoned_ += 1

        '                        End If

        '                    Next

        '                    If countUndoned_ > 0 Then

        '                        Await session_.AbortTransactionAsync()

        '                        Dim isDone_ As TagWatcher = Await Transaccion(documentoElectronico_, True)

        '                        If Not isDone_.Status = TagWatcher.TypeStatus.Ok Then

        '                            status_.SetError(Me, "Error upserting to MongoDB: =( ")

        '                        Else
        '                            'here falteishon ok
        '                            status_.SetOK()

        '                            status_.ObjectReturned = operacionGenerica_

        '                        End If

        '                    Else

        '                        status_.SetOK()

        '                        status_.ObjectReturned = operacionGenerica_

        '                        Await session_.CommitTransactionAsync()

        '                    End If


        '                End If

        '            Else

        '                status_.SetOK()

        '                status_.ObjectReturned = operacionGenerica_

        '                Await session_.CommitTransactionAsync()

        '            End If

        '        Catch e As Exception

        '            status_.SetError(Me, "Error writing to MongoDB: " & e.Message)

        '            session_.AbortTransactionAsync()

        '            Return status_

        '        End Try

        '    End Using

        '    Return status_

        'End Function


        'Para un solo paquete de datos
        Public Function ProduceTransaccion(ByVal estructuraRequerida_ As IEntidadDatos,
                                           ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta,
                                           ByVal clausulasLibres_ As String,
                                           Optional ByVal tipoOperacionDatos_ As IOperacionesCatalogo.TiposOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Consulta) As TagWatcher

            Dim tagwatcherlocal_ As New TagWatcher

            _registros.Clear()

            _tabla.Columns.Clear()

            _tabla.Clear()

            If estructuraRequerida_ Is Nothing Then
                'SIN DIMENSION ESPEFICICADA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            ElseIf estructuraRequerida_.Dimension = IEnlaceDatos.TiposDimension.SinDefinir Then
                'SIN DIMENSION ESPEFICICADA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            Else

                Dim indice_ As Int32 = 1

                Dim adaptador_ As IAdaptadorDatos = New AdaptadorDatos

                With adaptador_

                    'Creamos la instancia IOP para trabajar en los niveles más bajos
                    .IOperaciones = New OperacionesCatalogo

                    'Notificamos las acciones de replicación de datos potenciales
                    .IOperaciones.ReflejarEn = _destinoParaReplicacion

                    .IOperaciones.CantidadVisibleRegistros = _limiteRegistros

                    .IOperaciones.ClausulasLibres = clausulasLibres_

                    .EspacioTrabajo = _espacioTrabajo

                    .LimiteRegistros = _limiteRegistros

                    .LimiteRegistrosPresentacion = _limiteRegistrosColeccionMuestral

                    .FiltrosAvanzados = _filtrosAvanzados

                    .ModalidadPresentacion = _modalidadPresentacion

                    'Pasamos los datos de DriverDatabase y ObjetoDatos

                    .ObjetoDatos = _objetoDatos

                    .TipoConexion = _tipoConextion

                    .OrigenDatos = _origenDatos

                End With


                ':::::::::::::::::

                Dim tiempoInicialL_ As Long = Nothing

                Dim tiempoFinalL_ As Long = Nothing

                Dim tiempoTranscurridoL_ As Long = 0

                tiempoInicialL_ = ObtenerMilisegundos(DateTime.Now)

                Select Case tipoOperacionDatos_

                    Case IOperacionesCatalogo.TiposOperacionSQL.Consulta

                        adaptador_.ProcesaConsulta(estructuraRequerida_,
                                                   clausulasLibres_,
                                                   modalidadConsulta_)

                    Case Else

                        tagwatcherlocal_ = adaptador_.OperacionDatos(
                                            estructuraRequerida_,
                                            tipoOperacionDatos_,
                                            clausulasLibres_,
                                            modalidadConsulta_)

                End Select

                tiempoFinalL_ = ObtenerMilisegundos(DateTime.Now)

                tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

                _tiempoTranscurridoMilisegundos = tiempoTranscurridoL_

                'Se verifica si se proceso exitosamente
                If adaptador_.Estatus.Status = TagWatcher.TypeStatus.Ok Then

                    If tipoOperacionDatos_ = IOperacionesCatalogo.TiposOperacionSQL.Consulta Then

                        _tabla = adaptador_.Tabla

                        _registros = adaptador_.Registros

                        _iOperaciones = adaptador_.IOperaciones

                        tagwatcherlocal_.SetOK()

                    Else
                        'NOT NECCESARY
                        'tagwatcherlocal_.SetOK()

                    End If

                Else

                    'PENDIENTE DE IMPLEMENTAR
                    tagwatcherlocal_.SetError()

                End If

            End If

            Return tagwatcherlocal_

        End Function

        'Para un bulk de datos
        Public Function ProduceTransaccionesBulk(ByVal bulkRequerida_ As List(Of IEntidadDatos),
                                           ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta,
                                           ByVal clausulasLibres_ As String,
                                           ByVal tipoOperacionDatos_ As IOperacionesCatalogo.TiposOperacionSQL) As TagWatcher

            Dim tagwatcherlocal_ As New TagWatcher

            _registros.Clear()

            _tabla.Columns.Clear()

            _tabla.Clear()

            If bulkRequerida_ Is Nothing Then
                'SIN ESTRUCTURA DEFINIDA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            ElseIf bulkRequerida_(0).Dimension = IEnlaceDatos.TiposDimension.SinDefinir Then
                'SIN DIMENSION ESPEFICICADA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            Else

                'Factoría de métodos
                'Dim registroNuevo_ As IEntidadDatos

                'Select Case _granularidad

                '    Case IEnlaceDatos.TiposDimension.CuentaGastos

                '        registroNuevo_ = New CuentaGastos

                '    Case IEnlaceDatos.TiposDimension.ExtranetGeneralOperaciones

                '        registroNuevo_ = New Referencia

                '    Case IEnlaceDatos.TiposDimension.Referencias

                '        registroNuevo_ = New Referencia

                '    Case IEnlaceDatos.TiposDimension.Contenedores

                '        tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00003)

                '    Case IEnlaceDatos.TiposDimension.Fracciones

                '        registroNuevo_ = New Fracciones

                '    Case IEnlaceDatos.TiposDimension.Facturas

                '        registroNuevo_ = New Facturas

                '    Case IEnlaceDatos.TiposDimension.Mercancias

                '        registroNuevo_ = New Mercancias

                '    Case IEnlaceDatos.TiposDimension.ImpuestosPedimento

                '        tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00003)

                '    Case IEnlaceDatos.TiposDimension.PagosDeTercerosAddendas

                '        registroNuevo_ = New PagosTerceros

                '    Case IEnlaceDatos.TiposDimension.CopiasSimples

                '        registroNuevo_ = New CopiasSimples

                '    Case IEnlaceDatos.TiposDimension.FacturacionAgenciaAduanal

                '        tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00003)

                '    Case IEnlaceDatos.TiposDimension.LDAHidrocarburos

                '        registroNuevo_ = New LDAHidrocarburos

                '    Case IEnlaceDatos.TiposDimension.SeguimientoKBW

                '        registroNuevo_ = New Seguimiento

                '    Case Else

                '        tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00003)

                'End Select

                'registroNuevo_.NewRow = _tabla.NewRow()

                Dim indice_ As Int32 = 1

                Dim adaptador_ As IAdaptadorDatos = New AdaptadorDatos

                'Creamos la instancia IOP para trabajar en los niveles más bajos
                adaptador_.IOperaciones = New OperacionesCatalogo

                'Notificamos las acciones de replicación de datos potenciales
                adaptador_.IOperaciones.ReflejarEn = _destinoParaReplicacion

                adaptador_.EspacioTrabajo = _espacioTrabajo

                adaptador_.LimiteRegistros = _limiteRegistros

                adaptador_.LimiteRegistrosPresentacion = _limiteRegistrosColeccionMuestral

                adaptador_.FiltrosAvanzados = _filtrosAvanzados

                adaptador_.ModalidadPresentacion = _modalidadPresentacion

                'Pasamos los datos de DriverDatabase y ObjetoDatos

                adaptador_.ObjetoDatos = _objetoDatos

                adaptador_.TipoConexion = _tipoConextion

                adaptador_.OrigenDatos = _origenDatos

                ':::::::::::::::::

                Dim tiempoInicialL_ As Long = Nothing

                Dim tiempoFinalL_ As Long = Nothing

                Dim tiempoTranscurridoL_ As Long = 0

                tiempoInicialL_ = ObtenerMilisegundos(DateTime.Now)

                Select Case tipoOperacionDatos_

                    Case IOperacionesCatalogo.TiposOperacionSQL.Consulta

                        'tagwatcherlocal_ = adaptador_.ProcesaConsulta(estructuraRequerida_,
                        '                   clausulasLibres_,
                        '                   modalidadConsulta_)

                        'adaptador_.ProcesaConsulta(estructuraRequerida_,
                        '                           clausulasLibres_,
                        '                           modalidadConsulta_)

                        tagwatcherlocal_.SetError(IRecursosSistemas.RecursosCMF.gsol_krom_LineaBaseEnlaceDatos64, TagWatcher.ErrorTypes.C3_001_3009)

                    Case Else

                        tagwatcherlocal_ = adaptador_.OperacionDatos(bulkRequerida_,
                                            tipoOperacionDatos_,
                                            clausulasLibres_,
                                            modalidadConsulta_)



                End Select


                tiempoFinalL_ = ObtenerMilisegundos(DateTime.Now)

                tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

                _tiempoTranscurridoMilisegundos = tiempoTranscurridoL_

                'MsgBox("Tiempo transcurrido: " & tiempoTranscurrido_.ToString & " Segundos, (" & tiempoTranscurridoL_.ToString & ") milisegundos.")

                ':::::::::::::::::::

                'Se verifica si se proceso exitosamente
                If adaptador_.Estatus.Status = TagWatcher.TypeStatus.Ok Then

                    If tipoOperacionDatos_ = IOperacionesCatalogo.TiposOperacionSQL.Consulta Then

                        _tabla = adaptador_.Tabla

                        _registros = adaptador_.Registros

                        _iOperaciones = adaptador_.IOperaciones

                        tagwatcherlocal_.SetOK()

                    Else
                        'NOT NECCESARY
                        'tagwatcherlocal_.SetOK()

                    End If

                Else

                    'PENDIENTE DE IMPLEMENTAR
                    tagwatcherlocal_.SetError()

                End If

            End If

            Return tagwatcherlocal_

        End Function

        Private Function ObtenerMilisegundos(ByVal fecha_ As DateTime) As Long

            Dim respuesta_ As Long = 0

            Dim fechaAuxiliar_ As Date = New Date(1970, 1, 1, 0, 0, 0, 0)

            respuesta_ = DateDiff(DateInterval.Second, fechaAuxiliar_, fecha_) * 1000 + fecha_.Millisecond

            Return respuesta_

        End Function

        Public Function ProduceEstructuraCompleta(ByVal estructuraRequerida_ As IEntidadDatos,
                                                  ByVal nombreEntidad_ As String,
                                                    ByVal clausulasLibres_ As String,
                                                    ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher

            Dim tagwatcherlocal_ As New TagWatcher

            If estructuraRequerida_ Is Nothing Then
                'SIN ESTRUCTURA DEFINIDA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            ElseIf estructuraRequerida_.Dimension = IEnlaceDatos.TiposDimension.SinDefinir Then
                'SIN DIMENSION ESPEFICICADA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            Else

                Dim adaptador_ As IAdaptadorDatos = New AdaptadorDatos

                adaptador_.EspacioTrabajo = _espacioTrabajo

                adaptador_.LimiteRegistros = 1

                adaptador_.ModalidadPresentacion = _modalidadPresentacion

                adaptador_.GeneraEstructuraCompleta(estructuraRequerida_,
                                                    nombreEntidad_,
                                                      clausulasLibres_,
                                                      modalidadConsulta_)

                'Se verifica si se proceso exitosamente
                If adaptador_.Estatus.Status = TagWatcher.TypeStatus.Ok Then

                    tagwatcherlocal_.SetOK()

                    tagwatcherlocal_.ObjectReturned = adaptador_.Estatus.ObjectReturned

                Else

                    'PENDIENTE DE IMPLEMENTAR
                    tagwatcherlocal_.SetError()

                End If

            End If

            Return tagwatcherlocal_

        End Function

        Public Function ProduceEstructuraResultados(ByRef estructuraRequerida_ As IEntidadDatos,
                                                    ByVal clausulasLibres_ As String,
                                                    ByVal modalidadConsulta_ As IOperacionesCatalogo.ModalidadesConsulta) As TagWatcher

            Dim tagwatcherlocal_ As New TagWatcher

            If estructuraRequerida_ Is Nothing Then
                'SIN ESTRUCTURA DEFINIDA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            ElseIf estructuraRequerida_.Dimension = IEnlaceDatos.TiposDimension.SinDefinir Then
                'SIN DIMENSION ESPEFICICADA

                tagwatcherlocal_.SetError(TagWatcher.ErrorTypes.C5_013_00002)

            Else

                Dim adaptador_ As IAdaptadorDatos = New AdaptadorDatos

                adaptador_.EspacioTrabajo = _espacioTrabajo

                adaptador_.LimiteRegistros = 1

                adaptador_.ModalidadPresentacion = _modalidadPresentacion

                'Suspendida temporalmente
                'adaptador_.GeneraEstructuraResultados(estructuraRequerida_,
                '                                      clausulasLibres_,
                '                                      modalidadConsulta_)

                adaptador_.GeneraEstructuraResultados(estructuraRequerida_,
                                      clausulasLibres_,
                                      modalidadConsulta_)

                'Se verifica si se proceso exitosamente
                If adaptador_.Estatus.Status = TagWatcher.TypeStatus.Ok Then

                    tagwatcherlocal_.SetOK()

                    tagwatcherlocal_.ObjectReturned = adaptador_.Estatus.ObjectReturned

                Else

                    'PENDIENTE DE IMPLEMENTAR
                    tagwatcherlocal_.SetError()

                End If

            End If

            Return tagwatcherlocal_

        End Function

#End Region

    End Class

End Namespace