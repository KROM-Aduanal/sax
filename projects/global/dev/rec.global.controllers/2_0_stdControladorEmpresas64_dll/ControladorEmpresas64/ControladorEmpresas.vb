Imports System.Text.RegularExpressions
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports Wma.Exceptions
'Imports Rec.Globals.Controllers.IControladorEmpresas64
Imports Rec.Globals.IEmpresa64
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
Imports Microsoft.VisualBasic.FileIO
Imports System.Runtime.InteropServices


Public Class ControladorEmpresas
    Implements IControladorEmpresas, IDisposable

#Region "PROPIEDADES PRIVADAS"

    Private _listaPaisesDomicilios _
        As List(Of PaisDomicilio)

    Private _espacioTrabajo _
        As IEspacioTrabajo

    Private disposedValue As Boolean

    Private _paisPresentacion As String

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

        _paisPresentacion = "MEX - MÉXICO (ESTADOS UNIDOS MEXICANOS)"

        Estado = New TagWatcher

    End Sub


#Region "MÉTODOS PRIVADOS"

    Private Function BuscarPaisPorCveISO(ByVal pais_ As String) _
                                        As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

            Dim result_ = operationsDB_.Aggregate.
                                        Project(Function(e) _
                                                    New With {
                                                        Key ._id = e._id,
                                                        Key .cveISO3 = e.cveISO3,
                                                        Key .nombrepaisesp = e.nombrepaiscortoesp,
                                                        Key .estado = e.estado
                                            }).
                                       Match(Function(x) pais_.Equals(x.cveISO3) And x.estado = 1).
                                       Limit(1).
                                       ToList()


            With Estado

                If result_.Count <> 0 Then

                    .ObjectReturned = result_

                    .SetOK()

                Else

                    .SetOKBut(Me, "No se encontró país solicitado")

                End If

            End With

        End Using

        Return Estado

    End Function


    Private Function ObtenerDomicilio(ByRef paisesdomicilios_ As List(Of PaisDomicilio)) _
                                      As List(Of PaisDomicilio)

        _listaPaisesDomicilios = New List(Of PaisDomicilio)

        Dim buscarPais_ As TagWatcher

        Dim clavePais_ As ObjectId

        Dim pais_ As String

        Dim paisPresentacion_ As String

        If PaisEmpresa = "MEX" Then

            clavePais_ = New ObjectId("635acf2ba8210bfa0d5843f3")

            pais_ = PaisEmpresa

            paisPresentacion_ = _paisPresentacion

        Else

            buscarPais_ = BuscarPaisPorCveISO(PaisEmpresa)

            With buscarPais_

                If .Status = TagWatcher.TypeStatus.Ok Then

                    If .ObjectReturned.Count > 0 Then

                        clavePais_ = .ObjectReturned(0)._id

                        pais_ = .ObjectReturned(0).cveISO3

                        paisPresentacion_ = pais_ & " - " & .ObjectReturned(0).nombrepaisesp

                    End If

                End If

            End With

        End If

        _listaPaisesDomicilios = New List(Of PaisDomicilio) _
                                         From {(New PaisDomicilio _
                                         With {
                                                .idpais = clavePais_,
                                                .sec = 1,
                                                .pais = pais_,
                                                .paisPresentacion = paisPresentacion_,
                                                .domicilios = New List(Of Domicilio)
                                         })}

        If paisesdomicilios_ IsNot Nothing Then

            If paisesdomicilios_.Count > 0 Then

                Dim domicilioAux_ = paisesdomicilios_(0).domicilios

                Dim sec_ = 0

                For Each item_ In domicilioAux_

                    Dim aux_ = New Domicilio

                    sec_ = sec_ + 1

                    aux_ = New Domicilio With {
                                        ._iddomicilio = IIf(item_._iddomicilio = ObjectId.Empty, ObjectId.GenerateNewId, item_._iddomicilio),
                                        .sec = IIf(item_.sec > 0, item_.sec, sec_),
                                        .calle = item_.calle,
                                        .numeroexterior = item_.numeroexterior,
                                        .numerointerior = item_.numerointerior,
                                        .colonia = item_.colonia,
                                        .ciudad = item_.ciudad,
                                        .municipio = item_.municipio,
                                        .localidad = item_.localidad,
                                        .entidadfederativa = item_.entidadfederativa,
                                        .codigopostal = item_.codigopostal,
                                        .pais = pais_,
                                        .paisPresentacion = paisPresentacion_,
                                        .estado = 1,
                                        .archivado = False
                                    }

                    _listaPaisesDomicilios(0).domicilios.Add(aux_)

                Next

            End If

        End If

        Return _listaPaisesDomicilios

    End Function


    Private Function ObtenerRfc(ByRef rfc_ As String,
                                Optional ByRef idrfc_? As ObjectId = Nothing) _
                                As List(Of Rfc)

        Dim rfcAux_ As New List(Of Rfc)

        If rfc_ IsNot Nothing Then

            rfcAux_ = New List(Of Rfc) _
                            From {(New Rfc _
                            With {.idrfc = IIf(idrfc_ Is Nothing,
                                               ObjectId.GenerateNewId,
                                               idrfc_),
                                  .sec = 1,
                                  .rfc = rfc_,
                                  .estado = 1,
                                  .archivado = False
                            })}
        End If

        Return rfcAux_

    End Function


    Private Function ObtenerCurp(ByRef curp_ As String,
                                Optional ByRef idcurp_? As ObjectId = Nothing) _
                                As List(Of Curp)

        Dim curpAux_ As New List(Of Curp)

        If curp_ IsNot Nothing Then

            curpAux_ = New List(Of Curp) _
                            From {(New Curp _
                            With {.idcurp = IIf(idcurp_ Is Nothing,
                                               ObjectId.GenerateNewId,
                                               idcurp_),
                                  .sec = 1,
                                  .curp = curp_,
                                  .estado = 1,
                                  .archivado = False
                            })}
        End If

        Return curpAux_

    End Function


    Private Function ObtenerTaxid(ByRef taxid_ As List(Of TaxId)) _
        As List(Of TaxId)

        Dim taxidAux_ = New List(Of TaxId)

        If taxid_ IsNot Nothing Then

            taxidAux_ = New List(Of TaxId) _
                        From {(New TaxId _
                        With {
                                .idtaxid = ObjectId.GenerateNewId,
                                .sec = 1,
                                .taxid = taxid_(0).taxid,
                                .estado = 1,
                                .archivado = False
                            })}
        End If

        Return taxidAux_

    End Function


    Private Function ObtenerBus(ByRef bu_ As String) _
        As List(Of Bus)

        Dim buAux_ = New List(Of Bus)

        If bu_ IsNot Nothing Then

            buAux_ = New List(Of Bus) _
                            From {(New Bus _
                            With {.idunidadnegocio = ObjectId.GenerateNewId,
                                  .sec = 1,
                                  .unidadnegocio = bu_,
                                  .estado = 1,
                                  .archivado = False
                            })}
        End If

        Return buAux_

    End Function


    Private Function ArchivarRegistro(ByVal tipo_ As Integer,
                                      ByVal item_ As ObjectId) _
                                      As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

            Dim filter_ As FilterDefinition(Of EmpresaNacional)

            Dim setStructureOfSubs_ As UpdateDefinition(Of EmpresaNacional)

            Dim result_ As UpdateResult

            Select Case tipo_

                Case 1

                    'Dim filter_ As FilterDefinition(Of EmpresaNacional) = Builders(Of EmpresaNacional).Filter.And(Builders(Of EmpresaNacional).Filter.Eq(Function(e) e._id, objectIdEmpresa_),
                    'Builders(Of EmpresaNacional).Filter.ElemMatch(Function(x) x.rfcs, Function(y) y.idrfc.Equals(objectIdEmpresa_)))

                    filter_ = Builders(Of EmpresaNacional).Filter.ElemMatch(Function(y) y.rfcs,
                                                                        Function(z) z.idrfc.Equals(item_))

                    setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.
                                      Set(Of Boolean)("rfcs.$[].archivado", True)

                    result_ = operationsDB_.UpdateMany(filter_, setStructureOfSubs_)

                Case 2

                    filter_ = Builders(Of EmpresaNacional).Filter.ElemMatch(Function(y) y.curps,
                                                                      Function(z) z.idcurp.Equals(item_))

                    setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.
                                      Set(Of Boolean)("curps.$[].archivado", True)

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


    Private Function SecuenciaRfcInterna(ByRef empresa_ As EmpresaNacional) _
                                         As Integer

        Dim secLast_ = empresa_.rfcs.Last.sec

        With empresa_

            Dim idLast_ = .rfcs.Last.idrfc

            Dim rfcLast_ = .rfcs.Last.rfc

            If .rfcNuevo IsNot Nothing Then

                If .rfcNuevo Then

                    ArchivarRegistro(1, idLast_)

                    secLast_ += 1

                End If

            Else

                If empresa_.rfc <> rfcLast_ Then

                    ArchivarRegistro(1, idLast_)

                    secLast_ += 1

                End If

            End If

        End With

        Return secLast_

    End Function


    Private Function SecuenciaCurpInterna(ByRef empresa_ As EmpresaNacional) _
                                          As Integer

        Dim secLast_ = empresa_.curps.Last.sec

        With empresa_

            Dim idLast_ = .curps.Last.idcurp

            Dim curpLast_ = .curps.Last.curp

            If .curpNuevo IsNot Nothing Then

                If .curpNuevo Then

                    ArchivarRegistro(2, idLast_)

                    secLast_ += 1

                End If

            Else

                If empresa_.curp <> curpLast_ Then

                    ArchivarRegistro(2, idLast_)

                    secLast_ += 1

                End If

            End If

        End With

        Return secLast_

    End Function


    Private Function AgregarDomicilio(ByRef empresa_ As Empresa) _
                                      As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                                         With {.EspacioTrabajo = _espacioTrabajo}

            With empresa_

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                Dim lastDomicilio = empresa_.paisesdomicilios.Last.domicilios.Last

                Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x._id, empresa_._id) And
                              Builders(Of EmpresaNacional).Filter.ElemMatch(Function(y) y.paisesdomicilios,
                                                                            Function(x) x.pais.Equals(PaisEmpresa))

                Dim setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.
                                          AddToSet("paisesdomicilios.$.domicilios", New Domicilio With {
                                                             ._iddomicilio = lastDomicilio._iddomicilio,
                                                             .sec = lastDomicilio.sec,
                                                             .calle = lastDomicilio.calle,
                                                             .numeroexterior = lastDomicilio.numeroexterior,
                                                             .numerointerior = lastDomicilio.numerointerior,
                                                             .ciudad = lastDomicilio.ciudad,
                                                             .colonia = lastDomicilio.colonia,
                                                             .localidad = lastDomicilio.localidad,
                                                             .municipio = lastDomicilio.municipio,
                                                             .codigopostal = lastDomicilio.codigopostal,
                                                             .entidadfederativa = lastDomicilio.entidadfederativa,
                                                             .pais = lastDomicilio.pais,
                                                             .paisPresentacion = lastDomicilio.paisPresentacion
                                          })

                Dim result_ = operationsDB_.UpdateOne(filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True})

                With Estado

                    If result_.ModifiedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se generaron cambios")

                    End If

                End With

            End With

        End Using

        Return Estado

    End Function


    Private Function SecuenciaContactoInterna(ByRef empresa_ As Empresa) _
        As Contacto

        Dim structureContactos_ As Contacto

        Dim pruebas_ = empresa_.contactos

        If empresa_.contactos.Count <> 0 Then

            If empresa_.contactos.Last IsNot Nothing Then

                Dim lastContacto = empresa_.contactos.Last

                structureContactos_ = New Contacto With {
                                            .idcontacto = lastContacto.idcontacto,
                                            .sec = lastContacto.sec,
                                            .nombrecompleto = lastContacto.nombrecompleto,
                                            .correoelectronico = lastContacto.correoelectronico,
                                            .telefono = lastContacto.telefono
                                      }

            End If

        End If

        Return structureContactos_

    End Function


    Private Function SecuenciaRegimenFiscalInterna(ByRef empresa_ _
                                                   As EmpresaNacional) As RegimenFiscal
        Dim structureRegimenFiscal_ As RegimenFiscal

        If empresa_.regimenesfiscales.Count <> 0 Then

            If empresa_.regimenesfiscales.Last IsNot Nothing Then

                Dim lastRegimenFiscal = empresa_.regimenesfiscales.Last

                structureRegimenFiscal_ = New RegimenFiscal With {
                                            .idregimenfiscal = lastRegimenFiscal.idregimenfiscal,
                                            ._sec = lastRegimenFiscal._sec,
                                            .regimenfiscal = lastRegimenFiscal.regimenfiscal
                                          }

            End If

        End If

        Return structureRegimenFiscal_

    End Function


    Private Function SecuenciaTaxIdInterna(ByRef empresa_ _
                                           As EmpresaInternacional) As TaxId
        Dim structureTaxId_ As TaxId

        If empresa_.taxids.Count <> 0 Then

            If empresa_.taxids.Last IsNot Nothing Then

                Dim lastTaxId = empresa_.taxids.Last

                structureTaxId_ = New TaxId With {
                                                .idtaxid = lastTaxId.idtaxid,
                                                .sec = lastTaxId.sec,
                                                .taxid = lastTaxId.taxid
                                              }

            End If

        End If

        Return structureTaxId_

    End Function


    Private Function SecuenciaBusInterna(ByRef empresa_ _
                                         As EmpresaInternacional) As Bus
        Dim structureBus_ As Bus

        If empresa_.bus.Count <> 0 Then

            If empresa_.bus.Last IsNot Nothing Then

                Dim lastBus = empresa_.bus.Last

                structureBus_ = New Bus With {
                                                .idunidadnegocio = lastBus.idunidadnegocio,
                                                .sec = lastBus.sec,
                                                .unidadnegocio = lastBus.unidadnegocio
                                              }

                Return structureBus_

            End If

        End If

        Return structureBus_

    End Function

    Private Function SecuenciaDomiciliosInterna(ByRef empresa_ _
                                                As Empresa) As Domicilio
        Dim structureDomicilio_ As Domicilio

        If empresa_.paisesdomicilios.Count <> 0 Then

            If empresa_.paisesdomicilios.Last.domicilios IsNot Nothing Then

                Dim lastDomicilio = empresa_.paisesdomicilios.Last.domicilios.Last

                structureDomicilio_ = New Domicilio With {
                                                            ._iddomicilio = lastDomicilio._iddomicilio,
                                                            .sec = lastDomicilio.sec,
                                                            .calle = lastDomicilio.calle,
                                                            .numeroexterior = lastDomicilio.numeroexterior,
                                                            .numerointerior = lastDomicilio.numerointerior,
                                                            .ciudad = lastDomicilio.ciudad,
                                                            .colonia = lastDomicilio.colonia,
                                                            .localidad = lastDomicilio.localidad,
                                                            .municipio = lastDomicilio.municipio,
                                                            .codigopostal = lastDomicilio.codigopostal,
                                                            .entidadfederativa = lastDomicilio.entidadfederativa,
                                                            .pais = lastDomicilio.pais,
                                                            .paisPresentacion = lastDomicilio.paisPresentacion
                                                        }
            End If

        End If

        Return structureDomicilio_

    End Function


    Private Function AgregarEmpresaNacional(ByVal empresaNacional_ As EmpresaNacional,
                                            Optional ByVal objetoRetorno_ As Boolean = False,
                                            Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                            As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

            Dim totalEmpresasNacionales_ = operationsDB_.EstimatedDocumentCount()

            With empresaNacional_

                ._id = ObjectId.GenerateNewId

                ._idempresa = Convert.ToInt64(totalEmpresasNacionales_) + 1

                If ._idcurp = ObjectId.Empty Then

                    .curps = ObtenerCurp(.curp)

                    ._idcurp = .curps.Last.idcurp

                ElseIf .curps Is Nothing Or .curps.Count = 0 Then

                    .curps = ObtenerCurp(.curp, ._idcurp)

                    ._idcurp = .curps.Last.idcurp

                End If

                If ._idrfc = ObjectId.Empty Then

                    .rfcs = ObtenerRfc(.rfc)

                    ._idrfc = .rfcs.Last.idrfc

                ElseIf .rfcs Is Nothing Or .rfcs.Count = 0 Then

                    .rfcs = ObtenerRfc(.rfc, ._idrfc)

                    ._idrfc = .rfcs.Last.idrfc

                End If

                .paisesdomicilios = ObtenerDomicilio(.paisesdomicilios)

                .estatus = 1

                .estado = 1

                .abierto = True

                .archivado = False

            End With

            If session_ IsNot Nothing Then

                Dim result_ = operationsDB_.InsertOneAsync(session_, empresaNacional_).ConfigureAwait(False)

            Else

                Dim result_ = operationsDB_.InsertOneAsync(empresaNacional_).ConfigureAwait(False)

            End If

            With Estado

                If objetoRetorno_ Then

                    Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.razonsocial, empresaNacional_.razonsocial)

                    Dim empresaResult_ = operationsDB_.Find(filter_).Limit(1).ToList()

                    If empresaResult_.Count > 0 Then

                        ListaEmpresas = New List(Of IEmpresa)

                        ListaEmpresas.Add(empresaResult_(0))

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

            Dim totalEmpresasInternacionales_ = operationsDB_.EstimatedDocumentCount()

            With empresaInternacional_

                ._id = ObjectId.GenerateNewId

                ._idempresa = Convert.ToInt64(totalEmpresasInternacionales_) + 1

                .paisesdomicilios = ObtenerDomicilio(.paisesdomicilios)

                If .taxids.Last.idtaxid = ObjectId.Empty Then

                    .taxids = ObtenerTaxid(.taxids)

                End If

                .estatus = 1

                .estado = 1

                .abierto = True

                .archivado = False

            End With

            If session_ IsNot Nothing Then

                Dim result_ = operationsDB_.InsertOneAsync(session_, empresaInternacional_).ConfigureAwait(False)

            Else

                Dim result_ = operationsDB_.InsertOneAsync(empresaInternacional_).ConfigureAwait(False)

            End If

            With Estado

                If objetoRetorno_ Then

                    ListaEmpresas = New List(Of IEmpresa)

                    Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.razonsocial, empresaInternacional_.razonsocial)

                    Dim empresaResult_ = operationsDB_.Find(filter_).Limit(1).ToList()

                    If empresaResult_.Count > 0 Then

                        ListaEmpresas = New List(Of IEmpresa)

                        ListaEmpresas.Add(empresaResult_(0))

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

                Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.razonsocial, .razonsocial) And
                              Builders(Of EmpresaNacional).Filter.ElemMatch(Function(y) y.paisesdomicilios,
                                                                            Function(x) x.pais.Equals(PaisEmpresa)) And
                              Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

                Dim lastDomicilio_ = .paisesdomicilios.Last.domicilios.Last

                Dim iddomicilio_ = lastDomicilio_._iddomicilio

                Dim secDomicilio_ = lastDomicilio_.sec

                If .esNuevoDomicilio Then

                    iddomicilio_ = ObjectId.GenerateNewId

                    secDomicilio_ += 1

                End If

                Dim structureContactos_ As Contacto

                If .contactos IsNot Nothing Then

                    structureContactos_ = SecuenciaContactoInterna(empresaNacional_)

                End If

                Dim structureRegimenFiscal_ As RegimenFiscal

                If .regimenesfiscales IsNot Nothing Then

                    structureRegimenFiscal_ = SecuenciaRegimenFiscalInterna(empresaNacional_)

                End If

                Dim rfcAux_ As Rfc

                If .rfcs.Last IsNot Nothing Then

                    Dim secRfc_ = SecuenciaRfcInterna(empresaNacional_)

                    rfcAux_ = New Rfc _
                              With {
                                    .idrfc = IIf(Not empresaNacional_._idrfc = ObjectId.Empty,
                                                  empresaNacional_._idrfc, ObjectId.GenerateNewId),
                                    .sec = secRfc_,
                                    .rfc = empresaNacional_.rfc,
                                    .archivado = False,
                                    .estado = 1
                              }
                End If

                Dim curpAux_ As Curp

                If .curps.Last IsNot Nothing Then

                    Dim secCurp_ = SecuenciaCurpInterna(empresaNacional_)

                    curpAux_ = New Curp _
                               With {.idcurp = IIf(empresaNacional_._idcurp IsNot Nothing,
                                                   empresaNacional_._idcurp,
                                                   ObjectId.GenerateNewId),
                                     .sec = secCurp_,
                                     .curp = empresaNacional_.curp,
                                     .archivado = False,
                                     .estado = 1
                               }

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
                                                                        Set(Function(x) x._idrfc, rfcAux_.idrfc).
                                                                        Set(Function(x) x.rfc, rfcAux_.rfc).
                                                                        AddToSet("rfcs", rfcAux_).
                                                                        Set(Function(x) x._idcurp, curpAux_.idcurp).
                                                                        Set(Function(x) x.curp, curpAux_.curp).
                                                                        AddToSet("curps", curpAux_).
                                                                        AddToSet("paisesdomicilios.$.domicilios", New Domicilio With {
                                                                             ._iddomicilio = iddomicilio_,
                                                                             .sec = secDomicilio_,
                                                                             .calle = lastDomicilio_.calle,
                                                                             .numeroexterior = lastDomicilio_.numeroexterior,
                                                                             .numerointerior = lastDomicilio_.numerointerior,
                                                                             .ciudad = lastDomicilio_.ciudad,
                                                                             .colonia = lastDomicilio_.colonia,
                                                                             .localidad = lastDomicilio_.localidad,
                                                                             .municipio = lastDomicilio_.municipio,
                                                                             .codigopostal = lastDomicilio_.codigopostal,
                                                                             .entidadfederativa = lastDomicilio_.entidadfederativa,
                                                                             .pais = lastDomicilio_.pais,
                                                                             .paisPresentacion = lastDomicilio_.paisPresentacion
                                                                        }).
                                                                        AddToSet("contactos", structureContactos_).
                                                                        AddToSet("regimenesfiscales", structureRegimenFiscal_)

                Dim result_ As UpdateResult

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_,
                                                              New UpdateOptions With {.IsUpsert = True}).Result
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

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                                         With {.EspacioTrabajo = _espacioTrabajo}

            With empresaInternacional_

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.razonsocial, .razonsocial) And
                              Builders(Of EmpresaInternacional).Filter.ElemMatch(Function(y) y.paisesdomicilios,
                                                                                 Function(x) x.pais.Equals(PaisEmpresa)) And
                              Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

                Dim lastDomicilio_ = .paisesdomicilios.Last.domicilios.Last

                Dim iddomicilio_ = lastDomicilio_._iddomicilio

                Dim secDomicilio_ = lastDomicilio_.sec

                If .esNuevoDomicilio Then

                    iddomicilio_ = ObjectId.GenerateNewId

                    secDomicilio_ += 1

                End If

                Dim structureContactos_ As Contacto

                If .contactos IsNot Nothing Then

                    structureContactos_ = SecuenciaContactoInterna(empresaInternacional_)

                End If

                Dim structureTaxId_ As TaxId

                If .taxids IsNot Nothing Then

                    structureTaxId_ = SecuenciaTaxIdInterna(empresaInternacional_)

                End If

                Dim structureBus_ As Bus

                If .bus IsNot Nothing Then

                    structureBus_ = SecuenciaBusInterna(empresaInternacional_)

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
                                                                        AddToSet("taxids", structureTaxId_).
                                                                        AddToSet("paisesdomicilios.$.domicilios", New Domicilio With {
                                                                             ._iddomicilio = iddomicilio_,
                                                                             .sec = secDomicilio_,
                                                                             .calle = lastDomicilio_.calle,
                                                                             .numeroexterior = lastDomicilio_.numeroexterior,
                                                                             .numerointerior = lastDomicilio_.numerointerior,
                                                                             .ciudad = lastDomicilio_.ciudad,
                                                                             .colonia = lastDomicilio_.colonia,
                                                                             .localidad = lastDomicilio_.localidad,
                                                                             .municipio = lastDomicilio_.municipio,
                                                                             .codigopostal = lastDomicilio_.codigopostal,
                                                                             .entidadfederativa = lastDomicilio_.entidadfederativa,
                                                                             .pais = lastDomicilio_.pais,
                                                                             .paisPresentacion = lastDomicilio_.paisPresentacion
                                                                        }).
                                                                        AddToSet("contactos", structureContactos_).
                                                                        AddToSet("bus", structureBus_)
                Dim result_ As UpdateResult

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_,
                                                           New UpdateOptions With {.IsUpsert = True}).Result

                Else

                    result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_,
                                                           New UpdateOptions With {.IsUpsert = True}).Result

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

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                                             With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsdb_ = iEnlace_.
                                    GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                'release el proyecto y deben de ir las puras dll 

                'INDICE CATEGORIA 4 DE TEXTO
                'Dim queryExpr_ = New BsonRegularExpression(New Regex(razonSocial_, RegexOptions.IgnoreCase))

                'Dim filter_ = Builders(Of EmpresaNacional).Filter.Regex("razonsocial", queryExpr_) And
                '              Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

                Dim filter_ = Builders(Of EmpresaNacional).Filter.Text(razonSocial_) And
                              Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

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

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                                             With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsdb_ = iEnlace_.
                                    GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                Dim paisFilter_ = PaisEmpresa

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.ElemMatch(Function(x) x.paisesdomicilios,
                                                                                 Function(z) z.pais.Equals(paisFilter_)) And
                              Builders(Of EmpresaInternacional).Filter.Text(razonSocial_) And
                              Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

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

            Select Case Modalidad

                Case IControladorEmpresas.Modalidades.Extrinseca

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = _espacioTrabajo}

                        Dim operationsDB_ = iEnlace_.
                                            GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                        Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x._id, cveEmpresa_) And
                                      Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

                        Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                        If result_.Count > 0 Then

                            .ObjectReturned = result_(0)

                            .SetOK()

                        Else

                            .SetOKBut(Me, "Empresa no encontrada")

                        End If

                    End Using

                Case IControladorEmpresas.Modalidades.Intrinseca

                    If ListaEmpresas.Count > 0 Then

                        .ObjectReturned = ListaEmpresas.Where(Function(t) t._id = cveEmpresa_)

                    Else

                        .SetOKBut(Me, "Lista de empresas vacía")

                    End If

            End Select

        End With

        Return Estado

    End Function

    Private Function ConsultarEmpresaInternacional(ByVal cveEmpresa_ As ObjectId) _
                                                   As TagWatcher
        With Estado

            Select Case Modalidad

                Case IControladorEmpresas.Modalidades.Extrinseca

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                        With {.EspacioTrabajo = _espacioTrabajo}

                        Dim operationsDB_ = iEnlace_.
                                            GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

                        Dim filter_ = Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x._id, cveEmpresa_) And
                                      Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1)

                        Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                        If result_.Count > 0 Then

                            .ObjectReturned = result_(0)

                            .SetOK()

                        Else

                            .SetOKBut(Me, "Empresa no encontrada")

                        End If

                    End Using

                Case IControladorEmpresas.Modalidades.Intrinseca

                    If ListaEmpresas.Count > 0 Then

                        .ObjectReturned = ListaEmpresas.Where(Function(t) t._id = cveEmpresa_)

                    Else

                        .SetOKBut(Me, "Lista de empresas vacía")

                    End If

            End Select

        End With

        Return Estado

    End Function


    Private Function ConsultarDomiciliosEmpresaNacional(ByVal cveEmpresa_ _
                                                        As ObjectId) As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Select Case Modalidad

                Case IControladorEmpresas.Modalidades.Extrinseca

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = _espacioTrabajo}

                        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

                        Dim filter_ = Builders(Of EmpresaNacional).Filter.Eq(Function(x) x._id, cveEmpresa_) And
                                      Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1)

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

                Case IControladorEmpresas.Modalidades.Intrinseca

                    If ListaEmpresas IsNot Nothing And
                        ListaEmpresas.Count > 0 Then

                        Dim domiciliosInt_ = ListaEmpresas.
                            Where(Function(t) t._id = cveEmpresa_).
                            Select(Function(z) z.paisesdomicilios.
                            Where(Function(a) a.pais = PaisEmpresa).
                            Select(Function(b) b.domicilios))

                        For Each item_ In domiciliosInt_

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

                    Else

                        .SetOKBut(Me, "Lista de empresas vacía")

                    End If

            End Select

        End With

        Return Estado

    End Function

    Private Function ConsultarDomiciliosEmpresaInternacional(ByVal cveEmpresa_ _
                                                             As ObjectId) As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Select Case Modalidad

                Case IControladorEmpresas.Modalidades.Extrinseca

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = _espacioTrabajo}

                        Dim operationsDB_ = iEnlace_.
                                            GetMongoCollection(Of EmpresaInternacional)("Glo007EmpresasInternacionales")

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

                Case IControladorEmpresas.Modalidades.Intrinseca

                    If ListaEmpresas IsNot Nothing And
                        ListaEmpresas.Count > 0 Then

                        Dim domiciliosInt_ = ListaEmpresas.
                            Where(Function(t) t._id = cveEmpresa_).
                            Select(Function(z) z.paisesdomicilios.
                            Where(Function(a) a.pais = PaisEmpresa).
                            Select(Function(b) b.domicilios))

                        For Each item_ In domiciliosInt_

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

                    Else

                        .SetOKBut(Me, "Lista de empresas vacía")

                    End If

            End Select

        End With

        Return Estado

    End Function

    Private Function ConsultarDomicilioEmpresaNacional(ByVal cveEmpresa_ As ObjectId,
                                                       ByVal cveDomicilio_ As ObjectId) _
                                                       As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Select Case Modalidad

                Case IControladorEmpresas.Modalidades.Extrinseca

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = _espacioTrabajo}

                        Dim operationsDB_ = iEnlace_.
                                            GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")

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

                Case IControladorEmpresas.Modalidades.Intrinseca

                    If ListaEmpresas IsNot Nothing And
                        ListaEmpresas.Count > 0 Then

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

            End Select

        End With

        Return Estado

    End Function

    Private Function ConsultarDomicilioEmpresaInternacional(ByVal cveEmpresa_ As ObjectId,
                                                            ByVal cveDomicilio_ As ObjectId) _
                                                            As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Select Case Modalidad

                Case IControladorEmpresas.Modalidades.Extrinseca

                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = _espacioTrabajo}

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

                Case IControladorEmpresas.Modalidades.Intrinseca

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

            End Select

        End With

        Return Estado

    End Function

    Private Function ArchivarEmpresasNacionales(ByRef objectIdEmpresa_ _
                                                As List(Of ObjectId)) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)("Glo007EmpresasNacionales")


            Dim filter_ = Builders(Of EmpresaNacional).Filter.In(Function(x) x._id, objectIdEmpresa_)

            Dim setStructureOfSubs_ = Builders(Of EmpresaNacional).Update.
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

    Private Function ArchivarEmpresasInternacionales(ByRef listaObjectIdEmpresa_ _
                                                     As List(Of ObjectId)) As TagWatcher

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
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
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
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
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
                              Optional session_ As IClientSessionHandle = Nothing) _
                              As TagWatcher _
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
                              Optional session_ As IClientSessionHandle = Nothing) _
                              As TagWatcher _
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

    Public Function ConsultarUna(cveEmpresa_ As ObjectId) _
                                 As TagWatcher _
                                 Implements IControladorEmpresas.ConsultarUna
        With Estado

            If Not cveEmpresa_ = ObjectId.Empty Then

                Select Case TipoEmpresa

                    Case IControladorEmpresas.TiposEmpresas.Nacional

                        ConsultarEmpresaNacional(cveEmpresa_)

                    Case IControladorEmpresas.TiposEmpresas.Internacional

                        ConsultarEmpresaInternacional(cveEmpresa_)

                End Select

            Else

                .SetError(Me, "No se recibió una clave de empresa")

            End If

        End With

        Return Estado

    End Function


    Public Function ConsultarDomicilios(ByVal cveEmpresa_ As ObjectId) _
                                     As TagWatcher _
                                     Implements IControladorEmpresas.ConsultarDomicilios
        With Estado

            If Not cveEmpresa_ = ObjectId.Empty Then

                Select Case TipoEmpresa

                    Case IControladorEmpresas.TiposEmpresas.Nacional

                        ConsultarDomiciliosEmpresaNacional(cveEmpresa_)

                    Case IControladorEmpresas.TiposEmpresas.Internacional

                        ConsultarDomiciliosEmpresaInternacional(cveEmpresa_)

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

                    Select Case TipoEmpresa

                        Case IControladorEmpresas.TiposEmpresas.Nacional

                            ConsultarDomicilioEmpresaNacional(cveEmpresa_, cveDomicilio_)

                        Case IControladorEmpresas.TiposEmpresas.Internacional

                            ConsultarDomicilioEmpresaInternacional(cveEmpresa_, cveDomicilio_)

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


    ''' <summary>
    ''' 'NO ES CORRECTO QUE VAYA EN ESTE LUGAR, PERO ES UNA NECESIDAD QUE SE TENÍA QUE SOLVENTAR DE MOMENTO, AL MENOS
    ''' HASTA QUE EL INGE Y ROSITA INDIQUEN LO CONTRARIO O SE DE UNA MEJOR SOLUCIÓN
    ''' PERO TU NO VASS AQUI
    ''' ESTA NO ES TU FAMILIA <
    ''' <returns></returns>
    Public Function ConsultarPaises(ByVal pais_ As String,
                                    Optional ByVal limiteResultados_ As Int32 = 10) _
                                    As TagWatcher _
                                    Implements IControladorEmpresas.ConsultarPaises

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos _
                                             With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

                Dim filter_ = Builders(Of Pais).Filter.Text(pais_) And
                              Builders(Of Pais).Filter.Eq(Function(x) x.estado, 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(limiteResultados_).ToList()

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

End Class
#End Region