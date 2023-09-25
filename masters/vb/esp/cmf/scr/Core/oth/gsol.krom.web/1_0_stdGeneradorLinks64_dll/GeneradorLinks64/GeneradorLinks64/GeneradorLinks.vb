Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports Gsol.documento
Imports Gsol.seguridad
Imports System.Security.Cryptography
Imports System.IO

Namespace gsol.krom.web

    Public Class GeneradorLinks
        Implements IGeneradorLinks

#Region "Enums"

#End Region

#Region "Atributos"

        Private _host As String

        Private _tagWatchaer As TagWatcher

        Private _organismo As Organismo

        Private _espacioTrabajo As IEspacioTrabajo

        Private _iOperaciones As IOperacionesCatalogo

        Private _listaClavesDocumentos As List(Of Int64)

#End Region

#Region "Propiedades"

#End Region

#Region "Constructores"

        Sub New(ByVal iOperaciones_ As OperacionesCatalogo)

            _host = "http://web.kromaduanal.com/" ' host de producción

            '_host = "http://localhost:1755/" ' host de pruebas

            _tagWatchaer = New TagWatcher

            _organismo = New Organismo

            _espacioTrabajo = iOperaciones_.EspacioTrabajo

            _iOperaciones = iOperaciones_

            _listaClavesDocumentos = New List(Of Int64)

        End Sub

#End Region

#Region "Metodos"

        Public Function CrearLink(ByVal clave_ As Int64,
                                  ByVal tipoDato_ As IGeneradorLinks.TipoDatoClave,
                                  ByVal comportamiento_ As IGeneradorLinks.TipoComportamiento,
                                  Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                                  Optional ByVal parametros_ As String = "") As TagWatcher Implements IGeneradorLinks.CrearLink

            Dim clausula_ As String = PreparaClausulaTipoClave(clave_, tipoDato_)

            If ValidaDocumentos(clausula_).Status = TagWatcher.TypeStatus.Ok Then

                If ValidaPermisos().Status = TagWatcher.TypeStatus.Ok Then

                    If CrearGuardarLink(comportamiento_, rutaWeb_, parametros_).Status = TagWatcher.TypeStatus.Ok Then

                        _listaClavesDocumentos = New List(Of Int64)

                        Return _tagWatchaer

                    End If

                End If

            End If

            Return _tagWatchaer

        End Function

        Public Function CrearLink(ByVal nombre_ As String,
                                  ByVal tipoDato_ As IGeneradorLinks.TipoDatoNombre,
                                  ByVal comportamiento_ As IGeneradorLinks.TipoComportamiento,
                                  Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                                  Optional ByVal parametros_ As String = "") As TagWatcher Implements IGeneradorLinks.CrearLink

            Dim clausula_ As String = PreparaClausulaTipoNombre(nombre_, tipoDato_)

            If ValidaDocumentos(clausula_).Status = TagWatcher.TypeStatus.Ok Then

                If ValidaPermisos().Status = TagWatcher.TypeStatus.Ok Then

                    If CrearGuardarLink(comportamiento_, rutaWeb_, parametros_).Status = TagWatcher.TypeStatus.Ok Then

                        _listaClavesDocumentos = New List(Of Int64)

                        Return _tagWatchaer

                    End If

                End If

            End If

            Return _tagWatchaer

        End Function

        Public Function CrearLink(ByVal listaClaves_ As List(Of Int64),
                                  ByVal tipoDato_ As IGeneradorLinks.TipoDatoListaClaves,
                                  ByVal comportamiento_ As IGeneradorLinks.TipoComportamiento,
                                  Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                                  Optional ByVal parametros_ As String = "") As TagWatcher Implements IGeneradorLinks.CrearLink

            If listaClaves_.Count = 0 Then

                _tagWatchaer.SetError(TagWatcher.ErrorTypes.C6_029_0006)

                Return _tagWatchaer

            End If

            Dim clausula_ As String = PreparaClausulaTipoListaClaves(listaClaves_, tipoDato_)

            If ValidaDocumentos(clausula_).Status = TagWatcher.TypeStatus.Ok Then

                If ValidaPermisos().Status = TagWatcher.TypeStatus.Ok Then

                    If CrearGuardarLink(comportamiento_, rutaWeb_, parametros_).Status = TagWatcher.TypeStatus.Ok Then

                        _listaClavesDocumentos = New List(Of Int64)

                        Return _tagWatchaer

                    End If

                End If

            End If

            Return _tagWatchaer

        End Function

        Public Function CrearLink(ByVal listaNombres_ As List(Of String),
                                  ByVal tipoDato_ As IGeneradorLinks.TipoDatoListaNombres,
                                  ByVal comportamiento_ As IGeneradorLinks.TipoComportamiento,
                                  Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                                  Optional ByVal parametros_ As String = "") As TagWatcher Implements IGeneradorLinks.CrearLink

            If listaNombres_.Count = 0 Then

                _tagWatchaer.SetError(TagWatcher.ErrorTypes.C6_029_0007)

                Return _tagWatchaer

            End If

            Dim clausula_ As String = PreparaClausulaTipoListaNombres(listaNombres_, tipoDato_)

            If ValidaDocumentos(clausula_).Status = TagWatcher.TypeStatus.Ok Then

                If ValidaPermisos().Status = TagWatcher.TypeStatus.Ok Then

                    If CrearGuardarLink(comportamiento_, rutaWeb_, parametros_).Status = TagWatcher.TypeStatus.Ok Then

                        _listaClavesDocumentos = New List(Of Int64)

                        Return _tagWatchaer

                    End If

                End If

            End If

            Return _tagWatchaer

        End Function

        Private Function ValidaDocumentos(ByVal clausula_ As String) As TagWatcher

            Dim consultaDocumento_ As IOperacionesCatalogo = New OperacionesCatalogo

            With consultaDocumento_

                .EspacioTrabajo = _espacioTrabajo

                .CantidadVisibleRegistros = 1

            End With

            consultaDocumento_ = _organismo.ConsultaModulo(_espacioTrabajo,
                                                      "DocumentosLinksKBW",
                                                      clausula_)

            If _organismo.TieneResultados(consultaDocumento_) Then

                For Each campo_ As DataRow In consultaDocumento_.Vista.Tables(0).Rows

                    _listaClavesDocumentos.Add(campo_.Item("Clave documento"))

                Next

                _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

            Else

                _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_012_1015

            End If

            Return _tagWatchaer

        End Function

        Private Function ValidaPermisos() As TagWatcher

            For Each clave_ As Int64 In _listaClavesDocumentos

                Dim operacionesDocumento_ = New OperacionesDocumento64(_iOperaciones)

                operacionesDocumento_.BuscarPrivilegioUsuario(clave_)

                If operacionesDocumento_.Estatus.Status = TagWatcher.TypeStatus.Ok Then

                    _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                Else

                    _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                    '_tagWatchaer.Errors = operacionesDocumento_.Estatus.Status
                    _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_012_1028

                End If

            Next

            Return _tagWatchaer

        End Function

        Private Function CrearGuardarLink(ByVal comportamiento_ As IGeneradorLinks.TipoComportamiento,
                                          ByVal rutaWeb_ As String,
                                          ByVal parametros_ As String) As TagWatcher

            Dim llaveInsertada_ As Int64 = Nothing

            Dim claveUsuario_ As Int32 = _iOperaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

            ' GUARDA EL ENCABEZADO DEL LINK
            Dim encabezadoLink_ As IOperacionesCatalogo = _organismo.EnsamblaModulo("EncabezadoLogLinks")

            With encabezadoLink_

                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                .EspacioTrabajo = _espacioTrabajo

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                .CampoPorNombre("i_Cve_Usuario") = claveUsuario_

                Select Case comportamiento_

                    Case IGeneradorLinks.TipoComportamiento.Consultar

                        .CampoPorNombre("i_TipoLink") = 1

                    Case IGeneradorLinks.TipoComportamiento.Descargar

                        .CampoPorNombre("i_TipoLink") = 2

                    Case Else

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                        _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0001

                        Return _tagWatchaer

                End Select

                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    llaveInsertada_ = .ValorIndice()

                    _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                Else

                    _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                    _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0002

                    Return _tagWatchaer

                    Exit Function

                End If

            End With

            ' GUARDA EL DETALLES DEL LINK

            Dim detalleLink_ As IOperacionesCatalogo = _organismo.EnsamblaModulo("DetalleLogLinks")

            With detalleLink_

                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                .EspacioTrabajo = _espacioTrabajo

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

            End With

            For Each clave_ As Int64 In _listaClavesDocumentos

                With detalleLink_

                    .CampoPorNombre("i_Cve_RegistroLink") = llaveInsertada_

                    .CampoPorNombre("i_Cve_Documento") = clave_

                    If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                    Else

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                        _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0003

                    End If

                End With

            Next

            If _tagWatchaer.Status = TagWatcher.TypeStatus.Ok Then

                'Cadena cifrada = {FechaCreacion}{ClaveUsuario}{ClaveSistema}{ClaveLink}

                Dim date_ As DateTime = Date.Now

                Dim creacion_ As String = date_.ToString("yyyyMMdd")

                Dim cifrado_ As String = CifraCadena(creacion_ & _
                                                     _espacioTrabajo.MisCredenciales.ClaveUsuario & _
                                                     _espacioTrabajo.MisCredenciales.Aplicacion & _
                                                     llaveInsertada_.ToString)

                'Esta es una lista de caracteres no permitidos en el cifrado de la URL, por que si no no los reconoce al leerla
                Dim caracteresNoPermitidos_ As New List(Of String) From {"/", "&", """", "#", "+", "'"}

                For Each caracter_ In caracteresNoPermitidos_

                    If Not cifrado_.Contains(caracter_) Then

                        Continue For

                    End If

                    Dim ramdom_ As New Random()

                    Dim numero_ As Int16 = ramdom_.Next(0, 100)

                    cifrado_ = Replace(cifrado_, caracter_, numero_)

                Next

                Dim liga_ As String = _host & rutaWeb_ & "?link=" & cifrado_ & parametros_

                ' ACTUALIZA EL REGISTRO DEL LINK CON LA LIGA CREADA
                Dim guardaLifa_ As IOperacionesCatalogo = _organismo.EnsamblaModulo("EncabezadoLogLinks")

                With guardaLifa_

                    .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                    .EspacioTrabajo = _espacioTrabajo

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    .EditaCampoPorNombre("b_LinkLeido").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    .EditaCampoPorNombre("f_FechaLeido").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    .EditaCampoPorNombre("f_FechaVigencia").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    .CampoPorNombre("t_Host") = _host

                    .CampoPorNombre("t_RutaLink") = rutaWeb_ & "?link="

                    .CampoPorNombre("t_ClaveCifrada") = cifrado_

                    .CampoPorNombre("t_Parametros") = parametros_

                    If .Modificar(llaveInsertada_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        Dim link_ As New Link

                        With link_

                            .ClaveLink = llaveInsertada_

                            .Link = liga_

                            .TipoLink = comportamiento_

                        End With

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                        _tagWatchaer.ObjectReturned = link_

                    Else

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                        _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0010

                    End If

                End With


            Else

                _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0008

            End If

            Return _tagWatchaer

        End Function

        ' Funcion anterior
        Private Function CrearGuardarLink2(ByVal comportamiento_ As IGeneradorLinks.TipoComportamiento,
                                          ByVal rutaWeb_ As String,
                                          ByVal parametros_ As String) As TagWatcher

            Dim llaveInsertada_ As Int64 = Nothing

            Dim claveUsuario_ As Int32 = _iOperaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

            ' GUARDA EL ENCABEZADO DEL LINK
            Dim encabezadoLink_ As IOperacionesCatalogo = _organismo.EnsamblaModulo("EncabezadoLogLinks")

            With encabezadoLink_

                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                .EspacioTrabajo = _espacioTrabajo

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                .CampoPorNombre("i_Cve_Usuario") = claveUsuario_

                Select Case comportamiento_

                    Case IGeneradorLinks.TipoComportamiento.Consultar

                        .CampoPorNombre("i_TipoLink") = 1

                    Case IGeneradorLinks.TipoComportamiento.Descargar

                        .CampoPorNombre("i_TipoLink") = 2

                    Case Else

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                        _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0001

                        Return _tagWatchaer

                End Select

                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    llaveInsertada_ = .ValorIndice()

                    _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                Else

                    _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                    _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0002

                    Return _tagWatchaer

                    Exit Function

                End If

            End With

            ' GUARDA EL DETALLES DEL LINK

            Dim detalleLink_ As IOperacionesCatalogo = _organismo.EnsamblaModulo("DetalleLogLinks")

            With detalleLink_

                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                .EspacioTrabajo = _espacioTrabajo

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

            End With

            For Each clave_ As Int64 In _listaClavesDocumentos

                With detalleLink_

                    .CampoPorNombre("i_Cve_RegistroLink") = llaveInsertada_

                    .CampoPorNombre("i_Cve_Documento") = clave_

                    If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                    Else

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                        _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0003

                    End If

                End With

            Next

            If _tagWatchaer.Status = TagWatcher.TypeStatus.Ok Then

                Dim liga_ As String = _host & rutaWeb_ & "?" & CifraCadena("link=" & llaveInsertada_.ToString & parametros_)

                ' ACTUALIZA EL REGISTRO DEL LINK CON LA LIGA CREADA
                Dim guardaLifa_ As IOperacionesCatalogo = _organismo.EnsamblaModulo("EncabezadoLogLinks")

                With guardaLifa_

                    .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

                    .EspacioTrabajo = _espacioTrabajo

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    .EditaCampoPorNombre("b_LinkLeido").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    .EditaCampoPorNombre("f_FechaLeido").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    .EditaCampoPorNombre("f_FechaVigencia").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                    .CampoPorNombre("t_Link") = liga_

                    If .Modificar(llaveInsertada_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Ok

                        Dim link_ As New Link

                        With link_

                            .ClaveLink = llaveInsertada_

                            .Link = liga_

                            .TipoLink = comportamiento_

                        End With

                        _tagWatchaer.ObjectReturned = link_

                    Else

                        _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                        _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0010

                    End If

                End With

            Else

                _tagWatchaer.Status = TagWatcher.TypeStatus.Errors

                _tagWatchaer.Errors = TagWatcher.ErrorTypes.C6_029_0008

            End If

            Return _tagWatchaer

        End Function

        Private Function PreparaClausulaTipoClave(ByVal busqueda_ As Object, ByVal tipoClausula_ As IGeneradorLinks.TipoDatoClave) As String

            Dim clausula_ As String = Nothing

            Select Case tipoClausula_

                Case IGeneradorLinks.TipoDatoClave.ClaveDocumento

                    clausula_ = " AND i_Cve_Documento = " & busqueda_

                Case IGeneradorLinks.TipoDatoClave.ClaveFactura

                    clausula_ = " AND i_Cve_Factura = 0" 'Falta incluir la clave de la factura en el query

                Case IGeneradorLinks.TipoDatoClave.MaestroOperaciones

                    clausula_ = " AND i_Cve_MaestroOperaciones = 0" 'Falta incluir la clave del maestro de operaciones en el query

                Case Else

                    clausula_ = " AND i_cve_documento = 0"

            End Select

            Return clausula_

        End Function

        Private Function PreparaClausulaTipoNombre(ByVal busqueda_ As Object, ByVal tipoClausula_ As IGeneradorLinks.TipoDatoNombre) As String

            Dim clausula_ As String = Nothing

            Select Case tipoClausula_

                Case IGeneradorLinks.TipoDatoNombre.FolioFactura

                    clausula_ = " AND t_FolioFactura = '" & busqueda_ & "'" 'Falta incluir el folio de la factura en el query

                Case IGeneradorLinks.TipoDatoNombre.NombreDocumento

                    clausula_ = " AND t_NombreDocumento = '" & busqueda_ & "'"

                Case IGeneradorLinks.TipoDatoNombre.Referencia

                    clausula_ = " AND t_Referencia = '" & busqueda_ & "'"

                Case Else

                    clausula_ = " AND i_cve_documento = 0"

            End Select

            Return clausula_

        End Function

        Private Function PreparaClausulaTipoListaClaves(ByVal busqueda_ As Object, ByVal tipoClausula_ As IGeneradorLinks.TipoDatoListaClaves) As String

            Dim clausula_ As String = Nothing

            Select Case tipoClausula_

                Case IGeneradorLinks.TipoDatoListaClaves.ListaClaves

                    Dim contador_ As Int16 = 0

                    Dim claves_ As String = Nothing

                    For Each clave_ In busqueda_

                        If contador_ <> 0 Then

                            claves_ += ", " & clave_

                            contador_ += 1

                            Continue For

                        End If

                        claves_ += clave_

                        contador_ += 1

                    Next

                    clausula_ = " AND i_Cve_Documento IN (" & claves_ & ")"

                Case Else

                    clausula_ = " AND i_Cve_Documento = 0"

            End Select

            Return clausula_

        End Function

        Private Function PreparaClausulaTipoListaNombres(ByVal busqueda_ As Object, ByVal tipoClausula_ As IGeneradorLinks.TipoDatoListaNombres) As String

            Dim clausula_ As String = Nothing

            Select Case tipoClausula_

                Case IGeneradorLinks.TipoDatoListaNombres.ListaNombres

                    Dim contador_ As Int16 = 0

                    Dim nombres_ As String = Nothing

                    For Each clave_ In busqueda_

                        If contador_ <> 0 Then

                            nombres_ += ", '" & clave_ & "'"

                            contador_ += 1

                            Continue For

                        End If

                        nombres_ += "'" & clave_ & "'"

                        contador_ += 1

                    Next

                    clausula_ = " AND t_NombreDocumento IN (" & nombres_ & ")"

                Case Else

                    clausula_ = " AND i_cve_documento = 0"

            End Select

            Return clausula_


        End Function

        Private Function CifraCadena(ByVal cadena_ As String) As String

            'Dim largoClaveUsuario_ As Int16 = claveUsuario_.ToString.Length

            Dim metodo_ As SymmetricAlgorithm = New RijndaelManaged

            Dim cifrado_ As ICifrado = New Cifrado256

            Dim cadenaCifrada_ As String = cifrado_.CifraCadena(cadena_.ToString, metodo_, _organismo.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

            Return cadenaCifrada_

        End Function

        Private Function DescifraCadena(ByVal cadenaCifrada_ As String) As String

            'Dim largoClaveUsuario_ As Int16 = claveUsuario_.ToString.Length

            Dim metodo_ As SymmetricAlgorithm = New RijndaelManaged

            Dim cifrado_ As ICifrado = New Cifrado256

            Dim cadenaDescifrada_ As String = cifrado_.DescifraCadena(cadenaCifrada_, metodo_, _organismo.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

            Return cadenaDescifrada_

        End Function

#End Region

    End Class

End Namespace