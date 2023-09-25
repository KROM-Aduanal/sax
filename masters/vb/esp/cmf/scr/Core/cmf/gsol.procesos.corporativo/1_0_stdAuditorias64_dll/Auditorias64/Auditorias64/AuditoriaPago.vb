Imports gsol.BaseDatos.Operaciones
Imports Wma.Exceptions
Imports gsol.Procesos.corporativo


Public Class AuditoriaPago
    Implements IAuditoriasVistosBuenos

#Region "Atributos"

    Private _sistema As Organismo

    Private _mensaje As String

#End Region

#Region "Propiedades"

    Public ReadOnly Property MensajeError As String Implements IAuditoriasVistosBuenos.MensajeError
        Get
            Return _mensaje
        End Get
    End Property

#End Region

#Region "Metodos Publicos"

    Public Function GuardarInformacion(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                   ByVal i_Cve_Referencia As Integer, _
                                   ByVal i_Cve_Sistema As Integer) As Boolean Implements IAuditoriasVistosBuenos.GuardarInformacion

        _mensaje = Nothing

        If ExisteAuditoria(ioperacionescatalogo_, i_Cve_Referencia, i_Cve_Sistema) Then

            _mensaje = "Ya existe un registro activo para esta auditoría. "

            Return False

        End If

        Dim InformacionTrafico_ As New OperacionesCatalogo

        InformacionTrafico_ = ConsultaInformacionTrafico(ioperacionescatalogo_, i_Cve_Referencia, i_Cve_Sistema, "VistoBuenoPagoTrafico")

        If InformacionTrafico_ Is Nothing Then

            _mensaje = "No se ha podido consultar la información de la referencia a guardar. "
            '_sistema.GsDialogo("No se ha podido consultar la información de la referencia a guardar", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        If GuardarInformacion(ioperacionescatalogo_, InformacionTrafico_) = False Then

            _mensaje = "No se ha podido guardar la información de la referencia. "

            '_sistema.GsDialogo("No se ha podido guardar la información de la referencia", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        Return True

    End Function

    Public Function CancelaAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                 ByVal i_Cve_Referencia_ As Integer, _
                                 ByVal i_Cve_Sistema_ As Integer, _
                                 ByVal t_Observaciones_ As String) As Boolean Implements IAuditoriasVistosBuenos.CancelaAuditoria

        _mensaje = Nothing

        Dim claveInfoAlmacenada_ As Integer = ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ioperacionescatalogo_, i_Cve_Referencia_, i_Cve_Sistema_)

        If claveInfoAlmacenada_ = 0 Then

            _mensaje = "No existe la auditoría para esa referencia. "

            '_sistema.GsDialogo("No existe la auditoría para esa referencia", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return True

        End If

        If claveInfoAlmacenada_ = -1 Then

            _mensaje = "Existe mas de una auditoría viva para esta referencia, favor de verificar. "

            '_sistema.GsDialogo("Existe mas de una auditoría viva para esta referencia, favor de verificar", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        'Dim _operacionesAux = New OperacionesCatalogo
        Dim operacionesAux_ As IOperacionesCatalogo

        operacionesAux_ = _sistema.EnsamblaModulo("VistoBuenoPagoInfoAlmacenada")

        operacionesAux_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        operacionesAux_.PreparaCatalogo()

        operacionesAux_.CampoPorNombre("i_Cve_UsuarioCancelo") = ioperacionescatalogo_.EspacioTrabajo.MisCredenciales.ClaveUsuario

        operacionesAux_.CampoPorNombre("f_FechaCancelacion") = Date.Now.ToString("dd/MM/yy H:mm:ss.fff")

        operacionesAux_.CampoPorNombre("i_Cve_Estatus") = "0"

        If operacionesAux_.Modificar(claveInfoAlmacenada_) = IOperacionesCatalogo.EstadoOperacion.COk Then

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Ok
            Return True

        End If

        Return False

    End Function


    Public Function CompararInformacion() As Boolean

        Return True

    End Function

    'Revisar si ya tiene esa auditoría la referencia (viva)
    Public Function ExisteAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                    ByVal i_Cve_Referencia As Integer, _
                                    ByVal i_Cve_Sistema As Integer) As Boolean Implements IAuditoriasVistosBuenos.ExisteAuditoria

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim claususalSQL_ As String

        claususalSQL_ = " and t_NombreCorto = 'VBP'" & _
            " and i_Cve_idReferencia = " & i_Cve_Referencia & _
            " and i_Cve_SistemaTrafico = " & i_Cve_Sistema & _
            " and i_Cve_Estatus = 1"

        ioperacionesLocal_ = _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, "VistoBuenoPagoInfoAlmacenada", claususalSQL_)

        If Not ioperacionesLocal_ Is Nothing Then

            Return True

        End If

        Return False

    End Function

#End Region

#Region "Metodos privados"

    Private Function ConsultaInformacionTrafico(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                ByVal i_Cve_Referencia As Integer, _
                                                ByVal i_Cve_Sistema As Integer, _
                                                ByVal modulo_ As String) As IOperacionesCatalogo Implements IAuditoriasVistosBuenos.ConsultaInformacionTrafico

        'Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo
        'Dim informacionTrafico_ As DataTable

        Dim claususalSQL_ As String

        claususalSQL_ = " and idReferencia = " & i_Cve_Referencia & " and i_Cve_SistemaTrafico = " & i_Cve_Sistema

        Return _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, modulo_, claususalSQL_)

    End Function

    Private Function GuardarInformacion(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                        ByVal datos_ As IOperacionesCatalogo) As Boolean Implements IAuditoriasVistosBuenos.GuardarInformacion



        'Dim guardado_ As Boolean = False
        'Dim tagWatcherLocal_ = New TagWatcher

        Dim operacionesAux_ As IOperacionesCatalogo

        Dim objAdministracionAuditoria_ As New AdministracionAuditorias

        operacionesAux_ = _sistema.EnsamblaModulo("VistoBuenoPagoInfoAlmacenada")

        operacionesAux_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        operacionesAux_.PreparaCatalogo()

        operacionesAux_.CampoPorNombre("i_Cve_SistemaTrafico") = datos_.Vista(0, "i_Cve_SistemaTrafico", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Cve_idReferencia") = datos_.Vista(0, "idReferencia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Referencia") = datos_.Vista(0, "Referencia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Aduana") = datos_.Vista(0, "Aduana", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Patente") = datos_.Vista(0, "Patente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Pedimento") = datos_.Vista(0, "Pedimento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_ImportadorExportadorClave") = datos_.Vista(0, "ClaveCliente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_ImportadorExportadorNombre") = datos_.Vista(0, "Cliente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_ClavePedimento") = datos_.Vista(0, "Clave", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Cve_CatalogoAuditoria") = objAdministracionAuditoria_.ConsultaClaveAuditoriaDesdeNombreCorto(ioperacionescatalogo_, "VBP")

        operacionesAux_.CampoPorNombre("i_Cve_Estatus") = "1"

        operacionesAux_.CampoPorNombre("i_Cve_UsuarioRegistro") = ioperacionescatalogo_.EspacioTrabajo.MisCredenciales.ClaveUsuario

        operacionesAux_.CampoPorNombre("f_FechaRegistro") = Date.Now.ToString("dd/MM/yy H:mm:ss.fff")

        operacionesAux_.CampoPorNombre("i_Cve_Estado") = "1"

        If operacionesAux_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

            Return True
        Else

            Return False
        End If


    End Function

    Private Function ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                                       ByVal i_Cve_Referencia As Integer, _
                                                                       ByVal i_Cve_Sistema As Integer) Implements IAuditoriasVistosBuenos.ConsultaClaveRegistroAuditoriaVivaDesdeReferencia

        Dim claveRegistro_ As Integer = 0

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim claususalSQL_ As String

        claususalSQL_ = " and t_NombreCorto = 'VBP'" & _
            " and i_Cve_idReferencia = " & i_Cve_Referencia & _
            " and i_Cve_SistemaTrafico = " & i_Cve_Sistema & _
            " and i_Cve_Estatus = 1"

        ioperacionesLocal_ = _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, "VistoBuenoPagoInfoAlmacenada", claususalSQL_)

        If Not ioperacionesLocal_ Is Nothing Then

            If ioperacionesLocal_.Vista.Tables(0).Rows.Count > 1 Then

                claveRegistro_ = -1

            Else

                claveRegistro_ = ioperacionesLocal_.Vista(0, "i_Cve_CamposVistosBuenos", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

            End If

        End If

        Return claveRegistro_

    End Function

#End Region

#Region "Constructores"

    Public Sub New()

        _sistema = New Organismo

    End Sub

#End Region

End Class
