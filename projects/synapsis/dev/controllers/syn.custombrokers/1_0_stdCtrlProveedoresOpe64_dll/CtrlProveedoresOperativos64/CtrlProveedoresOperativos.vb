Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Utils
Imports Wma.Exceptions

Public Class CtrlProveedoresOperativos

#Region "Enums"

    Public Enum TipoOperacion

        Importacion = 1
        Exportacion = 2

    End Enum

    Public Enum TipoSelectOption

        IdRazonsocial = 1
        IdIdentificador = 2
        CveRazonsocial = 3
        CveIdentificador = 4

    End Enum

#End Region

#Region "Propiedades"

    Property _tipoOperacion As TipoOperacion

#End Region

#Region "Constructores"

    Sub New()

    End Sub

#End Region

#Region "Funciones"

    Public Function ListarProveedores() As List(Of ConstructorProveedoresOperativos)
        'No funcional hasta que se utilice
        Dim provedororOperativo_ As New ConstructorProveedoresOperativos()

        Dim listaProveedores_ As New List(Of ConstructorProveedoresOperativos)

        Select Case _tipoOperacion

            Case TipoOperacion.Exportacion

                Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                    Using _entidadDatos As IEntidadDatos = provedororOperativo_

                        Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                        Return listaProveedores_

                    End Using

                End Using

            Case TipoOperacion.Importacion

        End Select

        Return listaProveedores_

    End Function

    Public Function BuscarProveedores(ByVal razonSocial_ As String,
                                      Optional ByVal esDestinatario_ As Boolean = True) As Object
        'Solo esta funcional en tipo exportación
        Dim provedororOperativo_ As New ConstructorProveedoresOperativos()

        Dim tagwacher_ As New TagWatcher

        Select Case _tipoOperacion

            Case TipoOperacion.Exportacion

                Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                    {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                    Using _entidadDatos As IEntidadDatos = provedororOperativo_

                        Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                        tagwacher_ = _enlaceDatos.BusquedaGeneralDocumento(documentoElectronico_,
                                                                            1,
                                                                            5003,
                                                                            razonSocial_)

                        Return tagwacher_.ObjectReturned

                    End Using

                End Using

            Case TipoOperacion.Importacion

        End Select

        Return tagwacher_

    End Function

    Public Function BuscarProveedor(Optional ByVal objectId_ As ObjectId = Nothing,
                                    Optional ByVal listaObjectId_ As List(Of ObjectId) = Nothing) As Object

        'Solo funciona en exportación y cuando traes un Object Id
        Dim provedororOperativo_ As New ConstructorProveedoresOperativos()

        Dim tipo_ As String = GetType(ConstructorProveedoresOperativos).Name

        Select Case _tipoOperacion

            Case TipoOperacion.Exportacion

                If objectId_ <> Nothing Then

                    Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
                        {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                        Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(tipo_)

                        Dim resultadoDocumentos_ As New List(Of OperacionGenerica)

                        Dim filtro_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, objectId_)

                        resultadoDocumentos_ = operacionesDB_.Find(filtro_).Limit(1).ToList

                        If resultadoDocumentos_.Count Then

                            Dim operacionGenerica_ As OperacionGenerica = resultadoDocumentos_(0)

                            Return New TagWatcher(1) With {.ObjectReturned = operacionGenerica_}

                        Else

                            Return New TagWatcher(0, Me, "No se encontró ningún valor para esta consulta")

                        End If

                    End Using

                Else


                End If


            Case TipoOperacion.Importacion

        End Select

        Return New TagWatcher(0, Me, "Sin resultados")

    End Function

    Public Function BuscarDomicilios(ByVal proveedores_ As DocumentoElectronico) As Object

        'Regresa una lista de select option se debe ajustar a que se devuelva una lista de domicilios
        'usando la clase de empresa-domicilios y que se use linq para la lectura.
        Dim listaDomicilios_ As New List(Of SelectOption)

        If proveedores_ IsNot Nothing Then

            If proveedores_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas > 0 Then

                For indice_ As Int32 = 1 To proveedores_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

                    With proveedores_.Seccion(SeccionesProvedorOperativo.SPRO2).Partida(indice_)

                        If .estado = 1 Then

                            Dim domicilio_ = .Attribute(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor & " | " &
                                .Attribute(CamposDomicilio.CA_CALLE).Valor & " #" &
                                .Attribute(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor & " C.P. " &
                                .Attribute(CamposDomicilio.CA_CALLE).Valor & " COL." &
                                .Attribute(CamposDomicilio.CA_COLONIA).Valor & ", " &
                                .Attribute(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor & ", " &
                                .Attribute(CamposDomicilio.CA_PAIS).Valor

                            Dim id_ = .Attribute(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                            Dim dataSourceItem_ = New SelectOption With {
                                .Text = domicilio_,
                                .Value = id_
                            }

                            listaDomicilios_.Add(dataSourceItem_)

                        End If

                    End With

                Next

                Return listaDomicilios_

            End If

        End If

        Return listaDomicilios_

    End Function

    Public Function BuscarVinculaciones(ByVal proveedores_ As DocumentoElectronico,
                                        ByVal idProveedor_ As String, ByVal idCliente_ As ObjectId) As Object

        'Solo esta para exportacion, se debe ajustar a que mande una lista de vinculaciones o un objeto diferente
        'al select option
        Dim listaVinculacion_ As New List(Of SelectOption)

        Select Case _tipoOperacion

            Case TipoOperacion.Exportacion

                If proveedores_ IsNot Nothing Then

                    If proveedores_.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas > 0 Then

                        For indice_ As Int32 = 1 To proveedores_.Seccion(SeccionesProvedorOperativo.SPRO4).CantidadPartidas

                            With proveedores_.Seccion(SeccionesProvedorOperativo.SPRO4).Partida(indice_)

                                If .estado = 1 And .Attribute(CamposProveedorOperativo.CP_ID_CLIENTE_VINCULACION).Valor =
                                    idCliente_ And .Attribute(CamposProveedorOperativo.CP_RFC_PROVEEDOR_VINCULACION).ValorPresentacion =
                                    idProveedor_ Then

                                    Dim vinculacion_ = .Attribute(CamposProveedorOperativo.CA_CVE_VINCULACION).ValorPresentacion

                                    Dim cve_ = .Attribute(CamposProveedorOperativo.CA_CVE_VINCULACION).Valor

                                    Dim dataSourceItem_ = New SelectOption With {
                                        .Text = vinculacion_,
                                        .Value = cve_
                                    }

                                    listaVinculacion_.Add(dataSourceItem_)

                                End If

                            End With

                        Next

                        Return listaVinculacion_

                    End If

                End If

            Case TipoOperacion.Importacion

        End Select

        Return listaVinculacion_

    End Function

    Public Function BuscarConfiguraciones(ByVal proveedores_ As DocumentoElectronico,
                                          ByVal idProveedor_ As String, ByVal idCliente_ As ObjectId) As Object
        'Solo esta para exportacion, se debe ajustar a que mande una lista de configuraciones o un objeto
        'diferente al select option
        Dim listaConfiguracion_ As New List(Of SelectOption)

        Select Case _tipoOperacion

            Case TipoOperacion.Exportacion

                If proveedores_ IsNot Nothing Then

                    If proveedores_.Seccion(SeccionesProvedorOperativo.SPRO5).CantidadPartidas > 0 Then

                        For indice_ As Int32 = 1 To proveedores_.Seccion(SeccionesProvedorOperativo.SPRO5).CantidadPartidas

                            With proveedores_.Seccion(SeccionesProvedorOperativo.SPRO5).Partida(indice_)

                                If .estado = 1 And .Attribute(CamposProveedorOperativo.CP_ID_CLIENTE_CONFIGURACION).Valor =
                                    idCliente_ And .Attribute(CamposProveedorOperativo.CP_RFC_PROVEEDOR_CONFIGURACION).ValorPresentacion =
                                    idProveedor_ Then

                                    Dim metodoValoracion_ = .Attribute(CamposProveedorOperativo.CA_CVE_METODO_VALORACION).ValorPresentacion

                                    Dim cve_ = .Attribute(CamposProveedorOperativo.CA_CVE_METODO_VALORACION).Valor

                                    Dim dataSourceItem_ = New SelectOption With {
                                        .Text = metodoValoracion_,
                                        .Value = cve_
                                    }

                                    listaConfiguracion_.Add(dataSourceItem_)

                                End If

                            End With

                        Next

                        Return listaConfiguracion_

                    End If

                End If

            Case TipoOperacion.Importacion

        End Select

        Return listaConfiguracion_

    End Function

    Public Function ToSelectOption(Optional ByVal listaProveedores_ As Object = Nothing,
                                   Optional ByVal proveedorDocumento_ As DocumentoElectronico = Nothing,
                                   Optional ByVal tipoSelect_ As TipoSelectOption = TipoSelectOption.IdRazonsocial) As List(Of SelectOption)
        'Completar las funciones y considerar que puede ser adaptado en el controlador backend
        Dim listaProveedoresComponente_ As New List(Of SelectOption)

        If listaProveedores_ IsNot Nothing Then

            If listaProveedores_.Count() > 0 Then

                For Each item_ As Dictionary(Of Object, Object) In listaProveedores_

                    Select Case tipoSelect_

                        Case TipoSelectOption.IdRazonsocial

                            listaProveedoresComponente_.Add(New SelectOption With {.Value = item_.Item("ID").ToString, .Text = item_.Item("valorOperacion") & " - " & item_.Item("folioOperacion")})

                        Case TipoSelectOption.IdIdentificador

                        Case TipoSelectOption.CveRazonsocial

                        Case TipoSelectOption.CveIdentificador

                    End Select

                Next

            End If

        ElseIf proveedorDocumento_ IsNot Nothing Then


        End If

        Return listaProveedoresComponente_

    End Function

    Public Function BuscarProveedores1(ByVal token_ As String, btipooperacion_ As Boolean) As List(Of SelectOption)
        Dim listaProveedores_ As New List(Of SelectOption)
        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                   {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim CtrlRecursosGenerales_ As New Organismo
            Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorProveedoresOperativos).Name)

            Dim itipooperacion_ As Integer
            If btipooperacion_ Then
                itipooperacion_ = 1
            Else
                itipooperacion_ = 2
            End If

            operationsDB_.Aggregate().Project(Function(ch) New With {
                                          Key .IDS = ch.Id,
                                          Key .razonsocial = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente,
                                          Key .foliodocumentos = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento,
                                          Key .tipouso = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(4).Nodos(0), Campo).Valor
              }).Match(BsonDocument.Parse(CtrlRecursosGenerales_.SeparacionPalabras(token_, "razonsocial", "tipouso", itipooperacion_.ToString, ""))).
                ToList().AsEnumerable.ToList().ForEach(Sub(estatus_)
                                                           listaProveedores_.Add(New SelectOption With {
                                                                          .Value = estatus_.IDS.ToString,
                                                                          .Text = estatus_.razonsocial & " | " & estatus_.foliodocumentos
                                                           })
                                                       End Sub)

        End Using
        Return listaProveedores_


    End Function

    Public Function BuscarProveedor(ByVal token_ As String, ByRef stipoidentificador_ As String) As ConstructorProveedoresOperativos
        Dim ConstructorProveedoresOperativos_ As New ConstructorProveedoresOperativos
        'MsgBox(token_ & ":::" & tipopersona_ & ":::" & stipoidentificador_)
        'Dim documentopersonas_ As New DocumentoPersona
        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
               {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}
            Dim operationsDB_ As IMongoCollection(Of OperacionGenerica)
            operationsDB_ = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorProveedoresOperativos).Name)
            If stipoidentificador_ = "ID" Then
                operationsDB_.Aggregate().Project(Function(ch) New With {
                                      Key .IDS = ch.Id,
                                      Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                }).Match(Function(e) e.IDS = New ObjectId(token_)).
                                 ToList().ForEach(Sub(estatus_)
                                                      ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                                                  End Sub)
            Else

                If stipoidentificador_ = "TAXID" Then
                    operationsDB_.Aggregate().Project(Function(ch) New With {
                                      Key .IDS = ch.Id,
                                      Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                      Key .TaxId = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0).Nodos(0).Nodos(2).Nodos(0), Campo).Valor
          }).Match(Function(e) e.TaxId.Equals(token_)).
            ToList().ForEach(Sub(estatus_)
                                 ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                             End Sub)
                Else
                    If stipoidentificador_ = "RFC" Then

                        operationsDB_.Aggregate().Project(Function(ch) New With {
                                          Key .IDS = ch.Id,
                                          Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                          Key .RFC = DirectCast(ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(0).Nodos(0).Nodos(0).Nodos(1).Nodos(0), Campo).Valor
                                          }).Match(Function(e) e.RFC.Equals(token_)).
                ToList().ForEach(Sub(estatus_)
                                     ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                                 End Sub)
                    Else

                        operationsDB_.Aggregate().Project(Function(ch) New With {
                                          Key .IDS = ch.Id,
                                          Key .DocumentoProveedor = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                          Key .RazonSocial = ch.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombreCliente
              }).Match(Function(e) e.RazonSocial.Equals(token_)).
                ToList().ForEach(Sub(estatus_)
                                     ConstructorProveedoresOperativos_ = New ConstructorProveedoresOperativos(True, estatus_.DocumentoProveedor) _
                                                                                              With {.Id = estatus_.IDS.ToString}
                                 End Sub)
                    End If

                End If

            End If

        End Using
        Return ConstructorProveedoresOperativos_



    End Function

#End Region

End Class