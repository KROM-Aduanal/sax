Imports Wma.Exceptions
Imports gsol.basedatos.Operaciones
Imports gsol.Componentes.SistemaBase.GsDialogo
Imports System.IO
Imports gsol.documento
Imports System.Text

Namespace gsol.documento

    Public Class OperacionesDocumentoHilos64
        Implements IOperacionesDocumento


#Region "Atributos"

        Private _estatus As TagWatcher

        Private _operaciones As IOperacionesCatalogo

        Private _sistema As Organismo

        Private _documento As Documento

        Private _divisionMiEmpresa As Int32

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

        Public Property DivisionMiEmpresa As Int32

            Get

                Return _divisionMiEmpresa

            End Get

            Set(value As Int32)

                _divisionMiEmpresa = value

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

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

            Me.New()

            _operaciones = ioperaciones_

        End Sub

#End Region

#Region "Métodos"

        Public Sub ProcesarDocumento(Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.ProcesarDocumento

            _estatus.SetOK()

            _documento.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            If Not _operaciones.EspacioTrabajo.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then

                _documento.ClaveDivisionMiEmpresa = _divisionMiEmpresa

            End If

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

        End Sub

        Private Sub CargarRepositorios() Implements IOperacionesDocumento.CargarRepositorios

            Dim dataTable_ As DataTable = Nothing

            Dim consulta_ As String = Nothing

            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                         "select * from Vt012PlantillasDocumentos where i_cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & _
                         " and i_Cve_DivisionMiEmpresa = " & _divisionMiEmpresa

            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                If dataTable_.Rows.Count > 0 Then

                    For Each fila_ As DataRow In dataTable_.Rows

                        _documento.ClaveTipoDocumento = fila_.Item("i_Cve_TipoDocumento").ToString

                        _documento.TipoDocumento = fila_.Item("t_Cve_TipoDocumento").ToString

                    Next

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1000)

                End If

                If Not _documento.ClaveTipoDocumento = 0 Then

                    consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                                "select * from Vt012TiposDocumentos where i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento

                    _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                    If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                        If dataTable_.Rows.Count > 0 Then

                            For Each fila_ As DataRow In dataTable_.Rows

                                _documento.ClaveTipoDocumento = fila_.Item("i_Cve_TipoDocumento").ToString

                                _documento.TipoDocumento = fila_.Item("t_Cve_TipoDocumento").ToString

                                Dim TagAux_ As TagWatcher = Nothing

                                Dim DataTableAux_ As DataTable = Nothing

                                consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                                             "select * from Vt012RepositoriosDigitales where i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento & "" & _
                                             " and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa

                                TagAux_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                                DataTableAux_ = DirectCast(TagAux_.ObjectReturned, DataTable)

                                If DataTableAux_.Rows.Count > 0 Then

                                    For Each fila2_ As DataRow In DataTableAux_.Rows

                                        _documento.RutaDocumentoCompleto = fila2_.Item("t_Servidor").ToString & fila2_.Item("t_RutaCarpeta").ToString & "\"

                                        _documento.RutaDocumento = Nothing

                                        _documento.RutaDocumentoContingenciaCompleto = fila2_.Item("t_ServidorContingencia").ToString & fila2_.Item("t_RutaCarpetaContingencia").ToString & "\"

                                        _documento.RutaDocumentoContingencia = Nothing

                                        _documento.ClaveRepositorioDigital = fila2_.Item("i_Cve_RepositorioDigital").ToString

                                        _documento.TipoArchivo = fila_.Item("i_Cve_TipoArchivo").ToString

                                        _documento.Extension = Path.GetExtension(_documento.RutaDocumentoOrigen).ToLower()

                                        _documento.NombreDocumento = Path.GetFileNameWithoutExtension(_documento.RutaDocumentoOrigen)

                                        'If Not fila_.Item("t_Extension").ToString = _documento.Extension Then

                                        '    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1009)

                                        'End If

                                    Next

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1002)

                                End If

                            Next

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1001)

                        End If

                    End If

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1001)

                End If

            End If

        End Sub

        Private Sub ValidarDocumentosDuplicados() Implements IOperacionesDocumento.ValidarDocumentosDuplicados

            Dim clausulasSQL_ As List(Of String) = New List(Of String)

            Dim cantidadCaracteristicas_ As Integer = 0

            Dim clavesCaracteristicas_ As String = Nothing

            Dim valoresCaracteristicas_ As String = Nothing

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                Dim valorCampoInicial_ As String = Nothing

                Dim valorCampoFinal_ As String = Nothing

                Dim formatoCaracteristica_ As CaracteristicaDocumento.TiposDatosCaracteristicas = caracteristicaDocumento_.TipoDatoCaracteristica

                If caracteristicaDocumento_.CaracteristicaRevision = CaracteristicaDocumento.Revision.Si Then

                    If (CStr(caracteristicaDocumento_.Valor) <> "" And formatoCaracteristica_ <> CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo) Or _
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

            Dim dataTable_ As DataTable = Nothing

            Dim consulta_ As String = Nothing

            'consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
            '            "SELECT COUNT(*) Cantidad " & _
            '            "FROM VT012CatalogoVisorMaestroDocumentos AS cat WITH(NOLOCK) " & _
            '            "WHERE cat.i_Cve_TipoDocumento = " & _documento.ClaveTipoDocumento & _
            '            " AND EXISTS (SELECT i_Cve_Documento " & _
            '            "             FROM Ext012CaracteristicasExtDocumentos as b WITH(NOLOCK) " & _
            '            "             WHERE cat.i_Cve_Documento = b.i_Cve_Documento AND i_Cve_CaracteristicaDocumento IN (" & clavesCaracteristicas_ & ") AND (" & valoresCaracteristicas_ & ") " & _
            '            "             GROUP BY i_Cve_Documento " & _
            '            "             HAVING COUNT(*) = " & cantidadCaracteristicas_ & ")"

            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                        "SELECT i_Cve_Documento " & _
                        "FROM Ext012CaracteristicasExtDocumentos AS b WITH(NOLOCK) " & _
                        "WHERE i_Cve_Estatus in (1,2) and i_cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & _
                        " AND i_Cve_CaracteristicaDocumento IN (" & clavesCaracteristicas_ & ") AND (" & valoresCaracteristicas_ & ") " & _
                        "GROUP BY i_Cve_Documento " & _
                        "HAVING COUNT(*) = " & cantidadCaracteristicas_ & ""

            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                If dataTable_.Rows.Count > 0 Then

                    If dataTable_(0)(0) > 0 Then

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1030)

                    End If

                End If

            End If

        End Sub

        Public Sub CargarPlantilla() Implements IOperacionesDocumento.CargarPlantilla

            Dim dataTable_ As DataTable = Nothing

            _documento.PlantillaDocumento.CaracteristicasDocumentos.Clear()

            Dim consulta_ As String = Nothing

            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                         "select * from VT012ResumenPlantillasCaracteristicas where i_cve_EncPlantillaDocumento = " & _documento.PlantillaDocumento.ClavePlantilla & " " & _
                         " and i_Cve_DivisionMiEmpresa = " & _divisionMiEmpresa

            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                If dataTable_.Rows.Count > 0 Then

                    For Each fila_ As DataRow In dataTable_.Rows

                        _documento.PlantillaDocumento.ClavePlantilla = fila_.Item("i_Cve_EncPlantillaDocumento").ToString

                        _documento.PlantillaDocumento.NombrePlantilla = fila_.Item("t_Nombre").ToString

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

                            caracteristicaDocumento_.CaracteristicaRevision = IIf(fila_.Item("i_Revision").ToString, 1, 0)

                            caracteristicaDocumento_.AnchoVisual = fila_.Item("i_AnchoVisual").ToString

                            caracteristicaDocumento_.CaracteristicaRutaDinamica = IIf(fila_.Item("i_RutaDinamica").ToString, 1, 0)

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

                    _estatus.ObjectReturned = caracteristicaDocumento_.TituloCaracteristica

                    Exit For

                End If

                If caracteristicaDocumento_.CaracteristicaRutaDinamica = CaracteristicaDocumento.RutaDinamica.Si Then

                    Select Case caracteristicaDocumento_.TipoDatoCaracteristica

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                            _documento.RutaDocumentoCompleto = _documento.RutaDocumentoCompleto & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumentoContingenciaCompleto = _documento.RutaDocumentoContingenciaCompleto & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumento = _documento.RutaDocumento & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                            _documento.RutaDocumentoContingencia = _documento.RutaDocumentoContingencia & Year(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "\" & Day(CType(caracteristicaDocumento_.Valor, Date)) & "\"

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

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

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha

                            If valorRenombrar_ = 1 Then

                                _documento.NombreDocumento = Year(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Day(CType(caracteristicaDocumento_.Valor, Date))

                            Else

                                _documento.NombreDocumento = _documento.NombreDocumento & "_" & Year(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Month(CType(caracteristicaDocumento_.Valor, Date)) & "_" & Day(CType(caracteristicaDocumento_.Valor, Date))

                            End If

                            valorRenombrar_ = valorRenombrar_ + 1

                        Case CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora

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

            Next

            _documento.NombreDocumento = _documento.NombreDocumento & "_" & _documento.TipoDocumento

        End Sub

        Private Sub CrearDirectorio() Implements IOperacionesDocumento.CrearDirectorio

            Dim dataTable_ As DataTable = Nothing

            Dim consulta_ As String = Nothing

            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                         "select * from VT012Directorios where t_RepositorioDirectorioCompleto = '" & _documento.RutaDocumentoCompleto & "'"

            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                If dataTable_.Rows.Count > 0 Then

                    For Each fila_ As DataRow In dataTable_.Rows

                        _documento.ClaveDirectorio = fila_.Item("i_Cve_Directorio").ToString

                    Next

                Else

                    consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                                "INSERT INTO [dbo].[Reg012Directorios] " & _
                                "           ([t_RepositorioDirectorio]" & _
                                "           ,[t_RepositorioDirectorioCompleto]" & _
                                "           ,[t_RepositorioDirectorioContingencia] " & _
                                "           ,[t_RepositorioDirectorioCompletoContingencia] " & _
                                "           ,[i_Cve_Estatus]" & _
                                "           ,[i_Cve_Estado]" & _
                                "           ,[i_Cve_DivisionMiEmpresa]) " & _
                                "     VALUES " & _
                                "           ('" & _documento.RutaDocumento & "'" & _
                                "           ,'" & _documento.RutaDocumentoCompleto & "'" & _
                                "           ,'" & _documento.RutaDocumentoContingencia & "'" & _
                                "           ,'" & _documento.RutaDocumentoContingenciaCompleto & "'" & _
                                "           ,1" & _
                                "           ,1" & _
                                "           , " & _documento.ClaveDivisionMiEmpresa & ")" & _
                                "  SELECT @@IDENTITY AS 'Clave'"

                    _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                    If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                        If dataTable_.Rows.Count > 0 Then

                            For Each fila_ As DataRow In dataTable_.Rows

                                _documento.ClaveDirectorio = fila_.Item("Clave").ToString
                            Next

                        Else

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1004)

                        End If

                    Else

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1004)

                    End If

                End If

            End If

        End Sub

        Private Sub InsertarRegistros() Implements IOperacionesDocumento.InsertarRegistros

            Dim dataTable_ As DataTable = Nothing

            Dim consulta_ As String = Nothing

            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                          "INSERT INTO [dbo].[Reg012Documentos] " & _
                          "           ([t_RutaDocumentoOrigen]" & _
                          "           ,[i_Cve_Estado]" & _
                          "           ,[i_Cve_TipoDocumento] " & _
                          "           ,[f_FechaRegistro] " & _
                          "           ,[i_Cve_EstatusDocumento]" & _
                          "           ,[t_NombreDocumento]" & _
                          "           ,[t_FolioDocumento]" & _
                          "           ,[i_Cve_RepositorioDigital]" & _
                          "           ,[t_Documento]" & _
                          "           ,[i_Cve_DivisionMiEmpresa]" & _
                          "           ,[t_VersionCFDi]" & _
                          "           ,[i_Cve_EstatusLiquidacion]) " & _
                          "     VALUES " & _
                          "           ('" & _documento.RutaDocumentoOrigen & "'" & _
                          "           ,1" & _
                          "           ," & _documento.ClaveTipoDocumento & "" & _
                          "           ,'" & _documento.FechaRegistro.ToString("dd/MM/yyyy H:mm:ss") & "'" & _
                          "           ," & _documento.StatusDocumento & "" & _
                          "           ,'" & _documento.NombreDocumento & "'" & _
                          "           ,'" & _documento.FolioDocumento & "'" & _
                          "           ," & _documento.ClaveRepositorioDigital & "" & _
                          "           ,'" & _documento.Documento & "'" & _
                          "           ," & _documento.ClaveDivisionMiEmpresa & "" & _
                          "           ,'" & _documento.VersionCFDI & "'" & _
                          "           , " & _documento.StatusLiquidacion & ")" & _
                          "  SELECT @@IDENTITY AS 'Clave'"

            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

            If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                If dataTable_.Rows.Count > 0 Then

                    For Each fila_ As DataRow In dataTable_.Rows

                        _documento.Clave = fila_.Item("Clave").ToString

                        consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                                     "INSERT INTO [dbo].[Ext012Documentos] " & _
                                     "           ([i_Cve_Documento]" & _
                                     "           ,[t_URLPublico]" & _
                                     "           ,[t_NombreDocumento] " & _
                                     "           ,[i_Cve_TipoArchivo] " & _
                                     "           ,[i_Cve_TipoDocumento]" & _
                                     "           ,[i_ConsultableCliente]" & _
                                     "           ,[f_Registro]" & _
                                     "           ,[i_Cve_EncPlantillaDocumento]" & _
                                     "           ,[i_Cve_RepositorioDigital]" & _
                                     "           ,[i_Cve_Directorio]" & _
                                     "           ,[i_Cve_Usuario]" & _
                                     "           ,[i_Cve_Estatus]" &
                                     "           ,[i_Cve_Estado]" &
                                     "           ,[i_Cve_DivisionMiEmpresa]) " & _
                                     "     VALUES " & _
                                     "           (" & _documento.Clave & "" & _
                                     "           ,'" & _documento.UrlPublico & "'" & _
                                     "           ,'" & _documento.NombreDocumento & "'" & _
                                     "           ," & _documento.TipoArchivo & "" & _
                                     "           ," & _documento.ClaveTipoDocumento & "" & _
                                     "           ," & _documento.DocumentoConsultableCliente & "" & _
                                     "           ,'" & _documento.FechaRegistro.ToString("dd/MM/yyyy H:mm:ss") & "'" & _
                                     "           ," & _documento.PlantillaDocumento.ClavePlantilla & "" & _
                                     "           ," & _documento.ClaveRepositorioDigital & "" & _
                                     "           ," & _documento.ClaveDirectorio & "" & _
                                     "           ," & _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario & "" & _
                                     "           ,1" & _
                                     "           ,1" & _
                                     "           ," & _documento.ClaveDivisionMiEmpresa & ")"

                        _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                    Next

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1005)

                End If

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

                ''Guardar en contingencia
                'If Not Directory.Exists(_documento.RutaDocumentoContingenciaCompleto) Then

                '    My.Computer.FileSystem.CreateDirectory(_documento.RutaDocumentoContingenciaCompleto)

                'End If

                'System.IO.File.Copy(_documento.RutaDocumentoOrigen,
                '                    _documento.RutaDocumentoContingenciaCompleto & "" & _documento.NombreDocumento)

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1008)

            End Try

        End Sub

        Private Sub InsertarRegistroVinculacion(Optional ByVal claveModulo_ As Integer = Nothing) Implements IOperacionesDocumento.InsertarRegistroVinculacion

            Dim campo_ As String = Nothing

            For contador_ As Integer = 1 To _documento.PlantillaDocumento.CaracteristicasDocumentos.Count

                If _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).ClaveCaracteristica = 49 Then 'Referencia

                    campo_ = _documento.PlantillaDocumento.CaracteristicasDocumentos(contador_).Valor

                    _documento.TipoVinculacion = gsol.documento.Documento.TiposVinculacion.Referencia

                    Exit For

                End If

            Next contador_

            Select Case _documento.TipoVinculacion

                Case gsol.documento.Documento.TiposVinculacion.NoDefinidia

                Case gsol.documento.Documento.TiposVinculacion.Referencia

                    _documento.VinculacionDocumentos.Modulo = "DocumentosMaestroOperaciones"

                    _documento.VinculacionDocumentos.CampoLLaveModulo = "i_Cve_MaestroOperaciones"

                    Dim dataTable_ As DataTable = Nothing

                    Dim consulta_ As String = Nothing

                    consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                                "select i_Cve_MaestroOperaciones from Reg009MaestroOperaciones where t_Referencia  = '" & campo_ & "' and i_Cve_DivisionMiEmpresa = " & _documento.ClaveDivisionMiEmpresa & "  and i_TipoReferencia = 1 and i_Cve_Estado = 1"

                    _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                    If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                        If dataTable_.Rows.Count > 0 Then

                            _documento.VinculacionDocumentos.ClaveModulo = dataTable_(0)(0).ToString

                            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " & _
                                          "INSERT INTO [dbo].[Vin012DocumentosMaestroOperaciones] " & _
                                          "           ([i_Cve_MaestroOperaciones]" & _
                                          "           ,[i_Cve_Documento]" & _
                                          "           ,[i_Cve_Usuario] " & _
                                          "           ,[i_Cve_Estatus]" & _
                                          "           ,[i_Cve_DivisionMiEmpresa]" & _
                                          "           ,[i_Cve_Estado])" & _
                                          "     VALUES " & _
                                          "           (" & _documento.VinculacionDocumentos.ClaveModulo & "" & _
                                          "           ," & _documento.Clave & "" & _
                                          "           ," & _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario & "" & _
                                          "           ,1" & _
                                          "           ," & _documento.ClaveDivisionMiEmpresa & _
                                          "           ,1) "

                            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                            If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1018)

                            End If

                        End If

                    End If

                Case gsol.documento.Documento.TiposVinculacion.Factura

            End Select

        End Sub

        Public Sub InsertarRegistrosCaracteristicas() Implements IOperacionesDocumento.InsertarRegistrosCaracteristicas

            Dim consulta_ As String = Nothing

            consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED "

            For Each elemento_ As KeyValuePair(Of Integer, CaracteristicaDocumento) In _documento.PlantillaDocumento.CaracteristicasDocumentos

                Dim orden_ As Integer = elemento_.Key

                Dim caracteristicaDocumento_ As CaracteristicaDocumento = elemento_.Value

                consulta_ = consulta_ & _
                              "INSERT INTO [dbo].[Vin012CaracteristicasExtDocumentos] " & _
                              "           ([i_Cve_CaracteristicaDocumento]" & _
                              "           ,[i_Cve_Documento]" & _
                              "           ,[i_ValorNumero]" & _
                              "           ,[i_ValorDecimal]" & _
                              "           ,[t_ValorTexto]" & _
                              "           ,[f_ValorFecha]" & _
                              "           ,[f_ValorHora]" & _
                              "           ,[f_ValorFechaHora]" & _
                              "           ,[i_ValorClaveCatalogo]" & _
                              "           ,[t_ValorCatalogo]" & _
                              "           ,[i_Cve_Estatus]" & _
                              "           ,[i_Cve_Estado]" & _
                              "           ,[i_Cve_DivisionMiEmpresa]) " & _
                              "     VALUES " & _
                              "           (" & caracteristicaDocumento_.ClaveCaracteristica & "" & _
                              "           ," & _documento.Clave & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.Valor), "" & caracteristicaDocumento_.Valor & "", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.Valor), "" & caracteristicaDocumento_.Valor & "", "null") & "" & _
                              "           ," & IIf((DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Texto, caracteristicaDocumento_) Or DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso, caracteristicaDocumento_)) And Not IsNothing(caracteristicaDocumento_.Valor), "'" & Replace(caracteristicaDocumento_.Valor, "'", "''") & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.Valor), "'" & caracteristicaDocumento_.Valor & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Hora, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.Valor), "'" & caracteristicaDocumento_.Valor & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.Valor), "'" & caracteristicaDocumento_.Valor & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorClaveCatalogo), "'" & caracteristicaDocumento_.ValorClaveCatalogo & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorCatalogo), "'" & caracteristicaDocumento_.ValorCatalogo & "'", "null") & "" & _
                              "           ,1 " &
                              "           ,1 " & _
                              "           , " & _documento.ClaveDivisionMiEmpresa & ")"

                If caracteristicaDocumento_.ClaveCaracteristicaSecundaria > 0 Then

                    consulta_ = consulta_ & _
                              "INSERT INTO [dbo].[Vin012CaracteristicasExtDocumentos] " & _
                              "           ([i_Cve_CaracteristicaDocumento]" & _
                              "           ,[i_Cve_Documento]" & _
                              "           ,[i_ValorNumero]" & _
                              "           ,[i_ValorDecimal]" & _
                              "           ,[t_ValorTexto]" & _
                              "           ,[f_ValorFecha]" & _
                              "           ,[f_ValorHora]" & _
                              "           ,[f_ValorFechaHora]" & _
                              "           ,[i_ValorClaveCatalogo]" & _
                              "           ,[t_ValorCatalogo]" & _
                              "           ,[i_Cve_Estatus]" & _
                              "           ,[i_Cve_Estado]" & _
                              "           ,[i_Cve_DivisionMiEmpresa]) " & _
                              "     VALUES " & _
                              "           (" & caracteristicaDocumento_.ClaveCaracteristicaSecundaria & "" & _
                              "           ," & _documento.Clave & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoEntero, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorFin), "" & caracteristicaDocumento_.ValorFin & "", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.NumericoDecimal, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorFin), "" & caracteristicaDocumento_.ValorFin & "", "null") & "" & _
                              "           ," & IIf((DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Texto, caracteristicaDocumento_) Or DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.VerdaderoFalso, caracteristicaDocumento_)) And Not IsNothing(caracteristicaDocumento_.ValorFin), "'" & Replace(caracteristicaDocumento_.ValorFin, "'", "''") & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Fecha, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorFin), "'" & caracteristicaDocumento_.ValorFin & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Hora, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorFin), "'" & caracteristicaDocumento_.ValorFin & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.FechaHora, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorFin), "'" & caracteristicaDocumento_.ValorFin & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorClaveCatalogoFin), "'" & caracteristicaDocumento_.ValorClaveCatalogoFin & "'", "null") & "" & _
                              "           ," & IIf(DeterminarValor(CaracteristicaDocumento.TiposDatosCaracteristicas.Catalogo, caracteristicaDocumento_) And Not IsNothing(caracteristicaDocumento_.ValorCatalogoFin), "'" & caracteristicaDocumento_.ValorCatalogoFin & "'", "null") & "" & _
                              "           ,1 " & _
                              "           ,1 " & _
                              "           , " & _documento.ClaveDivisionMiEmpresa & ")"

                End If

            Next

            consulta_ = consulta_ & _
                                   "INSERT INTO [dbo].[Ext012CaracteristicasExtDocumentos]" & _
                                   "           ([i_Cve_VinCaracteristicasExtDocumentos]" & _
                                   "           ,[i_Cve_Documento]" & _
                                   "           ,[t_NombreDocumento]" & _
                                   "           ,[t_URLPublico]" & _
                                   "           ,[i_Cve_DivisionMiEmpresa]" & _
                                   "           ,[i_Cve_CaracteristicaDocumento]" & _
                                   "           ,[t_Titulo]" & _
                                   "           ,[t_Valor]" & _
                                   "           ,[i_Orden]" & _
                                   "           ,[i_Cve_FormatoCaracteristica]" & _
                                   "           ,[f_Registro]" & _
                                   "           ,[i_Cve_RepositorioDigital]" & _
                                   "           ,[i_cve_EncPlantillaDocumento]" & _
                                   "           ,[i_Cve_TipoArchivo]" & _
                                   "           ,[i_Cve_Estatus]" & _
                                   "           ,[i_Cve_Estado]" & _
                                   "           ,[t_RutaDocumento]" & _
                                   "           ,[t_RutaDocumentoContingencia]" & _
                                   "           ,[t_DisplayField]" & _
                                   "           ,[t_KeyField]" & _
                                   "           ,[t_NameAsKey]" & _
                                   "           ,[t_PermissionNumber]" & _
                                   "           ,[i_Cve_TipoDocumento]" & _
                                   "           ,[t_EjecutivoRegistro])" & _
                                   "select * " & _
                                   "from VT012MaestroDocumentosLista " & _
                                   "where i_Cve_Documento = " & _documento.Clave

            _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

            If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                _estatus.SetError(TagWatcher.ErrorTypes.C6_012_1019)

            End If

        End Sub

        Public Sub BuscarDocumento(Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.BuscarDocumento

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Public Sub BuscarPrivilegioUsuario(Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.BuscarPrivilegioUsuario

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Public Sub CargarCaracteristicasConInformacion() Implements IOperacionesDocumento.CargarCaracteristicasConInformacion

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)


        End Sub

        Public Sub CargarCaracteristicasVisorMaestroDocumentos() Implements IOperacionesDocumento.CargarCaracteristicasVisorMaestroDocumentos

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Public Sub ModificarDocumento(Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.ModificarDocumento

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Private Sub ModificarRegistros() Implements IOperacionesDocumento.ModificarRegistros

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Private Sub ModificarRegistrosCaracteristicas() Implements IOperacionesDocumento.ModificarRegistrosCaracteristicas

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Public Sub EliminarDocumento(Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.EliminarDocumento

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

        Public Sub BuscarPrivilegioUsuario(ByVal ClaveDocumento_ As Integer, Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing) Implements IOperacionesDocumento.BuscarPrivilegioUsuario

            _estatus.SetOK()

            _estatus.SetError(TagWatcher.ErrorTypes.C1_000_0001)

        End Sub

#End Region

#Region "Funciones"

        Private Function DeterminarValor(ByVal tipoDatosEsperado_ As CaracteristicaDocumento.TiposDatosCaracteristicas,
                                         ByVal caracteristiaDocumento_ As CaracteristicaDocumento) As Boolean

            If caracteristiaDocumento_.TipoDatoCaracteristica = tipoDatosEsperado_ Then

                Return True

            Else

                Return False

            End If

        End Function

        'Public Function TraducirCaracteristicas(ByVal bytesInput_ As Byte()) As String

        '    Dim md5_ = System.Security.Cryptography.MD5.Create()

        '    Dim hashValue_ = _
        '        md5_.ComputeHash(System.Text.Encoding.ASCII.GetBytes(""))


        '    Dim token_ As New StringBuilder(hashValue_.Length * 2)

        '    For Each b As Byte In bytesInput_

        '        token_.Append(Conversion.Hex(b))

        '    Next

        '    Return token_.ToString()

        'End Function

#End Region


    End Class

End Namespace

