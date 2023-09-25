Imports Wma.Exceptions
Imports Gsol.basedatos.Operaciones
Imports System.IO
Imports gsol.documento


Public Class Documentos64
    Inherits LeerDocumento

#Region "Atributos"

    'Lista de documentos que recibe
    Private _documentos As List(Of String)
    Private _documentosRepresentacionImpresa As List(Of String)
    Private _documentosAlmacenados As List(Of String)
    Private _listaMensajesError As List(Of String)

    Private _sistema As Organismo
    Private _ioperacionescatalogo As IOperacionesCatalogo

    'Private _claveDocumento As Int32 = 0
    'Private _tipoCarga As TipoCarga

    ''Private _ioperacionesInsercion As IOperacionesCatalogo
    Public _rutaDocumentoOrigen As String
    Public _nombreDocumento As String
    'Private _claveTipoDocumento As Int32 = 0
    Private _claveRepositorioDigital As Int32 = 0
    'Private _documentoOrigen As String
    Private _extension As String
    Private _extensionReconocida As String = Nothing
    'Private _claveVistaEntorno As Int32 = 0
    'Private _clausulaVistaEntorno As String
    Private _claveUUID As Int32 = 0
    Private _UUID As String
    Private _carpetaDestino, carpetaRespositorioContingencia_ As String
    'Private _servidorDestino As String
    'Private _estatus As Int32 = 0
    Private _rutaCompletaDestino
    Private _fechaDocumento As Date
    Private _RFC As String
    'Private _mensajeGeneral As String = Nothing
    Private _versionCFDi As String = Nothing

    'Private _caracteristicasCFDIs As New Dictionary(Of String, Int32)

    Private _IOPServidorContingencia As String
    Private _IOPRutaContingencia As String


    Private _tipoComprobante As String

#End Region

#Region "Propiedades"

    Public Property Documentos As List(Of String)
        Get

            Return _documentos

        End Get
        Set(value As List(Of String))

            _documentos = value

        End Set
    End Property

    Public Property ListaMensajes As List(Of String)
        Get
            Return _listaMensajesError
        End Get
        Set(value As List(Of String))
            _listaMensajesError = value
        End Set
    End Property

#End Region


#Region "Constructores"

    Sub New()

        _ioperacionescatalogo = New OperacionesCatalogo

        _sistema = New Organismo

        _listaMensajesError = New List(Of String)

        '_documentos = New List(Of Documento)

    End Sub

    Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

        Me.New()

        _ioperacionescatalogo = ioperaciones_

    End Sub

#End Region

    Public Sub CargarCFDIsTerceros(ByVal tipoDocumento_ As IDocumento.TiposDocumentos)

        If _documentos.Count < 1 Then

            Exit Sub

        End If

        Select Case tipoDocumento_

            Case IDocumento.TiposDocumentos.XML_COMPROBANTESPAGOTERCEROS

                CargarComplementosPagoTerceros()

            Case IDocumento.TiposDocumentos.XML_FACTURASTERCEROS

                CargarFacturasTerceros()

        End Select

    End Sub

    Private Sub CargarComplementosPagoTerceros()

        Dim documento_ As LeerDocumento

        _listaMensajesError.Clear()

        For Each documentoEnLista_ In _documentos

            If Path.GetExtension(documentoEnLista_).ToLower <> ".xml" Then

                Continue For

            End If

            Limpiar()

            'Dim documento_ As IDocumento = New LeerDocumento
            documento_ = New LeerDocumento

            _rutaDocumentoOrigen = documentoEnLista_

            '_documentoOrigen = File.ReadAllText(_rutaDocumentoOrigen)

            _nombreDocumento = Path.GetFileName(_rutaDocumentoOrigen)

            documento_.NombreDocumento = _nombreDocumento

            documento_.CargarDesdeRuta = _rutaDocumentoOrigen

            documento_.ProcesarDocumento(IDocumento.TiposProcesable.XMLCFDIComplementoPago)

            If documento_.TagWatcherActive.Status <> TagWatcher.TypeStatus.Ok Then

                _listaMensajesError.Add(documento_.NombreDocumento & "->" & documento_.TagWatcherActive.ErrorDescription)

                Continue For

                'Else

                '    _documentosAlmacenados.Add(documentoEnLista_)

            End If

            _tipoComprobante = documento_.Comprobante.TipoDeComprobante

            _UUID = documento_.Comprobante.UUID

            _versionCFDi = documento_.Comprobante.Version

            _RFC = documento_.Comprobante.RFCEmisor

            _fechaDocumento = documento_.Comprobante.Fecha

            If _tipoComprobante <> "P" Then

                _listaMensajesError.Add(documento_.NombreDocumento & "->" & " Este archivo no es de tipo pago")

                Continue For

            End If

            If RevisaExisteUUID(_UUID) Then

                _listaMensajesError.Add(documento_.NombreDocumento & "->" & " Ya ha sido registrado previamente")

                Continue For

            End If


            ''Dim Documento As Documento

            'Dim OperacionesDocumento As New OperacionesDocumento64

            'Dim documentos_ As New List(Of gsol.documento.Documento)

            'Dim rutas_ As New Dictionary(Of IOperacionesDocumento.RutasRelativaDocumento, String)


            'rutas_.Add(IOperacionesDocumento.RutasRelativaDocumento.RFCProveedorPagosTerceros, _RFC)

            'rutas_.Add(IOperacionesDocumento.RutasRelativaDocumento.Anio, _fechaDocumento.Year.ToString)

            'rutas_.Add(IOperacionesDocumento.RutasRelativaDocumento.Mes, _fechaDocumento.Month.ToString)

            'documentos_.Add(New gsol.documento.Documento With {.NombreDocumento = _nombreDocumento,
            '                                                   .FechaRegistro = Date.Now,
            '                                                   .GuardarDocumento = True,
            '                                                   .RutasRelativas = rutas_,
            '                                                   .RutaDocumentoOrigen = _rutaDocumentoOrigen,
            '                                                   .FolioDocumento = _UUID,
            '                                                   .TipoDocumento = tipoDocumento_
            '                                                   .VersionCFDI = IIf(_versionCFDi = "3.3", IOperacionesDocumento.VersionesCFDI.TresTres, IOperacionesDocumento.VersionesCFDI.NoAplica),
            '                                                   .EstatusDocumento = IOperacionesDocumento.EstatusDocumento.ProcesadoConExito})

            'Dim tag_ As New TagWatcher

            'tag_ = OperacionesDocumento.ProcesarDocumentoCompleto(documentos_, _ioperacionescatalogo.Clone)

            'If tag_.Status <> TagWatcher.TypeStatus.Ok Then

            '    _listaMensajesError.Add("Archivo:" & documento_.NombreDocumento & "->" & " No se logró registrar en el MO")

            '    Continue For

            'End If

            'documento_.Comprobante.claveDocumento = OperacionesDocumento.Documentos(0).Clave

            _extension = Path.GetExtension(documentoEnLista_).ToLower()

            If Not _extension = _extensionReconocida Then

                BuscaTipoDocumento(_extension)

            End If

            _rutaCompletaDestino = _carpetaDestino & "\" & _RFC

            carpetaRespositorioContingencia_ = _IOPServidorContingencia & _IOPRutaContingencia & "\" & _RFC

            If Not RegistraListaDocumentos(documento_.Comprobante) Then

                _listaMensajesError.Add(documento_.NombreDocumento & "->" & " Ocurrio un error al intentar registrar el documento")

                Continue For

            Else

                _listaMensajesError.Add(documento_.NombreArchivo & "->" & " Registrado correctamente")

            End If

            RespaldoServidor(_rutaCompletaDestino, _rutaDocumentoOrigen, _fechaDocumento, _claveUUID, _RFC, _nombreDocumento)

            'GuardarDocumento(documento_, True)

        Next

    End Sub
    'ñCambiar
    Private Function RevisaExisteUUID(ByVal uuid_ As String) As Boolean

        Dim script_ As String =
            " Select i_Cve_Documento,i_Cve_DivisionMiEmpresa " & _
            " from Reg012Documentos as d" & _
            " where contains(d.t_FolioDocumento, '" & uuid_ & "') and i_cve_estado= 1 and i_Cve_DivisionMiEmpresa = " & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        If _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(script_) Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                    If _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count >= 1 Then

                        Try

                            If _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count >= 1 Then

                                '_claveUUID = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)("i_Cve_Documento").ToString

                                Return True

                            End If


                        Catch ex As Exception

                            Return True

                            '_claveUUID = 0

                            '_sistema.GsDialogo("Ocurrio un error al intentar verificar el UUID en la base de datos", Componentes.SistemaBase.GsDialogo.TipoDialogo.Err)

                        End Try

                    End If

                End If

            End If

        End If

        Return False

        '_claveUUID = 0

    End Function
    'ñ Cambiar
    Public Function RegistraListaDocumentos(ByVal comprobante_ As ComprobanteCFDi) As Boolean

        Dim scriptDocumentos_ As String = _
            " BEGIN TRANSACTION TransaccionDoctosCFDiUser" & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ClaveUsuario & Chr(13) & _
            " BEGIN TRY  " & Chr(13) & _
            " DECLARE @ClaveComplemento as Integer;" & Chr(13) & _
            " DECLARE @ClavePago as Integer;" & Chr(13) & _
            " SET NOCOUNT ON " & Chr(13) & _
            " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   " & Chr(13) & _
            " "

        'Borrar cuando esté el maestro de documentos
        scriptDocumentos_ = scriptDocumentos_ & _
               " /*  Reg012Documentos */" & Chr(13) & _
               " DECLARE @ClaveDocumento as Integer;" & Chr(13) & _
               " Insert into Reg012Documentos" & Chr(13) & _
               " (t_RutaDocumentoOrigen,i_Cve_Estado,i_Cve_TipoDocumento,f_FechaRegistro,i_Cve_EstatusDocumento,t_NombreDocumento, t_FolioDocumento,i_Cve_RepositorioDigital,i_Cve_DivisionMiEmpresa,t_VersionCFDi)" & Chr(13) & _
               " values " & Chr(13) & _
               " ('" & _rutaDocumentoOrigen & "',1,3,'" & Now.Date & "',1,'" & _nombreDocumento & "','" & comprobante_.UUID & "'," & _claveRepositorioDigital & "," & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & "," & comprobante_.Version & ")" & Chr(13) & _
               " set @ClaveDocumento = @@IDENTITY; " & Chr(13)
        'hasta aquí borrar

        scriptDocumentos_ = scriptDocumentos_ & _
                " /*  CFDi */" & Chr(13) & _
                " Insert into Enc013ComplementosPagoTerceros" & Chr(13) & _
                " (i_Cve_DocumentoCFDiPago,f_FechaComprobante,c_Folio,c_Serie,c_TipoComprobante,c_LugarExpedicion, c_RFCEmisor,c_RFCReceptor,c_UUID,i_Cve_Estado,i_Cve_Estatus,i_Cve_DivisionMiEmpresa)" & Chr(13) & _
                " values " & Chr(13) & _
                " (@ClaveDocumento,'" & _
                comprobante_.Fecha.ToString("dd/MM/yyyy H:mm:ss") & "','" & _
                comprobante_.Folio & "','" & _
                comprobante_.Serie & "','" & _
                comprobante_.TipoDeComprobante & "','" & _
                comprobante_.LugarExpedicion & "','" & _
                comprobante_.RFCEmisor & "','" & _
                comprobante_.RFCReceptor & "','" & _
                comprobante_.UUID & "',1,1," & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & ")" & Chr(13) & _
                " set @ClaveComplemento = @@IDENTITY; " & Chr(13)
        '" ('" & _
        'comprobante_.claveDocumento & "','" & _
        For Each pago_ As ComprobantePagos In comprobante_.ListaPagos

            scriptDocumentos_ = scriptDocumentos_ & _
                " /* Pagos */" & Chr(13) & _
                " Insert into Det013PagosComplementosPagoTerceros" & Chr(13) & _
                " (i_Cve_ComplementoPagoTerceros,i_Cve_DocumentoCFDiPago,f_FechaPago,c_FormaDePagoP,c_MonedaP,c_TipoCambioP, c_Monto,c_CtaOrdenante,c_NumOperacion,c_RfcEmisorCtaOrd,c_NomBancoOrdExt,c_CtaBeneficiario,c_RfcEmisorCtaBen,i_Cve_Estado,i_Cve_Estatus,i_Cve_DivisionMiEmpresa)" & Chr(13) & _
                " values " & Chr(13) & _
                " (@ClaveComplemento,@ClaveDocumento,'" & _
                pago_.FechaPago.ToString("dd/MM/yyyy H:mm:ss") & "','" & _
                pago_.FormaPagoP & "','" & _
                pago_.MonedaP & "'," & _
                pago_.TipoCambioP & "," & _
                pago_.Monto & ",'" & _
                pago_.CuentaOrdenante & "','" & _
                pago_.NumOperacion & "','" & _
                pago_.RFCEmisorCtaOrd & "','" & _
                pago_.BancoOrdExt & "','" & _
                pago_.CuentaBeneficiario & "','" & _
                pago_.RFCEmisorCtaBen & "',1,1," & _
                _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & ")" & Chr(13) & _
                " set @ClavePago = @@IDENTITY; " & Chr(13)

            'comprobante_.claveDocumento & ",'" & _
            scriptDocumentos_ = scriptDocumentos_ & _
                        " /* DoctosRelacionados */" & Chr(13) & _
                        " Insert into Vin013ComplementosPagoTercerosDR" & Chr(13) & _
                        " (i_Cve_PagoComplementoPagoTerceros,c_UUID,c_Serie,c_Folio,c_MonedaDR,c_TipoCambioDR,c_MetodoDePagoDR,c_NumParcialidad,d_ImpSaldoAnt,d_ImpPagado,d_ImpSaldoInsoluto,i_Cve_Estado,i_Cve_Estatus,i_Cve_DivisionMiEmpresa)" & Chr(13) & _
                        " values " & Chr(13)

            Dim posicion_ As Int32 = 1

            For Each DoctoRelacionado In pago_.ListaDocumentosRelacionados

                If posicion_ = 1 Then

                    scriptDocumentos_ = scriptDocumentos_ & _
                        " (@ClavePago,'" & _
                        DoctoRelacionado.idDocumento & "','" & _
                        DoctoRelacionado.Serie & "','" & _
                        DoctoRelacionado.Folio & "','" & _
                        DoctoRelacionado.MonedaDR & "'," & _
                        DoctoRelacionado.TipoCambioDR & ",'" & _
                        DoctoRelacionado.MetodoDePagoDR & "','" & _
                        DoctoRelacionado.NumParcialidad & "'," & _
                        DoctoRelacionado.SaldoAnterior & "," & _
                        DoctoRelacionado.ImportePagado & "," & _
                        DoctoRelacionado.SaldoInsoluto & ",1,1," & _
                        _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & ")" & Chr(13)

                    posicion_ += 1

                Else

                    scriptDocumentos_ = scriptDocumentos_ & _
                    "," & _
                    " (@ClavePago,'" & _
                        DoctoRelacionado.idDocumento & "','" & _
                        DoctoRelacionado.Serie & "','" & _
                        DoctoRelacionado.Folio & "','" & _
                        DoctoRelacionado.MonedaDR & "'," & _
                        DoctoRelacionado.TipoCambioDR & ",'" & _
                        DoctoRelacionado.MetodoDePagoDR & "','" & _
                        DoctoRelacionado.NumParcialidad & "'," & _
                        DoctoRelacionado.SaldoAnterior & "," & _
                        DoctoRelacionado.ImportePagado & "," & _
                        DoctoRelacionado.SaldoInsoluto & ",1,1," & _
                        _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & ")" & Chr(13)

                End If

            Next


        Next

        scriptDocumentos_ = scriptDocumentos_ & Chr(13) & _
            " COMMIT TRANSACTION  TransaccionDoctosCFDiUser" & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ClaveUsuario & Chr(13) & _
            " SELECT  " & Chr(13) & _
            " 'OK' as ESTATUS, @ClaveComplemento as ClaveComplemento, @ClaveDocumento as claveDocumento" & Chr(13) & _
            "  END TRY " & Chr(13) & _
            " BEGIN CATCH " & Chr(13) & _
            "  	ROLLBACK TRANSACTION TransaccionDoctosCFDiUser" & _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.ClaveUsuario & Chr(13) & _
            "   Select " & Chr(13) & _
            "   'ERROR' as ESTATUS , " & Chr(13) & _
            "   ERROR_NUMBER() AS ErrorNumber, " & Chr(13) & _
            "   ERROR_SEVERITY() AS ErrorSeverity, " & Chr(13) & _
            "   ERROR_STATE() AS ErrorState, " & Chr(13) & _
            "   ERROR_PROCEDURE() AS ErrorProcedure, " & Chr(13) & _
            "   ERROR_LINE() AS ErrorLine, " & Chr(13) & _
            "   ERROR_MESSAGE() AS ErrorMessage " & Chr(13) & _
            "  END CATCH "


        If Not RegistraDocumentosBaseDatos(scriptDocumentos_) Then

            '_listaMensajesError.Add("Ocurrio un error al intentar registrar los documentos")

            Return False

        End If

        Return True

    End Function

    Private Function RegistraDocumentosBaseDatos(ByVal script_ As String) As Boolean

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        If _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(script_) Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                    If _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count >= 1 Then

                        Dim respuestaMensaje_ As String = Nothing

                        Try

                            respuestaMensaje_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables( _
                                _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count - 1).Rows(0)("ESTATUS").ToString

                            _claveUUID = 0

                            _claveUUID = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables( _
                                _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count - 1).Rows(0)("claveDocumento").ToString

                            If respuestaMensaje_ = "OK" Then

                                Return True

                            End If

                        Catch ex As Exception

                            Return False

                        End Try

                    End If

                End If

            End If

        End If

        Return False

    End Function

    Private Sub RespaldoServidor(ByVal rutaDestino_ As String, _
                                   ByVal rutaOrigen_ As String, _
                                   ByVal fechaDocumento_ As Date, _
                                   ByVal claveDocumento_ As Int32, _
                                   ByVal rfc_ As String, _
                                   ByVal nombre_ As String)

        rutaDestino_ = Path.Combine(rutaDestino_, Now.Year, Now.Month)

        Dim rutaDestinoContingencia = Path.Combine(carpetaRespositorioContingencia_, Now.Year, Now.Month)

        If Not Directory.Exists(rutaDestino_) Then

            System.IO.Directory.CreateDirectory(rutaDestino_)

        End If

        If Not Directory.Exists(rutaDestinoContingencia) Then

            System.IO.Directory.CreateDirectory(rutaDestinoContingencia)

        End If

        Dim archivo = "CFDI-" & Format(fechaDocumento_, "yyyy-MM") & "-" & claveDocumento_ & "-" & rfc_ & "-" & nombre_

        Dim pathDestino_ As String = Path.Combine(rutaDestino_, archivo)


        If Not File.Exists(pathDestino_) Then

            File.Copy(rutaOrigen_, pathDestino_)

        End If

        Dim pathDestinoContingencia_ As String = Path.Combine(rutaDestinoContingencia, archivo)

        If Not File.Exists(pathDestinoContingencia_) Then

            File.Copy(rutaOrigen_, pathDestinoContingencia_)

        End If

    End Sub

    Private Sub BuscaTipoDocumento(ByVal extension_ As String)

        Dim t_ClausulaTipoDocumento_ As String = Nothing

        Dim ioperacionesbusqueda As IOperacionesCatalogo = New OperacionesCatalogo

        ioperacionesbusqueda.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

        'Me._movimientosComercializacion = New MovimientosComercializacion(_iOperacionesCatalogoAtlas)

        ioperacionesbusqueda.VistaEncabezados = "Ve012IURepositoriosDigitales"

        ioperacionesbusqueda.OperadorCatalogoConsulta = "vt012RepositoriosDigitales"

        Select Case extension_

            Case ".xml", ".XML"

                t_ClausulaTipoDocumento_ = " and i_Cve_TipoDocumento = 3 "

            Case ".pdf", ".PDF"

                t_ClausulaTipoDocumento_ = " and i_Cve_TipoDocumento = 4 "

        End Select

        ioperacionesbusqueda.ClausulasLibres = t_ClausulaTipoDocumento_ & " and t_Extension='" & extension_ & "' AND i_Cve_DivisionMiEmpresa=" & _
            _ioperacionescatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

        ioperacionesbusqueda.CantidadVisibleRegistros = 1

        ioperacionesbusqueda.PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta)

        ioperacionesbusqueda.GenerarVista()

        If ioperacionesbusqueda.Vista.Tables.Count > 0 Then

            If ioperacionesbusqueda.Vista.Tables(0).Rows.Count > 0 Then

                '_claveTipoDocumento = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Clave tipo de documento")

                _claveRepositorioDigital = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Clave")

                carpetaRespositorioContingencia_ = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Servidor contingencia") & _
                    ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Ruta contingencia")

                _IOPServidorContingencia = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Servidor contingencia")

                _IOPRutaContingencia = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Ruta contingencia")

                '_servidorDestino = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Servidor")

                _carpetaDestino = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Servidor") & ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Ruta")

                _extensionReconocida = extension_

            End If

        End If

    End Sub

    'Private Function GuardarDocumento(ByVal listaDocumentos_ As List(Of Documento)) As TagWatcher

    '    Try

    '        For Each documento_ In listaDocumentos_

    '            If Directory.Exists(documento_.RutaDocumentoCompleta) Then

    '                If File.Exists(documento_.RutaDocumentoCompleta & "" & documento_.NombreDocumento) Then

    '                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1010)

    '                    Return _estatus

    '                End If

    '            Else

    '                My.Computer.FileSystem.CreateDirectory(documento_.RutaDocumentoCompleta)

    '            End If

    '            System.IO.File.Copy(documento_.RutaDocumentoOrigen & "" & documento_.NombreDocumento,
    '                                documento_.RutaDocumentoCompleta & "" & documento_.NombreDocumento)

    '        Next

    '        Return _estatus

    '    Catch ex As Exception

    '        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1011)

    '        Return _estatus

    '    End Try

    'End Function

    Private Sub Limpiar()

        _rutaDocumentoOrigen = ""

        _nombreDocumento = ""

        _tipoComprobante = ""

        _UUID = ""

        _versionCFDi = ""

        _RFC = ""

        _fechaDocumento = Nothing

        _extension = ""

        _rutaCompletaDestino = ""

    End Sub



    Private Sub CargarFacturasTerceros()

        'Limpiar()

        '_rutaDocumentoOrigen = p_sArchivo

        '_documentoOrigen = File.ReadAllText(_rutaDocumentoOrigen)

        '_nombreDocumento = Path.GetFileName(_rutaDocumentoOrigen)

        '_extension = Path.GetExtension(_nombreDocumento)

        'If _extensionReconocida Is Nothing Then

        '    If Not _extension = _extensionReconocida Then

        '        BuscaTipoDocumento(_extension)

        '        _extensionReconocida = _extension

        '    End If

        'End If

        'iDocumento_.NombreDocumento = _nombreDocumento

        'iDocumento_.CargarDesdeRuta = _rutaDocumentoOrigen
        ''bautismen


        'iDocumento_.ProcesarDocumento(IDocumento.TiposProcesable.XML)


        'Dim recorrido_ As Int32 = 0

        'For Each caracteristica_ As String In iDocumento_.AtributosProcesados.Keys

        '    Select Case caracteristica_

        '        Case "Version" '?

        '            _versionCFDi = iDocumento_.AtributosProcesados.Values(recorrido_)

        '        Case "UUID" '11

        '            _UUID = iDocumento_.AtributosProcesados.Values(recorrido_)

        '            BuscaUUIDFTI(_UUID)

        '        Case "RFCEmisor" '8

        '            _rutaCompletaDestino = _carpetaDestino & "\" & iDocumento_.AtributosProcesados.Values(recorrido_)

        '            ''carpetaRespositorioContingencia_ = carpetaRespositorioContingencia_ & "\" & iDocumento_.AtributosProcesados.Values(recorrido_)

        '            'Dim auxiliar_ As String = carpetaRespositorioContingencia_

        '            'carpetaRespositorioContingencia_ = Nothing

        '            'carpetaRespositorioContingencia_ = auxiliar_ & "\" & iDocumento_.AtributosProcesados.Values(recorrido_)

        '            _RFC = iDocumento_.AtributosProcesados.Values(recorrido_)

        '            carpetaRespositorioContingencia_ = _IOPServidorContingencia & _IOPRutaContingencia & "\" & _RFC



        '            'carpetaRespositorioContingencia_ = ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Servidor contingencia") & _
        '            'ioperacionesbusqueda.Vista.Tables(0).Rows(0)("Ruta contingencia")


        '        Case "Fecha" '2

        '            _fechaDocumento = iDocumento_.AtributosProcesados.Values(recorrido_)

        '    End Select

        '    recorrido_ = recorrido_ + 1

        'Next


        'If _claveUUID = 0 Then

        '    GuardarDocumento(iDocumento_, True)

        'Else

        '    MsgBox("El UUID [" & _UUID & "] ya fue registrado con anterioridad con el folio de documento [" & _claveUUID & "]")

        'End If

    End Sub
End Class
