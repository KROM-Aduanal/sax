Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports Wma.Exceptions
Imports Rec.Globals.Controllers.ControladorPaises
Imports Rec.Globals.Empresas
Imports Rec.Globals.Utils.Secuencias
Imports Wma.Exceptions.TagWatcher
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo
Imports MongoDB.Bson.Serialization


Public Class ControladorEmpresas
    Implements IControladorEmpresas, IDisposable

    '********************** DUMMIES *************************************************

    '********************* EMPRESA INTERNACIONAL *************************************

    ''INSTANCIAR LA CLASE PARA EMPRESAS INTERNACIONALES
    'Dim _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(_espacioTrabajo,
    '                                               Rec.Globals.Controllers.Empresas.IControladorEmpresas.TiposEmpresas.Internacional)

    ''ES IMPORTANTE AGREGAR EL PAÍS CON EL QUE VA A TRABAJAR ESA EMPRESA
    '_controladorEmpresas.PaisEmpresa = "ESP"


    ' _tagwatcher = _controladorEmpresas.Consultar("PROVEEDOR CHINO")
    '_tagwatcher = _controladorEmpresas.ConsultarUna(New ObjectId("662bc88ac4054e506baeb4e6"))

    'NOTA: Recordar que esta forma de consumir el controlador " New Rec.Globals.Controllers.Empresas" es temporal

    ' ****************** EMPRESA NACIONAL **********************************

    ''INSTANCIAR PARA UNA EMPRESA NACIONAL
    'Dim _controladorEmpresas = New Rec.Globals.Controllers.Empresas.ControladorEmpresas(_espacioTrabajo,
    ' Rec.Globals.Controllers.Empresas.IControladorEmpresas.TiposEmpresas.Nacional)

    ''FORMA DE TRABAJAR DE FORMA LOCAL POR DEFECTO EL CONTROLADOR TIENE UNA MODALIDAD EXTRINSECA
    '_controladorEmpresas.Modalidad = IControladorEmpresas.Modalidades.Intrinseca

    ''ES IMPORTANTE LLENAR LA PROPIEDAD ListaEmpresas
    ''POR DEFAULT YA SE ENCUENTRA INICIALIZADA
    ' _controladorEmpresas.ListaEmpresas.Add(_tagwatcher.ObjectReturned(0))

    '_controladorEmpresas.ConsultarDomicilios(New ObjectId("661d5b65272307d7f2f8e3dc"))

    ' _controladorEmpresas.ConsultarDomicilio(New ObjectId("662bc88ac4054e506baeb4e6"),
    ' New ObjectId("662bd665aa793757435ee7ac"))


#Region "PROPIEDADES PRIVADAS"

    Private _listaPaisesDomicilios _
        As List(Of PaisDomicilio)

    Private _espacioTrabajo _
        As IEspacioTrabajo

    Private disposedValue As Boolean

    Private _controladorSecuencias As IControladorSecuencia

    Private _controladorPaises As ControladorPaises

    Private _secuencia As ISecuencia

    Private _paisInterno As Pais


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

        _paisInterno = New Pais _
                       With {
                                ._id = New ObjectId("661d5b65272307d7f2f8e3dc"),
                                .cveISO3 = "MEX",
                                .nombrepaisesp = "MEX - MÉXICO (ESTADOS UNIDOS MEXICANOS)"
                        }

        ListaEmpresas = New List(Of IEmpresa)

        Estado = New TagWatcher

    End Sub


#Region "MÉTODOS PRIVADOS"
    Private Function GenerarSecuencia(Optional ByVal tipoSecuencia_ _
                                      As Int32 = 1) _
                                      As Secuencia

        _controladorSecuencias = New ControladorSecuencia

        _secuencia = New Secuencia

        Dim secuencia_ = Recursos.SecuenciasRecursos.Empresas.ToString

        Estado = _controladorSecuencias.Generar(secuencia_, tipoSecuencia_, 1, 1, 1, 1)

        With Estado

            If .Status = TagWatcher.TypeStatus.Ok Then

                _secuencia = DirectCast(.ObjectReturned, Secuencia)

            End If

        End With

        Return _secuencia

    End Function

    Private Function GenerarPais() As Pais

        _paisInterno = New Pais

        Estado = ControladorPaises.ConsultarListaPaisesPorClaveISO(PaisEmpresa)

        With Estado

            If .Status = TagWatcher.TypeStatus.Ok Then

                For Each item_ In .ObjectReturned

                    With _paisInterno

                        ._id = item_._id
                        .cveISO3 = item_.cveISO3
                        .nombrepaisesp = item_.paisPresentacion

                    End With

                Next

            End If

        End With

        Return _paisInterno

    End Function

    Private Function ArchivarRegistro(ByVal tipo_ As IControladorEmpresas.TipoEstructura,
                                            ByVal filtro_ As String) _
                                            As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)(GetType(EmpresaNacional).Name)

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

                If result_ IsNot Nothing Then

                    If result_.MatchedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    Else

                        .SetOKBut(Me, "No se generaron cambios")

                    End If

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

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)(GetType(EmpresaNacional).Name)


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

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)(GetType(EmpresaNacional).Name)

                Dim filter_ = Builders(Of EmpresaNacional).Filter.And(
                                Builders(Of EmpresaNacional).Filter.Eq(Of ObjectId)("_id", ._id),
                                Builders(Of EmpresaNacional).Filter.ElemMatch(Function(y) y.paisesdomicilios, Function(x) x.pais.Equals(PaisEmpresa)),
                                Builders(Of EmpresaNacional).Filter.Eq(Function(x) x.estado, 1))

                Dim lastDomicilio_ As New Domicilio

                lastDomicilio_ = DirectCast(empresaNacional_.paisesdomicilios.Last.domicilios.Last, Domicilio)

                If .rfcNuevo Then

                    ArchivarRegistro(IControladorEmpresas.TipoEstructura.RFC, .razonsocial)

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
                                                                        AddToSet("paisesdomicilios.$.domicilios", lastDomicilio_)
                If .curpNuevo IsNot Nothing Then

                    If .curpNuevo Then

                        ArchivarRegistro(IControladorEmpresas.TipoEstructura.CURP, .razonsocial)

                        setStructureOfSubs_ = setStructureOfSubs_.AddToSet("curps", DirectCast(.curps.Last, Curp))

                    End If

                End If

                Dim estructuraContactos_ = Nothing

                If .contactos IsNot Nothing Then

                    If .contactos.Count > 0 Then

                        If .contactos.Last IsNot Nothing Then

                            setStructureOfSubs_ = setStructureOfSubs_.AddToSet("contactos", DirectCast(.contactos.Last, Contacto))

                        End If

                    End If

                End If

                If .regimenesfiscales IsNot Nothing Then

                    If .regimenesfiscales.Count > 0 Then

                        If .regimenesfiscales.Last IsNot Nothing Then

                            setStructureOfSubs_ = setStructureOfSubs_.AddToSet("regimenesfiscales", DirectCast(.regimenesfiscales.Last, RegimenFiscal))

                        End If

                    End If

                End If

                Dim result_ As UpdateResult

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result

                Else

                    result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result

                End If

                With Estado

                    If result_ IsNot Nothing Then

                        If result_.ModifiedCount <> 0 Then

                            .SetOK()

                        ElseIf result_.UpsertedId IsNot Nothing Then

                            .SetOK()

                        ElseIf result_.MatchedCount = 1 Then

                            .SetOK()

                        Else

                            .SetOKBut(Me, "Registro no modificado")

                        End If

                    Else

                        .SetOKBut(Me, "Registro no modificado")

                    End If

                End With

            End With

        End Using

        Return Estado

    End Function

    Private Function ModificarEmpresaInternacional(ByRef empresaInternacional_ As EmpresaInternacional,
                                                   Optional session_ As IClientSessionHandle = Nothing,
                                                   Optional ByVal crearEstructuraDomicilio_ As Boolean = False) _
                                                   As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            With empresaInternacional_

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional)(GetType(EmpresaInternacional).Name)

                Dim filter_ = Builders(Of EmpresaInternacional).Filter.And(
                                Builders(Of EmpresaInternacional).Filter.Eq(Of ObjectId)("_id", ._id),
                                Builders(Of EmpresaInternacional).Filter.ElemMatch(Function(y) y.paisesdomicilios, Function(x) x.pais.Equals(PaisEmpresa)),
                                Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1))

                Dim setStructureOfSubs_ As UpdateDefinition(Of EmpresaInternacional)

                Dim domicilioNuevo_ = .paisesdomicilios.Where(Function(x) x.pais = PaisEmpresa).Select(Function(x) x.domicilios.Last).AsEnumerable.ToList()

                setStructureOfSubs_ = Builders(Of EmpresaInternacional).Update.
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
                                                                        AddToSet("taxids", .taxids.Last)

                If crearEstructuraDomicilio_ Then

                    filter_ = Builders(Of EmpresaInternacional).Filter.And(
                                Builders(Of EmpresaInternacional).Filter.Eq(Of ObjectId)("_id", ._id),
                                Builders(Of EmpresaInternacional).Filter.Eq(Function(x) x.estado, 1))

                    Dim estructuraPaisDomicilio_ As New PaisDomicilio

                    estructuraPaisDomicilio_ = DirectCast(.paisesdomicilios.Where(Function(x) x.pais = PaisEmpresa).AsEnumerable.ToList()(0), PaisDomicilio)

                    setStructureOfSubs_ = setStructureOfSubs_.AddToSet("paisesdomicilios", estructuraPaisDomicilio_)

                Else

                    setStructureOfSubs_ = setStructureOfSubs_.AddToSet("paisesdomicilios.$.domicilios", DirectCast(domicilioNuevo_.Last, Domicilio))

                End If

                If .contactos IsNot Nothing Then

                    If .contactos.Count > 0 Then

                        If .contactos.Last IsNot Nothing Then

                            setStructureOfSubs_ = setStructureOfSubs_.AddToSet("contactos", DirectCast(.contactos.Last, Contacto))

                        End If

                    End If

                End If

                Dim estructuraBus_ = Nothing

                If .bus IsNot Nothing Then

                    If .bus.Count > 0 Then

                        If .bus.Last IsNot Nothing Then

                            setStructureOfSubs_ = setStructureOfSubs_.AddToSet("bus", DirectCast(.bus.Last, Bus))

                        End If

                    End If

                End If

                Dim result_ As UpdateResult

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result

                Else

                    Try

                        result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result

                    Catch ex As Exception

                        Try

                            ModificarEmpresaInternacional(empresaInternacional_, crearEstructuraDomicilio_:=True)

                        Catch ex_ As Exception

                            Estado.ObjectReturned = ex_

                            Estado.SetError()

                            Return Estado

                        End Try

                    End Try

                End If

                With Estado

                    If result_ IsNot Nothing Then

                        If result_.ModifiedCount > 0 Then

                            .SetOK()

                        ElseIf result_.UpsertedId IsNot Nothing Then

                            .SetOK()

                        ElseIf result_.MatchedCount = 1 Then

                            .SetOK()

                        Else

                            .SetOKBut(Me, "Registro no modificado")

                        End If
                    Else

                        .SetOKBut(Me, "Registro no modificado")

                    End If

                End With

            End With

        End Using

        Return Estado

    End Function

    Private Function ConsultarEmpresas(Of T)(ByRef razonSocial_ As String,
                                             ByRef limiteResultados_ As Int32) As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}


                Dim operationsdb_ = iEnlace_.GetMongoCollection(Of T)(GetType(T).Name)

                Dim filter_ = Builders(Of T).Filter.And(
                                Builders(Of T).Filter.Text(razonSocial_),
                                Builders(Of T).Filter.Eq(Of Boolean)("estado", 1))

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

    Private Function ConsultarEmpresa(Of T)(ByVal cveEmpresa_ As ObjectId) _
                                              As TagWatcher
        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of T)(GetType(T).Name)

                Dim filter_ = Builders(Of T).Filter.Eq(Of ObjectId)("_id", cveEmpresa_) _
                              And Builders(Of T).Filter.Eq(Of Boolean)("estado", 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList()

                If result_.Count > 0 Then

                    .ObjectReturned = result_.FirstOrDefault()

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

    Private Function ConsultarDomiciliosExternos(Of T)(ByVal cveEmpresa_ As ObjectId) _
        As TagWatcher

        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of T)(GetType(T).Name)

                Dim filter_ = Builders(Of T).Filter.Eq(Of ObjectId)("_id", cveEmpresa_) _
                              And Builders(Of T).Filter.Eq(Of Boolean)("estado", 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList().AsEnumerable

                Dim empresa_ = DirectCast(result_.FirstOrDefault, IEmpresa)

                For Each domicilio_ In empresa_.paisesdomicilios.
                                                Where(Function(x) x.pais = PaisEmpresa).
                                                SelectMany(Function(y) y.domicilios)

                    ListaDomicilios.Add(domicilio_)

                Next

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

            ListaDomicilios = ListaEmpresas.
                    Where(Function(t) t._id = cveEmpresa_).
                    SelectMany(Function(z) z.paisesdomicilios).
                    Where(Function(x) x.pais = pais_).
                    SelectMany(Function(x) x.domicilios).ToList()

            If ListaDomicilios.Count > 0 Then

                .ObjectReturned = ListaDomicilios

                .SetOK()

            Else

                .SetOKBut(Me, "Domicilios no disponibles")

            End If

        End With

        Return Estado

    End Function

    Private Function ConsultarDomicilioExterno(Of T)(ByVal idEmpresa_ As ObjectId,
                                                ByVal idDomicilio_ As ObjectId) _
                                                As TagWatcher
        ListaDomicilios = New List(Of Domicilio)

        With Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of T)(GetType(T).Name)

                Dim filter_ = Builders(Of T).Filter.Eq(Of ObjectId)("_id", idEmpresa_) And
                              Builders(Of T).Filter.Eq(Of Boolean)("estado", 1)

                Dim result_ = operationsDB_.Find(filter_).Limit(1).ToList().AsEnumerable

                Dim empresa_ = DirectCast(result_.FirstOrDefault, IEmpresa)

                For Each domicilio_ In empresa_.paisesdomicilios.
                                                Where(Function(x) x.pais = PaisEmpresa).
                                                SelectMany(Function(y) y.domicilios).
                                                Where(Function(x) x._iddomicilio = idDomicilio_)

                    ListaDomicilios.Add(domicilio_)

                Next

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

                ListaDomicilios = ListaEmpresas.
                                    Where(Function(t) t._id = cveEmpresa_).
                                    SelectMany(Function(z) z.paisesdomicilios).
                                    Where(Function(a) a.pais = PaisEmpresa).
                                    SelectMany(Function(b) b.domicilios).
                                    Where(Function(c) c._iddomicilio = cveDomicilio_).ToList()

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

    Private Function ArchivarEmpresas(Of T)(ByRef listIdEmpresa_ _
                                            As List(Of ObjectId)) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of T)(GetType(T).Name)

            Dim filter_ = Builders(Of T).Filter.In(Of ObjectId)("_id", listIdEmpresa_)

            Dim setStructureOfSubs_ = Builders(Of T).Update.Set(Of Boolean)("archivado", True)

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

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional)(GetType(EmpresaNacional).Name)


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

    'Private Function ConsultarDomicilioExterno(Of T)(ByVal idEmpresa_ As ObjectId,
    '                                            ByVal idDomicilio_ As ObjectId) _
    '                                            As TagWatcher
    '    ListaDomicilios = New List(Of Domicilio)

    '    With Estado

    '        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}

    '            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of T)(GetType(T).Name)

    ''CUANDO VA POR TODOS LOS DOMICILIOS
    'Dim result_ = operationsDB_.Aggregate().
    '                            Match(filter_).
    '                            Unwind("paisesdomicilios").
    '                            Match(New BsonDocument("paisesdomicilios.pais", PaisEmpresa)).
    '                            Project(New BsonDocument("_id", 0)).
    '                            Project(New BsonDocument("domicilio", "$paisesdomicilios.domicilios")).ToList()

    'CUANDO VA POR UN SOLO DOMICILIO
    '            Dim result_ = operationsDB_.Aggregate().
    '                                        Match(New BsonDocument("_id", idEmpresa_)).
    '                                        Unwind("paisesdomicilios").
    '                                        Unwind("paisesdomicilios.domicilios").
    '                                        Match(New BsonDocument("paisesdomicilios.domicilios._iddomicilio", idDomicilio_)).
    '                                        Project(New BsonDocument("_id", 0)).
    '                                        Project(New BsonDocument("domicilio", "$paisesdomicilios.domicilios")).ToList().AsEnumerable

    '            If result_.Count > 0 Then

    '                ListaDomicilios.Add(BsonSerializer.
    '                                    Deserialize(Of Domicilio)(result_.FirstOrDefault.Elements.ElementAt(0).Value.ToBson))

    '                .ObjectReturned = ListaDomicilios

    '                .SetOK()

    '            Else

    '                .SetOKBut(Me, "Empresa no contiene domicilios")

    '            End If

    '        End Using

    '    End With

    '    Return Estado

    'End Function


#End Region

#Region "Métodos públicos"

    Public Function EstructuraEmpresaNacional() _
                    As IEmpresaNacional _
                    Implements IControladorEmpresas.EstructuraEmpresaNacional

        '''
        '''Crea la estructura semi-llena de una empresa NACIONAL
        '''

        Dim empresaNacional_ = New EmpresaNacional

        With empresaNacional_

            ._id = ObjectId.GenerateNewId
            ._idempresa = GenerarSecuencia().sec
            ._idempresakb = Nothing
            .razonsocial = Nothing
            .razonsocialcorto = Nothing
            .nombrecomercial = Nothing
            .abreviatura = Nothing
            .paisesdomicilios = New List(Of PaisDomicilio) _
                                From {New PaisDomicilio _
                                With {
                                        .idpais = _paisInterno._id,
                                        .sec = 1,
                                        .domicilios = New List(Of Domicilio),
                                        .pais = _paisInterno.cveISO3,
                                        .paisPresentacion = _paisInterno.nombrepaisesp,
                                        .estado = 1,
                                        .archivado = False
                                }}
            .rfc = Nothing
            ._idrfc = Nothing
            .rfcs = Nothing
            .curp = Nothing
            ._idcurp = Nothing
            .curps = Nothing
            .contactos = Nothing
            .girocomercial = Nothing
            .regimenesfiscales = Nothing
            .tipopersona = Nothing
            ._idgrupocomercial = Nothing
            .rfcNuevo = Nothing
            .curpNuevo = Nothing
            .estado = 1
            .estatus = 1
            .abierto = True
            .archivado = False

        End With

        Return empresaNacional_

    End Function

    Public Function EstructuraEmpresaNacional(ByVal razonSocial_ As String,
                                              ByVal rfc_ As String,
                                              Optional ByVal tipoPersona_ As IEmpresaNacional.TiposPersona = IEmpresaNacional.TiposPersona.Moral,
                                              Optional ByVal curp_ As String = Nothing) _
                                              As IEmpresaNacional _
                                              Implements IControladorEmpresas.EstructuraEmpresaNacional

        '''
        '''Crea la estructura semi-llena de una empresa NACIONAL
        '''

        Dim empresaNacional_ = New EmpresaNacional

        With empresaNacional_

            ._id = ObjectId.GenerateNewId
            ._idempresa = GenerarSecuencia().sec
            ._idempresakb = Nothing
            .razonsocial = razonSocial_
            .razonsocialcorto = Nothing
            .nombrecomercial = Nothing
            .abreviatura = Nothing
            .paisesdomicilios = New List(Of PaisDomicilio) _
                                From {New PaisDomicilio _
                                With {
                                        .idpais = _paisInterno._id,
                                        .sec = 1,
                                        .domicilios = New List(Of Domicilio),
                                        .pais = _paisInterno.cveISO3,
                                        .paisPresentacion = _paisInterno.nombrepaisesp,
                                        .estado = 1,
                                        .archivado = False
                                }}
            .rfc = rfc_
            ._idrfc = ObjectId.GenerateNewId
            .rfcs = New List(Of Rfc)
            .rfcNuevo = True
            .curp = IIf(curp_ IsNot Nothing, curp_, Nothing)
            ._idcurp = IIf(curp_ IsNot Nothing, ObjectId.GenerateNewId, Nothing)
            .curpNuevo = IIf(curp_ IsNot Nothing, True, Nothing)
            .curps = Nothing
            .contactos = Nothing
            .girocomercial = Nothing
            .regimenesfiscales = Nothing
            .tipopersona = tipoPersona_
            ._idgrupocomercial = Nothing
            .estado = 1
            .estatus = 1
            .abierto = True
            .archivado = False

        End With

        Dim rfcAux_ = New Rfc _
                      With {
                            .idrfc = empresaNacional_._idrfc,
                            .sec = 1,
                            .rfc = empresaNacional_.rfc,
                            .estado = 1,
                            .archivado = False
                    }

        empresaNacional_.rfcs.Add(rfcAux_)


        If tipoPersona_ = IEmpresaNacional.TiposPersona.Fisica Then

            empresaNacional_.curps = New List(Of Curp)

            Dim curpAux_ = Nothing

            If curp_ IsNot Nothing Then

                curpAux_ = New Curp _
                       With {
                        .idcurp = empresaNacional_._idcurp,
                        .sec = 1,
                        .curp = empresaNacional_.curp,
                        .estado = 1,
                        .archivado = False
                }

                empresaNacional_.curpNuevo = True

                empresaNacional_.curps.Add(curpAux_)

            End If

        End If

        Return empresaNacional_

    End Function

    Public Function EstructuraEmpresaNacional(ByVal razonSocial_ As String,
                                              ByVal rfc_ As String,
                                              ByVal domicilio_ As Domicilio,
                                              Optional ByVal tipoPersona_ As IEmpresaNacional.TiposPersona = IEmpresaNacional.TiposPersona.Moral,
                                              Optional ByVal curp_ As String = Nothing) _
                                              As IEmpresaNacional _
                                              Implements IControladorEmpresas.EstructuraEmpresaNacional

        '''
        '''Crea la estructura semi-llena de una empresa NACIONAL
        '''

        Dim empresaNacional_ = New EmpresaNacional

        With empresaNacional_

            ._id = ObjectId.GenerateNewId
            ._idempresa = GenerarSecuencia().sec
            ._idempresakb = Nothing
            .razonsocial = razonSocial_
            .razonsocialcorto = Nothing
            .nombrecomercial = Nothing
            .abreviatura = Nothing
            .paisesdomicilios = New List(Of PaisDomicilio) _
                                From {New PaisDomicilio _
                                With {
                                        .idpais = _paisInterno._id,
                                        .sec = 1,
                                        .domicilios = New List(Of Domicilio) _
                                                      From {New Domicilio _
                                                      With {
                                                            ._iddomicilio = domicilio_._iddomicilio,
                                                            .sec = 1,
                                                            .calle = domicilio_.calle,
                                                            .ciudad = domicilio_.ciudad,
                                                            .colonia = domicilio_.colonia,
                                                            .numeroexterior = domicilio_.numeroexterior,
                                                            .numerointerior = domicilio_.numerointerior,
                                                            .codigopostal = domicilio_.codigopostal,
                                                            .cveEntidadfederativa = domicilio_.cveEntidadfederativa,
                                                            .entidadfederativa = domicilio_.entidadfederativa,
                                                            .localidad = domicilio_.localidad,
                                                            .cveMunicipio = domicilio_.cveMunicipio,
                                                            .municipio = domicilio_.municipio,
                                                            .domicilioPresentacion = domicilio_.domicilioPresentacion,
                                                            .estado = 1,
                                                            .archivado = False
                                                      }},
                                        .pais = _paisInterno.cveISO3,
                                        .paisPresentacion = _paisInterno.nombrepaisesp,
                                        .estado = 1,
                                        .archivado = False
                                }}
            .rfc = rfc_
            ._idrfc = ObjectId.GenerateNewId
            .rfcs = New List(Of Rfc)
            .rfcNuevo = True
            .curp = IIf(curp_ IsNot Nothing, curp_, Nothing)
            ._idcurp = IIf(curp_ IsNot Nothing, ObjectId.GenerateNewId, Nothing)
            .curpNuevo = IIf(curp_ IsNot Nothing, True, Nothing)
            .curps = Nothing
            .contactos = Nothing
            .girocomercial = Nothing
            .regimenesfiscales = Nothing
            .tipopersona = tipoPersona_
            ._idgrupocomercial = Nothing
            .estado = 1
            .estatus = 1
            .abierto = True
            .archivado = False

        End With

        Dim rfcAux_ = New Rfc _
                      With {
                            .idrfc = empresaNacional_._idrfc,
                            .sec = 1,
                            .rfc = empresaNacional_.rfc,
                            .estado = 1,
                            .archivado = False
                    }

        empresaNacional_.rfcs.Add(rfcAux_)


        If tipoPersona_ = IEmpresaNacional.TiposPersona.Fisica Then

            empresaNacional_.curps = New List(Of Curp)

            Dim curpAux_ = Nothing

            If curp_ IsNot Nothing Then

                curpAux_ = New Curp _
                       With {
                        .idcurp = empresaNacional_._idcurp,
                        .sec = 1,
                        .curp = empresaNacional_.curp,
                        .estado = 1,
                        .archivado = False
                }

                empresaNacional_.curpNuevo = True

                empresaNacional_.curps.Add(curpAux_)

            End If

        End If

        Return empresaNacional_

    End Function

    Public Function EstructuraEmpresaInternacional() _
        As IEmpresaInternacional _
        Implements IControladorEmpresas.EstructuraEmpresaInternacional

        '''
        '''Crea la estructura semi-llena de una empresa INTERNACIONAL
        '''

        Dim empresaInternacional_ = New EmpresaInternacional

        _paisInterno = New Pais

        _paisInterno = GenerarPais()

        With empresaInternacional_

            ._id = ObjectId.GenerateNewId
            ._idempresa = GenerarSecuencia(2).sec
            ._idempresakb = Nothing
            .razonsocial = Nothing
            .razonsocialcorto = Nothing
            .nombrecomercial = Nothing
            .abreviatura = Nothing
            .paisesdomicilios = New List(Of PaisDomicilio) _
                                From {New PaisDomicilio _
                                With {
                                        .idpais = _paisInterno._id,
                                        .sec = 1,
                                        .domicilios = New List(Of Domicilio),
                                        .pais = _paisInterno.cveISO3,
                                        .paisPresentacion = _paisInterno.nombrepaisesp,
                                        .estado = 1,
                                        .archivado = False
                                }}
            .contactos = Nothing
            .girocomercial = Nothing
            .bu = Nothing
            .bus = Nothing
            ._idbu = Nothing
            .taxids = New List(Of TaxId)
            ._idgrupocomercial = Nothing
            .estado = 1
            .estatus = 1
            .abierto = True
            .archivado = False

        End With

        Return empresaInternacional_

    End Function

    Public Function EstructuraEmpresaInternacional(ByVal razonSocial_ As String,
                                                   ByVal domicilio_ As Domicilio,
                                                   ByVal taxid_ As String) _
                                                   As IEmpresaInternacional _
                                                   Implements IControladorEmpresas.EstructuraEmpresaInternacional

        '''
        '''Crea la estructura SEMI-llena de una empresa INTERNACIONAL
        '''

        Dim empresaInternacional_ = New EmpresaInternacional

        _paisInterno = New Pais

        _paisInterno = GenerarPais()

        With empresaInternacional_

            ._id = ObjectId.GenerateNewId
            ._idempresa = GenerarSecuencia(2).sec
            ._idempresakb = Nothing
            .razonsocial = razonSocial_
            .razonsocialcorto = Nothing
            .nombrecomercial = Nothing
            .abreviatura = Nothing
            .paisesdomicilios = New List(Of PaisDomicilio) _
                                From {New PaisDomicilio _
                                With {
                                        .idpais = _paisInterno._id,
                                        .sec = 1,
                                        .domicilios = New List(Of Domicilio) _
                                                      From {New Domicilio _
                                                      With {
                                                            ._iddomicilio = domicilio_._iddomicilio,
                                                            .sec = 1,
                                                            .calle = domicilio_.calle,
                                                            .ciudad = domicilio_.ciudad,
                                                            .colonia = domicilio_.colonia,
                                                            .numeroexterior = domicilio_.numeroexterior,
                                                            .numerointerior = domicilio_.numerointerior,
                                                            .codigopostal = domicilio_.codigopostal,
                                                            .cveEntidadfederativa = domicilio_.cveEntidadfederativa,
                                                            .entidadfederativa = domicilio_.entidadfederativa,
                                                            .localidad = domicilio_.localidad,
                                                            .cveMunicipio = domicilio_.cveMunicipio,
                                                            .municipio = domicilio_.municipio,
                                                            .domicilioPresentacion = domicilio_.domicilioPresentacion,
                                                            .estado = 1,
                                                            .archivado = False
                                                      }},
                                        .pais = _paisInterno.cveISO3,
                                        .paisPresentacion = _paisInterno.nombrepaisesp,
                                        .estado = 1,
                                        .archivado = False
                                }}
            .contactos = Nothing
            .girocomercial = Nothing
            .bu = Nothing
            .bus = Nothing
            ._idbu = Nothing
            .taxids = New List(Of TaxId) _
                      From {New TaxId _
                      With {.idtaxid = ObjectId.GenerateNewId,
                            .sec = 1,
                            .taxid = taxid_,
                            .estado = 1,
                            .archivado = False
            }}
            ._idgrupocomercial = Nothing
            .estado = 1
            .estatus = 1
            .abierto = True
            .archivado = False

        End With

        Return empresaInternacional_

    End Function

    Public Function EstructuraEmpresaInternacional(ByVal razonSocial_ As String) _
                                                   As IEmpresaInternacional _
                                                   Implements IControladorEmpresas.EstructuraEmpresaInternacional

        '''
        '''Crea la estructura SEMI-llena de una empresa INTERNACIONAL
        '''

        Dim empresaInternacional_ = New EmpresaInternacional

        _paisInterno = New Pais

        _paisInterno = GenerarPais()

        With empresaInternacional_

            ._id = ObjectId.GenerateNewId
            ._idempresa = GenerarSecuencia(2).sec
            ._idempresakb = Nothing
            .razonsocial = razonSocial_
            .razonsocialcorto = Nothing
            .nombrecomercial = Nothing
            .abreviatura = Nothing
            .paisesdomicilios = New List(Of PaisDomicilio) _
                                From {New PaisDomicilio _
                                With {
                                        .idpais = _paisInterno._id,
                                        .sec = 1,
                                        .domicilios = New List(Of Domicilio),
                                        .pais = _paisInterno.cveISO3,
                                        .paisPresentacion = _paisInterno.nombrepaisesp,
                                        .estado = 1,
                                        .archivado = False
                                }}
            .contactos = Nothing
            .girocomercial = Nothing
            .bu = Nothing
            .bus = Nothing
            ._idbu = Nothing
            .taxids = New List(Of TaxId)
            ._idgrupocomercial = Nothing
            .estado = 1
            .estatus = 1
            .abierto = True
            .archivado = False

        End With

        Return empresaInternacional_

    End Function


    Public Function Agregar(empresa_ As IEmpresaNacional,
                            Optional objetoRetorno_ As Boolean = False,
                            Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                            Implements IControladorEmpresas.Agregar
        '''
        '''Crea un DOCUMENTO de una EMPRESA NACIONAL, puede retornar el DOCUMENTO dentro del TAGWATCHER
        '''al colocar el valor de True en el paramétro del objetoRetorno_
        '''

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
        '''
        '''Crea un DOCUMENTO de una EMPRESA INTERNACIONAL, puede retornar el DOCUMENTO dentro del TAGWATCHER 
        '''al colocar el valor de True en el paramétro del objetoRetorno_
        '''

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

        '''
        '''Modifica un DOCUMENTO de una EMPRESA INTERNACIONAL_
        '''

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

        '''
        '''Modifica un DOCUMENTO de una EMPRESA INTERNACIONAL_
        '''

        With Estado

            If empresa_ IsNot Nothing Then

                If PaisEmpresa <> "" Then

                    If PaisEmpresa.Length = 3 Then

                        ModificarEmpresaInternacional(empresa_, session_)

                    Else

                        .SetError(Me, "Debe definir un pais con clave ISO 3")

                    End If

                Else

                    .SetError(Me, "Debe definir un pais con clave ISO 3")

                End If

            Else

                .SetError(Me, "No existe instancia de empresa internacional")

            End If

        End With

        Return Estado

    End Function

    Public Function Consultar(razonSocial_ As String,
                              Optional ByVal limiteResultados_ As Int32 = 10) As TagWatcher _
                              Implements IControladorEmpresas.Consultar
        '''
        '''Obtiene una LISTA DE DOCUMENTOS DE EMPRESAS por medio de su RAZÓN SOCIAL dentro del TAGWATCHER
        '''

        With Estado

            If razonSocial_ IsNot Nothing Then

                If limiteResultados_ <= 200 Then

                    Select Case TipoEmpresa

                        Case IControladorEmpresas.TiposEmpresas.Nacional

                            ConsultarEmpresas(Of EmpresaNacional)(razonSocial_, limiteResultados_)

                        Case IControladorEmpresas.TiposEmpresas.Internacional

                            ConsultarEmpresas(Of EmpresaInternacional)(razonSocial_, limiteResultados_)

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
        '''
        '''Obtiene el DOCUMENTO de la EMPRESA solicitada por medio de su OBJECT ID dentro del TAGWATCHER
        '''Si utiliza el modo intrinseco, debe llenar la propiedad ListaEmpresas
        '''Con al menos una empresa
        '''

        With Estado

            If Not cveEmpresa_ = ObjectId.Empty Then

                Select Case Modalidad

                    Case IControladorEmpresas.Modalidades.Extrinseca

                        If TipoEmpresa = IControladorEmpresas.TiposEmpresas.Nacional Then

                            ConsultarEmpresa(Of EmpresaNacional)(cveEmpresa_)

                        Else

                            ConsultarEmpresa(Of EmpresaInternacional)(cveEmpresa_)

                        End If

                    Case IControladorEmpresas.Modalidades.Intrinseca

                        If ListaEmpresas.Count > 0 Then

                            ConsultarEmpresaInterna(cveEmpresa_)

                        Else

                            .SetError(Me, "Lista de empresas vacía")

                        End If

                End Select

            Else

                .SetError(Me, "No se recibió una clave de empresa")

            End If

        End With

        Return Estado

    End Function

    Public Function ConsultarDomicilios(ByVal idEmpresa_ As ObjectId) As TagWatcher _
                                     Implements IControladorEmpresas.ConsultarDomicilios


        '''
        '''Obtiene los domicilios de una Empresa
        '''Si utiliza el modo intrínseco
        '''Debe llenar la propiedad ListaEmpresa con al menos una empresa
        '''

        With Estado

            If Not idEmpresa_ = ObjectId.Empty Then

                Select Case Modalidad

                    Case IControladorEmpresas.Modalidades.Extrinseca

                        If TipoEmpresa = IControladorEmpresas.TiposEmpresas.Nacional Then

                            ConsultarDomiciliosExternos(Of EmpresaNacional)(idEmpresa_)

                        Else

                            ConsultarDomiciliosExternos(Of EmpresaInternacional)(idEmpresa_)

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

    Public Function ConsultarDomicilio(idEmpresa_ As ObjectId,
                                       idDomicilio_ As ObjectId) _
                                       As TagWatcher _
                                       Implements IControladorEmpresas.ConsultarDomicilio

        '''
        '''Obtiene el domicilio de una Empresa
        '''En modo intrinseco, utilizar la propiedad de ListaEmpresas
        '''Para que el controlador pueda trabajar
        '''

        With Estado

            If Not idEmpresa_ = ObjectId.Empty Then

                If Not idDomicilio_ = ObjectId.Empty Then

                    Select Case Modalidad

                        Case IControladorEmpresas.Modalidades.Extrinseca

                            If TipoEmpresa = IControladorEmpresas.TiposEmpresas.Nacional Then

                                ConsultarDomicilioExterno(Of EmpresaNacional)(idEmpresa_, idDomicilio_)

                            Else

                                ConsultarDomicilioExterno(Of EmpresaInternacional)(idEmpresa_, idDomicilio_)

                            End If

                        Case IControladorEmpresas.Modalidades.Intrinseca

                            If ListaEmpresas.Count > 0 Then

                                ConsultarDomicilioInterno(idEmpresa_, idDomicilio_)

                            Else

                                .SetError(Me, "Lista de empresas vacías")

                            End If

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

    Function Archivar(ByVal listIdEmpresas_ As List(Of ObjectId)) _
                      As TagWatcher _
                      Implements IControladorEmpresas.Archivar

        '''
        ''' Archiva una empresa o lista de empresas, Nacionales y/o Internacionales
        '''

        With Estado

            If listIdEmpresas_.Count > 0 Then

                Select Case TipoEmpresa

                    Case IControladorEmpresas.TiposEmpresas.Nacional

                        ArchivarEmpresas(Of EmpresaNacional)(listIdEmpresas_)

                    Case IControladorEmpresas.TiposEmpresas.Internacional

                        ArchivarEmpresas(Of EmpresaInternacional)(listIdEmpresas_)

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
        'NO IMPLEMENTAR. NO FUNCIONAL'
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

                _controladorSecuencias = Nothing

                _controladorPaises = Nothing

                _secuencia = Nothing

                _paisInterno = Nothing

                TipoEmpresa = Nothing

                ListaDomicilios = Nothing

                Estado = Nothing

                Modalidad = Nothing

                PaisEmpresa = Nothing

                ListaEmpresas = Nothing

                Empresa = Nothing

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