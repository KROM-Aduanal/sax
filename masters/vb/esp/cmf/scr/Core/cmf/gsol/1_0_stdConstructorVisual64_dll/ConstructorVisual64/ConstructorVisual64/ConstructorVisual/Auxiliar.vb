Imports System.Collections.Generic
Imports System.Configuration
Imports System.Reflection

Namespace gsol.ConstructorVisual64

    Public Class Auxiliar

#Region "Atributos"

        Private Shared _gestorModulos As IList(Of Type) = New List(Of Type)()
        Private Shared _gestorQuickstarts As IDictionary(Of String, Integer) = New Dictionary(Of String, Integer)()

        Private Shared _gestosModulosFormularios As IList(Of String) = New List(Of String)()

#End Region

#Region "Métodos"

        'ByVal script_ As String, _

        Public Shared Function Inicializador(Of T)(
            ByVal espacioTrabajo_ As Object,
            ByVal operacion_ As IScript.TipoOperacion,
            ByVal control_ As Object,
            ByVal enviador_ As Object,
            ByVal etiqueta_ As String
        ) As Boolean


            'SE OBTIENE EL TIPO DEL MÓDULO A GESTIONAR
            Dim tipoControl_ As Type = control_.GetType

            'SI EL MÓDULO EXISTE EN EL GESTOR, SALIR

            If tipoControl_.FullName <> "Gsol.Modulos.GenericCatalog64.Frm000Generic" And _
               tipoControl_.FullName <> "Wma.Modules.GenericCatalog64.Frm000Generic" Then

                If _gestorModulos.Contains(tipoControl_) Then

                    Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(enviador_)

                    Dim contenedorMDI_ As Object = contenedorPrincipal_.Controls.Find(
                      ConfigurationManager.AppSettings("NombreContenedorMDI"), True)

                    'contenedorMDI_(0).gfPanelNavegacion.VisibleLargeButtons = 5

                    Dim formulario_ As Object = contenedorPrincipal_.Controls.Find(control_.Name, True)

                    If contenedorMDI_.Length = 0 Or
                       formulario_.Length = 0 Then

                        Return False

                    End If

                    '---Auxiliar.MostrarPanelQuickStartWinForm(Of T)(formulario_(0), Nothing)
                    contenedorMDI_(0).TabPages(formulario_(0)).Select()


                    'Dim contenedorPrincipal2_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(enviador_)

                    ' Dim contenedorNaviBar2_ As Object = contenedorPrincipal_.Controls.Find(
                    '   ConfigurationManager.AppSettings("gfPanelNavegacion"), True)

                    ' contenedorNaviBar2_.VisibleLargeButtons = 5


                    Return False

                End If

            Else

                If _gestosModulosFormularios.Contains(tipoControl_.FullName & "." & DirectCast(control_, System.Windows.Forms.Form).Text) Then

                    'Return False
                Else
                    _gestosModulosFormularios.Add(tipoControl_.FullName & "." & DirectCast(control_, System.Windows.Forms.Form).Text)

                End If

                Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(enviador_)

                'Dim contenedorNaviBar2_ As Object = contenedorPrincipal_.Controls.Find(
                '    ConfigurationManager.AppSettings("NombreNavBar"), True)
                'gfPanelNavegacion.VisibleLargeButtons = 2

                ' DirectCast(DirectCast(contenedorNaviBar2_, System.Windows.Forms.Control())(0), Guifreaks.Navisuite.NaviBar).VisibleLargeButtons = 2

            End If


            'AGREGAR EL MÓDULO AL GESTOR
            _gestorModulos.Add(tipoControl_)

            Select Case operacion_
                'OPERACION: MOSTRAR
                Case IScript.TipoOperacion.Mostrar
                    'ASOCIAR EVENTO QUE PERMITA ELIMINAR EL MÓDULO
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).FormClosed, AddressOf Auxiliar.EliminarModulo
                    control_.ShowDialog()
                    Exit Select

                    'OPERACION: AGREGAR
                Case IScript.TipoOperacion.Agregar
                    Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(enviador_)
                    Dim contenedorMDI_ As Object = contenedorPrincipal_.Controls.Find(
                        ConfigurationManager.AppSettings("NombreContenedorMDI"), True)

                    If contenedorMDI_.Length = 0 Then
                        Return False
                    End If

                    'AGREGAR MÓDULO (FORMULARIO) AL CONTENEDOR
                    contenedorMDI_(0).TabPages.Add(control_)

                    '                    contenedorMDI_(0).gfPanelNavegacion.VisibleLargeButtons = 5

                    'AGREGAR BARRA DE ACCESO RÁPIDO AL GESTOR
                    Auxiliar.GestionarPanelQuickStartWinForm(Of T)(
                        operacion_, tipoControl_.ToString, etiqueta_, control_,
                        contenedorPrincipal_, espacioTrabajo_
                    )

                    'ASOCIAR EVENTOS QUE PERMITA ELIMINAR/MOSTRAR/OCULTAR EL MÓDULO
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).FormClosed, AddressOf Auxiliar.EliminarModuloWinForm(Of T)
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).Leave, AddressOf Auxiliar.OcultarPanelQuickStartWinForm(Of T)
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).Shown, AddressOf Auxiliar.MostrarPanelQuickStartWinForm(Of T)
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).Enter, AddressOf Auxiliar.MostrarPanelQuickStartWinForm(Of T)
                    Exit Select
                Case Else
                    Exit Select
            End Select

            Return True
        End Function


        ''' <summary>
        ''' Gestiona instancias de módulos en un contenedor
        ''' </summary>
        ''' <typeparam name="T">Tipo del contenedor principal de la aplicación</typeparam>
        ''' <param name="espacioTrabajo_">Espacio de trabajo de la sesión</param>
        ''' <param name="operacion_">Operación a realizar con el módulo</param>
        ''' <param name="control_">Módulo a instanciar</param>
        ''' <param name="enviador_">Control origen del evento</param>
        ''' <param name="etiqueta_">Etiqueta del control origen del evento</param>
        ''' <returns>Verdadero, si se crea/muestra una instancia del módulo. Falso, en caso contrario</returns>
        Public Shared Function GestionarModuloWinForm(Of T)(
            ByVal espacioTrabajo_ As Object,
            ByVal operacion_ As IScript.TipoOperacion,
            ByVal control_ As Object,
            ByVal enviador_ As Object,
            ByVal etiqueta_ As String
        ) As Boolean

            'MsgBox("GestionarModuloWinForm")

            'SE OBTIENE EL TIPO DEL MÓDULO A GESTIONAR
            Dim tipoControl_ As Type = control_.GetType

            'SI EL MÓDULO EXISTE EN EL GESTOR, SALIR

            If tipoControl_.FullName <> "Gsol.Modulos.GenericCatalog64.Frm000Generic" And _
               tipoControl_.FullName <> "Wma.Modules.GenericCatalog64.Frm000Generic" Then

                If _gestorModulos.Contains(tipoControl_) Then

                    Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(enviador_)

                    Dim contenedorMDI_ As Object = contenedorPrincipal_.Controls.Find(
                      ConfigurationManager.AppSettings("NombreContenedorMDI"), True)

                    Dim formulario_ As Object = contenedorPrincipal_.Controls.Find(control_.Name, True)

                    If contenedorMDI_.Length = 0 Or
                       formulario_.Length = 0 Then

                        Return False

                    End If

                    '---Auxiliar.MostrarPanelQuickStartWinForm(Of T)(formulario_(0), Nothing)
                    contenedorMDI_(0).TabPages(formulario_(0)).Select()

                    Return False

                Else

                End If

            Else

                If _gestosModulosFormularios.Contains(tipoControl_.FullName & "." & DirectCast(control_, System.Windows.Forms.Form).Text) Then

                    'Return False
                Else
                    _gestosModulosFormularios.Add(tipoControl_.FullName & "." & DirectCast(control_, System.Windows.Forms.Form).Text)

                End If

            End If


            'AGREGAR EL MÓDULO AL GESTOR
            _gestorModulos.Add(tipoControl_)

            Select Case operacion_
                'OPERACION: MOSTRAR
                Case IScript.TipoOperacion.Mostrar
                    'ASOCIAR EVENTO QUE PERMITA ELIMINAR EL MÓDULO
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).FormClosed, AddressOf Auxiliar.EliminarModulo
                    control_.ShowDialog()
                    Exit Select

                    'OPERACION: AGREGAR
                Case IScript.TipoOperacion.Agregar
                    Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(enviador_)
                    Dim contenedorMDI_ As Object = contenedorPrincipal_.Controls.Find(
                        ConfigurationManager.AppSettings("NombreContenedorMDI"), True)

                    If contenedorMDI_.Length = 0 Then
                        Return False
                    End If

                    'AGREGAR MÓDULO (FORMULARIO) AL CONTENEDOR
                    contenedorMDI_(0).TabPages.Add(control_)
                    'AGREGAR BARRA DE ACCESO RÁPIDO AL GESTOR
                    Auxiliar.GestionarPanelQuickStartWinForm(Of T)(
                        operacion_, tipoControl_.ToString, etiqueta_, control_,
                        contenedorPrincipal_, espacioTrabajo_
                    )

                    'ASOCIAR EVENTOS QUE PERMITA ELIMINAR/MOSTRAR/OCULTAR EL MÓDULO
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).FormClosed, AddressOf Auxiliar.EliminarModuloWinForm(Of T)
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).Leave, AddressOf Auxiliar.OcultarPanelQuickStartWinForm(Of T)
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).Shown, AddressOf Auxiliar.MostrarPanelQuickStartWinForm(Of T)
                    AddHandler DirectCast(control_, System.Windows.Forms.Form).Enter, AddressOf Auxiliar.MostrarPanelQuickStartWinForm(Of T)
                    Exit Select
                Case Else
                    Exit Select
            End Select

            Return True
        End Function

        ''' <summary>
        ''' Elimina un módulo del gestor
        ''' </summary>
        ''' <typeparam name="T">Tipo del contenedor principal de la aplicación</typeparam>
        ''' <param name="sender">Módulo a eliminar</param>
        ''' <param name="e">Parámetros del evento</param>
        Private Shared Sub EliminarModuloWinForm(Of T)(
            ByVal sender As Object,
            ByVal e As EventArgs
        )
            'ELIMINA EL MÓDULO DEL GESTOR
            _gestorModulos.Remove(sender.GetType())
            'ELIMINAR LA BARRA DE ACCESO RÁPIDO DEL GESTOR
            Auxiliar.GestionarPanelQuickStartWinForm(Of T)(
                IScript.TipoOperacion.Eliminar, sender.GetType().ToString, Nothing, Nothing,
                Auxiliar.ObtenerContenedorPrincipal(Of T)(sender), Nothing
            )
        End Sub

        ''' <summary>
        ''' Elimina un módulo del gestor
        ''' </summary>
        ''' <param name="sender">Módulo a eliminar</param>
        ''' <param name="e">Parámetros del evento</param>
        Private Shared Sub EliminarModulo(
            ByVal sender As Object,
            ByVal e As EventArgs
        )
            'ELIMINA EL MÓDULO DEL GESTOR
            _gestorModulos.Remove(sender.GetType())
        End Sub

        ''' <summary>
        ''' Gestiona instancias de paneles de acceso rápido
        ''' </summary>
        ''' <typeparam name="T">Tipo del contenedor principal de la aplicación</typeparam>
        ''' <param name="operacion_">Operación a realizar con la barra de acceso rápido</param>
        ''' <param name="llave_">Clave que identifica unívocamente la barra de acceso rápido en el gestor</param>
        ''' <param name="etiqueta_">Etiqueta del control origen del evento</param>
        ''' <param name="modulo_">Módulo del cuál se deriva el panel de acceso rápido</param>
        ''' <param name="contenedorPrincipal_">Contenedor principal de la aplicación</param>
        ''' <param name="espacioTrabajo_">Espacio de trabajo de la sesión</param>
        Private Shared Sub GestionarPanelQuickStartWinForm(Of T)(
            ByVal operacion_ As IScript.TipoOperacion,
            ByVal llave_ As String,
            ByVal etiqueta_ As String,
            ByVal modulo_ As Object,
            ByVal contenedorPrincipal_ As Object,
            ByVal espacioTrabajo_ As Object
        )

            If contenedorPrincipal_ Is Nothing Then
                Exit Sub
            End If

            'OBTENER EL ÍNDICE DEL TAB QUE ALOJARÁ LOS PANELES DE ACCESO RÁPIDO
            Dim indice_ As Integer = ConfigurationManager.AppSettings("IndicePanelAccesoRapido")
            Dim menuRibbon_ As Object = contenedorPrincipal_.Controls.Find(
                ConfigurationManager.AppSettings("NombreRibbonMenu"), True)

            If menuRibbon_.Length = 0 Then
                Exit Sub
            End If

            menuRibbon_(0).SuspendUpdating()

            Select Case operacion_
                'OPERACIÓN: AGREGAR
                Case IScript.TipoOperacion.Agregar
                    If _gestorQuickstarts.ContainsKey(llave_) Then
                        menuRibbon_(0).Tabs(indice_).Panels(_gestorQuickstarts(llave_)).Visible = True
                        Exit Select
                    End If

                    Dim constructorVisual_ As ConstructorVisual = New ConstructorVisual()
                    Dim panelAccesoRapido_ As Object = constructorVisual_.ConstruirEntornoVisual(
                        menuRibbon_(0).Tabs(indice_).Panels, Nothing,
                        espacioTrabajo_.SectorEntorno,
                        IConstructorVisual.TipoModulo.AccesoRapido, llave_
                    )

                    If panelAccesoRapido_ Is Nothing Then
                        Exit Select
                    End If

                    Auxiliar.VincularEvento(panelAccesoRapido_.Items, modulo_)
                    'AGREGAR PANEL DE ACCESO RÁPIDO AL CONTENEDOR
                    menuRibbon_(0).Tabs(indice_).Panels.Add(panelAccesoRapido_)
                    'AGREGAR PANEL DE ACCESO RÁPIDO AL GESTOR
                    _gestorQuickstarts.Add(llave_, menuRibbon_(0).Tabs(indice_).Panels.Count - 1)
                    Exit Select
                    'OPERACIÓN: ELIMINAR
                Case IScript.TipoOperacion.Eliminar

                    'SI EL PANEL DE ACCESO RÁPIDO NO EXISTE EN EL GESTOR, SALIR
                    If Not _gestorQuickstarts.ContainsKey(llave_) Then
                        Exit Select
                    End If

                    'ELIMINAR PANEL DE ACCESO RÁPIDO DEL CONTENEDOR
                    menuRibbon_(0).Tabs(indice_).Panels.RemoveAt(_gestorQuickstarts(llave_))
                    'ELIMINAR  PANEL DE ACCESO RÁPIDO DEL CONTENEDOR
                    _gestorQuickstarts.Remove(llave_)
                    Exit Select
                Case Else
                    Exit Select
            End Select

            menuRibbon_(0).ResumeUpdating(True)
        End Sub

        ''' <summary>
        ''' Muestra un panel de acceso rápido
        ''' </summary>
        ''' <param name="sender">Panel a mostrar</param>
        ''' <param name="e">Parámetros del evento</param>
        Private Shared Sub MostrarPanelQuickStartWinForm(Of T)(
            ByVal sender As Object,
            ByVal e As EventArgs
        )
            Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(sender)
            Dim indice_ As Integer = ConfigurationManager.AppSettings("IndicePanelAccesoRapido")
            Dim menuRibbon_ As Object = contenedorPrincipal_.Controls.Find(
                ConfigurationManager.AppSettings("NombreRibbonMenu"), True)

            If menuRibbon_.Length = 0 Then
                Exit Sub
            End If

            menuRibbon_(0).SuspendUpdating()

            For Each item_ In menuRibbon_(0).Tabs(indice_).Panels
                item_.Visible = False
            Next

            menuRibbon_(0).ResumeUpdating(True)

            If Not _gestorQuickstarts.ContainsKey(sender.GetType.ToString) Then
                Exit Sub
            End If

            menuRibbon_(0).SuspendUpdating()
            menuRibbon_(0).Tabs(indice_).Panels(_gestorQuickstarts(sender.GetType.ToString)).Visible = True
            menuRibbon_(0).ResumeUpdating(True)
        End Sub

        ''' <summary>
        ''' Determina el contenedor principal de la aplicación, apartir de un control hijo (Bubbling)
        ''' </summary>
        ''' <typeparam name="T">Tipo del contenedor principal de la aplicación</typeparam>
        ''' <param name="control_">Control hijo apartir del cuál iniciará la búsqueda</param>
        ''' <returns>Objeto contenedor principal de la aplicación</returns>
        Private Shared Function ObtenerContenedorPrincipal(Of T)(
            ByVal control_ As Object
        ) As T

            'SI EL CONTROL ES DEL TIPO DEL CONTENEDOR PRINCIPAL DE LA APLICACIÓN...
            If TypeOf control_ Is T Then
                'DEVOLVER
                Return DirectCast(control_, T)
            End If

            'SI EL PADRE DEL CONTROL NO ES 'NULO'
            If control_.Parent IsNot Nothing Then
                'CONTINUAR LA BÚSQUEDA

                ' MsgBox("ObtenerContenedorPrincipal")
                Return ObtenerContenedorPrincipal(Of T)(control_.Parent)
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Oculta un panel de acceso rápido
        ''' </summary>
        ''' <param name="sender">Panel a ocultar</param>
        ''' <param name="e">Parámetros del evento</param>
        Private Shared Sub OcultarPanelQuickStartWinForm(Of T)(
            ByVal sender As Object,
            ByVal e As EventArgs
        )

            If Not _gestorQuickstarts.ContainsKey(sender.GetType.ToString) Then
                Exit Sub
            End If

            Dim contenedorPrincipal_ As Object = Auxiliar.ObtenerContenedorPrincipal(Of T)(sender)
            Dim indice_ As Integer = ConfigurationManager.AppSettings("IndicePanelAccesoRapido")
            Dim menuRibbon_ As Object = contenedorPrincipal_.Controls.Find(
                ConfigurationManager.AppSettings("NombreRibbonMenu"), True)

            If menuRibbon_.Length = 0 Then
                Exit Sub
            End If

            menuRibbon_(0).SuspendUpdating()
            menuRibbon_(0).Tabs(indice_).Panels(_gestorQuickstarts(sender.GetType.ToString)).Visible = False
            menuRibbon_(0).ResumeUpdating(True)
        End Sub

        ''' <summary>
        ''' Vincula un evento a un control
        ''' </summary>
        ''' <param name="botonera_">Colección de botones del panel de acceso rápido</param>
        ''' <param name="modulo_">Módulo del cuál se deriva el panel de acceso rápido</param>
        Private Shared Sub VincularEvento(
            ByVal botonera_ As IList,
            ByVal modulo_ As Object
        )

            'RECORRE LA LISTA DE BOTONES DEL PANEL DE ACCESO RÁPIDO
            For Each item_ In botonera_
                Dim descripcionEvento_() As String = item_.Tag.ToString.Split(".")
                Dim control_ As Object = modulo_

                'RECORRE LA DESCRIPCIÓN DEL EVENTO, HASTA LLEGAR AL ÚLTIMO NIVEL DE PROPIEDAD
                For indicePropiedad_ As Integer = 0 To descripcionEvento_.Length - 2
                    control_ = control_.GetType.GetProperty(descripcionEvento_(indicePropiedad_)).GetValue(control_, Nothing)
                Next

                'OBTIENE LA INFORMACIÓN DEL EVENTO DEL CONTROL
                Dim informacionEvento_ As EventInfo = item_.GetType.GetEvent("Click")
                'OBTIENE LA INFORMACIÓN DEL MÉTODO QUE SERÁ ASOCIADO AL EVENTO DEL CONTROL
                Dim informacionMetodo_ As MethodInfo = control_.GetType.GetMethod(descripcionEvento_(descripcionEvento_.Length - 1))

                If informacionMetodo_ Is Nothing Then
                    Exit Sub
                End If

                'CREA UN DELEGADO A PARTIR DE LA INFORMACIÓN DEL MÉTODO QUE SERÁ ASOCIADO AL EVENTO DEL CONTROL
                Dim delegado_ As System.Delegate = System.Delegate.CreateDelegate(informacionEvento_.EventHandlerType, control_, informacionMetodo_)

                'VINCULA EL EVENTO AL CONTROL
                informacionEvento_.AddEventHandler(item_, delegado_)
            Next
        End Sub

#End Region

    End Class

End Namespace
