Imports System.Text.RegularExpressions
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports Wma.Exceptions
Imports Rec.Globals.Controllers.IControladorEmpresas.TipoEstructura

Imports Rec.Globals.PaisDomicilio64
Imports Rec.Globals.TaxId64
Imports Rec.Globals.Contacto64
Imports Rec.Globals.Rfc64
Imports Rec.Globals.RegimenFiscal64
Imports Rec.Globals.IEmpresaNacional64
Imports Rec.Globals.EmpresaNacional64
Imports Rec.Globals.IEmpresaInternacional64
Imports Rec.Globals.EmpresaInternacional64
Imports Rec.Globals.Domicilio64
Imports Rec.Globals.Curp64
Imports Rec.Globals.Bus64
Imports Rec.Globals.Empresa64
Imports Rec.Globals

Imports Microsoft.VisualBasic.FileIO
Imports System.Runtime.InteropServices
Imports System.CodeDom


Public Class ControladorEmpresas
    Implements IControladorEmpresas, IDisposable

#Region "PROPIEDADES PRIVADAS"

    Private _listaPaisesDomicilios _
        As List(Of PaisDomicilio)

    Private _espacioTrabajo _
        As IEspacioTrabajo

    Private disposedValue As Boolean

    Private _paisPresentacion = "MEX - MÉXICO (ESTADOS UNIDOS MEXICANOS)"

#End Region

#Region "PROPIEDADES PUBLICAS"
    Public Property TipoEmpresa _
        As IControladorEmpresas.TiposEmpresas _
        Implements IControladorEmpresas.TipoEmpresa

    Public Property ListaDomicilios As List(Of Domicilio) _
        Implements IControladorEmpresas.ListaDomicilios

    Public Property Estado _
        As TagWatcher _
        Implements IControladorEmpresas.Estado

    Public Property Modalidad _
        As IControladorEmpresas.Modalidades _
        Implements IControladorEmpresas.Modalidad

    Public Property PaisEmpresa _
        As String Implements IControladorEmpresas.PaisEmpresa

    Public Property ListaEmpresas As List(Of IEmpresa) _
        Implements IControladorEmpresas.ListaEmpresas

    Public Property Empresa As IEmpresa _
        Implements IControladorEmpresas.Empresa
#End Region

    Sub New(ByVal espacioTrabajo_ As IEspacioTrabajo)

        Inicializa(espacioTrabajo_)

    End Sub

    Sub New(ByVal espacioTrabajo_ As IEspacioTrabajo,
            ByVal tipoEmpresa_ As IControladorEmpresas.TiposEmpresas)

        Inicializa(espacioTrabajo_,
                   tipoEmpresa_)
    End Sub

    Sub New(ByVal espacioTrabajo_ As IEspacioTrabajo,
            ByVal tipoEmpresa_ _
            As IControladorEmpresas.TiposEmpresas,
            ByVal paisEmpresa_ As String,
            Optional ByVal modalidad_ _
            As IControladorEmpresas.Modalidades = IControladorEmpresas.Modalidades.Extrinseca)

        Inicializa(espacioTrabajo_,
                   tipoEmpresa_,
                   paisEmpresa_,
                   modalidad_)
    End Sub

    Private Sub Inicializa(ByVal espacioTrabajo_ As IEspacioTrabajo,
                          Optional ByVal tipoEmpresa_ _
                          As IControladorEmpresas.TiposEmpresas = IControladorEmpresas.TiposEmpresas.Nacional,
                          Optional ByVal paisEmpresa_ _
                          As String = "MEX",
                          Optional ByVal modalidad_ _
                          As IControladorEmpresas.Modalidades = IControladorEmpresas.Modalidades.Extrinseca)

        _espacioTrabajo = espacioTrabajo_

        TipoEmpresa = tipoEmpresa_

        Modalidad = modalidad_

        PaisEmpresa = paisEmpresa_

        ListaEmpresas = New List(Of IEmpresa)

        Estado = New TagWatcher

    End Sub


#Region "MÉTODOS PRIVADOS"

    Private Function ArchivarRegistro(ByVal tipo_ As IControladorEmpresas.TipoEstructura,
                                            ByVal filtro_ As String) _
                                            As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)(GetType(EmpresaInternacional).Name)

            Dim setStructureOfSubs_ As UpdateDefinition(Of EmpresaNacional)

            Dim result_ As UpdateResult

            Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.razonsocial, filtro_)

            Select Case tipo_

                Case IControladorEmpresas.TipoEstructura.RFC

                    setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.Set(Of Boolean)("rfcs.$[].archivado", True)

                    result_ = operationsDB_.UpdateMany(filter_, setStructureOfSubs_)

                Case IControladorEmpresas.TipoEstructura.CURP

                    setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.Set(Of Boolean)("curps.$[].archivado", True)

                    result_ = operationsDB_.UpdateMany(filter_, setStructureOfSubs_)

            End Select

            With Estado

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se generaron cambios")

                End If

            End With

        End Using

        Return Estado

    End Function

    Private Function AgregarEmpresaNacional(ByVal empresaNacional_ As EmpresaNacional,
                                            Optional ByVal objetoRetorno_ As Boolean = False,
                                            Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                            As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                                         With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

            If session_ IsNot Nothing Then

                Dim result_ = operationsDB_.InsertOneAsync(session_, empresaNacional_).ConfigureAwait(False)

            Else

                Dim result_ = operationsDB_.InsertOneAsync(empresaNacional_).ConfigureAwait(False)

            End If

            With Estado

                If objetoRetorno_ Then

                    Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.razonsocial,
                                                                         empresaNacional_.razonsocial)

                    Dim empresaResult_ = operationsDB_.Find(filter_).Limit(1).ToList()

                    If empresaResult_.Count > 0 Then

                        ListaEmpresas = New List(Of IEmpresa) From {empresaResult_(0)}

                        .ObjectReturned = ListaEmpresas

                        .SetOK()
                    Else

                        .SetOKBut(Me, "Empresa no encontrada")

                    End If

                Else

                    .SetOK()

                End If

            End With

        End Using

        Return Estado

    End Function

    Private Function AgregarEmpresaInternacional(ByVal empresaInternacional_ As EmpresaInternacional,
                                                 Optional ByVal objetoRetorno_ As Boolean = False,
                                                 Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                 As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

            If session_ IsNot Nothing Then

                Dim result_ = operationsDB_.InsertOneAsync(session_, empresaInternacional_).ConfigureAwait(False)

            Else

                Dim result_ = operationsDB_.InsertOneAsync(empresaInternacional_).ConfigureAwait(False)

            End If

            With Estado

                If objetoRetorno_ Then

                    Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.razonsocial,
                                                                              empresaInternacional_.razonsocial)

                    Dim empresaResult_ = operationsDB_.Find(filter_).Limit(1).ToList()

                    If empresaResult_.Count > 0 Then

                        ListaEmpresas = New List(Of IEmpresa) From {empresaResult_(0)}

                        .ObjectReturned = ListaEmpresas

                        .SetOK()
                    Else

                        .SetOKBut(Me, "Empresa no encontrada")

                    End If

                Else

                    .SetOK()

                End If

            End With

        End Using

        Return Estado

    End Function

    Private Function ModificarEmpresaNacional(ByRef empresaNacional_ As EmpresaNacional,
                                              Optional session_ As IClientSessionHandle = Nothing) _
                                              As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            With empresaNacional_

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                Dim filter_ = Builders(Of EmpresaNacional).Filter.And(
                                Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.razonsocial, .razonsocial),
                                Builders(Of EmpresaNacional).Filter.ElemMatch(Function(y) y.paisesdomicilios, Function(x) x.pais.Equals(PaisEmpresa)),
                                Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1))

                Dim lastDomicilio_ As New Domicilio

                lastDomicilio_ = DirectCast(empresaNacional_.paisesdomicilios.Last.domicilios.Last, Domicilio)

                If .rfcNuevo Then

                    ArchivarRegistro(IControladorEmpresas.TipoEstructura.RFC, .razonsocial)

                End If

                Dim estructuraCurp_ = Nothing

                If .curpNuevo IsNot Nothing Then

                    If .curpNuevo Then

                        ArchivarRegistro(IControladorEmpresas.TipoEstructura.CURP, .razonsocial)

                        estructuraCurp_ = .curps.Last

                    End If

                End If

                Dim estructuraContactos_ = New List(Of Contacto)

                If .contactos IsNot Nothing Then

                    If .contactos.Count <> 0 Then

                        estructuraContactos_.Add(.contactos.Last)
                    Else

                        estructuraContactos_ = Nothing

                    End If
                Else

                    estructuraContactos_ = Nothing

                End If

                Dim estructuraRegimenes_ = New List(Of RegimenFiscal)

                If .regimenesfiscales IsNot Nothing Then

                    If .regimenesfiscales.Count <> 0 Then

                        estructuraRegimenes_.Add(.regimenesfiscales.Last)

                    Else

                        estructuraRegimenes_ = Nothing

                    End If

                Else

                    estructuraRegimenes_ = Nothing

                End If

                Dim setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.
                                                                        Set(Function(x) x.razonsocial, .razonsocial).
                                                                        Set(Function(x) x._idempresa, ._idempresa).
                                                                        Set(Function(x) x._idempresakb, ._idempresakb).
                                                                        Set(Function(x) x.razonsocialcorto, .razonsocialcorto).
                                                                        Set(Function(x) x.abreviatura, .abreviatura).
                                                                        Set(Function(x) x.nombrecomercial, .nombrecomercial).
                                                                        Set(Function(x) x.girocomercial, .girocomercial).
                                                                        Set(Function(x) x._idgrupocomercial, ._idgrupocomercial).
                                                                        Set(Function(x) x.tipopersona, .tipopersona).
                                                                        Set(Function(x) x._idrfc, ._idrfc).
                                                                        Set(Function(x) x.rfc, .rfc).
                                                                        AddToSet("rfcs", .rfcs.Last).
                                                                        Set(Function(x) x._idcurp, ._idcurp).
                                                                        Set(Function(x) x.curp, .curp).
                                                                        AddToSet("curps", estructuraCurp_).
                                                                        AddToSet("contactos", estructuraContactos_).
                                                                        AddToSet("regimenesfiscales", estructuraRegimenes_).
                                                                        AddToSet("paisesdomicilios.$.domicilios", lastDomicilio_)

                Dim result_ As UpdateResult

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result
                Else

                    result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result

                End If

                With Estado

                    If result_.ModifiedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    ElseIf result_.MatchedCount = 1 Then

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Registro no modificado")

                    End If

                End With

            End With

        End Using

        Return Estado

    End Function

    Private Function ModificarEmpresaInternacional(ByRef empresaInternacional_ As EmpresaInternacional,
                                                   Optional session_ As IClientSessionHandle = Nothing) _
                                                   As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            With empresaInternacional_

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.razonsocial, .razonsocial) And
                              Builders(Of EmpresaInternacional).Filter.ElemMatch(Function(y) y.paisesdomicilios, Function(x) x.pais.Equals(PaisEmpresa)) And
                              Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

                Dim lastDomicilio_ As New Domicilio

                lastDomicilio_ = DirectCast(empresaInternacional_.paisesdomicilios.Last.domicilios.Last, Domicilio)

                Dim estructuraContactos_ = New List(Of Contacto)

                If .contactos IsNot Nothing Then

                    If .contactos.Count <> 0 Then

                        estructuraContactos_.Add(.contactos.Last)
                    Else

                        estructuraContactos_ = Nothing

                    End If
                Else

                    estructuraContactos_ = Nothing

                End If

                Dim estructuraBus_ = New List(Of Bus)

                If .bus IsNot Nothing Then

                    If .bus.Count <> 0 Then

                        estructuraBus_.Add(.bus.Last)
                    Else

                        estructuraBus_ = Nothing

                    End If
                Else

                    estructuraBus_ = Nothing

                End If

                Dim setStructureOfSubs_ = Builders(Of EmpresaInternacional).Update.
                                                                        Set(Function(x) x.razonsocial, .razonsocial).
                                                                        Set(Function(x) x._idempresa, ._idempresa).
                                                                        Set(Function(x) x._idempresakb, ._idempresakb).
                                                                        Set(Function(x) x.razonsocialcorto, .razonsocialcorto).
                                                                        Set(Function(x) x.abreviatura, .abreviatura).
                                                                        Set(Function(x) x.nombrecomercial, .nombrecomercial).
                                                                        Set(Function(x) x.girocomercial, .girocomercial).
                                                                        Set(Function(x) x._idgrupocomercial, ._idgrupocomercial).
                                                                        Set(Function(x) x._idbu, ._idbu).
                                                                        Set(Function(x) x.bu, .bu).
                                                                        AddToSet("taxids", .taxids.Last).
                                                                        AddToSet("paisesdomicilios.$.domicilios", lastDomicilio_).
                                                                        AddToSet("contactos", estructuraContactos_).
                                                                        AddToSet("bus", estructuraBus_)
                Dim result_ As UpdateResult

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_,
                                                           New UpdateOptions With {.IsUpsert = True}).Result

                Else

                    Try

                        result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_,
                                                               New UpdateOptions With {.IsUpsert = True}).Result

                    Catch ex As Exception

                        Dim aaa_ = ex

                    End Try

                End If

                With Estado

                    If result_.ModifiedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    ElseIf result_.MatchedCount = 1 Then

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Registro no modificado")

                    End If

                End With

            End With

        End Using

        Return Estado

    End Function

    Private Function ConsultarEmpresasNacionales(ByRef razonSocial_ As String,
                                                 ByRef limiteResultados_ As Int32) As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsdb_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                Dim filter_ = Builders(Of EmpresaNacional).Filter.And(
                                Builders(Of EmpresaNacional).Filter.Text(razonSocial_),
                                Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1))

                Dim result_ = operationsdb_.Find(filter_).Limit(limiteResultados_).ToList()

                If result_.Count <> 0 Then

                    .ObjectReturned = result_

                    .SetOK()

                Else
                    .SetOKBut(Me, "No se encontraron resultados")

                End If

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarEmpresasInternacionales(ByRef razonSocial_ As String,
                                                      ByRef limiteResultados_ As Int32) As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsdb_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.And(
                                Builders(Of EmpresaInternacional).Filter.Text(razonSocial_),
                                Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1))

                Dim result_ = operationsdb_.Find(filter_).Limit(limiteResultados_).ToList()

                If result_.Count <> 0 Then

                    .ObjectReturned = result_

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se encontraron resultados")

                End If

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarEmpresaNacional(ByVal cveEmpresa_ As ObjectId) _
                                              As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x._id, cveEmpresa_) _
                              And Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                If result_.Count > 0 Then

                    .ObjectReturned = result_(0)

                    .SetOK()

                Else

                    .SetOKBut(Me, "Empresa no encontrada")

                End If

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarEmpresaInternacional(ByVal cveEmpresa_ As ObjectId) _
                                                   As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x._id, cveEmpresa_) _
                              And Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                If result_.Count > 0 Then

                    .ObjectReturned = result_(0)

                    .SetOK()

                Else

                    .SetOKBut(Me, "Empresa no encontrada")

                End If

            End Using


        End With

        Return Estado

    End Function

    Private Function ConsultarEmpresaInterna(ByVal cveEmpresa_ As ObjectId) _
        As TagWatcher

        With Estado

            If ListaEmpresas IsNot Nothing Then

                If ListaEmpresas.Count > 0 Then

                    .ObjectReturned = ListaEmpresas.Where(Function(t) t._id = cveEmpresa_)

                Else

                    .SetOKBut(Me, "Lista de empresas vacía")

                End If

            Else

                .SetOKBut(Me, "Lista de empresas vacía")

            End If

        End With

        Return Estado

    End Function

    Private Function ConsultarDomiciliosNacionales(ByVal cveEmpresa_ As ObjectId) As TagWatcher

        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x._id, cveEmpresa_) _
                              And Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                result_.AsEnumerable.ToList().ForEach(Sub(empresa_)

                                                          Dim domiciliosExt_ = empresa_.paisesdomicilios.
                                                                                        Where(Function(x) x.pais = PaisEmpresa).
                                                                                        Select(Function(y) y.domicilios)

                                                          For Each item_ In domiciliosExt_

                                                              ListaDomicilios = item_

                                                          Next

                                                      End Sub)

                If ListaDomicilios.Count > 0 Then

                    .ObjectReturned = ListaDomicilios

                    .SetOK()

                Else

                    .SetOKBut(Me, "Empresa no contiene domicilios")

                End If

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarDomiciliosInternacionales(ByVal cveEmpresa_ As ObjectId) As TagWatcher

        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x._id, cveEmpresa_) And
                              Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                result_.AsEnumerable.ToList().ForEach(Sub(empresa_)

                                                          Dim domiciliosExt_ = empresa_.paisesdomicilios.
                                                                                        Where(Function(x) x.pais = PaisEmpresa).
                                                                                        Select(Function(y) y.domicilios)

                                                          For Each item_ In domiciliosExt_

                                                              ListaDomicilios = item_

                                                          Next

                                                      End Sub)

                If ListaDomicilios.Count > 0 Then

                    .ObjectReturned = ListaDomicilios

                    .SetOK()

                Else

                    .SetOKBut(Me, "Empresa no contiene domicilios")

                End If

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarDomiciliosInternos(ByVal cveEmpresa_ As ObjectId,
                                                 ByVal pais_ As String) _
                                                 As TagWatcher

        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Dim domicilios_ = ListaEmpresas.
                    Where(Function(t) t._id = cveEmpresa_).
                    Select(Function(z) z.paisesdomicilios.
                    Where(Function(a) a.pais = pais_).
                    Select(Function(b) b.domicilios)).AsEnumerable

            For Each item_ In domicilios_

                For Each i_ In item_

                    ListaDomicilios = i_

                Next

            Next

            If ListaDomicilios.Count > 0 Then

                .ObjectReturned = ListaDomicilios

                .SetOK()

            Else

                .SetOKBut(Me, "Domicilios no disponibles")

            End If

        End With

        Return Estado

    End Function

    Private Function ConsultarDomicilioNacional(ByVal cveEmpresa_ As ObjectId,
                                                ByVal cveDomicilio_ As ObjectId) _
                                                As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x._id, cveEmpresa_) And
                              Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                result_.AsEnumerable.ToList().ForEach(Sub(empresa_)

                                                          Dim domiciliosExt_ = empresa_.paisesdomicilios.
                                                                                Where(Function(x) x.pais = PaisEmpresa).
                                                                                Select(Function(y) y.domicilios.
                                                                                Where(Function(z) z._iddomicilio = cveDomicilio_))

                                                          For Each item_ In domiciliosExt_

                                                              For Each i_ In item_

                                                                  ListaDomicilios.Add(i_)

                                                              Next

                                                          Next

                                                      End Sub)
                If ListaDomicilios.Count > 0 Then

                    .ObjectReturned = ListaDomicilios

                    .SetOK()

                Else

                    .SetOKBut(Me, "Empresa no contiene domicilios")

                End If

            End Using

        End With

        Return Estado

    End Function

    Private Function ConsultarDomicilioInternacional(ByVal cveEmpresa_ As ObjectId,
                                                     ByVal cveDomicilio_ As ObjectId) _
                                                     As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x._id, cveEmpresa_) And
                              Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                result_.AsEnumerable.ToList().ForEach(Sub(empresa_)

                                                          Dim domiciliosExt_ = empresa_.paisesdomicilios.
                                                                                Where(Function(x) x.pais = PaisEmpresa).
                                                                                Select(Function(y) y.domicilios.
                                                                                Where(Function(z) z._iddomicilio = cveDomicilio_))

                                                          For Each item_ In domiciliosExt_

                                                              For Each i_ In item_

                                                                  ListaDomicilios.Add(i_)

                                                              Next

                                                          Next

                                                      End Sub)

                If ListaDomicilios.Count > 0 Then

                    .ObjectReturned = ListaDomicilios

                    .SetOK()

                Else

                    .SetOKBut(Me, "Empresa no contiene domicilios")

                End If

            End Using


        End With

        Return Estado

    End Function

    Private Function ConsultarDomicilioInterno(ByVal cveEmpresa_ As ObjectId,
                                               ByVal cveDomicilio_ As ObjectId) _
                                               As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            If ListaEmpresas IsNot Nothing And
                        ListaEmpresas.Count <> 0 Then

                Dim domicilioInt_ = ListaEmpresas.
                                    Where(Function(t) t._id = cveEmpresa_).
                                    Select(Function(z) z.paisesdomicilios.
                                    Where(Function(a) a.pais = PaisEmpresa).
                                    Select(Function(b) b.domicilios.
                                    Where(Function(y) y._iddomicilio = cveDomicilio_)))


                For Each domicilio_ In domicilioInt_

                    For Each item_ In domicilio_

                        For Each i_ In item_

                            ListaDomicilios.Add(i_)

                        Next

                    Next

                Next

                If ListaDomicilios.Count > 0 Then

                    .ObjectReturned = ListaDomicilios

                    .SetOK()

                Else

                    .SetOKBut(Me, "Domicilios no disponibles")

                End If

            Else

                .SetOKBut(Me, "Lista de empresas vacía")

            End If

        End With

        Return Estado

    End Function

    Private Function ArchivarEmpresasNacionales(ByRef objectIdEmpresa_ _
                                                As List(Of ObjectId)) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")


            Dim filter_ = Builders(Of EmpresaNacional).Filter.In(Function(x) x._id, objectIdEmpresa_)

            Dim setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.Set(Of Boolean)("archivado", True)

            Dim result_ = operationsDB_.UpdateMany(filter_, setStructureOfSubs_)

            With Estado

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se generaron cambios")

                End If

            End With

        End Using

        Return Estado

    End Function

    Private Function ArchivarEmpresasInternacionales(ByRef listaObjectIdEmpresa_ As List(Of ObjectId)) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

            Dim filter_ = Builders(Of EmpresaInternacional).Filter.In(Function(x) x._id, listaObjectIdEmpresa_)

            Dim setStructureOfSubs_ = Builders(Of EmpresaInternacional).Update.
                                      Set(Of Boolean)("archivado", True)

            Dim result_ = operationsDB_.UpdateMany(filter_, setStructureOfSubs_)

            With Estado

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se generaron cambios")

                End If

            End With

        End Using

        Return Estado

    End Function

    'PENDIENTE DE CHECAR, SE SIGUEN ARCHIVANDO TODOS LOS DOMICILIOS, EN VEZ DE SOLO LOS DE LA LISTA QUE SE MANDEN
    Private Function ArchivarDomiciliosNacionales(ByVal objectIdEmpresa_ As ObjectId,
                                                  ByVal listaObjectIdDomicilio_ As List(Of ObjectId)) _
                                                  As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")


            Dim filter_ As FilterDefinition(Of EmpresaNacional) = Builders(Of EmpresaNacional).Filter.And(
            Builders(Of EmpresaNacional).Filter.Eq(Function(e) e._id, objectIdEmpresa_),
            Builders(Of EmpresaNacional).Filter.ElemMatch(Function(e) e.paisesdomicilios,
                                                          Function(p) p.pais.Equals(PaisEmpresa) And
                                                                      p.domicilios.Any(Function(d) listaObjectIdDomicilio_.Contains(d._iddomicilio))))

            'Dim filter_ As FilterDefinition(Of EmpresaNacional) = Builders(Of EmpresaNacional).Filter.And(Builders(Of EmpresaNacional).Filter.Eq(Function(e) e._id, objectIdEmpresa_),
            '    Builders(Of EmpresaNacional).Filter.ElemMatch(Function(x) x.paisesdomicilios, Function(y) y.domicilios.In(listaObjectIdDomicilio_, Function(z) z._iddomicilios)))




            Dim setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.
                                      Set(Of Boolean)("paisesdomicilios.domicilios.$[].archivado", True)

            Dim result_ = operationsDB_.UpdateOne(filter_, setStructureOfSubs_)

            'Dim filter_ As FilterDefinition(Of EmpresaNacional) = Builders(Of EmpresaNacional).Filter.And(
            '                                                      Builders(Of EmpresaNacional).Filter.Eq(Function(e) e._id, objectIdEmpresa_),
            '                                                      Builders(Of EmpresaNacional).Filter.ElemMatch(
            '                                                            Function(e) e.paisesdomicilios,
            '                                                            Function(p) p.pais.Equals(PaisEmpresa) And
            '                                                                        p.domicilios.Any(Function(d) listaObjectIdDomicilio_.Contains(d._iddomicilio))
            '                                                        )
            '                                                    )

            'Dim setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.Set(Of Boolean)("paisesdomicilios.$[outer].domicilios.$[inner].archivado", True)

            'Dim arrayFilters As New List(Of ArrayFilterDefinition)()
            'arrayFilters.Add(Builders(Of EmpresaNacional).Filter.Eq("outner.pais", PaisEmpresa)),
            'arrayFilters.Add(Builders(Of EmpresaNacional).Filter.Eq("inner._iddomicilio", listaObjectIdDomicilio_(0))) ' Cambia el índice según tus necesidades

            'Dim updateOptions As New UpdateOptions()
            'updateOptions.ArrayFilters = arrayFilters

            'Dim result_ = operationsDB_.UpdateMany(filter_, setStructureOfSubs_, updateOptions)

            With Estado

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetError(Me, "No se generaron cambios")

                End If

            End With

        End Using

        Return Estado

    End Function

#End Region

#Region "Métodos públicos"
    Public Function Agregar(empresa_ As IEmpresaNacional,
                            Optional objetoRetorno_ As Boolean = False,
                            Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                            Implements IControladorEmpresas.Agregar
        With Estado

            If empresa_ IsNot Nothing Then

                AgregarEmpresaNacional(empresa_, objetoRetorno_, session_)

            Else

                .SetError(Me, "Empresa es requerida")

            End If

        End With

        Return Estado

    End Function

    Public Function Agregar(empresa_ As IEmpresaInternacional,
                            Optional objetoRetorno_ As Boolean = False,
                            Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                            Implements IControladorEmpresas.Agregar
        With Estado

            If empresa_ IsNot Nothing Then

                AgregarEmpresaInternacional(empresa_, objetoRetorno_, session_)

            Else

                .SetError(Me, "Empresa es requerida")

            End If

        End With

        Return Estado

    End Function

    Public Function Modificar(empresa_ As IEmpresaNacional,
                              Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                              Implements IControladorEmpresas.Modificar
        With Estado

            If empresa_ IsNot Nothing Then

                ModificarEmpresaNacional(empresa_, session_)

            Else

                .SetError(Me, "No existe instancia de empresa nacional")

            End If

        End With

        Return Estado

    End Function

    Public Function Modificar(empresa_ As IEmpresaInternacional,
                              Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                              Implements IControladorEmpresas.Modificar
        With Estado

            If empresa_ IsNot Nothing Then

                ModificarEmpresaInternacional(empresa_, session_)

            Else

                .SetError(Me, "No existe instancia de empresa internacional")

            End If

        End With

        Return Estado

    End Function

    Public Function Consultar(razonSocial_ As String,
                              Optional ByVal limiteResultados_ As Int32 = 10) As TagWatcher _
                              Implements IControladorEmpresas.Consultar
        With Estado

            If razonSocial_ IsNot Nothing Then

                If limiteResultados_ <= 200 Then

                    Select Case TipoEmpresa

                        Case IControladorEmpresas.TiposEmpresas.Nacional

                            ConsultarEmpresasNacionales(razonSocial_, limiteResultados_)

                        Case IControladorEmpresas.TiposEmpresas.Internacional

                            ConsultarEmpresasInternacionales(razonSocial_, limiteResultados_)

                    End Select

                Else

                    .SetError(Me, "Limitador de resultados supera los 200 permitidos")

                End If

            Else

                .SetError(Me, "No se recibió razón social")

            End If

        End With

        Return Estado

    End Function

    Public Function ConsultarUna(cveEmpresa_ As ObjectId) As TagWatcher _
                                 Implements IControladorEmpresas.ConsultarUna
        With Estado

            If Not cveEmpresa_ = ObjectId.Empty Then

                Select Case Modalidad

                    Case IControladorEmpresas.Modalidades.Extrinseca

                        If TipoEmpresa = IControladorEmpresas.TiposEmpresas.Nacional Then

                            ConsultarEmpresaNacional(cveEmpresa_)

                        Else

                            ConsultarEmpresaInternacional(cveEmpresa_)

                        End If

                    Case IControladorEmpresas.Modalidades.Intrinseca

                        ConsultarEmpresaInterna(cveEmpresa_)

                End Select

            Else

                .SetError(Me, "No se recibió una clave de empresa")

            End If

        End With

        Return Estado

    End Function

    Public Function ConsultarDomicilios(ByVal idEmpresa_ As ObjectId) As TagWatcher _
                                     Implements IControladorEmpresas.ConsultarDomicilios
        With Estado

            If Not idEmpresa_ = ObjectId.Empty Then

                Select Case Modalidad

                    Case IControladorEmpresas.Modalidades.Extrinseca

                        If TipoEmpresa = IControladorEmpresas.TiposEmpresas.Nacional Then

                            ConsultarDomiciliosNacionales(idEmpresa_)

                        Else

                            ConsultarDomiciliosInternacionales(idEmpresa_)

                        End If

                    Case IControladorEmpresas.Modalidades.Intrinseca

                        If ListaEmpresas.Count > 0 Then

                            ConsultarDomiciliosInternos(idEmpresa_, PaisEmpresa)

                        Else

                            .SetError(Me, "Lista de empresas vacía")

                        End If

                End Select

            Else

                .SetError(Me, "No se recibió clave de empresa")

            End If

        End With

        Return Estado

    End Function

    Public Function ConsultarDomicilio(cveEmpresa_ As ObjectId,
                                       cveDomicilio_ As ObjectId) _
                                       As TagWatcher _
                                       Implements IControladorEmpresas.ConsultarDomicilio
        With Estado

            If Not cveEmpresa_ = ObjectId.Empty Then

                If Not cveDomicilio_ = ObjectId.Empty Then

                    Select Case Modalidad

                        Case IControladorEmpresas.Modalidades.Extrinseca

                            If TipoEmpresa = IControladorEmpresas.TiposEmpresas.Nacional Then

                                ConsultarDomicilioNacional(cveEmpresa_, cveDomicilio_)

                            Else

                                ConsultarDomicilioInternacional(cveEmpresa_, cveDomicilio_)

                            End If

                        Case IControladorEmpresas.Modalidades.Intrinseca

                            ConsultarDomicilioInterno(cveEmpresa_, cveDomicilio_)

                    End Select

                Else

                    .SetError(Me, "No se recibió clave de domicilio")

                End If

            Else

                .SetError(Me, "No se recibió clave de empresa")

            End If

        End With

        Return Estado

    End Function

    Function Archivar(ByVal listaObjectIdEmpresas_ As List(Of ObjectId)) _
                      As TagWatcher _
                      Implements IControladorEmpresas.Archivar
        With Estado

            If listaObjectIdEmpresas_.Count > 0 Then

                Select Case TipoEmpresa

                    Case IControladorEmpresas.TiposEmpresas.Nacional

                        ArchivarEmpresasNacionales(listaObjectIdEmpresas_)

                    Case IControladorEmpresas.TiposEmpresas.Internacional

                        ArchivarEmpresasInternacionales(listaObjectIdEmpresas_)

                End Select

            Else

                .SetError(Me, "No existen empresas en pila")

            End If

        End With

        Return Estado

    End Function

    'PENDIENTE DE IMPLEMENTAR
    Public Function ArchivarDomicilios(ByVal objectIdEmpresa As ObjectId,
                                       ByVal listaObjectIdDomicilio_ As List(Of ObjectId)) _
                                       As TagWatcher _
                                       Implements IControladorEmpresas.ArchivarDomicilios
        With Estado

            If Not objectIdEmpresa = ObjectId.Empty Then

                If listaObjectIdDomicilio_.Count > 0 Then

                    Select Case TipoEmpresa

                        Case IControladorEmpresas.TiposEmpresas.Nacional

                            ArchivarDomiciliosNacionales(objectIdEmpresa, listaObjectIdDomicilio_)

                        Case IControladorEmpresas.TiposEmpresas.Internacional

                            'ArchivarDomiciliosInternacionales(objectIdEmpresa, listaObjectIdDomicilio_)

                    End Select

                Else

                    .SetOKBut(Me, "No existen claves de domicilios")

                End If

            Else

                .SetError(Me, "No existe clave de empresa")

            End If

        End With

        Return Estado

    End Function

#End Region

#Region "CLONE AND DISPOSE"
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)

                _espacioTrabajo = Nothing

                _listaPaisesDomicilios = Nothing

                disposedValue = Nothing

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

    Public Sub ReiniciarControlador() _
        Implements IControladorEmpresas.ReiniciarControlador

        Inicializa(Nothing,
                   IControladorEmpresas.TiposEmpresas.Nacional,
                   "MEX",
                   IControladorEmpresas.Modalidades.Extrinseca)

    End Sub

End Class
#End Region