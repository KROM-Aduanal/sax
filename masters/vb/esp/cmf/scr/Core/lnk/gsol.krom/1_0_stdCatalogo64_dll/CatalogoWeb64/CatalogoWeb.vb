Imports Gsol.krom
Imports Gsol.krom.controladores
Imports Wma.Exceptions
Imports gsol.krom.controladores.ControladorWeb.FiltrosDashBoard
Imports System.Web.Services
Imports Sax.Web
Imports Gsol.krom.ControladorAccesoKBW64

Public Class CatalogoWeb
    Inherits ControladorBackendKBW

#Region "Atributos"

    Public _estatus As TagWatcher

    Public _limiteTotal As Int32 = 0

    Public _controladorWeb As ControladorWeb

    Public _entidadDatos As IEntidadDatos

    Public _cantidadRegistros As Dictionary(Of String, Integer)

    Public _aCookie As HttpCookie

    'Private _empresasCliente As DataTable
    Private _empresasCliente As Object

    Public _esConsultaMuestral As Boolean = True

    Public _nombreModulo As String = Nothing

    Public _granularidad As IEnlaceDatos.TiposDimension

    Public _datosBusquedaPrincipal As Dictionary(Of String, String)

#End Region

#Region "Propiedades"

    ReadOnly Property EmpresasCliente

        Get

            Return _empresasCliente

        End Get

    End Property

    ReadOnly Property DatosBusquedaPrincial As Dictionary(Of String, String)

        Get

            Return _datosBusquedaPrincipal

        End Get

    End Property

#End Region

#Region "Constructores"

    Sub New()

        _estatus = New TagWatcher

        _aCookie = New HttpCookie("cookieTotalRegistros")

        _datosBusquedaPrincipal = New Dictionary(Of String, String)

    End Sub

#End Region

#Region "Metodos"

    Public Overrides Sub Inicializa()

        _controladorWeb = GetVars("ControladorWeb")

        _cantidadRegistros = GetVars("CantidadRegistros")

        If Not Me.IsPostBack Then

            _controladorWeb.EnlaceDatos.Granularidad = _granularidad

            CargarEntidadDatos()

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then


                Dim tag_ As TagWatcher = _controladorWeb.EnlaceDatos.ObtieneEstructuraResultados(_entidadDatos, Nothing)

                Dim entidadDatosEstructura_ As List(Of IEntidadDatos) = Nothing

                If tag_.Status = TagWatcher.TypeStatus.Ok Then

                    '_datosBusquedaPrincipal = _controladorWeb.ObtenerBusquedaPrincipal()

                    entidadDatosEstructura_ = tag_.ObjectReturned

                    _controladorWeb.ListaAtributos = entidadDatosEstructura_(0).Atributos

                    With _controladorWeb

                        If .ComponentesOperacionesUsuario.ContainsKey(RazonesSocialesCliente) Then

                            _empresasCliente = .ComponentesOperacionesUsuario(RazonesSocialesCliente)

                        Else

                            _empresasCliente = Nothing

                        End If

                    End With

                    ClientScript.RegisterStartupScript(Me.GetType(),
                                                       "JSScript",
                                                       _controladorWeb.ObtenerFuncionesJavaScript(_nombreModulo,
                                                                                                  True,
                                                                                                  False,
                                                                                                  True,
                                                                                                  True).ToString)
                End If

            Else

                'Not implemented

            End If

        End If

        PreparaRequest(Request, Response, Me)

    End Sub

    Public Sub Inicializar(ByVal nombreModulo_ As String)

        _estatus = New TagWatcher

        _aCookie = New HttpCookie("cookieTotalRegistros")

        _nombreModulo = nombreModulo_

    End Sub

    Protected Overridable Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete

        _esConsultaMuestral = True

    End Sub

    '<WebMethod> Shared
    Public Function ConsultarControladorWeb(clausulasLibres_ As String)

        Dim controladorWeb_ As ControladorWeb = GetVars("ControladorWeb")

        If _esConsultaMuestral Then

            controladorWeb_.EnlaceDatos.LimiteRegistrosColeccionMuestral = _cantidadRegistros("LimiteMuestralMinimo")

            controladorWeb_.EnlaceDatos.LimiteResultados = _cantidadRegistros("LimiteMuestralMinimo")

            _esConsultaMuestral = False

        Else

            controladorWeb_.EnlaceDatos.LimiteRegistrosColeccionMuestral = _cantidadRegistros("LimiteRegistrosColeccionMuestral")

            controladorWeb_.EnlaceDatos.LimiteResultados = _cantidadRegistros("LimiteResultados")

        End If

        Dim registrosEncontrados_ As New List(Of IEntidadDatos)

        If clausulasLibres_ = "" Then

            clausulasLibres_ = Nothing

        End If

        controladorWeb_.FiltrosAvanzados = clausulasLibres_

        controladorWeb_.EnlaceDatos.TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativo

        controladorWeb_.EnlaceDatos.ModalidadConsulta = Gsol.BaseDatos.Operaciones.IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

        registrosEncontrados_ = controladorWeb_.EnlaceDatos.GeneraTransaccion(_entidadDatos, Nothing)

        _aCookie.Value = controladorWeb_.EnlaceDatos.Registros.Count

        '_aCookie.Values("nombreUsuario") = HttpContext.Current.Session("DatosUsuario").item("Nombre")

        _aCookie.Expires = DateTime.Now.AddMinutes(1)

        Response.Cookies.Add(_aCookie)

        Return controladorWeb_.EnlaceDatos.ObtenerListaRegistros

    End Function

    Public Overridable Sub CargarEntidadDatos()



    End Sub

    '<WebMethod>
    'Public Shared Function ConsultarDocumentos(ByVal id_ As String)

    '    Dim controladorWeb_ As ControladorWeb = Nothing

    '    Dim jsonDocumentos_ As String = ""

    '    If Not HttpContext.Current.Session("ControladorWeb") Is Nothing Then

    '        controladorWeb_ = HttpContext.Current.Session("ControladorWeb")

    '        If Not controladorWeb_ Is Nothing Then

    '            Dim organismo_ As New Organismo

    '            Dim iOperaciones_ As IOperacionesCatalogo

    '            Dim espacioTrabajo_ As EspacioTrabajo = HttpContext.Current.Session("EspacioTrabajoExtranet")

    '            If Not espacioTrabajo_ Is Nothing Then

    '                iOperaciones_ = organismo_.EnsamblaModulo("ConsultaTipoDocumentosKBW")

    '                With iOperaciones_

    '                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta)

    '                    .EspacioTrabajo = espacioTrabajo_

    '                    .ClausulasLibres = " AND i_Cve_MaestroOperaciones = '" & id_ + "'"

    '                    .OrdenarResultados(1) = IOperacionesCatalogo.OrdenConsulta.ASC

    '                End With

    '                iOperaciones_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

    '                iOperaciones_.GenerarVista()

    '                If organismo_.TieneResultados(iOperaciones_) Then

    '                    Dim tablaDocumentos_ As DataTable = iOperaciones_.Vista().Tables(0)

    '                    Dim listaDocumentos_ As New List(Of Dictionary(Of String, String))

    '                    For Each campo_ As DataRow In tablaDocumentos_.Rows

    '                        Dim documentos_ As New Dictionary(Of String, String)

    '                        documentos_.Add("nombreArchivo", campo_.Item("t_NombreArchivo"))

    '                        documentos_.Add("nombreDocumento", campo_.Item("t_NombreDocumento"))

    '                        'documentos_.Add("tipoDocumento", campo_.Item("t_Cve_TipoDocumento"))

    '                        documentos_.Add("referencia", campo_.Item("t_Referencia"))

    '                        documentos_.Add("extension", campo_.Item("t_ExtensionArchivo"))

    '                        documentos_.Add("descarga", campo_.Item("i_TipoDocumentoDescarga"))

    '                        listaDocumentos_.Add(documentos_)

    '                    Next

    '                    jsonDocumentos_ = New JavaScriptSerializer().Serialize(New With {listaDocumentos_})

    '                End If

    '            End If

    '        Else

    '            HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

    '        End If

    '    Else

    '        HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

    '    End If

    '    Return jsonDocumentos_

    'End Function

#End Region

End Class