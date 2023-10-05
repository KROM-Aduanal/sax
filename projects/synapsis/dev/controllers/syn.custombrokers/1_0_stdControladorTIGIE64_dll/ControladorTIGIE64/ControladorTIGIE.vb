Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesTarifaArancelaria
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Syn.Nucleo.RecursosComercioExterior.CamposTarifaArancelaria
Imports MongoDB.Bson.Serialization.Serializers
Imports Syn.Utils
Imports Syn.Nucleo
Imports System.Xml.Serialization

Public Class ControladorTIGIE
    Implements IControladorTIGIE, ICloneable, IDisposable

    Private _enlaceDatos As IEnlaceDatos
    Private _seccionIE As Seccion

#Region "Enums"


#End Region

#Region "Propiedades"

    Public Property Estado As TagWatcher Implements IControladorTIGIE.Estado

#End Region

#Region "Constructores"

    Sub New()

        _Estado = New TagWatcher

        _enlaceDatos = New EnlaceDatos()

    End Sub

#End Region

#Region "Funciones"
    'https://stackoverflow.com/questions/60142898/unable-to-cast-object-of-type-mongodb-bson-bsonstring-to-type-mongodb-bson-bs
    'Match(Function(e) e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento.Contains(texto_)).
    'ToEnumerable().Where(Function(e) e.Fraccion.Contains(texto_)).ToList()

    Public Function Prueba() As List(Of Object)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                   {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New ConstructorCliente()

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                Dim collection = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

                Dim results = collection.Aggregate().
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Fuente = e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                         }).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Seccion = e.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos,
                                            Key .Fecha = e.Fuente.FechaCreacion,
                                            Key .FolioDocumento = e.Fuente.FolioDocumento,
                                            Key .UsuarioGenerador = e.Fuente.UsuarioGenerador
                                         }).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .RazonSocial = DirectCast(e.Seccion(1).Nodos(0), Campo).Valor,
                                            Key .CURP = DirectCast(e.Seccion(5).Nodos(0), Campo).Valor,
                                            Key .Domicilio = DirectCast(e.Seccion(10).Nodos(0), Campo).Valor,
                                            Key .Fecha = e.Fecha,
                                            Key .FolioDocumento = e.FolioDocumento,
                                            Key .UsuarioGenerador = e.UsuarioGenerador
                                         }).
                                         ToList()


                Dim listaFracciones As New List(Of Object)
                Dim i = 1
                results.AsEnumerable.ToList().ForEach(Sub(x)

                                                          listaFracciones.Add(New Dictionary(Of String, Object) From {
                                                            {"Id", x.Id},
                                                            {"RazonSocial", x.RazonSocial},
                                                            {"CURP", x.CURP},
                                                            {"Domicilio", x.Domicilio},
                                                            {"Fecha", x.Fecha},
                                                            {"FolioDocumento", x.FolioDocumento},
                                                            {"UsuarioGenerador", x.UsuarioGenerador},
                                                            {"archivado", False},
                                                            {"borrado", False},
                                                            {"editando", False},
                                                            {"calapsado", True},
                                                            {"indice", i}
                                                          })


                                                          i = i + 1

                                                      End Sub)

                Return listaFracciones

            End Using

        End Using


        Return Nothing

    End Function

    Public Function EnlistarFracciones(ByVal texto_ As String) As TagWatcher _
                            Implements IControladorTIGIE.EnlistarFracciones

        'Match(BsonDocument.Parse("{$text:{$search:" & q & "}}")).
        'Match(Builders(Of OperacionGenerica).Filter.Text(q)).

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                   {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New ConstructorTIGIE()

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                Dim collection = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

                Dim regexFraccion As Regex = New Regex("^\d{4}\.\d{2}\.\d{2}$")

                Dim q = IIf(regexFraccion.Match(texto_).Success,
                            Chr(39) & Chr(34) & texto_ & Chr(34) & Chr(39),
                            Chr(34) & String.Join(Chr(32), (From w In texto_.Trim().Split(Chr(32)) Select Chr(92) & Chr(34) & w & Chr(92) & Chr(34)).ToArray()) & Chr(34))

                Dim results = collection.Aggregate().
                                         Match(BsonDocument.Parse("{$text:{$search:" & q & "}}")).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Fuente = e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                         }).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Seccion = e.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos
                                         }).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Fraccion = DirectCast(e.Seccion(0).Nodos(0), Campo).Valor,
                                            Key .DescripcionFraccion = DirectCast(e.Seccion(2).Nodos(0), Campo).Valor,
                                            Key .FechaPublicacion = DirectCast(e.Seccion(11).Nodos(0), Campo).Valor,
                                            Key .FechaInicioVigencia = DirectCast(e.Seccion(12).Nodos(0), Campo).Valor,
                                            Key .FechaFinVigencia = DirectCast(e.Seccion(13).Nodos(0), Campo).Valor
                                         }).
                                         ToList()

                'Match(BsonDocument.Parse("{$or:[{'Fraccion':/" & texto_ & "/},{'DescripcionFraccion':/ " & texto_ & "/}]}")).

                Dim listaFracciones As New List(Of FraccionArancelaria)

                results.AsEnumerable.ToList().ForEach(Sub(x)

                                                          listaFracciones.Add(New FraccionArancelaria With {
                                                            .Id = x.Id,
                                                            .Fraccion = x.Fraccion,
                                                            .DescripcionFraccion = x.DescripcionFraccion,
                                                            .FechaPublicacion = x.FechaPublicacion,
                                                            .FechaInicioVigencia = x.FechaInicioVigencia,
                                                            .FechaFinVigencia = x.FechaFinVigencia
                                                          })

                                                      End Sub)

                If listaFracciones.Count Then

                    Return New TagWatcher(1) With {.ObjectReturned = listaFracciones}

                Else

                    Return New TagWatcher(0, Me, "No se encontró ningún valor para esta consulta")

                End If
            End Using

        End Using

    End Function

    'Public Function EnlistarFracciones(ByVal texto_ As String) As List(Of FraccionArancelaria)

    '    Dim tiggie_ As New ConstructorTIGIE()

    '    Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
    '                {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

    '        Using _entidadDatos As IEntidadDatos = tiggie_

    '            Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

    '            Dim operacionesNodo_ = New OperacionesNodos

    '            operacionesNodo_.CrearPartidasDocumentoElectronico(documentoElectronico_)

    '            Dim numeroFraccion_ = operacionesNodo_.ObtenerRutaCampo(documentoElectronico_, 1, 10101)

    '            Dim descripcionFraccion_ = operacionesNodo_.ObtenerRutaCampo(documentoElectronico_, 1, 10103)

    '            Dim pipeline_ As New List(Of BsonDocument)

    '            Dim raiz_ As New BsonDocument() From {
    '                    {
    '                        "$addFields", New BsonDocument From {
    '                        {"Nico", "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.Nodos.Nodos.Nodos"},
    '                                {"Documento", "$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado"}
    '                        }
    '                    }
    '            }

    '            Dim plancharDatosRai2z_ = New BsonDocument From {
    '            {
    '                    "$unwind", New BsonDocument From {
    '                                {"path", "$Nico"},
    '                                {"preserveNullAndEmptyArrays", True}
    '                    }
    '                }
    '            }
    '            Dim plancharDatosRaiz_ = New BsonDocument From {
    '            {
    '                    "$unwind", New BsonDocument From {
    '                                {"path", "$Documento"},
    '                                {"preserveNullAndEmptyArrays", True}
    '                    }
    '                }
    '            }

    '            pipeline_.Add(raiz_)

    '            pipeline_.Add(plancharDatosRaiz_)
    '            pipeline_.Add(plancharDatosRai2z_)

    '            Dim partsNumeroFraccion_ As String() = numeroFraccion_.Split(".")

    '            partsNumeroFraccion_ = partsNumeroFraccion_.Skip(1).ToArray

    '            Dim indice_ = 1

    '            For Each rutaActual_ As String In partsNumeroFraccion_

    '                Dim agregarCampo_ = New BsonDocument From {
    '                    {
    '                        "$addFields", New BsonDocument From {
    '                                    {"Nodo" & indice_, If(indice_ = 1, "$Documento.Nodos", "$Nodo" & (indice_ - 1) & ".Nodos")}
    '                        }
    '                    }
    '                }

    '                Dim plancharDatos_ = New BsonDocument From {
    '                {
    '                        "$unwind", New BsonDocument From {
    '                                    {"path", "$Nodo" & indice_.ToString()},
    '                                    {"preserveNullAndEmptyArrays", True}
    '                        }
    '                    }
    '                }

    '                pipeline_.Add(agregarCampo_)

    '                pipeline_.Add(plancharDatos_)

    '                If indice_ <partsNumeroFraccion_.Count Then

    '                    indice_ += 1

    '                End If

    '            Next

    '            Dim indice2_ = 1

    '            Dim partsDescripcionFraccion_ As String() = descripcionFraccion_.Split(".")

    '            partsDescripcionFraccion_ = partsDescripcionFraccion_.Skip(1).ToArray

    '            For Each rutaActual_ As String In partsDescripcionFraccion_

    '                Dim agregarCampo_ = New BsonDocument From {
    '                    {
    '                        "$addFields", New BsonDocument From {
    '                                    {"Nodob" & indice2_, If(indice2_ = 1, "$Documento.Nodos", "$Nodob" & (indice2_ - 1) & ".Nodos")}
    '                        }
    '                    }
    '                }

    '                Dim plancharDatos_ = New BsonDocument From {
    '                {
    '                        "$unwind", New BsonDocument From {
    '                                    {"path", "$Nodob" & indice2_.ToString()},
    '                                    {"preserveNullAndEmptyArrays", True}
    '                        }
    '                    }
    '                }

    '                pipeline_.Add(agregarCampo_)

    '                pipeline_.Add(plancharDatos_)

    '                If indice2_ < partsDescripcionFraccion_.Count Then

    '                    indice2_ += 1

    '                End If

    '            Next

    '            Dim condicionesConsulta_ = New BsonDocument From {
    '                {
    '                    "$match", New BsonDocument From {
    '                                {"estado", 1},
    '                                {"Nodo" & indice_ & ".IDUnico", 10101},
    '                                {"Nodob" & indice2_ & ".IDUnico", 10103},
    '                                {"$or", New BsonArray From {
    '                                             New BsonDocument From {
    '                                                 {"Nodo" & indice_ & ".Valor", New BsonDocument From {
    '                                                            {"$regex", texto_},
    '                                                            {"$options", "i"}
    '                                                        }
    '                                                 }
    '                                            }, New BsonDocument From {
    '                                                 {"Nodob" & indice2_ & ".Valor", New BsonDocument From {
    '                                                            {"$regex", texto_},
    '                                                            {"$options", "i"}
    '                                                        }
    '                                                 }
    '                                            }
    '                                        }
    '                                }
    '                    }
    '                }
    '            }

    '            Dim camposConsulta_ = New BsonDocument From {
    '                {
    '                    "$project", New BsonDocument From {
    '                                {"_id", 1},
    '                                {
    '                                    "NumeroFraccion", "$Nodo" & indice_.ToString() & ".Valor"
    '                                },
    '                                {
    '                                    "DescripcionFraccion", "$Nodob" & indice2_.ToString() & ".Valor"
    '                                }
    '                    }
    '                }
    '            }

    '            pipeline_.Add(condicionesConsulta_)

    '            pipeline_.Add(camposConsulta_)

    '            Dim limiteRegistros_ = New BsonDocument From {
    '                {
    '                    "$limit", 1000
    '                }
    '            }

    '            pipeline_.Add(limiteRegistros_)

    '            Dim status_ As New TagWatcher

    '            Dim operacionesDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

    '            Try

    '                Dim objectResult_ = operacionesDB_.Aggregate(Of BsonDocument)(pipeline_).ToList

    '                Dim listaResultados_ = New List(Of Object)

    '                For Each stat As BsonDocument In objectResult_

    '                    listaResultados_.Add(New Dictionary(Of Object, Object) From {
    '                        {"ID", stat.GetElement("_id").Value.ToString},
    '                        {"NumeroFraccion", stat.GetElement("NumeroFraccion").Value.ToString},
    '                        {"DescripcionFraccion", stat.GetElement("DescripcionFraccion").Value.ToString}
    '                    })

    '                Next

    '                status_.SetOK()

    '                status_.ObjectReturned = listaResultados_

    '            Catch e As Exception

    '                status_.SetError(Me, "Error writing to MongoDB: " & e.Message)

    '            End Try

    '            'Return status_

    '        End Using

    '    End Using

    '    Return Nothing
    '    'preallocated
    'End Function

    Public Function EnlistarNicosFraccion(ByVal fraccion_ As String) As TagWatcher _
                                        Implements IControladorTIGIE.EnlistarNicosFraccion
        'Match(Function(e) e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento.Contains(vehiculoFraccion_)).

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                    {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New ConstructorTIGIE()

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                Dim collection = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

                Dim results = collection.Aggregate().
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Fuente = e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                         }).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Seccion = e.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos
                                         }).
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Fraccion = DirectCast(e.Seccion(0).Nodos(0), Campo).Valor,
                                            Key .Nico = DirectCast(e.Seccion(1).Nodos(0), Campo).Valor,
                                            Key .DescripcionNico = DirectCast(e.Seccion(3).Nodos(0), Campo).Valor,
                                            Key .FechaPublicacion = DirectCast(e.Seccion(11).Nodos(0), Campo).Valor,
                                            Key .FechaInicioVigencia = DirectCast(e.Seccion(12).Nodos(0), Campo).Valor,
                                            Key .FechaFinVigencia = DirectCast(e.Seccion(13).Nodos(0), Campo).Valor
                                         }).
                                         Match(BsonDocument.Parse("{'Fraccion':" & Chr(34) & fraccion_ & Chr(34) & "}")).
                                         ToList()

                Dim listaNicosFraccion As New List(Of NicoFraccionArancelaria)

                results.AsEnumerable.ToList().ForEach(Sub(x)

                                                          listaNicosFraccion.Add(New NicoFraccionArancelaria With {
                                                            .Id = x.Id,
                                                            .Nico = x.Nico,
                                                            .DescripcionNico = x.DescripcionNico,
                                                            .FechaPublicacion = x.FechaPublicacion,
                                                            .FechaInicioVigencia = x.FechaInicioVigencia,
                                                            .FechaFinVigencia = x.FechaFinVigencia
                                                          })

                                                      End Sub)

                If listaNicosFraccion.Count Then

                    Return New TagWatcher(1) With {.ObjectReturned = listaNicosFraccion}

                Else

                    Return New TagWatcher(0, Me, "No se encontró ningún valor para esta consulta")

                End If

            End Using

        End Using

    End Function

    Public Function BuscarNico(id_ As ObjectId,
                               Optional tipoOperacion_ As IControladorTIGIE.TipoOperacion = IControladorTIGIE.TipoOperacion.Importacion,
                               Optional fecha_ As Date = Nothing,
                               Optional pais_ As String = Nothing) As TagWatcher _
                               Implements IControladorTIGIE.BuscarNico

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
                        {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorTIGIE).Name)

            Dim resultadoDocumentos_ As New List(Of OperacionGenerica)

            Dim filtro_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, id_)

            resultadoDocumentos_ = operacionesDB_.Find(filtro_).Limit(1).ToList

            If resultadoDocumentos_.Count Then

                Dim nicoFracionArancelaria_ = New NicoFraccionArancelaria

                Dim documentoElectronico_ As DocumentoElectronico = resultadoDocumentos_(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                Dim seccionTarifaArancelaria_ As SeccionesTarifaArancelaria = IIf(tipoOperacion_ = IControladorTIGIE.TipoOperacion.Importacion, SeccionesTarifaArancelaria.TIGIE2, SeccionesTarifaArancelaria.TIGIE3)

                With nicoFracionArancelaria_

                    .Nico = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_NUMERO_NICO).Valor

                    .DescripcionNico = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_NICO).Valor

                    .FechaPublicacion = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FECHA_PUBLICACION).Valor

                    .FechaInicioVigencia = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FECHA_ENTRADA_VIGOR).Valor

                    .FechaFinVigencia = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FECHA_FIN).Valor

                    .UnidadesDeMedida = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Attribute(CA_UNIDAD_MEDIDA).Valor

                    .Impuestos = Nothing

                    .Tratados = GetTratados(documentoElectronico_, seccionTarifaArancelaria_)

                    .Normas = GetNormas(documentoElectronico_, seccionTarifaArancelaria_)

                    .Permisos = GetPermisos(documentoElectronico_, seccionTarifaArancelaria_)

                    .CuotasCompensatorias = GetCuotasCompensatorias(documentoElectronico_, seccionTarifaArancelaria_)

                    .PreciosEstimados = GetPreciosEstimados(documentoElectronico_, seccionTarifaArancelaria_)

                    .IEPS = GetIeps(documentoElectronico_, seccionTarifaArancelaria_)

                    .PadronSectorial = GetPadrones(documentoElectronico_, seccionTarifaArancelaria_)

                End With

                Return New TagWatcher(1) With {.ObjectReturned = nicoFracionArancelaria_}

            Else

                Return New TagWatcher(0, Me, "No se encontró ningún valor para esta consulta")

            End If

        End Using

    End Function

    Public Function TraeDatosFraccion(Of T)(fraccion_ As String,
                                      tipoOperacion_ As IControladorTIGIE.TipoOperacion,
                                      pais_ As String,
                                      fecha_ As Date) As TagWatcher _
                                      Implements IControladorTIGIE.TraeDatosFraccion

        With _Estado

            Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(Activator.CreateInstance(Of T)().GetType.Name)

            Dim items_ = operacionesDB_.Aggregate().Match(Function(a) fraccion_.Equals(a.FolioOperacion)).ToList()

            If items_.Count Then

                Dim item_ = items_(0)

                Dim constructorTigie_ = item_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                .SetOK()

                .ObjectReturned = GenerarVehiculo(constructorTigie_, tipoOperacion_, pais_, fecha_)

            Else

                .SetOKBut(Me, "No se encontró la fracción")

            End If


            Return _Estado

        End With

    End Function
    Public Function TraeDatosFraccion(id_ As ObjectId,
                                      Optional tipoOperacion_ As IControladorTIGIE.TipoOperacion = IControladorTIGIE.TipoOperacion.Importacion,
                                      Optional pais_ As String = Nothing) As TagWatcher _
                                      Implements IControladorTIGIE.TraeDatosFraccion

        Dim seccionesCampos_ = New Dictionary(Of [Enum], List(Of [Enum])) From {{SeccionesTarifaArancelaria.TIGIE1,
                                                                                New List(Of [Enum]) From {CA_NUMERO_FRACCION_ARANCELARIA, CA_NUMERO_NICO, CA_FRACCION_ARANCELARIA}},
                                                                                {SeccionesTarifaArancelaria.TIGIE2, Nothing}
                                                                               }

        With _Estado


            If Not id_ = ObjectId.Empty Then
                'using
                _Estado = ObtenerListaValores(New List(Of ObjectId) From {id_},
                                                seccionesCampos_)

            Else

                .SetOKBut(Me, "Facturas no disponibles en batería")

            End If



            Return _Estado

        End With

    End Function

    Public Function TraeDatosFraccion(fraccion_ As String,
                                      Optional tipoOperacion_ As IControladorTIGIE.TipoOperacion = IControladorTIGIE.TipoOperacion.Importacion,
                                      Optional pais_ As String = Nothing) As TagWatcher _
                                      Implements IControladorTIGIE.TraeDatosFraccion

        Throw New NotImplementedException()

    End Function

    Private Function GetTratados(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'Tratados
        Dim dataTratados_ As New List(Of TratadoItem)
        Dim tratados = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE6)

        For indice_ As Int32 = 1 To tratados.CantidadPartidas

            With tratados.Partida(indice_)

                Dim claveTratado_ = .Attribute(CA_NOMBRE_CORTO_TRATADO).Valor

                Dim paises = .Seccion(SeccionesTarifaArancelaria.TIGIE7)

                For indice2_ As Int32 = 1 To paises.CantidadPartidas

                    With paises.Partida(indice2_)

                        Dim tratadoitem_ = New TratadoItem

                        tratadoitem_.ClaveTratado = claveTratado_
                        tratadoitem_.NombrePais = .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor
                        tratadoitem_.Tasa = .Attribute(CA_ARANCEL).Valor
                        tratadoitem_.Nota = .Attribute(CA_OBSERVACION).Valor
                        tratadoitem_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                        tratadoitem_.FechaIncioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                        tratadoitem_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                        dataTratados_.Add(tratadoitem_)

                    End With

                Next

            End With

        Next

        Return dataTratados_

    End Function

    Private Function GetIeps(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'IEPS
        Dim dataIpes_ As New List(Of IepsItem)
        Dim ieps_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE9)

        For indice_ As Int32 = 1 To ieps_.CantidadPartidas

            With ieps_.Partida(indice_)

                Dim iepsitem_ = New IepsItem

                iepsitem_.Nota = .Attribute(CA_OBSERVACION).Valor

                dataIpes_.Add(iepsitem_)

            End With

        Next

        Return dataIpes_

    End Function

    Private Function GetCuotasCompensatorias(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'Cuotas
        Dim dataCuotas_ As New List(Of CuotaItem)
        Dim cuotas_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE10)

        For indice_ As Int32 = 1 To cuotas_.CantidadPartidas

            With cuotas_.Partida(indice_)

                Dim cuota_ = New CuotaItem

                cuota_.NombreEmpresa = .Attribute(CA_EMPRESA).Valor
                cuota_.NombrePais = .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor
                cuota_.Tasa = .Attribute(CA_CUOTA).Valor
                cuota_.TipoCuota = .Attribute(CA_TIPO).Valor
                cuota_.Nota = .Attribute(CA_ACOTACION).Valor
                cuota_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                cuota_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                cuota_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataCuotas_.Add(cuota_)

            End With

        Next

        Return dataCuotas_

    End Function

    Private Function GetPreciosEstimados(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'Precios Estimados
        Dim dataPrecios_ As New List(Of PrecioItem)
        Dim precios_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE11)

        For indice_ As Int32 = 1 To precios_.CantidadPartidas

            With precios_.Partida(indice_)

                Dim precio_ = New PrecioItem

                precio_.PrecioEstimado = .Attribute(CA_PRECIO).Valor
                precio_.DescripcionUM = .Attribute(CA_UNIDAD_MEDIDA).Valor
                precio_.DetalleProducto = .Attribute(CA_DESCRIPCION).Valor
                precio_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                precio_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                precio_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataPrecios_.Add(precio_)

            End With

        Next

        Return dataPrecios_

    End Function

    Private Function GetPermisos(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'Permisos
        Dim dataPermisos_ As New List(Of PermisoItem)
        Dim permisos_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE5).Seccion(SeccionesTarifaArancelaria.TIGIE13)

        For indice_ As Int32 = 1 To permisos_.CantidadPartidas

            With permisos_.Partida(indice_)

                Dim permiso_ = New PermisoItem

                permiso_.ClavePermiso = .Attribute(CA_CLAVE).Valor
                permiso_.Descripcion = .Attribute(CA_PERMISO).Valor
                permiso_.Particularidad = .Attribute(CA_ACOTACION).Valor
                permiso_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                permiso_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                permiso_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataPermisos_.Add(permiso_)

            End With

        Next

        Return dataPermisos_

    End Function

    Private Function GetNormas(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'Normas
        Dim dataNorms_ As New List(Of NormaItem)
        Dim normas_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE5).Seccion(SeccionesTarifaArancelaria.TIGIE14)

        For indice_ As Int32 = 1 To normas_.CantidadPartidas

            With normas_.Partida(indice_)

                Dim norma_ = New NormaItem

                norma_.NOM = .Attribute(CA_NORMA).Valor
                norma_.Descripcion = .Attribute(CA_DESCRIPCION).Valor
                norma_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                norma_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                norma_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataNorms_.Add(norma_)

            End With

        Next

        Return dataNorms_

    End Function

    Private Function GetPadrones(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        'Padrones
        Dim dataPadrones_ As New List(Of PadronItem)
        Dim padrones_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE5).Seccion(SeccionesTarifaArancelaria.TIGIE18)

        For indice_ As Int32 = 1 To padrones_.CantidadPartidas

            With padrones_.Partida(indice_)

                Dim padron_ = New PadronItem

                padron_.Clave = .Attribute(CA_SECTOR).Valor
                padron_.Encabezado = .Attribute(CA_DESCRIPCION).Valor

                dataPadrones_.Add(padron_)

            End With

        Next

        Return dataPadrones_

    End Function

    Private Function ObtenerListaValores(idsFracciones_ As List(Of ObjectId),
                                        seccionesCampos_ As Dictionary(Of [Enum], List(Of [Enum]))) _
                                        As TagWatcher

        With _Estado

            Try

                Dim listadoValoresObject_ = New Dictionary(Of ObjectId, List(Of Nodo))

                Dim diccionarioValoresObjectId_ As New Dictionary(Of ObjectId, List(Of Nodo))

                Dim _rOrganismo = New Organismo  'constructor 

                listadoValoresObject_ = _rOrganismo.ObtenerCamposSeccionExterior(idsFracciones_,
                                                                                New ConstructorTIGIE,
                                                                                seccionesCampos_)

                If listadoValoresObject_.Count > 0 Then

                    For Each listaValor_ In listadoValoresObject_

                        diccionarioValoresObjectId_.Add(listaValor_.Key, listaValor_.Value)

                    Next

                Else

                    .SetError(Me, "No se encontraron campos en el listado de fracciones")

                End If

                If diccionarioValoresObjectId_.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de valores") : End If

                Dim fraccion_ = New Fraccion  'atributo de clase

                fraccion_.impuestos = New List(Of String)

                fraccion_.unidadMedida = "Unidad Medida"

                fraccion_.tratados = New List(Of String)

                fraccion_.normas = New List(Of String)

                fraccion_.anexos = New List(Of String)

                For Each tarifa_ As KeyValuePair(Of ObjectId, List(Of Nodo)) In diccionarioValoresObjectId_

                    For Each nodo_ As Nodo In tarifa_.Value

                        Select Case nodo_.DescripcionTipoNodo

                            Case "Campo"

                                fraccion_.fraccion &= DirectCast(nodo_, Campo).Valor

                            Case Else

                                fraccion_.unidadMedida &= "|" & nodo_.Attribute(CA_CLAVE_UNIDAD_MEDIDA).Valor & "|" & nodo_.Attribute(CA_UNIDAD_MEDIDA).Valor

                                For Each impuesto_ As Nodo In nodo_.Seccion(TIGIE19).Nodos

                                    fraccion_.impuestos.Add(impuesto_.Attribute(CA_NOMBRE_IMPUESTO).Valor & "|%|" &
                                                            impuesto_.Attribute(CA_VALOR_IMPUESTO).Valor)

                                Next

                                For Each permiso_ As Nodo In nodo_.Seccion(TIGIE13).Nodos

                                    If permiso_.Attribute(CA_FECHA_FIN).Valor > Now() Then

                                        fraccion_.permiso = "Permiso|" & permiso_.Attribute(CamposTarifaArancelaria.CA_CLAVE).Valor & "|" &
                                                            permiso_.Attribute(CA_ACOTACION).Valor

                                    End If

                                Next

                                For Each tratado_ As Nodo In nodo_.Seccion(TIGIE6).Nodos

                                    For Each pais_ As Nodo In tratado_.Seccion(TIGIE7).Nodos

                                        If pais_.Attribute(CA_FECHA_FIN).Valor > Now() Then

                                            If pais_.Attribute(CA_PAIS).Valor.ToString.Contains("CHILE") Then

                                                fraccion_.tratados.Add(IIf(tratado_.Attribute(CA_NOMBRE_CORTO_TRATADO).Valor IsNot Nothing,
                                                                           tratado_.Attribute(CA_NOMBRE_CORTO_TRATADO).Valor, "") & "|%|" &
                                                                       pais_.Attribute(CA_ARANCEL).Valor & "|" &
                                                                       pais_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor)

                                            End If

                                        End If

                                    Next

                                Next

                                Dim cadena_ As String

                                For Each normas_ As Nodo In nodo_.Seccion(TIGIE14).Nodos

                                    If normas_.Attribute(CA_FECHA_FIN).Valor > Now() Then

                                        cadena_ = ""

                                        For Each identificador_ As Nodo In normas_.Seccion(TIGIE20).Nodos

                                            If cadena_ = "" Then

                                                cadena_ &= identificador_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor

                                            Else

                                                cadena_ &= ", " & identificador_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor

                                            End If

                                        Next

                                        fraccion_.normas.Add("Norma|" & cadena_ & "|" & normas_.Attribute(CA_NORMA).Valor)

                                    End If

                                Next

                                For Each anexo_ As Nodo In nodo_.Seccion(TIGIE15).Nodos

                                    fraccion_.anexos.Add(anexo_.Attribute(CA_NOMBRE).Valor)

                                Next

                        End Select

                    Next

                Next



                .ObjectReturned = fraccion_


            Catch ex As Exception

                .SetError(Me, ex.Message)

            End Try

            Return _Estado

        End With

    End Function

    Private Function GenerarVehiculo(fraccion_ As DocumentoElectronico,
                                     tipoOperacion_ As IControladorTIGIE.TipoOperacion,
                                     pais_ As String,
                                     fecha_ As Date) _
                                     As Object

        With _Estado

            Select Case tipoOperacion_
                Case IControladorTIGIE.TipoOperacion.Importacion

                    _seccionIE = fraccion_.Seccion(TIGIE2)

                Case IControladorTIGIE.TipoOperacion.Exportacion

                    _seccionIE = fraccion_.Seccion(TIGIE3)

            End Select

            Dim vehiculoFraccion_ = New VehiculoFraccion

            vehiculoFraccion_.fraccion = fraccion_.FolioOperacion

            vehiculoFraccion_.claveUnidadMedida = _seccionIE.Attribute(CA_CLAVE_UNIDAD_MEDIDA).Valor

            vehiculoFraccion_.unidadMedida = _seccionIE.Attribute(CA_UNIDAD_MEDIDA_CORTO).Valor
            'IMPUESTOS
            If _seccionIE.Seccion(TIGIE19).Nodos.Count > 0 Then

                vehiculoFraccion_.impuestos = New List(Of Impuestos)

                For Each impuesto_ As Nodo In _seccionIE.Seccion(TIGIE19).Nodos

                    Dim impuestos_ = New Impuestos

                    impuestos_.impuesto = impuesto_.Attribute(CA_NOMBRE_IMPUESTO_CORTO).Valor

                    impuestos_.tipoTasa = impuesto_.Attribute(CA_TIPO_TASA).Valor

                    impuestos_.tasa = impuesto_.Attribute(CA_VALOR_IMPUESTO).Valor

                    vehiculoFraccion_.impuestos.Add(impuestos_)

                Next

            End If
            'CUPOS ARANCEL
            If _seccionIE.Seccion(TIGIE8).Nodos.Count > 0 Then

                vehiculoFraccion_.cuposArancel = New List(Of CuposArancel)

                For Each cupo_ As Nodo In _seccionIE.Seccion(TIGIE8).Nodos

                    Dim cupos_ = New CuposArancel

                    cupos_.pais = cupo_.Attribute(CA_PAIS).Valor

                    cupos_.iconoPais = cupo_.Attribute(CA_ICONO_PAIS).Valor

                    cupos_.arancel = cupo_.Attribute(CA_ARANCEL).Valor

                    cupos_.arancelFuera = cupo_.Attribute(CA_ARANCEL_FUERA).Valor

                    cupos_.totalCupo = cupo_.Attribute(CA_TOTAL_CUPO).Valor

                    cupos_.medida = cupo_.Attribute(CA_UNIDAD_MEDIDA).Valor

                    vehiculoFraccion_.cuposArancel.Add(cupos_)

                Next

            End If
            'IEPS
            If _seccionIE.Seccion(TIGIE9).Nodos.Count > 0 Then

                vehiculoFraccion_.ieps = New List(Of Ieps)

                For Each ieps_ As Nodo In _seccionIE.Seccion(TIGIE9).Nodos

                    Dim iepsArancel_ = New Ieps

                    iepsArancel_.categoría = ieps_.Attribute(CA_CATEGORIA).Valor

                    iepsArancel_.cuota = ieps_.Attribute(CA_CUOTA).Valor

                    iepsArancel_.medida = ieps_.Attribute(CA_UNIDAD_MEDIDA).Valor

                    iepsArancel_.tasa = ieps_.Attribute(CA_TASA).Valor

                    iepsArancel_.tipo = ieps_.Attribute(CA_TIPO).Valor

                    iepsArancel_.observacion = ieps_.Attribute(CA_OBSERVACION).Valor

                    vehiculoFraccion_.ieps.Add(iepsArancel_)

                Next

            End If
            'CUOTAS COMPENSATORIAS
            If _seccionIE.Seccion(TIGIE10).Nodos.Count > 0 Then

                vehiculoFraccion_.cuotasCompensatorias = New List(Of CuotasCompensatorias)

                For Each cuotas_ As Nodo In _seccionIE.Seccion(TIGIE10).Nodos

                    Dim cuotasComsensatorias_ = New CuotasCompensatorias

                    cuotasComsensatorias_.empresa = cuotas_.Attribute(CA_EMPRESA).Valor

                    cuotasComsensatorias_.cuota = cuotas_.Attribute(CA_CUOTA).Valor

                    cuotasComsensatorias_.acotacion = cuotas_.Attribute(CA_ACOTACION).Valor

                    cuotasComsensatorias_.pais = cuotas_.Attribute(CA_PAIS).Valor

                    cuotasComsensatorias_.tipo = cuotas_.Attribute(CA_TIPO).Valor

                    vehiculoFraccion_.cuotasCompensatorias.Add(cuotasComsensatorias_)

                Next

            End If
            'PRECIOS ESTIMADOS
            If _seccionIE.Seccion(TIGIE11).Nodos.Count > 0 Then

                vehiculoFraccion_.preciosEstimados = New List(Of PreciosEstimados)

                For Each precios_ As Nodo In _seccionIE.Seccion(TIGIE11).Nodos

                    Dim preciosEstimados = New PreciosEstimados

                    preciosEstimados.unidad = precios_.Attribute(CA_UNIDAD_MEDIDA).Valor

                    preciosEstimados.precio = precios_.Attribute(CA_PRECIO).Valor

                    preciosEstimados.descripcion = precios_.Attribute(CA_DESCRIPCION).Valor

                    vehiculoFraccion_.preciosEstimados.Add(preciosEstimados)

                Next

            End If
            'TRATADOS
            If _seccionIE.Seccion(TIGIE6).Nodos.Count > 0 Then

                vehiculoFraccion_.tratados = New List(Of Tratados)

                For Each tratado_ As Nodo In _seccionIE.Seccion(TIGIE6).Nodos

                    For Each paises_ As Nodo In tratado_.Seccion(TIGIE7).Nodos

                        If paises_.Attribute(CA_FECHA_FIN).Valor > Now() Then

                            If paises_.Attribute(CA_PAIS).Valor.ToString.Contains(pais_) Then

                                Dim tratados_ = New Tratados

                                tratados_.tratado = IIf(tratado_.Attribute(CA_NOMBRE_CORTO_TRATADO).Valor IsNot Nothing, tratado_.Attribute(CA_NOMBRE_CORTO_TRATADO).Valor, "")

                                tratados_.tipoTasa = paises_.Attribute(CA_TIPO_TASA).Valor

                                tratados_.tasa = paises_.Attribute(CA_ARANCEL).Valor

                                tratados_.identificador = paises_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor

                                vehiculoFraccion_.tratados.Add(tratados_)

                            End If

                        End If

                    Next

                Next

            End If
            'ALADIS
            If _seccionIE.Seccion(TIGIE22).Nodos.Count > 0 Then

                vehiculoFraccion_.aladis = New List(Of Aladis)

                For Each aladis_ As Nodo In _seccionIE.Seccion(TIGIE22).Nodos

                    For Each paises_ As Nodo In aladis_.Seccion(TIGIE23).Nodos

                        If paises_.Attribute(CA_FECHA_FIN).Valor > Now() Then

                            If paises_.Attribute(CA_PAIS).Valor.ToString.Contains(pais_) Then

                                Dim aladi_ = New Aladis

                                aladi_.aladi = IIf(aladis_.Attribute(CA_NOMBRE_ALADI).Valor IsNot Nothing, aladis_.Attribute(CA_NOMBRE_ALADI).Valor, "")

                                aladi_.descuento = paises_.Attribute(CP_DESCUENTO).Valor

                                aladi_.identificador = paises_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor

                                vehiculoFraccion_.aladis.Add(aladi_)

                            End If

                        End If

                    Next

                Next

            End If
            'PERMISOS
            If _seccionIE.Seccion(TIGIE13).Nodos.Count > 0 Then

                vehiculoFraccion_.permiso = New List(Of Permisos)

                For Each permisos_ As Nodo In _seccionIE.Seccion(TIGIE13).Nodos

                    Dim permiso_ = New Permisos

                    permiso_.clave = permisos_.Attribute(CamposTarifaArancelaria.CA_CLAVE).Valor

                    permiso_.acotacion = permisos_.Attribute(CA_ACOTACION).Valor

                    vehiculoFraccion_.permiso.Add(permiso_)

                Next

            End If
            'NORMAS
            If _seccionIE.Seccion(TIGIE14).Nodos.Count > 0 Then

                vehiculoFraccion_.normas = New List(Of Normas)

                For Each normas_ As Nodo In _seccionIE.Seccion(TIGIE14).Nodos

                    Dim norma_ = New Normas

                    If normas_.Attribute(CA_FECHA_FIN).Valor > Now() Then

                        norma_.norma = normas_.Attribute(CA_NORMA).Valor

                        norma_.identificadores = New List(Of String)

                        For Each identificador_ As Nodo In normas_.Seccion(TIGIE20).Nodos

                            norma_.identificadores.Add(identificador_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor)

                        Next

                    End If

                Next

            End If
            'ANEXOS
            If _seccionIE.Seccion(TIGIE15).Nodos.Count > 0 Then

                vehiculoFraccion_.anexos = New List(Of String)

                For Each anexo_ As Nodo In _seccionIE.Seccion(TIGIE15).Nodos

                    vehiculoFraccion_.anexos.Add(anexo_.Attribute(CA_NOMBRE).Valor)

                Next

            End If
            'EMBARGOS
            'CUPOS MINIMOS
            'PADRON SECTORIAL







            Return vehiculoFraccion_

        End With

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

#End Region

End Class


Public Structure FraccionArancelaria

    Public Property Id As ObjectId
    Public Property Fraccion As String
    Public Property DescripcionFraccion As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String

End Structure
Public Structure NicoFraccionArancelaria
    Public Property Id As ObjectId
    Public Property Nico As String
    Public Property DescripcionNico As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
    Public Property UnidadesDeMedida As UnidadesDeMedida
    Public Property Impuestos As List(Of ImpuestoItem)
    Public Property Tratados As List(Of TratadoItem)
    Public Property Normas As List(Of NormaItem)
    Public Property Permisos As List(Of PermisoItem)
    Public Property CuotasCompensatorias As List(Of CuotaItem)
    Public Property PreciosEstimados As List(Of PrecioItem)
    'Public Property Restricciones As List(Of RestriccionItem)
    Public Property IEPS As List(Of IepsItem)
    Public Property PadronSectorial As List(Of PadronItem)
End Structure
Public Structure UnidadesDeMedida

    Public Property idUnidad As String
    Public Property ClaveUnidad As String
    Public Property ClaveCOVE As String
    Public Property Unidad As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String

End Structure
Public Structure ImpuestoItem
    Public Property idTipoTasa As String
    Public Property Tasa As String
    Public Property DescripcionTasa As String
    Public Property idTipoTarifa As String
    Public Property Tarifa As String
    Public Property idContribucion As String
    Public Property Contribucion As String
    Public Property ClaveContribucion As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
End Structure
Public Structure TratadoItem
    Public Property idPais As String
    Public Property NombrePais As String
    Public Property ClaveTratado As String
    Public Property idTipoTasa As String
    Public Property Tasa As String
    Public Property DescripcionTasa As String
    Public Property idIdentificador As String
    Public Property ClaveIdentificador As String
    Public Property Identificador As String
    Public Property idNota As String
    Public Property Nota As String
    Public Property FechaPublicacion As String
    Public Property FechaIncioVigencia As String
    Public Property FechaFinVigencia As String
End Structure
Public Structure NormaItem
    Public Property NOM As String
    Public Property Descripcion As String
    Public Property idAcuerdo As String
    Public Property Fraccion As String
    Public Property Acuerdo As String
    Public Property idArticulo As String
    Public Property NumeroArticulo As String
    Public Property Articulo As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
End Structure
Public Structure PermisoItem
    Public Property idPermiso As String
    Public Property ClavePermiso As String
    Public Property Permiso As String
    Public Property idIncisoPermiso As String
    Public Property Inciso As String
    Public Property Descripcion As String
    Public Property Particularidad As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
End Structure
Public Structure CuotaItem
    Public Property Producto As String
    Public Property idPais As String
    Public Property NombrePais As String
    Public Property idClaveEmpresa As String
    Public Property NombreEmpresa As String
    Public Property Tasa As String
    Public Property TipoCuota As String
    Public Property idUnidad As String
    Public Property ClaveUnidad As String
    Public Property ClaveCOVE As String
    Public Property Unidad As String
    Public Property idCCNota As String
    Public Property Nota As String
    Public Property Regimen As String
    Public Property TasaReferencia As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
End Structure
Public Structure PrecioItem
    Public Property Producto As String
    Public Property DetalleProducto As String
    Public Property idUnidadMedidaPE As String
    Public Property DescripcionUM As String
    Public Property PrecioEstimado As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
End Structure
Public Structure IepsItem
    Public Property idNotaIEPS As String
    Public Property idFraccion As String
    Public Property Nota As String
End Structure
Public Structure PadronItem
    Public Property idTIGIEAnexo As String
    Public Property idFraccion As String
    Public Property Clave As String
    Public Property SubClave As String
    Public Property Encabezado As String

End Structure
Public Structure Fraccion
    Public Property fraccion As String
    Public Property unidadMedida As String
    Public Property impuestos As List(Of String)
    Public Property tratados As List(Of String)
    Public Property permiso As String
    Public Property anexos As List(Of String)
    Public Property normas As List(Of String)
    Public Property cupo As String

End Structure
Public Structure VehiculoFraccion
    Public Property fraccion As String
    Public Property claveUnidadMedida As Int32
    Public Property unidadMedida As String
    Public Property impuestos As List(Of Impuestos)
    Public Property cuposArancel As List(Of CuposArancel)
    Public Property ieps As List(Of Ieps)
    Public Property cuotasCompensatorias As List(Of CuotasCompensatorias)
    Public Property preciosEstimados As List(Of PreciosEstimados)
    Public Property tratados As List(Of Tratados)
    Public Property aladis As List(Of Aladis)
    Public Property permiso As List(Of Permisos)
    Public Property normas As List(Of Normas)
    Public Property anexos As List(Of String)
    Public Property embargos As List(Of Embargos)
    Public Property cuposMinimos As List(Of CuposMinimos)
    Public Property padronSectorial As List(Of PadronSectorial)

End Structure
Public Structure Impuestos
    Public Property impuesto As String
    Public Property tipoTasa As Int32
    Public Property tasa As Decimal
End Structure
Public Structure CuposArancel
    Public Property pais As String
    Public Property iconoPais As String
    Public Property totalCupo As Int32
    Public Property arancel As String
    Public Property arancelFuera As String
    Public Property medida As Int32
End Structure
Public Structure Ieps
    Public Property categoría As String
    Public Property tipo As String
    Public Property tasa As Int32
    Public Property cuota As String
    Public Property medida As Int32
    Public Property observacion As String
End Structure
Public Structure CuotasCompensatorias
    Public Property empresa As String
    Public Property pais As String
    Public Property cuota As Int32
    Public Property tipo As String
    Public Property acotacion As String
End Structure
Public Structure PreciosEstimados
    Public Property precio As String
    Public Property unidad As String
    Public Property descripcion As String
End Structure
Public Structure Tratados
    Public Property tratado As String
    Public Property tipoTasa As Int32
    Public Property tasa As Int32
    Public Property identificador As String

End Structure
Public Structure Aladis
    Public Property aladi As String
    Public Property descuento As Decimal
    Public Property identificador As String

End Structure
Public Structure Permisos
    Public Property clave As String
    Public Property acotacion As String

End Structure
Public Structure Normas
    Public Property norma As String
    Public Property identificadores As List(Of String)

End Structure
Public Structure Embargos
    Public Property pais As String
    Public Property iconoPais As String
    Public Property aplicacion As String
    Public Property acotacion As String
    Public Property mercancia As String
End Structure
Public Structure CuposMinimos
    Public Property pais As String
    Public Property iconoPais As String
    Public Property unidad As String
    Public Property cupo As Int32
    Public Property descripcion As String
End Structure
Public Structure PadronSectorial
    Public Property sector As String
    Public Property anexo As String
    Public Property acotacion As String
    Public Property descripcion As String
End Structure