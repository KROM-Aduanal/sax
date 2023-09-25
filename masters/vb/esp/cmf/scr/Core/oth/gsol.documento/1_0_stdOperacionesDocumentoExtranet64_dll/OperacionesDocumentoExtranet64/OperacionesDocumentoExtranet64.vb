Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.Componentes.SistemaBase.GsDialogo
Imports System.IO
Imports Ionic.Zip

Namespace gsol.documento

    Public Class OperacionesDocumentoExtranet64
        Implements IOperacionesDocumentoExtranet


#Region "Atributos"

        Private _sistema As Organismo

        Private _estatus As TagWatcher

        Private _operaciones As IOperacionesCatalogo

        Private _documentosExtranet As List(Of DocumentoExtranet)

        Private _documentosExtranetNoEncontrados As List(Of DocumentoExtranet)

#End Region

#Region "Propiedades"

        Public ReadOnly Property DocumentosExtranet As List(Of DocumentoExtranet) Implements IOperacionesDocumentoExtranet.DocumentosExtranet

            Get

                Return _documentosExtranet

            End Get

        End Property

        Public ReadOnly Property DocumentosExtranetNoEncontrados As List(Of DocumentoExtranet)

            Get

                Return _documentosExtranetNoEncontrados

            End Get

        End Property

        Public ReadOnly Property Estatus As TagWatcher Implements IOperacionesDocumentoExtranet.Estatus

            Get

                Return _estatus

            End Get

        End Property

        Public Property Operaciones As IOperacionesCatalogo Implements IOperacionesDocumentoExtranet.Operaciones

            Get

                Return _operaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _operaciones = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _operaciones = New OperacionesCatalogo

            _estatus = New TagWatcher

            _estatus.SetOK()

            _sistema = New Organismo

            _documentosExtranet = New List(Of DocumentoExtranet)

            _documentosExtranetNoEncontrados = New List(Of DocumentoExtranet)

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

            Me.New()

            _operaciones = ioperaciones_

        End Sub

#End Region

        Public Function ObtenerDocumento(ByVal listaClaves_ As List(Of Int64)) As TagWatcher Implements IOperacionesDocumentoExtranet.ObtenerDocumento

            If listaClaves_.Count > 0 Then

                Dim clausula_ As String = Nothing

                Dim contador_ As Int16 = 0

                Dim stringClaves_ As String = Nothing

                For Each clave_ In listaClaves_

                    If contador_ <> 0 Then

                        stringClaves_ += ", " & clave_

                        contador_ += 1

                        Continue For

                    End If

                    stringClaves_ += clave_

                    contador_ += 1

                Next

                clausula_ = " AND i_Cve_Documento IN (" & stringClaves_ & ")"

                Dim documentosMDZIP_ As IOperacionesCatalogo

                documentosMDZIP_ = _sistema.EnsamblaModulo("DocumentosLinksKBW").Clone

                With documentosMDZIP_

                    .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta)

                    .EspacioTrabajo = _operaciones.EspacioTrabajo

                    .CantidadVisibleRegistros = 0

                    .ClausulasLibres = clausula_

                    .GenerarVista()

                End With

                If _sistema.TieneResultados(documentosMDZIP_) Then

                    Dim listaDocumentos_ = New List(Of DocumentoExtranet)

                    Dim documentoExtranet_ = New DocumentoExtranet

                    documentoExtranet_.RutaCompleta = Replace(documentosMDZIP_.Vista.Tables(0).Rows(0).Item("Carpeta Documento").ToString() & "KBWeb_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".zip", "M:", "\\10.66.3.1\KromBase")

                    For Each registroDocumentos_ As DataRow In documentosMDZIP_.Vista.Tables(0).Rows

                        Dim documentoMD_ = New DocumentoExtranet

                        documentoMD_.Folio = registroDocumentos_.Item("Nombre Archivo").ToString()

                        documentoMD_.RutaCompleta = Replace(registroDocumentos_.Item("Ruta Documento").ToString(), "M:", "\\10.66.3.1\KromBase")

                        ' Este segmento es para las representaciones impresas
                        If registroDocumentos_.Item("Clave documento virtualizado") = 1 Then

                            Dim listaSegmentosNombreDocumento_ As List(Of String) = Split(registroDocumentos_.Item("Nombre Archivo").ToString(), ".").ToList()

                            Dim folioDocumentoVirtualizado_ As String = Replace(registroDocumentos_.Item("Nombre Archivo").ToString(), "." & listaSegmentosNombreDocumento_.Max, registroDocumentos_.Item("Extension archivo virtualizar"))

                            Dim rutaDocumentoVirtualizado_ As String = Replace(Replace(registroDocumentos_.Item("Ruta Documento").ToString(), "M:", "\\10.66.3.1\KromBase"), "." & listaSegmentosNombreDocumento_.Max, registroDocumentos_.Item("Extension archivo virtualizar"))

                            Dim documentoVistualizadoMD_ = New DocumentoExtranet

                            documentoVistualizadoMD_.Folio = folioDocumentoVirtualizado_

                            documentoVistualizadoMD_.RutaCompleta = rutaDocumentoVirtualizado_

                            listaDocumentos_.Add(documentoVistualizadoMD_)

                        End If

                        listaDocumentos_.Add(documentoMD_)

                    Next

                    documentoExtranet_.DocumentosSoporteExtranet = listaDocumentos_

                    Dim zip_ = New ZipFile

                    For Each listaDocumento_ As DocumentoExtranet In documentoExtranet_.DocumentosSoporteExtranet

                        If BuscarArchivo(listaDocumento_) Then

                            Dim b() As Byte = IO.File.ReadAllBytes(listaDocumento_.RutaCompleta)

                            zip_.AddEntry(listaDocumento_.Folio, b)

                        End If

                    Next

                    zip_.Save(documentoExtranet_.RutaCompleta)

                    _documentosExtranet.Add(documentoExtranet_)

                    _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                    Return _estatus

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1015)

                    Return _estatus

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_029_0006)

            End If

            Return _estatus

        End Function

        Public Function ObtenerDocumento(ByVal documento_ As DocumentoExtranet,
                                         Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) As TagWatcher Implements IOperacionesDocumentoExtranet.ObtenerDocumento

            Dim sql_ = " and " & documento_.NombreTecnicoBusqueda & " in ('" & documento_.ValorBusqueda & "')"

            Dim opDocumento_ = New OperacionesCatalogo

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            opDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            opDocumento_.CantidadVisibleRegistros = 0

            opDocumento_ = _sistema.ConsultaModulo(opDocumento_.EspacioTrabajo,
                                                   documento_.TokenBusqueda,
                                                   sql_)

            If _sistema.TieneResultados(opDocumento_) Then

                For Each registroDocumento_ As DataRow In opDocumento_.Vista.Tables(0).Rows

                    Select Case documento_.TipoDocumentoExtranet

                        Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.DocumentosMaestroDocumentosZIP

                            Dim documentoExtranet_ = New DocumentoExtranet

                            Dim clausula_ As String = Nothing

                            Dim nombreArchivo_ As String = Nothing

                            Dim documentosMDZIP_ As IOperacionesCatalogo = _sistema.EnsamblaModulo("DigitalizacionMaestroDocumentosKBWeb")

                            With documentosMDZIP_

                                .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta)

                                .EspacioTrabajo = _operaciones.EspacioTrabajo

                                .CantidadVisibleRegistros = 0

                                If Not documento_.TiposDocumentosDescargables Is Nothing Then

                                    clausula_ = " AND t_Referencia = '" & documento_.ValorBusqueda & "' AND i_Cve_TipoDocumento IN (" & documento_.TiposDocumentosDescargables & ")"

                                    nombreArchivo_ = "Nombre Archivo Zip"

                                Else

                                    clausula_ = " AND t_Referencia = '" & documento_.ValorBusqueda & "'"

                                    nombreArchivo_ = "Numero Referencia"

                                End If

                                .ClausulasLibres = clausula_

                                .GenerarVista()

                            End With

                            If _sistema.TieneResultados(documentosMDZIP_) Then

                                Dim listaDocumentos_ = New List(Of DocumentoExtranet)

                                'documentoExtranet_.RutaCompleta = documentosMDZIP_.Vista.Tables(0).Rows(0).Item("Carpeta Documento").ToString() & "KBWeb_" & documentosMDZIP_.Vista.Tables(0).Rows(0).Item("Numero Referencia").ToString() & ".zip"

                                documentoExtranet_.RutaCompleta = Replace(documentosMDZIP_.Vista.Tables(0).Rows(0).Item("Carpeta Documento").ToString() & "KBWeb_" & documentosMDZIP_.Vista.Tables(0).Rows(0).Item(nombreArchivo_).ToString() & ".zip", "M:", "\\10.66.3.1\KromBase")

                                For Each registroDocumentos_ As DataRow In documentosMDZIP_.Vista.Tables(0).Rows

                                    Dim documentoMD_ = New DocumentoExtranet

                                    documentoMD_.Folio = registroDocumentos_.Item("Nombre Archivo").ToString()

                                    'documentoMD_.RutaCompleta = registroDocumentos_.Item("Ruta Documento").ToString()
                                    documentoMD_.RutaCompleta = Replace(registroDocumentos_.Item("Ruta Documento").ToString(), "M:", "\\10.66.3.1\KromBase")

                                    listaDocumentos_.Add(documentoMD_)

                                Next

                                documentoExtranet_.DocumentosSoporteExtranet = listaDocumentos_

                                Dim zip_ = New ZipFile

                                For Each listaDocumento_ As DocumentoExtranet In documentoExtranet_.DocumentosSoporteExtranet

                                    If BuscarArchivo(listaDocumento_) Then

                                        Dim b() As Byte = IO.File.ReadAllBytes(listaDocumento_.RutaCompleta)
                                        'Dim b() As Byte = IO.File.ReadAllBytes(Replace(listaDocumento_.RutaCompleta, "M:", "\\10.66.3.1\KromBase"))

                                        zip_.AddEntry(listaDocumento_.Folio, b)

                                    End If

                                Next

                                zip_.Save(documentoExtranet_.RutaCompleta)

                                _documentosExtranet.Add(documentoExtranet_)

                                _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                Return _estatus

                            Else

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1015)

                                Return _estatus

                            End If

                        Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.DocumentosMaestroDocumentos

                            Dim documentoExtranet_ = New DocumentoExtranet

                            'documento_.ValorBusqueda = registroDocumento_.Item("Numero Referencia").ToString()
                            documento_.Folio = registroDocumento_.Item("Numero Referencia").ToString()

                            'documento_.RutaCompleta = registroDocumento_.Item("Ruta Documento").ToString()
                            documento_.RutaCompleta = Replace(registroDocumento_.Item("Ruta Documento").ToString(), "M:", "\\10.66.3.1\KromBase")

                            _documentosExtranet.Add(documento_)

                            If BuscarArchivo(documento_) Then

                                _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                Return _estatus

                            Else

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1016)

                                Return _estatus

                            End If

                        Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.ClaveDocumentosMaestroDocumentos

                            Dim documentoExtranet_ = New DocumentoExtranet

                            'documento_.ValorBusqueda = registroDocumento_.Item("Numero Referencia").ToString()
                            documento_.Folio = registroDocumento_.Item("i_Cve_Documento").ToString()

                            'documento_.RutaCompleta = registroDocumento_.Item("Ruta Documento").ToString()
                            documento_.RutaCompleta = Replace(registroDocumento_.Item("Ruta Documento").ToString(), "M:", "\\10.66.3.1\KromBase")

                            _documentosExtranet.Add(documento_)

                            If BuscarArchivo(documento_) Then

                                _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                Return _estatus

                            Else

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1016)

                                Return _estatus

                            End If

                        Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.Operativo

                            documento_.ValorBusqueda = registroDocumento_.Item("Numero Referencia").ToString()

                            documento_.Folio = registroDocumento_.Item("Numero Referencia").ToString()

                            documento_.RutaCompleta = registroDocumento_.Item("Ruta Archivo Especial").ToString() & registroDocumento_.Item("Numero Referencia").ToString() & ".pdf"

                            documento_.DocumentoEncontrado = registroDocumento_.Item("Clave Archivo Especial Encontrado").ToString()

                            _documentosExtranet.Add(documento_)

                            If documento_.DocumentoEncontrado = IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo.No Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1015)

                                Return _estatus

                            Else

                                If Not BuscarArchivo(documento_) Then

                                    _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                    Return _estatus

                                End If

                            End If

                        Case IOperacionesDocumentoExtranet.TiposDocumentoExtranet.Administrativo

                            Dim documentoExtranet_ = New DocumentoExtranet

                            documentoExtranet_.ValorBusqueda = registroDocumento_.Item("Clave Factura").ToString()

                            documentoExtranet_.Folio = registroDocumento_.Item("Folio Factura").ToString()

                            documentoExtranet_.Clave = registroDocumento_.Item("Clave Documento").ToString()

                            Select Case documento_.TipoArchivoEsperado

                                Case IOperacionesDocumentoExtranet.TiposArchivosEsperado.PDF

                                    documentoExtranet_.RutaCompleta = registroDocumento_.Item("Ruta").ToString() & registroDocumento_.Item("Folio Factura").ToString() & ".pdf"

                                    If Not BuscarArchivo(documentoExtranet_) Then

                                        _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                        Return _estatus

                                    End If

                                Case IOperacionesDocumentoExtranet.TiposArchivosEsperado.XML

                                    documentoExtranet_.RutaCompleta = registroDocumento_.Item("Ruta").ToString() & registroDocumento_.Item("Folio Factura").ToString() & ".xml"

                                    If Not BuscarArchivo(documentoExtranet_) Then

                                        _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                        Return _estatus

                                    End If

                                Case IOperacionesDocumentoExtranet.TiposArchivosEsperado.ZIP

                                    Select Case documento_.ConsultarTipoClaseDocumento

                                        Case IOperacionesDocumentoExtranet.TipoClaseDocumento.DocumentosSoporte
                                            documentoExtranet_.RutaCompleta = registroDocumento_.Item("Ruta").ToString() & registroDocumento_.Item("Folio Factura").ToString() & ".zip"

                                        Case IOperacionesDocumentoExtranet.TipoClaseDocumento.ComplementosPago
                                            documentoExtranet_.RutaCompleta = registroDocumento_.Item("Ruta").ToString() & registroDocumento_.Item("Folio Factura").ToString() & "_CPG.zip"

                                        Case IOperacionesDocumentoExtranet.TipoClaseDocumento.ComplementosPagoTerceros
                                            documentoExtranet_.RutaCompleta = registroDocumento_.Item("Ruta").ToString() & registroDocumento_.Item("Folio Factura").ToString() & "_CPT.zip"

                                        Case Else
                                            documentoExtranet_.RutaCompleta = registroDocumento_.Item("Ruta").ToString() & registroDocumento_.Item("Folio Factura").ToString() & ".zip"

                                    End Select

                            End Select

                            ' DOCUMENTOS DE COMPLEMENTOS DE PAGO TERCEROS
                            If documento_.ConsultarTipoClaseDocumento = IOperacionesDocumentoExtranet.TipoClaseDocumento.ComplementosPagoTerceros Then

                                Dim operacionesCPT_ = New OperacionesCatalogo

                                With operacionesCPT_

                                    .EspacioTrabajo = _operaciones.EspacioTrabajo

                                    .CantidadVisibleRegistros = 0

                                    operacionesCPT_ = _sistema.ConsultaModulo(operacionesCPT_.EspacioTrabajo, "DigitalizacionComplementosPagoTerceros", sql_)

                                End With

                                If _sistema.TieneResultados(operacionesCPT_) Then

                                    Dim listaCPT_ = New List(Of DocumentoExtranet)

                                    For Each complementoPago_ As DataRow In operacionesCPT_.Vista.Tables(0).Rows

                                        'Dim complemento_ = New DocumentoExtranet

                                        'complemento_.Folio = complmentoPago_.Item("t_NombreDocumento").ToString()

                                        'complemento_.RutaCompleta = complmentoPago_.Item("t_RutaDocumento").ToString()

                                        'listaCPG_.Add(complemento_)


                                        ' Esta parte se debe activar cuando ya esten los PDFs tambien
                                        Dim nombreComplemento_ As String = Replace(complementoPago_.Item("t_NombreDocumentoNacional").ToString(), ".xml", "")

                                        Dim complementoPDF_ = New DocumentoExtranet

                                        complementoPDF_.Folio = nombreComplemento_ & ".pdf"

                                        complementoPDF_.RutaCompleta = complementoPago_.Item("t_RutaRepositorio").ToString() & complementoPDF_.Folio

                                        listaCPT_.Add(complementoPDF_)

                                        Dim complementoXML_ = New DocumentoExtranet

                                        complementoXML_.Folio = nombreComplemento_ & ".xml"

                                        complementoXML_.RutaCompleta = complementoPago_.Item("t_RutaRepositorio").ToString() & complementoXML_.Folio

                                        listaCPT_.Add(complementoXML_)

                                    Next

                                    documentoExtranet_.DocumentosSoporteExtranet = listaCPT_

                                    Dim zip_ = New ZipFile

                                    For Each complementoPago_ As DocumentoExtranet In documentoExtranet_.DocumentosSoporteExtranet

                                        If BuscarArchivo(complementoPago_) Then

                                            Dim b() As Byte = IO.File.ReadAllBytes(complementoPago_.RutaCompleta)

                                            zip_.AddEntry(complementoPago_.Folio, b)

                                        End If

                                    Next

                                    zip_.Save(documentoExtranet_.RutaCompleta)

                                    _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1016)

                                    Return _estatus

                                End If

                            End If

                            ' DOCUMENTOS DE COMPLEMENTOS DE PAGO
                            If documento_.ConsultarTipoClaseDocumento = IOperacionesDocumentoExtranet.TipoClaseDocumento.ComplementosPago Then

                                Dim operacionesCPG_ = New OperacionesCatalogo

                                With operacionesCPG_

                                    .EspacioTrabajo = _operaciones.EspacioTrabajo

                                    .CantidadVisibleRegistros = 0

                                    operacionesCPG_ = _sistema.ConsultaModulo(operacionesCPG_.EspacioTrabajo, "DigitalizacionComplementosPago", sql_)

                                End With

                                If _sistema.TieneResultados(operacionesCPG_) Then

                                    Dim listaCPG_ = New List(Of DocumentoExtranet)

                                    For Each complmentoPago_ As DataRow In operacionesCPG_.Vista.Tables(0).Rows

                                        Dim nombreComplemento_ As String = Replace(complmentoPago_.Item("t_NombreDocumento").ToString(), ".xml", "")

                                        Dim complementoPDF_ = New DocumentoExtranet

                                        complementoPDF_.Folio = nombreComplemento_ & ".pdf"

                                        complementoPDF_.RutaCompleta = complmentoPago_.Item("t_RepositorioDirectorio").ToString() & complementoPDF_.Folio

                                        listaCPG_.Add(complementoPDF_)

                                        Dim complementoXML_ = New DocumentoExtranet

                                        complementoXML_.Folio = nombreComplemento_ & ".xml"

                                        complementoXML_.RutaCompleta = complmentoPago_.Item("t_RepositorioDirectorio").ToString() & complementoXML_.Folio

                                        listaCPG_.Add(complementoXML_)

                                    Next

                                    documentoExtranet_.DocumentosSoporteExtranet = listaCPG_

                                    Dim zip_ = New ZipFile

                                    For Each complementoPago_ As DocumentoExtranet In documentoExtranet_.DocumentosSoporteExtranet

                                        If BuscarArchivo(complementoPago_) Then

                                            Dim b() As Byte = IO.File.ReadAllBytes(complementoPago_.RutaCompleta)

                                            zip_.AddEntry(complementoPago_.Folio, b)

                                        End If

                                    Next

                                    zip_.Save(documentoExtranet_.RutaCompleta)

                                    _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1016)

                                    Return _estatus

                                End If


                            End If

                            ' DOCUMENTOS DE PAGOS HECHO (SOPORTE)
                            If documento_.ConsultarTipoClaseDocumento = IOperacionesDocumentoExtranet.TipoClaseDocumento.DocumentosSoporte Then

                                Dim opSoportes_ = New OperacionesCatalogo

                                opSoportes_.EspacioTrabajo = _operaciones.EspacioTrabajo

                                opSoportes_.CantidadVisibleRegistros = 0

                                opSoportes_ = _sistema.ConsultaModulo(opSoportes_.EspacioTrabajo,
                                                                      "DigitalizacionSoportesFactura",
                                                                      sql_)

                                If _sistema.TieneResultados(opSoportes_) Then

                                    Dim listaSoportes_ = New List(Of DocumentoExtranet)

                                    For Each registrosSoportes_ As DataRow In opSoportes_.Vista.Tables(0).Rows

                                        Dim archivoMexicano_ As Integer

                                        archivoMexicano_ = registrosSoportes_.Item("Comprobante mexicano").ToString()

                                        If archivoMexicano_ = 1 Then

                                            Dim soporteXML_ = New DocumentoExtranet

                                            soporteXML_.Folio = registrosSoportes_.Item("Nombre Documento Nacional").ToString()

                                            soporteXML_.RutaCompleta = registrosSoportes_.Item("Directorio Archivo Nacional").ToString()

                                            listaSoportes_.Add(soporteXML_)

                                            If registrosSoportes_.Item("Estatus Representacion Impresa").ToString() = 1 Then

                                                Dim soportePDF_ = New DocumentoExtranet

                                                soportePDF_.Folio = registrosSoportes_.Item("Nombre Documento Nacional").ToString().Replace(".xml", ".pdf").Replace(".XML", ".pdf")

                                                soportePDF_.RutaCompleta = registrosSoportes_.Item("Directorio Archivo Nacional").ToString().Replace(".xml", ".pdf").Replace(".XML", ".pdf")

                                                listaSoportes_.Add(soportePDF_)

                                            End If

                                        Else

                                            Dim soportePDF_ = New DocumentoExtranet

                                            soportePDF_.Folio = registrosSoportes_.Item("Nombre Documento Extranjero").ToString()

                                            soportePDF_.RutaCompleta = registrosSoportes_.Item("Directorio Archivo Extranjero").ToString()

                                            listaSoportes_.Add(soportePDF_)

                                        End If

                                    Next

                                    documentoExtranet_.DocumentosSoporteExtranet = listaSoportes_

                                    Dim zip_ = New ZipFile

                                    For Each documentosSoporteFacturas_ As DocumentoExtranet In documentoExtranet_.DocumentosSoporteExtranet

                                        If BuscarArchivo(documentosSoporteFacturas_) Then

                                            Dim b() As Byte = IO.File.ReadAllBytes(documentosSoporteFacturas_.RutaCompleta)

                                            zip_.AddEntry(documentosSoporteFacturas_.Folio, b)

                                        End If

                                    Next

                                    zip_.Save(documentoExtranet_.RutaCompleta)

                                    _estatus.ObjectReturned = _documentosExtranetNoEncontrados

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1016)

                                    Return _estatus

                                End If

                            End If

                            _documentosExtranet.Add(documentoExtranet_)

                    End Select

                Next

            Else

                _documentosExtranet.Add(documento_)

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1015)

                Return _estatus

            End If

            Return _estatus

        End Function

        Public Function ObtenerDocumento(documentos_ As List(Of DocumentoExtranet), Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) As TagWatcher Implements IOperacionesDocumentoExtranet.ObtenerDocumento

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

            Return _estatus

        End Function

        Public Function BuscarArchivo(ByVal documento_ As DocumentoExtranet) As Boolean

            If File.Exists(documento_.RutaCompleta) Then

                Return True

            Else

                '_documentosExtranetNoEncontrados.Add(documento_)

                '_estatus.SetError(TagWatcher.ErrorTypes.C6_012_1015)

                Return False

            End If

        End Function

        Private Sub EscribirEnLog(ByVal t_Texto_ As String)

            Dim t_Ruta_ As String = "C:\logs\TestKBW JCCS " & Date.Now().ToString("yyyy-MM-dd") & ".txt"

            Dim escritor_ As StreamWriter

            Try

                escritor_ = File.AppendText(t_Ruta_)

                escritor_.WriteLine(t_Texto_)

                escritor_.Flush()

                escritor_.Close()

            Catch ex As Exception

            End Try

        End Sub

    End Class

End Namespace

