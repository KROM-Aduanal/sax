Imports System.Net.Mime
Imports gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Rec.Globals.InstitucionBancaria
Imports Rec.Globals.Utils
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposAcuseValor
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Operaciones
Imports Syn.Utils
Imports Cube.Validators
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Cube.Interpreters

Public Class Ges022_001_CuboDatos
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                 Defina en esta región sus atributos o propiedades locales                      ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Private _ctrlCube As ICubeController

    Private _ctrlInterpreter As IMathematicalInterpreter

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    'EVENTO INICIALIZADOR
    Public Overrides Sub Inicializa()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Configure la barra de búsquedas para el módulo                                            '
        ' Asigne una instancia de su clase constructura "Preasignación" en la propiedad DataObject  '
        ' Asigne n cantidad de filtros u opciones de consulta para su documento "Preasignación"     '
        '  -defina la seccion donde quiere consultar                                                '
        '  -defina el campo que debe consultar en la seccio dada                                    '
        '  -defina un titulo a los resultados de su filtro                                          '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If _ctrlCube Is Nothing Then

            _ctrlCube = New CubeController


        End If

        With Buscador

            '.DataObject = New ControladorAcuseValor
            'Dim dictionaryKey_ As New Dictionary(Of Object, Object) From {{"Value", "ZERG"}, {"Text", "Chitly"}}
            ''Dim keyValuePair_ As New KeyValuePair(Of Object, Dictionary(Of Object, Object))("Value", dictionaryKey_)
            'Dim dictionary_ As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object) From {{"Habitación", New List(Of Dictionary(Of Object, Object)) From {dictionaryKey_}}}


            ''Dim algo_ As New Dictionary(Of Object, Object) From {{"Value", "3*8"}}

            '.DataSource = dictionary_

            '.addFilter(1, 1, "Cubo de Datos")
            '.addFilter(SeccionesAcuseValor.SAcuseValor1, CamposFacturaComercial.CA_NUMERO_FACTURA, "FACTURA")
            '.addFilter(SeccionesAcuseValor.SAcuseValor2, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")
            '.addFilter(SeccionesAcuseValor.SAcuseValor3, CamposDestinatario.CA_RAZON_SOCIAL, "Destinatario")
            '.addFilter(SeccionesAcuseValor.SAcuseValor4, CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, "Descripción A.V.")

        End With

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' SeccionesClientes.SCS1         = ID de la sección en nuestro documento donde se quiere buscar              '
        ' CamposClientes.CA_RAZON_SOCIAL = ID del campo dentro de la sección asignada donde se realizara la búsqueda '
        ' "Cliente"                      = Titulo personalizado para el filtro                                       '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' NOTAS A CONSIDERAR                                                                                               '
        ' -----------------------------------------------------------------------------------------------------------------'
        ' SESIONES: para el uso de secciones utilice los métodos:                                                          '
        '                                                                                                                  '
        ' SetVars(ByVal var_ As String, Optional ByVal value_ As Object = Nothing)                                         '
        ' GetVars(ByVal var_ As String, Optional ByVal defaultValue_ As Object = Nothing)                                  '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' BOTONERA ESTADO INICIAL: si se desea tener un estado inicial de la botera distinto a lo que ofrece por defecto,  '
        ' sobreescriba el método Public Overridable Sub InicializaBotonera() y asigne la modalidad deseada                 '
        '                                                                                                                  '
        ' Formulario.Modality = FormControl.ButtonBarModality.Open                                                         '
        '                                                                                                                  '
        ' Formulario es una propiedad global que hace referencia a nuestro formulario en el marcado.                       '
        ' asegúrate que dicha asignación ocurra solo cuando no hay postback, coloquelo dentro del siguiente IF             '
        ' If Not Page.IsPostBack Then ..... EndIf                                                                          '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' BOTONERA CAMBIO DE ESTADO: Si se desea cambiar el estado de la botonera en cualquier otro momento como           '
        ' al desencadenar un evento utilice el método PreparaBotonera(ByVal modality_ As [Enum]) y asigne                  '
        ' el estado deseado                                                                                                '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' TARJETEROS CAMBIO DE ESTADO: para cambiar el estado en un tarjetero utilice el siguiente método                  '
        '                                                                                                                  '
        ' PreparaTarjetero(ByVal modality_ As [Enum], ByRef tarjetero_ As PillboxControl)                                  '
        ' Designe la modalidad y el ID de su PillboxControl                                                                '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' ACTIVAR/DESACTIVAR FORMULARIO                                                                                    '
        ' si desea activar o desactivar los controles en el formulario en algun caso especial utilice el siguiente método  '
        '                                                                                                                  '
        ' ActivaControles(Optional ByVal activar_ As Boolean = True)                                                       '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'
        ' MOSTRAR MENSAJES                                                                                                 '
        ' DisplayMessage(ByVal message_ As String, Optional ByVal status_ As StatusMessage = StatusMessage.Success)        '
        '                                                                                                                  '
        ' message_  = contenido del mensaje a mostrar al usuario                                                           '
        ' status_   = por defecto siempre es success                                                                       '
        ' -----------------------------------------------------------------------------------------------------------------'
        ' VENTANAS DE DIALOGO                                                                                              '
        ' DisplayAlert(ByVal title_ As String,                                                                             '
        '                    ByVal message_ As String,                                                                     '
        '                    ByVal argument_ As String,                                                                    '
        '                    Optional accept_ As String = "Entendido",                                                     '
        '                    Optional reject_ As String = Nothing)                                                         '
        '                                                                                                                  '
        ' title_  = contenido del título de la ventana de dialogo                                                          '
        ' message_ = contenido del mensaje de  la ventana de dialogo                                                       '
        ' argument_ = valor custom por el programador para evaluarlo y realizar acciones a conveniencia                    '
        ' accept_ = titulo del boton por defecto del dialogo                                                               '
        ' reject_ = titulo del boton de cancelar, cuando se definen ambos botones en automatico se convierte               '
        ' en una ventana de conformación y sus eventos son capturables en el código para realizar alguna tarea             '
        '                                                                                                                  '
        ' todas la ventanas de dialogo ejecutaran los siguientes métodos he alli donde la propiedad arguement_             '
        ' tiene sentido, sobre escriba los métodos en su código                                                            '
        '                                                                                                                  '
        ' Public Overridable Sub AceptaConfirmacion(argument_ As String)                                                   '
        ' Public Overridable Sub RechazaConfirmacion(argument_ As String)                                                  '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




        _ctrlInterpreter = GetVars("_interpreterController")

        If _ctrlInterpreter Is Nothing Then

            _ctrlInterpreter = New MathematicalInterpreterNCalc

        End If

        SetVars("_cubeController", _ctrlCube)

        SetVars("_interpreterController", _ctrlInterpreter)

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher


        'Datos Generales
        'Case SeccionesACUSEVALOR.SACUSEVALOR1

        Return New TagWatcher(Ok)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción nuevo (+) '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Este método se manda llamar al dar clic en el boton Guardar                             '
        ' Llamamos el método "ProcesarTransaccion" pasando el tipo de nuestra clase constructora  '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Dim acuseValorFindBar_ As ConstructorAcuseValor = GetVars("_AcuseValorFindBar")

        ' If Not ProcesarTransaccion(Of ConstructorAcuseValor)().Status = TypeStatus.Errors Then : End If



        Dim algo_ = _ctrlCube.SetFormula(Of String)(ic_RoomName.Value, ic_RoomRules.Value, sc_DestinoCubo.Text.ToString, sc_TipoRegla.Text.ToString.ToLower)

        If algo_.Status = Ok Then

            DisplayMessage("Regla Asignada Satisfactoriamente", Ok)

        End If

    End Sub

    Public Overrides Sub BotoneraClicPublicar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Publicar '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''

        With Buscador

            'Dim Algo = GetVars("_AcuseValorFindBar")

            'Dim TagWatcher_ = _icontroladorAcuseValor.GenerarAcuseValor(GetVars("_AcuseValorFindBar"))

            'If TagWatcher_.ObjectReturned <> "" Then

            '    DisplayMessage("SU ACUSE DE VALOR HA SIDO GENERADO", StatusMessage.Info)

            '    ic_ReglaGajo.ValueDetail = TagWatcher_.ObjectReturned

            'End If

        End With

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Seguir Editando '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pb_PartidasAcuseValor)

        'PreparaBotonera(FormControl.ButtonbarModality.Draft)

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Borrar'
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub BotoneraClicArchivar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Archivar '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en cualquiera de las opciones del      '
        ' dropdown en la botonera; recibe el valor indice del boton al que se le ha dado '
        ' clic                                                                           '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Dim factormonedas_ As Dictionary(Of String, FactorMonedaPrincipal) = GetVars("_FactoresMonedas")
        'If factormonedas_ IsNot Nothing Then
        'DisplayMessage("Factor: $" & factormonedas_(sc_TipoMoneda.Value.ToString).valorfactor & " al " & factormonedas_(sc_TipoMoneda.Value.ToString).Fecha.ToString("dd-MM-yyyy"))

        Select Case IndexSelected_

            Case 7

                Dim interpreterController_ As IMathematicalInterpreter = GetVars("_interpreterController")

                If interpreterController_ Is Nothing Then

                    interpreterController_ = New MathematicalInterpreterNCalc

                    SetVars("_interpreterController", interpreterController_)

                End If

                Dim cubeController_ As ICubeController = GetVars("_cubeController")

                If cubeController_ Is Nothing Then

                    cubeController_ = New CubeController

                End If

                interpreterController_.addOperands(cubeController_.GetOperands().ObjectReturned)

                Dim Values_ = New Dictionary(Of String, Object)

                Dim icontroladorMonedas_ As IControladorMonedas = New ControladorMonedas

                Dim params_ = interpreterController_.GetParams(ic_RoomRules.Value)

                Dim rnd_ As New Random()



                For Each param_ In params_

                    Dim numeroAleatorio As Integer = rnd_.Next(1, 1000)

                    If Not Values_.ContainsKey(param_) Then

                        Dim position_ = param_.LastIndexOf(".")

                        Dim positionFound_ As Boolean

                        If position_ = -1 Then

                            positionFound_ = False

                        Else

                            positionFound_ = Int32.TryParse(param_.Substring(position_ + 1), position_)

                        End If

                        If positionFound_ Then

                            Values_.Add(param_, numeroAleatorio)

                        Else

                            Values_.Add(param_ & ".0", numeroAleatorio)

                        End If



                    End If

                Next

                Dim found_ = True

                If cc_ValoresOperandos.DataSource IsNot Nothing Then

                    For Each elementos_ In cc_ValoresOperandos.DataSource

                        Dim operandName_ = elementos_("operandName_").ToString

                        Dim operandNameFull = elementos_("operandName_").ToString

                        Dim position_ = operandName_.LastIndexOf(".")



                        operandName_ = operandName_.Substring(position_ + 1)

                        Dim doubleValue_ As Double

                        If Double.TryParse(operandName_, doubleValue_) Then

                            operandNameFull = elementos_("operandName_").ToString

                            operandName_ = operandNameFull.Substring(0, position_)

                        Else



                            operandName_ = operandNameFull

                            If found_ Then

                                position_ = Values_.Keys.Where(Function(ch) ch.Contains(operandName_)).Count - 1

                                found_ = False

                            Else

                                position_ = Values_.Keys.Where(Function(ch) ch.Contains(operandName_)).Count


                            End If


                            If position_ = -1 Then

                                position_ = 0

                            End If

                            operandNameFull = operandName_ & "." & position_
                            ' position = Values_.Keys.Where(Function(ch) ch.Contains(operandName_)).Count)

                        End If

                        If Values_.Keys.Where(Function(ch) ch.Contains(operandName_)).Count > 0 Then

                            Values_(operandNameFull) = elementos_("operandValue_")

                        End If


                    Next

                End If

                Dim cuenta_ = 1

                cc_ValoresOperandos.ClearRows()

                For Each valor_ In Values_

                    cc_ValoresOperandos.SetRow(Sub(catalogRow_ As CatalogRow)

                                                   'Define el valor Llave de tu fila

                                                   catalogRow_.SetIndice(cc_ValoresOperandos.KeyField, cuenta_)

                                                   'Define el valor de una columna de la fila



                                                   Dim algo_ As New InputControl With {.ID = "operandName_",
                                                                                           .Value = valor_.Key,
                                                                                           .Type = InputControl.InputType.Text}

                                                   Dim algo2_ As New InputControl With {.ID = "operandValue_",
                                                                                           .Value = valor_.Value,
                                                                                           .Type = InputControl.InputType.Text}



                                                   catalogRow_.SetColumn(algo_, valor_.Key)

                                                   catalogRow_.SetColumn(algo2_, valor_.Value)


                                                   'de esta manera agregamos todas las columnas de nuestra fila 
                                                   'usando el control asociado a la columna y el valor que se asignara

                                               End Sub)

                    cuenta_ += 1

                Next

                cc_ValoresOperandos.CatalogDataBinding()

                DisplayMessage(interpreterController_.RunExpression(Of Object)(ic_RoomRules.Value, Values_), TypeStatus.OkInfo)

            Case 8

                ic_RoomName.Value = ""

                ic_RoomRules.Value = ""

                sc_DestinoCubo.Value = 1

                sc_TipoRegla.Value = 1

                cc_ValoresOperandos.ClearRows()

                cc_ValoresOperandos.CatalogDataBinding()

            Case 9

                Dim cubeController_ As ICubeController = GetVars("_cubeController")

                If cubeController_ Is Nothing Then

                    cubeController_ = New CubeController

                End If

                cubeController_.GetStatus(ObjectId.Parse(ic_RoomRules.Value))

        End Select


    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local
            '████████████████████████


            'pb_PartidasCOVE.DeletePillbox()
            '  ████████fin█████████       Logica de negocios local       ███████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_


    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim secuencia_ As New Secuencia _
              With {.anio = 0,
                    .environment = Statements.GetOfficeOnline()._id,
                    .mes = 0,
                    .nombre = "ACUSEVALOR",
                    .tiposecuencia = 1,
                    .subtiposecuencia = 0
                    }

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        __SYSTEM_MODULE_FORM.Modality = FormControl.ButtonbarModality.Draft
        'PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pb_PartidasAcuseValor)
        Return New TagWatcher(Ok)

    End Function


    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)


    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        'MsgBox("LLEGA AQUÍ")
        Return New TagWatcher(Ok)


    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar justo al seleccionar uno de los resultados de la busqueda general       '
        ' Aqui ocurre el llenado del formulario                                                               '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al realizar una consulta en la barra de busqueda y obtenemos resultados '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al realizar una consulta en la barra de busqueda y no obtenemos resultados '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar la primera vez que carga la página y despues de culminar una transacción '
        ' importante limpies tus variables de sessión aqui                                                     '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub Limpiar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar la primera vez que carga la página y despues de culminar una transacción '
        ' importante limpies tus controles asigando un Value o DataSource en Nothing                           '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub




#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                 Defina en esta región su lógica de negocio para este módulo                    ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Public Sub LimpiarTodo()

        ic_RoomName.Value = ""
        ic_RoomRules.Value = ""
        sc_DestinoCubo.Value = 1
        sc_TipoRegla.Value = 1


    End Sub

    Public Sub BuscarGajo()
        'Dim keyValuePair_ As New KeyValuePair(Of Object, Dictionary(Of Object, Object))("Value", dictionaryKey_)

        Dim roomdictionary_ As New Dictionary(Of ObjectId, Room)

        Dim dictionary_ As New Dictionary(Of Object, Object) From {{"Habitación", New List(Of Dictionary(Of Object, Object))}}

        Dim TagWartcher_ = _ctrlCube.GetRoomNames(Buscador.Text)

        For Each room_ As Room In TagWartcher_.ObjectReturned

            dictionary_("Habitación").add(New Dictionary(Of Object, Object) From {{"Value", room_._id.ToString},
                                                                                   {"Text", room_.roomname}})
            roomdictionary_(room_._id) = room_

        Next

        SetVars("_roomdictionary_", roomdictionary_)

        Buscador.DataSource = dictionary_

        Buscador.DataBind()


    End Sub

    Public Sub CargarGajo(sender_ As Object, e As EventArgs)

        Dim rooms_ As Dictionary(Of ObjectId, Room) = GetVars("_roomdictionary_")


        If sender_.Value.ToString() <> "" Then

            Dim objectId_ = ObjectId.Parse(sender_.Value.ToString())

            Dim room_ = rooms_(objectId_)

            Dim pointPosition_ = room_.roomname.IndexOf(".")


            ic_RoomName.Value = room_.roomname.Substring(pointPosition_ + 1)

            Select Case room_.roomname.Substring(0, pointPosition_ - 1)

                Case "A22"

                    sc_DestinoCubo.Value = 1

                Case "VOCE"

                    sc_DestinoCubo.Value = 2
                Case "UCC"

                    sc_DestinoCubo.Value = 3
                Case "UCAA"

                    sc_DestinoCubo.Value = 4
                Case "UAA"

                    sc_DestinoCubo.Value = 5
                Case "CDI"

                    sc_DestinoCubo.Value = 6

                Case Else

                    sc_DestinoCubo.Value = 1

            End Select

            ic_RoomRules.Value = room_.rules

            Select Case room_.contenttype

                Case "formula"

                    sc_TipoRegla.Value = 1

                Case "operando"

                    sc_TipoRegla.Value = 2

                Case "csv"

                    sc_TipoRegla.Value = 3

                Case "json"

                    sc_TipoRegla.Value = 4

            End Select

        End If






    End Sub






#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██ Defina en esta región todo lo que involucre el uso de controladores externos al contexto actual██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


#End Region


End Class