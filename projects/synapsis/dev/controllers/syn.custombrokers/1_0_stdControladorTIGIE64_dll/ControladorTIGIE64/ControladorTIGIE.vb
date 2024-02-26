Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
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
Imports MongoDB.Bson.Serialization

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
    Public Function EnlistarFracciones(ByVal texto_ As String) As TagWatcher _
                            Implements IControladorTIGIE.EnlistarFracciones

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                   {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New ConstructorTIGIE()

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                Dim collection_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

                Dim regexFraccion_ As Regex = New Regex("^\d{4}\.\d{2}\.\d{2}$")

                Dim q = IIf(regexFraccion_.Match(texto_).Success,
                            Chr(39) & Chr(34) & texto_ & Chr(34) & Chr(39),
                            Chr(34) & String.Join(Chr(32), (From w In texto_.Trim().Split(Chr(32)) Select Chr(92) & Chr(34) & w & Chr(92) & Chr(34)).ToArray()) & Chr(34))

                Dim results = collection_.Aggregate().
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

    Public Function EnlistarNicosFraccion(ByVal fraccion_ As String) As TagWatcher _
                                        Implements IControladorTIGIE.EnlistarNicosFraccion

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                    {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New ConstructorTIGIE()

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                Dim collection_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(documentoElectronico_.GetType.Name)

                Dim results_ = collection_.Aggregate().
                                         Project(Function(e) New With {
                                            Key .Id = e.Id,
                                            Key .Seccion = e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos
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

                results_.AsEnumerable.ToList().ForEach(Sub(x)

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

    Public Function ConsultaFraccionArancelaria(Of T)(fraccion_ As String,
                                      tipoOperacion_ As IControladorTIGIE.TipoOperacion,
                                      pais_ As String,
                                       Optional ByVal fecha_ As Date = Nothing) As TagWatcher _
                                      Implements IControladorTIGIE.GetHsCode

        With _Estado

            If fecha_ = Nothing Then
                fecha_ = Now.Date
            End If

            Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(Activator.CreateInstance(Of T)().GetType.Name)

#Region "Aggregate Aplanador"

            Dim items_ = operacionesDB_.Aggregate().Match(Function(a) fraccion_.Equals(a.FolioOperacion)).
                    Project(Function(e) New With {
                        Key .Fraccion = e.FolioOperacion,
                        Key .Fuente = e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                    }).
                    Project(Function(e) New With {
                        Key .fraccion = e.Fraccion,
                        Key .importacion = e.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0)
                    }).
                    Project(Function(e) New With {
                        Key .fraccion = 1,
                        Key .claveUnidadMedida = DirectCast(e.importacion.Nodos(2).Nodos(0), Campo).Valor,
                        Key .unidadMedida = DirectCast(e.importacion.Nodos(1).Nodos(0), Campo).Valor,
                        Key .importacion = 1,
                        Key .impuestos = (e.importacion.Nodos(3).Nodos(0).Nodos),
                        Key .regulacionesArancelarias = (e.importacion.Nodos(4).Nodos(0)),
                        Key .regulacionesNoArancelarias = (e.importacion.Nodos(5).Nodos(0))
                    }).
                    Project(Function(e) New With {
                        Key .fraccion = 1,
                        Key .claveUnidadMedida = 1,
                        Key .unidadMedida = 1,
                        Key .importacion = 1,
                        Key .impuestos = 1,
                        Key .tratadosComerciales = e.regulacionesArancelarias.Nodos(0).Nodos(0).Nodos,
                        Key .cuposArancel = e.regulacionesArancelarias.Nodos(1).Nodos(0).Nodos,
                        Key .ieps = e.regulacionesArancelarias.Nodos(2).Nodos(0).Nodos,
                        Key .cuotasCompensatorias = e.regulacionesArancelarias.Nodos(3).Nodos(0).Nodos,
                        Key .preciosEstimados = e.regulacionesArancelarias.Nodos(4).Nodos(0).Nodos,
                        Key .aladis = e.regulacionesArancelarias.Nodos(5).Nodos(0).Nodos,
                        Key .permisos = e.regulacionesNoArancelarias.Nodos(0).Nodos(0).Nodos,
                        Key .normas = e.regulacionesNoArancelarias.Nodos(1).Nodos(0).Nodos,
                        Key .anexos = e.regulacionesNoArancelarias.Nodos(2).Nodos(0).Nodos,
                        Key .embargos = e.regulacionesNoArancelarias.Nodos(3).Nodos(0).Nodos,
                        Key .cuposMinimos = e.regulacionesNoArancelarias.Nodos(4).Nodos(0).Nodos,
                        Key .padronSectorial = e.regulacionesNoArancelarias.Nodos(5).Nodos(0).Nodos
                    }).
                    Unwind("tratadosComerciales").
                    Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", 1).
                        Add("tratadosComerciales", 1).
                        Add("paisesTratados",
                            New BsonDocument(
                                "$arrayElemAt", New BsonArray() From {
                                    New BsonDocument("$arrayElemAt",
                                        New BsonArray() From {
                                            "$tratadosComerciales.Nodos.Nodos.Nodos",
                                            3
                                        }
                                    ),
                                    0
                                }
                            )
                        ).
                        Add("cuposArancel", 1).
                        Add("ieps", 1).
                        Add("cuotasCompensatorias", 1).
                        Add("preciosEstimados", 1).
                        Add("aladis", 1).
                        Add("permisos", 1).
                        Add("normas", 1).
                        Add("anexos", 1).
                        Add("embargos", 1).
                        Add("cuposMinimos", 1).
                        Add("padronSectorial", 1)
                    ).AppendStage(Of BsonDocument)(
                        New BsonDocument("$addFields",
                            New BsonDocument().
                                Add("paisesTratados", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$paisesTratados").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$and", New BsonArray() From {
                                        New BsonDocument("$eq", New BsonArray() From {
                                            New BsonDocument("$ifNull", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                        "$$item.Nodos.Nodos.Valor",
                                                        15
                                                    }),
                                                    0
                                                }),
                                                "otro"
                                            }),
                                            pais_
                                        }),
                                        New BsonDocument("$gte", New BsonArray() From {
                                            New BsonDocument("$ifNull", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                        "$$item.Nodos.Nodos.Valor",
                                                        10
                                                    }),
                                                    0
                                                }),
                                                "otro"
                                            }),
                                            "2050-12-31"
                                        })
                                    }))
                                )
                            ).Add("normas", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$normas").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("impuestos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$impuestos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("cuposArancel", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$cuposArancel").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("ieps", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$ieps").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("cuotasCompensatorias", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$cuotasCompensatorias").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("preciosEstimados", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$preciosEstimados").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("aladis", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$aladis").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("permisos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$permisos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    5
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("anexos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$anexos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("embargos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$embargos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("cuposMinimos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$cuposMinimos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("padronSectorial", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$padronSectorial").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            )
                        )
                    ).Match(New BsonDocument().
                        Add("paisesTratados", New BsonDocument().
                            Add("$exists", True).
                            Add("$ne", New BsonArray())
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", "$impuestos.Nodos.Nodos").
                        Add("tratadosComerciales", 1).
                        Add("cuposArancel", "$cuposArancel.Nodos.Nodos").
                        Add("ieps", "$ieps.Nodos.Nodos").
                        Add("cuotasCompensatorias", "$cuotasCompensatorias.Nodos.Nodos").
                        Add("preciosEstimados", "$preciosEstimados.Nodos.Nodos").
                        Add("aladis", 1).
                        Add("paisesTratados", "$paisesTratados.Nodos.Nodos").
                        Add("permisos", "$permisos.Nodos.Nodos").
                        Add("normas", "$normas.Nodos.Nodos").
                        Add("anexos", "$anexos.Nodos.Nodos").
                        Add("embargos", "$embargos.Nodos.Nodos").
                        Add("cuposMinimos", "$cuposMinimos.Nodos.Nodos").
                        Add("padronSectorial", "$padronSectorial.Nodos.Nodos")
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$impuestos").
                                Add("as", "impuesto").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$impuesto").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {1, 2, 3}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("paisesTratados",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$paisesTratados").
                                Add("as", "tratado").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$tratado").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {4, 5, 11}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposArancel",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$cuposArancel").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4, 5}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("ieps",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$ieps").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4, 5}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuotasCompensatorias",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$cuotasCompensatorias").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("preciosEstimados",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$preciosEstimados").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("aladis", 1).
                        Add("tratadosComerciales",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$tratadosComerciales.Nodos").
                                Add("as", "tratado").
                                Add("in",
                                    New BsonDocument("$arrayElemAt",
                                        New BsonArray() From {
                                            "$$tratado.Nodos.Valor",
                                            0
                                        }
                                    )
                                )
                            )
                        ).
                        Add("permisos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$permisos").
                                Add("as", "permiso").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$permiso").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 2}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("normas",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$normas").
                                Add("as", "norma").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$norma").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 7}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("anexos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$anexos").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("embargos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$embargos").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposMinimos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$cuposMinimos").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("padronSectorial",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$padronSectorial").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$impuestos").
                                Add("as", "impuesto").
                                Add("in", New BsonDocument().
                                    Add("Impuesto", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$impuesto.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("TipoTasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$impuesto.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Tasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$impuesto.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("tratadosComerciales", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$paisesTratados").
                                Add("as", "tratado").
                                Add("in", New BsonDocument().
                                    Add("TipoTasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$tratado.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Tasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$tratado.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Identificador", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$tratado.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Tratado", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                "$tratadosComerciales",
                                                2
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposArancel", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposArancel").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("iconoPais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("totalCupo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("arancel", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("arancelFuera", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("medida", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        5
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("ieps", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$ieps").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("categoria", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("tipo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("tasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("cuota", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("medida", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("observacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        5
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuotasCompensatorias", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuotasCompensatorias").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("empresa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("cuota", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("tipo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("preciosEstimados", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$preciosEstimados").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("precio", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("unidad", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("descripcion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("aladis", 1).
                        Add("permisos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$permisos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("clave", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("normas", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$normas").
                                Add("as", "norma").
                                Add("in", New BsonDocument().
                                    Add("Nombre", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$norma.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Identificadores", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$norma.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("anexos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$anexos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("embargos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$embargos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("iconoPais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("aplicacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("mercancia", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposMinimos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposMinimos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("iconoPais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("unidad", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("cupo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("descripcion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("padronSectorial", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$padronSectorial").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("sector", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("anexo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("descripcion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$impuestos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("impuesto", "$$item.Impuesto.Valor").
                                    Add("tipoTasa", "$$item.TipoTasa.Valor").
                                    Add("tasa", "$$item.Tasa.Valor")
                                )
                            )
                        ).
                        Add("tratadosComerciales", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$tratadosComerciales").
                                Add("as", "tratado").
                                Add("in", New BsonDocument().
                                    Add("tratado", "$$tratado.Tratado").
                                    Add("tipoTasa", "$$tratado.TipoTasa.Valor").
                                    Add("tasa", "$$tratado.Tasa.Valor").
                                    Add("identificador", "$$tratado.Identificador.Valor")
                                )
                            )
                        ).
                        Add("cuposArancel", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposArancel").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", "$$item.pais.Valor").
                                    Add("iconoPais", "$$item.iconoPais.Valor").
                                    Add("totalCupo", "$$item.totalCupo.Valor").
                                    Add("arancel", "$$item.arancel.Valor").
                                    Add("arancelFuera", "$$item.arancelFuera.Valor").
                                    Add("medida", "$$item.medida.Valor")
                                )
                            )
                        ).
                        Add("ieps", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$ieps").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("categoría", "$$item.categoría.Valor").
                                    Add("tipo", "$$item.tipo.Valor").
                                    Add("tasa", "$$item.tasa.Valor").
                                    Add("cuota", "$$item.cuota.Valor").
                                    Add("medida", "$$item.medida.Valor").
                                    Add("observacion", "$$item.observacion.Valor")
                                )
                            )
                        ).
                        Add("cuotasCompensatorias", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuotasCompensatorias").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("empresa", "$$item.empresa.Valor").
                                    Add("pais", "$$item.pais.Valor").
                                    Add("cuota", "$$item.cuota.Valor").
                                    Add("tipo", "$$item.tipo.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor")
                                )
                            )
                        ).
                        Add("preciosEstimados", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$preciosEstimados").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("precio", "$$item.precio.Valor").
                                    Add("unidad", "$$item.unidad.Valor").
                                    Add("descripcion", "$$item.descripcion.Valor")
                                )
                            )
                        ).
                        Add("aladis", 1).
                        Add("permisos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$permisos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("clave", "$$item.clave.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor")
                                )
                            )
                        ).
                        Add("normas", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$normas").
                                Add("as", "norma").
                                Add("in", New BsonDocument().
                                    Add("norma", "$$norma.Nombre.Valor").
                                    Add("identificadores", New BsonDocument("$map",
                                        New BsonDocument().
                                            Add("input", "$$norma.Identificadores.Nodos").
                                            Add("as", "identificador").
                                            Add("in", New BsonDocument(
                                                "$arrayElemAt", New BsonArray() From {
                                                    New BsonDocument("$arrayElemAt",
                                                        New BsonArray() From {
                                                            "$$identificador.Nodos.Nodos.Valor",
                                                            0
                                                        }
                                                    ),
                                                    0
                                                })
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("anexos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$anexos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("anexo", "$$item.anexo.Valor")
                                )
                            )
                        ).
                        Add("embargos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$embargos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", "$$item.pais.Valor").
                                    Add("iconoPais", "$$item.iconoPais.Valor").
                                    Add("aplicacion", "$$item.aplicacion.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor").
                                    Add("mercancia", "$$item.mercancia.Valor")
                                )
                            )
                        ).
                        Add("cuposMinimos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposMinimos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", "$$item.pais.Valor").
                                    Add("iconoPais", "$$item.iconoPais.Valor").
                                    Add("unidad", "$$item.unidad.Valor").
                                    Add("cupo", "$$item.cupo.Valor").
                                    Add("descripcion", "$$item.descripcion.Valor")
                                )
                            )
                        ).
                        Add("padronSectorial", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$padronSectorial").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("sector", "$$item.sector.Valor").
                                    Add("anexo", "$$item.anexo.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor").
                                    Add("descripcion", "$$item.descripcion.Valor")
                                )
                            )
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", 1).
                        Add("regulacionesArancelarias",
                            New BsonDocument().
                                Add("tratadosComerciales", "$tratadosComerciales").
                                Add("aladis", "$aladis").
                                Add("cuposArancel", "$cuposArancel").
                                Add("ieps", "$ieps").
                                Add("cuotasCompensatorias", "$cuotasCompensatorias").
                                Add("preciosEstimados", "$preciosEstimados")
                        ).
                        Add("regulacionesNoArancelarias",
                            New BsonDocument().
                                Add("permisos", "$permisos").
                                Add("normas", "$normas").
                                Add("anexos", "$anexos").
                                Add("embargos", "$embargos").
                                Add("cuposMinimos", "$cuposMinimos").
                                Add("padronSectorial", "$padronSectorial")
                        )
                    ).ToList()

            '  regulacionesArancelarias

#End Region

            If items_.Count Then

                Dim item_ = items_(0)

                Dim constructorTigie_ = item_

                Dim fraccionArancelaria As hsCode = BsonSerializer.Deserialize(Of hsCode)(item_)

                .SetOK()

                .ObjectReturned = fraccionArancelaria

            Else

                .SetOKBut(Me, "No se encontró la fracción")

            End If


            Return _Estado

        End With

    End Function

    Public Function Pruebas() Implements IControladorTIGIE.Pruebas

#Region "Conexión"
        Dim client_ = New MongoClient("mongodb+srv://azamora87:lenM0M0N087@zamora0.hid645p.mongodb.net/?retryWrites=true&w=majority")
        Dim db_ As IMongoDatabase = client_.GetDatabase("Zamora")
        Dim Collection1_ = db_.GetCollection(Of Pruebas2)("Prueba2")
#End Region

        Dim prueba_ = Collection1_.Aggregate().
                Project(Function(e) New With {
                    Key .nombre = e.nombre,
                    Key .numero = e.numero
                }).
                Match(Function(a) Not a.numero.Equals(3)).
                Group(Function(ee) New With {
                    Key ._id = ee.nombre
                }, Function(g) New With {
                 .hhh = g.Key,
                 .numeroTotal = g.Sum(Function(x) x.numero)
                }).
                Project(Function(e) New With {
                    Key .numero = e.numeroTotal,
                    Key .nombre = e.hhh._id
                }).
                Sort("{numero: -1}").ToList

        Dim bsonDoc_ = Collection1_.Aggregate().
            Project(New BsonDocument().
                Add("nombre", 1).
                Add("numero", 1)
            ).
            Match(New BsonDocument().
                Add("numero", New BsonDocument().
                    Add("$ne", 3)
                )
            ).
            Group(New BsonDocument().
                Add("_id", "$nombre").
                Add("numero", New BsonDocument("$sum", "Snumero"))
            ).
            Project(New BsonDocument().
                Add("nombre", "$_id").
                Add("numero", 1)
            ).
            Sort(New BsonDocument("numero", -1)).ToList()

        Dim Prueba2_ As Pruebas2 = BsonSerializer.Deserialize(Of Pruebas2)(bsonDoc_(0))


        Dim project_ As PipelineStageDefinition(Of Pruebas2, BsonDocument) = BsonDocument.Parse("{ ""$project"": { ""nombre"": 1, ""numero"": 1 } }")
        Dim match_ As PipelineStageDefinition(Of BsonDocument, BsonDocument) = BsonDocument.Parse("{ ""$match"": { ""numero"": { ""$ne"": 3, } } }")
        Dim group_ As PipelineStageDefinition(Of BsonDocument, BsonDocument) = BsonDocument.Parse("{ $group: { _id:""$nombre"", numero: { $sum: ""$numero"" } } }")
        Dim project2_ As PipelineStageDefinition(Of BsonDocument, BsonDocument) = BsonDocument.Parse("{ $project: { nombre: ""$_id"", numero: 1 } }")
        Dim sort_ As BsonDocument = BsonDocument.Parse("{ $sort: { numero: -1 } } ")

        Dim prueba3_ = Collection1_.Aggregate().AppendStage(Of BsonDocument)(project_).
            AppendStage(Of BsonDocument)(match_).AppendStage(Of BsonDocument)(group_).
            AppendStage(Of BsonDocument)(project2_).AppendStage(Of BsonDocument)(sort_).ToList

        Dim project21_ = BsonDocument.Parse("{ ""$project"": { ""nombre"": 1, ""numero"": 1 } }")
        Dim match2_ = BsonDocument.Parse("{ ""$match"": { ""numero"": { ""$ne"": 3, } } }")
        Dim group2_ = BsonDocument.Parse("{ $group: { _id:""$nombre"", numero: { $sum: ""$numero"" } } }")
        Dim project22_ = BsonDocument.Parse("{ $project: { nombre: ""$_id"", numero: 1 } }")
        Dim sort2_ = BsonDocument.Parse("{ $sort: { numero: -1 } } ")

        Dim pipeline_ As PipelineDefinition(Of Pruebas2, BsonDocument) = New BsonDocument() {project21_, match2_, group2_, project22_, sort2_}

        Dim prueba4_ = Collection1_.Aggregate(Of BsonDocument)(pipeline_).ToList

    End Function

    Public Function Pruebas2() Implements IControladorTIGIE.Pruebas2

#Region "Conexión"
        Dim client_ = New MongoClient("mongodb+srv://azamora87:lenM0M0N087@zamora0.hid645p.mongodb.net/")
        Dim db_ As IMongoDatabase = client_.GetDatabase("Zamora")
        Dim Collection1_ = db_.GetCollection(Of Pruebas3)("Prueba3")
#End Region

        Dim bsonDoc_ = Collection1_.Aggregate().
            AppendStage(Of BsonDocument)(
                    New BsonDocument("$addFields",
                    New BsonDocument().Add("comentarios",
                        New BsonDocument("$filter",
                            New BsonDocument().
                                Add("input", "$comentarios").
                                Add("as", "comentario").
                                Add("cond",
                                    New BsonDocument("$or",
                                        New BsonArray From {
                                            New BsonDocument("$eq",
                                                New BsonArray From {
                                                    "fulano234",
                                                    "$$comentario.autor"
                                                }
                                            ),
                                            New BsonDocument("$eq",
                                                New BsonArray From {
                                                    "fulano543",
                                                    "$$comentario.autor"
                                                }
                                            )
                                        }
                                    )
                                )
                        )
                    )
                )
            ).
            Match(New BsonDocument("$and",
                New BsonArray From {
                        New BsonDocument("comentarios", New BsonDocument("$ne", New BsonArray)),
                        New BsonDocument("tags", New BsonDocument("$eq", "txt")),
                        New BsonDocument("tags", New BsonDocument("$eq", "doc"))
                    }
                )
            ).
            Unwind("tags").ToList()

        Dim prueba31_ As Pruebas3 = BsonSerializer.Deserialize(Of Pruebas3)(bsonDoc_(0))


        Dim addfields_ As PipelineStageDefinition(Of Pruebas3, BsonDocument) = BsonDocument.Parse("{""$addFields"":{""comentarios"":{""$filter"":{input:""$comentarios"",
            ""as"":""comentario"",""cond"":{""$or"":[{""$eq"":[""fulano234"",""$$comentario.autor""]},{ ""$eq"":[""fulano543"",""$$comentario.autor""]} ] } } } } }")
        Dim match_ As PipelineStageDefinition(Of BsonDocument, BsonDocument) = BsonDocument.Parse("{""$match"": { ""$and"": [ { ""comentarios"": { ""$ne"": [] } }, 
            { ""tags"": { ""$eq"": ""txt"" } }, { ""tags"": { ""$eq"": ""doc"" } } ] } }")
        Dim unwind_ As PipelineStageDefinition(Of BsonDocument, BsonDocument) = BsonDocument.Parse("{ ""$unwind"": { ""path"": ""$tags"" } }")

        Dim prueba32 = Collection1_.Aggregate().AppendStage(Of BsonDocument)(addfields_).
            AppendStage(Of BsonDocument)(match_).AppendStage(Of BsonDocument)(unwind_).ToList

        Dim addfields2_ As BsonDocument = BsonDocument.Parse("{""$addFields"":{""comentarios"":{""$filter"":{input:""$comentarios"",
            ""as"":""comentario"",""cond"":{""$or"":[{""$eq"":[""fulano234"",""$$comentario.autor""]},{ ""$eq"":[""fulano543"",""$$comentario.autor""]} ] } } } } }")
        Dim match2_ As BsonDocument = BsonDocument.Parse("{""$match"": { ""$and"": [ { ""comentarios"": { ""$ne"": [] } }, 
            { ""tags"": { ""$eq"": ""txt"" } }, { ""tags"": { ""$eq"": ""doc"" } } ] } }")
        Dim unwind2_ As BsonDocument = BsonDocument.Parse("{ ""$unwind"": { ""path"": ""$tags"" } }")

        Dim pipeline_ As PipelineDefinition(Of Pruebas3, BsonDocument) = New BsonDocument() {addfields2_, match2_, unwind2_}

        Dim prueba33_ = Collection1_.Aggregate(Of BsonDocument)(pipeline_).ToList

    End Function

    Public Function ConsultaFraccionArancelaria(Of T)(fracciones_ As List(Of String), 'getHsCode
                                      tipoOperacion_ As IControladorTIGIE.TipoOperacion,
                                      pais_ As String,
                                       Optional ByVal fecha_ As Date = Nothing) As TagWatcher _
        Implements IControladorTIGIE.GetHsCode

        Dim listaFracciones_ = New List(Of hsCode)

        With _Estado

            If fecha_ = Nothing Then

                fecha_ = Now.Date

            End If

            Dim operacionesDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(Activator.CreateInstance(Of T)().GetType.Name)

#Region "Aggregate Aplanador"

            Dim items_ = operacionesDB_.Aggregate().Match(Function(a) fracciones_.Contains(a.FolioOperacion)).
                    Project(Function(e) New With {
                        Key .Fraccion = e.FolioOperacion,
                        Key .Fuente = e.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                    }).
                    Project(Function(e) New With {
                        Key .fraccion = e.Fraccion,
                        Key .importacion = e.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0)
                    }).
                    Project(Function(e) New With {
                        Key .fraccion = 1,
                        Key .claveUnidadMedida = DirectCast(e.importacion.Nodos(2).Nodos(0), Campo).Valor,
                        Key .unidadMedida = DirectCast(e.importacion.Nodos(1).Nodos(0), Campo).Valor,
                        Key .importacion = 1,
                        Key .impuestos = (e.importacion.Nodos(3).Nodos(0).Nodos),
                        Key .regulacionesArancelarias = (e.importacion.Nodos(4).Nodos(0)),
                        Key .regulacionesNoArancelarias = (e.importacion.Nodos(5).Nodos(0))
                    }).
                    Project(Function(e) New With {
                        Key .fraccion = 1,
                        Key .claveUnidadMedida = 1,
                        Key .unidadMedida = 1,
                        Key .importacion = 1,
                        Key .impuestos = 1,
                        Key .tratadosComerciales = e.regulacionesArancelarias.Nodos(0).Nodos(0).Nodos,
                        Key .cuposArancel = e.regulacionesArancelarias.Nodos(1).Nodos(0).Nodos,
                        Key .ieps = e.regulacionesArancelarias.Nodos(2).Nodos(0).Nodos,
                        Key .cuotasCompensatorias = e.regulacionesArancelarias.Nodos(3).Nodos(0).Nodos,
                        Key .preciosEstimados = e.regulacionesArancelarias.Nodos(4).Nodos(0).Nodos,
                        Key .aladis = e.regulacionesArancelarias.Nodos(5).Nodos(0).Nodos,
                        Key .permisos = e.regulacionesNoArancelarias.Nodos(0).Nodos(0).Nodos,
                        Key .normas = e.regulacionesNoArancelarias.Nodos(1).Nodos(0).Nodos,
                        Key .anexos = e.regulacionesNoArancelarias.Nodos(2).Nodos(0).Nodos,
                        Key .embargos = e.regulacionesNoArancelarias.Nodos(3).Nodos(0).Nodos,
                        Key .cuposMinimos = e.regulacionesNoArancelarias.Nodos(4).Nodos(0).Nodos,
                        Key .padronSectorial = e.regulacionesNoArancelarias.Nodos(5).Nodos(0).Nodos
                    }).
                    Unwind("tratadosComerciales").
                    Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", 1).
                        Add("tratadosComerciales", 1).
                        Add("paisesTratados",
                            New BsonDocument(
                                "$arrayElemAt", New BsonArray() From {
                                    New BsonDocument("$arrayElemAt",
                                        New BsonArray() From {
                                            "$tratadosComerciales.Nodos.Nodos.Nodos",
                                            3
                                        }
                                    ),
                                    0
                                }
                            )
                        ).
                        Add("cuposArancel", 1).
                        Add("ieps", 1).
                        Add("cuotasCompensatorias", 1).
                        Add("preciosEstimados", 1).
                        Add("aladis", 1).
                        Add("permisos", 1).
                        Add("normas", 1).
                        Add("anexos", 1).
                        Add("embargos", 1).
                        Add("cuposMinimos", 1).
                        Add("padronSectorial", 1)
                    ).AppendStage(Of BsonDocument)(
                        New BsonDocument("$addFields",
                            New BsonDocument().
                                Add("paisesTratados", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$paisesTratados").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$and", New BsonArray() From {
                                        New BsonDocument("$eq", New BsonArray() From {
                                            New BsonDocument("$ifNull", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                        "$$item.Nodos.Nodos.Valor",
                                                        15
                                                    }),
                                                    0
                                                }),
                                                "otro"
                                            }),
                                            pais_
                                        }),
                                        New BsonDocument("$gte", New BsonArray() From {
                                            New BsonDocument("$ifNull", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                        "$$item.Nodos.Nodos.Valor",
                                                        10
                                                    }),
                                                    0
                                                }),
                                                "otro"
                                            }),
                                            "2050-12-31"
                                        })
                                    }))
                                )
                            ).Add("normas", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$normas").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("impuestos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$impuestos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("cuposArancel", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$cuposArancel").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("ieps", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$ieps").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("cuotasCompensatorias", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$cuotasCompensatorias").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("preciosEstimados", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$preciosEstimados").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("aladis", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$aladis").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("permisos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$permisos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    5
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("anexos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$anexos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("embargos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$embargos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("cuposMinimos", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$cuposMinimos").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            ).
                            Add("padronSectorial", New BsonDocument("$filter",
                                New BsonDocument().
                                    Add("input", "$padronSectorial").
                                    Add("as", "item").
                                    Add("cond", New BsonDocument("$gte", New BsonArray() From {
                                        New BsonDocument("$ifNull", New BsonArray() From {
                                            New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt", New BsonArray() From {
                                                    "$$item.Nodos.Nodos.Valor",
                                                    6
                                                }),
                                                0
                                            }),
                                            "otro"
                                        }),
                                        "2050-12-31"
                                        })
                                    )
                                )
                            )
                        )
                    ).Match(New BsonDocument().
                        Add("paisesTratados", New BsonDocument().
                            Add("$exists", True).
                            Add("$ne", New BsonArray())
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", "$impuestos.Nodos.Nodos").
                        Add("tratadosComerciales", 1).
                        Add("cuposArancel", "$cuposArancel.Nodos.Nodos").
                        Add("ieps", "$ieps.Nodos.Nodos").
                        Add("cuotasCompensatorias", "$cuotasCompensatorias.Nodos.Nodos").
                        Add("preciosEstimados", "$preciosEstimados.Nodos.Nodos").
                        Add("aladis", 1).
                        Add("paisesTratados", "$paisesTratados.Nodos.Nodos").
                        Add("permisos", "$permisos.Nodos.Nodos").
                        Add("normas", "$normas.Nodos.Nodos").
                        Add("anexos", "$anexos.Nodos.Nodos").
                        Add("embargos", "$embargos.Nodos.Nodos").
                        Add("cuposMinimos", "$cuposMinimos.Nodos.Nodos").
                        Add("padronSectorial", "$padronSectorial.Nodos.Nodos")
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$impuestos").
                                Add("as", "impuesto").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$impuesto").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {1, 2, 3}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("paisesTratados",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$paisesTratados").
                                Add("as", "tratado").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$tratado").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {4, 5, 11}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposArancel",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$cuposArancel").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4, 5}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("ieps",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$ieps").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4, 5}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuotasCompensatorias",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$cuotasCompensatorias").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("preciosEstimados",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$preciosEstimados").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("aladis", 1).
                        Add("tratadosComerciales",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$tratadosComerciales.Nodos").
                                Add("as", "tratado").
                                Add("in",
                                    New BsonDocument("$arrayElemAt",
                                        New BsonArray() From {
                                            "$$tratado.Nodos.Valor",
                                            0
                                        }
                                    )
                                )
                            )
                        ).
                        Add("permisos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$permisos").
                                Add("as", "permiso").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$permiso").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 2}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("normas",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$normas").
                                Add("as", "norma").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$norma").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 7}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("anexos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$anexos").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("embargos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$embargos").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposMinimos",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$cuposMinimos").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3, 4}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("padronSectorial",
                            New BsonDocument("$map", New BsonDocument().
                                Add("input", "$padronSectorial").
                                Add("as", "item").
                                Add("in",
                                    New BsonDocument("$reduce", New BsonDocument().
                                        Add("input", "$$item").
                                        Add("initialValue",
                                            New BsonDocument().
                                                Add("filteredArray", New BsonArray()).
                                                Add("currentIndex", 0)
                                        ).
                                        Add("in", New BsonDocument().
                                            Add("filteredArray",
                                                New BsonDocument("$cond", New BsonDocument().
                                                    Add("if",
                                                        New BsonDocument("$in",
                                                            New BsonArray() From {
                                                                "$$value.currentIndex",
                                                                New BsonArray() From {0, 1, 2, 3}
                                                            }
                                                        )
                                                    ).
                                                    Add("then",
                                                        New BsonDocument("$concatArrays",
                                                            New BsonArray() From {
                                                                "$$value.filteredArray",
                                                                New BsonArray() From {"$$this"}
                                                            }
                                                        )
                                                    ).
                                                    Add("else", "$$value.filteredArray")
                                                )
                                            ).
                                            Add("currentIndex",
                                                New BsonDocument("$add",
                                                    New BsonArray() From {
                                                        "$$value.currentIndex",
                                                        1
                                                    }
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$impuestos").
                                Add("as", "impuesto").
                                Add("in", New BsonDocument().
                                    Add("Impuesto", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$impuesto.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("TipoTasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$impuesto.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Tasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$impuesto.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("tratadosComerciales", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$paisesTratados").
                                Add("as", "tratado").
                                Add("in", New BsonDocument().
                                    Add("TipoTasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$tratado.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Tasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$tratado.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Identificador", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$tratado.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Tratado", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                "$tratadosComerciales",
                                                2
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposArancel", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposArancel").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("iconoPais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("totalCupo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("arancel", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("arancelFuera", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("medida", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        5
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("ieps", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$ieps").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("categoria", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("tipo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("tasa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("cuota", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("medida", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("observacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        5
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuotasCompensatorias", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuotasCompensatorias").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("empresa", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("cuota", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("tipo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("preciosEstimados", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$preciosEstimados").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("precio", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("unidad", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("descripcion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("aladis", 1).
                        Add("permisos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$permisos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("clave", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("normas", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$normas").
                                Add("as", "norma").
                                Add("in", New BsonDocument().
                                    Add("Nombre", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$norma.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("Identificadores", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$norma.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("anexos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$anexos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("embargos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$embargos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("iconoPais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("aplicacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("mercancia", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("cuposMinimos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposMinimos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("iconoPais", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("unidad", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("cupo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("descripcion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        4
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        ).
                        Add("padronSectorial", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$padronSectorial").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("sector", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        0
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("anexo", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        1
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("acotacion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        2
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    ).
                                    Add("descripcion", New BsonDocument(
                                            "$arrayElemAt", New BsonArray() From {
                                                New BsonDocument("$arrayElemAt",
                                                    New BsonArray() From {
                                                        "$$item.filteredArray",
                                                        3
                                                    }
                                                ),
                                                0
                                            }
                                        )
                                    )
                                )
                            )
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$impuestos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("impuesto", "$$item.Impuesto.Valor").
                                    Add("tipoTasa", "$$item.TipoTasa.Valor").
                                    Add("tasa", "$$item.Tasa.Valor")
                                )
                            )
                        ).
                        Add("tratadosComerciales", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$tratadosComerciales").
                                Add("as", "tratado").
                                Add("in", New BsonDocument().
                                    Add("tratado", "$$tratado.Tratado").
                                    Add("tipoTasa", "$$tratado.TipoTasa.Valor").
                                    Add("tasa", "$$tratado.Tasa.Valor").
                                    Add("identificador", "$$tratado.Identificador.Valor")
                                )
                            )
                        ).
                        Add("cuposArancel", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposArancel").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", "$$item.pais.Valor").
                                    Add("iconoPais", "$$item.iconoPais.Valor").
                                    Add("totalCupo", "$$item.totalCupo.Valor").
                                    Add("arancel", "$$item.arancel.Valor").
                                    Add("arancelFuera", "$$item.arancelFuera.Valor").
                                    Add("medida", "$$item.medida.Valor")
                                )
                            )
                        ).
                        Add("ieps", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$ieps").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("categoría", "$$item.categoría.Valor").
                                    Add("tipo", "$$item.tipo.Valor").
                                    Add("tasa", "$$item.tasa.Valor").
                                    Add("cuota", "$$item.cuota.Valor").
                                    Add("medida", "$$item.medida.Valor").
                                    Add("observacion", "$$item.observacion.Valor")
                                )
                            )
                        ).
                        Add("cuotasCompensatorias", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuotasCompensatorias").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("empresa", "$$item.empresa.Valor").
                                    Add("pais", "$$item.pais.Valor").
                                    Add("cuota", "$$item.cuota.Valor").
                                    Add("tipo", "$$item.tipo.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor")
                                )
                            )
                        ).
                        Add("preciosEstimados", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$preciosEstimados").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("precio", "$$item.precio.Valor").
                                    Add("unidad", "$$item.unidad.Valor").
                                    Add("descripcion", "$$item.descripcion.Valor")
                                )
                            )
                        ).
                        Add("aladis", 1).
                        Add("permisos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$permisos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("clave", "$$item.clave.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor")
                                )
                            )
                        ).
                        Add("normas", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$normas").
                                Add("as", "norma").
                                Add("in", New BsonDocument().
                                    Add("norma", "$$norma.Nombre.Valor").
                                    Add("identificadores", New BsonDocument("$map",
                                        New BsonDocument().
                                            Add("input", "$$norma.Identificadores.Nodos").
                                            Add("as", "identificador").
                                            Add("in", New BsonDocument(
                                                "$arrayElemAt", New BsonArray() From {
                                                    New BsonDocument("$arrayElemAt",
                                                        New BsonArray() From {
                                                            "$$identificador.Nodos.Nodos.Valor",
                                                            0
                                                        }
                                                    ),
                                                    0
                                                })
                                            )
                                        )
                                    )
                                )
                            )
                        ).
                        Add("anexos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$anexos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("anexo", "$$item.anexo.Valor")
                                )
                            )
                        ).
                        Add("embargos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$embargos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", "$$item.pais.Valor").
                                    Add("iconoPais", "$$item.iconoPais.Valor").
                                    Add("aplicacion", "$$item.aplicacion.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor").
                                    Add("mercancia", "$$item.mercancia.Valor")
                                )
                            )
                        ).
                        Add("cuposMinimos", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$cuposMinimos").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("pais", "$$item.pais.Valor").
                                    Add("iconoPais", "$$item.iconoPais.Valor").
                                    Add("unidad", "$$item.unidad.Valor").
                                    Add("cupo", "$$item.cupo.Valor").
                                    Add("descripcion", "$$item.descripcion.Valor")
                                )
                            )
                        ).
                        Add("padronSectorial", New BsonDocument("$map",
                            New BsonDocument().
                                Add("input", "$padronSectorial").
                                Add("as", "item").
                                Add("in", New BsonDocument().
                                    Add("sector", "$$item.sector.Valor").
                                    Add("anexo", "$$item.anexo.Valor").
                                    Add("acotacion", "$$item.acotacion.Valor").
                                    Add("descripcion", "$$item.descripcion.Valor")
                                )
                            )
                        )
                    ).Project(New BsonDocument().
                        Add("fraccion", 1).
                        Add("claveUnidadMedida", 1).
                        Add("unidadMedida", 1).
                        Add("impuestos", 1).
                        Add("regulacionesArancelarias",
                            New BsonDocument().
                                Add("tratadosComerciales", "$tratadosComerciales").
                                Add("aladis", "$aladis").
                                Add("cuposArancel", "$cuposArancel").
                                Add("ieps", "$ieps").
                                Add("cuotasCompensatorias", "$cuotasCompensatorias").
                                Add("preciosEstimados", "$preciosEstimados")
                        ).
                        Add("regulacionesNoArancelarias",
                            New BsonDocument().
                                Add("permisos", "$permisos").
                                Add("normas", "$normas").
                                Add("anexos", "$anexos").
                                Add("embargos", "$embargos").
                                Add("cuposMinimos", "$cuposMinimos").
                                Add("padronSectorial", "$padronSectorial")
                        )
                    ).ToList()

#End Region

            If items_.Count Then

                For Each item_ In items_

                    Dim constructorTigie_ = item_

                    Dim fraccionArancelaria As hsCode = BsonSerializer.Deserialize(Of hsCode)(item_)

                    .SetOK()

                    listaFracciones_.Add(fraccionArancelaria)

                Next

                .ObjectReturned = listaFracciones_

            Else

                .SetOKBut(Me, "No se encontró la fracción")

            End If


            Return _Estado

        End With

    End Function

    Private Function GetTratados(ByRef documentoElectronico_ As DocumentoElectronico, ByRef seccionTarifaArancelaria_ As SeccionesTarifaArancelaria)

        Dim dataTratados_ As New List(Of TratadoItem)

        Dim tratados_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE6)

        For indice_ As Int32 = 1 To tratados_.CantidadPartidas

            With tratados_.Partida(indice_)

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

    Private Function GenerarVehiculo(fraccion_ As DocumentoElectronico,
                                     tipoOperacion_ As IControladorTIGIE.TipoOperacion, 'enum preferenciasarancelarias/nopreferenciasaaa/ambasdos
                                     pais_ As String,  'notas
                                     fecha_ As Date) _
                                     As hsCode

        With _Estado

            Select Case tipoOperacion_

                Case IControladorTIGIE.TipoOperacion.Importacion

                    _seccionIE = fraccion_.Seccion(TIGIE2)

                Case IControladorTIGIE.TipoOperacion.Exportacion

                    _seccionIE = fraccion_.Seccion(TIGIE3)

            End Select

            Dim vehiculoFraccion_ = New hsCode

            vehiculoFraccion_.fraccion = fraccion_.FolioOperacion

            vehiculoFraccion_.claveUnidadMedida = _seccionIE.Attribute(CA_CLAVE_UNIDAD_MEDIDA).Valor

            vehiculoFraccion_.unidadMedida = _seccionIE.Attribute(CA_UNIDAD_MEDIDA_CORTO).Valor
            'IMPUESTOS
            If _seccionIE.Seccion(TIGIE19).Nodos.Count > 0 Then

                vehiculoFraccion_.impuestos = New List(Of impuesto)

                For Each impuesto_ As Nodo In _seccionIE.Seccion(TIGIE19).Nodos.Where(Function(g) Convert.ToDateTime(g.Attribute(CA_FECHA_FIN).Valor) >= fecha_)

                    Dim impuestos_ = New impuesto

                    impuestos_.impuesto = impuesto_.Attribute(CA_NOMBRE_IMPUESTO_CORTO).Valor

                    impuestos_.tipoTasa = impuesto_.Attribute(CA_TIPO_TASA).Valor

                    impuestos_.tasa = impuesto_.Attribute(CA_VALOR_IMPUESTO).Valor

                    vehiculoFraccion_.impuestos.Add(impuestos_)

                Next

            End If

            Dim regulacionesArancelarias_ = New regulacionesArancelarias

            Dim regulacionesNoArancelarias_ = New regulacionesNoArancelarias
            'CUPOS ARANCEL
            If _seccionIE.Seccion(TIGIE8).Nodos.Count > 0 Then

                regulacionesArancelarias_.cuposArancel = New List(Of cupoArancel)

                For Each cupo_ As Nodo In _seccionIE.Seccion(TIGIE8).Nodos

                    Dim cupos_ = New cupoArancel

                    cupos_.pais = cupo_.Attribute(CA_PAIS).Valor

                    cupos_.iconoPais = cupo_.Attribute(CA_ICONO_PAIS).Valor

                    cupos_.arancel = cupo_.Attribute(CA_ARANCEL).Valor

                    cupos_.arancelFuera = cupo_.Attribute(CA_ARANCEL_FUERA).Valor

                    cupos_.totalCupo = cupo_.Attribute(CA_TOTAL_CUPO).Valor

                    cupos_.medida = cupo_.Attribute(CA_UNIDAD_MEDIDA).Valor

                    regulacionesArancelarias_.cuposArancel.Add(cupos_)

                Next

            End If
            'IEPS
            If _seccionIE.Seccion(TIGIE9).Nodos.Count > 0 Then

                regulacionesArancelarias_.ieps = New List(Of Ieps)

                For Each ieps_ As Nodo In _seccionIE.Seccion(TIGIE9).Nodos

                    Dim iepsArancel_ = New Ieps

                    iepsArancel_.categoría = ieps_.Attribute(CA_CATEGORIA).Valor

                    iepsArancel_.cuota = ieps_.Attribute(CA_CUOTA).Valor

                    iepsArancel_.medida = ieps_.Attribute(CA_UNIDAD_MEDIDA).Valor

                    iepsArancel_.tasa = ieps_.Attribute(CA_TASA).Valor

                    iepsArancel_.tipo = ieps_.Attribute(CA_TIPO).Valor

                    iepsArancel_.observacion = ieps_.Attribute(CA_OBSERVACION).Valor

                    regulacionesArancelarias_.ieps.Add(iepsArancel_)

                Next

            End If
            'CUOTAS COMPENSATORIAS
            If _seccionIE.Seccion(TIGIE10).Nodos.Count > 0 Then

                regulacionesArancelarias_.cuotasCompensatorias = New List(Of cuotaCompensatoria)

                For Each cuotas_ As Nodo In _seccionIE.Seccion(TIGIE10).Nodos

                    Dim cuotasComsensatorias_ = New cuotaCompensatoria

                    cuotasComsensatorias_.empresa = cuotas_.Attribute(CA_EMPRESA).Valor

                    cuotasComsensatorias_.cuota = cuotas_.Attribute(CA_CUOTA).Valor

                    cuotasComsensatorias_.acotacion = cuotas_.Attribute(CA_ACOTACION).Valor

                    cuotasComsensatorias_.pais = cuotas_.Attribute(CA_PAIS).Valor

                    cuotasComsensatorias_.tipo = cuotas_.Attribute(CA_TIPO).Valor

                    regulacionesArancelarias_.cuotasCompensatorias.Add(cuotasComsensatorias_)

                Next

            End If
            'PRECIOS ESTIMADOS
            If _seccionIE.Seccion(TIGIE11).Nodos.Count > 0 Then

                regulacionesArancelarias_.preciosEstimados = New List(Of precioEstimado)

                For Each precios_ As Nodo In _seccionIE.Seccion(TIGIE11).Nodos

                    Dim preciosEstimados = New precioEstimado

                    preciosEstimados.unidad = precios_.Attribute(CA_UNIDAD_MEDIDA).Valor

                    preciosEstimados.precio = precios_.Attribute(CA_PRECIO).Valor

                    preciosEstimados.descripcion = precios_.Attribute(CA_DESCRIPCION).Valor

                    regulacionesArancelarias_.preciosEstimados.Add(preciosEstimados)

                Next

            End If
            'TRATADOS
            If _seccionIE.Seccion(TIGIE6).Nodos.Count > 0 Then

                regulacionesArancelarias_.tratadosComerciales = New List(Of tratado)

                For Each tratado_ As Nodo In _seccionIE.Seccion(TIGIE6).Nodos

                    For Each paises_ As Nodo In tratado_.Seccion(TIGIE7).Nodos.Where(Function(g) Convert.ToDateTime(g.Attribute(CA_FECHA_FIN).Valor) > fecha_ _
                                                    And g.Attribute(CP_NOMBRECORTO_PAIS).Valor.ToString.Equals(pais_))

                        Dim tratados_ = New tratado

                        tratados_.tratado = IIf(tratado_.Attribute(CA_NOMBRE_CORTO_TRATADO).Valor IsNot Nothing, tratado_.Attribute(CA_NOMBRE_CORTO_TRATADO).Valor, "")

                        tratados_.tipoTasa = paises_.Attribute(CA_TIPO_TASA).Valor

                        tratados_.tasa = paises_.Attribute(CA_ARANCEL).Valor

                        tratados_.identificador = paises_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor

                        regulacionesArancelarias_.tratadosComerciales.Add(tratados_)

                    Next

                Next

            End If
            'ALADIS
            If _seccionIE.Seccion(TIGIE22).Nodos.Count > 0 Then

                regulacionesArancelarias_.aladis = New List(Of aladi)

                For Each aladis_ As Nodo In _seccionIE.Seccion(TIGIE22).Nodos

                    For Each paises_ As Nodo In aladis_.Seccion(TIGIE23).Nodos.Where(Function(g) Convert.ToDateTime(g.Attribute(CA_FECHA_FIN).Valor) > fecha_ _
                                                    And g.Attribute(CA_PAIS).Valor.ToString.Contains(pais_))

                        Dim aladi_ = New aladi

                        aladi_.aladi = IIf(aladis_.Attribute(CA_NOMBRE_ALADI).Valor IsNot Nothing, aladis_.Attribute(CA_NOMBRE_ALADI).Valor, "")

                        aladi_.descuento = paises_.Attribute(CP_DESCUENTO).Valor

                        aladi_.identificador = paises_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor

                        regulacionesArancelarias_.aladis.Add(aladi_)

                    Next

                Next

            End If
            'PERMISOS
            If _seccionIE.Seccion(TIGIE13).Nodos.Count > 0 Then

                regulacionesNoArancelarias_.permisos = New List(Of permiso)

                For Each permisos_ As Nodo In _seccionIE.Seccion(TIGIE13).Nodos.Where(Function(g) Convert.ToDateTime(g.Attribute(CA_FECHA_FIN).Valor) > fecha_)

                    Dim permiso_ = New permiso

                    permiso_.clave = permisos_.Attribute(CamposTarifaArancelaria.CA_CLAVE).Valor

                    permiso_.acotacion = permisos_.Attribute(CA_ACOTACION).Valor

                    regulacionesNoArancelarias_.permisos.Add(permiso_)

                Next

            End If
            'NORMAS
            If _seccionIE.Seccion(TIGIE14).Nodos.Count > 0 Then

                regulacionesNoArancelarias_.normas = New List(Of norma)

                For Each normas_ As Nodo In _seccionIE.Seccion(TIGIE14).Nodos.Where(Function(g) Convert.ToDateTime(g.Attribute(CA_FECHA_FIN).Valor) > fecha_)

                    Dim norma_ = New norma

                    norma_.norma = normas_.Attribute(CA_NORMA).Valor

                    norma_.identificadores = New List(Of String)

                    For Each identificador_ As Nodo In normas_.Seccion(TIGIE20).Nodos

                        norma_.identificadores.Add(identificador_.Attribute(CA_CLAVE_IDENTIFICADOR).Valor)

                    Next

                    regulacionesNoArancelarias_.normas.Add(norma_)

                Next

            End If
            'ANEXOS
            If _seccionIE.Seccion(TIGIE15).Nodos.Count > 0 Then

                regulacionesNoArancelarias_.anexos = New List(Of String)

                For Each anexo_ As Nodo In _seccionIE.Seccion(TIGIE15).Nodos

                    regulacionesNoArancelarias_.anexos.Add(anexo_.Attribute(CA_NOMBRE).Valor)

                Next

            End If
            'EMBARGOS
            'CUPOS MINIMOS
            'PADRON SECTORIAL

            vehiculoFraccion_.regulacionesArancelarias = regulacionesArancelarias_

            vehiculoFraccion_.regulacionesNoArancelarias = regulacionesNoArancelarias_

            Return vehiculoFraccion_

        End With

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not Me.disposedValue Then

            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
            End If

            'Propiedades no administradas

            With Me

                .Estado.Clear()


            End With

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If

        Me.disposedValue = True

    End Sub


    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

#End Region

End Class
<Serializable()>
Public Class Pruebas3

    Public Property _id As Object
    <BsonIgnoreIfNull>
    Public Property autor As String
    <BsonIgnoreIfNull>
    Public Property titulo As String
    <BsonIgnoreIfNull>
    Public Property texto As String
    <BsonIgnoreIfNull>
    Public Property tags As String
    <BsonIgnoreIfNull>
    Public Property comentarios As List(Of Object)

End Class

Public Class Comentario
    Public Property autor As String
    Public Property comentario As String

End Class

<Serializable()>
Public Class Pruebas2
    Public Property _id As String
    <BsonIgnoreIfNull>
    Public Property nombre As String
    <BsonIgnoreIfNull>
    Public Property numero As Int32
    <BsonIgnoreIfNull>
    Public Property datox As String

End Class

Public Class FraccionArancelaria

    Public Property Id As ObjectId
    Public Property Fraccion As String
    Public Property DescripcionFraccion As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String

End Class
Public Class NicoFraccionArancelaria
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
End Class
Public Class UnidadesDeMedida

    Public Property idUnidad As String
    Public Property ClaveUnidad As String
    Public Property ClaveCOVE As String
    Public Property Unidad As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String

End Class
Public Class ImpuestoItem
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
End Class
Public Class TratadoItem
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
End Class
Public Class NormaItem
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
End Class
Public Class PermisoItem
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
End Class
Public Class CuotaItem
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
End Class
Public Class PrecioItem
    Public Property Producto As String
    Public Property DetalleProducto As String
    Public Property idUnidadMedidaPE As String
    Public Property DescripcionUM As String
    Public Property PrecioEstimado As String
    Public Property FechaPublicacion As String
    Public Property FechaInicioVigencia As String
    Public Property FechaFinVigencia As String
End Class
Public Class IepsItem
    Public Property idNotaIEPS As String
    Public Property idFraccion As String
    Public Property Nota As String
End Class
Public Class PadronItem
    Public Property idTIGIEAnexo As String
    Public Property idFraccion As String
    Public Property Clave As String
    Public Property SubClave As String
    Public Property Encabezado As String

End Class
Public Class hsCode
    Public Property fraccion As String
    Public Property claveUnidadMedida As Int32
    Public Property unidadMedida As String
    Public Property impuestos As List(Of impuesto)
    Public Property regulacionesArancelarias As New regulacionesArancelarias
    Public Property regulacionesNoArancelarias As regulacionesNoArancelarias

End Class
Public Class impuesto
    Public Property impuesto As String
    Public Property tipoTasa As Int64
    Public Property tasa As Double
End Class
Public Class regulacionesArancelarias
    Public Property tratadosComerciales As List(Of tratado)
    Public Property aladis As List(Of aladi)
    Public Property cuposArancel As List(Of cupoArancel)
    Public Property ieps As List(Of Ieps)
    Public Property cuotasCompensatorias As List(Of cuotaCompensatoria)
    Public Property preciosEstimados As List(Of precioEstimado)

End Class
Public Class regulacionesNoArancelarias
    Public Property permisos As List(Of permiso)
    Public Property normas As List(Of norma)
    Public Property anexos As List(Of String)
    Public Property embargos As List(Of embargo)
    Public Property cuposMinimos As List(Of cupoMinimo)
    Public Property padronSectorial As List(Of padronSectorial)

End Class
Public Class cupoArancel
    Public Property pais As String
    Public Property iconoPais As String
    Public Property totalCupo As Int32
    Public Property arancel As String
    Public Property arancelFuera As String
    Public Property medida As Int32
End Class
Public Class Ieps
    Public Property categoría As String
    Public Property tipo As String
    Public Property tasa As Int32
    Public Property cuota As String
    Public Property medida As Int32
    Public Property observacion As String
End Class
Public Class cuotaCompensatoria
    Public Property empresa As String
    Public Property pais As String
    Public Property cuota As Int32
    Public Property tipo As String
    Public Property acotacion As String
End Class
Public Class precioEstimado
    Public Property precio As String
    Public Property unidad As String
    Public Property descripcion As String
End Class
Public Class tratado
    Public Property tratado As String
    Public Property tipoTasa As Int32
    Public Property tasa As Double
    Public Property identificador As String

End Class
Public Class aladi
    Public Property aladi As String
    Public Property descuento As Double
    Public Property identificador As String

End Class
Public Class permiso
    Public Property clave As String
    Public Property acotacion As String

End Class
Public Class norma
    Public Property norma As String
    Public Property identificadores As List(Of String)

End Class
Public Class embargo
    Public Property pais As String
    Public Property iconoPais As String
    Public Property aplicacion As String
    Public Property acotacion As String
    Public Property mercancia As String
End Class
Public Class cupoMinimo
    Public Property pais As String
    Public Property iconoPais As String
    Public Property unidad As String
    Public Property cupo As Int32
    Public Property descripcion As String
End Class
Public Class padronSectorial
    Public Property sector As String
    Public Property anexo As String
    Public Property acotacion As String
    Public Property descripcion As String
End Class
