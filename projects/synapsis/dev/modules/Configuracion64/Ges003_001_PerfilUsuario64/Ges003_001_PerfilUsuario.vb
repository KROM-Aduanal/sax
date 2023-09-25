Imports System.Security.Cryptography
Imports Gsol.BaseDatos.Operaciones
Imports Gsol.krom.controladores
Imports System.Web.Services
Imports Gsol.seguridad
Imports System.Web

Public Class Ges003_001_PerfilUsuario
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private _organismo As Organismo

    Private _controladorWeb As ControladorWeb

    'Atributos usuario
    Private _nombreUsuario As String

    Private _paternoUsuario As String

    Private _maternoUsuario As String

    Private _correoUsuario As String

    Private _empresaUsuario As String

    Private _telefonoUsuario As String

    Private _cumpleanoUsuario As String

    Private _imagenUsuario As String

    Private _empresasUsuario As Dictionary(Of String, String)

#End Region

#Region "Propiedades"

    'Propiedades usuario
    ReadOnly Property NombreUsuario

        Get

            Return _nombreUsuario

        End Get

    End Property

    ReadOnly Property ApellidoPaternoUsuario

        Get

            Return _paternoUsuario

        End Get

    End Property

    ReadOnly Property ApellidoMaternoUsuario

        Get

            Return _maternoUsuario

        End Get

    End Property

    ReadOnly Property CorreoUsuario

        Get

            Return _correoUsuario

        End Get

    End Property

    ReadOnly Property EmpresaUsuario

        Get

            Return _empresaUsuario

        End Get

    End Property

    ReadOnly Property TelefonoUsuario

        Get

            Return _telefonoUsuario

        End Get

    End Property

    ReadOnly Property CumpleanosUsuario

        Get

            Return _cumpleanoUsuario

        End Get

    End Property

    ReadOnly Property ImagenUsuario

        Get

            Return _imagenUsuario

        End Get

    End Property

    ReadOnly Property EmpresasUsuario As Dictionary(Of String, String)

        Get

            Return _empresasUsuario

        End Get

    End Property

#End Region

#Region "Constructores"

    Sub New()

        _organismo = New Organismo

        'Constructores usuario
        _nombreUsuario = ""

        _paternoUsuario = ""

        _maternoUsuario = ""

        _correoUsuario = ""

        _empresaUsuario = ""

        _telefonoUsuario = ""

        _cumpleanoUsuario = ""

        _imagenUsuario = ""

    End Sub

#End Region

#Region "Metodos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("ControladorWeb") Is Nothing Then

            _controladorWeb = Session("ControladorWeb")

            DatosUsuario()

        Else

            Response.Redirect("http://web.kromaduanal.com/default.aspx")

        End If

    End Sub

    <WebMethod>
    Public Shared Function ActualizarUsuario(ByVal data_ As Object)

        Dim controladorWeb_ As ControladorWeb

        If Not HttpContext.Current.Session("ControladorWeb") Is Nothing Then

            controladorWeb_ = HttpContext.Current.Session("ControladorWeb")

            If Not controladorWeb_ Is Nothing Then

                Dim espacioTrabajo_ As EspacioTrabajo = HttpContext.Current.Session("EspacioTrabajoExtranet")

                If Not espacioTrabajo_ Is Nothing Then

                    Dim organismo_ As New Organismo

                    Dim perfilUsuario_ As New Ges003_001_PerfilUsuario

                    Dim i_Cve_Usuario_ As Integer = 0

                    'Se obtiene el i_Cve_Persona para actualizar los datos del usuario
                    'Dim ioperacionesnew_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                    Dim ioperacionesnew_ As IOperacionesCatalogo

                    ioperacionesnew_ = organismo_.EnsamblaModulo("Usuarios")

                    Dim usuario As Dictionary(Of String, String) = perfilUsuario_.InformacionUsuario()

                    If usuario.Count > 0 Then

                        i_Cve_Usuario_ = usuario.Item("id")

                    Else

                        i_Cve_Usuario_ = 0

                    End If

                    With ioperacionesnew_

                        '.EspacioTrabajo = controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo

                        '.ClausulasLibres = " AND i_Cve_Usuario = " & i_Cve_Usuario_

                        .EspacioTrabajo = espacioTrabajo_

                        .ClausulasLibres = " AND i_Cve_Usuario = " & i_Cve_Usuario_

                    End With

                    ioperacionesnew_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    ioperacionesnew_.GenerarVista()

                    If ioperacionesnew_.Vista.Tables.Count > 0 Then

                        If ioperacionesnew_.Vista.Tables(0).Rows.Count > 0 Then

                            Dim i_Cve_Persona_ As Integer = ioperacionesnew_.Vista(0, "i_Cve_Persona", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            'Valida si hay registros en Cat001Personas
                            Dim tieneRegistroPersonas As Boolean = ExisteRegistro("i_Cve_Persona", "Vt000Personas", "Cat001Personas", "Ve000IUPersonas", " AND i_Cve_Persona = " & i_Cve_Persona_)

                            'Decide si inserta o actualiza en la tabla Cat001Personas
                            If tieneRegistroPersonas Then

                                'Actualiza
                                'Dim ioperacionesUpdate_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                                Dim ioperacionesUpdate_ As IOperacionesCatalogo

                                ioperacionesUpdate_ = organismo_.EnsamblaModulo("Usuarios")

                                ioperacionesUpdate_.IdentificadorCatalogo = "i_Cve_Persona"

                                ioperacionesUpdate_.OperadorCatalogoConsulta = "Vt000Personas"

                                ioperacionesUpdate_.TablaEdicion = "Cat001Personas"

                                ioperacionesUpdate_.VistaEncabezados = "Ve000IUPersonas"

                                ioperacionesUpdate_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                                ioperacionesUpdate_.PreparaCatalogo()

                                Dim listaDatosPersona_ As New List(Of String) From {"t_Nombre", "t_ApellidoPaterno", "t_ApellidoMaterno"}

                                ioperacionesUpdate_.CampoPorNombre("t_Nombre") = data_("nombre_usuario")

                                ioperacionesUpdate_.CampoPorNombre("t_ApellidoPaterno") = data_("paterno_usuario")

                                ioperacionesUpdate_.CampoPorNombre("t_ApellidoMaterno") = If(data_("materno_usuario") = Nothing, " ", data_("materno_usuario"))

                                organismo_.OptimizaOperacion(ioperacionesUpdate_, listaDatosPersona_, IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                                If ioperacionesUpdate_.Modificar(i_Cve_Persona_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    HttpContext.Current.Session("DatosUsuario").item("Nombre") = data_("nombre_usuario") & " " & data_("paterno_usuario")

                                End If

                            End If

                            'Valida si hay registros en Reg001InformacionPersonal
                            Dim tieneRegistroInformacionPersonal As Boolean = ExisteRegistro("i_Cve_InformacionPersonal", "Vt001InformacionPersonal", "Reg001InformacionPersonal", "Ve001IUInformacionPersonal", " AND i_Cve_Persona = " & i_Cve_Persona_)

                            If tieneRegistroInformacionPersonal Then

                                'Busca la primary key
                                'Dim llaveInformacionPersonal_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone
                                Dim llaveInformacionPersonal_ As IOperacionesCatalogo

                                llaveInformacionPersonal_ = organismo_.EnsamblaModulo("Usuarios")

                                llaveInformacionPersonal_.IdentificadorCatalogo = "i_Cve_InformacionPersonal"

                                llaveInformacionPersonal_.OperadorCatalogoConsulta = "Vt001InformacionPersonal"

                                llaveInformacionPersonal_.TablaEdicion = "Reg001InformacionPersonal"

                                llaveInformacionPersonal_.VistaEncabezados = "Ve001IUInformacionPersonal"

                                With llaveInformacionPersonal_

                                    '.EspacioTrabajo = controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo

                                    .EspacioTrabajo = espacioTrabajo_

                                    .ClausulasLibres = " AND i_Cve_Persona = " & i_Cve_Persona_

                                End With

                                llaveInformacionPersonal_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                                llaveInformacionPersonal_.GenerarVista()

                                If organismo_.TieneResultados(llaveInformacionPersonal_) Then

                                    Dim i_Cve_InformacionPersonal_ As Integer = llaveInformacionPersonal_.Vista(0, "i_Cve_InformacionPersonal", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                    'Actualiza
                                    'Dim updateInformacionPersonal_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                                    Dim updateInformacionPersonal_ As IOperacionesCatalogo

                                    updateInformacionPersonal_ = organismo_.EnsamblaModulo("Usuarios")

                                    updateInformacionPersonal_.IdentificadorCatalogo = "i_Cve_InformacionPersonal"

                                    updateInformacionPersonal_.OperadorCatalogoConsulta = "Vt001InformacionPersonal"

                                    updateInformacionPersonal_.TablaEdicion = "Reg001InformacionPersonal"

                                    updateInformacionPersonal_.VistaEncabezados = "Ve001IUInformacionPersonal"

                                    updateInformacionPersonal_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                                    updateInformacionPersonal_.PreparaCatalogo()

                                    Dim listaDatosPersona_ As New List(Of String) From {"t_TelefonoUno", "i_Cve_Persona", "i_Cve_Estado", "i_Cve_Tipo", "f_FechaNacimiento"}

                                    updateInformacionPersonal_.CampoPorNombre("t_TelefonoUno") = If(data_("telefono_usuario") = Nothing, Nothing, data_("telefono_usuario").trim())

                                    updateInformacionPersonal_.CampoPorNombre("i_Cve_Persona") = i_Cve_Persona_

                                    updateInformacionPersonal_.CampoPorNombre("i_Cve_Estado") = 1

                                    updateInformacionPersonal_.CampoPorNombre("i_Cve_Tipo") = 0

                                    updateInformacionPersonal_.CampoPorNombre("f_FechaNacimiento") = If(data_("nacimiento_usuario") = Nothing, "", data_("nacimiento_usuario"))

                                    organismo_.OptimizaOperacion(updateInformacionPersonal_, listaDatosPersona_, IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                                    If updateInformacionPersonal_.Modificar(i_Cve_InformacionPersonal_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    End If

                                End If

                            Else

                                'Inserta
                                'Dim iOperacionesInsert_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                                Dim iOperacionesInsert_ As IOperacionesCatalogo

                                iOperacionesInsert_ = organismo_.EnsamblaModulo("Usuarios")

                                iOperacionesInsert_.IdentificadorCatalogo = "i_Cve_InformacionPersonal"

                                iOperacionesInsert_.OperadorCatalogoConsulta = "Vt001InformacionPersonal"

                                iOperacionesInsert_.TablaEdicion = "Reg001InformacionPersonal"

                                iOperacionesInsert_.VistaEncabezados = "Ve001IUInformacionPersonal"

                                iOperacionesInsert_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                                iOperacionesInsert_.PreparaCatalogo()

                                Dim listaDatosPersona_ As New List(Of String) From {"t_TelefonoUno", "i_Cve_Persona", "i_Cve_Estado", "i_Cve_Tipo", "f_FechaNacimiento"}

                                iOperacionesInsert_.CampoPorNombre("t_TelefonoUno") = data_("telefono_usuario")

                                iOperacionesInsert_.CampoPorNombre("i_Cve_Persona") = i_Cve_Persona_

                                iOperacionesInsert_.CampoPorNombre("i_Cve_Estado") = 1

                                iOperacionesInsert_.CampoPorNombre("i_Cve_Tipo") = 0

                                iOperacionesInsert_.CampoPorNombre("f_FechaNacimiento") = If(data_("nacimiento_usuario") = Nothing, "", data_("nacimiento_usuario"))

                                organismo_.OptimizaOperacion(iOperacionesInsert_, listaDatosPersona_, IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                If iOperacionesInsert_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                End If

                            End If

                            Return "{""code"": ""200"", ""message"": ""Los datos se actualizaron correctamente""}"
                        Else

                            Return "{""code"": ""400"", ""message"": ""No se pudieron actulizar los datos""}"

                        End If

                    End If

                Else

                    HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

                End If

            Else

                HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

            End If

        Else

            HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

        End If

    End Function

    Private Shared Function ExisteRegistro(ByVal llave_ As String,
                                           ByVal vt_ As String,
                                           ByVal tabla_ As String,
                                           ByVal ve_ As String,
                                           ByVal clausula_ As String)

        Dim tieneRegistro As Boolean = False

        Dim controladorWeb_ As ControladorWeb

        If Not HttpContext.Current.Session("ControladorWeb") Is Nothing Then

            controladorWeb_ = HttpContext.Current.Session("ControladorWeb")

            If Not controladorWeb_ Is Nothing Then

                Dim espacioTrabajo_ As EspacioTrabajo = HttpContext.Current.Session("EspacioTrabajoExtranet")

                If Not espacioTrabajo_ Is Nothing Then

                    Dim organismo_ As New Organismo

                    'Dim ioperacionesnew_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                    Dim ioperacionesnew_ As IOperacionesCatalogo

                    ioperacionesnew_ = organismo_.EnsamblaModulo("Usuarios")

                    ioperacionesnew_.IdentificadorCatalogo = llave_

                    ioperacionesnew_.OperadorCatalogoConsulta = vt_

                    ioperacionesnew_.TablaEdicion = tabla_

                    ioperacionesnew_.VistaEncabezados = ve_

                    With ioperacionesnew_

                        '.EspacioTrabajo = controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo

                        .EspacioTrabajo = espacioTrabajo_

                        .ClausulasLibres = clausula_

                    End With

                    ioperacionesnew_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    ioperacionesnew_.GenerarVista()

                    If organismo_.TieneResultados(ioperacionesnew_) Then

                        Return True

                    Else

                        Return False

                    End If

                Else

                    HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

                End If

            Else

                HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

            End If

        Else

            HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

        End If

    End Function

    Public Sub DatosUsuario()

        If Not _controladorWeb Is Nothing Then

            Dim espacioTrabajo_ As EspacioTrabajo = Session("EspacioTrabajoExtranet")

            If Not espacioTrabajo_ Is Nothing Then

                Dim t_Nombre As String = ""

                Dim ioperacionesnew_ As IOperacionesCatalogo

                'Dim ioperacionesnew_ As IOperacionesCatalogo = _controladorWeb.EnlaceDatos.IOperaciones.Clone

                ioperacionesnew_ = _organismo.EnsamblaModulo("Usuarios")

                With ioperacionesnew_

                    '.EspacioTrabajo = _controladorWeb.EnlaceDatos.IOperaciones.EspacioTrabajo

                    '.ClausulasLibres = " AND t_usuario = '" & _controladorWeb.EnlaceDatos.IOperaciones.EspacioTrabajo.MisCredenciales.CredencialUsuario & "'"

                    .EspacioTrabajo = espacioTrabajo_

                    .ClausulasLibres = " AND t_usuario = '" & espacioTrabajo_.MisCredenciales.CredencialUsuario & "'"

                End With

                ioperacionesnew_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                ioperacionesnew_.GenerarVista()

                If ioperacionesnew_.Vista.Tables.Count > 0 Then

                    If ioperacionesnew_.Vista.Tables(0).Rows.Count > 0 Then

                        '_idUsuario = ioperacionesnew_.Vista(0, "i_Cve_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _nombreUsuario = ioperacionesnew_.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _paternoUsuario = ioperacionesnew_.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _maternoUsuario = ioperacionesnew_.Vista(0, "t_ApellidoMaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _correoUsuario = ioperacionesnew_.Vista(0, "t_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _empresaUsuario = ioperacionesnew_.Vista(0, "t_NombreEmpresa", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _telefonoUsuario = ioperacionesnew_.Vista(0, "t_TelefonoUno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _cumpleanoUsuario = ioperacionesnew_.Vista(0, "f_FechaNacimiento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        _imagenUsuario = ioperacionesnew_.Vista(0, "t_URLFotografia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        Dim request_ = HttpContext.Current.Request

                        Dim rutaImagenUsuario_ As String = Server.MapPath("/FrontEnd/Recursos/Imgs/" & Session("DatosUsuario").item("Imagen"))

                        Dim urlImagen_ = request_.Url.GetLeftPart(UriPartial.Authority) & request_.ApplicationPath & "FrontEnd/Recursos/Imgs/" & Session("DatosUsuario").item("Imagen")

                        urlImagen_ = urlImagen_.Replace(":8083", "")

                        Dim rutaImagenAvatar_ As String = "/FrontEnd/Recursos/Imgs/avatarkrom.png"

                        If Session("DatosUsuario").item("Imagen") <> "" Then

                            If System.IO.File.Exists(rutaImagenUsuario_) Then

                                _imagenUsuario = urlImagen_

                            Else

                                _imagenUsuario = rutaImagenAvatar_

                            End If

                        Else

                            _imagenUsuario = rutaImagenAvatar_

                        End If

                        'If _imagenUsuario = "" Then

                        '    _imagenUsuario = "./Componentes/dist/img/avatarkrom.png"

                        'Else

                        '    _imagenUsuario = "./Componentes/imgs/" & _imagenUsuario

                        'End If

                    End If

                End If

            Else

                Response.Redirect("http://web.kromaduanal.com/default.aspx")

            End If

        Else

            Response.Redirect("http://web.kromaduanal.com/default.aspx")

        End If

    End Sub

    <WebMethod>
    Public Shared Function CambiarContrasena(ByVal data_ As Object)

        Dim controladorWeb_ As ControladorWeb

        If Not HttpContext.Current.Session("ControladorWeb") Is Nothing Then

            controladorWeb_ = HttpContext.Current.Session("ControladorWeb")

            If Not controladorWeb_ Is Nothing Then

                Dim espacioTrabajo_ As EspacioTrabajo = HttpContext.Current.Session("EspacioTrabajoExtranet")

                If Not espacioTrabajo_ Is Nothing Then

                    Dim metodo_ As SymmetricAlgorithm = New RijndaelManaged

                    Dim cifrado_ As ICifrado = New Cifrado256

                    Dim sistema_ As New Organismo

                    Dim i_Cve_Usuario_ As Integer = 0

                    Dim frmContrasena_ As String = ""

                    Dim dbContrasena_ As String = ""

                    'Dim ioperacionesnew_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                    Dim ioperacionesnew_ As IOperacionesCatalogo

                    ioperacionesnew_ = sistema_.EnsamblaModulo("Usuarios")

                    With ioperacionesnew_

                        '.EspacioTrabajo = controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo

                        '.ClausulasLibres = " AND t_usuario = '" & controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo.MisCredenciales.CredencialUsuario & "'"

                        .EspacioTrabajo = espacioTrabajo_

                        .ClausulasLibres = " AND t_usuario = '" & espacioTrabajo_.MisCredenciales.CredencialUsuario & "'"

                    End With

                    ioperacionesnew_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    ioperacionesnew_.GenerarVista()

                    If ioperacionesnew_.Vista.Tables.Count > 0 Then

                        If ioperacionesnew_.Vista.Tables(0).Rows.Count > 0 Then

                            i_Cve_Usuario_ = ioperacionesnew_.Vista(0, "i_Cve_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            dbContrasena_ = ioperacionesnew_.Vista(0, "t_Contrasena", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        End If

                    End If

                    'frmContrasena_ = cifrado_.CifraCadena(data_("contraseña_actual"), metodo_, sistema_.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

                    'dbContrasena_ = cifrado_.DescifraCadena(dbContrasena_, metodo_, sistema_.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

                    frmContrasena_ = cifrado_.CifraCadena(data_("contraseña_actual"), ICifrado.Metodos.SHA1)

                    If frmContrasena_ = dbContrasena_ Then

                        Dim tokens_ As New List(Of String) From {"Usuarios", "KardexPermisos"}

                        Dim cambioContrasena As Boolean = False

                        For Each token_ As String In tokens_

                            'Dim ioperacionesUpdate_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                            Dim ioperacionesUpdate_ As IOperacionesCatalogo

                            ioperacionesUpdate_ = sistema_.EnsamblaModulo(token_)

                            ioperacionesUpdate_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                            ioperacionesUpdate_.PreparaCatalogo()

                            Dim _lista As New List(Of String)

                            _lista.Add("t_Contrasena")

                            ioperacionesUpdate_.CampoPorNombre("t_Contrasena") = cifrado_.CifraCadena(data_("nueva_contraseña"), ICifrado.Metodos.SHA1)

                            sistema_.OptimizaOperacion(ioperacionesUpdate_, _lista, IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                            If ioperacionesUpdate_.Modificar(i_Cve_Usuario_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                                cambioContrasena = True

                            Else

                                cambioContrasena = False

                            End If

                        Next

                        If cambioContrasena Then

                            Return "{""code"": ""200"", ""message"": ""La contraseña se cambio correctamente""}"

                        Else

                            Return "{""code"": ""400"", ""message"": ""No se pudo actulizar la nueva contraseña""}"

                        End If

                    Else

                        Return "{""code"": ""400"", ""message"": ""La contraseña actual no es la correcta""}"

                    End If

                Else

                    HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

                End If

            Else

                HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

            End If

        Else

            HttpContext.Current.Response.Redirect("http://web.kromaduanal.com/default.aspx")

        End If

    End Function

    Public Function InformacionUsuario()

        Dim controladorWeb_ As ControladorWeb

        Dim datos_ As New Dictionary(Of String, String)

        If Not HttpContext.Current.Session("ControladorWeb") Is Nothing Then

            controladorWeb_ = HttpContext.Current.Session("ControladorWeb")

            If Not controladorWeb_ Is Nothing Then

                Dim organismo_ As New Organismo

                Dim i_Cve_Usuario_ As Integer = 0

                Dim ioperacionesnew_ As IOperacionesCatalogo

                Dim espacioTrabajo_ As EspacioTrabajo = Session("EspacioTrabajoExtranet")

                If Not espacioTrabajo_ Is Nothing Then

                    'Dim ioperacionesnew_ As IOperacionesCatalogo = controladorWeb_.EnlaceDatos.IOperaciones.Clone

                    ioperacionesnew_ = organismo_.EnsamblaModulo("Usuarios")

                    With ioperacionesnew_

                        '.EspacioTrabajo = controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo

                        '.ClausulasLibres = " AND t_usuario = '" & controladorWeb_.EnlaceDatos.IOperaciones.EspacioTrabajo.MisCredenciales.CredencialUsuario & "'"

                        .EspacioTrabajo = espacioTrabajo_

                        .ClausulasLibres = " AND t_usuario = '" & espacioTrabajo_.MisCredenciales.CredencialUsuario & "'"

                    End With

                    ioperacionesnew_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    ioperacionesnew_.GenerarVista()

                    If ioperacionesnew_.Vista.Tables.Count > 0 Then

                        If ioperacionesnew_.Vista.Tables(0).Rows.Count > 0 Then

                            'i_Cve_Usuario_ = ioperacionesnew_.Vista(0, "i_Cve_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                            datos_.Add("id", ioperacionesnew_.Vista(0, "i_Cve_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico))

                            datos_.Add("nombre_usuario", ioperacionesnew_.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico))

                            datos_.Add("apellidoPaterno_usuario", ioperacionesnew_.Vista(0, "t_ApellidoPaterno", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico))

                            'Dim dataView_ As New DataView(ioperacionesnew_.Vista.Tables(0))
                            'Dim tablaResumen_ As New DataTable
                            'tablaResumen_ = dataView_.ToTable(True, "t_NombreEmpresa")
                        End If

                    End If

                Else

                    Response.Redirect("http://web.kromaduanal.com/default.aspx")

                End If

            Else

                Response.Redirect("http://web.kromaduanal.com/default.aspx")

            End If

        Else

            Response.Redirect("http://web.kromaduanal.com/default.aspx")

        End If

        Return datos_

    End Function

#End Region

End Class
