Imports gsol.basedatos.Operaciones
Imports gsol
Imports System.IO
Imports System.Security
Imports System.Threading
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Wma.Exceptions
Imports gsol.documento


Public Class ControladorRecolectorDocumentos64

#Region "Atributos"

    Private _sesion As ISesion

    Private _estatusSesion As Boolean

    Private _ioperacionescatalogo As OperacionesCatalogo

    Private _modalidadoperativa As IOperacionesCatalogo.TiposOperacionSQL

    Private _sistema As Organismo

    Private _espacioTrabajo As EspacioTrabajo

    Private _listaRecolecciones As List(Of RecolectorDocumentos64)

    Private _tagWatcher As New TagWatcher

    Private Shared _cantidadHilos As Integer

    Private Const CantidadMaxHilos As Integer = 100

    Private Const CantidadMaxDocumentosPorHilo As Integer = 1000

#End Region

#Region "Constructores"

    Public Sub New()

        _ioperacionescatalogo = New OperacionesCatalogo

        _modalidadoperativa = IOperacionesCatalogo.TiposOperacionSQL.SinDefinir

        _sistema = New Organismo

        _estatusSesion = False

        _tagWatcher.SetOK()

        IniciarSesion()

        _ioperacionescatalogo.EspacioTrabajo = _espacioTrabajo

        _ioperacionescatalogo.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

        _listaRecolecciones = New List(Of RecolectorDocumentos64)

        _cantidadHilos = 0

    End Sub

#End Region

#Region "Propiedades"

    Public ReadOnly Property OperacionesCatalogo As OperacionesCatalogo
        Get
            Return _ioperacionescatalogo
        End Get
    End Property

    Public ReadOnly Property EspacioTrabajo As EspacioTrabajo
        Get
            Return _espacioTrabajo
        End Get
    End Property

#End Region

#Region "Metodos"

    Private Sub IniciarSesion()

        _sesion = New Sesion

        _sesion.IdentificadorUsuario = "desarrollo@kromaduanal.com"

        _sesion.ContraseniaUsuario = "DesarrolloKROM19"

        _sesion.GrupoEmpresarial = 1

        _sesion.DivisionEmpresarial = 1

        _sesion.Aplicacion = 4

        _sesion.Idioma = ISesion.Idiomas.Espaniol

        _estatusSesion = _sesion.StatusArgumentos ' Autenticación

        If _estatusSesion Then

            _espacioTrabajo = _sesion.EspacioTrabajo

        End If

    End Sub

    Public Sub IniciarRecoleccion()

        Try

            'Obtener Recolectores EN PROCESO

            Dim dataTable_ As DataTable = Nothing

            dataTable_ = ConsultarRecolectoresDocumentos("WHERE i_Cve_Estatus = 3")

            Dim recolector_ As RecolectorDocumentos64

            ' Crea recolectores por cada registro

            For Each row As DataRow In dataTable_.Rows

                recolector_ = New RecolectorDocumentos64()

                recolector_.Clave = row.Item("i_Cve_RecolectorDocumentos").ToString

                recolector_.Nombre = row.Item("t_Nombre").ToString

                recolector_.RutaRaiz = row.Item("t_RutaRaiz").ToString

                recolector_.SeparadorSubdirectorios = row.Item("t_SeparadorSubdirectorios").ToString

                recolector_.DivisionMiEmpresa = row.Item("i_Cve_DivisionMiEmpresa").ToString

                ' Escuchar Directorio (FileWatcher)

                recolector_.ProcesoEscuchaDirectorio = New Thread(New ParameterizedThreadStart(AddressOf EscuchaDirectorio))

                recolector_.ProcesoEscuchaDirectorio.Start(recolector_)

                recolector_.ProcesoEscuchaDirectorio.Priority = ThreadPriority.Normal

                _cantidadHilos = _cantidadHilos + 5

                ' Registro de documentos que no se encuentran en bitácora

                recolector_.ProcesoRegistro = New Thread(New ParameterizedThreadStart(AddressOf RealizaRegistroArchivos))

                recolector_.ProcesoRegistro.Start(recolector_)

                _cantidadHilos = _cantidadHilos + 1

                ' Reconocimiento de los documentos en bitácora

                recolector_.ProcesoReconocimiento = New Thread(New ParameterizedThreadStart(AddressOf RealizaReconocimientoArchivos))

                recolector_.ProcesoReconocimiento.Start(recolector_)

                _cantidadHilos = _cantidadHilos + 1

                ' Recolección de documentos (Maestro de documentos)

                recolector_.OperacionesDocumento = New OperacionesDocumentoHilos64(_ioperacionescatalogo)

                recolector_.ProcesoRecoleccion = New Thread(New ParameterizedThreadStart(AddressOf RealizaRecoleccionArchivos))

                recolector_.ProcesoRecoleccion.Start(recolector_)

                recolector_.ProcesoRecoleccion.Priority = ThreadPriority.Highest

                _cantidadHilos = _cantidadHilos + 1

                _listaRecolecciones.Add(recolector_)

            Next

        Catch ex As Exception

            EscribirEnLog(ex.Message)

            EscribirEnLog("Stack Trace: " & vbCrLf & ex.StackTrace)           

        End Try
       
    End Sub

    Public Sub DetenerRecoleccion()

        For Each recolector_ As RecolectorDocumentos64 In _listaRecolecciones

            recolector_.DetenerProcesos()

        Next

    End Sub

    Private Sub EscuchaDirectorio(ByVal recolector_ As RecolectorDocumentos64)

        Dim fileWatcher_ As New FileWatcher64

        If recolector_.Estatus Then

            recolector_.FileWatcher = fileWatcher_

            recolector_.FileWatcher.ListDirectory(recolector_.RutaRaiz)

            recolector_.FechaActualizaLista = System.DateTime.Now

            recolector_.FileWatcher.WatchDirectory()

            recolector_.EstatusProcesoEscuchaDirectorio = True

        End If

    End Sub

    Private Sub RealizaRegistroArchivos(ByVal recolector_ As RecolectorDocumentos64)

        Dim documentosBitacora_ As DataTable = Nothing

        Dim documentosPendientes_ As IEnumerable(Of String) = New List(Of String)

        Dim documentosRegistradosHistorico_ As IEnumerable(Of String) = New List(Of String)

        Dim documentosRegistrados_ As IEnumerable(Of String) = New List(Of String)

        Dim documentosSinRegistrar_ As IEnumerable(Of String) = New List(Of String)

        Try

            Do While recolector_.Estatus

                If Not recolector_.EstatusProcesoRegistro Then

                    EscribirEnLog(System.DateTime.Now & " - " & "Registro Recolector: " & recolector_.Nombre & " #" & Thread.CurrentThread.ManagedThreadId)

                    SyncLock recolector_.LockRegistro

                        recolector_.EstatusProcesoRegistro = True

                        ' Obtiene la lista de documentos

                        If recolector_.EstatusProcesoEscuchaDirectorio = True Then

                            If (recolector_.Clave = 8 Or recolector_.Clave = 9) Then

                                If ((recolector_.FechaActualizaLista.Date <> System.DateTime.Now.Date) Or (recolector_.FechaActualizaLista.AddHours(1) <= System.DateTime.Now)) Then

                                    recolector_.EstatusProcesoEscuchaDirectorio = False

                                    recolector_.FileWatcher.ListDirectory(recolector_.RutaRaiz)

                                    recolector_.FechaActualizaLista = System.DateTime.Now

                                    recolector_.EstatusProcesoEscuchaDirectorio = True

                                End If

                            Else

                                If (recolector_.FechaActualizaLista.Date <> System.DateTime.Now.Date) Then

                                    recolector_.EstatusProcesoEscuchaDirectorio = False

                                    recolector_.FileWatcher.ListDirectory(recolector_.RutaRaiz)

                                    recolector_.FechaActualizaLista = System.DateTime.Now

                                    recolector_.EstatusProcesoEscuchaDirectorio = True

                                End If

                            End If

                            documentosPendientes_ = recolector_.FileWatcher.ListDocuments

                            EscribirEnLog(System.DateTime.Now & " - " & "Registro Recolector: " & recolector_.Nombre & " Pendientes " & recolector_.FileWatcher.ListDocuments.Count)

                            If documentosPendientes_.Count > 0 Then

                                ' Verifica que exista  el archivo JSON con la bitácora del día anterior
                                If File.Exists("C:\logs\RecolectorDocumentos\Bitacora_" & recolector_.Clave & "_" & recolector_.Nombre & "_" & _
                                               System.DateTime.Now.AddDays(-1).Year & System.DateTime.Now.AddDays(-1).DayOfYear & ".json") Then

                                    Dim tagWatcher_ As TagWatcher

                                    tagWatcher_ = GetListAsJSON(recolector_.Clave, recolector_.Nombre, "Bitacora")

                                    If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                                        documentosRegistradosHistorico_ = DirectCast(tagWatcher_.ObjectReturned, List(Of String))

                                    Else

                                        Dim directory_ As DirectoryInfo = New DirectoryInfo("C:\logs\RecolectorDocumentos")

                                        For Each file_ In directory_.EnumerateFiles("Bitacora_" & recolector_.Clave & "_" & recolector_.Nombre & _
                                                                                    "*", SearchOption.AllDirectories).AsParallel()

                                            file_.Delete()

                                        Next

                                        'Consulta bitacóra hasta el día anterior

                                        documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND CONVERT(DATE,f_FechaRegistro,103) <= '" & _
                                                                                             System.DateTime.Now.AddDays(-1).Date & "'")

                                        If Not documentosBitacora_ Is Nothing Then

                                            documentosRegistradosHistorico_ = documentosBitacora_.AsEnumerable() _
                                                               .Select(Function(r) r.Field(Of String)("t_RutaOriginal")) _
                                                               .Distinct() _
                                                               .ToList()

                                            If (Not documentosRegistradosHistorico_ Is Nothing) And (documentosRegistradosHistorico_.Count > 0) Then

                                                ConvertListToJSON(recolector_.Clave,
                                                                  recolector_.Nombre,
                                                                  "Bitacora",
                                                                  documentosRegistradosHistorico_)

                                            End If

                                        End If

                                    End If

                                Else

                                    Dim directory_ As DirectoryInfo = New DirectoryInfo("C:\logs\RecolectorDocumentos")

                                    'Borra la bitácora anterior
                                    For Each file_ In directory_.EnumerateFiles("Bitacora_" & recolector_.Clave & "_" & recolector_.Nombre & _
                                                                                "*", SearchOption.AllDirectories).AsParallel()

                                        file_.Delete()

                                    Next

                                    'Consulta bitacóra hasta el día anterior

                                    documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND CONVERT(DATE,f_FechaRegistro,103) <= '" & _
                                                                                         System.DateTime.Now.AddDays(-1).Date & "'")

                                    If Not documentosBitacora_ Is Nothing Then

                                        documentosRegistradosHistorico_ = documentosBitacora_.AsEnumerable() _
                                                           .Select(Function(r) r.Field(Of String)("t_RutaOriginal")) _
                                                           .Distinct() _
                                                           .ToList()

                                        If (Not documentosRegistradosHistorico_ Is Nothing) And (documentosRegistradosHistorico_.Count > 0) Then

                                            ConvertListToJSON(recolector_.Clave,
                                                              recolector_.Nombre,
                                                              "Bitacora",
                                                              documentosRegistradosHistorico_)

                                        End If

                                    End If

                                End If

                                documentosRegistrados_ = documentosRegistradosHistorico_

                                'Consulta bitacóra del día actual

                                documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND CONVERT(DATE,f_FechaRegistro,103) >= '" & _
                                                                                     System.DateTime.Now.Date & "'")

                                If Not documentosBitacora_ Is Nothing Then

                                    documentosRegistrados_ = documentosRegistrados_.Union(documentosBitacora_.AsEnumerable() _
                                                       .Select(Function(r) r.Field(Of String)("t_RutaOriginal")) _
                                                       .Distinct() _
                                                       .ToList())


                                End If

                                ' Hace una comparación entre los documentos registrados y los pendientes de registrar

                                If (Not documentosPendientes_ Is Nothing And Not documentosRegistrados_ Is Nothing) Then

                                    documentosSinRegistrar_ = documentosPendientes_.Except(documentosRegistrados_)

                                End If

                                If Not documentosSinRegistrar_ Is Nothing Then

                                    If documentosSinRegistrar_.Count > 0 Then

                                        ' Calcula cantidad de hilos subprocesos a generar

                                        'Dim promedio_ As Integer = Math.Round(documentosSinRegistrar_.Count() / CantidadMaxDocumentosPorHilo)

                                        'Dim numHilos_ As Integer = 0

                                        'If _cantidadHilos < CantidadMaxHilos Then

                                        '    If _cantidadHilos + promedio_ < CantidadMaxHilos Then

                                        '        numHilos_ = promedio_

                                        '        _cantidadHilos = _cantidadHilos + numHilos_

                                        '    End If

                                        'End If

                                        'recolector_.SubProcesosRegistro = New List(Of Thread)

                                        'If numHilos_ > 0 Then

                                        '    For i As Int32 = 0 To numHilos_ - 1

                                        '        Dim indexI_ As Integer = 0

                                        '        Dim indexF_ As Integer = (i + 1) * CantidadMaxDocumentosPorHilo

                                        '        If i > 0 Then

                                        '            indexI_ = (i * CantidadMaxDocumentosPorHilo) + 1

                                        '        End If

                                        '        If i = numHilos_ - 1 Then

                                        '            indexF_ = documentosSinRegistrar_.Count() - 1

                                        '        End If

                                        'recolector_.SubProcesosRegistro.Add(New Thread(Sub() Me.InsertarRegistrosBitacora(recolector_.Clave, documentosSinRegistrar_.ToList(), indexI_, indexF_)))

                                        'recolector_.SubProcesosRegistro(i).Start()

                                        '_cantidadHilos = _cantidadHilos + 1

                                        'Next

                                        'Else

                                        InsertarRegistrosBitacora(recolector_.Clave, documentosSinRegistrar_.ToList(), 0, documentosSinRegistrar_.Count - 1)

                                        'End If

                                    End If

                                End If

                            End If

                        End If

                        recolector_.EstatusProcesoRegistro = False

                    End SyncLock

                End If

                recolector_.ProcesoRegistro.Join(60000)

            Loop

        Catch ex As Exception

            EscribirEnLog(ex.Message)

            EscribirEnLog("Stack Trace: " & vbCrLf & ex.StackTrace)

        End Try

    End Sub

    Private Sub RealizaReconocimientoArchivos(ByVal recolector_ As RecolectorDocumentos64)

        Dim documentosBitacora_ As DataTable = Nothing

        Dim tiposDocumentos_ As DataTable = Nothing

        Dim l_TiposDocumentos_ As New Dictionary(Of String, String())

        Dim t_TiposDocumentos_ As String = Nothing

        Try

            Do While recolector_.Estatus

                If Not recolector_.EstatusProcesoReconocimiento Then

                    EscribirEnLog(System.DateTime.Now & " - " & "Reconocimiento Recolector: " & recolector_.Nombre & " #" & Thread.CurrentThread.ManagedThreadId)

                    SyncLock recolector_.LockReconocimiento

                        recolector_.EstatusProcesoReconocimiento = True

                        ' Obtiene la lista de los tipos de documentos configurados 

                        tiposDocumentos_ = ConsultarTiposDocumentosPorRecolector(recolector_.Clave, " AND i_Cve_Estatus = 1")

                        ' Obtiene la lista de documentos pendientes de reconocer

                        documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND i_Cve_Estatus IN (0,3) AND i_Cve_TipoDocumento IS NULL")

                        If Not documentosBitacora_ Is Nothing Then

                            For Each documento_ As DataRow In documentosBitacora_.Rows

                                l_TiposDocumentos_.Clear()

                                t_TiposDocumentos_ = Nothing

                                If Not tiposDocumentos_ Is Nothing Then

                                    For Each tipoDocumento_ As DataRow In tiposDocumentos_.Rows

                                        If ClasificarDocumento(documento_.Item("t_RutaOriginal"),
                                                              tipoDocumento_.Item("t_Extension"),
                                                              Convert.ToBoolean(tipoDocumento_.Item("i_IgnorarExtension")),
                                                              tipoDocumento_.Item("t_PatronIdentificable"),
                                                              Convert.ToBoolean(tipoDocumento_.Item("i_PatronSensible"))) Then

                                            l_TiposDocumentos_.Add(tipoDocumento_.Item("i_Cve_TiposDocumentosRecolector"),
                                                                   New String() {tipoDocumento_.Item("i_Cve_TipoDocumento"),
                                                                                 tipoDocumento_.Item("i_Cve_EncPlantillaDocumento"),
                                                                                 tipoDocumento_.Item("i_ConsultableCliente")})

                                            If t_TiposDocumentos_ Is Nothing Then

                                                t_TiposDocumentos_ = tipoDocumento_.Item("t_Nombre")

                                            Else

                                                t_TiposDocumentos_ = t_TiposDocumentos_ & ", " & tipoDocumento_.Item("t_Nombre")

                                            End If

                                            If l_TiposDocumentos_.Count > 1 Then

                                                Exit For

                                            End If

                                        End If

                                    Next

                                    ' Actualiza Bitácora e inserta errores

                                    If l_TiposDocumentos_.Count > 0 Then

                                        If l_TiposDocumentos_.Count > 1 Then

                                            InsertarErroresRecolectorDocumentos(documento_.Item("i_Cve_RecolectorDocumentos"), _
                                                                                documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                                TagWatcher.ErrorTypes.C6_030_0000.ToString(), _
                                                                                _tagWatcher.GetEnumDescription(TagWatcher.ErrorTypes.C6_030_0000) & " (" & t_TiposDocumentos_ & ")")

                                            ActualizarBitacoraRecolectorDocumentos(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                   TagWatcher.ErrorTypes.C6_030_0000.ToString(),
                                                                                    _tagWatcher.GetEnumDescription(TagWatcher.ErrorTypes.C6_030_0000),
                                                                                    Nothing,
                                                                                    "3")


                                        Else

                                            ActualizarBitacoraRecolectorDocumentos(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                                  l_TiposDocumentos_.Keys(0), _
                                                                                  l_TiposDocumentos_.Values(0)(0), _
                                                                                  l_TiposDocumentos_.Values(0)(1), _
                                                                                  Nothing, _
                                                                                  Nothing, _
                                                                                  l_TiposDocumentos_.Values(0)(2), _
                                                                                  "1")

                                        End If

                                    End If

                                End If

                            Next

                        End If

                        recolector_.EstatusProcesoReconocimiento = False

                    End SyncLock

                End If

                recolector_.ProcesoReconocimiento.Join(60000)

            Loop

        Catch ex As Exception

            EscribirEnLog(ex.Message)

            EscribirEnLog("Stack Trace: " & vbCrLf & ex.StackTrace)

        End Try

    End Sub

    Private Sub RealizaRecoleccionArchivos(ByVal recolector_ As RecolectorDocumentos64)

        Dim documentosBitacora_ As DataTable = Nothing

        Dim listaCaracteristicasDocumento_ As New Dictionary(Of Integer, CaracteristicaDocumento)

        Try

            Do While recolector_.Estatus

                If Not recolector_.EstatusProcesoRecoleccion Then

                    EscribirEnLog(System.DateTime.Now & " - " & "Recolección Recolector: " & recolector_.Nombre & " #" & Thread.CurrentThread.ManagedThreadId)

                    SyncLock recolector_.LockRecoleccion

                        recolector_.EstatusProcesoRecoleccion = True

                        ' Obtiene la lista de documentos pendientes de recolectar

                        'documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND i_Cve_Estatus IN (1) AND i_Cve_Documento IS NULL")

                        documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND i_Cve_Estatus IN (1,3) AND i_Cve_TipoDocumento IS NOT NULL AND i_Cve_Documento IS NULL")

                        'documentosBitacora_ = ConsultarBitacoraPorRecolector(recolector_.Clave, " AND i_Cve_Estatus IN (8) AND i_Cve_TipoDocumento IS NOT NULL AND i_Cve_Documento IS NULL")

                        If Not documentosBitacora_ Is Nothing Then

                            For Each documento_ As DataRow In documentosBitacora_.Rows

                                Dim tagWatcher_ As New TagWatcher

                                ' Cargar Caracteristicas
                                listaCaracteristicasDocumento_ = CargarCaracteristicas(recolector_.Clave,
                                                                                       recolector_.RutaRaiz,
                                                                                       recolector_.SeparadorSubdirectorios,
                                                                                       documento_.Item("t_RutaOriginal"),
                                                                                       documento_.Item("i_Cve_TiposDocumentosRecolector"))


                                ' Insertar en Maestro de documentos
                                tagWatcher_ = InsertarMaestroDocumentos(recolector_.OperacionesDocumento,
                                                               recolector_.DivisionMiEmpresa,
                                                               documento_.Item("i_Cve_EncPlantillaDocumento"),
                                                               documento_.Item("i_Cve_TipoDocumento"),
                                                               documento_.Item("t_RutaOriginal"),
                                                               listaCaracteristicasDocumento_,
                                                               documento_.Item("i_ConsultableCliente"))

                                If tagWatcher_.Status = TagWatcher.TypeStatus.Ok Then

                                    Dim infoDocumento_ As Dictionary(Of String, String) = DirectCast(tagWatcher_.ObjectReturned, Dictionary(Of String, String))

                                    ' Verificar existencia
                                    If File.Exists(infoDocumento_.Item("t_RutaRecoleccion")) Then

                                        '' Actualizar BD
                                        AplicarRecoleccionEnBitacora(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"),
                                                                    infoDocumento_.Item("i_Cve_Documento"),
                                                                    infoDocumento_.Item("t_RutaRecoleccion"),
                                                                    infoDocumento_.Item("f_FechaRecoleccion"))

                                        ' Renombrar archivo                                  

                                        'ruta_ = Path.GetDirectoryName(documento_.Item("t_RutaOriginal")) & "\"

                                        'nombre_ = Path.GetFileNameWithoutExtension(documento_.Item("t_RutaOriginal")) & "_RECMD"

                                        'extension_ = Path.GetExtension(documento_.Item("t_RutaOriginal"))

                                        'FileSystem.Rename(documento_.Item("t_RutaOriginal"), ruta_ & nombre_ & extension_)

                                    End If

                                Else

                                    Dim t_NombreCaracteristica_ As String = Nothing

                                    If tagWatcher_.Errors = TagWatcher.ErrorTypes.C6_012_1017 Then

                                        If Not tagWatcher_.ObjectReturned Is Nothing Then

                                            t_NombreCaracteristica_ = " (" & DirectCast(tagWatcher_.ObjectReturned, String) & ")"

                                        End If

                                    End If

                                    InsertarErroresRecolectorDocumentos(documento_.Item("i_Cve_RecolectorDocumentos"), _
                                                                        documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                        tagWatcher_.Errors.ToString(), _
                                                                        tagWatcher_.ErrorDescription & t_NombreCaracteristica_)


                                    Dim f_fechaRegistro As DateTime = documento_.Item("f_FechaRegistro")

                                    Select Case tagWatcher_.Errors

                                        Case TagWatcher.ErrorTypes.C6_012_1030       ' Ya se encuentra registrado

                                            ActualizarBitacoraRecolectorDocumentos(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                       documento_.Item("i_Cve_TiposDocumentosRecolector"), _
                                                                       documento_.Item("i_Cve_TipoDocumento"), _
                                                                       documento_.Item("i_Cve_EncPlantillaDocumento"), _
                                                                       tagWatcher_.Errors.ToString(), _
                                                                       tagWatcher_.ErrorDescription, _
                                                                       documento_.Item("i_ConsultableCliente"), _
                                                                       "4")

                                        Case TagWatcher.ErrorTypes.C6_012_1017     ' No fue capturada una característica

                                            If f_fechaRegistro.Date.AddDays(3) <= DateTime.Now.Date Then         ' Lo cancela después de 3 días

                                                ActualizarBitacoraRecolectorDocumentos(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                             documento_.Item("i_Cve_TiposDocumentosRecolector"), _
                                                                             documento_.Item("i_Cve_TipoDocumento"), _
                                                                             documento_.Item("i_Cve_EncPlantillaDocumento"), _
                                                                             tagWatcher_.Errors.ToString(), _
                                                                             tagWatcher_.ErrorDescription, _
                                                                             documento_.Item("i_ConsultableCliente"), _
                                                                             "4")
                                            Else

                                                ActualizarBitacoraRecolectorDocumentos(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                                 documento_.Item("i_Cve_TiposDocumentosRecolector"), _
                                                                                 documento_.Item("i_Cve_TipoDocumento"), _
                                                                                 documento_.Item("i_Cve_EncPlantillaDocumento"), _
                                                                                 tagWatcher_.Errors.ToString(), _
                                                                                 tagWatcher_.ErrorDescription, _
                                                                                 documento_.Item("i_ConsultableCliente"), _
                                                                                 "3")

                                            End If

                                        Case Else

                                            ActualizarBitacoraRecolectorDocumentos(documento_.Item("i_Cve_BitacoraRecolectorDocumentos"), _
                                                                        documento_.Item("i_Cve_TiposDocumentosRecolector"), _
                                                                        documento_.Item("i_Cve_TipoDocumento"), _
                                                                        documento_.Item("i_Cve_EncPlantillaDocumento"), _
                                                                        tagWatcher_.Errors.ToString(), _
                                                                        tagWatcher_.ErrorDescription, _
                                                                        documento_.Item("i_ConsultableCliente"), _
                                                                        "3")

                                    End Select

                                End If

                            Next

                        End If

                        recolector_.EstatusProcesoRecoleccion = False

                    End SyncLock

                End If

                recolector_.ProcesoRecoleccion.Join(60000)

            Loop

        Catch ex As Exception

            EscribirEnLog(ex.Message)

            EscribirEnLog("Stack Trace: " & vbCrLf & ex.StackTrace)

        End Try

    End Sub

    Private Function SepararCadena(ByVal t_Cadena_ As String, ByVal t_CadenaSustraer_ As String, ByVal t_Separador_ As String) As List(Of String)

        Dim arrayCadenas_ As New List(Of String)

        Dim cadenasAuxiliar_ As String() = Nothing

        If Not t_CadenaSustraer_ Is Nothing Then

            t_Cadena_ = t_Cadena_.Remove(0, t_CadenaSustraer_.Length + 1)

        End If

        t_Cadena_ = t_Cadena_.Replace("  ", " ")

        cadenasAuxiliar_ = t_Cadena_.Split("\")

        For Each cadena_ As String In cadenasAuxiliar_

            If t_Separador_.Equals("") Then

                arrayCadenas_.AddRange(cadena_.Split(" "))

            Else

                arrayCadenas_.AddRange(cadena_.Split(t_Separador_))

            End If

        Next

        Return arrayCadenas_

    End Function

    Private Function ObtenerValorCadena(ByVal listaCadenas_ As List(Of String), ByVal i_Orden As Int32, ByVal i_InvertirOrden As Int32, ByVal i_PosicionInicial_ As Int32, ByVal i_NumeroCaracteres_ As Int32) As String

        Dim t_Valor_ As String = Nothing

        Dim i_Orden_ As Int32 = Nothing

        If i_InvertirOrden = 1 Then

            i_Orden_ = listaCadenas_.Count - (i_Orden - 1)

        Else

            i_Orden_ = i_Orden

        End If

        If (i_Orden_ <= listaCadenas_.Count) Then

            If (i_PosicionInicial_ > 0 And i_NumeroCaracteres_ > 0) Then

                If ((i_PosicionInicial_ <= listaCadenas_(i_Orden_ - 1).Length) And _
                (((i_PosicionInicial_ - 1) + i_NumeroCaracteres_) <= listaCadenas_(i_Orden_ - 1).Length)) Then

                    t_Valor_ = listaCadenas_(i_Orden_ - 1).Substring(i_PosicionInicial_ - 1, i_NumeroCaracteres_)

                End If

            Else

                If i_PosicionInicial_ > 0 Then

                    If (i_PosicionInicial_ <= listaCadenas_(i_Orden_ - 1).Length) Then

                        t_Valor_ = listaCadenas_(i_Orden_ - 1).Substring(i_PosicionInicial_ - 1)

                    End If

                Else

                    t_Valor_ = listaCadenas_(i_Orden_ - 1)

                End If

            End If

        End If

        Return t_Valor_

    End Function

    Private Function CargarCaracteristicas(ByVal i_Cve_RecolectorDocumentos_ As Integer,
                                           ByVal t_RutaRaiz_ As String,
                                           ByVal t_SeparadorSubdirectorios_ As String,
                                           ByVal t_RutaOriginal_ As String,
                                           ByVal i_Cve_TiposDocumentosRecolector_ As String) As Dictionary(Of Integer, CaracteristicaDocumento)

        Dim caracteristicasRecolector_ As DataTable = Nothing

        Dim caracteristicasTiposDocumentos_ As DataTable = Nothing

        Dim tiposDocumentosRecolector_ As DataTable = Nothing

        Dim listaCadenasSubdirectorios_ As New List(Of String)

        Dim listaCadenasNombreArchivo_ As New List(Of String)

        Dim listaCaracteristicasDocumento_ As New Dictionary(Of Integer, CaracteristicaDocumento)

        ' Recorre características de recolector y asigna valores

        listaCadenasSubdirectorios_ = SepararCadena(Path.GetDirectoryName(t_RutaOriginal_), t_RutaRaiz_, t_SeparadorSubdirectorios_)

        caracteristicasRecolector_ = ConsultarCaracteristicasDocumentos("WHERE i_TipoCaracteristica = 1 AND i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_ & " AND i_Cve_Estatus = 1")

        For Each caracteristicaRecolector_ As DataRow In caracteristicasRecolector_.Rows

            Dim caracteristica_ As New CaracteristicaDocumento

            caracteristica_.ClaveCaracteristica = caracteristicaRecolector_.Item("i_Cve_CaracteristicaDocumento")

            caracteristica_.NombreCaracteristica = caracteristicaRecolector_.Item("t_Nombre")

            If Not caracteristicaRecolector_.Item("t_ValorFijo").Equals("") Then

                caracteristica_.Valor = caracteristicaRecolector_.Item("t_ValorFijo")

            Else

                caracteristica_.Valor = ObtenerValorCadena(listaCadenasSubdirectorios_,
                                                      caracteristicaRecolector_.Item("i_Orden"),
                                                      caracteristicaRecolector_.Item("i_InvertirOrden"),
                                                      caracteristicaRecolector_.Item("i_PosicionInicial"),
                                                      caracteristicaRecolector_.Item("i_NumCaracteres"))

            End If

            listaCaracteristicasDocumento_.Add(caracteristicaRecolector_.Item("i_Cve_CaracteristicaDocumento"), caracteristica_)

        Next

        ' Recorre características del tipo de documento y asigna valores

        tiposDocumentosRecolector_ = ConsultarTiposDocumentosPorRecolector(i_Cve_RecolectorDocumentos_, " AND i_Cve_TiposDocumentosRecolector = " & i_Cve_TiposDocumentosRecolector_)

        If Not tiposDocumentosRecolector_ Is Nothing Then

            Select Case tiposDocumentosRecolector_.Rows(0)("i_Cve_TipoDocumento")

                Case 110      ' Archivo de validación M3
                    listaCadenasNombreArchivo_ = SepararCadena(Path.GetFileName(t_RutaOriginal_), Nothing, tiposDocumentosRecolector_.Rows(0)("t_SeparadorArchivo"))

                Case 116      ' Archivo de respuesta de validación
                    listaCadenasNombreArchivo_ = SepararCadena(Path.GetFileName(t_RutaOriginal_), Nothing, tiposDocumentosRecolector_.Rows(0)("t_SeparadorArchivo"))

                Case 117      ' Archivo de pago electrónico
                    listaCadenasNombreArchivo_ = SepararCadena(Path.GetFileName(t_RutaOriginal_), Nothing, tiposDocumentosRecolector_.Rows(0)("t_SeparadorArchivo"))

                Case 118      ' Archivo de acuse de pago electrónico
                    listaCadenasNombreArchivo_ = SepararCadena(Path.GetFileName(t_RutaOriginal_), Nothing, tiposDocumentosRecolector_.Rows(0)("t_SeparadorArchivo"))

                Case Else
                    listaCadenasNombreArchivo_ = SepararCadena(Path.GetFileNameWithoutExtension(t_RutaOriginal_), Nothing, tiposDocumentosRecolector_.Rows(0)("t_SeparadorArchivo"))

            End Select

        End If

        caracteristicasTiposDocumentos_ = ConsultarCaracteristicasDocumentos("WHERE i_TipoCaracteristica = 2 AND i_Cve_TiposDocumentosRecolector = " & i_Cve_TiposDocumentosRecolector_ & " AND i_Cve_Estatus = 1")

        For Each caracteristicaTipoDocumento_ As DataRow In caracteristicasTiposDocumentos_.Rows

            Dim caracteristica_ As New CaracteristicaDocumento

            caracteristica_.ClaveCaracteristica = caracteristicaTipoDocumento_.Item("i_Cve_CaracteristicaDocumento")

            caracteristica_.NombreCaracteristica = caracteristicaTipoDocumento_.Item("t_Nombre")

            If Not caracteristicaTipoDocumento_.Item("t_ValorFijo").Equals("") Then

                caracteristica_.Valor = caracteristicaTipoDocumento_.Item("t_ValorFijo")

            Else

                caracteristica_.Valor = ObtenerValorCadena(listaCadenasNombreArchivo_,
                                                      caracteristicaTipoDocumento_.Item("i_Orden"),
                                                      caracteristicaTipoDocumento_.Item("i_InvertirOrden"),
                                                      caracteristicaTipoDocumento_.Item("i_PosicionInicial"),
                                                      caracteristicaTipoDocumento_.Item("i_NumCaracteres"))

            End If

            listaCaracteristicasDocumento_.Add(caracteristicaTipoDocumento_.Item("i_Cve_CaracteristicaDocumento"), caracteristica_)

        Next

        Return listaCaracteristicasDocumento_

    End Function

    Private Function ConsultarRecolectoresDocumentos(ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                 "SELECT * FROM [Solium].[dbo].[Vt030RecolectorDocumentos] " & t_ClausulasLibres_

        Try

            _sistema.ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    Private Function ConsultarTiposDocumentosPorRecolector(ByVal i_Cve_RecolectorDocumentos_ As Integer,
                                                          ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                    "SELECT * FROM [Solium].[dbo].[Vt030TiposDocumentosRecolector] " & _
                    "WHERE i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_ & t_ClausulasLibres_

        If VerificarWhere(t_Query_) Then

            Try

                _sistema.ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

                If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                    dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

                End If

            Catch ex As System.Exception

                EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

            End Try

        End If

        Return dataTable_

    End Function

    Private Function ConsultarCaracteristicasDocumentos(ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                    "SELECT * FROM [Solium].[dbo].[Vt030CaracteristicasDocumentosRecolector] " & t_ClausulasLibres_

        Try

            _sistema.ConexionSingleton.SQLServerSingletonConexion.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    Private Function ConsultarBitacoraPorRecolector(ByVal i_Cve_RecolectorDocumentos_ As Integer,
                                                    ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                    "SELECT * FROM [Solium].[dbo].[Vt030BitacoraRecolectorDocumentos] " & _
                    "WHERE i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_ & t_ClausulasLibres_

        If VerificarWhere(t_Query_) Then

            Try

                tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

                If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                    dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

                End If

            Catch ex As System.Exception

                EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

            End Try

        End If

        Return dataTable_

    End Function

    Public Function ConsultarResumenGeneral(ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                   "SELECT  i_Cve_RecolectorDocumentos, " & _
                            "t_NombreRecolector, " & _
                            "COUNT(DISTINCT i_Cve_BitacoraRecolectorDocumentos) AS i_Registrados, " & _
                            "SUM(CASE WHEN i_Cve_TipoDocumento IS NOT NULL THEN 1 ELSE 0 END) AS i_Reconocidos, " & _
                            "SUM(CASE WHEN i_Cve_Documento IS NOT NULL THEN 1 ELSE 0 END) AS i_Recolectados, " & _
                            "SUM(CASE WHEN i_Cve_Estatus = 3 THEN 1 ELSE 0 END) AS i_Errores, " & _
                            "SUM(CASE WHEN i_Cve_Estatus = 4 THEN 1 ELSE 0 END) AS i_Cancelados, " & _
                            "MAX(f_FechaRecoleccion) AS f_FechaRecoleccion " & _
                   "FROM Vt030BitacoraRecolectorDocumentos " & t_ClausulasLibres_ & _
                   " GROUP BY i_Cve_RecolectorDocumentos, " & _
                            "t_NombreRecolector "

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    Public Function ConsultarResumenTiposDocumentos(ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                   "SELECT  tip.i_Cve_TiposDocumentosRecolector, " & _
                            "doc.t_Nombre, " & _
                            "COUNT(reg.i_Cve_BitacoraRecolectorDocumentos) AS i_Reconocidos, " & _
                            "SUM(CASE WHEN reg.i_Cve_Documento IS NOT NULL THEN 1 ELSE 0 END) AS i_Recolectados " & _
                   "FROM Vin030TiposDocumentosRecolector AS tip " & _
                   "INNER JOIN Cat012TiposDocumento AS doc ON doc.i_Cve_TipoDocumento = tip.i_Cve_TipoDocumento " & _
                   "LEFT OUTER JOIN Reg030BitacoraRecolectorDocumentos AS reg ON reg.i_Cve_TiposDocumentosRecolector = tip.i_Cve_TiposDocumentosRecolector " & _
                   "WHERE tip.i_Cve_Estado = 1 AND reg.i_Cve_Estado = 1 " & t_ClausulasLibres_ & _
                   " GROUP BY tip.i_Cve_TiposDocumentosRecolector, " & _
                             "doc.t_Nombre " & _
                   "ORDER BY tip.i_Cve_TiposDocumentosRecolector ASC "

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    Public Function ConsultarResumenErrores(ByVal t_ClausulasLibres_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                   "SELECT err.t_CodigoError, " & _
                           "(SELECT COUNT(i_Cve_ErroresRecolector) FROM Reg030ErroresRecolector AS err WHERE err.i_Cve_Estado = 1 " & t_ClausulasLibres_ & " ) AS i_TotalErrores, " & _
                           "COUNT (err.i_Cve_ErroresRecolector) AS i_ErroresCodigo " & _
                    "FROM Reg030ErroresRecolector AS err " & _
                    "WHERE err.i_Cve_Estado = 1 " & t_ClausulasLibres_ & _
                    " GROUP BY err.t_CodigoError " & _
                    "ORDER BY err.t_CodigoError ASC "

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    'Private Function ConsultarDatosReferencia(ByVal t_ClausulasLibres_ As String) As DataTable

    '    Dim dataTable_ As DataTable = Nothing

    '    Dim tagWatcher_ As New TagWatcher

    '    Dim t_Query_ As String = Nothing

    '    t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
    '                "SELECT * FROM [Solium].[dbo].[VM016Operaciones] " & _
    '                t_ClausulasLibres_

    '    Try

    '        tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

    '        If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

    '            dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

    '        End If

    '    Catch ex As System.Exception

    '        EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

    '    End Try

    '    Return dataTable_

    'End Function

    Private Function ConsultarDatosReferencia(ByVal t_ClausulasLibres_ As String, ByVal t_ClausulasLibresContains_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        If ConsultarEstatusActualizacion("DatosReferencia") > 0 Then

            t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                        "SELECT * FROM [Solium].[dbo].[VM016Operaciones] " & _
                        t_ClausulasLibres_

        Else

            't_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
            '            "SELECT * FROM [SISTEMAS].[dbo].[Reg016DatosReferencia] WITH(NOLOCK) " & _
            '            t_ClausulasLibresContains_

            t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                        "SELECT * FROM [SISTEMAS].[dbo].[Reg016DatosReferencia] WITH(NOLOCK) " & _
                        t_ClausulasLibres_

        End If

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    Private Function ConsultarEstatusActualizacion(ByVal t_NombreToken_ As String) As Int32

        Dim dataTable_ As DataTable = Nothing

        Dim estatusActualizacion_ As Int32 = 0

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                   "SELECT COUNT(*) AS Pendientes FROM [SISTEMAS].[dbo].[Bit015ActualizacionTablas] WITH(NOLOCK) " & _
                   "WHERE FORMAT(f_FechaActualizacion,'dd/MM/yyyy HH') = FORMAT(GETDATE(),'dd/MM/yyyy HH') " & _
                   "AND CONTAINS (t_NombreToken, '" & t_NombreToken_ & "') " & _
                   "AND i_Cve_Estado = 1 AND i_Cve_Estatus = 0 "

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

                estatusActualizacion_ = dataTable_.Rows(0).Item("Pendientes")

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return estatusActualizacion_

    End Function

    Private Function ConsultarDatosDigitalizacionVUCEM(ByVal t_ClausulasLibres_ As String, ByVal t_ClausulasLibresContains_ As String) As DataTable

        Dim dataTable_ As DataTable = Nothing

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        If ConsultarEstatusActualizacion("DatosDigitalizacionVUCEM") > 0 Then

            t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                       "SELECT * FROM [Solium].[dbo].[Vt016DigitalizacionVUCEM] " & t_ClausulasLibres_

        Else

            't_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
            '            "SELECT * FROM [SISTEMAS].[dbo].[Reg016DatosDigitalizacionVUCEM] WITH(NOLOCK) " & _
            '            t_ClausulasLibresContains_

            t_Query_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                        "SELECT * FROM [SISTEMAS].[dbo].[Reg016DatosDigitalizacionVUCEM] WITH(NOLOCK) " & _
                        t_ClausulasLibres_

        End If

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(tagWatcher_.ObjectReturned, DataTable)

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return dataTable_

    End Function

    Private Function InsertarBitacoraRecolectorDocumentos(ByVal i_Cve_RecolectorDocumentos_ As String,
                                                       ByVal i_Cve_TiposDocumentosRecolector_ As String,
                                                       ByVal i_Cve_TipoDocumento_ As String,
                                                       ByVal i_Cve_EncPlantillaDocumento_ As String,
                                                       ByVal t_RutaOriginal_ As String,
                                                       ByVal d_TamanoArchivoKB_ As String) As Boolean

        Dim b_Resultado_ As Boolean = False

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "INSERT INTO [Solium].[dbo].[Reg030BitacoraRecolectorDocumentos] " & _
                "([i_Cve_RecolectorDocumentos], " & _
                "[i_Cve_TiposDocumentosRecolector], " & _
                "[i_Cve_TipoDocumento], " & _
                "[i_Cve_EncPlantillaDocumento], " & _
                "[i_Cve_Documento], " & _
                "[t_RutaOriginal], " & _
                "[t_RutaRecoleccion], " & _
                "[f_FechaRegistro], " & _
                "[f_FechaRecoleccion], " & _
                "[d_TamanoArchivoKB], " & _
                "[t_CodigoError], " & _
                "[t_DescripcionError], " & _
                "[i_ConsultableCliente], " & _
                "[i_Cve_Estatus], " & _
                "[i_Cve_Estado]) " & _
                "VALUES" & _
                "(" & _
                "" & i_Cve_RecolectorDocumentos_ & "," & _
                "" & (IIf(i_Cve_TiposDocumentosRecolector_ Is Nothing, "NULL", i_Cve_TiposDocumentosRecolector_)) & ", " & _
                "" & (IIf(i_Cve_TipoDocumento_ Is Nothing, "NULL", i_Cve_TipoDocumento_)) & ", " & _
                "" & (IIf(i_Cve_EncPlantillaDocumento_ Is Nothing, "NULL", i_Cve_EncPlantillaDocumento_)) & ", " & _
                "NULL," & _
                "" & (IIf(t_RutaOriginal_ Is Nothing, "NULL", "'" & t_RutaOriginal_ & "'")) & ", " & _
                "NULL," & _
                "SYSDATETIME()," & _
                "NULL," & _
                "" & (IIf(d_TamanoArchivoKB_ Is Nothing, "NULL", d_TamanoArchivoKB_)) & ", " & _
                "NULL," & _
                "NULL," & _
                "NULL," & _
                "0," & _
                "1)"

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                b_Resultado_ = True

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return b_Resultado_

    End Function

    Private Function ActualizarBitacoraRecolectorDocumentos(ByVal i_Cve_BitacoraRecolectorDocumentos_ As Int32,
                                                           ByVal i_Cve_TiposDocumentosRecolector_ As String,
                                                           ByVal i_Cve_TipoDocumento_ As String,
                                                           ByVal i_Cve_EncPlantillaDocumento_ As String,
                                                           ByVal t_CodigoError_ As String,
                                                           ByVal t_DescripcionError_ As String,
                                                           ByVal i_ConsultableCliente_ As String,
                                                           ByVal i_Cve_Estatus_ As Int32) As Boolean

        Dim b_Resultado_ As Boolean = False

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "UPDATE [Solium].[dbo].[Reg030BitacoraRecolectorDocumentos] SET " & _
                "[i_Cve_TiposDocumentosRecolector] = " & (IIf(i_Cve_TiposDocumentosRecolector_ Is Nothing, "NULL", i_Cve_TiposDocumentosRecolector_)) & ", " & _
                "[i_Cve_TipoDocumento] = " & (IIf(i_Cve_TipoDocumento_ Is Nothing, "NULL", i_Cve_TipoDocumento_)) & ", " & _
                "[i_Cve_EncPlantillaDocumento] =  " & (IIf(i_Cve_EncPlantillaDocumento_ Is Nothing, "NULL", i_Cve_EncPlantillaDocumento_)) & ", " & _
                "[t_CodigoError] = " & (IIf(t_CodigoError_ Is Nothing, "NULL", "'" & t_CodigoError_ & "'")) & ", " & _
                "[t_DescripcionError] = " & (IIf(t_DescripcionError_ Is Nothing, "NULL", "'" & t_DescripcionError_ & "'")) & ", " & _
                "[i_ConsultableCliente] = " & (IIf(i_ConsultableCliente_ Is Nothing, "NULL", i_ConsultableCliente_)) & ", " & _
                "[i_Cve_Estatus] = " & i_Cve_Estatus_ & " " & _
                "WHERE [i_Cve_BitacoraRecolectorDocumentos] = " & i_Cve_BitacoraRecolectorDocumentos_

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                b_Resultado_ = True

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return b_Resultado_

    End Function

    Private Function AplicarRecoleccionEnBitacora(ByVal i_Cve_BitacoraRecolectorDocumentos_ As Int32,
                                                  ByVal i_Cve_Documento_ As String,
                                                  ByVal t_RutaRecoleccion_ As String,
                                                  ByVal f_FechaRecoleccion_ As String) As Boolean

        Dim b_Resultado_ As Boolean = False

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "UPDATE [Solium].[dbo].[Reg030BitacoraRecolectorDocumentos] SET " & _
                "[i_Cve_Documento] = " & (IIf(i_Cve_Documento_ Is Nothing, "NULL", i_Cve_Documento_)) & ", " & _
                "[t_RutaRecoleccion] = " & (IIf(t_RutaRecoleccion_ Is Nothing, "NULL", "'" & t_RutaRecoleccion_ & "'")) & ", " & _
                "[f_FechaRecoleccion] = " & (IIf(f_FechaRecoleccion_ Is Nothing, "NULL", "'" & f_FechaRecoleccion_ & "'")) & ", " & _
                "[t_CodigoError] = NULL, " & _
                "[t_DescripcionError] = NULL, " & _
                "[i_Cve_Estatus] = 2 " & _
                "WHERE [i_Cve_BitacoraRecolectorDocumentos] = " & i_Cve_BitacoraRecolectorDocumentos_

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                b_Resultado_ = True

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return b_Resultado_

    End Function

    Private Function InsertarErroresRecolectorDocumentos(ByVal i_Cve_RecolectorDocumentos_ As String,
                                                        ByVal i_Cve_BitacoraRecolectorDocumentos_ As String,
                                                        ByVal t_CodigoError_ As String,
                                                        ByVal t_DescripcionError_ As String) As Boolean

        Dim b_Resultado_ As Boolean = False

        Dim tagWatcher_ As New TagWatcher

        Dim t_Query_ As String = Nothing

        t_Query_ = "INSERT INTO [Solium].[dbo].[Reg030ErroresRecolector] " & _
                "([i_Cve_RecolectorDocumentos], " & _
                "[i_Cve_BitacoraRecolectorDocumentos], " & _
                "[t_CodigoError], " & _
                "[t_DescripcionError], " & _
                "[f_FechaRegistro], " & _
                "[i_Cve_Estatus], " & _
                "[i_Cve_Estado]) " & _
                "VALUES" & _
                "(" & _
                "" & i_Cve_RecolectorDocumentos_ & "," & _
                "" & i_Cve_BitacoraRecolectorDocumentos_ & "," & _
                "" & (IIf(t_CodigoError_ Is Nothing, "NULL", "'" & t_CodigoError_ & "'")) & ", " & _
                "" & (IIf(t_DescripcionError_ Is Nothing, "NULL", "'" & t_DescripcionError_ & "'")) & ", " & _
                "SYSDATETIME()," & _
                "1," & _
                "1)"

        Try

            tagWatcher_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(t_Query_)

            If Not tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                b_Resultado_ = True

            End If

        Catch ex As System.Exception

            EscribirEnLog(System.DateTime.Now & " - " & ex.Message)

        End Try

        Return b_Resultado_

    End Function

    Private Function ClasificarDocumento(ByVal t_RutaOriginal_ As String,
                                         ByVal t_Extension_ As String,
                                         ByVal i_IgnorarExtension As Boolean,
                                         ByVal t_PatronIdentificable_ As String,
                                         ByVal i_PatronSensible_ As Boolean) As Boolean


        Dim b_Resultado_ As Boolean = False

        Dim t_NombreArchivo_ As String = Nothing

        Dim t_ExtensionArchivo_ As String = Nothing

        Dim i_OpcionBusqueda_ As Int32 = Nothing

        t_NombreArchivo_ = Path.GetFileName(t_RutaOriginal_)

        t_ExtensionArchivo_ = Path.GetExtension(t_RutaOriginal_)

        ' Verifica si el patrón es sensible a mayúsculas y minúsculas
        If i_PatronSensible_ Then

            i_OpcionBusqueda_ = RegexOptions.None

        Else

            i_OpcionBusqueda_ = RegexOptions.IgnoreCase

        End If

        If Regex.IsMatch(t_NombreArchivo_, t_PatronIdentificable_, i_OpcionBusqueda_) Then

            ' Verifica si se debe tomar en cuenta la extensión del tipo del documento configurado
            If i_IgnorarExtension Then

                b_Resultado_ = True

            Else

                If t_Extension_.ToUpper().Equals(t_ExtensionArchivo_.ToUpper()) Then

                    b_Resultado_ = True

                End If

            End If

        End If

        Return b_Resultado_

    End Function

    Private Function InsertarMaestroDocumentos(ByVal operacionesDocumento_ As OperacionesDocumentoHilos64,
                                               ByVal i_Cve_DivisionMiEmpresa_ As String,
                                               ByVal i_Cve_EncPlantillaDocumento_ As String,
                                               ByVal i_Cve_TipoDocumento_ As String,
                                               ByVal t_RutaOriginal_ As String,
                                               ByVal listaCaracteristicasDocumento_ As Dictionary(Of Integer, CaracteristicaDocumento),
                                               ByVal i_ConsultableCliente_ As Integer) As TagWatcher

        Dim tagWatcher_ As New TagWatcher

        Dim documento_ As New Dictionary(Of String, String)

        operacionesDocumento_.DivisionMiEmpresa = i_Cve_DivisionMiEmpresa_

        operacionesDocumento_.Documento.PlantillaDocumento.ClavePlantilla = i_Cve_EncPlantillaDocumento_

        operacionesDocumento_.CargarPlantilla()

        Dim caracteristicasDocumentos_ = New SortedDictionary(Of Integer, CaracteristicaDocumento)

        If operacionesDocumento_.Documento.PlantillaDocumento.CaracteristicasDocumentos.Count > 0 Then

            ' Llenado de caracteristicas

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In operacionesDocumento_.Documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                If listaCaracteristicasDocumento_.ContainsKey(caracteristicaDocumento_.ClaveCaracteristica) Then

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            caracteristicaDocumento_.ValorClaveCatalogo = listaCaracteristicasDocumento_.Item(caracteristicaDocumento_.ClaveCaracteristica).ValorClaveCatalogo

                            caracteristicaDocumento_.ValorCatalogo = listaCaracteristicasDocumento_.Item(caracteristicaDocumento_.ClaveCaracteristica).ValorCatalogo

                        Case Else

                            caracteristicaDocumento_.Valor = listaCaracteristicasDocumento_.Item(caracteristicaDocumento_.ClaveCaracteristica).Valor

                    End Select

                    If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                        Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                                caracteristicaDocumento_.ValorClaveCatalogoFin = listaCaracteristicasDocumento_.Item(caracteristicaDocumento_.ClaveCaracteristica).ValorClaveCatalogoFin

                                caracteristicaDocumento_.ValorCatalogoFin = listaCaracteristicasDocumento_.Item(caracteristicaDocumento_.ClaveCaracteristica).ValorCatalogoFin

                            Case Else

                                caracteristicaDocumento_.ValorFin = listaCaracteristicasDocumento_.Item(caracteristicaDocumento_.ClaveCaracteristica).ValorFin

                        End Select

                    End If

                Else   ' Si no encuentra la caracteristica 

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                            caracteristicaDocumento_.ValorClaveCatalogo = Nothing

                            caracteristicaDocumento_.ValorCatalogo = Nothing

                        Case Else

                            caracteristicaDocumento_.Valor = ObtenerValorCaracteristica(i_Cve_TipoDocumento_, caracteristicaDocumento_.ClaveCaracteristica, listaCaracteristicasDocumento_)

                    End Select

                    If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                        Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                            Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                                caracteristicaDocumento_.ValorClaveCatalogoFin = Nothing

                                caracteristicaDocumento_.ValorCatalogoFin = Nothing

                            Case Else

                                caracteristicaDocumento_.ValorFin = Nothing

                        End Select

                    End If

                    listaCaracteristicasDocumento_.Add(caracteristicaDocumento_.ClaveCaracteristica, caracteristicaDocumento_)

                End If

                caracteristicasDocumentos_.Add(orden_, caracteristicaDocumento_)

            Next

            ' Procesa documento

            operacionesDocumento_.Documento.PlantillaDocumento.CaracteristicasDocumentos = caracteristicasDocumentos_

            operacionesDocumento_.Documento.DocumentoConsultableCliente = i_ConsultableCliente_

            operacionesDocumento_.Documento.RutaDocumentoOrigen = t_RutaOriginal_

            operacionesDocumento_.ProcesarDocumento()

            tagWatcher_ = operacionesDocumento_.Estatus

            If tagWatcher_.Status = TagWatcher.TypeStatus.Ok Then

                ' Obtiene valores del documento registrado

                documento_.Clear()

                documento_.Add("i_Cve_Documento", operacionesDocumento_.Documento.Clave)

                documento_.Add("t_RutaRecoleccion", operacionesDocumento_.Documento.RutaDocumentoCompleto & operacionesDocumento_.Documento.NombreDocumento)

                documento_.Add("f_FechaRecoleccion", operacionesDocumento_.Documento.FechaRegistro.Date & " " & operacionesDocumento_.Documento.FechaRegistro.Hour & ":" & operacionesDocumento_.Documento.FechaRegistro.Minute & ":" & operacionesDocumento_.Documento.FechaRegistro.Second)

                tagWatcher_.ObjectReturned = documento_

            End If

        End If

        Return tagWatcher_

    End Function

    Private Function ObtieneTamanoArchivoKB(ByVal t_Path_ As String) As String

        Dim fiInfo_ As New FileInfo(t_Path_)

        If fiInfo_.Exists Then

            Return Math.Round((fiInfo_.Length / 1024), 2).ToString()

        Else

            Return String.Empty

        End If

    End Function

    Private Function ObtenerValorCaracteristica(ByVal i_Cve_TipoDocumento_ As Integer,
                                                ByVal i_Cve_CaracteristicaDocumento_ As Integer,
                                                ByRef listaCaracteristicasDocumento_ As Dictionary(Of Integer, CaracteristicaDocumento)) As String

        Dim t_ValorCaracteristica_ As String = Nothing

        Dim i_Cve_CaracteristicaBusqueda_ As Integer = Nothing

        Dim t_ValorCaracteristicaBusqueda_ As String = Nothing

        Dim t_NombreColumna_ As String = Nothing

        Dim dataTable_ As DataTable = Nothing

        Dim t_ClausulasLibres_ As String = Nothing

        Dim t_ClausulasLibresContains_ As String = Nothing

        ' Asigna valores para las búsquedas

        Select Case i_Cve_CaracteristicaDocumento_

            Case 49 ' REFERENCIA AA

                i_Cve_CaracteristicaBusqueda_ = 57

                t_NombreColumna_ = "NumeroReferencia"

            Case 50 ' REFERENCIA BODEGA

            Case 51 ' CLAVE ADUANA SECCION

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "ClaveAduana"

            Case 52 ' PATENTE

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "Patente"

            Case 53 ' RFC CLIENTE

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "RFCCliente"

            Case 54 ' NUMERO PEDIMENTO COMPLETO

            Case 55 ' AÑO PEDIMENTO

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "Anio"

            Case 56  'NUMERO EDOCUMENT

                i_Cve_CaracteristicaBusqueda_ = 65

                t_NombreColumna_ = "eDocument"

            Case 63 ' NUMERO PEDIMENTO (6 DIGITOS)

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "NumeroPedimento"

            Case 66 ' TIPO EDOCUMENT

                i_Cve_CaracteristicaBusqueda_ = 56

                t_NombreColumna_ = "ClaveTipoDocumento"

            Case 67 ' RAZON SOCIAL CLIENTE

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "Cliente"

            Case 68 ' NOMBRE ADUANA

                i_Cve_CaracteristicaBusqueda_ = 49

                t_NombreColumna_ = "NombreAduana"

        End Select

        If i_Cve_CaracteristicaBusqueda_ = 49 Then

            'If (Not listaCaracteristicasDocumento_.ContainsKey(49)) And (listaCaracteristicasDocumento_.ContainsKey(57)) Then

            If (listaCaracteristicasDocumento_.ContainsKey(57)) Then

                If listaCaracteristicasDocumento_.Item(57).Valor <> Nothing Then

                    i_Cve_CaracteristicaBusqueda_ = 57

                End If

            End If

        End If

        'Realiza las búsquedas

        If (listaCaracteristicasDocumento_.ContainsKey(i_Cve_CaracteristicaBusqueda_)) Then

            t_ValorCaracteristicaBusqueda_ = listaCaracteristicasDocumento_.Item(i_Cve_CaracteristicaBusqueda_).Valor

            Select Case i_Cve_CaracteristicaBusqueda_

                Case 49 ' REFERENCIA AA

                    t_ClausulasLibres_ = " WHERE NumeroReferencia LIKE '" & t_ValorCaracteristicaBusqueda_ & "'"

                    t_ClausulasLibresContains_ = " WHERE CONTAINS (NumeroReferencia, '" & t_ValorCaracteristicaBusqueda_ & "')"

                    ' En caso de que cuente con las caracteristicas RFC CLIENTE, NUMERO PEDIMENTO y PATENTE las evalua también

                    If listaCaracteristicasDocumento_.ContainsKey(51) Then  ' Clave Aduana Sección

                        t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ClaveAduana LIKE '" & listaCaracteristicasDocumento_.Item(51).Valor & "'"

                        t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ClaveAduana, '" & listaCaracteristicasDocumento_.Item(51).Valor & "')"

                    End If

                    If listaCaracteristicasDocumento_.ContainsKey(52) Then  ' Patente

                        t_ClausulasLibres_ = t_ClausulasLibres_ & " AND Patente LIKE '" & listaCaracteristicasDocumento_.Item(52).Valor & "'"

                        t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (Patente, '" & listaCaracteristicasDocumento_.Item(52).Valor & "')"

                    End If

                    If listaCaracteristicasDocumento_.ContainsKey(53) Then  ' RFCCliente

                        t_ClausulasLibres_ = t_ClausulasLibres_ & " AND RFCCliente LIKE '" & listaCaracteristicasDocumento_.Item(53).Valor & "'"

                        t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (RFCCliente, '" & listaCaracteristicasDocumento_.Item(53).Valor & "')"

                    End If

                    If listaCaracteristicasDocumento_.ContainsKey(63) Then  ' Pedimento

                        t_ClausulasLibres_ = t_ClausulasLibres_ & " AND NumeroPedimento LIKE '" & listaCaracteristicasDocumento_.Item(63).Valor & "'"

                        t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (NumeroPedimento, '" & listaCaracteristicasDocumento_.Item(63).Valor & "')"

                    End If

                    If (listaCaracteristicasDocumento_.ContainsKey(62)) Then   'Número COVE

                        If listaCaracteristicasDocumento_.Item(62).Valor <> Nothing Then

                            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND NumerosCOVE LIKE '%" & listaCaracteristicasDocumento_.Item(62).Valor & "%'"

                            t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (NumerosCOVE, '" & listaCaracteristicasDocumento_.Item(62).Valor & "')"

                        End If

                    End If

                    If (listaCaracteristicasDocumento_.ContainsKey(96)) Then   'Número Integración

                        If listaCaracteristicasDocumento_.Item(96).Valor <> Nothing Then

                            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND NumerosIntegracion LIKE '%" & listaCaracteristicasDocumento_.Item(96).Valor & "%'"

                            t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (NumerosIntegracion, '" & listaCaracteristicasDocumento_.Item(96).Valor & "')"

                        End If

                    End If

                    If listaCaracteristicasDocumento_.ContainsKey(65) Then   ' Nombre original archivo

                        Select Case i_Cve_TipoDocumento_

                            Case 110 ' Archivo de validación M3

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoValidacion LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ArchivoValidacion, '" & listaCaracteristicasDocumento_.Item(65).Valor & "')"

                            Case 116 ' Archivo de respuesta de validación

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoAcuseValidacion LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ArchivoAcuseValidacion, '" & listaCaracteristicasDocumento_.Item(65).Valor & "')"

                            Case 117 ' Archivo de pago electrónico

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoPago LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ArchivoPago, '" & listaCaracteristicasDocumento_.Item(65).Valor & "')"

                            Case 118 ' Archivo de acuse de pago electrónico

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoAcusePago LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ArchivoAcusePago, '" & listaCaracteristicasDocumento_.Item(65).Valor & "')"

                        End Select

                    End If

                    dataTable_ = ConsultarDatosReferencia(t_ClausulasLibres_, t_ClausulasLibresContains_)

                    If Not dataTable_ Is Nothing Then

                        If dataTable_.Rows.Count > 0 Then

                            t_ValorCaracteristica_ = dataTable_.Rows(0).Item(t_NombreColumna_)

                        End If

                    End If

                Case 56 ' NUMERO EDOCUMENT

                    If Not t_ValorCaracteristicaBusqueda_ Is Nothing Then

                        If listaCaracteristicasDocumento_.ContainsKey(49) Then

                            t_ClausulasLibres_ = " WHERE NumeroReferencia LIKE '" & listaCaracteristicasDocumento_.Item(49).Valor & "' AND eDocument LIKE '" & t_ValorCaracteristicaBusqueda_ & "'"

                            t_ClausulasLibresContains_ = " WHERE CONTAINS (NumeroReferencia, '" & listaCaracteristicasDocumento_.Item(49).Valor & "') AND CONTAINS (eDocument, '" & t_ValorCaracteristicaBusqueda_ & "')"

                            If listaCaracteristicasDocumento_.ContainsKey(51) Then  ' Clave Aduana Sección

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ClaveAduana LIKE '" & listaCaracteristicasDocumento_.Item(51).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ClaveAduana, '" & listaCaracteristicasDocumento_.Item(51).Valor & "')"

                            End If

                            If listaCaracteristicasDocumento_.ContainsKey(52) Then  ' Patente

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND Patente LIKE '" & listaCaracteristicasDocumento_.Item(52).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (Patente, '" & listaCaracteristicasDocumento_.Item(52).Valor & "')"

                            End If

                            If listaCaracteristicasDocumento_.ContainsKey(63) Then  ' Pedimento

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND NumeroPedimento LIKE '" & listaCaracteristicasDocumento_.Item(63).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (NumeroPedimento, '" & listaCaracteristicasDocumento_.Item(63).Valor & "')"

                            End If

                            'If listaCaracteristicasDocumento_.ContainsKey(65) Then   ' Nombre original archivo

                            '    If i_Cve_TipoDocumento_ = 119 Then

                            '        t_ClausulasLibres_ = t_ClausulasLibres_ & " AND NombreDocumentoExpediente LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                            '    End If

                            'End If

                            dataTable_ = ConsultarDatosDigitalizacionVUCEM(t_ClausulasLibres_, t_ClausulasLibresContains_)

                        End If

                        If Not dataTable_ Is Nothing Then

                            If dataTable_.Rows.Count > 0 Then

                                t_ValorCaracteristica_ = dataTable_.Rows(0).Item(t_NombreColumna_)

                                If listaCaracteristicasDocumento_.ContainsKey(65) Then   ' Nombre original archivo

                                    listaCaracteristicasDocumento_.Item(65).Valor = dataTable_.Rows(0).Item("Archivo")

                                End If

                            Else

                                ' Se extraia del número de edocumento, se descarta por errores de vucem
                                If t_ValorCaracteristicaBusqueda_.Length = 13 Then

                                    t_ValorCaracteristica_ = t_ValorCaracteristicaBusqueda_.Substring(1, 3)

                                End If

                            End If

                        End If

                    End If

                Case 57  ' REFERENCIA INTERNA SLAM

                    If Not t_ValorCaracteristicaBusqueda_ Is Nothing Then

                        t_ClausulasLibres_ = " WHERE ( ReferenciaInternaSLAM LIKE '" & t_ValorCaracteristicaBusqueda_ & "'" & _
                        " OR ReferenciaInternaSLAM LIKE '" & Replace(t_ValorCaracteristicaBusqueda_, "-", "/") & "' )"

                        t_ClausulasLibresContains_ = " WHERE ( CONTAINS (ReferenciaInternaSLAM, '" & t_ValorCaracteristicaBusqueda_ & "')" & _
                        " OR CONTAINS (ReferenciaInternaSLAM, '" & Replace(t_ValorCaracteristicaBusqueda_, "-", "/") & "') )"

                        If listaCaracteristicasDocumento_.ContainsKey(51) Then  ' Clave Aduana Sección

                            If Not listaCaracteristicasDocumento_.Item(51).Valor Is Nothing Then

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ClaveAduana LIKE '" & listaCaracteristicasDocumento_.Item(51).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (ClaveAduana, '" & listaCaracteristicasDocumento_.Item(51).Valor & "')"

                            End If

                        End If

                        If listaCaracteristicasDocumento_.ContainsKey(52) Then  ' Patente

                            If Not listaCaracteristicasDocumento_.Item(52).Valor Is Nothing Then

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND Patente LIKE '" & listaCaracteristicasDocumento_.Item(52).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (Patente, '" & listaCaracteristicasDocumento_.Item(52).Valor & "')"

                            End If

                        End If

                        If listaCaracteristicasDocumento_.ContainsKey(53) Then  ' RFCCliente

                            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND RFCCliente LIKE '" & listaCaracteristicasDocumento_.Item(53).Valor & "'"

                            t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (RFCCliente, '" & listaCaracteristicasDocumento_.Item(53).Valor & "')"

                        End If

                        If listaCaracteristicasDocumento_.ContainsKey(63) Then  ' Pedimento

                            If Not listaCaracteristicasDocumento_.Item(63).Valor Is Nothing Then

                                t_ClausulasLibres_ = t_ClausulasLibres_ & " AND NumeroPedimento LIKE '" & listaCaracteristicasDocumento_.Item(63).Valor & "'"

                                t_ClausulasLibresContains_ = t_ClausulasLibresContains_ & " AND CONTAINS (NumeroPedimento, '" & listaCaracteristicasDocumento_.Item(63).Valor & "')"

                            End If

                        End If

                        'If listaCaracteristicasDocumento_.ContainsKey(65) Then   ' Nombre original archivo

                        '    Select Case i_Cve_TipoDocumento_

                        '        Case 110 ' Archivo de validación M3

                        '            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoValidacion LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                        '        Case 116 ' Archivo de respuesta de validación

                        '            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoAcuseValidacion LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                        '        Case 117 ' Archivo de pago electrónico

                        '            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoPago LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                        '        Case 118 ' Archivo de acuse de pago electrónico

                        '            t_ClausulasLibres_ = t_ClausulasLibres_ & " AND ArchivoAcusePago LIKE '" & listaCaracteristicasDocumento_.Item(65).Valor & "'"

                        '    End Select

                        'End If

                        dataTable_ = ConsultarDatosReferencia(t_ClausulasLibres_, t_ClausulasLibresContains_)

                        If Not dataTable_ Is Nothing Then

                            If dataTable_.Rows.Count > 0 Then

                                t_ValorCaracteristica_ = dataTable_.Rows(0).Item(t_NombreColumna_)

                                listaCaracteristicasDocumento_.Item(57).Valor = dataTable_.Rows(0).Item("ReferenciaInternaSLAM")

                            End If

                        End If

                    End If

                Case 65  ' NOMBRE ORIGINAL ARCHIVO

                    If i_Cve_TipoDocumento_ = 119 Then    ' Documento para digitalización VUCEM (EDOCUMENT)

                        If listaCaracteristicasDocumento_.ContainsKey(49) Then

                            'dataTable_ = ConsultarDatosDigitalizacionVUCEM(" WHERE NumeroReferencia LIKE '" & listaCaracteristicasDocumento_.Item(49).Valor & "' AND NombreDocumentoExpediente LIKE '" & t_ValorCaracteristicaBusqueda_ & "-para'")

                            'dataTable_ = ConsultarDatosDigitalizacionVUCEM(" WHERE NumeroReferencia LIKE '" & listaCaracteristicasDocumento_.Item(49).Valor & "' AND NombreDocumentoExpediente LIKE '" & t_ValorCaracteristicaBusqueda_ & "para'")

                            dataTable_ = ConsultarDatosDigitalizacionVUCEM(" WHERE NumeroReferencia LIKE '" & listaCaracteristicasDocumento_.Item(49).Valor & "' AND NombreDocumentoExpediente LIKE '" & t_ValorCaracteristicaBusqueda_ & "para'", _
                                                                           " WHERE CONTAINS (NumeroReferencia, '" & listaCaracteristicasDocumento_.Item(49).Valor & "') AND CONTAINS (NombreDocumentoExpediente, '" & t_ValorCaracteristicaBusqueda_ & "para')")

                        End If

                        If Not dataTable_ Is Nothing Then

                            If dataTable_.Rows.Count > 0 Then

                                t_ValorCaracteristica_ = dataTable_.Rows(0).Item(t_NombreColumna_)

                                listaCaracteristicasDocumento_.Item(65).Valor = dataTable_.Rows(0).Item("Archivo")

                            End If

                        End If

                    End If

            End Select

        End If

        Return t_ValorCaracteristica_

    End Function

    Private Function VerificarWhere(ByVal t_Sentencia_ As String) As Boolean

        Dim b_Resultado_ As Boolean = False

        If t_Sentencia_.Contains("where") Or t_Sentencia_.Contains("WHERE") Then

            b_Resultado_ = True

        End If

        Return b_Resultado_

    End Function

    Private Sub EscribirEnLog(ByVal t_Texto_ As String)

        Dim t_Ruta_ As String = "C:\logs\RecolectorDocumentos\log_recolectorDocumentos.txt"

        Dim escritor_ As StreamWriter

        Try

            escritor_ = File.AppendText(t_Ruta_)

            escritor_.WriteLine(t_Texto_)

            escritor_.Flush()

            escritor_.Close()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub InsertarRegistrosBitacora(ByVal i_Cve_RecolectorDocumentos_ As Integer,
                                      ByVal documentosSinRegistrar_ As List(Of String),
                                      ByVal i_IndexInicial As Integer,
                                      ByVal i_IndexFinal As Integer)

        For Each documentoSinRegistrar_ As String In documentosSinRegistrar_.GetRange(i_IndexInicial, (i_IndexFinal - (i_IndexInicial)) + 1)

            InsertarBitacoraRecolectorDocumentos(i_Cve_RecolectorDocumentos_, Nothing, Nothing, Nothing, documentoSinRegistrar_, ObtieneTamanoArchivoKB(documentoSinRegistrar_))

        Next

    End Sub

    Private Function ConvertListToJSON(ByVal i_Cve_RecolectorDocumentos_ As String,
                                     ByVal t_NombreRecolector_ As String,
                                     ByVal t_TipoArchivo_ As String,
                                     ByVal lista_ As List(Of String)) As Boolean

        Try

            If Not lista_ Is Nothing Then

                Dim serialJSON_ As New System.Runtime.Serialization.Json.DataContractJsonSerializer(lista_.GetType)

                Dim memoryStream_ As New System.IO.MemoryStream

                serialJSON_.WriteObject(memoryStream_, lista_)

                Dim textoJson As String = System.Text.Encoding.UTF8.GetString(memoryStream_.ToArray)

                Dim fileName_ As String = "C:\logs\RecolectorDocumentos\" & t_TipoArchivo_ & "_" & i_Cve_RecolectorDocumentos_ & "_" & t_NombreRecolector_ & "_" & System.DateTime.Now.Date.AddDays(-1).Year & System.DateTime.Now.AddDays(-1).DayOfYear & ".json"

                Dim objWriter_ As New System.IO.StreamWriter(fileName_)

                objWriter_.Write(textoJson)

                objWriter_.Close()

                Return True

            End If

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Function GetListAsJSON(ByVal i_Cve_RecolectorDocumentos_ As Integer,
                                  ByVal t_NombreRecolector_ As String,
                                  ByVal t_TipoArchivo_ As String) As TagWatcher

        Dim tagwatcher_ As New TagWatcher

        Dim jsonFile_ As String = Nothing

        Dim rutaLogs_ As String = Nothing

        If Directory.Exists("C:\logs\RecolectorDocumentos\") Then

            rutaLogs_ = "C:\logs\RecolectorDocumentos\"

        Else

            Try

                Directory.CreateDirectory("C:\logs\RecolectorDocumentos\")

                rutaLogs_ = "C:\logs\RecolectorDocumentos\"

            Catch ex As Exception

                If Not Directory.Exists("C:\logs\RecolectorDocumentos\") Then

                    Directory.CreateDirectory(rutaLogs_ = "C:\logs\RecolectorDocumentos\")

                    rutaLogs_ = "C:\logs\RecolectorDocumentos\"

                End If

            End Try

        End If

        jsonFile_ = GetTextForOutput(rutaLogs_ & t_TipoArchivo_ & "_" & i_Cve_RecolectorDocumentos_ & "_" & t_NombreRecolector_ & "_" & System.DateTime.Now.AddDays(-1).Year & System.DateTime.Now.AddDays(-1).DayOfYear & ".json")

        If Not jsonFile_ Is Nothing Then

            Dim serialJSON_ As New System.Web.Script.Serialization.JavaScriptSerializer

            serialJSON_.MaxJsonLength = 500000000

            Dim requestedObject_ As List(Of String) = serialJSON_.Deserialize(Of List(Of String))(jsonFile_)

            With tagwatcher_

                .ObjectReturned = requestedObject_

                .SetOK()

            End With

        Else

            tagwatcher_.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End If

        Return tagwatcher_

    End Function

    Private Function GetTextForOutput(ByVal filePath_ As String) As String

        Dim sb_ As New System.Text.StringBuilder()

        Dim sr_ As System.IO.StreamReader

        ' Verifica que el archivo exista
        If My.Computer.FileSystem.FileExists(filePath_) = False Then

            Return Nothing

        End If

        ' Abre el archivo
        sr_ = My.Computer.FileSystem.OpenTextFileReader(filePath_)

        ' Lee el archivo
        If sr_.Peek() >= 0 Then

            sb_.Append(sr_.ReadLine())

        End If

        sr_.Close()

        Return sb_.ToString

    End Function

#End Region

#Region "Eventos"

#End Region

End Class
