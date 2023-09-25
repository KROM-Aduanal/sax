Imports Gsol.BaseDatos.Operaciones
Imports Wma.Exceptions
Imports Gsol.BaseDatos

'Faltan las politicas para saber si esa auditoría se cumple o no para poder registrar la auditoría
'Faltan las notificaciones
'Falta ver lo de las reglas (revisar si es necesario o mejor se hace con las políticas)
'Revisar que es lo de auditorías externas, según yo era para la administración cuando se guardaban en el sysExperto como una situación de tráfico, pero eso ya no sucedera
'hay que ver si tienen otra utilidad

Public Class AdministracionAuditorias

#Region "Atributos"

    'Private i_Cve_DivisionMiEmpresa As Integer

    Private _coleccionAuditoriasPrevias As List(Of AuditoriasPrevias)

    'Private _ioperacionesCatalogoAuditoria As IOperacionesCatalogo

    Private _sistema As Organismo

    Private _mensaje As String

    'Private _tagWatcher As New TagWatcher

    Enum enmTipoDato
        Entero = 0
        Cadena
        Decimal_
        Boleano
        Fecha
    End Enum

    Private Enum enmModulo
        AuditoriasPrevias = 0
        RegistroAuditorias
        CatalogoAuditorias
    End Enum

    Enum EstatusAuditorias
        Cancelado = 0   'El azulito bajo
        Aprobado = 1    'El verde
        Rechazado = 2   'El rojo
        Pendiente = 3   'El amarillo
    End Enum

#End Region

#Region "Propiedades"

    Public ReadOnly Property AuditoriasPrevias As List(Of AuditoriasPrevias)
        Get
            Return _coleccionAuditoriasPrevias
        End Get
    End Property

    Public ReadOnly Property MensajeError As String
        Get
            Return _mensaje
        End Get
    End Property
    'Public Property DivisionMiEmpresa As Integer
    '    Get
    '        DivisionMiEmpresa = i_Cve_DivisionMiEmpresa
    '    End Get
    '    Set(value As Integer)
    '        i_Cve_DivisionMiEmpresa = value
    '    End Set
    'End Property

    'ReadOnly Property GetTagWatcher As TagWatcher

    '    Get

    '        Return _tagWatcher

    '    End Get

    'End Property

#End Region

#Region "Constructores"

    Public Sub New()

        _sistema = New Organismo

        _coleccionAuditoriasPrevias = New List(Of AuditoriasPrevias)

        '_ioperacionesCatalogoAuditoria = New OperacionesCatalogo

        '_ioperacionesCatalogoAuditoria = ioperacionescatalogo_

        '_tagWatcher.Status = TagWatcher.TypeStatus.Empty

        'i_Cve_DivisionMiEmpresa = i_Cve_DivisionMiEmpresa_

    End Sub

#End Region

#Region "Metodos Publicos"

    ''' <summary>
    ''' Revisa si tiene ya registradas las auditorías previas
    ''' En _resultsCont regresara cuantas auditorias previas debería de tener según la configuración
    ''' En _flag regresa true si tiene todas las auditorias previas requeridas, false si no tiene todas las auditorías previas requeridas
    ''' En ObjectReturned regresa las auditorías en la cual se puede saber cuales tiene y cuales faltan
    ''' </summary>
    ''' <param name="ioperacionescatalogo_"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TieneAuditoriasPrevias(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                            ByVal nombreCortoAuditoria_ As String,
                                            ByVal i_Cve_MaestroOperaciones_ As Integer, _
                                            ByVal i_Cve_DivisionMiEmpresa_ As Integer, _
                                            ByVal caracteristicasPoliticasAuditoriasPrevias_ As List(Of CaracteristicaCatalogo)) As TagWatcher

        Dim tagWatcherLocal_ = New TagWatcher

        If RequiereAuditoriasPrevias(ioperacionescatalogo_, nombreCortoAuditoria_, i_Cve_DivisionMiEmpresa_).FlagReturned = 1 Then
            'Falta revisar tema de políticas de auditorías previas
            If RevisaTieneAuditoriasPrevias(ioperacionescatalogo_, _
                                            nombreCortoAuditoria_, _
                                            i_Cve_MaestroOperaciones_, _
                                            i_Cve_DivisionMiEmpresa_, _
                                            caracteristicasPoliticasAuditoriasPrevias_) Then

                tagWatcherLocal_.SetOK(_coleccionAuditoriasPrevias.Count, "1") 'Si tiene las auditoras previas que requiere

            Else

                tagWatcherLocal_.SetOK(_coleccionAuditoriasPrevias.Count, "0") 'No tiene las auditorías previas que requiere

            End If

            tagWatcherLocal_.ObjectReturned = _coleccionAuditoriasPrevias 'Se envían las auditorías previas requeridas con su dato de cual tiene y cual falta

        Else

            tagWatcherLocal_.SetOK(0, "1") 'No requiere auditorías previas

        End If

        Return tagWatcherLocal_

    End Function

    'Se recomienda usar TieneAuditoriasPrevias ese te dice cuales ya tiene y cuales no
    Public Function RequiereAuditoriasPrevias(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                               ByVal nombreCortoAuditoria_ As String, _
                                            ByVal i_Cve_DivisionMiEmpresa_ As Integer) As TagWatcher

        Dim tagWatcherLocal_ = New TagWatcher

        AuditoriasPreviasRequeridas(ioperacionescatalogo_, nombreCortoAuditoria_, i_Cve_DivisionMiEmpresa_)

        If _coleccionAuditoriasPrevias.Count < 1 Then

            tagWatcherLocal_.SetOK(0, 0) 'No requiere auditorías previas

        Else

            tagWatcherLocal_.ObjectReturned = _coleccionAuditoriasPrevias

            tagWatcherLocal_.SetOK(_coleccionAuditoriasPrevias.Count, 1) 'Requiere auditorías previas

        End If

        Return tagWatcherLocal_

    End Function

    'Revisar si ya tiene esa auditoría la referencia (viva)
    Public Function ExisteAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                    ByVal nombreCortoAuditoria_ As String, _
                                    ByVal i_Cve_MaestroOperaciones_ As Integer, _
                                    ByVal i_Cve_DivisionMiEmpresa_ As Integer) As Boolean

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim listaClausulasModulos_ As New List(Of CamposClausulasLibres)

        Dim objClausulasModulos_ As New CamposClausulasLibres("t_NombreCorto", "=", nombreCortoAuditoria_, enmTipoDato.Cadena)

        listaClausulasModulos_.Add(objClausulasModulos_)

        Dim objClausulasMaestroOperaciones_ As New CamposClausulasLibres("i_Cve_MaestroOperaciones", "=", i_Cve_MaestroOperaciones_, enmTipoDato.Entero)

        listaClausulasModulos_.Add(objClausulasMaestroOperaciones_)

        Dim objClausulasDivisionMiEmpresa_ As New CamposClausulasLibres("i_Cve_DivisionMiEmpresa", "=", i_Cve_DivisionMiEmpresa_, enmTipoDato.Entero)

        listaClausulasModulos_.Add(objClausulasDivisionMiEmpresa_)

        Dim objClausulasEstatus_ As New CamposClausulasLibres("i_Cve_Estatus", "=", 1, enmTipoDato.Entero) 'Que estén vivas        

        listaClausulasModulos_.Add(objClausulasEstatus_)

        ioperacionesLocal_ = ConsultaInformacionModulo(ioperacionescatalogo_, enmModulo.RegistroAuditorias, listaClausulasModulos_)

        If Not ioperacionesLocal_ Is Nothing Then

            If ioperacionesLocal_.Vista(0, "i_Cve_Estatus", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) = 1 Then

                Return True

            End If

        End If

        Return False

    End Function

    'Guardar en las auditorías
    'En la propiedad Mensaje se pueden ver los mesajes de error
    'En caso de que faltan auditorías previas, an la propiedad AuditoriasPrevias se pueden ver las que tiene y las que faltan y las que se requieren según las politicas
    'o si hubo un error (mensaje) en las auditorías previas como falta de parametros u otro error de consulta
    Public Function AsignarAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                     ByVal nombreCortoAuditoria_ As String, _
                                     ByVal i_Cve_MaestroOperaciones_ As Integer, _
                                     ByVal i_Cve_DivisionMiEmpresa_ As Integer,
                                     ByVal caracteristicasPoliticasAuditoriasPrevias_ As List(Of CaracteristicaCatalogo),
                                     Optional ByVal estatusAuditoria_ As EstatusAuditorias = EstatusAuditorias.Aprobado) As Boolean

        'Dim tagWatcherLocal_ = New TagWatcher
        _mensaje = Nothing

        If ExisteAuditoria(ioperacionescatalogo_, nombreCortoAuditoria_, i_Cve_MaestroOperaciones_, i_Cve_DivisionMiEmpresa_) Then

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Errors

            'tagWatcherLocal_.SetError(TagWatcher.ErrorTypes.C6_034_0001)
            '_sistema.GsDialogo("Ya existe un registro para esta auditoría", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            _mensaje = "Ya existe un registro para esta auditoría. "

            Return False

        End If

        Dim tagWatcherLocal_ = New TagWatcher

        tagWatcherLocal_ = TieneAuditoriasPrevias(ioperacionescatalogo_, nombreCortoAuditoria_, i_Cve_MaestroOperaciones_, i_Cve_DivisionMiEmpresa_, caracteristicasPoliticasAuditoriasPrevias_)

        If tagWatcherLocal_.FlagReturned = 0 Then

            _mensaje = "Faltan Auditorías previas. "

            Return False

        End If

        Dim claveCatalogoAuditoria As Integer = ConsultaClaveAuditoriaDesdeNombreCorto(ioperacionescatalogo_, nombreCortoAuditoria_)

        'Dim _operacionesAux = New OperacionesCatalogo
        Dim operacionesAux_ As IOperacionesCatalogo

        operacionesAux_ = _sistema.EnsamblaModulo("RegistroAuditorias")

        operacionesAux_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        operacionesAux_.PreparaCatalogo()

        operacionesAux_.CampoPorNombre("i_Cve_CatalogoAuditoria") = claveCatalogoAuditoria

        operacionesAux_.CampoPorNombre("i_Cve_MaestroOperaciones") = i_Cve_MaestroOperaciones_

        operacionesAux_.CampoPorNombre("f_FechaRegistro") = Date.Now.ToString("dd/MM/yy H:mm:ss.fff")

        operacionesAux_.CampoPorNombre("i_Cve_UsuarioRegistro") = ioperacionescatalogo_.EspacioTrabajo.MisCredenciales.ClaveUsuario

        operacionesAux_.CampoPorNombre("i_Cve_DivisionMiEmpresa") = i_Cve_DivisionMiEmpresa_

        operacionesAux_.CampoPorNombre("i_Cve_Estatus") = estatusAuditoria_

        operacionesAux_.CampoPorNombre("i_Cve_Estado") = "1"

        If operacionesAux_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

            Return True
            'tagWatcherLocal_.SetOK(0, operacionesAux_.ValorIndice.ToString)

        Else

            _mensaje = "No se ha podido registrar la auditoría. "

            '_sistema.GsDialogo("No se ha podido registrar la auditoría", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Errors

            'tagWatcherLocal_.SetError(TagWatcher.ErrorTypes.C6_029_0009)

            Return False

        End If

        'Return tagWatcherLocal_

    End Function

    Public Function CancelaAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                    ByVal nombreCortoAuditoria_ As String, _
                                    ByVal i_Cve_MaestroOperaciones_ As Integer, _
                                    ByVal i_Cve_DivisionMiEmpresa_ As Integer) As Boolean

        'Dim tagWatcherLocal_ = New TagWatcher

        _mensaje = Nothing

        Dim claveRegistroAuditoria_ As Integer = ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ioperacionescatalogo_, nombreCortoAuditoria_, i_Cve_MaestroOperaciones_, i_Cve_DivisionMiEmpresa_)

        If claveRegistroAuditoria_ = 0 Then

            _mensaje = "No existe auditoría para esa referencia. "

            '_sistema.GsDialogo("No existe auditoría para esa referencia", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return True

        End If

        If claveRegistroAuditoria_ = -1 Then

            _mensaje = "Existe mas de una auditoría viva para esta referencia, favor de verificar. "

            '_sistema.GsDialogo("Existe mas de una auditoría viva para esta referencia, favor de verificar", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        'Dim _operacionesAux = New OperacionesCatalogo
        Dim operacionesAux_ As IOperacionesCatalogo

        operacionesAux_ = _sistema.EnsamblaModulo("RegistroAuditorias")

        operacionesAux_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        operacionesAux_.PreparaCatalogo()

        operacionesAux_.CampoPorNombre("i_Cve_Estatus") = "0"

        operacionesAux_.CampoPorNombre("i_Cve_UsuarioCancelo") = ioperacionescatalogo_.EspacioTrabajo.MisCredenciales.ClaveUsuario

        operacionesAux_.CampoPorNombre("f_FechaCancelacion") = Date.Now.ToString("dd/MM/yy H:mm:ss.fff")

        If operacionesAux_.Modificar(claveRegistroAuditoria_) = IOperacionesCatalogo.EstadoOperacion.COk Then

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Ok
            Return True

        End If

        Return False

    End Function

    Public Function ConsultaClaveAuditoriaDesdeNombreCorto(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                  ByVal nombreCortoAuditoria_ As String) As Integer

        Dim claveCatalogo_ As Integer = -1

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim listaClausulasModulos_ As New List(Of CamposClausulasLibres)

        Dim objClausulasModulos_ As New CamposClausulasLibres("t_NombreCorto", "=", nombreCortoAuditoria_, enmTipoDato.Cadena)

        listaClausulasModulos_.Add(objClausulasModulos_)

        ioperacionesLocal_ = ConsultaInformacionModulo(ioperacionescatalogo_, enmModulo.CatalogoAuditorias, listaClausulasModulos_)

        If Not ioperacionesLocal_ Is Nothing Then

            claveCatalogo_ = ioperacionesLocal_.Vista(0, "i_Cve_CatalogoAuditoria", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        End If

        Return claveCatalogo_

    End Function

#End Region

#Region "Metodos Privados"


    'La division Mi empresa no se pide porque ya viene en la confituraciónd el módulo
    Private Sub AuditoriasPreviasRequeridas(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                            ByVal nombreCortoAuditoria_ As String, _
                                            ByVal i_Cve_DivisionMiEmpresa_ As Integer)

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim listaClausulasModulos_ As New List(Of CamposClausulasLibres)

        Dim objClausulasModulos_ As New CamposClausulasLibres("t_NombreCortoAuditoria", "=", nombreCortoAuditoria_, enmTipoDato.Cadena)

        listaClausulasModulos_.Add(objClausulasModulos_)

        Dim objClausulasDivisionMiEmpresa_ As New CamposClausulasLibres("i_Cve_DivisionMiEmpresa", "=", i_Cve_DivisionMiEmpresa_, enmTipoDato.Entero)

        listaClausulasModulos_.Add(objClausulasDivisionMiEmpresa_)


        ioperacionesLocal_ = ConsultaInformacionModulo(ioperacionescatalogo_, enmModulo.AuditoriasPrevias, listaClausulasModulos_)
        'Dim token_ As String = "AuditoriasPrevias"

        'Dim clausaConsulta_ As String = " and t_NombreCortoAuditoria = " & nombreCortoAuditoria

        'ioperacionesLocal_ = _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, token_, clausaConsulta_)


        _coleccionAuditoriasPrevias.Clear()

        If Not ioperacionesLocal_ Is Nothing Then

            For Each fila_ As DataRow In ioperacionesLocal_.Vista.Tables(0).Rows

                Dim AuditoriaPrevia_ As New AuditoriasPrevias

                AuditoriaPrevia_.i_Cve_Catalogo = fila_.Item("i_Cve_CatalogoAuditoria")

                If Not IsDBNull(fila_.Item("NombreCortoAuditoria")) Then
                    AuditoriaPrevia_.NombreCorto = fila_.Item("NombreCortoAuditoria")
                Else
                    AuditoriaPrevia_.NombreCorto = ""
                End If

                AuditoriaPrevia_.i_Cve_CatalogoAuditoriaPrevia = fila_.Item("i_Cve_CatalogoAuditoriaPreviaRequerida")

                If Not IsDBNull(fila_.Item("NombreCortoAuditoriaPrevia")) Then
                    AuditoriaPrevia_.NombreCortoAuditoriaPrevia = fila_.Item("NombreCortoAuditoriaPrevia")
                Else
                    AuditoriaPrevia_.NombreCortoAuditoriaPrevia = ""
                End If

                If Not IsDBNull(fila_.Item("i_Cve_Politica")) Then
                    AuditoriaPrevia_.i_Cve_Politica = fila_.Item("i_Cve_Politica")
                Else
                    AuditoriaPrevia_.i_Cve_Politica = 0
                End If

                _coleccionAuditoriasPrevias.Add(AuditoriaPrevia_)

            Next

        End If

    End Sub

    'Solo se llama cuando ya paso por AuditoriasPreviasRequeridas por eso es private y no public
    'Revisa si tiene o no tiene todas las auditorías previas requeridas según registro y en su caso según política
    Private Function RevisaTieneAuditoriasPrevias(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                  ByVal nombreCortoAuditoria_ As String, _
                                                  ByVal i_Cve_MaestroOperaciones_ As Integer, _
                                                  ByVal i_Cve_DivisionMiEmpresa_ As Integer, _
                                                  ByVal caracteristicasPoliticasAuditoriasPrevias_ As List(Of CaracteristicaCatalogo)) As Boolean
        'Solo se llama cuando ya paso por AuditoriasPreviasRequeridas por eso es private y no public
        'por eso se quitó esto de aquí para no tener una doble consulta
        'If _coleccionAuditoriasPrevias.Count < 1 Then

        '    AuditoriasPreviasRequeridas(ioperacionescatalogo_, nombreCortoAuditoria, i_Cve_DivisionMiEmpresa)

        'Else

        '    If _coleccionAuditoriasPrevias(0).NombreCorto <> nombreCortoAuditoria Then

        '        AuditoriasPreviasRequeridas(ioperacionescatalogo_, nombreCortoAuditoria, i_Cve_DivisionMiEmpresa)

        '    End If

        'End If

        'Si entra aqui es porque tiene auditorías previas
        For Each Auditoria_ As AuditoriasPrevias In _coleccionAuditoriasPrevias

            Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

            Dim listaClausulasModulos_ As New List(Of CamposClausulasLibres)

            Dim objClausulasModulos_ As New CamposClausulasLibres("t_NombreCorto", "=", Auditoria_.NombreCortoAuditoriaPrevia, enmTipoDato.Cadena)

            listaClausulasModulos_.Add(objClausulasModulos_)

            Dim objClausulasMaestroOperaciones_ As New CamposClausulasLibres("i_Cve_MaestroOperaciones", "=", i_Cve_MaestroOperaciones_, enmTipoDato.Entero)

            listaClausulasModulos_.Add(objClausulasMaestroOperaciones_)

            Dim objClausulasDivisionMiEmpresa_ As New CamposClausulasLibres("i_Cve_DivisionMiEmpresa", "=", i_Cve_DivisionMiEmpresa_, enmTipoDato.Entero)

            listaClausulasModulos_.Add(objClausulasDivisionMiEmpresa_)

            Dim objClausulasEstatus_ As New CamposClausulasLibres("i_Cve_Estatus", "=", 1, enmTipoDato.Entero) 'Que estén vivas

            listaClausulasModulos_.Add(objClausulasEstatus_)
            'Revisa si tiene registrad la auditoría
            ioperacionesLocal_ = ConsultaInformacionModulo(ioperacionescatalogo_, enmModulo.RegistroAuditorias, listaClausulasModulos_)

            Auditoria_.tieneRegistroAuditoriaViva = False

            If Not ioperacionesLocal_ Is Nothing Then

                If ioperacionesLocal_.Vista(0, "i_Cve_Estatus", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) = 1 Then

                    Auditoria_.tieneRegistroAuditoriaViva = True

                End If

            End If
            'No tiene auditoría viva, pero si tiene politica, hasta aqui no se hace el return, ya que la función RevisaRequiereAuditoriaPreviaSegunPolitica 
            'llena datos de la auditoría, abajo es en donde se validan las condiciones para saber si tiene o no tiene todas las auditorías previas requeridas
            If Auditoria_.tieneRegistroAuditoriaViva = False And Auditoria_.i_Cve_Politica > 0 Then

                RevisaRequiereAuditoriaPreviaSegunPolitica(ioperacionescatalogo_, Auditoria_, caracteristicasPoliticasAuditoriasPrevias_)

            End If

        Next
        'Si algúna no cumple se regresa false (No tiene auditoría que si se requiere)
        'Los datos como el mensaje ya se encuentra en las auditorías previas, claro en caso de que teng politica
        For Each Auditoria_ As AuditoriasPrevias In _coleccionAuditoriasPrevias

            'Si tiene auditoría viva
            If Auditoria_.tieneRegistroAuditoriaViva = True Then

                Continue For

            End If
            'Si no tiene auditoría ni politica, entonces si debe de tener auditoría pero no la tiene
            If Auditoria_.i_Cve_Politica = 0 Then

                Return False

            End If
            'Si no tiene auditoría pero si politica se revisa si se requiere la auditoría
            If Auditoria_.requiereAuditoriaSegunPolitica = True Then
                'Si la requiere, pero no la tuvo
                Return False

            End If
        Next

        Return True

    End Function

    Private Function ConsultaInformacionModulo(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                               ByVal modulo_ As enmModulo, _
                                               ByRef objClausulasModulo_ As List(Of CamposClausulasLibres)) As IOperacionesCatalogo

        Dim objInformacionTokenClausula As New ModuloClausulaLibreArmada

        RegresaDatosParaConsulta(modulo_, objClausulasModulo_, objInformacionTokenClausula)

        Return _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, objInformacionTokenClausula._token, objInformacionTokenClausula._clausula)

    End Function

    Private Sub RegresaDatosParaConsulta(ByVal modulo_ As enmModulo, _
                                              ByVal objClausulasModulo_ As List(Of CamposClausulasLibres), _
                                              ByRef objInformacionTokenClausula_ As ModuloClausulaLibreArmada)

        Select Case modulo_
            Case enmModulo.AuditoriasPrevias

                objInformacionTokenClausula_._token = "AuditoriasPrevias"

            Case enmModulo.RegistroAuditorias

                objInformacionTokenClausula_._token = "RegistroAuditorias"

            Case enmModulo.CatalogoAuditorias

                objInformacionTokenClausula_._token = "CatalogoAuditorias"

        End Select

        For Each clausula_ As CamposClausulasLibres In objClausulasModulo_

            objInformacionTokenClausula_._clausula = objInformacionTokenClausula_._clausula & _
                " and " & clausula_._nombreCampo & clausula_._comparacion & _
                IIf(clausula_._tipoDato = enmTipoDato.Cadena Or clausula_._tipoDato = enmTipoDato.Fecha, _
                    IIf(clausula_._comparacion.ToUpper = "LIKE", _
                        "'%" & clausula_._valorCampo.Replace(" ", "") & "%'", _
                        "'" & clausula_._valorCampo.Replace(" ", "") & "'"), _
                        clausula_._valorCampo)

        Next

    End Sub

    Private Function ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                                       ByVal nombreCortoAuditoria_ As String, _
                                                                       ByVal i_Cve_MaestroOperaciones_ As Integer, _
                                                                       ByVal i_Cve_DivisionMiEmpresa_ As Integer)

        Dim claveRegistro_ As Integer = 0

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim listaClausulasModulos_ As New List(Of CamposClausulasLibres)

        Dim objClausulasModulos_ As New CamposClausulasLibres("t_NombreCorto", "=", nombreCortoAuditoria_, enmTipoDato.Cadena)

        listaClausulasModulos_.Add(objClausulasModulos_)

        Dim objClausulasMaestroOperaciones_ As New CamposClausulasLibres("i_Cve_MaestroOperaciones", "=", i_Cve_MaestroOperaciones_, enmTipoDato.Entero)

        listaClausulasModulos_.Add(objClausulasMaestroOperaciones_)

        Dim objClausulasDivisionMiEmpresa_ As New CamposClausulasLibres("i_Cve_DivisionMiEmpresa", "=", i_Cve_DivisionMiEmpresa_, enmTipoDato.Entero)

        listaClausulasModulos_.Add(objClausulasDivisionMiEmpresa_)

        Dim objClausulasEstatus_ As New CamposClausulasLibres("i_Cve_Estatus", "=", 1, enmTipoDato.Entero) 'Que estén vivas

        listaClausulasModulos_.Add(objClausulasEstatus_)


        ioperacionesLocal_ = ConsultaInformacionModulo(ioperacionescatalogo_, enmModulo.RegistroAuditorias, listaClausulasModulos_)

        If Not ioperacionesLocal_ Is Nothing Then

            If ioperacionesLocal_.Vista.Tables(0).Rows.Count > 1 Then

                claveRegistro_ = -1

            Else

                claveRegistro_ = ioperacionesLocal_.Vista(0, "i_Cve_RegistroAuditoria", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

            End If

        End If

        Return claveRegistro_

    End Function
    'Revisa si es requerida la auditoría previa de una referencia
    'En la colección de características deberían de venir los parámetros necesarios para las politicas de las auditorias previas
    Private Sub RevisaRequiereAuditoriaPreviaSegunPolitica(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                  ByRef auditoriasPrevias_ As AuditoriasPrevias, _
                                                  ByVal caracteristicasPoliticasAuditoriasPrevias_ As List(Of CaracteristicaCatalogo))

        auditoriasPrevias_.requiereAuditoriaSegunPolitica = False

        auditoriasPrevias_.mensaje = Nothing

        ' Verifica política
        Dim tagWatcherLocal_ As TagWatcher

        Dim datosReferencia_ As DataTable = Nothing

        Dim _politicasBaseDatos As BaseDatos.PoliticasBaseDatos = _
            New PoliticasBaseDatos(ioperacionescatalogo_,
                                   auditoriasPrevias_.i_Cve_Politica,
                                   caracteristicasPoliticasAuditoriasPrevias_)

        tagWatcherLocal_ = _politicasBaseDatos.GetTagWatcher

        If tagWatcherLocal_.Status = TagWatcher.TypeStatus.Errors Then

            auditoriasPrevias_.requiereAuditoriaSegunPolitica = True

            '1 significa que si lo requiere

            Select Case True

                Case tagWatcherLocal_.Errors = TagWatcher.ErrorTypes.C1_000_0001

                    datosReferencia_ = DirectCast(tagWatcherLocal_.ObjectReturned, DataTable)

                    If Not datosReferencia_ Is Nothing Then

                        If datosReferencia_.Rows.Count > 0 Then

                            If Not datosReferencia_.Rows(0)("t_Valor") Is DBNull.Value Then

                                auditoriasPrevias_.mensaje = datosReferencia_.Rows(0)("t_Valor").ToString()

                            End If

                        End If

                    End If

                Case Else

                    'Error de la politica o de consulta (por ejemplo parametros incorrectos
                    auditoriasPrevias_.mensaje = tagWatcherLocal_.ErrorDescription

            End Select

        End If

    End Sub

    'Private Sub InicializaTagWatcher()

    '    _tagWatcher.Status = TagWatcher.TypeStatus.Empty

    '    _tagWatcher.ResultsCount = -1

    '    _tagWatcher.ErrorDescription = Nothing

    '    _tagWatcher.ObjectReturned = Nothing

    '    _tagWatcher.FlagReturned = Nothing

    'End Sub

#End Region


    'Realizar que l
    'Public Function RevisaAuditoria() As String

    'End Function
    'Faltaría revisar tema de:
    '-notificaciones 
    '-politicas 
    '-politicas de auditorías previas
    Private Function EnviaNotificaciones()

    End Function

    Public Function RevisaPoliticas() As Boolean

    End Function

End Class
