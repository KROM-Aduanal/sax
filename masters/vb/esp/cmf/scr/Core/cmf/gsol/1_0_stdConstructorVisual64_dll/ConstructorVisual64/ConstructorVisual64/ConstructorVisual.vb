Imports gsol.ConstructorVisual64
Imports System.ComponentModel
Imports System.Configuration
Imports System.IO
Imports System.Linq.Expressions
Imports System.Reflection

Namespace gsol

    Public Class ConstructorVisual
        Implements IConstructorVisual

#Region "Atributos"

        Private _estatus As IConstructorVisual.EstatusConstruccion

        Private _tipo As IConstructorVisual.TipoAplicacion

#End Region

#Region "Constructores"

        Sub New()
            Me._estatus = IConstructorVisual.EstatusConstruccion.NoConstruido
            Me._tipo = IConstructorVisual.TipoAplicacion.NoDefinido
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public ReadOnly Property Estatus As IConstructorVisual.EstatusConstruccion _
            Implements IConstructorVisual.Estatus
            Get
                Return Me._estatus
            End Get
        End Property

        Public ReadOnly Property Tipo As IConstructorVisual.TipoAplicacion _
            Implements IConstructorVisual.Tipo
            Get
                Return Me._tipo
            End Get
        End Property

#End Region

#Region "Metodos"


        ''' <summary>
        ''' Construye el entorno visual, en base a los valores que conforman el espacio de trabajo autorizado para las credenciales de la sesión actual
        ''' </summary>
        ''' <param name="contenedor_">Contenedor principal de la aplicación</param>
        ''' <param name="manejadoresEventos_">Control propietario del contenedor principal de la aplicación</param>
        ''' <param name="sectoresEntorno_">Módulos autorizados</param>
        ''' <param name="tipoModulo_">Tipo de módulo a contruir: Gráfico, AccesoRápido, etc</param>
        ''' <param name="filtro_">Filtro a aplicar (opcional)</param>
        Public Function ConstruirEntornoVisual(
            ByVal contenedor_ As IList,
            ByVal manejadoresEventos_ As IList(Of System.Reflection.MethodInfo),
            ByVal sectoresEntorno_ As IDictionary(Of Int32, ISectorEntorno),
            ByVal tipoModulo_ As IConstructorVisual.TipoModulo,
            ByVal filtro_ As String
        ) As Object _
            Implements IConstructorVisual.ConstruirEntornoVisual

            Me._tipo = Me.ObtenerTipoAplicacion(contenedor_.GetType.Namespace)

            If sectoresEntorno_ IsNot Nothing And
               sectoresEntorno_.Values.Count > 0 Then

                'RECORRE LA COLECCIÓN DE MÓDULOS
                For Each sector_ As ISectorEntorno In sectoresEntorno_.Values

                    'SI EL MÓDULO NO ES DEL TIPO PASADO COMO ARGUMENTO, IGNORAR
                    If sector_.Identitificador <> tipoModulo_ Then
                        Continue For
                    End If

                    Dim permisos_ = sector_.Permisos.Values.ToList

                    'FILTRAR PERMISOS (OPCIONAL)
                    'Esta linea permite mostrar los quickbuttons, pedrobm, 03/2017
                    If filtro_ IsNot Nothing Then
                        permisos_ = permisos_.Where(Function(item) item.Descripcion = filtro_).ToList
                    End If

                    'RECORRE LA COLECCIÓN DE ENTIDADES
                    For Each permiso_ As IPermisos In permisos_

                        'SI ES UNA ENTIDAD DEPENDIENTE... IGNORAR
                        If permiso_.Dependencia <>
                            IConstructorVisual.TipoEntidad.Independiente Then

                            Continue For

                        End If

                        'OBTENER ENTIDAD
                        Dim entidad_ As Object =
                            Me.ObtenerEntidad(permiso_, permisos_, manejadoresEventos_)

                        If entidad_ Is Nothing Then

                            Continue For

                        End If

                        If tipoModulo_ = IConstructorVisual.TipoModulo.AccesoRapido Then

                            'MsgBox("quick!")

                            Return entidad_

                        End If

                        'AGREGA LA ENTIDAD AL CONTENEDOR
                        contenedor_.Add(entidad_)

                    Next

                Next

                Me._estatus = IConstructorVisual.EstatusConstruccion.Construido

            End If

            Return Nothing

        End Function

        ''' <summary>
        ''' Agrega un control, a su entidad padre correspondiente
        ''' </summary>
        ''' <param name="controlContenedor_">Control padre</param>
        ''' <param name="controlHijo_">Control hijo</param>
        ''' <param name="nombreContenedor_">Identificador del contenedor (del control padre), donde se agregará el control hijo</param>
        Private Sub AgregarControl(
            ByVal controlContenedor_ As Object,
            ByVal controlHijo_ As Object,
            ByVal nombreContenedor_ As String
        )

            If controlHijo_ Is Nothing Then
                Exit Sub
            End If

            'EN CASO DE SER UN CONTENEDOR "COMPUESTO"...

            If nombreContenedor_.Contains(".") = True Then

                Dim contenedores_() As String = nombreContenedor_.Split(".")

                'RECORRE EL CONTENEDOR, HASTA LLEGAR AL ÚLTIMO NIVEL ASIGNABLE
                For indiceContenedor_ As Integer = 0 To contenedores_.Length - 2

                    controlContenedor_ =
                        Interaction.CallByName(controlContenedor_, contenedores_(indiceContenedor_), CallType.Get)
                Next

                nombreContenedor_ = contenedores_(contenedores_.Length - 1)

            End If

            'OBTIENE EL OBJETO CONTENEDOR, DEL CONTROL PADRE
            Dim contenedor_ As Object =
                controlContenedor_.GetType.GetProperty(nombreContenedor_).GetValue(controlContenedor_, Nothing)

            If contenedor_ Is Nothing Then
                Exit Sub
            End If

            'AGREGA EL CONTROL HIJO, AL CONTENEDOR DEL CONTROL PADRE
            contenedor_.GetType.GetMethod(
                "Add",
                New Type() {controlHijo_.GetType}
            ).Invoke(
                contenedor_,
                New Object() {controlHijo_}
            )
        End Sub

        ''' <summary>
        ''' Asigna el valor de una propiedad a un control
        ''' </summary>
        ''' <param name="instanciaControl_">Instancia del control al que se asignará la propiedad</param>
        ''' <param name="nombrePropiedad_">Nombre de la propiedad</param>
        ''' <param name="tipoPropiedad_">Tipo de la propiedad</param>
        ''' <param name="valorPropiedad_">Valor de la propiedad</param>
        Private Sub AsignarPropiedad(
            ByVal instanciaControl_ As Object,
            ByVal nombrePropiedad_ As String,
            ByVal tipoPropiedad_ As IConstructorVisual.TipoPropiedad,
            ByVal valorPropiedad_ As String
        )

            If tipoPropiedad_ = IConstructorVisual.TipoPropiedad.Null Then
                tipoPropiedad_ = System.Enum.Parse(GetType(IConstructorVisual.TipoPropiedad), nombrePropiedad_)
            End If

            Select Case tipoPropiedad_
                'PROPIEDAD: HABILITAR REORDENAMIENTO TABS | AJUSTE AUTOMÁTICO CONTENIDO BOTONES | VISIBILIDAD BARRA TÍTULO | 
                '           CONTRAÍDO | HABILITADO | ALTURA | NOMBRE | MINIMIZADO | TEXTO RIBBON | VISIBILIDAD BOTÓN CERRAR TAB | 
                '           ALTURA TAB | DESPLAZAMIENTO TAB | ESPACIADO TAB | TEXTO | DESCRIPCIÓN EMERGENTE | VISIBILIDAD
                Case IConstructorVisual.TipoPropiedad.AllowTabReorder,
                     IConstructorVisual.TipoPropiedad.AutoSizeContentButtons,
                     IConstructorVisual.TipoPropiedad.CaptionBarVisible,
                     IConstructorVisual.TipoPropiedad.Collapsed,
                     IConstructorVisual.TipoPropiedad.Enabled,
                     IConstructorVisual.TipoPropiedad.Height,
                     IConstructorVisual.TipoPropiedad.ImageIndex,
                     IConstructorVisual.TipoPropiedad.ImageKey,
                     IConstructorVisual.TipoPropiedad.Name,
                     IConstructorVisual.TipoPropiedad.Minimized,
                     IConstructorVisual.TipoPropiedad.OrbText,
                     IConstructorVisual.TipoPropiedad.SelectedImageIndex,
                     IConstructorVisual.TipoPropiedad.SelectedImageKey,
                     IConstructorVisual.TipoPropiedad.TabCloseButtonVisible,
                     IConstructorVisual.TipoPropiedad.TabHeight,
                     IConstructorVisual.TipoPropiedad.TabOffset,
                     IConstructorVisual.TipoPropiedad.TabSpacing,
                     IConstructorVisual.TipoPropiedad.Tag,
                     IConstructorVisual.TipoPropiedad.Text,
                     IConstructorVisual.TipoPropiedad.ToolTip,
                     IConstructorVisual.TipoPropiedad.Visible
                    'ESTABLECE EL VALOR DE LA PROPIEDAD EN LA INSTANCIA DEL CONTROL
                    '(SÓLO APLICA PARA PROPIEDADES CON TIPO DE DATO NATIVOS: String, Integer, Double, Boolean, etc)
                    Interaction.CallByName(instanciaControl_, nombrePropiedad_, CallType.Set, valorPropiedad_)

                Case Else
                    'CONSTRUYE UN OBJETO QUE REPRESENTA EL VALOR DE UNA PROPIEDAD DEL CONTROL
                    Dim propiedad_ = Me.ObtenerPropiedad(tipoPropiedad_, valorPropiedad_)

                    'bautismen()

                    If propiedad_ Is Nothing Then
                        Exit Select
                    End If

                    'ESTABLECE EL VALOR DE LA PROPIEDAD EN LA INSTANCIA DEL CONTROL
                    Interaction.CallByName(instanciaControl_, nombrePropiedad_, CallType.Set, propiedad_)
            End Select

        End Sub

        ''' <summary>
        ''' Asigna el valor de una propiedad a un control
        ''' </summary>
        ''' <param name="instanciaControl_">Instancia del control al que se asignará la propiedad</param>
        ''' <param name="nombrePropiedad_">Nombre de la propiedad</param>
        ''' <param name="valorPropiedad_">Valor de la propiedad</param>
        Private Sub AsignarPropiedadCompuesta(
            ByVal instanciaControl_ As Object,
            ByVal nombrePropiedad_ As String,
            ByVal valorPropiedad_ As String
        )
            Dim propiedad_() As String = nombrePropiedad_.Split(".")
            Dim instanciaPropiedad_ As Object = instanciaControl_

            'RECORRE LA PROPIEDAD, HASTA LLEGAR AL ÚLTIMO NIVEL ASIGNABLE
            For indicePropiedad_ As Integer = 0 To propiedad_.Length - 2
                instanciaPropiedad_ = Interaction.CallByName(instanciaPropiedad_, propiedad_(indicePropiedad_), CallType.Get)
            Next

            'ESTABLECE EL VALOR DEL PROPIEDAD EN LA INSTANCIA DE LA PROPIEDAD COMPUESTA
            Me.AsignarPropiedad(instanciaPropiedad_, propiedad_(propiedad_.Length - 1), IConstructorVisual.TipoPropiedad.Null, valorPropiedad_)
        End Sub

        ''' <summary>
        ''' Construye una control, a partir de su especificación
        ''' </summary>
        ''' <param name="permiso_">Objeto que contiene las especificaciones del control a construir</param>
        ''' <param name="manejadoresEventos_">Control propietario del contenedor principal de la aplicación</param>
        ''' <returns>Objeto que representa el control de un módulo de aplicación</returns>
        Private Function CrearControl(
            ByVal permiso_ As IPermisos,
            ByVal manejadoresEventos_ As List(Of MethodInfo)
        ) As Object

            Dim control_ As Object = Nothing

            'RECORRE LA COLECCIÓN DE ENTIDADES
            For Each entidad_ In permiso_.Entidades.Values

                'RECORRE LA COLECCIÓN DE ATRIBUTOS DE CADA ENTIDAD
                For Each atributo_ In entidad_.Atributos.Values

                    'EN CASO DE SER UNA PROPIEDAD "COMPUESTA"...
                    If atributo_.Descripcion.Contains(".") = True Then
                        Me.AsignarPropiedadCompuesta(control_, atributo_.Descripcion, atributo_.Valor)
                        Continue For
                    End If

                    'DETERMINA EL TIPO DE PROPIEDAD/ACCIÓN
                    Dim tipoPropiedad_ As Object = System.Enum.Parse(GetType(IConstructorVisual.TipoPropiedad), atributo_.Descripcion)

                    Select Case tipoPropiedad_
                        'PROPIEDAD: TIPO
                        Case IConstructorVisual.TipoPropiedad.Type
                            'bautismen()

                            'CREA UNA INSTANCIA DEL TIPO DE CONTROL ESPECIFICADO
                            control_ = Me.ObtenerInstancia(permiso_.NombreEnsamblado, atributo_.Valor)

                            If control_ Is Nothing Then

                                Exit For

                            End If

                        Case Else
                            'ESTABLECE EL VALOR DEL PROPIEDAD EN LA INSTANCIA DEL CONTROL
                            Me.AsignarPropiedad(control_, atributo_.Descripcion, tipoPropiedad_, atributo_.Valor)

                    End Select

                Next

                'RECORRE LA COLLECIÓN DE EVENTOS DE CADA ENTIDAD
                For Each evento_ In entidad_.Eventos.Values

                    Dim tipoEvento_ = System.Enum.Parse(GetType(IConstructorVisual.TipoEvento), evento_.Descripcion)

                    Me.VincularEvento(control_, evento_.Descripcion, tipoEvento_, evento_.Valor, manejadoresEventos_)

                Next

            Next

            Return control_

        End Function

        ''' <summary>
        ''' Construye - recursivamente - una entidad y sus dependencias, a partir de su especificación
        ''' </summary>
        ''' <param name="permiso_">Objeto que contiene las especificaciones, a partir de las cuales se construirá la entidad</param>
        ''' <param name="permisos_">Colección (original) de permisos de un sector</param>
        ''' <param name="manejadoresEventos_">Control propietario del contenedor principal de la aplicación</param>
        ''' <returns>Objeto que representa una entidad de un módulo de aplicación</returns>
        Private Function ObtenerEntidad(
            ByVal permiso_ As IPermisos,
            ByVal permisos_ As List(Of IPermisos),
            ByVal manejadoresEventos_ As List(Of MethodInfo)
        ) As Object

            'Dim oki_ As MethodInfo

            'oki_.Attributes.
            ' bautismen()

            'CONSTRUYE LA ENTIDAD
            Dim entidad_ As Object = Me.CrearControl(permiso_, manejadoresEventos_)

            If entidad_ Is Nothing Then
                Return Nothing
            End If

            'OBTIENE SUS DEPENDENCIAS
            Dim dependencias_ As List(Of IPermisos) = permisos_.Where(Function(item) item.Dependencia = permiso_.Identificador).ToList

            'RECORRE LA COLLECCIÓN DE ENTIDADES DEPENDIENTES
            For Each dependencia_ As IPermisos In dependencias_
                'AGREGA LA ENTIDAD DEPENDIENTE, A SU ENTIDAD PADRE CORRESPONDIENTE
                Me.AgregarControl(entidad_, Me.ObtenerEntidad(dependencia_, permisos_, manejadoresEventos_), permiso_.NombreContenedor)

            Next

            Return entidad_

        End Function

        ''' <summary>
        ''' Construye una instancia de un control, a partir de un tipo y ensamblado
        ''' </summary>
        ''' <param name="nombreEnsamblado_">Nombre del ensamblado</param>
        ''' <param name="tipoControl_">Tipo del control a generar</param>
        ''' <returns>Objeto que representa la instancia del control</returns>
        Private Function ObtenerInstancia(
            ByVal nombreEnsamblado_ As String,
            ByVal tipoControl_ As String
        ) As Object
            Dim control_ As Object = Nothing

            'CARGA EL EMSAMBLADO
            'Dim ensamblado_ As Assembly = Assembly.LoadFrom(ConfigurationManager.AppSettings("RutaDependencias") & nombreEnsamblado_)
            'C:\Sax\v1.0\vb\esp\Src\Apps\Solutions\kb\Main\ID000SistemaBase\GrupoSolium..\..\..\..\..\..\..\..\Libs\Bin\Apps\Solutions\kb\Settings\Dependencies

            'Dim path_ = Environment.ExpandEnvironmentVariables("%saxpath%\masters\vb\esp\sax2.0\bin\Foreign\")

            'Dim saxpath_ As String = Environment.GetEnvironmentVariable("saxpath")

            Dim ensamblado_ As Assembly =
                 Assembly.LoadFrom("..\..\..\..\..\masters\vb\esp\sax2.0\bin\Foreign\" & nombreEnsamblado_)
            'Assembly.LoadFrom(saxpath_ & "\masters\vb\esp\sax2.0\bin\Foreign\" & nombreEnsamblado_)


            '
            'OBTIENE EL TIPO DE CONTROL A GENERAR
            Dim tipo_ As Type = ensamblado_.GetType(tipoControl_)

            'OBTIENE UNA INSTANCIA DEL CONTROL
            control_ = Activator.CreateInstance(tipo_)

            Return control_

        End Function

        ''' <summary>
        ''' Construye un objeto que representa el valor de una propiedad de un control
        ''' </summary>
        ''' <param name="tipoPropiedad_">Tipo de la propiedad a construir</param>
        ''' <param name="valorPropiedad_">Valor de la propiedad a contruir</param>
        Private Function ObtenerPropiedad(
            ByVal tipoPropiedad_ As IConstructorVisual.TipoPropiedad,
            ByVal valorPropiedad_ As String
        ) As Object

            Select Case Me._tipo
                Case IConstructorVisual.TipoAplicacion.Escritorio

                    Select Case tipoPropiedad_
                        'PROPIEDAD: ANCLA
                        Case IConstructorVisual.TipoPropiedad.Anchor
                            Return System.Enum.Parse(GetType(System.Windows.Forms.AnchorStyles), valorPropiedad_)
                            'PROPIEDAD: COLOR FONDO | COLOR
                        Case IConstructorVisual.TipoPropiedad.BackColor,
                             IConstructorVisual.TipoPropiedad.BackHighColor,
                             IConstructorVisual.TipoPropiedad.BackLowColor,
                             IConstructorVisual.TipoPropiedad.Color,
                             IConstructorVisual.TipoPropiedad.ForeColor
                            Return System.Drawing.Color.FromName(valorPropiedad_)
                            'PROPIEDAD: ESTILO BORDE
                        Case IConstructorVisual.TipoPropiedad.BorderStyle
                            Return System.Enum.Parse(GetType(System.Windows.Forms.BorderStyle), valorPropiedad_)
                            'PROPIEDAD: ACOPLAMIENTO
                        Case IConstructorVisual.TipoPropiedad.Dock
                            Return System.Enum.Parse(GetType(System.Windows.Forms.DockStyle), valorPropiedad_)
                            'PROPIEDAD: ÍCONO
                        Case IConstructorVisual.TipoPropiedad.Icon
                            'Return New System.Drawing.Icon(ConfigurationManager.AppSettings("RutaRecursos") & valorPropiedad_)
                            'Return New System.Drawing.Icon(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "..\..\..\..\Settings\Resources\" & valorPropiedad_)
                            'Dim saxpath_ As String = Environment.GetEnvironmentVariable("saxpath")

                            'Return New System.Drawing.Icon(Environment.GetEnvironmentVariable("saxpath") & "\settings\assets\resources\" & valorPropiedad_)
                            Return New System.Drawing.Icon("..\..\..\..\..\settings\assets\resources\" & valorPropiedad_)
                            'Assembly.LoadFrom("..\..\..\..\..\..\masters\vb\esp\sax2.0\bin\Foreign\" & nombreEnsamblado_)
                            ' "..\..\..\..\..\..\..\..\Libs\Bin\Apps\Solutions\kb\Settings\Dependencies\" 
                            '
                            'PROPIEDAD: IMÁGEN (NORMAL | GRANDE | PEQUEÑA) | IMÁGEN RIBBON |
                            '		   IMÁGENES TABS (NORMAL | DESHABILITADA | ACTIVA)
                        Case IConstructorVisual.TipoPropiedad.Image,
                             IConstructorVisual.TipoPropiedad.LargeImage,
                             IConstructorVisual.TipoPropiedad.OrbImage,
                             IConstructorVisual.TipoPropiedad.SmallImage,
                             IConstructorVisual.TipoPropiedad.TabCloseButtonImage,
                             IConstructorVisual.TipoPropiedad.TabCloseButtonImageDisabled,
                             IConstructorVisual.TipoPropiedad.TabCloseButtonImageHot
                            'Return System.Drawing.Image.FromFile(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "..\..\..\..\Settings\Resources\" & valorPropiedad_)
                            'Return System.Drawing.Image.FromFile(Environment.GetEnvironmentVariable("saxpath") & "\settings\assets\resources\" & valorPropiedad_)
                            Return System.Drawing.Image.FromFile("..\..\..\..\..\settings\assets\resources\" & valorPropiedad_)

                            'PROPIEDAD: LISTA IMÁGENES | LISTA IMÁGENES PEQUEÑAS | LISTA IMÁGENES GRANDES | LISTA IMÁGENES ESTADO
                        Case IConstructorVisual.TipoPropiedad.ImageList,
                             IConstructorVisual.TipoPropiedad.LargeImageList,
                             IConstructorVisual.TipoPropiedad.SmallImageList,
                             IConstructorVisual.TipoPropiedad.StateImageList

                            Dim imagenes_() As String = valorPropiedad_.Split(",")

                            Dim listaImagenes_ As System.Windows.Forms.ImageList = New System.Windows.Forms.ImageList()

                            For Each imagen_ As String In imagenes_
                                listaImagenes_.Images.Add(
                                    Me.ObtenerPropiedad(
                                        IIf(
                                            imagen_.EndsWith(".ico"),
                                            IConstructorVisual.TipoPropiedad.Icon,
                                            IConstructorVisual.TipoPropiedad.Image
                                        ),
                                        imagen_
                                    )
                                )
                            Next

                            Return listaImagenes_
                            'PROPIEDAD: ESTILO LAYOUT
                        Case IConstructorVisual.TipoPropiedad.LayoutStyle
                            Return System.Enum.Parse(GetType(Guifreaks.Navisuite.NaviLayoutStyle), valorPropiedad_)
                            'PROPIEDAD: UBICACIÓN | TAMAÑO (MÁXIMO | MÍNIMO)
                        Case IConstructorVisual.TipoPropiedad.Location,
                             IConstructorVisual.TipoPropiedad.MaximumSize,
                             IConstructorVisual.TipoPropiedad.MinimumSize
                            Dim posicionX_ As Integer = valorPropiedad_.Split(",")(0)
                            Dim posicionY_ As Integer = valorPropiedad_.Split(",")(1)

                            Return New System.Drawing.Point(posicionX_, posicionY_)
                            'PROPIEDAD: MODO TAMAÑO (MÁXIMO | MÍNIMO)
                        Case IConstructorVisual.TipoPropiedad.MaxSizeMode,
                             IConstructorVisual.TipoPropiedad.MinSizeMode
                            Return System.Enum.Parse(GetType(System.Windows.Forms.RibbonElementSizeMode), valorPropiedad_)
                            'PROPIEDAD: ESTILO RIBBON
                        Case IConstructorVisual.TipoPropiedad.OrbStyle
                            Return System.Enum.Parse(GetType(System.Windows.Forms.RibbonOrbStyle), valorPropiedad_)
                            'PROPIEDAD: RELLENO | MÁRGEN 
                        Case IConstructorVisual.TipoPropiedad.Padding,
                             IConstructorVisual.TipoPropiedad.TabsMargin
                            Return New System.Windows.Forms.Padding(valorPropiedad_)
                            'PROPIEDAD: TAMAÑO | TAMAÑO BOTÓN CERRAR TAB | TAMAÑO ÍCONO TAB
                        Case IConstructorVisual.TipoPropiedad.Size,
                             IConstructorVisual.TipoPropiedad.TabCloseButtonSize,
                             IConstructorVisual.TipoPropiedad.TabIconSize
                            Dim ancho_ As Integer = valorPropiedad_.Split(",")(0)
                            Dim alto_ As Integer = valorPropiedad_.Split(",")(1)

                            Return New System.Drawing.Size(ancho_, alto_)
                            'PROPIEDAD: ESTILO BOTÓN RIBBON
                        Case IConstructorVisual.TipoPropiedad.Style
                            Return System.Enum.Parse(GetType(System.Windows.Forms.RibbonButtonStyle), valorPropiedad_)
                            'PROPIEDAD: ALINEACIÓN TEXTO BOTÓN RIBBON
                        Case IConstructorVisual.TipoPropiedad.TextAlignment
                            Return System.Enum.Parse(GetType(System.Windows.Forms.RibbonItem.RibbonItemTextAlignment), valorPropiedad_)
                            'PROPIEDAD: COLOR TEMA
                        Case IConstructorVisual.TipoPropiedad.ThemeColor
                            Return System.Enum.Parse(GetType(System.Windows.Forms.RibbonTheme), valorPropiedad_)
                        Case Else
                            'TODO: Agregar casos de tipos de PROPIEDAD restantes
                            Return Nothing
                    End Select
                Case IConstructorVisual.TipoAplicacion.Web

                    Return Nothing

                Case Else

                    Return Nothing

            End Select
        End Function

        ''' <summary>
        ''' Establece el tipo de aplicación a construir
        ''' </summary>
        ''' <param name="nombreEnsamblado_">Nombre del ensamblado del contenedor principal de la aplicación</param>
        ''' <returns>Valor que representa el tipo de aplicación a construir: : Windows, Web, etc</returns>
        Private Function ObtenerTipoAplicacion(
            ByVal nombreEnsamblado_ As String
        ) As IConstructorVisual.TipoAplicacion

            Select Case nombreEnsamblado_
                Case IConstructorVisual.TipoAplicacion.Escritorio.Descripcion
                    Return IConstructorVisual.TipoAplicacion.Escritorio
                Case IConstructorVisual.TipoAplicacion.Web.Descripcion
                    Return IConstructorVisual.TipoAplicacion.Web
                Case Else
                    Return IConstructorVisual.TipoAplicacion.NoDefinido
            End Select
        End Function

        ''' <summary>
        ''' Vincula un evento a un control
        ''' </summary>
        ''' <param name="instanciaControl_">Instancia del control al que se vinculará el evento</param>
        ''' <param name="nombreEvento_">Nombre del evento</param>
        ''' <param name="tipoEvento_">Tipo de evento</param>
        ''' <param name="codigoEvento_">Código a ejecutarse en el evento</param>
        ''' <param name="manejadoresEventos_">Control propietario del contenedor principal de la aplicación</param>
        Private Sub VincularEvento(
            ByVal instanciaControl_ As Object,
            ByVal nombreEvento_ As String,
            ByVal tipoEvento_ As IConstructorVisual.TipoEvento,
            ByVal codigoEvento_ As String,
            ByVal manejadoresEventos_ As List(Of MethodInfo)
        )

            Select Case Me._tipo
                Case IConstructorVisual.TipoAplicacion.Escritorio

                    Select Case tipoEvento_
                        'EVENTO: CLICK
                        Case IConstructorVisual.TipoEvento.Click,
                             IConstructorVisual.TipoEvento.NodeMouseClick
                            'OBTIENE LA INFORMACIÓN DEL EVENTO DEL CONTROL
                            Dim informacionEvento_ As EventInfo = instanciaControl_.GetType.GetEvent(nombreEvento_)
                            'OBTIENE LA INFORMACIÓN DEL MÉTODO QUE SERÁ ASOCIADO AL EVENTO DEL CONTROL
                            Dim informacionMetodo_ As MethodInfo = manejadoresEventos_.Find(Function(item) item.Name = IConstructorVisual.TipoEvento.Click.Descripcion)

                            If informacionMetodo_ Is Nothing Then
                                Exit Select
                            End If

                            'CREA UN DELEGADO A PARTIR DE LA INFORMACIÓN DEL MÉTODO QUE SERÁ ASOCIADO AL EVENTO DEL CONTROL
                            '(ESTE MÉTODO DEBERÁ ESTAR ESPECIFICADO EN EL PROPIETARIO DEL CONTENEDOR PRINCIPAL DE LA APLICACIÓN)
                            Dim delegado_ As System.Delegate = System.Delegate.CreateDelegate(informacionEvento_.EventHandlerType, Nothing, informacionMetodo_)

                            instanciaControl_.Tag = codigoEvento_
                            'VINCULA EL EVENTO DEL PROPIETARIO DEL CONTENEDOR PRINCIPAL DE LA APLICACIÓN, AL CONTROL
                            informacionEvento_.AddEventHandler(instanciaControl_, delegado_)
                        Case Else
                            Exit Select
                    End Select
                Case IConstructorVisual.TipoAplicacion.Web
                    Exit Select
                Case Else
                    Exit Select
            End Select
        End Sub

#End Region

    End Class

End Namespace
