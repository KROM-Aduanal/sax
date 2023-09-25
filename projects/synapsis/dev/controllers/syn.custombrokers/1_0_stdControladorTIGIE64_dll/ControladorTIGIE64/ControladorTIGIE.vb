Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Syn.Nucleo.RecursosComercioExterior.CamposTarifaArancelaria

Public Class ControladorTIGIE

#Region "Enums"

    Public Enum TipoOperacion

        Importacion = 1
        Exportacion = 2

    End Enum

#End Region

#Region "Propiedades"



#End Region

#Region "Constructores"

    Sub New()

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

    Public Function EnlistarFracciones(ByVal texto_ As String) As TagWatcher

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

    Public Function EnlistarNicosFraccion(ByVal fraccion_ As String) As TagWatcher
        'Match(Function(e) e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento.Contains(fraccion_)).

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

    Public Function BuscarNico(ByVal id_ As ObjectId,
                               Optional ByVal tipoOperacion_ As TipoOperacion = TipoOperacion.Importacion,
                               Optional ByVal fecha_ As Date = Nothing,
                               Optional ByVal pais_ As String = Nothing) As TagWatcher

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
                        {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorTIGIE).Name)

            Dim resultadoDocumentos_ As New List(Of OperacionGenerica)

            Dim filtro_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, id_)

            resultadoDocumentos_ = operacionesDB_.Find(filtro_).Limit(1).ToList

            If resultadoDocumentos_.Count Then

                Dim nicoFracionArancelaria_ = New NicoFraccionArancelaria

                Dim documentoElectronico_ As DocumentoElectronico = resultadoDocumentos_(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                Dim seccionTarifaArancelaria_ As SeccionesTarifaArancelaria = IIf(tipoOperacion_ = TipoOperacion.Importacion, SeccionesTarifaArancelaria.TIGIE2, SeccionesTarifaArancelaria.TIGIE3)

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

                padron_.Sector = .Attribute(CA_SECTOR).Valor
                padron_.Notas = .Attribute(CA_DESCRIPCION).Valor

                dataPadrones_.Add(padron_)

            End With

        Next

        Return dataPadrones_

    End Function

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

'Public Structure RestriccionItem
'    Public Property idRestriccion As String
'    Public Property ClaveRestriccion As String
'    Public Property Restriccion As String
'    Public Property FechaPublicacion As String
'    Public Property FechaInicioVigencia As String
'    Public Property FechaFinVigencia As String
'End Structure

Public Structure IepsItem
    Public Property idNotaIEPS As String
    Public Property idFraccion As String
    Public Property Nota As String
End Structure

Public Structure PadronItem
    Public Property idTIGIEAnexo10 As String
    Public Property idFraccion As String
    Public Property Sector As String
    Public Property Notas As String
End Structure