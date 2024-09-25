Imports System
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports gsol
Imports gsol.krom.EntidadDatos
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Empresa
Imports Rec.Globals.Utils
Imports Syn.Utils
Imports Wma.Exceptions
Imports Syn.Documento.OperacionGenerica






Public Class ControladorEmpresas
    ''' <summary>
    ''' CONTROLADOR DE EMPRESAS V.1.0.0 
    ''' DESCONTINUADO
    ''' UTILIZAR CONTROLADOR DE EMPRESAS V.2.0.0
    ''' </summary>
    ''' <returns></returns>

#Region "Propiedades"

    Property t_CURP As String

    Property t_Calle As String

    Property t_NumeroExt As String

    Property t_NumeroInt As String

    Property t_Colonia As String

    Property t_Ciudad As String

    Property t_Estado As String

    Property t_Pais As String

    Property i_Cve_Empresa As String

    Property t_RFC As String

    Property t_localidad As String

    Property t_municipio As String

    Property t_entidadfederativa As String

    Property s_tipoPersona As Boolean

    Property s_Extranjero As Boolean

    Property esNuevoDomicilio As Boolean

    Property t_CodigoPostal As String

#End Region

    Public Function NuevaEmpresa(ByVal empresa_ As Empresa,
                                 Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Empresa)("Glo007Empresas")

        Dim tagwatcher_ As New TagWatcher

        'Dim curp_ As New curp With
        '                        {.curp = t_CURP,
        '                         ._idcurp = ObjectId.GenerateNewId
        '                        }

        'Dim domicilio_ As New domicilio With
        '                        {._iddomicilio = ObjectId.GenerateNewId,
        '                         .sec = 1,
        '                         .calle = t_Calle,
        '                         .numeroexterior = t_NumeroExt,
        '                         .numerointerior = t_NumeroInt,
        '                         .colonia = t_Colonia,
        '                         .ciudad = t_Ciudad,
        '                         .pais = t_Pais,
        '                         .estadorepublica = t_Estado,
        '                         .archivado = False}

        'Dim empresa_ As New Empresa(i_Cve_Empresa,
        '                    New rfc With {.rfc = t_RFC, ._idrfc = ObjectId.GenerateNewId},
        '                    IIf(s_tipoPersona, TiposPersona.Moral, TiposPersona.Fisica),
        '                    IIf(s_Extranjero, TiposEmpresa.Extranjera, TiposEmpresa.Nacional),
        '                    IIf(domicilio_ IsNot Nothing, domicilio_, Nothing),
        '                    IIf(curp_ IsNot Nothing, curp_, Nothing))

        Dim result_ = operationsDB_.InsertOneAsync(session_, empresa_).ConfigureAwait(False)

        With tagwatcher_

            .SetOK()

            .ObjectReturned = empresa_

        End With

        Return tagwatcher_

    End Function

    Public Function NuevaEmpresa(Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Empresa)("Glo007Empresas")

        Dim tagwatcher_ As New TagWatcher

        Dim curp_ As New curp With
                                {.curp = t_CURP,
                                 ._idcurp = ObjectId.GenerateNewId
                                }

        Dim domicilio_ As New domicilio With
                                {._iddomicilio = ObjectId.GenerateNewId,
                                 .sec = 1,
                                 .calle = t_Calle,
                                 .numeroexterior = t_NumeroExt,
                                 .numerointerior = t_NumeroInt,
                                 .colonia = t_Colonia,
                                 .ciudad = t_Ciudad,
                                 .localidad = t_localidad,
                                 .municipio = t_municipio,
                                 .entidadfederativa = t_entidadfederativa,
                                 .pais = t_Pais,
                                 .estadorepublica = t_Estado,
                                 .cp = t_CodigoPostal,
                                 .archivado = False}

        Dim empresa_ As New Empresa(i_Cve_Empresa,
                            New rfc With {.rfc = t_RFC, ._idrfc = ObjectId.GenerateNewId},
                            IIf(s_tipoPersona, TiposPersona.Moral, TiposPersona.Fisica),
                            IIf(s_Extranjero, TiposEmpresa.Extranjera, TiposEmpresa.Nacional),
                            IIf(domicilio_ IsNot Nothing, domicilio_, Nothing),
                            IIf(curp_ IsNot Nothing, curp_, Nothing))

        Dim result_ = operationsDB_.InsertOneAsync(session_, empresa_).ConfigureAwait(False)

        With tagwatcher_

            .SetOK()

            .ObjectReturned = empresa_

        End With

        Return tagwatcher_

    End Function

    Public Function ActualizaEmpresa(ByVal empresa_ As Empresa,
                               Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        If empresa_ IsNot Nothing Then

            With empresa_
                '._id = idEmpresa_,
                '.estado = 1
                '.abierto = True
                '._idempresa = 0
                '._idempresakb = 0
                .razonsocial = i_Cve_Empresa
                '._idrfc = t_RFC.

                If (.rfc <> t_RFC) Then

                    Dim newRFCId_ As ObjectId = ObjectId.GenerateNewId
                    .rfc = t_RFC
                    ._idrfc = newRFCId_
                    .rfcs.Add(New rfc With
                          {._idrfc = newRFCId_,
                           .rfc = t_RFC
                          })

                End If

                If (.curp <> t_CURP) Then

                    Dim newCURPId_ As ObjectId = ObjectId.GenerateNewId
                    .curp = t_CURP
                    ._idcurp = newCURPId_
                    .curps.Add(New curp With
                           {._idcurp = newCURPId_,
                            .curp = t_CURP
                           })

                End If

                .tipoempresa = IIf(s_Extranjero, TiposEmpresa.Nacional, TiposEmpresa.Extranjera)
                .tipopersona = IIf(s_tipoPersona, TiposPersona.Moral, TiposPersona.Fisica)

                If esNuevoDomicilio = True Then

                    'Validamos que los datos sean realmente distintos en el los domicilios de la empresa
                    Dim unique_ = From domicilio In .domicilios Where
                                                              domicilio.calle = t_Calle And
                                                              domicilio.numeroexterior = t_NumeroExt And
                                                              domicilio.numerointerior = t_NumeroInt And
                                                              domicilio.colonia = t_Colonia And
                                                              domicilio.ciudad = t_Ciudad And
                                                              domicilio.localidad = t_localidad And
                                                              domicilio.municipio = t_municipio And
                                                              domicilio.entidadfederativa = t_entidadfederativa And
                                                              domicilio.pais = t_Pais And
                                                              domicilio.cp = t_CodigoPostal And
                                                              domicilio.estadorepublica = t_Estado

                    If unique_.Count = 0 Then

                        Dim sec = empresa_.domicilios.Last.sec + 1

                        .domicilios.Add(
                         New domicilio With
                            {._iddomicilio = ObjectId.GenerateNewId,
                              .sec = sec,
                              .calle = t_Calle,
                              .numeroexterior = t_NumeroExt,
                              .numerointerior = t_NumeroInt,
                              .colonia = t_Colonia,
                              .ciudad = t_Ciudad,
                              .localidad = t_localidad,
                              .municipio = t_municipio,
                              .entidadfederativa = t_entidadfederativa,
                              .pais = t_Pais,
                              .estadorepublica = t_Estado,
                              .cp = t_CodigoPostal,
                              .archivado = False
                            }
                       )

                        System.Web.HttpContext.Current.Session("_secDomicilio") = sec - 1

                    Else

                        System.Web.HttpContext.Current.Session("_secDomicilio") = 0

                    End If

                End If


                '.girocomercial = "Comercializadora"
                '._idgrupocomercial = ObjectId.GenerateNewId
                '.contactos = New List(Of contacto) _
                '                  From {New contacto With
                '                            {
                '                             ._idejecutivo = ObjectId.GenerateNewId,
                '                             .nombrecompleto = "Pedro Bautista",
                '                             .archivado = False
                '                            }
                '                       }
                '.unidadesnegocios = New List(Of unidadnegocio) _
                '                         From {
                '                               New unidadnegocio With {
                '                                                       ._idunidadnegocio = ObjectId.GenerateNewId,
                '                                                       .nombreunidad = "Importaciones",
                '                                                       .archivado = False
                '                                                      }
                '                              }

                '.regimenfiscal = New List(Of regimenfiscal) _
                '                      From {New regimenfiscal With {.id = 1456,
                '                                                    .regimen = "Distribucion de bebidas y alimentos",
                '                                                    .archivado = False}
                '}


            End With

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Empresa)("Glo007Empresas")

            Dim filter_ = Builders(Of Empresa).Filter.Eq(Function(x) x._id, empresa_._id)

            Dim setStructureOfSubs_ = Builders(Of Empresa).Update.
                                 Set(Function(x) x.razonsocial, empresa_.razonsocial).
                                 Set(Function(x) x.rfc, empresa_.rfc).
                                 Set(Function(x) x.curp, empresa_.curp).
                                 Set(Function(x) x.tipopersona, empresa_.tipopersona).
                                 Set(Function(x) x.tipoempresa, empresa_.tipoempresa).
                                 AddToSet("rfcs", New rfc With {.rfc = empresa_.rfc, ._idrfc = empresa_._idrfc}).
                                 AddToSet("curps", New curp With {.curp = empresa_.curp, ._idcurp = empresa_._idcurp}).
                                 AddToSet(Of domicilio)("domicilios", empresa_.domicilios(System.Web.HttpContext.Current.Session("_secDomicilio")))

            Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_).Result

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

    Public Shared Function BuscarEmpresas(ByRef empresastemporal_ As List(Of Empresa),
                                   ByVal token_ As String) As List(Of SelectOption)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos


        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Empresa)("Glo007Empresas")

        'token_ = Chr(34) & token_ & Chr(34)

        Dim queryExpr = New BsonRegularExpression(New Regex(token_, RegexOptions.IgnoreCase))

        Dim filter_ = Builders(Of Empresa).Filter.Regex("razonsocial", queryExpr) And 'Text(token_) And
        Builders(Of Empresa).Filter.Eq(Function(x) x.estado, 1)

        Dim empresas_ As New List(Of Empresa)

        Dim listEmpresas_ As New List(Of SelectOption) 'Dictionary(Of Object, Object)

        empresas_ = operationsDB_.Find(filter_).Limit(10).ToList()

        If empresas_ IsNot Nothing And empresas_.Count > 0 Then

            For Each empresa_ In empresas_

                listEmpresas_.Add(New SelectOption With {.Value = empresa_._idempresa, .Text = empresa_.razonsocial})

            Next

        End If

        empresastemporal_ = empresas_

        Return listEmpresas_

    End Function
    Public Shared Function BuscarDomicilios(ByVal cveEmpresa_ As Int32,
                                      ByVal empresasTemporal_ As List(Of Empresa)) As List(Of SelectOption)

        Dim domicilios_ As New List(Of SelectOption) ' Dictionary(Of Object, Object) = Nothing

        Dim lista_ =
            empresasTemporal_.Where(Function(t) t._idempresa = cveEmpresa_).
                              Select(Function(t) t.domicilios)

        For Each domicilio_ In lista_

            Dim stringdomicilio_ As String

            For item_ As Int32 = 0 To domicilio_.Count - 1

                With domicilio_.Item(item_)

                    stringdomicilio_ = .calle & " " &
                                       .numeroexterior & " " &
                                       .cp & " " &
                                       .colonia & " " &
                                       .ciudad & " " &
                                       .pais

                    stringdomicilio_ = stringdomicilio_.Replace("  ", " ")

                    domicilios_.Add(
                                    New SelectOption With
                                    {.Value = domicilio_.Item(item_).sec,
                                    .Text = stringdomicilio_
                                    }
                                   )

                End With

            Next

        Next

        Return domicilios_

    End Function

    Public Shared Function BuscarDomicilio(ByVal cveEmpresa_ As Int32,
                                           ByVal cveDomicilio_ As Int32,
                                      ByVal empresasTemporal_ As List(Of Empresa)) As domicilio

        Dim lista_ =
            empresasTemporal_.Where(Function(t) t._idempresa = cveEmpresa_).
                              Select(Function(t) t.domicilios)

        Dim domicilio_ = From data In lista_(0)
                         Where data.sec = cveDomicilio_

        Return domicilio_(0)

    End Function
    Public Shared Function BuscarEmpresa(ByVal idEmpresa_ As ObjectId,
                                   ByVal idDomicilio_ As ObjectId,
                                   ByRef domicilios_ As List(Of SelectOption),
                                   ByRef domicilio_ As domicilio) As Empresa

        Dim empresa_ As Empresa = Nothing



        '-----------------Búsqueda provisional-------------------

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Empresa)("Glo007Empresas")

        Dim filter_ = Builders(Of Empresa).Filter.Eq(Function(x) x._id, idEmpresa_) ' myObjectID_)

        Dim empresas_ As New List(Of Empresa)

        empresas_ = operationsDB_.Find(filter_).ToList()

        '-----------------Búsqueda provisional-------------------

        If empresas_ IsNot Nothing And empresas_.Count > 0 Then

            If Not empresas_(0) Is Nothing Then

                empresa_ = empresas_(0)

                ' Dim domicilios_ = New List(Of SelectOption)

                If empresa_ IsNot Nothing Then
                    Dim idx_ As Int32 = 1

                    For Each domicilioItem_ As domicilio In empresa_.domicilios

                        domicilios_.Add(New SelectOption With
                                    {.Value = idx_,
                                    .Text = domicilioItem_.calle & " No." &
                                            domicilioItem_.numeroexterior & " " &
                                            domicilioItem_.cp & " " &
                                            domicilioItem_.colonia & "," &
                                            domicilioItem_.fraccionamiento & ", " &
                                            domicilioItem_.ciudad & ", " &
                                            domicilioItem_.pais})

                        If domicilioItem_._iddomicilio = idDomicilio_ Then

                            domicilio_ = domicilioItem_

                        End If

                    Next

                End If

            End If

        End If

        Return empresa_

    End Function

    Public Function BuscarClientes1(ByVal token_ As String, btipooperacion_ As Boolean) As List(Of SelectOption)

        '''
        ''' MÉTODO DESCONTINUADO, FAVOR DE UTILIZAR EL NUEVO CONTROLADOR
        '''

        Dim listaclientes_ As New List(Of SelectOption)
        'Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
        '           {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}


        '    Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorCliente).Name)

        '    Dim ctrlRecursosGenerales_ As New Organismo
        '    Dim itipooperacion_ As Integer
        '    If btipooperacion_ Then
        '        itipooperacion_ = 1
        '    Else
        '        itipooperacion_ = 2
        '    End If

        '    'Dim Algo As DocumentoElectronico



        '    operationsDB_.Aggregate().Project(Function(ch) New With {
        '                                  Key .IDS = ch.Id,
        '                                  Key .razonsocial = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente,
        '                                  Key .foliodocumentos = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento,
        '                                  Key .tipouso = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0).Nodos(0).Nodos(4).Nodos(0), Campo).Valor
        '      }).Match(BsonDocument.Parse(ctrlRecursosGenerales_.SeparacionPalabras(token_, "razonsocial", "tipouso", itipooperacion_.ToString, ""))).
        '        ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)
        '                                                   listaclientes_.Add(New SelectOption With {
        '                                                                  .Value = estatus_.IDS.ToString,
        '                                                                  .Text = estatus_.razonsocial & " | " & estatus_.foliodocumentos
        '                                                   })
        '                                               End Sub)


        'End Using
        Return listaclientes_


    End Function

    Public Function BuscarCliente(ByVal token_ As String, ByRef stipoidentificador_ As String) As ConstructorCliente

        '''
        ''' SI ESTABAS UTILIZANDO ESTE FUNCIONAMIENTO
        ''' LO DESACTIVE TEMPORALMENTE
        ''' FAVOR DE IMPLEMENTAR EL CONTROLADOR ADECUADO
        ''' PARA ESTA FUNCION
        ''' CONTROLADOR DE EMPRESAS NO SABE NADA DE CLIENTES
        ''' PERO SI DE RAZONES SOCIALES :V
        ''' '''



        Dim ConstructorCliente_ As New ConstructorCliente


        'MsgBox(token_ & ":::" & tipopersona_ & ":::" & stipoidentificador_)
        'Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
        '       {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}
        '    Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorCliente).Name)
        '    If stipoidentificador_ = "ID" Then
        '        operationsDB_.Aggregate().Project(Function(ch) New With {
        '                                  Key .IDS = ch.Id,
        '                                  Key .DocumentoCliente = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
        '      }).Match(Function(e) e.IDS = New ObjectId(token_)).
        '        ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)

        '                                                   ConstructorCliente_ = New ConstructorCliente(True, estatus_.DocumentoCliente) _
        '                                                                         With {.Id = estatus_.IDS.ToString}

        '                                               End Sub)
        '    Else
        '        If stipoidentificador_ = "TAXID" Then
        '            operationsDB_.Aggregate().Project(Function(ch) New With {
        '                                  Key .IDS = ch.Id,
        '                                  Key .DocumentoCliente = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
        '                                  Key .TaxId = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(4).Nodos(0), Campo).Valor
        '      }).Match(Function(e) e.TaxId.Equals(token_)).
        '        ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)
        '                                                   ConstructorCliente_ = New ConstructorCliente(True, estatus_.DocumentoCliente) _
        '                                                                         With {.Id = estatus_.IDS.ToString}

        '                                               End Sub)
        '        Else
        '            If stipoidentificador_ = "RFC" Then

        '                operationsDB_.Aggregate().Project(Function(ch) New With {
        '                                      Key .IDS = ch.Id,
        '                                      Key .DocumentoCliente = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
        '                                      Key .RFC = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(3).Nodos(0), Campo).Valor
        '          }).Match(Function(e) e.RFC.Equals(token_)).
        '            ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)
        '                                                       ConstructorCliente_ = New ConstructorCliente(True, estatus_.DocumentoCliente) _
        '                                                                         With {.Id = estatus_.IDS.ToString}

        '                                                   End Sub)
        '            Else
        '                operationsDB_.Aggregate().Project(Function(ch) New With {
        '                                      Key .IDS = ch.Id,
        '                                      Key .DocumentoCliente = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
        '                                      Key .RazonSocial = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor
        '          }).Match(Function(e) e.RazonSocial.Equals(token_)).
        '            ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)
        '                                                       ConstructorCliente_ = New ConstructorCliente(True, estatus_.DocumentoCliente) _
        '                                                                         With {.Id = estatus_.IDS.ToString}

        '                                                   End Sub)
        '            End If

        '        End If

        '    End If

        'End Using
        Return ConstructorCliente_



    End Function

End Class
