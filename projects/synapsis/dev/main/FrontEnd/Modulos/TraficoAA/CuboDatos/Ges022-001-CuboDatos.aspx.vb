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
Imports System.Runtime.Remoting
Imports System.Windows.Forms
Imports MongoDB.Bson.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Sax


Public Class Ges022_001_CuboDatos
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                 Defina en esta región sus atributos o propiedades locales                      ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Private _ctrlCube As ICubeController

    Private _ctrlInterpreter As IMathematicalInterpreter

    Private _idRoom As ObjectId

    Public Property _accionDate As String
    Public Property _accionDate2 As String
    Public Property _accionDate3 As String
    Public Property _userImage As String
    Public Property _userImage2 As String
    Public Property _userImage3 As String

    Public Property _userName As String

    Public Property _userName2 As String

    Public Property _userName3 As String

    Public Property _accionText As String
    Public Property _accionText2 As String
    Public Property _accionText3 As String
    '<GWC-userdata _userName="Sergionor Flores Martínez" _accionDate="hace 3 días" _userImage="/FrontEnd/Librerias/Krom/imgs/nouser.png" ID="ud_UserData34" Name="ud_UserData34"></GWC-userdata>

    Private _organismo As Organismo


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

        _ctrlCube = GetVars("_cubeController")

        If _ctrlCube Is Nothing Then

            _ctrlCube = New CubeController

            _ctrlInterpreter.SetValidFields(_ctrlCube.GetFieldsNamesResource().ObjectReturned)

        End If

        SetVars("_cubeController", _ctrlCube)

        SetVars("_interpreterController", _ctrlInterpreter)

        _userName = GetVars("_userName")

        _accionDate = GetVars("_accionDate")

        _userImage = "/FrontEnd/Librerias/Krom/imgs/nouser.png"

        _idRoom = GetVars("_idRoom")

        If GetVars("_cubeSource") = "" Then

            SetVars("_cubeSource", "A22")


        End If

        If IsPostBack Then

            SetVars("_filled", "")

            SetVars("_userdatas", False)

        End If

        bc_LimpiarFormula.Enabled = True

        bc_SourceCube.Label = GetVars("_cubeSource")

        bc_SourceCubeChange.Label = GetVars("_cubeSource")

        ShowUserData()

        'bc_Verificado.ForeColor = Drawing.Color.DarkGray

        'bc_VerificadoNueva.ForeColor = Drawing.Color.DarkGray
    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher


        'Datos Generales
        'Case Secciones CUBO




        Return New TagWatcher(Ok)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción nuevo (+) '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''



    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Este método se manda llamar al dar clic en el boton Guardar                             '
        ' Llamamos el método "ProcesarTransaccion" pasando el tipo de nuestra clase constructora  '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Dim acuseValorFindBar_ As ConstructorAcuseValor = GetVars("_AcuseValorFindBar")

        ' If Not ProcesarTransaccion(Of ConstructorAcuseValor)().Status = TypeStatus.Errors Then : End If

        Dim bc_comparavisible As ButtonControl

        Dim bc_PendienteAutorizacion_ As ButtonControl

        Dim branchName_ As String

        Dim swconline_ As SwitchControl

        Dim bc_function_ As ButtonControl





        If tb_Formula.Enabled Then

            If tb_Formula.Text <> GetVars("_filled") Then

                ' bc_Verificado.Visible = False

                bc_Verificado.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

            bc_comparavisible = bc_Verificado

            bc_PendienteAutorizacion_ = bc_PorAutorizar

            branchName_ = bc_SourceCube.Label

            swconline_ = swc_Online

            bc_function_ = bc_Function

        Else

            'bc_Verificado.Visible = False
            bc_Verificado.Enabled = False



            If tb_FormulaNueva.Text <> GetVars("_filled") Then

                bc_VerificadoNueva.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

            bc_comparavisible = bc_VerificadoNueva

            bc_PendienteAutorizacion_ = bc_PorAutorizarNueva

            branchName_ = bc_SourceCubeChange.Label

            swconline_ = swc_OnlineNueva

            bc_function_ = bc_FunctionChange

        End If




        If bc_PendienteAutorizacion_.Enabled Then

            DisplayMessage("No se puede actualizar porque está pendiente de autorización", TypeStatus.OkBut)

        Else




            If bc_comparavisible.Enabled Then


                Dim rulesType_, status_ As String


                If bc_function_.Visible Then

                    rulesType_ = "formula"

                Else

                    rulesType_ = "operando"


                End If

                If swconline_.Checked Then
                    status_ = "on"

                Else
                    status_ = "off"

                End If


                Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                _userName = loginUsuario_("WebServiceUserID")

                Dim roomName_, rules_ As String


                If tb_Formula.Enabled Then

                    roomName_ = ic_RoomName.Value

                    rules_ = tb_Formula.Text

                    ic_changeReason.Value = ""

                Else

                    roomName_ = ic_RoomNameNew.Value

                    rules_ = tb_FormulaNueva.Text

                End If

                If roomName_ = "" Then

                    DisplayMessage("Falta especificar el nombre de la habitación", TypeStatus.OkBut)

                Else

                    _ctrlInterpreter = GetVars("_interpreterController")

                    If _ctrlInterpreter Is Nothing Then

                        _ctrlInterpreter = New MathematicalInterpreterNCalc

                        _ctrlCube = GetVars("_cubeController")

                        If _ctrlCube Is Nothing Then

                            _ctrlCube = New CubeController

                            _ctrlInterpreter.SetValidFields(_ctrlCube.GetFieldsNamesResource().ObjectReturned)

                        End If

                        SetVars("_cubeController", _ctrlCube)

                        SetVars("_interpreterController", _ctrlInterpreter)

                    End If

                    If rulesType_ = "formula" AndAlso _ctrlInterpreter.GetValidFields.FindAll(Function(e) e.Contains(roomName_)).Count = 0 Then

                        DisplayMessage("Nombre inválido para una habitación de tipo fórmula", TypeStatus.OkBut)

                    Else


                        _idRoom = GetVars("_idRoom")


                        Dim tagwatcher_ = _ctrlCube.SetFormula(Of String)(_idRoom, roomName_, rules_, branchName_, rulesType_, ic_DescripcionRules.Value, status_, Nothing, userName_:=loginUsuario_("WebServiceUserID"), reason_:=ic_changeReason.Value)

                        If tagwatcher_.ObjectReturned IsNot Nothing Then

                            Dim room_ As room = tagwatcher_.ObjectReturned

                            ColocaHistorial(room_.historical)

                            DisplayMessage("Regla asignada satisfactoriamente", Ok)

                            SetVars("_userName", _userName)

                            SetVars("_accionDate", _accionDate)

                        Else


                            DisplayMessage("Ya existe una racámara con nombre " & roomName_ & " en " & branchName_, TypeStatus.OkBut)


                        End If

                    End If


                End If


            Else

                DisplayMessage("Esta regla no ha sido verificada", TypeStatus.OkBut)

            End If

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

        '__SYSTEM_MODULE_FORM.Modality = FormControl.ButtonbarModality.Draft

        '__SYSTEM_MODULE_FORM.Buttonbar.ButtonSource(3).Text = "Autorización"

        p_FormulaActual.Enabled = True

        bc_ElaborarPrueba.Enabled = False

        bc_LimpiarFormula.Enabled = False

        bc_ElaborarPruebaEditar.Enabled = True

        bc_LimpiarFormulaEditar.Enabled = True


        tb_FormulaNueva.Enabled = True

        ic_changeReason.Enabled = True

        bc_SourceCube.Label = GetVars("_cubeSource")

        bc_SourceCubeChange.Label = GetVars("_cubeSource")

        SetVars("_userdatas", False)

        ShowUserData()

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

        Dim bc_comparavisible As ButtonControl

        Dim bc_PendienteAutorizacion_ As ButtonControl

        Dim bc_ContenType_ As ButtonControl

        Dim swc_online_ As SwitchControl


        If tb_Formula.Enabled Then

            If tb_Formula.Text <> GetVars("_filled") Then

                'bc_Verificado.Visible = False
                bc_Verificado.Enabled = False




                SetVars("_bc_verificado", "NO")

            End If

            bc_comparavisible = bc_Verificado

            bc_PendienteAutorizacion_ = bc_PorAutorizar

            bc_ContenType_ = bc_Function

            swc_online_ = swc_Online

        Else

            ' bc_Verificado.Visible = False
            bc_Verificado.Enabled = False




            If tb_FormulaNueva.Text <> GetVars("_filled") Then

                bc_VerificadoNueva.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

            bc_comparavisible = bc_VerificadoNueva

            bc_PendienteAutorizacion_ = bc_PorAutorizarNueva

            bc_ContenType_ = bc_FunctionChange

            swc_online_ = swc_OnlineNueva


        End If

        Select Case IndexSelected_

            Case 7  'Está Opción será para mandar a Solicitar Autorización

                ' VerificarFormula()

                If ic_changeReason.Value = "" Then

                    DisplayMessage("Falta Escificar la razón de la autorización", TypeStatus.OkBut)

                    'ic_changeReason.ToolTip = "Obligatorio para solicitar autorización"

                    'ic_changeReason.ShowToolTip()

                Else

                    If bc_PendienteAutorizacion_.Enabled Then

                        DisplayMessage("Esta fórmula ya está pendiente de autorización", TypeStatus.OkBut)

                    Else

                        If bc_comparavisible.Enabled Then

                            Dim rulesType_, status_ As String


                            If bc_ContenType_.Visible Then

                                rulesType_ = "formula"

                            Else

                                rulesType_ = "operando"


                            End If

                            If swc_online_.Checked Then

                                status_ = "on"

                            Else

                                status_ = "off"

                            End If


                            Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                            _userName = loginUsuario_("WebServiceUserID")

                            Dim roomName_, rules_ As String


                            If tb_Formula.Enabled Then

                                roomName_ = ic_RoomName.Value

                                rules_ = tb_Formula.Text

                            Else

                                roomName_ = ic_RoomNameNew.Value

                                rules_ = tb_FormulaNueva.Text

                            End If

                            _idRoom = GetVars("_idRoom")

                            Dim tagwatcher_ = _ctrlCube.SetFormula(Of String)(_idRoom, roomName_, rules_, GetVars("_cubeSource"), rulesType_, ic_DescripcionRules.Value, status_, Nothing, userName_:=loginUsuario_("WebServiceUserID"), enviado_:="sent", reason_:=ic_changeReason.Value)

                            If tagwatcher_.Status = Ok Then

                                Dim room_ As room = tagwatcher_.ObjectReturned

                                ColocaHistorial(room_.historical)

                                DisplayMessage("Su solicitud ha sido enviada", TypeStatus.OkInfo)

                                SetVars("_userName", _userName)

                                SetVars("_accionDate", _accionDate)


                                bc_PendienteAutorizacion_.Enabled = True

                            End If


                        Else

                            DisplayMessage("Esta regla no ha sido verificada", TypeStatus.OkBut)


                        End If

                    End If

                End If




            Case 8  'Esta Opción limpia todo el formlario

                LimpiarTodo()

            Case 9

                'Dim cubeController_ As ICubeController = GetVars("_cubeController")

                'If cubeController_ Is Nothing Then

                '    cubeController_ = New CubeController

                'End If



                'cubeController_.UpdateRoomResource()

                'MsgBox("Recursos Actualizados Satisfactoriamente")

            Case 12 ' Esta Opción manda a autorizar la fórmula

                If bc_comparavisible.Enabled Then

                    Dim rulesType_, status_ As String






                    If swc_online_.Checked Then

                        status_ = "on"

                    Else

                        status_ = "off"

                    End If


                    Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                    _userName = loginUsuario_("WebServiceUserID")

                    Dim roomName_, rules_ As String


                    If tb_Formula.Enabled Then

                        roomName_ = ic_RoomName.Value

                        rules_ = tb_Formula.Text

                        bc_ContenType_ = bc_Function

                    Else

                        roomName_ = ic_RoomNameNew.Value

                        rules_ = tb_FormulaNueva.Text

                        bc_ContenType_ = bc_FunctionChange

                    End If

                    If bc_ContenType_.Visible Then

                        rulesType_ = "formula"

                    Else

                        rulesType_ = "operando"


                    End If

                    _idRoom = GetVars("_idRoom")

                    Dim tagwatcher_ = _ctrlCube.SetFormula(Of String)(_idRoom, roomName_, rules_, GetVars("_cubeSource"), rulesType_, ic_DescripcionRules.Value, status_, Nothing, userName_:=loginUsuario_("WebServiceUserID"), enviado_:="on", reason_:=ic_changeReason.Value)

                    If tagwatcher_.Status = Ok Then

                        Dim room_ As room = tagwatcher_.ObjectReturned

                        ColocaHistorial(room_.historical)


                        DisplayMessage("El cambio ha sido autorizado", TypeStatus.Ok)

                        SetVars("_userName", _userName)

                        SetVars("_accionDate", _accionDate)



                    End If

                Else

                    DisplayMessage("Debes Verificar la fórmula antes de autorizarla", TypeStatus.OkBut)

                End If

            Case 13 'Esta opción es para mandar a desechar una fórmula

                If bc_comparavisible.Enabled Then

                    Dim rulesType_, status_ As String


                    If bc_Function.Visible Then

                        rulesType_ = "formula"

                    Else

                        rulesType_ = "operando"


                    End If

                    If swc_Online.Checked Then

                        status_ = "on"

                    Else

                        status_ = "off"

                    End If


                    Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                    _userName = loginUsuario_("WebServiceUserID")

                    Dim roomName_, rules_ As String


                    If tb_Formula.Enabled Then

                        roomName_ = ic_RoomName.Value

                        rules_ = tb_Formula.Text

                    Else

                        roomName_ = ic_RoomNameNew.Value

                        rules_ = tb_FormulaNueva.Text

                    End If

                    _idRoom = GetVars("_idRoom")

                    Dim tagwatcher_ = _ctrlCube.SetFormula(Of String)(_idRoom, roomName_, rules_, GetVars("_cubeSource"), rulesType_, ic_DescripcionRules.Value, status_, Nothing, userName_:=loginUsuario_("WebServiceUserID"), enviado_:="off", reason_:=ic_changeReason.Value)

                    If tagwatcher_.Status = Ok Then

                        Dim room_ As room = tagwatcher_.ObjectReturned

                        ColocaHistorial(room_.historical)


                        DisplayMessage("El cambio ha sido desechado", TypeStatus.Ok)

                        SetVars("_userName", _userName)

                        SetVars("_accionDate", _accionDate)



                    End If

                Else

                    DisplayMessage("Debes Verificar la fórmula para estar segura de desecharla", TypeStatus.OkBut)

                End If

            Case 14 'esta opción muestra las dos fórmulas para compararlas

                p_formulillas.Visible = True

                p_formulillas.CssClass = "col-md-6 col-xs-6"

                bc_ElaborarPrueba.Visible = False

                bc_LimpiarFormula.Visible = False

                p_actualizacionformula.CssClass = "col-md-6 col-xs-6"

                ic_RoomName.Enabled = False

                Dim historical_ As List(Of roomhistory) = GetVars("_historical")

                If historical_ IsNot Nothing Then

                    ColocaHistorial(historical_)

                End If

                bc_Verificado.Visible = False
                bc_PorAutorizar.Visible = False

            Case 15


                _ctrlCube.CamposExcelMongo("C:\ZERG\SYNAPSIS\CUBO\CAMPOSPEDIMENTOCSV.csv")



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
                    .nombre = "CUBO",
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
        '''



    End Sub




#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                 Defina en esta región su lógica de negocio para este módulo                    ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Public Sub LimpiarTodo()


        ic_RoomName.Value = ""

        tb_Formula.Text = ""

        bc_SourceCube.Label = "A22"

        bc_Function.Visible = True

        bc_Var.Visible = False

        ic_DescripcionRules.Value = ""

        ic_changeReason.Value = ""

        cc_ValoresOperandos.ClearRows()

        cc_ValoresOperandos.CatalogDataBinding()

        swc_Online.Checked = False

        'bc_Verificado.Visible = False
        bc_Verificado.Enabled = False

        bc_PorAutorizar.Enabled = False

        bc_VerificadoNueva.Enabled = False

        bc_PorAutorizarNueva.Enabled = False

        __SYSTEM_CONTEXT_FINDER.Text = ""

        __SYSTEM_CONTEXT_FINDER.Value = ""

        ic_changeReason.Enabled = True

        bi_SudoAutorizar.Visible = False

        bi_SudoDesechar.Visible = False

        bi_SolicitarAutorizacion.Visible = False

        SetVars("_userdatas", False)

        ShowUserData()



        SetVars("_bc_porautorizar", "SI")

        CargaInicialModulo()

    End Sub

    Public Sub BuscarGajo()
        'Dim keyValuePair_ As New KeyValuePair(Of Object, Dictionary(Of Object, Object))("Value", dictionaryKey_)

        Dim roomResourceDictionary_ As New Dictionary(Of ObjectId, roomresource)

        Dim dictionary_ As New Dictionary(Of Object, Object) From {{"Habitación", New List(Of Dictionary(Of Object, Object))}}

        Dim buscador_ = Buscador.Text.Replace(" | ", ".")



        Dim TagWatcher_ = _ctrlCube.GetRoomNamesResource(buscador_.Substring(buscador_.IndexOf(".") + 1))

        For Each roomResource_ As roomresource In TagWatcher_.ObjectReturned

            dictionary_("Habitación").add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                   {"Text", roomResource_.branchname & " | " & roomResource_.valorpresentacion}})
            roomResourceDictionary_(roomResource_._id) = roomResource_

        Next

        SetVars("_roomdictionary_", roomResourceDictionary_)

        Buscador.DataSource = dictionary_

        Buscador.DataBind()


    End Sub

    Public Sub CargarGajo(sender_ As Object, e As EventArgs)

        Dim roomsResource_ As Dictionary(Of ObjectId, roomresource) = GetVars("_roomdictionary_")

        bc_Verificado.Enabled = False

        bc_PorAutorizar.Enabled = False

        bc_VerificadoNueva.Enabled = False

        bc_PorAutorizarNueva.Enabled = False


        If sender_.Value.ToString() <> "" Then

            bc_Function.Enabled = True

            bc_FunctionChange.Enabled = True

            bc_Var.Enabled = True

            bc_VarChange.Enabled = True


            Dim objectId_ = ObjectId.Parse(sender_.Value.ToString())

            Dim roomResource_ = roomsResource_(objectId_)

            bc_SourceCube.Label = roomResource_.branchname

            Dim branchname_ As String = roomResource_.branchname



            Dim rooms_ As List(Of room) = _ctrlCube.GetRoom(roomResource_.idroom, roomResource_.rolid).ObjectReturned

            SetVars("_idRoom", roomResource_.idroom)

            SetVars("_idRoomResource", roomResource_._id)

            _organismo = New Organismo

            Dim currentUser_ As String

            If rooms_(0).historical Is Nothing Then

                currentUser_ = _organismo.GetCurrentUser(rooms_(0)._id.CreationTime, DateTime.Now)

                _userName = ""

            Else

                If rooms_(0).historical.Count = 0 Then

                    currentUser_ = _organismo.GetCurrentUser(rooms_(0)._id.CreationTime, DateTime.Now)

                    _userName = ""

                Else

                    ColocaHistorial(rooms_(0).historical)

                    SetVars("_userdatas", True)

                    ShowUserData()


                End If


            End If

            SetVars("_historical", rooms_(0).historical)

            SetVars("_userName", _userName)

            SetVars("_accionDate", _accionDate)

            If rooms_(0).contenttype = "formula" Then

                bc_Var.Visible = False

                bc_VarChange.Visible = False

                bc_Function.Visible = True

                bc_FunctionChange.Visible = True
            Else

                bc_Function.Visible = False

                bc_FunctionChange.Visible = False

                bc_Var.Visible = True

                bc_VarChange.Visible = True

            End If

            ic_RoomName.Value = roomResource_.valorpresentacion

            ic_RoomNameNew.Value = roomResource_.valorpresentacion

            tb_FormulaNueva.Text = rooms_(0).rules

            bc_SourceCubeChange.Label = roomResource_.branchname


            bi_SudoAutorizar.Visible = False

            If rooms_(0).awaitingupdates IsNot Nothing Then

                If rooms_(0).awaitingupdates.Count > 0 Then

                    If rooms_(0).awaitingupdates(0).status = "on" Or rooms_(0).awaitingupdates(0).status = "off" Then

                        ic_RoomNameNew.Value = rooms_(0).roomname.Substring(rooms_(0).roomname.IndexOf(".") + 1)

                        tb_FormulaNueva.Text = rooms_(0).rules

                        ic_changeReason.Value = ""

                        bc_SourceCubeChange.Label = roomResource_.branchname


                        bc_PorAutorizar.Enabled = False

                        bc_PorAutorizarNueva.Enabled = False



                    Else

                        Dim roomNameNew_ As String = rooms_(0).awaitingupdates(0).roomname

                        ic_RoomNameNew.Value = roomNameNew_.Substring(roomNameNew_.IndexOf(".") + 1)

                        tb_FormulaNueva.Text = rooms_(0).awaitingupdates(0).rules

                        ic_changeReason.Value = rooms_(0).awaitingupdates(0).reason

                        branchname_ = roomNameNew_.Substring(0, roomNameNew_.IndexOf("."))

                        bc_SourceCubeChange.Label = branchname_



                        If rooms_(0).awaitingupdates(0).status = "sent" Then

                            bc_PorAutorizar.Enabled = False

                            bc_PorAutorizarNueva.Enabled = True

                            bi_SudoAutorizar.Visible = True

                            bi_SudoDesechar.Visible = True

                            bc_Function.Enabled = False

                            bc_FunctionChange.Enabled = False

                            bc_Var.Enabled = False

                            bc_VarChange.Enabled = False

                        End If

                    End If


                End If


            End If


            tb_Formula.Text = rooms_(0).rules

            ic_DescripcionRules.Value = rooms_(0).description

            If rooms_(0).status = "on" Then

                swc_Online.Checked = True

            Else

                swc_Online.Checked = False

            End If

            SetVars("_cubeSource", branchname_)

            CargaEncontradolModulo()

            SetVars("_bc_porautorizar", "SI")

            bi_SolicitarAutorizacion.Visible = True

            bi_Comparar.Visible = True

            'bc_Verificado.Visible = False
            bc_Verificado.Enabled = False

            p_userdata.Visible = False

            p_userchange.Visible = True


            bc_VerificadoNueva.Enabled = False


            SetVars("_bc_verificado", "NO")

        End If

    End Sub


    Sub NewChangeContent()

        '  bc_VarChange.BackColor = Drawing.Color.Orange

        If bc_FunctionChange.Visible Then

            bc_FunctionChange.Visible = False

            bc_VarChange.Visible = True

        Else

            bc_FunctionChange.Visible = True

            bc_VarChange.Visible = False

        End If

        If GetVars("_userName") IsNot Nothing Then

            _userName = GetVars("_userName")

            _accionDate = GetVars("_accionDate")

        End If

    End Sub
    Sub CambioContenido()

        'bc_Var.BackColor = Drawing.Color.Orange

        If bc_Function.Visible Then

            bc_Function.Visible = False

            bc_Var.Visible = True

        Else

            bc_Var.Visible = False

            bc_Function.Visible = True

        End If


        If GetVars("_userName") IsNot Nothing Then

            _userName = GetVars("_userName")

            _accionDate = GetVars("_accionDate")

        End If


    End Sub

    Sub ColocaHistorial(historical_ As List(Of roomhistory))

        If historical_ IsNot Nothing Then

            If tb_Formula.Enabled Then

                p_userdata.Visible = True

            Else

                p_userchange.Visible = True
                p_userdata.Visible = False


            End If


            Dim cuenta_ = 1

            If _organismo Is Nothing Then

                _organismo = New Organismo

            End If

            For Each roomhstory_ In historical_

                Select Case cuenta_

                    Case 1

                        _accionDate = _organismo.GetCurrentUser(roomhstory_.createat, DateTime.Now)

                        _userName = roomhstory_.username

                        _accionText = roomhstory_.reason

                    Case 2

                        _accionDate2 = _organismo.GetCurrentUser(roomhstory_.createat, DateTime.Now)

                        _userName2 = roomhstory_.username

                        _accionText2 = roomhstory_.reason

                    Case 3

                        _userName3 = roomhstory_.username

                        _accionDate3 = _organismo.GetCurrentUser(roomhstory_.createat, DateTime.Now)

                        _accionText3 = roomhstory_.reason

                    Case Else

                        Exit For

                End Select

                cuenta_ += 1

            Next

        End If

    End Sub

    Sub VerificarFormula()



        Dim formulaFormato_ As String = tb_Formula.Text.Replace("[13]", vbCrLf)
        Dim formulaNuevaFormato_ As String = tb_FormulaNueva.Text.Replace("[13]", vbCrLf)

        If formulaFormato_ <> "" AndAlso
            (tb_Formula.Enabled OrElse
           (formulaNuevaFormato_ <> "" AndAlso
           tb_FormulaNueva.Enabled)) Then

            Dim formula_ As String


            If _ctrlInterpreter Is Nothing Then

                _ctrlInterpreter = New MathematicalInterpreterNCalc

                SetVars("_interpreterController", _ctrlInterpreter)

            End If

            If _ctrlCube Is Nothing Then

                _ctrlCube = New CubeController

                _ctrlInterpreter.SetValidFields(_ctrlCube.GetFieldsNamesResource().ObjectReturned)

            End If


            _ctrlInterpreter.addOperands(_ctrlCube.GetOperands().ObjectReturned)

            Dim Values_ = New Dictionary(Of String, Object)

            Dim icontroladorMonedas_ As IControladorMonedas = New ControladorMonedas

            Dim params_ As List(Of String)

            If tb_Formula.Enabled Then

                formula_ = formulaFormato_.Replace(vbCrLf, "")

            Else

                formula_ = formulaNuevaFormato_.Replace(vbCrLf, "")

            End If

            params_ = _ctrlInterpreter.GetParams(formula_)


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




            DisplayMessage(_ctrlInterpreter.RunExpression(Of Object)(formula_, Values_), TypeStatus.OkInfo)

            Dim report_ = _ctrlInterpreter.GetReportFull

            If report_.getTitle = "" Then

                Dim botonFormula_ As ButtonControl

                If tb_Formula.Enabled Then

                    botonFormula_ = bc_Function

                Else

                    botonFormula_ = bc_FunctionChange


                End If

                If cuenta_ > 1 Or Not botonFormula_.Visible Then

                    If tb_Formula.Enabled Then

                        bc_Verificado.Enabled = True

                        SetVars("_filled", tb_Formula.Text)

                    Else

                        bc_VerificadoNueva.Enabled = True

                        SetVars("_filled", tb_FormulaNueva.Text)

                    End If

                    bi_SudoAutorizar.Visible = True

                    SetVars("_bc_verificado", "SI")

                Else

                    'bc_Verificado.Visible = False
                    bc_Verificado.Enabled = False



                    bc_VerificadoNueva.Enabled = False

                    SetVars("_bc_verificado", "NO")

                End If

            Else

                'bc_Verificado.Visible = False
                bc_Verificado.Enabled = False




                bc_VerificadoNueva.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

            If tb_Formula.Enabled Then

                bi_SolicitarAutorizacion.Visible = bc_Verificado.Enabled

            Else

                bi_SolicitarAutorizacion.Visible = bc_VerificadoNueva.Enabled

            End If


        Else

            DisplayMessage("Falta Especificar la fórmula", TypeStatus.OkBut)

        End If



    End Sub


    Sub IrVerificarFormula()


        'bc_Verificado.Visible = False
        SetVars("_bc_verificado", "NO")

        fscProbarFormulas.Visible = True

        Dim formulaFormato_ As String = tb_Formula.Text.Replace("[13]", vbCrLf)
        Dim formulaNuevaFormato_ As String = tb_FormulaNueva.Text.Replace("[13]", vbCrLf)


        If formulaFormato_ <> "" AndAlso
            (tb_Formula.Enabled OrElse
           (formulaNuevaFormato_ <> "" AndAlso
           tb_FormulaNueva.Enabled)) Then



            If _ctrlInterpreter Is Nothing Then

                _ctrlInterpreter = New MathematicalInterpreterNCalc

                SetVars("_interpreterController", _ctrlInterpreter)

            End If

            If _ctrlCube Is Nothing Then

                _ctrlCube = New CubeController

                _ctrlInterpreter.SetValidFields(_ctrlCube.GetFieldsNamesResource().ObjectReturned)

            End If

            _ctrlInterpreter.addOperands(_ctrlCube.GetOperands().ObjectReturned)

            Dim Values_ = New Dictionary(Of String, Object)

            Dim icontroladorMonedas_ As IControladorMonedas = New ControladorMonedas

            Dim params_ As List(Of String)

            Dim fomula_ = formulaFormato_

            Dim botonformula_ As ButtonControl

            If tb_Formula.Enabled Then

                params_ = _ctrlInterpreter.GetParams(formulaFormato_)

                botonformula_ = bc_Function

            Else

                params_ = _ctrlInterpreter.GetParams(formulaNuevaFormato_)

                fomula_ = formulaNuevaFormato_

                botonformula_ = bc_FunctionChange

            End If

            If _ctrlInterpreter.ExistFields(fomula_, params_) Then


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

                If cuenta_ = 1 AndAlso (tb_Formula.Enabled AndAlso formulaFormato_ <> "" Or formulaNuevaFormato_ <> "") Then

                    VerificarFormula()

                End If

            Else

                ' bc_Verificado.Visible = False
                bc_Verificado.Enabled = False



                bc_VerificadoNueva.Enabled = False

                cc_ValoresOperandos.ClearRows()
                cc_ValoresOperandos.CatalogDataBinding()

            End If

        Else

            DisplayMessage("Falta Especificar la fórmula", TypeStatus.OkBut)




        End If

    End Sub

    Sub EjecutarRoom()


        Dim cubeController_ As ICubeController = GetVars("_cubeController")

        If cubeController_ Is Nothing Then

            cubeController_ = New CubeController

        End If

        Dim params_ As New Dictionary(Of String, Object) From {{"S1.CA_TIPO_OPERACION.0", 1}, {"S10.CA_VINCULACION.0", ""}}

        Dim reports_ = cubeController_.RunRoom(Of Object)("A22.CA_VINCULACION", params_)

        MsgBox(reports_.result)

    End Sub


    Sub LimpiarFormulaCubo()


        If tb_Formula.Enabled Then

            tb_Formula.Text = ""

        Else

            tb_FormulaNueva.Text = ""

        End If



    End Sub


    Sub CargaInicialModulo()

        If GetVars("_cubeSource") <> "" Then
            bc_SourceCube.Label = GetVars("_cubeSource")
        End If
        p_formulillas.CssClass = "col-md-12 col-xs-6"

        p_descriptions.CssClass = "col-md-12 col-xs-6 wc-cubo-description"

        p_formulillas.Visible = True

        p_historico.Visible = False

        p_actualizacionformula.Visible = False

        p_FormulaActual.Enabled = True

        tb_Formula.Enabled = True

        bc_ElaborarPrueba.Enabled = False

        fscProbarFormulas.Visible = False

        bc_LimpiarFormula.Enabled = True

        __SYSTEM_MODULE_FORM.Modality = FormControl.ButtonbarModality.Open



        SetVars("_idRoom", Nothing)


    End Sub

    Sub CargaEncontradolModulo()

        __SYSTEM_MODULE_FORM.Modality = FormControl.ButtonbarModality.Reading

        p_historico.Visible = True

        p_descriptions.CssClass = "col-md-6 col-xs-6 wc-cubo-description"


        p_formulillas.Visible = False


        p_actualizacionformula.CssClass = "col-md-12 col-xs-6"

        p_actualizacionformula.Visible = True

        p_actualizacionformula.Enabled = False

        p_FormulaActual.Enabled = False

        tb_Formula.Enabled = False

        tb_FormulaNueva.Enabled = False

        bc_ElaborarPrueba.Enabled = False

        ic_changeReason.Enabled = False

        bc_LimpiarFormula.Enabled = False

        bi_SolicitarAutorizacion.Visible = True

        bc_SourceCubeChange.Label = GetVars("_cubeSource")

        ColocaHistorial(GetVars("_historical"))


        ' bc_FunctionChange.Enabled = False

        ' ic_RoomNameNew.Enabled = False

    End Sub


    Sub ColocaAutorizar()

        Dim bc_PendienteAutorizacion_ As ButtonControl

        If tb_Formula.Enabled Then

            bc_PendienteAutorizacion_ = bc_PorAutorizar

        Else

            bc_PendienteAutorizacion_ = bc_PorAutorizarNueva

        End If

        bi_SudoAutorizar.Visible = bc_PendienteAutorizacion_.Visible

        bi_SudoDesechar.Visible = bc_PendienteAutorizacion_.Visible

        Dim solicitaAutorizar_ As String = GetVars("_bc_porautorizar")

        If solicitaAutorizar_ = Nothing Then

            bi_SolicitarAutorizacion.Visible = False


            SetVars("_bc_porautorizar", "NO")

        Else

            If solicitaAutorizar_ = "NO" Then

                bi_SolicitarAutorizacion.Visible = False


                SetVars("_bc_porautorizar", "NO")

            Else

                bi_SolicitarAutorizacion.Visible = True


                SetVars("_bc_porautorizar", "SI")

            End If

        End If

        bi_Comparar.Visible = bi_SolicitarAutorizacion.Visible

        bc_SourceCubeChange.Label = GetVars("_cubeSource")

        If GetVars("_userdatas") Then

            ColocaHistorial(GetVars("_historical"))

        End If


    End Sub

    Sub RevisaVerificado()

        Dim showverify_ As String

        showverify_ = GetVars("_bc_verificado")

        If showverify_ = Nothing Then

            'bc_Verificado.Visible = False
            bc_Verificado.Enabled = False



            bc_VerificadoNueva.Enabled = False


            SetVars("_bc_verificado", "NO")

        Else

            If showverify_ = "SI" Then

                SetVars("_bc_verificado", "SI")

            Else

                'bc_Verificado.Visible = Fals
                bc_Verificado.Enabled = False



                bc_VerificadoNueva.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

        End If

    End Sub

    Sub CambioFormula()



        If bc_Verificado.Enabled = False Then

            SetVars("_bc_verificado", "NO")

        Else

            SetVars("_bc_verificado", "SI")


        End If

    End Sub

    Sub ChangeCubeSource()

        If sc_BranchNames.Value <> "7" Then

            bc_SourceCube.Label = sc_BranchNames.Text

            SetVars("_cubeSource", bc_SourceCube.Label)

        Else

        End If



        bc_SourceCube.Visible = True

        sc_BranchNames.Visible = False

    End Sub

    Sub ShowBranchNames()

        bc_SourceCube.Visible = False

        sc_BranchNames.Visible = True
        '  bc_cabulidades.

        ' swc_Online.DataBind()

    End Sub

    Sub ShowUserData()

        If GetVars("_userdatas") Then

            p_userdata.Visible = True

            p_userchange.Visible = True

            p_historico.Visible = True

        Else

            p_userdata.Visible = False

            p_userchange.Visible = False

            p_historico.Visible = False


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