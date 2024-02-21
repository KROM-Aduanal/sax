Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.basededatos
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports gsol
Imports gsol.basededatos.ConexionesNoSQL
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Operaciones
Imports System.Threading.Tasks
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions.TagWatcher
Imports Rec.Globals.Utils
Imports Syn.Utils

Namespace gsol.krom
    Public Class EnlaceDatos
        Implements IEnlaceDatos, ICloneable, IDisposable

#Region "Enum"

#End Region

#Region "Atributos"

        Private _dimension As IEnlaceDatos.TiposDimension

        Private _convertirCamposNulos As Boolean

        Private _espacioTrabajo As IEspacioTrabajo ' Evaluar, 

        Private _limiteResultados As Int32

        Private _milisegundosLatenciaMaxima As Int32

        Private _tiempoTranscurridoMilisegundos As Long

        Private _tipoRespuestaRequerida As IEnlaceDatos.FormatosRespuesta

        Private _lineaBaseEnlaceDatos As LineaBaseEnlaceDatos '20092018

        Private _tagWatcher As TagWatcher

        Private _granularidad As IEnlaceDatos.TiposDimension

        Private _modalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta

        Private _clausulasLibres As String

        Private _tipoGestionOperativa As IEnlaceDatos.TiposGestionOperativa

        'Private _limiteRegistrosColeccionMuestral As Int32

        'Ahora incluiremos el manifiesto de carga de Sax para obtener recursos de ahí mediante el singleton
        Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

#End Region

#Region "Constructores"

        Sub New()

            'Parámetros por defecto
            _convertirCamposNulos = True

            _espacioTrabajo = Nothing

            _limiteResultados = 1000

            _milisegundosLatenciaMaxima = 5000

            _tipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.ObjetoSimple

            _lineaBaseEnlaceDatos = New LineaBaseEnlaceDatos

            _tagWatcher = New TagWatcher

            _tipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativo

        End Sub

#End Region

#Region "Metodos"

        Private Function ProduceBusquedaGeneralDocumento(ByVal objetoDatos_ As DocumentoElectronico,
                                          ByVal IdUnicoSeccion_ As Integer,
                                          ByVal IdUnicoCampo_ As Integer,
                                          ByVal valor_ As String) As TagWatcher


            Dim operacionesNodo_ = New OperacionesNodos

            operacionesNodo_.CrearPartidasDocumentoElectronico(objetoDatos_)

            Dim rutaNodo_ = operacionesNodo_.ObtenerRutaCampo(objetoDatos_, IdUnicoSeccion_, IdUnicoCampo_)


            Dim partsRutaNodo_ As String() = rutaNodo_.Split(".")

            Dim pipeline_ As New List(Of BsonDocument)

            Dim raiz_ As New BsonDocument() From {
                    {
                        "$addFields", New BsonDocument From {
                                {"FolioDocumento", "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento"},
                                {"NombreCliente", "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente"},
                                {"Documento", "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." & partsRutaNodo_(0).ToString()}
                        }
                    }
                }

            Dim plancharDatosRaiz_ = New BsonDocument From {
                {
                        "$unwind", New BsonDocument From {
                                    {"path", "$Documento"},
                                    {"preserveNullAndEmptyArrays", True}
                        }
                    }
                }


            pipeline_.Add(raiz_)

            pipeline_.Add(plancharDatosRaiz_)

            partsRutaNodo_ = partsRutaNodo_.Skip(1).ToArray

            Dim indice_ = 1

            For Each rutaActual_ As String In partsRutaNodo_

                Dim agregarCampo_ = New BsonDocument From {
                    {
                        "$addFields", New BsonDocument From {
                                    {"Nodo" & indice_, If(indice_ = 1, "$Documento.Nodos", "$Nodo" & (indice_ - 1) & ".Nodos")}
                        }
                    }
                }

                Dim plancharDatos_ = New BsonDocument From {
                {
                        "$unwind", New BsonDocument From {
                                    {"path", "$Nodo" & indice_.ToString()},
                                    {"preserveNullAndEmptyArrays", True}
                        }
                    }
                }

                pipeline_.Add(agregarCampo_)

                pipeline_.Add(plancharDatos_)

                If indice_ < partsRutaNodo_.Count Then

                    indice_ += 1

                End If

            Next

            Dim condicionesConsulta_ = New BsonDocument From {
                    {
                        "$match", New BsonDocument From {
                                    {"estado", 1},
                                    {"Nodo" & indice_ & ".TipoNodo", 2},
                                    {"Nodo" & indice_ & ".IDUnico", IdUnicoCampo_},
                                    {"$or", New BsonArray From {
                                                New BsonDocument From {
                                                     {"Nodo" & indice_ & ".Valor", New BsonDocument From {
                                                                {"$regex", valor_},
                                                                {"$options", "i"}
                                                            }
                                                     }
                                                },
                                                New BsonDocument From {
                                                     {"Nodo" & indice_ & ".ValorPresentacion", New BsonDocument From {
                                                                {"$regex", valor_},
                                                                {"$options", "i"}
                                                            }
                                                     }
                                                }
                                            }
                                    }
                        }
                    }
                }


            Dim camposConsulta_ = New BsonDocument From {
                    {
                        "$project", New BsonDocument From {
                                    {"_id", 1},
                                    {"FolioOperacion", 1},
                                    {"FolioDocumento", 1},
                                    {"NombreCliente", 1},
                                    {"NombreCampo", "$Nodo" & indice_.ToString() & ".Nombre"},
                                    {"IDCampo", "$Nodo" & indice_.ToString() & ".IDUnico"},
                                    {
                                        "CampoValor", New BsonDocument From {
                                            {
                                                "$ifNull", New BsonArray() From {
                                                    "$Nodo" & indice_.ToString() & ".Valor", "$Nodo" & indice_.ToString() & ".ValorPresentacion"
                                                }
                                            }
                                        }
                                    }
                        }
                    }
                }

            pipeline_.Add(condicionesConsulta_)

            pipeline_.Add(camposConsulta_)

            Dim limiteRegistros_ = New BsonDocument From {
                {
                    "$limit", _limiteResultados
                }
            }


            pipeline_.Add(limiteRegistros_)


            Dim status_ As New TagWatcher

            'Dim tiempoInicialL_ As Long = Nothing

            'Dim tiempoFinalL_ As Long = Nothing

            'Dim tiempoTranscurridoL_ As Long = 0

            'tiempoInicialL_ = ObtenerMilisegundos(DateTime.Now)


            'Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operacionesDB_ As IMongoCollection(Of OperacionGenerica) = Me.GetMongoCollection(Of OperacionGenerica)(objetoDatos_.GetType.Name)

            Try

                Dim objectResult_ = operacionesDB_.Aggregate(Of BsonDocument)(pipeline_).ToList

                Dim listaResultados_ = New List(Of Object)

                For Each stat As BsonDocument In objectResult_

                    listaResultados_.Add(New Dictionary(Of Object, Object) From {
                        {"ID", stat.GetElement("_id").Value.ToString},
                        {"folioOperacion", stat.GetElement("FolioOperacion").Value.ToString},
                        {"folioDocumento", stat.GetElement("FolioDocumento").Value.ToString},
                        {"propietario", stat.GetElement("NombreCliente").Value.ToString},
                        {"valorOperacion", stat.GetElement("CampoValor").Value.ToString}
                    })

                Next

                status_.SetOK()

                status_.ObjectReturned = listaResultados_

            Catch e As Exception

                status_.SetError(Me, "Error writing to MongoDB: " & e.Message)

            End Try

            'tiempoFinalL_ = ObtenerMilisegundos(DateTime.Now)

            'tiempoTranscurridoL_ = tiempoFinalL_ - tiempoInicialL_

            '_tiempoTranscurridoMilisegundos = tiempoTranscurridoL_

            'iEnlace_.Dispose()

            Return status_


        End Function

        Public Function BusquedaGeneralDocumento(objetoDatos_ As DocumentoElectronico,
                                                 IdUnicoSeccion_ As Integer,
                                                 IdUnicoCampo_ As Integer,
                                                 valor_ As String) As TagWatcher _
            Implements IEnlaceDatos.BusquedaGeneralDocumento

            _tagWatcher.Clear()

            With _lineaBaseEnlaceDatos

                .LimiteRegistros = _limiteResultados

                .Registros.Clear()

                _dimension = IEnlaceDatos.TiposDimension.SinDefinir

                If Not _espacioTrabajo Is Nothing Then

                    _tagWatcher = ProduceBusquedaGeneralDocumento(objetoDatos_,
                                                            IdUnicoSeccion_,
                                                            IdUnicoCampo_,
                                                            valor_)

                    'Tiempo de respuesta de la operación a la base de datos
                    _tiempoTranscurridoMilisegundos = .TiempoTranscurridoMilisegundos

                Else

                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            End With

            Return _tagWatcher

        End Function

        Public Function AgregarDatos(ByVal bulkDatos_ As List(Of IEntidadDatos),
                              Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher Implements IEnlaceDatos.AgregarDatos

            _tagWatcher.Clear()

            _lineaBaseEnlaceDatos.LimiteRegistros = _limiteResultados

            _lineaBaseEnlaceDatos.Registros.Clear()

            'Seleccionamos un objeto de la lista para determinar la dimension
            If bulkDatos_.Count >= 1 Then

                _dimension = bulkDatos_(0).Dimension

            Else

                Return New TagWatcher(0, Me, "No se encontraron datos en la lista", TagWatcher.ErrorTypes.C3_001_3010)

            End If

            If Not _espacioTrabajo Is Nothing Then

                'ProduceTransaccion(estructuraRequerida_)

                _tagWatcher = _lineaBaseEnlaceDatos.ProduceTransaccionesBulk(bulkDatos_,
                                                                   _modalidadConsulta,
                                                                   _clausulasLibres,
                                                                   IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                'Tiempo de respuesta de la operación a la base de datos
                _tiempoTranscurridoMilisegundos = _lineaBaseEnlaceDatos.TiempoTranscurridoMilisegundos


            Else

                _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

            End If

            Return _tagWatcher

        End Function

        Public Function AgregarDatos(ByVal objetoDatos_ As DocumentoElectronico,
                                     Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher _
            Implements IEnlaceDatos.AgregarDatos

            _tagWatcher.Clear()

            'MOP apagado temporalmente 22022020
            Dim operacionNodo_ = New OperacionesNodos(_espacioTrabajo)

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In objetoDatos_.EstructuraDocumento.Parts

                For Each parts_ As Nodo In parDatos_.Value

                    ' operacionNodo_.AgregaRegistroMovimientos(nodos_:=parts_.Nodos)

                Next

            Next

            With _lineaBaseEnlaceDatos

                .LimiteRegistros = _limiteResultados

                .Registros.Clear()

                _dimension = IEnlaceDatos.TiposDimension.SinDefinir

                If Not _espacioTrabajo Is Nothing Then

                    '_tagWatcher = .ProduceTransaccionDocumento(objetoDatos_,
                    '                                  _modalidadConsulta,
                    '                                  _clausulasLibres,
                    '                                  IOperacionesCatalogo.TiposOperacionNoSQL.Insercion)

                    'Tiempo de respuesta de la operación a la base de datos
                    _tiempoTranscurridoMilisegundos = .TiempoTranscurridoMilisegundos

                Else

                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            End With

            Return _tagWatcher

        End Function


        Public Sub PreparaMetaDatos(ByRef documentoOriginal_ As DocumentoElectronico)

            Dim operacionNodoNoSQL_ = New OperacionesNodosNoSQL(_espacioTrabajo)

            Dim listaCampos_ As New List(Of CampoGenerico)

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In documentoOriginal_.EstructuraDocumento.Parts

                For Each parts_ As Nodo In parDatos_.Value

                    Dim listaCamposHijo_ As New List(Of CampoGenerico)

                    If Not IsNothing(parts_.Nodos) Then

                        listaCamposHijo_ = operacionNodoNoSQL_.PreparaMetaDatos(parts_.Nodos)

                        If Not IsNothing(listaCamposHijo_) Then

                            If listaCamposHijo_.Count > 0 Then

                                For Each campo_ As CampoGenerico In listaCamposHijo_

                                    listaCampos_.Add(campo_)

                                Next

                            End If

                        End If

                    End If

                Next

            Next

            If listaCampos_.Count > 0 Then

                documentoOriginal_.Metadatos = listaCampos_

            End If

        End Sub

        Public Async Function AgregarDatosDocumento(ByVal objetoDatos_ As DocumentoElectronico,
                                                    Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing,
                                                    Optional ByVal session_ As IClientSessionHandle = Nothing) As Threading.Tasks.Task(Of TagWatcher) _
            Implements IEnlaceDatos.AgregarDatosDocumento

            _tagWatcher.Clear()

            'MOP apagado temporalmente 22022020
            Dim operacionNodo_ = New OperacionesNodos(_espacioTrabajo)

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In objetoDatos_.EstructuraDocumento.Parts

                For Each parts_ As Nodo In parDatos_.Value

                    'operacionNodo_.AgregaRegistroMovimientos(nodos_:=parts_.Nodos)

                Next

            Next

            With _lineaBaseEnlaceDatos

                .LimiteRegistros = _limiteResultados

                .Registros.Clear()

                _dimension = IEnlaceDatos.TiposDimension.SinDefinir

                If Not _espacioTrabajo Is Nothing Then

                    'Asignar metadatos
                    PreparaMetaDatos(objetoDatos_)

                    _tagWatcher = Await .ProduceTransaccionDocumento(objetoDatos_,
                                                      _modalidadConsulta,
                                                      _clausulasLibres,
                                                      IOperacionesCatalogo.TiposOperacionNoSQL.Insercion,
                                                      session_)

                    'Tiempo de respuesta de la operación a la base de datos
                    _tiempoTranscurridoMilisegundos = .TiempoTranscurridoMilisegundos

                Else

                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            End With

            Return _tagWatcher

        End Function

        Public Function AgregarDatos(ByVal objetoDatos_ As IEntidadDatos,
                                     Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing,
                                     Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher _
            Implements IEnlaceDatos.AgregarDatos

            'Type.name ="ConstructorPedimentoNormal"
            'If Not objetoDatos_.GetType.Name Is "EntidadDatos" Then
            If objetoDatos_.Dimension = IEnlaceDatos.TiposDimension.DocumentoElectronico Or
               objetoDatos_.GetType.BaseType.Name = "EntidadDatosDocumento" Then

                Dim respuesta_ As Task(Of TagWatcher) = AgregarDatosDocumento(objetoDatos_, espacioTrabajo_, session_)

                Return respuesta_.Result

            End If

            _tagWatcher.Clear()

            With _lineaBaseEnlaceDatos

                .LimiteRegistros = _limiteResultados

                .Registros.Clear()

                'estructuraRequerida_.Dimension = estructuraRequerida_.Dimension

                _dimension = objetoDatos_.Dimension

                If Not _espacioTrabajo Is Nothing Then

                    _tagWatcher = .ProduceTransaccion(objetoDatos_,
                                                      _modalidadConsulta,
                                                      _clausulasLibres,
                                                      IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    'Tiempo de respuesta de la operación a la base de datos
                    _tiempoTranscurridoMilisegundos = .TiempoTranscurridoMilisegundos

                Else

                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            End With

            Return _tagWatcher

        End Function

        Public Function NotificarSubscriptores(ByVal recurso_ As String,
                                               ByVal iddocumento_ As ObjectId,
                                               ByVal documentoelectronico_ As DocumentoElectronico,
                                               Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher Implements IEnlaceDatos.NotificarSubscriptores

            Dim tagwacher_ As New TagWatcher

            Using controladorSubscripciones_ As ControladorSubscripciones = New ControladorSubscripciones

                With controladorSubscripciones_

                    If .LeerSuscriptores(recurso_, iddocumento_).Count Then

                        tagwacher_ = .DifusionDatos(documentoelectronico_, session_)

                    Else

                        tagwacher_ = New TagWatcher(1)

                    End If

                End With

            End Using

            Return tagwacher_

        End Function

        Public Function EliminarSuscripciones(ByVal iddocumento_ As ObjectId,
                                              ByVal subscriptionsgroup_ As List(Of subscriptionsgroup),
                                              Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher Implements IEnlaceDatos.EliminarSuscripciones

            For Each subscriptionsgroup As subscriptionsgroup In subscriptionsgroup_

                Using controladorSubscripciones_ As ControladorSubscripciones = New ControladorSubscripciones

                    With controladorSubscripciones_

                        If Not .EliminarSuscripciones(subscriptionsgroup.toresource, iddocumento_, session_).Status = TypeStatus.Ok Then

                            Return New TagWatcher(0)

                        End If

                    End With

                End Using

            Next

            Return New TagWatcher(1)

        End Function

        Public Function FirmarDocumento(ByVal recurso_ As String,
                                        ByVal iddocumento_ As ObjectId,
                                        ByVal claveusuario_ As String,
                                        Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher Implements IEnlaceDatos.FirmarDocumento

            Using controladorFirmaElectronica_ As New ControladorFirmaElectronica()

                Return controladorFirmaElectronica_.FirmarDocumento(recurso_, iddocumento_, claveusuario_, session_:=session_)

            End Using

        End Function

        Public Function AnalizaDiferencias(ByVal documentoNuevo_ As DocumentoElectronico,
                                           ByVal documentoOriginal_ As DocumentoElectronico) As List(Of DocumentoElectronicoObjetoActualizador) Implements IEnlaceDatos.AnalizaDiferencias

            Dim operacionNodoNoSQL_ = New OperacionesNodosNoSQL(_espacioTrabajo)

            Dim listaActualizaciones_ As New List(Of DocumentoElectronicoObjetoActualizador)

            Dim rutaEnsobretado_ As String = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente."

            'With documentoOriginal_

            '    If Not .FolioOperacion = documentoOriginal_.FolioOperacion Then : listaActualizaciones_.Add(rutaEnsobretado_ & "FolioOperacion->" & .FolioOperacion & "|" & "FolioOperacion->" & .FolioOperacion & "|Texto") : End If

            '    'If Not .TipoDocumentoElectronico = documentoNuevo_.TipoDocumentoElectronico Then : listaActualizaciones_.Add(rutaEnsobretado_ & "TipoDocumentoElectronico->" & .TipoDocumentoElectronico & "|" & "TipoDocumentoElectronico->" & .TipoDocumentoElectronico & "|Entero") : End If

            '    If Not .NombreCliente = documentoOriginal_.NombreCliente Then : listaActualizaciones_.Add(rutaEnsobretado_ & "NombreCliente->" & .NombreCliente & "|" & "NombreCliente->" & .NombreCliente & "|Texto") : End If

            '    If Not .IdCliente = documentoOriginal_.IdCliente Then : listaActualizaciones_.Add(rutaEnsobretado_ & "IdCliente->" & .IdCliente & "|" & "IdCliente->" & .IdCliente & "|Entero") : End If

            '    'If Not .Id = documentoNuevo_.Id Then : listaActualizaciones_.Add(rutaEnsobretado_ & "Id->" & .Id & "|" & "Id->" & .Id & "|Texto") : End If

            '    If Not .IdDocumento = documentoOriginal_.IdDocumento Then : listaActualizaciones_.Add(rutaEnsobretado_ & "IdDocumento=" & .IdDocumento & "|" & "IdDocumento->" & .IdDocumento & "|Entero") : End If

            '    If Not .FolioDocumento = documentoOriginal_.FolioDocumento Then : listaActualizaciones_.Add(rutaEnsobretado_ & "FolioDocumento=" & .FolioDocumento & "|" & "FolioDocumento->" & .FolioDocumento & "|Texto") : End If

            '    If Not .FechaCreacion = documentoOriginal_.FechaCreacion Then : listaActualizaciones_.Add(rutaEnsobretado_ & "FechaCreacion=" & .FechaCreacion & "|" & "FechaCreacion->" & .FechaCreacion & "|Fecha") : End If

            '    If Not .UsuarioGenerador = documentoOriginal_.UsuarioGenerador Then : listaActualizaciones_.Add(rutaEnsobretado_ & "UsuarioGenerador=" & .UsuarioGenerador & "|" & "UsuarioGenerador->" & .UsuarioGenerador & "|Texto") : End If

            '    If Not .EstatusDocumento = documentoOriginal_.EstatusDocumento Then : listaActualizaciones_.Add(rutaEnsobretado_ & "EstatusDocumento=" & .EstatusDocumento & "|" & "EstatusDocumento->" & .EstatusDocumento & "|Texto") : End If

            'End With

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In documentoNuevo_.EstructuraDocumento.Parts

                Dim contadorNodos_ As Int32 = 0

                For Each parts_ As Nodo In parDatos_.Value

                    Dim rutaActualizacion_ = rutaEnsobretado_ & "Documento.Parts." & parDatos_.Key.ToString & "."

                    Dim listaActualizacionesHijo_ As New List(Of DocumentoElectronicoObjetoActualizador)

                    If Not IsNothing(parts_.Nodos) Then

                        listaActualizacionesHijo_ = operacionNodoNoSQL_.AnalizaDiferenciasNodos(nodosNuevo_:=parts_.Nodos,
                                                                                                nodosOriginal_:=documentoOriginal_.EstructuraDocumento.Parts(parDatos_.Key.ToString)(contadorNodos_).Nodos)

                        If Not IsNothing(listaActualizacionesHijo_) Then

                            If listaActualizacionesHijo_.Count > 0 Then

                                For Each actualizacion_ As DocumentoElectronicoObjetoActualizador In listaActualizacionesHijo_

                                    listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                        .RutaActualizacion = rutaActualizacion_ & contadorNodos_.ToString & ".Nodos." & actualizacion_.RutaActualizacion,
                                        .Valor = actualizacion_.Valor,
                                        .TipoDato = actualizacion_.TipoDato,
                                        .PropiedadActualizar = actualizacion_.PropiedadActualizar,
                                        .TipoNodo = actualizacion_.TipoNodo
                                    })

                                Next

                            End If

                        End If

                    End If

                    contadorNodos_ += 1

                Next

            Next

            'test
            'movi acuse de valor un par de metodos, revisar con sergio porque chayo me paso su controlador de factura
            If Not documentoOriginal_.DocumentosAsociados Is Nothing Then

                If documentoOriginal_.DocumentosAsociados.Equals(documentoNuevo_.DocumentosAsociados) = False Then

                    listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                        .RutaActualizacion = "Borrador.Folder.DocumentosAsociados",
                        .Valor = documentoNuevo_.DocumentosAsociados,
                        .TipoDato = Componentes.Campo.TiposDato.Documento,
                        .PropiedadActualizar = Nothing,
                        .TipoNodo = TiposNodo.SinDefinir
                    })

                End If

            End If


            Return listaActualizaciones_

        End Function


        Public Function GetMongoCollection(Of T)(Optional ByVal resourceName_ As String = Nothing,
                                                 Optional ByVal rootid_ As Int32? = Nothing) As IMongoCollection(Of T) Implements IEnlaceDatos.GetMongoCollection

            Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                If resourceName_ Is Nothing Then
                    If GetType(T).Name <> "OperacionGenerica" Then
                        resourceName_ = GetType(T).Name
                    End If
                End If

                Return conexionesNoSQL_.GetMongoCollection(Of T)(GetMongoClient(), resourceName_, rootid_)

            End Using

        End Function

        Public Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
                                                 Optional ByVal resourceName_ As String = Nothing,
                                                 Optional ByVal rootid_ As Int32? = Nothing) As IMongoCollection(Of T) Implements IEnlaceDatos.GetMongoCollection

            Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                Return conexionesNoSQL_.GetMongoCollection(Of T)(imongoClient_, resourceName_, rootid_)

            End Using

        End Function

        Public Function GetMongoClient(Optional ByVal settingsType_ As Sax.SaxStatements.SettingTypes = Sax.SaxStatements.SettingTypes.Projects) As IMongoClient _
                                      Implements IEnlaceDatos.GetMongoClient

            Using conexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

                Return conexionesNoSQL_.GetMongoClient()

            End Using

        End Function

        'Public Async Sub CambiosDetectadosAsync(Of T)(ByVal listaActualizaciones_ As List(Of DocumentoElectronicoObjetoActualizador),
        Public Sub CambiosDetectadosAsync(Of T)(ByVal listaActualizaciones_ As List(Of DocumentoElectronicoObjetoActualizador),
                                                ByVal referencia_ As String,
                                                ByVal id_ As ObjectId,
                                                ByVal nombreRecursoSolicitante_ As String,
                                                Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                Implements IEnlaceDatos.CambiosDetectadosAsync

            If Not listaActualizaciones_ Is Nothing And listaActualizaciones_.Count > 0 Then

                '**********************************************************************************

                Dim operacionesDB_ = GetMongoCollection(Of T)(nombreRecursoSolicitante_)

                '**********************************************************************************

                Dim filter = Builders(Of T).Filter.Eq(Of ObjectId)("Id", id_)

                Dim updateDefinition = New List(Of UpdateDefinition(Of T))()

                Dim listaDescriptivaActualizaciones_ As String = Nothing

                For Each actualizacion_ As DocumentoElectronicoObjetoActualizador In listaActualizaciones_

                    With updateDefinition

                        listaDescriptivaActualizaciones_ = listaDescriptivaActualizaciones_ & " " & Chr(13) '& campo_.Nombre

                        Select Case actualizacion_.TipoDato

                            Case Componentes.Campo.TiposDato.Texto

                                .Add(Builders(Of T).Update.Set(Of String)(actualizacion_.RutaActualizacion & "." & actualizacion_.PropiedadActualizar, actualizacion_.Valor))

                            Case Componentes.Campo.TiposDato.Entero

                                .Add(Builders(Of T).Update.Set(Of Int32)(actualizacion_.RutaActualizacion & "." & actualizacion_.PropiedadActualizar, actualizacion_.Valor))

                            Case Componentes.Campo.TiposDato.Booleano

                                .Add(Builders(Of T).Update.Set(Of Boolean)(actualizacion_.RutaActualizacion & "." & actualizacion_.PropiedadActualizar, actualizacion_.Valor))

                            Case Componentes.Campo.TiposDato.Fecha

                                .Add(Builders(Of T).Update.Set(Of Date)(actualizacion_.RutaActualizacion & "." & actualizacion_.PropiedadActualizar, actualizacion_.Valor))

                            Case Componentes.Campo.TiposDato.Real

                                .Add(Builders(Of T).Update.Set(Of Double)(actualizacion_.RutaActualizacion & "." & actualizacion_.PropiedadActualizar, actualizacion_.Valor))

                            Case Componentes.Campo.TiposDato.IdObject

                                .Add(Builders(Of T).Update.Set(Of ObjectId)(actualizacion_.RutaActualizacion & "." & actualizacion_.PropiedadActualizar, actualizacion_.Valor))

                            Case Componentes.Campo.TiposDato.Documento

                                'Dim aux_ As Object = CType(actualizacion_.Valor, Object)

                                Select Case actualizacion_.TipoNodo

                                    Case TiposNodo.Partida

                                        Dim aux_ As PartidaGenerica = CType(actualizacion_.Valor, PartidaGenerica)

                                        .Add(Builders(Of T).Update.Set(Of PartidaGenerica)(actualizacion_.RutaActualizacion, aux_))

                                    Case TiposNodo.Seccion

                                    Case TiposNodo.SinDefinir

                                        Dim aux_ As List(Of DocumentoAsociado) = CType(actualizacion_.Valor, List(Of DocumentoAsociado))

                                        .Add(Builders(Of T).Update.Set(Of List(Of DocumentoAsociado))(actualizacion_.RutaActualizacion, aux_))

                                End Select

                                '.Add(Builders(Of T).Update.Set(Of Object)(actualizacion_.RutaActualizacion, aux_))

                            Case Componentes.Campo.TiposDato.SinDefinir

                                _tagWatcher.SetError(Me, "No se encontró el tipo de datos definido")

                            Case Else

                                _tagWatcher.SetError(Me, "No se encontró el tipo de datos definido")

                        End Select

                    End With

                Next

                Dim combinedUpdate = Builders(Of T).Update.Combine(updateDefinition)

                'Await operacionesDB_.UpdateOneAsync(session_, filter, combinedUpdate).ConfigureAwait(False)
                'operacionesDB_.UpdateOneAsync(session_, filter, combinedUpdate)

                operacionesDB_.UpdateOneAsync(session_, filter, combinedUpdate).ConfigureAwait(False)

            End If

        End Sub
        Private Async Function ComparaDocumentosAsync(Of tipo_)(ByVal object1_ As Object,
                                                                ByVal iEnlaceDatos_ As IEnlaceDatos,
                                                                ByVal documentoElectronico_ As DocumentoElectronico) As Task(Of TagWatcher) Implements IEnlaceDatos.ComparaDocumentosAsync
            Dim respuesta_ As New TagWatcher

            Using iEnlaceDatos_

                With documentoElectronico_

                    Using _entidadDatos As IEntidadDatos = object1_

                        Dim s_ As IEntidadDatos = Activator.CreateInstance(Of tipo_) '(New ConstructorCliente(True))

                        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

                        Dim operacionesDB_ As IMongoCollection(Of OperacionGenerica) = iEnlace_.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

                        Dim documentosEncontrados_ As New List(Of OperacionGenerica)

                        Dim filtro_ As BsonDocument = New BsonDocument() _
                            .Add("FolioOperacion", documentoElectronico_.FolioOperacion)

                        documentosEncontrados_ = operacionesDB_.Find(filtro_).ToListAsync().Result

                        s_ = documentosEncontrados_(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone(Activator.CreateInstance(Of tipo_)) '(New ConstructorCliente(True))


                        Dim listaActualizaciones_ As New List(Of DocumentoElectronicoObjetoActualizador)

                        listaActualizaciones_ = iEnlaceDatos_.AnalizaDiferencias(s_, _entidadDatos)

                        If listaActualizaciones_.Count > 0 Then

                            respuesta_.SetOKInfo(Me, "¡Se descargaron " & listaActualizaciones_.Count.ToString & " actualizaciones de la nube :)!")

                            respuesta_.ObjectReturned = documentosEncontrados_(0)

                        Else

                            respuesta_.SetOKBut(Me, "¡Hey! No hay cambios...")

                        End If

                    End Using

                End With

            End Using

            Return respuesta_

        End Function


        Public Async Function ActualizarDocumento(Of T)(ByVal id_ As ObjectId,
                                                        ByVal enlaceDatos_ As IEnlaceDatos,
                                                        ByVal entidadDatos_ As IEntidadDatos,
                                                        ByVal datosCargados_ As IEntidadDatos,
                                                        Optional ByVal session_ As IClientSessionHandle = Nothing) As Task(Of TagWatcher) Implements IEnlaceDatos.ActualizarDocumento

            Dim respuesta_ As TagWatcher = CompararActualizar(Of T)(id_, datosCargados_, entidadDatos_, Nothing, session_)

            If respuesta_.Status = TagWatcher.TypeStatus.OkBut Then

                'Dim revisionRemota_ As TagWatcher =
                '                ComparaDocumentosAsync(Of T) _
                '                (entidadDatos_, enlaceDatos_, datosCargados_).Result

                'Return revisionRemota_
                respuesta_.SetOK()

            End If

            Return respuesta_

        End Function

        Public Function CompararActualizar(Of T)(ByVal id_ As ObjectId,
                                                 ByVal instantaneaActual_ As DocumentoElectronico,
                                                 ByVal instantaneaAnterior_ As DocumentoElectronico,
                                                 Optional ByVal collectionType_ As Sax.SaxStatements.CollectionTypes = Sax.SaxStatements.CollectionTypes.OO,
                                                 Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                 As Wma.Exceptions.TagWatcher Implements IEnlaceDatos.CompararActualizar

            Dim respuesta_ As New TagWatcher

            If Not instantaneaActual_ Is Nothing Then

                Dim listaActualizaciones_ As New List(Of DocumentoElectronicoObjetoActualizador)

                listaActualizaciones_ = AnalizaDiferencias(instantaneaActual_,
                                                           instantaneaAnterior_)

                If listaActualizaciones_.Count > 0 Then

                    CambiosDetectadosAsync(Of T)(listaActualizaciones_,
                                                 instantaneaActual_.FolioOperacion,
                                                 id_,
                                                 instantaneaActual_.GetType.Name,
                                                 session_)

                    respuesta_.SetOKInfo(Me, "¡Se actualizaron con éxito " & listaActualizaciones_.Count & " elementos!", 1)

                Else

                    respuesta_.SetOKBut(Me, "¡Hey! No hay cambios.")

                End If

            Else
                respuesta_.SetError(Me, "No se encontró instancia")

            End If

            Return respuesta_

        End Function

        Public Function EliminarDatos(ByVal objetoDatos_ As IEntidadDatos,
                                      Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher _
                                  Implements IEnlaceDatos.EliminarDatos

            _tagWatcher.Clear()

            With _lineaBaseEnlaceDatos

                .LimiteRegistros = _limiteResultados

                .Registros.Clear()

                _dimension = objetoDatos_.Dimension

                If Not _espacioTrabajo Is Nothing Then

                    _tagWatcher = .ProduceTransaccion(objetoDatos_,
                                                      _modalidadConsulta,
                                                      _clausulasLibres,
                                                      IOperacionesCatalogo.TiposOperacionSQL.Eliminar)

                    'Tiempo de respuesta de la operación a la base de datos
                    _tiempoTranscurridoMilisegundos = .TiempoTranscurridoMilisegundos

                Else

                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            End With

            Return _tagWatcher

        End Function

        Public Function ModificarDatos(ByVal objetoDatos_ As IEntidadDatos,
                                       Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher _
                                   Implements IEnlaceDatos.ModificarDatos


            _tagWatcher.Clear()

            With _lineaBaseEnlaceDatos

                .LimiteRegistros = _limiteResultados

                .Registros.Clear()

                'estructuraRequerida_.Dimension = estructuraRequerida_.Dimension

                _dimension = objetoDatos_.Dimension

                If Not _espacioTrabajo Is Nothing Then

                    _tagWatcher = .ProduceTransaccion(objetoDatos_,
                                                      _modalidadConsulta,
                                                      _clausulasLibres,
                                                      IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    'Tiempo de respuesta de la operación a la base de datos
                    _tiempoTranscurridoMilisegundos = .TiempoTranscurridoMilisegundos

                Else

                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            End With

            Return _tagWatcher

        End Function



#End Region

#Region "Propiedades"

        Public Property ReflejarEn As IEnlaceDatos.DestinosParaReplicacion Implements IEnlaceDatos.ReflejarEn
            Get
                Return _lineaBaseEnlaceDatos.ReflejarEn
            End Get
            Set(ByVal value As IEnlaceDatos.DestinosParaReplicacion)
                _lineaBaseEnlaceDatos.ReflejarEn = value
            End Set
        End Property

        Property TipoGestionOperativa As IEnlaceDatos.TiposGestionOperativa Implements IEnlaceDatos.TipoGestionOperativa

            Get
                Return _tipoGestionOperativa
            End Get

            Set(value As IEnlaceDatos.TiposGestionOperativa)
                _tipoGestionOperativa = value
            End Set

        End Property


        Public WriteOnly Property Granularidad As IEnlaceDatos.TiposDimension _
            Implements IEnlaceDatos.Granularidad

            Set(value As IEnlaceDatos.TiposDimension)

                _granularidad = value
                _lineaBaseEnlaceDatos.Granularidad = value

            End Set

        End Property

        Property FiltrosAvanzados As String Implements IEnlaceDatos.FiltrosAvanzados

            Get
                Return _lineaBaseEnlaceDatos.FiltrosAvanzados
            End Get

            Set(value As String)
                _lineaBaseEnlaceDatos.FiltrosAvanzados = value
            End Set

        End Property

        Property LimiteRegistrosColeccionMuestral As Int32 Implements IEnlaceDatos.LimiteRegistrosColeccionMuestral
            Get
                Return _lineaBaseEnlaceDatos.LimiteRegistrosColeccionMuestral
            End Get
            Set(value As Int32)
                _lineaBaseEnlaceDatos.LimiteRegistrosColeccionMuestral = value
            End Set
        End Property


        ReadOnly Property ObtenerListaRegistros As List(Of Object) Implements IEnlaceDatos.ObtenerListaRegistros
            Get
                Return ConvertirResultadosLista()
            End Get
        End Property

        ReadOnly Property ObtenerTablaResumen As DataTable Implements IEnlaceDatos.ObtenerTablaResumen
            Get
                Return CrearTablaResumen()
            End Get
        End Property

        ReadOnly Property ObtenerListaResumen As List(Of Object) Implements IEnlaceDatos.ObtenerListaResumen
            Get
                Return CrearListaResumen()
            End Get
        End Property


        Property ModalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta _
            Implements IEnlaceDatos.ModalidadConsulta
            Get
                Return _modalidadConsulta

            End Get
            Set(value As IOperacionesCatalogo.ModalidadesConsulta)
                _modalidadConsulta = value
            End Set
        End Property


        Property ClausulasLibres As String _
            Implements IEnlaceDatos.ClausulasLibres
            Get
                Return _clausulasLibres

            End Get
            Set(value As String)
                _clausulasLibres = value
            End Set
        End Property

        Public Property ModalidadPresentacion As IEnlaceDatos.ModalidadPresentacionEncabezados _
            Implements IEnlaceDatos.ModalidadPresentacion

            Get

                Return _lineaBaseEnlaceDatos.ModalidadPresentacion

            End Get

            Set(value As IEnlaceDatos.ModalidadPresentacionEncabezados)

                _lineaBaseEnlaceDatos.ModalidadPresentacion = value

            End Set

        End Property

        ReadOnly Property TiempoTranscurridoMilisegundos As Long _
            Implements IEnlaceDatos.TiempoTranscurridoMilisegundos

            Get

                Return _tiempoTranscurridoMilisegundos

            End Get

        End Property

        Property IOperaciones As IOperacionesCatalogo _
            Implements IEnlaceDatos.IOperaciones

            Get

                Return _lineaBaseEnlaceDatos.IOperaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _lineaBaseEnlaceDatos.IOperaciones = value

            End Set

        End Property

        ReadOnly Property Dimension As IEnlaceDatos.TiposDimension

            Get

                Return _dimension

            End Get

        End Property

        Property MensajeTagWatcher As TagWatcher _
            Implements IEnlaceDatos.MensajeTagWatcher

            Get

                Return _tagWatcher

            End Get

            Set(value As TagWatcher)

                _tagWatcher = value

            End Set

        End Property

        Public WriteOnly Property ConvertirCamposNulos As Integer _
            Implements IEnlaceDatos.ConvertirCamposNulos

            Set(value As Integer)

                _convertirCamposNulos = value

            End Set

        End Property

        Public WriteOnly Property EspacioTrabajo As IEspacioTrabajo _
            Implements IEnlaceDatos.EspacioTrabajo

            Set(value As IEspacioTrabajo)

                _espacioTrabajo = value

                _lineaBaseEnlaceDatos.EspacioTrabajo = _espacioTrabajo

            End Set

        End Property


        Public ReadOnly Property Tabla As DataTable _
            Implements IEnlaceDatos.Tabla

            Get

                Return _lineaBaseEnlaceDatos.Tabla

            End Get

        End Property

        Private Function ConvertirResultadosLista() As List(Of Object)

            'Lista principal
            Dim listaResultados_ As New List(Of Object)

            Dim sistema_ As New Organismo

            If sistema_.TieneResultados(_lineaBaseEnlaceDatos.IOperaciones) Then

                Dim indice_ As Int32 = 1

                For Each registro_ As DataRow In _lineaBaseEnlaceDatos.IOperaciones.Vista.Tables(0).Rows

                    If indice_ > _lineaBaseEnlaceDatos.LimiteRegistrosColeccionMuestral Then

                        Exit For

                    End If

                    'Lista por registro

                    If _lineaBaseEnlaceDatos.Registros.Count > 0 Then

                        Dim registros_ As New Dictionary(Of String, String)

                        With registros_

                            Dim posicion_ As Int32 = 0

                            For Each campoVirtual_ As CampoVirtual In _lineaBaseEnlaceDatos.Registros.Item(0).Atributos

                                Dim nombreCampo_ As String = Nothing

                                Dim tipoDato_ As Organismo.VerificarTipoDatoDBNULL = Organismo.VerificarTipoDatoDBNULL.Numero

                                With campoVirtual_

                                    If .TipoDato = IEntidadDatos.TiposDatos.Entero Then

                                        tipoDato_ = Organismo.VerificarTipoDatoDBNULL.Numero

                                    ElseIf .TipoDato = IEntidadDatos.TiposDatos.Booleno Then

                                        tipoDato_ = Organismo.VerificarTipoDatoDBNULL.Numero

                                    ElseIf .TipoDato = IEntidadDatos.TiposDatos.Fecha Then

                                        tipoDato_ = Organismo.VerificarTipoDatoDBNULL.Cadena

                                    ElseIf .TipoDato = IEntidadDatos.TiposDatos.Real Then

                                        tipoDato_ = Organismo.VerificarTipoDatoDBNULL.Numero

                                    ElseIf .TipoDato = IEntidadDatos.TiposDatos.Texto Then

                                        tipoDato_ = Organismo.VerificarTipoDatoDBNULL.Cadena

                                    End If

                                    If ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo Then

                                        nombreCampo_ = .Atributo.ToString

                                    Else

                                        nombreCampo_ = .Descripcion

                                    End If

                                    registros_.Add(nombreCampo_,
                                                  sistema_.ValidarVacios(registro_(posicion_),
                                                  tipoDato_))

                                    posicion_ += 1

                                End With

                            Next

                        End With

                        listaResultados_.Add(registros_)

                        indice_ += 1

                    End If

                Next

            End If

            Return listaResultados_

        End Function

        Public Function GeneraTransaccion(ByVal estructuraRequerida_ As IEntidadDatos,
                                          Optional ByVal clausulas_ As List(Of String) = Nothing,
                                          Optional espacioTrabajo_ As IEspacioTrabajo = Nothing,
                                          Optional limiteResultados_ As Integer = 1000,
                                          Optional convertirCamposNulos_ As Boolean = True,
                                          Optional milisegundosLatenciaMaxima_ As Integer = 5000,
                                          Optional tipoRespuestaRequerida_ As IEnlaceDatos.FormatosRespuesta = IEnlaceDatos.FormatosRespuesta.ObjetoSimple) As Object _
                                          Implements IEnlaceDatos.GeneraTransaccion

            _tagWatcher.Clear()

            _lineaBaseEnlaceDatos.LimiteRegistros = _limiteResultados

            _lineaBaseEnlaceDatos.Registros.Clear()

            'estructuraRequerida_.Dimension = estructuraRequerida_.Dimension

            _dimension = estructuraRequerida_.Dimension

            If Not _espacioTrabajo Is Nothing Then

                'ProduceTransaccion(estructuraRequerida_)

                _tagWatcher = _lineaBaseEnlaceDatos.ProduceTransaccion(estructuraRequerida_,
                                                                   _modalidadConsulta,
                                                                   _clausulasLibres)

                'Tiempo de respuesta de la operación a la base de datos
                _tiempoTranscurridoMilisegundos = _lineaBaseEnlaceDatos.TiempoTranscurridoMilisegundos


                If _tagWatcher.Status = TagWatcher.TypeStatus.Ok Then

                    Select Case _tipoRespuestaRequerida

                        Case IEnlaceDatos.FormatosRespuesta.IOperaciones

                            '_iOperaciones = _lineaBaseEnlaceDatos.IOperaciones

                        Case IEnlaceDatos.FormatosRespuesta.ObjetoSimple
                            'NO IMPLEMENTADO

                        Case IEnlaceDatos.FormatosRespuesta.ObjetoXML
                            'NO IMPLEMENTADO

                        Case Else
                            'NO IMPLEMENTADO

                    End Select

                    ' MsgBox("Done!")

                End If

            Else

                _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

            End If

            Return _lineaBaseEnlaceDatos.Registros

        End Function


        Public Function ObtieneEstructuraResultados(ByRef estructuraRequerida_ As IEntidadDatos,
                                  Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher _
                                  Implements IEnlaceDatos.ObtieneEstructuraResultados


            _tagWatcher.Clear()

            If Not _espacioTrabajo Is Nothing Then

                _tagWatcher = _lineaBaseEnlaceDatos.ProduceEstructuraResultados(estructuraRequerida_,
                                                                                _clausulasLibres,
                                                                                _modalidadConsulta)
            Else

                _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

            End If

            Return _tagWatcher

        End Function

        Public Function ObtieneEstructuraCompleta(ByVal estructuraRequerida_ As IEntidadDatos,
                                                  ByVal nombreEntidad_ As String,
                                                  Optional espacioTrabajo_ As IEspacioTrabajo = Nothing) As TagWatcher _
                          Implements IEnlaceDatos.ObtieneEstructuraCompleta


            _tagWatcher.Clear()

            If Not _espacioTrabajo Is Nothing Then

                _tagWatcher = _lineaBaseEnlaceDatos.ProduceEstructuraCompleta(estructuraRequerida_,
                                                                              nombreEntidad_,
                                                                                _clausulasLibres,
                                                                                _modalidadConsulta)
            Else

                _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

            End If

            Return _tagWatcher

        End Function

        Public Function CrearTablaResumen(Optional ByVal tabla_ As DataTable = Nothing,
                                          Optional ByVal estructuraResumen_ As IEntidadDatos = Nothing) As DataTable Implements IEnlaceDatos.CrearTablaResumen

            Dim tablaOriginal_ As New DataTable

            Dim atributosResumen_ As New List(Of CampoVirtual)

            Dim tablaResumen_ As New DataTable

            Dim tablaOrdenada_ As DataTable = Nothing

            Dim sistema_ As New Organismo

            Dim camposAgrupadores_ As New List(Of String)

            Dim clausulasOrdenamiento_ As String = Nothing

            Dim camposFuncionesAgregacion_ As New Dictionary(Of String, String)

            'Asigna la tabla original

            If Not tabla_ Is Nothing Then

                tablaOriginal_ = tabla_

            Else

                If sistema_.TieneResultados(_lineaBaseEnlaceDatos.IOperaciones) Then

                    tablaOriginal_ = _lineaBaseEnlaceDatos.Tabla

                End If

            End If

            ' Asigna la lista de atributos 

            If Not estructuraResumen_ Is Nothing Then

                atributosResumen_ = estructuraResumen_.Atributos

            Else

                If sistema_.TieneResultados(_lineaBaseEnlaceDatos.IOperaciones) Then

                    atributosResumen_ = _lineaBaseEnlaceDatos.Registros.Item(0).Atributos

                End If

            End If


            If Not tablaOriginal_ Is Nothing Then

                ' Recorre estructura para obtener los agrupadores y las funciones de agrupación
                For Each campoVirtual_ As CampoVirtual In atributosResumen_

                    If campoVirtual_.EsAgrupador Then

                        camposAgrupadores_.Add(campoVirtual_.Atributo.ToString)

                        ' Crea las clasulas de ordenamiento
                        If campoVirtual_.TipoOrdenamiento <> IEntidadDatos.TiposOrdenamiento.SinDefinir Then

                            If clausulasOrdenamiento_ Is Nothing Then

                                clausulasOrdenamiento_ = campoVirtual_.Atributo.ToString & " " &
                                    sistema_.GetEnumDescription(DirectCast(Convert.ToInt32(campoVirtual_.TipoOrdenamiento), IEntidadDatos.TiposOrdenamiento))
                            Else

                                clausulasOrdenamiento_ = clausulasOrdenamiento_ & ", " & campoVirtual_.Atributo.ToString & " " &
                                   sistema_.GetEnumDescription(DirectCast(Convert.ToInt32(campoVirtual_.TipoOrdenamiento), IEntidadDatos.TiposOrdenamiento))

                            End If

                        End If

                    End If

                    If Not campoVirtual_.FuncionAgregacion = IEntidadDatos.TiposFuncionesAgregacion.SinDefinir Then

                        camposFuncionesAgregacion_.Add(campoVirtual_.Atributo.ToString,
                            sistema_.GetEnumDescription(DirectCast(Convert.ToInt32(campoVirtual_.FuncionAgregacion), IEntidadDatos.TiposFuncionesAgregacion)) &
                            "(" & campoVirtual_.Atributo.ToString & ")")

                    End If

                Next

                ' Verifica que existan campos con funciones de agregación
                If camposFuncionesAgregacion_.Count > 0 Then

                    If tablaOriginal_.Rows.Count > 0 Then

                        Dim dataView_ As New DataView(tablaOriginal_)

                        If camposAgrupadores_.Count > 0 Then

                            tablaResumen_ = dataView_.ToTable(True, camposAgrupadores_.ToArray())

                        End If

                        For Each funcionAgregada_ As KeyValuePair(Of String, String) In camposFuncionesAgregacion_

                            ' tablaResumen_.Columns.Add(funcionAgregada_.Key, GetType(Integer))
                            tablaResumen_.Columns.Add(funcionAgregada_.Key, tablaOriginal_.Columns.Item(funcionAgregada_.Key).DataType())


                            If tablaResumen_.Rows.Count > 0 Then

                                For Each dr As DataRow In tablaResumen_.Rows

                                    Dim t_ClausulaAgrupacion As String = Nothing

                                    For i_ As Int32 = 0 To camposAgrupadores_.Count - 1

                                        If t_ClausulaAgrupacion Is Nothing Then

                                            t_ClausulaAgrupacion = tablaResumen_.Columns.Item(i_).ColumnName & " = '" & dr.Item(tablaResumen_.Columns.Item(i_).ColumnName) & "'"

                                        Else

                                            t_ClausulaAgrupacion = t_ClausulaAgrupacion & " AND " & tablaResumen_.Columns.Item(i_).ColumnName & " = '" & dr.Item(tablaResumen_.Columns.Item(i_).ColumnName) & "'"

                                        End If

                                    Next

                                    dr(funcionAgregada_.Key) = tablaOriginal_.Compute(funcionAgregada_.Value, t_ClausulaAgrupacion)

                                Next

                            Else

                                tablaResumen_.Rows.Add()

                                tablaResumen_(0)(funcionAgregada_.Key) = tablaOriginal_.Compute(funcionAgregada_.Value, String.Empty)

                            End If

                        Next

                    End If

                    If Not tablaResumen_ Is Nothing Then

                        If tablaResumen_.Columns.Count > 0 Then

                            ' Realiza el ordenamiento de la tabla
                            tablaOrdenada_ = tablaResumen_.Select("", clausulasOrdenamiento_).CopyToDataTable

                        End If

                    End If

                Else
                    'PENDIENTE IMPLEMENTAR CODIGOS DE ERROR
                    _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

                End If

            Else

                'PENDIENTE IMPLEMENTAR CODIGOS DE ERROR
                _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

            End If

            Return tablaOrdenada_

        End Function

        Public Function CrearListaResumen(Optional ByVal tabla_ As DataTable = Nothing,
                                          Optional ByVal estructuraRequerida_ As IEntidadDatos = Nothing) As List(Of Object) Implements IEnlaceDatos.CrearListaResumen

            Dim listaResumen_ As New List(Of Object)

            Dim tablaResumen_ As DataTable = CrearTablaResumen(tabla_, estructuraRequerida_)

            If Not tablaResumen_ Is Nothing Then

                For i_ As Int32 = 0 To tablaResumen_.Rows.Count - 1

                    Dim registros_ As New Dictionary(Of String, String)

                    For j_ As Int32 = 0 To tablaResumen_.Columns.Count - 1

                        registros_.Add(tablaResumen_.Columns(j_).ColumnName, tablaResumen_.Rows(i_)(j_))

                    Next

                    listaResumen_.Add(registros_)

                Next

            End If

            Return listaResumen_

        End Function

        Public Property Registros As List(Of IEntidadDatos) Implements IEnlaceDatos.Registros

            Get

                Return _lineaBaseEnlaceDatos.Registros

            End Get

            Set(value As List(Of IEntidadDatos))

                _lineaBaseEnlaceDatos.Registros = value

            End Set

        End Property

        Public WriteOnly Property LimiteResultados As Integer _
            Implements IEnlaceDatos.LimiteResultados

            Set(value As Integer)

                _limiteResultados = value

                _lineaBaseEnlaceDatos.LimiteRegistros = value

            End Set

        End Property

        Public WriteOnly Property MilisegundosLatenciaMaxima As Integer _
            Implements IEnlaceDatos.MilisegundosLatenciaMaxima

            Set(value As Integer)

                _milisegundosLatenciaMaxima = value

            End Set

        End Property

        Public WriteOnly Property TipoRespuestaRequerida As IEnlaceDatos.FormatosRespuesta _
            Implements IEnlaceDatos.TipoRespuestaRequerida

            Set(value As IEnlaceDatos.FormatosRespuesta)

                _tipoRespuestaRequerida = value

            End Set

        End Property

#End Region

#Region "Clone"

        Public Function Clone() As Object Implements ICloneable.Clone

            Dim enlaceDatosClonado_ As IEnlaceDatos = New EnlaceDatos

            With enlaceDatosClonado_

                .ClausulasLibres = Me.ClausulasLibres

                .EspacioTrabajo = Me._espacioTrabajo

                .FiltrosAvanzados = Me.FiltrosAvanzados

                .Granularidad = Me._granularidad

                .IOperaciones = Me.IOperaciones

                .LimiteRegistrosColeccionMuestral = Me.LimiteRegistrosColeccionMuestral

                .LimiteResultados = Me._limiteResultados

                .MilisegundosLatenciaMaxima = Me._milisegundosLatenciaMaxima

                .ModalidadConsulta = Me.ModalidadConsulta

                .ModalidadPresentacion = Me.ModalidadPresentacion

                .Registros = Me.Registros

                .TipoGestionOperativa = Me.TipoGestionOperativa

                .TipoRespuestaRequerida = Me._tipoRespuestaRequerida

            End With

            Return enlaceDatosClonado_

        End Function

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Para detectar llamadas redundantes

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: eliminar estado administrado (objetos administrados).
                End If

                ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                ' TODO: Establecer campos grandes como Null.

                'Recursos no adminitrados dependientes.

                With Me

                    .EspacioTrabajo = Nothing

                    .IOperaciones = Nothing

                    .Registros = Nothing

                End With



            End If

            Me.disposedValue = True

        End Sub

        ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
        'Protected Overrides Sub Finalize()
        '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)

        End Sub

#End Region

        Public Function Consultar(ByVal estructuraRequerida_ As IEntidadDatos,
                                  Optional clausulas_ As List(Of String) = Nothing,
                                  Optional espacioTrabajo_ As IEspacioTrabajo = Nothing,
                                  Optional limiteResultados_ As Integer = 1000,
                                  Optional convertirCamposNulos_ As Boolean = True,
                                  Optional milisegundosLatenciaMaxima_ As Integer = 5000,
                                  Optional objetoDatos_ As IConexiones.TiposRepositorio = IConexiones.TiposRepositorio.Automatico) As TagWatcher _
                              Implements IEnlaceDatos.Consultar

            '_lineaBaseEnlaceDatos.ObjetoDatos = objetoDatos_

            _tagWatcher.Clear()

            _lineaBaseEnlaceDatos.LimiteRegistros = _limiteResultados

            _lineaBaseEnlaceDatos.Registros.Clear()

            _dimension = estructuraRequerida_.Dimension

            If Not _espacioTrabajo Is Nothing Then

                'Tiempo de respuesta de la operación a la base de datos
                _tiempoTranscurridoMilisegundos = _lineaBaseEnlaceDatos.TiempoTranscurridoMilisegundos

                'Select Case _lineaBaseEnlaceDatos.OrigenDatos

                '    Case IConexiones.Controladores.Automatico, IConexiones.Controladores.SQLServer2008, IConexiones.Controladores.MySQL51

                '    Case IConexiones.Controladores.MongoDB

                '    Case Else

                'End Select

                'Select Case _lineaBaseEnlaceDatos.ObjetoDatos

                '    Case IConexiones.TiposRepositorio.DataSetObject,
                '         IConexiones.TiposRepositorio.Automatico

                _tagWatcher = _lineaBaseEnlaceDatos.ProduceTransaccion(estructuraRequerida_,
                                                      _modalidadConsulta,
                                                      _clausulasLibres)

                '    Case IConexiones.TiposRepositorio.DataTableObject
                ''NO IMPLEMENTADO

                '    Case IConexiones.TiposRepositorio.BSONDocumentObject
                ''NOT IMPLEMENTED

                '    Case Else
                ''NO IMPLEMENTADO

                'End Select


            Else

                _tagWatcher.SetError(Me, TagWatcher.ErrorTypes.C5_013_00001)

            End If

            Return _tagWatcher

        End Function

        Public Property ObjetoDatos As IConexiones.TiposRepositorio Implements IEnlaceDatos.ObjetoDatos
            Get
                Return _lineaBaseEnlaceDatos.ObjetoDatos
            End Get
            Set(value As IConexiones.TiposRepositorio)
                _lineaBaseEnlaceDatos.ObjetoDatos = value
            End Set
        End Property

        Public Property OrigenDatos As IConexiones.Controladores Implements IEnlaceDatos.OrigenDatos
            Get
                Return _lineaBaseEnlaceDatos.OrigenDatos
            End Get
            Set(value As IConexiones.Controladores)
                _lineaBaseEnlaceDatos.OrigenDatos = value
            End Set
        End Property
        Public Property TipoConexion As IConexiones.TipoConexion Implements IEnlaceDatos.TipoConexion
            Get
                Return _lineaBaseEnlaceDatos.TipoConexion
            End Get
            Set(value As IConexiones.TipoConexion)
                _lineaBaseEnlaceDatos.TipoConexion = value
            End Set
        End Property



    End Class

End Namespace