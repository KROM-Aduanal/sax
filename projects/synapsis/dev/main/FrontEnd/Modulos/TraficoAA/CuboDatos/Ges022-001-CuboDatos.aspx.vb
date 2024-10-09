Imports Cube.Validators
Imports gsol.krom
Imports gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Controllers
Imports Rec.Globals.Utils
Imports Sax.Web
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Syn.Utils
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus


Public Class Ges022_001_CuboDatos
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                 Defina en esta región sus atributos o propiedades locales                      ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    'Private _ctrlValidationRoute As IValidationRoute

    Private _ctrlCube As ICubeController

    Private _idRoom As ObjectId

    Private _elementos As List(Of SelectOption)

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

            '.DataObject = New ConstructorAcuseValor With {.FolioDocumento = "HYPERCUBE"}

            Buscador.addFilter(1, 1, "A22/f(x)")
            Buscador.addFilter(2, 2, "A22/(x)")
            Buscador.addFilter(3, 3, "VOCE/(x)")
            Buscador.addFilter(4, 4, "UCAA/(x)")
            Buscador.addFilter(5, 5, "UAA/(x)")
            Buscador.addFilter(6, 6, "UCC/(x)")
            Buscador.addFilter(7, 7, "CDI/(x)")
            Buscador.addFilter(8, 8, "PREV/(x)")
            Buscador.addFilter(9, 9, "Descripción")
            Buscador.addFilter(10, 10, "Por Autorizar")



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

        _organismo = GetVars("_organismo")

        If _organismo Is Nothing Then

            _organismo = New Organismo

        End If

        _ctrlCube = GetVars("_cubeController")

        If _ctrlCube Is Nothing Then

            _ctrlCube = New CubeController

        End If

        If GetVars("_bcComparar") Is Nothing Then

            SetVars("_bcComparar", 1)

        Else

            ChecaBotonComparar()

        End If

        SetVars("_cubeController", _ctrlCube)


        SetVars("_organismo", _organismo)

        _userName = GetVars("_userName")

        _accionDate = GetVars("_accionDate")

        _userImage = "/FrontEnd/Librerias/Krom/imgs/nouser.png"

        _idRoom = GetVars("_idRoom")

        If GetVars("_cubeSource") = "" Then

            SetVars("_cubeSource", "A22")

        End If

        _elementos = GetVars("_fieldmiss")

        If _elementos Is Nothing Then

            _elementos = _organismo.ObtenerSelectOption(_ctrlCube.fieldmiss)

            SetVars("_fieldmiss", _elementos)

        End If

        If IsPostBack Then

            SetVars("_filled", "")

            SetVars("_userdatas", False)

            bc_SourceCube.Label = GetVars("_cubeSource")

            bc_SourceCube.Value = GetVars("_cubeSource")

            bc_SourceCubeChange.Label = GetVars("_cubeSource")

            bc_SourceCubeChange.Value = GetVars("_cubeSource")

        End If

        bc_LimpiarFormula.Enabled = True

        ' fbc_RoomName.ForeColor = Drawing.Color.Aqua

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

        CargaInicialModulo()

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Este método se manda llamar al dar clic en el boton Guardar                             '
        ' Llamamos el método "ProcesarTransaccion" pasando el tipo de nuestra clase constructora  '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bc_comparavisible As ButtonControl

        Dim bc_PendienteAutorizacion_ As ButtonControl

        Dim branchName_ As ICubeController.CubeSlices

        Dim swconline_ As SwitchControl

        Dim bc_function_ As ButtonControl

        Dim roomName_ As String

        Dim useType_ As ICubeController.UseType

        Dim checkUseType_ As Boolean


        If tb_Formula.Enabled Then

            bc_comparavisible = bc_Verificado

            bc_PendienteAutorizacion_ = bc_PorAutorizar

            branchName_ = [Enum].Parse(GetType(ICubeController.CubeSlices), bc_SourceCube.Label, True)

            swconline_ = swc_Online

            bc_function_ = bc_Function

            If bc_Function.Visible Then

                roomName_ = fbc_RoomName.Text

            Else

                roomName_ = ic_RoomName.Value

            End If

            checkUseType_ = [Enum].TryParse(Of ICubeController.UseType)(scUseType.Value, useType_)

            If checkUseType_ Then

                If tb_Formula.Text <> GetVars("_filled") Then

                    bc_Verificado.Enabled = False

                    SetVars("_bc_verificado", "NO")

                End If



            Else

                DisplayMessage("Tipo de usp inválido", TypeStatus.Errors)

            End If


        Else

            bc_comparavisible = bc_VerificadoNueva

            bc_PendienteAutorizacion_ = bc_PorAutorizarNueva

            branchName_ = [Enum].Parse(GetType(ICubeController.CubeSlices), bc_SourceCubeChange.Label, True)

            swconline_ = swc_OnlineNueva

            bc_function_ = bc_FunctionChange

            roomName_ = ic_RoomNameNew.Value

            checkUseType_ = [Enum].TryParse(Of ICubeController.UseType)(scUseTypeNew.Value, useType_)

            If checkUseType_ Then

                bc_Verificado.Enabled = False

                If tb_FormulaNueva.Text <> GetVars("_filled") Then

                    bc_VerificadoNueva.Enabled = False

                    SetVars("_bc_verificado", "NO")

                End If

            Else

                DisplayMessage("Tipo de uso inválido", TypeStatus.OkBut)


            End If


        End If

        If checkUseType_ Then

            If bc_PendienteAutorizacion_.Enabled Then

                DisplayMessage("No se puede actualizar porque está pendiente de autorización", TypeStatus.OkBut)

            Else

                If ic_DescripcionRules.Value = "" Then

                    DisplayMessage("Falta especificar la descripción de la fórmula ó variable", TypeStatus.OkBut)

                Else

                    If bc_comparavisible.Enabled Then

                        Dim status_ As String

                        Dim rulesType_ As ICubeController.ContentTypes

                        If bc_function_.Visible Then

                            rulesType_ = ICubeController.ContentTypes.Formula

                        Else

                            rulesType_ = ICubeController.ContentTypes.Operando

                        End If

                        If swconline_.Checked Then
                            status_ = "on"

                        Else
                            status_ = "off"

                        End If


                        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                        _userName = loginUsuario_("WebServiceUserID")

                        Dim rules_ As String

                        If tb_Formula.Enabled Then

                            rules_ = tb_Formula.Text

                            ic_changeReason.Value = ""

                        Else

                            rules_ = tb_FormulaNueva.Text

                        End If

                        If roomName_ = "" Then

                            DisplayMessage("Falta especificar el nombre de la habitación", TypeStatus.OkBut)

                        Else

                            _ctrlCube = GetVars("_cubeController")

                            If _ctrlCube Is Nothing Then

                                _ctrlCube = New CubeController

                            End If

                            If rulesType_ = ICubeController.ContentTypes.Formula AndAlso _ctrlCube.interpreter.GetValidFields.FindAll(Function(e) e.Contains(roomName_)).Count = 0 Then

                                DisplayMessage("Nombre inválido para una habitación de tipo fórmula", TypeStatus.OkBut)

                            Else

                                If rulesType_ <> ICubeController.ContentTypes.Formula AndAlso _ctrlCube.interpreter.GetValidFields.FindAll(Function(e) e.Contains(roomName_)).Count > 0 Then

                                    DisplayMessage("Has seleccionado variable y ese nombre está reservado para una habitación de tipo fórmula", TypeStatus.OkBut)

                                Else

                                    _idRoom = GetVars("_idRoom")

                                    Dim ejecutar_ = True

                                    If rulesType_ = ICubeController.ContentTypes.Formula And useType_ = ICubeController.UseType.VALIDATION Then

                                        Dim valor_ = GetVars("resultTest_")

                                        If TypeOf valor_ Is Dictionary(Of String, String) OrElse TypeOf valor_ Is List(Of String) Then

                                            ejecutar_ = False

                                            DisplayMessage("Las reglas de tipo fórmula sólo pueden devolver OK o BAD", TypeStatus.OkBut)

                                        Else

                                            If GetVars("resultTest_").ToString = "OK" OrElse GetVars("resultTest_").ToString = "BAD" Then

                                            Else

                                                ejecutar_ = False

                                                DisplayMessage("Las reglas de tipo fórmula sólo pueden devolver OK o BAD", TypeStatus.OkBut)

                                            End If

                                        End If

                                    End If

                                    If ejecutar_ Then

                                        Dim tagwatcher_ = _ctrlCube.RunTransaction(_idRoom,
                                                                                      roomName_,
                                                                                      rules_,
                                                                                      branchName_,
                                                                                      rulesType_,
                                                                                      ic_DescripcionRules.Value,
                                                                                      status_,
                                                                                      useType_,
                                                                                      CrearListaMensages,
                                                                                      Nothing,
                                                                                      userName_:=loginUsuario_("WebServiceUserID"),
                                                                                      reason_:=ic_changeReason.Value)

                                        If tagwatcher_.ObjectReturned IsNot Nothing Then

                                            Dim room_ As room = tagwatcher_.ObjectReturned

                                            ColocaHistorial(room_.historical)

                                            DisplayMessage("Regla asignada satisfactoriamente", Ok)

                                            SetVars("_userName", _userName)

                                            SetVars("_accionDate", _accionDate)

                                            If tb_Formula.Enabled = True Then

                                                _ctrlCube = New CubeController

                                                SetVars("_cubeController", _ctrlCube)

                                            End If

                                        Else

                                            DisplayMessage("Ya existe una racámara con nombre " & roomName_ & " en " & branchName_, TypeStatus.OkBut)

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    Else

                        DisplayMessage("Esta regla no ha sido verificada", TypeStatus.OkBut)

                    End If

                End If



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

        p_actualizacionformula.Enabled = True

        ic_changeReason.Enabled = True

        bc_SourceCube.Label = GetVars("_cubeSource")

        bc_SourceCubeChange.Label = GetVars("_cubeSource")

        ic_RoomNameNew.Enabled = True



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

        Dim contentType_ As ICubeController.ContentTypes

        Dim useType_ As ICubeController.UseType



        'MsgBox(IndexSelected_)


        If tb_Formula.Enabled Then

            If tb_Formula.Text <> GetVars("_filled") Then

                bc_Verificado.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

            bc_comparavisible = bc_Verificado

            bc_PendienteAutorizacion_ = bc_PorAutorizar

            bc_ContenType_ = bc_Function

            swc_online_ = swc_Online

            If Not [Enum].TryParse(Of ICubeController.UseType)(scUseType.Value, useType_) Then

                useType_ = ICubeController.UseType.Undefined

            End If



        Else

            If Not [Enum].TryParse(Of ICubeController.UseType)(scUseTypeNew.Value, useType_) Then

                useType_ = ICubeController.UseType.Undefined

            End If

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

                If ic_changeReason.Value = "" Then

                    DisplayMessage("Falta Especificar la razón de la autorización", TypeStatus.OkBut)

                Else

                    If bc_PendienteAutorizacion_.Enabled Then

                        DisplayMessage("Esta fórmula ya está pendiente de autorización", TypeStatus.OkBut)

                    Else

                        If bc_comparavisible.Enabled Then

                            Dim status_ As String

                            If bc_ContenType_.Visible Then

                                contentType_ = ICubeController.ContentTypes.Formula

                            Else

                                contentType_ = ICubeController.ContentTypes.Operando

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

                            Dim ejecutar_ = True

                            If contentType_ = ICubeController.ContentTypes.Formula And useType_ = ICubeController.UseType.VALIDATION Then

                                Dim valor_ = GetVars("resultTest_")

                                If TypeOf valor_ Is Dictionary(Of String, String) OrElse TypeOf valor_ Is List(Of String) Then

                                    ejecutar_ = False

                                    DisplayMessage("Las reglas de tipo fórmula sólo pueden devolver OK o BAD", TypeStatus.Errors)

                                Else

                                    If GetVars("resultTest_").ToString = "OK" OrElse GetVars("resultTest_").ToString = "BAD" Then

                                    Else

                                        ejecutar_ = False

                                        DisplayMessage("Las reglas de tipo fórmula sólo pueden devolver OK o BAD", TypeStatus.OkBut)

                                    End If

                                End If

                            End If


                            If ejecutar_ Then

                                Dim tagwatcher_ = _ctrlCube.RunTransaction(_idRoom,
                                                                                  roomName_,
                                                                                  rules_.Replace("[13]",
                                                                                                 vbCrLf),
                                                                                  [Enum].Parse(GetType(ICubeController.CubeSlices), GetVars("_cubeSource"), True),
                                                                                  contentType_,
                                                                                  ic_DescripcionRules.Value,
                                                                                  status_,
                                                                                  useType_,
                                                                                  CrearListaMensages,
                                                                                  Nothing,
                                                                                  userName_:=loginUsuario_("WebServiceUserID"),
                                                                                  enviado_:="sent",
                                                                                  reason_:=ic_changeReason.Value)

                                If tagwatcher_.Status = Ok Then

                                    Dim room_ As room = tagwatcher_.ObjectReturned

                                    ColocaHistorial(room_.historical)

                                    DisplayMessage("Su solicitud ha sido enviada", TypeStatus.OkInfo)

                                    SetVars("_userName", _userName)

                                    SetVars("_accionDate", _accionDate)

                                    bc_PendienteAutorizacion_.Enabled = True

                                    bi_SolicitarAutorizacion.Visible = False

                                End If

                            End If

                        Else

                            DisplayMessage("Esta regla no ha sido verificada", TypeStatus.OkBut)

                        End If

                    End If

                End If

            Case 8  'Esta Opción limpia todo el formulario

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

                    Dim status_ As String

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

                        contentType_ = ICubeController.ContentTypes.Formula

                    Else

                        contentType_ = ICubeController.ContentTypes.Operando

                    End If


                    _idRoom = GetVars("_idRoom")

                    Dim ejecutar_ = True

                    If contentType_ = ICubeController.ContentTypes.Formula And useType_ = ICubeController.UseType.VALIDATION Then

                        Dim valor_ = GetVars("resultTest_")

                        If TypeOf valor_ Is Dictionary(Of String, String) OrElse TypeOf valor_ Is List(Of String) Then

                            ejecutar_ = False

                            DisplayMessage("Las reglas de tipo fórmula sólo pueden devolver OK o BAD", TypeStatus.OkBut)

                        Else

                            If GetVars("resultTest_").ToString = "OK" OrElse GetVars("resultTest_").ToString = "BAD" Then

                            Else

                                ejecutar_ = False

                                DisplayMessage("Las reglas de tipo fórmula sólo pueden devolver OK o BAD", TypeStatus.OkBut)
                            End If

                        End If

                    End If

                    If ejecutar_ Then

                        Dim tagwatcher_ = _ctrlCube.RunTransaction(_idRoom,
                                                                          roomName_,
                                                                          rules_.Replace("[13]",
                                                                                         vbCrLf),
                                                                          [Enum].Parse(GetType(ICubeController.CubeSlices), GetVars("_cubeSource"), True),
                                                                          contentType_,
                                                                          ic_DescripcionRules.Value,
                                                                          status_,
                                                                          useType_,
                                                                          CrearListaMensages,
                                                                          Nothing,
                                                                          userName_:=loginUsuario_("WebServiceUserID"),
                                                                          enviado_:="on",
                                                                          reason_:=ic_changeReason.Value)

                        If tagwatcher_.Status = Ok Then

                            Dim room_ As room = tagwatcher_.ObjectReturned

                            ColocaHistorial(room_.historical)

                            DisplayMessage("El cambio ha sido autorizado", TypeStatus.Ok)

                            SetVars("_userName", _userName)

                            SetVars("_accionDate", _accionDate)

                            _ctrlCube = New CubeController

                            SetVars("_cubeController", _ctrlCube)

                        End If

                    End If

                Else

                    DisplayMessage("Debes Verificar la fórmula antes de autorizarla", TypeStatus.OkBut)

                End If

            Case 13 'Esta opción es para mandar a desechar una fórmula

                If bc_comparavisible.Enabled Then

                    Dim status_ As String

                    If bc_Function.Visible Then

                        contentType_ = ICubeController.ContentTypes.Formula

                    Else

                        contentType_ = ICubeController.ContentTypes.Operando

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

                    If GetVars("_cubeSource") = "PREV" Then

                        useType_ = ICubeController.UseType.ASSISTANCE

                    Else

                        useType_ = ICubeController.UseType.MOTOR

                    End If

                    Dim tagwatcher_ = _ctrlCube.RunTransaction(_idRoom,
                                                                      roomName_,
                                                                      rules_,
                                                                      [Enum].Parse(GetType(ICubeController.CubeSlices), GetVars("_cubeSource"), True),
                                                                      contentType_,
                                                                      ic_DescripcionRules.Value,
                                                                      status_,
                                                                      useType_,
                                                                      CrearListaMensages,
                                                                      Nothing,
                                                                      userName_:=loginUsuario_("WebServiceUserID"),
                                                                      enviado_:="off",
                                                                      reason_:=ic_changeReason.Value)

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

                SetVars("_bcComparar", 3)

                p_formulillas.Visible = True

                p_formulillas.CssClass = "col-md-6 col-xs-6"

                bc_ElaborarPrueba.Visible = False

                bc_LimpiarFormula.Visible = False

                p_actualizacionformula.CssClass = "col-md-6 col-xs-6"

                ChecaBotonComparar()

                PreparaBotonera(FormControl.ButtonbarModality.Open)

                ic_RoomName.Enabled = False

                p_userchange.Visible = False

                bc_Verificado.Visible = False

                bc_PorAutorizar.Visible = False

                bc_VerificadoNueva.Visible = True

                bc_PorAutorizarNueva.Visible = True

                l_RulesNew.Text = "Regla Nueva"

                l_RulesOld.Text = "Regla Vigente"

            Case 15 'esta opción quita las dos fórmulas que se mostraban dejando sólo una

                SetVars("_bcComparar", 2)

                p_formulillas.Visible = False

                p_formulillas.CssClass = "col-md-12 col-xs-12"

                bc_ElaborarPrueba.Visible = False

                bc_LimpiarFormula.Visible = False

                p_actualizacionformula.CssClass = "col-md-12 col-xs-12"

                ic_RoomName.Enabled = False

                p_userchange.Visible = True

                bc_Verificado.Visible = False
                bc_PorAutorizar.Visible = False

                l_RulesNew.Text = "Regla"

                l_RulesOld.Text = "Regla"

                ChecaBotonComparar()

                PreparaBotonera(FormControl.ButtonbarModality.Open)


            Case 16 ' Esta Opción era para leer el CSV


                '_ctrlCube.CamposExcelMongo("C:\ZERG\SYNAPSIS\CUBO\ROOMSELIMINAR\ROOMSMOTORELIMINAR.csv")
                'RunRoom(Of T)(roomname_ As String, params_ As Dictionary(Of String, T))

                'Dim diccionary_ As Dictionary(Of String, Object) = Nothing
                'Dim algo_ = _ctrlCube.RunRoom(Of Object)("A22.CAT_INCOTERM", diccionary_)

                'Dim algo_2 = 4

                'Dim algodon_ As IMathematicalInterpreter = New MathematicalInterpreterNCalc

                'Dim algo_ = algodon_.RunExpression(Of Object)("DICCIONARIO(ENLISTAR('1 | ADIDAS','2 | NIKE', '3 | CHITLY'),'|')", Nothing)

                'If algo_ Is Nothing Then

                'End If


                'seccion_ = pedimento_.Seccion(SeccionesPedimento.ANS18)

                'Dim index_ = 0

                'For Each Nodo_ In seccion_.Nodos

                '    sections_ = New List(Of SeccionesPedimento) From {SeccionesPedimento.ANS18}

                '    fields_ = New List(Of CamposPedimento) From {CamposPedimento.CA_CVE_IDENTIFICADOR}

                '    ValidateField(sections_,
                '              fields_,
                '              elementMessage_,
                '              Nodo_,
                '              message_,
                '              useNoticed_,
                '              "",
                '              False,
                '              validation_,
                '              index_,)

                Dim dictionary_ As New Dictionary(Of String, Object) From {{"CABULIDAD", 5}, {"RECABULIDAD", 9}}
                Dim report3_ = _ctrlCube.RunRoom(Of Object)("A22.VARIABLEDEPRUEBA", dictionary_)

                Dim diccionarioCubo_ As Dictionary(Of String, String) '= report_.ObjectReturned

                dictionary_ = New Dictionary(Of String, Object) From {{"CA_TIPO_OPERACION.0", "1"}, {"CP_TIPO_PEDIMENTO.0", "GLOBAL COMPLEMENTARIO"}}
                'Dim report_ = _ctrlCube.RunRoom(Of Object)("A22.AS_PED2", dictionary_)

                'Dim diccionarioCubo_ As Dictionary(Of String, String) = _ctrlCube.status.ObjectReturned

                'MsgBox(DirectCast(fsc_Alertas.ListControls(0), InputControl).Value)

                'Dim dictionary_ As New Dictionary(Of String, Object) From {{"NumeroContenedor.0", "8"}}
                'Dim report_ = _ctrlCube.RunRoom(Of Object)("SI(NumeroContenedor=8,500,300)", dictionary_)

                'Dim diccionarioCubo_ = _ctrlCube.status.ObjectReturned

                'Dim diccionarioNuevo_ As New Dictionary(Of String, List(Of String))

                'For Each elemento_ In diccionarioCubo_.Keys

                '    diccionarioNuevo_.Add(elemento_, diccionarioCubo_(elemento_).Split(",").ToList)


                ' Next

                Dim report_ = _ctrlCube.
                          RunAssistance(Of Object)("PREV.AS_PED2",
                                             dictionary_)

                diccionarioCubo_ = report_.
                                   ObjectReturned

                Dim reportcube_ = _ctrlCube.
                               GetReports()


                'ASI SE USA EL DISPOSE CUANDO ES INTERFACE

                'CType(_ctrlCube, CubeController).Dispose()



            Case 17 ' Esta opción era para probar la ruta de validación

                'Dim pedimento_ As New DocumentoElectronico

                'Dim folioOperacion_ = tb_Formula.Text

                'If folioOperacion_ = "" Then

                '    folioOperacion_ = "RKU24-00000301"

                'End If

                'Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(13)

                '    Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.
                '                                                                  GetMongoCollection(Of OperacionGenerica) _
                '                                                                  (GetType(ConstructorPedimentoNormal).Name)

                '    operationsDB_.Aggregate().
                '                  Match(Function(ch) ch.
                '                                     Borrador.
                '                                     Folder.
                '                                     ArchivoPrincipal.
                '                                     Dupla.
                '                                     Fuente.
                '                                     FolioOperacion.
                '                                     Equals(folioOperacion_)).
                '                  ToList().
                '                  ForEach(Sub(items)

                '                              pedimento_ = items.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                '                          End Sub)

                'End Using

                '_ctrlValidationRoute = New ValidationRouteAll

                'Dim validationType_ As IValidationRoute.ValidationRoutes

                'If pedimento_.Seccion(SeccionesPedimento.ANS1).Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor = "1" Then

                '    validationType_ = IValidationRoute.ValidationRoutes.RUVA1

                'Else

                '    validationType_ = IValidationRoute.ValidationRoutes.RUVA4

                'End If

                'Dim resultado_ = _ctrlValidationRoute.ValidateLinear(pedimento_).validateroute(validationType_)

                'Dim ListControls_ As IEnumerable(Of Object) = ccErrores.ListControls

                'resultado_.ShowMessageError(ListControls_.ToList)

                'If resultado_.details IsNot Nothing Then

                '    ccErrores.Visible = True

                'End If

            Case 18  ' Esta opción nos dirá en que secciones se usa un campo del pedimento

                If tb_Formula.Text <> "" Then

                    DisplayMessage(_ctrlCube.
                                   GetSectionsResource(tb_Formula.Text).
                                   ObjectReturned,
                                   StatusMessage.Info)

                End If

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

        bc_SourceCube.Value = "A22"

        scUseType.Value = 1

        sc_BranchNames.Value = 1

        bc_Function.Visible = True

        bc_Function.Enabled = False

        bc_Var.Enabled = False

        tb_Formula.Enabled = True

        bc_Var.Visible = False

        bc_SourceCube.Enabled = False

        ic_DescripcionRules.Value = ""

        ic_changeReason.Value = ""

        cc_ValoresOperandos.ClearRows()

        cc_ValoresOperandos.CatalogDataBinding()

        swc_Online.Checked = False

        bc_Verificado.Visible = True

        bc_PorAutorizar.Visible = True

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

        fbc_RoomName.Enabled = False

        SetVars("_userdatas", False)

        ShowUserData()

        SetVars("_bc_porautorizar", "SI")

        fbc_RoomName.Label = "Escriba quí el nombre de la habitación"

        fbc_RoomName.Text = ""

        l_RulesNew.Text = "Regla"

        l_RulesOld.Text = "Regla"

        p_formulillas.CssClass = "col-md-12 col-xs-6"

        p_descriptions.CssClass = "col-md-12 col-xs-6 wc-cubo-description"

        p_formulillas.Visible = True

        p_actualizacionformula.Visible = False

        tb_FormulaNueva.Text = ""

        If _ctrlCube Is Nothing Then

            _ctrlCube = New CubeController

        End If

        DirectCast(fsc_Alertas.ListControls(0), InputControl).Value = ""

        DirectCast(fsc_Advertencias.ListControls(0), InputControl).Value = ""

        DirectCast(fsc_Información.ListControls(0), InputControl).Value = ""

        _ctrlCube = New CubeController '.GetFieldsNamesResource()

        SetVars("_cubeController", _ctrlCube)

        SetVars("_cubeSource", "A22")

        PreparaBotonera(FormControl.ButtonbarModality.Default)

    End Sub

    Public Sub BuscarGajo()

        Dim filtering_ As New List(Of String)

        Dim searchFilter_ As String

        Dim joinBranchName_ As String = "{$or:["

        Dim indexFilter_ As Int16 = 0

        Dim checkAutorizar_ As Boolean = False

        Dim checkDescription_ As Boolean = False

        For Each checkbox_ In Buscador.CheckBoxs

            If checkbox_.Checked Then

                Dim cubeSlicesCheked_ As String = Buscador.Filters(indexFilter_).Text

                Dim contentType_ As String

                Dim diagonalPosition_ = cubeSlicesCheked_.IndexOf("/")

                If diagonalPosition_ = -1 Then

                    contentType_ = ""

                    If Buscador.Filters(indexFilter_).Text.ToUpper = "POR AUTORIZAR" Then

                        checkAutorizar_ = True

                    Else

                        checkDescription_ = True

                    End If

                Else

                    contentType_ = cubeSlicesCheked_.Substring(diagonalPosition_ + 1)

                    If contentType_ = "f(x)" Then

                        contentType_ = "'formula'"

                    Else

                        contentType_ = "'operando'"

                    End If

                    cubeSlicesCheked_ = cubeSlicesCheked_.Substring(0, diagonalPosition_)

                    joinBranchName_ &= "{$and:[{branchname:'" &
                                        cubeSlicesCheked_.
                                        Replace(" ", "") &
                                        "'},{contenttype:" &
                                        contentType_ &
                                        "}]},"

                End If

            End If

            indexFilter_ += 1

        Next

        Dim roomResourceDictionary_ As New Dictionary(Of ObjectId, roomresource)

        Dim buscador_ As String = Buscador.Text

        If Buscador.Text.Contains("|") Then

            Dim indexInitial_ = buscador_.IndexOf("|")

            Dim indexFinal_ = buscador_.LastIndexOf("|")

            buscador_ = buscador_.Substring(0, indexInitial_)

        End If

        Dim words_ = buscador_.Split(" ")

        Dim word_ = words_.TakeWhile(Function(ch) ch.Contains("@"))

        Dim email_ As New List(Of String)

        If word_.Count > 0 Then

            email_.Add("username")

            buscador_ = buscador_.Replace(word_(0), "")

            email_.Add("'" & word_(0) & "'")

        Else

            email_.Add("")

            email_.Add("")

        End If

        Dim TagWatcher_ As TagWatcher

        Dim key_ As String

        Dim keys_ As New List(Of String) From {
                                                "()A22/f(x)",
                                                "()A22/(x)",
                                                "()VOCE/(x)",
                                                "()UCAA/(x)",
                                                "()UAA/(x)",
                                                "()UCC/(x)",
                                                "()CDI/(x)",
                                                "()PREV/(x)"
                                              }

        Dim dictionary_ As New Dictionary(Of Object, Object)

        searchFilter_ = _organismo.SeparacionPalabras(buscador_, "valorpresentacion", email_(0), email_(1), "normal")

        If joinBranchName_ <> "{$or:[" Then

            joinBranchName_ = joinBranchName_.Substring(0, joinBranchName_.Length - 1) & "]}"

            TagWatcher_ = _ctrlCube.GetRoomNamesResource(searchFilter_.Substring(0, searchFilter_.Length - 2) & "," & joinBranchName_ & "]}", ICubeController.TypeSearch.Free)

            Dim roomResourceList_ As List(Of roomresource) = TagWatcher_.ObjectReturned

            For Each keyFor_ In keys_

                Dim branchname_ = keyFor_.Substring(2, keyFor_.IndexOf("/") - 2)

                Dim contentType_ As String = If(keyFor_.Contains("f(x)"), "formula", "operando")

                Dim roomResourceListForEach_ = roomResourceList_.FindAll(Function(ch) ch.branchname = branchname_ And
                                                                      ch.contenttype = contentType_)

                key_ = keyFor_.Replace("()", "(" & roomResourceListForEach_.Count & ")")

                dictionary_.Add(key_, New List(Of Dictionary(Of Object, Object)))

                For Each roomResource_ As roomresource In roomResourceListForEach_

                    Dim valorPresentacion_ As String = roomResource_.valorpresentacion

                    For Each wordBuscador_ In buscador_.Split(" ")

                        valorPresentacion_ = Replace(valorPresentacion_, wordBuscador_.ToUpper, "<span>" & wordBuscador_.ToUpper & "</span>")

                    Next

                    If email_(0) = "" Then

                        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                   {"Text", valorPresentacion_ &
                                                                                            " | " &
                                                                                            roomResource_.usetype &
                                                                                            " | " &
                                                                                            If(roomResource_.awaitingapproval, "PENDIENTE", "AUTORIZADO")}})
                    Else

                        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                   {"Text", valorPresentacion_ & " | " & roomResource_.username}})
                    End If

                    roomResourceDictionary_(roomResource_._id) = roomResource_

                Next

                If dictionary_(key_).Count = 0 Then

                    dictionary_.Remove(key_)

                End If


            Next


            'key_ = "(" & TagWatcher_.ObjectReturned.count & ")A22/(x)"

            'dictionary_.Add(key_, New List(Of Dictionary(Of Object, Object)))

            'For Each roomResource_ As roomresource In roomResourceList_.FindAll(Function(ch) ch.branchname = "A22" And
            '                                                           ch.contenttype = "formula")
            '    Dim valorPresentacion_ As String = roomResource_.valorpresentacion

            '    For Each wordBuscador_ In buscador_.Split(" ")

            '        valorPresentacion_ = Replace(valorPresentacion_, wordBuscador_.ToUpper, "<span>" & wordBuscador_.ToUpper & "</span>")

            '    Next

            '    If email_(0) = "" Then

            '        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
            '                                                                       {"Text", roomResource_.branchname & " | " & valorPresentacion_}})
            '    Else

            '        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
            '                                                                       {"Text", roomResource_.branchname & " | " & valorPresentacion_ & " | " & roomResource_.username}})
            '    End If

            '    roomResourceDictionary_(roomResource_._id) = roomResource_

            'Next

            'If dictionary_(key_).Count = 0 Then

            '    dictionary_.Remove(key_)

            'End If


            'If TagWatcher_.ObjectReturned.Count > 0 Then

            '        dictionary_.Add(key_, New List(Of Dictionary(Of Object, Object)))

            '        For Each roomResource_ As roomresource In TagWatcher_.ObjectReturned

            '            Dim valorPresentacion_ As String = roomResource_.valorpresentacion

            '            For Each wordBuscador_ In buscador_.Split(" ")

            '                valorPresentacion_ = Replace(valorPresentacion_, wordBuscador_.ToUpper, "<span>" & wordBuscador_.ToUpper & "</span>")

            '            Next

            '            If email_(0) = "" Then

            '                dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
            '                                                                           {"Text", roomResource_.branchname & " | " & valorPresentacion_}})
            '            Else

            '                dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
            '                                                                           {"Text", roomResource_.branchname & " | " & valorPresentacion_ & " | " & roomResource_.username}})
            '            End If

            '            roomResourceDictionary_(roomResource_._id) = roomResource_

            '        Next

            '    End If

        End If

        If checkAutorizar_ Then

            TagWatcher_ = _ctrlCube.GetRoomNamesResource(True, searchFilter_, 2)

            If TagWatcher_.ObjectReturned.count > 0 Then

                key_ = "(" & TagWatcher_.ObjectReturned.count & ")Pendientes de Revisión"

                dictionary_.Add(key_, New List(Of Dictionary(Of Object, Object)))

                For Each roomResource_ As roomresource In TagWatcher_.ObjectReturned

                    Dim valorPresentacion_ As String = roomResource_.valorpresentacion

                    For Each wordBuscador_ In buscador_.Split(" ")

                        valorPresentacion_ = Replace(valorPresentacion_, wordBuscador_.ToUpper, "<span>" & wordBuscador_.ToUpper & "</span>")

                    Next

                    If email_(0) = "" Then

                        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                       {"Text", valorPresentacion_ &
                                                                                                " | " &
                                                                                                roomResource_.usetype &
                                                                                                " | " &
                                                                                                roomResource_.branchname}})
                    Else

                        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                       {"Text", roomResource_.branchname & " | " & valorPresentacion_ & " | " & roomResource_.username}})
                    End If
                    roomResourceDictionary_(roomResource_._id) = roomResource_

                Next

            End If

        End If

        If checkDescription_ Then

            TagWatcher_ = _ctrlCube.GetRoomNamesResource(_organismo.SeparacionPalabras(buscador_, "description", email_(0), email_(1), "normal"), ICubeController.TypeSearch.Free)

            If TagWatcher_.ObjectReturned.Count > 0 Then

                key_ = "(" & TagWatcher_.ObjectReturned.count & ")Descripción"

                dictionary_.Add(key_, New List(Of Dictionary(Of Object, Object)))

                For Each roomResource_ As roomresource In TagWatcher_.ObjectReturned

                    Dim valorPresentacion_ As String = roomResource_.valorpresentacion

                    For Each wordBuscador_ In buscador_.Split(" ")

                        valorPresentacion_ = Replace(valorPresentacion_, wordBuscador_.ToUpper, "<span>" & wordBuscador_.ToUpper & "</span>")

                    Next

                    If email_(0) = "" Then

                        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                       {"Text", valorPresentacion_ &
                                                                                                " | " &
                                                                                                roomResource_.usetype &
                                                                                                " | " &
                                                                                                roomResource_.branchname}})
                    Else

                        dictionary_(key_).add(New Dictionary(Of Object, Object) From {{"Value", roomResource_._id.ToString},
                                                                                       {"Text", roomResource_.branchname & " | " & valorPresentacion_ & " | " & roomResource_.username}})
                    End If
                Next

            End If

        End If

        If Buscador.Value = "" Then

            SetVars("_roomdictionary_", roomResourceDictionary_)

        Else

            Dim _idRoomResource_ = ObjectId.Parse(Buscador.Value)

            Dim roomResourceSelected_ = New Dictionary(Of ObjectId, roomresource) From {{_idRoomResource_, roomResourceDictionary_(_idRoomResource_)}}

            roomResourceDictionary_ = roomResourceSelected_

            SetVars("_roomdictionary_", roomResourceDictionary_)

        End If

        Buscador.DataSource = dictionary_

        Buscador.DataBind()

    End Sub

    Public Sub CargarGajo(sender_ As Object, e As EventArgs)

        Dim roomsResource_ As Dictionary(Of ObjectId, roomresource) = GetVars("_roomdictionary_")

        Dim messages_ As List(Of String)

        bc_Verificado.Enabled = False

        bc_PorAutorizar.Enabled = False

        bc_VerificadoNueva.Enabled = False

        bc_PorAutorizarNueva.Enabled = False

        l_RulesNew.Text = "Regla"

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

                currentUser_ = _organismo.DateDiffUX(rooms_(0)._id.CreationTime, DateTime.Now)

                _userName = ""

            Else

                If rooms_(0).historical.Count = 0 Then

                    currentUser_ = _organismo.DateDiffUX(rooms_(0)._id.CreationTime, DateTime.Now)

                    _userName = ""

                Else

                    ColocaHistorial(rooms_(0).historical)

                    SetVars("_userdatas", True)

                    ShowUserData()


                End If


            End If

            If rooms_(0).usetype Is Nothing Then

                scUseTypeNew.Value = 1

            Else

                scUseTypeNew.Value = [Enum].Parse(GetType(ICubeController.UseType), rooms_(0).usetype, True)

            End If

            SetVars("_historical", rooms_(0).historical)

            SetVars("_userName", _userName)

            SetVars("_accionDate", _accionDate)

            If rooms_(0).contenttype = "formula" Then

                bc_Var.Visible = False

                bc_VarChange.Visible = False

                bc_Function.Visible = True

                bc_FunctionChange.Visible = True

                fbc_RoomName.Visible = True

                ic_RoomName.Visible = False

                fbc_RoomName.Text = roomResource_.valorpresentacion

            Else

                bc_Function.Visible = False

                bc_FunctionChange.Visible = False

                bc_Var.Visible = True

                bc_VarChange.Visible = True

                fbc_RoomName.Visible = False

                ic_RoomName.Visible = True

                ic_RoomName.Value = roomResource_.valorpresentacion

            End If

            bi_SolicitarAutorizacion.Visible = True

            ic_RoomNameNew.Value = roomResource_.valorpresentacion

            tb_FormulaNueva.Text = rooms_(0).rules

            bc_SourceCubeChange.Label = roomResource_.branchname

            messages_ = rooms_(0).messages

            bi_SudoAutorizar.Visible = False

            ic_DescripcionRules.Value = rooms_(0).description

            If rooms_(0).awaitingupdates IsNot Nothing Then

                If rooms_(0).awaitingupdates.Count > 0 Then

                    If rooms_(0).awaitingupdates(0).status = "on" Or rooms_(0).awaitingupdates(0).status = "off" Then

                        ic_RoomNameNew.Value = rooms_(0).roomname.Substring(rooms_(0).roomname.IndexOf(".") + 1)

                        tb_FormulaNueva.Text = rooms_(0).rules.Replace(vbCrLf, "[13]")

                        ic_changeReason.Value = ""

                        bc_SourceCubeChange.Label = roomResource_.branchname

                        messages_ = rooms_(0).messages

                        bc_PorAutorizar.Enabled = False

                        bc_PorAutorizarNueva.Enabled = False



                    Else

                        Dim roomNameNew_ As String = rooms_(0).awaitingupdates(0).roomname

                        ic_RoomNameNew.Value = roomNameNew_.Substring(roomNameNew_.IndexOf(".") + 1)

                        tb_FormulaNueva.Text = rooms_(0).awaitingupdates(0).rules.Replace(vbCrLf, "[13]")

                        ic_changeReason.Value = rooms_(0).awaitingupdates(0).reason

                        branchname_ = roomNameNew_.Substring(0, roomNameNew_.IndexOf("."))

                        messages_ = rooms_(0).awaitingupdates(0).messages

                        If rooms_(0).awaitingupdates(0).usetype Is Nothing Then

                            scUseTypeNew.Value = 1

                        Else

                            scUseTypeNew.Value = [Enum].Parse(GetType(ICubeController.UseType), rooms_(0).usetype, True)

                        End If

                        bc_SourceCubeChange.Label = branchname_

                        If rooms_(0).awaitingupdates(0).contenttype = "formula" Then

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

                        ic_DescripcionRules.Value = rooms_(0).awaitingupdates(0).description

                        If rooms_(0).awaitingupdates(0).status = "sent" Then

                            bc_PorAutorizar.Enabled = False

                            bc_PorAutorizarNueva.Enabled = True

                            bi_SudoAutorizar.Visible = True

                            bi_SolicitarAutorizacion.Visible = False

                            bi_SudoDesechar.Visible = True

                            bc_Function.Enabled = False

                            bc_FunctionChange.Enabled = False

                            bc_Var.Enabled = False

                            bc_VarChange.Enabled = False

                        Else

                            l_RulesNew.Text = "Regla Borrador"

                        End If

                    End If

                End If

            End If

            tb_Formula.Text = rooms_(0).rules

            If rooms_(0).status = "on" Then

                swc_Online.Checked = True

            Else

                swc_Online.Checked = False

            End If

            SetVars("_bcComparar", 2)

            ChecaBotonComparar()

            SetVars("_cubeSource", branchname_)

            CargaEncontradolModulo()

            SetVars("_bc_porautorizar", "SI")

            If messages_.Count = 0 Then

                DirectCast(fsc_Alertas.ListControls(0), InputControl).Value = ""

                DirectCast(fsc_Advertencias.ListControls(0), InputControl).Value = ""

                DirectCast(fsc_Información.ListControls(0), InputControl).Value = ""

            Else

                DirectCast(fsc_Alertas.ListControls(0), InputControl).Value = messages_(0)

                DirectCast(fsc_Advertencias.ListControls(0), InputControl).Value = messages_(1)

                DirectCast(fsc_Información.ListControls(0), InputControl).Value = messages_(2)

            End If

            bc_Verificado.Enabled = False

            p_userdata.Visible = False

            p_userchange.Visible = True

            bc_VerificadoNueva.Enabled = False

            SetVars("_bc_verificado", "NO")

        End If

    End Sub

    Sub NewChangeContent()

        Dim cubeSlice_ = bc_SourceCubeChange.Value

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

        bc_SourceCubeChange.Label = cubeSlice_

        SetVars("_cubeSource", cubeSlice_)

    End Sub
    Sub CambioContenido()

        Dim cubeSlice_ = bc_SourceCube.Value

        If cubeSlice_ = "" Then

            cubeSlice_ = GetVars("_cubeSource")

        End If

        If bc_Function.Visible Then

            bc_Function.Visible = False

            bc_Var.Visible = True

            fbc_RoomName.Visible = False

            ic_RoomName.Visible = True

            ic_RoomName.Enabled = True

            ic_RoomName.Value = ""

        Else

            bc_Var.Visible = False

            bc_Function.Visible = True

            fbc_RoomName.Visible = True

            ic_RoomName.Visible = False

            fbc_RoomName.Enabled = True

        End If


        If GetVars("_userName") IsNot Nothing Then

            _userName = GetVars("_userName")

            _accionDate = GetVars("_accionDate")

        End If

        bc_SourceCube.Label = cubeSlice_

        SetVars("_cubeSource", cubeSlice_)

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

                        _accionDate = _organismo.DateDiffUX(roomhstory_.createat, DateTime.Now)

                        _userName = roomhstory_.username

                        _accionText = roomhstory_.reason

                    Case 2

                        _accionDate2 = _organismo.DateDiffUX(roomhstory_.createat, DateTime.Now)

                        _userName2 = roomhstory_.username

                        _accionText2 = roomhstory_.reason

                    Case 3

                        _userName3 = roomhstory_.username

                        _accionDate3 = _organismo.DateDiffUX(roomhstory_.createat, DateTime.Now)

                        _accionText3 = roomhstory_.reason

                    Case Else

                        Exit For

                End Select

                cuenta_ += 1

            Next

        End If

    End Sub

    Sub VerificarFormula()

        Dim tipoRegla_ As Boolean

        If p_actualizacionformula.Visible Then

            tipoRegla_ = bc_FunctionChange.Visible

        Else

            tipoRegla_ = bc_Function.Visible

        End If

        Dim formulaFormato_ As String = tb_Formula.Text.Replace("[13]", vbCrLf)

        Dim formulaNuevaFormato_ As String = tb_FormulaNueva.Text.Replace("[13]", vbCrLf)

        If formulaFormato_ <> "" AndAlso
            (tb_Formula.Enabled OrElse
           (formulaNuevaFormato_ <> "" AndAlso
           tb_FormulaNueva.Enabled)) Then

            Dim formula_ As String

            _ctrlCube = GetVars("_cubeController")

            If _ctrlCube Is Nothing Then

                _ctrlCube = New CubeController

            End If

            Dim Values_ = New Dictionary(Of String, Object)

            Dim params_ As List(Of String)

            If tb_Formula.Enabled Then

                formula_ = formulaFormato_.Replace(vbCrLf, "")

            Else

                formula_ = formulaNuevaFormato_.Replace(vbCrLf, "")

            End If

            params_ = _ctrlCube.interpreter.GetParams(formula_)

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

                                               catalogRow_.SetIndice(cc_ValoresOperandos.KeyField, cuenta_)

                                               Dim operandName_ As New InputControl With {.ID = "operandName_",
                                                                                       .Value = valor_.Key,
                                                                                       .Type = InputControl.InputType.Text}

                                               Dim operandValue_ As New InputControl With {.ID = "operandValue_",
                                                                                       .Value = valor_.Value,
                                                                                       .Type = InputControl.InputType.Text}

                                               catalogRow_.SetColumn(operandName_, valor_.Key)

                                               catalogRow_.SetColumn(operandValue_, valor_.Value)

                                               'de esta manera agregamos todas las columnas de nuestra fila 
                                               'usando el control asociado a la columna y el valor que se asignara

                                           End Sub)

                cuenta_ += 1

            Next

            cc_ValoresOperandos.CatalogDataBinding()

            Dim resultado_ = _ctrlCube.interpreter.RunExpression(Of Object)(formula_, Values_)

            If TypeOf resultado_ Is Dictionary(Of String, List(Of String)) Then

                Dim stringFinal_ = ""

                For Each element_ In resultado_.Keys

                    Dim stringJoin_ = ""

                    For Each elementList In resultado_(element_)

                        stringJoin_ &= elementList & ","

                    Next

                    stringFinal_ &= "[" & element_ & "==" & stringJoin_.Substring(0, stringJoin_.Length - 1) & "],"

                Next

                SetVars("resultTest_", "BADBAD")

                Dim cubeReport_ = _ctrlCube.interpreter.GetReportFull

                If cubeReport_.title <> "" Then

                    cubeReport_.ShowMessageError(2, Me)

                Else

                    DisplayMessage(stringFinal_, TypeStatus.OkInfo)

                End If


            Else

                If TypeOf resultado_ Is Dictionary(Of String, String) Then

                    Dim stringFinal_ = ""

                    For Each element_ In resultado_.Keys

                        stringFinal_ &= "[" & element_ & "==" & resultado_(element_) & "],"

                    Next

                    SetVars("resultTest_", "BADBAD")

                    Dim interpreterReport_ = _ctrlCube.interpreter.GetReportFull

                    If interpreterReport_.title <> "" Then

                        interpreterReport_.ShowMessageError(2, Me)

                    Else

                        DisplayMessage(stringFinal_, TypeStatus.OkInfo)

                    End If

                Else

                    If TypeOf resultado_ IsNot List(Of String) Then

                        If TypeOf resultado_ IsNot List(Of List(Of String)) Then


                            Dim messageResult_ As String = resultado_

                            Select Case resultado_.ToString

                                Case "OK"

                                    Dim informationResult_ = DirectCast(fsc_Información.ListControls(0), InputControl).Value

                                    If informationResult_ <> "" Then

                                        messageResult_ = informationResult_


                                    End If

                                Case "BAD"

                                    Dim alertResult_ = DirectCast(fsc_Alertas.ListControls(0), InputControl).Value

                                    If alertResult_ <> "" Then

                                        messageResult_ = alertResult_

                                    End If

                                Case Else

                                    messageResult_ = resultado_

                            End Select

                            For Each key_ In Values_.Keys

                                messageResult_ = messageResult_.Replace("$" & key_, Values_(key_))

                                Dim pointPos_ = key_.LastIndexOf(".")

                                If pointPos_ <> -1 Then

                                    messageResult_ = messageResult_.Replace("$" & key_.Substring(0, pointPos_), Values_(key_))

                                End If

                            Next

                            SetVars("resultTest_", resultado_)

                            Dim reportInterpreter_ = _ctrlCube.interpreter.GetReportFull

                            If reportInterpreter_.title <> "" Then

                                reportInterpreter_.ShowMessageError(2, Me)

                            Else

                                DisplayMessage(messageResult_.ToString, TypeStatus.OkInfo)

                            End If

                        Else

                            Dim listValue_ = resultado_(0)

                            Dim listText_ = resultado_(1)

                            Dim index_ = 0

                            Dim stringFinal_ = ""

                            For Each element_ In listValue_

                                stringFinal_ &= "[" & element_ & "==" & listText_(index_) & "],"

                                index_ += 1

                            Next

                            Dim interpreterReport_ = _ctrlCube.interpreter.GetReportFull

                            If interpreterReport_.title <> "" Then

                                interpreterReport_.ShowMessageError(2, Me)

                            Else

                                DisplayMessage(stringFinal_, TypeStatus.OkInfo)

                            End If

                        End If

                    Else

                        Dim stringFinal_ = ""

                        For Each element_ In resultado_

                            stringFinal_ &= element_ & ","
                        Next

                        SetVars("resultTest_", "BADBAD")

                        Dim interpreterReport = _ctrlCube.interpreter.GetReportFull

                        If interpreterReport.title <> "" Then

                            interpreterReport.ShowMessageError(2, Me)

                        Else

                            DisplayMessage(stringFinal_, TypeStatus.OkInfo)

                        End If

                    End If

                End If

            End If

            Dim report_ = _ctrlCube.interpreter.GetReportFull

            If report_.GetTitle = "" Then

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

                    bc_Verificado.Enabled = False

                    bc_VerificadoNueva.Enabled = False

                    SetVars("_bc_verificado", "NO")

                End If

            Else

                bc_Verificado.Enabled = False

                bc_VerificadoNueva.Enabled = False

                SetVars("_bc_verificado", "NO")

            End If

            bi_SolicitarAutorizacion.Visible = Not bi_SudoAutorizar.Visible

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
        '  MsgBox(tb_FormulaNueva.Text)

        If formulaFormato_ <> "" AndAlso
            (tb_Formula.Enabled OrElse
           (formulaNuevaFormato_ <> "" AndAlso
           tb_FormulaNueva.Enabled)) Then

            _ctrlCube = GetVars("_cubeController")

            If _ctrlCube Is Nothing Then

                _ctrlCube = New CubeController


            End If

            Dim Values_ = New Dictionary(Of String, Object)

            Dim icontroladorMonedas_ As IControladorMonedas = New ControladorMonedas

            Dim params_ As List(Of String)

            Dim fomula_ = formulaFormato_

            Dim botonformula_ As ButtonControl

            If tb_Formula.Enabled Then

                params_ = _ctrlCube.interpreter.GetParams(formulaFormato_)

                botonformula_ = bc_Function

            Else

                params_ = _ctrlCube.interpreter.GetParams(formulaNuevaFormato_)

                fomula_ = formulaNuevaFormato_

                botonformula_ = bc_FunctionChange

            End If

            If _ctrlCube.interpreter.ExistFields(fomula_, params_) Then


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



                                                   Dim operandName_ As New InputControl With {.ID = "operandName_",
                                                                                           .Value = valor_.Key,
                                                                                           .Type = InputControl.InputType.Text}

                                                   Dim operandValue_ As New InputControl With {.ID = "operandValue_",
                                                                                           .Value = valor_.Value,
                                                                                           .Type = InputControl.InputType.Text}



                                                   catalogRow_.SetColumn(operandName_, valor_.Key)

                                                   catalogRow_.SetColumn(operandValue_, valor_.Value)


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

                Dim reportCabulidad_ = _ctrlCube.interpreter.GetReportFull

                reportCabulidad_.ShowMessageError(2, Me)

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

        fscformulas.Enabled = True

        ic_RoomName.Visible = False

        fbc_RoomName.Visible = True

        fscinformacion.Enabled = True

        fscProbarFormulas.Enabled = True

        p_actualizacionformula.Visible = False

        p_FormulaActual.Enabled = True

        tb_Formula.Enabled = True

        bc_ElaborarPrueba.Enabled = False

        bc_ElaborarPrueba.Visible = True

        bc_LimpiarFormula.Visible = True

        fscProbarFormulas.Visible = False

        bc_LimpiarFormula.Enabled = True

        p_formulillas.Enabled = True

        fbc_RoomName.Enabled = True

        bc_Function.Enabled = True

        bc_Var.Enabled = True

        bc_SourceCube.Enabled = True

        fbc_RoomName.Enabled = True

        scUseType.Value = 1

        SetVars("_idRoom", Nothing)

    End Sub

    Sub CargaEncontradolModulo()

        PreparaBotonera(FormControl.ButtonbarModality.Reading)

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

        ic_RoomNameNew.Enabled = False

        bc_SourceCubeChange.Label = GetVars("_cubeSource")

        ColocaHistorial(GetVars("_historical"))

    End Sub


    Sub ColocaAutorizar()

        Dim bc_PendienteAutorizacion_ As ButtonControl

        If tb_Formula.Enabled Then

            bc_PendienteAutorizacion_ = bc_PorAutorizar

        Else

            bc_PendienteAutorizacion_ = bc_PorAutorizarNueva

        End If

        bi_SudoAutorizar.Visible = bc_PendienteAutorizacion_.Enabled

        bi_SudoDesechar.Visible = bc_PendienteAutorizacion_.Enabled

        bi_SolicitarAutorizacion.Visible = Not bi_SudoAutorizar.Visible

        ChecaBotonComparar()

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

        If sc_BranchNames.Value <> "8" Then

            bc_SourceCube.Label = sc_BranchNames.Text

            bc_SourceCube.Value = sc_BranchNames.Text

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

    Sub Fbc_RoomName_TextChanged(sender As Object, e As EventArgs)

        Dim lista_ As List(Of SelectOption)

        '        lista_ = _organismo.ObtenerSelectOption(_ctrlCube.fieldmiss.FindAll(_organismo.GetPredicates(sender.Text.ToString.ToUpper)))

        lista_ = _organismo.ObtenerSelectOption(_ctrlCube.GetValidFieldsOn(sender.Text.ToString.ToUpper).ObjectReturned)

        If lista_ Is Nothing Then

            lista_ = New List(Of SelectOption)
        End If


        sender.DataSource = lista_

        sender.Label = ""

    End Sub



    Sub ChecaBotonComparar()

        Dim comparar_ As Int16 = GetVars("_bcComparar")

        ' MsgBox("Comparar:" & comparar_)

        Select Case comparar_

            Case 1

                bi_Comparar.Visible = False

                bi_QuitarComparar.Visible = False

            Case 2

                bi_Comparar.Visible = True

                bi_QuitarComparar.Visible = False


            Case 3

                bi_Comparar.Visible = False

                bi_QuitarComparar.Visible = True


            Case Else

                bi_Comparar.Visible = False

                bi_QuitarComparar.Visible = False

                SetVars("_bcComparar", 1)

        End Select

        'For Each button_ In __SYSTEM_MODULE_FORM.Buttonbar.DropdownButtons

        '    If button_.ID = bi_Comparar.ID Then

        '        button_ = bi_Comparar

        '    Else

        '        If button_.ID = bi_QuitarComparar.ID Then

        '            button_ = bi_QuitarComparar

        '        End If

        '    End If

        'Next

        'For Each contol_ In __SYSTEM_MODULE_FORM.Fieldsets

        '    contol_.

        '    MsgBox(contol_.GetType.ToString)

        'Next


    End Sub

    Function CrearListaMensages() As List(Of String)

        Return New List(Of String) From
               {DirectCast(fsc_Alertas.ListControls(0), InputControl).Value,
               DirectCast(fsc_Advertencias.ListControls(0), InputControl).Value,
               DirectCast(fsc_Información.ListControls(0), InputControl).Value}

    End Function

    Sub ActualizarSlice(sender_ As Object, e As EventArgs)

        Dim cubeSlice_ As ICubeController.CubeSlices

        If [Enum].TryParse(Of ICubeController.CubeSlices)(sc_BranchNames.Text, cubeSlice_) Then

            If bc_SourceCube.Visible Then

                bc_SourceCube.Value = sc_BranchNames.Text

                bc_SourceCube.Label = sc_BranchNames.Text

                SetVars("_cubeSource", bc_SourceCube.Value)

            Else

                bc_SourceCubeChange.Value = sc_BranchNames.Text

                bc_SourceCubeChange.Label = sc_BranchNames.Text

                SetVars("_cubeSource", bc_SourceCubeChange.Value)

            End If

        Else

            Dim cubeSource_ As String = GetVars("_cubeSource")

            If [Enum].TryParse(Of ICubeController.CubeSlices)(cubeSource_, cubeSlice_) Then

                sc_BranchNames.Value = cubeSlice_

            End If


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