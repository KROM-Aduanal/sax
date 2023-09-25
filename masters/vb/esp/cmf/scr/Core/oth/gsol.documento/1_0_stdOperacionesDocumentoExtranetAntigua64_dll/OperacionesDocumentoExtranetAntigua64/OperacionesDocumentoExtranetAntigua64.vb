Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.Componentes.SistemaBase.GsDialogo
Imports System.IO.Compression
Imports System.IO

Namespace gsol.documento

    Public Class OperacionesDocumentoExtranetAntigua64

#Region "Atributos"

        Private _estatus As TagWatcher

        Private _operaciones As IOperacionesCatalogo

        Private _sistema As Organismo

        Private _documentosFacturas As List(Of DocumentosFacturas)

        Private _documentosOperativos As List(Of DocumentosOperativos)

#End Region

#Region "Propiedades"

        Public ReadOnly Property DocumentosFacturas As List(Of DocumentosFacturas)

            Get

                Return _documentosFacturas

            End Get

        End Property

        Public ReadOnly Property DocumentosOperativos As List(Of DocumentosOperativos)

            Get

                Return _documentosOperativos

            End Get

        End Property


        Public Property Operaciones As IOperacionesCatalogo

            Get

                Return _operaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _operaciones = value

            End Set

        End Property

        Public ReadOnly Property Estatus As TagWatcher

            Get

                Return _estatus

            End Get

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _operaciones = New OperacionesCatalogo

            _estatus = New TagWatcher

            _estatus.SetOK()

            _sistema = New Organismo

            _documentosFacturas = New List(Of DocumentosFacturas)

            _documentosOperativos = New List(Of DocumentosOperativos)

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

            Me.New()

            _operaciones = ioperaciones_

        End Sub

#End Region

        Public Function ObtenerDocumentoFacturas(ByVal documentosFacturas_ As List(Of DocumentosFacturas),
                                                 Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) As TagWatcher

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            For Each documento_ As DocumentosFacturas In documentosFacturas_

                Dim opDocumento_ = New OperacionesCatalogo

                opDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                opDocumento_.CantidadVisibleRegistros = 0

                Dim sql_ = " and i_Cve_Factura in (" & documento_.ClaveFactura & ")"

                opDocumento_ = _sistema.ConsultaModulo(opDocumento_.EspacioTrabajo,
                                                       "DigitalizacionFacturasAntigua",
                                                        sql_)

                If _sistema.TieneResultados(opDocumento_) Then

                    For Each registroDocumento_ As DataRow In opDocumento_.Vista.Tables(0).Rows

                        documento_.ClaveFactura = registroDocumento_.Item("Clave Factura").ToString()

                        documento_.FolioFactura = registroDocumento_.Item("Folio Factura").ToString()

                        documento_.ClaveDocumento = registroDocumento_.Item("Clave Documento").ToString()

                        documento_.Ruta = registroDocumento_.Item("Ruta").ToString()

                        _documentosFacturas.Add(documento_)

                    Next

                Else
                    _documentosFacturas.Add(documento_)

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1016)

                    Return _estatus

                End If

            Next

            Return _estatus

        End Function

        Public Function ObtenerDocumentacionCompletaOperativo(ByVal documentosOperativos_ As List(Of DocumentosOperativos),
                                                              Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) As TagWatcher

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            For Each documento_ As DocumentosOperativos In documentosOperativos_

                Dim opDocumento_ = New OperacionesCatalogo

                opDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                opDocumento_.CantidadVisibleRegistros = 0

                Dim sql_ = " and t_NumeroReferencia in ('" & documento_.NumeroReferencia & "')"

                opDocumento_ = _sistema.ConsultaModulo(opDocumento_.EspacioTrabajo,
                                                       "DigitalizacionArchivoEspecialAntigua",
                                                        sql_)

                If _sistema.TieneResultados(opDocumento_) Then

                    For Each registroDocumento_ As DataRow In opDocumento_.Vista.Tables(0).Rows

                        documento_.NumeroReferencia = registroDocumento_.Item("Numero Referencia").ToString()

                        documento_.Ruta = registroDocumento_.Item("Ruta Archivo Especial").ToString()

                        documento_.DocumentoEncontrado = registroDocumento_.Item("Clave Archivo Especial Encontrado").ToString()

                        If documento_.DocumentoEncontrado = 0 Then

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1015)

                            Return _estatus

                        End If

                        _documentosOperativos.Add(documento_)

                    Next

                Else

                    _documentosOperativos.Add(documento_)

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1017)

                    Return _estatus

                End If

            Next

            Return _estatus

        End Function

        Public Function ObtenerDocumentosFacturasSoporte() As Integer


            'Dim myProcess As New Process()

            'myProcess.StartInfo.UseShellExecute = True
            'myProcess.StartInfo.FileName = "cmd.exe"
            'myProcess.StartInfo.CreateNoWindow = True
            'myProcess.StartInfo.WorkingDirectory = "C:\"
            'myProcess.StartInfo.Arguments = "Rar_x64 a C:\PruebaZIP\PruebaZIP.zip C:\PruebaZIP\1.pdf"

            'myProcess.Start()

            Shell("cmd.exe /c A: cd PruebaZIP Rar_x64 a PruebaZIP.zip 1.pdf")
            Shell("cmd.exe /c A: cd PruebaZIP Rar_x64 a PruebaZIP.zip 2.pdf")
            Shell("cmd.exe /c A: cd PruebaZIP Rar_x64 a PruebaZIP.zip 3.pdf")


            Dim ARCHIVO As New FileInfo("C:\PruebaZIP\1.pdf")
            Dim ARCHIVO2 As New FileInfo("C:\PruebaZIP\2.pdf")
            Dim ARCHIVO3 As New FileInfo("C:\PruebaZIP\3.pdf")
            Dim ARCHIVO4 As New FileInfo("C:\PruebaZIP\4.pdf")
            Dim ARCHIVO5 As New FileInfo("C:\PruebaZIP\5.pdf")
            Dim ARCHIVO6 As New FileInfo("C:\PruebaZIP\6.pdf")
            Dim ARCHIVO7 As New FileInfo("C:\PruebaZIP\7.pdf")
            Dim ARCHIVO8 As New FileInfo("C:\PruebaZIP\8.pdf")
            Dim ARCHIVO9 As New FileInfo("C:\PruebaZIP\9.pdf")
            Dim ARCHIVO10 As New FileInfo("C:\PruebaZIP\10.pdf")
            Dim ARCHIVO11 As New FileInfo("C:\PruebaZIP\11.pdf")
            Dim ARCHIVO12 As New FileInfo("C:\PruebaZIP\12.pdf")
            Dim ARCHIVO13 As New FileInfo("C:\PruebaZIP\13.pdf")
            Dim ARCHIVO14 As New FileInfo("C:\PruebaZIP\14.pdf")
            Dim ARCHIVO15 As New FileInfo("C:\PruebaZIP\15.pdf")
            Dim ARCHIVO16 As New FileInfo("C:\PruebaZIP\16.pdf")
            Dim ARCHIVO17 As New FileInfo("C:\PruebaZIP\17.pdf")
            Dim ARCHIVO18 As New FileInfo("C:\PruebaZIP\18.pdf")
            Dim ARCHIVO19 As New FileInfo("C:\PruebaZIP\19.pdf")
            Dim ARCHIVO20 As New FileInfo("C:\PruebaZIP\20.pdf")
            Dim ARCHIVO21 As New FileInfo("C:\PruebaZIP\21.pdf")
            Dim ARCHIVO22 As New FileInfo("C:\PruebaZIP\22.pdf")
            Dim ARCHIVO23 As New FileInfo("C:\PruebaZIP\23.pdf")
            Dim ARCHIVO24 As New FileInfo("C:\PruebaZIP\24.pdf")
            Dim ARCHIVO25 As New FileInfo("C:\PruebaZIP\25.pdf")
            Dim ARCHIVO26 As New FileInfo("C:\PruebaZIP\26.pdf")
            Dim ARCHIVO27 As New FileInfo("C:\PruebaZIP\27.pdf")
            Dim ARCHIVO28 As New FileInfo("C:\PruebaZIP\28.pdf")
            Dim ARCHIVO29 As New FileInfo("C:\PruebaZIP\29.pdf")
            Dim ARCHIVO30 As New FileInfo("C:\PruebaZIP\30.pdf")
            Dim ARCHIVO31 As New FileInfo("C:\PruebaZIP\31.pdf")
            Dim ARCHIVO32 As New FileInfo("C:\PruebaZIP\32.pdf")
            Dim ARCHIVO33 As New FileInfo("C:\PruebaZIP\33.pdf")
            Dim ARCHIVO34 As New FileInfo("C:\PruebaZIP\34.pdf")
            Dim ARCHIVO35 As New FileInfo("C:\PruebaZIP\35.pdf")
            Dim ARCHIVO36 As New FileInfo("C:\PruebaZIP\36.pdf")
            Dim ARCHIVO37 As New FileInfo("C:\PruebaZIP\37.pdf")
            Dim ARCHIVO38 As New FileInfo("C:\PruebaZIP\38.pdf")
            Dim ARCHIVO39 As New FileInfo("C:\PruebaZIP\39.pdf")
            Dim ARCHIVO40 As New FileInfo("C:\PruebaZIP\40.pdf")
            Dim ARCHIVO41 As New FileInfo("C:\PruebaZIP\41.pdf")
            Dim ARCHIVO42 As New FileInfo("C:\PruebaZIP\42.pdf")
            Dim ARCHIVO43 As New FileInfo("C:\PruebaZIP\43.pdf")
            Dim ARCHIVO44 As New FileInfo("C:\PruebaZIP\44.pdf")
            Dim ARCHIVO45 As New FileInfo("C:\PruebaZIP\45.pdf")
            Dim ARCHIVO46 As New FileInfo("C:\PruebaZIP\46.pdf")
            Dim ARCHIVO47 As New FileInfo("C:\PruebaZIP\47.pdf")
            Dim ARCHIVO48 As New FileInfo("C:\PruebaZIP\48.pdf")
            Dim ARCHIVO49 As New FileInfo("C:\PruebaZIP\49.pdf")
            Dim ARCHIVO50 As New FileInfo("C:\PruebaZIP\50.pdf")
            Dim ARCHIVO51 As New FileInfo("C:\PruebaZIP\51.pdf")
            Dim ARCHIVO52 As New FileInfo("C:\PruebaZIP\52.pdf")
            Dim ARCHIVO53 As New FileInfo("C:\PruebaZIP\53.pdf")
            Dim ARCHIVO54 As New FileInfo("C:\PruebaZIP\54.pdf")
            Dim ARCHIVO55 As New FileInfo("C:\PruebaZIP\55.pdf")
            Dim ARCHIVO56 As New FileInfo("C:\PruebaZIP\56.pdf")
            Dim ARCHIVO57 As New FileInfo("C:\PruebaZIP\57.pdf")
            Dim ARCHIVO58 As New FileInfo("C:\PruebaZIP\58.pdf")
            Dim ARCHIVO59 As New FileInfo("C:\PruebaZIP\59.pdf")
            Dim ARCHIVO60 As New FileInfo("C:\PruebaZIP\60.pdf")
            Dim ARCHIVO61 As New FileInfo("C:\PruebaZIP\61.pdf")
            Dim ARCHIVO62 As New FileInfo("C:\PruebaZIP\62.pdf")
            Dim ARCHIVO63 As New FileInfo("C:\PruebaZIP\63.pdf")
            Dim ARCHIVO64 As New FileInfo("C:\PruebaZIP\64.pdf")

            Dim ENTRADA As FileStream = ARCHIVO.OpenRead
            Dim ENTRADA2 As FileStream = ARCHIVO2.OpenRead
            Dim ENTRADA3 As FileStream = ARCHIVO3.OpenRead
            Dim ENTRADA4 As FileStream = ARCHIVO4.OpenRead
            Dim ENTRADA5 As FileStream = ARCHIVO5.OpenRead
            Dim ENTRADA6 As FileStream = ARCHIVO6.OpenRead
            Dim ENTRADA7 As FileStream = ARCHIVO7.OpenRead
            Dim ENTRADA8 As FileStream = ARCHIVO8.OpenRead
            Dim ENTRADA9 As FileStream = ARCHIVO9.OpenRead
            Dim ENTRADA10 As FileStream = ARCHIVO10.OpenRead
            Dim ENTRADA11 As FileStream = ARCHIVO11.OpenRead
            Dim ENTRADA12 As FileStream = ARCHIVO12.OpenRead
            Dim ENTRADA13 As FileStream = ARCHIVO13.OpenRead
            Dim ENTRADA14 As FileStream = ARCHIVO14.OpenRead
            Dim ENTRADA15 As FileStream = ARCHIVO15.OpenRead
            Dim ENTRADA16 As FileStream = ARCHIVO16.OpenRead
            Dim ENTRADA17 As FileStream = ARCHIVO17.OpenRead
            Dim ENTRADA18 As FileStream = ARCHIVO18.OpenRead
            Dim ENTRADA19 As FileStream = ARCHIVO19.OpenRead
            Dim ENTRADA20 As FileStream = ARCHIVO20.OpenRead
            Dim ENTRADA21 As FileStream = ARCHIVO21.OpenRead
            Dim ENTRADA22 As FileStream = ARCHIVO22.OpenRead
            Dim ENTRADA23 As FileStream = ARCHIVO23.OpenRead
            Dim ENTRADA24 As FileStream = ARCHIVO24.OpenRead
            Dim ENTRADA25 As FileStream = ARCHIVO25.OpenRead
            Dim ENTRADA26 As FileStream = ARCHIVO26.OpenRead
            Dim ENTRADA27 As FileStream = ARCHIVO27.OpenRead
            Dim ENTRADA28 As FileStream = ARCHIVO28.OpenRead
            Dim ENTRADA29 As FileStream = ARCHIVO29.OpenRead
            Dim ENTRADA30 As FileStream = ARCHIVO30.OpenRead
            Dim ENTRADA31 As FileStream = ARCHIVO31.OpenRead
            Dim ENTRADA32 As FileStream = ARCHIVO32.OpenRead
            Dim ENTRADA33 As FileStream = ARCHIVO33.OpenRead
            Dim ENTRADA34 As FileStream = ARCHIVO34.OpenRead
            Dim ENTRADA35 As FileStream = ARCHIVO35.OpenRead
            Dim ENTRADA36 As FileStream = ARCHIVO36.OpenRead
            Dim ENTRADA37 As FileStream = ARCHIVO37.OpenRead
            Dim ENTRADA38 As FileStream = ARCHIVO38.OpenRead
            Dim ENTRADA39 As FileStream = ARCHIVO39.OpenRead
            Dim ENTRADA40 As FileStream = ARCHIVO40.OpenRead
            Dim ENTRADA41 As FileStream = ARCHIVO41.OpenRead
            Dim ENTRADA42 As FileStream = ARCHIVO42.OpenRead
            Dim ENTRADA43 As FileStream = ARCHIVO43.OpenRead
            Dim ENTRADA44 As FileStream = ARCHIVO44.OpenRead
            Dim ENTRADA45 As FileStream = ARCHIVO45.OpenRead
            Dim ENTRADA46 As FileStream = ARCHIVO46.OpenRead
            Dim ENTRADA47 As FileStream = ARCHIVO47.OpenRead
            Dim ENTRADA48 As FileStream = ARCHIVO48.OpenRead
            Dim ENTRADA49 As FileStream = ARCHIVO49.OpenRead
            Dim ENTRADA50 As FileStream = ARCHIVO50.OpenRead
            Dim ENTRADA51 As FileStream = ARCHIVO51.OpenRead
            Dim ENTRADA52 As FileStream = ARCHIVO52.OpenRead
            Dim ENTRADA53 As FileStream = ARCHIVO53.OpenRead
            Dim ENTRADA54 As FileStream = ARCHIVO54.OpenRead
            Dim ENTRADA55 As FileStream = ARCHIVO55.OpenRead
            Dim ENTRADA56 As FileStream = ARCHIVO56.OpenRead
            Dim ENTRADA57 As FileStream = ARCHIVO57.OpenRead
            Dim ENTRADA58 As FileStream = ARCHIVO58.OpenRead
            Dim ENTRADA59 As FileStream = ARCHIVO59.OpenRead
            Dim ENTRADA60 As FileStream = ARCHIVO60.OpenRead
            Dim ENTRADA61 As FileStream = ARCHIVO61.OpenRead
            Dim ENTRADA62 As FileStream = ARCHIVO62.OpenRead
            Dim ENTRADA63 As FileStream = ARCHIVO63.OpenRead
            Dim ENTRADA64 As FileStream = ARCHIVO64.OpenRead

            Using COMPRIMIDO As FileStream = File.Create("C:\PruebaZip\Prueba.gz")

                Using COMPRESION As GZipStream = New GZipStream(COMPRIMIDO, CompressionMode.Compress)

                    ENTRADA.CopyTo(COMPRESION)
                    ENTRADA2.CopyTo(COMPRESION)
                    ENTRADA3.CopyTo(COMPRESION)
                    ENTRADA4.CopyTo(COMPRESION)
                    ENTRADA5.CopyTo(COMPRESION)
                    ENTRADA6.CopyTo(COMPRESION)
                    ENTRADA7.CopyTo(COMPRESION)
                    ENTRADA8.CopyTo(COMPRESION)
                    ENTRADA9.CopyTo(COMPRESION)
                    ENTRADA10.CopyTo(COMPRESION)
                    ENTRADA11.CopyTo(COMPRESION)
                    ENTRADA12.CopyTo(COMPRESION)
                    ENTRADA13.CopyTo(COMPRESION)
                    ENTRADA14.CopyTo(COMPRESION)
                    ENTRADA15.CopyTo(COMPRESION)
                    ENTRADA16.CopyTo(COMPRESION)
                    ENTRADA17.CopyTo(COMPRESION)
                    ENTRADA18.CopyTo(COMPRESION)
                    ENTRADA19.CopyTo(COMPRESION)
                    ENTRADA20.CopyTo(COMPRESION)
                    ENTRADA21.CopyTo(COMPRESION)
                    ENTRADA22.CopyTo(COMPRESION)
                    ENTRADA23.CopyTo(COMPRESION)
                    ENTRADA24.CopyTo(COMPRESION)
                    ENTRADA25.CopyTo(COMPRESION)
                    ENTRADA26.CopyTo(COMPRESION)
                    ENTRADA27.CopyTo(COMPRESION)
                    ENTRADA28.CopyTo(COMPRESION)
                    ENTRADA29.CopyTo(COMPRESION)
                    ENTRADA30.CopyTo(COMPRESION)
                    ENTRADA31.CopyTo(COMPRESION)
                    ENTRADA32.CopyTo(COMPRESION)
                    ENTRADA33.CopyTo(COMPRESION)
                    ENTRADA34.CopyTo(COMPRESION)
                    ENTRADA35.CopyTo(COMPRESION)
                    ENTRADA36.CopyTo(COMPRESION)
                    ENTRADA37.CopyTo(COMPRESION)
                    ENTRADA38.CopyTo(COMPRESION)
                    ENTRADA39.CopyTo(COMPRESION)
                    ENTRADA40.CopyTo(COMPRESION)
                    ENTRADA41.CopyTo(COMPRESION)
                    ENTRADA42.CopyTo(COMPRESION)
                    ENTRADA43.CopyTo(COMPRESION)
                    ENTRADA44.CopyTo(COMPRESION)
                    ENTRADA45.CopyTo(COMPRESION)
                    ENTRADA46.CopyTo(COMPRESION)
                    ENTRADA47.CopyTo(COMPRESION)
                    ENTRADA48.CopyTo(COMPRESION)
                    ENTRADA49.CopyTo(COMPRESION)
                    ENTRADA50.CopyTo(COMPRESION)
                    ENTRADA51.CopyTo(COMPRESION)
                    ENTRADA52.CopyTo(COMPRESION)
                    ENTRADA53.CopyTo(COMPRESION)
                    ENTRADA54.CopyTo(COMPRESION)
                    ENTRADA55.CopyTo(COMPRESION)
                    ENTRADA56.CopyTo(COMPRESION)
                    ENTRADA57.CopyTo(COMPRESION)
                    ENTRADA58.CopyTo(COMPRESION)
                    ENTRADA59.CopyTo(COMPRESION)
                    ENTRADA60.CopyTo(COMPRESION)
                    ENTRADA61.CopyTo(COMPRESION)
                    ENTRADA62.CopyTo(COMPRESION)
                    ENTRADA63.CopyTo(COMPRESION)
                    ENTRADA64.CopyTo(COMPRESION)

                End Using

            End Using

            Return 1

        End Function

    End Class

    Public Class DocumentosFacturas

#Region "Atributos"

        Private _claveFactura As Integer

        Private _folioFactura As String

        Private _claveDocumento As Integer

        Private _ruta As String

#End Region

#Region "Propiedades"

        Public Property ClaveFactura As Integer

            Get

                Return _claveFactura

            End Get

            Set(value As Integer)

                _claveFactura = value

            End Set

        End Property

        Public Property ClaveDocumento As Integer

            Get

                Return _claveDocumento

            End Get

            Set(value As Integer)

                _claveDocumento = value

            End Set

        End Property

        Public Property FolioFactura As String

            Get

                Return _folioFactura

            End Get

            Set(value As String)

                _folioFactura = value

            End Set

        End Property

        Public Property Ruta As String

            Get

                Return _ruta

            End Get

            Set(value As String)

                _ruta = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _claveFactura = 0

            _folioFactura = ""

            _claveDocumento = 0

            _ruta = ""

        End Sub

#End Region

    End Class

    Public Class DocumentosOperativos

#Region "Atributos"

        Private _numeroReferencia As String

        Private _ruta As String

        Private _documentoEncontrado As Integer

#End Region

#Region "Propiedades"

        Public Property NumeroReferencia As String

            Get

                Return _numeroReferencia

            End Get

            Set(value As String)

                _numeroReferencia = value

            End Set

        End Property

        Public Property Ruta As String

            Get

                Return _ruta

            End Get

            Set(value As String)

                _ruta = value

            End Set

        End Property

        Public Property DocumentoEncontrado As Integer

            Get

                Return _documentoEncontrado

            End Get

            Set(value As Integer)

                _documentoEncontrado = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _numeroReferencia = ""

            _ruta = ""

            _documentoEncontrado = 0

        End Sub

#End Region

    End Class

End Namespace

