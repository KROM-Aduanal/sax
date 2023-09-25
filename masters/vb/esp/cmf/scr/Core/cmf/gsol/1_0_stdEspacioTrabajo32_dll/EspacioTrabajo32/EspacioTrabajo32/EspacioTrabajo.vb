Imports gsol.Componentes.SistemaBase

Namespace gsol

    Public Class EspacioTrabajo
        Implements IEspacioTrabajo


#Region "Atributos"

        Private _usuario As String
        Private _grupoEmpresarial As Int32
        Private _divisionEmpresarial As Int32
        Private _aplicacion As Int32
        Private _idioma As Int32
        Private _sectoresEntorno As Dictionary(Of Int32, ISectorEntorno)
        Private _estatusConstrucccion As IConstructorVisual.EstatusConstruccion
        Private _misCredenciales As ICredenciales

        Private _modalidad As IEspacioTrabajo.ModalidadesEspacio
        Private _sistema As Organismo = New Organismo

        Private _claveEjecutivo As Int32

#End Region

#Region "Constructores"

        Sub New()
            _usuario = Nothing
            _grupoEmpresarial = -1
            _divisionEmpresarial = -1
            _aplicacion = -1
            _idioma = 1
            _sectoresEntorno = New Dictionary(Of Int32, ISectorEntorno)
            _estatusConstrucccion = IConstructorVisual.EstatusConstruccion.NoConstruido

            _modalidad = IEspacioTrabajo.ModalidadesEspacio.Produccion
        End Sub

        Sub New(
            ByVal usuario_ As String,
            ByVal grupoEmpresarial_ As Integer,
            ByVal divisionEmpresarial_ As Integer,
            ByVal aplicacion_ As Integer,
            ByRef idioma_ As Integer
        )
            _usuario = usuario_
            _grupoEmpresarial = grupoEmpresarial_
            _divisionEmpresarial = divisionEmpresarial_
            _aplicacion = aplicacion_
            _idioma = idioma_
            _sectoresEntorno = New Dictionary(Of Int32, ISectorEntorno)
            _estatusConstrucccion = IConstructorVisual.EstatusConstruccion.NoConstruido

            _modalidad = IEspacioTrabajo.ModalidadesEspacio.Produccion
        End Sub


        'Dim espacioTrabajo_ As IEspacioTrabajo =
        '    New EspacioTrabajo With {.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas,
        '                             .ClaveEjecutivo = 139,
        '                             .DivisionEmpresarial = 7785,
        '                             .Aplicacion = 4}

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"


        Public Property ClaveEjecutivo As Integer Implements IEspacioTrabajo.ClaveEjecutivo
            Get

                Return _claveEjecutivo

            End Get
            Set(value As Integer)
                _claveEjecutivo = value
            End Set
        End Property


        Function CambiaModalidad(ByVal modalidad_ As IEspacioTrabajo.ModalidadesEspacio)

            If modalidad_ = IEspacioTrabajo.ModalidadesEspacio.Pruebas Then
                _usuario = "Prueba"
                _grupoEmpresarial = 1
                _divisionEmpresarial = 1
                _aplicacion = 4
                _idioma = 1
                _sectoresEntorno = New Dictionary(Of Int32, ISectorEntorno)
                _estatusConstrucccion = IConstructorVisual.EstatusConstruccion.Construido


            Else
                _usuario = Nothing
                _grupoEmpresarial = -1
                _divisionEmpresarial = -1
                _aplicacion = -1
                _idioma = 1
                _sectoresEntorno = New Dictionary(Of Int32, ISectorEntorno)
                _estatusConstrucccion = IConstructorVisual.EstatusConstruccion.NoConstruido
            End If
            Return modalidad_
        End Function


        Public Property ModalidadEspacio As IEspacioTrabajo.ModalidadesEspacio _
            Implements IEspacioTrabajo.ModalidadEspacio
            Get
                Return _modalidad
            End Get
            Set(ByVal value As IEspacioTrabajo.ModalidadesEspacio)

                _modalidad = CambiaModalidad(value)
            End Set
        End Property

        Public WriteOnly Property Usuario As String _
            Implements IEspacioTrabajo.Usuario
            Set(ByVal value As String)
                Me._usuario = value
            End Set
        End Property

        Public WriteOnly Property GrupoEmpresarial As Integer _
            Implements IEspacioTrabajo.GrupoEmpresarial
            Set(ByVal value As Integer)
                Me._grupoEmpresarial = value
            End Set
        End Property

        Public WriteOnly Property DivisionEmpresarial As Integer _
            Implements IEspacioTrabajo.DivisionEmpresarial
            Set(ByVal value As Integer)
                Me._divisionEmpresarial = value
            End Set
        End Property

        Public WriteOnly Property Aplicacion As Integer _
            Implements IEspacioTrabajo.Aplicacion
            Set(ByVal value As Integer)
                Me._aplicacion = value
            End Set
        End Property

        Public Property Idioma As Integer _
            Implements IEspacioTrabajo.Idioma
            Get
                Return _idioma
            End Get
            Set(ByVal value As Integer)
                _idioma = value
            End Set
        End Property

        Public Property SectorEntorno As Dictionary(Of Int32, ISectorEntorno) _
            Implements IEspacioTrabajo.SectorEntorno
            Get
                Return _sectoresEntorno
            End Get
            Set(ByVal value As Dictionary(Of Int32, ISectorEntorno))
                _sectoresEntorno = value
            End Set
        End Property

        Public ReadOnly Property EstatusConstruccion As IConstructorVisual.EstatusConstruccion _
         Implements IEspacioTrabajo.EstatusConstruccion
            Get
                Return Me._estatusConstrucccion
            End Get
        End Property

        Public Property MisCredenciales As ICredenciales Implements IEspacioTrabajo.MisCredenciales
            Get
                Return _misCredenciales
            End Get
            Set(ByVal value As ICredenciales)
                _misCredenciales = value
            End Set
        End Property

#End Region

#Region "Métodos"

        Public Function ListaPermisosModulo(ByVal tipoModulo_ As IEspacioTrabajo.TipoModulo) As System.Collections.Generic.Dictionary(Of Integer, String) _
            Implements IEspacioTrabajo.ListaPermisosModulo
            Dim listaPermisos_ As New Dictionary(Of Int32, String)

            listaPermisos_.Add(118, "Acceso libre")

            For Each sectores_ In _sectoresEntorno.Values

                Dim sector_ = sectores_.Identitificador

                If sector_ <> tipoModulo_ Then
                    Continue For
                End If

                For Each itemPermiso_ As IPermisos In sectores_.Permisos.Values

                    If Not listaPermisos_.ContainsKey(itemPermiso_.Identificador) Then

                        listaPermisos_.Add(itemPermiso_.Identificador, itemPermiso_.Descripcion)

                    End If

                Next

            Next

            Return listaPermisos_

        End Function

        Public Sub GenerarEspacioTrabajo() Implements IEspacioTrabajo.GenerarEspacioTrabajo

            Select Case _modalidad
                Case IEspacioTrabajo.ModalidadesEspacio.Produccion
                    Dim lineaBase_ = New LineaBaseIniciaSesion

                    lineaBase_.Usuario = Me._usuario
                    lineaBase_.GrupoEmpresarial = Me._grupoEmpresarial
                    lineaBase_.DivisionEmpresarial = Me._divisionEmpresarial
                    lineaBase_.Aplicacion = Me._aplicacion
                    lineaBase_.Idioma = Me._idioma

                    Me._sectoresEntorno = _
                        lineaBase_.ObtenerEspacioTrabajo(
                            Me._usuario,
                            Me._grupoEmpresarial,
                            Me._divisionEmpresarial,
                            Me._aplicacion
                                        )


                    'Dim reesultado_ = Me.BuscaPermiso(1180, IEspacioTrabajo.TipoModulo.Abstracto)
                Case IEspacioTrabajo.ModalidadesEspacio.Pruebas
                    _sistema.GsDialogo("AVISO: Actualmente no cuenta con una sesión, " & Chr(13) & _
                                         "NO es posible Construir entorno en esta modalidad de {Pruebas}" & Chr(13) & _
                                         "Para continuar pulse ACEPTAR")

            End Select

        End Sub

        Public Sub ConstruirEntorno(
            ByVal contenedor_ As System.Collections.IList,
            ByVal manejadoresEventos_ As List(Of System.Reflection.MethodInfo)
        ) Implements IEspacioTrabajo.ConstruirEntorno

            Select Case _modalidad

                Case IEspacioTrabajo.ModalidadesEspacio.Produccion

                    Dim constructorVisual_ = New ConstructorVisual

                    constructorVisual_.ConstruirEntornoVisual(contenedor_, manejadoresEventos_, Me._sectoresEntorno, IConstructorVisual.TipoModulo.Grafico, Nothing)
                    Me._estatusConstrucccion = constructorVisual_.Estatus

                Case IEspacioTrabajo.ModalidadesEspacio.Pruebas
                    _sistema.GsDialogo("AVISO: Actualmente no cuenta con una sesión, " & Chr(13) & _
                                       "NO es posible Construir entorno en esta modalidad de {Pruebas} " & Chr(13) & _
                                       "Para continuar pulse ACEPTAR")

            End Select

        End Sub

        Public Function BuscaPermiso(ByVal identificador_ As Integer, _
            ByVal tipomodulo_ As IEspacioTrabajo.TipoModulo) As Boolean _
            Implements IEspacioTrabajo.BuscaPermiso
            Dim busqueda_ As Boolean

            If identificador_ = 0 Then

                Return False

            End If

            Select Case _modalidad

                Case IEspacioTrabajo.ModalidadesEspacio.Produccion

                    Select Case tipomodulo_
                        Case IEspacioTrabajo.TipoModulo.Grafico
                            If _sectoresEntorno.Count > 0 Then

                                Dim sector_ = _sectoresEntorno.FirstOrDefault(Function(item) item.Key = 8).Value

                                If sector_.Identitificador = 8 Then
                                    Dim permiso_ = sector_.Permisos.FirstOrDefault(Function(item) item.Key = identificador_).Key

                                    If permiso_ > 0 Then
                                        Return True
                                    Else
                                        busqueda_ = False
                                    End If
                                Else
                                    busqueda_ = False
                                End If
                            Else
                                busqueda_ = False
                            End If
                        Case IEspacioTrabajo.TipoModulo.Abstracto
                            If _sectoresEntorno.Count > 0 Then

                                For Each sectores_ In _sectoresEntorno.Values

                                    Dim sector_ = sectores_.Identitificador

                                    If sector_ <> 8 Then

                                        Dim permiso_ = sectores_.Permisos.FirstOrDefault(Function(item) item.Key = identificador_).Key

                                        If permiso_ > 0 Then
                                            Return True
                                        Else
                                            busqueda_ = False
                                        End If

                                    Else
                                        busqueda_ = False
                                    End If
                                Next

                            Else
                                busqueda_ = False
                            End If
                    End Select

                Case IEspacioTrabajo.ModalidadesEspacio.Pruebas
                    _sistema.GsDialogo("AVISO: Actualmente no cuenta con una sesión, " & Chr(13) & _
                                       "Este módulo esta entregando banderas únicamente en modalidad de {Pruebas}" & Chr(13) & _
                                       "Para continuar pulse ACEPTAR")

                    busqueda_ = True
            End Select


            If busqueda_ = False Then

                '_sistema.GsDialogo("{Acceso denegado} Esta acción requiere el IDPermiso:[" & identificador_ & "], solicitelo.")

                Console.WriteLine("{Acceso denegado} Esta acción requiere el IDPermiso:[" & identificador_ & "], solicitelo.")

                'Throw New Exception("{Acceso denegado} Esta acción requiere el IDPermiso:[" & identificador_ & "], solicitelo.")


            End If

            Return busqueda_

        End Function

#End Region

    End Class

End Namespace
