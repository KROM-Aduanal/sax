Imports Syn.Documento
Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Wma.Exceptions.TagWatcher
Imports gsol.Web.Template.FormularioGeneralWeb.StatusMessage
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports gsol.Web
Imports gsol.Web.Components
Imports System.Web.UI
Imports System.Web
Imports Syn.Documento.Componentes.Campo
Imports Sax.Web.ControladorBackend
Imports Rec.Globals.Utils
Imports System.Web.UI.WebControls
Imports gsol.Web.Components.FormControl.ButtonbarModality
Imports System.Text.RegularExpressions
Imports gsol
Imports System.Xml.Serialization
Imports System.IO
Imports Syn.Operaciones
Imports Rec.Globals.Controllers

Public Class ControladorBackend
    Inherits Template.FormularioGeneralWeb

#Region "enums"
    Public Enum Datos
        SinDefinir = 0
        PaginaReciente = 1
        OficinaReciente = 2
        PerfilUsuario = 3
        SessionID = 4
        ModoEdicion = 5
    End Enum

    Public Enum Cookies

        MiSesion = 1
        MiCache = 2

    End Enum

    Enum TiposFiltro
        SinDefinir = 0
        PorDefecto = 1
        Avanzado = 2
    End Enum

    Enum TiposCaracteristica
        cUndefined = -1
        cInt32 = 0
        cString = 1
        cBoolean = 2
        cReal = 3
        cDateTime = 4
    End Enum

    Enum TiposLlave
        SinLlave = 0
        Primaria = 1
    End Enum

    Enum TiposVisible
        No = 0
        Si = 1
        Acarreo = 2
        Impresion = 3
        Virtual = 4
        Informacion = 5
    End Enum

    Enum TiposRigorDatos
        No = 0
        Si = 1
        Opcional = 2
    End Enum

    Enum TiposAsignacion
        Valor = 0 ' Valor
        ValorPresentacion = 1
        ValorFirma = 2
    End Enum

    Enum PropiedadesControl
        Auto = 0
        Valor = 1
        Text = 2
        OffText = 3
        OnText = 4
        Checked = 5
        Ninguno = 6
        ValueDetail = 7
        Signature = 8
    End Enum

    Public Enum TiposFlujo
        Entrada = 1
        Salida = 2
    End Enum

    Public Enum Modalidades
        Auto = 0
        Colectar = 1
        Procesar = 2
    End Enum

#End Region

#Region "Atributos"

    Private operacionNueva_ As Object

    Private operacionCopia_ As Object

    Private operacionActual_ As Object

    Private _caracteristicas As New List(Of Caracteristica)

    '******************* O P E R A C I O N   G E N E R I C A *************************
    'Private _operacionGenerica As OperacionGenerica

    '***********************  S t a t e m e n t s   S a x  ***************************
    Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

#End Region

#Region "Propiedades"

    Public Overridable ReadOnly Property IsPopup As Boolean

        Get

            Return If(Request.QueryString("is_popup") = "true", True, False)

        End Get

    End Property

    Public Property OperacionGenerica As OperacionGenerica
        Get
            'Return _operacionGenerica
            Return GetVars("_operacionGenerica")
        End Get
        Set(value As OperacionGenerica)
            '_operacionGenerica = value
            SetVars("_operacionGenerica", value)
        End Set
    End Property

    Public Property Statements As SaxStatements
        Get
            Return _statements
        End Get
        Set(value As SaxStatements)
            _statements = value
        End Set
    End Property

    Public Property OperacionNueva As Object
        Get
            Return operacionNueva_
        End Get
        Set(value As Object)
            operacionNueva_ = value
        End Set
    End Property

    Public Property OperacionCopia As Object
        Get
            Return operacionCopia_
        End Get
        Set(value As Object)
            operacionCopia_ = value
        End Set
    End Property

    Public Property OperacionActual As Object
        Get
            Return operacionActual_
        End Get
        Set(value As Object)
            operacionActual_ = value
        End Set
    End Property

    Private ReadOnly Property Formulario As FormControl
        Get
            Return DirectCast(Master.FindControl("contentBody").FindControl("__SYSTEM_MODULE_FORM"), FormControl)
        End Get
    End Property

    Public ReadOnly Property ListaEmpresas As SelectControl
        Get
            Return DirectCast(Master.FindControl("ContentCompanyList").FindControl("__SYSTEM_ENVIRONMENT"), SelectControl)
        End Get
    End Property

    Public ReadOnly Property Buscador As FindbarControl
        Get
            Return DirectCast(Master.FindControl("ContentFindbar").FindControl("__SYSTEM_CONTEXT_FINDER"), FindbarControl)
        End Get
    End Property

#End Region

#Region "Constructor"

    Protected Overridable Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Inicializa()

        ColocaEmpresas()

        InicializaControles()

        InicializaBotonera()

        InicializaCollectionViews()

    End Sub

    Protected Overridable Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'observar y validar bien este metodo es inestable
            LimpiarSessionesOperacion()
            'OperacionGenerica = Nothing
            'LimpiaSesion()

            ViewState("_PageID") = (New Random()).Next().ToString()
            'SetVars("ActivaControles", False) : ActivaControles((GetVars("ActivaControles", False)))

            'DataBind()

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "__serverObserver();", True)

        Else

            ActivaControles(GetVars("ActivaControles", False))

            InicializaTarjeteros()

        End If

    End Sub

    Protected Overridable Sub Page_Completed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        Formulario.Capped = PaginaMarcada(Request.RawUrl)

    End Sub

    Public Overridable Sub Inicializa()


    End Sub

    Public Function GetVars(ByVal var_ As String,
                        Optional ByVal defaultValue_ As Object = Nothing) As Object


        Dim pagevar_ = var_ & "_" & ViewState("_PageID")

        If Session(pagevar_) Is Nothing Then

            Session(pagevar_) = defaultValue_

        End If

        Return Session(pagevar_)

    End Function

    Public Sub SetVars(ByVal var_ As String,
                        Optional ByVal value_ As Object = Nothing)

        Dim pagevar_ = var_ & "_" & ViewState("_PageID")

        Session(pagevar_) = value_

    End Sub

#End Region

#Region "Metodos"

    Public Overridable Sub InicializaBotonera()

        If Formulario IsNot Nothing Then

            If Not Page.IsPostBack Then

                'If Not String.IsNullOrEmpty(ViewState("_PageID")) Then

                '    Formulario.Modality = GetVars("ModalidadBotonera")

                'Else

                '    Formulario.Modality = [Default]

                'End If

                Formulario.Modality = [Default]

            End If

        End If

    End Sub

    Private Sub InicializaTarjeteros()

        If Formulario IsNot Nothing Then

            For Each control_ As Object In DirectCast(Formulario.Controls(0), System.Web.UI.UpdatePanel).ContentTemplateContainer.Controls

                For indice_ As Int32 = 0 To control_.Controls.Count - 1

                    If control_.Controls(indice_).GetType().Name = "FieldsetControl" Then

                        For Each ccontrol_ As Object In DirectCast(control_.Controls(indice_), FieldsetControl).ListControls

                            If ccontrol_.GetType().Name = "PillboxControl" Then

                                If Not String.IsNullOrEmpty(ViewState("_PageID")) Then

                                    DirectCast(ccontrol_, PillboxControl).Modality = GetVars("ModalidadTarjetero_" & ccontrol_.ID, PillboxControl.ToolbarModality.Default)

                                Else

                                    DirectCast(ccontrol_, PillboxControl).Modality = PillboxControl.ToolbarModality.Default

                                End If

                            End If

                        Next

                    End If

                Next

            Next

        End If

    End Sub

    Private Sub InicializaCollectionViews()

        If Formulario IsNot Nothing Then

            For Each control_ As Object In DirectCast(Formulario.Controls(0), System.Web.UI.UpdatePanel).ContentTemplateContainer.Controls

                For indice_ As Int32 = 0 To control_.Controls.Count - 1

                    If control_.Controls(indice_).GetType().Name = "FieldsetControl" Then

                        For Each ccontrol_ As Object In DirectCast(control_.Controls(indice_), FieldsetControl).ListControls

                            If ccontrol_.GetType().Name = "CollectionViewControl" Then

                                DirectCast(ccontrol_, CollectionViewControl).DataSource = Session("DataCollectionView_" & ccontrol_.ID)

                                'If Not String.IsNullOrEmpty(ViewState("_PageID")) Then

                                '    DirectCast(ccontrol_, CollectionViewControl).DataSource = Session("DataCollectionView_" & ccontrol_.ID)

                                'Else

                                '    DirectCast(ccontrol_, CollectionViewControl).DataSource = Nothing

                                'End If

                            End If

                        Next

                    End If

                Next

            Next

        End If

    End Sub

    Private Sub InicializaControles()

        If Formulario IsNot Nothing Then

            If Not Page.IsPostBack Then

                'If Not String.IsNullOrEmpty(ViewState("_PageID")) Then

                '    ActivaControles(GetVars("ActivaControles", False))

                'Else

                '    ActivaControles(False)

                'End If

                ActivaControles(False)

            End If

        End If

    End Sub

    Public Sub PreparaBotonera(ByVal modality_ As [Enum])

        If Formulario IsNot Nothing Then

            SetVars("ModalidadBotonera", modality_)

            Formulario.Modality = GetVars("ModalidadBotonera")

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "__serverObserver();", True)

        End If

    End Sub

    Public Sub PreparaTarjetero(ByVal modality_ As [Enum], ByRef tarjetero_ As PillboxControl)

        If Formulario IsNot Nothing Then

            SetVars("ModalidadTarjetero_" & tarjetero_.ID, modality_)

            tarjetero_.Modality = GetVars("ModalidadTarjetero_" & tarjetero_.ID)

        End If

    End Sub

    Public Sub PreparaCollectionView(ByVal dataSource_ As Object, ByRef collectionview_ As CollectionViewControl)

        If Formulario IsNot Nothing Then

            Session("DataCollectionView_" & collectionview_.ID) = dataSource_

            collectionview_.DataSource = Session("DataCollectionView_" & collectionview_.ID)

        End If

    End Sub

    Public Sub ActivaControles(Optional ByVal activar_ As Boolean = True)

        If Formulario IsNot Nothing Then

            For Each fieldset_ As Object In DirectCast(Formulario.Controls(0), System.Web.UI.UpdatePanel).ContentTemplateContainer.Controls

                For indice_ As Int32 = 0 To fieldset_.Controls.Count - 1

                    If fieldset_.Controls(indice_).GetType().Name = "FieldsetControl" Then

                        For Each control_ As Object In DirectCast(fieldset_.Controls(indice_), FieldsetControl).ListControls

                            If control_.GetType = GetType(TabbarControl) Then

                                For Each childcontrol_ As Object In DirectCast(control_, TabbarControl).TabsSections

                                    childcontrol_.Enabled = activar_

                                Next

                            ElseIf control_.GetType = GetType(PillboxControl) Then

                                For Each childcontrol_ As Object In DirectCast(control_, PillboxControl).ListControls

                                    childcontrol_.Enabled = activar_

                                Next

                            ElseIf control_.GetType = GetType(CatalogControl) Then

                                Dim catalogcontrol_ = DirectCast(control_, CatalogControl)

                                With catalogcontrol_

                                    For Each childcontrol_ As Object In .Columns

                                        childcontrol_.Enabled = activar_

                                    Next

                                    .EnabledToolBar(activar_)

                                End With

                            ElseIf control_.GetType = GetType(FileControl) Then

                                DirectCast(control_, FileControl).EnabledButton(activar_)

                            Else

                                control_.Enabled = activar_

                            End If

                        Next

                    End If

                Next

            Next

        End If

    End Sub

    Private Function PaginaMarcada(ByVal pagina_ As String) As Boolean

        Dim cookie_ As HttpCookie = Preferencias(Cookies.MiSesion, Datos.PaginaReciente, crearSiNoExiste_:=False)

        If cookie_ IsNot Nothing Then

            Dim paginaMarcada_ As String = cookie_.Value

            If ("PaginaReciente=" & pagina_ = paginaMarcada_) Then

                Return True

            Else

                Return False

            End If

        Else

            Return False

        End If

    End Function

    Protected Sub MarcarPagina(sender As Object, e As EventArgs)

        If Formulario.Capped Then

            Preferencias(Cookies.MiSesion, Datos.PaginaReciente, crearSiNoExiste_:=True, Request.RawUrl)

        Else

            Preferencias(Cookies.MiSesion, eliminar_:=True)

        End If

    End Sub

    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        Dim cookie_ As HttpCookie

        If eliminar_ Then

            cookie_ = Request.Cookies(variableGlobal_.ToString)

            cookie_.Values.Set(variableGlobal_.ToString, "")

            cookie_.Values.Clear()

            cookie_.Values.Remove(variableGlobal_.ToString)

            cookie_.Expires = DateTime.Now.AddDays(-1)

            Response.Cookies.Set(cookie_)

            Session.Abandon()

            Return cookie_

        Else

            cookie_ = Request.Cookies(variableGlobal_.ToString)

            Return cookie_

        End If

    End Function

    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                          ByVal dato_ As Datos,
                                          ByVal crearSiNoExiste_ As Boolean,
                                          Optional ByVal valorAsignado_ As Object = Nothing) As HttpCookie
        Return PreferenciasUsuario(variableGlobal_, dato_, crearSiNoExiste_, valorAsignado_)


    End Function

    Function PreferenciasUsuario(ByVal variableGlobal_ As Cookies,
                                 ByVal dato_ As Datos,
                                 ByVal crearSiNoExiste_ As Boolean,
                                 Optional ByVal valorAsignado_ As Object = Nothing,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        Dim cookie_ As HttpCookie

        If eliminar_ Then

            Response.Cookies.Item(variableGlobal_.ToString).Value = Nothing

            Response.Cookies.Remove(variableGlobal_.ToString)

            Response.Cookies.Clear()

            Response.Cookies.Add(New HttpCookie(variableGlobal_.ToString, ""))

            Response.Cookies(variableGlobal_.ToString).Expires = DateTime.Now.AddDays(-1)

            Session.Abandon()

            Return Nothing

        End If

        cookie_ = Request.Cookies(variableGlobal_.ToString)

        If crearSiNoExiste_ Then

            If cookie_ Is Nothing Then
                'No existe
                cookie_ = New HttpCookie(variableGlobal_.ToString)

                cookie_.Values.Add(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                System.Web.HttpContext.Current.Response.AppendCookie(cookie_)

            Else
                'Existe Request.RawUrl
                cookie_ = Request.Cookies(variableGlobal_.ToString)

                cookie_.Values.Set(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                Response.Cookies.Set(cookie_)

            End If

        End If

        Return cookie_

    End Function

    Public Overridable Sub Destructores()

        operacionNueva_ = Nothing
        operacionCopia_ = Nothing
        operacionActual_ = Nothing

    End Sub

    Public Overridable Sub Constructores()

        'Nueva operación
        If Not GetVars("IsEditing", False) Then 'Nuevo

            'OperacionNueva = New ConstructorCliente(Convert.ToInt32(_empresa._idempresa),
            '                                 _empresa._idempresa,
            '                                 _empresa._idempresa,
            '                                 _empresa.razonsocial)

            OperacionNueva = Activator.CreateInstance(Buscador.DataObject.GetType)

        Else  'Edición

            'OperacionGenerica = Statements.ObjectSession

            OperacionCopia = Activator.CreateInstance(Buscador.DataObject.GetType, New Object() {True, OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone()})

            OperacionActual = Activator.CreateInstance(Buscador.DataObject.GetType, New Object() {True, OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente})

            'OperacionCopia = New ConstructorCliente(True,
            '                             OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone())

            'OperacionActual = New ConstructorCliente(True,
            '                                          OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)
        End If

    End Sub

    Public Overridable Function Guardar(ByVal object1_ As Object,
                                        Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Using enlaceDatos_ As IEnlaceDatos =
              New EnlaceDatos With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = object1_


                '----Operador automático------
                OperadorDatos(_entidadDatos, TiposFlujo.Entrada, Modalidades.Colectar)

                RealizarInsercion(_entidadDatos)

                OperadorDatos(_entidadDatos, TiposFlujo.Entrada, Modalidades.Procesar)

                DespuesOperadorDatosProcesar(_entidadDatos)

                '----Operador automático------

                Dim tagWatcher_ As TagWatcher = enlaceDatos_.AgregarDatos(_entidadDatos, Nothing, session_)

                If tagWatcher_.Status = Ok Then

                    DisplayMessage("Registro insertado ok", Success)

                    Return tagWatcher_

                Else

                    DisplayMessage("Ha ocurrido un error al guardar, intente de nuevo", Fail)

                    Return tagWatcher_

                End If

            End Using

        End Using

        Return Nothing

    End Function

    Public Overridable Function Guardar(Of T)(ByRef _operacionGenerica As OperacionGenerica,
                                              ByVal object1_ As Object,
                                              ByVal copia_ As IEntidadDatos,
                                              Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim respuesta_ As TagWatcher

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos _
              With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

            Using entidadDatos_ As IEntidadDatos = object1_

                '----Operador automático------
                OperadorDatos(entidadDatos_, TiposFlujo.Entrada, Modalidades.Colectar)

                RealizarModificacion(entidadDatos_)

                OperadorDatos(entidadDatos_, TiposFlujo.Entrada, Modalidades.Procesar)

                DespuesOperadorDatosProcesar(entidadDatos_)

                '----Operador automático------

                respuesta_ =
                        enlaceDatos_.ActualizarDocumento(Of T) _
                                                        (_operacionGenerica.Id,
                                                         enlaceDatos_,
                                                         copia_,
                                                         entidadDatos_,
                                                         session_).Result
                With respuesta_

                    Dim statusMessage_ As StatusMessage = Info

                    Select Case .Status

                        Case Ok, OkBut, OkInfo

                            If respuesta_.ObjectReturned IsNot Nothing Then
                                'CUANDO HAY CAMBIOS EN EL SERVIDOR
                                LimpiarCaracteristicasAutomaticas()

                                _operacionGenerica = .ObjectReturned


                                Dim documentoElectronico_ = _operacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone(Activator.CreateInstance(Of T))
                                '----Operador automático------
                                OperadorDatos(documentoElectronico_, TiposFlujo.Salida, Modalidades.Auto)
                                '----Operador automático------

                                'PreparaModificacion(_operacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone(Activator.CreateInstance(Of T)))
                                PreparaModificacion(documentoElectronico_)

                            End If

                        Case TypeStatus.Errors, Empty

                            statusMessage_ = Fail

                    End Select

                End With

            End Using

        End Using

        Return respuesta_

    End Function

    ' Delegado convariante para instrucciones adicionales del módulo
    Public Overridable Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

            '  ████████fin█████████       Logica de negocios local       ███████████████████████

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    ' Delegado convariante para instrucciones adicionales del módulo
    Public Overridable Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

            '  ████████fin█████████        Logica de negocios local      ███████████████████████

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overridable Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overridable Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overridable Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    Public Overridable Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    'no se esta usando, se llamante  esta comentado 
    Public Overridable Function ProcesarOperacion(Of T)() As TagWatcher

        Dim tagwatcher_ As TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim tipoMensaje_ As StatusMessage = StatusMessage.Info

        If Not GetVars("IsEditing", False) Then 'Nuevo

            Dim antesRealizarInsercion_ As Func(Of IClientSessionHandle, TagWatcher) =
                AddressOf AntesRealizarInsercion


            If antesRealizarInsercion_(Nothing).Status = TypeStatus.Ok Then

                Constructores()

                tagwatcher_ = Guardar(operacionNueva_, Nothing)

                If tagwatcher_.Status = Ok Then

                    OperacionGenerica = tagwatcher_.ObjectReturned ': Statements.ObjectSession = OperacionGenerica

                    SetVars("IsEditing", True)

                    If DespuesRealizarInsercion.Status = Ok Then

                        RefreshControls()

                        tipoMensaje_ = Success

                    Else

                        tipoMensaje_ = Fail

                    End If

                Else

                    tipoMensaje_ = Fail

                End If

            Else

                tipoMensaje_ = Fail

            End If

        Else

            Dim antesRealizarModificacion_ As Func(Of IClientSessionHandle, TagWatcher) =
                        AddressOf AntesRealizarModificacion

            If antesRealizarModificacion_(Nothing).Status = TypeStatus.Ok Then

                Constructores()

                If OperacionGenerica IsNot Nothing Then

                    Dim copia_ As IEntidadDatos = operacionCopia_

                    tagwatcher_ = Guardar(Of T) _
                                                 (OperacionGenerica,
                                                  operacionActual_,
                                                  copia_,
                                                  Nothing)

                    With tagwatcher_

                        If .Status = Ok Then

                            'ESTAMOS PROBANDO AQUI PORQUE DA SUS COSAS
                            'OperacionGenerica = .ObjectReturned ': Statements.ObjectSession = OperacionGenerica

                            If DespuesRealizarModificacion.Status = Ok Then

                                RefreshControls()

                                tipoMensaje_ = Success

                            Else

                                tipoMensaje_ = Fail

                            End If

                        ElseIf .Status = OkInfo Then

                            If DespuesRealizarModificacion.Status = Ok Then

                                RefreshControls()

                                tipoMensaje_ = Info

                            Else

                                tipoMensaje_ = Fail

                            End If

                        ElseIf .Status = OkBut Then


                            If DespuesRealizarModificacion.Status = Ok Then

                                RefreshControls()

                                tipoMensaje_ = Info

                            Else

                                tipoMensaje_ = Fail

                            End If

                        Else

                            tipoMensaje_ = Fail

                        End If

                    End With

                Else

                    tagwatcher_ = New TagWatcher

                    tipoMensaje_ = Fail : tagwatcher_.SetError(Me, "No se encontró la instancia del documento para edición")

                End If

            Else

                tipoMensaje_ = Fail

            End If

        End If

        DisplayMessage(tagwatcher_.LastMessage, tipoMensaje_)

        Return tagwatcher_

    End Function

    Public Overridable Async Function ProcesarTransaccion(Of T)() As Task(Of TagWatcher)

        Dim tagwatcher_ As TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim tipoMensaje_ As StatusMessage = StatusMessage.Info

        Using session_ = Await iEnlace_.GetMongoClient().StartSessionAsync().ConfigureAwait(False)

            session_.StartTransaction()

            Try

                If Not GetVars("IsEditing", False) Then 'Nuevo

                    Dim antesRealizarInsercion_ As Func(Of IClientSessionHandle, TagWatcher) =
                        AddressOf AntesRealizarInsercion


                    If antesRealizarInsercion_(session_).Status = TypeStatus.Ok Then

                        Constructores()

                        tagwatcher_ = Guardar(operacionNueva_, session_)

                        If tagwatcher_.Status = Ok Then

                            OperacionGenerica = tagwatcher_.ObjectReturned ': Statements.ObjectSession = OperacionGenerica

                            SetVars("IsEditing", True)

                            If DespuesRealizarInsercion.Status = Ok Then

                                RefreshControls()

                                tipoMensaje_ = Success : session_.CommitTransaction()

                            Else

                                tipoMensaje_ = Fail : session_.AbortTransaction()

                            End If

                        Else

                            tipoMensaje_ = Fail : session_.AbortTransaction()

                        End If

                    Else

                        tipoMensaje_ = Fail : session_.AbortTransaction()

                    End If

                Else

                    Dim antesRealizarModificacion_ As Func(Of IClientSessionHandle, TagWatcher) =
                        AddressOf AntesRealizarModificacion

                    If antesRealizarModificacion_(session_).Status = TypeStatus.Ok Then

                        Constructores()

                        If OperacionGenerica IsNot Nothing Then

                            Dim copia_ As IEntidadDatos = operacionCopia_

                            tagwatcher_ = Guardar(Of T) _
                                                 (OperacionGenerica,
                                                  operacionActual_,
                                                  copia_,
                                                  session_)

                            With tagwatcher_

                                If .Status = Ok Then
                                    'ESTAMOS PROBANDO AQUI PORQUE DA SUS COSAS
                                    'OperacionGenerica = .ObjectReturned ': Statements.ObjectSession = OperacionGenerica

                                    If DespuesRealizarModificacion.Status = Ok Then

                                        Dim notifytagwacher_ = NotificarSubscriptores(session_)

                                        If notifytagwacher_.Status = TypeStatus.Ok Then

                                            RefreshControls()

                                            tipoMensaje_ = Info : session_.CommitTransaction()

                                        Else

                                            tagwatcher_.LastMessage = notifytagwacher_.ErrorDescription

                                            tipoMensaje_ = Fail : session_.AbortTransaction()

                                        End If

                                    Else

                                        tipoMensaje_ = Fail : session_.AbortTransaction()

                                    End If

                                ElseIf .Status = OkInfo Then

                                    If DespuesRealizarModificacion.Status = Ok Then

                                        Dim notifytagwacher_ = NotificarSubscriptores(session_)

                                        If notifytagwacher_.Status = TypeStatus.Ok Then

                                            RefreshControls()

                                            tipoMensaje_ = Info : session_.CommitTransaction()

                                        Else

                                            tagwatcher_.LastMessage = notifytagwacher_.ErrorDescription

                                            tipoMensaje_ = Fail : session_.AbortTransaction()

                                        End If

                                    Else

                                        tipoMensaje_ = Fail : session_.AbortTransaction()

                                    End If

                                ElseIf .Status = OkBut Then

                                    If DespuesRealizarModificacion.Status = Ok Then

                                        RefreshControls()

                                        tipoMensaje_ = Info : session_.CommitTransaction()

                                    Else

                                        tipoMensaje_ = Fail : session_.AbortTransaction()

                                    End If

                                Else

                                    tipoMensaje_ = Fail

                                    session_.AbortTransaction()

                                End If

                            End With

                        Else

                            tagwatcher_ = New TagWatcher

                            tipoMensaje_ = Fail : tagwatcher_.SetError(Me, "No se encontró la instancia del documento para edición")

                            session_.AbortTransaction()

                        End If

                    Else

                        tipoMensaje_ = Fail : session_.AbortTransaction()

                    End If

                End If

                DisplayMessage(tagwatcher_.LastMessage, tipoMensaje_)

                Return tagwatcher_

            Catch ex As Exception

                DisplayMessage(ex.Message, Fail)

                session_.AbortTransaction()

            End Try

        End Using

        Return Nothing
        'edite nodos operaciones y no sql y constructor y enlace
    End Function

    Private Function NotificarSubscriptores(Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos _
              With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

            Return enlaceDatos_.NotificarSubscriptores(Buscador.DataObject.GetType.Name,
                                                       OperacionGenerica.Id,
                                                       OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                       session_)

        End Using

    End Function

    Public Overridable Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overridable Sub LimpiaSesion()

    End Sub

    Public Overridable Sub Limpiar()

    End Sub

    'Public Sub LimpiaVariablesSession()
    '    'HttpSessionStateBase
    '    For Each objeto_ As Object In Page.Session

    '        'objeto_.Abandon()
    '        'objeto_.Clear()

    '    Next
    '    'HttpSessionStateBase
    '    For Each objeto_ As Object In Page.Application.Contents

    '        'objeto_.Abandon()
    '        'objeto_.Clear()

    '    Next

    'End Sub

    Private Sub LimpiarSessionesOperacion()

        Dim sessions As New List(Of String)

        For Each objeto_ As String In Page.Session

            If objeto_.Contains("_operacionGenerica") Then

                sessions.Add(objeto_)

            End If

        Next

        If sessions.Count Then

            For Each session_ As String In sessions

                Session(session_) = Nothing

                Session.Remove(session_)

            Next

        End If

    End Sub

    Private Sub ReiniciarTarjeteros()

        Dim sessions As New List(Of String)

        For Each objeto_ As String In Page.Session

            If objeto_.Contains("ModalidadTarjetero_") Then

                sessions.Add(objeto_)

            End If

        Next

        If sessions.Count Then

            For Each session_ As String In sessions

                Session(session_) = PillboxControl.ToolbarModality.Default

            Next

        End If

    End Sub

    Public Sub LimpiarCaracteristicasAutomaticas()

        If _caracteristicas Is Nothing Or _caracteristicas.Count = 0 Then

            Configuracion()

        End If

        For Each caracteristica_ In _caracteristicas

            If caracteristica_.Control Is Nothing Then

                caracteristica_.Valor = Nothing

                Continue For

            End If

            Dim tipoControl_ As String = caracteristica_.Control.GetType.Name

            Select Case tipoControl_

                Case "SelectControl"

                    DirectCast(caracteristica_.Control, SelectControl).Value = Nothing

                Case "InputControl"

                    DirectCast(caracteristica_.Control, InputControl).Value = Nothing

                Case "SwitchControl"

                    DirectCast(caracteristica_.Control, SwitchControl).Value = Nothing
                    DirectCast(caracteristica_.Control, SwitchControl).Checked = False

                Case "FindboxControl"

                    DirectCast(caracteristica_.Control, FindboxControl).Value = Nothing
                    DirectCast(caracteristica_.Control, FindboxControl).Text = Nothing

                Case "CatalogControl"

                    DirectCast(caracteristica_.Control, CatalogControl).DataSource = Nothing

                Case "DualityBarControl"

                    DirectCast(caracteristica_.Control, DualityBarControl).Value = Nothing
                    DirectCast(caracteristica_.Control, DualityBarControl).ValueDetail = Nothing

                Case "PillboxControl"

                    DirectCast(caracteristica_.Control, PillboxControl).DataSource = Nothing

                Case "ListboxControl"

                    DirectCast(caracteristica_.Control, ListboxControl).Value = Nothing
                    DirectCast(caracteristica_.Control, ListboxControl).Text = Nothing

                Case "FileControl"

                    DirectCast(caracteristica_.Control, FileControl).Value = Nothing
                    DirectCast(caracteristica_.Control, FileControl).Text = Nothing

                Case Else

            End Select

        Next

        _caracteristicas.Clear()

        ' LimpiaVariablesSession()

        SetVars("isEditing", Nothing)

        LimpiaSesion()

        Limpiar()

    End Sub

    Public Sub LimpiarCaracteristicasAutomaticas(ByVal caracteristicas_ As List(Of Caracteristica))

        For Each caracteristica_ In caracteristicas_

            If caracteristica_.Control Is Nothing Then

                caracteristica_.Valor = Nothing

                Continue For

            End If

            Dim tipoControl_ As String = caracteristica_.Control.GetType.Name

            Select Case tipoControl_

                Case "SelectControl"

                    DirectCast(caracteristica_.Control, SelectControl).Value = Nothing

                Case "InputControl"

                    DirectCast(caracteristica_.Control, InputControl).Value = Nothing

                Case "SwitchControl"

                    DirectCast(caracteristica_.Control, SwitchControl).Value = Nothing
                    DirectCast(caracteristica_.Control, SwitchControl).Checked = False

                Case "FindboxControl"

                    DirectCast(caracteristica_.Control, FindboxControl).Value = Nothing
                    DirectCast(caracteristica_.Control, FindboxControl).Text = Nothing

                Case "CatalogControl"

                    DirectCast(caracteristica_.Control, CatalogControl).DataSource = Nothing

                Case "DualityBarControl"

                    DirectCast(caracteristica_.Control, DualityBarControl).Value = Nothing
                    DirectCast(caracteristica_.Control, DualityBarControl).ValueDetail = Nothing

                Case "PillboxControl"

                    DirectCast(caracteristica_.Control, PillboxControl).DataSource = Nothing

                Case "ListboxControl"

                    DirectCast(caracteristica_.Control, ListboxControl).Value = Nothing
                    DirectCast(caracteristica_.Control, ListboxControl).Text = Nothing

                Case "FileControl"

                    DirectCast(caracteristica_.Control, FileControl).Value = Nothing
                    DirectCast(caracteristica_.Control, FileControl).Text = Nothing

                Case Else

            End Select

        Next

    End Sub

    Public Overridable Sub BusquedaGeneral(ByVal sender As FindbarControl, ByVal e As EventArgs)

        LimpiarCaracteristicasAutomaticas()

        SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

        If String.IsNullOrEmpty(sender.Value) Then

            PreparaBotonera([Default])

            DespuesBuquedaGeneralSinDatos()

        Else

            Dim tagwatcher_ = BuscaDocumento(sender.Value)

            If tagwatcher_.ObjectReturned IsNot Nothing Then

                OperacionGenerica = tagwatcher_.ObjectReturned ': Statements.ObjectSession = OperacionGenerica

                If OperacionGenerica IsNot Nothing Then

                    SetVars("IsEditing", True)

                    PreparaBotonera(Closed)

                    DespuesBuquedaGeneralConDatos()

                Else

                    DespuesBuquedaGeneralSinDatos()

                End If

            Else

                SetVars("IsEditing", False)

                PreparaBotonera([Default])

            End If

        End If

    End Sub

    Public Overridable Sub DespuesBuquedaGeneralConDatos()

    End Sub
    Public Overridable Sub DespuesBuquedaGeneralSinDatos()

    End Sub

    Private Function BuscaDocumento(ByVal objectId_ As String) As TagWatcher

        Dim respuesta_ = New TagWatcher

        If objectId_ Is Nothing Or objectId_ = "" Or objectId_ = "-1" Then

            respuesta_.SetError(Me, "No se encontró el Id :'(")

            Return respuesta_

        End If

        Dim tipo_ As String = Buscador.DataObject.GetType.Name 'GetType(T).Name

        If Not String.IsNullOrEmpty(objectId_) Then

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

                Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(tipo_)

                Dim resultadoDocumentos_ As New List(Of OperacionGenerica)

                Dim filtro_ As BsonDocument = New BsonDocument().Add("_id", New ObjectId(objectId_))

                resultadoDocumentos_ = operacionesDB_.Find(filtro_).ToList

                If resultadoDocumentos_.Count Then

                    Dim operacionGenerica_ As OperacionGenerica = resultadoDocumentos_(0)

                    'OperacionGenerica = operacionGenerica_

                    Dim documentoElectronico_ = operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                    OperadorDatos(documentoElectronico_, TiposFlujo.Salida, Modalidades.Auto)

                    'PreparaModificacion(operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)
                    PreparaModificacion(documentoElectronico_)

                    respuesta_.ObjectReturned = operacionGenerica_

                    respuesta_.SetOK()

                Else

                    respuesta_.SetError(Me, TagWatcher.InfoTypes.G0_000_0000)

                    DisplayMessage("No se encontró ningún valor para esta consulta", Info)

                End If

            End Using

        End If

        Return respuesta_

    End Function

    Public Overridable Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Private Async Function FirmarDocumentoPublicar() As Task(Of TagWatcher)

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos _
                    With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

            Using session_ = Await enlaceDatos_.GetMongoClient().StartSessionAsync().ConfigureAwait(False)

                session_.StartTransaction()

                Dim tagwatcher_ As New TagWatcher

                Dim tipoMensaje_ As StatusMessage = StatusMessage.Info

                Dim espacioTrabajo_ = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

                If Buscador.DataObject.SubscriptionsGroup IsNot Nothing Then

                    If enlaceDatos_.EliminarSuscripciones(OperacionGenerica.Id,
                                                          Buscador.DataObject.SubscriptionsGroup,
                                                          session_).Status = TypeStatus.Ok Then

                        If enlaceDatos_.FirmarDocumento(Buscador.DataObject.GetType.Name,
                                                                   OperacionGenerica.Id,
                                                                   espacioTrabajo_.MisCredenciales.ClaveUsuario,
                                                                   session_).Status = TypeStatus.Ok Then

                            tipoMensaje_ = Info : tagwatcher_.SetOKInfo(Me, "El documento se ha firmado correctamente")

                            session_.CommitTransaction()

                        Else

                            tipoMensaje_ = Fail : tagwatcher_.SetError(Me, "No se pudo firmar el documento")

                            session_.AbortTransaction()

                        End If

                    Else

                        tipoMensaje_ = Fail : tagwatcher_.SetError(Me, "No se pudo eliminar las suscripciones el documento")

                        session_.AbortTransaction()

                    End If

                Else

                    If enlaceDatos_.FirmarDocumento(Buscador.DataObject.GetType.Name,
                                                                   OperacionGenerica.Id,
                                                                   espacioTrabajo_.MisCredenciales.ClaveUsuario,
                                                                   session_).Status = TypeStatus.Ok Then

                        tipoMensaje_ = Info : tagwatcher_.SetOKInfo(Me, "El documento se ha firmado correctamente")

                        session_.CommitTransaction()


                    Else

                        tipoMensaje_ = Fail : tagwatcher_.SetError(Me, "No se pudo firmar el documento")

                        session_.AbortTransaction()

                    End If

                End If

                DisplayMessage(IIf(tagwatcher_.Status = TypeStatus.OkInfo, tagwatcher_.LastMessage, tagwatcher_.ErrorDescription), tipoMensaje_)

                Return tagwatcher_

            End Using

        End Using

    End Function

    'EVENTOS DEL PANEL DE CONTROL PRINCIPAL (Nuevo, editar, borrar, otras acciones)
    Protected Sub EventosBotonera(ByVal sender_ As ButtonbarControl,
                                  ByVal e As EventArgs)

        Select Case sender_.IndexSelected

            Case 0 'Nuevo

                If OperacionGenerica IsNot Nothing Then

                    OperacionGenerica.Dispose()

                    'Statements.ObjectSession = Nothing
                    SetVars("_operacionGenerica", Nothing)

                End If

                LimpiarCaracteristicasAutomaticas()

                SetVars("IsEditing", False) : Formulario.IsEditing = GetVars("IsEditing")

                Buscador.Value = Nothing

                Buscador.Text = Nothing

                SetVars("ActivaControles", True) : ActivaControles(GetVars("ActivaControles"))

                PreparaBotonera(Open)

                BotoneraClicNuevo()

            Case 1 'Guardar

                'Dim a = Buscador.DataObject.GetType
                'ProcesarOperacion(Of Something)()
                'If Not ProcesarTransaccion(Of OperacionGenerica)().Status = TypeStatus.Errors Then : End If

                BotoneraClicGuardar()

            Case 2 'Publicar

                If FirmarDocumentoPublicar().Result.Status = TypeStatus.OkInfo Then

                    SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                    PreparaBotonera(Closed)

                    BotoneraClicPublicar()



                End If

            Case 3 'Seguir Editando

                SetVars("ActivaControles", True) : ActivaControles(GetVars("ActivaControles"))

                PreparaBotonera(Open)

                BotoneraClicEditar()

            Case 4 'Archivar

                BotoneraClicArchivar()

            Case 5 'Cancelar

                If Formulario.SaveConfirm = True Then

                    VerifyLeaveDocument()

                Else

                    ReiniciarTarjeteros()

                    SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                    If GetVars("IsEditing") = True Then

                        Dim documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                        OperadorDatos(documentoElectronico_, TiposFlujo.Salida, Modalidades.Auto)

                        PreparaBotonera(Closed)

                    Else

                        LimpiarCaracteristicasAutomaticas()

                        PreparaBotonera([Default])

                    End If

                End If

            Case 6 'Eliminar

                DisplayAlert("Eliminar Documento", "¿Esta seguro(a) que desea eliminar este documento?", "__DELETE_DOCUMENT", "Continuar", "Cancelar")

            Case Else 'Otras Acciones

                BotoneraClicOtros(sender_.IndexSelected)

        End Select

    End Sub

    Private Sub VerifyLeaveDocument()

        If GetVars("IsEditing") = False Then

            Dim hasChanges = 0

            Configuracion()

            For Each caracteristica_ In _caracteristicas

                If caracteristica_.Control Is Nothing Then

                    Continue For

                End If

                Dim tipoControl_ As String = caracteristica_.Control.GetType.Name

                Select Case tipoControl_

                    Case "SelectControl"

                        If Not String.IsNullOrEmpty(DirectCast(caracteristica_.Control, SelectControl).Value) Then

                            hasChanges += 1

                        End If

                    Case "InputControl"

                        If Not String.IsNullOrEmpty(DirectCast(caracteristica_.Control, InputControl).Value) Then

                            hasChanges += 1

                        End If

                    'Case "SwitchControl"

                    '    If DirectCast(caracteristica_.Control, SwitchControl).Checked = True Then

                    '        hasChanges += 1

                    '    End If

                    Case "FindboxControl"

                        If Not String.IsNullOrEmpty(DirectCast(caracteristica_.Control, FindboxControl).Value) Then

                            hasChanges += 1

                        End If

                    Case "CatalogControl"

                        If DirectCast(caracteristica_.Control, CatalogControl).DataSource IsNot Nothing Then

                            hasChanges += 1

                        End If

                    Case "DualityBarControl"

                        'ValueDetail
                        If Not String.IsNullOrEmpty(DirectCast(caracteristica_.Control, DualityBarControl).Value) Then

                            hasChanges += 1

                        End If

                    Case "PillboxControl"

                        If DirectCast(caracteristica_.Control, PillboxControl).DataSource IsNot Nothing Then

                            hasChanges += 1

                        End If

                    Case "ListboxControl"

                        If DirectCast(caracteristica_.Control, ListboxControl).Value IsNot Nothing Then

                            hasChanges += 1

                        End If

                    Case "FileControl"

                        If Not String.IsNullOrEmpty(DirectCast(caracteristica_.Control, FileControl).Value.ToString) Then

                            hasChanges += 1

                        End If
                    Case Else

                End Select

            Next

            If hasChanges > 0 Then

                DisplayAlert("Advertencia", "¿Esta seguro(a) que desea salir sin guardar los cambios realizados?", "__LEAVE_DOCUMENT", "Continuar", "Cancelar")

            Else

                ReiniciarTarjeteros()

                SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                LimpiarCaracteristicasAutomaticas()

                PreparaBotonera([Default])

            End If

        Else

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos _
                        With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

                Dim object1_ = Activator.CreateInstance(Buscador.DataObject.GetType, New Object() {True, OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone()})

                Dim object2_ = Activator.CreateInstance(Buscador.DataObject.GetType, New Object() {True, OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente})

                Using datosActuales_ As IEntidadDatos = object1_

                    OperadorDatos(datosActuales_, TiposFlujo.Entrada, Modalidades.Auto)

                    Dim datosCopia_ As IEntidadDatos = object2_

                    Dim listaActualizaciones_ As New List(Of DocumentoElectronicoObjetoActualizador)

                    listaActualizaciones_ = enlaceDatos_.AnalizaDiferencias(datosActuales_, datosCopia_)

                    If listaActualizaciones_.Count > 0 Then

                        DisplayAlert("Advertencia", "¿Esta seguro(a) que desea salir sin guardar los cambios realizados?", "__LEAVE_DOCUMENT", "Continuar", "Cancelar")

                    Else

                        ReiniciarTarjeteros()

                        SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                        Dim documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                        OperadorDatos(documentoElectronico_, TiposFlujo.Salida, Modalidades.Auto)

                        PreparaBotonera(Closed)

                    End If

                End Using

            End Using

        End If

    End Sub

    Public Overridable Sub BotoneraClicNuevo()

    End Sub

    Public Overridable Sub BotoneraClicGuardar()

    End Sub

    Public Overridable Sub BotoneraClicPublicar()

    End Sub

    Public Overridable Sub BotoneraClicEditar()

    End Sub

    Public Overridable Sub BotoneraClicArchivar()

    End Sub

    Public Overridable Sub BotoneraClicBorrar()

    End Sub

    Public Overridable Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

    End Sub

    Public Overridable Sub ColocaEmpresas()

        If Not Page.IsPostBack Then

            Dim environmentData_ = Sax.SaxStatements.GetInstance().GetEnvironments()

            Dim oficinas_ = New List(Of SelectOption)

            For Each kvp As KeyValuePair(Of String, String) In environmentData_

                oficinas_.Add(New SelectOption With {.Text = kvp.Value, .Value = kvp.Key})

            Next

            ListaEmpresas.DataSource = oficinas_

            ListaEmpresas.Value = Sax.SaxStatements.GetInstance().GetOfficeOnline()._id

        End If

    End Sub

    Protected Sub CambiarEmpresa(ByVal sender_ As SelectControl, ByVal e As EventArgs)

        Sax.SaxStatements.GetInstance().SetEnvironmentOnline(sender_.Value)

    End Sub

    Public Overridable Sub AceptaConfirmacion(argument_ As String)

    End Sub

    Public Overridable Sub RechazaConfirmacion(argument_ As String)

    End Sub

    Public Overrides Sub RechazaConfirmacionDialogo(argument_ As String)

        RechazaConfirmacion(argument_)

    End Sub

    Public Overrides Sub AceptaConfirmacionDialogo(argument_ As String)

        If argument_ = "__DELETE_DOCUMENT" Then

            If GetVars("IsEditing") = True Then

                'Dim resultado_ = BorrarDocumento(Statements.ObjectSession.Id.ToString())
                Dim resultado_ = BorrarDocumento(OperacionGenerica.Id.ToString())

                If resultado_.Status = TagWatcher.TypeStatus.Ok Then

                    If OperacionGenerica IsNot Nothing Then

                        OperacionGenerica.Dispose()

                        'Statements.ObjectSession = Nothing
                        SetVars("_operacionGenerica", Nothing)

                    End If

                    LimpiarCaracteristicasAutomaticas()

                    SetVars("IsEditing", False) : Formulario.IsEditing = GetVars("IsEditing")

                    'Lipiamos el contenido de la búsqueda
                    Buscador.Value = Nothing

                    Buscador.Text = Nothing

                    DisplayMessage("Elemento eliminado correctamente", StatusMessage.Success)

                    BotoneraClicBorrar()

                Else

                    DisplayMessage(resultado_.ErrorDescription, StatusMessage.Fail)

                End If

            End If

        ElseIf argument_ = "__LEAVE_DOCUMENT" Then

            ReiniciarTarjeteros()

            SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

            If GetVars("IsEditing") = True Then

                Dim documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                OperadorDatos(documentoElectronico_, TiposFlujo.Salida, Modalidades.Auto)

                PreparaBotonera(Closed)

            Else

                SetVars("ActivaControles", False) : ActivaControles(GetVars("ActivaControles"))

                LimpiarCaracteristicasAutomaticas()

                PreparaBotonera([Default])

            End If
        Else

            AceptaConfirmacion(argument_)

        End If

    End Sub

    Public Overridable Function BorrarDocumento(ByVal objectId_ As String) As TagWatcher

        Dim tagwatcher_ = New TagWatcher

        If objectId_ Is Nothing Or objectId_ = "" Or objectId_ = "-1" Then

            tagwatcher_.SetError(Me, "No se encontró el Id :'(")

            Return tagwatcher_

        End If

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

            Dim tipo_ As String = Buscador.DataObject.GetType.Name

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(tipo_)

            Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, New ObjectId(objectId_))

            Dim setStructureOfSubs_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.estado, 0)

            Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

            With tagwatcher_

                If result_.MatchedCount <> 0 Then

                    .SetOK()

                ElseIf result_.UpsertedId IsNot Nothing Then

                    .SetOK()

                Else

                    .SetError(Me, "No se generaron cambios")

                End If

            End With

            Return tagwatcher_

        End Using

    End Function

    Private Sub RefreshControls(Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

        If documentoElectronico_ Is Nothing Then
            'Statements.ObjectSession
            If OperacionGenerica IsNot Nothing Then
                'Statements.ObjectSession
                documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            End If


        End If

        Configuracion()

        For Each caracteristica_ As Caracteristica In _caracteristicas

            If caracteristica_.Control IsNot Nothing Then

                If caracteristica_.Control.GetType() = GetType(CatalogControl) Or
                    caracteristica_.Control.GetType() = GetType(PillboxControl) Or
                    caracteristica_.Control.GetType() = GetType(ListboxControl) Then

                    Dim check_ As TagWatcher =
                                    Out(caracteristica_, documentoElectronico_)

                End If

            End If

        Next

    End Sub

    'Public Sub Refresh(ByVal control_ As UIControl, Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

    '    If documentoElectronico_ Is Nothing Then
    '        'Statements.ObjectSession
    '        If OperacionGenerica IsNot Nothing Then
    '            'Statements.ObjectSession
    '            documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

    '        End If


    '    End If

    '    Configuracion()

    '    For Each caracteristica_ As Caracteristica In _caracteristicas

    '        If caracteristica_.Control IsNot Nothing Then

    '            If caracteristica_.Control.ID = control_.ID Then

    '                Dim check_ As TagWatcher =
    '                                Out(caracteristica_, documentoElectronico_)

    '            End If

    '        End If

    '    Next

    'End Sub


#End Region

#Region "Acciones"

    Public Function [Get](Of T)(campo_ As [Enum]) As Object

        If OperacionGenerica IsNot Nothing Then
            'Statements.ObjectSession
            Dim documentoElectronico_ As DocumentoElectronico = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Return documentoElectronico_.Attribute(Convert.ToInt32(campo_)).Valor

        End If

        Return Nothing

    End Function

    Private Function [In](ByVal valor_? As ObjectId,
                     ByRef documentoElectronico_ As DocumentoElectronico,
                     ByVal campo_ As [Enum],
                     ByVal asignacion_ As TiposAsignacion) As TagWatcher

        If valor_ Is Nothing Then : Return New TagWatcher(1) : End If

        Dim idUnico_ As Int32 = Convert.ToInt32(campo_)

        With documentoElectronico_

            Dim campoUnico_ As Object = .Attribute(idUnico_)

            If campoUnico_ IsNot Nothing Then

                Select Case campoUnico_.TipoDato

                    Case Componentes.Campo.TiposDato.IdObject

                        If asignacion_ = TiposAsignacion.Valor Then

                            .Attribute(idUnico_).Valor = valor_ : Return New TagWatcher(1)

                        Else

                            Return New TagWatcher(0, Me, "El tipo ObjectId no acepta valor de presentación")

                        End If

                    Case Else

                        Return New TagWatcher(0, Me, "El valor no es ObjectID")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró el campo [" & idUnico_ & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function [In](ByVal valor_? As Double,
                     ByRef documentoElectronico_ As DocumentoElectronico,
                     ByVal campo_ As [Enum],
                     ByVal asignacion_ As TiposAsignacion) As TagWatcher

        If valor_ Is Nothing Then : Return New TagWatcher(1) : End If

        Dim idUnico_ As Int32 = Convert.ToInt32(campo_)

        With documentoElectronico_

            Dim campoUnico_ As Object = .Attribute(idUnico_)

            If campoUnico_ IsNot Nothing Then

                Select Case campoUnico_.TipoDato

                    Case Componentes.Campo.TiposDato.Real

                        If asignacion_ = TiposAsignacion.Valor Then

                            .Attribute(idUnico_).Valor = valor_ : Return New TagWatcher(1)

                        Else

                            .Attribute(idUnico_).ValorPresentacion = valor_ : Return New TagWatcher(1)

                        End If

                    Case Else

                        Return New TagWatcher(0, Me, "El valor no es una cadena")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró el campo [" & idUnico_ & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function [In](ByVal valor_ As String,
                     ByRef documentoElectronico_ As DocumentoElectronico,
                     ByVal campo_ As [Enum],
                     ByVal asignacion_ As TiposAsignacion) As TagWatcher

        If valor_ Is Nothing Then : Return New TagWatcher(1) : End If

        Dim idUnico_ As Int32 = Convert.ToInt32(campo_)

        With documentoElectronico_

            Dim campoUnico_ As Object = .Attribute(idUnico_)

            If campoUnico_ IsNot Nothing Then

                Select Case campoUnico_.TipoDato

                    Case Componentes.Campo.TiposDato.Texto

                        If asignacion_ = TiposAsignacion.Valor Then

                            .Attribute(idUnico_).Valor = valor_ : Return New TagWatcher(1)

                        Else

                            .Attribute(idUnico_).ValorPresentacion = valor_ : Return New TagWatcher(1)

                        End If

                    Case Else

                        Return New TagWatcher(0, Me, "El valor no es una cadena")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró el campo [" & idUnico_ & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function [In](ByVal valor_ As Integer,
                     ByRef documentoElectronico_ As DocumentoElectronico,
                     ByVal campo_ As [Enum],
                     ByVal asignacion_ As TiposAsignacion) As TagWatcher

        Dim idUnico_ As Int32 = Convert.ToInt32(campo_)

        With documentoElectronico_

            Dim campoUnico_ As Object = .Attribute(idUnico_)

            If campoUnico_ IsNot Nothing Then

                Select Case campoUnico_.TipoDato

                    Case Componentes.Campo.TiposDato.Entero

                        If asignacion_ = TiposAsignacion.Valor Then

                            .Attribute(idUnico_).Valor = valor_ : Return New TagWatcher(1)

                        Else

                            .Attribute(idUnico_).ValorPresentacion = valor_ : Return New TagWatcher(1)

                        End If

                    Case Else

                        Return New TagWatcher(0, Me, "El valor no es un entero")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró el campo [" & idUnico_ & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function [In](ByVal valor_? As DateTime,
                     ByRef documentoElectronico_ As DocumentoElectronico,
                     ByVal campo_ As [Enum]) As TagWatcher

        If valor_ Is Nothing Then : Return New TagWatcher(1) : End If

        Dim idUnico_ As Int32 = Convert.ToInt32(campo_)

        With documentoElectronico_

            Dim campoUnico_ As Object = .Attribute(idUnico_)

            If campoUnico_ IsNot Nothing Then

                Select Case campoUnico_.TipoDato

                    Case Componentes.Campo.TiposDato.Fecha

                        .Attribute(idUnico_).Valor = Convert.ToDateTime(valor_).Date : Return New TagWatcher(1)

                    Case Else

                        Return New TagWatcher(0, Me, "El valor del campo [" & idUnico_ & "] no es una fecha")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró el campo [" & idUnico_ & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function [In](ByVal valor_? As Boolean,
                     ByRef documentoElectronico_ As DocumentoElectronico,
                     ByVal campo_ As [Enum],
                     ByVal asignacion_ As TiposAsignacion) As TagWatcher

        If valor_ Is Nothing Then : Return New TagWatcher(1) : End If

        Dim idUnico_ As Int32 = Convert.ToInt32(campo_)

        With documentoElectronico_

            Dim campoUnico_ As Object = .Attribute(idUnico_)

            If campoUnico_ IsNot Nothing Then

                Select Case campoUnico_.TipoDato

                    Case Componentes.Campo.TiposDato.Booleano

                        If asignacion_ = TiposAsignacion.Valor Then

                            .Attribute(idUnico_).Valor = valor_ : Return New TagWatcher(1)

                        Else

                            .Attribute(idUnico_).ValorPresentacion = valor_ : Return New TagWatcher(1)

                        End If

                    Case Else

                        Return New TagWatcher(0, Me, "El valor no es un boolean")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró el campo [" & idUnico_ & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function [In](ByRef caracteristica_ As Caracteristica,
                     ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        Dim idSeccionUnico_ As Int32 = Convert.ToInt32(caracteristica_.Seccion)

        Dim seccionUnica_ As Componentes.Seccion = Nothing

        If idSeccionUnico_ Then

            seccionUnica_ = documentoElectronico_.Seccion(idSeccionUnico_)

        End If


        Dim idCampoUnico_ As Int32 = Convert.ToInt32(caracteristica_.Campo)

        Dim campoUnico_ As Componentes.Campo = Nothing

        If idCampoUnico_ Then

            campoUnico_ = documentoElectronico_.Attribute(idCampoUnico_)

        End If

        Select Case caracteristica_.Control.GetType()

            Case GetType(CatalogControl)

                Return FillDocumentAttributesFromCatalog(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(PillboxControl)

                Return FillDocumentAttributesFromPillbox(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(ListboxControl)

                Return FillDocumentAttributesFromListbox(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(CollectionViewControl)

                Return FillDocumentAttributesFromCollectionView(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(InputControl)

                Return FillDocumentAttributesFromGenericControl(Of InputControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(SelectControl)

                Return FillDocumentAttributesFromGenericControl(Of SelectControl)(caracteristica_, campoUnico_, documentoElectronico_, True)

            Case GetType(SwitchControl)

                Return FillDocumentAttributesFromGenericControl(Of SwitchControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(FileControl)

                Return FillDocumentAttributesFromGenericControl(Of FileControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(FindboxControl)

                Return FillDocumentAttributesFromGenericControl(Of FindboxControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(DualityBarControl)

                Return FillDocumentAttributesFromGenericControl(Of DualityBarControl)(caracteristica_, campoUnico_, documentoElectronico_)

        End Select

        Return New TagWatcher(1)

    End Function

    Private Function FillDocumentAttributesFromGenericControl(Of T)(ByRef caracteristica_ As Caracteristica,
                                                                    ByRef campoUnico_ As Componentes.Campo,
                                                                    ByRef documentoElectronico_ As DocumentoElectronico,
                                                                    ByVal Optional tieneValorPresentacion_ As Boolean = False) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        If campoUnico_ IsNot Nothing Then

            With documentoElectronico_

                Dim valorAsignado_ = Nothing

                Dim valorPresentacionAsignado_ = Nothing

                Dim valorFirmaAsignado_ = Nothing

                Dim control_ As Object = caracteristica_.Control 'CType(Convert.ChangeType(caracteristica_.Control, GetType(T)), T)

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        valorAsignado_ = control_.Value

                    Case PropiedadesControl.Text

                        valorAsignado_ = control_.Text

                    Case PropiedadesControl.Checked

                        valorAsignado_ = control_.Checked

                    Case PropiedadesControl.ValueDetail

                        valorAsignado_ = control_.ValueDetail

                    Case PropiedadesControl.OnText

                        valorAsignado_ = control_.OnText

                    Case PropiedadesControl.OffText

                        valorAsignado_ = control_.OffText

                    Case PropiedadesControl.Signature

                        valorAsignado_ = control_.Signature

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

                If tieneValorPresentacion_ = False Then

                    If caracteristica_.Asignacion = TiposAsignacion.Valor Then

                        caracteristica_.Valor = StringFormat(campoUnico_.TipoDato, valorAsignado_)

                        .Attribute(campoUnico_.IDUnico).Valor = StringFormat(campoUnico_.TipoDato, valorAsignado_)

                    ElseIf caracteristica_.Asignacion = TiposAsignacion.ValorPresentacion Then

                        caracteristica_.ValorPresentacion = StringFormat(campoUnico_.TipoDato, valorAsignado_)

                        .Attribute(campoUnico_.IDUnico).ValorPresentacion = StringFormat(campoUnico_.TipoDato, valorAsignado_)

                    ElseIf caracteristica_.Asignacion = TiposAsignacion.ValorFirma Then

                        caracteristica_.ValorPresentacion = StringFormat(TiposDato.Texto, valorAsignado_)

                        .Attribute(campoUnico_.IDUnico).ValorFirma = StringFormat(TiposDato.Texto, valorAsignado_)

                    End If

                Else
                    'Lineas unicas por los selectores
                    caracteristica_.Valor = StringFormat(campoUnico_.TipoDato, valorAsignado_)

                    .Attribute(campoUnico_.IDUnico).Valor = StringFormat(campoUnico_.TipoDato, valorAsignado_)

                    'Valor Presentación

                    valorPresentacionAsignado_ = control_.Text

                    caracteristica_.ValorPresentacion = valorPresentacionAsignado_

                    .Attribute(campoUnico_.IDUnico).ValorPresentacion = valorPresentacionAsignado_

                    'Valor Firma

                    valorFirmaAsignado_ = control_.Signature

                    caracteristica_.ValorFirma = valorFirmaAsignado_

                    .Attribute(campoUnico_.IDUnico).ValorFirma = valorFirmaAsignado_

                End If

            End With

        Else

            Return New TagWatcher(0, Me, "No se encontró el campo [" & campoUnico_.IDUnico & "]")

        End If

        Return New TagWatcher(1)

    End Function

    Private Function FillDocumentAttributesFromCatalog(ByRef caracteristica_ As Caracteristica,
                                                       ByRef seccionUnica_ As Componentes.Seccion,
                                                       ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim documentoElectronicoReferenciaLocal_ = documentoElectronico_

                        Dim catalogControl_ As CatalogControl = DirectCast(caracteristica_.Control, CatalogControl)

                        catalogControl_.ForEach(Sub(catalogRow_ As CatalogRow)

                                                    If catalogRow_.GetIndice(catalogControl_.KeyField) > 0 Then

                                                        With seccionUnicaReferenciaLocal_.Partida(numeroSecuencia_:=catalogRow_.GetIndice(catalogControl_.KeyField))

                                                            Dim rowId_ = catalogControl_.DeleteRowsId

                                                            If rowId_ IsNot Nothing Then

                                                                If catalogControl_.DeleteRowsId.Contains(catalogRow_.GetIndice(catalogControl_.KeyField)) Then

                                                                    .estado = 0

                                                                End If

                                                            End If

                                                            For Each control_ As IUIControl In catalogControl_.Columns

                                                                Select Case control_.GetType()

                                                                    Case GetType(InputControl)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, catalogRow_.GetColumn(DirectCast(control_, InputControl)))

                                                                    Case GetType(SelectControl)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, catalogRow_.GetColumn(DirectCast(control_, SelectControl)).Value)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = catalogRow_.GetColumn(DirectCast(control_, SelectControl)).Text

                                                                    Case GetType(SwitchControl)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, catalogRow_.GetColumn(DirectCast(control_, SwitchControl)))

                                                                End Select

                                                            Next

                                                        End With

                                                    Else

                                                        With seccionUnicaReferenciaLocal_.Partida(documentoElectronicoReferenciaLocal_)

                                                            For Each control_ As IUIControl In catalogControl_.Columns

                                                                Select Case control_.GetType()

                                                                    Case GetType(InputControl)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, catalogRow_.GetColumn(DirectCast(control_, InputControl)))

                                                                    Case GetType(SelectControl)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, catalogRow_.GetColumn(DirectCast(control_, SelectControl)).Value)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = catalogRow_.GetColumn(DirectCast(control_, SelectControl)).Text

                                                                    Case GetType(SwitchControl)

                                                                        .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, catalogRow_.GetColumn(DirectCast(control_, SwitchControl)))

                                                                End Select

                                                            Next

                                                        End With

                                                    End If

                                                End Sub)

                        catalogControl_.CatalogDataRefresh()

                    Case PropiedadesControl.Ninguno

                        Return New TagWatcher(1)

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function FillDocumentAttributesFromPillbox(ByRef caracteristica_ As Caracteristica,
                                                       ByRef seccionUnica_ As Componentes.Seccion,
                                                       ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim documentoElectronicoReferenciaLocal_ = documentoElectronico_

                        Dim pillboxControl_ As PillboxControl = DirectCast(caracteristica_.Control, PillboxControl)

                        pillboxControl_.ForEach(Sub(pillbox_ As PillBox)

                                                    If pillbox_.GetIndice(pillboxControl_.KeyField) > 0 Then

                                                        With seccionUnicaReferenciaLocal_.Partida(numeroSecuencia_:=pillbox_.GetIndice(pillboxControl_.KeyField))

                                                            For Each control_ As IUIControl In pillboxControl_.PillboxListControls

                                                                If pillbox_.IsDeleted() = True Then

                                                                    .estado = 0

                                                                End If

                                                                If pillbox_.IsFiled() = True Then

                                                                    .archivado = True

                                                                End If

                                                                If control_.WorksWith IsNot Nothing Then

                                                                    Select Case control_.GetType()

                                                                        Case GetType(InputControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, InputControl)))

                                                                        Case GetType(SelectControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, SelectControl)).Value)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = pillbox_.GetControlValue(DirectCast(control_, SelectControl)).Text

                                                                        Case GetType(SwitchControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, SwitchControl)))

                                                                        Case GetType(FindboxControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, FindboxControl)).Value)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = pillbox_.GetControlValue(DirectCast(control_, FindboxControl)).Text

                                                                    End Select

                                                                Else

                                                                    If control_.GetType() = GetType(CatalogControl) Then

                                                                        Dim catalogcontrol_ = DirectCast(control_, CatalogControl)

                                                                        catalogcontrol_.DataSource = pillbox_.GetControlValue(catalogcontrol_)

                                                                        Dim caracteristicaAnidada_ = _caracteristicas.Find(Function(ByVal item_ As Caracteristica)

                                                                                                                               If item_.Control IsNot Nothing Then

                                                                                                                                   Return item_.Control.ID = catalogcontrol_.ID

                                                                                                                               End If

                                                                                                                               Return Nothing

                                                                                                                           End Function)

                                                                        If caracteristicaAnidada_ IsNot Nothing Then

                                                                            caracteristicaAnidada_.PropiedadDelControl = PropiedadesControl.Valor

                                                                            Dim seccionUnicaAnidada_ = .Seccion(Convert.ToInt32(caracteristicaAnidada_.Seccion))

                                                                            FillDocumentAttributesFromCatalog(caracteristicaAnidada_, seccionUnicaAnidada_, documentoElectronicoReferenciaLocal_)

                                                                        End If

                                                                    End If

                                                                End If

                                                            Next

                                                        End With

                                                    Else

                                                        If pillbox_.IsDeleted() = False And pillbox_.IsFiled() = False Then

                                                            With seccionUnicaReferenciaLocal_.Partida(documentoElectronicoReferenciaLocal_)

                                                                For Each control_ As IUIControl In pillboxControl_.PillboxListControls

                                                                    If control_.WorksWith IsNot Nothing Then

                                                                        Select Case control_.GetType()

                                                                            Case GetType(InputControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, InputControl)))

                                                                            Case GetType(SelectControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, SelectControl)).Value)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = pillbox_.GetControlValue(DirectCast(control_, SelectControl)).Text

                                                                            Case GetType(SwitchControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, SwitchControl)))

                                                                            Case GetType(FindboxControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, pillbox_.GetControlValue(DirectCast(control_, FindboxControl)).Value)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = pillbox_.GetControlValue(DirectCast(control_, FindboxControl)).Text

                                                                        End Select

                                                                    Else

                                                                        If control_.GetType() = GetType(CatalogControl) Then

                                                                            Dim catalogcontrol_ = DirectCast(control_, CatalogControl)

                                                                            catalogcontrol_.DataSource = pillbox_.GetControlValue(catalogcontrol_)

                                                                            Dim caracteristicaAnidada_ = _caracteristicas.Find(Function(ByVal item_ As Caracteristica)

                                                                                                                                   If item_.Control IsNot Nothing Then

                                                                                                                                       Return item_.Control.ID = catalogcontrol_.ID

                                                                                                                                   End If

                                                                                                                                   Return Nothing

                                                                                                                               End Function)

                                                                            If caracteristicaAnidada_ IsNot Nothing Then

                                                                                caracteristicaAnidada_.PropiedadDelControl = PropiedadesControl.Valor

                                                                                Dim seccionUnicaAnidada_ = .Seccion(Convert.ToInt32(caracteristicaAnidada_.Seccion))

                                                                                FillDocumentAttributesFromCatalog(caracteristicaAnidada_, seccionUnicaAnidada_, documentoElectronicoReferenciaLocal_)

                                                                            End If

                                                                        End If

                                                                    End If

                                                                Next

                                                            End With

                                                        End If

                                                    End If

                                                End Sub)

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function FillDocumentAttributesFromListbox(ByRef caracteristica_ As Caracteristica,
                                                       ByRef seccionUnica_ As Componentes.Seccion,
                                                       ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim dElectronico_ = documentoElectronico_

                        Dim listbox_ As ListboxControl = DirectCast(caracteristica_.Control, ListboxControl)

                        If listbox_.Value IsNot Nothing Then

                            For Each option_ As SelectOption In listbox_.Value

                                If option_.Indice > 0 Then

                                    With seccionUnicaReferenciaLocal_.Partida(numeroSecuencia_:=option_.Indice)

                                        If option_.Delete = True Then

                                            .estado = 0

                                        End If

                                        .Attribute(Convert.ToInt32(listbox_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(listbox_.WorksWith)).TipoDato, option_.Value)

                                        .Attribute(Convert.ToInt32(listbox_.WorksWith)).ValorPresentacion = option_.Text

                                    End With

                                Else

                                    If option_.Delete = False Then

                                        With seccionUnicaReferenciaLocal_.Partida(dElectronico_)

                                            .Attribute(Convert.ToInt32(listbox_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(listbox_.WorksWith)).TipoDato, option_.Value)

                                            .Attribute(Convert.ToInt32(listbox_.WorksWith)).ValorPresentacion = option_.Text

                                        End With

                                    End If

                                End If

                            Next

                        End If

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function FillDocumentAttributesFromCollectionView(ByRef caracteristica_ As Caracteristica,
                                                              ByRef seccionUnica_ As Componentes.Seccion,
                                                              ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim dElectronico_ = documentoElectronico_

                        Dim collectionview_ As CollectionViewControl = DirectCast(caracteristica_.Control, CollectionViewControl)

                        collectionview_.ForEach(Sub(ByVal collectionitem_ As CollectionItem)

                                                    If collectionitem_.GetIndice(collectionview_.KeyField) > 0 Then

                                                        With seccionUnicaReferenciaLocal_.Partida(numeroSecuencia_:=collectionitem_.GetIndice(collectionview_.KeyField))

                                                            For Each control_ As IUIControl In collectionview_.CollectionViewListControls

                                                                If collectionitem_.IsDeleted() = True Then

                                                                    .estado = 0

                                                                End If

                                                                If collectionitem_.IsFiled() = True Then

                                                                    .archivado = True

                                                                End If

                                                                If control_.WorksWith IsNot Nothing Then

                                                                    'evaluar como hacerle para propiedades  tipo catalogo
                                                                    'esto esta interesante
                                                                    'seccionUnica_.Partida analizar esta parte en los catalogos porque debo entrar a las partidas correspondientes del catalogo
                                                                    'hay pedo para saber la seccion de los grids dentro de un collection

                                                                    Select Case control_.GetType()

                                                                        Case GetType(InputControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, InputControl).ID))

                                                                        Case GetType(SelectControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, SelectControl).ID).Value)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = collectionitem_.GetControlValue(DirectCast(control_, SelectControl).ID).Text

                                                                        Case GetType(SwitchControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, SwitchControl).ID))

                                                                        Case GetType(FindboxControl)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, FindboxControl).ID).Value)

                                                                            .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = collectionitem_.GetControlValue(DirectCast(control_, FindboxControl).ID).Text

                                                                        Case GetType(CatalogControl)


                                                                    End Select

                                                                End If

                                                            Next

                                                        End With

                                                    Else

                                                        If collectionitem_.IsDeleted() = False And collectionitem_.IsFiled() = False Then

                                                            With seccionUnicaReferenciaLocal_.Partida(dElectronico_)

                                                                For Each control_ As IUIControl In collectionview_.CollectionViewListControls

                                                                    If control_.WorksWith IsNot Nothing Then

                                                                        Select Case control_.GetType()

                                                                            Case GetType(InputControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, InputControl).ID))

                                                                            Case GetType(SelectControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, SelectControl).ID).Value)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = collectionitem_.GetControlValue(DirectCast(control_, SelectControl).ID).Text

                                                                            Case GetType(SwitchControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, SwitchControl).ID))

                                                                            Case GetType(FindboxControl)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).Valor = StringFormat(.Attribute(Convert.ToInt32(control_.WorksWith)).TipoDato, collectionitem_.GetControlValue(DirectCast(control_, FindboxControl).ID).Value)

                                                                                .Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion = collectionitem_.GetControlValue(DirectCast(control_, FindboxControl).ID).Text

                                                                            Case GetType(CatalogControl)


                                                                        End Select

                                                                    End If

                                                                Next

                                                            End With

                                                        End If

                                                    End If

                                                End Sub)

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function Out(ByRef caracteristica_ As Caracteristica,
                         ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher


        Dim idSeccionUnico_ As Int32 = Convert.ToInt32(caracteristica_.Seccion)

        Dim seccionUnica_ As Componentes.Seccion = Nothing

        If idSeccionUnico_ Then

            seccionUnica_ = documentoElectronico_.Seccion(idSeccionUnico_)

        End If


        Dim idCampoUnico_ As Int32 = Convert.ToInt32(caracteristica_.Campo)

        Dim campoUnico_ As Componentes.Campo = Nothing

        If idCampoUnico_ Then

            campoUnico_ = documentoElectronico_.Attribute(idCampoUnico_)

        End If

        Select Case caracteristica_.Control.GetType()

            Case GetType(CatalogControl)

                Return FillCatalogFromDocumentAttributes(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(PillboxControl)

                Return FillPillboxFromDocumentAttributes(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(ListboxControl)

                Return FillListboxFromDocumentAttributes(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(CollectionViewControl)

                Return FillCollectionViewFromDocumentAttributes(caracteristica_, seccionUnica_, documentoElectronico_)

            Case GetType(InputControl)

                Return FillGenericControlFromDocumentAttributes(Of InputControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(SelectControl)

                Return FillGenericControlFromDocumentAttributes(Of SelectControl)(caracteristica_, campoUnico_, documentoElectronico_, True)

            Case GetType(SwitchControl)

                Return FillGenericControlFromDocumentAttributes(Of SwitchControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(FileControl)

                Return FillGenericControlFromDocumentAttributes(Of FileControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(FindboxControl)

                Return FillGenericControlFromDocumentAttributes(Of FindboxControl)(caracteristica_, campoUnico_, documentoElectronico_)

            Case GetType(DualityBarControl)

                Return FillGenericControlFromDocumentAttributes(Of DualityBarControl)(caracteristica_, campoUnico_, documentoElectronico_)

        End Select

        Return New TagWatcher(1)

    End Function

    Private Function FillGenericControlFromDocumentAttributes(Of T)(ByRef caracteristica_ As Caracteristica,
                                                                    ByRef campoUnico_ As Componentes.Campo,
                                                                    ByRef documentoElectronico_ As DocumentoElectronico,
                                                                    ByVal Optional tieneValorPresentacion_ As Boolean = False) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        If campoUnico_ IsNot Nothing Then

            With documentoElectronico_

                Dim valorAsignado_ = Nothing

                Dim valorPresentacionAsignado_ = Nothing

                If tieneValorPresentacion_ = False Then

                    If caracteristica_.Asignacion = TiposAsignacion.Valor Then

                        valorAsignado_ = campoUnico_.Valor

                        caracteristica_.Valor = valorAsignado_

                    Else

                        valorAsignado_ = campoUnico_.ValorPresentacion

                        caracteristica_.ValorPresentacion = valorAsignado_

                    End If

                Else
                    'Lineas unicas por los selectores

                    valorAsignado_ = campoUnico_.Valor

                    caracteristica_.Valor = valorAsignado_


                    valorPresentacionAsignado_ = campoUnico_.ValorPresentacion

                    caracteristica_.ValorPresentacion = valorPresentacionAsignado_

                End If

                Dim control_ As Object = caracteristica_.Control 'CType(Convert.ChangeType(caracteristica_.Control, GetType(T)), T)

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Auto, PropiedadesControl.Valor

                        If tieneValorPresentacion_ = False Then

                            If GetType(T) = GetType(InputControl) Then

                                control_.Value = StringMask(control_.Format, valorAsignado_?.ToString)

                            Else

                                control_.Value = valorAsignado_?.ToString

                            End If

                        Else
                            'Lineas unicas por los selectores

                            With control_

                                .DataSource = New List(Of SelectOption) From {New SelectOption With {
                                    .Value = valorAsignado_?.ToString,
                                    .Text = valorPresentacionAsignado_?.ToString}
                                }

                                .Value = valorAsignado_?.ToString

                            End With

                        End If

                    Case PropiedadesControl.Text

                        control_.Text = valorAsignado_?.ToString

                    Case PropiedadesControl.Checked

                        control_.Checked = valorAsignado_

                    Case PropiedadesControl.ValueDetail

                        control_.ValueDetail = valorAsignado_?.ToString

                    Case PropiedadesControl.OnText

                        control_.OnText = valorAsignado_?.ToString

                    Case PropiedadesControl.OffText

                        control_.OffText = valorAsignado_?.ToString

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            End With

        Else

            Return New TagWatcher(0, Me, "No se encontró el campo [" & campoUnico_.IDUnico & "]")

        End If

        Return New TagWatcher(1)

    End Function

    Private Function FillCatalogFromDocumentAttributes(ByRef caracteristica_ As Caracteristica,
                                                       ByRef seccionUnica_ As Componentes.Seccion,
                                                       ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher


        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim catalog_ As CatalogControl = DirectCast(caracteristica_.Control, CatalogControl)

                        catalog_.ClearRows()

                        For indice_ As Int32 = 1 To seccionUnicaReferenciaLocal_.CantidadPartidas

                            If seccionUnicaReferenciaLocal_.Partida(indice_).estado = 1 Then

                                catalog_.SetRow(Sub(catalogRow_ As CatalogRow)

                                                    catalogRow_.SetIndice(catalog_.KeyField, indice_)

                                                    For Each control_ As IUIControl In catalog_.Columns

                                                        If control_.WorksWith IsNot Nothing Then

                                                            Select Case control_.GetType()

                                                                Case GetType(InputControl)

                                                                    catalogRow_.SetColumn(DirectCast(control_, InputControl), StringMask(DirectCast(control_, InputControl).Format, seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor))

                                                                Case GetType(SelectControl)

                                                                    catalogRow_.SetColumn(DirectCast(control_, SelectControl), New SelectOption With {
                                                                            .Value = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor?.ToString(),
                                                                            .Text = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion
                                                                        })

                                                                Case GetType(SwitchControl)

                                                                    catalogRow_.SetColumn(DirectCast(control_, SwitchControl), seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor)

                                                            End Select

                                                        End If

                                                    Next

                                                End Sub)

                            End If

                        Next

                        catalog_.CatalogDataBinding()

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function FillPillboxFromDocumentAttributes(ByRef caracteristica_ As Caracteristica,
                                                       ByRef seccionUnica_ As Componentes.Seccion,
                                                       ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim documentoElectronicoReferenciaLocal_ = documentoElectronico_

                        Dim pillboxControl_ As PillboxControl = DirectCast(caracteristica_.Control, PillboxControl)

                        pillboxControl_.ClearRows()

                        For indice_ As Int32 = 1 To seccionUnicaReferenciaLocal_.CantidadPartidas

                            If seccionUnicaReferenciaLocal_.Partida(indice_).estado = 1 Then

                                pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)

                                                               pillbox_.SetIndice(pillboxControl_.KeyField, indice_)

                                                               pillbox_.SetFiled(seccionUnicaReferenciaLocal_.Partida(indice_).archivado)

                                                               For Each control_ As IUIControl In pillboxControl_.PillboxListControls

                                                                   If control_.WorksWith IsNot Nothing Then

                                                                       Select Case control_.GetType()

                                                                           Case GetType(InputControl)

                                                                               pillbox_.SetControlValue(DirectCast(control_, InputControl), StringMask(DirectCast(control_, InputControl).Format, seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor))

                                                                           Case GetType(SelectControl)

                                                                               pillbox_.SetControlValue(DirectCast(control_, SelectControl), New SelectOption With {
                                                                                        .Value = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor?.ToString(),
                                                                                        .Text = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion
                                                                                   })

                                                                           Case GetType(SwitchControl)

                                                                               pillbox_.SetControlValue(DirectCast(control_, SwitchControl), seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor)

                                                                           Case GetType(FindboxControl)

                                                                               pillbox_.SetControlValue(DirectCast(control_, FindboxControl), New SelectOption With {
                                                                                        .Value = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor?.ToString(),
                                                                                        .Text = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion
                                                                                   })

                                                                       End Select

                                                                   Else

                                                                       If control_.GetType() = GetType(CatalogControl) Then

                                                                           Dim catalogcontrol_ = DirectCast(control_, CatalogControl)

                                                                           Dim caracteristicaAnidada_ = _caracteristicas.Find(Function(ByVal item_ As Caracteristica)

                                                                                                                                  If item_.Control IsNot Nothing Then

                                                                                                                                      Return item_.Control.ID = catalogcontrol_.ID

                                                                                                                                  End If

                                                                                                                                  Return Nothing

                                                                                                                              End Function)

                                                                           With caracteristicaAnidada_

                                                                               .PropiedadDelControl = PropiedadesControl.Valor

                                                                               Dim seccionUnicaAnidada_ = seccionUnicaReferenciaLocal_.Partida(indice_).Seccion(Convert.ToInt32(.Seccion))

                                                                               FillCatalogFromDocumentAttributes(caracteristicaAnidada_, seccionUnicaAnidada_, documentoElectronicoReferenciaLocal_)

                                                                               Dim dataSource_ = New Dictionary(Of Object, Object)

                                                                               With dataSource_

                                                                                   .Add("Items", catalogcontrol_.DataSource)

                                                                                   .Add("DropItems", Nothing)

                                                                                   .Add("SelectedItem", Nothing)

                                                                               End With

                                                                               pillbox_.SetControlValue(catalogcontrol_, dataSource_)

                                                                           End With

                                                                       End If


                                                                   End If

                                                               Next

                                                           End Sub)

                            End If

                        Next

                        pillboxControl_.PillBoxDataBinding()

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function FillListboxFromDocumentAttributes(ByRef caracteristica_ As Caracteristica,
                                                       ByRef seccionUnica_ As Componentes.Seccion,
                                                       ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim listBox_ As ListboxControl = DirectCast(caracteristica_.Control, ListboxControl)

                        Dim dataSource_ As New List(Of SelectOption)

                        For indice_ As Int32 = 1 To seccionUnicaReferenciaLocal_.CantidadPartidas

                            dataSource_.Add(New SelectOption With {
                                               .Indice = indice_,
                                               .Value = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(listBox_.WorksWith)).Valor?.ToString(),
                                               .Text = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(listBox_.WorksWith)).ValorPresentacion,
                                               .Delete = IIf(seccionUnicaReferenciaLocal_.Partida(indice_).estado = 1, False, True)})

                        Next

                        listBox_.Value = dataSource_

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function FillCollectionViewFromDocumentAttributes(ByRef caracteristica_ As Caracteristica,
                                                              ByRef seccionUnica_ As Componentes.Seccion,
                                                              ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        If caracteristica_.PropiedadDelControl = PropiedadesControl.Ninguno Then

            Return New TagWatcher(1)

        End If

        With documentoElectronico_

            Dim seccionUnicaReferenciaLocal_ = seccionUnica_

            If seccionUnicaReferenciaLocal_ IsNot Nothing Then

                Select Case caracteristica_.PropiedadDelControl

                    Case PropiedadesControl.Valor, PropiedadesControl.Auto

                        Dim collectionControl_ As CollectionViewControl = DirectCast(caracteristica_.Control, CollectionViewControl)

                        'collectionControl_.ClearRows()
                        For indice_ As Int32 = 1 To seccionUnicaReferenciaLocal_.CantidadPartidas

                            If seccionUnicaReferenciaLocal_.Partida(indice_).estado = 1 Then

                                collectionControl_.SetItem(Sub(collectionview_ As CollectionItem)

                                                               collectionview_.SetIndice(collectionControl_.KeyField, indice_)

                                                               collectionview_.SetFiled(seccionUnicaReferenciaLocal_.Partida(indice_).archivado)

                                                               For Each control_ As IUIControl In collectionControl_.CollectionViewListControls

                                                                   If control_.WorksWith IsNot Nothing Then

                                                                       Select Case control_.GetType()

                                                                           Case GetType(InputControl)

                                                                               collectionview_.SetControlValue(DirectCast(control_, InputControl).ID, StringMask(DirectCast(control_, InputControl).Format, seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor))

                                                                           Case GetType(SelectControl)

                                                                               collectionview_.SetControlValue(DirectCast(control_, SelectControl).ID, New SelectOption With {
                                                                                    .Value = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor?.ToString(),
                                                                                    .Text = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion
                                                                               })

                                                                           Case GetType(SwitchControl)

                                                                               collectionview_.SetControlValue(DirectCast(control_, SwitchControl).ID, seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor)

                                                                           Case GetType(FindboxControl)

                                                                               collectionview_.SetControlValue(DirectCast(control_, FindboxControl).ID, New SelectOption With {
                                                                                    .Value = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).Valor?.ToString(),
                                                                                    .Text = seccionUnicaReferenciaLocal_.Partida(indice_).Attribute(Convert.ToInt32(control_.WorksWith)).ValorPresentacion
                                                                               })

                                                                           Case GetType(CatalogControl)



                                                                       End Select

                                                                   End If

                                                               Next

                                                           End Sub)

                            End If

                        Next

                        collectionControl_.CollectionViewDataBinding()

                    Case Else

                        Return New TagWatcher(0, Me, "Propiedad no soportada para este control")

                End Select

            Else

                Return New TagWatcher(0, Me, "No se encontró la sección [" & seccionUnica_.IDUnico & "]")

            End If

        End With

        Return New TagWatcher(1)

    End Function

    Private Function StringMask(ByVal format_ As InputControl.InputFormat, ByVal valorAsignado_ As String)

        Select Case format_

            Case InputControl.InputFormat.Calendar

                If IsDate(valorAsignado_) Then

                    Return Convert.ToDateTime(valorAsignado_).Date.ToString("yyyy-MM-dd")

                End If

            Case InputControl.InputFormat.Money

                Return FormatCurrency(valorAsignado_)

            Case Else

                Return valorAsignado_

        End Select

        Return Nothing

    End Function

    Private Function StringFormat(ByVal TipoDato_ As TiposDato, ByVal Cadena_ As String)

        If String.IsNullOrEmpty(Cadena_) Then

            Return Nothing

        End If

        Select Case TipoDato_

            Case TiposDato.Booleano

                Return Convert.ToBoolean(Cadena_)

            Case TiposDato.Entero

                Dim reg As New Regex("[^0-9]")

                Cadena_ = reg.Replace(Cadena_, "")

                If IsNumeric(Cadena_) Then

                    Return Convert.ToInt32(Cadena_)

                End If

            Case TiposDato.Fecha

                If IsDate(Cadena_) Then

                    Return Convert.ToDateTime(Cadena_).Date

                End If

            Case TiposDato.Real

                Dim reg As New Regex("[^0-9.]")

                Cadena_ = reg.Replace(Cadena_, "")

                If IsNumeric(Cadena_) Then

                    Return Convert.ToDecimal(Cadena_)

                End If

            Case TiposDato.IdObject

                Return New ObjectId(Cadena_)

            Case TiposDato.Documento

        End Select

        Return Cadena_

    End Function

    Public Sub [Set](ByVal valor_ As ObjectId,
                     ByVal campo_ As [Enum],
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        AgregarCaracteristica(valor_,
                              campo_,
                              nombreMostrar,
                              tipoAsignacion_,
                              visible_,
                              puedeInsertar_,
                              puedeModificar_,
                              valorDefault_,
                              tipoFiltro_,
                              llave_,
                              soloLectura_,
                              permisoConsulta_)

    End Sub

    Private Sub AgregarCaracteristica(ByVal valor_ As Object,
                     ByVal campo_ As [Enum],
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        If soloLectura_ Then : puedeInsertar_ = False : puedeModificar_ = False : End If

        _caracteristicas.Add(New Caracteristica With {.Valor = valor_, .Campo = campo_, .Asignacion = tipoAsignacion_})

    End Sub

    Public Sub [Set](ByRef control_ As IUIControl,
                     ByVal campo_ As [Enum],
                     Optional ByVal seccion_ As [Enum] = Nothing,
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal asignarA_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal propiedadDelControl_ As PropiedadesControl = PropiedadesControl.Auto,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing,
                     Optional ByVal documentoDigital As Boolean = False)

        control_.WorksWith = campo_

        If soloLectura_ Then : puedeInsertar_ = False : puedeModificar_ = False : End If

        If permisoConsulta_ IsNot Nothing Then : control_.IdPermiso = permisoConsulta_ : End If

        _caracteristicas.Add(New Caracteristica With {.Control = control_, .Campo = campo_, .Seccion = seccion_, .Asignacion = asignarA_, .PropiedadDelControl = propiedadDelControl_, .DocumentoDigital = documentoDigital})

    End Sub

    Public Sub [Set](ByVal valor_ As String,
                     ByVal campo_ As [Enum],
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        AgregarCaracteristica(valor_,
                              campo_,
                              nombreMostrar,
                              tipoAsignacion_,
                              visible_,
                              puedeInsertar_,
                              puedeModificar_,
                              valorDefault_,
                              tipoFiltro_,
                              llave_,
                              soloLectura_,
                              permisoConsulta_)

    End Sub

    Public Sub [Set](ByVal valor_ As Double,
                     ByVal campo_ As [Enum],
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        AgregarCaracteristica(valor_,
                              campo_,
                              nombreMostrar,
                              tipoAsignacion_,
                              visible_,
                              puedeInsertar_,
                              puedeModificar_,
                              valorDefault_,
                              tipoFiltro_,
                              llave_,
                              soloLectura_,
                              permisoConsulta_)

    End Sub

    Public Sub [Set](ByVal valor_ As DateTime,
                     ByVal campo_ As [Enum],
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        AgregarCaracteristica(valor_,
                              campo_,
                              nombreMostrar,
                              tipoAsignacion_,
                              visible_,
                              puedeInsertar_,
                              puedeModificar_,
                              valorDefault_,
                              tipoFiltro_,
                              llave_,
                              soloLectura_,
                              permisoConsulta_)

    End Sub

    Public Sub [Set](ByVal valor_ As Object,
                     ByVal campo_ As [Enum],
                     ByVal tipoDato_ As TiposDato,
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        Select Case tipoDato_
            Case TiposDato.Entero
                valor_ = Convert.ToInt32(valor_)
            Case TiposDato.Real
                valor_ = Convert.ToDouble(valor_)
            Case TiposDato.Booleano
                valor_ = Convert.ToInt32(valor_)
            Case TiposDato.IdObject
                'valor_ = Convert.(valor_)
            Case TiposDato.Texto
                valor_ = Convert.ToString(valor_)
            Case TiposDato.Fecha
                valor_ = Convert.ToDateTime(valor_).Date

        End Select


        AgregarCaracteristica(valor_,
                              campo_,
                              nombreMostrar,
                              tipoAsignacion_,
                              visible_,
                              puedeInsertar_,
                              puedeModificar_,
                              valorDefault_,
                              tipoFiltro_,
                              llave_,
                              soloLectura_,
                              permisoConsulta_)

    End Sub

    Public Sub [Set](ByVal valor_ As Boolean,
                     ByVal campo_ As [Enum],
                     Optional ByVal nombreMostrar As String = Nothing,
                     Optional ByVal tipoAsignacion_ As TiposAsignacion = TiposAsignacion.Valor,
                     Optional ByVal visible_ As TiposVisible = TiposVisible.Si,
                     Optional ByVal puedeInsertar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal puedeModificar_ As TiposRigorDatos = TiposRigorDatos.Si,
                     Optional ByVal valorDefault_ As Object = Nothing,
                     Optional ByVal tipoFiltro_ As TiposFiltro? = Nothing,
                     Optional ByVal llave_ As TiposLlave? = Nothing,
                     Optional ByVal soloLectura_ As Boolean = False,
                     Optional ByVal permisoConsulta_ As Int32? = Nothing)

        AgregarCaracteristica(valor_,
                              campo_,
                              nombreMostrar,
                              tipoAsignacion_,
                              visible_,
                              puedeInsertar_,
                              puedeModificar_,
                              valorDefault_,
                              tipoFiltro_,
                              llave_,
                              soloLectura_,
                              permisoConsulta_)

    End Sub

    Public Sub OperadorDatos(ByRef documentoElectronico_ As DocumentoElectronico,
                             ByVal tipoFlujo_ As TiposFlujo,
                             Optional ByVal modalidad_ As Modalidades = Modalidades.Auto)

        Select Case modalidad_

            Case Modalidades.Auto

                'MOP. Next step must could be singleton
                Configuracion()

                Procesamiento(documentoElectronico_, tipoFlujo_)

            Case Modalidades.Colectar

                Configuracion()

            Case Modalidades.Procesar

                Procesamiento(documentoElectronico_, tipoFlujo_)

                'Preparación de documentos asociados
                ProcesarDocumentosAsociados(documentoElectronico_)

        End Select

    End Sub

    Private Sub ProcesarDocumentosAsociados(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            If .DocumentosAsociados IsNot Nothing Then

                If .DocumentosAsociados.Count Then

                    Dim listaDocumentosAsociados_ As New List(Of DocumentoAsociado)

                    For Each documentosasociado_ As DocumentoAsociado In .DocumentosAsociados

                        If documentosasociado_.idsection = 0 Then

                            Dim campo_ As Componentes.Campo = .Campo(documentosasociado_.idcampo)

                            AntesGuardarDocumentoAsociado(documentosasociado_, documentoElectronico_)

                            listaDocumentosAsociados_.Add(New DocumentoAsociado With {
                                                             ._iddocumentoasociado = campo_.Valor,
                                                             .idcoleccion = documentosasociado_.idcoleccion,
                                                             .identificadorrecurso = documentosasociado_.identificadorrecurso,
                                                             .firmaelectronica = campo_.ValorFirma,
                                                             .metadatos = documentosasociado_.metadatos
                                                         })

                        Else

                            Dim seccion_ As Componentes.Seccion = .Seccion(documentosasociado_.idsection)

                            For indice_ As Int32 = 1 To seccion_.CantidadPartidas

                                Dim partida_ As Componentes.Partida = seccion_.Partida(indice_)

                                AntesGuardarDocumentoAsociado(documentosasociado_, documentoElectronico_)

                                listaDocumentosAsociados_.Add(New DocumentoAsociado With {
                                                             ._iddocumentoasociado = partida_.Attribute(documentosasociado_.idcampo).Valor,
                                                             .idcoleccion = documentosasociado_.idcoleccion,
                                                             .identificadorrecurso = documentosasociado_.identificadorrecurso,
                                                             .firmaelectronica = partida_.Attribute(documentosasociado_.idcampo).ValorFirma,
                                                             .metadatos = documentosasociado_.metadatos
                                                         })

                            Next

                        End If

                    Next

                    .DocumentosAsociados = listaDocumentosAsociados_

                End If

            End If

        End With

    End Sub

    Public Overridable Sub AntesGuardarDocumentoAsociado(ByRef documentoasociado_ As DocumentoAsociado, ByRef documentoelectronico_ As DocumentoElectronico)

    End Sub

    Public Function Procesamiento(ByRef documentoElectronico_ As DocumentoElectronico,
                                  ByVal tipoFlujo_ As TiposFlujo) As TagWatcher

        Dim caracteristicas_ = _caracteristicas

        Dim mensajes_ As TagWatcher

        Select Case tipoFlujo_

            Case TiposFlujo.Entrada

                For Each caracteristica_ As Caracteristica In caracteristicas_

                    With caracteristica_

                        If caracteristica_.Control IsNot Nothing Then ' Mediante control

                            Try


                                Dim check_ As TagWatcher = [In](caracteristica_, documentoElectronico_)

                                If check_.Status <> TypeStatus.Ok Then : Return check_ : End If

                            Catch ex As Exception

                                Dim a = ex

                            End Try

                        ElseIf caracteristica_.Valor IsNot Nothing Then 'Mediante valor directo
                            'Por el momento no soporta asignacion directa a otras propiedades como Text, eso solo puede suceder mediante control
                            'Buscaremos el repositorio del dato en el documento para enterarnos del tipo de dato que toca.

                            Dim idUnico_ As Int32 = Convert.ToInt32(caracteristica_.Campo)

                            Dim campoUnico_ As Object = documentoElectronico_.Attribute(idUnico_)

                            If campoUnico_ IsNot Nothing Then

                                caracteristica_.TipoDato = campoUnico_.TipoDato

                                Try

                                    Dim check_ As New TagWatcher

                                    Select Case .TipoDato

                                        Case TiposDato.IdObject

                                            'DirectCast(.Valor, ObjectId)
                                            check_ = [In](New ObjectId(.Valor?.ToString), documentoElectronico_, .Campo, .Asignacion)

                                        Case TiposDato.Booleano

                                            check_ = [In](DirectCast(.Valor, Boolean), documentoElectronico_, .Campo, .Asignacion)

                                        Case TiposDato.Entero

                                            check_ = [In](DirectCast(.Valor, Integer), documentoElectronico_, .Campo, .Asignacion)

                                        Case TiposDato.Fecha

                                            check_ = [In](DirectCast(.Valor, DateTime), documentoElectronico_, .Campo) ', .Asignacion)

                                        Case TiposDato.Real

                                            check_ = [In](DirectCast(.Valor, Double), documentoElectronico_, .Campo, .Asignacion)

                                        Case TiposDato.Texto

                                            check_ = [In](DirectCast(.Valor, String), documentoElectronico_, .Campo, .Asignacion)

                                        Case Else

                                            mensajes_ = New TagWatcher(0, Me, "No se encontró el tipo de dato")

                                    End Select

                                    If check_.Status <> TypeStatus.Ok Then : Return check_ : End If

                                Catch ex As Exception

                                    Dim a = ex

                                End Try

                            Else

                                mensajes_ = New TagWatcher(0, Me, "No se encontró el idUnico(" & idUnico_ & ") en el documento")

                            End If

                        Else

                            mensajes_ = New TagWatcher(0, Me, "Debe asignar un valor para el (" & caracteristica_.Campo.ToString & ")")

                        End If

                    End With

                Next

            Case TiposFlujo.Salida

                For Each caracteristica_ As Caracteristica In caracteristicas_

                    With caracteristica_

                        If caracteristica_.Control IsNot Nothing Then ' Mediante control

                            Dim check_ As TagWatcher =
                                Out(caracteristica_, documentoElectronico_)

                            If check_.Status <> TypeStatus.Ok Then : Return check_ : End If

                        End If

                    End With

                Next

        End Select

    End Function

    Public Overridable Function Configuracion() As TagWatcher

        Return New TagWatcher(1)

    End Function

    Public Function XMLDecode(Of T)(ByVal xmlRootAttribute As String, Optional queryString As String = Nothing, Optional ByVal filePath As String = Nothing) As Object
        'dll agregadas (Organismo, iCOnexiones, SQLServerSingletonConexion)

        Dim organismo_ = New Organismo()

        Dim sqlQueryString_ = queryString

        If Not String.IsNullOrEmpty(filePath) Then

            sqlQueryString_ = My.Computer.FileSystem.ReadAllText(filePath)

        End If

        Dim dataTable_ = ConsultaLibre(sqlQueryString_)

        If dataTable_ IsNot Nothing Then

            Dim xmlstring_ = ""

            For Each element As DataRow In dataTable_.Rows

                xmlstring_ = xmlstring_ & element(0)

            Next

            Dim serializer As New XmlSerializer(GetType(T), New XmlRootAttribute(xmlRootAttribute))

            Try

                Return serializer.Deserialize(New StringReader(xmlstring_))

            Catch ex As Exception

            End Try

        End If

        Return Nothing

    End Function

#End Region
End Class

Public Class Caracteristica

    Property TipoDato As TiposDato

    Property Control As UIControl

    Property NombreMostrar As String

    Property Asignacion As TiposAsignacion

    Property PropiedadDelControl As PropiedadesControl

    Property Campo As [Enum]

    Property Seccion As [Enum]

    Property Valor As Object

    Property ValorPresentacion As Object

    Property ValorFirma As Object

    Property Visible As TiposVisible

    Property PuedeInsertar As TiposRigorDatos

    Property PuedeModificar As TiposRigorDatos

    Property ValorDefault As String

    Property TipoFiltro As TiposFiltro

    Property Llave As TiposLlave

    Property SoloLectura As Boolean

    Property PermisoConsulta As Int32

    Property Estado As TagWatcher

    Property DocumentoDigital As Boolean

End Class