Imports System.Windows.Forms
Imports gsol.BaseDatos.Operaciones
Imports System.Drawing
Imports gsol
Imports gsol.Componentes.SistemaBase
Imports gsol.BaseDatos

Imports gsol.krom

Imports gsol.krom.IEnlaceDatos.TiposDimension
Imports System.Reflection
Imports Wma.Operations
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

Public Class FormularioBase64

#Region "Atributos"

    'Entidad de datos
    Private _enlaceDatos As IEnlaceDatos
    'Entidad de datos
    Private _entidadDatos As IEntidadDatos


    Public _focuBuscar As Boolean
    'Definifiones auxiliares
    'EIN=Escritura inmediata
    'EMI=Escritura por Memoria Intermedia (Bajo Demanda)
    'TBD=Transacción bajo demanda

    Public Enum ClasesFormulario
        ClaseUn = 0
        ClaseA1 = 1 'Formulario 1:Maestro, EI(Si), EMI(No) 
        ClaseA2 = 2 'Formulario 1:Maestro, EI(Si), EMI(Si) 
        ClaseB1 = 3 'Formulario 1:Maestro->1:Detalle, EI(Si), EMI(Parcialmente), de escritura inmediata, con salvedad en items nuevos, 
        ClaseB2 = 4 'Formulario 1:Maestro->1:Detalle, EI(Si), EMI(Si)
        ClaseC1 = 5 'Formulario 1:Maestro->N:Detalle, EI(Si), EMI(Si)
    End Enum

    Public Enum FuncionesTransaccionales

        SinDefinir = 0
        GrabarTransacciones = 1
        CierreTransaccional = 2
        DetalleTemporal = 3
        DetalleTemporal2 = 4
        RollBackTransacciones = 4

    End Enum

    Public _ioperacionescatalogo As IOperacionesCatalogo

    Public _modalidadoperativa As IOperacionesCatalogo.TiposOperacionSQL

    Public _sistema As Organismo

    Public _claseformulario As ClasesFormulario

    Public _funcionTransaccional As FuncionesTransaccionales

    Public _autoCierreFormulario As Boolean

    Private _continuarOperacion As Boolean

    Private _respuestaOperacion As IOperacionesCatalogo.EstadoOperacion

    'Transaccionales
    Public _valorIndice As Int32 = 1

    Private _catalogosTransaccion As List(Of GsCatalogo)

    Private _nombreClaveModulo As String = Nothing

    Private _nombreClaveUpsert As String = Nothing

    Private _versionModulo As String = Nothing

    Private _listaControlesDinamicos As List(Of ControlDinamico)

#End Region

#Region "Events"

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones antes de cargar datos para inserción")> _
    Public Event AntesCargarDatosInsercion()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones antes de cargar datos para edición")> _
    Public Event AntesCargarDatosEdicion()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción")> _
    Public Event DespuesInsercionCometida()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de edición")> _
    Public Event DespuesEdicionCometida()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera exitosa")> _
    Public Event DespuesInsercionCometidaExitosa()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera fallida")> _
    Public Event DespuesInsercionCometidaFallida()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera exitosa")> _
    Public Event DespuesModificacionCometidaExitosa()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera fallida")> _
    Public Event DespuesModificacionCometidaFallida()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de pulsar el click en Aceptar y después de invocar la operación")> _
    Public Event EventoClickAceptarProcesado()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de pulsar el click en Cancelar, antes de procesar la operación")> _
    Public Event EventoClickCancelarPrevioProcesamiento()

#End Region

#Region "Constructores"

    ' delegado con parametros
    Public Delegate Sub Aviso(ByVal lsTexto As String)

    Sub New()

        InitializeComponent()

        'Comparamos la versión actual del módulo

        '''''''''''''''Ejemplo para utilizar políticas''''''''''''''''''''''''''''''''''''''''''''''''


        'If Not (_versionModulo Is Nothing And
        '       _nombreClaveModulo Is Nothing) Then

        '    Dim caracteristicas_ = New List(Of CaracteristicaCatalogo)

        '    With caracteristicas_

        '        .Add(New CaracteristicaCatalogo With {.Nombre = "t_NombreToken", .ValorAsignado = _nombreClaveModulo})

        '        .Add(New CaracteristicaCatalogo With {.Nombre = "t_Version", .ValorAsignado = _versionModulo})

        '    End With

        '    Dim _politicasBaseDatos As IPoliticasBaseDatos =
        '        New PoliticasBaseDatos(_ioperacionescatalogo,
        '                               8,
        '                               caracteristicas_)

        '    With _politicasBaseDatos

        '        If .GetTagWatcher.Status = TypeStatus.Errors Then

        '            _sistema.GsDialogo("Observación:" & .GetTagWatcher.ErrorDescription & "" & vbNewLine & "" &
        '                               "Número de la Política: " & .GetNumeroPolitica & "" & vbNewLine & "" &
        '                               "Parámetros: " & .GetParametrosPolitica & "" & vbNewLine & "" &
        '                               "Nombre de la Política: " & .GetNombrePolitica,
        '                               Componentes.SistemaBase.GsDialogo.TipoDialogo.Alerta)
        '        Else 'Proceder
        '            _sistema.GsDialogo("Versión correcta",
        '                               Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)
        '        End If

        '    End With

        'End If

        _sistema = New Organismo

        _ioperacionescatalogo = New OperacionesCatalogo

        _modalidadoperativa = IOperacionesCatalogo.TiposOperacionSQL.Insercion

        _claseformulario = New ClasesFormulario

        _claseformulario = ClasesFormulario.ClaseUn

        _focuBuscar = True

        _funcionTransaccional = FuncionesTransaccionales.SinDefinir

        _autoCierreFormulario = True

        _continuarOperacion = True

        _respuestaOperacion = IOperacionesCatalogo.EstadoOperacion.CSinDefinir

        _catalogosTransaccion = New List(Of GsCatalogo)


    End Sub

#End Region

#Region "Propiedades"

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Nombre clave para modalidad Upsert")> _
    Public Property NombreClaveUpsert As String

        Get

            Return _nombreClaveUpsert

        End Get

        Set(value As String)

            _nombreClaveUpsert = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Nombre clave del módulo, es nombre único")> _
    Public Property NombreClaveModulo As String

        Get

            Return _nombreClaveModulo

        End Get

        Set(value As String)

            _nombreClaveModulo = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Versión actualizada de la implementación")> _
    Public Property VersionModulo As String
        Get

            Return _versionModulo

        End Get

        Set(value As String)

            _versionModulo = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Lista de catálogos involucrados en la transacción")> _
    Public Property CatalogosTransaccion As IList(Of GsCatalogo)

        Get

            Return _catalogosTransaccion

        End Get

        Set(value As IList(Of GsCatalogo))

            _catalogosTransaccion = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Respuesta de la última operación invocada")> _
    Public Property RespuestaOperacion As IOperacionesCatalogo.EstadoOperacion

        Get

            Return _respuestaOperacion

        End Get

        Set(value As IOperacionesCatalogo.EstadoOperacion)

            _respuestaOperacion = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Decide si continúa el lanzador de ejecución de escalares o se detiene")> _
    Public Property ContinuarOperacionActual As Boolean
        Get
            Return _continuarOperacion
        End Get
        Set(value As Boolean)
            _continuarOperacion = value

        End Set
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Activa o desactiva el autocierre de un formluario")> _
    Public Property AutoCierreFormluario As Boolean
        Get
            Return _autoCierreFormulario
        End Get
        Set(value As Boolean)
            _autoCierreFormulario = value

        End Set
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Coloca por default el foco en el control de búsqueda")> _
    Public Property FocoBuscar As Boolean
        Get
            Return _focuBuscar
        End Get
        Set(value As Boolean)
            _focuBuscar = value

        End Set
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Valor actual del índice ")> _
    Public Property ValorIndice As String
        Get
            Return _ioperacionescatalogo.ValorIndice
        End Get
        Set(value As String)
            _ioperacionescatalogo.ValorIndice = value
        End Set
    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Objeto actual de IOperacionesCatalogo asignado")> _
    Public Property OperacionesCatalogo As IOperacionesCatalogo
        Get
            Return Me._ioperacionescatalogo
        End Get
        Set(value As IOperacionesCatalogo)

            Me._ioperacionescatalogo = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Overridable Sub PreparaModificacion()

    End Sub


    Public Overridable Sub RealizarInsercion()


    End Sub

    Public Overridable Sub RealizarModificacion()


    End Sub

    Public Sub ConstructorContexto(
        ByVal claseformulario_ As ClasesFormulario, _
        ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL, _
        ByVal ioperacionesCatalogo_ As IOperacionesCatalogo,
        ByVal versionModulo_ As String,
        ByVal granularidad_ As String,
        ByVal dimension_ As String,
        ByVal entidadDatos_ As String,
        Optional ByVal listaLibrerias_ As List(Of String) = Nothing,
        Optional ByVal listaAtributosSeleccionados_ As List(Of String) = Nothing)

        'Lista de controles dinamicos
        _listaControlesDinamicos = New List(Of ControlDinamico)

        _ioperacionescatalogo = New OperacionesCatalogo

        _ioperacionescatalogo = ioperacionesCatalogo_

        _modalidadoperativa = tipoOperacion_

        'Control de versiones
        _versionModulo = versionModulo_ 'lblVersionModulo.Text

        'Inicializamos enlace de datos
        _enlaceDatos = New EnlaceDatos

        Dim objetoEnumGranularidad_ As IEnlaceDatos.TiposDimension =
            GetEnumByStringValueAttribute(Of IEnlaceDatos.TiposDimension)(granularidad_)

        Dim objetoEnumDimension_ As IEnlaceDatos.TiposDimension =
            GetEnumByStringValueAttribute(Of IEnlaceDatos.TiposDimension)(dimension_)


        With _enlaceDatos

            .EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

            .LimiteResultados = 100

            'Granularidad = IEnlaceDatos.TiposDimension.SinDefinir
            .Granularidad = objetoEnumGranularidad_

            .ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

            .TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

            .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

            .ClausulasLibres = Nothing

            .FiltrosAvanzados = Nothing

        End With


        _entidadDatos = _sistema.Eval("New " & entidadDatos_,
                                      entidadDatos_,
                                    listaLibrerias_)

        _entidadDatos.Dimension = objetoEnumDimension_ 'Vt022ConsultaOperaciones 'IEnlaceDatos.TiposDimension.Vt022ConsultaOperaciones

        Dim tag_ As TagWatcher

        If listaAtributosSeleccionados_ Is Nothing Then

            listaAtributosSeleccionados_ = New List(Of String)

            tag_ = _enlaceDatos.ObtieneEstructuraCompleta(_entidadDatos,
                                                          entidadDatos_,
                                                          Nothing)

            Dim entidadDatosEstructuraAuxiliar_ As List(Of IEntidadDatos) = Nothing

            entidadDatosEstructuraAuxiliar_ = tag_.ObjectReturned

            Dim listaAtributosAuxiliar_ As List(Of CampoVirtual) = entidadDatosEstructuraAuxiliar_(0).Atributos

            For Each campo_ As CampoVirtual In listaAtributosAuxiliar_
                listaAtributosSeleccionados_.Add(campo_.Atributo)
            Next

            CargarEntidadDatos(_entidadDatos, listaAtributosSeleccionados_)

        Else

            CargarEntidadDatos(_entidadDatos, listaAtributosSeleccionados_)

            tag_ = _enlaceDatos.ObtieneEstructuraResultados(_entidadDatos, Nothing)

        End If

        Dim entidadDatosEstructura_ As List(Of IEntidadDatos) = Nothing

        Dim listaAtributos_ As List(Of CampoVirtual)

        If tag_.Status = TagWatcher.TypeStatus.Ok Then

            entidadDatosEstructura_ = tag_.ObjectReturned

            listaAtributos_ = entidadDatosEstructura_(0).Atributos

            Dim posx_ As Int32 = 30

            Dim posy_ As Int32 = 90

            _listaControlesDinamicos.Clear()

            Dim ancho_ As Int32 = 400

            Dim alto_ As Int32 = 200

            Dim factor_ As Int32 = 50

            Dim cantidadControlesColumna_ As Int32 = 9

            Dim enUmbralDeControles_ As Boolean = False

            Dim contadorControles_ As Int32 = 0

            For Each campo_ As CampoVirtual In listaAtributos_

                If CrearControlVirtual(campo_, posx_, posy_, tipoOperacion_, contadorControles_, cantidadControlesColumna_) Then

                    posy_ += 50

                    If contadorControles_ <= cantidadControlesColumna_ Then

                        If Not enUmbralDeControles_ Then

                            alto_ += factor_

                        End If

                    End If

                    contadorControles_ += 1

                End If

                If contadorControles_ > cantidadControlesColumna_ Then
                    ancho_ += 240
                    posx_ += 280
                    posy_ = 90
                    contadorControles_ = 0

                    enUmbralDeControles_ = True

                End If

            Next

            '---Sección para asignaciones por defecto



            '----------------------------------------

            Me.Height = alto_

            Me.Width = ancho_

        End If

        InicializaFormulario(claseformulario_, tipoOperacion_)

    End Sub

    'Asignación en vivo de valores
    Public Sub AsignaEnLineaControles(ByRef currentControl_ As Object,
                                      ByVal tipoControl_ As String,
                                      ByVal campoVinculacion_ As String,
                                      ByVal valorCampoVinculacion_ As String,
                                      ByVal controlDinamico_ As ControlDinamico,
                                      Optional ByVal modalidadDatos_ As Int16 = 2)


        Dim nombreControl_ As String = campoVinculacion_

        Dim textBoxFinderAuxiliar_ As WclTextBoxFinder

        Select Case tipoControl_

            Case "WclTextBoxFinder"

                'nombreControl_ = DirectCast(currentControl_, WclTextBoxFinder).Name

                textBoxFinderAuxiliar_ = DirectCast(currentControl_, WclTextBoxFinder)

            Case "WclComboBox"

            Case "TextBox"
                'Not implemented
            Case "ComboBox"
                'Not implemented
            Case "CheckBox"
                'Not implemented
            Case "DateTimePicker"
                'Not implemented
            Case "RadioButton"
                'Not implemented

        End Select

        'Busca asignación de campo vinculación

        For Each Control_ As ControlDinamico In _listaControlesDinamicos

            If Control_.CampoVirtual.Atributo = campoVinculacion_ Then

                Select Case Control_.TipoControlEspecial

                    Case "TextBoxFinder"
                        'Not Implemented

                    Case "TextBoxFinderLong"
                        'Not Implemented

                    Case "WclComboBox"
                        'Not implemented
                    Case "ComboBox"
                        'Not implemented
                    Case "Textbox"
                        'Not implementes
                    Case "DateTimePicker"
                        'not implemented
                    Case "CheckBox"
                        'not implemented
                    Case "RadioButton"
                        'Not implemented
                    Case Else

                        Select Case Control_.CampoVirtual.TipoDato
                            Case IEntidadDatos.TiposDatos.Booleno
                                DirectCast(Control_.ObjetoControl, CheckBox).Checked = valorCampoVinculacion_

                                Exit For
                            Case IEntidadDatos.TiposDatos.Entero
                                DirectCast(Control_.ObjetoControl, MaskedTextBox).Text = valorCampoVinculacion_

                                Exit For
                            Case IEntidadDatos.TiposDatos.Fecha
                                ' DirectCast(Control_.ObjetoControl, DateTimePicker).Value = valorCampoVinculacion_.ToString("dd/MM/yyyy")

                                Exit For
                            Case IEntidadDatos.TiposDatos.Real
                                DirectCast(Control_.ObjetoControl, MaskedTextBox).Text = valorCampoVinculacion_

                                Exit For
                            Case IEntidadDatos.TiposDatos.Texto
                                DirectCast(Control_.ObjetoControl, TextBox).Text = valorCampoVinculacion_

                                Exit For
                            Case IEntidadDatos.TiposDatos.SinDefinir



                                Exit For

                        End Select

                End Select

                '_sistema.GsDialogo("No se encontró el campo asociado con la vinculación [" & campoVinculacion_ & "]", GsDialogo.TipoDialogo.Alerta)

                Exit For

            End If

        Next

        'Busca asignación de campos default

        'For Each Control_ As ControlDinamico In _listaControlesDinamicos

        '    If Not Control_.ValorDefault Is Nothing Then

        '        Select Case Control_.TipoControlEspecial

        '            Case "TextBoxFinder"
        '                'Not Implemented

        '            Case "WclComboBox"
        '                'Not implemented
        '            Case "ComboBox"
        '                'Not implemented
        '            Case "Textbox"
        '                'Not implementes
        '            Case "DateTimePicker"
        '                'not implemented
        '            Case "CheckBox"
        '                'not implemented
        '            Case "RadioButton"
        '                'Not implemented
        '            Case Else

        '                Select Case Control_.CampoVirtual.TipoDato
        '                    Case IEntidadDatos.TiposDatos.Booleno
        '                        'DirectCast(Control_.ObjetoControl, CheckBox).Checked = Control_.ValorDefault

        '                        Exit For
        '                    Case IEntidadDatos.TiposDatos.Entero
        '                        DirectCast(Control_.ObjetoControl, MaskedTextBox).Text = Control_.ValorDefault

        '                        Exit For
        '                    Case IEntidadDatos.TiposDatos.Fecha

        '                        If LTrim(LCase(Control_.ValorDefault)) = "getdate()" Then

        '                            DirectCast(Control_.ObjetoControl, DateTimePicker).Value = Now.ToString("dd/MM/yyyy HH:mm:ss")

        '                        End If

        '                        Exit For
        '                    Case IEntidadDatos.TiposDatos.Real
        '                        DirectCast(Control_.ObjetoControl, MaskedTextBox).Text = Control_.ValorDefault

        '                        Exit For
        '                    Case IEntidadDatos.TiposDatos.Texto
        '                        DirectCast(Control_.ObjetoControl, TextBox).Text = Control_.ValorDefault

        '                        Exit For
        '                    Case IEntidadDatos.TiposDatos.SinDefinir

        '                        Exit For
        '                End Select

        '        End Select

        '        Exit For

        '    End If

        'Next

        'Recorriendo los auxiliares asignados
        If Not controlDinamico_.Auxiliares Is Nothing Then

            If _listaControlesDinamicos.Count < controlDinamico_.Auxiliares.Count Then
                _sistema.GsDialogo("Revise su configuración, la catindad de auxiliares es superior al contenido de sus campos ", GsDialogo.TipoDialogo.Alerta)
                Exit Sub

            End If

            For Each auxiliar_ As ParAuxiliares In controlDinamico_.Auxiliares

                'Buscando control
                For Each ctrl_ As ControlDinamico In _listaControlesDinamicos

                    'Determinamos el tipo de cada control
                    If ctrl_.CampoVirtual.Atributo = auxiliar_.Campo Then '.Asignacion Then

                        Select Case ctrl_.TipoControlEspecial

                            Case "TextBoxFinder"
                                'Not implemented
                            Case "TextBoxFinderLong"

                            Case "WclComboBox"
                                'Not implemented
                            Case "ComboBox"
                                'Not implemented
                            Case "Textbox"
                                'Not implementes
                            Case "DateTimePicker"
                                'not implemented
                            Case "CheckBox"
                                'not implemented
                            Case "RadioButton"
                                'Not implemented
                            Case Else

                                Select Case ctrl_.CampoVirtual.TipoDato

                                    Case IEntidadDatos.TiposDatos.Booleno
                                        Select Case modalidadDatos_
                                            Case 1  'Advanced
                                                'Not implemented
                                            Case 2 'Simple
                                                'Not implemented
                                        End Select

                                        'Not implemented
                                        Exit For

                                    Case IEntidadDatos.TiposDatos.Entero
                                        Select Case modalidadDatos_
                                            Case 1  'Simple
                                                DirectCast(ctrl_.ObjetoControl, MaskedTextBox).Text = textBoxFinderAuxiliar_.IOperations.Vista(0, auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                                            Case 2 'Advanced
                                                DirectCast(ctrl_.ObjetoControl, MaskedTextBox).Text = textBoxFinderAuxiliar_.IOperations.CampoPorNombreAvanzado(auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                                        End Select

                                        Exit For

                                    Case IEntidadDatos.TiposDatos.Fecha

                                        Select Case modalidadDatos_

                                            Case 1
                                                'Advanced
                                                DirectCast(ctrl_.ObjetoControl, DateTimePicker).Value = _
                                                    textBoxFinderAuxiliar_.IOperations.Vista(0, auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                                'Not implemented
                                            Case 2
                                                'Simple
                                                DirectCast(ctrl_.ObjetoControl, DateTimePicker).Value = _
                                                    textBoxFinderAuxiliar_.IOperations.CampoPorNombreAvanzado(auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                        End Select

                                        'DirectCast(ctrl_.ObjetoControl, DateTimePicker).Value = textBoxFinderAuxiliar_.IOperations.Vista(0, auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) '.ToString("d/MM/yyyy")

                                        Exit For

                                    Case IEntidadDatos.TiposDatos.Real

                                        Select Case modalidadDatos_
                                            Case 1  'Advanced
                                                DirectCast(ctrl_.ObjetoControl, MaskedTextBox).Text = textBoxFinderAuxiliar_.IOperations.Vista(0, auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico) 'Not implemented
                                            Case 2 'Simple
                                                DirectCast(ctrl_.ObjetoControl, MaskedTextBox).Text = textBoxFinderAuxiliar_.IOperations.CampoPorNombreAvanzado(auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)
                                        End Select

                                        Exit For

                                    Case IEntidadDatos.TiposDatos.Texto

                                        Select Case modalidadDatos_

                                            Case 1  'Advanced
                                                DirectCast(ctrl_.ObjetoControl, TextBox).Text = textBoxFinderAuxiliar_.IOperations.Vista(0, auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                            Case 2 'Simple
                                                DirectCast(ctrl_.ObjetoControl, TextBox).Text = textBoxFinderAuxiliar_.IOperations.CampoPorNombreAvanzado(auxiliar_.Asignacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                        End Select


                                        Exit For

                                    Case IEntidadDatos.TiposDatos.SinDefinir
                                        'Not implemented
                                        _sistema.GsDialogo("No se encontró el campo asociado con el auxiliar [" & auxiliar_.Campo & "]", GsDialogo.TipoDialogo.Alerta)

                                        Exit For


                                End Select

                                _sistema.GsDialogo("No se encontró el campo asociado con el auxiliar [" & auxiliar_.Campo & "]", GsDialogo.TipoDialogo.Alerta)

                                Exit For

                        End Select

                    End If

                Next

            Next

        End If

    End Sub


    Public Function GetEnumByStringValueAttribute(value As String, enumType As Type) As Object

        Dim aux_ As IEnlaceDatos.TiposDimension = SinDefinir

        For Each val As [Enum] In [Enum].GetValues(enumType)

            Dim fi As FieldInfo = enumType.GetField(val.ToString())

            aux_ = DirectCast(val, IEnlaceDatos.TiposDimension)

            If aux_.ToString = value Then

                Return aux_

            End If
        Next

        Return aux_

    End Function

    Public Function GetEnumByStringValueAttribute(Of TiposDimension)(value As String) As TiposDimension

        Return CType(GetEnumByStringValueAttribute(value, GetType(TiposDimension)), TiposDimension)

    End Function

    Private Function CrearControlVirtual(
                            ByVal controlVirtual_ As CampoVirtual,
                            ByVal posicionX_ As Int32,
                            ByVal posicionY_ As Int32,
                            ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL,
                            ByVal indice_ As Int32,
                            Optional ByVal maximoFilas_ As Int32 = 9) As Boolean

        Dim creado_ As Boolean = False

        With controlVirtual_

            If .Visible = ICaracteristica.TiposVisible.Si Or
                .Visible = ICaracteristica.TiposVisible.Impresion Or
                .Visible = ICaracteristica.TiposVisible.Virtual Then

                creado_ = CrearControlesVisuales(controlVirtual_, posicionX_, posicionY_, indice_, maximoFilas_)

                If .Visible = ICaracteristica.TiposVisible.Virtual Then
                    creado_ = False
                End If


            End If

        End With

        Return creado_

    End Function

    Sub textBoxFinder_AfterClickForAdvancedSearchEventHandler(ByRef textBoxFinderSender_ As WclTextBoxFinder,
                                                              ByVal controlDinamico_ As ControlDinamico)

        AsignaEnLineaControles(textBoxFinderSender_,
                               "WclTextBoxFinder",
                               controlDinamico_.Vinculacion,
                               textBoxFinderSender_.IOperations.CampoPorNombreAvanzado(controlDinamico_.Vinculacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico),
                               controlDinamico_,
                               2)


    End Sub

    Sub textBoxFinder_AfterClickForSimpleSearchEventHandler(ByRef textBoxFinderSender_ As WclTextBoxFinder,
                                                          ByVal controlDinamico_ As ControlDinamico)
        'Campo principal seleccionado

        AsignaEnLineaControles(textBoxFinderSender_,
                               "WclTextBoxFinder",
                               controlDinamico_.Vinculacion,
                               textBoxFinderSender_.IOperations.Vista(0, controlDinamico_.Vinculacion, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico),
                               controlDinamico_,
                               1)

    End Sub

    Private Function CrearControlesVisuales(ByRef controlVirtual_ As CampoVirtual,
                                         ByVal posicionX_ As Int32,
                                         ByVal posicionY_ As Int32,
                                         ByVal indice_ As Int32,
                                         Optional ByVal limiteFilas_ As Int32 = 9) As Boolean
        Dim creado_ As Boolean = False

        'las llaves no se presentan por ser automáticas
        If controlVirtual_.AtributoLLave = True Or
            controlVirtual_.AtributoLLave = 1 Then

            Return False

        End If


        Dim lblEtiqueta_ As New Label()
        'Dim lblEtiquetaRequerido_ As New Label
        Dim distancia_ As Int32 = 21

        Dim color_ As Color = Color.LightYellow

        Dim ancho_ As Int32 = 250

        Dim alto_ As Int32 = 20

        Dim controlDinamico_ As New ControlDinamico


        '--------Etiquetado
        Dim tipoControlEspecial_ As String = Nothing

        Dim vinculacion_ As String = Nothing

        Dim nameAsKey_ As String = Nothing

        Dim campoDesplegable_ As String = Nothing

        Dim campoLlave_ As String = Nothing

        Dim filtrosAvanzados_ As String = Nothing

        Dim requestedObject_ As List(Of ComponentView)

        Dim activarControl_ As Boolean = True

        If (controlVirtual_.PuedeInsertar = ICaracteristica.TiposRigorDatos.No) Then

            activarControl_ = False

        ElseIf controlVirtual_.PuedeInsertar = ICaracteristica.TiposRigorDatos.Si Then



        ElseIf controlVirtual_.TipoRigorDato = IEntidadDatos.TiposRigorDato.Opcional Then

            'lblEtiquetaRequerido_.Text = ""

        ElseIf controlVirtual_.TipoRigorDato = IEntidadDatos.TiposRigorDato.SinDefinir Then

            'lblEtiquetaRequerido_.Text = "(?)"

        End If

        If controlVirtual_.Visible = ICaracteristica.TiposVisible.Acarreo Then

            lblEtiqueta_.Visible = False

        ElseIf controlVirtual_.Visible = ICaracteristica.TiposVisible.Impresion Then

            activarControl_ = False
            '            lblEtiqueta_.Visible = True

        End If

        '  Me.Controls.Add(lblEtiquetaRequerido_)

        '--------------------------Revisión de interfaz'
        Dim jsonFile_ As String = controlVirtual_.Interfaz

        Try

            If Not jsonFile_ Is Nothing Then

                Dim serialJSON_ As New System.Web.Script.Serialization.JavaScriptSerializer

                requestedObject_ = serialJSON_.Deserialize(Of List(Of ComponentView))(jsonFile_)

                If Not requestedObject_ Is Nothing Then

                    tipoControlEspecial_ = requestedObject_(0).Tipo

                    vinculacion_ = requestedObject_(0).Vinculacion

                    campoDesplegable_ = requestedObject_(0).CampoDesplegar

                    campoLlave_ = requestedObject_(0).CampoLLave

                    filtrosAvanzados_ = requestedObject_(0).ClausulasLibres

                    nameAsKey_ = controlVirtual_.NameAsKey

                    controlDinamico_.NameAsKey = nameAsKey_

                    controlDinamico_.NombreCampoLlave = requestedObject_(0).CampoLLave

                    controlDinamico_.CampoDesplegable = requestedObject_(0).CampoDesplegar

                    controlDinamico_.ValorDefault = requestedObject_(0).ValorDefault

                    controlDinamico_.Auxiliares = requestedObject_(0).Auxiliares

                End If

            End If

        Catch ex As Exception

        End Try


        '--------------------------

        '--------Etiquetado

        'Pendiente Visible
        With controlDinamico_

            .CampoLlave = controlVirtual_.AtributoLLave

            '.CampoDesplegable = Nothing
            .CampoVirtual = controlVirtual_
            .IDControl = controlVirtual_.Atributo.ToString
            '.NameAsKey = Nothing
            .ObjetoControl = Nothing
            .TipoRigorDato = controlVirtual_.TipoRigorDato
            .TipoDato = controlVirtual_.TipoDato
            '.Visible = controlVirtual_.Visible
        End With

        Dim font_ As New Font("Arial", 9, FontStyle.Regular)

        With lblEtiqueta_
            .Location = New Point(posicionX_, posicionY_ - distancia_)
            .Height = alto_
            .Width = ancho_
            '.BackColor = Color.Transparent
            .BackColor = Color.Transparent
            .Name = "lbl" & controlVirtual_.Atributo.ToString
            .Text = controlVirtual_.Descripcion
            .Font = font_
        End With


        Dim font2_ As New Font("Arial", 9, FontStyle.Bold)

        'With lblEtiquetaRequerido_
        '    .Location = New Point(lblEtiqueta_.Width + 10, posicionY_ - distancia_)
        '    .Height = alto_
        '    .Width = ancho_
        '    .BackColor = Color.Transparent
        '    .Name = "lblprior" & controlVirtual_.Atributo.ToString
        '    .Text = "(*)"
        '    .Font = font2_
        'End With

        If tipoControlEspecial_ <> "TextBoxFinder" And tipoControlEspecial_ <> "TextBoxFinderLong" Then

            If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                lblEtiqueta_.Hide() '= False
            End If
            Me.Controls.Add(lblEtiqueta_)
        End If

        'Fabrica de controles automáticos, según definición
        If Not tipoControlEspecial_ Is Nothing Then

            controlDinamico_.TipoControlEspecial = tipoControlEspecial_

            controlDinamico_.Vinculacion = vinculacion_

            If tipoControlEspecial_ = "WclComboBox" Then

                Dim controlComboBox_ As New ComboBox()

                With controlComboBox_
                    .Location = New Point(posicionX_, posicionY_)
                    .Height = alto_
                    .Width = ancho_
                    .BackColor = color_
                    .Name = controlVirtual_.Atributo.ToString
                    .Text = Nothing 'controlVirtual_.Descripcion
                    .Font = font_
                    .DropDownStyle = ComboBoxStyle.DropDownList

                    Dim iOperacionesNuevo_ As IOperacionesCatalogo = New OperacionesCatalogo

                    iOperacionesNuevo_ = _sistema.EnsamblaModulo(nameAsKey_).Clone

                    PreparaListaDeplegable(controlComboBox_,
                                           iOperacionesNuevo_.VistaEncabezados,
                                           iOperacionesNuevo_.OperadorCatalogoConsulta,
                                           campoDesplegable_,
                                           campoLlave_) ', filtrosAvanzados_)



                    If Not controlDinamico_.ValorDefault Is Nothing Then

                        If IsNumeric(controlDinamico_.ValorDefault) Then
                            .SelectedValue = Convert.ToInt32(controlDinamico_.ValorDefault)
                        End If

                    End If


                End With

                If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                    controlComboBox_.Hide() '= False
                End If
                Me.Controls.Add(controlComboBox_)


                creado_ = True

                controlDinamico_.ObjetoControl = controlComboBox_

                _listaControlesDinamicos.Add(controlDinamico_)

            ElseIf tipoControlEspecial_ = "ComboBox" Then

                Dim controlComboBox_ As New ComboBox()

                With controlComboBox_
                    .Location = New Point(posicionX_, posicionY_)
                    .Height = alto_
                    .Width = ancho_ / 2
                    .BackColor = color_
                    .Name = controlVirtual_.Atributo.ToString
                    .Text = Nothing 'controlVirtual_.Descripcion
                    .Font = font_
                    .Enabled = True
                    .DropDownStyle = ComboBoxStyle.DropDownList

                End With

                If Not requestedObject_ Is Nothing Then

                    If Not requestedObject_(0).Valores Is Nothing Then

                        'Dim ItemObject_ As Object() = New Object()

                        Dim itemIndex_ As Int32 = 0

                        Dim items_ As New List(Of Int32)

                        For Each valor_ As ParValores In requestedObject_(0).Valores

                            'controlComboBox_.Items.Insert(itemIndex_, valor_.Descripcion)
                            itemIndex_ = valor_.Valor

                            If Not items_.Contains(itemIndex_) Then

                                items_.Add(itemIndex_)

                                controlComboBox_.Items.Insert(itemIndex_, valor_.Descripcion)

                                'controlComboBox_.Items.Insert(0, ("Zion"))

                                'controlComboBox_.SelectedIndex = 0
                                'controlComboBox_.Sorted = True


                                'itemIndex_ += 1

                            Else
                                _sistema.GsDialogo("El item {" & itemIndex_ & "} en el control [" & controlVirtual_.Atributo.ToString & "], se encuentra repetido, por favor verificar su configuración", GsDialogo.TipoDialogo.Err)


                            End If


                        Next

                    End If

                End If


                If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                    controlComboBox_.Hide() '= False
                End If
                Me.Controls.Add(controlComboBox_)

                creado_ = True

                controlDinamico_.ObjetoControl = controlComboBox_

                _listaControlesDinamicos.Add(controlDinamico_)

            ElseIf tipoControlEspecial_ = "TextBoxFinder" Then

                Dim textBoxFinder_ As New WclTextBoxFinder()

                With textBoxFinder_

                    Dim maximo_ As Double = 385

                    Dim correccionY_ As Double = (maximo_ * indice_) / limiteFilas_

                    .Location = New Point(posicionX_ - 7, correccionY_ + 65)
                    .tbTextBox.Width = textBoxFinder_.tbTextBox.Width - 5
                    .tbKey.Width = textBoxFinder_.tbKey.Width - 5

                    '.Height = alto_ + 15
                    .tbTextBox.Height = alto_ '+ 15

                    .Width = ancho_
                    '.BackColor = color_
                    .Name = controlVirtual_.Atributo.ToString
                    .Text = Nothing
                    .Font = font_
                    .Enabled = True
                    .Title = controlVirtual_.Descripcion 'campoDesplegable_
                    .IOperations = _ioperacionescatalogo.Clone
                    .ModeKeyField = WclTextBoxFinder.KeyFieldModes.ManualTechnicalKeyField
                    .SetModalFinder = WclTextBoxFinder.TextBoxFinderModals.AdvancedFinder
                    .NameAsKey = nameAsKey_
                    .KeyField = vinculacion_
                    .DisplayField = campoDesplegable_
                    .FreeClausules = filtrosAvanzados_
                    .PermissionNumber = "118"
                    '.BackColor = Color.Beige

                    Dim tipoTBF_ As ICaracteristica.TiposCaracteristica = ICaracteristica.TiposCaracteristica.cInt32

                    tipoTBF_ = BuscaTipoCaracteristica(vinculacion_)

                    .KeyValueDataType = tipoTBF_

                End With


                If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then

                    textBoxFinder_.Hide() '= False

                End If

                Me.Controls.Add(textBoxFinder_)

                creado_ = True

                controlDinamico_.ObjetoControl = textBoxFinder_

                'Agregando eventos
                AddHandler textBoxFinder_.AfterClickForAdvancedSearch, Sub() textBoxFinder_AfterClickForAdvancedSearchEventHandler(textBoxFinder_, controlDinamico_)

                AddHandler textBoxFinder_.AfterClickForSimpleSearch, Sub() textBoxFinder_AfterClickForSimpleSearchEventHandler(textBoxFinder_, controlDinamico_)

                _listaControlesDinamicos.Add(controlDinamico_)


            ElseIf tipoControlEspecial_ = "TextBoxFinderLong" Then

                Dim textBoxFinder_ As New WclTextBoxFinder()

                With textBoxFinder_

                    Dim maximo_ As Double = 385

                    Dim correccionY_ As Double = (maximo_ * indice_) / limiteFilas_

                    .Location = New Point(posicionX_ - 7, correccionY_ + 65)
                    .tbTextBox.Width = textBoxFinder_.tbTextBox.Width - 41
                    '.tbKey.BackColor = Color.Beige
                    .tbTextLeft = 88

                    '.tbTextBox.BackColor = Color.BlueViolet

                    .tbKey.Width = textBoxFinder_.tbKey.Width + 30
                    '.tbKey.Dock = DockStyle.Left
                    '.tbTextBox.Dock = DockStyle.Right


                    '.Height = alto_ + 15
                    .tbTextBox.Height = alto_ '+ 15

                    .Width = ancho_
                    '.BackColor = color_
                    .Name = controlVirtual_.Atributo.ToString
                    .Text = Nothing
                    .Font = font_
                    .Enabled = True
                    .Title = controlVirtual_.Descripcion 'campoDesplegable_
                    .IOperations = _ioperacionescatalogo.Clone
                    .ModeKeyField = WclTextBoxFinder.KeyFieldModes.ManualTechnicalKeyField
                    .SetModalFinder = WclTextBoxFinder.TextBoxFinderModals.AdvancedFinder
                    .NameAsKey = nameAsKey_
                    .KeyField = vinculacion_
                    .DisplayField = campoDesplegable_
                    .FreeClausules = filtrosAvanzados_
                    .PermissionNumber = "118"
                    '.BackColor = Color.BlueViolet

                    Dim tipoTBF_ As ICaracteristica.TiposCaracteristica = ICaracteristica.TiposCaracteristica.cInt32

                    tipoTBF_ = BuscaTipoCaracteristica(vinculacion_)

                    .KeyValueDataType = tipoTBF_

                End With


                If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                    textBoxFinder_.Hide() '= False
                End If
                Me.Controls.Add(textBoxFinder_)

                creado_ = True

                controlDinamico_.ObjetoControl = textBoxFinder_

                'Agregando eventos
                AddHandler textBoxFinder_.AfterClickForAdvancedSearch, Sub() textBoxFinder_AfterClickForAdvancedSearchEventHandler(textBoxFinder_, controlDinamico_)

                AddHandler textBoxFinder_.AfterClickForSimpleSearch, Sub() textBoxFinder_AfterClickForSimpleSearchEventHandler(textBoxFinder_, controlDinamico_)

                _listaControlesDinamicos.Add(controlDinamico_)




            Else

                Dim controlMaskedTextBox_ As New MaskedTextBox()

                With controlMaskedTextBox_
                    .Location = New Point(posicionX_, posicionY_)
                    .Height = alto_
                    .Width = ancho_ / 2
                    .Mask = "###"
                    .BackColor = color_
                    .Name = controlVirtual_.Atributo.ToString
                    .Text = Nothing 'controlVirtual_.Descripcion
                    .Font = font_

                    If Not activarControl_ = True Then
                        .Enabled = False
                        .BackColor = Color.WhiteSmoke
                    End If

                End With


                If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                    controlMaskedTextBox_.Hide() '= False
                End If
                Me.Controls.Add(controlMaskedTextBox_)

                creado_ = True

                controlDinamico_.ObjetoControl = controlMaskedTextBox_

                _listaControlesDinamicos.Add(controlDinamico_)

            End If

        Else
            Select Case controlVirtual_.TipoDato

                Case IEntidadDatos.TiposDatos.Booleno

                    Dim controlRadioBtn_ As New CheckBox()

                    With controlRadioBtn_
                        .Location = New Point(posicionX_, posicionY_)
                        .Height = alto_
                        .Width = ancho_
                        .BackColor = color_
                        .Name = controlVirtual_.Atributo.ToString
                        .Text = Nothing 'controlVirtual_.Descripcion
                        .Font = font_
                        .Enabled = activarControl_

                        If Not activarControl_ = True Then
                            .Enabled = False
                            .BackColor = Color.WhiteSmoke
                        End If

                    End With

                    If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                        controlRadioBtn_.Hide() '= False
                    End If
                    Me.Controls.Add(controlRadioBtn_)
                    creado_ = True

                    controlDinamico_.ObjetoControl = controlRadioBtn_

                    _listaControlesDinamicos.Add(controlDinamico_)

                Case IEntidadDatos.TiposDatos.Entero


                    Dim controlMaskedTextBox_ As New MaskedTextBox()

                    With controlMaskedTextBox_
                        .Location = New Point(posicionX_, posicionY_)
                        .Height = alto_
                        .Width = ancho_ / 2
                        .Mask = "###"
                        .BackColor = color_
                        .Name = controlVirtual_.Atributo.ToString
                        .Text = Nothing 'controlVirtual_.Descripcion
                        .Font = font_

                        If Not activarControl_ = True Then
                            .Enabled = False
                            .BackColor = Color.WhiteSmoke
                        End If

                    End With

                    If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                        controlMaskedTextBox_.Hide() '= False
                    End If
                    Me.Controls.Add(controlMaskedTextBox_)

                    creado_ = True

                    controlDinamico_.ObjetoControl = controlMaskedTextBox_

                    _listaControlesDinamicos.Add(controlDinamico_)


                Case IEntidadDatos.TiposDatos.Fecha

                    Dim controlDateTimePicker_ As New DateTimePicker()

                    With controlDateTimePicker_

                        .Location = New Point(posicionX_, posicionY_)
                        .Height = alto_
                        .Width = ancho_
                        .BackColor = color_
                        .Format = DateTimePickerFormat.Short
                        '.Text = controlVirtual_.Descripcion
                        .Value = Now()
                        .Name = controlVirtual_.Atributo.ToString
                        .Font = font_
                        .ShowCheckBox = True

                        If Not activarControl_ = True Then
                            .Enabled = False
                            .BackColor = Color.WhiteSmoke
                        End If

                    End With

                    If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                        controlDateTimePicker_.Hide() '= False
                    End If
                    Me.Controls.Add(controlDateTimePicker_)

                    creado_ = True

                    controlDinamico_.ObjetoControl = controlDateTimePicker_

                    _listaControlesDinamicos.Add(controlDinamico_)

                Case IEntidadDatos.TiposDatos.Real

                    Dim controlMaskedTextBox_ As New MaskedTextBox()

                    With controlMaskedTextBox_
                        .Location = New Point(posicionX_, posicionY_)
                        .Height = alto_
                        .Width = ancho_ / 2
                        .BackColor = color_
                        .Mask = "###"
                        .Name = controlVirtual_.Atributo.ToString
                        .Text = Nothing 'controlVirtual_.Descripcion
                        .Font = font_

                        If Not activarControl_ = True Then
                            .Enabled = False
                            .BackColor = Color.WhiteSmoke
                        End If

                    End With

                    If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                        controlMaskedTextBox_.Hide() '= False
                    End If
                    Me.Controls.Add(controlMaskedTextBox_)

                    creado_ = True

                    controlDinamico_.ObjetoControl = controlMaskedTextBox_

                    _listaControlesDinamicos.Add(controlDinamico_)

                Case IEntidadDatos.TiposDatos.Texto

                    Dim controlTextBox_ As New TextBox()

                    With controlTextBox_
                        .Location = New Point(posicionX_, posicionY_)
                        .Height = alto_
                        .Width = ancho_
                        .BackColor = color_
                        .Text = Nothing 'controlVirtual_.Descripcion
                        .Name = controlVirtual_.Atributo.ToString
                        .Font = font_

                        If Not activarControl_ = True Then
                            .Enabled = False
                            .BackColor = Color.WhiteSmoke
                        End If

                    End With

                    If controlVirtual_.Visible = ICaracteristica.TiposVisible.Virtual Then
                        controlTextBox_.Hide() '= False
                    End If

                    Me.Controls.Add(controlTextBox_)

                    controlDinamico_.ObjetoControl = controlTextBox_

                    _listaControlesDinamicos.Add(controlDinamico_)

                    creado_ = True

            End Select

        End If

        Return creado_

    End Function

    Public Function BuscaTipoCaracteristica(ByVal vinculacion_ As String) As ICaracteristica.TiposCaracteristica

        Dim revision_ As String() = vinculacion_.Split("t_")

        If revision_.Count >= 2 Then
            Return ICaracteristica.TiposCaracteristica.cString
        Else
            Return ICaracteristica.TiposCaracteristica.cInt32
        End If

    End Function


    Public Overridable Sub CargarEntidadDatos(ByRef entidadDatos_ As IEntidadDatos,
                                               ByVal listaAtributosSeleccionados_ As List(Of String))

        For Each item_ As String In listaAtributosSeleccionados_

            With entidadDatos_
                .cmp(item_)
            End With

        Next

        With _enlaceDatos

            .ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesAutomaticas

            .TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

            .TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativo

        End With

    End Sub


    Public Sub InicializaFormulario(ByVal claseformulario_ As ClasesFormulario, _
                                ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        VersionCorrecta()

        _funcionTransaccional = FuncionesTransaccionales.SinDefinir

        Select Case claseformulario_

            Case ClasesFormulario.ClaseA1 ' Sencillo
                IniciaRegistrosA1(tipooperacion_)

            Case ClasesFormulario.ClaseA2 'Maestro Detalle, Cátalogo->Detalle->Detalle
                IniciaRegistrosA2(tipooperacion_, IOperacionesCatalogo.TiposEscritura.SinDefinir)

            Case ClasesFormulario.ClaseB1 'Maestro Detalle, Cátalogo->Detalle->Simple<FIN>

                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseB2 'Maestro Detalle, Cátalogo->Detalle->Detalle<FIN>
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseC1
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseUn
                _sistema.GsDialogo("No se ha asignado controlador de inicio para el formulario")

        End Select

    End Sub

    Public Sub VersionCorrecta()

        If _versionModulo = "0.0.0.0" Then
            Exit Sub
        End If

        If Not (_versionModulo Is Nothing And
         _nombreClaveModulo Is Nothing) Then

            Dim caracteristicas_ = New List(Of CaracteristicaCatalogo)

            With caracteristicas_

                .Add(New CaracteristicaCatalogo With {.Nombre = "t_NombreToken", .ValorAsignado = _nombreClaveModulo})

                .Add(New CaracteristicaCatalogo With {.Nombre = "t_Version", .ValorAsignado = _versionModulo})

            End With

            Dim _politicasBaseDatos As IPoliticasBaseDatos =
                New PoliticasBaseDatos(_ioperacionescatalogo,
                                       8,
                                       caracteristicas_)

            With _politicasBaseDatos

                If .GetTagWatcher.Status = TypeStatus.Errors Then

                    _sistema.GsDialogo("Versión del módulo incorrecta [" & VersionModulo & " <-Obsoleta]" & vbNewLine &
                                       "- Requiere actualización para seguir operando" & vbNewLine &
                                       "- " & .GetTagWatcher.ErrorDescription & vbNewLine &
                                       "- ID Política : " & .GetNumeroPolitica & vbNewLine &
                                       "- Descripción: " & .GetNombrePolitica,
                                       Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

                    Me.Close()

                End If

            End With

        End If

    End Sub


    Public Sub InicializaFormulario(ByVal claseformulario_ As ClasesFormulario, _
                                ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL, _
                                Optional funcionTransaccional_ As FuncionesTransaccionales = FuncionesTransaccionales.SinDefinir,
                                Optional ByVal tipoEscrituraPrevia_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.SinDefinir)

        VersionCorrecta()

        _funcionTransaccional = funcionTransaccional_

        Select Case claseformulario_

            Case ClasesFormulario.ClaseA1 ' Sencillo
                IniciaRegistrosA1(tipooperacion_)

            Case ClasesFormulario.ClaseA2 'Maestro Detalle, Cátalogo->Detalle->Detalle
                IniciaRegistrosA2(tipooperacion_, tipoEscrituraPrevia_)

            Case ClasesFormulario.ClaseB1 'Maestro Detalle, Cátalogo->Detalle->Simple<FIN>

                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseB2 'Maestro Detalle, Cátalogo->Detalle->Detalle<FIN>
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseC1
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseUn
                _sistema.GsDialogo("No se ha asignado controlador de inicio para el formulario")

        End Select

    End Sub

    Public Overridable Sub IniciaRegistros(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        Select Case tipooperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                LblAccion.Text = "Nuevo registro {" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                LblAccion.Text = "Edición {" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

                PreparaModificacion()

            Case Else

        End Select

    End Sub

    Public Overridable Sub IniciaRegistrosA1(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        Select Case tipooperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                LblAccion.Text = "Nuevo registro {" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                LblAccion.Text = "Edición {" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

                AsignacionAutomatica(IOperacionesCatalogo.TiposOperacionSQL.Modificar, True)

                PreparaModificacion()

            Case Else

        End Select

    End Sub

    Public Sub ObtieneLLavesInicialesA2(ByRef llavePrimaria_ As String, ByRef llaveForanea1_ As String, ByVal llavePrimariaAnterior_ As String)

        llavePrimaria_ = "-1"

        If _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion And _
            _ioperacionescatalogo.OperacionAnterior.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion And _
            _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata Then

            llaveForanea1_ = "-1"

        End If

        If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

            llaveForanea1_ = "-1"

            ObtieneIndiceCierreA2(_ioperacionescatalogo.TipoOperacionSQL)

        Else

            llaveForanea1_ = _ioperacionescatalogo.OperacionAnterior.CampoPorNombre(llavePrimariaAnterior_)

        End If

    End Sub

    Sub ObtieneIndiceCierreA2(ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        Select Case tipoOperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia Or _
                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                    If _ioperacionescatalogo.ValorIndice = "-1" Or _ioperacionescatalogo.ValorIndice Is Nothing Then

                        _valorIndice = 1

                    Else

                        _valorIndice = _ioperacionescatalogo.OperacionAnterior.IndiceTablaTemporal - 1

                    End If

                End If

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                If _ioperacionescatalogo.ValorIndice = "-1" Or _ioperacionescatalogo.ValorIndice Is Nothing Then

                    MsgBox("No ha seleccionado ningun registro para editar...")

                    _valorIndice = 1

                Else

                    _valorIndice = _ioperacionescatalogo.ValorIndice

                End If

            Case Else

        End Select

        _ioperacionescatalogo.IndiceTablaTemporalLlamante = _ioperacionescatalogo.OperacionAnterior.IndiceTablaTemporal

    End Sub


    Public Overridable Sub IniciaRegistrosA2(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL, _
                                             Optional ByVal tipoEscrituraPrevia_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.SinDefinir)

        _ioperacionescatalogo.TipoOperacionSQL = tipooperacion_



        If _ioperacionescatalogo.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion And _
            _ioperacionescatalogo.OperacionAnterior.TipoOperacionSQL = IOperacionesCatalogo.TiposOperacionSQL.Insercion And _
            tipoEscrituraPrevia_ = IOperacionesCatalogo.TiposEscritura.Inmediata Then

            tipoEscrituraPrevia_ = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

        End If

        If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

            ObtieneIndiceCierreA2(tipooperacion_)


            If _funcionTransaccional = FuncionesTransaccionales.CierreTransaccional Then

                _ioperacionescatalogo.IDObjetoTransaccional = "IOPD"

                _ioperacionescatalogo.IDNivelTransaccional = "N3"

            End If


        End If

        Select Case tipooperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                If tipoEscrituraPrevia_ <> IOperacionesCatalogo.TiposEscritura.SinDefinir Then

                    _ioperacionescatalogo.TipoEscritura = tipoEscrituraPrevia_

                Else

                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                End If

                LblAccion.Text = "Nuevo registro {BT}{" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                If _funcionTransaccional = FuncionesTransaccionales.GrabarTransacciones Then

                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                Else
                    If tipoEscrituraPrevia_ <> IOperacionesCatalogo.TiposEscritura.SinDefinir Then

                        _ioperacionescatalogo.TipoEscritura = tipoEscrituraPrevia_

                    Else
                        _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    End If

                End If

                LblAccion.Text = "Edición {BT}{" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

                PreparaModificacion()

            Case Else

        End Select

    End Sub

    Public Overridable Sub InvocarOperacion()

        'Revisión upsert, MasterOfPuppets 30/10/2019

        If Not _nombreClaveUpsert Is Nothing Then

            If IsDBNull(_ioperacionescatalogo.CampoPorNombre(_nombreClaveUpsert)) Or
                _ioperacionescatalogo.CampoPorNombre(_nombreClaveUpsert) = "" Then

                'Si el campo upsert esta en null o vacio, quiere decir que la operación requiere ser en un comienzo inserción, de lo contrario será actualización
                _modalidadoperativa = IOperacionesCatalogo.TiposOperacionSQL.Insercion

            Else

                _modalidadoperativa = IOperacionesCatalogo.TiposOperacionSQL.Modificar

            End If

        End If

        If _continuarOperacion Then

            _respuestaOperacion = IOperacionesCatalogo.EstadoOperacion.CSinDefinir

            Dim observaciones_ As String = Nothing

            observaciones_ = _ioperacionescatalogo.ValidaValoresCampos(_modalidadoperativa)

            If observaciones_ Is Nothing Then

                Select Case _modalidadoperativa

                    Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                        _respuestaOperacion = _ioperacionescatalogo.Agregar()


                    Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                        _respuestaOperacion = _ioperacionescatalogo.Modificar(_ioperacionescatalogo.ValorIndice)

                End Select

                If _respuestaOperacion = IOperacionesCatalogo.EstadoOperacion.COk Then

                    Select Case _modalidadoperativa

                        Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                            RaiseEvent DespuesInsercionCometidaExitosa()

                        Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                            RaiseEvent DespuesModificacionCometidaExitosa()

                    End Select

                    LblMensaje.ForeColor = Color.Green

                    LblMensaje.Text = "Operación exitosa!"

                Else

                    Select Case _modalidadoperativa

                        Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                            RaiseEvent DespuesInsercionCometidaFallida()

                        Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                            RaiseEvent DespuesModificacionCometidaFallida()

                    End Select

                    RaiseEvent DespuesInsercionCometidaFallida()

                    LblMensaje.ForeColor = Color.Red

                    LblMensaje.Text = "No se ha podido realizar la operación."

                End If

                If _autoCierreFormulario Then

                    Me.Close()

                End If

            Else

                LblMensaje.Text = observaciones_

            End If

        End If

    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click

        RaiseEvent EventoClickCancelarPrevioProcesamiento()

        Me.Close()

    End Sub

    Private Sub IndicacionesInternasDespuesCargaDatosEdicion()

        Select Case _ioperacionescatalogo.TipoEscritura

            Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                Select Case _funcionTransaccional

                    Case FuncionesTransaccionales.GrabarTransacciones
                        'NOT IMPLEMENTED

                    Case FuncionesTransaccionales.DetalleTemporal

                        'Dim myKey_ As String = Nothing

                        'myKey_ = _
                        '_ioperacionescatalogo.IDObjetoTransaccional & "." & _
                        '_ioperacionescatalogo.IDNivelTransaccional & "." & _
                        '_valorIndice

                        'If _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.ContainsKey(myKey_) Then

                        '    _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Remove(myKey_)

                        'End If

                        '_ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Add(myKey_, _ioperacionescatalogo.Clone)

                        '_ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Clear()

                        '_ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Add( _
                        'myKey_ & "." & _valorIndice & "", _catalogosTransaccion.Item(0).OperacionesCatalogo.Clone)

                        Dim indiceCatalogo_ As Int32 = 1
                        For Each catalogo_ As GsCatalogo In _catalogosTransaccion

                            Dim myKey_ As String = Nothing

                            myKey_ = _
                            _ioperacionescatalogo.IDObjetoTransaccional & "." & _
                            _ioperacionescatalogo.IDNivelTransaccional & "." & _
                            _valorIndice & "_" & _
                            indiceCatalogo_

                            If _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.ContainsKey(myKey_) Then

                                _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Remove(myKey_)

                            End If

                            _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Add(myKey_, _ioperacionescatalogo.Clone)

                            _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Clear()

                            _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Add( _
                            myKey_ & "." & _valorIndice & "", catalogo_.OperacionesCatalogo.Clone)

                            indiceCatalogo_ += 1
                        Next


                End Select

            Case Else
                'NOT IMPLEMENTED

        End Select

    End Sub

    Private Sub IndicacionesInternasDespuesCargaDatosInsercion()

        Select Case _ioperacionescatalogo.TipoEscritura
            Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                Select Case _funcionTransaccional

                    Case FuncionesTransaccionales.GrabarTransacciones

                        If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                            Dim myKey_ As String = Nothing
                            myKey_ = _
                                _ioperacionescatalogo.IDObjetoTransaccional & "." & _
                                _ioperacionescatalogo.IDNivelTransaccional & "." & _
                                (_ioperacionescatalogo.IndiceTablaTemporal - 1).ToString

                            If _ioperacionescatalogo.ColeccionInstrucciones.ContainsKey(myKey_) Then

                                _ioperacionescatalogo.ColeccionInstrucciones.Remove(myKey_)

                            End If

                            _ioperacionescatalogo.ColeccionInstrucciones.Add(myKey_, _ioperacionescatalogo.Clone)

                        End If

                    Case FuncionesTransaccionales.DetalleTemporal 'Primer Detalle solamente

                        If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                            ''Entregamos los datos  la bateria de instrucciones
                            'Dim myKey_ As String = Nothing

                            'myKey_ = _
                            '_ioperacionescatalogo.IDObjetoTransaccional & "." & _
                            '_ioperacionescatalogo.IDNivelTransaccional & "." & _
                            '_valorIndice

                            'If _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.ContainsKey(myKey_) Then

                            '    _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Remove(myKey_)

                            'End If

                            '_ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Add(myKey_, _ioperacionescatalogo.Clone)

                            ''Guardando detalle 
                            '_ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Clear()


                            '_ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Add( _
                            '    myKey_ & "." & _valorIndice & "", _catalogosTransaccion.Item(0).OperacionesCatalogo.Clone)

                            Dim indiceCatalogo_ As Int32 = 1
                            For Each catalogo_ As GsCatalogo In _catalogosTransaccion

                                'Entregamos los datos  la bateria de instrucciones
                                Dim myKey_ As String = Nothing

                                myKey_ = _
                                _ioperacionescatalogo.IDObjetoTransaccional & "." & _
                                _ioperacionescatalogo.IDNivelTransaccional & "." & _
                                _valorIndice & "_" & _
                                indiceCatalogo_

                                If _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.ContainsKey(myKey_) Then

                                    _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Remove(myKey_)

                                End If

                                _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Add(myKey_, _ioperacionescatalogo.Clone)

                                'Guardando detalle 
                                _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Clear()

                                _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Add( _
                                myKey_ & "." & _valorIndice & "", catalogo_.OperacionesCatalogo.Clone)

                                indiceCatalogo_ += 1

                            Next



                        End If


                    Case Else
                        'NOT IMPLEMENTED

                End Select

            Case Else
                'NOT IMPLEMENTED

        End Select


    End Sub

    Public Sub AsignacionAutomatica(ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL,
                                     Optional ByVal presentacionDatos_ As Boolean = False)

        If Not _listaControlesDinamicos Is Nothing Then

            For Each control_ As ControlDinamico In _listaControlesDinamicos

                Dim valorAsignadoCadena_ As String = Nothing

                If Not control_.TipoControlEspecial Is Nothing Then

                    If control_.TipoControlEspecial = "ComboBox" Then

                        valorAsignadoCadena_ = DirectCast(control_.ObjetoControl, ComboBox).SelectedIndex.ToString

                        If presentacionDatos_ Then


                            valorAsignadoCadena_ = _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion)

                            DirectCast(control_.ObjetoControl, ComboBox).SelectedIndex = valorAsignadoCadena_

                        Else
                            If valorAsignadoCadena_ <> "-1" Then

                                'MsgBox(DirectCast(control_.ObjetoControl, ComboBox).SelectedItem & "_" &
                                '       DirectCast(control_.ObjetoControl, ComboBox).SelectedText & "_" &
                                '       DirectCast(control_.ObjetoControl, ComboBox).SelectedValue)

                                If DirectCast(control_.ObjetoControl, ComboBox).SelectedItem = "" Or
                                    DirectCast(control_.ObjetoControl, ComboBox).SelectedIndex = -1 Then

                                    _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = Nothing

                                Else
                                    _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = valorAsignadoCadena_

                                End If

                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = Nothing

                            End If

                        End If


                    ElseIf control_.TipoControlEspecial = "WclComboBox" Then

                        If presentacionDatos_ Then

                            valorAsignadoCadena_ = _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion)

                            Dim iOperacionesNuevo_ As IOperacionesCatalogo = New OperacionesCatalogo

                            iOperacionesNuevo_ = _sistema.EnsamblaModulo(control_.NameAsKey).Clone

                            '     'Cargar de nuevo
                            PreparaListaDeplegable(control_.ObjetoControl,
                                                   iOperacionesNuevo_.VistaEncabezados,
                                                   iOperacionesNuevo_.OperadorCatalogoConsulta,
                                                   control_.CampoDesplegable,
                                                   control_.NombreCampoLlave) ', filtrosAvanzados_)

                            '     'Cargar de nuevo
                            '     PreparaListaDeplegable(control_.ObjetoControl,
                            '"Ve026IUCausalesTracking",
                            '"Vt026CausalesTracking",
                            '"Descripción causal",
                            '"ID")

                            If IsNumeric(valorAsignadoCadena_) Then
                                DirectCast(control_.ObjetoControl, ComboBox).SelectedValue = Convert.ToInt32(valorAsignadoCadena_)
                            End If


                        Else

                            valorAsignadoCadena_ = DirectCast(control_.ObjetoControl, ComboBox).SelectedValue.ToString

                            'If valorAsignadoCadena_ <> "-1" Then
                            _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = valorAsignadoCadena_
                            'Else
                            '    _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = Nothing

                            'End If

                        End If


                    ElseIf control_.TipoControlEspecial = "TextBoxFinder" Then

                        valorAsignadoCadena_ = DirectCast(control_.ObjetoControl, WclTextBoxFinder).tbKey.Text

                        If presentacionDatos_ Then

                            valorAsignadoCadena_ = _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion)

                            'DirectCast(control_.ObjetoControl, WclTextBoxFinder).tbKey.Text = valorAsignadoCadena_

                            DirectCast(control_.ObjetoControl, WclTextBoxFinder).SetKeyValue = valorAsignadoCadena_

                        Else

                            If valorAsignadoCadena_ <> "-1" Then

                                _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = valorAsignadoCadena_
                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = Nothing

                            End If

                        End If

                    ElseIf control_.TipoControlEspecial = "TextBoxFinderLong" Then

                        valorAsignadoCadena_ = DirectCast(control_.ObjetoControl, WclTextBoxFinder).tbKey.Text

                        If presentacionDatos_ Then

                            valorAsignadoCadena_ = _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion)

                            'DirectCast(control_.ObjetoControl, WclTextBoxFinder).tbKey.Text = valorAsignadoCadena_

                            DirectCast(control_.ObjetoControl, WclTextBoxFinder).SetKeyValue = valorAsignadoCadena_

                        Else

                            If valorAsignadoCadena_ <> "-1" Then

                                _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = valorAsignadoCadena_
                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.Vinculacion) = Nothing

                            End If

                        End If

                    End If

                Else

                    With control_

                        If .CampoVirtual.TipoDato = IEntidadDatos.TiposDatos.Booleno Then

                            valorAsignadoCadena_ = DirectCast(.ObjetoControl, CheckBox).Checked.ToString

                            If presentacionDatos_ Then

                                DirectCast(.ObjetoControl, CheckBox).Checked = _ioperacionescatalogo.CampoPorNombre(control_.IDControl)

                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = valorAsignadoCadena_

                            End If

                        ElseIf .CampoVirtual.TipoDato = IEntidadDatos.TiposDatos.Fecha Then

                            valorAsignadoCadena_ = DirectCast(.ObjetoControl, DateTimePicker).Value.ToString("dd/MM/yyyy HH:mm:ss")

                            If presentacionDatos_ Then

                                If IsDate(_ioperacionescatalogo.CampoPorNombre(control_.IDControl)) Then
                                    DirectCast(.ObjetoControl, DateTimePicker).Value = _ioperacionescatalogo.CampoPorNombre(control_.IDControl)

                                    DirectCast(.ObjetoControl, DateTimePicker).Checked = True

                                Else
                                    DirectCast(.ObjetoControl, DateTimePicker).Value = Now

                                    DirectCast(.ObjetoControl, DateTimePicker).Checked = False

                                End If

                            Else

                                If DirectCast(.ObjetoControl, DateTimePicker).Checked = True Then

                                    _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = valorAsignadoCadena_

                                Else
                                    _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = Nothing

                                End If

                            End If

                        ElseIf .CampoVirtual.TipoDato = IEntidadDatos.TiposDatos.Entero Then

                            valorAsignadoCadena_ = DirectCast(.ObjetoControl, MaskedTextBox).Text

                            If presentacionDatos_ Then
                                DirectCast(.ObjetoControl, MaskedTextBox).Text = _ioperacionescatalogo.CampoPorNombre(control_.IDControl)
                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = valorAsignadoCadena_
                            End If

                        ElseIf .CampoVirtual.TipoDato = IEntidadDatos.TiposDatos.Real Then

                            valorAsignadoCadena_ = DirectCast(.ObjetoControl, MaskedTextBox).Text

                            If presentacionDatos_ Then
                                DirectCast(.ObjetoControl, MaskedTextBox).Text = _ioperacionescatalogo.CampoPorNombre(control_.IDControl)
                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = valorAsignadoCadena_
                            End If


                        ElseIf .CampoVirtual.TipoDato = IEntidadDatos.TiposDatos.Texto Then

                            valorAsignadoCadena_ = DirectCast(.ObjetoControl, TextBox).Text

                            If presentacionDatos_ Then
                                DirectCast(.ObjetoControl, TextBox).Text = _ioperacionescatalogo.CampoPorNombre(control_.IDControl)
                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = valorAsignadoCadena_
                            End If


                        ElseIf .CampoVirtual.TipoDato = IEntidadDatos.TiposDatos.SinDefinir Then

                            valorAsignadoCadena_ = DirectCast(.ObjetoControl, TextBox).Text

                            If presentacionDatos_ Then
                                DirectCast(.ObjetoControl, TextBox).Text = _ioperacionescatalogo.CampoPorNombre(control_.IDControl)
                            Else
                                _ioperacionescatalogo.CampoPorNombre(control_.IDControl) = valorAsignadoCadena_
                            End If

                        End If

                    End With

                End If

            Next

            _ioperacionescatalogo.CampoPorNombre("i_Cve_Estado") = 1

        End If

    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click

        If _ioperacionescatalogo.PreparaCatalogo = IOperacionesCatalogo.EstadoOperacion.COk Then

            Select Case _modalidadoperativa

                Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                    RaiseEvent AntesCargarDatosInsercion()

                    AsignacionAutomatica(_modalidadoperativa)

                    RealizarInsercion()

                    IndicacionesInternasDespuesCargaDatosInsercion()

                    InvocarOperacion()

                    RaiseEvent DespuesInsercionCometida()


                Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                    RaiseEvent AntesCargarDatosEdicion()

                    AsignacionAutomatica(_modalidadoperativa)

                    RealizarModificacion()

                    IndicacionesInternasDespuesCargaDatosEdicion()

                    InvocarOperacion()

                    RaiseEvent DespuesEdicionCometida()

                Case Else

                    MsgBox("No hay implementación para esta modalidad")

            End Select

        Else
            'No se ha cargado adecuadamente la configuración del catálogo
            LblMensaje.Text = "No se ha cargado IOperaciones Adecuadamente"

        End If

        RaiseEvent EventoClickAceptarProcesado()

    End Sub


    Private Sub FormularioBase64_KeyDown(sender As Object, e As KeyEventArgs) _
        Handles MyBase.KeyDown

        Select Case e.KeyData

            Case Keys.Escape

                Me.Close()

            Case Keys.Enter

                If _focuBuscar Then

                    btnAceptar.PerformClick()

                End If


        End Select

    End Sub

    Public Sub PreparaListaDeplegable(ByRef combobox_ As ComboBox, _
                                      ByVal vistaiuconfiguracion_ As String, _
                                      ByVal vistapresentacion_ As String, _
                                      ByVal campoparamostrar_ As String, _
                                      ByVal campodevalor_ As String, _
                                      Optional ByVal clausulalibre_ As String = Nothing)

        Dim ioperacioneslistadesplegable_ As IOperacionesCatalogo

        Dim valor_ As String = "DESC"

        ioperacioneslistadesplegable_ = New OperacionesCatalogo


        ioperacioneslistadesplegable_.OrdenarResultados(0) = 0

        ioperacioneslistadesplegable_.VistaEncabezados = vistaiuconfiguracion_

        ioperacioneslistadesplegable_.OperadorCatalogoConsulta = vistapresentacion_

        ioperacioneslistadesplegable_.CantidadVisibleRegistros = 500

        ioperacioneslistadesplegable_.ClausulasLibres = clausulalibre_


        ioperacioneslistadesplegable_.GenerarVista()

        If ioperacioneslistadesplegable_.Vista.Tables.Count > 0 Then

            combobox_.DataSource = ioperacioneslistadesplegable_.Vista.Tables(0)

            combobox_.DisplayMember = campoparamostrar_

            combobox_.ValueMember = campodevalor_

        Else

            combobox_.Items.Clear()

            combobox_.Items.Add("<Vacío>")

        End If

    End Sub

    Public Sub PreparaListaDeplegable(ByRef combobox_ As ComboBox, _
                                      ByVal vistaiuconfiguracion_ As String, _
                                      ByVal vistapresentacion_ As String, _
                                      ByVal campoparamostrar_ As String, _
                                      ByVal campodevalor_ As String, _
                                      ByVal clausulalibre_ As String, _
                                      ByVal columnaOrdenar_ As Int32, _
                                      ByVal tipoOrdenamiento_ As Int32)

        Dim cantidadRegistros_ As Int32 = 500

        Dim ioperacioneslistadesplegable_ As IOperacionesCatalogo

        Dim valor_ As String = "DESC"

        ioperacioneslistadesplegable_ = New OperacionesCatalogo

        Select Case tipoOrdenamiento_

            Case 0 : valor_ = "DESC"
            Case 1 : valor_ = "ASC"
            Case 2 : valor_ = "INDEFINIDO"

        End Select

        ioperacioneslistadesplegable_.OrdenarResultados(columnaOrdenar_) = tipoOrdenamiento_

        ioperacioneslistadesplegable_.VistaEncabezados = vistaiuconfiguracion_

        ioperacioneslistadesplegable_.OperadorCatalogoConsulta = vistapresentacion_

        ioperacioneslistadesplegable_.CantidadVisibleRegistros = cantidadRegistros_

        ioperacioneslistadesplegable_.ClausulasLibres = clausulalibre_

        ioperacioneslistadesplegable_.GenerarVista()

        If ioperacioneslistadesplegable_.Vista.Tables.Count > 0 Then

            combobox_.DataSource = ioperacioneslistadesplegable_.Vista.Tables(0)

            combobox_.DisplayMember = campoparamostrar_

            combobox_.ValueMember = campodevalor_

        Else
            combobox_.Items.Clear()

            combobox_.Items.Add("<Vacío>")

        End If

    End Sub

    Public Sub PreparaListaDeplegable(ByRef combobox_ As ComboBox, _
                                      ByVal vistaiuconfiguracion_ As String, _
                                      ByVal vistapresentacion_ As String, _
                                      ByVal campoparamostrar_ As String, _
                                      ByVal campodevalor_ As String, _
                                      ByVal clausulalibre_ As String, _
                                      ByVal columnaOrdenar_ As Int32, _
                                      ByVal tipoOrdenamiento_ As Int32,
                                      ByVal cantidadRegistros_ As Int32)

        Dim ioperacioneslistadesplegable_ As IOperacionesCatalogo

        Dim valor_ As String = "DESC"

        ioperacioneslistadesplegable_ = New OperacionesCatalogo

        Select Case tipoOrdenamiento_

            Case 0 : valor_ = "DESC"
            Case 1 : valor_ = "ASC"
            Case 2 : valor_ = "INDEFINIDO"

        End Select

        ioperacioneslistadesplegable_.OrdenarResultados(columnaOrdenar_) = tipoOrdenamiento_

        ioperacioneslistadesplegable_.VistaEncabezados = vistaiuconfiguracion_

        ioperacioneslistadesplegable_.OperadorCatalogoConsulta = vistapresentacion_

        ioperacioneslistadesplegable_.CantidadVisibleRegistros = cantidadRegistros_

        ioperacioneslistadesplegable_.ClausulasLibres = clausulalibre_

        ioperacioneslistadesplegable_.GenerarVista()

        If ioperacioneslistadesplegable_.Vista.Tables.Count > 0 Then

            combobox_.DataSource = ioperacioneslistadesplegable_.Vista.Tables(0)

            combobox_.DisplayMember = campoparamostrar_

            combobox_.ValueMember = campodevalor_

        Else
            combobox_.Items.Clear()

            combobox_.Items.Add("<Vacío>")

        End If

    End Sub


#End Region


End Class

Public Class ControlDinamico

    Public ObjetoControl As Control
    Public CampoVirtual As CampoVirtual
    Public IDControl As String
    Public CampoLlave As ICaracteristica.TipoLlave
    Public NombreCampoLlave As String
    Public CampoDesplegable As String
    Public NameAsKey As String
    Public ValorDefault As String
    Public TipoRigorDato As ICaracteristica.TiposRigorDatos
    Public TipoDato As ICaracteristica.TiposCaracteristica
    Public Visible As ICaracteristica.TiposVisible
    Public TipoControlEspecial As String
    Public Vinculacion As String
    Public Auxiliares As List(Of ParAuxiliares)


    Sub New()

    End Sub

    Sub New(ByVal objetoControl_ As Control,
        ByVal campoVirtual_ As CampoVirtual,
        ByVal idControl_ As String,
        ByVal campoLlave_ As Boolean,
        ByVal nameAsKey_ As String)
        ObjetoControl = objetoControl_
        CampoVirtual = campoVirtual_
        IDControl = idControl_
        ValorDefault = Nothing
        CampoLlave = campoLlave_
        CampoDesplegable = Nothing
        NameAsKey = nameAsKey_
        TipoControlEspecial = Nothing
        Vinculacion = Nothing
    End Sub
End Class

