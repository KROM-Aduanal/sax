Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.Componentes.SistemaBase.GsDialogo
Imports System.IO
Imports gsol.documento
Imports gsol.correoelectronico
Imports System.Environment
Imports Limilabs.Mail.MIME
Imports gsol.Componentes.SistemaBase
Imports SautinSoft
Imports System.Drawing.Imaging
Imports System.Threading
Imports iTextSharp.text
'Imports iTextSharp.text.Document
Imports iTextSharp.text.pdf
Imports Ionic.Zip
Imports Limilabs.Mail
Imports Microsoft.Office.Interop.Word

Imports gsol
Imports gsol.BaseDatos

Namespace gsol.documento

    Public Class OperacionesDocumento64
        Implements IOperacionesDocumento

#Region "Atributos"

        Private _estatus As TagWatcher

        Private _operaciones As IOperacionesCatalogo

        Private _sistema As Organismo

        Private _documento As Documento

        Private _referencia As String

        Private _claveMaestroOperaciones As Integer

        Private _TipoCFDI As Boolean

        Public _archivosReprocesado As Dictionary(Of String, String)

        Public _contador As Integer = 0

#End Region

#Region "Propiedades"

        Public Property Documento As Documento Implements IOperacionesDocumento.Documento

            Get

                Return _documento

            End Get

            Set(value As Documento)

                _documento = value

            End Set

        End Property

        Public Property Operaciones As IOperacionesCatalogo Implements IOperacionesDocumento.Operaciones

            Get

                Return _operaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _operaciones = value

            End Set

        End Property

        Public ReadOnly Property Estatus As TagWatcher Implements IOperacionesDocumento.Estatus

            Get

                Return _estatus

            End Get

        End Property

        Public Property Referencia As String

            Get

                Return _referencia

            End Get

            Set(value As String)

                _referencia = value

            End Set

        End Property

        Public Property ClaveMaestroOperaciones As Integer

            Get

                Return _claveMaestroOperaciones

            End Get

            Set(value As Integer)

                _claveMaestroOperaciones = value

            End Set

        End Property


#End Region

#Region "Constructores"

        Sub New()

            _operaciones = New OperacionesCatalogo

            _estatus = New TagWatcher

            _estatus.SetOK()

            _sistema = New Organismo

            _documento = New Documento

            _referencia = Nothing

            _claveMaestroOperaciones = 0

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

            Me.New()

            _operaciones = ioperaciones_

            _referencia = Nothing

            _claveMaestroOperaciones = 0

        End Sub

#End Region

#Region "Métodos"

        Public Sub ProcesarDocumentoCorreoElectronico(ByVal correoElectronico_ As MensajeCorreoElectronico,
                                                      ByVal attachment_ As MimeData,
                                                      Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            Dim extension_ = Path.GetExtension(attachment_.FileName)

            If extension_ Is Nothing Then

                Return

            End If

            Dim tipoComprimidos_ = New List(Of String) From {".zip", ".rar"}

            If tipoComprimidos_.Contains(extension_.ToLower) Then

                _archivosReprocesado = New Dictionary(Of String, String)

                ProcesaComprimidoCorreo(correoElectronico_, attachment_)

            Else

                _contador += 1

                _documento.ClaveDivisionMiEmpresa = correoElectronico_.ClaveDivisionMiEmpresa

                _documento.TipoVinculacion = gsol.documento.Documento.TiposVinculacion.CorreoElectronico

                _documento.RutaDocumentoOrigen = EliminarCaracteresEnTexto(attachment_.FileName)

                BuscarPlantillaDocumentosCorreoElectronico(attachment_)

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    CargarRepositoriosCorreoElectronico()

                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                        CargarPlantilla()

                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                            LlenarCaracteristicasCorreoElectronico(correoElectronico_)

                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                ValidarCaracteristicasPlantilla()

                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                    CrearDirectorio()

                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                        GuardarDocumentoCorreoElectronico(attachment_)

                                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                            InsertarRegistros()

                                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                InsertarRegistrosCaracteristicas()

                                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                    InsertarRegistroVinculacion(correoElectronico_.Clave)

                                                End If

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If

                End If

            End If

        End Sub

        Public Sub ProcesaComprimidoCorreo(ByVal correoElectronico_ As MensajeCorreoElectronico,
                                                      ByVal attachment_ As MimeData,
                                                      Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            Dim path_ As String = "C:\Temp\ContenidoZip\"

            Dim extension_ = Path.GetExtension(attachment_.FileName)

            Try

                My.Computer.FileSystem.DeleteDirectory(path_, FileIO.DeleteDirectoryOption.DeleteAllContents)

                If Not Directory.Exists(path_) Then

                    My.Computer.FileSystem.CreateDirectory(path_)

                End If

            Catch ex As Exception

                Try

                    If Not Directory.Exists(path_) Then

                        My.Computer.FileSystem.CreateDirectory(path_)

                    End If

                Catch exx As Exception

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1052)

                End Try

            End Try

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                attachment_.Save(path_ + attachment_.FileName)

                If extension_ = ".zip" Then

                    ProcesaZipCorreo(correoElectronico_, attachment_, path_)

                Else

                    ProcesaRar(correoElectronico_, attachment_, path_)

                End If

            End If

        End Sub

        Sub ProcesaRar(ByVal correoElectronico_ As MensajeCorreoElectronico,
                             ByVal attachment_ As MimeData,
                             ByVal path_ As String, Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            Dim rand_ = New Random()

            Dim WinRarPath As String = "C:\Program Files\WinRar\"

            Dim rarIt_ As String

            Dim nuevoZip_ As String = Path.GetFileNameWithoutExtension(attachment_.FileName) & ".zip"

            Try

                rarIt_ = Shell(WinRarPath & "WinRar.exe cv -y " & (path_ + attachment_.FileName), AppWinStyle.NormalFocus)

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1050)

                Return

            End Try

            System.Threading.Thread.Sleep(255)

            Dim operacionesDoc_ = New OperacionesDocumento64(_operaciones.Clone)

            Dim operacionesCorreo_ = New OperacionesCorreoElectronico64

            Try

                Dim proEmail_ As New MailBuilder()

                Dim documentoAdjunto_ As MimeData

                documentoAdjunto_ = proEmail_.AddAttachment(path_ & nuevoZip_.ToString)

                operacionesDoc_.ProcesarDocumentoCorreoElectronico(correoElectronico_, documentoAdjunto_)

                If operacionesDoc_.Estatus.Status = TagWatcher.TypeStatus.Errors Then

                    operacionesCorreo_.BitacoraCorreoElectronicoEntradaDocumentos(correoElectronico_:=correoElectronico_,
                                                                        tagWatcher_:=operacionesDoc_.Estatus,
                                                                        observaciones_:=attachment_.FileName)

                End If

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1053)

                Return

            End Try


            Try

                My.Computer.FileSystem.DeleteFile(path_ + attachment_.FileName)

            Catch ex As Exception

                '_estatus.SetError(TagWatcher.ErrorTypes.C6_012_1060)

                Return

            End Try

        End Sub

        Sub ProcesaZipCorreo(ByVal correoElectronico_ As MensajeCorreoElectronico,
                             ByVal attachment_ As MimeData,
                             ByVal path_ As String, Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            Dim rand_ = New Random()

            Dim temporal_ As String = rand_.Next().ToString + "\"

            Dim exclusiones_ = New List(Of String) From {".exe", ".bat"}

            Dim archivosZip_ As List(Of String) = New List(Of String)

            Try

                Using zip1_ As ZipFile = ZipFile.Read((path_ + attachment_.FileName))

                    Dim e As ZipEntry

                    For Each e In zip1_

                        If Not exclusiones_.Contains((Path.GetExtension(e.FileName)).ToLower) Then

                            e.Extract(path_ + temporal_, ExtractExistingFileAction.OverwriteSilently)

                            archivosZip_.Add(e.FileName)

                        End If

                    Next

                End Using

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1054)

                Return

            End Try

            Dim operacionesDoc_ = New OperacionesDocumento64(_operaciones.Clone)

            Dim operacionesCorreo_ = New OperacionesCorreoElectronico64

            For Each nombre_ In archivosZip_

                Try

                    If File.Exists(path_ + temporal_ + nombre_) Then

                        Dim proEmail_ As New MailBuilder()

                        Dim documentoAdjunto_ As MimeData

                        documentoAdjunto_ = proEmail_.AddAttachment(path_ & temporal_ & nombre_.ToString)

                        operacionesDoc_.ProcesarDocumentoCorreoElectronico(correoElectronico_, documentoAdjunto_)

                        If operacionesDoc_.Estatus.Status = TagWatcher.TypeStatus.Errors Then

                            operacionesCorreo_.BitacoraCorreoElectronicoEntradaDocumentos(correoElectronico_:=correoElectronico_,
                                                                           tagWatcher_:=operacionesDoc_.Estatus,
                                                                           observaciones_:=nombre_.ToString)

                            _archivosReprocesado.Add(nombre_.ToString, operacionesDoc_.Estatus.ErrorDescription.ToString & " " & nombre_.ToString)

                        End If

                    End If

                    '_contador += operacionesDoc_._contador

                Catch ex As Exception

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1051)

                    Return

                End Try

            Next

            _contador += operacionesDoc_._contador

            Try

                My.Computer.FileSystem.DeleteDirectory(path_ + temporal_, FileIO.DeleteDirectoryOption.DeleteAllContents)

                My.Computer.FileSystem.DeleteFile(path_ + attachment_.FileName)

            Catch ex As Exception

                '_estatus.SetError(TagWatcher.ErrorTypes.C6_012_1060)

            End Try

        End Sub

        Private Sub BuscarPlantillaDocumentosCorreoElectronico(ByVal attachment_ As MimeData)

            Dim extension_ = Path.GetExtension(attachment_.FileName)

            Select Case extension_.ToLower

                Case ".csv"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 702

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 723

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 734

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 745

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 756

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 767

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 778

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".doc"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 703

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 722

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 733

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 744

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 755

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 766

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 777

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".docx"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 704

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 721

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 732

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 743

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 754

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 765

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 776

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".jpeg"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 705

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 720

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 731

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 742

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 753

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 764

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 775

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".jpg"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 706

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 719

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 730

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 741

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 752

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 763

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 774

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".pdf"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 707

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 718

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 729

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 740

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 751

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 762

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 773

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".png"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 708

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 717

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 728

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 739

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 750

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 761

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 772

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".xls"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 709

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 716

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 727

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 738

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 749

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 760

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 771

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".xlsx"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 710

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 715

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 726

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 737

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 748

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 759

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 770

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".xml"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 711

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 714

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 725

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 736

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 747

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 758

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 769

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".zip"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 712

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 713

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 724

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 735

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 746

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 757

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 768

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case ".xlsb"

                    Select Case _documento.ClaveDivisionMiEmpresa

                        Case 1

                            _documento.PlantillaDocumento.ClavePlantilla = 1941 '1941 1825 'PRO PRU

                        Case 3

                            _documento.PlantillaDocumento.ClavePlantilla = 1942 '1942 1826 'PRO PRU

                        Case 6

                            _documento.PlantillaDocumento.ClavePlantilla = 1943 '1943 1827 'PRO PRU

                        Case 7

                            _documento.PlantillaDocumento.ClavePlantilla = 1944 '1944 1828 'PRO PRU

                        Case 8

                            _documento.PlantillaDocumento.ClavePlantilla = 1945 '1945 1829 'PRO PRU

                        Case 9

                            _documento.PlantillaDocumento.ClavePlantilla = 1946 '1946 1830 'PRO PRU

                        Case 113

                            _documento.PlantillaDocumento.ClavePlantilla = 1947 '1947 1831 'PRO PRU

                        Case Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

                    End Select

                Case Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1039)

            End Select

        End Sub

        Private Sub CargarRepositoriosCorreoElectronico()

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1

            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "PlantillasDocumentosGeneral",
                                                          " and i_cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & "" &
                                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    _documento.ClaveTipoDocumento = fila_.Item("Clave TipoDocumento").ToString

                    _documento.TipoDocumento = fila_.Item("TipoDocumento").ToString

                Next

            End If

            If Not _documento.ClaveTipoDocumento = 0 Then

                Dim tipoDocumento_ = New OperacionesCatalogo

                tipoDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                tipoDocumento_.CantidadVisibleRegistros = 1

                tipoDocumento_ = _sistema.ConsultaModulo(tipoDocumento_.EspacioTrabajo,
                                                         "TiposDocumentos",
                                                         " and i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento)

                If _sistema.TieneResultados(tipoDocumento_) Then

                    For Each fila_ As DataRow In tipoDocumento_.Vista.Tables(0).Rows

                        Dim repositorioDigital_ = New OperacionesCatalogo

                        repositorioDigital_.EspacioTrabajo = _operaciones.EspacioTrabajo

                        repositorioDigital_.CantidadVisibleRegistros = 1

                        repositorioDigital_ = _sistema.ConsultaModulo(repositorioDigital_.EspacioTrabajo,
                                                                      "RepositoriosDigitalesGeneral",
                                                                      " and i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento &
                                                                      " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

                        If _sistema.TieneResultados(repositorioDigital_) Then

                            For Each fila2_ As DataRow In repositorioDigital_.Vista.Tables(0).Rows

                                _documento.RutaDocumento = Nothing

                                _documento.RutaDocumentoCompleto = fila2_.Item("Servidor").ToString & fila2_.Item("Ruta").ToString & "\"

                                _documento.RutaDocumentoContingencia = Nothing

                                _documento.RutaDocumentoContingenciaCompleto = fila2_.Item("Servidor contingencia").ToString & fila2_.Item("Ruta contingencia").ToString & "\"

                                _documento.ClaveRepositorioDigital = fila2_.Item("Clave").ToString

                                _documento.TipoArchivo = fila_.Item("Clave tipo de archivo").ToString

                                _documento.Extension = Path.GetExtension(_documento.RutaDocumentoOrigen).ToLower()

                                _documento.NombreDocumento = Path.GetFileNameWithoutExtension(_documento.RutaDocumentoOrigen)

                                If Not fila_.Item("Extensión").ToString = _documento.Extension Then

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1009)

                                End If

                            Next

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1002)

                        End If

                    Next

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1001)

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1000)

            End If

        End Sub

        Private Sub LlenarCaracteristicasCorreoElectronico(ByVal correoElectronico_ As MensajeCorreoElectronico)

            Dim caracteristicasDocumentos_ = New SortedDictionary(Of Integer, CaracteristicaDocumento)

            If _documento.PlantillaDocumento.CaracteristicasDocumentos.Count > 0 Then

                For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                    Dim orden_ As Integer = elemento_.Key

                    Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                    Select Case caracteristicaDocumento_.ClaveCaracteristica

                        Case 115

                            caracteristicaDocumento_.Valor = correoElectronico_.FechaEnvioRecibido.Day

                        Case 114

                            caracteristicaDocumento_.Valor = correoElectronico_.FechaEnvioRecibido.Month

                        Case 113

                            caracteristicaDocumento_.Valor = correoElectronico_.SenderFrom

                        Case 112

                            caracteristicaDocumento_.Valor = correoElectronico_.FechaEnvioRecibido

                        Case 93

                            caracteristicaDocumento_.Valor = correoElectronico_.FechaEnvioRecibido.Year

                    End Select

                    caracteristicasDocumentos_.Add(orden_, caracteristicaDocumento_)

                Next

                _documento.PlantillaDocumento.CaracteristicasDocumentos = caracteristicasDocumentos_

            End If

        End Sub

        Private Sub GuardarDocumentoCorreoElectronico(ByVal attachment_ As MimeData)

            Dim nombreDocumento_ = _documento.NombreDocumento

            Dim contador_ = 2

            Try

                '.Replace("M:", "\\10.66.3.1\KromBase")

                If Not Directory.Exists(_documento.RutaDocumentoCompleto) Then

                    My.Computer.FileSystem.CreateDirectory(_documento.RutaDocumentoCompleto)

                End If

                While True

                    If File.Exists(_documento.RutaDocumentoCompleto & "" & nombreDocumento_ & _documento.Extension) Then

                        nombreDocumento_ = _documento.NombreDocumento & "_" & contador_

                        contador_ = contador_ + 1

                    Else

                        _documento.NombreDocumento = nombreDocumento_ & _documento.Extension

                        attachment_.Save(_documento.RutaDocumentoCompleto & "" & _documento.NombreDocumento)

                        Exit While

                    End If

                End While

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1007)

            End Try

            Try

                'Guardar en contingencia
                'If Not Directory.Exists(_documento.RutaDocumentoContingenciaCompleto) Then

                'My.Computer.FileSystem.CreateDirectory(_documento.RutaDocumentoContingenciaCompleto)

                ' End If

                'System.IO.File.Copy(_documento.RutaDocumentoOrigen,
                '                   _documento.RutaDocumentoContingenciaCompleto & "" & _documento.NombreDocumento)

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1008)

            End Try

        End Sub

        Public Sub ConvertirExcelAPDF(ByVal rutaDocumentoAConvertir_ As String,
                                      Optional ByVal rutaDocumentoAGuardar_ As String = "")

            _estatus.SetOK()

            Dim aplicacionExcel_ As Microsoft.Office.Interop.Excel.Application

            aplicacionExcel_ = New Microsoft.Office.Interop.Excel.Application()

            Dim libroExcel_ As Microsoft.Office.Interop.Excel.Workbook = Nothing

            Dim tipoFormato_ As Microsoft.Office.Interop.Excel.XlFixedFormatType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF

            Dim extension_ As String = Split(rutaDocumentoAConvertir_, ".").Last

            Dim rutaPDF_ As String = Replace(rutaDocumentoAConvertir_, extension_, "pdf")

            If rutaDocumentoAGuardar_ <> "" Then

                rutaPDF_ = rutaDocumentoAGuardar_

            End If

            '  xlQualityMinimum == smallest file size
            Dim calidad_ As Microsoft.Office.Interop.Excel.XlFixedFormatQuality = Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityMinimum

            Dim propiedadesDocumento_ As Boolean = True

            Dim ignorarAreasImpresion_ As Boolean = True

            Dim abrirAlGuardar_ As Boolean = False

            Try

                libroExcel_ = aplicacionExcel_.Workbooks.Open(rutaDocumentoAConvertir_)

                Dim numeroPaginas_ As Int16 = libroExcel_.Sheets.VPageBreaks.Count + 2

                ' Ensure that it fits to one page 
                Dim hojaExcel_ As Microsoft.Office.Interop.Excel.Worksheet

                hojaExcel_ = aplicacionExcel_.Worksheets(1)

                hojaExcel_.PageSetup.FitToPagesWide = numeroPaginas_

                hojaExcel_.PageSetup.FitToPagesTall = numeroPaginas_

                hojaExcel_.PageSetup.Zoom = False

                If Not libroExcel_ Is Nothing Then

                    libroExcel_.ExportAsFixedFormat(tipoFormato_,
                                                    rutaPDF_,
                                                    calidad_,
                                                    propiedadesDocumento_,
                                                    ignorarAreasImpresion_,
                                                    Type.Missing,
                                                    Type.Missing,
                                                    abrirAlGuardar_)

                    _estatus.ObjectReturned = rutaPDF_


                End If

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1036)

            Finally

                If Not libroExcel_ Is Nothing Then

                    libroExcel_.Close(False)

                    libroExcel_ = Nothing

                End If

                If Not aplicacionExcel_ Is Nothing Then

                    aplicacionExcel_.DisplayAlerts = False

                    aplicacionExcel_.Quit()

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(aplicacionExcel_)

                    aplicacionExcel_ = Nothing

                End If

                GC.Collect()

                GC.WaitForPendingFinalizers()

            End Try

        End Sub

        Public Sub ConvertirWordAPDF(ByVal rutaDocumentoAConvertir_ As String,
                                      Optional ByVal rutaDocumentoAGuardar_ As String = "")

            _estatus.SetOK()

            Dim word_ As Microsoft.Office.Interop.Word.Application = New Microsoft.Office.Interop.Word.Application()

            Dim doc_ As Microsoft.Office.Interop.Word.Document = word_.Documents.Open(rutaDocumentoAConvertir_)

            Dim extension_ As String = Split(rutaDocumentoAConvertir_, ".").Last

            Dim rutaPDF_ As String = Replace(rutaDocumentoAConvertir_, extension_, "pdf")

            If rutaDocumentoAGuardar_ <> "" Then

                rutaPDF_ = rutaDocumentoAGuardar_

            End If

            Try

                doc_.Activate()

                doc_.SaveAs2(rutaPDF_, WdSaveFormat.wdFormatPDF)

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1055)

            Finally

                If Not doc_ Is Nothing Then

                    doc_.Close(False)

                End If

                If Not word_ Is Nothing Then

                    word_.DisplayAlerts = False

                    word_.Quit()

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(word_)

                End If

                GC.Collect()

                GC.WaitForPendingFinalizers()

            End Try

        End Sub

        Public Sub ConvertirXmlaPDF(ByVal rutaDocumentoAConvertir_ As String, Optional ByVal rutaDocumentoAGuardar_ As String = "")

            _estatus.SetOK()

            Dim extension_ As String = Split(rutaDocumentoAConvertir_, ".").Last

            Dim rutaPDF_ As String = Replace(rutaDocumentoAConvertir_, extension_, "pdf")

            If rutaDocumentoAGuardar_ <> "" Then

                rutaPDF_ = rutaDocumentoAGuardar_

            End If

            Dim pdfDoc_ As New iTextSharp.text.Document()

            Dim fileReader_ As String = My.Computer.FileSystem.ReadAllText(rutaDocumentoAConvertir_, System.Text.Encoding.UTF8)

            Dim pdf_ As PdfWriter = PdfWriter.GetInstance(pdfDoc_, New System.IO.FileStream(rutaPDF_, FileMode.Create))

            Try

                pdfDoc_.Open()

                pdfDoc_.Add(New iTextSharp.text.Paragraph(fileReader_, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

                pdfDoc_.NewPage()

                pdf_.Flush()

                pdfDoc_.Close()

            Catch ex As Exception

                If File.Exists(rutaPDF_) Then

                    If pdfDoc_.IsOpen Then pdfDoc_.Close()

                    File.Delete(rutaPDF_)

                End If

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1056)

            Finally

                pdf_ = Nothing

                GC.Collect()

                GC.WaitForPendingFinalizers()

            End Try

        End Sub

        Public Sub ConvertirImagenaPDF(ByVal rutaDocumentoAConvertir_ As String, Optional ByVal rutaDocumentoAGuardar_ As String = "")

            _estatus.SetOK()

            Dim extension_ As String = Split(rutaDocumentoAConvertir_, ".").Last

            Dim rutaPDF_ As String = Replace(rutaDocumentoAConvertir_, extension_, "pdf")

            If rutaDocumentoAGuardar_ <> "" Then

                rutaPDF_ = rutaDocumentoAGuardar_

            End If

            Dim documento_ As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 0, 0, 0, 0)

            Try

                iTextSharp.text.pdf.PdfWriter.GetInstance(documento_, New FileStream(rutaPDF_, FileMode.Create))

                Dim image_ As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(rutaDocumentoAConvertir_)

                documento_.Open()

                documento_.NewPage()

                image_.ScaleToFit(documento_.PageSize.Width, documento_.PageSize.Height)

                image_.Alignment = iTextSharp.text.Image.ALIGN_CENTER

                documento_.Add(image_)

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1057)

            Finally

                documento_.Close()

                GC.Collect()

                GC.WaitForPendingFinalizers()

            End Try

        End Sub

        Public Sub ProcesarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.ProcesarDocumento

            _estatus.SetOK()

            _estatus.ObjectReturned = "golA"

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            BuscarPrivilegioUsuarioDocumento()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                CargarRepositorios()

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    ValidarCaracteristicasPlantilla()

                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                        ValidarDocumentosDuplicados()

                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                            CrearDirectorio()

                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                GuardarDocumento()

                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                    InsertarRegistros()

                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                        InsertarRegistrosCaracteristicas()

                                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                            InsertarRegistroVinculacion()

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If

                End If

            End If

        End Sub

        Private Sub CargarRepositorios() Implements IOperacionesDocumento.CargarRepositorios

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1

            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "PlantillasDocumentos",
                                                          " and i_cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & "" &
                                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    _documento.ClaveTipoDocumento = fila_.Item("Clave TipoDocumento").ToString

                    _documento.TipoDocumento = fila_.Item("TipoDocumento").ToString

                Next

            End If

            If Not _documento.ClaveTipoDocumento = 0 Then

                Dim tipoDocumento_ = New OperacionesCatalogo

                tipoDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                tipoDocumento_.CantidadVisibleRegistros = 1

                tipoDocumento_ = _sistema.ConsultaModulo(tipoDocumento_.EspacioTrabajo,
                                                         "TiposDocumentos",
                                                         " and i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento)

                If _sistema.TieneResultados(tipoDocumento_) Then

                    For Each fila_ As DataRow In tipoDocumento_.Vista.Tables(0).Rows

                        Dim repositorioDigital_ = New OperacionesCatalogo

                        repositorioDigital_.EspacioTrabajo = _operaciones.EspacioTrabajo

                        repositorioDigital_.CantidadVisibleRegistros = 1

                        repositorioDigital_ = _sistema.ConsultaModulo(repositorioDigital_.EspacioTrabajo,
                                                                      "RepositoriosDigitales",
                                                                      " and i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento &
                                                                      " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

                        If _sistema.TieneResultados(repositorioDigital_) Then

                            For Each fila2_ As DataRow In repositorioDigital_.Vista.Tables(0).Rows

                                _documento.RutaDocumento = Nothing

                                _documento.RutaDocumentoCompleto = fila2_.Item("Servidor").ToString & fila2_.Item("Ruta").ToString & "\"

                                _documento.RutaDocumentoContingencia = Nothing

                                _documento.RutaDocumentoContingenciaCompleto = fila2_.Item("Servidor contingencia").ToString & fila2_.Item("Ruta contingencia").ToString & "\"

                                _documento.ClaveRepositorioDigital = fila2_.Item("Clave").ToString

                                _documento.TipoArchivo = fila_.Item("Clave tipo de archivo").ToString

                                _documento.Extension = Path.GetExtension(_documento.RutaDocumentoOrigen).ToLower()

                                _documento.NombreDocumento = Path.GetFileNameWithoutExtension(_documento.RutaDocumentoOrigen)

                                If Not fila_.Item("Extensión").ToString = _documento.Extension Then

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1009)

                                End If

                            Next

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1002)

                        End If

                    Next

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1001)

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1000)

            End If

        End Sub

        Public Sub CargarPlantilla() Implements IOperacionesDocumento.CargarPlantilla

            _documento.PlantillaDocumento.CaracteristicasDocumentos.Clear()

            Dim plantilla_ = New OperacionesCatalogo

            plantilla_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantilla_.CantidadVisibleRegistros = 1

            plantilla_ = _sistema.ConsultaModulo(plantilla_.EspacioTrabajo,
                                                 "ResumenPlantillasCaracteristicas",
                                                     " and i_Cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & "" &
                                                     " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

            If _sistema.TieneResultados(plantilla_) Then

                For Each fila_ As DataRow In plantilla_.Vista.Tables(0).Rows

                    _documento.PlantillaDocumento.ClavePlantilla = fila_.Item("i_Cve_EncPlantillaDocumento").ToString

                    _documento.PlantillaDocumento.NombrePlantilla = fila_.Item("t_Nombre").ToString

                    _documento.PlantillaDocumento.ClaveTipoDocumentoPlantilla = fila_.Item("i_Cve_TipoPlantillaDocumento").ToString

                    Dim caracteristicaDocumento_ = New CaracteristicaDocumento

                    With caracteristicaDocumento_

                        caracteristicaDocumento_.ClaveCaracteristica = fila_.Item("i_Cve_CaracteristicaDocumento").ToString

                        caracteristicaDocumento_.TipoCaracteristica = fila_.Item("t_Tipo").ToString

                        caracteristicaDocumento_.ClaveTipoCaracteristica = fila_.Item("i_Cve_Tipo").ToString

                        caracteristicaDocumento_.ClaveCaracteristicaPrimaria = IIf(fila_.Item("i_cve_CaracteristicaPrimaria").ToString = "", 0, fila_.Item("i_cve_CaracteristicaPrimaria").ToString)

                        caracteristicaDocumento_.ClaveFormatoCaracteristica = fila_.Item("i_Cve_FormatoCaracteristica").ToString

                        caracteristicaDocumento_.ClaveDetallePlantilla = fila_.Item("i_Cve_DetPlantillaDocumento").ToString

                        caracteristicaDocumento_.Orden = fila_.Item("i_Orden").ToString

                        caracteristicaDocumento_.CaracteristicaRequerida = IIf(fila_.Item("i_Requerida").ToString, 1, 0)

                        caracteristicaDocumento_.AnchoVisual = fila_.Item("i_AnchoVisual").ToString

                        caracteristicaDocumento_.CaracteristicaRutaDinamica = IIf(fila_.Item("i_RutaDinamica").ToString, 1, 0)

                        caracteristicaDocumento_.CaracteristicaRevision = IIf(fila_.Item("i_Revision").ToString, 1, 0)

                        caracteristicaDocumento_.CaracteristicaRenombrarArchivo = IIf(fila_.Item("i_RenombrarArchivo").ToString, 1, 0)

                        caracteristicaDocumento_.TipoDatoCaracteristica = fila_.Item("i_Cve_FormatoCaracteristica").ToString

                        caracteristicaDocumento_.ClaveCaracteristicaSecundaria = IIf(fila_.Item("i_cve_CaracteristicaDocumentoSecundaria").ToString = "", 0, fila_.Item("i_cve_CaracteristicaDocumentoSecundaria").ToString)

                        caracteristicaDocumento_.TituloCaracteristica = fila_.Item("t_TituloCaracteristica").ToString

                        caracteristicaDocumento_.Titulo = fila_.Item("t_Titulo").ToString

                        caracteristicaDocumento_.TituloSecundaria = fila_.Item("t_TituloSecundaria").ToString

                        caracteristicaDocumento_.DisplayField = fila_.Item("t_DisplayField").ToString

                        caracteristicaDocumento_.KeyField = fila_.Item("t_KeyField").ToString

                        caracteristicaDocumento_.NameAsKey = fila_.Item("t_NameAsKey").ToString

                        caracteristicaDocumento_.PermissionNumber = fila_.Item("t_PermissionNumber").ToString

                    End With

                    _documento.PlantillaDocumento.CaracteristicasDocumentos.Add(fila_.Item("i_Orden").ToString,
                                                                                caracteristicaDocumento_)

                Next

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1003)

            End If

        End Sub

        Private Sub ValidarCaracteristicasPlantilla() Implements IOperacionesDocumento.ValidarCaracteristicasPlantilla

            Dim valorRenombrar_ = 1

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                If caracteristicaDocumento_.CaracteristicaRequerida = CaracteristicaDocumento.Requerida.Si And
                  ((caracteristicaDocumento_.Valor Is Nothing And Not caracteristicaDocumento_.TipoDatoCaracteristica = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Or
                    caracteristicaDocumento_.ValorClaveCatalogo Is Nothing And caracteristicaDocumento_.TipoDatoCaracteristica = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Then

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1017)

                    Exit For

                End If

                If caracteristicaDocumento_.CaracteristicaRutaDinamica = CaracteristicaDocumento.RutaDinamica.Si Then

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                            _documento.RutaDocumentoCompleto = _documento.RutaDocumentoCompleto & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumentoContingenciaCompleto = _documento.RutaDocumentoContingenciaCompleto & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumento = _documento.RutaDocumento & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumentoContingencia = _documento.RutaDocumentoContingencia & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                            _documento.RutaDocumentoCompleto = _documento.RutaDocumentoCompleto & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumentoContingenciaCompleto = _documento.RutaDocumentoContingenciaCompleto & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumento = _documento.RutaDocumento & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumentoContingencia = _documento.RutaDocumentoContingencia & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero, CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                            _documento.RutaDocumentoCompleto = _documento.RutaDocumentoCompleto & CType(caracteristicaDocumento_.Valor, String) & "\"

                            _documento.RutaDocumentoContingenciaCompleto = _documento.RutaDocumentoContingenciaCompleto & CType(caracteristicaDocumento_.Valor, String) & "\"

                            _documento.RutaDocumento = _documento.RutaDocumento & CType(caracteristicaDocumento_.Valor, String) & "\"

                            _documento.RutaDocumentoContingencia = _documento.RutaDocumentoContingencia & CType(caracteristicaDocumento_.Valor, String) & "\"

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                    End Select

                End If

                If caracteristicaDocumento_.CaracteristicaRenombrarArchivo = CaracteristicaDocumento.RenombrarArchivo.Si Then

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            If valorRenombrar_ = 1 Then

                                _documento.NombreDocumento = CType(caracteristicaDocumento_.ValorClaveCatalogo, String) & "_" & CType(caracteristicaDocumento_.ValorCatalogo, String)

                            Else

                                _documento.NombreDocumento = _documento.NombreDocumento & "_" & CType(caracteristicaDocumento_.ValorClaveCatalogo, String) & "_" & CType(caracteristicaDocumento_.ValorCatalogo, String)

                            End If

                            valorRenombrar_ = valorRenombrar_ + 1

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                            If valorRenombrar_ = 1 Then

                                _documento.NombreDocumento = Year(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date))

                            Else

                                _documento.NombreDocumento = _documento.NombreDocumento & "_" & Year(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date))

                            End If

                            valorRenombrar_ = valorRenombrar_ + 1

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                            If valorRenombrar_ = 1 Then

                                _documento.NombreDocumento = Year(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date))

                            Else

                                _documento.NombreDocumento = _documento.NombreDocumento & "_" & Year(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.Valor, Date))

                            End If

                            valorRenombrar_ = valorRenombrar_ + 1

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero, CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                            If valorRenombrar_ = 1 Then

                                _documento.NombreDocumento = CType(caracteristicaDocumento_.Valor, String)

                            Else

                                _documento.NombreDocumento = _documento.NombreDocumento & "_" & CType(caracteristicaDocumento_.Valor, String)

                            End If

                            valorRenombrar_ = valorRenombrar_ + 1

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                    End Select

                End If


                If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                    If caracteristicaDocumento_.CaracteristicaRequerida = CaracteristicaDocumento.Requerida.Si And
                        ((caracteristicaDocumento_.ValorFin Is Nothing And Not caracteristicaDocumento_.TipoDatoCaracteristica = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Or
                        caracteristicaDocumento_.ValorClaveCatalogoFin Is Nothing And caracteristicaDocumento_.TipoDatoCaracteristica = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Then

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1017)

                        Exit For

                    End If

                    If caracteristicaDocumento_.CaracteristicaRutaDinamica = CaracteristicaDocumento.RutaDinamica.Si Then

                        Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                                _documento.RutaDocumentoCompleto = _documento.RutaDocumentoCompleto & Year(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Month(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.ValorFin, Date)) & "\"

                                _documento.RutaDocumentoContingenciaCompleto = _documento.RutaDocumentoContingenciaCompleto & Year(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Month(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.ValorFin, Date)) & "\"

                                _documento.RutaDocumento = _documento.RutaDocumento & Year(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Month(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.ValorFin, Date)) & "\"

                                _documento.RutaDocumentoContingencia = _documento.RutaDocumentoContingencia & Year(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Month(CType(caracteristicaDocumento_.ValorFin, Date)) & "\" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.ValorFin, Date)) & "\"

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero, CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                                _documento.RutaDocumentoCompleto = _documento.RutaDocumentoCompleto & CType(caracteristicaDocumento_.ValorFin, String) & "\"

                                _documento.RutaDocumentoContingenciaCompleto = _documento.RutaDocumentoContingenciaCompleto & CType(caracteristicaDocumento_.ValorFin, String) & "\"

                                _documento.RutaDocumento = _documento.RutaDocumento & CType(caracteristicaDocumento_.ValorFin, String) & "\"

                                _documento.RutaDocumentoContingencia = _documento.RutaDocumentoContingencia & CType(caracteristicaDocumento_.ValorFin, String) & "\"

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                        End Select

                    End If

                    If caracteristicaDocumento_.CaracteristicaRenombrarArchivo = CaracteristicaDocumento.RenombrarArchivo.Si Then

                        Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                                If valorRenombrar_ = 1 Then

                                    _documento.NombreDocumento = Year(CType(caracteristicaDocumento_.ValorFin, Date)) & "_" & Month(CType(caracteristicaDocumento_.ValorFin, Date)) & "_" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.ValorFin, Date))

                                Else

                                    _documento.NombreDocumento = _documento.NombreDocumento & "_" & Year(CType(caracteristicaDocumento_.ValorFin, Date)) & "_" & Month(CType(caracteristicaDocumento_.ValorFin, Date)) & "_" & Microsoft.VisualBasic.DateAndTime.Day(CType(caracteristicaDocumento_.ValorFin, Date))

                                End If

                                valorRenombrar_ = valorRenombrar_ + 1

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero, CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                                If valorRenombrar_ = 1 Then

                                    _documento.NombreDocumento = CType(caracteristicaDocumento_.ValorFin, String)

                                Else

                                    _documento.NombreDocumento = _documento.NombreDocumento & "_" & CType(caracteristicaDocumento_.ValorFin, String)

                                End If

                                valorRenombrar_ = valorRenombrar_ + 1

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                        End Select

                    End If

                End If

            Next

            _documento.NombreDocumento = _documento.NombreDocumento & "_" & _documento.TipoDocumento

        End Sub

        Private Sub CrearDirectorio() Implements IOperacionesDocumento.CrearDirectorio

            Dim directorio_ = New OperacionesCatalogo

            directorio_.EspacioTrabajo = _operaciones.EspacioTrabajo

            directorio_.CantidadVisibleRegistros = 1

            directorio_ = _sistema.ConsultaModulo(directorio_.EspacioTrabajo,
                                                  "Directorios",
                                                  " and t_RepositorioDirectorioCompleto = '" & _documento.RutaDocumentoCompleto & "'")

            If _sistema.TieneResultados(directorio_) Then

                For Each fila_ As DataRow In directorio_.Vista.Tables(0).Rows

                    _documento.ClaveDirectorio = fila_.Item("Clave").ToString

                Next

            Else

                directorio_ = _sistema.EnsamblaModulo("Directorios")

                With directorio_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    .CampoPorNombre("t_RepositorioDirectorio") = _documento.RutaDocumento

                    .CampoPorNombre("t_RepositorioDirectorioCompleto") = _documento.RutaDocumentoCompleto

                    .CampoPorNombre("t_RepositorioDirectorioContingencia") = _documento.RutaDocumentoContingencia

                    .CampoPorNombre("t_RepositorioDirectorioCompletoContingencia") = _documento.RutaDocumentoContingenciaCompleto

                    .CampoPorNombre("i_Cve_Estatus") = 1

                    .CampoPorNombre("i_Cve_Estado") = 1

                    .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                    If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _documento.ClaveDirectorio = .ValorIndice

                    Else

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1004)

                    End If

                End With

            End If

        End Sub

        Private Sub InsertarRegistros() Implements IOperacionesDocumento.InsertarRegistros

            Dim OCDocumento_ = New OperacionesCatalogo

            OCDocumento_ = _sistema.EnsamblaModulo("RegistroDocumentos")

            With OCDocumento_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                .CampoPorNombre("t_RutaDocumentoOrigen") = _documento.RutaDocumentoOrigen

                .CampoPorNombre("i_Cve_TipoDocumento") = _documento.ClaveTipoDocumento

                .CampoPorNombre("f_FechaRegistro") = _documento.FechaRegistro.ToString("dd/MM/yyyy H:mm:ss")

                .CampoPorNombre("i_Cve_EstatusDocumento") = _documento.StatusDocumento

                .CampoPorNombre("t_NombreDocumento") = _documento.NombreDocumento

                .CampoPorNombre("t_FolioDocumento") = _documento.FolioDocumento

                .CampoPorNombre("t_Documento") = _documento.Documento

                .CampoPorNombre("i_Cve_RepositorioDigital") = _documento.ClaveRepositorioDigital

                .CampoPorNombre("t_VersionCFDi") = _documento.VersionCFDI

                .CampoPorNombre("i_Cve_EstatusLiquidacion") = _documento.StatusLiquidacion

                .CampoPorNombre("i_Cve_Estado") = 1

                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

            End With

            If OCDocumento_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                _documento.Clave = OCDocumento_.ValorIndice

                Dim OCExtensionDocumento_ = New OperacionesCatalogo

                OCExtensionDocumento_ = _sistema.EnsamblaModulo("ExtensionRegistroDocumentos")

                With OCExtensionDocumento_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    .CampoPorNombre("i_Cve_Documento") = _documento.Clave

                    .CampoPorNombre("t_NombreDocumento") = _documento.NombreDocumento

                    .CampoPorNombre("t_URLPublico") = _documento.UrlPublico

                    .CampoPorNombre("i_Cve_Estado") = 1

                    .CampoPorNombre("i_Cve_Estatus") = 1

                    .CampoPorNombre("i_Cve_TipoArchivo") = _documento.TipoArchivo

                    .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                    .CampoPorNombre("i_Cve_TipoDocumento") = _documento.ClaveTipoDocumento

                    .CampoPorNombre("i_ConsultableCliente") = _documento.DocumentoConsultableCliente

                    .CampoPorNombre("i_Cve_Directorio") = _documento.ClaveDirectorio

                    .CampoPorNombre("i_cve_EncPlantillaDocumento") = _documento.PlantillaDocumento.ClavePlantilla

                    .CampoPorNombre("i_Cve_Usuario") = _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

                    .CampoPorNombre("f_Registro") = _documento.FechaRegistro.ToString("dd/MM/yyyy H:mm:ss")

                    .CampoPorNombre("i_Cve_RepositorioDigital") = _documento.ClaveRepositorioDigital

                    If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1006)

                    End If

                End With

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1005)

            End If

        End Sub

        Private Sub GuardarDocumento() Implements IOperacionesDocumento.GuardarDocumento

            Dim nombreDocumento_ = _documento.NombreDocumento

            Dim contador_ = 2

            Try

                'Guardar en producción
                If Not Directory.Exists(_documento.RutaDocumentoCompleto) Then

                    My.Computer.FileSystem.CreateDirectory(_documento.RutaDocumentoCompleto)

                End If

                While True

                    If File.Exists(_documento.RutaDocumentoCompleto & "" & nombreDocumento_ & _documento.Extension) Then

                        nombreDocumento_ = _documento.NombreDocumento & "_" & contador_

                        contador_ = contador_ + 1

                    Else

                        _documento.NombreDocumento = nombreDocumento_ & _documento.Extension

                        System.IO.File.Copy(_documento.RutaDocumentoOrigen,
                                            _documento.RutaDocumentoCompleto & "" & _documento.NombreDocumento)

                        Exit While

                    End If

                End While

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1007)

            End Try


            Try

                'Guardar en contingencia
                'If Not Directory.Exists(_documento.RutaDocumentoContingenciaCompleto) Then

                'My.Computer.FileSystem.CreateDirectory(_documento.RutaDocumentoContingenciaCompleto)

                ' End If

                'System.IO.File.Copy(_documento.RutaDocumentoOrigen,
                '                   _documento.RutaDocumentoContingenciaCompleto & "" & _documento.NombreDocumento)

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1008)

            End Try

        End Sub

        Private Sub InsertarRegistroVinculacion(Optional ByVal claveModulo_ As Integer = Nothing) Implements IOperacionesDocumento.InsertarRegistroVinculacion

            _documento.VinculacionDocumentos.Modulo = Nothing

            _documento.VinculacionDocumentos.CampoLLaveModulo = Nothing

            _documento.VinculacionDocumentos.ClaveModulo = Nothing

            Select Case _documento.TipoVinculacion

                Case gsol.documento.Documento.TiposVinculacion.NoDefinidia

                    _documento.VinculacionDocumentos.Modulo = Nothing

                    _documento.VinculacionDocumentos.CampoLLaveModulo = Nothing

                    _documento.VinculacionDocumentos.ClaveModulo = Nothing

                Case gsol.documento.Documento.TiposVinculacion.Referencia

                    _documento.VinculacionDocumentos.Modulo = "DocumentosMaestroOperaciones"

                    _documento.VinculacionDocumentos.CampoLLaveModulo = "i_Cve_MaestroOperaciones"

                    Dim dataTable_ As System.Data.DataTable = Nothing

                    Dim consulta_ As String = Nothing

                    consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                                "select i_Cve_MaestroOperaciones from Reg009MaestroOperaciones where t_Referencia  = '" & _referencia & "' and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa & "  and i_TipoReferencia = 1 and i_Cve_Estado = 1"

                    _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                    If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        dataTable_ = DirectCast(_estatus.ObjectReturned, System.Data.DataTable)

                        If dataTable_.Rows.Count > 0 Then

                            _documento.VinculacionDocumentos.ClaveModulo = dataTable_(0)(0).ToString

                        End If

                    End If

                Case gsol.documento.Documento.TiposVinculacion.Factura

                Case gsol.documento.Documento.TiposVinculacion.CorreoElectronico

                    _documento.VinculacionDocumentos.Modulo = "DocumentosBandejaEntrada"

                    _documento.VinculacionDocumentos.CampoLLaveModulo = "i_Cve_CorreoElectronicoEntrada"

                    _documento.VinculacionDocumentos.ClaveModulo = claveModulo_


            End Select

            If _documento.VinculacionDocumentos.ClaveModulo > 0 Then

                If Not _documento.VinculacionDocumentos.CampoLLaveModulo Is Nothing Then

                    Dim vinculacion_ = New OperacionesCatalogo

                    vinculacion_.EspacioTrabajo = _operaciones.EspacioTrabajo

                    vinculacion_ = _sistema.EnsamblaModulo(_documento.VinculacionDocumentos.Modulo)

                    With vinculacion_

                        .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                        .CampoPorNombre("i_Cve_Documento") = _documento.Clave

                        .CampoPorNombre("i_Cve_Usuario") = _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

                        .CampoPorNombre(_documento.VinculacionDocumentos.CampoLLaveModulo) = _documento.VinculacionDocumentos.ClaveModulo

                        .CampoPorNombre("i_Cve_Estatus") = 1

                        .CampoPorNombre("i_Cve_Estado") = 1

                        .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                        If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1018)

                        End If

                    End With

                End If

            End If

            _referencia = Nothing

        End Sub

        Public Sub InsertarRegistrosCaracteristicas() Implements IOperacionesDocumento.InsertarRegistrosCaracteristicas

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                Dim ioCaracteristica_ = New OperacionesCatalogo

                ioCaracteristica_.EspacioTrabajo = _operaciones.EspacioTrabajo

                ioCaracteristica_ = _sistema.EnsamblaModulo("VinCaracteristicasExtDocumentos")

                Dim ioCaracteristicaSecundaria_ = New OperacionesCatalogo

                ioCaracteristicaSecundaria_.EspacioTrabajo = _operaciones.EspacioTrabajo

                ioCaracteristicaSecundaria_ = _sistema.EnsamblaModulo("VinCaracteristicasExtDocumentos")

                With ioCaracteristica_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    .CampoPorNombre("i_Cve_CaracteristicaDocumento") = caracteristicaDocumento_.ClaveCaracteristica

                    .CampoPorNombre("i_Cve_Documento") = _documento.Clave

                    .CampoPorNombre("i_ValorNumero") = Nothing

                    .CampoPorNombre("i_ValorDecimal") = Nothing

                    .CampoPorNombre("t_ValorTexto") = Nothing

                    .CampoPorNombre("f_ValorFecha") = Nothing

                    .CampoPorNombre("f_ValorHora") = Nothing

                    .CampoPorNombre("f_ValorFechaHora") = Nothing

                    .CampoPorNombre("i_ValorClaveCatalogo") = Nothing

                    .CampoPorNombre("t_ValorCatalogo") = Nothing

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero

                            .CampoPorNombre("i_ValorNumero") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                            .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                            .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                            .CampoPorNombre("i_ValorDecimal") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                            .CampoPorNombre("f_ValorFechaHora") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                            .CampoPorNombre("f_ValorFecha") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                            .CampoPorNombre("f_ValorHora") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            .CampoPorNombre("i_ValorClaveCatalogo") = caracteristicaDocumento_.ValorClaveCatalogo

                            .CampoPorNombre("t_ValorCatalogo") = caracteristicaDocumento_.ValorCatalogo

                    End Select

                    .CampoPorNombre("i_Cve_Estatus") = 1

                    .CampoPorNombre("i_Cve_Estado") = 1

                    .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                End With

                If ioCaracteristica_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                        With ioCaracteristicaSecundaria_

                            .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                            .CampoPorNombre("i_Cve_CaracteristicaDocumento") = caracteristicaDocumento_.ClaveCaracteristicaSecundaria

                            .CampoPorNombre("i_Cve_Documento") = _documento.Clave

                            .CampoPorNombre("i_ValorNumero") = Nothing

                            .CampoPorNombre("i_ValorDecimal") = Nothing

                            .CampoPorNombre("t_ValorTexto") = Nothing

                            .CampoPorNombre("f_ValorFecha") = Nothing

                            .CampoPorNombre("f_ValorHora") = Nothing

                            .CampoPorNombre("f_ValorFechaHora") = Nothing

                            .CampoPorNombre("i_ValorClaveCatalogo") = Nothing

                            .CampoPorNombre("t_ValorCatalogo") = Nothing

                            Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero

                                    .CampoPorNombre("i_ValorNumero") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                                    .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                                    .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                                    .CampoPorNombre("i_ValorDecimal") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                                    .CampoPorNombre("f_ValorFechaHora") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                                    .CampoPorNombre("f_ValorFecha") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                                    .CampoPorNombre("f_ValorHora") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                                    .CampoPorNombre("i_ValorClaveCatalogo") = caracteristicaDocumento_.ValorClaveCatalogoFin

                                    .CampoPorNombre("t_ValorCatalogo") = caracteristicaDocumento_.ValorCatalogoFin

                            End Select

                            .CampoPorNombre("i_Cve_Estatus") = 1

                            .CampoPorNombre("i_Cve_Estado") = 1

                            .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                            If Not ioCaracteristicaSecundaria_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1011)

                                Exit For

                            End If

                        End With

                    End If

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1010)

                    Exit For

                End If

            Next

            RegenerarCaracteristicasExtension(_documento.Clave)

        End Sub

        Public Sub BuscarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.BuscarDocumento

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            BuscarPrivilegioUsuarioDocumento()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                CargarCaracteristicasVisorMaestroDocumentos()

            End If

        End Sub

        Public Sub BuscarPrivilegioUsuario(ByVal ClaveDocumento_ As Integer,
                                           Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.BuscarPrivilegioUsuario

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                With _documento

                    .ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                    .Clave = ClaveDocumento_

                    .TipoPrivilegio = Documento.TiposPrivilegios.Consultar

                End With

            End If

            BuscarPrivilegioUsuarioDocumento()

        End Sub

        Public Sub BuscarPrivilegioUsuario(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.BuscarPrivilegioUsuario

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            BuscarPrivilegioUsuarioDocumento()

        End Sub

        Public Sub BuscarPrivilegioUsuarioDocumento()

            If Not _documento.TipoPrivilegio = gsol.documento.Documento.TiposPrivilegios.NoIdentificado Then

                Dim token_ As String = Nothing

                Dim query_ As String = Nothing

                query_ = " and (i_Todos = 1 " &
                         " or i_Cve_Usuario = " & _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario & " )"

                Select Case Documento.TipoPrivilegio

                    Case gsol.documento.Documento.TiposPrivilegios.Insertar

                        query_ = query_ & " and (i_PuedeInsertar = 1 or i_Administrador = 1) and i_Cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla

                        token_ = "ResumenPermisosMaestroDocumentosPlantilla"

                    Case gsol.documento.Documento.TiposPrivilegios.Modificar

                        query_ = query_ & " and (i_PuedeModificar = 1 or i_Administrador = 1) and i_Cve_Documento = " & _documento.Clave

                        token_ = "ResumenPermisosMaestroDocumentos"

                    Case gsol.documento.Documento.TiposPrivilegios.Consultar

                        query_ = query_ & " and (i_PuedeConsultar = 1 or i_Administrador = 1) and i_Cve_Documento = " & _documento.Clave

                        token_ = "ResumenPermisosMaestroDocumentos"

                    Case gsol.documento.Documento.TiposPrivilegios.Administrador

                        query_ = query_ & " and i_Administrador = 1"

                    Case Else

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1026)

                End Select

                query_ = query_ & " and  1 = (case when i_MultiEmpresa = 1 then 1" &
                         "                        when  i_Cve_DivisionMiEmpresa = " & _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & " then 1" &
                         "                        else 0 end)"

                Dim privigelios_ = New OperacionesCatalogo

                privigelios_.EspacioTrabajo = _operaciones.EspacioTrabajo

                privigelios_.CantidadVisibleRegistros = 0

                privigelios_ = _sistema.ConsultaModulo(privigelios_.EspacioTrabajo,
                                                       token_,
                                                       query_)

                If _sistema.TieneResultados(privigelios_) Then

                    For Each fila_ As DataRow In privigelios_.Vista.Tables(0).Rows

                        If fila_.Item("i_TipoRegla").ToString = 1 Then

                            _documento.TienePrivilegio = True

                        Else

                            _documento.TienePrivilegio = False

                            Select Case Documento.TipoPrivilegio

                                Case gsol.documento.Documento.TiposPrivilegios.Administrador

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1029)

                                Case gsol.documento.Documento.TiposPrivilegios.Insertar

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1033)

                                Case gsol.documento.Documento.TiposPrivilegios.Modificar

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1027)

                                Case gsol.documento.Documento.TiposPrivilegios.Consultar

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1028)

                            End Select

                            Exit For

                        End If

                    Next

                Else

                    Select Case Documento.TipoPrivilegio

                        Case gsol.documento.Documento.TiposPrivilegios.Administrador

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1029)

                        Case gsol.documento.Documento.TiposPrivilegios.Insertar

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1033)

                        Case gsol.documento.Documento.TiposPrivilegios.Modificar

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1027)

                        Case gsol.documento.Documento.TiposPrivilegios.Consultar

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1028)

                    End Select

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1026)

            End If

        End Sub

        Public Sub CargarCaracteristicasConInformacion() Implements IOperacionesDocumento.CargarCaracteristicasConInformacion

            _documento.PlantillaDocumento.CaracteristicasDocumentos.Clear()

            Dim plantilla_ = New OperacionesCatalogo

            plantilla_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantilla_.CantidadVisibleRegistros = 0

            plantilla_ = _sistema.ConsultaModulo(plantilla_.EspacioTrabajo,
                                                 "ResumenPlantillasCaracteristicasConDatos",
                                                 " and i_Cve_Documento = " & _documento.Clave)

            If _sistema.TieneResultados(plantilla_) Then

                For Each fila_ As DataRow In plantilla_.Vista.Tables(0).Rows

                    _documento.PlantillaDocumento.ClavePlantilla = fila_.Item("i_Cve_EncPlantillaDocumento").ToString

                    Dim caracteristicaDocumento_ = New CaracteristicaDocumento

                    With caracteristicaDocumento_

                        caracteristicaDocumento_.ClaveCaracteristica = fila_.Item("i_Cve_CaracteristicaDocumento").ToString

                        caracteristicaDocumento_.TipoCaracteristica = fila_.Item("t_Tipo").ToString

                        caracteristicaDocumento_.ClaveTipoCaracteristica = fila_.Item("i_Cve_Tipo").ToString

                        caracteristicaDocumento_.ClaveCaracteristicaPrimaria = IIf(fila_.Item("i_cve_CaracteristicaPrimaria").ToString = "", 0, fila_.Item("i_cve_CaracteristicaPrimaria").ToString)

                        caracteristicaDocumento_.ClaveFormatoCaracteristica = fila_.Item("i_Cve_FormatoCaracteristica").ToString

                        caracteristicaDocumento_.TipoDatoCaracteristica = fila_.Item("i_Cve_FormatoCaracteristica").ToString

                        caracteristicaDocumento_.TituloCaracteristica = fila_.Item("t_TituloCaracteristica").ToString

                        caracteristicaDocumento_.Titulo = fila_.Item("t_Titulo").ToString

                        caracteristicaDocumento_.DisplayField = fila_.Item("t_DisplayField").ToString

                        caracteristicaDocumento_.KeyField = fila_.Item("t_KeyField").ToString

                        caracteristicaDocumento_.NameAsKey = fila_.Item("t_NameAsKey").ToString

                        caracteristicaDocumento_.PermissionNumber = fila_.Item("t_PermissionNumber").ToString

                        Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero

                                caracteristicaDocumento_.Valor = fila_.Item("i_ValorNumero").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                                caracteristicaDocumento_.Valor = fila_.Item("t_ValorTexto").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                                caracteristicaDocumento_.Valor = fila_.Item("t_ValorTexto").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                                caracteristicaDocumento_.Valor = fila_.Item("i_ValorDecimal").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                                caracteristicaDocumento_.Valor = fila_.Item("f_ValorFechaHora").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                                caracteristicaDocumento_.Valor = fila_.Item("f_ValorFecha").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                                caracteristicaDocumento_.Valor = fila_.Item("f_ValorHora").ToString

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                                caracteristicaDocumento_.ValorClaveCatalogo = fila_.Item("i_ValorClaveCatalogo").ToString

                                caracteristicaDocumento_.ValorCatalogo = fila_.Item("t_ValorCatalogo").ToString

                        End Select

                    End With

                    _documento.PlantillaDocumento.CaracteristicasDocumentos.Add(fila_.Item("i_Cve_CaracteristicaDocumento").ToString,
                                                                                caracteristicaDocumento_)
                Next

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1020)

            End If


        End Sub

        Private Sub ValidarDocumentosDuplicados() Implements IOperacionesDocumento.ValidarDocumentosDuplicados

            Dim clausulasSQL_ As List(Of String) = New List(Of String)

            Dim cantidadCaracteristicas_ As Integer = 0

            Dim cantidadCaracteristicasARevisar_ As Integer = 0

            Dim clavesCaracteristicas_ As String = Nothing

            Dim valoresCaracteristicas_ As String = Nothing

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                Dim valorCampoInicial_ As String = Nothing

                Dim valorCampoFinal_ As String = Nothing

                Dim formatoCaracteristica_ As CaracteristicaDocumento.TiposDatosCaracteristicas = caracteristicaDocumento_.TipoDatoCaracteristica

                If caracteristicaDocumento_.CaracteristicaRevision = CaracteristicaDocumento.Revision.Si Then

                    cantidadCaracteristicasARevisar_ = cantidadCaracteristicasARevisar_ + 1

                    If (CStr(caracteristicaDocumento_.Valor) <> "" And formatoCaracteristica_ <> CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Or
                        (CStr(caracteristicaDocumento_.ValorCatalogo) <> "" And formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Then

                        clavesCaracteristicas_ = clavesCaracteristicas_ & IIf(clavesCaracteristicas_ Is Nothing, caracteristicaDocumento_.ClaveCaracteristica, "," & caracteristicaDocumento_.ClaveCaracteristica)

                        cantidadCaracteristicas_ = cantidadCaracteristicas_ + 1

                        If ((formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero) Or (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal)) Then

                            valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor , '""" & caracteristicaDocumento_.Valor & """'))", " or  (CONTAINS(t_Valor , '""" & caracteristicaDocumento_.Valor & """'))")

                        ElseIf ((formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Texto) Or (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso)) Then

                            ' valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (t_Valor = '" & caracteristicaDocumento_.Valor & "')", " or  (t_Valor = '" & caracteristicaDocumento_.Valor & "')")

                            valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.Valor & """'))", " or (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.Valor & """'))")

                        ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha) Then

                            valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.Valor).ToString("yyyy-MM-dd") & """'))", " or  (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.Valor).ToString("yyyy-MM-dd") & """'))")

                        ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora) Then

                            valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.Valor).ToString("yyyy-MM-dd HH:mm") & """'))", " or  (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.Valor).ToString("yyyy-MM-dd HH:mm") & """'))")

                        ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Hora) Then

                            valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.Valor).ToString("HH:mm") & """'))", " or  (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.Valor).ToString("HH:mm") & """'))")

                        ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Then

                            'valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (t_Valor = '" & caracteristicaDocumento_.ValorCatalogo & "')", " or  (t_Valor = '" & caracteristicaDocumento_.ValorCatalogo & "')")

                            valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.ValorCatalogo & """'))", " or (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.ValorCatalogo & """'))")

                        End If

                        If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                            clavesCaracteristicas_ = clavesCaracteristicas_ & IIf(clavesCaracteristicas_ Is Nothing, caracteristicaDocumento_.ClaveCaracteristicaSecundaria, "," & caracteristicaDocumento_.ClaveCaracteristicaSecundaria)

                            cantidadCaracteristicas_ = cantidadCaracteristicas_ + 1

                            If ((formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero) Or (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal)) Then

                                valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor , '""" & caracteristicaDocumento_.ValorFin & """'))", " or  (CONTAINS(t_Valor , '""" & caracteristicaDocumento_.ValorFin & """'))")

                            ElseIf ((formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Texto) Or (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso)) Then

                                'valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (t_Valor = '" & caracteristicaDocumento_.ValorFin & "')", " or  (t_Valor = '" & caracteristicaDocumento_.ValorFin & "')")

                                valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.ValorFin & """'))", " or (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.ValorFin & """'))")

                            ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha) Then

                                valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & Convert.ToDateTime(caracteristicaDocumento_.ValorFin).ToString("yyyy-MM-dd") & """'))", " or  (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.ValorFin).ToString("yyyy-MM-dd") & """'))")

                            ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora) Then

                                valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & Convert.ToDateTime(caracteristicaDocumento_.ValorFin).ToString("yyyy-MM-dd HH:mm") & """'))", " or  (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.ValorFin).ToString("yyyy-MM-dd HH:mm") & """'))")

                            ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Hora) Then

                                valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & Convert.ToDateTime(caracteristicaDocumento_.ValorFin).ToString("HH:mm") & """'))", " or  (CONTAINS(t_Valor , '""" & Convert.ToDateTime(caracteristicaDocumento_.ValorFin).ToString("HH:mm") & """'))")

                            ElseIf (formatoCaracteristica_ = CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Then

                                'valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (t_Valor = '" & caracteristicaDocumento_.ValorCatalogoFin & "')", " or  (t_Valor = '" & caracteristicaDocumento_.ValorCatalogoFin & "')")

                                valoresCaracteristicas_ = valoresCaracteristicas_ & IIf(valoresCaracteristicas_ Is Nothing, " (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.ValorCatalogoFin & """'))", " or (CONTAINS(t_Valor, '""" & caracteristicaDocumento_.ValorCatalogoFin & """'))")

                            End If

                        End If

                    End If

                End If

            Next

            If cantidadCaracteristicasARevisar_ > 0 Then

                Dim dataTable_ As System.Data.DataTable = Nothing

                Dim consulta_ As String = Nothing

                consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                            "select i_Cve_Documento " &
                             "from Ext012CaracteristicasExtDocumentos as b with(nolock) " &
                             "where i_Cve_Estatus in (1,2) and i_cve_EncPlantillaDocumento =  " & _documento.PlantillaDocumento.ClavePlantilla &
                             " and i_Cve_CaracteristicaDocumento in (" & clavesCaracteristicas_ & ") and (" & valoresCaracteristicas_ & ") " &
                             "group by i_Cve_Documento " &
                             "having counT(*) =  " & cantidadCaracteristicas_ & ""

                _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                    dataTable_ = DirectCast(_estatus.ObjectReturned, System.Data.DataTable)

                    If dataTable_.Rows.Count > 0 Then

                        If dataTable_(0)(0) > 0 Then

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1030)

                        End If

                    End If

                End If

            End If

        End Sub

        Public Sub CargarCaracteristicasVisorMaestroDocumentos() Implements IOperacionesDocumento.CargarCaracteristicasVisorMaestroDocumentos

            _documento.PlantillaDocumento.CaracteristicasDocumentos.Clear()

            Dim CaracteristicaVisor_ = New OperacionesCatalogo

            CaracteristicaVisor_.EspacioTrabajo = _operaciones.EspacioTrabajo

            CaracteristicaVisor_.CantidadVisibleRegistros = 0

            CaracteristicaVisor_ = _sistema.ConsultaModulo(CaracteristicaVisor_.EspacioTrabajo,
                                                 "VisorMaestroDocumentos",
                                                 " and i_Cve_Documento = " & _documento.Clave)

            If _sistema.TieneResultados(CaracteristicaVisor_) Then

                For Each fila_ As DataRow In CaracteristicaVisor_.Vista.Tables(0).Rows

                    Dim caracteristicaDocumento_ = New CaracteristicaDocumento

                    With caracteristicaDocumento_

                        caracteristicaDocumento_.ClaveCaracteristica = fila_.Item("Clave CaracteristicaDocumento").ToString

                        caracteristicaDocumento_.Valor = fila_.Item("Valor").ToString

                        caracteristicaDocumento_.TipoDatoCaracteristica = fila_.Item("Clave FormatoCaracteristica").ToString

                        caracteristicaDocumento_.DisplayField = fila_.Item("DisplayField").ToString

                        caracteristicaDocumento_.KeyField = fila_.Item("KeyField").ToString

                        caracteristicaDocumento_.NameAsKey = fila_.Item("NameAsKey").ToString

                        caracteristicaDocumento_.PermissionNumber = fila_.Item("PermissionNumber").ToString

                        _documento.NombreDocumento = fila_.Item("Nombre Documento").ToString

                        _documento.RutaDocumentoCompleto = fila_.Item("Ruta Documento").ToString

                        _documento.RutaDocumentoContingenciaCompleto = fila_.Item("Ruta Documento Contingencia").ToString

                        _documento.ClaveEstatus = fila_.Item("Clave Estatus").ToString

                    End With

                    _documento.PlantillaDocumento.CaracteristicasDocumentos.Add(fila_.Item("Clave CaracteristicaDocumento").ToString,
                                                                                caracteristicaDocumento_)
                Next

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1021)

            End If

            If _documento.ClaveEstatus = 3 Then

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1031)

            End If

        End Sub

        Public Sub ModificarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.ModificarDocumento

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            BuscarPrivilegioUsuario()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                ValidarCaracteristicasPlantilla()

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    ModificarRegistros()

                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                        ModificarRegistrosCaracteristicas()

                    End If

                End If

            End If

        End Sub

        Private Sub ModificarRegistros() Implements IOperacionesDocumento.ModificarRegistros

            Dim OCExtensionDocumento_ = New OperacionesCatalogo

            OCExtensionDocumento_ = _sistema.EnsamblaModulo("ExtensionRegistroDocumentos")

            With OCExtensionDocumento_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                .CampoPorNombre("i_Cve_Estatus") = _documento.ClaveEstatus

                .CampoPorNombre("i_ConsultableCliente") = _documento.DocumentoConsultableCliente

            End With

            If Not OCExtensionDocumento_.Modificar(_documento.Clave) = IOperacionesCatalogo.EstadoOperacion.COk Then

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1022)

            End If

        End Sub

        Private Sub ModificarRegistrosCaracteristicas() Implements IOperacionesDocumento.ModificarRegistrosCaracteristicas

            Dim plantilla_ = New OperacionesCatalogo

            plantilla_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantilla_.CantidadVisibleRegistros = 0

            plantilla_ = _sistema.ConsultaModulo(plantilla_.EspacioTrabajo,
                                                 "ResumenPlantillasCaracteristicasConDatos",
                                                 " and i_Cve_Documento = " & _documento.Clave)

            If _sistema.TieneResultados(plantilla_) Then

                Dim orden_ As Integer = 1

                For Each fila_ As DataRow In plantilla_.Vista.Tables(0).Rows

                    If fila_.Item("i_Cve_CaracteristicaPrimaria").ToString = "" Then

                        _documento.PlantillaDocumento.CaracteristicasDocumentos(orden_).ClaveCaracteristicaVin = fila_.Item("i_Cve_VinCaracteristicasExtDocumentos").ToString

                    Else

                        orden_ = orden_ - 1

                        _documento.PlantillaDocumento.CaracteristicasDocumentos(orden_).ClaveCaracteristicaSecundaria = fila_.Item("i_Cve_VinCaracteristicasExtDocumentos").ToString

                    End If

                    orden_ = orden_ + 1

                Next fila_

            End If

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                Dim ioCaracteristica_ = New OperacionesCatalogo

                ioCaracteristica_.EspacioTrabajo = _operaciones.EspacioTrabajo

                ioCaracteristica_ = _sistema.EnsamblaModulo("VinCaracteristicasExtDocumentos")

                Dim ioCaracteristicaSecundaria_ = New OperacionesCatalogo

                ioCaracteristicaSecundaria_.EspacioTrabajo = _operaciones.EspacioTrabajo

                ioCaracteristicaSecundaria_ = _sistema.EnsamblaModulo("VinCaracteristicasExtDocumentos")

                With ioCaracteristica_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero

                            .CampoPorNombre("i_ValorNumero") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                            .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                            .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                            .CampoPorNombre("i_ValorDecimal") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                            .CampoPorNombre("f_ValorFechaHora") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                            .CampoPorNombre("f_ValorFecha") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                            .CampoPorNombre("f_ValorHora") = caracteristicaDocumento_.Valor

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            .CampoPorNombre("i_ValorClaveCatalogo") = caracteristicaDocumento_.ValorClaveCatalogo

                            .CampoPorNombre("t_ValorCatalogo") = caracteristicaDocumento_.ValorCatalogo

                    End Select

                End With

                If ioCaracteristica_.Modificar(caracteristicaDocumento_.ClaveCaracteristicaVin) = IOperacionesCatalogo.EstadoOperacion.COk Then

                    If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                        With ioCaracteristicaSecundaria_

                            .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                            Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero

                                    .CampoPorNombre("i_ValorNumero") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Texto

                                    .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso

                                    .CampoPorNombre("t_ValorTexto") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal

                                    .CampoPorNombre("i_ValorDecimal") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

                                    .CampoPorNombre("f_ValorFechaHora") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                                    .CampoPorNombre("f_ValorFecha") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Hora

                                    .CampoPorNombre("f_ValorHora") = caracteristicaDocumento_.ValorFin

                                Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                                    .CampoPorNombre("i_ValorClaveCatalogo") = caracteristicaDocumento_.ValorClaveCatalogoFin

                                    .CampoPorNombre("t_ValorCatalogo") = caracteristicaDocumento_.ValorCatalogoFin

                            End Select

                            If Not ioCaracteristicaSecundaria_.Modificar(caracteristicaDocumento_.ClaveCaracteristicaVinSecundaria) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1024)

                                Exit For

                            End If

                        End With

                    End If

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1023)

                    Exit For

                End If

            Next

            RegenerarCaracteristicasExtension(_documento.Clave)

        End Sub

        Public Sub EliminarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.EliminarDocumento

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Public Sub BuscarInformacionCaracteristicasCliente()

            Dim razonSocial_ As String = Nothing

            For contador_ As Integer = 1 To _documento.PlantillaDocumento.CaracteristicasDocumentos.Count

                If _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 67 Then

                    razonSocial_ = _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor

                    Exit For

                End If

            Next contador_

            If Not razonSocial_ Is Nothing Then

                Dim dataTable_ As System.Data.DataTable = Nothing

                Dim consulta_ As String = Nothing

                consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                            "select top 1 sdie_t_RazonSocial,sdie_t_RFC from Vt012DatosClientes where sdie_t_RazonSocial like  '%" & razonSocial_ & "%'"

                _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                    dataTable_ = DirectCast(_estatus.ObjectReturned, System.Data.DataTable)

                    If dataTable_.Rows.Count > 0 Then

                        For contador_ As Integer = 1 To _documento.PlantillaDocumento.CaracteristicasDocumentos.Count

                            If _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 67 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(0).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 53 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(1).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 87 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = "CLIENTE"

                            End If

                        Next contador_

                    Else

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1034)

                    End If

                End If

            End If

        End Sub

        Public Sub VisualizarDocumento(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            _documento.TipoPrivilegio = Documento.TiposPrivilegios.Consultar

            BuscarPrivilegioUsuario()

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                If _documento.TienePrivilegio Then

                    _operaciones.ObjetoRepositorio = _documento.Clave

                    _sistema.CargaModuloVirtual("VisorMaestroDocumentos",
                                                _operaciones,
                                                IOperacionesCatalogo.TiposOperacionSQL.Consulta)

                End If

            End If

        End Sub

        Public Sub VisualizarDocumentoVentanaNoDependiente(Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            _documento.TipoPrivilegio = Documento.TiposPrivilegios.Consultar

            BuscarPrivilegioUsuario()

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                If _documento.TienePrivilegio Then

                    Dim MyForm_ = New gsol.Modulos.VisorMaestroDocumentos64.frm012VisorMaestroDocumentos(ioperacion_:=_operaciones.Clone,
                                                                                                         claveDocumento_:=_documento.Clave)
                    MyForm_.Show()

                End If

            End If

        End Sub

        Public Sub VisualizarDocumentoCatalogo(ByVal gsCatalogo_ As GsCatalogo,
                                               Optional ByVal nombreCampoEntornoClaveDocumento_ As String = "Clave Documento",
                                               Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            Dim generoError_ As Boolean = False

            Dim sbCadena_ = New System.Text.StringBuilder()

            Dim rtbMensaje_ As New System.Windows.Forms.RichTextBox

            sbCadena_.Append("{\rtf1\ansi")

            sbCadena_.Append("{\colortbl ; \red255\green60\blue51; \red0\green0\blue255;}")

            Dim respuesta_ As String = Nothing

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            For Each fila_ As System.Windows.Forms.DataGridViewRow In gsCatalogo_.DgvCatalogo.Rows

                If fila_.Selected Then

                    _estatus.SetOK()

                    _documento.Clave = fila_.Cells(nombreCampoEntornoClaveDocumento_).Value

                    VisualizarDocumentoVentanaNoDependiente()

                    If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        generoError_ = True

                        sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoClaveDocumento_).Value &
                                         "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Descargado. \b0") &
                                         "\par ******************************************************")

                    End If

                End If

            Next

            sbCadena_.Append("}")

            rtbMensaje_.Rtf = sbCadena_.ToString()

            If generoError_ Then

                _sistema.GsDialogo(Nothing, rtbMensaje_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

            End If

        End Sub

        Public Sub DescargarDocumentoCatalogo(ByVal gsCatalogo_ As GsCatalogo,
                                              Optional ByVal nombreCampoEntornoClaveDocumento_ As String = "Clave Documento",
                                              Optional ByVal nombreCampoEntornoNombrePlantilla_ As String = "Plantilla",
                                              Optional ByVal descargaEstandar_ As Boolean = False,
                                              Optional ByVal nombreCarpeta_ As String = Nothing,
                                              Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            Dim rutaDescarga_ As String = Nothing

            Dim generoError_ As Boolean = False

            Dim sbCadena_ = New System.Text.StringBuilder()

            Dim rtbMensaje_ As New System.Windows.Forms.RichTextBox

            sbCadena_.Append("{\rtf1\ansi")

            sbCadena_.Append("{\colortbl ; \red255\green60\blue51; \red0\green0\blue255;}")

            Dim respuesta_ As String = Nothing

            For Each fila_ As System.Windows.Forms.DataGridViewRow In gsCatalogo_.DgvCatalogo.Rows

                If fila_.Selected Then

                    _estatus.SetOK()

                    DescargarDocumento(claveDocumento_:=fila_.Cells(nombreCampoEntornoClaveDocumento_).Value,
                                       descargaEstandar_:=descargaEstandar_,
                                       nombreCarpeta_:=nombreCarpeta_,
                                       operacionesCatalogo_:=operacionesCatalogo_)

                    rutaDescarga_ = IIf(IsNothing(_estatus.ObjectReturned), "", _estatus.ObjectReturned)

                    _estatus.ObjectReturned = Nothing

                    If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        generoError_ = True

                        If Not nombreCampoEntornoNombrePlantilla_ Is Nothing Then

                            sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoClaveDocumento_).Value &
                                             "\par Documento: " & fila_.Cells(nombreCampoEntornoNombrePlantilla_).Value &
                                             "\par Ruta: " & rutaDescarga_.Replace("\", "\\") &
                                             "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Descargado. \b0") &
                                             "\par ******************************************************")

                        Else

                            sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoClaveDocumento_).Value &
                                             IIf(IsNothing(rutaDescarga_), "", "\par Ruta: " & rutaDescarga_.Replace("\", "\\")) &
                                             "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Descargado. \b0") &
                                             "\par ******************************************************")

                        End If

                    End If

                End If

            Next

            sbCadena_.Append("}")

            rtbMensaje_.Rtf = sbCadena_.ToString()

            If generoError_ Then

                _sistema.GsDialogo(Nothing, rtbMensaje_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

            Else

                _sistema.GsDialogo(Nothing, "Documentos descargados en la siguiente ruta: " & rutaDescarga_, Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            End If

        End Sub

        Public Sub DescargarDocumento(ByVal claveDocumento_ As Integer,
                                      Optional ByVal descargaEstandar_ As Boolean = False,
                                      Optional ByVal nombreCarpeta_ As String = Nothing,
                                      Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            _documento.Clave = claveDocumento_

            _documento.TipoPrivilegio = gsol.documento.Documento.TiposPrivilegios.Consultar

            BuscarPrivilegioUsuario()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                Dim respuesta_ As TagWatcher

                Dim query_ As String = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT top 1 * FROM VT012VisorMaestroDocumentos WHERE i_Cve_documento= " & claveDocumento_ & " ORDER BY i_orden ASC"

                respuesta_ = _sistema.ComandosSingletonSQL(query_)

                If respuesta_.Status = TagWatcher.TypeStatus.Ok Then

                    Dim cursorTable_ As New System.Data.DataTable

                    cursorTable_ = DirectCast(respuesta_.ObjectReturned, System.data.DataTable)

                    For Each fila_ As DataRow In cursorTable_.Rows

                        _documento.RutaDocumentoCompleto = fila_.Item("t_RutaDocumento").ToString

                        _documento.RutaDocumentoContingenciaCompleto = fila_.Item("t_RutaDocumentoContingencia").ToString

                        _documento.NombreDocumento = fila_.Item("t_NombreDocumento")

                        Dim extensionVirtualizada_ As String = Nothing

                        If IsDBNull(fila_.Item("t_ExtensionVirtualizada")) Then

                            extensionVirtualizada_ = Nothing

                        Else

                            extensionVirtualizada_ = fila_.Item("t_ExtensionVirtualizada")

                        End If

                        Dim rutaDescarga_ As String

                        If descargaEstandar_ Then

                            rutaDescarga_ = GetFolderPath(SpecialFolder.UserProfile) & "\Downloads\"

                        Else

                            rutaDescarga_ = Nothing

                        End If

                        Try

                            If Not nombreCarpeta_ Is Nothing Then

                                rutaDescarga_ = rutaDescarga_ & nombreCarpeta_ & "\"

                                If Not Directory.Exists(rutaDescarga_) Then

                                    Directory.CreateDirectory(rutaDescarga_)

                                End If

                            End If

                            _estatus.ObjectReturned = rutaDescarga_

                            If File.Exists(_documento.RutaDocumentoCompleto) Then

                                If Not File.Exists(rutaDescarga_ & _documento.NombreDocumento) Then

                                    System.IO.File.Copy(_documento.RutaDocumentoCompleto,
                                                        rutaDescarga_ & _documento.NombreDocumento)

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1035)

                                End If

                            Else

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1041)

                            End If

                            If Not extensionVirtualizada_ Is Nothing Then

                                If File.Exists(_documento.RutaDocumentoCompleto.Replace(Path.GetExtension(_documento.RutaDocumentoCompleto), extensionVirtualizada_)) Then

                                    If Not File.Exists(rutaDescarga_ & _documento.NombreDocumento.Replace(Path.GetExtension(_documento.RutaDocumentoCompleto), extensionVirtualizada_)) Then

                                        System.IO.File.Copy(_documento.RutaDocumentoCompleto.Replace(Path.GetExtension(_documento.RutaDocumentoCompleto), extensionVirtualizada_),
                                                            rutaDescarga_ & _documento.NombreDocumento.Replace(Path.GetExtension(_documento.RutaDocumentoCompleto), extensionVirtualizada_))

                                    Else

                                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1035)

                                    End If

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1041)

                                End If

                            End If

                        Catch ex As Exception

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1042)

                        End Try

                    Next

                End If

            End If

        End Sub

        Public Sub CambiarEstatusDocumentoCatalogo(ByVal gsCatalogo_ As GsCatalogo,
                                                   Optional ByVal nombreCampoEntornoClaveDocumento_ As String = "Clave Documento",
                                                   Optional ByVal estatusDocumento_ As Documento.EstatusDocumento = gsol.documento.Documento.EstatusDocumento.Habilitado,
                                                   Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)


            _estatus.SetOK()

            Dim generoError_ As Boolean = False

            Dim sbCadena_ = New System.Text.StringBuilder()

            Dim rtbMensaje_ As New System.Windows.Forms.RichTextBox

            sbCadena_.Append("{\rtf1\ansi")

            sbCadena_.Append("{\colortbl ; \red255\green60\blue51; \red0\green0\blue255;}")

            Dim respuesta_ As String = Nothing

            For Each fila_ As System.Windows.Forms.DataGridViewRow In gsCatalogo_.DgvCatalogo.Rows

                If fila_.Selected Then

                    _estatus.SetOK()

                    CambiarEstatusDocumento(claveDocumento_:=fila_.Cells(nombreCampoEntornoClaveDocumento_).Value,
                                            estatusDocumento_:=estatusDocumento_,
                                            operacionesCatalogo_:=operacionesCatalogo_)

                    If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoClaveDocumento_).Value &
                                         "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Estatus cambiado. \b0") &
                                         "\par ******************************************************")

                        generoError_ = True

                    End If

                End If

            Next

            sbCadena_.Append("}")

            rtbMensaje_.Rtf = sbCadena_.ToString()

            If generoError_ Then

                _sistema.GsDialogo(Nothing, rtbMensaje_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

            Else
                _sistema.GsDialogo(Nothing, "Estatus modificados", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            End If

        End Sub

        Public Sub CambiarEstatusDocumento(ByVal claveDocumento_ As Integer,
                                           Optional ByVal estatusDocumento_ As Documento.EstatusDocumento = gsol.documento.Documento.EstatusDocumento.Habilitado,
                                           Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            _documento.Clave = claveDocumento_

            _documento.TipoPrivilegio = gsol.documento.Documento.TiposPrivilegios.Modificar

            BuscarPrivilegioUsuario()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                Dim OCExtensionDocumento_ = New OperacionesCatalogo

                OCExtensionDocumento_ = _sistema.EnsamblaModulo("ExtensionRegistroDocumentos")

                With OCExtensionDocumento_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    .CampoPorNombre("i_Cve_Estatus") = estatusDocumento_

                    .EditaCampoPorNombre("i_ConsultableCliente").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                End With

                If OCExtensionDocumento_.Modificar(claveDocumento_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                    RegenerarCaracteristicasExtension(claveDocumento_)

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1040)

                End If

            End If

        End Sub

        Public Sub GenerarImagenesCaracteristicasVUCEM(ByVal claveDocumento_ As Integer)

            Dim listaImagenes_ As List(Of String) = New List(Of String)

            Dim pdf_ = New PdfFocus

            Dim jpegDir_ As String = Path.GetDirectoryName("M:\MDO\CorreoElectronico\RKU\2020\9\5\notificaciones@kromaduanal.com\2020_9_5_DCE.pdf")

            pdf_.OpenPdf("M:\MDO\CorreoElectronico\RKU\2020\9\5\notificaciones@kromaduanal.com\2020_9_5_DCE.pdf")

            If pdf_.PageCount > 0 Then

                pdf_.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg

                pdf_.ImageOptions.Dpi = 300

                pdf_.ImageOptions.ColorDepth = PdfFocus.CImageOptions.eColorDepth.Grayscale8bpp

                pdf_.ImageOptions.JpegQuality = 95

                For pagina_ As Integer = 1 To pdf_.PageCount

                    Dim jpegFile_ As String = Path.Combine(jpegDir_, String.Format("Temporal_PageKB {0}.jpg", pagina_))

                    Dim resultado_ As Integer = pdf_.ToImage(jpegFile_, pagina_)

                    Select Case resultado_

                        Case 2

                            _sistema.GsDialogo("No se pudo crear el listado de imágenes, es necesario checar la ruta de colocación", TipoDialogo.Err)

                            Exit Sub

                        Case 3

                            _sistema.GsDialogo("No se pudo crear el listado de imágenes, error de conversión", TipoDialogo.Err)

                            Exit Sub

                        Case 0

                            listaImagenes_.Add(jpegFile_)

                    End Select

                Next pagina_

            End If

        End Sub

        Public Sub ObtenerRutaDocumento(ByVal claveDocumento_ As Integer,
                                        Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            Dim respuesta_ As TagWatcher

            Dim query_ As String = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT top 1 * FROM VT012VisorMaestroDocumentos WHERE i_Cve_documento= " & claveDocumento_ & " ORDER BY i_orden ASC"

            respuesta_ = _sistema.ComandosSingletonSQL(query_)

            If respuesta_.Status = TagWatcher.TypeStatus.Ok Then

                Dim cursorTable_ As New System.Data.DataTable

                cursorTable_ = DirectCast(respuesta_.ObjectReturned, System.Data.DataTable)

                For Each fila_ As DataRow In cursorTable_.Rows

                    _documento.NombreDocumento = fila_.Item("t_NombreDocumento").ToString

                    _documento.RutaDocumentoCompleto = fila_.Item("t_RutaDocumento").ToString

                    _documento.RutaDocumentoContingenciaCompleto = fila_.Item("t_RutaDocumentoContingencia").ToString

                Next

                Try

                    If Not File.Exists(_documento.RutaDocumentoCompleto) Then

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1041)

                    End If

                Catch ex As Exception

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1042)

                End Try

            End If

        End Sub

        Public Sub ProcesarDocumentoListaImagenes(ByVal listaImagenes_ As List(Of String),
                                                  Optional ByVal claveDocumentoPadre_ As Integer = 0,
                                                  Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            BuscarPrivilegioUsuarioDocumento()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                CargarRepositoriosListaImagenes()

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    ValidarCaracteristicasPlantilla()

                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                        ValidarDocumentosDuplicados()

                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                            CrearDirectorio()

                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                GuardarDocumentoListaImagenes(listaImagenes_)

                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                    InsertarRegistros()

                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                        InsertarRegistrosCaracteristicas()

                                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                            InsertarRegistroVinculacion()

                                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                InsertarRegistroDocumentosDivididos(claveDocumentoPadre_)

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If

                End If

            End If

        End Sub

        Private Sub GuardarDocumentoListaImagenes(ByVal listaImagenes_ As List(Of String),
                                                  Optional ByVal esDocument_ As Boolean = False)

            Dim nombreDocumento_ = _documento.NombreDocumento

            Dim contador_ = 2

            Try

                If Not Directory.Exists(_documento.RutaDocumentoCompleto) Then

                    My.Computer.FileSystem.CreateDirectory(_documento.RutaDocumentoCompleto)

                End If

                While True

                    If File.Exists(_documento.RutaDocumentoCompleto & "" & nombreDocumento_ & _documento.Extension) Then

                        nombreDocumento_ = _documento.NombreDocumento & "_" & contador_

                        contador_ = contador_ + 1

                    Else
                        _documento.NombreDocumento = nombreDocumento_ & _documento.Extension

                        ProcesoGuardadoImagenAPDF(listaImagenes_:=listaImagenes_,
                                                  rutaDocumento_:=_documento.RutaDocumentoCompleto,
                                                  nombreDocumento_:=_documento.NombreDocumento)

                        If esDocument_ Then

                            ObtenerRutaRepositorioAternativoExpedienteOperativo()

                            If Not IsNothing(_documento.RutaRepositorioAlternativoExpedienteOperativo) Then

                                ProcesoGuardadoImagenAPDF(listaImagenes_:=listaImagenes_,
                                                          rutaDocumento_:=_documento.RutaRepositorioAlternativoExpedienteOperativo & _referencia & "\",
                                                          nombreDocumento_:=_documento.NombreDocumento,
                                                          revisarExisteRuta_:=True)

                            End If

                        End If

                        Exit While

                    End If

                End While

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1007)

            End Try

        End Sub

        Private Sub ProcesoGuardadoImagenAPDF(ByVal listaImagenes_ As List(Of String),
                                              ByVal rutaDocumento_ As String,
                                              ByVal nombreDocumento_ As String,
                                              Optional revisarExisteRuta_ As Boolean = False)

            Try


                If revisarExisteRuta_ Then

                    If Not Directory.Exists(rutaDocumento_) Then

                        My.Computer.FileSystem.CreateDirectory(rutaDocumento_)

                    End If

                End If

                Dim documento_ = New iTextSharp.text.Document(PageSize.A4, 0, 0, 0, 0)

                Dim documentoStream_ = New FileStream(rutaDocumento_ & nombreDocumento_,
                                                      FileMode.Create,
                                                      FileAccess.Write,
                                                      FileShare.None)

                PdfWriter.GetInstance(documento_,
                                      documentoStream_)

                documento_.Open()

                For Each imagen_ As String In listaImagenes_

                    Dim imageStream_ = New FileStream(imagen_,
                                                     FileMode.Open,
                                                     FileAccess.Read,
                                                     FileShare.ReadWrite)

                    Dim image_ = Image.GetInstance(imageStream_)

                    image_.Alignment = Element.ALIGN_CENTER

                    image_.ScaleToFit(documento_.PageSize.Width,
                                      documento_.PageSize.Height)

                    documento_.Add(image_)

                Next

                documento_.Close()

                'Dim IM_ As Integer = Nothing

                'Dim indexImagen_ As Integer = Nothing

                'Dim AxDocumentopdf_ = New AxPDFBuilderX.AxPDFDoc()

                'AxDocumentopdf_.CreateControl()

                'With AxDocumentopdf_

                '    .DefaultPageSize = PDFBuilderX.TxPageSize.psLetter_P

                '    .Units = PDFBuilderX.TxUnitsMeas.umMillimeters

                '    .Clear()

                '    Dim pagina_ = 1

                '    For Each imagen_ As String In listaImagenes_



                '        .AddPage(pagina_)

                '        IM_ = .AddImageFile(imagen_)

                '        indexImagen_ = .ApplyResource(pagina_, IM_)

                '         .ScaleObject(pagina_, indexImagen_,100)

                '        .Locate(pagina_, indexImagen_, 0, .get_PageHeight(IM_))

                '        pagina_ = pagina_ + 1

                '    Next

                '    .SaveToFile(_documento.RutaDocumentoCompleto & "" & _documento.NombreDocumento)

                '    .Clear()

                'End With

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1007)

            End Try

        End Sub

        Private Sub GuardarDocumentoListaImagenesPrueba(ByVal listaImagenes_ As List(Of String))

            'Dim IM_ As Integer = Nothing

            'Dim indexImagen_ As Integer = Nothing

            'Dim AxDocumentopdf_ = New AxPDFBuilderX.AxPDFDoc()

            'AxDocumentopdf_.CreateControl()

            'With AxDocumentopdf_

            '    .DefaultPageSize = PDFBuilderX.TxPageSize.psLetter_P

            '    .Units = PDFBuilderX.TxUnitsMeas.umMillimeters

            '    .Clear()

            '    Dim pagina_ = 1

            '    For Each imagen_ As String In listaImagenes_

            '        .AddPage(pagina_)

            '        IM_ = .AddImageFile(imagen_)

            '        indexImagen_ = .ApplyResource(pagina_, IM_)

            '        .ScaleObject(pagina_, indexImagen_, 100)

            '        .Locate(pagina_, indexImagen_, 0, .get_PageHeight(IM_))

            '        pagina_ = pagina_ + 1

            '    Next

            '    .SaveToFile("M:\MDO\CorreoElectronico\RKU\2020\9\5\notificaciones@kromaduanal.com\DocumentoVUCEM.pdf")

            'End With

        End Sub

        Public Sub BuscarInformacionReferencia()

            If Not _referencia Is Nothing Then

                Dim dataTable_ As System.Data.DataTable = Nothing

                Dim consulta_ As String = Nothing

                consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                            "select RFCCliente, ClaveAduana, Anio, Patente, NumeroPedimento, NombreAduana, Cliente from VT012Operaciones where NumeroReferencia =  '" & _referencia & "'"

                _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                    dataTable_ = DirectCast(_estatus.ObjectReturned, System.Data.DataTable)

                    If dataTable_.Rows.Count > 0 Then

                        For contador_ As Integer = 1 To _documento.PlantillaDocumento.CaracteristicasDocumentos.Count

                            If _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 53 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(0).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 51 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(1).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 55 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(2).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 52 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(3).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 63 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(4).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 68 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(5).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 67 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = dataTable_(0)(6).ToString

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 49 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = _referencia

                            ElseIf _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 87 Then

                                _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor = "CLIENTE"

                            End If

                        Next contador_

                    Else

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1032)

                    End If

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1032)

            End If

        End Sub

        Private Sub IncrementaContadorEdocument()

            Try

                Dim caracteristicas_ = New List(Of CaracteristicaCatalogo)

                With caracteristicas_

                    .Add(New CaracteristicaCatalogo With {.Nombre = "i_Cve_MaestroOperaciones",
                                                          .ValorAsignado = _claveMaestroOperaciones})

                    .Add(New CaracteristicaCatalogo With {.Nombre = "i_Cve_DivisionMiEmpresa",
                                                         .ValorAsignado = _documento.ClaveDivisionMiEmpresa})

                End With

                Dim _politicasBaseDatos As BaseDatos.PoliticasBaseDatos =
                    New PoliticasBaseDatos(_operaciones,
                                           35,
                                           caracteristicas_,
                                           IPoliticasBaseDatos.TiposProcedimientos.Funcion,
                                           IPoliticasBaseDatos.VerificarCampos.No)

                Dim tag_ = New TagWatcher

                tag_ = _politicasBaseDatos.GetTagWatcher

                Dim cursorDataTable_ As New System.Data.DataTable

                cursorDataTable_ = DirectCast(tag_.ObjectReturned, System.Data.DataTable)

                If tag_.Status = TagWatcher.TypeStatus.Ok Then

                    _estatus.SetOK()

                    If cursorDataTable_.Rows.Count > 0 Then

                        If cursorDataTable_.Rows(0)("i_numeroEdocument") >= 1 Then

                            _documento.NombreDocumento = _documento.NombreDocumento & "_" & (cursorDataTable_.Rows(0)("i_numeroEdocument") + 1)

                        End If

                    End If

                End If

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1058)

            End Try

        End Sub

        Public Sub ProcesarDocumentoEdocumentListaImagenes(ByVal listaImagenes_ As List(Of String),
                                                           ByVal claveDocumentoPadre_ As Integer,
                                                           ByVal claveDocumentoVUCEM_ As Integer,
                                                           Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            BuscarPrivilegioUsuarioDocumento()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                CargarRepositoriosListaImagenes()

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    ValidarCaracteristicasPlantilla()

                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                        IncrementaContadorEdocument()

                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                            ValidarDocumentosDuplicados()

                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                CrearDirectorio()

                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                    GuardarDocumentoListaImagenes(listaImagenes_:=listaImagenes_,
                                                              esDocument_:=True)

                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                        InsertarRegistros()

                                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                            InsertarRegistrosCaracteristicas()

                                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                InsertarRegistroVinculacion()

                                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                    InsertarRegistroDocumentosDivididos(claveDocumentoPadre_)

                                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                        InsertarRegistroEdocuments(claveDocumentoVUCEM_)

                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If


                    End If

                End If

            End If

        End Sub

        Private Sub CargarRepositoriosListaImagenes()

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1

            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "PlantillasDocumentos",
                                                          " and i_cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & "" &
                                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    _documento.ClaveTipoDocumento = fila_.Item("Clave TipoDocumento").ToString

                    _documento.TipoDocumento = fila_.Item("TipoDocumento").ToString

                Next

            End If

            If Not _documento.ClaveTipoDocumento = 0 Then

                Dim tipoDocumento_ = New OperacionesCatalogo

                tipoDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                tipoDocumento_.CantidadVisibleRegistros = 1

                tipoDocumento_ = _sistema.ConsultaModulo(tipoDocumento_.EspacioTrabajo,
                                                         "TiposDocumentos",
                                                         " and i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento)

                If _sistema.TieneResultados(tipoDocumento_) Then

                    For Each fila_ As DataRow In tipoDocumento_.Vista.Tables(0).Rows

                        Dim repositorioDigital_ = New OperacionesCatalogo

                        repositorioDigital_.EspacioTrabajo = _operaciones.EspacioTrabajo

                        repositorioDigital_.CantidadVisibleRegistros = 1

                        repositorioDigital_ = _sistema.ConsultaModulo(repositorioDigital_.EspacioTrabajo,
                                                                      "RepositoriosDigitales",
                                                                      " and i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento &
                                                                      " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa)

                        If _sistema.TieneResultados(repositorioDigital_) Then

                            For Each fila2_ As DataRow In repositorioDigital_.Vista.Tables(0).Rows

                                _documento.RutaDocumento = Nothing

                                _documento.RutaDocumentoCompleto = fila2_.Item("Servidor").ToString & fila2_.Item("Ruta").ToString & "\"

                                _documento.RutaDocumentoContingencia = Nothing

                                _documento.RutaDocumentoContingenciaCompleto = fila2_.Item("Servidor contingencia").ToString & fila2_.Item("Ruta contingencia").ToString & "\"

                                _documento.ClaveRepositorioDigital = fila2_.Item("Clave").ToString

                                _documento.TipoArchivo = fila_.Item("Clave tipo de archivo").ToString

                                _documento.Extension = ".pdf"

                                _documento.NombreDocumento = ""

                                If Not fila_.Item("Extensión").ToString = _documento.Extension Then

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1009)

                                End If

                            Next

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1002)

                        End If

                    Next

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1001)

                End If

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1000)

            End If

        End Sub

        Private Sub InsertarRegistroDocumentosDivididos(ByVal claveDocumentoPadre_ As Integer)

            If claveDocumentoPadre_ > 0 Then

                Dim documentosDivididos_ = New OperacionesCatalogo

                documentosDivididos_.EspacioTrabajo = _operaciones.EspacioTrabajo

                documentosDivididos_ = _sistema.EnsamblaModulo("DocumentosDivididos")

                With documentosDivididos_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    .CampoPorNombre("i_Cve_DocumentoPadre") = claveDocumentoPadre_

                    .CampoPorNombre("i_Cve_DocumentoHijo") = _documento.Clave

                    .CampoPorNombre("i_Cve_Usuario") = _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

                    .CampoPorNombre("i_Cve_Estatus") = 1

                    .CampoPorNombre("i_Cve_Estado") = 1

                    .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                    If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1043)

                    End If

                End With

            End If

        End Sub

        Private Sub InsertarRegistroEdocuments(ByVal claveDocumentoVUCEM_ As Integer)

            If _claveMaestroOperaciones > 0 Then

                Dim plantillaDocumento_ = New OperacionesCatalogo

                plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                plantillaDocumento_.CantidadVisibleRegistros = 1

                plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                              "TiposDocumentosVUCEM",
                                                              " and spc_i_Cve_TipoDocumentoVUCEM = " & claveDocumentoVUCEM_)


                If _sistema.TieneResultados(plantillaDocumento_) Then

                    For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                        claveDocumentoVUCEM_ = fila_.Item("Clave").ToString

                    Next

                    Dim documentosDivididos_ = New OperacionesCatalogo

                    documentosDivididos_.EspacioTrabajo = _operaciones.EspacioTrabajo

                    documentosDivididos_ = _sistema.EnsamblaModulo("DocumentosEdocuments")

                    With documentosDivididos_

                        .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                        .CampoPorNombre("i_Cve_Documento") = _documento.Clave

                        .CampoPorNombre("i_Cve_MaestroOperaciones") = _claveMaestroOperaciones

                        .CampoPorNombre("i_Cve_TipoDocumentoVUCEM") = claveDocumentoVUCEM_

                        .CampoPorNombre("i_Cve_Estatus") = 1

                        .CampoPorNombre("i_Cve_Estado") = 1

                        .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _documento.ClaveDivisionMiEmpresa

                        If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1044)

                        End If

                    End With

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1045)

                End If

            End If

        End Sub

        Public Sub BuscarDocumentosPorPlantillaCuentaGastos(ByVal claveFactura_ As Integer,
                                                             Optional ByVal claveEncPlantillaDocumento_ As Integer = 0,
                                                             Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1000

            Dim sentenciaSQL_ As String = " and i_Cve_Factura = " & claveFactura_ & " " &
                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa & " " &
                                          IIf(claveEncPlantillaDocumento_ > 0, " and i_Cve_EncPlantillaDocumento = " & claveEncPlantillaDocumento_ & " ", "")


            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "DocumentosFacturas",
                                                          sentenciaSQL_)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                Dim documentos_ = New List(Of Documento)

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    Dim documento_ = New Documento

                    documento_.Clave = fila_.Item("Clave Documento").ToString

                    documento_.NombreDocumento = fila_.Item("Nombre Documento").ToString

                    documento_.DocumentoConsultableCliente = IIf(fila_.Item("Clave ConsultableCliente").ToString, 1, 0)

                    documento_.ClaveDivisionMiEmpresa = fila_.Item("Clave DivisionMiEmpresa").ToString

                    documento_.PlantillaDocumento.ClavePlantilla = fila_.Item("Clave EncPlantillaDocumento").ToString

                    documento_.PlantillaDocumento.NombrePlantilla = fila_.Item("Plantilla").ToString

                    documentos_.Add(documento_)

                Next

                _estatus.ObjectReturned = documentos_

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1046)

            End If

        End Sub

        Public Sub BuscarDocumentosPorCuentaGastos(ByVal claveFactura_ As Integer,
                                                   ByVal listaClavesPlantillasDocumento_ As List(Of Integer))

            _estatus.SetOK()

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1000

            _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            Dim sentenciaSQL_ As String = " and i_Cve_Factura = " & claveFactura_ & " " &
                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa & " " &
                                          If(listaClavesPlantillasDocumento_.Count > 0, " and i_Cve_EncPlantillaDocumento IN (" & String.Join(",", listaClavesPlantillasDocumento_) & ") ", "")


            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "DocumentosFacturas",
                                                          sentenciaSQL_)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                Dim documentos_ = New List(Of Documento)

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    Dim documento_ = New Documento

                    documento_.Clave = fila_.Item("Clave Documento").ToString

                    documento_.NombreDocumento = fila_.Item("Nombre Documento").ToString

                    documento_.DocumentoConsultableCliente = IIf(fila_.Item("Clave ConsultableCliente").ToString, 1, 0)

                    documento_.ClaveDivisionMiEmpresa = fila_.Item("Clave DivisionMiEmpresa").ToString

                    documento_.PlantillaDocumento.ClavePlantilla = fila_.Item("Clave EncPlantillaDocumento").ToString

                    documento_.PlantillaDocumento.NombrePlantilla = fila_.Item("Plantilla").ToString

                    documentos_.Add(documento_)

                Next

                _estatus.ObjectReturned = documentos_

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1046)

            End If

        End Sub

        Public Sub BuscarDocumentosPorPlantillaMaestroOperaciones(ByVal claveMaestroOperaciones_ As Integer,
                                                                  Optional ByVal claveEncPlantillaDocumento_ As Integer = 0,
                                                                  Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1000

            Dim sentenciaSQL_ As String = " and i_Cve_MaestroOperaciones = " & claveMaestroOperaciones_ & " " &
                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa & " " &
                                          IIf(claveEncPlantillaDocumento_ > 0, " and i_Cve_EncPlantillaDocumento = " & claveEncPlantillaDocumento_ & " ", "")


            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "DocumentosMaestroOperaciones",
                                                          sentenciaSQL_)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                Dim documentos_ = New List(Of Documento)

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    Dim documento_ = New Documento

                    documento_.Clave = fila_.Item("Clave Documento").ToString

                    documento_.NombreDocumento = fila_.Item("Nombre Documento").ToString

                    documento_.DocumentoConsultableCliente = IIf(fila_.Item("Clave ConsultableCliente").ToString, 1, 0)

                    documento_.ClaveDivisionMiEmpresa = fila_.Item("Clave DivisionMiEmpresa").ToString

                    documento_.PlantillaDocumento.ClavePlantilla = fila_.Item("Clave EncPlantillaDocumento").ToString

                    documento_.PlantillaDocumento.NombrePlantilla = fila_.Item("Plantilla").ToString

                    documentos_.Add(documento_)

                Next

                _estatus.ObjectReturned = documentos_

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1047)

            End If

        End Sub

        Public Sub BuscarDocumentosPorMaestroOperaciones(ByVal claveMaestroOperaciones_ As Integer, ByVal listaClavePlantillas_ As List(Of Integer))

            _estatus.SetOK()

            _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            Dim plantillaDocumento_ = New OperacionesCatalogo

            plantillaDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

            plantillaDocumento_.CantidadVisibleRegistros = 1000

            Dim sentenciaSQL_ As String = " and i_Cve_MaestroOperaciones = " & claveMaestroOperaciones_ &
                                          " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa & " " &
                                          " and i_Cve_EncPlantillaDocumento IN (" & String.Join(",", listaClavePlantillas_) & ")" &
                                          " and i_ConsultableCliente = 1"


            plantillaDocumento_ = _sistema.ConsultaModulo(plantillaDocumento_.EspacioTrabajo,
                                                          "DocumentosMaestroOperaciones",
                                                          sentenciaSQL_)

            If _sistema.TieneResultados(plantillaDocumento_) Then

                Dim documentos_ = New List(Of Documento)

                For Each fila_ As DataRow In plantillaDocumento_.Vista.Tables(0).Rows

                    Dim documento_ = New Documento

                    documento_.Clave = fila_.Item("Clave Documento").ToString

                    documento_.NombreDocumento = fila_.Item("Nombre Documento").ToString

                    documento_.DocumentoConsultableCliente = IIf(fila_.Item("Clave ConsultableCliente").ToString, 1, 0)

                    documento_.ClaveDivisionMiEmpresa = fila_.Item("Clave DivisionMiEmpresa").ToString

                    documento_.PlantillaDocumento.ClavePlantilla = fila_.Item("Clave EncPlantillaDocumento").ToString

                    documento_.PlantillaDocumento.NombrePlantilla = fila_.Item("Plantilla").ToString

                    documentos_.Add(documento_)

                Next

                _estatus.ObjectReturned = documentos_

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1047)

            End If

        End Sub

        Public Sub CambiarConsultableClienteDocumentoCatalogo(ByVal gsCatalogo_ As GsCatalogo,
                                                              Optional ByVal nombreCampoEntornoClaveDocumento_ As String = "Clave Documento",
                                                              Optional ByVal consultableCliente_ As Documento.ConsultableClientes = gsol.documento.Documento.ConsultableClientes.No,
                                                              Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)


            _estatus.SetOK()

            Dim generoError_ As Boolean = False

            Dim sbCadena_ = New System.Text.StringBuilder()

            Dim rtbMensaje_ As New System.Windows.Forms.RichTextBox

            sbCadena_.Append("{\rtf1\ansi")

            sbCadena_.Append("{\colortbl ; \red255\green60\blue51; \red0\green0\blue255;}")

            Dim respuesta_ As String = Nothing

            For Each fila_ As System.Windows.Forms.DataGridViewRow In gsCatalogo_.DgvCatalogo.Rows

                If fila_.Selected Then

                    _estatus.SetOK()

                    CambiarConsultableClienteDocumento(claveDocumento_:=fila_.Cells(nombreCampoEntornoClaveDocumento_).Value,
                                                       consultableCliente_:=consultableCliente_,
                                                       operacionesCatalogo_:=operacionesCatalogo_)

                    If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoClaveDocumento_).Value &
                                         "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Estatus cambiado. \b0") &
                                         "\par ******************************************************")

                        generoError_ = True

                    End If

                End If

            Next

            sbCadena_.Append("}")

            rtbMensaje_.Rtf = sbCadena_.ToString()

            If generoError_ Then

                _sistema.GsDialogo(Nothing, rtbMensaje_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

            Else
                _sistema.GsDialogo(Nothing, "Consutable cliente actualizado", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            End If

        End Sub

        Public Sub CambiarConsultableClienteDocumento(ByVal claveDocumento_ As Integer,
                                                      Optional ByVal consultableCliente_ As Documento.ConsultableClientes = gsol.documento.Documento.ConsultableClientes.No,
                                                      Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            _documento.Clave = claveDocumento_

            _documento.TipoPrivilegio = gsol.documento.Documento.TiposPrivilegios.Modificar

            BuscarPrivilegioUsuario()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                Dim OCExtensionDocumento_ = New OperacionesCatalogo

                OCExtensionDocumento_ = _sistema.EnsamblaModulo("ExtensionRegistroDocumentos")

                With OCExtensionDocumento_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    .CampoPorNombre("i_ConsultableCliente") = consultableCliente_

                    .EditaCampoPorNombre("i_Cve_Estatus").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                End With

                If Not OCExtensionDocumento_.Modificar(claveDocumento_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1048)

                End If

            End If

        End Sub

        Private Sub RegenerarCaracteristicasExtension(ByVal claveDocumento_ As Integer)

            Dim query_ As String = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                "DELETE from Ext012CaracteristicasExtDocumentos where i_Cve_Documento = " & claveDocumento_ & " " &
                "INSERT INTO [dbo].[Ext012CaracteristicasExtDocumentos]" &
                "           ([i_Cve_VinCaracteristicasExtDocumentos]" &
                "           ,[i_Cve_Documento]" &
                "           ,[t_NombreDocumento]" &
                "           ,[t_URLPublico]" &
                "           ,[i_Cve_DivisionMiEmpresa]" &
                "           ,[i_Cve_CaracteristicaDocumento]" &
                "           ,[t_Titulo]" &
                "           ,[t_Valor]" &
                "           ,[i_Orden]" &
                "           ,[i_Cve_FormatoCaracteristica]" &
                "           ,[f_Registro]" &
                "           ,[i_Cve_RepositorioDigital]" &
                "           ,[i_cve_EncPlantillaDocumento]" &
                "           ,[i_Cve_TipoArchivo]" &
                "           ,[i_Cve_Estatus]" &
                "           ,[i_Cve_Estado]" &
                "           ,[t_RutaDocumento]" &
                "           ,[t_RutaDocumentoContingencia]" &
                "           ,[t_DisplayField]" &
                "           ,[t_KeyField]" &
                "           ,[t_NameAsKey]" &
                "           ,[t_PermissionNumber]" &
                "           ,[i_Cve_TipoDocumento]" &
                 "          ,[t_EjecutivoRegistro]" &
                 "          ,[i_Cve_TipoArchivoVirtualizado]" &
                 "          ,[t_ExtensionVirtualizada])" &
                "select * " &
                "from VT012MaestroDocumentosLista_ " &
                "where i_Cve_Documento = " & claveDocumento_

            _estatus = _sistema.ComandosSingletonSQL(query_, "i_Cve_Documento")

            If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1019)

            End If

        End Sub

        Public Sub CambiarEstatusEDocumentCatalogo(ByVal gsCatalogo_ As GsCatalogo,
                                                   ByVal referencia_ As String,
                                                   Optional ByVal nombreCampoEntornoClaveDocumento_ As String = "Clave Documento",
                                                   Optional ByVal nombreCampoEntornoNombreDocumento_ As String = "Nombre Documento",
                                                   Optional ByVal estatusDocumento_ As Documento.EstatusDocumento = gsol.documento.Documento.EstatusDocumento.Habilitado,
                                                   Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)


            _estatus.SetOK()

            Dim generoError_ As Boolean = False

            Dim sbCadena_ = New System.Text.StringBuilder()

            Dim rtbMensaje_ As New System.Windows.Forms.RichTextBox

            sbCadena_.Append("{\rtf1\ansi")

            sbCadena_.Append("{\colortbl ; \red255\green60\blue51; \red0\green0\blue255;}")

            Dim respuesta_ As String = Nothing

            ObtenerRutaRepositorioAternativoExpedienteOperativo()

            For Each fila_ As System.Windows.Forms.DataGridViewRow In gsCatalogo_.DgvCatalogo.Rows

                If fila_.Selected Then

                    _estatus.SetOK()

                    CambiarEstatusEDocument(claveDocumento_:=fila_.Cells(nombreCampoEntornoClaveDocumento_).Value,
                                            nombreDocumento_:=fila_.Cells(nombreCampoEntornoNombreDocumento_).Value,
                                            referencia_:=referencia_,
                                            estatusDocumento_:=estatusDocumento_,
                                            operacionesCatalogo_:=operacionesCatalogo_)

                    If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoClaveDocumento_).Value &
                                         "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Estatus cambiado. \b0") &
                                         "\par ******************************************************")

                        generoError_ = True

                    End If

                End If

            Next

            sbCadena_.Append("}")

            rtbMensaje_.Rtf = sbCadena_.ToString()

            If generoError_ Then

                _sistema.GsDialogo(Nothing, rtbMensaje_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

            Else
                _sistema.GsDialogo(Nothing, "Estatus modificados", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            End If

        End Sub

        Public Sub CambiarEstatusEDocument(ByVal claveDocumento_ As Integer,
                                           ByVal nombreDocumento_ As String,
                                           ByVal referencia_ As String,
                                           Optional ByVal estatusDocumento_ As Documento.EstatusDocumento = gsol.documento.Documento.EstatusDocumento.Habilitado,
                                           Optional ByVal operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

            End If

            _documento.Clave = claveDocumento_

            _documento.TipoPrivilegio = gsol.documento.Documento.TiposPrivilegios.Modificar

            BuscarPrivilegioUsuario()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                Dim OCExtensionDocumento_ = New OperacionesCatalogo

                OCExtensionDocumento_ = _sistema.EnsamblaModulo("ExtensionRegistroDocumentos")

                With OCExtensionDocumento_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                    .CampoPorNombre("i_Cve_Estatus") = estatusDocumento_

                    .EditaCampoPorNombre("i_ConsultableCliente").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                End With

                If OCExtensionDocumento_.Modificar(claveDocumento_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                    RegenerarCaracteristicasExtension(claveDocumento_)

                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                        If estatusDocumento_ = gsol.documento.Documento.EstatusDocumento.Deshabilitado _
                            Or estatusDocumento_ = gsol.documento.Documento.EstatusDocumento.Eliminado Then

                            EliminarDocumentoRutaRepositorioAlternativoExpedienteOperativo(nombreDocumento_:=nombreDocumento_,
                                                                                           referencia_:=referencia_)

                        End If

                    End If

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1040)

                End If

            End If

        End Sub

        Private Sub ObtenerRutaRepositorioAternativoExpedienteOperativo()

            Dim rutaRepositorioAlternativoExpedienteOperativo_ As String = Nothing

            Dim IOMisEmpresas_ = New OperacionesCatalogo

            IOMisEmpresas_.EspacioTrabajo = _operaciones.EspacioTrabajo

            IOMisEmpresas_.CantidadVisibleRegistros = 1

            IOMisEmpresas_ = _sistema.ConsultaModulo(IOMisEmpresas_.EspacioTrabajo,
                                                          "MisEmpresas",
                                                          " and i_Cve_DivisionMiEmpresa = " & _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria)

            If _sistema.TieneResultados(IOMisEmpresas_) Then

                For Each fila_ As DataRow In IOMisEmpresas_.Vista.Tables(0).Rows

                    If Not IsDBNull(fila_.Item("Ruta repositorio alternativo expediente operativo").ToString) Then

                        If Not IsNothing(fila_.Item("Ruta repositorio alternativo expediente operativo").ToString) Then

                            If fila_.Item("Ruta repositorio alternativo expediente operativo").ToString <> "" Then

                                _documento.RutaRepositorioAlternativoExpedienteOperativo = fila_.Item("Ruta repositorio alternativo expediente operativo").ToString

                                If Not IsNothing(_documento.RutaRepositorioAlternativoExpedienteOperativo) Then

                                    If _documento.RutaRepositorioAlternativoExpedienteOperativo = "" Then

                                        _documento.RutaRepositorioAlternativoExpedienteOperativo = Nothing

                                    End If

                                End If

                            End If

                        End If

                    End If

                Next

            End If

        End Sub

        Private Sub EliminarDocumentoRutaRepositorioAlternativoExpedienteOperativo(ByVal nombreDocumento_ As String,
                                                                                   ByVal referencia_ As String)

            Dim rutaDocumento_ As String = _documento.RutaRepositorioAlternativoExpedienteOperativo & referencia_ & "\" & nombreDocumento_

            Try

                If File.Exists(rutaDocumento_) Then

                    File.Delete(rutaDocumento_)

                End If

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1049)

            End Try

        End Sub

#End Region

#Region "Funciones"

        Private Function EliminarCaracteresEnTexto(ByVal texto_ As String) As String

            If Not IsNothing(texto_) Then

                texto_ = (texto_.Replace("'", "")).Replace("""", "")

                Return texto_

            End If

            Return texto_

        End Function

#End Region

    End Class

End Namespace