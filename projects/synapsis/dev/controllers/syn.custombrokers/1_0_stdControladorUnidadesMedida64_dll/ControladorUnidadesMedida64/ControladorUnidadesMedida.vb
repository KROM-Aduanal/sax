Imports System.Security.Cryptography
Imports System.Web.UI
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Utils
Imports Wma.Exceptions

Public Class ControladorUnidadesMedida

#Region "Enums"

    Public Enum TiposUnidad

        Comercial = 1
        Cove = 2

    End Enum

    Public Enum TipoSelectOption

        Idnombreoficialesp = 1
        Idnombreoficialing = 2
        CveMXnombreoficiales = 3
        CveMXnombreoficialing = 4

    End Enum

#End Region

#Region "Propiedades"

    <BsonId>
    Property _id As ObjectId
    Property tipounidad As TiposUnidad

    <BsonIgnoreIfNull>
    Property listaunidades As List(Of UnidadMedida)

#End Region

#Region "Constructores"

    Sub New()

    End Sub

#End Region

#Region "Funciones"

    Public Function NuevaUnidadCove(ByVal unidadCove_ As UnidadMedida) As TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")

        Dim tagwatcher_ As New TagWatcher

        Dim result_ = operationsDB_.InsertOneAsync(unidadCove_).ConfigureAwait(False)

        With tagwatcher_

            .SetOK()

            .ObjectReturned = unidadCove_

        End With

        Return tagwatcher_

    End Function

    Public Function EditarUnidadCove(ByVal unidadCove_ As UnidadMedida) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If unidadCove_ IsNot Nothing Then

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")

            Dim filter_ = Builders(Of UnidadMedida).Filter.Eq(Function(x) x._id, unidadCove_._id)

            Dim setStructureOfSubs_ = Builders(Of UnidadMedida).Update.
                                 Set(Function(x) x.cvecomercioMX, unidadCove_.cvecomercioMX).
                                 Set(Function(x) x.simbolooficial, unidadCove_.simbolooficial).
                                 Set(Function(x) x.nombreoficialesp, unidadCove_.nombreoficialesp).
                                 Set(Function(x) x.nombreoficialing, unidadCove_.nombreoficialing).
                                 Set(Function(x) x.archivado, unidadCove_.archivado).
                                 Set(Function(x) x.estado, unidadCove_.estado)

            Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

            With tagwatcher_

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetError(Me, "No se generaron cambios")

                End If

            End With

            Return tagwatcher_

        Else

            tagwatcher_.SetError(Me, "No existe una instancia de la empresa")

        End If

        Return tagwatcher_

    End Function

    Public Shared Function BuscarUnidades(ByVal tipoUnidad_ As TiposUnidad, ByVal token_ As String, Optional ByVal top_ As Int32 = 10) As List(Of UnidadMedida)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim unidades_ As New List(Of UnidadMedida)

        Select Case tipoUnidad_

            Case TiposUnidad.Cove

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")

                Dim filter_ As FilterDefinition(Of UnidadMedida) = Nothing

                If token_ IsNot Nothing And token_ <> "" Then

                    Dim options_ = New AggregateOptions With {
                        .AllowDiskUse = False
                    }

                    token_ = "\""" & token_ & """\"

                    Dim pipeline_ As PipelineDefinition(Of UnidadMedida, UnidadMedida) = New BsonDocument() {
                        New BsonDocument("$match", New BsonDocument().Add("$text", New BsonDocument().Add("$search", token_))),
                        New BsonDocument("$project", New BsonDocument From {
                                                    {"_id", 1},
                                                    {"_idunidadmedida", 1},
                                                    {"cvecomercioMX", 1},
                                                    {"nombreoficialesp", 1},
                                                    {"nombreoficialing", 1},
                                                    {"archivado", 1},
                                                    {"estado", 1},
                                                    {"score", New BsonDocument("$meta", "textScore")}
                                                }),
                        New BsonDocument("$sort", New BsonDocument("score", -1))
                    }

                    Using cursorUnidad_ = operationsDB_.AggregateAsync(Of UnidadMedida)(pipeline_, options_).Result

                        While cursorUnidad_.MoveNext

                            unidades_ = cursorUnidad_.Current.ToList()

                        End While

                    End Using

                Else

                    filter_ = Builders(Of UnidadMedida).Filter.Eq(Function(x) x.estado, 1)

                End If

                If Not unidades_.Count > 0 And filter_ IsNot Nothing Then

                    If top_ > 0 Then

                        unidades_ = operationsDB_.Find(filter_).Limit(top_).ToList()

                    ElseIf unidades_ Is Nothing Then

                        unidades_ = operationsDB_.Find(filter_).ToList()

                    End If

                End If

            Case TiposUnidad.Comercial

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadComercial)("[SynapsisN].[dbo].[Vt022UnidadesMedidaA07]")

                Dim filter_ As FilterDefinition(Of UnidadComercial) = Nothing

                If token_ IsNot Nothing And token_ <> "" Then

                    Dim options_ = New AggregateOptions With {
                        .AllowDiskUse = False
                    }

                    Dim pipeline_ As PipelineDefinition(Of UnidadComercial, UnidadComercial) = New BsonDocument() {
                        New BsonDocument(
                                        "$match", New BsonDocument().Add("$or", New BsonArray From {
                                            New BsonDocument From {
                                                {"t_DescripcionUnidadMedida", New BsonDocument From {
                                                    {"$regex", token_},
                                                    {"$options", "$i"}
                                                    }
                                                },
                                                {"i_Cve_Estado", 1}
                                            }
                                        }
                                    )
                                ),
                        New BsonDocument("$sort", New BsonDocument("i_Cve_UnidadMedida", 1))
                        }

                    Using cursorUnidad_ = operationsDB_.AggregateAsync(Of UnidadComercial)(pipeline_, Nothing).Result

                        While cursorUnidad_.MoveNext

                            unidades_ = ToUnidadMoneda(cursorUnidad_.Current.ToList())

                        End While

                    End Using

                Else

                    filter_ = Builders(Of UnidadComercial).Filter.Eq(Function(x) x.i_Cve_Estado, 1)

                End If


                If Not unidades_.Count > 0 And filter_ IsNot Nothing Then

                    Dim unidadesComercial_ As New List(Of UnidadComercial)

                    If top_ > 0 Then

                        unidadesComercial_ = operationsDB_.Find(filter_).Limit(top_).ToList()

                    Else

                        unidadesComercial_ = operationsDB_.Find(filter_).ToList()

                    End If

                    unidades_ = ToUnidadMoneda(unidadesComercial_)

                End If

        End Select

        If unidades_ IsNot Nothing Then

            Return unidades_

        Else

            Return Nothing

        End If

    End Function

    Public Shared Function BuscarUnidad(ByVal tipoUnidad_ As TiposUnidad, Optional ByVal listaObjectId_ As List(Of ObjectId) = Nothing, Optional ByVal objectId_ As ObjectId = Nothing) As List(Of UnidadMedida)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim unidades_ As New List(Of UnidadMedida)

        Select Case tipoUnidad_

            Case TiposUnidad.Cove

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")

                Dim filter_ As FilterDefinition(Of UnidadMedida) = Nothing

                If listaObjectId_ IsNot Nothing Then

                    filter_ = Builders(Of UnidadMedida).Filter.In(Function(x) x._id, listaObjectId_) And Builders(Of UnidadMedida).Filter.Eq(Function(x) x.estado, 1)

                Else

                    filter_ = Builders(Of UnidadMedida).Filter.Eq(Function(x) x._id, objectId_) And Builders(Of UnidadMedida).Filter.Eq(Function(x) x.estado, 1)

                End If

                unidades_ = operationsDB_.Find(filter_).ToList()

            Case TiposUnidad.Comercial

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadComercial)("[SynapsisN].[dbo].[Vt022UnidadesMedidaA07]")

                Dim filter_ As FilterDefinition(Of UnidadComercial) = Nothing

                If listaObjectId_ IsNot Nothing Then

                    filter_ = Builders(Of UnidadComercial).Filter.In(Function(x) x._id, listaObjectId_) And Builders(Of UnidadComercial).Filter.Eq(Function(x) x.i_Cve_Estado, 1)

                Else

                    filter_ = Builders(Of UnidadComercial).Filter.Eq(Function(x) x._id, objectId_) And Builders(Of UnidadComercial).Filter.Eq(Function(x) x.i_Cve_Estado, 1)

                End If

                Dim unidadesComercial_ As New List(Of UnidadComercial)

                unidadesComercial_ = operationsDB_.Find(filter_).ToList()

                unidades_ = ToUnidadMoneda(unidadesComercial_)

        End Select

        If unidades_ IsNot Nothing Then

            Return unidades_

        Else

            Return Nothing

        End If

    End Function

    Public Shared Function ListarUnidades(ByVal tipoUnidad_ As TiposUnidad) As List(Of UnidadMedida)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim unidades_ As New List(Of UnidadMedida)

        Select Case tipoUnidad_

            Case TiposUnidad.Cove

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")

                Dim filter_ As FilterDefinition(Of UnidadMedida) = Nothing

                filter_ = Builders(Of UnidadMedida).Filter.Eq(Function(x) x.estado, 1)

                Dim sort_ = Builders(Of UnidadMedida).Sort.Ascending(Function(x) x.nombreoficialesp)

                unidades_ = operationsDB_.Find(filter_).ToList()

            Case TiposUnidad.Comercial

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadComercial)("[SynapsisN].[dbo].[Vt022UnidadesMedidaA07]")

                Dim filter_ As FilterDefinition(Of UnidadComercial) = Nothing

                filter_ = Builders(Of UnidadComercial).Filter.Eq(Function(x) x.i_Cve_Estado, 1)

                Dim sort_ = Builders(Of UnidadComercial).Sort.Ascending(Function(x) x.i_Cve_UnidadMedida)

                Dim unidadesComercial_ As New List(Of UnidadComercial)

                unidadesComercial_ = operationsDB_.Find(filter_).Sort(sort_).ToList()

                unidades_ = ToUnidadMoneda(unidadesComercial_)

        End Select

        If unidades_ IsNot Nothing Then

            Return unidades_

        Else

            Return Nothing

        End If

    End Function

    Private Shared Function ToUnidadMoneda(ByVal unidadesComercial_ As List(Of UnidadComercial)) As List(Of UnidadMedida)

        Dim unidades_ As New List(Of UnidadMedida)

        For Each unidad_ In unidadesComercial_

            unidades_.Add(New UnidadMedida With {._id = unidad_._id,
                                                 ._idunidadmedida = unidad_.i_ClaveUnidadMedida,
                                                 .cvecomercioMX = unidad_.i_Cve_UnidadMedida,
                                                 .nombreoficialesp = unidad_.t_DescripcionUnidadMedida,
                                                 .archivado = unidad_.i_Cve_Estatus,
                                                 .estado = unidad_.i_Cve_Estado
                                                })

        Next

        Return unidades_

    End Function

    Public Shared Function ToSelectOption(ByVal listaUnidades_ As List(Of UnidadMedida), Optional ByVal tipoSelect_ As TipoSelectOption = TipoSelectOption.Idnombreoficialesp) As List(Of SelectOption)

        Dim listaUnidadesComponente_ As New List(Of SelectOption)

        If listaUnidades_ IsNot Nothing Then

            For Each unidad_ As UnidadMedida In listaUnidades_

                Select Case tipoSelect_

                    Case TipoSelectOption.Idnombreoficialesp

                        listaUnidadesComponente_.Add(New SelectOption With {.Value = unidad_._id.ToString, .Text = unidad_.cvecomercioMX & " - " & unidad_.nombreoficialesp})

                    Case TipoSelectOption.Idnombreoficialing

                        listaUnidadesComponente_.Add(New SelectOption With {.Value = unidad_._id.ToString, .Text = unidad_.cvecomercioMX & " - " & unidad_.nombreoficialing})

                    Case TipoSelectOption.CveMXnombreoficiales

                        listaUnidadesComponente_.Add(New SelectOption With {.Value = unidad_.cvecomercioMX, .Text = unidad_.cvecomercioMX & " - " & unidad_.nombreoficialesp})

                    Case TipoSelectOption.CveMXnombreoficialing

                        listaUnidadesComponente_.Add(New SelectOption With {.Value = unidad_.cvecomercioMX, .Text = unidad_.cvecomercioMX & " - " & unidad_.nombreoficialing})

                End Select

            Next

            Return listaUnidadesComponente_

        End If

        Return listaUnidadesComponente_

    End Function

    Public Function ListarUnidadesCOVE(ilimit_ As Int32) As List(Of UnidadMedida)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")

        Dim unidades_ As New List(Of UnidadMedida)

        operationsDB_.Aggregate().Project(BsonDocument.Parse("{" & "_id:1,cvecomercioMX:1,nombreoficialesp:1,presentacion:{$concat:['$nombreoficialesp',' | ','$cvecomercioMX']},demanda:{$switch:{branches:[{case:{$eq:['$cvecomercioMX','C62_1']}, then:5},{case:{$eq:['$cvecomercioMX','KGM']},then:4}], default:0}}}")).
                              Match(BsonDocument.Parse("{" & "$or:[{'cvecomercioMX':'C62_1'},{'cvecomercioMX':'KGM'},{'cvecomercioMX': 'EA'},{'cvecomercioMX': 'CS'},{'cvecomercioMX': 'SET'},{'cvecomercioMX': 'C62_2'},{'cvecomercioMX': 'KT'},{'cvecomercioMX': 'TNE'},{'cvecomercioMX': 'LM'},{'cvecomercioMX': 'MIL'},{'cvecomercioMX': 'MQ'},{'cvecomercioMX': 'MTK'},{'cvecomercioMX': 'BX'},{'cvecomercioMX': 'LTR'},{'cvecomercioMX': 'GRM'}]}")).
                              Sort(BsonDocument.Parse("{" & "demanda:-1}")).Limit(ilimit_).
        ToList().ForEach(Sub(estatus_)

                             If Not unidades_.Exists(Function(y) y.cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value) Then

                                 unidades_.Add(New UnidadMedida With {
                                                       ._id = New ObjectId(estatus_.GetElement("_id").Value.ToString),
                                                       .cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value.ToString,
                                                       .nombreoficialesp = estatus_.GetElement("presentacion").Value.ToString
                                                    })

                             End If

                         End Sub)

        Return unidades_
    End Function

    Public Function BuscarUnidadesCOVE(token_ As String, Optional stipo_ As String = "", Optional ByVal limit_ As Int32 = 10) As List(Of UnidadMedida)
        Dim unidades_ As New List(Of UnidadMedida)
        Dim CtrlRecursosGenerales_ As New Organismo
        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos


            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")


            If stipo_ = "itext" Then
                operationsDB_.Aggregate().Match(BsonDocument.Parse(CtrlRecursosGenerales_.SeparacionPalabras(token_, "", "", "", stipo_))).
                                      Project(BsonDocument.Parse("{" & "_id:1,cvecomercioMX:1,nombreoficialesp:1,nombreoficialing:1,presentacion:{$concat:['$nombreoficialesp',' | ','$cvecomercioMX']},demanda:{$switch:{branches:[{case:{$eq:['$cvecomercioMX','C62_1']}, then:5},{case:{$eq:['$cvecomercioMX','KGM']},then:4}], default:0}}}")).
                                  Sort(BsonDocument.Parse("{" & "demanda:-1}")).Limit(limit_).
            ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)

                                                       If Not unidades_.Exists(Function(y) y.cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value) Then

                                                           unidades_.Add(New UnidadMedida With {
                                                           ._id = New ObjectId(estatus_.GetElement("_id").Value.ToString),
                                                           .cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value.ToString,
                                                           .nombreoficialesp = estatus_.GetElement("presentacion").Value.ToString
                                                        })

                                                       End If

                                                   End Sub)
            Else
                operationsDB_.Aggregate().Project(BsonDocument.Parse("{" & "_id:1,cvecomercioMX:1,nombreoficialesp:1,nombreoficialing:1,presentacion:{$concat:['$nombreoficialesp',' | ','$cvecomercioMX']},demanda:{$switch:{branches:[{case:{$eq:['$cvecomercioMX','C62_1']}, then:5},{case:{$eq:['$cvecomercioMX','KGM']},then:4}], default:0}}}")).
                                  Match(BsonDocument.Parse("{" & "$or:[{'cvecomercioMX':{$regex:" & "'" & token_ & "',$options:'i'}}," & CtrlRecursosGenerales_.SeparacionPalabras(token_, "nombreoficialesp", "", "", "") & "," & CtrlRecursosGenerales_.SeparacionPalabras(token_, "nombreoficialing", "", "", "") & "]}")).
                                  Sort(BsonDocument.Parse("{" & "demanda:-1}")).Limit(limit_).
            ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)

                                                       If Not unidades_.Exists(Function(y) y.cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value) Then

                                                           unidades_.Add(New UnidadMedida With {
                                                           ._id = New ObjectId(estatus_.GetElement("_id").Value.ToString),
                                                           .cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value.ToString,
                                                           .nombreoficialesp = estatus_.GetElement("presentacion").Value.ToString
                                                        })

                                                       End If

                                                   End Sub)


            End If
        End Using

        Return unidades_
    End Function

    Public Function BuscarUnidadesCOVE(tokens_ As List(Of String), Optional stipo_ As String = "") As List(Of UnidadMedida)
        Dim unidades_ As New List(Of UnidadMedida)
        Dim CtrlRecursosGenerales_ As New Organismo
        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos


            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of UnidadMedida)("Reg007UnidadMedidaCove")


            If stipo_ = "itext" And tokens_.Count = 1 Then

                operationsDB_.Aggregate().Match(BsonDocument.Parse(CtrlRecursosGenerales_.SeparacionPalabras(tokens_(0), "", "", "", stipo_))).
                                      Project(BsonDocument.Parse("{" & "_id:1,cvecomercioMX:1,nombreoficialesp:1,nombreoficialing:1,presentacion:{$concat:['$nombreoficialesp',' | ','$cvecomercioMX']},demanda:{$switch:{branches:[{case:{$eq:['$cvecomercioMX','C62_1']}, then:5},{case:{$eq:['$cvecomercioMX','KGM']},then:4}], default:0}}}")).
                                  Sort(BsonDocument.Parse("{" & "demanda:-1}")).
            ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)

                                                       If Not unidades_.Exists(Function(y) y.cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value) Then

                                                           unidades_.Add(New UnidadMedida With {
                                                           ._id = New ObjectId(estatus_.GetElement("_id").Value.ToString),
                                                           .cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value.ToString,
                                                           .nombreoficialesp = estatus_.GetElement("presentacion").Value.ToString
                                                        })

                                                       End If

                                                   End Sub)
            Else
                Dim condicionmatch_ As String = "{$or:["
                For Each token_ In tokens_
                    condicionmatch_ = condicionmatch_ & "{" & "$or:[{'cvecomercioMX':{$regex:" & "'" & token_ & "',$options:'i'}}," & CtrlRecursosGenerales_.SeparacionPalabras(token_, "nombreoficialesp", "", "", "") & "," & CtrlRecursosGenerales_.SeparacionPalabras(token_, "nombreoficialing", "", "", "") & "]},"
                Next
                condicionmatch_ = condicionmatch_.Substring(0, condicionmatch_.Length - 1) & "]}"
                operationsDB_.Aggregate().Project(BsonDocument.Parse("{" & "_id:1,cvecomercioMX:1,nombreoficialesp:1,nombreoficialing:1,presentacion:{$concat:['$nombreoficialesp',' | ','$cvecomercioMX']},demanda:{$switch:{branches:[{case:{$eq:['$cvecomercioMX','C62_1']}, then:5},{case:{$eq:['$cvecomercioMX','KGM']},then:4}], default:0}}}")).
                                  Match(BsonDocument.Parse(condicionmatch_)).
                                  Sort(BsonDocument.Parse("{" & "demanda:-1}")).
            ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)

                                                       If Not unidades_.Exists(Function(y) y.cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value) Then

                                                           unidades_.Add(New UnidadMedida With {
                                                           ._id = New ObjectId(estatus_.GetElement("_id").Value.ToString),
                                                           .cvecomercioMX = estatus_.GetElement("cvecomercioMX").Value.ToString,
                                                           .nombreoficialesp = estatus_.GetElement("presentacion").Value.ToString
                                                        })

                                                       End If

                                                   End Sub)


            End If
        End Using

        Return unidades_
    End Function

    Public Function substraeUnidadMedida(sUnidadMedida_ As String) As String
        If sUnidadMedida_ = "" Then
            sUnidadMedida_ = "PIEZA"
        End If
        Dim indice_ As Int32 = sUnidadMedida_.IndexOf("-") + 2
        If indice_ >= 0 Then
            sUnidadMedida_ = sUnidadMedida_.Substring(sUnidadMedida_.IndexOf("-") + 2)
        End If
        Return sUnidadMedida_
    End Function

#End Region

End Class

Public Class UnidadMedida
    Property _id As ObjectId
    Property _idunidadmedida As Int32?
    Property cvecomercioMX As String
    <BsonIgnoreIfNull>
    Property simbolooficial As String
    Property nombreoficialesp As String
    <BsonIgnoreIfNull>
    Property nombreoficialing As String
    Property estado As Int16 = 1
    Property archivado As Boolean? = False
    Property score As Double

End Class

Public Class UnidadComercial
    Property _id As ObjectId
    Property f_FechaRegistro As DateTime
    Property i_ClaveUnidadMedida As Int32?
    Property i_Cve_UnidadMedida As Int32?
    <BsonIgnoreIfNull>
    Property t_DescripcionUnidadMedida As String
    Property i_Cve_Estado As Int16 = 1
    Property i_Cve_Estatus As Boolean? = False

End Class